
public class AuthorRepository : IAuthorRepository
{
    private ChirpDBContext context;

    public AuthorRepository(ChirpDBContext context)
    {
        this.context = context;
    }

    public async void CreateNewAuthor(string name, string email)
    {

        bool usernameExists = await UsernameExists(name);
        if (usernameExists)
        {
            throw new Exception("Username already exists exception");
        }

        context.Authors.Add(new Author { AuthorId = Guid.NewGuid(), Name = name, Email = email, Cheeps = new List<Cheep>() });
        context.SaveChanges();
    }

    private async Task<bool> UsernameExists(string username)
    {
        var author = await context.Authors.FirstOrDefaultAsync(c => c.Name == username);
        return author != null;
    }

    public async Task<AuthorInfo> GetAuthorInfo(string username)
    {
        var author = await context.Authors.FirstOrDefaultAsync(c => c.Name == username);

        if (author == null)
        {
            throw new UsernameNotFoundException($"The username {username} does not exist in the database.");
        }

        var authorInfo = new AuthorInfo(
            Username: author.Name,
            Email: author.Email
        );

        return authorInfo;
    }
}
