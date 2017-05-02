using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Template10.Common;
using Template10.Services.NavigationService;

namespace DevilHome.UWP.MainView.ViewModels.Settings
{
    public class LedColorPageViewModel : DevilHomeBase
    {
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            SetBusy(true);

            WindowWrapper.Current().Dispatcher.Dispatch(() =>
            {
                try
                {
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.Message);
                }
            });

            await Task.CompletedTask;
            SetBusy(false);
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending)
        {
            if (suspending)
            {

            }

            await Task.CompletedTask;
        }

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            await Task.CompletedTask;
        }



    }
}