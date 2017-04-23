using DevilHome.Database.Interfaces.Base;

namespace DevilHome.Database.Interfaces.Values
{
    public interface IDeviceValue : IBaseValue
    {
        int Fk_Devicegroup_Id { get; set; }
    }
}