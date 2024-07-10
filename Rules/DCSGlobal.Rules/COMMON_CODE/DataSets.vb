Option Explicit On
Option Strict On


Imports System.Data
Imports System.Data.SqlClient

Imports DCSGlobal.BusinessRules.Logging
Imports DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff
Imports DCSGlobal.BusinessRules.CoreLibraryII


Namespace DCSGlobal.Rules


    Public Class DataSets
        Implements IDisposable


        Private log As New logExecption()
        '  Private sch As New SchedulerLog()
        '   Private ss As New StringStuff()

        Dim _AppName As String = "Rule BandAids"
        Private _ConnectionString As String = String.Empty
        Private _CommandTimeOut As Integer = 90
        Private disposedValue As Boolean ' To detect redundant calls


        Private _Verbose As Integer = 0

        Private _sCfgUspRulesToFire As String = String.Empty
        Private _sCfgUspTankAddress As String = String.Empty
        Private _sCfgUspGetAllTankById As String = String.Empty
        Private _sCfgUspGetAllData As String = String.Empty
        Private _sCfgUspGetAllDataHL7Rows As String = String.Empty 'SUBBA-20160713
        Private _NoData As Boolean = False

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
        Public WriteOnly Property AppName As String

            Set(value As String)
                _AppName = value
            End Set
        End Property


        Public WriteOnly Property ConnectionString As String

            Set(value As String)
                _ConnectionString = value
                log.ConnectionString = value
                '  sch.ConnectionString = value
            End Set
        End Property

        Public WriteOnly Property CommandTimeOut As Integer

            Set(value As Integer)
                _CommandTimeOut = value

            End Set
        End Property


        Public WriteOnly Property sCfgUspRulesToFire As String

            Set(value As String)
                _sCfgUspRulesToFire = value

            End Set
        End Property

        Public WriteOnly Property sCfgUspTankAddress As String

            Set(value As String)
                _sCfgUspTankAddress = value

            End Set
        End Property



        Public WriteOnly Property sCfgUspGetAllTankById As String

            Set(value As String)
                _sCfgUspGetAllTankById = value

            End Set
        End Property

        Public WriteOnly Property sCfgUspGetAllData As String

            Set(value As String)
                _sCfgUspGetAllData = value

            End Set
        End Property

        Public WriteOnly Property sCfgUspGetAllDataHL7Rows As String

            Set(value As String)
                _sCfgUspGetAllDataHL7Rows = value

            End Set
        End Property

        Public WriteOnly Property Verbose As Integer

            Set(value As Integer)
                _Verbose = value

            End Set
        End Property


        Public ReadOnly Property NoData As Boolean
            Get
                Return _NoData
            End Get
        End Property


        Public Function getRulesToFire(ByVal context As String, ByVal HospCode As String, ByVal RuleID As Integer) As DataSet
            '   Dim ruleConn As SqlConnection = New SqlConnection
            Dim ds As DataSet
            Dim da As SqlDataAdapter
            '  Dim ruleComm As SqlCommand
            '  Dim sqlString As String
            'Dim bUpdate As Boolean
            ' Dim errMessage As String
            Dim iReturnReocords As Integer
            ' Dim strContext As String
            'Dim strHosp As String


            Using con As New SqlConnection(_ConnectionString)
                con.Open()
                Using cmd As New SqlCommand("usp_rules_to_fire_validate", con)


                    cmd.CommandType = CommandType.StoredProcedure

                    cmd.Parameters.Add("@context", SqlDbType.VarChar).Value = context
                    cmd.Parameters.Add("@hosp_code", SqlDbType.VarChar).Value = HospCode
                    cmd.Parameters.Add("@rule_id", SqlDbType.Int).Value = RuleID


                    da = New SqlDataAdapter
                    da.TableMappings.Add("Table", "RULES_FIRE")
                    da.SelectCommand = cmd
                    ds = New DataSet("RULES_FIRE")
                    iReturnReocords = da.Fill(ds)

                End Using
                con.Close()
            End Using



            Return ds
        End Function


        ''' <summary>
        ''' set _sCfgUspTankAddress as a proprty 
        ''' </summary>
        ''' <param name="sPatHospitalCode"></param>
        ''' <returns>DataSet</returns>
        ''' <remarks></remarks>
        Public Function getAddresDataTankByHospCode(ByVal sPatHospitalCode As String) As DataSet
            '    Dim sqlConn As SqlConnection = New SqlConnection
            Dim ds As DataSet
            Dim da As SqlDataAdapter
            '     Dim ruleComm As SqlCommand
            Dim sqlString As String = String.Empty
            Dim errMessage As String = String.Empty
            Dim iReturnReocords As Integer = 0

            Try
                'If (sqlConn.State = ConnectionState.Closed) Then
                '    sqlConn.ConnectionString = SQL_CONNECTION_STRING
                '    sqlConn.Open()
                'End If

                sqlString = _sCfgUspTankAddress 'subba-022509 "usp_get_tank_address"  'subba-042308
                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using ruleComm As New SqlCommand(sqlString, Con)
                        ruleComm.CommandType = CommandType.StoredProcedure

                        ruleComm.Parameters.Add("@pat_hosp_code", SqlDbType.VarChar, 20).Value = sPatHospitalCode

                        da = New SqlDataAdapter
                        da.TableMappings.Add("Table", "ADDRESS_TANK_DATA")
                        da.SelectCommand = ruleComm
                        ds = New DataSet("ADDRESS_TANK_DATA")
                        iReturnReocords = da.Fill(ds)
                    End Using
                    Con.Close()
                End Using

            Catch sx As SqlException
                errMessage = sx.Message
                '   If (sDebugMode = "Y") Then SaveTextToFile(errMessage + ":ConsoleFireRules-getRulesToFire:" + Now.ToString + "   ", sErrLogPath, "")
                ds = Nothing
                log.ExceptionDetails(":ConsoleFireRules-getRulesToFire:", sx)


            Catch ex As Exception
                errMessage = ex.Message
                '   If (sDebugMode = "Y") Then SaveTextToFile(errMessage + ":ConsoleFireRules-getRulesToFire:" + Now.ToString + "   ", sErrLogPath, "")
                ds = Nothing
                log.ExceptionDetails(":ConsoleFireRules-getRulesToFire:", ex)


            End Try

            Return ds
        End Function


        ''' <summary>
        ''' set _sCfgUspGetAllTankById as a proprty 
        ''' </summary>
        ''' <param name="sPatHospitalCode"></param>
        ''' <returns>DataSet</returns>
        ''' <remarks></remarks>
        Public Function getAllDataTankByID(ByVal id As Integer, ByVal sPatHospitalCode As String) As DataSet




            '  Dim sqlConn As SqlConnection = New SqlConnection
            Dim ds As DataSet
            Dim da As SqlDataAdapter
            ' Dim ruleComm As SqlCommand
            Dim sqlString As String = String.Empty
            Dim errMessage As String = String.Empty
            Dim iReturnReocords As Integer = 0

            Try


                sqlString = _sCfgUspGetAllTankById 'subba-022509 '"usp_get_all_data_tank_ByID"  'look 'subba-042208
                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using ruleComm As New SqlCommand(sqlString, Con)
                        ruleComm.CommandType = CommandType.StoredProcedure
                        ruleComm.Parameters.Add("@id", SqlDbType.Int).Value = id
                        ruleComm.Parameters.Add("@pat_hosp_code", SqlDbType.VarChar, 20).Value = sPatHospitalCode


                        da = New SqlDataAdapter
                        da.TableMappings.Add("Table", "RULES_TANK_DATA")
                        da.SelectCommand = ruleComm
                        ds = New DataSet("RULES_TANK_DATA")
                        iReturnReocords = da.Fill(ds)
                        _NoData = False
                        '
                    End Using
                    Con.Close()
                End Using

                If (ds.Tables(0).Rows.Count() = 0) Then
                    _NoData = True
                    Using ba As New BandAids()
                        ba.AppName = "usp_rules_non_processed_rows"
                        ba.ConnectionString = _ConnectionString
                        ba.usp_rules_non_processed_rows = "usp_rules_non_processed_rows"
                        ba.uspRulesNonProcessedRows(id, sPatHospitalCode)
                    End Using
                End If






            Catch ex As Exception
                errMessage = ex.Message
                log.ExceptionDetails(":ConsoleFireRules-getRulesToFire:", ex)
                ds = Nothing

            End Try

            Return ds
        End Function


        Public Function GetHospCodes(ByVal UserID As String) As List(Of String)


            Dim _hospital_code As List(Of String) = New List(Of String)()
            Dim s As String = String.Empty


            Try


                Using con As New SqlConnection(_ConnectionString)
                    con.Open()
                    Using cmd As New SqlCommand("usp_get_user_hosp", con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@userid", SqlDbType.VarChar).Value = UserID

                        cmd.CommandType = CommandType.StoredProcedure
                        Using idr = cmd.ExecuteReader()
                            '          Console.WriteLine("16")

                            If idr.HasRows Then



                                Do While idr.Read

                                    If Not IsDBNull(idr.Item("hospital_code")) Then
                                        _hospital_code.Add(Convert.ToString(idr.Item("hospital_code")))
                                    End If


                                Loop
                            End If
                        End Using


                    End Using
                    con.Close()
                End Using
            Catch ex As Exception

                s = ex.Message

            End Try



            Return _hospital_code


        End Function

        Public Function getDummyPatientDataPatient(ByVal PatientNumber As String, ByVal strHospCode As String) As DataSet

            Dim da As SqlDataAdapter
            Dim ds As DataSet
            ' Dim sqlString As String
            Dim iReturnReocords As Integer


            Try

                Using con As New SqlConnection(_ConnectionString)
                    con.Open()


                    Using cmd As New SqlCommand("usp_get_all_data", con)

                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.Add("@hosp_code", SqlDbType.VarChar).Value = strHospCode
                        cmd.Parameters.Add("@patientNum", SqlDbType.VarChar).Value = PatientNumber
                        cmd.Parameters.Add("@pid", SqlDbType.Int).Value = 0





                        da = New SqlDataAdapter
                        da.TableMappings.Add("Table", "RULES_TANK_DATA")
                        da.SelectCommand = cmd
                        ds = New DataSet("RULES_TANK_DATA")
                        iReturnReocords = da.Fill(ds, "DUMMY_PATIENT")

                    End Using

                    con.Close()
                End Using

            Catch sx As SqlException
                '  errMessage = sx.Message
                ds = Nothing
                log.ExceptionDetails(":ConsoleFireRules-getRulesData:", sx)


            Catch ex As Exception
                '  errMessage = ex.Message
                ds = Nothing
                log.ExceptionDetails(":ConsoleFireRules-getRulesData:", ex)



            End Try



            Return ds


            'Try
            '    ruleConn.ConnectionString = HttpContext.Current.Application("ConnectionString") 'Global.GlbConnStr

            '    ruleConn.Open()

            '    'sqlString = "usp_rules_validation_patient_pid"
            '    ''sqlString = "usp_get_all_data"
            '    ruleComm = New SqlCommand("usp_get_all_data", ruleConn)
            '    ruleComm.CommandType = CommandType.StoredProcedure
            '    ruleComm.CommandTimeout = CInt(ConfigurationManager.AppSettings("CommandTimeOut"))


            '    ruleComm.Parameters.Add("@patientNum", SqlDbType.VarChar)
            '    ruleComm.Parameters("@patientNum").Direction = ParameterDirection.Input
            '    ruleComm.Parameters("@patientNum").Value = strPID

            '    ruleComm.Parameters.Add("@hosp_code", SqlDbType.VarChar).Value = strHospCode


            '    ruleComm.Parameters.Add("@pid", SqlDbType.Int).Value = 0


            '    da = New SqlDataAdapter(ruleComm)
            '    dsDummyPatient = New DataSet
            '    iReturnReocords = da.Fill(dsDummyPatient, "DUMMY_PATIENT")
            'Catch ex As System.Exception
            '    If Application("debug") = "Y" Then
            '        Response.Write(ex.Message + "   " + ex.StackTrace)
            '    Else
            '        Session("stackTrace") = ex.StackTrace
            '        Response.Redirect("../qaGeneral/qaErrors.aspx?errID=1001&Msg=" & Server.HtmlEncode(ex.Message) & "&Pg=validateRule.aspx&Md=qaGeneral&Pr=getDummyPatientDataPatient")
            '    End If
            '    dsDummyPatient = Nothing
            'Finally
            '    ruleComm = Nothing
            '    ruleConn.Close()
            '    ruleConn = Nothing
            'End Try

            'Return dsDummyPatient
        End Function





        Public Function GetContext() As Dictionary(Of Integer, String)

            Dim r As New Dictionary(Of Integer, String)
            Dim s As String = String.Empty


            Try


                Using con As New SqlConnection(_ConnectionString)
                    con.Open()
                    Using cmd As New SqlCommand("usp_get_context", con)


                        cmd.CommandType = CommandType.StoredProcedure




                        Using idr = cmd.ExecuteReader()
                            '          Console.WriteLine("16")

                            If idr.HasRows Then

                                Dim key As Integer = 0
                                Dim value As String = String.Empty



                                Do While idr.Read

                                    If Not IsDBNull(idr.Item("context_id")) Then
                                        key = Convert.ToInt32(idr.Item("context_id"))
                                    End If

                                    If Not IsDBNull(idr.Item("context_desc")) Then
                                        value = Convert.ToString(idr.Item("context_desc"))
                                    End If

                                    r.Add(key, value)

                                Loop
                            End If
                        End Using


                    End Using
                    con.Close()
                End Using
            Catch ex As Exception

                s = ex.Message

            End Try

            Return r

        End Function


        Public Function GetRuleMessages(ByVal UserID As String) As Dictionary(Of Integer, String)

            Dim r As New Dictionary(Of Integer, String)()

            r = GetRuleMessageDictonary(UserID, "", "N", "N")

            Return r

        End Function




        Public Function GetRuleMessages(ByVal UserID As String, ByVal SearchString As String, ByVal isSearchExact As String, ByVal isSearchLast As String) As Dictionary(Of Integer, String)


            Dim r As New Dictionary(Of Integer, String)()


            r = GetRuleMessageDictonary(UserID, SearchString, isSearchExact, isSearchLast)


            Return r

        End Function



        Private Function GetRuleMessageDictonary(ByVal UserID As String, ByVal SearchString As String, ByVal isSearchExact As String, ByVal isSearchLast As String) As Dictionary(Of Integer, String)



            Dim r As New Dictionary(Of Integer, String)()
            Dim s As String = String.Empty


            Try


                Using con As New SqlConnection(_ConnectionString)
                    con.Open()
                    Using cmd As New SqlCommand("usp_get_context", con)


                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@struser", SqlDbType.VarChar).Value = UserID
                        cmd.Parameters.Add("@searchString", SqlDbType.VarChar).Value = SearchString
                        cmd.Parameters.Add("@isSearchExact", SqlDbType.VarChar).Value = isSearchExact
                        cmd.Parameters.Add("@isSearchLast", SqlDbType.VarChar).Value = isSearchLast


                        Using idr = cmd.ExecuteReader()
                            '          Console.WriteLine("16")

                            If idr.HasRows Then

                                Dim key As Integer = 0
                                Dim value As String = String.Empty



                                Do While idr.Read

                                    If Not IsDBNull(idr.Item("message_id")) Then
                                        key = Convert.ToInt32(idr.Item("message_id"))
                                    End If

                                    If Not IsDBNull(idr.Item("message_desc")) Then
                                        value = Convert.ToString(idr.Item("message_desc"))
                                    End If

                                    r.Add(key, value)

                                Loop
                            End If
                        End Using


                    End Using
                    con.Close()
                End Using
            Catch ex As Exception

                s = ex.Message

            End Try

            Return r

        End Function


        'SUBBA-20160713
        Public Function getRulesDataAuditTrailHL7Rows(ByVal HL7rowId As Integer) As DataSet
            ' Dim sqlConn As SqlConnection = New SqlConnection
            Dim ds As DataSet
            Dim da As SqlDataAdapter
            '  Dim ruleComm As SqlCommand
            Dim sqlString As String
            '   Dim bUpdate As Boolean
            Dim errMessage As String
            Dim iReturnReocords As Integer
            '    Dim sGetAllDataSp As String

            Try
                ''sqlString = "usp_get_all_data"
                ''''sGetAllDataSp = System.Configuration.ConfigurationSettings.AppSettings("getAllDataSp").Trim()

                sqlString = _sCfgUspGetAllDataHL7Rows 'SUBBA-20160713 '_sCfgUspGetAllData 'subba-022509 ' sGetAllDataSp  '"usp_get_pat_audit_trail" - subba-090407

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()


                    Using ruleComm As New SqlCommand(sqlString, Con)
                        ruleComm.CommandType = CommandType.StoredProcedure

                        ruleComm.Parameters.Add("@PATIENT_AUDIT_TRAIL_ID", SqlDbType.BigInt).Value = HL7rowId

                        da = New SqlDataAdapter
                        da.TableMappings.Add("Table", "RULES_TANK_DATA")
                        da.SelectCommand = ruleComm
                        ds = New DataSet("RULES_TANK_DATA")
                        iReturnReocords = da.Fill(ds)

                    End Using

                    Con.Close()
                End Using

            Catch sx As SqlException
                errMessage = sx.Message
                ds = Nothing
                log.ExceptionDetails(":ConsoleFireRules-getRulesData:", sx)


            Catch ex As Exception
                errMessage = ex.Message
                ds = Nothing
                log.ExceptionDetails(":ConsoleFireRules-getRulesData:", ex)



            End Try

            Return ds
        End Function



        Public Function deleteRuleResultsForEachDataRow(ByVal UserID As String) As Integer

            Dim r As Integer = 0
            Dim s As String = String.Empty


            Try


                Using con As New SqlConnection(_ConnectionString)
                    con.Open()
                    Using cmd As New SqlCommand("usp_rule_result_validate_delete", con)


                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@userid", SqlDbType.VarChar).Value = UserID

                        r = cmd.ExecuteNonQuery()

                    End Using
                    con.Close()
                End Using
            Catch ex As Exception
                r = 0
                s = ex.Message

            End Try

            Return r

        End Function









        Public Function updateValidateListProcessed(ByVal UserID As String, ByVal BatchID As Integer) As Integer

            Dim r As Integer = 0
            Dim s As String = String.Empty


            Try


                Using con As New SqlConnection(_ConnectionString)
                    con.Open()
                    Using cmd As New SqlCommand("usp_processed_all_data_validate", con)


                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@userid", SqlDbType.VarChar).Value = UserID
                        cmd.Parameters.Add("@batch_id", SqlDbType.Int).Value = BatchID


                        r = cmd.ExecuteNonQuery()

                    End Using
                    con.Close()
                End Using
            Catch ex As Exception
                r = 0
                s = ex.Message

            End Try

            Return r

        End Function

        Public Function getCounts(ByVal UserID As String, ByVal BatchID As Integer, ByVal CountOption As String) As Integer
            Dim r As Integer


            Try


                Using con As New SqlConnection(_ConnectionString)
                    con.Open()
                    Using cmd As New SqlCommand("usp_insert_range_validate", con)


                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@userid", SqlDbType.VarChar).Value = UserID
                        cmd.Parameters.Add("@batch_id", SqlDbType.Int).Value = BatchID

                        cmd.Parameters.Add("@option", SqlDbType.VarChar).Value = CountOption


                        cmd.ExecuteNonQuery()
                        r = Convert.ToInt32(cmd.Parameters("@intRows").Value)

                        Using idr = cmd.ExecuteReader()
                            '          Console.WriteLine("16")

                            If idr.HasRows Then




                                Do While idr.Read

                                    If Not IsDBNull(idr.Item(0)) Then
                                        r = Convert.ToInt32(idr.Item(0))
                                    End If


                                Loop
                            End If
                        End Using


                    End Using
                    con.Close()
                End Using
            Catch ex As Exception
                r = 0


            End Try

            Return r
        End Function


        Public Function getRulesData(ByVal UserID As String, ByVal BatchID As Integer) As DataSet

            Dim ds As DataSet
            Dim da As SqlDataAdapter
            'Dim ruleComm As SqlCommand
            'Dim sqlString As String
            ''Dim bUpdate As Boolean
            'Dim errMessage As String
            Dim iReturnReocords As Integer
            Dim dt As DataTable
            '@audit_start_date varchar(8), @audit_end_date varchar(8)




            Using con As New SqlConnection(_ConnectionString)
                con.Open()
                Using cmd As New SqlCommand("usp_get_all_data_validate", con)


                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@userid", SqlDbType.VarChar).Value = UserID
                    cmd.Parameters.Add("@batch_id", SqlDbType.Int).Value = BatchID



                    da = New SqlDataAdapter
                    da.TableMappings.Add("Table", "RULES_DATA")
                    da.SelectCommand = cmd
                    dt = New DataTable("RULES_TABLE")
                    ds = New DataSet("RULES_DATA")
                    ' CAJ Change to a result set
                    iReturnReocords = da.Fill(ds)

                End Using
                con.Close()
            End Using




            Return ds
        End Function


        Public Function getValidateList(ByVal UserID As String, ByVal BatchID As Integer, ByVal HospCode As String, _
                                        ByVal Patient As String, ByVal BeginDate As Date, ByVal Enddate As Date) As Integer

            Dim r As Integer = 0
            Dim s As String = String.Empty


            Try


                Using con As New SqlConnection(_ConnectionString)
                    con.Open()
                    Using cmd As New SqlCommand("usp_insert_range_validate", con)


                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@userid", SqlDbType.VarChar).Value = UserID
                        cmd.Parameters.Add("@batch_id", SqlDbType.Int).Value = BatchID

                        cmd.Parameters.Add("@hospital_code", SqlDbType.VarChar).Value = HospCode
                        cmd.Parameters.Add("@patient", SqlDbType.VarChar).Value = Patient

                        cmd.Parameters.Add("@begin_date", SqlDbType.VarChar).Value = Convert.ToString(BeginDate)
                        cmd.Parameters.Add("@end_date", SqlDbType.VarChar).Value = Convert.ToString(Enddate)

                        cmd.Parameters.Add("@intRows", SqlDbType.Int)
                        cmd.Parameters("@intRows").Direction = ParameterDirection.Output

                        cmd.ExecuteNonQuery()
                        r = Convert.ToInt32(cmd.Parameters("@intRows").Value)




                    End Using
                    con.Close()
                End Using
            Catch ex As Exception
                r = 0
                s = ex.Message

            End Try

            Return r

        End Function


    End Class

End Namespace