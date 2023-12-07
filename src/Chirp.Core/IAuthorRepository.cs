namespace Chirp.Core;
public interface IAuthorRepository
{
    Task<AuthorDTO> GetAuthorDTOByUsername(string username);
    Task CreateNewAuthor(string name);
    Task FollowAuthor(AuthorDTO authorDTO, AuthorDTO authorToFollowDTO);
    Task UnfollowAuthor(AuthorDTO authorDTO, AuthorDTO authorToUnfollowDTO);
    Task<IEnumerable<string>> GetFollowingUsernames(AuthorDTO authorDTO);
    Task<IEnumerable<string>> GetFollowersUsernames(AuthorDTO authorDTO);
    Task<bool> UsernameIsHidden(string username);
    Task<bool> UsernameExistsAsync(string username);
    Task<int> GetAmmountOfCheeps(string username);
    Task SetHidden(string username, bool hidden);
    Task<List<AuthorDTO>> GetScoreboardAsync();
}
