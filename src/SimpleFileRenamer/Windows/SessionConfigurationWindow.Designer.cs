namespace SimpleFileRenamer;

partial class SessionConfigurationWindow
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
        label1 = new Label();
        label2 = new Label();
        WatchedFolderTextBox = new TextBox();
        DestinationFolderTextBox = new TextBox();
        BrowseWatchedFolderButton = new Button();
        BrowseDestinationFolderButton = new Button();
        CancelEditButton = new Button();
        SaveButton = new Button();
        AvailableExtensionsListBox = new ListBox();
        label3 = new Label();
        SelectedExtensionsListBox = new ListBox();
        SelectExtensionButton = new Button();
        RemoveExtensionButton = new Button();
        label4 = new Label();
        DeleteCacheButton = new Button();
        SuspendLayout();
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(12, 9);
        label1.Name = "label1";
        label1.Size = new Size(113, 20);
        label1.TabIndex = 0;
        label1.Text = "Watched Folder";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new Point(12, 73);
        label2.Name = "label2";
        label2.Size = new Size(131, 20);
        label2.TabIndex = 1;
        label2.Text = "Destination Folder";
        // 
        // WatchedFolderTextBox
        // 
        WatchedFolderTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        WatchedFolderTextBox.Location = new Point(12, 32);
        WatchedFolderTextBox.Name = "WatchedFolderTextBox";
        WatchedFolderTextBox.Size = new Size(303, 27);
        WatchedFolderTextBox.TabIndex = 2;
        // 
        // DestinationFolderTextBox
        // 
        DestinationFolderTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        DestinationFolderTextBox.Location = new Point(12, 96);
        DestinationFolderTextBox.Name = "DestinationFolderTextBox";
        DestinationFolderTextBox.Size = new Size(303, 27);
        DestinationFolderTextBox.TabIndex = 3;
        // 
        // BrowseWatchedFolderButton
        // 
        BrowseWatchedFolderButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        BrowseWatchedFolderButton.Location = new Point(321, 31);
        BrowseWatchedFolderButton.Name = "BrowseWatchedFolderButton";
        BrowseWatchedFolderButton.Size = new Size(94, 29);
        BrowseWatchedFolderButton.TabIndex = 4;
        BrowseWatchedFolderButton.Text = "Browse";
        BrowseWatchedFolderButton.UseVisualStyleBackColor = true;
        BrowseWatchedFolderButton.Click += BrowseWatchedFolderButton_Click;
        // 
        // BrowseDestinationFolderButton
        // 
        BrowseDestinationFolderButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        BrowseDestinationFolderButton.Location = new Point(321, 95);
        BrowseDestinationFolderButton.Name = "BrowseDestinationFolderButton";
        BrowseDestinationFolderButton.Size = new Size(94, 29);
        BrowseDestinationFolderButton.TabIndex = 5;
        BrowseDestinationFolderButton.Text = "Browse";
        BrowseDestinationFolderButton.UseVisualStyleBackColor = true;
        BrowseDestinationFolderButton.Click += BrowseDestinationFolderButton_Click;
        // 
        // CancelEditButton
        // 
        CancelEditButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        CancelEditButton.Location = new Point(12, 396);
        CancelEditButton.Name = "CancelEditButton";
        CancelEditButton.Size = new Size(94, 29);
        CancelEditButton.TabIndex = 6;
        CancelEditButton.Text = "Cancel";
        CancelEditButton.UseVisualStyleBackColor = true;
        CancelEditButton.Click += CancelButton_Click;
        // 
        // SaveButton
        // 
        SaveButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        SaveButton.Location = new Point(112, 396);
        SaveButton.Name = "SaveButton";
        SaveButton.Size = new Size(94, 29);
        SaveButton.TabIndex = 7;
        SaveButton.Text = "Save";
        SaveButton.UseVisualStyleBackColor = true;
        SaveButton.Click += SaveButton_Click;
        // 
        // AvailableExtensionsListBox
        // 
        AvailableExtensionsListBox.FormattingEnabled = true;
        AvailableExtensionsListBox.ItemHeight = 20;
        AvailableExtensionsListBox.Location = new Point(12, 158);
        AvailableExtensionsListBox.Name = "AvailableExtensionsListBox";
        AvailableExtensionsListBox.Size = new Size(176, 184);
        AvailableExtensionsListBox.TabIndex = 8;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Location = new Point(12, 135);
        label3.Name = "label3";
        label3.Size = new Size(144, 20);
        label3.TabIndex = 9;
        label3.Text = "Available Extensions";
        // 
        // SelectedExtensionsListBox
        // 
        SelectedExtensionsListBox.FormattingEnabled = true;
        SelectedExtensionsListBox.ItemHeight = 20;
        SelectedExtensionsListBox.Location = new Point(242, 158);
        SelectedExtensionsListBox.Name = "SelectedExtensionsListBox";
        SelectedExtensionsListBox.Size = new Size(173, 184);
        SelectedExtensionsListBox.TabIndex = 10;
        // 
        // SelectExtensionButton
        // 
        SelectExtensionButton.Location = new Point(194, 215);
        SelectExtensionButton.Name = "SelectExtensionButton";
        SelectExtensionButton.Size = new Size(42, 29);
        SelectExtensionButton.TabIndex = 11;
        SelectExtensionButton.Text = ">";
        SelectExtensionButton.UseVisualStyleBackColor = true;
        SelectExtensionButton.Click += SelectExtensionButton_Click;
        // 
        // RemoveExtensionButton
        // 
        RemoveExtensionButton.Location = new Point(194, 250);
        RemoveExtensionButton.Name = "RemoveExtensionButton";
        RemoveExtensionButton.Size = new Size(42, 29);
        RemoveExtensionButton.TabIndex = 12;
        RemoveExtensionButton.Text = "<";
        RemoveExtensionButton.UseVisualStyleBackColor = true;
        RemoveExtensionButton.Click += RemoveExtensionButton_Click;
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.Location = new Point(242, 135);
        label4.Name = "label4";
        label4.Size = new Size(139, 20);
        label4.TabIndex = 13;
        label4.Text = "Selected Extensions";
        // 
        // DeleteCacheButton
        // 
        DeleteCacheButton.Location = new Point(12, 355);
        DeleteCacheButton.Name = "DeleteCacheButton";
        DeleteCacheButton.Size = new Size(403, 29);
        DeleteCacheButton.TabIndex = 14;
        DeleteCacheButton.Text = "Delete Cache";
        DeleteCacheButton.UseVisualStyleBackColor = true;
        DeleteCacheButton.Click += DeleteCacheButton_Click;
        // 
        // SessionConfigurationWindow
        // 
        AcceptButton = SaveButton;
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(427, 437);
        Controls.Add(DeleteCacheButton);
        Controls.Add(label4);
        Controls.Add(RemoveExtensionButton);
        Controls.Add(SelectExtensionButton);
        Controls.Add(SelectedExtensionsListBox);
        Controls.Add(label3);
        Controls.Add(AvailableExtensionsListBox);
        Controls.Add(SaveButton);
        Controls.Add(CancelEditButton);
        Controls.Add(BrowseDestinationFolderButton);
        Controls.Add(BrowseWatchedFolderButton);
        Controls.Add(DestinationFolderTextBox);
        Controls.Add(WatchedFolderTextBox);
        Controls.Add(label2);
        Controls.Add(label1);
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "SessionConfigurationWindow";
        StartPosition = FormStartPosition.CenterParent;
        Text = "SessionConfigurationWindow";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label label1;
    private Label label2;
    private TextBox WatchedFolderTextBox;
    private TextBox DestinationFolderTextBox;
    private Button BrowseWatchedFolderButton;
    private Button BrowseDestinationFolderButton;
    private Button CancelEditButton;
    private Button SaveButton;
    private ListBox AvailableExtensionsListBox;
    private Label label3;
    private ListBox SelectedExtensionsListBox;
    private Button SelectExtensionButton;
    private Button RemoveExtensionButton;
    private Label label4;
    private Button DeleteCacheButton;
}