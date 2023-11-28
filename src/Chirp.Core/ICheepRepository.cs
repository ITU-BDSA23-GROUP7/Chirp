namespace Chirp.Core;
public interface ICheepRepository
{
    Task<IEnumerable<CheepDTO>> GetCheeps(int pageNum = 1, string? author = null);
    int GetPageCount(string? auhtor = null);
    Task AddCheepAsync(string username, string message);
}
