using Serilog;
using SimpleFileRenamer.Abstractions.Services;
using System.Diagnostics;

namespace SimpleFileRenamer.Services;
public class RenameService : IRenameService
{
    private string? _selectedDirectory;

    private readonly IConfigurationService _configuration;

    public string SelectedDirectory => _selectedDirectory ?? string.Empty;

    public RenameService(IConfigurationService configuration)
    {
        _configuration = configuration;
    }

    public void SelectDirectory(string directoryPath)
    {
        Log.Verbose("Selecting directory '{DirecotryPath}'", directoryPath);
        _selectedDirectory = directoryPath;
    }

    public IEnumerable<FileRename> LoadDirectory(Action<int, int, string>? progressAction)
    {
        if (string.IsNullOrWhiteSpace(_selectedDirectory))
        {
            Log.Verbose("Skipping loading from directory as directory path null or whitespace");
            yield break;
        }
        var stopwatch = new Stopwatch();
        stopwatch.Start();

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

            progressAction?.Invoke(i + 1, files.Length, originalNameFull);
            yield return new FileRename(originalNameFull, originalPath, newName, newFilePath);
        }
        stopwatch.Stop();

        Log.Information("Finished loading files from '{SelectedDirectory}' in {ElapsedMilliseconds}ms",
            _selectedDirectory, stopwatch.ElapsedMilliseconds);
    }

    private static string[] SmartSplit(string input, char delimiter)
    {
        var result = input.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

        return result.Length == 1 && result[0] == input
            ? Array.Empty<string>()
            : result;
    }
}

public class FileRename
{
    public string OriginalName { get; }

    public string OriginalPath { get; }

    public string NewName { get; }

    public string NewPath { get; }

    public FileRename(string originalName, string originalPath, string newName, string newPath)
    {
        OriginalName = originalName;
        OriginalPath = originalPath;

        NewName = newName;
        NewPath = newPath;
    }
}