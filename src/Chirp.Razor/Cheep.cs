

public class Cheep
{
    public string? Message{get; set;}
    public DateTime? TimeStamp{get; set;}
    public Author? Author{get; set;}

}

public class Author
{
    public string? Name{get; set;}
    public string? Email{get; set;}
    public IEnumerable<Cheep> Cheeps{get; set;}

    public Author(){
        Cheeps = new List<Cheep>();
    } 
}