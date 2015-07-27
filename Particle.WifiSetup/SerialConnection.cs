using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Particle.WifiSetup
{
    using System.Configuration;
    using System.Diagnostics;
    using System.IO.Ports;

    public class SerialConnection
    {
        private static readonly int BaudRate;

        /// <summary>
        /// The result.
        /// </summary>
        private byte[] result = null;

        /// <summary>
        /// The completed.
        /// </summary>
        private bool completed = false;

        /// <summary>
        /// The port.
        /// </summary>
        private SerialPort port;

        private int counter;

        static SerialConnection()
        {
            var baudString = ConfigurationManager.AppSettings["BaudRate"];
            var baudRate = 115200;
            if (!string.IsNullOrEmpty(baudString))
            {
                int.TryParse(baudString, out baudRate);
            }

            BaudRate = baudRate;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerialConnection"/> class.
        /// </summary>
        /// <param name="comPort">
        /// The com port.
        /// </param>
        public SerialConnection(string comPort)
        {
            this.port = new SerialPort(comPort, BaudRate, Parity.None, 8, StopBits.One);
            this.port.DataReceived += this.PortOnDataReceived;
        }

        /// <summary>
        /// The setup <c>WIFI</c>.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool SetupWifi(WifiSecurity security, string ssid, string password)
        {
            this.counter = 0;

            this.port.Open();

            if (!this.port.IsOpen)
            {
                this.port.Close();
                return false;
            }

            var connectionString = string.Format("{0}\0{1}\0{2}\0", Convert.ToChar(security), ssid, password);

            this.port.Write(connectionString);

            while (!this.completed)
            {
            }

            this.port.Close();
            return true;
        }

        /// <summary>
        /// The port on data received.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="serialDataReceivedEventArgs">
        /// The serial data received event args.
        /// </param>
        private void PortOnDataReceived(object sender, SerialDataReceivedEventArgs serialDataReceivedEventArgs)
        {
            this.result = new byte[this.port.BytesToRead];
            this.port.Read(this.result, 0, this.port.BytesToRead);

            this.completed = true;
        }
    }
}
