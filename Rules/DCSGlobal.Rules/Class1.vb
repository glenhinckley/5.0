Option Strict On
Option Explicit On

Imports System
Imports System.Data.DataColumn
Imports System.Data

Imports System.Data.SqlClient
Imports System.IO
Imports System.Text.RegularExpressions
Imports MSScriptControl.ScriptControlClass
Imports System.Collections
Imports System.Text



Imports System.Xml
Imports System.Xml.XPath
Imports System.IO.TextReader
Imports System.IO.Pipes
Imports System.Security.Principal

Imports DCSGlobal.BusinessRules.Logging
Imports DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff
Imports DCSGlobal.BusinessRules.CoreLibraryII
Imports DCSGlobal.Rules.FireRules



Public Class iPAS7



    Implements IDisposable
    Private disposedValue As Boolean ' To detect redundant calls
    Dim scriptVBValue As String
    Private _ConnectionString As String = String.Empty

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If
            ' _scriptCtl = Nothing
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

    Public WriteOnly Property ConnectionString As String

        Set(value As String)
            _ConnectionString = value
            'log.ConnectionString = value
            'sch.ConnectionString = value
            'dss.ConnectionString = value
            ''    RA.ConnectionString = value
            'Met.ConnectionString = value
        End Set
    End Property


    'Public WriteOnly Property DLL As String

    '    Set(value As String)
    '        _DLL = value

    '    End Set
    'End Property


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strPID"></param>
    ''' <param name="strHospCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function getDummyPatientDataPid(ByVal PID As Decimal, ByVal HospCode As String) As DataSet

        Dim dsDummyPatient As DataSet = New DataSet()

        Using Con As New SqlConnection(_ConnectionString)
            Con.Open()
            Using cmd As New SqlCommand("usp_Rules_validation", Con)
                cmd.CommandType = CommandType.Text
                cmd.Parameters.Add("@pid", SqlDbType.Decimal).Value = PID

                Using da As SqlDataAdapter = New SqlDataAdapter(cmd)
                    da.Fill(dsDummyPatient, "DUMMY_PATIENT")
                End Using

            End Using
        End Using

        Return dsDummyPatient

    End Function



    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strPID"></param>
    ''' <param name="strHospCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetHospCodeList(ByVal userid As String, ByVal HospCode As String) As DataSet

        Dim dsDummyPatient As DataSet = New DataSet()

        Using Con As New SqlConnection(_ConnectionString)
            Con.Open()
            Using cmd As New SqlCommand("usp_Rules_validation", Con)
                cmd.CommandType = CommandType.Text
                cmd.Parameters.Add("@userid", SqlDbType.Decimal).Value = PID

                Using da As SqlDataAdapter = New SqlDataAdapter(cmd)
                    da.Fill(dsDummyPatient, "DUMMY_PATIENT")
                End Using

            End Using
        End Using

        Return dsDummyPatient

    End Function




    Public Function getPatientEvents(ByVal patient_number As String, ByVal hospital_code As String) As Integer





        Dim SQLConn As System.Data.SqlClient.SqlConnection = New System.Data.SqlClient.SqlConnection
        Dim sqlComm As SqlCommand

        ' Dim sqlString As String

        Dim sqlReader As SqlDataReader
        Dim eventlstItem As ListItem










        Using Con As New SqlConnection(_ConnectionString)
            Con.Open()
            Using cmd As New SqlCommand("usp_get_patient_events", Con)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@patient_number", SqlDbType.VarChar).Value = patient_number
                cmd.Parameters.Add("@hospital_code", SqlDbType.VarChar).Value = hospital_code

                Using rdr = cmd.ExecuteReader()
                    If rdr.HasRows Then
                        Do While rdr.Read
                            txtName.Text = RDR.Item("Name").ToString()
                        Loop
                    End If
                End Using

            End Using
        End Using







        Try
            SQLConn.ConnectionString = HttpContext.Current.Application("ConnectionString") 'Global.GlbConnStr
            SQLConn.Open()

            ''sqlString = "usp_get_patient_events"
            sqlComm = New SqlCommand("usp_get_patient_events", SQLConn)
            sqlComm.CommandType = CommandType.StoredProcedure
            sqlComm.CommandTimeout = CInt(ConfigurationManager.AppSettings("CommandTimeOut"))


            sqlComm.Parameters.Add("@patient_number", SqlDbType.VarChar)
            sqlComm.Parameters("@patient_number").Direction = ParameterDirection.Input
            sqlComm.Parameters("@patient_number").Value = txtAcctCode.Text

            sqlComm.Parameters.Add("@hospital_code", SqlDbType.VarChar)
            sqlComm.Parameters("@hospital_code").Direction = ParameterDirection.Input
            sqlComm.Parameters("@hospital_code").Value = ddlHospCode.SelectedValue

            sqlReader = sqlComm.ExecuteReader()

            ddlPatientEvents.Items.Clear()

            eventlstItem = New ListItem("Current Event", "0")
            ddlPatientEvents.Items.Add(eventlstItem)

            While sqlReader.Read()
                If Not IsDBNull(Trim(sqlReader.GetValue(0))) Then
                    eventlstItem = New ListItem(Global.GeneralFuncs.ReplaceQuote(Trim(sqlReader.GetValue(1)), "'", "''"), Global.GeneralFuncs.ReplaceQuote(Trim(sqlReader.GetValue(0)), "'", "''"))
                    ddlPatientEvents.Items.Add(eventlstItem)
                    'dicHospCodes.Add(Global.GeneralFuncs.ReplaceQuote(Trim(hospReader.GetValue(0)), "'", "''"), Global.GeneralFuncs.ReplaceQuote(Trim(hospReader.GetValue(0)), "'", "''"))
                End If
            End While

            'Session("dicHospCodes") = dicHospCodes

        Catch ex As System.Exception
            If Application("debug") = "Y" Then
                Response.Write(ex.Message)
            Else
                Session("stackTrace") = ex.StackTrace
                Response.Redirect("../qaGeneral/qaErrors.aspx?errID=1001&Msg=" & ex.Message.Replace(Environment.NewLine, " ") & "&Pg=validateRule.aspx&Md=Rules&Pr=getPatientEvents")
            End If
        Finally
            SQLConn.Close()
            SQLConn = Nothing
            sqlComm = Nothing
            sqlReader = Nothing
        End Try


    End Function


End Class
