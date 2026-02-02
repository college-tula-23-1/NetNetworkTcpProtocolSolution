using System.Net;
using System.Net.Sockets;

// BINARY CLIENT

using TcpClient client = new();
await client.ConnectAsync(IPAddress.Loopback, 9000);

var stream = client.GetStream();
BinaryReader reader = new BinaryReader(stream);
BinaryWriter writer = new BinaryWriter(stream);

Console.Write("Input name: ");
string name = Console.ReadLine()!;

Console.Write("Input age: ");
int age = Int32.Parse(Console.ReadLine()!);

Console.Write("Input salary: ");
decimal salary = Decimal.Parse(Console.ReadLine());

Console.Write("Marryed?: <y/n>");
bool married = (Console.ReadLine() == "y");

writer.Write(name);
writer.Write(age);
writer.Write(salary);
writer.Write(married);
writer.Flush();

Guid id = new Guid(reader.ReadString());
Console.WriteLine($"Our employee init id: {id}");
