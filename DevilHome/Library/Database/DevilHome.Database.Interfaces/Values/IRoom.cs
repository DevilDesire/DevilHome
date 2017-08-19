using DevilHome.Database.Interfaces.Base;

namespace DevilHome.Database.Interfaces.Values
{
    public interface IRoom : IDatabaseValue
    {
        string Name { get; set; }
    }
}