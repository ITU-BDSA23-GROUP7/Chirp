public record CheepDTO(string Author, string Message, string Timestamp);

public interface IChirpRepository : IDisposable
{
    Task<IEnumerable<CheepDTO>> GetCheeps(int pageNum = 1, string? author = null);

    int GetPageCount(string? auhtor = null);
}