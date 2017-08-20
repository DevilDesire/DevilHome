using Sensors.Dht;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Foundation;
using DevilDesireDevLib.Implementation.Networking;
using DevilHome.Common.Interfaces.Enums;

namespace DevilHome.Server.Services.TempertureService
{
    internal class TemperatureGetter
    {
        private static GpioPin _gpioPin;

        public IAsyncAction Run()
        {
            return Task.Run(() =>
            {
                _gpioPin = GpioController.GetDefault().OpenPin(13, GpioSharingMode.Exclusive);
                var timer = new Timer(CollectData, null, TimeSpan.Zero, new TimeSpan(0, 1, 0));
            }).AsAsyncAction();
        }

        private async void CollectData(object state)
        {
            try
            {
                if (_gpioPin != null)
                {
                    DateTime now = DateTime.Now;
                    Debug.WriteLine(now.ToString("HH:mm:ss"));

                    if (now.Minute == 0 || now.Minute == 15 || now.Minute == 30 || now.Minute == 45)
                    {
                        Dht22 dht = new Dht22(_gpioPin, GpioPinDriveMode.Input);
                        bool isValid = false;

                        double temperature = 0;
                        double humidity = 0;

                        while (!isValid)
                        {
                            DhtReading reader = await dht.GetReadingAsync().AsTask();
                            temperature = reader.Temperature;
                            humidity = reader.Humidity;
                            isValid = reader.IsValid;
                        }

                        Debug.WriteLine($"Sending: {temperature} / {humidity} as {now:dd.MM.yyyy HH:mm:ss}");
                        try
                        {
                            Network.Instance.LoadUrl(string.Format("http://192.168.178.47:9000/api/sensoren/adddata?raumname={0}&sensortyp={1}&value={2}", "Wohnzimmer", SensorEnum.Temperatur.GetStringValue(), temperature));
                            Network.Instance.LoadUrl(string.Format("http://192.168.178.47:9000/api/sensoren/adddata?raumname={0}&sensortyp={1}&value={2}", "Wohnzimmer", SensorEnum.Humidity.GetStringValue(), humidity));
                        }
                        catch (Exception)
                        {
                            Network.Instance.LoadUrl(string.Format("http://localhost:9000/api/sensoren/adddata?raumname={0}&sensortyp={1}&value={2}", "Wohnzimmer", SensorEnum.Temperatur.GetStringValue(), temperature));
                            Network.Instance.LoadUrl(string.Format("http://localhost:9000/api/sensoren/adddata?raumname={0}&sensortyp={1}&value={2}", "Wohnzimmer", SensorEnum.Humidity.GetStringValue(), humidity));
                        }
                        

                        await Task.Delay(new TimeSpan(0, 0, 30));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                IsolatedStorageFile isolatedStorageFile = IsolatedStorageFile.GetUserStoreForApplication();
                if (!isolatedStorageFile.FileExists("log.txt"))
                {
                    using (StreamWriter writer = new StreamWriter(new IsolatedStorageFileStream("log.txt", FileMode.Create, FileAccess.Write, isolatedStorageFile)))
                    {
                        await writer.WriteLineAsync(ex.Message);
                    }
                }
                else
                {
                    IsolatedStorageFileStream iso = isolatedStorageFile.OpenFile("log.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);

                    string existingText = "";

                    if (isolatedStorageFile.FileExists("log.txt"))
                    {
                        using (var reader = new StreamReader(iso))
                        {
                            existingText = reader.ReadToEnd();
                        }
                    }

                    using (StreamWriter writer = new StreamWriter(iso))
                    {
                        await writer.WriteLineAsync(existingText + "\r\n" + ex.Message);
                    }

                }
            }
        }
    }
}