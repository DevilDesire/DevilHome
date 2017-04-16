using DevilHome.Common.Implementations.Base;
using DevilHome.Common.Interfaces.Values;

namespace DevilHome.Common.Implementations.Values
{
    public class PoweroutletValue : BaseValue, IPoweroutletValue
    {
        public int Fk_Raum_Id { get; set; }
        public string HausCode { get; set; }
        public string DeviceCode { get; set; }
    }
}