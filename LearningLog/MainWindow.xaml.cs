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

        private void buttonRecord_Click(object sender, RoutedEventArgs e)
        {
            RecordWav.StartRecording();
            MessageBoxResult recording = MessageBox.Show("Recording in progress.\nClicking any button will end the recording.", "Recording in progress", MessageBoxButton.YesNo);
            if (recording == MessageBoxResult.Yes)
            {
                FileInfo newRecordingInfo = RecordWav.EndRecording();
                textNotes.Text = newRecordingInfo.ToString();
                recordingInfo = newRecordingInfo.ToString();
                buttonPlay.IsEnabled = true;
                buttonDelete.IsEnabled = true;
            }
            else
            {
                FileInfo newRecordingInfo = RecordWav.EndRecording();
                textNotes.Text = newRecordingInfo.ToString();
                recordingInfo = newRecordingInfo.ToString();
                buttonPlay.IsEnabled = true;
                buttonDelete.IsEnabled = true;
            }
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e, Exception fileNotFoundException)
        {
            try
            {
                File.Delete(recordingInfo);
            }
            catch
            {
                throw fileNotFoundException;           
            }
        }

        private void buttonPlay_Click(object sender, RoutedEventArgs e)
        {
            SoundPlayer newRecording = new SoundPlayer(recordingInfo);
            newRecording.Play();
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(Environment.CurrentDirectory + "Files\\RecordingInfo" + DateTime.Now.ToString("yyyyMMdd") + ".text", textNotes.Text);
        }

        private void textNotes_TextChanged(object sender, TextChangedEventArgs e)
        {
            buttonSave.IsEnabled = true;
        }
    }
}
