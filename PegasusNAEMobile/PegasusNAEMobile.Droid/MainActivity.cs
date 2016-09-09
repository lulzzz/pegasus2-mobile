using System;
using System.Net.WebSockets;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using PegasusNAEMobile;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Android.Util;
using Octane.Xam.VideoPlayer.Android;


namespace PegasusNAEMobile.Droid
{
    [Activity(Label = "PegasusMission NAE", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity, IWebSocketClient
    {
        public event WebSocketEventHandler OnClose;
        public event WebSocketErrorHandler OnError;
        public event WebSocketMessageHandler OnMessage;
        public event WebSocketEventHandler OnOpen;

        private const int receiveChunkSize = 256;
        private ClientWebSocket client;
        private Queue<byte[]> messageQueue;

        public bool IsConnected
        {
            get
            {
                if (this.client == null)
                {
                    return false;
                }
                else
                {
                    return this.client.State == WebSocketState.Open;
                }
            }
        }
        public Task ConnectAsync(string host)
        {
            return ConnectAsync(host, null, null);
        }

        public async Task ConnectAsync(string host, string subprotocol, string securityToken)
        {
            client.Options.SetBuffer(1024, 1024);
            client.Options.KeepAliveInterval = TimeSpan.FromMilliseconds(5000);

            if (!string.IsNullOrEmpty(subprotocol))
            {
                this.client.Options.AddSubProtocol(subprotocol);
            }

            if (!string.IsNullOrEmpty(securityToken))
            {
                client.Options.SetRequestHeader("Authorization", String.Format("Bearer {0}", securityToken));
            }

            await client.ConnectAsync(new Uri(host), CancellationToken.None);

            Thread receiveLoopThread = new Thread(ReceiveLoopAsync);
            receiveLoopThread.Start();

            if (OnOpen != null)
            {
                OnOpen(this, "Web socket is opened.");
            }
        }

        public async Task CloseAsync()
        {
            await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Web Socket closed by client.", CancellationToken.None);
            // TODO: reap thread

            if (OnClose != null)
            {
                OnClose(this, "Web socket is closed.");
            }
        }
        private async void ReceiveLoopAsync()
        {
            Exception exception = null;
            WebSocketReceiveResult result = null;

            while (client.State == WebSocketState.Open && exception == null)
            {
                int remainingLength = 0;

                try
                {
                    byte[] prefix = new byte[4];
                    
                    int prefixSize = 4;
                    int offset = 0;
                    while (offset < prefix.Length)
                    {
                        byte[] array = new byte[prefixSize - offset];
                        result = await client.ReceiveAsync(new ArraySegment<byte>(array), CancellationToken.None);
                        Buffer.BlockCopy(array, 0, prefix, offset, result.Count);
                        offset += result.Count;
                        if (prefix.Length < 4)
                        {
                            //Trace.TraceInformation("Prefix too short.");
                        }
                    }

                    prefix = BitConverter.IsLittleEndian ? prefix.Reverse().ToArray() : prefix;
                    remainingLength = BitConverter.ToInt32(prefix, 0);

                    int index = 0;
                    byte[] message = new byte[remainingLength];
                    //                    do
                    //                    {
                    int bufferSize = remainingLength > receiveChunkSize ? receiveChunkSize : remainingLength;
                    byte[] buffer = new byte[bufferSize];

                    Label_1E9:
                    result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    //Trace.WriteLine("Received " + result.Count + " bytes, remainingLength: " + remainingLength);
                    if (result.Count < remainingLength)
                    {
                        Buffer.BlockCopy(buffer, 0, message, index, result.Count);
                        remainingLength = remainingLength - result.Count;
                        index += result.Count;
                        goto Label_1E9;
                    }
                    else
                    {
                        Buffer.BlockCopy(buffer, 0, message, index, result.Count);
                       
                        remainingLength = remainingLength - result.Count;
                    }

                   

                    if (!result.EndOfMessage)
                    {
                        if (OnError != null)
                        {
                            OnError(this, new WebSocketException("Expected EOF for Web Socket message received."));
                        }
                        else
                        {
                            throw new WebSocketException("Expected EOF for Web Socket message received.");
                        }
                    }

                    if (OnMessage != null)
                    {
                        OnMessage(this, message);
                    }
                }
                catch (Exception ex)
                {
                    exception = ex;
                    
                    break;
                }
            }

            if (exception != null)
            {
                if (OnError != null)
                {
                    OnError(this, new WebSocketException(exception.Message));
                }
            }
        }

        /// <summary>
        /// Send a message through the websocket
        /// </summary>
        /// <param name="messageBytes"></param>
        public void Send(byte[] messageBytes)
        {
            this.messageQueue.Enqueue(messageBytes);
            Exception exception = null;
            try
            {
                while (this.messageQueue.Count > 0)
                {
                    byte[] message = this.messageQueue.Dequeue();

                    byte[] prefix = BitConverter.IsLittleEndian ? BitConverter.GetBytes(message.Length).Reverse().ToArray() : BitConverter.GetBytes(message.Length);
                    byte[] messageBuffer = new byte[message.Length + prefix.Length];
                    Buffer.BlockCopy(prefix, 0, messageBuffer, 0, prefix.Length);
                    Buffer.BlockCopy(message, 0, messageBuffer, prefix.Length, message.Length);

                    int remainingLength = messageBuffer.Length;
                    int index = 0;
                    do
                    {
                        int bufferSize = remainingLength > receiveChunkSize ? receiveChunkSize : remainingLength;
                        byte[] buffer = new byte[bufferSize];
                        Buffer.BlockCopy(messageBuffer, index, buffer, 0, bufferSize);
                        index += bufferSize;
                        remainingLength = messageBuffer.Length - index;
                        try
                        {
                            client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Binary, remainingLength == 0, CancellationToken.None).Wait();
                        }
                        catch (Exception ex)
                        {
                            //Trace.TraceWarning("Web Socket send fault.");
                            //Trace.TraceError(ex.Message);
                            throw;

                        }
                    } while (remainingLength > 0);
                }
            }
            catch (Exception ex)
            {
                exception = ex;
                //Trace.TraceWarning("Web Socket exception during send.");
                //Trace.TraceError(ex.Message);
            }

            if (exception != null)
            {
                CloseAsync().Wait();

                if (OnClose != null)
                {
                    OnClose(this, "Client forced to close.");
                }

                throw exception;
            }
        }

        public static Task<bool> FromWaitHandle(WaitHandle handle, TimeSpan timeout)
        {
            // Handle synchronous cases.
            var alreadySignalled = handle.WaitOne(0);
            if (alreadySignalled)
                return Task.FromResult(true);
            if (timeout == TimeSpan.Zero)
                return Task.FromResult(false);

            // Register all asynchronous cases.
            var tcs = new TaskCompletionSource<bool>();
            var threadPoolRegistration = ThreadPool.RegisterWaitForSingleObject(handle,
                (state, timedOut) => ((TaskCompletionSource<bool>)state).TrySetResult(!timedOut),
                tcs, timeout, true);
            return tcs.Task;
        }

        public async Task SendAsync(byte[] messageBytes)
        {
            var ev = new EventWaitHandle(false, EventResetMode.AutoReset);
            ThreadPool.QueueUserWorkItem(unused => { Send(messageBytes); ev.Set(); });
            await FromWaitHandle(ev, new TimeSpan(-1));
        }
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            //EngagementConfiguration engagementConfiguration = new EngagementConfiguration();
            //engagementConfiguration.ConnectionString = "Endpoint=PegasusMissions.device.mobileengagement.windows.net;SdkKey=8793710d17cf2f66578a557eedb0e00b;AppId=cup000171";
            //EngagementAgent.Init(engagementConfiguration);

            this.client = new ClientWebSocket();
            this.messageQueue = new Queue<byte[]>();
            PegasusNAEMobile.App.Init(this);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            //ImageCircleRenderer.Init();
            FormsVideoPlayer.Init("EC98EB3C1D9B07A03E67203528E3F3055FD5AB36");          
            App.SetScreenHeightAndWidth((int)(Resources.DisplayMetrics.HeightPixels / Resources.DisplayMetrics.Density),(int)(Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density) );  // Get device independednt pixels
            LoadApplication(new App());
        }

       
    }
}

