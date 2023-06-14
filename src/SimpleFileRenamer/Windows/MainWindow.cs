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

    private readonly Stack<List<RenameData>> _undoActions = new Stack<List<RenameData>>();
    private readonly Stack<List<RenameData>> _redoActions = new Stack<List<RenameData>>();

    public MainWindow(IWindowFactory windowFactory, IRenameService renameService)
    {
        Log.Verbose("Initializing Main Window");
        _windowFactory = windowFactory;
        _renameService = renameService;

        _renameService.RenameProgressChanged += RenameWorker_ProgressChanged;
        _renameService.RenameCompleted += RenameWorker_RunWorkerCompleted;

        _renameService.UndoProgressChanged += UndoWorker_ProgressChanged;
        _renameService.UndoCompleted += UndoWorker_RunWorkerCompleted;

        _renameService.RedoProgressChanged += RedoWorker_ProgressChanged;
        _renameService.RedoCompleted += RedoWorker_RunWorkerCompleted;

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
        RenameStatusLabel.Text = string.Empty;
        RenameProgressBar.Value = 0;

        _renameService.UndoRename();

        RedoToolStripMenuItem.Enabled = _renameService.CanRedo;
        UndoToolStripMenuItem.Enabled = _renameService.CanUndo;
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
        RenameStatusLabel.Text = string.Empty;
        RenameProgressBar.Value = 0;

        _renameService.RedoRename();

        RedoToolStripMenuItem.Enabled = _renameService.CanRedo;
        UndoToolStripMenuItem.Enabled = _renameService.CanUndo;
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
    private void RenameWorker_ProgressChanged(int progress, string status)
    {
        RenameProgressBar.Value = progress;
        RenameStatusLabel.Text = status;
    }

    private void RenameWorker_RunWorkerCompleted(List<RenameData>? renameData)
    {
        _undoActions.Push(renameData!);
        ButtonRename.Enabled = false;

        UndoToolStripMenuItem.Enabled = _renameService.CanUndo;
        RedoToolStripMenuItem.Enabled = _renameService.CanRedo;

        LoadedListView.Items.Clear();
        PreviewListView.Items.Clear();
    }
    #endregion

    #region UndoWorker
    private void UndoWorker_ProgressChanged(int progress, string status)
    {
        RenameProgressBar.Value = progress;
        RenameStatusLabel.Text = status;
    }

    private void UndoWorker_RunWorkerCompleted()
    {
        ButtonRename.Enabled = false;
    }
    #endregion

    #region RedoWorker
    private void RedoWorker_ProgressChanged(int progress, string status)
    {
        RenameProgressBar.Value = progress;
        RenameStatusLabel.Text = status;
    }

    private void RedoWorker_RunWorkerCompleted()
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
        RenameStatusLabel.Text = string.Empty;
        RenameProgressBar.Maximum = LoadedListView.Items.Count;
        RenameProgressBar.Value = 0;

        _renameService.StartRenaming();
    }
    #endregion

    #region Internal
    private void LoadFromDirectory()
    {
        var stopwatch = Stopwatch.StartNew();

        RenameStatusLabel.Text = $"Loading {_renameService.SelectedDirectory}";
        RenameProgressBar.Value = 0;

        var loadSuccess = _renameService.LoadDirectory(
            (progress, total, fileName) =>
            {
                if (RenameProgressBar.Value == 0)
                {
                    RenameProgressBar.Maximum = total;
                }

                RenameStatusLabel.Text = $"Loading {fileName}";
                RenameProgressBar.Value = progress;
            });

        if (loadSuccess)
        {
            // Clear the ListView before adding new items
            LoadedListView.Items.Clear();
            PreviewListView.Items.Clear();

            foreach (var data in _renameService.RenameData)
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
