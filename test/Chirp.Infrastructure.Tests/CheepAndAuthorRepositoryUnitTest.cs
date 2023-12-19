namespace Chirp.Infrastructure.Tests;

public class CheepAndAuthorRepositoryUnitTest
{
    private readonly ICheepRepository _cheepService;
    private readonly SqliteConnection _connection;

    private readonly IAuthorRepository _authorService;
    public CheepAndAuthorRepositoryUnitTest()
    {
        //Building the connection to a database
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        var builder = new DbContextOptionsBuilder<ChirpDBContext>()
            .UseSqlite(_connection);

        //Injecting the context into the database
        var context = new ChirpDBContext(builder.Options);
        context.Database.EnsureCreatedAsync(); // Applies the schema to the database

        //Creating sample Cheeps to the database
        DbInitializer.SeedDatabase(context);

        //Making a CheepService from the database.
        _cheepService = new CheepRepository(context);//Here the repository should be created with our context data

        _authorService = new AuthorRepository(context);//And the context is also injected to this service
    }

    [Fact]
    public async void HiddenCheepsAreNotShown(){
        //Arange
        var username="Casper";
        await _authorService.CreateNewAuthor(username);
        await _cheepService.AddCheepAsync(username, "Hello world!!!");

        //Act
        var visibleUsersCheeps = await _cheepService.GetCheeps(1 , username);

        //Assert
        Assert.NotEmpty(visibleUsersCheeps);

        //Act
        await _authorService.SetHidden(username, true);
        var hiddenUsersCheeps = await _cheepService.GetCheeps(1 , username);

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
