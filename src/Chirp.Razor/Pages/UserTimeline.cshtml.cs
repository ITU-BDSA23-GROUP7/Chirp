﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Chirp.Razor.Pages;

public class UserTimelineModel : PageModel
{
    private readonly ICheepRepository _cheepRepository;
    private readonly IAuthorRepository _authorRepository;
    public required IEnumerable<CheepDTO> Cheeps { get; set; }
    public required List<string> Following { get; set; }
    public int PageCount { get; private set; }
    public AddCheepModel AddCheepModel{ get; set; }
    private string _author {  get; set; }

    public UserTimelineModel(ICheepRepository cheepRepository, IAuthorRepository authorRepository)
    {
        _cheepRepository = cheepRepository;
        _authorRepository = authorRepository;
        AddCheepModel = new AddCheepModel(cheepRepository);
    }

    /// <summary>
    ///     Sets PageCount to the amount of total pages from the given author. <br/>
    ///     Sets Cheeps depending on the "page" query parameter. <br/>
    ///     * Will set Cheeps to the first page if no parameter is specified. <br/>
    ///     * Will set Cheeps to an empty list if parameter is not a positive integer. <br/>
    /// </summary>
    /// <param name="author">The name of the author.</param>
    /// <returns></returns>
    public async Task<IActionResult> OnGet(string author)
    {
        _author = author;
        PageCount = _cheepRepository.GetPageCount(author);

        string pageNumStr = Request.Query["page"]!;

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

        if (pageNumStr == null)
        {
            Cheeps = await _cheepRepository.GetCheeps(1, author);
            return Page();
        }

        int pageNum;

        if (!int.TryParse(pageNumStr, out pageNum))
        {
            Cheeps = await _cheepRepository.GetCheeps(1, author);
            return Page();
        }

        if (pageNum < 0)
        {
            Cheeps = await _cheepRepository.GetCheeps(1, author);
            return Page();
        }

        Cheeps = await _cheepRepository.GetCheeps(pageNum, author);
        return Page();
    }

    [BindProperty]
    public string method { get; set; }
    public async Task<IActionResult> OnPostAsync()
    {
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
        return await OnGet((string) HttpContext.GetRouteValue("author"));
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
