using System;
using DevilHome.CoreServer.Controllers.Database;
using DevilHome.Database.Implementations.Tables;
using DevilHome.Database.Interfaces.Tables;
using Newtonsoft.Json;
using StructureMap;
using IDatabaseBase = DevilHome.CoreServer.Controllers.Database.IDatabaseBase;

namespace DevilHome.CoreServer.Controllers
{
    public class ControllersBase
    {
        #region Init

        protected static IContainer Container;
        private static bool _isInitialized;
        public static void Configure()
        {
            if (!_isInitialized)
            {
                Container = new Container(x =>
                {
                    x.For<IDbSensorData>().Use<DbSensorData>();
                    x.For<IDatabaseBase>().Use<DatabaseBase>();
                    //x.For<IDbSensor>().Use<DbSensor>();
                    x.For<IDbPoweroutlet>().Use<DbPoweroutlet>();
                    x.For<IDbRoom>().Use<DbRoom>();
                    x.For<IDbSystemConfiguration>().Use<DbSystemConfiguration>();
                });

                _isInitialized = true;
            }
        }

        #endregion

        protected IDatabaseBase DatabaseBase => Container.GetInstance<IDatabaseBase>();
        protected IDbSensorData DbSensorData => Container.GetInstance<IDbSensorData>();
        protected IDbSensor DbSensor => Container.GetInstance<IDbSensor>();
        protected IDbPoweroutlet DbPoweroutlet => Container.GetInstance<IDbPoweroutlet>();
        protected IDbRoom DbRoom => Container.GetInstance<IDbRoom>();
        protected IDbSystemConfiguration DbSystemConfiguration => Container.GetInstance<IDbSystemConfiguration>();
        protected DateTime Now = DateTime.Now;

        protected string ConvertToJson<T>(T objectToConvert)
        {
            return JsonConvert.SerializeObject(objectToConvert);
        }

    }
}