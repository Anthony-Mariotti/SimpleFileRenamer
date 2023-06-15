namespace SimpleFileRenamer;

partial class SessionWindow
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
        FilesListView = new ListView();
        ExitSessionButton = new Button();
        FinishSessionButton = new Button();
        SuspendLayout();
        // 
        // FilesListView
        // 
        FilesListView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        FilesListView.Location = new Point(12, 12);
        FilesListView.Name = "FilesListView";
        FilesListView.Size = new Size(384, 269);
        FilesListView.TabIndex = 0;
        FilesListView.UseCompatibleStateImageBehavior = false;
        // 
        // ExitSessionButton
        // 
        ExitSessionButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        ExitSessionButton.Location = new Point(12, 287);
        ExitSessionButton.Name = "ExitSessionButton";
        ExitSessionButton.Size = new Size(120, 29);
        ExitSessionButton.TabIndex = 1;
        ExitSessionButton.Text = "Close Session";
        ExitSessionButton.UseVisualStyleBackColor = true;
        ExitSessionButton.Click += ExitSessionButton_Click;
        // 
        // FinishSessionButton
        // 
        FinishSessionButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        FinishSessionButton.Location = new Point(264, 287);
        FinishSessionButton.Name = "FinishSessionButton";
        FinishSessionButton.Size = new Size(121, 29);
        FinishSessionButton.TabIndex = 2;
        FinishSessionButton.Text = "Finish Session";
        FinishSessionButton.UseVisualStyleBackColor = true;
        FinishSessionButton.Click += FinishSessionButton_Click;
        // 
        // SessionWindow
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = ExitSessionButton;
        ClientSize = new Size(397, 328);
        Controls.Add(FinishSessionButton);
        Controls.Add(ExitSessionButton);
        Controls.Add(FilesListView);
        MaximizeBox = false;
        MaximumSize = new Size(415, 375);
        MinimumSize = new Size(415, 375);
        Name = "SessionWindow";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Session";
        FormClosing += SessionWindow_FormClosing;
        ResumeLayout(false);
    }

    #endregion

    private ListView FilesListView;
    private Button ExitSessionButton;
    private Button FinishSessionButton;
}