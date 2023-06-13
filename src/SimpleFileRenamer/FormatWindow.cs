using Serilog;

namespace SimpleFileRenamer;
public partial class FormatWindow : Form
{
    public char? Delimiter { get; set; } = default!;

    public string? Format { get; set; } = default!;

    public FormatWindow(char delimiter, string format)
    {
        InitializeComponent();
        Log.Verbose("Loading MainWindow");

        Delimiter = delimiter;
        Format = format;

        if (Delimiter.HasValue && char.IsWhiteSpace(Delimiter.Value))
        {
            Delimiter = Delimiter.Value;
        }
        else
        {
            seperatorTextBox.Text = delimiter.ToString();
        }

        formatTextBox.Text = format;
        saveButton.Enabled = false;
    }

    private void seperatorTextBox_TextChanged(object sender, EventArgs e)
    {
        saveButton.Enabled = true;

        if (sender is TextBox textBox)
        {
            useSpaceButton.Enabled = true;
            useSpaceButton.Text = "Using Space";

            if (textBox.Text.Length == 0)
            {
                useSpaceButton.Enabled = false;
                Delimiter = ' ';
            }

            if (textBox.Text.Length == 1)
            {
                useSpaceButton.Enabled = true;
                useSpaceButton.Text = "Use Space ✓";

                if (!char.TryParse(textBox.Text, out var resultChar))
                {
                    MessageBox.Show(
                        $"{textBox.Text} is not a supported character.",
                        "Invalid seperator",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                Delimiter = resultChar;
            }

            if (textBox.Text.Length > 1)
            {
                useSpaceButton.Enabled = true;
                useSpaceButton.Text = "Use Space ✓";

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
    }

    private void useSpaceButton_Click(object sender, EventArgs e)
    {
        Log.Debug("Using space as the delimiter selected");
        saveButton.Enabled = true;

        useSpaceButton.Enabled = false;
        useSpaceButton.Text = "Use Space ✓";
        Delimiter = ' ';
        seperatorTextBox.Text = "";
    }

    private void formatTextBox_TextChanged(object sender, EventArgs e)
    {
        saveButton.Enabled = true;

        if (sender is TextBox textBox)
        {
            Format = textBox.Text;
        }
    }

    private void saveButton_Click(object sender, EventArgs e)
    {
        Log.Debug("Triggering format save action");
        DialogResult = DialogResult.OK;
    }
}
