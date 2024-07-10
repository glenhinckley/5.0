Imports System.Text.RegularExpressions
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Collections.Generic
'Imports System.Data.DataSetExtensions

Imports System.IO
Imports System.IO.File
Imports System.Text
Imports System.Threading

Imports DCSGlobal.BusinessRules.Logging
Imports DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff



Namespace DCSGlobal.RealAlert




    Public Class GetSettings
        Implements IDisposable

        Private log As New logExecption()
        Private sch As New SchedulerLog()
        Private ss As New StringStuff()
        Dim RA_TIMEOUT_MINUTES As String = "1"

        Private _ConnectionString As String = String.Empty

        Public WriteOnly Property ConnectionString As String

            Set(value As String)
                _ConnectionString = value
                log.ConnectionString = value
                sch.ConnectionString = value
            End Set
        End Property


        Public WriteOnly Property dbTimeOut As String

            Set(value As String)

            End Set
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sOperatorID"></param>
        ''' <param name="sRaUID"></param>
        ''' <param name="sRaCookie"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getRealAlertIP(ByVal sOperatorID As String, ByRef sRaUID As String, ByRef sRaCookie As String) As String
            Dim sqlConn As SqlConnection = New SqlConnection
            '  Dim ds As DataSet
            Dim ruleComm As SqlCommand
            Dim ruleReader As SqlDataReader
            Dim sqlString As String = String.Empty
            'Dim bUpdate As Boolean
            Dim errMessage As String = String.Empty
            Dim sReturnIP As String = String.Empty
            Dim sActiveStatus As String = String.Empty

            If RA_TIMEOUT_MINUTES.Trim = "1" Then RA_TIMEOUT_MINUTES = "240" ' subba-020508

            Try
                If (sqlConn.State = ConnectionState.Closed) Then
                    sqlConn.ConnectionString = _ConnectionString
                    sqlConn.Open()
                End If

                'sqlString = "select user_id,IP_Address,cookie from ra_login where operator_id='" + sOperatorID + "'"
                sqlString = "select user_id,IP_Address,cookie,active_status from ra_login where operator_id='" + sOperatorID + "' and DATEDIFF(mi, modified_date, getdate()) < " + RA_TIMEOUT_MINUTES
                ruleComm = New SqlCommand(sqlString, sqlConn)
                ruleComm.CommandType = CommandType.Text 'CommandType.StoredProcedure

                ruleReader = ruleComm.ExecuteReader()

                While ruleReader.Read()
                    If Not IsDBNull(Trim(ruleReader.GetValue(0))) Then
                        sRaUID = CStr(ruleReader.GetValue(0))
                    End If
                    If Not IsDBNull(Trim(ruleReader.GetValue(1))) Then
                        sReturnIP = CStr(ruleReader.GetValue(1))
                    End If
                    If Not IsDBNull(Trim(ruleReader.GetValue(2))) Then
                        sRaCookie = CStr(ruleReader.GetValue(2))
                    End If
                    If Not IsDBNull(Trim(ruleReader.GetValue(ruleReader.GetOrdinal("active_status")))) Then
                        sActiveStatus = ruleReader.GetValue(ruleReader.GetOrdinal("active_status"))
                    End If

                End While

                If (sActiveStatus = "N") Then sReturnIP = String.Empty

            Catch ex As Exception
                errMessage = ex.Message
                sReturnIP = String.Empty
            Finally
                ruleComm = Nothing
                sqlConn.Close()
                sqlConn = Nothing
            End Try
            Return sReturnIP

        End Function

        Public Function getRaTimeoutMinutes() As String

            Dim strRaTimeoutMinutes As String = "1"
            Dim SysTpComm As SqlCommand
            Dim sqlConn As SqlConnection = New SqlConnection

            Try
                Dim SysPrefReader As SqlDataReader
                Dim sqlString As String

                If (sqlConn.State = ConnectionState.Closed) Then
                    sqlConn.ConnectionString = _ConnectionString
                    sqlConn.Open()
                End If

                sqlString = "select lov_text, lov_value from system_preferences where module_name = 'ConsoleFireRules' and lov_name = 'RaTimeoutMinutes'"
                SysTpComm = New SqlCommand(sqlString, sqlConn)
                SysPrefReader = SysTpComm.ExecuteReader()

                While SysPrefReader.Read()
                    If Not IsDBNull(Trim(SysPrefReader.GetValue(1))) Then
                        strRaTimeoutMinutes = Trim(SysPrefReader.GetValue(1))
                    End If
                End While

            Catch ex As Exception
                strRaTimeoutMinutes = "0"
            Finally
                SysTpComm = Nothing
                sqlConn.Close()
                sqlConn = Nothing
            End Try

            Return strRaTimeoutMinutes

        End Function


#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
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
#End Region

    End Class
End Namespace