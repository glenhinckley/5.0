Option Explicit On
Option Strict On

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


Namespace FireRules

    Public Class CachedDataSets

        Implements IDisposable

        Private disposedValue As Boolean ' To detect redundant calls

        Private log As New logExecption()


        Private _Verbose As Integer = 1


        Private _htRuleMsgs As Hashtable = New Hashtable


        Private _ConnectionString As String = String.Empty


        Private _CfgUspGetRuleMsg As String = String.Empty



        Private _AppName As String = ""


        Private _ClassName As String = "CachedDataSets"


        Private _usp_rules_non_processed_rows As String = String.Empty

        Private _sCfgUspRulesToFire As String = String.Empty



        Private RA_VERSION As String
        Private RA_SERVER_PORT As String
        Private RA_PROTOCOL As String
        Private INSERT_INTO_RA_DEBUG_FLAG As String
        Private RA_MSG_DELIMITER As String

        Private sAddrFilter As String = String.Empty
        Private sAddrGetMultipleAddress As String = String.Empty

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




        Public WriteOnly Property CfgUspRulesToFire As String

            Set(value As String)
                _sCfgUspRulesToFire = value


            End Set
        End Property

        Public WriteOnly Property CfgUspGetRuleMsg As String

            Set(value As String)
                _CfgUspGetRuleMsg = value


            End Set
        End Property



        Public ReadOnly Property AddrGetMultipleAddress As String
            Get
                Return sAddrGetMultipleAddress
            End Get
        End Property


        Public ReadOnly Property AddrFilter As String
            Get
                Return sAddrFilter
            End Get
        End Property



        Public ReadOnly Property htRuleMsgs As Hashtable
            Get
                Return _htRuleMsgs
            End Get
        End Property

        Public Function LoadFireAddressParameters() As Integer

            Dim r As Integer = -1

            Dim strSQLRaVer As String = "Select module_name, lov_name, lov_text, lov_value from system_preferences where module_name = 'Address' "

            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(strSQLRaVer, Con)
                        cmd.CommandType = CommandType.Text


                        Dim drOprRaVer As SqlDataReader = cmd.ExecuteReader()

                        If drOprRaVer.HasRows Then
                            While drOprRaVer.Read()
                                If Not IsDBNull(Trim(CStr(drOprRaVer.GetValue(drOprRaVer.GetOrdinal("lov_text"))))) Then
                                    ' If Not isDbNull(drOprRaVer.GetValue(drOprRaVer.GetOrdinal("lov_text")) Then
                                    ' 1.0.0 

                                    If (drOprRaVer.GetValue(drOprRaVer.GetOrdinal("lov_text")).ToString()).ToLower() = "addresstypefilter" Then
                                        sAddrFilter = DirectCast(drOprRaVer.GetValue(drOprRaVer.GetOrdinal("lov_value")), String).Trim().ToUpper()
                                    End If

                                    If (drOprRaVer.GetValue(drOprRaVer.GetOrdinal("lov_text")).ToString()).ToLower() = "addressmultipleaddress" Then
                                        sAddrGetMultipleAddress = DirectCast(drOprRaVer.GetValue(drOprRaVer.GetOrdinal("lov_value")), String).Trim().ToUpper()
                                    End If
                                End If
                            End While
                        End If
                        r = 0
                    End Using
                    Con.Close()
                End Using




            Catch sx As SqlException


                log.ExceptionDetails(_AppName + " LoadFireAddressParams", sx)



            Catch ex As Exception

                log.ExceptionDetails(_AppName + " LoadFireAddressParams", ex)

            End Try




            Return r




        End Function

        ''' <summary>
        ''' this needs to get changed to a collection and get all the rules at one time. 
        ''' no more onesy twooossses this is stupid slow
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function populateRuleMessagesHT() As Integer



            Dim r As Integer = -1
            Dim __strRuleMsg As String = String.Empty

            Try
                ' Private _CfgUspGetRuleMsg As String = String.Empty

                'If (sqlConn.State = ConnectionState.Closed) Then
                '    sqlConn.ConnectionString = SQL_CONNECTION_STRING
                '    sqlConn.Open()
                'End If

                '    __sqlString = _CfgUspGetRuleMsg 'subba-022509 '"usp_get_rule_messages"
                '  ruleComm = New SqlCommand(sqlString, sqlConn)
                '  ruleComm.CommandType = CommandType.StoredProcedure
                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using ruleComm As New SqlCommand(_CfgUspGetRuleMsg, Con)
                        ruleComm.CommandType = CommandType.StoredProcedure
                        ruleComm.Parameters.Add("@intId", SqlDbType.Int)
                        ruleComm.Parameters("@intId").Direction = ParameterDirection.Input
                        ruleComm.Parameters("@intId").Value = 0 'all rules
                        Dim ruleReader As SqlDataReader
                        ruleReader = ruleComm.ExecuteReader()

                        While ruleReader.Read()
                            If Not IsDBNull(Trim(CStr(ruleReader.GetValue(ruleReader.GetOrdinal("message_id"))))) Then
                                'Me.txtMessage.Text = ruleReader.GetValue(1)
                                __strRuleMsg = CStr(ruleReader.GetValue(ruleReader.GetOrdinal("message_desc")))  'getOrdinal
                                Try
                                    If Not _htRuleMsgs.ContainsKey(Trim(CStr(ruleReader.GetValue(ruleReader.GetOrdinal("message_id"))))) Then
                                        _htRuleMsgs.Add(Trim(ruleReader.GetValue(ruleReader.GetOrdinal("message_id")).ToString().Replace(ControlChars.CrLf, "").Trim), __strRuleMsg)
                                    End If                            '
                                Catch ex As Exception
                                    '    log.ExceptionDetails("OddPlace", ex)

                                End Try
                            End If
                        End While
                    End Using
                    Con.Close()
                End Using

                r = 0

            Catch sx As SqlException

                log.ExceptionDetails("populateRuleMessagesHT", sx)




            Catch ex As Exception

                log.ExceptionDetails("populateRuleMessagesHT", ex)


            End Try

            Return r
        End Function








        Function LoadRealAlertParameters() As Integer
            Dim _FunctionName As String = "LoadRealAlertParameters"
            Dim r = -1

            Try

                Dim strSQLRaVer As String = "Select module_name, lov_name, lov_text, lov_value from  system_preferences (nolock) where module_name = 'RealAlert' "

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using SQLCommRaVer As New SqlCommand(strSQLRaVer, Con)
                        SQLCommRaVer.CommandType = CommandType.Text

                        Dim drOprRaVer As SqlDataReader = SQLCommRaVer.ExecuteReader

                        If drOprRaVer.HasRows Then
                            While drOprRaVer.Read()
                                If Not IsDBNull(drOprRaVer.GetValue(drOprRaVer.GetOrdinal("lov_text"))) Then    ' 1.0.0

                                    If (Convert.ToString(drOprRaVer.GetValue(drOprRaVer.GetOrdinal("lov_text"))).Trim.ToLower = "raversion") Then
                                        RA_VERSION = Convert.ToString(drOprRaVer.GetValue(drOprRaVer.GetOrdinal("lov_value"))).Trim.ToUpper()
                                    End If


                                    If (Convert.ToString(drOprRaVer.GetValue(drOprRaVer.GetOrdinal("lov_text"))).Trim.ToLower = "raserverport") Then
                                        RA_SERVER_PORT = Convert.ToString(drOprRaVer.GetValue(drOprRaVer.GetOrdinal("lov_value"))).Trim.ToUpper()
                                    End If

                                    If (Convert.ToString(drOprRaVer.GetValue(drOprRaVer.GetOrdinal("lov_text"))).Trim.ToLower = "raprotocol") Then
                                        RA_PROTOCOL = Convert.ToString(drOprRaVer.GetValue(drOprRaVer.GetOrdinal("lov_value"))).Trim.ToUpper()
                                    End If

                                    If (Convert.ToString(drOprRaVer.GetValue(drOprRaVer.GetOrdinal("lov_text"))).Trim.ToLower = "raclientdetaildebug") Then
                                        INSERT_INTO_RA_DEBUG_FLAG = Convert.ToString(drOprRaVer.GetValue(drOprRaVer.GetOrdinal("lov_value"))).Trim.ToUpper()
                                    End If

                                    If (Convert.ToString(drOprRaVer.GetValue(drOprRaVer.GetOrdinal("lov_text"))).Trim.ToLower = "ramsgdelimeter") Then
                                        RA_MSG_DELIMITER = Convert.ToString(drOprRaVer.GetValue(drOprRaVer.GetOrdinal("lov_value"))).Trim.ToUpper()
                                    End If

                                End If
                            End While
                        End If
                        r = 0
                    End Using
                    Con.Close()
                End Using


            Catch sx As SqlException

                log.ExceptionDetails("LoadRealAlertParams", sx)

            Catch ex As Exception

                log.ExceptionDetails("LoadRealAlertParams", ex)
            End Try


            Return r



        End Function


        ''' <summary>
        ''' set _sCfgUspRulesToFire as a proprty 
        ''' </summary>
        ''' <param name="context_desc"></param>
        ''' <returns>DataSet</returns>
        ''' <remarks></remarks>
        Public Function getRulesToFire(ByVal context_desc As String) As DataSet
            ' Dim sqlConn As SqlConnection = New SqlConnection
            Dim ds As DataSet
            Dim da As SqlDataAdapter
            '  Dim ruleComm As SqlCommand
            Dim sqlString As String
            Dim errMessage As String
            Dim iReturnReocords As Integer


            Try
                'If (sqlConn.State = ConnectionState.Closed) Then
                '    sqlConn.ConnectionString = SQL_CONNECTION_STRING
                '    sqlConn.Open()
                'End If

                '     sqlString =  'subba-022509'"usp_rules_to_fire"




                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using ruleComm As New SqlCommand(_sCfgUspRulesToFire, Con)
                        ruleComm.CommandType = CommandType.StoredProcedure



                        'ruleComm.CommandType = CommandType.StoredProcedure
                        'ruleComm.CommandTimeout = _CommandTimeOut


                        ruleComm.Parameters.Add("@context", SqlDbType.VarChar, 20).Value = context_desc


                        da = New SqlDataAdapter
                        da.TableMappings.Add("Table", "RULES_FIRE")
                        da.SelectCommand = ruleComm
                        ds = New DataSet("RULES_FIRE")
                        iReturnReocords = da.Fill(ds)


                    End Using
                    Con.Close()
                End Using

            Catch sx As SqlException
                '   errMessage = sx.Message
                '   If (sDebugMode = "Y") Then SaveTextToFile(errMessage + ":ConsoleFireRules-getRulesToFire:" + Now.ToString + "   ", sErrLogPath, "")
                log.ExceptionDetails(":ConsoleFireRules-getRulesToFire:", sx)
                ds = Nothing



            Catch ex As Exception
                '  errMessage = ex.Message
                '   If (sDebugMode = "Y") Then SaveTextToFile(errMessage + ":ConsoleFireRules-getRulesToFire:" + Now.ToString + "   ", sErrLogPath, "")
                log.ExceptionDetails(":ConsoleFireRules-getRulesToFire:", ex)
                ds = Nothing
            End Try

            Return ds
        End Function





        Public Function uspRulesNonProcessedRows(ByVal RoleID As Integer, ByVal PatHouseCode As String) As Integer
            Dim _FunctionName As String = "CachedDataSets"
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
End Namespace
