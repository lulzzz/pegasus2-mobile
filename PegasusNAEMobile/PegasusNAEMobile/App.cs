using System;
using System.Text;
using PegasusData;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Piraeus.ServiceModel.Protocols.Coap;
using PegasusNAEMobile.ViewModels;
using ModernHttpClient;
using System.Net.Http;
using PegasusNAEMobile.Collections;

namespace PegasusNAEMobile
{
    public delegate void WebSocketEventHandler(object sender, string message);
    public delegate void WebSocketErrorHandler(object sender, Exception ex);
    public delegate void WebSocketMessageHandler(object sender, byte[] message);
    public interface IWebSocketClient
    {
        event WebSocketEventHandler OnOpen;
        event WebSocketEventHandler OnClose;
        event WebSocketErrorHandler OnError;
        event WebSocketMessageHandler OnMessage;       
        Task ConnectAsync(string host);

        Task ConnectAsync(string host, string subprotocol, string securityToken);
        
        //public Task CloseAsync();

        Task SendAsync(byte[] message);
    }

    public class App : Application
    {
        private ushort messageId;
        public static IWebSocketClient WebSocketClient { get; set; }
        public static void Init(IWebSocketClient client)
        {
            WebSocketClient = client; 
        }
        
        public App()
        {

            // The root page of your application
            Instance = this;
            CurrentVehicleTelemetry = new LiveTelemetryViewModel();
            NavigationPage page = new NavigationPage(new MainPage() { Title = "Pegasus Mission" })
            {
                BackgroundColor = Color.FromRgba(35, 35, 43, 1)
                
            };
            page.BarTextColor = Color.White;
            
            //page.BarBackgroundColor = Color.FromRgba(35, 35, 43, 1);
            
            MainPage = new NavigationPage(new MainPage());
            
        }       

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public static App Instance
        {
            get;
            private set;
        }

        public LiveTelemetryViewModel CurrentVehicleTelemetry
        {
            get;
            private set;
        }

        public async void ConnectWebSocketLiveTelemetry()
        {
            if (App.WebSocketClient != null)
            {
                try
                {
                    // GET security Token.

                    if (String.IsNullOrEmpty(Constants.SavedSecurityToken))
                    {
                        await LoadSecurityToken();
                    }
                    messageId = 1;
                    App.WebSocketClient.OnClose += WebSocketClient_OnClose;
                    App.WebSocketClient.OnError += WebSocketClient_OnError;
                    App.WebSocketClient.OnMessage += WebSocketClient_OnMessage;
                    App.WebSocketClient.OnOpen += WebSocketClient_OnOpen;
                    await App.WebSocketClient.ConnectAsync(Constants.LiveTelemetryHostUri, Constants.SubProtocol, Constants.SavedSecurityToken);    // Use the token to open the web socket connection.
                    
                    await SubscribeTopicAsync(Constants.TelemterySubscribeUri);     // Subscribe to the Telemetry Uri          
                }
                catch (Exception ex)
                {
                    //App.WebSocketClient.OnError
                }
            }            
        }

        private void WebSocketClient_OnOpen(object sender, string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
        }

        /// <summary>
        /// Invoked when a message is received on the opened WebSocket
        /// </summary>        
        private void WebSocketClient_OnMessage(object sender, byte[] message)
        {
            //throw new NotImplementedException();
            System.Diagnostics.Debug.WriteLine("Message Received");

            CoapMessage coapMessage = CoapMessage.DecodeMessage(message);
            string jsonString = Encoding.UTF8.GetString(coapMessage.Payload, 0, coapMessage.Payload.Length);

            if (coapMessage.ResourceUri.OriginalString == Constants.TelemteryPublishUri)
            {
                VehicleTelemetry vtr = VehicleTelemetry.Load(jsonString);
                var telemetry = JsonConvert.DeserializeObject<VehicleTelemetry>(jsonString);
                this.CurrentVehicleTelemetry.Data = telemetry;
            }
        }

        private void WebSocketClient_OnError(object sender, Exception ex)
        {
            //throw new NotImplementedException();
            Device.BeginInvokeOnMainThread(async () =>
            {
                // this.AppData.StatusMessage = "Web Socket error: " + ex.Message;
                //this.AppData.BusyCount--;
                App.WebSocketClient = null;
                // Always give us two seconds before trying to reconnect, in case
                // the phone is bringing up a connection.  This also solves an
                // animation problem.
                await Task.Delay(2000);
                ConnectWebSocketLiveTelemetry();
            });
        }

        private void WebSocketClient_OnClose(object sender, string message)
        {
            //throw new NotImplementedException();
            System.Diagnostics.Debug.WriteLine(message);
        }

        /// <summary>
        /// Used to store the device's height and width
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        public static void SetScreenHeightAndWidth(int height, int width)
        {
            try
            {
                Constants.ScreenHeight = height;
                Constants.ScreenWidth = width;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        /// <summary>
        /// GETs the security token needed to make future requests. 
        /// The token is stored in App Settings.
        /// </summary>
        /// <returns></returns>
        private async Task LoadSecurityToken()
        {
            string requestUriString = String.Format("{0}?key={1}", Constants.TokenWebApiUri, Constants.TokenSecret);
            try
            {
                //var httpclient = new HttpClient(new NativeMessageHandler());
                //var str = await httpclient.GetStreamAsync(requestUriString);
               // HttpResponseMessage response = await httpclient.GetAsync(new Uri(requestUriString));
                var request = WebRequest.CreateHttp(requestUriString);
                WebResponse responseObject = await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, request);
                using (var responseStream = responseObject.GetResponseStream())
                {
                    using (var sr = new StreamReader(responseStream))
                    {
                        string jsonString = await sr.ReadToEndAsync();
                        string token = JsonConvert.DeserializeObject<string>(jsonString);
                        Constants.SavedSecurityToken = token;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Subscribes to messages from a URI
        /// </summary>
        /// <param name="subscribeUri"></param>
        /// <returns></returns>
        private Task SubscribeTopicAsync(string subscribeUri)
        {
            Uri resourceUri = new Uri(subscribeUri);
            CoapRequest request = new CoapRequest(messageId++, RequestMessageType.NonConfirmable, MethodType.POST, resourceUri, MediaType.Json);
            byte[] message = request.Encode();
            return (App.WebSocketClient.SendAsync(message));
        }


        public async Task SendUserMessageAsync(string message)
        {
            //this.AppData.StatusMessage = "Sending...";
            //this.AppData.BusyCount++;

            UserMessage umessage = new UserMessage();
            umessage.Message = message;

            umessage.Id = Guid.NewGuid().ToString();
            string jsonString = UserMessage.UserMessageSerializer(umessage);
            byte[] payload = Encoding.UTF8.GetBytes(jsonString);
            //byte[] payload = UserMessage.ToCraftMessage(umessage);
            CoapRequest request = new CoapRequest(messageId++,
                                                    RequestMessageType.NonConfirmable,
                                                    MethodType.POST,
                                                    new Uri(Constants.UserMessageTopicUri),
                                                    MediaType.Json,
                                                    payload);

            byte[] messageBytes = request.Encode();
            try
            {
                await App.WebSocketClient.SendAsync(messageBytes);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Downloads file from given blob URL
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetFileFromBlob(string uri)
        {
            var request = WebRequest.CreateHttp(uri);
            try
            {
                WebResponse responseObject = await Task<WebResponse>.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, request);
                using (var responseStream = responseObject.GetResponseStream())
                {
                    using (var sr = new StreamReader(responseStream))
                    {
                        string jsonString = await sr.ReadToEndAsync();

                        return jsonString;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }        
    }
}
