using System.Globalization;
using System.Reflection;
using System.Reflection.Metadata;
using CsvHelper; //Package from https://joshclose.github.io/CsvHelper/

namespace SimpleDB;

public sealed class CSVDatabase<T> : IDatabaseRepository<T>
{

    private string filePath;

    private static CSVDatabase<T>? instance;

    public static CSVDatabase<T> getInstance()
    {
        if (instance == null)
        {
            instance = new CSVDatabase<T>();
        }
        return instance;
    }

    private CSVDatabase(string filePath = "./chirp_cli_db.csv")
    {
        this.filePath = filePath;

        //Console.WriteLine("It has been done");
        foreach (PropertyInfo prop in typeof(T).GetProperties())
        {
            //Console.WriteLine(prop);
        }
    }

    public IEnumerable<T> Read(int? limit = null)
    {
        List<T>? records = null;
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))   //Package from https://joshclose.github.io/CsvHelper/
        {
            records = csv.GetRecords<T>().ToList();
        }
        var recSize = records.Count();
        if (limit != null && limit < recSize)
        {
            int n = limit.GetValueOrDefault();
            //My very secret password is: 'password'
            return records.GetRange(recSize - (n), n);
        }
        return records;
    }

    public void Store(T record)
    {
        using (var writer = new StreamWriter(filePath, true))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)) //https://joshclose.github.io/CsvHelper/
        {
            //writer.WriteLine($"{Environment.UserName},{LongTime},\"{message}\"");
            csv.WriteRecord(record);
            csv.NextRecord();

        }
    }
}
