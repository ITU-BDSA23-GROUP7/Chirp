using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;
using SimpleDB;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Collections.Generic;
using System.Net.Http.Json;


class Program
{
    public record Cheep(string Author, string Message, long Timestamp);

    private static HttpClient client;

    public static void Main(string[] args)
    {
        //The next code is inspired by https://learn.microsoft.com/en-us/dotnet/standard/commandline/define-commands#define-a-root-command
        var rootCommand = new RootCommand();
        rootCommand.SetHandler(() =>
        {
            Console.WriteLine("Default");
        });

        string url = "http://localhost:5027";
        client = new HttpClient();
        client.BaseAddress = new Uri(url);

        var readLinesArgument = new Argument<int>(
            name: "cheeps",
            description: "The ammount of cheeps printed.",
            getDefaultValue: () => 10
        );
        var readCommand = new Command("read", "Prints the newest cheeps.")
        {
            readLinesArgument
        };
        readCommand.SetHandler((cheeps) =>
        {
            read(cheeps);
        }, readLinesArgument);
        rootCommand.Add(readCommand);

        var cheepMessageArgument = new Argument<string>(
            name: "message",
            description: "The message that will be added."
        );
        var cheepCommand = new Command("cheep", "Makes a new cheep with the current logged in user and the current time.")
        {
            cheepMessageArgument
        };
        cheepCommand.SetHandler((message) =>
        {
            cheep(message);
        }, cheepMessageArgument);
        rootCommand.Add(cheepCommand);

        rootCommand.Invoke(args);
        while (true) ;
    }

    //This was partly inspired by https://joshclose.github.io/CsvHelper/getting-started/
    private static async void read(int? limit = null)
    {

        var response = await client.GetAsync($"/cheeps?cap={limit}");
        var responseContent = await response.Content.ReadAsStringAsync();
        var records = JsonSerializer.Deserialize<IEnumerable<Cheep>>(responseContent);

        foreach (var record in records)
        {
            UserInterface.read(record);
        }
    }

    //This was partly inspired by https://joshclose.github.io/CsvHelper/getting-started/
    private static void cheep(string message)
    {
        DateTimeOffset utcTime = DateTimeOffset.UtcNow; //https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset.utcnow?view=net-7.0
        long LongTime = utcTime.ToFileTime();

        Cheep newCheep = new Cheep(Environment.UserName, message, LongTime);
        StringContent jsonObject = new(JsonSerializer.Serialize(newCheep));
        client.PostAsync("/cheep", jsonObject);
    }
}
