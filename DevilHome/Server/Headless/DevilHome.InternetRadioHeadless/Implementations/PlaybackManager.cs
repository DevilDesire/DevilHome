using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DevilHome.InternetRadioHeadless.Interfaces;
using Microsoft.Maker.Media.UniversalMediaEngine;

namespace DevilHome.InternetRadioHeadless.Implementations
{
    class PlaybackManager : IPlaybackManager
    {
        private MediaEngine m_MediaEngine;
        private PlaybackState m_State;

        public event VolumeChangedEventHandler VolumeChanged;
        public event PlaybackStatusChangedEventHandler PlaybackStateChanged;

        public double Volume
        {
            get
            {
                return m_MediaEngine.Volume;
            }
            set
            {
                if (value >= 0 && value <= 1)
                {
                    m_MediaEngine.Volume = value;
                    VolumeChanged?.Invoke(this, new VolumeChangedEventArgs { Volume = value });
                }
                else
                {
                    Debug.WriteLine("Invalid volume entered");
                }
            }
        }

        public PlaybackState PlaybackState
        {
            get
            {
                return m_State;
            }
            internal set
            {
                if (m_State != value)
                {
                    m_State = value;
                    PlaybackStateChanged?.Invoke(this, new PlaybackStateChangedEventArgs { State = m_State });
                }
            }
        }

        private void MediaEngine_MediaStateChanged(MediaState state)
        {
            switch (state)
            {
                case MediaState.Loading:
                    PlaybackState = PlaybackState.Loading;

                    break;

                case MediaState.Stopped:
                    PlaybackState = PlaybackState.Paused;

                    break;

                case MediaState.Playing:
                    PlaybackState = PlaybackState.Playing;
                    break;

                case MediaState.Error:
                    PlaybackState = PlaybackState.ErrorMediaInvalid;
                    break;

                case MediaState.Ended:
                    PlaybackState = PlaybackState.Ended;
                    break;
            }
        }

        public async Task InitializeAsync()
        {
            m_MediaEngine = new MediaEngine();
            var result = await m_MediaEngine.InitializeAsync();
            if (result == MediaEngineInitializationResult.Fail)
            {
                Debug.WriteLine("MediaEngine_FailedToInitialize");

            }

            m_MediaEngine.MediaStateChanged += MediaEngine_MediaStateChanged;
        }

        public void Play(Uri mediaAddress)
        {
            var addressString = mediaAddress.ToString();
            m_MediaEngine.Play(addressString);
        }

        public void Pause()
        {
            m_MediaEngine.Pause();
            PlaybackState = PlaybackState.Paused;
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}