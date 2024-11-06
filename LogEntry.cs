/* Author : mehak kapur
 * created : October 29th, 2024
 * Description : this class is for the entry in the learning log
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment3
{
    internal class LogEntry
    {
        //static variables :
        private static int count = 0;
        private static DateTime firstEntry;
        private static DateTime newEntry;
        //instance variables
        private int logId;
        private DateTime logDate = DateTime.Now;
        private int logWellness;
        private int logQuality;
        private string logNotes = String.Empty;
        private FileInfo logFile = null;

        // Constructors  
        // constructor that sets the logId and count
        public LogEntry() { 
            // update the static variables
            count++;
            logId = count;
        }
        /// <summary>
        /// Parameterized constructor to create new log entry objects
        /// </summary>
        /// <param name="wellnessValue">A wellness value between 1 and 5</param>
        /// <param name="qualityValue">A quality value between 1 and 5</param>
        /// <param name="notesValue">ANotes for this logEntry</param>
        /// <param name="fileValue">File path to the recording</param>
        public LogEntry(int wellnessValue, int qualityValue, string notesValue, FileInfo fileValue) 
        {
            count++;
            if (count == 0)
            {
                firstEntry = DateTime.Now;
            }
            newEntry = DateTime.Now;
            // set values for instance properties
            logId = count;  
            Wellness = wellnessValue;
            Quality = qualityValue;
            Notes = notesValue;
            File = fileValue;           
        }


        //properties:
        public int Id { get => logId; private set { logId = value; } } //get to reference the variable, set property as whatever id will be assigned 
        public DateTime EntryDate { get => logDate; private set {logDate = value; } }
        public int Wellness { get => logWellness; private set { logWellness = value; } }
        public int Quality { get => logQuality; private set {logQuality = value; } }    
        public string Notes { get => logNotes;  private set {logNotes = value; } }  
        public FileInfo File { get => logFile; private set {logFile = value; } }

        // get the total number of log entries
        public static int Count => count;
        // get the first log entry's date 
        public static DateTime FirstEntry => firstEntry;
        // get the most recent log entry's date 
        public static DateTime NewEntry => newEntry;

        // methods :
        // to display a log entry as a string
        public override string ToString()
        {
            return "Entry " + Id + "created at "  + EntryDate.ToString();
        }

    }
}
