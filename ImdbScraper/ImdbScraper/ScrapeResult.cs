namespace ImdbScraper;

public class ScrapeResult
{
    public bool Success { get; internal set; }
    public DateTime? ScrapeDate { get; internal set; }
    public string RegionalTitle { get; internal set; }
    public string OriginalTitle { get; internal set; }
    public short? Year { get; internal set; }
    public float? Rating { get; internal set; }

    public ScrapeResult(bool success, string regionalTitle, string originalTitle, short? year)
    {
        Success = success;
        ScrapeDate = null;
        RegionalTitle = regionalTitle;
        OriginalTitle = originalTitle;
        Year = year;
        Rating = null;
    }

    public static ScrapeResult Fail() =>
        new(false, "", "", null);
}