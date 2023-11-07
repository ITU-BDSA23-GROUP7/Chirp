
public class AuthorRepository : IAuthorRepository
{
    private ChirpDBContext context;

    public AuthorRepository(ChirpDBContext context)
    {
        this.context = context;
    }

    public async void CreateNewAuthor(int authorId, string name, string email){
        
        bool usernameExists = await UsernameExists(name);
        if (usernameExists) {
            throw new Exception("Username already exists exception");
        }

        context.Authors.Add(new Author{AuthorId = authorId, Name=name, Email=email, Cheeps = new List<Cheep>()});
        context.SaveChanges();
    }

    private async Task<bool> UsernameExists(string username) {
        var author = await context.Authors.FirstOrDefaultAsync(c => c.Name == username);
        return author != null;
    }

    public async Task<AuthorInfo> GetAuthorInfo(string username)
    {
        var author = await context.Authors.FirstOrDefaultAsync(c => c.Name == username);

        if (author == null) {
            throw new UsernameNotFoundException($"The username {username} doesn't exist in the database.");
        }

        var authorInfo = new AuthorInfo(
            Username: author.Name,
            Email: author.Email
        );

        return authorInfo;
    }

    protected virtual void Dispose(bool disposing)
    {
        throw new NotImplementedException("The dispose method is not implemented for AuthorRepository");
    }
    public void Dispose()
    {
        Dispose(true);
    }
}