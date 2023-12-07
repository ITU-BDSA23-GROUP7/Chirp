using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;

namespace MyApp.Namespace
{
    public class IndexViewComponent : ViewComponent
    {
        private IMemoryCache _cache;
        private IAuthorRepository _repository;
        public IndexViewComponent(IMemoryCache cache, IAuthorRepository repository)
        {
            _cache = cache;
            _repository = repository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated || User.Identity.Name == null) {
                return View();
            }

            bool usernameExists = await _repository.UsernameExistsAsync(User.Identity.Name);
            if (!usernameExists)
            {
                await _repository.CreateNewAuthor(User.Identity.Name);
            }

            AuthorDTO authorDTO = await _repository.GetAuthorDTOByUsername(User.Identity.Name);

            return View();
        }
    }
}
