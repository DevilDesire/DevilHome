using DevilHome.Database.Implementations.Base;
using DevilHome.Database.Interfaces.Values;

namespace DevilHome.Database.Implementations.Values
{
    public class SystemConfigurationValue : DatabaseValue, ISystemConfigurationValue
    {
        public string Code { get; set; }
        public string Value { get; set; }
    }
}