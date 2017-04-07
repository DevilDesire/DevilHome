using System;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using DevilDesireDevLib.Interfaces;

namespace DevilDesireDevLib.Implementation.Converter
{
    public class LocalPathToImageConverter : IConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string path = value as string;
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            if (path.StartsWith("\\"))
            {
                path = path.Remove(0, 1);
            }
            StorageFile file;
            try
            {
                file = ApplicationData.Current.LocalFolder.GetFileAsync(path).GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                try
                {
                    file = ApplicationData.Current.LocalFolder.GetFileAsync("Avatar\\" + path).GetAwaiter().GetResult();
                }
                catch (Exception)
                {
                    return null;
                }
            }

            BitmapImage img = new BitmapImage();

            using (var stream = (FileRandomAccessStream)file.OpenAsync(FileAccessMode.Read).GetAwaiter().GetResult())
            {
                img.SetSource(stream);
            }

            return img;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}