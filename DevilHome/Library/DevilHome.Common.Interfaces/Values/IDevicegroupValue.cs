using DevilHome.Common.Interfaces.Base;

namespace DevilHome.Common.Interfaces.Values
{
    public interface IDevicegroupValue : IBaseValue
    {
        int Fk_Poweroutlet_Id { get; set; }
    }
}