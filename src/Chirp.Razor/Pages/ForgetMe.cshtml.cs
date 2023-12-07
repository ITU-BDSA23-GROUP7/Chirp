using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class ForgetMe : PageModel
{
    public IActionResult OnGet()
    {
        if(User.Identity == null || !User.Identity.IsAuthenticated)
        {
            return Redirect("/");
        }

        return Page();
    }
}
