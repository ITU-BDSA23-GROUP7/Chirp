using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

class UserInterface 
{
    public static void read(Program.Cheep record) 
    {
        var Author = record.Author;
        DateTimeOffset UTCTimestamp = DateTimeOffset.FromFileTime(record.Timestamp);
        var Message = record.Message;
        Console.WriteLine($"{Author} @ {UTCTimestamp.ToString("HH:mm:ss dd/MM/yyyy")}: {Message.Trim('"')}");
    }

    public static void noCommand()
    {
        Console.WriteLine("Please choose 'read' or 'cheep'");
    }
    
}