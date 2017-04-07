using System.ComponentModel;
using System.Linq;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Composition;
using Template10.Common;
using Template10.Controls;
using Template10.Services.NavigationService;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Template10.Mvvm;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Toolkit.Uwp.UI.Animations;

namespace DevilHome.UWP.MainView.Views
{
    public sealed partial class Shell : Page
    {
        public static Shell Instance { get; set; }
        public static HamburgerMenu HamburgerMenu => Instance.MyHamburgerMenu;
        Services.SettingsServices.SettingsService _settings;

        public Shell()
        {
            Instance = this;
            InitializeComponent();
            _settings = Services.SettingsServices.SettingsService.Instance;
            SetImageBlur();
        }

        public async void SetImageBlur()
        {
            await imgBackground.Blur(duration: 10, delay: 0, value: 10).StartAsync();

            //using (var stream = await mainGrid.RenderToRandomAccessStream())
            //{
            //    var device = new CanvasDevice();
            //    var bitmap = await CanvasBitmap.LoadAsync(device, stream);

            //    var renderer = new CanvasRenderTarget(device,
            //                                          bitmap.SizeInPixels.Width,
            //                                          bitmap.SizeInPixels.Height,
            //                                          bitmap.Dpi);

            //    using (var ds = renderer.CreateDrawingSession())
            //    {
            //        var blur = new DropShadow()
            //        {
                        
            //        };

            //        ds.DrawImage(blur);
            //    }

            //    stream.Seek(0);
            //    await renderer.SaveAsync(stream, CanvasBitmapFileFormat.Png);

            //    BitmapImage image = new BitmapImage();
            //    image.SetSource(stream);
            //    //imgBackground.Source = image;
            //}
        }

        public Shell(INavigationService navigationService) : this()
        {
            SetNavigationService(navigationService);
        }

        public void SetNavigationService(INavigationService navigationService)
        {
            MyHamburgerMenu.NavigationService = navigationService;
            HamburgerMenu.RefreshStyles(_settings.AppTheme, true);
            HamburgerMenu.IsFullScreen = _settings.IsFullScreen;
            HamburgerMenu.HamburgerButtonVisibility = _settings.ShowHamburgerButton ? Visibility.Visible : Visibility.Collapsed;
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

