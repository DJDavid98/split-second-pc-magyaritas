using System;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;

namespace SplitSecondMagyaritas {
	public partial class MainForm : Form {
		private const string extractorAppPath = "Split_Second_ARK_Tool.exe";
		private const string regKey = "Split/Second";
		private const string defaultFileSizeText = "Válassz fájlt!";
		private const string defaultDirectorySizeText = "Válassz mappát!";
		private const string browseForFileName = "resident.ark";
		private const double arkExtractMultiplier = 1.72;

		private string browseForFilePath = $@"data\{browseForFileName}";
		private long requiredExtractSpace = 0;
		private long freeSpace = 0;
		private string extractorPath;
		private FileStream extractorLock = null;

		public MainForm() {
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e) {
			checkLabelExtractor.Text = String.Format(checkLabelExtractor.Text, extractorAppPath);
			arkGroup.Text = String.Format(arkGroup.Text, browseForFilePath);
			resetArkInputs();
			resetExtractInputs();

			controlAllInputs();

			detectSplitSecondInstall();

			detectTempPath();

			controlAllInputs();

			UseWaitCursor = false;
		}

		// Check for extractor app
		private bool isExtractorAppPresent() {
			extractorPath = Path.Combine(Directory.GetCurrentDirectory(), extractorAppPath);
			if (File.Exists(extractorPath) && (extractorLock != null || fileIsExecutable(extractorPath))) {
				if (extractorLock == null) {
					// Lock the extractor file to prevent moving/renaming it
					extractorLock = new FileStream(extractorPath, FileMode.Open, FileAccess.Read, FileShare.None);
				}
				return true;
			}

			return false;
		}

		private bool isThereEnoughFreeSpace() {
			return freeSpace > requiredExtractSpace;
		}

		private Image getCheckLabelIcon(bool success) {
			if (success) {
				return SplitSecondMagyaritas.Properties.Resources.checkmark;
			}

			return SystemIcons.Error.ToBitmap();
		}

		private Color getCheckLabelColor(bool success) {
			if (success) {
				return Color.LimeGreen;
			}

			return Color.Red;
		}

		public bool fileIsExecutable(string file) {
			try {
				var firstTwoBytes = new byte[2];
				using (var fileStream = File.Open(file, FileMode.Open)) {
					fileStream.Read(firstTwoBytes, 0, 2);
				}
				return Encoding.UTF8.GetString(firstTwoBytes) == "MZ";
			} catch { }
			return false;
		}

		private void arkBrowseButton_Click(object sender, EventArgs e) {
			arkFileDialog.Title = $@"Tallózd be a {browseForFilePath} fájlt";
			arkFileDialog.Filter = $"{browseForFileName}|{browseForFileName}";

			if (arkFileDialog.ShowDialog() == DialogResult.OK) {
				string filePath = arkFileDialog.FileName;
				validateArkFile(filePath);
			} else {
				resetArkInputs();
			}
		}

		private void extractBrowseButton_Click(object sender, EventArgs e) {
			tempFolderDialog.Description = "Adj meg egy ideiglenes mappát a fájlok kibontásához";

			if (tempFolderDialog.ShowDialog() == DialogResult.OK) {
				string folderPath = tempFolderDialog.SelectedPath;
				validateExtractPath(folderPath);
			} else {
				resetExtractInputs();
			}
		}

		private void arkFile_DragDrop(object sender, DragEventArgs e) {
			string[] fileList = (string[]) e.Data.GetData(DataFormats.FileDrop, false);
			if (fileList.Length == 1 && fileList[0].EndsWith(browseForFileName)) {
				validateArkFile(fileList[0]);
			}
		}

		private void arkFile_DragEnter(object sender, DragEventArgs e) {
			// If the data is a file or a bitmap, display the copy cursor.
			if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
				e.Effect = DragDropEffects.Copy;
				return;
			}
			e.Effect = DragDropEffects.None;
		}

		private bool validateArkFile(string path) {
			// File exists
			if (!File.Exists(path)) {
				resetArkInputs();
				return false;
			}

			// Non-zero size
			long filesize = 0;
			try {
				filesize = new FileInfo(path).Length;
			} catch { }
			if (filesize == 0) {
				resetArkInputs();
				return false;
			}

			requiredExtractSpace = Convert.ToInt64(filesize * arkExtractMultiplier);
			arkSizeOutput.Text = String.Format("{0} (kb. {1} kibontva)", bytesToString(filesize), bytesToString(requiredExtractSpace));
			arkSizeOutput.ForeColor = Color.Gainsboro;
			arkFilePath.Text = path;
			controlAllInputs();
			return true;
		}

		private bool validateExtractPath(string path) {
			string pattern = "^[A-Z]:";
			if (!Regex.IsMatch(path, pattern) && !Directory.Exists(path)) {
				resetExtractInputs();
				return false;
			}

			string driveLetter = Regex.Match(path, pattern).Groups[0].Captures[0].Value;
			DriveInfo dInfo = new DriveInfo(driveLetter);
			freeSpace = dInfo.AvailableFreeSpace;
			diskFreeOutput.Text = bytesToString(freeSpace);
			diskFreeOutput.ForeColor = Color.Gainsboro;
			extractFolderPath.Text = path;
			controlAllInputs();
			return true;
		}

		static string bytesToString(long byteCount) {
			string[] suf = { "B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB" };
			int factor = 1024;
			int place = 0;
			double num = 1;
			if (byteCount > 0) {
				long bytes = Math.Abs(byteCount);
				place = Convert.ToInt32(Math.Floor(Math.Log(bytes, factor)));
				num = Math.Round(bytes / Math.Pow(factor, place), 1);
			}
			return $"{(Math.Sign(byteCount) * num).ToString()} {suf[place]}";
		}

		private void resetArkInputs() {
			arkFilePath.Text = "";
			arkSizeOutput.Text = defaultFileSizeText;
			arkSizeOutput.ForeColor = Color.Red;
			requiredExtractSpace = 0;
			controlAllInputs();
		}

		private void resetExtractInputs() {
			extractFolderPath.Text = "";
			diskFreeOutput.Text = defaultDirectorySizeText;
			diskFreeOutput.ForeColor = Color.Red;
			freeSpace = 0;
			controlAllInputs();
		}

		private void controlAllInputs() {
			bool extractorPresent = isExtractorAppPresent();
			checkIconExtractor.Image = getCheckLabelIcon(extractorPresent);
			checkLabelExtractor.ForeColor = getCheckLabelColor(extractorPresent);


			bool enoughFreeSpace = isThereEnoughFreeSpace();
			checkIconTempSpace.Image = getCheckLabelIcon(enoughFreeSpace);
			checkLabelTempSpace.ForeColor = getCheckLabelColor(enoughFreeSpace);

			bool inputsEnabled = extractorPresent;
			arkFilePath.Enabled = inputsEnabled;
			arkBrowseButton.Enabled = inputsEnabled;
			arkGroup.Enabled = inputsEnabled;
			extractFolderPath.Enabled = inputsEnabled;
			extractBrowseButton.Enabled = inputsEnabled;
			extractGroup.Enabled = inputsEnabled;

			installButton.Enabled = extractorPresent && enoughFreeSpace && arkFilePath.Text.Length > 0 && extractFolderPath.Text.Length > 0;
		}

		private void detectSplitSecondInstall() {
			string appPATH = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
			using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(appPATH)) {
				foreach (string skName in rk.GetSubKeyNames()) {
					using (RegistryKey sk = rk.OpenSubKey(skName)) {
						try {
							var displayName = sk.GetValue("DisplayName");
							var installLocation = sk.GetValue("InstallLocation");

							if (displayName != null) {
								string item = displayName.ToString();
								if (item.Contains(regKey)) {
									if (installLocation != null) {
										string expectedPath = $@"{trimExtraBackslash(installLocation.ToString())}\{browseForFilePath}";
										if (validateArkFile(expectedPath)) {
											break;
										}
									}

								}
							}
						} catch { }
					}
				}
			}
		}

		private string trimExtraBackslash(string input) {
			return input.TrimEnd(new char[] { '\\' });
		}

		private void detectTempPath() {
			string path = "";
			try {
				path = trimExtraBackslash(Path.GetTempPath());
			} catch { }

			validateExtractPath(path);
		}

		private void installButton_Click(object sender, EventArgs e) {
			this.Hide();
			string extractorPath = Path.Combine(Directory.GetCurrentDirectory(), extractorAppPath);
			var installForm = new InstallForm(arkFilePath.Text, extractFolderPath.Text, extractorPath, extractorLock);
			installForm.Closed += (s, args) => {
				extractorLock.Close();
				this.Close();
			};
			installForm.Show();
		}
	}
}
