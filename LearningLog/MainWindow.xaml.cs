// Devon O'Brien
// 2024-11-02
// description: Allows the user to start and end recordings as well as listen to those recordings.
//              Also allows the user to delete the recordings and see where they are saved to.



using System.IO;
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
            ID++;
            LogEntry newEntry;
            string noteEntry = textNotes.Text;
            newEntry = new LogEntry(ID, DateTime.Now.ToString("yyyyMMdd"), int.Parse(comboWellness.Text), int.Parse(comboQuality.Text), noteEntry, storedInfo);
            buttonDelete.IsEnabled = false;
            statusChange("Saved recording");
            buttonRecord.IsEnabled = true;
            buttonPlay.IsEnabled = false;
            buttonSave.IsEnabled = false;
            updateSummary(newEntry);
            textNotes.Text = string.Empty;
            textNotes.IsEnabled = false;
            
        }

        // When notes change activates the save button
        private void textNotes_TextChanged(object sender, TextChangedEventArgs e)
        {
            buttonSave.IsEnabled = true;
            statusChange("Edited notes");
        }

        // sets the status bar
        private void statusChange(string e)
        {
            textStatus.Text = e + " " +DateTime.Now;
        }

        // update the summary page
        private void updateSummary(LogEntry entry)
        {
            textID.Text = entry.GetID().ToString();
            textDate.Text = entry.GetEntryDate();
            textSummaryNotes.Text = entry.GetNotes();
            textWellness.Text = entry.GetWellness().ToString();
            textQuality.Text = entry.GetQuality().ToString();
            textFile.Text = entry.GetRecordingFile().ToString();
        }
    }
}
