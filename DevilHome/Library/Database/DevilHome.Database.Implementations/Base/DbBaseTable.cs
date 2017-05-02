using System.Collections.Generic;
using System.Linq;
using DevilHome.Database.Interfaces.Base;
using SQLite.Net;

namespace DevilHome.Database.Implementations.Base
{
    public class DbBaseTable<T, TY> : Connection.Connection, IDbBaseTable<TY> where TY : class, IDatabaseValue where T : DatabaseValue
    {
        public int Insert(TY value)
        {
            using (SQLiteConnection conn = DbConnection)
            {
                conn.Insert(value);
            }

            return value.Id;
        }

        public List<TY> GetValuesByFkNameAndId(string fkName, int id, int limit = 100)
        {
            List<TY> returnList;

            using (SQLiteConnection conn = DbConnection)
            {
                string tableName = typeof(TY).Name.StartsWith("I") ? typeof(TY).Name.Remove(0, 1) : typeof(TY).Name;
                returnList = conn.Query<T>($"select * from {tableName} where {fkName}={id} order by Id desc Limit {limit}").OrderBy(r => r.Id).Cast<TY>().ToList();
            }

            return returnList;
        }
    }
}