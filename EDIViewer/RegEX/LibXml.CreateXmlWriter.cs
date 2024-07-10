
# region Heading

/**************************************************************************************************************/
/*                                                                                                            */
/*  LibXml.CreateXmlWriter.cs                                                                                 */
/*                                                                                                            */
/*  Helps create XmlWriters                                                                                   */
/*                                                                                                            */
/*  This is free code, use it as you require. If you modify it please use your own namespace.                 */
/*                                                                                                            */
/*  If you like it or have suggestions for improvements please let me know at: PIEBALDconsult@aol.com         */
/*                                                                                                            */
/*  Modification history:                                                                                     */
/*  2009-02-03          Sir John E. Boucher     Created                                                       */
/*                                                                                                            */
/**************************************************************************************************************/

# endregion

namespace PIEBALD.Lib
{
    partial class LibXml
    {
        private static class 
        XmlWriterSettings
        {
            public static readonly System.Xml.XmlWriterSettings Settings ;
            
            static XmlWriterSettings
            (
            )
            {
                Settings = new System.Xml.XmlWriterSettings() ;

                Settings.Indent = true ;
                Settings.Encoding = System.Text.Encoding.UTF8 ;
                Settings.OmitXmlDeclaration = true ;
                Settings.CheckCharacters = false ;

                return ;
            }
        }

/**************************************************************************************************************/
            
        /** 
        <summary>
            Helps create XmlWriters.
        </summary>
        <param name="File">
            The name of the file to write.
        </param>
        <param name="Settings">
            An instance of System.Xml.XmlWriterSettings with the settings to use.
        </param>
        <returns>
            An XmlWriter that uses the settings of my choice.
        </returns>
        */
        public static System.Xml.XmlWriter
        CreateXmlWriter
        (
            string                       File
        ,
            System.Xml.XmlWriterSettings Settings
        )
        {
            return ( System.Xml.XmlWriter.Create 
            ( 
                System.Environment.ExpandEnvironmentVariables ( File )
            , 
                Settings 
            ) ) ;
        }

/**************************************************************************************************************/

        /** 
        <summary>
            Helps create XmlWriters.
        </summary>
        <param name="File">
            Information about the file write.
        </param>
        <param name="Settings">
            An instance of System.Xml.XmlWriterSettings with the settings to use.
        </param>
        <returns>
            An XmlWriter that uses the settings of my choice.
        </returns>
        */
        public static System.Xml.XmlWriter
        CreateXmlWriter
        (
            System.IO.FileInfo           File
        ,
            System.Xml.XmlWriterSettings Settings
        )
        {
            return ( CreateXmlWriter ( File.FullName , Settings ) ) ;
        }

/**************************************************************************************************************/
        
        /** 
        <summary>
            Helps create XmlWriters.
        </summary>
        <param name="Stream">
            A stream to wrap in an XmlWriter.
        </param>
        <param name="Settings">
            An instance of System.Xml.XmlWriterSettings with the settings to use.
        </param>
        <returns>
            An XmlWriter that uses the settings of my choice.
        </returns>
        */
        public static System.Xml.XmlWriter
        CreateXmlWriter
        (
            System.IO.Stream             Stream
        ,
            System.Xml.XmlWriterSettings Settings
        )
        {
            return ( System.Xml.XmlWriter.Create 
            ( 
                Stream 
            , 
                Settings 
            ) ) ;
        }

/**************************************************************************************************************/
        
        /** 
        <summary>
            Helps create XmlWriters.
        </summary>
        <param name="File">
            A TextWriter to wrap in an XmlWriter.
        </param>
        <param name="Settings">
            An instance of System.Xml.XmlWriterSettings with the settings to use.
        </param>
        <returns>
            An XmlWriter that uses the settings of my choice.
        </returns>
        */
        public static System.Xml.XmlWriter
        CreateXmlWriter
        (
            System.IO.TextWriter         File
        ,
            System.Xml.XmlWriterSettings Settings
        )
        {
            return ( System.Xml.XmlWriter.Create 
            ( 
                File 
            , 
                Settings 
            ) ) ;
        }

/**************************************************************************************************************/
        
        /** 
        <summary>
            Helps create XmlWriters.
        </summary>
        <param name="File">
            An XmlTextWriter to wrap in an XmlWriter.
        </param>
        <param name="Settings">
            An instance of System.Xml.XmlWriterSettings with the settings to use.
        </param>
        <returns>
            An XmlWriter that uses the settings of my choice.
        </returns>
        */
        public static System.Xml.XmlWriter
        CreateXmlWriter
        (
            System.Xml.XmlTextWriter     File
        ,
            System.Xml.XmlWriterSettings Settings
        )
        {
            return ( System.Xml.XmlWriter.Create 
            ( 
                File 
            , 
                Settings 
            ) ) ;
        }
    }
}
