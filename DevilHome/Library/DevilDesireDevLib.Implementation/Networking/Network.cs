using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DevilDesireDevLib.Interfaces.Networking;

namespace DevilDesireDevLib.Implementation.Networking
{
    public class Network : INetwork
    {
        public string LoadUrl(string url, CookieContainer cookieContainer = null, int timeout = 10000)
        {
            return LoadUrlPost(url, null, cookieContainer, timeout);
        }

        public string LoadUrlPost(string url, Dictionary<string, string> postParams, CookieContainer cookieContainer = null, int timeout = 10000)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(url);

            if (cookieContainer != null)
            {
                httpWebRequest.CookieContainer = cookieContainer;
            }

            if (postParams != null)
            {
                StringBuilder postParamBuilder = new StringBuilder();

                foreach (string key in postParams.Keys)
                {
                    postParamBuilder.Append(string.Format("{0}={1}&", UrlEncode(key), UrlEncode(postParams[key])));
                }

                byte[] dataBytes = StringToAscii(postParamBuilder.ToString());

                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";

                Task<Stream> requestStreamAsync = httpWebRequest.GetRequestStreamAsync();
                if (!requestStreamAsync.Wait(timeout))
                {
                    throw new IOException(string.Format("Failed to send request for url {0}", url));
                }

                Stream requestStream = requestStreamAsync.Result;

                requestStream.Write(dataBytes, 0, dataBytes.Length);
                requestStream.Flush();
                requestStream.Dispose();
            }
            else
            {
                httpWebRequest.Method = "GET";
            }

            Task<WebResponse> asyncResponse = httpWebRequest.GetResponseAsync();

            if (!asyncResponse.Wait(timeout))
            {
                throw new IOException(string.Format("No response for url {0}", url));
            }

            using (StreamReader streamReader = new StreamReader(asyncResponse.Result.GetResponseStream()))
            {
                return streamReader.ReadToEnd();
            }

        }

        private string UrlEncode(string data)
        {
            if (data.Length > 327000)
            {
                StringBuilder dataBuilder = new StringBuilder(data.Length);
                int blocks = data.Length/32700;

                for (int i = 0; i < blocks; i++)
                {
                    dataBuilder.Append(i < blocks 
                        ? Uri.EscapeDataString(data.Substring(32700*i, 32700))
                        : Uri.EscapeDataString(data.Substring(32700*i)));
                }

                return dataBuilder.ToString();
            }

            return Uri.EscapeDataString(data);
        }

        private byte[] StringToAscii(string s)
        {
            byte[] bytes = new byte[s.Length];

            for (int i = 0; i < s.Length; i++)
            {
                char ch = s[i];
                if (ch < 0x7f)
                {
                    bytes[i] = (byte) ch;
                    continue;
                }

                bytes[i] = (byte) '?';
            }

            return bytes;
        }
    }
}