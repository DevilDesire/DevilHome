using DevilHome.Database.Implementations.Base;
using DevilHome.Database.Interfaces.Values;

namespace DevilHome.Database.Implementations.Values
{
    public class Sensor : BaseValue, ISensor
    {
        public int Fk_SensorTyp_Id { get; set; }
        public int Fk_Raum_Id { get; set; }
    }
}