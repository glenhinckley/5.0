
# region Heading

/**************************************************************************************************************/
/*                                                                                                            */
/*  LibSys.Compile.cs                                                                                         */
/*                                                                                                            */
/*  Uses a CodeDomProvider to compile the code contained in a string.                                         */
/*                                                                                                            */
/*  This is free code, use it as you require.                                                                 */
/*  If you modify it please use your own namespace.                                                           */
/*                                                                                                            */
/*  If you like it or have suggestions for improvements please let me know at: PIEBALDconsult@aol.com         */
/*                                                                                                            */
/*  Modification history:                                                                                     */
/*  2009-08-14          Sir John E. Boucher     Created                                                       */
/*                                                                                                            */
/**************************************************************************************************************/

# endregion

namespace PIEBALD.Lib
{
    public static partial class LibSys
    {
        /**
        <summary>
            Uses a CodeDomProvider to compile the code contained in a string.
        </summary>
        <remarks>
            Requires a reference to System.Runtime.Remoting.
        </remarks>
        <param name="Code">
            A string containing program code.
        </param>
        <param name="Language">
            A string containing the name of the language to use.
            The available languages vary by system (try CSharp).
        </param>
        <param name="ReferencedAssemblies">
            An optional list of assemblies to reference.
            System.dll will be referenced automatically.
        </param>
        <returns>
            An Assembly containing the compiled code.
            The Assembly will be loaded in the current AppDomain.
        </returns>
        <exception cref="System.ArgumentException">
            If the Code or Language parameter is null or empty or the Language is not known on the system.
        </exception>
        <exception cref="System.Exception">
            If the compiler for the language fails to compile the code.
            The Errors and Output from the compiler will be in the Data property of the Exception.
        </exception>
        */
        public static System.Reflection.Assembly
        Compile
        (
            string          Code
        ,
            string          Language
        ,
            params string[] ReferencedAssemblies
        )
        {
            if ( System.String.IsNullOrEmpty ( Code ) )
            {
                throw ( new System.ArgumentException 
                    ( "You must supply some code" , "Code" ) ) ;
            }

            if ( System.String.IsNullOrEmpty ( Language ) )
            {
                throw ( new System.ArgumentException 
                    ( "You must supply the name of a known language" , "Language" ) ) ;
            }

            if ( !System.CodeDom.Compiler.CodeDomProvider.IsDefinedLanguage ( Language ) )
            {
                throw ( new System.ArgumentException 
                    ( "That language is not known on this system" , "Language" ) ) ;
            }
            
            using 
            (
                System.CodeDom.Compiler.CodeDomProvider cdp 
            = 
                System.CodeDom.Compiler.CodeDomProvider.CreateProvider ( Language )
            )
            {
                System.CodeDom.Compiler.CompilerParameters cp = 
                    System.CodeDom.Compiler.CodeDomProvider.GetCompilerInfo 
                        ( Language ).CreateDefaultCompilerParameters() ;

                cp.GenerateInMemory = true ;
            
                cp.TreatWarningsAsErrors = true ;
                
                cp.WarningLevel = 4 ;
                
                cp.ReferencedAssemblies.Add ( "System.dll" ) ;
                
                if 
                ( 
                    ( ReferencedAssemblies != null ) 
                && 
                    ( ReferencedAssemblies.Length > 0 ) 
                )
                {
                    cp.ReferencedAssemblies.AddRange ( ReferencedAssemblies ) ;
                }

                System.CodeDom.Compiler.CompilerResults cr = 
                    cdp.CompileAssemblyFromSource
                    (
                        cp 
                    , 
                        Code 
                    ) ;
                
                if ( cr.Errors.HasErrors )
                {
                    System.Exception err = new System.Exception ( "Compilation failure" ) ;
                    
                    err.Data [ "Errors" ] = cr.Errors ;
                    
                    err.Data [ "Output" ] = cr.Output ;
                    
                    throw ( err ) ;
                }

                return ( cr.CompiledAssembly ) ;
            }
        }
    }
}
