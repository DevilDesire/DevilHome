using DevilHome.Database.Implementations.Base;
using DevilHome.Database.Interfaces.Values;

namespace DevilHome.Database.Implementations.Values
{
    public class Devicegroup : BaseValue, IDevicegroup
    {
        public int Fk_Poweroutlet_Id { get; set; }
    }
}