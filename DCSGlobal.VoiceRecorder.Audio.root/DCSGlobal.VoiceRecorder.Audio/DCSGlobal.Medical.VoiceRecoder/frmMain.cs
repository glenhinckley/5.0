using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;

using System.Threading.Tasks;
using System.Threading;
using System.Timers;

using System.Runtime.InteropServices;
using System.Windows.Threading;
using System.IO;

using NAudio.Wave;

using NAudio.CoreAudioApi;
using NAudio.Mixer;

using DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff;
using DCSGlobal.BusinessRules.CoreLibraryII;
using DCSGlobal.BusinessRules.Security;


namespace DCSGlobal.Medical.VoiceRecoder
{
    public partial class frmMain : Form
    {

        private WMPLib.WindowsMediaPlayer player = new WMPLib.WindowsMediaPlayer();


        private IWaveIn waveIn;
        private INAudioDemoPlugin currentPlugin;

        private WaveFileWriter writer;


        private IWavePlayer waveOut;
        private AudioFileReader audioFileReader;
        private Action<float> setVolumeDelegate;


        private bool isFirstMarker = true;
        private int Login = -1;
        private string _UserName = string.Empty;
        private int _TrackID = 0;
        private int _TrackDetailID = 0;
        private DateTime _Start_Time;
        private DateTime _End_Time;
        private string _TrackName = string.Empty;
        private string _TrackPath = string.Empty;
        private int ElapsedTrackSeconds = 0;

        private int TrackDetailStart = 0;
        private int TrackDetailStop = 0;

        private int Mode = 0;

        private bool MarkerDirty = false;

        private int _ActiveTag = 0;

        private string _FilePath = @"c:\usr\";
        private int _DeviceNumber = 0;
        private string _DeviceName = string.Empty;

        LASTINPUTINFO lastInputInf = new LASTINPUTINFO();

        MMDeviceEnumerator enu = new MMDeviceEnumerator();



        AppSettings ap = new AppSettings();

        int TDCount = 0;




        public WaveIn waveSource = null;
        public WaveFileWriter waveFile = null;


        StringStuff ss = new StringStuff();


        [DllImport("user32.dll")]
        public static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);


        public frmMain()
        {



            InitializeComponent();


        }

        public int GetLastInputTime()
        {
            int idletime = 0;
            idletime = 0;
            lastInputInf.cbSize = Marshal.SizeOf(lastInputInf);
            lastInputInf.dwTime = 0;

            if (GetLastInputInfo(ref lastInputInf))
            {
                idletime = Environment.TickCount - lastInputInf.dwTime;
            }

            if (idletime != 0)
            {
                return idletime / 1000;
            }
            else
            {
                return 0;
            }
        }


        public string UserName
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                _UserName = value;

            }
        }

        public int RecorderMode
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        private void cmdGetWaveInDevices_Click(object sender, EventArgs e)
        {
            GetWaveinDevices();


        }

        private void cmdAddMarker_Click(object sender, EventArgs e)
        {




            AddMarker();


        }

        private void cmbStart_Click(object sender, EventArgs e)
        {
            StartRecording();
            AddMarker();


            cmdAddMarker.Enabled = true;
            cmdSaveMarker.Enabled = true;
            timer2.Enabled = true;

        }

        private void cmdStop_Click(object sender, EventArgs e)
        {
            StopRecording();



            SetupPlayBack();
            cmdEditSelectedRow.Enabled = true;

        }

        private void cmdFinish_Click(object sender, EventArgs e)
        {
            // make the little tracks

            // Convert all to mp3

            // send the track
            UpdateAndSendMasterTrack();
            // save all the markers
            SaveAllMarkers();

            // send the detail traks


            //cleanup

            this.Close();

        }

        private void cmdCanelMarker_Click(object sender, EventArgs e)
        {
            CancelMarker();
        }

        private void cmdSaveMarker_Click(object sender, EventArgs e)
        {
            SaveMarker();
        }





        private void frmMain_Load(object sender, EventArgs e)
        {



            GetWaveinDevices();
            BuildGrid();
            LoadForm();

        }


        void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {

            if (e.Exception != null)
            {
                MessageBox.Show(e.Exception.Message, "Playback Device Error");
            }
            if (audioFileReader != null)
            {
                audioFileReader.Position = 0;
            }
        }


        private void CloseWaveOut()
        {
            if (waveOut != null)
            {
                waveOut.Stop();
            }
            if (audioFileReader != null)
            {
                // this one really closes the file and ACM conversion
                audioFileReader.Dispose();
                setVolumeDelegate = null;
                audioFileReader = null;
            }
            if (waveOut != null)
            {
                waveOut.Dispose();
                waveOut = null;
            }
        }


        private void BuildGrid()
        {
            grdMarkers.Columns.Add("TrackDetailID", "Track Detail ID");
            grdMarkers.Columns.Add("HospCode", "Hosp Code");
            grdMarkers.Columns.Add("AccountNumber", "Account Number");
            grdMarkers.Columns.Add("MRN", "MRN");
            grdMarkers.Columns.Add("DOB", "DOB");
            grdMarkers.Columns.Add("Subject", "Subject");
            grdMarkers.Columns.Add("Note", "Note");
            grdMarkers.Columns.Add("Start", "Start");
            grdMarkers.Columns.Add("Stop", "Stop");

            grdMarkers.Columns["TrackDetailID"].Visible = false;
            grdMarkers.Columns["Start"].Visible = false;
            grdMarkers.Columns["Start"].Visible = false;
        }

        private void GetWaveinDevices()
        {
            var deviceEnum = new MMDeviceEnumerator();
            var devices = deviceEnum.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active).ToList();

            comboWasapiDevices.DataSource = devices;
            comboWasapiDevices.DisplayMember = "FriendlyName";



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



        private void UpdateAndSendMasterTrack()
        {
            _End_Time = DateTime.Now;
            int TrackSent = -1;

            using (srVoiceRecorder.VoiceRecorderSoapClient sr = new srVoiceRecorder.VoiceRecorderSoapClient())
            {

                _TrackID = sr.UpdateTrack(_TrackID, _Start_Time, _End_Time, _TrackName, "AUDIO_MASTER_TRACK", ap.ClientName, ap.RootDirectory, ap.ClientDirectory);
                TrackSent = UpLoadTrack("AUDIO_MASTER_TRACK", _TrackName);
                if (TrackSent == 0)
                {
                    sr.TrackSent(_TrackID);
                }
            }

            EndTrack();
            lblStatusMsg.Text = "Ready";





        }



        void StopRecording()
        {

            // waveSource.StopRecording();

            if (waveIn != null) waveIn.StopRecording();
            cmdStart.Enabled = false;
            cmdStop.Enabled = false;
            cmdFinish.Enabled = true;
            timer1.Enabled = false;
            lblRecordingStatusMsg.Text = "Recording Stoped Click Finish To Upload";
            lblRecordingStatusMsg.ForeColor = System.Drawing.Color.Black;


            var device = (MMDevice)comboWasapiDevices.SelectedItem;
            device.AudioEndpointVolume.Mute = true;

        }


        void StartRecording()
        {

            pgbTrackTime.Value = 0;
            pgbTrackTime.Visible = true;

            _Start_Time = DateTime.Now;
            _End_Time = DateTime.Now;

            Mode = 1; // recoring

            ElapsedTrackSeconds = 0;







            Guid g = Guid.NewGuid();
            _TrackName = Convert.ToString(g);
            _TrackName = _TrackName + ".wav";

            using (srVoiceRecorder.VoiceRecorderSoapClient sr = new srVoiceRecorder.VoiceRecorderSoapClient())
            {


                _TrackID = sr.AddTrack(_UserName, "phone", _TrackName);
            }


            // _TrackID = 4;

            if (_TrackID > 0)
            {


                //var device = (MMDevice)comboWasapiDevices.SelectedItem;

                //waveSource = new WaveIn();
                //waveSource.WaveFormat = new WaveFormat(8000, 1);



                //waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
                //waveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(waveSource_RecordingStopped);

                //waveFile = new WaveFileWriter(_FilePath + _TrackName, waveSource.WaveFormat);

                //waveSource.StartRecording();



                // naudio code

                Cleanup(); // WaveIn is still unreliable in some circumstances to being reused

                if (waveIn == null)
                {
                    waveIn = CreateWaveInDevice();
                }


                // Forcibly turn on the microphone (some programs (Skype) turn it off).
                var device = (MMDevice)comboWasapiDevices.SelectedItem;
                device.AudioEndpointVolume.Mute = false;
                device.AudioEndpointVolume.MasterVolumeLevel = 5;

                writer = new WaveFileWriter(Path.Combine(ap.FilePath, _TrackName), waveIn.WaveFormat);

                waveIn.StartRecording();

                lblRecordingStatusMsg.ForeColor = System.Drawing.Color.Red;
                lblRecordingStatusMsg.Text = "Recording";



            }

            timer1.Enabled = true;
            timer2.Enabled = true;
            cmdAddMarker.Enabled = true;
            cmdSaveMarker.Enabled = false;
            cmdCanelMarker.Enabled = false;
            lblStatusMsg.Text = "Recording";
            cmdStop.Enabled = true;
            cmdStart.Enabled = false;


        }


        private int UpLoadTrack(string HOSP_CODE, string TRACK_NAME)
        {
            int r = -1;
            try
            {
                using (srFileTransferService.FileTransferClient f = new srFileTransferService.FileTransferClient())
                {
                    byte[] buff = null;
                    buff = FileToByteArray(_FilePath + TRACK_NAME);
                    f.SaveFile(buff, ap.ClientName, HOSP_CODE, ap.RootDirectory, ap.ClientDirectory, TRACK_NAME, true, "");
                    r = 0;
                }
            }
            catch (Exception ex)
            {

            }


            return r;

        }





        public byte[] FileToByteArray(string fileName)
        {
            return File.ReadAllBytes(fileName);
        }


        private void EndTrack()
        {

            ElapsedTrackSeconds = 0;

            txtAccountNumber.Enabled = false;
            dtDOB.Enabled = false;
            txtMRN.Enabled = false;
            txtNote.Enabled = false;
            txtSubject.Enabled = false;


            cmdStart.Enabled = true;
            cmdStop.Enabled = false;
            cmdFinish.Enabled = false;

            cmdAddMarker.Enabled = false;
            //cmdGetWaveInDevices.IsEnabled = false;
            cmdStop.Enabled = false;
            cmdAddMarker.Enabled = false;
            cmdSaveMarker.Enabled = false;
            cmdCanelMarker.Enabled = false;
            cmdUpdateMarker.Enabled = false;
            cmdEditSelectedRow.Enabled = false;
            cmbHospCode.SelectedIndex = 0;
            cmbHospCode.Enabled = false;
            pgbTrackTime.Value = 0;

            grdMarkers.Rows.Clear();
            grdMarkers.Refresh();

            lblTrackTotalTime.Text = "0:00";
            lblRecordingStatusMsg.Text = "Ready To Record";
            lblRecordingStatusMsg.ForeColor = System.Drawing.Color.DarkGreen;
            File.Delete(_FilePath + _TrackName);


        }




        private void LoadForm()
        {


            ap.GetAll();

            // use reflection to find all the demos
            var demos = ReflectionHelper.CreateAllInstancesOf<INAudioDemoPlugin>().OrderBy(d => d.Name);






            GetHospCodes();

            txtAccountNumber.Enabled = false;
            dtDOB.Enabled = false;
            txtMRN.Enabled = false;
            txtNote.Enabled = false;
            txtSubject.Enabled = false;


            cmdStart.Enabled = true;
            cmdStop.Enabled = false;
            cmdFinish.Enabled = false;

            cmdAddMarker.Enabled = false;
            //cmdGetWaveInDevices.IsEnabled = false;
            cmdStop.Enabled = false;
            cmdAddMarker.Enabled = false;
            cmdSaveMarker.Enabled = false;
            cmdCanelMarker.Enabled = false;
            cmdEditSelectedRow.Enabled = false;
            cmdUpdateMarker.Enabled = false;


            cmbHospCode.Enabled = false;

            //  tbVol.Value = ap.MicLevel;

            lblStatusMsg.Text = "Loged in as: " + _UserName;



        }



        void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            if (InvokeRequired)
            {
                //Debug.WriteLine("Data Available");
                BeginInvoke(new EventHandler<WaveInEventArgs>(OnDataAvailable), sender, e);
            }
            else
            {
                //Debug.WriteLine("Flushing Data Available");
                writer.Write(e.Buffer, 0, e.BytesRecorded);
                int secondsRecorded = (int)(writer.Length / writer.WaveFormat.AverageBytesPerSecond);

            }
        }



        private IWaveIn CreateWaveInDevice()
        {
            IWaveIn newWaveIn;

            //newWaveIn = new WaveIn();
            //newWaveIn.WaveFormat = new WaveFormat(8000, 1);
            // can't set WaveFormat as WASAPI doesn't support SRC
            var device = (MMDevice)comboWasapiDevices.SelectedItem;
            newWaveIn = new WasapiCapture(device);

            newWaveIn.DataAvailable += OnDataAvailable;
            newWaveIn.RecordingStopped += OnRecordingStopped;
            return newWaveIn;
        }

        private void OnRecordingStopped(object sender, StoppedEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new EventHandler<StoppedEventArgs>(OnRecordingStopped), sender, e);
            }
            else
            {
                FinalizeWaveFile();
                pgbTrackTime.Value = 0;
                if (e.Exception != null)
                {
                    MessageBox.Show(String.Format("A problem was encountered during recording {0}",
                                                  e.Exception.Message));
                }
                //   int newItemIndex = listBoxRecordings.Items.Add(outputFilename);
                //   listBoxRecordings.SelectedIndex = newItemIndex;
                // SetControlStates(false);
            }
        }





        private void GetHospCodes()
        {
            string hospCodeList = string.Empty;
            string t = string.Empty;
            List<string> lHospCode = new List<string>();

            using (srVoiceRecorder.VoiceRecorderSoapClient sr = new srVoiceRecorder.VoiceRecorderSoapClient())
            {

                hospCodeList = sr.GetHospCode();



            }
            int c = 0;
            int cH = 0;

            cH = ss.CountCharacters(hospCodeList, "|");



            for (c = 0; c < cH + 1; c++)
            {
                t = string.Empty;

                lHospCode.Add(ss.ParseDemlimtedString(hospCodeList, "|", c));


            }




            BindingSource bs = new BindingSource();
            bs.DataSource = lHospCode;
            cmbHospCode.DataSource = bs;

        }



        private void CancelMarker()
        {

            txtAccountNumber.Text = "";
            dtDOB.Text = "";
            txtMRN.Text = "";
            txtNote.Text = "";
            txtSubject.Text = "";


            txtAccountNumber.Enabled = false;
            dtDOB.Enabled = false;
            txtMRN.Enabled = false;
            txtNote.Enabled = false;
            txtSubject.Enabled = false;

            cmdSaveMarker.Enabled = false;
            cmdCanelMarker.Enabled = false;
            cmdAddMarker.Enabled = true;
            cmbHospCode.Enabled = false;

        }

        private void SaveMarker()
        {
            TrackDetailStop = ElapsedTrackSeconds;



            using (srVoiceRecorder.VoiceRecorderSoapClient sr = new srVoiceRecorder.VoiceRecorderSoapClient())
            {

                string h = Convert.ToString(cmbHospCode.SelectedValue);

                //  _TrackDetailID = sr.UpdateTrackDetails(_TrackID, txtSubject.Text, txtNote.Text, _UserName, TrackDetailStart, TrackDetailStop, txtAccountNumber.Text,
                //  txtMRN.Text, dtDOB.Text, h);
                //                      txtContactLastName.Text, txtContactEmail.Text, User_ID, Convert.ToString(_Start_Time), Convert.ToString(_End_Time), txtTrackAccountNumber.Text, txtTrackMRN.Text,
                //                      txtTrackDOR.Text, "phone");




            }







            txtAccountNumber.Text = "";
            dtDOB.Text = "";
            txtMRN.Text = "";
            txtNote.Text = "";
            txtSubject.Text = "";


            txtAccountNumber.Enabled = false;
            dtDOB.Enabled = false;
            txtMRN.Enabled = false;
            txtNote.Enabled = false;
            txtSubject.Enabled = false;

            cmdSaveMarker.Enabled = false;
            cmdCanelMarker.Enabled = false;
            cmdAddMarker.Enabled = true;
            cmbHospCode.Enabled = false;
        }

        private void AddMarker()
        {
            //_End_Time = DateTime.Now;


            // int TrackSent = -1;
            if (!isFirstMarker)
            {


                UpdateMarker(_TrackDetailID, "", txtSubject.Text, txtNote.Text,
                            _UserName, Convert.ToInt32(txtStart.Text),  Convert.ToInt32(txtStop.Text), Convert.ToString(cmbHospCode.SelectedValue),
                             txtAccountNumber.Text, txtMRN.Text, dtDOB.Value, "",
                            ap.ClientName, ap.RootDirectory, ap.ClientDirectory, _TrackName);


                UpdateGrid(_TrackDetailID, txtAccountNumber.Text, dtDOB.Value, txtMRN.Text, txtNote.Text, txtSubject.Text,
                           Convert.ToInt32(txtStart.Text), Convert.ToInt32(txtStop.Text), Convert.ToString(cmbHospCode.SelectedValue));
            }
            else
            {
                isFirstMarker = false;
            }


            if (MarkerDirty)
            {
                TrackDetailStop = ElapsedTrackSeconds;
                SaveMarker();
            }




            TrackDetailStart = 0;
            TrackDetailStop = 0;

            TrackDetailStart = ElapsedTrackSeconds;


            string h = Convert.ToString(cmbHospCode.SelectedValue);


            txtAccountNumber.Text = "";
            dtDOB.Text = "";
            txtMRN.Text = "";
            txtNote.Text = "";
            txtSubject.Text = "";


            using (srVoiceRecorder.VoiceRecorderSoapClient sr = new srVoiceRecorder.VoiceRecorderSoapClient())
            {

                _TrackDetailID = sr.AddTrackDetail(_TrackID, "");
            }

            // _ActiveTag = _TrackDetailID;



            grdMarkers.Rows.Add(Convert.ToString(_TrackDetailID), h, txtAccountNumber.Text, txtMRN.Text, dtDOB.Text, txtSubject.Text, txtNote.Text, Convert.ToString(TrackDetailStart), "0");

            txtStart.Text = Convert.ToString(TrackDetailStart);



            TDCount++;

            string StartTag = string.Empty;
            string StopTag = string.Empty;

            StartTag = Convert.ToString(_ActiveTag) + "StartTag";
            StopTag = Convert.ToString(_ActiveTag) + "StopTag";

            // Tags(TDCount, StartTag, StopTag);

            if (_TrackDetailID > 0)
            {


                txtAccountNumber.Enabled = true;
                dtDOB.Enabled = true;
                txtMRN.Enabled = true;
                txtNote.Enabled = true;
                txtSubject.Enabled = true;

                cmdAddMarker.Enabled = true;


                cmdSaveMarker.Enabled = true;
                cmdCanelMarker.Enabled = true;

                cmbHospCode.Enabled = true;


                cmdEditSelectedRow.Enabled = false;
                cmdUpdateMarker.Enabled = false;



                txtAccountNumber.Text = "";
                dtDOB.Text = "";
                txtMRN.Text = "";
                txtNote.Text = "";
                txtSubject.Text = "";

            }
        }


        private void cmdEditSelectedRow_Click(object sender, EventArgs e)
        {

            lblStatusMsg.Text = Convert.ToString(grdMarkers.CurrentCell.RowIndex);
            txtAccountNumber.Enabled = true;
            dtDOB.Enabled = true;
            txtMRN.Enabled = true;
            txtNote.Enabled = true;
            txtSubject.Enabled = true;

            cmdAddMarker.Enabled = false;
            cmdSaveMarker.Enabled = false;
            cmdCanelMarker.Enabled = false;
            cmdUpdateMarker.Enabled = true;
            cmbHospCode.Enabled = true;
            cmdPBPlay.Enabled = true;



            if (grdMarkers.SelectedRows.Count > 0) // make sure user select at least 1 row 
            {

                string HospCode = string.Empty;

                _TrackDetailID = Convert.ToInt32(grdMarkers.SelectedRows[0].Cells["TrackDetailID"].Value);
                txtTrackDetailID.Text = Convert.ToString(_TrackDetailID);
                HospCode = grdMarkers.SelectedRows[0].Cells["HospCode"].Value + string.Empty;
                txtAccountNumber.Text = grdMarkers.SelectedRows[0].Cells["AccountNumber"].Value + string.Empty;
                dtDOB.Text = grdMarkers.SelectedRows[0].Cells["DOR"].Value + string.Empty;
                txtMRN.Text = grdMarkers.SelectedRows[0].Cells["MRN"].Value + string.Empty;
                txtNote.Text = grdMarkers.SelectedRows[0].Cells["NOTE"].Value + string.Empty;
                txtSubject.Text = grdMarkers.SelectedRows[0].Cells["Subject"].Value + string.Empty;
                TrackDetailStart = Convert.ToInt32(grdMarkers.SelectedRows[0].Cells["Start"].Value + string.Empty);
                TrackDetailStop = Convert.ToInt32(grdMarkers.SelectedRows[0].Cells["Stop"].Value + string.Empty);

                if (HospCode != "")
                {
                    // cmbHospCode.SelectedText = HospCode;
                }


            }

        }

        private void cmdUpdateMarker_Click(object sender, EventArgs e)
        {


            // this only activate after the user clicks on stop recording
            //    UpdateMarker(_TrackDetailID);
            //   UpdateGrid(_TrackDetailID);

            string __AccountNumber = string.Empty;
            DateTime  __DOB; // = string.Empty;
            string __MRN = string.Empty;
            string __NOTE = string.Empty;
            string __Subject = string.Empty;
            int __start = 0;
            int __stop = 0;
            string __hosp = string.Empty;
            int _TD = 0;

            
            _TD = Convert.ToInt32(txtTrackDetailID.Text);
            __AccountNumber = txtAccountNumber.Text;
            __DOB = dtDOB.Value;
            __MRN = txtMRN.Text;
            __NOTE = txtNote.Text;
            __Subject = txtSubject.Text;
            __start = Convert.ToInt32(txtStart.Text);
            __stop = Convert.ToInt32(txtStop.Text);
            __hosp = Convert.ToString(cmbHospCode.SelectedValue);

            UpdateMarker(_TD, "", __Subject, __NOTE,
                                  UserName, __start, __stop, "",
                                  __AccountNumber, __MRN, __DOB, "",
                                  ap.ClientName, ap.RootDirectory, ap.ClientDirectory, _TrackName);

            UpdateGrid(_TD, __AccountNumber, __DOB, __MRN, __NOTE, __Subject, __start, __stop, __hosp);


            switch (Mode)
            {
                case 1:  // onging recording so just update the grid and save it



                    break;
                case 2:


                    txtAccountNumber.Text = "";
                    dtDOB.Text = "";
                    txtMRN.Text = "";
                    txtNote.Text = "";
                    txtSubject.Text = "";
                    txtAccountNumber.Enabled = false;
                    dtDOB.Enabled = false;
                    txtMRN.Enabled = false;
                    txtNote.Enabled = false;
                    txtSubject.Enabled = false;


                    cmdAddMarker.Enabled = false;


                    cmdSaveMarker.Enabled = false;
                    cmdCanelMarker.Enabled = false;
                    cmdUpdateMarker.Enabled = false;
                    cmbHospCode.Enabled = false;
                    cmdPBPlay.Enabled = false;

                    break;
            }


        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            ElapsedTrackSeconds++;
            if (ElapsedTrackSeconds == pgbTrackTime.Maximum)
            {

                int i = 0;

                i = pgbTrackTime.Maximum;

                i = i * 2;

                pgbTrackTime.Maximum = i;

            }

            pgbTrackTime.Value = ElapsedTrackSeconds;




            int seconds = ElapsedTrackSeconds % 60;
            int minutes = ElapsedTrackSeconds / 60;


            int Aseconds = pgbTrackTime.Maximum % 60;
            int Aminutes = pgbTrackTime.Maximum / 60;

            lblATime.Text = String.Format("{0:00}:{1:00}", (int)Aminutes, Aseconds);


            lblTrackTotalTime.Text = String.Format("{0:00}:{1:00}", (int)minutes, seconds);



            string time = minutes + ":" + seconds;
            TrackDetailStop = ElapsedTrackSeconds;

            MoveActiveMarker(_ActiveTag, TrackDetailStart, TrackDetailStop);


            //  lblTrackTotalTime.Text = time;

            //time.Text 

            txtStop.Text = Convert.ToString(ElapsedTrackSeconds);




        }

        private void SaveAllMarkers()
        {


            string __AccountNumber = string.Empty;
            string __DOB = string.Empty;
            string __MRN = string.Empty;
            string __NOTE = string.Empty;
            string __Subject = string.Empty;
            int __start = 0;
            int __stop = 0;
            string __hosp = string.Empty;
            int __TD = 0;
            string __TrackDetailFileName = string.Empty;


            //  grdMarkers.SelectedRows.Clear();

            foreach (DataGridViewRow row in grdMarkers.Rows)
            {




                __AccountNumber = string.Empty;
                __DOB = string.Empty;
                __MRN = string.Empty;
                __NOTE = string.Empty;
                __Subject = string.Empty;
                __start = 0;
                __stop = 0;
                __hosp = string.Empty;
                __TD = 0;
                __TrackDetailFileName = string.Empty;



                __TD = Convert.ToInt32(((DataGridViewTextBoxCell)row.Cells[0]).Value);

                __TrackDetailFileName = Convert.ToString(__TD) + "_" + _TrackName;

                row.Selected = true;

                __AccountNumber = grdMarkers.SelectedRows[0].Cells["AccountNumber"].Value + string.Empty;
                __DOB = grdMarkers.SelectedRows[0].Cells["DOB"].Value + string.Empty;
                __MRN = grdMarkers.SelectedRows[0].Cells["MRN"].Value + string.Empty;
                __NOTE = grdMarkers.SelectedRows[0].Cells["NOTE"].Value + string.Empty;
                __Subject = grdMarkers.SelectedRows[0].Cells["Subject"].Value + string.Empty;
                __start = Convert.ToInt32(grdMarkers.SelectedRows[0].Cells["Start"].Value);
                __stop = Convert.ToInt32(grdMarkers.SelectedRows[0].Cells["Stop"].Value);
                __hosp = grdMarkers.SelectedRows[0].Cells["HospCode"].Value + string.Empty;

                UpdateMarker(__TD, __TrackDetailFileName, __Subject, __NOTE,
                             _UserName, Convert.ToInt32(__start), Convert.ToInt32(__stop), __hosp,
                             __AccountNumber, __MRN, Convert.ToDateTime(__DOB), "",
                             ap.ClientName, ap.RootDirectory, ap.ClientDirectory, _TrackName);


            }




        }



        private void MoveActiveMarker(int TrackDetailID, int start, int stop)
        {


            string StartTag = string.Empty;
            string StopTag = string.Empty;
            int SL = 0;
            double SF = 0.0;


            StartTag = Convert.ToString(TrackDetailID) + "StartTag";
            StopTag = Convert.ToString(TrackDetailID) + "StopTag";



            SF = stop * .205;

            stop = Convert.ToInt32(SF);

            foreach (Control c in this.Controls)
            {
                if (c is Button)
                {
                    string ctag = Convert.ToString(c.Tag);
                    if (Convert.ToString(c.Tag) == StartTag)
                    {
                        //  c.Left = start;
                        c.SendToBack();
                    }

                    if (Convert.ToString(c.Tag) == StopTag)
                    {

                        int l = c.Left;
                        l = l + stop;

                        c.Left = 18 + stop;
                    }
                    // Do stuff here ;]
                }
            }





        }



        private void UpdateMarker(int TrackDetailID, string TrackDetailFileName, string TrackDetailSubject, string TrackDetailNote,
                                  string UserID, int StartTime, int StopTime, string HOSP_CODE,
                                  string AccountNumber, string MRN, DateTime DOB, string SoruceDevice,
                                  string ClientName, string RootDirectory, string ClientDirectory, string FileName)
        {
            using (srVoiceRecorder.VoiceRecorderSoapClient sr = new srVoiceRecorder.VoiceRecorderSoapClient())
            {
                TrackDetailID = sr.UpdateTrackDetails(TrackDetailID, TrackDetailFileName, TrackDetailSubject, TrackDetailNote,
                                                      UserID, StartTime, StopTime, HOSP_CODE,
                                                      AccountNumber, MRN, DOB, SoruceDevice,
                                                      ClientName, RootDirectory, ClientDirectory, FileName);


            }
        }

        private void UpdateGrid(int TrackDetailID, string AccountNumber, DateTime  DOB, string MRN, string NOTE, string Subject, int start, int stop, string HOSP)
        {
            int _TD = 0;

            //  grdMarkers.SelectedRows.Clear();

            foreach (DataGridViewRow row in grdMarkers.Rows)
            {
                _TD = Convert.ToInt32(((DataGridViewTextBoxCell)row.Cells[0]).Value);


                if (_TrackDetailID == _TD)
                {
                    row.Selected = true;

                    grdMarkers.SelectedRows[0].Cells["AccountNumber"].Value = AccountNumber;
                    grdMarkers.SelectedRows[0].Cells["DOB"].Value = Convert.ToString(DOB);
                    grdMarkers.SelectedRows[0].Cells["MRN"].Value = MRN;
                    grdMarkers.SelectedRows[0].Cells["NOTE"].Value = NOTE;
                    grdMarkers.SelectedRows[0].Cells["Subject"].Value = Subject;
                    grdMarkers.SelectedRows[0].Cells["Start"].Value = start;
                    grdMarkers.SelectedRows[0].Cells["Stop"].Value = stop;
                    grdMarkers.SelectedRows[0].Cells["HospCode"].Value = HOSP;
                }
            }

        }



        private void TryGetVolumeControl()
        {
            int waveInDeviceNumber = 0;
            var mixerLine = new MixerLine((IntPtr)waveInDeviceNumber, 0, MixerFlags.WaveIn);


            foreach (var control in mixerLine.Controls)
            {
                if (control.ControlType == MixerControlType.Volume)
                {
                    // volumeControl = control as UnsignedMixerControl;
                    break;
                }
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            //  TryGetVolumeControl();

            if (comboWasapiDevices.SelectedItem != null)
            {
                var device = (MMDevice)comboWasapiDevices.SelectedItem;


                // lblStatusMsg.Text = Convert.ToString(device.AudioEndpointVolume.VolumeRange.MaxDecibels); //= 1;   

                lblStatusMsg.Text = Convert.ToString(device.AudioEndpointVolume.MasterVolumeLevel = tbVol.Value); //= 1;                 

                int i = (int)(Math.Round(device.AudioMeterInformation.MasterPeakValue * 100));

                if (i < 75)
                {
                    vuMeeter.ForeColor = System.Drawing.Color.Green;
                }

                if (i > 75)
                {
                    vuMeeter.ForeColor = System.Drawing.Color.OrangeRed;
                }
                if (i > 80)
                {
                    vuMeeter.ForeColor = System.Drawing.Color.Red;
                }

                vuMeeter.Value = i;
                //    device.AudioEndpointVolume = tbVol.Value;

            }
        }


        private void DisplayTime()
        {
            // totaltime = GetLastInputTime();
            if (GetLastInputTime().Equals(1))
            {
                //   Label1.Content = "Tempo di inattività pari a" + " " + GetLastInputTime().ToString() + " " + "secondo";
            }
            else
            {
                // Label1.Content = "Tempo di inattività pari a" + " " + GetLastInputTime().ToString() + " " + "secondi";
            }
        }


        private void cmdStart_Click(object sender, EventArgs e)
        {
            StartRecording();
            AddMarker();


            cmdAddMarker.Enabled = true;
            cmdSaveMarker.Enabled = false;
            timer2.Enabled = true;
        }


        private void Cleanup()
        {
            if (waveIn != null)
            {
                waveIn.Dispose();
                waveIn = null;
            }
            FinalizeWaveFile();
        }

        private void FinalizeWaveFile()
        {
            if (writer != null)
            {
                writer.Dispose();
                writer = null;
            }
        }


        void SetupPlayBack()
        {







        }


        [StructLayout(LayoutKind.Sequential)]
        public struct LASTINPUTINFO
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public int dwTime;
        }

        private void tbVol_Scroll(object sender, EventArgs e)
        {

        }


        private void PlayTrackDetail()
        {

            //    TrackDetailStart
            //   TrackDetailStop 

            string file = string.Empty;

            player.URL = file; //file to be played
            player.controls.play();
            player.controls.currentPosition = TrackDetailStart; //set the position you want here when user drag

            timPlayBack.Enabled = true;


        }




        private void cmdPBPlay_Click(object sender, EventArgs e)
        {

            ElapsedTrackSeconds = TrackDetailStart;
            PlayTrackDetail();
        }

        private void cmdPBPause_Click(object sender, EventArgs e)
        {

            //if (waveOut != null)
            //{
            //    if (waveOut.PlaybackState == PlaybackState.Playing)
            //    {
            //        waveOut.Pause();
            //    }
            //}

        }

        private void cmdPBStop_Click(object sender, EventArgs e)
        {
            //if (waveOut != null)
            //{
            //    waveOut.Stop();
            //}
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {




        }

        private void timPlayBack_Tick(object sender, EventArgs e)
        {

            // TrackDetailStop


            ElapsedTrackSeconds++;

            if (ElapsedTrackSeconds == TrackDetailStop)
            {
                player.controls.stop();
            }


        }



        public interface IOutputDevicePlugin
        {
            IWavePlayer CreateDevice(int latency);
            UserControl CreateSettingsPanel();
            string Name { get; }
            bool IsAvailable { get; }
            int Priority { get; }
        }


    }


}
