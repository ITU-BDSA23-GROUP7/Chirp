namespace Chirp.Razor.Tests;

public class CheepServiceUnitTest
{
    private readonly ICheepRepository _cheepService;
    private readonly SqliteConnection _connection;

    //This service is used for some tests, but this is not the primary service tested in these tests
    private readonly IAuthorRepository _authorService;
    public CheepServiceUnitTest()
    {
        //Building the connection to a database
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>()
            .UseSqlite(_connection);

        //injecting the context into the database
        var context = new ChirpDBContext(builder.Options);
        context.Database.EnsureCreatedAsync(); // Applies the schema to the database

        //Creating sample Cheeps to the database
        DbInitializer.SeedDatabase(context);

        //Making a CheepService from the database.
        _cheepService = new CheepRepository(context);//Here the repository should be created with our context data

        _authorService = new AuthorRepository(context);//And the context is also injected to this service
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
    public async void AddCheepWillAddCheepToUser()
    {
        // Arrange
        string author = "Helge";
        string message = "Hello World!";

        // Act
        IEnumerable<CheepDTO> cheepList = await _cheepService.GetCheeps(1, author);
        int beforeCheepCount = cheepList.Count();

        await _cheepService.AddCheepAsync(author, message);

        cheepList = await _cheepService.GetCheeps(1, author);
        int afterCheepCount = cheepList.Count();

        // Assert
        Assert.Equal(beforeCheepCount + 1, afterCheepCount);
    }

    [Fact]
    public async void UserDoesNotExistWhenAddingCheep()
    {
        //Arrange
        var notExistingName = "Bobby";
        var message = "Hello World";

        //Act & Assert
        await Assert.ThrowsAsync<UsernameNotFoundException>(async () => await _cheepService.AddCheepAsync(notExistingName, message));
    }

    [Fact]
    public async void HiddenCheepsAreNotShown(){
        //Arange
        var username="Casper";
        await _authorService.CreateNewAuthor(username);
        await _cheepService.AddCheepAsync(username, "Hello world!!!");

        //Act
        await _authorService.SetHidden(username, true);
        IEnumerable<CheepDTO> hiddenUsersCheeps = await _cheepService.GetCheeps(1 , username);

        //Assert
        Assert.Empty(hiddenUsersCheeps);
    }

    //Hidden user cannot cheep
    [Fact]
    public async void HiddenUserCannotCheep(){
        //Arrange
        var hiddenName="Per";
        await _authorService.CreateNewAuthor(hiddenName);

        //Act
        await _authorService.SetHidden(hiddenName, true);

        //Assert        
        await Assert.ThrowsAsync<Exception>(async () => await _cheepService.AddCheepAsync(hiddenName, "Can i as a hidden user cheep?"));
    }
}
