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
    /// </summary>
    /// <param name="auhtor"></param>
    /// <returns>The ammount of pages that <c>GetCheeps(...)</c> could return.</returns>
    int GetPageCount(string? auhtor = null);
    Task AddCheepAsync(string username, string message);
    /// <summary>
    /// Gets the cheeps from the authors that <c>author</c> follows as well as the cheeps from
    /// <c>author</c>. Within a specific page.
    /// </summary>
    /// <param name="author">The author in question.</param>
    /// <param name="pageNum">The page in question.</param>
    /// <returns>A task that returns a list of <c>CheepDTO</c>'s.</returns>
    Task<List<CheepDTO>> GetFollowerCheeps(string author, int pageNum = 1);
    /// <summary>
    /// </summary>
    /// <param name="auhtor"></param>
    /// <returns>The ammount of pages that <c>GetFollowerCheeps(...)</c> could return.</returns>
    int GetFollowersPageCount(string author);
    Task RemoveCheep(Guid cheepId);
}
