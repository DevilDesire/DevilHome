using System.Collections.Generic;
using System.Linq;
using SQLite.Net;

namespace DevilHome.Database.Implementations.Engine
{
    public class DatabaseEngineBase
    {
        protected SQLiteConnection DbConnection => Connection.Connection.DbConnection;

        private bool DatabaseIsInitialized()
        {
            return Connection.Connection.DbConnection != null;
        }

        protected List<T> GetAllValues<T>() where T : class, new()
        {
            List<T> responseList = null;

            if (DatabaseIsInitialized())
            {
                using (SQLiteConnection conn = DbConnection)
                {
                    string statement = $"select * from {typeof(T).Name}";
                    responseList = conn.Query<T>(statement).ToList();
                }
            }

            return responseList ?? new List<T>();
        }

        protected T GetValueById<T>(int id) where T : class, new()
        {
            T returnValue;

            using (SQLiteConnection conn = DbConnection)
            {
                string statement = $"select * from {typeof(T).Name}";
                returnValue = conn.Query<T>(statement).FirstOrDefault();
            }

            return returnValue;
        }

        protected void Insert<T>(T value) where T : class, new()
        {
            if (DatabaseIsInitialized())
            {
                using (SQLiteConnection conn = DbConnection)
                {
                    conn.Insert(value);
                }
            }
        }

        protected void InsertUpdate<T>(T value)
        {
            if (DatabaseIsInitialized())
            {
                using (SQLiteConnection conn = DbConnection)
                {
                    conn.InsertOrReplace(value);
                }
            }
        }

        protected void Update<T>(T value)
        {
            if (DatabaseIsInitialized())
            {
                using (SQLiteConnection conn = DbConnection)
                {
                    conn.Update(value);
                }
            }
        }

        protected void Delete<T>(T value)
        {
            if (DatabaseIsInitialized())
            {
                using (SQLiteConnection conn = DbConnection)
                {
                    conn.Delete(value);
                }
            }
        }
    }
}