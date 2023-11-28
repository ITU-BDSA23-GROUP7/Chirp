namespace Chirp.Core;
public interface IAuthorRepository
{
    public Task<AuthorDTO> GetAuthorDTOByUsername(string username);
    public void CreateNewAuthor(string name);

    public Task FollowAuthor(AuthorDTO authorDTO, AuthorDTO authorToFollowDTO);
    public Task UnfollowAuthor(AuthorDTO authorDTO, AuthorDTO authorToUnfollowDTO);
    public Task<IEnumerable<string>> GetFollowingUsernames(AuthorDTO authorDTO);

    Task<bool> UsernameExistsAsync(string username);
    Task CreateNewAuthor(string name);

}
