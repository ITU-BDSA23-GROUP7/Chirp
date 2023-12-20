namespace Chirp.Infrastructure;

/// <summary>
/// Implements the <c>DbContext</c> interface.
/// Represents the database context for the Chirp-application, giving access to
/// <c>Cheeps</c> and <c>Authors</c> objects.
/// </summary>
public class ChirpDBContext : DbContext
{
    public DbSet<Cheep> Cheeps { get; set; }
    public DbSet<Author> Authors { get; set; }

    /// <summary>
    /// Initializes a new instance of <c>ChirpDBContext</c> with the given <c>DbContextOptions</c>.
    /// </summary>
    /// <param name="options">Options for configuring <c>ChirpDBContext</c> </param>
    public ChirpDBContext(DbContextOptions<ChirpDBContext> options) : base(options) { }
}
