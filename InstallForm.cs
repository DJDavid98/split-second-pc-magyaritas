using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;

namespace SplitSecondMagyaritas {
	public partial class InstallForm : Form {
		private const string tempFolderName = "SplitSecondMagyaritas";
		private const string enFileStartsWith = "ADJUST MUSIC VOLUME";
		private const string fontFile1Contains = "These are the symbolic font names we have defined for the game.";
		private const string fontFile2Contains = "$HudFontA - EN";
		private const string fontFile3Contains = "$CCSpills - EN";
		private string arkPathUserSelection;
		private string tempPathUserSelection;
		private string extractorPathIn;

		private string tempFolder;
		private string arkBackupPath;
		private string enLangFilePath;
		private string newArkPath;

		private List<Action> rollbacks;
		private List<Action> cleanups;
		private ConcurrentQueue<string> writeQueue;
		private FileStream extractorLockIn;

		public InstallForm(string arkPath, string tempPath, string extractorPath, FileStream extractorLock) {
			InitializeComponent();
			arkPathUserSelection = arkPath;
			tempPathUserSelection = tempPath;
			extractorPathIn = extractorPath;
			extractorLockIn = extractorLock;
			rollbacks = new List<Action>();
			cleanups = new List<Action>();
			writeQueue = new ConcurrentQueue<string>();
		}

		private void InstallForm_Load(object sender, EventArgs e) {
			installWorker.RunWorkerAsync();
		}

		private void installWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
			if (e.Cancelled) {
				flushRollbacks();
			}

			installEnd(!e.Cancelled);
		}
		private void installWorker_DoWork(object sender, DoWorkEventArgs e) {
			queueOutputLine("Telepítési paraméterek:");
			queueOutputLine($"ARK fájl: {arkPathUserSelection}");
			queueOutputLine($"Ideiglenes mappa helye: {tempPathUserSelection}");
			queueOutputLine("");

			var installSteps = new List<Action>(new Action[] {
				createTempFolder,
				backupArkFile,
				() => {
					// Release lock on extractor to allow execution
					extractorLockIn.Close();
				},
				exportArkFile,
				findEnglishLanguageFile,
				overwriteWithHungarianLanguageFile,
				swapFonts,
				importArkFile,
				renameNewArkFile,
			});

			foreach (var step in installSteps) {
				step.Invoke();

				if (installWorker.CancellationPending == true) {
					e.Cancel = true;
					flushRollbacks(installWorker);
					return;
				}
			}
		}

		private void createTempFolder() {
			installWorker.ReportProgress((int)InstallProgress.START);

			tempFolder = Path.Combine(tempPathUserSelection, tempFolderName);
			queueOutputLine($"Ideiglenes mappa létrehozása: {tempFolder}");

			bool success = false;
			try {
				if (Directory.Exists(tempFolder) && Directory.GetFiles(tempFolder).Length > 0) {
					queueOutputLine("Az ideiglenes mappa már létezik és nem üres, kibontás előtt törlöm...");
					Directory.Delete(tempFolder, true);
					queueOutputLine("Az ideiglenes mappa törölve.");
				}
				Directory.CreateDirectory(tempFolder);
				success = true;
			} catch (Exception err) {
				cancelInstall("Hiba a mappa létrehozása közben", err);
				return;
			}

			if (success) {
				rollbacks.Insert(0, deleteTempFolder);
				cleanups.Insert(0, deleteTempFolder);
				installWorker.ReportProgress((int)InstallProgress.TEMP_FOLDER);
				queueOutputLine("Ideiglenes mappa sikeresen létrehozva.");
			}
		}

		private void deleteTempFolder() {
			queueOutputLine($"Ideiglenes mappa törlése: {tempFolder}");
			bool success = false;
			try {
				Directory.Delete(tempFolder, true);
				success = true;
			} catch (Exception err) {
				queueOutputLine($"Hiba a mappa törlése közben: {err.Message}\n{err.StackTrace}");
			}

			if (success) {
				queueOutputLine("Ideiglenes mappa sikeresen törölve.");
			}
		}

		private void backupArkFile() {
			arkBackupPath = Regex.Replace(arkPathUserSelection, @"\.ark$", ".ark.bak");
			queueOutputLine($"Eredeti ARK fájl mozgatása ide: {arkBackupPath }");

			bool success = false;
			bool moved = false;
			try {
				if (!File.Exists(arkBackupPath)) {
					File.Move(arkPathUserSelection, arkBackupPath);
					moved = true;
				} else {
					queueOutputLine($"Már létezik az eredeti fájl, a mozgatást átugrom.");
				}
				success = true;
			} catch (Exception err) {
				cancelInstall("Hiba a fájl mozgatása közben", err);
				return;
			}

			if (success) {
				if (moved) {
					rollbacks.Insert(0, restoreArkFile);
					queueOutputLine($"ARK fájl sikeresen átmozgatva.");
				}
				installWorker.ReportProgress((int)InstallProgress.ARK_BACKUP);
			}
		}

		private void restoreArkFile() {
			queueOutputLine($"Eredeti ARK fájl visszamozgatása ide: {arkPathUserSelection}");

			bool success = false;
			try {
				File.Move(arkBackupPath, arkPathUserSelection);
				success = true;
			} catch (Exception err) {
				queueOutputLine($"Hiba a fájl mozgatása közben: {err.Message}\n{err.StackTrace}");
			}

			if (success) {
				queueOutputLine("ARK fájl sikeresen visszamozgatva.");
			}
		}

		private void exportArkFile() {
			queueOutputLine($"ARK fájl kicsomagolása ide: {tempFolder}");
			int initialProgress = (int)InstallProgress.ARK_BACKUP;
			int targetProgress = (int)InstallProgress.ARK_EXPORT;

			StringBuilder lineBuffer = new StringBuilder();
			bool success = false;
			try {
				string processFileName = extractorPathIn;
				string processArguments = String.Join(" ", new string[] { "-e", $"\"{arkBackupPath}\"", $"\"{tempFolder}\"" });

				Process extractProcess = runProcessWithProgress(processFileName, processArguments, lineBuffer, initialProgress, targetProgress);

				extractProcess.WaitForExit();

				if (extractProcess.ExitCode != 0) {
					throw new Exception($"A kicsomagoló program visszatérési értéke ({extractProcess.ExitCode}) hibát jelöl");
				}

				success = true;
			} catch (Exception err) {
				queueOutput(lineBuffer.ToString());
				cancelInstall("Hiba a fájl kicsomagolása közben közben", err);
				return;
			}

			if (success) {
				installWorker.ReportProgress(targetProgress);
				queueOutputLine("ARK fájl sikeresen kicsomagolva.");
			}
		}

		private void findEnglishLanguageFile() {
			queueOutputLine($"Angol nyelvi fájl keresése itt: {tempFolder}");

			bool success = false;
			try {
				var files = new List<string>(Directory.GetFiles(tempFolder));
				if (files.Count == 0) {
					throw new Exception("Az ideiglenes mappa üres");
				}

				var textFiles = files.FindAll(fileName => fileName.EndsWith(".txt"));
				foreach (var textFileName in textFiles) {
					var firstLine = File.ReadLines(textFileName).First();
					queueOutputLine($"Fájl ellenőrzése: {textFileName} - Első sor: {firstLine}");
					if (firstLine.StartsWith(enFileStartsWith)) {
						enLangFilePath = textFileName;
						success = true;
						break;
					}
				}
				if (!success) {
					throw new Exception("Nem sikerült megtalálni az angol nyelvi fájlt.");
				}
			} catch (Exception err) {
				cancelInstall("Hiba az angol nyelvi fájl keresése során", err);
				return;
			}

			if (success) {
				installWorker.ReportProgress((int)InstallProgress.FIND_EN_TXT);
				queueOutputLine($"Angol nyelvi fájl megtalálva: {enLangFilePath}");
			}
		}

		private void overwriteWithHungarianLanguageFile() {
			queueOutputLine($"Angol nyelvi fájl felülírása a magyar szöveggel...");

			bool success = false;
			try {
				queueOutputLine($"Magyar szöveg beolvasása...");
				string huText = Properties.Resources.huLangFile;

				queueOutputLine($"Játék által nem támogatott karakterek cseréje...");
				huText = huText.Replace('ő', 'ô').Replace('Ő', 'Ô').Replace('ű', 'û').Replace('Ű', 'Û');

				queueOutputLine($"Végleges szöveg fájlba írása...");
				File.WriteAllText(enLangFilePath, huText, Encoding.UTF8);

				success = true;
			} catch (Exception err) {
				cancelInstall("Hiba a nyelvi fájl felülírása során", err);
				return;
			}

			if (success) {
				installWorker.ReportProgress((int)InstallProgress.COPY_HU_TXT);
				queueOutputLine($"Angol nyelvi fájl sikeresen felülírva a magyar szöveggel.");
			}
		}
		private void swapFonts() {
			queueOutputLine($"Betűtípus fájlok cseréje hosszú ő és ű betűket támogatóra...");
			int expectToFindFontFileCount = 3;
			int[] foundProgressEquivalent = new int[] { (int)InstallProgress.COPY_HU_TXT, (int)InstallProgress.SWAP_FONT_1, (int)InstallProgress.SWAP_FONT_2, (int)InstallProgress.SWAP_FONT_3 };
			int targetProgress = foundProgressEquivalent[expectToFindFontFileCount];

			int found = 0;
			bool file1Found = false;
			bool file2Found = false;
			bool file3Found = false;
			try {
				var files = new List<string>(Directory.GetFiles(tempFolder));
				if (files.Count == 0) {
					throw new Exception("Az ideiglenes mappa üres");
				}
				double totalFileCount = Convert.ToDouble(files.Count);
				double fileCounter = 0.0;

				var datFiles = files.FindAll(fileName => fileName.EndsWith(".dat"));
				var fontFile1ContainsBytes = Encoding.ASCII.GetBytes(fontFile1Contains);
				var fontFile2ContainsBytes = Encoding.ASCII.GetBytes(fontFile2Contains);
				var fontFile3ContainsBytes = Encoding.ASCII.GetBytes(fontFile3Contains);
				foreach (var datFileName in datFiles) {
					if (found == expectToFindFontFileCount) {
						break;
					}

					byte[] first3Bytes = new byte[3];
					var fileHandle = File.OpenRead(datFileName);
					fileHandle.Read(first3Bytes, 0, 3);
					fileHandle.Close();
					var first3Chars = Encoding.ASCII.GetString(first3Bytes);
					if (first3Chars != "GFX") continue;
					queueOutputLine($"Fájl ellenőrzése: {datFileName} - Lehetséges találat, első 3 karakter: {first3Chars}");

					var fileContents = File.ReadAllBytes(datFileName);
					var fontFileFound = false;
					string whichFileWasFound = "valamelyik";
					if (!file1Found && sequenceContains(fileContents, fontFile1ContainsBytes)) {
						File.WriteAllBytes(datFileName, Properties.Resources.fonts1);
						whichFileWasFound = "az első";
						file1Found = true;
						fontFileFound = true;
					}
					else if (!file2Found && sequenceContains(fileContents, fontFile2ContainsBytes)) {
						File.WriteAllBytes(datFileName, Properties.Resources.fonts2);
						whichFileWasFound = "a második";
						file2Found = true;
						fontFileFound = true;
					}
					else if (!file3Found && sequenceContains(fileContents, fontFile3ContainsBytes)) {
						File.WriteAllBytes(datFileName, Properties.Resources.fonts2);
						whichFileWasFound = "a harmadik";
						file3Found = true;
						fontFileFound = true;
					}

					if (fontFileFound) {
						found++;
						queueOutputLine($"Fájl {datFileName} felülírva {whichFileWasFound} betűtípus fájllal ({found}/{expectToFindFontFileCount})");
					}

					var baseProgress = foundProgressEquivalent[found];
					installWorker.ReportProgress(Convert.ToInt32(baseProgress + ((targetProgress - baseProgress) * (fileCounter / totalFileCount))));
					fileCounter++;
				}

				if (found < expectToFindFontFileCount) {
					throw new Exception("Nem sikerült megtalálni az összes várt betűtípust.");
				}

				if (found > expectToFindFontFileCount) {
					throw new Exception("A vártnál több betűtípus fájl került megtalálásra.");
				}
			} catch (Exception err) {
				cancelInstall("Hiba a betűtípus fájlok keresése során", err);
				return;
			}

			installWorker.ReportProgress(foundProgressEquivalent[found]);
			queueOutputLine("Betűtípus fájlok sikeresen lecserélve.");
		}
		private void importArkFile() {
			queueOutputLine("ARK fájl tartalmának visszacsomagolása...");
			int initialProgress = (int)InstallProgress.SWAP_FONT_3;
			int targetProgress = (int)InstallProgress.ARK_IMPORT;

			StringBuilder lineBuffer = new StringBuilder();
			bool success = false;
			try {
				deleteOriginalArkFileIfNeeded();

				string processFileName = extractorPathIn;
				string processArguments = String.Join(" ", new string[] { "-i", $"\"{arkBackupPath}\"", $"\"{tempFolder}\"" });

				Process importProcess = runProcessWithProgress(processFileName, processArguments, lineBuffer, initialProgress, targetProgress);

				importProcess.WaitForExit();

				if (importProcess.ExitCode != 0) {
					throw new Exception($"A visszacsomagoló program visszatérési értéke ({importProcess.ExitCode}) hibát jelöl");
				}

				newArkPath = $"{arkBackupPath}.NEW";
				if (!File.Exists(newArkPath)) {
					throw new Exception($"Az elvárt új ARK fájl ({newArkPath}) nem jött létre");
				}

				success = true;
			} catch (Exception err) {
				queueOutput(lineBuffer.ToString());
				cancelInstall("Hiba a fájl visszacsomagolása közben közben", err);
				return;
			}

			if (success) {
				installWorker.ReportProgress(targetProgress);
				queueOutputLine("ARK fájl sikeresen visszacsomagolva.");
			}
		}
		private void renameNewArkFile() {
			queueOutputLine($"Újonnan létrejött ARK fájl visszanevezése...");

			bool success = false;
			try {
				deleteOriginalArkFileIfNeeded();

				File.Move(newArkPath, arkPathUserSelection);

				success = true;
			} catch (Exception err) {
				cancelInstall("Hiba az új ARK fájl átnevezése során", err);
				return;
			}

			if (success) {
				installWorker.ReportProgress((int)InstallProgress.NEW_ARK_RENAME);
				queueOutputLine($"Új ARK fájl sikeresen átnevezve.");
			}
		}

		private void deleteOriginalArkFileIfNeeded() {
			if (File.Exists(arkPathUserSelection)) {
				queueOutputLine("Az eredeti ARK fájl még létezik, törlöm...");
				File.Delete(arkPathUserSelection);
				queueOutputLine("Az eredeti ARK fájl törölve.");
			}
		}

		private void flushRollbacks(BackgroundWorker worker) {
			worker.ReportProgress((int)InstallProgress.START);
			flushRollbacks(true);
		}

		private void flushRollbacks(bool worker = false) {
			if (rollbacks.Count > 0) {
				queueOutputLine("Módosítások visszavonása...");
				rollbacks.ForEach(rollback => rollback.Invoke());
				rollbacks.Clear();
				if (!worker) {
					installProgress.Value = (int)InstallProgress.START;
				}
				queueOutputLine("A telepítés a megszakadt.");
			}
		}

		private void flushCleanups() {
			if (cleanups.Count > 0) {
				queueOutputLine("Telepítés utáni takarítás...");
				cleanups.ForEach(cleanup => cleanup.Invoke());
				cleanups.Clear();
			}
		}

		private void queueOutputLine(string line) {
			queueOutput(line + "\r\n");
		}

		private void queueOutput(string text) {
			writeQueue.Enqueue(text);
		}

		private void writeInstallOutput(string text) {
			installOutput.AppendText(text);
			installOutput.ScrollToCaret();
		}

		private void flushWrittenOutput() {
			string result;
			while (writeQueue.TryDequeue(out result)) writeInstallOutput(result);
		}

		private void updateProgress(object sender, ProgressChangedEventArgs e) {
			updateProgress(e.ProgressPercentage);
		}
		private void updateProgress(int percent) {
			installProgress.Value = percent;
			flushWrittenOutput();
		}

		private void cancelButton_Click(object sender, EventArgs e) {
			if (!cancelInstall()) {
				Close();
			}
		}

		private bool cancelInstall(string actionDescription, Exception err) {
			queueOutputLine($"{actionDescription}: {err.Message}\n{err.StackTrace}");
			return cancelInstall();
		}

		private bool cancelInstall() {
			if (!installWorker.IsBusy) return false;
			installWorker.ReportProgress(100);
			installWorker.CancelAsync();
			queueOutputLine("A telepítés megszakítása...");
			return true;
		}

		private void installEnd(bool success) {
			updateProgress((int)InstallProgress.NEW_ARK_RENAME);
			cancelButton.Text = "Bezárás (Esc)";
			string clearScreenNewLines = string.Concat(Enumerable.Repeat("\r\n", 1));
			if (success) {
				cancelButton.ForeColor = Color.LimeGreen;

				// Job's done
				flushCleanups();
				flushWrittenOutput();

				writeInstallOutput(String.Join("\r\n", new string[] {
						"A magyarítás sikeresen telepítve lett.",
						"",
						$"Ha el szeretnéd távolítani a magyarítást, egyszerűen csak töröld {mapArticle(arkPathUserSelection[0])} {arkPathUserSelection} fájlt, " +
						$"majd nevezd vissza {mapArticle(arkBackupPath[0])} {arkBackupPath} fájlt {arkPathUserSelection}-ra.",
						""
				}));
			} else {
				cancelButton.ForeColor = Color.Red;
				writeInstallOutput("A telepítés megszakadt.\r\n");
			}
			installOutput.ForeColor = cancelButton.ForeColor;
		}

		private Process runProcessWithProgress(string processFileName, string processArguments, StringBuilder lineBuffer, int initialProgress, int targetProgress) {
			queueOutputLine($"Külső parancs futtatása: {processFileName} {processArguments}\n");
			Process externalProcess = new Process();
			externalProcess.StartInfo.FileName = processFileName;
			externalProcess.StartInfo.Arguments = processArguments;
			externalProcess.StartInfo.UseShellExecute = false;
			externalProcess.StartInfo.RedirectStandardOutput = true;
			externalProcess.StartInfo.CreateNoWindow = true;
			externalProcess.Start();

			using (StreamReader reader = externalProcess.StandardOutput) {
				while (!reader.EndOfStream) {
					var line = reader.ReadLine();
					lineBuffer.AppendLine(line);

					// Attempt to parse progress from ARK tool command line output
					string pattern = @"\s(\d+)\|(\d+)";
					if (Regex.IsMatch(line, pattern)) {
						Match match = Regex.Match(line, pattern);
						double currentFileIndex = Convert.ToDouble(match.Groups[1].Captures[0].Value);
						double fileCount = Convert.ToDouble(match.Groups[2].Captures[0].Value);
						if (fileCount > 0) {
							installWorker.ReportProgress(initialProgress + Convert.ToInt32((targetProgress - initialProgress) * (currentFileIndex / fileCount)));
						}
					}

					if (installWorker.CancellationPending == true) {
						externalProcess.Close();
						break;
					}
				}
			}

			return externalProcess;
		}

		private string mapArticle(char firstLetter) {
			switch (firstLetter) {
				case 'A':
				case 'E':
				case 'F':
				case 'I':
				case 'L':
				case 'M':
				case 'N':
				case 'O':
				case 'R':
				case 'S':
				case 'U':
				case 'X':
				case 'Y':
					return "az";
				default:
					return "a";
			}
		}

		public bool sequenceContains(byte[] haystack, byte[] needle) {
			var matchLength = 0;
			var requiredMatchLength = needle.Length - 1;
			for (int i = 0; i < haystack.Length; i++) {
				if (matchLength < needle.Length && haystack[i] == needle[matchLength]) {
					if (matchLength == requiredMatchLength) {
						queueOutputLine($"Teljes egyezés (hossz: {matchLength}/{requiredMatchLength})");
						return true;
					}
					else if (matchLength >= 5 && matchLength % 5 == 0) {
						string matchText = Encoding.ASCII.GetString(haystack.Skip(i - matchLength).Take(matchLength).ToArray());
						queueOutputLine($"Részleges egyezés: {matchText} (hossz: {matchLength}/{requiredMatchLength})");
					}
					matchLength++;
					continue;
				}
				matchLength = 0;
			}
			return false;
		}
	}

	public enum InstallProgress {
		START = 0,
		TEMP_FOLDER = 2,
		ARK_BACKUP = 4,
		ARK_EXPORT = 48,
		FIND_EN_TXT = 49,
		COPY_HU_TXT = 50,
		SWAP_FONT_1 = 52,
		SWAP_FONT_2 = 54,
		SWAP_FONT_3 = 56,
		ARK_IMPORT = 99,
		NEW_ARK_RENAME = 100
	}
}
