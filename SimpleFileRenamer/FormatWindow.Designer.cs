namespace SimpleFileRenamer;

partial class FormatWindow
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
        seperatorTextBox = new TextBox();
        label2 = new Label();
        formatTextBox = new TextBox();
        cancelButton = new Button();
        saveButton = new Button();
        useSpaceButton = new Button();
        SuspendLayout();
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(12, 9);
        label1.Name = "label1";
        label1.Size = new Size(74, 20);
        label1.TabIndex = 0;
        label1.Text = "Seperator";
        // 
        // seperatorTextBox
        // 
        seperatorTextBox.Location = new Point(12, 32);
        seperatorTextBox.Name = "seperatorTextBox";
        seperatorTextBox.Size = new Size(200, 27);
        seperatorTextBox.TabIndex = 1;
        seperatorTextBox.TextChanged += seperatorTextBox_TextChanged;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new Point(12, 71);
        label2.Name = "label2";
        label2.Size = new Size(114, 20);
        label2.TabIndex = 2;
        label2.Text = "Rename Format";
        // 
        // formatTextBox
        // 
        formatTextBox.Location = new Point(12, 94);
        formatTextBox.Name = "formatTextBox";
        formatTextBox.Size = new Size(320, 27);
        formatTextBox.TabIndex = 3;
        formatTextBox.TextChanged += formatTextBox_TextChanged;
        // 
        // cancelButton
        // 
        cancelButton.Location = new Point(12, 139);
        cancelButton.Name = "cancelButton";
        cancelButton.Size = new Size(94, 29);
        cancelButton.TabIndex = 4;
        cancelButton.Text = "Cancel";
        cancelButton.UseVisualStyleBackColor = true;
        // 
        // saveButton
        // 
        saveButton.Enabled = false;
        saveButton.Location = new Point(238, 139);
        saveButton.Name = "saveButton";
        saveButton.Size = new Size(94, 29);
        saveButton.TabIndex = 5;
        saveButton.Text = "Save";
        saveButton.UseVisualStyleBackColor = true;
        saveButton.Click += saveButton_Click;
        // 
        // useSpaceButton
        // 
        useSpaceButton.Enabled = false;
        useSpaceButton.Location = new Point(218, 30);
        useSpaceButton.Name = "useSpaceButton";
        useSpaceButton.Size = new Size(114, 29);
        useSpaceButton.TabIndex = 6;
        useSpaceButton.Text = "Using Space";
        useSpaceButton.UseVisualStyleBackColor = true;
        useSpaceButton.Click += useSpaceButton_Click;
        // 
        // FormatWindow
        // 
        AcceptButton = saveButton;
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = cancelButton;
        ClientSize = new Size(349, 180);
        Controls.Add(useSpaceButton);
        Controls.Add(saveButton);
        Controls.Add(cancelButton);
        Controls.Add(formatTextBox);
        Controls.Add(label2);
        Controls.Add(seperatorTextBox);
        Controls.Add(label1);
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FormatWindow";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Format Settings";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label label1;
    private TextBox seperatorTextBox;
    private Label label2;
    private TextBox formatTextBox;
    private Button cancelButton;
    private Button saveButton;
    private Button useSpaceButton;
}