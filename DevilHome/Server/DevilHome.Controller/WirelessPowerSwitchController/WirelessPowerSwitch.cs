using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevilHome.Common.Interfaces.Enums;
using DevilHome.Common.Interfaces.Values;
using RCSwitch;

namespace DevilHome.Controller.WirelessPowerSwitchController
{
    internal class WirelessPowerSwitch
    {
        private static RCSwitchIO m_RcSwitch;


        public void InitializeGpio()
        {
            m_RcSwitch = new RCSwitchIO(5, -1);
        }

        public async Task<string> ProcessingGetRequest(IQueryValue queryValue)
        {
            return "";
        }

        public Task ProcessingSetRequest(IQueryValue queryValue)
        {
            List<string> actionParameter = queryValue.Action.Split('-').ToList();
            switch (queryValue.FunctionType)
            {
                case FunctionType.Outlet:
                    HandleOutlet(actionParameter);
                    break;
            }

            return Task.CompletedTask;
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