using SQLitePCL;

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
            Name = author.Name,
            Email = author.Email
        };

        return authorInfo;
    }

    public async Task<Author> FindAuthorByDTO(AuthorDTO authorDTO)
    {
        var author = await context.Authors.Include(a => a.Following)
                        .FirstOrDefaultAsync(a => a.Name == authorDTO.Name);
        return author!;
    }
    public async Task<bool> UsernameIsHidden(string username)
    {
        var author = await context.Authors.FirstOrDefaultAsync(c => c.Name == username);

        if (author == null)
        {
            throw new UsernameNotFoundException($"The username {username} does not exist in the database.");
        }

        return author.Hidden;
    }


    public async Task FollowAuthor(AuthorDTO currentAuthorDTO, AuthorDTO authorToFollowDTO)
    {
        var findCurrentAuthorTask = FindAuthorByDTO(currentAuthorDTO);
        var findAuthorToFollowTask = FindAuthorByDTO(authorToFollowDTO);

        var currentAuthor = await findCurrentAuthorTask;
        var authorToFollow = await findAuthorToFollowTask;

        if (currentAuthor != authorToFollow)
        {
            currentAuthor.Following.Add(authorToFollow);
            authorToFollow.Followers.Add(currentAuthor);
            await context.SaveChangesAsync();
        }
        else
        {
            throw new CannotFollowSelfException("You cannot follow yourself.");
        }
    }

    public async Task UnfollowAuthor(AuthorDTO currentAuthorDTO, AuthorDTO authorToUnfollowDTO)
    {
        var findCurrentAuthorTask = FindAuthorByDTO(currentAuthorDTO);
        var findAuthorToUnfollowTask = FindAuthorByDTO(authorToUnfollowDTO);

        var currentAuthor = await findCurrentAuthorTask;
        var authorToUnfollow = await findAuthorToUnfollowTask;

        if (currentAuthor != authorToUnfollow)
        {
            currentAuthor.Following.Remove(authorToUnfollow);
            authorToUnfollow.Followers.Remove(currentAuthor);
            await context.SaveChangesAsync();
        }
        else
        {
            throw new CannotFollowSelfException("You cannot unfollow yourself.");
        }
    }

    public async Task<IEnumerable<string>> GetFollowingUsernames(AuthorDTO authorDTO)
    {
        var author = await FindAuthorByDTO(authorDTO);
        var followingUsernames = new List<string>();
        var following = author.Following.Where(c => !c.Hidden);

        foreach (var followingAuthor in following)
        {
            followingUsernames.Add(followingAuthor.Name);
        }

        return followingUsernames;
    }

    public async Task<IEnumerable<string>> GetFollowersUsernames(AuthorDTO authorDTO)
    {
        var author = await FindAuthorByDTO(authorDTO);
        var followerUsernames = new List<string>();
        var followers = author.Followers.Where(c => !c.Hidden);

        foreach (var followersAuthor in followers)
        {
            followerUsernames.Add(followersAuthor.Name);
        }

        return followerUsernames;
    }

    public async Task<int> GetAmmountOfCheeps(string username)
    {
        var author = await context.Authors.Include(a => a.Cheeps).FirstOrDefaultAsync(c => c.Name == username);

        if (author == null)
        {
            throw new UsernameNotFoundException($"The username {username} does not exist in the database.");
        }

        if (author.Hidden)
        {
            return 0;
        }

        return author.Cheeps.Count;
    }

    public async Task SetHidden(string username, bool hidden)
    {
        var author = await context.Authors.Include(a => a.Cheeps).FirstOrDefaultAsync(c => c.Name == username);

        if (author == null)
        {
            throw new UsernameNotFoundException($"The username {username} does not exist in the database.");
        }

        author.Hidden = hidden;

        await context.SaveChangesAsync();
    }
}
