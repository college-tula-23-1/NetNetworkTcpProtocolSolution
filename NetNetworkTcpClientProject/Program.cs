using System.Net;
using System.Net.Sockets;
using System.Text;

// TCP CLIENT

using TcpClient client = new TcpClient();
IPAddress serverAddress = IPAddress.Loopback;
int port = 8000;

await client.ConnectAsync(serverAddress, port);

if (client.Connected)
    Console.WriteLine($"Client connected to server {client.Client.RemoteEndPoint}");
else
{
    Console.WriteLine("Not connection");
    return;
}

NetworkStream stream = client.GetStream();
byte[] messageBuffer = new byte[1024];

int bytesCount = await stream.ReadAsync(messageBuffer);
string messageFromServer = Encoding.UTF8.GetString(messageBuffer);

Console.WriteLine(messageFromServer);

string message = $"[{DateTime.Now.ToLongTimeString()}] Client message";
messageBuffer = Encoding.UTF8.GetBytes(message);

await stream.WriteAsync(messageBuffer);
Console.WriteLine($"Message send to server {client.Client.RemoteEndPoint}");
    