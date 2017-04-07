using System;
using System.Threading.Tasks;

namespace DevilHome.InternetRadioHeadless.Interfaces
{
    internal enum PlaybackState
    {
        ErrorMediaInvalid = 0,
        ErrorLostConnection,
        Stopped = 100,
        Paused,
        Loading,
        Playing,
        Ended
    }

    internal struct VolumeChangedEventArgs
    {
        public double Volume;
    }

    internal struct PlaybackStateChangedEventArgs
    {
        public PlaybackState State;
    }

    delegate void VolumeChangedEventHandler(object sender, VolumeChangedEventArgs e);
    delegate void PlaybackStatusChangedEventHandler(object sender, PlaybackStateChangedEventArgs e);

    internal interface IPlaybackManager
    {
        event VolumeChangedEventHandler VolumeChanged;
        event PlaybackStatusChangedEventHandler PlaybackStateChanged;

        double Volume { get; set; }
        PlaybackState PlaybackState { get; }
        Task InitializeAsync();
        void Play(Uri mediaAdress);
        void Pause();
        void Stop();
    }
}