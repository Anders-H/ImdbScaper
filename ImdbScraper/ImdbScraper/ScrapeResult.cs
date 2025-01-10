using System.Web;

namespace ImdbScraper;

public class ScrapeResult
{
    public bool Success { get; internal set; }
    public DateTime? ScrapeDate { get; internal set; }
    public string RegionalTitle { get; internal set; }
    public string OriginalTitle { get; internal set; }
    public short? Year { get; internal set; }
    public float? Rating { get; internal set; }
    public string? Url { get; internal set; }

    public ScrapeResult(bool success, string regionalTitle, string originalTitle, short? year)
    {
        Success = success;
        ScrapeDate = null;
        RegionalTitle = regionalTitle;
        OriginalTitle = originalTitle;
        Year = year;
        Rating = null;
        Url = null;

        if (string.IsNullOrEmpty(OriginalTitle))
            OriginalTitle = RegionalTitle;

        if (string.IsNullOrWhiteSpace(RegionalTitle))
            RegionalTitle = OriginalTitle;

        RegionalTitle = HttpUtility.HtmlDecode(RegionalTitle);
        OriginalTitle = HttpUtility.HtmlDecode(OriginalTitle);
    }

    public static ScrapeResult Fail() =>
        new(false, "", "", null);

    public override string ToString() =>
        $@"Scrape date:
{(ScrapeDate.HasValue ? ScrapeDate!.Value.ToShortDateString() : "")}

Regional title:
{RegionalTitle}

Original title:
{OriginalTitle}

Year:
{(Year.HasValue ? Year.Value : "")}

Rating:
{(Rating.HasValue ? Rating.Value.ToString("n0") : "")}";
}