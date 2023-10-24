
public class ChirpDBContext : DbContext
{

    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }
    public string DbPath { get; }

    public ChirpDBContext()
    {
        var path = Path.GetTempPath();
        DbPath = System.IO.Path.Join(path, "chirp.db");
        Console.WriteLine(DbPath);
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}
