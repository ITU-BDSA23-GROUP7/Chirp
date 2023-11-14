public record AuthorInfo(string Username, string Email);

public interface IAuthorRepository
{
    Task<AuthorInfo> GetAuthorInfo(string username);
    void CreateNewAuthor(string name, string email);

}
