using DevilHome.Database.Interfaces.Base;

namespace DevilHome.Database.Interfaces.Values
{
    public interface IPoweroutlet : IDatabaseValue
    {
        string Name { get; set; }
        int Fk_Raum_Id { get; set;}
        string HausCode { get; set; }
        string DeviceCode { get; set; }
    }
}