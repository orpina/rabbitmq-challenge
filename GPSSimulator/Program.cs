using System.Text;
using System.Text.Json;

double minimum = 10.000, maximum = 130.999;
int totalRequests = 10;
using HttpClient client = new();
Random random = new();

for (int i = 1; i <= totalRequests; i++)
{
    using StringContent content = new(
        JsonSerializer.Serialize(
            new
            {
                deviceId = "A104-B1500023-0001",
                latitude = random.NextDouble() * (maximum - minimum) + minimum,
                longitude = random.NextDouble() * (maximum - minimum) + minimum
            }),
        Encoding.UTF8,
        "application/json"
        );

    var response = await client.PostAsync("https://localhost:7053/api/Location", content);

    Console.WriteLine($"Sending request: {i} of {totalRequests}");

    Thread.Sleep(5000);
}

Console.ReadLine();
