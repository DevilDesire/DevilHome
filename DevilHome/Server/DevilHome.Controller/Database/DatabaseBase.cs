using System;
using DevilHome.Controller.Utils;
using DevilHome.Database.Implementations.Connection;

namespace DevilHome.Controller.Database
{
    internal class DatabaseBase
    {
        public static async void InitDatabase()
        {
            try
            {
                Connection.InitializeDatabase();
            }
            catch (Exception ex)
            {
                await Logger.LogError(ex, PluginEnum.Database);
            }
        }
    }
}