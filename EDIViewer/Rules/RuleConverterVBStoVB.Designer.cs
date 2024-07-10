namespace Manual_test_app.Rules
{
    partial class RuleConverterVBStoVB
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RuleConverterVBStoVB));
            this.fctRULE_VB = new FastColoredTextBoxNS.FastColoredTextBox();
            this.fctRULE_VBS = new FastColoredTextBoxNS.FastColoredTextBox();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.cmdDecode = new System.Windows.Forms.Button();
            this.txtConstring = new System.Windows.Forms.TextBox();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.txtSQL = new System.Windows.Forms.TextBox();
            this.txtRuleID = new System.Windows.Forms.TextBox();
            this.txtRuleName = new System.Windows.Forms.TextBox();
            this.txtRuleDescription = new System.Windows.Forms.TextBox();
            this.cmbRuleStatus = new System.Windows.Forms.ComboBox();
            this.cmbApprovedStatus = new System.Windows.Forms.ComboBox();
            this.cmbRuleType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.pnlParserCMDGroup = new System.Windows.Forms.Panel();
            this.cmdBuildDll = new System.Windows.Forms.Button();
            this.chkAutoParse = new System.Windows.Forms.CheckBox();
            this.cmdPtoVB = new System.Windows.Forms.Button();
            this.cmdTestRule = new System.Windows.Forms.Button();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.txtTop = new System.Windows.Forms.TextBox();
            this.cmdSearch = new System.Windows.Forms.Button();
            this.cmdCompileRule = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.fctRULE_VB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fctRULE_VBS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.pnlParserCMDGroup.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.SuspendLayout();
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
            this.fctRULE_VB.Location = new System.Drawing.Point(3, 3);
            this.fctRULE_VB.Name = "fctRULE_VB";
            this.fctRULE_VB.Paddings = new System.Windows.Forms.Padding(0);
            this.fctRULE_VB.RightBracket = ')';
            this.fctRULE_VB.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fctRULE_VB.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctRULE_VB.ServiceColors")));
            this.fctRULE_VB.Size = new System.Drawing.Size(701, 400);
            this.fctRULE_VB.TabIndex = 90;
            this.fctRULE_VB.Text = "Paste or select File::Open";
            this.fctRULE_VB.WordWrap = true;
            this.fctRULE_VB.Zoom = 100;
            // 
            // fctRULE_VBS
            // 
            this.fctRULE_VBS.AutoCompleteBracketsList = new char[] {
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
            this.fctRULE_VBS.AutoScrollMinSize = new System.Drawing.Size(0, 14);
            this.fctRULE_VBS.BackBrush = null;
            this.fctRULE_VBS.CharHeight = 14;
            this.fctRULE_VBS.CharWidth = 8;
            this.fctRULE_VBS.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fctRULE_VBS.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fctRULE_VBS.IsReplaceMode = false;
            this.fctRULE_VBS.Location = new System.Drawing.Point(3, 3);
            this.fctRULE_VBS.Name = "fctRULE_VBS";
            this.fctRULE_VBS.Paddings = new System.Windows.Forms.Padding(0);
            this.fctRULE_VBS.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fctRULE_VBS.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctRULE_VBS.ServiceColors")));
            this.fctRULE_VBS.Size = new System.Drawing.Size(351, 400);
            this.fctRULE_VBS.TabIndex = 91;
            this.fctRULE_VBS.Text = "Paste or select File::Open";
            this.fctRULE_VBS.WordWrap = true;
            this.fctRULE_VBS.Zoom = 100;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(42, 17);
            this.toolStripStatusLabel1.Text = "ERROR";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(26, 422);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.fctRULE_VBS);
            this.splitContainer1.Panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel1_Paint);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.fctRULE_VB);
            this.splitContainer1.Panel2.SizeChanged += new System.EventHandler(this.splitContainer1_Panel2_SizeChanged);
            this.splitContainer1.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel2_Paint);
            this.splitContainer1.Size = new System.Drawing.Size(1071, 416);
            this.splitContainer1.SplitterDistance = 357;
            this.splitContainer1.TabIndex = 123;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 944);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1120, 22);
            this.statusStrip1.TabIndex = 122;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // cmdDecode
            // 
            this.cmdDecode.Location = new System.Drawing.Point(1030, 907);
            this.cmdDecode.Name = "cmdDecode";
            this.cmdDecode.Size = new System.Drawing.Size(75, 23);
            this.cmdDecode.TabIndex = 121;
            this.cmdDecode.Text = "Decode";
            this.cmdDecode.UseVisualStyleBackColor = true;
            // 
            // txtConstring
            // 
            this.txtConstring.Location = new System.Drawing.Point(26, 907);
            this.txtConstring.Name = "txtConstring";
            this.txtConstring.Size = new System.Drawing.Size(991, 20);
            this.txtConstring.TabIndex = 120;
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(22, 59);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(1075, 185);
            this.dataGridView2.TabIndex = 109;
            this.dataGridView2.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellContentClick);
            this.dataGridView2.SelectionChanged += new System.EventHandler(this.dataGridView2_SelectionChanged);
            // 
            // txtSQL
            // 
            this.txtSQL.Location = new System.Drawing.Point(22, 6);
            this.txtSQL.Multiline = true;
            this.txtSQL.Name = "txtSQL";
            this.txtSQL.Size = new System.Drawing.Size(971, 47);
            this.txtSQL.TabIndex = 124;
            // 
            // txtRuleID
            // 
            this.txtRuleID.Location = new System.Drawing.Point(29, 274);
            this.txtRuleID.Name = "txtRuleID";
            this.txtRuleID.Size = new System.Drawing.Size(100, 20);
            this.txtRuleID.TabIndex = 126;
            // 
            // txtRuleName
            // 
            this.txtRuleName.Location = new System.Drawing.Point(146, 274);
            this.txtRuleName.Name = "txtRuleName";
            this.txtRuleName.Size = new System.Drawing.Size(950, 20);
            this.txtRuleName.TabIndex = 127;
            // 
            // txtRuleDescription
            // 
            this.txtRuleDescription.Location = new System.Drawing.Point(145, 318);
            this.txtRuleDescription.Multiline = true;
            this.txtRuleDescription.Name = "txtRuleDescription";
            this.txtRuleDescription.Size = new System.Drawing.Size(951, 51);
            this.txtRuleDescription.TabIndex = 128;
            // 
            // cmbRuleStatus
            // 
            this.cmbRuleStatus.FormattingEnabled = true;
            this.cmbRuleStatus.Location = new System.Drawing.Point(146, 391);
            this.cmbRuleStatus.Name = "cmbRuleStatus";
            this.cmbRuleStatus.Size = new System.Drawing.Size(121, 21);
            this.cmbRuleStatus.TabIndex = 129;
            // 
            // cmbApprovedStatus
            // 
            this.cmbApprovedStatus.FormattingEnabled = true;
            this.cmbApprovedStatus.Location = new System.Drawing.Point(273, 391);
            this.cmbApprovedStatus.Name = "cmbApprovedStatus";
            this.cmbApprovedStatus.Size = new System.Drawing.Size(121, 21);
            this.cmbApprovedStatus.TabIndex = 130;
            // 
            // cmbRuleType
            // 
            this.cmbRuleType.FormattingEnabled = true;
            this.cmbRuleType.Location = new System.Drawing.Point(400, 391);
            this.cmbRuleType.Name = "cmbRuleType";
            this.cmbRuleType.Size = new System.Drawing.Size(121, 21);
            this.cmbRuleType.TabIndex = 131;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 255);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 132;
            this.label1.Text = "Rule ID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(146, 255);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 133;
            this.label2.Text = "Rule Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(145, 299);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 134;
            this.label3.Text = "Rule Description";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(145, 376);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 135;
            this.label4.Text = "label4";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(273, 376);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 136;
            this.label5.Text = "label5";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(400, 374);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 13);
            this.label6.TabIndex = 137;
            this.label6.Text = "label6";
            // 
            // pnlParserCMDGroup
            // 
            this.pnlParserCMDGroup.Controls.Add(this.cmdCompileRule);
            this.pnlParserCMDGroup.Controls.Add(this.cmdBuildDll);
            this.pnlParserCMDGroup.Controls.Add(this.chkAutoParse);
            this.pnlParserCMDGroup.Controls.Add(this.cmdPtoVB);
            this.pnlParserCMDGroup.Controls.Add(this.cmdTestRule);
            this.pnlParserCMDGroup.Location = new System.Drawing.Point(603, 382);
            this.pnlParserCMDGroup.Name = "pnlParserCMDGroup";
            this.pnlParserCMDGroup.Size = new System.Drawing.Size(498, 37);
            this.pnlParserCMDGroup.TabIndex = 164;
            // 
            // cmdBuildDll
            // 
            this.cmdBuildDll.Location = new System.Drawing.Point(396, 6);
            this.cmdBuildDll.Name = "cmdBuildDll";
            this.cmdBuildDll.Size = new System.Drawing.Size(75, 23);
            this.cmdBuildDll.TabIndex = 167;
            this.cmdBuildDll.Text = "Build DLL";
            this.cmdBuildDll.UseVisualStyleBackColor = true;
            this.cmdBuildDll.Click += new System.EventHandler(this.cmdBuildDll_Click);
            // 
            // chkAutoParse
            // 
            this.chkAutoParse.AutoSize = true;
            this.chkAutoParse.Location = new System.Drawing.Point(7, 12);
            this.chkAutoParse.Name = "chkAutoParse";
            this.chkAutoParse.Size = new System.Drawing.Size(78, 17);
            this.chkAutoParse.TabIndex = 166;
            this.chkAutoParse.Text = "Auto Parse";
            this.chkAutoParse.UseVisualStyleBackColor = true;
            // 
            // cmdPtoVB
            // 
            this.cmdPtoVB.Location = new System.Drawing.Point(111, 7);
            this.cmdPtoVB.Name = "cmdPtoVB";
            this.cmdPtoVB.Size = new System.Drawing.Size(75, 23);
            this.cmdPtoVB.TabIndex = 165;
            this.cmdPtoVB.Text = "Parse To VB";
            this.cmdPtoVB.UseVisualStyleBackColor = true;
            this.cmdPtoVB.Click += new System.EventHandler(this.cmdPtoVB_Click_1);
            // 
            // cmdTestRule
            // 
            this.cmdTestRule.Location = new System.Drawing.Point(192, 6);
            this.cmdTestRule.Name = "cmdTestRule";
            this.cmdTestRule.Size = new System.Drawing.Size(75, 23);
            this.cmdTestRule.TabIndex = 164;
            this.cmdTestRule.Text = "Test Rule";
            this.cmdTestRule.UseVisualStyleBackColor = true;
            this.cmdTestRule.Click += new System.EventHandler(this.cmdTestRule_Click_1);
            // 
            // pnlSearch
            // 
            this.pnlSearch.Controls.Add(this.txtTop);
            this.pnlSearch.Controls.Add(this.cmdSearch);
            this.pnlSearch.Location = new System.Drawing.Point(999, 6);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(97, 47);
            this.pnlSearch.TabIndex = 165;
            // 
            // txtTop
            // 
            this.txtTop.Location = new System.Drawing.Point(17, 2);
            this.txtTop.Name = "txtTop";
            this.txtTop.Size = new System.Drawing.Size(75, 20);
            this.txtTop.TabIndex = 127;
            this.txtTop.Text = "100";
            this.txtTop.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cmdSearch
            // 
            this.cmdSearch.Location = new System.Drawing.Point(19, 24);
            this.cmdSearch.Name = "cmdSearch";
            this.cmdSearch.Size = new System.Drawing.Size(75, 23);
            this.cmdSearch.TabIndex = 126;
            this.cmdSearch.Text = "Search";
            this.cmdSearch.UseVisualStyleBackColor = true;
            this.cmdSearch.Click += new System.EventHandler(this.cmdSearch_Click_1);
            // 
            // cmdCompileRule
            // 
            this.cmdCompileRule.Location = new System.Drawing.Point(273, 7);
            this.cmdCompileRule.Name = "cmdCompileRule";
            this.cmdCompileRule.Size = new System.Drawing.Size(117, 23);
            this.cmdCompileRule.TabIndex = 168;
            this.cmdCompileRule.Text = "Compile Rule";
            this.cmdCompileRule.UseMnemonic = false;
            this.cmdCompileRule.UseVisualStyleBackColor = true;
            this.cmdCompileRule.Click += new System.EventHandler(this.cmdCompileRule_Click);
            // 
            // RuleConverterVBStoVB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1120, 966);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.pnlParserCMDGroup);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbRuleType);
            this.Controls.Add(this.cmbApprovedStatus);
            this.Controls.Add(this.cmbRuleStatus);
            this.Controls.Add(this.txtRuleDescription);
            this.Controls.Add(this.txtRuleName);
            this.Controls.Add(this.txtRuleID);
            this.Controls.Add(this.txtSQL);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.cmdDecode);
            this.Controls.Add(this.txtConstring);
            this.Controls.Add(this.dataGridView2);
            this.Name = "RuleConverterVBStoVB";
            this.Text = "RuleEditor";
            this.Load += new System.EventHandler(this.RuleEditor_Load);
            this.ResizeEnd += new System.EventHandler(this.RuleConverterVBStoVB_ResizeEnd);
            this.SizeChanged += new System.EventHandler(this.RuleConverterVBStoVB_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.fctRULE_VB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fctRULE_VBS)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.pnlParserCMDGroup.ResumeLayout(false);
            this.pnlParserCMDGroup.PerformLayout();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FastColoredTextBoxNS.FastColoredTextBox fctRULE_VB;
        private FastColoredTextBoxNS.FastColoredTextBox fctRULE_VBS;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Button cmdDecode;
        private System.Windows.Forms.TextBox txtConstring;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.TextBox txtSQL;
        private System.Windows.Forms.TextBox txtRuleID;
        private System.Windows.Forms.TextBox txtRuleName;
        private System.Windows.Forms.TextBox txtRuleDescription;
        private System.Windows.Forms.ComboBox cmbRuleStatus;
        private System.Windows.Forms.ComboBox cmbApprovedStatus;
        private System.Windows.Forms.ComboBox cmbRuleType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel pnlParserCMDGroup;
        private System.Windows.Forms.CheckBox chkAutoParse;
        private System.Windows.Forms.Button cmdPtoVB;
        private System.Windows.Forms.Button cmdTestRule;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.TextBox txtTop;
        private System.Windows.Forms.Button cmdSearch;
        private System.Windows.Forms.Button cmdBuildDll;
        private System.Windows.Forms.Button cmdCompileRule;
    }
}