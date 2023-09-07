namespace SimpleDB;

public class CSVDatabase<T> : IDatabaseRepository<T>
{
    public CSVDatabase()
    {
        Console.WriteLine("It has been done");
    }    

    public IEnumerable<T> Read(int? limit= null)
    {
        return null;
    }


    public void Store(T record)
    {
        return;
    }

}

