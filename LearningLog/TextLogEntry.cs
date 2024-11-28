using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningLog
{
    internal class TextLogEntry : LogEntry
    {
        //LogEntry class definition
        public TextLogEntry(int ID, int Wellnes, int Quality, string Notes, FileInfo TextFile)
        {
            logID = ID;
            logWellness = Wellnes;
            logQuality = Quality;
            logEntry = Notes;
            logFile = TextFile;
            if (logID == 1)
            {
                firstEntry = DateTime.Now;
            }
            newestEntry = DateTime.Now;
        }

        public TextLogEntry()
        {

        }
    }

   
}
