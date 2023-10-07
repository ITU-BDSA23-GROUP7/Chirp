using System;
using Microsoft.Data.Sqlite;

public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps();
    public List<CheepViewModel> GetCheepsFromAuthor(string author);
}

public class CheepService : ICheepService
{
    private static readonly List<CheepViewModel> _cheeps;



    public CheepService()
    {
        var sqlDBFilePath = "./chirp.db";
        var sqlQuery = @"SELECT * FROM message ORDER by message.pub_date desc";
        using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
        {

            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader);
            }
        }
    }

    public List<CheepViewModel> GetCheeps()
    {
        return _cheeps;
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author)
    {
        // filter by the provided author name
        return _cheeps.Where(x => x.Author == author).ToList();
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }
}
