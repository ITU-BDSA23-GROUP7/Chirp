using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepRepository _repository;
    private readonly IAuthorRepository _authorRepository;
    public required IEnumerable<CheepDTO> Cheeps { get; set; }
    public required List<string> Following { get; set; }
    public int PageCount { get; private set; }
    public AddCheepModel AddCheepModel { get; set; }

    [BindProperty]
    public CheepDTO CheepDTO { get; set; }
    [BindProperty]
    public AuthorDTO AuthorDTO { get; set; }

    public PublicModel(ICheepRepository repository, IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
        _repository = repository;
    }

    /// <summary>
    ///     Sets PageCount to the amount of total pages. <br/>
    ///     Sets Cheeps depending on the "page" query parameter. <br/>
    ///     * Will set Cheeps to the first page if no parameter is specified. <br/>
    ///     * Will set Cheeps to an empty list if parameter is not a positive integer. <br/>
    /// </summary>
    /// <returns></returns>
    public async Task<ActionResult> OnGet()
    {
        PageCount = _repository.GetPageCount();

        string pageNumStr = Request.Query["page"]!;

        if (User.Identity.IsAuthenticated)
        {
            var username = User.Identity.Name;
            if (!await _authorRepository.UsernameExistsAsync(username))
            {
                await _authorRepository.CreateNewAuthor(username);
            }
            var authorDTO = await _authorRepository.GetAuthorDTOByUsername(username);

            //temp test
            var authorDTO2 = await _authorRepository.GetAuthorDTOByUsername("Roger Histand");
            //await _authorRepository.FollowAuthor(authorDTO, authorDTO2);


            var following = await _authorRepository.GetFollowingUsernames(authorDTO);
            Following = following.ToList();
        }

        if (pageNumStr == null)
        {
            Cheeps = await _repository.GetCheeps();
            return Page();
        }

        int pageNum;

        if (!int.TryParse(pageNumStr, out pageNum))
        {
            Cheeps = new List<CheepDTO>();
            return Page();
        }

        if (pageNum <= 0)
        {
            Cheeps = new List<CheepDTO>();
            return Page();
        }
        Cheeps = await _repository.GetCheeps(pageNum);
        return Page();
    }

    [BindProperty]
    public string CheepText { get; set; }
    public async Task OnPostAsync(String Username, String CheepText)
    {
        Console.WriteLine($"CheepText: {CheepText}, and user {User.Identity.Name}");
        await _repository.AddCheepAsync(User.Identity.Name, CheepText);
    }
    

    public async Task<IActionResult> OnPostFollow(string authorName)
    {
        if (User.Identity.IsAuthenticated)
        {
            var userDTO = await _authorRepository.GetAuthorDTOByUsername(User.Identity.Name);
            var authorDTO = await _authorRepository.GetAuthorDTOByUsername(authorName);
            await _authorRepository.FollowAuthor(userDTO, authorDTO);
        }
        return RedirectToPage("Public");
    }

    public async Task<IActionResult> OnPostUnfollow(string authorName)
    {
        if (User.Identity.IsAuthenticated)
        {
            var userDTO = await _authorRepository.GetAuthorDTOByUsername(User.Identity.Name);
            var authorDTO = await _authorRepository.GetAuthorDTOByUsername(authorName);
            await _authorRepository.UnfollowAuthor(userDTO, authorDTO);
        }
        return RedirectToPage("Public");
    }


}
