using SimpleDB;

namespace Chirp.CSVDB.Tests;

public class UnitTest1
{
    public record Cheep(string Author, string Message, long Timestamp);
    [Fact]
    public void ReadTypes()
    {
        // Arrange
        IDatabaseRepository<Cheep> testDB = CSVDatabase<Cheep>.getInstance();

        // Act
        var newCheep = testDB.Read(1).First();

        // Assert
        Assert.IsType<String>(newCheep.Author);
        Assert.IsType<String>(newCheep.Message);
        Assert.IsType<long>(newCheep.Timestamp);
    }
}
