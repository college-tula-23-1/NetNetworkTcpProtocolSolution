using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace NetNetworkTcpChatServer
{
    public class TcpChatClient
    {
        public string Id { get; } = Guid.NewGuid().ToString();
        public string? Name { get; set; }

        public StreamWriter Writer { get; }
        public StreamReader Reader { get; }

        TcpClient client;
        TcpChatListener listener;

        public TcpChatClient(TcpClient client, TcpChatListener listener)
        {
            this.client = client;
            this.listener = listener;

            var stream = client.GetStream();
            Writer = new StreamWriter(stream);
            Reader = new StreamReader(stream);
        }

        public async Task ProcessAsync()
        {
            try
            {
                this.Name = await Reader.ReadLineAsync();
                string? message = $"[{Name}] come in to chat";

                await listener.SendMessageToAllAsync(message, this.Id);
                Console.WriteLine(message);

                while(true)
                {
                    try
                    {
                        message = await Reader.ReadLineAsync();
                        if (message is null) continue;
                        message = $"From:{Name} Message: {message}";
                        await listener.SendMessageToAllAsync(message, this.Id);
                        Console.WriteLine(message);
                    }
                    catch
                    {
                        message = $"{Name} out from chat";
                        await listener.SendMessageToAllAsync(message, this.Id);
                        Console.WriteLine(message);
                        break;
                    }
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message); 
            }
            finally
            {
                listener.RemoveClient(this.Id);
            }
        }

        public void Close()
        {
            Writer.Close();
            Reader.Close();
            client.Close();
        }
    }
}
