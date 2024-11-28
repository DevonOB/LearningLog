using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningLog
{
    class AudioLogEntry : LogEntry
    {
        //LogEntry class definition
        public AudioLogEntry(int ID, int Wellnes, int Quality, string Notes, FileInfo RecordingFile)
        {
            logID = ID;
            logWellness = Wellnes;
            logQuality = Quality;
            logNotes = Notes;
            logFile = RecordingFile;
            if (logID == 1)
            {
                firstEntry = DateTime.Now;
            }
            newestEntry = DateTime.Now;

        }
        public AudioLogEntry()
        {

        }


    }
}
