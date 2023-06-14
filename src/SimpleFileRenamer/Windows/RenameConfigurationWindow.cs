using Serilog;
using SimpleFileRenamer.Abstractions.Services;

namespace SimpleFileRenamer;
public partial class RenameConfigurationWindow : Form
{
    private readonly IConfigurationService _configuration;

    public RenameConfigurationWindow(IConfigurationService configuration)
    {
        Log.Verbose("Initializing Rename Configuration Window");
        _configuration = configuration;

        InitializeComponent();

        SeperatorTextBox.Text = _configuration.Value.Renamer.Delimiter.ToString();
        FormatTextBox.Text = _configuration.Value.Renamer.Format;
        SaveEditButton.Enabled = false;
    }

    private void SeperatorTextBox_TextChanged(object sender, EventArgs e)
    {
        SaveEditButton.Enabled = true;

        if (sender is not TextBox textBox)
        {
            return;
        }

        UseSpaceButton.Enabled = true;
        UseSpaceButton.Text = "Use Space";

        if (textBox.Text.Length == 0)
        {
            UseSpaceButton.Enabled = false;
            UseSpaceButton.Text = "Using Space ✓";
            _configuration.Value.Renamer.Delimiter = ' ';
            return;
        }

        if (textBox.Text.Length == 1)
        {
            UseSpaceButton.Enabled = true;
            UseSpaceButton.Text = "Use Space";

            if (!char.TryParse(textBox.Text, out var resultChar))
            {
                MessageBox.Show(
                    $"{textBox.Text} is not a supported character.",
                    "Invalid seperator",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (char.IsWhiteSpace(resultChar))
            {
                UseSpaceButton.Enabled = false;
                UseSpaceButton.Text = "Using Space ✓";
            }

            _configuration.Value.Renamer.Delimiter = resultChar;
            return;
        }

        if (textBox.Text.Length > 1)
        {
            UseSpaceButton.Enabled = true;
            UseSpaceButton.Text = "Using Space ✓";

            var characterLengthDialog = MessageBox.Show(
                    $"A seperator can only be 1 character.",
                    "Invalid seperator",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

            if (characterLengthDialog == DialogResult.OK)
            {
                textBox.Text = textBox.Text.First().ToString();
            }

            return;
        }
    }

    private void UseSpaceButton_Click(object sender, EventArgs e)
    {
        Log.Debug("Using space as the delimiter selected");
        SaveEditButton.Enabled = true;

        UseSpaceButton.Enabled = false;
        UseSpaceButton.Text = "Use Space ✓";

        _configuration.Value.Renamer.Delimiter = ' ';
        SeperatorTextBox.Text = " ";
    }

    private void FormatTextBox_TextChanged(object sender, EventArgs e)
    {
        SaveEditButton.Enabled = true;

        if (sender is TextBox textBox)
        {
            Log.Verbose("Format text changed {Text}", FormatTextBox.Text);
            _configuration.Value.Renamer.Format = textBox.Text;
        }
    }

    private void SaveEditButton_Click(object sender, EventArgs e)
    {
        Log.Debug("Saving rename configuration {@Config}", _configuration.Value.Renamer);
        _configuration.Save();

        DialogResult = DialogResult.OK;
    }
}
