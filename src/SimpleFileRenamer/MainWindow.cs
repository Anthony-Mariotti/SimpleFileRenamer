using Serilog;
using System.ComponentModel;
using System.Diagnostics;

namespace SimpleFileRenamer;

public partial class MainWindow : Form
{
    private Stack<Dictionary<string, string>> renamedFileStack = new Stack<Dictionary<string, string>>();

    private char Delimiter { get; set; }
    private string Format { get; set; }

    private string? DirectoryPath { get; set; }

    public MainWindow()
    {
        InitializeComponent();
        Log.Verbose("Loading MainWindow");

        loadedListView.Partner = previewListView;
        previewListView.Partner = loadedListView;

        loadedListView.View = View.Details;
        loadedListView.Columns.Add("Loaded File", loadedListView.Width - 25);

        previewListView.View = View.Details;
        previewListView.Columns.Add("Rename Preview", previewListView.Width - 25);


        if (char.TryParse(Settings.Default.Delimiter, out var delimiter))
        {
            Log.Debug("Delimiter parsed to {Delimiter} successfully", delimiter);
            Delimiter = delimiter;
        }
        else
        {
            if (Settings.Default.Delimiter == "[SPACE]")
            {
                Log.Debug("Delimiter parsed to {Delimiter} successfully", "[SPACE]");
                Delimiter = ' ';
            }
            else
            {
                Log.Debug("Delimiter was not able to be read and hard reest", delimiter);
                var hardResetDelimiter = '-';
                Delimiter = hardResetDelimiter;
                Settings.Default.Delimiter = hardResetDelimiter.ToString();
                Settings.Default.Save();
            }
        }

        Format = Settings.Default.Format;
    }

    private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
    {
        using var folderDialog = new FolderBrowserDialog
        {
            UseDescriptionForTitle = true,
            Description = "Select a folder to load files from"
        };

        if (folderDialog.ShowDialog() == DialogResult.OK)
        {
            Log.Debug("Selected {Folder} to load from", folderDialog.SelectedPath);
            DirectoryPath = folderDialog.SelectedPath;
            LoadFromDirectory(DirectoryPath);

        }
    }

    private void closeToolStripMenuItem_Click(object sender, EventArgs e) => Close();

    private void loadedListView_SelectedIndexChanged(object sender, EventArgs e)
    {
        previewListView.SelectedItems.Clear();

        // Select the corresponding items in listView2 based on the selected indices in listView1
        foreach (int selectedIndex in loadedListView.SelectedIndices)
        {
            if (selectedIndex >= 0 && selectedIndex < previewListView.Items.Count)
            {
                previewListView.Items[selectedIndex].Selected = true;
            }
        }
    }

    private void buttonRename_Click(object sender, EventArgs e)
    {
        if (!renameWorker.IsBusy && loadedListView.Items.Count > 0)
        {
            renameProgressBar.Maximum = loadedListView.Items.Count;
            renameProgressBar.Value = 0;

            // Create lists of file paths and new filenames in the UI thread
            var filePaths = new List<string>();
            var newFilenames = new List<string>();

            foreach (ListViewItem item in loadedListView.Items)
            {
                filePaths.Add((string)item.Tag);
            }

            foreach (ListViewItem item in previewListView.Items)
            {
                newFilenames.Add(item.Text);
            }

            // Pass the file paths to the BackgroundWorker
            renameWorker.RunWorkerAsync(Tuple.Create(filePaths, newFilenames));
        }
    }

    private void undoToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (undoWorker.IsBusy)
        {
            return;
        }

        if (renamedFileStack.TryPop(out var undoRename))
        {
            renameProgressBar.Value = 0;
            renameProgressBar.Maximum = undoRename.Count;

            undoWorker.RunWorkerAsync(undoRename);
        }

        if (renamedFileStack.Count <= 0)
        {
            undoToolStripMenuItem.Enabled = false;
        }
    }

    private void renameWorker_DoWork(object sender, DoWorkEventArgs e)
    {
        var currentRename = new Dictionary<string, string>();

        var args = (Tuple<List<string>, List<string>>)e.Argument!;

        var filePaths = args.Item1;
        var newFilenames = args.Item2;

        for (var i = 0; i < filePaths.Count; i++)
        {
            var originalFilePath = filePaths[i];

            var newFilePath = Path.Combine(
                Path.GetDirectoryName(originalFilePath)!,
                newFilenames[i]);

            currentRename[originalFilePath] = newFilePath;

            try
            {
                Debug.WriteLine($"Renaming {originalFilePath} -> {newFilePath}", "[FileRenameWorker]");
                renameWorker.ReportProgress(
                    i + 1, $"{Path.GetFileName(originalFilePath)} -> {Path.GetFileName(newFilePath)}");

                //Task.Delay(500).Wait(); // Simulated Rename
                File.Move(originalFilePath, newFilePath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed rename {originalFilePath} -> {newFilePath} - {ex.Message}", "[FileRenameWorker]");
            }
        }

        e.Result = currentRename;
    }

    private void renameWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        renameProgressBar.Value = e.ProgressPercentage;
        renameStatusLabel.Text = e.UserState!.ToString();
    }

    private void renameWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        renamedFileStack.Push((Dictionary<string, string>)e.Result!);
        renameStatusLabel.Text = "Done";
        buttonRename.Enabled = false;
        undoToolStripMenuItem.Enabled = true;

        loadedListView.Items.Clear();
        previewListView.Items.Clear();
    }

    private void undoWorker_DoWork(object sender, DoWorkEventArgs e)
    {
        var undoRename = (Dictionary<string, string>)e.Argument!;

        renameProgressBar.Value = 0;
        renameProgressBar.Maximum = undoRename.Count;

        var index = 0;
        foreach (var item in undoRename)
        {
            var newFilePath = item.Value;
            var originalFilePath = item.Key;

            try
            {
                Debug.WriteLine($"Undo renaming {newFilePath} -> {originalFilePath}", "[UndoRenameWorker]");
                undoWorker.ReportProgress(
                    index + 1, $"{Path.GetFileName(originalFilePath)} -> {Path.GetFileName(newFilePath)}");

                //Task.Delay(500).Wait(); // Simulated Rename
                File.Move(newFilePath, originalFilePath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed undo rename {newFilePath} -> {newFilePath} - {ex.Message}", "[UndoRenameWorker]");
            }

            index++;
        }
    }

    private void undoWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        renameProgressBar.Value = e.ProgressPercentage;
        renameStatusLabel.Text = e.UserState!.ToString();
    }

    private void undoWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        renameStatusLabel.Text = "Done";
        buttonRename.Enabled = false;
    }

    private void removeLoadedFileMenuItem_Click(object sender, EventArgs e)
    {
        if (loadedListView.SelectedItems.Count > 0)
        {
            foreach (ListViewItem item in loadedListView.SelectedItems)
            {
                var loadedIndex = loadedListView.Items.IndexOf(item);

                loadedListView.Items.Remove(item);
                previewListView.Items.RemoveAt(loadedIndex);
            }
        }
    }

    private void loadedFileContextMenu_Opening(object sender, CancelEventArgs e)
    {
        if (loadedListView.SelectedItems.Count == 0)
        {
            removeLoadedFileMenuItem.Enabled = false;
        }
        else
        {
            removeLoadedFileMenuItem.Enabled = true;
        }
    }

    private void formatToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Log.Verbose("Attempting to open Format Window");
        var formatDialog = new FormatWindow(Delimiter, Format);

        if (formatDialog.ShowDialog() == DialogResult.OK)
        {
            if (formatDialog.Delimiter.HasValue &&
                char.IsWhiteSpace(formatDialog.Delimiter.Value))
            {
                Log.Debug("Format Window returned with '{Delimiter}' delimiter", "[SPACE]");
                Delimiter = formatDialog.Delimiter.Value;
                Settings.Default.Delimiter = "[SPACE]";
            }
            else if (formatDialog.Delimiter.HasValue)
            {
                Log.Debug("Format Window returned with '{Delimiter}' delimiter", formatDialog.Delimiter);
                Delimiter = formatDialog.Delimiter.Value;
                Settings.Default.Delimiter = formatDialog.Delimiter.ToString();
            }

            if (Format != formatDialog.Format && !string.IsNullOrWhiteSpace(DirectoryPath))
            {
                Log.Debug("New format '{Format}' has been chosen", formatDialog.Format);
                Format = formatDialog.Format ?? Settings.Default.Format;
                Settings.Default.Format = Format;

                LoadFromDirectory(DirectoryPath);
            }

            Settings.Default.Save();

            return;
        }

        Debug.WriteLine("Cancelled format settings", "[FormatMenuItem]");
    }

    private void LoadFromDirectory(string directoryPath)
    {
        if (string.IsNullOrWhiteSpace(directoryPath))
        {
            Log.Verbose("Skipping loading from directory as directory path null or whitespace");
            return;
        }

        var files = Directory.GetFiles(directoryPath);

        // Clear the ListView before adding new items
        loadedListView.Items.Clear();
        previewListView.Items.Clear();

        renameStatusLabel.Text = $"Loading {directoryPath}";
        renameProgressBar.Maximum = files.Length;
        renameProgressBar.Value = 0;

        Log.Verbose("Begining file load into the views");
        // Add the files to the ListView
        foreach (var file in files)
        {
            //Task.Delay(100).Wait(); // Simulate
            Debug.WriteLine(file, "[FileLoading]");
            renameProgressBar.Value++;

            // Create a new ListViewItem with the filename
            var item = new ListViewItem(Path.GetFileName(file))
            {
                // Store the full file path in the Tag property
                Tag = file
            };

            // Add the item to the ListView
            loadedListView.Items.Add(item);

            // Generate new file name using the pattern provided by the user
            var originalFileName = Path.GetFileNameWithoutExtension(file);
            var extension = Path.GetExtension(file);
            var newNameFormat = $"{Format}{extension}";

            // Split the original filename into firstname and lastname
            var nameParts = SmartSplit(originalFileName, Delimiter);

            if (nameParts.Length > 0)
            {
                // Generate new file name using the format "qr_firstname_lastname.png"
                var newFileName = string.Format(newNameFormat, nameParts).ToLower();

                // Create a new ListViewItem with the filename
                var newItem = new ListViewItem(newFileName)
                {
                    Tag = file
                };

                // Add the item to the ListView
                previewListView.Items.Add(newItem);
            }
            else
            {
                Log.Debug("Failed to split {FileName} into supplied template", originalFileName);
                previewListView.Items.Add(new ListViewItem
                {
                    Tag = file
                });
            }
        }

        renameProgressBar.Value = 0;
        renameStatusLabel.Text = "Done";
        buttonRename.Enabled = true;
    }

    private string[] SmartSplit(string input, char delimiter)
    {
        var result = input.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

        return result.Length == 1 && result[0] == input
            ? Array.Empty<string>()
            : result;
    }

    private void liveModeToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Hide();
        using var liveModeDialog = new LiveMode();
        liveModeDialog.FormClosed += (s, e) => Show();
        liveModeDialog.ShowDialog();
    }

    private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
    {
        Log.Information("Application Shutting Down");
    }
}
