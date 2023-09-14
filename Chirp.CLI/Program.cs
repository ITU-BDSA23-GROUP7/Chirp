using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using SimpleDB;


class Program
{
    public record Cheep(string Author, string Message, long Timestamp);

    static IDatabaseRepository<Cheep> dbRepository;

    public static void Main(string[] args)
    {
        dbRepository = new CSVDatabase<Cheep>();
       
        if (args.Length == 0)
        {
            UserInterface.noCommand();
            return;
        }

        if (args[0] == "read")
        {
            if(args.Length > 1) read(int.Parse(args[1]));

            else read();
        }
        else if (args[0] == "cheep")
        {
            cheep(args[1]);
        }
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
