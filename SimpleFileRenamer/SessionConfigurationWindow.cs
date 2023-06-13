using Newtonsoft.Json;

namespace SimpleFileRenamer;
public partial class SessionConfigurationWindow : Form
{
    private string _configFilePath;
    private LiveViewConfig _config = default!;
    private readonly IReadOnlyCollection<string> _possibleExtensions = new List<string>
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

    public SessionConfigurationWindow()
    {
        InitializeComponent();

        _configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
        LoadConfiguration();
        LoadExtensionsIntoList();
    }

    private void LoadConfiguration()
    {
        if (File.Exists(_configFilePath))
        {
            var configJson = File.ReadAllText(_configFilePath);
            _config = JsonConvert.DeserializeObject<LiveViewConfig>(configJson) ?? new();

            WatchedFolderTextBox.Text = _config?.WatchedFolder ?? string.Empty;
            DestinationFolderTextBox.Text = _config?.DestinationFolder ?? string.Empty;
        }
    }

    private void LoadExtensionsIntoList()
    {
        // Load extensions from the configuration into the SelectedExtensionsListBox.
        SelectedExtensionsListBox.Items.Clear();
        foreach (var extension in _config.MonitoredExtensions)
        {
            SelectedExtensionsListBox.Items.Add(extension);
        }

        // Load the fixed list of available extensions into AvailableExtensionsListBox
        AvailableExtensionsListBox.Items.Clear();
        foreach (var extension in _possibleExtensions) // add more extensions here
        {
            // Only add to AvailableExtensionsListBox if not already in SelectedExtensionsListBox
            if (!_config.MonitoredExtensions.Contains(extension))
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
        var config = new LiveViewConfig
        {
            WatchedFolder = WatchedFolderTextBox.Text,
            DestinationFolder = DestinationFolderTextBox.Text,
            MonitoredExtensions = _config.MonitoredExtensions,
        };

        File.WriteAllText(_configFilePath, JsonConvert.SerializeObject(config));
        Close();
    }

    private void CancelButton_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void SelectExtensionButton_Click(object sender, EventArgs e)
    {
        // Move the selected file extension from AvailableExtensionsListBox to SelectedExtensionsListBox.
        if (AvailableExtensionsListBox.SelectedItem is string selectedExtension)
        {
            _config.MonitoredExtensions.Add(selectedExtension);
            SelectedExtensionsListBox.Items.Add(selectedExtension);
            AvailableExtensionsListBox.Items.Remove(selectedExtension);
        }
    }

    private void RemoveExtensionButton_Click(object sender, EventArgs e)
    {
        // Remove the selected file extension from SelectedExtensionsListBox and put it back in AvailableExtensionsListBox.
        var selectedExtension = SelectedExtensionsListBox.SelectedItem as string;
        if (selectedExtension != null)
        {
            _config.MonitoredExtensions.Remove(selectedExtension);
            SelectedExtensionsListBox.Items.Remove(selectedExtension);
            AvailableExtensionsListBox.Items.Add(selectedExtension);
        }
    }
}
