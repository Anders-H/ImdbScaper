using System.Collections;

namespace ImdbScraper;

public class Repository
{
    private Hashtable Cache { get; } = new();

    public GetMovieResult GetMovie(uint imdbId)
    {
        if (IsCached(imdbId))
        {
            var cached = (GetMovieResult)Cache[imdbId]!;
            
            if (cached.IsOk)
            {
                cached.GetMovieResultStatus = GetMovieResultStatus.FromCache;
                return cached;
            }
        }

        var s = new Scraper();
        var downloadResult = s.Download(imdbId);

        if (!downloadResult.InfrastructureSuccess)
            return GetMovieResult.InfrastructureError(imdbId);
        
        if (!downloadResult.GrabSuccess)
            return GetMovieResult.GrabError(imdbId);
        
        var scrapeResult = s.Scrape(downloadResult.Html);
        
        if (!scrapeResult.Success)
            return GetMovieResult.ScrapeError(imdbId);

        var result = GetMovieResult.FromSource(imdbId, scrapeResult.OriginalTitle, scrapeResult.RegionalTitle, scrapeResult.Year);
        
        Cache.Add(imdbId, result);

        return result;
    }

    public bool IsCached(uint imdbId) =>
        Cache.ContainsKey(imdbId);
}