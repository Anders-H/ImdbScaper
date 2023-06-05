namespace ImdbScraper;

public class GetMovieResult
{
    public uint ImdbId { get; }
    public GetMovieResultStatus GetMovieResultStatus { get; internal set; }
    public string RegionalTitle { get; }
    public string OriginalTitle { get; }
    public short? Year { get; }
        
    internal GetMovieResult(uint imdbId) : this(imdbId,  GetMovieResultStatus.FromSource, "", "", null)
    {
    }

    internal GetMovieResult(uint imdbId, GetMovieResultStatus status) : this(imdbId, status, "", "", null)
    {
    }

    internal GetMovieResult(uint imdbId, GetMovieResultStatus status, string regionalTitle, string originalTitle, short? year)
    {
        ImdbId = imdbId;
        RegionalTitle = regionalTitle;
        OriginalTitle = originalTitle;
        Year = year;
    }

    public bool IsOk =>
        GetMovieResultStatus == GetMovieResultStatus.FromSource || GetMovieResultStatus == GetMovieResultStatus.FromCache;

    internal static GetMovieResult InfrastructureError(uint imdbId) =>
        new(imdbId, GetMovieResultStatus.InfrastructureError);

    internal static GetMovieResult GrabError(uint imdbId) =>
        new(imdbId, GetMovieResultStatus.GrabError);

    internal static GetMovieResult ScrapeError(uint imdbId) =>
        new(imdbId, GetMovieResultStatus.ScrapeError);

    internal static GetMovieResult FromSource(uint imdbId, string originalTitle, string regionalTitle,  short? year) =>
        new(imdbId, GetMovieResultStatus.FromSource, originalTitle, regionalTitle, year);

    public override string ToString() =>
        $@"IMDb ID: {ImdbId}
OK: {IsOk}
Regional title: {RegionalTitle}
Original title: {OriginalTitle}
Year: {Year}
Repository result status: {GetMovieResultStatus}";
}