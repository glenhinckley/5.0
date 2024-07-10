namespace DCSGlobal.Medical.VoiceRecoder
{
    partial class RecordingPanel
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBoxRecordingApi = new System.Windows.Forms.GroupBox();
            this.radioButtonWasapiLoopback = new System.Windows.Forms.RadioButton();
            this.radioButtonWasapi = new System.Windows.Forms.RadioButton();
            this.radioButtonWaveInEvent = new System.Windows.Forms.RadioButton();
            this.radioButtonWaveIn = new System.Windows.Forms.RadioButton();
            this.comboWasapiDevices = new System.Windows.Forms.ComboBox();
            this.vuMeeter = new System.Windows.Forms.ProgressBar();
            this.tbVol = new System.Windows.Forms.TrackBar();
            this.lblTrackTotalTime = new System.Windows.Forms.Label();
            this.lblTrackTime = new System.Windows.Forms.Label();
            this.lblRecordingStatusMsg = new System.Windows.Forms.Label();
            this.lblRecordingStatus = new System.Windows.Forms.Label();
            this.cmdStart = new System.Windows.Forms.Button();
            this.cmdFinish = new System.Windows.Forms.Button();
            this.cmdStop = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.groupBoxRecordingApi.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbVol)).BeginInit();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(14, 91);
            this.progressBar1.Maximum = 30;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(749, 23);
            this.progressBar1.TabIndex = 4;
            // 
            // groupBoxRecordingApi
            // 
            this.groupBoxRecordingApi.Controls.Add(this.radioButtonWasapiLoopback);
            this.groupBoxRecordingApi.Controls.Add(this.radioButtonWasapi);
            this.groupBoxRecordingApi.Controls.Add(this.radioButtonWaveInEvent);
            this.groupBoxRecordingApi.Controls.Add(this.radioButtonWaveIn);
            this.groupBoxRecordingApi.Location = new System.Drawing.Point(987, 556);
            this.groupBoxRecordingApi.Name = "groupBoxRecordingApi";
            this.groupBoxRecordingApi.Size = new System.Drawing.Size(269, 112);
            this.groupBoxRecordingApi.TabIndex = 11;
            this.groupBoxRecordingApi.TabStop = false;
            this.groupBoxRecordingApi.Text = "Recording API";
            // 
            // radioButtonWasapiLoopback
            // 
            this.radioButtonWasapiLoopback.AutoSize = true;
            this.radioButtonWasapiLoopback.Location = new System.Drawing.Point(6, 86);
            this.radioButtonWasapiLoopback.Name = "radioButtonWasapiLoopback";
            this.radioButtonWasapiLoopback.Size = new System.Drawing.Size(118, 17);
            this.radioButtonWasapiLoopback.TabIndex = 8;
            this.radioButtonWasapiLoopback.Text = "WASAPI Loopback";
            this.radioButtonWasapiLoopback.UseVisualStyleBackColor = true;
            // 
            // radioButtonWasapi
            // 
            this.radioButtonWasapi.AutoSize = true;
            this.radioButtonWasapi.Location = new System.Drawing.Point(6, 63);
            this.radioButtonWasapi.Name = "radioButtonWasapi";
            this.radioButtonWasapi.Size = new System.Drawing.Size(67, 17);
            this.radioButtonWasapi.TabIndex = 9;
            this.radioButtonWasapi.Text = "WASAPI";
            this.radioButtonWasapi.UseVisualStyleBackColor = true;
            // 
            // radioButtonWaveInEvent
            // 
            this.radioButtonWaveInEvent.AutoSize = true;
            this.radioButtonWaveInEvent.Location = new System.Drawing.Point(6, 40);
            this.radioButtonWaveInEvent.Name = "radioButtonWaveInEvent";
            this.radioButtonWaveInEvent.Size = new System.Drawing.Size(140, 17);
            this.radioButtonWaveInEvent.TabIndex = 10;
            this.radioButtonWaveInEvent.Text = "waveIn Event Callbacks";
            this.radioButtonWaveInEvent.UseVisualStyleBackColor = true;
            // 
            // radioButtonWaveIn
            // 
            this.radioButtonWaveIn.AutoSize = true;
            this.radioButtonWaveIn.Checked = true;
            this.radioButtonWaveIn.Location = new System.Drawing.Point(6, 17);
            this.radioButtonWaveIn.Name = "radioButtonWaveIn";
            this.radioButtonWaveIn.Size = new System.Drawing.Size(60, 17);
            this.radioButtonWaveIn.TabIndex = 11;
            this.radioButtonWaveIn.TabStop = true;
            this.radioButtonWaveIn.Text = "waveIn";
            this.radioButtonWaveIn.UseVisualStyleBackColor = true;
            // 
            // comboWasapiDevices
            // 
            this.comboWasapiDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboWasapiDevices.FormattingEnabled = true;
            this.comboWasapiDevices.Location = new System.Drawing.Point(520, 3);
            this.comboWasapiDevices.Name = "comboWasapiDevices";
            this.comboWasapiDevices.Size = new System.Drawing.Size(243, 21);
            this.comboWasapiDevices.TabIndex = 12;
            // 
            // vuMeeter
            // 
            this.vuMeeter.ForeColor = System.Drawing.Color.Green;
            this.vuMeeter.Location = new System.Drawing.Point(12, 3);
            this.vuMeeter.Name = "vuMeeter";
            this.vuMeeter.Size = new System.Drawing.Size(436, 21);
            this.vuMeeter.Step = 1;
            this.vuMeeter.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.vuMeeter.TabIndex = 134;
            // 
            // tbVol
            // 
            this.tbVol.Location = new System.Drawing.Point(6, 25);
            this.tbVol.Maximum = 100;
            this.tbVol.Name = "tbVol";
            this.tbVol.Size = new System.Drawing.Size(451, 42);
            this.tbVol.TabIndex = 133;
            this.tbVol.TickFrequency = 10;
            // 
            // lblTrackTotalTime
            // 
            this.lblTrackTotalTime.AutoSize = true;
            this.lblTrackTotalTime.Location = new System.Drawing.Point(338, 72);
            this.lblTrackTotalTime.Name = "lblTrackTotalTime";
            this.lblTrackTotalTime.Size = new System.Drawing.Size(28, 13);
            this.lblTrackTotalTime.TabIndex = 130;
            this.lblTrackTotalTime.Text = "0:00";
            // 
            // lblTrackTime
            // 
            this.lblTrackTime.AutoSize = true;
            this.lblTrackTime.Location = new System.Drawing.Point(268, 72);
            this.lblTrackTime.Name = "lblTrackTime";
            this.lblTrackTime.Size = new System.Drawing.Size(64, 13);
            this.lblTrackTime.TabIndex = 129;
            this.lblTrackTime.Text = "Track Time:";
            // 
            // lblRecordingStatusMsg
            // 
            this.lblRecordingStatusMsg.AutoSize = true;
            this.lblRecordingStatusMsg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lblRecordingStatusMsg.Location = new System.Drawing.Point(108, 72);
            this.lblRecordingStatusMsg.Name = "lblRecordingStatusMsg";
            this.lblRecordingStatusMsg.Size = new System.Drawing.Size(92, 13);
            this.lblRecordingStatusMsg.TabIndex = 128;
            this.lblRecordingStatusMsg.Text = "Ready To Record";
            // 
            // lblRecordingStatus
            // 
            this.lblRecordingStatus.AutoSize = true;
            this.lblRecordingStatus.Location = new System.Drawing.Point(14, 72);
            this.lblRecordingStatus.Name = "lblRecordingStatus";
            this.lblRecordingStatus.Size = new System.Drawing.Size(95, 13);
            this.lblRecordingStatus.TabIndex = 127;
            this.lblRecordingStatus.Text = "Recording Status: ";
            // 
            // cmdStart
            // 
            this.cmdStart.Location = new System.Drawing.Point(520, 62);
            this.cmdStart.Name = "cmdStart";
            this.cmdStart.Size = new System.Drawing.Size(75, 23);
            this.cmdStart.TabIndex = 126;
            this.cmdStart.Text = "Start";
            this.cmdStart.UseVisualStyleBackColor = true;
            this.cmdStart.Click += new System.EventHandler(this.cmdStart_Click);
            // 
            // cmdFinish
            // 
            this.cmdFinish.Location = new System.Drawing.Point(682, 62);
            this.cmdFinish.Name = "cmdFinish";
            this.cmdFinish.Size = new System.Drawing.Size(75, 23);
            this.cmdFinish.TabIndex = 137;
            this.cmdFinish.Text = "Finish";
            this.cmdFinish.UseVisualStyleBackColor = true;
            // 
            // cmdStop
            // 
            this.cmdStop.Location = new System.Drawing.Point(601, 62);
            this.cmdStop.Name = "cmdStop";
            this.cmdStop.Size = new System.Drawing.Size(75, 23);
            this.cmdStop.TabIndex = 136;
            this.cmdStop.Text = "Stop";
            this.cmdStop.UseVisualStyleBackColor = true;
            this.cmdStop.Click += new System.EventHandler(this.cmdStop_Click);
            // 
            // RecordingPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboWasapiDevices);
            this.Controls.Add(this.cmdFinish);
            this.Controls.Add(this.cmdStop);
            this.Controls.Add(this.vuMeeter);
            this.Controls.Add(this.tbVol);
            this.Controls.Add(this.lblTrackTotalTime);
            this.Controls.Add(this.lblTrackTime);
            this.Controls.Add(this.lblRecordingStatusMsg);
            this.Controls.Add(this.lblRecordingStatus);
            this.Controls.Add(this.cmdStart);
            this.Controls.Add(this.groupBoxRecordingApi);
            this.Controls.Add(this.progressBar1);
            this.Name = "RecordingPanel";
            this.Size = new System.Drawing.Size(777, 184);
            this.Load += new System.EventHandler(this.RecordingPanel_Load);
            this.groupBoxRecordingApi.ResumeLayout(false);
            this.groupBoxRecordingApi.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbVol)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.GroupBox groupBoxRecordingApi;
        private System.Windows.Forms.ComboBox comboWasapiDevices;
        private System.Windows.Forms.RadioButton radioButtonWasapiLoopback;
        private System.Windows.Forms.RadioButton radioButtonWasapi;
        private System.Windows.Forms.RadioButton radioButtonWaveInEvent;
        private System.Windows.Forms.RadioButton radioButtonWaveIn;
        private System.Windows.Forms.ProgressBar vuMeeter;
        private System.Windows.Forms.TrackBar tbVol;
        private System.Windows.Forms.Label lblTrackTotalTime;
        private System.Windows.Forms.Label lblTrackTime;
        private System.Windows.Forms.Label lblRecordingStatusMsg;
        private System.Windows.Forms.Label lblRecordingStatus;
        private System.Windows.Forms.Button cmdStart;
        private System.Windows.Forms.Button cmdFinish;
        private System.Windows.Forms.Button cmdStop;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
    }
}