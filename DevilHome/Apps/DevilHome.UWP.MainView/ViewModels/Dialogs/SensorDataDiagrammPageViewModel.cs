using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using DevilHome.Common.Implementations.Values;
using DevilHome.Common.Interfaces.Values;
using Newtonsoft.Json;
using Template10.Common;
using Template10.Services.NavigationService;

namespace DevilHome.UWP.MainView.ViewModels.Dialogs
{
    public class SensorDataDiagrammPageViewModel : DevilHomeBase
    {
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            SetBusy(true);

            WindowWrapper.Current().Dispatcher.Dispatch(() =>
            {
                try
                {
                    ISensorValue sensor = parameter as ISensorValue;
                    Einheit = sensor != null && sensor.Fk_SensorTyp_Id == 1 ? "°C" : "%";
                    GetSensorData(sensor);
                    ComboItems = GetItems();
                    string roomName = GetRoomName(sensor);
                    PageTitel = string.Format("{0} {1}", sensor != null && sensor.Fk_SensorTyp_Id == 1 ? "Temperatur" : "Luftfeuchtigkeit", roomName);
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

        private List<ISensorDataValue> m_SensorValues;

        public List<ISensorDataValue> SensorValues
        {
            get { return m_SensorValues; }
            set { Set(ref m_SensorValues, value); }
        }

        private List<ComboBoxItem> m_ComboItems;

        public List<ComboBoxItem> ComboItems
        {
            get { return m_ComboItems; }
            set { Set(ref m_ComboItems, value); }
        }

        private string m_PageTitel;

        public string PageTitel
        {
            get { return m_PageTitel; }
            set { Set(ref m_PageTitel, value); }
        }

        private string m_Einheit;

        public string Einheit
        {
            get { return m_Einheit; }
            set { Set(ref m_Einheit, value); }
        }

        private List<ComboBoxItem> GetItems()
        {
            return new List<ComboBoxItem>
            {
                new ComboBoxItem { Content = "Letzte 3 Stunden" },
                new ComboBoxItem { Content = "Letzte 6 Stunden" },
                new ComboBoxItem { Content = "Letzte 12 Stunden" },
                new ComboBoxItem { Content = "Letzte 24 Stunden" },
                new ComboBoxItem { Content = "Benutzerdefinierter Zeitraum" },
            };
        }

        private void GetSensorData(ISensorValue value)
        {
            string json = Network.LoadUrl($"{ConfigurationValues.BaseUrl}get/sensor?id={value.Id}", null, 10000);
            SensorValues = JsonConvert.DeserializeObject<List<SensorDataValue>>(json).Cast<ISensorDataValue>().ToList();
        }

        // ReSharper disable once UnusedParameter.Local
        private string GetRoomName(ISensorValue value)
        {
            return "Wohnzimmer";
        }
    }
}