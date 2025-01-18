using System.Diagnostics;
using System.Drawing.Imaging;
using System.Globalization;
using System.Text.RegularExpressions;
using ImdbScraper;

namespace ImdbApp;

public partial class MainWindow : Form
{
    private uint _lastSearchedMovie;
    private readonly Repository _repository;
    private readonly List<string> _notDeletedFiles;

    public MainWindow()
    {
        InitializeComponent();
        _lastSearchedMovie = 0;
        _repository = new Repository();
        _notDeletedFiles = new List<string>();
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

        var match = Regex.Match(t, @"(tt[0-9]+).*(rm[0-9]+)");

        if (match.Success)
        {
            ScrapeImage(match.Groups[1].Value, match.Groups[2].Value);
            return;
        }

        match = Regex.Match(t, @"https:\/\/www.imdb.com\/title\/tt([0-9]+).*");

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

        ClearForm();
        lblSuccess.Text = @"I don't understand.";
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

    private void ScrapeImage(string title, string image)
    {
        ClearForm();
        var url = $"https://www.imdb.com/title/{title}/mediaviewer/{image}";
        Cursor = Cursors.WaitCursor;
        Refresh();
        var imdbImg = new ImdbImage(url);
        var img = imdbImg.GetImage(image);
        Cursor = Cursors.Default;
        Refresh();

        if (img == null)
        {
            lblSuccess.Text = @"Image scrape failed.";
            return;
        }

        pictureBox1.Image = img;
        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        pictureBox1.Visible = true;
        Refresh();
        using var x = new SaveFileDialog();
        x.Title = @"Save scraped jpg image";
        x.Filter = @"*.jpg|*.jpg";
        x.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        x.FileName = imdbImg.SuggestFilename();

        if (x.ShowDialog(this) != DialogResult.OK)
            return;

        var parameters = new EncoderParameters(1);
        parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100L);
        var encoder = ImageCodecInfo.GetImageDecoders().FirstOrDefault(x => x.FormatID == ImageFormat.Jpeg.Guid);

        if (encoder == null)
        {
            pictureBox1.Image = null;
            pictureBox1.Visible = false;
            Refresh();
            MessageBox.Show(this, @"Failed to get encoder.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        try
        {
            img.Save(x.FileName, encoder, parameters);
        }
        catch
        {
            MessageBox.Show(this, @"Save failed.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        pictureBox1.Image = null;
        pictureBox1.Visible = false;
        Refresh();
        //TODO: Spara i listan över filer att radera om misslyckas.
        imdbImg.DeleteTempImage();
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