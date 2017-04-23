using System.Collections.Generic;
using DevilHome.Database.Implementations.Engine;
using DevilHome.Database.Implementations.Values;
using DevilHome.Database.Interfaces.Values;

namespace DevilHome.Database.Implementations.Tables
{
    public class DbSensor : DatabaseEngineBase
    {
        public List<ISensorValue> GetAllValues()
        {
            return new List<ISensorValue>(GetAllValues<SensorValue>());
        }

        public ISensorValue GetValueById(int id)
        {
            return GetValueById<SensorValue>(id);
        }
    }
}