using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Template10.Mvvm;
using Template10.Services.NavigationService;

namespace DevilHome.UWP.MainView.ViewModels
{
    public class InternetRadioPageViewModel : DevilHomeBase
    {
        public InternetRadioPageViewModel()
        {
            m_Play = false;
        }

        private bool m_Play;

        public bool Play
        {
            get
            {
                return m_Play;
            }
            set
            {
                Set(ref m_Play, value);
                SetPlay.Execute();
            }
        }

        private double m_Volume;

        public double Volume
        {
            get { return m_Volume; }
            set { Set(ref m_Volume, value); }
        }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            await Task.CompletedTask;
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

        private DelegateCommand m_SetPlay;

        public DelegateCommand SetPlay
            => m_SetPlay ?? (m_SetPlay = new DelegateCommand(() =>
            {
                Network.LoadUrl(string.Format("{0}set/Radio?Music={1}", ConfigurationValues.BaseUrl,
                    Play ? "play" : "stop"));
            }));
    }
}