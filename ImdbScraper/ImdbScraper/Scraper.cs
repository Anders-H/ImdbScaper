using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;

namespace ImdbScraper;

public class Scraper
{
    private const string JsonStartTag = "<script type=\"application/ld+json\">";

    public DownloadResult Download(uint imdbId)
    {
        return new DownloadResult {GrabSuccess = true, Html = @"{
  ""@context"": ""https://schema.org"",
  ""@type"": ""Movie"",
  ""url"": ""/title/tt0087332/"",
  ""name"": ""Ghostbusters"",
  ""alternateName"": ""Ghostbusters - Spökligan"",
  ""image"": ""https://m.media-amazon.com/images/M/MV5BMTkxMjYyNzgwMl5BMl5BanBnXkFtZTgwMTE3MjYyMTE@._V1_.jpg"",
  ""description"": ""Three parapsychologists forced out of their university funding set up shop as a unique ghost removal service in New York City, attracting frightened yet skeptical customers."",
  ""review"": {
    ""@type"": ""Review"",
    ""itemReviewed"": {
      ""@type"": ""CreativeWork"",
      ""url"": ""/title/tt0087332/""
    },
    ""author"": {
      ""@type"": ""Person"",
      ""name"": ""degeneraatti""
    },
    ""dateCreated"": ""2014-09-28"",
    ""inLanguage"": ""English"",
    ""name"": ""A film that doesn&apos;t miss a step"",
    ""reviewBody"": ""Laughing at someone is an easy way to make comedy. It&apos;s the way school bullies operate. Laughing at an unwilling comedy vehicle also gives to ones doing the laughing a sense of superiority, tickling the satisfaction centers in our brain.\n\nThis is not the way Ghost Busters work. They had every possibility to ridicule various groups of humans, but luckily decided not to. The humor here is benign, well written, and constant. The humanity of all characters present is both endearing and an endless supply of laughter.\n\nBut what makes Ghost Busters a classic is the fact it doesn&apos;t stop here. The story is exciting, with a real sense of adventure. Still, not missing a beat, it never goes overboard, helping audience to laugh whenever laughs are provided.\n\nThis movie is comedy gold, and a must for everyone with even a mild love for movies and entertainment. Best watched with a pack of marshmallows."",
    ""reviewRating"": {
      ""@type"": ""Rating"",
      ""worstRating"": 1,
      ""bestRating"": 10,
      ""ratingValue"": 8
    }
  },
  ""aggregateRating"": {
    ""@type"": ""AggregateRating"",
    ""ratingCount"": 426363,
    ""bestRating"": 10,
    ""worstRating"": 1,
    ""ratingValue"": 7.8
  },
  ""contentRating"": ""11"",
  ""genre"": [
    ""Action"",
    ""Comedy"",
    ""Fantasy""
  ],
  ""datePublished"": ""1984-12-07"",
  ""keywords"": ""ghost,ghostbuster,scientist,paranormal investigation team,supernatural being"",
  ""trailer"": {
    ""@type"": ""VideoObject"",
    ""name"": ""30th Anniversary Trailer"",
    ""embedUrl"": ""https://www.imdb.com/video/imdb/vi2800593945"",
    ""thumbnail"": {
      ""@type"": ""ImageObject"",
      ""contentUrl"": ""https://m.media-amazon.com/images/M/MV5BMTgxNTM3MDUxNF5BMl5BanBnXkFtZTgwNDgwMDgxMjE@._V1_.jpg""
    },
    ""thumbnailUrl"": ""https://m.media-amazon.com/images/M/MV5BMTgxNTM3MDUxNF5BMl5BanBnXkFtZTgwNDgwMDgxMjE@._V1_.jpg"",
    ""url"": ""https://www.imdb.com/video/vi2800593945/"",
    ""description"": ""Watch the 30th Anniversary Trailer for Ghostbusters."",
    ""duration"": ""PT1M35S"",
    ""uploadDate"": ""2014-07-12T11:06:13Z""
  },
  ""actor"": [
    {
      ""@type"": ""Person"",
      ""url"": ""/name/nm0000195/"",
      ""name"": ""Bill Murray""
    },
    {
      ""@type"": ""Person"",
      ""url"": ""/name/nm0000101/"",
      ""name"": ""Dan Aykroyd""
    },
    {
      ""@type"": ""Person"",
      ""url"": ""/name/nm0000244/"",
      ""name"": ""Sigourney Weaver""
    }
  ],
  ""director"": [
    {
      ""@type"": ""Person"",
      ""url"": ""/name/nm0718645/"",
      ""name"": ""Ivan Reitman""
    }
  ],
  ""creator"": [
    {
      ""@type"": ""Organization"",
      ""url"": ""/company/co0050868/""
    },
    {
      ""@type"": ""Organization"",
      ""url"": ""/company/co0147858/""
    },
    {
      ""@type"": ""Organization"",
      ""url"": ""/company/co0012330/""
    },
    {
      ""@type"": ""Person"",
      ""url"": ""/name/nm0000101/"",
      ""name"": ""Dan Aykroyd""
    },
    {
      ""@type"": ""Person"",
      ""url"": ""/name/nm0000601/"",
      ""name"": ""Harold Ramis""
    },
    {
      ""@type"": ""Person"",
      ""url"": ""/name/nm0001548/"",
      ""name"": ""Rick Moranis""
    }
  ],
  ""duration"": ""PT1H45M""
}

", InfrastructureSuccess = true};

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
        //var startIndex = html.IndexOf(JsonStartTag, StringComparison.Ordinal);

        //html = html[(startIndex + JsonStartTag.Length)..];

        //var endIndex = html.IndexOf("</script>", StringComparison.Ordinal);

        //html = html.Substring(0, endIndex);

        var data = JObject.Parse(html);

        if (!data.TryGetValue("name", StringComparison.InvariantCultureIgnoreCase, out var name))
            return ScrapeResult.Fail();

        string? originalTitle = name.Value<string>() ?? "";
        var regionalTitle = "";

        if (data.TryGetValue("alternateName", StringComparison.InvariantCultureIgnoreCase, out var alternateName))
            regionalTitle = alternateName.Value<string>() ?? "";

        short? year = 0;
        if (data.TryGetValue("datePublished", StringComparison.CurrentCultureIgnoreCase, out var datePublished))
            year = (short)datePublished.Value<DateTime>().Year;

        return new ScrapeResult(true, regionalTitle, originalTitle, 0)
        {
            //Rating = rating
        };
    }
}