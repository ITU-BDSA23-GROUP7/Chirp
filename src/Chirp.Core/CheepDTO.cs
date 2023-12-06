namespace Chirp.Core;
public class CheepDTO
{
    public required Guid CheepId { get; set; }
    public required AuthorDTO Author { get; set; }
    public required string Message { get; set; }
    public required string Timestamp { get; set; }
}