using Newtonsoft.Json;
using Serilog;

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
        Log.Verbose("Loading session window");

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
        Log.Verbose("Loading Configuration");
        if (File.Exists(_configFilePath))
        {
            var configJson = File.ReadAllText(_configFilePath);
            var config = JsonConvert.DeserializeObject<LiveViewConfig>(configJson);
            Log.Verbose("Loading configuration from {FilePath}", _configFilePath);

            if (config != null)
            {
                _config = config;

                var safePersonName = _personName.Replace(" ", "_").ToLower();

                // Retrieve the last session number for this person from the config
                var lastSessionNumber = 0;
                if (_config.LastSessionNumbers.ContainsKey(safePersonName))
                {
                    Log.Debug("Configuration found session number {SessionNumber} for {KeyName}", _config.LastSessionNumbers[safePersonName], safePersonName);
                    lastSessionNumber = _config.LastSessionNumbers[safePersonName];
                }

                _currentSessionNumber = lastSessionNumber;
            }
            else
            {
                Log.Warning("Failed to load configuration and asking for re-configuration by user");
                MessageBox.Show("Configuration file failed to load. Please re-configuring the application before starting a session.",
                "Configuration Read Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
        }
        else
        {
            Log.Warning("No configuration was found and asking for configuration by user before use");
            MessageBox.Show("Configuration file not found. Please configure the application before starting a session.",
                "Configuration Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Close();
            return;
        }
    }

    private void StartFolderMonitoring()
    {
        Log.Verbose("Starting folder monitoring");
        var selectedExtensions = _config.MonitoredExtensions;

        if (selectedExtensions == null || !selectedExtensions.Any())
        {
            // No extensions selected, disable the FileSystemWatcher
            MessageBox.Show("No file extensions are selected for monitoring. Please select file extensions through the configuration.",
                "No Extensions Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Close();
            return;
        }

        if (!Directory.Exists(_config.WatchedFolder))
        {
            Log.Warning("Configured WatchedFolder does not exist: {WatchedFolder}", _config.WatchedFolder);
            MessageBox.Show("Please check your configuration file, the configured watched folder path is not valid",
                "Configuration Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            Close();
            return;
        }

        if (!Directory.Exists(_config.DestinationFolder))
        {
            Log.Warning("Configured DestinationFolder does not exist: {DestinationFolder}", _config.DestinationFolder);
            MessageBox.Show("Please check your configuration file, the configured destination folder path is not valid",
                "Configuration Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            Close();
            return;
        }

        Log.Debug("Loading file watchers for {Extensions}", string.Join(", ", selectedExtensions));
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
        Log.Verbose("Saving session state");
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
        Log.Verbose("Saving session state to {FilePath}", sessionStateFilePath);

        File.WriteAllText(sessionStateFilePath, JsonConvert.SerializeObject(sessionState));

        // Save the updated session number back to the config
        _config.LastSessionNumbers[safePersonName] = _currentSessionNumber;

        // Save the config back to the file
        SaveConfiguration();
        Log.Debug("Sucessfully saved the session state");
    }

    private void LoadSessionState()
    {
        Log.Verbose("Loading Session State");

        // Replace spaces with underscores for safe file naming
        var safePersonName = _personName.Replace(" ", "_").ToLower();
        var sessionStateFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"session_{safePersonName}.json");

        if (File.Exists(sessionStateFilePath))
        {
            Log.Verbose("Session state file exists at {FilePath}", sessionStateFilePath);
            var sessionStateJson = File.ReadAllText(sessionStateFilePath);
            var sessionState = JsonConvert.DeserializeObject<SessionState>(sessionStateJson);

            if (sessionState == null)
            {
                Log.Warning("The session state failed to load");
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

            Log.Verbose("Session state loading with {FileCount} files", _fileCount);
        }

        Log.Debug("Sucessfully loaded the session state");
    }

    private void SaveConfiguration()
    {
        Log.Verbose("Saving configuration");
        if (_config != null)
        {
            try
            {
                // Save the config back to the file
                File.WriteAllText(_configFilePath, JsonConvert.SerializeObject(_config));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to save configuration file");
                MessageBox.Show("Failed to save configuration. " + ex.Message,
                    "Error Saving Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        Log.Verbose("Sucessfully saved configuration");
    }

    private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
    {
        Log.Verbose("Detected new file {FileName} created in destination folder", e.Name);

        // The method is triggered as soon as the file is created, it may not be ready to be used yet
        // Wait until the file is released by another process
        bool fileReady = false;

        Log.Debug("Waiting for file to be ready on the disk");
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
            Log.Debug("File ready with {Extension} extension", Path.GetExtension(e.FullPath));
            var safePersonName = _personName.Replace(" ", "_").ToLower();

            var newFileName = $"{safePersonName}_{_currentSessionNumber}_{++_fileCount}{Path.GetExtension(e.FullPath)}";
            var destinationPath = Path.Combine(_config.DestinationFolder, newFileName);

            // Move and rename the file
            try
            {
                // Try to copy the file
                Log.Verbose("Attempting to copy {FilePath} to {DestinationPath}", e.FullPath, destinationPath);
                File.Copy(e.FullPath, destinationPath);

                // Use Invoke to update the UI on the main thread
                Invoke(() => FilesListView.Items.Add(new ListViewItem(newFileName))); // Add to the ListView
                return;
            }
            catch (IOException ex)
            {

                // If max attempts reached, show error message
                Log.Error(ex, "The file could not be processed after multiple attempts");
                MessageBox.Show("Error handling file copy: The file could not be accessed after multiple attempts.", "Copy Failure",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        Log.Warning("File at {FilePath} was never ready and was unable to be copied after {MaxRetry} retries", e.FullPath, retries);
    }

    private void ExitSessionButton_Click(object sender, EventArgs e)
    {
        Log.Verbose("Exiting Session");
        SaveSessionState();
        Close();
    }

    private void FinishSessionButton_Click(object sender, EventArgs e)
    {
        Log.Verbose("Attempting to finish session");
        var dialogResult = MessageBox.Show(
            "Are you sure you want to finish the session?",
            "Finish Session",
            MessageBoxButtons.YesNo);

        if (dialogResult == DialogResult.Yes)
        {
            Log.Verbose("Finish session requested");

            // Mark the person as completed in the PeopleListView on the main form
            _personListItem.SubItems[0].Text = "Completed";

            // Delete the session state file
            var safePersonName = _personName.Replace(" ", "_").ToLower(); // Replace spaces with underscores for safe file naming
            var sessionStateFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"session_{safePersonName}.json");

            if (File.Exists(sessionStateFilePath))
            {
                Log.Verbose("Found and removing session file {FilePath}", sessionStateFilePath);
                File.Delete(sessionStateFilePath);
            }

            // Save the updated session number back to the config
            _config.LastSessionNumbers[safePersonName] = ++_currentSessionNumber;

            // Save the config back to the file
            SaveConfiguration();

            Log.Debug("Successfully finished session");
            Close();
            return;
        }
    }

    private void SessionWindow_FormClosing(object sender, FormClosingEventArgs e)
    {
        Log.Verbose("Session window closing");
        foreach (var watcher in _fileWatchers)
        {
            // Unsubscribe from the created event
            watcher.Created -= FileSystemWatcher_Created;

            // Disable the FileSystemWatcher to stop raising events
            watcher.EnableRaisingEvents = false;

            // Dispose the FileSystemWatcher to release resources
            watcher.Dispose();
        }
        Log.Verbose("Shutdown file watchers");
    }
}
