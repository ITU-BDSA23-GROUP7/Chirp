namespace Chirp.Infrastructure.Tests;

public class AuthorRepositoryUnitTest
{
    private readonly IAuthorRepository _authorRepository;
    private readonly SqliteConnection _connection;
    public AuthorRepositoryUnitTest(){
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
    public void Test()
    {
        Assert.Equal(1,1);
    }

    [Fact]
    public void CreateNewAuthorTest()
    {
        //Arrange
        
        //Act
        _authorRepository.CreateNewAuthor(23, "Casper", "Ben@Dover.com");
        AuthorInfo newUser = _authorRepository.GetAuthorInfo("Casper").Result;

        //Assert
        Assert.Equal("Ben@Dover.com", newUser.Email);
    }
}
