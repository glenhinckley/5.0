
# region Heading

/**************************************************************************************************************/
/*                                                                                                            */
/*  frmRegexTester.Static.cs                                                                                  */
/*                                                                                                            */
/*  Static members of frmRegexTester.                                                                         */
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
        private static readonly System.Collections.Generic.Dictionary<Mode,string> templates ;
    
        private static readonly System.Collections.Generic.Dictionary<char,char> whitespace ;
        
        static frmRegexTester
        (
        )
        {
            templates = new System.Collections.Generic.Dictionary<Mode,string>() ;
            
            templates [ Mode.CSharp ] =
                @"
                namespace TextWrapper 
                {{
                    public static class TextWrapper 
                    {{
                        public const string Text = {0} ; 
                    }} 
                }}
                " ;
            
            templates [ Mode.VisualBasic ] =
                @"
                Namespace TextWrapper
                    Public Class TextWrapper
                        Public Shared Text As String = {0}
                    End Class
                End Namespace
                " ;
        
            whitespace = new System.Collections.Generic.Dictionary<char,char>() ;
            
            whitespace [ ' '  ] = 'Ⓢ' ;
            whitespace [ '\0' ] = '⓪' ;
            whitespace [ '\a' ] = 'Ⓐ' ;
            whitespace [ '\b' ] = 'Ⓑ' ;
            whitespace [ '\f' ] = 'Ⓕ' ;
            whitespace [ '\n' ] = 'Ⓝ' ;
            whitespace [ '\r' ] = 'Ⓡ' ;
            whitespace [ '\t' ] = 'Ⓣ' ;
            whitespace [ '\v' ] = 'Ⓥ' ;
        
            return ;
        }
        
        private static string
        WrapText
        (
            string Text
        ,
            Mode   Language
        )
        {
            string code = System.String.Format
            (
                templates [ Language ]
            ,
                Text
            ) ;

            System.Reflection.Assembly assm = PIEBALD.Lib.LibSys.Compile
            ( 
                code
            , 
                Language.ToString() 
            ) ;
            
            System.Type type = assm.GetType ( "TextWrapper.TextWrapper" ) ;
            
            System.Reflection.FieldInfo field = type.GetField
            ( 
                "Text" 
            ,
                System.Reflection.BindingFlags.Public 
                | 
                System.Reflection.BindingFlags.Static 
            ) ;

            return ( (System.String) field.GetValue ( null ) ) ;
        }

        private static string
        ReplaceWhitespace
        (
            string Text
        )
        {
            System.Text.StringBuilder result = 
                new System.Text.StringBuilder ( Text.Length ) ;
            
            foreach  (char ch in Text )
            {
                if ( whitespace.ContainsKey ( ch ) )
                {
                    result.Append ( whitespace [ ch ] ) ;
                }
                else
                {
                    result.Append ( ch ) ;
                }
            }
            
            return ( result.ToString() ) ;
        }

        private static void
        GetResult
        (
            object ControlBlock
        )
        {
            GetResultControlBlock controlblock = 
                ControlBlock as GetResultControlBlock ;
                
            try
            {
                controlblock.Enable ( controlblock.Control , false ) ;
            
                string regtext = controlblock.Regex ;
                
                switch ( controlblock.Mode )
                {
                    case Mode.AsIs :
                    {
                        break ;
                    }
                    
                    case Mode.Unescape :
                    {
                        /* replace "" with ", \\ with \, and \" with " */
                        regtext = regtext.Replace ( "\"\"" , "\"" ) ;
                        regtext = regtext.Replace ( "\\\\" , "\\" ) ;
                        regtext = regtext.Replace ( "\\\"" , "\"" ) ;
                        
                        break ;
                    }

                    default :
                    {
                        regtext = WrapText ( regtext , controlblock.Mode ) ;
                        
                        break ;
                    }
                }

                System.Text.RegularExpressions.Regex reg = 
                    new System.Text.RegularExpressions.Regex
                    (
                        regtext
                    ,
                        controlblock.Options
                    ) ;

                System.Text.RegularExpressions.MatchCollection matches = 
                    reg.Matches ( controlblock.Input ) ;
                
                if ( matches.Count == 0 )
                {
                    controlblock.Result ( "No matches" ) ;
                }
                else
                {
                    foreach 
                    ( 
                        System.Text.RegularExpressions.Match mat 
                    in 
                        matches 
                    )
                    {
                        controlblock.Result ( "Match: " ) ;

                        controlblock.Result
                        (
                            controlblock.ShowWhitespace ? 
                                ReplaceWhitespace ( mat.Value ) : 
                                mat.Value
                        ) ;
                        
                        foreach ( string grp in reg.GetGroupNames() )
                        {
                            controlblock.Result ( System.String.Format
                            (
                                "\r\n    Group {0}: "
                            ,
                                grp
                            ) ) ;
                            
                            controlblock.Result
                            (
                                controlblock.ShowWhitespace ? 
                                    ReplaceWhitespace ( mat.Groups [ grp ].Value ) : 
                                    mat.Groups [ grp ].Value
                            ) ;
                        }
                        
                        controlblock.Result ( "\r\n\r\n" ) ;
                    }
                } 
            }
            catch ( System.Exception err )
            {
                controlblock.Result ( err.ToString() ) ;
                
                if ( err.Data.Contains ( "Errors" ) )
                {
                    System.CodeDom.Compiler.CompilerErrorCollection errors = 
                        err.Data [ "Errors" ] as 
                        System.CodeDom.Compiler.CompilerErrorCollection ;
                    
                    if ( errors != null )
                    {
                        foreach 
                        ( 
                            System.CodeDom.Compiler.CompilerError error 
                        in 
                            errors 
                        )
                        {
                            controlblock.Result ( "\r\n" + error.ToString() ) ;
                        }
                    }
                }
            }   
            finally
            {
                controlblock.Enable ( controlblock.Control , true ) ;
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
