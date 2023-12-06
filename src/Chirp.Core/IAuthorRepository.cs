namespace Chirp.Core;
public interface IAuthorRepository
{
    public Task<AuthorDTO> GetAuthorDTOByUsername(string username);
    public Task CreateNewAuthor(string name);

    public Task FollowAuthor(AuthorDTO authorDTO, AuthorDTO authorToFollowDTO);
    public Task UnfollowAuthor(AuthorDTO authorDTO, AuthorDTO authorToUnfollowDTO);
    public Task<IEnumerable<string>> GetFollowingUsernames(AuthorDTO authorDTO);
    public Task<bool> UsernameIsHidden(string username);
    Task<bool> UsernameExistsAsync(string username);
    public Task<int> GetAmmountOfCheeps(string username);
    public Task SetHidden(string username, bool hidden);
    public Task<List<AuthorDTO>> GetScoreboardAsync();
}
