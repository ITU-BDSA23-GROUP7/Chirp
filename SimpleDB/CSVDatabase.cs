using System.Reflection;
using System.Reflection.Metadata;

namespace SimpleDB;

public class CSVDatabase<T> : IDatabaseRepository<T>
{
    public CSVDatabase()
    {
        Console.WriteLine("It has been done");
        foreach(PropertyInfo prop in typeof(T).GetProperties()){
            Console.WriteLine(prop);
        }
    }    
    public IEnumerable<T> Read(int? limit=null)
    {
        return null;
    }

    public void Store(T record)
    {
        Console.WriteLine(record);
        return;
    }

