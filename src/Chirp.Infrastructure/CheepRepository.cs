namespace Chirp.Infrastructure;
public class CheepRepository : ICheepRepository
{
    private int pageLength = 32;
    private ChirpDBContext context;
    public CheepRepository(ChirpDBContext context)
    {
        this.context = context;
    }

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
            var cheepDTO = new CheepDTO
            {
                Author = new AuthorDTO
                {
                    Name = cheep.Author.Name,
                    Email = cheep.Author.Email
                },
                Message = cheep.Text,
                Timestamp = cheep.TimeStamp.ToString()
            };

            cheepDTOs.Add(cheepDTO);
        }

        return cheepDTOs;
    }

    public async Task<List<CheepDTO>> GetFollowerCheeps(string author, int pageNum = 1)
    {
        int pageIndex = pageNum - 1;

        List<string> following = new List<string>();

        foreach (Author a in context.Authors.Where(a => a.Name == author).First().Following)
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
            var cheepDTO = new CheepDTO
            {
                Author = new AuthorDTO
                {
                    Name = cheep.Author.Name,
                    Email = cheep.Author.Email
                },
                Message = cheep.Text,
                Timestamp = cheep.TimeStamp.ToString()
            };

            cheepDTOs.Add(cheepDTO);
        }

        return cheepDTOs;
    }

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
}
