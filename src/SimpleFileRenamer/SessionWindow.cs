using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;

namespace SimpleFileRenamer;

public partial class SessionWindow : Form
{
    private readonly string _configFilePath;
    private readonly ListViewItem _personListItem;
    private readonly List<FileSystemWatcher> _fileWatchers = new List<FileSystemWatcher>();
    private readonly string _personName;

    private LiveViewConfig _config = default!;
    private int _fileCount = 0;
    private int _currentSessionNumber = 0;

    public SessionWindow(string personName, ListViewItem personListItem)
    {
        InitializeComponent();
        Text = $"{personName}'s Session";

        _configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
        _personName = personName;
        _personListItem = personListItem;

        LoadConfiguration();
        LoadSessionState();

        FilesListView.View = View.Details;
        FilesListView.Columns.Add("Files", FilesListView.Width - 4);

        StartFolderMonitoring();
    }

    private void LoadConfiguration()
    {
        if (File.Exists(_configFilePath))
        {
            var configJson = File.ReadAllText(_configFilePath);
            var config = JsonConvert.DeserializeObject<LiveViewConfig>(configJson);

            if (config != null)
            {
                _config = config;

                var safePersonName = _personName.Replace(" ", "_").ToLower();

                // Retrieve the last session number for this person from the config
                var lastSessionNumber = 0;
                if (_config.LastSessionNumbers.ContainsKey(safePersonName))
                {
                    lastSessionNumber = _config.LastSessionNumbers[safePersonName];
                }

                _currentSessionNumber = lastSessionNumber;
            }
            else
            {
                MessageBox.Show("Configuration file failed to load. Please re-configuring the application before starting a session.",
                "Configuration Read Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
        }
        else
        {
            MessageBox.Show("Configuration file not found. Please configure the application before starting a session.",
                "Configuration Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Close();
            return;
        }
    }

    private void StartFolderMonitoring()
    {
        var selectedExtensions = _config.MonitoredExtensions;

        if (selectedExtensions == null || !selectedExtensions.Any())
        {
            // No extensions selected, disable the FileSystemWatcher
            MessageBox.Show("No file extensions are selected for monitoring. Please select file extensions through the configuration.",
                "No Extensions Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Close();
            return;
        }

        foreach (var extension in selectedExtensions)
        {
            var fileWatcher = new FileSystemWatcher
            {
                Path = _config.WatchedFolder,
                Filter = $"*{extension}",
                EnableRaisingEvents = true,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size
            };

            fileWatcher.Created += FileSystemWatcher_Created;

            _fileWatchers.Add(fileWatcher);
        }

        // Mark the person's status as "Started"
        _personListItem.SubItems[0].Text = "Started";
    }

    private void SaveSessionState()
    {
        var sessionState = new SessionState
        {
            PersonName = _personName,
            CreatedFiles = new List<string>() // You'll populate this list with the file names created during the session
        };

        // For example, populate the list from the ListView
        foreach (ListViewItem item in FilesListView.Items)
        {
            sessionState.CreatedFiles.Add(item.Text);
        }

        // Replace spaces with underscores for safe file naming
        var safePersonName = _personName.Replace(" ", "_").ToLower();
        var sessionStateFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"session_{safePersonName}.json");
        
        File.WriteAllText(sessionStateFilePath, JsonConvert.SerializeObject(sessionState));

        // Save the updated session number back to the config
        _config.LastSessionNumbers[safePersonName] = _currentSessionNumber;

        // Save the config back to the file
        SaveConfiguration();
    }

    private void LoadSessionState()
    {
        // Replace spaces with underscores for safe file naming
        var safePersonName = _personName.Replace(" ", "_").ToLower();
        var sessionStateFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"session_{safePersonName}.json");

        if (File.Exists(sessionStateFilePath))
        {
            var sessionStateJson = File.ReadAllText(sessionStateFilePath);
            var sessionState = JsonConvert.DeserializeObject<SessionState>(sessionStateJson);

            if (sessionState == null)
            {
                MessageBox.Show($"Failed to load session for {_personName}", "Session Load Failure",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //_personName = sessionState.PersonName;
            _fileCount = sessionState.CreatedFiles.Count;

            // Populate the FilesListView with the loaded session data
            foreach (var fileName in sessionState.CreatedFiles)
            {
                FilesListView.Items.Add(fileName);
            }
        }
    }

    private void SaveConfiguration()
    {
        if (_config != null)
        {
            try
            {
                // Save the config back to the file
                File.WriteAllText(_configFilePath, JsonConvert.SerializeObject(_config));
            }
            catch (Exception ex)
            {
                // Log error or show message to user
                MessageBox.Show("Failed to save configuration. " + ex.Message,
                    "Error Saving Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
    {
        // The method is triggered as soon as the file is created, it may not be ready to be used yet
        // Wait until the file is released by another process
        bool fileReady = false;

        var retries = 10; // number of retries
        while (!fileReady && retries > 0)
        {
            try
            {
                using var stream = File.Open(e.FullPath, FileMode.Open, FileAccess.ReadWrite);
                fileReady = true;
            }
            catch (IOException)
            {
                retries--;
                Thread.Sleep(500); // Wait and retry
            }
        }

        if (fileReady)
        {
            var safePersonName = _personName.Replace(" ", "_").ToLower();

            var newFileName = $"{safePersonName}_{_currentSessionNumber}_{++_fileCount}{Path.GetExtension(e.FullPath)}";
            var destinationPath = Path.Combine(_config.DestinationFolder, newFileName);

            // Move and rename the file
            try
            {
                // Try to copy the file
                File.Copy(e.FullPath, destinationPath);

                // Use Invoke to update the UI on the main thread
                Invoke(() => FilesListView.Items.Add(new ListViewItem(newFileName))); // Add to the ListView
            }
            catch (IOException ex)
            {
                Debug.WriteLine(ex, "[SessionFileWatcher]");
                // If max attempts reached, show error message
                MessageBox.Show("Error handling file copy: The file could not be accessed after multiple attempts.", "Copy Failure",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void ExitSessionButton_Click(object sender, EventArgs e)
    {
        SaveSessionState();

        // Logic to handle session close (e.g. you can just close the form)
        Close();
    }

    private void FinishSessionButton_Click(object sender, EventArgs e)
    {
        var dialogResult = MessageBox.Show(
            "Are you sure you want to finish the session?",
            "Finish Session",
            MessageBoxButtons.YesNo);

        if (dialogResult == DialogResult.Yes)
        {
            // Mark the person as completed in the PeopleListView on the main form
            _personListItem.SubItems[0].Text = "Completed";

            // Delete the session state file
            var safePersonName = _personName.Replace(" ", "_").ToLower(); // Replace spaces with underscores for safe file naming
            var sessionStateFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"session_{safePersonName}.json");

            if (File.Exists(sessionStateFilePath))
            {
                File.Delete(sessionStateFilePath);
            }

            // Save the updated session number back to the config
            _config.LastSessionNumbers[safePersonName] = ++_currentSessionNumber;

            // Save the config back to the file
            SaveConfiguration();

            Close();
        }
    }

    private void SessionWindow_FormClosing(object sender, FormClosingEventArgs e)
    {
        foreach (var watcher in _fileWatchers)
        {
            // Unsubscribe from the created event
            watcher.Created -= FileSystemWatcher_Created;

            // Disable the FileSystemWatcher to stop raising events
            watcher.EnableRaisingEvents = false;

            // Dispose the FileSystemWatcher to release resources
            watcher.Dispose();
        }
    }
}
