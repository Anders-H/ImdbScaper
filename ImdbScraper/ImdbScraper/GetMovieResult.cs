namespace ImdbScraper;

public class GetMovieResult
{
    public uint ImdbId { get; }
    public GetMovieResultStatus GetMovieResultStatus { get; internal set; }
    public string RegionalTitle { get; }
    public string OriginalTitle { get; }
    public short? Year { get; }
    public DateTime? ScrapeDate { get; internal set; }
    public float? Rating { get; internal set; }
    public string? Url { get; internal set; }

    internal GetMovieResult(uint imdbId) : this(imdbId,  GetMovieResultStatus.FromSource, "", "", null, null, null, null)
    {
    }

    internal GetMovieResult(uint imdbId, GetMovieResultStatus status) : this(imdbId, status, "", "", null, null, null, null)
    {
    }

    internal GetMovieResult(uint imdbId, GetMovieResultStatus status, string regionalTitle, string originalTitle, short? year, float? rating, string? url, DateTime? scrapeDate)
    {
        ImdbId = imdbId;
        GetMovieResultStatus = status;
        RegionalTitle = regionalTitle;
        OriginalTitle = originalTitle;
        Year = year;
        ScrapeDate = scrapeDate;
        Rating = rating;
        Url = url;
    }

    public bool IsOk =>
        GetMovieResultStatus == GetMovieResultStatus.FromSource || GetMovieResultStatus == GetMovieResultStatus.FromCache;

    internal static GetMovieResult InfrastructureError(uint imdbId) =>
        new(imdbId, GetMovieResultStatus.InfrastructureError);

    internal static GetMovieResult GrabError(uint imdbId) =>
        new(imdbId, GetMovieResultStatus.GrabError);

    internal static GetMovieResult ScrapeError(uint imdbId) =>
        new(imdbId, GetMovieResultStatus.ScrapeError);

    internal static GetMovieResult FromSource(uint imdbId, string originalTitle, string regionalTitle,  short? year, float? rating, string? url, DateTime? scrapeDate) =>
        new(imdbId, GetMovieResultStatus.FromSource, originalTitle, regionalTitle, year, rating, url, scrapeDate);

    public override string ToString() =>
        $@"IMDb ID: {ImdbId}
OK: {IsOk}
Scrape date: {(ScrapeDate.HasValue ? ScrapeDate.Value.ToShortDateString() : "")}
Regional title: {RegionalTitle}
Original title: {OriginalTitle}
Year: {Year}
Rating: {Rating:n1}
Url: {Url}
Repository result status: {GetMovieResultStatus}";
}