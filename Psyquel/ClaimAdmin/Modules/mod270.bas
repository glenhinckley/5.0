Attribute VB_Name = "mod270"
Option Explicit

Private g_lngNumSeg As Long 'Data segment counter
Private g_lngEndTxNum As Long
Private g_FileNumber As Long
Private g_FileName As String
Private g_blnLineFeed As Boolean
Private g_Allowed As Double
Private g_CheckDate As Date
Private HL_01, HL_Parent As Long
Private g_ElementSeperator As String
Private g_PrtString As String
Private g_bln5010 As Boolean

Public Function GenerateVerificationRequest(ByVal rstClrHse As ADODB.Recordset, ByVal rst As ADODB.Recordset, ByVal lngEndTxNum As Long, _
                                         ByVal lngFileNumber As Long, ByVal strFileName As String) As Long
'-------------------------------------------------------------------------------
'Author: Duane C Orth
'Date: 06/10/2005
'Description: Main function that generates EDI text file compliant to X.12 276 v4010
'Parameters: rst - Recordset of Claims Status Request to be electronically generated
'Returns: Null
'-------------------------------------------------------------------------------

   On Error GoTo ErrTrap:
    
   Dim strPayerCode As String
   Dim strPayerCodeTmp As String
   Dim lngPrevClrHseID As Long
   Dim GE_01 As Long
   Dim dtDOB As Date
    
   g_lngEndTxNum = lngEndTxNum
   g_FileNumber = lngFileNumber
   g_FileName = strFileName
   HL_01 = 0: HL_Parent = 0
    
   Screen.MousePointer = vbHourglass
   GE_01 = 1
    
   If rstClrHse.Fields("fldUseCrLfYN").Value = "Y" Then
        g_blnLineFeed = True
   Else
        g_blnLineFeed = False
   End If
    
   If rst.RecordCount > 0 Then
    
        If rst.Fields("fldClearingHouseID").Value <> 66 Then lngPrevClrHseID = rst.Fields("fldClearingHouseID").Value
            
        Call WriteISAHeader(rstClrHse, rst)
        Call WriteGroupHeader(rstClrHse, rst)
        Call WriteTxSetHeader(rstClrHse)
        
        Do While Not rst.EOF
            strPayerCode = IfNull(rst.Fields("fldPayerCodeVerify").Value, IfNull(rst.Fields("fldPayerCode").Value, ""))
            
            'Call WriteVerification(rstClrHse, rst, strPayerCode)
            
            Call WriteLoop2000A                             'Heirarchy level (Payer)
            Call WriteLoop2100A(rstClrHse, rst, strPayerCode)    'Receiver information
    
            Call WriteLoop2000B                             'Heirarchy level (Receiver)
            Call WriteLoop2100B(rstClrHse, rst, strPayerCode)    'Receiver information
 '          Call WriteLoop2120C(rst, strPayerCode)         'Subscriber Benefit-Related Entity Name
    
            Call WriteLoop2000C(rstClrHse, rst)                        'Heirarchy level (Subscriber)
            Call WriteLoop2100C(rstClrHse, rst, strPayerCode)          'Subscriber Name Segment
    
            'Medicare and Medicaid do not allow any relationship but self
            dtDOB = IfNull(rst.Fields("fldInsdDOB").Value, 0)
            If rst.Fields("fldPatientID").Value <> rst.Fields("fldRPID").Value And _
               rst.Fields("fldInsType").Value <> "MP" And _
               rst.Fields("fldInsType").Value <> "MB" And _
               rst.Fields("fldInsType").Value <> "MC" And _
               (IsDate(dtDOB) And dtDOB > 0) Then
                'Patient information.  This loop is written only if the patient is not the
                'insured/subscriber.  Otherwise the patient and insured/subscriber information
                'is identical and has already been written in Loop 2010BA.
                Call WriteLoop2000D(rstClrHse, rst)                        'Heirarchy level (Dependent Level)
                Call WriteLoop2100D(rstClrHse, rst)                        'Dependent Name
                Call WriteLoop2110D(rst)                        'Dependent Eligibility or Benefit Inquiry
  '      Call WriteLoop2200D(rstClrHse, rst)                        'Claim Submitter Trace number
  '  Call WriteLoop2210D(rst) 'Service information
            Else
                Call WriteLoop2110C(rst)                        'Service Type Code
            End If
            Call UpdateFileID(rst.Fields("fldPatRPPlanID").Value, rst.Fields("fldVerPlanID").Value)
            
            g_lngNumClaims = g_lngNumClaims + 1
            
            If Not rst.EOF Then
                rst.MoveNext
             '   HL_01 = 0: HL_Parent = 0
            Else
                Call WriteTxSetTrailer
                Call WriteGroupTrailer(GE_01)
                Call WriteISATrailer
                g_lngEndTxNum = g_lngEndTxNum + 1 'According to THIN, each BHT and ST/SE must have a unique identifier
                Exit Do
            End If
            
            If Not rst.EOF Then
                If lngPrevClrHseID <> rst.Fields("fldClearingHouseID").Value Then
                    Call WriteTxSetTrailer
                    Call WriteGroupTrailer(GE_01)
                    Call WriteISATrailer
                    g_lngEndTxNum = g_lngEndTxNum + 1
                    Exit Do
                End If
                If strPayerCode <> IfNull(rst.Fields("fldPayerCodeVerify").Value, IfNull(rst.Fields("fldPayerCode").Value, "")) Then
                     Call WriteTxSetTrailer
                     HL_01 = 0: HL_Parent = 0: g_lngNumSeg = 0
                     GE_01 = GE_01 + 1
               '     Call WriteGroupTrailer(GE_01)
               '     Call WriteISATrailer
                    g_lngEndTxNum = g_lngEndTxNum + 1
                     Call WriteTxSetHeader(rstClrHse)
               '     Exit Do
                End If
            Else
                Call WriteTxSetTrailer
                Call WriteGroupTrailer(GE_01)
                Call WriteISATrailer
                g_lngEndTxNum = g_lngEndTxNum + 1
                Exit Do
            End If
        Loop
        
        Set rst = Nothing
    End If
            
    'Signal completion
    'Call MsgBox(lngNumClaims & " claims have been placed into file " & rstClrHse.Fields("fldFolder").Value & "\" & strFileName, vbOKOnly + vbExclamation, "Complete")

    'Return the last Transaction number to calling procedure
    GenerateVerificationRequest = g_lngEndTxNum
    
    Screen.MousePointer = vbNormal
    
    Exit Function

ErrTrap:
    Screen.MousePointer = vbNormal
    GenerateVerificationRequest = -1
    MsgBox "Error: " & Err.Number & " - " & Err.Description, vbCritical, "Error in file export"
    
End Function


Private Sub WriteISAHeader(ByVal rstClrHse As ADODB.Recordset, ByVal rst As ADODB.Recordset)
'Writes ISA (Interchange Control Header) for file

    Dim strISA As String
    Dim strISA01, strISA02, strISA03, strISA04, strISA05 As String
    Dim strISA06, strISA07, strISA08, strISA09, strISA10 As String
    Dim strISA11, strISA12, strISA13, strISA14, strISA15, strISA16 As String
    Dim dteCreateDate As Date
    
    On Error GoTo ErrTrap:
    
    dteCreateDate = Now
     
    'ISA (Interchange Control Header)
    'Note: The ISA header is a fixed length segment
    strISA01 = Format(rstClrHse.Fields("fldISA01").Value, "!@@")                    'Security information qualifier
    strISA02 = Format(IfNull(rstClrHse.Fields("fldISA02").Value, " "), "!@@@@@@@@@@")            'Security information
    strISA03 = Format(rstClrHse.Fields("fldISA03").Value, "!@@")                                                          'Security information qualifier
    strISA04 = Format(IfNull(rstClrHse.Fields("fldISA04").Value, " "), "!@@@@@@@@@@")           'Security information
    strISA05 = Format(rstClrHse.Fields("fldISA05").Value, "!@@")                    'interchange ID qualifier (ZZ = mutually defined)
    strISA06 = Format(IfNull(rstClrHse.Fields("fldISA06").Value, " "), "!@@@@@@@@@@@@@@@")       'interchange sender ID (length must be 15)
    strISA07 = Format(rstClrHse.Fields("fldISA07").Value, "!@@")                    'interchange ID qualifier (ZZ = mutually defined)
    
    If IfNull(rstClrHse.Fields("fldReceiverUsePayerYN").Value, "N") = "N" Then
        strISA08 = Format(IfNull(rstClrHse.Fields("fldISA08").Value, " "), "!@@@@@@@@@@@@@@@")
    Else
        If rstClrHse.Fields("fldReceiverPrefixYN").Value = "Y" Then
            strISA08 = Format(CreatePayerCodePrefix(rst.Fields("fldInsType").Value, IfNull(rst.Fields("fldPayerCodeVerify").Value, IfNull(rst.Fields("fldPayerCode").Value, ""))) & IfNull(rst.Fields("fldPayerCodeVerify").Value, IfNull(rst.Fields("fldPayerCode").Value, "")), "!@@@@@@@@@@@@@@@")
        Else
            strISA08 = Format(IfNull(rst.Fields("fldPayerCodeVerify").Value, IfNull(rst.Fields("fldPayerCode").Value, "")), "!@@@@@@@@@@@@@@@")
        End If
    End If
    
    strISA09 = Format(dteCreateDate, "yymmdd")                'interchange date
    strISA10 = Format(Now(), "HHMM")                          'interchange time
    strISA11 = "^"                                            'interchange control standards identifier
    strISA12 = rstClrHse.Fields("fldVersion").Value           'interchange control version number
    strISA13 = Format(g_FileNumber, "000000000")              'interchange control number
    strISA14 = 0  'rstClrHse.Fields("fldISA14").Value    'acknowledgment requested (0 = no ack requested; 1 ack requested)
    
    'usage indicator (P = production data / T = test data)
    If frmEDIWizard.chkTest.Value = vbChecked Then
        strISA15 = "T"
    Else
        strISA15 = "P"
    End If
    
    strISA16 = ":"                'component element separator
    
    strISA = "ISA*" & strISA01 & "*" & strISA02 & "*" & strISA03 & "*" & strISA04 _
      & "*" & strISA05 & "*" & strISA06 & "*" & strISA07 & "*" & strISA08 & "*" & strISA09 _
      & "*" & strISA10 & "*" & strISA11 & "*" & strISA12 & "*" & strISA13 & "*" & strISA14 & "*" & strISA15 & "*" & strISA16 & "~"
    
    If g_blnLineFeed Then Print #1, strISA Else Print #1, strISA;
    
    Exit Sub
    
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub

Private Sub WriteGroupHeader(ByVal rstClrHse As ADODB.Recordset, ByVal rst As ADODB.Recordset)
'Writes functional Group Header to EDI file

    Dim strGS As String
    Dim strGS01, strGS02, strGS03, strGS04 As String
    Dim strGS05, strGS06, strGS07, strGS08 As String
    Dim dteCreateDate As Date
    
    On Error GoTo ErrTrap:
    
    dteCreateDate = Now
    
    'GS (Functional Group Header)
    strGS01 = "HS"                                          'HS = Eligibility, Coverage or Benefit Inquiry
    strGS02 = rstClrHse.Fields("fldGS02").Value             'application sender's code
    
    If IfNull(rstClrHse.Fields("fldReceiverUsePayerYN").Value, "N") = "N" Then
         strGS03 = rstClrHse.Fields("fldGS03").Value
    Else
        If rstClrHse.Fields("fldReceiverPrefixYN").Value = "Y" Then
            strGS03 = CreatePayerCodePrefix(rst.Fields("fldInsType").Value, IfNull(rst.Fields("fldPayerCodeVerify").Value, IfNull(rst.Fields("fldPayerCode").Value, ""))) & IfNull(rst.Fields("fldPayerCodeVerify").Value, IfNull(rst.Fields("fldPayerCode").Value, ""))
        Else
            strGS03 = IfNull(rst.Fields("fldPayerCodeVerify").Value, IfNull(rst.Fields("fldPayerCode").Value, ""))
        End If
    End If
    
    If rst.Fields("fldClearingHouseID").Value = 92 Then 'BcBs of Hawaii HMSA
        strGS02 = "BACC"
        strGS03 = "BHAWA"
    End If
   
    strGS04 = Format(dteCreateDate, "yyyymmdd")             'date
    strGS05 = Format(dteCreateDate, "hhnn")                 'time
    strGS06 = g_FileNumber                                  'group control number
    strGS07 = "X"                                           'responsible agency code
    strGS08 = "005010X279A1"    'rstClrHse.Fields("fldRelease").Value          'version/release/industry identifier code
    
    strGS = "GS*" & strGS01 & "*" & strGS02 & "*" & strGS03 & "*" & strGS04 _
         & "*" & strGS05 & "*" & strGS06 & "*" & strGS07 & "*" & strGS08 & "~"

    If g_blnLineFeed Then Print #1, strGS Else Print #1, strGS;

    Exit Sub
    
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub

Private Sub WriteTxSetHeader(ByVal rstClrHse As ADODB.Recordset)
'Writes header information for a claim. This method writes the ST, BHT, and REF
'data segments of the header.

    On Error GoTo ErrTrap:

    Dim strST As String
    Dim strST01, strST02, strST03 As String
    Dim strBHT As String
    Dim strBHT01, strBHT02, strBHT03, strBHT04, strBHT05, strBHT06 As String
    Dim strRef As String
    Dim dteCreateDate As Date
    
    'ST (transaction set header)
    strST01 = "270"  'transaction set identifier code
    strST02 = Format(CStr(g_lngEndTxNum), "00000000") 'transaction set control number
'    strST03 = rstClrHse.Fields("fldRelease").Value
    strST03 = "005010X279A1"
    
    strST = "ST*" & strST01 & "*" & strST02 & "*" & strST03 & "~"
    
    'BHT (beginning of hierarchical transaction)
    dteCreateDate = Now()
    
    strBHT01 = "0022"                               'hierarchical structure code
    strBHT02 = "13"                                 'transaction set purpose code
    strBHT03 = Format(g_lngEndTxNum, "000000")      'reference identification
    strBHT04 = Format(dteCreateDate, "yyyymmdd")    'date
    strBHT05 = Format(dteCreateDate, "hhnn")        'time
    strBHT06 = ""                                 'transaction type code CH = Chargable, RP = Reporting, RT Spend Down
    
    If strBHT06 > "" Then
        strBHT = "BHT*" & strBHT01 & "*" & strBHT02 & "*" & strBHT03 _
         & "*" & strBHT04 & "*" & strBHT05 & "*" & strBHT06 & "~"
    Else
        strBHT = "BHT*" & strBHT01 & "*" & strBHT02 & "*" & strBHT03 _
         & "*" & strBHT04 & "*" & strBHT05 & "~"
    End If
    
    'REF (transmission type identification) 'Note: when in production mode value is 004010X098;
 '   strRef = "REF*87*" & rstClrHse.Fields("fldRelease").Value & "~"

    If g_blnLineFeed Then Print #1, strST Else Print #1, strST;
    g_lngNumSeg = g_lngNumSeg + 1 'increment segment counter

    If g_blnLineFeed Then Print #1, strBHT Else Print #1, strBHT;
    g_lngNumSeg = g_lngNumSeg + 1

 '   If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
 '   g_lngNumSeg = g_lngNumSeg + 1

    Exit Sub
    
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub
Private Sub WriteISATrailer()
    
    On Error GoTo ErrTrap:
    
    'ISA footer
    Dim strIEA As String
    strIEA = "IEA*1*" & Format(g_FileNumber, "000000000") & "~"

    If g_blnLineFeed Then Print #1, strIEA Else Print #1, strIEA;
    
    Exit Sub
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub

Private Sub WriteGroupTrailer(ByVal intNumClaims As Integer)

    On Error GoTo ErrTrap:
    
    'GS footer
    Dim strGE As String

    strGE = "GE*" & CStr(intNumClaims) & "*" & CStr(g_FileNumber) & "~"
    
    If g_blnLineFeed Then Print #1, strGE Else Print #1, strGE;
    
    Exit Sub
    
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub

Private Sub WriteTxSetTrailer()
    
    'SE segment
    Dim strSE As String
        
    On Error GoTo ErrTrap:
    
    'Increment segment counter one more time - segment count includes the 'SE' (trailer) segment
    g_lngNumSeg = g_lngNumSeg + 1
    
    strSE = "SE*" & g_lngNumSeg & "*" & Format(CStr(g_lngEndTxNum), "00000000") & "~"
    
    If g_blnLineFeed Then Print #1, strSE Else Print #1, strSE;
    
    g_lngNumSeg = 0
    
    Exit Sub
        
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description
        
End Sub

Private Sub WriteLoop2000A()
'Writes heirarchy level (this is always level 1) and specialty information (mental
'health claims)

    Dim strHL, strHL01, strHL02, strHL03, strHL04 As String
    
    HL_01 = HL_01 + 1
    strHL01 = Format(HL_01, "##0") 'hierarchical ID number (see pg. 77 of X098.pdf)
    strHL02 = ""               'hierarchical parent ID
    strHL03 = "20"             'hierarchical level code(20 = information source)
    strHL04 = "1"              'hierarchical child code
    HL_Parent = HL_01
    
    strHL = "HL*" & strHL01 & "*" & strHL02 & "*" & strHL03 & "*" & strHL04 & "~"
    
    If g_blnLineFeed Then Print #1, strHL Else Print #1, strHL;
    g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
    
End Sub

Private Function WriteLoop2100A(ByVal rstClrHse As ADODB.Recordset, ByVal rst As ADODB.Recordset, ByVal strPayerCode As String)
'Writes Submitter information

    Dim strNM101, strNM102, strNM103, strNM104, strNM105 As String
    Dim strNM106, strNM107, strNM108, strNM109, strNM110, strNM111 As String
    Dim strNM1, strN2 As String
    Dim strPer As String
    Dim strPer01, strPer02, strPer03, strPer04, strPer05, strPer06 As String
     
    'Submitter Name
    strNM101 = "PR"         'entity identifier code (41 = submitter)
    strNM102 = "2"          'Qualifier - 2:Non-person entity
    strNM103 = UCase(IfNull(rst.Fields("fldPlanName").Value, rst.Fields("fldCPCName").Value))    'Plan name
    strNM104 = ""           'Individual first name
    strNM105 = ""           'Individual last name
    strNM106 = ""                                           'not used (name prefix)
    strNM107 = ""                                           'not used (name suffix)
    strNM108 = "PI"                                         'identification code qualifier (46 = ETIN)
    strNM109 = UCase(IfNull(strPayerCode, IfNull(rst.Fields("fldPayerCodeVerify").Value, IfNull(rst.Fields("fldPayerCode").Value, ""))))      'Payer Code
        
    If rstClrHse.Fields("fldClearingHouseID").Value = 92 Then    'BcBs of Hawaii HMSA
        strNM103 = "HAWAII MEDICAL SERVICE ASSOCIATION"
        strNM108 = "FI"                                         'identification code qualifier (46 = ETIN)
        strNM109 = rstClrHse.Fields("fldReceiverID").Value
    End If
    
    strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 & "*" & strNM104 _
         & "*" & strNM105 & "*" & strNM106 & "*" & strNM107 & "*" & strNM108 & "*" & strNM109 & "~"
    
    If g_blnLineFeed Then Print #1, UCase(strNM1) Else Print #1, UCase(strNM1);
    g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
    
    Exit Function
    
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Private Function WriteLoop2000B()
'information receiver

    Dim strHL, strHL01, strHL02, strHL03, strHL04 As String
    Dim strPat As String
    
    On Error GoTo ErrTrap:
    
    HL_01 = HL_01 + 1
    strHL01 = Format(HL_01, "##0")      'hierarchical ID number - always 3
    strHL02 = Format(HL_Parent, "##0")  'hierarchical parent ID
    strHL03 = "21"                      'hierarchical level code (21 = information receiver)
    strHL04 = "1"                       'hierarchical child code
    HL_Parent = HL_01
    
    strHL = "HL*" & strHL01 & "*" & strHL02 & "*" & strHL03 & "*" & strHL04 & "~"
    If g_blnLineFeed Then Print #1, strHL Else Print #1, strHL;
    g_lngNumSeg = g_lngNumSeg + 1
    
    Exit Function
    
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description
End Function

Private Function WriteLoop2100B(ByVal rstClrHse As ADODB.Recordset, ByVal rst As ADODB.Recordset, ByVal strPayerCode As String)
'Writes Billing Provider information
    
   Dim strNM101, strNM102, strNM103, strNM104, strNM105 As String
   Dim strNM106, strNM107, strNM108, strNM109, strNM110, strNM111 As String
   Dim strNM1, strN2, strN3, strN4 As String
   Dim strN401, strN402, strN403, strN404 As String
   Dim strRef, strRef01, strRef02 As String
   Dim strPRV As String
   Dim strPRV01, strPRV02, strPRV03, strPRV04 As String
    
   On Error GoTo ErrTrap:

   strNM101 = "1P"        'entity code identifier (85 - billing provider)
   strNM102 = "1"         'entity type qualifier (1=person; 2=non-person)

   If IfNull(rst.Fields("fldGroupNPI").Value, "") > "" Or _
      IfNull(rst.Fields("fldSuperNPI").Value, IfNull(rst.Fields("fldProviderNPI").Value, "")) > "" Then
      If Not IsNull(rst.Fields("fldSupervisorID").Value) Then
            strNM103 = Trim(StripDelimiters(IfNull(rst.Fields("fldSuperLastName").Value, Mid(Replace(rst.Fields("fldProviderLastName").Value, "'", ""), 1, 35))))     'Last name
            strNM104 = Trim(StripDelimiters(IfNull(rst.Fields("fldSuperFirstName").Value, Mid(Replace(rst.Fields("fldProviderFirstName").Value, "'", ""), 1, 35))))   'first name
            strNM105 = IfNull(rst.Fields("fldSuperMI").Value, "")   'middle name
            strNM106 = ""          'name prefix (not used)
            strNM107 = ""          'name suffix (not used)
            strNM108 = "XX"        'NPI
            strNM109 = IfNull(rst.Fields("fldSuperNPI").Value, IfNull(rst.Fields("fldProviderNPI").Value, ""))
      Else
            strNM103 = Trim(StripDelimiters(Mid(Replace(IfNull(rst.Fields("fldProviderLastName").Value, ""), "'", ""), 1, 35)))          'Last name
            strNM104 = Trim(StripDelimiters(Mid(Replace(IfNull(rst.Fields("fldProviderFirstName").Value, ""), "'", ""), 1, 35)))   'first name
            strNM105 = IfNull(rst.Fields("fldProviderMI").Value, "")   'middle name
            strNM106 = ""          'name prefix (not used)
            strNM107 = ""          'name suffix (not used)
            strNM108 = "XX"        'NPI
            strNM109 = IfNull(rst.Fields("fldProviderNPI").Value, "")
      End If
   Else
      strNM102 = "2"         'entity type qualifier (1=person; 2=non-person)
      strNM103 = Trim(Mid(Replace(IfNull(rst.Fields("fldGroupName").Value, ""), "'", ""), 1, 35))          'Last name
      strNM104 = ""           'first name
      strNM105 = ""       'middle name
      strNM106 = ""          'name prefix (not used)
      strNM107 = ""          'name suffix (not used)
      strNM108 = "XX"        'NPI
      strNM109 = rst.Fields("fldGroupNPI").Value
   End If
                 
   strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 & "*" _
         & strNM104 & "*" & strNM105 & "*" & strNM106 & "*" _
         & strNM107 & "*" & strNM108 & "*" & strNM109 & "~"
    
   If g_blnLineFeed Then Print #1, UCase(strNM1) Else Print #1, UCase(strNM1);
   g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
        
    'Provider Billing Address
'    '5010 The pay-to-Address cannot be a PO Box.  -->> Changed 08/12/2014
   If InStr(1, StripDelimiters(IfNull(rst.Fields("fldProviderAddress").Value, rst.Fields("fldFacilityAddress").Value)), "PO Box", vbTextCompare) > 0 Then
        strN3 = "N3*" & StripDelimiters(IfNull(rst.Fields("fldFacilityAddress").Value, rst.Fields("fldProviderAddress").Value)) & "~"
   Else
        strN3 = "N3*" & StripDelimiters(IfNull(rst.Fields("fldProviderAddress").Value, rst.Fields("fldFacilityAddress").Value)) & "~"
   End If
        
 '   If g_blnLineFeed Then Print #1, UCase(strN3) Else Print #1, UCase(strN3);
 '   g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters

    If InStr(1, StripDelimiters(IfNull(rst.Fields("fldProviderAddress").Value, rst.Fields("fldFacilityAddress").Value)), "PO Box", vbTextCompare) > 0 Then
        strN401 = Replace(IfNull(rst.Fields("fldFacilityCity").Value, rst.Fields("fldProviderCity").Value), "'", "")      'city
        strN402 = Replace(IfNull(rst.Fields("fldFacilityState").Value, rst.Fields("fldProviderState").Value), "'", "")    'State code (2 digits)
        strN403 = Replace(IfNull(rst.Fields("fldFacilityZip").Value, rst.Fields("fldProviderZip").Value), "-", "")        'postal code
        If strN402 = "OT" Or strN402 = "ON" Then strN404 = "CAN"
    Else
        strN401 = Replace(IfNull(rst.Fields("fldProviderCity").Value, rst.Fields("fldFacilityCity").Value), "'", "")      'city
        strN402 = Replace(IfNull(rst.Fields("fldProviderState").Value, rst.Fields("fldFacilityState").Value), "'", "")    'State code (2 digits)
        strN403 = Replace(IfNull(rst.Fields("fldProviderZip").Value, Replace(IfNull(rst.Fields("fldFacilityZip").Value, rst.Fields("fldProviderZip").Value), "-", "")), "-", "")        'postal code
        If strN402 = "OT" Or strN402 = "ON" Then strN404 = "CAN"
    End If
        
    strN4 = "N4*" & strN401 & "*" & strN402 & "*" & strN403 & "~"
    If strN404 > "" Then strN4 = "N4*" & strN401 & "*" & strN402 & "*" & strN403 & "*" & strN404 & "~"
    
    If g_blnLineFeed Then Print #1, UCase(strN4) Else Print #1, UCase(strN4);
    g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
    
    
    strPRV01 = "PE"                 'Provider Code (PE-Performing, BI-Billing, PT-Payto, RF-Referring)
    strPRV02 = "PXC"    '5010 uses PXC for the Taxonomy Code
    
    'Specialty information
 '   If Not IsNull(rst.Fields("fldGroupNPI").Value) And Not IsNull(rst.Fields("fldSupervisorID").Value) Then
 '       strPRV03 = Trim(StripDelimiters(IfNull(rst.Fields("fldGroupTaxonomyCode").Value, "101Y00000X")))
 '   Else
        If Not IsNull(rst.Fields("fldSupervisorID").Value) And IfNull(rst.Fields("fldSuperNPI").Value, "") > "" And IfNull(rst.Fields("fldSuperTaxonomyCode").Value, "") > "" Then
            strPRV03 = Trim(StripDelimiters(IfNull(rst.Fields("fldSuperTaxonomyCode").Value, "101Y00000X")))         'ZZ relies on Provider Taxonomy Code published by BC/BS Association
        Else
            strPRV03 = Trim(StripDelimiters(IfNull(rst.Fields("fldTaxonomyCode").Value, "101Y00000X")))         'ZZ relies on Provider Taxonomy Code published by BC/BS Association
        End If
 '   End If
    
    If strPRV03 > "" Then
        strPRV = "PRV*" & strPRV01 & "*" & strPRV02 & "*" & strPRV03 & "~"
        If g_blnLineFeed Then Print #1, UCase(strPRV) Else Print #1, UCase(strPRV);
        g_lngNumSeg = g_lngNumSeg + 1
    End If
    
    Exit Function
    
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Private Function WriteLoop2120C(ByVal rst As ADODB.Recordset, ByVal strPayerCode As String)
'Subscriber Benefit-Related Entity Name
'This segment is used to provide eligibility information regarding primary care
'physicians, managed care entities and their related networks, third party liability (TPL)
'carriers, and members’ restricted provider(s).
'NM1*2B*2*Blue Cross Blue Shield*****PI*1435365~
    
    Dim strNM101, strNM102, strNM103, strNM104, strNM105 As String
    Dim strNM106, strNM107, strNM108, strNM109, strNM110, strNM111 As String
    Dim strNM1, strN2, strN3, strN4 As String
    Dim strN401, strN402, strN403, strN404 As String
    Dim strRef, strRef01, strRef02 As String
    Dim strPRV As String
    Dim strPRV01, strPRV02, strPRV03, strPRV04 As String
    
    On Error GoTo ErrTrap:

    strNM101 = "2B"        'entity code identifier (P3 – Primary Care Provider, P5 – Plan Sponsor, 1P – Provider, 2B – Third-Party Administrator)
    strNM102 = "1"         'entity type qualifier (1=person; 2=non-person)

    strNM102 = "2"         'entity type qualifier (1=person; 2=non-person)
    strNM103 = UCase(IfNull(rst.Fields("fldPlanName").Value, rst.Fields("fldCPCName").Value))    'Plan name
    strNM104 = ""           'first name
    strNM105 = ""       'middle name
    strNM106 = ""          'name prefix (not used)
    strNM107 = ""          'name suffix (not used)
            
    strNM108 = "PI"         'PI – Payer Identification
    strNM109 = strPayerCode
            
    strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 & "*" _
         & strNM104 & "*" & strNM105 & "*" & strNM106 & "*" _
         & strNM107 & "*" & strNM108 & "*" & strNM109 & "~"
    
    If g_blnLineFeed Then Print #1, UCase(strNM1) Else Print #1, UCase(strNM1);
    g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
    
'This segment provides the IHCP provider number of the provider to whom the member
'is restricted.
'PRV*PE*PXC*547689356~
     
    strPRV01 = "PE"                 'H – Hospital, HH – Home Health Care, PE – Performing, P2 – Pharmacy, SK – Skilled Nursing Facility
    strPRV02 = "PXC"                 'PXC – Provider Taxonomy Code
    
    'Specialty information
'    If Not IsNull(rst.Fields("fldGroupNPI").Value) And Not IsNull(rst.Fields("fldSupervisorID").Value) Then
'        strPRV03 = Trim(StripDelimiters(IfNull(rst.Fields("fldGroupTaxonomyCode").Value, "101Y00000X")))
'    Else
        If Not IsNull(rst.Fields("fldSupervisorID").Value) And IfNull(rst.Fields("fldSuperNPI").Value, "") > "" And IfNull(rst.Fields("fldSuperTaxonomyCode").Value, "") > "" Then
            strPRV03 = Trim(StripDelimiters(IfNull(rst.Fields("fldSuperTaxonomyCode").Value, "101Y00000X")))         'ZZ relies on Provider Taxonomy Code published by BC/BS Association
        Else
            strPRV03 = Trim(StripDelimiters(IfNull(rst.Fields("fldTaxonomyCode").Value, "101Y00000X")))         'ZZ relies on Provider Taxonomy Code published by BC/BS Association
        End If
'    End If
    
    If strPRV03 > "" Then
        strPRV = "PRV*" & strPRV01 & "*" & strPRV02 & "*" & strPRV03 & "~"
'        If g_blnLineFeed Then Print #1, UCase(strPRV) Else Print #1, UCase(strPRV);
'        g_lngNumSeg = g_lngNumSeg + 1
    End If
    
    Exit Function
    
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description

End Function
        
Private Sub WriteLoop2000C(ByVal rstClrHse As ADODB.Recordset, ByVal rst As ADODB.Recordset)
'Subscriber

   Dim strHL, strHL01, strHL02, strHL03, strHL04 As String
   Dim strPat As String
   Dim dtDOB As Date
    
   On Error GoTo ErrTrap:
    
   HL_01 = HL_01 + 1
   strHL01 = Format(HL_01, "##0")      'hierarchical ID number
   strHL02 = Format(HL_Parent, "##0")  'hierarchical parent ID
   strHL03 = "22"                      'hierarchical level code (19 = Provider Of Service)
    
   'Medicare and Medicaid do not allow any relationship but self
   dtDOB = IfNull(rst.Fields("fldInsdDOB").Value, 0)
   If rst.Fields("fldPatientID").Value = rst.Fields("fldRPID").Value Or _
      rst.Fields("fldInsType").Value = "MP" Or _
      rst.Fields("fldInsType").Value = "MB" Or _
      rst.Fields("fldInsType").Value = "MC" Or _
      (dtDOB = 0) Then
       strHL04 = "0"                       'hierarchical no child code
   Else
       strHL04 = "1"                       'hierarchical child code
   End If
   HL_Parent = HL_01
    
   strHL = "HL*" & strHL01 & "*" & strHL02 & "*" & strHL03 & "*" & strHL04 & "~"
   If g_blnLineFeed Then Print #1, strHL Else Print #1, strHL;
   g_lngNumSeg = g_lngNumSeg + 1
    
'Current Transaction Trace Number
    
   Dim strTrn, strTrn01, strTrn02, strTrn03, strTrn04 As String
        
   strTrn01 = "1"                  'Current Transaction Trace Number
    'Format(rst.Fields("fldPlanID").Value, "000000")
   strTrn02 = Format(rst.Fields("fldPatientID").Value, "00000000") & Format(rst.Fields("fldPlanID").Value, "000000")  'our account number
   If rst.Fields("fldVerPlanID").Value <= 0 Then
      If rst.Fields("fldVerifyElectronicYN").Value = "Y" Then
         If rst.Fields("fldReVerifyElectronicYN").Value = "Y" Then
            strTrn02 = strTrn02 & "R"
         Else
            strTrn02 = strTrn02 & "V"
         End If
      End If
   End If

   strTrn03 = IfNull(rstClrHse.Fields("fldReceiverID").Value, g_lngEndTxNum)      'Psyquel Submitter ID ' g_lngEndTxNum        'not used
   
   strTrn = "TRN*" & strTrn01 & "*" & strTrn02 & "~"
   If rst.Fields("fldClearingHouseID").Value = 23 Or _
      rstClrHse.Fields("fldISA08").Value = "1541414194" Then
      strTrn03 = "1352215297"
      strTrn = "TRN*" & strTrn01 & "*" & strTrn02 & "*" & strTrn03 & "~"
   ElseIf rst.Fields("fldClearingHouseID").Value = 22 Then
      strTrn03 = "1570287419"    'BCBS SC
      strTrn = "TRN*" & strTrn01 & "*" & strTrn02 & "*" & strTrn03 & "~"
   ElseIf rst.Fields("fldClearingHouseID").Value = 27 Or _
      rstClrHse.Fields("fldISA08").Value = "ANTHEM" Then
      strTrn03 = "1352215297"
      strTrn = "TRN*" & strTrn01 & "*" & strTrn02 & "*" & strTrn03 & "~"
   ElseIf rst.Fields("fldClearingHouseID").Value = 33 Then
      strTrn03 = "1580469845"    'BCBS GA
      strTrn = "TRN*" & strTrn01 & "*" & strTrn02 & "*" & strTrn03 & "~"
   ElseIf rst.Fields("fldClearingHouseID").Value = 59 Then
      strTrn03 = "1237391136"    'Empire NY
      strTrn = "TRN*" & strTrn01 & "*" & strTrn02 & "*" & strTrn03 & "~"
   ElseIf rst.Fields("fldClearingHouseID").Value = 66 Then
      strTrn03 = "1050158952"    'RI
      strTrn = "TRN*" & strTrn01 & "*" & strTrn02 & "*" & strTrn03 & "~"
   ElseIf rst.Fields("fldClearingHouseID").Value = 92 Then
      strTrn03 = "1990040115"    'Hawaii
      strTrn = "TRN*" & strTrn01 & "*" & strTrn02 & "*" & strTrn03 & "~"
   ElseIf rst.Fields("fldClearingHouseID").Value = 109 Then
      strTrn03 = "1420318333"    'Iowa
      strTrn = "TRN*" & strTrn01 & "*" & strTrn02 & "*" & strTrn03 & "~"
   ElseIf rst.Fields("fldClearingHouseID").Value = 159 Then
      strTrn03 = "1611241225"    'tricare east
      strTrn = "TRN*" & strTrn01 & "*" & strTrn02 & "*" & strTrn03 & "~"
   End If
   
   dtDOB = IfNull(rst.Fields("fldInsdDOB").Value, 0)
   If rst.Fields("fldPatientID").Value = rst.Fields("fldRPID").Value Or _
      rst.Fields("fldInsType").Value = "MP" Or _
      rst.Fields("fldInsType").Value = "MB" Or _
      rst.Fields("fldInsType").Value = "MC" Or _
      (dtDOB = 0) Then
      If g_blnLineFeed Then Print #1, strTrn Else Print #1, strTrn;
      g_lngNumSeg = g_lngNumSeg + 1
   End If
    
   Exit Sub
ErrTrap:
   Err.Raise Err.Number, Err.Source, Err.Description

End Sub

Private Function WriteLoop2100C(ByVal rstClrHse As ADODB.Recordset, ByVal rst As ADODB.Recordset, ByVal strPayerCode As String) As Boolean
'2100C-NM1 – Subscriber Name Segment
'Writes Subscriber information
    
   Dim strNM101, strNM102, strNM103, strNM104, strNM105 As String
   Dim strNM106, strNM107, strNM108, strNM109, strNM110, strNM111 As String
   Dim strNM1, strN2, strN3, strN4 As String
   Dim strN401, strN402, strN403, strN404, strN301 As String
   Dim strDTP, strDTP_01, strDTP_02, strDTP_03 As String
   Dim strINS, strINS01, strINS02, strINS03, strINS04, strINS05 As String
   Dim strEQ, strEQ_01 As String
   Dim strDMG, strRef As String
   Dim dtDOB As Date
    
   On Error GoTo ErrTrap:
    
   strNM101 = "IL"                    'entity identifier code (IL = insured)
   strNM102 = "1"                     'entity type qualifier (1=person)
   dtDOB = IfNull(rst.Fields("fldInsdDOB").Value, 0)
   If rst.Fields("fldPatientID").Value = rst.Fields("fldRPID").Value Or _
        rst.Fields("fldInsType").Value = "MP" Or _
        rst.Fields("fldInsType").Value = "MB" Or _
        rst.Fields("fldInsType").Value = "MC" Or _
        (Not IsDate(dtDOB) Or dtDOB = 0) Then
        strNM103 = Trim(StripDelimiters(Mid(Replace(IfNull(rst.Fields("fldPatientLastName").Value, ""), "'", ""), 1, 35)))    'Last name
        strNM104 = Trim(StripDelimiters(Mid(Replace(IfNull(rst.Fields("fldPatientFirstName").Value, ""), "'", ""), 1, 35)))   'first name
        strNM105 = Trim(IfNull(rst.Fields("fldPatientMI").Value, ""))   'middle name
        strN301 = Trim(StripDelimiters(IfNull(rst.Fields("fldPatientStreetNum").Value, "")))
        strN401 = Trim(StripDelimiters(IfNull(rst.Fields("fldPatientCity").Value, "")))   'city
        strN402 = Trim(Replace(IfNull(rst.Fields("fldPatientState").Value, ""), "'", ""))  'State code (2 digits)
        strN403 = Trim(NumbersOnly(Replace(IfNull(rst.Fields("fldPatientZip").Value, ""), "'", "")))    'postal code
        If strN402 = "OT" Or strN402 = "ON" Then strN404 = "CAN"
   Else
        strNM103 = Trim(StripDelimiters(IfNull(rst.Fields("fldInsuredLastName").Value, Mid(Replace(rst.Fields("fldPatientLastName").Value, "'", ""), 1, 35))))     'Last name
        strNM104 = Trim(StripDelimiters(IfNull(rst.Fields("fldInsuredFirstName").Value, Mid(Replace(rst.Fields("fldPatientFirstName").Value, "'", ""), 1, 35))))   'first name
        strNM105 = Trim(IfNull(rst.Fields("fldInsuredMI").Value, ""))  'middle name
        strN301 = Trim(StripDelimiters(IfNull(rst.Fields("fldInsuredStreetNum").Value, rst.Fields("fldPatientStreetNum").Value)))
        strN401 = Trim(StripDelimiters(IfNull(rst.Fields("fldInsuredCity").Value, rst.Fields("fldPatientCity").Value)))   'city
        strN402 = Trim(Replace(IfNull(rst.Fields("fldInsuredState").Value, rst.Fields("fldPatientState").Value), "'", ""))  'State code (2 digits)
        strN403 = Trim(NumbersOnly(Replace(IfNull(rst.Fields("fldInsuredZip").Value, rst.Fields("fldPatientZip").Value), "'", "")))
        If strN402 = "OT" Or strN402 = "ON" Then strN404 = "CAN"
   End If
   strNM106 = ""                      'name prefix (not used)
   strNM107 = ""                      'name suffix
   strNM108 = "MI"                    'Identification code qualifier (MI=Member Identification Number)
   strNM109 = Trim(StripDelimiters(IfNull(rst.Fields("fldCardNum").Value, "00"))) 'identification code
    
   strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
         & "*" & strNM104 & "*" & strNM105 & "*" & strNM106 _
         & "*" & strNM107 & "*" & strNM108 & "*" & strNM109 _
         & "~"
    
   If g_blnLineFeed Then Print #1, UCase(strNM1) Else Print #1, UCase(strNM1);
   g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
        
        
    If rst.Fields("fldInsType").Value <> "MP" And _
        rst.Fields("fldInsType").Value <> "MB" And _
        rst.Fields("fldInsType").Value <> "MC" Then
        If StripDelimiters(IfNull(Trim(rst.Fields("fldGroupNum").Value), "")) > "" And _
        StripDelimiters(IfNull(Trim(rst.Fields("fldGroupNum").Value), "")) <> "None" Then
            strRef = "REF*6P*" & StripDelimiters(IfNull(Trim(rst.Fields("fldGroupNum").Value), "")) & "~"  'Subscriber group or policy number
            If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
            g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
        End If
    End If
        
   'Subscriber address
    If Not IsNull(strN301) And rst.Fields("fldInsuranceID").Value <> 1278 Then
        'Patient address
        strN3 = "N3*" & strN301 & "~"
  '      If g_blnLineFeed Then Print #1, strN3 Else Print #1, strN3;
  '      g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters

        strN4 = "N4*" & strN401 & "*" & strN402 & "*" & strN403 & "~"
        If strN404 > "" Then strN4 = "N4*" & strN401 & "*" & strN402 & "*" & strN403 & "*" & strN404 & "~"
        If g_blnLineFeed Then Print #1, UCase(strN4) Else Print #1, UCase(strN4);
        g_lngNumSeg = g_lngNumSeg + 1
    End If
        
    'SSN
   dtDOB = IfNull(rst.Fields("fldInsdDOB").Value, 0)
   If rst.Fields("fldPatientID").Value = rst.Fields("fldRPID").Value Or _
        rst.Fields("fldInsType").Value = "MP" Or _
        rst.Fields("fldInsType").Value = "MB" Or _
        rst.Fields("fldInsType").Value = "MC" Or _
        (Not IsDate(dtDOB) Or dtDOB = 0) Then
      dtDOB = IfNull(rst.Fields("fldPatientDOB").Value, 0)
      If IfNull(rst.Fields("fldPatientSSN").Value, "") > "" And _
            (Not IsDate(dtDOB) Or dtDOB = 0) And _
            StripDelimiters(IfNull(rst.Fields("fldPatientSSN").Value, "")) <> strNM109 And _
            StripDelimiters(IfNull(rst.Fields("fldPatientSSN").Value, "")) <> "000000000" And _
            StripDelimiters(IfNull(rst.Fields("fldPatientSSN").Value, "")) <> "111111111" And _
            StripDelimiters(IfNull(rst.Fields("fldPatientSSN").Value, "")) <> "222222222" And _
            StripDelimiters(IfNull(rst.Fields("fldPatientSSN").Value, "")) <> "999999999" Then
            strRef = "REF*SY*" & StripDelimiters(rst.Fields("fldPatientSSN").Value) & "~"
            If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
            g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
      End If
   Else
      dtDOB = IfNull(rst.Fields("fldInsdDOB").Value, 0)
      If IfNull(rst.Fields("fldInsuredSSN").Value, "") > "" And _
            (Not IsDate(dtDOB) Or dtDOB = 0) And _
            StripDelimiters(IfNull(rst.Fields("fldInsuredSSN").Value, "")) <> strNM109 And _
            StripDelimiters(IfNull(rst.Fields("fldInsuredSSN").Value, "")) <> "000000000" And _
            StripDelimiters(IfNull(rst.Fields("fldInsuredSSN").Value, "")) <> "111111111" And _
            StripDelimiters(IfNull(rst.Fields("fldInsuredSSN").Value, "")) <> "222222222" And _
            StripDelimiters(IfNull(rst.Fields("fldInsuredSSN").Value, "")) <> "999999999" Then
            strRef = "REF*SY*" & StripDelimiters(rst.Fields("fldInsuredSSN").Value) & "~"
            If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
            g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
      End If
   End If
    
    'Subscriber DOB
   dtDOB = IfNull(rst.Fields("fldInsdDOB").Value, 0)
   If rst.Fields("fldPatientID").Value = rst.Fields("fldRPID").Value Or _
        rst.Fields("fldInsType").Value = "MP" Or _
        rst.Fields("fldInsType").Value = "MB" Or _
        rst.Fields("fldInsType").Value = "MC" Or _
        (Not IsDate(dtDOB) Or dtDOB = 0) Then
        dtDOB = IfNull(rst.Fields("fldPatientDOB").Value, 0)
        If IsDate(dtDOB) And dtDOB > 0 And rst.Fields("fldInsuranceID").Value <> 1278 Then
                strDMG = "DMG*D8*" & Year(dtDOB) & Format(Month(dtDOB), "00") & Format(Day(dtDOB), "00") & "*"
                strDMG = strDMG & IfNull(rst.Fields("fldPatientSex").Value, IfNull(rst.Fields("fldInsdSex").Value, "U")) & "~"
                If g_blnLineFeed Then Print #1, strDMG Else Print #1, strDMG;
                g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
        End If
   Else
      dtDOB = IfNull(rst.Fields("fldInsdDOB").Value, 0)
      'Subscriber DOB - If the DOB is not known then this line must be ignored otherwise validation errors occur
      If IsDate(dtDOB) And dtDOB > 0 And rst.Fields("fldInsuranceID").Value <> 1278 Then
         strDMG = "DMG*D8*" & Year(dtDOB) & Format(Month(dtDOB), "00") & Format(Day(dtDOB), "00") & "*"
         strDMG = strDMG & IfNull(rst.Fields("fldInsdSex").Value, "U") & "~"
         If g_blnLineFeed Then Print #1, strDMG Else Print #1, strDMG;
         g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
      End If
   End If
    
   strINS01 = "Y"
   If rst.Fields("fldPatientID").Value = rst.Fields("fldRPID").Value Or _
      rst.Fields("fldInsType").Value = "MP" Or _
      rst.Fields("fldInsType").Value = "MB" Or _
      rst.Fields("fldInsType").Value = "MC" Then
        
      If rst.Fields("fldClearingHouseID").Value <> 22 And _
         rst.Fields("fldClearingHouseID").Value <> 23 And _
         rst.Fields("fldClearingHouseID").Value <> 27 And _
         rst.Fields("fldClearingHouseID").Value <> 33 And _
         rst.Fields("fldClearingHouseID").Value <> 59 And _
         rst.Fields("fldClearingHouseID").Value <> 66 And _
         rst.Fields("fldClearingHouseID").Value <> 92 And _
         rst.Fields("fldClearingHouseID").Value <> 109 And _
         rst.Fields("fldInsType").Value <> "CH" And _
         rstClrHse.Fields("fldISA08").Value <> "ANTHEM" Then
        'individual relationship code (18 = self)
         strINS02 = "18"
         strINS = "INS*" & strINS01 & "*" & strINS02 & "~"
         If g_blnLineFeed Then Print #1, strINS Else Print #1, strINS;
         g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
      End If
      
      'Eligibility Date
      strDTP_01 = "291"                'date/time qualifier
      strDTP_02 = "D8"                 'date time period format qualifier
      strDTP_03 = Format(Now() - 10, "yyyymmdd")              'date/time period in CCYYMMDD
      strDTP = "DTP*" & strDTP_01 & "*" & strDTP_02 & "*" & strDTP_03 & "~"
      If g_blnLineFeed Then Print #1, strDTP Else Print #1, strDTP;
      g_lngNumSeg = g_lngNumSeg + 1
        
   End If
    
   Exit Function

ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description
    
End Function

Private Function WriteLoop2110C(ByVal rst As ADODB.Recordset) As Boolean
'Service Type Code
    '30 Health Benefit Coverage
    '98 Professional (Physician) Visit - Office
    'A0 Professional (Physician) Visit - Outpatient
    'A3 Professional (Physician) Visit - Home
    'A6 Psychotherapy
    'A7 Psychiatric - Inpatient
    'A8 Psychiatric - Outpatient
    'AI Substance Abuse
    'MH Mental Health
    
    Dim strEQ, strEQ_01, strEQ_02 As String
    Dim strIII, strIII_01, strIII_02 As String
    
    strEQ_01 = "MH"                'Health Benefit Coverage
    strEQ = "EQ*" & strEQ_01 & "~"
    If g_blnLineFeed Then Print #1, strEQ Else Print #1, strEQ;
    g_lngNumSeg = g_lngNumSeg + 1
    
    
'    strEQ_01 = ""                 'Service Type Code
'    strEQ_02 = "HC:90834"              'Procedure codes
'    strEQ = "EQ*" & strEQ_01 & "*" & strEQ_02 & "~"
'    If g_blnLineFeed Then Print #1, strEQ Else Print #1, strEQ;
'    g_lngNumSeg = g_lngNumSeg + 1
    
'    strEQ_01 = ""                 'Service Type Code
'    strEQ_02 = "HC:90837"              'Procedure codes
'    strEQ = "EQ*" & strEQ_01 & "*" & strEQ_02 & "~"
'    If g_blnLineFeed Then Print #1, strEQ Else Print #1, strEQ;
'    g_lngNumSeg = g_lngNumSeg + 1
    
    strIII_01 = "ZZ"                              'Place of Service
    strIII_02 = rst.Fields("fldPOSCode").Value
    strIII = "III*" & strIII_01 & "*" & strIII_02 & "~"
    If g_blnLineFeed Then Print #1, strIII Else Print #1, strIII;
    g_lngNumSeg = g_lngNumSeg + 1
    
    Exit Function

ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description
    
End Function

Private Sub WriteLoop2000D(ByVal rstClrHse As ADODB.Recordset, ByVal rst As ADODB.Recordset)
'Writes heirarchy level (Dependent Level).
    
   Dim strHL, strHL01, strHL02, strHL03, strHL04 As String
   Dim strDMG, strDMG01, strDMG02, strDMG03, strDMG04 As String
   Dim dtDOB As Date
    
   HL_01 = HL_01 + 1
   strHL01 = Format(HL_01, "##0")      'hierarchical ID number - always 2 (see p. 108 of X098.pdf)
   strHL02 = Format(HL_Parent, "##0")  'hierarchical parent ID
   strHL03 = "23"                      'hierarchical level code (23 = Dependent)
    
'   If rst.Fields("fldPatientID").Value <> rst.Fields("fldRPID").Value And _
'       rst.Fields("fldInsType").Value <> "MP" And _
'       rst.Fields("fldInsType").Value <> "MB" And _
'       rst.Fields("fldInsType").Value <> "MC" Then
'       HL_Parent = HL_01               'Keep track of Parent Segment
'       strHL04 = "1"                   'we have subordinate in hierarchical child code
'   Else
       strHL04 = "0"                   'hierarchical child code Patient is the Insured
'   End If
    
   strHL = "HL*" & strHL01 & "*" & strHL02 & "*" & strHL03 & "*" & strHL04 & "~"
   If g_blnLineFeed Then Print #1, strHL Else Print #1, strHL;
   g_lngNumSeg = g_lngNumSeg + 1
       
'Current Transaction Trace Number
    
   Dim strTrn, strTrn01, strTrn02, strTrn03, strTrn04 As String
        
   strTrn01 = "1"                  'Current Transaction Trace Number
    'Format(rst.Fields("fldPlanID").Value, "000000")
   strTrn02 = Format(rst.Fields("fldPatientID").Value, "00000000") & Format(rst.Fields("fldPlanID").Value, "000000")  'our account number
   If rst.Fields("fldVerPlanID").Value <= 0 Then
      If rst.Fields("fldVerifyElectronicYN").Value = "Y" Then
         If rst.Fields("fldReVerifyElectronicYN").Value = "Y" Then
            strTrn02 = strTrn02 & "R"
         Else
            strTrn02 = strTrn02 & "V"
         End If
      End If
   End If

'   strTrn03 = IfNull(rstClrHse.Fields("fldReceiverID").Value, g_lngEndTxNum)      'Psyquel Submitter ID ' g_lngEndTxNum        'not used
   
   If rst.Fields("fldClearingHouseID").Value = 23 Then
      strTrn03 = "1541414194"
      strTrn = "TRN*" & strTrn01 & "*" & strTrn02 & "*" & strTrn03 & "~"
   ElseIf rst.Fields("fldClearingHouseID").Value = 22 Then
      strTrn03 = "1570287419"    'BCBS SC
      strTrn = "TRN*" & strTrn01 & "*" & strTrn02 & "*" & strTrn03 & "~"
   ElseIf rst.Fields("fldClearingHouseID").Value = 27 Or _
      rstClrHse.Fields("fldISA08").Value = "ANTHEM" Then
      strTrn03 = "1352215297"
      strTrn = "TRN*" & strTrn01 & "*" & strTrn02 & "*" & strTrn03 & "~"
   ElseIf rst.Fields("fldClearingHouseID").Value = 33 Then
      strTrn03 = "1580469845"    'BCBS GA
      strTrn = "TRN*" & strTrn01 & "*" & strTrn02 & "*" & strTrn03 & "~"
   ElseIf rst.Fields("fldClearingHouseID").Value = 59 Then
      strTrn03 = "1237391136"    'Empire NY
      strTrn = "TRN*" & strTrn01 & "*" & strTrn02 & "*" & strTrn03 & "~"
   ElseIf rst.Fields("fldClearingHouseID").Value = 66 Then
      strTrn03 = "1050158952"    'RI
      strTrn = "TRN*" & strTrn01 & "*" & strTrn02 & "*" & strTrn03 & "~"
   ElseIf rst.Fields("fldClearingHouseID").Value = 92 Then
      strTrn03 = "1990040115"    'Hawaii
      strTrn = "TRN*" & strTrn01 & "*" & strTrn02 & "*" & strTrn03 & "~"
   ElseIf rst.Fields("fldClearingHouseID").Value = 109 Then
      strTrn03 = "1420318333"    'Iowa
      strTrn = "TRN*" & strTrn01 & "*" & strTrn02 & "*" & strTrn03 & "~"
   ElseIf rst.Fields("fldClearingHouseID").Value = 159 Then
      strTrn03 = "1611241225"    'tricare east
      strTrn = "TRN*" & strTrn01 & "*" & strTrn02 & "*" & strTrn03 & "~"
   Else
      strTrn = "TRN*" & strTrn01 & "*" & strTrn02 & "~"
   End If
   
   If g_blnLineFeed Then Print #1, strTrn Else Print #1, strTrn;
   g_lngNumSeg = g_lngNumSeg + 1
   
    Exit Sub
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub

Private Function WriteLoop2100D(ByVal rstClrHse As ADODB.Recordset, ByVal rst As ADODB.Recordset)
'Patient name, demographics

    Dim strNM101, strNM102, strNM103, strNM104, strNM105 As String
    Dim strNM106, strNM107, strNM108, strNM109, strNM110, strNM111 As String
    Dim strRef, strRef01, strRef02 As String
    Dim strPRV As String
    Dim strPRV01, strPRV02, strPRV03, strPRV04 As String
    Dim strNM1, strN2, strN3, strN4 As String
    Dim strN401, strN402, strN403, strN404, strN301 As String
    Dim strDMG, strDMG01, strDMG02, strDMG03, strDMG04 As String
    Dim strDTP, strDTP_01, strDTP_02, strDTP_03 As String
    Dim strINS, strINS01, strINS02, strINS03, strINS04, strINS05 As String
    Dim dtDOB As Date
    
    On Error GoTo ErrTrap:
    
    strNM101 = "03"         'entity identifier code (03 = Dependent)
    strNM102 = "1"          'Qualifier - 2:Non-person entity
    strNM103 = Trim(Mid(Replace(IfNull(rst.Fields("fldPatientLastName").Value, ""), "'", ""), 1, 35))    'Last name
    strNM104 = Trim(Mid(Replace(IfNull(rst.Fields("fldPatientFirstName").Value, ""), "'", ""), 1, 35))   'first name
    strNM105 = Trim(IfNull(rst.Fields("fldPatientMI").Value, ""))   'middle name
    strNM106 = ""        'not used (name prefix)
    strNM107 = ""        'not used (name suffix)
    strNM108 = ""        'not used (identification code qualifier)
    'Subscribers Identification number as assigned by the payer.
    strNM109 = ""       'not used Trim(Replace(StripDelimiters(IfNull(rst.Fields("fldCardNum").Value, "00")), " ", "", 1))
    
    If strNM105 > "" Then
        strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
         & "*" & strNM104 & "*" & strNM105 & "~"
    Else
        strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
         & "*" & strNM104 & "~"
    End If
    
    If g_blnLineFeed Then Print #1, UCase(strNM1) Else Print #1, UCase(strNM1);
    g_lngNumSeg = g_lngNumSeg + 1

    'Patient address
    strN3 = "N3*" & Trim(StripDelimiters(IfNull(rst.Fields("fldPatientStreetNum").Value, ""))) & "~"
'    If g_blnLineFeed Then Print #1, strN3 Else Print #1, strN3;
'    g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters

    strN401 = Trim(Replace(IfNull(StripDelimiters(rst.Fields("fldPatientCity").Value), ""), "'", ""))   'city
    strN402 = Trim(Replace(IfNull(rst.Fields("fldPatientState").Value, ""), "'", ""))  'State code (2 digits)
    strN403 = Trim(NumbersOnly(Replace(IfNull(rst.Fields("fldPatientZip").Value, ""), "'", "")))    'postal code
    If strN402 = "OT" Or strN402 = "ON" Then strN404 = "CAN"
    
    strN4 = "N4*" & strN401 & "*" & strN402 & "*" & strN403 & "~"
    If strN404 > "" Then strN4 = "N4*" & strN401 & "*" & strN402 & "*" & strN403 & "*" & strN404 & "~"
    If g_blnLineFeed Then Print #1, UCase(strN4) Else Print #1, UCase(strN4);
    g_lngNumSeg = g_lngNumSeg + 1
    
        
    strPRV01 = "PE"                 'Provider Code (PE-Performing, BI-Billing, PT-Payto, RF-Referring)
    strPRV02 = "PXC"    '5010 uses PXC for the Taxonomy Code
    
    'Provider information
 '   If Not IsNull(rst.Fields("fldGroupNPI").Value) And Not IsNull(rst.Fields("fldSupervisorID").Value) Then
 '       strPRV03 = Trim(StripDelimiters(IfNull(rst.Fields("fldGroupTaxonomyCode").Value, "101Y00000X")))
 '   Else
        If Not IsNull(rst.Fields("fldSupervisorID").Value) And IfNull(rst.Fields("fldSuperNPI").Value, "") > "" And IfNull(rst.Fields("fldSuperTaxonomyCode").Value, "") > "" Then
            strPRV03 = Trim(StripDelimiters(IfNull(rst.Fields("fldSuperTaxonomyCode").Value, "101Y00000X")))         'ZZ relies on Provider Taxonomy Code published by BC/BS Association
        Else
            strPRV03 = Trim(StripDelimiters(IfNull(rst.Fields("fldTaxonomyCode").Value, "101Y00000X")))         'ZZ relies on Provider Taxonomy Code published by BC/BS Association
        End If
 '   End If
 '   If Not IsNull(rst.Fields("fldSupervisorID").Value) And IfNull(rst.Fields("fldSuperNPI").Value, "") > "" Then
 '       strPRV03 = Trim(StripDelimiters(IfNull(rst.Fields("fldSuperNPI").Value, rst.Fields("fldProviderNPI").Value)))
 '   Else
 '       strPRV03 = Trim(StripDelimiters(IfNull(rst.Fields("fldProviderNPI").Value, "101Y00000X")))
 '   End If
    
    If strPRV03 > "" Then
        strPRV = "PRV*" & strPRV01 & "*" & strPRV02 & "*" & strPRV03 & "~"
'        If g_blnLineFeed Then Print #1, strPRV Else Print #1, strPRV;
'        g_lngNumSeg = g_lngNumSeg + 1
    End If
        
    'SSN
   dtDOB = IfNull(rst.Fields("fldPatientDOB").Value, 0)
   If IfNull(rst.Fields("fldPatientSSN").Value, "") > "" And _
     (Not IsDate(dtDOB) Or dtDOB = 0) And _
     StripDelimiters(IfNull(rst.Fields("fldPatientSSN").Value, "")) <> strNM109 And _
     StripDelimiters(IfNull(rst.Fields("fldPatientSSN").Value, "")) <> "000000000" And _
     StripDelimiters(IfNull(rst.Fields("fldPatientSSN").Value, "")) <> "111111111" And _
     StripDelimiters(IfNull(rst.Fields("fldPatientSSN").Value, "")) <> "222222222" And _
     StripDelimiters(IfNull(rst.Fields("fldPatientSSN").Value, "")) <> "999999999" Then
      strRef = "REF*SY*" & StripDelimiters(rst.Fields("fldPatientSSN").Value) & "~"
      If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
      g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
   End If
    
    'When the subscriber is the patient use DMG Segment
'    If rst.Fields("fldPatientID").Value = rst.Fields("fldRPID").Value Then
        dtDOB = IfNull(rst.Fields("fldPatientDOB").Value, 0)
        If IsDate(dtDOB) And dtDOB > 0 Then
            strDMG01 = "D8"
            strDMG02 = Year(dtDOB) & Format(Month(dtDOB), "00") & Format(Day(dtDOB), "00")
            strDMG03 = IfNull(rst.Fields("fldPatientSex").Value, IfNull(rst.Fields("fldInsdSex").Value, "U"))
            strDMG = "DMG*" & strDMG01 & "*" & strDMG02 & "*" & strDMG03 & "~"
            If g_blnLineFeed Then Print #1, strDMG Else Print #1, strDMG;
            g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
        End If
'    End If
    
   'If rst.Fields("fldClearingHouseID").Value <> 23 And
   If rst.Fields("fldClearingHouseID").Value <> 22 And _
      rst.Fields("fldClearingHouseID").Value <> 27 And _
      rst.Fields("fldClearingHouseID").Value <> 33 And _
      rst.Fields("fldClearingHouseID").Value <> 59 And _
      rst.Fields("fldClearingHouseID").Value <> 66 And _
      rst.Fields("fldClearingHouseID").Value <> 92 And _
      rst.Fields("fldClearingHouseID").Value <> 109 And _
      rst.Fields("fldInsType").Value <> "CH" And _
      rstClrHse.Fields("fldISA08").Value <> "ANTHEM" Then
      strINS01 = "N"
      strINS02 = "19"              'individual relationship code (01, 19, 34)
      strINS = "INS*" & strINS01 & "*" & strINS02 & "~"
      If g_blnLineFeed Then Print #1, strINS Else Print #1, strINS;
      g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
   End If
        
   'Eligibility Date
   strDTP_01 = "291"                'date/time qualifier
   strDTP_02 = "D8"                 'date time period format qualifier
   strDTP_03 = Format(Now() - 10, "yyyymmdd")              'date/time period in CCYYMMDD
   strDTP = "DTP*" & strDTP_01 & "*" & strDTP_02 & "*" & strDTP_03 & "~"
   If g_blnLineFeed Then Print #1, strDTP Else Print #1, strDTP;
   g_lngNumSeg = g_lngNumSeg + 1
    
   Exit Function
    
ErrTrap:
   Err.Raise Err.Number, Err.Source, Err.Description
End Function

Private Sub WriteLoop2110D(ByVal rst As ADODB.Recordset)
'Service Type Code
    '30 Health Benefit Coverage
    '98 Professional (Physician) Visit - Office
    'A0 Professional (Physician) Visit - Outpatient
    'A3 Professional (Physician) Visit - Home
    'A6 Psychotherapy
    'A7 Psychiatric - Inpatient
    'A8 Psychiatric - Outpatient
    'AI Substance Abuse
    'MH Mental Health
    
    Dim strEQ, strEQ_01, strEQ_02 As String
    Dim strIII, strIII_01, strIII_02 As String
    
    strEQ_01 = "MH"                'Health Benefit Coverage
    strEQ = "EQ*" & strEQ_01 & "~"
    If g_blnLineFeed Then Print #1, strEQ Else Print #1, strEQ;
    g_lngNumSeg = g_lngNumSeg + 1
    
    
 '   strEQ_01 = ""                       'Service Type Code
 '   strEQ_02 = "HC:90834"              'Procedure codes
 '   strEQ = "EQ*" & strEQ_01 & "*" & strEQ_02 & "~"
 '   If g_blnLineFeed Then Print #1, strEQ Else Print #1, strEQ;
 '   g_lngNumSeg = g_lngNumSeg + 1
    
 '   strEQ_01 = ""                 'Service Type Code
 '   strEQ_02 = "HC:90837"              'Procedure codes
 '   strEQ = "EQ*" & strEQ_01 & "*" & strEQ_02 & "~"
 '   If g_blnLineFeed Then Print #1, strEQ Else Print #1, strEQ;
 '   g_lngNumSeg = g_lngNumSeg + 1
    
    strIII_01 = "ZZ"                              'Place of Service
    strIII_02 = rst.Fields("fldPOSCode").Value
    strIII = "III*" & strIII_01 & "*" & strIII_02 & "~"
    If g_blnLineFeed Then Print #1, strIII Else Print #1, strIII;
    g_lngNumSeg = g_lngNumSeg + 1
    
    
    
    Exit Sub
    
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub

Private Sub WriteLoop2200D(ByVal rstClrHse As ADODB.Recordset, ByVal rst As ADODB.Recordset)
'Current Transaction Trace Number
    
   Dim strTrn, strTrn01, strTrn02, strTrn03, strTrn04 As String
   Dim strRef, strRef01, strRef02 As String
   Dim strAmt, strAmt01, strAmt02 As String
   Dim strDTP, strDTP_01, strDTP_02, strDTP_03 As String
    
   On Error GoTo ErrTrap:
        
   strTrn01 = "1"              'Current Transaction Trace NumberstrTrn01 = "1"                  'Current Transaction Trace Number
    'Format(rst.Fields("fldPlanID").Value, "000000")
   strTrn02 = Format(rst.Fields("fldPatientID").Value, "00000000") & Format(rst.Fields("fldPlanID").Value, "000000")  'our account number
   If rst.Fields("fldVerPlanID").Value <= 0 Then
      If rst.Fields("fldVerifyElectronicYN").Value = "Y" Then
         If rst.Fields("fldReVerifyElectronicYN").Value = "Y" Then
            strTrn02 = strTrn02 & "R"
         Else
            strTrn02 = strTrn02 & "V"
         End If
      End If
   End If
   strTrn03 = IfNull(rstClrHse.Fields("fldSubmitterID").Value, g_lngEndTxNum)      'Psyquel Submitter ID  'g_lngEndTxNum    'not used
   strTrn04 = ""               'not used
   
   strTrn = "TRN*" & strTrn01 & "*" & strTrn02 & "~"
   If rst.Fields("fldClearingHouseID").Value = 23 Or _
      rstClrHse.Fields("fldISA08").Value = "1541414194" Then
      strTrn03 = "1352215297"
      strTrn = "TRN*" & strTrn01 & "*" & strTrn02 & "*" & strTrn03 & "~"
   ElseIf rst.Fields("fldClearingHouseID").Value = 22 Then
      strTrn03 = "1570287419"    'BCBS SC
      strTrn = "TRN*" & strTrn01 & "*" & strTrn02 & "*" & strTrn03 & "~"
   ElseIf rst.Fields("fldClearingHouseID").Value = 27 Or _
      rstClrHse.Fields("fldISA08").Value = "ANTHEM" Then
      strTrn03 = "1352215297"
      strTrn = "TRN*" & strTrn01 & "*" & strTrn02 & "*" & strTrn03 & "~"
   ElseIf rst.Fields("fldClearingHouseID").Value = 33 Then
      strTrn03 = "1580469845"    'BCBS GA
      strTrn = "TRN*" & strTrn01 & "*" & strTrn02 & "*" & strTrn03 & "~"
   ElseIf rst.Fields("fldClearingHouseID").Value = 59 Then
      strTrn03 = "1237391136"    'Empire NY
      strTrn = "TRN*" & strTrn01 & "*" & strTrn02 & "*" & strTrn03 & "~"
   ElseIf rst.Fields("fldClearingHouseID").Value = 66 Then
      strTrn03 = "1050158952"    'RI
      strTrn = "TRN*" & strTrn01 & "*" & strTrn02 & "*" & strTrn03 & "~"
   ElseIf rst.Fields("fldClearingHouseID").Value = 92 Then
      strTrn03 = "1990040115"    'Hawaii
      strTrn = "TRN*" & strTrn01 & "*" & strTrn02 & "*" & strTrn03 & "~"
   ElseIf rst.Fields("fldClearingHouseID").Value = 109 Then
      strTrn03 = "1420318333"    'Iowa
      strTrn = "TRN*" & strTrn01 & "*" & strTrn02 & "*" & strTrn03 & "~"
   ElseIf rst.Fields("fldClearingHouseID").Value = 159 Then
      strTrn03 = "1611241225"    'tricare east
      strTrn = "TRN*" & strTrn01 & "*" & strTrn02 & "*" & strTrn03 & "~"
   End If
   If g_blnLineFeed Then Print #1, strTrn Else Print #1, strTrn;
   g_lngNumSeg = g_lngNumSeg + 1
    
    'When the subscriber is the patient use REF Segment - Payers assign control number
    If rst.Fields("fldPatientID").Value = rst.Fields("fldRPID").Value Then
      '  If Not IsNull(rst.Fields("fldICN").Value) Then
      '      strRef01 = "1K"       'Payers claim number
      '      strRef02 = Trim(StripDelimiters(rst.Fields("fldICN").Value))
      '      strRef = "REF*" & strRef01 & "*" & strRef02 & "~"
      '      If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
      '      g_lngNumSeg = g_lngNumSeg + 1
      '  End If
    End If

    If rst.Fields("fldPatientID").Value = rst.Fields("fldRPID").Value Then
        strRef01 = "EA"              'Medical Record Number
        strRef02 = rst.Fields("fldEncounterLogID").Value
        strRef = "REF*" & strRef01 & "*" & strRef02 & "~"
        If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
        g_lngNumSeg = g_lngNumSeg + 1
    End If
    
    If rst.Fields("fldPatientID").Value = rst.Fields("fldRPID").Value Then
        strAmt01 = "T3"              'Medical Record Number
        strAmt02 = rst.Fields("fldFee").Value * rst.Fields("fldUnits").Value  'total claim charge amount
        strAmt = "AMT*" & strAmt01 & "*" & strAmt02 & "~"
        If g_blnLineFeed Then Print #1, strAmt Else Print #1, strAmt;
        g_lngNumSeg = g_lngNumSeg + 1
    End If
        
    'Date of Service
    strDTP_01 = "232"                'date/time qualifier
    strDTP_02 = "RD8"                'date time period format qualifier
    strDTP_03 = Format(rst.Fields("fldDOS").Value, "yyyymmdd") & "-" & Format(rst.Fields("fldDOS").Value, "yyyymmdd")
    'date/time period in CCYYMMDD
    strDTP = "DTP*" & strDTP_01 & "*" & strDTP_02 & "*" & strDTP_03 & "~"
    If g_blnLineFeed Then Print #1, strDTP Else Print #1, strDTP;
    g_lngNumSeg = g_lngNumSeg + 1
    
    
    Exit Sub
        
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub


Private Function GetRelationCode(ByVal strPatRelat As String)
    
    On Error GoTo ErrTrap:
    
    Select Case strPatRelat
        Case "Self"
            GetRelationCode = "18"  '(18 = self)
        Case "Spouse"
            GetRelationCode = "01"  '(01 = spouse)
        Case "Child"
            GetRelationCode = "19"  '(19 = child)
        Case Else
            GetRelationCode = "G8"  '(G8 = other)
    End Select
    
    Exit Function

ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description

End Function


Private Function StripDelimiters(ByVal strData As String) As String

    Dim strTemp As String
    
    strTemp = strData 'Make a copy of the data
    strTemp = Replace(strTemp, ":", "", 1)
    strTemp = Replace(strTemp, "*", "", 1)
    strTemp = Replace(strTemp, "-", "", 1)
    strTemp = Replace(strTemp, "~", "", 1)
    strTemp = Replace(strTemp, ".", "", 1)
    strTemp = Replace(strTemp, " ", "", 1)
    strTemp = Replace(strTemp, "AMYSIS#", "", 1)
    
    StripDelimiters = strTemp

End Function
Private Function StripDecimals(ByVal strData As String) As String

    Dim strTemp As String
    Dim intPos As Integer
    
    strTemp = strData 'Make a copy of the data

    strTemp = Replace(strTemp, ".", "", 1)
    
    StripDecimals = strTemp

End Function

Public Sub UpdateFileID(ByVal lngPatRPPlanID As Long, ByVal lngPlanID As Long)
    
    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command
    Dim strSQL As String

    On Error GoTo ErrTrap:

    'Prepare the SQL statement
    strSQL = "UPDATE tblPatRPPlanRule "
    strSQL = strSQL & " SET "
    strSQL = strSQL & " fldVerifyElectronicYN = 'Y', fldVerifyRejectYN = 'N', fldReVerifyElectronicYN = 'N', "
    strSQL = strSQL & " fldVerifyFileID = '" & g_lngEndTxNum & "' "
    strSQL = strSQL & " WHERE "
    strSQL = strSQL & " fldPatRPPlanID = " & lngPatRPPlanID & " AND "
    strSQL = strSQL & " fldVerPlanID = " & lngPlanID

    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    cmdSQL.CommandText = strSQL
    cmdSQL.CommandType = adCmdText
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords

    'Close the connection and free all resources
    Set cmdSQL.ActiveConnection = Nothing
    Set cmdSQL = Nothing
    cnnSQL.Close
    Set cnnSQL = Nothing

    'Signal successful completion
    'GetObjectContext.SetComplete

    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    'GetObjectContext.SetAbort
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub


