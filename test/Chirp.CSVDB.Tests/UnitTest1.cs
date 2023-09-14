using SimpleDB;

namespace Chirp.CSVDB.Tests;

public class UnitTest1
{
    public record Cheep(string Author, string Message, long Timestamp);
    [Fact]
    public void ConversionOfRecordTest()
    {
        // Arrange
        IDatabaseRepository<Cheep> testDB = new CSVDatabase<Cheep>("../../../testdb.csv");
        
        // Act
        var oldCheep = new Cheep("JensHansen","Jeg har en bondegaard",231235543);
        testDB.Store(oldCheep);

        var newCheep = testDB.Read(1).First();

        // Assert
        Assert.Equal("JensHansen",newCheep.Author);
        Assert.Equal("Jeg har en bondegaard",newCheep.Message);
        Assert.Equal(231235543,newCheep.Timestamp);


    }
}