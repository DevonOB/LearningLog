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

        int ID = 0;
        string recordingInfo = string.Empty;
        string recordingNotes = string.Empty;
        FileInfo storedInfo;
        bool recording = false;

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
                buttonPlay.IsEnabled = true;
                recording = false;
                buttonRecord.IsEnabled = false;
                buttonDelete.IsEnabled = true;
            }
            
        }

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

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            ID++;
            LogEntry newEntry;
            newEntry = new LogEntry(ID, DateTime.Now.ToString("yyyyMMdd"), int.Parse(comboWellness.Text), int.Parse(comboQuality.Text), textNotes.Text, storedInfo);
            buttonDelete.IsEnabled = false;
            statusChange("Saved recording");
            buttonRecord.IsEnabled = true;
            buttonPlay.IsEnabled = false;
            buttonSave.IsEnabled = false;
            textNotes.Text = string.Empty;
        }

        private void textNotes_TextChanged(object sender, TextChangedEventArgs e)
        {
            buttonSave.IsEnabled = true;
            statusChange("Edited notes");
        }

        private void statusChange(string e)
        {
            textStatus.Text = e + " " +DateTime.Now;
        }
    }
}
