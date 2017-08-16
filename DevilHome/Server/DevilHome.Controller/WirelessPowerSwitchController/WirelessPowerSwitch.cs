using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevilHome.Common.Interfaces.Enums;
using DevilHome.Common.Interfaces.Values;
using DevilHome.Controller.ApiV2.Poweroutlet;
using DevilHome.Controller.Utils;
using RCSwitch;

namespace DevilHome.Controller.WirelessPowerSwitchController
{
    internal class WirelessPowerSwitch
    {
        private static RCSwitchIO m_RcSwitch;


        public async void InitializeGpio()
        {
            try
            {
                m_RcSwitch = new RCSwitchIO(5, -1);
            }
            catch (Exception ex)
            {
                await Logger.LogError(ex, PluginEnum.WirelessPowerSwitchController);
            }
            
        }

        public async Task<string> ProcessingGetRequest(IQueryValue queryValue)
        {
            return "";
        }

        public async Task ProcessingSetRequest(IQueryValue queryValue)
        {
            try
            {
                switch (queryValue.FunctionType)
                {
                    case FunctionType.Outlet:
                        HandleOutlet(queryValue.Action.Split('-').ToList());
                        break;
                    case FunctionType.Create:
                        new PoweroutletSetter().CreateNewPoweroutlet(queryValue);
                        break;
                }
            }
            catch (Exception ex)
            {
                await Logger.LogError(ex, PluginEnum.WirelessPowerSwitchController);
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
    }
}