namespace Chirp.Infrastructure;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Each cheep represents a post a user makes at a specific moment in time. For that reason
/// each attribute is required (not nullable).
/// </summary>
public class Cheep
{
    public required Guid CheepId { get; set; }
    [MaxLength(160, ErrorMessage = "The message cannot be longer than 160 characters")]
    public required string Text { get; set; }
    public required DateTime TimeStamp { get; set; }
    public required Author Author { get; set; }

    /// <summary>
    /// Returns a new <c>CheepDTO</c> object with all its properties set to the ones in <c>Cheep</c>.
    /// </summary>
    /// <returns></returns>
    public CheepDTO ToCheepDTO() {
        var cheepDTO = new CheepDTO
        {
            CheepId = CheepId,
            Author = Author.ToAuthorDTO(),
            Message = Text,
            Timestamp = TimeStamp.ToString()
        };

        return cheepDTO;
    }
}
