namespace ImdbScraper;

public class DownloadResult
{
    public bool InfrastructureSuccess { get; }
    public bool GrabSuccess { get; }
    public string Html { get; }

    public DownloadResult() : this(false, false, "")
    {
    }

    public DownloadResult(bool infrastructureSuccess, bool grabSuccess, string html)
    {
        InfrastructureSuccess = infrastructureSuccess;
        GrabSuccess = grabSuccess;
        Html = html;
    }
}