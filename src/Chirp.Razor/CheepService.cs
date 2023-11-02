public interface ICheepService
{
    public Task<IEnumerable<CheepDTO>> GetCheeps(int pageNum = 1);
    public Task<IEnumerable<CheepDTO>> GetCheepsFromAuthor(string author, int pageNum = 1);
    public int GetPageCount();
    public int GetPageCountFromAuthor(string author);
}

public class CheepService : ICheepService
{
    private static readonly IChirpRepository Repository = new ChirpRepository();

    public async Task<IEnumerable<CheepDTO>> GetCheeps(int pageNum = 1)
    {
        return await Repository.GetCheeps(pageNum);
    }

    public async Task<IEnumerable<CheepDTO>> GetCheepsFromAuthor(string author, int pageNum = 1)
    {
        return await Repository.GetCheeps(pageNum, author);
    }

    public int GetPageCount()
    {
        return Repository.GetPageCount();
    }
    public int GetPageCountFromAuthor(string author)
    {
        return Repository.GetPageCount(author);
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }
}
