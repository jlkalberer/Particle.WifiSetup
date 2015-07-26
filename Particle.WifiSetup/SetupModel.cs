namespace Particle.WifiSetup
{
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
        }

        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        public RelayCommand Command { get; set; }

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

            var v = new SerialConnection("COM6");
            v.SetupWifi(this.WifiSecurity, this.SSID, password);
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
    }
}
