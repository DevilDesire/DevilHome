using System.Collections.Generic;
using DevilHome.Database.Implementations.Base;
using DevilHome.Database.Implementations.Values;
using DevilHome.Database.Interfaces.Tables;
using DevilHome.Database.Interfaces.Values;

namespace DevilHome.Database.Implementations.Tables
{
    public class DbPoweroutlet : DbBaseTable<Poweroutlet, IPoweroutlet>, IDbPoweroutlet
    {
        public List<IPoweroutlet> GetValuesByRoomId(int roomId)
        {
            return GetValuesByFkNameAndId("Fk_Raum_Id", roomId);
        }
    }
}