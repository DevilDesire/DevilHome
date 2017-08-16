using System.Collections.Generic;
using DevilHome.Database.Interfaces.Base;
using DevilHome.Database.Interfaces.Values;

namespace DevilHome.Database.Interfaces.Tables
{
    public interface IDbPoweroutlet : IDbBaseTable<IPoweroutlet>
    {
        List<IPoweroutlet> GetValuesByRoomId(int roomId);
    }
}