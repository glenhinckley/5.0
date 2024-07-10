

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

Public Class EDI_5010_271_005010X279A1

    Private _ConnectionString As String = String.Empty
    Private _AppPath As String = String.Empty
    Private _EDIOutput As String = String.Empty
    Private _MachineName As String = String.Empty
    Private _UserLoginName As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property



    Private _DB As New dbStuff.db
    Private _MD As New dbStuff.ModCommon

    Dim ErrorMessage As String
    Dim strSystemErr As String


    Function Translate271(ByVal strDir As String, ByVal inFile As String, ByVal curFile As String, ByVal strFile As String, ByVal fldClearingHouseID As Integer)

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

        'Open inFile For Input As #1    ' Open file.
        '        Do While Not EOF(1)    ' Loop until end of file.
        '      MyChar = Input(1, #1)    ' Get one character.
        '            'Debug.Print Asc(MyChar)    ' Print to the Immediate window.

        If Asc(MyChar) = 10 And Cnt = 105 And Mid(strData, 1, 3) = "ISA" Then
            strSegmentTerminator = MyChar
        End If
        If (Asc(MyChar) = 10 Or Asc(MyChar) = 13) And strSegmentTerminator <> MyChar And Mid(strData, 1, 2) = "ST" Then
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
        '  Loop
        'Close #1    ' Close file.
        '        f.Close()

        'Open (strDir & "\Schema.ini") For Output As #2
        'Print #2, "[" & strFile & ".txt]"
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
    Function ImportBenefitFiles(ByVal strDir As String, ByVal inFile As String, ByVal curFile As String, ByVal strFile As String, ByVal fldClearingHouseID As Integer)

        Dim blnReject, blnReverify, blnVerify As Boolean
        Dim strData As Object
        Dim Msg, Style, Title As String
        Dim intID As String
        Dim ReceiverID, strHLtype As String
        Dim strPayorID, strReceive, HealthPlanName, BatchNumber, LineNbr, ErrMsg, PatFirstName, PatLastName As String
        Dim ReceiveDate, CreationDate, EffDate, TermDate, ProcessDate, PatientDOB As Date
        Dim lngCtr As Integer
        Dim curAmt, Allowed, Disallow, AmountPaid, curBalance, PatBalance As Decimal
        Dim strID, strSQL, strSQL1, RecID As String
        Dim RecordID, GroupID, lngProviderID, lngInsuranceID, lngPatientID, fldOwnerID As Long
        Dim lngPlanID, fldOrder As Long
        Dim MedicalRecordNumber, SupplementalInsurerName1, SupplementalInsurerID1, PatientNumber As String
        Dim InsuredPolicyID, InsuredGroupNbr, InsuredGroupName, InsuredLastName, InsuredFirstName, InsuredMiddleInitial As String
        Dim PatientPolicyID, PatientGroupNbr, PatientGroupName, PatientLastName, PatientFirstName, PatientMiddleInitial, PatientSex As String
        Dim MessageCode1, MessageCode2, MessageCode3, MessageCode4, MessageCode5, Posted As String
        Dim LineNumber, ServiceLineNumber, Units As Long
        Dim PlaceOfService, TypeOfService As String
        Dim arr As Object

        ErrorMessage = "" : PatientDOB = 0
        MedicalRecordNumber = "" : SupplementalInsurerName1 = "" : SupplementalInsurerID1 = "" : PatientNumber = ""

        Dim fs, f
        Const ForReading = 1, ForWriting = 2, ForAppending = 8
        strData = "" : strSystemErr = ""
        fs = CreateObject("Scripting.FileSystemObject")
        'fldClearingHouseID = 51
        Translate271(strDir, inFile, curFile, strFile, fldClearingHouseID)


        Dim objConn As New ADODB.Connection
        Dim rst As New ADODB.Recordset
        Dim cnnSQL As New ADODB.Connection
        Dim cmdSQL As New ADODB.Command
        Dim objSQL As New ADODB.Connection
        Dim rstSQL As New ADODB.Recordset

        Dim rstTx As New ADODB.Recordset
        Dim rstMsg As New ADODB.Recordset
        Dim objBenefit As New BenefactorBz.CPatRPPlanBz
        Dim objPatBenefit As New BenefitsBz.CPatientBenefitBZ
        Dim objMsg As New ListBZ.CMsgBz
        Dim objUser As New ClinicBZ.CUserBz


        '************************************************************************************************************************************************
        'this non sesne is to get a directory list
        'Set the database connection
        objConn.Open("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & strDir & ";Extended Properties='text';")
        '                   "Extended Properties='text;HDR=YES;FMT=Delimited(*)';"

        'Query from the file assuming it as a database table
        rst.Open("SELECT * FROM [" & strFile & ".txt]", _
                  objConn, adOpenStatic)  ', adLockOptimistic, adCmdText
        '***********************************************************************************************************************************************
        '''877 220 7035


        'NO FILES QUEUED
        If rst.EOF Then
            ' Title = "Invalid Benefit File"
            ' intID = MsgBox("Invalid Benefit File Format " & strFile & ", ISA Record Missing!", vbOKOnly)
            rst.Close()
            'delete Benefit file from 271 Folder
            If Dir(inFile) > "" Then
                If Dir(Mid(inFile, 1, InStr(1, inFile, "\271\") + 4) & "Archive\" & Mid(inFile, InStr(1, inFile, "\271\") + 5)) > "" Then
                    fs.DeleteFile(Mid(inFile, 1, InStr(1, inFile, "\271\") + 4) & "Archive\" & Mid(inFile, InStr(1, inFile, "\271\") + 5))
                End If
                fs.MoveFile(inFile, Mid(inFile, 1, InStr(1, inFile, "\271\") + 4) & "Archive\" & Mid(inFile, InStr(1, inFile, "\271\") + 5))
            End If
            Exit Function
        End If   'delete remittance file from ElectronicClaims Folder

        If Convert.ToString(rst.Fields.Item("F1")) <> "ISA" Then
            ' Title = "Invalid Remittance File"
            ' intID = MsgBox("Invalid Remittance File Format " & strFile & ", ISA Record Missing!", vbOKOnly)
            rst.Close()
            'delete remittance file from ElectronicClaims Folder
            If Dir(inFile) > "" Then
                If Dir(Mid(inFile, 1, InStr(1, inFile, "\271\") + 4) & "Archive\" & Mid(inFile, InStr(1, inFile, "\271\") + 5)) > "" Then
                    fs.DeleteFile(Mid(inFile, 1, InStr(1, inFile, "\271\") + 4) & "Archive\" & Mid(inFile, InStr(1, inFile, "\271\") + 5))
                End If
                fs.MoveFile(inFile, Mid(inFile, 1, InStr(1, inFile, "\271\") + 4) & "Archive\" & Mid(inFile, InStr(1, inFile, "\271\") + 5))
            End If
            Exit Function
        End If   'delete remittance file from ElectronicClaims Folder

        objSQL = CreateObject("ADODB.Connection")
        objSQL.Open(_ConnectionString)


        Do While Not rst.EOF
            RecID = Convert.ToString(rst.Fields.Item("F1"))
            If (RecID = "ISA") Or (RecID = "SE") Or (RecID = "GE") Or (RecID = "ST") Then
                If (RecID = "SE") Or (RecID = "GE") Then
                    rst.MoveNext()
                    If IsDBNull(Convert.ToString(rst.Fields.Item("F1"))) Then rst.MoveNext()
                    RecID = Convert.ToString(rst.Fields("F1"))
                End If
                If (RecID = "IEA") Then
                    rst.MoveNext()
                    If IsDBNull(Convert.ToString(rst.Fields.Item("F1"))) Then rst.MoveNext()
                End If
                Do While (RecID <> "HL") And (RecID <> "SE") And Not rst.EOF
                    Select Case RecID
                        Case "ST"
                            lngProviderID = 0 : lngPatientID = 0 : blnReverify = False : blnVerify = False : blnReject = False : strHLtype = ""
                            If Convert.ToString(rst.Fields("F2")) <> "271" Then Exit Function
                            strData = "Electronic Verification received on " & ReceiveDate & "." & Chr(10)
                            strReceive = "Electronic Verification received on " & ReceiveDate & "." & Chr(10)
                        Case "ISA"
                            GroupID = 0
                            CreationDate = DateSerial(CInt(Mid(Convert.ToString(rst.Fields("F10")), 1, 2)) + 2000, CInt(Mid(Convert.ToString(rst.Fields("F10"), 3, 2))), CInt(Mid(Convert.ToString(rst.Fields("F10"), 5, 2))))
                            strPayorID = Trim(Convert.ToString(rst.Fields("F7")))
                            ReceiverID = IIf(IsDBNull(Convert.ToString(rst.Fields("F9")), strPayorID, Trim(Convert.ToString(rst.Fields("F9")))))
                        Case "GS"
                            If (Convert.ToString(rst.Fields("F2")) = "HP")) Or (Convert.ToString(rst.Fields("F2")) = "HB")) Then
                                BatchNumber = Convert.ToString(rst.Fields("F7"))

                                If Len(Convert.ToString(rst.Fields("F5")))) = 6 Then
                                    ReceiveDate = DateSerial(CInt(Mid(Convert.ToString(rst.Fields("F5"), 1, 2))) + 2000, CInt(Mid(Convert.ToString(rst.Fields("F5"), 3, 2))), CInt(Mid(Convert.ToString(rst.Fields("F5"), 5, 2))))
                                Else
                                    ReceiveDate = DateSerial(CInt(Mid(Convert.ToString(rst.Fields("F5"), 1, 4))), CInt(Mid(Convert.ToString(rst.Fields("F5"), 5, 2))), CInt(Mid(Convert.ToString(rst.Fields("F5"), 7, 2))))
                                End If
                            End If
                        Case "N1"
                        Case "N3"
                        Case "N4"
                        Case "REF"
                        Case "TRN"
                        Case "DTM"
                        Case "LX"
                        Case "SE"
                            rst.MoveNext()
                            If IsDBNull(Convert.ToString(rst.Fields.Item("F1"))) Then rst.MoveNext()
                            RecID = Convert.ToString(rst.Fields("F1"))
                            If RecID = "IEA" Then
                                rst.MoveNext()
                                If IsDBNull(Convert.ToString(rst.Fields.Item("F1"))) Then rst.MoveNext()
                            End If
                    End Select
                    rst.MoveNext()
                    If IsDBNull(Convert.ToString(rst.Fields.Item("F1"))) Then rst.MoveNext()
                    If Not rst.EOF Then RecID = Convert.ToString(rst.Fields.Item("F1"))
                Loop







            End If
            '************************************************************************************************************************************
            '************************************************************************************************************************************





            '************************************************************************************************************************************
            '************************************************************************************************************************************
            '2000A 20 – Payer Information Source
            'HL*1**20*1~
            If RecID = "HL" And Not rst.EOF Then
                If Convert.ToString(rst.Fields("F4")) = "20" Then
                    lngProviderID = 0
                    lngPatientID = 0
                    blnReverify = False
                    blnVerify = False
                    blnReject = False
                    strHLtype = "Payer"
                    rst.MoveNext()


                    If Not rst.EOF Then
                        RecID = Convert.ToString(rst.Fields.Item("F1"))
                    End If

                    Do While (RecID <> "HL") And (RecID <> "EB") And (RecID <> "DTP") And (RecID <> "SE") And Not rst.EOF

                        'NM1*PR*2*IL BCBS*****PI*G00621~
                        If RecID = "NM1" And Not rst.EOF Then
                            'PR = Payer
                            If Convert.ToString(rst.Fields("F2")) = "PR" Then
                                strPayorID = Replace(_DB.IfNull(Convert.ToString(rst.Fields("F10")), ""), "'", "")
                                HealthPlanName = Trim(Convert.ToString(rst.Fields("F4")))
                                strData = strData & Trim(Convert.ToString(rst.Fields("F4"))) & " " & strPayorID & Chr(10)
                            End If
                        End If

                        'AAA*N**51*C~  Receiver Error
                        If RecID = "AAA" And Not rst.EOF Then

                            If Convert.ToString(rst.Fields("F2")) = "N" Then
                                blnReject = False
                            End If

                            If Convert.ToString(rst.Fields("F2")) = "Y" Then
                                blnReject = True
                            End If





                            If Convert.ToString(rst.Fields("F4")) = "" Then
                                If Convert.ToString(rst.Fields("F4")) = "15" Then strData = strData & "Error: Required application data missing" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "41" Then strData = strData & "Error: Authorization/Access Restrictions" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "42" Then strData = strData & "Error: Unable to IDENTIFY YOUR PATIENT" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "43" Then strData = strData & "Error: Invalid/Missing Provider" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "44" Then strData = strData & "Error: Invalid/Missing Provider Name" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "45" Then strData = strData & "Error: Invalid/Missing Provider Specialty" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "46" Then strData = strData & "Error: Invalid/Missing Provider Phone Number" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "47" Then strData = strData & "Error: Invalid/Missing Provider Phone Number" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "48" Then strData = strData & "Error: Invalid/Missing Referring Provider Identification Number" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "50" Then strData = strData & "Error: Provider Ineligible for Inquiries" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "51" Then strData = strData & "Error: Provider not on file" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "57" Then strData = strData & "Error: Invalid/Missing Date-of-Service" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "58" Then strData = strData & "Error: Invalid/Missing Date-of-Birth" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "60" Then strData = strData & "Error: Date-of-Birth Follows Date-of-Service" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "63" Then strData = strData & "Error: Date of Service in Future" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "64" Then strData = strData & "Error: Invalid/Missing Patient ID" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "65" Then strData = strData & "Error: Invalid/Missing Dependent Name" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "66" Then strData = strData & "Error: Invalid/Missing Gender" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "67" Then strData = strData & "Error: Dependent Not Found" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "68" Then strData = strData & "Error: Duplicate Dependent Id Number" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "70" Then strData = strData & "Error: Patient Gender Does Not Match" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "71" Then strData = strData & "Error: Patient Birth Date Does Not Match" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "72" Then strData = strData & "Error: Invalid/Missing Subscriber/Insured ID" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "73" Then strData = strData & "Error: Invalid/Missing Subscriber Name" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "74" Then strData = strData & "Error: Invalid/Missing Subscriber Gender" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "75" Then strData = strData & "Error: Subscriber Not Found" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "76" Then strData = strData & "Error: Duplicate Subscriber Id Number" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "77" Then strData = strData & "Error: Patient Not Found" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "78" Then strData = strData & "Error: Subscriber Not in Group" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "79" Then strData = strData & "Error: Invalid Participant Identification" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "80" Then strData = strData & "Error: No Response Received" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "97" Then strData = strData & "Error: Invalid or Missing Provider Address" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "T4" Then strData = strData & "Error: Payer Name or Identifier Missing" & Chr(10)
                                blnReject = True
                            End If

                            If Convert.ToString(rst.Fields("F5")) = "C" Then  'Follow-up Action Code
                                strData = strData & "Correct and resend." & Chr(10)
                            End If

                        End If

                        rst.MoveNext()

                        If Not rst.EOF Then
                            RecID = Convert.ToString(rst.Fields.Item("F1"))
                        End If

                    Loop
                End If
            End If
            '************************************************************************************************************************************
            '************************************************************************************************************************************



            '************************************************************************************************************************************
            '************************************************************************************************************************************
            '2000B - Receiver Information
            'HL*2*1*21*1~
            If RecID = "HL" And Not rst.EOF Then
                If Convert.ToString(rst.Fields("F4")) = "21" Then
                    strHLtype = "Receiver"
                    rst.MoveNext()



                    If Not rst.EOF Then
                        RecID = Convert.ToString(rst.Fields.Item("F1"))
                    End If



                    Do While (RecID <> "HL") And (RecID <> "EB") And (RecID <> "DTP") And (RecID <> "SE") And Not rst.EOF
                        Select Case RecID
                            Case "HL"
                                If Convert.ToString(rst.Fields("F4")) = "21" Then
                                End If
                            Case "NM1"
                                ' Notes ***************************************************************
                                'NM1*1P*1*RAY*MARY****XX*1033652573~
                                '1P = Provider
                                '2B = Third-Party Administrator
                                '80 = Hospital
                                'FA = Facility
                                'GP = Gateway Provider
                                'P5 = Plan Sponsor
                                'PR = Payer
                                '***********************************************************************
                                If Convert.ToString(rst.Fields("F2")) = "1P" Then
                                    If Convert.ToString(rst.Fields("F3")) = "1" Then  'Provider
                                        If Trim(Convert.ToString(rst.Fields("F9"))) = "XX" Then     'National Provider Identification Number

                                            If lngProviderID <= 0 Then

                                                strSQL = "SELECT P.fldProviderID, P.fldGroupID FROM tblProvider AS P INNER JOIN tblUser AS U ON U.fldUserID = P.fldProviderID WHERE (P.fldProviderNPI = '" & Replace(_DB.IfNull(Convert.ToString(rst.Fields("F10")), ""), "~", "") & "' AND U.fldDisabledYN = 'N')"
                                                rstSQL = CreateObject("ADODB.Recordset")
                                                rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                                lngProviderID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldProviderID"))
                                                rstSQL = Nothing



                                            End If

                                            If lngProviderID <= 0 Then
                                                strSQL = "SELECT P.fldProviderID, P.fldGroupID FROM tblProvider AS P INNER JOIN tblUser AS U ON U.fldUserID = P.fldProviderID WHERE (P.fldProviderNPI = '" & Replace(_DB.IfNull(Convert.ToString(rst.Fields("F10")), ""), "~", "") & "' AND U.fldDisabledYN = 'Y')"
                                                rstSQL = CreateObject("ADODB.Recordset")
                                                rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                                lngProviderID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldProviderID"))
                                                rstSQL = Nothing
                                            End If

                                        End If


                                        strData = strData & Trim(Convert.ToString(rst.Fields("F5"))) & " " & Trim(Convert.ToString(rst.Fields("F4"))) & " NPI: " & Replace(_DB.IfNull(Convert.ToString(rst.Fields("F10")), ""), "~", "") & Chr(10)



                                    ElseIf Convert.ToString(rst.Fields("F3")) = "2" Then  'Group
                                        strSQL = "SELECT fldGroupID FROM tblProviderGroup WHERE (fldGroupNPI = '" & _DB.IfNull(Convert.ToString(rst.Fields("F10")), "") & "')"
                                        rstSQL = CreateObject("ADODB.Recordset")
                                        rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                        GroupID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldGroupID"))
                                        rstSQL = Nothing
                                        strData = strData & Trim(Convert.ToString(rst.Fields("F4"))) & " NPI: " & Replace(_DB.IfNull(Convert.ToString(rst.Fields("F10")), "ERROR: Missing NPI"), "~", "") & Chr(10)
                                    End If


                                End If

                            Case "N3"
                                'Insured Street address
                                strData = strData & Convert.ToString(rst.Fields("F2")) & Chr(10)

                            Case "N4"
                                'Insured City State address
                                strData = strData + Convert.ToString(rst.Fields("F2")) & Chr(10)

                            Case "PRV"
                                'PRV*BI
                                'Providers role
                                strData = strData & "Providers role: "
                                Select Case Trim(Convert.ToString(rst.Fields("F2")))
                                    Case "H"
                                        strData = strData & "Hospital "
                                    Case "R"
                                        strData = strData & "Rural Health Clinic "
                                    Case "AD"
                                        strData = strData & "Admitting "
                                    Case "AT"
                                        strData = strData & "Attending "
                                    Case "BI"
                                        strData = strData & "Billing "
                                    Case "CO"
                                        strData = strData & "Consulting "
                                    Case "CV"
                                        strData = strData & "Covering "
                                    Case "HH"
                                        strData = strData & "Home Health Care "
                                    Case "LA"
                                        strData = strData & "Laboratory "
                                    Case "OT"
                                        strData = strData & "Other Physician "
                                    Case "P1"
                                        strData = strData & "Pharmacist "
                                    Case "P2"
                                        strData = strData & "Pharmacy "
                                    Case "PC"
                                        strData = strData & "Primary Care Physician"
                                    Case "PE"
                                        strData = strData & "Performing "
                                    Case "RF"
                                        strData = strData & "Referring "
                                    Case "SB"
                                        strData = strData & "Submitting "
                                    Case "SK"
                                        strData = strData & "Skilled Nursing Facility "
                                    Case "SU"
                                        strData = strData & "Supervising "
                                End Select

                            Case "AAA"
                                'AAA*N**51*C~  Receiver Error
                                If Convert.ToString(rst.Fields("F2")) = "N" Then blnReject = False
                                If Convert.ToString(rst.Fields("F2")) = "Y" Then blnReject = True
                                If Convert.ToString(rst.Fields("F4")) <> "" Then
                                    If Convert.ToString(rst.Fields("F4")) = "15" Then strData = strData & "Error: Required application data missing" & Chr(10)
                                    If Convert.ToString(rst.Fields("F4")) = "35" Then strData = strData & "Error: Out of Network" & Chr(10)
                                    If Convert.ToString(rst.Fields("F4")) = "41" Then strData = strData & "Error: Authorization/Access Restrictions" & Chr(10)
                                    If Convert.ToString(rst.Fields("F4")) = "42" Then strData = strData & "Error: Unable to IDENTIFY YOUR PATIENT" & Chr(10)
                                    If Convert.ToString(rst.Fields("F4")) = "43" Then strData = strData & "Error: Invalid/Missing Provider" & Chr(10)
                                    If Convert.ToString(rst.Fields("F4")) = "44" Then strData = strData & "Error: Invalid/Missing Provider Name" & Chr(10)
                                    If Convert.ToString(rst.Fields("F4")) = "45" Then strData = strData & "Error: Invalid/Missing Provider Specialty" & Chr(10)
                                    If Convert.ToString(rst.Fields("F4")) = "46" Then strData = strData & "Error: Invalid/Missing Provider Phone Number" & Chr(10)
                                    If Convert.ToString(rst.Fields("F4")) = "47" Then strData = strData & "Error: Invalid/Missing Provider Phone Number" & Chr(10)
                                    If Convert.ToString(rst.Fields("F4")) = "48" Then strData = strData & "Error: Invalid/Missing Referring Provider Identification Number" & Chr(10)
                                    If Convert.ToString(rst.Fields("F4")) = "50" Then strData = strData & "Error: Provider Ineligible for Inquiries" & Chr(10)
                                    If Convert.ToString(rst.Fields("F4")) = "51" Then strData = strData & "Error: Provider not on file" & Chr(10)
                                    If Convert.ToString(rst.Fields("F4")) = "57" Then strData = strData & "Error: Invalid/Missing Date-of-Service" & Chr(10)
                                    If Convert.ToString(rst.Fields("F4")) = "58" Then strData = strData & "Error: Invalid/Missing Date-of-Birth" & Chr(10)
                                    If Convert.ToString(rst.Fields("F4")) = "60" Then strData = strData & "Error: Date-of-Birth Follows Date-of-Service" & Chr(10)
                                    If Convert.ToString(rst.Fields("F4")) = "63" Then strData = strData & "Error: Date of Service in Future" & Chr(10)
                                    If Convert.ToString(rst.Fields("F4")) = "64" Then strData = strData & "Error: Invalid/Missing Patient ID" & Chr(10)
                                    If Convert.ToString(rst.Fields("F4")) = "65" Then strData = strData & "Error: Invalid/Missing Dependent Name" & Chr(10)
                                    If Convert.ToString(rst.Fields("F4")) = "66" Then strData = strData & "Error: Invalid/Missing Gender" & Chr(10)
                                    If Convert.ToString(rst.Fields("F4")) = "67" Then strData = strData & "Error: Dependent Not Found" & Chr(10)
                                    If Convert.ToString(rst.Fields("F4")) = "68" Then strData = strData & "Error: Duplicate Dependent Id Number" & Chr(10)
                                    If Convert.ToString(rst.Fields("F4")) = "70" Then strData = strData & "Error: Patient Gender Does Not Match" & Chr(10)
                                    If Convert.ToString(rst.Fields("F4")) = "71" Then strData = strData & "Error: Patient Birth Date Does Not Match" & Chr(10)
                                    If Convert.ToString(rst.Fields("F4")) = "72" Then strData = strData & "Error: Invalid/Missing Subscriber/Insured ID" & Chr(10)
                                    If Convert.ToString(rst.Fields("F4")) = "73" Then strData = strData & "Error: Invalid/Missing Subscriber Name" & Chr(10)
                                    If Convert.ToString(rst.Fields("F4")) = "74" Then strData = strData & "Error: Invalid/Missing Subscriber Gender" & Chr(10)
                                    If Convert.ToString(rst.Fields("F4")) = "75" Then strData = strData & "Error: Subscriber Not Found" & Chr(10)
                                    If Convert.ToString(rst.Fields("F4")) = "76" Then strData = strData & "Error: Duplicate Subscriber Id Number" & Chr(10)
                                    If Convert.ToString(rst.Fields("F4")) = "77" Then strData = strData & "Error: Patient Not Found" & Chr(10)
                                    If Convert.ToString(rst.Fields("F4")) = "78" Then strData = strData & "Error: Subscriber Not in Group" & Chr(10)
                                    If Convert.ToString(rst.Fields("F4")) = "79" Then strData = strData & "Error: Invalid Participant Identification" & Chr(10)
                                    If Convert.ToString(rst.Fields("F4")) = "80" Then strData = strData & "Error: No Response Received" & Chr(10)
                                    If Convert.ToString(rst.Fields("F4")) = "97" Then strData = strData & "Error: Invalid or Missing Provider Address" & Chr(10)
                                    If Convert.ToString(rst.Fields("F4")) = "T4" Then strData = strData & "Error: Payer Name or Identifier Missing" & Chr(10)
                                    blnReject = True
                                End If
                                If Convert.ToString(rst.Fields("F5")) = "C" Then  'Follow-up Action Code
                                    strData = strData & "Correct and resend." & Chr(10)
                                End If
                        End Select
                        rst.MoveNext()
                        If Not rst.EOF Then RecID = Convert.ToString(rst.Fields.Item("F1"))
                    Loop
                End If
            End If

            '************************************************************************************************************************************
            '************************************************************************************************************************************





            '************************************************************************************************************************************
            '************************************************************************************************************************************
            '2000C Subscriber
            'HL*3*2*22*1~
            If RecID = "HL" And Not rst.EOF Then
                If Convert.ToString(rst.Fields("F4")) = "22" Then
                    strHLtype = "Subscriber"
                    blnReject = False
                    lngPatientID = 0
                    PatientLastName = ""
                    PatientFirstName = ""
                    PatientMiddleInitial = ""
                    PatientPolicyID = ""
                    InsuredGroupName = ""
                    InsuredGroupNbr = ""
                    rst.MoveNext()
                    If Not rst.EOF Then
                        RecID = Convert.ToString(rst.Fields.Item("F1"))
                    End If


                    Do While (RecID <> "EB") And (RecID <> "SE") And (RecID <> "ST") And (RecID & Convert.ToString(rst.Fields.Item("F4")) <> "HL22") And Not rst.EOF




                        Select Case RecID

                            Case "NM1"
                                'NM1*IL*1*THURBER*JEFFREY****MI*JZH123688728001~
                                If Convert.ToString(rst.Fields("F2")) = "IL" Then  'Subscriber
                                    If Convert.ToString(rst.Fields("F3")) = "1" Then  'Person
                                        'InsuredPolicyID = IIf(IsDBNull(Convert.ToString(rst.Fields("F10"))), "", Trim(Convert.ToString(rst.Fields("F10"))))
                                        'InsuredLastName = Replace(IIf(IsDBNull(Convert.ToString(rst.Fields("F4"))), "", Trim(Convert.ToString(rst.Fields("F4"))), "'", ""))
                                        'InsuredFirstName = Replace(IIf(IsDBNull(Convert.ToString(rst.Fields("F5"))), "", Trim(Convert.ToString(rst.Fields("F5"))), "'", ""))
                                        'InsuredMiddleInitial = IIf(IsDBNull(convert.tostring(rst.Fields("F6")), "", Trim(Mid(convert.tostring(rst.Fields)("F6"), 1, 1)))
                                        'PatientPolicyID = IIf(IsDBNull(convert.tostring(rst.Fields("F10")), "", Trim(convert.tostring(rst.Fields("F10")))
                                        'PatientLastName = Replace(IIf(IsDBNull(convert.tostring(rst.Fields("F4")), "", Trim(convert.tostring(rst.Fields("F4"))), "'", "")
                                        'PatientFirstName = Replace(IIf(IsDBNull(convert.tostring(rst.Fields("F5")), "", Trim(convert.tostring(rst.Fields("F5"))), "'", "")
                                        'PatientMiddleInitial = IIf(IsDBNull(convert.tostring(rst.Fields("F6")), "", Trim(Mid(convert.tostring(rst.Fields("F6"), 1, 1)))
                                        'strData = strData & "Subscriber: " & InsuredLastName & ", " & InsuredFirstName & " " & InsuredMiddleInitial & Chr(10)
                                        'strData = strData & "Policy ID: " & InsuredPolicyID & Chr(10)
                                    ElseIf Convert.ToString(rst.Fields("F3")) = "2" Then  'Group
                                    End If
                                End If
                                'NM1*03*1*THURBER*AMANDA~

                                If Convert.ToString(rst.Fields("F2")) = "03" Then  'Dependent
                                    If Convert.ToString(rst.Fields("F3")) = "1" Then  'Person
                                        'PatientPolicyID = IIf(IsDBNull(convert.tostring(rst.Fields("F10")), PatientPolicyID, Trim(convert.tostring(rst.Fields("F10")))
                                        'PatientLastName = Replace(IIf(IsDBNull(convert.tostring(rst.Fields("F4")), "", Trim(convert.tostring(rst.Fields("F4"))), "'", "")
                                        'PatientFirstName = Replace(IIf(IsDBNull(convert.tostring(rst.Fields("F5")), "", Trim(convert.tostring(rst.Fields("F5"))), "'", "")
                                        'PatientMiddleInitial = IIf(IsDBNull(convert.tostring(rst.Fields("F6")), "", Trim(Mid(convert.tostring(rst.Fields("F6"), 1, 1)))
                                        'strData = strData & "Dependent: " & PatientLastName & ", " & PatientFirstName & " " & PatientMiddleInitial & Chr(10)
                                    ElseIf Convert.ToString(rst.Fields("F3")) = "2" Then  'Group
                                    End If
                                End If
                                '*******************************************************************************************************************************************************



                            Case "REF"
                                '*******************************************************************************************************************************************************

                                '18, 49, 6P, HJ, Q4
                                '18 = Plan Number
                                'IL = Group or Policy Number
                                '1W = Member Identification Number
                                '6P = Group Number
                                'EA = Medical Record Identification Number
                                'EJ = Patient Account Number
                                'F6 = Medicare ID
                                'HJ = Identity Card Number
                                'IF = Issue Number
                                'IG = Insurance Policy Number
                                'N6 = Plan Network Identification Number
                                'NQ = Medicaid Recipient Identification Number
                                'Q4 = Prior Identifier Number
                                'Y4 = Agency Claim Number
                                'SY = Social Security Number
                                'REF*6P*Y00002636*City Group Medical~
                                If Convert.ToString(rst.Fields("F2")) = "6P" Then
                                    InsuredGroupNbr = IIf(IsDBNull(Convert.ToString(rst.Fields("F3"))), "", Trim(Convert.ToString(rst.Fields("F3"))))
                                    InsuredGroupName = IIf(IsDBNull(Convert.ToString(rst.Fields("F4"))), "", Trim(Convert.ToString(rst.Fields("F4"))))
                                    strData = strData & "Group: " & InsuredGroupNbr & " " & InsuredGroupName & Chr(10)
                                End If
                                'REF*18*725~
                                If Convert.ToString(rst.Fields("F2")) = "18" Then
                                    'Dependent
                                End If
                                If Convert.ToString(rst.Fields("F2")) = "49" Then
                                End If
                                If Convert.ToString(rst.Fields("F2")) = "HJ" Then  'Submitted Dependent ID
                                End If
                                If Convert.ToString(rst.Fields("F2")) = "Q4" Then  'Different than Submitted Dependent ID
                                End If
                                If Convert.ToString(rst.Fields("F2")) = "SY" Then  'SSN
                                End If
                                '*******************************************************************************************************************************************************





                            Case "TRN"
                                '*******************************************************************************************************************************************************
                                'TRN*2*9219135698*1336509281~
                                If Convert.ToString(rst.Fields("F2")) = "2" Or Convert.ToString(rst.Fields("F2")) = "1" And lngPatientID = 0 Then  ' And blnReject
                                    If IsNumeric(Convert.ToString(rst.Fields("F3"))) Then  'PatientID and PlanID
                                        'If Len(Convert.ToString(rst.Fields("F3"))) = 14 Then
                                        '    lngPatientID = CLng(Trim(Left(IIf(IsDBNull(convert.tostring(rst.Fields("F3")), "0", Trim(convert.tostring(rst.Fields("F3"))), 8)))
                                        '    lngPlanID = CLng(Trim(Mid(IIf(IsDBNull(convert.tostring(rst.Fields("F3")), "0", Trim(convert.tostring(rst.Fields("F3"))), 9, 6)))
                                        'Else
                                        '    lngPatientID = CLng(Trim(Left(IIf(IsDBNull(convert.tostring(rst.Fields("F3")), "0", Trim(convert.tostring(rst.Fields("F3"))), 8)))
                                        '    lngPlanID = CLng(Trim(Mid(IIf(IsDBNull(convert.tostring(rst.Fields("F3")), "0", Trim(convert.tostring(rst.Fields("F3"))), 8, 6)))
                                        'End If


                                    ElseIf lngPatientID = 0 Then  'Reverify

                                        If Len(Convert.ToString(rst.Fields("F3"))) >= 11 And (Right(Convert.ToString(rst.Fields("F3"), 1)) = "R" Or Right(Convert.ToString(rst.Fields("F3")), 1) = "V") Then  'PatientID, PlanID and Reverify Code 'R'


                                            If Right(Convert.ToString(rst.Fields("F3")), 1) = "R" Then
                                                blnReverify = True
                                            End If

                                            If Right(Convert.ToString(rst.Fields("F3"), 1)) = "V" Then
                                                blnVerify = True
                                            End If

                                            If Len(Convert.ToString(rst.Fields("F3"))) <= 11 Then
                                                lngPatientID = CLng(Trim(Left(IIf(IsDBNull(convert.tostring(rst.Fields("F3")), "0", Trim(convert.tostring(rst.Fields("F3"))), 8)))
                                                lngPlanID = CLng(Trim(Mid(IIf(IsDBNull(convert.tostring(rst.Fields("F3")), "0", Trim(convert.tostring(rst.Fields("F3"))), 9, 6)))
                                            Else
                                                lngPatientID = CLng(Trim(Left(IIf(IsDBNull(convert.tostring(rst.Fields("F3")), "0", Trim(convert.tostring(rst.Fields("F3"))), 8)))
                                                lngPlanID = CLng(Trim(Mid(IIf(IsDBNull(convert.tostring(rst.Fields("F3")), "0", Trim(convert.tostring(rst.Fields("F3"))), 9, 6)))
                                            End If


                                        End If

                                    End If

                                    If lngPatientID > 0 Then
                                        strData = strData & "PatientID: " & lngPatientID & " PlanID: " & lngPlanID & Chr(10)
                                    ElseIf Convert.ToString(rst.Fields("F4")) <> "" Then 'BatchID
                                        strData = strData & "BatchID: " & Convert.ToString(rst.Fields("F4")) & Chr(10)
                                    End If




                                ElseIf Convert.ToString(rst.Fields("F2")) = "1" And lngPatientID <> 0 Then
                                    'If IsNumeric(convert.tostring(rst.Fields("F3")) Then
                                    '    If Len(convert.tostring(rst.Fields("F3")) < 13 Or Len(convert.tostring(rst.Fields("F3")) > 15 Then
                                    '        If convert.tostring(rst.Fields("F4") <> "" Then
                                    '            strData = strData & "COVERED BY ANOTHER INTERMEDIARY: " & convert.tostring(rst.Fields("F4").ToString & Chr(10)
                                    '                End If
                                    '            End If
                                    '        End If
                                End If

                                '***********************************************************************************************************************************************************









                                '***********************************************************************************************************************************************************
                            Case "DMG"
                                'DMG*D8*20010322*F~
                                If Convert.ToString(rst.Fields("F2")) = "D8" Then  'Date Of Birth, Sex
                                    'PatientDOB.ToOADate = 0
                                    'LC
                                    PatientSex = IIf(IsDBNull(Convert.ToString(rst.Fields("F4")), "", Trim(Convert.ToString(rst.Fields("F4")))))

                                    'LC
                                    If Convert.ToString(rst.Fields("F3")) <> "" Then
                                        If Len(Convert.ToString(rst.Fields("F3"))) = 6 Then
                                            PatientDOB = DateSerial(CInt(Mid(Convert.ToString(rst.Fields("F3")), 1, 2)) + 2000, CInt(Mid(Convert.ToString(rst.Fields("F3"), 3, 2))), CInt(Mid(Convert.ToString(rst.Fields("F3"), 5, 2))))
                                        Else
                                            PatientDOB = DateSerial(CInt(Mid(Convert.ToString(rst.Fields("F3"), 1, 4))), CInt(Mid(Convert.ToString(rst.Fields("F3"), 5, 2))), CInt(Mid(Convert.ToString(rst.Fields("F3"), 7, 2))))
                                        End If
                                        strData = strData & strHLtype & " DOB: " & PatientDOB & " " & strHLtype & " Sex: " & PatientSex & Chr(10)
                                    Else
                                        strData = strData & strHLtype & " DOB: " & " " & strHLtype & " Sex: " & PatientSex & Chr(10)
                                    End If


                                End If





                                '************************************************************************************************************************************

                            Case "HL"
                                'HL*4*3*23*0~
                                If Convert.ToString(rst.Fields("F4")) = "23" Then  'Dependent
                                    strHLtype = "Dependent"
                                End If


                                '************************************************************************************************************************************

                            Case "N3"   'Insured Street address
                                strData = strData & Convert.ToString(rst.Fields("F2")) & Chr(10)



                                '************************************************************************************************************************************


                            Case "N4"   'Insured City State Zip
                                strData = strData & Convert.ToString(rst.Fields("F2")) & " " & Convert.ToString(rst.Fields("F3")) & ", " & Convert.ToString(rst.Fields("F4")) & Chr(10)




                                '************************************************************************************************************************************

                            Case "DTP"
                                'RD8 - both a start date and end date
                                '290 Coordination of Benefits
                                '291 Plan
                                '292 Benefit
                                '295 Primary Care Provider
                                '304 Latest Visit Or Consultation
                                '307 Eligibility
                                '318 Added
                                '346 Plan Begin
                                '348 Benefit Begin
                                '349 Benefit End
                                '356 Eligibility Begin
                                '357 Eligibility End
                                '435 Admission
                                '472 Service
                                '540 Policy Expiration
                                '636 Date of Last Update
                                EffDate = 0
                                TermDate = 0


                                If Trim(Convert.ToString(rst.Fields("F3"))) = "D8" And Not IsDBNull(Convert.ToString(rst.Fields("F4"))) Then
                                    If Len(Convert.ToString(rst.Fields("F4"))) = 6 Then
                                        '  EffDate = DateSerial(CInt(Mid(Convert.ToString(rst.Fields("F4"), 1, 2)) + 2000), CInt(Mid(Convert.ToString(rst.Fields("F4"), 3, 2))), CInt(Mid(Convert.ToString(rst.Fields("F4"), 5, 2))))
                                    Else
                                        'EffDate = DateSerial(CInt(Mid(Convert.ToString(rst.Fields("F4"), 1, 4))), CInt(Mid(Convert.ToString(rst.Fields("F4"), 5, 2))), CInt(Mid(Convert.ToString(rst.Fields("F4"), 7, 2))))
                                    End If
                                ElseIf Trim(Convert.ToString(rst.Fields("F3"))) = "RD8" And Not IsDBNull(Convert.ToString(rst.Fields("F4"))) Then
                                    If Len(Convert.ToString(rst.Fields("F4"))) > 18 Then
                                        '  EffDate = DateSerial(CInt(Mid(convert.tostring(rst.Fields("F4"), 1, 2)) + 2000, CInt(Mid(convert.tostring(rst.Fields("F4"), 3, 2)), CInt(Mid(convert.tostring(rst.Fields("F4"), 5, 2)))
                                        TermDate = 0
                                    Else
                                        '                 Msg = CInt(Mid(convert.tostring(rst.Fields("F4"), InStr(1, convert.tostring(rst.Fields("F4"), "-") + 7, 2))
                                        'EffDate = DateSerial(CInt(Mid(convert.tostring(rst.Fields("F4"), 1, 4)), CInt(Mid(convert.tostring(rst.Fields("F4"), 5, 2)), CInt(Mid(convert.tostring(rst.Fields("F4"), 7, 2)))
                                        ' TermDate = DateSerial(CInt(Mid(convert.tostring(rst.Fields("F4"), InStr(1, convert.tostring(rst.Fields("F4"), "-") + 1, 4)), CInt(Mid(convert.tostring(rst.Fields("F4"), InStr(1, convert.tostring(rst.Fields("F4"), "-") + 5, 2)), CInt(Mid(convert.tostring(rst.Fields("F4"), InStr(1, convert.tostring(rst.Fields("F4"), "-") + 7, 2)))
                                    End If
                                End If





                                ' this whole blcok should be 3 lines of code!!!!!!!!!!!!!!!!
                                '307 = Eligibility
                                '472 = Service
                                If Convert.ToString(rst.Fields("F2")) = "346" Then    'DTP*346*D8*20070501~ = Member eligibility started on 05/01/2007
                                    strData = strData & "Effective Date: " & EffDate & " "
                                    If TermDate <> 0 Then
                                        strData = strData & "Termed Date: " & TermDate & " "
                                    End If
                                End If
                                If Convert.ToString(rst.Fields("F2")) = "349" Then    'DTP*349*D8*20080630~ = coverage ended on of 6/30/2008
                                    strData = strData & "Termed Date: " & EffDate & " "
                                End If
                                If Convert.ToString(rst.Fields("F2")) = "356" Then    'DTP*349*D8*20080630~ = coverage ended on of 6/30/2008
                                    strData = strData & "Eligibility Begins: " & EffDate & " "
                                End If
                                If Convert.ToString(rst.Fields("F2")) = "291" Then    'DTP*291*RD8*20160601-99991231~  'Subscriber’s Coverage Dates
                                    strData = strData & "Subscribers Coverage Date: " & EffDate & " "
                                    If TermDate <> 0 Then
                                        strData = strData & "Termed Date: " & TermDate & " "
                                    End If
                                End If
                                If Convert.ToString(rst.Fields("F2")) = "290" Then    'DTP*291*RD8*20160601-99991231~  'Subscriber’s Coverage Dates
                                    If TermDate <> 0 Then
                                        strData = strData & "Subscribers Coverage Date: " & EffDate & " "
                                        strData = strData & "Termed Date: " & TermDate & " "
                                    Else
                                        strData = strData & "Termed Date: " & EffDate & " "
                                    End If
                                End If
                                If Convert.ToString(rst.Fields("F2")) = "307" Then     '307 - Eligibility Time Period~
                                    strData = strData & "Effective Date: " & EffDate & " "
                                    If TermDate <> 0 Then
                                        strData = strData & " Termed Date: " & TermDate & " "
                                    End If
                                End If
                                If Convert.ToString(rst.Fields("F2")) = "472" Then     '307 - Eligibility Requested for Date~
                                    If EffDate <> 0 Then
                                        strData = strData & "Eligibility Requested for: " & EffDate & " "
                                    End If
                                End If
                                If Convert.ToString(rst.Fields("F2")) = "540" Then     '540 - Policy Expiration~
                                    If EffDate <> 0 Then
                                        strData = strData & "Policy Expiration Date: " & EffDate & " "
                                    End If
                                End If
                                strData = strData & Chr(10)



                                '************************************************************************************************************************************

                            Case "INS"
                                strData = strData & "Responsible Party: "
                                'INS01 Insured Indicator - Y N
                                If Convert.ToString(rst.Fields("F2")) = "Y" Then strData = strData & "Insured "
                                If Convert.ToString(rst.Fields("F2")) = "N" Then strData = strData & "Dependent "
                                ' “Y” value indicates the insured )is a subscriber: an
                                ' “N” value indicates a dependent.
                                'INS02 Relationship Code1111
                                If Convert.ToString(rst.Fields("F3")) = "18" Then strData = strData & "Self "
                                If Convert.ToString(rst.Fields("F3")) = "1" Then strData = strData & "Spouse "
                                If Convert.ToString(rst.Fields("F3")) = "19" Then strData = strData & "Child "
                                If Convert.ToString(rst.Fields("F3")) = "20" Then strData = strData & "Employee "
                                If Convert.ToString(rst.Fields("F3")) = "21" Then strData = strData & "Unknown "
                                If Convert.ToString(rst.Fields("F3")) = "39" Then strData = strData & "Organ Donor "
                                If Convert.ToString(rst.Fields("F3")) = "40" Then strData = strData & "Cadaver Donor "
                                If Convert.ToString(rst.Fields("F3")) = "53" Then strData = strData & "Life Partner "
                                If Convert.ToString(rst.Fields("F3")) = "G8" Then strData = strData & "Other Relationship "
                                'INS03 Maintenance Type Code
                                If Convert.ToString(rst.Fields("F4")) = "001" Then strData = strData & "Change "
                                'INS04 Maintenance Reason Code
                                If Convert.ToString(rst.Fields("F5")) = "25" Then strData = strData & "Change in Identifying Data Elements "
                                strData = strData & Chr(10)
                            Case "AAA"
                                'AAA*N**51*C~  Receiver Error
                                If Convert.ToString(rst.Fields("F2")) = "N" Then blnReject = False
                                If Convert.ToString(rst.Fields("F2")) = "Y" Then blnReject = True
                                If Convert.ToString(rst.Fields("F4")) <> "" Then

                                End If
                                If Convert.ToString(rst.Fields("F4")) = "15" Then strData = strData & "Error: Required application data missing" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "41" Then strData = strData & "Error: Authorization/Access Restrictions" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "42" Then strData = strData & "Error: Unable to IDENTIFY YOUR PATIENT" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "43" Then strData = strData & "Error: Invalid/Missing Provider" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "44" Then strData = strData & "Error: Invalid/Missing Provider Name" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "45" Then strData = strData & "Error: Invalid/Missing Provider Specialty" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "46" Then strData = strData & "Error: Invalid/Missing Provider Phone Number" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "47" Then strData = strData & "Error: Invalid/Missing Provider Phone Number" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "48" Then strData = strData & "Error: Invalid/Missing Referring Provider Identification Number" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "50" Then strData = strData & "Error: Provider Ineligible for Inquiries" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "51" Then strData = strData & "Error: Provider not on file" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "57" Then strData = strData & "Error: Invalid/Missing Date-of-Service" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "58" Then strData = strData & "Error: Invalid/Missing Date-of-Birth" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "60" Then strData = strData & "Error: Date-of-Birth Follows Date-of-Service" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "63" Then strData = strData & "Error: Date of Service in Future" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "64" Then strData = strData & "Error: Invalid/Missing Patient ID" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "65" Then strData = strData & "Error: Invalid/Missing Dependent Name" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "66" Then strData = strData & "Error: Invalid/Missing Gender" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "67" Then strData = strData & "Error: Dependent Not Found" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "68" Then strData = strData & "Error: Duplicate Dependent Id Number" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "70" Then strData = strData & "Error: Patient Gender Does Not Match" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "71" Then strData = strData & "Error: Patient Birth Date Does Not Match" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "72" Then strData = strData & "Error: Invalid/Missing Subscriber/Insured ID" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "73" Then strData = strData & "Error: Invalid/Missing Subscriber Name" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "74" Then strData = strData & "Error: Invalid/Missing Subscriber Gender" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "75" Then strData = strData & "Error: Subscriber Not Found" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "76" Then strData = strData & "Error: Duplicate Subscriber Id Number" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "77" Then strData = strData & "Error: Patient Not Found" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "78" Then strData = strData & "Error: Subscriber Not in Group" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "79" Then strData = strData & "Error: Invalid Participant Identification" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "80" Then strData = strData & "Error: No Response Received" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "97" Then strData = strData & "Error: Invalid or Missing Provider Address" & Chr(10)
                                If Convert.ToString(rst.Fields("F4")) = "T4" Then strData = strData & "Error: Payer Name or Identifier Missing" & Chr(10)
                                blnReject = True
                                End If
                If Convert.ToString(rst.Fields("F5")) = "C" Then  'Follow-up Action Code
                    strData = strData & "Correct and resend." & Chr(10)
                End If
                        End Select
                        rst.MoveNext()
                        If Not rst.EOF Then RecID = Convert.ToString(rst.Fields.Item("F1"))
                    Loop
                End If
            End If

            '2110CD     EB01 active-1, inactive-6 or non-covered-I in loop 2110C/D
            If (RecID = "DTP" Or RecID = "EB") And Not rst.EOF Then
                Do While (RecID <> "SE") And (RecID <> "ST") And (RecID <> "HL" Or Convert.ToString(rst.Fields("F4")) = "23") And Not rst.EOF
                    Select Case RecID
                        Case "DTP"
                            'RD8 - both a start date and end date
                            '290 Coordination of Benefits
                            '291 Plan
                            '292 Benefit
                            '295 Primary Care Provider
                            '304 Latest Visit Or Consultation
                            '307 Eligibility
                            '318 Added
                            '346 Plan Begin
                            '348 Benefit Begin
                            '349 Benefit End
                            '356 Eligibility Begin
                            '357 Eligibility End
                            '435 Admission
                            '472 Service
                            '636 Date of Last Update
                            EffDate = 0 : TermDate = 0
                            If Trim(convert.tostring(rst.Fields("F3"))) = "D8" And Not IsDBNull(convert.tostring(rst.Fields("F4")) Then
                                If Len(Convert.ToString(rst.Fields("F4"))) = 6 Then
                                    EffDate = DateSerial(CInt(Mid(convert.tostring(rst.Fields("F4"), 1, 2)) + 2000, CInt(Mid(convert.tostring(rst.Fields("F4"), 3, 2)), CInt(Mid(convert.tostring(rst.Fields("F4"), 5, 2)))
                                Else
                                    EffDate = DateSerial(CInt(Mid(convert.tostring(rst.Fields("F4"), 1, 4)), CInt(Mid(convert.tostring(rst.Fields("F4"), 5, 2)), CInt(Mid(convert.tostring(rst.Fields("F4"), 7, 2)))
                                End If
                            ElseIf Trim(convert.tostring(rst.Fields("F3"))) = "RD8" And Not IsDBNull(convert.tostring(rst.Fields("F4")) Then
                                If Len(Convert.ToString(rst.Fields("F4"))) > 18 Then
                                    EffDate = DateSerial(CInt(Mid(convert.tostring(rst.Fields("F4"), 1, 2)) + 2000, CInt(Mid(convert.tostring(rst.Fields("F4"), 3, 2)), CInt(Mid(convert.tostring(rst.Fields("F4"), 5, 2)))
                                    TermDate = 0
                                Else
                                    '                 Msg = CInt(Mid(convert.tostring(rst.Fields("F4"), InStr(1, convert.tostring(rst.Fields("F4"), "-") + 7, 2))
                                    EffDate = DateSerial(CInt(Mid(convert.tostring(rst.Fields("F4"), 1, 4)), CInt(Mid(convert.tostring(rst.Fields("F4"), 5, 2)), CInt(Mid(convert.tostring(rst.Fields("F4"), 7, 2)))
                                    TermDate = DateSerial(CInt(Mid(convert.tostring(rst.Fields("F4"), InStr(1, convert.tostring(rst.Fields("F4"), "-") + 1, 4)), CInt(Mid(convert.tostring(rst.Fields("F4"), InStr(1, convert.tostring(rst.Fields("F4"), "-") + 5, 2)), CInt(Mid(convert.tostring(rst.Fields("F4"), InStr(1, convert.tostring(rst.Fields("F4"), "-") + 7, 2)))
                                End If
                            End If
                            '307 = Eligibility
                            '472 = Service
                            If Convert.ToString(rst.Fields("F2")) = "346" Then    'DTP*346*D8*20070501~ = Member eligibility started on 05/01/2007
                                strData = strData & "Effective Date: " & EffDate & " "
                                If TermDate <> 0 Then
                                    strData = strData & "Termed Date: " & TermDate & " "
                                End If
                            End If
                            If Convert.ToString(rst.Fields("F2")) = "349" Then    'DTP*349*D8*20080630~ = coverage ended on of 6/30/2008
                                strData = strData & "Termed Date: " & EffDate & " "
                            End If
                            If Convert.ToString(rst.Fields("F2")) = "356" Then    'DTP*349*D8*20080630~ = coverage ended on of 6/30/2008
                                strData = strData & "Eligibility Begins: " & EffDate & " "
                            End If
                            If Convert.ToString(rst.Fields("F2")) = "291" Then    'DTP*291*RD8*20160601-99991231~  'Subscriber’s Coverage Dates
                                strData = strData & "Subscribers Coverage Date: " & EffDate & " "
                                If TermDate <> 0 Then
                                    strData = strData & "Termed Date: " & TermDate & " "
                                End If
                            End If
                            If Convert.ToString(rst.Fields("F2")) = "290" Then    'DTP*291*RD8*20160601-99991231~  'Subscriber’s Coverage Dates
                                If TermDate <> 0 Then
                                    strData = strData & "Subscribers Coverage Date: " & EffDate & " "
                                    strData = strData & "Termed Date: " & TermDate & " "
                                Else
                                    strData = strData & "Termed Date: " & EffDate & " "
                                End If
                            End If
                            If Convert.ToString(rst.Fields("F2")) = "307" Then     '307 - Eligibility Time Period~
                                strData = strData & "Effective Date: " & EffDate & " "
                                If TermDate <> 0 Then
                                    strData = strData & " Termed Date: " & TermDate & " "
                                End If
                            End If
                            If Convert.ToString(rst.Fields("F2")) = "472" Then     '307 - Eligibility Requested for Date~
                                If EffDate <> 0 Then
                                    strData = strData & "Eligibility Requested for: " & EffDate & " "
                                End If
                            End If
                            strData = strData & Chr(10)
                        Case "III"  'Place of Service
                            'III*ZZ*21~
                            Select Case Trim(Convert.ToString(rst.Fields("F2")))
                                Case "11"
                                    strData = strData & "Office Visit "
                                Case "21"
                                    strData = strData & "Hospital Visit "
                                Case "22"
                                    strData = strData & "Facility Visit "
                            End Select
                            strData = strData & Chr(10)
                        Case "HL"
                            'HL*4*3*23*0~
                            If Convert.ToString(rst.Fields("F4")) = "23" Then  'Dependent
                                strHLtype = "Dependent"
                            End If
                        Case "TRN"
                            'TRN*2*9219135698*1336509281~
                            If Convert.ToString(rst.Fields("F2")) = "2" Or (Convert.ToString(rst.Fields("F2")) = "1" And lngPatientID = 0) Then  ' And blnReject
                                If IsNumeric(Convert.ToString(rst.Fields("F3"))) Then  'PatientID and PlanID
                                    If Len(Convert.ToString(rst.Fields("F3"))) = 14 Then
                                        lngPatientID = CLng(Trim(Left(IIf(IsDBNull(convert.tostring(rst.Fields("F3")), "0", Trim(convert.tostring(rst.Fields("F3"))), 8)))
                                        lngPlanID = CLng(Trim(Mid(IIf(IsDBNull(convert.tostring(rst.Fields("F3")), "0", Trim(convert.tostring(rst.Fields("F3"))), 9, 6)))
                                    Else
                                        lngPatientID = CLng(Trim(Left(IIf(IsDBNull(convert.tostring(rst.Fields("F3")), "0", Trim(convert.tostring(rst.Fields("F3"))), 8)))
                                        lngPlanID = CLng(Trim(Mid(IIf(IsDBNull(convert.tostring(rst.Fields("F3")), "0", Trim(convert.tostring(rst.Fields("F3"))), 8, 6)))
                                    End If
                                ElseIf lngPatientID = 0 Then  'Reverify
                                    If Len(convert.tostring(rst.Fields("F3")) >= 11 And (Right(convert.tostring(rst.Fields("F3"), 1)) = "R" Or Right(convert.tostring(rst.Fields("F3"), 1)) = "V") Then  'PatientID, PlanID and Reverify Code 'R'
                                        If Right(Convert.ToString(rst.Fields("F3"), 1)) = "R" Then blnReverify = True
                                        If Right(convert.tostring(rst.Fields("F3"), 1) = "V" Then blnVerify = True
                                        If Len(convert.tostring(rst.Fields("F3")) <= 11 Then
                                            lngPatientID = CLng(Trim(Left(IIf(IsDBNull(convert.tostring(rst.Fields("F3")), "0", Trim(convert.tostring(rst.Fields("F3"))), 8)))
                                            lngPlanID = CLng(Trim(Mid(IIf(IsDBNull(convert.tostring(rst.Fields("F3")), "0", Trim(convert.tostring(rst.Fields("F3"))), 9, 6)))
                                            Else
                                            lngPatientID = CLng(Trim(Left(IIf(IsDBNull(convert.tostring(rst.Fields("F3")), "0", Trim(convert.tostring(rst.Fields("F3"))), 8)))
                                            lngPlanID = CLng(Trim(Mid(IIf(IsDBNull(convert.tostring(rst.Fields("F3")), "0", Trim(convert.tostring(rst.Fields("F3"))), 9, 6)))
                                            End If
                                        End If
                                    End If
                                    If lngPatientID > 0 Then
                                        strData = strData & "PatientID: " & lngPatientID & " PlanID: " & lngPlanID & Chr(10)
                                ElseIf convert.tostring(rst.Fields("F4").ToString > "" Then 'BatchID
                                    strData = strData & "BatchID: " & convert.tostring(rst.Fields("F4") & Chr(10)
                                    End If
                            ElseIf convert.tostring(rst.Fields("F2") = "1" And lngPatientID <> 0 Then
                                If IsNumeric(convert.tostring(rst.Fields("F3")) Then
                                    If Len(convert.tostring(rst.Fields("F3")) < 13 Or Len(convert.tostring(rst.Fields("F3")) > 15 Then
                                        If convert.tostring(rst.Fields("F4") > "" Then
                                            strData = strData & "COVERED BY ANOTHER INTERMEDIARY: " & convert.tostring(rst.Fields("F4") & Chr(10)
                                            End If
                                        End If
                                    End If
                                End If
                        Case "NM1"
                                'NM1*03*1*HILES*KATELYN*E~
                            If convert.tostring(rst.Fields("F2") = "03" Then  'Patient
                                If convert.tostring(rst.Fields("F3") = "1" Then  'Person
                                    PatientPolicyID = IIf(IsDBNull(convert.tostring(rst.Fields("F10")), InsuredPolicyID, Trim(convert.tostring(rst.Fields("F10")))
                                    PatientLastName = Replace(IIf(IsDBNull(convert.tostring(rst.Fields("F4")), "", Trim(convert.tostring(rst.Fields("F4"))), "'", "")
                                    PatientFirstName = Replace(IIf(IsDBNull(convert.tostring(rst.Fields("F5")), "", Trim(convert.tostring(rst.Fields("F5"))), "'", "")
                                    PatientMiddleInitial = IIf(IsDBNull(convert.tostring(rst.Fields("F6")), "", Trim(Mid(convert.tostring(rst.Fields("F6"), 1, 1)))
                                        strData = strData & "Dependent: " & PatientLastName & ", " & PatientFirstName & " " & PatientMiddleInitial & Chr(10)
                                        strData = strData & "Policy ID: " & PatientPolicyID & Chr(10)
                                    End If
                                Else
                                If convert.tostring(rst.Fields("F2") = "PR" Then  'Group
                                    If convert.tostring(rst.Fields("F3") = "2" Then  'Claims Mailing address
                                        strData = strData & "Claims Mailing address: " & Chr(10) & Trim(convert.tostring(rst.Fields("F4")) & Chr(10)
                                            rst.MoveNext()
                                        If Not rst.EOF Then RecID = convert.tostring(rst.Fields.Item("F1")
                                            If RecID = "N2" Then
                                            strData = strData & Trim(convert.tostring(rst.Fields("F2")) & Chr(10)
                                                rst.MoveNext()
                                            If Not rst.EOF Then RecID = convert.tostring(rst.Fields.Item("F1")
                                            End If
                                            If RecID = "N3" Then
                                            strData = strData & Trim(convert.tostring(rst.Fields("F2")) & Chr(10)
                                                rst.MoveNext()
                                            If Not rst.EOF Then RecID = convert.tostring(rst.Fields.Item("F1")
                                            End If
                                            If RecID = "N4" Then
                                            strData = strData & Trim(convert.tostring(rst.Fields("F2")) & ", " & Trim(convert.tostring(rst.Fields("F3")) & " " & Trim(convert.tostring(rst.Fields("F4")) & Chr(10)
                                            End If
                                        End If
                                    End If
                                End If
                        Case "DMG"
                                'DMG*D8*20010322*F~
                            If convert.tostring(rst.Fields("F2") = "D8" Then  'Date Of Birth, Sex
                                    PatientDOB = 0
                                PatientSex = IIf(IsDBNull(convert.tostring(rst.Fields("F4")), "", Trim(convert.tostring(rst.Fields("F4")))
                                If convert.tostring(rst.Fields("F3") > "" Then
                                    If Len(convert.tostring(rst.Fields("F3")) = 6 Then
                                        PatientDOB = DateSerial(CInt(Mid(convert.tostring(rst.Fields("F3"), 1, 2)) + 2000, CInt(Mid(convert.tostring(rst.Fields("F3"), 3, 2)), CInt(Mid(convert.tostring(rst.Fields("F3"), 5, 2)))
                                        Else
                                        PatientDOB = DateSerial(CInt(Mid(convert.tostring(rst.Fields("F3"), 1, 4)), CInt(Mid(convert.tostring(rst.Fields("F3"), 5, 2)), CInt(Mid(convert.tostring(rst.Fields("F3"), 7, 2)))
                                        End If
                                        strData = strData & strHLtype & " DOB: " & PatientDOB & " " & strHLtype & " Sex: " & PatientSex & Chr(10)
                                    Else
                                        strData = strData & strHLtype & " DOB: " & " " & strHLtype & " Sex: " & PatientSex & Chr(10)
                                    End If
                                End If
                        Case "INS"
                                strData = strData & "Responsible Party: "
                                'INS01 Insured Indicator - Y N
                            If convert.tostring(rst.Fields("F2") = "Y" Then strData = strData & "Insured "
                            If convert.tostring(rst.Fields("F2") = "N" Then strData = strData & "Dependent "
                                        ' “Y” value indicates the insured is a subscriber: an
                                        ' “N” value indicates a dependent.
                                        'INS02 Relationship Code
                            If convert.tostring(rst.Fields("F3") = "18" Then strData = strData & "Self "
                            If convert.tostring(rst.Fields("F3") = "1" Then strData = strData & "Spouse "
                            If convert.tostring(rst.Fields("F3") = "19" Then strData = strData & "Child "
                            If convert.tostring(rst.Fields("F3") = "20" Then strData = strData & "Employee "
                            If convert.tostring(rst.Fields("F3") = "21" Then strData = strData & "Unknown "
                            If convert.tostring(rst.Fields("F3") = "39" Then strData = strData & "Organ Donor "
                            If convert.tostring(rst.Fields("F3") = "40" Then strData = strData & "Cadaver Donor "
                            If convert.tostring(rst.Fields("F3") = "53" Then strData = strData & "Life Partner "
                            If convert.tostring(rst.Fields("F3") = "G8" Then strData = strData & "Other Relationship "
                                                                            'INS03 Maintenance Type Code
                            If convert.tostring(rst.Fields("F4") = "001" Then strData = strData & "Change "
                                                                                'INS04 Maintenance Reason Code
                            If convert.tostring(rst.Fields("F5") = "25" Then strData = strData & "Change in Identifying Data Elements "
                                                                                    strData = strData & Chr(10)
                        Case "EB"
                                                                                    'EB03 Service Type Code
                            If Trim(convert.tostring(rst.Fields("F4")) > "" Then
                                arr = Split(convert.tostring(rst.Fields("F4"), "^")
                                                                                        lngCtr = UBound(arr)
                                                                                        If lngCtr >= 0 Then
                                                                                            For RecordID = 0 To lngCtr
                                                                                                ErrMsg = ""
                                                                                                strSQL = "SELECT fldServiceTypeDesc FROM tblServiceTypeCode WHERE (fldServiceTypeCode ='" & arr(RecordID) & "');"
                                                                                                rstSQL = CreateObject("ADODB.Recordset")
                                                                                                rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                                                                                If Not rstSQL.EOF Then
                                                                                                    ErrMsg = Trim(IIf(IsDBNull(rstSQL.Fields("fldServiceTypeDesc")), "", rstSQL.Fields("fldServiceTypeDesc")))
                                                                                                    If lngCtr > 0 And RecordID <> lngCtr Then
                                                                                                        strData = strData + ErrMsg + "; "
                                                                                                    Else
                                                                                                        strData = strData + ErrMsg + " "
                                                                                                    End If
                                                                                                End If
                                                                                                rstSQL = Nothing
                                                                                            Next
                                                                                            strData = strData & Chr(10) & "     "
                                                                                        End If
                                                                                    End If
                                                                                    'EB*A*IND*CE***27**.2***Y*Y~
                                                                                    'EB*I**CE^CF^CG^CH^MH*********N~
                                                                                    '' '' '' '' '' '' '' ''dtp()
                                                                                    '' '' '' '' '' '' '' ''dpt()
                                                                                    '' '' '' '' '' '' '' ''amt()
                                                                                    '' '' '' '' '' '' '' ''Msg = 'befnits bla bla'
                                                                                    'EB12 In Plan Network benefits EB12 = Y,N,U, W
                            If Trim(convert.tostring(rst.Fields("F13")) > "" Then
                                Select Case Trim(convert.tostring(rst.Fields("F13"))
                                                                                            Case "Y"
                                                                                                strData = strData & "In-Network "
                                                                                            Case "N"
                                                                                                strData = strData & "Out-Of-Network "
                                                                                            Case "W"
                                                                                                strData = strData & "In-Network or Out-Of-Network "
                                                                                        End Select
                                                                                    End If
                                                                                    'EB01 - Eligibility or Benefit Information
                            Select Case Trim(convert.tostring(rst.Fields("F2"))
                                                                                        Case "1"
                                                                                            strData = strData & "Active Coverage "
                                                                                        Case "2"
                                                                                            strData = strData & "Active – Full Risk Capitation "
                                                                                        Case "6"
                                                                                            strData = strData & "Inactive " & Chr(10) : blnReject = True
                                                                                        Case "A"
                                                                                            strData = strData & "Member benefit has Co-Insurance "
                                                                                        Case "B"
                                                                                            strData = strData & "Co-Payment "
                                                                                        Case "C"
                                                                                            strData = strData & "Deductible "
                                                                                        Case "D"
                                                                                            strData = strData & "Benefit Description "
                                                                                        Case "E"
                                                                                            strData = strData & "Exclusions "
                                                                                        Case "F"
                                                                                            strData = strData & "Member has benefit level Limitations "
                                                                                        Case "G"
                                                                                            strData = strData & "Out of Pocket (Patient Liability Amount) "
                                                                                        Case "H"
                                                                                            strData = strData & "Unlimited "
                                                                                        Case "I"
                                                                                            strData = strData & "Non-Covered " : blnReject = True
                                                                                        Case "J"
                                                                                            strData = strData & "Cost Containment "
                                                                                        Case "L"
                                                                                            strData = strData & "Primary Care Provider "
                                                                                        Case "M"
                                                                                            strData = strData & "Pre-existing Condition "
                                                                                        Case "MC"
                                                                                            strData = strData & "Managed Care Coordinator "
                                                                                        Case "N"
                                                                                            strData = strData & "Services Restricted to Following Provider "
                                                                                        Case "O"
                                                                                            strData = strData & "Not Deemed a Medical Necessity " : blnReject = True
                                                                                        Case "P"
                                                                                            strData = strData & "Disclaimer "
                                                                                        Case "R"
                                                                                            strData = strData & "Additional Payer "
                                                                                        Case "U"
                                                                                            strData = strData & "Contact Following Entity for Eligibility or Benefit Information "
                                                                                        Case "V"
                                                                                            strData = strData & "Cannot Process " : blnReject = True
                                                                                        Case "W"
                                                                                            strData = strData & "Other Source of Data "
                                                                                        Case "X"
                                                                                            strData = strData & "Health Care Facility "
                                                                                        Case "Y"
                                                                                            strData = strData & "Spend Down "
                                                                                        Case "CB"
                                                                                            strData = strData & "Coverage Basis "
                                                                                    End Select

                                                                                    'EB02 Coverage Level -
                            If Trim(convert.tostring(rst.Fields("F3")) > "" Then
                                Select Case Trim(convert.tostring(rst.Fields("F3"))
                                                                                            Case "ECH"
                                                                                                strData = strData & "ECH "
                                                                                            Case "FAM"
                                                                                                strData = strData & "Family "
                                                                                            Case "IND"
                                                                                                strData = strData & "Individual "
                                                                                            Case Else
                                                                                                strData = strData & ""
                                                                                        End Select
                                                                                    End If

                                                                                    'EB04 Insurance Type code
                            If Trim(convert.tostring(rst.Fields("F5")) > "" Then
                                Select Case Trim(convert.tostring(rst.Fields("F5"))
                                                                                            Case "C1"
                                                                                                strData = strData & "Commercial "
                                                                                            Case "IN"
                                                                                                strData = strData & "Indemnity"
                                                                                            Case "HM"
                                                                                                strData = strData & "HMO "
                                                                                            Case "PR"
                                                                                                strData = strData & "PPO "
                                                                                            Case "MC"
                                                                                                strData = strData & "Medicaid "
                                                                                            Case "MB"
                                                                                                strData = strData & "Medicare "
                                                                                            Case "PS"
                                                                                                strData = strData & "Point of Service "
                                                                                        End Select
                                                                                    End If

                                                                                    'EB05 Plan Coverage Descriptioncode
                            If Trim(convert.tostring(rst.Fields("F6")) > "" Then
                                strData = strData & Trim(convert.tostring(rst.Fields("F6")) & " "
                                                                                    End If
                                                                                    'EB06 Used to specify that the value in field EB07 is the remaining balance
                            If Trim(convert.tostring(rst.Fields("F7")) > "" Then
                                Select Case Trim(convert.tostring(rst.Fields("F7"))
                                                                                            Case "23"
                                                                                                strData = strData & "per Calendar year "
                                                                                            Case "24"
                                                                                                strData = strData & "year to date "
                                                                                            Case "25"
                                                                                                strData = strData & "Contract "
                                                                                            Case "27"
                                                                                                strData = strData & "per Visit "
                                                                                            Case "29"
                                                                                                strData = strData & "Remaining balance "
                                                                                            Case "30"
                                                                                                strData = strData & "Exceeded "
                                                                                            Case "31"
                                                                                                strData = strData & "Not Exceeded "
                                                                                            Case "32"
                                                                                                strData = strData & "Lifetime "
                                                                                            Case "33"
                                                                                                strData = strData & "Lifetime Remaining "
                                                                                            Case "34"
                                                                                                strData = strData & "Month "
                                                                                            Case "35"
                                                                                                strData = strData & "Week "
                                                                                            Case "36"
                                                                                                strData = strData & "Admission "
                                                                                        End Select
                                                                                    End If

                                                                                    'EB07 Dollar amount remaining - Patient Responsibility
                            If Trim(convert.tostring(rst.Fields("F8")) > "" Then
                                If IsNumeric(Trim(convert.tostring(rst.Fields("F8"))) Then
                                    strData = strData & "amount: " & Format(Trim(convert.tostring(rst.Fields("F8")), "Dim r as new list(of string)")
                                                                                        Else
                                    strData = strData & "amount: " & Trim(convert.tostring(rst.Fields("F8")) & " "
                                                                                        End If
                                                                                    End If

                                                                                    'EB08 percent of co-insurance that applies to the member
                            If Trim(convert.tostring(rst.Fields("F9")) > "" Then
                                                                                        '         Msg = Format(CDbl(Trim(convert.tostring(rst.Fields("F9"))) * 100, "!@@")
                                strData = strData & "members co-insurance is " & Format(CDbl(Trim(convert.tostring(rst.Fields("F9"))) * 100, "!@@") & "% "
                                                                                    End If

                                                                                    'EB09
                            If Trim(convert.tostring(rst.Fields("F10")) > "" Then
                                Select Case Trim(convert.tostring(rst.Fields("F10"))
                                                                                            Case "P6"
                                                                                                strData = strData & "Number of visits: "
                                                                                        End Select
                                                                                    End If
                                                                                    'EB10 number of visits limitation
                            If Trim(convert.tostring(rst.Fields("F11")) > "" Then
                                strData = strData & "limitation " & Trim(convert.tostring(rst.Fields("F11")) & " "
                                                                                    End If

                                                                                    'EB11 Requires Pre Auth EB11 = Y,N,U
                            If Trim(convert.tostring(rst.Fields("F12")) > "" Then
                                Select Case Trim(convert.tostring(rst.Fields("F12"))
                                                                                            Case "Y"
                                                                                                strData = strData & "Authorization Required "
                                                                                            Case "N"
                                                                                                strData = strData & "Authorization Not Required "
                                                                                            Case "W"
                                                                                                strData = strData & "Authorization "
                                                                                        End Select
                                                                                    End If
                                                                                    'Code Name
                                                                                    'EB13 Procedure Code
                            If Trim(convert.tostring(rst.Fields("F14")) > "" Then
                                arr = Split(convert.tostring(rst.Fields("F14"), ":")
                                                                                        lngCtr = UBound(arr)
                                                                                        If lngCtr >= 0 Then
                                                                                            For RecordID = 0 To lngCtr
                                                                                                If Len(arr(RecordID)) >= 5 Then
                                                                                                    strData = strData & "for " & arr(RecordID) & "; "
                                                                                                End If
                                                                                            Next
                                                                                        End If
                                                                                    End If
                                                                                    'AD American Dental Association Codes
                                                                                    'CJ Current Procedural Terminology (CPT) Codes
                                                                                    'HC Health Care Financing Administration Common Procedural Coding System (HCPCS) Codes
                                                                                    'ID ICD-10-CM - Procedure
                                                                                    'IV Home Infusion EDI Coalition (HIEC) Product/Service Code
                                                                                    'N4 National Drug Code in 5-4-2 Format
                                                                                    'ZZ Mutually Defined


                                                                                    strData = strData & Chr(10)
                        Case "HSD"
                                                                                    'HSD01 specify the visits for Professional
                            Select Case Trim(convert.tostring(rst.Fields("F2"))
                                                                                        Case "VS"
                                                                                            strData = strData & "VISITS: "
                                                                                    End Select
                                                                                    'HSD02 number of visits
                            If Trim(convert.tostring(rst.Fields("F3")) > "" Then
                                strData = strData & Trim(convert.tostring(rst.Fields("F3")) & " "
                                                                                    End If
                                                                                    'HSD05 Time period
                            Select Case Trim(convert.tostring(rst.Fields("F6"))
                                                                                        Case "34"
                                                                                            strData = strData & "Month"
                                                                                    End Select
                                                                                    'HSD06 length of period
                                                                                    strData = strData & Chr(10)
                        Case "MSG"
                                                                                    'MSG01 Free Form Message Text
                            If Trim(convert.tostring(rst.Fields("F2")) > "" Then
                                strData = strData & Trim(convert.tostring(rst.Fields("F2")) & Chr(10)
                                                                                    End If
                        Case "PER"
                                                                                    'PER*IC**TE*8007292422~
                            Select Case Trim(convert.tostring(rst.Fields("F2"))
                                                                                        Case "IC"
                                                                                            strData = strData & "Contact "
                                                                                    End Select
                            If Trim(convert.tostring(rst.Fields("F3")) > "" Then
                                strData = strData & Trim(convert.tostring(rst.Fields("F3")) & " "
                                                                                    End If
                            Select Case Trim(convert.tostring(rst.Fields("F4"))
                                                                                        Case "ED"
                                                                                            strData = strData & "EDI Access Number: "
                                                                                        Case "EM"
                                                                                            strData = strData & "Email: "
                                                                                        Case "FX"
                                                                                            strData = strData & "Facsimile: "
                                                                                        Case "TE"
                                                                                            strData = strData & "Telephone: "
                                                                                        Case "UR"
                                                                                            strData = strData & "URL: "
                                                                                    End Select
                            If Trim(convert.tostring(rst.Fields("F5")) > "" Then
                                If Len(Trim(convert.tostring(rst.Fields("F5"))) = 10 Then
                                    strData = strData & Mid(Trim(convert.tostring(rst.Fields("F5")), 1, 3) & " " & Mid(Trim(convert.tostring(rst.Fields("F5")), 4, 3) & " " & Mid(Trim(convert.tostring(rst.Fields("F5")), 7, 4) & " "
                                                                                        Else
                                    strData = strData & Trim(convert.tostring(rst.Fields("F5")) & " "
                                                                                        End If
                                                                                    End If

                            Select Case Trim(convert.tostring(rst.Fields("F6"))
                                                                                        Case "ED"
                                                                                            strData = strData & "EDI Access Number: "
                                                                                        Case "EM"
                                                                                            strData = strData & "Email: "
                                                                                        Case "FX"
                                                                                            strData = strData & "Facsimile: "
                                                                                        Case "TE"
                                                                                            strData = strData & "Telephone: "
                                                                                        Case "UR"
                                                                                            strData = strData & "URL: "
                                                                                    End Select
                            If Trim(convert.tostring(rst.Fields("F7")) > "" Then
                                If Len(Trim(convert.tostring(rst.Fields("F7"))) = 10 Then
                                    strData = strData & Mid(Trim(convert.tostring(rst.Fields("F7")), 1, 3) & " " & Mid(Trim(convert.tostring(rst.Fields("F7")), 4, 3) & " " & Mid(Trim(convert.tostring(rst.Fields("F7")), 7, 4) & " "
                                                                                        Else
                                    strData = strData & Trim(convert.tostring(rst.Fields("F7")) & " "
                                                                                        End If
                                                                                    End If

                                                                                    strData = strData & Chr(10)
                        Case "REF"
                            Select Case Trim(convert.tostring(rst.Fields("F2"))
                                                                                        Case "18"
                                                                                            strData = strData & "Plan Number "
                                                                                        Case "1W"
                                                                                            strData = strData & "Member Identification Number "
                                                                                        Case "6P"
                                                                                            strData = strData & "Group Number "
                                                                                        Case "IG"
                                                                                            strData = strData & "Insurance Policy Number "
                                                                                        Case "1L"
                                                                                            strData = strData & "Group or Policy Number "
                                                                                    End Select
                            strData = strData & Trim(convert.tostring(rst.Fields("F3")) & Chr(10)
                        Case "NM1"
                                                                                    'NM1*PRP*2*BCBS MINNESOTA~
                                                                                    'NM101
                            Select Case Trim(convert.tostring(rst.Fields("F2"))
                                                                                        Case "13"
                                                                                            strData = strData & "Contracted Service Provider "
                                                                                        Case "1I"
                                                                                            strData = strData & "Preferred Provider Organization (PPO) "
                                                                                        Case "1P"
                                                                                            strData = strData & "Provider "
                                                                                        Case "2B"
                                                                                            strData = strData & "Third-Party Administrator "
                                                                                        Case "36"
                                                                                            strData = strData & "Employer "
                                                                                        Case "73"
                                                                                            strData = strData & "Other Physician "
                                                                                        Case "FA"
                                                                                            strData = strData & "Facility "
                                                                                        Case "GP"
                                                                                            strData = strData & "Gateway Provider "
                                                                                        Case "GW"
                                                                                            strData = strData & "Group "
                                                                                        Case "I3"
                                                                                            strData = strData & "Independent Physicians Association (IPA) "
                                                                                        Case "IL"
                                                                                            strData = strData & "Insured or Subscriber "
                                                                                        Case "LR"
                                                                                            strData = strData & "Legal Representative "
                                                                                        Case "OC"
                                                                                            strData = strData & "Origin Carrier "
                                                                                        Case "P3"
                                                                                            strData = strData & "Primary Care Provider "
                                                                                        Case "P4"
                                                                                            strData = strData & "Prior Insurance Carrier "
                                                                                        Case "P5"
                                                                                            strData = strData & "Plan Sponsor "
                                                                                        Case "PR"
                                                                                            strData = strData & "Payer "
                                                                                        Case "VN"
                                                                                            strData = strData & "Vendor "
                                                                                        Case "VY"
                                                                                            strData = strData & "Organization Completing Configuration Change "
                                                                                        Case "X3"
                                                                                            strData = strData & "Utilization Management Organization "
                                                                                        Case "Y2"
                                                                                            strData = strData & "Managed Care Organization "
                                                                                        Case "PRP"
                                                                                            strData = strData & "Primary Payer "
                                                                                        Case "SEP"
                                                                                            strData = strData & "Secondary Payer "
                                                                                        Case "TTP"
                                                                                            strData = strData & "Tertiary Payer "
                                                                                        Case "VER"
                                                                                            strData = strData & "Party Performing Verification "
                                                                                    End Select
                                                                                    'NM102
                                                                                    'NM103
                            If Trim(convert.tostring(rst.Fields("F3")) = 1 Then
                                strData = strData & Trim(convert.tostring(rst.Fields("F4")) & ", " & Trim(convert.tostring(rst.Fields("F5")) & " "
                                                                                    Else
                                strData = strData & Trim(convert.tostring(rst.Fields("F4")) & " "
                                                                                    End If
                                                                                    'NM108
                            Select Case Trim(convert.tostring(rst.Fields("F9"))
                                                                                        Case "24"
                                                                                            strData = strData & "Employers Identification Number: "
                                                                                        Case "34"
                                                                                            strData = strData & "Social Security Number: "
                                                                                        Case "46"
                                                                                            strData = strData & "Electronic Transmitter Identification Number(ETIN): "
                                                                                        Case "FA"
                                                                                            strData = strData & "Facility Identification: "
                                                                                        Case "FI"
                                                                                            strData = strData & "Federal EIN: "
                                                                                        Case "II"
                                                                                            strData = strData & "Standard Unique Health Identifier for each Individual in the United States: "
                                                                                        Case "MI"
                                                                                            strData = strData & "Member Identification Number: "
                                                                                        Case "NI"
                                                                                            strData = strData & "National Association of Insurance Commissioners (NAIC) Identification: "
                                                                                        Case "PI"
                                                                                            strData = strData & "Payor Identification: "
                                                                                        Case "PP"
                                                                                            strData = strData & "Pharmacy Processor Number: "
                                                                                        Case "SV"
                                                                                            strData = strData & "Service Provider Number: "
                                                                                        Case "XV"
                                                                                            strData = strData & "Centers for Medicare and Medicaid Services PlanID: "
                                                                                        Case "XX"
                                                                                            strData = strData & "Centers for Medicare and Medicaid Services National Provider Identifier: "
                                                                                    End Select
                                                                                    'NM109
                            strData = strData & Trim(convert.tostring(rst.Fields("F10")) & " "
                                                                                    'NM110 Entity Relationship Code
                            Select Case Trim(convert.tostring(rst.Fields("F11"))
                                                                                        Case "01"
                                                                                            strData = strData & "Parent "
                                                                                        Case "2"
                                                                                            strData = strData & "Child "
                                                                                        Case "27"
                                                                                            strData = strData & "Domestic Partner "
                                                                                        Case "41"
                                                                                            strData = strData & "Spouse "
                                                                                        Case "48"
                                                                                            strData = strData & "Employee "
                                                                                        Case "65"
                                                                                            strData = strData & "Other "
                                                                                        Case "72 Unknown"
                                                                                    End Select
                                                                                    strData = strData & Chr(10)
                    End Select
                    rst.MoveNext()
                    If Not rst.EOF Then RecID = convert.tostring(rst.Fields.Item("F1")
                Loop

            End If
            ''' Debug.Print (strData)
            'Write to Verification record.
            If Trim(strData) > "" And lngPatientID > 0 Then
                objBenefit = CreateObject("BenefactorBZ.CPatRPPlanBZ")
                rstTx = objBenefit.FetchRPPlansByPat(lngPatientID, True)
                objBenefit = Nothing
                intID = 0

                For lngCtr = 1 To rstTx.RecordCount
                    If rstTx.Fields("fldPatientID").Value = lngPatientID Then
                        If lngProviderID = 0 Then lngProviderID = rstTx.Fields("fldOwnerID").Value
                        If blnReverify Or blnVerify Then
                            If rstTx.Fields("fldPlanID").Value = lngPlanID Then intID = rstTx.Fields("fldPatRPPlanID").Value
                        Else
                            If rstTx.Fields("fldVerPlanID").Value = lngPlanID Then intID = rstTx.Fields("fldPatRPPlanID").Value
                        End If
                        If PatientFirstName = "" Then PatientFirstName = rstTx.Fields("fldFirst").Value
                        If PatientLastName = "" Then PatientLastName = rstTx.Fields("fldLast").Value
                    End If
                    rstTx.MoveNext()
                Next lngCtr
                rstTx = Nothing

                If intID = 0 Then
                    objBenefit = CreateObject("BenefactorBZ.CPatRPPlanBZ")
                    rstTx = objBenefit.FetchRPPlansByPat(lngPatientID, False)
                    objBenefit = Nothing
                    intID = 0

                    For lngCtr = 1 To rstTx.RecordCount
                        If rstTx.Fields("fldPatientID").Value = lngPatientID Then
                            If rstTx.Fields("fldPlanID").Value = lngPlanID Then intID = rstTx.Fields("fldPatRPPlanID").Value
                            If rstTx.Fields("fldVerPlanID").Value = lngPlanID Then intID = rstTx.Fields("fldPatRPPlanID").Value
                            If PatientFirstName = "" Then PatientFirstName = rstTx.Fields("fldFirst").Value
                            If PatientLastName = "" Then PatientLastName = rstTx.Fields("fldLast").Value
                        End If
                        rstTx.MoveNext()
                    Next lngCtr
                    rstTx = Nothing
                End If

                If intID = 0 Then
                    objBenefit = CreateObject("BenefactorBZ.CPatRPPlanBZ")
                    rstTx = objBenefit.FetchRPPlansByPat(lngPatientID, True)
                    objBenefit = Nothing
                    intID = 0

                    For lngCtr = 1 To rstTx.RecordCount
                        If rstTx.Fields("fldPatientID").Value = lngPatientID Then
                            If rstTx.Fields("fldPlanID").Value = lngPlanID Then intID = rstTx.Fields("fldPatRPPlanID").Value
                            If rstTx.Fields("fldVerPlanID").Value = lngPlanID Then intID = rstTx.Fields("fldPatRPPlanID").Value
                            If PatientFirstName = "" Then PatientFirstName = rstTx.Fields("fldFirst").Value
                            If PatientLastName = "" Then PatientLastName = rstTx.Fields("fldLast").Value
                        End If
                        rstTx.MoveNext()
                    Next lngCtr
                    rstTx = Nothing
                End If

                If intID = 0 Then
                    lngInsuranceID = 0
                    strSQL = "SELECT fldInsuranceID " & _
                             "FROM (tblPlan AS A INNER JOIN tblClaimsProcCtr AS B ON A.fldCPCID = B.fldCPCID) " & _
                             "WHERE (A.fldPlanID=" & lngPlanID & ");"
                    rstSQL = CreateObject("ADODB.Recordset")
                    rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                    lngInsuranceID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldInsuranceID"))
                    rstSQL = Nothing
                    If lngInsuranceID > 0 And lngPlanID > 0 Then
                        objBenefit = CreateObject("BenefactorBZ.CPatRPPlanBZ")
                        rstTx = objBenefit.FetchRPPlansByPat(lngPatientID, True)
                        objBenefit = Nothing
                        For lngCtr = 1 To rstTx.RecordCount
                            If rstTx.Fields("fldPatientID").Value = lngPatientID And intID = 0 Then
                                If rstTx.Fields("fldPlanID").Value = lngPlanID Then intID = rstTx.Fields("fldPatRPPlanID").Value
                                If rstTx.Fields("fldVerPlanID").Value = lngPlanID Then intID = rstTx.Fields("fldPatRPPlanID").Value
                                If blnReverify Or blnVerify Then
                                    If rstTx.Fields("fldOrder").Value > 0 And rstTx.Fields("fldInsuranceID").Value = lngInsuranceID Then intID = rstTx.Fields("fldPatRPPlanID").Value
                                Else
                                    strSQL = "SELECT fldInsuranceID " & _
                                             "FROM (tblPlan AS A INNER JOIN tblClaimsProcCtr AS B ON A.fldCPCID = B.fldCPCID) " & _
                                             "WHERE (A.fldPlanID=" & rstTx.Fields("fldVerPlanID").Value & ");"
                                    rstSQL = CreateObject("ADODB.Recordset")
                                    rstSQL.Open(strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
                                    If lngInsuranceID = IIf(rstSQL.EOF, 0, rstSQL.Fields("fldInsuranceID")) Then
                                        intID = rstTx.Fields("fldPatRPPlanID").Value
                                    End If
                                    rstSQL = Nothing
                                End If
                                If PatientFirstName = "" Then PatientFirstName = rstTx.Fields("fldFirst").Value
                                If PatientLastName = "" Then PatientLastName = rstTx.Fields("fldLast").Value
                            End If
                            rstTx.MoveNext()
                        Next lngCtr
                        rstTx = Nothing
                    End If
                End If

                If intID > 0 Then
                    objPatBenefit = CreateObject("BenefitsBz.CPatientBenefitBZ")
                    If blnReject Then
                        objPatBenefit.UpdateVerify(intID, "", Left(InsuredPolicyID, 30), Left(InsuredGroupNbr, 30), "", Left(strPayorID, 6), 0, 0, 0, 0, 0, 0, strData, "Y", "N", "N", 0, 0, "", GetLoginName())
                    Else
                        objPatBenefit.UpdateVerify(intID, "", Left(InsuredPolicyID, 30), Left(InsuredGroupNbr, 30), "", Left(strPayorID, 6), 0, 0, 0, 0, 0, 0, strData, "N", "N", "N", 0, 0, "", GetLoginName())
                    End If
                    objPatBenefit = Nothing

                    If lngProviderID > 0 And (blnReverify Or blnVerify) Then
                        objUser = CreateObject("ClinicBz.CUserBz")
                        rstMsg = objUser.FetchManagersByProvider(lngProviderID)
                        objUser = Nothing

                        objMsg = CreateObject("ListBZ.CMsgBz")

                        If Not rstMsg.BOF And Not rstMsg.EOF Then
                            While Not rstMsg.EOF
                                If rstMsg.Fields("fldDisabledYN").Value = "N" And rstMsg.Fields("fldPsyquelVBYN").Value = "Y" And IsNumeric(Mid(rstMsg.Fields("fldUserName").Value, 6, 1)) Then
                                    If blnReject Then
                                        intID = objMsg.Insert(Left("Your patient <a href='PatientSelect.asp?ID=" & lngPatientID & "'>" & Trim(PatientFirstName) & " " & Trim(PatientLastName) & "</a> Verification has been Electroniy Rejected.  " & HealthPlanName, 180), "system", rstMsg.Fields("fldUserID").Value)
                                    Else
                                        intID = objMsg.Insert(Left("Your patient <a href='PatientSelect.asp?ID=" & lngPatientID & "'>" & Trim(PatientFirstName) & " " & Trim(PatientLastName) & "</a> has been Electroniy Verified.  " & HealthPlanName, 180), "system", rstMsg.Fields("fldUserID").Value)
                                    End If
                                End If
                                rstMsg.MoveNext()
                            End While
                        End If
                        rstMsg = Nothing
                        If blnReject Then
                            intID = objMsg.Insert(Left("Your patient <a href='PatientSelect.asp?ID=" & lngPatientID & "'>" & Trim(PatientFirstName) & " " & Trim(PatientLastName) & "</a> Verification has been Electroniy Rejected.  " & HealthPlanName, 180), "system", lngProviderID)
                        Else
                            intID = objMsg.Insert(Left("Your patient <a href='PatientSelect.asp?ID=" & lngPatientID & "'>" & Trim(PatientFirstName) & " " & Trim(PatientLastName) & "</a> has been Electroniy Verified.  " & HealthPlanName, 180), "system", lngProviderID)
                        End If
                    End If
                End If
            End If
            strData = strReceive : lngPatientID = 0
            If Not rst.EOF And RecID <> "HL" Then rst.MoveNext()
            If Not rst.EOF Then RecID = convert.tostring(rst.Fields.Item("F1")
        Loop
        rst.Close()
        'delete 271 file from ElectronicClaims Folder
        If Dir(inFile) > "" Then
            If Dir(Mid(inFile, 1, InStr(1, inFile, "\271\") + 4) & "Archive\" & Mid(inFile, InStr(1, inFile, "\271\") + 5)) > "" Then
                fs.DeleteFile(Mid(inFile, 1, InStr(1, inFile, "\271\") + 4) & "Archive\" & Mid(inFile, InStr(1, inFile, "\271\") + 5))
            End If
            fs.MoveFile(inFile, Mid(inFile, 1, InStr(1, inFile, "\271\") + 4) & "Archive\" & Mid(inFile, InStr(1, inFile, "\271\") + 5))
            fs.DeleteFile(curFile)
        End If
        Exit Function

ErrTrap:
        rst.Close()
        Err.Raise(Err.Number, Err.Source, Err.Description & " : " & inFile & " : " & lngPatientID & " : " & PatientLastName)
    End Function



End Class














