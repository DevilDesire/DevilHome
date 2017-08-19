using DevilHome.Database.Implementations.Base;
using DevilHome.Database.Interfaces.Values;

namespace DevilHome.Database.Implementations.Values
{
    public class Room : DatabaseValue, IRoom
    {
        public string Name { get; set; }
    }
}