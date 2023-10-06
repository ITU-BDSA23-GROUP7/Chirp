using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepViewModel> Cheeps { get; set; }

    public PublicModel(ICheepService service)
    {
        _service = service;
    }
    
    public ActionResult OnGet()
    {
        string pageNumStr = Request.Query["page"];

        if (pageNumStr == null) {
            Console.WriteLine("Page number is a null value.");
            Cheeps = _service.GetCheeps();
            return Page();
        }

        int pageNum;

        if (!int.TryParse(pageNumStr, out pageNum)) {
            Console.WriteLine("Page number isn't an integer.");
            Cheeps = _service.GetCheeps(-1);
            return Page();
        }

        if (pageNum < 0) {
            Console.WriteLine("Page number is less than 0.");
            Cheeps = _service.GetCheeps(-1);
            return Page();
        }

        Cheeps = _service.GetCheeps(pageNum);
        return Page();
    }
}
