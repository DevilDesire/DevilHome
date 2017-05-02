using DevilHome.Common.Implementations.Base;
using DevilHome.Common.Interfaces.Values;

namespace DevilHome.Common.Implementations.Values
{
    public class SensorValue : BaseValue, ISensorValue
    {
        public int Fk_SensorTyp_Id { get; set; }
        public int Fk_Raum_Id { get; set; }
        public double LastValue { get; set; }
    }
}