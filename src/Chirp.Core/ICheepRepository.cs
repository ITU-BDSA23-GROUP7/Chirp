namespace Chirp.Core;
public interface ICheepRepository
{
    /// <summary>
    /// Gets cheeps from a specific page.
    /// </summary>
    /// <param name="pageNum">The page in question.</param>
    /// <param name="author">If specified, will only return cheeps from this author.</param>
    /// <returns>A task that returns a list of <c>CheepDTO</c>'s.</returns>
    Task<List<CheepDTO>> GetCheeps(int pageNum = 1, string? author = null);
    /// <summary>
    /// Returns the ammount of pages that <c>GetCheeps(...)</c> could return.
    /// </summary>
    /// <param name="auhtor"></param>
    /// <returns></returns>
    int GetPageCount(string? auhtor = null);
    Task AddCheepAsync(string username, string message);
    Task<List<CheepDTO>> GetFollowerCheeps(string author, int pageNum = 1);
    int GetFollowersPageCount(string author);
    Task RemoveCheep(Guid cheepId);
}
