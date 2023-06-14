using SimpleFileRenamer.Services;

namespace SimpleFileRenamer.Abstractions.Services;

/// <summary>
/// Provides functionality for managing rename history, allowing undo and redo operations.
/// </summary>
public interface IRenameHistoryService
{
    /// <summary>
    /// Adds a new rename operation history to the stack.
    /// </summary>
    /// <param name="history">The list of rename data to be added to history.</param>
    void AddHistory(List<RenameData> history);

    /// <summary>
    /// Performs an undo operation and returns the previous rename history, if available.
    /// </summary>
    /// <returns>
    /// A list of <see cref="RenameData"/> representing the previous rename history, or null if undo is not possible.
    /// </returns>
    List<RenameData>? Undo();

    /// <summary>
    /// Performs a redo operation and returns the corresponding rename history, if available.
    /// </summary>
    /// <returns>
    /// A list of <see cref="RenameData"/> representing the redo history, or null if redo is not possible.
    /// </returns>
    List<RenameData>? Redo();

    /// <summary>
    /// Determines if an undo operation can be performed.
    /// </summary>
    /// <returns>
    /// <c>true</c> if there is history available to undo; otherwise, <c>false</c>.
    /// </returns>
    bool CanUndo();

    /// <summary>
    /// Determines if a redo operation can be performed.
    /// </summary>
    /// <returns>
    /// <c>true</c> if there is history available to redo; otherwise, <c>false</c>.
    /// </returns>
    bool CanRedo();
}
