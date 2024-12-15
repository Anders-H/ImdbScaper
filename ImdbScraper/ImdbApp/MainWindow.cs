using System.Globalization;
using ImdbScraper;

namespace ImdbApp;

public partial class MainWindow : Form
{
    private uint _lastSearched;
    private Scraper _scraper;

    public MainWindow()
    {
        InitializeComponent();
        _lastSearched = 0;
        _scraper = new Scraper();
    }

    private void cboImdbId_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyData == Keys.Enter)
            ScrapeMovie();
    }

    private void cboImdbId_Leave(object sender, EventArgs e)
    {
        ScrapeMovie();
    }

    private void ScrapeMovie()
    {
        if (!uint.TryParse(cboImdbId.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out var id))
            return;

        if (_lastSearched == id)
            return;

        _lastSearched = id;
        var movieStatus = _scraper.Download(id);
        
        if (movieStatus.GrabSuccess)
        {
            lblSuccess.Text = @"Yes";
            var scraped = _scraper.Scrape(movieStatus.Html);
            if (scraped.Success)
            {

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
}