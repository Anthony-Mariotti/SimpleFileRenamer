using SimpleFileRenamer.Services;

namespace SimpleFileRenamer.Abstractions.Services;

public interface IRenameService
{
    /// <summary>
    /// Gets the selected directory path.
    /// </summary>
    /// <returns>
    /// The path of the selected directory as a string. Returns an empty string if no directory is selected.
    /// </returns>
    string SelectedDirectory { get; }

    /// <summary>
    /// Gets the collection of data representing the renaming operations.
    /// </summary>
    /// <returns>
    /// A read-only collection of <see cref="RenameData"/> representing the original and new names and paths.
    /// </returns>
    IReadOnlyCollection<RenameData> RenameData { get; }

    /// <summary>
    /// Gets a value indicating whether an undo operation can be performed.
    /// </summary>
    /// <returns>
    /// Returns <c>true</c> if there is at least one rename operation in the history that can be undone; otherwise, <c>false</c>.
    /// </returns>
    bool CanUndo { get; }

    /// <summary>
    /// Gets a value indicating whether a redo operation can be performed.
    /// </summary>
    /// <returns>
    /// Returns <c>true</c> if there is at least one rename operation in the history that can be redone; otherwise, <c>false</c>.
    /// </returns>
    bool CanRedo { get; }

    /// <summary>
    /// Event triggered during the renaming process with progress updates.
    /// </summary>
    event Action<int, string>? RenameProgressChanged;

    /// <summary>
    /// Event triggered when the renaming process is completed.
    /// </summary>
    event Action<List<RenameData>?>? RenameCompleted;

    /// <summary>
    /// Event triggered during the undo process with progress updates.
    /// </summary>
    event Action<int, string>? UndoProgressChanged;

    /// <summary>
    /// Event triggered when the undo process is completed.
    /// </summary>
    event Action? UndoCompleted;

    /// <summary>
    /// Event triggered during the redo process with progress updates.
    /// </summary>
    event Action<int, string>? RedoProgressChanged;

    /// <summary>
    /// Event triggered when the redo process is completed.
    /// </summary>
    event Action? RedoCompleted;

    /// <summary>
    /// Sets the directory to be used by the rename service.
    /// </summary>
    /// <param name="directoryPath">The path to the directory.</param>
    void SelectDirectory(string directoryPath);

    /// <summary>
    /// Loads the directory specified in <see cref="SelectDirectory"/> and initializes the rename process.
    /// </summary>
    /// <param name="progressAction">Action to report the progress of loading.</param>
    /// <returns>True if the directory is successfully loaded, false otherwise.</returns>
    bool LoadDirectory(Action<int, int, string>? progressAction);

    /// <summary>
    /// Starts the renaming process.
    /// </summary>
    void StartRenaming();

    /// <summary>
    /// Undoes the most recent renaming operation.
    /// </summary>
    void UndoRename();

    /// <summary>
    /// Redoes the most recently undone renaming operation.
    /// </summary>
    void RedoRename();
}