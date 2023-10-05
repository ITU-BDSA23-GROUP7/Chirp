

public class Cheep
{
    public required string Message{get; set;}
    public required DateTime TimeStamp{get; set;}
    public required Author Author{get; set;}

}

public class Author
{
    public required string Name{get; set;}
    public required string Email{get; set;}
    public IEnumerable<Cheep> Cheeps{get; set;}

    public Author(){
        Cheeps = new List<Cheep>();
    } 
}