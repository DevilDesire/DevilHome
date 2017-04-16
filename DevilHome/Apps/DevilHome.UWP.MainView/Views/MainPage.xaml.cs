using System;
using DevilHome.UWP.MainView.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
//using System.Management;

namespace DevilHome.UWP.MainView.Views
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            //ManagementClass mc = new ManagementClass("Win32_TemperatureProbe");
        }
    }
}
