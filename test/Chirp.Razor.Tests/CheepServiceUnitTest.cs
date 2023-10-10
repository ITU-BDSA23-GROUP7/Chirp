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
    public void GetCheepsFromAuthorJonradHasNoCheeps()
    {
        // Arrange
        ICheepService cheepService = new CheepService();
        List<CheepViewModel> jonradCheepList = cheepService.GetCheepsFromAuthor("Jonrad");

        // Act
        int length = jonradCheepList.Count;

        // Assert
        Assert.Equal(0, length);
    }



    /*public void GetPagesCountFromCheepCountPageCountIs29()
    {
        // Arrange


        // Act

        // Assert

    }*/
}