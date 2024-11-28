// Author: Kyle Chapman
// Created: October 31, 2024
// Updated: November 17, 2024
// Description:
// Form code for a form that allows a user to make an audio recording
// as a log entry. It also allows users to save, play, or delete recordings.

using System;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Controls;

namespace LearningLog2024
{
    /// <summary>
    /// Interaction logic for EntryWindow.xaml
    /// </summary>
    public partial class EntryWindow : Window
    {
        private bool isRecording = false;
        private FileInfo recordingFile;

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
            var newEntry = new AudioLogEntry(
                (int)comboWellnessAudio.SelectedItem,
                (int)comboQualityAudio.SelectedItem,
                textNotesAudio.Text,
                recordingFile
            );

            listViewEntries.Items.Add (newEntry.ToString());
            
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

        private void DisableAudioButtons()
        {
            buttonSaveAudio.IsEnabled = false;
            buttonPlayAudio.IsEnabled = false;
            buttonDeleteAudio.IsEnabled = false;
        }

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
            var newEntry = new TextLogEntry(
                (int)comboWellnessText.SelectedItem,
                (int)comboQualityText.SelectedItem,
                textNotesText.Text
                
            );
            UpdateStatus(newEntry.ToString());
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
                foreach (var entry in LogEntry.Entries)
                {
                    ListViewItem eachItem = new ListViewItem();
                    eachItem.Content = new { EntryDate = entry.EntryDate, LogId = entry.Id, Wellness = entry.Wellness, Quality = entry.Quality, Notes = entry.Notes };
                    listViewEntries.Items.Add(eachItem);
                }
            }
        }
        #endregion
    }
}

