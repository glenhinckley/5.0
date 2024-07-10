
# region Heading

/**************************************************************************************************************/
/*                                                                                                            */
/*  RegexTester.cs                                                                                            */
/*                                                                                                            */
/*  A test-bed for .net Regular Expressions.                                                                  */
/*                                                                                                            */
/*  This is free code, use it as you require.                                                                 */
/*  If you modify it please use your own namespace.                                                           */
/*                                                                                                            */
/*  If you like it or have suggestions for improvements please let me know at: PIEBALDconsult@aol.com         */
/*                                                                                                            */
/*  Modification history:                                                                                     */
/*  2009-08-09          Sir John E. Boucher     Created                                                       */
/*                                                                                                            */
/**************************************************************************************************************/

# endregion

namespace PIEBALD.RegexTester
{
    public partial class frmRegexTester : System.Windows.Forms.Form
    {
        private System.ComponentModel.IContainer    components       = null ;
        private System.Windows.Forms.SplitContainer scMain           ;
        private System.Windows.Forms.SplitContainer scTopLeft        ;        
        private System.Windows.Forms.TextBox        tbRegex          ;
        private System.Windows.Forms.CheckedListBox clbOptions       ;
        private System.Windows.Forms.TextBox        tbInput          ;
        private System.Windows.Forms.RichTextBox    tbResult         ;
        private System.Windows.Forms.Button         bGetResult       ;
        private System.Windows.Forms.ToolTip        ttMain           ;
        private System.Windows.Forms.ComboBox       cbMode           ;
        private System.Windows.Forms.CheckBox       cbShowWhitespace ;


        private readonly System.IO.FileInfo configfile ;
        
        /* The following delegates are to resolve cross-thread violations */
        private readonly EnableDelegate            DoEnable            ;
        private readonly AppendResultDelegate      DoAppendResult      ;

        public frmRegexTester 
        (
        )
        {
            InitializeComponent() ;
            
            foreach 
            ( 
                System.Text.RegularExpressions.RegexOptions opt
            in 
                System.Enum.GetValues ( typeof(System.Text.RegularExpressions.RegexOptions) )
            )
            {
                if ( opt != System.Text.RegularExpressions.RegexOptions.None )
                {
                    this.clbOptions.Items.Add ( opt ) ;
                }
            }
            
            foreach 
            ( 
                Mode mode
            in 
                System.Enum.GetValues ( typeof(Mode) )
            )
            {
                this.cbMode.Items.Add ( mode ) ;
            }
            
            this.cbMode.SelectedIndex = 0 ;

            
            this.DoEnable = new EnableDelegate ( this.Enable ) ;
            
            this.DoAppendResult = new AppendResultDelegate ( this.AppendResult ) ;
            
            this.HelpRequested += this.GiveHelp ;
            
            this.configfile = new System.IO.FileInfo
            (
                System.Environment.ExpandEnvironmentVariables
                (
                    "%USERPROFILE%\\Local Settings\\RegexTester.xml" 
                ) 
            ) ;

            return ;
        }

# region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            this.components = new System.ComponentModel.Container ();
            System.Windows.Forms.SplitContainer scTop;
            System.Windows.Forms.GroupBox gbRegex;
            System.Windows.Forms.GroupBox gbInput;
            System.Windows.Forms.GroupBox gbControl;
            System.Windows.Forms.GroupBox gbResult;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager ( typeof ( frmRegexTester ) );
            this.scTopLeft = new System.Windows.Forms.SplitContainer ();
            this.tbRegex = new System.Windows.Forms.TextBox ();
            this.tbInput = new System.Windows.Forms.TextBox ();
            this.bGetResult = new System.Windows.Forms.Button ();
            this.clbOptions = new System.Windows.Forms.CheckedListBox ();
            this.cbShowWhitespace = new System.Windows.Forms.CheckBox ();
            this.cbMode = new System.Windows.Forms.ComboBox ();
            this.tbResult = new System.Windows.Forms.RichTextBox ();
            this.scMain = new System.Windows.Forms.SplitContainer ();
            this.ttMain = new System.Windows.Forms.ToolTip ( this.components );
            scTop = new System.Windows.Forms.SplitContainer ();
            gbRegex = new System.Windows.Forms.GroupBox ();
            gbInput = new System.Windows.Forms.GroupBox ();
            gbControl = new System.Windows.Forms.GroupBox ();
            gbResult = new System.Windows.Forms.GroupBox ();
            scTop.Panel1.SuspendLayout ();
            scTop.Panel2.SuspendLayout ();
            scTop.SuspendLayout ();
            this.scTopLeft.Panel1.SuspendLayout ();
            this.scTopLeft.Panel2.SuspendLayout ();
            this.scTopLeft.SuspendLayout ();
            gbRegex.SuspendLayout ();
            gbInput.SuspendLayout ();
            gbControl.SuspendLayout ();
            gbResult.SuspendLayout ();
            this.scMain.Panel1.SuspendLayout ();
            this.scMain.Panel2.SuspendLayout ();
            this.scMain.SuspendLayout ();
            this.SuspendLayout ();
            // 
            // scTop
            // 
            scTop.Dock = System.Windows.Forms.DockStyle.Fill;
            scTop.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            scTop.IsSplitterFixed = true;
            scTop.Location = new System.Drawing.Point ( 0 , 0 );
            scTop.Name = "scTop";
            // 
            // scTop.Panel1
            // 
            scTop.Panel1.Controls.Add ( this.scTopLeft );
            // 
            // scTop.Panel2
            // 
            scTop.Panel2.Controls.Add ( gbControl );
            scTop.Size = new System.Drawing.Size ( 592 , 240 );
            scTop.SplitterDistance = 436;
            scTop.TabIndex = 0;
            scTop.TabStop = false;
            // 
            // scTopLeft
            // 
            this.scTopLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scTopLeft.Location = new System.Drawing.Point ( 0 , 0 );
            this.scTopLeft.Name = "scTopLeft";
            this.scTopLeft.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scTopLeft.Panel1
            // 
            this.scTopLeft.Panel1.Controls.Add ( gbRegex );
            this.scTopLeft.Panel1MinSize = 75;
            // 
            // scTopLeft.Panel2
            // 
            this.scTopLeft.Panel2.Controls.Add ( gbInput );
            this.scTopLeft.Panel2MinSize = 75;
            this.scTopLeft.Size = new System.Drawing.Size ( 436 , 240 );
            this.scTopLeft.SplitterDistance = 117;
            this.scTopLeft.TabIndex = 0;
            this.scTopLeft.TabStop = false;
            // 
            // gbRegex
            // 
            gbRegex.Controls.Add ( this.tbRegex );
            gbRegex.Dock = System.Windows.Forms.DockStyle.Fill;
            gbRegex.Location = new System.Drawing.Point ( 0 , 0 );
            gbRegex.Name = "gbRegex";
            gbRegex.Size = new System.Drawing.Size ( 436 , 117 );
            gbRegex.TabIndex = 0;
            gbRegex.TabStop = false;
            gbRegex.Text = "Regular Expression";
            // 
            // tbRegex
            // 
            this.tbRegex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbRegex.Font = new System.Drawing.Font ( "Arial Unicode MS" , 12F , System.Drawing.FontStyle.Bold , System.Drawing.GraphicsUnit.Point , ( (byte) ( 0 ) ) );
            this.tbRegex.Location = new System.Drawing.Point ( 3 , 16 );
            this.tbRegex.Multiline = true;
            this.tbRegex.Name = "tbRegex";
            this.tbRegex.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbRegex.Size = new System.Drawing.Size ( 430 , 98 );
            this.tbRegex.TabIndex = 0;
            this.ttMain.SetToolTip ( this.tbRegex , "Type or copy the text for the Regex" );
            this.tbRegex.WordWrap = false;
            // 
            // gbInput
            // 
            gbInput.Controls.Add ( this.tbInput );
            gbInput.Dock = System.Windows.Forms.DockStyle.Fill;
            gbInput.Location = new System.Drawing.Point ( 0 , 0 );
            gbInput.Name = "gbInput";
            gbInput.Size = new System.Drawing.Size ( 436 , 119 );
            gbInput.TabIndex = 0;
            gbInput.TabStop = false;
            gbInput.Text = "Input";
            // 
            // tbInput
            // 
            this.tbInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbInput.Font = new System.Drawing.Font ( "Arial Unicode MS" , 12F , System.Drawing.FontStyle.Bold , System.Drawing.GraphicsUnit.Point , ( (byte) ( 0 ) ) );
            this.tbInput.Location = new System.Drawing.Point ( 3 , 16 );
            this.tbInput.Multiline = true;
            this.tbInput.Name = "tbInput";
            this.tbInput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbInput.Size = new System.Drawing.Size ( 430 , 100 );
            this.tbInput.TabIndex = 0;
            this.ttMain.SetToolTip ( this.tbInput , "Type or copy some text to test with the Regex" );
            this.tbInput.WordWrap = false;
            // 
            // gbControl
            // 
            gbControl.Controls.Add ( this.bGetResult );
            gbControl.Controls.Add ( this.clbOptions );
            gbControl.Controls.Add ( this.cbShowWhitespace );
            gbControl.Controls.Add ( this.cbMode );
            gbControl.Dock = System.Windows.Forms.DockStyle.Fill;
            gbControl.Location = new System.Drawing.Point ( 0 , 0 );
            gbControl.Name = "gbControl";
            gbControl.Size = new System.Drawing.Size ( 152 , 240 );
            gbControl.TabIndex = 4;
            gbControl.TabStop = false;
            gbControl.Text = "Options";
            // 
            // bGetResult
            // 
            this.bGetResult.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.bGetResult.Location = new System.Drawing.Point ( 6 , 212 );
            this.bGetResult.Name = "bGetResult";
            this.bGetResult.Size = new System.Drawing.Size ( 140 , 23 );
            this.bGetResult.TabIndex = 3;
            this.bGetResult.Text = "Get result";
            this.ttMain.SetToolTip ( this.bGetResult , "Click to create the Regex and test it with the Input" );
            this.bGetResult.UseVisualStyleBackColor = true;
            this.bGetResult.Click += new System.EventHandler ( this.bGetResult_Click );
            // 
            // clbOptions
            // 
            this.clbOptions.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                        | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.clbOptions.CheckOnClick = true;
            this.clbOptions.FormattingEnabled = true;
            this.clbOptions.IntegralHeight = false;
            this.clbOptions.Location = new System.Drawing.Point ( 6 , 16 );
            this.clbOptions.Name = "clbOptions";
            this.clbOptions.Size = new System.Drawing.Size ( 140 , 143 );
            this.clbOptions.TabIndex = 0;
            this.ttMain.SetToolTip ( this.clbOptions , "Options to apply to the Regex" );
            // 
            // cbShowWhitespace
            // 
            this.cbShowWhitespace.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left ) ) );
            this.cbShowWhitespace.AutoSize = true;
            this.cbShowWhitespace.Location = new System.Drawing.Point ( 6 , 192 );
            this.cbShowWhitespace.Name = "cbShowWhitespace";
            this.cbShowWhitespace.Size = new System.Drawing.Size ( 110 , 17 );
            this.cbShowWhitespace.TabIndex = 2;
            this.cbShowWhitespace.Text = "Show whitespace";
            this.ttMain.SetToolTip ( this.cbShowWhitespace , "Show whitespace characters in the Result" );
            this.cbShowWhitespace.UseVisualStyleBackColor = true;
            // 
            // cbMode
            // 
            this.cbMode.Anchor = ( (System.Windows.Forms.AnchorStyles) ( ( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left )
                        | System.Windows.Forms.AnchorStyles.Right ) ) );
            this.cbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMode.FormattingEnabled = true;
            this.cbMode.Location = new System.Drawing.Point ( 6 , 165 );
            this.cbMode.Name = "cbMode";
            this.cbMode.Size = new System.Drawing.Size ( 140 , 21 );
            this.cbMode.TabIndex = 1;
            this.ttMain.SetToolTip ( this.cbMode , "Regex handling mode" );
            // 
            // gbResult
            // 
            gbResult.Controls.Add ( this.tbResult );
            gbResult.Dock = System.Windows.Forms.DockStyle.Fill;
            gbResult.Location = new System.Drawing.Point ( 0 , 0 );
            gbResult.Name = "gbResult";
            gbResult.Size = new System.Drawing.Size ( 592 , 129 );
            gbResult.TabIndex = 0;
            gbResult.TabStop = false;
            gbResult.Text = "Result";
            // 
            // tbResult
            // 
            this.tbResult.BackColor = System.Drawing.SystemColors.Window;
            this.tbResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbResult.Font = new System.Drawing.Font ( "Arial Unicode MS" , 12F , System.Drawing.FontStyle.Bold , System.Drawing.GraphicsUnit.Point , ( (byte) ( 0 ) ) );
            this.tbResult.Location = new System.Drawing.Point ( 3 , 16 );
            this.tbResult.Name = "tbResult";
            this.tbResult.ReadOnly = true;
            this.tbResult.Size = new System.Drawing.Size ( 586 , 110 );
            this.tbResult.TabIndex = 0;
            this.tbResult.TabStop = false;
            this.tbResult.Text = "";
            this.ttMain.SetToolTip ( this.tbResult , "The matches and groups of the Regex" );
            this.tbResult.WordWrap = false;
            // 
            // scMain
            // 
            this.scMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scMain.Location = new System.Drawing.Point ( 0 , 0 );
            this.scMain.Name = "scMain";
            this.scMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scMain.Panel1
            // 
            this.scMain.Panel1.Controls.Add ( scTop );
            this.scMain.Panel1MinSize = 240;
            // 
            // scMain.Panel2
            // 
            this.scMain.Panel2.Controls.Add ( gbResult );
            this.scMain.Panel2MinSize = 129;
            this.scMain.Size = new System.Drawing.Size ( 592 , 373 );
            this.scMain.SplitterDistance = 240;
            this.scMain.TabIndex = 0;
            this.scMain.TabStop = false;
            // 
            // frmRegexTester
            // 
            this.AcceptButton = this.bGetResult;
            this.AutoScaleDimensions = new System.Drawing.SizeF ( 6F , 13F );
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size ( 592 , 373 );
            this.Controls.Add ( this.scMain );
            this.Icon = ( (System.Drawing.Icon) ( resources.GetObject ( "$this.Icon" ) ) );
            this.MinimumSize = new System.Drawing.Size ( 600 , 400 );
            this.Name = "frmRegexTester";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Regular Expression tester for .net";
            this.Load += new System.EventHandler ( this.frmRegexTester_Load );
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler ( this.frmRegexTester_FormClosing );
            scTop.Panel1.ResumeLayout ( false );
            scTop.Panel2.ResumeLayout ( false );
            scTop.ResumeLayout ( false );
            this.scTopLeft.Panel1.ResumeLayout ( false );
            this.scTopLeft.Panel2.ResumeLayout ( false );
            this.scTopLeft.ResumeLayout ( false );
            gbRegex.ResumeLayout ( false );
            gbRegex.PerformLayout ();
            gbInput.ResumeLayout ( false );
            gbInput.PerformLayout ();
            gbControl.ResumeLayout ( false );
            gbControl.PerformLayout ();
            gbResult.ResumeLayout ( false );
            this.scMain.Panel1.ResumeLayout ( false );
            this.scMain.Panel2.ResumeLayout ( false );
            this.scMain.ResumeLayout ( false );
            this.ResumeLayout ( false );

        }

# endregion

        private void 
        frmRegexTester_Load 
        ( 
            object           sender 
        , 
            System.EventArgs e 
        )
        {
            try
            {
                if ( this.configfile.Exists )
                {
                    System.Xml.XmlDocument config = 
                        PIEBALD.Lib.LibXml.LoadXmlDocument ( this.configfile ) ;

                    System.Xml.XmlElement   ele  ;
                    System.Xml.XmlAttribute att  ;
                    int                     temp ;
                    
                    ele = config.SelectSingleNode ( "RegexTester/Window" ) 
                        as System.Xml.XmlElement ;
                    
                    if ( ele != null )
                    {                
                        att = ele.Attributes [ "Maximized" ] ;
                        
                        if
                        (
                            ( att != null )
                        &&
                            ( att.Value == System.Boolean.TrueString )
                        )
                        {
                            this.WindowState = System.Windows.Forms.FormWindowState.Maximized ;
                        }
                        
                        att = ele.Attributes [ "Top" ] ;
                        
                        if
                        (
                            ( att != null )
                        &&
                            System.Int32.TryParse ( att.Value , out temp )
                        )
                        {
                            this.Top = temp ;
                        }
                        
                        att = ele.Attributes [ "Left" ] ;
                        
                        if
                        (
                            ( att != null )
                        &&
                            System.Int32.TryParse ( att.Value , out temp )
                        )
                        {
                            this.Left = temp ;
                        }
                        
                        att = ele.Attributes [ "Height" ] ;
                        
                        if
                        (
                            ( att != null )
                        &&
                            System.Int32.TryParse ( att.Value , out temp )
                        )
                        {
                            this.Height = temp ;
                        }
                        
                        att = ele.Attributes [ "Width" ] ;
                        
                        if
                        (
                            ( att != null )
                        &&
                            System.Int32.TryParse ( att.Value , out temp )
                        )
                        {
                            this.Width = temp ;
                        }
                        
                        att = ele.Attributes [ "MainSplitter" ] ;
                        
                        if
                        (
                            ( att != null )
                        &&
                            System.Int32.TryParse ( att.Value , out temp )
                        )
                        {
                            this.scMain.SplitterDistance = temp ;
                        }
                        
                        att = ele.Attributes [ "TopLeftSplitter" ] ;
                        
                        if
                        (
                            ( att != null )
                        &&
                            System.Int32.TryParse ( att.Value , out temp )
                        )
                        {
                            this.scTopLeft.SplitterDistance = temp ;
                        }
                    }
                    
                    ele = config.SelectSingleNode ( "RegexTester/Options" ) 
                        as System.Xml.XmlElement ;
                    
                    if ( ele != null )
                    {                
                        att = ele.Attributes [ "RegexOptions" ] ;
                        
                        if
                        (
                            ( att != null )
                        &&
                            System.Int32.TryParse ( att.Value , out temp )
                        )
                        {
                            this.SetCurrentOptions ( (System.Text.RegularExpressions.RegexOptions) temp ) ;
                        }
                                    
                        att = ele.Attributes [ "Mode" ] ;
                        
                        if
                        (
                            ( att != null )
                        &&
                            System.Int32.TryParse ( att.Value , out temp )
                        )
                        {
                            this.SetCurrentMode ( (Mode) temp ) ;
                        }
                                    
                        att = ele.Attributes [ "ShowWhitespace" ] ;
                        
                        if
                        (
                            ( att != null )
                        )
                        {
                            this.cbShowWhitespace.Checked = att.Value == System.Boolean.TrueString ;
                        }
                    }
                    
                    ele = config.SelectSingleNode ( "RegexTester/Regex" ) 
                        as System.Xml.XmlElement ;
                    
                    if ( ele != null )
                    {                
                        this.tbRegex.Text = ele.InnerText ;
                    }
                                    
                    ele = config.SelectSingleNode ( "RegexTester/Input" ) 
                        as System.Xml.XmlElement ;
                    
                    if ( ele != null )
                    {                
                        this.tbInput.Text = ele.InnerText ;
                    }
                }
            }
            catch
            {
                /* If any errors occur while processing the config file, just ignore them */
            }
                            
            return ;
        }
        
        private void 
        frmRegexTester_FormClosing
        ( 
            object                                    sender 
        , 
            System.Windows.Forms.FormClosingEventArgs e
        )
        {
            try
            {
                System.Xml.XmlDocument  config = new System.Xml.XmlDocument() ;
                System.Xml.XmlElement   ele    ;
                System.Xml.XmlAttribute att    ;
                
                config.AppendChild ( ele = config.CreateElement ( "RegexTester" ) ) ;


                config.DocumentElement.AppendChild ( ele = config.CreateElement ( "Window" ) ) ;

                ele.Attributes.Append ( att = config.CreateAttribute ( "Maximized" ) ) ;
                
                att.Value = ( this.WindowState == System.Windows.Forms.FormWindowState.Maximized ).ToString() ;

                ele.Attributes.Append ( att = config.CreateAttribute ( "Top" ) ) ;
                
                att.Value = this.Top.ToString() ;

                ele.Attributes.Append ( att = config.CreateAttribute ( "Left" ) ) ;
                
                att.Value = this.Left.ToString() ;

                ele.Attributes.Append ( att = config.CreateAttribute ( "Height" ) ) ;
                
                att.Value = this.Height.ToString() ;

                ele.Attributes.Append ( att = config.CreateAttribute ( "Width" ) ) ;
                
                att.Value = this.Width.ToString() ;

                ele.Attributes.Append ( att = config.CreateAttribute ( "MainSplitter" ) ) ;
                
                att.Value = this.scMain.SplitterDistance.ToString() ;

                ele.Attributes.Append ( att = config.CreateAttribute ( "TopLeftSplitter" ) ) ;
                
                att.Value = this.scTopLeft.SplitterDistance.ToString() ;


                config.DocumentElement.AppendChild ( ele = config.CreateElement ( "Options" ) ) ;

                ele.Attributes.Append ( att = config.CreateAttribute ( "RegexOptions" ) ) ;
                
                /* Storing as int due to paranoia */
                att.Value = ((int) this.GetCurrentOptions()).ToString() ;

                ele.Attributes.Append ( att = config.CreateAttribute ( "Mode" ) ) ;
                
                /* Storing as int due to paranoia */
                att.Value = ((int) this.GetCurrentMode()).ToString() ;

                ele.Attributes.Append ( att = config.CreateAttribute ( "ShowWhitespace" ) ) ;
                
                att.Value = this.cbShowWhitespace.Checked.ToString() ;

                
                config.DocumentElement.AppendChild ( ele = config.CreateElement ( "Regex" ) ) ;

                ele.InnerText = this.tbRegex.Text ;
                
                config.DocumentElement.AppendChild ( ele = config.CreateElement ( "Input" ) ) ;

                ele.InnerText = this.tbInput.Text ;
                

                PIEBALD.Lib.LibXml.WriteXmlDocument
                (
                    config 
                , 
                    this.configfile 
                ) ;
            }
            catch
            {
                /* If any errors occur while processing the config file, just ignore them */
            }
                            
            return ;
        }

        private void 
        bGetResult_Click 
        ( 
            object           sender 
        , 
            System.EventArgs e 
        )
        {
            if ( this.bGetResult.Enabled )
            {
                this.bGetResult.Enabled = false ;
                
                this.tbResult.Text = "" ;
                
                GetResultControlBlock cb = new GetResultControlBlock
                (
                    this.bGetResult
                ,
                    this.GetCurrentOptions()
                ,
                    this.GetCurrentMode()
                ,
                    this.cbShowWhitespace.Checked
                ,
                    this.tbRegex.Text
                ,
                    this.tbInput.Text
                ,
                    this.DoEnable
                ,
                    this.DoAppendResult
                ) ;
                
                (new System.Threading.Thread ( GetResult )).Start ( cb ) ;
            }
            
            return ;
        }
    }
}
