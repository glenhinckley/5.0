Option Explicit On

Imports Microsoft.VisualBasic.Compatibility

Imports ADODB.CommandTypeEnum
Imports ADODB.CompareEnum
Imports ADODB.ParameterAttributesEnum



Imports ADODB.DataTypeEnum
Imports ADODB.ParameterDirectionEnum
Imports ADODB.ExecuteOptionEnum

Imports ADODB.CursorTypeEnum
Imports ADODB.CursorLocationEnum
Imports ADODB.CursorOptionEnum
Imports ADODB.LockTypeEnum
'--------------------------- Lydia Orth 

Imports Psyquel.BusinessRules.CoreLibraryIII

Imports EDIBZ


Public Module mod835

        Private _DB As New dbStuff.db
    Private _MD As New dbStuff.ModCommon
    Private _ConnectionString As String = String.Empty



    Dim ErrorMessage, Reason1, Reason2, Reason3, Reason4, Reason5, Reason6, Reason7, Reason8 As String
    Dim fldDenTxTypeID, fldNoteTxTypeID As Long
    Dim Charge, Payment, Deductible, Coinsurance, PatientResp, Interest, Disallowed, OtherDisallowed As Double
    Dim strSystemErr As String

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property

    Function TranslateERA(ByVal ernDir As String, ByVal inFile As String, ByVal curFile As String, ByVal ernFile As String, ByVal fldClearingHouseID As Integer)

        Dim wrkDate
        Dim MyChar, PreviousChar
        Dim Cnt, cntWrk As Long
        Dim strData As String
        Dim PayorID
        Dim strSegmentTerminator As String
        Dim strElementSeperator As String
        Dim strSubElementSeperator As String

        Dim fs, f
        Const ForReading = 1, ForWriting = 2, ForAppending = 8
        strData = "" : strSystemErr = ""
        fs = CreateObject("Scripting.FileSystemObject")
        f = fs.OpenTextFile(curFile, ForWriting, True, 0)
        strSegmentTerminator = ""


        Dim fileReader As String
        fileReader = My.Computer.FileSystem.ReadAllText(curFile)








        'Open inFile For Input As #1    ' Open file.
        '        Do While Not EOF(1)    ' Loop until end of file.
        '      MyChar = Input(1, #1)    ' Get one character.
        '            'Debug.Print Asc(MyChar)    ' Print to the Immediate window.






        If Asc(MyChar) = 10 And Cnt = 105 And Mid(strData, 1, 3) = "ISA" Then
            strSegmentTerminator = MyChar
        End If


        If (Asc(MyChar) = 10 Or Asc(MyChar) = 13) And strSegmentTerminator <> MyChar Then
            MyChar = ""
        Else
            Cnt = Cnt + 1
            If Cnt > 0 Then
                If Cnt = 104 And Mid(strData, 1, 3) = "ISA" Then strElementSeperator = MyChar
                If Cnt = 105 And Mid(strData, 1, 3) = "ISA" Then strSubElementSeperator = MyChar
                If Cnt = 106 And Mid(strData, 1, 3) = "ISA" Then strSegmentTerminator = MyChar
                '     ascChar = Asc(MyChar)
                If Cnt > 70 And Mid(strData, 1, 3) = "ISA" Then
                    PayorID = Trim(Mid(strData, 55, 15))      'medicaid of NC uses a CR for end of segment
                End If
                If MyChar = strSubElementSeperator Then MyChar = ":"
                If MyChar = strElementSeperator Then MyChar = "*"
                If MyChar = strSegmentTerminator Then MyChar = "~"
                If MyChar = "~" Then
                    Cnt = Cnt + 1
                    If strElementSeperator <> "*" And Mid(strData, 1, 3) = "ISA" Then strData = Replace(strData, strElementSeperator, "*")
                    If Trim(strData) > "" Then
                        strData = Trim(strData) & Chr(13) & Chr(10)
                        strData = Replace(strData, "&nbsp;", " ", 1, , vbBinaryCompare)
                        f.Write(strData)
                    End If
                    Cnt = 0
                    strData = ""
                    MyChar = ""
                End If
                strData = strData + MyChar
            End If
        End If
        'Loop
        'Close #1    ' Close file.
        '        f.Close()

        'Open (ernDir & "\Schema.ini") For Output As #2
        'Print #2, "[" & ernFile & ".txt]"
        'Print #2, "Format=Delimited(*)"
        'Print #2, "ColNameHeader = False"
        'Print #2, ""
        'Print #2, "Col1=F1 TEXT"
        'Print #2, "Col2=F2 TEXT"
        'Print #2, "Col3=F3 TEXT"
        'Print #2, "Col4=F4 TEXT"
        'Print #2, "Col5=F5 TEXT"
        'Print #2, "Col6=F6 TEXT"
        'Print #2, "Col7=F7 TEXT"
        'Print #2, "Col8=F8 TEXT"
        'Print #2, "Col9=F9 TEXT"
        'Print #2, "Col10=F10 TEXT"
        'Print #2, "Col11=F11 TEXT"
        'Print #2, "Col12=F12 TEXT"
        'Print #2, "Col13=F13 TEXT"
        'Print #2, "Col14=F14 TEXT"
        'Print #2, "Col15=F15 TEXT"
        'Print #2, "Col16=F16 TEXT"
        'Print #2, "Col17=F17 TEXT"
        'Print #2, "Col18=F18 TEXT"
        'Print #2, "Col19=F19 TEXT"
        'Print #2, "Col20=F20 TEXT"
        'Close #2 'Close the file

    End Function
    Function ImportRemittanceFiles(ByVal ernDir As String, ByVal inFile As String, ByVal curFile As String, ByVal ernFile As String, ByVal fldClearingHouseID As Integer)

        Dim Found, PostAmount, CoInsAmount, EndProcess, FoundCAS, FoundCLP, FoundCheck, FoundPayment As Boolean
        Dim strData As String
        Dim Msg, Style, Title As String
        Dim intID As String
        Dim ReceiverID, InsuranceType As String
        Dim PayorID, Provider, ProviderID, HealthPlan, HealthPlanName, BatchNumber, CheckNumber, PatientID, LineNbr, ErrMsg, PatFirstName, PatLastName, strAddr, strCity As String
        Dim ClaimReceiveDate, ReceiveDate, CreationDate, CheckDate, RemittanceDate, ProcessDate, PatientDOB, FirstDateOfService, LastDateOfService As Date
        Dim BatchNbr, SeqNbr As Integer
        Dim CheckAmount, Allowed, Disallow, AmountPaid, Balance, PatBalance
        Dim strID, strSQL, strSQL1, RecID, strSeg01, strSeg02, Mod1, Mod2, Mod3, ProcedureCode, PrevProcedureCode, fldPrintHcfaYN, fldDepositNum As String
        Dim RecordID, RecordDetailID, PayerCheckID, PayerCheckLogID, fldGroupID, fldProviderID, intMaxEncID, fldEncounterLogID, fldEncDetailID, prevEncounterLogID, intMaxPatID, fldPatientID, fldOwnerID As Long
        Dim fldPmtTxTypeID, fldDisTxTypeID, fldDedTxTypeID, fldCoInsTxTypeID, fldRefTxTypeID, fldNoteTxTypeID, fldPlanID, fldOrder, lngCPTRecordID As Long
        Dim MedicalRecordNumber, SupplementalInsurerName1, SupplementalInsurerID1, PatientNumber As String
        Dim InsuredPolicyID, InsuredGroupNbr, InsuredLastName, InsuredFirstName, InsuredMiddleInitial, PolicyID As String
        Dim PatientPolicyID, PatientGroupNbr, PatientLastName, PatientFirstName, PatientMiddleInitial, PatientSex As String
        Dim MessageCode1, MessageCode2, MessageCode3, MessageCode4, MessageCode5, Posted As String
        Dim I, LineNumber, ServiceLineNumber, Units As Long
        Dim PlaceOfService, TypeOfService, PLBHeader As String, PLBMessage As String
        ErrorMessage = "" : Reason1 = "" : Reason2 = "" : Reason3 = "" : Reason4 = "" : Reason5 = "" : Reason6 = "" : Reason7 = "" : PatientDOB = 0 : PLBMessage = ""
        MedicalRecordNumber = "" : SupplementalInsurerName1 = "" : SupplementalInsurerID1 = "" : PatientNumber = ""

        Dim fs, f
        Const ForReading = 1, ForWriting = 2, ForAppending = 8
        strData = "" : strSystemErr = ""
        fs = CreateObject("Scripting.FileSystemObject")
        'fldClearingHouseID = 51
         TranslateERA(ernDir, inFile, curFile, ernFile, fldClearingHouseID)

        On Error GoTo ErrTrap

        Dim objConn As ADODB.Connection
        Dim rst As ADODB.Recordset
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim objSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset

        Dim objCheck As PostingBz.CCheckBz
        Dim rstCheck As ADODB.Recordset
        Dim objCheckLog As PostingBz.CCheckLogBZ
        Dim rstCheckLog As ADODB.Recordset
        Dim objTx As PostingBz.CTxBz
        Dim rstTx As ADODB.Recordset
        Dim objRemit As EDIBz.CRemitBZ
        Dim objRemitDetail As EDIBz.CRemitDetailBz

        objConn = CreateObject("ADODB.Connection")
        rst = CreateObject("ADODB.Recordset")

        'Set the database connection
        objConn.Open("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & ernDir & ";Extended Properties='text';")
        '                   "Extended Properties='text;HDR=YES;FMT=Delimited(*)';"

        'Query from the file assuming it as a database table
        rst.Open("SELECT * FROM [" & ernFile & ".txt]", objConn, adOpenStatic)  ', adLockOptimistic, adCmdText

        'NO FILES QUEUED
        If rst.EOF Then
            ' Title = "Invalid Remittance File"
            ' intID = MsgBox("Invalid Remittance File Format " & ernFile & ", ISA Record Missing!", vbOKOnly)
            rst.Close()
            'delete remittance file from ElectronicClaims Folder
            If Dir(inFile) > "" Then
                If Dir(Mid(inFile, 1, InStr(1, inFile, "\835\") + 4) & "Archive\" & Mid(inFile, InStr(1, inFile, "\835\") + 5)) > "" Then
                     fs.DeleteFile(Mid(inFile, 1, InStr(1, inFile, "\835\") + 4) & "Archive\" & Mid(inFile, InStr(1, inFile, "\835\") + 5))
                End If
                 fs.MoveFile(inFile, Mid(inFile, 1, InStr(1, inFile, "\835\") + 4) & "Archive\" & Mid(inFile, InStr(1, inFile, "\835\") + 5))
            End If
            Exit Function
        End If   'delete remittance file from ElectronicClaims Folder

        If rst.Fields.Item("F1") <> "ISA" Then
            ' Title = "Invalid Remittance File"
            ' intID = MsgBox("Invalid Remittance File Format " & ernFile & ", ISA Record Missing!", vbOKOnly)
            rst.Close()
            'delete remittance file from ElectronicClaims Folder
            If Dir(inFile) > "" Then
                If Dir(Mid(inFile, 1, InStr(1, inFile, "\835\") + 4) & "Archive\" & Mid(inFile, InStr(1, inFile, "\835\") + 5)) > "" Then
                     fs.DeleteFile(Mid(inFile, 1, InStr(1, inFile, "\835\") + 4) & "Archive\" & Mid(inFile, InStr(1, inFile, "\835\") + 5))
                End If
                 fs.MoveFile(inFile, Mid(inFile, 1, InStr(1, inFile, "\835\") + 4) & "Archive\" & Mid(inFile, InStr(1, inFile, "\835\") + 5))
            End If
            Exit Function
        End If   'delete remittance file from ElectronicClaims Folder

        objSQL = CreateObject("ADODB.Connection")
        objSQL.Open(_Connectionstring)

        strSQL = "SELECT fldNextID FROM tblID WHERE fldTableName = 'tblBenefactor'"
        rstSQL = CreateObject("ADODB.Recordset")
        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
        intMaxPatID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldNextID"))
        rstSQL = Nothing
        strSQL = "SELECT fldNextID FROM tblID WHERE fldTableName = 'tblEncounterLog'"
        rstSQL = CreateObject("ADODB.Recordset")
        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
        intMaxEncID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldNextID"))
        rstSQL = Nothing

        Do While Not rst.EOF
            RecID = rst.Fields.Item("F1")
            If (RecID = "ISA") Or (RecID = "GE") Or (RecID = "ST") Then
                If (RecID = "GE") Then
                    If PayerCheckID = 0 Then
                        PayerCheckID = FindCheck(Convert.ToString(CheckNumber), CDate(CheckDate), Convert.ToDouble(CheckAmount), HealthPlan, fldProviderID, fldGroupID)
                    End If
                    rstSQL = Nothing
                    objCheck = CreateObject("PostingBz.CCheckBz")
                    If PayerCheckID = 0 Then
                        If fldGroupID <= 0 Or fldGroupID = 680 Or IsDBNull(fldGroupID) Then
                            fldGroupID = 680
                            PayerCheckID = objCheck.Insert(HealthPlan, CheckNumber, "000", CDate(CheckDate), Format(Now(), "Short Date"), Convert.ToDecimal(CheckAmount), "system", fldProviderID, fldGroupID, False, Convert.ToString("Electronic EOB"), True)
                        Else
                            PayerCheckID = objCheck.Insert(HealthPlan, CheckNumber, "000", CDate(CheckDate), Format(Now(), "Short Date"), Convert.ToDecimal(CheckAmount), "system", -1, fldGroupID, False, Convert.ToString("Electronic EOB"), True)
                        End If
                        objCheck = Nothing
                        FoundCheck = False
                    Else
                        FoundCheck = True
                    End If
                    objCheck = Nothing
                    fldGroupID = 0 : fldProviderID = 0 : ProviderID = "" : Provider = "" : FoundCAS = False : FoundCLP = False : fldDepositNum = "000"
                    fldPatientID = 0 : fldEncounterLogID = 0 : fldEncDetailID = 0 : prevEncounterLogID = 0
                    MedicalRecordNumber = "" : SupplementalInsurerName1 = "" : SupplementalInsurerID1 = "" : PatientNumber = ""
                    rst.MoveNext()
                    If IsDBNull(rst.Fields.Item("F1")) Then rst.MoveNext()
                    RecID = rst.Fields("F1")
                ElseIf (RecID = "IEA") Then
                    rst.MoveNext()
                    If IsDBNull(rst.Fields.Item("F1")) Then rst.MoveNext()
                End If
                Do While (RecID <> "CLP") And (RecID <> "GE") And Not rst.EOF
                    Select Case RecID
                        Case "ST"
                            If rst.Fields("F2") <> "835" Then Exit Function
                            HealthPlan = 0 : fldGroupID = 0 : fldProviderID = 0 : ProviderID = "" : fldDepositNum = "000" : FoundCheck = False : RecordID = 0 : RecordDetailID = 0
                        Case "ISA"
                            HealthPlan = 0 : fldGroupID = 0 : fldProviderID = 0 : ProviderID = "" : fldDepositNum = "000" : FoundCheck = False : PayorID = ""
                            CreationDate = DateSerial(Convert.ToInt32(Mid(rst.Fields("F10"), 1, 2)) + 2000, Convert.ToInt32(Mid(rst.Fields("F10"), 3, 2)), Convert.ToInt32(Mid(rst.Fields("F10"), 5, 2)))
                            PayorID = Trim(rst.Fields("F7"))
                            ReceiverID = IIf(IsDBNull(rst.Fields("F9")), PayorID, Trim(rst.Fields("F9")))
                        Case "GS"
                            If (rst.Fields("F2") = "HP" Or rst.Fields("F2") = "HP" Or rst.Fields("F2") = "FA") Then

                                PayorID = Replace(rst.Fields("F3"), "'", "")
                                BatchNumber = rst.Fields("F7")
                                HealthPlan = FindHealthPlan(Convert.ToString(RecID), Convert.ToString(rst.Fields("F2")), fldClearingHouseID, PayorID, ReceiverID, HealthPlan, HealthPlanName, fldEncounterLogID)

                                If Len(rst.Fields("F5")) = 6 Then
                                    RemittanceDate = DateSerial(Convert.ToInt32(Mid(rst.Fields("F5"), 1, 2)) + 2000, Convert.ToInt32(Mid(rst.Fields("F5"), 3, 2)), Convert.ToInt32(Mid(rst.Fields("F5"), 5, 2)))
                                Else
                                    RemittanceDate = DateSerial(Convert.ToInt32(Mid(rst.Fields("F5"), 1, 4)), Convert.ToInt32(Mid(rst.Fields("F5"), 5, 2)), Convert.ToInt32(Mid(rst.Fields("F5"), 7, 2)))
                                End If
                            End If
                            Do Until (HealthPlan > "")
                                Title = "Payor ID Missing"
                                Msg = "Please Enter a HealthPlan for this Response File!"
                                HealthPlan = InputBox(Msg, Title)
                                If HealthPlan = "exit" Or HealthPlan = "quit" Or HealthPlan = "" Then
                                    '        Kill "N:\Apps\QBill\Remittance\" & ernFile & ".txt"
                                    'DoCmd.SetWarnings True
                                    Exit Function
                                End If
                                strSQL = "SELECT fldInsName, fldPayerCode FROM tblInsCompany WHERE (fldInsuranceID = " & HealthPlan & ")"
                                rstSQL = CreateObject("ADODB.Recordset")
                                rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                HealthPlanName = IIf(rstSQL.EOF, Null, Left(rstSQL.Fields("HealthPlanName"), 33))
                                PayorID = IIf(rstSQL.EOF, "", rstSQL.Fields("fldPayerCode"))
                                rstSQL = Nothing
                                If IsDBNull(HealthPlanName) Then
                                    Title = "Invalid Health Plan Entered!"
                                    Msg = "Please Enter a Valid HealthPlan!"
                                    HealthPlan = InputBox(Msg, Title)
                                    strSQL = "SELECT fldInsName, fldPayerCode FROM tblInsCompany WHERE (fldInsuranceID = " & HealthPlan & ")"
                                    rstSQL = CreateObject("ADODB.Recordset")
                                    rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                    HealthPlanName = IIf(rstSQL.EOF, System.DBNull, Left(rstSQL.Fields("HealthPlanName"), 33))
                                    PayorID = IIf(rstSQL.EOF, "", rstSQL.Fields("fldPayerCode"))
                                    rstSQL = Nothing
                                End If
                                If HealthPlanName = "exit" Or HealthPlanName = "quit" Then Exit Function
                                If IsDBNull(HealthPlanName) Or HealthPlan = "" Then HealthPlan = 0
                            Loop
                            If HealthPlan > 0 Then
                                strSQL = "SELECT fldInsName, fldPayerCode FROM tblInsCompany WHERE (fldInsuranceID = " & HealthPlan & ")"
                                rstSQL = CreateObject("ADODB.Recordset")
                                rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                HealthPlanName = IIf(rstSQL.EOF, Null, Left(rstSQL.Fields("fldInsName"), 33))
                                rstSQL = Nothing
                            End If
                        Case "N1"
                            If (rst.Fields("F2") = "PE") Then
                                Provider = IIf(IsDBNull(rst.Fields("F3")), "", Trim(rst.Fields("F3")))
                                ProviderID = Replace(rst.Fields("F5"), "~", "")

                                If fldProviderID <= 0 Then fldProviderID = FindProvider(rst, fldProviderID, 0, 0, HealthPlan)

                                If fldProviderID > 0 And fldGroupID = 0 Then
                                    strSQL = "SELECT fldGroupID FROM tblProvider WHERE (fldProviderID = " & fldProviderID & " AND fldDisabledYN = 'N')"
                                    rstSQL = CreateObject("ADODB.Recordset")
                                    rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                    fldGroupID = IIf(rstSQL.EOF, fldGroupID, rstSQL.Fields("fldGroupID"))
                                    rstSQL = Nothing
                                End If

                                If fldGroupID = 0 Then
                                    strSQL = "SELECT fldGroupID FROM tblProviderGroup WHERE (fldGroupNPI = '" + Convert.ToString(rst.Fields("F5")) + "' AND fldDisabledYN = 'N')"
                                    rstSQL = CreateObject("ADODB.Recordset")
                                    rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                    fldGroupID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldGroupID"))
                                    rstSQL = Nothing
                                End If
                            End If
                            If (rst.Fields("F2") = "PR") Then
                                HealthPlanName = Trim(Left(rst.Fields("F3"), 33))
                                If Not IsDBNull(rst.Fields("F5")) And PayorID <> IIf(IsDBNull(rst.Fields("F5")), "", Trim(rst.Fields("F5"))) Then HealthPlan = 0
                                If Not IsDBNull(rst.Fields("F5")) Then
                                    PayorID = IIf(IsDBNull(rst.Fields("F5")), PayorID, Trim(rst.Fields("F5")))
                                Else
                                    strAddr = "" : strCity = ""
                                    rst.MoveNext()
                                    If IsDBNull(rst.Fields.Item("F1")) Then rst.MoveNext()
                                    If Not rst.EOF Then RecID = rst.Fields.Item("F1")
                                    If RecID = "N3" Then
                                        strAddr = Trim(rst.Fields("F2"))
                                        rst.MoveNext()
                                        If IsDBNull(rst.Fields.Item("F1")) Then rst.MoveNext()
                                        If Not rst.EOF Then RecID = rst.Fields.Item("F1")
                                    End If
                                    If RecID = "N4" Then
                                        strCity = Trim(rst.Fields("F2"))
                                        If InStr(1, strAddr, "BOX") > 0 Then
                                            strSQL = "SELECT fldInsuranceID, fldPayerCode FROM tblClaimsProcCtr WHERE (fldAddress1 Like '%" & Trim(Mid(strAddr, InStr(1, strAddr, "BOX"))) & "' AND fldCity like '%" & Trim(strCity) & "%' AND fldDisabledYN = 'N') ORDER BY fldInsuranceID "
                                            rstSQL = CreateObject("ADODB.Recordset")
                                            rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                            If rstSQL.EOF Then
                                                strSQL = "SELECT fldInsuranceID, fldPayerCode FROM tblClaimsProcCtr WHERE (fldAddress1 Like '%" & Trim(Mid(strAddr, InStr(1, strAddr, "BOX"))) & "' AND fldCity like '%" & Trim(strCity) & "%' AND fldDisabledYN = 'Y') ORDER BY fldInsuranceID "
                                                rstSQL = CreateObject("ADODB.Recordset")
                                                rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                                If rstSQL.EOF Then
                                                End If
                                            End If
                                        Else
                                            strSQL = "SELECT fldInsuranceID, fldPayerCode FROM tblClaimsProcCtr WHERE (fldPayerCode = '" & PayorID & "' AND fldAddress1 Like '%" & Trim(strAddr) & "%' AND fldCity like '%" & Trim(strCity) & "%' AND fldDisabledYN = 'N') ORDER BY fldInsuranceID "
                                            rstSQL = CreateObject("ADODB.Recordset")
                                            rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                            If rstSQL.EOF Then
                                                strSQL = "SELECT fldInsuranceID, fldPayerCode FROM tblClaimsProcCtr WHERE (fldAddress1 Like '%" & Trim(strAddr) & "%' AND fldCity like '%" & Trim(strCity) & "%' AND fldDisabledYN = 'N') ORDER BY fldInsuranceID "
                                                rstSQL = CreateObject("ADODB.Recordset")
                                                rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                            End If
                                        End If
                                        If Not rstSQL.EOF Then
                                            If HealthPlan = 0 Then PayorID = IIf(IsDBNull(rstSQL.Fields("fldPayerCode")), PayorID, rstSQL.Fields("fldPayerCode"))
                                            If HealthPlan = 0 Then HealthPlan = IIf(IsDBNull(rstSQL.Fields("fldInsuranceID")), HealthPlan, rstSQL.Fields("fldInsuranceID"))
                                        End If
                                        rstSQL = Nothing
                                    End If
                                End If
                                HealthPlan = FindHealthPlan(RecID, rst.Fields("F2"), fldClearingHouseID, PayorID, ReceiverID, HealthPlan, HealthPlanName, fldEncounterLogID)
                            End If
                        Case "N3"
                            If InStr(1, Convert.ToString(rst.Fields("F2")), "200837") > 0 Or InStr(1, Convert.ToString(rst.Fields("F3")), "200837") Or _
                            InStr(1, Convert.ToString(rst.Fields("F2")), "691993") > 0 Or InStr(1, Convert.ToString(rst.Fields("F3")), "691993") Or _
                            InStr(1, Convert.ToString(rst.Fields("F2")).ToString, "8860") > 0 Or InStr(1, Convert.ToString(rst.Fields("F3")), "8860") Then
                                If HealthPlan = 24 And Left(Convert.ToString(CheckNumber), 1) = "8" Then
                                    fldDepositNum = "000"
                                Else
                                    fldDepositNum = "200837"
                                End If
                            End If
                        Case "N4"
                            If HealthPlan = 1281 And rst.Fields("F3") = "OH" Then HealthPlan = 1301
                        Case "REF"
                            If (rst.Fields("F2") = "2U") Then


                                PayorID = Trim(rst.Fields("F3"))



                                '************************************************************************************************************************************************
                                '  left trim of PayorID so it does not overflow db field  --- ghinckley 02-13-3-2024
                                '************************************************************************************************************************************************

                                PayorID = Left(PayorID, 32)





                                If HealthPlan = 0 Then HealthPlan = FindHealthPlan(RecID, rst.Fields("F2"), fldClearingHouseID, PayorID, ReceiverID, HealthPlan, HealthPlanName, fldEncounterLogID)
                                CheckNumber = FindCheckNumber(CheckNumber, PayorID, HealthPlan, fldClearingHouseID, IIf(IsDBNull(rst.Fields("F5")), "", rst.Fields("F5")))
                                If HealthPlan = 0 Then
                                    strSQL = "SELECT fldInsuranceID, fldPayerCode FROM tblClaimsProcCtr WHERE (fldPayerCode Like '%" & Trim(PayorID) & "%' AND fldCity like '%" & Trim(strCity) & "%' AND fldDisabledYN = 'N') ORDER BY fldInsuranceID "
                                    rstSQL = CreateObject("ADODB.Recordset")
                                    rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                    If Not rstSQL.EOF Then
                                        HealthPlan = IIf(IsDBNull(rstSQL.Fields("fldInsuranceID")), HealthPlan, rstSQL.Fields("fldInsuranceID"))
                                    End If
                                    rstSQL = Nothing
                                End If
                                HealthPlan = FindHealthPlan(RecID, rst.Fields("F2"), fldClearingHouseID, PayorID, ReceiverID, HealthPlan, HealthPlanName, fldEncounterLogID)
                            End If
                            If (rst.Fields("F2") = "1C" Or rst.Fields("F2") = "1D" Or rst.Fields("F2") = "1B" Or rst.Fields("F2") = "B3" _
                                  Or rst.Fields("F2") = "1H" Or rst.Fields("F2") = "G2" Or rst.Fields("F2") = "PQ" Or rst.Fields("F2") = "XX") Then

                                If fldProviderID <= 0 Then fldProviderID = FindProvider(rst, fldProviderID, 0, 0, HealthPlan)

                                If rst.Fields("F2") = "1C" Then
                                    If fldProviderID > 0 Then
                                        strSQL = "SELECT fldFirstName + ' ' + fldLastName as Provider FROM tblUser WHERE (fldUserID = " & fldProviderID & ")"
                                        rstSQL = CreateObject("ADODB.Recordset")
                                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                        Provider = IIf(rstSQL.EOF, "", rstSQL.Fields("Provider"))
                                        rstSQL = Nothing
                                    End If
                                End If
                                If fldProviderID > 0 Then
                                    strSQL = "SELECT fldGroupID FROM tblProvider WHERE (fldProviderID = " & fldProviderID & " AND fldDisabledYN = 'N')"
                                    rstSQL = CreateObject("ADODB.Recordset")
                                    rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                    fldGroupID = IIf(rstSQL.EOF, fldGroupID, rstSQL.Fields("fldGroupID"))
                                    rstSQL = Nothing
                                End If
                            End If
                        Case "BPR"
                            CheckAmount = rst.Fields("F3")
                            If Len(rst.Fields("F17")) = 6 Then
                                CheckDate = DateSerial(Convert.ToInt32(Mid(rst.Fields("F17"), 1, 2)) + 2000, Convert.ToInt32(Mid(rst.Fields("F17"), 3, 2)), Mid(Convert.rst.Fields("F17"), 5, 2))
                            Else
                                CheckDate = DateSerial(Convert.ToInt32(Mid(rst.Fields("F17"), 1, 4)), Convert.ToInt32(Mid(rst.Fields("F17"), 5, 2)), Convert.ToInt32(Mid(rst.Fields("F17"), 7, 2)))
                            End If
                        Case "TRN"
                            fldDepositNum = "000" : FoundCheck = False : PayerCheckID = 0
                            If fldClearingHouseID = 150 And PayorID = "CLAIMMD" And HealthPlanName Like "Prairie States*" Then PayorID = "36373"
                            If HealthPlan = 0 Then HealthPlan = FindHealthPlan(Convert.ToString(RecID), Convert.ToString(rst.Fields("F2")), fldClearingHouseID, PayorID, ReceiverID, HealthPlan, HealthPlanName, fldEncounterLogID)
                            CheckNumber = FindCheckNumber(rst.Fields("F3"), PayorID, HealthPlan, fldClearingHouseID, IIf(IsDBNull(rst.Fields("F5")), "", rst.Fields("F5")))
                        Case "DTM"
                            If IsDBNull(rst.Fields("F3")) Then
                                ProcessDate = Format(Now(), "Short Date")
                            ElseIf Len(rst.Fields("F3")) = 6 Then
                                ProcessDate = DateSerial(Convert.ToInt32(Mid(rst.Fields("F3"), 1, 2)) + 2000, Convert.ToInt32(Mid(rst.Fields("F3"), 3, 2)), Convert.ToInt32(Mid(rst.Fields("F3"), 5, 2)))
                            Else
                                ProcessDate = DateSerial(Convert.ToInt32(Mid(rst.Fields("F3"), 1, 4)), Convert.ToInt32(Mid(rst.Fields("F3"), 5, 2)), Convert.ToInt32(Mid(rst.Fields("F3"), 7, 2)))
                            End If
                        Case "TS3"
                            If IsDBNull(fldProviderID) Or (fldProviderID <= "") Or (fldProviderID = 0) Or IsEmpty(fldProviderID) Then
                                If Trim(rst.Fields("F3")) = "11" Then     'Facility Code

                                    If fldProviderID <= 0 Then fldProviderID = FindProvider(rst, fldProviderID, 0, 0, HealthPlan)

                                    If fldProviderID > 0 And fldGroupID = 0 Then
                                        strSQL = "SELECT fldGroupID FROM tblProvider WHERE (fldProviderID = " & fldProviderID & " AND fldDisabledYN = 'N')"
                                        rstSQL = CreateObject("ADODB.Recordset")
                                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                        fldGroupID = IIf(rstSQL.EOF, fldGroupID, rstSQL.Fields("fldGroupID"))
                                        rstSQL = Nothing
                                    End If
                                    If fldGroupID = 0 Then
                                        strSQL = "SELECT fldGroupID FROM tblProviderGroup WHERE (fldGroupNPI = '" & Convert.ToString(rst.Fields("F2")) & "' AND fldDisabledYN = 'N')"
                                        rstSQL = CreateObject("ADODB.Recordset")
                                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                        fldGroupID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldGroupID"))
                                        rstSQL = Nothing
                                    End If
                                End If
                            End If
                        Case "LX"
                        Case "SE"
                    End Select
                    rst.MoveNext()
                    If IsDBNull(rst.Fields.Item("F1")) Then rst.MoveNext()
                    If Not rst.EOF Then
                        RecID = rst.Fields.Item("F1")
                        If (RecID = "CLP") Then 'First time finding the CLP segment create a Check
                            If HealthPlan = 0 Then
                                PatientID = IIf(Len(Trim(rst.Fields("F2"))) > 9, "0", IIf(Not IsNumeric(Trim(rst.Fields("F2"))), "0", Trim(_MD.NumbersOnly(rst.Fields("F2")))))
                                If CLng(PatientID) > 0 Then
                                    HealthPlan = FindHealthPlan(RecID, rst.Fields("F2"), fldClearingHouseID, PayorID, ReceiverID, HealthPlan, HealthPlanName, CLng(PatientID))
                                End If
                            End If


                            If CLng(PayerCheckID) = 0 Then
                                PayerCheckID = FindCheck(Convert.ToString(CheckNumber), CDate(CheckDate), Convert.ToDouble(CheckAmount), HealthPlan, fldProviderID, fldGroupID)
                            End If
                            rstSQL = Nothing
                            objCheck = CreateObject("PostingBz.CCheckBz")
                            If PayerCheckID = 0 Then
                                If fldGroupID <= 0 Or fldGroupID = 680 Or IsDBNull(fldGroupID) Then
                                    fldGroupID = 680
                                    PayerCheckID = objCheck.Insert(HealthPlan, CheckNumber, "000", CDate(CheckDate), Format(Now(), "Short Date"), Convert.ToDecimal(CheckAmount), "system", fldProviderID, fldGroupID, False, Convert.ToString("Electronic EOB"), True)
                                Else
                                    PayerCheckID = objCheck.Insert(HealthPlan, CheckNumber, "000", CDate(CheckDate), Format(Now(), "Short Date"), Convert.ToDecimal(CheckAmount), "system", -1, fldGroupID, False, Convert.ToString("Electronic EOB"), True)
                                End If
                                FoundCheck = False
                            Else
                                FoundCheck = True
                            End If
                            objCheck = Nothing

                        End If
                        If (RecID = "IEA") Then rst.MoveNext()
                    End If
                Loop
            ElseIf (RecID = "CLP") Then
                Do While RecID <> "GE" And RecID <> "ST"
                    If (RecID = "CLP") Then    'Still on the same CLP segment, check has been created.
                        ProcedureCode = "" : PrevProcedureCode = "" : FirstDateOfService = Nothing : LastDateOfService = Nothing
                        Units = 0 : Charge = 0 : Payment = 0 : Deductible = 0 : Coinsurance = 0 : Interest = 0 : PatientResp = 0 : Disallowed = 0 : OtherDisallowed = 0 : Allowed = 0 : Balance = 0 : PatBalance = 0
                        ErrorMessage = "" : Reason1 = "" : Reason2 = "" : Reason3 = "" : Reason4 = "" : Reason5 = "" : Reason6 = "" : Reason7 = "" : Reason8 = ""
                        Do While RecID <> "SVC" And RecID <> "SE" And RecID <> "LX"         'And RecID <> "PLB"
                            Select Case RecID
                                Case "CLP"
                                    FoundCAS = False : FoundCLP = True
                                    PatientID = IIf(Len(Trim(rst.Fields("F2"))) > 9, "0", IIf(Not IsNumeric(Trim(rst.Fields("F2"))), "0", Trim(_MD.NumbersOnly(rst.Fields("F2")))))
                                    fldPatientID = 0
                                    fldEncounterLogID = 0 : fldEncDetailID = 0 : prevEncounterLogID = 0
                                    fldDenTxTypeID = 0 : fldNoteTxTypeID = 0
                                    PatFirstName = "" : PatLastName = ""
                                    If Not IsNumeric(PatientID) Then PatientID = "0"
                                    If Len(PatientID) > 8 Then PatientID = "0"
                                    If Len(PatientID) < 5 Then PatientID = "0"
                                    If IsNumeric(PatientID) Then
                                        If CLng(Left(PatientID, 9)) < intMaxPatID Then PatientID = "0"
                                        If CLng(Left(PatientID, 9)) > intMaxEncID Then PatientID = "0"
                                        If (Len(PatientID) = 7 Or Len(PatientID) = 8) And CLng(PatientID) < 5000000 Then PatientID = "0"
                                        If Len(PatientID) = 5 And CLng(PatientID) < 50000 Then PatientID = "0"
                                    End If
                                    If IsNumeric(PatientID) Then
                                        If CLng(PatientID) > 0 Then fldPatientID = FindPatient(PatientID, fldProviderID, fldGroupID, intMaxPatID, fldOwnerID)
                                        If fldPatientID > 0 Then
                                            strSQL = "SELECT A.fldBenefactorID, A.fldOwnerID, C.fldProviderID, C.fldGroupID " & _
                                                    "FROM tblBenefactor As A INNER JOIN " & _
                                                    "tblPatientProvider AS B ON A.fldBenefactorID = B.fldPatientID INNER JOIN " & _
                                                    "tblProvider AS C ON B.fldProviderID = C.fldProviderID " & _
                                                    "WHERE (A.fldBenefactorID = " & Trim(fldPatientID) & " AND fldOwnerID IS NOT NULL )"
                                            rstSQL = CreateObject("ADODB.Recordset")
                                            rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                            fldPatientID = IIf(rstSQL.EOF, fldPatientID, rstSQL.Fields("fldBenefactorID"))
                                            fldOwnerID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldOwnerID"))
                                            If fldGroupID <> 680 And fldGroupID = IIf(rstSQL.EOF, -1, rstSQL.Fields("fldGroupID")) Then
                                                fldProviderID = fldOwnerID
                                            End If
                                        End If
                                        rstSQL = Nothing
                                        If CLng(Left(PatientID, 9)) > intMaxPatID And CLng(Left(PatientID, 9)) <= intMaxEncID Then
                                            fldEncounterLogID = FindEncounter(CLng(Left(PatientID, 9)), fldPatientID, fldProviderID, fldGroupID, fldOwnerID, FirstDateOfService, ProcedureCode, PatFirstName, PatLastName)
                                            If fldEncounterLogID > 0 And (fldPatientID = 0) Then
                                                strSQL = "SELECT A.fldPatientID, A.fldEncounterLogID, A.fldProviderID, B.fldGroupID FROM tblEncounterLog AS A INNER JOIN tblProvider AS B ON A.fldProviderID = B.fldProviderID WHERE (A.fldEncounterLogID =" & fldEncounterLogID & ")"
                                                rstSQL = CreateObject("ADODB.Recordset")
                                                rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                                fldPatientID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldPatientID"))
                                                fldProviderID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldProviderID"))
                                                fldGroupID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldGroupID"))
                                                rstSQL = Nothing
                                            End If
                                        End If
                                    Else
                                        fldPatientID = 0
                                        fldEncounterLogID = 0
                                    End If
                                    If IsNumeric(rst.Fields("F4")) Then
                                        Charge = rst.Fields("F4")
                                    End If
                                    If IsNumeric(rst.Fields("F5")) Then
                                        AmountPaid = rst.Fields("F5")
                                        Payment = rst.Fields("F5")
                                    End If
                                    If IsNumeric(rst.Fields("F6")) Then
                                        PatientResp = rst.Fields("F6")
                                    End If
                                    MedicalRecordNumber = Trim(rst.Fields("F8"))
                                    If IsNumeric(rst.Fields("F9")) Then
                                        PlaceOfService = Right(Trim(rst.Fields("F9")), 2)
                                    End If
                                    'claim frequency code
                                Case "NM1"
                                    If (rst.Fields("F2") = "PR" Or rst.Fields("F2") = "TT") And rst.Fields("F3") <> "1" Then
                                        SupplementalInsurerName1 = Replace(IIf(IsDBNull(rst.Fields("F4")), "", Trim(rst.Fields("F4"))), "'", "")
                                        SupplementalInsurerID1 = Trim(rst.Fields("F10"))
                                    End If
                                    If rst.Fields("F2") = "QC" Then
                                        If fldPatientID = 0 And fldProviderID > 0 Then
                                            strSQL = "SELECT fldBenefactorID, fldOwnerID FROM tblBenefactor WHERE (fldlast like '" & Replace(IIf(IsDBNull(rst.Fields("F4")), "", Trim(rst.Fields("F4"))), "'", "") & "%' AND fldFirst like '" & Replace(IIf(IsDBNull(rst.Fields("F5")), "", Trim(rst.Fields("F5"))), "'", "") & "%' AND fldOwnerID =" & fldProviderID & ")"
                                            rstSQL = CreateObject("ADODB.Recordset")
                                            rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                            fldPatientID = IIf(rstSQL.EOF, fldPatientID, rstSQL.Fields("fldBenefactorID"))
                                            fldOwnerID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldOwnerID"))
                                            rstSQL = Nothing
                                        End If
                                        If fldOwnerID = 0 And fldProviderID > 0 Then
                                            strSQL = "SELECT fldGroupID FROM tblProvider WHERE (fldProviderID = " & fldProviderID & " AND fldDisabledYN = 'N')"
                                            rstSQL = CreateObject("ADODB.Recordset")
                                            rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                            If fldGroupID <> 680 And fldGroupID = IIf(rstSQL.EOF, -1, rstSQL.Fields("fldGroupID")) Then
                                                rstSQL = Nothing
                                                strSQL = "SELECT fldBenefactorID, fldOwnerID " & _
                                                         "FROM tblBenefactor INNER JOIN " & _
                                                         "tblPatientProvider ON tblBenefactor.fldBenefactorID = tblPatientProvider.fldPatientID INNER JOIN " & _
                                                         "tblProvider ON tblPatientProvider.fldProviderID = tblProvider.fldProviderID " & _
                                                         "WHERE (tblBenefactor.fldlast like '" & Replace(IIf(IsDBNull(rst.Fields("F4")), "", Trim(rst.Fields("F4"))), "'", "") & "%' AND tblBenefactor.fldFirst like '" & Replace(IIf(IsDBNull(rst.Fields("F5")), "", Trim(rst.Fields("F5"))), "'", "") & "%' AND tblProvider.fldGroupID = " & fldGroupID & " AND fldOwnerID IS NOT NULL )"
                                                rstSQL = CreateObject("ADODB.Recordset")
                                                rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                                If fldPatientID = 0 Then fldPatientID = IIf(rstSQL.EOF, fldPatientID, rstSQL.Fields("fldBenefactorID"))
                                                fldOwnerID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldOwnerID"))
                                                rstSQL = Nothing
                                                If fldOwnerID > 0 And fldProviderID <> fldOwnerID Then
                                                    strSQL = "SELECT fldGroupID FROM tblProvider WHERE (fldProviderID = " & fldOwnerID & " AND fldDisabledYN = 'N')"
                                                    rstSQL = CreateObject("ADODB.Recordset")
                                                    rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                                    If fldGroupID <> 680 And fldGroupID = IIf(rstSQL.EOF, -1, rstSQL.Fields("fldGroupID")) Then
                                                        fldProviderID = fldOwnerID
                                                    End If
                                                    rstSQL = Nothing
                                                End If
                                            End If
                                            rstSQL = Nothing
                                            fldOwnerID = 0
                                            If fldGroupID = 680 Then fldOwnerID = fldProviderID
                                        End If
                                        PatientNumber = Trim(Left(PatientID, 20))
                                        InsuredPolicyID = IIf(IsDBNull(rst.Fields("F10")), "", Trim(rst.Fields("F10")))
                                        InsuredLastName = Replace(IIf(IsDBNull(rst.Fields("F4")), "", Trim(rst.Fields("F4"))), "'", "")
                                        InsuredFirstName = Replace(IIf(IsDBNull(rst.Fields("F5")), "", Trim(rst.Fields("F5"))), "'", "")
                                        InsuredMiddleInitial = IIf(IsDBNull(rst.Fields("F6")), "", Trim(Mid(rst.Fields("F6"), 1, 1)))
                                        PatientPolicyID = Right(IIf(IsDBNull(rst.Fields("F10")), "", Trim(rst.Fields("F10"))), 25)
                                        PatientLastName = Replace(IIf(IsDBNull(rst.Fields("F4")), "", Trim(rst.Fields("F4"))), "'", "")
                                        PatientFirstName = Replace(IIf(IsDBNull(rst.Fields("F5")), "", Trim(rst.Fields("F5"))), "'", "")
                                        PatientMiddleInitial = IIf(IsDBNull(rst.Fields("F6")), "", Trim(Mid(rst.Fields("F6"), 1, 1)))
                                        PatFirstName = IIf(IsDBNull(PatientFirstName), "", PatientFirstName)
                                        PatLastName = IIf(IsDBNull(PatientLastName), "", PatientLastName)
                                    End If
                                    If rst.Fields("F2") = "IL" Or rst.Fields("F2") = "74" Then
                                        PatientNumber = Trim(Left(PatientID, 20))
                                        InsuredPolicyID = IIf(IsDBNull(rst.Fields("F10")), "", Trim(rst.Fields("F10")))
                                        InsuredLastName = Replace(IIf(IsDBNull(rst.Fields("F4")), "", Trim(rst.Fields("F4"))), "'", "")
                                        InsuredFirstName = Replace(IIf(IsDBNull(rst.Fields("F5")), "", Trim(rst.Fields("F5"))), "'", "")
                                        InsuredMiddleInitial = IIf(IsDBNull(rst.Fields("F6")), "", Trim(Mid(rst.Fields("F6"), 1, 1)))
                                    End If


                                    If rst.Fields("F2") = "82" And (rst.Fields("F3") = "1" Or rst.Fields("F3") = "2") Then
                                        If Not IsDBNull(rst.Fields("F5")) And rst.Fields("F5") > "" Then
                                            If rst.Fields("F6") > "" Then
                                                If (Provider <= "" Or IsDBNull(Provider)) Then Provider = rst.Fields("F5") & " " & rst.Fields("F6") & " " & rst.Fields("F4")
                                                ProviderID = rst.Fields("F10")
                                                Provider = rst.Fields("F5") & " " & rst.Fields("F6") & " " & rst.Fields("F4")
                                            Else
                                                Provider = rst.Fields("F5") & " " & rst.Fields("F4")
                                            End If
                                        End If
                                        ProviderID = IIf(IsDBNull(rst.Fields("F10")), "", Trim(rst.Fields("F10")))

                                        If fldProviderID <= 0 Then fldProviderID = FindProvider(rst, fldProviderID, fldEncounterLogID, fldPatientID, HealthPlan)

                                        If fldProviderID > 0 And fldGroupID <= 0 Then
                                            strSQL = "SELECT fldGroupID FROM tblProvider WHERE (fldProviderID = " & fldProviderID & " AND fldDisabledYN = 'N')"
                                            rstSQL = CreateObject("ADODB.Recordset")
                                            rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                            fldGroupID = IIf(rstSQL.EOF, fldGroupID, rstSQL.Fields("fldGroupID"))
                                            rstSQL = Nothing
                                        End If
                                    End If
                                Case "DTM"
                                    If rst.Fields("F2") = "050" Then
                                        If Len(rst.Fields("F3")) = 6 Then
                                            ClaimReceiveDate = DateSerial(Convert.ToInt32(Mid(rst.Fields("F3"), 1, 2)) + 2000, Convert.ToInt32(Mid(rst.Fields("F3"), 3, 2)), Convert.ToInt32(Mid(rst.Fields("F3"), 5, 2)))
                                        Else
                                            ClaimReceiveDate = DateSerial(Convert.ToInt32(Mid(rst.Fields("F3"), 1, 4)), Convert.ToInt32(Mid(rst.Fields("F3"), 5, 2)), Convert.ToInt32(Mid(rst.Fields("F3"), 7, 2)))
                                        End If
                                    End If
                                    If (rst.Fields("F2") = "150") Or (rst.Fields("F2") = "232") Or (rst.Fields("F2") = "472") Then
                                        If Len(rst.Fields("F3")) = 6 Then
                                            FirstDateOfService = DateSerial(Convert.ToInt32(Mid(rst.Fields("F3"), 1, 2)) + 2000, Convert.ToInt32(Mid(rst.Fields("F3"), 3, 2)), Convert.ToInt32(Mid(rst.Fields("F3"), 5, 2)))
                                        Else
                                            FirstDateOfService = DateSerial(Convert.ToInt32(Mid(rst.Fields("F3"), 1, 4)), Convert.ToInt32(Mid(rst.Fields("F3"), 5, 2)), Convert.ToInt32(Mid(rst.Fields("F3"), 7, 2)))
                                        End If
                                    End If
                                    If (rst.Fields("F2") = "151") Or (rst.Fields("F2") = "233") Or (rst.Fields("F2") = "473") Then
                                        If Len(rst.Fields("F3")) = 6 Then
                                            LastDateOfService = DateSerial(Convert.ToInt32(Mid(rst.Fields("F3"), 1, 2)) + 2000, Convert.ToInt32(Mid(rst.Fields("F3"), 3, 2)), Convert.ToInt32(Mid(rst.Fields("F3"), 5, 2)))
                                        Else
                                            LastDateOfService = DateSerial(Convert.ToInt32(Mid(rst.Fields("F3"), 1, 4)), Convert.ToInt32(Mid(rst.Fields("F3"), 5, 2)), Convert.ToInt32(Mid(rst.Fields("F3"), 7, 2)))
                                        End If
                                    End If
                                Case "MOA"
                                    MessageCode1 = Trim(rst.Fields("F4"))
                                    MessageCode2 = Trim(rst.Fields("F5"))
                                    MessageCode3 = Trim(rst.Fields("F6"))
                                    MessageCode4 = Trim(rst.Fields("F7"))
                                    If MessageCode1 > "" Then
                                        ErrMsg = ""
                                        strSQL = "SELECT * FROM tblRemittanceErrorCodes WHERE (fldRemittanceErrorCode ='" & MessageCode1 & "');"
                                        rstSQL = CreateObject("ADODB.Recordset")
                                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                        If Not rstSQL.EOF Then
                                            ErrMsg = Trim(IIf(IsDBNull(rstSQL.Fields("fldRemittanceErrorDescription")), "", rstSQL.Fields("fldRemittanceErrorDescription")))
                                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And Convert.ToInt32(rstSQL.Fields("fldBillStatusID")) = 4 Then fldDenTxTypeID = Convert.Tolong(rstSQL.Fields("fldTxTypeID"))
                                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And Convert.ToInt32(rstSQL.Fields("fldBillStatusID")) = 21 Then fldNoteTxTypeID = Convert.Tolong(rstSQL.Fields("fldTxTypeID"))
                                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And IsDBNull(ErrorMessage) And ErrMsg > "" Then
                                                ErrorMessage = ErrMsg
                                            ElseIf IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And ErrMsg > "" Then
                                                ErrorMessage = ErrorMessage + "; " + ErrMsg
                                            End If
                                        End If
                                        rstSQL = Nothing
                                    End If
                                    If MessageCode2 > "" Then
                                        ErrMsg = ""
                                        strSQL = "SELECT * FROM tblRemittanceErrorCodes WHERE (fldRemittanceErrorCode ='" & MessageCode2 & "');"
                                        rstSQL = CreateObject("ADODB.Recordset")
                                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                        If Not rstSQL.EOF Then
                                            ErrMsg = Trim(IIf(IsDBNull(rstSQL.Fields("fldRemittanceErrorDescription")), "", rstSQL.Fields("fldRemittanceErrorDescription")))
                                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And Convert.ToInt32(rstSQL.Fields("fldBillStatusID")) = 4 Then fldDenTxTypeID = rstSQL.Fields("fldTxTypeID")
                                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And Convert.ToInt32(rstSQL.Fields("fldBillStatusID")) = 21 Then fldNoteTxTypeID = rstSQL.Fields("fldTxTypeID")
                                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And IsDBNull(ErrorMessage) And ErrMsg > "" Then
                                                ErrorMessage = ErrMsg
                                            ElseIf IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And ErrMsg > "" Then
                                                ErrorMessage = ErrorMessage + "; " + ErrMsg
                                            End If
                                        End If
                                        rstSQL = Nothing
                                    End If
                                    If MessageCode3 > "" Then
                                        ErrMsg = ""
                                        strSQL = "SELECT * FROM tblRemittanceErrorCodes WHERE (fldRemittanceErrorCode ='" & MessageCode3 & "');"
                                        rstSQL = CreateObject("ADODB.Recordset")
                                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                        If Not rstSQL.EOF Then
                                            ErrMsg = Trim(IIf(IsDBNull(rstSQL.Fields("fldRemittanceErrorDescription")), "", rstSQL.Fields("fldRemittanceErrorDescription")))
                                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And Convert.ToInt32(rstSQL.Fields("fldBillStatusID")) = 4 Then fldDenTxTypeID = rstSQL.Fields("fldTxTypeID")
                                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And Convert.ToInt32(rstSQL.Fields("fldBillStatusID")) = 21 Then fldNoteTxTypeID = rstSQL.Fields("fldTxTypeID")
                                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And IsDBNull(ErrorMessage) And ErrMsg > "" Then
                                                ErrorMessage = ErrMsg
                                            ElseIf IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And ErrMsg > "" Then
                                                ErrorMessage = ErrorMessage + "; " + ErrMsg
                                            End If
                                        End If
                                        rstSQL = Nothing
                                    End If
                                Case "REF"
                                    If (rst.Fields("F2") = "ZZ") Then MessageCode1 = rst.Fields("F3")
                                    If (rst.Fields("F2") = "CE") Then
                                        SupplementalInsurerName1 = Replace(IIf(IsDBNull(rst.Fields("F3")), "", Left(Trim(rst.Fields("F3")), 50)), "'", "")
                                        SupplementalInsurerID1 = Replace(IIf(IsDBNull(rst.Fields("F4")), "", Trim(rst.Fields("F4"))), "'", "")
                                    End If
                                    If (rst.Fields("F2") = "G2" Or rst.Fields("F2") = "PQ") Then 'Provider Identification Number
                                        ProviderID = rst.Fields("F3")
                                        strSQL = "SELECT fldProviderID FROM tblProviderPracticeNumber WHERE (fldPracticeNumber = '" & rst.Fields("F3") & "' AND fldInsuranceID =" & HealthPlan & ")"
                                        rstSQL = CreateObject("ADODB.Recordset")
                                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                        fldProviderID = IIf(rstSQL.EOF, fldProviderID, rstSQL.Fields("fldProviderID"))
                                        rstSQL = Nothing
                                        If fldProviderID > 0 And fldGroupID <= 0 Then
                                            strSQL = "SELECT fldGroupID FROM tblProvider WHERE (fldProviderID = " & fldProviderID & " AND fldDisabledYN = 'N')"
                                            rstSQL = CreateObject("ADODB.Recordset")
                                            rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                            fldGroupID = IIf(rstSQL.EOF, fldGroupID, rstSQL.Fields("fldGroupID"))
                                            rstSQL = Nothing
                                        End If
                                    End If
                                Case "CAS"
                                    If Not FoundCAS Then
                                        ProcedureCode = "" : LineNumber = 0 : ServiceLineNumber = 0 : Units = 0 : Disallowed = 0 : OtherDisallowed = 0 : Deductible = 0 : Coinsurance = 0 : Interest = 0 : Balance = 0 : PatBalance = 0
                                    End If
                                    FoundCAS = True
                                    Segment_Cas(rst, PayorID)
                                Case "PER"
                                Case "PLB"
                                    PLBHeader = "" : PLBMessage = ""
                                    Do While (RecID = "PLB") And (Not rst.EOF)
                                        PLBHeader = "" : PLBMessage = ""
                                        For I = 1 To rst.Fields.Count - 1 Step 1
                                            PLBMessage = PLBMessage & rst.Fields("F" & I) & " "
                                            If Trim(PLBMessage) > "" And I = 3 Then
                                                PLBHeader = PLBMessage
                                                PLBMessage = ""
                                            End If
                                            If (I = 4 Or I = 6 Or I = 8 Or I = 10) Then
                                                Select Case Left(rst.Fields("F" & I), 2)
                                                    Case "L6"
                                                        PLBMessage = PLBMessage & "Interest: "
                                                    Case "WO"
                                                        PLBMessage = PLBMessage & "Overpayment recovery: "
                                                    Case "LE"
                                                        PLBMessage = PLBMessage & "IRS Withholding: "
                                                    Case "CS"
                                                        PLBMessage = PLBMessage & "Adjustment: "
                                                    Case "FB"
                                                        PLBMessage = PLBMessage & "Balance Forward: "
                                                    Case "72"
                                                        PLBMessage = PLBMessage & "Refund Received: "
                                                End Select
                                            End If
                                        Next I



                                        If Trim(PLBHeader) > "" And FoundCheck = False Then
                                            objRemit = CreateObject("EDIBz.CRemitBz")
                                            RecordID = objRemit.Insert(Convert.ToString(Left(PayorID, 10)), Convert.ToString(HealthPlan), Mid(HealthPlanName, 1, 33), _
                                               IIf(IsDate(CreationDate), CDate(CreationDate), 0), Convert.ToString(BatchNumber), Trim(Mid(Provider, 1, 75)), Mid(Replace(ProviderID, " ", ""), 1, 15), _
                                               Left(Convert.ToString(CheckNumber), 30), IIf(IsDate(CheckDate), CDate(CheckDate), 0), Convert.ToDouble(CheckAmount), 0, _
                                               0, IIf(IsDate(RemittanceDate), CDate(RemittanceDate), 0), IIf(IsDate(ProcessDate), CDate(ProcessDate), 0), "0", _
                                               "", "", "", "", "", "", "", "", "", "", "", , False, "", "", "", "", "", "", "", "", "", "", _
                                               CLng(HealthPlan), CLng(fldProviderID), 0, 0, CLng(PayerCheckID), _
                                               Trim(Left(ernFile, 50)), False, 0, IIf(PLBHeader > "", PLBHeader, ""), "system")
                                            objRemit = Nothing
                                            objCheckLog = CreateObject("PostingBz.CCheckLogBZ")
                                            PayerCheckLogID = objCheckLog.Insert(PayerCheckID, HealthPlan, fldProviderID, 0, 0, 0, 0, False, Format(Now(), "Short Date"), 0, 0, 0, _
                                                                 0, 0, 0, 0, 0, 0, 0, 0, 0, Convert.ToString(Left(PLBHeader & " " & PLBMessage, 512)), 0, False, 0, "system")
                                            objCheckLog = Nothing
                                            objRemitDetail = CreateObject("EDIBz.CRemitDetailBz")
                                            RecordDetailID = objRemitDetail.Insert(RecordID, Convert.ToString(Left(PayorID, 10)), 0, _
                                                        Convert.ToString(0), Convert.ToString(0), 0, 0, 0, "", "", "", "", "", "", "", 0, 0, 0, 0, 0, 0, _
                                                        0, 0, Mid(Replace(Convert.ToString(ProviderID), " ", ""), 1, 15), _
                                                        "", "", "", "", "", "", "", False, 0, 0, 0, CLng(HealthPlan), CLng(fldProviderID), _
                                                        CLng(PayerCheckID), CLng(PayerCheckLogID), Convert.ToString(Trim(Left(ernFile, 50))), Convert.ToString(Left(PLBMessage, 255)), "system")
                                            objRemitDetail = Nothing
                                        End If
                                        rst.MoveNext()
                                        RecID = rst.Fields.Item("F1")
                                    Loop
                                    RecID = "PLB"
                                Case "TE"
                            End Select
                            'rst.MoveNext
                            If RecID <> "PLB" Then rst.MoveNext()
                            RecID = rst.Fields.Item("F1")
                            If RecID = "CLP" Or RecID = "SE" Then
                                If FoundCAS = True Or FoundCLP = True Then
                                    If fldEncounterLogID = 0 Then
                                        fldEncounterLogID = FindEncounter(CLng(Left(PatientID, 9)), fldPatientID, fldProviderID, fldGroupID, fldOwnerID, FirstDateOfService, ProcedureCode, PatFirstName, PatLastName)
                                        If fldEncounterLogID > 0 And (fldPatientID = 0 Or fldProviderID = 0) Then
                                            strSQL = "SELECT A.fldPatientID, A.fldEncounterLogID, A.fldProviderID FROM tblEncounterLog AS A WHERE (A.fldEncounterLogID =" & fldEncounterLogID & " AND A.fldDOS = CONVERT(DATETIME, '" & Format(FirstDateOfService, "yyyy-mm-dd") & " 00:00:00', 102)" & ")"
                                            rstSQL = CreateObject("ADODB.Recordset")
                                            rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                            fldPatientID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldPatientID"))
                                            fldProviderID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldProviderID"))
                                            rstSQL = Nothing
                                        End If
                                    End If
                                    fldPlanID = 0 : fldOrder = 0 : fldPrintHcfaYN = "N"
                                    PolicyID = IIf(Convert.ToString(PatientPolicyID) > "", Convert.ToString(PatientPolicyID), IIf(Convert.ToString(InsuredPolicyID) > "", Convert.ToString(InsuredPolicyID), ""))
                                    If PayerCheckID > 0 Then
                                        If fldEncounterLogID > 0 Then
                                            fldPmtTxTypeID = 0 : fldDisTxTypeID = 0 : fldDedTxTypeID = 0 : fldCoInsTxTypeID = 0 : fldRefTxTypeID = 0 : fldNoteTxTypeID = 0

                                            rstSQL = Nothing
                                            rstSQL = FindPlanID(CLng(fldPatientID), CLng(fldEncounterLogID), CLng(HealthPlan), lngCPTRecordID, FirstDateOfService, PolicyID, PatFirstName, PatLastName)
                                            If Not rstSQL.EOF Then
                                                fldPlanID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldPlanID"))
                                                fldOrder = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldOrder"))
                                                HealthPlan = IIf(rstSQL.Fields("fldInsuranceID") <> HealthPlan, rstSQL.Fields("fldInsuranceID"), HealthPlan)
                                                PatBalance = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldRPBal"))
                                                Balance = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldPlanBal"))
                                            End If
                                            If fldEncounterLogID > 0 And lngCPTRecordID > 0 And IsDate(FirstDateOfService) Then
                                                strSQL = "SELECT fldEncDetailID FROM tblEncDetail WHERE (fldEncounterLogID =" & fldEncounterLogID & " AND fldDOS = CONVERT(DATETIME, '" & Format(FirstDateOfService, "yyyy-mm-dd") & " 00:00:00', 102) AND (fldCPTRecordID = " & lngCPTRecordID & " OR fldAddOnCPTRecordID = " & lngCPTRecordID & " OR fldAddOnSecCPTRecordID = " & lngCPTRecordID & "))"
                                                rstSQL = CreateObject("ADODB.Recordset")
                                                rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                                fldEncDetailID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldEncDetailID"))
                                                rstSQL = Nothing
                                            End If
                                        Else
                                            rstSQL = Nothing
                                            rstSQL = FindPlanID(CLng(fldPatientID), CLng(fldEncounterLogID), CLng(HealthPlan), lngCPTRecordID, FirstDateOfService, PolicyID, PatFirstName, PatLastName)
                                            If Not rstSQL.EOF Then
                                                fldEncounterLogID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldEncounterLogID"))
                                                fldPatientID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldPatientID"))
                                                If fldEncounterLogID > 0 Then
                                                    fldPlanID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldPlanID"))
                                                    fldOrder = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldOrder"))
                                                End If
                                                HealthPlan = IIf(rstSQL.Fields("fldInsuranceID") <> HealthPlan, rstSQL.Fields("fldInsuranceID"), HealthPlan)
                                                rstSQL = Nothing
                                            End If
                                        End If
                                        '                 End If
                                        If Payment <> 0 Then
                                            If fldOrder = 1 Then
                                                fldPmtTxTypeID = 120          'Primary insurance Payment
                                                strSQL = "SELECT fldPlanID FROM tblBillingResponsibility WHERE (fldEncounterLogID =" & fldEncounterLogID & " AND fldInsuranceID = " & HealthPlan & " AND fldOrder = 3 AND fldPayerCode = 'I')"
                                                rstSQL = CreateObject("ADODB.Recordset")
                                                rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                                fldPrintHcfaYN = IIf(rstSQL.EOF, "N", "Y")
                                                rstSQL = Nothing
                                            ElseIf fldOrder = 2 Then
                                                fldPmtTxTypeID = 105          'Secondary insurance Payment
                                                If OtherDisallowed <> 0 Or Reason1 = "OA23" Or Reason2 = "OA23" Or Reason3 = "OA23" Then
                                                    If Convert.ToDouble(Charge) - Convert.ToDouble(OtherDisallowed) = 0 Then Payment = AmountPaid ' Other Disallowance
                                                End If
                                                If fldPlanID <> 4420 And fldPlanID <> 4607 And fldPlanID <> 4857 And HealthPlan <> 24 And HealthPlan <> 9 Then Disallowed = 0 'Diallowance
                                            ElseIf fldOrder = 3 Then
                                                fldPmtTxTypeID = 106          'Terciary insurance Payment
                                                If OtherDisallowed <> 0 Or Reason1 = "OA23" Or Reason2 = "OA23" Or Reason3 = "OA23" Then
                                                    If Convert.ToDouble(Charge) - Convert.ToDouble(OtherDisallowed) = 0 Then Payment = AmountPaid ' Other Disallowance
                                                End If
                                                If fldPlanID <> 4420 And fldPlanID <> 4607 And fldPlanID <> 4857 And HealthPlan <> 24 And HealthPlan <> 9 Then Disallowed = 0 'Diallowance
                                            Else
                                                fldPmtTxTypeID = 120
                                                If OtherDisallowed <> 0 Or Reason1 = "OA23" Or Reason2 = "OA23" Or Reason3 = "OA23" Then Disallowed = 0 ' Other Disallowance
                                            End If
                                        Else 'payment is = 0
                                            If OtherDisallowed <> 0 Or Reason1 = "OA23" Or Reason2 = "OA23" Or Reason3 = "OA23" Then
                                                If HealthPlan = 105 And fldOrder > 1 Then
                                                    Disallowed = (PatBalance + Balance) - AmountPaid
                                                    fldDenTxTypeID = 0
                                                Else
                                                    Disallowed = 0
                                                End If
                                            ElseIf Deductible <> 0 Then
                                                If HealthPlan <> 105 And fldOrder > 1 Then
                                                    If Disallowed > (PatBalance + Balance) Then Disallowed = 0
                                                End If
                                            End If
                                        End If
                                        If fldDenTxTypeID = 313 Then                        'Duplicate Claim
                                            Disallowed = 0 : fldDisTxTypeID = 0 : Deductible = 0 : fldDedTxTypeID = 0
                                        End If
                                        If (fldOrder = 1) And (Disallowed = 0) And (fldDenTxTypeID <> 313) And (Allowed > 0) Then Disallowed = Charge - Allowed
                                        If Disallowed <> 0 Then fldDisTxTypeID = 7 'Diallowance
                                        If Deductible <> 0 Then fldDedTxTypeID = 87 'Patient Responsibility
                                        If Coinsurance <> 0 Then fldCoInsTxTypeID = 180 'CoInsurance
                                        If Payment <> 0 Then fldDenTxTypeID = 0 'Remove Denial if there is a Payment
                                        If Payment <> 0 Then
                                            strSQL = "SELECT fldPayerCheckLogID FROM tblPayerCheckLog WHERE (fldCheckID =" & PayerCheckID & " AND fldEncounterLogID = " & fldEncounterLogID & " AND fldInsuranceID = " & HealthPlan & " AND fldPayment =" & Payment & " AND fldAddedBy <> 'system')"
                                        Else
                                            strSQL = "SELECT fldPayerCheckLogID FROM tblPayerCheckLog WHERE (fldCheckID =" & PayerCheckID & " AND fldEncounterLogID = " & fldEncounterLogID & " AND fldInsuranceID = " & HealthPlan & " AND fldPayment =" & Payment & " AND fldDisallowance =" & Disallowed & " AND fldDeductable =" & Deductible & " AND fldAddedBy <> 'system')"
                                        End If
                                        rstSQL = CreateObject("ADODB.Recordset")
                                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                        PayerCheckLogID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldPayerCheckLogID"))
                                        If (FoundCheck = False And (PayerCheckLogID = 0 Or fldEncounterLogID = 0)) Or (FoundCheck = True And (PayerCheckLogID = 0 And fldEncounterLogID > 0)) Then
                                            'If FoundCheck = False And (PayerCheckLogID = 0 Or fldEncounterLogID = 0) Then
                                            rstSQL = Nothing
                                            objCheckLog = CreateObject("PostingBz.CCheckLogBZ")
                                            PayerCheckLogID = objCheckLog.Insert(PayerCheckID, HealthPlan, _
                                                                 fldProviderID, fldPatientID, fldEncounterLogID, fldEncDetailID, lngCPTRecordID, False, _
                                                                 Format(Now(), "Short Date"), Payment, fldPmtTxTypeID, Disallowed, _
                                                                 fldDisTxTypeID, Deductible, fldDedTxTypeID, Coinsurance, fldCoInsTxTypeID, 0, _
                                                                 fldRefTxTypeID, 0, fldDenTxTypeID, _
                                                                 Convert.ToString(Left(ErrorMessage, 512)), fldNoteTxTypeID, IIf(fldPrintHcfaYN = "Y", True, False), fldPlanID, _
                                                                 "system")
                                            objCheckLog = Nothing

                                            If PayerCheckID > 0 And PayerCheckLogID > 0 Then
                                                rstSQL = Nothing
                                                strSQL = "SELECT RemittanceRecordID, RemittanceDetailRecordID FROM tblRemittanceDetail WHERE (fldCheckID =" & PayerCheckID & " AND fldEncounterLogID = " & fldEncounterLogID & " AND fldInsuranceID = " & HealthPlan & " AND fldPayerCheckLogID =" & PayerCheckLogID & " )"
                                                rstSQL = CreateObject("ADODB.Recordset")
                                                rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                                RecordDetailID = IIf(rstSQL.EOF, 0, rstSQL.Fields("RemittanceDetailRecordID"))
                                                RecordID = IIf(rstSQL.EOF, 0, rstSQL.Fields("RemittanceRecordID"))
                                                rstSQL = Nothing
                                            End If

                                            If RecordID = 0 Then
                                                objRemit = CreateObject("EDIBz.CRemitBz")
                                                RecordID = objRemit.Insert(Convert.ToString(Left(PayorID, 10)), Convert.ToString(HealthPlan), Mid(HealthPlanName, 1, 33), _
                                                       IIf(IsDate(CreationDate), CDate(CreationDate), 0), Convert.ToString(BatchNumber), Trim(Mid(Provider, 1, 75)), Mid(Replace(ProviderID, " ", ""), 1, 15), _
                                                       Left(Convert.ToString(CheckNumber), 30), IIf(IsDate(CheckDate), CDate(CheckDate), 0), Convert.ToDouble(CheckAmount), Convert.ToDouble(AmountPaid), _
                                                       Convert.ToDouble(Interest), IIf(IsDate(RemittanceDate), CDate(RemittanceDate), 0), IIf(IsDate(ProcessDate), CDate(ProcessDate), 0), Convert.ToString(PatientNumber), _
                                                       Convert.ToString(InsuredGroupNbr), Convert.ToString(InsuredPolicyID), Convert.ToString(Left(InsuredLastName, 20)), Convert.ToString(Left(InsuredFirstName, 20)), _
                                                       Convert.ToString(InsuredMiddleInitial), Convert.ToString(PatientGroupNbr), Convert.ToString(PatientPolicyID), Convert.ToString(Left(PatientLastName, 20)), _
                                                       Convert.ToString(PatientFirstName), Convert.ToString(PatientMiddleInitial), Convert.ToString(PatientSex), IIf(IsDate(PatientDOB), CDate(PatientDOB), 0), _
                                                       False, IIf(MedicalRecordNumber > "", Left(Trim(MedicalRecordNumber), 50), ""), _
                                                       IIf(MessageCode1 > "", MessageCode1, ""), IIf(MessageCode2 > "", MessageCode2, ""), IIf(MessageCode3 > "", MessageCode3, ""), IIf(MessageCode4 > "", MessageCode4, ""), IIf(MessageCode5 > "", MessageCode5, ""), _
                                                       IIf(SupplementalInsurerName1 > "", SupplementalInsurerName1, ""), Left(IIf(SupplementalInsurerID1 > "", SupplementalInsurerID1, ""), 15), "", "", _
                                                       CLng(HealthPlan), CLng(fldProviderID), CLng(fldPatientID), CLng(fldEncounterLogID), CLng(PayerCheckID), _
                                                       Trim(Left(ernFile, 50)), False, 0, IIf(ErrorMessage > "", Left(ErrorMessage, 255), ""), "system")
                                                objRemit = Nothing

                                                objRemitDetail = CreateObject("EDIBz.CRemitDetailBz")
                                                RecordDetailID = objRemitDetail.Insert(RecordID, Convert.ToString(Left(PayorID, 10)), Convert.ToString(Trim(Left(PatientID, 15))), _
                                                       Convert.ToString(LineNumber), Convert.ToString(ServiceLineNumber), IIf(IsDate(FirstDateOfService), CDate(FirstDateOfService), 0), _
                                                       IIf(IsDate(LastDateOfService), CDate(LastDateOfService), 0), IIf(IsDate(ClaimReceiveDate), CDate(ClaimReceiveDate), 0), _
                                                       Convert.ToString(PlaceOfService), Convert.ToString(TypeOfService), Convert.ToString(ProcedureCode), _
                                                       Convert.ToString(Mod1), Convert.ToString(Mod2), Convert.ToString(Mod3), _
                                                       Convert.ToString(Units), Convert.ToDouble(Charge), Convert.ToDouble(Disallowed), Convert.ToDouble(Allowed), _
                                                       Convert.ToDouble(Deductible), Convert.ToDouble(Coinsurance), Convert.ToDouble(PatientResp), _
                                                       Convert.ToDouble(Interest), Convert.ToDouble(Payment), Mid(Replace(Convert.ToString(ProviderID), " ", ""), 1, 15), _
                                                       IIf(Reason1 > "", Left(Convert.ToString(Reason1), 6), ""), IIf(Reason2 > "", Left(Convert.ToString(Reason2), 6), ""), IIf(Reason3 > "", Left(Convert.ToString(Reason3), 6), ""), _
                                                       IIf(Reason4 > "", Left(Convert.ToString(Reason4), 6), ""), IIf(Reason5 > "", Left(Convert.ToString(Reason5), 6), ""), IIf(Reason6 > "", Left(Convert.ToString(Reason6), 6), ""), _
                                                       IIf(Reason7 > "", Left(Convert.ToString(Reason7), 6), ""), False, 0, _
                                                       CLng(fldEncounterLogID), CLng(fldPatientID), CLng(HealthPlan), CLng(fldProviderID), _
                                                       CLng(PayerCheckID), CLng(PayerCheckLogID), Convert.ToString(Trim(Left(ernFile, 50))), Convert.ToString(Left(ErrorMessage, 255)), "system")
                                                objRemitDetail = Nothing
                                            End If
                                        End If
                                        fldDenTxTypeID = 0 : fldNoteTxTypeID = 0
                                    End If
                                    FirstDateOfService = Nothing : LastDateOfService = Nothing : FoundCAS = False : FoundCLP = False
                                    Units = 0 : Charge = 0 : Payment = 0 : Deductible = 0 : Coinsurance = 0 : Interest = 0 : PatientResp = 0 : Disallowed = 0 : OtherDisallowed = 0 : Allowed = 0
                                    ErrorMessage = "" : Reason1 = "" : Reason2 = "" : Reason3 = "" : Reason4 = "" : Reason5 = "" : Reason6 = "" : Reason7 = "" : Reason8 = "" : PlaceOfService = ""
                                End If
                            End If
                        Loop
                    ElseIf (RecID = "SVC") Or (RecID = "CAS") Or (RecID = "LX") Or (RecID = "PLB") Or (RecID = "SE") Then
                        Do While (RecID <> "ST") And (RecID <> "GE") 'And (RecID <> "PLB")   'RecID <> "CLP") And
                            Select Case RecID
                                Case "SVC", "CLP", "SE"
                                    If (ProcedureCode = "") Then  'First time finding a SVC
                                        intID = InStr(4, rst.Fields("F2"), ":")
                                        If intID = 0 Then ProcedureCode = Trim(Mid(rst.Fields("F2"), 4, 5))
                                        If intID > 0 Then ProcedureCode = Trim(Mid(rst.Fields("F2"), 4, InStr(4, rst.Fields("F2"), ":") - 4))
                                        prevEncounterLogID = fldEncounterLogID
                                        PrevProcedureCode = ProcedureCode
                                        lngCPTRecordID = 0
                                        strSQL = "SELECT fldCPTRecordID FROM tblCPTCode WHERE (fldCPTCode = '" & ProcedureCode & "')"
                                        rstSQL = CreateObject("ADODB.Recordset")
                                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                        lngCPTRecordID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldCPTRecordID"))
                                        rstSQL = Nothing
                                    Else
                                        'If PrevProcedureCode <> ProcedureCode Then fldEncounterLogID = 0
                                        If fldEncounterLogID = 0 Then
                                            fldEncounterLogID = FindEncounter(CLng(Left(PatientID, 9)), fldPatientID, fldProviderID, fldGroupID, fldOwnerID, FirstDateOfService, ProcedureCode, PatFirstName, PatLastName)
                                            If fldEncounterLogID > 0 And (fldPatientID = 0 Or fldProviderID = 0) Then
                                                strSQL = "SELECT A.fldPatientID, A.fldEncounterLogID, A.fldProviderID FROM tblEncounterLog AS A WHERE (A.fldEncounterLogID =" & fldEncounterLogID & " AND A.fldDOS = CONVERT(DATETIME, '" & Format(FirstDateOfService, "yyyy-mm-dd") & " 00:00:00', 102)" & ")"
                                                rstSQL = CreateObject("ADODB.Recordset")
                                                rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                                fldPatientID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldPatientID"))
                                                fldProviderID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldProviderID"))
                                                rstSQL = Nothing
                                            Else
                                                If FoundCLP = True And fldEncounterLogID = 0 Then fldEncounterLogID = prevEncounterLogID
                                            End If
                                        End If

                                        fldPlanID = 0 : fldOrder = 0 : fldPrintHcfaYN = "N"
                                        PolicyID = IIf(Convert.ToString(PatientPolicyID) > "", Convert.ToString(PatientPolicyID), IIf(Convert.ToString(InsuredPolicyID) > "", Convert.ToString(InsuredPolicyID), ""))
                                        If PayerCheckID > 0 Then
                                            If fldEncounterLogID > 0 Then
                                                fldPmtTxTypeID = 0 : fldDisTxTypeID = 0 : fldDedTxTypeID = 0 : fldCoInsTxTypeID = 0 : fldRefTxTypeID = 0 : fldNoteTxTypeID = 0

                                                rstSQL = Nothing
                                                rstSQL = FindPlanID(CLng(fldPatientID), CLng(fldEncounterLogID), CLng(HealthPlan), lngCPTRecordID, FirstDateOfService, PolicyID, PatFirstName, PatLastName)
                                                If Not rstSQL.EOF Then
                                                    fldPlanID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldPlanID"))
                                                    fldOrder = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldOrder"))
                                                    HealthPlan = IIf(rstSQL.Fields("fldInsuranceID") <> HealthPlan, rstSQL.Fields("fldInsuranceID"), HealthPlan)
                                                    PatBalance = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldRPBal"))
                                                    Balance = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldPlanBal"))
                                                End If
                                                If fldEncounterLogID > 0 And lngCPTRecordID > 0 And IsDate(FirstDateOfService) Then
                                                    strSQL = "SELECT fldEncDetailID FROM tblEncDetail WHERE (fldEncounterLogID =" & fldEncounterLogID & " AND fldDOS = CONVERT(DATETIME, '" & Format(FirstDateOfService, "yyyy-mm-dd") & " 00:00:00', 102) AND (fldCPTRecordID = " & lngCPTRecordID & " OR fldAddOnCPTRecordID = " & lngCPTRecordID & " OR fldAddOnSecCPTRecordID = " & lngCPTRecordID & "))"
                                                    rstSQL = CreateObject("ADODB.Recordset")
                                                    rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                                    fldEncDetailID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldEncDetailID"))
                                                    rstSQL = Nothing
                                                End If
                                            Else
                                                rstSQL = Nothing
                                                rstSQL = FindPlanID(CLng(fldPatientID), CLng(fldEncounterLogID), CLng(HealthPlan), lngCPTRecordID, FirstDateOfService, PolicyID, PatFirstName, PatLastName)
                                                If Not rstSQL.EOF Then
                                                    fldEncounterLogID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldEncounterLogID"))
                                                    fldPatientID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldPatientID"))
                                                    If fldEncounterLogID > 0 Then
                                                        fldPlanID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldPlanID"))
                                                        fldOrder = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldOrder"))
                                                    End If
                                                    HealthPlan = IIf(rstSQL.Fields("fldInsuranceID") <> HealthPlan, rstSQL.Fields("fldInsuranceID"), HealthPlan)
                                                    rstSQL = Nothing
                                                End If

                                            End If
                                            If fldEncounterLogID > 0 And fldPatientID > 0 And IsDate(FirstDateOfService) And CLng(HealthPlan) > 0 And fldPlanID = 0 Then
                                                fldPlanID = 0
                                            End If
                                            If Payment <> 0 Then
                                                If fldOrder = 1 Then
                                                    fldPmtTxTypeID = 120          'Primary insurance Payment
                                                    strSQL = "SELECT fldPlanID FROM tblBillingResponsibility WHERE (fldEncounterLogID =" & fldEncounterLogID & " AND fldOrder = 3 AND fldPayerCode = 'I')"
                                                    rstSQL = CreateObject("ADODB.Recordset")
                                                    rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                                    fldPrintHcfaYN = IIf(Not (rstSQL.EOF) And HealthPlan <> "24", "Y", "N")
                                                    rstSQL = Nothing
                                                    'Check to see if there are Two plans with the same Insurance Company
                                                    '        PolicyID = IIf(Convert.ToString(PatientPolicyID) > "", Convert.ToString(PatientPolicyID), IIf(Convert.ToString(InsuredPolicyID) > "", Convert.ToString(InsuredPolicyID), ""))
                                                    '        If PolicyID > "" Then
                                                    '           strSQL = "SELECT A.fldOrder FROM tblPatRPPlan AS A INNER JOIN tblPatRPPlanRule AS B ON A.fldPatRPPlanID = B.fldPatRPPlanID WHERE (fldPlanID <> 331 AND fldPatientID =" & fldPatientID & " AND fldOrder > 0 AND B.fldCardNum = '" & PolicyID & "')"
                                                    '           Set rstSQL = CreateObject("ADODB.Recordset")
                                                    '           rstSQL.Open strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch
                                                    '           fldOrder = IIf(rstSQL.EOF, 1, rstSQL.Fields("fldOrder"))
                                                    '           Set rstSQL = Nothing
                                                    '           If fldOrder = 2 Then fldPmtTxTypeID = 105          'Secondary insurance Payment
                                                    '           If fldOrder = 3 Then fldPmtTxTypeID = 106          'Terciary insurance Payment
                                                    '        End If
                                                ElseIf fldOrder = 2 Then
                                                    fldPmtTxTypeID = 105          'Secondary insurance Payment
                                                    '  If OtherDisallowed <> 0 Or Reason1 = "OA23" Or Reason2 = "OA23" Or Reason3 = "OA23" Then Payment = Charge - Disallowed - OtherDisallowed - PatientResp       ' Other Disallowance
                                                    If OtherDisallowed <> 0 Or Reason1 = "OA23" Or Reason2 = "OA23" Or Reason3 = "OA23" Then
                                                        If Convert.ToDouble(Charge) - Convert.ToDouble(OtherDisallowed) = 0 Then Payment = AmountPaid ' Other Disallowance
                                                    End If
                                                    If fldPlanID <> 4420 And fldPlanID <> 4607 And fldPlanID <> 4857 And HealthPlan <> 24 And HealthPlan <> 9 Then Disallowed = 0 'Diallowance
                                                ElseIf fldOrder = 3 Then
                                                    fldPmtTxTypeID = 106          'Terciary insurance Payment
                                                    '  If OtherDisallowed <> 0 Or Reason1 = "OA23" Or Reason2 = "OA23" Or Reason3 = "OA23" Then Payment = Charge - Disallowed - OtherDisallowed - PatientResp       ' Other Disallowance
                                                    If OtherDisallowed <> 0 Or Reason1 = "OA23" Or Reason2 = "OA23" Or Reason3 = "OA23" Then
                                                        If Convert.ToDouble(Charge) - Convert.ToDouble(OtherDisallowed) = 0 Then Payment = AmountPaid ' Other Disallowance
                                                    End If
                                                    If fldPlanID <> 4420 And fldPlanID <> 4607 And fldPlanID <> 4857 And HealthPlan <> 24 And HealthPlan <> 9 Then Disallowed = 0 'Diallowance
                                                Else
                                                    fldPmtTxTypeID = 120
                                                    If OtherDisallowed <> 0 Or Reason1 = "OA23" Or Reason2 = "OA23" Or Reason3 = "OA23" Then Disallowed = 0 ' Other Disallowance
                                                End If
                                            Else 'payment is = 0
                                                If OtherDisallowed <> 0 Or Reason1 = "OA23" Or Reason2 = "OA23" Or Reason3 = "OA23" Then
                                                    If HealthPlan = 105 And fldOrder > 1 Then
                                                        Disallowed = (PatBalance + Balance) - AmountPaid
                                                        fldDenTxTypeID = 0
                                                    Else
                                                        Disallowed = 0
                                                    End If
                                                ElseIf Deductible <> 0 Then
                                                    If HealthPlan <> 105 And fldOrder > 1 Then
                                                        If Disallowed > (PatBalance + Balance) Then Disallowed = 0
                                                    End If
                                                End If
                                            End If
                                            If fldDenTxTypeID = 313 Then                        'Duplicate Claim
                                                Disallowed = 0 : fldDisTxTypeID = 0 : Deductible = 0 : fldDedTxTypeID = 0
                                            End If
                                            If Disallowed <> 0 Then fldDisTxTypeID = 7 'Diallowance
                                            If Deductible <> 0 Then fldDedTxTypeID = 87 'Patient Responsibility
                                            If Coinsurance <> 0 Then fldCoInsTxTypeID = 180 'CoInsurance
                                            If Payment > 0 Then fldDenTxTypeID = 0 'Remove Denial if there is a Payment

                                            If Payment <> 0 Then
                                                If fldEncounterLogID > 0 Then
                                                    strSQL = "SELECT fldPayerCheckLogID FROM tblPayerCheckLog WHERE (fldCheckID =" & PayerCheckID & " AND fldEncounterLogID = " & fldEncounterLogID & " AND fldInsuranceID = " & HealthPlan & " AND fldPayment =" & Payment & " )"   'AND fldAddedBy <> 'system'
                                                Else
                                                    strSQL = "SELECT fldPayerCheckLogID FROM tblPayerCheckLog WHERE (fldCheckID =" & PayerCheckID & " AND fldEncounterLogID = " & fldEncounterLogID & " AND fldPatientID = " & fldPatientID & " AND fldInsuranceID = " & HealthPlan & " AND fldPayment =" & Payment & " AND fldAddedBy = 'system')"   '
                                                End If
                                            Else
                                                strSQL = "SELECT fldPayerCheckLogID FROM tblPayerCheckLog WHERE (fldCheckID =" & PayerCheckID & " AND fldEncounterLogID = " & fldEncounterLogID & " AND fldInsuranceID = " & HealthPlan & " AND fldPayment =" & Payment & " AND fldDisallowance =" & Disallowed & " AND fldDeductable =" & Deductible & " AND fldAddedBy = 'system')"
                                            End If
                                            rstSQL = CreateObject("ADODB.Recordset")
                                            rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                            PayerCheckLogID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldPayerCheckLogID"))
                                            If (FoundCheck = False And (PayerCheckLogID = 0 Or fldEncounterLogID = 0)) Or (FoundCheck = False And (PayerCheckLogID > 0 And fldEncounterLogID > 0)) Or (FoundCheck = True And (PayerCheckLogID = 0 And fldEncounterLogID > 0)) Then
                                                rstSQL = Nothing
                                                objCheckLog = CreateObject("PostingBz.CCheckLogBZ")
                                                PayerCheckLogID = objCheckLog.Insert(PayerCheckID, HealthPlan, _
                                                                  fldProviderID, fldPatientID, fldEncounterLogID, fldEncDetailID, lngCPTRecordID, False, _
                                                                  Format(Now(), "Short Date"), Payment, fldPmtTxTypeID, Disallowed, _
                                                                  fldDisTxTypeID, Deductible, fldDedTxTypeID, Coinsurance, fldCoInsTxTypeID, 0, _
                                                                  fldRefTxTypeID, 0, fldDenTxTypeID, _
                                                                  Convert.ToString(Left(ErrorMessage, 512)), fldNoteTxTypeID, IIf(fldPrintHcfaYN = "Y", True, False), fldPlanID, _
                                                                  "system")
                                                objCheckLog = Nothing
                                                If (fldOrder = 1) And (Disallowed = 0) And (fldDenTxTypeID <> 313) And (Allowed > 0) Then Disallowed = Charge - Allowed
                                                fldDenTxTypeID = 0
                                                If (Disallowed = 0 And Allowed > 0) Then Disallowed = Charge - Allowed

                                                RecordID = 0
                                                If PayerCheckID > 0 And PayerCheckLogID > 0 Then
                                                    rstSQL = Nothing
                                                    strSQL = "SELECT RemittanceRecordID, RemittanceDetailRecordID FROM tblRemittanceDetail WHERE (fldCheckID =" & PayerCheckID & " AND fldEncounterLogID = " & fldEncounterLogID & " AND fldInsuranceID = " & HealthPlan & " AND fldPayerCheckLogID =" & PayerCheckLogID & " )"
                                                    rstSQL = CreateObject("ADODB.Recordset")
                                                    rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                                    RecordDetailID = IIf(rstSQL.EOF, 0, rstSQL.Fields("RemittanceDetailRecordID"))
                                                    RecordID = IIf(rstSQL.EOF, 0, rstSQL.Fields("RemittanceRecordID"))
                                                    rstSQL = Nothing
                                                End If
                                                If RecordID = 0 Then
                                                    objRemit = CreateObject("EDIBz.CRemitBz")
                                                    RecordID = objRemit.Insert(Convert.ToString(Left(PayorID, 10)), Convert.ToString(HealthPlan), Mid(HealthPlanName, 1, 33), _
                                                           IIf(IsDate(CreationDate), CDate(CreationDate), 0), Convert.ToString(BatchNumber), Trim(Mid(Provider, 1, 75)), Mid(Replace(ProviderID, " ", ""), 1, 15), _
                                                           Left(Convert.ToString(CheckNumber), 30), IIf(IsDate(CheckDate), CDate(CheckDate), 0), Convert.ToDouble(CheckAmount), Convert.ToDouble(AmountPaid), _
                                                           Convert.ToDouble(Interest), IIf(IsDate(RemittanceDate), CDate(RemittanceDate), 0), IIf(IsDate(ProcessDate), CDate(ProcessDate), 0), Convert.ToString(PatientNumber), _
                                                           Convert.ToString(InsuredGroupNbr), Convert.ToString(InsuredPolicyID), Convert.ToString(Left(InsuredLastName, 20)), Convert.ToString(Left(InsuredFirstName, 20)), _
                                                           Convert.ToString(InsuredMiddleInitial), Convert.ToString(PatientGroupNbr), Convert.ToString(PatientPolicyID), Convert.ToString(Left(PatientLastName, 20)), _
                                                           Convert.ToString(PatientFirstName), Convert.ToString(PatientMiddleInitial), Convert.ToString(PatientSex), IIf(IsDate(PatientDOB), CDate(PatientDOB), 0), _
                                                           False, IIf(MedicalRecordNumber > "", Left(Trim(MedicalRecordNumber), 50), ""), _
                                                           IIf(MessageCode1 > "", MessageCode1, ""), IIf(MessageCode2 > "", MessageCode2, ""), IIf(MessageCode3 > "", MessageCode3, ""), IIf(MessageCode4 > "", MessageCode4, ""), IIf(MessageCode5 > "", MessageCode5, ""), _
                                                           IIf(SupplementalInsurerName1 > "", SupplementalInsurerName1, ""), Left(IIf(SupplementalInsurerID1 > "", SupplementalInsurerID1, ""), 15), "", "", _
                                                           CLng(HealthPlan), CLng(fldProviderID), CLng(fldPatientID), CLng(fldEncounterLogID), CLng(PayerCheckID), _
                                                           Trim(Left(ernFile, 50)), False, 0, IIf(ErrorMessage > "", Left(ErrorMessage, 255), ""), "system")
                                                    objRemit = Nothing

                                                    objRemitDetail = CreateObject("EDIBz.CRemitDetailBz")
                                                    RecordDetailID = objRemitDetail.Insert(RecordID, Convert.ToString(Left(PayorID, 10)), Convert.ToString(Trim(Left(PatientID, 15))), _
                                                          Convert.ToString(LineNumber), Convert.ToString(ServiceLineNumber), IIf(IsDate(FirstDateOfService), CDate(FirstDateOfService), 0), _
                                                          IIf(IsDate(LastDateOfService), CDate(LastDateOfService), 0), IIf(IsDate(ClaimReceiveDate), CDate(ClaimReceiveDate), 0), _
                                                          Convert.ToString(PlaceOfService), Convert.ToString(TypeOfService), Convert.ToString(ProcedureCode), _
                                                          Convert.ToString(Mod1), Convert.ToString(Mod2), Convert.ToString(Mod3), _
                                                          Convert.ToString(Units), Convert.ToDouble(Charge), Convert.ToDouble(Disallowed), Convert.ToDouble(Allowed), _
                                                          Convert.ToDouble(Deductible), Convert.ToDouble(Coinsurance), Convert.ToDouble(PatientResp), _
                                                          Convert.ToDouble(Interest), Convert.ToDouble(Payment), Mid(Replace(Convert.ToString(ProviderID), " ", ""), 1, 15), _
                                                          IIf(Reason1 > "", Left(Convert.ToString(Reason1), 6), ""), IIf(Reason2 > "", Left(Convert.ToString(Reason2), 6), ""), IIf(Reason3 > "", Left(Convert.ToString(Reason3), 6), ""), _
                                                          IIf(Reason4 > "", Left(Convert.ToString(Reason4), 6), ""), IIf(Reason5 > "", Left(Convert.ToString(Reason5), 6), ""), IIf(Reason6 > "", Left(Convert.ToString(Reason6), 6), ""), _
                                                          IIf(Reason7 > "", Left(Convert.ToString(Reason7), 6), ""), False, 0, _
                                                          CLng(fldEncounterLogID), CLng(fldPatientID), CLng(HealthPlan), CLng(fldProviderID), _
                                                          CLng(PayerCheckID), CLng(PayerCheckLogID), Convert.ToString(Trim(Left(ernFile, 50))), Convert.ToString(Left(ErrorMessage, 255)), "system")
                                                    objRemitDetail = Nothing
                                                End If
                                            End If
                                        End If

                                        If RecID = "SE" And PayerCheckID > 0 Then
                                            rstSQL = Nothing
                                            strSQL = "SELECT fldSubmissionOnlyYN FROM tblProvider WHERE (fldProviderID = " & fldProviderID & ")"
                                            rstSQL = CreateObject("ADODB.Recordset")
                                            rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                            If IIf(rstSQL.EOF, "N", rstSQL.Fields("fldSubmissionOnlyYN")) = "Y" Then
                                                strData = "Electronic EOB - Provider Responsible"
                                            Else
                                                strData = "Electronic EOB"
                                            End If
                                            rstSQL = Nothing
                                            objCheck = CreateObject("PostingBz.CCheckBz")
                                            If fldGroupID <= 0 Or fldGroupID = 680 Or IsDBNull(fldGroupID) Then
                                                fldGroupID = 680
                                                objCheck.Update(PayerCheckID, HealthPlan, CheckNumber, fldDepositNum, CDate(CheckDate), IIf(IsDate(ReceiveDate), CDate(ReceiveDate), Now()), Convert.ToDecimal(CheckAmount), fldProviderID, fldGroupID, IIf(Posted = "Y", True, False), strData, True)
                                            Else
                                                objCheck.Update(PayerCheckID, HealthPlan, CheckNumber, fldDepositNum, CDate(CheckDate), IIf(IsDate(ReceiveDate), CDate(ReceiveDate), Now()), Convert.ToDecimal(CheckAmount), -1, fldGroupID, IIf(Posted = "Y", True, False), strData, True)
                                            End If
                                            objCheck = Nothing
                                        End If
                                        'FirstDateOfService = Nothing: LastDateOfService = Nothing
                                        FoundCAS = False : RecordDetailID = 0
                                        Units = 0 : Charge = 0 : Payment = 0 : Deductible = 0 : Coinsurance = 0 : Interest = 0 : PatientResp = 0 : Disallowed = 0 : Allowed = 0 : OtherDisallowed = 0
                                        ErrorMessage = "" : Reason1 = "" : Reason2 = "" : Reason3 = "" : Reason4 = "" : Reason5 = "" : Reason6 = "" : Reason7 = "" : Reason8 = ""
                                        fldEncounterLogID = 0 : fldEncDetailID = 0     'Removed 02/21/2019
                                    End If
                                    If RecID <> "SE" And RecID <> "CLP" Then
                                        intID = InStr(4, rst.Fields("F2"), ":")
                                        If intID = 0 Then ProcedureCode = Trim(Mid(rst.Fields("F2"), 4, 5))
                                        If intID > 0 Then ProcedureCode = Trim(Mid(rst.Fields("F2"), 4, InStr(4, rst.Fields("F2"), ":") - 4))
                                        If Len(rst.Fields("F2")) > 9 Then Mod1 = Trim(Mid(rst.Fields("F2"), 10, 2))
                                        If Len(rst.Fields("F2")) > 12 Then Mod2 = Trim(Mid(rst.Fields("F2"), 13, 2))
                                        LineNumber = 0
                                        ServiceLineNumber = 0
                                        Units = IIf(IsNumeric(rst.Fields("F6")), rst.Fields("F6"), 0)
                                        If IsNumeric(rst.Fields("F3")) Then
                                            Charge = Convert.ToDouble(rst.Fields("F3"))
                                        Else
                                            ProcedureCode = rst.Fields("F3")
                                            Charge = 0
                                        End If
                                        If AmountPaid = 0 Then
                                            Deductible = Convert.ToDouble(rst.Fields("F4"))
                                            If FoundCAS = False Then Payment = 0
                                        Else
                                            Payment = Convert.ToDouble(rst.Fields("F4"))
                                        End If
                                        If (Payment = 0 And Deductible = 0 And Allowed = 0) Then Disallowed = 0
                                        lngCPTRecordID = 0
                                        strSQL = "SELECT fldCPTRecordID FROM tblCPTCode WHERE (fldCPTCode = '" & ProcedureCode & "')"
                                        rstSQL = CreateObject("ADODB.Recordset")
                                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                        lngCPTRecordID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldCPTRecordID"))
                                        rstSQL = Nothing
                                        If IsNumeric(rst.Fields("F9")) Then
                                            PlaceOfService = Right(Trim(rst.Fields("F9")), 2)
                                        End If
                                        If fldEncounterLogID = 0 Then
                                            fldEncounterLogID = prevEncounterLogID
                                        End If
                                    End If
                                Case "DTM"
                                    If (rst.Fields("F2") = "150") Or (rst.Fields("F2") = "232") Or (rst.Fields("F2") = "472") Then
                                        If Len(rst.Fields("F3")) = 6 Then
                                            FirstDateOfService = DateSerial(Convert.ToInt32(Mid(rst.Fields("F3"), 1, 2)) + 2000, Convert.ToInt32(Mid(rst.Fields("F3"), 3, 2)), Convert.ToInt32(Mid(rst.Fields("F3"), 5, 2)))
                                            LastDateOfService = DateSerial(Convert.ToInt32(Mid(rst.Fields("F3"), 1, 2)) + 2000, Convert.ToInt32(Mid(rst.Fields("F3"), 3, 2)), Convert.ToInt32(Mid(rst.Fields("F3"), 5, 2)))
                                        Else
                                            FirstDateOfService = DateSerial(Convert.ToInt32(Mid(rst.Fields("F3"), 1, 4)), Convert.ToInt32(Mid(rst.Fields("F3"), 5, 2)), Convert.ToInt32(Mid(rst.Fields("F3"), 7, 2)))
                                            LastDateOfService = DateSerial(Convert.ToInt32(Mid(rst.Fields("F3"), 1, 4)), Convert.ToInt32(Mid(rst.Fields("F3"), 5, 2)), Convert.ToInt32(Mid(rst.Fields("F3"), 7, 2)))
                                        End If
                                    End If
                                    If (rst.Fields("F2") = "151") Or (rst.Fields("F2") = "233") Or (rst.Fields("F2") = "473") Then
                                        If Len(rst.Fields("F3")) = 6 Then
                                            LastDateOfService = DateSerial(Convert.ToInt32(Mid(rst.Fields("F3"), 1, 2)) + 2000, Convert.ToInt32(Mid(rst.Fields("F3"), 3, 2)), Convert.ToInt32(Mid(rst.Fields("F3"), 5, 2)))
                                        Else
                                            LastDateOfService = DateSerial(Convert.ToInt32(Mid(rst.Fields("F3"), 1, 4)), Convert.ToInt32(Mid(rst.Fields("F3"), 5, 2)), Convert.ToInt32(Mid(rst.Fields("F3"), 7, 2)))
                                        End If
                                    End If
                                Case "CAS", "LQ"
                                    FoundCAS = False
                                    Segment_Cas(rst, PayorID)
                                Case "AMT"
                                    If rst.Fields("F2") = "B6" Then 'Amount Allowed
                                        Allowed = Convert.ToDouble(rst.Fields("F3"))
                                    End If
                                    If rst.Fields("F2") = "I" Then 'Interest Amount
                                        Interest = Convert.ToDouble(rst.Fields("F3"))
                                    End If
                                Case "LX"
                                Case "REF"
                                    If (rst.Fields("F2") = "6R") Then TypeOfService = Right(Trim(rst.Fields("F3")), 2)
                                    If (rst.Fields("F2") = "LU") Then PlaceOfService = Right(Trim(rst.Fields("F3")), 2)
                                    If (rst.Fields("F2") = "1C") Then ProviderID = Trim(rst.Fields("F3"))
                                Case "TS3"
                                    If (rst.Fields("F3") = "11") Or (rst.Fields("F2") = "232") Or (rst.Fields("F2") = "472") Then
                                        '      If Len(rst.Fields("F4")) = 6 Then
                                        '          FirstDateOfService = DateSerial(Convert.toint32(Mid(rst.Fields("F4"), 1, 2)) + 2000, Convert.toint32(Mid(rst.Fields("F4"), 3, 2)), Convert.toint32(Mid(rst.Fields("F4"), 5, 2)))
                                        '          LastDateOfService = DateSerial(Convert.toint32(Mid(rst.Fields("F4"), 1, 2)) + 2000, Convert.toint32(Mid(rst.Fields("F4"), 3, 2)), Convert.toint32(Mid(rst.Fields("F4"), 5, 2)))
                                        '      Else
                                        '          FirstDateOfService = DateSerial(Convert.toint32(Mid(rst.Fields("F4"), 1, 4)), Convert.toint32(Mid(rst.Fields("F4"), 5, 2)), Convert.toint32(Mid(rst.Fields("F4"), 7, 2)))
                                        '          LastDateOfService = DateSerial(Convert.toint32(Mid(rst.Fields("F4"), 1, 4)), Convert.toint32(Mid(rst.Fields("F4"), 5, 2)), Convert.toint32(Mid(rst.Fields("F4"), 7, 2)))
                                        '      End If
                                    End If
                                Case "PLB"
                                    PLBHeader = "" : PLBMessage = ""
                                    Do While (RecID = "PLB") And (Not rst.EOF)
                                        PLBHeader = "" : PLBMessage = ""
                                        For I = 1 To rst.Fields.Count - 1 Step 1
                                            PLBMessage = PLBMessage & rst.Fields("F" & I) & " "
                                            If Trim(PLBMessage) > "" And I = 3 Then
                                                PLBHeader = PLBMessage
                                                PLBMessage = ""
                                            End If
                                            If (I = 4 Or I = 6 Or I = 8 Or I = 10) Then
                                                Select Case Left(rst.Fields("F" & I), 2)
                                                    Case "L6"
                                                        PLBMessage = PLBMessage & "Interest: "
                                                    Case "WO"
                                                        PLBMessage = PLBMessage & "Overpayment recovery: "
                                                    Case "LE"
                                                        PLBMessage = PLBMessage & "IRS Withholding: "
                                                    Case "CS"
                                                        PLBMessage = PLBMessage & "Adjustment: "
                                                    Case "FB"
                                                        PLBMessage = PLBMessage & "Balance Forward: "
                                                    Case "72"
                                                        PLBMessage = PLBMessage & "Refund Recveived: "
                                                End Select
                                            End If
                                        Next I
                                        If Trim(PLBHeader) > "" And FoundCheck = False Then
                                            objRemit = CreateObject("EDIBz.CRemitBz")
                                            RecordID = objRemit.Insert(Convert.ToString(Left(PayorID, 10)), Convert.ToString(HealthPlan), Mid(HealthPlanName, 1, 33), _
                                               IIf(IsDate(CreationDate), CDate(CreationDate), 0), Convert.ToString(BatchNumber), Trim(Mid(Provider, 1, 75)), Mid(Replace(ProviderID, " ", ""), 1, 15), _
                                               Left(Convert.ToString(CheckNumber), 30), IIf(IsDate(CheckDate), CDate(CheckDate), 0), Convert.ToDouble(CheckAmount), 0, _
                                               0, IIf(IsDate(RemittanceDate), CDate(RemittanceDate), 0), IIf(IsDate(ProcessDate), CDate(ProcessDate), 0), "0", _
                                               "", "", "", "", "", "", "", "", "", "", "", 0, False, "", "", "", "", "", "", "", "", "", "", _
                                               CLng(HealthPlan), CLng(fldProviderID), 0, 0, CLng(PayerCheckID), _
                                               Trim(Left(ernFile, 50)), False, 0, IIf(PLBHeader > "", PLBHeader, ""), "system")
                                            objRemit = Nothing
                                            objCheckLog = CreateObject("PostingBz.CCheckLogBZ")
                                            PayerCheckLogID = objCheckLog.Insert(PayerCheckID, HealthPlan, fldProviderID, 0, 0, 0, 0, False, Format(Now(), "Short Date"), 0, 0, 0, _
                                                                 0, 0, 0, 0, 0, 0, 0, 0, 0, Convert.ToString(Left(PLBHeader & " " & PLBMessage, 512)), 0, False, 0, "system")
                                            objCheckLog = Nothing
                                            objRemitDetail = CreateObject("EDIBz.CRemitDetailBz")
                                            RecordDetailID = objRemitDetail.Insert(RecordID, Convert.ToString(Left(PayorID, 10)), 0, _
                                                        Convert.ToString(0), Convert.ToString(0), 0, 0, 0, "", "", "", "", "", "", "", 0, 0, 0, 0, 0, 0, _
                                                        0, 0, Mid(Replace(Convert.ToString(ProviderID), " ", ""), 1, 15), _
                                                        "", "", "", "", "", "", "", False, 0, 0, 0, CLng(HealthPlan), CLng(fldProviderID), _
                                                        CLng(PayerCheckID), CLng(PayerCheckLogID), Convert.ToString(Trim(Left(ernFile, 50))), Convert.ToString(Left(PLBMessage, 255)), "system")
                                            objRemitDetail = Nothing
                                        End If
                                        rst.MoveNext()
                                        RecID = rst.Fields.Item("F1")
                                    Loop
                                    RecID = "PLB"


                            End Select
                            If RecID = "CLP" Then Exit Do
                            If RecID <> "PLB" Then rst.MoveNext()
                            RecID = rst.Fields.Item("F1")
                        Loop
                        'Old Logic ------------------->
                        '<----------------

                        FirstDateOfService = Nothing : LastDateOfService = Nothing
                        FoundCAS = False : FoundCLP = False
                        Units = 0 : Charge = 0 : Payment = 0 : Deductible = 0 : Coinsurance = 0 : Interest = 0 : PatientResp = 0 : Disallowed = 0 : Allowed = 0
                        ErrorMessage = "" : Reason1 = "" : Reason2 = "" : Reason3 = "" : Reason4 = "" : Reason5 = "" : Reason6 = "" : Reason7 = "" : Reason8 = ""
                        PLBHeader = "" : PLBMessage = ""
                    End If
                Loop
            End If
            'rst.MoveNext
            If EndProcess = True Then Exit Do
        Loop

        'delete remittance file from ElectronicClaims Folder
        If Dir(inFile) > "" Then
            If Dir(Mid(inFile, 1, InStr(1, inFile, "\835\") + 4) & "Archive\" & Mid(inFile, InStr(1, inFile, "\835\") + 5)) > "" Then
                fs.DeleteFile(Mid(inFile, 1, InStr(1, inFile, "\835\") + 4) & "Archive\" & Mid(inFile, InStr(1, inFile, "\835\") + 5))
            End If
            fs.MoveFile(inFile, Mid(inFile, 1, InStr(1, inFile, "\835\") + 4) & "Archive\" & Mid(inFile, InStr(1, inFile, "\835\") + 5))
        End If


        'Update Error Messages
        'Instantiate and prepare the command object
        cmdSQL = New ADODB.Command

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(CONST_PSYQUEL_CNN)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        With cmdSQL
            .CommandText = "uspUpdRemitErrorMsgs"
            .CommandType = adCmdStoredProc
            .Parameters.Append.CreateParameter("@ernFile", adVarChar, adParamInput, 100, ernFile)
            .Parameters.Append.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
        End With
        cmdSQL.Execute, , adExecuteNoRecords
        'Check the SQLErrorNum parameter before deciding to commit the transaction.
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            GoTo ErrTrap
        End If
        'Close the connection
        cnnSQL.Close()

        'Free all resources
        cmdSQL = Nothing
        cnnSQL = Nothing

        Exit Function
ErrTrap:
        Screen.MousePointer = vbDefault
        ShowError(Err)
    End Function
    Function FindCheckNumber(ByVal CheckNumber As String, ByVal PayorID As String, ByVal HealthPlan As String, ByVal fldClearingHouseID As Integer, ByVal strF5 As String) As String
        Dim strCheckNumber As String

        If IsNumeric(Trim(CheckNumber)) Then
            strCheckNumber = Trim(CheckNumber)
            While Left(strCheckNumber, 1) = "0" And strCheckNumber <> "" And Len(strCheckNumber) > 1
                strCheckNumber = Right(strCheckNumber, Len(strCheckNumber) - 1)
            End While
            If Len(strCheckNumber) < 10 Then
                'If CLng(strCheckNumber) > 0 Then strCheckNumber = CLng(strCheckNumber)
                If CLng(strCheckNumber) = 0 Then strCheckNumber = Format(Now(), "yyyymmdd") & Format(Now(), "ss") & Format(Now(), "nn")
            Else
                If Mid(strCheckNumber, 1, 4) = Year(Now()) And IsDate(Convert.ToInt32(Mid(strCheckNumber, 1, 4)) & "/" & Convert.ToInt32(Mid(strCheckNumber, 5, 2)) & "/" & Convert.ToInt32(Mid(strCheckNumber, 7, 2))) Then
                    strCheckNumber = Mid(strCheckNumber, 9)
                ElseIf Len(strCheckNumber) < 10 Then
                    If CLng(strCheckNumber) = 0 Then
                        strCheckNumber = Format(Now(), "yyyymmdd") & Format(Now(), "ss") & Format(Now(), "nn")
                    End If
                Else
                    'strCheckNumber = ""
                End If
            End If
        Else
            If UCase(Left(Trim(CheckNumber), 10)) = UCase("No Payment") Or UCase(Left(Trim(CheckNumber), 8)) = UCase("No Check") Then
                strCheckNumber = Format(Now(), "yyyymmdd") & Format(Now(), "ss") & Format(Now(), "nn")
            ElseIf UCase(Left(Trim(CheckNumber), 8)) = "NONCHECK" Or UCase(Trim(CheckNumber)) = "NOCHK" Or UCase(Trim(CheckNumber)) = "NO CHK" Then
                strCheckNumber = "NOCHK" & Format(Now(), "yyyymmdd") & Format(Now(), "ss") & Format(Now(), "nn")
                'NONCHECK -V101789656
                If InStr(1, Trim(CheckNumber), "-") > 0 Then
                    If IsNumeric(Trim(Mid(Trim(CheckNumber), InStr(1, Trim(CheckNumber), "-") + 2, 9))) Then
                        strCheckNumber = Trim(Mid(Trim(CheckNumber), InStr(1, Trim(CheckNumber), "-") + 1, 10))   '''"NOCHK" &
                    End If
                End If
            ElseIf UCase(Left(Trim(CheckNumber), 9)) = UCase("No_Check_") And Len(Trim(CheckNumber)) > 12 Then
                strCheckNumber = Mid(Trim(CheckNumber), 10)
            ElseIf UCase(Left(Trim(CheckNumber), 8)) = UCase("No_Check") And Len(Trim(CheckNumber)) > 12 Then
                strCheckNumber = Mid(Trim(CheckNumber), 9)
            ElseIf Len(CheckNumber) > 15 And InStr(1, Trim(CheckNumber), "Pay-Plus") > 0 Then   'Pay-Plus Payment ID - 54055338
                strCheckNumber = Trim(Mid(Trim(CheckNumber), InStr(1, Trim(CheckNumber), "Pay-Plus") + 21))
            ElseIf Len(CheckNumber) > 25 And UCase(Left(Trim(CheckNumber), 7)) = "NOPYMNT" Then   'NOPYMNT2018082015015382898
                strCheckNumber = Mid(Trim(CheckNumber), InStr(1, Trim(CheckNumber), "NOPYMNT") + 15)
            ElseIf Len(CheckNumber) > 15 And InStr(1, Trim(CheckNumber), "00000000") > 0 And IsNumeric(Mid(Trim(CheckNumber), InStr(1, Trim(CheckNumber), "00000000") + 7)) Then
                strCheckNumber = Trim(Mid(Trim(CheckNumber), InStr(1, Trim(CheckNumber), "00000000") + 7))
                If Len(strCheckNumber) > 15 Then
                    strCheckNumber = CLng(Mid(Trim(strCheckNumber), 1, 10)) & CLng(Mid(Trim(strCheckNumber), 11))
                End If
            ElseIf Len(CheckNumber) > 15 And InStr(1, Trim(CheckNumber), "      ") > 0 And IsNumeric(Mid(Trim(CheckNumber), InStr(1, Trim(CheckNumber), "      ") + 6)) Then
                strCheckNumber = Trim(Mid(Trim(CheckNumber), InStr(1, Trim(CheckNumber), "      ") + 6))
                If Len(strCheckNumber) > 15 Then
                    strCheckNumber = CLng(Mid(Trim(strCheckNumber), 1, 10)) & CLng(Mid(Trim(strCheckNumber), 11))
                End If
            ElseIf Len(CheckNumber) > 15 And UCase(Left(Trim(CheckNumber), 3)) = "EOB" Then   'EOB201901253002773BSC1
                strCheckNumber = Mid(Trim(CheckNumber), InStr(1, Trim(CheckNumber), "EOB") + 11)
            ElseIf Len(CheckNumber) > 15 And UCase(Right(Trim(CheckNumber), 5)) = "EPRA0" Then   '20221213108002240EPRA0
                strCheckNumber = Mid(Trim(CheckNumber), 1, Len(Trim(CheckNumber)) - 5)
                If CLng(Mid(strCheckNumber, 1, 4)) = Year(Now()) And IsDate(Convert.ToInt32(Mid(strCheckNumber, 1, 4)) & "/" & Convert.ToInt32(Mid(strCheckNumber, 5, 2)) & "/" & Convert.ToInt32(Mid(strCheckNumber, 7, 2))) Then
                    strCheckNumber = Mid(strCheckNumber, 9) & "-" & Format(Now(), "dd") & Format(Now(), "ss")
                End If
            ElseIf Len(CheckNumber) > 25 And InStr(1, Trim(CheckNumber), "-") > 0 Then
                strCheckNumber = Mid(Trim(CheckNumber), InStr(1, Trim(CheckNumber), "-") + 1)
            Else
                strCheckNumber = Trim(CheckNumber)
            End If
        End If
        If Len(CheckNumber) >= 15 And (Len(strCheckNumber) = 0 Or Len(strCheckNumber) >= 15) Then
            If HealthPlan = 57 Or HealthPlan = 321 Or HealthPlan = 72 Or HealthPlan = 1309 Then '20190308105017880EZPS030919072552
                If InStr(1, Trim(CheckNumber), "EZ") > 0 Then
                    If Len(strCheckNumber) > 25 Then
                        strCheckNumber = Trim(Mid(Trim(CheckNumber), InStr(1, Trim(CheckNumber), "EZ") + 5))
                    Else
                        strCheckNumber = Trim(Mid(CheckNumber, 9, 12))
                    End If
                ElseIf InStr(1, Trim(CheckNumber), " ") > 0 Then
                    If Len(strCheckNumber) > 25 Then
                        strCheckNumber = Trim(Mid(CheckNumber, InStr(1, Trim(CheckNumber), " ") + 1)) 'parse the strCheckNumber for Cigna
                    Else
                        strCheckNumber = Trim(Left(CheckNumber, 13))
                    End If
                ElseIf Len(strCheckNumber) > 15 Then
                    If Left(strCheckNumber, 3) = "000" Then strCheckNumber = Mid(strCheckNumber, 4)
                    If Left(strCheckNumber, 1) = "0" Then strCheckNumber = Mid(strCheckNumber, 2)
                    If CLng(Mid(strCheckNumber, 1, 4)) = Year(Now()) And IsDate(Convert.ToInt32(Mid(strCheckNumber, 1, 4)) & "/" & Convert.ToInt32(Mid(strCheckNumber, 5, 2)) & "/" & Convert.ToInt32(Mid(strCheckNumber, 7, 2))) Then
                        strCheckNumber = Mid(strCheckNumber, 9)
                    ElseIf CLng("20" & Mid(CheckNumber, 1, 2)) = Year(Now()) And IsDate(Convert.ToInt32(Mid(CheckNumber, 1, 2)) & "/" & Convert.ToInt32(Mid(CheckNumber, 3, 2)) & "/" & Convert.ToInt32(Mid(CheckNumber, 5, 2))) Then
                        strCheckNumber = Mid(CheckNumber, 7)
                    End If
                End If
            Else
                If IsNumeric(Left(CheckNumber, 13)) Then
                    If CLng(Mid(CheckNumber, 1, 4)) = Year(Now()) And IsDate(Convert.ToInt32(Mid(CheckNumber, 1, 4)) & "/" & Convert.ToInt32(Mid(CheckNumber, 5, 2)) & "/" & Convert.ToInt32(Mid(CheckNumber, 7, 2))) Then
                        strCheckNumber = Mid(CheckNumber, 9)
                    ElseIf CLng("20" & Mid(CheckNumber, 1, 2)) = Year(Now()) And IsDate(Convert.ToInt32(Mid(CheckNumber, 1, 2)) & "/" & Convert.ToInt32(Mid(CheckNumber, 3, 2)) & "/" & Convert.ToInt32(Mid(CheckNumber, 5, 2))) Then
                        strCheckNumber = Mid(CheckNumber, 7)
                    End If
                Else
                    If UCase(Left(Trim(strCheckNumber), 5)) = "NOCHK" And (Len(strCheckNumber) = 0) Then
                        strCheckNumber = Left(Trim(strCheckNumber), 9) & Format(Now(), "yymmdd") & Format(Now(), "ss") & Format(Now(), "nn")
                    Else
                        If (Len(strCheckNumber) = 0) Then strCheckNumber = Trim(Left(CheckNumber, 13))
                    End If
                    If IsNumeric(Right(CheckNumber, 15)) Then
                        strCheckNumber = Right(CheckNumber, 15)
                    End If
                End If
            End If
        End If
        If (fldClearingHouseID = 1 Or fldClearingHouseID = 142) And UCase(Left(Trim(CheckNumber), 3)) = "EFT" Then
            If IsNumeric(Right(CheckNumber, 8)) Then
                strCheckNumber = CLng(Right(CheckNumber, 8)) 'parse the strCheckNumber for SmokyMTN
            Else
                strCheckNumber = Right(CheckNumber, 12)
            End If
        End If
        If HealthPlan = 1288 Then    'BcBs of MI
            If Len(CheckNumber) = 25 Then strCheckNumber = Right(CheckNumber, 9)
            If Len(CheckNumber) = 28 Then strCheckNumber = Left(CheckNumber, 19)
        End If
        If HealthPlan = 1324 Then
            If IsNumeric(Right(CheckNumber, 6)) Then
                strCheckNumber = Right(CheckNumber, 6) 'parse the strCheckNumber for BCBS of KS
            Else
                strCheckNumber = Left(CheckNumber, 17)
            End If
        End If
        If HealthPlan = 422 Then    'MHN Zero Checks
            If Len(CheckNumber) >= 25 Then strCheckNumber = Right(CheckNumber, 8)
        End If
        If (Right(PayorID, 5) = "84980" Or Right(PayorID, 5) = "00621" Or Right(PayorID, 5) = "00840" Or _
           HealthPlan = 1280 Or HealthPlan = 1292 Or HealthPlan = 1298 Or HealthPlan = 1302 Or HealthPlan = 1309 Or HealthPlan = 1308) And _
           (Len(strCheckNumber) = 0 Or Len(strCheckNumber) >= 15) Then
            If InStr(1, Trim(CheckNumber), "EZ") = 0 Then
                If Not IsNumeric(Left(strCheckNumber, 1)) And InStr(1, strCheckNumber, " ") > 0 Then
                    strCheckNumber = Trim(Left(strCheckNumber, IIf(InStr(1, strCheckNumber, " ") > 0, InStr(1, strCheckNumber, " "), Len(strCheckNumber))))
                Else
                    strCheckNumber = Left(Right(CheckNumber, 9), 8) 'parse the strCheckNumber for BCBS
                    If Len(strCheckNumber) > 0 Then
                        If ((HealthPlan = 1308) And IsNumeric(strCheckNumber)) Then strCheckNumber = Left(Right(CheckNumber, 10), 9)
                        If ((HealthPlan = 1308) And Not IsNumeric(strCheckNumber)) Then strCheckNumber = Left(Mid(CheckNumber, 9, 10), 9)
                        If Not (IsNumeric(Left(strCheckNumber, 1))) Then strCheckNumber = Left(Right(CheckNumber, 8), 7)
                        If CLng(strCheckNumber) > 0 Then strCheckNumber = CLng(strCheckNumber)
                        If CLng(strCheckNumber) = 0 Then strCheckNumber = Format(Now(), "yyyymmdd") & Format(Now(), "ss")
                    End If
                End If
            End If
        End If
        If (Right(PayorID, 5) = "MC039" Or Right(PayorID, 5) = "SKCT0") And HealthPlan = 105 And Len(strCheckNumber) >= 18 Then
            strCheckNumber = Trim(Mid(CheckNumber, 18)) 'parse the strCheckNumber for Medicaid
            If Len(strCheckNumber) > 0 Then
                If Not (IsNumeric(strCheckNumber)) Then strCheckNumber = Trim(CheckNumber)
                If CLng(strCheckNumber) > 0 Then strCheckNumber = CLng(strCheckNumber)
                If CLng(strCheckNumber) = 0 Then strCheckNumber = Format(Now(), "yyyymmdd") & Format(Now(), "ss")
            End If
        End If
        If (Right(PayorID, 5) = "36373" Or Right(PayorID, 5) = "81040") And InStr(1, Trim(CheckNumber), "PAY-PLUS PAYMENT") > 0 And Len(strCheckNumber) = 0 Then
            strCheckNumber = Trim(Right(CheckNumber, 9)) 'parse the strCheckNumber for Great West
            If Len(strCheckNumber) > 0 Then
                If Not (IsNumeric(CLng(Right(strCheckNumber, 9)))) Then strCheckNumber = Trim(CheckNumber)
                If CLng(Right(strCheckNumber, 9)) > 0 Then strCheckNumber = CLng(Right(CheckNumber, 9))
                If CLng(Right(strCheckNumber, 9)) = 0 Then strCheckNumber = Format(Now(), "yyyymmdd") & Format(Now(), "ss")
            End If
        End If
        If (Right(PayorID, 5) = "81400") And Len(CheckNumber) > 15 And HealthPlan = 79 Then '1E000000003572255
            strCheckNumber = Trim(Right(CheckNumber, 9))
            If Len(strCheckNumber) > 0 Then
                If Not (IsNumeric(CLng(Right(strCheckNumber, 9)))) Then strCheckNumber = Trim(CheckNumber)
                If CLng(Right(strCheckNumber, 9)) > 0 Then strCheckNumber = CLng(Right(strCheckNumber, 9))
                If CLng(Right(strCheckNumber, 9)) = 0 Then strCheckNumber = Format(Now(), "yyyymmdd") & Format(Now(), "ss")
            End If
        End If
        If HealthPlan = 1313 And Len(CheckNumber) > 20 Then  'Premera ' 20140614121016860EPRA71682208
            If IsNumeric(Mid(CheckNumber, 22)) And Not IsNumeric(Mid(CheckNumber, 21)) And Mid(CheckNumber, 22) <> "0" Then
                strCheckNumber = Trim(Mid(CheckNumber, 22)) 'parse the strCheckNumber for Cigna
            Else
                If IsNumeric(Mid(CheckNumber, 21)) And Mid(CheckNumber, 21) <> "0" Then
                    strCheckNumber = Trim(Mid(CheckNumber, 21)) 'parse the strCheckNumber for Cigna
                Else
                    If Mid(CheckNumber, 21) = "0" Then
                        strCheckNumber = Trim(Mid(CheckNumber, 18, 3)) & Format(Now(), "yymmdd") & Format(Now(), "ss") & Format(Now(), "nn")
                    Else
                        If Mid(CheckNumber, 22) = "0" Then
                            strCheckNumber = Trim(Mid(CheckNumber, 18, 4)) & Format(Now(), "yymmdd") & Format(Now(), "ss") & Format(Now(), "nn")
                        Else
                            strCheckNumber = Trim(Mid(CheckNumber, 18))
                        End If
                    End If
                End If
            End If
        End If
        If UCase(Trim(strCheckNumber)) = "0" Then
            strCheckNumber = Left(Trim(strCheckNumber), 9) & Format(Now(), "yymmdd") & Format(Now(), "ss") & Format(Now(), "nn")
        End If
        If InStr(1, CheckNumber, "9999999999-") > 0 Then strCheckNumber = Trim(Replace(CheckNumber, "9999999999-", ""))
        If (HealthPlan = 1305 Or HealthPlan = 1385) And strF5 = "CHP" Then HealthPlan = 1385
        If (HealthPlan = 1305 Or HealthPlan = 1385) And strF5 = "WKP" Then HealthPlan = 1305

        FindCheckNumber = strCheckNumber
    End Function


    Function Segment_Cas(ByVal rst As ADODB.Recordset, ByVal PayorID As String)
        'Dim xlFile As String
        Dim PostAmount, CoInsAmount As Boolean
        Dim objSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        objSQL = CreateObject("ADODB.Connection")
        objSQL.Open(CONST_PSYQUEL_CNN)
        'Dim ErrMsg As String
        'Dim ClaimReceiveDate, CreationDate, CheckDate, RemittanceDate, ProcessDate, FirstDateOfService, LastDateOfService As Date
        Dim strSQL, ErrMsg As String

        PostAmount = False : CoInsAmount = False : ErrMsg = ""
        If rst.Fields("F2") = "HE" Or Len(Trim(rst.Fields("F3"))) >= 4 Then
            strSQL = "SELECT * FROM tblRemittanceErrorCodes WHERE (fldRemittanceErrorCode ='" & Trim(rst.Fields("F3")) & "');"
        Else
            strSQL = "SELECT * FROM tblRemittanceErrorCodes WHERE (fldRemittanceErrorCode ='" & Trim(rst.Fields("F2")) & Trim(rst.Fields("F3")) & "');"
        End If
        rstSQL = CreateObject("ADODB.Recordset")
        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
        If Not rstSQL.EOF Then
            PostAmount = IIf(rstSQL.Fields("fldPostAmountYN") = "Y", True, False)
            CoInsAmount = IIf(rstSQL.Fields("fldCoInsAmountYN") = "Y", True, False)
            ErrMsg = Trim(IIf(IsDBNull(rstSQL.Fields("fldRemittanceErrorDescription")), "", rstSQL.Fields("fldRemittanceErrorDescription")))
            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 4 Then fldDenTxTypeID = rstSQL.Fields("fldTxTypeID")
            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 21 Then fldNoteTxTypeID = rstSQL.Fields("fldTxTypeID")
            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And ErrorMessage = "" And ErrMsg > "" Then
                ErrorMessage = ErrMsg
            ElseIf IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And ErrMsg > "" Then
                ErrorMessage = ErrorMessage + "; " + ErrMsg
            End If
        End If
        rstSQL = Nothing

        If IsDBNull(Reason1) Or IsEmpty(Reason1) Or Reason1 = "" Then
            Reason1 = Trim(rst.Fields("F2")) & Trim(rst.Fields("F3"))
            If rst.Fields("F2") = "HE" Or Len(Trim(rst.Fields("F3"))) >= 4 Then Reason1 = Trim(rst.Fields("F3"))
            Select Case rst.Fields("F2")
                Case "CO", "OA", "PI", "HE" 'Contractual Write off
                    'Other Adjustments
                    'Other Patient Adjustments
                    If Reason1 = "OA23" Then
                        If PostAmount Then OtherDisallowed = Convert.ToDouble(OtherDisallowed) + Convert.ToDouble(rst.Fields("F4"))
                    Else
                        If PostAmount Then Disallowed = Convert.ToDouble(Disallowed) + Convert.ToDouble(rst.Fields("F4"))
                    End If
                    If Not IsDBNull(rst.Fields("F6")) Then
                        Reason2 = Trim(rst.Fields("F2")) & Trim(rst.Fields("F6"))
                        If rst.Fields("F2") = "HE" Or Len(Trim(rst.Fields("F6"))) >= 4 Then Reason2 = Trim(rst.Fields("F6"))
                        strSQL = "SELECT * FROM tblRemittanceErrorCodes WHERE (fldRemittanceErrorCode ='" & Trim(Reason2) & "');"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        If Not rstSQL.EOF Then
                            ErrMsg = Trim(IIf(IsDBNull(rstSQL.Fields("fldRemittanceErrorDescription")), "", rstSQL.Fields("fldRemittanceErrorDescription")))
                            If Reason2 = "OA23" Then
                                If IIf(rstSQL.Fields("fldPostAmountYN") = "Y", True, False) Then OtherDisallowed = Convert.ToDouble(OtherDisallowed) + Convert.ToDouble(rst.Fields("F7"))
                            Else
                                If IIf(rstSQL.Fields("fldPostAmountYN") = "Y", True, False) Then Disallowed = Convert.ToDouble(Disallowed) + Convert.ToDouble(rst.Fields("F7"))
                            End If
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 4 Then fldDenTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 21 Then fldNoteTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And (Trim(ErrorMessage) = "" Or IsDBNull(ErrorMessage)) And rstSQL.Fields("fldRemittanceErrorDescription") > "" Then
                                ErrorMessage = rstSQL.Fields("fldRemittanceErrorDescription")
                            ElseIf IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And ErrMsg > "" Then
                                ErrorMessage = ErrorMessage + "; " + rstSQL.Fields("fldRemittanceErrorDescription")
                            End If
                        End If
                        rstSQL = Nothing
                    End If
                    If Not IsDBNull(rst.Fields("F9")) Then
                        Reason3 = Trim(rst.Fields("F2")) & Trim(rst.Fields("F9"))
                        If rst.Fields("F2") = "HE" Or Len(Trim(rst.Fields("F9"))) >= 4 Then Reason3 = Trim(rst.Fields("F9"))
                        strSQL = "SELECT * FROM tblRemittanceErrorCodes WHERE (fldRemittanceErrorCode ='" & Trim(Reason3) & "');"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        If Not rstSQL.EOF Then
                            ErrMsg = Trim(IIf(IsDBNull(rstSQL.Fields("fldRemittanceErrorDescription")), "", rstSQL.Fields("fldRemittanceErrorDescription")))
                            If IIf(rstSQL.Fields("fldPostAmountYN") = "Y", True, False) Then Disallowed = Convert.ToDouble(Disallowed) + Convert.ToDouble(rst.Fields("F10"))
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 4 Then fldDenTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 21 Then fldNoteTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And (Trim(ErrorMessage) = "" Or IsDBNull(ErrorMessage)) And rstSQL.Fields("fldRemittanceErrorDescription") > "" Then
                                ErrorMessage = rstSQL.Fields("fldRemittanceErrorDescription")
                            ElseIf IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And ErrMsg > "" Then
                                ErrorMessage = ErrorMessage + "; " + rstSQL.Fields("fldRemittanceErrorDescription")
                            End If
                        End If
                        rstSQL = Nothing
                    End If
                    If Not IsDBNull(rst.Fields("F12")) Then
                        Reason4 = Trim(rst.Fields("F2")) & Trim(rst.Fields("F12"))
                        If rst.Fields("F2") = "HE" Or Len(Trim(rst.Fields("F12"))) >= 4 Then Reason4 = Trim(rst.Fields("F12"))
                        strSQL = "SELECT * FROM tblRemittanceErrorCodes WHERE (fldRemittanceErrorCode ='" & Trim(Reason4) & "');"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        If Not rstSQL.EOF Then
                            ErrMsg = Trim(IIf(IsDBNull(rstSQL.Fields("fldRemittanceErrorDescription")), "", rstSQL.Fields("fldRemittanceErrorDescription")))
                            If IIf(rstSQL.Fields("fldPostAmountYN") = "Y", True, False) Then Disallowed = Convert.ToDouble(Disallowed) + Convert.ToDouble(rst.Fields("F13"))
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 4 Then fldDenTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 21 Then fldNoteTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And (Trim(ErrorMessage) = "" Or IsDBNull(ErrorMessage)) And rstSQL.Fields("fldRemittanceErrorDescription") > "" Then
                                ErrorMessage = rstSQL.Fields("fldRemittanceErrorDescription")
                            ElseIf IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And ErrMsg > "" Then
                                ErrorMessage = ErrorMessage + "; " + rstSQL.Fields("fldRemittanceErrorDescription")
                            End If
                        End If
                        rstSQL = Nothing
                    End If
                Case "CR", "PR"      'Correction And Reversal Adjustments
                    'Patient Resposibility
                    If PostAmount Then
                        If Trim(rst.Fields("F2")) = "CR" Then
                            If Trim(rst.Fields("F3")) = "42" Or Trim(rst.Fields("F3")) = "45" Or Trim(rst.Fields("F3")) = "A2" Then
                                Disallowed = Convert.ToDouble(Disallowed) + Convert.ToDouble(rst.Fields("F4"))
                            End If
                        Else
                            Deductible = Convert.ToDouble(Deductible) + Convert.ToDouble(rst.Fields("F4"))
                        End If
                    End If
                    If CoInsAmount Then Coinsurance = Coinsurance + Convert.ToDouble(rst.Fields("F4"))
                    If Not IsDBNull(rst.Fields("F6")) And Not IsDBNull(rst.Fields("F7")) Then
                        Reason2 = Trim(rst.Fields("F2")) & Trim(rst.Fields("F6"))
                        strSQL = "SELECT * FROM tblRemittanceErrorCodes WHERE (fldRemittanceErrorCode ='" & Trim(rst.Fields("F2")) & Trim(rst.Fields("F6")) & "');"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        If Not rstSQL.EOF Then
                            If IIf(rstSQL.Fields("fldPostAmountYN") = "Y", True, False) Then
                                If Trim(rst.Fields("F2")) = "CR" Then
                                    If Trim(rst.Fields("F6")) = "42" Or Trim(rst.Fields("F6")) = "45" Or Trim(rst.Fields("F6")) = "A2" Then
                                        Disallowed = Convert.ToDouble(Disallowed) + Convert.ToDouble(rst.Fields("F7"))
                                    End If
                                Else
                                    Deductible = Convert.ToDouble(Deductible) + Convert.ToDouble(rst.Fields("F7"))
                                End If
                            End If
                            ErrMsg = Trim(IIf(IsDBNull(rstSQL.Fields("fldRemittanceErrorDescription")), "", rstSQL.Fields("fldRemittanceErrorDescription")))
                            If IIf(rstSQL.Fields("fldCoInsAmountYN") = "Y", True, False) Then Coinsurance = Coinsurance + Convert.ToDouble(rst.Fields("F7"))
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 4 Then fldDenTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 21 Then fldNoteTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And (Trim(ErrorMessage) = "" Or IsDBNull(ErrorMessage)) And rstSQL.Fields("fldRemittanceErrorDescription") > "" Then
                                ErrorMessage = rstSQL.Fields("fldRemittanceErrorDescription")
                            ElseIf IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And ErrMsg > "" Then
                                ErrorMessage = ErrorMessage + "; " + rstSQL.Fields("fldRemittanceErrorDescription")
                            End If
                        End If
                        rstSQL = Nothing
                    End If
                    If Not IsDBNull(rst.Fields("F9")) And Not IsDBNull(rst.Fields("F10")) Then
                        Reason3 = Trim(rst.Fields("F2")) & Trim(rst.Fields("F9"))
                        strSQL = "SELECT * FROM tblRemittanceErrorCodes WHERE (fldRemittanceErrorCode ='" & Trim(rst.Fields("F2")) & Trim(rst.Fields("F9")) & "');"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        If Not rstSQL.EOF Then
                            If IIf(rstSQL.Fields("fldPostAmountYN") = "Y", True, False) Then
                                If Trim(rst.Fields("F2")) = "CR" Then
                                    If Trim(rst.Fields("F9")) = "42" Or Trim(rst.Fields("F9")) = "45" Or Trim(rst.Fields("F9")) = "A2" Then
                                        Disallowed = Convert.ToDouble(Disallowed) + Convert.ToDouble(rst.Fields("F10"))
                                    End If
                                Else
                                    Deductible = Convert.ToDouble(Deductible) + Convert.ToDouble(rst.Fields("F10"))
                                End If
                            End If
                            ErrMsg = Trim(IIf(IsDBNull(rstSQL.Fields("fldRemittanceErrorDescription")), "", rstSQL.Fields("fldRemittanceErrorDescription")))
                            If IIf(rstSQL.Fields("fldCoInsAmountYN") = "Y", True, False) Then Coinsurance = Coinsurance + Convert.ToDouble(rst.Fields("F10"))
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 4 Then fldDenTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 21 Then fldNoteTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And (Trim(ErrorMessage) = "" Or IsDBNull(ErrorMessage)) And rstSQL.Fields("fldRemittanceErrorDescription") > "" Then
                                ErrorMessage = rstSQL.Fields("fldRemittanceErrorDescription")
                            ElseIf IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And ErrMsg > "" Then
                                ErrorMessage = ErrorMessage + "; " + rstSQL.Fields("fldRemittanceErrorDescription")
                            End If
                        End If
                        rstSQL = Nothing
                    End If
                    If Not IsDBNull(rst.Fields("F12")) And Not IsDBNull(rst.Fields("F13")) Then
                        Reason4 = Trim(rst.Fields("F2")) & Trim(rst.Fields("F12"))
                        strSQL = "SELECT * FROM tblRemittanceErrorCodes WHERE (fldRemittanceErrorCode ='" & Trim(rst.Fields("F2")) & Trim(rst.Fields("F12")) & "');"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        If Not rstSQL.EOF Then
                            If IIf(rstSQL.Fields("fldPostAmountYN") = "Y", True, False) Then
                                If Trim(rst.Fields("F2")) = "CR" Then
                                    If Trim(rst.Fields("F12")) = "42" Or Trim(rst.Fields("F12")) = "45" Or Trim(rst.Fields("F12")) = "A2" Then
                                        Disallowed = Convert.ToDouble(Disallowed) + Convert.ToDouble(rst.Fields("F13"))
                                    End If
                                Else
                                    Deductible = Convert.ToDouble(Deductible) + Convert.ToDouble(rst.Fields("F13"))
                                End If
                            End If
                            ErrMsg = Trim(IIf(IsDBNull(rstSQL.Fields("fldRemittanceErrorDescription")), "", rstSQL.Fields("fldRemittanceErrorDescription")))
                            If IIf(rstSQL.Fields("fldCoInsAmountYN") = "Y", True, False) Then Coinsurance = Coinsurance + Convert.ToDouble(rst.Fields("F13"))
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 4 Then fldDenTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 21 Then fldNoteTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And (Trim(ErrorMessage) = "" Or IsDBNull(ErrorMessage)) And rstSQL.Fields("fldRemittanceErrorDescription") > "" Then
                                ErrorMessage = rstSQL.Fields("fldRemittanceErrorDescription")
                            ElseIf IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And ErrMsg > "" Then
                                ErrorMessage = ErrorMessage + "; " + rstSQL.Fields("fldRemittanceErrorDescription")
                            End If
                        End If
                        rstSQL = Nothing
                    End If
            End Select
            If (PayorID = "86916" And Payment <> 0 And (Reason1 = "CO144" Or Reason1 = "OAA2")) Then
                Disallowed = Charge - Payment
            End If
        ElseIf IsDBNull(Reason2) Or IsEmpty(Reason2) Or Reason2 = "" Then
            Select Case rst.Fields("F2")
                Case "CO", "OA", "PI", "HE" 'Contractual Write off
                    'Other Adjustments
                    'Other Patient Adjustments
                    Reason2 = Trim(rst.Fields("F2")) & Trim(rst.Fields("F3"))
                    If rst.Fields("F2") = "HE" Or Len(Trim(rst.Fields("F3"))) >= 4 Then Reason2 = Trim(rst.Fields("F3"))
                    If Reason2 = "OA23" Then
                        If PostAmount Then OtherDisallowed = Convert.ToDouble(OtherDisallowed) + Convert.ToDouble(rst.Fields("F4"))
                    Else
                        If PostAmount Then Disallowed = Convert.ToDouble(Disallowed) + Convert.ToDouble(rst.Fields("F4"))
                    End If
                    If Not IsDBNull(rst.Fields("F6")) Then
                        Reason3 = Trim(rst.Fields("F2")) & Trim(rst.Fields("F6"))
                        If rst.Fields("F2") = "HE" Or Len(Trim(rst.Fields("F6"))) >= 4 Then Reason3 = Trim(rst.Fields("F6"))
                        strSQL = "SELECT * FROM tblRemittanceErrorCodes WHERE (fldRemittanceErrorCode ='" & Trim(Reason3) & "');"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        If Not rstSQL.EOF Then
                            ErrMsg = Trim(IIf(IsDBNull(rstSQL.Fields("fldRemittanceErrorDescription")), "", rstSQL.Fields("fldRemittanceErrorDescription")))
                            If Reason3 = "OA23" Then
                                If IIf(rstSQL.Fields("fldPostAmountYN") = "Y", True, False) Then OtherDisallowed = Convert.ToDouble(OtherDisallowed) + Convert.ToDouble(rst.Fields("F7"))
                            Else
                                If IIf(rstSQL.Fields("fldPostAmountYN") = "Y", True, False) Then Disallowed = Convert.ToDouble(Disallowed) + Convert.ToDouble(rst.Fields("F7"))
                            End If
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 4 Then fldDenTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 21 Then fldNoteTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And (Trim(ErrorMessage) = "" Or IsDBNull(ErrorMessage)) And rstSQL.Fields("fldRemittanceErrorDescription") > "" Then
                                ErrorMessage = rstSQL.Fields("fldRemittanceErrorDescription")
                            ElseIf IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And ErrMsg > "" Then
                                ErrorMessage = ErrorMessage + "; " + rstSQL.Fields("fldRemittanceErrorDescription")
                            End If
                        End If
                        rstSQL = Nothing
                    End If
                Case "CR", "PR"      'Correction And Reversal Adjustments
                    'Patient Resposibility
                    Reason2 = Trim(rst.Fields("F2")) & Trim(rst.Fields("F3"))
                    If PostAmount Then
                        If Trim(rst.Fields("F2")) = "CR" Then
                            If Trim(rst.Fields("F3")) = "42" Or Trim(rst.Fields("F3")) = "45" Or Trim(rst.Fields("F3")) = "A2" Then
                                Disallowed = Convert.ToDouble(Disallowed) + Convert.ToDouble(rst.Fields("F4"))
                            End If
                        Else
                            Deductible = Convert.ToDouble(Deductible) + Convert.ToDouble(rst.Fields("F4"))
                        End If
                    End If
                    If CoInsAmount Then Coinsurance = Coinsurance + rst.Fields("F4")
                    If Not IsDBNull(rst.Fields("F6")) And Not IsDBNull(rst.Fields("F7")) Then
                        Reason3 = Trim(rst.Fields("F2")) & Trim(rst.Fields("F6"))
                        strSQL = "SELECT * FROM tblRemittanceErrorCodes WHERE (fldRemittanceErrorCode ='" & Trim(Reason3) & "');"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        If Not rstSQL.EOF Then
                            If IIf(rstSQL.Fields("fldPostAmountYN") = "Y", True, False) Then
                                If Trim(rst.Fields("F2")) = "CR" Then
                                    If Trim(rst.Fields("F6")) = "42" Or Trim(rst.Fields("F6")) = "45" Or Trim(rst.Fields("F6")) = "A2" Then
                                        Disallowed = Convert.ToDouble(Disallowed) + Convert.ToDouble(rst.Fields("F7"))
                                    End If
                                Else
                                    Deductible = Convert.ToDouble(Deductible) + Convert.ToDouble(rst.Fields("F7"))
                                End If
                            End If
                            ErrMsg = Trim(IIf(IsDBNull(rstSQL.Fields("fldRemittanceErrorDescription")), "", rstSQL.Fields("fldRemittanceErrorDescription")))
                            If IIf(rstSQL.Fields("fldCoInsAmountYN") = "Y", True, False) Then Coinsurance = Coinsurance + Convert.ToDouble(rst.Fields("F7"))
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 4 Then fldDenTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 21 Then fldNoteTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And (Trim(ErrorMessage) = "" Or IsDBNull(ErrorMessage)) And rstSQL.Fields("fldRemittanceErrorDescription") > "" Then
                                ErrorMessage = rstSQL.Fields("fldRemittanceErrorDescription")
                            ElseIf IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And ErrMsg > "" Then
                                ErrorMessage = ErrorMessage + "; " + rstSQL.Fields("fldRemittanceErrorDescription")
                            End If
                        End If
                        rstSQL = Nothing
                    End If
                    If Not IsDBNull(rst.Fields("F9")) And Not IsDBNull(rst.Fields("F10")) Then
                        Reason4 = Trim(rst.Fields("F2")) & Trim(rst.Fields("F9"))
                        strSQL = "SELECT * FROM tblRemittanceErrorCodes WHERE (fldRemittanceErrorCode ='" & Trim(rst.Fields("F2")) & Trim(rst.Fields("F9")) & "');"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        If Not rstSQL.EOF Then
                            If IIf(rstSQL.Fields("fldPostAmountYN") = "Y", True, False) Then
                                If Trim(rst.Fields("F2")) = "CR" Then
                                    If Trim(rst.Fields("F9")) = "42" Or Trim(rst.Fields("F9")) = "45" Or Trim(rst.Fields("F9")) = "A2" Then
                                        Disallowed = Convert.ToDouble(Disallowed) + Convert.ToDouble(rst.Fields("F10"))
                                    End If
                                Else
                                    Deductible = Convert.ToDouble(Deductible) + Convert.ToDouble(rst.Fields("F10"))
                                End If
                            End If
                            ErrMsg = Trim(IIf(IsDBNull(rstSQL.Fields("fldRemittanceErrorDescription")), "", rstSQL.Fields("fldRemittanceErrorDescription")))
                            If IIf(rstSQL.Fields("fldCoInsAmountYN") = "Y", True, False) Then Coinsurance = Coinsurance + Convert.ToDouble(rst.Fields("F10"))
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 4 Then fldDenTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 21 Then fldNoteTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And (Trim(ErrorMessage) = "" Or IsDBNull(ErrorMessage)) And rstSQL.Fields("fldRemittanceErrorDescription") > "" Then
                                ErrorMessage = rstSQL.Fields("fldRemittanceErrorDescription")
                            ElseIf IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And ErrMsg > "" Then
                                ErrorMessage = ErrorMessage + "; " + rstSQL.Fields("fldRemittanceErrorDescription")
                            End If
                        End If
                        rstSQL = Nothing
                    End If
                    If Not IsDBNull(rst.Fields("F12")) And Not IsDBNull(rst.Fields("F13")) Then
                        Reason5 = Trim(rst.Fields("F2")) & Trim(rst.Fields("F12"))
                        strSQL = "SELECT * FROM tblRemittanceErrorCodes WHERE (fldRemittanceErrorCode ='" & Trim(rst.Fields("F2")) & Trim(rst.Fields("F12")) & "');"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        If Not rstSQL.EOF Then
                            If IIf(rstSQL.Fields("fldPostAmountYN") = "Y", True, False) Then
                                If Trim(rst.Fields("F2")) = "CR" Then
                                    If Trim(rst.Fields("F12")) = "42" Or Trim(rst.Fields("F12")) = "45" Or Trim(rst.Fields("F12")) = "A2" Then
                                        Disallowed = Convert.ToDouble(Disallowed) + Convert.ToDouble(rst.Fields("F13"))
                                    End If
                                Else
                                    Deductible = Convert.ToDouble(Deductible) + Convert.ToDouble(rst.Fields("F13"))
                                End If
                            End If
                            ErrMsg = Trim(IIf(IsDBNull(rstSQL.Fields("fldRemittanceErrorDescription")), "", rstSQL.Fields("fldRemittanceErrorDescription")))
                            If IIf(rstSQL.Fields("fldCoInsAmountYN") = "Y", True, False) Then Coinsurance = Coinsurance + Convert.ToDouble(rst.Fields("F13"))
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 4 Then fldDenTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 21 Then fldNoteTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And (Trim(ErrorMessage) = "" Or IsDBNull(ErrorMessage)) And rstSQL.Fields("fldRemittanceErrorDescription") > "" Then
                                ErrorMessage = rstSQL.Fields("fldRemittanceErrorDescription")
                            ElseIf IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And ErrMsg > "" Then
                                ErrorMessage = ErrorMessage + "; " + rstSQL.Fields("fldRemittanceErrorDescription")
                            End If
                        End If
                        rstSQL = Nothing
                    End If
            End Select
            If (PayorID = "86916" And Payment <> 0 And (Reason2 = "CO144" Or Reason2 = "OAA2")) Then
                Disallowed = Charge - Payment
            End If
        ElseIf IsDBNull(Reason3) Or IsEmpty(Reason3) Or Reason3 = "" Then
            Select Case rst.Fields("F2")
                Case "CO", "OA", "PI", "HE" 'Contractual Write off
                    'Other Adjustments
                    'Other Patient Adjustments
                    Reason3 = Trim(rst.Fields("F2")) & Trim(rst.Fields("F3"))
                    If rst.Fields("F2") = "HE" Then Reason3 = Trim(rst.Fields("F3"))
                    If Reason3 = "OA23" Then
                        If PostAmount Then OtherDisallowed = Convert.ToDouble(OtherDisallowed) + Convert.ToDouble(rst.Fields("F4"))
                    Else
                        If PostAmount Then Disallowed = Convert.ToDouble(Disallowed) + Convert.ToDouble(rst.Fields("F4"))
                    End If
                    If Not IsDBNull(rst.Fields("F6")) Then
                        Reason4 = Trim(rst.Fields("F2")) & Trim(rst.Fields("F6"))
                        If rst.Fields("F2") = "HE" Then Reason4 = Trim(rst.Fields("F6"))
                        strSQL = "SELECT * FROM tblRemittanceErrorCodes WHERE (fldRemittanceErrorCode ='" & Trim(rst.Fields("F2")) & Trim(rst.Fields("F6")) & "');"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        If Not rstSQL.EOF Then
                            ErrMsg = Trim(IIf(IsDBNull(rstSQL.Fields("fldRemittanceErrorDescription")), "", rstSQL.Fields("fldRemittanceErrorDescription")))
                            If Reason4 = "OA23" Then
                                If IIf(rstSQL.Fields("fldPostAmountYN") = "Y", True, False) Then OtherDisallowed = Convert.ToDouble(OtherDisallowed) + Convert.ToDouble(rst.Fields("F7"))
                            Else
                                If IIf(rstSQL.Fields("fldPostAmountYN") = "Y", True, False) Then Disallowed = Convert.ToDouble(Disallowed) + Convert.ToDouble(rst.Fields("F7"))
                            End If
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 4 Then fldDenTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 21 Then fldNoteTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And Trim(ErrorMessage) = "" And rstSQL.Fields("fldRemittanceErrorDescription") > "" Then
                                ErrorMessage = rstSQL.Fields("fldRemittanceErrorDescription")
                            ElseIf IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And ErrMsg > "" Then
                                ErrorMessage = ErrorMessage + "; " + rstSQL.Fields("fldRemittanceErrorDescription")
                            End If
                        End If
                        rstSQL = Nothing
                    End If
                Case "CR", "PR"      'Correction And Reversal Adjustments
                    'Patient Resposibility
                    Reason3 = Trim(rst.Fields("F2")) & Trim(rst.Fields("F3"))
                    If PostAmount Then
                        If Trim(rst.Fields("F2")) = "CR" Then
                            If Trim(rst.Fields("F3")) = "42" Or Trim(rst.Fields("F3")) = "45" Or Trim(rst.Fields("F3")) = "A2" Then
                                Disallowed = Convert.ToDouble(Disallowed) + Convert.ToDouble(rst.Fields("F4"))
                            End If
                        Else
                            Deductible = Convert.ToDouble(Deductible) + Convert.ToDouble(rst.Fields("F4"))
                        End If
                    End If
                    If CoInsAmount Then Coinsurance = Coinsurance + rst.Fields("F4")
                    If Not IsDBNull(rst.Fields("F6")) And Not IsDBNull(rst.Fields("F7")) Then
                        Reason4 = Trim(rst.Fields("F2")) & Trim(rst.Fields("F6"))
                        strSQL = "SELECT * FROM tblRemittanceErrorCodes WHERE (fldRemittanceErrorCode ='" & Trim(rst.Fields("F2")) & Trim(rst.Fields("F6")) & "');"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        If Not rstSQL.EOF Then
                            If IIf(rstSQL.Fields("fldPostAmountYN") = "Y", True, False) Then
                                If Trim(rst.Fields("F2")) = "CR" Then
                                    If Trim(rst.Fields("F6")) = "42" Or Trim(rst.Fields("F6")) = "45" Or Trim(rst.Fields("F6")) = "A2" Then
                                        Disallowed = Convert.ToDouble(Disallowed) + Convert.ToDouble(rst.Fields("F7"))
                                    End If
                                Else
                                    Deductible = Convert.ToDouble(Deductible) + Convert.ToDouble(rst.Fields("F7"))
                                End If
                            End If
                            ErrMsg = Trim(IIf(IsDBNull(rstSQL.Fields("fldRemittanceErrorDescription")), "", rstSQL.Fields("fldRemittanceErrorDescription")))
                            If IIf(rstSQL.Fields("fldCoInsAmountYN") = "Y", True, False) Then Coinsurance = Coinsurance + Convert.ToDouble(rst.Fields("F7"))
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 4 Then fldDenTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 21 Then fldNoteTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And Trim(ErrorMessage) = "" And rstSQL.Fields("fldRemittanceErrorDescription") > "" Then
                                ErrorMessage = rstSQL.Fields("fldRemittanceErrorDescription")
                            ElseIf IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And ErrMsg > "" Then
                                ErrorMessage = ErrorMessage + "; " + rstSQL.Fields("fldRemittanceErrorDescription")
                            End If
                        End If
                        rstSQL = Nothing
                    End If
                    If Not IsDBNull(rst.Fields("F9")) And Not IsDBNull(rst.Fields("F10")) Then
                        Reason5 = Trim(rst.Fields("F2")) & Trim(rst.Fields("F9"))
                        strSQL = "SELECT * FROM tblRemittanceErrorCodes WHERE (fldRemittanceErrorCode ='" & Trim(rst.Fields("F2")) & Trim(rst.Fields("F9")) & "');"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        If Not rstSQL.EOF Then
                            If IIf(rstSQL.Fields("fldPostAmountYN") = "Y", True, False) Then
                                If Trim(rst.Fields("F2")) = "CR" Then
                                    If Trim(rst.Fields("F9")) = "42" Or Trim(rst.Fields("F9")) = "45" Or Trim(rst.Fields("F9")) = "A2" Then
                                        Disallowed = Convert.ToDouble(Disallowed) + Convert.ToDouble(rst.Fields("F10"))
                                    End If
                                Else
                                    Deductible = Convert.ToDouble(Deductible) + Convert.ToDouble(rst.Fields("F10"))
                                End If
                            End If
                            ErrMsg = Trim(IIf(IsDBNull(rstSQL.Fields("fldRemittanceErrorDescription")), "", rstSQL.Fields("fldRemittanceErrorDescription")))
                            If IIf(rstSQL.Fields("fldCoInsAmountYN") = "Y", True, False) Then Coinsurance = Coinsurance + Convert.ToDouble(rst.Fields("F10"))
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 4 Then fldDenTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 21 Then fldNoteTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And Trim(ErrorMessage) = "" And rstSQL.Fields("fldRemittanceErrorDescription") > "" Then
                                ErrorMessage = rstSQL.Fields("fldRemittanceErrorDescription")
                            ElseIf IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And ErrMsg > "" Then
                                ErrorMessage = ErrorMessage + "; " + rstSQL.Fields("fldRemittanceErrorDescription")
                            End If
                        End If
                        rstSQL = Nothing
                    End If
                    If Not IsDBNull(rst.Fields("F12")) And Not IsDBNull(rst.Fields("F13")) Then
                        Reason6 = Trim(rst.Fields("F2")) & Trim(rst.Fields("F12"))
                        strSQL = "SELECT * FROM tblRemittanceErrorCodes WHERE (fldRemittanceErrorCode ='" & Trim(rst.Fields("F2")) & Trim(rst.Fields("F12")) & "');"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        If Not rstSQL.EOF Then
                            If IIf(rstSQL.Fields("fldPostAmountYN") = "Y", True, False) Then
                                If Trim(rst.Fields("F2")) = "CR" Then
                                    If Trim(rst.Fields("F12")) = "42" Or Trim(rst.Fields("F12")) = "45" Or Trim(rst.Fields("F12")) = "A2" Then
                                        Disallowed = Convert.ToDouble(Disallowed) + Convert.ToDouble(rst.Fields("F13"))
                                    End If
                                Else
                                    Deductible = Convert.ToDouble(Deductible) + Convert.ToDouble(rst.Fields("F13"))
                                End If
                            End If
                            ErrMsg = Trim(IIf(IsDBNull(rstSQL.Fields("fldRemittanceErrorDescription")), "", rstSQL.Fields("fldRemittanceErrorDescription")))
                            If IIf(rstSQL.Fields("fldCoInsAmountYN") = "Y", True, False) Then Coinsurance = Coinsurance + Convert.ToDouble(rst.Fields("F13"))
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 4 Then fldDenTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 21 Then fldNoteTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And Trim(ErrorMessage) = "" And rstSQL.Fields("fldRemittanceErrorDescription") > "" Then
                                ErrorMessage = rstSQL.Fields("fldRemittanceErrorDescription")
                            ElseIf IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And ErrMsg > "" Then
                                ErrorMessage = ErrorMessage + "; " + rstSQL.Fields("fldRemittanceErrorDescription")
                            End If
                        End If
                        rstSQL = Nothing
                    End If
            End Select
            If (PayorID = "86916" And Payment <> 0 And (Reason3 = "CO144" Or Reason3 = "OAA2")) Then
                Disallowed = Charge - Payment
            End If
        ElseIf IsDBNull(Reason4) Or IsEmpty(Reason4) Or Reason4 = "" Then
            Select Case rst.Fields("F2")
                Case "CO", "OA", "PI", "HE" 'Contractual Write off
                    'Other Adjustments
                    'Other Patient Adjustments
                    Reason4 = Trim(rst.Fields("F2")) & Trim(rst.Fields("F3"))
                    If rst.Fields("F2") = "HE" Then Reason4 = Trim(rst.Fields("F3"))
                    If PostAmount Then Disallowed = Convert.ToDouble(Disallowed) + Convert.ToDouble(rst.Fields("F4"))
                    If Not IsDBNull(rst.Fields("F6")) Then
                        Reason5 = Trim(rst.Fields("F2")) & Trim(rst.Fields("F6"))
                        If rst.Fields("F2") = "HE" Then Reason5 = Trim(rst.Fields("F6"))
                        strSQL = "SELECT * FROM tblRemittanceErrorCodes WHERE (fldRemittanceErrorCode ='" & Trim(rst.Fields("F2")) & Trim(rst.Fields("F6")) & "');"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        If Not rstSQL.EOF Then
                            If IIf(rstSQL.Fields("fldPostAmountYN") = "Y", True, False) Then Disallowed = Convert.ToDouble(Disallowed) + Convert.ToDouble(rst.Fields("F7"))
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 4 Then fldDenTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 21 Then fldNoteTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And Trim(ErrorMessage) = "" And rstSQL.Fields("fldRemittanceErrorDescription") > "" Then
                                ErrorMessage = rstSQL.Fields("fldRemittanceErrorDescription")
                            ElseIf IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And ErrMsg > "" Then
                                ErrorMessage = ErrorMessage + "; " + rstSQL.Fields("fldRemittanceErrorDescription")
                            End If
                        End If
                        rstSQL = Nothing
                    End If
                Case "CR", "PR"      'Correction And Reversal Adjustments
                    'Patient Resposibility
                    Reason4 = Trim(rst.Fields("F2")) & Trim(rst.Fields("F3"))
                    If PostAmount Then
                        If (Trim(rst.Fields("F2")) & Trim(rst.Fields("F3")) = "CR42") Or (Trim(rst.Fields("F2")) & Trim(rst.Fields("F3")) = "CRA2") Then
                            Disallowed = Convert.ToDouble(Disallowed) + Convert.ToDouble(rst.Fields("F4"))
                        Else
                            Deductible = Convert.ToDouble(Deductible) + Convert.ToDouble(rst.Fields("F4"))
                        End If
                    End If
                    If CoInsAmount Then Coinsurance = Coinsurance + rst.Fields("F4")
                    If Not IsDBNull(rst.Fields("F6")) And Not IsDBNull(rst.Fields("F7")) Then
                        Reason5 = Trim(rst.Fields("F2")) & Trim(rst.Fields("F6"))
                        strSQL = "SELECT * FROM tblRemittanceErrorCodes WHERE (fldRemittanceErrorCode ='" & Trim(rst.Fields("F2")) & Trim(rst.Fields("F6")) & "');"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        If Not rstSQL.EOF Then
                            If IIf(rstSQL.Fields("fldPostAmountYN") = "Y", True, False) Then
                                If Trim(rst.Fields("F2")) & Trim(rst.Fields("F6")) = "CR42" Then
                                    Disallowed = Convert.ToDouble(Disallowed) + Convert.ToDouble(rst.Fields("F7"))
                                Else
                                    Deductible = Convert.ToDouble(Deductible) + Convert.ToDouble(rst.Fields("F7"))
                                End If
                            End If
                            If IIf(rstSQL.Fields("fldCoInsAmountYN") = "Y", True, False) Then Coinsurance = Coinsurance + Convert.ToDouble(rst.Fields("F7"))
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 4 Then fldDenTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 21 Then fldNoteTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And Trim(ErrorMessage) = "" And rstSQL.Fields("fldRemittanceErrorDescription") > "" Then
                                ErrorMessage = rstSQL.Fields("fldRemittanceErrorDescription")
                            ElseIf IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And ErrMsg > "" Then
                                ErrorMessage = ErrorMessage + "; " + rstSQL.Fields("fldRemittanceErrorDescription")
                            End If
                        End If
                        rstSQL = Nothing
                    End If
                    If Not IsDBNull(rst.Fields("F9")) And Not IsDBNull(rst.Fields("F10")) Then
                        Reason6 = Trim(rst.Fields("F2")) & Trim(rst.Fields("F9"))
                        strSQL = "SELECT * FROM tblRemittanceErrorCodes WHERE (fldRemittanceErrorCode ='" & Trim(rst.Fields("F2")) & Trim(rst.Fields("F9")) & "');"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        If Not rstSQL.EOF Then
                            If IIf(rstSQL.Fields("fldPostAmountYN") = "Y", True, False) Then
                                If Trim(rst.Fields("F2")) & Trim(rst.Fields("F9")) = "CR42" Then
                                    Disallowed = Convert.ToDouble(Disallowed) + Convert.ToDouble(rst.Fields("F10"))
                                Else
                                    Deductible = Convert.ToDouble(Deductible) + Convert.ToDouble(rst.Fields("F10"))
                                End If
                            End If
                            If IIf(rstSQL.Fields("fldCoInsAmountYN") = "Y", True, False) Then Coinsurance = Coinsurance + Convert.ToDouble(rst.Fields("F10"))
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 4 Then fldDenTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 21 Then fldNoteTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And Trim(ErrorMessage) = "" And rstSQL.Fields("fldRemittanceErrorDescription") > "" Then
                                ErrorMessage = rstSQL.Fields("fldRemittanceErrorDescription")
                            ElseIf IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And ErrMsg > "" Then
                                ErrorMessage = ErrorMessage + "; " + rstSQL.Fields("fldRemittanceErrorDescription")
                            End If
                        End If
                        rstSQL = Nothing
                    End If
                    If Not IsDBNull(rst.Fields("F12")) And Not IsDBNull(rst.Fields("F13")) Then
                        Reason7 = Trim(rst.Fields("F2")) & Trim(rst.Fields("F12"))
                        strSQL = "SELECT * FROM tblRemittanceErrorCodes WHERE (fldRemittanceErrorCode ='" & Trim(rst.Fields("F2")) & Trim(rst.Fields("F12")) & "');"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        If Not rstSQL.EOF Then
                            If IIf(rstSQL.Fields("fldPostAmountYN") = "Y", True, False) Then
                                If Trim(rst.Fields("F2")) & Trim(rst.Fields("F12")) = "CR42" Then
                                    Disallowed = Convert.ToDouble(Disallowed) + Convert.ToDouble(rst.Fields("F13"))
                                Else
                                    Deductible = Convert.ToDouble(Deductible) + Convert.ToDouble(rst.Fields("F13"))
                                End If
                            End If
                            If IIf(rstSQL.Fields("fldCoInsAmountYN") = "Y", True, False) Then Coinsurance = Coinsurance + Convert.ToDouble(rst.Fields("F13"))
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 4 Then fldDenTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 21 Then fldNoteTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And Trim(ErrorMessage) = "" And rstSQL.Fields("fldRemittanceErrorDescription") > "" Then
                                ErrorMessage = rstSQL.Fields("fldRemittanceErrorDescription")
                            ElseIf IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And ErrMsg > "" Then
                                ErrorMessage = ErrorMessage + "; " + rstSQL.Fields("fldRemittanceErrorDescription")
                            End If
                        End If
                        rstSQL = Nothing
                    End If
            End Select
            If (PayorID = "86916" And Payment <> 0 And (Reason4 = "CO144" Or Reason4 = "OAA2")) Then
                Disallowed = Charge - Payment
            End If

        ElseIf IsDBNull(Reason5) Or IsEmpty(Reason5) Or Reason5 = "" Then
            Select Case rst.Fields("F2")
                Case "CO", "OA", "PI", "HE" 'Contractual Write off
                    'Other Adjustments
                    'Other Patient Adjustments
                    Reason5 = Trim(rst.Fields("F2")) & Trim(rst.Fields("F3"))
                    If rst.Fields("F2") = "HE" Then Reason5 = Trim(rst.Fields("F3"))
                    If PostAmount Then Disallowed = Convert.ToDouble(Disallowed) + Convert.ToDouble(rst.Fields("F4"))
                    If Not IsDBNull(rst.Fields("F6")) Then
                        Reason6 = Trim(rst.Fields("F2")) & Trim(rst.Fields("F6"))
                        If rst.Fields("F2") = "HE" Then Reason6 = Trim(rst.Fields("F6"))
                        strSQL = "SELECT * FROM tblRemittanceErrorCodes WHERE (fldRemittanceErrorCode ='" & Trim(rst.Fields("F2")) & Trim(rst.Fields("F6")) & "');"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        If Not rstSQL.EOF Then
                            If IIf(rstSQL.Fields("fldPostAmountYN") = "Y", True, False) Then Disallowed = Convert.ToDouble(Disallowed) + Convert.ToDouble(rst.Fields("F7"))
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 4 Then fldDenTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 21 Then fldNoteTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And Trim(ErrorMessage) = "" And rstSQL.Fields("fldRemittanceErrorDescription") > "" Then
                                ErrorMessage = rstSQL.Fields("fldRemittanceErrorDescription")
                            ElseIf IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And ErrMsg > "" Then
                                ErrorMessage = ErrorMessage + "; " + rstSQL.Fields("fldRemittanceErrorDescription")
                            End If
                        End If
                        rstSQL = Nothing
                    End If
                Case "CR", "PR"      'Correction And Reversal Adjustments
                    'Patient Resposibility
                    Reason5 = Trim(rst.Fields("F2")) & Trim(rst.Fields("F3"))
                    If PostAmount Then
                        If (Trim(rst.Fields("F2")) & Trim(rst.Fields("F3")) = "CR42") Or (Trim(rst.Fields("F2")) & Trim(rst.Fields("F3")) = "CRA2") Then
                            Disallowed = Convert.ToDouble(Disallowed) + Convert.ToDouble(rst.Fields("F4"))
                        Else
                            Deductible = Convert.ToDouble(Deductible) + Convert.ToDouble(rst.Fields("F4"))
                        End If
                    End If
                    If CoInsAmount Then Coinsurance = Coinsurance + rst.Fields("F4")
                    If Not IsDBNull(rst.Fields("F6")) And Not IsDBNull(rst.Fields("F7")) Then
                        Reason6 = Trim(rst.Fields("F2")) & Trim(rst.Fields("F6"))
                        strSQL = "SELECT * FROM tblRemittanceErrorCodes WHERE (fldRemittanceErrorCode ='" & Trim(rst.Fields("F2")) & Trim(rst.Fields("F6")) & "');"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        If Not rstSQL.EOF Then
                            If IIf(rstSQL.Fields("fldPostAmountYN") = "Y", True, False) Then
                                If Trim(rst.Fields("F2")) & Trim(rst.Fields("F6")) = "CR42" Then
                                    Disallowed = Convert.ToDouble(Disallowed) + Convert.ToDouble(rst.Fields("F7"))
                                Else
                                    Deductible = Convert.ToDouble(Deductible) + Convert.ToDouble(rst.Fields("F7"))
                                End If
                            End If
                            If IIf(rstSQL.Fields("fldCoInsAmountYN") = "Y", True, False) Then Coinsurance = Coinsurance + Convert.ToDouble(rst.Fields("F7"))
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 4 Then fldDenTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 21 Then fldNoteTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And Trim(ErrorMessage) = "" And rstSQL.Fields("fldRemittanceErrorDescription") > "" Then
                                ErrorMessage = rstSQL.Fields("fldRemittanceErrorDescription")
                            ElseIf IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And ErrMsg > "" Then
                                ErrorMessage = ErrorMessage + "; " + rstSQL.Fields("fldRemittanceErrorDescription")
                            End If
                        End If
                        rstSQL = Nothing
                    End If
                    If Not IsDBNull(rst.Fields("F9")) And Not IsDBNull(rst.Fields("F10")) Then
                        Reason7 = Trim(rst.Fields("F2")) & Trim(rst.Fields("F9"))
                        strSQL = "SELECT * FROM tblRemittanceErrorCodes WHERE (fldRemittanceErrorCode ='" & Trim(rst.Fields("F2")) & Trim(rst.Fields("F9")) & "');"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        If Not rstSQL.EOF Then
                            If IIf(rstSQL.Fields("fldPostAmountYN") = "Y", True, False) Then
                                If Trim(rst.Fields("F2")) & Trim(rst.Fields("F9")) = "CR42" Then
                                    Disallowed = Convert.ToDouble(Disallowed) + Convert.ToDouble(rst.Fields("F10"))
                                Else
                                    Deductible = Convert.ToDouble(Deductible) + Convert.ToDouble(rst.Fields("F10"))
                                End If
                            End If
                            If IIf(rstSQL.Fields("fldCoInsAmountYN") = "Y", True, False) Then Coinsurance = Coinsurance + Convert.ToDouble(rst.Fields("F10"))
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 4 Then fldDenTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 21 Then fldNoteTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And Trim(ErrorMessage) = "" And rstSQL.Fields("fldRemittanceErrorDescription") > "" Then
                                ErrorMessage = rstSQL.Fields("fldRemittanceErrorDescription")
                            ElseIf IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And ErrMsg > "" Then
                                ErrorMessage = ErrorMessage + "; " + rstSQL.Fields("fldRemittanceErrorDescription")
                            End If
                        End If
                        rstSQL = Nothing
                    End If
                    If Not IsDBNull(rst.Fields("F12")) And Not IsDBNull(rst.Fields("F13")) Then
                        Reason8 = Trim(rst.Fields("F2")) & Trim(rst.Fields("F12"))
                        strSQL = "SELECT * FROM tblRemittanceErrorCodes WHERE (fldRemittanceErrorCode ='" & Trim(rst.Fields("F2")) & Trim(rst.Fields("F12")) & "');"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        If Not rstSQL.EOF Then
                            If IIf(rstSQL.Fields("fldPostAmountYN") = "Y", True, False) Then
                                If Trim(rst.Fields("F2")) & Trim(rst.Fields("F12")) = "CR42" Then
                                    Disallowed = Convert.ToDouble(Disallowed) + Convert.ToDouble(rst.Fields("F13"))
                                Else
                                    Deductible = Convert.ToDouble(Deductible) + Convert.ToDouble(rst.Fields("F13"))
                                End If
                            End If
                            If IIf(rstSQL.Fields("fldCoInsAmountYN") = "Y", True, False) Then Coinsurance = Coinsurance + Convert.ToDouble(rst.Fields("F13"))
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 4 Then fldDenTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And rstSQL.Fields("fldBillStatusID") = 21 Then fldNoteTxTypeID = rstSQL.Fields("fldTxTypeID")
                            If IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And Trim(ErrorMessage) = "" And rstSQL.Fields("fldRemittanceErrorDescription") > "" Then
                                ErrorMessage = rstSQL.Fields("fldRemittanceErrorDescription")
                            ElseIf IIf(rstSQL.Fields("fldPostErrorYN") = "Y", True, False) And ErrMsg > "" Then
                                ErrorMessage = ErrorMessage + "; " + rstSQL.Fields("fldRemittanceErrorDescription")
                            End If
                        End If
                        rstSQL = Nothing
                    End If
            End Select
            If (PayorID = "86916" And Payment <> 0 And (Reason5 = "CO144" Or Reason5 = "OAA2")) Then
                Disallowed = Charge - Payment
            End If
        End If
    End Function

    Function FindCheck(ByVal strCheckNumber As String, _
                   ByVal dteCheckDate As Date, _
                   ByVal curCheckAmount As Decimal, _
                   ByVal fldInsuranceID As Long, _
                   ByVal fldProviderID As Long, _
                   ByVal fldGroupID As Long) As Long

        Dim objSQL As ADODB.Connection
        objSQL = CreateObject("ADODB.Connection")
        objSQL.Open(CONST_PSYQUEL_CNN)
        Dim rstSQL As ADODB.Recordset
        Dim strSQL, strData, Posted As String
        Dim PayerCheckID As Long
        Dim ReceiveDate As Date
        Dim objCheck As PostingBz.CCheckBz

        If curCheckAmount > 0 Then
            strSQL = "SELECT * FROM tblPayerCheck WHERE (fldCheckNum = '" & strCheckNumber & "' AND fldTotalAmount > 0 AND fldTotalAmount = " & curCheckAmount & " AND Format(fldCheckDate, 'MM/dd/yyyy') = '" & Format(dteCheckDate, "mm/dd/yyyy") & "')"
            rstSQL = CreateObject("ADODB.Recordset")
            rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
            PayerCheckID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldCheckID"))
            If PayerCheckID = 0 Then
                strSQL = "SELECT * FROM tblRemittance WHERE (CheckNumber = '" & strCheckNumber & "' AND CheckAmount > 0 AND CheckAmount = " & curCheckAmount & " AND Format(CheckDate, 'MM/dd/yyyy') = '" & Format(dteCheckDate, "mm/dd/yyyy") & "')"
                rstSQL = CreateObject("ADODB.Recordset")
                rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                PayerCheckID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldCheckID"))
                If PayerCheckID = 0 Then
                    strSQL = "SELECT * FROM tblPayerCheck WHERE (fldCheckNum = '" & strCheckNumber & "' AND fldTotalAmount > 0 AND fldTotalAmount = " & curCheckAmount & " AND (fldCheckDate > GETDATE() - 90))"
                Else
                    strSQL = "SELECT * FROM tblPayerCheck WHERE (fldCheckID = " & PayerCheckID & ")"
                End If
            End If
            rstSQL = Nothing
        Else
            strSQL = "SELECT fldCheckID = isdbnull(Min(fldCheckID),0) FROM tblPayerCheck WHERE (fldCheckNum = '" & strCheckNumber & "' AND fldTotalAmount = " & curCheckAmount & " AND Format(fldCheckDate, 'MM/dd/yyyy') = '" & Format(dteCheckDate, "mm/dd/yyyy") & "') AND fldInsuranceID = " & fldInsuranceID & " AND (fldCheckID > " & PayerCheckID - 2000 & ")"
            rstSQL = CreateObject("ADODB.Recordset")
            rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
            PayerCheckID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldCheckID"))
            rstSQL = Nothing
            If PayerCheckID = 0 Then
                strSQL = "SELECT * FROM tblRemittance WHERE (CheckNumber = '" & strCheckNumber & "' AND CheckAmount = 0 AND CheckAmount = " & curCheckAmount & " AND Format(CheckDate, 'MM/dd/yyyy') = '" & Format(dteCheckDate, "mm/dd/yyyy") & "')"
                rstSQL = CreateObject("ADODB.Recordset")
                rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                PayerCheckID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldCheckID"))
                If PayerCheckID = 0 Then
                    strSQL = "SELECT * FROM tblPayerCheck WHERE (fldCheckNum = '" & strCheckNumber & "' AND fldTotalAmount > 0 AND fldTotalAmount = " & curCheckAmount & " AND (fldCheckDate > GETDATE() - 90))"
                Else
                    strSQL = "SELECT * FROM tblPayerCheck WHERE (fldCheckID = " & PayerCheckID & ")"
                End If
            Else
                strSQL = "SELECT * FROM tblPayerCheck WHERE (fldCheckID = " & PayerCheckID & ")"
            End If
        End If

        rstSQL = CreateObject("ADODB.Recordset")
        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
        PayerCheckID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldCheckID"))
        If fldProviderID <= 0 Then fldProviderID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldProviderID"))
        If fldGroupID <= 0 Then fldGroupID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldGroupID"))
        ReceiveDate = IIf(rstSQL.EOF, Now(), IIf(IsDBNull(rstSQL.Fields("fldReceiveDate")), Now(), rstSQL.Fields("fldReceiveDate")))
        Posted = IIf(rstSQL.EOF, "N", rstSQL.Fields("fldPostedYN"))
        rstSQL = Nothing

        strData = "Electronic EOB"
        If fldProviderID > 0 Then
            rstSQL = Nothing
            strSQL = "SELECT fldSubmissionOnlyYN FROM tblProvider WHERE (fldProviderID = " & fldProviderID & ")"
            rstSQL = CreateObject("ADODB.Recordset")
            rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
            If IIf(rstSQL.EOF, "N", rstSQL.Fields("fldSubmissionOnlyYN")) = "Y" Then
                strData = "Electronic EOB - Provider Responsible"
            End If
            rstSQL = Nothing
        End If

        rstSQL = Nothing
        objCheck = CreateObject("PostingBz.CCheckBz")
        If PayerCheckID > 0 Then
            If fldProviderID <= 0 Then
                strSQL = "SELECT TOP 1 * FROM tblPayerCheckLog WHERE (fldCheckID = " & PayerCheckID & " AND fldInsuranceID = " & fldInsuranceID & " AND fldProviderID > 0)"
                rstSQL = CreateObject("ADODB.Recordset")
                rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                fldProviderID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldProviderID"))
            End If
            If fldGroupID <= 0 Or fldGroupID = 680 Or IsDBNull(fldGroupID) Then
                fldGroupID = 680
                objCheck.Update(PayerCheckID, fldInsuranceID, strCheckNumber, "000", CDate(dteCheckDate), CDate(ReceiveDate), Convert.ToDecimal(curCheckAmount), fldProviderID, fldGroupID, IIf(Posted = "Y", True, False), Convert.ToString(strData), True)
            Else
                objCheck.Update(PayerCheckID, fldInsuranceID, strCheckNumber, "000", CDate(dteCheckDate), CDate(ReceiveDate), Convert.ToDecimal(curCheckAmount), -1, fldGroupID, IIf(Posted = "Y", True, False), Convert.ToString(strData), True)
            End If
        End If
        objCheck = Nothing

        FindCheck = PayerCheckID


    End Function

    Function FindPatient(ByVal PatientID As String, _
                   ByVal fldProviderID As Long, _
                   ByVal fldGroupID As Long, _
                   ByVal intMaxPatID As Long, _
                   ByVal fldOwnerID As Long _
                   ) As Long

        Dim fldPatientID As Long
        Dim fldEncounterLogID As Long
        'Dim fldOwnerID As Long
        Dim objSQL As ADODB.Connection
        objSQL = CreateObject("ADODB.Connection")
        objSQL.Open(CONST_PSYQUEL_CNN)
        Dim rstSQL As ADODB.Recordset
        Dim strSQL As String

        fldPatientID = 0
        fldEncounterLogID = 0
        If CLng(Left(PatientID, 9)) > 0 Then
            If fldProviderID <= 0 And CLng(Left(PatientID, 9)) <= intMaxPatID Then
                strSQL = "SELECT fldBenefactorID, fldOwnerID FROM tblBenefactor WHERE (fldBenefactorID = " & Trim(PatientID) & ")"
                rstSQL = CreateObject("ADODB.Recordset")
                rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                fldPatientID = IIf(rstSQL.EOF Or isdbnull(rstSQL.Fields("fldOwnerID")), 0, rstSQL.Fields("fldBenefactorID"))
                fldProviderID = IIf(rstSQL.EOF Or isdbnull(rstSQL.Fields("fldOwnerID")), 0, rstSQL.Fields("fldOwnerID"))
                fldOwnerID = IIf(rstSQL.EOF Or isdbnull(rstSQL.Fields("fldOwnerID")), 0, rstSQL.Fields("fldOwnerID"))
                rstSQL = Nothing
            End If
            If fldPatientID = 0 And fldProviderID > 0 And CLng(Left(PatientID, 9)) <= intMaxPatID Then
                strSQL = "SELECT fldBenefactorID, fldOwnerID FROM tblBenefactor WHERE (fldBenefactorID = " & Trim(PatientID) & " AND fldOwnerID = " & fldProviderID & ")"
                rstSQL = CreateObject("ADODB.Recordset")
                rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                fldPatientID = IIf(rstSQL.EOF Or isdbnull(rstSQL.Fields("fldOwnerID")), 0, rstSQL.Fields("fldBenefactorID"))
                fldOwnerID = IIf(rstSQL.EOF Or isdbnull(rstSQL.Fields("fldOwnerID")), 0, rstSQL.Fields("fldOwnerID"))
                rstSQL = Nothing
                '       ElseIf fldPatientID = 0 And fldProviderID > 0 And CLng(Left(PatientID, 9)) > intMaxPatID Then
                '          strSQL = "SELECT A.fldPatientID FROM tblEncounterLog AS A WHERE (A.fldEncounterLogID =" & CLng(Left(PatientID, 9)) & " AND A.fldProviderID = " & fldProviderID & ")"
                '          Set rstSQL = CreateObject("ADODB.Recordset")
                '         rstSQL.Open strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch
                '          fldPatientID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldPatientID"))
                '          fldOwnerID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldPatientID"))
                '          Set rstSQL = Nothing
            End If
            If fldPatientID = 0 And fldProviderID > 0 And CLng(PatientID) <= intMaxPatID Then
                strSQL = "SELECT fldBenefactorID, fldOwnerID FROM tblBenefactor INNER JOIN tblPatientProvider ON tblBenefactor.fldBenefactorID = tblPatientProvider.fldPatientID INNER JOIN tblProvider ON tblPatientProvider.fldProviderID = tblProvider.fldProviderID WHERE (fldBenefactorID = " & Trim(PatientID) & " AND tblProvider.fldGroupID = " & fldGroupID & " AND fldOwnerID <> " & fldProviderID & ")"
                rstSQL = CreateObject("ADODB.Recordset")
                rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                fldPatientID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldBenefactorID"))
                fldOwnerID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldOwnerID"))
                If fldOwnerID <> 0 Then fldProviderID = fldOwnerID
                rstSQL = Nothing
            End If
            If fldOwnerID = 0 And fldProviderID > 0 Then
                strSQL = "SELECT fldGroupID FROM tblProvider WHERE (fldProviderID = " & fldProviderID & ") AND fldDisabledYN = 'N'"
                rstSQL = CreateObject("ADODB.Recordset")
                rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                If fldGroupID <> 680 And fldGroupID = IIf(rstSQL.EOF, -1, rstSQL.Fields("fldGroupID")) Then
                    rstSQL = Nothing
                    strSQL = "SELECT fldBenefactorID, fldOwnerID " & _
                        "FROM tblBenefactor INNER JOIN " & _
                        "tblPatientProvider ON tblBenefactor.fldBenefactorID = tblPatientProvider.fldPatientID INNER JOIN " & _
                        "tblProvider ON tblPatientProvider.fldProviderID = tblProvider.fldProviderID " & _
                        "WHERE (tblBenefactor.fldBenefactorID = " & Trim(PatientID) & " AND tblProvider.fldGroupID = " & fldGroupID & " AND fldOwnerID IS NOT NULL )"
                    rstSQL = CreateObject("ADODB.Recordset")
                    rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                    fldPatientID = IIf(rstSQL.EOF, fldPatientID, rstSQL.Fields("fldBenefactorID"))
                    fldOwnerID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldOwnerID"))
                    rstSQL = Nothing
                    If fldOwnerID > 0 And fldProviderID <> fldOwnerID Then
                        strSQL = "SELECT fldGroupID FROM tblProvider WHERE (fldProviderID = " & fldOwnerID & ") AND fldDisabledYN = 'N'"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        If fldGroupID <> 680 And fldGroupID = IIf(rstSQL.EOF, -1, rstSQL.Fields("fldGroupID")) Then
                            fldProviderID = fldOwnerID
                        End If
                        rstSQL = Nothing
                    End If
                End If
                rstSQL = Nothing
                fldOwnerID = 0
            End If
        End If

        FindPatient = fldPatientID

    End Function
    Private Function FindEncounter(ByVal fldEncounterLogID As Long, _
                   ByVal fldPatientID As Long, _
                   ByVal fldProviderID As Long, _
                   ByVal fldGroupID As Long, _
                   ByVal fldOwnerID As Long, _
                   ByVal FirstDateOfService As Date, _
                   ByVal ProcedureCode As String, _
                   ByVal PatFirstName As String, _
                   ByVal PatLastName As String _
                   ) As Long
        Dim objSQL As ADODB.Connection
        objSQL = CreateObject("ADODB.Connection")
        objSQL.Open(CONST_PSYQUEL_CNN)
        Dim rstSQL As ADODB.Recordset
        Dim strSQL As String

        If fldEncounterLogID > 0 Then
            If IsDate(FirstDateOfService) And FirstDateOfService > 0 Then
                strSQL = "SELECT A.fldPatientID, A.fldEncounterLogID, A.fldProviderID FROM tblEncounterLog AS A WHERE (A.fldEncounterLogID =" & fldEncounterLogID & " AND A.fldDOS = CONVERT(DATETIME, '" & Format(FirstDateOfService, "yyyy-mm-dd") & " 00:00:00', 102)" & ")"
                rstSQL = CreateObject("ADODB.Recordset")
                rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                fldEncounterLogID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldEncounterLogID"))
                fldPatientID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldPatientID"))
                fldProviderID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldProviderID"))
                rstSQL = Nothing
            Else
                strSQL = "SELECT A.fldPatientID, A.fldEncounterLogID, A.fldProviderID FROM tblEncounterLog AS A WHERE (A.fldEncounterLogID =" & fldEncounterLogID & ")"
                rstSQL = CreateObject("ADODB.Recordset")
                rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                fldEncounterLogID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldEncounterLogID"))
                fldPatientID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldPatientID"))
                fldProviderID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldProviderID"))
                rstSQL = Nothing
            End If
        End If

        If fldEncounterLogID = 0 Then
            If IsDate(FirstDateOfService) Then
                If fldPatientID > 0 Then
                    If Not isdbnull(ProcedureCode) Then
                        strSQL = "SELECT fldPatientID, fldEncounterLogID, fldDOS FROM tblEncounterLog WHERE (fldPatientID =" & fldPatientID & " AND fldDOS = CONVERT(DATETIME, '" & Format(FirstDateOfService, "yyyy-mm-dd") & " 00:00:00', 102)  AND (fldCptCode = '" & ProcedureCode & "' or fldAddOnCPTCode = '" & ProcedureCode & "' or fldAddOnSecCPTCode = '" & ProcedureCode & "'))"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        fldEncounterLogID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldEncounterLogID"))
                        rstSQL = Nothing
                    End If
                    If fldEncounterLogID = 0 And fldProviderID > 0 Then
                        strSQL = "SELECT fldPatientID, fldEncounterLogID, fldDOS FROM tblEncounterLog WHERE (fldPatientID =" & fldPatientID & " AND fldDOS = CONVERT(DATETIME, '" & Format(FirstDateOfService, "yyyy-mm-dd") & " 00:00:00', 102)  AND fldProviderID = " & fldProviderID & ")"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        fldEncounterLogID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldEncounterLogID"))
                        rstSQL = Nothing
                    End If
                    If fldEncounterLogID = 0 Then
                        strSQL = "SELECT fldPatientID, fldEncounterLogID, fldDOS FROM tblEncounterLog WHERE (fldPatientID =" & fldPatientID & " AND fldDOS = CONVERT(DATETIME, '" & Format(FirstDateOfService, "yyyy-mm-dd") & " 00:00:00', 102))"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        fldEncounterLogID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldEncounterLogID"))
                        rstSQL = Nothing
                    End If
                    If fldEncounterLogID = 0 Then
                        strSQL = "SELECT A.fldPatientID, A.fldEncounterLogID, B.fldDOS FROM tblEncounterLog AS A INNER JOIN tblEncDetail AS B ON A.fldEncounterLogID = B.fldEncounterLogID WHERE (A.fldPatientID =" & fldPatientID & " AND B.fldDOS = CONVERT(DATETIME, '" & Format(FirstDateOfService, "yyyy-mm-dd") & " 00:00:00', 102))"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        fldEncounterLogID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldEncounterLogID"))
                        rstSQL = Nothing
                    End If
                Else
                    If fldProviderID > 0 Then
                        strSQL = "SELECT fldBenefactorID, fldOwnerID FROM tblBenefactor WHERE (fldlast ='" & Trim(Replace(PatLastName, "'", "")) & "' AND fldFirst ='" & Trim(Replace(PatFirstName, "-", " ")) & "' AND fldOwnerID = " & fldProviderID & ")"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        fldPatientID = IIf(rstSQL.EOF, fldPatientID, rstSQL.Fields("fldBenefactorID"))
                        fldOwnerID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldOwnerID"))
                        rstSQL = Nothing
                        If fldOwnerID = 0 Then
                            strSQL = "SELECT fldGroupID FROM tblProvider WHERE (fldProviderID = " & fldProviderID & ") AND fldDisabledYN = 'N'"
                            rstSQL = CreateObject("ADODB.Recordset")
                            rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                            If fldGroupID <> 680 And fldGroupID = IIf(rstSQL.EOF, -1, rstSQL.Fields("fldGroupID")) Then
                                rstSQL = Nothing
                                strSQL = "SELECT fldBenefactorID, fldOwnerID " & _
                                      "FROM tblBenefactor INNER JOIN " & _
                                      "tblPatientProvider ON tblBenefactor.fldBenefactorID = tblPatientProvider.fldPatientID INNER JOIN " & _
                                      "tblProvider ON tblPatientProvider.fldProviderID = tblProvider.fldProviderID " & _
                                      "WHERE (tblBenefactor.fldlast like '" & Trim(Replace(PatLastName, "'", "")) & "%' AND tblBenefactor.fldFirst like '" & Trim(Replace(PatFirstName, "-", " ")) & "%' AND tblProvider.fldGroupID = " & fldGroupID & " AND fldOwnerID IS NOT NULL )"
                                rstSQL = CreateObject("ADODB.Recordset")
                                rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                fldPatientID = IIf(rstSQL.EOF, fldPatientID, rstSQL.Fields("fldBenefactorID"))
                                fldOwnerID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldOwnerID"))
                                rstSQL = Nothing
                                If fldOwnerID > 0 And fldProviderID <> fldOwnerID Then
                                    strSQL = "SELECT fldGroupID FROM tblProvider WHERE (fldProviderID = " & fldOwnerID & ") AND fldDisabledYN = 'N'"
                                    rstSQL = CreateObject("ADODB.Recordset")
                                    rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                    If fldGroupID <> 680 And fldGroupID = IIf(rstSQL.EOF, -1, rstSQL.Fields("fldGroupID")) Then
                                        fldProviderID = fldOwnerID
                                    End If
                                    rstSQL = Nothing
                                    fldOwnerID = 0
                                End If
                            End If
                        End If
                    End If
                End If
                If fldEncounterLogID = 0 And fldPatientID > 0 And fldProviderID > 0 Then
                    strSQL = "SELECT fldPatientID, fldEncounterLogID, fldDOS FROM tblEncounterLog WHERE (fldPatientID =" & fldPatientID & " AND fldDOS = CONVERT(DATETIME, '" & Format(FirstDateOfService, "yyyy-mm-dd") & " 00:00:00', 102) AND fldProviderID = " & fldProviderID & ")"
                    rstSQL = CreateObject("ADODB.Recordset")
                    rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                    fldEncounterLogID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldEncounterLogID"))
                    rstSQL = Nothing
                End If
            End If
        End If

        FindEncounter = fldEncounterLogID

    End Function

    Private Function FindProvider(ByVal rst As Recordset, ByVal ProviderID As Long, ByVal fldEncounterLogID As Long, _
                   ByVal fldPatientID As Long, ByVal HealthPlan As Long) As Long

        Dim objSQL As ADODB.Connection
        objSQL = CreateObject("ADODB.Connection")
        objSQL.Open(CONST_PSYQUEL_CNN)
        Dim rstSQL As ADODB.Recordset
        Dim strSQL As String

        If rst.Fields("F1") = "NM1" Or rst.Fields("F1") = "N1" Then
            If (rst.Fields("F2") = "PE") Then
                If Trim(rst.Fields("F4")) = "FI" Or Trim(rst.Fields("F4")) = "TJ" Then     'Tax ID
                    If ProviderID <= 0 Then
                        strSQL = "SELECT fldProviderID FROM tblProviderTaxID WHERE (fldTaxID = '" & rst.Fields("F5") & "')"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        ProviderID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldProviderID"))
                        rstSQL = Nothing
                    End If
                    If ProviderID <= 0 Then
                        strSQL = "SELECT fldProviderID FROM tblProviderTaxID WHERE (fldTaxID = '" & Left(rst.Fields("F5"), 2) & "-" & Right(rst.Fields("F5"), 7) & "')"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        ProviderID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldProviderID"))
                        rstSQL = Nothing
                    End If
                    If ProviderID <= 0 Then
                        strSQL = "SELECT fldProviderID FROM tblProviderPracticeNumber WHERE (fldPracticeNumber = '" & rst.Fields("F5") & "' AND fldInsuranceID =" & HealthPlan & ")"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        ProviderID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldProviderID"))
                        rstSQL = Nothing
                    End If
                    If ProviderID <= 0 Then
                        strSQL = "SELECT fldUserID FROM tblUser WHERE (fldSSN = '" & rst.Fields("F5") & "')"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        ProviderID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldUserID"))
                        rstSQL = Nothing
                    End If
                End If
                If (ProviderID <= 0) Then
                    If Trim(rst.Fields("F4")) = "XX" Then     'National Provider Identification Number
                        If ProviderID <= 0 Then
                            strSQL = "SELECT P.fldProviderID, P.fldGroupID FROM tblProvider AS P INNER JOIN " & _
                                     "tblUser AS U ON U.fldUserID = P.fldProviderID WHERE (P.fldProviderNPI = '" & Replace(rst.Fields("F5"), "~", "") & "' AND U.fldDisabledYN = 'N')"
                            rstSQL = CreateObject("ADODB.Recordset")
                            rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                            ProviderID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldProviderID"))
                            rstSQL = Nothing
                        End If
                        If ProviderID <= 0 Then
                            '  strSQL = "SELECT P.fldProviderID, P.fldGroupID, G.fldGroupNPI, U.fldLastName, U.fldFirstName FROM (tblProvider AS P INNER JOIN tblProviderGroup AS G ON P.fldGroupID = G.fldGroupID) INNER JOIN dbo_tblUser AS U ON P.fldProviderID = U.fldUserID WHERE ((G.fldGroupNPI='" & Replace(rst.Fields("F5"), "~", "") & "') AND (U.fldLastName='dog') AND (U.fldFirstName='msn'));"
                            strSQL = "SELECT P.fldProviderID, P.fldGroupID FROM tblProvider AS P INNER JOIN " & _
                                     "tblUser AS U ON U.fldUserID = P.fldProviderID WHERE (P.fldProviderNPI = '" & Replace(rst.Fields("F5"), "~", "") & "' AND U.fldDisabledYN = 'Y')"
                            rstSQL = CreateObject("ADODB.Recordset")
                            rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                            ProviderID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldProviderID"))
                            rstSQL = Nothing
                        End If
                    End If
                End If
                If ProviderID <= 0 And fldPatientID > 0 Then
                    strSQL = "SELECT fldOwnerID FROM tblBenefactor WHERE (fldBenefactorID = " & fldPatientID & " AND fldOwnerID IS NOT NULL)"
                    rstSQL = CreateObject("ADODB.Recordset")
                    rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                    ProviderID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldOwnerID"))
                    rstSQL = Nothing
                End If
            End If
        End If

        If rst.Fields("F1") = "NM1" Then
            If rst.Fields("F2") = "82" And (rst.Fields("F3") = "1" Or rst.Fields("F3") = "2") Then
                If Trim(rst.Fields("F9")) = "XX" And ProviderID <= 0 Then     'Tax ID
                    strSQL = "SELECT P.fldProviderID, P.fldGroupID FROM tblProvider AS P INNER JOIN " & _
                             "tblUser AS U ON U.fldUserID = P.fldProviderID WHERE (P.fldProviderNPI = '" & Replace(rst.Fields("F10"), "~", "") & "' AND U.fldDisabledYN = 'N')"
                    rstSQL = CreateObject("ADODB.Recordset")
                    rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                    ProviderID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldProviderID"))
                    rstSQL = Nothing
                    If ProviderID <= 0 Then
                        strSQL = "SELECT P.fldProviderID, P.fldGroupID FROM tblProvider AS P INNER JOIN " & _
                           "tblUser AS U ON U.fldUserID = P.fldProviderID WHERE (P.fldProviderNPI = '" & Replace(rst.Fields("F10"), "~", "") & "' AND U.fldDisabledYN = 'Y')"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        ProviderID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldProviderID"))
                        rstSQL = Nothing
                    End If
                    If ProviderID <= 0 Then
                        strSQL = "SELECT P.fldProviderID, P.fldGroupID, G.fldGroupNPI, U.fldLastName, U.fldFirstName " & _
                           "FROM (tblProvider AS P INNER JOIN tblProviderGroup AS G ON P.fldGroupID = G.fldGroupID) INNER JOIN " & _
                           "tblUser AS U ON P.fldProviderID = U.fldUserID " & _
                           "WHERE ((G.fldGroupNPI='" & Replace(rst.Fields("F10"), "~", "") & "') AND " & _
                           "(U.fldLastName='" & rst.Fields("F4") & "') AND " & _
                           "(U.fldFirstName='" & rst.Fields("F5") & "'));"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        ProviderID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldProviderID"))
                        rstSQL = Nothing
                    End If
                ElseIf Trim(rst.Fields("F9")) = "FI" And ProviderID <= 0 Then     'Tax ID
                    strSQL = "SELECT fldProviderID FROM tblProviderTaxID WHERE (fldTaxID = '" & rst.Fields("F10") & "')"
                    rstSQL = CreateObject("ADODB.Recordset")
                    rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                    ProviderID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldProviderID"))
                    rstSQL = Nothing
                    If ProviderID <= 0 Then
                        strSQL = "SELECT fldProviderID FROM tblProviderTaxID WHERE (fldTaxID = '" & Left(rst.Fields("F10"), 2) & "-" & Right(rst.Fields("F10"), 7) & "')"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        ProviderID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldProviderID"))
                        rstSQL = Nothing
                    End If
                    If ProviderID <= 0 Then
                        strSQL = "SELECT fldProviderID FROM tblProviderPracticeNumber WHERE (fldPracticeNumber = '" & rst.Fields("F10") & "' AND fldInsuranceID =" & HealthPlan & ")"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        ProviderID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldProviderID"))
                        rstSQL = Nothing
                    End If
                    If ProviderID <= 0 Then
                        strSQL = "SELECT fldUserID FROM tblUser WHERE (fldSSN = '" & Replace(rst.Fields("F10"), "-", "") & "')"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        ProviderID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldUserID"))
                        rstSQL = Nothing
                    End If
                ElseIf ProviderID <= 0 Then
                    strSQL = "SELECT fldProviderID FROM tblProviderPracticeNumber WHERE (fldPracticeNumber = '" & rst.Fields("F10") & "' AND fldInsuranceID =" & HealthPlan & ")"
                    rstSQL = CreateObject("ADODB.Recordset")
                    rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                    ProviderID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldProviderID"))
                    rstSQL = Nothing
                End If
            End If
        End If



        If rst.Fields("F1") = "REF" Then
            If (rst.Fields("F2") = "1C" Or rst.Fields("F2") = "1D" Or rst.Fields("F2") = "1B" Or rst.Fields("F2") = "B3" _
               Or rst.Fields("F2") = "1H" Or rst.Fields("F2") = "G2" Or rst.Fields("F2") = "PQ" Or rst.Fields("F2") = "XX") Then
                If Trim(rst.Fields("F2")) = "XX" And ProviderID = 0 Then     'National Provider Identification Number
                    If ProviderID <= 0 Then
                        strSQL = "SELECT P.fldProviderID, P.fldGroupID FROM tblProvider AS P INNER JOIN " & _
                                    "tblUser AS U ON U.fldUserID = P.fldProviderID WHERE (P.fldProviderNPI = '" & Replace(rst.Fields("F3"), "~", "") & "' AND U.fldDisabledYN = 'N')"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        ProviderID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldProviderID"))
                        rstSQL = Nothing
                    End If
                    If ProviderID <= 0 Then
                        '  strSQL = "SELECT P.fldProviderID, P.fldGroupID, G.fldGroupNPI, U.fldLastName, U.fldFirstName FROM (tblProvider AS P INNER JOIN tblProviderGroup AS G ON P.fldGroupID = G.fldGroupID) INNER JOIN dbo_tblUser AS U ON P.fldProviderID = U.fldUserID WHERE ((G.fldGroupNPI='" & Replace(rst.Fields("F5"), "~", "") & "') AND (U.fldLastName='dog') AND (U.fldFirstName='msn'));"
                        strSQL = "SELECT P.fldProviderID, P.fldGroupID FROM tblProvider AS P INNER JOIN " & _
                                    "tblUser AS U ON U.fldUserID = P.fldProviderID WHERE (P.fldProviderNPI = '" & Replace(rst.Fields("F3"), "~", "") & "' AND U.fldDisabledYN = 'Y')"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        ProviderID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldProviderID"))
                        rstSQL = Nothing
                    End If

                End If
                If Trim(rst.Fields("F2")) = "PQ" Or rst.Fields("F2") = "1B" Or rst.Fields("F2") = "1D" Or rst.Fields("F2") = "1C" Then      'Provider Identification Number
                    If ProviderID <= 0 Then
                        strSQL = "SELECT fldProviderID FROM tblProviderPracticeNumber WHERE (fldPracticeNumber = '" & rst.Fields("F3") & "' AND fldInsuranceID =" & HealthPlan & ")"
                        rstSQL = CreateObject("ADODB.Recordset")
                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                        ProviderID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldProviderID"))
                        rstSQL = Nothing
                    End If
                End If
            End If

            If Trim(rst.Fields("F2")) = "FI" Or Trim(rst.Fields("F2")) = "TJ" Then     'Tax ID
                If ProviderID <= 0 Then
                    strSQL = "SELECT fldProviderID FROM tblProviderTaxID WHERE (fldTaxID = '" & rst.Fields("F3") & "')"
                    rstSQL = CreateObject("ADODB.Recordset")
                    rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                    ProviderID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldProviderID"))
                    rstSQL = Nothing
                End If
                If ProviderID <= 0 Then
                    strSQL = "SELECT fldProviderID FROM tblProviderTaxID WHERE (fldTaxID = '" & Left(rst.Fields("F3"), 2) & "-" & Right(rst.Fields("F3"), 7) & "')"
                    rstSQL = CreateObject("ADODB.Recordset")
                    rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                    ProviderID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldProviderID"))
                    rstSQL = Nothing
                End If
                If ProviderID <= 0 Then
                    strSQL = "SELECT fldProviderID FROM tblProviderPracticeNumber WHERE (fldPracticeNumber = '" & rst.Fields("F3") & "' AND fldInsuranceID =" & HealthPlan & ")"
                    rstSQL = CreateObject("ADODB.Recordset")
                    rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                    ProviderID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldProviderID"))
                    rstSQL = Nothing
                End If
                If ProviderID <= 0 Then
                    strSQL = "SELECT fldUserID FROM tblUser WHERE (fldSSN = '" & rst.Fields("F3") & "')"
                    rstSQL = CreateObject("ADODB.Recordset")
                    rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                    ProviderID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldUserID"))
                    rstSQL = Nothing
                End If
            End If
        End If


        If rst.Fields("F1") = "TS3" Then
            If Trim(rst.Fields("F3")) = "11" Then     'Facility Code
                strSQL = "SELECT fldProviderID, fldGroupID FROM tblProvider WHERE (fldProviderNPI = '" & rst.Fields("F2") & "')"
                rstSQL = CreateObject("ADODB.Recordset")
                rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                ProviderID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldProviderID"))
                rstSQL = Nothing
            End If
        End If

        FindProvider = ProviderID

    End Function

    Private Function FindHealthPlan(ByVal strSeg01 As String, ByVal strSeg02 As String, ByVal fldClearingHouseID As Long, ByVal PayorID As String, ByVal ReceiverID As String, ByVal HealthPlan As Long, ByVal HealthPlanName As String, ByVal fldEncounterLogID As Long) As Long

        Dim objSQL As ADODB.Connection
        objSQL = CreateObject("ADODB.Connection")
        objSQL.Open(CONST_PSYQUEL_CNN)
        Dim rstSQL As ADODB.Recordset
        Dim strSQL As String

        Dim cnn As ADODB.Connection
        Dim cmd As ADODB.Command

        'Update Error Messages
        'Instantiate and prepare the command object
        cmd = New ADODB.Command

        'Acquire the database connection.
        cnn = New ADODB.Connection

         cnn.Open(CONST_PSYQUEL_CNN)
        'Assign the connection to the Command object and execute the stored procedure
        cmd.ActiveConnection = cnn
        With cmd
            .CommandText = "uspFindHealthPlan"
            .CommandType = adCmdStoredProc
            .Parameters.Append.CreateParameter("@HealthPlanID", adInteger, adParamReturnValue)
            .Parameters.Append.CreateParameter("@Seg01", adVarChar, adParamInput, 5, Left(strSeg01, 5))
            .Parameters.Append.CreateParameter("@Seg02", adVarChar, adParamInput, 5, Left(strSeg02, 5))
            .Parameters.Append.CreateParameter("@ClearingHouseID", adInteger, adParamInput, , fldClearingHouseID)
            .Parameters.Append.CreateParameter("@PayorID", adVarChar, adParamInput, 32, PayorID)
            .Parameters.Append.CreateParameter("@ReceiverID", adVarChar, adParamInput, 64, ReceiverID)
            .Parameters.Append.CreateParameter("@InsuranceID", adInteger, adParamInput, , HealthPlan)
            .Parameters.Append.CreateParameter("@PlanName", adVarChar, adParamInput, 128, Left(HealthPlanName, 128))
            .Parameters.Append.CreateParameter("@EncID", adInteger, adParamInput, , fldEncounterLogID)
            .Parameters.Append.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
        End With
        cmd.Execute, , adExecuteNoRecords

        FindHealthPlan = cmd.Parameters(0).Value

        'Check the SQLErrorNum parameter before deciding to commit the transaction.
        If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
            GoTo ErrTrap
        End If
        'Close the connection
        cnn.Close()

        'Free all resources
        cmd = Nothing
        cnn = Nothing

        Exit Function
ErrTrap:
        Screen.MousePointer = vbDefault
         ShowError(Err)
    End Function

    Private Function FindPlanID(ByVal lngPatientID As Long, ByVal lngEncounterLogID As Long, ByVal lngInsuranceID As Long, _
                          ByVal lngCPTRecordID As Long, ByVal dteDOS As Date, ByVal strPolicyID As String, ByVal strPatFirst As String, ByVal strPatLast As String) As ADODB.Recordset

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim objSQL As ADODB.Connection
        Dim strSQL As String

        Dim strPolicyIDPart As String
        Dim blnUserWhere As Boolean
        Dim lngRPID, lngEncDetailID, lngInsID, lngPlanID, lngOrder As Long
        Dim strDisabledYN As String
        Dim curBalance, PatBalance As Decimal

        objSQL = CreateObject("ADODB.Connection")
        objSQL.Open(CONST_PSYQUEL_CNN)

        On Error GoTo ErrTrap

        If Len(strPolicyID) > 2 Then
            strPolicyIDPart = Mid(strPolicyID, 1, Len(strPolicyID) - 2)
        Else
            strPolicyID = ""
        End If
        lngPlanID = 0

        If lngPatientID = 0 Then
            strSQL = "SELECT B.fldPatientID " & _
                     "FROM tblBenefactor AS A INNER JOIN " & _
                     " tblPatRPPlan AS B ON A.fldBenefactorID = B.fldPatientID INNER JOIN  " & _
                     " tblPatRPPlanRule AS C ON B.fldPatRPPlanID = C.fldPatRPPlanID INNER JOIN " & _
                     " tblPlan AS D ON B.fldPlanID = D.fldPlanID INNER JOIN " & _
                     " tblClaimsProcCtr AS E ON D.fldCPCID = E.fldCPCID " & _
                     "WHERE ((C.fldCardNum LIKE '%" & strPolicyID & "%') OR (C.fldCardNum LIKE '%" & strPolicyIDPart & "%')) AND " & _
                     " ((A.fldLast LIKE '%" & strPatLast & "%') OR (A.fldFirst LIKE '%" & strPatLast & "%') OR (A.fldFirst LIKE '%" & strPatFirst & "%') OR (A.fldFirst LIKE '%" & strPatFirst & "%')) AND " & _
                     " (D.fldDisabledYN = 'N') AND (B.fldOrder > 0)"
            'Debug.Print strSQL
            rstSQL = CreateObject("ADODB.Recordset")
            rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
            lngPatientID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldPatientID"))
        End If

        'Does this patient have this Policy ID
        If lngPatientID > 0 And strPolicyID > "" And (strPatFirst > "" Or strPatLast > "") Then
            strSQL = "SELECT B.fldRPID, B.fldPlanID, B.fldOrder, E.fldInsuranceID, D.fldDisabledYN " & _
                     "FROM tblBenefactor AS A INNER JOIN " & _
                     " tblPatRPPlan AS B ON A.fldBenefactorID = B.fldPatientID INNER JOIN " & _
                     " tblPatRPPlanRule AS C ON B.fldPatRPPlanID = C.fldPatRPPlanID INNER JOIN " & _
                     " tblPlan AS D ON B.fldPlanID = D.fldPlanID INNER JOIN " & _
                     " tblClaimsProcCtr AS E ON D.fldCPCID = E.fldCPCID " & _
                     "WHERE " & _
                     " (B.fldPatientID = " & lngPatientID & ") AND ((C.fldCardNum LIKE '%" & strPolicyID & "%') OR (C.fldCardNum LIKE '%" & Mid(strPolicyID, 1, Len(strPolicyID) - 2) & "%')) AND " & _
                     " ((A.fldLast LIKE '%" & strPatLast & "%') OR (A.fldFirst LIKE '%" & strPatLast & "%') OR (A.fldFirst LIKE '%" & strPatFirst & "%') OR (A.fldFirst LIKE '%" & strPatFirst & "%')) AND " & _
                     " (D.fldDisabledYN = 'N') AND (B.fldOrder > 0)"
            rstSQL = CreateObject("ADODB.Recordset")
            rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
            lngRPID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldRPID"))
            lngInsID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldInsuranceID"))
            lngPlanID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldPlanID"))
            lngOrder = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldOrder"))
            strDisabledYN = IIf(rstSQL.EOF, "", rstSQL.Fields("fldDisabledYN"))
            rstSQL = Nothing
        End If

        If lngPlanID = 0 And lngPatientID > 0 And strPolicyID > "" And (strPatFirst > "" Or strPatLast > "") Then
            strSQL = "SELECT B.fldRPID, B.fldPlanID, B.fldOrder, E.fldInsuranceID, D.fldDisabledYN " & _
                     "FROM tblBenefactor AS A INNER JOIN " & _
                     " tblPatRPPlan AS B ON A.fldBenefactorID = B.fldPatientID INNER JOIN " & _
                     " tblPatRPPlanRule AS C ON B.fldPatRPPlanID = C.fldPatRPPlanID INNER JOIN " & _
                     " tblPlan AS D ON B.fldPlanID = D.fldPlanID INNER JOIN " & _
                     " tblClaimsProcCtr AS E ON D.fldCPCID = E.fldCPCID " & _
                     "WHERE " & _
                     " (B.fldPatientID = " & lngPatientID & ") AND ((C.fldCardNum LIKE '%" & strPolicyID & "%') OR (C.fldCardNum LIKE '%" & Mid(strPolicyID, 1, Len(strPolicyID) - 2) & "%')) AND " & _
                     " (D.fldDisabledYN = 'N') AND (B.fldOrder > 0)"
            rstSQL = CreateObject("ADODB.Recordset")
            rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
            lngRPID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldRPID"))
            lngInsID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldInsuranceID"))
            lngPlanID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldPlanID"))
            lngOrder = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldOrder"))
            strDisabledYN = IIf(rstSQL.EOF, "", rstSQL.Fields("fldDisabledYN"))
            rstSQL = Nothing
        End If

        If lngPlanID = 0 And lngPatientID > 0 And strPolicyID > "" And (strPatFirst > "" Or strPatLast > "") Then
            strSQL = "SELECT C.fldRPID, C.fldPlanID, C.fldOrder, F.fldInsuranceID, E.fldDisabledYN " & _
                     "FROM tblBenefactor AS A INNER JOIN " & _
                     " tblEncounterLog AS B ON A.fldBenefactorID = B.fldPatientID INNER JOIN " & _
                     " tblPatRPPlan AS C ON A.fldBenefactorID = C.fldPatientID INNER JOIN " & _
                     " tblPatRPPlanRule AS D ON C.fldPatRPPlanID = D.fldPatRPPlanID INNER JOIN " & _
                     " tblPlan AS E ON C.fldPlanID = E.fldPlanID INNER JOIN " & _
                     " tblClaimsProcCtr AS F ON E.fldCPCID = F.fldCPCID " & _
                     "WHERE " & _
                     " (B.fldPatientID = " & lngPatientID & ") AND ((D.fldCardNum > '')) AND " & _
                     " (B.fldEncounterLogID =" & lngEncounterLogID & ") AND " & _
                     " ((A.fldLast LIKE '%" & strPatLast & "%') OR (A.fldFirst LIKE '%" & strPatLast & "%') OR (A.fldFirst LIKE '%" & strPatFirst & "%') OR (A.fldFirst LIKE '%" & strPatFirst & "%')) AND " & _
                     " (C.fldDisabledYN = 'N') AND (C.fldOrder > 0)"
            '   Debug.Print strSQL
            rstSQL = CreateObject("ADODB.Recordset")
            rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
            lngRPID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldRPID"))
            lngInsID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldInsuranceID"))
            lngPlanID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldPlanID"))
            lngOrder = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldOrder"))
            strDisabledYN = IIf(rstSQL.EOF, "", rstSQL.Fields("fldDisabledYN"))
            rstSQL = Nothing
        End If
        If lngPlanID = 0 And lngPatientID > 0 And (strPatFirst > "" Or strPatLast > "") Then
            strSQL = "SELECT B.fldRPID, B.fldPlanID, B.fldOrder, E.fldInsuranceID, B.fldDisabledYN " & _
                     "FROM tblBenefactor AS A INNER JOIN " & _
                     " tblPatRPPlan AS B ON A.fldBenefactorID = B.fldPatientID INNER JOIN " & _
                     " tblPatRPPlanRule AS C ON B.fldPatRPPlanID = C.fldPatRPPlanID INNER JOIN " & _
                     " tblPlan AS D ON B.fldPlanID = D.fldPlanID INNER JOIN " & _
                     " tblClaimsProcCtr AS E ON D.fldCPCID = E.fldCPCID " & _
                     "WHERE " & _
                     " (B.fldPatientID = " & lngPatientID & ") AND E.fldInsuranceID = " & lngInsuranceID & " AND " & _
                     " ((A.fldLast LIKE '%" & strPatLast & "%') OR (A.fldFirst LIKE '%" & strPatLast & "%') OR (A.fldFirst LIKE '%" & strPatFirst & "%') OR (A.fldFirst LIKE '%" & strPatFirst & "%')) AND " & _
                     " (B.fldDisabledYN = 'N') AND (B.fldOrder > 0)"
            Debug.Print(strSQL)
            rstSQL = CreateObject("ADODB.Recordset")
            rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
            lngRPID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldRPID"))
            lngInsID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldInsuranceID"))
            lngPlanID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldPlanID"))
            lngOrder = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldOrder"))
            strDisabledYN = IIf(rstSQL.EOF, "", rstSQL.Fields("fldDisabledYN"))
            rstSQL = Nothing
        End If
        If lngPlanID = 0 And lngPatientID > 0 And (strPatFirst > "" Or strPatLast > "") Then
            strSQL = "SELECT B.fldRPID, B.fldPlanID, B.fldOrder, E.fldInsuranceID, B.fldDisabledYN " & _
                     "FROM tblBenefactor AS A INNER JOIN " & _
                     " tblPatRPPlan AS B ON A.fldBenefactorID = B.fldPatientID INNER JOIN " & _
                     " tblPatRPPlanRule AS C ON B.fldPatRPPlanID = C.fldPatRPPlanID INNER JOIN " & _
                     " tblPlan AS D ON B.fldPlanID = D.fldPlanID INNER JOIN " & _
                     " tblClaimsProcCtr AS E ON D.fldCPCID = E.fldCPCID " & _
                     "WHERE " & _
                     " (B.fldPatientID = " & lngPatientID & ") AND E.fldInsuranceID = " & lngInsuranceID & " AND " & _
                     " ((A.fldLast LIKE '%" & strPatLast & "%') OR (A.fldFirst LIKE '%" & strPatLast & "%') OR (A.fldFirst LIKE '%" & strPatFirst & "%') OR (A.fldFirst LIKE '%" & strPatFirst & "%')) AND " & _
                     " (B.fldDisabledYN = 'Y') AND (B.fldOrder < 0) AND (B.fldDateDisabled >= '" & dteDOS & "') "
            Debug.Print(strSQL)
            rstSQL = CreateObject("ADODB.Recordset")
            rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
            lngRPID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldRPID"))
            lngInsID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldInsuranceID"))
            lngPlanID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldPlanID"))
            lngOrder = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldOrder"))
            strDisabledYN = IIf(rstSQL.EOF, "", rstSQL.Fields("fldDisabledYN"))
            rstSQL = Nothing
        End If
        If lngPlanID > 0 And lngPatientID > 0 And lngInsID > 0 And lngInsID <> lngInsuranceID And (strPatFirst > "" Or strPatLast > "") Then
            strSQL = "SELECT B.fldRPID, B.fldPlanID, B.fldOrder, E.fldInsuranceID, B.fldDisabledYN " & _
                     "FROM tblBenefactor AS A INNER JOIN " & _
                     " tblPatRPPlan AS B ON A.fldBenefactorID = B.fldPatientID INNER JOIN " & _
                     " tblPatRPPlanRule AS C ON B.fldPatRPPlanID = C.fldPatRPPlanID INNER JOIN " & _
                     " tblPlan AS D ON B.fldPlanID = D.fldPlanID INNER JOIN " & _
                     " tblClaimsProcCtr AS E ON D.fldCPCID = E.fldCPCID " & _
                     "WHERE " & _
                     " (B.fldPatientID = " & lngPatientID & ") AND E.fldInsuranceID = " & lngInsuranceID & " AND " & _
                     " ((A.fldLast LIKE '%" & strPatLast & "%') OR (A.fldFirst LIKE '%" & strPatLast & "%') OR (A.fldFirst LIKE '%" & strPatFirst & "%') OR (A.fldFirst LIKE '%" & strPatFirst & "%')) AND " & _
                     " (B.fldDisabledYN = 'N') AND (B.fldOrder > 0) "
            Debug.Print(strSQL)
            rstSQL = CreateObject("ADODB.Recordset")
            rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
            If Not rstSQL.EOF Then
                lngRPID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldRPID"))
                lngInsID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldInsuranceID"))
                lngPlanID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldPlanID"))
                lngOrder = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldOrder"))
                strDisabledYN = IIf(rstSQL.EOF, "", rstSQL.Fields("fldDisabledYN"))
            End If
            rstSQL = Nothing
        End If
        If lngPlanID > 0 Then
            If lngEncounterLogID > 0 Then
                strSQL = "SELECT B.fldPatientID, B.fldRPID, B.fldPlanID, fldOrder = B.fldOrder-1, B.fldInsuranceID, B.fldBalance, A.fldRPBal, A.fldPlanBal " & _
                      "FROM tblEncounterLog AS A INNER JOIN " & _
                      "tblBillingResponsibility AS B ON A.fldEncounterLogID = B.fldEncounterLogID " & _
                      "WHERE (B.fldEncounterLogID =" & lngEncounterLogID & ") AND " & _
                      " (B.fldPatientID =" & lngPatientID & ") AND " & _
                      " (B.fldInsuranceID = " & lngInsID & ") AND " & _
                      " (B.fldOrder " & IIf(lngOrder < 0, " > 0 ", " = " & lngOrder + 1) & ") AND (B.fldPayerCode = 'I')"
                rstSQL = CreateObject("ADODB.Recordset")
                rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                If rstSQL.EOF Then
                    strSQL = "SELECT B.fldPatientID, B.fldRPID, B.fldPlanID, fldOrder = B.fldOrder-1, B.fldInsuranceID, B.fldBalance, A.fldRPBal, A.fldPlanBal " & _
                       "FROM tblEncounterLog AS A INNER JOIN " & _
                       "tblBillingResponsibility AS B ON A.fldEncounterLogID = B.fldEncounterLogID " & _
                       "WHERE (B.fldEncounterLogID =" & lngEncounterLogID & ") AND " & _
                       " (B.fldPatientID =" & lngPatientID & ") AND " & _
                       " (B.fldInsuranceID = " & lngInsID & ") AND " & _
                       " (B.fldOrder > 1) AND (B.fldPayerCode = 'I')"
                End If
                rstSQL = Nothing
            Else
                strSQL = "SELECT A.fldPatientID, B.fldRPID, A.fldEncounterLogID, B.fldPlanID, fldOrder = B.fldOrder-1, B.fldInsuranceID, B.fldBalance, A.fldRPBal, A.fldPlanBal " & _
                      "FROM tblEncounterLog AS A INNER JOIN " & _
                      " tblBillingResponsibility AS B ON A.fldEncounterLogID = B.fldEncounterLogID " & _
                      "WHERE (A.fldPatientID =" & lngPatientID & " AND B.fldInsuranceID = " & lngInsuranceID & " AND A.flddos = '" & dteDOS & "' AND B.fldPayerCode = 'I')"
                Debug.Print(strSQL)
                rstSQL = CreateObject("ADODB.Recordset")
                rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                If rstSQL.EOF Then
                    strSQL = "SELECT fldPatientID = fldBenefactorID, fldRPID = " & lngRPID & ", fldEncounterLogID = 0, fldPlanID = " & lngPlanID & ", fldOrder = 1, fldInsuranceID = " & lngInsuranceID & ", fldBalance =0, fldRPBal=0, fldPlanBal=0 FROM tblBenefactor WHERE (fldBenefactorID =" & lngPatientID & ")"
                End If
            End If
        End If
        ' Debug.Print strSQL

        'Instantiate the Recordset.
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = adUseClient

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
         cnnSQL.Open(CONST_PSYQUEL_CNN)

        'Populate the recordset
         rstSQL.Open(strSQL, cnnSQL, adOpenForwardOnly, adLockReadOnly, adCmdText + adAsyncFetch)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing
        FindPlanID = rstSQL
        cnnSQL.Close()
        cnnSQL = Nothing


        'Signal successful completion
        'GetObjectContext.SetComplete

        Exit Function

ErrTrap:
        'Signal incompletion and raise the error to the ing environment.
        GetObjectContext.SetAbort()
        FindPlanID = rstSQL
        cnnSQL = Nothing
        Err.Raise(Err.Number, Err.Source, Err.Description)

    End Function


End Module