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
        }
        
        // sets global variables for use in the code
        int ID = 0;
        string recordingInfo = string.Empty;
        string recordingNotes = string.Empty;
        FileInfo storedInfo;
        bool recording = false;
        protected internal static List<String> fileInfoList = new List<String>();

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
                    string noteEntry = textNotes.Text;
                    newEntry = new AudioLogEntry(ID, int.Parse(comboWellness.Text), int.Parse(comboQuality.Text), noteEntry, storedInfo);                    buttonDelete.IsEnabled = false;
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
                    storedInfo = RecordText.SaveTextEntry(textEntry.Text);
                    string noteEntry = textEntry.Text;
                    newEntry = new TextLogEntry(ID, int.Parse(comboWellness.Text), int.Parse(comboQuality.Text), noteEntry, storedInfo);
                    buttonDeleteText.IsEnabled = false;
                    statusChange("Saved text");
                    buttonSaveText.IsEnabled = false;
                    updateSummary(newEntry);
                    textEntry.Text = string.Empty;
                }
                // Adds the saved item to both lists and the list tab
                if (!fileInfoList.Contains(newEntry.GetFile().ToString()))
                {
                    fileInfoList.Add(newEntry.GetFile().ToString());
                    newEntry.AddToList(newEntry);
                    listEntries.Items.Add(newEntry.GetFile());
                    SaveXML(newEntry);
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
            if (tabController.SelectedItem == tabEntry)
            {
                labelSummaryNotes.Content = "Notes:";
                textID.Text = entry.GetID().ToString();
                textNewestEntry.Text = entry.GetNewestRecording().ToString();
                textFirstEntry.Text = entry.GetFirstRecording().ToString();
                textSummaryNotes.Text = entry.GetNotes();
                textWellness.Text = entry.GetWellness().ToString();
                textQuality.Text = entry.GetQuality().ToString();
                textFile.Text = entry.GetFile().ToString();
            }
            // update summary for a text log
            else
            {
                labelSummaryNotes.Content = "Entry:";
                textID.Text = entry.GetID().ToString();
                textNewestEntry.Text = entry.GetNewestRecording().ToString();
                textFirstEntry.Text = entry.GetFirstRecording().ToString();
                textSummaryNotes.Text = entry.GetEntry();
                textWellness.Text = entry.GetWellness().ToString();
                textQuality.Text = entry.GetQuality().ToString();
                textFile.Text = entry.GetFile().ToString();
            }
        }

        /// <summary>
        /// saves to xml file hopefully
        /// </summary>
        private void SaveXML(LogEntry log)
        {
            try
            {
                int listLength = log.GetList(log).Count;
                List<LogEntry> logEntries = log.GetList(log);
                // starts a xml writer
                XmlWriter writer = XmlWriter.Create("LogEntry.xml");

                writer.WriteStartDocument();
                writer.WriteStartElement("LogEntry");
                // write an audio log
                for (int i = 0; i < listLength; i++)
                {

                    if (log.GetType() == typeof(AudioLogEntry))
                    {
                        writer.WriteStartElement("AudioLog");
                        writer.WriteElementString("Newest", logEntries[i].GetNewestRecording().ToString());
                        writer.WriteElementString("First", logEntries[i].GetFirstRecording().ToString());
                        writer.WriteElementString("ID", logEntries[i].GetID().ToString());
                        writer.WriteElementString("Wellness", logEntries[i].GetWellness().ToString());
                        writer.WriteElementString("Quality", logEntries[i].GetQuality().ToString());
                        writer.WriteElementString("Notes", logEntries[i].GetNotes());
                        writer.WriteElementString("File", logEntries[i].GetFile().ToString());
                        writer.WriteEndElement();
                    }
                    // write a text log
                    else
                    {
                        writer.WriteStartElement("TextLog");
                        writer.WriteElementString("Newest", logEntries[i].GetNewestRecording().ToString());
                        writer.WriteElementString("First", logEntries[i].GetFirstRecording().ToString());
                        writer.WriteElementString("ID", logEntries[i].GetID().ToString());
                        writer.WriteElementString("Wellness", logEntries[i].GetWellness().ToString());
                        writer.WriteElementString("Quality", logEntries[i].GetQuality().ToString());
                        writer.WriteElementString("Entry", logEntries[i].GetEntry());
                        writer.WriteElementString("File", logEntries[i].GetFile().ToString());
                        writer.WriteEndElement();
                    }
                }

                // close the xml writer
                writer.WriteEndDocument();
                writer.Flush();
                writer.Close();
            }
            catch
            {
                MessageBox.Show("Error unable to save XML file");
            }
        }
            

        /// <summary>
        /// 
        /// </summary>
        private void menuHelp_Click(object sender, RoutedEventArgs e)
        {
            statusChange("Viewed file information");
            MessageBox.Show("This here file belongs to Devon O'Brien\n" + DateTime.Now);
        }

        private void restore()
        {

        }
    }
}
