public class Author
{
    public required Guid AuthorId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public IEnumerable<Cheep> Cheeps { get; set; }

}
