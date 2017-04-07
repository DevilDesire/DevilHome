using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace DevilHome.InternetRadioHeadless
{
    public sealed class StartupTask : IBackgroundTask
    {
        private static BackgroundTaskDeferral m_Deferral;
        internal static InternetRadio InternetRadio;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            m_Deferral = taskInstance.GetDeferral();

            if (InternetRadio == null)
            {
                InternetRadio = new InternetRadio();
                await InternetRadio.Initialize();
            }
        }
    }
}
