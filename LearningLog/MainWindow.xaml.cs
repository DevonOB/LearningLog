// Devon O'Brien
// 2024-11-02
// description: Allows the user to start and end recordings as well as listen to those recordings.
//              Also allows the user to delete the recordings and see where they are saved to.



using System.IO;
using System.Xml;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System;
using System.Media;
using System.Data;
using System.ComponentModel;


namespace LearningLog
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            RestoreList();
        }
        
        // sets global variables for use in the code
        int ID = 0;
        string recordingInfo = string.Empty;
        string recordingNotes = string.Empty;
        string logType = string.Empty;
        FileInfo storedInfo = new FileInfo("File.text");
        FileInfo previousInfo = new FileInfo("File.text");
        bool recording = false;
        string noteEntry = string.Empty;
        protected internal static List<String> fileInfoList = new List<String>();
        List<LogEntry> absoluteList = new List<LogEntry>();
        int wellness = 0;
        int quality = 0;
        DateTime oldestEntry;
        bool isPreStored = false;

        // One the first click starts a recording and on the second click ends the recording and sets the rest of the page to be interactible
        private void buttonRecord_Click(object sender, RoutedEventArgs e)
        {
            if (recording == false )
            {
                RecordWav.StartRecording();
                recording = true;
                buttonRecord.Content = ("Stop recording");
                statusChange("New recording started");
            }
            else
            {
                statusChange("Recording ended");
                buttonRecord.Content = ("Record");
                FileInfo newRecordingInfo = RecordWav.EndRecording();
                recordingInfo = newRecordingInfo.ToString();
                storedInfo = newRecordingInfo;
                buttonPlay.IsEnabled = true;
                recording = false;
                buttonRecord.IsEnabled = false;
                buttonDelete.IsEnabled = true;
                textNotes.IsEnabled = true;
            }
            
        }

        // Deletes the current recording and resets the page for a new recording to occur
        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (tabController.SelectedItem == tabEntry)
            {
                try
                {
                    if (recordingInfo != string.Empty)
                    {
                        statusChange("Deleted recording");
                        File.Delete(recordingInfo);
                        recordingInfo = string.Empty;
                        buttonRecord.IsEnabled = true;
                        buttonDelete.IsEnabled = false;
                        buttonSave.IsEnabled = false;
                        buttonPlay.IsEnabled = false;
                    }
                    else
                    {
                        statusChange("Failed to delete recording");
                        throw new FileNotFoundException("No wav file availible for deletion");
                    }
                }
                catch (FileNotFoundException a)
                {
                    MessageBox.Show(a.Message);
                }
            }
            else
            {
                statusChange("Deleted text entry");
                buttonSaveText.IsEnabled = false;
                buttonDeleteText.IsEnabled = false;
                textEntry.Text = string.Empty;
            }
        }

        // Plays the recording and displays an error message if it is unavailible
        private void buttonPlay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (recordingInfo != string.Empty)
                {
                    statusChange("Played recording");
                    SoundPlayer newRecording = new SoundPlayer(recordingInfo);
                    newRecording.Play();
                }
                else
                {
                    statusChange("Failed to played recording");
                    throw new FileNotFoundException("No wav file is availible to play");

                }
            }
            catch (FileNotFoundException a)
            {
                MessageBox.Show(a.Message);
            }
        }

        /// <summary>
        /// Clears all fields when clicked
        /// </summary>
        private void menuClear_Click(object sender, RoutedEventArgs e)
        {
            buttonRecord.IsEnabled = true;
            buttonPlay.IsEnabled = false;
            textNotes.Text = string.Empty;
            buttonSave.IsEnabled = false;
            textNotes.IsEnabled = false;
            buttonDeleteText.IsEnabled = false;
            textEntry.Text = string.Empty;
            buttonSaveText.IsEnabled = false;
        }

        // Saves the current object and resets the page
        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (buttonSave.IsEnabled==true || buttonSaveText.IsEnabled==true)
            {
                ID++;
                LogEntry newEntry;
                // Saves audio recording
                if (tabController.SelectedItem == tabEntry)
                {
                    noteEntry = textNotes.Text;
                    newEntry = new AudioLogEntry(ID, int.Parse(comboWellness.Text), int.Parse(comboQuality.Text), noteEntry, storedInfo);                    
                    buttonDelete.IsEnabled = false;
                    statusChange("Saved recording");
                    buttonRecord.IsEnabled = true;
                    buttonPlay.IsEnabled = false;
                    textNotes.Text = string.Empty;
                    buttonSave.IsEnabled = false;
                    updateSummary(newEntry);
                    textNotes.IsEnabled = false;
                }
                // Saves text file
                else
                {
                    if (!isPreStored)
                    {
                        storedInfo = RecordText.SaveTextEntry(textEntry.Text);
                    }
                    noteEntry = textEntry.Text;
                    newEntry = new TextLogEntry(ID, int.Parse(comboWellnessText.Text), int.Parse(comboQualityText.Text), noteEntry, storedInfo);
                    buttonDeleteText.IsEnabled = false;
                    statusChange("Saved text");
                    updateSummary(newEntry);
                    textEntry.Text = string.Empty;
                    buttonSaveText.IsEnabled = false;
                    textEntry.IsEnabled = 
                    isPreStored = false;
                }
                // Adds the saved item to both lists and the list tab
                if (fileInfoList.Contains(newEntry.GetFile().ToString()))
                {
                    for (int i = 0; i < fileInfoList.Count; i++)
                    {
                        if (fileInfoList[i] == newEntry.GetFile().ToString())
                        {
                            fileInfoList.Remove(fileInfoList[i]);
                            listEntries.Items.Remove(listEntries.Items[i]);
                            newEntry.RemoveFromList(newEntry);
                            absoluteList.Remove(absoluteList[i]);
                            absoluteList.Add(newEntry);
                            fileInfoList.Add(newEntry.GetFile().ToString());
                            listEntries.Items.Add(newEntry.GetFile());
                            newEntry.AddToList(newEntry);
                        }
                    }
                }
                else
                {
                    fileInfoList.Add(newEntry.GetFile().ToString());
                    newEntry.AddToList(newEntry);
                    listEntries.Items.Add(newEntry.GetFile());
                    absoluteList.Add(newEntry);
                }
            }
            else
            {
                MessageBox.Show("No entry availible to save");
            }

        }

        // When notes change activates the save button
        private void textNotes_TextChanged(object sender, TextChangedEventArgs e)
        {
            // edit status for audio log
            if (tabController.SelectedItem == tabEntry)
            {
                statusChange("Edited notes");
                buttonSave.IsEnabled = true;
            }
            // edit status for text log
            else
            {
                statusChange("Edited writing entry");
                buttonSaveText.IsEnabled = true;
                buttonDeleteText.IsEnabled = true;
            }
        }

        void Window_Closing(object sender, CancelEventArgs e)
        {
            string msg = "Would you like to exit? Unsaved data will be lost";
            MessageBoxResult result =
              MessageBox.Show(
                msg,
                "Exiting Page",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                SaveXML();
            }
        }

        // sets the status bar
        private void statusChange(string e)
        {
            textStatus.Text = e + " " +DateTime.Now;
            textStatusText.Text = e + " " + DateTime.Now;
        }

        // update the summary page
        private void updateSummary(LogEntry entry)
        {
            // update summary for an audio log
            if (tabController.SelectedItem == tabEntry || logType == "AudioLog")
            {
                labelSummaryNotes.Content = "Notes:";
                textID.Text = listEntries.Items.Count.ToString();
                textNewestEntry.Text = entry.GetNewestRecording().ToString();
                if (absoluteList.Count == 0)
                {
                    textFirstEntry.Text = entry.GetFirstRecording().ToString();
                }
                textSummaryNotes.Text = entry.GetNotes();
                textWellness.Text = entry.GetWellness().ToString();
                textQuality.Text = entry.GetQuality().ToString();
                textFile.Text = entry.GetFile().ToString();
                logType = "";
            }
            // update summary for a text log
            else
            {
                labelSummaryNotes.Content = "Entry:";
                textID.Text = listEntries.Items.Count.ToString();
                textNewestEntry.Text = entry.GetNewestRecording().ToString();
                if (absoluteList.Count == 0)
                {
                    textFirstEntry.Text = entry.GetFirstRecording().ToString();
                }
                textSummaryNotes.Text = entry.GetEntry();
                textWellness.Text = entry.GetWellness().ToString();
                textQuality.Text = entry.GetQuality().ToString();
                textFile.Text = entry.GetFile().ToString();
                logType = "";
            }
        }

        /// <summary>
        /// saves to xml file hopefully
        /// </summary>
        private void SaveXML()
        {
            //try
            //{
            int listLength = absoluteList.Count;
            // starts a xml writer
            using (XmlWriter writer = XmlWriter.Create("LogEntry.xml"))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("LogEntry");
                // write an audio log
                for (int i = 0; i < listLength; i++)
                {

                    if (absoluteList[i].GetType() == typeof(AudioLogEntry))
                    {
                        writer.WriteStartElement("AudioLog");
                    }
                    // write a text log
                    else
                    {
                        writer.WriteStartElement("TextLog");
                    }
                    writer.WriteElementString("Newest", absoluteList[i].GetNewestRecording().ToString());
                    writer.WriteElementString("First", absoluteList[i].GetFirstRecording().ToString());
                    writer.WriteElementString("ID", absoluteList[i].GetID().ToString());
                    writer.WriteElementString("Wellness", absoluteList[i].GetWellness().ToString());
                    writer.WriteElementString("Quality", absoluteList[i].GetQuality().ToString());
                    if (absoluteList[i].GetType() == typeof(AudioLogEntry))
                    {
                        writer.WriteElementString("Notes", absoluteList[i].GetNotes());
                    }
                    // write a text log
                    else
                    {
                        writer.WriteElementString("Entry", absoluteList[i].GetEntry());
                    }
                    
                    writer.WriteElementString("File", absoluteList[i].GetFile().ToString());
                    writer.WriteEndElement();
                }

                // close the xml writer
                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();
                //}
                //catch
                //{
                //MessageBox.Show("Error unable to save XML file");
                //}
            }
        }
            

        /// <summary>
        /// displays the about menu
        /// </summary>
        private void menuHelp_Click(object sender, RoutedEventArgs e)
        {
            statusChange("Viewed file information");
            MessageBox.Show("This here file belongs to Devon O'Brien\n" + DateTime.Now);
        }

        /// <summary>
        /// updates the list of logs when the program is run
        /// </summary>
        private void RestoreList()
        {
            string xmlFilePath = "LogEntry.xml";
            List<String> addLogs = new List<string>();
            List<String> addNewest = new List<string>();
            List<String> addFirst = new List<string>();
            List<int> addID = new List<int>();
            List<int> addWellness = new List<int>();
            List<int> addQuality = new List<int>();
            List<String> addNotes = new List<string>();
            List<FileInfo> addFile = new List<FileInfo>();

            try
            {
                using (XmlReader reader = XmlReader.Create(xmlFilePath))
                {

                    while (reader.Read())
                    {
                        if (reader.Name == "AudioLog")
                        {
                            logType = "AudioLog";
                            addLogs.Add(logType);
                        }
                        if (reader.Name == "TextLog")
                        {
                            logType = "TextLog";
                            addLogs.Add(logType);
                        }
                        if (reader.Name == "Newest")
                        {
                            string newest = reader.ReadElementContentAsString();
                            addNewest.Add(newest);
                        }
                        if (reader.Name == "First")
                        {
                            string first = reader.ReadElementContentAsString();
                            addFirst.Add(first);
                        }
                        if (reader.Name == "ID")
                        {
                            ID = reader.ReadElementContentAsInt();
                            addID.Add(ID);
                        }
                        if (reader.Name == "Wellness")
                        {
                            wellness = reader.ReadElementContentAsInt();
                            addWellness.Add(wellness);
                        }
                        if (reader.Name == "Quality")
                        {
                            quality = reader.ReadElementContentAsInt();
                            addQuality.Add(quality);
                        }
                        if (reader.Name == "Entry")
                        {
                            noteEntry = reader.ReadElementContentAsString();
                            addNotes.Add(noteEntry);
                        }
                        if (reader.Name == "Notes")
                        {
                            noteEntry = reader.ReadElementContentAsString();
                            addNotes.Add(noteEntry);
                        }
                        if (reader.Name == "File")
                        {
                            storedInfo = new FileInfo(reader.ReadElementContentAsString());
                            addFile.Add(storedInfo);
                        }

                    }
                    reader.Dispose();

                }
                MessageBox.Show(addLogs.Count().ToString() + " previous entries found");
                for (int i = 0; i < addLogs.Count; i++)
                {
                    if (addLogs[i] == "AudioLog")
                    {
                        AudioLogEntry oldEntry = new AudioLogEntry(addID[i], addWellness[i], addQuality[i], addNotes[i], addFile[i]);
                        absoluteList.Add(oldEntry);
                        logType = "AudioLog";
                        updateSummary(oldEntry);
                        textFirstEntry.Text = addFirst[i];
                        textNewestEntry.Text = addNewest[i];
                        fileInfoList.Add(oldEntry.GetFile().ToString());
                        oldEntry.AddToList(oldEntry);
                        listEntries.Items.Add(oldEntry.GetFile());
                        SaveXML();
                    }
                    if (addLogs[i] == "TextLog")
                    {
                        TextLogEntry oldEntry = new TextLogEntry(addID[i], addWellness[i], addQuality[i], addNotes[i], addFile[i]);
                        absoluteList.Add(oldEntry);
                        logType = "TextLog";
                        updateSummary(oldEntry);
                        textFirstEntry.Text = addFirst[i];
                        textNewestEntry.Text = addNewest[i];
                        fileInfoList.Add(oldEntry.GetFile().ToString());
                        oldEntry.AddToList(oldEntry);
                        listEntries.Items.Add(oldEntry.GetFile());
                        SaveXML();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No previous entries found");
            }
        }

        private void buttonDeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < absoluteList.Count; i++)
            {
                if (absoluteList[i].GetFile() == listEntries.SelectedItem)
                {
                    absoluteList.Remove(absoluteList[i]);
                    listEntries.Items.Remove(listEntries.SelectedItem);
                }
            }
        }

        private void buttonEditSelected_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < absoluteList.Count; i++)
            {
                if (absoluteList[i].GetFile() == listEntries.SelectedItem)
                {
                    if (absoluteList[i].GetType() == typeof(AudioLogEntry))
                    {
                        tabController.SelectedItem = tabEntry;
                        textNotes.Text = absoluteList[i].GetNotes();
                        comboQuality.Text = absoluteList[i].GetQuality().ToString();
                        comboWellness.Text = absoluteList[i].GetWellness().ToString();
                        storedInfo = absoluteList[i].GetFile();
                        recordingInfo = storedInfo.ToString();
                        buttonSave.IsEnabled = true;
                        buttonRecord.IsEnabled = false;
                        buttonPlay.IsEnabled = true;
                        buttonDelete.IsEnabled = false;
                    }
                    else
                    {
                        tabController.SelectedItem = tabTextRecord;
                        comboQualityText.Text = absoluteList[i].GetQuality().ToString();
                        comboWellness.Text = absoluteList[i].GetWellness().ToString();
                        textEntry.Text = absoluteList[i].GetEntry();
                        textEntry.IsEnabled = false;
                        storedInfo = absoluteList[i].GetFile();
                        buttonSaveText.IsEnabled = true;
                        buttonDeleteText.IsEnabled = false;
                        isPreStored = true;
                    }
                }
            }

        }
    }
}
