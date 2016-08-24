using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

using System.Threading.Tasks;
using PiraeusWebSockets;

namespace PiraeusWebSocketClient
{
    public class WebSocketClient : IWebSocketClient
    {
        private const int receiveChunkSize = 1024;
        
        public event WebSocketEventHandler OnClose;
        public event WebSocketErrorHandler OnError;
        public event WebSocketMessageHandler OnMessage;
        public event WebSocketEventHandler OnOpen;
        
        public Task ConnectAsync(string host)
        {
            throw new NotImplementedException();
        }

        public Task ConnectAsync(string host, string subprotocol, string securityToken)
        {
            throw new NotImplementedException();
        }

        public Task SendAsync(byte[] message)
        {
            throw new NotImplementedException();
        }
    }
}
