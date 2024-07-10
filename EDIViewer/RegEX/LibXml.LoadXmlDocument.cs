
# region Heading

/**************************************************************************************************************/
/*                                                                                                            */
/*  LibXml.LoadXmlDocument.cs                                                                                 */
/*                                                                                                            */
/*  Helps load XmlDocuments                                                                                   */
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
        XmlReaderSettings
        {
            public static readonly System.Xml.XmlReaderSettings Settings ;

            static XmlReaderSettings
            (
            )
            {
                Settings = new System.Xml.XmlReaderSettings() ;

                System.Xml.XmlUrlResolver resolver = new System.Xml.XmlUrlResolver() ;

                resolver.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials ;

                Settings.XmlResolver = resolver ;

                Settings.ValidationType = System.Xml.ValidationType.Schema ;
                Settings.ProhibitDtd = false ;

                return ;
            }
        }

/**************************************************************************************************************/
        
        /** 
        <summary>
            Helps load XmlDocuments.
        </summary>
        <param name="Source">
            The URI of the document to load.
        </param>
        <returns>
            The XmlDocument that was loaded.
        </returns>
        */
        public static System.Xml.XmlDocument
        LoadXmlDocument
        (
            string Source
        )
        {
            using 
            ( 
                System.Xml.XmlReader reader 
            = 
                System.Xml.XmlReader.Create
                ( 
                    System.Environment.ExpandEnvironmentVariables ( Source )
                , 
                    XmlReaderSettings.Settings 
                )
            )
            {            
                System.Xml.XmlDocument result = new System.Xml.XmlDocument() ;
                
                result.Load ( reader ) ;
                
                reader.Close() ;
            
                return ( result ) ;
            }
        }

/**************************************************************************************************************/
        
        /** 
        <summary>
            Helps load XmlDocuments.
        </summary>
        <param name="Source">
            The URI of the document to load.
        </param>
        <returns>
            The XmlDocument that was loaded.
        </returns>
        */
        public static System.Xml.XmlDocument
        LoadXmlDocument
        (
            System.Uri Source
        )
        {
            return ( LoadXmlDocument ( Source.AbsoluteUri ) ) ;
        }

/**************************************************************************************************************/
        
        /** 
        <summary>
            Helps load XmlDocuments.
        </summary>
        <param name="Source">
            Information about a file from which to load a document.
        </param>
        <returns>
            The XmlDocument that was loaded.
        </returns>
        */
        public static System.Xml.XmlDocument
        LoadXmlDocument
        (
            System.IO.FileInfo Source
        )
        {
            return ( LoadXmlDocument ( Source.FullName ) ) ;
        }

/**************************************************************************************************************/
        
        /** 
        <summary>
            Helps load XmlDocuments.
        </summary>
        <param name="Source">
            A TextReader from which to load the document.
        </param>
        <returns>
            The XmlDocument that was loaded.
        </returns>
        */
        public static System.Xml.XmlDocument
        LoadXmlDocument
        (
            System.IO.TextReader Source
        )
        {
            using 
            ( 
                System.Xml.XmlReader reader 
            = 
                System.Xml.XmlReader.Create 
                ( 
                    Source 
                , 
                    XmlReaderSettings.Settings 
                )
            )
            {            
                System.Xml.XmlDocument result = new System.Xml.XmlDocument() ;
                
                result.Load ( reader ) ;
                
                reader.Close() ;
            
                return ( result ) ;
            }
        }
    }
}
