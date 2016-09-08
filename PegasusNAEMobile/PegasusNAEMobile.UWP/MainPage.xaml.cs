using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Networking.Sockets;
using Windows.Web.Http;
using PegasusNAEMobile;
using System.Threading.Tasks;
using Windows.Web;
using PegasusData;
using Newtonsoft.Json;
using Windows.Storage.Streams;
using System.Threading;
using Windows.Devices.Geolocation;

namespace PegasusNAEMobile.UWP
{
    public sealed partial class MainPage : IWebSocketClient, IGeolocator
    {
        private readonly Windows.Devices.Geolocation.Geolocator _locator;
        public event WebSocketEventHandler OnClose;
        public event WebSocketErrorHandler OnError;
        public event WebSocketMessageHandler OnMessage;
        public event WebSocketEventHandler OnOpen;
        public event EventHandler<PositionErrorEventArgs> PositionError;
        public event EventHandler<PositionEventArgs> PositionChanged;

        private MessageWebSocket client_LiveURI;
        private MessageWebSocket client;
        private const int receiveChunkSize = 1024;
        private bool connected = false;
        private Queue<byte[]> messageQueue;

        public double DesiredAccuracy
        {
            get
            {
                return (_locator.DesiredAccuracy == PositionAccuracy.Default) ? 100 : 10;
            }

            set
            {
                _locator.DesiredAccuracy = (value > 10) ? PositionAccuracy.Default : PositionAccuracy.High;
            }
        }


        public bool isGeoLocationEnabled
        {
            get
            {
                return _locator.LocationStatus != PositionStatus.Disabled;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool isGeoLocationAvailable
        {
            get
            {
                return _locator.LocationStatus != PositionStatus.NotAvailable;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public MainPage()
        {
            this.client = new MessageWebSocket();
            client_LiveURI = new MessageWebSocket();
            this.messageQueue = new Queue<byte[]>();
            _locator = new Windows.Devices.Geolocation.Geolocator();
            this.InitializeComponent();
            PegasusNAEMobile.App.Init(this);
            PegasusNAEMobile.App.SetScreenHeightAndWidth((int)Window.Current.Bounds.Height, (int)Window.Current.Bounds.Width);
            
            LoadApplication(new PegasusNAEMobile.App());
        }

        public async Task ConnectAsync(string host)
        {
            await ConnectAsync(host, null, null);
        }

        public async Task ConnectAsync(string host, string subprotocol, string securityToken)
        {
            this.client.Control.OutboundBufferSizeInBytes = 1024;

            if (!String.IsNullOrEmpty(subprotocol))
            {
                this.client.Control.SupportedProtocols.Add(subprotocol);
            }

            if (!String.IsNullOrEmpty(securityToken))
            {
                this.client.SetRequestHeader("Authorization", String.Format("Bearer {0}", securityToken));
            }

            this.client.MessageReceived += Client_MessageReceived;

            try
            {
                await client.ConnectAsync(new Uri(host));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Web Socket Failed to connect");
                connected = false;
                throw;
            }

            connected = true;

            if (OnOpen != null)
            {
                OnOpen(this, "Web Socket is Open");
            }
            //return true;
        }

       
        private void Client_MessageReceived(MessageWebSocket sender, MessageWebSocketMessageReceivedEventArgs args)
        {
            try
            {
                var messageReader = args.GetDataReader();
                int length = messageReader.ReadInt32();
                byte[] message = new byte[length];
                messageReader.ReadBytes(message);

                if (OnMessage != null)
                {
                    OnMessage(this, message);
                }
            }
            catch (Exception ex)
            {
                WebErrorStatus status = WebSocketError.GetStatus(ex.GetBaseException().HResult);
                //Trace.TraceWarning("Web Socket exception during send.");
                System.Diagnostics.Debug.WriteLine(ex.Message);

                client.Dispose();
                client = null;

                if (OnError != null)
                {
                    OnError(this, ex);
                }
            }
        }

        public async Task SendAsync(byte[] message)
        {
            try
            {
                using (var messageWriter = new DataWriter(this.client.OutputStream))
                {
                    messageWriter.WriteInt32(message.Length);
                    messageWriter.WriteBytes(message);
                    await messageWriter.StoreAsync();
                    messageWriter.DetachStream();
                }
            }
            catch (Exception ex)
            {
                WebErrorStatus status = WebSocketError.GetStatus(ex.GetBaseException().HResult);
                //Trace.TraceWarning("Web Socket exception during send.");
                System.Diagnostics.Debug.WriteLine(ex.Message);

                client.Dispose();
                client = null;

                if (OnError != null)
                {
                    OnError(this, ex);
                }
            }
        }

        public async Task<Position> GetPositionAsync(int timeout)
        {
            var accessStatus = await Geolocator.RequestAccessAsync();
            Position p = null;
            return p;
        }

        public Task<Position> GetPositionAsync(int timeout, bool includeHeading)
        {
            throw new NotImplementedException();
        }

        public Task<Position> GetPositionAsync(CancellationToken cancelToken)
        {
            throw new NotImplementedException();
        }

        public Task<Position> GetPositionAsync(CancellationToken cancelToken, bool includeHeading)
        {
            throw new NotImplementedException();
        }

        public Task<Position> GetPositionAsync(int timeout, CancellationToken cancelToken)
        {
            throw new NotImplementedException();
        }

        public Task<Position> GetPositionAsync(int timeout, CancellationToken cancelToken, bool includeHeading)
        {
            throw new NotImplementedException();
        }

        public void StartListening(uint minTime, double minDistance)
        {
            throw new NotImplementedException();
        }

        public void StartListening(uint minTime, double minDistance, bool includeHeading)
        {
            throw new NotImplementedException();
        }

        public void StopListening()
        {
            throw new NotImplementedException();
        }
    }
}
