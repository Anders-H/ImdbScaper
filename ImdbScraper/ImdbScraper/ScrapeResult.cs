namespace ImdbScraper;

public class ScrapeResult
{
    public bool Success { get; internal set; }
    public string RegionalTitle { get; internal set; }
    public string OriginalTitle { get; internal set; }
    public short? Year { get; internal set; }

    public ScrapeResult(bool success, string regionalTitle, string originalTitle, short? year)
    {
        Success = success;
        RegionalTitle = regionalTitle;
        OriginalTitle = originalTitle;
        Year = year;
    }

    public static ScrapeResult Fail() =>
        new(false, "", "", null);
}