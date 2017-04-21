using DevilHome.Common.Interfaces.Extensions;

namespace DevilHome.Common.Interfaces.Enums
{
    public enum QueryType
    {
        [StringValue("radio")]
        Radio,
        [StringValue("power")]
        Power,
        [StringValue("sensor")]
        Sensor,
        [StringValue("error")]
        Error
    }
}