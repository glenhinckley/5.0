namespace Manual_test_app
{
    partial class recrisiveserch
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
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.lstFilesFound = new System.Windows.Forms.ListBox();
            this.cboDirectory = new System.Windows.Forms.ComboBox();
            this.lblDirectory = new System.Windows.Forms.Label();
            this.lblFile = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(771, 600);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "button1";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtFile
            // 
            this.txtFile.Location = new System.Drawing.Point(274, 250);
            this.txtFile.Multiline = true;
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(572, 20);
            this.txtFile.TabIndex = 1;
            // 
            // lstFilesFound
            // 
            this.lstFilesFound.FormattingEnabled = true;
            this.lstFilesFound.Location = new System.Drawing.Point(274, 276);
            this.lstFilesFound.Name = "lstFilesFound";
            this.lstFilesFound.Size = new System.Drawing.Size(572, 316);
            this.lstFilesFound.TabIndex = 2;
            // 
            // cboDirectory
            // 
            this.cboDirectory.FormattingEnabled = true;
            this.cboDirectory.Location = new System.Drawing.Point(271, 157);
            this.cboDirectory.Name = "cboDirectory";
            this.cboDirectory.Size = new System.Drawing.Size(575, 21);
            this.cboDirectory.TabIndex = 3;
            // 
            // lblDirectory
            // 
            this.lblDirectory.AutoSize = true;
            this.lblDirectory.Location = new System.Drawing.Point(271, 217);
            this.lblDirectory.Name = "lblDirectory";
            this.lblDirectory.Size = new System.Drawing.Size(35, 13);
            this.lblDirectory.TabIndex = 4;
            this.lblDirectory.Text = "label1";
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Location = new System.Drawing.Point(271, 234);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(35, 13);
            this.lblFile.TabIndex = 5;
            this.lblFile.Text = "label2";
            // 
            // recrisiveserch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1134, 794);
            this.Controls.Add(this.lblFile);
            this.Controls.Add(this.lblDirectory);
            this.Controls.Add(this.cboDirectory);
            this.Controls.Add(this.lstFilesFound);
            this.Controls.Add(this.txtFile);
            this.Controls.Add(this.btnSearch);
            this.Name = "recrisiveserch";
            this.Text = "recrisiveserch";
            this.Load += new System.EventHandler(this.recrisiveserch_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.ListBox lstFilesFound;
        private System.Windows.Forms.ComboBox cboDirectory;
        private System.Windows.Forms.Label lblDirectory;
        private System.Windows.Forms.Label lblFile;
    }
}