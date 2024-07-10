namespace Manual_test_app.Rules
{
    partial class BuildDllStringOutput
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BuildDllStringOutput));
            this.fctRULE_VB = new FastColoredTextBoxNS.FastColoredTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.fctRULE_VB)).BeginInit();
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
            this.fctRULE_VB.Location = new System.Drawing.Point(86, 12);
            this.fctRULE_VB.Name = "fctRULE_VB";
            this.fctRULE_VB.Paddings = new System.Windows.Forms.Padding(0);
            this.fctRULE_VB.RightBracket = ')';
            this.fctRULE_VB.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fctRULE_VB.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctRULE_VB.ServiceColors")));
            this.fctRULE_VB.Size = new System.Drawing.Size(999, 877);
            this.fctRULE_VB.TabIndex = 91;
            this.fctRULE_VB.Text = "Paste or select File::Open";
            this.fctRULE_VB.WordWrap = true;
            this.fctRULE_VB.Zoom = 100;
            // 
            // BuildDllStringOutput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1097, 911);
            this.Controls.Add(this.fctRULE_VB);
            this.Name = "BuildDllStringOutput";
            this.Text = "BuildDllStringOutput";
            this.Load += new System.EventHandler(this.BuildDllStringOutput_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fctRULE_VB)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private FastColoredTextBoxNS.FastColoredTextBox fctRULE_VB;
    }
}