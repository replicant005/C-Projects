/* Author : mehak kapur
 * created : October 30th, 2024
 * description : this is the .cs file for the wpf application 
 */


using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Data;
using System.Media;

namespace Assignment3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isRecording = false;
        FileInfo recordingFile;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (recordingFile != null && recordingFile.Exists)
            {
                try
                {
                    // Delete the file
                    recordingFile.Delete();
                    UpdateStatus("Recording deleted successfully.");

                    // Reset the buutons 
                    buttonRecord.IsEnabled = true;
                    buttonPlay.IsEnabled = false;
                    buttonDelete.IsEnabled = false;
                    buttonSave.IsEnabled = false;

                    //  reset any displayed information related to the deleted recording
                    recordingFile = null;
                    RefreshLog();
                }
                catch (Exception ex)
                {
                    // Log the error and show a message to the user
                    UpdateStatus("Error deleting recording: " + ex.Message);
                }
            }
            else
            {
                // Inform the user if there's no recording to delete
                UpdateStatus("No recording file found to delete.");
            }

        }

        private void RefreshLog()
        { // reset all the controls 
            text_Notes.Clear();
            comboBox_Wellness.Items.Clear();    
            comboBox_Quality.Items.Clear(); 
            buttonSave.IsEnabled = false;
            buttonPlay.IsEnabled = false;
            buttonDelete.IsEnabled = false;
            buttonRecord.IsEnabled = true;
                        
                }
        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (recordingFile == null)
            {
                // update the status bar
                UpdateStatus("No recording to save");
                return;
            }

            int Wellness = comboBox_Wellness.SelectedIndex;
            int Quality = comboBox_Quality.SelectedIndex;
            string textValue = text_Notes.Text;
            LogEntry newEntry = new LogEntry(Wellness,Quality, textValue, recordingFile);
            UpdateStatus(newEntry.ToString());
        }

        private void UpdateStatus(string status)
        {
            statusState.Content = status;
        
        }
        private void buttonPlay_Click(object sender, RoutedEventArgs e)
        {
            var player = new SoundPlayer(recordingFile.FullName);
            player.Play();
            UpdateStatus("Playing" + recordingFile.FullName);
        }

        private void buttonRecord_Click(object sender, RoutedEventArgs e)
        {
            if (!isRecording)
            {
                labelRecordText.Content = "Stop";
                isRecording = true;
                RecordWav.StartRecording();

                // disable buttons during recording
                buttonSave.IsEnabled = false;
                buttonPlay.IsEnabled = false;
                buttonDelete.IsEnabled = false; // Disable delete while recording
                UpdateStatus("Recording started at " + DateTime.Now.ToString("yyyyMMdd") + ".");
            }
            else
            {
                labelRecordText.Content = "_Record";
                isRecording = false;

                // end the recording and store the file reference
                recordingFile = RecordWav.EndRecording();

                // enable buttons after recording
                buttonSave.IsEnabled = true;
                buttonPlay.IsEnabled = true;
                buttonDelete.IsEnabled = true; // Enable delete now that recording is saved
                UpdateStatus("Recording completed and saved to " + recordingFile.FullName);
            }

        }

            private void textNotes_TextChanged(object sender, TextChangedEventArgs e) { }

        private void TabChanged(object sender, SelectionChangedEventArgs e) 
        {
            if (TabController.SelectedItem == tabSummary)
            {
                // update the summary tab fields 
                UpdateStatus("Viewing Summary");
                labelNumber.Content = LogEntry.Count.ToString();
                labelFirstEntryName.Content = LogEntry.FirstEntry.ToString();
                labelNewEntryName.Content = LogEntry.NewEntry.ToString();
                UpdateStatus("Summary tab refreshed.");
            }
        }
            
        }
    }
