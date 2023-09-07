using System;
using System.IO;
using System.Text.RegularExpressions;
using SimpleDB;

class Program
{
    public static void Main(string[] args)
    {
       IDatabaseRepository<string> idr = new CSVDatabase<>();

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

    private static void read()
    {
        using (var reader = new StreamReader("chirp_cli_db.csv")){

            // reads next line throws NullReferenceException if it returns 'null'
            string? nextln = reader.ReadLine() ?? throw new NullReferenceException();

            // The following regex splits our CSV elements by ',' but ignores a ',' if it is within a set of quotes
            // The regex is generated using ChatGPT
            Regex reg = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

            string[] arr = reg.Split(nextln);


            if(!(arr.Length == 3 && arr[0] == "Author" && arr[1] == "Timestamp" && arr[2] == "Message"))
            {
                throw new ArgumentException("CSV order does not correspond to the programs expectations. The format should be: 'Author,Timestamp,Message'");
            }

            int lineno = 1;
            while((nextln = reader.ReadLine()) != null)
            {
                lineno++;
                arr = reg.Split(nextln);
                if(arr.Length != 3) throw new ArgumentException($"CSV element does not have the correct length at line {lineno}, every line should contain 3 elements");
                DateTimeOffset utcTime = DateTimeOffset.FromFileTime(long.Parse(arr[1]));
                Console.WriteLine($"{arr[0]} @ {utcTime.ToString("HH:mm:ss dd/MM/yyyy")}: {arr[2].Trim('"')}");
                
            }
        }
    }

    private static void cheep(string message)
    {
        using (var writer = new StreamWriter("chirp_cli_db.csv",true))
        {
            DateTimeOffset utcTime = DateTimeOffset.UtcNow; //https://learn.microsoft.com/en-us/dotnet/api/system.datetimeoffset.utcnow?view=net-7.0
            long LongTime = utcTime.ToFileTime();
            writer.WriteLine($"{Environment.UserName},{LongTime},\"{message}\"");
        }

    }
}
