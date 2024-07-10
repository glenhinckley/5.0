namespace DCSGlobal.Medical.VoiceRecoder
{
    partial class frmListener
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
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmdExit = new System.Windows.Forms.Button();
            this.cmdPhone = new System.Windows.Forms.Button();
            this.cmdMicrophone = new System.Windows.Forms.Button();
            this.cmdOptions = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblUserName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // cmdExit
            // 
            this.cmdExit.Location = new System.Drawing.Point(435, 13);
            this.cmdExit.Name = "cmdExit";
            this.cmdExit.Size = new System.Drawing.Size(75, 23);
            this.cmdExit.TabIndex = 0;
            this.cmdExit.Text = "Exit";
            this.cmdExit.UseVisualStyleBackColor = true;
            this.cmdExit.Click += new System.EventHandler(this.cmdExit_Click);
            // 
            // cmdPhone
            // 
            this.cmdPhone.Location = new System.Drawing.Point(26, 13);
            this.cmdPhone.Name = "cmdPhone";
            this.cmdPhone.Size = new System.Drawing.Size(75, 23);
            this.cmdPhone.TabIndex = 1;
            this.cmdPhone.Text = "Phone";
            this.cmdPhone.UseVisualStyleBackColor = true;
            this.cmdPhone.Click += new System.EventHandler(this.cmdPhone_Click);
            // 
            // cmdMicrophone
            // 
            this.cmdMicrophone.Location = new System.Drawing.Point(108, 13);
            this.cmdMicrophone.Name = "cmdMicrophone";
            this.cmdMicrophone.Size = new System.Drawing.Size(75, 23);
            this.cmdMicrophone.TabIndex = 2;
            this.cmdMicrophone.Text = "Microphone";
            this.cmdMicrophone.UseVisualStyleBackColor = true;
            this.cmdMicrophone.Click += new System.EventHandler(this.cmdMicrophone_Click);
            // 
            // cmdOptions
            // 
            this.cmdOptions.Location = new System.Drawing.Point(354, 12);
            this.cmdOptions.Name = "cmdOptions";
            this.cmdOptions.Size = new System.Drawing.Size(75, 23);
            this.cmdOptions.TabIndex = 3;
            this.cmdOptions.Text = "Options";
            this.cmdOptions.UseVisualStyleBackColor = true;
            this.cmdOptions.Click += new System.EventHandler(this.cmdOptions_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Logged in as:";
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Location = new System.Drawing.Point(108, 43);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(37, 13);
            this.lblUserName.TabIndex = 5;
            this.lblUserName.Text = "aaaaa";
            // 
            // frmListener
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 69);
            this.Controls.Add(this.lblUserName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdOptions);
            this.Controls.Add(this.cmdMicrophone);
            this.Controls.Add(this.cmdPhone);
            this.Controls.Add(this.cmdExit);
            this.Name = "frmListener";
            this.Text = "frmListener";
            this.Load += new System.EventHandler(this.frmListener_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Button cmdExit;
        private System.Windows.Forms.Button cmdPhone;
        private System.Windows.Forms.Button cmdMicrophone;
        private System.Windows.Forms.Button cmdOptions;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblUserName;
    }
}