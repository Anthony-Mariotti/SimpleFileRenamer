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
        closeFolderToolStripMenuItem = new ToolStripMenuItem();
        toolStripSeparator1 = new ToolStripSeparator();
        SettingsToolStripMenuItem = new ToolStripMenuItem();
        toolStripSeparator2 = new ToolStripSeparator();
        ExitToolStripMenuItem = new ToolStripMenuItem();
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
        LoadedListViewContextMenu = new ContextMenuStrip(components);
        RemoveLoadedFileMenuItem = new ToolStripMenuItem();
        PreviewListView = new SyncedListView();
        ButtonRename = new Button();
        RenameWorker = new System.ComponentModel.BackgroundWorker();
        UndoWorker = new System.ComponentModel.BackgroundWorker();
        HeaderMenuStrip.SuspendLayout();
        FooterStatusStrip.SuspendLayout();
        LoadedListViewContextMenu.SuspendLayout();
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
        FileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { OpenFolderToolStripMenuItem, closeFolderToolStripMenuItem, toolStripSeparator1, SettingsToolStripMenuItem, toolStripSeparator2, ExitToolStripMenuItem });
        FileToolStripMenuItem.Name = "FileToolStripMenuItem";
        FileToolStripMenuItem.Size = new Size(46, 24);
        FileToolStripMenuItem.Text = "&File";
        // 
        // OpenFolderToolStripMenuItem
        // 
        OpenFolderToolStripMenuItem.Name = "OpenFolderToolStripMenuItem";
        OpenFolderToolStripMenuItem.Size = new Size(224, 26);
        OpenFolderToolStripMenuItem.Text = "&Open Folder...";
        OpenFolderToolStripMenuItem.Click += OpenFolderToolStripMenuItem_Click;
        // 
        // closeFolderToolStripMenuItem
        // 
        closeFolderToolStripMenuItem.Enabled = false;
        closeFolderToolStripMenuItem.Name = "closeFolderToolStripMenuItem";
        closeFolderToolStripMenuItem.Size = new Size(224, 26);
        closeFolderToolStripMenuItem.Text = "&Close Folder";
        // 
        // toolStripSeparator1
        // 
        toolStripSeparator1.Name = "toolStripSeparator1";
        toolStripSeparator1.Size = new Size(221, 6);
        // 
        // SettingsToolStripMenuItem
        // 
        SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem";
        SettingsToolStripMenuItem.Size = new Size(224, 26);
        SettingsToolStripMenuItem.Text = "&Settings";
        SettingsToolStripMenuItem.Click += SettingsToolStripMenuItem_Click;
        // 
        // toolStripSeparator2
        // 
        toolStripSeparator2.Name = "toolStripSeparator2";
        toolStripSeparator2.Size = new Size(221, 6);
        // 
        // ExitToolStripMenuItem
        // 
        ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
        ExitToolStripMenuItem.Size = new Size(224, 26);
        ExitToolStripMenuItem.Text = "&Exit";
        ExitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;
        // 
        // EditToolStripMenuItem
        // 
        EditToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { UndoToolStripMenuItem, RedoToolStripMenuItem });
        EditToolStripMenuItem.Name = "EditToolStripMenuItem";
        EditToolStripMenuItem.Size = new Size(49, 24);
        EditToolStripMenuItem.Text = "&Edit";
        // 
        // UndoToolStripMenuItem
        // 
        UndoToolStripMenuItem.Enabled = false;
        UndoToolStripMenuItem.Name = "UndoToolStripMenuItem";
        UndoToolStripMenuItem.Size = new Size(128, 26);
        UndoToolStripMenuItem.Text = "&Undo";
        UndoToolStripMenuItem.Click += UndoToolStripMenuItem_Click;
        // 
        // RedoToolStripMenuItem
        // 
        RedoToolStripMenuItem.Enabled = false;
        RedoToolStripMenuItem.Name = "RedoToolStripMenuItem";
        RedoToolStripMenuItem.Size = new Size(128, 26);
        RedoToolStripMenuItem.Text = "&Redo";
        RedoToolStripMenuItem.Click += RedoToolStripMenuItem_Click;
        // 
        // ToolsToolStripMenuItem
        // 
        ToolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { LiveModeToolStripMenuItem });
        ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem";
        ToolsToolStripMenuItem.Size = new Size(58, 24);
        ToolsToolStripMenuItem.Text = "&Tools";
        // 
        // LiveModeToolStripMenuItem
        // 
        LiveModeToolStripMenuItem.Name = "LiveModeToolStripMenuItem";
        LiveModeToolStripMenuItem.Size = new Size(161, 26);
        LiveModeToolStripMenuItem.Text = "&Live Mode";
        LiveModeToolStripMenuItem.Click += LiveModeToolStripMenuItem_Click;
        // 
        // AboutToolStripMenuItem
        // 
        AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
        AboutToolStripMenuItem.Size = new Size(64, 24);
        AboutToolStripMenuItem.Text = "&About";
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
        LoadedListView.ContextMenuStrip = LoadedListViewContextMenu;
        LoadedListView.Location = new Point(12, 31);
        LoadedListView.Name = "LoadedListView";
        LoadedListView.Partner = null;
        LoadedListView.Size = new Size(450, 600);
        LoadedListView.TabIndex = 2;
        LoadedListView.UseCompatibleStateImageBehavior = false;
        LoadedListView.SelectedIndexChanged += LoadedListView_SelectedIndexChanged;
        LoadedListView.KeyUp += LoadedListView_KeyUp;
        // 
        // LoadedListViewContextMenu
        // 
        LoadedListViewContextMenu.ImageScalingSize = new Size(20, 20);
        LoadedListViewContextMenu.Items.AddRange(new ToolStripItem[] { RemoveLoadedFileMenuItem });
        LoadedListViewContextMenu.Name = "loadedFileContextMenu";
        LoadedListViewContextMenu.Size = new Size(211, 56);
        LoadedListViewContextMenu.Opening += LoadedListViewContextMenu_Opening;
        // 
        // RemoveLoadedFileMenuItem
        // 
        RemoveLoadedFileMenuItem.Name = "RemoveLoadedFileMenuItem";
        RemoveLoadedFileMenuItem.Size = new Size(210, 24);
        RemoveLoadedFileMenuItem.Text = "Remove";
        RemoveLoadedFileMenuItem.Click += RemoveLoadedFileMenuItem_Click;
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
        ButtonRename.Click += ButtonRename_Click;
        // 
        // RenameWorker
        // 
        RenameWorker.WorkerReportsProgress = true;
        RenameWorker.DoWork += RenameWorker_DoWork;
        RenameWorker.ProgressChanged += RenameWorker_ProgressChanged;
        RenameWorker.RunWorkerCompleted += RenameWorker_RunWorkerCompleted;
        // 
        // UndoWorker
        // 
        UndoWorker.WorkerReportsProgress = true;
        UndoWorker.DoWork += UndoWorker_DoWork;
        UndoWorker.ProgressChanged += UndoWorker_ProgressChanged;
        UndoWorker.RunWorkerCompleted += UndoWorker_RunWorkerCompleted;
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
        LoadedListViewContextMenu.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private MenuStrip HeaderMenuStrip;
    private ToolStripMenuItem FileToolStripMenuItem;
    private ToolStripMenuItem OpenFolderToolStripMenuItem;
    private ToolStripMenuItem ExitToolStripMenuItem;
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
    private ContextMenuStrip LoadedListViewContextMenu;
    private ToolStripMenuItem RemoveLoadedFileMenuItem;
    private ToolStripMenuItem ToolsToolStripMenuItem;
    private ToolStripMenuItem LiveModeToolStripMenuItem;
    private ToolStripMenuItem SettingsToolStripMenuItem;
    private ToolStripMenuItem RedoToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator1;
    private ToolStripMenuItem closeFolderToolStripMenuItem;
    private ToolStripSeparator toolStripSeparator2;
}
