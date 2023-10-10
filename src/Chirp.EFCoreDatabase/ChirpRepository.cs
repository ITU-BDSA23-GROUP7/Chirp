public record CheepDTO(string Author, string Message, string Timestamp);

public interface IChirpRepository : IDisposable {
    Task<IEnumerable<CheepDTO>> GetCheeps(int page = 1);
    Task<IEnumerable<CheepDTO>> GetCheepsByAuthor(string author, int page = 1);

}

public class ChirpRepository : IChirpRepository {
    private int pageLength = 32;
    private ChirpDBContext context;
    public ChirpRepository(ChirpDBContext context) {
        this.context = context;

        // Adds example data to the database if nothing has been added yet
        DbInitializer.SeedDatabase(context);
    }
    public async Task<IEnumerable<CheepDTO>> GetCheeps(int pageNum = 1)
    {
        int pageIndex = pageNum - 1;

        List<Cheep> cheeps = await context.Cheeps
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
    public Task<IEnumerable<CheepDTO>> GetCheepsByAuthor(string author, int page = 1)
    {
        throw new NotImplementedException();
    }
    protected virtual void Dispose(bool disposing) {

    }
    public void Dispose() {
        Dispose(true);
    }
}
