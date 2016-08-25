using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PegasusData;
using Xamarin.Forms;
using PegasusNAEMobile.Pages;
using System.Threading.Tasks;
using System.Net.Http;
using PegasusNAEMobile.Helpers;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Piraeus.ServiceModel.Protocols.Coap;

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
            MainPage = new MainPage();
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

        private void WebSocketClient_OnMessage(object sender, byte[] message)
        {
            //throw new NotImplementedException();
            System.Diagnostics.Debug.WriteLine("Message Received");
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
        /// GETs the security token needed to make future requests. 
        /// The token is stored in App Settings.
        /// </summary>
        /// <returns></returns>
        private async Task LoadSecurityToken()
        {
            string requestUriString = String.Format("{0}?key={1}", Constants.TokenWebApiUri, Constants.TokenSecret);

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
    }
}
