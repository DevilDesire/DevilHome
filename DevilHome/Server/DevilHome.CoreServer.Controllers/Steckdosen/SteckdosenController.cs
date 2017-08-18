using System;
using System.Collections.Generic;
using System.Globalization;
using DevilHome.Database.Implementations.Values;
using DevilHome.Database.Interfaces.Values;
using RCSwitch;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;

namespace DevilHome.CoreServer.Controllers.Steckdosen
{
    [RestController(InstanceCreationType.Singleton)]
    public class SteckdosenController : ControllersBase
    {
        private static RCSwitchIO _rcSwitch;

        public SteckdosenController()
        {
            Configure();
            _rcSwitch = new RCSwitchIO(5, -1);
            DatabaseBase.InitDatabase();
        }

        [UriFormat("/steckdosen")]
        public IGetResponse GetAllSteckdosen()
        {
            List<IPoweroutlet> steckdosen = DbPoweroutlet.GetAllValues();
            List<Poweroutlet> convertedValues = new List<Poweroutlet>();
            steckdosen.ForEach(
                x =>
                    convertedValues.Add(new Poweroutlet
                    {
                        Date = x.Date,
                        DeviceCode = x.DeviceCode,
                        HausCode = x.HausCode,
                        Fk_Raum_Id = x.Fk_Raum_Id,
                        Name = x.Name,
                        Id = x.Id
                    }));
            return new GetResponse(GetResponse.ResponseStatus.OK, convertedValues);
        }

        [UriFormat("/steckdosen/get?raumId={raumId}")]
        public IGetResponse GetSteckdosenByRoomId(int raumId)
        {
            try
            {
                List<IPoweroutlet> steckdosen = DbPoweroutlet.GetValuesByRoomId(raumId);
                List<Poweroutlet> convertedValues = new List<Poweroutlet>();
                steckdosen.ForEach(
                    x =>
                        convertedValues.Add(new Poweroutlet
                        {
                            Date = x.Date,
                            DeviceCode = x.DeviceCode,
                            HausCode = x.HausCode,
                            Fk_Raum_Id = x.Fk_Raum_Id,
                            Name = x.Name,
                            Id = x.Id
                        }));

                return new GetResponse(GetResponse.ResponseStatus.OK, convertedValues);
            }
            catch (Exception ex)
            {
                return new GetResponse(GetResponse.ResponseStatus.NotFound, ex.Message);
            }
        }

        [UriFormat("/steckdosen/switch?hc={homeCode}&dc={deviceCode}&s={status}")]
        public IGetResponse SwitchSteckdose(string homeCode, string deviceCode, string status)
        {
            GetResponse.ResponseStatus responseStatus = GetResponse.ResponseStatus.OK;

            try
            {
                _rcSwitch.Switch(homeCode, deviceCode, status == "on");
            }
            catch (Exception)
            {
                responseStatus = GetResponse.ResponseStatus.NotFound;
            }

            return new GetResponse(responseStatus);
        }

        [UriFormat("/steckdosen/add?hc={homeCode}&dc={deviceCode}&raumId={raumId}&name={name}&secure={secure}")]
        public GetResponse AddSteckdose(string homeCode, string deviceCode, string raumId, string name, bool secure)
        {
            try
            {
                DbPoweroutlet.Insert(new Poweroutlet
                {
                    Date = Now.ToString(CultureInfo.CurrentCulture),
                    DeviceCode = deviceCode,
                    HausCode = homeCode,
                    Fk_Raum_Id = Convert.ToInt32(raumId),
                    Name = name,
                    Secure = secure
                });

                return new GetResponse(GetResponse.ResponseStatus.OK);
            }
            catch (Exception ex)
            {
                return new GetResponse(GetResponse.ResponseStatus.NotFound, ex.Message);
            }
        }

        [UriFormat("steckdosen/delete?id={id}")]
        public GetResponse DeleteSteckdose(string id)
        {
            try
            {
                DbPoweroutlet.DeleteById(Convert.ToInt32(id));
                return new GetResponse(GetResponse.ResponseStatus.OK);
            }
            catch
            {
                return new GetResponse(GetResponse.ResponseStatus.NotFound);
            }
        }

        [UriFormat("/steckdosen/update?id={id}&hc={homeCode}&dc={deviceCode}&raumId={raumId}&name={name}&secure={secure}")]
        public GetResponse UpdateSteckdose(int id, string homeCode, string deviceCode, int raumId, string name, bool secure)
        {
            try
            {
                IPoweroutlet updateValue = DbPoweroutlet.GetValueById(id);

                updateValue.Fk_Raum_Id = raumId;
                updateValue.DeviceCode = deviceCode;
                updateValue.HausCode = homeCode;
                updateValue.Secure = secure;
                updateValue.Name = name;

                DbPoweroutlet.Update(updateValue);
                return new GetResponse(GetResponse.ResponseStatus.OK);
            }
            catch (Exception ex)
            {
                return new GetResponse(GetResponse.ResponseStatus.NotFound, ex.Message);
            }
        }
    }
}