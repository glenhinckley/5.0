
/**************************************************************************************************************/
/*                                                                                                            */
/*  LibXml.WriteXmlDocument.cs                                                                                */
/*                                                                                                            */
/*  Writes an XmlDocument to the specified file                                                               */
/*                                                                                                            */
/*  Modification history:                                                                                     */
/*  2007-05-08          Sir John E. Boucher     Created                                                       */
/*                                                                                                            */
/**************************************************************************************************************/

namespace PIEBALD.Lib
{
    public static partial class LibXml
    {
        public static void
        WriteXmlDocument
        (
            System.Xml.XmlDocument Doc
        ,
            System.IO.FileInfo     File
        )
        {
            WriteXmlDocument 
            ( 
                Doc 
            , 
                File.FullName 
            ) ;
            
            return ;
        } 

        public static void
        WriteXmlDocument
        (
            System.Xml.XmlDocument Doc
        ,
            string                 File
        )
        {
            WriteXmlDocument 
            ( 
                Doc 
            , 
                File 
            , 
                PIEBALD.Lib.LibXml.XmlWriterSettings.Settings 
            ) ;
            
            return ;
        } 

        public static void
        WriteXmlDocument
        (
            System.Xml.XmlDocument       Doc
        ,
            System.IO.FileInfo           File
        ,
            System.Xml.XmlWriterSettings Settings
        )
        {
            WriteXmlDocument 
            (  
                Doc 
            , 
                File.FullName 
            , 
                Settings 
            ) ;
            
            return ;
        } 

        public static void
        WriteXmlDocument
        (
            System.Xml.XmlDocument       Doc
        ,
            string                       File
        ,
            System.Xml.XmlWriterSettings Settings
        )
        {
            using 
            ( 
                System.Xml.XmlWriter writer 
            = 
                PIEBALD.Lib.LibXml.CreateXmlWriter
                ( 
                    File
                , 
                    Settings 
                ) 
            )
            {
                Doc.WriteTo ( writer ) ;
            }
            
            return ;
        } 
    }
}
