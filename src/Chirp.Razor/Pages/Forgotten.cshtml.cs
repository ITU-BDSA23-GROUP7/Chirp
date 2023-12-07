using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class Forgotten : PageModel
{
    private readonly IAuthorRepository _authorRepository;
    public Forgotten(IAuthorRepository authorRepository, ICheepRepository cheepRepository)
    {
        _authorRepository = authorRepository;
    }
    public async Task<IActionResult> OnGet()
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated || User.Identity.Name == null) {
            return Redirect("/");
        }

        await _authorRepository.SetHidden(User.Identity.Name, true);

        return Page();
    }
}
