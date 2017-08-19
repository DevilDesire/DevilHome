using DevilHome.Database.Interfaces.Base;

namespace DevilHome.Database.Interfaces.Values
{
    public interface ISystemConfigurationValue : IDatabaseValue
    {
        string Code { get; set; }
        string Value { get; set; }
    }
}