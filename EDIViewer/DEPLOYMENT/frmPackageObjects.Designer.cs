namespace Manual_test_app.DEPLOYMENT
{
    partial class frmPackageObjects
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
            this.txtSource = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabTABLES = new System.Windows.Forms.TabPage();
            this.cmdTableRemoveAll = new System.Windows.Forms.Button();
            this.cmdTableAddAll = new System.Windows.Forms.Button();
            this.chkSourceTablesStartsWith = new System.Windows.Forms.CheckBox();
            this.cmdTableSearch = new System.Windows.Forms.Button();
            this.txtSearchTable = new System.Windows.Forms.TextBox();
            this.cmdTableRemove = new System.Windows.Forms.Button();
            this.cmdTableAdd = new System.Windows.Forms.Button();
            this.lstTargetTable = new System.Windows.Forms.ListBox();
            this.lstSourceTable = new System.Windows.Forms.ListBox();
            this.tabSP = new System.Windows.Forms.TabPage();
            this.cmdRemoveSPALL = new System.Windows.Forms.Button();
            this.cmdSPAddAll = new System.Windows.Forms.Button();
            this.chkSourceSPStartsWith = new System.Windows.Forms.CheckBox();
            this.cmdSPSearch = new System.Windows.Forms.Button();
            this.txtSearchSP = new System.Windows.Forms.TextBox();
            this.cmdRemoveSP = new System.Windows.Forms.Button();
            this.cmdSPAdd = new System.Windows.Forms.Button();
            this.lstTargetSP = new System.Windows.Forms.ListBox();
            this.lstSourceSP = new System.Windows.Forms.ListBox();
            this.tabUTYPES = new System.Windows.Forms.TabPage();
            this.cmdUTYPERemoveAll = new System.Windows.Forms.Button();
            this.cmdUTYPEAddAll = new System.Windows.Forms.Button();
            this.chkSourceUTYPEStartsWith = new System.Windows.Forms.CheckBox();
            this.cmdUTYPESearch = new System.Windows.Forms.Button();
            this.txtSearchUTYPE = new System.Windows.Forms.TextBox();
            this.cmdUTYPERemove = new System.Windows.Forms.Button();
            this.cmdUTYPEAdd = new System.Windows.Forms.Button();
            this.lstTargetUTYPE = new System.Windows.Forms.ListBox();
            this.lstSourceUTYPE = new System.Windows.Forms.ListBox();
            this.tabVIEWS = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button5 = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.tabINDEXES = new System.Windows.Forms.TabPage();
            this.tabTRIGERS = new System.Windows.Forms.TabPage();
            this.tabRELATEDOBJECTS = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.cmdSourceDecode = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tspMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.cmdSavePackage = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblPackage = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabTABLES.SuspendLayout();
            this.tabSP.SuspendLayout();
            this.tabUTYPES.SuspendLayout();
            this.tabVIEWS.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSource
            // 
            this.txtSource.Location = new System.Drawing.Point(99, 610);
            this.txtSource.Name = "txtSource";
            this.txtSource.Size = new System.Drawing.Size(572, 20);
            this.txtSource.TabIndex = 5;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabTABLES);
            this.tabControl1.Controls.Add(this.tabSP);
            this.tabControl1.Controls.Add(this.tabUTYPES);
            this.tabControl1.Controls.Add(this.tabVIEWS);
            this.tabControl1.Controls.Add(this.tabINDEXES);
            this.tabControl1.Controls.Add(this.tabTRIGERS);
            this.tabControl1.Controls.Add(this.tabRELATEDOBJECTS);
            this.tabControl1.Location = new System.Drawing.Point(12, 34);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(786, 527);
            this.tabControl1.TabIndex = 6;
            // 
            // tabTABLES
            // 
            this.tabTABLES.Controls.Add(this.cmdTableRemoveAll);
            this.tabTABLES.Controls.Add(this.cmdTableAddAll);
            this.tabTABLES.Controls.Add(this.chkSourceTablesStartsWith);
            this.tabTABLES.Controls.Add(this.cmdTableSearch);
            this.tabTABLES.Controls.Add(this.txtSearchTable);
            this.tabTABLES.Controls.Add(this.cmdTableRemove);
            this.tabTABLES.Controls.Add(this.cmdTableAdd);
            this.tabTABLES.Controls.Add(this.lstTargetTable);
            this.tabTABLES.Controls.Add(this.lstSourceTable);
            this.tabTABLES.Location = new System.Drawing.Point(4, 22);
            this.tabTABLES.Name = "tabTABLES";
            this.tabTABLES.Padding = new System.Windows.Forms.Padding(3);
            this.tabTABLES.Size = new System.Drawing.Size(778, 501);
            this.tabTABLES.TabIndex = 0;
            this.tabTABLES.Text = "TABLES";
            this.tabTABLES.UseVisualStyleBackColor = true;
            // 
            // cmdTableRemoveAll
            // 
            this.cmdTableRemoveAll.Location = new System.Drawing.Point(343, 248);
            this.cmdTableRemoveAll.Name = "cmdTableRemoveAll";
            this.cmdTableRemoveAll.Size = new System.Drawing.Size(75, 23);
            this.cmdTableRemoveAll.TabIndex = 12;
            this.cmdTableRemoveAll.Text = "<<";
            this.cmdTableRemoveAll.UseVisualStyleBackColor = true;
            this.cmdTableRemoveAll.Click += new System.EventHandler(this.cmdTableRemoveAll_Click);
            // 
            // cmdTableAddAll
            // 
            this.cmdTableAddAll.Location = new System.Drawing.Point(343, 151);
            this.cmdTableAddAll.Name = "cmdTableAddAll";
            this.cmdTableAddAll.Size = new System.Drawing.Size(75, 23);
            this.cmdTableAddAll.TabIndex = 11;
            this.cmdTableAddAll.Text = ">>";
            this.cmdTableAddAll.UseVisualStyleBackColor = true;
            this.cmdTableAddAll.Click += new System.EventHandler(this.cmdTableAddAll_Click);
            // 
            // chkSourceTablesStartsWith
            // 
            this.chkSourceTablesStartsWith.AutoSize = true;
            this.chkSourceTablesStartsWith.Checked = true;
            this.chkSourceTablesStartsWith.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSourceTablesStartsWith.Location = new System.Drawing.Point(63, 16);
            this.chkSourceTablesStartsWith.Name = "chkSourceTablesStartsWith";
            this.chkSourceTablesStartsWith.Size = new System.Drawing.Size(78, 17);
            this.chkSourceTablesStartsWith.TabIndex = 10;
            this.chkSourceTablesStartsWith.Text = "Starts With";
            this.chkSourceTablesStartsWith.UseVisualStyleBackColor = true;
            // 
            // cmdTableSearch
            // 
            this.cmdTableSearch.Location = new System.Drawing.Point(262, 37);
            this.cmdTableSearch.Name = "cmdTableSearch";
            this.cmdTableSearch.Size = new System.Drawing.Size(54, 25);
            this.cmdTableSearch.TabIndex = 9;
            this.cmdTableSearch.Text = "Search";
            this.cmdTableSearch.UseVisualStyleBackColor = true;
            this.cmdTableSearch.Click += new System.EventHandler(this.cmdTableSearch_Click);
            // 
            // txtSearchTable
            // 
            this.txtSearchTable.Location = new System.Drawing.Point(63, 39);
            this.txtSearchTable.Name = "txtSearchTable";
            this.txtSearchTable.Size = new System.Drawing.Size(193, 20);
            this.txtSearchTable.TabIndex = 8;
            // 
            // cmdTableRemove
            // 
            this.cmdTableRemove.Location = new System.Drawing.Point(343, 219);
            this.cmdTableRemove.Name = "cmdTableRemove";
            this.cmdTableRemove.Size = new System.Drawing.Size(75, 23);
            this.cmdTableRemove.TabIndex = 7;
            this.cmdTableRemove.Text = "<";
            this.cmdTableRemove.UseVisualStyleBackColor = true;
            this.cmdTableRemove.Click += new System.EventHandler(this.cmdTableRemove_Click);
            // 
            // cmdTableAdd
            // 
            this.cmdTableAdd.Location = new System.Drawing.Point(343, 180);
            this.cmdTableAdd.Name = "cmdTableAdd";
            this.cmdTableAdd.Size = new System.Drawing.Size(75, 23);
            this.cmdTableAdd.TabIndex = 6;
            this.cmdTableAdd.Text = ">";
            this.cmdTableAdd.UseVisualStyleBackColor = true;
            this.cmdTableAdd.Click += new System.EventHandler(this.cmdTableAdd_Click);
            // 
            // lstTargetTable
            // 
            this.lstTargetTable.FormattingEnabled = true;
            this.lstTargetTable.Location = new System.Drawing.Point(462, 66);
            this.lstTargetTable.Name = "lstTargetTable";
            this.lstTargetTable.Size = new System.Drawing.Size(253, 368);
            this.lstTargetTable.TabIndex = 5;
            // 
            // lstSourceTable
            // 
            this.lstSourceTable.FormattingEnabled = true;
            this.lstSourceTable.Location = new System.Drawing.Point(63, 66);
            this.lstSourceTable.Name = "lstSourceTable";
            this.lstSourceTable.Size = new System.Drawing.Size(253, 368);
            this.lstSourceTable.TabIndex = 4;
            // 
            // tabSP
            // 
            this.tabSP.Controls.Add(this.cmdRemoveSPALL);
            this.tabSP.Controls.Add(this.cmdSPAddAll);
            this.tabSP.Controls.Add(this.chkSourceSPStartsWith);
            this.tabSP.Controls.Add(this.cmdSPSearch);
            this.tabSP.Controls.Add(this.txtSearchSP);
            this.tabSP.Controls.Add(this.cmdRemoveSP);
            this.tabSP.Controls.Add(this.cmdSPAdd);
            this.tabSP.Controls.Add(this.lstTargetSP);
            this.tabSP.Controls.Add(this.lstSourceSP);
            this.tabSP.Location = new System.Drawing.Point(4, 22);
            this.tabSP.Name = "tabSP";
            this.tabSP.Padding = new System.Windows.Forms.Padding(3);
            this.tabSP.Size = new System.Drawing.Size(778, 501);
            this.tabSP.TabIndex = 1;
            this.tabSP.Text = "STORED PROCEDURES";
            this.tabSP.UseVisualStyleBackColor = true;
            // 
            // cmdRemoveSPALL
            // 
            this.cmdRemoveSPALL.Location = new System.Drawing.Point(343, 248);
            this.cmdRemoveSPALL.Name = "cmdRemoveSPALL";
            this.cmdRemoveSPALL.Size = new System.Drawing.Size(75, 23);
            this.cmdRemoveSPALL.TabIndex = 15;
            this.cmdRemoveSPALL.Text = "<<";
            this.cmdRemoveSPALL.UseVisualStyleBackColor = true;
            this.cmdRemoveSPALL.Click += new System.EventHandler(this.cmdRemoveSPALL_Click);
            // 
            // cmdSPAddAll
            // 
            this.cmdSPAddAll.Location = new System.Drawing.Point(343, 151);
            this.cmdSPAddAll.Name = "cmdSPAddAll";
            this.cmdSPAddAll.Size = new System.Drawing.Size(75, 23);
            this.cmdSPAddAll.TabIndex = 14;
            this.cmdSPAddAll.Text = ">>";
            this.cmdSPAddAll.UseVisualStyleBackColor = true;
            this.cmdSPAddAll.Click += new System.EventHandler(this.cmdSPAddAll_Click);
            // 
            // chkSourceSPStartsWith
            // 
            this.chkSourceSPStartsWith.AutoSize = true;
            this.chkSourceSPStartsWith.Checked = true;
            this.chkSourceSPStartsWith.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSourceSPStartsWith.Location = new System.Drawing.Point(63, 17);
            this.chkSourceSPStartsWith.Name = "chkSourceSPStartsWith";
            this.chkSourceSPStartsWith.Size = new System.Drawing.Size(78, 17);
            this.chkSourceSPStartsWith.TabIndex = 13;
            this.chkSourceSPStartsWith.Text = "Starts With";
            this.chkSourceSPStartsWith.UseVisualStyleBackColor = true;
            // 
            // cmdSPSearch
            // 
            this.cmdSPSearch.Location = new System.Drawing.Point(262, 38);
            this.cmdSPSearch.Name = "cmdSPSearch";
            this.cmdSPSearch.Size = new System.Drawing.Size(54, 25);
            this.cmdSPSearch.TabIndex = 12;
            this.cmdSPSearch.Text = "Search";
            this.cmdSPSearch.UseVisualStyleBackColor = true;
            this.cmdSPSearch.Click += new System.EventHandler(this.cmdSPSearch_Click);
            // 
            // txtSearchSP
            // 
            this.txtSearchSP.Location = new System.Drawing.Point(63, 40);
            this.txtSearchSP.Name = "txtSearchSP";
            this.txtSearchSP.Size = new System.Drawing.Size(193, 20);
            this.txtSearchSP.TabIndex = 11;
            // 
            // cmdRemoveSP
            // 
            this.cmdRemoveSP.Location = new System.Drawing.Point(343, 219);
            this.cmdRemoveSP.Name = "cmdRemoveSP";
            this.cmdRemoveSP.Size = new System.Drawing.Size(75, 23);
            this.cmdRemoveSP.TabIndex = 7;
            this.cmdRemoveSP.Text = "<";
            this.cmdRemoveSP.UseVisualStyleBackColor = true;
            this.cmdRemoveSP.Click += new System.EventHandler(this.cmdRemoveSP_Click);
            // 
            // cmdSPAdd
            // 
            this.cmdSPAdd.Location = new System.Drawing.Point(343, 180);
            this.cmdSPAdd.Name = "cmdSPAdd";
            this.cmdSPAdd.Size = new System.Drawing.Size(75, 23);
            this.cmdSPAdd.TabIndex = 6;
            this.cmdSPAdd.Text = ">";
            this.cmdSPAdd.UseVisualStyleBackColor = true;
            this.cmdSPAdd.Click += new System.EventHandler(this.cmdSPAdd_Click);
            // 
            // lstTargetSP
            // 
            this.lstTargetSP.FormattingEnabled = true;
            this.lstTargetSP.Location = new System.Drawing.Point(462, 66);
            this.lstTargetSP.Name = "lstTargetSP";
            this.lstTargetSP.Size = new System.Drawing.Size(253, 368);
            this.lstTargetSP.TabIndex = 5;
            // 
            // lstSourceSP
            // 
            this.lstSourceSP.FormattingEnabled = true;
            this.lstSourceSP.Location = new System.Drawing.Point(63, 66);
            this.lstSourceSP.Name = "lstSourceSP";
            this.lstSourceSP.Size = new System.Drawing.Size(253, 368);
            this.lstSourceSP.TabIndex = 4;
            // 
            // tabUTYPES
            // 
            this.tabUTYPES.Controls.Add(this.cmdUTYPERemoveAll);
            this.tabUTYPES.Controls.Add(this.cmdUTYPEAddAll);
            this.tabUTYPES.Controls.Add(this.chkSourceUTYPEStartsWith);
            this.tabUTYPES.Controls.Add(this.cmdUTYPESearch);
            this.tabUTYPES.Controls.Add(this.txtSearchUTYPE);
            this.tabUTYPES.Controls.Add(this.cmdUTYPERemove);
            this.tabUTYPES.Controls.Add(this.cmdUTYPEAdd);
            this.tabUTYPES.Controls.Add(this.lstTargetUTYPE);
            this.tabUTYPES.Controls.Add(this.lstSourceUTYPE);
            this.tabUTYPES.Location = new System.Drawing.Point(4, 22);
            this.tabUTYPES.Name = "tabUTYPES";
            this.tabUTYPES.Padding = new System.Windows.Forms.Padding(3);
            this.tabUTYPES.Size = new System.Drawing.Size(778, 501);
            this.tabUTYPES.TabIndex = 2;
            this.tabUTYPES.Text = "UTYPE TABLES";
            this.tabUTYPES.UseVisualStyleBackColor = true;
            // 
            // cmdUTYPERemoveAll
            // 
            this.cmdUTYPERemoveAll.Location = new System.Drawing.Point(343, 248);
            this.cmdUTYPERemoveAll.Name = "cmdUTYPERemoveAll";
            this.cmdUTYPERemoveAll.Size = new System.Drawing.Size(75, 23);
            this.cmdUTYPERemoveAll.TabIndex = 16;
            this.cmdUTYPERemoveAll.Text = "<<";
            this.cmdUTYPERemoveAll.UseVisualStyleBackColor = true;
            this.cmdUTYPERemoveAll.Click += new System.EventHandler(this.cmdUTYPERemoveAll_Click);
            // 
            // cmdUTYPEAddAll
            // 
            this.cmdUTYPEAddAll.Location = new System.Drawing.Point(343, 151);
            this.cmdUTYPEAddAll.Name = "cmdUTYPEAddAll";
            this.cmdUTYPEAddAll.Size = new System.Drawing.Size(75, 23);
            this.cmdUTYPEAddAll.TabIndex = 14;
            this.cmdUTYPEAddAll.Text = ">>";
            this.cmdUTYPEAddAll.UseVisualStyleBackColor = true;
            this.cmdUTYPEAddAll.Click += new System.EventHandler(this.cmdUTYPEAddAll_Click);
            // 
            // chkSourceUTYPEStartsWith
            // 
            this.chkSourceUTYPEStartsWith.AutoSize = true;
            this.chkSourceUTYPEStartsWith.Checked = true;
            this.chkSourceUTYPEStartsWith.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSourceUTYPEStartsWith.Location = new System.Drawing.Point(63, 17);
            this.chkSourceUTYPEStartsWith.Name = "chkSourceUTYPEStartsWith";
            this.chkSourceUTYPEStartsWith.Size = new System.Drawing.Size(78, 17);
            this.chkSourceUTYPEStartsWith.TabIndex = 13;
            this.chkSourceUTYPEStartsWith.Text = "Starts With";
            this.chkSourceUTYPEStartsWith.UseVisualStyleBackColor = true;
            // 
            // cmdUTYPESearch
            // 
            this.cmdUTYPESearch.Location = new System.Drawing.Point(262, 38);
            this.cmdUTYPESearch.Name = "cmdUTYPESearch";
            this.cmdUTYPESearch.Size = new System.Drawing.Size(54, 25);
            this.cmdUTYPESearch.TabIndex = 12;
            this.cmdUTYPESearch.Text = "Search";
            this.cmdUTYPESearch.UseVisualStyleBackColor = true;
            this.cmdUTYPESearch.Click += new System.EventHandler(this.cmdUTYPESearch_Click);
            // 
            // txtSearchUTYPE
            // 
            this.txtSearchUTYPE.Location = new System.Drawing.Point(63, 40);
            this.txtSearchUTYPE.Name = "txtSearchUTYPE";
            this.txtSearchUTYPE.Size = new System.Drawing.Size(193, 20);
            this.txtSearchUTYPE.TabIndex = 11;
            // 
            // cmdUTYPERemove
            // 
            this.cmdUTYPERemove.Location = new System.Drawing.Point(343, 219);
            this.cmdUTYPERemove.Name = "cmdUTYPERemove";
            this.cmdUTYPERemove.Size = new System.Drawing.Size(75, 23);
            this.cmdUTYPERemove.TabIndex = 7;
            this.cmdUTYPERemove.Text = "<";
            this.cmdUTYPERemove.UseVisualStyleBackColor = true;
            this.cmdUTYPERemove.Click += new System.EventHandler(this.cmdUTYPERemove_Click);
            // 
            // cmdUTYPEAdd
            // 
            this.cmdUTYPEAdd.Location = new System.Drawing.Point(343, 180);
            this.cmdUTYPEAdd.Name = "cmdUTYPEAdd";
            this.cmdUTYPEAdd.Size = new System.Drawing.Size(75, 23);
            this.cmdUTYPEAdd.TabIndex = 6;
            this.cmdUTYPEAdd.Text = ">";
            this.cmdUTYPEAdd.UseVisualStyleBackColor = true;
            this.cmdUTYPEAdd.Click += new System.EventHandler(this.cmdUTYPEAdd_Click);
            // 
            // lstTargetUTYPE
            // 
            this.lstTargetUTYPE.FormattingEnabled = true;
            this.lstTargetUTYPE.Location = new System.Drawing.Point(462, 66);
            this.lstTargetUTYPE.Name = "lstTargetUTYPE";
            this.lstTargetUTYPE.Size = new System.Drawing.Size(253, 368);
            this.lstTargetUTYPE.TabIndex = 5;
            // 
            // lstSourceUTYPE
            // 
            this.lstSourceUTYPE.FormattingEnabled = true;
            this.lstSourceUTYPE.Location = new System.Drawing.Point(63, 66);
            this.lstSourceUTYPE.Name = "lstSourceUTYPE";
            this.lstSourceUTYPE.Size = new System.Drawing.Size(253, 368);
            this.lstSourceUTYPE.TabIndex = 4;
            // 
            // tabVIEWS
            // 
            this.tabVIEWS.Controls.Add(this.button1);
            this.tabVIEWS.Controls.Add(this.button2);
            this.tabVIEWS.Controls.Add(this.checkBox1);
            this.tabVIEWS.Controls.Add(this.button5);
            this.tabVIEWS.Controls.Add(this.textBox2);
            this.tabVIEWS.Controls.Add(this.button6);
            this.tabVIEWS.Controls.Add(this.button8);
            this.tabVIEWS.Controls.Add(this.listBox1);
            this.tabVIEWS.Controls.Add(this.listBox2);
            this.tabVIEWS.Location = new System.Drawing.Point(4, 22);
            this.tabVIEWS.Name = "tabVIEWS";
            this.tabVIEWS.Size = new System.Drawing.Size(778, 501);
            this.tabVIEWS.TabIndex = 3;
            this.tabVIEWS.Text = "VIEWS";
            this.tabVIEWS.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(343, 247);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 21;
            this.button1.Text = "<<";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(343, 150);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 20;
            this.button2.Text = ">>";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(63, 15);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(78, 17);
            this.checkBox1.TabIndex = 19;
            this.checkBox1.Text = "Starts With";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(262, 36);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(54, 25);
            this.button5.TabIndex = 18;
            this.button5.Text = "Search";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(63, 38);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(193, 20);
            this.textBox2.TabIndex = 17;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(343, 218);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 16;
            this.button6.Text = "<";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(343, 179);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(75, 23);
            this.button8.TabIndex = 15;
            this.button8.Text = ">";
            this.button8.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(462, 65);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(253, 368);
            this.listBox1.TabIndex = 14;
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(63, 65);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(253, 368);
            this.listBox2.TabIndex = 13;
            // 
            // tabINDEXES
            // 
            this.tabINDEXES.Location = new System.Drawing.Point(4, 22);
            this.tabINDEXES.Name = "tabINDEXES";
            this.tabINDEXES.Size = new System.Drawing.Size(778, 501);
            this.tabINDEXES.TabIndex = 4;
            this.tabINDEXES.Text = "INDEXES";
            this.tabINDEXES.UseVisualStyleBackColor = true;
            // 
            // tabTRIGERS
            // 
            this.tabTRIGERS.Location = new System.Drawing.Point(4, 22);
            this.tabTRIGERS.Name = "tabTRIGERS";
            this.tabTRIGERS.Size = new System.Drawing.Size(778, 501);
            this.tabTRIGERS.TabIndex = 6;
            this.tabTRIGERS.Text = "TRIGERS";
            this.tabTRIGERS.UseVisualStyleBackColor = true;
            // 
            // tabRELATEDOBJECTS
            // 
            this.tabRELATEDOBJECTS.Location = new System.Drawing.Point(4, 22);
            this.tabRELATEDOBJECTS.Name = "tabRELATEDOBJECTS";
            this.tabRELATEDOBJECTS.Size = new System.Drawing.Size(778, 501);
            this.tabRELATEDOBJECTS.TabIndex = 5;
            this.tabRELATEDOBJECTS.Text = "RELATED OBJECTS";
            this.tabRELATEDOBJECTS.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(52, 613);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Source";
            // 
            // cmdSourceDecode
            // 
            this.cmdSourceDecode.Location = new System.Drawing.Point(724, 608);
            this.cmdSourceDecode.Name = "cmdSourceDecode";
            this.cmdSourceDecode.Size = new System.Drawing.Size(75, 23);
            this.cmdSourceDecode.TabIndex = 10;
            this.cmdSourceDecode.Text = "Decode";
            this.cmdSourceDecode.UseVisualStyleBackColor = true;
            this.cmdSourceDecode.Click += new System.EventHandler(this.cmdSourceDecode_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tspMessage});
            this.statusStrip1.Location = new System.Drawing.Point(0, 643);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(815, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tspMessage
            // 
            this.tspMessage.Name = "tspMessage";
            this.tspMessage.Size = new System.Drawing.Size(109, 17);
            this.tspMessage.Text = "toolStripStatusLabel1";
            // 
            // cmdSavePackage
            // 
            this.cmdSavePackage.Location = new System.Drawing.Point(677, 567);
            this.cmdSavePackage.Name = "cmdSavePackage";
            this.cmdSavePackage.Size = new System.Drawing.Size(122, 23);
            this.cmdSavePackage.TabIndex = 12;
            this.cmdSavePackage.Text = "Save Package";
            this.cmdSavePackage.UseVisualStyleBackColor = true;
            this.cmdSavePackage.Click += new System.EventHandler(this.cmdSavePackage_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(99, 567);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(572, 20);
            this.textBox1.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 570);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Package Name";
            // 
            // lblPackage
            // 
            this.lblPackage.AutoSize = true;
            this.lblPackage.Location = new System.Drawing.Point(16, 11);
            this.lblPackage.Name = "lblPackage";
            this.lblPackage.Size = new System.Drawing.Size(0, 13);
            this.lblPackage.TabIndex = 15;
            // 
            // frmPackageObjects
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(815, 665);
            this.Controls.Add(this.lblPackage);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.cmdSavePackage);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.cmdSourceDecode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.txtSource);
            this.Name = "frmPackageObjects";
            this.Text = "frmDBscan";
            this.Load += new System.EventHandler(this.frmDBscan_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabTABLES.ResumeLayout(false);
            this.tabTABLES.PerformLayout();
            this.tabSP.ResumeLayout(false);
            this.tabSP.PerformLayout();
            this.tabUTYPES.ResumeLayout(false);
            this.tabUTYPES.PerformLayout();
            this.tabVIEWS.ResumeLayout(false);
            this.tabVIEWS.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabTABLES;
        private System.Windows.Forms.TextBox txtSearchTable;
        private System.Windows.Forms.Button cmdTableRemove;
        private System.Windows.Forms.Button cmdTableAdd;
        private System.Windows.Forms.ListBox lstTargetTable;
        private System.Windows.Forms.ListBox lstSourceTable;
        private System.Windows.Forms.TabPage tabSP;
        private System.Windows.Forms.Button cmdRemoveSP;
        private System.Windows.Forms.Button cmdSPAdd;
        private System.Windows.Forms.ListBox lstTargetSP;
        private System.Windows.Forms.ListBox lstSourceSP;
        private System.Windows.Forms.TabPage tabUTYPES;
        private System.Windows.Forms.Button cmdUTYPERemove;
        private System.Windows.Forms.Button cmdUTYPEAdd;
        private System.Windows.Forms.ListBox lstTargetUTYPE;
        private System.Windows.Forms.ListBox lstSourceUTYPE;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button cmdSourceDecode;
        private System.Windows.Forms.Button cmdTableSearch;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.CheckBox chkSourceTablesStartsWith;
        private System.Windows.Forms.CheckBox chkSourceSPStartsWith;
        private System.Windows.Forms.Button cmdSPSearch;
        private System.Windows.Forms.TextBox txtSearchSP;
        private System.Windows.Forms.CheckBox chkSourceUTYPEStartsWith;
        private System.Windows.Forms.Button cmdUTYPESearch;
        private System.Windows.Forms.TextBox txtSearchUTYPE;
        private System.Windows.Forms.Button cmdTableAddAll;
        private System.Windows.Forms.Button cmdSPAddAll;
        private System.Windows.Forms.Button cmdUTYPEAddAll;
        private System.Windows.Forms.Button cmdSavePackage;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripStatusLabel tspMessage;
        private System.Windows.Forms.Button cmdTableRemoveAll;
        private System.Windows.Forms.Button cmdRemoveSPALL;
        private System.Windows.Forms.TabPage tabVIEWS;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.Button cmdUTYPERemoveAll;
        private System.Windows.Forms.TabPage tabINDEXES;
        private System.Windows.Forms.TabPage tabTRIGERS;
        private System.Windows.Forms.TabPage tabRELATEDOBJECTS;
        private System.Windows.Forms.Label lblPackage;

    }
}