using System.Collections.Generic;

namespace DevilHome.Database.Interfaces.Base
{
    public interface IDbBaseTable<TY> where TY : IDatabaseValue
    {
        List<TY> GetAllValues();
        TY GetValueById(int id);
        int Insert(TY value);
        void Update(TY value);
        void DeleteById(int id);
        List<TY> GetValuesByFkNameAndId(string fkName, int id, int limit = 100);
    }
}