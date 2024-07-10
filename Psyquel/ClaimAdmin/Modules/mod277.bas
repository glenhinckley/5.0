Attribute VB_Name = "mod276"
Option Explicit

Private g_lngNumSeg As Long 'Data segment counter
Private g_lngEndTxNum As Long
Private g_FileNumber As Long
Private g_FileName As String
Private g_blnLineFeed As Boolean
Private HL_01, HL_Parent As Long

Public Function GenerateClaimStatusRequest(ByVal rstClrHse As ADODB.Recordset, ByVal rst As ADODB.Recordset, ByVal lngEndTxNum As Long, _
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
    Dim lngNumClaims As Long
    
    g_lngEndTxNum = lngEndTxNum
    g_FileNumber = lngFileNumber
    g_FileName = strFileName
    HL_01 = 0: HL_Parent = 0
    
    Screen.MousePointer = vbHourglass
    lngNumClaims = 0
    
    If rstClrHse.Fields("fldUseCrLfYN").Value = "Y" Then
        g_blnLineFeed = True
    Else
        g_blnLineFeed = False
    End If
    
    If rst.RecordCount > 0 Then
    
        lngPrevClrHseID = rst.Fields("fldClearingHouseID").Value
            
        Call WriteISAHeader(rstClrHse, rst)
        Call WriteGroupHeader(rstClrHse, rst)
        
        Do While Not rst.EOF
            If rstClrHse.Fields("fldReceiverPrefixYN").Value = "Y" Then
                strPayerCode = CreatePayerCodePrefix(rst.Fields("fldInsType").Value, rst.Fields("fldPayerCode").Value) & rst.Fields("fldPayerCode").Value
            Else
                strPayerCode = rst.Fields("fldPayerCode").Value
            End If
            Call WriteTxSetHeader(rstClrHse)
 '           Call WriteClaimStatus(rstClrHse, rst, strPayerCode)
            Call WriteTxSetTrailer
 '           Call StoreCrossReference(rst.Fields("fldClaimID").Value, rst.Fields("fldEncounterLogID").Value, _
                                     g_lngEndTxNum, g_FileName)
            
            lngNumClaims = lngNumClaims + 1
            g_lngNumClaims = g_lngNumClaims + 1
            g_lngEndTxNum = g_lngEndTxNum + 1 'According to THIN, each BHT and ST/SE must have a unique identifier
            
            frmEDIWizard.barStatus.Value = lngNumClaims
            frmEDIWizard.barStatus.Refresh
            
            If Not rst.EOF Then
                rst.MoveNext
                HL_01 = 0: HL_Parent = 0
            Else
                Call WriteGroupTrailer(lngNumClaims)
                Call WriteISATrailer
                Exit Do
            End If
            
            If Not rst.EOF Then
                If lngPrevClrHseID <> rst.Fields("fldClearingHouseID").Value Then
                    Call WriteGroupTrailer(lngNumClaims)
                    Call WriteISATrailer
                    Exit Do
                End If
                If rstClrHse.Fields("fldReceiverPrefixYN").Value = "Y" Then
                    If strPayerCode <> CreatePayerCodePrefix(rst.Fields("fldInsType").Value, rst.Fields("fldPayerCode").Value) & rst.Fields("fldPayerCode").Value Then
                        Call WriteGroupTrailer(lngNumClaims)
                        Call WriteISATrailer
                        Exit Do
                    End If
                Else
                    If strPayerCode <> rst.Fields("fldPayerCode").Value Then
                        Call WriteGroupTrailer(lngNumClaims)
                        Call WriteISATrailer
                        Exit Do
                    End If
                End If
            Else
                Call WriteGroupTrailer(lngNumClaims)
                Call WriteISATrailer
                Exit Do
            End If
        Loop
        
        Set rst = Nothing
    End If
            
    'Signal completion
    'Call MsgBox(lngNumClaims & " claims have been placed into file " & rstClrHse.Fields("fldFolder").Value & "\" & strFileName, vbOKOnly + vbExclamation, "Complete")

    'Return the last Transaction number to calling procedure
    GenerateClaimStatusRequest = g_lngEndTxNum
    
    Screen.MousePointer = vbNormal
    
    Exit Function

ErrTrap:
    Screen.MousePointer = vbNormal
    GenerateClaimStatusRequest = -1
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
            strISA08 = Format(CreatePayerCodePrefix(rst.Fields("fldInsType").Value, rst.Fields("fldPayerCode").Value) & rst.Fields("fldPayerCode").Value, "!@@@@@@@@@@@@@@@")
        Else
            strISA08 = Format(rst.Fields("fldPayerCode").Value, "!@@@@@@@@@@@@@@@")
        End If
    End If
    
    strISA09 = Format(dteCreateDate, "yymmdd")                'interchange date
    strISA10 = Format(Now(), "HHMM")                          'interchange time
    strISA11 = "U"                                            'interchange control standards identifier
    strISA12 = rstClrHse.Fields("fldVersion").Value           'interchange control version number
    strISA13 = Format(g_FileNumber, "000000000")              'interchange control number
    strISA14 = rstClrHse.Fields("fldISA14").Value    'acknowledgment requested (0 = no ack requested; 1 ack requested)
    
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
    
    If rst.Fields("fldClearingHouseID").Value = 27 Then
        Print #1, Left(strISA, 80)
        Print #1, Mid(strISA, 81, 80)
    Else
        If g_blnLineFeed Then Print #1, strISA Else Print #1, strISA;
    End If
    
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
    strGS01 = rstClrHse.Fields("fldGS01").Value             'functional identifier code - "HR"
    strGS02 = rstClrHse.Fields("fldGS02").Value             'application sender's code
    
    If IfNull(rstClrHse.Fields("fldReceiverUsePayerYN").Value, "N") = "N" Then
         strGS03 = rstClrHse.Fields("fldGS03").Value
    Else
        If rstClrHse.Fields("fldReceiverPrefixYN").Value = "Y" Then
            strGS03 = CreatePayerCodePrefix(rst.Fields("fldInsType").Value, rst.Fields("fldPayerCode").Value) & rst.Fields("fldPayerCode").Value
        Else
            strGS03 = rst.Fields("fldPayerCode").Value
        End If
    End If
        
    strGS04 = Format(dteCreateDate, "yyyymmdd")             'date
    strGS05 = Format(dteCreateDate, "hhnn")                 'time
    strGS06 = g_FileNumber                                  'group control number
    strGS07 = "X"                                           'responsible agency code
    strGS08 = rstClrHse.Fields("fldRelease").Value          'version/release/industry identifier code
    
    strGS = "GS*" & strGS01 & "*" & strGS02 & "*" & strGS03 & "*" & strGS04 _
         & "*" & strGS05 & "*" & strGS06 & "*" & strGS07 & "*" & strGS08 & "~"

    If g_blnLineFeed Then Print #1, strGS Else Print #1, strGS;

    Exit Sub
    
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub
Private Sub WriteClaimStatus(ByVal rstClrHse As ADODB.Recordset, ByVal rst As ADODB.Recordset, ByVal strPayerCode As String)

    Dim strBHT As String
    Dim strSvc As String
    Dim strRef As String
    Dim strPRV As String
    
    Call WriteLoop2000A                             'Heirarchy level (Payer)
    Call WriteLoop2100A(rst, strPayerCode)          'Payer Name
    
    Call WriteLoop2000B                             'Heirarchy level (Receiver)
    Call WriteLoop2100B(rstClrHse, rst, strPayerCode)    'Receiver information
    
    Call WriteLoop2000C                             'Heirarchy level (Provider)
    Call WriteLoop2100C(rst)                        'Rendering Provider information
    
    Call WriteLoop2000D(rst)                        'Heirarchy level (Subscriber)
    Call WriteLoop2100D(rst)                        'Patientor Insured information
    Call WriteLoop2200D(rst)                        'Claim Submitter Trace number
    
    Call WriteLoop2210D(rst) 'Service information
    
    If rst.Fields("fldInsType").Value = "BL" And strPayerCode = "560894904" Then    'North Carolina BCBS
    Else
   '     Call WriteLoop2420B(rst, strPayerCode) 'Rendering Provider information for each Service Line
    End If
    
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

Private Sub WriteTxSetHeader(ByVal rstClrHse As ADODB.Recordset)
'Writes header information for a claim. This method writes the ST, BHT, and REF
'data segments of the header.

    On Error GoTo ErrTrap:

    Dim strST As String
    Dim strST01, strST02 As String
    Dim strBHT As String
    Dim strBHT01, strBHT02, strBHT03, strBHT04, strBHT05, strBHT06 As String
    Dim strRef As String
    Dim dteCreateDate As Date
    
    'ST (transaction set header)
    strST01 = "837"  'transaction set identifier code
    strST02 = Format(CStr(g_lngEndTxNum), "00000000") 'transaction set control number
    
    strST = "ST*" & strST01 & "*" & strST02 & "~"
    
    'BHT (beginning of hierarchical transaction)
    dteCreateDate = Now()
    
    strBHT01 = "0019"                               'hierarchical structure code
    strBHT02 = "00"                                 'transaction set purpose code
    strBHT03 = Format(g_lngEndTxNum, "000000")      'reference identification
    strBHT04 = Format(dteCreateDate, "yyyymmdd")    'date
    strBHT05 = Format(dteCreateDate, "hhnn")        'time
    strBHT06 = "CH"                                 'transaction type code (CH = Chargable /RP = Reporting)
    
    strBHT = "BHT*" & strBHT01 & "*" & strBHT02 & "*" & strBHT03 _
         & "*" & strBHT04 & "*" & strBHT05 & "*" & strBHT06 & "~"
    
    'REF (transmission type identification) 'Note: when in production mode value is 004010X098;
    strRef = "REF*87*" & rstClrHse.Fields("fldRelease").Value & "~"

    If g_blnLineFeed Then Print #1, strST Else Print #1, strST;
    g_lngNumSeg = g_lngNumSeg + 1 'increment segment counter

    If g_blnLineFeed Then Print #1, strBHT Else Print #1, strBHT;
    g_lngNumSeg = g_lngNumSeg + 1

    If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
    g_lngNumSeg = g_lngNumSeg + 1

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
    Dim strPRV As String
    
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

Private Function WriteLoop2100A(ByVal rst As ADODB.Recordset, ByVal strPayerCode As String) As Boolean
'Payer information

    On Error GoTo ErrTrap:
    
    Dim strNM101, strNM102, strNM103, strNM104, strNM105 As String
    Dim strNM106, strNM107, strNM108, strNM109, strNM110, strNM111 As String
    Dim strN301, strN302 As String
    Dim strNM1, strN2, strN3, strN4 As String
    Dim strRef, strRef01, strRef02 As String
    
    'Loop 2010BB - Payer Name
    strNM101 = "PR"                'entity identifier code (PR = payer)
    strNM102 = "2"                 'entity type qualifier (2 = non-person)
    strNM103 = Mid(StripDelimiters(IfNull(rst.Fields("fldInsName").Value, "")), 1, 35)       'insurance company name
    strNM104 = ""                  'first name
    strNM105 = ""                  'middle name
    strNM106 = ""                  'name prefix (not used)
    strNM107 = ""                  'name suffix
    strNM108 = "PI"                'identification code qualifier
   
    If rst.Fields("fldInsType").Value = "MC" And strPayerCode = "NCDMH" Then    'North Carolina Medicaid
         strNM103 = "NCXIX"
    End If
    
    If rst.Fields("fldInsType").Value = "BL" And strPayerCode = "560894904" Then    'North Carolina BCBS
         strNM103 = "BCBSNC"
    End If
    
    If rst.Fields("fldInsType").Value = "BL" And strPayerCode = "54771" Then    'Highmark BS
         strNM103 = "HIGHMARK"
    End If
    
    If rst.Fields("fldInsType").Value = "WC" Then
         strNM109 = "TWCCP"
    Else
         strNM109 = strPayerCode
    End If
    
    
    strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
         & "*" & strNM104 & "*" & strNM105 & "*" & strNM106 _
         & "*" & strNM107 & "*" & strNM108 & "*" & strNM109 _
         & "~"
    
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
'Writes Submitter information

    Dim strNM101, strNM102, strNM103, strNM104, strNM105 As String
    Dim strNM106, strNM107, strNM108, strNM109, strNM110, strNM111 As String
    Dim strNM1, strN2 As String
    Dim strPer As String
    Dim strPer01, strPer02, strPer03, strPer04, strPer05, strPer06 As String
     
    'Submitter Name
    strNM101 = "41"         'entity identifier code (41 = submitter)
    strNM102 = "2"          'Qualifier - 2:Non-person entity
    strNM103 = "PSYQUEL"    'Submitter name
    strNM104 = ""           'Individual first name
    strNM105 = ""           'Individual last name
    strNM106 = ""                                           'not used (name prefix)
    strNM107 = ""                                           'not used (name suffix)
    strNM108 = "46"                                         'identification code qualifier (46 = ETIN)
    strNM109 = IfNull(rst.Fields("SubmitterID").Value, rstClrHse.Fields("fldSubmitterID").Value)      'Psyquel Submitter ID

    If rstClrHse.Fields("fldClearingHouseID").Value = 1 Then 'ThinEDI
        If Right(strPayerCode, 5) = "00528" Or _
            Right(strPayerCode, 5) = "00520" Or _
            Right(strPayerCode, 5) = "00882" Then   'Medicare of Louisiana, Arkansas, Medicare RailRoad
            strNM109 = Right(strPayerCode, 5)       'Use Payer Code
        End If
    End If
    
    strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 & "*" & strNM104 _
         & "*" & strNM105 & "*" & strNM106 & "*" & strNM107 & "*" & strNM108 & "*" & strNM109 & "~"
    
    If g_blnLineFeed Then Print #1, strNM1 Else Print #1, strNM1;
    g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
    
    Exit Function
    
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Private Sub WriteLoop2000C()
'Provider Of Service

    Dim strHL, strHL01, strHL02, strHL03, strHL04 As String
    Dim strPat As String
    
    On Error GoTo ErrTrap:
    
    HL_01 = HL_01 + 1
    strHL01 = Format(HL_01, "##0")      'hierarchical ID number
    strHL02 = Format(HL_Parent, "##0")  'hierarchical parent ID
    strHL03 = "19"                      'hierarchical level code (19 = Provider Of Service)
    strHL04 = "1"                       'hierarchical child code
    HL_Parent = HL_01
    
    strHL = "HL*" & strHL01 & "*" & strHL02 & "*" & strHL03 & "*" & strHL04 & "~"
    If g_blnLineFeed Then Print #1, strHL Else Print #1, strHL;
    g_lngNumSeg = g_lngNumSeg + 1
    
    Exit Sub
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub

Private Function WriteLoop2100C(ByVal rst As ADODB.Recordset) As Boolean
'Writes Billing Provider information
    
    Dim strNM101, strNM102, strNM103, strNM104, strNM105 As String
    Dim strNM106, strNM107, strNM108, strNM109, strNM110, strNM111 As String
    Dim strNM1, strN2, strN3, strN4 As String
    Dim strN401, strN402, strN403 As String
    Dim strRef, strRef01, strRef02 As String
    
    On Error GoTo ErrTrap:

    strNM101 = "1P"        'entity code identifier (1P -  provider)
    strNM102 = "1"         'entity type qualifier (1=person; 2=non-person)
       
    strNM103 = Mid(Replace(IfNull(rst.Fields("fldProviderLastName").Value, ""), "'", ""), 1, 35)          'Last name
    strNM104 = Mid(Replace(IfNull(rst.Fields("fldProviderFirstName").Value, ""), "'", ""), 1, 35)   'first name
    strNM105 = IfNull(rst.Fields("fldProviderMI").Value, "")   'middle name
    strNM106 = ""          'name prefix (not used)
    strNM107 = ""          'name suffix (not used)
    
    'identification code qualifier
    If IfNull(rst.Fields("fldIndPracticeNum").Value, "") > "" Then
        strNM108 = "SV"        'individual
        strNM109 = IfNull(rst.Fields("fldIndPracticeNum").Value, "")
    Else
        strNM108 = "FI"        'FTIN
        strNM109 = NumbersOnly(rst.Fields("fldTIN").Value)
    End If
    
    strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 & "*" _
         & strNM104 & "*" & strNM105 & "*" & strNM106 & "*" _
         & strNM107 & "*" & strNM108 & "*" & strNM109 & "~"
    
    If g_blnLineFeed Then Print #1, strNM1 Else Print #1, strNM1;
    g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
       
    Exit Function

ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description

End Function
Private Sub WriteLoop2000D(ByVal rst As ADODB.Recordset)
'Writes heirarchy level (Subscriber).
    
    Dim strHL, strHL01, strHL02, strHL03, strHL04 As String
    Dim strDMG, strDMG01, strDMG02, strDMG03, strDMG04 As String
    Dim dtDOB As Date
    
    HL_01 = HL_01 + 1
    strHL01 = Format(HL_01, "##0")      'hierarchical ID number - always 2 (see p. 108 of X098.pdf)
    strHL02 = Format(HL_Parent, "##0")  'hierarchical parent ID
    strHL03 = "22"                      'hierarchical level code (22 = subscriber)
    
    If rst.Fields("fldPatientID").Value <> rst.Fields("fldRPID").Value And _
        rst.Fields("fldInsType").Value <> "MP" And _
        rst.Fields("fldInsType").Value <> "MB" And _
        rst.Fields("fldInsType").Value <> "MC" Then
        HL_Parent = HL_01               'Keep track of Parent Segment
        strHL04 = "1"                   'we have subordinate in hierarchical child code
    Else
        strHL04 = "0"                   'hierarchical child code Patient is the Insured
    End If
    
    strHL = "HL*" & strHL01 & "*" & strHL02 & "*" & strHL03 & "*" & strHL04 & "~"
    
    'When the subscriber is the patient use DMG Segment
    If rst.Fields("fldPatientID").Value = rst.Fields("fldRPID").Value Then
        dtDOB = IfNull(rst.Fields("fldPatientDOB").Value, IfNull(rst.Fields("fldInsdDOB").Value, 0))
        If IsDate(dtDOB) And dtDOB > 0 Then
            strDMG01 = "D8"
            strDMG02 = Year(dtDOB) & Format(Month(dtDOB), "00") & Format(Day(dtDOB), "00")
            strDMG03 = IfNull(rst.Fields("fldPatientSex").Value, IfNull(rst.Fields("fldInsdSex").Value, "U"))
            strDMG = "DMG*" & strDMG01 & "*" & strDMG02 & "*" & strDMG03 & "~"
            If g_blnLineFeed Then Print #1, strDMG Else Print #1, strDMG;
            g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
        End If
    End If
    
    Exit Sub
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub

Private Function WriteLoop2100D(ByVal rst As ADODB.Recordset)
'Patient name, demographics

    Dim strNM101, strNM102, strNM103, strNM104, strNM105 As String
    Dim strNM106, strNM107, strNM108, strNM109, strNM110, strNM111 As String
    Dim strRef, strRef01, strRef02 As String
    Dim strNM1 As String
    
    On Error GoTo ErrTrap:
    
    'When the subscriber is the patient use QC Segment
    If rst.Fields("fldPatientID").Value = rst.Fields("fldRPID").Value Then
        strNM101 = "QC"         'entity identifier code (QC = submitter)
        strNM102 = "1"          'Qualifier - 2:Non-person entity
        strNM103 = Trim(Mid(Replace(IfNull(rst.Fields("fldPatientLastName").Value, ""), "'", ""), 1, 35))    'Last name
        strNM104 = Trim(Mid(Replace(IfNull(rst.Fields("fldPatientFirstName").Value, ""), "'", ""), 1, 35))   'first name
        strNM105 = Trim(IfNull(rst.Fields("fldPatientMI").Value, ""))   'middle name
    Else
        strNM101 = "IL"         'entity identifier code (IL = Insured)
        strNM102 = "1"          'Qualifier - 2:Non-person entity
        strNM103 = Trim(IfNull(rst.Fields("fldInsuredLastName").Value, Mid(Replace(rst.Fields("fldPatientLastName").Value, "'", ""), 1, 35)))     'Last name
        strNM104 = Trim(IfNull(rst.Fields("fldInsuredFirstName").Value, Mid(Replace(rst.Fields("fldPatientFirstName").Value, "'", ""), 1, 35)))   'first name
        strNM105 = Trim(IfNull(rst.Fields("fldInsuredMI").Value, IfNull(rst.Fields("fldPatientMI").Value, "")))  'middle name
    End If
    strNM106 = ""        'not used (name prefix)
    strNM107 = ""        'not used (name suffix)
    strNM108 = "MI"      'identification code qualifier
    'Subscribers Identification number as assigned by the payer.
    strNM109 = Trim(Replace(StripDelimiters(IfNull(rst.Fields("fldCardNum").Value, "00")), " ", "", 1))
    
    strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
         & "*" & strNM104 & "*" & strNM105 & "*" & strNM106 _
         & "*" & strNM107 & "*" & strNM108 & "*" & strNM109 _
         & "~"
    
    If g_blnLineFeed Then Print #1, strNM1 Else Print #1, strNM1;
    g_lngNumSeg = g_lngNumSeg + 1

    
    Exit Function
    
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description
End Function

Private Sub WriteLoop2200D(ByVal rst As ADODB.Recordset)
'Current Transaction Trace Number
    
    Dim strTrn, strTrn01, strTrn02, strTrn03, strTrn04 As String
    Dim strRef, strRef01, strRef02 As String
    Dim strAmt, strAmt01, strAmt02 As String
    Dim strDTP, strDTP_01, strDTP_02, strDTP_03 As String
    
    On Error GoTo ErrTrap:
        
    strTrn01 = "1"              'Current Transaction Trace Number
    strTrn02 = rst.Fields("fldEncounterLogID").Value   'our account number
    strTrn03 = ""               'not used
    strTrn04 = ""               'not used
    
    strTrn = "TRN*" & strTrn01 & "*" & strTrn02 & "~"
       
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


Private Sub WriteLoop2210D(ByVal rst As ADODB.Recordset)
'Service Line
    
    Dim strSvc As String
    Dim strSV1, strSV1_01, strSV1_02, strSV1_03 As String
    Dim strSV1_04, strSV1_05, strSV1_06 As String
    Dim strSV1_07, strSV1_08, strSV1_09 As String
    Dim strSV2, strSV3, strSV4, strSV5, strSV6, strSV7 As String
    Dim strDTP, strDTP_01, strDTP_02, strDTP_03 As String
    Dim strRef, strRef01, strRef02 As String

    On Error GoTo ErrTrap:
    
    'SV1 segment (professional service)
    strSV1_01 = "HC"                    'product or service qualifer
    If IfNull(rst.Fields("fldModifier").Value, "") > "" Then
        strSV1_01 = strSV1_01 & ":" & rst.Fields("fldCPTCode").Value & ":" & rst.Fields("fldModifier").Value
    Else
        strSV1_01 = strSV1_01 & ":" & rst.Fields("fldCPTCode").Value    'product/service ID/modifier1/modifier2
    End If
    strSV2 = rst.Fields("fldFee").Value * rst.Fields("fldUnits").Value   'monetary amount
    strSV3 = ""                             'Not Used
    strSV4 = "UN"                           'unit or basis for measurement code
    strSV5 = rst.Fields("fldUnits").Value   'quantity (units or minutes)
    strSV6 = ""                             'Service Type Code - Not used
    strSV7 = "1"                            'Units of Service
    
    strSvc = "SV1*" & strSV1_01 & "*" & strSV2 & "*" & strSV3 & "*" & strSV4 & "*" _
        & strSV5 & "*" & strSV6 & "*" & strSV7 & "~"
    If g_blnLineFeed Then Print #1, strSvc Else Print #1, strSvc;
    g_lngNumSeg = g_lngNumSeg + 1
    
    'Line Item Control Number
    strRef01 = "FJ"                'Seq Number
    strRef02 = "001"
    strRef = "REF*" & strRef01 & "*" & strRef02 & "~"
    If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
    g_lngNumSeg = g_lngNumSeg + 1
    
    'Date of Service
    strDTP_01 = "472"                'date/time qualifier
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


Private Function StripDelimiters(ByVal strData As String) As String

    Dim strTemp As String
    
    strTemp = strData 'Make a copy of the data
    strTemp = Replace(strTemp, ":", "", 1)
    strTemp = Replace(strTemp, "*", "", 1)
    strTemp = Replace(strTemp, "-", "", 1)
    strTemp = Replace(strTemp, "~", "", 1)
    strTemp = Replace(strTemp, ".", "", 1)
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
