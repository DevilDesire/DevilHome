using DevilHome.Common.Implementations.Base;
using DevilHome.Common.Interfaces.Values;

namespace DevilHome.Common.Implementations.Values
{
    public class DevicegroupValue : BaseValue, IDevicegroupValue
    {
        public int Fk_Poweroutlet_Id { get; set; }
    }
}