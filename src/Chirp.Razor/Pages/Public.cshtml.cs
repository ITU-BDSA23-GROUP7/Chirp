using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepRepository _repository;
    public required IEnumerable<CheepDTO> Cheeps { get; set; }
    public int PageCount { get; private set; }

    public PublicModel(ICheepRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    ///     Sets PageCount to the amount of total pages. <br/>
    ///     Sets Cheeps depending on the "page" query parameter. <br/>
    ///     * Will set Cheeps to the first page if no parameter is specified. <br/>
    ///     * Will set Cheeps to an empty list if parameter is not a positive integer. <br/>
    /// </summary>
    /// <returns></returns>
    public async Task<ActionResult> OnGet()
    {
        PageCount = _repository.GetPageCount();

        string pageNumStr = Request.Query["page"]!;

        if (pageNumStr == null)
        {
            Console.WriteLine("Page number is a null value.");
            Cheeps = await _repository.GetCheeps();
            return Page();
        }

        int pageNum;

        if (!int.TryParse(pageNumStr, out pageNum))
        {
            Console.WriteLine("Page number isn't an integer.");
            Cheeps = new List<CheepDTO>();
            return Page();
        }

        if (pageNum <= 0)
        {
            Console.WriteLine("Page number is less than or equal to 0.");
            Cheeps = new List<CheepDTO>();
            return Page();
        }

        Cheeps = await _repository.GetCheeps(pageNum);
        return Page();
    }
}
