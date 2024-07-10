Option Explicit On
Imports System.Xml


Public Class XML

    Dim input As String
    Dim inputxsdns As String
    Dim xml As String 'to store path of xml file
    Dim xsd As String ' To store path of xsd file
 

    ' Function to verify file exist or not



    ' Function to load and validate xml against XSD
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="byfileName"></param>
    ''' <param name="xsd"></param>
    ''' <param name="xsdns"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function LoadXMLDocument(ByVal byfileName As String, ByVal xsd As String, ByVal xsdns As String) As Integer

        Dim r As Integer = -1

        Dim objDOM
        ' Dim objSchema
        Dim loadStatus()
        Dim xmlParseErr, objXMLDocSchemaCache, xmlXSDError

        'Create Object for MSXML
        objDOM = CreateObject("MSXML2.DOMDocument.6.0")
        objXMLDocSchemaCache = CreateObject("Msxml2.XMLSchemaCache.6.0")

        'Add the XSD file to SchemCache object
        objXMLDocSchemaCache.add(xsdns, xsd)

        'Set the DOM variables
        objDOM.async = False
        objDOM.validateOnParse = True
        objDOM.resolveExternals = True

        'Set the loaded XSD to DOM and load XML to DOM
        objDOM.schemas = objXMLDocSchemaCache
        loadStatus = objDOM.Load(byfileName)
        xmlParseErr = objDOM.parseError

        'verify that XML is validated aganist Schema and loaded successfully
        If xmlParseErr.errorCode <> 0 Then
            MsgBox(xmlParseErr.reason, vbCritical)
        Else
            xmlXSDError = objDOM.validate
            If xmlXSDError.errorCode <> 0 Then
                r = xmlXSDError.errorCode
            Else
                r = 0
            End If
        End If

        objDOM = Nothing
        objXMLDocSchemaCache = Nothing

        Return r

    End Function

End Class
