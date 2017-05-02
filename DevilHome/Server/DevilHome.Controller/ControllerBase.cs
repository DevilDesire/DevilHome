using DevilHome.Controller.Database;
using DevilHome.Database.Implementations.Tables;
using DevilHome.Database.Interfaces.Tables;
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
            });
        }

        #endregion

        protected IDatabaseBase DatabaseBase => Container.GetInstance<IDatabaseBase>();
        protected IDbSensorData DbSensorData => Container.GetInstance<IDbSensorData>();
        protected IDbSensor DbSensor => Container.GetInstance<IDbSensor>();
    }
}