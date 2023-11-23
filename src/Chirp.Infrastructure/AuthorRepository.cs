
public class AuthorRepository : IAuthorRepository
{
    private ChirpDBContext context;

    public AuthorRepository(ChirpDBContext context)
    {
        this.context = context;
    }

    public async void CreateNewAuthor(int authorId, string name, string email)
    {

        bool usernameExists = await UsernameExists(name);
        if (usernameExists)
        {
            throw new Exception("Username already exists exception");
        }

        context.Authors.Add(new Author { AuthorId = authorId, Name = name, Email = email, Cheeps = new List<Cheep>()});
        context.SaveChanges();
    }

    private async Task<bool> UsernameExists(string username)
    {
        var author = await context.Authors.FirstOrDefaultAsync(c => c.Name == username);
        return author != null;
    }

    public async Task<AuthorDTO> GetAuthorDTOByUsername(string username)
    {
        var author = await context.Authors.FirstOrDefaultAsync(c => c.Name == username);

        if (author == null)
        {
            throw new UsernameNotFoundException($"The username {username} doesn't exist in the database.");
        }

        var authorInfo = new AuthorDTO
        {
            AuthorId = author.AuthorId,
            Name = author.Name,
            Email = author.Email
        };

        return authorInfo;
    }

    public async Task<Author> FindAuthorByDTO(AuthorDTO authorDTO)
    {
        var author = await context.Authors.Include(a => a.Following)
                        .FirstOrDefaultAsync(a => a.AuthorId == authorDTO.AuthorId);
        return author!;
    }


    public async Task FollowAuthor(AuthorDTO authorDTO, AuthorDTO followAuthorDTO)
    {
        var author = await FindAuthorByDTO(authorDTO);
        var followAuthor = await FindAuthorByDTO(followAuthorDTO);

        author.Following.Add(followAuthor);
        followAuthor.Followers.Add(author);
        await context.SaveChangesAsync();
    }

    public async Task UnfollowAuthor(AuthorDTO authorDTO, AuthorDTO unfollowAuthorDTO)
    {
        var author = await FindAuthorByDTO(authorDTO);
        var authorToUnfollow = await FindAuthorByDTO(unfollowAuthorDTO);

        author.Following.Remove(authorToUnfollow);
        authorToUnfollow.Followers.Remove(author);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<string>> GetFollowingUsernames(AuthorDTO authorDTO)
    {
        var author = await FindAuthorByDTO(authorDTO);
        var following = new List<string>();

        foreach (var followingAuthor in author.Following)
        {
            following.Add(followingAuthor.Name);
        }

        return following;
    }
}