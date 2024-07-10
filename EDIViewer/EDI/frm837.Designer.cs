namespace Manual_test_app
{
    partial class edi837i
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
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdParse = new System.Windows.Forms.Button();
            this.lblFF = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.cmdFileFix = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtOutFile = new System.Windows.Forms.TextBox();
            this.txtInFile = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(183, 338);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 39;
            this.label3.Text = "label3";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(183, 363);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 38;
            this.label1.Text = "label1";
            // 
            // cmdParse
            // 
            this.cmdParse.Location = new System.Drawing.Point(1060, 354);
            this.cmdParse.Name = "cmdParse";
            this.cmdParse.Size = new System.Drawing.Size(75, 23);
            this.cmdParse.TabIndex = 37;
            this.cmdParse.Text = "Parse";
            this.cmdParse.UseVisualStyleBackColor = true;
            // 
            // lblFF
            // 
            this.lblFF.AutoSize = true;
            this.lblFF.Location = new System.Drawing.Point(1038, 317);
            this.lblFF.Name = "lblFF";
            this.lblFF.Size = new System.Drawing.Size(16, 13);
            this.lblFF.TabIndex = 36;
            this.lblFF.Text = "re";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(511, 314);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 35;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // cmdFileFix
            // 
            this.cmdFileFix.Location = new System.Drawing.Point(1060, 312);
            this.cmdFileFix.Name = "cmdFileFix";
            this.cmdFileFix.Size = new System.Drawing.Size(165, 23);
            this.cmdFileFix.TabIndex = 34;
            this.cmdFileFix.Text = "FixFile";
            this.cmdFileFix.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(647, 318);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(62, 13);
            this.label15.TabIndex = 33;
            this.label15.Text = "Out Put File";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(111, 317);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(50, 13);
            this.label11.TabIndex = 32;
            this.label11.Text = "Input File";
            // 
            // txtOutFile
            // 
            this.txtOutFile.Location = new System.Drawing.Point(726, 314);
            this.txtOutFile.Name = "txtOutFile";
            this.txtOutFile.Size = new System.Drawing.Size(292, 20);
            this.txtOutFile.TabIndex = 31;
            // 
            // txtInFile
            // 
            this.txtInFile.Location = new System.Drawing.Point(167, 315);
            this.txtInFile.Name = "txtInFile";
            this.txtInFile.Size = new System.Drawing.Size(338, 20);
            this.txtInFile.TabIndex = 30;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // edi837i
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1336, 688);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdParse);
            this.Controls.Add(this.lblFF);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cmdFileFix);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtOutFile);
            this.Controls.Add(this.txtInFile);
            this.Name = "edi837i";
            this.Text = "edi837i";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cmdParse;
        private System.Windows.Forms.Label lblFF;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button cmdFileFix;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtOutFile;
        private System.Windows.Forms.TextBox txtInFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}