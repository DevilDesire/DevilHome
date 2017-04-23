using DevilHome.Database.Implementations.Base;
using DevilHome.Database.Interfaces.Values;

namespace DevilHome.Database.Implementations.Values
{
    public class DevicegroupValue : BaseValue, IDevicegroupValue
    {
        public int Fk_Poweroutlet_Id { get; set; }
    }
}