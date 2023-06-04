using System.Collections;

namespace ImdbScraper;

public class Repository
{
    private Hashtable Cache { get; } = new();

    public GetMovieResult GetMovie(uint imdbId)
    {
        if (IsCached(imdbId))
        {
            var cached = (GetMovieResult)Cache[imdbId];
            
            if (cached.IsOk)
            {
                cached.GetMovieResultStatus = GetMovieResultStatus.FromCache;
                return cached;
            }
        }

        var s = new Scraper();
        var downloadResult = s.Download(imdbId);

        if (!downloadResult.InfrastructureSuccess)
            return new GetMovieResult(imdbId) { GetMovieResultStatus = GetMovieResultStatus.InfrastructureError };
        
        if (!downloadResult.GrabSuccess)
            return new GetMovieResult(imdbId) { GetMovieResultStatus = GetMovieResultStatus.GrabError };
        
        var scrapeResult = s.Scrape(downloadResult.Html);
        
        if (!scrapeResult.Success)
            return new GetMovieResult(imdbId) { GetMovieResultStatus = GetMovieResultStatus.ScrapeError };
        
        var result = new GetMovieResult(imdbId)
        {
            GetMovieResultStatus = GetMovieResultStatus.FromSource,
            OriginalTitle = scrapeResult.OriginalTitle,
            RegionalTitle = scrapeResult.RegionalTitle,
            Year = scrapeResult.Year
        };
        
        Cache.Add(imdbId, result);

        return result;
    }

    public bool IsCached(uint imdbId) =>
        Cache.ContainsKey(imdbId);
}