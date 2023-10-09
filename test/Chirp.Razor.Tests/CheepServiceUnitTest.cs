namespace Chirp.Razor.Tests;

public class CheepServiceUnitTest
{
    [Fact]
    public void GetCheepsListLength32()
    {
        // Arrange
        ICheepService cheepService = new CheepService;
        cheepService

        // Act
        List<CheepViewModel> cheepList = cheepService.GetCheeps();




        // Assert
    }
}