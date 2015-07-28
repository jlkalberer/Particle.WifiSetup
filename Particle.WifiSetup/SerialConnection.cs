namespace Particle.WifiSetup
{
    using System;
    using System.Configuration;
    using System.IO.Ports;
    using System.Runtime.InteropServices;

    public class SerialConnection
    {
        /// <summary>
        /// The baud rate.
        /// </summary>
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

        /// <summary>
        /// Initializes static members of the <see cref="SerialConnection"/> class.
        /// </summary>
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
        /// The install <c>hinf</c> section.
        /// </summary>
        /// <param name="hwnd">
        /// The <c>hwnd</c>.
        /// </param>
        /// <param name="moduleHandle">
        /// The module handle.
        /// </param>
        /// <param name="cmdLineBuffer">
        /// The 
        /// <c>CMD</c> line buffer.
        /// </param>
        /// <param name="cmdShow">
        /// The CMD show.
        /// </param>
        [DllImport("Setupapi.dll", EntryPoint = "InstallHinfSection", CallingConvention = CallingConvention.StdCall)]
        public static extern void InstallHinfSection(
            [In] IntPtr hwnd,
            [In] IntPtr moduleHandle,
            [In, MarshalAs(UnmanagedType.LPWStr)] string cmdLineBuffer,
            int cmdShow);

        /// <summary>
        /// The setup <c>WIFI</c>.
        /// </summary>
        /// <param name="security">
        /// The security.
        /// </param>
        /// <param name="ssid">
        /// The SSID.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool SetupWifi(WifiSecurity security, string ssid, string password)
        {
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
