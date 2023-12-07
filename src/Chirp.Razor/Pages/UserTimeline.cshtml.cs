using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepRepository _cheepRepository;
    private readonly IAuthorRepository _authorRepository;
    public required IEnumerable<CheepDTO> Cheeps { get; set; }
    public required List<string> Following { get; set; }
    public int PageCount { get; private set; }
    public AddCheepModel AddCheepModel{ get; set; }

    public UserTimelineModel(ICheepRepository cheepRepository, IAuthorRepository authorRepository)
    {
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
        AddCheepModel = new AddCheepModel(cheepRepository);
    }

    /// <summary>
    ///     Sets PageCount to the amount of total pages from the given author. <br/>
    ///     Sets Cheeps depending on the "page" query parameter. <br/>
    ///     * Will set Cheeps to the first page if no parameter is specified. <br/>
    ///     * Will set Cheeps to an empty list if parameter is not a positive integer. <br/>
    /// </summary>
    /// <param name="author">The name of the author.</param>
    /// <returns></returns>
    public async Task<IActionResult> OnGet(string author)
    {
        PageCount = _cheepRepository.GetPageCount(author);

        if (User.Identity == null) {
            return Page();
        }

        if (User.Identity.IsAuthenticated)
        {
            var username = User.Identity.Name;
            if (username != null) 
            {
                if (!await _authorRepository.UsernameExistsAsync(username))
                {
                    await _authorRepository.CreateNewAuthor(username);
                }
                var following = await _authorRepository.GetFollowingUsernames(username);
                Following = following.ToList();
            }
        }

        if(User.Identity.IsAuthenticated && author == User.Identity.Name)
        {
            await LoadPersonalTimeline(author);
        }
        else
        {
            await LoadAuthorTimeline(author);
        }

        return Page();
    }

    private async Task LoadPersonalTimeline(string author)
    {
        string pageNumStr = Request.Query["page"]!;

        if (pageNumStr == null)
        {
            Cheeps = await _cheepRepository.GetFollowerCheeps(author, 1);

        }

        int pageNum;

        if (!int.TryParse(pageNumStr, out pageNum))
        {
            Cheeps = await _cheepRepository.GetFollowerCheeps(author, 1);

        }

        if (pageNum < 0)
        {
            Cheeps = await _cheepRepository.GetFollowerCheeps(author, 1);

        }

        Cheeps = await _cheepRepository.GetFollowerCheeps(author, pageNum);
    }

    private async Task LoadAuthorTimeline(string author)

    {
        string pageNumStr = Request.Query["page"]!;

        if (pageNumStr == null)
        {
            Cheeps = await _cheepRepository.GetCheeps(1, author);
            
        }

        int pageNum;

        if (!int.TryParse(pageNumStr, out pageNum))
        {
            Cheeps = await _cheepRepository.GetCheeps(1, author);
            
        }

        if (pageNum < 0)
        {
            Cheeps = await _cheepRepository.GetCheeps(1, author);
            
        }

        Cheeps = await _cheepRepository.GetCheeps(pageNum, author);
        
    }

    [BindProperty]
    public string? method { get; set; }
    public async Task<IActionResult> OnPostAsync()
    {
        switch (method)
        {
            case "follow":
                await OnPostFollow();
                break;
            case "unfollow":
                await OnPostUnfollow();
                break;
            case "addCheep":
                await OnPostAddCheep();
                break;
        }

        var routeValue = HttpContext.GetRouteValue("author");

        if (routeValue == null) {
            return Page();
        }

        return await OnGet((string) routeValue);
    }

    [BindProperty]
    public string? authorName { get; set; }
    public async Task OnPostFollow()
    {
        if (User.Identity == null || User.Identity.Name == null || authorName == null) {
            return;
        }

        if (User.Identity.IsAuthenticated)
        {
            await _authorRepository.FollowAuthor(User.Identity.Name, authorName);
        }

    }

    public async Task OnPostUnfollow()
    {
        if (User.Identity == null || User.Identity.Name == null || authorName == null) {
            return;
        }

        if (User.Identity.IsAuthenticated)
        {
            await _authorRepository.UnfollowAuthor(User.Identity.Name, authorName);
        }
    }

    [BindProperty]
    public string? CheepText { get; set; }
    public async Task OnPostAddCheep()
    {
        if (User.Identity == null || User.Identity.Name == null || CheepText == null)
        {
            return;
        }

        await AddCheepModel.OnPostAsync(User.Identity.Name, CheepText);
    }
}
