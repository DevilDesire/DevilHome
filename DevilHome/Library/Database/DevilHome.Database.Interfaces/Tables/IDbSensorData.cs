using System.Collections.Generic;
using DevilHome.Database.Interfaces.Base;
using DevilHome.Database.Interfaces.Values;

namespace DevilHome.Database.Interfaces.Tables
{
    public interface IDbSensorData : IDbBaseTable<ISensorData>
    {
        List<ISensorData> GetValuesBySensorId(int id);
        double GetLastValueBySensorId(int id);
    }
}