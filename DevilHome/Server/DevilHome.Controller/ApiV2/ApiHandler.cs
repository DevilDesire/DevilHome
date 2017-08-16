using System;
using System.Threading.Tasks;
using DevilHome.Common.Interfaces.Enums;
using DevilHome.Common.Interfaces.Values;
using DevilHome.Controller.ApiV2.Poweroutlet;
using DevilHome.Controller.TemperatureController;
using DevilHome.Controller.Utils;

namespace DevilHome.Controller.ApiV2
{
    internal class ApiHandler : ControllerBase
    {
        public async Task<string> HandleGet(IQueryValue queryValue)
        {
            try
            {
                switch (queryValue.QueryType)
                {
                    case QueryType.Power:
                        return await new PoweroutletGetter().ProcessingGetRequest(queryValue);
                    case QueryType.Sensor:
                        return await new Temperature().ProcessingGetRequest(queryValue);
                    default:
                        return "";
                }
            }
            catch (Exception ex)
            {
                await Logger.LogError(ex, PluginEnum.Controller);
            }

            return "";
        }
    }
}