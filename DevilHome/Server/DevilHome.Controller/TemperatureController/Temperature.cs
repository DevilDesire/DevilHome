using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Foundation.Collections;
using DevilHome.Common.Implementations.Values;
using DevilHome.Common.Interfaces.Values;
using Newtonsoft.Json;
using Sensors.Dht;

namespace DevilHome.Controller.TemperatureController
{
    internal class Temperature
    {
        private static GpioPin m_GpioPin;

        public void InitializeGpio()
        {
            m_GpioPin = GpioController.GetDefault().OpenPin(13, GpioSharingMode.Exclusive);

        }

        public async Task<string> ProcessingGetRequest(IQueryValue queryValue)
        {
            string response = null;
            try
            {
                Dht22 dht = new Dht22(m_GpioPin, GpioPinDriveMode.Input);
                double temperature = 0;
                double humidity = 0;
                bool isValid = false;

                while (!isValid)
                {
                    DhtReading reader = await dht.GetReadingAsync().AsTask();
                    temperature = reader.Temperature;
                    humidity = reader.Humidity;
                    isValid = reader.IsValid;
                }

                response = JsonConvert.SerializeObject(new List<ISensorValue>
                        {
                            new SensorValue
                            {
                                Fk_Raum_Id = 1,
                                Fk_SensorTyp_Id = 1,
                                Value = Convert.ToDecimal(temperature)
                            },
                            new SensorValue
                            {
                                Fk_Raum_Id = 1,
                                Fk_SensorTyp_Id = 2,
                                Value = Convert.ToDecimal(humidity)
                            }
                        });

                Debug.WriteLine($"Temperatur: {temperature}°C\r\nLuftfeuchtigkeit: {humidity}%");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return response;
        }

        public Task ProcessingSetRequest(IQueryValue queryValue)
        {
            return Task.CompletedTask;
        }
    }
}