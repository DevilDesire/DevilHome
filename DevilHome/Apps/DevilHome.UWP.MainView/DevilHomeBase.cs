using DevilDesireDevLib.Interfaces.Networking;
using Template10.Mvvm;

namespace DevilHome.UWP.MainView
{
    public class DevilHomeBase : ViewModelBase
    {
        protected INetwork Network { get; } = DevilDesireDevLib.Implementation.Networking.Network.Instance;
    }
}