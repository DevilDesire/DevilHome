using DevilHome.Database.Interfaces.Base;

namespace DevilHome.Database.Interfaces.Values
{
    public interface IDevicegroupValue : IBaseValue
    {
        int Fk_Poweroutlet_Id { get; set; }
    }
}