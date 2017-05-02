using System;
using DevilHome.Common.Implementations.Base;
using DevilHome.Common.Interfaces.Values;

namespace DevilHome.Common.Implementations.Values
{
    public class SensorDataValue : BaseValue, ISensorDataValue
    {
        public int Fk_Sensor_Id { get; set; }
        public double Value { get; set; }
        public DateTime Date { get; set; }
    }
}