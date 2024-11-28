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
    internal abstract class LogEntry
    {
        protected internal int logID;
        protected internal int logWellness;
        protected internal int logQuality;
        protected internal string logNotes;
        protected internal string logEntry;
        protected internal FileInfo logFile;
        protected internal static DateTime firstEntry;
        protected internal static DateTime newestEntry;
        protected internal static List<LogEntry> list = new List<LogEntry>();


        // function to retreive the id
        protected internal int GetID()
        {
            return logID;
        }

        // function to retrieve the wellness
        protected internal int GetWellness()
        {
            return (logWellness);
        }

        // function to retrieve the quality
        protected internal int GetQuality() 
        { 
            return logQuality;
        }

        // function to retrieve notes
        protected internal string GetNotes()
        {
            return logNotes;
        }

        // function to retrieve the file location
        protected internal FileInfo GetFile()
        {
            return logFile;
        }

        // function to retrieve the first file
        protected internal DateTime GetFirstRecording()
        {
            return firstEntry;
        }
        protected internal DateTime GetNewestRecording()
        {
            return newestEntry;
        }
        protected internal string GetEntry()
        {
            return logEntry;
        }

        protected internal void AddToList(LogEntry e)
        {
            list.Add(e);
        }

        protected internal List<LogEntry> GetList(LogEntry e)
        {
            return list;
        }
    }
}
