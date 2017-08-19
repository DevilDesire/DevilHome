using Sensors.Dht;
using System;
using System.Diagnostics;
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
                var timer = new Timer(CollectData, null, TimeSpan.Zero, new TimeSpan(0, 15, 0));
            }).AsAsyncAction();
        }

        private async void CollectData(object state)
        {
            if (_gpioPin != null)
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

                try
                {
                    Network.Instance.LoadUrl(string.Format("http://localhost:9000/api/sensoren/adddata?raumname={0}&sensortyp={1}&value={2}", "Wohnzimmer", SensorEnum.Temperatur.GetStringValue(), temperature));
                    Network.Instance.LoadUrl(string.Format("http://localhost:9000/api/sensoren/adddata?raumname={0}&sensortyp={1}&value={2}", "Wohnzimmer", SensorEnum.Humidity.GetStringValue(), humidity));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }
    }
}