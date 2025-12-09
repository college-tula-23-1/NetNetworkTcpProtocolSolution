using System.Net;
using System.Net.Sockets;
using System.Text;

// TCP LISTENER

IPAddress address = IPAddress.Any;
int port = 8000;
IPEndPoint serverEndPoint = new(address, port);

TcpListener server = new TcpListener(serverEndPoint);

try
{
    server.Start();
    Console.WriteLine("Server starting...");

    while(true)
    {
        using TcpClient client = await server.AcceptTcpClientAsync();
        Console.WriteLine($"Client {client.Client.RemoteEndPoint} connected");

        var stream = client.GetStream();
        string message = $"[{DateTime.Now.ToLongTimeString()}] Server message";
        byte[] messageBuffer = Encoding.UTF8.GetBytes(message);

        await stream.WriteAsync(messageBuffer);
        Console.WriteLine($"Send message to client {client.Client.RemoteEndPoint}");

        messageBuffer = new byte[1024];
        List<byte> responseMessage = new List<byte>();

        int bytesCount = 0;
        while(true)
        {
            bytesCount = await stream.ReadAsync(messageBuffer);
            if (bytesCount == 0) break;
            responseMessage.AddRange(messageBuffer);
        }
        string messageFromClient = Encoding.UTF8.GetString(responseMessage.ToArray());
        Console.WriteLine(messageFromClient);
    }
}
finally
{
    server.Stop();
}