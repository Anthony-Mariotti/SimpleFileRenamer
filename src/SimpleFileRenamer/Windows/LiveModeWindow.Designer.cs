namespace SimpleFileRenamer;

partial class LiveModeWindow
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
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
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        LiveModeMenuStrip = new MenuStrip();
        FileToolStripMenuItem = new ToolStripMenuItem();
        SettingsMenuItem = new ToolStripMenuItem();
        toolStripSeparator2 = new ToolStripSeparator();
        ExitToolStripMenuItem = new ToolStripMenuItem();
        AboutToolStripMenuItem = new ToolStripMenuItem();
        ImportButton = new Button();
        PeopleListView = new ListView();
        LiveModeMenuStrip.SuspendLayout();
        SuspendLayout();
        // 
        // LiveModeMenuStrip
        // 
        LiveModeMenuStrip.ImageScalingSize = new Size(20, 20);
        LiveModeMenuStrip.Items.AddRange(new ToolStripItem[] { FileToolStripMenuItem, AboutToolStripMenuItem });
        LiveModeMenuStrip.Location = new Point(0, 0);
        LiveModeMenuStrip.Name = "LiveModeMenuStrip";
        LiveModeMenuStrip.Size = new Size(582, 28);
        LiveModeMenuStrip.TabIndex = 0;
        LiveModeMenuStrip.Text = "menuStrip1";
        // 
        // FileToolStripMenuItem
        // 
        FileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { SettingsMenuItem, toolStripSeparator2, ExitToolStripMenuItem });
        FileToolStripMenuItem.Name = "FileToolStripMenuItem";
        FileToolStripMenuItem.Size = new Size(46, 24);
        FileToolStripMenuItem.Text = "File";
        // 
        // SettingsMenuItem
        // 
        SettingsMenuItem.Name = "SettingsMenuItem";
        SettingsMenuItem.Size = new Size(224, 26);
        SettingsMenuItem.Text = "Settings";
        SettingsMenuItem.Click += SettingsMenuItem_Click;
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
        ExitToolStripMenuItem.Text = "Exit Live Mode";
        ExitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;
        // 
        // AboutToolStripMenuItem
        // 
        AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
        AboutToolStripMenuItem.Size = new Size(64, 24);
        AboutToolStripMenuItem.Text = "About";
        // 
        // ImportButton
        // 
        ImportButton.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        ImportButton.Location = new Point(12, 31);
        ImportButton.Name = "ImportButton";
        ImportButton.Size = new Size(558, 29);
        ImportButton.TabIndex = 1;
        ImportButton.Text = "Import from XLSX/CSV";
        ImportButton.UseVisualStyleBackColor = true;
        ImportButton.Click += ImportButton_Click;
        // 
        // PeopleListView
        // 
        PeopleListView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        PeopleListView.Location = new Point(12, 66);
        PeopleListView.Name = "PeopleListView";
        PeopleListView.Size = new Size(558, 275);
        PeopleListView.TabIndex = 2;
        PeopleListView.UseCompatibleStateImageBehavior = false;
        PeopleListView.ItemActivate += PeopleListView_ItemActivate;
        // 
        // LiveModeWindow
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(582, 353);
        Controls.Add(PeopleListView);
        Controls.Add(ImportButton);
        Controls.Add(LiveModeMenuStrip);
        MainMenuStrip = LiveModeMenuStrip;
        MinimumSize = new Size(600, 400);
        Name = "LiveModeWindow";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "(Live Mode) Simple File Renamer";
        LiveModeMenuStrip.ResumeLayout(false);
        LiveModeMenuStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private MenuStrip LiveModeMenuStrip;
    private ToolStripMenuItem FileToolStripMenuItem;
    private ToolStripMenuItem ExitToolStripMenuItem;
    private ToolStripMenuItem AboutToolStripMenuItem;
    private Button ImportButton;
    private ListView PeopleListView;
    private ToolStripMenuItem SettingsMenuItem;
    private ToolStripSeparator toolStripSeparator2;
}