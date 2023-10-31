
public class AuthorRepository : IAuthorRepository
{
    //private ChirpDBContext context;

    public AuthorRepository()
    {

    }

    public async Task<AuthorInfo> GetAuthorInfo(string userName)
    {
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