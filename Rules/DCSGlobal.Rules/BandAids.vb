

Option Explicit On

Imports System.Collections.Specialized
Imports System.Text.RegularExpressions
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Net.Security
Imports System.Threading
Imports System.Timers
Imports System.Web
Imports System.Net
Imports System.IO
Imports System.Data
Imports System.Data.SqlTypes
Imports System.Reflection

Imports System.Diagnostics
Imports System.Globalization


Imports System.IO.Pipes
Imports System.Security.Principal
Imports System


Imports System.Collections
Imports System.Text

Imports DCSGlobal.BusinessRules.Logging
Imports DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff
Imports DCSGlobal.BusinessRules.CoreLibraryII
Imports DCSGlobal.RealAlert.DCSGlobal.RealAlert

Public Class BandAids

    Implements IDisposable

    Private disposedValue As Boolean ' To detect redundant calls

    Private log As New logExecption()


    Private _Verbose As Integer = 1





    Private _ConnectionString As String = String.Empty






    Dim _AppName As String = "Rule BandAids"


    Dim _usp_rules_non_processed_rows As String = String.Empty


    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).

                log = Nothing
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    '' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Public WriteOnly Property ConnectionString As String

        Set(value As String)
            _ConnectionString = value
            log.ConnectionString = value


        End Set
    End Property


    Public WriteOnly Property AppName As String

        Set(value As String)
            _AppName = value
        End Set
    End Property


    Public WriteOnly Property usp_rules_non_processed_rows As String

        Set(value As String)
            _usp_rules_non_processed_rows = value


        End Set
    End Property


    Public Function uspRulesNonProcessedRows(ByVal RoleID As Integer, ByVal PatHouseCode As String) As Integer

        Dim r As Integer = -1

        Try

            Using Con As New SqlConnection(_ConnectionString)
                Con.Open()
                Using ruleComm As New SqlCommand(_usp_rules_non_processed_rows, Con)
                    ruleComm.CommandType = CommandType.StoredProcedure

                    ruleComm.Parameters.Add("@pat_hosp_code", SqlDbType.VarChar).Value = PatHouseCode
                    ruleComm.Parameters.Add("@id", SqlDbType.Int).Value = RoleID
    

                    ruleComm.ExecuteNonQuery()
                End Using
                Con.Close()
            End Using


        Catch ex As Exception
            log.ExceptionDetails(_AppName, ex)
        End Try



        Return r

    End Function


End Class
