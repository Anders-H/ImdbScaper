using System.Net;
using System.Text;

namespace ImdbScraper;

public class Scraper
{
    private const string JsonStartTag = "<script type=\"application/ld+json\">";

    public DownloadResult Download(uint imdbId)
    {
        var url = $@"https://www.imdb.com/title/tt{imdbId:0000000}/";

        var result = new DownloadResult
        {
            InfrastructureSuccess = false,
            GrabSuccess = false,
            Html = ""
        };

        try
        {
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                result.Html = client.DownloadString(url);
            }

            result.InfrastructureSuccess = true;
            
            result.GrabSuccess = !string.IsNullOrEmpty(result.Html)
                && result.Html.Length > 10000
                && result.Html.IndexOf(JsonStartTag, StringComparison.Ordinal) > 0;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return result;
    }

    public ScrapeResult Scrape(string html)
    {
        var startIndex = html.IndexOf(JsonStartTag, StringComparison.Ordinal);

        html = html[(startIndex + JsonStartTag.Length)..];

        var endIndex = html.IndexOf("</script>");

        html = html.Substring(0, endIndex);

        var rating = GetFloat("ratingValue");


        return  new ScrapeResult(true, "", "", 0)
        {
            Rating = rating
        };
    }

    private static float? GetFloat(string name)
    {
        var s = GetValue(name);






    }
}