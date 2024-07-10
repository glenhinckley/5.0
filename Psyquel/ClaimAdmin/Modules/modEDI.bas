Attribute VB_Name = "modEDI"
Option Explicit

Global g_DataBase As String
Private g_lngNumSeg As Long 'Data segment counter
Private g_lngEndTxNum As Long
Private g_FileNumber As Long
Private g_FileName As String
Private g_blnLineFeed As Boolean
Private g_Allowed As Double
Private g_CheckDate As Date
Private HL_01, HL_Parent As Long
Global g_lngNumClaims As Long
Private g_ElementSeperator As String
Private g_PrtString As String
Private g_bln5010 As Boolean
Private Const TX_ECLAIM_SUBMITTED As Long = 6

Public Function GenerateElectronicClaims(ByVal rstClrHse As ADODB.Recordset, ByVal rst As ADODB.Recordset, ByVal lngEndTxNum As Long, _
                                         ByVal lngFileNumber As Long, ByVal strFileName As String, ByVal blnNightlyProcess As Boolean, Optional ByVal strDataBase As String) As Long
'-------------------------------------------------------------------------------
'Author: Dave Richkun
'Date: 11/07/2002
'Description: Main function that generates EDI text file compliant to X.12 837 v4010
'Parameters: rst - Recordset of claims to be electronically generated
'Returns: Null
'-------------------------------------------------------------------------------

    On Error GoTo ErrTrap:
    
    Dim strPayerCode As String
    Dim strPayerCodeTmp As String
    Dim strPrevSubmitterID As String
    Dim strClaimType As String
    Dim strPrevClaimType As String
    Dim lngPrevClrHseID As Long
    Dim lngNumClaims As Long
    Dim dblTotalAmount As Double
    Dim lngNumST_Segments As Long
    Dim dtPrintDate As Date
    Dim objClaim As ClaimBz.CClaimBz
    
    g_DataBase = strDataBase    ''Need to read according to the Database
    g_lngEndTxNum = lngEndTxNum
    g_FileNumber = lngFileNumber
    g_FileName = strFileName
    HL_01 = 0: HL_Parent = 0
    lngNumClaims = 0: lngNumST_Segments = 0: dblTotalAmount = 0
    
    Screen.MousePointer = vbHourglass
    g_blnLineFeed = False: g_bln5010 = False
    If rstClrHse.Fields("fldUseCrLfYN").Value = "Y" Then g_blnLineFeed = True
    If rstClrHse.Fields("fldVersion").Value = "00501" Then g_bln5010 = True
    
    If rst.RecordCount > 0 Then
    
        strPrevClaimType = rst.Fields("fldClaimType").Value
        lngPrevClrHseID = rst.Fields("fldClearingHouseID").Value
        strPrevSubmitterID = IfNull(rst.Fields("SubmitterID").Value, "")
            
       ' g_lngEndTxNum = g_lngEndTxNum + 1 'According to THIN, each BHT and ST/SE must have a unique identifier

        Call WriteISAHeader(rstClrHse, rst)
        Call WriteGroupHeader(rstClrHse, rst)
        Call WriteTxSetHeader(rstClrHse, rst)
        lngNumST_Segments = lngNumST_Segments + 1
        If rstClrHse.Fields("fldReceiverPrefixYN").Value = "Y" Then
            strPayerCode = CreatePayerCodePrefix(rst.Fields("fldInsType").Value, rst.Fields("fldPayerCode").Value) & rst.Fields("fldPayerCode").Value
        Else
            strPayerCode = rst.Fields("fldPayerCode").Value
        End If
        
        Call WriteLoop1000A(rstClrHse, rst, strPayerCode) 'Submitter information
        Call WriteLoop1000B(rstClrHse, rst, strPayerCode) 'Receiver information
        
        Do While Not rst.EOF
            
            Call WriteClaim(rstClrHse, rst, strPayerCode)
            
            Call StoreCrossReference(rst.Fields("fldClaimID").Value, rst.Fields("fldEncounterLogID").Value, _
                                     g_lngEndTxNum, g_FileName, rst.Fields("fldDataBase"))
                                     
            If blnNightlyProcess Then
               'Update the database with the claim submit date
               Set objClaim = CreateObject("ClaimBz.CClaimBz")
               dtPrintDate = CDate(Format(Now, "mm/dd/yyyy hh:nn"))
               Call objClaim.SubmitClaim(dtPrintDate, rst.Fields("fldClaimID").Value, rst.Fields("fldEncounterLogID").Value, rst.Fields("fldPlanID").Value, TX_ECLAIM_SUBMITTED, -1, GetLoginName(), rst.Fields("fldDataBase"))
               Set objClaim = Nothing
            End If
            
            dblTotalAmount = dblTotalAmount + (IfNull(rst.Fields("fldFee").Value, 0) * IfNull(rst.Fields("fldUnits").Value, 1))
            lngNumClaims = lngNumClaims + 1
            g_lngNumClaims = g_lngNumClaims + 1
       
            frmEDIWizard.barStatus.Value = g_lngNumClaims
            frmEDIWizard.barStatus.Refresh
            
            If Not rst.EOF Then
                rst.MoveNext
            Else
                Call WriteTxSetTrailer(rstClrHse)
                Call WriteGroupTrailer(rstClrHse, lngNumST_Segments)
                Call WriteISATrailer(rstClrHse)
                Exit Do
            End If
                        
            If Not rst.EOF Then
                If strPrevClaimType <> rst.Fields("fldClaimType").Value Or _
                   lngPrevClrHseID <> rst.Fields("fldClearingHouseID").Value Or _
                   strPrevSubmitterID <> IfNull(rst.Fields("SubmitterID").Value, "") Then
                    Call WriteTxSetTrailer(rstClrHse)
                    Call WriteGroupTrailer(rstClrHse, lngNumST_Segments)
                    Call WriteISATrailer(rstClrHse)
                    Exit Do
                End If
                If rstClrHse.Fields("fldReceiverPrefixYN").Value = "Y" Then
                    If strPayerCode <> CreatePayerCodePrefix(rst.Fields("fldInsType").Value, rst.Fields("fldPayerCode").Value) & rst.Fields("fldPayerCode").Value Then
                        Call WriteTxSetTrailer(rstClrHse)
                        Call WriteGroupTrailer(rstClrHse, lngNumST_Segments)
                        Call WriteISATrailer(rstClrHse)
                        Exit Do
                    End If
                Else
                    If strPayerCode <> rst.Fields("fldPayerCode").Value Then
                        If rst.Fields("fldClearingHouseID").Value = 1 Or _
                           rst.Fields("fldClearingHouseID").Value = 35 Or _
                           rst.Fields("fldClearingHouseID").Value = 90 Then  'Availity, Post-N-Track
                            Call WriteTxSetTrailer(rstClrHse)
                            g_lngEndTxNum = g_lngEndTxNum + 1
                            HL_01 = 0: HL_Parent = 0
                            strPayerCode = rst.Fields("fldPayerCode").Value
                            Call WriteTxSetHeader(rstClrHse, rst)
                            lngNumST_Segments = lngNumST_Segments + 1
                            Call WriteLoop1000A(rstClrHse, rst, strPayerCode) 'Submitter information
                            Call WriteLoop1000B(rstClrHse, rst, strPayerCode)  'Receiver information
                        Else
                            Call WriteTxSetTrailer(rstClrHse)
                            Call WriteGroupTrailer(rstClrHse, lngNumST_Segments)
                            Call WriteISATrailer(rstClrHse)
                            Exit Do
                        End If
                    End If
                End If
                If rst.Fields("fldClearingHouseID").Value = 62 Then
                    Call WriteTxSetTrailer(rstClrHse)
                    g_lngEndTxNum = g_lngEndTxNum + 1
                    HL_01 = 0: HL_Parent = 0
                    strPayerCode = CreatePayerCodePrefix(rst.Fields("fldInsType").Value, rst.Fields("fldPayerCode").Value) & rst.Fields("fldPayerCode").Value
                    Call WriteTxSetHeader(rstClrHse, rst)
                    lngNumST_Segments = lngNumST_Segments + 1
                    Call WriteLoop1000A(rstClrHse, rst, strPayerCode) 'Submitter information
                    Call WriteLoop1000B(rstClrHse, rst, strPayerCode)  'Receiver information
                End If
            Else
                Call WriteTxSetTrailer(rstClrHse)
                Call WriteGroupTrailer(rstClrHse, lngNumST_Segments)
                Call WriteISATrailer(rstClrHse)
                Exit Do
            End If
        Loop
        
        Set rst = Nothing
    End If
            
    'Signal completion
    'Call MsgBox(lngNumClaims & " claims have been placed into file " & rstClrHse.Fields("fldFolder").Value & "\" & strFileName, vbOKOnly + vbExclamation, "Complete")
    
    'Return the last Transaction number to calling procedure
    GenerateElectronicClaims = g_lngEndTxNum
    
    Screen.MousePointer = vbNormal
    
    Exit Function

ErrTrap:
    Screen.MousePointer = vbNormal
    GenerateElectronicClaims = -1
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
     
    If (rstClrHse.Fields("fldClearingHouseID").Value = 31) Then
        If g_blnLineFeed Then
            'Print #1, "$$ADD ID=" & rst.Fields("SubmitterID").Value & " BID='PANSI837P'"
            Print #1, "$$ADD ID=" & rst.Fields("SubmitterID").Value & " BID='ASC837' SCAN=Y"
        Else
            'Print #1, "$$ADD ID=" & rst.Fields("SubmitterID").Value & " BID='PANSI837P'";
            Print #1, "$$ADD ID=" & rst.Fields("SubmitterID").Value & " BID='ASC837' SCAN=Y"; 'SCAN=Y is for the MULTIFILE
        End If
    End If
    'ISA (Interchange Control Header)
    'Note: The ISA header is a fixed length segment
    strISA01 = Format(rstClrHse.Fields("fldISA01").Value, "!@@")                            'Security information qualifier
    strISA02 = Format(IfNull(rstClrHse.Fields("fldISA02").Value, " "), "!@@@@@@@@@@")       'Security information
    strISA03 = Format(rstClrHse.Fields("fldISA03").Value, "!@@")                            'Security information qualifier
    strISA04 = Format(IfNull(rstClrHse.Fields("fldISA04").Value, " "), "!@@@@@@@@@@")       'Security information
    strISA05 = Format(rstClrHse.Fields("fldISA05").Value, "!@@")                            'Interchange ID qualifier (ZZ = mutually defined)
    
    'Interchange sender ID (length must be 15)
    strISA06 = Format(IfNull(rstClrHse.Fields("fldISA06").Value, " "), "!@@@@@@@@@@@@@@@")
    If (rstClrHse.Fields("fldClearingHouseID").Value = 31 Or rstClrHse.Fields("fldClearingHouseID").Value = 153) And rst.Fields("SubmitterID").Value > "" Then
      strISA06 = Format(IfNull(rst.Fields("SubmitterID").Value, " "), "!@@@@@@@@@@@@@@@")
    End If
    strISA07 = Format(rstClrHse.Fields("fldISA07").Value, "!@@")                            'Interchange ID qualifier (ZZ = mutually defined)
    
    If IfNull(rstClrHse.Fields("fldReceiverUsePayerYN").Value, "N") = "N" Then
        strISA08 = Format(IfNull(rstClrHse.Fields("fldISA08").Value, " "), "!@@@@@@@@@@@@@@@") ' Interchange Receiver ID
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
    If g_bln5010 Then strISA11 = "^"
    strISA12 = rstClrHse.Fields("fldVersion").Value           'interchange control version number
    strISA13 = Format(g_FileNumber, "000000000")              'interchange control number
    strISA14 = rstClrHse.Fields("fldISA14").Value    'acknowledgment requested (0 = no ack requested; 1 ack requested)
    
    'usage indicator (P = production data / T = test data)
    If frmEDIWizard.chkTest.Value = vbChecked Then
        strISA15 = "T"
    Else
        strISA15 = rstClrHse.Fields("fldTransmitMethod").Value
    End If
    
    If rst.Fields("fldClearingHouseID").Value = 76 Then
       g_ElementSeperator = ">"                'component element separator
    Else
       g_ElementSeperator = ":"                'component element separator
    End If
    strISA16 = g_ElementSeperator                'component element separator
    
    strISA = "ISA*" & strISA01 & "*" & strISA02 & "*" & strISA03 & "*" & strISA04 _
      & "*" & strISA05 & "*" & strISA06 & "*" & strISA07 & "*" & strISA08 & "*" & strISA09 _
      & "*" & strISA10 & "*" & strISA11 & "*" & strISA12 & "*" & strISA13 & "*" & strISA14 & "*" & strISA15 & "*" & strISA16 & "~"
  
    If rst.Fields("fldClearingHouseID").Value = 999 Or _
       rst.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = strISA
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
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
    strGS01 = rstClrHse.Fields("fldGS01").Value             'functional identifier code
    
    'application sender's code
    strGS02 = rstClrHse.Fields("fldGS02").Value
    If (rstClrHse.Fields("fldClearingHouseID").Value = 31 Or rstClrHse.Fields("fldClearingHouseID").Value = 153) And rst.Fields("SubmitterID").Value > "" Then
      strGS02 = rst.Fields("SubmitterID").Value
    End If
   
    strGS03 = rstClrHse.Fields("fldGS03").Value
    If IfNull(rstClrHse.Fields("fldReceiverUsePayerYN").Value, "N") = "N" Then
    Else
        If rstClrHse.Fields("fldReceiverPrefixYN").Value = "Y" Then
            strGS03 = CreatePayerCodePrefix(rst.Fields("fldInsType").Value, rst.Fields("fldPayerCode").Value) & rst.Fields("fldPayerCode").Value
        Else
            strGS03 = rst.Fields("fldPayerCode").Value
        End If
    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 29 And rst.Fields("fldPayerCode").Value = "54763" Then
        strGS03 = rst.Fields("fldPayerCode").Value
    End If
    If rstClrHse.Fields("fldClearingHouseID").Value = 29 And rst.Fields("fldPayerCode").Value = "TA720" Then
        strGS03 = rst.Fields("fldPayerCode").Value
    End If
        
    strGS04 = Format(dteCreateDate, "yyyymmdd")             'date
    strGS05 = Format(dteCreateDate, "hhnn")                 'time
    strGS06 = g_FileNumber                                  'group control number
    strGS07 = "X"                                           'responsible agency code
    strGS08 = rstClrHse.Fields("fldRelease").Value          'version/release/industry identifier code
    If rst.Fields("fldClaimType").Value = "I" Then
        If g_bln5010 Then
            strGS08 = "005010X223A2"
        Else
            strGS08 = "004010X096A1" 'Institutional Claims
        End If
    End If
    
    strGS = "GS*" & strGS01 & "*" & strGS02 & "*" & strGS03 & "*" & strGS04 _
         & "*" & strGS05 & "*" & strGS06 & "*" & strGS07 & "*" & strGS08 & "~"

    If rst.Fields("fldClearingHouseID").Value = 999 Or _
       rst.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strGS
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strGS Else Print #1, strGS;
    End If

    Exit Sub
    
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub

Private Sub WriteTxSetHeader(ByVal rstClrHse As ADODB.Recordset, ByVal rst As ADODB.Recordset)
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
    If g_bln5010 Then strST = "ST*" & strST01 & "*" & strST02 & "*" & rstClrHse.Fields("fldRelease").Value & "~"
    If rst.Fields("fldClaimType").Value = "I" Then
        If g_bln5010 Then
            strST = "ST*" & strST01 & "*" & strST02 & "*" & "005010X223A2" & "~" 'Institutional Claims
        Else
            strST = "ST*" & strST01 & "*" & strST02 & "*" & "004010X096A1" & "~" 'Institutional Claims
        End If
    End If
    
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
    If rst.Fields("fldClaimType").Value = "I" Then
        If g_bln5010 Then
            strRef = "REF*87*" & "005010X223A2" & "~" 'Institutional Claims
        Else
            strRef = "REF*87*" & "004010X096A1" & "~" 'Institutional Claims
        End If
    End If

    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strST
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strST Else Print #1, strST;
    End If
    g_lngNumSeg = g_lngNumSeg + 1 'increment segment counter

    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strBHT
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strBHT Else Print #1, strBHT;
    End If
    g_lngNumSeg = g_lngNumSeg + 1

    If Not g_bln5010 Then '5010 does not use this segment
        If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
            g_PrtString = g_PrtString + strRef
            If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
            End If
        Else
            If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
        End If
        g_lngNumSeg = g_lngNumSeg + 1
    End If

    Exit Sub
    
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub

Private Sub WriteClaim(ByVal rstClrHse As ADODB.Recordset, ByVal rst As ADODB.Recordset, ByVal strPayerCode As String)

    Dim objEncDetail As EncounterLogBz.CEncounterLogBz
    Dim rstDetail As ADODB.Recordset
    Dim strBHT As String
    Dim strSvc As String
    Dim strRef As String
    Dim strPRV As String
    Dim intLine, I As Integer
    Dim arr As Variant
    
    Call WriteLoop2000A(rst, rstClrHse)   'Heirarchy level (Billing Provider)
    Call WriteLoop2010AA(rst, rstClrHse, strPayerCode)  'Billing Provider information
    If InStr(1, StripDelimiters(IfNull(rst.Fields("fldBillingCompanyLine2").Value, rst.Fields("fldProviderAddress").Value)), "PO Box", vbTextCompare) > 0 Then
        Call WriteLoop2010AB(rst, rstClrHse, strPayerCode)  'Pay To Provider information
    End If
    
    Call WriteLoop2000B(rst, rstClrHse, strPayerCode)  'Heirarchy level (Subscriber)
    Call WriteLoop2010BA(rst, rstClrHse, strPayerCode) 'Subscriber information
    
    Call WriteLoop2010BB(rst, rstClrHse, strPayerCode) 'Payer information
    
    'Medicare and Medicaid do not allow any relationship but self
    If rst.Fields("fldPatientID").Value <> rst.Fields("fldRPID").Value And _
        rst.Fields("fldInsType").Value <> "MP" And _
        rst.Fields("fldInsType").Value <> "MB" And _
        rst.Fields("fldInsType").Value <> "MC" Then
        'Patient information.  This loop is written only if the patient is not the
        'insured/subscriber.  Otherwise the patient and insured/subscriber information
        'is identical and has already been written in Loop 2010BA.
        Call WriteLoop2000C(rst, rstClrHse) 'Patient information
        Call WriteLoop2010CA(rst, rstClrHse) 'Patient name, demographibs
    ElseIf rst.Fields("fldPatientID").Value = 0 And rst.Fields("fldRPID").Value = 0 And _
        rst.Fields("fldPatientFirstName").Value <> rst.Fields("fldInsuredFirstName").Value Then
        Call WriteLoop2000C(rst, rstClrHse) 'Patient information
        Call WriteLoop2010CA(rst, rstClrHse) 'Patient name, demographibs
    End If
    
    Call WriteLoop2300(rst, rstClrHse, strPayerCode) 'Claim information
    
    If Trim(rst.Fields("fldReferPhy").Value) > "" Or (Trim(rst.Fields("fldReferLast").Value) > "" And Trim(rst.Fields("fldReferFirst").Value) > "") Then
        Call WriteLoop2310A(rst, rstClrHse) 'Referring Physician
    End If
    
    'Rendering Provider information
    If rst.Fields("fldInsuranceID").Value = 1317 Or rst.Fields("fldInsuranceID").Value = 1624 Or rst.Fields("fldPlanID").Value = 4429 Or _
       rst.Fields("fldPlanID").Value = 4666 Or rst.Fields("fldPlanID").Value = 3437 Then   ' Managed Health, Beacon Heath Strategies, Wellcare Requires Rendering Provider  ----> Added 10/17/2017
        Call WriteLoop2310B(rst, rstClrHse)
    Else
        'Rendering Provider information.  This loop is written only if the Rendering provider
        'is part of a group.
        If (IfNull(rst.Fields("fldGroupNPI").Value, "") > "" Or IfNull(rst.Fields("fldGrpPracticeNum").Value, "") > "") And _
           (IfNull(rst.Fields("fldProviderNPI").Value, "") > "" Or IfNull(rst.Fields("fldSuperNPI").Value, "") > "") Then
            Call WriteLoop2310B(rst, rstClrHse)
        End If
    End If
    
    'Facility Provider
    'Facility address is needed if it is not an office visit or if the Billing address is not a physical address.
    'LockBox Providers will have a different address for there billing and there facility
    If (rst.Fields("fldPOS").Value = "11") And rstClrHse.Fields("fldClearingHouseID").Value <> 152 Then
        If (InStr(1, StripDelimiters(IfNull(rst.Fields("fldBillingCompanyLine2").Value, rst.Fields("fldProviderAddress").Value)), "PO Box", vbTextCompare) > 0) Or _
           (IfNull(rst.Fields("fldBillingCompanyCity").Value, IfNull(rst.Fields("fldFacilityCity").Value, rst.Fields("fldProviderCity").Value)) <> IfNull(rst.Fields("fldFacilityCity").Value, rst.Fields("fldProviderCity").Value)) Or _
           (Replace(IfNull(rst.Fields("fldBillingCompanyZip").Value, IfNull(rst.Fields("fldFacilityZip").Value, "")), "-", "") <> Replace(IfNull(rst.Fields("fldFacilityZip").Value, IfNull(rst.Fields("fldBillingCompanyZip").Value, "")), "-", "")) Or _
           (rst.Fields("fldPlanID").Value = 4666 Or rst.Fields("fldPlanID").Value = 4965 Or rst.Fields("fldPlanID").Value = 5172) Then  '4965 - AmeriGroup of Texas, Managed Health and Martin's Point wants this loop
                                                                        'And (rst.Fields("fldInsuranceID").Value <> 1317) 'removed Beacon 08/29/2017
            Call WriteLoop2310C(rst, rstClrHse)
        End If
    ElseIf rst.Fields("fldPOS").Value = "02" And rstClrHse.Fields("fldClearingHouseID").Value <> 152 Then
         If (IfNull(rst.Fields("fldBillingCompanyCity").Value, IfNull(rst.Fields("fldFacilityCity").Value, rst.Fields("fldProviderCity").Value)) <> IfNull(rst.Fields("fldFacilityCity").Value, rst.Fields("fldProviderCity").Value)) Or _
            (Replace(IfNull(rst.Fields("fldBillingCompanyZip").Value, IfNull(rst.Fields("fldFacilityZip").Value, "")), "-", "") <> Replace(IfNull(rst.Fields("fldFacilityZip").Value, IfNull(rst.Fields("fldBillingCompanyZip").Value, "")), "-", "")) Or _
           (rst.Fields("fldPlanID").Value = 4666 Or rst.Fields("fldPlanID").Value = 4965 Or rst.Fields("fldPlanID").Value = 5172) Then  '4965 - AmeriGroup of Texas, Managed Health and Martin's Point wants this loop
            Call WriteLoop2310C(rst, rstClrHse)
        End If
    ElseIf rstClrHse.Fields("fldClearingHouseID").Value = 152 Then  'MaineCare
        If IfNull(rst.Fields("fldIndSecondaryID").Value, "") > "" Then
            Call WriteLoop2310C(rst, rstClrHse)
        End If
    ElseIf (rst.Fields("fldPOS").Value <> "11" And rst.Fields("fldPOS").Value <> "02" And rst.Fields("fldPOS").Value <> "10") Then      'pos 11, 02 and 10
      If (rst.Fields("fldShortageAreaYN").Value = "Y" And (rst.Fields("fldInsType").Value = "MP" Or rst.Fields("fldInsType").Value = "MB")) Then
         If rst.Fields("fldClaimType").Value <> "I" Then
            Call WriteLoop2310C(rst, rstClrHse)
         End If
      ElseIf (rst.Fields("fldShortageAreaYN").Value = "N" And rst.Fields("fldPOS").Value <> "12" And (rst.Fields("fldInsType").Value <> "MP" And rst.Fields("fldInsType").Value <> "MB")) Then
         If rst.Fields("fldClaimType").Value <> "I" Then
            Call WriteLoop2310C(rst, rstClrHse)
         End If
      End If
    End If
        
    'Required for testing California Medicare South
    If rst.Fields("fldUseSuperYN").Value = "Y" Then
        If IfNull(rst.Fields("fldSuperNPI").Value, "") <> IfNull(rst.Fields("fldProviderNPI").Value, "") And _
           IfNull(rst.Fields("fldSuperNPI").Value, "") > "" And _
           IfNull(rst.Fields("fldProviderNPI").Value, "") > "" And _
           rst.Fields("fldInsuranceID").Value <> 1235 Then
            Call WriteLoop2310E(rst, rstClrHse) 'Supervising Physician
        End If
    End If
        
    If (Not IsNull(rst.Fields("fldOthPlanName").Value) And _
        Not IsNull(rst.Fields("fldOthInsdLastName").Value) And _
        IfNull(rst.Fields("fldOthInsMedigapPayerCode").Value, "") > "") Or _
       (Not IsNull(rst.Fields("fldOthPlanName").Value) And _
        Not IsNull(rst.Fields("fldOthInsdLastName").Value) And _
        Not IsNull(rst.Fields("fldOthInsdDOB").Value) And _
        IfNull(rst.Fields("fldOthInsPayerCode").Value, "") > "") Or _
       (Not IsNull(rst.Fields("fldOthPlanName").Value) And _
        Not IsNull(rst.Fields("fldOthInsdLastName").Value) And _
       (rst.Fields("fldOrder").Value > 1)) Then    'Other Insurance Secondary
        Call WriteLoop2320(rst, rstClrHse)
        Call WriteLoop2330(rst, rstClrHse)
        Call WriteLoop2330B(rst, rstClrHse)
    End If
   
 '      (Not IsNull(rst.Fields("fldOthPlanName").Value) And
 '       Not IsNull(rst.Fields("fldOthInsdLastName").Value)) Or
   
    Set objEncDetail = CreateObject("EncounterLogBz.CEncounterLogBz")
    Set rstDetail = objEncDetail.FetchClaimDetailByClaimID(rst.Fields("fldClaimID").Value, rst.Fields("fldDataBase"))    ''Need to read according to the Database
    Set objEncDetail = Nothing
    
    intLine = 0
    If rstDetail.EOF Then
    End If
    For I = 1 To rstDetail.RecordCount
        If rstDetail.Fields("fldPlanID").Value = rst.Fields("fldPlanID").Value And rstDetail.Fields("fldInsuranceID").Value = rst.Fields("fldInsuranceID").Value Then
            intLine = intLine + 1
            Call WriteLoop2400(rst, rstClrHse, intLine, rstDetail)
            If rst.Fields("fldOrder").Value > 1 Then Call WriteLoop2430(rst, rstClrHse, rstDetail, "P")
            If rst.Fields("fldOrder").Value > 2 Then Call WriteLoop2430(rst, rstClrHse, rstDetail, "S")
        ElseIf rst.Fields("fldOrder").Value > 1 Then
            intLine = intLine + 1
            Call WriteLoop2400(rst, rstClrHse, intLine, rstDetail)
            If rst.Fields("fldOrder").Value > 1 Then Call WriteLoop2430(rst, rstClrHse, rstDetail, "P")
            If rst.Fields("fldOrder").Value > 2 Then Call WriteLoop2430(rst, rstClrHse, rstDetail, "S")
        End If
        rstDetail.MoveNext
    Next I
    Set rstDetail = Nothing
    
    'AddOn CPTCode is present
'    If rst.Fields("fldAddOnCPTCode").Value > "" Then
'        intLine = intLine + 1
'   '     Call WriteLoop2400_Line02(rst, rstClrHse) 'Service information
'        Call WriteLoop2400(rst, rstClrHse, intLine, IIf(IsNull(rst.Fields("fldAddOnMod1").Value), "", rst.Fields("fldAddOnMod1").Value), IIf(IsNull(rst.Fields("fldAddOnMod2").Value), "", rst.Fields("fldAddOnMod2").Value), _
'            rst.Fields("fldAddOnCPTCode").Value, "", IIf(IsNull(rst.Fields("fldAddOnFee").Value), 0, rst.Fields("fldAddOnFee").Value), IIf(IsNull(rst.Fields("fldAddOnUnits").Value), 1, rst.Fields("fldAddOnUnits").Value))
'
'        If rst.Fields("fldOrder").Value > 1 Then Call WriteLoop2430_Line02(rst, rstClrHse, "P")
'        If rst.Fields("fldOrder").Value > 2 Then Call WriteLoop2430_Line02(rst, rstClrHse, "S")
'    End If
    
    
    'AddOnSec CPTCode is present
'    If rst.Fields("fldAddOnSecCPTCode").Value > "" And rst.Fields("fldAddOnCPTCode").Value > "" Then
'        intLine = intLine + 1
 '       Call WriteLoop2400_Line03(rst, rstClrHse) 'Service information
'        Call WriteLoop2400(rst, rstClrHse, intLine, IIf(IsNull(rst.Fields("fldAddOnSecMod1").Value), "", rst.Fields("fldAddOnSecMod1").Value), IIf(IsNull(rst.Fields("fldAddOnSecMod2").Value), "", rst.Fields("fldAddOnSecMod2").Value), _
'            rst.Fields("fldAddOnSecCPTCode").Value, "", IIf(IsNull(rst.Fields("fldAddOnSecFee").Value), 0, rst.Fields("fldAddOnSecFee").Value), IIf(IsNull(rst.Fields("fldAddOnSecUnits").Value), 1, rst.Fields("fldAddOnSecUnits").Value))
    
'        If rst.Fields("fldOrder").Value > 1 Then Call WriteLoop2430_Line03(rst, rstClrHse, "P")
'        If rst.Fields("fldOrder").Value > 2 Then Call WriteLoop2430_Line03(rst, rstClrHse, "S")
'    End If
    
    'MIPS is present
'    If rst.Fields("fldMips").Value > "" Then
'        arr = Split(rst.Fields("fldMips").Value, ":")
'        If IsArray(arr) Then
'            For I = 0 To (UBound(arr) - 1)
'                intLine = intLine + 1
'               '''' Call WriteLoop2400(rst, rstClrHse, intLine, rst, arr(I), "Y")
'            Next
'        End If
'    End If
    
''    If rst.Fields("fldeScribeYN").Value = "Y" Then
''        Call WriteLoop2400_Line04(rst, rstClrHse) 'Service information
''        intLine = intLine + 1
''        Call WriteLoop2400(rst, rstClrHse, intLine, "", "", "G8553", "", 0, 1)
''        If rst.Fields("fldOrder").Value > 1 Then Call WriteLoop2430_Line04(rst, rstClrHse)
''    End If
End Sub

Private Sub WriteISATrailer(ByVal rstClrHse As ADODB.Recordset)
    
    On Error GoTo ErrTrap:
    
    'ISA footer
    Dim strIEA As String
    strIEA = "IEA*1*" & Format(g_FileNumber, "000000000") & "~"

    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strIEA
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
        If g_blnLineFeed Then Print #1, g_PrtString Else Print #1, g_PrtString;
    Else
        If g_blnLineFeed Then Print #1, strIEA Else Print #1, strIEA;
    End If
    
    Exit Sub
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub

Private Sub WriteGroupTrailer(ByVal rstClrHse As ADODB.Recordset, ByVal intNumStSegments As Integer)

    On Error GoTo ErrTrap:
    
    'GS footer
    Dim strGE As String

    strGE = "GE*" & CStr(intNumStSegments) & "*" & CStr(g_FileNumber) & "~"
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strGE
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strGE Else Print #1, strGE;
    End If
    
    Exit Sub
    
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub
Private Function WriteLoop1000A(ByVal rstClrHse As ADODB.Recordset, ByVal rst As ADODB.Recordset, ByVal strPayerCode As String)
'Writes Submitter information

    Dim strNM101, strNM102, strNM103, strNM104, strNM105 As String
    Dim strNM106, strNM107, strNM108, strNM109, strNM110, strNM111 As String
    Dim strNM1, strN2 As String
    Dim strPer As String
    Dim strPer01, strPer02, strPer03, strPer04, strPer05, strPer06 As String
     
    'Submitter Name
    strNM101 = "41"         'entity identifier code (41 = submitter)
    strNM102 = "2"          'Qualifier - 2:Non-person entity
    strNM103 = "PSYQUEL"  'Submitter name
    strNM104 = ""           'Individual first name
    strNM105 = ""           'Individual last name
    strNM106 = ""                                           'not used (name prefix)
    strNM107 = ""                                           'not used (name suffix)
    strNM108 = "46"                                         'identification code qualifier (46 = ETIN)
    strNM109 = IfNull(rst.Fields("SubmitterID").Value, rstClrHse.Fields("fldSubmitterID").Value)    'Psyquel Submitter ID
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 200 Then
        strNM103 = "REIMBURSIFY"  'Submitter name
    End If
    
    strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 & "*" & strNM104 _
         & "*" & strNM105 & "*" & strNM106 & "*" & strNM107 & "*" & strNM108 & "*" & strNM109 & "~"
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strNM1
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strNM1 Else Print #1, strNM1;
    End If
    g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters

    'Submitter Contact Information
    strPer01 = "IC"                    'contact function code (IC = Information Contact)
    strPer02 = ""    '5010 does not use this segment
    If rst.Fields("fldInsuranceID").Value = 1268 And _
        (Trim(Replace(StripDelimiters(IfNull(rst.Fields("fldCardNum").Value, "00")), " ", "", 1)) Like "XCSF*" Or _
         Trim(Replace(StripDelimiters(IfNull(rst.Fields("fldCardNum").Value, "00")), " ", "", 1)) Like "XCXF*") Then
        strPer02 = "PSYQUEL SOLUTIONS"     'Submitter contact name
    End If
    strPer03 = "TE"                    'communication number qualifier (TE = Telephone)
    strPer04 = "2106941354"            'communication number (phone should be XXXYYYZZZZ)
    strPer05 = "FX"                    'communication number qualifier (FX = Fax)
    strPer06 = "2106941399"            'communication number (phone should be XXXYYYZZZZ)
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 200 Then
        strPer04 = "9175383905"            'communication number (phone should be XXXYYYZZZZ)
        strPer06 = "9175383905"            'communication number (phone should be XXXYYYZZZZ)
    End If
    
    strPer = "PER*" & strPer01 & "*" & strPer02 & "*" & strPer03 & "*" & strPer04 & "*" & strPer05 & "*" & strPer06 & "~"

    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strPer
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strPer Else Print #1, strPer;
    End If
    g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
    
    Exit Function
    
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description

End Function


Private Function WriteLoop1000B(ByVal rstClrHse As ADODB.Recordset, ByVal rst As ADODB.Recordset, ByVal strPayerCode As String) As Boolean
'Writes Receiver information

    Dim strNM101, strNM102, strNM103, strNM104, strNM105 As String
    Dim strNM106, strNM107, strNM108, strNM109, strNM110, strNM111 As String
    Dim strNM1, strN2 As String
    
    On Error GoTo ErrTrap:
    
    strNM101 = "40"            'entity ID code (40 = receiver)
    strNM102 = "2"             'entity type qualifier (2 = non-person)
    strNM103 = UCase(Trim(StripDelimiters(Mid(IfNull(rst.Fields("fldPlanName").Value, ""), 1, 35)))) 'last name or organization
    strNM104 = ""              'first name (optional)
    strNM105 = ""              'middle name (optional)
    strNM106 = ""              'name prefix (optional)
    strNM107 = ""              'name suffix (optional)
    strNM108 = "46"            'ID code qualifier (46 = ETIN)
    
    'Since we are only creating one claim per ST then the ST is not mixed!
    strNM109 = strPayerCode
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 47 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 57 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 121 Then    'BC/BS of Western New York, Bc/Bs Of KC
        strNM103 = "ASK INC"
        strNM109 = rstClrHse.Fields("fldReceiverID").Value
    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 27 Then    'BC of Ca
        strNM103 = "ANTHEM BLUE CROSS"
        strNM109 = rstClrHse.Fields("fldReceiverID").Value
    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 50 Then    'Virginia Medicaid
        strNM103 = "DEPT OF MED ASSIST SVCS"
        strNM109 = rstClrHse.Fields("fldReceiverID").Value
    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 61 Then    'Computer Sciences of NY
        strNM103 = "NYSDOH"
        strNM109 = rstClrHse.Fields("fldReceiverID").Value
    End If
    
 '   If rstClrHse.Fields("fldClearingHouseID").Value = 63 Then    'Maryland Health Partners
 '       strNM103 = "MARYLAND PMHS"
 '       strNM109 = rstClrHse.Fields("fldReceiverID").Value
 '   End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 66 Then    'BC/BS of Rhode Island
        strNM103 = "BCBS OF RHODE ISLAND"
        strNM109 = rstClrHse.Fields("fldReceiverID").Value
    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 69 Then    'BC/BS of Nebraska
        strNM103 = "NEBLUECONNECT"
        strNM109 = rstClrHse.Fields("fldReceiverID").Value
    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 71 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 90 Then    'BC/BS of Minnesota, Post-n-Track
        strNM109 = rstClrHse.Fields("fldReceiverID").Value
    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 84 Then    'Tricare South
        strNM103 = "TRICARE"
        strNM109 = rstClrHse.Fields("fldReceiverID").Value
    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 92 Then    'Tricare South
        strNM103 = "HAWAII MEDICAL SERVICE ASSOCIATION"
        strNM109 = rstClrHse.Fields("fldReceiverID").Value
    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 106 Then    'Vermont Medicaid
        strNM103 = "VT MEDICIAD"
        strNM109 = rstClrHse.Fields("fldReceiverID").Value
    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 114 Then    'Kansas Medicaid
        strNM103 = "KANSAS MEDICAL ASSISTANCE PROGRAM"
        strNM109 = rstClrHse.Fields("fldReceiverID").Value
    End If
    
    'If the receiver name is more than 35 characters then we use segment N2 to hold the
    'overflow characters
    If Len(IfNull(rst.Fields("fldPlanName").Value, "")) > 35 Then
        strN2 = "N2*" & Mid(rst.Fields("fldPlanName").Value, 36) & "~"
    Else
        strN2 = ""
    End If
    
    strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
        & "*" & strNM104 & "*" & strNM105 & "*" & strNM106 & "*" & strNM107 _
        & "*" & strNM108 & "*" & strNM109 & "~"
        
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strNM1
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strNM1 Else Print #1, strNM1;
    End If
    g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
    
    If strN2 > "" Then
'        If g_blnLineFeed Then Print #1, strN2 Else Print #1, strN2;  'Print overflow line
'        g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
    End If
        
    Exit Function
    
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description

End Function
Private Sub WriteLoop2000A(ByVal rst As ADODB.Recordset, ByVal rstClrHse As ADODB.Recordset)
'Writes heirarchy level (this is always level 1) and specialty information (mental
'health claims)

    Dim strHL, strHL01, strHL02, strHL03, strHL04 As String
    Dim strPRV, strPRV01, strPRV02, strPRV03, strPRV04 As String
    
    HL_01 = HL_01 + 1
    strHL01 = Format(HL_01, "##0") 'hierarchical ID number (see pg. 77 of X098.pdf)
    strHL02 = ""               'hierarchical parent ID
    strHL03 = "20"             'hierarchical level code(20 = information source)
    strHL04 = "1"              'hierarchical child code
    HL_Parent = HL_01
    
    strHL = "HL*" & strHL01 & "*" & strHL02 & "*" & strHL03 & "*" & strHL04 & "~"
        
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strHL
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strHL Else Print #1, strHL;
    End If
    g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
    
    'Specialty information
    strPRV01 = "BI"                 'Provider Code (PE-Performing, BI-Billing, PT-Payto, RF-Referring)
    strPRV02 = "ZZ"                 'Reference Identification Qualifier
    If g_bln5010 Then strPRV02 = "PXC"    '5010 uses PXC for the Taxonomy Code
    strPRV03 = ""
    If (IsNull(rst.Fields("fldGroupNPI").Value)) Or (rst.Fields("fldGroupNPI").Value = "") Or (rst.Fields("fldGroupName").Value = "INDIVIDUAL") Or (rst.Fields("fldGroupName").Value = "") Then
        'Specialty information
        If rst.Fields("fldUseSuperYN").Value = "Y" And _
           (IfNull(rst.Fields("fldSuperNPI").Value, "") = IfNull(rst.Fields("fldProviderNPI").Value, "") Or _
            IfNull(rst.Fields("fldProviderNPI").Value, "") = "") And _
           (IfNull(rst.Fields("fldSuperNPI").Value, "") > "") Then
            strPRV03 = Trim(StripDelimiters(IfNull(rst.Fields("fldSuperTaxonomyCode").Value, "101Y00000X")))         'ZZ relies on Provider Taxonomy Code published by BC/BS Association
        Else
            strPRV03 = Trim(StripDelimiters(IfNull(rst.Fields("fldTaxonomyCode").Value, "101Y00000X")))         'ZZ relies on Provider Taxonomy Code published by BC/BS Association
        End If
    Else
        strPRV03 = Trim(StripDelimiters(IfNull(rst.Fields("fldGrpTaxonomyCode").Value, "193400000X")))
            'strPRV03 = "193400000X"
    End If
    If strPRV03 > "" Then
        strPRV = "PRV*" & strPRV01 & "*" & strPRV02 & "*" & strPRV03 & "~"
        If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
            g_PrtString = g_PrtString + strPRV
            If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
            End If
        Else
            If g_blnLineFeed Then Print #1, strPRV Else Print #1, strPRV;
        End If
        g_lngNumSeg = g_lngNumSeg + 1
    End If

End Sub
Private Sub WriteLoop2000C(ByVal rst As ADODB.Recordset, ByVal rstClrHse As ADODB.Recordset)
'Patient information - written only if patient is not same person as insured.

    Dim strHL, strHL01, strHL02, strHL03, strHL04 As String
    Dim strPat As String
    
    On Error GoTo ErrTrap:
    
    HL_01 = HL_01 + 1
    strHL01 = Format(HL_01, "##0")      'hierarchical ID number - always 3 (see p. 152 of X098.pdf)
    strHL02 = Format(HL_Parent, "##0")  'hierarchical parent ID
    strHL03 = "23"                      'hierarchical level code (23 = dependant)
    strHL04 = "0"                       'hierarchical child code
    HL_Parent = HL_01
    
    strHL = "HL*" & strHL01 & "*" & strHL02 & "*" & strHL03 & "*" & strHL04 & "~"
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strHL
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strHL Else Print #1, strHL;
    End If
    g_lngNumSeg = g_lngNumSeg + 1

    strPat = "PAT*"
    strPat = strPat & GetRelationCode(IfNull(rst.Fields("fldPatientRelation").Value, "")) & "~"
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strPat
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strPat Else Print #1, strPat;
    End If
    g_lngNumSeg = g_lngNumSeg + 1
    
    Exit Sub
    
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description
    
End Sub
Private Function WriteLoop2010CA(ByVal rst As ADODB.Recordset, ByVal rstClrHse As ADODB.Recordset)
'Patient name, demographics - written only if patient is not same person as insured.

    Dim strNM101, strNM102, strNM103, strNM104, strNM105 As String
    Dim strNM106, strNM107, strNM108, strNM109, strNM110, strNM111 As String
    Dim strRef, strRef01, strRef02 As String
    Dim strNM1 As String
    Dim strN3, strN4 As String
    Dim strN401, strN402, strN403, strN404 As String
    Dim strDMG As String
    Dim dtDOB As Date
    
    On Error GoTo ErrTrap:
    
    'Loop 2000CA
    strNM101 = "QC"         'entity identifier code (QC = submitter)
    strNM102 = "1"          'Qualifier - 2:Non-person entity
    strNM103 = Trim(StripDelimiters(Mid(Replace(IfNull(rst.Fields("fldPatientLastName").Value, ""), "'", ""), 1, 35)))    'Last name
    strNM104 = Trim(StripDelimiters(Mid(Replace(IfNull(rst.Fields("fldPatientFirstName").Value, ""), "'", ""), 1, 35)))   'first name
    strNM105 = IfNull(rst.Fields("fldPatientMI").Value, "")   'middle name
    strNM106 = ""        'not used (name prefix)
    strNM107 = ""        'not used (name suffix)
    strNM108 = "MI"      'identification code qualifier
    If g_bln5010 Then strNM108 = ""    '5010 does not use this field
    'Subscribers Identification number as assigned by the payer.
    strNM109 = Trim(Replace(StripDelimiters(IfNull(rst.Fields("fldCardNum").Value, "00")), " ", "", 1))
    If g_bln5010 Then strNM109 = ""    '5010 does not use this field
    
    strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
         & "*" & strNM104 & "*" & strNM105 & "*" & strNM106 _
         & "*" & strNM107 & "*" & strNM108 & "*" & strNM109 _
         & "~"
         
    If g_bln5010 Then
        If strNM105 > "" Then
            strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
                & "*" & strNM104 & "*" & strNM105 & "~"
        Else
            strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
                & "*" & strNM104 & "~"
        End If
    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strNM1
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strNM1 Else Print #1, strNM1;
    End If
    g_lngNumSeg = g_lngNumSeg + 1

    If Not IsNull(rst.Fields("fldPatientStreetNum").Value) Then
        'Patient address
        strN3 = "N3*" & Trim(StripDelimiters(IfNull(rst.Fields("fldPatientStreetNum").Value, ""))) & "~"
        If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
            g_PrtString = g_PrtString + strN3
            If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
            End If
        Else
            If g_blnLineFeed Then Print #1, strN3 Else Print #1, strN3;
        End If
        g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters

        strN401 = Trim(Replace(IfNull(StripDelimiters(rst.Fields("fldPatientCity").Value), ""), "'", ""))   'city
        strN402 = Trim(Replace(IfNull(rst.Fields("fldPatientState").Value, ""), "'", ""))  'State code (2 digits)
        strN403 = Trim(NumbersOnly(Replace(IfNull(rst.Fields("fldPatientZip").Value, ""), "'", "")))    'postal code
        If strN402 = "OT" Or strN402 = "ON" Then strN404 = "CA"
    
        strN4 = "N4*" & strN401 & "*" & strN402 & "*" & strN403 & "~"
        If strN404 > "" Then strN4 = "N4*" & strN401 & "*" & strN402 & "*" & strN403 & "*" & strN404 & "~"
        If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
            g_PrtString = g_PrtString + strN4
            If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
            End If
        Else
            If g_blnLineFeed Then Print #1, strN4 Else Print #1, strN4;
        End If
        g_lngNumSeg = g_lngNumSeg + 1
    End If
    
    If IsNull(rst.Fields("fldPatientDOB").Value) Then
        dtDOB = 0
    Else
        dtDOB = rst.Fields("fldPatientDOB").Value
    End If
    
    'If the DOB is not known, then this line must be ignored otherwise errors occur
    If IsDate(dtDOB) And dtDOB > 0 Then
        strDMG = "DMG*D8*" & Year(dtDOB) & Format(Month(dtDOB), "00") & Format(Day(dtDOB), "00") & "*"
        strDMG = strDMG & IfNull(rst.Fields("fldPatientSex").Value, "U") & "~"
        If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
            g_PrtString = g_PrtString + strDMG
            If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
            End If
        Else
            If g_blnLineFeed Then Print #1, strDMG Else Print #1, strDMG;
        End If
        g_lngNumSeg = g_lngNumSeg + 1
    End If

    If rst.Fields("fldInsType").Value <> "MP" And _
        rst.Fields("fldInsType").Value <> "MB" And _
        rst.Fields("fldInsType").Value <> "MC" And _
        rst.Fields("fldInsType").Value <> "BL" Then
        If IfNull(rst.Fields("fldInsuredSSN").Value, "") > "" And _
            IfNull(rst.Fields("fldInsuredSSN").Value, "") <> strNM109 And _
            IfNull(rst.Fields("fldInsuredSSN").Value, "") <> "000000000" And _
            IfNull(rst.Fields("fldInsuredSSN").Value, "") <> "111111111" And _
            IfNull(rst.Fields("fldInsuredSSN").Value, "") <> "222222222" And _
            IfNull(rst.Fields("fldInsuredSSN").Value, "") <> "999999999" Then
            strRef = "REF*SY*" & StripDelimiters(rst.Fields("fldInsuredSSN").Value) & "~"
            If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
                rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
                g_PrtString = g_PrtString + strRef
                If Len(g_PrtString) >= 80 Then
                    Print #1, Left(g_PrtString, 80)
                    g_PrtString = Mid(g_PrtString, 81)
                End If
            Else
                If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
            End If
            g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
        Else
            If rst.Fields("fldPatientID").Value = rst.Fields("fldRPID").Value And _
                IfNull(rst.Fields("fldPatientSSN").Value, "") > "" And _
                IfNull(rst.Fields("fldPatientSSN").Value, "") <> strNM109 And _
                IfNull(rst.Fields("fldPatientSSN").Value, "") <> "000000000" And _
                IfNull(rst.Fields("fldInsuredSSN").Value, "") <> "111111111" And _
                IfNull(rst.Fields("fldInsuredSSN").Value, "") <> "222222222" And _
                IfNull(rst.Fields("fldPatientSSN").Value, "") <> "999999999" Then
                    strRef = "REF*SY*" & StripDelimiters(rst.Fields("fldPatientSSN").Value) & "~"
                    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
                        rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
                        g_PrtString = g_PrtString + strRef
                        If Len(g_PrtString) >= 80 Then
                            Print #1, Left(g_PrtString, 80)
                            g_PrtString = Mid(g_PrtString, 81)
                        End If
                    Else
                        If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
                    End If
                    g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
            End If
        End If
    End If
    
    Exit Function
    
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description

End Function
Private Function WriteLoop2010AA(ByVal rst As ADODB.Recordset, ByVal rstClrHse As ADODB.Recordset, ByVal strPayerCode As String) As Boolean
'Writes Billing Provider information
    
    Dim strNM101, strNM102, strNM103, strNM104, strNM105 As String
    Dim strNM106, strNM107, strNM108, strNM109, strNM110, strNM111 As String
    Dim strNM1, strN2, strN3, strN4 As String
    Dim strN401, strN402, strN403, strN404 As String
    Dim strRef, strRef01, strRef02 As String
    Dim strPer, strPer01, strPer02, strPer03, strPer04, strPer05, strPer06 As String
    
    On Error GoTo ErrTrap:

    strNM101 = "85"        'entity code identifier (85 - billing provider)
    strNM102 = "1"         'entity type qualifier (1=person; 2=non-person)

    If (IsNull(rst.Fields("fldGrpPracticeNum").Value) And IsNull(rst.Fields("fldGroupNPI").Value)) Or _
       (Trim(rst.Fields("fldGroupName").Value) = "INDIVIDUAL") Or _
       (Trim(rst.Fields("fldGroupName").Value) = "INDIVIDUAL PROVIDER") Or _
       (Trim(rst.Fields("fldGroupName").Value) = "") Then
        strNM103 = Trim(StripDelimiters(Mid(Replace(IfNull(rst.Fields("fldProviderLastName").Value, ""), "'", ""), 1, 35)))          'Last name
        strNM104 = Trim(StripDelimiters(Mid(Replace(IfNull(rst.Fields("fldProviderFirstName").Value, ""), "'", ""), 1, 35)))   'first name
        strNM105 = IfNull(rst.Fields("fldProviderMI").Value, "")   'middle name
        If rst.Fields("fldUseSuperYN").Value = "Y" Then
            If (IfNull(rst.Fields("fldSuperNPI").Value, "") = IfNull(rst.Fields("fldProviderNPI").Value, "") Or _
                IfNull(rst.Fields("fldProviderNPI").Value, "") = "") And _
               (IfNull(rst.Fields("fldSuperNPI").Value, "") > "") Then
                strNM103 = Trim(StripDelimiters(IfNull(rst.Fields("fldSuperLastName").Value, Mid(Replace(rst.Fields("fldProviderLastName").Value, "'", ""), 1, 35))))     'Last name
                strNM104 = Trim(StripDelimiters(IfNull(rst.Fields("fldSuperFirstName").Value, Mid(Replace(rst.Fields("fldProviderFirstName").Value, "'", ""), 1, 35))))   'first name
                strNM105 = IfNull(rst.Fields("fldSuperMI").Value, "")   'middle name
            End If
        End If
    Else
        strNM102 = "2"         'entity type qualifier (1=person; 2=non-person)
        strNM103 = Trim(Mid(Replace(IfNull(rst.Fields("fldGroupName").Value, ""), "'", ""), 1, 35))          'Last name
        strNM104 = ""           'first name
        strNM105 = ""       'middle name
    End If
    strNM106 = ""          'name prefix (not used)
    strNM107 = ""          'name suffix (not used)
    strNM108 = "XX"        'NPI
    strNM109 = IfNull(rst.Fields("fldProviderNPI").Value, "")
            
    If IfNull(rst.Fields("fldGroupNPI").Value, "") = "" And _
        IIf(rst.Fields("fldUseSuperYN").Value = "Y", IfNull(rst.Fields("fldSuperNPI").Value, ""), IfNull(rst.Fields("fldProviderNPI").Value, "")) = "" Then
        'identification code qualifier
        If rst.Fields("fldTinType").Value = 1 Then
            strNM108 = "34"        'individual SSN SY
        Else
            strNM108 = "24"        '24 = employer'S ID EI
        End If
        strNM109 = Left(NumbersOnly(rst.Fields("fldTIN").Value), 9)         'billing provider identifier code
    Else
        If IfNull(rst.Fields("fldGroupNPI").Value, "") > "" Then
            strNM109 = rst.Fields("fldGroupNPI").Value
        ElseIf rst.Fields("fldUseSuperYN").Value = "Y" Then
            If (IfNull(rst.Fields("fldSuperNPI").Value, "") = IfNull(rst.Fields("fldProviderNPI").Value, "") Or _
                IfNull(rst.Fields("fldProviderNPI").Value, "") = "") And _
               (IfNull(rst.Fields("fldSuperNPI").Value, "") > "") Then
                strNM109 = IfNull(rst.Fields("fldSuperNPI").Value, IfNull(rst.Fields("fldProviderNPI").Value, ""))
            End If
        End If
    End If
    
    'Institutional Claims
    If rst.Fields("fldClaimType").Value = "I" Then
        strNM102 = "2"         'entity type qualifier (1=person; 2=non-person)
        strNM103 = Trim(Mid(Replace(IfNull(rst.Fields("fldFacilityLine1").Value, Trim(rst.Fields("fldGroupName").Value)), "'", ""), 1, 35))          'Billing Clinic Name
        strNM104 = ""           'first name
        strNM105 = ""       'middle name
        strNM106 = ""          'name prefix (not used)
        strNM107 = ""          'name suffix (not used)
        strNM108 = "XX"        'NPI
        strNM109 = IfNull(rst.Fields("fldFacilityNPI").Value, IfNull(rst.Fields("fldGroupNPI").Value, ""))
    End If
        
    strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 & "*" _
         & strNM104 & "*" & strNM105 & "*" & strNM106 & "*" _
         & strNM107 & "*" & strNM108 & "*" & strNM109 & "~"
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strNM1
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strNM1 Else Print #1, strNM1;
    End If
    g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
    
    'Provider Billing Address
'    If g_bln5010 Then    '5010 The pay-to-Address cannot be a PO Box.  -->> Changed 08/12/2014
        If InStr(1, StripDelimiters(IfNull(rst.Fields("fldBillingCompanyLine2").Value, rst.Fields("fldProviderAddress").Value)), "PO Box", vbTextCompare) > 0 Then
            strN3 = "N3*" & StripDelimiters(IfNull(rst.Fields("fldFacilityLine2").Value, rst.Fields("fldBillingCompanyLine2").Value)) & "~"
        Else
            strN3 = "N3*" & StripDelimiters(IfNull(rst.Fields("fldBillingCompanyLine2").Value, rst.Fields("fldProviderAddress").Value)) & "~"
        End If
'    Else
'        strN3 = "N3*" & StripDelimiters(IfNull(rst.Fields("fldBillingCompanyLine2").Value, rst.Fields("fldProviderAddress").Value)) & "~"
'    End If
    
    'Institutional ClaimsBilling Address
    If rst.Fields("fldClaimType").Value = "I" Then
        strN3 = "N3*" & StripDelimiters(IfNull(rst.Fields("fldFacilityLine2").Value, rst.Fields("fldBillingCompanyLine2").Value)) & "~"
    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strN3
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strN3 Else Print #1, strN3;
    End If
    g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters

    If IsNull(rst.Fields("fldBillingCompanyLine2").Value) Then
        strN401 = Replace(IfNull(rst.Fields("fldProviderCity").Value, rst.Fields("fldBillingCompanyCity").Value), "'", "")      'city
        strN402 = Replace(IfNull(rst.Fields("fldProviderState").Value, rst.Fields("fldBillingCompanyState").Value), "'", "")    'State code (2 digits)
        strN403 = Replace(IfNull(rst.Fields("fldProviderZip").Value, rst.Fields("fldBillingCompanyZip").Value), "-", "")        'postal code
        If strN402 = "OT" Or strN402 = "ON" Then strN404 = "CA"
    Else
        strN401 = Replace(IfNull(rst.Fields("fldBillingCompanyCity").Value, rst.Fields("fldProviderCity").Value), "'", "")      'city
        strN402 = Replace(IfNull(rst.Fields("fldBillingCompanyState").Value, rst.Fields("fldProviderState").Value), "'", "")    'State code (2 digits)
        strN403 = Replace(IfNull(rst.Fields("fldBillingCompanyZip").Value, rst.Fields("fldProviderZip").Value), "-", "")        'postal code
        If strN402 = "OT" Or strN402 = "ON" Then strN404 = "CA"
    End If
    
'    If g_bln5010 Then    '5010 The pay-to-Address cannot be a PO Box. -->> Changed 08/12/2014
        If InStr(1, StripDelimiters(IfNull(rst.Fields("fldBillingCompanyLine2").Value, rst.Fields("fldProviderAddress").Value)), "PO Box", vbTextCompare) > 0 Then
            strN401 = Replace(IfNull(rst.Fields("fldFacilityCity").Value, rst.Fields("fldBillingCompanyCity").Value), "'", "")   'city
            strN402 = Replace(IfNull(rst.Fields("fldFacilityState").Value, rst.Fields("fldBillingCompanyState").Value), "'", "")  'State code (2 digits)
            strN403 = Replace(IfNull(rst.Fields("fldFacilityZip").Value, rst.Fields("fldBillingCompanyZip").Value), "-", "")      'postal code
            If strN402 = "OT" Or strN402 = "ON" Then strN404 = "CA"
        End If
'    End If
    
    'Institutional ClaimsBilling Address
    If rst.Fields("fldClaimType").Value = "I" Then
        strN401 = Replace(IfNull(rst.Fields("fldFacilityCity").Value, rst.Fields("fldBillingCompanyCity").Value), "'", "")      'city
        strN402 = Replace(IfNull(rst.Fields("fldFacilityState").Value, rst.Fields("fldBillingCompanyState").Value), "'", "")    'State code (2 digits)
        strN403 = Replace(IfNull(rst.Fields("fldFacilityZip").Value, rst.Fields("fldBillingCompanyZip").Value), "-", "")        'postal code
        If strN402 = "OT" Or strN402 = "ON" Then strN404 = "CA"
    End If
    
    strN4 = "N4*" & strN401 & "*" & strN402 & "*" & strN403 & "~"
    If strN404 > "" Then strN4 = "N4*" & strN401 & "*" & strN402 & "*" & strN403 & "*" & strN404 & "~"
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strN4
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strN4 Else Print #1, strN4;
    End If
    g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
    
    
        If rst.Fields("fldTinType").Value = 1 And IfNull(rst.Fields("fldGroupNPI").Value, "") = "" Then
            strRef01 = "SY"        'individual SSN SY
            strRef02 = Left(NumbersOnly(rst.Fields("fldTIN").Value), 9)         'billing provider identifier code
        Else
            strRef01 = "EI"        'EI = employer'S ID EI
            strRef02 = Left(NumbersOnly(rst.Fields("fldTIN").Value), 9)         'billing provider identifier code
        End If
        If strRef02 > "" And strRef02 <> strNM109 Then
            strRef = "REF*" & strRef01 & "*" & strRef02 & "~"
            If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
                rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
                g_PrtString = g_PrtString + strRef
                If Len(g_PrtString) >= 80 Then
                    Print #1, Left(g_PrtString, 80)
                    g_PrtString = Mid(g_PrtString, 81)
                End If
            Else
                If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
            End If
            g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
        End If
       
    'Add the billing provider secondary identification
    If rst.Fields("fldInsType").Value = "MP" Then
            strRef01 = "1C"
            strRef02 = IfNull(rst.Fields("fldGrpPracticeNum").Value, "")
    ElseIf rst.Fields("fldInsType").Value = "MB" Then
            strRef01 = "1C"
            strRef02 = IfNull(rst.Fields("fldGrpPracticeNum").Value, "")
    ElseIf rst.Fields("fldInsType").Value = "MC" Then
            If rstClrHse.Fields("fldClearingHouseID").Value = 51 Then
                strRef01 = "G5"
            Else
                strRef01 = "1D"
            End If
            strRef02 = IfNull(rst.Fields("fldGrpPracticeNum").Value, "")
    ElseIf rst.Fields("fldInsType").Value = "BL" Then  'BC/BS
            strRef01 = "1B"
            strRef02 = IfNull(rst.Fields("fldGrpPracticeNum").Value, "")
        'For REF01: 1A-Blue Cross; 1B-Blue Shield.  See page 98 of EDI specification
        'We will record both numbers if payer name is not specific to Blue Cross or Blue Shield
    ElseIf rst.Fields("fldInsType").Value = "1A" Then
            strRef01 = "1A"
            strRef02 = IfNull(rst.Fields("fldGrpPracticeNum").Value, "")
    ElseIf rst.Fields("fldInsType").Value = "BQ" Then
            strRef01 = "BQ"
            strRef02 = IfNull(rst.Fields("fldGrpPracticeNum").Value, "")
    ElseIf rst.Fields("fldInsType").Value = "CH" Then
            strRef01 = "1H"
            strRef02 = IfNull(rst.Fields("fldGrpPracticeNum").Value, "") 'Federal Tax ID
    Else
        'Commercial Claims
            strRef01 = "G2"
            strRef02 = IfNull(rst.Fields("fldGrpPracticeNum").Value, "")
    End If
 
'    If g_bln5010 Then    '5010 Beacon Health requires the Taxonomy Code to be sent as a secondary id
'        If rst.Fields("fldInsuranceID").Value = 1317 Then
'            If (IsNull(rst.Fields("fldGrpPracticeNum").Value) And IsNull(rst.Fields("fldGroupNPI").Value)) Then
'                If rst.Fields("fldUseSuperYN").Value = "Y" Then
'                    strRef02 = Trim(StripDelimiters(IfNull(rst.Fields("fldSuperTaxonomyCode").Value, "101Y00000X")))
'                Else
'                    strRef02 = Trim(StripDelimiters(IfNull(rst.Fields("fldTaxonomyCode").Value, "101Y00000X")))
'                End If
'            Else
'                strRef02 = Trim(StripDelimiters(IfNull(rst.Fields("fldGrpTaxonomyCode").Value, "193400000X")))
'            End If
'            strRef01 = "G2"
'            strRef = "REF*" & strRef01 & "*" & strRef02 & "~"
'            If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
'                rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
'                g_PrtString = g_PrtString + strRef
'                If Len(g_PrtString) >= 80 Then
'                    Print #1, Left(g_PrtString, 80)
'                    g_PrtString = Mid(g_PrtString, 81)
'                End If
'            Else
'                If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
'            End If
'            g_lngNumSeg = g_lngNumSeg + 1
'        End If
'    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 200 Then
        'Billing Provider Phone Contact Information
        strPer01 = "IC"                    'contact function code (IC = Information Contact)
        strPer02 = ""                      '5010 does not use this segment
        strPer03 = "TE"                    'communication number qualifier (TE = Telephone)
        strPer04 = Trim(Left(NumbersOnly(rst.Fields("fldProviderPhone").Value), 10))           'communication number (phone should be XXXYYYZZZZ)
        If strPer04 = "" Then strPer04 = Trim(Left(NumbersOnly(rst.Fields("fldBillingCompanyPhone").Value), 10))
        If strPer04 = "" Then strPer04 = "9175383905"
        
        strPer = "PER*" & strPer01 & "*" & strPer02 & "*" & strPer03 & "*" & strPer04 & "*" & "~"

        If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
         rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
            g_PrtString = g_PrtString + strPer
            If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
            End If
        Else
            If g_blnLineFeed Then Print #1, strPer Else Print #1, strPer;
        End If
        g_lngNumSeg = g_lngNumSeg + 1   'Increment segment counters
    End If
    
    Exit Function

ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Private Function WriteLoop2010AB(ByVal rst As ADODB.Recordset, ByVal rstClrHse As ADODB.Recordset, ByVal strPayerCode As String) As Boolean
'Writes Billing Provider information
    
    Dim strNM101, strNM102, strNM103, strNM104, strNM105 As String
    Dim strNM106, strNM107, strNM108, strNM109, strNM110, strNM111 As String
    Dim strNM1, strN2, strN3, strN4 As String
    Dim strN401, strN402, strN403, strN404 As String
    Dim strRef, strRef01, strRef02 As String
    
    On Error GoTo ErrTrap:

    strNM101 = "87"        'entity code identifier (87 - Pay To provider)
    strNM102 = "1"         'entity type qualifier (1=person; 2=non-person)

    If (IsNull(rst.Fields("fldGrpPracticeNum").Value) And IsNull(rst.Fields("fldGroupNPI").Value)) Or _
       (Trim(rst.Fields("fldGroupName").Value) = "INDIVIDUAL") Or _
       (Trim(rst.Fields("fldGroupName").Value) = "INDIVIDUAL PROVIDER") Or _
       (Trim(rst.Fields("fldGroupName").Value) = "") Then
        strNM103 = Trim(StripDelimiters(Mid(Replace(IfNull(rst.Fields("fldProviderLastName").Value, ""), "'", ""), 1, 35)))          'Last name
        strNM104 = Trim(StripDelimiters(Mid(Replace(IfNull(rst.Fields("fldProviderFirstName").Value, ""), "'", ""), 1, 35)))   'first name
        strNM105 = IfNull(rst.Fields("fldProviderMI").Value, "")   'middle name
        If rst.Fields("fldUseSuperYN").Value = "Y" Then
            If (IfNull(rst.Fields("fldSuperNPI").Value, "") = IfNull(rst.Fields("fldProviderNPI").Value, "") Or _
                IfNull(rst.Fields("fldProviderNPI").Value, "") = "") And _
               (IfNull(rst.Fields("fldSuperNPI").Value, "") > "") Then
                strNM103 = Trim(StripDelimiters(IfNull(rst.Fields("fldSuperLastName").Value, Mid(Replace(rst.Fields("fldProviderLastName").Value, "'", ""), 1, 35))))     'Last name
                strNM104 = Trim(StripDelimiters(IfNull(rst.Fields("fldSuperFirstName").Value, Mid(Replace(rst.Fields("fldProviderFirstName").Value, "'", ""), 1, 35))))   'first name
                strNM105 = IfNull(rst.Fields("fldSuperMI").Value, "")   'middle name
            End If
        End If
    Else
        strNM102 = "2"         'entity type qualifier (1=person; 2=non-person)
        strNM103 = Trim(Mid(Replace(IfNull(rst.Fields("fldGroupName").Value, ""), "'", ""), 1, 35))          'Last name
        strNM104 = ""           'first name
        strNM105 = ""       'middle name
    End If
    strNM106 = ""          'name prefix (not used)
    strNM107 = ""          'name suffix (not used)
    strNM108 = "XX"        'NPI
    strNM109 = IfNull(rst.Fields("fldProviderNPI").Value, "")
            
    If IfNull(rst.Fields("fldGroupNPI").Value, "") = "" And _
        IIf(rst.Fields("fldUseSuperYN").Value = "Y", IfNull(rst.Fields("fldSuperNPI").Value, ""), IfNull(rst.Fields("fldProviderNPI").Value, "")) = "" Then
        'identification code qualifier
        If rst.Fields("fldTinType").Value = 1 Then
            strNM108 = "34"        'individual SSN SY
        Else
            strNM108 = "24"        '24 = employer'S ID EI
        End If
        strNM109 = Left(NumbersOnly(rst.Fields("fldTIN").Value), 9)        'billing provider identifier code
    Else
        If IfNull(rst.Fields("fldGroupNPI").Value, "") > "" Then
            strNM109 = rst.Fields("fldGroupNPI").Value
        ElseIf rst.Fields("fldUseSuperYN").Value = "Y" Then
            If (IfNull(rst.Fields("fldSuperNPI").Value, "") = IfNull(rst.Fields("fldProviderNPI").Value, "") Or _
                IfNull(rst.Fields("fldProviderNPI").Value, "") = "") And _
               (IfNull(rst.Fields("fldSuperNPI").Value, "") > "") Then
                strNM109 = IfNull(rst.Fields("fldSuperNPI").Value, IfNull(rst.Fields("fldProviderNPI").Value, ""))
            End If
        End If
    End If
    
    'Institutional Claims
    If rst.Fields("fldClaimType").Value = "I" Then
        strNM102 = "2"         'entity type qualifier (1=person; 2=non-person)
        strNM103 = Trim(Mid(Replace(IfNull(rst.Fields("fldFacilityLine1").Value, Trim(rst.Fields("fldGroupName").Value)), "'", ""), 1, 35))          'Billing Clinic Name
        strNM104 = ""           'first name
        strNM105 = ""       'middle name
        strNM106 = ""          'name prefix (not used)
        strNM107 = ""          'name suffix (not used)
        strNM108 = "XX"        'NPI
        strNM109 = IfNull(rst.Fields("fldFacilityNPI").Value, IfNull(rst.Fields("fldGroupNPI").Value, ""))
    End If
   
    If Not g_bln5010 Then    '5010 The pay-to-name is not used.
        strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 & "*" _
         & strNM104 & "*" & strNM105 & "*" & strNM106 & "*" _
         & strNM107 & "*" & strNM108 & "*" & strNM109 & "~"
    Else
        strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "~"
    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strNM1
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strNM1 Else Print #1, strNM1;
    End If
    g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
    
    strN3 = "N3*" & StripDelimiters(IfNull(rst.Fields("fldBillingCompanyLine2").Value, rst.Fields("fldProviderAddress").Value)) & "~"
    
    'Institutional ClaimsBilling Address
    If rst.Fields("fldClaimType").Value = "I" Then
        strN3 = "N3*" & StripDelimiters(IfNull(rst.Fields("fldFacilityLine2").Value, rst.Fields("fldBillingCompanyLine2").Value)) & "~"
    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strN3
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strN3 Else Print #1, strN3;
    End If
    g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters

    If IsNull(rst.Fields("fldBillingCompanyLine2").Value) Then
        strN401 = Replace(IfNull(rst.Fields("fldProviderCity").Value, rst.Fields("fldBillingCompanyCity").Value), "'", "")      'city
        strN402 = Replace(IfNull(rst.Fields("fldProviderState").Value, rst.Fields("fldBillingCompanyState").Value), "'", "")    'State code (2 digits)
        strN403 = Replace(IfNull(rst.Fields("fldProviderZip").Value, rst.Fields("fldBillingCompanyZip").Value), "-", "")        'postal code
        If strN402 = "OT" Or strN402 = "ON" Then strN404 = "CA"
    Else
        strN401 = Replace(IfNull(rst.Fields("fldBillingCompanyCity").Value, rst.Fields("fldProviderCity").Value), "'", "")      'city
        strN402 = Replace(IfNull(rst.Fields("fldBillingCompanyState").Value, rst.Fields("fldProviderState").Value), "'", "")    'State code (2 digits)
        strN403 = Replace(IfNull(rst.Fields("fldBillingCompanyZip").Value, rst.Fields("fldProviderZip").Value), "-", "")        'postal code
        If strN402 = "OT" Or strN402 = "ON" Then strN404 = "CA"
    End If
    
    'Institutional ClaimsBilling Address
    If rst.Fields("fldClaimType").Value = "I" Then
        strN401 = Replace(IfNull(rst.Fields("fldFacilityCity").Value, rst.Fields("fldBillingCompanyCity").Value), "'", "")      'city
        strN402 = Replace(IfNull(rst.Fields("fldFacilityState").Value, rst.Fields("fldBillingCompanyState").Value), "'", "")    'State code (2 digits)
        strN403 = Replace(IfNull(rst.Fields("fldFacilityZip").Value, rst.Fields("fldBillingCompanyZip").Value), "-", "")        'postal code
        If strN402 = "OT" Or strN402 = "ON" Then strN404 = "CA"
    End If
    
    strN4 = "N4*" & strN401 & "*" & strN402 & "*" & strN403 & "~"
    If strN404 > "" Then strN4 = "N4*" & strN401 & "*" & strN402 & "*" & strN403 & "*" & strN404 & "~"
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strN4
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strN4 Else Print #1, strN4;
    End If
    g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
    
    Exit Function

ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Private Sub WriteLoop2000B(ByVal rst As ADODB.Recordset, ByVal rstClrHse As ADODB.Recordset, ByVal strPayerCode As String)
'Writes heirarchy level (this level is always '2').
    
    Dim strHL, strHL01, strHL02, strHL03, strHL04 As String
    Dim strSBR, strSBR01, strSBR02, strSBR03, strSBR04 As String
    Dim strSBR05, strSBR06, strSBR07, strSBR08, strSBR09 As String
    
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

    'SBR (subscriber information)
    strSBR01 = "P"             'payer resp. sequence number code (see p. 110)
    If rst.Fields("fldOrder").Value = 2 Then strSBR01 = "S"
    If rst.Fields("fldOrder").Value = 3 Then strSBR01 = "T"
    
    If rst.Fields("fldPatientID").Value = rst.Fields("fldRPID").Value Or _
        rst.Fields("fldInsType").Value = "MP" Or _
        rst.Fields("fldInsType").Value = "MB" Or _
        rst.Fields("fldInsType").Value = "MC" Then
        strSBR02 = "18"              'individual relationship code (18 = self)
    Else
        If (strPayerCode = "F22099" Or strPayerCode = "F23281") Then
            strSBR02 = GetRelationCode(IfNull(rst.Fields("fldPatientRelation").Value, ""))
        Else
            strSBR02 = ""
        End If
    End If
    
    If (strPayerCode = "01260" Or strPayerCode = "F01260") Then   'Magellan DOES NOT WANT THE GROUP NUMBER
        strSBR03 = ""
    ElseIf (strPayerCode = "47198" Or strPayerCode = "F47198") Then
        strSBR03 = StripDelimiters(IfNull(rst.Fields("fldGroupNum").Value, "999999")) 'Blue Cross of Ca wants 999999 when unknown
    Else
        strSBR03 = StripDelimiters(IfNull(Trim(rst.Fields("fldGroupNum").Value), "")) 'Subscriber group or policy number
    End If
    If rst.Fields("fldPlanID").Value = 5568 And rst.Fields("fldOrder").Value = 3 Then
        strSBR04 = "0FILL"
    Else
        strSBR04 = UCase(Trim(StripDelimiters(Mid(IfNull(rst.Fields("fldPlanName").Value, ""), 1, 35))))              'insured group name
    End If
    
    If g_bln5010 And strSBR03 = "NONE" Then strSBR03 = ""   '5010 when SBR03 is used then SBR04 cannot be used.
    If g_bln5010 And (strSBR03 > "" Or rst.Fields("fldInsuranceID").Value = 24 Or rst.Fields("fldInsuranceID").Value = 452) Then strSBR04 = ""
    
    'Institutional Claims
    If rst.Fields("fldClaimType").Value = "I" Then
        If strSBR03 > "" Then strSBR04 = ""
    End If
    
    'insurance type code applies only to Medicare where Medicare is secondary payer
'12 = Working Aged
'13 = End Stage Renal Disease
'14 = Automobile/No-Fault
'15 = Workers' Compensation
'16 = Federal
'41 = Black Lung
'42 = Veterans
'43 = Disability
'47 = Medicare is Secondary, other insurance is primary
    If (rst.Fields("fldInsuranceID").Value = 24 Or rst.Fields("fldInsuranceID").Value = 452) And rst.Fields("fldOrder").Value > 1 Then      '
        strSBR05 = "47"        'Medicare is Secondary, other insurance is primary
        If Trim(rst.Fields("fldMSPcode").Value) > "" Then strSBR05 = rst.Fields("fldMSPcode").Value
    Else
        strSBR05 = ""
    End If
    strSBR06 = ""              'not used
    strSBR07 = ""              'not used
    strSBR08 = ""              'not used
    
    strSBR09 = GetClaimFilingCode(rst.Fields("fldInsType").Value)
    
    If rst.Fields("fldPlanID").Value = 934 Then strSBR09 = "HM"   ' BcBs of MI Blue Care Network
    If rst.Fields("fldPlanID").Value = 4898 Then strSBR09 = "FI"   ' BcBs of MI Federal Claims
    If rst.Fields("fldPlanID").Value = 5889 Then strSBR09 = "ZZ"   ' State of MS employees 5889
    If rst.Fields("fldPlanID").Value = 4896 Then strSBR09 = "HM"   ' Blue Cross Complete Claims
    If rst.Fields("fldPlanID").Value = 6079 Then strSBR09 = "HM"   ' Independent Health requires HM
    If rst.Fields("fldPlanID").Value = 1408 Then strSBR09 = "MB"   ' Independent Health requires HM
    
    strSBR = "SBR*" & strSBR01 & "*" & strSBR02 & "*" & strSBR03 _
         & "*" & strSBR04 & "*" & strSBR05 & "*" & strSBR06 _
         & "*" & strSBR07 & "*" & strSBR08 & "*" & strSBR09 & "~"
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strHL
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strHL Else Print #1, strHL;
    End If
    g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strSBR
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strSBR Else Print #1, strSBR;
    End If
    g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters

End Sub
Private Function WriteLoop2010BA(ByVal rst As ADODB.Recordset, ByVal rstClrHse As ADODB.Recordset, ByVal strPayerCode As String) As Boolean
'Writes Subscriber information
    
    Dim strNM101, strNM102, strNM103, strNM104, strNM105 As String
    Dim strNM106, strNM107, strNM108, strNM109, strNM110, strNM111 As String
    Dim strNM1, strN2, strN3, strN4 As String
    Dim strN401, strN402, strN403, strN404 As String
    Dim strDMG, strRef As String
    Dim dtDOB As Date
    
    On Error GoTo ErrTrap:
    
    strNM101 = "IL"                    'entity identifier code (IL = insured)
    strNM102 = "1"                     'entity type qualifier (1=person)
    If rst.Fields("fldInsType").Value = "MP" Or _
        rst.Fields("fldInsType").Value = "MB" Or _
        rst.Fields("fldInsType").Value = "MC" Then
        strNM103 = Trim(StripDelimiters(Mid(Replace(IfNull(rst.Fields("fldPatientLastName").Value, ""), "'", ""), 1, 35)))    'Last name
        strNM104 = Trim(StripDelimiters(Mid(Replace(IfNull(rst.Fields("fldPatientFirstName").Value, ""), "'", ""), 1, 35)))   'first name
        strNM105 = Trim(IfNull(rst.Fields("fldPatientMI").Value, ""))   'middle name
    Else
        strNM103 = Trim(StripDelimiters(IfNull(rst.Fields("fldInsuredLastName").Value, Mid(Replace(rst.Fields("fldPatientLastName").Value, "'", ""), 1, 35))))     'Last name
        strNM104 = Trim(StripDelimiters(IfNull(rst.Fields("fldInsuredFirstName").Value, Mid(Replace(rst.Fields("fldPatientFirstName").Value, "'", ""), 1, 35))))   'first name
        strNM105 = Trim(IfNull(rst.Fields("fldInsuredMI").Value, IfNull(rst.Fields("fldPatientMI").Value, "")))  'middle name
    End If
    strNM106 = ""                      'name prefix (not used)
    strNM107 = ""                      'name suffix
    strNM108 = "MI"                    'Identification code qualifier (MI=Member Identification Number)
    strNM109 = Trim(StripDelimiters(IfNull(rst.Fields("fldCardNum").Value, "00"))) 'identification code
    
    strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
         & "*" & strNM104 & "*" & strNM105 & "*" & strNM106 _
         & "*" & strNM107 & "*" & strNM108 & "*" & strNM109 _
         & "~"
    
    'N2 (additional subscriber name information)
    'If the subscriber name is more than 35 characters then we need to use
    'Segment N2 to hold the overflow characters
    If Len(IfNull(rst.Fields("fldInsuredLastName").Value, Mid(Replace(rst.Fields("fldPatientLastName").Value, "'", ""), 1, 35))) > 35 Then
        'N2 segment (Additional receiver name information)
        'this segment is used if the length of strNM103 is more than 35
        strN2 = "N2*" & Mid(rst.Fields("fldInsuredLastName").Value, 36) & "~"
    Else
        strN2 = ""
    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strNM1
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strNM1 Else Print #1, strNM1;
    End If
    g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
    
    If strN2 <> "" Then
'        If g_blnLineFeed Then Print #1, strN2 Else Print #1, strN2; 'Print overflow line
'        g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
    End If
    
    'Subscriber address If the Subscriber is the same as the Insured then don't send the Subscribers address.  Mcr and Mcd the patient is the subsciber.
    If Not g_bln5010 Or _
      (rst.Fields("fldPatientID").Value = rst.Fields("fldRPID").Value) Or _
      (rst.Fields("fldPatientID").Value <> rst.Fields("fldRPID").Value And _
       (rst.Fields("fldInsType").Value = "MP" Or _
        rst.Fields("fldInsType").Value = "MB" Or _
        rst.Fields("fldInsType").Value = "MC")) Then         '---->> Or rst.Fields("fldInsuranceID").Value = 24
        strN3 = "N3*" & Trim(StripDelimiters(IfNull(rst.Fields("fldInsuredStreetNum").Value, ""))) & "~"
        If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
            g_PrtString = g_PrtString + strN3
            If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
            End If
        Else
            If g_blnLineFeed Then Print #1, strN3 Else Print #1, strN3;
        End If
        g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters

        strN401 = Trim(StripDelimiters(IfNull(rst.Fields("fldInsuredCity").Value, "")))   'city
        strN402 = Trim(Replace(IfNull(rst.Fields("fldInsuredState").Value, ""), "'", ""))  'State code (2 digits)
        strN403 = Trim(NumbersOnly(Replace(IfNull(rst.Fields("fldInsuredZip").Value, ""), "'", "")))    'postal code
        If strN402 = "OT" Or strN402 = "ON" Then strN404 = "CA"
    
        strN4 = "N4*" & strN401 & "*" & strN402 & "*" & strN403 & "~"
        If strN404 > "" Then strN4 = "N4*" & strN401 & "*" & strN402 & "*" & strN403 & "*" & strN404 & "~"
        If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
           rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
            g_PrtString = g_PrtString + strN4
            If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
            End If
        Else
            If g_blnLineFeed Then Print #1, strN4 Else Print #1, strN4;
        End If
        g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
    
      'Subscriber demographics
        If rst.Fields("fldInsType").Value = "MP" Or _
            rst.Fields("fldInsType").Value = "MB" Or _
            rst.Fields("fldInsType").Value = "MC" Then
            dtDOB = IfNull(rst.Fields("fldPatientDOB").Value, IfNull(rst.Fields("fldInsdDOB").Value, 0))
            If IsDate(dtDOB) And dtDOB > 0 Then
                strDMG = "DMG*D8*" & Year(dtDOB) & Format(Month(dtDOB), "00") & Format(Day(dtDOB), "00") & "*"
                strDMG = strDMG & IfNull(rst.Fields("fldPatientSex").Value, IfNull(rst.Fields("fldInsdSex").Value, "U")) & "~"
                If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
                    rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
                    g_PrtString = g_PrtString + strDMG
                    If Len(g_PrtString) >= 80 Then
                        Print #1, Left(g_PrtString, 80)
                        g_PrtString = Mid(g_PrtString, 81)
                    End If
                Else
                    If g_blnLineFeed Then Print #1, strDMG Else Print #1, strDMG;
                End If
                g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
            End If
        Else
            dtDOB = IfNull(rst.Fields("fldInsdDOB").Value, 0)
            'Subscriber DOB - If the DOB is not known then this line must be ignored otherwise validation errors occur
            If IsDate(dtDOB) And dtDOB > 0 Then
                strDMG = "DMG*D8*" & Year(dtDOB) & Format(Month(dtDOB), "00") & Format(Day(dtDOB), "00") & "*"
                strDMG = strDMG & IfNull(rst.Fields("fldInsdSex").Value, "U") & "~"
                If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
                    rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
                    g_PrtString = g_PrtString + strDMG
                    If Len(g_PrtString) >= 80 Then
                        Print #1, Left(g_PrtString, 80)
                        g_PrtString = Mid(g_PrtString, 81)
                    End If
                Else
                    If g_blnLineFeed Then Print #1, strDMG Else Print #1, strDMG;
                End If
                g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
            End If
        End If
    
    'Secondary number to identify subscriber
      If Not g_bln5010 Then
        If IfNull(rst.Fields("fldGroupNum").Value, "") > "" Then
            If g_bln5010 And rst.Fields("fldInsuranceID").Value = 24 Then
            ElseIf (strPayerCode = "01260" Or strPayerCode = "F01260") Then   'Magellan does not want the group number
            Else
                strRef = "REF*IG*" & StripDelimiters(rst.Fields("fldGroupNum").Value) & "~"
                If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
                    rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
                    g_PrtString = g_PrtString + strRef
                    If Len(g_PrtString) >= 80 Then
                        Print #1, Left(g_PrtString, 80)
                        g_PrtString = Mid(g_PrtString, 81)
                    End If
                Else
                    If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
                End If
                g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
            End If
        End If
      End If
    
      If rst.Fields("fldPatientID").Value = rst.Fields("fldRPID").Value And _
        rst.Fields("fldInsType").Value <> "MP" And _
        rst.Fields("fldInsType").Value <> "MB" And _
        rst.Fields("fldInsType").Value <> "MC" Then
        If IfNull(rst.Fields("fldPatientSSN").Value, "") > "" And _
            IfNull(rst.Fields("fldPatientSSN").Value, "") <> strNM109 And _
            IfNull(rst.Fields("fldPatientSSN").Value, "") <> "000000000" And _
            IfNull(rst.Fields("fldInsuredSSN").Value, "") <> "111111111" And _
            IfNull(rst.Fields("fldPatientSSN").Value, "") <> "222222222" And _
            IfNull(rst.Fields("fldPatientSSN").Value, "") <> "999999999" Then
            strRef = "REF*SY*" & StripDelimiters(rst.Fields("fldPatientSSN").Value) & "~"
            If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
                rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
                g_PrtString = g_PrtString + strRef
                If Len(g_PrtString) >= 80 Then
                    Print #1, Left(g_PrtString, 80)
                    g_PrtString = Mid(g_PrtString, 81)
                End If
            Else
                If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
            End If
            g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
        End If
      ElseIf rst.Fields("fldPlanID").Value = 3460 And _
        IfNull(rst.Fields("fldPatientSSN").Value, "") > "" And _
        IfNull(rst.Fields("fldPatientSSN").Value, "") <> strNM109 Then  'Medicaid of NC
        strRef = "REF*SY*" & StripDelimiters(rst.Fields("fldPatientSSN").Value) & "~"
        If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
        g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
      End If
    
    End If
    Exit Function

ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description
    
End Function
Private Function WriteLoop2010BB(ByVal rst As ADODB.Recordset, ByVal rstClrHse As ADODB.Recordset, ByVal strPayerCode As String) As Boolean
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
    strNM103 = IIf(IfNull(rst.Fields("fldInsName").Value, "") = "Other" Or IfNull(rst.Fields("fldInsName").Value, "") = "WORKERS COMP", UCase(Trim(StripDelimiters(Mid(IfNull(rst.Fields("fldPlanName").Value, ""), 1, 35)))), UCase(Trim(Mid(StripDelimiters(IfNull(rst.Fields("fldInsName").Value, "")), 1, 35))))  'insurance company name
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
    
    If rst.Fields("fldInsType").Value = "MC" And strPayerCode = "141797357" Then    'Computer Sciences of NY
        strNM103 = "NYSDOH"
    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 92 Then    'Tricare South
        strNM103 = "HAWAII MEDICAL SERVICE ASSOCIATION"
    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 114 Then    'Kansas Medicaid
        strNM103 = "KANSAS MEDICAL ASSISTANCE PROGRAM"
    End If
    
    strNM109 = strPayerCode
        
    strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
         & "*" & strNM104 & "*" & strNM105 & "*" & strNM106 _
         & "*" & strNM107 & "*" & strNM108 & "*" & strNM109 _
         & "~"
    
    'If the payer name is more than 35 characters then we use
    'N2 to hold the overflow characters
    If Len(StripDelimiters(rst.Fields("fldInsName").Value)) > 35 Then
        strN2 = "N2*" & Mid(StripDelimiters(rst.Fields("fldInsName").Value), 36) & "~"
    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strNM1
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strNM1 Else Print #1, strNM1;
    End If
    g_lngNumSeg = g_lngNumSeg + 1
    
    If strN2 <> "" Then
'        If g_blnLineFeed Then Print #1, strN2 Else Print #1, strN2;
'        g_lngNumSeg = g_lngNumSeg + 1
    End If
    
    'Payer address
    strN301 = StripDelimiters(rst.Fields("fldInsAddress1").Value)
    
    If (strPayerCode = "01260" Or strPayerCode = "F01260") Then   'Magellan requires PO Box number only
        strN301 = Replace(strN301, "P.O. Box ", "", 1)
        strN301 = Replace(strN301, "PO Box ", "", 1)
        strN301 = Replace(strN301, "P.O. Box", "", 1)
        strN301 = Replace(strN301, "PO Box", "", 1)
        strN301 = Replace(strN301, "POST OFFICE Box", "", 1)
    End If
    
    strN3 = "N3*" & strN301 & "~"
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strN3
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strN3 Else Print #1, strN3;
    End If
    g_lngNumSeg = g_lngNumSeg + 1
    
    strN4 = "N4*" & rst.Fields("fldInsCity").Value & "*" & rst.Fields("fldInsState").Value & "*" & NumbersOnly(rst.Fields("fldInsZip").Value) & "~"
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strN4
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strN4 Else Print #1, strN4;
    End If
    g_lngNumSeg = g_lngNumSeg + 1
    
    If (strPayerCode = "01260") Then   'Magellan requires 01260    Capario
        strRef01 = "FY"                 'Magellan requires 01260
        strRef02 = "01260"
        strRef = "REF*" & strRef01 & "*" & strRef02 & "~"
        If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
            g_PrtString = g_PrtString + strRef
            If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
            End If
        Else
            If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
        End If
        g_lngNumSeg = g_lngNumSeg + 1
    End If
    
    If (strPayerCode = "F01260") Then   'Magellan requires PO BOX Number   ThinEDI
        strRef01 = "FY"                 'Magellan requires PO BOX Number
        strRef02 = strN301
        strRef = "REF*" & strRef01 & "*" & strRef02 & "~"
        If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
            g_PrtString = g_PrtString + strRef
            If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
            End If
        Else
            If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
        End If
        g_lngNumSeg = g_lngNumSeg + 1
    End If
    
    If (rst.Fields("fldPlanID").Value = "5717") Then    'Advocate Health PLans - Humana
        strRef01 = "FY"                                 'Advocate Health PLans
        strRef02 = "0010"
        strRef = "REF*" & strRef01 & "*" & strRef02 & "~"
        If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
            g_PrtString = g_PrtString + strRef
            If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
            End If
        Else
            If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
        End If
        g_lngNumSeg = g_lngNumSeg + 1
    End If
    
    If (rst.Fields("fldPlanID").Value = "5134") Then    'Advocate Health PLans - Other
        strRef01 = "FY"                                 'Advocate Health PLans
        strRef02 = "0011"
        strRef = "REF*" & strRef01 & "*" & strRef02 & "~"
        If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
            g_PrtString = g_PrtString + strRef
            If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
            End If
        Else
            If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
        End If
        g_lngNumSeg = g_lngNumSeg + 1
    End If
    
    If (rst.Fields("fldPlanID").Value = 556 Or rst.Fields("fldPlanID").Value = 3238 Or rst.Fields("fldPlanID").Value = 545) Then    'Medicaid of TX, LA
      strRef01 = "G2"
      strRef02 = StripDelimiters(IfNull(rst.Fields("fldGrpPracticeNum").Value, IfNull(rst.Fields("fldIndPracticeNum").Value, "")))
      strRef = "REF*" & strRef01 & "*" & strRef02 & "~"
      If strRef02 > "" Then
        If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
            g_PrtString = g_PrtString + strRef
            If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
            End If
        Else
            If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
        End If
        g_lngNumSeg = g_lngNumSeg + 1
      End If
    End If
    
    
    Exit Function
        
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Private Sub WriteLoop2300(ByVal rst As ADODB.Recordset, ByVal rstClrHse As ADODB.Recordset, ByVal strPayerCode As String)
'Claim information
    
    'Loop 2300 - Claim Level Information
    Dim strClaim, strClm01, strClm02, strClm03 As String
    Dim strClm04, strClm05, strClm06 As String
    Dim strClm07, strClm08, strClm09 As String
    Dim strClm10, strClm11, strClm12, strClm18, strClm20 As String
    Dim strClm05_1, strClm05_2, strClm05_3 As String
    Dim strClm11_1, strClm11_2, strClm11_3, strClm11_4, strClm11_5 As String
    Dim strAmt, strAmt01, strAmt02, strAmt03, strAmt04 As String
    Dim strAMT05, strAMT06, strAMT07, strAMT08, strAMT09 As String
    Dim strDTP, strDTP_01, strDTP_02, strDTP_03 As String
    Dim strCL1, strCL1_01, strCL1_02, strCL1_03, strCL1_04 As String
    Dim strRef, strRef01, strRef02 As String
    Dim strNte, strNte01, strNte02 As String
    Dim strHI, strBK, strBF, strBJ As String
    
    On Error GoTo ErrTrap:
        
    strClm01 = rst.Fields("fldEncounterLogID").Value   'our account number
    If IfNull(rst.Fields("fldClaimTotal").Value, 0) > 0 Then
        strClm02 = rst.Fields("fldClaimTotal").Value
    Else
        strClm02 = rst.Fields("fldFee").Value * rst.Fields("fldUnits").Value  'total claim charge amount
        If rst.Fields("fldAddOnCPTCode").Value > "" Then
            strClm02 = rst.Fields("fldFee").Value * rst.Fields("fldUnits").Value + rst.Fields("fldAddOnFee").Value * IIf(rst.Fields("fldAddOnUnits").Value < 0, 1, rst.Fields("fldAddOnUnits").Value) 'Monetary amount PG 402
            If rst.Fields("fldAddOnSecCPTCode").Value > "" Then
                strClm02 = rst.Fields("fldFee").Value * rst.Fields("fldUnits").Value + rst.Fields("fldAddOnFee").Value * IIf(rst.Fields("fldAddOnUnits").Value < 0, 1, rst.Fields("fldAddOnUnits").Value) + rst.Fields("fldAddOnSecFee").Value * IIf(rst.Fields("fldAddOnSecUnits").Value < 0, 1, rst.Fields("fldAddOnSecUnits").Value)  'Monetary amount PG 402
            End If
        End If
    End If
    
    strClm03 = ""             'not used
    strClm04 = ""             'not used
    strClm05_1 = rst.Fields("fldPOS").Value
    If rst.Fields("fldClaimType").Value = "I" Then
        Select Case rst.Fields("fldPOS").Value            'Type Of Bill
            Case "13"
                strClm05_1 = "22"
            Case "22"
                strClm05_1 = "13"
            Case "21"
                If rst.Fields("fldInsuranceID").Value <> 24 Then
                    strClm05_1 = "11"
                Else
                    strClm05_1 = "12"
                End If
            Case "31"
                If rst.Fields("fldInsuranceID").Value <> 24 Then
                    strClm05_1 = "21"
                Else
                    strClm05_1 = "22"
                End If
            Case "32"
                If rst.Fields("fldInsuranceID").Value <> 24 Then
                    strClm05_1 = "21"
                Else
                    strClm05_1 = "22"
                End If
            Case "34"
                strClm05_1 = "12"
            Case "89"
                strClm05_1 = "89"
        End Select
    End If
    
    strClm05_2 = ""           'not used
    If g_bln5010 Then strClm05_2 = "B"    '5010 does use this field
    'Institutional Claims
    If rst.Fields("fldClaimType").Value = "I" Then strClm05_2 = "A"
    
    'Replacement Claim
    If Trim(IfNull(rst.Fields("fldICN").Value, "")) > "" Then
        If rst.Fields("fldOrder").Value = 1 Then
            strClm05_3 = "7"          'claim frequency code 7 - Replacement claim
        Else
            strClm05_3 = "1"          'claim frequency code 1 - Original
        End If
        If rst.Fields("fldClaimFrequency").Value = "8" And Trim(IfNull(rst.Fields("fldICN").Value, "")) > "" Then strClm05_3 = "8" 'void claim
    Else
        strClm05_3 = "1"          'claim frequency code 1 - Original
    End If
    'Institutional Claims
    If rst.Fields("fldClaimType").Value = "I" Then
        If Trim(IfNull(rst.Fields("fldICN").Value, "")) > "" Then
            If rst.Fields("fldOrder").Value = 1 Then
                strClm05_3 = "7"          'claim frequency code 7 - Replacement claim
            Else
                strClm05_3 = "3"          'claim frequency code 3 - Interim Claim this is a continuing claim
            End If
        Else
     '      strClm05_3 = "1"          'claim frequency code 1 - Admission thru Discharge claim
     '      strClm05_3 = "2"          'claim frequency code 2 - Interim Claim this is the first claim
            strClm05_3 = "3"          'claim frequency code 3 - Interim Claim this is a continuing claim
     '      strClm05_3 = "4"          'claim frequency code 4 - Interim Claim this is the last claim
     '      strClm05_3 = "8"          'claim frequency code 8 - Void or Cancel previous claim
        End If
    End If
    If rst.Fields("fldEncounterLogID").Value = 5295114 Then strClm05_3 = "2"
    
    strClm06 = "Y"            'provider signature on file?
    If g_bln5010 And rst.Fields("fldClaimType").Value = "I" Then strClm06 = ""    '5010 Institutional Claims Not Used-> provider signature on file?
    strClm07 = "A"            'provider accept Medicare Assignment code
    If IfNull(rst.Fields("fldAssignmentYN").Value, "Y") = "N" And rst.Fields("fldInsuranceID").Value <> 24 Then strClm07 = "C" 'Not Assigned
    strClm08 = "Y"            'assignment of benefits indicator fldAssignmentYN
    If Not IsNull(rst.Fields("fldAssignmentYN").Value) Then strClm08 = rst.Fields("fldAssignmentYN").Value
    strClm09 = "Y"            'release of information code
    strClm10 = "B"            'patient signature source code
    If g_bln5010 Then strClm10 = "P"    '5010 Provider is the source of the Signature
    If g_bln5010 And rst.Fields("fldClaimType").Value = "I" Then strClm10 = ""    '5010 Institutional Claims Not Used-> Signature Source
    
    'Sub-elements CLM11-1 to 11-5 pertain to Auto Accident (AA), Other Accident (OA), Employment (EM), etc
    If IfNull(rst.Fields("fldCondEmployYN").Value, "") = "Y" Then strClm11_1 = "EM"
    If IfNull(rst.Fields("fldCondAutoYN").Value, "") = "Y" Then strClm11_1 = "AA"
    If IfNull(rst.Fields("fldCondOtherYN").Value, "") = "Y" Then strClm11_1 = "OA"
    strClm11_2 = ""
    strClm11_3 = ""
    
    If strClm11_1 = "AA" Then strClm11_4 = rst.Fields("fldCondAutoState").Value
   
    If strClm11_1 > "" Then strClm11 = strClm11_1
    If strClm11_4 > "" Then strClm11 = strClm11 & g_ElementSeperator & strClm11_2 & g_ElementSeperator & strClm11_3 & g_ElementSeperator & strClm11_4
    
    'Institutional Claims
    strClm18 = ""
    If rst.Fields("fldClaimType").Value = "I" Then strClm18 = "Y"
    If g_bln5010 And rst.Fields("fldClaimType").Value = "I" Then strClm18 = ""    '5010 Institutional Claims Not Used-> Response Code
    
    strClm20 = ""
    If rst.Fields("fldInsType").Value = "MC" And strPayerCode = "141797357" Then    'Computer Sciences of NY
        If (Not IsNull(rst.Fields("fldDOS").Value)) Then
            If DateDiff("D", rst.Fields("fldDOS").Value, Now()) > 90 Then       'Delay Reason Code
                If rst.Fields("fldOrder").Value = 1 Then
                    strClm20 = "9"                                                  'Original Claim Rejected or Denied due to an Unrelated Reason
                Else
                    strClm20 = "7"                                                  'Third Party Processing Delay
                End If
            End If
        End If
    End If
    
    If strClm20 > "" Then
        strClaim = "CLM*" & strClm01 & "*" & strClm02 & "*" & strClm03 _
            & "*" & strClm04 & "*" & strClm05_1 & g_ElementSeperator & strClm05_2 & g_ElementSeperator & strClm05_3 & "*" & strClm06 _
            & "*" & strClm07 & "*" & strClm08 & "*" & strClm09 & "*" & strClm10 & "*" & strClm11 _
            & "*******" & strClm18 & "**" & strClm20 & "~"
    ElseIf strClm18 > "" Then
        strClaim = "CLM*" & strClm01 & "*" & strClm02 & "*" & strClm03 _
            & "*" & strClm04 & "*" & strClm05_1 & g_ElementSeperator & strClm05_2 & g_ElementSeperator & strClm05_3 & "*" & strClm06 _
            & "*" & strClm07 & "*" & strClm08 & "*" & strClm09 & "*" & strClm10 & "*" & strClm11 _
            & "*******" & strClm18 & "~"
    ElseIf strClm11 > "" Then
        strClaim = "CLM*" & strClm01 & "*" & strClm02 & "*" & strClm03 _
            & "*" & strClm04 & "*" & strClm05_1 & g_ElementSeperator & strClm05_2 & g_ElementSeperator & strClm05_3 & "*" & strClm06 _
            & "*" & strClm07 & "*" & strClm08 & "*" & strClm09 & "*" & strClm10 & "*" & strClm11 & "~"
    ElseIf strClm10 > "" Then
        strClaim = "CLM*" & strClm01 & "*" & strClm02 & "*" & strClm03 _
            & "*" & strClm04 & "*" & strClm05_1 & g_ElementSeperator & strClm05_2 & g_ElementSeperator & strClm05_3 & "*" & strClm06 _
            & "*" & strClm07 & "*" & strClm08 & "*" & strClm09 & "*" & strClm10 & "~"
    ElseIf strClm09 > "" Then
        strClaim = "CLM*" & strClm01 & "*" & strClm02 & "*" & strClm03 _
            & "*" & strClm04 & "*" & strClm05_1 & g_ElementSeperator & strClm05_2 & g_ElementSeperator & strClm05_3 & "*" & strClm06 _
            & "*" & strClm07 & "*" & strClm08 & "*" & strClm09 & "~"
    Else
        strClaim = "CLM*" & strClm01 & "*" & strClm02 & "*" & strClm03 _
            & "*" & strClm04 & "*" & strClm05_1 & g_ElementSeperator & strClm05_2 & g_ElementSeperator & strClm05_3 & "*" & strClm06 _
            & "*" & strClm07 & "*" & strClm08 & "*" & strClm09 & "*" & strClm10 & "~"
    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strClaim
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strClaim Else Print #1, strClaim;
    End If
    g_lngNumSeg = g_lngNumSeg + 1
    
    
    'Institutional Claims
    If rst.Fields("fldClaimType").Value = "I" Then
        'Statement Date
        strDTP_01 = "434"                'date/time qualifier
        strDTP_02 = "D8"                 'date time period format qualifier
        If g_bln5010 Then strDTP_02 = "RD8" ' RD8 for a range of dates CCYYMMDD-CCYYMMDD
        strDTP_03 = Format(rst.Fields("fldDOS").Value, "yyyymmdd")              'date/time period in CCYYMMDD
        If g_bln5010 Then strDTP_03 = strDTP_03 & "-" & Format(IfNull(rst.Fields("fldEndDOS").Value, rst.Fields("fldDOS").Value), "yyyymmdd") ' CCYYMMDD-CCYYMMDD
        strDTP = "DTP*" & strDTP_01 & "*" & strDTP_02 & "*" & strDTP_03 & "~"
        If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
            g_PrtString = g_PrtString + strDTP
            If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
            End If
        Else
            If g_blnLineFeed Then Print #1, strDTP Else Print #1, strDTP;
        End If
        g_lngNumSeg = g_lngNumSeg + 1
    End If
    
    ' Date of Onset
    If (Not IsNull(rst.Fields("fldDateOfOnset").Value)) Then
        'Not Institutional Claims
        If rst.Fields("fldClaimType").Value <> "I" And rst.Fields("fldDateOfOnset").Value <> rst.Fields("fldDOS").Value And strClm11_1 <> "AA" And strClm11_1 <> "OA" And strClm11_1 <> "EM" Then
            strDTP_01 = "431"                'date/time qualifier
            strDTP_02 = "D8"                 'date time period format qualifier
            strDTP_03 = Format(rst.Fields("fldDateOfOnset").Value, "yyyymmdd")              'date/time period in CCYYMMDD
            strDTP = "DTP*" & strDTP_01 & "*" & strDTP_02 & "*" & strDTP_03 & "~"
            If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
                rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
                g_PrtString = g_PrtString + strDTP
                If Len(g_PrtString) >= 80 Then
                    Print #1, Left(g_PrtString, 80)
                    g_PrtString = Mid(g_PrtString, 81)
                End If
            Else
                If g_blnLineFeed Then Print #1, strDTP Else Print #1, strDTP;
            End If
            g_lngNumSeg = g_lngNumSeg + 1
        End If
    End If
     
    ' Date of Accident
    '    Required when CLM11-1 or CLM11-2 has a value of ‘AA’ or ‘OA’. OR Required when CLM11-1 or CLM11-2
    '    has a value of ‘EM’ and this claim is the result of an accident.
    If (Not IsNull(rst.Fields("fldDateOfOnset").Value)) Then
        'Not Institutional Claims
        If rst.Fields("fldClaimType").Value <> "I" And rst.Fields("fldDateOfOnset").Value <> rst.Fields("fldDOS").Value And (strClm11_1 = "AA" Or strClm11_1 = "OA" Or strClm11_1 = "EM") Then
            strDTP_01 = "439"                'date/time qualifier
            strDTP_02 = "D8"                 'date time period format qualifier
            strDTP_03 = Format(rst.Fields("fldDateOfOnset").Value, "yyyymmdd")              'date/time period in CCYYMMDD
            strDTP = "DTP*" & strDTP_01 & "*" & strDTP_02 & "*" & strDTP_03 & "~"
            If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
                rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
                g_PrtString = g_PrtString + strDTP
                If Len(g_PrtString) >= 80 Then
                    Print #1, Left(g_PrtString, 80)
                    g_PrtString = Mid(g_PrtString, 81)
                End If
            Else
                If g_blnLineFeed Then Print #1, strDTP Else Print #1, strDTP;
            End If
            g_lngNumSeg = g_lngNumSeg + 1
        End If
    End If
    
    ' Date of Onset of Current Illness
'    If (Not IsNull(rst.Fields("fldDateOfOnset").Value)) And (rst.Fields("fldInsuranceID").Value = 1269) Then
'        strDTP_01 = "374"                'date/time qualifier
'        strDTP_02 = "D8"                 'date time period format qualifier
'        strDTP_03 = Format(rst.Fields("fldDateOfOnset").Value, "yyyymmdd")              'date/time period in CCYYMMDD
'        strDTP = "DTP*" & strDTP_01 & "*" & strDTP_02 & "*" & strDTP_03 & "~"
'        If g_blnLineFeed Then Print #1, strDTP Else Print #1, strDTP;
'        g_lngNumSeg = g_lngNumSeg + 1
'    End If
    
    ' Date Last Seen
    If (Not IsNull(rst.Fields("fldDOS").Value)) Then
        strDTP_01 = "304"                'date/time qualifier
        strDTP_02 = "D8"                 'date time period format qualifier
        strDTP_03 = Format(rst.Fields("fldDOS").Value, "yyyymmdd")              'date/time period in CCYYMMDD
        strDTP = "DTP*" & strDTP_01 & "*" & strDTP_02 & "*" & strDTP_03 & "~"
     '   California Medicare South Testing
     '   If g_blnLineFeed Then Print #1, strDTP Else Print #1, strDTP;
     '   g_lngNumSeg = g_lngNumSeg + 1
    End If
   
    'Date of Admission
    If ((Not IsNull(rst.Fields("fldAdmitDate").Value)) And (rst.Fields("fldPOS").Value <> 11)) Then
        strDTP_01 = "435"                'date/time qualifier
        strDTP_02 = "D8"                 'date time period format qualifier
        strDTP_03 = Format(IfNull(rst.Fields("fldAdmitDate").Value, rst.Fields("fldDOS").Value), "yyyymmdd")            'date/time period in CCYYMMDD
        If (rst.Fields("fldClaimType").Value = "I" And Not g_bln5010) Or (rst.Fields("fldClaimType").Value = "I" And rstClrHse.Fields("fldClearingHouseID").Value = 11) Then
            strDTP_02 = "DT"                 'date time period format qualifier
            strDTP_03 = Format(IfNull(rst.Fields("fldAdmitDate").Value, rst.Fields("fldDOS").Value), "yyyymmdd") & "0800"             'date/time period in CCYYMMDD
        End If
        strDTP = "DTP*" & strDTP_01 & "*" & strDTP_02 & "*" & strDTP_03 & "~"
        If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
            g_PrtString = g_PrtString + strDTP
            If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
            End If
        Else
            If g_blnLineFeed Then Print #1, strDTP Else Print #1, strDTP;
        End If
        g_lngNumSeg = g_lngNumSeg + 1
    End If
    
    If (Not IsNull(rst.Fields("fldDischargeDate").Value)) And (rst.Fields("fldPOS").Value <> 11) And (rst.Fields("fldClaimType").Value <> "I") Then
        'fldDischargeDate
        strDTP_01 = "096"                'date/time qualifier
        strDTP_02 = "D8"                 'date time period format qualifier
        strDTP_03 = Format(rst.Fields("fldDischargeDate").Value, "yyyymmdd")              'date/time period in CCYYMMDD
    '    If rst.Fields("fldClaimType").Value = "I" Then
    '        strDTP_02 = "TM"             'date time period format qualifier
    '        strDTP_03 = "1700"           'Just the time HHMM
    '    End If
        strDTP = "DTP*" & strDTP_01 & "*" & strDTP_02 & "*" & strDTP_03 & "~"
        If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
            g_PrtString = g_PrtString + strDTP
            If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
            End If
        Else
            If g_blnLineFeed Then Print #1, strDTP Else Print #1, strDTP;
        End If
        g_lngNumSeg = g_lngNumSeg + 1
    End If
     
    'Institutional Claims
    If g_bln5010 And rst.Fields("fldClaimType").Value = "I" Then
        'Statement Date
        strCL1_01 = "3"                   'Admission Type Code
        strCL1_02 = "1"                   'Admission Source Code
        strCL1_03 = "30"                  'Patient Status Code - 30 => Still Patient or Expected to Return for Outpatient Services
        strCL1_04 = ""                    'Nursing Home Code
        strCL1 = "CL1*" & strCL1_01 & "*" & strCL1_02 & "*" & strCL1_03 & "~"
        If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
            g_PrtString = g_PrtString + strCL1
            If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
            End If
        Else
            If g_blnLineFeed Then Print #1, strCL1 Else Print #1, strCL1;
        End If
        g_lngNumSeg = g_lngNumSeg + 1
    End If
    
    'Patient Pament Amount  AMT - F5 Patient Amount Paid
    
    If g_bln5010 And IfNull(rst.Fields("fldPatientPaid").Value, 0) > 0 Then
            strAmt01 = "F5"
            strAmt02 = Format(IfNull(rst.Fields("fldPatientPaid").Value, 0), "#####.00")
            strAmt = "AMT*" & strAmt01 & "*" & strAmt02 & "~"
            If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
                rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
                g_PrtString = g_PrtString + strAmt
                If Len(g_PrtString) >= 80 Then
                    Print #1, Left(g_PrtString, 80)
                    g_PrtString = Mid(g_PrtString, 81)
                End If
            Else
                If g_blnLineFeed Then Print #1, strAmt Else Print #1, strAmt;
            End If
            g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
    End If
    
    'Cert/authorization information
    If Not IsNull(rst.Fields("fldCertNum").Value) Then
        strRef01 = "G1"
        strRef02 = Trim(StripDelimiters(rst.Fields("fldCertNum").Value))
        strRef = "REF*" & strRef01 & "*" & strRef02 & "~"
        If InStr(1, strRef02, "No Auth") > 0 Or InStr(1, strRef02, "Not Req") > 0 Then strRef02 = ""
        If Trim(strRef02) > "" Then
            If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
                rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
                g_PrtString = g_PrtString + strRef
                If Len(g_PrtString) >= 80 Then
                    Print #1, Left(g_PrtString, 80)
                    g_PrtString = Mid(g_PrtString, 81)
                End If
            Else
                If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
            End If
            g_lngNumSeg = g_lngNumSeg + 1
        End If
    End If
    
    'Clearinghouse Identification Number
    If rstClrHse.Fields("fldClearingHouseID").Value = 1 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 35 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 150 Then   'Availity
        strRef01 = "D9"
        strRef02 = Format(CStr(g_lngEndTxNum), "0000000") & "-" & Format(CStr(rst.Fields("fldClaimID").Value), "0000000") & "-" & rst.Fields("fldPlanID").Value
        strRef = "REF*" & strRef01 & "*" & Right(strRef02, 20) & "~"
        If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
            g_PrtString = g_PrtString + strRef
            If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
            End If
        Else
            If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
        End If
        g_lngNumSeg = g_lngNumSeg + 1
    End If
    
    'Replacement Claim
    If Trim(IfNull(rst.Fields("fldICN").Value, "")) > "" And _
      (strClm05_3 = "6" Or strClm05_3 = "7" Or strClm05_3 = "8" Or strClm05_3 = "G" Or strClm05_3 = "I" Or strClm05_3 = "X" Or strClm05_3 = "Y") Then
        strRef01 = "F8"
        strRef02 = Trim(StripDelimiters(rst.Fields("fldICN").Value))
        strRef = "REF*" & strRef01 & "*" & strRef02 & "~"
        If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
            g_PrtString = g_PrtString + strRef
            If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
            End If
        Else
            If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
        End If
        g_lngNumSeg = g_lngNumSeg + 1
    End If
    
    'DSM Code - decimals are removed from the code
    'fldDSM_IV, fldICD_1, fldICD_2, fldICD_3 , fldICD_4
    'ABK - International Classification of Diseases Clinical Modification (ICD-10-CM) Principal Diagnosis
    'ABF - International Classification of Diseases Clinical Modification (ICD-10-CM) Diagnosis
    
    strBK = "*BK"
    strBF = "*BF"
    strBJ = "*BJ"
    If rst.Fields("fldDOS").Value >= CDate("10/01/2015") Then
        strBK = "*ABK"
        strBF = "*ABF"
        strBJ = "*ABJ"
    End If
    
    If StripDecimals(IfNull(rst.Fields("fldDSM_IV").Value, "")) > "" Then
        strHI = "HI" & strBK & g_ElementSeperator & StripDecimals(IfNull(rst.Fields("fldDSM_IV").Value, ""))
    End If
    If StripDecimals(IfNull(rst.Fields("fldICD_1").Value, "")) > "" And StripDecimals(IfNull(rst.Fields("fldICD_1").Value, "")) <> StripDecimals(IfNull(rst.Fields("fldDSM_IV").Value, "")) Then
        strHI = strHI & strBF & g_ElementSeperator & StripDecimals(IfNull(rst.Fields("fldICD_1").Value, ""))
    End If
    If StripDecimals(IfNull(rst.Fields("fldICD_2").Value, "")) > "" And StripDecimals(IfNull(rst.Fields("fldICD_2").Value, "")) <> StripDecimals(IfNull(rst.Fields("fldDSM_IV").Value, "")) Then
        strHI = strHI & strBF & g_ElementSeperator & StripDecimals(IfNull(rst.Fields("fldICD_2").Value, ""))
    End If
    If StripDecimals(IfNull(rst.Fields("fldICD_3").Value, "")) > "" And StripDecimals(IfNull(rst.Fields("fldICD_3").Value, "")) <> StripDecimals(IfNull(rst.Fields("fldDSM_IV").Value, "")) Then
        strHI = strHI & strBF & g_ElementSeperator & StripDecimals(IfNull(rst.Fields("fldICD_3").Value, ""))
    End If
    If StripDecimals(IfNull(rst.Fields("fldICD_4").Value, "")) > "" And StripDecimals(IfNull(rst.Fields("fldICD_4").Value, "")) <> StripDecimals(IfNull(rst.Fields("fldDSM_IV").Value, "")) Then
        strHI = strHI & strBF & g_ElementSeperator & StripDecimals(IfNull(rst.Fields("fldICD_4").Value, ""))
    End If
    If strHI > "" Then
        strHI = strHI & "~"
    End If
    
    'Institutional Claims Primary Diagnosis
    If rst.Fields("fldClaimType").Value = "I" Then
        If StripDecimals(IfNull(rst.Fields("fldDSM_IV").Value, "")) > "" Then
            strHI = "HI" & strBK & g_ElementSeperator & StripDecimals(IfNull(rst.Fields("fldDSM_IV").Value, "")) & "~"
        End If
    End If
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
        rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strHI
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strHI Else Print #1, strHI;
    End If
    g_lngNumSeg = g_lngNumSeg + 1
    
    'Institutional Claims Admitting Diagnosis       ---->>Removed 07/01/2016
    strHI = ""
    If rst.Fields("fldClaimType").Value = "I" Then
        If StripDecimals(IfNull(rst.Fields("fldDSM_IV").Value, "")) > "" And rst.Fields("fldPOS").Value <> "22" Then
            strHI = "HI" & strBJ & g_ElementSeperator & StripDecimals(IfNull(rst.Fields("fldDSM_IV").Value, "")) & "~" 'Admitting Diagnosis
            If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
                rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
                g_PrtString = g_PrtString + strHI
                If Len(g_PrtString) >= 80 Then
                    Print #1, Left(g_PrtString, 80)
                    g_PrtString = Mid(g_PrtString, 81)
                End If
            Else
                If g_blnLineFeed Then Print #1, strHI Else Print #1, strHI;
            End If
            g_lngNumSeg = g_lngNumSeg + 1
        End If
    End If
    
    'Institutional Claims
    strHI = ""
    If rst.Fields("fldClaimType").Value = "I" Then
        If StripDecimals(IfNull(rst.Fields("fldICD_2").Value, "")) > "" Then
            strHI = "HI" & strBF & g_ElementSeperator & StripDecimals(IfNull(rst.Fields("fldICD_2").Value, ""))
            If StripDecimals(IfNull(rst.Fields("fldICD_3").Value, "")) > "" Then
                strHI = strHI & strBF & g_ElementSeperator & StripDecimals(IfNull(rst.Fields("fldICD_3").Value, ""))
            End If
            If StripDecimals(IfNull(rst.Fields("fldICD_4").Value, "")) > "" Then
                strHI = strHI & strBF & g_ElementSeperator & StripDecimals(IfNull(rst.Fields("fldICD_4").Value, ""))
            End If
            strHI = strHI & "~"
            If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
                rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
                g_PrtString = g_PrtString + strHI
                If Len(g_PrtString) >= 80 Then
                    Print #1, Left(g_PrtString, 80)
                    g_PrtString = Mid(g_PrtString, 81)
                End If
            Else
                If g_blnLineFeed Then Print #1, strHI Else Print #1, strHI;
            End If
            g_lngNumSeg = g_lngNumSeg + 1
        End If
    End If
    
    ' Occurrence Date
    If (Not IsNull(rst.Fields("fldDateOfOnset").Value)) Then
        'Institutional Claims
        If rst.Fields("fldClaimType").Value = "I" Then
            strHI = "HI*BH:11:D8:" & Format(rst.Fields("fldDateOfOnset").Value, "yyyymmdd") & "~"
            If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
                rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
                g_PrtString = g_PrtString + strHI
                If Len(g_PrtString) >= 80 Then
                    Print #1, Left(g_PrtString, 80)
                    g_PrtString = Mid(g_PrtString, 81)
                End If
            Else
                If g_blnLineFeed Then Print #1, strHI Else Print #1, strHI;
            End If
            g_lngNumSeg = g_lngNumSeg + 1
        End If
    End If
    'Magellan requires A Unique numer to identify the claim in the response file
  '  strRef01 = "D9"
  '  strRef02 = rst.Fields("fldEncounterLogID").Value
  '  strRef = "REF*" & strRef01 & "*" & strRef02 & "~"
  '  If (strPayerCode = "f01260" Or strPayerCode = "F01260") Then   'Magellan requires 01260
  '      If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
  '      g_lngNumSeg = g_lngNumSeg + 1
  '  End If
        
    'CLIA Number Enter the 10-digit CLIA
    'If rst.Fields("fldClia").Value > "" Then
   '    strRef01 = "X4"
   '    strRef02 = rst.Fields("fldClia").Value
   '    strRef = "REF*" & strRef01 & "*" & strRef02 & "~"
    '    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
    '        rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
    '        g_PrtString = g_PrtString + strNte
    '        If Len(g_PrtString) >= 80 Then
    '            Print #1, Left(g_PrtString, 80)
    '            g_PrtString = Mid(g_PrtString, 81)
    '        End If
    '    Else
    '        If g_blnLineFeed Then Print #1, strNte Else Print #1, strNte;
    '    End If
    '   g_lngNumSeg = g_lngNumSeg + 1
   ' End If
    
    'Claim Notes - TriWest uses this when there is a referring physician
    If Not IsNull(rst.Fields("fldReserved19").Value) And Right(strPayerCode, 5) = "WESTR" And Trim(StripDelimiters(IfNull(rst.Fields("fldReferPhy").Value, ""))) > "" Then
        strNte01 = "ADD"
        strNte02 = Trim(rst.Fields("fldReserved19").Value)
        strNte = "NTE*" & strNte01 & "*" & strNte02 & "~"
        If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
            g_PrtString = g_PrtString + strNte
            If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
            End If
        Else
            If g_blnLineFeed Then Print #1, strNte Else Print #1, strNte;
        End If
        g_lngNumSeg = g_lngNumSeg + 1
    End If
    
    
    Exit Sub
        
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub

Private Sub WriteLoop2310A(ByVal rst As ADODB.Recordset, ByVal rstClrHse As ADODB.Recordset)
'Referring provider information

    Dim strNM101, strNM102, strNM103, strNM104, strNM105 As String
    Dim strNM106, strNM107, strNM108, strNM109 As String
    Dim strNM1 As String
    Dim strPRV, strPRV01, strPRV02, strPRV03, strPRV04 As String
    Dim strRef, strRef01, strRef02 As String
    Dim strFirst, strLast, strMI, strTitle, strWrk As String
    Dim intLength, intStart As Integer
    
    On Error GoTo ErrTrap:
    'Find the last and first name of the Referring Provider
    intStart = 1: strFirst = "": strLast = "": strMI = "": strTitle = ""
    strWrk = Trim(IfNull(rst.Fields("fldReferPhy").Value, ""))
    If Right(strWrk, 1) = "." Then strWrk = Mid(strWrk, 1, Len(strWrk) - 1)
 '   strWrk = "Last, First"
    strWrk = Trim(Replace(strWrk, "  ", " "))
    strWrk = Trim(Replace(strWrk, ", III", ""))
    strWrk = Trim(Replace(strWrk, ",III", ""))
    strWrk = Trim(Replace(strWrk, "III", ""))
    strWrk = Trim(Replace(strWrk, ", Jr.", ""))
    strWrk = Trim(Replace(strWrk, " Jr.", ""))
    strWrk = Trim(Replace(strWrk, "Jr.", ""))
    strWrk = Trim(Replace(strWrk, ", Sr.", ""))
    strWrk = Trim(Replace(strWrk, " Sr.", ""))
    strWrk = Trim(Replace(strWrk, "Sr.", ""))
    If Left(strWrk, 4) = "DR. " Then strTitle = "DR": strWrk = Mid(strWrk, 5)
    If Left(strWrk, 3) = "DR." Then strTitle = "DR": strWrk = Mid(strWrk, 4)
    If Left(strWrk, 3) = "DR " Then strTitle = "DR": strWrk = Mid(strWrk, 4)
    If Right(strWrk, 4) = ", MD" Then strTitle = "MD": strWrk = Left(strWrk, Len(strWrk) - 4)
    If Right(strWrk, 3) = ",MD" Then strTitle = "MD": strWrk = Left(strWrk, Len(strWrk) - 3)
    If Right(strWrk, 3) = " MD" Then strTitle = "MD": strWrk = Left(strWrk, Len(strWrk) - 3)
    If Right(strWrk, 6) = ", M.D." Then strTitle = "MD": strWrk = Left(strWrk, Len(strWrk) - 6)
    If Right(strWrk, 5) = ", M.D" Then strTitle = "MD": strWrk = Left(strWrk, Len(strWrk) - 5)
    If Right(strWrk, 5) = ",M.D." Then strTitle = "MD": strWrk = Left(strWrk, Len(strWrk) - 5)
    If Right(strWrk, 4) = ",M.D" Then strTitle = "MD": strWrk = Left(strWrk, Len(strWrk) - 4)
    If Right(strWrk, 5) = " M.D." Then strTitle = "MD": strWrk = Left(strWrk, Len(strWrk) - 5)
    If Right(strWrk, 4) = " M.D" Then strTitle = "MD": strWrk = Left(strWrk, Len(strWrk) - 4)
    If Right(strWrk, 4) = ", DO" Then strTitle = "DO": strWrk = Left(strWrk, Len(strWrk) - 4)
    If Right(strWrk, 3) = ",DO" Then strTitle = "DO": strWrk = Left(strWrk, Len(strWrk) - 3)
    If Right(strWrk, 6) = ", D.O." Then strTitle = "DO": strWrk = Left(strWrk, Len(strWrk) - 6)
    If Right(strWrk, 5) = ", D.O" Then strTitle = "DO": strWrk = Left(strWrk, Len(strWrk) - 5)
    If Right(strWrk, 5) = ",D.O." Then strTitle = "DO": strWrk = Left(strWrk, Len(strWrk) - 5)
    If Right(strWrk, 4) = ",D.O" Then strTitle = "DO": strWrk = Left(strWrk, Len(strWrk) - 4)
    If Right(strWrk, 5) = " D.O." Then strTitle = "DO": strWrk = Left(strWrk, Len(strWrk) - 5)
    If Right(strWrk, 5) = " D.O" Then strTitle = "DO": strWrk = Left(strWrk, Len(strWrk) - 4)
    strWrk = Trim(Replace(strWrk, "  ", " "))
    strNM102 = "1"                 'entity type qualifier (1 = person)
    strNM101 = "DN"                'entity identifier code (DN = referring physician)
    'First M. Last, Title
    If InStr(1, strWrk, ".") > 0 And InStr(1, strWrk, ",") > 0 And _
      (InStr(1, strWrk, ".") < InStr(1, strWrk, ",")) And _
      (InStr(1, strWrk, " ") < InStr(1, strWrk, ".")) Then
      intLength = InStr(1, strWrk, " ")
      strFirst = Trim(Mid(strWrk, 1, intLength - 1))
      intStart = intLength + 1
      strMI = Mid(strWrk, intStart, 1)
      intStart = InStr(intStart, strWrk, ". ") + 2
      If intStart = 2 Then intStart = InStr(1, strWrk, ".") + 1
      intLength = InStr(intStart, strWrk, ",") - intStart
      strLast = Trim(Mid(strWrk, intStart, intLength))
      intStart = InStr(1, strWrk, ",") + 1
      strTitle = Trim(Mid(strWrk, intStart))
   'First M Last, Title
   ElseIf InStr(1, strWrk, ".") = 0 And InStr(1, strWrk, ",") > 0 And _
      (InStr(1, strWrk, " ") < InStr(1, strWrk, ",")) And _
      (InStr(InStr(1, strWrk, " ") + 1, strWrk, " ") > 0) And _
      (InStr(InStr(1, strWrk, " ") + 1, strWrk, " ") < InStr(1, strWrk, ",")) Then
      intLength = InStr(1, strWrk, " ")
      strFirst = Trim(Mid(strWrk, 1, intLength - 1))
      intStart = intLength + 1
      strMI = Mid(strWrk, intStart, 1)
      intStart = InStr(intStart, strWrk, " ") + 1
      intLength = InStr(intStart, strWrk, ",") - intStart
      strLast = Trim(Mid(strWrk, intStart, intLength))
      intStart = InStr(1, strWrk, ",") + 1
      strTitle = Trim(Mid(strWrk, intStart))
   'First Last, Title
   ElseIf InStr(1, strWrk, ".") = 0 And InStr(1, strWrk, ",") > 0 And _
      (InStr(1, strWrk, " ") < InStr(1, strWrk, ",")) And _
      (InStr(InStr(1, strWrk, " ") + 1, strWrk, " ") > 0) And _
      (InStr(InStr(1, strWrk, " ") + 1, strWrk, " ") > InStr(1, strWrk, ",")) Then
      intLength = InStr(1, strWrk, " ")
      strFirst = Trim(Mid(strWrk, 1, intLength - 1))
      intStart = intLength + 1
      strMI = ""
      intLength = InStr(intStart, strWrk, ",") - intStart
      strLast = Trim(Mid(strWrk, intStart, intLength))
      intStart = InStr(1, strWrk, ",") + 1
      strTitle = Trim(Mid(strWrk, intStart))
    'First M. Last
    ElseIf InStr(1, strWrk, ".") > 0 And InStr(1, strWrk, ",") = 0 And _
      (InStr(1, strWrk, ". ") > 0) And _
      (InStr(1, strWrk, " ") < InStr(InStr(1, strWrk, " ") + 1, strWrk, " ")) Then
      intLength = InStr(1, strWrk, " ")
      strFirst = Trim(Mid(strWrk, intStart, intLength - 1))
      intStart = intLength + 1
      strMI = Mid(strWrk, intStart, 1)
      intStart = InStr(intStart, strWrk, ". ") + 2
      If intStart = 3 Then intStart = InStr(1, strWrk, ".") + 2
      strLast = Trim(Mid(strWrk, intStart))
    '  strTitle = ""
   'First Last
   ElseIf InStr(1, strWrk, ".") = 0 And InStr(1, strWrk, ",") = 0 And _
      (InStr(1, strWrk, " ") > 0) Then
      intLength = InStr(1, strWrk, " ")
      strFirst = Trim(Mid(strWrk, 1, intLength - 1))
      intStart = intLength + 1
      strMI = ""
      intStart = 1
      intStart = InStr(intStart, strWrk, " ") + 1
      strLast = Trim(Mid(strWrk, intStart))
    '  strTitle = ""
   'Last, First
   ElseIf InStr(1, strWrk, ",") > 0 Then
      intLength = InStr(1, strWrk, ",")
      strLast = Mid(strWrk, 1, intLength - 1)
      intStart = intLength + 2
      If InStr(intStart, strWrk, " ") > 0 Then 'Middle Initital
            intLength = InStr(intStart, strWrk, " ")
            strFirst = Trim(Mid(strWrk, intStart, intLength - intStart))
            intStart = intLength + 1
            strMI = Mid(strWrk, intStart, 1)
            intStart = intLength + 1
        Else
            strFirst = Trim(Right(strWrk, Len(strWrk) - intLength))
        End If
    'DR. GERALD R.WOODARD D.O.
'    ElseIf InStr(1, strWrk, ".") > 0 And InStr(1, strWrk, ",") = 0 And _
'      (InStr(1, strWrk, ".") < InStr(1, strWrk, ",")) And _
'      (InStr(1, strWrk, " ") < InStr(1, strWrk, ".")) Then
'      intLength = InStr(1, strWrk, " ")
'      strFirst = Trim(Mid(strWrk, 1, intLength - 1))
'      intStart = intLength + 1
'      strMI = Mid(strWrk, intStart, 1)
'      intStart = InStr(intStart, strWrk, ". ") + 2
'      If intStart = 2 Then intStart = InStr(1, strWrk, ".") + 1
'      intLength = InStr(intStart, strWrk, ",") - intStart
'      strLast = Trim(Mid(strWrk, intStart, intLength))
'      intStart = InStr(1, strWrk, ",") + 1
'      strTitle = Trim(Mid(strWrk, intStart))
    Else
        strLast = strWrk
        strNM102 = "2"                 'entity type qualifier (2 = entity)
    End If
    
    If Trim(StripDelimiters(IfNull(rst.Fields("fldReferNPI").Value, ""))) > "" And _
      Trim(StripDelimiters(IfNull(rst.Fields("fldReferFirst").Value, ""))) > "" And _
      Trim(StripDelimiters(IfNull(rst.Fields("fldReferLast").Value, ""))) > "" Then
      strFirst = Trim(StripDelimiters(IfNull(rst.Fields("fldReferFirst").Value, "")))
      strLast = Trim(StripDelimiters(IfNull(rst.Fields("fldReferLast").Value, "")))
      strMI = Trim(StripDelimiters(IfNull(rst.Fields("fldReferMI").Value, "")))
      strTitle = Trim(StripDelimiters(IfNull(rst.Fields("fldReferTitle").Value, "")))
    End If
    
    strNM103 = Trim(Mid(strLast, 1, 35)) 'last name
    strNM104 = Trim(Mid(strFirst, 1, 25)) 'first name
    strNM105 = Trim(Mid(strMI, 1, 25)) 'middle name
    strNM106 = ""                  'name prefix (not used)
    strNM107 = ""                  'name suffix
    strNM108 = "24"                'identification code qualifier
    If Trim(StripDelimiters(IfNull(rst.Fields("fldReferNPI").Value, ""))) > "" Then
        strNM109 = Trim(StripDelimiters(IfNull(rst.Fields("fldReferNPI").Value, ""))) 'npi number
    Else
        strNM109 = Trim(StripDelimiters(IfNull(rst.Fields("fldReferNum").Value, ""))) 'Reference number
    End If
    
    'fldReferNum is the NOT not the Tax ID
    If Len(strNM109) = 10 Then strNM108 = "XX"
    
    'fldReferNum is the UPIN not the Tax ID
    If Len(strNM109) <> 9 And Len(strNM109) <> 10 Then strNM109 = ""
    
    If strNM109 > "" Then
        strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
         & "*" & strNM104 & "*" & strNM105 & "*" & strNM106 _
         & "*" & strNM107 & "*" & strNM108 & "*" & strNM109 _
         & "~"
    ElseIf strNM105 > "" Then
        strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
         & "*" & strNM104 & "*" & strNM105 & "~"
    Else
        strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
         & "*" & strNM104 & "~"
    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strNM1
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strNM1 Else Print #1, strNM1;
    End If
    g_lngNumSeg = g_lngNumSeg + 1
    
    If Not g_bln5010 Then
        strPRV01 = "RF"                 'Provider Code (BI-Billing, PT-Payto, RF-Referring)
        'Institutional Claims
        If rst.Fields("fldClaimType").Value = "I" Then strPRV01 = "AT"  'entity identifier code (AT = attending provider)
        strPRV02 = "ZZ"                 'Reference Identification Qualifier
        If g_bln5010 Then strPRV02 = "PXC"    '5010 uses PXC for the Taxonomy Code
        'Specialty information
        strPRV03 = "101Y00000X"         'ZZ relies on Provider Taxonomy Code published by BC/BS Association
    
        strPRV = "PRV*" & strPRV01 & "*" & strPRV02 & "*" & strPRV03 & "~"
        If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
            g_PrtString = g_PrtString + strPRV
            If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
            End If
        Else
            If g_blnLineFeed Then Print #1, strPRV Else Print #1, strPRV;
        End If
        g_lngNumSeg = g_lngNumSeg + 1
    End If
    
    If Not g_bln5010 And strNM108 <> "XX" Then
      strRef01 = "XX"                 'Refer Provider NPI Number
      strRef02 = Trim(StripDelimiters(IfNull(rst.Fields("fldReferNPI").Value, ""))) 'NPI Only
    
      strRef = "REF*" & strRef01 & "*" & strRef02 & "~"
      If strRef02 > "" Then
        If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
            g_PrtString = g_PrtString + strRef
            If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
            End If
        Else
            If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
        End If
        g_lngNumSeg = g_lngNumSeg + 1
      End If
    End If
    
    Exit Sub
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub
Private Sub WriteLoop2310B(ByVal rst As ADODB.Recordset, ByVal rstClrHse As ADODB.Recordset)
'Rendering provider information

    Dim strNM101, strNM102, strNM103, strNM104, strNM105 As String
    Dim strNM106, strNM107, strNM108, strNM109 As String
    Dim strNM1 As String
    Dim strPRV, strPRV01, strPRV02, strPRV03, strPRV04 As String
    Dim strRef, strRef01, strRef02 As String
    
    On Error GoTo ErrTrap:
    
    strNM101 = "82"                'entity identifier code (82 = rendering provider)
    'Institutional Claims
    If rst.Fields("fldClaimType").Value = "I" Then strNM101 = "71"  'entity identifier code (71 = attending provider)
    
    strNM102 = "1"                 'entity type qualifier (1 = person)
    strNM103 = Trim(StripDelimiters(Mid(Replace(IfNull(rst.Fields("fldProviderLastName").Value, ""), "'", ""), 1, 35)))    'Last name
    strNM104 = Trim(StripDelimiters(Mid(Replace(IfNull(rst.Fields("fldProviderFirstName").Value, ""), "'", ""), 1, 35)))   'first name
    strNM105 = IfNull(rst.Fields("fldProviderMI").Value, "")   'middle name
    
    If rst.Fields("fldUseSuperYN").Value = "Y" Then
        If (IfNull(rst.Fields("fldSuperNPI").Value, "") = IfNull(rst.Fields("fldProviderNPI").Value, "") Or _
            IfNull(rst.Fields("fldProviderNPI").Value, "") = "") And _
           (IfNull(rst.Fields("fldSuperNPI").Value, "") > "") Then
            strNM103 = Trim(StripDelimiters(IfNull(rst.Fields("fldSuperLastName").Value, Mid(Replace(rst.Fields("fldProviderLastName").Value, "'", ""), 1, 35))))     'Last name
            strNM104 = Trim(StripDelimiters(IfNull(rst.Fields("fldSuperFirstName").Value, Mid(Replace(rst.Fields("fldProviderFirstName").Value, "'", ""), 1, 35))))   'first name
            strNM105 = IfNull(rst.Fields("fldSuperMI").Value, "")   'middle name
        ElseIf rst.Fields("fldInsuranceID").Value = 1235 Then
            strNM103 = Trim(StripDelimiters(IfNull(rst.Fields("fldSuperLastName").Value, Mid(Replace(rst.Fields("fldProviderLastName").Value, "'", ""), 1, 35))))     'Last name
            strNM104 = Trim(StripDelimiters(IfNull(rst.Fields("fldSuperFirstName").Value, Mid(Replace(rst.Fields("fldProviderFirstName").Value, "'", ""), 1, 35))))   'first name
            strNM105 = IfNull(rst.Fields("fldSuperMI").Value, "")   'middle name
      End If
    End If
    strNM106 = ""                  'name prefix (not used)
    strNM107 = ""                  'name suffix
    strNM108 = "XX"                'NPI
    strNM109 = IfNull(rst.Fields("fldProviderNPI").Value, "")
        
    If IIf(rst.Fields("fldUseSuperYN").Value = "Y", IfNull(rst.Fields("fldSuperNPI").Value, ""), IfNull(rst.Fields("fldProviderNPI").Value, "")) = "" Then
        If rst.Fields("fldTinType").Value = 1 Then
            strNM108 = "34"        'individual SSN SY
        Else
            strNM108 = "24"        '24 = employer'S ID EI
        End If
        strNM109 = Left(NumbersOnly(rst.Fields("fldTIN").Value), 9)         'billing provider identifier code
    ElseIf rst.Fields("fldUseSuperYN").Value = "Y" Then
        If (IfNull(rst.Fields("fldSuperNPI").Value, "") = IfNull(rst.Fields("fldProviderNPI").Value, "") Or _
            IfNull(rst.Fields("fldProviderNPI").Value, "") = "") And _
           (IfNull(rst.Fields("fldSuperNPI").Value, "") > "") Then
            strNM109 = IfNull(rst.Fields("fldSuperNPI").Value, IfNull(rst.Fields("fldProviderNPI").Value, ""))
        ElseIf rst.Fields("fldInsuranceID").Value = 1235 Then
            strNM109 = IfNull(rst.Fields("fldSuperNPI").Value, IfNull(rst.Fields("fldProviderNPI").Value, ""))
        End If
    End If
    
    'fldReferNum is the UPIN not the Tax ID
    If Len(strNM109) <= 6 Then strNM109 = ""
    
    If strNM109 > "" Then
        strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
         & "*" & strNM104 & "*" & strNM105 & "*" & strNM106 _
         & "*" & strNM107 & "*" & strNM108 & "*" & strNM109 _
         & "~"
    ElseIf strNM105 > "" Then
        strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
         & "*" & strNM104 & "*" & strNM105 & "~"
    Else
        strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
         & "*" & strNM104 & "~"
    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
        rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strNM1
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strNM1 Else Print #1, strNM1;
    End If
    g_lngNumSeg = g_lngNumSeg + 1
    
    strPRV01 = "PE"                 'Provider Code (PE-Performing, BI-Billing, PT-Payto, RF-Referring)
    'Institutional Claims
    If rst.Fields("fldClaimType").Value = "I" Then strPRV01 = "AT"  'entity identifier code (AT = attending provider)
    strPRV02 = "ZZ"                 'Reference Identification Qualifier
    If g_bln5010 Then strPRV02 = "PXC"    '5010 uses PXC for the Taxonomy Code
    
    'Specialty information
    strPRV03 = Trim(StripDelimiters(IfNull(rst.Fields("fldTaxonomyCode").Value, "101Y00000X")))         'ZZ relies on Provider Taxonomy Code published by BC/BS Association
    If rst.Fields("fldUseSuperYN").Value = "Y" Then
        If (IfNull(rst.Fields("fldSuperNPI").Value, "") = IfNull(rst.Fields("fldProviderNPI").Value, "") Or _
            IfNull(rst.Fields("fldProviderNPI").Value, "") = "") And _
           (IfNull(rst.Fields("fldSuperNPI").Value, "") > "") Then
            strPRV03 = Trim(StripDelimiters(IfNull(rst.Fields("fldSuperTaxonomyCode").Value, "101Y00000X")))         'ZZ relies on Provider Taxonomy Code published by BC/BS Association
        ElseIf rst.Fields("fldInsuranceID").Value = 1235 Then
            strPRV03 = IfNull(rst.Fields("fldSuperTaxonomyCode").Value, IfNull(rst.Fields("fldTaxonomyCode").Value, "101Y00000X"))
        End If
    End If
    If strPRV03 > "" Then
        strPRV = "PRV*" & strPRV01 & "*" & strPRV02 & "*" & strPRV03 & "~"
        If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
            g_PrtString = g_PrtString + strPRV
            If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
            End If
        Else
            If g_blnLineFeed Then Print #1, strPRV Else Print #1, strPRV;
        End If
        g_lngNumSeg = g_lngNumSeg + 1
    End If
            
    'Physician Type
    If rst.Fields("fldInsType").Value = "MP" Then
            strRef01 = "1C"
            strRef02 = StripDelimiters(IfNull(rst.Fields("fldIndPracticeNum").Value, ""))
    ElseIf rst.Fields("fldInsType").Value = "MB" Then
            strRef01 = "1C"
            strRef02 = StripDelimiters(IfNull(rst.Fields("fldIndPracticeNum").Value, ""))
    ElseIf rst.Fields("fldInsType").Value = "MC" Then
            strRef01 = "1D"
            strRef02 = StripDelimiters(IfNull(rst.Fields("fldIndPracticeNum").Value, ""))
    ElseIf rst.Fields("fldInsType").Value = "BL" Then 'BC/BS
            strRef01 = "1B"
            strRef02 = StripDelimiters(IfNull(rst.Fields("fldIndPracticeNum").Value, ""))
    ElseIf rst.Fields("fldInsType").Value = "BQ" Then 'BC/BS
            strRef01 = "N5"
            strRef02 = StripDelimiters(IfNull(rst.Fields("fldIndPracticeNum").Value, ""))
    ElseIf rst.Fields("fldInsType").Value = "CH" Then
            strRef01 = "1H"
            strRef02 = StripDelimiters(IfNull(rst.Fields("fldIndPracticeNum").Value, ""))
    ElseIf rst.Fields("fldInsType").Value = "BS" Then
            strRef01 = "1A"
            strRef02 = StripDelimiters(IfNull(rst.Fields("fldIndPracticeNum").Value, ""))
    ElseIf Not IsNull(rst.Fields("fldIndPracticeNum").Value) Then  'Commercial Claims
            strRef01 = "G2"
            strRef02 = StripDelimiters(IfNull(rst.Fields("fldIndPracticeNum").Value, ""))
            'strRef01 = "EI"
            'strRef02 = NumbersOnly(rst.Fields("fldTIN").Value)
    Else
            strRef01 = ""
            strRef02 = ""
    End If
        
    'Medicaid of Nebraska
    If rstClrHse.Fields("fldClearingHouseID").Value = 64 Then
        strRef01 = "SY"        'individual SSN
    End If
    'Blue Cross of CA (Anthem) requires License Number 0B
    If rst.Fields("fldInsuranceID").Value = 1270 Then
        strRef01 = "0B"        'License Number 0B
    End If
    
    'Send the Providers SSN for specific Insurance Companies
    If (IfNull(rst.Fields("fldProviderNPI").Value, "") > "") And _
       (rst.Fields("fldInsuranceID").Value = 26 Or _
        rst.Fields("fldInsuranceID").Value = 217 Or _
        rst.Fields("fldInsuranceID").Value = 1270 Or _
        rst.Fields("fldInsuranceID").Value = 1301 Or _
        rst.Fields("fldInsuranceID").Value = 1311 Or _
        rst.Fields("fldPlanID").Value = 6064) Then 'Aetna, Magellan, BcBs NJ and Cigna require EIN/SY for group providers
        'rst.Fields("fldInsuranceID").Value = 1297 Or REMOVED BcBs NJ 12/24/2018
        'rst.Fields("fldInsuranceID").Value = 3 Or REMOVED AETNA 07/15/2014
        If Not IsNull(rst.Fields("fldIndPracticeNum").Value) And Len(StripDelimiters(IfNull(rst.Fields("fldIndPracticeNum").Value, ""))) = 9 Then
            strRef01 = "SY"        'individual SSN SY
            strRef02 = StripDelimiters(IfNull(rst.Fields("fldIndPracticeNum").Value, ""))
        End If
        If strRef02 > "" And strRef02 <> strNM109 Then
            If Not g_bln5010 Or strRef01 = "SY" Then
                strRef = "REF*" & strRef01 & "*" & strRef02 & "~"
                If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
                    rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
                    g_PrtString = g_PrtString + strRef
                    If Len(g_PrtString) >= 80 Then
                        Print #1, Left(g_PrtString, 80)
                        g_PrtString = Mid(g_PrtString, 81)
                    End If
                Else
                    If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
                End If
                g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
                strRef01 = "": strRef02 = ""
            End If
        End If
    End If
    
    strRef = "REF*" & strRef01 & "*" & strRef02 & "~"
      'removed 08/17/2016
'    If g_bln5010 Then    '5010 Beacon Health, CareSource requires the Taxonomy Code to be sent as a secondary id
'        If rst.Fields("fldInsuranceID").Value = 1317 Or _
'           rst.Fields("fldInsuranceID").Value = 1460 Then
'            If rst.Fields("fldUseSuperYN").Value = "Y" Then
'                strRef02 = Trim(StripDelimiters(IfNull(rst.Fields("fldSuperTaxonomyCode").Value, "101Y00000X")))
'            Else
'                strRef02 = Trim(StripDelimiters(IfNull(rst.Fields("fldTaxonomyCode").Value, "101Y00000X")))
'            End If
'            strRef01 = "G2"
'            strRef = "REF*" & strRef01 & "*" & strRef02 & "~"
'            If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
'                rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
'                g_PrtString = g_PrtString + strRef
'                If Len(g_PrtString) >= 80 Then
'                    Print #1, Left(g_PrtString, 80)
'                    g_PrtString = Mid(g_PrtString, 81)
'                End If
'            Else
'                If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
 '           End If
'            g_lngNumSeg = g_lngNumSeg + 1
'        End If
'    End If

    If Not g_bln5010 Then 'Legacy ID's are not sent in 5010
        If strRef02 > "" And strRef01 <> "EI" And strRef02 <> strNM109 Then
            If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
                rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
                g_PrtString = g_PrtString + strRef
                If Len(g_PrtString) >= 80 Then
                    Print #1, Left(g_PrtString, 80)
                    g_PrtString = Mid(g_PrtString, 81)
                End If
            Else
                If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
            End If
            g_lngNumSeg = g_lngNumSeg + 1
        End If
    
      If Not IsNull(rst.Fields("fldIndSecondaryID").Value) Then
        If rstClrHse.Fields("fldClearingHouseID").Value = 33 Then    'BcBs of Georgia
            strRef01 = ""
            strRef02 = ""
        Else
            strRef01 = "LU"
            strRef02 = IfNull(rst.Fields("fldIndSecondaryID").Value, "")
        End If
        If strRef02 > "" Then
            strRef = "REF*" & strRef01 & "*" & strRef02 & "~"
            If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
                rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
                g_PrtString = g_PrtString + strRef
                If Len(g_PrtString) >= 80 Then
                    Print #1, Left(g_PrtString, 80)
                    g_PrtString = Mid(g_PrtString, 81)
                End If
            Else
                If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
            End If
            g_lngNumSeg = g_lngNumSeg + 1
        End If
      End If
    End If
    
    Exit Sub
        
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub
Private Sub WriteLoop2310C(ByVal rst As ADODB.Recordset, ByVal rstClrHse As ADODB.Recordset)
'Service Facility information
   On Error GoTo ErrTrap:

   Dim strNM101, strNM102, strNM103, strNM104, strNM105 As String
   Dim strNM106, strNM107, strNM108, strNM109 As String
   Dim strNM1, strN2, strN3, strN4 As String
   Dim strN401, strN402, strN403, strN404 As String
   Dim strPRV, strPRV01, strPRV02, strPRV03, strPRV04 As String
   Dim strRef, strRef01, strRef02 As String
   Dim FacilityID, FacilityTypeID As String
     
   strNM101 = "FA"                'entity identifier code (FA = facility, LI = independant laboratory)
   If g_bln5010 Then strNM101 = "77"    '5010 Service Lcoation
   strNM102 = "2"                 'entity type qualifier (1 = person, 2 = non-person)
   strNM103 = Trim(Mid(StripDelimiters(IfNull(rst.Fields("fldFacilityLine1").Value, "")), 1, 35))  'Facility Name
   strNM104 = ""                  'first name
   strNM105 = ""                  'middle name
   strNM106 = ""                  'name prefix (not used)
   strNM107 = ""                  'name suffix
   strNM108 = ""                  'identification code qualifier
   strNM109 = ""                     'Facility Tax ID number
   If IfNull(rst.Fields("fldFacilityNPI").Value, "") = "" Then
        If (rst.Fields("fldPOS").Value = "11" And Not g_bln5010) Or rst.Fields("fldPLANID").Value = 4552 Or _
            rst.Fields("fldPayerCode").Value = "52189" Then      '5010 Do not duplicate 2010AA NM109
            strNM108 = "XX"        'NPI
            strNM109 = IfNull(rst.Fields("fldGroupNPI").Value, IfNull(rst.Fields("fldProviderNPI").Value, ""))
        End If
   Else    'Not g_bln5010 Or
        If (rst.Fields("fldFacilityNPI").Value <> IfNull(rst.Fields("fldProviderNPI").Value, "") And _
            rst.Fields("fldFacilityNPI").Value <> IfNull(rst.Fields("fldGroupNPI").Value, "")) Or _
            rst.Fields("fldPLANID").Value = 4552 Or _
            rst.Fields("fldPlanID").Value = 4922 Or _
            rst.Fields("fldPayerCode").Value = "52189" Then    '5010 Do not duplicate 2010AA NM109
            strNM108 = "XX"        'NPI
            strNM109 = Trim(rst.Fields("fldFacilityNPI").Value)
        End If
   End If
       
   If rst.Fields("fldPOS").Value = "12" And rst.Fields("fldShortageAreaYN").Value = "Y" And rst.Fields("fldPlanID").Value <> 4922 Then 'Patient Visit in a Shortage Area at their home.
      strNM102 = "1"          'Qualifier - 2:Non-person entity
      strNM103 = Trim(StripDelimiters(Mid(Replace(IfNull(rst.Fields("fldPatientLastName").Value, ""), "'", ""), 1, 35)))    'Last name
      strNM104 = Trim(StripDelimiters(Mid(Replace(IfNull(rst.Fields("fldPatientFirstName").Value, ""), "'", ""), 1, 35)))   'first name
      strNM105 = IfNull(rst.Fields("fldPatientMI").Value, "")   'middle name
      strNM106 = ""        'not used (name prefix)
      strNM107 = ""        'not used (name suffix)
      strNM108 = ""                  'identification code qualifier
      strNM109 = ""                     'Facility Tax ID number
   End If
   
   If strNM109 > "" Then
        strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
         & "*" & strNM104 & "*" & strNM105 & "*" & strNM106 _
         & "*" & strNM107 & "*" & strNM108 & "*" & strNM109 _
         & "~"
   ElseIf strNM105 > "" Then
        strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
         & "*" & strNM104 & "*" & strNM105 & "~"
   ElseIf strNM104 > "" Then
        strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
         & "*" & strNM104 & "~"
   Else
       strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 & "~"
   End If
     
   If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strNM1
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
   Else
        If g_blnLineFeed Then Print #1, strNM1 Else Print #1, strNM1;
   End If
   g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
   
   'If the Facility name is more than 35 characters then we use segment N2 to hold the
   'overflow characters
    If Len(IfNull(rst.Fields("fldFacilityLine1").Value, "")) > 35 Then
        strN2 = "N2*" & Trim(Mid(rst.Fields("fldFacilityLine1").Value, 36)) & "~"
    Else
        strN2 = ""
    End If
        
    If strN2 > "" Then
    '   If g_blnLineFeed Then Print #1, strN2 Else Print #1, strN2;  'Print overflow line
    '   g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
    End If
      
   strN3 = ""
   If rst.Fields("fldPOS").Value = "12" And rst.Fields("fldShortageAreaYN").Value = "Y" Then 'Patient Visit in a Shortage Area at their home.
      If Not IsNull(rst.Fields("fldPatientStreetNum").Value) Then           'Patient address
         strN3 = "N3*" & Trim(StripDelimiters(IfNull(rst.Fields("fldPatientStreetNum").Value, ""))) & "~"
      End If
   Else
      If IfNull(rst.Fields("fldFacilityLine2").Value, "") > "" Then
         strN3 = "N3*" & Trim(StripDelimiters(rst.Fields("fldFacilityLine2").Value)) & "~"
      End If
   End If

   If Trim(strN3) > "" Then
      If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
         rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
         g_PrtString = g_PrtString + strN3
         If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
         End If
      Else
            If g_blnLineFeed Then Print #1, strN3 Else Print #1, strN3;
      End If
      g_lngNumSeg = g_lngNumSeg + 1
   End If
   
   strN4 = ""
   If rst.Fields("fldPOS").Value = "12" And rst.Fields("fldShortageAreaYN").Value = "Y" Then 'Patient Visit in a Shortage Area at their home.
      strN401 = Trim(Replace(IfNull(StripDelimiters(rst.Fields("fldPatientCity").Value), ""), "'", ""))   'city
      strN402 = Trim(Replace(IfNull(rst.Fields("fldPatientState").Value, ""), "'", ""))  'State code (2 digits)
      strN403 = Trim(NumbersOnly(Replace(IfNull(rst.Fields("fldPatientZip").Value, ""), "'", "")))    'postal code
      If strN402 = "OT" Or strN402 = "ON" Then strN404 = "CA"
      strN4 = "N4*" & strN401 & "*" & strN402 & "*" & strN403 & "~"
      If strN404 > "" Then strN4 = "N4*" & strN401 & "*" & strN402 & "*" & strN403 & "*" & strN404 & "~"
   Else
        strN4 = "N4*" & Trim(rst.Fields("fldFacilityCity").Value) & "*" & UCase(rst.Fields("fldFacilityState").Value) & "*" & Trim(NumbersOnly(Replace(IfNull(rst.Fields("fldFacilityZip").Value, ""), "'", ""))) & "~"
   End If
   If Trim(strN3) > "" Then
      If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
         rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
         g_PrtString = g_PrtString + strN4
         If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
         End If
      Else
            If g_blnLineFeed Then Print #1, strN4 Else Print #1, strN4;
      End If
      g_lngNumSeg = g_lngNumSeg + 1
   End If
      
   If rstClrHse.Fields("fldClearingHouseID").Value = 152 Then 'MAINECARE
      If Not IsNull(rst.Fields("fldIndSecondaryID").Value) Then
        strRef01 = "LU"
        strRef02 = IfNull(rst.Fields("fldIndSecondaryID").Value, "")
      End If
      If strRef02 > "" Then
         strRef = "REF*" & strRef01 & "*" & strRef02 & "~"
         If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
                rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
                g_PrtString = g_PrtString + strRef
                If Len(g_PrtString) >= 80 Then
                    Print #1, Left(g_PrtString, 80)
                    g_PrtString = Mid(g_PrtString, 81)
                End If
         Else
                If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
         End If
         g_lngNumSeg = g_lngNumSeg + 1
      End If
   End If
   Exit Sub
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub

Private Sub WriteLoop2310E(ByVal rst As ADODB.Recordset, ByVal rstClrHse As ADODB.Recordset)
'Supervising provider information

    Dim strNM101, strNM102, strNM103, strNM104, strNM105 As String
    Dim strNM106, strNM107, strNM108, strNM109 As String
    Dim strNM1 As String
    Dim strPRV, strPRV01, strPRV02, strPRV03, strPRV04 As String
    Dim strRef, strRef01, strRef02 As String
    Dim strFirst, strLast As String
    Dim intLength As Integer
    
    On Error GoTo ErrTrap:
    strNM101 = "DQ"                'entity identifier code (DQ = supervising physician)
    'Institutional Claims
    If rst.Fields("fldClaimType").Value = "I" Then strNM101 = "82"   'entity identifier code (82 = rendering provider)
    strNM102 = "1"                 'entity type qualifier (1 = person)
    strNM103 = Trim(StripDelimiters(IfNull(rst.Fields("fldSuperLastName").Value, Mid(Replace(rst.Fields("fldProviderLastName").Value, "'", ""), 1, 35))))     'Last name
    strNM104 = Trim(StripDelimiters(IfNull(rst.Fields("fldSuperFirstName").Value, Mid(Replace(rst.Fields("fldProviderFirstName").Value, "'", ""), 1, 35))))   'first name
    strNM105 = Trim(IfNull(rst.Fields("fldSuperMI").Value, ""))   'middle name
    strNM106 = ""                  'name prefix (not used)
    strNM107 = ""                  'name suffix
    strNM108 = "XX"                'npi
    strNM109 = StripDelimiters(IfNull(rst.Fields("fldSuperNPI").Value, "")) 'Reference number
        
    If strNM109 > "" Then
        strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
         & "*" & strNM104 & "*" & strNM105 & "*" & strNM106 _
         & "*" & strNM107 & "*" & strNM108 & "*" & strNM109 _
         & "~"
    ElseIf strNM105 > "" Then
        strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
         & "*" & strNM104 & "*" & strNM105 & "~"
    Else
        strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
         & "*" & strNM104 & "~"
    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strNM1
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strNM1 Else Print #1, strNM1;
    End If
    g_lngNumSeg = g_lngNumSeg + 1
         
    strPRV01 = "PE"                 'Provider Code (PE-Performing, BI-Billing, PT-Payto, RF-Referring)
    'Institutional Claims
    If rst.Fields("fldClaimType").Value = "I" Then strPRV01 = "AT"  'entity identifier code (AT = attending provider)
    strPRV02 = "ZZ"                 'Reference Identification Qualifier
    If g_bln5010 Then strPRV02 = "PXC"    '5010 uses PXC for the Taxonomy Code
    
    'Specialty information
    strPRV03 = Trim(StripDelimiters(IfNull(rst.Fields("fldSuperTaxonomyCode").Value, "101Y00000X")))         'ZZ relies on Provider Taxonomy Code published by BC/BS Association
    If strPRV03 > "" And Not g_bln5010 Then
        strPRV = "PRV*" & strPRV01 & "*" & strPRV02 & "*" & strPRV03 & "~"
        If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
            g_PrtString = g_PrtString + strPRV
            If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
            End If
        Else
            If g_blnLineFeed Then Print #1, strPRV Else Print #1, strPRV;
        End If
        g_lngNumSeg = g_lngNumSeg + 1
    End If
    
    Exit Sub
        
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub
Private Sub WriteLoop2320(ByVal rst As ADODB.Recordset, ByVal rstClrHse As ADODB.Recordset)
'Writes Other Subscriber Information(Secondary Insurance).
    
    Dim objCheckLog As PostingBz.CCheckLogBZ
    Dim rstCheckLog As ADODB.Recordset
    Dim objTx As PostingBz.CTxBz
    Dim rstTx As ADODB.Recordset
    Dim strSBR, strSBR01, strSBR02, strSBR03, strSBR04 As String
    Dim strSBR05, strSBR06, strSBR07, strSBR08, strSBR09 As String
    Dim strOI, strOI01, strOI02, strOI03, strOI04 As String
    Dim strOI05, strOI06
    Dim strCAS, strCAS01, strCAS02, strCAS03, strCAS04 As String
    Dim strCAS05, strCAS06, strCAS07, strCAS08, strCAS09 As String
    Dim strAmt, strAmt01, strAmt02, strAmt03, strAmt04 As String
    Dim strAMT05, strAMT06, strAMT07, strAMT08, strAMT09 As String
    Dim strDMG As String
    Dim dtDOB As Date
    Dim lngAllowed, lngDeductable, lngDisallowance, lngPayment, lngCoInsurance As Double
    Dim dteCheckDate, dtePostDate As Date
    
    g_Allowed = 0
    dteCheckDate = 0
    g_CheckDate = 0
   
    'SBR (subscriber information)
    
    If rst.Fields("fldOrder").Value = 1 Then strSBR01 = "S"             'payer resp. Secondary (see p. 113)
    If rst.Fields("fldOrder").Value = 2 Then strSBR01 = "P"             'payer resp. Primary (see p. 113)
    If rst.Fields("fldOrder").Value = 3 Then strSBR01 = "P"             'When the claim is a Tertiary, then the Other Ins is primary and Sec is Secondary
    
    'Relationsip to Subscriber
    strSBR02 = GetRelationCode(IfNull(rst.Fields("fldOthInsRelation").Value, ""))
   
    If (IfNull(rst.Fields("fldOthInsPayerCode").Value, "") = "01260") Then  'Magellan DOES NOT WANT THE GROUP NUMBER
        strSBR03 = ""
    ElseIf (IfNull(rst.Fields("fldOthInsPayerCode").Value, "") = "47198") Then
        strSBR03 = StripDelimiters(IfNull(rst.Fields("fldOthInsdGroupNum").Value, "999999")) 'Blue Cross of Ca wasnt 999999 when unknown
    Else
        strSBR03 = StripDelimiters(IfNull(rst.Fields("fldOthInsdGroupNum").Value, "")) 'Subscriber group or policy number
    End If
    strSBR04 = StripDelimiters(IfNull(rst.Fields("fldOthPlanName").Value, ""))              'insured group name

    If g_bln5010 And rst.Fields("fldOthInsuranceID").Value = 24 Then strSBR03 = ""
    If g_bln5010 And strSBR03 = "NONE" Then strSBR03 = ""   '5010 when SBR03 is used then SBR04 cannot be used.
    If g_bln5010 And (strSBR03 > "" Or _
        rst.Fields("fldOthInsType").Value = "MP" Or _
        rst.Fields("fldOthInsType").Value = "MB" Or _
        rst.Fields("fldOthInsType").Value = "MC") Then strSBR03 = "": strSBR04 = ""

   If rst.Fields("fldOthInsType").Value = "MP" Then
        strSBR05 = "MB"
   ElseIf rst.Fields("fldOthInsType").Value = "MB" Then strSBR05 = "MB"    'Medicare Primary or 'MB' Medicare Part B
   ElseIf rst.Fields("fldOthInsType").Value = "MC" Then strSBR05 = "MC"    'Medicaid
   ElseIf rst.Fields("fldOthInsType").Value = "BL" Then strSBR05 = "C1"    'Blue Cross
   ElseIf rst.Fields("fldOthInsType").Value = "CH" Then strSBR05 = "C1"    'Tricare
   ElseIf rst.Fields("fldOthInsType").Value = "WC" Then strSBR05 = "C1"    'Workmans Comp
   ElseIf rst.Fields("fldOthInsType").Value = "MI" Then strSBR05 = "MI"    'Medigap
   Else
      'Commercial Claims
      strSBR05 = "C1"
   End If
   
   'Insurance Type
   'TODO: insurance type code applies only to Medicare where Medicare is secondary payer
   If IfNull(rst.Fields("fldOthInsMedigapPayerCode").Value, "") > "" Then
      strSBR05 = "MI"            'Medigap
  '   strSBR05 = "SP"            'Supplamental Policy
   End If
    
   If g_bln5010 Then strSBR05 = ""
   'TODO: insurance type code applies only to Medicare where Medicare is secondary payer
   If rst.Fields("fldInsuranceID").Value <> 24 And (rst.Fields("fldOthInsuranceID").Value = 24 Or rst.Fields("fldOthInsuranceID").Value = 452) And rst.Fields("fldOrder").Value = 1 Then
        strSBR05 = "47"   'Medicare is Secondary, other insurance is primary
        If Trim(rst.Fields("fldMSPcode").Value) > "" Then strSBR05 = rst.Fields("fldMSPcode").Value
   End If
    
   strSBR06 = ""              'not used
   strSBR07 = ""              'not used
   strSBR08 = ""              'not used
    
   strSBR09 = GetClaimFilingCode(IfNull(rst.Fields("fldOthInsType").Value, ""))
   If rst.Fields("fldPlanID").Value = 3090 Then strSBR09 = "16"   ' Missouri Medicaid
       
   strSBR = "SBR*" & strSBR01 & "*" & strSBR02 & "*" & strSBR03 _
         & "*" & strSBR04 & "*" & strSBR05 & "*" & strSBR06 _
         & "*" & strSBR07 & "*" & strSBR08 & "*" & strSBR09 & "~"
       
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
        rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strSBR
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strSBR Else Print #1, strSBR;
    End If
    g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
   
   'Primary Insurance Has paid, send Payment Information to Secondary Insurance
    If rst.Fields("fldOrder").Value >= 2 Then
        'Fetch Check Transaction record from DB
  '      Set objCheckLog = CreateObject("PostingBz.CCheckLogBz")
  '      Set rstCheckLog = objCheckLog.Fetch(False, "(A.fldPatientID = " & CLng(rst.Fields("fldPatientID").Value) & ") AND " & _
  '                      "(A.fldEncounterLogID = " & CLng(rst.Fields("fldEncounterLogID").Value) & ") AND " & _
  '                      "(A.fldInsuranceID = " & CLng(rst.Fields("fldOthInsuranceID").Value) & ") AND " & _
  '                      "(A.fldAllowed > 0 or A.fldPayment > 0 or A.fldDisallowance > 0) AND (A.fldDenTxTypeID <> 313)")
  '      Set objCheckLog = Nothing
        lngAllowed = 0: lngDeductable = 0: lngDisallowance = 0: lngPayment = 0: lngCoInsurance = 0: g_Allowed = 0
  '      If rstCheckLog.RecordCount > 0 Then
  '       Do While Not rstCheckLog.EOF
  '          lngPayment = lngPayment + IfNull(rstCheckLog.Fields("fldPayment").Value, 0)
  '          lngDeductable = lngDeductable + IfNull(rstCheckLog.Fields("fldDeductable").Value, 0)
  '          lngCoInsurance = lngCoInsurance + IfNull(rstCheckLog.Fields("fldCoInsurance").Value, 0)
  '          lngDisallowance = lngDisallowance + IfNull(rstCheckLog.Fields("fldDisallowance").Value, 0)
  '          g_Allowed = g_Allowed + IfNull(rstCheckLog.Fields("fldAllowed").Value, 0)
  '          g_Allowed = IIf(g_Allowed = 0, CDbl(lngPayment) + CDbl(lngDeductable) + CDbl(lngCoInsurance), g_Allowed)
  '          g_CheckDate = IfNull(rstCheckLog.Fields("fldCheckDate").Value, 0)
  '          rstCheckLog.MoveNext
  '       Loop
  '       g_Allowed = Round(IfNull(rst.Fields("fldFee").Value, 0) * IfNull(rst.Fields("fldUnits").Value, 0) - CDbl(lngDisallowance), 2)
  '       If rst.Fields("fldAddOnCPTCode").Value > "" Then
  '          g_Allowed = Round(rst.Fields("fldFee").Value * rst.Fields("fldUnits").Value + rst.Fields("fldAddOnFee").Value * IIf(rst.Fields("fldAddOnUnits").Value < 1, 1, rst.Fields("fldAddOnUnits").Value) - CDbl(lngDisallowance), 2)  'Monetary amount PG 402
  '          If rst.Fields("fldAddOnSecCPTCode").Value > "" Then
  '             g_Allowed = Round(rst.Fields("fldFee").Value * rst.Fields("fldUnits").Value + rst.Fields("fldAddOnFee").Value * IIf(rst.Fields("fldAddOnUnits").Value < 1, 1, rst.Fields("fldAddOnUnits").Value) + rst.Fields("fldAddOnSecFee").Value * IIf(rst.Fields("fldAddOnSecUnits").Value < 1, 1, rst.Fields("fldAddOnSecUnits").Value) - CDbl(lngDisallowance), 2)  'Monetary amount PG 402
  '          End If
  '       End If
  '      Else
            Set objTx = CreateObject("PostingBz.CTxBz")
            Set rstTx = objTx.Fetch(CLng(rst.Fields("fldEncounterLogID").Value), 4)    ''Need to read according to the Database
            Set objTx = Nothing
        
            If rstTx.RecordCount > 0 Then
                Do While Not rstTx.EOF
                  If rstTx.Fields("fldInsuranceID").Value = rst.Fields("fldOthInsuranceID").Value Then
                    If rstTx.Fields("fldCheckID").Value > 0 And Not IsNull(rstTx.Fields("fldCheckDate").Value) And dteCheckDate = 0 Then dteCheckDate = rstTx.Fields("fldCheckDate").Value
                    If rstTx.Fields("fldTxTypeID").Value = 1 Then
                       g_Allowed = g_Allowed + Round(IfNull(rstTx.Fields("fldAmount").Value, 0), 2)
                    End If
                    If rstTx.Fields("fldTxTypeID").Value = 87 Then
                       lngDeductable = lngDeductable + Round(IfNull(rstTx.Fields("fldAmount").Value, 0), 2)
                       dteCheckDate = IfNull(rstTx.Fields("fldCheckDate").Value, rstTx.Fields("fldPostDate").Value)
                    End If
                    If rstTx.Fields("fldTxTypeID").Value = 7 Then
                       lngDisallowance = lngDisallowance + Round(IfNull(rstTx.Fields("fldAmount").Value, 0), 2)
                       dteCheckDate = IfNull(rstTx.Fields("fldCheckDate").Value, rstTx.Fields("fldPostDate").Value)
                    End If
                    If rstTx.Fields("fldTxTypeID").Value = 10 Or _
                       rstTx.Fields("fldTxTypeID").Value = 11 Or _
                       rstTx.Fields("fldTxTypeID").Value = 69 Or _
                       rstTx.Fields("fldTxTypeID").Value = 105 Or _
                       rstTx.Fields("fldTxTypeID").Value = 106 Or _
                       rstTx.Fields("fldTxTypeID").Value = 120 And _
                       Round(IfNull(rstTx.Fields("fldAmount").Value, 0), 2) <> 0 Then
                       lngPayment = CDbl(lngPayment) + Round(IfNull(rstTx.Fields("fldAmount").Value, 0), 2)
                       dteCheckDate = IfNull(rstTx.Fields("fldCheckDate").Value, rstTx.Fields("fldPostDate").Value)
                    End If
                    lngAllowed = Round(IfNull(rstTx.Fields("fldFee").Value, 0) * IfNull(rstTx.Fields("fldUnits").Value, 0), 2)
                    If rstTx.Fields("fldAddOnCPTCode").Value > "" Then
                        lngAllowed = Round(rstTx.Fields("fldFee").Value * rstTx.Fields("fldUnits").Value + rstTx.Fields("fldAddOnFee").Value * IIf(rstTx.Fields("fldAddOnUnits").Value < 1, 1, rstTx.Fields("fldAddOnUnits").Value), 2)  'Monetary amount PG 402
                        If rstTx.Fields("fldAddOnSecCPTCode").Value > "" Then
                            lngAllowed = Round(rstTx.Fields("fldFee").Value * rstTx.Fields("fldUnits").Value + rstTx.Fields("fldAddOnFee").Value * IIf(rstTx.Fields("fldAddOnUnits").Value < 1, 1, rstTx.Fields("fldAddOnUnits").Value) + rstTx.Fields("fldAddOnSecFee").Value * IIf(rstTx.Fields("fldAddOnSecUnits").Value < 1, 1, rstTx.Fields("fldAddOnSecUnits").Value), 2)  'Monetary amount PG 402
                        End If
                    End If
                  End If
                  rstTx.MoveNext
                Loop
                Set rstTx = Nothing
                If g_Allowed <= 0 Then g_Allowed = lngAllowed
                g_Allowed = Round(g_Allowed - CDbl(lngDisallowance), 2)
                If CDbl(lngDeductable) > 0 And CDbl(lngDeductable) > g_Allowed Then lngDeductable = g_Allowed - CDbl(lngPayment)
                lngCoInsurance = CDbl(g_Allowed) - CDbl(lngPayment) - CDbl(lngDeductable)
                g_CheckDate = dteCheckDate
            End If
  '      End If
        
  '      If lngPayment > 0 Then  'Print a $0.00 Payment record
            strAmt01 = "D"
            strAmt02 = Format(IfNull(lngPayment, 0), "#####.00")
            strAmt = "AMT*" & strAmt01 & "*" & strAmt02 & "~"
            If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
                rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
                g_PrtString = g_PrtString + strAmt
                If Len(g_PrtString) >= 80 Then
                    Print #1, Left(g_PrtString, 80)
                    g_PrtString = Mid(g_PrtString, 81)
                End If
            Else
                If g_blnLineFeed Then Print #1, strAmt Else Print #1, strAmt;
            End If
            g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
  '      End If
        
        If lngDeductable + lngCoInsurance + IfNull(lngPayment, 0) <> IfNull(g_Allowed, 0) Then
            lngCoInsurance = g_Allowed - lngPayment - lngDeductable
        End If
        If lngDeductable + lngCoInsurance > 0 Then
            strAmt01 = "F2"
            If g_bln5010 Then strAmt01 = "EAF"     'Patient Liability
            strAmt02 = Format(Round(CStr(CDbl(lngDeductable) + CDbl(lngCoInsurance)), 2), "#####.00")
            strAmt = "AMT*" & strAmt01 & "*" & strAmt02 & "~"
            If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
                rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
                g_PrtString = g_PrtString + strAmt
                If Len(g_PrtString) >= 80 Then
                    Print #1, Left(g_PrtString, 80)
                    g_PrtString = Mid(g_PrtString, 81)
                End If
            Else
                If g_blnLineFeed Then Print #1, strAmt Else Print #1, strAmt;
            End If
            g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
        End If
            
        If IfNull(g_Allowed, 0) > 0 And Not g_bln5010 Then
            strAmt01 = "B6"
            strAmt02 = Format(IfNull(g_Allowed, 0), "#####.00")
            strAmt = "AMT*" & strAmt01 & "*" & strAmt02 & "~"
            If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
                rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT+
                g_PrtString = g_PrtString + strAmt
                If Len(g_PrtString) >= 80 Then
                    Print #1, Left(g_PrtString, 80)
                    g_PrtString = Mid(g_PrtString, 81)
                End If
            Else
                If g_blnLineFeed Then Print #1, strAmt Else Print #1, strAmt;
            End If
            g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
        End If
        
    End If
   
   'Subscriber demographics
    If IsNull(rst.Fields("fldOthInsdDOB").Value) Then
        dtDOB = 0
    Else
        dtDOB = rst.Fields("fldOthInsdDOB").Value
    End If
    
    'If the DOB is not known, then this line must be ignored otherwise errors occur
    If (IsDate(dtDOB)) And (dtDOB > 0) And (Not g_bln5010) Then    '5010 Does not want the Insured DOB
        strDMG = "DMG*D8*" & Year(dtDOB) & Format(Month(dtDOB), "00") & Format(Day(dtDOB), "00") & "*"
        strDMG = strDMG & IfNull(rst.Fields("fldOthInsdSex").Value, "U") & "~"
        If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
            g_PrtString = g_PrtString + strDMG
            If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
            End If
        Else
            If g_blnLineFeed Then Print #1, strDMG Else Print #1, strDMG;
        End If
        g_lngNumSeg = g_lngNumSeg + 1
    End If
   
   'OI (Other Insurance information)
    strOI01 = ""              'not used
    strOI02 = ""              'not used
    strOI03 = "Y"             'Assignment of Benefits fldAssignmentYN
    If Not IsNull(rst.Fields("fldAssignmentYN").Value) Then strOI03 = rst.Fields("fldAssignmentYN").Value
    strOI04 = "B"             'Signature Source
    If g_bln5010 Then strOI04 = "P"    '5010 Provider is the source of the Signature
    strOI05 = ""              'not used
    strOI06 = "Y"             'Release of Patient Information
      
    strOI = "OI*" & strOI01 & "*" & strOI02 & "*" & strOI03 _
         & "*" & strOI04 & "*" & strOI05 & "*" & strOI06 & "~"
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strOI
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strOI Else Print #1, strOI;
    End If
    g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
   
End Sub
Private Function WriteLoop2330(ByVal rst As ADODB.Recordset, ByVal rstClrHse As ADODB.Recordset) As Boolean
'Writes Subscriber information
    On Error GoTo ErrTrap:
    
    Dim strNM101, strNM102, strNM103, strNM104, strNM105 As String
    Dim strNM106, strNM107, strNM108, strNM109, strNM110, strNM111 As String
    Dim strNM1, strN2, strN3, strN4 As String
    Dim strN301, strN302, strN303, strN304, strN305 As String
    Dim strN401, strN402, strN403, strN404 As String
    Dim strRef As String
    
    strNM101 = "IL"                       'entity identifier code (IL = insured)
    strNM102 = "1"                        'entity type qualifier (1=person, 2=nonperson)
   
    strNM103 = Trim(Mid(Replace(IfNull(rst.Fields("fldOthInsdLastName").Value, ""), "'", ""), 1, 35))    'Last name
    strNM104 = Trim(Mid(Replace(IfNull(rst.Fields("fldOthInsdFisrtName").Value, ""), "'", ""), 1, 35))   'first name
    strNM105 = IfNull(rst.Fields("fldOthInsdMI").Value, "")   'middle name
    strNM106 = ""        'not used (name prefix)
    strNM107 = ""        'not used (name suffix)
    strNM108 = "MI"      'identification code qualifier
    'Subscribers Identification number as assigned by the payer.
    strNM109 = Trim(Replace(StripDelimiters(IIf(rst.Fields("fldOthInsdCardNum").Value > "", rst.Fields("fldOthInsdCardNum").Value, "00")), " ", "", 1))

    strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
         & "*" & strNM104 & "*" & strNM105 & "*" & strNM106 _
         & "*" & strNM107 & "*" & strNM108 & "*" & strNM109 _
         & "~"
 
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strNM1
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strNM1 Else Print #1, strNM1;
    End If
    g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
    
   'Subscriber address
    If IsNull(rst.Fields("fldInsuredStreetNum").Value) Then
        strN301 = Trim(StripDelimiters(IfNull(rst.Fields("fldPatientStreetNum").Value, "")))
        strN401 = Trim(StripDelimiters(IfNull(rst.Fields("fldPatientCity").Value, "")))   'city
        strN402 = Trim(Replace(IfNull(rst.Fields("fldPatientState").Value, ""), "'", ""))  'State code (2 digits)
        strN403 = Trim(NumbersOnly(Replace(IfNull(rst.Fields("fldPatientZip").Value, ""), "'", "")))    'postal code
        If strN402 = "OT" Or strN402 = "ON" Then strN404 = "CA"
    Else
        strN301 = Trim(StripDelimiters(IfNull(rst.Fields("fldInsuredStreetNum").Value, "")))
        strN401 = Trim(StripDelimiters(IfNull(rst.Fields("fldInsuredCity").Value, "")))   'city
        strN402 = Trim(Replace(IfNull(rst.Fields("fldInsuredState").Value, ""), "'", ""))  'State code (2 digits)
        strN403 = Trim(NumbersOnly(Replace(IfNull(rst.Fields("fldInsuredZip").Value, ""), "'", "")))
        If strN402 = "OT" Or strN402 = "ON" Then strN404 = "CA"
    End If
    If Not IsNull(strN301) Then
        'Patient address
        strN3 = "N3*" & strN301 & "~"
        If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
            g_PrtString = g_PrtString + strN3
            If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
            End If
        Else
            If g_blnLineFeed Then Print #1, strN3 Else Print #1, strN3;
        End If
        g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters

        strN4 = "N4*" & strN401 & "*" & strN402 & "*" & strN403 & "~"
        If strN404 > "" Then strN4 = "N4*" & strN401 & "*" & strN402 & "*" & strN403 & "*" & strN404 & "~"
        If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
            g_PrtString = g_PrtString + strN4
            If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
            End If
        Else
            If g_blnLineFeed Then Print #1, strN4 Else Print #1, strN4;
        End If
        g_lngNumSeg = g_lngNumSeg + 1
    End If
        
   'Secondary number to identify subscriber
    If rst.Fields("fldOthInsType").Value <> "MP" And rst.Fields("fldOthInsType").Value <> "MB" Then  'Medicare
        If Not IsNull(rst.Fields("fldInsuredSSN").Value) And _
            IfNull(rst.Fields("fldInsuredSSN").Value, "") > "" And _
            IfNull(rst.Fields("fldInsuredSSN").Value, "") <> strNM109 And _
            IfNull(rst.Fields("fldInsuredSSN").Value, "") <> "000000000" And _
            IfNull(rst.Fields("fldInsuredSSN").Value, "") <> "111111111" And _
            IfNull(rst.Fields("fldInsuredSSN").Value, "") <> "999999999" Then
            strRef = "REF*SY*" & Trim(StripDelimiters(rst.Fields("fldInsuredSSN").Value)) & "~"   '1G Insured Policy ID
            If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
                rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
                g_PrtString = g_PrtString + strRef
                If Len(g_PrtString) >= 80 Then
                    Print #1, Left(g_PrtString, 80)
                    g_PrtString = Mid(g_PrtString, 81)
                End If
            Else
                If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
            End If
            g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
        ElseIf Not IsNull(rst.Fields("fldPatientSSN").Value) And _
            IfNull(rst.Fields("fldPatientSSN").Value, "") > "" And _
            IfNull(rst.Fields("fldPatientSSN").Value, "") <> strNM109 And _
            IfNull(rst.Fields("fldPatientSSN").Value, "") <> "000000000" And _
            IfNull(rst.Fields("fldPatientSSN").Value, "") <> "111111111" And _
            IfNull(rst.Fields("fldPatientSSN").Value, "") <> "999999999" Then
            If GetRelationCode(IfNull(rst.Fields("fldOthInsRelation").Value, "")) = "18" Then
                strRef = "REF*SY*" & Trim(StripDelimiters(rst.Fields("fldPatientSSN").Value)) & "~"   'SY Insured SSN ID
                If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
                    rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
                    g_PrtString = g_PrtString + strRef
                    If Len(g_PrtString) >= 80 Then
                        Print #1, Left(g_PrtString, 80)
                        g_PrtString = Mid(g_PrtString, 81)
                    End If
                Else
                    If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
                End If
                g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
            End If
        End If
    End If
        
   Exit Function
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Private Function WriteLoop2330B(ByVal rst As Recordset, ByVal rstClrHse As ADODB.Recordset) As Boolean
'Writes Receiver information

Dim objCheckLog As PostingBz.CCheckLogBZ
Dim rstCheckLog As ADODB.Recordset
Dim objTx As PostingBz.CTxBz
Dim rstTx As ADODB.Recordset
    
Dim strNM101, strNM102, strNM103, strNM104, strNM105 As String
Dim strNM106, strNM107, strNM108, strNM109, strNM110, strNM111 As String
Dim strNM1, strN2, strN3, strN4 As String
Dim strN401, strN402, strN403, strN404 As String
Dim strDMG As String
Dim strDTP, strDTP_01, strDTP_02, strDTP_03 As String
Dim strSBR, strSBR01, strSBR02, strSBR03, strSBR04 As String
Dim strSBR05, strSBR06, strSBR07, strSBR08, strSBR09 As String
Dim strOI, strOI01, strOI02, strOI03, strOI04 As String
Dim strOI05, strOI06
Dim strCAS, strCAS01, strCAS02, strCAS03, strCAS04 As String
Dim strCAS05, strCAS06, strCAS07, strCAS08, strCAS09 As String
Dim strAmt, strAmt01, strAmt02, strAmt03, strAmt04 As String
Dim strAMT05, strAMT06, strAMT07, strAMT08, strAMT09 As String
Dim strICN As String
Dim strRef, strRef01, strRef02 As String
Dim dtDOB As Date
Dim lngDeductable, lngDisallowance, lngPayment, lngCoInsurance As Double
Dim dteCheckDate, dtePostDate As Date
    
On Error GoTo ErrTrap:
    
strNM101 = "PR"                        'entity ID code (PR = payer)
strNM102 = "2"                         'entity type qualifier (2 = non-person)
strNM103 = Trim(Mid(Replace(IfNull(rst.Fields("fldOthInsName").Value, ""), "'", ""), 1, 35)) 'last name or organization
strNM104 = ""                          'first name (optional)
strNM105 = ""                          'middle name (optional)
strNM106 = ""                          'name prefix (optional)
strNM107 = ""                          'name suffix (optional)
strNM108 = "PI"                        'ID code qualifier (46 = ETIN, PI = Payor Identification)
    
'Medigap PayerCode
If IfNull(rst.Fields("fldOthInsMedigapPayerCode").Value, "") > "" Then
        strNM109 = rst.Fields("fldOthInsMedigapPayerCode").Value
Else
   If rstClrHse.Fields("fldReceiverPrefixYN").Value = "Y" Then
      strNM109 = CreatePayerCodePrefix(rst.Fields("fldOthInsType").Value, IIf(rst.Fields("fldOthInsPayerCode").Value > "", rst.Fields("fldOthInsPayerCode").Value, "999999")) & IIf(rst.Fields("fldOthInsPayerCode").Value > "", rst.Fields("fldOthInsPayerCode").Value, "999999")
   Else
      strNM109 = IIf(rst.Fields("fldOthInsPayerCode").Value > "", IIf(Len(rst.Fields("fldOthInsPayerCode").Value) < 5, Format(rst.Fields("fldOthInsPayerCode").Value, "00000"), rst.Fields("fldOthInsPayerCode").Value), "99999")
   End If
End If
  
   '    Carrier Code
   '     5508 Medicaid of New Hamshire
   '     2135 Medicaid of Rode Island
   '     2206 MediCal of California
   '     4420 MediCal of Conneticut
If rst.Fields("fldOrder").Value = 2 And _
      (rst.Fields("fldPlanID").Value = 2135 Or _
       rst.Fields("fldPlanID").Value = 2206 Or _
       rst.Fields("fldPlanID").Value = 4420 Or _
       rst.Fields("fldPlanID").Value = 5508) Then
        If IsNumeric(rst.Fields("fldOthPlanName").Value) Then strNM109 = rst.Fields("fldOthPlanName").Value
End If

If rstClrHse.Fields("fldClearingHouseID").Value = 150 And strNM109 = "PSYQUEL" Then strNM109 = "SX173"
If rstClrHse.Fields("fldClearingHouseID").Value = 150 And strNM109 = "00710" Then strNM109 = "MIBLS"
If rstClrHse.Fields("fldClearingHouseID").Value = 150 And strNM109 = "15460" Then strNM109 = "95462"
If rstClrHse.Fields("fldClearingHouseID").Value = 150 And strNM109 = "88848" Then strNM109 = "IABLS"
If rstClrHse.Fields("fldClearingHouseID").Value = 150 And strNM109 = "00624" Then strNM109 = "47198"
If rstClrHse.Fields("fldClearingHouseID").Value = 150 And strNM109 = "990040115" Then strNM109 = "SB971"
If rstClrHse.Fields("fldClearingHouseID").Value = 150 And strNM109 = "0027000" Then strNM109 = "00200"
If rstClrHse.Fields("fldClearingHouseID").Value = 150 And strNM109 = "0085000" Then strNM109 = "14212"
If rstClrHse.Fields("fldClearingHouseID").Value = 150 And rst.Fields("fldPlanID").Value = 1255 And strNM109 = "87726" Then strNM109 = "55555555"
If rstClrHse.Fields("fldClearingHouseID").Value = 150 And rst.Fields("fldPlanID").Value = 1255 And rst.Fields("fldOthInsuranceID").Value = 24 Then strNM109 = "44444444"
If rstClrHse.Fields("fldClearingHouseID").Value = 150 And rst.Fields("fldPlanID").Value <> 3817 And rst.Fields("fldPlanID").Value <> 2899 And rst.Fields("fldPlanID").Value <> 2125 And strNM109 = "620" Then strNM109 = "11202"
If rstClrHse.Fields("fldClearingHouseID").Value = 150 And rst.Fields("fldPlanID").Value <> 3817 And rst.Fields("fldPlanID").Value <> 2899 And rst.Fields("fldPlanID").Value <> 2125 And strNM109 = "401" Then strNM109 = "00401"
If rstClrHse.Fields("fldClearingHouseID").Value = 150 And strNM109 = "0088" Then strNM109 = "14312"
If rst.Fields("fldPatientID").Value = 942501 Then strNM109 = "X0C"
If rst.Fields("fldPatientID").Value = 1062234 Then strNM109 = "X0M"
           
strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
        & "*" & strNM104 & "*" & strNM105 & "*" & strNM106 & "*" & strNM107 _
        & "*" & strNM108 & "*" & strNM109 & "~"

If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
   rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
   g_PrtString = g_PrtString + strNM1
   If Len(g_PrtString) >= 80 Then
      Print #1, Left(g_PrtString, 80)
      g_PrtString = Mid(g_PrtString, 81)
   End If
Else
   If g_blnLineFeed Then Print #1, strNM1 Else Print #1, strNM1;
End If
g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
        
               
    'Claim Check or Remittance Date   ---- Not everyone uses this.
  '  If g_CheckDate > 0 Then
  '      strDTP_01 = "573"                'date/time qualifier
  '      strDTP_02 = "D8"                 'date time period format qualifier
  '      strDTP_03 = Format(g_CheckDate, "yyyymmdd")              'date/time period in CCYYMMDD
  '
  '      strDTP = "DTP*" & strDTP_01 & "*" & strDTP_02 & "*" & strDTP_03 & "~"
  '      If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
  '          rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
  '          g_PrtString = g_PrtString + strDTP
  '          If Len(g_PrtString) >= 80 Then
  '              Print #1, Left(g_PrtString, 80)
  '              g_PrtString = Mid(g_PrtString, 81)
  '          End If
  '      Else
  '          If g_blnLineFeed Then Print #1, strDTP Else Print #1, strDTP;
  '      End If
  '  g_lngNumSeg = g_lngNumSeg + 1
  '  End If
    
'If (rst.Fields("fldPatientID").Value = 774730) And _
'   rst.Fields("fldPlanID").Value = 1315 And rst.Fields("fldOrder").Value = 3 Then
If rst.Fields("fldOrder").Value = 3 Then
        
      'SBR (subscriber information) Build Secondary Payer Info
    
      If rst.Fields("fldOrder").Value = 3 Then strSBR01 = "S"             'payer resp. Secondary (see p. 113)
      strSBR02 = GetRelationCode(IfNull(rst.Fields("fldOthInsRelation").Value, ""))
   
      If (IfNull(rst.Fields("fldSecInsPayerCode").Value, "") = "01260") Then  'Magellan DOES NOT WANT THE GROUP NUMBER
        strSBR03 = ""
      ElseIf (IfNull(rst.Fields("fldSecInsPayerCode").Value, "") = "47198") Then
        strSBR03 = StripDelimiters(IfNull(rst.Fields("fldSecInsdGroupNum").Value, "999999")) 'Blue Cross of Ca wasnt 999999 when unknown
      Else
        strSBR03 = StripDelimiters(IfNull(rst.Fields("fldSecInsdGroupNum").Value, IIf(rst.Fields("fldOthInsType").Value = "MB", "NONE", IIf(rst.Fields("fldOthInsType").Value = "MP", "NONE", "")))) 'Subscriber group or policy number
      End If
      strSBR04 = StripDelimiters(IfNull(rst.Fields("fldSecPlanName").Value, ""))              'insured group name

      If g_bln5010 And strSBR03 = "NONE" Then strSBR03 = ""   '5010 when SBR03 is used then SBR04 cannot be used.
      If g_bln5010 And (strSBR03 > "" Or _
        rst.Fields("fldSecInsType").Value = "MP" Or _
        rst.Fields("fldSecInsType").Value = "MB" Or _
        rst.Fields("fldSecInsType").Value = "MC") Then strSBR03 = "": strSBR04 = "MEDICARE"
    
      If rst.Fields("fldSecInsType").Value = "MP" Then
        strSBR05 = "MB"
      ElseIf rst.Fields("fldSecInsType").Value = "MB" Then strSBR05 = "MB"    'Medicare Primary or 'MB' Medicare Part B
      ElseIf rst.Fields("fldSecInsType").Value = "MC" Then strSBR05 = "MC"    'Medicaid
      ElseIf rst.Fields("fldSecInsType").Value = "BL" Then strSBR05 = "C1"    'Blue Cross
      ElseIf rst.Fields("fldSecInsType").Value = "CH" Then strSBR05 = "C1"    'Tricare
      ElseIf rst.Fields("fldSecInsType").Value = "WC" Then strSBR05 = "C1"    'Workmans Comp
      ElseIf rst.Fields("fldSecInsType").Value = "MI" Then strSBR05 = "MI"    'Medigap
      Else
      'Commercial Claims
         strSBR05 = "C1"
      End If
      If IfNull(rst.Fields("fldSecInsMedigapPayerCode").Value, "") > "" Then
         strSBR05 = "MI"            'Medigap
  '      strSBR05 = "SP"            'Supplamental Policy
      End If
    
      If g_bln5010 Then strSBR05 = ""
      'Insurance Type
      'Insurance type code applies only to Medicare where Medicare is secondary payer
'12 = Working Aged
'13 = End Stage Renal Disease
'14 = Automobile/No-Fault
'15 = Workers' Compensation
'16 = Federal
'41 = Black Lung
'42 = Veterans
'43 = Disability
'47 = Medicare is Secondary, other insurance is primary
      If (rst.Fields("fldSecInsuranceID").Value = 24 Or rst.Fields("fldSecInsuranceID").Value = 452) And rst.Fields("fldOrder").Value = 3 Then
         strSBR05 = "47"        'Medicare is Secondary, other insurance is primary
         If Trim(rst.Fields("fldMSPcode").Value) > "" Then strSBR05 = rst.Fields("fldMSPcode").Value
      End If
      
      strSBR06 = ""              'not used
      strSBR07 = ""              'not used
      strSBR08 = ""              'not used
    
      strSBR09 = GetClaimFilingCode(IfNull(rst.Fields("fldSecInsType").Value, ""))
      If rst.Fields("fldPlanID").Value = 1315 And strSBR09 = "MB" Then strSBR09 = "CI" 'Masshealth does not accept Medicare Secondaries
       
      strSBR = "SBR*" & strSBR01 & "*" & strSBR02 & "*" & strSBR03 _
         & "*" & strSBR04 & "*" & strSBR05 & "*" & strSBR06 _
         & "*" & strSBR07 & "*" & strSBR08 & "*" & strSBR09 & "~"
       
      If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
        rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strSBR
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
      Else
        If g_blnLineFeed Then Print #1, strSBR Else Print #1, strSBR;
      End If
      g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
        
      '  strSBR = "SBR*S*18**NATIONAL GOVERNMENT SERVICES, INC.*MB****MB~"
      '  If rst.Fields("fldPatientID").Value = 155572 Then strSBR = "SBR*S*18**GROUP HEALTH INC*C1****CI~"
      '  If rst.Fields("fldPatientID").Value = 159618 Then strSBR = "SBR*S*18**GROUP HEALTH INC*C1****CI~"
      '  If rst.Fields("fldPatientID").Value = 165620 Then strSBR = "SBR*S*18**GROUP HEALTH INC*C1****CI~"
      '  If rst.Fields("fldPatientID").Value = 169233 Then strSBR = "SBR*S*18**GROUP HEALTH INC*C1****CI~"
      '  If g_blnLineFeed Then Print #1, strSBR Else Print #1, strSBR;
      '  g_lngNumSeg = g_lngNumSeg + 1
        'Need the amount that was disallowed
        'Need to get the amount the secondary payer paid
        
      'Secondary Insurance Has paid, send Payment Information to Tertiary Insurance
      If rst.Fields("fldOrder").Value = 3 Then
 '       'Fetch Check Transaction record from DB
 '        Set objCheckLog = CreateObject("PostingBz.CCheckLogBz")
 '        Set rstCheckLog = objCheckLog.Fetch(False, "(A.fldPatientID = " & CLng(rst.Fields("fldPatientID").Value) & ") AND " & _
 '                       "(A.fldEncounterLogID = " & CLng(rst.Fields("fldEncounterLogID").Value) & ") AND " & _
 '                       "(A.fldInsuranceID = " & CLng(rst.Fields("fldSecInsuranceID").Value) & ") AND " & _
 '                       "(A.fldAllowed > 0 or A.fldPayment > 0 or A.fldDisallowance > 0) AND (A.fldDenTxTypeID <> 313)")
 '        Set objCheckLog = Nothing
         lngDeductable = 0: lngDisallowance = 0: lngPayment = 0: lngCoInsurance = 0: g_Allowed = 0
 '        If rstCheckLog.RecordCount > 0 Then
 '           Do While Not rstCheckLog.EOF
 '              lngPayment = lngPayment + IfNull(rstCheckLog.Fields("fldPayment").Value, 0)
 '              lngDeductable = lngDeductable + IfNull(rstCheckLog.Fields("fldDeductable").Value, 0)
 '              lngCoInsurance = lngCoInsurance + IfNull(rstCheckLog.Fields("fldCoInsurance").Value, 0)
 '              lngDisallowance = lngDisallowance + IfNull(rstCheckLog.Fields("fldDisallowance").Value, 0)
 '              g_Allowed = g_Allowed + IfNull(rstCheckLog.Fields("fldAllowed").Value, 0)
 '              g_Allowed = IIf(g_Allowed = 0, CDbl(lngPayment) + CDbl(lngDeductable) + CDbl(lngCoInsurance), g_Allowed)
 '              g_CheckDate = IfNull(rstCheckLog.Fields("fldCheckDate").Value, 0)
 '              rstCheckLog.MoveNext
 '           Loop
 '           g_Allowed = Round(IfNull(rst.Fields("fldFee").Value, 0) * IfNull(rst.Fields("fldUnits").Value, 0) - CDbl(lngDisallowance), 2)
 '           If rst.Fields("fldAddOnCPTCode").Value > "" Then
 '              g_Allowed = Round(rst.Fields("fldFee").Value * rst.Fields("fldUnits").Value + rst.Fields("fldAddOnFee").Value * IIf(rst.Fields("fldAddOnUnits").Value < 1, 1, rst.Fields("fldAddOnUnits").Value) - CDbl(lngDisallowance), 2)  'Monetary amount PG 402
 '              If rst.Fields("fldAddOnSecCPTCode").Value > "" Then
 '                 g_Allowed = Round(rst.Fields("fldFee").Value * rst.Fields("fldUnits").Value + rst.Fields("fldAddOnFee").Value * IIf(rst.Fields("fldAddOnUnits").Value < 1, 1, rst.Fields("fldAddOnUnits").Value) + rst.Fields("fldAddOnSecFee").Value * IIf(rst.Fields("fldAddOnSecUnits").Value < 1, 1, rst.Fields("fldAddOnSecUnits").Value) - CDbl(lngDisallowance), 2)  'Monetary amount PG 402
 '              End If
 '           End If
 '        Else
            Set objTx = CreateObject("PostingBz.CTxBz")
            Set rstTx = objTx.Fetch(CLng(rst.Fields("fldEncounterLogID").Value), 4)    ''Need to read according to the Database
            Set objTx = Nothing
        
            If rstTx.RecordCount > 0 Then
               Do While Not rstTx.EOF
                  If rstTx.Fields("fldInsuranceID").Value = rst.Fields("fldSecInsuranceID").Value Then
                    If rstTx.Fields("fldCheckID").Value > 0 And Not IsNull(rstTx.Fields("fldCheckDate").Value) And dteCheckDate = 0 Then dteCheckDate = rstTx.Fields("fldCheckDate").Value
                    If rstTx.Fields("fldTxTypeID").Value = 87 Then lngDeductable = lngDeductable + Round(IfNull(rstTx.Fields("fldAmount").Value, 0), 2)
                    If rstTx.Fields("fldTxTypeID").Value = 7 Then lngDisallowance = lngDisallowance + Round(IfNull(rstTx.Fields("fldAmount").Value, 0), 2)
                    If rstTx.Fields("fldTxTypeID").Value = 10 Or _
                       rstTx.Fields("fldTxTypeID").Value = 11 Or _
                       rstTx.Fields("fldTxTypeID").Value = 69 Or _
                       rstTx.Fields("fldTxTypeID").Value = 105 Or _
                       rstTx.Fields("fldTxTypeID").Value = 106 Or _
                       rstTx.Fields("fldTxTypeID").Value = 120 And _
                       Round(IfNull(rstTx.Fields("fldAmount").Value, 0), 2) <> 0 Then
                       lngPayment = CDbl(lngPayment) + Round(IfNull(rstTx.Fields("fldAmount").Value, 0), 2)
                       dteCheckDate = IfNull(rstTx.Fields("fldCheckDate").Value, rstTx.Fields("fldPostDate").Value)
                    End If
                    g_Allowed = Round(IfNull(rstTx.Fields("fldFee").Value, 0) * IfNull(rstTx.Fields("fldUnits").Value, 0) - CDbl(lngDisallowance), 2)
                    If rstTx.Fields("fldAddOnCPTCode").Value > "" Then
                        g_Allowed = Round(rstTx.Fields("fldFee").Value * rstTx.Fields("fldUnits").Value + rstTx.Fields("fldAddOnFee").Value * IIf(rstTx.Fields("fldAddOnUnits").Value < 1, 1, rstTx.Fields("fldAddOnUnits").Value) - CDbl(lngDisallowance), 2)  'Monetary amount PG 402
                        If rstTx.Fields("fldAddOnSecCPTCode").Value > "" Then
                            g_Allowed = Round(rstTx.Fields("fldFee").Value * rstTx.Fields("fldUnits").Value + rstTx.Fields("fldAddOnFee").Value * IIf(rstTx.Fields("fldAddOnUnits").Value < 1, 1, rstTx.Fields("fldAddOnUnits").Value) + rstTx.Fields("fldAddOnSecFee").Value * IIf(rstTx.Fields("fldAddOnSecUnits").Value < 1, 1, rstTx.Fields("fldAddOnSecUnits").Value) - CDbl(lngDisallowance), 2)  'Monetary amount PG 402
                        End If
                    End If
                  End If
                  rstTx.MoveNext
               Loop
               Set rstTx = Nothing
               lngCoInsurance = CDbl(g_Allowed) - CDbl(lngPayment) - CDbl(lngDeductable)
               g_CheckDate = dteCheckDate
            End If
   '      End If
         
        
   '     strAmt = "AMT*D*0.00~"
  '      If lngPayment > 0 Then  'Print a $0.00 Payment record
            strAmt01 = "D"
            strAmt02 = Format(IfNull(lngPayment, 0), "#####.00")
            strAmt = "AMT*" & strAmt01 & "*" & strAmt02 & "~"
            If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
                rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
                g_PrtString = g_PrtString + strAmt
                If Len(g_PrtString) >= 80 Then
                    Print #1, Left(g_PrtString, 80)
                    g_PrtString = Mid(g_PrtString, 81)
                End If
            Else
                If g_blnLineFeed Then Print #1, strAmt Else Print #1, strAmt;
            End If
            g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
  '      End If
        
         If lngDeductable + lngCoInsurance + IfNull(lngPayment, 0) <> IfNull(g_Allowed, 0) Then
            lngCoInsurance = g_Allowed - lngPayment - lngDeductable
         End If
         
   ' Removed 01/30/2018 - Invalid Amt
  '       If lngDeductable + lngCoInsurance > 0 Then
  '          strAmt01 = "F2"
  '          If g_bln5010 Then strAmt01 = "EAF"     'Patient Liability
  '          strAmt02 = Round(CStr(CDbl(lngDeductable) + CDbl(lngCoInsurance)), 2)
  '          strAmt = "AMT*" & strAmt01 & "*" & strAmt02 & "~"
  '          If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
  '              rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
  '              g_PrtString = g_PrtString + strAmt
  '              If Len(g_PrtString) >= 80 Then
  '                  Print #1, Left(g_PrtString, 80)
  '                  g_PrtString = Mid(g_PrtString, 81)
  '              End If
  '          Else
  '              If g_blnLineFeed Then Print #1, strAmt Else Print #1, strAmt;
  '          End If
  '          g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
  '       End If
            
         If IfNull(g_Allowed, 0) > 0 And Not g_bln5010 Then
            strAmt01 = "B6"
            strAmt02 = Format(IfNull(g_Allowed, 0), "#####.00")
            strAmt = "AMT*" & strAmt01 & "*" & strAmt02 & "~"
            If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
                rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT+
                g_PrtString = g_PrtString + strAmt
                If Len(g_PrtString) >= 80 Then
                    Print #1, Left(g_PrtString, 80)
                    g_PrtString = Mid(g_PrtString, 81)
                End If
            Else
                If g_blnLineFeed Then Print #1, strAmt Else Print #1, strAmt;
            End If
            g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
         End If
       
        
        If IsNull(rst.Fields("fldSecInsdDOB").Value) Then
            dtDOB = 0
        Else
            dtDOB = rst.Fields("fldSecInsdDOB").Value
        End If
    
        'If the DOB is not known, then this line must be ignored otherwise errors occur
        If (IsDate(dtDOB)) And (dtDOB > 0) And (Not g_bln5010) Then    '5010 Does not want the Insured DOB
            strDMG = "DMG*D8*" & Year(dtDOB) & Format(Month(dtDOB), "00") & Format(Day(dtDOB), "00") & "*"
            strDMG = strDMG & IfNull(rst.Fields("fldSecInsdSex").Value, "U") & "~"
            If g_blnLineFeed Then Print #1, strDMG Else Print #1, strDMG;
            g_lngNumSeg = g_lngNumSeg + 1
        End If
        'strOI = "OI***Y*B**Y~"
        'OI (Other Insurance information)
        strOI01 = ""              'not used
        strOI02 = ""              'not used
        strOI03 = "Y"             'Assignment of Benefits fldAssignmentYN
        If Not IsNull(rst.Fields("fldAssignmentYN").Value) Then strOI03 = rst.Fields("fldAssignmentYN").Value
        strOI04 = "B"             'Signature Source
        If g_bln5010 Then strOI04 = "P"    '5010 Provider is the source of the Signature
        strOI05 = ""              'not used
        strOI06 = "Y"             'Release of Patient Information
      
        strOI = "OI*" & strOI01 & "*" & strOI02 & "*" & strOI03 _
                & "*" & strOI04 & "*" & strOI05 & "*" & strOI06 & "~"
        If g_blnLineFeed Then Print #1, strOI Else Print #1, strOI;
        g_lngNumSeg = g_lngNumSeg + 1
        
        strNM101 = "IL"         'entity identifier code
        strNM102 = "1"          'Qualifier - 2:Non-person entity
        strNM103 = Trim(StripDelimiters(Mid(Replace(IfNull(rst.Fields("fldSecInsdLastName").Value, rst.Fields("fldPatientLastName").Value), "'", ""), 1, 35)))    'Last name
        strNM104 = Trim(StripDelimiters(Mid(Replace(IfNull(rst.Fields("fldSecInsdFisrtName").Value, rst.Fields("fldPatientFirstName").Value), "'", ""), 1, 35)))   'first name
        strNM105 = IfNull(rst.Fields("fldSecInsdMI").Value, "")   'middle name
        strNM106 = ""        'not used (name prefix)
        strNM107 = ""        'not used (name suffix)
        strNM108 = "MI"      'identification code qualifier
        strNM109 = Trim(Replace(StripDelimiters(IIf(rst.Fields("fldSecInsdCardNum").Value > "", rst.Fields("fldSecInsdCardNum").Value, "00")), " ", "", 1))
    
        strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
         & "*" & strNM104 & "*" & strNM105 & "*" & strNM106 _
         & "*" & strNM107 & "*" & strNM108 & "*" & strNM109 _
         & "~"

        If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
            g_PrtString = g_PrtString + strNM1
            If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
            End If
        Else
            If g_blnLineFeed Then Print #1, strNM1 Else Print #1, strNM1;
        End If
        g_lngNumSeg = g_lngNumSeg + 1

        If Not IsNull(rst.Fields("fldPatientStreetNum").Value) Then
            'Patient address
            strN3 = "N3*" & Trim(StripDelimiters(IfNull(rst.Fields("fldPatientStreetNum").Value, ""))) & "~"
            If g_blnLineFeed Then Print #1, strN3 Else Print #1, strN3;
            g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters

            strN401 = Trim(Replace(StripDelimiters(IfNull(rst.Fields("fldPatientCity").Value, "")), "'", ""))   'city
            strN402 = Trim(Replace(IfNull(rst.Fields("fldPatientState").Value, ""), "'", ""))  'State code (2 digits)
            strN403 = Trim(NumbersOnly(Replace(IfNull(rst.Fields("fldPatientZip").Value, ""), "'", "")))    'postal code
            
            If strN402 = "OT" Or strN402 = "ON" Then strN404 = "CA"
    
            strN4 = "N4*" & strN401 & "*" & strN402 & "*" & strN403 & "~"
            If strN404 > "" Then strN4 = "N4*" & strN401 & "*" & strN402 & "*" & strN403 & "*" & strN404 & "~"
            If g_blnLineFeed Then Print #1, strN4 Else Print #1, strN4;
            g_lngNumSeg = g_lngNumSeg + 1
        End If
      'Secondary info here
      strNM101 = "PR"                        'entity ID code (PR = payer)
      strNM102 = "2"                         'entity type qualifier (2 = non-person)
      strNM103 = Trim(Mid(Replace(IfNull(rst.Fields("fldSecInsName").Value, ""), "'", ""), 1, 35)) 'last name or organization
      strNM104 = ""                          'first name (optional)
      strNM105 = ""                          'middle name (optional)
      strNM106 = ""                          'name prefix (optional)
      strNM107 = ""                          'name suffix (optional)
      strNM108 = "PI"                        'ID code qualifier (46 = ETIN, PI = Payor Identification)
    
      'Medigap PayerCode
   '   If IfNull(rst.Fields("fldSecInsMedigapPayerCode").Value, "") > "" Then  'Medicaid of SC uses the Medigap payer ID for its secondary claims.
   '     strNM109 = rst.Fields("fldSecInsMedigapPayerCode").Value
   '   Else
   '      If rstClrHse.Fields("fldReceiverPrefixYN").Value = "Y" Then
   '         strNM109 = CreatePayerCodePrefix(rst.Fields("fldOthInsType").Value, IIf(rst.Fields("fldSecInsPayerCode").Value > "", rst.Fields("fldOthInsPayerCode").Value, "999999")) & IIf(rst.Fields("fldOthInsPayerCode").Value > "", rst.Fields("fldOthInsPayerCode").Value, "999999")
   '      Else
           strNM109 = IIf(rst.Fields("fldSecInsPayerCode").Value > "", rst.Fields("fldSecInsPayerCode").Value, "99999")
   '      End If
   '   End If
      If rstClrHse.Fields("fldClearingHouseID").Value = 150 And strNM109 = "PSYQUEL" Then strNM109 = "SX173"
      If rstClrHse.Fields("fldClearingHouseID").Value = 150 And strNM109 = "00710" Then strNM109 = "MIBLS"
      If rstClrHse.Fields("fldClearingHouseID").Value = 150 And strNM109 = "15460" Then strNM109 = "95462"
      If rstClrHse.Fields("fldClearingHouseID").Value = 150 And strNM109 = "88848" Then strNM109 = "IABLS"
      If rstClrHse.Fields("fldClearingHouseID").Value = 150 And strNM109 = "00624" Then strNM109 = "47198"
      If rstClrHse.Fields("fldClearingHouseID").Value = 150 And strNM109 = "990040115" Then strNM109 = "SB971"
      If rstClrHse.Fields("fldClearingHouseID").Value = 150 And strNM109 = "0027000" Then strNM109 = "00200"
      If rstClrHse.Fields("fldClearingHouseID").Value = 150 And strNM109 = "0085000" Then strNM109 = "14212"
      If rstClrHse.Fields("fldClearingHouseID").Value = 150 And rst.Fields("fldPlanID").Value <> 3817 And rst.Fields("fldPlanID").Value <> 2899 And rst.Fields("fldPlanID").Value <> 2125 And strNM109 = "620" Then strNM109 = "11202"
      If rstClrHse.Fields("fldClearingHouseID").Value = 150 And rst.Fields("fldPlanID").Value <> 3817 And rst.Fields("fldPlanID").Value <> 2899 And rst.Fields("fldPlanID").Value <> 2125 And strNM109 = "401" Then strNM109 = "00401"
      If rstClrHse.Fields("fldClearingHouseID").Value = 150 And strNM109 = "0088" Then strNM109 = "14312"
      If rst.Fields("fldPatientID").Value = 942501 Then strNM109 = "X0C"
      If rst.Fields("fldPatientID").Value = 1062234 Then strNM109 = "X0M"
        
   '    Carrier Code
   '     5508 Medicaid of New Hamshire
   '     2135 Medicaid of Rode Island
   '     2206 MediCal of California
   '     4420 MediCal of Conneticut
      If rst.Fields("fldOrder").Value = 3 And _
        (rst.Fields("fldPlanID").Value = 2135 Or _
         rst.Fields("fldPlanID").Value = 2206 Or _
         rst.Fields("fldPlanID").Value = 4420 Or _
         rst.Fields("fldPlanID").Value = 5508) Then
        If IsNumeric(rst.Fields("fldSecPlanName").Value) Then strNM109 = rst.Fields("fldSecPlanName").Value
      End If
        
      strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
        & "*" & strNM104 & "*" & strNM105 & "*" & strNM106 & "*" & strNM107 _
        & "*" & strNM108 & "*" & strNM109 & "~"
     '   strNM1 = "NM1*PR*2*AETNA*****PI*60054~"
        If rst.Fields("fldPatientID").Value = 155572 Then strNM1 = "NM1*PR*2*GROUP HEALTH INC*****PI*13551~"
        If rst.Fields("fldPatientID").Value = 159618 Then strNM1 = "NM1*PR*2*GROUP HEALTH INC*****PI*13551~"
        If rst.Fields("fldPatientID").Value = 165620 Then strNM1 = "NM1*PR*2*GROUP HEALTH INC*****PI*13551~"
        If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
            g_PrtString = g_PrtString + strNM1
            If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
            End If
        Else
            If g_blnLineFeed Then Print #1, strNM1 Else Print #1, strNM1;
        End If
        g_lngNumSeg = g_lngNumSeg + 1
    'Need Medicare Report Number when Filing secondary claims to Medicare
      If rst.Fields("fldInsuranceID").Value <> 24 And rst.Fields("fldSecInsuranceID").Value = 24 And Not IsNull(rst.Fields("fldICN").Value) Then
         If Trim(StripDelimiters(rst.Fields("fldICN").Value)) > "" Then
            strRef01 = "F8"
            strRef02 = Trim(StripDelimiters(rst.Fields("fldICN").Value))
            strRef = "REF*" & strRef01 & "*" & strRef02 & "~"
            If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
               rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
               g_PrtString = g_PrtString + strRef
               If Len(g_PrtString) >= 80 Then
                  Print #1, Left(g_PrtString, 80)
                  g_PrtString = Mid(g_PrtString, 81)
               End If
            Else
               If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
            End If
            g_lngNumSeg = g_lngNumSeg + 1
         End If
      End If
   End If
End If
    
    'Need Medicare Report Number when Filing secondary claims to Medicare
If rst.Fields("fldInsuranceID").Value <> 24 And rst.Fields("fldOthInsuranceID").Value = 24 And Not IsNull(rst.Fields("fldICN").Value) Then
   If Trim(StripDelimiters(rst.Fields("fldICN").Value)) > "" Then
      strRef01 = "F8"
      strRef02 = Trim(StripDelimiters(rst.Fields("fldICN").Value))
      strRef = "REF*" & strRef01 & "*" & strRef02 & "~"
      If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
         rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
         g_PrtString = g_PrtString + strRef
         If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
         End If
      Else
         If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
      End If
      g_lngNumSeg = g_lngNumSeg + 1
   End If
End If
    
Exit Function
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Private Sub WriteLoop2400(ByVal rst As ADODB.Recordset, ByVal rstClrHse As ADODB.Recordset, ByVal intLine As Integer, ByVal rstDetail As ADODB.Recordset)
'Service Line
    
    Dim strSvc As String
    Dim strSV1, strSV1_01, strSV1_02, strSV1_03 As String
    Dim strSV1_04, strSV1_05, strSV1_06 As String
    Dim strSV1_07, strSV1_08, strSV1_09 As String
    Dim strSV1_10, strSV1_11, strSV1_12 As String
    Dim strDTP, strDTP_01, strDTP_02, strDTP_03 As String
    Dim strRef, strRef01, strRef02 As String
    Dim strAmt, strAmt01, strAmt02, strAmt03, strAmt04 As String
    Dim strRevenueCode, strSQL As String
    Dim intCtr As Integer

    Dim objSQL As ADODB.Connection
    Dim rstSQL As ADODB.Recordset
    Set objSQL = CreateObject("ADODB.Connection")
    Set rstSQL = CreateObject("ADODB.Recordset")

    On Error GoTo ErrTrap:

    strSvc = "LX*" & intLine & "~"
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
        rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strSvc
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strSvc Else Print #1, strSvc;
    End If
    g_lngNumSeg = g_lngNumSeg + 1
        
    'SV1 segment (professional service)
    strSV1_01 = "HC"                    'product or service qualifer
    If IfNull(rstDetail.Fields("fldMod1").Value, "") > "" Then
        If IfNull(rstDetail.Fields("fldMod2").Value, "") > "" Then
            strSV1_01 = strSV1_01 & g_ElementSeperator & rstDetail.Fields("fldCPTCode").Value & g_ElementSeperator & rstDetail.Fields("fldMod1").Value & g_ElementSeperator & rstDetail.Fields("fldMod2").Value
            If rstDetail.Fields("fldCPTCode").Value = "H0046" Or rstDetail.Fields("fldCPTCode").Value = "90899" Or rstDetail.Fields("fldCPTCode").Value = "99499" Then
               strSV1_01 = strSV1_01 & g_ElementSeperator & g_ElementSeperator & g_ElementSeperator
               strSV1_01 = strSV1_01 & Trim(Left(rst.Fields("fldClaimNotes").Value, 80))
            End If
        Else
            strSV1_01 = strSV1_01 & g_ElementSeperator & rstDetail.Fields("fldCPTCode").Value & g_ElementSeperator & rstDetail.Fields("fldMod1").Value
            If (rstDetail.Fields("fldCPTCode").Value = "H0046" Or rstDetail.Fields("fldCPTCode").Value = "90899" Or rstDetail.Fields("fldCPTCode").Value = "99499") And Trim(Left(rst.Fields("fldClaimNotes").Value, 80)) > "" Then
               strSV1_01 = strSV1_01 & g_ElementSeperator & g_ElementSeperator & g_ElementSeperator & g_ElementSeperator
               strSV1_01 = strSV1_01 & Trim(Left(rst.Fields("fldClaimNotes").Value, 80))
            End If
        End If
    Else
        strSV1_01 = strSV1_01 & g_ElementSeperator & rstDetail.Fields("fldCPTCode").Value    'product/service ID/modifier1/modifier2
        If (rstDetail.Fields("fldCPTCode").Value = "H0046" Or rstDetail.Fields("fldCPTCode").Value = "90899" Or rstDetail.Fields("fldCPTCode").Value = "99499") And Trim(Left(rst.Fields("fldClaimNotes").Value, 80)) > "" Then
            strSV1_01 = strSV1_01 & g_ElementSeperator & g_ElementSeperator & g_ElementSeperator & g_ElementSeperator & g_ElementSeperator
            strSV1_01 = strSV1_01 & Trim(Left(rst.Fields("fldClaimNotes").Value, 80))
        End If
    End If
    
    Set objSQL = CreateObject("ADODB.Connection")
    objSQL.Open (CONST_PSYQUEL_CNN)
    'Institutional Claims
    strRevenueCode = ""
    If rst.Fields("fldClaimType").Value = "I" Then
      strSQL = "SELECT fldRevenueCode FROM tblCPTCode WHERE (fldCPTCode = '" & rstDetail.Fields("fldCPTCode").Value & "')"
      Set rstSQL = CreateObject("ADODB.Recordset")
      rstSQL.Open strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch
      strRevenueCode = IIf(rstSQL.EOF, "0914", rstSQL.Fields("fldRevenueCode"))
      rstSQL.Close
 '     Set rstSQL = Nothing
 '     If rst.Fields("fldInsuranceID").Value = 57 And (IfNull(rstDetail.Fields("fldMod1").Value, "") = "95" Or IfNull(rstDetail.Fields("fldMod2").Value, "") = "95") Then
 '        strRevenueCode = "780"
 '     End If
 '     If IfNull(rst.Fields("fldGroupNPI").Value, "") = "1508280322" And rst.Fields("fldInsuranceID").Value = 57 And rstDetail.Fields("fldCPTCode").Value Like "9*" Then
 '        strRevenueCode = "914"
 '     End If
      If strRevenueCode > "" Then strSV1_01 = strRevenueCode & "*" & strSV1_01
    End If
    objSQL.Close
    
    strSV1_02 = rstDetail.Fields("fldfee").Value * rstDetail.Fields("fldUnits").Value             'Monetary amount PG 402
    strSV1_03 = "UN"                          'unit or basis for measurement code
    strSV1_04 = rstDetail.Fields("fldUnits").Value                      'quantity (units or minutes)
    strSV1_05 = ""                            'Place of Service code(if different from loop 2300)
    If rstDetail.Fields("fldPOSCode").Value > "" And rst.Fields("fldPOS").Value <> rstDetail.Fields("fldPOSCode").Value Then strSV1_05 = rstDetail.Fields("fldPOSCode").Value
    strSV1_06 = ""                            'Service Type Code - Not used
    strSV1_07 = ""                            'DSM-IV pointer - points to first diagnosis line in HL* line
    
    intCtr = 1
    If IfNull(rstDetail.Fields("fldCodeIcd10").Value, "") > "" Then
        strSV1_07 = intCtr: intCtr = intCtr + 1
    End If
    If IfNull(rst.Fields("fldICD_1").Value, "") > "" And StripDecimals(IfNull(rst.Fields("fldICD_1").Value, "")) <> StripDecimals(IfNull(rstDetail.Fields("fldCodeIcd10").Value, "")) And intCtr < 5 Then
        strSV1_07 = strSV1_07 & g_ElementSeperator & intCtr: intCtr = intCtr + 1
    End If
    If IfNull(rst.Fields("fldICD_2").Value, "") > "" And StripDecimals(IfNull(rst.Fields("fldICD_2").Value, "")) <> StripDecimals(IfNull(rstDetail.Fields("fldCodeIcd10").Value, "")) And intCtr < 5 Then
        strSV1_07 = strSV1_07 & g_ElementSeperator & intCtr: intCtr = intCtr + 1
    End If
    If IfNull(rst.Fields("fldICD_3").Value, "") > "" And StripDecimals(IfNull(rst.Fields("fldICD_3").Value, "")) <> StripDecimals(IfNull(rstDetail.Fields("fldCodeIcd10").Value, "")) And intCtr < 5 Then
        strSV1_07 = strSV1_07 & g_ElementSeperator & intCtr: intCtr = intCtr + 1
    End If
    If IfNull(rst.Fields("fldICD_4").Value, "") > "" And StripDecimals(IfNull(rst.Fields("fldICD_4").Value, "")) <> StripDecimals(IfNull(rstDetail.Fields("fldCodeIcd10").Value, "")) And intCtr < 5 Then
        strSV1_07 = strSV1_07 & g_ElementSeperator & intCtr: intCtr = intCtr + 1
    End If
    strSV1_08 = ""
    strSV1_09 = ""                            'Emergency indicatior (Y/N) - Used only when a "Y" version A01
    strSV1_10 = ""                            'Multiple Procedure Code
    strSV1_11 = ""                            'EPSDT Indicator Y/N
    If IfNull(rst.Fields("fldEPSDTyn").Value, "") = "Y" And DateDiff("yyyy", rst.Fields("fldPatientDOB").Value, Now()) < 19 Then strSV1_11 = "Y"    'Medicaid AL  'rst.Fields("fldPlanID").Value = 5803 And
    strSV1_12 = ""                            'Family Planning Indicator Y/N
    
    'Institutional Claims
    If rst.Fields("fldClaimType").Value = "I" Then
        strSV1 = "SV2*" & strSV1_01 & "*" & strSV1_02 & "*" & strSV1_03 & "*" & strSV1_04 & "~"
    ElseIf strSV1_11 = "Y" Then
        strSV1 = "SV1*" & strSV1_01 & "*" & strSV1_02 & "*" & strSV1_03 & "*" & strSV1_04 & "*" _
        & strSV1_05 & "*" & strSV1_06 & "*" & strSV1_07 & "*" & strSV1_08 & "*" & strSV1_09 & "*" & strSV1_10 & "*" & strSV1_11 & "~"
    Else
        strSV1 = "SV1*" & strSV1_01 & "*" & strSV1_02 & "*" & strSV1_03 & "*" & strSV1_04 & "*" _
        & strSV1_05 & "*" & strSV1_06 & "*" & strSV1_07 & "~"
    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
        rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strSV1
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strSV1 Else Print #1, strSV1;
    End If
    g_lngNumSeg = g_lngNumSeg + 1
    
    'Date of Service
    strDTP_01 = "472"                'date/time qualifier
    strDTP_02 = "D8"                 'date time period format qualifier
    strDTP_03 = Format(rstDetail.Fields("fldDOS").Value, "yyyymmdd")              'date/time period in CCYYMMDD
    
    strDTP = "DTP*" & strDTP_01 & "*" & strDTP_02 & "*" & strDTP_03 & "~"
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
        rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strDTP
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strDTP Else Print #1, strDTP;
    End If
    g_lngNumSeg = g_lngNumSeg + 1
    
    
    If rst.Fields("fldClaimType").Value <> "I" Then
        'Line Item Control Number
        strRef01 = "6R"                'Seq Number
        strRef02 = "00" & intLine & ""
        strRef = "REF*" & strRef01 & "*" & strRef02 & "~"
        If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
            g_PrtString = g_PrtString + strRef
            If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
            End If
        Else
            If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
        End If
        g_lngNumSeg = g_lngNumSeg + 1
    End If
    
        'Cert/authorization information
    If IfNull(rstDetail.Fields("fldCertNum").Value, "") > "" And IfNull(rst.Fields("fldCertNum").Value, "") <> IfNull(rstDetail.Fields("fldCertNum").Value, "") Then
        strRef01 = "G1"
        strRef02 = Trim(StripDelimiters(rstDetail.Fields("fldCertNum").Value))
        strRef = "REF*" & strRef01 & "*" & strRef02 & "~"
        If InStr(1, strRef02, "No Auth") > 0 Or InStr(1, strRef02, "Not Req") > 0 Then strRef02 = ""
        If Trim(strRef02) > "" Then
            If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
                rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
                g_PrtString = g_PrtString + strRef
                If Len(g_PrtString) >= 80 Then
                    Print #1, Left(g_PrtString, 80)
                    g_PrtString = Mid(g_PrtString, 81)
                End If
            Else
                If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
            End If
            g_lngNumSeg = g_lngNumSeg + 1
        End If
    End If
        
    Exit Sub
    
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub



Private Sub WriteLoop2420B(ByVal rst As ADODB.Recordset, ByVal rstClrHse As ADODB.Recordset, ByVal strPayerCode As String)
'Rendering provider information

    Dim strNM101, strNM102, strNM103, strNM104, strNM105 As String
    Dim strNM106, strNM107, strNM108, strNM109 As String
    Dim strNM1 As String
    Dim strPRV, strPRV01, strPRV02, strPRV03, strPRV04 As String
    Dim strRef, strRef01, strRef02 As String
    
    On Error GoTo ErrTrap:
    
    strNM101 = "82"                'entity identifier code (82 = rendering provider)
    'Institutional Claims
    If rst.Fields("fldClaimType").Value = "I" Then strNM101 = "71"  'entity identifier code (71 = attending provider)
    
    strNM102 = "1"                 'entity type qualifier (1 = person)
    strNM103 = Trim(StripDelimiters(Mid(Replace(IfNull(rst.Fields("fldProviderLastName").Value, ""), "'", ""), 1, 35)))    'Last name
    strNM104 = Trim(StripDelimiters(Mid(Replace(IfNull(rst.Fields("fldProviderFirstName").Value, ""), "'", ""), 1, 35)))   'first name
    strNM105 = IfNull(rst.Fields("fldProviderMI").Value, "")   'middle name
    
    If rst.Fields("fldUseSuperYN").Value = "Y" Then
        If (IfNull(rst.Fields("fldSuperNPI").Value, "") = IfNull(rst.Fields("fldProviderNPI").Value, "") Or _
            IfNull(rst.Fields("fldProviderNPI").Value, "") = "") And _
           (IfNull(rst.Fields("fldSuperNPI").Value, "") > "") Then
            strNM103 = Trim(StripDelimiters(IfNull(rst.Fields("fldSuperLastName").Value, Mid(Replace(rst.Fields("fldProviderLastName").Value, "'", ""), 1, 35))))     'Last name
            strNM104 = Trim(StripDelimiters(IfNull(rst.Fields("fldSuperFirstName").Value, Mid(Replace(rst.Fields("fldProviderFirstName").Value, "'", ""), 1, 35))))   'first name
            strNM105 = IfNull(rst.Fields("fldSuperMI").Value, "")   'middle name
        ElseIf rst.Fields("fldInsuranceID").Value = 1235 Then
            strNM103 = Trim(StripDelimiters(IfNull(rst.Fields("fldSuperLastName").Value, Mid(Replace(rst.Fields("fldProviderLastName").Value, "'", ""), 1, 35))))     'Last name
            strNM104 = Trim(StripDelimiters(IfNull(rst.Fields("fldSuperFirstName").Value, Mid(Replace(rst.Fields("fldProviderFirstName").Value, "'", ""), 1, 35))))   'first name
            strNM105 = IfNull(rst.Fields("fldSuperMI").Value, "")   'middle name
        End If
    End If
    
    strNM106 = ""                  'name prefix (not used)
    strNM107 = ""                  'name suffix
    strNM108 = "XX"        'NPI
    strNM109 = IfNull(rst.Fields("fldProviderNPI").Value, IfNull(rst.Fields("fldSuperNPI").Value, ""))
    
    If IIf(rst.Fields("fldUseSuperYN").Value = "Y", IfNull(rst.Fields("fldSuperNPI").Value, ""), IfNull(rst.Fields("fldProviderNPI").Value, "")) = "" Then
        If rst.Fields("fldTinType").Value = 1 Then
            strNM108 = "34"        'individual SSN SY
        Else
            strNM108 = "24"        '24 = employer'S ID EI
        End If
        If rst.Fields("fldInsType").Value = "MB" And (Mid(strPayerCode, 1, 2) = "MR") Then  'Capario Medicare edit
            strNM108 = "24"        '24 = employer'S ID
        End If
        strNM109 = Left(NumbersOnly(rst.Fields("fldTIN").Value), 9)         'billing provider identifier code
    ElseIf rst.Fields("fldUseSuperYN").Value = "Y" Then
        If (IfNull(rst.Fields("fldSuperNPI").Value, "") = IfNull(rst.Fields("fldProviderNPI").Value, "") Or _
            IfNull(rst.Fields("fldProviderNPI").Value, "") = "") And _
           (IfNull(rst.Fields("fldSuperNPI").Value, "") > "") Then
            strNM109 = rst.Fields("fldSuperNPI").Value
        ElseIf rst.Fields("fldInsuranceID").Value = 1235 Then
            strNM109 = rst.Fields("fldSuperNPI").Value
        End If
    End If
    
    'fldReferNum is the UPIN not the Tax ID
    If Len(strNM109) <= 6 Then strNM109 = ""
    
    If strNM109 > "" Then
        strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
         & "*" & strNM104 & "*" & strNM105 & "*" & strNM106 _
         & "*" & strNM107 & "*" & strNM108 & "*" & strNM109 _
         & "~"
    ElseIf strNM105 > "" Then
        strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
         & "*" & strNM104 & "*" & strNM105 & "~"
    Else
        strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
         & "*" & strNM104 & "~"
    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strNM1
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strNM1 Else Print #1, strNM1;
    End If
    g_lngNumSeg = g_lngNumSeg + 1
    
    strPRV01 = "PE"                 'Provider Code (PE-Performing, BI-Billing, PT-Payto, RF-Referring)
    'Institutional Claims
    If rst.Fields("fldClaimType").Value = "I" Then strPRV01 = "AT"  'entity identifier code (AT = attending provider)
    
    strPRV02 = "ZZ"                 'Reference Identification Qualifier
    If g_bln5010 Then strPRV02 = "PXC"    '5010 uses PXC for the Taxonomy Code
    'Specialty information
    strPRV03 = Trim(StripDelimiters(IfNull(rst.Fields("fldTaxonomyCode").Value, "101Y00000X")))         'ZZ relies on Provider Taxonomy Code published by BC/BS Association
    If rst.Fields("fldUseSuperYN").Value = "Y" Then
        If (IfNull(rst.Fields("fldSuperNPI").Value, "") = IfNull(rst.Fields("fldProviderNPI").Value, "") Or _
            IfNull(rst.Fields("fldProviderNPI").Value, "") = "") And _
           (IfNull(rst.Fields("fldSuperNPI").Value, "") > "") Then
            strPRV03 = Trim(StripDelimiters(IfNull(rst.Fields("fldSuperTaxonomyCode").Value, "101Y00000X")))         'ZZ relies on Provider Taxonomy Code published by BC/BS Association
        End If
    End If
    If strPRV03 > "" And rst.Fields("fldClaimType").Value <> "I" Then
        strPRV = "PRV*" & strPRV01 & "*" & strPRV02 & "*" & strPRV03 & "~"
        If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
            g_PrtString = g_PrtString + strPRV
            If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
            End If
        Else
            If g_blnLineFeed Then Print #1, strPRV Else Print #1, strPRV;
        End If
        g_lngNumSeg = g_lngNumSeg + 1
    End If
    
    strRef01 = "": strRef02 = ""
    
    'Physician Type
    If rst.Fields("fldInsType").Value = "MP" Then
            strRef01 = "1C"
            strRef02 = StripDelimiters(IfNull(rst.Fields("fldIndPracticeNum").Value, ""))
    ElseIf rst.Fields("fldInsType").Value = "MB" Then
            strRef01 = "1C"
            strRef02 = StripDelimiters(IfNull(rst.Fields("fldIndPracticeNum").Value, ""))
    ElseIf rst.Fields("fldInsType").Value = "MC" Then
            strRef01 = "1D"
            strRef02 = StripDelimiters(IfNull(rst.Fields("fldIndPracticeNum").Value, ""))
    ElseIf rst.Fields("fldInsType").Value = "BL" Then 'BC/BS
            strRef01 = "1B"
            strRef02 = StripDelimiters(IfNull(rst.Fields("fldIndPracticeNum").Value, ""))
    ElseIf rst.Fields("fldInsType").Value = "BQ" Then 'BC/BS
            strRef01 = "N5"
            strRef02 = StripDelimiters(IfNull(rst.Fields("fldIndPracticeNum").Value, ""))
    ElseIf rst.Fields("fldInsType").Value = "CH" Then
            strRef01 = "1H"
            strRef02 = StripDelimiters(IfNull(rst.Fields("fldIndPracticeNum").Value, ""))
    ElseIf rst.Fields("fldInsType").Value = "1A" Then
            strRef01 = "1A"
            strRef02 = StripDelimiters(IfNull(rst.Fields("fldIndPracticeNum").Value, ""))
    ElseIf Not IsNull(rst.Fields("fldIndPracticeNum").Value) Then  'Commercial Claims
            strRef01 = "G2"
            strRef02 = StripDelimiters(IfNull(rst.Fields("fldIndPracticeNum").Value, ""))
    Else
            strRef01 = ""
            strRef02 = ""
    End If
    
    If rst.Fields("fldInsuranceID").Value = 1270 Then
        strRef01 = "0B"        'License Number 0B
    End If

    'Rendering Provider Primary identification code qualifier
    If (IfNull(rst.Fields("fldProviderNPI").Value, "") > "") And _
       (rst.Fields("fldInsuranceID").Value = 26 Or _
        rst.Fields("fldInsuranceID").Value = 217 Or _
        rst.Fields("fldInsuranceID").Value = 1270 Or _
        rst.Fields("fldInsuranceID").Value = 1301 Or _
        rst.Fields("fldInsuranceID").Value = 1311 Or _
        rst.Fields("fldPlanID").Value = 6064) Then 'Aetna, Magellan, BcBs NJ and Cigna require EIN/SY for group providers
        'rst.Fields("fldInsuranceID").Value = 1297 Or REMOVED BcBs NJ 12/24/2018
        'rst.Fields("fldInsuranceID").Value = 3 Or removed aetna 07/15/2014
        If Not IsNull(rst.Fields("fldIndPracticeNum").Value) And Len(StripDelimiters(IfNull(rst.Fields("fldIndPracticeNum").Value, ""))) = 9 Then
            strRef01 = "SY"        'individual SSN SY
            strRef02 = StripDelimiters(IfNull(rst.Fields("fldIndPracticeNum").Value, ""))
        End If
        If strRef02 > "" And strRef02 <> strNM109 Then
            strRef = "REF*" & strRef01 & "*" & strRef02 & "~"
            If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
                rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
                g_PrtString = g_PrtString + strRef
                If Len(g_PrtString) >= 80 Then
                    Print #1, Left(g_PrtString, 80)
                    g_PrtString = Mid(g_PrtString, 81)
                End If
            Else
                If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
            End If
            g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
        End If
    End If
        
    If Not g_bln5010 Then 'Legacy ID's are not sent in 5010
        strRef = "REF*" & strRef01 & "*" & strRef02 & "~"
        If strRef02 > "" And strRef01 <> "SY" Then
            If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
                rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
                g_PrtString = g_PrtString + strRef
                If Len(g_PrtString) >= 80 Then
                    Print #1, Left(g_PrtString, 80)
                    g_PrtString = Mid(g_PrtString, 81)
                End If
            Else
                If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
            End If
            g_lngNumSeg = g_lngNumSeg + 1
        End If
    End If
    
    Exit Sub
        
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub
Private Sub WriteLoop2420C(ByVal rst As ADODB.Recordset, ByVal rstClrHse As ADODB.Recordset)
'Service Facility information
   On Error GoTo ErrTrap:

   Dim strNM101, strNM102, strNM103, strNM104, strNM105 As String
   Dim strNM106, strNM107, strNM108, strNM109 As String
   Dim strNM1, strN2, strN3, strN4 As String
   Dim strN401, strN402, strN403, strN404 As String
   Dim strPRV, strPRV01, strPRV02, strPRV03, strPRV04 As String
   Dim strRef, strRef01, strRef02 As String
   Dim FacilityID, FacilityTypeID As String
     
   strNM101 = "FA"                'entity identifier code (FA = facility, LI = independant laboratory)
   If g_bln5010 Then strNM101 = "77"    '5010 Service Lcoation
   strNM102 = "2"                 'entity type qualifier (1 = person, 2 = non-person)
   strNM103 = Trim(Mid(StripDelimiters(IfNull(rst.Fields("fldFacilityLine1").Value, "")), 1, 35))  'Facility Name
   strNM104 = ""                  'first name
   strNM105 = ""                  'middle name
   strNM106 = ""                  'name prefix (not used)
   strNM107 = ""                  'name suffix
   strNM108 = ""                  'identification code qualifier
   strNM109 = ""                     'Facility Tax ID number
   If IfNull(rst.Fields("fldFacilityNPI").Value, "") = "" Then
        If (rst.Fields("fldPOS").Value = "11" And Not g_bln5010) Or rst.Fields("fldPLANID").Value = 4552 Or _
            rst.Fields("fldPayerCode").Value = "52189" Then      '5010 Do not duplicate 2010AA NM109
            strNM108 = "XX"        'NPI
            strNM109 = IfNull(rst.Fields("fldGroupNPI").Value, IfNull(rst.Fields("fldProviderNPI").Value, ""))
        End If
   Else    'Not g_bln5010 Or
        If (rst.Fields("fldFacilityNPI").Value <> IfNull(rst.Fields("fldProviderNPI").Value, "") And _
            rst.Fields("fldFacilityNPI").Value <> IfNull(rst.Fields("fldGroupNPI").Value, "")) Or _
            rst.Fields("fldPLANID").Value = 4552 Or _
            rst.Fields("fldPayerCode").Value = "52189" Then    '5010 Do not duplicate 2010AA NM109
            strNM108 = "XX"        'NPI
            strNM109 = Trim(rst.Fields("fldFacilityNPI").Value)
        End If
   End If
       
   If strNM109 > "" Then
        strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
         & "*" & strNM104 & "*" & strNM105 & "*" & strNM106 _
         & "*" & strNM107 & "*" & strNM108 & "*" & strNM109 _
         & "~"
   ElseIf strNM104 > "" Then
        strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
         & "*" & strNM104 & "*" & strNM105 & "~"
   Else
       strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 & "~"
   End If
     
   If rst.Fields("fldPOS").Value = "12" And rst.Fields("fldShortageAreaYN").Value = "Y" Then 'Patient Visit in a Shortage Area at their home.
      strNM102 = "1"          'Qualifier - 2:Non-person entity
      strNM103 = Trim(StripDelimiters(Mid(Replace(IfNull(rst.Fields("fldPatientLastName").Value, ""), "'", ""), 1, 35)))    'Last name
      strNM104 = Trim(StripDelimiters(Mid(Replace(IfNull(rst.Fields("fldPatientFirstName").Value, ""), "'", ""), 1, 35)))   'first name
      strNM105 = IfNull(rst.Fields("fldPatientMI").Value, "")   'middle name
      strNM106 = ""        'not used (name prefix)
      strNM107 = ""        'not used (name suffix)
      strNM108 = ""                  'identification code qualifier
      strNM109 = ""                     'Facility Tax ID number
    
      strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
         & "*" & strNM104 & "*" & strNM105 & "*" & strNM106 _
         & "*" & strNM107 & "*" & strNM108 & "*" & strNM109 _
         & "~"
         
      If g_bln5010 Then
         If strNM105 > "" Then
            strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
                & "*" & strNM104 & "*" & strNM105 & "~"
         Else
            strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
                & "*" & strNM104 & "~"
         End If
      End If
   End If
   
   If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strNM1
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
   Else
        If g_blnLineFeed Then Print #1, strNM1 Else Print #1, strNM1;
   End If
   g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
   
   'If the Facility name is more than 35 characters then we use segment N2 to hold the
   'overflow characters
    If Len(IfNull(rst.Fields("fldFacilityLine1").Value, "")) > 35 Then
        strN2 = "N2*" & Trim(Mid(rst.Fields("fldFacilityLine1").Value, 36)) & "~"
    Else
        strN2 = ""
    End If
        
    If strN2 > "" Then
    '   If g_blnLineFeed Then Print #1, strN2 Else Print #1, strN2;  'Print overflow line
    '   g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
    End If
      
   strN3 = ""
   If rst.Fields("fldPOS").Value = "12" And rst.Fields("fldShortageAreaYN").Value = "Y" Then 'Patient Visit in a Shortage Area at their home.
      If Not IsNull(rst.Fields("fldPatientStreetNum").Value) Then           'Patient address
         strN3 = "N3*" & Trim(StripDelimiters(IfNull(rst.Fields("fldPatientStreetNum").Value, ""))) & "~"
      End If
   Else
      If IfNull(rst.Fields("fldFacilityLine2").Value, "") > "" Then
         strN3 = "N3*" & Trim(StripDelimiters(rst.Fields("fldFacilityLine2").Value)) & "~"
      End If
   End If

   If Trim(strN3) > "" Then
      If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
         rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
         g_PrtString = g_PrtString + strN3
         If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
         End If
      Else
            If g_blnLineFeed Then Print #1, strN3 Else Print #1, strN3;
      End If
      g_lngNumSeg = g_lngNumSeg + 1
   End If
   
   strN4 = ""
   If rst.Fields("fldPOS").Value = "12" And rst.Fields("fldShortageAreaYN").Value = "Y" Then 'Patient Visit in a Shortage Area at their home.
      strN401 = Trim(Replace(IfNull(StripDelimiters(rst.Fields("fldPatientCity").Value), ""), "'", ""))   'city
      strN402 = Trim(Replace(IfNull(rst.Fields("fldPatientState").Value, ""), "'", ""))  'State code (2 digits)
      strN403 = Trim(NumbersOnly(Replace(IfNull(rst.Fields("fldPatientZip").Value, ""), "'", "")))    'postal code
      If Len(strN403) = 5 Then strN403 = strN403 & "9998"   'Medicare requires 9 numeric for zip suffix.
      If strN402 = "OT" Or strN402 = "ON" Then strN404 = "CA"
      strN4 = "N4*" & strN401 & "*" & strN402 & "*" & strN403 & "~"
      If strN404 > "" Then strN4 = "N4*" & strN401 & "*" & strN402 & "*" & strN403 & "*" & strN404 & "~"
   Else
        strN4 = "N4*" & Trim(rst.Fields("fldFacilityCity").Value) & "*" & UCase(rst.Fields("fldFacilityState").Value) & "*" & Trim(NumbersOnly(Replace(IfNull(rst.Fields("fldFacilityZip").Value, ""), "'", ""))) & "~"
   End If
   If Trim(strN3) > "" Then
      If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
         rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
         g_PrtString = g_PrtString + strN4
         If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
         End If
      Else
            If g_blnLineFeed Then Print #1, strN4 Else Print #1, strN4;
      End If
      g_lngNumSeg = g_lngNumSeg + 1
   End If
      
   If rstClrHse.Fields("fldClearingHouseID").Value = 152 Then 'MAINECARE
      If Not IsNull(rst.Fields("fldIndSecondaryID").Value) Then
        strRef01 = "LU"
        strRef02 = IfNull(rst.Fields("fldIndSecondaryID").Value, "")
      End If
      If strRef02 > "" Then
         strRef = "REF*" & strRef01 & "*" & strRef02 & "~"
         If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
                rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
                g_PrtString = g_PrtString + strRef
                If Len(g_PrtString) >= 80 Then
                    Print #1, Left(g_PrtString, 80)
                    g_PrtString = Mid(g_PrtString, 81)
                End If
         Else
                If g_blnLineFeed Then Print #1, strRef Else Print #1, strRef;
         End If
         g_lngNumSeg = g_lngNumSeg + 1
      End If
   End If
   Exit Sub
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub


Private Sub WriteLoop2430(ByVal rst As ADODB.Recordset, ByVal rstClrHse As ADODB.Recordset, ByVal rstDetail As ADODB.Recordset, ByVal strInsSeq As String)
'Service Line
    
    Dim objCheck As PostingBz.CCheckBz
    Dim rstCheck As ADODB.Recordset
    Dim objCheckLog As PostingBz.CCheckLogBZ
    Dim rstCheckLog As ADODB.Recordset
    Dim objTx As PostingBz.CTxBz
    Dim rstTx As ADODB.Recordset
    Dim strSvc As String
    Dim strSVD, strSVD_01, strSVD_02, strSVD_03 As String
    Dim strSVD_04, strSVD_05, strSVD_06 As String
    Dim strSVD_07, strSVD_08, strSVD_09 As String
    Dim strDTP, strDTP_01, strDTP_02, strDTP_03 As String
    Dim strCAS, strCAS01, strCAS02, strCAS03, strCAS04 As String
    Dim strCAS05, strCAS06, strCAS07, strCAS08, strCAS09, strCAS10 As String
    Dim strAmt, strAmt01, strAmt02, strAmt03, strAmt04 As String
    Dim strAMT05, strAMT06, strAMT07, strAMT08, strAMT09 As String
    Dim lngDeductable, lngDisallowance, lngPayment, lngAllowed, lngCoInsurance, lngCoPay As Currency
    Dim lngPayAddOn, lngDisAddOn, lngDedAddOn, lngCoAddOn, lngCoPayAddOn  As Currency
    Dim lngPayAddOnSec, lngDisAddOnSec, lngDedAddOnSec, lngCoAddOnSec, lngCoPayAddOnSec, dblTotal  As Currency
    Dim lngCheckID, lngInsuranceID, lngPlanID As Long
    Dim dteCheckDate, dteCheckDateAddOn, dteCheckDateAddOnSec, dtePostDate As Date
    Dim blnFound As Boolean
    Dim objSQL As ADODB.Connection
    Set objSQL = CreateObject("ADODB.Connection")
    objSQL.Open (CONST_PSYQUEL_CNN)
    Dim strSQL, strSQL1 As String
    Dim rstSQL As ADODB.Recordset

    On Error GoTo ErrTrap:
    
    lngDeductable = 0: lngDedAddOn = 0: lngDisAddOnSec = 0: lngDisallowance = 0: lngDisAddOn = 0: lngDisAddOnSec = 0: lngPayment = 0: lngPayAddOn = 0: lngPayAddOnSec = 0: lngCoInsurance = 0: lngCoPay = 0: lngCoPayAddOn = 0:  lngCoPayAddOnSec = 0
    lngCheckID = 0: dteCheckDate = 0: dteCheckDateAddOn = 0: dteCheckDateAddOnSec = 0: dtePostDate = 0: g_Allowed = 0: blnFound = False
    'Fetch Check Log Transaction record from DB
    Set objCheckLog = CreateObject("PostingBz.CCheckLogBz")
    If strInsSeq = "P" Then lngInsuranceID = CLng(rst.Fields("fldOthInsuranceID").Value)
    If strInsSeq = "S" Then lngInsuranceID = CLng(rst.Fields("fldSecInsuranceID").Value)
    If strInsSeq = "P" Then lngPlanID = CLng(rst.Fields("fldOthPlanID").Value)
    If strInsSeq = "S" Then lngPlanID = CLng(rst.Fields("fldSecPlanID").Value)
    strSQL = "(A.fldPatientID = " & CLng(rst.Fields("fldPatientID").Value) & ") AND " & _
             "(A.fldEncounterLogID = " & CLng(rst.Fields("fldEncounterLogID").Value) & ") AND " & _
             "(A.fldDenTxTypeID <> 255) AND " & _
             "(A.fldInsuranceID = " & CLng(lngInsuranceID) & ") AND " & _
             "(A.fldPlanID = " & CLng(lngPlanID) & ")"
    If Trim(IfNull(rst.Fields("fldICN").Value, "")) = "" Then
        strSQL = strSQL & " AND (A.fldCPTRecordID = " & CLng(rstDetail.Fields("fldCPTRecordID").Value) & ")  "
    End If
    Set rstCheckLog = objCheckLog.Fetch(False, strSQL, "E.fldCheckDate")
    Set objCheckLog = Nothing
    
    'Check for a specific CPT Code - fldCPTRecordID
    If rstCheckLog.RecordCount > 0 Then
        If CLng(rstDetail.Fields("fldCPTRecordID").Value) > 0 Then
            If Trim(IfNull(rst.Fields("fldICN").Value, "")) > "" Then
                Set objCheckLog = CreateObject("PostingBz.CCheckLogBz")
                strSQL1 = strSQL & " AND (A.fldCPTRecordID = " & CLng(rstDetail.Fields("fldCPTRecordID").Value) & ")  "
                Set rstCheckLog = objCheckLog.Fetch(False, strSQL1, "E.fldCheckDate")
                Set objCheckLog = Nothing
                If rstCheckLog.RecordCount <= 0 Then
                    Set objCheckLog = CreateObject("PostingBz.CCheckLogBz")
                    Set rstCheckLog = objCheckLog.Fetch(False, strSQL, "E.fldCheckDate")
                    Set objCheckLog = Nothing
                End If
            End If
        End If
    End If
    
    'Fetch Check Transaction record from DB
    If rstCheckLog.RecordCount > 0 Then
        'SVD service line adjudication
        Do While Not rstCheckLog.EOF
            lngCheckID = rstCheckLog.Fields("fldCheckID").Value
            g_CheckDate = IfNull(rstCheckLog.Fields("fldCheckDate").Value, 0)
            If rstCheckLog.Fields("fldInsuranceID").Value = lngInsuranceID Then
               strSQL = "SELECT B.AmountPaid, A.Payment, A.Disallowed, A.Disallowed, A.Deductible, A.Coinsurance, A.PatientResp, A.ProcedureCode, A.Reason1 " & _
                        "FROM tblRemittanceDetail AS A INNER JOIN " & _
                        "   tblRemittance AS B ON A.RemittanceRecordID = B.RemittanceRecordID " & _
                        "WHERE (A.fldPatientID = " & CLng(rst.Fields("fldPatientID").Value) & ") And " & _
                        "(A.fldEncounterLogID = " & CLng(rst.Fields("fldEncounterLogID").Value) & ") And " & _
                        "(A.fldCheckID = " & rstCheckLog.Fields("fldCheckID").Value & ") And " & _
                        "(A.fldPayerCheckLogID = " & rstCheckLog.Fields("fldPayerCheckLogID").Value & ")"
               Set rstSQL = CreateObject("ADODB.Recordset")
               rstSQL.Open strSQL, objSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch
               If Not rstSQL.EOF Then
                  If Trim(IfNull(rst.Fields("fldICN").Value, "")) > "" Or IfNull(rstSQL.Fields("ProcedureCode"), "") = rstDetail.Fields("fldCPTCode").Value Then
                     blnFound = True
                     lngPayment = lngPayment + Round(IfNull(rstSQL.Fields("Payment"), 0), 2)
                     If lngPayment = 0 And rstSQL.Fields("Reason1") = "OA96" And IfNull(rstSQL.Fields("AmountPaid"), 0) > 0 Then lngPayment = lngPayment + Round(IfNull(rstSQL.Fields("AmountPaid"), 0), 2)
                     lngDeductable = lngDeductable + Round(IfNull(rstSQL.Fields("Deductible"), 0), 2)
                     lngCoInsurance = lngCoInsurance + Round(IfNull(rstSQL.Fields("Coinsurance"), 0), 2)
                     lngCoPay = lngCoPay + Round(IfNull(rstSQL.Fields("PatientResp"), 0), 2)
                     lngDisallowance = lngDisallowance + Round(IfNull(rstSQL.Fields("Disallowed"), 0), 2)
                     lngCoPay = IIf(CCur(lngCoInsurance) = CCur(lngCoPay), 0, CCur(lngCoPay))
                     lngCoPay = IIf((CCur(lngDeductable) + CCur(lngCoInsurance)) = CCur(lngCoPay), 0, CCur(lngCoPay))
                     g_CheckDate = IfNull(rstCheckLog.Fields("fldCheckDate").Value, 0)
                     g_Allowed = g_Allowed + Round(IfNull(rstDetail.Fields("fldLineTotal").Value, 0) - CCur(lngDisallowance), 2)
                     g_Allowed = IIf(g_Allowed = 0, CCur(lngPayment) + CCur(lngDeductable) + CCur(lngCoInsurance) + CCur(lngCoPay), g_Allowed)
                  End If
         '         If IfNull(rstSQL.Fields("ProcedureCode"), "") = IfNull(rst.Fields("fldAddOnCPTCode").Value, "") Then
         '            lngPayAddOn = lngPayAddOn + Round(IfNull(rstSQL.Fields("Payment"), 0), 2)
          '           lngDedAddOn = lngDedAddOn + Round(IfNull(rstSQL.Fields("Deductible"), 0), 2)
          '           lngCoAddOn = lngCoAddOn + Round(IfNull(rstSQL.Fields("Coinsurance"), 0), 2)
           '          lngCoPayAddOn = lngCoPayAddOn + Round(IfNull(rstSQL.Fields("PatientResp"), 0), 2)
            '         lngDisAddOn = lngDisAddOn + Round(IfNull(rstSQL.Fields("Disallowed"), 0), 2)
             '        dteCheckDateAddOn = IfNull(rstCheckLog.Fields("fldCheckDate").Value, 0)
              '       g_Allowed = g_Allowed + Round(IfNull(rst.Fields("fldAddOnFee").Value, 0) * IfNull(rst.Fields("fldAddOnUnits").Value, 0) - CCur(lngDisAddOn), 2)
               '      g_Allowed = IIf(g_Allowed = 0, CCur(lngPayAddOn) + CCur(lngDedAddOn) + CCur(lngCoAddOn) + CCur(lngCoPayAddOn), g_Allowed)
               '   End If
       '           If IfNull(rstSQL.Fields("ProcedureCode"), "") = IfNull(rst.Fields("fldAddOnSecCPTCode").Value, "") Then
        '             lngPayAddOnSec = Round(IfNull(rstSQL.Fields("Payment"), 0), 2)
         '            lngDedAddOnSec = Round(IfNull(rstSQL.Fields("Deductible"), 0), 2)
          '           lngCoAddOnSec = Round(IfNull(rstSQL.Fields("Coinsurance"), 0), 2)
           '          lngCoPayAddOnSec = lngCoPayAddOnSec + Round(IfNull(rstSQL.Fields("PatientResp"), 0), 2)
            '         lngDisAddOnSec = Round(IfNull(rstSQL.Fields("Disallowed"), 0), 2)
             '        dteCheckDateAddOnSec = IfNull(rstCheckLog.Fields("fldCheckDate").Value, 0)
              '       g_Allowed = g_Allowed + Round(IfNull(rst.Fields("fldAddOnSecFee").Value, 0) * IfNull(rst.Fields("fldAddOnSecUnits").Value, 0) - CCur(lngDisAddOnSec), 2)
               '      g_Allowed = IIf(g_Allowed = 0, CCur(lngPayAddOnSec) + CCur(lngDedAddOnSec) + CCur(lngCoAddOnSec) + CCur(lngCoPayAddOnSec), g_Allowed)
                '  End If
               End If
               Set rstSQL = Nothing
               rstCheckLog.MoveNext
            End If
        Loop
        If blnFound = True And lngPayment = 0 And lngDeductable = 0 And lngDisallowance = 0 And lngCoInsurance = 0 And CDbl(rstDetail.Fields("fldFee").Value) > 0 Then blnFound = False
    End If
    If blnFound = False Then
        Set objTx = CreateObject("PostingBz.CTxBz")
        Set rstTx = objTx.Fetch(CLng(rst.Fields("fldEncounterLogID").Value), 2)
        Set objTx = Nothing
        
        If rstTx.RecordCount > 0 Then
            Do While Not rstTx.EOF
              If rstTx.Fields("fldInsuranceID").Value = lngInsuranceID And _
                (Trim(IfNull(rst.Fields("fldICN").Value, "")) > "" Or CLng(IfNull(rstTx.Fields("fldCPTRecordID").Value, 0)) = CLng(rstDetail.Fields("fldCPTRecordID").Value)) Then
             '   If rstTx.Fields("fldCheckID").Value > 0 And Not IsNull(rstTx.Fields("fldCheckDate").Value) And dteCheckDate = 0 Then dteCheckDate = rstTx.Fields("fldCheckDate").Value
                If rstTx.Fields("fldTxTypeID").Value = 87 Then
                    lngDeductable = lngDeductable + Round(IfNull(rstTx.Fields("fldAmount").Value, 0), 2)
                    dtePostDate = rstTx.Fields("fldPostDate").Value
                    If Not IsNull(rstTx.Fields("fldCheckDate").Value) And dteCheckDate = 0 Then
                        dteCheckDate = IfNull(rstTx.Fields("fldCheckDate").Value, rstTx.Fields("fldPostDate").Value)
                        lngCheckID = IfNull(rstTx.Fields("fldCheckID").Value, 0)
                    End If
                End If
                If rstTx.Fields("fldTxTypeID").Value = 10 Or _
                    rstTx.Fields("fldTxTypeID").Value = 11 Or _
                    rstTx.Fields("fldTxTypeID").Value = 105 Or _
                    rstTx.Fields("fldTxTypeID").Value = 106 Or _
                    rstTx.Fields("fldTxTypeID").Value = 120 Then
                '    If rstTx.Fields("fldTxTypeID").Value = 120 Or rstTx.Fields("fldTxTypeID").Value = 10 Then
                        dtePostDate = rstTx.Fields("fldPostDate").Value
                '        If rstTx.Fields("fldAddOnCPTCode").Value > "" And lngPayment = 0 Then
                '            lngPayment = lngPayment + Round(IfNull(rstTx.Fields("fldAmount").Value, 0), 2)
                '        ElseIf rstTx.Fields("fldAddOnCPTCode").Value > "" And lngPayment <> 0 And lngPayAddOn = 0 Then
                '            lngPayAddOn = lngPayAddOn + Round(IfNull(rstTx.Fields("fldAmount").Value, 0), 2)
                '        ElseIf rstTx.Fields("fldAddOnSecCPTCode").Value > "" And lngPayment <> 0 And lngPayAddOn <> 0 And lngPayAddOnSec = 0 Then
                '            lngPayAddOnSec = lngPayAddOnSec + Round(IfNull(rstTx.Fields("fldAmount").Value, 0), 2)
                '        Else
                            lngPayment = lngPayment + Round(IfNull(rstTx.Fields("fldAmount").Value, 0), 2)
                '        End If
                        If Not IsNull(rstTx.Fields("fldCheckDate").Value) And (dteCheckDate = 0 Or rstTx.Fields("fldTxTypeID").Value = 10 Or rstTx.Fields("fldTxTypeID").Value = 120) Then
                            dteCheckDate = IfNull(rstTx.Fields("fldCheckDate").Value, rstTx.Fields("fldPostDate").Value)
                            lngCheckID = IfNull(rstTx.Fields("fldCheckID").Value, 0)
                        End If
                '    End If
                End If
                If rstTx.Fields("fldTxTypeID").Value = 7 Then
                    dtePostDate = rstTx.Fields("fldPostDate").Value
            '        If rstTx.Fields("fldAddOnCPTCode").Value > "" And lngDisallowance = 0 Then
                            lngDisallowance = lngDisallowance + Round(IfNull(rstTx.Fields("fldAmount").Value, 0), 2)
            '        ElseIf rstTx.Fields("fldAddOnCPTCode").Value > "" And lngDisallowance <> 0 And lngDisAddOn = 0 Then
            '                lngDisAddOn = lngDisAddOn + Round(IfNull(rstTx.Fields("fldAmount").Value, 0), 2)
            '        ElseIf rstTx.Fields("fldAddOnSecCPTCode").Value > "" And lngDisallowance <> 0 And lngDisAddOn <> 0 And lngDisAddOnSec = 0 Then
            '                lngDisAddOnSec = lngDisAddOnSec + Round(IfNull(rstTx.Fields("fldAmount").Value, 0), 2)
            '        Else
            '                lngDisallowance = lngDisallowance + Round(IfNull(rstTx.Fields("fldAmount").Value, 0), 2)
            '        End If
                    If Not IsNull(rstTx.Fields("fldCheckDate").Value) And dteCheckDate = 0 Then
                            dteCheckDate = IfNull(rstTx.Fields("fldCheckDate").Value, rstTx.Fields("fldPostDate").Value)
                            lngCheckID = IfNull(rstTx.Fields("fldCheckID").Value, 0)
                    End If
                End If
              End If
              rstTx.MoveNext
            Loop
            dteCheckDate = IIf(dteCheckDate = 0, g_CheckDate, dteCheckDate)
            g_Allowed = Round(IfNull(rstDetail.Fields("fldLineTotal").Value, 0) - CCur(lngDisallowance), 2)
         '   If rst.Fields("fldAddOnCPTCode").Value > "" Then
         '       g_Allowed = Round(rstDetail.Fields("fldFee").Value * rstDetail.Fields("fldUnits").Value + rst.Fields("fldAddOnFee").Value * IIf(rst.Fields("fldAddOnUnits").Value < 1, 1, rst.Fields("fldAddOnUnits").Value) - CCur(lngDisallowance), 2)  'Monetary amount PG 402
         '       If rst.Fields("fldAddOnSecCPTCode").Value > "" Then
         '           g_Allowed = Round(rstDetail.Fields("fldFee").Value * rstDetail.Fields("fldUnits").Value + rst.Fields("fldAddOnFee").Value * IIf(rst.Fields("fldAddOnUnits").Value < 1, 1, rst.Fields("fldAddOnUnits").Value) + rst.Fields("fldAddOnSecFee").Value * IIf(rst.Fields("fldAddOnSecUnits").Value < 1, 1, rst.Fields("fldAddOnSecUnits").Value) - CCur(lngDisallowance), 2)  'Monetary amount PG 402
         '       End If
         '   End If
            Set rstTx = Nothing
        End If
    End If
    
    'Verify Disallowance for AddOn Codes
'    If lngDisAddOn > rst.Fields("fldAddOnFee").Value * rst.Fields("fldAddOnUnits").Value And _
'       lngDisallowance < rst.Fields("fldAddOnFee").Value * rst.Fields("fldAddOnUnits").Value Then
'        dblTotal = lngDisAddOn
'        lngDisAddOn = lngDisallowance
'        lngDisallowance = dblTotal
'    End If
    If lngDisallowance > rstDetail.Fields("fldLineTotal").Value Then
        dblTotal = Round(rstDetail.Fields("fldLineTotal").Value, 2)
        lngDisallowance = dblTotal
    End If
        
    lngAllowed = Round(IfNull(rstDetail.Fields("fldFee").Value, 0) * IfNull(rstDetail.Fields("fldUnits").Value, 0) - CCur(lngDisallowance), 2)
    
    If lngPayment = 0 And lngDeductable = 0 And lngCoInsurance = 0 Then
        lngDeductable = Round(lngAllowed, 2)
        lngCoPay = 0
    ElseIf lngPayment = 0 And lngDeductable > 0 And lngCoInsurance = 0 And lngDeductable > lngAllowed Then
        lngDeductable = Round(lngAllowed, 2)
        lngCoPay = 0
    ElseIf lngPayment = 0 And lngDeductable = 0 And lngCoInsurance > 0 And lngCoInsurance > lngAllowed Then
        lngCoInsurance = Round(lngAllowed, 2)
        lngCoPay = 0
    ElseIf lngPayment = 0 And lngDeductable > 0 And lngCoInsurance > 0 And (lngDeductable + lngCoInsurance) > lngAllowed Then
        lngDeductable = Round(lngAllowed, 2)
        lngCoInsurance = 0
        lngCoPay = 0
    ElseIf lngPayment = 0 And lngDeductable > 0 And lngCoInsurance > 0 Then
        lngCoPay = Round((lngAllowed - lngDeductable - lngCoInsurance), 2)
    ElseIf lngCoInsurance = 0 Then
        lngCoInsurance = Round((lngAllowed - lngPayment - lngDeductable), 2)
        lngCoPay = 0
    End If
    
    ' Other Payer Identifier    'Medigap PayerCode
    If IfNull(rst.Fields("fldOthInsMedigapPayerCode").Value, "") > "" Then
        If strInsSeq = "P" Then strSVD_01 = rst.Fields("fldOthInsMedigapPayerCode").Value
        If strInsSeq = "S" Then strSVD_01 = rst.Fields("fldSecInsMedigapPayerCode").Value
    Else
        If rstClrHse.Fields("fldReceiverPrefixYN").Value = "Y" Then
            If strInsSeq = "P" Then strSVD_01 = CreatePayerCodePrefix(rst.Fields("fldOthInsType").Value, IIf(rst.Fields("fldOthInsPayerCode").Value > "", rst.Fields("fldOthInsPayerCode").Value, "999999")) & IIf(rst.Fields("fldOthInsPayerCode").Value > "", rst.Fields("fldOthInsPayerCode").Value, "999999")
            If strInsSeq = "S" Then strSVD_01 = CreatePayerCodePrefix(rst.Fields("fldSecInsType").Value, IIf(rst.Fields("fldSecInsPayerCode").Value > "", rst.Fields("fldSecInsPayerCode").Value, "999999")) & IIf(rst.Fields("fldOthInsPayerCode").Value > "", rst.Fields("fldOthInsPayerCode").Value, "999999")
        Else
            If strInsSeq = "P" Then strSVD_01 = IIf(rst.Fields("fldOthInsPayerCode").Value > "", IIf(Len(rst.Fields("fldOthInsPayerCode").Value) < 5, Format(rst.Fields("fldOthInsPayerCode").Value, "00000"), rst.Fields("fldOthInsPayerCode").Value), "99999")
            If strInsSeq = "S" Then strSVD_01 = IIf(rst.Fields("fldSecInsPayerCode").Value > "", IIf(Len(rst.Fields("fldSecInsPayerCode").Value) < 5, Format(rst.Fields("fldSecInsPayerCode").Value, "00000"), rst.Fields("fldSecInsPayerCode").Value), "99999")
        End If
    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 150 And strSVD_01 = "PSYQUEL" Then strSVD_01 = "SX173"
    If rstClrHse.Fields("fldClearingHouseID").Value = 150 And strSVD_01 = "00710" Then strSVD_01 = "MIBLS"
    If rstClrHse.Fields("fldClearingHouseID").Value = 150 And strSVD_01 = "88848" Then strSVD_01 = "IABLS"
    If rstClrHse.Fields("fldClearingHouseID").Value = 150 And strSVD_01 = "00624" Then strSVD_01 = "47198"
    If rstClrHse.Fields("fldClearingHouseID").Value = 150 And strSVD_01 = "990040115" Then strSVD_01 = "SB971"
    If rstClrHse.Fields("fldClearingHouseID").Value = 150 And strSVD_01 = "0027000" Then strSVD_01 = "00200"
    If rstClrHse.Fields("fldClearingHouseID").Value = 150 And strSVD_01 = "0085000" Then strSVD_01 = "14212"
    If rstClrHse.Fields("fldClearingHouseID").Value = 150 And rst.Fields("fldPlanID").Value <> 3817 And rst.Fields("fldPlanID").Value <> 2899 And rst.Fields("fldPlanID").Value <> 2125 And strSVD_01 = "620" Then strSVD_01 = "11202"
    If rstClrHse.Fields("fldClearingHouseID").Value = 150 And rst.Fields("fldPlanID").Value <> 3817 And rst.Fields("fldPlanID").Value <> 2899 And rst.Fields("fldPlanID").Value <> 2125 And strSVD_01 = "401" Then strSVD_01 = "00401"
    If rstClrHse.Fields("fldClearingHouseID").Value = 150 And strSVD_01 = "0088" Then strSVD_01 = "14312"
    If rst.Fields("fldPatientID").Value = 942501 Then strSVD_01 = "X0C"
    If rst.Fields("fldPatientID").Value = 1062234 Then strSVD_01 = "X0M"

    strSVD_02 = IIf(IfNull(lngPayment, 0) = 0, 0, Format(IfNull(lngPayment, 0), "####0.00"))           'monetary amount paid
    strSVD_03 = "HC"                    'product or service qualifer
    If IfNull(rstDetail.Fields("fldMod1").Value, "") > "" Then
        If IfNull(rstDetail.Fields("fldMod2").Value, "") > "" Then
            strSVD_03 = strSVD_03 & g_ElementSeperator & rstDetail.Fields("fldCPTCode").Value & g_ElementSeperator & rstDetail.Fields("fldMod1").Value & g_ElementSeperator & rstDetail.Fields("fldMod2").Value
        Else
            strSVD_03 = strSVD_03 & g_ElementSeperator & rstDetail.Fields("fldCPTCode").Value & g_ElementSeperator & rstDetail.Fields("fldMod1").Value
        End If
    Else
        strSVD_03 = strSVD_03 & g_ElementSeperator & rstDetail.Fields("fldCPTCode").Value    'product/service ID/modifier1/modifier2
    End If
    strSVD_04 = ""                          'not used
    strSVD_05 = rstDetail.Fields("fldUnits").Value  'quantity (units or minutes)
        
    strSVD = "SVD*" & strSVD_01 & "*" & strSVD_02 & "*" & strSVD_03 & "*" & strSVD_04 & "*" _
            & strSVD_05 & "~"
            
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strSVD
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strSVD Else Print #1, strSVD;
    End If
    g_lngNumSeg = g_lngNumSeg + 1
                
   ' If lngDeductable + lngCoInsurance + IfNull(lngPayment, 0) <> IfNull(g_Allowed, 0) Then
   '         lngCoInsurance = Round((g_Allowed - lngPayment - lngDeductable), 2)
   ' End If
    If IfNull(lngDisallowance, 0) > 0 Then
        strCAS01 = "CO"
        strCAS02 = "45"
        strCAS03 = Format(IfNull(lngDisallowance, 0), "#####.00")
        strCAS = "CAS*" & strCAS01 & "*" & strCAS02 & "*" & strCAS03 & "~"
        If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
            g_PrtString = g_PrtString + strCAS
            If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
            End If
        Else
            If g_blnLineFeed Then Print #1, strCAS Else Print #1, strCAS;
        End If
        g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
        strCAS = ""
    End If
    
    If IfNull(lngDeductable, 0) > 0 Then
        strCAS01 = "PR"
        strCAS02 = "1"
        strCAS03 = Format(IfNull(lngDeductable, 0), "#####.00")
        strCAS04 = ""
        If IfNull(lngCoInsurance, 0) > 0 Then
            strCAS05 = "2"
            strCAS06 = Format(Round(IfNull(lngCoInsurance, 0), 2), "#####.00")
            strCAS07 = ""
            If IfNull(lngCoPay, 0) > 0 Then
               strCAS08 = "3"
               strCAS09 = Format(Round(IfNull(lngCoPay, 0), 2), "#####.00")
               strCAS10 = ""
               strCAS = "CAS*" & strCAS01 & "*" & strCAS02 & "*" & strCAS03 _
                            & "*" & strCAS04 & "*" & strCAS05 & "*" & strCAS06 & "*" & strCAS07 & "*" & strCAS08 & "*" & strCAS09 & "~"
            Else
               strCAS = "CAS*" & strCAS01 & "*" & strCAS02 & "*" & strCAS03 _
                            & "*" & strCAS04 & "*" & strCAS05 & "*" & strCAS06 & "~"
            End If
        Else
            strCAS = "CAS*" & strCAS01 & "*" & strCAS02 & "*" & strCAS03 & "~"
        End If
        If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
            g_PrtString = g_PrtString + strCAS
            If Len(g_PrtString) >= 80 Then
                Print #1, Left(g_PrtString, 80)
                g_PrtString = Mid(g_PrtString, 81)
            End If
        Else
            If g_blnLineFeed Then Print #1, strCAS Else Print #1, strCAS;
        End If
        g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
    Else
        If IfNull(lngCoInsurance, 0) > 0 Then
            strCAS01 = "PR"
            strCAS02 = "2"
            strCAS03 = Format(Round(IfNull(lngCoInsurance, 0), 2), "#####.00")
            strCAS04 = ""
            If IfNull(lngCoPay, 0) = lngAllowed And lngCoInsurance < lngAllowed Then
                strCAS05 = "1"
                strCAS06 = Format(Round(lngCoPay - lngCoInsurance, 2), "#####.00")
                strCAS07 = ""
                strCAS = "CAS*" & strCAS01 & "*" & strCAS02 & "*" & strCAS03 _
                            & "*" & strCAS04 & "*" & strCAS05 & "*" & strCAS06 & "~"
            Else
                strCAS = "CAS*" & strCAS01 & "*" & strCAS02 & "*" & strCAS03 & "~"
            End If
            If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
                rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
                g_PrtString = g_PrtString + strCAS
                If Len(g_PrtString) >= 80 Then
                    Print #1, Left(g_PrtString, 80)
                    g_PrtString = Mid(g_PrtString, 81)
                End If
            Else
                If g_blnLineFeed Then Print #1, strCAS Else Print #1, strCAS;
            End If
            g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
        End If
    End If
            
    'Date of Payment
    strDTP_01 = "573"                'date/time qualifier
    strDTP_02 = "D8"                 'date time period format qualifier
    strDTP_03 = Format(IfNull(IIf(dteCheckDate = 0, g_CheckDate, dteCheckDate), Now()), "yyyymmdd")              'date/time period in CCYYMMDD
    strDTP = "DTP*" & strDTP_01 & "*" & strDTP_02 & "*" & strDTP_03 & "~"
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
        rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strDTP
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strDTP Else Print #1, strDTP;
    End If
    g_lngNumSeg = g_lngNumSeg + 1
    
    Exit Sub
    
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub


Private Sub WriteTxSetTrailer(ByVal rstClrHse As ADODB.Recordset)
    
    'SE segment
    Dim strSE As String
        
    On Error GoTo ErrTrap:
    
    'Increment segment counter one more time - segment count includes the 'SE' (trailer) segment
    g_lngNumSeg = g_lngNumSeg + 1
    
    strSE = "SE*" & g_lngNumSeg & "*" & Format(CStr(g_lngEndTxNum), "00000000") & "~"
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 999 Or _
        rstClrHse.Fields("fldClearingHouseID").Value = 76 Then ' BcBs of VT
        g_PrtString = g_PrtString + strSE
        If Len(g_PrtString) >= 80 Then
            Print #1, Left(g_PrtString, 80)
            g_PrtString = Mid(g_PrtString, 81)
        End If
    Else
        If g_blnLineFeed Then Print #1, strSE Else Print #1, strSE;
    End If
    
    g_lngNumSeg = 0
    
    Exit Sub
        
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description
        
End Sub

Function CreatePayerCodePrefix(ByVal strInsType As String, ByVal strPayerCode As String) As String
    
    Dim strPayerPrefix As String
    
    On Error GoTo ErrTrap:
    
    Select Case strInsType
        Case "MP"
            strPayerPrefix = "C"
        Case "MB"
            strPayerPrefix = "C"
        Case "MC"
            strPayerPrefix = "D"
        Case "BL"
            strPayerPrefix = "G"
        Case "CH"
            strPayerPrefix = "H"
        Case Else 'Commercial Claims
            strPayerPrefix = "F"
    End Select
    If strPayerCode = "SB580" Or strPayerCode = "SB690" Then
            strPayerPrefix = "F"
    End If
    CreatePayerCodePrefix = strPayerPrefix

    Exit Function
    
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description
    
End Function

Private Function StripDelimiters(ByVal strData As String) As String

    Dim strTemp As String
    
    strTemp = strData 'Make a copy of the data
    strTemp = Replace(strTemp, ":", "", 1)
    strTemp = Replace(strTemp, "*", "", 1)
    strTemp = Replace(strTemp, "_", "", 1)
    strTemp = Replace(strTemp, "~", "", 1)
    strTemp = Replace(strTemp, ".", "", 1)
    strTemp = Replace(strTemp, "#", "", 1)
    strTemp = Replace(strTemp, "/", "", 1)
    strTemp = Replace(strTemp, "\", "", 1)
    strTemp = Replace(strTemp, "(", "", 1)
    strTemp = Replace(strTemp, ")", "", 1)
    strTemp = Replace(strTemp, "'", "", 1)
    strTemp = Replace(strTemp, "'", "", 1)
    strTemp = Replace(strTemp, "`", "", 1)
    strTemp = Replace(strTemp, "’", "", 1)
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
Private Function GetClaimFilingCode(ByVal strInsType As String)

    Dim strFilingCode As String

    Select Case strInsType
        Case "WC"
    '        strFilingCode = "WC"
            strFilingCode = "CI" 'Commercial Insurance
        Case "MP", "MB"
            strFilingCode = "MB"
        Case "MC"
            strFilingCode = "MC"
        Case "BL"
            strFilingCode = "BL"
        Case "CH"
            strFilingCode = "CH"
        Case "HM"
            strFilingCode = "HM"
        Case Else
            strFilingCode = "CI" 'Commercial Insurance
    End Select

    GetClaimFilingCode = strFilingCode

End Function


Private Function GetRelationCode(ByVal strPatRelat As String)
    
    On Error GoTo ErrTrap:
    
    Select Case UCase(strPatRelat)
        Case UCase("Self")
            GetRelationCode = "18"  '(18 = self)
        Case UCase("Spouse")
            GetRelationCode = "01"  '(01 = spouse)
        Case UCase("Child")
            GetRelationCode = "19"  '(19 = child)
        Case Else
            GetRelationCode = "G8"  '(G8 = other)
    End Select
    
    Exit Function
'1 Spouse
'4 Grandfather or Grandmother
'5 Grandson or Grandaughter
'7 Nephew or Niece
'10 Foster Child
'15 Ward of the Court
'17 Stepson or Stepdaughter
'18 Self
'19 Child
'20 Employee
'21 Unknown
'22 Handicapped/Dependent
'23 Sponsored Dependent
'24 Dependent of Minor Dependent
'29 Significant Other
'32 Mother
'33 Father
'36 Emancipated Minor
'39 Organ Donor
'40 Cadaver Donor
'41 Injured Plaintiff
'43 Child Where Insured Has No Financial Responsibility
'53 Life Partner
'G8 Other Relationship

ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Private Sub StoreCrossReference(ByVal lngClaimID As Long, ByVal lngEncounterLogID As Long, _
                                ByVal lngEFileID As Long, ByVal strEFileName As String, ByVal strDataBase As String)
'Stores the cross-reference number of the electronic claim in tblClaim, so that a reference
'between THIN and our system exists.
                    
    Dim objEDI As CEDIBz

    On Error GoTo ErrTrap:
    
    Set objEDI = New CEDIBz
    Call objEDI.EstablishXRef(lngClaimID, lngEncounterLogID, lngEFileID, strEFileName, strDataBase)
    Set objEDI = Nothing
    
    Exit Sub
                    
ErrTrap:
    Set objEDI = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description
                    
End Sub

Private Function FetchProviderPracticeNumber(ByVal lngEncounterLogID As Long, ByVal lngInsuranceID As Long, _
                                             ByVal lngTINType As Long) As String
'Returns the Provider ID number for a given payer.  The TIN Type determines whether the individual
'Provider number or Group provider number is returned.

    Dim objProv As ProviderBz.CProviderBZ
    Dim objEL As EncounterLogBz.CEncounterLogBz
    Dim rst As ADODB.Recordset
    Dim intCtr As Integer
    Dim lngProviderID As Long
    Dim strProvNumber As String
    Dim strTempNumber As String
    
    On Error GoTo ErrTrap:
    
    'DR: 06/06/2003: This is ineffecient but necessary - fldProviderID does not exist in tblClaim ?!
    'This is far less work than restructering all the claim logic.
    Set objEL = CreateObject("EncounterLogBz.CEncounterLogBz")
    Set rst = objEL.FetchByID(lngEncounterLogID)
    lngProviderID = rst.Fields("fldProviderID").Value
    Set objEL = Nothing
    Set rst = Nothing
    
    Set objProv = CreateObject("ProviderBz.CProviderBZ")
    Set rst = objProv.FetchPracticeNumbers(lngProviderID)
    Set objProv = Nothing
    
    For intCtr = 1 To rst.RecordCount
        If rst.Fields("fldInsuranceID").Value = lngInsuranceID Then
            'If provider chooses wrong Tax ID, provide a BC/BS number
            strTempNumber = rst.Fields("fldPracticeNumber").Value
            
            If lngTINType = 3 Then 'Group number
                If rst.Fields("fldIsGroupYN").Value = "Y" Then
                    strProvNumber = rst.Fields("fldPracticeNumber").Value
                    Exit For
                End If
            Else
                If rst.Fields("fldIsGroupYN").Value = "N" Then
                    strProvNumber = rst.Fields("fldPracticeNumber").Value
                    Exit For
                End If
            End If
        End If
        
        rst.MoveNext
    Next intCtr
    
    Set rst = Nothing
    
    If strProvNumber = "" Then
        strProvNumber = strTempNumber
    End If
    
    FetchProviderPracticeNumber = strProvNumber
    
    Exit Function
                    
ErrTrap:
    Set objProv = Nothing
    Set rst = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description
                                             
End Function



