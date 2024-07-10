namespace Manual_test_app.LinQ
{
    partial class LinQPad
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LinQPad));
            this.txtRS = new System.Windows.Forms.TextBox();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.cmdRunQuery = new System.Windows.Forms.Button();
            this.label21 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.cmdRunLinq = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tslConnectionString = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslCS = new System.Windows.Forms.ToolStripStatusLabel();
            this.txtSQL = new FastColoredTextBoxNS.FastColoredTextBox();
            this.txtLINQ = new FastColoredTextBoxNS.FastColoredTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSQL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLINQ)).BeginInit();
            this.SuspendLayout();
            // 
            // txtRS
            // 
            this.txtRS.Location = new System.Drawing.Point(1048, 28);
            this.txtRS.Name = "txtRS";
            this.txtRS.Size = new System.Drawing.Size(36, 20);
            this.txtRS.TabIndex = 90;
            this.txtRS.Text = "0";
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(9, 83);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(1075, 198);
            this.dataGridView2.TabIndex = 86;
            // 
            // cmdRunQuery
            // 
            this.cmdRunQuery.Location = new System.Drawing.Point(1009, 54);
            this.cmdRunQuery.Name = "cmdRunQuery";
            this.cmdRunQuery.Size = new System.Drawing.Size(75, 23);
            this.cmdRunQuery.TabIndex = 85;
            this.cmdRunQuery.Text = "Run Query";
            this.cmdRunQuery.UseVisualStyleBackColor = true;
            this.cmdRunQuery.Click += new System.EventHandler(this.cmdRunQuery_Click);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(987, 31);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(56, 13);
            this.label21.TabIndex = 89;
            this.label21.Text = "Result Set";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(8, 370);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1075, 198);
            this.dataGridView1.TabIndex = 92;
            // 
            // cmdRunLinq
            // 
            this.cmdRunLinq.Location = new System.Drawing.Point(1008, 341);
            this.cmdRunLinq.Name = "cmdRunLinq";
            this.cmdRunLinq.Size = new System.Drawing.Size(75, 23);
            this.cmdRunLinq.TabIndex = 91;
            this.cmdRunLinq.Text = "Linq";
            this.cmdRunLinq.UseVisualStyleBackColor = true;
            this.cmdRunLinq.Click += new System.EventHandler(this.cmdRunLinq_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslConnectionString,
            this.tslCS});
            this.statusStrip1.Location = new System.Drawing.Point(0, 625);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1095, 22);
            this.statusStrip1.TabIndex = 94;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tslConnectionString
            // 
            this.tslConnectionString.Name = "tslConnectionString";
            this.tslConnectionString.Size = new System.Drawing.Size(139, 17);
            this.tslConnectionString.Text = "Current Connection String: ";
            // 
            // tslCS
            // 
            this.tslCS.Name = "tslCS";
            this.tslCS.Size = new System.Drawing.Size(109, 17);
            this.tslCS.Text = "toolStripStatusLabel1";
            // 
            // txtSQL
            // 
            this.txtSQL.AutoCompleteBracketsList = new char[] {
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
            this.txtSQL.AutoIndentCharsPatterns = "\r\n^\\s*[\\w\\.\\(\\)]+\\s*(?<range>=)\\s*(?<range>.+)\r\n";
            this.txtSQL.AutoScrollMinSize = new System.Drawing.Size(27, 14);
            this.txtSQL.BackBrush = null;
            this.txtSQL.CharHeight = 14;
            this.txtSQL.CharWidth = 8;
            this.txtSQL.CommentPrefix = "\'";
            this.txtSQL.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSQL.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.txtSQL.IsReplaceMode = false;
            this.txtSQL.Language = FastColoredTextBoxNS.Language.SQL;
            this.txtSQL.LeftBracket = '(';
            this.txtSQL.Location = new System.Drawing.Point(8, 12);
            this.txtSQL.Name = "txtSQL";
            this.txtSQL.Paddings = new System.Windows.Forms.Padding(0);
            this.txtSQL.RightBracket = ')';
            this.txtSQL.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtSQL.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("txtSQL.ServiceColors")));
            this.txtSQL.Size = new System.Drawing.Size(973, 65);
            this.txtSQL.TabIndex = 95;
            this.txtSQL.Zoom = 100;
            // 
            // txtLINQ
            // 
            this.txtLINQ.AutoCompleteBracketsList = new char[] {
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
            this.txtLINQ.AutoIndentCharsPatterns = "\r\n^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;]+);\r\n^\\s*(case|default)\\s*[^:]" +
    "*(?<range>:)\\s*(?<range>[^;]+);\r\n";
            this.txtLINQ.AutoScrollMinSize = new System.Drawing.Size(27, 14);
            this.txtLINQ.BackBrush = null;
            this.txtLINQ.BracketsHighlightStrategy = FastColoredTextBoxNS.BracketsHighlightStrategy.Strategy2;
            this.txtLINQ.CharHeight = 14;
            this.txtLINQ.CharWidth = 8;
            this.txtLINQ.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtLINQ.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.txtLINQ.IsReplaceMode = false;
            this.txtLINQ.Language = FastColoredTextBoxNS.Language.CSharp;
            this.txtLINQ.LeftBracket = '(';
            this.txtLINQ.LeftBracket2 = '{';
            this.txtLINQ.Location = new System.Drawing.Point(8, 293);
            this.txtLINQ.Name = "txtLINQ";
            this.txtLINQ.Paddings = new System.Windows.Forms.Padding(0);
            this.txtLINQ.RightBracket = ')';
            this.txtLINQ.RightBracket2 = '}';
            this.txtLINQ.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtLINQ.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("txtLINQ.ServiceColors")));
            this.txtLINQ.Size = new System.Drawing.Size(994, 71);
            this.txtLINQ.TabIndex = 96;
            this.txtLINQ.Zoom = 100;
            // 
            // LinQPad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1095, 647);
            this.Controls.Add(this.txtLINQ);
            this.Controls.Add(this.txtSQL);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.cmdRunLinq);
            this.Controls.Add(this.txtRS);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.cmdRunQuery);
            this.Controls.Add(this.label21);
            this.Name = "LinQPad";
            this.Text = "LinQPad";
            this.Load += new System.EventHandler(this.LinQPad_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSQL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLINQ)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtRS;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button cmdRunQuery;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button cmdRunLinq;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tslConnectionString;
        private System.Windows.Forms.ToolStripStatusLabel tslCS;
        private FastColoredTextBoxNS.FastColoredTextBox txtSQL;
        private FastColoredTextBoxNS.FastColoredTextBox txtLINQ;

    }
}