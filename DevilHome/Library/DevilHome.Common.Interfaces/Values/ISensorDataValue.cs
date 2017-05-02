using System;
using DevilHome.Common.Interfaces.Base;

namespace DevilHome.Common.Interfaces.Values
{
    public interface ISensorDataValue : IBaseValue
    {
        int Fk_Sensor_Id { get; set; }
        double Value { get; set; }
        DateTime Date { get; set; }
    }
}