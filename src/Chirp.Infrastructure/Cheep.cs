

using System.ComponentModel.DataAnnotations;

public class Cheep
{
    public required int CheepId { get; set; }
    [MaxLength(160, ErrorMessage = "The message cannot be longer than 160 characters")]
    public required string Text { get; set; }
    public required DateTime TimeStamp { get; set; }
    public required Author Author { get; set; }
}
