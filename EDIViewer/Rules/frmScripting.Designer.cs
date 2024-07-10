namespace Manual_test_app
{
    partial class frmScripting
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
            this.txtScript = new System.Windows.Forms.TextBox();
            this.lstKeyPair = new System.Windows.Forms.ListBox();
            this.lblRuletInput = new System.Windows.Forms.Label();
            this.lblRuleOuput = new System.Windows.Forms.Label();
            this.cmdRunScript = new System.Windows.Forms.Button();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.cmdKV = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.cmdClear = new System.Windows.Forms.Button();
            this.lstOutput = new System.Windows.Forms.ListBox();
            this.cmdRunTime = new System.Windows.Forms.Button();
            this.lblCount = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtScript
            // 
            this.txtScript.Location = new System.Drawing.Point(12, 118);
            this.txtScript.Multiline = true;
            this.txtScript.Name = "txtScript";
            this.txtScript.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtScript.Size = new System.Drawing.Size(890, 333);
            this.txtScript.TabIndex = 0;
            // 
            // lstKeyPair
            // 
            this.lstKeyPair.FormattingEnabled = true;
            this.lstKeyPair.Location = new System.Drawing.Point(260, 496);
            this.lstKeyPair.Name = "lstKeyPair";
            this.lstKeyPair.Size = new System.Drawing.Size(218, 147);
            this.lstKeyPair.TabIndex = 1;
            this.lstKeyPair.SelectedIndexChanged += new System.EventHandler(this.lstKeyPair_SelectedIndexChanged);
            this.lstKeyPair.DoubleClick += new System.EventHandler(this.lstKeyPair_DoubleClick);
            // 
            // lblRuletInput
            // 
            this.lblRuletInput.AutoSize = true;
            this.lblRuletInput.Location = new System.Drawing.Point(22, 467);
            this.lblRuletInput.Name = "lblRuletInput";
            this.lblRuletInput.Size = new System.Drawing.Size(56, 13);
            this.lblRuletInput.TabIndex = 2;
            this.lblRuletInput.Text = "Rule Input";
            // 
            // lblRuleOuput
            // 
            this.lblRuleOuput.AutoSize = true;
            this.lblRuleOuput.Location = new System.Drawing.Point(503, 467);
            this.lblRuleOuput.Name = "lblRuleOuput";
            this.lblRuleOuput.Size = new System.Drawing.Size(64, 13);
            this.lblRuleOuput.TabIndex = 3;
            this.lblRuleOuput.Text = "Rule Output";
            // 
            // cmdRunScript
            // 
            this.cmdRunScript.Location = new System.Drawing.Point(653, 88);
            this.cmdRunScript.Name = "cmdRunScript";
            this.cmdRunScript.Size = new System.Drawing.Size(75, 23);
            this.cmdRunScript.TabIndex = 5;
            this.cmdRunScript.Text = "Run Rule";
            this.cmdRunScript.UseVisualStyleBackColor = true;
            this.cmdRunScript.Click += new System.EventHandler(this.cmdRunScript_Click);
            // 
            // txtKey
            // 
            this.txtKey.Location = new System.Drawing.Point(96, 496);
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(158, 20);
            this.txtKey.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 498);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Key";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 524);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Value";
            // 
            // txtValue
            // 
            this.txtValue.Location = new System.Drawing.Point(96, 522);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(158, 20);
            this.txtValue.TabIndex = 8;
            // 
            // cmdKV
            // 
            this.cmdKV.Location = new System.Drawing.Point(155, 548);
            this.cmdKV.Name = "cmdKV";
            this.cmdKV.Size = new System.Drawing.Size(99, 23);
            this.cmdKV.TabIndex = 10;
            this.cmdKV.Text = "Add Key Value";
            this.cmdKV.UseVisualStyleBackColor = true;
            this.cmdKV.Click += new System.EventHandler(this.cmdKV_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(13, 90);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(634, 21);
            this.comboBox1.TabIndex = 11;
            // 
            // cmdClear
            // 
            this.cmdClear.Location = new System.Drawing.Point(379, 467);
            this.cmdClear.Name = "cmdClear";
            this.cmdClear.Size = new System.Drawing.Size(99, 23);
            this.cmdClear.TabIndex = 12;
            this.cmdClear.Text = "Clear Key Value";
            this.cmdClear.UseVisualStyleBackColor = true;
            this.cmdClear.Click += new System.EventHandler(this.cmdClear_Click);
            // 
            // lstOutput
            // 
            this.lstOutput.FormattingEnabled = true;
            this.lstOutput.Location = new System.Drawing.Point(506, 496);
            this.lstOutput.Name = "lstOutput";
            this.lstOutput.Size = new System.Drawing.Size(396, 147);
            this.lstOutput.TabIndex = 13;
            // 
            // cmdRunTime
            // 
            this.cmdRunTime.Location = new System.Drawing.Point(734, 88);
            this.cmdRunTime.Name = "cmdRunTime";
            this.cmdRunTime.Size = new System.Drawing.Size(75, 23);
            this.cmdRunTime.TabIndex = 14;
            this.cmdRunTime.Text = "RunTime";
            this.cmdRunTime.UseVisualStyleBackColor = true;
            this.cmdRunTime.Click += new System.EventHandler(this.cmdRunTime_Click);
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(817, 93);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(35, 13);
            this.lblCount.TabIndex = 15;
            this.lblCount.Text = "label3";
            // 
            // frmScripting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 655);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.cmdRunTime);
            this.Controls.Add(this.lstOutput);
            this.Controls.Add(this.cmdClear);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.cmdKV);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtKey);
            this.Controls.Add(this.cmdRunScript);
            this.Controls.Add(this.lblRuleOuput);
            this.Controls.Add(this.lblRuletInput);
            this.Controls.Add(this.lstKeyPair);
            this.Controls.Add(this.txtScript);
            this.Name = "frmScripting";
            this.Text = "frmScripting";
            this.Load += new System.EventHandler(this.frmScripting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtScript;
        private System.Windows.Forms.ListBox lstKeyPair;
        private System.Windows.Forms.Label lblRuletInput;
        private System.Windows.Forms.Label lblRuleOuput;
        private System.Windows.Forms.Button cmdRunScript;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.Button cmdKV;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button cmdClear;
        private System.Windows.Forms.ListBox lstOutput;
        private System.Windows.Forms.Button cmdRunTime;
        private System.Windows.Forms.Label lblCount;
    }
}