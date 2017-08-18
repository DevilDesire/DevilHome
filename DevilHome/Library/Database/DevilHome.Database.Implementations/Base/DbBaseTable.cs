using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DevilHome.Database.Interfaces.Base;
using SQLite.Net;

namespace DevilHome.Database.Implementations.Base
{
    public class DbBaseTable<T, TY> : Connection.Connection, IDbBaseTable<TY> where TY : class, IDatabaseValue where T : DatabaseValue
    {
        public List<TY> GetAllValues()
        {
            using (SQLiteConnection conn = DbConnection)
            {
                return conn.Query<T>($"select * from {GetTableName()}").OrderBy(r => r.Id).Cast<TY>().ToList();
            }
        }

        public TY GetValueById(int id)
        {
            using (SQLiteConnection conn = DbConnection)
            {
                return conn.Query<T>($"select * from {GetTableName()} where Id={id}").FirstOrDefault() as TY;
            }
        }

        public int Insert(TY value)
        {
            using (SQLiteConnection conn = DbConnection)
            {
                conn.Insert(value);
            }

            return value.Id;
        }

        public void Update(TY value)
        {
            using (SQLiteConnection conn = DbConnection)
            {
                string updateString = $"update {GetTableName()} set";

                foreach (PropertyInfo propertyInfo in value.GetType().GetProperties())
                {
                    if (propertyInfo.Name == "Id")
                    {
                        continue;
                    }

                    updateString += $" {propertyInfo.Name}={propertyInfo.GetValue(value)},";
                }

                if (updateString.EndsWith(","))
                {
                    updateString = updateString.Remove(updateString.LastIndexOf(",", StringComparison.Ordinal));
                }

                updateString += $"where Id={value.Id}";

                conn.Query<T>(updateString);
            }
        }

        public void DeleteById(int id)
        {
            using (SQLiteConnection conn = DbConnection)
            {
                conn.Query<T>($"delete from {GetTableName()} where Id={id}");
            }
        }

        public List<TY> GetValuesByFkNameAndId(string fkName, int id, int limit = 100)
        {
            List<TY> returnList;

            using (SQLiteConnection conn = DbConnection)
            {
                returnList = conn.Query<T>($"select * from {GetTableName()} where {fkName}={id} order by Id desc Limit {limit}").OrderBy(r => r.Id).Cast<TY>().ToList();
            }

            return returnList;
        }

        private string GetTableName()
        {
            return typeof(TY).Name.StartsWith("I") ? typeof(TY).Name.Remove(0, 1) : typeof(TY).Name;
        }
    }
}