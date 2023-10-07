

public class Cheep
{
    public required int CheepId { get; set; }
    public required string Text { get; set; }
    public required DateTime TimeStamp { get; set; }
    public required Author Author { get; set; }
}

public class Author
{
    public required int AuthorId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public IEnumerable<Cheep> Cheeps { get; set; }

}
