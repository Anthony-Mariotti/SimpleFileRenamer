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
        menuStrip1 = new MenuStrip();
        fileToolStripMenuItem = new ToolStripMenuItem();
        saveToolStripMenuItem = new ToolStripMenuItem();
        SettingsMenuItem = new ToolStripMenuItem();
        closeToolStripMenuItem = new ToolStripMenuItem();
        aboutToolStripMenuItem = new ToolStripMenuItem();
        ImportButton = new Button();
        PeopleListView = new ListView();
        menuStrip1.SuspendLayout();
        SuspendLayout();
        // 
        // menuStrip1
        // 
        menuStrip1.ImageScalingSize = new Size(20, 20);
        menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, aboutToolStripMenuItem });
        menuStrip1.Location = new Point(0, 0);
        menuStrip1.Name = "menuStrip1";
        menuStrip1.Size = new Size(582, 28);
        menuStrip1.TabIndex = 0;
        menuStrip1.Text = "menuStrip1";
        // 
        // fileToolStripMenuItem
        // 
        fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { saveToolStripMenuItem, SettingsMenuItem, closeToolStripMenuItem });
        fileToolStripMenuItem.Name = "fileToolStripMenuItem";
        fileToolStripMenuItem.Size = new Size(46, 24);
        fileToolStripMenuItem.Text = "File";
        // 
        // saveToolStripMenuItem
        // 
        saveToolStripMenuItem.Name = "saveToolStripMenuItem";
        saveToolStripMenuItem.Size = new Size(189, 26);
        saveToolStripMenuItem.Text = "Save";
        // 
        // SettingsMenuItem
        // 
        SettingsMenuItem.Name = "SettingsMenuItem";
        SettingsMenuItem.Size = new Size(189, 26);
        SettingsMenuItem.Text = "Settings";
        SettingsMenuItem.Click += SettingsMenuItem_Click;
        // 
        // closeToolStripMenuItem
        // 
        closeToolStripMenuItem.Name = "closeToolStripMenuItem";
        closeToolStripMenuItem.Size = new Size(189, 26);
        closeToolStripMenuItem.Text = "Exit Live Mode";
        // 
        // aboutToolStripMenuItem
        // 
        aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
        aboutToolStripMenuItem.Size = new Size(64, 24);
        aboutToolStripMenuItem.Text = "About";
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
        // LiveMode
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(582, 353);
        Controls.Add(PeopleListView);
        Controls.Add(ImportButton);
        Controls.Add(menuStrip1);
        MainMenuStrip = menuStrip1;
        MinimumSize = new Size(600, 400);
        Name = "LiveMode";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "LiveMode";
        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private MenuStrip menuStrip1;
    private ToolStripMenuItem fileToolStripMenuItem;
    private ToolStripMenuItem saveToolStripMenuItem;
    private ToolStripMenuItem closeToolStripMenuItem;
    private ToolStripMenuItem aboutToolStripMenuItem;
    private Button ImportButton;
    private ListView PeopleListView;
    private ToolStripMenuItem SettingsMenuItem;
}