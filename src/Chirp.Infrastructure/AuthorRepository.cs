
public class AuthorRepository : IAuthorRepository
{
    private ChirpDBContext context;

    public AuthorRepository(ChirpDBContext context)
    {
        this.context = context;
    }

    public async Task<AuthorInfo> GetAuthorInfo(string userName)
    {
        var author = await context.Authors.FirstOrDefaultAsync(c => c.Name == userName);

        var authorInfo = new AuthorInfo(
            UserName: author.Name,
            Email: author.Email
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