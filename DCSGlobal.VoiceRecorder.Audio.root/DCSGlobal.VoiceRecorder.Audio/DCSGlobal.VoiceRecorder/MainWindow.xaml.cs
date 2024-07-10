using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Collections.ObjectModel;
using System.IO;


using NAudio.Wave;



namespace DCSGlobal.VoiceRecorder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private static int Login = -1;
        private static string User_ID = string.Empty;
        private static int TrackID = 0;
        private static int TrackDetailID = 0;
        private static DateTime _Start_Time;
        private static DateTime _End_Time;
        private static string _TrackName = string.Empty;
        private static string _TrackPath = string.Empty;
        private int ElapsedTrackSeconds = 0;

        private int TrackDetailStart = 0;
        private int TrackDetailStop = 0;


        public WaveIn waveSource = null;
        public WaveFileWriter waveFile = null;

        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            LoadForm();



        }





        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            ElapsedTrackSeconds++;
        }

        void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (waveFile != null)
            {
                waveFile.Write(e.Buffer, 0, e.BytesRecorded);
                waveFile.Flush();
            }
        }

        void waveSource_RecordingStopped(object sender, StoppedEventArgs e)
        {
            if (waveSource != null)
            {
                waveSource.Dispose();
                waveSource = null;
            }

            if (waveFile != null)
            {
                waveFile.Dispose();
                waveFile = null;
            }

            //  StartBtn.Enabled = true;
        }



        private void cmdStart_Click(object sender, RoutedEventArgs e)
        {
            _Start_Time = DateTime.Now;
            _End_Time = DateTime.Now;

            
            
            dispatcherTimer.Start();
            ElapsedTrackSeconds = 0;


            Guid g = Guid.NewGuid();
            _TrackName = Convert.ToString(g);
            _TrackName = _TrackName + ".wav";

            using (srVoiceRecorder.VoiceRecorderSoapClient sr = new srVoiceRecorder.VoiceRecorderSoapClient())
            {


                TrackID = sr.AddTrack(_TrackName, txtTrackSubject.Text, txtTrackNote.Text, txtContactPhoneNumber.Text, txtContactFirstName.Text,
                                      txtContactLastName.Text, txtContactEmail.Text, User_ID, Convert.ToString(_Start_Time), Convert.ToString(_End_Time), txtTrackAccountNumber.Text, txtTrackMRN.Text,
                                      txtTrackDOR.Text, "phone");
            }




            if (TrackID > 0)
            {


                //   StartBtn.Enabled = false;
                //  StopBtn.Enabled = true;

                waveSource = new WaveIn();
                waveSource.WaveFormat = new WaveFormat(44100, 1);

                waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
                waveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(waveSource_RecordingStopped);

                waveFile = new WaveFileWriter(@"C:\usr\" + _TrackName, waveSource.WaveFormat);

                waveSource.StartRecording();


            }

            cmdAddMarker.IsEnabled = true;
            cmdSaveMarker.IsEnabled = false;
            cmdCanelMarker.IsEnabled = false;
            lblStatusMsg.Content = "Recording";
            cmdStop.IsEnabled = true;
            cmdStart.IsEnabled = false; 

        }

        private void cmdFinish_Click(object sender, RoutedEventArgs e)
        {




            _End_Time = DateTime.Now;
            int TrackSent = -1;

            using (srVoiceRecorder.VoiceRecorderSoapClient sr = new srVoiceRecorder.VoiceRecorderSoapClient())
            {
                TrackID = sr.UpdateTrack(TrackID, _TrackName, txtTrackSubject.Text, txtTrackNote.Text, txtContactPhoneNumber.Text, txtContactFirstName.Text,
                                      txtContactLastName.Text, txtContactEmail.Text, User_ID, Convert.ToString(_Start_Time), Convert.ToString(_End_Time), txtTrackAccountNumber.Text, txtTrackMRN.Text,
                                      txtTrackDOR.Text, "phone");

                TrackSent = UpLoadTrack();
                sr.TrackSent(TrackID, TrackSent);
            }


            EndTrack();


        }

        private void cmdAddMarker_Click(object sender, RoutedEventArgs e)
        {


            //_End_Time = DateTime.Now;
           
            
           // int TrackSent = -1;
           TrackDetailStart = 0;
           TrackDetailStop = 0; 
            
           TrackDetailStart = ElapsedTrackSeconds;
            

            using (srVoiceRecorder.VoiceRecorderSoapClient sr = new srVoiceRecorder.VoiceRecorderSoapClient())
            {

                TrackDetailID = sr.AddTrackDetails(TrackID, txtTrackDetailSubject.Text, txtTrackDetailNote.Text, User_ID, TrackDetailStart, TrackDetailStop, txtTrackDetailAccountNumber.Text,
                                                    txtTrackDetailMRN.Text, txtTrackDetailDOR.Text);
                //                      txtContactLastName.Text, txtContactEmail.Text, User_ID, Convert.ToString(_Start_Time), Convert.ToString(_End_Time), txtTrackAccountNumber.Text, txtTrackMRN.Text,
                //                      txtTrackDOR.Text, "phone");




            }


            if (TrackDetailID > 0)
            {

                txtTrackDetailAccountNumber.Text = "";
                txtTrackDetailDOR.Text = "";
                txtTrackDetailMRN.Text = "";
                txtTrackDetailNote.Text = "";
                txtTrackDetailSubject.Text = "";



                txtTrackDetailAccountNumber.IsEnabled = true;
                txtTrackDetailDOR.IsEnabled = true;
                txtTrackDetailMRN.IsEnabled = true;
                txtTrackDetailNote.IsEnabled = true;
                txtTrackDetailSubject.IsEnabled = true;

                cmdCanelMarker.IsEnabled = true;
                cmdSaveMarker.IsEnabled = true;
                cmdAddMarker.IsEnabled = false;
            }



        }

        private void cmdLogin_Click_1(object sender, RoutedEventArgs e)
        {




            using (srSecurity.SecuritySoapClient l = new srSecurity.SecuritySoapClient())
            {

                Login = l.Login(txtUserID.Text, txtPassword.Text);

            }

            if (Login == 0)
            {

                User_ID = txtUserID.Text;
                lblLoginStatus.Content = User_ID;

                txtUserID.Visibility = System.Windows.Visibility.Hidden;
                lblPasswd.Visibility = System.Windows.Visibility.Hidden;
                txtPassword.Visibility = System.Windows.Visibility.Hidden;

                cmdLogOut.Visibility = System.Windows.Visibility.Visible;
                cmdLogin.Visibility = System.Windows.Visibility.Hidden;

                cmdStart.IsEnabled = true;
                cmdAddMarker.IsEnabled = true;
                cmdGetWaveInDevices.IsEnabled = true;

                lblStatusMsg.Content = "Ready";
                GetWaveInDevices();

            }

            else
            {
                lblStatusMsg.Content = "Login Failed";

            }




        }

        private void cmdLogOut_Click(object sender, RoutedEventArgs e)
        {

            User_ID = string.Empty;
            lblLoginStatus.Content = string.Empty;
            lblLoginStatus.Visibility = System.Windows.Visibility.Visible;
            Login = -1;
            cmdLogOut.Visibility = System.Windows.Visibility.Hidden;

            cmdLogin.Visibility = System.Windows.Visibility.Visible;
            txtUserID.Visibility = System.Windows.Visibility.Visible;
            lblPasswd.Visibility = System.Windows.Visibility.Visible;
            txtPassword.Visibility = System.Windows.Visibility.Visible;



            cmdStart.IsEnabled = false;
            cmdAddMarker.IsEnabled = false;
            cmdGetWaveInDevices.IsEnabled = false;
            cmdStop.IsEnabled = false;

            lblStatusMsg.Content = "Loged Out";

        }


        private int UpLoadTrack()
        {
            int r = -1;


            try
            {
                using (srFileTransferService.FileTransferClient f = new srFileTransferService.FileTransferClient())
                {
                    byte[] buff = null;
                    buff = FileToByteArray(@"C:\usr\" + _TrackName);
                    f.SaveFile(buff, "Glen", "audio", "c:\\webdir_data\\", "audiotest\\", _TrackName, true, "");
                    r = 0;
                }
            }
            catch (Exception ex)
            {



            }


            return r;

        }


        private void GetWaveInDevices()
        {

            list.Clear();

            for (int n = 0; n < WaveIn.DeviceCount; n++)
            {
                list.Add(WaveIn.GetCapabilities(n).ProductName);
            }


            if (list.Count > 0)
            {
                cmbSource.ItemsSource = list;
                cmbSource.SelectedIndex = 0;

            }
            else
            {
                list.Add("No Recording Device Found");
                cmbSource.SelectedIndex = 0;
            }


        }


        private void cmdGetWaveInDevices_Click(object sender, RoutedEventArgs e)
        {
            GetWaveInDevices();

        }


        public byte[] FileToByteArray(string fileName)
        {
            return File.ReadAllBytes(fileName);
        }

        private void cmdStop_Click(object sender, RoutedEventArgs e)
        {




            waveSource.StopRecording();
            cmdStart.IsEnabled = false;
            cmdStop.IsEnabled = false;
            cmdFinish.IsEnabled = true;

            lblStatusMsg.Content = "Recording Stoped Click Finish To Upload";

        }

        private void cmdSaveMarker_Click(object sender, RoutedEventArgs e)
        {

            TrackDetailStop = ElapsedTrackSeconds;
            
            myDataGrid.Items.Add(new MyData
            {
                id = TrackDetailID,
                AccountNumber = txtTrackDetailAccountNumber.Text,
                MRN = txtTrackDetailMRN.Text,
                DOR = txtTrackDetailDOR.Text,
                Subject = txtTrackDetailSubject.Text,
                Detail = txtTrackDetailNote.Text,
                Start = TrackDetailStart,
                Stop = TrackDetailStop
            });

            using (srVoiceRecorder.VoiceRecorderSoapClient sr = new srVoiceRecorder.VoiceRecorderSoapClient())
            {

                TrackDetailID = sr.UpdateTrackDetails(TrackID, txtTrackDetailSubject.Text, txtTrackDetailNote.Text, User_ID, TrackDetailStart, TrackDetailStop, txtTrackDetailAccountNumber.Text,
                                                    txtTrackDetailMRN.Text, txtTrackDetailDOR.Text);
                //                      txtContactLastName.Text, txtContactEmail.Text, User_ID, Convert.ToString(_Start_Time), Convert.ToString(_End_Time), txtTrackAccountNumber.Text, txtTrackMRN.Text,
                //                      txtTrackDOR.Text, "phone");




            }



            txtTrackDetailAccountNumber.Text = "";
            txtTrackDetailDOR.Text = "";
            txtTrackDetailMRN.Text = "";
            txtTrackDetailNote.Text = "";
            txtTrackDetailSubject.Text = "";


            txtTrackDetailAccountNumber.IsEnabled = false;
            txtTrackDetailDOR.IsEnabled = false;
            txtTrackDetailMRN.IsEnabled = false;
            txtTrackDetailNote.IsEnabled = false;
            txtTrackDetailSubject.IsEnabled = false;

            cmdSaveMarker.IsEnabled = false;
            cmdCanelMarker.IsEnabled = false;
            cmdAddMarker.IsEnabled = true;

        }

        private void cmdCanelMarker_Click(object sender, RoutedEventArgs e)
        {
            txtTrackDetailAccountNumber.Text = "";
            txtTrackDetailDOR.Text = "";
            txtTrackDetailMRN.Text = "";
            txtTrackDetailNote.Text = "";
            txtTrackDetailSubject.Text = "";


            txtTrackDetailAccountNumber.IsEnabled = false;
            txtTrackDetailDOR.IsEnabled = false;
            txtTrackDetailMRN.IsEnabled = false;
            txtTrackDetailNote.IsEnabled = false;
            txtTrackDetailSubject.IsEnabled = false;

            cmdSaveMarker.IsEnabled = false;
            cmdCanelMarker.IsEnabled = false;
            cmdAddMarker.IsEnabled = true;
        }



        private void LoadForm()
        {

            DataGridTextColumn col1 = new DataGridTextColumn();
            DataGridTextColumn col2 = new DataGridTextColumn();
            DataGridTextColumn col3 = new DataGridTextColumn();
            DataGridTextColumn col4 = new DataGridTextColumn();
            DataGridTextColumn col5 = new DataGridTextColumn();
            DataGridTextColumn col6 = new DataGridTextColumn();
            DataGridTextColumn col7 = new DataGridTextColumn();
            DataGridTextColumn col8 = new DataGridTextColumn();

            myDataGrid.Columns.Add(col1);
            myDataGrid.Columns.Add(col2);
            myDataGrid.Columns.Add(col3);
            myDataGrid.Columns.Add(col4);
            myDataGrid.Columns.Add(col5);
            myDataGrid.Columns.Add(col6);
            myDataGrid.Columns.Add(col7);
            myDataGrid.Columns.Add(col8);

            col1.Binding = new Binding("id");
            col2.Binding = new Binding("AccountNumber");
            col3.Binding = new Binding("MRN");
            col4.Binding = new Binding("DOR");
            col5.Binding = new Binding("Subject");
            col6.Binding = new Binding("Detail");
            col7.Binding = new Binding("Start");
            col8.Binding = new Binding("Stop");

            col1.Header = "Track Detail ID";
            col2.Header = "Account Number";
            col3.Header = "MRN";
            col4.Header = "DOR";
            col5.Header = "Subject";
            col6.Header = "Detail";
            col7.Header = "Start";
            col8.Header = "Stop";

            // col1.Visibility = System.Windows.Visibility.Hidden;



            txtTrackDetailAccountNumber.Text = "";
            txtTrackDetailDOR.Text = "";
            txtTrackDetailMRN.Text = "";
            txtTrackDetailNote.Text = "";
            txtTrackDetailSubject.Text = "";



            txtTrackDetailAccountNumber.IsEnabled = false;
            txtTrackDetailDOR.IsEnabled = false;
            txtTrackDetailMRN.IsEnabled = false;
            txtTrackDetailNote.IsEnabled = false;
            txtTrackDetailSubject.IsEnabled = false;

            cmdSaveMarker.IsEnabled = false;
            cmdCanelMarker.IsEnabled = false;
            cmdAddMarker.IsEnabled = false;

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);





        }


        private void EndTrack()
        {

            txtTrackDetailAccountNumber.Text = "";
            txtTrackDetailDOR.Text = "";
            txtTrackDetailMRN.Text = "";
            txtTrackDetailNote.Text = "";
            txtTrackDetailSubject.Text = "";



            txtTrackDetailAccountNumber.IsEnabled = false;
            txtTrackDetailDOR.IsEnabled = false;
            txtTrackDetailMRN.IsEnabled = false;
            txtTrackDetailNote.IsEnabled = false;
            txtTrackDetailSubject.IsEnabled = false;


            cmdStart.IsEnabled = true;
            cmdStop.IsEnabled = false;
            cmdFinish.IsEnabled = false;

            cmdAddMarker.IsEnabled = false;
            //cmdGetWaveInDevices.IsEnabled = false;
            cmdStop.IsEnabled = false;
            cmdAddMarker.IsEnabled = false;
            cmdSaveMarker.IsEnabled = false;
            cmdCanelMarker.IsEnabled = false;

        }

    }
}
