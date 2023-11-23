namespace Chirp.Core;

public class AuthorDTO
{
    public required Guid AuthorId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public IEnumerable<CheepDTO> Cheeps { get; set; }
    public IEnumerable<AuthorDTO> Following { get; set; }
    public IEnumerable<AuthorDTO> Followers { get; set; }
};
