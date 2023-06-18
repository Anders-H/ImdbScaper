namespace ImdbScraper;

public class DownloadResult
{
    public bool InfrastructureSuccess { get; internal set; }

    public bool GrabSuccess { get; internal set; }

    public string? Html { get; internal set; }
}