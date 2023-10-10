public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public Task<IEnumerable<CheepDTO>> GetCheeps(int pageNum = 1);
    public List<CheepViewModel> GetCheepsFromAuthor(string author, int pageNum = 1);
    public int GetPageCount();
    public int GetPageCountFromAuthor(string author);
}

public class CheepService : ICheepService
{
    private int pageLength = 32;

    private static readonly IChirpRepository repository = new ChirpRepository(new ChirpDBContext());

    public async Task<IEnumerable<CheepDTO>> GetCheeps(int pageNum = 1)
    {
        return await repository.GetCheeps(pageNum);
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author, int pageNum = 1)
    {
        throw new NotImplementedException();
    }

    public int GetPageCount() {
       return repository.GetPageCount();
    }
    public int GetPageCountFromAuthor(string author) {
        return 42;
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

    /*

    /// <summary>
    /// Returns a list of CheepViewModel, with a length of <c>pageLength</c>.
    /// </summary>
    /// <param name="cheeps"></param>
    /// <param name="pageNum"></param>
    /// <returns>The </returns>
    private List<CheepDTO> GetPageFromCheepList(List<CheepDTO> cheeps, int pageNum) {

        int pageIndex = pageNum - 1;

        if (pageIndex < 0)
            return GetEmptyCheepList();

        int fromIndex = pageIndex * pageLength;

        if (cheeps.Count < fromIndex)
            return GetEmptyCheepList();

        int maxPageLength = cheeps.Count - fromIndex;

        int currentPageLength = int.Min(pageLength, maxPageLength);

        return cheeps.GetRange(fromIndex, currentPageLength);
    }

    private List<CheepDTO> GetEmptyCheepList() {
        return new List<CheepDTO>();
    }

    private int GetPagesCountFromCheepCount(int cheepCount) {
        int fullPagesCount = cheepCount / pageLength;
        int remainderCheeps = cheepCount % pageLength;
        if (remainderCheeps == 0) {
            return fullPagesCount;
        }
        return fullPagesCount + 1;
    }
    //*/
}
