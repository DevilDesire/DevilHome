using System.Collections.Generic;
using DevilHome.Database.Implementations.Engine;
using DevilHome.Database.Implementations.Values;
using DevilHome.Database.Interfaces.Values;

namespace DevilHome.Database.Implementations.Tables
{
    public class DbRoom : DatabaseEngineBase
    {
        public List<IRoom> GetAllValues()
        {
            return new List<IRoom>(GetAllValues<Room>());
        }
        
        public IRoom GetValueById(int id)
        {
            return GetValueById<Room>(id);
        }

        public void Insert(Room room) 
        {
            Insert<Room>(room);
        }

        public int GetIdByName(string typeName)
        {
            Room responseValue = GetValueByName<Room>(typeName);
            return responseValue.Id;
        }

    }
}