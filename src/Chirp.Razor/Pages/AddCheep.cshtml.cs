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
            return Page();
        }

        [BindProperty]
        public string CheepText { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine($"CheepText: {CheepText}");
            await _repository.AddCheepAsync(User.Identity.Name, CheepText);
            return RedirectToPage("Public");
        }
    }

    public class AddCheepViewComponent : ViewComponent
    {
        private readonly ICheepRepository _repository;

        public AddCheepViewComponent(ICheepRepository repository)
        {
            _repository = repository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new AddCheepModel(_repository);
            return View(model);
        }
    }
}
