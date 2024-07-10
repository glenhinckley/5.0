namespace Manual_test_app.DEPLOYMENT
{
    partial class frmDeployPackage
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
            this.cmbPackages = new System.Windows.Forms.ComboBox();
            this.cmdDecode = new System.Windows.Forms.Button();
            this.txtConstring = new System.Windows.Forms.TextBox();
            this.cmdGo = new System.Windows.Forms.Button();
            this.tc = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.wbReport = new System.Windows.Forms.WebBrowser();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tslSelectedPackage = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslTarget = new System.Windows.Forms.ToolStripStatusLabel();
            this.tc.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbPackages
            // 
            this.cmbPackages.FormattingEnabled = true;
            this.cmbPackages.Location = new System.Drawing.Point(18, 39);
            this.cmbPackages.Name = "cmbPackages";
            this.cmbPackages.Size = new System.Drawing.Size(912, 21);
            this.cmbPackages.TabIndex = 0;
            this.cmbPackages.SelectedIndexChanged += new System.EventHandler(this.cmbPackages_SelectedIndexChanged);
            // 
            // cmdDecode
            // 
            this.cmdDecode.Location = new System.Drawing.Point(38, 705);
            this.cmdDecode.Name = "cmdDecode";
            this.cmdDecode.Size = new System.Drawing.Size(75, 23);
            this.cmdDecode.TabIndex = 11;
            this.cmdDecode.Text = "Decode";
            this.cmdDecode.UseVisualStyleBackColor = true;
            // 
            // txtConstring
            // 
            this.txtConstring.Location = new System.Drawing.Point(119, 708);
            this.txtConstring.Name = "txtConstring";
            this.txtConstring.Size = new System.Drawing.Size(892, 20);
            this.txtConstring.TabIndex = 10;
            // 
            // cmdGo
            // 
            this.cmdGo.Location = new System.Drawing.Point(936, 37);
            this.cmdGo.Name = "cmdGo";
            this.cmdGo.Size = new System.Drawing.Size(75, 23);
            this.cmdGo.TabIndex = 13;
            this.cmdGo.Text = "Go";
            this.cmdGo.UseVisualStyleBackColor = true;
            this.cmdGo.Click += new System.EventHandler(this.cmdGo_Click);
            // 
            // tc
            // 
            this.tc.Controls.Add(this.tabPage1);
            this.tc.Controls.Add(this.tabPage2);
            this.tc.Controls.Add(this.tabPage3);
            this.tc.Location = new System.Drawing.Point(18, 66);
            this.tc.Name = "tc";
            this.tc.SelectedIndex = 0;
            this.tc.Size = new System.Drawing.Size(993, 623);
            this.tc.TabIndex = 14;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.wbReport);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(985, 597);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "HTML Report";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(985, 597);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "SQL";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // wbReport
            // 
            this.wbReport.AllowNavigation = false;
            this.wbReport.AllowWebBrowserDrop = false;
            this.wbReport.Location = new System.Drawing.Point(6, 6);
            this.wbReport.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbReport.Name = "wbReport";
            this.wbReport.Size = new System.Drawing.Size(973, 585);
            this.wbReport.TabIndex = 13;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(985, 597);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "File List";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslSelectedPackage,
            this.tslTarget});
            this.statusStrip1.Location = new System.Drawing.Point(0, 761);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1034, 22);
            this.statusStrip1.TabIndex = 15;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.statusStrip1_ItemClicked);
            // 
            // tslSelectedPackage
            // 
            this.tslSelectedPackage.Name = "tslSelectedPackage";
            this.tslSelectedPackage.Size = new System.Drawing.Size(106, 17);
            this.tslSelectedPackage.Text = "No selected Package";
            // 
            // tslTarget
            // 
            this.tslTarget.Name = "tslTarget";
            this.tslTarget.Size = new System.Drawing.Size(99, 17);
            this.tslTarget.Text = "No Target Selected";
            // 
            // frmDeployPackage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1034, 783);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tc);
            this.Controls.Add(this.cmdGo);
            this.Controls.Add(this.cmdDecode);
            this.Controls.Add(this.txtConstring);
            this.Controls.Add(this.cmbPackages);
            this.Name = "frmDeployPackage";
            this.Text = "frmDeploy";
            this.Load += new System.EventHandler(this.frmDeployPackage_Load);
            this.tc.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbPackages;
        private System.Windows.Forms.Button cmdDecode;
        private System.Windows.Forms.TextBox txtConstring;
        private System.Windows.Forms.Button cmdGo;
        private System.Windows.Forms.TabControl tc;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.WebBrowser wbReport;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tslSelectedPackage;
        private System.Windows.Forms.ToolStripStatusLabel tslTarget;
    }
}