namespace Manual_test_app.db
{
    partial class frmConnectionStrings
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
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdApply = new System.Windows.Forms.Button();
            this.cmdSave = new System.Windows.Forms.Button();
            this.cmdNew = new System.Windows.Forms.Button();
            this.cmbConStrings = new System.Windows.Forms.ComboBox();
            this.txtEncrypted = new System.Windows.Forms.TextBox();
            this.txtIC = new System.Windows.Forms.TextBox();
            this.txtPasswd = new System.Windows.Forms.TextBox();
            this.txtUserID = new System.Windows.Forms.TextBox();
            this.txtConName = new System.Windows.Forms.TextBox();
            this.txtDataSource = new System.Windows.Forms.TextBox();
            this.txtClearText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lnkClearText = new System.Windows.Forms.LinkLabel();
            this.lnkEncrypted = new System.Windows.Forms.LinkLabel();
            this.lnkParse = new System.Windows.Forms.LinkLabel();
            this.lnkDecryptAndParse = new System.Windows.Forms.LinkLabel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.chkPersistSecurityInfo = new System.Windows.Forms.CheckBox();
            this.cmbEM = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Location = new System.Drawing.Point(408, 414);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 0;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdApply
            // 
            this.cmdApply.Location = new System.Drawing.Point(489, 414);
            this.cmdApply.Name = "cmdApply";
            this.cmdApply.Size = new System.Drawing.Size(75, 23);
            this.cmdApply.TabIndex = 1;
            this.cmdApply.Text = "Apply";
            this.cmdApply.UseVisualStyleBackColor = true;
            this.cmdApply.Click += new System.EventHandler(this.cmdApply_Click);
            // 
            // cmdSave
            // 
            this.cmdSave.Location = new System.Drawing.Point(570, 414);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(75, 23);
            this.cmdSave.TabIndex = 2;
            this.cmdSave.Text = "Save";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // cmdNew
            // 
            this.cmdNew.Location = new System.Drawing.Point(578, 40);
            this.cmdNew.Name = "cmdNew";
            this.cmdNew.Size = new System.Drawing.Size(75, 23);
            this.cmdNew.TabIndex = 3;
            this.cmdNew.Text = "New";
            this.cmdNew.UseVisualStyleBackColor = true;
            this.cmdNew.Click += new System.EventHandler(this.cmdNew_Click);
            // 
            // cmbConStrings
            // 
            this.cmbConStrings.FormattingEnabled = true;
            this.cmbConStrings.Location = new System.Drawing.Point(13, 13);
            this.cmbConStrings.Name = "cmbConStrings";
            this.cmbConStrings.Size = new System.Drawing.Size(640, 21);
            this.cmbConStrings.TabIndex = 4;
            this.cmbConStrings.SelectedIndexChanged += new System.EventHandler(this.cmbConStrings_SelectedIndexChanged);
            // 
            // txtEncrypted
            // 
            this.txtEncrypted.Location = new System.Drawing.Point(5, 369);
            this.txtEncrypted.Multiline = true;
            this.txtEncrypted.Name = "txtEncrypted";
            this.txtEncrypted.Size = new System.Drawing.Size(640, 39);
            this.txtEncrypted.TabIndex = 5;
            // 
            // txtIC
            // 
            this.txtIC.Location = new System.Drawing.Point(87, 200);
            this.txtIC.Name = "txtIC";
            this.txtIC.Size = new System.Drawing.Size(191, 20);
            this.txtIC.TabIndex = 8;
            // 
            // txtPasswd
            // 
            this.txtPasswd.Location = new System.Drawing.Point(87, 249);
            this.txtPasswd.Name = "txtPasswd";
            this.txtPasswd.Size = new System.Drawing.Size(191, 20);
            this.txtPasswd.TabIndex = 9;
            // 
            // txtUserID
            // 
            this.txtUserID.Location = new System.Drawing.Point(87, 226);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(191, 20);
            this.txtUserID.TabIndex = 11;
            // 
            // txtConName
            // 
            this.txtConName.Location = new System.Drawing.Point(84, 40);
            this.txtConName.Name = "txtConName";
            this.txtConName.Size = new System.Drawing.Size(473, 20);
            this.txtConName.TabIndex = 12;
            // 
            // txtDataSource
            // 
            this.txtDataSource.Location = new System.Drawing.Point(87, 173);
            this.txtDataSource.Name = "txtDataSource";
            this.txtDataSource.Size = new System.Drawing.Size(191, 20);
            this.txtDataSource.TabIndex = 13;
            // 
            // txtClearText
            // 
            this.txtClearText.Location = new System.Drawing.Point(5, 307);
            this.txtClearText.Multiline = true;
            this.txtClearText.Name = "txtClearText";
            this.txtClearText.Size = new System.Drawing.Size(640, 39);
            this.txtClearText.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 288);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Clear Text";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 353);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Encrypted";
            // 
            // lnkClearText
            // 
            this.lnkClearText.AutoSize = true;
            this.lnkClearText.Location = new System.Drawing.Point(514, 291);
            this.lnkClearText.Name = "lnkClearText";
            this.lnkClearText.Size = new System.Drawing.Size(131, 13);
            this.lnkClearText.TabIndex = 17;
            this.lnkClearText.TabStop = true;
            this.lnkClearText.Text = "copy cleartext to clipboard";
            this.lnkClearText.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkClearText_LinkClicked);
            // 
            // lnkEncrypted
            // 
            this.lnkEncrypted.AutoSize = true;
            this.lnkEncrypted.Location = new System.Drawing.Point(501, 353);
            this.lnkEncrypted.Name = "lnkEncrypted";
            this.lnkEncrypted.Size = new System.Drawing.Size(144, 13);
            this.lnkEncrypted.TabIndex = 18;
            this.lnkEncrypted.TabStop = true;
            this.lnkEncrypted.Text = "copy encrypted to clippboard";
            this.lnkEncrypted.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkEncrypted_LinkClicked);
            // 
            // lnkParse
            // 
            this.lnkParse.AutoSize = true;
            this.lnkParse.Location = new System.Drawing.Point(475, 291);
            this.lnkParse.Name = "lnkParse";
            this.lnkParse.Size = new System.Drawing.Size(33, 13);
            this.lnkParse.TabIndex = 19;
            this.lnkParse.TabStop = true;
            this.lnkParse.Text = "parse";
            this.lnkParse.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkParse_LinkClicked);
            // 
            // lnkDecryptAndParse
            // 
            this.lnkDecryptAndParse.AutoSize = true;
            this.lnkDecryptAndParse.Location = new System.Drawing.Point(398, 353);
            this.lnkDecryptAndParse.Name = "lnkDecryptAndParse";
            this.lnkDecryptAndParse.Size = new System.Drawing.Size(92, 13);
            this.lnkDecryptAndParse.TabIndex = 20;
            this.lnkDecryptAndParse.TabStop = true;
            this.lnkDecryptAndParse.Text = "decrypt and parse";
            this.lnkDecryptAndParse.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkDecryptAndParse_LinkClicked);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 176);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Data Source";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 203);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Initial Catalog";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 229);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 24;
            this.label6.Text = "User ID";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 252);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 25;
            this.label7.Text = "Password";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkPersistSecurityInfo
            // 
            this.chkPersistSecurityInfo.AutoSize = true;
            this.chkPersistSecurityInfo.Checked = true;
            this.chkPersistSecurityInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPersistSecurityInfo.Location = new System.Drawing.Point(408, 147);
            this.chkPersistSecurityInfo.Name = "chkPersistSecurityInfo";
            this.chkPersistSecurityInfo.Size = new System.Drawing.Size(119, 17);
            this.chkPersistSecurityInfo.TabIndex = 26;
            this.chkPersistSecurityInfo.Text = "Persist Security Info";
            this.chkPersistSecurityInfo.UseVisualStyleBackColor = true;
            // 
            // cmbEM
            // 
            this.cmbEM.FormattingEnabled = true;
            this.cmbEM.Items.AddRange(new object[] {
            "None",
            "Subba",
            "Full"});
            this.cmbEM.Location = new System.Drawing.Point(408, 187);
            this.cmbEM.Name = "cmbEM";
            this.cmbEM.Size = new System.Drawing.Size(119, 21);
            this.cmbEM.TabIndex = 27;
            this.cmbEM.SelectedIndexChanged += new System.EventHandler(this.cmbEM_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(405, 169);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(96, 13);
            this.label8.TabIndex = 28;
            this.label8.Text = "Encryption Method";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 150);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(17, 13);
            this.label9.TabIndex = 30;
            this.label9.Text = "IP";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(87, 147);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(191, 20);
            this.txtIP.TabIndex = 29;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 66);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 13);
            this.label10.TabIndex = 32;
            this.label10.Text = "Description";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(84, 66);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(473, 53);
            this.txtDescription.TabIndex = 31;
            // 
            // frmConnectionStrings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 449);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cmbEM);
            this.Controls.Add(this.chkPersistSecurityInfo);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lnkDecryptAndParse);
            this.Controls.Add(this.lnkParse);
            this.Controls.Add(this.lnkEncrypted);
            this.Controls.Add(this.lnkClearText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtClearText);
            this.Controls.Add(this.txtDataSource);
            this.Controls.Add(this.txtConName);
            this.Controls.Add(this.txtUserID);
            this.Controls.Add(this.txtPasswd);
            this.Controls.Add(this.txtIC);
            this.Controls.Add(this.txtEncrypted);
            this.Controls.Add(this.cmbConStrings);
            this.Controls.Add(this.cmdNew);
            this.Controls.Add(this.cmdSave);
            this.Controls.Add(this.cmdApply);
            this.Controls.Add(this.cmdCancel);
            this.Name = "frmConnectionStrings";
            this.Text = "Connection String Manager";
            this.Load += new System.EventHandler(this.frmConnectionStrings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdApply;
        private System.Windows.Forms.Button cmdSave;
        private System.Windows.Forms.Button cmdNew;
        private System.Windows.Forms.ComboBox cmbConStrings;
        private System.Windows.Forms.TextBox txtEncrypted;
        private System.Windows.Forms.TextBox txtIC;
        private System.Windows.Forms.TextBox txtPasswd;
        private System.Windows.Forms.TextBox txtUserID;
        private System.Windows.Forms.TextBox txtConName;
        private System.Windows.Forms.TextBox txtDataSource;
        private System.Windows.Forms.TextBox txtClearText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel lnkClearText;
        private System.Windows.Forms.LinkLabel lnkEncrypted;
        private System.Windows.Forms.LinkLabel lnkParse;
        private System.Windows.Forms.LinkLabel lnkDecryptAndParse;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkPersistSecurityInfo;
        private System.Windows.Forms.ComboBox cmbEM;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtDescription;
    }
}