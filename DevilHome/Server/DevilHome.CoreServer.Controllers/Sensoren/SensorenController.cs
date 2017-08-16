using System;
using System.Collections.Generic;
using DevilHome.Common.Implementations.Values;
using DevilHome.Database.Implementations.Tables;
using DevilHome.Database.Implementations.Values;
using DevilHome.Database.Interfaces.Values;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;

namespace DevilHome.CoreServer.Controllers.Sensoren
{
    [RestController(InstanceCreationType.Singleton)]
    public class SensorenController : ControllersBase
    {
        public SensorenController()
        {
            Configure();
            DatabaseBase.InitDatabase();
        }

        [UriFormat("/sensor")]
        public IGetResponse GetAllSensoren()
        {
            return new GetResponse(GetResponse.ResponseStatus.OK, "");
        }

        [UriFormat("/sensoren?p={roomId}")]
        public IGetResponse GetSensorenByRoomId(string roomId)
        {
            return new GetResponse(GetResponse.ResponseStatus.OK, roomId);
        }

        [UriFormat("/sensoren/adddata?raumname={raum}&sensortyp={sensortyp}&value={value}")]
        public IGetResponse AddSensorValues(string raum, string sensortyp, string value)
        {
            int sensorId = new DbSensor().GetIdBySensorTypNameRoomName(sensortyp, raum);
            DbSensorData.Insert(new SensorData
            {
                Value = Convert.ToDouble(value),
                Fk_Sensor_Id = sensorId
            });

            return new GetResponse(GetResponse.ResponseStatus.OK);
        }

        [UriFormat("/sensoren/get?sensorId={sensorId}")]
        public IGetResponse GetSensorDataBySensorId(int sensorId)
        {
            List<ISensorData> values = DbSensorData.GetValuesBySensorId(sensorId);
            List<SensorDataValue> convertedValues = new List<SensorDataValue>();
            values.ForEach(x => convertedValues.Add(new SensorDataValue { Date = Convert.ToDateTime(x.Date), Value = x.Value, Fk_Sensor_Id = x.Fk_Sensor_Id }));

            return new GetResponse(GetResponse.ResponseStatus.OK, convertedValues);
        }
    }
}