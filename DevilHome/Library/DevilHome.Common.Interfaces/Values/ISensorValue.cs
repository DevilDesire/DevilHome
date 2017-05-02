using DevilHome.Common.Interfaces.Base;

namespace DevilHome.Common.Interfaces.Values
{
    public interface ISensorValue : IBaseValue
    {
        int Fk_SensorTyp_Id { get; set; }
        int Fk_Raum_Id { get; set; }
        double LastValue { get; set; }
    }
}