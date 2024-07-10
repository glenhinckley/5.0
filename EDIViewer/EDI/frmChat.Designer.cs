namespace Manual_test_app
{
    partial class frmChat
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
            this.m_lbRecievedData = new System.Windows.Forms.ListBox();
            this.m_tbMessage = new System.Windows.Forms.TextBox();
            this.m_tbServerAddress = new System.Windows.Forms.TextBox();
            this.m_btnConnect = new System.Windows.Forms.Button();
            this.m_btnSend = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_lbRecievedData
            // 
            this.m_lbRecievedData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_lbRecievedData.IntegralHeight = false;
            this.m_lbRecievedData.Location = new System.Drawing.Point(28, 95);
            this.m_lbRecievedData.Name = "m_lbRecievedData";
            this.m_lbRecievedData.Size = new System.Drawing.Size(656, 340);
            this.m_lbRecievedData.TabIndex = 3;
            // 
            // m_tbMessage
            // 
            this.m_tbMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_tbMessage.Location = new System.Drawing.Point(27, 69);
            this.m_tbMessage.Name = "m_tbMessage";
            this.m_tbMessage.Size = new System.Drawing.Size(576, 20);
            this.m_tbMessage.TabIndex = 4;
            // 
            // m_tbServerAddress
            // 
            this.m_tbServerAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_tbServerAddress.Location = new System.Drawing.Point(28, 12);
            this.m_tbServerAddress.Name = "m_tbServerAddress";
            this.m_tbServerAddress.Size = new System.Drawing.Size(204, 20);
            this.m_tbServerAddress.TabIndex = 5;
            this.m_tbServerAddress.Text = "192.168.0.8";
            // 
            // m_btnConnect
            // 
            this.m_btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_btnConnect.Location = new System.Drawing.Point(609, 12);
            this.m_btnConnect.Name = "m_btnConnect";
            this.m_btnConnect.Size = new System.Drawing.Size(75, 23);
            this.m_btnConnect.TabIndex = 6;
            this.m_btnConnect.Text = "Connect";
            this.m_btnConnect.Click += new System.EventHandler(this.m_btnConnect_Click);
            // 
            // m_btnSend
            // 
            this.m_btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_btnSend.Location = new System.Drawing.Point(609, 66);
            this.m_btnSend.Name = "m_btnSend";
            this.m_btnSend.Size = new System.Drawing.Size(75, 23);
            this.m_btnSend.TabIndex = 7;
            this.m_btnSend.Text = "Send";
            // 
            // frmChat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(696, 447);
            this.Controls.Add(this.m_btnSend);
            this.Controls.Add(this.m_btnConnect);
            this.Controls.Add(this.m_tbServerAddress);
            this.Controls.Add(this.m_tbMessage);
            this.Controls.Add(this.m_lbRecievedData);
            this.Name = "frmChat";
            this.Text = "frmChat";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox m_lbRecievedData;
        private System.Windows.Forms.TextBox m_tbMessage;
        private System.Windows.Forms.TextBox m_tbServerAddress;
        private System.Windows.Forms.Button m_btnConnect;
        private System.Windows.Forms.Button m_btnSend;
    }
}