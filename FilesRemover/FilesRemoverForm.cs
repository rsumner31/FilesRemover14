using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
mespace FilesRemover
{
    public partial class FilesRemoverForm : Form
    {
<<<        ILogger logger;

        private Stopwatch _stopwatch;
        private FilesRemoverModel _filesRemoverModel;
        private FilesRemover _filesRemover;
>>>>>>>+HEAD
====
           ILogger messageBoxLogger;

        private DateTime _borderDate;
        private Stopwatch _stopwatch;

        private IEnumerable<FileData> allFiles;
        private IEnumerable<string> allDirectories;

        private bool copyAndDeleteFiles;
        private bool deleteEmptyDirectories;

        private int maxValueProgressBar;

        private int allDirectoriesCount;
        private int allFilesCount;

        private bool overrideFiles;
>>>>>>>-dbb3e01
     public FilesRemoverForm()
        {
            InitializeComponent();

<<<<            // Logi do MessageBox-a
            logger = new MessageBoxLogger();

            // Ustawiane wszystkie opcje
            _filesRemoverModel = new FilesRemoverModel();
     
            // padding okienka wyświetlającego logi
            logBox.SelectionTabs = new[] {0, 800};
        }

        private void FilesRemoverForm_Load(object sender, EventArgs e)
        {
            // Bind textboxes - source and destination path
            sourcePathTextBox.DataBindings.Add("Text", _filesRemoverModel, "SourcePath", true, DataSourceUpdateMode.OnPropertyChanged);
            destinationPathTextBox.DataBindings.Add("Text", _filesRemoverModel, "DestinationPath", true, DataSourceUpdateMode.OnPropertyChanged);

            // Bind checkbox - overrideFiles
            overrideFilesCheckBox.DataBindings.Add("Checked", _filesRemoverModel, "OverrideFiles", true, DataSourceUpdateMode.OnPropertyChanged);

            // Aktualizacja daty granicznej
            UpdateBorderDate((int)numberOfWeeks.Value);
>>>>>>>+HEAD
=
                  messageBoxLogger = new MessageBoxLogger();

            // padding okienka wyświetlającego logi
            logBox.SelectionTabs = new[] {0, 800};

            UpdateAll();
>>>>>>>-dbb3e01


        #region Events

        {
            _filesRemoverModel.SourcePath = ShowFolderDialog(_filesRemoverModel.SourcePath);
        }

        private void destinationPathDialog_Click(object sender, EventArgs e)
        {
            _filesRemoverModel.DestinationPath = ShowFolderDialog(_filesRemoverModel.DestinationPath);
>>>>>>>+HEAD
       p        private void button1_Click(object sender, EventArgs e)
        {
            ShowFolderDialog(sourcePathTextBox);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ShowFolderDialog(destinationPathTextBox);
>>>>>>>-dbb3e01

        private void startButton_Click(object sender, EventArgs e)
        {
            ChangeEnableControls(false);
            logBox.ResetText();

>>>>>>>+HEAD
                    //czyszczenie pola z logiem operacji i aktualizacja daty granicznej
            ChangeEnableControls(false);
            logBox.ResetText();

            UpdateAll();
>>>>>>>-dbb3e01
 InitializeProgressBar();

            if (IsOptionsValid())
            {
<<<<<<< HEA                _filesRemover = new FilesRemover(_filesRemoverModel, logger, logBox, progressBar);

                UseWaitCursor = true;
                Application.DoEvents();

                StartTimer();
                _filesRemover.Start(() =>
                {
                    StopTimer();
                    UseWaitCursor = false;
                    ChangeEnableControls(true);
                    logger.Log("Zakończono!");
                });
            }
            else
            {
                ChangeEnableControls(true);
            }          
>>>>>>>+HEAD
                        StartTimer();
                UseWaitCursor = true;
                Application.DoEvents();

                logBox.Font = new System.Drawing.Font("Arial", 30);
                logBox.Text = "Inicjalizacja...";

                var jobsWorker = new System.ComponentModel.BackgroundWorker();
                jobsWorker.DoWork += (ss, ee) => DoJobs();
                jobsWorker.RunWorkerCompleted += (ss, ee) => { StopTimer(); SaveLogToFile(); ChangeEnableControls(true); allFiles = null; allDirectories = null; };

                var fileInfoWorker = new System.ComponentModel.BackgroundWorker();
                fileInfoWorker.DoWork += (ss, ee) => { CollectInfoForJobs(); };
                fileInfoWorker.RunWorkerCompleted += (ss, ee) =>
                {
                    progressBar.Maximum = maxValueProgressBar;
                    UseWaitCursor = false;
                    logBox.ResetText(); logBox.Font = new System.Drawing.Font("Arial", 7);
                    jobsWorker.RunWorkerAsync();
                };

                fileInfoWorker.RunWorkerAsync();
            }
            else
                ChangeEnableControls(true);
>>>>>>>-dbb3e01
      private void changePaddingButton_Click_1(object sender, EventArgs e)
        {
            int padding = int.Parse(paddingValue.Text);
            string text = logBox.Text;

            logBox.Clear();
            logBox.SelectionTabs = new[] {0, padding};
            logBox.AppendText(text);
        }

        private void numberOfWeeks_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown numberOfWeeks = sender as NumericUpDown;

<<<<<<< HEAD
            UpdateBorderDate((int)numberOfWeeks.Value);
>>>>>>>+HEAD
       i            int days = Convert.ToInt32(numberOfWeeks.Value)*7;
            _borderDate = DateTime.Now;
            _borderDate = _borderDate.Subtract(TimeSpan.FromDays(days));
            borderDateLabel.Text = _borderDate.ToShortDateString();
>>>>>>>-dbb3e01
     private void deleteCopyFilesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
<<<<<<< HEAD
             _filesRemoverModel.CopyAndDeleteFiles = (sender as CheckBox).Checked;
            AnyJobSelected();
        }

        private void deleteEmptyDirectoriesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _filesRemoverModel.DeleteEmptyDirectories = (sender as CheckBox).Checked;
            AnyJobSelected();
        }
       
        private void swapPathButton_Click(object sender, EventArgs e)
        {
            string tmp = _filesRemoverModel.SourcePath;
            _filesRemoverModel.SourcePath = _filesRemoverModel.DestinationPath;
            _filesRemoverModel.DestinationPath = tmp;
        }

        private void aboutProgram_Click(object sender, EventArgs e)
        {
            Form aboutForm = AboutProgram.GetInstance();

            if (!aboutForm.Visible)
            {
                aboutForm.Show();
            }
            else
            {
                aboutForm.BringToFront();
            }
        }
>>>>>>>+HEAD
      An            AnyJobSelected();
        }

        private void deleteDirectoriesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            AnyJobSelected();
        }
>>>>>>>-dbb3e01
gion

        #region HelperMethods

        private void AnyJobSelected()
        {
<<<<<<< HEAD
              if (!_filesRemoverModel.CopyAndDeleteFiles && !_filesRemoverModel.DeleteEmptyDirectories)
>>>>>>>+HEAD
     if             if (!deleteCopyFilesCheckBox.Checked && !deleteDirectoriesCheckBox.Checked)
>>>>>>>-dbb3e01
startButton.Enabled = false;
            else
                startButton.Enabled = true;
        }

<<<<<<< HEAD
           public void ChangeEnableControls(bool value)
        {
            startButton.Enabled = value;
            deleteCopyFilesCheckBox.Enabled = value;
            deleteEmptyDirectoriesCheckBox.Enabled = value;
>>>>>>>+HEAD
private         private bool CheckStartEndPath()
        {
            if (!Directory.Exists(sourcePathTextBox.Text))
            {
                return false;
            }
            if (!Directory.Exists(destinationPathTextBox.Text))
            {
                return false;
            }

            return true;
        }

        private void ChangeEnableControls(bool value)
        {
            startButton.Enabled = value;
            deleteCopyFilesCheckBox.Enabled = value;
            deleteDirectoriesCheckBox.Enabled = value;
>>>>>>>-dbb3e01
rOfWeeks.Enabled = value;
            sourcePathTextBox.Enabled = value;
            destinationPathTextBox.Enabled = value;
            changePaddingButton.Enabled = value;
            paddingValue.Enabled = value;
            overrideFilesCheckBox.Enabled = value;
        }

<<<<<<< HEAD
            private void InitializeProgressBar()
        { 
            progressBar.Value = 0;
            progressBar.Step = 1;
            progressBar.Maximum = 1;
            progressBar.ForeColor = System.Drawing.Color.Yellow;
        }

        private bool IsOptionsValid()
        {
            //sprawdzanie poprawności scieżki początkowej i końcowej
            if (!Directory.Exists(_filesRemoverModel.SourcePath))
            {
                logger.Log("Podana ścieżka początkowa nie istnieje!");    
                return false;
            }

            if (_filesRemoverModel.CopyAndDeleteFiles && !Directory.Exists(_filesRemoverModel.DestinationPath))
            {
                logger.Log("Podana ścieżka końcowa nie istnieje!");
                return false;
            }

            if (_filesRemoverModel.CopyAndDeleteFiles && Path.GetFullPath(_filesRemoverModel.SourcePath) == Path.GetFullPath(_filesRemoverModel.DestinationPath))
            {
                logger.Log("Ścieżka początkowa i końcowa nie może być taka sama!");
                return false;
            }

            return true;
        }

        private string ShowFolderDialog(string currentPath)
        {
            string resultPath = currentPath;
            var folderDialog = new FolderBrowserDialog();
            
            folderDialog.Reset();
            folderDialog.SelectedPath = currentPath;
>>>>>>>+HEAD
ectFiles        private void CollectFilesInfo(string path)
        {
            try
            {
                allFiles = FastDirectoryEnumerator.EnumerateFiles(path, "*", SearchOption.AllDirectories);
                allFilesCount = allFiles.Count();
                maxValueProgressBar += allFilesCount;
            }
            catch (Exception ex)
            {
                messageBoxLogger.Log(ex.Message);
                return;
            }
        }

        private void CollectDirectoriesInfo(string path)
        {
            try
            {
                allDirectories = Directory.EnumerateDirectories(path, "*", SearchOption.AllDirectories);
                allDirectoriesCount = allDirectories.Count();
                maxValueProgressBar += allDirectoriesCount;
            }
            catch (Exception ex)
            {
                messageBoxLogger.Log(ex.Message);
                return;
            }
        }

        private void CopyAndRemoveUnusedFiles()
        {
            logBox.Invoke(new MethodInvoker(delegate {
                logBox.AppendText($"Liczba wszystkich plików: {allFilesCount} \n\n");
                logBox.AppendText("Pełna scieżka pliku \t Czas ostatniego dostępu do pliku \n");
            }));

            List<string> errors = new List<string>();
            int count = 0;
            
            foreach (FileData file in allFiles)
            {
                var lastAccessDate = file.LastAccesTime;
                if (lastAccessDate <= _borderDate)
                {
                    string fileName = Path.GetFileName(file.Path);
                    try
                    {
                        string copiedFilePath = $@"{destinationPathTextBox.Text}\{fileName}";
                        if (File.Exists(copiedFilePath))
                        {
                            copiedFilePath = copiedFilePath.Insert(copiedFilePath.LastIndexOf('.'), "_copy");
                        }
                        File.Copy(file.Path, copiedFilePath, overrideFiles);
                        File.SetLastAccessTime(copiedFilePath, lastAccessDate);
                        File.Delete(file.Path);

                        logBox.Invoke(new MethodInvoker(delegate { logBox.AppendText(file.Path + " \t" + lastAccessDate + "\n"); }));

                        count++;
                    }
                    catch (Exception ex)
                    {
                        errors.Add(ex.Message);
                    }
                }

                Application.DoEvents();
                progressBar.Invoke(new MethodInvoker(delegate { progressBar.PerformStep(); }));
            }

            logBox.Invoke(new MethodInvoker(delegate
            {
                logBox.Text = $"Liczba skopiowanych plików: {count}\n" + logBox.Text;
                if (errors.Count != 0)
                    logBox.AppendText("\n\n " + string.Join("\n", errors.ToArray()));
            }));
        }

        private void DeleteEmptyDirectories()
        {
            List<string> errors = new List<string>();
            int count = 0;

            logBox.Invoke(new MethodInvoker(delegate
            {
                logBox.AppendText($"Liczba wszystkich katalogów: {allDirectoriesCount} \n");
            }));

            foreach (string directory in allDirectories)
            {
                if (IsDirectoryEmpty(directory))
                {
                    try
                    {
                        Directory.Delete(directory);
                        logBox.Invoke(new MethodInvoker(delegate { logBox.AppendText(directory + " \n"); }));
                        count++;
                    }
                    catch (Exception ex)
                    {
                        errors.Add(ex.Message);
                    }
                }
                progressBar.Invoke(new MethodInvoker(delegate { progressBar.PerformStep(); }));
            }

            logBox.Invoke(new MethodInvoker(delegate { logBox.AppendText($"\nLiczba usuniętych katalogów: {count}"); }));

            if (errors.Count != 0)
                logBox.Invoke(new MethodInvoker(delegate { logBox.AppendText("\nBłędy: \n\n" + string.Join("\n", errors.ToArray())); }));
        }

        private void DoJobs()
        {      
            if (allFiles != null)
                CopyAndRemoveUnusedFiles();

            if (allDirectories != null)
                DeleteEmptyDirectories();

            progressBar.Invoke(new MethodInvoker(delegate { progressBar.PerformStep(); }));
        }

        private void CollectInfoForJobs()
        {
            if (copyAndDeleteFiles)
                CollectFilesInfo(sourcePathTextBox.Text);
            if (deleteEmptyDirectories)
                CollectDirectoriesInfo(sourcePathTextBox.Text);
        }

        private void InitializeProgressBar()
        {
            maxValueProgressBar = 1;

            progressBar.Value = 0;
            progressBar.Step = 1;
            progressBar.Maximum = maxValueProgressBar;
            progressBar.ForeColor = System.Drawing.Color.Yellow;
        }

        private void UpdateAll()
        {
            UpdateBorderDate();
            UpdateJobOptions();
        }

        private void UpdateBorderDate()
        {
            int days = Convert.ToInt32(numberOfWeeks.Value) * 7;

            _borderDate = DateTime.Now;
            _borderDate = _borderDate.Subtract(TimeSpan.FromDays(days));
            borderDateLabel.Text = _borderDate.ToShortDateString();
        }

        private void UpdateJobOptions()
        {
            copyAndDeleteFiles = deleteCopyFilesCheckBox.Checked;
            deleteEmptyDirectories = deleteDirectoriesCheckBox.Checked;
            overrideFiles = overrideFilesCheckBox.Checked;
        }

        private bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        private bool IsOptionsValid()
        {
            //sprawdzanie poprawności scieżki początkowej i końcowej
            if (!Directory.Exists(sourcePathTextBox.Text))
            {
                messageBoxLogger.Log("Podana ścieżka początkowa nie istnieje!");    
                return false;
            }

            if (!Directory.Exists(destinationPathTextBox.Text))
            {
                messageBoxLogger.Log("Podana ścieżka końcowa nie istnieje!");
                return false;
            }

            if (Path.GetFullPath(sourcePathTextBox.Text) == Path.GetFullPath(destinationPathTextBox.Text))
            {
                messageBoxLogger.Log("Ścieżka początkowa i końcowa nie może być taka sama!");
                return false;
            }

            return CheckStartEndPath();
        }

        private void SaveLogToFile()
        {
            DateTime dateTime = DateTime.Now;
            string logFileName = dateTime.ToString("yy-MM-dd") + "_godz_" + dateTime.ToString("HH-mm-ss");
            string path = Directory.GetCurrentDirectory() + $@"\{logFileName}.txt";

            try
            {
                logBox.SaveFile(path, RichTextBoxStreamType.PlainText);
            }
            catch (Exception ex)
            {
                messageBoxLogger.Log(path + "\n" + ex.Message);
            }
        }

        private void ShowFolderDialog(TextBox textBox)
        {
            var folderDialog = new FolderBrowserDialog();
            string prevpath = folderDialog.SelectedPath;
            folderDialog.Reset();
            folderDialog.SelectedPath = textBox.Text;
>>>>>>>-dbb3e01
      DialogResult dr = folderDialog.ShowDialog();
            if (dr == DialogResult.OK || dr == DialogResult.Yes)
            {
<<<<<<< HEAD
                resultPath = folderDialog.Selec                resultPath = folderDialog.SelectedPath;
            }

            return resultPath;
>>>>>>>+HEAD
Path;
                  textBox.Text = folderDialog.SelectedPath;
            }
            folderDialog.SelectedPath = prevpath;
>>>>>>>-dbb3e01
      _stopwatch = new Stopwatch();

            startTimeLabel.Visible = true;
            endTimeLabel.Visible = false;
            durationLabel.Visible = false;

            _stopwatch.Start();

            startTimeLabel.Text = "Czas rozpoczęcia: " + DateTime.Now.ToString("HH:mm:ss");
            Application.DoEvents();
        }

        private void StopTimer()
        {
            _stopwatch.Stop();

            endTimeLabel.Text = "Czas ukończenia: " + DateTime.Now.ToString("HH:mm:ss");
            endTimeLabel.Visible = true;

            durationLabel.Text = "Czas trwania: " + _stopwatch.Elapsed.Seconds + " sek";
            durationLabel.Visible = true;
        }

<<<<<<< HEAD
        private void UpdateBorderDate(int numberOf        private void UpdateBorderDate(int numberOfWeeksBack)
        {
            int days = numberOfWeeksBack * 7;
            _filesRemoverModel.BorderDate = DateTime.Now;
            _filesRemoverModel.BorderDate = _filesRemoverModel.BorderDate.Subtract(TimeSpan.FromDays(days));

            borderDateLabel.Text = _filesRemoverModel.BorderDate.ToShortDateString();
        }

        #endregion
>>>>>>>+HEAD
object sender, EventArgs e)
        {
            string tmp = sourcePathTextBox.Text;
            sourcePathTextBox.Text = destinationPathTextBox.Text;
            destinationPathTextBox.Text = tmp;
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
>>>>>>> dbb3e01... Dodaj pliki projektów.
    }
}mespace FilesRemover
{
    public partial class FilesRemoverForm : Form
    {
<<<        ILogger logger;

        private Stopwatch _stopwatch;
        private FilesRemoverModel _filesRemoverModel;
        private FilesRemover _filesRemover;
>>>>>>>+HEAD
====
           ILogger messageBoxLogger;

        private Stopwatch _stopwatch;

        private IEnumerable<FileData> allFiles;
        private IEnumerable<string> allDirectories;

        private int maxValueProgressBar;

        private int allDirectoriesCount;
        private int allFilesCount;

<<<<<<< master
        private bool overrideFiles;
>>>>>>>-dbb3e01
     public FilesRemoverForm()
        {
            InitializeComponent();

<<<<            // Logi do MessageBox-a
            logger = new MessageBoxLogger();

            // Ustawiane wszystkie opcje
            _filesRemoverModel = new FilesRemoverModel();
     
            // padding okienka wyświetlającego logi
            logBox.SelectionTabs = new[] {0, 800};
        }

        private void FilesRemoverForm_Load(object sender, EventArgs e)
        {
            // Bind textboxes - source and destination path
            sourcePathTextBox.DataBindings.Add("Text", _filesRemoverModel, "SourcePath", true, DataSourceUpdateMode.OnPropertyChanged);
            destinationPathTextBox.DataBindings.Add("Text", _filesRemoverModel, "DestinationPath", true, DataSourceUpdateMode.OnPropertyChanged);

            // Bind checkbox - overrideFiles
            overrideFilesCheckBox.DataBindings.Add("Checked", _filesRemoverModel, "OverrideFiles", true, DataSourceUpdateMode.OnPropertyChanged);

            // Aktualizacja daty granicznej
            UpdateBorderDate((int)numberOfWeeks.Value);
>>>>>>>+HEAD
=
                  messageBoxLogger = new MessageBoxLogger();

            // padding okienka wyświetlającego logi
            logBox.SelectionTabs = new[] {0, 800};

            UpdateAll();
>>>>>>>-dbb3e01


        #region Events

<<<<<<< H        private void sourcePathDialog_Click(object sender, EventArgs e)
        {
            _filesRemoverModel.SourcePath = ShowFolderDialog(_filesRemoverModel.SourcePath);
        }

        private void destinationPathDialog_Click(object sender, EventArgs e)
        {
            _filesRemoverModel.DestinationPath = ShowFolderDialog(_filesRemoverModel.DestinationPath);
>>>>>>>+HEAD
       p        private void button1_Click(object sender, EventArgs e)
        {
            filesRemoverModel.SourcePath = ShowFolderDialog(filesRemoverModel.SourcePath);
        }

        private void destinationPathDialog_Click(object sender, EventArgs e)
        {
            ShowFolderDialog(destinationPathTextBox);
>>>>>>>-dbb3e01

        private void startButton_Click(object sender, EventArgs e)
        {
            ChangeEnableControls(false);
            logBox.ResetText();

>>>>>>>+HEAD
                    //czyszczenie pola z logiem operacji i aktualizacja daty granicznej
            ChangeEnableControls(false);
            logBox.ResetText();

<<<<<<< master
            UpdateAll();
>>>>>>>-dbb3e01
 InitializeProgressBar();

            if (IsOptionsValid())
            {

                UseWaitCursor = true;
                Application.DoEvents();

                StartTimer();
                _filesRemover.Start(() =>
                {
                    StopTimer();
                    UseWaitCursor = false;
                    ChangeEnableControls(true);
                    logger.Log("Zakończono!");
                });
            }
            else
            {
                ChangeEnableControls(true);
            }          
>>>>>>>+HEAD
                        StartTimer();
                UseWaitCursor = true;
                Application.DoEvents();

                logBox.Font = new System.Drawing.Font("Arial", 30);
                logBox.Text = "Inicjalizacja...";

                var jobsWorker = new System.ComponentModel.BackgroundWorker();
                jobsWorker.DoWork += (ss, ee) => DoJobs();
                jobsWorker.RunWorkerCompleted += (ss, ee) => { StopTimer(); SaveLogToFile(); ChangeEnableControls(true); allFiles = null; allDirectories = null; };

                var fileInfoWorker = new System.ComponentModel.BackgroundWorker();
                fileInfoWorker.DoWork += (ss, ee) => { CollectInfoForJobs(); };
                fileInfoWorker.RunWorkerCompleted += (ss, ee) =>
                {
                    progressBar.Maximum = maxValueProgressBar;
                    UseWaitCursor = false;
                    logBox.ResetText(); logBox.Font = new System.Drawing.Font("Arial", 7);
                    jobsWorker.RunWorkerAsync();
                };

                fileInfoWorker.RunWorkerAsync();
            }
            else
                ChangeEnableControls(true);
>>>>>>>-dbb3e01
      private void changePaddingButton_Click_1(object sender, EventArgs e)
        {
            int padding = int.Parse(paddingValue.Text);
            string text = logBox.Text;

            logBox.Clear();
            logBox.SelectionTabs = new[] {0, padding};
            logBox.AppendText(text);
        }

        private void numberOfWeeks_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown numberOfWeeks = sender as NumericUpDown;

<<<<<<< master
<<<<<<< HEAD
            UpdateBorderDate((int)numberOfWeeks.Value);
>>>>>>>+HEAD
       i            int days = Convert.ToInt32(numberOfWeeks.Value)*7;
            _borderDate = DateTime.Now;
            _borderDate = _borderDate.Subtract(TimeSpan.FromDays(days));
            borderDateLabel.Text = _borderDate.ToShortDateString();
>>>>>>>-dbb3e01
     private void deleteCopyFilesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
<<<<<<< HEAD
             _filesRemoverModel.CopyAndDeleteFiles = (sender as CheckBox).Checked;
            AnyJobSelected();
        }

        private void deleteEmptyDirectoriesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _filesRemoverModel.DeleteEmptyDirectories = (sender as CheckBox).Checked;
            AnyJobSelected();
        }
       
        private void swapPathButton_Click(object sender, EventArgs e)
        {
            string tmp = _filesRemoverModel.SourcePath;
            _filesRemoverModel.SourcePath = _filesRemoverModel.DestinationPath;
            _filesRemoverModel.DestinationPath = tmp;
        }

        private void aboutProgram_Click(object sender, EventArgs e)
        {
            Form aboutForm = AboutProgram.GetInstance();

            if (!aboutForm.Visible)
            {
                aboutForm.Show();
            }
            else
            {
                aboutForm.BringToFront();
            }
        }
>>>>>>>+HEAD
      An            AnyJobSelected();
        }

        private void deleteEmptyDirectoriesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filesRemoverModel.DeleteEmptyDirectories = (sender as CheckBox).Checked;
            AnyJobSelected();
        }
>>>>>>>-dbb3e01
gion

        #region HelperMethods

        private void AnyJobSelected()
        {
<<<<<<< HEAD
              if (!_filesRemoverModel.CopyAndDeleteFiles && !_filesRemoverModel.DeleteEmptyDirectories)
>>>>>>>+HEAD
     if             if (!deleteCopyFilesCheckBox.Checked && !deleteDirectoriesCheckBox.Checked)
>>>>>>>-dbb3e01
startButton.Enabled = false;
            else
                startButton.Enabled = true;
        }

<<<<<<< HEAD
           public void ChangeEnableControls(bool value)
        {
            startButton.Enabled = value;
            deleteCopyFilesCheckBox.Enabled = value;
            deleteEmptyDirectoriesCheckBox.Enabled = value;
>>>>>>>+HEAD
private         private bool CheckStartEndPath()
        {
            if (!Directory.Exists(sourcePathTextBox.Text))
            {
                return false;
            }
            if (!Directory.Exists(destinationPathTextBox.Text))
            {
                return false;
            }

            return true;
        }

        private void ChangeEnableControls(bool value)
        {
            startButton.Enabled = value;
            deleteCopyFilesCheckBox.Enabled = value;
            deleteDirectoriesCheckBox.Enabled = value;
>>>>>>>-dbb3e01
rOfWeeks.Enabled = value;
            sourcePathTextBox.Enabled = value;
            destinationPathTextBox.Enabled = value;
            changePaddingButton.Enabled = value;
            paddingValue.Enabled = value;
            overrideFilesCheckBox.Enabled = value;
        }

            private void InitializeProgressBar()
        { 
            progressBar.Value = 0;
            progressBar.Step = 1;
            progressBar.Maximum = 1;
            progressBar.ForeColor = System.Drawing.Color.Yellow;
        }

        private bool IsOptionsValid()
        {
            //sprawdzanie poprawności scieżki początkowej i końcowej
            if (!Directory.Exists(_filesRemoverModel.SourcePath))
            {
                logger.Log("Podana ścieżka początkowa nie istnieje!");    
                return false;
            }

            if (_filesRemoverModel.CopyAndDeleteFiles && !Directory.Exists(_filesRemoverModel.DestinationPath))
            {
                logger.Log("Podana ścieżka końcowa nie istnieje!");
                return false;
            }

            if (_filesRemoverModel.CopyAndDeleteFiles && Path.GetFullPath(_filesRemoverModel.SourcePath) == Path.GetFullPath(_filesRemoverModel.DestinationPath))
            {
                logger.Log("Ścieżka początkowa i końcowa nie może być taka sama!");
                return false;
            }

            return true;
        }

        private string ShowFolderDialog(string currentPath)
        {
            string resultPath = currentPath;
            var folderDialog = new FolderBrowserDialog();
            
            folderDialog.Reset();
            folderDialog.SelectedPath = currentPath;
>>>>>>>+HEAD
ectFiles        private void CollectFilesInfo(string path)
        {
            try
            {
                allFiles = FastDirectoryEnumerator.EnumerateFiles(path, "*", SearchOption.AllDirectories);
                allFilesCount = allFiles.Count();
                maxValueProgressBar += allFilesCount;
            }
            catch (Exception ex)
            {
                messageBoxLogger.Log(ex.Message);
                return;
            }
        }

        private void CollectDirectoriesInfo(string path)
        {
            try
            {
                allDirectories = Directory.EnumerateDirectories(path, "*", SearchOption.AllDirectories);
                allDirectoriesCount = allDirectories.Count();
                maxValueProgressBar += allDirectoriesCount;
            }
            catch (Exception ex)
            {
                messageBoxLogger.Log(ex.Message);
                return;
            }
        }

        private void CopyAndRemoveUnusedFiles()
        {
            logBox.Invoke(new MethodInvoker(delegate {
                logBox.AppendText($"Liczba wszystkich plików: {allFilesCount} \n\n");
                logBox.AppendText("Pełna scieżka pliku \t Czas ostatniego dostępu do pliku \n");
            }));

            List<string> errors = new List<string>();
            int count = 0;
            
            foreach (FileData file in allFiles)
            {
                var lastAccessDate = file.LastAccesTime;
                if (lastAccessDate <= filesRemoverModel.BorderDate)
                {
                    string fileName = Path.GetFileName(file.Path);
                    try
                    {
                        string copiedFilePath = $@"{filesRemoverModel.DestinationPath}\{fileName}";
                        if (File.Exists(copiedFilePath))
                        {
                            copiedFilePath = copiedFilePath.Insert(copiedFilePath.LastIndexOf('.'), "_copy");
                        }
                        File.Copy(file.Path, copiedFilePath, filesRemoverModel.OverrideFiles);
                        File.SetLastAccessTime(copiedFilePath, lastAccessDate);
                        File.Delete(file.Path);

                        logBox.Invoke(new MethodInvoker(delegate { logBox.AppendText(file.Path + " \t" + lastAccessDate + "\n"); }));

                        count++;
                    }
                    catch (Exception ex)
                    {
                        errors.Add(ex.Message);
                    }
                }

                Application.DoEvents();
                progressBar.Invoke(new MethodInvoker(delegate { progressBar.PerformStep(); }));
            }

            logBox.Invoke(new MethodInvoker(delegate
            {
                logBox.Text = $"Liczba skopiowanych plików: {count}\n" + logBox.Text;
                if (errors.Count != 0)
                    logBox.AppendText("\n\n " + string.Join("\n", errors.ToArray()));
            }));
        }

        private void DeleteEmptyDirectories()
        {
            List<string> errors = new List<string>();
            int count = 0;

            logBox.Invoke(new MethodInvoker(delegate
            {
                logBox.AppendText($"Liczba wszystkich katalogów: {allDirectoriesCount} \n");
            }));

            foreach (string directory in allDirectories)
            {
                if (IsDirectoryEmpty(directory))
                {
                    try
                    {
                        Directory.Delete(directory);
                        logBox.Invoke(new MethodInvoker(delegate { logBox.AppendText(directory + " \n"); }));
                        count++;
                    }
                    catch (Exception ex)
                    {
                        errors.Add(ex.Message);
                    }
                }
                progressBar.Invoke(new MethodInvoker(delegate { progressBar.PerformStep(); }));
            }

            logBox.Invoke(new MethodInvoker(delegate { logBox.AppendText($"\nLiczba usuniętych katalogów: {count}"); }));

            if (errors.Count != 0)
                logBox.Invoke(new MethodInvoker(delegate { logBox.AppendText("\nBłędy: \n\n" + string.Join("\n", errors.ToArray())); }));
        }

        private void DoJobs()
        {      
            if (allFiles != null)
                CopyAndRemoveUnusedFiles();

            if (allDirectories != null)
                DeleteEmptyDirectories();

            progressBar.Invoke(new MethodInvoker(delegate { progressBar.PerformStep(); }));
        }

        private void CollectInfoForJobs()
        {
            if (filesRemoverModel.CopyAndDeleteFiles)
                CollectFilesInfo(filesRemoverModel.SourcePath);
            if (filesRemoverModel.DeleteEmptyDirectories)
                CollectDirectoriesInfo(filesRemoverModel.SourcePath);
        }

        private void InitializeProgressBar()
        {
            maxValueProgressBar = 1;

            progressBar.Value = 0;
            progressBar.Step = 1;
            progressBar.Maximum = maxValueProgressBar;
            progressBar.ForeColor = System.Drawing.Color.Yellow;
        }

        private bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        private bool IsOptionsValid()
        {
            //sprawdzanie poprawności scieżki początkowej i końcowej
            if (!Directory.Exists(filesRemoverModel.SourcePath))
            {
                messageBoxLogger.Log("Podana ścieżka początkowa nie istnieje!");    
                return false;
            }

            if (filesRemoverModel.CopyAndDeleteFiles && !Directory.Exists(filesRemoverModel.DestinationPath))
            {
                messageBoxLogger.Log("Podana ścieżka końcowa nie istnieje!");
                return false;
            }

            if (filesRemoverModel.CopyAndDeleteFiles && Path.GetFullPath(filesRemoverModel.SourcePath) == Path.GetFullPath(filesRemoverModel.DestinationPath))
            {
                messageBoxLogger.Log("Ścieżka początkowa i końcowa nie może być taka sama!");
                return false;
            }

            return true;
        }

        private void SaveLogToFile()
        {
            DateTime dateTime = DateTime.Now;
            string logFileName = dateTime.ToString("yy-MM-dd") + "_godz_" + dateTime.ToString("HH-mm-ss");
            string path = Directory.GetCurrentDirectory() + $@"\{logFileName}.txt";

            try
            {
                logBox.SaveFile(path, RichTextBoxStreamType.PlainText);
            }
            catch (Exception ex)
            {
                messageBoxLogger.Log(path + "\n" + ex.Message);
            }
        }

        private string ShowFolderDialog(string currentPath)
        {
            string resultPath = currentPath;
            var folderDialog = new FolderBrowserDialog();
            
            folderDialog.Reset();
<<<<<<< master
            folderDialog.SelectedPath = textBox.Text;
>>>>>>>-dbb3e01
      DialogResult dr = folderDialog.ShowDialog();
            if (dr == DialogResult.OK || dr == DialogResult.Yes)
            {
<<<<<<< HEAD
                resultPath = folderDialog.Selec                resultPath = folderDialog.SelectedPath;
            }

            return resultPath;
>>>>>>>+HEAD
Path;
                  textBox.Text = folderDialog.SelectedPath;
            }
            folderDialog.SelectedPath = prevpath;
>>>>>>>-dbb3e01
      _stopwatch = new Stopwatch();

            startTimeLabel.Visible = true;
            endTimeLabel.Visible = false;
            durationLabel.Visible = false;

            _stopwatch.Start();

            startTimeLabel.Text = "Czas rozpoczęcia: " + DateTime.Now.ToString("HH:mm:ss");
            Application.DoEvents();
        }

        private void StopTimer()
        {
            _stopwatch.Stop();

            endTimeLabel.Text = "Czas ukończenia: " + DateTime.Now.ToString("HH:mm:ss");
            endTimeLabel.Visible = true;

            durationLabel.Text = "Czas trwania: " + _stopwatch.Elapsed.Seconds + " sek";
            durationLabel.Visible = true;
        }

<<<<<<< HEAD
        private void UpdateBorderDate(int numberOf        private void UpdateBorderDate(int numberOfWeeksBack)
        {
            int days = numberOfWeeksBack * 7;
            _filesRemoverModel.BorderDate = DateTime.Now;
            _filesRemoverModel.BorderDate = _filesRemoverModel.BorderDate.Subtract(TimeSpan.FromDays(days));

            borderDateLabel.Text = _filesRemoverModel.BorderDate.ToShortDateString();
        }

        #endregion
>>>>>>>+HEAD
object sender, EventArgs e)
        {
            string tmp = sourcePathTextBox.Text;
            sourcePathTextBox.Text = destinationPathTextBox.Text;
            destinationPathTextBox.Text = tmp;
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
            int days = numberOfWeeksBack * 7;
            filesRemoverModel.BorderDate = DateTime.Now;
            filesRemoverModel.BorderDate = filesRemoverModel.BorderDate.Subtract(TimeSpan.FromDays(days));

            borderDateLabel.Text = filesRemoverModel.BorderDate.ToShortDateString();
        }
>>>>>>> dbb3e01... Dodaj pliki projektów.
    }
}