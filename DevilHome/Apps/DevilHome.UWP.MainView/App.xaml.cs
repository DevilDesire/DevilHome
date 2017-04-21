using System;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using DevilHome.UWP.MainView.Services.SettingsServices;
using Windows.ApplicationModel.Activation;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Template10.Controls;
using Template10.Common;
using Windows.UI.Xaml.Data;
using Windows.Media.SpeechRecognition;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Storage;

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

        public override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            // TODO: add your long-running task here
            try
            {
                StorageFile vcd =
                    await Package.Current.InstalledLocation.GetFileAsync(@"CortanaCommands\CortanaCommand.xml");
                await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(vcd);

            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                //throw;
            }

        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                await StatusBar.GetForCurrentView().HideAsync();
            }

            var e = args as VoiceCommandActivatedEventArgs;
            if (e != null)
            {
                //var result = e.Result;
                //var properties = result.SemanticInterpretation.Properties.ToDictionary(x => x.Key, x => x.Value);
                //var command = result.RulePath.First();

                //if (command.Equals("LichtAn"))
                //{
                //    // get spoken text
                //    var text = properties.First(x => x.Key.Equals("devicePhrase")).Value[0];
                //    // remember to handle response appropriately
                //    var mode = properties.First(x => x.Key.Equals("commandMode")).Value;
                //    if (mode.Equals("voice"))
                //    {
                //        /* okay to speak */
                //    }
                //    else
                //    {
                //        /* not okay to speak */
                //    }
                //    // update value
                //    if (ViewModels.RoomControlPageViewModel.Instance == null)
                //        NavigationService.Navigate(typeof(Views.MainPage), text);
                //    else
                //        ViewModels.RoomControlPageViewModel.Instance.Value = text;

                //}

                //else
                //{
                //    /* unexpected command */
                //}

            }
            else
            {
                await NavigationService.NavigateAsync(typeof(Views.RoomControlPage));
            }
        }
    }
}

//protected override void OnActivated(IActivatedEventArgs args)
        //{
        //    base.OnActivated(args);

        //    if (args.Kind == ActivationKind.VoiceCommand)
        //    {
        //        VoiceCommandActivatedEventArgs cmd = args as VoiceCommandActivatedEventArgs;
        //        SpeechRecognitionResult result = cmd.Result;

        //        string commandName = result.RulePath[0];

        //        switch (commandName)
        //        {
        //            case "LichtAn":
        //                //do something
        //                break;
        //            default:
        //                Debug.WriteLine("");
        //                break;
        //        }
        //    }
        //}


