using System.Net;
using System.Net.Sockets;

// BINARY LESTENER

TcpListener server = new(IPAddress.Any, 9000);
List<Employee> employees = new List<Employee>();

try
{
    server.Start();
    Console.WriteLine("Server starting...");

    while(true)
    {
        using var client = await server.AcceptTcpClientAsync();
        var stream = client.GetStream();
        BinaryReader reader = new BinaryReader(stream);
        BinaryWriter writer = new BinaryWriter(stream);

        string name = reader.ReadString();
        int age = reader.ReadInt32();
        decimal salary = reader.ReadDecimal();
        bool married = reader.ReadBoolean();

        Guid id = Guid.NewGuid();

        Employee employee = new Employee()
        {
            Id = id,
            Name = name,
            Age = age,
            Salary = salary,
            Married = married,
        };
        employees.Add(employee);
        Console.WriteLine("Employee created and addin to list");

        writer.Write(employee.Id.ToString());
        writer.Flush();

        foreach (var e in employees)
            Console.WriteLine(e);
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

class Employee
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public int Age { get; set; }
    public decimal Salary { get; set; }
    public bool Married { get; set; }

    public override string ToString()
    {
        return $"Name: {Name}, Age: {Age}, Salary: {Salary}, Married: {Married}";
    }

}