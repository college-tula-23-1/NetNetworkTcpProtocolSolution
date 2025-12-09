using System.Net;
using System.Net.Sockets;
using System.Text;

// CLIENT SOCKET

using Socket client = new(AddressFamily.InterNetwork, 
                          SocketType.Stream, 
                          ProtocolType.Tcp);

try
{
    await client.ConnectAsync(IPAddress.Loopback, 8000);
    Console.WriteLine($"Client connect to server {client.RemoteEndPoint}");

    //byte[] buffer = new byte[1024];
    //int bytesCount = await client.ReceiveAsync(buffer);

    byte[] bufferSize = new byte[4];
    await client.ReceiveAsync( bufferSize );
    int size = BitConverter.ToInt32(bufferSize, 0);

    byte[] buffer = new byte[size];
    int bytesCount = await client.ReceiveAsync( buffer );

    string messageFromServer = Encoding.UTF8.GetString(buffer, 0, bytesCount);
    Console.WriteLine($"Message size: {size}, Message: {messageFromServer}");

    string message = $"[{DateTime.Now.ToLongTimeString()}] client message\nOther info";
    byte[] messageBuffer = Encoding.UTF8.GetBytes(message);

    await client.SendAsync(messageBuffer);
    Console.WriteLine($"Message sended to server {client.RemoteEndPoint}");
}
catch(Exception ex)
{
    Console.WriteLine(ex.Message);
    Console.WriteLine("Not connection");
}