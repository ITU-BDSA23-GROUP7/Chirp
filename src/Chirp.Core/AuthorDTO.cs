namespace Chirp.Core;

public class AuthorDTO
{
    public required Guid AuthorId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public List<CheepDTO>? Cheeps { get; } = new(); //This should maybe be changed to an IEnumerable, but right now we need Add, so it is a list
    public List<AuthorDTO>? Following { get; } = new(); //This should maybe be changed to an IEnumerable, but right now we need Add, so it is a list
    public List<AuthorDTO>? Followers { get; } = new(); //This should maybe be changed to an IEnumerable, but right now we need Add, so it is a list
};

