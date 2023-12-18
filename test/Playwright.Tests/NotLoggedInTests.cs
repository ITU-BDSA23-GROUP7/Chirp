namespace Playwright.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class NotLoggedInTests : PageTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task HelgeExistsTest()
    {
        await Page.GotoAsync("http://localhost:5273/");

        await Page.GetByRole(AriaRole.Link, new() { Name = "23" }).First.ClickAsync();

        await Page.GetByRole(AriaRole.Link, new() { Name = "Helge" }).ClickAsync();

        await Expect(Page.GetByRole(AriaRole.Listitem)).ToContainTextAsync("Hello, BDSA students!");
    }

    [Test]
    public async Task ScoreboardContainsAUserTest()
    {
        await Page.GotoAsync("http://localhost:5273/");

        await Page.GetByRole(AriaRole.Link, new() { Name = "scoreboard" }).ClickAsync();

        await Expect(Page.Locator("#messagelist")).ToContainTextAsync("CHEEP STREAK");
    }

    [Test]
    public async Task FrontPageIsFirstPageTest()
    {
        await Page.GotoAsync("http://localhost:5273/");

        await Page.GetByRole(AriaRole.Heading, new() { Name = "Public Timeline" }).ClickAsync();

        await Expect(Page.Locator("h2")).ToContainTextAsync("Public Timeline");

    }

}