namespace Chirp.Infrastructure;
public class CheepRepository : ICheepRepository
{
    private int pageLength = 32;
    private ChirpDBContext context;
    public CheepRepository(ChirpDBContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// gets a list of CheepDTOs based on the given parameters
    /// </summary>
    /// <param name="pageNum">The page number of cheeps to retrieve (default is 1).</param>
    /// <param name="author">The optional author's name to filter cheeps (default is null for all cheeps)
    /// (if author is null, then all cheeps will be returned.).</param>
    /// <returns>
    /// A list of CheepDTOs representing the cheeps from the given author/all authors if author is null, ordered by timestamp,
    /// excluding cheeps from hidden authors, and paginated based on the specified page number.
    /// </returns>
    public async Task<List<CheepDTO>> GetCheeps(int pageNum = 1, string? author = null)
    {
        int pageIndex = pageNum - 1;

        // Checks wether there is an author, and takes cheeps corresponding to an author or all the cheeps if no author has been specified
        List<Cheep> cheeps = await
            (
                author == null ?
                context.Cheeps :
                context.Cheeps.Where(c => c.Author.Name == author)
            )
            .OrderByDescending(c => c.TimeStamp)
            .Where(c => !c.Author.Hidden)
            .Skip(pageIndex * pageLength)
            .Take(pageLength)
            .Include(c => c.Author)
            .ToListAsync();

        var cheepDTOs = new List<CheepDTO>();
        foreach (Cheep cheep in cheeps)
        {
            var cheepDTO = cheep.ToCheepDTO();

            cheepDTOs.Add(cheepDTO);
        }

        return cheepDTOs;
    }

    /// <summary>
    /// gets a list of CheepDTOs from the given authors followers cheeps.
    /// </summary>
    /// <param name="pageNum">The page number of cheeps to retrieve (default is 1).</param>
    /// <param name="author">The author's name to find the followers from.</param>
    /// <returns>
    /// A list of CheepDTOs representing the cheeps from the given authors followers, ordered by timestamp,
    /// excluding cheeps from hidden authors, and paginated based on the specified page number.
    /// </returns>
    public async Task<List<CheepDTO>> GetFollowerCheeps(string author, int pageNum = 1)
    {
        int pageIndex = pageNum - 1;

        List<string> following = new List<string>();

        var followingAuthors = context.Authors.Where(a => a.Name == author).Include(a => a.Following).First().Following;

        foreach (Author a in followingAuthors)
        {
            following.Add(a.Name);
        }

        // Checks wether there is an author, and takes cheeps corresponding to an author or all the cheeps if no author has been specified
        List<Cheep> cheeps = await context.Cheeps
            .Where(c => c.Author.Name == author || following.Contains(c.Author.Name))
            .OrderByDescending(c => c.TimeStamp)
            .Where(c => !c.Author.Hidden)
            .Skip(pageIndex * pageLength)
            .Take(pageLength)
            .Include(c => c.Author)
            .ToListAsync();

        var cheepDTOs = new List<CheepDTO>();
        foreach (Cheep cheep in cheeps)
        {
            var cheepDTO = cheep.ToCheepDTO();

            cheepDTOs.Add(cheepDTO);
        }

        return cheepDTOs;
    }

    /// <summary>
    /// gets an integer that represents the number of pages needed to display all cheeps of a user.
    /// </summary>
    /// <param name="author">The author's name to find the followers from.(default is null)
    /// (if author is null, then the pagecount to display all cheeps is used instead)</param>
    /// <returns>
    /// A list of CheepDTOs representing the cheeps from the given authors followers, ordered by timestamp,
    /// excluding cheeps from hidden authors, and paginated based on the specified page number.
    /// </returns>
    public int GetPageCount(string? author = null)
    {
        // Checks wether there is an author, and takes cheeps corresponding to an author or all the cheeps if no author has been specified
        int cheepCount = (
                author == null ?
                context.Cheeps :
                context.Cheeps.Where(c => c.Author.Name == author)
            )
            .Where(c => !c.Author.Hidden)
            .Count();
        return (int)MathF.Ceiling(1f * cheepCount / pageLength);
    }

    /// <summary>
    /// Adds a new cheep to the database for the specified author.
    /// </summary>
    /// <param name="username">The username of the author adding the cheep.</param>
    /// <param name="message">The text of the cheep being added.</param>
    /// <returns>A task representing the asynchronous operation of adding the cheep.</returns>
    /// <exception cref="UsernameNotFoundException">
    /// Thrown if the specified username does not exist in the database.
    /// </exception>
    /// <exception cref="Exception">
    /// Thrown if the specified username is hidden, and therefore, cannot cheep.
    /// </exception>
    public async Task AddCheepAsync(string username, string message)
    {

        Author? author = await context.Authors.Where(a => a.Name == username).FirstOrDefaultAsync();

        if (author == null)
        {
            throw new UsernameNotFoundException($"The username {username} does not exist in the database");
        }
        if (author.Hidden == true){
            throw new Exception($"The username {username} is hidden and can therefore not cheep!");
        }

        IQueryable<Cheep> Cheeps = context.Cheeps
            .Where(c => c.Author.Name == username)
            .OrderByDescending(c => c.TimeStamp);
        if (Cheeps.Any())
        {
            Cheep lastCheep = Cheeps.First();
            if (lastCheep != null)
            {
                DateTime yesterday = DateTime.Today.AddDays(-1);
                DateTime today = DateTime.Today;
                if (lastCheep.TimeStamp.Date == yesterday)
                {
                    author.CheepStreak++;
                }
                else if (lastCheep.TimeStamp.Date != today)
                {
                    author.CheepStreak = 0;
                }
            }
        }
        

        Cheep input = new Cheep
        {
            CheepId = Guid.NewGuid(),
            Text = message,
            TimeStamp = DateTime.Now,
            Author = author,
        };
        context.Cheeps.Add(input);
        Console.WriteLine("Did we reach this-?");

        await context.SaveChangesAsync();

    }

    /// <summary>
    /// Removes a cheep from the database based on its unique identifier.
    /// </summary>
    /// <param name="cheepId">The unique identifier of the cheep to be removed.</param>
    /// <returns>A task representing the asynchronous operation of removing the cheep.</returns>
    /// <exception cref="Exception">
    /// Thrown if the cheep with the specified unique identifier is not found.
    /// </exception>
    public async Task RemoveCheep(Guid cheepId)
    {
        var cheepToRemove = await context.Cheeps.Where(c => c.CheepId == cheepId).FirstOrDefaultAsync();

        if (cheepToRemove == null) {
            throw new Exception($"Cheep not found. GUID: {cheepId}");
        }

        context.Cheeps.Remove(cheepToRemove);
    
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// gets an integer that represents the number of pages needed to display all cheeps from all followers.
    /// </summary>
    /// <param name="author">The author's name to find the followers from.</param>
    /// <returns>
    /// An int representing the amount of pages needed to display the cheeps of all followers,
    /// excluding cheeps from hidden authors.
    /// </returns>
    public int GetFollowersPageCount(string author)
    {
        List<string> following = new List<string>();

        var followingAuthors = context.Authors.Where(a => a.Name == author).Include(a => a.Following).First().Following;

        foreach (Author a in followingAuthors)
        {
            following.Add(a.Name);
        }

        var cheepCount = context.Cheeps
            .Where(c => c.Author.Name == author || following.Contains(c.Author.Name))
            .Where(c => !c.Author.Hidden)
            .Count();

        return (int)MathF.Ceiling(1f * cheepCount / pageLength);
    }
}
