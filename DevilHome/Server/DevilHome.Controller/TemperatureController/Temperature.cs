using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.UI.Xaml;
using DevilHome.Common.Implementations.Values;
using DevilHome.Common.Interfaces.Enums;
using DevilHome.Common.Interfaces.Values;
using DevilHome.Controller.Utils;
using DevilHome.Database.Interfaces.Values;
using Newtonsoft.Json;
using Sensors.Dht;

namespace DevilHome.Controller.TemperatureController
{
    internal class Temperature : ControllerBase
    {
        private static GpioPin m_GpioPin;
        private double m_Temperature;
        private double m_Humidity;
        // ReSharper disable once NotAccessedField.Local
        private Timer m_Timer;

        private DispatcherTimer m_DispatcherTimer;

        public async void InitializeGpio()
        {
            try
            {
                m_GpioPin = GpioController.GetDefault().OpenPin(13, GpioSharingMode.Exclusive);
                m_Timer = new Timer(GetTemperature, null, TimeSpan.Zero, new TimeSpan(0, 15, 0));
            }
            catch (Exception ex)
            {
                await Logger.LogError(ex, PluginEnum.TemperatureController);
            }
            
        }

        private async void GetTemperature(object o)
        {
            try
            {
                if (m_GpioPin != null)
                {
                    Dht22 dht = new Dht22(m_GpioPin, GpioPinDriveMode.Input);
                    bool isValid = false;

                    while (!isValid)
                    {
                        DhtReading reader = await dht.GetReadingAsync().AsTask();
                        m_Temperature = reader.Temperature;
                        m_Humidity = reader.Humidity;
                        isValid = reader.IsValid;
                    }

                    DatabaseBase.InsertSensorValue(SensorEnum.Temperatur.GetStringValue(), "Wohnzimmer", "Temperatur", m_Temperature);
                    DatabaseBase.InsertSensorValue(SensorEnum.Humidity.GetStringValue(), "Wohnzimmer", "Luftfeuchtigkeit", m_Humidity);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, PluginEnum.TemperatureController);
            }
            
        }

        public async Task<string> ProcessingGetRequest(IQueryValue queryValue)
        {
            string response = null;

            try
            {
                if (string.IsNullOrEmpty(queryValue.Action))
                {
                    response = JsonConvert.SerializeObject(new List<ISensorValue>
                        {
                            new SensorValue
                            {
                                Id = 1,
                                Fk_Raum_Id = 1,
                                Fk_SensorTyp_Id = 1,
                                LastValue = DbSensorData.GetLastValueBySensorId(1)
                            },
                            new SensorValue
                            {
                                Id = 2,
                                Fk_Raum_Id = 1,
                                Fk_SensorTyp_Id = 2,
                                LastValue = DbSensorData.GetLastValueBySensorId(2)
                            }
                        });
                }
                else
                {
                    List<ISensorData> values = DbSensorData.GetValuesBySensorId(Convert.ToInt16(queryValue.Action));
                    List<ISensorDataValue> convertedValues = new List<ISensorDataValue>();
                    values.ForEach(x => convertedValues.Add(new SensorDataValue{Date = Convert.ToDateTime(x.Date), Value = x.Value, Fk_Sensor_Id = x.Fk_Sensor_Id}));
                    response = JsonConvert.SerializeObject(convertedValues);
                }

                Debug.WriteLine($"Temperatur: {m_Temperature}°C\r\nLuftfeuchtigkeit: {m_Humidity}%");
            }
            catch (Exception ex)
            {
                await Logger.LogError(ex, PluginEnum.TemperatureController);
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