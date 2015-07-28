namespace Particle.WifiSetup
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.IO.Ports;
    using System.Linq;
    using System.Management;
    using System.Windows.Controls;

    /// <summary>
    /// The setup model.
    /// </summary>
    public class SetupModel : ViewModelBase
    {
        /// <summary>
        /// The ssid.
        /// </summary>
        private string ssid;

        /// <summary>
        /// The wifi security.
        /// </summary>
        private WifiSecurity wifiSecurity;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetupModel"/> class.
        /// </summary>
        public SetupModel()
        {
            this.WifiSecurity = WifiSecurity.WPA2;
            this.SubmitEnabled = false;
            this.Command = new RelayCommand(this.Execute, this.CanExecute);
            this.InstallDriversCommand = new RelayCommand(this.InstallDrivers);
        }
        
        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        public RelayCommand Command { get; set; }

        /// <summary>
        /// Gets or sets the install drivers command.
        /// </summary>
        public RelayCommand InstallDriversCommand { get; set; }

        /// <summary>
        /// Gets or sets the SSID.
        /// </summary>
        public string SSID
        {
            get
            {
                return this.ssid;
            }

            set
            {
                this.ssid = value;
                this.RaisePropertyChanged("SSID");
            }
        }

        /// <summary>
        /// Gets or sets the wifi security.
        /// </summary>
        public WifiSecurity WifiSecurity
        {
            get
            {
                return this.wifiSecurity;
            }

            set
            {
                this.wifiSecurity = value;
                this.RaisePropertyChanged("WifiSecurity");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether submit enabled.
        /// </summary>
        public bool SubmitEnabled { get; set; }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        public void Execute(object parameter)
        {
            var passwordBox = parameter as PasswordBox;

            if (passwordBox == null)
            {
                return;
            }
            
            var password = passwordBox.Password;

            IList<string> portList;
            string[] portNames;
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Caption like '%(COM%'"))
            {
                portNames = SerialPort.GetPortNames();
                portList = searcher.Get().Cast<ManagementBaseObject>()
                    .Select(p => p["Caption"].ToString())
                    .ToList();
            }

            var item = portList.FirstOrDefault(p => p.Contains("Photon") || p.Contains("Spark"));
            var itemIndex = portList.IndexOf(item);
            if (itemIndex < 0)
            {
                return;
            }


            var serialConnection = new SerialConnection(portNames[itemIndex].Substring(0, 4));
            serialConnection.SetupWifi(this.WifiSecurity, this.SSID, password);
        }

        /// <summary>
        /// The can execute.
        /// </summary>
        /// <param name="parameter">
        /// The parameter.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            var passwordBox = parameter as PasswordBox;

            if (passwordBox == null)
            {
                return false;
            }

            var password = passwordBox.Password;

            return !string.IsNullOrEmpty(this.SSID) && !string.IsNullOrEmpty(password);
        }

        /// <summary>
        /// The install drivers.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        private void InstallDrivers(object obj)
        {
            var infName = ConfigurationManager.AppSettings["DriverName"];

            if (string.IsNullOrEmpty(infName))
            {
                return;
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), infName);

            SerialConnection.InstallHinfSection(IntPtr.Zero, IntPtr.Zero, string.Format("DefaultInstall 132 {0}", path), 0);
        }
    }
}
