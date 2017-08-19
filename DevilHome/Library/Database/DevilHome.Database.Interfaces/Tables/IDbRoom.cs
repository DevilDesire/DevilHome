using DevilHome.Database.Interfaces.Base;
using DevilHome.Database.Interfaces.Values;

namespace DevilHome.Database.Interfaces.Tables
{
    public interface IDbRoom : IDbBaseTable<IRoom>
    {
        int GetIdByName(string roomName);
    }
}