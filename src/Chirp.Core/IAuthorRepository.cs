namespace Chirp.Core;
public record AuthorInfo(string Username, string Email);
public interface IAuthorRepository
{
    Task<AuthorInfo> GetAuthorInfo(string username);
    void CreateNewAuthor(int authorId, string name, string email);

}