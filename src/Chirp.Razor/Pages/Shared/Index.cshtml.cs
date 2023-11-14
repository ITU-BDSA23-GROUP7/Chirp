using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;

namespace MyApp.Namespace
{
    public class IndexModel : PageModel
    {
        private IMemoryCache _cache;
        private IAuthorRepository _repository;
        public IndexModel(IMemoryCache cache, IAuthorRepository repository)
        {
            _cache = cache;
            _repository = repository;
        }
        public async void OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                //Check if there is a cache with the key 'user'
                var user = _cache.Get<string>("user");
                if (user == null)
                {
                    bool usernameExists = await _repository.UsernameExistsAsync(User.Identity.Name);
                    if (!usernameExists)
                    {
                        _repository.CreateNewAuthor(User.Identity.Name);
                    }
                    AuthorInfo authorInfo = await _repository.GetAuthorInfo(User.Identity.Name);
                    _cache.Set<string>("user", authorInfo.Username);
                }
            }
        }
    }
}
