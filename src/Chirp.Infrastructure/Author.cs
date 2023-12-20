namespace Chirp.Infrastructure;
public class Author
{
    public required Guid AuthorId { get; set; }
    public required string Name { get; set; }
    public string? Email { get; set; }
    public required List<Cheep> Cheeps { get; set; }
    public List<Author> Following { get; } = new();  //List of people the user follows
    public List<Author> Followers { get; } = new(); //List of people the user is following
    public int CheepStreak {get; set; } = 0;
    public bool Hidden { get; set; } = false; //Boolean used to hide a user



    /// <summary>
    /// Returns a new <c>AuthorDTO</c> object with all its properties set to the ones in <c>Author</c>
    /// </summary>
    /// <returns></returns>
    public AuthorDTO ToAuthorDTO() {
        var authorDTO = new AuthorDTO
        {
            Name = Name,
            Email = Email,
            CheepStreak = CheepStreak
        };

        return authorDTO;
    }
}
