using System.Collections.Generic;
using System.Linq;
using DevilHome.Database.Implementations.Base;
using DevilHome.Database.Implementations.Values;
using DevilHome.Database.Interfaces.Tables;
using DevilHome.Database.Interfaces.Values;

namespace DevilHome.Database.Implementations.Tables
{
    public class DbSensorData : DbBaseTable<SensorData, ISensorData>, IDbSensorData
    {
        public List<ISensorData> GetValuesBySensorId(int id)
        {
            return GetValuesByFkNameAndId("Fk_Sensor_Id", id);
        }

        public double GetLastValueBySensorId(int id)
        {
            return GetValuesByFkNameAndId("Fk_Sensor_Id", id, 1).First().Value;
        }
    }
}