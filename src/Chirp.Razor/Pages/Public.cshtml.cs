﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepService _service;
    public List<CheepViewModel> Cheeps { get; set; }
    public int PageCount { get; private set; }

    public PublicModel(ICheepService service)
    {
        _service = service;
    }
    
    /// <summary>
    ///     Sets PageCount to the amount of total pages. <br/>
    ///     Sets Cheeps depending on the "page" query parameter. <br/>
    ///     * Will set Cheeps to the first page if no parameter is specified. <br/>
    ///     * Will set Cheeps to an empty list if parameter is not a positive integer. <br/>
    /// </summary>
    /// <returns></returns>
    public ActionResult OnGet()
    {
        PageCount = _service.GetPageCount();

        string pageNumStr = Request.Query["page"];

        if (pageNumStr == null) {
            Console.WriteLine("Page number is a null value.");
            Cheeps = _service.GetCheeps();
            return Page();
        }

        int pageNum;

        if (!int.TryParse(pageNumStr, out pageNum)) {
            Console.WriteLine("Page number isn't an integer.");
            Cheeps = new List<CheepViewModel>();
            return Page();
        }

        if (pageNum <= 0) {
            Console.WriteLine("Page number is less than or equal to 0.");
            Cheeps = new List<CheepViewModel>();
            return Page();
        }

        Cheeps = _service.GetCheeps(pageNum);
        return Page();
    }
}
