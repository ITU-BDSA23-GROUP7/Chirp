namespace Chirp.Core;
public interface IAuthorRepository
{
    public Task<AuthorDTO> GetAuthorDTOByUsername(string username);
    public void CreateNewAuthor(int authorId, string name, string email);

    public Task FollowAuthor(AuthorDTO authorDTO, AuthorDTO authorToFollowDTO);
    public Task UnfollowAuthor(AuthorDTO authorDTO, AuthorDTO authorToUnfollowDTO);
    public Task<IEnumerable<string>> GetFollowingUsernames(AuthorDTO authorDTO);

}