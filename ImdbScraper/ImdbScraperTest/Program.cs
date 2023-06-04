using ImdbScraper;

var repository = new Repository();
var result = repository.GetMovie(87332);
Console.WriteLine(result.ToString());

do
{
    Console.Write("IMDb ID: ");
    var imdIdString = Console.ReadLine();

    if (uint.TryParse(imdIdString, out var imdbId))
    {
        var repositoryResult = repository.GetMovie(imdbId);
        Console.WriteLine(repositoryResult);
    }
    else
    {
        return;
    }

} while (true);