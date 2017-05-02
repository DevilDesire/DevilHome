using DevilHome.Database.Interfaces.Base;

namespace DevilHome.Database.Interfaces.Values
{
    public interface IDevicegroup : IBaseValue
    {
        int Fk_Poweroutlet_Id { get; set; }
    }
}