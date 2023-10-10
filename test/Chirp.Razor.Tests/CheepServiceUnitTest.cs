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
}