Option Explicit On
Option Strict On



Imports DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff
Imports DCSGlobal.BusinessRules.CoreLibrary.IO

Imports System.Text.RegularExpressions
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Collections.Generic

Imports System.Threading
Imports System.Collections

Imports System.IO




Imports DCSGlobal.BusinessRules.Logging



Namespace DCSGlobal.EDI
    Public Class WriteEDIToDisk
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



        Public Function ByString(ByVal RawEDI As String, ByVal FilePath As String, ByVal FileName As String) As Integer
            Dim r = -1
            Dim Path As String = String.Empty


            Path = FilePath + FileName



            Try
                If File.Exists(Path) = False Then

                    ' Create a file to write to.
                    Dim createText As String = RawEDI
                    File.WriteAllText(Path, createText)
                    r = 0
                Else
                    r = -2

                End If


            Catch ex As Exception


                log.ExceptionDetails("WriteEDIToDisk", ex)
                r = -3


            End Try





            Return r
        End Function


        Public Function ByList(ByVal EDIList As List(Of String), ByVal FilePath As String, ByVal FileName As String) As Integer
            Dim r = -1
            Dim Path As String = String.Empty

            Path = FilePath + FileName



            Try
                If File.Exists(Path) = False Then

                    ' Create a file to write to.
                    '   Dim createText As String = RawEDI
                    '   File.WriteAllText(Path, createText)
                Else
                    r = -2

                End If


            Catch ex As Exception


                log.ExceptionDetails("WriteEDIToDisk", ex)
                r = -3


            End Try





            Return r
        End Function


    End Class
End Namespace
