using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Devices.Gpio;
using Windows.Foundation;
using DevilHome.Common.Implementations.Values;
using DevilHome.Common.Interfaces.Enums;
using DevilHome.Common.Interfaces.Values;
using Newtonsoft.Json;
using RCSwitch;

namespace DevilHome.WirelessPowerSwitchHeadless
{
    internal class WirelessPowerSwitch
    {
        private AppServiceConnection m_Connection;
        private RCSwitchIO m_RcSwitch;
#pragma warning disable 414
        private GpioPin m_Pin;
#pragma warning restore 414

        private async void SetupAppService()
        {
            IReadOnlyList<AppInfo> listing = await AppServiceCatalog.FindAppServiceProvidersAsync("DevilHomeService");
            string packageName = listing.Count == 1 ? listing[0].PackageFamilyName : string.Empty;

            m_Connection = new AppServiceConnection
            {
                AppServiceName = "DevilHomeService",
                PackageFamilyName = packageName
            };

            AppServiceConnectionStatus status = await m_Connection.OpenAsync();

            if (status != AppServiceConnectionStatus.Success)
            {
                Debug.WriteLine($"Verbindung fehlgeschlagen: {status}");
            }
            else
            {
                Debug.WriteLine($"Verbindung erfolgreich hergestellt: {status}");
                m_Connection.RequestReceived += ConnectionOnRequestReceived;
            }
        }

        private void SetupGpio()
        {
            m_Pin = null;
        }

        private void SetupRcSwitch()
        {
            m_RcSwitch = new RCSwitchIO(5, -1);
        }

        private void ConnectionOnRequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            try
            {
                if (m_RcSwitch == null)
                {
                    return;
                }

                string json = args.Request.Message.First().Value.ToString();
                IQueryValue queryValue = JsonConvert.DeserializeObject<QueryValue>(json);

                if (queryValue.QueryType == QueryType.Power)
                {
                    List<string> actionParameter = queryValue.Action.Split('-').ToList();
                    HandlePowerAction(queryValue.FunctionType, actionParameter);
                }
            }
            catch
            {
                //ignore
            }
        }

        private void HandlePowerAction(FunctionType type, List<string> actionParameters)
        {
            switch (type)
            {
                case FunctionType.Outlet:
                    HandleOutlet(actionParameters);
                    break;
            }
        }

        private void HandleOutlet(List<string> parameterList)
        {
            if (parameterList.Count != 3)
            {
                throw new Exception("Um eine Steckdose zu steuern müssen es 3 Parameter sein (Hauscode, Gerätecode, Status)");
            }

            string homecode = parameterList[0];
            string devicecode = parameterList[1];
            bool status = parameterList[2] == "1";

            m_RcSwitch.Switch(homecode, devicecode, status);
        }

        public IAsyncAction Initialize()
        {
            return Task.Run(() =>
            {
                SetupAppService();
                SetupGpio();
                SetupRcSwitch();
            }).AsAsyncAction();
        }
    }
}