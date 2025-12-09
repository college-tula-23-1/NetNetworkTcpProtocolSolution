using System.Net;
using System.Net.Sockets;
using System.Text;

// TRANSLATE LISTENER

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

IPAddress address = IPAddress.Any;
int port = 8000;

TcpListener server = new(address, port);

try
{
    server.Start();
    Console.WriteLine("Server starting...");

    while(true)
    {
        using TcpClient client = await server.AcceptTcpClientAsync();
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[256];
        List<byte> response = new List<byte>();
        int byteData = 0;

        while(true)
        {
            while((byteData = stream.ReadByte()) != '\n')
                response.Add((byte)byteData);

            string key = Encoding.UTF8.GetString(response.ToArray());
            if (key == quit) break;

            string request = $"Eng word: {key} => ";
            if (!words.TryGetValue(key, out string? value))
                request += " Not found in dictionary";
            else
                request += $" Rus word: {value}";
            request += "\n";

            Console.WriteLine($"[{DateTime.Now.ToLongTimeString()} {client.Client.RemoteEndPoint}]: {request}");

            await stream.WriteAsync(Encoding.UTF8.GetBytes(request));
            response.Clear();
        }

        
    }
}
catch(Exception ex)
{
    Console.WriteLine(ex.Message);
}
finally
{
    server.Stop();
}
