namespace ImdbScraper;

public class GetMovieResult
{
    public GetMovieResultStatus GetMovieResultStatus { get; internal set; }
    public uint ImdbId { get; }
    public string RegionalTitle { get; internal set; }
    public string OriginalTitle { get; internal set; }
    public short? Year { get; internal set; }
        
    internal GetMovieResult(uint imdbId)
    {
        ImdbId = imdbId;
    }

    public bool IsOk =>
        GetMovieResultStatus == GetMovieResultStatus.FromSource || GetMovieResultStatus == GetMovieResultStatus.FromCache;

    public override string ToString() =>
        $@"IMDb ID: {ImdbId}
OK: {IsOk}
Regional title: {RegionalTitle}
Original title: {OriginalTitle}
Year: {Year}
Repository result status: {GetMovieResultStatus}";
}