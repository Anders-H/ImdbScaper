using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace ImdbScraper;

public class Scraper
{
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
                && result.Html.IndexOf(" - IMDb</title>", StringComparison.Ordinal) > 0;
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
        var regionalTitleStart = html.IndexOf("<h1 ", StringComparison.Ordinal);
        var regionalTitleEnd = html.IndexOf("</h1>", StringComparison.Ordinal);
        var regionalTitleLength = regionalTitleEnd - regionalTitleStart + 5;

        if (regionalTitleLength < 0 || regionalTitleLength < 1)
            return ScrapeResult.Fail();

        var regionalTitleHtml = html.Substring(regionalTitleStart, regionalTitleLength);

        var regionalTitleDom = new XmlDocument();
        regionalTitleDom.LoadXml($@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes"" ?>
{regionalTitleHtml}");

        var regionalTitle = regionalTitleDom.DocumentElement?.ChildNodes[0]?.InnerText ?? "";

        string originalTitle;
        var originalTitleStart = html.IndexOf("Original title:", StringComparison.Ordinal);
        
        if (originalTitleStart < 0)
        {
            originalTitle = regionalTitle;
        }
        else
        {
            originalTitle = html.Substring(originalTitleStart + 16);
            var closingTag = originalTitle.IndexOf('<');

            if (closingTag >= 0)
                originalTitle = originalTitle.Substring(0, closingTag);
        }

        return new ScrapeResult(true, regionalTitle, originalTitle, 0);
    }
}