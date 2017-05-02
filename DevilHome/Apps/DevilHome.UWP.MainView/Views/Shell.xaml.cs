using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Template10.Controls;
using Template10.Services.NavigationService;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Toolkit.Uwp.UI.Animations;

namespace DevilHome.UWP.MainView.Views
{
    public sealed partial class Shell
    {
        public static Shell Instance { get; set; }
        public static HamburgerMenu HamburgerMenu => Instance.MyHamburgerMenu;
        Services.SettingsServices.SettingsService m_Settings;

        public Shell()
        {
            Instance = this;
            InitializeComponent();
            m_Settings = Services.SettingsServices.SettingsService.Instance;
            SetImageBlur();
            InitView();
        }

        public void InitView()
        {
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                mainGrid.Margin = new Thickness(0d);
            }
        }

        public async void SetImageBlur()
        {
            await imgBackground.Blur(duration: 10, delay: 0, value: 10).StartAsync();
        }

        public Shell(INavigationService navigationService) : this()
        {
            SetNavigationService(navigationService);
        }

        public void SetNavigationService(INavigationService navigationService)
        {
            MyHamburgerMenu.NavigationService = navigationService;
            HamburgerMenu.RefreshStyles(m_Settings.AppTheme, true);
            HamburgerMenu.IsFullScreen = m_Settings.IsFullScreen;
            HamburgerMenu.HamburgerButtonVisibility = m_Settings.ShowHamburgerButton ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    public static class Utils
    {
        public static async Task<IRandomAccessStream> RenderToRandomAccessStream(this UIElement element)
        {
            RenderTargetBitmap rtb = new RenderTargetBitmap();
            await rtb.RenderAsync(element);

            IBuffer pixelBuffer = await rtb.GetPixelsAsync();
            byte[] pixels = pixelBuffer.ToArray();

            DisplayInformation displayInformation = DisplayInformation.GetForCurrentView();

            InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream();
            BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);
            encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied, (uint)rtb.PixelWidth, (uint)rtb.PixelHeight,
                                 displayInformation.RawDpiX, displayInformation.RawDpiY, pixels);

            await encoder.FlushAsync();
            stream.Seek(0);

            return stream;
        }
    }
}

