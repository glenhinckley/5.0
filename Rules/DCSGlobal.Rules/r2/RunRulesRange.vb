Option Explicit On
Option Strict On


Imports System.IO.Pipes
Imports System.Security.Principal

Imports DCSGlobal.BusinessRules.Logging
Imports DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff
Imports DCSGlobal.BusinessRules.CoreLibraryII
Imports System.Text

'Imports DCSGlobal.RealAlert.DCSGlobal.RealAlert


Namespace DCSGlobal.Rules


    Public Class RunRulesRange

        Implements IDisposable
        Private disposedValue As Boolean ' To detect redundant calls


        Private log As New logExecption()
        Private ss As New StringStuff()

        Private _UserID As String = String.Empty
        Private _BatchID As Integer = 0
        Private _RuleID As Integer = 0


        Private _HospCode As String = String.Empty
        Private _Context As String = String.Empty

        Private _Patient As String = String.Empty

        Private _BeginDate As Date = Date.Today
        Private _EndDate As Date = Date.Today

        Private _ConnectionString As String = String.Empty
        Private _CommandTimeout As Integer = 180

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

        Public WriteOnly Property ConnectionString As String

            Set(value As String)
                _ConnectionString = value
                log.ConnectionString = value

            End Set
        End Property




        Public Function validateMultiRules(ByVal UserID As String) As Integer



            Dim sResultAll As String = "" 'subba-20180716
            Dim sRuleSuccess As String = "" 'subba-20180716
            Dim dsRulesData As New DataSet
            Dim dsRulesToFire As New DataSet
            Dim dtRulesData As DataTable
            Dim alRules As New ArrayList
            Dim alFacilities As New ArrayList
            Dim strPatientAccount As String = "" 'subba-20180716
            Dim strOperatorId As String = "" 'subba-20180716

            Dim strRuleDef As String = "", strRuleID As String = "", strRuleName As String = "", strPid As String = "" 'subba-20180716
            Dim strContextDesc As String = "short summary"

            Dim dataRulesDataTable As System.Data.DataTable
            Dim dataRow As System.Data.DataRow
            Dim dataRulesFireTable As System.Data.DataTable
            Dim dataRulesFireRow As System.Data.DataRow
            'Dim dataColumn As System.Data.DataColumn
            Dim dlCol As DataColumn
            'Dim iRetScheduler As Integer
            Dim iDataRules As Integer
            Dim dtStart_ts As Date
            Dim strCommand As String
            Dim strTestRuleErrodCode As String
            Dim strPatHospCode As String
            Dim strPatFirstName, strPatLastName, strAdmitDate, strEventDate, strInsType As String
            Dim strRuleContextID As String
            'Dim scriptCtl As MSScriptControl.ScriptControl = New MSScriptControl.ScriptControl
            Dim scriptVBValue As String
            Dim sbAllRuleInsert As StringBuilder = Nothing 'subba-20180716
            'Dim strAllRuleInsert As String
            Dim iRowsToProcess As Integer
            Dim iTotalRulesToFire As Integer
            Dim strHospCode As String
            'Dim htMessages As Hashtable
            Dim intValidateRows As Integer
            'Dim sGetRaUID, sRaGetClientIP, sGetRaCookie As String

            strRuleContextID = Me.ddlContext.SelectedValue

            dtStart_ts = Now()

            Dim iDataRow As Integer = 0
            Dim iBatchID As Integer = 0

            'htMessages = populateRuleMessagesHT()
            ''''scriptCtl.Language = "vbscript"

            Try



                Using ds As New DataSets()

                    ds.ConnectionString = _ConnectionString
                    ds.deleteRuleResultsForEachDataRow(UserID)
                    'deleteRuleResultsForEachDataRow(Session("userid"))

                End Using


                'Build the pat_hosp_code list to validate


                Using ds As New DataSets()

                    ds.ConnectionString = _ConnectionString
                    intValidateRows = ds.getValidateList(UserID, _BatchID, _HospCode, _Patient, _BeginDate, _EndDate)

                    'intValidateRows = getValidateList()
                End Using





                If (intValidateRows = 0) Then
                    '     lblErrDisp.Text = "No Data Found."
                    ''Response.Write("No Data Found for selected input")
                End If


                If intValidateRows > 0 Then

                    Do While True
                        'maNOJ sINCE WE HAVE 1269 ROWS IS IT LOOPING 7 TIMES ????




                        Using ds As New DataSets()
                            ds.ConnectionString = _ConnectionString
                            ds.getRulesData(_UserID, _BatchID)

                            'dsRulesData = getRulesData()


                        End Using

                        If Not (IsDBNull(dsRulesData)) Then
                            iRowsToProcess = dsRulesData.Tables(0).Rows.Count

                            If (iRowsToProcess > 0) Then

                                dtRulesData = New DataTable(dsRulesData.Tables(0).ToString())





                                Using ds As New DataSets()
                                    ds.ConnectionString = _ConnectionString
                                    dsRulesToFire = ds.getRulesToFire(_Context, _HospCode, _RuleID)
                                    ' dsRulesToFire = getRulesToFire()
                                End Using

                                iTotalRulesToFire = dsRulesToFire.Tables(0).Rows.Count

                                For Each dataRulesDataTable In dsRulesData.Tables


                                    Dim strRegionCode As String

                                    ' Dim strDivisionCode As String = "" 'subba-20180716


                                    Dim bAllRuleDataRowSuccess As Boolean = True


                                    For Each dataRow In dataRulesDataTable.Rows
                                        '''strHospFacilityCode = dataRow("client_facility_code")
                                        '  strHospCode = Convert.ToString(dataRow("client_facility_code"))
                                        '  strRegionCode = Convert.ToString(dataRow("region_code"))




                                        'If Not IsDBNull(dataRow("division_code")) Then
                                        '    strDivisionCode = dataRow("division_code")
                                        'Else
                                        '    strDivisionCode = ""
                                        'End If

                                        If Not IsDBNull(dataRow("client_facility_code")) Then
                                            strHospCode = Convert.ToString(dataRow("client_facility_code"))
                                        Else
                                            strHospCode = String.Empty
                                        End If

                                        If Not IsDBNull(dataRow("region_code")) Then
                                            strRegionCode = Convert.ToString(dataRow("region_code"))
                                        Else
                                            strRegionCode = String.Empty
                                        End If


                                        If Not IsDBNull(dataRow("patient_number")) Then
                                            strPatientAccount = Convert.ToString(dataRow("patient_number"))
                                        Else
                                            strPatientAccount = String.Empty
                                        End If


                                        If Not IsDBNull(dataRow("Operator_id")) Then
                                            strOperatorId = Convert.ToString(dataRow("Operator_id"))
                                        Else
                                            strOperatorId = String.Empty
                                        End If


                                        If Not IsDBNull(dataRow("pid")) Then
                                            strPid = Convert.ToString(dataRow("pid"))
                                        Else
                                            strPid = String.Empty
                                        End If

                                        If Not IsDBNull(dataRow("event_datetime")) Then
                                            strEventDate = Convert.ToString(dataRow("event_datetime"))
                                        Else
                                            strEventDate = String.Empty
                                        End If

                                        If Not IsDBNull(dataRow("admitting_date")) Then
                                            strAdmitDate = Convert.ToString(dataRow("admitting_date"))
                                        Else
                                            strAdmitDate = String.Empty
                                        End If

                                        strInsType = "0"

                                        If Not IsDBNull(dataRow("pat_hosp_code")) Then
                                            strPatHospCode = Convert.ToString(dataRow("pat_hosp_code"))
                                        Else
                                            strPatHospCode = String.Empty
                                        End If


                                        If Not IsDBNull(dataRow("patient_last_name")) Then
                                            strPatLastName = Convert.ToString(dataRow("patient_last_name"))
                                        Else
                                            strPatLastName = String.Empty
                                        End If

                                        If Not IsDBNull(dataRow("patient_first_name")) Then
                                            strPatFirstName = Convert.ToString(dataRow("patient_first_name"))
                                        Else
                                            strPatFirstName = String.Empty
                                        End If
                                        '' For Region and Hospital - Find All Rules   -- filter for Region and Hosp on dsRulesToFire
                                        'strHospCode, strRegionCode

                                        'Dim filteredRows() As DataRow 'subba-20180713
                                        'manoj  mISSING dIVSVION CODE FROM cONSOLE
                                        Dim sFilterExp As String = "facility_code = '" + strHospCode + "' OR " + "facility_code = '" + strRegionCode + "'"
                                        'Dim sFilterExp As String = "facility_code = '" + strHospCode + "' OR " + "facility_code = '" + strRegionCode + "' OR " + "facility_code = '" + strDivisionCode + "'"
                                        Dim strSort As String = "facility_code desc"
                                        'Apply filter to DataTable
                                        For Each dataRulesFireTable In dsRulesToFire.Tables 'dsRulesToFireFiltered
                                            iDataRules = 0

                                            sbAllRuleInsert = New StringBuilder("<root>")

                                            'filteredRows = dataRulesFireTable.Select(sFilterExp, strSort)
                                            'For Each dataRulesFireRow In filteredRows
                                            For Each dataRulesFireRow In dataRulesFireTable.Rows
                                                strRuleID = Convert.ToString(dataRulesFireRow("rule_id"))
                                                strRuleName = Convert.ToString(dataRulesFireRow("rule_name"))
                                                strRuleDef = Convert.ToString(dataRulesFireRow("rule_def"))

                                                strCommand = strRuleDef.Replace("return", "testRule=")
                                                For Each dlCol In dsRulesData.Tables(0).Columns
                                                    If Not IsDBNull(dataRow(dlCol.ColumnName)) Then
                                                        strCommand = strCommand.Replace("#" + dlCol.ColumnName + "#", """" + CStr(dataRow(dlCol.ColumnName)) + """")
                                                    Else
                                                        strCommand = strCommand.Replace("#" + dlCol.ColumnName + "#", """" + "" + """")
                                                    End If
                                                    If (strCommand.IndexOf("#") < 0) Then Exit For 'look
                                                Next

                                                ' Verify VbScipt Syntax 
                                                Try

                                                    If (strCommand <> "" And strCommand.IndexOf("##") < 0) Then
                                                        scriptVBValue = "Function testRule()" & vbCrLf & strCommand & vbCrLf & "End Function"
                                                        ''scriptCtl.AddCode(scriptVBValue)
                                                        ''strTestRuleErrodCode = scriptCtl.Run("testRule")
                                                        Try
                                                            strTestRuleErrodCode = dcsRulesVerifyWs.DcsRulesVerify("", "", "", strCommand) 'subba-20160223
                                                        Catch ex As System.Exception

                                                            strTestRuleErrodCode = "-1000"
                                                        End Try

                                                        ' 'lookDebug1
                                                        ' If (sDebugMode = "Y") Then SaveTextToFile("strTestRuleErrodCode:" + ":ConsoleFireRules-MAIN:" + Now.ToString + "   " + "   " + CStr(strTestRuleErrodCode), sErrLogPath, "")


                                                        If (sResultAll = "Y") Then
                                                            sbAllRuleInsert.Append(buildXMLRuleInsert(CInt(strRuleID), CInt(strRuleContextID), strPatientAccount, strPatHospCode, strHospCode, CInt(strTestRuleErrodCode), strOperatorId, strAdmitDate, strEventDate, CInt(strPid), strInsType, intBatchId))
                                                            'lookDebug1
                                                        Else
                                                            If (CInt(strTestRuleErrodCode) < 0) Then
                                                                bAllRuleDataRowSuccess = False
                                                                sbAllRuleInsert.Append(buildXMLRuleInsert(CInt(strRuleID), CInt(strRuleContextID), strPatientAccount, strPatHospCode, strHospCode, CInt(strTestRuleErrodCode), strOperatorId, strAdmitDate, strEventDate, CInt(strPid), strInsType, intBatchId))
                                                            End If
                                                        End If
                                                    Else
                                                        strTestRuleErrodCode = -2000
                                                    End If
                                                Catch ex As System.Exception
                                                    strTestRuleErrodCode = -1000
                                                    bAllRuleDataRowSuccess = False
                                                    sbAllRuleInsert.Append(buildXMLRuleInsert(CInt(strRuleID), CInt(strRuleContextID), strPatientAccount, strPatHospCode, strHospCode, CInt(strTestRuleErrodCode), strOperatorId, strAdmitDate, strEventDate, CInt(strPid), strInsType, intBatchId))
                                                End Try
                                                strRuleDef = ""
                                            Next

                                            If (sRuleSuccess = "Y" And bAllRuleDataRowSuccess) Then
                                                sbAllRuleInsert.Append(buildXMLRuleInsert(0, CInt(strRuleContextID), strPatientAccount, strPatHospCode, strHospCode, 0, strOperatorId, strAdmitDate, strEventDate, CInt(strPid), strInsType, intBatchId)) ' All Rule passed
                                                'lookDebug1
                                            End If

                                        Next

                                        sbAllRuleInsert.Append("</root>")
                                        If sbAllRuleInsert.ToString() <> "<root></root>" Then
                                            iDataRules = insertAllRuleResultsByXML(sbAllRuleInsert.ToString(), strPatHospCode, CInt(strPid))
                                        End If
                                    Next ' call once for all each patient_nubmber
                                Next
                            Else



                                Using ds As New DataSets()
                                    ds.ConnectionString = _ConnectionString
                                    ds.updateValidateListProcessed(_UserID, _BatchID)
                                    '' updateValidateListProcessed()
                                End Using



                                Exit Do
                            End If '-- iRowsToProcess
                        End If

                        Using ds As New DataSets()
                            ds.ConnectionString = _ConnectionString
                            ds.updateValidateListProcessed(_UserID, _BatchID)
                            '' updateValidateListProcessed()
                        End Using
                    Loop

                End If ' -- intValidateRows


            Catch ex As System.Exception
                errMessage = ex.Message
            End Try



        End Function










    End Class

End Namespace
