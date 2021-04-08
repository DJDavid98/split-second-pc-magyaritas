
namespace SplitSecondMagyaritas {
	partial class InstallForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InstallForm));
			this.installProgress = new System.Windows.Forms.ProgressBar();
			this.cancelButton = new System.Windows.Forms.Button();
			this.installWorker = new System.ComponentModel.BackgroundWorker();
			this.installOutput = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			//
			// installProgress
			//
			this.installProgress.Location = new System.Drawing.Point(12, 223);
			this.installProgress.Name = "installProgress";
			this.installProgress.Size = new System.Drawing.Size(409, 23);
			this.installProgress.TabIndex = 0;
			//
			// cancelButton
			//
			this.cancelButton.BackColor = System.Drawing.Color.Transparent;
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.cancelButton.ForeColor = System.Drawing.Color.OrangeRed;
			this.cancelButton.Location = new System.Drawing.Point(427, 223);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(105, 23);
			this.cancelButton.TabIndex = 1;
			this.cancelButton.Text = "Megszakítás (Esc)";
			this.cancelButton.UseVisualStyleBackColor = false;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			//
			// installWorker
			//
			this.installWorker.WorkerReportsProgress = true;
			this.installWorker.WorkerSupportsCancellation = true;
			this.installWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.installWorker_DoWork);
			this.installWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.updateProgress);
			this.installWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.installWorker_RunWorkerCompleted);
			//
			// installOutput
			//
			this.installOutput.BackColor = System.Drawing.Color.Black;
			this.installOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.installOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.installOutput.ForeColor = System.Drawing.Color.Gainsboro;
			this.installOutput.Location = new System.Drawing.Point(13, 13);
			this.installOutput.Name = "installOutput";
			this.installOutput.ReadOnly = true;
			this.installOutput.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
			this.installOutput.Size = new System.Drawing.Size(519, 200);
			this.installOutput.TabIndex = 2;
			this.installOutput.Text = "";
			//
			// InstallForm
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(544, 258);
			this.Controls.Add(this.installOutput);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.installProgress);
			this.Icon = global::SplitSecondMagyaritas.Properties.Resources.icon;
			this.MaximizeBox = false;
			this.Name = "InstallForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Split/Second Magyarítás Telepítése";
			this.Load += new System.EventHandler(this.InstallForm_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ProgressBar installProgress;
		private System.Windows.Forms.Button cancelButton;
		private System.ComponentModel.BackgroundWorker installWorker;
		private System.Windows.Forms.RichTextBox installOutput;
	}
}
