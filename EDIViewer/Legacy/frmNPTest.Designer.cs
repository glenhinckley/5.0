﻿namespace Manual_test_app
{
    partial class frmNPTest
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
            this.button1 = new System.Windows.Forms.Button();
            this.txtAppName = new System.Windows.Forms.TextBox();
            this.txtNP = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(399, 323);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtAppName
            // 
            this.txtAppName.Location = new System.Drawing.Point(12, 48);
            this.txtAppName.Name = "txtAppName";
            this.txtAppName.Size = new System.Drawing.Size(462, 20);
            this.txtAppName.TabIndex = 1;
            // 
            // txtNP
            // 
            this.txtNP.Location = new System.Drawing.Point(12, 74);
            this.txtNP.Multiline = true;
            this.txtNP.Name = "txtNP";
            this.txtNP.Size = new System.Drawing.Size(462, 230);
            this.txtNP.TabIndex = 2;
            // 
            // frmNPTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 380);
            this.Controls.Add(this.txtNP);
            this.Controls.Add(this.txtAppName);
            this.Controls.Add(this.button1);
            this.Name = "frmNPTest";
            this.Text = "frmNPTest";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtAppName;
        private System.Windows.Forms.TextBox txtNP;
    }
}