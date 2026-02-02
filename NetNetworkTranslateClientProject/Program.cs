using System.Net;
using System.Net.Sockets;
using System.Text;

// TRANSLATE CLIENT

Console.Write("Input server ip: ");
string? serverIpString = Console.ReadLine();
IPAddress serverIp = IPAddress.Parse(serverIpString);

using TcpClient client = new TcpClient();
//await client.ConnectAsync(IPAddress.Loopback, 8000);
await client.ConnectAsync(serverIp, 8000);
var stream = client.GetStream();
var reader = new StreamReader(stream);
var writer = new StreamWriter(stream);

//List<byte> response = new List<byte>();
//int byteData = 0;

while (true)
{
    string? word = GetWord();
    //byte[] buffer = Encoding.UTF8.GetBytes(word + "\n");
    //await stream.WriteAsync(buffer);
    await writer.WriteLineAsync(word);
    await writer.FlushAsync();

    //while ((byteData = stream.ReadByte()) != '\n')
    //    response.Add((byte)byteData);

    //string answer = Encoding.UTF8.GetString(response.ToArray());
    string? answer = await reader.ReadLineAsync();

    Console.WriteLine(answer);
    //response.Clear();

    if (word == "quit") break;
}


string? GetWord()
{
    Console.Write("Input endlish word: ");
    string? word = Console.ReadLine();
    return word;
}

