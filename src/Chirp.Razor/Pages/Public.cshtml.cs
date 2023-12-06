using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class PublicModel : PageModel
{
    private readonly ICheepRepository _repository;
    private readonly IAuthorRepository _authorRepository;
    public required List<CheepDTO> Cheeps { get; set; }
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
        AddCheepModel = new AddCheepModel(repository);
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

        string PageNumStr = Request.Query["page"]!;

        if (User.Identity.IsAuthenticated)
        {
            var username = User.Identity.Name;
            if (!await _authorRepository.UsernameExistsAsync(username))
            {
                await _authorRepository.CreateNewAuthor(username);
            }
            var authorDTO = await _authorRepository.GetAuthorDTOByUsername(username);

            var following = await _authorRepository.GetFollowingUsernames(authorDTO);
            Following = following.ToList();
        }

        if (PageNumStr == null)
        {
            Cheeps = await _repository.GetCheeps();
            return Page();
        }

        int pageNum;

        if (!int.TryParse(PageNumStr, out pageNum))
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
    public string method { get; set; }
    public async Task<IActionResult> OnPostAsync(){
        switch (method)
        {
            case "follow":
                await OnPostFollow();
                break;
            case "unfollow":
                await OnPostUnfollow();
                break;
            case "addCheep":
                await OnPostAddCheep();
                break;
        }
        return RedirectToPage("Public");
    }

    [BindProperty]
    public string authorName { get; set; }
    public async Task OnPostFollow()
    {
        if (User.Identity.IsAuthenticated)
        {
            var userDTO = await _authorRepository.GetAuthorDTOByUsername(User.Identity.Name);
            var authorDTO = await _authorRepository.GetAuthorDTOByUsername(authorName);
            await _authorRepository.FollowAuthor(userDTO, authorDTO);
        }
        
    }

    public async Task OnPostUnfollow()
    {
        if (User.Identity.IsAuthenticated)
        {
            var userDTO = await _authorRepository.GetAuthorDTOByUsername(User.Identity.Name);
            var authorDTO = await _authorRepository.GetAuthorDTOByUsername(authorName);
            await _authorRepository.UnfollowAuthor(userDTO, authorDTO);
        }
    }

    [BindProperty]
    public string CheepText { get; set; }
    public async Task OnPostAddCheep()
    {
        await AddCheepModel.OnPostAsync(User.Identity.Name, CheepText);
    }


}
