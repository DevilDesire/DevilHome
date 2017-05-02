using DevilHome.Database.Interfaces.Base;

namespace DevilHome.Database.Interfaces.Values
{
    public interface ISensorData : IDatabaseValue
    {
        int Fk_Sensor_Id { get; set; }
        double Value { get; set; }
    }
}