using DevilHome.Database.Interfaces.Values;

namespace DevilHome.CoreServer.Controllers
{
    public class ConfigurationGetter : ControllersBase
    {
        private string GetValueValue(string code)
        {
            ISystemConfigurationValue value = DbSystemConfiguration.GetValueByCode(code);
            return value.Value;
        }

        
    }
}