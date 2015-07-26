namespace Particle.WifiSetup
{
    using System.ComponentModel;

    public enum WifiSecurity
    {
        [Description("Unsecured")]
        Unsecured = 0,
        [Description("WEP")]
        WEP,
        [Description("WPA")]
        WPA,
        [Description("WPA2")]
        WPA2
    }
}
