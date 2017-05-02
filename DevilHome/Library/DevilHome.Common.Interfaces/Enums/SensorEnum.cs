using DevilHome.Common.Interfaces.Extensions;

namespace DevilHome.Common.Interfaces.Enums
{
    public enum SensorEnum
    {
        [StringValue("Temperature")]
        Temperatur = 1,
        [StringValue("Humidity")]
        Humidity = 2
    }
}