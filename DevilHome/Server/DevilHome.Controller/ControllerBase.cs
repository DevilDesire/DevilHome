﻿using DevilHome.Controller.ApiV2;
using DevilHome.Controller.ApiV2.Poweroutlet;
using DevilHome.Controller.Database;
using DevilHome.Controller.TemperatureController;
using DevilHome.Controller.WirelessPowerSwitchController;
using DevilHome.Database.Implementations.Tables;
using DevilHome.Database.Interfaces.Tables;
using Newtonsoft.Json;
using StructureMap;

namespace DevilHome.Controller
{
    internal class ControllerBase
    {
        #region Init

        protected static IContainer Container;

        public static void Configure()
        {
            Container = new Container(x =>
            {
                x.For<IDbSensorData>().Use<DbSensorData>();
                x.For<IDatabaseBase>().Use<DatabaseBase>();
                x.For<IDbPoweroutlet>().Use<DbPoweroutlet>();
            });
        }

        #endregion

        protected IDatabaseBase DatabaseBase => Container.GetInstance<IDatabaseBase>();
        protected IDbSensorData DbSensorData => Container.GetInstance<IDbSensorData>();
        protected IDbSensor DbSensor => Container.GetInstance<IDbSensor>();
        protected IDbPoweroutlet DbPoweroutlet => Container.GetInstance<IDbPoweroutlet>();

        protected WirelessPowerSwitch WirelessPowerSwitchController { get; set; }
        protected Temperature TemperatureController { get; set; }
        protected ApiHandler ApiHandler { get; set; }

        protected string ConvertToJson<T>(T objectToConvert)
        {
            return JsonConvert.SerializeObject(objectToConvert);
        }

    }
}