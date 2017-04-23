using DevilHome.Database.Implementations.Base;
using DevilHome.Database.Interfaces.Values;

namespace DevilHome.Database.Implementations.Values
{
    public class PoweroutletValue : BaseValue, IPoweroutletValue
    {
        public int Fk_Raum_Id { get; set; }
        public string HausCode { get; set; }
        public string DeviceCode { get; set; }
    }
}