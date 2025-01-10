using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using ImdbScraper;

namespace ImdbApp;

public partial class MainWindow : Form
{
    private uint _lastSearchedMovie;
    private readonly Repository _repository;

    public MainWindow()
    {
        InitializeComponent();
        _lastSearchedMovie = 0;
        _repository = new Repository();
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

        match = Regex.Match(t, @"([0-9]+)");

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

        match = Regex.Match(t, @"(rm[0-9]*)");

        if (match.Success)
        {

        }
    }

    private void ScrapeMovie()
    {
        if (!uint.TryParse(cboImdbId.Text, NumberStyles.Any, CultureInfo.CurrentCulture, out var id))
            return;

        if (_lastSearchedMovie == id)
            return;

        ClearForm();

        _lastSearchedMovie = id;
        Cursor = Cursors.WaitCursor;
        Refresh();
        var movieStatus = _repository.GetMovie(id);
        Cursor = Cursors.Default;

        if (movieStatus.IsOk)
        {
            lblSuccess.Text = @"Yes";
            AddIfNotExits(id);

            lblScrapeDate.Text = movieStatus.ScrapeDate.HasValue
                ? movieStatus.ScrapeDate.Value.ToString("yyyy-MM-dd")
                : "";
            
            lblUrl.Text = movieStatus.Url;
            textBox1.Text = movieStatus.ToString();
        }
        else
        {
            switch (movieStatus.GetMovieResultStatus)
            {
                case GetMovieResultStatus.InfrastructureError:
                    lblSuccess.Text = @"No (infrastructure error)";
                    break;
                case GetMovieResultStatus.GrabError:
                    lblSuccess.Text = @"No (grab error)";
                    break;
                case GetMovieResultStatus.ScrapeError:
                    lblSuccess.Text = @"No (scrape error)";
                    break;
                default:
                    lblSuccess.Text = @"No (unknown error)";
                    break;
            }
        }
    }

    private void ClearForm()
    {
        lblSuccess.Text = "";
        lblScrapeDate.Text = "";
        lblUrl.Text = "";
        textBox1.Text = "";
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

    private void lblUrl_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(lblUrl.Text))
            return;

        try
        {
            var url = lblUrl.Text.Replace("&", "^&");
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
        catch
        {
            // ignored
        }
    }
}