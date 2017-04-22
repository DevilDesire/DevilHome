using System;
using System.Diagnostics;
using Windows.ApplicationModel.Background;
using DevilHome.Controller.Utils;

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
                    await Logger.InitializeLogger();
                    await Controller.Initialize();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                if (Logger.IsInitialized())
                {
                    await Logger.LogError(ex, PluginEnum.Controller);
                }
            }
        }

        private void TaskInstance_Canceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            m_Deferral?.Complete();
        }
    }
}
