using DevilHome.Database.Interfaces.Base;

namespace DevilHome.Database.Interfaces.Values
{
    public interface ISensorValue : IBaseValue
    {
        int Fk_SensorTyp_Id { get; set; }
        int Fk_Raum_Id { get; set; }
        decimal Value { get; set; }
    }
}