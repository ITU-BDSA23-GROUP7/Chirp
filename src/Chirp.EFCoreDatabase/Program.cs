// See https://aka.ms/new-console-template for more information
ChirpDBContext chirpContext = new ChirpDBContext();
DbInitializer.SeedDatabase(chirpContext);

var cheeps = chirpContext.Cheeps
                                .OrderBy(c => c.CheepId).ToList();
foreach (Cheep c in cheeps)
{
    Console.WriteLine($"{c.TimeStamp} - {c.Author.Name}: {c.Text}");
}
