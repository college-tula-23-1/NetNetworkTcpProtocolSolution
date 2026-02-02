using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace NetNetworkTcpChatServer
{
    public class TcpChatClient
    {
        string Id { get; } = Guid.NewGuid().ToString();

        public StreamWriter Writer { get; }
        public StreamReader Reader { get; }

        TcpClient client;
        TcpListener listener;

        public TcpChatClient(TcpClient client, TcpListener listener)
        {
            this.client = client;
            this.listener = listener;

            var stream = client.GetStream();
            Writer = new StreamWriter(stream);
            Reader = new StreamReader(stream);
        }
    }
}
