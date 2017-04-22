using DevilHome.Common.Interfaces.Extensions;

namespace DevilHome.Controller.Utils
{
    public enum PluginEnum
    {
        [StringValue("Controller")]
        Controller,
        [StringValue("Database")]
        Database,
        [StringValue("TemperatureController")]
        TemperatureController,
        [StringValue("WirelessPowerSwitchController")]
        WirelessPowerSwitchController
    }
}