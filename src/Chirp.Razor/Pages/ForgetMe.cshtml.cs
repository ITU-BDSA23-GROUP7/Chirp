using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class ForgetMe : PageModel
{
    public async Task<IActionResult> OnGet()
    {
        if(!User.Identity.IsAuthenticated)
        {
            return Redirect("/");
        }

        return null;
    }
}
