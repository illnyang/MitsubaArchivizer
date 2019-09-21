namespace MitsubaArchivizer.GUI
{
    partial class MainForm
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
            this.threadListTextBox = new System.Windows.Forms.RichTextBox();
            this.listLabel = new System.Windows.Forms.Label();
            this.mediaGroupBox = new System.Windows.Forms.GroupBox();
            this.thumbnailExtLabel = new System.Windows.Forms.Label();
            this.thumbnailExtTextBox = new System.Windows.Forms.TextBox();
            this.mediaExtLabel = new System.Windows.Forms.Label();
            this.mediaExtTextbox = new System.Windows.Forms.TextBox();
            this.groupByExtensionsCheckbox = new System.Windows.Forms.CheckBox();
            this.thumbnailsEnabledCheckbox = new System.Windows.Forms.CheckBox();
            this.mediaEnabledCheckbox = new System.Windows.Forms.CheckBox();
            this.jsonGroupBox = new System.Windows.Forms.GroupBox();
            this.formattedCheckbox = new System.Windows.Forms.CheckBox();
            this.jsonEnabledCheckbox = new System.Windows.Forms.CheckBox();
            this.htmlGroupBox = new System.Windows.Forms.GroupBox();
            this.styleLabel = new System.Windows.Forms.Label();
            this.styleComboBox = new System.Windows.Forms.ComboBox();
            this.samefagCheckbox = new System.Windows.Forms.CheckBox();
            this.coloredNamesCheckbox = new System.Windows.Forms.CheckBox();
            this.namesCheckbox = new System.Windows.Forms.CheckBox();
            this.prettifyCheckbox = new System.Windows.Forms.CheckBox();
            this.htmlEnabledCheckbox = new System.Windows.Forms.CheckBox();
            this.outputDirTextbox = new System.Windows.Forms.TextBox();
            this.outputDirectoryButton = new System.Windows.Forms.Button();
            this.outputDirectoryLabel = new System.Windows.Forms.Label();
            this.archivizeButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusTextLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.seperatorLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.mediaGroupBox.SuspendLayout();
            this.jsonGroupBox.SuspendLayout();
            this.htmlGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // threadListTextBox
            // 
            this.threadListTextBox.Location = new System.Drawing.Point(12, 25);
            this.threadListTextBox.Name = "threadListTextBox";
            this.threadListTextBox.Size = new System.Drawing.Size(600, 99);
            this.threadListTextBox.TabIndex = 0;
            this.threadListTextBox.Text = "";
            // 
            // listLabel
            // 
            this.listLabel.AutoSize = true;
            this.listLabel.Location = new System.Drawing.Point(9, 9);
            this.listLabel.Name = "listLabel";
            this.listLabel.Size = new System.Drawing.Size(73, 13);
            this.listLabel.TabIndex = 1;
            this.listLabel.Text = "List of threads";
            // 
            // mediaGroupBox
            // 
            this.mediaGroupBox.Controls.Add(this.thumbnailExtLabel);
            this.mediaGroupBox.Controls.Add(this.thumbnailExtTextBox);
            this.mediaGroupBox.Controls.Add(this.mediaExtLabel);
            this.mediaGroupBox.Controls.Add(this.mediaExtTextbox);
            this.mediaGroupBox.Controls.Add(this.groupByExtensionsCheckbox);
            this.mediaGroupBox.Controls.Add(this.thumbnailsEnabledCheckbox);
            this.mediaGroupBox.Controls.Add(this.mediaEnabledCheckbox);
            this.mediaGroupBox.Location = new System.Drawing.Point(12, 130);
            this.mediaGroupBox.Name = "mediaGroupBox";
            this.mediaGroupBox.Size = new System.Drawing.Size(196, 171);
            this.mediaGroupBox.TabIndex = 2;
            this.mediaGroupBox.TabStop = false;
            this.mediaGroupBox.Text = "Media";
            // 
            // thumbnailExtLabel
            // 
            this.thumbnailExtLabel.AutoSize = true;
            this.thumbnailExtLabel.Location = new System.Drawing.Point(3, 122);
            this.thumbnailExtLabel.Name = "thumbnailExtLabel";
            this.thumbnailExtLabel.Size = new System.Drawing.Size(145, 13);
            this.thumbnailExtLabel.TabIndex = 6;
            this.thumbnailExtLabel.Text = "Allowed thumbnail extensions";
            // 
            // thumbnailExtTextBox
            // 
            this.thumbnailExtTextBox.Location = new System.Drawing.Point(6, 138);
            this.thumbnailExtTextBox.Name = "thumbnailExtTextBox";
            this.thumbnailExtTextBox.Size = new System.Drawing.Size(184, 20);
            this.thumbnailExtTextBox.TabIndex = 5;
            this.thumbnailExtTextBox.Text = "mp4,webm";
            // 
            // mediaExtLabel
            // 
            this.mediaExtLabel.AutoSize = true;
            this.mediaExtLabel.Location = new System.Drawing.Point(3, 85);
            this.mediaExtLabel.Name = "mediaExtLabel";
            this.mediaExtLabel.Size = new System.Drawing.Size(128, 13);
            this.mediaExtLabel.TabIndex = 4;
            this.mediaExtLabel.Text = "Allowed media extensions";
            // 
            // mediaExtTextbox
            // 
            this.mediaExtTextbox.Location = new System.Drawing.Point(6, 101);
            this.mediaExtTextbox.Name = "mediaExtTextbox";
            this.mediaExtTextbox.Size = new System.Drawing.Size(184, 20);
            this.mediaExtTextbox.TabIndex = 3;
            this.mediaExtTextbox.Text = "gif,jpg,mp4,png,webm";
            // 
            // groupByExtensionsCheckbox
            // 
            this.groupByExtensionsCheckbox.AutoSize = true;
            this.groupByExtensionsCheckbox.Checked = true;
            this.groupByExtensionsCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.groupByExtensionsCheckbox.Location = new System.Drawing.Point(6, 65);
            this.groupByExtensionsCheckbox.Name = "groupByExtensionsCheckbox";
            this.groupByExtensionsCheckbox.Size = new System.Drawing.Size(117, 17);
            this.groupByExtensionsCheckbox.TabIndex = 2;
            this.groupByExtensionsCheckbox.Text = "Group by extension";
            this.groupByExtensionsCheckbox.UseVisualStyleBackColor = true;
            // 
            // thumbnailsEnabledCheckbox
            // 
            this.thumbnailsEnabledCheckbox.AutoSize = true;
            this.thumbnailsEnabledCheckbox.Checked = true;
            this.thumbnailsEnabledCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.thumbnailsEnabledCheckbox.Location = new System.Drawing.Point(6, 42);
            this.thumbnailsEnabledCheckbox.Name = "thumbnailsEnabledCheckbox";
            this.thumbnailsEnabledCheckbox.Size = new System.Drawing.Size(80, 17);
            this.thumbnailsEnabledCheckbox.TabIndex = 1;
            this.thumbnailsEnabledCheckbox.Text = "Thumbnails";
            this.thumbnailsEnabledCheckbox.UseVisualStyleBackColor = true;
            // 
            // mediaEnabledCheckbox
            // 
            this.mediaEnabledCheckbox.AutoSize = true;
            this.mediaEnabledCheckbox.Checked = true;
            this.mediaEnabledCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mediaEnabledCheckbox.Location = new System.Drawing.Point(6, 19);
            this.mediaEnabledCheckbox.Name = "mediaEnabledCheckbox";
            this.mediaEnabledCheckbox.Size = new System.Drawing.Size(65, 17);
            this.mediaEnabledCheckbox.TabIndex = 0;
            this.mediaEnabledCheckbox.Text = "Enabled";
            this.mediaEnabledCheckbox.UseVisualStyleBackColor = true;
            // 
            // jsonGroupBox
            // 
            this.jsonGroupBox.Controls.Add(this.formattedCheckbox);
            this.jsonGroupBox.Controls.Add(this.jsonEnabledCheckbox);
            this.jsonGroupBox.Location = new System.Drawing.Point(214, 130);
            this.jsonGroupBox.Name = "jsonGroupBox";
            this.jsonGroupBox.Size = new System.Drawing.Size(196, 171);
            this.jsonGroupBox.TabIndex = 3;
            this.jsonGroupBox.TabStop = false;
            this.jsonGroupBox.Text = "JSON";
            // 
            // formattedCheckbox
            // 
            this.formattedCheckbox.AutoSize = true;
            this.formattedCheckbox.Location = new System.Drawing.Point(6, 42);
            this.formattedCheckbox.Name = "formattedCheckbox";
            this.formattedCheckbox.Size = new System.Drawing.Size(145, 17);
            this.formattedCheckbox.TabIndex = 1;
            this.formattedCheckbox.Text = "Formatted (larger file size)";
            this.formattedCheckbox.UseVisualStyleBackColor = true;
            // 
            // jsonEnabledCheckbox
            // 
            this.jsonEnabledCheckbox.AutoSize = true;
            this.jsonEnabledCheckbox.Checked = true;
            this.jsonEnabledCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.jsonEnabledCheckbox.Location = new System.Drawing.Point(6, 19);
            this.jsonEnabledCheckbox.Name = "jsonEnabledCheckbox";
            this.jsonEnabledCheckbox.Size = new System.Drawing.Size(65, 17);
            this.jsonEnabledCheckbox.TabIndex = 0;
            this.jsonEnabledCheckbox.Text = "Enabled";
            this.jsonEnabledCheckbox.UseVisualStyleBackColor = true;
            // 
            // htmlGroupBox
            // 
            this.htmlGroupBox.Controls.Add(this.styleLabel);
            this.htmlGroupBox.Controls.Add(this.styleComboBox);
            this.htmlGroupBox.Controls.Add(this.samefagCheckbox);
            this.htmlGroupBox.Controls.Add(this.coloredNamesCheckbox);
            this.htmlGroupBox.Controls.Add(this.namesCheckbox);
            this.htmlGroupBox.Controls.Add(this.prettifyCheckbox);
            this.htmlGroupBox.Controls.Add(this.htmlEnabledCheckbox);
            this.htmlGroupBox.Location = new System.Drawing.Point(416, 130);
            this.htmlGroupBox.Name = "htmlGroupBox";
            this.htmlGroupBox.Size = new System.Drawing.Size(196, 171);
            this.htmlGroupBox.TabIndex = 3;
            this.htmlGroupBox.TabStop = false;
            this.htmlGroupBox.Text = "HTML";
            // 
            // styleLabel
            // 
            this.styleLabel.AutoSize = true;
            this.styleLabel.Location = new System.Drawing.Point(3, 128);
            this.styleLabel.Name = "styleLabel";
            this.styleLabel.Size = new System.Drawing.Size(30, 13);
            this.styleLabel.TabIndex = 7;
            this.styleLabel.Text = "Style";
            // 
            // styleComboBox
            // 
            this.styleComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.styleComboBox.FormattingEnabled = true;
            this.styleComboBox.Location = new System.Drawing.Point(6, 144);
            this.styleComboBox.Name = "styleComboBox";
            this.styleComboBox.Size = new System.Drawing.Size(104, 21);
            this.styleComboBox.TabIndex = 6;
            // 
            // samefagCheckbox
            // 
            this.samefagCheckbox.AutoSize = true;
            this.samefagCheckbox.Checked = true;
            this.samefagCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.samefagCheckbox.Location = new System.Drawing.Point(6, 111);
            this.samefagCheckbox.Name = "samefagCheckbox";
            this.samefagCheckbox.Size = new System.Drawing.Size(98, 17);
            this.samefagCheckbox.TabIndex = 5;
            this.samefagCheckbox.Text = "Samefag count";
            this.samefagCheckbox.UseVisualStyleBackColor = true;
            // 
            // coloredNamesCheckbox
            // 
            this.coloredNamesCheckbox.AutoSize = true;
            this.coloredNamesCheckbox.Checked = true;
            this.coloredNamesCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.coloredNamesCheckbox.Location = new System.Drawing.Point(6, 88);
            this.coloredNamesCheckbox.Name = "coloredNamesCheckbox";
            this.coloredNamesCheckbox.Size = new System.Drawing.Size(96, 17);
            this.coloredNamesCheckbox.TabIndex = 4;
            this.coloredNamesCheckbox.Text = "Colored names";
            this.coloredNamesCheckbox.UseVisualStyleBackColor = true;
            // 
            // namesCheckbox
            // 
            this.namesCheckbox.AutoSize = true;
            this.namesCheckbox.Checked = true;
            this.namesCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.namesCheckbox.Location = new System.Drawing.Point(6, 65);
            this.namesCheckbox.Name = "namesCheckbox";
            this.namesCheckbox.Size = new System.Drawing.Size(59, 17);
            this.namesCheckbox.TabIndex = 3;
            this.namesCheckbox.Text = "Names";
            this.namesCheckbox.UseVisualStyleBackColor = true;
            // 
            // prettifyCheckbox
            // 
            this.prettifyCheckbox.AutoSize = true;
            this.prettifyCheckbox.Location = new System.Drawing.Point(6, 42);
            this.prettifyCheckbox.Name = "prettifyCheckbox";
            this.prettifyCheckbox.Size = new System.Drawing.Size(130, 17);
            this.prettifyCheckbox.TabIndex = 2;
            this.prettifyCheckbox.Text = "Prettify (larger file size)";
            this.prettifyCheckbox.UseVisualStyleBackColor = true;
            // 
            // htmlEnabledCheckbox
            // 
            this.htmlEnabledCheckbox.AutoSize = true;
            this.htmlEnabledCheckbox.Checked = true;
            this.htmlEnabledCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.htmlEnabledCheckbox.Location = new System.Drawing.Point(6, 19);
            this.htmlEnabledCheckbox.Name = "htmlEnabledCheckbox";
            this.htmlEnabledCheckbox.Size = new System.Drawing.Size(65, 17);
            this.htmlEnabledCheckbox.TabIndex = 1;
            this.htmlEnabledCheckbox.Text = "Enabled";
            this.htmlEnabledCheckbox.UseVisualStyleBackColor = true;
            // 
            // outputDirTextbox
            // 
            this.outputDirTextbox.Location = new System.Drawing.Point(100, 311);
            this.outputDirTextbox.Name = "outputDirTextbox";
            this.outputDirTextbox.ReadOnly = true;
            this.outputDirTextbox.Size = new System.Drawing.Size(273, 20);
            this.outputDirTextbox.TabIndex = 4;
            // 
            // outputDirectoryButton
            // 
            this.outputDirectoryButton.Location = new System.Drawing.Point(379, 311);
            this.outputDirectoryButton.Name = "outputDirectoryButton";
            this.outputDirectoryButton.Size = new System.Drawing.Size(32, 20);
            this.outputDirectoryButton.TabIndex = 5;
            this.outputDirectoryButton.Text = "...";
            this.outputDirectoryButton.UseVisualStyleBackColor = true;
            this.outputDirectoryButton.Click += new System.EventHandler(this.outputDirectoryButton_Click);
            // 
            // outputDirectoryLabel
            // 
            this.outputDirectoryLabel.AutoSize = true;
            this.outputDirectoryLabel.Location = new System.Drawing.Point(9, 314);
            this.outputDirectoryLabel.Name = "outputDirectoryLabel";
            this.outputDirectoryLabel.Size = new System.Drawing.Size(85, 13);
            this.outputDirectoryLabel.TabIndex = 6;
            this.outputDirectoryLabel.Text = "Output directory:";
            // 
            // archivizeButton
            // 
            this.archivizeButton.Location = new System.Drawing.Point(12, 342);
            this.archivizeButton.Name = "archivizeButton";
            this.archivizeButton.Size = new System.Drawing.Size(398, 37);
            this.archivizeButton.TabIndex = 7;
            this.archivizeButton.Text = "Archivize";
            this.archivizeButton.UseVisualStyleBackColor = true;
            this.archivizeButton.Click += new System.EventHandler(this.archivizeButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::MitsubaArchivizer.GUI.Properties.Resources.logo1;
            this.pictureBox1.Location = new System.Drawing.Point(416, 307);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(196, 72);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.statusTextLabel,
            this.seperatorLabel,
            this.statusProgressBar});
            this.statusStrip.Location = new System.Drawing.Point(0, 388);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(624, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 9;
            this.statusStrip.Text = "Status:";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(42, 17);
            this.statusLabel.Text = "Status:";
            // 
            // statusTextLabel
            // 
            this.statusTextLabel.Name = "statusTextLabel";
            this.statusTextLabel.Size = new System.Drawing.Size(26, 17);
            this.statusTextLabel.Text = "Idle";
            // 
            // seperatorLabel
            // 
            this.seperatorLabel.Name = "seperatorLabel";
            this.seperatorLabel.Size = new System.Drawing.Size(541, 17);
            this.seperatorLabel.Spring = true;
            // 
            // statusProgressBar
            // 
            this.statusProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.statusProgressBar.Name = "statusProgressBar";
            this.statusProgressBar.Size = new System.Drawing.Size(100, 16);
            this.statusProgressBar.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 410);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.archivizeButton);
            this.Controls.Add(this.outputDirectoryLabel);
            this.Controls.Add(this.outputDirectoryButton);
            this.Controls.Add(this.outputDirTextbox);
            this.Controls.Add(this.htmlGroupBox);
            this.Controls.Add(this.jsonGroupBox);
            this.Controls.Add(this.mediaGroupBox);
            this.Controls.Add(this.listLabel);
            this.Controls.Add(this.threadListTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mitsuba Archivizer";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.mediaGroupBox.ResumeLayout(false);
            this.mediaGroupBox.PerformLayout();
            this.jsonGroupBox.ResumeLayout(false);
            this.jsonGroupBox.PerformLayout();
            this.htmlGroupBox.ResumeLayout(false);
            this.htmlGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox threadListTextBox;
        private System.Windows.Forms.Label listLabel;
        private System.Windows.Forms.GroupBox mediaGroupBox;
        private System.Windows.Forms.CheckBox mediaEnabledCheckbox;
        private System.Windows.Forms.GroupBox jsonGroupBox;
        private System.Windows.Forms.CheckBox jsonEnabledCheckbox;
        private System.Windows.Forms.GroupBox htmlGroupBox;
        private System.Windows.Forms.CheckBox htmlEnabledCheckbox;
        private System.Windows.Forms.CheckBox thumbnailsEnabledCheckbox;
        private System.Windows.Forms.CheckBox groupByExtensionsCheckbox;
        private System.Windows.Forms.CheckBox formattedCheckbox;
        private System.Windows.Forms.CheckBox prettifyCheckbox;
        private System.Windows.Forms.CheckBox namesCheckbox;
        private System.Windows.Forms.CheckBox coloredNamesCheckbox;
        private System.Windows.Forms.CheckBox samefagCheckbox;
        private System.Windows.Forms.Label styleLabel;
        private System.Windows.Forms.ComboBox styleComboBox;
        private System.Windows.Forms.Label thumbnailExtLabel;
        private System.Windows.Forms.TextBox thumbnailExtTextBox;
        private System.Windows.Forms.Label mediaExtLabel;
        private System.Windows.Forms.TextBox mediaExtTextbox;
        private System.Windows.Forms.TextBox outputDirTextbox;
        private System.Windows.Forms.Button outputDirectoryButton;
        private System.Windows.Forms.Label outputDirectoryLabel;
        private System.Windows.Forms.Button archivizeButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripStatusLabel statusTextLabel;
        private System.Windows.Forms.ToolStripProgressBar statusProgressBar;
        private System.Windows.Forms.ToolStripStatusLabel seperatorLabel;
    }
}