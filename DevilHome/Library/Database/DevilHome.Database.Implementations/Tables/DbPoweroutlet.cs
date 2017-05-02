using DevilHome.Database.Implementations.Engine;
using DevilHome.Database.Implementations.Values;

namespace DevilHome.Database.Implementations.Tables
{
    public class DbPoweroutlet : DatabaseEngineBase
    {
        public void Insert(Poweroutlet poweroutlet)
        {
            Insert<Poweroutlet>(poweroutlet);
        }
    }
}