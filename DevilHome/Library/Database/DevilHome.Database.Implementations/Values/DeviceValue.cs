using DevilHome.Database.Implementations.Base;
using DevilHome.Database.Interfaces.Values;

namespace DevilHome.Database.Implementations.Values
{
    public class DeviceValue : BaseValue, IDeviceValue
    {
        public int Fk_Devicegroup_Id { get; set; }
    }
}