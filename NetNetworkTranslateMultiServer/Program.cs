using System.Net;
using System.Net.Sockets;
using System.Text;

// MULTI CLIENT TRANSLATER - LISTENER

/////////////////////////////

Dictionary<string, string> words = new Dictionary<string, string>()
{
    { "table", "стол" },
    { "apple", "яблоко" },
    { "red", "красный" },
    { "sun", "солнце" },
    { "thread", "нить" },
    { "city", "город" },
    { "language", "язык" },
};

string quit = "quit";

/////////////////////////////

int port = 8000;
var addresses = Dns.GetHostAddresses(Dns.GetHostName(), AddressFamily.InterNetwork);
var address = addresses[0];

Console.WriteLine($"Server address: {address}");

TcpListener server = new(IPAddress.Any, port);

// Socket Listener 
//using Socket server = new(AddressFamily.InterNetwork, 
//                          SocketType.Stream, 
//                          ProtocolType.Tcp);

//IPEndPoint endPoint = new(IPAddress.Any, port);


//////////////////////////////

try
{
    // Socket Listener 
    //server.Bind(endPoint);
    //server.Listen();

    server.Start(); // for TcpListener

    Console.WriteLine("Server starting...");

    while(true)
    {
        // for TcpListener
        var client = await server.AcceptTcpClientAsync();
        Console.WriteLine($"Accept client: {client.Client.RemoteEndPoint}");
        Task.Run(async () => await ClientProcessAsync(client));

        // Socket Listener 
        //using Socket client = await server.AcceptAsync();
        //Console.WriteLine($"Accept client: {client.RemoteEndPoint}");

        //Task.Run(async () => await ClientSocketProcessAsync(client));
    }
}
catch(Exception ex)
{
    Console.WriteLine(ex.Message);
}
finally
{
    server.Stop(); // for TcpListener
}



async Task ClientSocketProcessAsync(Socket client)
{
    NetworkStream stream = new NetworkStream(client);

    List<byte> response = new();
    byte[] readedByte = new byte[1];

    while (true)
    {
        while (true)
        {
            int count = await client.ReceiveAsync(readedByte);

            if (count == 0 || readedByte[0] == '\n')
                break;

            response.Add(readedByte[0]);
        }

        string key = Encoding.UTF8.GetString(response.ToArray());
        if (key == quit) break;

        if (!words.TryGetValue(key, out string? translate))
            translate = $"word {key} not found";
        translate += '\n';

        await client.SendAsync(Encoding.UTF8.GetBytes(translate));
        Console.WriteLine($"Send to client: {client.RemoteEndPoint} key: {key} and translate: {translate}");

        response.Clear();
    }

    client.Close();
}
async Task ClientProcessAsync(TcpClient client)
{
    NetworkStream stream = client.GetStream();
    using StreamReader reader = new(stream);
    using StreamWriter writer = new(stream);

    //List<byte> buffer = new();
    //int readedByte = 0;

    while (true)
    {
        //while ((readedByte = stream.ReadByte()) != '\n')
        //{
        //    buffer.Add((byte)readedByte);
        //}

        //string key = Encoding.UTF8.GetString(buffer.ToArray());

        string? key = await reader.ReadLineAsync();

        if (key == quit) break;

        if (!words.TryGetValue(key, out string? translate))
            translate = $"word {key} not found";
        //translate += '\n';

        //await stream.WriteAsync(Encoding.UTF8.GetBytes(translate));
        await writer.WriteLineAsync(translate);
        await writer.FlushAsync();

        Console.WriteLine($"Send to client: {client.Client.RemoteEndPoint} key: {key} and translate: {translate}");

        //buffer.Clear();
    }

    client.Close();
}

