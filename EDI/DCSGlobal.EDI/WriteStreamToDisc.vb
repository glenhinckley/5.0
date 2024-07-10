Option Explicit On




Imports System.Text.RegularExpressions
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Collections.Generic

Imports System.Threading
Imports System.Collections


Imports DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff
Imports DCSGlobal.BusinessRules.CoreLibrary.IO
Imports DCSGlobal.BusinessRules.Logging




Public Class WriteStreamToDisc
    Implements IDisposable

    ' Keep track of when the object is disposed. 
    Protected disposed As Boolean = False







    Private log As New logExecption
    Private ss As New StringStuff
    Private _ConnectionString As String = String.Empty


    ' This method disposes the base object's resources. 
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposed Then
            If disposing Then


                log = Nothing
                'email = Nothing
                ' Insert code to free managed resources. 
            End If
            ' Insert code to free unmanaged resources. 
        End If
        Me.disposed = True
    End Sub


    ' Do not change or add Overridable to these methods. 
    ' Put cleanup code in Dispose(ByVal disposing As Boolean). 
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub



    Public WriteOnly Property ConnectionString As String

        Set(value As String)
            _ConnectionString = value
            log.ConnectionString = value
        End Set
    End Property








End Class
