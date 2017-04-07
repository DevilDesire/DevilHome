using System;
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
using DevilHome.InternetRadioHeadless.Implementations;
using DevilHome.InternetRadioHeadless.Interfaces;
using Newtonsoft.Json;

namespace DevilHome.InternetRadioHeadless
{
    internal class InternetRadio
    {
        #region Privates

        private IPlaybackManager m_PlaybackManager;
        private AppServiceConnection m_Connection;
        private uint m_PlaybackRetries;
        private const uint MAX_RETRIES = 3;

        #endregion

        #region Initialize

        private async void SetupRadio()
        {
            try
            {
                m_PlaybackManager = new PlaybackManager();
                m_PlaybackManager.PlaybackStateChanged += PlaybackManager_PlaybackStateChanged;
                m_PlaybackManager.VolumeChanged += PlaybackManagerOnVolumeChanged;
                await m_PlaybackManager.InitializeAsync();
                m_PlaybackManager.Volume = 0.2;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }


        }

        private void PlaybackManagerOnVolumeChanged(object sender, VolumeChangedEventArgs volumeChangedEventArgs)
        {

        }

        private async void SetupAppService()
        {
            IReadOnlyList<AppInfo> listing = await AppServiceCatalog.FindAppServiceProvidersAsync("DevilHomeService");
            string packageName = (listing.Count == 1)
                ? listing[0].PackageFamilyName
                : string.Empty;

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

        #endregion

        public IAsyncAction Initialize()
        {
            return Task.Run(async () =>
            {
                SetupRadio();
                SetupAppService();

            }).AsAsyncAction();
        }

        private async void ConnectionOnRequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            try
            {
                string json = args.Request.Message.First().Value.ToString();
                IQueryValue queryValue = JsonConvert.DeserializeObject<QueryValue>(json);
                object returnValue = await ControlRadio(queryValue);

                if (returnValue != null)
                {
                    await sender.SendMessageAsync(new ValueSet{ new KeyValuePair<string, object>("Volume", returnValue)});
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        private async Task<object> ControlRadio(IQueryValue queryValue)
        {
            if (queryValue.QueryType == QueryType.Error || queryValue.QueryType != QueryType.Radio)
            {
                return null;
            }

            try
            {
                if (queryValue.RequestType == RequestType.Set)
                {
                    switch (queryValue.FunctionType)
                    {
                        case FunctionType.Music:
                            switch (queryValue.Action.ToLower())
                            {
                                case "stop":
                                    m_PlaybackManager.Pause();
                                    break;
                                case "play":
                                    m_PlaybackManager.Play(new Uri("http://listen.to.techno4ever.net/dsl/aac"));
                                    break;
                            }
                            break;
                        case FunctionType.Volume:
                            try
                            {
                                double volume = Convert.ToDouble(queryValue.Action) / 100;
                                m_PlaybackManager.Volume = volume;
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex.Message);
                            }
                            break;
                    }
                }
                if (queryValue.RequestType == RequestType.Get)
                {
                    if (queryValue.FunctionType == FunctionType.Volume)
                    {
                        return m_PlaybackManager.Volume;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return null;
        }

        private void PlaybackManager_PlaybackStateChanged(object sender, PlaybackStateChangedEventArgs e)
        {
            Debug.WriteLine(string.Format("playbackstate changed: {0}", e.State.ToString()));
            switch (e.State)
            {
                case PlaybackState.ErrorMediaInvalid:
                    //await this.tryWriteToDisplay(this.resourceLoader.GetString("MediaErrorMessage") + "\n" + this.radioPresetManager.CurrentTrack.Name, 0);
                    break;

                case PlaybackState.Loading:
                    //await this.tryWriteToDisplay(this.resourceLoader.GetString("MediaLoadingMessage") + "\n" + this.radioPresetManager.CurrentTrack.Name, 0);
                    break;

                case PlaybackState.Playing:
                    m_PlaybackRetries = 0;
                    //await this.tryWriteToDisplay(this.resourceLoader.GetString("NowPlayingMessage") + "\n" + this.radioPresetManager.CurrentTrack.Name, 0);
                    break;
                case PlaybackState.Ended:
                    if (MAX_RETRIES > m_PlaybackRetries)
                    {
                        //playChannel(this.radioPresetManager.CurrentTrack);
                    }
                    else
                    {
                        //await this.tryWriteToDisplay(this.resourceLoader.GetString("ConnectionFailedMessage") + "\n" + this.radioPresetManager.CurrentTrack.Name, 0);
                    }

                    break;
            }
        }

    }
}