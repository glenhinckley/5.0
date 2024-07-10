namespace Manual_test_app
{
    partial class frmEDI5010
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEDI5010));
            this.SBar = new System.Windows.Forms.StatusStrip();
            this.tslState = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslTSIC = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslCR = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslPARSERSTATUS = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbValidate = new System.Windows.Forms.ToolStripButton();
            this.tsbParse = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbReadale = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabView = new System.Windows.Forms.TabControl();
            this.tbpRaw = new System.Windows.Forms.TabPage();
            this.fctEDI = new FastColoredTextBoxNS.FastColoredTextBox();
            this.tbpList = new System.Windows.Forms.TabPage();
            this.lstList = new System.Windows.Forms.ListView();
            this.tpbDataSegment = new System.Windows.Forms.TabPage();
            this.txtConstring = new System.Windows.Forms.TextBox();
            this.cmdDecode = new System.Windows.Forms.Button();
            this.tbpEDI = new System.Windows.Forms.TabControl();
            this.tab270 = new System.Windows.Forms.TabPage();
            this.cmdGetREQ = new System.Windows.Forms.Button();
            this.chkGetRES = new System.Windows.Forms.CheckBox();
            this.cmdPop271 = new System.Windows.Forms.Button();
            this.lblReturnCode = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtREQReturnBatchID = new System.Windows.Forms.TextBox();
            this.txt270BatchID = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txt270DeleteFlag = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txt270Source = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.chk270Use5010 = new System.Windows.Forms.CheckBox();
            this.cmdImport270 = new System.Windows.Forms.Button();
            this.txt270PatHospCode = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txt270AccountNumber = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txt270InsType = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txt270VendorName = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txt270PayorID = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.txt270HospCode = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.txt270UserId = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.txt270EBRID = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.cmdParse270 = new System.Windows.Forms.Button();
            this.tab271 = new System.Windows.Forms.TabPage();
            this.txt271DeadLockRetryCount = new System.Windows.Forms.TextBox();
            this.label36 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label30 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.cmdLegacyImport271 = new System.Windows.Forms.Button();
            this.cmdLegacyParse271 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label27 = new System.Windows.Forms.Label();
            this.lbl271ReturnCode = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.chk271Use5010 = new System.Windows.Forms.CheckBox();
            this.cmd271Import = new System.Windows.Forms.Button();
            this.cmd271Parse = new System.Windows.Forms.Button();
            this.chk271LogEDI = new System.Windows.Forms.CheckBox();
            this.chk271DeleteFlag = new System.Windows.Forms.CheckBox();
            this.txt271GlobalBatchID = new System.Windows.Forms.TextBox();
            this.lbl271BatchID = new System.Windows.Forms.Label();
            this.txt271Source = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.txt271VendorName = new System.Windows.Forms.TextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.txt271PayorID = new System.Windows.Forms.TextBox();
            this.label32 = new System.Windows.Forms.Label();
            this.txt271HospCode = new System.Windows.Forms.TextBox();
            this.label33 = new System.Windows.Forms.Label();
            this.txt271UserId = new System.Windows.Forms.TextBox();
            this.label34 = new System.Windows.Forms.Label();
            this.txt271EBRID = new System.Windows.Forms.TextBox();
            this.label35 = new System.Windows.Forms.Label();
            this.tab276 = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.cmdImport276 = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.lbl276_5010ImportReturnCode = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.cmd276_5010Import = new System.Windows.Forms.Button();
            this.cmd5010Parse276 = new System.Windows.Forms.Button();
            this.txt276DeletFlag = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtPatHouseCode = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtAccountNumber = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtInsType = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtVendorName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtPayerCode = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtHospCode = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUserId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEbrID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt276BatchID = new System.Windows.Forms.TextBox();
            this.tab277 = new System.Windows.Forms.TabPage();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.lbl277_5010ImportReturnCode = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.cmd277_5010Import = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.txt277DeleteFlag = new System.Windows.Forms.TextBox();
            this.label42 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label44 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label45 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label46 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label47 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label48 = new System.Windows.Forms.Label();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.label49 = new System.Windows.Forms.Label();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.label50 = new System.Windows.Forms.Label();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.label51 = new System.Windows.Forms.Label();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.label52 = new System.Windows.Forms.Label();
            this.txt277BatchID = new System.Windows.Forms.TextBox();
            this.tab835 = new System.Windows.Forms.TabPage();
            this.tab837 = new System.Windows.Forms.TabPage();
            this.txt837BatchID = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.SBar.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.tabView.SuspendLayout();
            this.tbpRaw.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fctEDI)).BeginInit();
            this.tbpList.SuspendLayout();
            this.tbpEDI.SuspendLayout();
            this.tab270.SuspendLayout();
            this.tab271.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tab276.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tab277.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tab837.SuspendLayout();
            this.SuspendLayout();
            // 
            // SBar
            // 
            this.SBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslState,
            this.tslTSIC,
            this.tslCR,
            this.tslPARSERSTATUS});
            this.SBar.Location = new System.Drawing.Point(0, 754);
            this.SBar.Name = "SBar";
            this.SBar.Size = new System.Drawing.Size(1268, 22);
            this.SBar.TabIndex = 3;
            this.SBar.Text = "statusStrip1";
            // 
            // tslState
            // 
            this.tslState.Name = "tslState";
            this.tslState.Size = new System.Drawing.Size(38, 17);
            this.tslState.Text = "Ready";
            // 
            // tslTSIC
            // 
            this.tslTSIC.Name = "tslTSIC";
            this.tslTSIC.Size = new System.Drawing.Size(25, 17);
            this.tslTSIC.Text = "xxx";
            // 
            // tslCR
            // 
            this.tslCR.Name = "tslCR";
            this.tslCR.Size = new System.Drawing.Size(37, 17);
            this.tslCR.Text = "x0000";
            // 
            // tslPARSERSTATUS
            // 
            this.tslPARSERSTATUS.Name = "tslPARSERSTATUS";
            this.tslPARSERSTATUS.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbValidate,
            this.tsbParse,
            this.toolStripSeparator1,
            this.tsbReadale,
            this.toolStripSeparator2,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1268, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbValidate
            // 
            this.tsbValidate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbValidate.Image = ((System.Drawing.Image)(resources.GetObject("tsbValidate.Image")));
            this.tsbValidate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbValidate.Name = "tsbValidate";
            this.tsbValidate.Size = new System.Drawing.Size(49, 22);
            this.tsbValidate.Text = "Validate";
            this.tsbValidate.ToolTipText = "Validate";
            this.tsbValidate.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // tsbParse
            // 
            this.tsbParse.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbParse.Image = ((System.Drawing.Image)(resources.GetObject("tsbParse.Image")));
            this.tsbParse.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbParse.Name = "tsbParse";
            this.tsbParse.Size = new System.Drawing.Size(38, 22);
            this.tsbParse.Text = "Parse";
            this.tsbParse.Click += new System.EventHandler(this.tsbParse_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbReadale
            // 
            this.tsbReadale.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbReadale.Image = ((System.Drawing.Image)(resources.GetObject("tsbReadale.Image")));
            this.tsbReadale.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbReadale.Name = "tsbReadale";
            this.tsbReadale.Size = new System.Drawing.Size(27, 22);
            this.tsbReadale.Text = "~\\r";
            this.tsbReadale.Click += new System.EventHandler(this.tsbReadale_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click_1);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1268, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // tabView
            // 
            this.tabView.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabView.Controls.Add(this.tbpRaw);
            this.tabView.Controls.Add(this.tbpList);
            this.tabView.Controls.Add(this.tpbDataSegment);
            this.tabView.Location = new System.Drawing.Point(303, 55);
            this.tabView.Name = "tabView";
            this.tabView.SelectedIndex = 0;
            this.tabView.Size = new System.Drawing.Size(952, 648);
            this.tabView.TabIndex = 7;
            // 
            // tbpRaw
            // 
            this.tbpRaw.Controls.Add(this.fctEDI);
            this.tbpRaw.Location = new System.Drawing.Point(4, 4);
            this.tbpRaw.Name = "tbpRaw";
            this.tbpRaw.Padding = new System.Windows.Forms.Padding(3);
            this.tbpRaw.Size = new System.Drawing.Size(944, 622);
            this.tbpRaw.TabIndex = 0;
            this.tbpRaw.Text = "Raw";
            this.tbpRaw.UseVisualStyleBackColor = true;
            // 
            // fctEDI
            // 
            this.fctEDI.AutoCompleteBracketsList = new char[] {
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
            this.fctEDI.AutoScrollMinSize = new System.Drawing.Size(235, 14);
            this.fctEDI.BackBrush = null;
            this.fctEDI.CharHeight = 14;
            this.fctEDI.CharWidth = 8;
            this.fctEDI.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fctEDI.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fctEDI.IsReplaceMode = false;
            this.fctEDI.Location = new System.Drawing.Point(6, 3);
            this.fctEDI.Name = "fctEDI";
            this.fctEDI.Paddings = new System.Windows.Forms.Padding(0);
            this.fctEDI.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fctEDI.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("fctEDI.ServiceColors")));
            this.fctEDI.Size = new System.Drawing.Size(932, 616);
            this.fctEDI.TabIndex = 8;
            this.fctEDI.Text = "Paste or select File::Open";
            this.fctEDI.Zoom = 100;
            // 
            // tbpList
            // 
            this.tbpList.Controls.Add(this.lstList);
            this.tbpList.Location = new System.Drawing.Point(4, 4);
            this.tbpList.Name = "tbpList";
            this.tbpList.Padding = new System.Windows.Forms.Padding(3);
            this.tbpList.Size = new System.Drawing.Size(944, 622);
            this.tbpList.TabIndex = 1;
            this.tbpList.Text = "List";
            this.tbpList.UseVisualStyleBackColor = true;
            // 
            // lstList
            // 
            this.lstList.Location = new System.Drawing.Point(0, 0);
            this.lstList.Name = "lstList";
            this.lstList.Size = new System.Drawing.Size(948, 608);
            this.lstList.TabIndex = 0;
            this.lstList.UseCompatibleStateImageBehavior = false;
            // 
            // tpbDataSegment
            // 
            this.tpbDataSegment.Location = new System.Drawing.Point(4, 4);
            this.tpbDataSegment.Name = "tpbDataSegment";
            this.tpbDataSegment.Size = new System.Drawing.Size(944, 622);
            this.tpbDataSegment.TabIndex = 2;
            this.tpbDataSegment.Text = "Selected Segment";
            this.tpbDataSegment.UseVisualStyleBackColor = true;
            // 
            // txtConstring
            // 
            this.txtConstring.Location = new System.Drawing.Point(300, 709);
            this.txtConstring.Name = "txtConstring";
            this.txtConstring.Size = new System.Drawing.Size(941, 20);
            this.txtConstring.TabIndex = 8;
            this.txtConstring.TextChanged += new System.EventHandler(this.txtConstring_TextChanged);
            // 
            // cmdDecode
            // 
            this.cmdDecode.Location = new System.Drawing.Point(219, 706);
            this.cmdDecode.Name = "cmdDecode";
            this.cmdDecode.Size = new System.Drawing.Size(75, 23);
            this.cmdDecode.TabIndex = 9;
            this.cmdDecode.Text = "Decode";
            this.cmdDecode.UseVisualStyleBackColor = true;
            this.cmdDecode.Click += new System.EventHandler(this.cmdDecode_Click);
            // 
            // tbpEDI
            // 
            this.tbpEDI.Alignment = System.Windows.Forms.TabAlignment.Left;
            this.tbpEDI.Controls.Add(this.tab270);
            this.tbpEDI.Controls.Add(this.tab271);
            this.tbpEDI.Controls.Add(this.tab276);
            this.tbpEDI.Controls.Add(this.tab277);
            this.tbpEDI.Controls.Add(this.tab835);
            this.tbpEDI.Controls.Add(this.tab837);
            this.tbpEDI.Location = new System.Drawing.Point(11, 55);
            this.tbpEDI.Multiline = true;
            this.tbpEDI.Name = "tbpEDI";
            this.tbpEDI.SelectedIndex = 0;
            this.tbpEDI.Size = new System.Drawing.Size(290, 648);
            this.tbpEDI.TabIndex = 25;
            // 
            // tab270
            // 
            this.tab270.Controls.Add(this.cmdGetREQ);
            this.tab270.Controls.Add(this.chkGetRES);
            this.tab270.Controls.Add(this.cmdPop271);
            this.tab270.Controls.Add(this.lblReturnCode);
            this.tab270.Controls.Add(this.label21);
            this.tab270.Controls.Add(this.label1);
            this.tab270.Controls.Add(this.txtREQReturnBatchID);
            this.tab270.Controls.Add(this.txt270BatchID);
            this.tab270.Controls.Add(this.label15);
            this.tab270.Controls.Add(this.txt270DeleteFlag);
            this.tab270.Controls.Add(this.label14);
            this.tab270.Controls.Add(this.txt270Source);
            this.tab270.Controls.Add(this.label29);
            this.tab270.Controls.Add(this.chk270Use5010);
            this.tab270.Controls.Add(this.cmdImport270);
            this.tab270.Controls.Add(this.txt270PatHospCode);
            this.tab270.Controls.Add(this.label16);
            this.tab270.Controls.Add(this.txt270AccountNumber);
            this.tab270.Controls.Add(this.label17);
            this.tab270.Controls.Add(this.txt270InsType);
            this.tab270.Controls.Add(this.label18);
            this.tab270.Controls.Add(this.txt270VendorName);
            this.tab270.Controls.Add(this.label19);
            this.tab270.Controls.Add(this.txt270PayorID);
            this.tab270.Controls.Add(this.label20);
            this.tab270.Controls.Add(this.txt270HospCode);
            this.tab270.Controls.Add(this.label22);
            this.tab270.Controls.Add(this.txt270UserId);
            this.tab270.Controls.Add(this.label23);
            this.tab270.Controls.Add(this.txt270EBRID);
            this.tab270.Controls.Add(this.label24);
            this.tab270.Controls.Add(this.cmdParse270);
            this.tab270.Location = new System.Drawing.Point(23, 4);
            this.tab270.Name = "tab270";
            this.tab270.Padding = new System.Windows.Forms.Padding(3);
            this.tab270.Size = new System.Drawing.Size(263, 640);
            this.tab270.TabIndex = 5;
            this.tab270.Text = "5010x270";
            this.tab270.UseVisualStyleBackColor = true;
            this.tab270.Click += new System.EventHandler(this.tab270_Click);
            // 
            // cmdGetREQ
            // 
            this.cmdGetREQ.Location = new System.Drawing.Point(23, 320);
            this.cmdGetREQ.Name = "cmdGetREQ";
            this.cmdGetREQ.Size = new System.Drawing.Size(75, 23);
            this.cmdGetREQ.TabIndex = 93;
            this.cmdGetREQ.Text = "Get REQ";
            this.cmdGetREQ.UseVisualStyleBackColor = true;
            // 
            // chkGetRES
            // 
            this.chkGetRES.AutoSize = true;
            this.chkGetRES.Location = new System.Drawing.Point(101, 425);
            this.chkGetRES.Name = "chkGetRES";
            this.chkGetRES.Size = new System.Drawing.Size(68, 17);
            this.chkGetRES.TabIndex = 92;
            this.chkGetRES.Text = "Get RES";
            this.chkGetRES.UseVisualStyleBackColor = true;
            // 
            // cmdPop271
            // 
            this.cmdPop271.Location = new System.Drawing.Point(182, 447);
            this.cmdPop271.Name = "cmdPop271";
            this.cmdPop271.Size = new System.Drawing.Size(75, 23);
            this.cmdPop271.TabIndex = 91;
            this.cmdPop271.Text = "POP 271";
            this.cmdPop271.UseVisualStyleBackColor = true;
            this.cmdPop271.Click += new System.EventHandler(this.cmdPop271_Click);
            // 
            // lblReturnCode
            // 
            this.lblReturnCode.AutoSize = true;
            this.lblReturnCode.Location = new System.Drawing.Point(208, 354);
            this.lblReturnCode.Name = "lblReturnCode";
            this.lblReturnCode.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblReturnCode.Size = new System.Drawing.Size(0, 13);
            this.lblReturnCode.TabIndex = 90;
            this.lblReturnCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(12, 354);
            this.label21.Name = "label21";
            this.label21.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label21.Size = new System.Drawing.Size(121, 13);
            this.label21.TabIndex = 89;
            this.label21.Text = "Parser 270 Return Code";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 401);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 88;
            this.label1.Text = "Return Batch ID";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtREQReturnBatchID
            // 
            this.txtREQReturnBatchID.Location = new System.Drawing.Point(101, 399);
            this.txtREQReturnBatchID.Name = "txtREQReturnBatchID";
            this.txtREQReturnBatchID.Size = new System.Drawing.Size(157, 20);
            this.txtREQReturnBatchID.TabIndex = 87;
            this.txtREQReturnBatchID.Text = "0";
            // 
            // txt270BatchID
            // 
            this.txt270BatchID.Location = new System.Drawing.Point(101, 265);
            this.txt270BatchID.Name = "txt270BatchID";
            this.txt270BatchID.Size = new System.Drawing.Size(157, 20);
            this.txt270BatchID.TabIndex = 86;
            this.txt270BatchID.Text = "0";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(11, 267);
            this.label15.Name = "label15";
            this.label15.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label15.Size = new System.Drawing.Size(49, 13);
            this.label15.TabIndex = 85;
            this.label15.Text = "Batch_id";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt270DeleteFlag
            // 
            this.txt270DeleteFlag.Location = new System.Drawing.Point(100, 6);
            this.txt270DeleteFlag.Name = "txt270DeleteFlag";
            this.txt270DeleteFlag.Size = new System.Drawing.Size(157, 20);
            this.txt270DeleteFlag.TabIndex = 84;
            this.txt270DeleteFlag.Text = "Y";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(10, 9);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(82, 13);
            this.label14.TabIndex = 76;
            this.label14.Text = "DELETE_FLAG";
            // 
            // txt270Source
            // 
            this.txt270Source.Location = new System.Drawing.Point(101, 109);
            this.txt270Source.Name = "txt270Source";
            this.txt270Source.Size = new System.Drawing.Size(157, 20);
            this.txt270Source.TabIndex = 83;
            this.txt270Source.Text = "SOURCE_AL";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(10, 112);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(39, 13);
            this.label29.TabIndex = 80;
            this.label29.Text = "source";
            // 
            // chk270Use5010
            // 
            this.chk270Use5010.AutoSize = true;
            this.chk270Use5010.Checked = true;
            this.chk270Use5010.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk270Use5010.Location = new System.Drawing.Point(100, 296);
            this.chk270Use5010.Name = "chk270Use5010";
            this.chk270Use5010.Size = new System.Drawing.Size(72, 17);
            this.chk270Use5010.TabIndex = 72;
            this.chk270Use5010.Text = "Use 5010";
            this.chk270Use5010.UseVisualStyleBackColor = true;
            // 
            // cmdImport270
            // 
            this.cmdImport270.Location = new System.Drawing.Point(101, 320);
            this.cmdImport270.Name = "cmdImport270";
            this.cmdImport270.Size = new System.Drawing.Size(75, 23);
            this.cmdImport270.TabIndex = 71;
            this.cmdImport270.Text = "Import270";
            this.cmdImport270.UseVisualStyleBackColor = true;
            // 
            // txt270PatHospCode
            // 
            this.txt270PatHospCode.Location = new System.Drawing.Point(101, 238);
            this.txt270PatHospCode.Name = "txt270PatHospCode";
            this.txt270PatHospCode.Size = new System.Drawing.Size(157, 20);
            this.txt270PatHospCode.TabIndex = 62;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(11, 241);
            this.label16.Name = "label16";
            this.label16.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label16.Size = new System.Drawing.Size(81, 13);
            this.label16.TabIndex = 54;
            this.label16.Text = "pat_hosp_code";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt270AccountNumber
            // 
            this.txt270AccountNumber.Location = new System.Drawing.Point(101, 212);
            this.txt270AccountNumber.Name = "txt270AccountNumber";
            this.txt270AccountNumber.Size = new System.Drawing.Size(157, 20);
            this.txt270AccountNumber.TabIndex = 70;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(11, 215);
            this.label17.Name = "label17";
            this.label17.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label17.Size = new System.Drawing.Size(87, 13);
            this.label17.TabIndex = 69;
            this.label17.Text = "account_number";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt270InsType
            // 
            this.txt270InsType.Location = new System.Drawing.Point(101, 186);
            this.txt270InsType.Name = "txt270InsType";
            this.txt270InsType.Size = new System.Drawing.Size(157, 20);
            this.txt270InsType.TabIndex = 68;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(11, 189);
            this.label18.Name = "label18";
            this.label18.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label18.Size = new System.Drawing.Size(46, 13);
            this.label18.TabIndex = 67;
            this.label18.Text = "ins_type";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt270VendorName
            // 
            this.txt270VendorName.Location = new System.Drawing.Point(101, 160);
            this.txt270VendorName.Name = "txt270VendorName";
            this.txt270VendorName.Size = new System.Drawing.Size(157, 20);
            this.txt270VendorName.TabIndex = 61;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(11, 163);
            this.label19.Name = "label19";
            this.label19.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label19.Size = new System.Drawing.Size(69, 13);
            this.label19.TabIndex = 57;
            this.label19.Text = "VendorName";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt270PayorID
            // 
            this.txt270PayorID.Location = new System.Drawing.Point(101, 134);
            this.txt270PayorID.Name = "txt270PayorID";
            this.txt270PayorID.Size = new System.Drawing.Size(157, 20);
            this.txt270PayorID.TabIndex = 66;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(11, 137);
            this.label20.Name = "label20";
            this.label20.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label20.Size = new System.Drawing.Size(59, 13);
            this.label20.TabIndex = 65;
            this.label20.Text = "PayorCode";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt270HospCode
            // 
            this.txt270HospCode.Location = new System.Drawing.Point(101, 83);
            this.txt270HospCode.Name = "txt270HospCode";
            this.txt270HospCode.Size = new System.Drawing.Size(157, 20);
            this.txt270HospCode.TabIndex = 64;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(11, 86);
            this.label22.Name = "label22";
            this.label22.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label22.Size = new System.Drawing.Size(60, 13);
            this.label22.TabIndex = 63;
            this.label22.Text = "hosp_code";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt270UserId
            // 
            this.txt270UserId.Location = new System.Drawing.Point(101, 57);
            this.txt270UserId.Name = "txt270UserId";
            this.txt270UserId.Size = new System.Drawing.Size(157, 20);
            this.txt270UserId.TabIndex = 58;
            this.txt270UserId.Text = "Auditlogix";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(12, 59);
            this.label23.Name = "label23";
            this.label23.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label23.Size = new System.Drawing.Size(41, 13);
            this.label23.TabIndex = 55;
            this.label23.Text = "user_id";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt270EBRID
            // 
            this.txt270EBRID.Location = new System.Drawing.Point(101, 31);
            this.txt270EBRID.Name = "txt270EBRID";
            this.txt270EBRID.Size = new System.Drawing.Size(157, 20);
            this.txt270EBRID.TabIndex = 51;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(11, 34);
            this.label24.Name = "label24";
            this.label24.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label24.Size = new System.Drawing.Size(61, 13);
            this.label24.TabIndex = 50;
            this.label24.Text = "ebrBatchID";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmdParse270
            // 
            this.cmdParse270.Location = new System.Drawing.Point(182, 320);
            this.cmdParse270.Name = "cmdParse270";
            this.cmdParse270.Size = new System.Drawing.Size(75, 23);
            this.cmdParse270.TabIndex = 49;
            this.cmdParse270.Text = "Parse270";
            this.cmdParse270.UseVisualStyleBackColor = true;
            this.cmdParse270.Click += new System.EventHandler(this.cmdParse270_Click);
            // 
            // tab271
            // 
            this.tab271.Controls.Add(this.txt271DeadLockRetryCount);
            this.tab271.Controls.Add(this.label36);
            this.tab271.Controls.Add(this.tabControl1);
            this.tab271.Controls.Add(this.chk271LogEDI);
            this.tab271.Controls.Add(this.chk271DeleteFlag);
            this.tab271.Controls.Add(this.txt271GlobalBatchID);
            this.tab271.Controls.Add(this.lbl271BatchID);
            this.tab271.Controls.Add(this.txt271Source);
            this.tab271.Controls.Add(this.label26);
            this.tab271.Controls.Add(this.txt271VendorName);
            this.tab271.Controls.Add(this.label31);
            this.tab271.Controls.Add(this.txt271PayorID);
            this.tab271.Controls.Add(this.label32);
            this.tab271.Controls.Add(this.txt271HospCode);
            this.tab271.Controls.Add(this.label33);
            this.tab271.Controls.Add(this.txt271UserId);
            this.tab271.Controls.Add(this.label34);
            this.tab271.Controls.Add(this.txt271EBRID);
            this.tab271.Controls.Add(this.label35);
            this.tab271.Location = new System.Drawing.Point(23, 4);
            this.tab271.Name = "tab271";
            this.tab271.Padding = new System.Windows.Forms.Padding(3);
            this.tab271.Size = new System.Drawing.Size(263, 640);
            this.tab271.TabIndex = 2;
            this.tab271.Text = "5010x271";
            this.tab271.UseVisualStyleBackColor = true;
            // 
            // txt271DeadLockRetryCount
            // 
            this.txt271DeadLockRetryCount.Location = new System.Drawing.Point(100, 213);
            this.txt271DeadLockRetryCount.Name = "txt271DeadLockRetryCount";
            this.txt271DeadLockRetryCount.Size = new System.Drawing.Size(157, 20);
            this.txt271DeadLockRetryCount.TabIndex = 119;
            this.txt271DeadLockRetryCount.Text = "1";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(9, 216);
            this.label36.Name = "label36";
            this.label36.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label36.Size = new System.Drawing.Size(88, 13);
            this.label36.TabIndex = 118;
            this.label36.Text = "Dead Lock Retry";
            this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(10, 273);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(246, 349);
            this.tabControl1.TabIndex = 117;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label30);
            this.tabPage1.Controls.Add(this.label28);
            this.tabPage1.Controls.Add(this.cmdLegacyImport271);
            this.tabPage1.Controls.Add(this.cmdLegacyParse271);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(238, 323);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Legacy";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(6, 67);
            this.label30.Name = "label30";
            this.label30.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label30.Size = new System.Drawing.Size(148, 13);
            this.label30.TabIndex = 122;
            this.label30.Text = "Calls Import via Parse 271(FE)";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(6, 16);
            this.label28.Name = "label28";
            this.label28.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label28.Size = new System.Drawing.Size(107, 13);
            this.label28.TabIndex = 121;
            this.label28.Text = "Calls Import Directally";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmdLegacyImport271
            // 
            this.cmdLegacyImport271.Location = new System.Drawing.Point(157, 6);
            this.cmdLegacyImport271.Name = "cmdLegacyImport271";
            this.cmdLegacyImport271.Size = new System.Drawing.Size(75, 23);
            this.cmdLegacyImport271.TabIndex = 120;
            this.cmdLegacyImport271.Text = "Import271";
            this.cmdLegacyImport271.UseVisualStyleBackColor = true;
            this.cmdLegacyImport271.Click += new System.EventHandler(this.cmdLegacyImport271_Click);
            // 
            // cmdLegacyParse271
            // 
            this.cmdLegacyParse271.Location = new System.Drawing.Point(157, 62);
            this.cmdLegacyParse271.Name = "cmdLegacyParse271";
            this.cmdLegacyParse271.Size = new System.Drawing.Size(75, 23);
            this.cmdLegacyParse271.TabIndex = 119;
            this.cmdLegacyParse271.Text = "Parse271";
            this.cmdLegacyParse271.UseVisualStyleBackColor = true;
            this.cmdLegacyParse271.Click += new System.EventHandler(this.cmdLegacyParse271_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label27);
            this.tabPage2.Controls.Add(this.lbl271ReturnCode);
            this.tabPage2.Controls.Add(this.label25);
            this.tabPage2.Controls.Add(this.chk271Use5010);
            this.tabPage2.Controls.Add(this.cmd271Import);
            this.tabPage2.Controls.Add(this.cmd271Parse);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(238, 323);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "5010";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(39, 217);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(121, 13);
            this.label27.TabIndex = 122;
            this.label27.Text = "Parser 271 Return Code";
            // 
            // lbl271ReturnCode
            // 
            this.lbl271ReturnCode.AutoSize = true;
            this.lbl271ReturnCode.Location = new System.Drawing.Point(199, 172);
            this.lbl271ReturnCode.Name = "lbl271ReturnCode";
            this.lbl271ReturnCode.Size = new System.Drawing.Size(0, 13);
            this.lbl271ReturnCode.TabIndex = 121;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(39, 185);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(121, 13);
            this.label25.TabIndex = 120;
            this.label25.Text = "Parser 271 Return Code";
            // 
            // chk271Use5010
            // 
            this.chk271Use5010.AutoSize = true;
            this.chk271Use5010.Checked = true;
            this.chk271Use5010.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk271Use5010.Location = new System.Drawing.Point(42, 28);
            this.chk271Use5010.Name = "chk271Use5010";
            this.chk271Use5010.Size = new System.Drawing.Size(72, 17);
            this.chk271Use5010.TabIndex = 119;
            this.chk271Use5010.Text = "Use 5010";
            this.chk271Use5010.UseVisualStyleBackColor = true;
            // 
            // cmd271Import
            // 
            this.cmd271Import.Location = new System.Drawing.Point(146, 75);
            this.cmd271Import.Name = "cmd271Import";
            this.cmd271Import.Size = new System.Drawing.Size(75, 23);
            this.cmd271Import.TabIndex = 118;
            this.cmd271Import.Text = "Import271";
            this.cmd271Import.UseVisualStyleBackColor = true;
            // 
            // cmd271Parse
            // 
            this.cmd271Parse.Location = new System.Drawing.Point(146, 113);
            this.cmd271Parse.Name = "cmd271Parse";
            this.cmd271Parse.Size = new System.Drawing.Size(75, 23);
            this.cmd271Parse.TabIndex = 117;
            this.cmd271Parse.Text = "Parse271";
            this.cmd271Parse.UseVisualStyleBackColor = true;
            // 
            // chk271LogEDI
            // 
            this.chk271LogEDI.AutoSize = true;
            this.chk271LogEDI.Checked = true;
            this.chk271LogEDI.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk271LogEDI.Location = new System.Drawing.Point(101, 239);
            this.chk271LogEDI.Name = "chk271LogEDI";
            this.chk271LogEDI.Size = new System.Drawing.Size(65, 17);
            this.chk271LogEDI.TabIndex = 113;
            this.chk271LogEDI.Text = "Log EDI";
            this.chk271LogEDI.UseVisualStyleBackColor = true;
            // 
            // chk271DeleteFlag
            // 
            this.chk271DeleteFlag.AutoSize = true;
            this.chk271DeleteFlag.Checked = true;
            this.chk271DeleteFlag.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk271DeleteFlag.Location = new System.Drawing.Point(101, 9);
            this.chk271DeleteFlag.Name = "chk271DeleteFlag";
            this.chk271DeleteFlag.Size = new System.Drawing.Size(98, 17);
            this.chk271DeleteFlag.TabIndex = 112;
            this.chk271DeleteFlag.Text = "DELETE FLAG";
            this.chk271DeleteFlag.UseVisualStyleBackColor = true;
            // 
            // txt271GlobalBatchID
            // 
            this.txt271GlobalBatchID.Location = new System.Drawing.Point(101, 186);
            this.txt271GlobalBatchID.Name = "txt271GlobalBatchID";
            this.txt271GlobalBatchID.Size = new System.Drawing.Size(157, 20);
            this.txt271GlobalBatchID.TabIndex = 111;
            this.txt271GlobalBatchID.Text = "1";
            // 
            // lbl271BatchID
            // 
            this.lbl271BatchID.AutoSize = true;
            this.lbl271BatchID.Location = new System.Drawing.Point(10, 189);
            this.lbl271BatchID.Name = "lbl271BatchID";
            this.lbl271BatchID.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbl271BatchID.Size = new System.Drawing.Size(82, 13);
            this.lbl271BatchID.TabIndex = 110;
            this.lbl271BatchID.Text = "Global Batch ID";
            this.lbl271BatchID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt271Source
            // 
            this.txt271Source.Location = new System.Drawing.Point(101, 109);
            this.txt271Source.Name = "txt271Source";
            this.txt271Source.Size = new System.Drawing.Size(157, 20);
            this.txt271Source.TabIndex = 108;
            this.txt271Source.Text = "TEST";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(10, 112);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(39, 13);
            this.label26.TabIndex = 107;
            this.label26.Text = "source";
            // 
            // txt271VendorName
            // 
            this.txt271VendorName.Location = new System.Drawing.Point(101, 160);
            this.txt271VendorName.Name = "txt271VendorName";
            this.txt271VendorName.Size = new System.Drawing.Size(157, 20);
            this.txt271VendorName.TabIndex = 94;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(11, 163);
            this.label31.Name = "label31";
            this.label31.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label31.Size = new System.Drawing.Size(69, 13);
            this.label31.TabIndex = 92;
            this.label31.Text = "VendorName";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt271PayorID
            // 
            this.txt271PayorID.Location = new System.Drawing.Point(101, 134);
            this.txt271PayorID.Name = "txt271PayorID";
            this.txt271PayorID.Size = new System.Drawing.Size(157, 20);
            this.txt271PayorID.TabIndex = 99;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(11, 137);
            this.label32.Name = "label32";
            this.label32.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label32.Size = new System.Drawing.Size(48, 13);
            this.label32.TabIndex = 98;
            this.label32.Text = "Payor ID";
            this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt271HospCode
            // 
            this.txt271HospCode.Location = new System.Drawing.Point(101, 83);
            this.txt271HospCode.Name = "txt271HospCode";
            this.txt271HospCode.Size = new System.Drawing.Size(157, 20);
            this.txt271HospCode.TabIndex = 97;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(11, 86);
            this.label33.Name = "label33";
            this.label33.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label33.Size = new System.Drawing.Size(60, 13);
            this.label33.TabIndex = 96;
            this.label33.Text = "hosp_code";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt271UserId
            // 
            this.txt271UserId.Location = new System.Drawing.Point(101, 57);
            this.txt271UserId.Name = "txt271UserId";
            this.txt271UserId.Size = new System.Drawing.Size(157, 20);
            this.txt271UserId.TabIndex = 93;
            this.txt271UserId.Text = "Auditlogix";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(12, 59);
            this.label34.Name = "label34";
            this.label34.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label34.Size = new System.Drawing.Size(41, 13);
            this.label34.TabIndex = 91;
            this.label34.Text = "user_id";
            this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt271EBRID
            // 
            this.txt271EBRID.Location = new System.Drawing.Point(101, 31);
            this.txt271EBRID.Name = "txt271EBRID";
            this.txt271EBRID.Size = new System.Drawing.Size(157, 20);
            this.txt271EBRID.TabIndex = 89;
            this.txt271EBRID.Text = "42";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(11, 34);
            this.label35.Name = "label35";
            this.label35.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label35.Size = new System.Drawing.Size(61, 13);
            this.label35.TabIndex = 88;
            this.label35.Text = "ebrBatchID";
            this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tab276
            // 
            this.tab276.Controls.Add(this.tabControl2);
            this.tab276.Controls.Add(this.txt276DeletFlag);
            this.tab276.Controls.Add(this.label12);
            this.tab276.Controls.Add(this.label11);
            this.tab276.Controls.Add(this.txtPatHouseCode);
            this.tab276.Controls.Add(this.label10);
            this.tab276.Controls.Add(this.txtAccountNumber);
            this.tab276.Controls.Add(this.label9);
            this.tab276.Controls.Add(this.txtInsType);
            this.tab276.Controls.Add(this.label8);
            this.tab276.Controls.Add(this.txtVendorName);
            this.tab276.Controls.Add(this.label7);
            this.tab276.Controls.Add(this.txtPayerCode);
            this.tab276.Controls.Add(this.label6);
            this.tab276.Controls.Add(this.txtSource);
            this.tab276.Controls.Add(this.label5);
            this.tab276.Controls.Add(this.txtHospCode);
            this.tab276.Controls.Add(this.label4);
            this.tab276.Controls.Add(this.txtUserId);
            this.tab276.Controls.Add(this.label3);
            this.tab276.Controls.Add(this.txtEbrID);
            this.tab276.Controls.Add(this.label2);
            this.tab276.Controls.Add(this.txt276BatchID);
            this.tab276.Location = new System.Drawing.Point(23, 4);
            this.tab276.Name = "tab276";
            this.tab276.Padding = new System.Windows.Forms.Padding(3);
            this.tab276.Size = new System.Drawing.Size(263, 640);
            this.tab276.TabIndex = 0;
            this.tab276.Text = "5010x276";
            this.tab276.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Location = new System.Drawing.Point(14, 292);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(243, 327);
            this.tabControl2.TabIndex = 48;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.cmdImport276);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(235, 301);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Legacy";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // cmdImport276
            // 
            this.cmdImport276.Location = new System.Drawing.Point(139, 28);
            this.cmdImport276.Name = "cmdImport276";
            this.cmdImport276.Size = new System.Drawing.Size(75, 23);
            this.cmdImport276.TabIndex = 26;
            this.cmdImport276.Text = "Import276";
            this.cmdImport276.UseVisualStyleBackColor = true;
            this.cmdImport276.Click += new System.EventHandler(this.cmdImport276_Click_1);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.lbl276_5010ImportReturnCode);
            this.tabPage4.Controls.Add(this.label37);
            this.tabPage4.Controls.Add(this.label38);
            this.tabPage4.Controls.Add(this.cmd276_5010Import);
            this.tabPage4.Controls.Add(this.cmd5010Parse276);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(235, 301);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "5010";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // lbl276_5010ImportReturnCode
            // 
            this.lbl276_5010ImportReturnCode.Location = new System.Drawing.Point(6, 32);
            this.lbl276_5010ImportReturnCode.Name = "lbl276_5010ImportReturnCode";
            this.lbl276_5010ImportReturnCode.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lbl276_5010ImportReturnCode.Size = new System.Drawing.Size(223, 17);
            this.lbl276_5010ImportReturnCode.TabIndex = 127;
            this.lbl276_5010ImportReturnCode.Text = "Import Return Code";
            this.lbl276_5010ImportReturnCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(3, 67);
            this.label37.Name = "label37";
            this.label37.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label37.Size = new System.Drawing.Size(127, 13);
            this.label37.TabIndex = 126;
            this.label37.Text = "Calls Import via Parse(FE)";
            this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(3, 16);
            this.label38.Name = "label38";
            this.label38.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label38.Size = new System.Drawing.Size(107, 13);
            this.label38.TabIndex = 125;
            this.label38.Text = "Calls Import Directally";
            this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmd276_5010Import
            // 
            this.cmd276_5010Import.Location = new System.Drawing.Point(154, 6);
            this.cmd276_5010Import.Name = "cmd276_5010Import";
            this.cmd276_5010Import.Size = new System.Drawing.Size(75, 23);
            this.cmd276_5010Import.TabIndex = 124;
            this.cmd276_5010Import.Text = "Import276";
            this.cmd276_5010Import.UseVisualStyleBackColor = true;
            this.cmd276_5010Import.Click += new System.EventHandler(this.cmd5010Import276_Click);
            // 
            // cmd5010Parse276
            // 
            this.cmd5010Parse276.Location = new System.Drawing.Point(154, 62);
            this.cmd5010Parse276.Name = "cmd5010Parse276";
            this.cmd5010Parse276.Size = new System.Drawing.Size(75, 23);
            this.cmd5010Parse276.TabIndex = 123;
            this.cmd5010Parse276.Text = "Parse276";
            this.cmd5010Parse276.UseVisualStyleBackColor = true;
            // 
            // txt276DeletFlag
            // 
            this.txt276DeletFlag.Location = new System.Drawing.Point(100, 6);
            this.txt276DeletFlag.Name = "txt276DeletFlag";
            this.txt276DeletFlag.Size = new System.Drawing.Size(157, 20);
            this.txt276DeletFlag.TabIndex = 37;
            this.txt276DeletFlag.Text = "Y";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(10, 9);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(82, 13);
            this.label12.TabIndex = 29;
            this.label12.Text = "DELETE_FLAG";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(10, 269);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(46, 13);
            this.label11.TabIndex = 30;
            this.label11.Text = "BatchID";
            // 
            // txtPatHouseCode
            // 
            this.txtPatHouseCode.Location = new System.Drawing.Point(100, 240);
            this.txtPatHouseCode.Name = "txtPatHouseCode";
            this.txtPatHouseCode.Size = new System.Drawing.Size(157, 20);
            this.txtPatHouseCode.TabIndex = 39;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 243);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(81, 13);
            this.label10.TabIndex = 31;
            this.label10.Text = "pat_hosp_code";
            // 
            // txtAccountNumber
            // 
            this.txtAccountNumber.Location = new System.Drawing.Point(100, 214);
            this.txtAccountNumber.Name = "txtAccountNumber";
            this.txtAccountNumber.Size = new System.Drawing.Size(157, 20);
            this.txtAccountNumber.TabIndex = 47;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 217);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(87, 13);
            this.label9.TabIndex = 46;
            this.label9.Text = "account_number";
            // 
            // txtInsType
            // 
            this.txtInsType.Location = new System.Drawing.Point(100, 188);
            this.txtInsType.Name = "txtInsType";
            this.txtInsType.Size = new System.Drawing.Size(157, 20);
            this.txtInsType.TabIndex = 45;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 191);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 13);
            this.label8.TabIndex = 44;
            this.label8.Text = "ins_type";
            // 
            // txtVendorName
            // 
            this.txtVendorName.Location = new System.Drawing.Point(100, 162);
            this.txtVendorName.Name = "txtVendorName";
            this.txtVendorName.Size = new System.Drawing.Size(157, 20);
            this.txtVendorName.TabIndex = 38;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 165);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 13);
            this.label7.TabIndex = 34;
            this.label7.Text = "VendorName";
            // 
            // txtPayerCode
            // 
            this.txtPayerCode.Location = new System.Drawing.Point(100, 136);
            this.txtPayerCode.Name = "txtPayerCode";
            this.txtPayerCode.Size = new System.Drawing.Size(157, 20);
            this.txtPayerCode.TabIndex = 43;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 139);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 13);
            this.label6.TabIndex = 42;
            this.label6.Text = "PayorCode";
            // 
            // txtSource
            // 
            this.txtSource.Location = new System.Drawing.Point(100, 110);
            this.txtSource.Name = "txtSource";
            this.txtSource.Size = new System.Drawing.Size(157, 20);
            this.txtSource.TabIndex = 36;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 113);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 33;
            this.label5.Text = "source";
            // 
            // txtHospCode
            // 
            this.txtHospCode.Location = new System.Drawing.Point(100, 84);
            this.txtHospCode.Name = "txtHospCode";
            this.txtHospCode.Size = new System.Drawing.Size(157, 20);
            this.txtHospCode.TabIndex = 41;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 40;
            this.label4.Text = "hosp_code";
            // 
            // txtUserId
            // 
            this.txtUserId.Location = new System.Drawing.Point(100, 58);
            this.txtUserId.Name = "txtUserId";
            this.txtUserId.Size = new System.Drawing.Size(157, 20);
            this.txtUserId.TabIndex = 35;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 32;
            this.label3.Text = "user_id";
            // 
            // txtEbrID
            // 
            this.txtEbrID.Location = new System.Drawing.Point(100, 32);
            this.txtEbrID.Name = "txtEbrID";
            this.txtEbrID.Size = new System.Drawing.Size(157, 20);
            this.txtEbrID.TabIndex = 28;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 27;
            this.label2.Text = "ebrBatchID";
            // 
            // txt276BatchID
            // 
            this.txt276BatchID.Location = new System.Drawing.Point(100, 266);
            this.txt276BatchID.Name = "txt276BatchID";
            this.txt276BatchID.Size = new System.Drawing.Size(157, 20);
            this.txt276BatchID.TabIndex = 25;
            // 
            // tab277
            // 
            this.tab277.Controls.Add(this.tabControl3);
            this.tab277.Controls.Add(this.txt277DeleteFlag);
            this.tab277.Controls.Add(this.label42);
            this.tab277.Controls.Add(this.label43);
            this.tab277.Controls.Add(this.textBox2);
            this.tab277.Controls.Add(this.label44);
            this.tab277.Controls.Add(this.textBox3);
            this.tab277.Controls.Add(this.label45);
            this.tab277.Controls.Add(this.textBox4);
            this.tab277.Controls.Add(this.label46);
            this.tab277.Controls.Add(this.textBox5);
            this.tab277.Controls.Add(this.label47);
            this.tab277.Controls.Add(this.textBox6);
            this.tab277.Controls.Add(this.label48);
            this.tab277.Controls.Add(this.textBox7);
            this.tab277.Controls.Add(this.label49);
            this.tab277.Controls.Add(this.textBox8);
            this.tab277.Controls.Add(this.label50);
            this.tab277.Controls.Add(this.textBox9);
            this.tab277.Controls.Add(this.label51);
            this.tab277.Controls.Add(this.textBox10);
            this.tab277.Controls.Add(this.label52);
            this.tab277.Controls.Add(this.txt277BatchID);
            this.tab277.Location = new System.Drawing.Point(23, 4);
            this.tab277.Name = "tab277";
            this.tab277.Padding = new System.Windows.Forms.Padding(3);
            this.tab277.Size = new System.Drawing.Size(263, 640);
            this.tab277.TabIndex = 1;
            this.tab277.Text = "5010x277";
            this.tab277.UseVisualStyleBackColor = true;
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage5);
            this.tabControl3.Controls.Add(this.tabPage6);
            this.tabControl3.Location = new System.Drawing.Point(12, 300);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(243, 327);
            this.tabControl3.TabIndex = 71;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.button1);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(235, 301);
            this.tabPage5.TabIndex = 0;
            this.tabPage5.Text = "Legacy";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(139, 28);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 26;
            this.button1.Text = "Import276";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.lbl277_5010ImportReturnCode);
            this.tabPage6.Controls.Add(this.label40);
            this.tabPage6.Controls.Add(this.label41);
            this.tabPage6.Controls.Add(this.cmd277_5010Import);
            this.tabPage6.Controls.Add(this.button3);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(235, 301);
            this.tabPage6.TabIndex = 1;
            this.tabPage6.Text = "5010";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // lbl277_5010ImportReturnCode
            // 
            this.lbl277_5010ImportReturnCode.Location = new System.Drawing.Point(6, 32);
            this.lbl277_5010ImportReturnCode.Name = "lbl277_5010ImportReturnCode";
            this.lbl277_5010ImportReturnCode.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lbl277_5010ImportReturnCode.Size = new System.Drawing.Size(223, 17);
            this.lbl277_5010ImportReturnCode.TabIndex = 127;
            this.lbl277_5010ImportReturnCode.Text = "Import Return Code";
            this.lbl277_5010ImportReturnCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(3, 67);
            this.label40.Name = "label40";
            this.label40.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label40.Size = new System.Drawing.Size(127, 13);
            this.label40.TabIndex = 126;
            this.label40.Text = "Calls Import via Parse(FE)";
            this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Location = new System.Drawing.Point(3, 16);
            this.label41.Name = "label41";
            this.label41.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label41.Size = new System.Drawing.Size(107, 13);
            this.label41.TabIndex = 125;
            this.label41.Text = "Calls Import Directally";
            this.label41.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmd277_5010Import
            // 
            this.cmd277_5010Import.Location = new System.Drawing.Point(154, 6);
            this.cmd277_5010Import.Name = "cmd277_5010Import";
            this.cmd277_5010Import.Size = new System.Drawing.Size(75, 23);
            this.cmd277_5010Import.TabIndex = 124;
            this.cmd277_5010Import.Text = "Import277";
            this.cmd277_5010Import.UseVisualStyleBackColor = true;
            this.cmd277_5010Import.Click += new System.EventHandler(this.cmd277_5010Import_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(154, 62);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 123;
            this.button3.Text = "Parse277";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // txt277DeleteFlag
            // 
            this.txt277DeleteFlag.Location = new System.Drawing.Point(98, 14);
            this.txt277DeleteFlag.Name = "txt277DeleteFlag";
            this.txt277DeleteFlag.Size = new System.Drawing.Size(157, 20);
            this.txt277DeleteFlag.TabIndex = 60;
            this.txt277DeleteFlag.Text = "Y";
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Location = new System.Drawing.Point(8, 17);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(82, 13);
            this.label42.TabIndex = 52;
            this.label42.Text = "DELETE_FLAG";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(8, 277);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(46, 13);
            this.label43.TabIndex = 53;
            this.label43.Text = "BatchID";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(98, 248);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(157, 20);
            this.textBox2.TabIndex = 62;
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(8, 251);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(81, 13);
            this.label44.TabIndex = 54;
            this.label44.Text = "pat_hosp_code";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(98, 222);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(157, 20);
            this.textBox3.TabIndex = 70;
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(8, 225);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(87, 13);
            this.label45.TabIndex = 69;
            this.label45.Text = "account_number";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(98, 196);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(157, 20);
            this.textBox4.TabIndex = 68;
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(8, 199);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(46, 13);
            this.label46.TabIndex = 67;
            this.label46.Text = "ins_type";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(98, 170);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(157, 20);
            this.textBox5.TabIndex = 61;
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(8, 173);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(69, 13);
            this.label47.TabIndex = 57;
            this.label47.Text = "VendorName";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(98, 144);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(157, 20);
            this.textBox6.TabIndex = 66;
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Location = new System.Drawing.Point(8, 147);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(59, 13);
            this.label48.TabIndex = 65;
            this.label48.Text = "PayorCode";
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(98, 118);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(157, 20);
            this.textBox7.TabIndex = 59;
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Location = new System.Drawing.Point(8, 121);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(39, 13);
            this.label49.TabIndex = 56;
            this.label49.Text = "source";
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(98, 92);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(157, 20);
            this.textBox8.TabIndex = 64;
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(8, 95);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(60, 13);
            this.label50.TabIndex = 63;
            this.label50.Text = "hosp_code";
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(98, 66);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(157, 20);
            this.textBox9.TabIndex = 58;
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Location = new System.Drawing.Point(9, 68);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(41, 13);
            this.label51.TabIndex = 55;
            this.label51.Text = "user_id";
            // 
            // textBox10
            // 
            this.textBox10.Location = new System.Drawing.Point(98, 40);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(157, 20);
            this.textBox10.TabIndex = 51;
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.Location = new System.Drawing.Point(8, 43);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(61, 13);
            this.label52.TabIndex = 50;
            this.label52.Text = "ebrBatchID";
            // 
            // txt277BatchID
            // 
            this.txt277BatchID.Location = new System.Drawing.Point(98, 274);
            this.txt277BatchID.Name = "txt277BatchID";
            this.txt277BatchID.Size = new System.Drawing.Size(157, 20);
            this.txt277BatchID.TabIndex = 49;
            // 
            // tab835
            // 
            this.tab835.Location = new System.Drawing.Point(23, 4);
            this.tab835.Name = "tab835";
            this.tab835.Size = new System.Drawing.Size(263, 640);
            this.tab835.TabIndex = 3;
            this.tab835.Text = "5010x835";
            this.tab835.UseVisualStyleBackColor = true;
            // 
            // tab837
            // 
            this.tab837.Controls.Add(this.txt837BatchID);
            this.tab837.Controls.Add(this.label13);
            this.tab837.Location = new System.Drawing.Point(23, 4);
            this.tab837.Name = "tab837";
            this.tab837.Size = new System.Drawing.Size(263, 640);
            this.tab837.TabIndex = 4;
            this.tab837.Text = "5010x837";
            this.tab837.UseVisualStyleBackColor = true;
            // 
            // txt837BatchID
            // 
            this.txt837BatchID.Location = new System.Drawing.Point(106, 15);
            this.txt837BatchID.Name = "txt837BatchID";
            this.txt837BatchID.Size = new System.Drawing.Size(141, 20);
            this.txt837BatchID.TabIndex = 3;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(16, 19);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(87, 13);
            this.label13.TabIndex = 2;
            this.label13.Text = "837_ BATCH_ID";
            // 
            // frmEDI5010
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1268, 776);
            this.Controls.Add(this.tbpEDI);
            this.Controls.Add(this.cmdDecode);
            this.Controls.Add(this.txtConstring);
            this.Controls.Add(this.tabView);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.SBar);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmEDI5010";
            this.Text = "frmEDI5010";
            this.Load += new System.EventHandler(this.frmEDI5010_Load);
            this.SBar.ResumeLayout(false);
            this.SBar.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabView.ResumeLayout(false);
            this.tbpRaw.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fctEDI)).EndInit();
            this.tbpList.ResumeLayout(false);
            this.tbpEDI.ResumeLayout(false);
            this.tab270.ResumeLayout(false);
            this.tab270.PerformLayout();
            this.tab271.ResumeLayout(false);
            this.tab271.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tab276.ResumeLayout(false);
            this.tab276.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tab277.ResumeLayout(false);
            this.tab277.PerformLayout();
            this.tabControl3.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            this.tab837.ResumeLayout(false);
            this.tab837.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip SBar;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbValidate;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tslState;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbReadale;
        private System.Windows.Forms.TabControl tabView;
        private System.Windows.Forms.TabPage tbpRaw;
        private System.Windows.Forms.TabPage tbpList;
        private System.Windows.Forms.ListView lstList;
        private System.Windows.Forms.ToolStripButton tsbParse;
        private System.Windows.Forms.ToolStripStatusLabel tslTSIC;
        private System.Windows.Forms.ToolStripStatusLabel tslCR;
        private System.Windows.Forms.TabPage tpbDataSegment;
        private System.Windows.Forms.ToolStripStatusLabel tslPARSERSTATUS;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.TextBox txtConstring;
        private System.Windows.Forms.Button cmdDecode;
        private System.Windows.Forms.TabControl tbpEDI;
        private System.Windows.Forms.TabPage tab277;
        private System.Windows.Forms.TabPage tab276;
        private System.Windows.Forms.TextBox txt276DeletFlag;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtPatHouseCode;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtAccountNumber;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtInsType;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtVendorName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtPayerCode;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtHospCode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtUserId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtEbrID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button cmdImport276;
        private System.Windows.Forms.TextBox txt276BatchID;
        private System.Windows.Forms.TabPage tab271;
        private FastColoredTextBoxNS.FastColoredTextBox fctEDI;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.TabPage tab835;
        private System.Windows.Forms.TabPage tab837;
        private System.Windows.Forms.TextBox txt837BatchID;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TabPage tab270;
        private System.Windows.Forms.Button cmdImport270;
        private System.Windows.Forms.TextBox txt270PatHospCode;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txt270AccountNumber;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txt270InsType;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txt270VendorName;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txt270PayorID;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txt270HospCode;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox txt270UserId;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox txt270EBRID;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Button cmdParse270;
        private System.Windows.Forms.CheckBox chk270Use5010;
        private System.Windows.Forms.TextBox txt270DeleteFlag;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txt270Source;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox txt270BatchID;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txt271GlobalBatchID;
        private System.Windows.Forms.Label lbl271BatchID;
        private System.Windows.Forms.TextBox txt271Source;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox txt271VendorName;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.TextBox txt271PayorID;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.TextBox txt271HospCode;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.TextBox txt271UserId;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.TextBox txt271EBRID;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.CheckBox chk271LogEDI;
        private System.Windows.Forms.CheckBox chk271DeleteFlag;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtREQReturnBatchID;
        private System.Windows.Forms.Label lblReturnCode;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Button cmdPop271;
        private System.Windows.Forms.Button cmdGetREQ;
        private System.Windows.Forms.CheckBox chkGetRES;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button cmdLegacyImport271;
        private System.Windows.Forms.Button cmdLegacyParse271;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label lbl271ReturnCode;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.CheckBox chk271Use5010;
        private System.Windows.Forms.Button cmd271Import;
        private System.Windows.Forms.Button cmd271Parse;
        private System.Windows.Forms.TextBox txt271DeadLockRetryCount;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Button cmd276_5010Import;
        private System.Windows.Forms.Button cmd5010Parse276;
        private System.Windows.Forms.Label lbl276_5010ImportReturnCode;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.Label lbl277_5010ImportReturnCode;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Button cmd277_5010Import;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox txt277DeleteFlag;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.TextBox txt277BatchID;
    }
}