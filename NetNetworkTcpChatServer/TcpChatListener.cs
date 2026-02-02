using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetNetworkTcpChatServer
{
    public class TcpChatListener
    {
        readonly IPAddress address;
        readonly int port;

        TcpListener listener;
        List<TcpChatClient> clients;

        public TcpChatListener(int port = 8000)
        {
            this.address = Dns.GetHostAddresses(Dns.GetHostName(), AddressFamily.InterNetwork)[0];
            this.port = port;

            listener = new TcpListener(IPAddress.Any, this.port);
            clients = new List<TcpChatClient>();
        }

        public async Task ListenAsync()
        {

        }

        public async Task SendMessageToAllAsync(string message, string id)
        {

        }

        public async Task SendMessageToClientAsync(string message, string frimId, string toId)
        {

        }

        void RemoveClient(string id)
        {

        }

        void Disconnect()
        {

        }
    }
}
