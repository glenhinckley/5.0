namespace DCSGlobal.Medical.VoiceRecoder
{
    partial class trmTestSplit
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
            this.panelDemo = new System.Windows.Forms.Panel();
            this.listBoxDemos = new System.Windows.Forms.ListBox();
            this.buttonLoadDemo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panelDemo
            // 
            this.panelDemo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelDemo.Location = new System.Drawing.Point(197, 2);
            this.panelDemo.Name = "panelDemo";
            this.panelDemo.Size = new System.Drawing.Size(681, 405);
            this.panelDemo.TabIndex = 6;
            // 
            // listBoxDemos
            // 
            this.listBoxDemos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxDemos.FormattingEnabled = true;
            this.listBoxDemos.Location = new System.Drawing.Point(39, 12);
            this.listBoxDemos.Name = "listBoxDemos";
            this.listBoxDemos.Size = new System.Drawing.Size(120, 329);
            this.listBoxDemos.TabIndex = 7;
            // 
            // buttonLoadDemo
            // 
            this.buttonLoadDemo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonLoadDemo.Location = new System.Drawing.Point(57, 361);
            this.buttonLoadDemo.Name = "buttonLoadDemo";
            this.buttonLoadDemo.Size = new System.Drawing.Size(75, 23);
            this.buttonLoadDemo.TabIndex = 8;
            this.buttonLoadDemo.Text = "Load";
            this.buttonLoadDemo.UseVisualStyleBackColor = true;
            this.buttonLoadDemo.Click += new System.EventHandler(this.buttonLoadDemo_Click);
            // 
            // trmTestSplit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1074, 408);
            this.Controls.Add(this.buttonLoadDemo);
            this.Controls.Add(this.listBoxDemos);
            this.Controls.Add(this.panelDemo);
            this.Name = "trmTestSplit";
            this.Text = "trmTestSplit";
            this.Load += new System.EventHandler(this.trmTestSplit_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelDemo;
        private System.Windows.Forms.ListBox listBoxDemos;
        private System.Windows.Forms.Button buttonLoadDemo;
    }
}