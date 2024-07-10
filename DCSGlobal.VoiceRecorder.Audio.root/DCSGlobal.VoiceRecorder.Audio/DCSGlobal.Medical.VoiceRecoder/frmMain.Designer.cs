namespace DCSGlobal.Medical.VoiceRecoder
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.cmdAddMarker = new System.Windows.Forms.Button();
            this.cmdSaveMarker = new System.Windows.Forms.Button();
            this.cmdCanelMarker = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblHospCode = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtMRN = new System.Windows.Forms.TextBox();
            this.txtAccountNumber = new System.Windows.Forms.TextBox();
            this.txtSubject = new System.Windows.Forms.TextBox();
            this.grdMarkers = new System.Windows.Forms.DataGridView();
            this.cmbHospCode = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblStatusMsg = new System.Windows.Forms.Label();
            this.cmdEditSelectedRow = new System.Windows.Forms.Button();
            this.cmdUpdateMarker = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.txtTrackDetailID = new System.Windows.Forms.TextBox();
            this.txtTrackID = new System.Windows.Forms.TextBox();
            this.txtStart = new System.Windows.Forms.TextBox();
            this.txtStop = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtNote = new GvS.Controls.HtmlTextbox();
            this.cmdStop7 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.button17 = new System.Windows.Forms.Button();
            this.button18 = new System.Windows.Forms.Button();
            this.button19 = new System.Windows.Forms.Button();
            this.cmdStop10 = new System.Windows.Forms.Button();
            this.cmdStop6 = new System.Windows.Forms.Button();
            this.cmdStop5 = new System.Windows.Forms.Button();
            this.cmdStop4 = new System.Windows.Forms.Button();
            this.cmdStop3 = new System.Windows.Forms.Button();
            this.cmdStop9 = new System.Windows.Forms.Button();
            this.cmdStop2 = new System.Windows.Forms.Button();
            this.cmdStart7 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.cmdStart10 = new System.Windows.Forms.Button();
            this.cmdStart6 = new System.Windows.Forms.Button();
            this.cmdStart5 = new System.Windows.Forms.Button();
            this.cmdStart4 = new System.Windows.Forms.Button();
            this.cmdStart3 = new System.Windows.Forms.Button();
            this.cmdStart9 = new System.Windows.Forms.Button();
            this.cmdStart2 = new System.Windows.Forms.Button();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.dtDOB = new System.Windows.Forms.DateTimePicker();
            this.cmdFinish = new System.Windows.Forms.Button();
            this.cmdStop = new System.Windows.Forms.Button();
            this.lblTrackTotalTime = new System.Windows.Forms.Label();
            this.lblTrackTime = new System.Windows.Forms.Label();
            this.lblRecordingStatusMsg = new System.Windows.Forms.Label();
            this.lblRecordingStatus = new System.Windows.Forms.Label();
            this.cmdStart = new System.Windows.Forms.Button();
            this.pgbTrackTime = new System.Windows.Forms.ProgressBar();
            this.comboWasapiDevices = new System.Windows.Forms.ComboBox();
            this.vuMeeter = new System.Windows.Forms.ProgressBar();
            this.tbVol = new System.Windows.Forms.TrackBar();
            this.lblATime = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmdPBPlay = new System.Windows.Forms.Button();
            this.timPlayBack = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.grdMarkers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbVol)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdAddMarker
            // 
            this.cmdAddMarker.Location = new System.Drawing.Point(682, 295);
            this.cmdAddMarker.Name = "cmdAddMarker";
            this.cmdAddMarker.Size = new System.Drawing.Size(75, 23);
            this.cmdAddMarker.TabIndex = 1;
            this.cmdAddMarker.Text = "Add Marker";
            this.cmdAddMarker.UseVisualStyleBackColor = true;
            this.cmdAddMarker.Click += new System.EventHandler(this.cmdAddMarker_Click);
            // 
            // cmdSaveMarker
            // 
            this.cmdSaveMarker.Location = new System.Drawing.Point(642, 527);
            this.cmdSaveMarker.Name = "cmdSaveMarker";
            this.cmdSaveMarker.Size = new System.Drawing.Size(109, 23);
            this.cmdSaveMarker.TabIndex = 2;
            this.cmdSaveMarker.Text = "Save Marker";
            this.cmdSaveMarker.UseVisualStyleBackColor = true;
            this.cmdSaveMarker.Click += new System.EventHandler(this.cmdSaveMarker_Click);
            // 
            // cmdCanelMarker
            // 
            this.cmdCanelMarker.Location = new System.Drawing.Point(421, 527);
            this.cmdCanelMarker.Name = "cmdCanelMarker";
            this.cmdCanelMarker.Size = new System.Drawing.Size(100, 23);
            this.cmdCanelMarker.TabIndex = 3;
            this.cmdCanelMarker.Text = "Cancel";
            this.cmdCanelMarker.UseVisualStyleBackColor = true;
            this.cmdCanelMarker.Click += new System.EventHandler(this.cmdCanelMarker_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(245, 300);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "MRN:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 343);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Subject:";
            // 
            // lblHospCode
            // 
            this.lblHospCode.AutoSize = true;
            this.lblHospCode.Location = new System.Drawing.Point(7, 300);
            this.lblHospCode.Name = "lblHospCode";
            this.lblHospCode.Size = new System.Drawing.Size(63, 13);
            this.lblHospCode.TabIndex = 8;
            this.lblHospCode.Text = "Hosp Code:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(137, 301);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Account Number:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(348, 300);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(33, 13);
            this.label11.TabIndex = 15;
            this.label11.Text = "DOB:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(8, 380);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(33, 13);
            this.label15.TabIndex = 19;
            this.label15.Text = "Note:";
            // 
            // txtMRN
            // 
            this.txtMRN.Location = new System.Drawing.Point(243, 316);
            this.txtMRN.Name = "txtMRN";
            this.txtMRN.Size = new System.Drawing.Size(100, 20);
            this.txtMRN.TabIndex = 21;
            // 
            // txtAccountNumber
            // 
            this.txtAccountNumber.Location = new System.Drawing.Point(137, 316);
            this.txtAccountNumber.Name = "txtAccountNumber";
            this.txtAccountNumber.Size = new System.Drawing.Size(100, 20);
            this.txtAccountNumber.TabIndex = 23;
            // 
            // txtSubject
            // 
            this.txtSubject.Location = new System.Drawing.Point(8, 359);
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(740, 20);
            this.txtSubject.TabIndex = 35;
            // 
            // grdMarkers
            // 
            this.grdMarkers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdMarkers.Location = new System.Drawing.Point(7, 136);
            this.grdMarkers.Name = "grdMarkers";
            this.grdMarkers.Size = new System.Drawing.Size(750, 150);
            this.grdMarkers.TabIndex = 40;
            // 
            // cmbHospCode
            // 
            this.cmbHospCode.FormattingEnabled = true;
            this.cmbHospCode.Location = new System.Drawing.Point(10, 316);
            this.cmbHospCode.Name = "cmbHospCode";
            this.cmbHospCode.Size = new System.Drawing.Size(121, 21);
            this.cmbHospCode.TabIndex = 41;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 537);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 13);
            this.label3.TabIndex = 42;
            this.label3.Text = "Status:";
            // 
            // lblStatusMsg
            // 
            this.lblStatusMsg.AutoSize = true;
            this.lblStatusMsg.Location = new System.Drawing.Point(54, 538);
            this.lblStatusMsg.Name = "lblStatusMsg";
            this.lblStatusMsg.Size = new System.Drawing.Size(67, 13);
            this.lblStatusMsg.TabIndex = 43;
            this.lblStatusMsg.Text = "lblStatusMsg";
            // 
            // cmdEditSelectedRow
            // 
            this.cmdEditSelectedRow.Location = new System.Drawing.Point(554, 295);
            this.cmdEditSelectedRow.Name = "cmdEditSelectedRow";
            this.cmdEditSelectedRow.Size = new System.Drawing.Size(120, 23);
            this.cmdEditSelectedRow.TabIndex = 47;
            this.cmdEditSelectedRow.Text = "Edit Selected Row";
            this.cmdEditSelectedRow.UseVisualStyleBackColor = true;
            this.cmdEditSelectedRow.Click += new System.EventHandler(this.cmdEditSelectedRow_Click);
            // 
            // cmdUpdateMarker
            // 
            this.cmdUpdateMarker.Location = new System.Drawing.Point(527, 527);
            this.cmdUpdateMarker.Name = "cmdUpdateMarker";
            this.cmdUpdateMarker.Size = new System.Drawing.Size(109, 23);
            this.cmdUpdateMarker.TabIndex = 48;
            this.cmdUpdateMarker.Text = "Update Marker";
            this.cmdUpdateMarker.UseVisualStyleBackColor = true;
            this.cmdUpdateMarker.Click += new System.EventHandler(this.cmdUpdateMarker_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "appbar.control.play.png");
            this.imageList1.Images.SetKeyName(1, "appbar.control.resume.png");
            this.imageList1.Images.SetKeyName(2, "appbar.control.rewind.png");
            this.imageList1.Images.SetKeyName(3, "appbar.control.rewind.variant.png");
            this.imageList1.Images.SetKeyName(4, "appbar.control.stop.png");
            this.imageList1.Images.SetKeyName(5, "appbar.microphone.png");
            this.imageList1.Images.SetKeyName(6, "appbar.moon.full.png");
            this.imageList1.Images.SetKeyName(7, "appbar.phone.hangup.png");
            this.imageList1.Images.SetKeyName(8, "appbar.phone.png");
            this.imageList1.Images.SetKeyName(9, "appbar.phone.voicemail.png");
            this.imageList1.Images.SetKeyName(10, "appbar.settings.png");
            this.imageList1.Images.SetKeyName(11, "appbar.sound.0.png");
            this.imageList1.Images.SetKeyName(12, "appbar.sound.1.png");
            this.imageList1.Images.SetKeyName(13, "appbar.sound.2.png");
            this.imageList1.Images.SetKeyName(14, "appbar.sound.3.png");
            this.imageList1.Images.SetKeyName(15, "appbar.sound.mute.png");
            this.imageList1.Images.SetKeyName(16, "appbar.warning.circle.png");
            this.imageList1.Images.SetKeyName(17, "appbar.warning.circlred.png");
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // txtTrackDetailID
            // 
            this.txtTrackDetailID.Location = new System.Drawing.Point(835, 844);
            this.txtTrackDetailID.Name = "txtTrackDetailID";
            this.txtTrackDetailID.Size = new System.Drawing.Size(100, 20);
            this.txtTrackDetailID.TabIndex = 56;
            // 
            // txtTrackID
            // 
            this.txtTrackID.Location = new System.Drawing.Point(836, 818);
            this.txtTrackID.Name = "txtTrackID";
            this.txtTrackID.Size = new System.Drawing.Size(100, 20);
            this.txtTrackID.TabIndex = 57;
            // 
            // txtStart
            // 
            this.txtStart.Location = new System.Drawing.Point(971, 818);
            this.txtStart.Name = "txtStart";
            this.txtStart.Size = new System.Drawing.Size(100, 20);
            this.txtStart.TabIndex = 58;
            // 
            // txtStop
            // 
            this.txtStop.Location = new System.Drawing.Point(968, 845);
            this.txtStop.Name = "txtStop";
            this.txtStop.Size = new System.Drawing.Size(100, 20);
            this.txtStop.TabIndex = 59;
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(610, 800);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(100, 20);
            this.txtUser.TabIndex = 60;
            // 
            // txtNote
            // 
            this.txtNote.Enabled = false;
            this.txtNote.Fonts = new string[] {
        "Corbel",
        "Corbel, Verdana, Arial, Helvetica, sans-serif",
        "Georgia, Times New Roman, Times, serif",
        "Consolas, Courier New, Courier, monospace"};
            this.txtNote.IllegalPatterns = new string[] {
        "<script.*?>",
        "<\\w+\\s+.*?(j|java|vb|ecma)script:.*?>",
        "<\\w+(\\s+|\\s+.*?\\s+)on\\w+\\s*=.+?>",
        "</?input.*?>"};
            this.txtNote.Location = new System.Drawing.Point(6, 396);
            this.txtNote.Name = "txtNote";
            this.txtNote.Padding = new System.Windows.Forms.Padding(1);
            this.txtNote.ShowHtmlSource = false;
            this.txtNote.Size = new System.Drawing.Size(744, 125);
            this.txtNote.TabIndex = 62;
            this.txtNote.ToolbarStyle = GvS.Controls.ToolbarStyles.AlwaysInternal;
            // 
            // cmdStop7
            // 
            this.cmdStop7.FlatAppearance.BorderSize = 0;
            this.cmdStop7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdStop7.Image = ((System.Drawing.Image)(resources.GetObject("cmdStop7.Image")));
            this.cmdStop7.Location = new System.Drawing.Point(825, 1068);
            this.cmdStop7.MaximumSize = new System.Drawing.Size(18, 18);
            this.cmdStop7.MinimumSize = new System.Drawing.Size(18, 18);
            this.cmdStop7.Name = "cmdStop7";
            this.cmdStop7.Size = new System.Drawing.Size(18, 18);
            this.cmdStop7.TabIndex = 120;
            this.cmdStop7.UseVisualStyleBackColor = true;
            // 
            // button16
            // 
            this.button16.FlatAppearance.BorderSize = 0;
            this.button16.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button16.Image = ((System.Drawing.Image)(resources.GetObject("button16.Image")));
            this.button16.Location = new System.Drawing.Point(916, 1078);
            this.button16.MaximumSize = new System.Drawing.Size(18, 18);
            this.button16.MinimumSize = new System.Drawing.Size(18, 18);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(18, 18);
            this.button16.TabIndex = 119;
            this.button16.UseVisualStyleBackColor = true;
            // 
            // button17
            // 
            this.button17.FlatAppearance.BorderSize = 0;
            this.button17.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button17.Image = ((System.Drawing.Image)(resources.GetObject("button17.Image")));
            this.button17.Location = new System.Drawing.Point(916, 1038);
            this.button17.MaximumSize = new System.Drawing.Size(18, 18);
            this.button17.MinimumSize = new System.Drawing.Size(18, 18);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(18, 18);
            this.button17.TabIndex = 118;
            this.button17.UseVisualStyleBackColor = true;
            // 
            // button18
            // 
            this.button18.FlatAppearance.BorderSize = 0;
            this.button18.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button18.Image = ((System.Drawing.Image)(resources.GetObject("button18.Image")));
            this.button18.Location = new System.Drawing.Point(916, 1005);
            this.button18.MaximumSize = new System.Drawing.Size(18, 18);
            this.button18.MinimumSize = new System.Drawing.Size(18, 18);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(18, 18);
            this.button18.TabIndex = 117;
            this.button18.UseVisualStyleBackColor = true;
            // 
            // button19
            // 
            this.button19.FlatAppearance.BorderSize = 0;
            this.button19.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button19.Image = ((System.Drawing.Image)(resources.GetObject("button19.Image")));
            this.button19.Location = new System.Drawing.Point(916, 971);
            this.button19.MaximumSize = new System.Drawing.Size(18, 18);
            this.button19.MinimumSize = new System.Drawing.Size(18, 18);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(18, 18);
            this.button19.TabIndex = 116;
            this.button19.UseVisualStyleBackColor = true;
            // 
            // cmdStop10
            // 
            this.cmdStop10.FlatAppearance.BorderSize = 0;
            this.cmdStop10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdStop10.Image = ((System.Drawing.Image)(resources.GetObject("cmdStop10.Image")));
            this.cmdStop10.Location = new System.Drawing.Point(916, 931);
            this.cmdStop10.MaximumSize = new System.Drawing.Size(18, 18);
            this.cmdStop10.MinimumSize = new System.Drawing.Size(18, 18);
            this.cmdStop10.Name = "cmdStop10";
            this.cmdStop10.Size = new System.Drawing.Size(18, 18);
            this.cmdStop10.TabIndex = 115;
            this.cmdStop10.UseVisualStyleBackColor = true;
            // 
            // cmdStop6
            // 
            this.cmdStop6.FlatAppearance.BorderSize = 0;
            this.cmdStop6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdStop6.Image = ((System.Drawing.Image)(resources.GetObject("cmdStop6.Image")));
            this.cmdStop6.Location = new System.Drawing.Point(825, 1039);
            this.cmdStop6.MaximumSize = new System.Drawing.Size(18, 18);
            this.cmdStop6.MinimumSize = new System.Drawing.Size(18, 18);
            this.cmdStop6.Name = "cmdStop6";
            this.cmdStop6.Size = new System.Drawing.Size(18, 18);
            this.cmdStop6.TabIndex = 114;
            this.cmdStop6.UseVisualStyleBackColor = true;
            // 
            // cmdStop5
            // 
            this.cmdStop5.FlatAppearance.BorderSize = 0;
            this.cmdStop5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdStop5.Image = ((System.Drawing.Image)(resources.GetObject("cmdStop5.Image")));
            this.cmdStop5.Location = new System.Drawing.Point(825, 1005);
            this.cmdStop5.MaximumSize = new System.Drawing.Size(18, 18);
            this.cmdStop5.MinimumSize = new System.Drawing.Size(18, 18);
            this.cmdStop5.Name = "cmdStop5";
            this.cmdStop5.Size = new System.Drawing.Size(18, 18);
            this.cmdStop5.TabIndex = 113;
            this.cmdStop5.UseVisualStyleBackColor = true;
            // 
            // cmdStop4
            // 
            this.cmdStop4.FlatAppearance.BorderSize = 0;
            this.cmdStop4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdStop4.Image = ((System.Drawing.Image)(resources.GetObject("cmdStop4.Image")));
            this.cmdStop4.Location = new System.Drawing.Point(825, 965);
            this.cmdStop4.MaximumSize = new System.Drawing.Size(18, 18);
            this.cmdStop4.MinimumSize = new System.Drawing.Size(18, 18);
            this.cmdStop4.Name = "cmdStop4";
            this.cmdStop4.Size = new System.Drawing.Size(18, 18);
            this.cmdStop4.TabIndex = 112;
            this.cmdStop4.UseVisualStyleBackColor = true;
            // 
            // cmdStop3
            // 
            this.cmdStop3.FlatAppearance.BorderSize = 0;
            this.cmdStop3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdStop3.Image = ((System.Drawing.Image)(resources.GetObject("cmdStop3.Image")));
            this.cmdStop3.Location = new System.Drawing.Point(824, 931);
            this.cmdStop3.MaximumSize = new System.Drawing.Size(18, 18);
            this.cmdStop3.MinimumSize = new System.Drawing.Size(18, 18);
            this.cmdStop3.Name = "cmdStop3";
            this.cmdStop3.Size = new System.Drawing.Size(18, 18);
            this.cmdStop3.TabIndex = 111;
            this.cmdStop3.UseVisualStyleBackColor = true;
            // 
            // cmdStop9
            // 
            this.cmdStop9.FlatAppearance.BorderSize = 0;
            this.cmdStop9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdStop9.Image = ((System.Drawing.Image)(resources.GetObject("cmdStop9.Image")));
            this.cmdStop9.Location = new System.Drawing.Point(916, 896);
            this.cmdStop9.MaximumSize = new System.Drawing.Size(18, 18);
            this.cmdStop9.MinimumSize = new System.Drawing.Size(18, 18);
            this.cmdStop9.Name = "cmdStop9";
            this.cmdStop9.Size = new System.Drawing.Size(18, 18);
            this.cmdStop9.TabIndex = 110;
            this.cmdStop9.UseVisualStyleBackColor = true;
            // 
            // cmdStop2
            // 
            this.cmdStop2.FlatAppearance.BorderSize = 0;
            this.cmdStop2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdStop2.Image = ((System.Drawing.Image)(resources.GetObject("cmdStop2.Image")));
            this.cmdStop2.Location = new System.Drawing.Point(824, 897);
            this.cmdStop2.MaximumSize = new System.Drawing.Size(18, 18);
            this.cmdStop2.MinimumSize = new System.Drawing.Size(18, 18);
            this.cmdStop2.Name = "cmdStop2";
            this.cmdStop2.Size = new System.Drawing.Size(18, 18);
            this.cmdStop2.TabIndex = 109;
            this.cmdStop2.UseVisualStyleBackColor = true;
            // 
            // cmdStart7
            // 
            this.cmdStart7.FlatAppearance.BorderSize = 0;
            this.cmdStart7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdStart7.Image = ((System.Drawing.Image)(resources.GetObject("cmdStart7.Image")));
            this.cmdStart7.Location = new System.Drawing.Point(631, 1077);
            this.cmdStart7.MaximumSize = new System.Drawing.Size(18, 18);
            this.cmdStart7.MinimumSize = new System.Drawing.Size(18, 18);
            this.cmdStart7.Name = "cmdStart7";
            this.cmdStart7.Size = new System.Drawing.Size(18, 18);
            this.cmdStart7.TabIndex = 106;
            this.cmdStart7.UseVisualStyleBackColor = true;
            // 
            // button13
            // 
            this.button13.FlatAppearance.BorderSize = 0;
            this.button13.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button13.Image = ((System.Drawing.Image)(resources.GetObject("button13.Image")));
            this.button13.Location = new System.Drawing.Point(722, 1078);
            this.button13.MaximumSize = new System.Drawing.Size(18, 18);
            this.button13.MinimumSize = new System.Drawing.Size(18, 18);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(18, 18);
            this.button13.TabIndex = 105;
            this.button13.UseVisualStyleBackColor = true;
            // 
            // button14
            // 
            this.button14.FlatAppearance.BorderSize = 0;
            this.button14.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button14.Image = ((System.Drawing.Image)(resources.GetObject("button14.Image")));
            this.button14.Location = new System.Drawing.Point(722, 1038);
            this.button14.MaximumSize = new System.Drawing.Size(18, 18);
            this.button14.MinimumSize = new System.Drawing.Size(18, 18);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(18, 18);
            this.button14.TabIndex = 104;
            this.button14.UseVisualStyleBackColor = true;
            // 
            // button9
            // 
            this.button9.FlatAppearance.BorderSize = 0;
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button9.Image = ((System.Drawing.Image)(resources.GetObject("button9.Image")));
            this.button9.Location = new System.Drawing.Point(722, 1005);
            this.button9.MaximumSize = new System.Drawing.Size(18, 18);
            this.button9.MinimumSize = new System.Drawing.Size(18, 18);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(18, 18);
            this.button9.TabIndex = 103;
            this.button9.UseVisualStyleBackColor = true;
            // 
            // button10
            // 
            this.button10.FlatAppearance.BorderSize = 0;
            this.button10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button10.Image = ((System.Drawing.Image)(resources.GetObject("button10.Image")));
            this.button10.Location = new System.Drawing.Point(722, 971);
            this.button10.MaximumSize = new System.Drawing.Size(18, 18);
            this.button10.MinimumSize = new System.Drawing.Size(18, 18);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(18, 18);
            this.button10.TabIndex = 102;
            this.button10.UseVisualStyleBackColor = true;
            // 
            // cmdStart10
            // 
            this.cmdStart10.FlatAppearance.BorderSize = 0;
            this.cmdStart10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdStart10.Image = ((System.Drawing.Image)(resources.GetObject("cmdStart10.Image")));
            this.cmdStart10.Location = new System.Drawing.Point(722, 931);
            this.cmdStart10.MaximumSize = new System.Drawing.Size(18, 18);
            this.cmdStart10.MinimumSize = new System.Drawing.Size(18, 18);
            this.cmdStart10.Name = "cmdStart10";
            this.cmdStart10.Size = new System.Drawing.Size(18, 18);
            this.cmdStart10.TabIndex = 101;
            this.cmdStart10.UseVisualStyleBackColor = true;
            // 
            // cmdStart6
            // 
            this.cmdStart6.FlatAppearance.BorderSize = 0;
            this.cmdStart6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdStart6.Image = ((System.Drawing.Image)(resources.GetObject("cmdStart6.Image")));
            this.cmdStart6.Location = new System.Drawing.Point(631, 1039);
            this.cmdStart6.MaximumSize = new System.Drawing.Size(18, 18);
            this.cmdStart6.MinimumSize = new System.Drawing.Size(18, 18);
            this.cmdStart6.Name = "cmdStart6";
            this.cmdStart6.Size = new System.Drawing.Size(18, 18);
            this.cmdStart6.TabIndex = 100;
            this.cmdStart6.UseVisualStyleBackColor = true;
            // 
            // cmdStart5
            // 
            this.cmdStart5.FlatAppearance.BorderSize = 0;
            this.cmdStart5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdStart5.Image = ((System.Drawing.Image)(resources.GetObject("cmdStart5.Image")));
            this.cmdStart5.Location = new System.Drawing.Point(631, 1005);
            this.cmdStart5.MaximumSize = new System.Drawing.Size(18, 18);
            this.cmdStart5.MinimumSize = new System.Drawing.Size(18, 18);
            this.cmdStart5.Name = "cmdStart5";
            this.cmdStart5.Size = new System.Drawing.Size(18, 18);
            this.cmdStart5.TabIndex = 99;
            this.cmdStart5.UseVisualStyleBackColor = true;
            // 
            // cmdStart4
            // 
            this.cmdStart4.FlatAppearance.BorderSize = 0;
            this.cmdStart4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdStart4.Image = ((System.Drawing.Image)(resources.GetObject("cmdStart4.Image")));
            this.cmdStart4.Location = new System.Drawing.Point(631, 965);
            this.cmdStart4.MaximumSize = new System.Drawing.Size(18, 18);
            this.cmdStart4.MinimumSize = new System.Drawing.Size(18, 18);
            this.cmdStart4.Name = "cmdStart4";
            this.cmdStart4.Size = new System.Drawing.Size(18, 18);
            this.cmdStart4.TabIndex = 98;
            this.cmdStart4.UseVisualStyleBackColor = true;
            // 
            // cmdStart3
            // 
            this.cmdStart3.FlatAppearance.BorderSize = 0;
            this.cmdStart3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdStart3.Image = ((System.Drawing.Image)(resources.GetObject("cmdStart3.Image")));
            this.cmdStart3.Location = new System.Drawing.Point(630, 931);
            this.cmdStart3.MaximumSize = new System.Drawing.Size(18, 18);
            this.cmdStart3.MinimumSize = new System.Drawing.Size(18, 18);
            this.cmdStart3.Name = "cmdStart3";
            this.cmdStart3.Size = new System.Drawing.Size(18, 18);
            this.cmdStart3.TabIndex = 97;
            this.cmdStart3.UseVisualStyleBackColor = true;
            // 
            // cmdStart9
            // 
            this.cmdStart9.FlatAppearance.BorderSize = 0;
            this.cmdStart9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdStart9.Image = ((System.Drawing.Image)(resources.GetObject("cmdStart9.Image")));
            this.cmdStart9.Location = new System.Drawing.Point(722, 896);
            this.cmdStart9.MaximumSize = new System.Drawing.Size(18, 18);
            this.cmdStart9.MinimumSize = new System.Drawing.Size(18, 18);
            this.cmdStart9.Name = "cmdStart9";
            this.cmdStart9.Size = new System.Drawing.Size(18, 18);
            this.cmdStart9.TabIndex = 96;
            this.cmdStart9.UseVisualStyleBackColor = true;
            // 
            // cmdStart2
            // 
            this.cmdStart2.FlatAppearance.BorderSize = 0;
            this.cmdStart2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdStart2.Image = ((System.Drawing.Image)(resources.GetObject("cmdStart2.Image")));
            this.cmdStart2.Location = new System.Drawing.Point(630, 897);
            this.cmdStart2.MaximumSize = new System.Drawing.Size(18, 18);
            this.cmdStart2.MinimumSize = new System.Drawing.Size(18, 18);
            this.cmdStart2.Name = "cmdStart2";
            this.cmdStart2.Size = new System.Drawing.Size(18, 18);
            this.cmdStart2.TabIndex = 95;
            this.cmdStart2.UseVisualStyleBackColor = true;
            // 
            // timer2
            // 
            this.timer2.Interval = 10;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // dtDOB
            // 
            this.dtDOB.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtDOB.Location = new System.Drawing.Point(351, 317);
            this.dtDOB.Name = "dtDOB";
            this.dtDOB.Size = new System.Drawing.Size(104, 20);
            this.dtDOB.TabIndex = 125;
            // 
            // cmdFinish
            // 
            this.cmdFinish.Location = new System.Drawing.Point(679, 46);
            this.cmdFinish.Name = "cmdFinish";
            this.cmdFinish.Size = new System.Drawing.Size(75, 23);
            this.cmdFinish.TabIndex = 148;
            this.cmdFinish.Text = "Finish";
            this.cmdFinish.UseVisualStyleBackColor = true;
            this.cmdFinish.Click += new System.EventHandler(this.cmdFinish_Click);
            // 
            // cmdStop
            // 
            this.cmdStop.Location = new System.Drawing.Point(598, 46);
            this.cmdStop.Name = "cmdStop";
            this.cmdStop.Size = new System.Drawing.Size(75, 23);
            this.cmdStop.TabIndex = 147;
            this.cmdStop.Text = "Stop";
            this.cmdStop.UseVisualStyleBackColor = true;
            this.cmdStop.Click += new System.EventHandler(this.cmdStop_Click);
            // 
            // lblTrackTotalTime
            // 
            this.lblTrackTotalTime.AutoSize = true;
            this.lblTrackTotalTime.BackColor = System.Drawing.Color.Transparent;
            this.lblTrackTotalTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTrackTotalTime.ForeColor = System.Drawing.Color.Black;
            this.lblTrackTotalTime.Location = new System.Drawing.Point(582, 80);
            this.lblTrackTotalTime.Name = "lblTrackTotalTime";
            this.lblTrackTotalTime.Size = new System.Drawing.Size(28, 13);
            this.lblTrackTotalTime.TabIndex = 144;
            this.lblTrackTotalTime.Text = "0:00";
            // 
            // lblTrackTime
            // 
            this.lblTrackTime.AutoSize = true;
            this.lblTrackTime.BackColor = System.Drawing.Color.Transparent;
            this.lblTrackTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTrackTime.ForeColor = System.Drawing.Color.Black;
            this.lblTrackTime.Location = new System.Drawing.Point(473, 76);
            this.lblTrackTime.Name = "lblTrackTime";
            this.lblTrackTime.Size = new System.Drawing.Size(64, 13);
            this.lblTrackTime.TabIndex = 143;
            this.lblTrackTime.Text = "Track Time:";
            // 
            // lblRecordingStatusMsg
            // 
            this.lblRecordingStatusMsg.AutoSize = true;
            this.lblRecordingStatusMsg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lblRecordingStatusMsg.Location = new System.Drawing.Point(103, 91);
            this.lblRecordingStatusMsg.Name = "lblRecordingStatusMsg";
            this.lblRecordingStatusMsg.Size = new System.Drawing.Size(92, 13);
            this.lblRecordingStatusMsg.TabIndex = 142;
            this.lblRecordingStatusMsg.Text = "Ready To Record";
            // 
            // lblRecordingStatus
            // 
            this.lblRecordingStatus.AutoSize = true;
            this.lblRecordingStatus.Location = new System.Drawing.Point(9, 91);
            this.lblRecordingStatus.Name = "lblRecordingStatus";
            this.lblRecordingStatus.Size = new System.Drawing.Size(95, 13);
            this.lblRecordingStatus.TabIndex = 141;
            this.lblRecordingStatus.Text = "Recording Status: ";
            // 
            // cmdStart
            // 
            this.cmdStart.Location = new System.Drawing.Point(517, 46);
            this.cmdStart.Name = "cmdStart";
            this.cmdStart.Size = new System.Drawing.Size(75, 23);
            this.cmdStart.TabIndex = 140;
            this.cmdStart.Text = "Start";
            this.cmdStart.UseVisualStyleBackColor = true;
            this.cmdStart.Click += new System.EventHandler(this.cmdStart_Click);
            // 
            // pgbTrackTime
            // 
            this.pgbTrackTime.Location = new System.Drawing.Point(9, 110);
            this.pgbTrackTime.Maximum = 30;
            this.pgbTrackTime.Name = "pgbTrackTime";
            this.pgbTrackTime.Size = new System.Drawing.Size(749, 23);
            this.pgbTrackTime.TabIndex = 138;
            // 
            // comboWasapiDevices
            // 
            this.comboWasapiDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboWasapiDevices.FormattingEnabled = true;
            this.comboWasapiDevices.Location = new System.Drawing.Point(490, 22);
            this.comboWasapiDevices.Name = "comboWasapiDevices";
            this.comboWasapiDevices.Size = new System.Drawing.Size(264, 21);
            this.comboWasapiDevices.TabIndex = 149;
            // 
            // vuMeeter
            // 
            this.vuMeeter.ForeColor = System.Drawing.Color.Green;
            this.vuMeeter.Location = new System.Drawing.Point(7, 22);
            this.vuMeeter.Name = "vuMeeter";
            this.vuMeeter.Size = new System.Drawing.Size(436, 21);
            this.vuMeeter.Step = 1;
            this.vuMeeter.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.vuMeeter.TabIndex = 146;
            // 
            // tbVol
            // 
            this.tbVol.LargeChange = 1;
            this.tbVol.Location = new System.Drawing.Point(1, 44);
            this.tbVol.Maximum = 20;
            this.tbVol.Name = "tbVol";
            this.tbVol.Size = new System.Drawing.Size(451, 42);
            this.tbVol.TabIndex = 145;
            this.tbVol.Scroll += new System.EventHandler(this.tbVol_Scroll);
            // 
            // lblATime
            // 
            this.lblATime.AutoSize = true;
            this.lblATime.BackColor = System.Drawing.Color.Transparent;
            this.lblATime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblATime.ForeColor = System.Drawing.Color.Black;
            this.lblATime.Location = new System.Drawing.Point(582, 94);
            this.lblATime.Name = "lblATime";
            this.lblATime.Size = new System.Drawing.Size(28, 13);
            this.lblATime.TabIndex = 151;
            this.lblATime.Text = "0:00";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(473, 91);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 13);
            this.label6.TabIndex = 150;
            this.label6.Text = "Avaibale Time:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 13);
            this.label4.TabIndex = 152;
            this.label4.Text = "Recording Level:";
            // 
            // cmdPBPlay
            // 
            this.cmdPBPlay.Enabled = false;
            this.cmdPBPlay.Image = ((System.Drawing.Image)(resources.GetObject("cmdPBPlay.Image")));
            this.cmdPBPlay.Location = new System.Drawing.Point(673, 330);
            this.cmdPBPlay.Name = "cmdPBPlay";
            this.cmdPBPlay.Size = new System.Drawing.Size(75, 23);
            this.cmdPBPlay.TabIndex = 155;
            this.cmdPBPlay.UseVisualStyleBackColor = true;
            this.cmdPBPlay.Click += new System.EventHandler(this.cmdPBPlay_Click);
            // 
            // timPlayBack
            // 
            this.timPlayBack.Tick += new System.EventHandler(this.timPlayBack_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 560);
            this.Controls.Add(this.cmdPBPlay);
            this.Controls.Add(this.txtSubject);
            this.Controls.Add(this.cmdSaveMarker);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmdCanelMarker);
            this.Controls.Add(this.lblATime);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.lblTrackTotalTime);
            this.Controls.Add(this.cmdUpdateMarker);
            this.Controls.Add(this.lblTrackTime);
            this.Controls.Add(this.txtNote);
            this.Controls.Add(this.comboWasapiDevices);
            this.Controls.Add(this.lblHospCode);
            this.Controls.Add(this.cmdFinish);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmdStop);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.vuMeeter);
            this.Controls.Add(this.txtMRN);
            this.Controls.Add(this.tbVol);
            this.Controls.Add(this.txtAccountNumber);
            this.Controls.Add(this.lblRecordingStatusMsg);
            this.Controls.Add(this.cmbHospCode);
            this.Controls.Add(this.lblRecordingStatus);
            this.Controls.Add(this.dtDOB);
            this.Controls.Add(this.cmdStart);
            this.Controls.Add(this.pgbTrackTime);
            this.Controls.Add(this.cmdStop7);
            this.Controls.Add(this.button16);
            this.Controls.Add(this.button17);
            this.Controls.Add(this.button18);
            this.Controls.Add(this.button19);
            this.Controls.Add(this.cmdStop10);
            this.Controls.Add(this.cmdStop6);
            this.Controls.Add(this.cmdStop5);
            this.Controls.Add(this.cmdStop4);
            this.Controls.Add(this.cmdStop3);
            this.Controls.Add(this.cmdStop9);
            this.Controls.Add(this.cmdStop2);
            this.Controls.Add(this.cmdStart7);
            this.Controls.Add(this.button13);
            this.Controls.Add(this.button14);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.cmdStart10);
            this.Controls.Add(this.cmdStart6);
            this.Controls.Add(this.cmdStart5);
            this.Controls.Add(this.cmdStart4);
            this.Controls.Add(this.cmdStart3);
            this.Controls.Add(this.cmdStart9);
            this.Controls.Add(this.cmdStart2);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.txtStop);
            this.Controls.Add(this.txtStart);
            this.Controls.Add(this.txtTrackID);
            this.Controls.Add(this.txtTrackDetailID);
            this.Controls.Add(this.cmdEditSelectedRow);
            this.Controls.Add(this.lblStatusMsg);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.grdMarkers);
            this.Controls.Add(this.cmdAddMarker);
            this.Name = "frmMain";
            this.Text = "DCSGlobal Medical Voice Recorder";
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdMarkers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbVol)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdAddMarker;
        private System.Windows.Forms.Button cmdSaveMarker;
        private System.Windows.Forms.Button cmdCanelMarker;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblHospCode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtMRN;
        private System.Windows.Forms.TextBox txtAccountNumber;
        private System.Windows.Forms.TextBox txtSubject;
        private System.Windows.Forms.DataGridView grdMarkers;
        private System.Windows.Forms.ComboBox cmbHospCode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblStatusMsg;
        private System.Windows.Forms.Button cmdEditSelectedRow;
        private System.Windows.Forms.Button cmdUpdateMarker;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox txtTrackDetailID;
        private System.Windows.Forms.TextBox txtTrackID;
        private System.Windows.Forms.TextBox txtStart;
        private System.Windows.Forms.TextBox txtStop;
        private System.Windows.Forms.TextBox txtUser;
        private GvS.Controls.HtmlTextbox txtNote;
        private System.Windows.Forms.Button cmdStop7;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.Button button18;
        private System.Windows.Forms.Button button19;
        private System.Windows.Forms.Button cmdStop10;
        private System.Windows.Forms.Button cmdStop6;
        private System.Windows.Forms.Button cmdStop5;
        private System.Windows.Forms.Button cmdStop4;
        private System.Windows.Forms.Button cmdStop3;
        private System.Windows.Forms.Button cmdStop9;
        private System.Windows.Forms.Button cmdStop2;
        private System.Windows.Forms.Button cmdStart7;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button cmdStart10;
        private System.Windows.Forms.Button cmdStart6;
        private System.Windows.Forms.Button cmdStart5;
        private System.Windows.Forms.Button cmdStart4;
        private System.Windows.Forms.Button cmdStart3;
        private System.Windows.Forms.Button cmdStart9;
        private System.Windows.Forms.Button cmdStart2;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.DateTimePicker dtDOB;
        private System.Windows.Forms.Button cmdFinish;
        private System.Windows.Forms.Button cmdStop;
        private System.Windows.Forms.Label lblTrackTotalTime;
        private System.Windows.Forms.Label lblTrackTime;
        private System.Windows.Forms.Label lblRecordingStatusMsg;
        private System.Windows.Forms.Label lblRecordingStatus;
        private System.Windows.Forms.Button cmdStart;
        private System.Windows.Forms.ProgressBar pgbTrackTime;
        private System.Windows.Forms.ComboBox comboWasapiDevices;
        private System.Windows.Forms.ProgressBar vuMeeter;
        private System.Windows.Forms.TrackBar tbVol;
        private System.Windows.Forms.Label lblATime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button cmdPBPlay;
        private System.Windows.Forms.Timer timPlayBack;
    }
}

