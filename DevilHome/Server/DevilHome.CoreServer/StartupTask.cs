using System;
using System.Diagnostics;
using Windows.ApplicationModel.Background;
using DevilHome.CoreServer.Controllers.Raum;
using DevilHome.CoreServer.Controllers.Sensoren;
using DevilHome.CoreServer.Controllers.Steckdosen;
using Restup.Webserver.Http;
using Restup.Webserver.Rest;

namespace DevilHome.CoreServer
{
    public sealed class StartupTask : IBackgroundTask
    {
        private HttpServer _httpServer;
        // ReSharper disable once NotAccessedField.Local
        private BackgroundTaskDeferral _deferral;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            try
            {
                _deferral = taskInstance.GetDeferral();
                RestRouteHandler restRouteHandler = new RestRouteHandler();

                restRouteHandler.RegisterController<SteckdosenController>();
                restRouteHandler.RegisterController<SensorenController>();
                restRouteHandler.RegisterController<RaumController>();

                HttpServerConfiguration configuration = new HttpServerConfiguration()
                    .ListenOnPort(9000)
                    .RegisterRoute("api", restRouteHandler)
                    .EnableCors();

                _httpServer = new HttpServer(configuration);

                await _httpServer.StartServerAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
