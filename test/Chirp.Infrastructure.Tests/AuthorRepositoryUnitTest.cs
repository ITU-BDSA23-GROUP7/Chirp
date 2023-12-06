namespace Chirp.Infrastructure.Tests;

public class AuthorRepositoryUnitTest
{
    private readonly IAuthorRepository _authorRepository;
    private readonly SqliteConnection _connection;
    public AuthorRepositoryUnitTest()
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

        //Making a AuthorRepository from the database.
        _authorRepository = new AuthorRepository(context);//Here the repository should be created with our context data
    }

    [Fact]
    public async void CreateNewAuthorTest()
    {
        //Arrange

        //Act
        _authorRepository.CreateNewAuthor("Casper");
        AuthorDTO newUser = await _authorRepository.GetAuthorDTOByUsername("Casper");

        //Assert
        Assert.Equal("Casper", newUser.Name);
    }

    [Fact]
    public async void UserDoesNotExistTest()
    {
        //Arrange
        var notExistingName = "Bobby";

        //Act & Assert
        await Assert.ThrowsAsync<UsernameNotFoundException>(async () => await _authorRepository.GetAuthorDTOByUsername(notExistingName));
    }

    [Fact]
    public async void UserDoesExist()
    {
        //Arrange
        var existingName = "Rasmus";

        //Act & Assert
        var foundAuthor = await _authorRepository.GetAuthorDTOByUsername(existingName);
    }

    [Fact]
    public async void UserCanBeHidden(){
        //Arrange
        var user = "John Doe";
        await _authorRepository.CreateNewAuthor(user);


        //Act
        _authorRepository.SetHidden(user, true);

        //Assert
        Assert.True(await _authorRepository.UsernameIsHidden(user));
    }


    [Fact]
    public async void UserCanBecomeVisibleAgain(){
        //Arrange
        var user = "John Doe";
        await _authorRepository.CreateNewAuthor(user);

        //Act
        _authorRepository.SetHidden(user, true);
        _authorRepository.SetHidden(user, false);

        //Assert
        Assert.False(await _authorRepository.UsernameIsHidden(user));
    }
}