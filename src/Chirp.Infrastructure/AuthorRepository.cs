
public class AuthorRepository : IAuthorRepository
{
    private ChirpDBContext context;

    public AuthorRepository(ChirpDBContext context)
    {
        this.context = context;
        context.Database.Migrate();
        // Adds example data to the database if nothing has been added yet
        DbInitializer.SeedDatabase(context);
    }

    public async Task<AuthorInfo> GetAuthorInfo(string userName)
    {
        IQueryable<Cheep> cheeps = context.Cheeps.Where(c => c.Author.Name == userName);
        //var authors = context.Cheeps.Where(c => c.Author.Name == author);
        var authorInfo = new AuthorInfo(
            UserName: "Peter File",
            Email: "ben@dover.cum"
        );
        return authorInfo;
    }

    protected virtual void Dispose(bool disposing)
    {
        throw new NotImplementedException("The dispose method is not implemented for ChirpRepository");
    }
    public void Dispose()
    {
        Dispose(true);
    }
}