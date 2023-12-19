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

        //Injecting the context into the database
        var context = new ChirpDBContext(builder.Options);
        context.Database.EnsureCreatedAsync(); // Applies the schema to the database

        //Creating sample Cheeps to the database
        DbInitializer.SeedDatabase(context);

        //Making an AuthorRepository from the database.
        _authorRepository = new AuthorRepository(context);//Here the repository should be created with our context data
    }

    [Fact]
    public async void CreateNewAuthorTest()
    {
        //Arrange

        //Act
        await _authorRepository.CreateNewAuthor("Casper");
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
    public async void UserCanBeHidden()
    {
        //Arrange
        var user = "John Doe";
        await _authorRepository.CreateNewAuthor(user);


        //Act
        await _authorRepository.SetHidden(user, true);

        //Assert
        Assert.True(await _authorRepository.UsernameIsHidden(user));
    }


    [Fact]
    public async void UserCanBecomeVisibleAgain()
    {
        //Arrange
        var user = "John Doe";
        await _authorRepository.CreateNewAuthor(user);

        //Act
        await _authorRepository.SetHidden(user, true);
        await _authorRepository.SetHidden(user, false);

        //Assert
        Assert.False(await _authorRepository.UsernameIsHidden(user));
    }

    [Fact]
    public async void FollowAuthor_WhenFollowingOtherAuthor_AddsAuthorToFollowing()
    {
        //Arrange
        var firstUser = "Casper";
        var secondUser = "Sebastian";
        await _authorRepository.CreateNewAuthor(firstUser);
        await _authorRepository.CreateNewAuthor(secondUser);

        //Act
        await _authorRepository.FollowAuthor(firstUser, secondUser);

        //Assert
        var casperFollowing = await _authorRepository.GetFollowingUsernames(firstUser);
        Assert.Equal(secondUser, casperFollowing.First());
        var sebastianFollowers = await _authorRepository.GetFollowersUsernames(secondUser);
        Assert.Equal(firstUser, sebastianFollowers.First());
    }


    [Fact]
    public async void UnfollowAuthor_WhenUnfollowingOtherAuthor_RemovesAuthorFromFollowing()
    {
        //Arrange
        var firstUser = "Casper";
        var secondUser = "Sebastian";
        await _authorRepository.CreateNewAuthor(firstUser);
        await _authorRepository.CreateNewAuthor(secondUser);

        //Act
        await _authorRepository.FollowAuthor(firstUser, secondUser);

        var maxFollowing = await _authorRepository.GetFollowingUsernames(firstUser);
        Assert.Equal(secondUser, maxFollowing.First());
        var sebastianFollowers = await _authorRepository.GetFollowersUsernames(secondUser);
        Assert.Equal(firstUser, sebastianFollowers.First());


        //Act
        await _authorRepository.UnfollowAuthor(firstUser, secondUser);

        //Assert
        var maxNewFollowing = await _authorRepository.GetFollowingUsernames(firstUser);
        Assert.Empty(maxNewFollowing);
        var sebastianNewFollowers = await _authorRepository.GetFollowersUsernames(secondUser);
        Assert.Empty(sebastianNewFollowers);
    }

    [Fact]
    public async void FollowAuthor_WhenAttemptToFollowsSelf_ThrowsCannotFollowSelfException()
    {
        // Arrange
        var user = "max";
        await _authorRepository.CreateNewAuthor(user);

        // Act & Assert
        await Assert.ThrowsAsync<CannotFollowSelfException>(async () => await _authorRepository.FollowAuthor(user, user));
    }

    [Fact]
    public async void FollowAuthor_WhenAttemptToUnfollowsSelf_ThrowsCannotFollowSelfException()
    {
        // Arrange
        var user = "max";
        await _authorRepository.CreateNewAuthor(user);

        // Act & Assert
        await Assert.ThrowsAsync<CannotFollowSelfException>(async () => await _authorRepository.UnfollowAuthor(user, user));
    }
}
