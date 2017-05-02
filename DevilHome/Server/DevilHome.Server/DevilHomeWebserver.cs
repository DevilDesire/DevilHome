using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using DevilHome.Common.Implementations.Utils;
using DevilHome.Common.Implementations.Values;
using DevilHome.Common.Interfaces.Enums;
using DevilHome.Common.Interfaces.Values;
using Newtonsoft.Json;

namespace DevilHome.Server
{
    internal class DevilHomeWebserver
    {
        private AppServiceConnection m_AppServiceConnection;
        private const uint BUFFER_SIZE = 8192;
        private string m_Name;

        public DevilHomeWebserver(AppServiceConnection connection)
        {
            m_AppServiceConnection = connection;
            m_AppServiceConnection.RequestReceived += ConnectionRequestReceived;
        }

        private void ConnectionRequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            m_Name = (string) args.Request.Message.First().Value;
        }

        public async void Start()
        {
            StreamSocketListener listener = new StreamSocketListener();
            try
            {
                await listener.BindServiceNameAsync(ConfigurationValues.Port);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            listener.ConnectionReceived += async (sender, args) =>
            {
                string exMessage = "Alles ok";
                bool hasResponse = false;
                try
                {
                    StringBuilder request = new StringBuilder();

                    using (IInputStream input = args.Socket.InputStream)
                    {
                        byte[] data = new byte[BUFFER_SIZE];
                        IBuffer buffer = data.AsBuffer();
                        uint dataRead = BUFFER_SIZE;

                        while (dataRead == BUFFER_SIZE)
                        {
                            await input.ReadAsync(buffer, BUFFER_SIZE, InputStreamOptions.Partial);
                            request.Append(Encoding.UTF8.GetString(data, 0, data.Length));
                            dataRead = buffer.Length;
                        }
                    }

                    IQueryValue query = GetQuery(request);
                    string json = JsonConvert.SerializeObject(query);
                    try
                    {
                        AppServiceResponse response = await m_AppServiceConnection.SendMessageAsync(new ValueSet
                        {
                            new KeyValuePair<string, object>("Query", json)
                        });

                        if (response != null && response.Status == AppServiceResponseStatus.Success)
                        {
                            try
                            {

                                exMessage = response.Message.First().Value.ToString();
                            }
                            catch (Exception ex)
                            {
                                exMessage = $"{ex.Message}\r\nKonnte die Daten nicht abrufen :/";
                            }
                            
                            
                            hasResponse = true;
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Sende problem: " + ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    exMessage = ex.Message + "\r\n";
                }

                using (IOutputStream output = args.Socket.OutputStream)
                {
                    using (Stream response = output.AsStreamForWrite())
                    {
                        string name = string.IsNullOrEmpty(m_Name) ? "Background" : m_Name;
                        byte[] html = Encoding.UTF8.GetBytes(hasResponse ? exMessage : $"<html><head><title>Background Message</title></head><body>Hello {name} from the background process!<br/>{exMessage}</body></html>");
                        using (MemoryStream bodyStream = new MemoryStream(html))
                        {
                            string header = $"HTTP/1.1 200 OK\r\nContent-type: application/json\r\nContent-Length: {bodyStream.Length}\r\nConnection: close\r\n\r\n";
                            byte[] headerArray = Encoding.UTF8.GetBytes(header);
                            await response.WriteAsync(headerArray, 0, headerArray.Length);
                            await bodyStream.CopyToAsync(response);
                            await response.FlushAsync();
                        }
                    }
                }
            };
        }

        private static IQueryValue GetQuery(StringBuilder request)
        {
            try
            {
                string[] requestLines = request.ToString().Split(' ');

                string url = requestLines.Length > 1
                ? requestLines[1] : string.Empty;

                Uri uri = new Uri("http://localhost" + url);
                string query = uri.Query.Replace("?", "");

                List<string> queryPathList = uri.AbsolutePath.Split('/').ToList();

                if (queryPathList[0] == "")
                {
                    queryPathList.RemoveAt(0);
                }

                QueryType queryType = queryPathList[2].Parse<QueryType>();
                RequestType reqType = queryPathList[1].Parse<RequestType>();

                return new QueryValue
                {
                    QueryType = queryType,
                    RequestType = reqType,
                    FunctionType = reqType == RequestType.Get ? FunctionType.Default : query.Split('=')[0].Parse<FunctionType>(),
                    Action = query.Split('=').Length == 2 ? query.Split('=')[1] : query.Split('=')[0]
                };
            }
            catch (Exception ex)
            {
                return new QueryValue
                {
                    QueryType = QueryType.Error,
                    Action = ex.Message
                };
            }
        }
    }
}