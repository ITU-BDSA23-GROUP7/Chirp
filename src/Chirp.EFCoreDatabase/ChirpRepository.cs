
public interface IChirpRepository : IDisposable {
    Task<IEnumerable<Cheep>> GetCheeps(int page = 1);
    Task<IEnumerable<Cheep>> GetCheepsByAuthor(string author, int page = 1);

}

public class ChirpRepository : IChirpRepository {
    private ChirpDBContext context;
    public ChirpRepository(ChirpDBContext context) {
        this.context = context;
    }
    public async Task<IEnumerable<Cheep>> GetCheeps(int page = 1)
    {
        return await context.Cheeps
            .OrderBy(c => c.TimeStamp)
            .Include(c => c.Author)
            .ToListAsync();
    }
    public Task<IEnumerable<Cheep>> GetCheepsByAuthor(string author, int page = 1)
    {
        throw new NotImplementedException();
    }
    protected virtual void Dispose(bool disposing) {

    }
    public void Dispose() {
        Dispose(true);
    }
}
