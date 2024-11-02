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

        public int GetID()
        {
            return logID;
        }

        public string GetEntryDate()
        {
            return logEntryDate;
        }

        public int GetWellness()
        {
            return logWellness;
        }

        public int GetQuality()
        {
            return logQuality;
        }

        public string GetNotes()
        {
            return logNotes;
        }

        public FileInfo GetRecordingFile()
        {
            return logFile;
        }
    }
}
