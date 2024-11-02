// Devon O'Brien
// 2024-11-02
// description: Creates a log entry class that is accessed by the xaml.cs file

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningLog
{
    class LogEntry
    {
        private int logID;
        private string logEntryDate;
        private int logWellness;
        private int logQuality;
        private string logNotes;
        private FileInfo logFile;

        //LogEntry class definition
        public LogEntry(int ID, string EntryDate, int Wellnes, int Quality, string Notes, FileInfo RecordingFile)
        {
            logID = ID;
            logEntryDate = EntryDate;
            logWellness = Wellnes;
            logQuality = Quality;
            logNotes = Notes;
            logFile = RecordingFile;
            
        }

        // function to retreive the id
        public int GetID()
        {
            return logID;
        }

        // function to retrieve the date
        public string GetEntryDate()
        {
            return logEntryDate;
        }

        // function to retrieve the wellness
        public int GetWellness()
        {
            return logWellness;
        }

        // function to retrieve the quality
        public int GetQuality()
        {
            return logQuality;
        }

        // function to retrieve notes
        public string GetNotes()
        {
            return logNotes;
        }

        // function to retrieve the file location
        public FileInfo GetRecordingFile()
        {
            return logFile;
        }
    }
}
