using System;
using DevilHome.Database.Implementations.Connection;

namespace DevilHome.CoreServer.Controllers.Database
{
    internal class DatabaseBase : ControllersBase, IDatabaseBase
    {
        public async void InitDatabase()
        {
            try
            {
                await Connection.InitializeDatabase();
            }
            catch (Exception ex)
            {
                //await Logger.LogError(ex, PluginEnum.Database);
            }
        }

        public void GetSensorDataBySensorId(int sensorId, int valueCount = 0)
        {
            DbSensorData.GetValuesBySensorId(sensorId);
        }
    }
}