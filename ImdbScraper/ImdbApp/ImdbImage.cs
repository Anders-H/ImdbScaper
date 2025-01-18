using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices.Marshalling;
using System.Text.RegularExpressions;

namespace ImdbApp;

public class ImdbImage
{
    private readonly string _url;
    private string? _originalFileName;
    private string? _rmNumber;
    private string? _tempFileName;

    public ImdbImage(string url)
    {
        _url = url;
    }

    public Bitmap? GetImage() =>
        GetImage("");

    public Bitmap? GetImage(string rmNumber)
    {
        _rmNumber = rmNumber;

        try
        {
            using var wc = new WebClient();
            var content = wc.DownloadString(_url);
            var match = Regex.Match(content, @"\""(https:\/\/.*\/[a-zA-Z0-9@\._]*\.jpg)\""");

            if (!match.Success)
                return null;

            var rawUrl = match.Groups[1].Value.Trim();

            if (string.IsNullOrWhiteSpace(rawUrl) || rawUrl.Length < 5)
                return null;

            if (rawUrl.StartsWith('\"'))
                rawUrl = rawUrl.Substring(1);

            var closingQuote = rawUrl.IndexOf('\"');

            if (closingQuote > 2)
                rawUrl = rawUrl.Substring(0, closingQuote);

            var tempPath = Path.GetTempPath();

            if (string.IsNullOrWhiteSpace(tempPath))
                return null;

            var nameParts = rawUrl.Split('/');
            _originalFileName = nameParts[^1];
            _tempFileName = Path.Combine(tempPath, _originalFileName);
            wc.DownloadFile(new Uri(rawUrl), _tempFileName);
            return new Bitmap(_tempFileName);
        }
        catch
        {
            return null;
        }
    }

    public string SuggestFilename()
    {
        const string defaultFilename = "image.jpg";

        if (!string.IsNullOrWhiteSpace(_rmNumber))
            return $"{_rmNumber}.jpg";

        if (string.IsNullOrWhiteSpace(_originalFileName))
            return defaultFilename;

        return _originalFileName.Length > 2 ? _originalFileName : defaultFilename;
    }

    public bool DeleteTempImage()
    {
        if (string.IsNullOrWhiteSpace(_tempFileName))
            return false;

        try
        {
            File.Delete(_tempFileName);
            return !File.Exists(_tempFileName);
        }
        catch
        {
            return false;
        }
    }
}