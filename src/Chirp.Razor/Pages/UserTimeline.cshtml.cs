using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepService _service;
    public IEnumerable<CheepDTO> Cheeps { get; set; }
    public int PageCount { get; private set; }

    public UserTimelineModel(ICheepService service)
    {
        _service = service;
    }

    /// <summary>
    ///     Sets PageCount to the amount of total pages from the given author. <br/>
    ///     Sets Cheeps depending on the "page" query parameter. <br/>
    ///     * Will set Cheeps to the first page if no parameter is specified. <br/>
    ///     * Will set Cheeps to an empty list if parameter is not a positive integer. <br/>
    /// </summary>
    /// <param name="author">The name of the author.</param>
    /// <returns></returns>
    public async Task<ActionResult> OnGet(string author)
    {
        PageCount = _service.GetPageCountFromAuthor(author);

        string pageNumStr = Request.Query["page"];

        if (pageNumStr == null)
        {
            Console.WriteLine("Page number is a null value.");
            Cheeps = await _service.GetCheepsFromAuthor(author);
            return Page();
        }

        int pageNum;

        if (!int.TryParse(pageNumStr, out pageNum))
        {
            Console.WriteLine("Page number isn't an integer.");
            Cheeps = await _service.GetCheepsFromAuthor(author, -1);
            return Page();
        }

        if (pageNum < 0)
        {
            Console.WriteLine("Page number is less than 0.");
            Cheeps = await _service.GetCheepsFromAuthor(author, -1);
            return Page();
        }

        Cheeps = await _service.GetCheepsFromAuthor(author, pageNum);
        return Page();
    }
}
