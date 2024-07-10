namespace Manual_test_app
{
    partial class EventLog
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
            this.Close = new System.Windows.Forms.Button();
            this.txtEvent = new System.Windows.Forms.TextBox();
            this.txtID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.cmdWarning = new System.Windows.Forms.Button();
            this.cmdError = new System.Windows.Forms.Button();
            this.cmdInformation = new System.Windows.Forms.Button();
            this.cmdFailureAudit = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Close
            // 
            this.Close.Location = new System.Drawing.Point(391, 294);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(75, 23);
            this.Close.TabIndex = 0;
            this.Close.Text = "Close";
            this.Close.UseVisualStyleBackColor = true;
            this.Close.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtEvent
            // 
            this.txtEvent.Location = new System.Drawing.Point(79, 45);
            this.txtEvent.Name = "txtEvent";
            this.txtEvent.Size = new System.Drawing.Size(100, 20);
            this.txtEvent.TabIndex = 1;
            // 
            // txtID
            // 
            this.txtID.Location = new System.Drawing.Point(79, 71);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(100, 20);
            this.txtID.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Event:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "ID:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Description:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(79, 110);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(307, 171);
            this.txtDescription.TabIndex = 6;
            // 
            // cmdWarning
            // 
            this.cmdWarning.Location = new System.Drawing.Point(392, 108);
            this.cmdWarning.Name = "cmdWarning";
            this.cmdWarning.Size = new System.Drawing.Size(75, 23);
            this.cmdWarning.TabIndex = 7;
            this.cmdWarning.Text = "Warning";
            this.cmdWarning.UseVisualStyleBackColor = true;
            this.cmdWarning.Click += new System.EventHandler(this.cmdWarning_Click);
            // 
            // cmdError
            // 
            this.cmdError.Location = new System.Drawing.Point(391, 137);
            this.cmdError.Name = "cmdError";
            this.cmdError.Size = new System.Drawing.Size(75, 23);
            this.cmdError.TabIndex = 8;
            this.cmdError.Text = "Error";
            this.cmdError.UseVisualStyleBackColor = true;
            this.cmdError.Click += new System.EventHandler(this.cmdError_Click);
            // 
            // cmdInformation
            // 
            this.cmdInformation.Location = new System.Drawing.Point(391, 166);
            this.cmdInformation.Name = "cmdInformation";
            this.cmdInformation.Size = new System.Drawing.Size(75, 23);
            this.cmdInformation.TabIndex = 9;
            this.cmdInformation.Text = "Information";
            this.cmdInformation.UseVisualStyleBackColor = true;
            this.cmdInformation.Click += new System.EventHandler(this.cmdInformation_Click);
            // 
            // cmdFailureAudit
            // 
            this.cmdFailureAudit.Location = new System.Drawing.Point(391, 195);
            this.cmdFailureAudit.Name = "cmdFailureAudit";
            this.cmdFailureAudit.Size = new System.Drawing.Size(75, 23);
            this.cmdFailureAudit.TabIndex = 10;
            this.cmdFailureAudit.Text = "FailureAudit";
            this.cmdFailureAudit.UseVisualStyleBackColor = true;
            this.cmdFailureAudit.Click += new System.EventHandler(this.cmdFailureAudit_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(186, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Must be a 16bit integer";
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Location = new System.Drawing.Point(79, 294);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(0, 13);
            this.lblError.TabIndex = 12;
            // 
            // EventLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 342);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmdFailureAudit);
            this.Controls.Add(this.cmdInformation);
            this.Controls.Add(this.cmdError);
            this.Controls.Add(this.cmdWarning);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtID);
            this.Controls.Add(this.txtEvent);
            this.Controls.Add(this.Close);
            this.Name = "EventLog";
            this.Text = "EventLog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Close;
        private System.Windows.Forms.TextBox txtEvent;
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Button cmdWarning;
        private System.Windows.Forms.Button cmdError;
        private System.Windows.Forms.Button cmdInformation;
        private System.Windows.Forms.Button cmdFailureAudit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblError;
    }
}