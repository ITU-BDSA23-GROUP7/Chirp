namespace Chirp.Core;
public interface IAuthorRepository
{
    Task<AuthorDTO> GetAuthorDTOByUsername(string username);
    Task CreateNewAuthor(string name);
    Task FollowAuthor(string newFollowerUsername, string newFollowingUsername);
    Task UnfollowAuthor(string oldFollowerUsername, string oldFollowingUsername);
    Task<IEnumerable<string>> GetFollowingUsernames(string username);
    Task<IEnumerable<string>> GetFollowersUsernames(string username);
    Task<bool> UsernameIsHidden(string username);
    Task<bool> UsernameExistsAsync(string username);
    Task<int> GetAmmountOfCheeps(string username);
    Task SetHidden(string username, bool hidden);
    Task<List<AuthorDTO>> GetScoreboardAsync();
}
