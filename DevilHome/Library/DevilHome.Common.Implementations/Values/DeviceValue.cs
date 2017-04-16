using DevilHome.Common.Implementations.Base;
using DevilHome.Common.Interfaces.Values;

namespace DevilHome.Common.Implementations.Values
{
    public class DeviceValue : BaseValue, IDeviceValue
    {
        public int Fk_Devicegroup_Id { get; set; }
    }
}