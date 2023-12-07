using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages
{
    public class ScoreboardModel : PageModel
    {
        public required List<AuthorDTO> Scores { get; set; }
        public IAuthorRepository _authorRepository { get; set; }

        public ScoreboardModel(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }
        public async Task<IActionResult> OnGet()
        {
            Scores = await _authorRepository.GetScoreboardAsync();
            return Page();
        }
    }
}
