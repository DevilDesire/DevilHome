using System;
using Windows.ApplicationModel.Background;

namespace DevilHome.Server.Services.TempertureService
{
    public sealed class StartupTask : IBackgroundTask
    {
        private BackgroundTaskDeferral _deferral;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();

            await new TemperatureGetter().Run();
        }
    }
}
