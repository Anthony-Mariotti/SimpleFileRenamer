using SimpleFileRenamer.Services;

namespace SimpleFileRenamer.Abstractions.Services;

public interface IRenameService
{
    public string SelectedDirectory { get; }

    void SelectDirectory(string directoryPath);

    IEnumerable<FileRename> LoadDirectory(Action<int, int, string>? progressAction);
}