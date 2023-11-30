using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class AboutMe : PageModel
{
    private readonly IAuthorRepository _repository;
    public required AuthorDTO Author { get; set; }
    public required string Email { get; set; }
    public required int NumberOfCheeps { get; set; }


    public AboutMe(IAuthorRepository repository)
    {
        _repository = repository;
    }

    public async Task SetUserinfo()
    {
        Author = await _repository.GetAuthorDTOByUsername(User.Identity.Name);

        Email = Author.Email;
        Console.WriteLine($"-{Author.Email}-");
        if (Email == null || Email.Equals(""))
        {
            Email = "[No email stored]";
        }
        //Cheeps not implementet in repository yet...
        NumberOfCheeps = 0;
    }

    public async Task<IActionResult> OnGet()
    {
        if(!User.Identity.IsAuthenticated)
        {
            return Redirect("/");
        }

        await SetUserinfo();
        return null;
    }
}
