public record CheepDTO(string Author, string Message, string Timestamp);

public interface ICheepRepository
{
    Task<IEnumerable<CheepDTO>> GetCheeps(int pageNum = 1, string? author = null);
    int GetPageCount(string? auhtor = null);
    void PostCheep(CheepDTO cheep);
}