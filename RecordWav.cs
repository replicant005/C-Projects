// Author:  Kyle Chapman
// Created: January 12, 2023
// Updated: October 9, 2024
// Description:
// Records audio using the default recording device, into a file in a default directory (relative to
// where this file is stored). Just two functions: StartRecording() and EndRecording().
// This class adapts and utilizes code from Darin Dimitrov @ https://stackoverflow.com/a/3694293 .

using System;
using System.IO;
using System.Runtime.InteropServices;

// Note to students: you are likely to need to change this namespace to match your project.
namespace Assignment3
{
    internal static class RecordWav
    {
        // This part is entirely from Darin Dimitrov @ https://stackoverflow.com/a/3694293 .
        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int mciSendString(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);

        /// <summary>
        /// Begins a .wav file recording using winmm.dll .
        /// </summary>
        internal static void StartRecording()
        {
            mciSendString("open new Type waveaudio Alias recsound", "", 0, 0);
            mciSendString("record recsound", "", 0, 0);
        }

        /// <summary>
        /// Ends a started .wav file recording using winmm.dll .
        /// </summary>
        /// <returns>A FileInfo object pointing to the resulting .wav file.</returns>
        internal static FileInfo EndRecording()

        {
            string directoryPath = @"C:\Users\mehak\source\repos\Assignment3\Recordings";
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string fileName = directoryPath + "\\Recording" + DateTime.Now.ToString("yyyyMMdd") + ".wav";

            int fileCounter = 0;
            while (File.Exists(fileName))
            {
                fileName = directoryPath + "\\Recording" + DateTime.Now.ToString("yyyyMMdd") + "_" + fileCounter.ToString("D2") + ".wav";
                fileCounter++;
            }
            
            mciSendString("save recsound " + fileName, "", 0, 0);
            mciSendString("close recsound ", "", 0, 0);

            FileInfo returnFile = new FileInfo(fileName);
            return returnFile;
        }

    }
}
