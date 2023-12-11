using SQLitePCL;

namespace Chirp.Infrastructure;
public class AuthorRepository : IAuthorRepository
{
    private ChirpDBContext context;

    public AuthorRepository(ChirpDBContext context)
    {
        this.context = context;
    }

    public async Task CreateNewAuthor(string username)
    {
        bool usernameExists = await UsernameExistsAsync(username);
        if (usernameExists)
        {
            throw new Exception("Username already exists exception");
        }

        context.Authors.Add(new Author { AuthorId = Guid.NewGuid(), Name = username, Cheeps = new List<Cheep>() });
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

        return author.ToAuthorDTO();
    }

    private async Task<Author> GetAuthorAsync(string username) {
        var author = await context.Authors.FirstOrDefaultAsync(a => a.Name == username)
            ?? throw new UsernameNotFoundException($"The username {username} does not exist in the database.");
        
        return author;
    }
    private async Task<Author> GetAuthorWithFollowingAsync(string username) {
        var author = await context.Authors.Include(a => a.Following).FirstOrDefaultAsync(a => a.Name == username)
            ?? throw new UsernameNotFoundException($"The username {username} does not exist in the database.");
        
        return author;
    }
    private async Task<Author> GetAuthorWithFollowersAsync(string username) {
        var author = await context.Authors.Include(a => a.Followers).FirstOrDefaultAsync(a => a.Name == username)
            ?? throw new UsernameNotFoundException($"The username {username} does not exist in the database.");
        
        return author;
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


    public async Task FollowAuthor(string newFollowerUsername, string newFollowingUsername)
    {
        var newFollowerAuthor = await GetAuthorAsync(newFollowerUsername);
        var newFollowingAuthor = await GetAuthorAsync(newFollowingUsername);

        if (newFollowerAuthor == newFollowingAuthor)
        {
            throw new CannotFollowSelfException("You cannot follow yourself.");
        }

        newFollowerAuthor.Following.Add(newFollowingAuthor);
        newFollowingAuthor.Followers.Add(newFollowerAuthor);
        await context.SaveChangesAsync();
    }

    public async Task UnfollowAuthor(string newUnFollowerUsername, string newUnFollowingUsername)
    {
        var newUnFollowerAuthor = await GetAuthorWithFollowingAsync(newUnFollowerUsername);;
        var newUnFollowingAuthor = await GetAuthorWithFollowersAsync(newUnFollowingUsername);

        if (newUnFollowerAuthor == newUnFollowingAuthor)
        {
            throw new CannotFollowSelfException("You cannot unfollow yourself.");
        }

        // Check if newUnFollowerAuthor follows newUnFollowingAuthor

        newUnFollowerAuthor.Following.Remove(newUnFollowingAuthor);
        newUnFollowingAuthor.Followers.Remove(newUnFollowerAuthor);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<string>> GetFollowingUsernames(string username)
    {
        var author = await GetAuthorWithFollowingAsync(username);
        var followingUsernames = new List<string>();
        var following = author.Following.Where(c => !c.Hidden);

        foreach (var followingAuthor in following)
        {
            followingUsernames.Add(followingAuthor.Name);
        }

        return followingUsernames;
    }

    public async Task<IEnumerable<string>> GetFollowersUsernames(string username)
    {
        var author = await GetAuthorWithFollowersAsync(username);
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

    public async Task<List<AuthorDTO>> GetScoreboardAsync()
    {
        List<Author> streaks = context.Authors.Where(a => a.CheepStreak > 0).ToList();
        DateTime yesterday = DateTime.Today.AddDays(-1);
        DateTime today = DateTime.Today;
        foreach (Author author in streaks)
        {
            IQueryable<Cheep> Cheeps = context.Cheeps
                .Where(c => c.Author.Name == author.Name)
                .OrderByDescending(c => c.TimeStamp); ;
            if (Cheeps.Any())
            {
                Cheep lastCheep = Cheeps.First();
                if (lastCheep != null)
                {
                    if (!(lastCheep.TimeStamp.Date == yesterday || lastCheep.TimeStamp.Date == today))
                    {
                        author.CheepStreak = 0;
                    }
                }
            }
        }
        await context.SaveChangesAsync();

        streaks = await context.Authors.Where(a => a.CheepStreak > 0)
            .OrderByDescending(a => a.CheepStreak)
            .Where(a => !a.Hidden)
            .Take(10)
            .ToListAsync();

        List<AuthorDTO> compressedStreaks = new List<AuthorDTO> ();

        foreach (Author author in streaks)
        {
            compressedStreaks.Add(author.ToAuthorDTO());
        }
        return compressedStreaks;
    }
}
