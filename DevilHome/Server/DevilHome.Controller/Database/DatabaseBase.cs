using System;
using DevilHome.Controller.Utils;
using DevilHome.Database.Implementations.Connection;
using DevilHome.Database.Implementations.Tables;
using DevilHome.Database.Implementations.Values;

namespace DevilHome.Controller.Database
{
    internal class DatabaseBase : ControllerBase, IDatabaseBase
    {
        public async void InitDatabase()
        {
            try
            {
                await Connection.InitializeDatabase();
            }
            catch (Exception ex)
            {
                await Logger.LogError(ex, PluginEnum.Database);
            }
        }

        public void InsertSensorValue(string sensorType, string roomName, string sensorName, double sensorValue)
        {
            int sensorId = new DbSensor().GetIdBySensorTypNameRoomName(sensorType, roomName);

            DbSensorData.Insert(new SensorData
            {
                Value = sensorValue,
                Fk_Sensor_Id = sensorId
            });
        }

        public void GetSensorDataBySensorId(int sensorId, int valueCount = 0)
        {
            DbSensorData.GetValuesBySensorId(sensorId);
        }
    }
}