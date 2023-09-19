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


class Program
{
    public record Cheep(string Author, string Message, long Timestamp);

    static IDatabaseRepository<Cheep> dbRepository;

    public static void Main(string[] args)
    {
        dbRepository = new CSVDatabase<Cheep>();
       
       var rootCommand = new RootCommand();
        rootCommand.SetHandler(() => {
            Console.WriteLine("Default");
        });

        var readLinesArgument = new Argument<int>(
            name: "cheeps",
            description: "The ammount of cheeps printed.",
            getDefaultValue: () => 10
        );
        var readCommand = new Command("read", "Prints the newest cheeps.")
        {
            readLinesArgument
        };
        readCommand.SetHandler((cheeps) => {
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
        cheepCommand.SetHandler((message) => {
            cheep(message);
        }, cheepMessageArgument);
        rootCommand.Add(cheepCommand);

        rootCommand.Invoke(args);        
    }

    //This was partly inspired by https://joshclose.github.io/CsvHelper/getting-started/
    private static void read(int? limit = null) 
    {
        
        var records = dbRepository.Read(limit);
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

        dbRepository.Store(new Cheep(Environment.UserName,message,LongTime));
    }
}
