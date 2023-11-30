namespace Chirp.Infrastructure;
public class AuthorRepository : IAuthorRepository
{
    private ChirpDBContext context;

    public AuthorRepository(ChirpDBContext context)
    {
        this.context = context;
    }

    public async Task CreateNewAuthor(string name)
    {

        bool usernameExists = await UsernameExistsAsync(name);
        if (usernameExists)
        {
            throw new Exception("Username already exists exception");
        }

        context.Authors.Add(new Author { AuthorId = Guid.NewGuid(), Name = name, Cheeps = new List<Cheep>() });
        context.SaveChanges();
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        var author = await context.Authors.FirstOrDefaultAsync(c => c.Name == username);
        return author != null;
    }

    public async Task<AuthorDTO> GetAuthorDTOByUsername(string username)
    {
        var author = await context.Authors.FirstOrDefaultAsync(c => c.Name == username);

        if (author == null)
        {
            throw new UsernameNotFoundException($"The username {username} does not exist in the database.");
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


    public async Task FollowAuthor(AuthorDTO currentAuthorDTO, AuthorDTO authorToFollowDTO)
    {
        var findCurrentAuthorTask = FindAuthorByDTO(currentAuthorDTO);
        var findAuthorToFollowTask = FindAuthorByDTO(authorToFollowDTO);

        var currentAuthor = await findCurrentAuthorTask;
        var authorToFollow = await findAuthorToFollowTask;

        currentAuthor.Following.Add(authorToFollow);
        authorToFollow.Followers.Add(currentAuthor);
        await context.SaveChangesAsync();
    }

    public async Task UnfollowAuthor(AuthorDTO currentAuthorDTO, AuthorDTO authorToUnfollowDTO)
    {
        var findCurrentAuthorTask = FindAuthorByDTO(currentAuthorDTO);
        var findAuthorToUnfollowTask = FindAuthorByDTO(authorToUnfollowDTO);

        var currentAuthor = await findCurrentAuthorTask;
        var authorToUnfollow = await findAuthorToUnfollowTask;

        currentAuthor.Following.Remove(authorToUnfollow);
        authorToUnfollow.Followers.Remove(currentAuthor);
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
