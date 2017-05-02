using DevilHome.Common.Implementations.Values;
using DevilHome.Common.Interfaces.Values;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Template10.Common;
using Template10.Services.NavigationService;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Template10.Mvvm;

namespace DevilHome.UWP.MainView.ViewModels
{
    public class RoomControlPageViewModel : DevilHomeBase
    {
        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            SetBusy(true);

            WindowWrapper.Current().Dispatcher.Dispatch(() =>
            {
                try
                {
                    RoomList = new List<IRoomValue>
                    {
                        new RoomValue
                        {
                            Id = 1,
                            Name = "Wohnzimmer",
                            Description = "",
                            PoweroutletValues = new List<IPoweroutletValue>
                            {
                                new PoweroutletValue
                                {
                                    Id = 1,
                                    Fk_Raum_Id = 1,
                                    DeviceCode = "10000",
                                    HausCode = "54321",
                                    Name = "Ecklicht",
                                    Description = ""
                                },
                                new PoweroutletValue
                                {
                                    Id = 2,
                                    Fk_Raum_Id = 1,
                                    DeviceCode = "01000",
                                    HausCode = "54321",
                                    Name = "PC Sophia",
                                    Description = ""
                                },
                                new PoweroutletValue
                                {
                                    Id = 3,
                                    Fk_Raum_Id = 1,
                                    DeviceCode = "00010",
                                    HausCode = "11111",
                                    Name = "PC Björn",
                                    Description = ""
                                }
                            }
                        },
                        new RoomValue
                        {
                            Id = 2,
                            Name = "Schlafzimmer",
                            Description = ""
                        },
                        new RoomValue
                        {
                            Id = 3,
                            Name = "Flur",
                            Description = ""
                        },
                        new RoomValue
                        {
                            Id = 4,
                            Name = "Gamingroom",
                            Description = ""
                        },
                        new RoomValue
                        {
                            Id = 5,
                            Name = "Küche",
                            Description = ""
                        },
                        new RoomValue
                        {
                            Id = 6,
                            Name = "Bad",
                            Description = ""
                        }
                    };

                    GetAndPoweroutletValues();
                    GetAndAddSensorValues();
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

        private List<IRoomValue> m_RoomList;

        public List<IRoomValue> RoomList
        {
            get { return m_RoomList; }
            set { Set(ref m_RoomList, value); }
        }

        private List<IPoweroutletValue> m_PoweroutletValues;

        public List<IPoweroutletValue> PoweroutletValues
        {
            get { return m_PoweroutletValues; }
            set { Set(ref m_PoweroutletValues, value); }
        }

        private ICommand m_SetOutletStatusOn;
        public ICommand SetOutletStatusOn => m_SetOutletStatusOn ?? (m_SetOutletStatusOn = new Executor(true));

        private ICommand m_SetOutletStatusOff;
        public ICommand SetOutletStatusOff => m_SetOutletStatusOff ?? (m_SetOutletStatusOff = new Executor(false));

        private ICommand m_GoToDiagrammView;

        public ICommand GoToDiagrammView
            =>
            m_GoToDiagrammView ??
            (m_GoToDiagrammView = new DelegateCommand<object>(a => NavigationService.Navigate(typeof(Views.Dialogs.SensorDataDiagrammPage), a)));

        private async void GetAndAddSensorValues()
        {
            try
            {
                string json = Network.LoadUrl($"{ConfigurationValues.BaseUrl}get/sensor", null, 10000);
                List<SensorValue> sensorValues = JsonConvert.DeserializeObject<List<SensorValue>>(json);
                RoomList.ForEach(x => x.SensorValues.AddRange(sensorValues.Where(y => y.Fk_Raum_Id == x.Id).ToList()));
            }
            catch (Exception)
            {
                await new MessageDialog("Beim Holen der Sensordaten ist ein Fehler aufgetreten!", "Sorry :(").ShowAsync();
            }
            
        }

        private void GetAndPoweroutletValues()
        {

        }

    }

    class Executor : DevilHomeBase, ICommand
    {
        private bool m_Status;
        public Executor(bool status)
        {
            m_Status = status;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            IPoweroutletValue poweroutletValue = parameter as IPoweroutletValue;
            string s = m_Status ? "1" : "0";

            if (poweroutletValue != null && !m_Status && poweroutletValue.Name.StartsWith("PC"))
            {
                MessageDialog dialog = new MessageDialog($"Wollen sie \"{poweroutletValue.Name}\" wirklich ausschalten?", "Achtung");
                dialog.Commands.Add(new UICommand("Ja"));
                dialog.Commands.Add(new UICommand("Nein"));

                IUICommand result = await dialog.ShowAsync();

                if (result.Label == "Nein")
                {
                    return;
                }
            }

            try
            {
                if (poweroutletValue != null)
                    Network.LoadUrl($"{ConfigurationValues.BaseUrl}set/power?outlet={poweroutletValue.HausCode}-{poweroutletValue.DeviceCode}-{s}");
            }
            catch (Exception e)
            {
                await new MessageDialog(e.Message, "Fehler").ShowAsync();
            }
        }

        /// <summary>Tritt ein, wenn Änderungen auftreten, die sich auf die Ausführung des Befehls auswirken.</summary>
        public event EventHandler CanExecuteChanged;
    }
}