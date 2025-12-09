using System.Net.Sockets;

string url = "yandex.ru";
int port = 80;

using Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

try
{
    await socket.ConnectAsync(url, port);
    Console.WriteLine($"Conntection to {url}");

    Console.WriteLine($"Remote address: {socket.RemoteEndPoint}");
    Console.WriteLine($"Local address: {socket.LocalEndPoint}");
}
catch(Exception ex)
{
    Console.WriteLine(ex.Message);
    Console.WriteLine($"Don't conntection to {url}");
}
finally
{
    await socket.DisconnectAsync(true);
}