using SQLitePCL;

namespace Chirp.Infrastructure;

/// <summary>
/// Repository class used to manage author related data.
/// Implements the <c>IAuthorRepository</c> interface.
/// </summary>
public class AuthorRepository : IAuthorRepository
{
    private ChirpDBContext context;

    /// <summary>
    /// Initializes new instance of AuthorRepository with specified ChirpDBContext.
    /// </summary>
    public AuthorRepository(ChirpDBContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Creates new author with specified username.
    /// Checks whether the username already exists, throws an exception if it does.
    /// </summary>
    /// <param name="username"> New author username </param>
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

    /// <summary>
    /// Asynchronously checks whether a specific username already exists in authors.
    /// </summary>
    /// <param name="username">Username to check for existence</param>
    /// <returns>True if username exists, false if it does not</returns>
    public async Task<bool> UsernameExistsAsync(string username)
    {
        var author = await context.Authors.FirstOrDefaultAsync(c => c.Name == username);
        return author != null;
    }

    /// <summary>
    /// Asynchronously retrieves the <c>AuthorDTO</c> for a specific username.
    /// Throws a <c>UserNotFoundException</c> if the username does not exist.
    /// </summary>
    /// <param name="username">Username to retrieve the <c>AuthorDTO</c> for</param>
    /// <returns><c>AuthorDTO</c> for the specified username</returns>
    public async Task<AuthorDTO> GetAuthorDTOByUsername(string username)
    {
        var author = await context.Authors.FirstOrDefaultAsync(c => c.Name == username);

        if (author == null)
        {
            throw new UsernameNotFoundException($"The username {username} does not exist in the database.");
        }

        return author.ToAuthorDTO();
    }

    /// <summary>
    /// Asynchronously retrieves an <c>Author</c> based on the given username.
    /// If a username does not exist a <c>UsernameNotFoundException</c> is thrown.
    /// </summary>
    /// <param name="username">Username to retrieve the <c>Author</c> for.</param>
    /// <returns>Returns the <c>Author</c> object for the given username</returns>
    private async Task<Author> GetAuthorAsync(string username) {
        var author = await context.Authors.FirstOrDefaultAsync(a => a.Name == username)
            ?? throw new UsernameNotFoundException($"The username {username} does not exist in the database.");

        return author;
    }

    /// <summary>
    /// Asynchronously retrieves an <c>Author</c> with a list of authors followed by the specified author.
    /// If the username does not exist a <c>UsernameNotFoundException</c> is thrown.
    /// </summary>
    /// <param name="username">Username to retrieve the <c>Author</c> for</param>
    /// <returns>Returns the <c>Author</c> object with a list authors followed by the author </returns>
    private async Task<Author> GetAuthorWithFollowingAsync(string username) {
        var author = await context.Authors.Include(a => a.Following).FirstOrDefaultAsync(a => a.Name == username)
            ?? throw new UsernameNotFoundException($"The username {username} does not exist in the database.");

        return author;
    }

    /// <summary>
    /// Asynchronously retrieves an <c>Author</c> with a list of the author's followers included.
    /// If the username does not exist a <c>UsernameNotFoundException</c> is thrown.
    /// </summary>
    /// <param name="username">Username to retrieve the <c>Author</c> for</param>
    /// <returns>Returns the <c>Author</c> object with a list of the author's followers</returns>
    private async Task<Author> GetAuthorWithFollowersAsync(string username) {
        var author = await context.Authors.Include(a => a.Followers).FirstOrDefaultAsync(a => a.Name == username)
            ?? throw new UsernameNotFoundException($"The username {username} does not exist in the database.");

        return author;
    }

    /// <summary>
    /// Asynchronously checks if the given author's profile is hidden.
    /// If the username does not exist a <c>UsernameNotFoundException</c> is thrown.
    /// </summary>
    /// <param name="username">Username whose hidden-status needs to be checked </param>
    /// <returns>True if the profile is hidden, false if it is not</returns>
    public async Task<bool> UsernameIsHidden(string username)
    {
        var author = await context.Authors.FirstOrDefaultAsync(c => c.Name == username);

        if (author == null)
        {
            throw new UsernameNotFoundException($"The username {username} does not exist in the database.");
        }

        return author.Hidden;
    }

    /// <summary>
    /// Asynchronously creates a follow-relationship between two authors.
    /// If the follower and the following author are the same a <c>CannotFollowSelfException</c> is thrown.
    /// </summary>
    /// <param name="newFollowerUsername">Username of the new follower</param>
    /// <param name="newFollowingUsername">Username of the author being followed</param>
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

    /// <summary>
    /// Asynchronously ends a follow-relationship between two authors.
    /// If the un-follower and the un-following author are the same a <c>CannotFollowSelfException</c> is thrown.
    /// </summary>
    /// <param name="newUnFollowerUsername">Username of the author unfollowing</param>
    /// <param name="newUnFollowingUsername">Username of the author being unfollowed</param>
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

    /// <summary>
    /// Asynchronously retrieves the usernames followed by a specific author.
    /// </summary>
    /// <param name="username">Username of author whose following list is retrieved</param>
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

    /// <summary>
    /// Asynchronously retrieves the usernames who follows the specific author.
    /// </summary>
    /// <param name="username"> Username of author whose follower-list is retrieved</param>
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

    /// <summary>
    /// Asynchronously retrieves number of cheeps by specific author.
    /// If the username does not exist a <c>UsernameNotFoundException</c> is thrown.
    /// </summary>
    /// <param name="username"> Username of the author whose cheep-count is retrieved</param>
    /// <returns> Returns author's cheep-count</returns>
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

    /// <summary>
    ///  Asynchronously sets the hidden-status of the specific author's profile.
    ///  If the username does not exist a <c>UsernameNotFoundException</c> is thrown.
    /// </summary>
    /// <param name="username">Username of the author whose hidden-status is changed</param>
    /// <param name="hidden">Desired hidden-status for the author's profile.</param>
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

    /// <summary>
    /// Asynchronously retrieves a scoreboard consisting of the top ten authors with the highest cheep streaks.
    /// An author's cheep-streak is reset if the author's newest cheep was not yesterday or today.
    /// </summary>
    /// <returns>Returns a list of <c>AuthorDTO</c>s representing the authors on the scoreboard</returns>
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
