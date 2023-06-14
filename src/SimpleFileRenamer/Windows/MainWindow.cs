using Serilog;
using SimpleFileRenamer.Abstractions.Factory;
using SimpleFileRenamer.Abstractions.Services;
using SimpleFileRenamer.Services;
using System.ComponentModel;
using System.Diagnostics;

namespace SimpleFileRenamer;

public partial class MainWindow : Form
{
    private readonly IWindowFactory _windowFactory;
    private readonly IRenameService _renameService;

    private readonly Stack<List<FileRename>> _undoActions = new Stack<List<FileRename>>();
    private readonly Stack<List<FileRename>> _redoActions = new Stack<List<FileRename>>();

    public MainWindow(IWindowFactory windowFactory, IRenameService renameService)
    {
        Log.Verbose("Initializing Main Window");
        _windowFactory = windowFactory;
        _renameService = renameService;

        InitializeComponent();

        LoadedListView.Partner = PreviewListView;
        PreviewListView.Partner = LoadedListView;

        LoadedListView.View = View.Details;
        LoadedListView.Columns.Add("Loaded File", LoadedListView.Width - 25);

        PreviewListView.View = View.Details;
        PreviewListView.Columns.Add("Rename Preview", PreviewListView.Width - 25);
    }

    #region MainWindow
    private void MainWindow_FormClosing(object sender, FormClosingEventArgs e) =>
        Log.Information("Application Shutting Down");
    #endregion

    #region MenuStrip
    private void OpenFolderToolStripMenuItem_Click(object sender, EventArgs e)
    {
        using var folderDialog = new FolderBrowserDialog
        {
            UseDescriptionForTitle = true,
            Description = "Select a folder to load files from"
        };

        if (folderDialog.ShowDialog() == DialogResult.OK)
        {
            Log.Debug("Selected {Folder} to load from", folderDialog.SelectedPath);
            _renameService.SelectDirectory(folderDialog.SelectedPath);

            LoadFromDirectory();
        }
    }

    private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Log.Verbose("Attempting to open Format Window");
        using var configurationWindow = _windowFactory.CreateRenameConfigurationWindow();
        _ = configurationWindow.ShowDialog();

        LoadFromDirectory();
    }

    private void ExitToolStripMenuItem_Click(object sender, EventArgs e) => Close();

    private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (UndoWorker.IsBusy)
        {
            return;
        }

        if (_undoActions.TryPop(out var undoRename))
        {
            _redoActions.Push(undoRename);
            RedoToolStripMenuItem.Enabled = true;

            RenameProgressBar.Value = 0;
            RenameProgressBar.Maximum = undoRename.Count;

            UndoWorker.RunWorkerAsync(undoRename);
        }

        if (_undoActions.Count <= 0)
        {
            UndoToolStripMenuItem.Enabled = false;
        }
    }

    private void LiveModeToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Hide();
        using var liveModeDialog = _windowFactory.CreateLiveModeWindow();
        liveModeDialog.FormClosed += (s, e) => Show();
        liveModeDialog.ShowDialog();
    }

    private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }
    #endregion

    #region LoadedListView
    private void LoadedListView_SelectedIndexChanged(object sender, EventArgs e)
    {
        PreviewListView.SelectedItems.Clear();

        // Select the corresponding items in listView2 based on the selected indices in listView1
        foreach (int selectedIndex in LoadedListView.SelectedIndices)
        {
            if (selectedIndex >= 0 && selectedIndex < PreviewListView.Items.Count)
            {
                PreviewListView.Items[selectedIndex].Selected = true;
            }
        }
    }

    private void LoadedListView_KeyUp(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Delete)
        {
            RemoveSelectedItems();
            return;
        }
    }
    #endregion

    #region RenameWorker
    private void RenameWorker_DoWork(object sender, DoWorkEventArgs e)
    {
        Log.Information("Starting processing of renaming files");
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var currentRename = new List<FileRename>();

        var renameList = (IEnumerable<FileRename?>?)e.Argument;

        if (renameList == null)
        {
            return;
        }

        var i = 0;
        foreach (var item in renameList)
        {
            if (item == null)
            {
                i++;
                continue;
            }

            try
            {
                Log.Verbose("Renaming '{OriginalName}' -> '{NewName}'", item.OriginalName, item.NewName);
                RenameWorker.ReportProgress(i + 1, $"{item.OriginalName} -> {item.NewName}");

                //Task.Delay(125).Wait(); // Simulated rename

                // Rename the file
                File.Move(item.OriginalPath, item.NewPath);
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

        RenameWorker.ReportProgress(i, $"Renamed {i} files in {stopwatch.ElapsedMilliseconds}ms");
        e.Result = currentRename;
    }

    private void RenameWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        RenameProgressBar.Value = e.ProgressPercentage;
        RenameStatusLabel.Text = e.UserState!.ToString();
    }

    private void RenameWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        _undoActions.Push((List<FileRename>)e.Result!);
        ButtonRename.Enabled = false;
        UndoToolStripMenuItem.Enabled = true;

        LoadedListView.Items.Clear();
        PreviewListView.Items.Clear();
    }
    #endregion

    #region UndoWorker
    private void UndoWorker_DoWork(object sender, DoWorkEventArgs e)
    {
        Log.Information("Starting processing of undoing last rename");
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var undoRename = (List<FileRename>)e.Argument!;

        RenameProgressBar.Value = 0;
        RenameProgressBar.Maximum = undoRename.Count;

        var i = 0;
        foreach (var item in undoRename)
        {
            try
            {
                Log.Verbose("Undoing rename '{NewName}' -> '{OriginalName}'", item.NewName, item.OriginalName);
                UndoWorker.ReportProgress(i + 1, $"{item.NewName} -> {item.OriginalName}");

                //Task.Delay(125).Wait(); // Simulated rename

                // Undo rename
                File.Move(item.NewPath, item.OriginalPath);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to undo rename '{NewName}' -> '{OriginalName}'", item.NewName, item.OriginalName);
            }

            i++;
        }

        stopwatch.Stop();
        Log.Information("Finished undoing rename of {Count} files in {ElapsedMilliseconds}ms", i, stopwatch.ElapsedMilliseconds);

        UndoWorker.ReportProgress(i, $"Undid {i} files in {stopwatch.ElapsedMilliseconds}ms");
    }

    private void UndoWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        RenameProgressBar.Value = e.ProgressPercentage;
        RenameStatusLabel.Text = e.UserState!.ToString();
    }

    private void UndoWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        ButtonRename.Enabled = false;
    }
    #endregion

    #region LoadedListViewContextMenu
    private void LoadedListViewContextMenu_Opening(object sender, CancelEventArgs e) =>
        RemoveLoadedFileMenuItem.Enabled = LoadedListView.SelectedItems.Count != 0;

    private void RemoveLoadedFileMenuItem_Click(object sender, EventArgs e) =>
        RemoveSelectedItems();
    #endregion

    #region UI Buttons
    private void ButtonRename_Click(object sender, EventArgs e)
    {
        if (!RenameWorker.IsBusy && LoadedListView.Items.Count > 0)
        {
            RenameProgressBar.Maximum = LoadedListView.Items.Count;
            RenameProgressBar.Value = 0;

            // Create a list of the file rename data within the UI thread.
            var renameList = new List<FileRename?>();
            foreach (ListViewItem item in LoadedListView.Items)
            {
                renameList.Add(item.Tag as FileRename);
            }

            // Pass the list items to the BackgroundWorker
            RenameWorker.RunWorkerAsync(renameList);
        }
    }
    #endregion

    #region Internal
    private void LoadFromDirectory()
    {
        var stopwatch = Stopwatch.StartNew();

        RenameStatusLabel.Text = $"Loading {_renameService.SelectedDirectory}";
        RenameProgressBar.Value = 0;

        var fileData = _renameService.LoadDirectory(
            (progress, total, fileName) =>
            {
                if (RenameProgressBar.Value == 0)
                {
                    RenameProgressBar.Maximum = total;
                }

                RenameStatusLabel.Text = $"Loading {fileName}";
                RenameProgressBar.Value = progress;
            });

        // Clear the ListView before adding new items
        LoadedListView.Items.Clear();
        PreviewListView.Items.Clear();

        foreach (var data in fileData)
        {
            var originalItem = new ListViewItem(data.OriginalName)
            {
                Tag = data
            };

            var newItem = new ListViewItem(data.NewName)
            {
                Tag = data
            };

            //Task.Delay(125).Wait(); // Simulate

            LoadedListView.Items.Add(originalItem);
            PreviewListView.Items.Add(newItem);
        }
        stopwatch.Stop();

        RenameStatusLabel.Text = $"Loaded {LoadedListView.Items.Count} files in {stopwatch.ElapsedMilliseconds}ms";
        ButtonRename.Enabled = true;
    }

    private void RemoveSelectedItems()
    {
        if (LoadedListView.SelectedItems.Count > 0)
        {
            foreach (ListViewItem item in LoadedListView.SelectedItems)
            {
                var loadedIndex = LoadedListView.Items.IndexOf(item);

                LoadedListView.Items.Remove(item);
                PreviewListView.Items.RemoveAt(loadedIndex);
            }
        }
    }
    #endregion
}
