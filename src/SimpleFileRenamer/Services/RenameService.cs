using Serilog;
using SimpleFileRenamer.Abstractions.Services;
using System.ComponentModel;
using System.Diagnostics;

namespace SimpleFileRenamer.Services;

public record RenameData(string OriginalName, string OriginalPath, string NewName, string NewPath);

public class RenameService : IRenameService
{
    #region Internal Properties
    private readonly IConfigurationService _configuration;
    private readonly IRenameHistoryService _renameHistory;

    private readonly BackgroundWorker _renameWorker = new BackgroundWorker();
    private readonly BackgroundWorker _undoWorker = new BackgroundWorker();
    private readonly BackgroundWorker _redoWorker = new BackgroundWorker();

    private readonly List<RenameData> _renameData = new List<RenameData>();

    private readonly bool _simulate = true;
    private readonly int _simulationSpeed = 25;

    private string? _selectedDirectory;
    #endregion

    #region Exposed Properties
    /// <inheritdoc/>
    public string SelectedDirectory => _selectedDirectory ?? string.Empty;

    /// <inheritdoc/>
    public IReadOnlyCollection<RenameData> RenameData => _renameData.AsReadOnly();

    /// <inheritdoc/>
    public bool CanUndo => _renameHistory.CanUndo();

    /// <inheritdoc/>
    public bool CanRedo => _renameHistory.CanRedo();

    /// <inheritdoc/>
    public event Action<int, string>? RenameProgressChanged;
    
    /// <inheritdoc/>
    public event Action<List<RenameData>?>? RenameCompleted;

    /// <inheritdoc/>
    public event Action<int, string>? UndoProgressChanged;

    /// <inheritdoc/>
    public event Action? UndoCompleted;

    /// <inheritdoc/>
    public event Action<int, string>? RedoProgressChanged;

    /// <inheritdoc/>
    public event Action? RedoCompleted;
    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="RenameService"/> class.
    /// </summary>
    /// <param name="configuration">The configuration service used for reading renaming patterns and settings.</param>
    /// <param name="renameHistory">The rename history service for maintaining undo/redo history.</param>
    /// <remarks>
    /// Sets up background workers for renaming, undoing, and redoing operations.
    /// </remarks>
    public RenameService(IConfigurationService configuration, IRenameHistoryService renameHistory)
    {
        _configuration = configuration;
        _renameHistory = renameHistory;

        #region Background Worker Setup
        // Setup RenameWorker
        _renameWorker.WorkerReportsProgress = true;
        _renameWorker.DoWork += RenameWorker_DoWork;
        _renameWorker.ProgressChanged += RenameWorker_ProgressChanged;
        _renameWorker.RunWorkerCompleted += RenameWorker_RunWorkerCompleted;

        // Setup UndoWorker
        _undoWorker.WorkerReportsProgress = true;
        _undoWorker.DoWork += UndoWorker_DoWork;
        _undoWorker.ProgressChanged += UndoWorker_ProgressChanged;
        _undoWorker.RunWorkerCompleted += UndoWorker_RunWorkerCompleted;

        // Setup RedoWorker
        _redoWorker.WorkerReportsProgress = true;
        _redoWorker.DoWork += RedoWorker_DoWork;
        _redoWorker.ProgressChanged += RedoWorker_ProgressChanged;
        _redoWorker.RunWorkerCompleted += RedoWorker_RunWorkerCompleted;
        #endregion
    }

    #region Exposed Methods
    /// <inheritdoc/>
    public void SelectDirectory(string directoryPath)
    {
        Log.Verbose("Selecting directory '{DirecotryPath}'", directoryPath);
        _selectedDirectory = directoryPath;
    }

    /// <inheritdoc/>
    /// <remarks>
    /// This implementation clears any previously loaded data, loads new data from the selected directory,
    /// logs the process, and measures the time taken to load the directory.
    /// </remarks>
    public bool LoadDirectory(Action<int, int, string>? progressAction)
    {
        _renameData.Clear();

        if (string.IsNullOrWhiteSpace(_selectedDirectory))
        {
            Log.Verbose("Skipping loading from directory as directory path null or whitespace");
            return false;
        }
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        try
        {
            var files = Directory.GetFiles(_selectedDirectory);
            Log.Information("Begin loading files from '{SelectedDirectory}'", _selectedDirectory);

            for (int i = 0; i < files.Length; i++)
            {
                var originalPath = files[i];
                Log.Verbose("Processing file '{FilePath}'", originalPath);

                // Generate new file name using the pattern provided by the user
                var originalName = Path.GetFileNameWithoutExtension(originalPath);
                var extension = Path.GetExtension(originalPath);
                var newNameFormat = string.Concat(_configuration.Value.Renamer.Format, extension);

                // Split the original filename into firstname and lastname
                var name = SmartSplit(originalName, _configuration.Value.Renamer.Delimiter);

                string newName = string.Empty;
                if (name.Length > 0)
                {
                    newName = string.Format(newNameFormat, name).ToLower();
                }

                var newFilePath = Path.Combine(
                    Path.GetDirectoryName(originalPath)!,
                    newName);

                var originalNameFull = Path.GetFileName(originalPath);

                _renameData.Add(new RenameData(originalNameFull, originalPath, newName, newFilePath));
                progressAction?.Invoke(i + 1, files.Length, originalNameFull);
                
                //Task.Delay(50).Wait(); // Simulate
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "There was an error loading the files from the directory");
            return false;
        }

        stopwatch.Stop();

        Log.Information("Finished loading files from '{SelectedDirectory}' in {ElapsedMilliseconds}ms",
            _selectedDirectory, stopwatch.ElapsedMilliseconds);

        return true;
    }

    /// <inheritdoc/>
    /// <remarks>
    /// This implementation uses a background worker to perform the renaming operations asynchronously.
    /// </remarks>
    public void StartRenaming()
    {
        if (_renameData != null && !_renameWorker.IsBusy)
        {
            // Start the background worker
            _renameWorker.RunWorkerAsync(_renameData);
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    /// This implementation uses a background worker to perform the undo operations asynchronously.
    /// </remarks>
    public void UndoRename()
    {
        var renameData = _renameHistory.Undo();

        if (renameData != null && !_undoWorker.IsBusy)
        {
            _undoWorker.RunWorkerAsync(renameData);
        }
    }

    /// <inheritdoc/>
    /// <remarks>
    /// This implementation uses a background worker to perform the redo operations asynchronously.
    /// </remarks>
    public void RedoRename()
    {
        var renameData = _renameHistory.Redo();

        if (renameData != null && !_redoWorker.IsBusy)
        {
            _redoWorker.RunWorkerAsync(renameData);
        }
    }
    #endregion

    #region RenameWorker
    private void RenameWorker_DoWork(object? sender, DoWorkEventArgs e)
    {
        Log.Information("Starting processing of renaming files");
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var currentRename = new List<RenameData>();

        if (_renameData == null)
        {
            return;
        }

        var i = 0;
        foreach (var item in _renameData)
        {
            if (item == null)
            {
                i++;
                continue;
            }

            try
            {
                Log.Verbose("Renaming '{OriginalName}' -> '{NewName}'", item.OriginalName, item.NewName);
                _renameWorker.ReportProgress(i + 1, $"{item.OriginalName} -> {item.NewName}");

                if (_simulate)
                {
                    // Simulated rename
                    Task.Delay(_simulationSpeed).Wait();
                }
                else
                {
                    // Rename the file
                    File.Move(item.OriginalPath, item.NewPath);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed rename '{OriginalPath}' -> '{NewPath}'", item.OriginalPath, item.NewPath);
            }
            finally
            {
                currentRename.Add(item);
            }

            i++;
        }

        stopwatch.Stop();
        Log.Information("Finished renaming of {Count} files in {ElapsedMilliseconds}ms", i, stopwatch.ElapsedMilliseconds);

        _renameWorker.ReportProgress(i, $"Renamed {i} files in {stopwatch.ElapsedMilliseconds}ms");
        e.Result = currentRename;
    }

    private void RenameWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
    {
        // Raise the event to notify the UI about progress
        RenameProgressChanged?.Invoke(e.ProgressPercentage, e.UserState?.ToString() ?? string.Empty);
    }

    private void RenameWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
    {
        var result = ((List<RenameData>?)e.Result) ?? new();
        
        _renameHistory.AddHistory(result);
        _selectedDirectory = null;
        _renameData.Clear();

        // Raise the event to notify the UI about completion
        RenameCompleted?.Invoke(result);
    }
    #endregion

    #region UndoWorker
    private void UndoWorker_DoWork(object? sender, DoWorkEventArgs e)
    {
        Log.Information("Starting processing of undoing last rename");
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var undoRename = (List<RenameData>)e.Argument!;

        var i = 0;
        foreach (var item in undoRename)
        {
            try
            {
                Log.Verbose("Undoing rename '{NewName}' -> '{OriginalName}'", item.NewName, item.OriginalName);
                _undoWorker.ReportProgress(i + 1, $"{item.NewName} -> {item.OriginalName}");

                if (_simulate)
                {
                    // Simulated rename
                    Task.Delay(_simulationSpeed).Wait();
                }
                else
                {
                    // Undo rename
                    File.Move(item.NewPath, item.OriginalPath);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to undo rename '{NewName}' -> '{OriginalName}'", item.NewName, item.OriginalName);
            }

            i++;
        }

        stopwatch.Stop();
        Log.Information("Finished undoing rename of {Count} files in {ElapsedMilliseconds}ms", i, stopwatch.ElapsedMilliseconds);

        _undoWorker.ReportProgress(i, $"Undid {i} files in {stopwatch.ElapsedMilliseconds}ms");
    }

    private void UndoWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
    {
        // Raise the event to notify the UI about progress
        UndoProgressChanged?.Invoke(e.ProgressPercentage, e.UserState?.ToString() ?? string.Empty);
    }

    private void UndoWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
    {
        // Raise the event to notify the UI about completion
        UndoCompleted?.Invoke();
    }
    #endregion

    #region RedoWorker
    private void RedoWorker_DoWork(object? sender, DoWorkEventArgs e)
    {
        Log.Information("Starting processing of redoing rename");
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var redoRename = (List<RenameData>)e.Argument!;

        var i = 0;
        foreach (var item in redoRename)
        {
            try
            {
                Log.Verbose("Redoing rename '{OriginalName}' -> '{NewName}'", item.OriginalName, item.NewName);
                _redoWorker.ReportProgress(i + 1, $"{item.OriginalName} -> {item.NewName}");

                if (_simulate)
                {
                    // Simulated rename
                    Task.Delay(_simulationSpeed).Wait();
                }
                else
                {
                    // Redo rename
                    File.Move(item.OriginalPath, item.NewPath);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to redo rename '{OriginalPath}' -> '{NewPath}'", item.OriginalPath, item.NewPath);
            }

            i++;
        }

        stopwatch.Stop();
        Log.Information("Finished redoing rename of {Count} files in {ElapsedMilliseconds}ms", i, stopwatch.ElapsedMilliseconds);

        _redoWorker.ReportProgress(i, $"Redid {i} files in {stopwatch.ElapsedMilliseconds}ms");
    }

    private void RedoWorker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
    {
        // Raise the event to notify the UI about progress
        RedoProgressChanged?.Invoke(e.ProgressPercentage, e.UserState?.ToString() ?? string.Empty);
    }

    private void RedoWorker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
    {
        // Raise the event to notify the UI about completion
        RedoCompleted?.Invoke();
    }
    #endregion

    #region Internal Methods
    private static string[] SmartSplit(string input, char delimiter)
    {
        var result = input.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

        return result.Length == 1 && result[0] == input
            ? Array.Empty<string>()
            : result;
    }
    #endregion
}
