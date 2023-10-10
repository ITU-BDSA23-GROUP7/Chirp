namespace Chirp.Razor.Tests;

public class CheepServiceUnitTest
{
    [Fact]
    public void GetCheepsListLength32()
    {
        // Arrange
        ICheepService cheepService = new CheepService();
        List<CheepViewModel> cheepList = cheepService.GetCheeps();

        // Act
        int length = cheepList.Count;

        // Assert
        Assert.Equal(32, length);
    }

    [Fact]
    public void GetCheepsFromAuthorHelgeHasMoreThanZeroCheeps()
    {
        // Arrange
        ICheepService cheepService = new CheepService();
        List<CheepViewModel> helgeCheepList = cheepService.GetCheepsFromAuthor("Helge");

        // Act
        int length = helgeCheepList.Count;

        // Assert
        Assert.True(length >= 1, "Helge's list of cheeps did not have a length of 1 or more");
    }

    [Fact]
    public void GetCheepsFromAuthor6A6F6E726164HasNoCheeps()
    {
        // Arrange
        ICheepService cheepService = new CheepService();
        List<CheepViewModel> cheepList = cheepService.GetCheepsFromAuthor("6A6F6E726164");

        // Act
        int length = cheepList.Count;

        // Assert
        Assert.Equal(0, length);
    }

    [Fact]
    public void GetPageCountPageCountIsMoreThanOne()
    {
        // Arrange
        ICheepService cheepService = new CheepService();

        // Act
        int pageCount = cheepService.GetPageCount();

        // Assert
        Assert.True(pageCount >= 1, "Page count is less than 1");
    }

    [Fact]
    public void GetPageCountPageCountIsOneWhenNoCheeps()
    {
        // Arrange
        ICheepService cheepService = new CheepService();

        // Act
        int pageCount = cheepService.GetPageCountFromAuthor("6A6F6E726164");

        // Assert
        Assert.True(pageCount == 1, "Page count is not 1.");
    }
}