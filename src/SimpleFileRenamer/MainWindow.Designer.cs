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
        mainMenuStrip = new MenuStrip();
        fileToolStripMenuItem = new ToolStripMenuItem();
        openFolderToolStripMenuItem = new ToolStripMenuItem();
        formatToolStripMenuItem = new ToolStripMenuItem();
        closeToolStripMenuItem = new ToolStripMenuItem();
        editToolStripMenuItem = new ToolStripMenuItem();
        undoToolStripMenuItem = new ToolStripMenuItem();
        redoToolStripMenuItem = new ToolStripMenuItem();
        toolsToolStripMenuItem = new ToolStripMenuItem();
        liveModeToolStripMenuItem = new ToolStripMenuItem();
        aboutToolStripMenuItem = new ToolStripMenuItem();
        mainStatusStrip = new StatusStrip();
        renameProgressBar = new ToolStripProgressBar();
        renameStatusLabel = new ToolStripStatusLabel();
        loadedListView = new SyncedListView();
        loadedFileContextMenu = new ContextMenuStrip(components);
        removeLoadedFileMenuItem = new ToolStripMenuItem();
        previewListView = new SyncedListView();
        buttonRename = new Button();
        renameWorker = new System.ComponentModel.BackgroundWorker();
        undoWorker = new System.ComponentModel.BackgroundWorker();
        mainMenuStrip.SuspendLayout();
        mainStatusStrip.SuspendLayout();
        loadedFileContextMenu.SuspendLayout();
        SuspendLayout();
        // 
        // mainMenuStrip
        // 
        mainMenuStrip.ImageScalingSize = new Size(20, 20);
        mainMenuStrip.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, editToolStripMenuItem, toolsToolStripMenuItem, aboutToolStripMenuItem });
        mainMenuStrip.Location = new Point(0, 0);
        mainMenuStrip.Name = "mainMenuStrip";
        mainMenuStrip.Size = new Size(1069, 28);
        mainMenuStrip.TabIndex = 0;
        mainMenuStrip.Text = "menuStrip1";
        // 
        // fileToolStripMenuItem
        // 
        fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openFolderToolStripMenuItem, formatToolStripMenuItem, closeToolStripMenuItem });
        fileToolStripMenuItem.Name = "fileToolStripMenuItem";
        fileToolStripMenuItem.Size = new Size(46, 24);
        fileToolStripMenuItem.Text = "File";
        // 
        // openFolderToolStripMenuItem
        // 
        openFolderToolStripMenuItem.Name = "openFolderToolStripMenuItem";
        openFolderToolStripMenuItem.Size = new Size(224, 26);
        openFolderToolStripMenuItem.Text = "Open Folder";
        openFolderToolStripMenuItem.Click += openFolderToolStripMenuItem_Click;
        // 
        // formatToolStripMenuItem
        // 
        formatToolStripMenuItem.Name = "formatToolStripMenuItem";
        formatToolStripMenuItem.Size = new Size(224, 26);
        formatToolStripMenuItem.Text = "Settings";
        formatToolStripMenuItem.Click += formatToolStripMenuItem_Click;
        // 
        // closeToolStripMenuItem
        // 
        closeToolStripMenuItem.Name = "closeToolStripMenuItem";
        closeToolStripMenuItem.Size = new Size(224, 26);
        closeToolStripMenuItem.Text = "Close";
        closeToolStripMenuItem.Click += closeToolStripMenuItem_Click;
        // 
        // editToolStripMenuItem
        // 
        editToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { undoToolStripMenuItem, redoToolStripMenuItem });
        editToolStripMenuItem.Name = "editToolStripMenuItem";
        editToolStripMenuItem.Size = new Size(49, 24);
        editToolStripMenuItem.Text = "Edit";
        // 
        // undoToolStripMenuItem
        // 
        undoToolStripMenuItem.Enabled = false;
        undoToolStripMenuItem.Name = "undoToolStripMenuItem";
        undoToolStripMenuItem.Size = new Size(224, 26);
        undoToolStripMenuItem.Text = "Undo";
        undoToolStripMenuItem.Click += undoToolStripMenuItem_Click;
        // 
        // redoToolStripMenuItem
        // 
        redoToolStripMenuItem.Enabled = false;
        redoToolStripMenuItem.Name = "redoToolStripMenuItem";
        redoToolStripMenuItem.Size = new Size(224, 26);
        redoToolStripMenuItem.Text = "Redo";
        // 
        // toolsToolStripMenuItem
        // 
        toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { liveModeToolStripMenuItem });
        toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
        toolsToolStripMenuItem.Size = new Size(58, 24);
        toolsToolStripMenuItem.Text = "Tools";
        // 
        // liveModeToolStripMenuItem
        // 
        liveModeToolStripMenuItem.Name = "liveModeToolStripMenuItem";
        liveModeToolStripMenuItem.Size = new Size(224, 26);
        liveModeToolStripMenuItem.Text = "Live Mode";
        liveModeToolStripMenuItem.Click += liveModeToolStripMenuItem_Click;
        // 
        // aboutToolStripMenuItem
        // 
        aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
        aboutToolStripMenuItem.Size = new Size(64, 24);
        aboutToolStripMenuItem.Text = "About";
        // 
        // mainStatusStrip
        // 
        mainStatusStrip.ImageScalingSize = new Size(20, 20);
        mainStatusStrip.Items.AddRange(new ToolStripItem[] { renameProgressBar, renameStatusLabel });
        mainStatusStrip.Location = new Point(0, 634);
        mainStatusStrip.Name = "mainStatusStrip";
        mainStatusStrip.Size = new Size(1069, 26);
        mainStatusStrip.TabIndex = 1;
        mainStatusStrip.Text = "statusStrip1";
        // 
        // renameProgressBar
        // 
        renameProgressBar.Name = "renameProgressBar";
        renameProgressBar.Size = new Size(100, 18);
        // 
        // renameStatusLabel
        // 
        renameStatusLabel.Name = "renameStatusLabel";
        renameStatusLabel.Size = new Size(34, 20);
        renameStatusLabel.Text = "Idle";
        // 
        // loadedListView
        // 
        loadedListView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        loadedListView.ContextMenuStrip = loadedFileContextMenu;
        loadedListView.Location = new Point(12, 31);
        loadedListView.Name = "loadedListView";
        loadedListView.Partner = null;
        loadedListView.Size = new Size(450, 600);
        loadedListView.TabIndex = 2;
        loadedListView.UseCompatibleStateImageBehavior = false;
        loadedListView.SelectedIndexChanged += loadedListView_SelectedIndexChanged;
        // 
        // loadedFileContextMenu
        // 
        loadedFileContextMenu.ImageScalingSize = new Size(20, 20);
        loadedFileContextMenu.Items.AddRange(new ToolStripItem[] { removeLoadedFileMenuItem });
        loadedFileContextMenu.Name = "loadedFileContextMenu";
        loadedFileContextMenu.Size = new Size(133, 28);
        loadedFileContextMenu.Opening += loadedFileContextMenu_Opening;
        // 
        // removeLoadedFileMenuItem
        // 
        removeLoadedFileMenuItem.Name = "removeLoadedFileMenuItem";
        removeLoadedFileMenuItem.Size = new Size(132, 24);
        removeLoadedFileMenuItem.Text = "Remove";
        removeLoadedFileMenuItem.Click += removeLoadedFileMenuItem_Click;
        // 
        // previewListView
        // 
        previewListView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
        previewListView.Location = new Point(607, 31);
        previewListView.Name = "previewListView";
        previewListView.Partner = null;
        previewListView.Size = new Size(450, 600);
        previewListView.TabIndex = 3;
        previewListView.UseCompatibleStateImageBehavior = false;
        // 
        // buttonRename
        // 
        buttonRename.Enabled = false;
        buttonRename.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
        buttonRename.Location = new Point(477, 305);
        buttonRename.Name = "buttonRename";
        buttonRename.Padding = new Padding(10, 0, 0, 0);
        buttonRename.Size = new Size(116, 59);
        buttonRename.TabIndex = 4;
        buttonRename.Text = "►";
        buttonRename.UseVisualStyleBackColor = true;
        buttonRename.Click += buttonRename_Click;
        // 
        // renameWorker
        // 
        renameWorker.WorkerReportsProgress = true;
        renameWorker.DoWork += renameWorker_DoWork;
        renameWorker.ProgressChanged += renameWorker_ProgressChanged;
        renameWorker.RunWorkerCompleted += renameWorker_RunWorkerCompleted;
        // 
        // undoWorker
        // 
        undoWorker.WorkerReportsProgress = true;
        undoWorker.DoWork += undoWorker_DoWork;
        undoWorker.ProgressChanged += undoWorker_ProgressChanged;
        undoWorker.RunWorkerCompleted += undoWorker_RunWorkerCompleted;
        // 
        // MainWindow
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1069, 660);
        Controls.Add(buttonRename);
        Controls.Add(previewListView);
        Controls.Add(loadedListView);
        Controls.Add(mainStatusStrip);
        Controls.Add(mainMenuStrip);
        MainMenuStrip = mainMenuStrip;
        MaximizeBox = false;
        Name = "MainWindow";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Simple File Renamer";
        mainMenuStrip.ResumeLayout(false);
        mainMenuStrip.PerformLayout();
        mainStatusStrip.ResumeLayout(false);
        mainStatusStrip.PerformLayout();
        loadedFileContextMenu.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private MenuStrip mainMenuStrip;
    private ToolStripMenuItem fileToolStripMenuItem;
    private ToolStripMenuItem openFolderToolStripMenuItem;
    private ToolStripMenuItem closeToolStripMenuItem;
    private ToolStripMenuItem aboutToolStripMenuItem;
    private StatusStrip mainStatusStrip;
    private ToolStripProgressBar renameProgressBar;
    private ToolStripStatusLabel renameStatusLabel;
    private SyncedListView loadedListView;
    private SyncedListView previewListView;
    private Button buttonRename;
    private ToolStripMenuItem editToolStripMenuItem;
    private ToolStripMenuItem undoToolStripMenuItem;
    private System.ComponentModel.BackgroundWorker renameWorker;
    private System.ComponentModel.BackgroundWorker undoWorker;
    private ContextMenuStrip loadedFileContextMenu;
    private ToolStripMenuItem removeLoadedFileMenuItem;
    private ToolStripMenuItem toolsToolStripMenuItem;
    private ToolStripMenuItem liveModeToolStripMenuItem;
    private ToolStripMenuItem formatToolStripMenuItem;
    private ToolStripMenuItem redoToolStripMenuItem;
}
