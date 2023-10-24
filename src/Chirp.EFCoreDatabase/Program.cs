// See https://aka.ms/new-console-template for more information
/*ChirpDBContext chirpContext = new ChirpDBContext();
DbInitializer.SeedDatabase(chirpContext);

var cheeps = await chirpContext.Cheeps
                                .Where(c => c.Author.Name == "Helge")
                                .OrderBy(c => c.TimeStamp)
                                .Include(c => c.Author)
                                .ToListAsync();

foreach (Cheep c in cheeps)
{
    Console.WriteLine($"{c.TimeStamp} - {c.Author.Name}: {c.Text}");
}*/
