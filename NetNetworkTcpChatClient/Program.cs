using System.Net.Sockets;


string serverHost;
int port = 8000;

Console.Write("Input server host: ");
serverHost = Console.ReadLine()!;

using TcpClient client = new TcpClient();
Console.Write("Input name: ");
string? name = Console.ReadLine();

StreamReader? Reader = null;
StreamWriter? Writer = null;

try
{
    client.Connect(serverHost, port);
    Reader = new StreamReader(client.GetStream());
    Writer = new StreamWriter(client.GetStream());

    if (Reader is null || Writer is null) return;

    Task.Run(() => ReceiveMessageAsync(Reader));

    await SendMessageAsync(Writer);
}
catch(Exception ex)
{
    Console.WriteLine(ex.Message);
}

Writer?.Close();
Reader?.Close();


async Task SendMessageAsync(StreamWriter writer)
{
    await writer.WriteLineAsync(name);
    await writer.FlushAsync();


    while(true)
    {
        Console.Write("Message: ");
        string? message = Console.ReadLine();
        await writer.WriteLineAsync(message);
        await writer.FlushAsync();
    }
}


async Task ReceiveMessageAsync(StreamReader reader)
{
    while(true)
    {
        try
        {
            string? message = await reader.ReadLineAsync();
            if (string.IsNullOrEmpty(message))
                continue;
            ConsoleMessagePrint(message);
        }
        catch
        {
            break;
        }
    }
}

void ConsoleMessagePrint(string message)
{
    var position = Console.GetCursorPosition();
    int left = position.Left;
    int top = position.Top;

    Console.MoveBufferArea(0, top, left, 1, 0, top + 1);
    Console.SetCursorPosition(0, top);
    Console.WriteLine(message);
    Console.SetCursorPosition(left, top + 1);

}