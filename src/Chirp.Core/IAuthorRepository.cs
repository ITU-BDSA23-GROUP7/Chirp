namespace Chirp.Core;
public record AuthorInfo(string Username, string Email);
public interface IAuthorRepository
{
    Task<AuthorInfo> GetAuthorInfo(string username);

    Task<bool> UsernameExistsAsync(string username);
    Task CreateNewAuthor(string name);

}
