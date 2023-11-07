
public class AuthorRepository : IAuthorRepository
{
    private ChirpDBContext context;

    public AuthorRepository(ChirpDBContext context)
    {
        this.context = context;
    }

    public async void CreateNewAuthor(int authorId, string name, string email){
        var alreadyExists = GetAuthorInfo(name);


        if(alreadyExists.Result.UserName == name){
            Console.WriteLine("User already exists, so user has not been created");
            //return;
        }

        context.Authors.Add(new Author{AuthorId = authorId, Name=name, Email=email, Cheeps = new List<Cheep>()});
        context.SaveChanges();
    }

    public async Task<AuthorInfo> GetAuthorInfo(string userName)
    {
        var author = await context.Authors.FirstOrDefaultAsync(c => c.Name == userName);

        var authorInfo = new AuthorInfo(
            UserName: author.Name,
            Email: author.Email
        );

        return authorInfo;
    }

    protected virtual void Dispose(bool disposing)
    {
        throw new NotImplementedException("The dispose method is not implemented for AuthorRepository");
    }
    public void Dispose()
    {
        Dispose(true);
    }
}