using DevilHome.Database.Implementations.Base;
using DevilHome.Database.Interfaces.Values;

namespace DevilHome.Database.Implementations.Values
{
    public class Poweroutlet : DatabaseValue, IPoweroutlet
    {
        public string Name { get; set; }
        public int Fk_Raum_Id { get; set; }
        public string HausCode { get; set; }
        public string DeviceCode { get; set; }
        public bool Secure { get; set; }
    }
}