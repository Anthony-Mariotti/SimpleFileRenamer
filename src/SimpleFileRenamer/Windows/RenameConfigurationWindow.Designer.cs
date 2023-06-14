namespace SimpleFileRenamer;

partial class RenameConfigurationWindow
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
        SeperatorLabel = new Label();
        SeperatorTextBox = new TextBox();
        RenameFormatLabel = new Label();
        FormatTextBox = new TextBox();
        CancelEditButton = new Button();
        SaveEditButton = new Button();
        UseSpaceButton = new Button();
        SuspendLayout();
        // 
        // SeperatorLabel
        // 
        SeperatorLabel.AutoSize = true;
        SeperatorLabel.Location = new Point(12, 9);
        SeperatorLabel.Name = "SeperatorLabel";
        SeperatorLabel.Size = new Size(74, 20);
        SeperatorLabel.TabIndex = 0;
        SeperatorLabel.Text = "Seperator";
        // 
        // SeperatorTextBox
        // 
        SeperatorTextBox.Location = new Point(12, 32);
        SeperatorTextBox.Name = "SeperatorTextBox";
        SeperatorTextBox.Size = new Size(200, 27);
        SeperatorTextBox.TabIndex = 1;
        SeperatorTextBox.TextChanged += SeperatorTextBox_TextChanged;
        // 
        // RenameFormatLabel
        // 
        RenameFormatLabel.AutoSize = true;
        RenameFormatLabel.Location = new Point(12, 71);
        RenameFormatLabel.Name = "RenameFormatLabel";
        RenameFormatLabel.Size = new Size(114, 20);
        RenameFormatLabel.TabIndex = 2;
        RenameFormatLabel.Text = "Rename Format";
        // 
        // FormatTextBox
        // 
        FormatTextBox.Location = new Point(12, 94);
        FormatTextBox.Name = "FormatTextBox";
        FormatTextBox.Size = new Size(320, 27);
        FormatTextBox.TabIndex = 3;
        FormatTextBox.TextChanged += FormatTextBox_TextChanged;
        // 
        // CancelEditButton
        // 
        CancelEditButton.Location = new Point(12, 139);
        CancelEditButton.Name = "CancelEditButton";
        CancelEditButton.Size = new Size(94, 29);
        CancelEditButton.TabIndex = 4;
        CancelEditButton.Text = "Cancel";
        CancelEditButton.UseVisualStyleBackColor = true;
        // 
        // SaveEditButton
        // 
        SaveEditButton.Enabled = false;
        SaveEditButton.Location = new Point(238, 139);
        SaveEditButton.Name = "SaveEditButton";
        SaveEditButton.Size = new Size(94, 29);
        SaveEditButton.TabIndex = 5;
        SaveEditButton.Text = "Save";
        SaveEditButton.UseVisualStyleBackColor = true;
        SaveEditButton.Click += SaveEditButton_Click;
        // 
        // UseSpaceButton
        // 
        UseSpaceButton.Enabled = false;
        UseSpaceButton.Location = new Point(218, 30);
        UseSpaceButton.Name = "UseSpaceButton";
        UseSpaceButton.Size = new Size(114, 29);
        UseSpaceButton.TabIndex = 6;
        UseSpaceButton.Text = "Using Space";
        UseSpaceButton.UseVisualStyleBackColor = true;
        UseSpaceButton.Click += UseSpaceButton_Click;
        // 
        // RenameConfigurationWindow
        // 
        AcceptButton = SaveEditButton;
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = CancelEditButton;
        ClientSize = new Size(349, 180);
        Controls.Add(UseSpaceButton);
        Controls.Add(SaveEditButton);
        Controls.Add(CancelEditButton);
        Controls.Add(FormatTextBox);
        Controls.Add(RenameFormatLabel);
        Controls.Add(SeperatorTextBox);
        Controls.Add(SeperatorLabel);
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "RenameConfigurationWindow";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Format Settings";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label SeperatorLabel;
    private TextBox SeperatorTextBox;
    private Label RenameFormatLabel;
    private TextBox FormatTextBox;
    private Button CancelEditButton;
    private Button SaveEditButton;
    private Button UseSpaceButton;
}