using DevilHome.Database.Implementations.Engine;
using DevilHome.Database.Implementations.Values;
using DevilHome.Database.Interfaces.Values;

namespace DevilHome.Database.Implementations.Tables
{
    public class DbSensorTyp : DatabaseEngineBase
    {
        public void Insert(SensorTyp sensorTyp)
        {
            Insert<SensorTyp>(sensorTyp);
        }

        public int GetIdByName(string typeName)
        {
            SensorTyp responseValue = GetValueByName<SensorTyp>(typeName);
            return responseValue.Id;
        }
    }
}