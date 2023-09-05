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

    }

    private static void cheep(string message)
    {
        using (var writer = new StreamWriter("chirp_cli_db.csv",true))
        {
            var timestamp = DateTime.Now;
            writer.WriteLine($"{Environment.UserName},{timestamp},{message}");
        }

    }
}
