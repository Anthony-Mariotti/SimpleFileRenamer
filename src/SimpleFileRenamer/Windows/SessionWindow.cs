using Serilog;
using SimpleFileRenamer.Abstractions.Services;
using SimpleFileRenamer.Models.Session;
using SimpleFileRenamer.Services;

namespace SimpleFileRenamer;

public partial class SessionWindow : Form
{
    private readonly List<FileSystemWatcher> _fileWatchers = new List<FileSystemWatcher>();
    private readonly IConfigurationService _configuration;
    private readonly ILiveModeCacheService _liveModeCache;

    private string? _safeSessionName;

    private int _fileCount = 0;

    private string _rowHash = default!;
    private ListViewItem _sessionItem = default!;

    private LiveSession _currentSession = default!;

    private bool _transfering = false;

    public SessionWindow(IConfigurationService configuration, ILiveModeCacheService liveModeCache)
    {
        Log.Verbose("Initializing Session Window");
        _configuration = configuration;
        _liveModeCache = liveModeCache;

        InitializeComponent();

        FilesListView.Items.Clear();
        FilesListView.View = View.Details;
        FilesListView.Columns.Add("Files", FilesListView.Width - 4);
    }

    public void SetSession(string rowHash, ListViewItem sessionItem)
    {
        ArgumentException.ThrowIfNullOrEmpty(rowHash, nameof(rowHash));
        ArgumentNullException.ThrowIfNull(sessionItem, nameof(sessionItem));

        _rowHash = rowHash;
        _sessionItem = sessionItem;

        _currentSession = _liveModeCache.GetLiveSession(_rowHash);

        var sessionName = _sessionItem.SubItems[1].Text;
        Text = $"{_sessionItem.SubItems[1].Text}'s Session";
        _safeSessionName = sessionName.Replace(" ", "_").ToLower();

        _fileCount = _currentSession.Files.Count;
        foreach (var file in _currentSession.Files)
        {
            FilesListView.Items.Add(new ListViewItem(file.Name)
            {
                Tag = file
            });
        }

        StartFolderMonitoring();
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
        _sessionItem!.SubItems[0].Text = "Started";
        _liveModeCache.SetRowStatus(_rowHash, "Started");
    }

    private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
    {
        Log.Verbose("Detected new file {FileName} created in destination folder", e.Name);
        _transfering = true;

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

            // Move and rename the file
            try
            {
                var newFileName = $"{_safeSessionName!}_{_currentSession.SessionId}_{++_fileCount}{Path.GetExtension(e.FullPath)}";
                var destinationPath = Path.Combine(_configuration.Value.LiveMode.DestinationFolder, newFileName);

                // Try to copy the file
                Log.Verbose("Attempting to copy {FilePath} to {DestinationPath}", e.FullPath, destinationPath);
                File.Copy(e.FullPath, destinationPath);

                // Use Invoke to update the UI on the main thread
                Invoke(() =>
                {
                    _currentSession.Files.Add(new SessionFile
                    {
                        Name = newFileName,
                        Path = destinationPath
                    });
                    FilesListView.Items.Add(new ListViewItem(newFileName)
                    {
                        Tag = destinationPath
                    });
                }); // Add to the ListView

                _transfering = false;
                return;
            }
            catch (IOException ex)
            {
                _transfering = false;
                // If max attempts reached, show error message
                Log.Error(ex, "The file could not be processed after multiple attempts");
                MessageBox.Show($"Error handling file copy: {ex.Message}", "Copy Failure",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        _transfering = false;

        Log.Warning("File at {FilePath} was never ready and was unable to be copied after {MaxRetry} retries", e.FullPath, retries);
    }

    private void ExitSessionButton_Click(object sender, EventArgs e)
    {
        if (!_transfering)
        {
            Log.Verbose("Exiting Session");
            _liveModeCache.UpdateLiveSession(_rowHash, _currentSession);
            _liveModeCache.Save();
            Close();
            return;
        }
    }

    private void FinishSessionButton_Click(object sender, EventArgs e)
    {
        Log.Verbose("Attempting to finish session");
        if (!_transfering)
        {
            var dialogResult = MessageBox.Show(
            "Are you sure you want to finish the session?",
            "Finish Session",
            MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                Log.Verbose("Finish session requested");

                // Mark the person as completed in the PeopleListView on the main form
                _sessionItem!.SubItems[0].Text = "Completed";
                _liveModeCache.SetRowStatus(_rowHash, "Completed");

                _currentSession.SessionId++;
                _currentSession.Files.Clear();

                _liveModeCache.UpdateLiveSession(_rowHash, _currentSession);
                _liveModeCache.Save();

                Log.Debug("Successfully finished session");
                Close();
                return;
            }

            return;
        }

        ShowTransferringMessage();
    }

    private void SessionWindow_FormClosing(object sender, FormClosingEventArgs e)
    {
        Log.Verbose("Session window closing");
        if (!_transfering)
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

            Log.Verbose("Shutdown file watchers");
            return;
        }

        e.Cancel = true;
        ShowTransferringMessage();
    }

    private void ShowTransferringMessage() => 
        _ = MessageBox.Show(
            "Cannot close while files are transferring",
            "Can't stop just yet",
            MessageBoxButtons.OK,
            MessageBoxIcon.Exclamation);
}
