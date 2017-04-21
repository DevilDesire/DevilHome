using System;
using System.Diagnostics;
using Windows.ApplicationModel.Background;

namespace DevilHome.Controller
{
    public sealed class StartupTask : IBackgroundTask
    {
        private static BackgroundTaskDeferral m_Deferral;
        internal static Controller Controller;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {

            try
            {
                m_Deferral = taskInstance.GetDeferral();
                taskInstance.Canceled += TaskInstance_Canceled;

                if (Controller == null)
                {
                    Controller = new Controller();
                    await Controller.Initialize();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void TaskInstance_Canceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            m_Deferral?.Complete();
        }
    }
}
