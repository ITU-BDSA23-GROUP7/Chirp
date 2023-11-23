using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepRepository _repository;
    public required IEnumerable<CheepDTO> Cheeps { get; set; }
    public int PageCount { get; private set; }
    public AddCheepModel AddCheepModel{ get; set; }

    public UserTimelineModel(ICheepRepository repository)
    {
        _repository = repository;
        AddCheepModel = new AddCheepModel(repository);
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
        PageCount = _repository.GetPageCount(author);

        string pageNumStr = Request.Query["page"]!;

        if (pageNumStr == null)
        {
            Cheeps = await _repository.GetCheeps(1, author);
            return Page();
        }

        int pageNum;

        if (!int.TryParse(pageNumStr, out pageNum))
        {
            Cheeps = await _repository.GetCheeps(1, author);
            return Page();
        }

        if (pageNum < 0)
        {
            Cheeps = await _repository.GetCheeps(1, author);
            return Page();
        }

        Cheeps = await _repository.GetCheeps(pageNum, author);
        return Page();
    }

    [BindProperty]
    public string CheepText { get; set; }
    public async Task<ActionResult> OnPostAsync()
    {
        await AddCheepModel.OnPostAsync(User.Identity.Name, CheepText);
        return RedirectToPage("UserTimeline", new { author = User.Identity.Name });
    }
}
