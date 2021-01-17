
namespace SplitSecondMagyaritas
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.arkFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.arkFilePath = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.arkGroup = new System.Windows.Forms.GroupBox();
			this.arkSizeOutput = new System.Windows.Forms.Label();
			this.arkSizeLabel = new System.Windows.Forms.Label();
			this.arkBrowseButton = new System.Windows.Forms.Button();
			this.checksGroup = new System.Windows.Forms.GroupBox();
			this.checkIconTempSpace = new System.Windows.Forms.PictureBox();
			this.checkIconExtractor = new System.Windows.Forms.PictureBox();
			this.checkLabelTempSpace = new System.Windows.Forms.Label();
			this.checkLabelExtractor = new System.Windows.Forms.Label();
			this.extractGroup = new System.Windows.Forms.GroupBox();
			this.diskFreeOutput = new System.Windows.Forms.Label();
			this.extractBrowseButton = new System.Windows.Forms.Button();
			this.diskFreeLabel = new System.Windows.Forms.Label();
			this.extractFolderPath = new System.Windows.Forms.TextBox();
			this.tempFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.installButton = new System.Windows.Forms.Button();
			this.arkGroup.SuspendLayout();
			this.checksGroup.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.checkIconTempSpace)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.checkIconExtractor)).BeginInit();
			this.extractGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// arkFileDialog
			// 
			this.arkFileDialog.RestoreDirectory = true;
			// 
			// arkFilePath
			// 
			this.arkFilePath.AllowDrop = true;
			this.arkFilePath.BackColor = System.Drawing.Color.Black;
			resources.ApplyResources(this.arkFilePath, "arkFilePath");
			this.arkFilePath.ForeColor = System.Drawing.Color.Gainsboro;
			this.arkFilePath.Name = "arkFilePath";
			this.arkFilePath.ReadOnly = true;
			this.arkFilePath.UseWaitCursor = true;
			this.arkFilePath.DragDrop += new System.Windows.Forms.DragEventHandler(this.arkFile_DragDrop);
			this.arkFilePath.DragEnter += new System.Windows.Forms.DragEventHandler(this.arkFile_DragEnter);
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.label1, "label1");
			this.label1.ForeColor = System.Drawing.Color.Gainsboro;
			this.label1.Name = "label1";
			this.label1.UseWaitCursor = true;
			// 
			// arkGroup
			// 
			this.arkGroup.BackColor = System.Drawing.Color.Transparent;
			this.arkGroup.Controls.Add(this.arkSizeOutput);
			this.arkGroup.Controls.Add(this.arkSizeLabel);
			this.arkGroup.Controls.Add(this.arkBrowseButton);
			this.arkGroup.Controls.Add(this.arkFilePath);
			resources.ApplyResources(this.arkGroup, "arkGroup");
			this.arkGroup.ForeColor = System.Drawing.Color.Gainsboro;
			this.arkGroup.Name = "arkGroup";
			this.arkGroup.TabStop = false;
			this.arkGroup.UseWaitCursor = true;
			this.arkGroup.DragDrop += new System.Windows.Forms.DragEventHandler(this.arkFile_DragDrop);
			this.arkGroup.DragEnter += new System.Windows.Forms.DragEventHandler(this.arkFile_DragEnter);
			// 
			// arkSizeOutput
			// 
			this.arkSizeOutput.AllowDrop = true;
			resources.ApplyResources(this.arkSizeOutput, "arkSizeOutput");
			this.arkSizeOutput.Name = "arkSizeOutput";
			this.arkSizeOutput.UseWaitCursor = true;
			this.arkSizeOutput.DragDrop += new System.Windows.Forms.DragEventHandler(this.arkFile_DragDrop);
			this.arkSizeOutput.DragEnter += new System.Windows.Forms.DragEventHandler(this.arkFile_DragEnter);
			// 
			// arkSizeLabel
			// 
			this.arkSizeLabel.AllowDrop = true;
			resources.ApplyResources(this.arkSizeLabel, "arkSizeLabel");
			this.arkSizeLabel.Name = "arkSizeLabel";
			this.arkSizeLabel.UseWaitCursor = true;
			this.arkSizeLabel.DragDrop += new System.Windows.Forms.DragEventHandler(this.arkFile_DragDrop);
			this.arkSizeLabel.DragEnter += new System.Windows.Forms.DragEventHandler(this.arkFile_DragEnter);
			// 
			// arkBrowseButton
			// 
			this.arkBrowseButton.AllowDrop = true;
			this.arkBrowseButton.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.arkBrowseButton, "arkBrowseButton");
			this.arkBrowseButton.Name = "arkBrowseButton";
			this.arkBrowseButton.UseVisualStyleBackColor = false;
			this.arkBrowseButton.UseWaitCursor = true;
			this.arkBrowseButton.Click += new System.EventHandler(this.arkBrowseButton_Click);
			this.arkBrowseButton.DragDrop += new System.Windows.Forms.DragEventHandler(this.arkFile_DragDrop);
			this.arkBrowseButton.DragEnter += new System.Windows.Forms.DragEventHandler(this.arkFile_DragEnter);
			// 
			// checksGroup
			// 
			this.checksGroup.BackColor = System.Drawing.Color.Transparent;
			this.checksGroup.Controls.Add(this.checkIconTempSpace);
			this.checksGroup.Controls.Add(this.checkIconExtractor);
			this.checksGroup.Controls.Add(this.checkLabelTempSpace);
			this.checksGroup.Controls.Add(this.checkLabelExtractor);
			this.checksGroup.ForeColor = System.Drawing.Color.Gainsboro;
			resources.ApplyResources(this.checksGroup, "checksGroup");
			this.checksGroup.Name = "checksGroup";
			this.checksGroup.TabStop = false;
			this.checksGroup.UseWaitCursor = true;
			// 
			// checkIconTempSpace
			// 
			resources.ApplyResources(this.checkIconTempSpace, "checkIconTempSpace");
			this.checkIconTempSpace.Name = "checkIconTempSpace";
			this.checkIconTempSpace.TabStop = false;
			this.checkIconTempSpace.UseWaitCursor = true;
			// 
			// checkIconExtractor
			// 
			resources.ApplyResources(this.checkIconExtractor, "checkIconExtractor");
			this.checkIconExtractor.Name = "checkIconExtractor";
			this.checkIconExtractor.TabStop = false;
			this.checkIconExtractor.UseWaitCursor = true;
			// 
			// checkLabelTempSpace
			// 
			this.checkLabelTempSpace.AutoEllipsis = true;
			resources.ApplyResources(this.checkLabelTempSpace, "checkLabelTempSpace");
			this.checkLabelTempSpace.Name = "checkLabelTempSpace";
			this.checkLabelTempSpace.UseWaitCursor = true;
			// 
			// checkLabelExtractor
			// 
			this.checkLabelExtractor.AutoEllipsis = true;
			resources.ApplyResources(this.checkLabelExtractor, "checkLabelExtractor");
			this.checkLabelExtractor.Name = "checkLabelExtractor";
			this.checkLabelExtractor.UseWaitCursor = true;
			// 
			// extractGroup
			// 
			this.extractGroup.BackColor = System.Drawing.Color.Transparent;
			this.extractGroup.Controls.Add(this.diskFreeOutput);
			this.extractGroup.Controls.Add(this.extractBrowseButton);
			this.extractGroup.Controls.Add(this.diskFreeLabel);
			this.extractGroup.Controls.Add(this.extractFolderPath);
			resources.ApplyResources(this.extractGroup, "extractGroup");
			this.extractGroup.ForeColor = System.Drawing.Color.Gainsboro;
			this.extractGroup.Name = "extractGroup";
			this.extractGroup.TabStop = false;
			this.extractGroup.UseWaitCursor = true;
			// 
			// diskFreeOutput
			// 
			resources.ApplyResources(this.diskFreeOutput, "diskFreeOutput");
			this.diskFreeOutput.Name = "diskFreeOutput";
			this.diskFreeOutput.UseWaitCursor = true;
			// 
			// extractBrowseButton
			// 
			this.extractBrowseButton.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.extractBrowseButton, "extractBrowseButton");
			this.extractBrowseButton.Name = "extractBrowseButton";
			this.extractBrowseButton.UseVisualStyleBackColor = false;
			this.extractBrowseButton.UseWaitCursor = true;
			this.extractBrowseButton.Click += new System.EventHandler(this.extractBrowseButton_Click);
			// 
			// diskFreeLabel
			// 
			resources.ApplyResources(this.diskFreeLabel, "diskFreeLabel");
			this.diskFreeLabel.Name = "diskFreeLabel";
			this.diskFreeLabel.UseWaitCursor = true;
			// 
			// extractFolderPath
			// 
			this.extractFolderPath.BackColor = System.Drawing.Color.Black;
			resources.ApplyResources(this.extractFolderPath, "extractFolderPath");
			this.extractFolderPath.ForeColor = System.Drawing.Color.Gainsboro;
			this.extractFolderPath.Name = "extractFolderPath";
			this.extractFolderPath.ReadOnly = true;
			this.extractFolderPath.UseWaitCursor = true;
			// 
			// installButton
			// 
			this.installButton.BackColor = System.Drawing.Color.Transparent;
			resources.ApplyResources(this.installButton, "installButton");
			this.installButton.ForeColor = System.Drawing.Color.LimeGreen;
			this.installButton.Name = "installButton";
			this.installButton.UseVisualStyleBackColor = false;
			this.installButton.UseWaitCursor = true;
			this.installButton.Click += new System.EventHandler(this.installButton_Click);
			// 
			// MainForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.Controls.Add(this.installButton);
			this.Controls.Add(this.extractGroup);
			this.Controls.Add(this.checksGroup);
			this.Controls.Add(this.arkGroup);
			this.Controls.Add(this.label1);
			this.Icon = global::SplitSecondMagyaritas.Properties.Resources.icon;
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.UseWaitCursor = true;
			this.Load += new System.EventHandler(this.Form1_Load);
			this.arkGroup.ResumeLayout(false);
			this.arkGroup.PerformLayout();
			this.checksGroup.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.checkIconTempSpace)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.checkIconExtractor)).EndInit();
			this.extractGroup.ResumeLayout(false);
			this.extractGroup.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.OpenFileDialog arkFileDialog;
		private System.Windows.Forms.TextBox arkFilePath;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox arkGroup;
		private System.Windows.Forms.Button arkBrowseButton;
		private System.Windows.Forms.GroupBox checksGroup;
		private System.Windows.Forms.PictureBox checkIconExtractor;
		private System.Windows.Forms.Label checkLabelExtractor;
		private System.Windows.Forms.PictureBox checkIconTempSpace;
		private System.Windows.Forms.Label checkLabelTempSpace;
		private System.Windows.Forms.GroupBox extractGroup;
		private System.Windows.Forms.Button extractBrowseButton;
		private System.Windows.Forms.TextBox extractFolderPath;
		private System.Windows.Forms.FolderBrowserDialog tempFolderDialog;
		private System.Windows.Forms.Label arkSizeOutput;
		private System.Windows.Forms.Label arkSizeLabel;
		private System.Windows.Forms.Label diskFreeLabel;
		private System.Windows.Forms.Label diskFreeOutput;
		private System.Windows.Forms.Button installButton;
	}
}

