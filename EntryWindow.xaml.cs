// Author: Kyle Chapman
// Created: October 31, 2024
// Updated: November 17, 2024
// Description:
// Form code for a form that allows a user to make an audio recording
// as a log entry. It also allows users to save, play, or delete recordings.

using Microsoft.Win32;
using System.IO;
using System.Media;
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
using System.Text.Json;
using static LearningLog2024.LogData;
using System.Collections.Generic;
using System.Windows.Interop;
using System.Diagnostics;
using System.Windows.Controls.Primitives;

namespace LearningLog2024
{
    /// <summary>
    /// Interaction logic for EntryWindow.xaml
    /// </summary>
    public partial class EntryWindow : Window
    {
        private bool isRecording = false;
        private FileInfo recordingFile;
        private LogData.Data selectedEntry;

        /// <summary>
        /// Constructor for the window.
        /// </summary>
        public EntryWindow()
        {
            InitializeComponent();
            InitializeForm();
        }

        /// <summary>
        /// Initializes the form elements.
        ///
        /// </summary>

        private void InitializeForm()
        {
            // Populate the ComboBoxes.
            for (int counter = 1; counter <= 5; counter++)
            {
                comboQualityAudio.Items.Add(counter);
                comboWellnessAudio.Items.Add(counter);

                comboQualityText.Items.Add(counter);
                comboWellnessText.Items.Add(counter);
            }

            ResetAudioForm();
            ResetTextForm();
        }

        #region Audio Log Methods

        /// <summary>
        /// Starts or stops an audio recording based on the current state.
        /// </summary>
        private void RecordClick(object sender, RoutedEventArgs e)
        {
            if (!isRecording)
            {
                StartAudioRecording();
            }
            else
            {
                StopAudioRecording();
            }
        }

        private void StartAudioRecording()
        {
            isRecording = true;
            RecordWav.StartRecording();
            DisableAudioButtons();
            UpdateStatus("Recording started.");
            buttonRecordAudio.Content = "Stop";
        }

        private void StopAudioRecording()
        {
            isRecording = false;
            recordingFile = RecordWav.EndRecording();
            EnableAudioButtons();
            UpdateStatus("Recording completed and saved to " + recordingFile.FullName);
            buttonRecordAudio.Content = "_Record";
        }

        /// <summary>
        /// Saves the audio log entry.
        /// </summary>
        private void SaveClick(object sender, RoutedEventArgs e)
        {
        
            try
            {
                string userNotes = textNotesAudio.Text;
                LogEntry newLogEntry = new AudioLogEntry((int)comboWellnessAudio.SelectedItem,
                (int)comboQualityAudio.SelectedItem,
                textNotesAudio.Text,
                recordingFile);

                newLogEntry.Notes = userNotes;
                UpdateStatus(newLogEntry.ToString());
                MessageBox.Show("Saved successfully!", "Success");
                listViewEntries.Items.Clear();
                List<LogData.Data> newAudioLog = new List<LogData.Data>();


                foreach (var item in LogEntry.List)
                {
                    ListViewItem itemAdded = new ListViewItem();
                    itemAdded.Content = new { Id = item.Id, EntryDate = item.EntryDate, Notes = item.Notes, Wellness = item.Wellness, Quality = item.Quality };
                    listViewEntries.Items.Add(itemAdded);
 
                    LogData.Data dataInAudio = new LogData.Data { Id = item.Id, EntryDate = item.EntryDate, Wellness = item.Wellness, Quality = item.Quality, Notes = item.Notes };
                    newAudioLog.Add(dataInAudio);
                }
                Serializer(newAudioLog);

                ResetAudioForm();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Entry Error");
            }
            ResetAudioForm();
        }

        /// <summary>
        /// Plays the recorded audio file.
        /// </summary>
        private void PlayClick(object sender, RoutedEventArgs e)
        {
            if (recordingFile != null && File.Exists(recordingFile.FullName))
            {
                var player = new SoundPlayer(recordingFile.FullName);
                player.Play();
                UpdateStatus("Playing " + recordingFile.FullName);
                EnableAudioButtons();   
            }
            else
            {
                UpdateStatus("Failed to play " + recordingFile?.FullName);
            }
        }

        /// <summary>
        /// Deletes the recorded audio file.
        /// </summary>
        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            if (recordingFile != null && File.Exists(recordingFile.FullName))
            {
                File.Delete(recordingFile.FullName);
                UpdateStatus("Deleted " + recordingFile.FullName);
            }
            else
            {
                UpdateStatus(recordingFile?.FullName + " does not exist.");
            }

            ResetAudioForm();
        }

        /// <summary>
        /// Resets the audio log entry form.
        /// </summary>
        private void ResetAudioForm()
        {
            comboQualityAudio.SelectedIndex = 2;
            comboWellnessAudio.SelectedIndex = 2;
            isRecording = false;
            buttonRecordAudio.Content = "_Record";
            DisableAudioButtons();
            textNotesAudio.Text = string.Empty;
           
        }
        /// <summary>
        /// disable audio button method , just for cleaner code
        /// </summary>
        private void DisableAudioButtons()
        {
            buttonSaveAudio.IsEnabled = false;
            buttonPlayAudio.IsEnabled = false;
            buttonDeleteAudio.IsEnabled = false;
        }
        /// <summary>
        /// enable audio button method , just for cleaner code
        /// </summary>
        private void EnableAudioButtons()
        {
            buttonSaveAudio.IsEnabled = true;
            buttonPlayAudio.IsEnabled = true;
            buttonDeleteAudio.IsEnabled = true;
        }

        #endregion

        #region Text Log Methods
        /// <summary>
        /// Saves the text log entry.
        /// </summary>
        private void SaveTextClick(object sender, RoutedEventArgs e)
        {
            // Create and save the file log entry
            LogEntry newFileEntry = new TextLogEntry((int)comboWellnessText.SelectedItem,
            (int)comboQualityText.SelectedItem,
            textNotesText.Text);
            List<LogData.Data> newTextItem = new List<LogData.Data>();

                UpdateStatus(newFileEntry.ToString());
                buttonSaveText.IsEnabled = false;
                listViewEntries.Items.Clear();
                foreach (var item in LogEntry.List)
                {
                ListViewItem textItem = new ListViewItem();
                textItem.Content = new { Id = item.Id, EntryDate = item.EntryDate, Notes = item.Notes, Wellness = item.Wellness, Quality = item.Quality };
                listViewEntries.Items.Add(textItem);
                LogData.Data dataInText = new LogData.Data { EntryDate = item.EntryDate, Wellness = item.Wellness, Quality = item.Quality, Notes = item.Notes };
                newTextItem.Add(dataInText);
                }
                Serializer(newTextItem);
                ResetTextForm();
        }

        /// <summary>
        /// Resets the text log entry form.
        /// </summary>
        private void ResetTextForm()
        {
            comboQualityText.SelectedIndex = 2;
            comboWellnessText.SelectedIndex = 2;
            textNotesText.Text = string.Empty;
            buttonSaveText.IsEnabled = true;
        }

        #endregion

        #region Shared Methods

        /// <summary>
        /// Saves any log entry and updates the status.
        /// </summary>

        /// <summary>
        /// Updates the status bar with a given message.
        /// </summary>
        private void UpdateStatus(string status)
        {
            statusState.Content = DateTime.Now.ToString("MM/dd/yy H:mm:ss") + ": " + status;
        }

        /// <summary>
        /// Updates the summary tab when the tab is switched.
        /// </summary>
        private void TabChanged(object sender, RoutedEventArgs e)

        {
           
            if (tabController.SelectedItem == tabSummary)
            {
                textNumberOfEntries.Text = LogEntry.Count.ToString();
                textFirstEntry.Text = LogEntry.firstEntry.ToString();
                textNewestEntry.Text = LogEntry.newestEntry.ToString();
                UpdateStatus("Viewing summary");
            }
            // for this part i took help of my classmate brendan
            if (tabController.SelectedItem == tabList)
            {
                listViewEntries.Items.Clear();
                foreach (var entry in LogEntry.List)
                {
                    ListViewItem eachItem = new ListViewItem();
                    eachItem.Content = new { EntryDate = entry.EntryDate, LogId = entry.Id, Wellness = entry.Wellness, Quality = entry.Quality, Notes = entry.Notes };
                    listViewEntries.Items.Add(eachItem);
                }
            }
        }
        #endregion
        /// <summary>
        /// serialize method to serialize the entry into json format
        /// </summary>
        /// <param name="logEntries"></param>
        public void Serializer(List<LogData.Data> logEntries)
        {
            string jsonString = JsonSerializer.Serialize(logEntries, new JsonSerializerOptions { WriteIndented = true });
            //write JSON string to file 
            string filePath = "C:\\Users\\mehak\\source\\repos\\LearningLog2024\\LearningLog2024\\LogData.json";
            File.WriteAllText(filePath, jsonString);

        }
         /// <summary>
         /// deserialize method to deserialize the data in json
         /// </summary>
        public void Deserialize()
        {
            string jsonString = File.ReadAllText("C:\\Users\\mehak\\source\repos\\LearningLog2024\\LearningLog2024\\LogData.json");
            var LogData = JsonSerializer.Deserialize<LogEntry>(jsonString);
            listViewEntries.ItemsSource = null;
            listViewEntries.Items.Clear();
            LogEntry.List.AddRange((IEnumerable<LogData.Data>)LogData);
            listViewEntries.DataContext = LogEntry.List;

        }
        /// <summary>
        /// edit button in list tab to edit any of the entries
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void listbuttonEdit_Click(object sender, RoutedEventArgs e)
        {
            selectedEntry = listViewEntries.SelectedItem as LogData.Data;

            if (selectedEntry is LogData.Data audioEntry)
            {
                // Switch to Audio Entry tab
                tabController.SelectedItem = tabAudioEntry;

                //add items to audio entry fields
                comboWellnessAudio.SelectedItem = audioEntry.Wellness;
                comboQualityAudio.SelectedItem = audioEntry.Quality;
                textNotesAudio.Text = audioEntry.Notes;

            
            }
            else if (selectedEntry is LogData.Data textEntry)
            {
                // Switch to Text Entry tab
                tabController.SelectedItem = tabTextEntry;

                //add items to text entry fields
                comboWellnessText.SelectedItem = textEntry.Wellness;
                comboQualityText.SelectedItem = textEntry.Quality;
                textNotesText.Text = textEntry.Notes;
            }
        }
        /// <summary>
        ///Delete button to delete any entry in the list
        ///code has been referenced from gpt , few websites , classmates etc.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listbuttonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (listViewEntries.SelectedItem is LogData.Data selectedItem)
            {
                // Get the ID of the selected item
                int selectedId = selectedItem.Id;

                // Define the path to the JSON file
                string filePath = "C:\\Users\\mehak\\source\\repos\\LearningLog\\LearningLog\\JsonLogData.json"; 

                if (File.Exists(filePath))
                {
                    try
                    {
                        // Load the existing data from the JSON file
                        string jsonData = File.ReadAllText(filePath);
                        List<LogData.Data> entries = JsonSerializer.Deserialize<List<LogData.Data>>(jsonData);

                        // Find and remove the entry with the matching ID
                        var entryToRemove = entries.FirstOrDefault(entry => entry.Id == selectedId);
                        if (entryToRemove != null)
                        {
                            entries.Remove(entryToRemove);

                            // Save the updated list back to the JSON file
                            string updatedJsonData = JsonSerializer.Serialize(entries);
                            File.WriteAllText(filePath, updatedJsonData);

                            // Remove the item from the ListView
                            listViewEntries.Items.Remove(selectedItem);

                            MessageBox.Show("Entry deleted successfully.", "Delete Success");
                        }
                        else
                        {
                            MessageBox.Show("Entry not found in the data source.", "Error");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error while deleting the entry: {ex.Message}", "Error");
                    }
                }
                else
                {
                    MessageBox.Show("Data file not found.", "Error");
                }
            }
            else
            {
                MessageBox.Show("No item selected.", "Error");
            }
        }

        private void MenuSave_Click(object sender, RoutedEventArgs e)
        {
            if (tabController.SelectedItem == tabAudioEntry)
            {
                SaveClick(sender, e);
            }
            else if (tabController.SelectedItem == tabTextEntry)
            {
                SaveTextClick(sender, e);
            }
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuClearInputs_Click(object sender, RoutedEventArgs e)
        {

            if (tabController.SelectedItem == tabAudioEntry)
            {
                ResetAudioForm();
            }
            else if (tabController.SelectedItem == tabTextEntry)
            {
                ResetTextForm();
            }
        }

        private void MenuClearList_Click(object sender, RoutedEventArgs e)

        {
            if (tabController.SelectedItem == tabAudioEntry)
            {
                if (MessageBox.Show("Clear all entries?", "Confirm Clear",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    LogEntry.List.Clear();
                    Serializer(new List<LogData.Data>());

                }

            }
        }

        private void MenuAbout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Hello there , If you need any help please contact mehak.kapur@dcmail.com"
            );
        }
    }

}

