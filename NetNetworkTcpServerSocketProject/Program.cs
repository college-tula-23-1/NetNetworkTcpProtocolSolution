using System.Net;
using System.Net.Sockets;
using System.Text;

// SERVER SOCKET

IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 8000);

using Socket listener = new(AddressFamily.InterNetwork,
                            SocketType.Stream,
                            ProtocolType.Tcp);

try
{
    listener.Bind(endPoint);
    Console.WriteLine("Server started");
    listener.Listen();

    while(true)
    {
        using Socket client = await listener.AcceptAsync();
        Console.WriteLine($"Server accept client {client.RemoteEndPoint}");

        string message = $"[{DateTime.Now.ToLongTimeString()}] Server message";
        byte[] messageBuffer = Encoding.UTF8.GetBytes(message);

        byte[] messageSize = BitConverter.GetBytes(messageBuffer.Length);

        await client.SendAsync(messageSize);
        await client.SendAsync(messageBuffer);
        Console.WriteLine($"Message sened to client {client.RemoteEndPoint}");

        byte[] buffer = new byte[1];
        List<byte> responseMessage = new List<byte>();
        int bytesCount = 0;

        while(true)
        {
            bytesCount = await client.ReceiveAsync(buffer);
            if (bytesCount == 0 || buffer[0] == '\n') break;

            responseMessage.AddRange(buffer.Take(bytesCount));
        } 

        string messageFromClient = Encoding.UTF8.GetString(responseMessage.ToArray());
        Console.WriteLine(messageFromClient);
    }
}
catch(Exception ex)
{
    Console.WriteLine(ex.Message);
}
