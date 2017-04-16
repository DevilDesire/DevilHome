using DevilHome.Common.Interfaces.Base;

namespace DevilHome.Common.Interfaces.Values
{
    public interface IDeviceValue : IBaseValue
    {
        int Fk_Devicegroup_Id { get; set; }
    }
}