namespace Chirp.Razor.Tests;

public class CheepServiceUnitTest
{
    private readonly ICheepRepository _cheepService;
    private readonly SqliteConnection _connection;
    public CheepServiceUnitTest()
    {
        //Building the connection to a database
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>()
            .UseSqlite(_connection);

        //injectin the context into the database
        var context = new ChirpDBContext(builder.Options);
        context.Database.EnsureCreatedAsync(); // Applies the schema to the database

        //Creating sample Cheeps to the database
        DbInitializer.SeedDatabase(context);

        //Making a CheepService from the database.
        _cheepService = new CheepRepository(context);//Here the repository should be created with our context data
    }

    [Fact]
    public async void GetCheepsListLength32()
    {
        // Arrange
        int pagesize = 32;

        // Act
        IEnumerable<CheepDTO> cheepList = await _cheepService.GetCheeps();
        int length = cheepList.Count();

        // Assert
        Assert.Equal(pagesize, length);
    }

    [Fact]
    public async void GetCheepsFromAuthorHelgeHasMoreThanZeroCheeps()
    {
        // Arrange
        IEnumerable<CheepDTO> helgeCheepList = await _cheepService.GetCheeps(author: "Helge");

        // Act
        int length = helgeCheepList.Count();

        // Assert
        Assert.True(length >= 1, "Helge's list of cheeps did not have a length of 1 or more");
    }

    [Fact]
    public async void GetCheepsFromAuthor6A6F6E726164HasNoCheeps()
    {
        //OBS. This test doesn't function if a Author has the name "6A6F6E726164"

        // Arrange
        IEnumerable<CheepDTO> cheepList = await _cheepService.GetCheeps(author: "6A6F6E726164");

        // Act
        int length = cheepList.Count();

        // Assert
        Assert.Equal(0, length);
    }

    [Fact]
    public void GetPageCountPageCountIsCorrect()
    {
        // Arrange
        int expectedPageCount = 21;

        // Act
        int actualPageCount = _cheepService.GetPageCount();

        // Assert
        Assert.Equal(expectedPageCount, actualPageCount);
    }

    // Test will currently only work under the assumption that "Helge" has less than 31 cheeps
    [Fact]
    public async void PostCheepWillAddCheepToUser()
    {
        // Arrange
        var newCheep = new CheepDTO
        {
            Author = new AuthorDTO
            {
                AuthorId = 0,
                Name = "Helge",
                Email = "test@test.test"
            },
            Message = "Hello world",
            Timestamp = ""
        };


        // Act
        IEnumerable<CheepDTO> cheepList = await _cheepService.GetCheeps(1, newCheep.Author.Name);
        int beforeCheepCount = cheepList.Count();

        _cheepService.PostCheep(newCheep);

        cheepList = await _cheepService.GetCheeps(1, newCheep.Author.Name);
        int afterCheepCount = cheepList.Count();

        // Assert
        Assert.Equal(beforeCheepCount + 1, afterCheepCount);
    }
}
