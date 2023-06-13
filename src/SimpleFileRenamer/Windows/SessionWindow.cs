using Serilog;
using SimpleFileRenamer.Abstractions.Services;
using SimpleFileRenamer.Abstractions.Windows;

namespace SimpleFileRenamer;

public partial class SessionWindow : BaseWindow
{
    private readonly List<FileSystemWatcher> _fileWatchers = new List<FileSystemWatcher>();
    private readonly IConfigurationService _configuration;
    private readonly ISessionStateService _session;

    private ListViewItem? _personListItem;
    private string? _safePersonName;
    private string? _personName;

    private int _fileCount = 0;
    private int _currentSessionNumber = 0;

    public SessionWindow(IConfigurationService configuration, ISessionStateService session)
    {
        Log.Verbose("Loading session window");
        InitializeComponent();

        _configuration = configuration;
        _session = session;
    }

    public DialogResult ShowDialog(IWin32Window parent, string personName, ListViewItem personListItem)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(parent));
        ArgumentException.ThrowIfNullOrEmpty(nameof(personName));
        ArgumentException.ThrowIfNullOrEmpty(nameof(personListItem));

        Text = $"{personName}'s Session";
        _personName = personName;
        _safePersonName = _personName.Replace(" ", "_").ToLower();

        if (string.IsNullOrWhiteSpace(_safePersonName))
        {
            throw new ArgumentException("Failed to create safe person name with supplied person name", nameof(personName));
        }

        _personListItem = personListItem;

        LoadConfiguration();
        LoadSessionState();

        FilesListView.View = View.Details;
        FilesListView.Columns.Add("Files", FilesListView.Width - 4);

        StartFolderMonitoring();

#pragma warning disable CS0618 // This is for overriding the ShowDialog, ignored
        return ShowDialog(parent);
#pragma warning restore CS0618 // This is for overriding the ShowDialog, ignored
    }

    private void LoadConfiguration()
    {
        Log.Verbose("Loading Configuration");

        var lastSessionNumber = 0;
        if (_configuration.Value.LiveMode.LastSessionNumbers.ContainsKey(_safePersonName!))
        {
            Log.Debug("Configuration found session number {SessionNumber} for {KeyName}",
                _configuration.Value.LiveMode.LastSessionNumbers[_safePersonName!], _safePersonName!);

            lastSessionNumber = _configuration.Value.LiveMode.LastSessionNumbers[_safePersonName!];
        }

        _currentSessionNumber = lastSessionNumber;
        Log.Verbose("Loaded Session Configuration");
    }

    private void StartFolderMonitoring()
    {
        Log.Verbose("Starting folder monitoring");
        var selectedExtensions = _configuration.Value.LiveMode.MonitoredExtensions;

        if (selectedExtensions == null || !selectedExtensions.Any())
        {
            // No extensions selected, disable the FileSystemWatcher
            MessageBox.Show("No file extensions are selected for monitoring. Please select file extensions through the configuration.",
                "No Extensions Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Close();
            return;
        }

        if (!Directory.Exists(_configuration.Value.LiveMode.WatchedFolder))
        {
            Log.Warning("Configured WatchedFolder does not exist: {WatchedFolder}",
                _configuration.Value.LiveMode.WatchedFolder);

            MessageBox.Show("Please check your configuration file, the configured watched folder path is not valid",
                "Configuration Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);

            Close();
            return;
        }

        if (!Directory.Exists(_configuration.Value.LiveMode.DestinationFolder))
        {
            Log.Warning("Configured DestinationFolder does not exist: {DestinationFolder}",
                _configuration.Value.LiveMode.DestinationFolder);

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
                Path = _configuration.Value.LiveMode.WatchedFolder,
                Filter = $"*{extension}",
                EnableRaisingEvents = true,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size
            };

            fileWatcher.Created += FileSystemWatcher_Created;

            _fileWatchers.Add(fileWatcher);
        }

        // Mark the person's status as "Started"
        _personListItem!.SubItems[0].Text = "Started";
    }

    private void SaveSessionState()
    {
        Log.Verbose("Saving session state");
        if (_session.Current != null)
        {
            _session.Current.Name = _personName!;
            _session.Current.CreatedFiles = new();

            // For example, populate the list from the ListView
            foreach (ListViewItem item in FilesListView.Items)
            {
                _session.Current.CreatedFiles.Add(item.Text);
            }

            _session.Save();

            // Save the updated session number back to the config
            _configuration.Value.LiveMode.LastSessionNumbers[_safePersonName!] = _currentSessionNumber;

            // Save the config back to the file
            SaveConfiguration();
            Log.Debug("Sucessfully saved the session state");
            return;
        }

        Log.Warning("No session available to save");
    }

    private void LoadSessionState()
    {
        Log.Verbose("Loading Session State");

        if (!_session.Load(_personName!))
        {
            Log.Warning("The session state failed to load");
            MessageBox.Show($"Failed to load session for {_personName}", "Session Load Failure",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _fileCount = _session.Current?.CreatedFiles.Count ?? 0;

        if (_session.Current?.CreatedFiles.Count > 0)
        {
            // Populate the FilesListView with the loaded session data
            foreach (var fileName in _session.Current.CreatedFiles)
            {
                FilesListView.Items.Add(fileName);
            }
        }

        Log.Verbose("Session state loaded with {FileCount} files", _fileCount);
    }

    private void SaveConfiguration()
    {
        Log.Verbose("Saving configuration");
        _configuration.Save();

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

            var newFileName = $"{_safePersonName!}_{_currentSessionNumber}_{++_fileCount}{Path.GetExtension(e.FullPath)}";
            var destinationPath = Path.Combine(_configuration.Value.LiveMode.DestinationFolder, newFileName);

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
            _personListItem!.SubItems[0].Text = "Completed";

            _session.Finish();

            // Save the updated session number back to the config
            _configuration.Value.LiveMode.LastSessionNumbers[_safePersonName!] = ++_currentSessionNumber;

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
