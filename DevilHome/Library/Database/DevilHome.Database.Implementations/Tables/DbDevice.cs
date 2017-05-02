using DevilHome.Database.Implementations.Engine;
using DevilHome.Database.Implementations.Values;

namespace DevilHome.Database.Implementations.Tables
{
    public class DbDevice : DatabaseEngineBase
    {
        public void Insert(Device device)
        {
            Insert<Device>(device);
        }
    }
}