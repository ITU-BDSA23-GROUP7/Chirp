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
            Console.WriteLine("AddCheepModel constructor");
            _repository = repository;
        }

        public async Task OnPostAsync(String Username, String CheepText)
        {
            Console.WriteLine($"CheepText: {CheepText}");
            await _repository.AddCheepAsync(Username, CheepText);
        }
    }

}
