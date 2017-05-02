using System.Collections.Generic;
using DevilHome.Database.Implementations.Engine;
using DevilHome.Database.Implementations.Values;
using DevilHome.Database.Interfaces.Values;

namespace DevilHome.Database.Implementations.Tables
{
    public class DbSensor : DatabaseEngineBase
    {
        public List<ISensor> GetAllValues()
        {
            return new List<ISensor>(GetAllValues<Sensor>());
        }

        public ISensor GetValueById(int id)
        {
            return GetValueById<Sensor>(id);
        }

        public void Insert(Sensor sensor)
        {
            Insert<Sensor>(sensor);
        }

        public int GetIdByName(string typeName)
        {
            Sensor responseValue = GetValueByName<Sensor>(typeName);
            return responseValue.Id;
        }

        public int GetIdBySensorTypNameRoomName(string sensorTypeName, string roomName)
        {
            Sensor responeValue = GetIdBySensorTypNameRoomName<Sensor>(sensorTypeName, roomName);
            return responeValue.Id;
        }
    }
}