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

        await CheckUserIdentity();

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

    private async Task CheckUserIdentity() {
        if (User.Identity == null || !User.Identity.IsAuthenticated) {
            return;
        }

        var username = User.Identity.Name;

        if (username == null) {
            return;
        }

        if (!await _authorRepository.UsernameExistsAsync(username))
        {
            await _authorRepository.CreateNewAuthor(username);
        }
        var authorDTO = await _authorRepository.GetAuthorDTOByUsername(username);

        var following = await _authorRepository.GetFollowingUsernames(authorDTO);
        Following = following.ToList();
    }

    [BindProperty]
    public string? authorName { get; set; }
    public async Task OnPostFollow()
    {
        if (User.Identity == null || User.Identity.Name == null || authorName == null) {
            return;
        }

        if (User.Identity.IsAuthenticated)
        {
            var userDTO = await _authorRepository.GetAuthorDTOByUsername(User.Identity.Name);
            var authorDTO = await _authorRepository.GetAuthorDTOByUsername(authorName);
            await _authorRepository.FollowAuthor(userDTO, authorDTO);
        }

    }

    public async Task OnPostUnfollow()
    {
        if (User.Identity == null || User.Identity.Name == null || authorName == null) {
            return;
        }

        if (User.Identity.IsAuthenticated)
        {
            var userDTO = await _authorRepository.GetAuthorDTOByUsername(User.Identity.Name);
            var authorDTO = await _authorRepository.GetAuthorDTOByUsername(authorName);
            await _authorRepository.UnfollowAuthor(userDTO, authorDTO);
        }
    }

    [BindProperty]
    public string? CheepText { get; set; }
    public async Task OnPostAddCheep()
    {
        if (User.Identity == null || User.Identity.Name == null || CheepText == null)
        {
            return;
        }

        await AddCheepModel.OnPostAsync(User.Identity.Name, CheepText);
    }


}
