using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace DevilHome.WirelessPowerSwitchHeadless
{
    public sealed class StartupTask : IBackgroundTask
    {
        private static BackgroundTaskDeferral m_Deferral;
        internal static WirelessPowerSwitch WirelessPowerSwitch;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            m_Deferral = taskInstance.GetDeferral();

            try
            {
                if (WirelessPowerSwitch == null)
                {
                    WirelessPowerSwitch = new WirelessPowerSwitch();
                    await WirelessPowerSwitch.Initialize();
                }
            }
            catch
            {
                m_Deferral.Complete();
            }
        }
    }
}
