using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages
{
    public class AddCheepModel : PageModel
    {
        private readonly ICheepRepository _repository;

        public AddCheepModel(ICheepRepository repository)
        {
            _repository = repository;
        }

        public IActionResult OnGet()
        {
            //Please uncomment when AddCheeps implemented...
            /*
            if(!User.Identity!.IsAuthenticated){
                return RedirectToPage("Public");
            }
            */
            return Page();
        }

        [BindProperty]
        public string CheepText { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine(CheepText);
            return RedirectToPage("Public");
        }
    }
}
