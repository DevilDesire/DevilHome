using DevilHome.Database.Interfaces.Base;

namespace DevilHome.Database.Interfaces.Values
{
    public interface IPoweroutlet : IBaseValue
    {
        int Fk_Raum_Id { get; set;}
        string HausCode { get; set; }
        string DeviceCode { get; set; }
        string Datum { get; set; }
    }
}