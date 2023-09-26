using System.Text.Json;
using SimpleDB;

class Program
{
    public record Cheep(string Author, string Message, long Timestamp);

    static IDatabaseRepository<Cheep> dbRepository;
    public static void Main(string[] args)
    {
        dbRepository = CSVDatabase<Cheep>.getInstance();

        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.MapPost("/cheep", (HttpRequest request) => StoreCheep(request));

        app.MapGet("/cheeps", (int cap) => GetCheeps(cap));

        app.Run();
    }

    private static string GetCheeps(int cap = 10)
    {
        IEnumerable<Cheep> records = dbRepository.Read(cap);
        return JsonSerializer.Serialize(records);
    }

    private static async void StoreCheep(HttpRequest request)
    {
        var body = new StreamReader(request.Body);
        string jsonObject = await body.ReadToEndAsync();
        Console.WriteLine(jsonObject);
        Cheep newCheep = JsonSerializer.Deserialize<Cheep>(jsonObject);
        Console.WriteLine(newCheep.Author);
        Console.WriteLine(newCheep.Timestamp);
        Console.WriteLine(newCheep.Message);
        dbRepository.Store(newCheep);
    }

}




