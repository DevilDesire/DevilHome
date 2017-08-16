﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation;
using Windows.Foundation.Collections;
using DevilHome.Common.Implementations.Values;
using DevilHome.Common.Interfaces.Enums;
using DevilHome.Common.Interfaces.Values;
using DevilHome.Controller.ApiV2;
using DevilHome.Controller.Configuration;
using DevilHome.Controller.TemperatureController;
using DevilHome.Controller.Utils;
using DevilHome.Controller.WirelessPowerSwitchController;
using Newtonsoft.Json;

namespace DevilHome.Controller
{
    internal class Controller : ControllerBase
    {
        private AppServiceConnection m_Connection;


        public IAsyncAction Initialize()
        {
            return Task.Run(() =>
            {
                StructureMapInitializer.Init();
                InitializeDatabase();
                InitializePlugins();
                SetupAppService();
                SetupGpio();
                FinishInitialize();
            }).AsAsyncAction();
        }

        private void InitializePlugins()
        {
            WirelessPowerSwitchController = new WirelessPowerSwitch();
            TemperatureController = new Temperature();
            ApiHandler = new ApiHandler();
        }

        private void SetupGpio()
        {
            WirelessPowerSwitchController.InitializeGpio();
            TemperatureController.InitializeGpio();
        }

        private void InitializeDatabase()
        {
            DatabaseBase.InitDatabase();
        }

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

        private async void FinishInitialize()
        {
            await Logger.LogInformation("DevilHome is successfully initialized", PluginEnum.Controller);
        }

        private async void ConnectionOnRequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            AppServiceDeferral messageDeferral = args.GetDeferral();
            try
            {
                string json = args.Request.Message.First().Value.ToString();
                IQueryValue queryValue = JsonConvert.DeserializeObject<QueryValue>(json);

                switch (queryValue.RequestType)
                {
                    case RequestType.Get:
                        string response = await ApiHandler.HandleGet(queryValue);
                        await args.Request.SendResponseAsync(new ValueSet
                        {
                            new KeyValuePair<string, object>("Query", response)
                        });
                        break;
                    case RequestType.Set:
                        HandleSet(queryValue);
                        break;
                }
            }
            catch (Exception ex)
            {
                await Logger.LogError(ex, PluginEnum.Controller);
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                messageDeferral?.Complete();
            }
        }

        private async void HandleSet(IQueryValue queryValue)
        {
            try
            {
                switch (queryValue.QueryType)
                {
                    case QueryType.Power:
                        await WirelessPowerSwitchController.ProcessingSetRequest(queryValue);
                        break;
                    case QueryType.Sensor:
                        await TemperatureController.ProcessingSetRequest(queryValue);
                        break;
                }
            }
            catch (Exception ex)
            {
                await Logger.LogError(ex, PluginEnum.Controller);
            }
        }
    }
}