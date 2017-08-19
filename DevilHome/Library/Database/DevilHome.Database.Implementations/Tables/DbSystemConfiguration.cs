using DevilHome.Database.Implementations.Base;
using DevilHome.Database.Implementations.Values;
using DevilHome.Database.Interfaces.Tables;
using DevilHome.Database.Interfaces.Values;

namespace DevilHome.Database.Implementations.Tables
{
    public class DbSystemConfiguration : DbBaseTable<SystemConfigurationValue, ISystemConfigurationValue>, IDbSystemConfiguration
    {
        public ISystemConfigurationValue GetValueByCode(string code)
        {
            return ExecuteScalar($"select * from DbSystemConfiguration where Code = '{code}'");
        }
    }
}