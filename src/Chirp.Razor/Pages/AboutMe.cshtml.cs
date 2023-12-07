using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class AboutMe : PageModel
{
    private readonly IAuthorRepository _authorRepository;
    private readonly ICheepRepository _cheepRepository;
    public required IEnumerable<CheepDTO> Cheeps { get; set; }
    public int PageCount { get; private set; }
    public required AuthorDTO Author { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required int NumberOfCheeps { get; set; }


    public AboutMe(IAuthorRepository authorRepository, ICheepRepository cheepRepository)
    {
        _authorRepository = authorRepository;
        _cheepRepository = cheepRepository;
        Username = "[No username]";
        Email = "[No email stored]";
        NumberOfCheeps = 0;
    }

    public async Task SetUserinfo(string? author)
    {
        if (author == null) {
            return;
        }

        Username = author;
        Author = await _authorRepository.GetAuthorDTOByUsername(author);

        if (Author.Email != null && !Email.Equals(""))
        {
            Email = Author.Email;
        }
        //Amount of cheeps the user has Cheeped
        NumberOfCheeps = await _authorRepository.GetAmmountOfCheeps(author);
    }

    public async Task SetCheeps(string? author) {
        if (author == null) {
            return;
        }

        PageCount = _cheepRepository.GetPageCount(author);

        string pageNumStr = Request.Query["page"]!;

        if (pageNumStr == null)
        {
            Cheeps = await _cheepRepository.GetCheeps(1, author);
            return;
        }

        int pageNum;

        if (!int.TryParse(pageNumStr, out pageNum))
        {
            Cheeps = await _cheepRepository.GetCheeps(1, author);
            return;
        }

        if (pageNum < 0)
        {
            Cheeps = await _cheepRepository.GetCheeps(1, author);
            return;
        }

        Cheeps = await _cheepRepository.GetCheeps(pageNum, author);
        return;
    }

    public async Task<IActionResult> OnGet()
    {
        if(User.Identity == null || !User.Identity.IsAuthenticated)
        {
            return Redirect("/");
        }

        //await _authorRepository.SetHidden(User.Identity.Name, false);

        await SetUserinfo(User.Identity.Name);

        await SetCheeps(User.Identity.Name);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(Guid cheepId)
    {
        await _cheepRepository.RemoveCheep(cheepId);
        return RedirectToPage("AboutMe");
    }
}
