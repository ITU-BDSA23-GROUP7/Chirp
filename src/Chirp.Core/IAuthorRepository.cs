public record AuthorInfo(string Username, string Email);
public record AuthorDTO(int AuthorId, string Name, string Email, IEnumerable<CheepDTO> Cheeps, IEnumerable<AuthorDTO> Following, IEnumerable<AuthorDTO> Followers);
public interface IAuthorRepository
{
    Task<AuthorInfo> GetAuthorInfo(string username);
    void CreateNewAuthor(int authorId, string name, string email);

}