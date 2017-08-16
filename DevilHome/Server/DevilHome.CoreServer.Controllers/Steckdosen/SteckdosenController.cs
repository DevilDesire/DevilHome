using System;
using RCSwitch;
using Restup.Webserver.Attributes;
using Restup.Webserver.Models.Contracts;
using Restup.Webserver.Models.Schemas;

namespace DevilHome.CoreServer.Controllers.Steckdosen
{
    public struct Test
    {
        public string Header { get; set; }
        public int Number { get; set; }
    }

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
            return new GetResponse(GetResponse.ResponseStatus.OK, new Test{Header = "TestHeader", Number = 12});
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
    }
}