public record AuthorInfo(string UserName, string Email);

public interface IAuthorRepository : IDisposable
{
    Task<AuthorInfo> GetAuthorInfo(string userName);

}