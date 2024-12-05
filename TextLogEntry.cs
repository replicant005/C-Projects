// Author:  Mehak Kapur
// Created: November 14, 2024
// Description: this is an inherrited class from LogEntry
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningLog2024
{
  
        internal class TextLogEntry : LogEntry
        {
            #region "Variable Declarations"
            // Static variables are all inherited

            // Instance variable
            private string TextEntered;
            #endregion

            #region "Constructors"
            /// <summary>
            /// Default constructor sets the count and id values.
            /// </summary>
            ///
            public TextLogEntry()
            {
                count++;
                logId = count;
            }

            /// <summary>
            /// Parameterized constructor to create a new text log entry object.
            /// </summary>
            /// <param name="wellnessValue">A wellness value between 1 and 5</param>
            /// <param name="qualityValue">A quality value between 1 and 5</param>
            /// <param name="notesValue">Notes for this log entry</param>
            /// <param name="extraTextValue">Additional text associated with the log entry</param>
            public TextLogEntry(int wellnessValue, int qualityValue, string TextValue)
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
                TextEntered = TextValue;
                Entries.Add(this);

        }
            #endregion

            #region "Properties"
            /// <summary>
            /// Additional text for the log entry.
            /// </summary>
            internal string EnteredText
            {
                get => TextEntered;
                set
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        throw new ArgumentException("text cannot be null or empty.", nameof(TextEntered));
                    }
                TextEntered = value;
                }
            }
            #endregion

            #region "Methods"
            /// <summary>
            /// Displays a log entry as a string.
            /// </summary>
            /// <returns>Log entry as a string.</returns>
            public override string ToString()
            {
                return base.ToString() + " Text: " + EnteredText; // Use base class ToString method and append extra text
            }
            #endregion
        }
    }
