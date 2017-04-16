using System;
using Windows.UI.Xaml;
using System.Threading.Tasks;
using DevilHome.UWP.MainView.Services.SettingsServices;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Template10.Controls;
using Template10.Common;
using Windows.UI.Xaml.Data;

namespace DevilHome.UWP.MainView
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki

    [Bindable]
    sealed partial class App : BootStrapper
    {
        public App()
        {
            InitializeComponent();
            SplashFactory = (e) => new Views.Splash(e);

            #region app settings

            // some settings must be set in app.constructor
            var settings = SettingsService.Instance;
            RequestedTheme = settings.AppTheme;
            CacheMaxDuration = settings.CacheMaxDuration;
            ShowShellBackButton = settings.UseShellBackButton;
            AutoSuspendAllFrames = true;
            AutoRestoreAfterTerminated = true;
            AutoExtendExecutionSession = true;
            //ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;

            #endregion
        }

        public override UIElement CreateRootElement(IActivatedEventArgs e)
        {
            var service = NavigationServiceFactory(BackButton.Attach, ExistingContent.Exclude);
            return new ModalDialog
            {
                DisableBackButtonWhenModal = true,
                Content = new Views.Shell(service),
                ModalContent = new Views.Busy(),
            };
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            // TODO: add your long-running task here
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                await StatusBar.GetForCurrentView().HideAsync();
            }

            await NavigationService.NavigateAsync(typeof(Views.RoomControlPage));
        }
    }
}

