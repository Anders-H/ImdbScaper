namespace ImdbScraper;

public enum GetMovieResultStatus
{
    FromSource,
    FromCache,
    InfrastructureError,
    GrabError,
    ScrapeError
}