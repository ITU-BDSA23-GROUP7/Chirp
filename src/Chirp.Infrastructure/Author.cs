public class Author
{
    public required Guid AuthorId { get; set; }
    public required string Name { get; set; }
    public string? Email { get; set; }
    public List<Cheep> Cheeps { get; set; }
    public List<Author> Following { get; } = new();  //List of people the user follows
    public List<Author> Followers { get;} = new(); //List of people the user is following
}
