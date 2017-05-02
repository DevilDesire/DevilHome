using DevilHome.Database.Implementations.Engine;
using DevilHome.Database.Implementations.Values;

namespace DevilHome.Database.Implementations.Tables
{
    public class DbDevicegroup : DatabaseEngineBase
    {
        public void Insert(Devicegroup devicegroup)
        {
            Insert<Devicegroup>(devicegroup);
        }
    }
}