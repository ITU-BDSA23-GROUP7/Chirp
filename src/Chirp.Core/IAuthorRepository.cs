public record AuthorInfo(string Username, string Email);

public interface IAuthorRepository : IDisposable
{
    Task<AuthorInfo> GetAuthorInfo(string username);
    void CreateNewAuthor(int authorId, string name, string email);

}