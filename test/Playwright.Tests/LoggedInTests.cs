namespace Playwright.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class LoggedInTests : PageTest
{

    [Test]
    public async Task TestUserCanLogInFollowAndUnfollowAndAddCheepsAndRemoveCheepsAndLogOut()
    {
        //go to website and log in
        await Page.GotoAsync("http://localhost:5273/");
        await Page.GetByRole(AriaRole.Link, new() { Name = "login" }).ClickAsync();
        await Page.GetByLabel("Username or email address").ClickAsync();
        await Page.GetByLabel("Username or email address").FillAsync("testUserITU2300@gmail.com");
        await Page.GetByLabel("Password").ClickAsync();
        await Page.GetByLabel("Password").FillAsync("BDSAnummer1Fag!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Sign in", Exact = true }).ClickAsync();

        //Make a cheep with the text "Hello Guys! This is my first Cheep!!!!", and check if it exists
        await Page.Locator("#CheepText").ClickAsync();
        await Page.Locator("#CheepText").FillAsync("Hello Guys! This is my first Cheep!!!!");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Cheep" }).ClickAsync();
        await Expect(Page.Locator("#messagelist")).ToContainTextAsync("testUserITU Hello Guys! This is my first Cheep!!!!");

        //Follow Daniel
        await Page.Locator("li").Filter(new() { HasText = "daniel-fich Follow fdasfdsas" }).GetByRole(AriaRole.Button).ClickAsync();

        //Check if the unfollow button now exists
        await Expect(Page.Locator("#messagelist")).ToContainTextAsync("Unfollow");

        //Follow Sebastian and Casper
        await Page.Locator("li").Filter(new() { HasText = "SebastianHylander Follow JEG FORSTÅR GODT DEN SHREK REFERENCE — 15-12-2023 11:42:" }).GetByRole(AriaRole.Button).ClickAsync();
        await Page.Locator("li").Filter(new() { HasText = "Casper2411 Follow Are YOU" }).GetByRole(AriaRole.Button).ClickAsync();

        //Go to own timeline, and check if the followers exist, and also own cheep
        await Page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();
        await Expect(Page.Locator("#messagelist")).ToContainTextAsync("SebastianHylander");
        await Expect(Page.Locator("#messagelist")).ToContainTextAsync("Casper2411");
        await Expect(Page.Locator("#messagelist")).ToContainTextAsync("daniel-fich");
        await Expect(Page.Locator("#messagelist")).ToContainTextAsync("Hello Guys! This is my first Cheep!!!!");

        //Unfollow Casper, and check if he is gone from the page
        await Page.Locator("li").Filter(new() { HasText = "Casper2411 Unfollow Somebody" }).GetByRole(AriaRole.Button).ClickAsync();
        var isCasperUnfollowed = await Page.QuerySelectorAsync("Casper2411");
        Assert.IsNull(isCasperUnfollowed);

        //Unfollow Daniel, and check if he is gone from the page
        await Page.Locator("li").Filter(new() { HasText = "daniel-fich Unfollow fdasfdsas" }).GetByRole(AriaRole.Button).ClickAsync();
        var isDanielUnfollowed = await Page.QuerySelectorAsync("daniel-fich");
        Assert.IsNull(isDanielUnfollowed);

        //Go to "about me" page and check that there should exist one cheep.
        await Page.GetByRole(AriaRole.Link, new() { Name = "about me" }).ClickAsync();
        await Expect(Page.Locator("body")).ToContainTextAsync("Cheeps: 1");

        //Delete the one cheep, and check if it the cheep count goes down
        await Page.GetByRole(AriaRole.Button, new() { Name = "Remove" }).ClickAsync();
        await Expect(Page.Locator("body")).ToContainTextAsync("Cheeps: 0");

        //Go to my timeline and unfollow Sebastian
        await Page.GetByRole(AriaRole.Link, new() { Name = "my timeline" }).ClickAsync();
        await Expect(Page.Locator("#messagelist")).ToContainTextAsync("SebastianHylander Unfollow gEt OuT Of mY sWamP!??!!11!!1!😂😂😂😂😂😂😂😂 — 16-12-2023 11:42:40");
        await Page.Locator("li").Filter(new() { HasText = "SebastianHylander Unfollow gEt OuT Of mY sWamP!??!!11!!1!😂😂😂😂😂😂😂😂 — 16-12-2023 11:42:40" }).GetByRole(AriaRole.Button).ClickAsync();

        //Logout of Chirp! And check if it succeeds
        await Page.GetByRole(AriaRole.Link, new() { Name = "logout [testUserITU]" }).ClickAsync();
        await Page.GetByRole(AriaRole.Heading, new() { Name = "Signed out" }).ClickAsync();
        await Expect(Page.Locator("h2")).ToContainTextAsync("Signed out");
        await Expect(Page.GetByRole(AriaRole.Paragraph)).ToContainTextAsync("You have successfully signed out.");
    }

}