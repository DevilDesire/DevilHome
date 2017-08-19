using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using DevilHome.Database.Implementations.Tables;
using DevilHome.Database.Implementations.Values;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;

namespace DevilHome.Database.Implementations.Connection
{
    public class Connection
    {
        private const string C_DATABASE_NAME_S = "DHDatabase";
        public static Task InitializeDatabase()
        {
            try
            {
                using (SQLiteConnection conn = DbConnection)
                {
                    conn.CreateTable<Room>();
                    conn.CreateTable<SensorTyp>();
                    conn.CreateTable<Sensor>();
                    conn.CreateTable<SensorData>();
                    conn.CreateTable<Device>();
                    conn.CreateTable<Devicegroup>(); 
                    conn.CreateTable<Poweroutlet>();
                }

                //SetupTestData();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return Task.CompletedTask;
        }

        public static void SetupTestData()
        {
            DbRoom dbRoom = new DbRoom();
            DbSensorTyp dbSensorTyp = new DbSensorTyp();
            DbSensor dbSensor = new DbSensor();
            DbSensorData dbSensorData = new DbSensorData();
            DbDevice dbDevice = new DbDevice();
            DbDevicegroup dbDevicegroup = new DbDevicegroup();
            DbPoweroutlet dbPoweroutlet = new DbPoweroutlet();


            Room room = new Room
            {
                Name = "Wohnzimmer"
            };

            dbRoom.Insert(room);

            SensorTyp temperatur = new SensorTyp
            {
                Name = "Temperature",
                Description = "Stellt Temperaturdaten in °C zur Verfügung"
            };

            dbSensorTyp.Insert(temperatur);

            SensorTyp luftfeuchtigkeit = new SensorTyp
            {
                Name = "Humidity",
                Description = "Stellt relative Luftfeuchtigkeitsdaten in % zur Verfügung"
            };

            dbSensorTyp.Insert(luftfeuchtigkeit);

            Sensor tempSens = new Sensor
            {
                Name = "Temperatur",
                Description = "Wohnzimmer Temperatursensor",
                Fk_Raum_Id = room.Id,
                Fk_SensorTyp_Id = temperatur.Id
            };

            Sensor humiSens = new Sensor
            {
                Name = "Luftfeuchtigkeit",
                Description = "Wohnzimmer Luftfeuchtigkeissensor",
                Fk_Raum_Id = room.Id,
                Fk_SensorTyp_Id = luftfeuchtigkeit.Id
            };

            dbSensor.Insert(tempSens);
            dbSensor.Insert(humiSens);

            //new DbSensorData().Insert(new SensorData());
        }

        public static SQLiteConnection DbConnection => new SQLiteConnection(new SQLitePlatformWinRT(), Path.Combine(ApplicationData.Current.LocalFolder.Path, $"{C_DATABASE_NAME_S}.sqlite"));

    }
}