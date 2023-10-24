
public class ChirpRepository : IChirpRepository
{
    private int pageLength = 32;
    private ChirpDBContext context;
    public ChirpRepository(ChirpDBContext context)
    {
        this.context = context;
        context.Database.Migrate();
        // Adds example data to the database if nothing has been added yet
        DbInitializer.SeedDatabase(context);
    }

    public async Task<IEnumerable<CheepDTO>> GetCheeps(int pageNum = 1, string? author = null)
    {
        int pageIndex = pageNum - 1;

        // Checks wether there is an author, and takes cheeps corresponding to an author or all the cheeps if no author has been specified
        List<Cheep> cheeps = await
            (
                author == null ?
                context.Cheeps :
                context.Cheeps.Where(c => c.Author.Name == author)
            )
            .OrderBy(c => c.TimeStamp)
            .Skip(pageIndex * pageLength)
            .Take(pageLength)
            .Include(c => c.Author)
            .ToListAsync();

        var cheepDTOs = new List<CheepDTO>();
        foreach (Cheep cheep in cheeps)
        {
            var cheepDTO = new CheepDTO(
                Author: cheep.Author.Name,
                Message: cheep.Text,
                Timestamp: cheep.TimeStamp.ToString()
            );

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
            .Count();
        return (int)MathF.Ceiling(1f * cheepCount / pageLength);
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
