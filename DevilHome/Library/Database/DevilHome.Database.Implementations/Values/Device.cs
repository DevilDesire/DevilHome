using DevilHome.Database.Implementations.Base;
using DevilHome.Database.Interfaces.Values;

namespace DevilHome.Database.Implementations.Values
{
    public class Device : BaseValue, IDevice
    {
        public int Fk_Devicegroup_Id { get; set; }
    }
}