using System;
using System.Diagnostics;
using System.IO;
using Windows.Storage;
using DevilHome.Database.Implementations.Values;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;

namespace DevilHome.Database.Implementations.Connection
{
    public class Connection
    {
        private const string C_DATABASE_NAME_S = "DevilHomeDatabase";
        public static void InitializeDatabase()
        {
            try
            {
                using (SQLiteConnection conn = DbConnection)
                {
                    conn.CreateTable<RoomValue>();
                    conn.CreateTable<SensorValue>();
                    conn.CreateTable<DeviceValue>();
                    conn.CreateTable<DevicegroupValue>();
                    conn.CreateTable<PoweroutletValue>();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public static SQLiteConnection DbConnection => new SQLiteConnection(new SQLitePlatformWinRT(), Path.Combine(ApplicationData.Current.LocalFolder.Path, $"{C_DATABASE_NAME_S}.sqlite"));

    }
}