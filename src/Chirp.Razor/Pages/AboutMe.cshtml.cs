using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class AboutMe : PageModel
{
    public IActionResult OnGet()
    {
        if(!User.Identity.IsAuthenticated)
        {
            return Redirect("/");
        }

        return null;
    }
}
