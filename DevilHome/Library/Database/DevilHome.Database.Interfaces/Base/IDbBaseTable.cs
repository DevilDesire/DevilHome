using System.Collections.Generic;

namespace DevilHome.Database.Interfaces.Base
{
    public interface IDbBaseTable<TY> where TY : IDatabaseValue
    {
        List<TY> GetAllValues();
        TY GetValueById(int id);
        int Insert(TY value);
        int InsertUpdate(TY value);
        int Update(TY value);
        void Delete(TY value);
        void DeleteById(int id);
        TY ExecuteScalar(string sqlString);
        List<TY> GetValuesByFkNameAndId(string fkName, int id, int limit = 100);
    }
}