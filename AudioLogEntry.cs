// Author:  Kyle Chapman
// Created: October 31, 2024
// Updated: November 20, 2024
// Description: this is an inherrited class from LogEntry

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LearningLog2024
{
    internal class AudioLogEntry : LogEntry
    {
        #region "Variable declarations"
        // Static variables are all inherited

        // Instance variables.
        private FileInfo logFile;

        #endregion

        #region "Constructors"
        //There should be a default constructor as well as a parametrized constructor that will take four parameters: wellness, quality, notes, and FileInfo.

        /// <summary>
        /// Default constructor sets the count and id values.
        /// </summary>
        public AudioLogEntry()
        {
            count++;
            logId = count;
        }

        /// <summary>
        /// Parametrized constructor to create a new log entry object.
        /// </summary>
        /// <param name="wellnessValue">A wellness value between 1 and 5</param>
        /// <param name="qualityValue">A quality value between 1 and 5</param>
        /// <param name="notesValue">Notes for this log entry</param>
        /// <param name="fileValue">File path to the associated recording file</param>
        public AudioLogEntry(int wellnessValue, int qualityValue, string notesValue, FileInfo fileValue)
        {

            count++;
            // Update the static variables.
            // If this is the first entry, set that property.
            if (count == 1)
            {
                firstEntry = DateTime.Now;
            }
            newestEntry = DateTime.Now;

            // Set values for the instance properties.
            logId = count;
            Wellness = wellnessValue;
            Quality = qualityValue;
            Notes = notesValue;
            RecordingFile = fileValue;
            logEntryList.Add(this);     
        }

        #endregion

        #region "Properties"
        /// <summary>
        /// Notes the file location of the audio file for the log entry.
        /// </summary>
        internal FileInfo RecordingFile { get => logFile; set { logFile = value; } }
        #endregion

        #region "Methods" 
        /// <summary>
        /// Displays a log entry as a string
        /// </summary>
        /// <returns>Log entry as a string</returns>
        public override string ToString()
        {
            return base.ToString() + " Audio";  // uses base class to string method and concatinates it with "audio"
        }

        #endregion 


    }
}
