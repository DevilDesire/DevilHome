using DevilHome.Database.Interfaces.Base;
using DevilHome.Database.Interfaces.Values;

namespace DevilHome.Database.Interfaces.Tables
{
    public interface IDbSystemConfiguration : IDbBaseTable<ISystemConfigurationValue>
    {
        ISystemConfigurationValue GetValueByCode(string code);
    }
}