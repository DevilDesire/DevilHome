using System.Collections.Generic;
using System.Net;

namespace DevilDesireDevLib.Interfaces.Networking
{
    public interface INetwork
    {
        string LoadUrl(string url, CookieContainer cookieContainer = null, int timeout = 3000);
        string LoadUrlPost(string url, Dictionary<string, string> postParams, CookieContainer cookieContainer = null, int timeout = 3000);
    }
}