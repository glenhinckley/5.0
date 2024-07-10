
# region Heading

/**************************************************************************************************************/
/*                                                                                                            */
/*  frmRegexTester.Types.cs                                                                                   */
/*                                                                                                            */
/*  Types for frmRegexTester.                                                                                 */
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
        private enum Mode
        {
            AsIs
        ,
            Unescape
        ,
            CSharp
        ,
            VisualBasic
        }

        private delegate void EnableDelegate 
        ( 
            System.Windows.Forms.Control Control 
        , 
            bool                         Enable 
        ) ;

        private delegate void AppendResultDelegate
        ( 
            string Result
        ) ;
        
        private sealed class GetResultControlBlock
        {
            public readonly System.Windows.Forms.Control                Control        ;
            public readonly System.Text.RegularExpressions.RegexOptions Options        ;
            public readonly Mode                                        Mode           ;
            public readonly bool                                        ShowWhitespace ;
            public readonly string                                      Regex          ;
            public readonly string                                      Input          ;
            public readonly EnableDelegate                              Enable         ;
            public readonly AppendResultDelegate                        Result         ;
            
            public GetResultControlBlock
            (
                System.Windows.Forms.Control                Control        
            ,
                System.Text.RegularExpressions.RegexOptions Options        
            ,
                Mode                                        Mode           
            ,
                bool                                        ShowWhitespace 
            ,
                string                                      Regex          
            ,
                string                                      Input          
            ,
                EnableDelegate                              Enable
            ,
                AppendResultDelegate                        Result
            )
            {
                this.Control        = Control        ;
                this.Options        = Options        ;
                this.Mode           = Mode           ;
                this.ShowWhitespace = ShowWhitespace ;
                this.Regex          = Regex          ;
                this.Input          = Input          ;
                this.Enable         = Enable         ;
                this.Result         = Result         ;
            
                return ;
            }
        }
    }
}
