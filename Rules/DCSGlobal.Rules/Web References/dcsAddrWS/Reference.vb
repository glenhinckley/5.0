﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Xml.Serialization

'
'This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
'
Namespace dcsAddrWS
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Web.Services.WebServiceBindingAttribute(Name:="DcsAddressSoap", [Namespace]:="http://www.dcsglobal.com/AddressService/")>  _
    Partial Public Class DcsAddress
        Inherits System.Web.Services.Protocols.SoapHttpClientProtocol
        
        Private VerifyAddressEnhancedOperationCompleted As System.Threading.SendOrPostCallback
        
        Private useDefaultCredentialsSetExplicitly As Boolean
        
        '''<remarks/>
        Public Sub New()
            MyBase.New
            Me.Url = Global.DCSGlobal.Rules.My.MySettings.Default.DCSGlobal_Rules_DcsAddress_DcsAddress
            If (Me.IsLocalFileSystemWebService(Me.Url) = true) Then
                Me.UseDefaultCredentials = true
                Me.useDefaultCredentialsSetExplicitly = false
            Else
                Me.useDefaultCredentialsSetExplicitly = true
            End If
        End Sub
        
        Public Shadows Property Url() As String
            Get
                Return MyBase.Url
            End Get
            Set
                If (((Me.IsLocalFileSystemWebService(MyBase.Url) = true)  _
                            AndAlso (Me.useDefaultCredentialsSetExplicitly = false))  _
                            AndAlso (Me.IsLocalFileSystemWebService(value) = false)) Then
                    MyBase.UseDefaultCredentials = false
                End If
                MyBase.Url = value
            End Set
        End Property
        
        Public Shadows Property UseDefaultCredentials() As Boolean
            Get
                Return MyBase.UseDefaultCredentials
            End Get
            Set
                MyBase.UseDefaultCredentials = value
                Me.useDefaultCredentialsSetExplicitly = true
            End Set
        End Property
        
        '''<remarks/>
        Public Event VerifyAddressEnhancedCompleted As VerifyAddressEnhancedCompletedEventHandler
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.dcsglobal.com/AddressService/VerifyAddressEnhanced", RequestNamespace:="http://www.dcsglobal.com/AddressService/", ResponseNamespace:="http://www.dcsglobal.com/AddressService/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function VerifyAddressEnhanced(ByVal Address1 As String, ByVal Address2 As String, ByVal City As String, ByVal State As String, ByVal ZipCode As String, ByVal CustomerID As String, ByVal LicenseKey As String) As AESRawAddress()
            Dim results() As Object = Me.Invoke("VerifyAddressEnhanced", New Object() {Address1, Address2, City, State, ZipCode, CustomerID, LicenseKey})
            Return CType(results(0),AESRawAddress())
        End Function
        
        '''<remarks/>
        Public Overloads Sub VerifyAddressEnhancedAsync(ByVal Address1 As String, ByVal Address2 As String, ByVal City As String, ByVal State As String, ByVal ZipCode As String, ByVal CustomerID As String, ByVal LicenseKey As String)
            Me.VerifyAddressEnhancedAsync(Address1, Address2, City, State, ZipCode, CustomerID, LicenseKey, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub VerifyAddressEnhancedAsync(ByVal Address1 As String, ByVal Address2 As String, ByVal City As String, ByVal State As String, ByVal ZipCode As String, ByVal CustomerID As String, ByVal LicenseKey As String, ByVal userState As Object)
            If (Me.VerifyAddressEnhancedOperationCompleted Is Nothing) Then
                Me.VerifyAddressEnhancedOperationCompleted = AddressOf Me.OnVerifyAddressEnhancedOperationCompleted
            End If
            Me.InvokeAsync("VerifyAddressEnhanced", New Object() {Address1, Address2, City, State, ZipCode, CustomerID, LicenseKey}, Me.VerifyAddressEnhancedOperationCompleted, userState)
        End Sub
        
        Private Sub OnVerifyAddressEnhancedOperationCompleted(ByVal arg As Object)
            If (Not (Me.VerifyAddressEnhancedCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent VerifyAddressEnhancedCompleted(Me, New VerifyAddressEnhancedCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        Public Shadows Sub CancelAsync(ByVal userState As Object)
            MyBase.CancelAsync(userState)
        End Sub
        
        Private Function IsLocalFileSystemWebService(ByVal url As String) As Boolean
            If ((url Is Nothing)  _
                        OrElse (url Is String.Empty)) Then
                Return false
            End If
            Dim wsUri As System.Uri = New System.Uri(url)
            If ((wsUri.Port >= 1024)  _
                        AndAlso (String.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) = 0)) Then
                Return true
            End If
            Return false
        End Function
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9037.0"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.dcsglobal.com/AddressService/")>  _
    Partial Public Class AESRawAddress
        
        Private diagnosticsField As String
        
        Private line2Field As String
        
        Private addressField As String
        
        Private cszField As String
        
        Private barcodeField As String
        
        Private cityField As String
        
        Private stateField As String
        
        Private zipField As String
        
        Private errorField As String
        
        '''<remarks/>
        Public Property diagnostics() As String
            Get
                Return Me.diagnosticsField
            End Get
            Set
                Me.diagnosticsField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property line2() As String
            Get
                Return Me.line2Field
            End Get
            Set
                Me.line2Field = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property address() As String
            Get
                Return Me.addressField
            End Get
            Set
                Me.addressField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property csz() As String
            Get
                Return Me.cszField
            End Get
            Set
                Me.cszField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property barcode() As String
            Get
                Return Me.barcodeField
            End Get
            Set
                Me.barcodeField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property city() As String
            Get
                Return Me.cityField
            End Get
            Set
                Me.cityField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property state() As String
            Get
                Return Me.stateField
            End Get
            Set
                Me.stateField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property zip() As String
            Get
                Return Me.zipField
            End Get
            Set
                Me.zipField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property [error]() As String
            Get
                Return Me.errorField
            End Get
            Set
                Me.errorField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0")>  _
    Public Delegate Sub VerifyAddressEnhancedCompletedEventHandler(ByVal sender As Object, ByVal e As VerifyAddressEnhancedCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9037.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class VerifyAddressEnhancedCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As AESRawAddress()
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),AESRawAddress())
            End Get
        End Property
    End Class
End Namespace
