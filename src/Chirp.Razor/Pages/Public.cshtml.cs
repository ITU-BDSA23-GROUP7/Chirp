using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepRepository _repository;
    public required IEnumerable<CheepDTO> Cheeps { get; set; }
    public int PageCount { get; private set; }
    public AddCheepModel AddCheepModel{ get; set; }

    public PublicModel(ICheepRepository repository)
    {
        _repository = repository;
        AddCheepModel = new AddCheepModel(repository);
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
            Cheeps = await _repository.GetCheeps();
            return Page();
        }

        int pageNum;

        if (!int.TryParse(pageNumStr, out pageNum))
        {
            Cheeps = new List<CheepDTO>();
            return Page();
        }

        if (pageNum <= 0)
        {
            Cheeps = new List<CheepDTO>();
            return Page();
        }
        Cheeps = await _repository.GetCheeps(pageNum);
        return Page();
    }

    [BindProperty]
    public string CheepText { get; set; }
    public async Task<ActionResult> OnPostAsync()
    {
        await AddCheepModel.OnPostAsync(User.Identity.Name, CheepText);
        return RedirectToPage("Public");
    }
}
