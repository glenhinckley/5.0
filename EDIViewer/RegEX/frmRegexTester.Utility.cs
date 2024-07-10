
# region Heading

/**************************************************************************************************************/
/*                                                                                                            */
/*  RegexTester.Utility.cs                                                                                    */
/*                                                                                                            */
/*  Utility members of frmRegexTester.                                                                        */
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
    partial class frmRegexTester
    {
        protected override void 
        Dispose 
        ( 
            bool disposing 
        )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose() ;
            }
 
            base.Dispose ( disposing ) ;
            
            return ;
        }

        private System.Text.RegularExpressions.RegexOptions
        GetCurrentOptions
        (
        )
        {
            System.Text.RegularExpressions.RegexOptions result = 
                System.Text.RegularExpressions.RegexOptions.None ;

            foreach 
            ( 
                System.Text.RegularExpressions.RegexOptions opt 
            in 
                this.clbOptions.CheckedItems 
            )
            {
                result |= opt ;
            }
                        
            return ( result ) ;
        }

        private void
        SetCurrentOptions
        (
            System.Text.RegularExpressions.RegexOptions Value
        )
        {
            for ( int i = 0 ; i < this.clbOptions.Items.Count ; i++ )
            {
                System.Text.RegularExpressions.RegexOptions opt = 
                    (System.Text.RegularExpressions.RegexOptions) this.clbOptions.Items [ i ] ;
                    
                this.clbOptions.SetItemChecked ( i , ( opt & Value ) == opt ) ;
            }
                        
            return ;
        }
        
        private Mode
        GetCurrentMode
        (
        )
        {
            return ( (Mode) this.cbMode.SelectedItem ) ;
        }
        
        private void
        SetCurrentMode
        (
            Mode Value
        )
        {
            this.cbMode.SelectedIndex = 0 ;

            for ( int i = 0 ; i < this.cbMode.Items.Count ; i++ )
            {
                if ( (Mode) this.cbMode.Items [ i ] == Value )
                {
                    this.cbMode.SelectedIndex = i ;
                    
                    break ;
                }
            }
            
            return ;
        }

        private void 
        GiveHelp 
        ( 
            object                             sender 
        , 
            System.Windows.Forms.HelpEventArgs e 
        )
        {
            System.Windows.Forms.MessageBox.Show
            (
                "RegexTester  V0.0\n\n" +
                "Copyright 2009 Sir John E. Boucher\n\n" +
                "A test bed for .net Regular Expressions"
            ,
                "About RegexTester"
            ,
                System.Windows.Forms.MessageBoxButtons.OK
            ,  
                System.Windows.Forms.MessageBoxIcon.Information
            ) ;
        
            return ;
        }
        
        private void 
        Enable
        (
            System.Windows.Forms.Control Control
        ,
            bool                         Enable
        )
        {
            if ( Control != null )
            {
                if ( this.InvokeRequired )
                {
                    this.Invoke 
                    ( 
                        this.DoEnable
                    , 
                        new object[] { Control , Enable } 
                    ) ;
                }
                else
                {
                    Control.Enabled = Enable ;
                }
            }
                        
            return ;
        }

        private void 
        AppendResult
        (
            string Result
        )
        {
            if ( !System.String.IsNullOrEmpty ( Result ) )
            {
                if ( this.InvokeRequired )
                {
                    this.Invoke 
                    ( 
                        this.DoAppendResult
                    , 
                        new string[] { Result } 
                    ) ;
                }
                else
                {
                    this.tbResult.Text += Result ;
                }
            }
                                    
            return ;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // frmRegexTester
            // 
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Name = "frmRegexTester";
            this.Load += new System.EventHandler(this.frmRegexTester_Load);
            this.ResumeLayout(false);

        }

        private void frmRegexTester_Load(object sender, System.EventArgs e)
        {

        }
    }
}
