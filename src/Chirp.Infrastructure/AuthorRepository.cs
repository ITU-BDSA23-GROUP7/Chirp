
public class AuthorRepository : IAuthorRepository
{
    private ChirpDBContext context;

    public AuthorRepository(ChirpDBContext context)
    {

    }

    public async Task<AuthorInfo> GetAuthorInfo(string userName)
    {
        var cheeps = context.Cheeps.Where(c => c.Author.Name == userName)
        .select();

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