using SimpleFileRenamer.Controls;

namespace SimpleFileRenamer;

partial class MainWindow
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        HeaderMenuStrip = new MenuStrip();
        FileToolStripMenuItem = new ToolStripMenuItem();
        OpenFolderToolStripMenuItem = new ToolStripMenuItem();
        SettingsToolStripMenuItem = new ToolStripMenuItem();
        CloseToolStripMenuItem = new ToolStripMenuItem();
        EditToolStripMenuItem = new ToolStripMenuItem();
        UndoToolStripMenuItem = new ToolStripMenuItem();
        RedoToolStripMenuItem = new ToolStripMenuItem();
        ToolsToolStripMenuItem = new ToolStripMenuItem();
        LiveModeToolStripMenuItem = new ToolStripMenuItem();
        AboutToolStripMenuItem = new ToolStripMenuItem();
        FooterStatusStrip = new StatusStrip();
        RenameProgressBar = new ToolStripProgressBar();
        RenameStatusLabel = new ToolStripStatusLabel();
        LoadedListView = new SyncedListView();
        LoadedFileContextMenu = new ContextMenuStrip(components);
        RemoveLoadedFileMenuItem = new ToolStripMenuItem();
        PreviewListView = new SyncedListView();
        ButtonRename = new Button();
        RenameWorker = new System.ComponentModel.BackgroundWorker();
        UndoWorker = new System.ComponentModel.BackgroundWorker();
        HeaderMenuStrip.SuspendLayout();
        FooterStatusStrip.SuspendLayout();
        LoadedFileContextMenu.SuspendLayout();
        SuspendLayout();
        // 
        // HeaderMenuStrip
        // 
        HeaderMenuStrip.ImageScalingSize = new Size(20, 20);
        HeaderMenuStrip.Items.AddRange(new ToolStripItem[] { FileToolStripMenuItem, EditToolStripMenuItem, ToolsToolStripMenuItem, AboutToolStripMenuItem });
        HeaderMenuStrip.Location = new Point(0, 0);
        HeaderMenuStrip.Name = "HeaderMenuStrip";
        HeaderMenuStrip.Size = new Size(1069, 28);
        HeaderMenuStrip.TabIndex = 0;
        HeaderMenuStrip.Text = "menuStrip1";
        // 
        // FileToolStripMenuItem
        // 
        FileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { OpenFolderToolStripMenuItem, SettingsToolStripMenuItem, CloseToolStripMenuItem });
        FileToolStripMenuItem.Name = "FileToolStripMenuItem";
        FileToolStripMenuItem.Size = new Size(46, 24);
        FileToolStripMenuItem.Text = "File";
        // 
        // OpenFolderToolStripMenuItem
        // 
        OpenFolderToolStripMenuItem.Name = "OpenFolderToolStripMenuItem";
        OpenFolderToolStripMenuItem.Size = new Size(174, 26);
        OpenFolderToolStripMenuItem.Text = "Open Folder";
        OpenFolderToolStripMenuItem.Click += openFolderToolStripMenuItem_Click;
        // 
        // SettingsToolStripMenuItem
        // 
        SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem";
        SettingsToolStripMenuItem.Size = new Size(174, 26);
        SettingsToolStripMenuItem.Text = "Settings";
        SettingsToolStripMenuItem.Click += formatToolStripMenuItem_Click;
        // 
        // CloseToolStripMenuItem
        // 
        CloseToolStripMenuItem.Name = "CloseToolStripMenuItem";
        CloseToolStripMenuItem.Size = new Size(174, 26);
        CloseToolStripMenuItem.Text = "Close";
        CloseToolStripMenuItem.Click += closeToolStripMenuItem_Click;
        // 
        // EditToolStripMenuItem
        // 
        EditToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { UndoToolStripMenuItem, RedoToolStripMenuItem });
        EditToolStripMenuItem.Name = "EditToolStripMenuItem";
        EditToolStripMenuItem.Size = new Size(49, 24);
        EditToolStripMenuItem.Text = "Edit";
        // 
        // UndoToolStripMenuItem
        // 
        UndoToolStripMenuItem.Enabled = false;
        UndoToolStripMenuItem.Name = "UndoToolStripMenuItem";
        UndoToolStripMenuItem.Size = new Size(128, 26);
        UndoToolStripMenuItem.Text = "Undo";
        UndoToolStripMenuItem.Click += undoToolStripMenuItem_Click;
        // 
        // RedoToolStripMenuItem
        // 
        RedoToolStripMenuItem.Enabled = false;
        RedoToolStripMenuItem.Name = "RedoToolStripMenuItem";
        RedoToolStripMenuItem.Size = new Size(128, 26);
        RedoToolStripMenuItem.Text = "Redo";
        // 
        // ToolsToolStripMenuItem
        // 
        ToolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { LiveModeToolStripMenuItem });
        ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem";
        ToolsToolStripMenuItem.Size = new Size(58, 24);
        ToolsToolStripMenuItem.Text = "Tools";
        // 
        // LiveModeToolStripMenuItem
        // 
        LiveModeToolStripMenuItem.Name = "LiveModeToolStripMenuItem";
        LiveModeToolStripMenuItem.Size = new Size(161, 26);
        LiveModeToolStripMenuItem.Text = "Live Mode";
        LiveModeToolStripMenuItem.Click += liveModeToolStripMenuItem_Click;
        // 
        // AboutToolStripMenuItem
        // 
        AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
        AboutToolStripMenuItem.Size = new Size(64, 24);
        AboutToolStripMenuItem.Text = "About";
        // 
        // FooterStatusStrip
        // 
        FooterStatusStrip.ImageScalingSize = new Size(20, 20);
        FooterStatusStrip.Items.AddRange(new ToolStripItem[] { RenameProgressBar, RenameStatusLabel });
        FooterStatusStrip.Location = new Point(0, 634);
        FooterStatusStrip.Name = "FooterStatusStrip";
        FooterStatusStrip.Size = new Size(1069, 26);
        FooterStatusStrip.TabIndex = 1;
        FooterStatusStrip.Text = "statusStrip1";
        // 
        // RenameProgressBar
        // 
        RenameProgressBar.Name = "RenameProgressBar";
        RenameProgressBar.Size = new Size(100, 18);
        // 
        // RenameStatusLabel
        // 
        RenameStatusLabel.Name = "RenameStatusLabel";
        RenameStatusLabel.Size = new Size(34, 20);
        RenameStatusLabel.Text = "Idle";
        // 
        // LoadedListView
        // 
        LoadedListView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        LoadedListView.ContextMenuStrip = LoadedFileContextMenu;
        LoadedListView.Location = new Point(12, 31);
        LoadedListView.Name = "LoadedListView";
        LoadedListView.Partner = null;
        LoadedListView.Size = new Size(450, 600);
        LoadedListView.TabIndex = 2;
        LoadedListView.UseCompatibleStateImageBehavior = false;
        LoadedListView.SelectedIndexChanged += loadedListView_SelectedIndexChanged;
        // 
        // LoadedFileContextMenu
        // 
        LoadedFileContextMenu.ImageScalingSize = new Size(20, 20);
        LoadedFileContextMenu.Items.AddRange(new ToolStripItem[] { RemoveLoadedFileMenuItem });
        LoadedFileContextMenu.Name = "loadedFileContextMenu";
        LoadedFileContextMenu.Size = new Size(133, 28);
        LoadedFileContextMenu.Opening += loadedFileContextMenu_Opening;
        // 
        // RemoveLoadedFileMenuItem
        // 
        RemoveLoadedFileMenuItem.Name = "RemoveLoadedFileMenuItem";
        RemoveLoadedFileMenuItem.Size = new Size(132, 24);
        RemoveLoadedFileMenuItem.Text = "Remove";
        RemoveLoadedFileMenuItem.Click += removeLoadedFileMenuItem_Click;
        // 
        // PreviewListView
        // 
        PreviewListView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
        PreviewListView.Location = new Point(607, 31);
        PreviewListView.Name = "PreviewListView";
        PreviewListView.Partner = null;
        PreviewListView.Size = new Size(450, 600);
        PreviewListView.TabIndex = 3;
        PreviewListView.UseCompatibleStateImageBehavior = false;
        // 
        // ButtonRename
        // 
        ButtonRename.Enabled = false;
        ButtonRename.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
        ButtonRename.Location = new Point(477, 305);
        ButtonRename.Name = "ButtonRename";
        ButtonRename.Padding = new Padding(10, 0, 0, 0);
        ButtonRename.Size = new Size(116, 59);
        ButtonRename.TabIndex = 4;
        ButtonRename.Text = "►";
        ButtonRename.UseVisualStyleBackColor = true;
        ButtonRename.Click += buttonRename_Click;
        // 
        // RenameWorker
        // 
        RenameWorker.WorkerReportsProgress = true;
        RenameWorker.DoWork += renameWorker_DoWork;
        RenameWorker.ProgressChanged += renameWorker_ProgressChanged;
        RenameWorker.RunWorkerCompleted += renameWorker_RunWorkerCompleted;
        // 
        // UndoWorker
        // 
        UndoWorker.WorkerReportsProgress = true;
        UndoWorker.DoWork += undoWorker_DoWork;
        UndoWorker.ProgressChanged += undoWorker_ProgressChanged;
        UndoWorker.RunWorkerCompleted += undoWorker_RunWorkerCompleted;
        // 
        // MainWindow
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1069, 660);
        Controls.Add(ButtonRename);
        Controls.Add(PreviewListView);
        Controls.Add(LoadedListView);
        Controls.Add(FooterStatusStrip);
        Controls.Add(HeaderMenuStrip);
        MainMenuStrip = HeaderMenuStrip;
        MaximizeBox = false;
        Name = "MainWindow";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Simple File Renamer";
        FormClosing += MainWindow_FormClosing;
        HeaderMenuStrip.ResumeLayout(false);
        HeaderMenuStrip.PerformLayout();
        FooterStatusStrip.ResumeLayout(false);
        FooterStatusStrip.PerformLayout();
        LoadedFileContextMenu.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private MenuStrip HeaderMenuStrip;
    private ToolStripMenuItem FileToolStripMenuItem;
    private ToolStripMenuItem OpenFolderToolStripMenuItem;
    private ToolStripMenuItem CloseToolStripMenuItem;
    private ToolStripMenuItem AboutToolStripMenuItem;
    private StatusStrip FooterStatusStrip;
    private ToolStripProgressBar RenameProgressBar;
    private ToolStripStatusLabel RenameStatusLabel;
    private SyncedListView LoadedListView;
    private SyncedListView PreviewListView;
    private Button ButtonRename;
    private ToolStripMenuItem EditToolStripMenuItem;
    private ToolStripMenuItem UndoToolStripMenuItem;
    private System.ComponentModel.BackgroundWorker RenameWorker;
    private System.ComponentModel.BackgroundWorker UndoWorker;
    private ContextMenuStrip LoadedFileContextMenu;
    private ToolStripMenuItem RemoveLoadedFileMenuItem;
    private ToolStripMenuItem ToolsToolStripMenuItem;
    private ToolStripMenuItem LiveModeToolStripMenuItem;
    private ToolStripMenuItem SettingsToolStripMenuItem;
    private ToolStripMenuItem RedoToolStripMenuItem;
}
