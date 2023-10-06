public record CheepViewModel(string Author, string Message, string Timestamp);

public interface ICheepService
{
    public List<CheepViewModel> GetCheeps(int pageNum = 1);
    public List<CheepViewModel> GetCheepsFromAuthor(string author, int pageNum = 1);
    public int GetPageCount();
    public int GetPageCountFromAuthor(string author);
}

public class CheepService : ICheepService
{
    private int pageLength = 32;



    // These would normally be loaded from a database for example
    private static readonly List<CheepViewModel> _cheeps = GetDefaultCheeps();

    // Returns some default cheeps for testing. It is not important for the final product.
    private static List<CheepViewModel> GetDefaultCheeps() {
        List<CheepViewModel> cheeps = new();
        List<string> userNames = new(){"Daniel", "Casper", "Max", "Line", "Sebastian"};
        for (int i = 0; i < 1000; i++) {
            var userName = userNames[i % userNames.Count];
            var holyCow = i % (userNames.Count - 1) == (userNames.Count - 2) ? "Holy cow! " : "";
            var cheep = new CheepViewModel(userName, $"{holyCow}This is cheep number {i+1}!", UnixTimeStampToDateTimeString(1690892208+i));
            cheeps.Add(cheep);
        }
        return cheeps;
    }

    public List<CheepViewModel> GetCheeps(int pageNum = 1)
    {
        return GetPageFromCheepList(_cheeps, pageNum);
    }

    public List<CheepViewModel> GetCheepsFromAuthor(string author, int pageNum = 1)
    {
        // filter by the provided author name
        var authorCheeps = _cheeps.Where(x => x.Author == author).ToList();
        return GetPageFromCheepList(authorCheeps, pageNum);
    }

    public int GetPageCount() {
        return GetPagesCountFromCheepCount(_cheeps.Count);
    }
    public int GetPageCountFromAuthor(string author) {
        var authorCheeps = _cheeps.Where(x => x.Author == author).ToList();
        return GetPagesCountFromCheepCount(authorCheeps.Count);
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

    /// <summary>
    /// Returns a list of CheepViewModel, with a length of <c>pageLength</c>.
    /// </summary>
    /// <param name="cheeps"></param>
    /// <param name="pageNum"></param>
    /// <returns>The </returns>
    private List<CheepViewModel> GetPageFromCheepList(List<CheepViewModel> cheeps, int pageNum) {

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

    private List<CheepViewModel> GetEmptyCheepList() {
        return new List<CheepViewModel>();
    }

    private int GetPagesCountFromCheepCount(int cheepCount) {
        int fullPagesCount = cheepCount / pageLength;
        int remainderCheeps = cheepCount % pageLength;
        if (remainderCheeps == 0) {
            return fullPagesCount;
        }
        return fullPagesCount + 1;
    }
}
