using DevilDesireDevLib.Interfaces.Networking;
using DevilHome.UWP.MainView.Views;
using Template10.Mvvm;

namespace DevilHome.UWP.MainView
{
    public class DevilHomeBase : ViewModelBase
    {
        protected INetwork Network { get; } = DevilDesireDevLib.Implementation.Networking.Network.Instance;

        protected void SetBusy(bool active)
        {
            Busy.SetBusy(active, active ? "Daten werden geladen..." : null);
        }
    }
}