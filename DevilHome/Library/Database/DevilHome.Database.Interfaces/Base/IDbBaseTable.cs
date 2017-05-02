using System.Collections.Generic;

namespace DevilHome.Database.Interfaces.Base
{
    public interface IDbBaseTable<TY> where TY : IDatabaseValue
    {
        int Insert(TY value);
        List<TY> GetValuesByFkNameAndId(string fkName, int id, int limit = 100);
    }
}