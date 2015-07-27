namespace Particle.WifiSetup
{
    using System.ComponentModel;

    public enum WifiSecurity
    {
        [Description("Unsecured")]
        Unsecured = 0,
        [Description("WEP")]
        WEP = 1,
        [Description("WPA")]
        WPA = 2,
        [Description("WPA2")]
        WPA2 = 3,
    }
}
