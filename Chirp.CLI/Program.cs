using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using CsvHelper; //Package from https://joshclose.github.io/CsvHelper/

class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Please choose 'read' or 'cheep'");
            return;
        }

        if (args[0] == "read")
        {
            read();
        }
        else if (args[0] == "cheep")
        {
            cheep(args[1]);
        }
    }

    //This was partly inspired by https://joshclose.github.io/CsvHelper/getting-started/
    private static void read()
    {
        using (var reader = new StreamReader("chirp_cli_db.csv"))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))   //Package from https://joshclose.github.io/CsvHelper/
        {
            var records = csv.GetRecords<Cheep>();

            foreach (var record in records)
            {
                var Author = record.Author;
                DateTimeOffset UTCTimestamp = DateTimeOffset.FromFileTime(record.Timestamp);
                var Message = record.Message;
                Console.WriteLine($"{Author} @ {UTCTimestamp.ToString("HH:mm:ss dd/MM/yyyy")}: {Message.Trim('"')}");
            }
        }
    }

    //This was partly inspired by https://joshclose.github.io/CsvHelper/getting-started/
    private static void cheep(string message)
    {
        using (var writer = new StreamWriter("chirp_cli_db.csv", true))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) //https://joshclose.github.io/CsvHelper/
        {
            DateTimeOffset utcTime = DateTimeOffset.UtcNow; //https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset.utcnow?view=net-7.0
            Cheep record = new Cheep
            {
                Author = Environment.UserName,
                Timestamp = utcTime.ToFileTime(),
                Message = $"\"{message}\""
            };
            csv.WriteRecord(record);
            csv.NextRecord();
        }

    }
}
