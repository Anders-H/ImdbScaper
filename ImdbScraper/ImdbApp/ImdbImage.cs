using System.Text.RegularExpressions;

namespace ImdbApp;

public class ImdbImage
{
    private readonly string _url;

    public ImdbImage(string url)
    {
        _url = url;
    }

    public Bitmap? GetImage()
    {
        try
        {
            using var wc = new System.Net.WebClient();
            var content = wc.DownloadString(_url);
            var match = Regex.Match(content, @"\""(https:\/\/.*\/[a-zA-Z0-9@\.\_]*\.jpg)\""");

            if (!match.Success)
                return null;

            xxx
        }
        catch
        {
            return null;
        }
    }
}