// Author:  Kyle Chapman
// Created: October 31, 2024
// Updated: November 12, 2024
// Description:
// Represents an entry in a learning log program with properties for
// notes on the entry, a state of wellness, a state of quality,
// as well as a filepath to a recording.

using System.IO;
using System.Windows;

namespace LearningLog2024
{
    internal abstract class LogEntry
    {
        #region "Variable declarations"
        // i wont be able to access these for my newest and first entry date hence i am going to make these public ; and i dont think they should be static 
        //// Static variables.
        //protected static int count = 0;
        //protected static DateTime firstEntry;
        //protected static DateTime newestEntry;
        protected static int count = 0;
        public static DateTime firstEntry;
        public static DateTime newestEntry;

        // Instance variables.
        protected int logId;
        protected DateTime logDate = DateTime.Now;
        protected int logWellness;
        protected int logQuality;
        protected string logNotes = String.Empty;

        // Static list of entries 
        protected internal static List<LogEntry> Entries { get; } = new List<LogEntry>();
        #endregion

        #region "Constructors"
        // *** even though i tried by using the list constructor in the constructor for audio and text entries it still didnt work the way i way wanted it to
        protected LogEntry()
        {
             
        }

        #endregion

        #region "Properties"
        /// <summary>
        /// Sets or accesses the log's unique identifier.
        /// </summary>
        protected internal int Id { get => logId; private set
            {
                logId = value;
            }
        }

        /// <summary>
        /// Date indicating when the audio was recorded accessed.
        /// </summary>
        protected internal DateTime EntryDate { get => logDate; set { logDate = value; } }

        /// <summary>
        /// Number based on the “Wellness/Mood” ComboBox.;
        /// </summary>
        protected internal int Wellness { get => logWellness; set {
                if (value < 1 || value > 5)
                    throw new ArgumentOutOfRangeException(nameof(Wellness), "Wellness rating must be between 1 and 5.");
                logWellness = value; } }

        /// <summary>
        /// Number based on the “Quality” ComboBox.;
        /// </summary>
        protected internal int Quality { get => logQuality; set {
                if (value < 1 || value > 5)
                    throw new ArgumentOutOfRangeException(nameof(Quality), "Quality rating must be between 1 and 5.");
                logQuality = value; } }

        /// <summary>
        /// Piece of text from the “notes” TextBox; notes about the log entry.
        /// </summary>
        protected internal string Notes
        {
            get => logNotes;
            set
            {
                if (value.Trim().Length > 0)
                { 
                    logNotes = value;
                }
                else
                {
                    MessageBox.Show("The notes have been left blank.","Entry Error");
                }
            }
        }

        /// <summary>
        /// Notes the file location of the audio file for the log entry.
        /// </summary>

        /// <summary>
        /// Get the total number of log entries.
        /// </summary>
        protected internal static int Count => count;

        /// <summary>
        /// Get the first log entry's date.
        /// </summary>
        protected internal static DateTime FirstEntry => firstEntry;

        /// <summary>
        /// Get the most recent log entry's date.
        /// </summary>
        protected internal static DateTime NewestEntry => newestEntry;
        #endregion

        #region "Methods"
        /// <summary>
        /// Displays a log entry as a string
        /// </summary>
        /// <returns>Log entry as a string</returns>
        public override string ToString()
        {
            return "Entry " + Id + " created at " + EntryDate.ToString();
        }
        #endregion
    }
}
