using DevilHome.Database.Implementations.Base;
using DevilHome.Database.Implementations.Values;
using DevilHome.Database.Interfaces.Tables;
using DevilHome.Database.Interfaces.Values;

namespace DevilHome.Database.Implementations.Tables
{
    public class DbRoom : DbBaseTable<Room, IRoom>, IDbRoom
    {
        public int GetIdByName(string roomName)
        {
            IRoom responseValue = ExecuteScalar($"select * from DbRoom where Name = '{roomName}'");
            return responseValue.Id;
        }
    }
}