namespace Manual_test_app.Rules
{
    partial class RuleResults
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RuleResults));
            this.cmdRetriveEvents = new System.Windows.Forms.Button();
            this.cmbContext = new System.Windows.Forms.ComboBox();
            this.cmbRule = new System.Windows.Forms.ComboBox();
            this.cmbHOSP_CODE = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.fctRULE_VB = new FastColoredTextBoxNS.FastColoredTextBox();
            this.fastColoredTextBox1 = new FastColoredTextBoxNS.FastColoredTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbEvent = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPatNumber = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.cmdClear = new System.Windows.Forms.Button();
            this.cmdDecode = new System.Windows.Forms.Button();
            this.txtConstring = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.fctRULE_VB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdRetriveEvents
            // 
            this.cmdRetriveEvents.Location = new System.Drawing.Point(862, 74);
            this.cmdRetriveEvents.Name = "cmdRetriveEvents";
            this.cmdRetriveEvents.Size = new System.Drawing.Size(121, 23);
            this.cmdRetriveEvents.TabIndex = 0;
            this.cmdRetriveEvents.Text = "Retrive Events";
            this.cmdRetriveEvents.UseVisualStyleBackColor = true;
            this.cmdRetriveEvents.Click += new System.EventHandler(this.cmdRetriveEvents_Click);
            // 
            // cmbContext
            // 
            this.cmbContext.FormattingEnabled = true;
            this.cmbContext.Location = new System.Drawing.Point(14, 36);
            this.cmbContext.Name = "cmbContext";
            this.cmbContext.Size = new System.Drawing.Size(121, 21);
            this.cmbContext.TabIndex = 1;
            // 
            // cmbRule
            // 
            this.cmbRule.FormattingEnabled = true;
            this.cmbRule.Location = new System.Drawing.Point(141, 35);
            this.cmbRule.Name = "cmbRule";
            this.cmbRule.Size = new System.Drawing.Size(715, 21);
            this.cmbRule.TabIndex = 2;
            // 
            // cmbHOSP_CODE
            // 
            this.cmbHOSP_CODE.FormattingEnabled = true;
            this.cmbHOSP_CODE.Location = new System.Drawing.Point(862, 35);
            this.cmbHOSP_CODE.Name = "cmbHOSP_CODE";
            this.cmbHOSP_CODE.Size = new System.Drawing.Size(121, 21);
            this.cmbHOSP_CODE.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Context";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(138, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Rule";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(859, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "HOSP CODE";
            // 
            // fctRULE_VB
            // 
            this.fctRULE_VB.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.fctRULE_VB.AutoIndentCharsPatterns = "\r\n^\\s*[\\w\\.\\(\\)]+\\s*(?<range>=)\\s*(?<range>.+)\r\n";
            this.fctRULE_VB.AutoScrollMinSize = new System.Drawing.Size(0, 14);
            this.fctRULE_VB.BackBrush = null;
            this.fctRULE_VB.CharHeight = 14;
            this.fctRULE_VB.CharWidth = 8;
            this.fctRULE_VB.CommentPrefix = "\'";
            this.fctRULE_VB.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fctRULE_VB.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fctRULE_VB.IsReplaceMode = false;
            this.fctRULE_VB.Language = FastColoredTextBoxNS.Language.VB;
            this.fctRULE_VB.LeftBracket = '(';
            this.fctRULE_VB.Location = new System.Drawing.Point(472, 109);
            this.fctRULE_VB.Name = "fctRULE_VB";
            this.fctRULE_VB.Paddings = new System.Windows.Forms.Padding(0);
            this.fctRULE_VB.RightBracket = ')';
            this.fctRULE_VB.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fctRULE_VB.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctRULE_VB.ServiceColors")));
            this.fctRULE_VB.Size = new System.Drawing.Size(511, 473);
            this.fctRULE_VB.TabIndex = 91;
            this.fctRULE_VB.Text = "Paste or select File::Open";
            this.fctRULE_VB.WordWrap = true;
            this.fctRULE_VB.Zoom = 100;
            // 
            // fastColoredTextBox1
            // 
            this.fastColoredTextBox1.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.fastColoredTextBox1.AutoIndentCharsPatterns = "\r\n^\\s*[\\w\\.\\(\\)]+\\s*(?<range>=)\\s*(?<range>.+)\r\n";
            this.fastColoredTextBox1.AutoScrollMinSize = new System.Drawing.Size(0, 14);
            this.fastColoredTextBox1.BackBrush = null;
            this.fastColoredTextBox1.CharHeight = 14;
            this.fastColoredTextBox1.CharWidth = 8;
            this.fastColoredTextBox1.CommentPrefix = "\'";
            this.fastColoredTextBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fastColoredTextBox1.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fastColoredTextBox1.IsReplaceMode = false;
            this.fastColoredTextBox1.Language = FastColoredTextBoxNS.Language.VB;
            this.fastColoredTextBox1.LeftBracket = '(';
            this.fastColoredTextBox1.Location = new System.Drawing.Point(17, 109);
            this.fastColoredTextBox1.Name = "fastColoredTextBox1";
            this.fastColoredTextBox1.Paddings = new System.Windows.Forms.Padding(0);
            this.fastColoredTextBox1.RightBracket = ')';
            this.fastColoredTextBox1.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fastColoredTextBox1.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fastColoredTextBox1.ServiceColors")));
            this.fastColoredTextBox1.Size = new System.Drawing.Size(449, 473);
            this.fastColoredTextBox1.TabIndex = 92;
            this.fastColoredTextBox1.Text = "Paste or select File::Open";
            this.fastColoredTextBox1.WordWrap = true;
            this.fastColoredTextBox1.Zoom = 100;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(138, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 94;
            this.label4.Text = "Event";
            // 
            // cmbEvent
            // 
            this.cmbEvent.FormattingEnabled = true;
            this.cmbEvent.Location = new System.Drawing.Point(141, 74);
            this.cmbEvent.Name = "cmbEvent";
            this.cmbEvent.Size = new System.Drawing.Size(715, 21);
            this.cmbEvent.TabIndex = 93;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 95;
            this.label5.Text = "Event";
            // 
            // txtPatNumber
            // 
            this.txtPatNumber.Location = new System.Drawing.Point(16, 74);
            this.txtPatNumber.Name = "txtPatNumber";
            this.txtPatNumber.Size = new System.Drawing.Size(119, 20);
            this.txtPatNumber.TabIndex = 96;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(144, 599);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(839, 68);
            this.textBox1.TabIndex = 97;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(144, 673);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(839, 68);
            this.textBox2.TabIndex = 98;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 602);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(124, 13);
            this.label6.TabIndex = 99;
            this.label6.Text = "Rule Error Message VBS";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 676);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(117, 13);
            this.label7.TabIndex = 100;
            this.label7.Text = "Rule Error Message VB";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(661, 747);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(195, 23);
            this.button1.TabIndex = 101;
            this.button1.Text = "Verify Rule Defenition";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // cmdClear
            // 
            this.cmdClear.Location = new System.Drawing.Point(862, 747);
            this.cmdClear.Name = "cmdClear";
            this.cmdClear.Size = new System.Drawing.Size(121, 23);
            this.cmdClear.TabIndex = 102;
            this.cmdClear.Text = "Clear";
            this.cmdClear.UseVisualStyleBackColor = true;
            // 
            // cmdDecode
            // 
            this.cmdDecode.Location = new System.Drawing.Point(906, 786);
            this.cmdDecode.Name = "cmdDecode";
            this.cmdDecode.Size = new System.Drawing.Size(77, 23);
            this.cmdDecode.TabIndex = 170;
            this.cmdDecode.Text = "Decode";
            this.cmdDecode.UseVisualStyleBackColor = true;
            this.cmdDecode.Click += new System.EventHandler(this.cmdDecode_Click);
            // 
            // txtConstring
            // 
            this.txtConstring.Location = new System.Drawing.Point(4, 788);
            this.txtConstring.Name = "txtConstring";
            this.txtConstring.Size = new System.Drawing.Size(896, 20);
            this.txtConstring.TabIndex = 169;
            // 
            // RuleResults
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1005, 835);
            this.Controls.Add(this.cmdDecode);
            this.Controls.Add(this.txtConstring);
            this.Controls.Add(this.cmdClear);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.txtPatNumber);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbEvent);
            this.Controls.Add(this.fastColoredTextBox1);
            this.Controls.Add(this.fctRULE_VB);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbHOSP_CODE);
            this.Controls.Add(this.cmbRule);
            this.Controls.Add(this.cmbContext);
            this.Controls.Add(this.cmdRetriveEvents);
            this.Name = "RuleResults";
            this.Text = "RuleResults";
            this.Load += new System.EventHandler(this.RuleResults_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fctRULE_VB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdRetriveEvents;
        private System.Windows.Forms.ComboBox cmbContext;
        private System.Windows.Forms.ComboBox cmbRule;
        private System.Windows.Forms.ComboBox cmbHOSP_CODE;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private FastColoredTextBoxNS.FastColoredTextBox fctRULE_VB;
        private FastColoredTextBoxNS.FastColoredTextBox fastColoredTextBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbEvent;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPatNumber;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button cmdClear;
        private System.Windows.Forms.Button cmdDecode;
        private System.Windows.Forms.TextBox txtConstring;
    }
}