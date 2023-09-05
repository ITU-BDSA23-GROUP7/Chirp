using System;
using System.IO;

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

    private static void read()
    {
        using (var reader = new StreamReader("chirp_cli_db.csv")){

            // reads next line throws NullReferenceException if it returns 'null'
            string? nextln = reader.ReadLine() ?? throw new NullReferenceException();
            string[] arr = nextln.Split(",");

            if(!(arr.Length == 3 && arr[0] == "Author" && arr[1] == "Timestamp" && arr[2] == "Message"))
            {
                throw new ArgumentException("CSV order does not correspond to the programs expectations. The format should be: 'Author,Timestamp,Message'");
            }

            int lineno = 1;
            while((nextln = reader.ReadLine()) != null)
            {
                lineno++;
                arr = nextln.Split(",");
                if(arr.Length != 3) throw new ArgumentException($"CSV element does not have the correct length at line {lineno}, every line should contain 3 elements");
                Console.WriteLine($"{arr[1]} - {arr[0]}: {arr[2]}");
                
            }
        }
    }

    private static void cheep(string message)
    {
        using (var writer = new StreamWriter("chirp_cli_db.csv",true))
        {
            string author = "anon";
            var timestamp = DateTime.Now;
            writer.WriteLine($"{author},{timestamp},{message}");
        }

    }
}
