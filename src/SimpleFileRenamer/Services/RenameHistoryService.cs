using SimpleFileRenamer.Abstractions.Services;

namespace SimpleFileRenamer.Services;

/// <summary>
/// Provides functionality for managing rename history, allowing undo and redo operations.
/// </summary>
public class RenameHistoryService : IRenameHistoryService
{
    private readonly Stack<List<RenameData>> _undoHistory = new Stack<List<RenameData>>();
    private readonly Stack<List<RenameData>> _redoHistory = new Stack<List<RenameData>>();

    /// <summary>
    /// Initializes a new instance of the <see cref="RenameHistoryService"/> class.
    /// </summary>
    public RenameHistoryService()
    {
        
    }

    /// <inheritdoc/>
    public void AddHistory(List<RenameData> renameData)
    {
        _undoHistory.Push(renameData);
        _redoHistory.Clear();
    }

    /// <inheritdoc/>
    public List<RenameData>? Undo()
    {
        if (CanUndo())
        {
            var history = _undoHistory.Pop();
            _redoHistory.Push(history);
            return history;
        }

        return default;
    }

    /// <inheritdoc/>
    public List<RenameData>? Redo()
    {
        if (CanRedo())
        {
            var history = _redoHistory.Pop();
            _undoHistory.Push(history);
            return history;
        }

        return default;
    }

    /// <inheritdoc/>
    public bool CanUndo() => _undoHistory.Count > 0;

    /// <inheritdoc/>
    public bool CanRedo() => _redoHistory.Count > 0;
}
