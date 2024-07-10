namespace Manual_test_app
{
    partial class RAServerTest
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
            this.button1 = new System.Windows.Forms.Button();
            this.cmdKillList = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.txtoperator_id = new System.Windows.Forms.TextBox();
            this.txtConString = new System.Windows.Forms.TextBox();
            this.txtInstanceName = new System.Windows.Forms.TextBox();
            this.cmdGo = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblSR = new System.Windows.Forms.Label();
            this.txtRawdata = new System.Windows.Forms.TextBox();
            this.cmdRun = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbTheBox = new System.Windows.Forms.ComboBox();
            this.lstTheBox = new System.Windows.Forms.ListBox();
            this.tmGo = new System.Windows.Forms.Timer(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.cmdWCFLogin = new System.Windows.Forms.Button();
            this.cmdWCFKillList = new System.Windows.Forms.Button();
            this.cmdWCFGet = new System.Windows.Forms.Button();
            this.wcf = new System.Windows.Forms.Panel();
            this.txtHospCode = new System.Windows.Forms.TextBox();
            this.statusStrip1.SuspendLayout();
            this.wcf.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(738, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cmdKillList
            // 
            this.cmdKillList.Location = new System.Drawing.Point(576, 9);
            this.cmdKillList.Name = "cmdKillList";
            this.cmdKillList.Size = new System.Drawing.Size(75, 23);
            this.cmdKillList.TabIndex = 1;
            this.cmdKillList.Text = "KillList";
            this.cmdKillList.UseVisualStyleBackColor = true;
            this.cmdKillList.Click += new System.EventHandler(this.cmdKillList_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(657, 9);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // txtoperator_id
            // 
            this.txtoperator_id.Location = new System.Drawing.Point(109, 12);
            this.txtoperator_id.Name = "txtoperator_id";
            this.txtoperator_id.Size = new System.Drawing.Size(276, 20);
            this.txtoperator_id.TabIndex = 3;
            this.txtoperator_id.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // txtConString
            // 
            this.txtConString.Location = new System.Drawing.Point(109, 90);
            this.txtConString.Name = "txtConString";
            this.txtConString.Size = new System.Drawing.Size(403, 20);
            this.txtConString.TabIndex = 4;
            this.txtConString.TextChanged += new System.EventHandler(this.txtConString_TextChanged);
            // 
            // txtInstanceName
            // 
            this.txtInstanceName.Location = new System.Drawing.Point(109, 64);
            this.txtInstanceName.Name = "txtInstanceName";
            this.txtInstanceName.Size = new System.Drawing.Size(276, 20);
            this.txtInstanceName.TabIndex = 5;
            // 
            // cmdGo
            // 
            this.cmdGo.Location = new System.Drawing.Point(437, 9);
            this.cmdGo.Name = "cmdGo";
            this.cmdGo.Size = new System.Drawing.Size(75, 23);
            this.cmdGo.TabIndex = 6;
            this.cmdGo.Text = "Go!";
            this.cmdGo.UseVisualStyleBackColor = true;
            this.cmdGo.Click += new System.EventHandler(this.cmdGo_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Opertor ID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "server insttnace";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "constring";
            // 
            // lblSR
            // 
            this.lblSR.AutoSize = true;
            this.lblSR.Location = new System.Drawing.Point(16, 345);
            this.lblSR.Name = "lblSR";
            this.lblSR.Size = new System.Drawing.Size(89, 13);
            this.lblSR.TabIndex = 10;
            this.lblSR.Text = "Server Response";
            // 
            // txtRawdata
            // 
            this.txtRawdata.Location = new System.Drawing.Point(16, 361);
            this.txtRawdata.Multiline = true;
            this.txtRawdata.Name = "txtRawdata";
            this.txtRawdata.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRawdata.Size = new System.Drawing.Size(788, 145);
            this.txtRawdata.TabIndex = 11;
            this.txtRawdata.WordWrap = false;
            // 
            // cmdRun
            // 
            this.cmdRun.Location = new System.Drawing.Point(437, 34);
            this.cmdRun.Name = "cmdRun";
            this.cmdRun.Size = new System.Drawing.Size(75, 23);
            this.cmdRun.TabIndex = 12;
            this.cmdRun.Text = "Run";
            this.cmdRun.UseVisualStyleBackColor = true;
            this.cmdRun.Click += new System.EventHandler(this.cmdRun_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3});
            this.statusStrip1.Location = new System.Drawing.Point(0, 533);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(827, 22);
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(109, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Click += new System.EventHandler(this.toolStripStatusLabel1_Click);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(76, 17);
            this.toolStripStatusLabel2.Text = "Timer Disabled";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(109, 17);
            this.toolStripStatusLabel3.Text = "toolStripStatusLabel3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 163);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "TheBox";
            // 
            // cmbTheBox
            // 
            this.cmbTheBox.FormattingEnabled = true;
            this.cmbTheBox.Location = new System.Drawing.Point(19, 180);
            this.cmbTheBox.Name = "cmbTheBox";
            this.cmbTheBox.Size = new System.Drawing.Size(785, 21);
            this.cmbTheBox.TabIndex = 15;
            // 
            // lstTheBox
            // 
            this.lstTheBox.FormattingEnabled = true;
            this.lstTheBox.Location = new System.Drawing.Point(19, 208);
            this.lstTheBox.Name = "lstTheBox";
            this.lstTheBox.Size = new System.Drawing.Size(785, 121);
            this.lstTheBox.TabIndex = 16;
            // 
            // tmGo
            // 
            this.tmGo.Enabled = true;
            this.tmGo.Interval = 30000;
            this.tmGo.Tick += new System.EventHandler(this.tmGo_Tick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 44);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "server ip";
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(109, 38);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(276, 20);
            this.txtIP.TabIndex = 17;
            // 
            // cmdWCFLogin
            // 
            this.cmdWCFLogin.Location = new System.Drawing.Point(84, 14);
            this.cmdWCFLogin.Name = "cmdWCFLogin";
            this.cmdWCFLogin.Size = new System.Drawing.Size(75, 23);
            this.cmdWCFLogin.TabIndex = 21;
            this.cmdWCFLogin.Text = "Login";
            this.cmdWCFLogin.UseVisualStyleBackColor = true;
            this.cmdWCFLogin.Click += new System.EventHandler(this.cmdWCFLogin_Click);
            // 
            // cmdWCFKillList
            // 
            this.cmdWCFKillList.Location = new System.Drawing.Point(3, 14);
            this.cmdWCFKillList.Name = "cmdWCFKillList";
            this.cmdWCFKillList.Size = new System.Drawing.Size(75, 23);
            this.cmdWCFKillList.TabIndex = 20;
            this.cmdWCFKillList.Text = "Kill List";
            this.cmdWCFKillList.UseVisualStyleBackColor = true;
            this.cmdWCFKillList.Click += new System.EventHandler(this.cmdWCFKillList_Click);
            // 
            // cmdWCFGet
            // 
            this.cmdWCFGet.Location = new System.Drawing.Point(165, 14);
            this.cmdWCFGet.Name = "cmdWCFGet";
            this.cmdWCFGet.Size = new System.Drawing.Size(75, 23);
            this.cmdWCFGet.TabIndex = 19;
            this.cmdWCFGet.Text = "GET";
            this.cmdWCFGet.UseVisualStyleBackColor = true;
            this.cmdWCFGet.Click += new System.EventHandler(this.cmdWCFGet_Click);
            // 
            // wcf
            // 
            this.wcf.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.wcf.Controls.Add(this.cmdWCFKillList);
            this.wcf.Controls.Add(this.cmdWCFLogin);
            this.wcf.Controls.Add(this.cmdWCFGet);
            this.wcf.Location = new System.Drawing.Point(564, 70);
            this.wcf.Name = "wcf";
            this.wcf.Size = new System.Drawing.Size(251, 54);
            this.wcf.TabIndex = 22;
            this.wcf.Tag = "weweqw";
            // 
            // txtHospCode
            // 
            this.txtHospCode.Location = new System.Drawing.Point(109, 116);
            this.txtHospCode.Name = "txtHospCode";
            this.txtHospCode.Size = new System.Drawing.Size(276, 20);
            this.txtHospCode.TabIndex = 23;
            // 
            // RAServerTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(827, 555);
            this.Controls.Add(this.txtHospCode);
            this.Controls.Add(this.wcf);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.lstTheBox);
            this.Controls.Add(this.cmbTheBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.cmdRun);
            this.Controls.Add(this.txtRawdata);
            this.Controls.Add(this.lblSR);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdGo);
            this.Controls.Add(this.txtInstanceName);
            this.Controls.Add(this.txtConString);
            this.Controls.Add(this.txtoperator_id);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.cmdKillList);
            this.Controls.Add(this.button1);
            this.Name = "RAServerTest";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.wcf.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button cmdKillList;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox txtoperator_id;
        private System.Windows.Forms.TextBox txtConString;
        private System.Windows.Forms.TextBox txtInstanceName;
        private System.Windows.Forms.Button cmdGo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblSR;
        private System.Windows.Forms.TextBox txtRawdata;
        private System.Windows.Forms.Button cmdRun;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbTheBox;
        private System.Windows.Forms.ListBox lstTheBox;
        private System.Windows.Forms.Timer tmGo;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Button cmdWCFLogin;
        private System.Windows.Forms.Button cmdWCFKillList;
        private System.Windows.Forms.Button cmdWCFGet;
        private System.Windows.Forms.Panel wcf;
        private System.Windows.Forms.TextBox txtHospCode;
    }
}

