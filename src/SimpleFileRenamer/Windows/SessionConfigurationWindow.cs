using Serilog;
using SimpleFileRenamer.Abstractions.Services;

namespace SimpleFileRenamer;
public partial class SessionConfigurationWindow : Form
{
    private readonly IConfigurationService _configuration;
    private readonly IReadOnlyCollection<string> _supportedExtensions = new List<string>
    {
        ".jpeg",
        ".jpg",
        ".png",
        ".gif",
        ".raw",
        ".cr2",
        ".nef",
        ".orf",
        ".sr2",
        ".bmp",
        ".tif",
        ".tiff"
    };

    public SessionConfigurationWindow(IConfigurationService configuration)
    {
        Log.Verbose("Initializing Session Configuration Window");
        _configuration = configuration;

        InitializeComponent();

        LoadConfiguration();
        LoadExtensionsIntoList();
    }

    private void LoadConfiguration()
    {
        WatchedFolderTextBox.Text = _configuration.Value.LiveMode.WatchedFolder ?? string.Empty;
        DestinationFolderTextBox.Text = _configuration.Value.LiveMode.DestinationFolder ?? string.Empty;
    }

    private void LoadExtensionsIntoList()
    {
        // Load extensions from the configuration into the SelectedExtensionsListBox.
        SelectedExtensionsListBox.Items.Clear();
        foreach (var extension in _configuration.Value.LiveMode.MonitoredExtensions)
        {
            SelectedExtensionsListBox.Items.Add(extension);
        }

        // Load the fixed list of available extensions into AvailableExtensionsListBox
        AvailableExtensionsListBox.Items.Clear();
        foreach (var extension in _supportedExtensions) // add more extensions here
        {
            // Only add to AvailableExtensionsListBox if not already in SelectedExtensionsListBox
            if (!_configuration.Value.LiveMode.MonitoredExtensions.Contains(extension))
            {
                AvailableExtensionsListBox.Items.Add(extension);
            }
        }
    }

    private void BrowseWatchedFolderButton_Click(object sender, EventArgs e)
    {
        using var dialog = new FolderBrowserDialog();

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            WatchedFolderTextBox.Text = dialog.SelectedPath;
        }
    }

    private void BrowseDestinationFolderButton_Click(object sender, EventArgs e)
    {
        using var dialog = new FolderBrowserDialog();

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            DestinationFolderTextBox.Text = dialog.SelectedPath;
        }
    }

    private void SaveButton_Click(object sender, EventArgs e)
    {
        _configuration.Value.LiveMode.WatchedFolder = WatchedFolderTextBox.Text;
        _configuration.Value.LiveMode.DestinationFolder = DestinationFolderTextBox.Text;
        _configuration.Save();

        Close();
    }

    private void CancelButton_Click(object sender, EventArgs e) => Close();

    private void SelectExtensionButton_Click(object sender, EventArgs e)
    {
        // Move the selected file extension from AvailableExtensionsListBox to SelectedExtensionsListBox.
        if (AvailableExtensionsListBox.SelectedItem is string selectedExtension)
        {
            _configuration.Value.LiveMode.MonitoredExtensions.Add(selectedExtension);
            SelectedExtensionsListBox.Items.Add(selectedExtension);
            AvailableExtensionsListBox.Items.Remove(selectedExtension);
        }
    }

    private void RemoveExtensionButton_Click(object sender, EventArgs e)
    {
        // Remove the selected file extension from SelectedExtensionsListBox and put it back in AvailableExtensionsListBox.
        if (SelectedExtensionsListBox.SelectedItem is string selectedExtension)
        {
            _configuration.Value.LiveMode.MonitoredExtensions.Remove(selectedExtension);
            SelectedExtensionsListBox.Items.Remove(selectedExtension);
            AvailableExtensionsListBox.Items.Add(selectedExtension);
        }
    }
}
