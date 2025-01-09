using System.Globalization;
using System.Text.RegularExpressions;
using ImdbScraper;

namespace ImdbApp;

public partial class MainWindow : Form
{
    private uint _lastSearchedMovie;
    private Scraper _scraper;

    public MainWindow()
    {
        InitializeComponent();
        _lastSearchedMovie = 0;
        _scraper = new Scraper();
    }

    private void cboImdbId_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyData == Keys.Enter)
            Scrape();
    }

    private void cboImdbId_Leave(object sender, EventArgs e)
    {
        Scrape();
    }

    private void Scrape()
    {
        var t = cboImdbId.Text.Trim();
        var match = Regex.Match(t, @"https:\/\/www.imdb.com\/title\/tt([0-9]+).*");

        if (match.Success)
        {
            var valueMatch = match.Groups[1].Value;

            if (uint.TryParse(valueMatch, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
            {
                cboImdbId.Text = value.ToString();
                ScrapeMovie();
            }

            return;
        }

        match = Regex.Match(t, @"[0-9]+");

        if (match.Success)
        {
            if (uint.TryParse(t, NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
            {
                cboImdbId.Text = value.ToString();
                ScrapeMovie();
            }

            return;
        }
    }

    private void ScrapeMovie()
    {
        if (!uint.TryParse(cboImdbId.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out var id))
            return;

        if (_lastSearchedMovie == id)
            return;

        _lastSearchedMovie = id;
        var movieStatus = _scraper.Download(id);

        if (movieStatus.GrabSuccess)
        {
            lblSuccess.Text = @"Yes";
            var scraped = _scraper.Scrape(movieStatus.Html);

            if (scraped.Success)
            {
                AddIfNotExits(id);
                lblScrapeDate.Text = scraped.ScrapeDate!.Value.ToString("yyyy-MM-dd");
            }
            else
            {
                lblSuccess.Text = @"No (successful download, failed scrape)";
            }
        }
        else
        {
            lblSuccess.Text = movieStatus.InfrastructureSuccess ? @"No (infrastructure)" : @"No (parsing)";
        }
    }

    private void AddIfNotExits(uint movieId)
    {
        var id = movieId.ToString();

        foreach (var item in cboImdbId.Items)
        {
            var i = item as string;

            if (i == id)
                return;
        }

        cboImdbId.Items.Add(id);
    }

    private void btnGet_Click(object sender, EventArgs e)
    {
        Scrape();
    }

    private void cboImdbId_Validating(object sender, System.ComponentModel.CancelEventArgs e)
    {
        Scrape();
    }
}