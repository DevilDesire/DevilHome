using System;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.System.Threading;

namespace DevilHome.Server
{
    public sealed class StartupTask : IBackgroundTask
    {
        private static BackgroundTaskDeferral m_Deferral;
        private AppServiceConnection m_Connection;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            m_Deferral = taskInstance.GetDeferral();

            try
            {
                AppServiceTriggerDetails triggerDetails = taskInstance.TriggerDetails as AppServiceTriggerDetails;

                if (triggerDetails == null)
                {
                    throw new Exception("Es konnte kein Trigger gefunden werden!");
                }

                m_Connection = triggerDetails.AppServiceConnection;

                DevilHomeWebserver devilHomeWebserver = new DevilHomeWebserver(m_Connection);

                await ThreadPool.RunAsync(workItem =>
                {
                    devilHomeWebserver.Start();
                });
            }
            catch (Exception ex)
            {
                m_Deferral.Complete();
                throw ex;
            }
        }
    }
}
