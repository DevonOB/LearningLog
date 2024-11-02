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

        string recordingInfo = string.Empty;
        string recordingNotes = string.Empty;
        private readonly Exception fileNotFoundException;

        private void buttonRecord_Click(object sender, RoutedEventArgs e)
        {
            //RecordWav.StartRecording();
            MessageBoxResult recording = MessageBox.Show("Recording in progress.\nClicking any button will end the recording.", "Recording in progress", MessageBoxButton.YesNo);
            if (recording == MessageBoxResult.Yes)
            {
                FileInfo newRecordingInfo = RecordWav.EndRecording();
                recordingInfo = newRecordingInfo.ToString();
                buttonPlay.IsEnabled = true;
            }
            else
            {
                FileInfo newRecordingInfo = RecordWav.EndRecording();
                recordingInfo = newRecordingInfo.ToString();
                buttonPlay.IsEnabled = true;
            }
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                File.Delete(recordingInfo);
                recordingInfo = string.Empty;
                
            }
            catch
            {
                throw fileNotFoundException;           
            }
            try
            {
                File.Delete(recordingNotes);

            }
            catch
            {
                throw fileNotFoundException;
            }
        }

        private void buttonPlay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SoundPlayer newRecording = new SoundPlayer(recordingInfo);
                newRecording.Play();
            }
            catch
            {
                throw fileNotFoundException;
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            recordingNotes = Environment.CurrentDirectory + "Files\\RecordingNotes" + DateTime.Now.ToString("yyyyMMdd") + ".text";
            File.WriteAllText(recordingNotes, textNotes.Text);
            buttonDelete.IsEnabled = true;
        }

        private void textNotes_TextChanged(object sender, TextChangedEventArgs e)
        {
            buttonSave.IsEnabled = true;
        }
    }
}
