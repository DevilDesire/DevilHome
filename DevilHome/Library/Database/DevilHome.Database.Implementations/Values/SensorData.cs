using DevilHome.Database.Implementations.Base;
using DevilHome.Database.Interfaces.Values;

namespace DevilHome.Database.Implementations.Values
{
    public class SensorData : DatabaseValue, ISensorData
    {
        public int Fk_Sensor_Id { get; set; }
        public double Value { get; set; }
    }
}