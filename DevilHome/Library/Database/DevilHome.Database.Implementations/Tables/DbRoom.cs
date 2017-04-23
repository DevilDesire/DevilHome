using System.Collections.Generic;
using DevilHome.Database.Implementations.Engine;
using DevilHome.Database.Implementations.Values;
using DevilHome.Database.Interfaces.Values;

namespace DevilHome.Database.Implementations.Tables
{
    public class DbRoom : DatabaseEngineBase
    {
        public List<IRoomValue> GetAllValues()
        {
            return new List<IRoomValue>(GetAllValues<RoomValue>());
        }
        
        public IRoomValue GetValueById(int id)
        {
            return GetValueById<RoomValue>(id);
        }
    }
}