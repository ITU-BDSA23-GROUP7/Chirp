public class Author
{
    public required Guid AuthorId { get; set; }
    public required string Name { get; set; }
    public string? Email { get; set; }
    public required IEnumerable<Cheep> Cheeps { get; set; }
    public required IEnumerable<Author> Following { get; set; }  //List of people the user follows
    public required IEnumerable<Author> Followers { get; set; }  //List of people the user is following
}
