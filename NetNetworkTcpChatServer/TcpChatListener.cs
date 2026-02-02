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
            try
            {
                listener.Start();
                Console.WriteLine($"Server {listener.LocalEndpoint.ToString()} starting. Wait connections...");

                while (true)
                {
                    TcpClient tcpClient = await listener.AcceptTcpClientAsync();
                    TcpChatClient client = new TcpChatClient(tcpClient, this);
                    clients.Add(client);
                    Task.Run(client.ProcessAsync);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Disconnect();
            }
        }

        public async Task SendMessageToAllAsync(string message, string id)
        {
            string fromName = clients.FirstOrDefault(c => c.Id == id)!.Name;
            foreach (var client in clients)
            {
                if(client.Id != id)
                {
                    await client.Writer.WriteLineAsync($"From [{fromName}] Message: {message}");
                    await client.Writer.FlushAsync();
                }
            }
        }

        public async Task SendMessageToClientAsync(string message, string fromId, string toId)
        {
            string fromName = clients.FirstOrDefault(c => c.Id == fromId)!.Name;
            var toClient = clients.FirstOrDefault(c => c.Id == toId);

            await toClient!.Writer.WriteLineAsync($"From [{fromName}] Message: {message}");
            await toClient!.Writer.FlushAsync();
        }

        public void RemoveClient(string id)
        {
            var client = clients.FirstOrDefault(c => c.Id == id);
            if(client is not null)
                clients.Remove(client);
            client?.Close();
        }

        void Disconnect()
        {
            foreach(var client in clients)
                client.Close();
            listener.Stop();
        }
    }
}
