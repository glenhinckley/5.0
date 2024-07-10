
Option Explicit

Private g_lngNumSeg As Long 'Data segment counter
Private g_lngEndTxNum As Long
Private g_FileNumber As Long
Private g_FileName As String

Private Const INSCO_MEDICARE As Long = 24
Private Const INSCO_MEDICARE_PARTB As Long = 29
Private Const INSCO_MEDICAID As Long = 105

'Private Const INSCO_BCBS As Long = 158
Private Const INSCO_BCBS_TX As Long = 1309
Private Const INSCO_BCBS_IL As Long = 1280
Private Const INSCO_BCBS_NM As Long = 1298

Private Const INSCO_CHAMPUS As Long = 1050

Module ediins

    Public Function GenerateElectronicClaims(ByVal rst As ADODB.Recordset, ByVal lngEndTxNum As Long, ByVal lngFileNumber As Long, ByVal strFileName As String) As Long
        '-------------------------------------------------------------------------------
        'Author: Dave Richkun
        'Date: 11/07/2002
        'Description: Main function that generates EDI text file compliant to X.12 837 v4010
        'Parameters: rst - Recordset of claims to be electronically generated
        'Returns: Null
        '-------------------------------------------------------------------------------


        Dim strPayerCode As String
        Dim strPayerCodeTmp As String
        Dim lngNumClaims As Long

        g_lngEndTxNum = lngEndTxNum
        g_FileNumber = lngFileNumber
        g_FileName = strFileName

        Screen.MousePointer = vbHourglass

        lngNumClaims = 0

        If rst.RecordCount > 0 Then
            strPayerCode = IfNull(rst.Fields("fldPayerCode").Value, "MIXED")  'or is it ZMIXED?
            Call WriteISAHeader(rst.Fields("fldInsuranceID").Value)
            Call WriteGroupHeader(rst.Fields("fldInsuranceID").Value, strPayerCode)

            Do While Not rst.EOF
                strPayerCode = IfNull(rst.Fields("fldPayerCode").Value, "MIXED")  'or is it ZMIXED?
                Call WriteTxSetHeader()
                Call WriteClaim(rst, strPayerCode)
                Call WriteTxSetTrailer()
                Call StoreCrossReference(rst.Fields("fldClaimID").Value, rst.Fields("fldEncounterLogID").Value, g_lngEndTxNum, g_FileName)

                lngNumClaims = lngNumClaims + 1
                g_lngEndTxNum = g_lngEndTxNum + 1 'According to THIN, each BHT and ST/SE must have a unique identifier

                frmEDIWizard.barStatus.Value = lngNumClaims
                frmEDIWizard.barStatus.Refresh()

                If Not rst.EOF Then
                    rst.MoveNext()
                Else
                    Exit Do
                End If
            Loop

            If rst.EOF Then
                Call WriteGroupTrailer(lngNumClaims)
                Call WriteISATrailer()
            End If

            rst = Nothing
        End If

        'Return number of claims to calling procedure
        GenerateElectronicClaims = g_lngEndTxNum

        Screen.MousePointer = vbNormal

        Exit Function

ErrTrap:
        Screen.MousePointer = vbNormal
        GenerateElectronicClaims = -1
        MsgBox("Error: " & Err.Number & " - " & Err.Description, vbCritical, "Error in file export")

    End Function


    Private Sub WriteISAHeader(ByVal lngInsuranceID As Long)
        'Writes ISA (Interchange Control Header) for file

        Dim strISA As String
        Dim strISA01, strISA02, strISA03, strISA04, strISA05 As String
        Dim strISA06, strISA07, strISA08, strISA09, strISA10 As String
        Dim strISA11, strISA12, strISA13, strISA14, strISA15, strISA16 As String
        Dim dteCreateDate As Date



        dteCreateDate = Now

        'ISA (Interchange Control Header)
        'Note: The ISA header is a fixed length segment
        strISA01 = "00"               'authorization information qualifier
        strISA02 = "          "       'authorization information
        strISA03 = "01"               'Security information qualifier
        strISA04 = "V06218    "       'Security information
        strISA05 = "ZZ"               'interchange ID qualifier (ZZ = mutually defined)
        strISA06 = "S03588         "  'interchange sender ID (length must be 15)
        strISA07 = "ZZ"               'interchange ID qualifier (ZZ = mutually defined)

        'If the file contains multiple payers then ISA108 should read ZMIXED (padded to 15 characters)
        '    If Me.optMultiple(0).Value = True Then
        strISA08 = "ZMIXED         "
        '    Else
        '        strISA08 = RPad(CreatePayerCodePrefix(lngInsuranceID) & Trim(Me.txtPayorCode.Text), 15 - Len(Me.txtPayorCode.Text) - 1)
        '    End If

        strISA09 = Format(dteCreateDate, "yymmdd")    'interchange date
        strISA10 = Format(Now(), "HHMM")              'interchange time
        strISA11 = "U"                'interchange control standards identifier
        strISA12 = "00401"            'interchange control version number
        strISA13 = Format(g_FileNumber, "000000000")   'interchange control number
        strISA14 = "1"                'acknowledgment requested (0 = no ack requested; 1 ack requested)

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

    Print #1, strISA

        Exit Sub

ErrTrap:
        Err.Raise(Err.Number, Err.Source, Err.Description)

    End Sub

    Private Sub WriteGroupHeader(ByVal lngInsuranceID As Long, ByVal strPayerCode As String)
        'Writes functional Group Header to EDI file

        Dim strGS As String
        Dim strGS01, strGS02, strGS03, strGS04 As String
        Dim strGS05, strGS06, strGS07, strGS08 As String
        Dim dteCreateDate As Date

        On Error GoTo ErrTrap

        'GS (Functional Group Header)
        strGS01 = "HC"                 'functional identifier code
        strGS02 = "S03588"   '"V06218" 'application sender's code changed from V code to S code 05/05/2003 per THIN

        '    If Me.optMultiple(0).Value = True Then
        strGS03 = "ZMIXED"
        '    Else
        '        strGS03 = CreatePayerCodePrefix(lngInsuranceID) & strPayerCode 'application receiver'S code
        '    End If

        strGS04 = Format(dteCreateDate, "yyyymmdd")        'date
        strGS05 = Format(dteCreateDate, "hhnn")            'time
        strGS06 = g_FileNumber  'group control number
        strGS07 = "X"                  'responsible agency code
        strGS08 = "004010X098"         'version/release/industry identifier code

        strGS = "GS*" & strGS01 & "*" & strGS02 & "*" & strGS03 & "*" & strGS04 _
             & "*" & strGS05 & "*" & strGS06 & "*" & strGS07 & "*" & strGS08 & "~"

    Print #1, strGS

        Exit Sub

ErrTrap:
        Err.Raise(Err.Number, Err.Source, Err.Description)

    End Sub
    Private Sub WriteClaim(ByVal rst As ADODB.Recordset, ByVal strPayerCode As String)

        Dim strBHT As String
        Dim strSvc As String
        Dim strRef As String
        Dim strTempCodePrefix As String
        Dim strPRV As String

        strTempCodePrefix = CreatePayerCodePrefix(rst.Fields("fldInsType").Value)

        Call WriteLoop1000A() 'Submitter information
        Call WriteLoop1000B(rst, strPayerCode) 'Receiver information

        Call WriteLoop2000A() 'Heirarchy level (Billing Provider)
        Call WriteLoop2010AA(rst) 'Billing Provider information

        Call WriteLoop2000B(rst) 'Heirarchy level (Subscriber)
        Call WriteLoop2010BA(rst) 'Subscriber information

        Call WriteLoop2010BB(rst, strPayerCode) 'Payer information

        'Medicare and Medicaid do not allow any relationship but self
        If rst.Fields("fldPatientID").Value <> rst.Fields("fldRPID").Value And _
            rst.Fields("fldInsType").Value <> "MP" And _
            rst.Fields("fldInsType").Value <> "MB" And _
            rst.Fields("fldInsType").Value <> "MC" Then
            'Patient information.  This loop is written only if the patient is not the
            'insured/subscriber.  Otherwise the patient and insured/subscriber information
            'is identical and has already been written in Loop 2010BA.
            Call WriteLoop2000C(rst) 'Patient information
            Call WriteLoop2000CA(rst) 'Patient name, demographibs
        End If

        Call WriteLoop2300(rst) 'Claim information

        If rst.Fields("fldReferPhy").Value > "" Then
            Call WriteLoop2310A(rst) 'Referring Physician
        End If

        Call WriteLoop2310B(rst) 'Rendering Provider information

        If rst.Fields("fldPOS").Value <> "11" Then
            Call WriteLoop2310D(rst) 'Facility Provider
        End If

        Call WriteLoop2400(rst) 'Service information

    End Sub

    Private Sub WriteISATrailer()

        On Error GoTo ErrTrap

        'ISA footer
        Dim strIEA As String
        strIEA = "IEA*1*" & Format(g_FileNumber, "000000000") & "~"

    Print #1, strIEA

        Exit Sub
ErrTrap:
        Err.Raise(Err.Number, Err.Source, Err.Description)

    End Sub

    Private Sub WriteGroupTrailer(ByVal intNumClaims As Integer)

        On Error GoTo ErrTrap

        'GS footer
        Dim strGE As String

        strGE = "GE*" & CStr(intNumClaims) & "*" & CStr(g_FileNumber) & "~"

    Print #1, strGE

        Exit Sub

ErrTrap:
        Err.Raise(Err.Number, Err.Source, Err.Description)

    End Sub

    Private Sub WriteTxSetHeader()
        'Writes header information for a claim. This method writes the ST, BHT, and REF
        'data segments of the header.

        On Error GoTo ErrTrap

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

        strBHT01 = "0019"                              'hierarchical structure code
        strBHT02 = "00"                                'transaction set purpose code
        strBHT03 = Format(g_lngEndTxNum, "000000")  'reference identification
        strBHT04 = Format(dteCreateDate, "yyyymmdd")   'date
        strBHT05 = Format(dteCreateDate, "hhnn")       'time
        strBHT06 = "CH"       'transaction type code (CH = Chargable /RP = Reporting)

        strBHT = "BHT*" & strBHT01 & "*" & strBHT02 & "*" & strBHT03 _
             & "*" & strBHT04 & "*" & strBHT05 & "*" & strBHT06 & "~"

        'REF (transmission type identification) 'Note: when in production mode value is 004010X098;
        strRef = "REF*87*004010X098~"

    Print #1, strST
        g_lngNumSeg = g_lngNumSeg + 1 'increment segment counter

    Print #1, strBHT
        g_lngNumSeg = g_lngNumSeg + 1

    Print #1, strRef
        g_lngNumSeg = g_lngNumSeg + 1

        Exit Sub

ErrTrap:
        Err.Raise(Err.Number, Err.Source, Err.Description)

    End Sub

    Private Function WriteLoop1000A()
        'Writes Submitter information

        Dim strNM101, strNM102, strNM103, strNM104, strNM105 As String
        Dim strNM106, strNM107, strNM108, strNM109, strNM110, strNM111 As String
        Dim strNM1, strN2 As String
        Dim strPer As String
        Dim strPer01, strPer02, strPer03, strPer04, strPer05, strPer06 As String

        'Submitter Name
        strNM101 = "41"         'entity identifier code (41 = submitter)
        strNM102 = "2"          'Qualifier - 2:Non-person entity
        strNM103 = "PSYQUEL LP" 'Submitter name
        strNM104 = ""           'Individual first name
        strNM105 = ""           'Individual last name
        strNM106 = ""        'not used (name prefix)
        strNM107 = ""        'not used (name suffix)
        strNM108 = "46"      'identification code qualifier (46 = ETIN)
        strNM109 = "S03588" 'Psyquel Submitter ID

        strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 & "*" & strNM104 _
             & "*" & strNM105 & "*" & strNM106 & "*" & strNM107 & "*" & strNM108 & "*" & strNM109 & "~"

    Print #1, strNM1
        g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters

        'Submitter Contact Information
        strPer01 = "IC"                    'contact function code (IC = Information Contact)
        strPer02 = "PSYQUEL LP"            'Submitter contact name
        strPer03 = "TE"                    'communication number qualifier (TE = Telephone)
        strPer04 = "2106941354"            'communication number (phone should be XXXYYYZZZZ)
        strPer05 = "FX"                    'communication number qualifier (FX = Fax)
        strPer06 = "2106941399"            'communication number (phone should be XXXYYYZZZZ)

        strPer = "PER*" & strPer01 & "*" & strPer02 & "*" & strPer03 & "*" & strPer04 & "*" & strPer05 & "*" & strPer06 & "~"

    Print #1, strPer
        g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters

        Exit Function

ErrTrap:
        Err.Raise(Err.Number, Err.Source, Err.Description)

    End Function


    Private Function WriteLoop1000B(ByVal rst As ADODB.Recordset, ByVal strPayerCode As String) As Boolean
        'Writes Receiver information

        Dim strNM101, strNM102, strNM103, strNM104, strNM105 As String
        Dim strNM106, strNM107, strNM108, strNM109, strNM110, strNM111 As String
        Dim strNM1, strN2 As String

        On Error GoTo ErrTrap

        strNM101 = "40"            'entity ID code (40 = receiver)
        strNM102 = "2"             'entity type qualifier (2 = non-person)
        strNM103 = StripDelimiters(Mid(rst.Fields("fldPlanName").Value, 1, 35))  'last name or organization
        strNM104 = ""              'first name (optional)
        strNM105 = ""              'middle name (optional)
        strNM106 = ""              'name prefix (optional)
        strNM107 = ""              'name suffix (optional)
        strNM108 = "46"            'ID code qualifier (46 = ETIN)

        'Since we are only creating one claim per ST then the ST is not mixed!
        '    If strPayerCode = "ZMIXED" Then
        '        strNM109 = strPayerCode
        '    Else
        strNM109 = CreatePayerCodePrefix(rst.Fields("fldInsType").Value) & strPayerCode
        '    End If

        'If the receiver name is more than 35 characters then we use segment N2 to hold the
        'overflow characters
        If Len(rst.Fields("fldPlanName").Value) > 35 Then
            strN2 = "N2*" & Mid(rst.Fields("fldPlanName").Value, 36) & "~"
        Else
            strN2 = ""
        End If

        strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
            & "*" & strNM104 & "*" & strNM105 & "*" & strNM106 & "*" & strNM107 _
            & "*" & strNM108 & "*" & strNM109 & "~"

    Print #1, strNM1
        g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters

        If strN2 > "" Then
        Print #1, strN2  'Print overflow line
            g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
        End If

        Exit Function

ErrTrap:
        Err.Raise(Err.Number, Err.Source, Err.Description)

    End Function
    Private Sub WriteLoop2000A()
        'Writes heirarchy level (this is always level 1) and specialty information (mental
        'health claims)

        Dim strHL, strHL01, strHL02, strHL03, strHL04 As String
        Dim strPRV As String

        strHL01 = "1"              'hierarchical ID number (see pg. 77 of X098.pdf)
        strHL02 = ""               'hierarchical parent ID
        strHL03 = "20"             'hierarchical level code(20 = information source)
        strHL04 = "1"              'hierarchical child code

        strHL = "HL*" & strHL01 & "*" & strHL02 & "*" & strHL03 & "*" & strHL04 & "~"

    Print #1, strHL
        g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters

        'Specialty information
        strPRV = "PRV*BI*ZZ*101Y00000X~"  'ZZ relies on Provider Taxonomy Code published by BC/BS Association

    Print #1, strPRV
        g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters


    End Sub
    Private Function WriteLoop2010AA(ByVal rst As ADODB.Recordset) As Boolean
        'Writes Billing Provider information

        Dim strNM101, strNM102, strNM103, strNM104, strNM105 As String
        Dim strNM106, strNM107, strNM108, strNM109, strNM110, strNM111 As String
        Dim strN401, strN402, strN403 As String
        Dim strNM1, strN2, strN3, strN4 As String
        Dim strRef, strRef01, strRef02 As String

        On Error GoTo ErrTrap

        strNM101 = "85"        'entity code identifier (85 - billing provider)
        strNM102 = "1"         'entity type qualifier (1=person; 2=non-person)

        If rst.Fields("fldUseSuperYN").Value = "Y" Then
            strNM103 = IfNull(rst.Fields("fldSuperLastName").Value, Mid(Replace(rst.Fields("fldProviderLastName").Value, "'", ""), 1, 35))     'Last name
            strNM104 = IfNull(rst.Fields("fldSuperLastName").Value, Mid(Replace(rst.Fields("fldProviderFirstName").Value, "'", ""), 1, 35))   'first name
            strNM105 = IfNull(rst.Fields("fldSuperLastName").Value, IfNull(rst.Fields("fldSuperMI").Value, ""))   'middle name
            strNM106 = ""          'name prefix (not used)
            strNM107 = ""          'name suffix (not used)
        Else
            If IsNull(rst.Fields("fldGrpPracticeNum").Value) Then
                strNM103 = Mid(Replace(IfNull(rst.Fields("fldProviderLastName").Value, ""), "'", ""), 1, 35)          'Last name
                strNM104 = Mid(Replace(IfNull(rst.Fields("fldProviderFirstName").Value, ""), "'", ""), 1, 35)   'first name
                strNM105 = IfNull(rst.Fields("fldProviderMI").Value, "")   'middle name
                strNM106 = ""          'name prefix (not used)
                strNM107 = ""          'name suffix (not used)
            Else
                strNM102 = "2"         'entity type qualifier (1=person; 2=non-person)
                strNM103 = Mid(Replace(IfNull(rst.Fields("fldGroupName").Value, ""), "'", ""), 1, 35)          'Last name
                strNM104 = ""           'first name
                strNM105 = ""       'middle name
                strNM106 = ""          'name prefix (not used)
                strNM107 = ""          'name suffix (not used)
            End If
        End If

        'identification code qualifier
        If rst.Fields("fldTinType").Value = 3 Then
            strNM108 = "34"        'individual SSN
        Else
            strNM108 = "24"        '24 = employer'S ID
        End If

        strNM109 = NumbersOnly(rst.Fields("fldTIN").Value)         'billing provider identifier code

        strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 & "*" _
             & strNM104 & "*" & strNM105 & "*" & strNM106 & "*" _
             & strNM107 & "*" & strNM108 & "*" & strNM109 & "~"

    Print #1, strNM1
        g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters

        'If the billing provider name is more than 35 characters then we need to use
        'Segment N2 to hold the overflow characters
        If rst.Fields("fldUseSuperYN").Value = "Y" Then
            If Len(IfNull(rst.Fields("fldSuperLastName").Value, Mid(Replace(rst.Fields("fldProviderLastName").Value, "'", ""), 1, 35))) > 35 Then
                'N2 segment (Additional receiver name information)
                'This segment is used if the length of strNM103 is more than 35
                strN2 = "N2*" & Mid(Replace(rst.Fields("fldSuperLastName").Value, "'", ""), 36) & "~"

            Print #1, strN2
                g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
            End If
        Else
            If Len(Replace(IfNull(rst.Fields("fldProviderLastName").Value, ""), "'", "")) > 35 Then
                'N2 segment (Additional receiver name information)
                'This segment is used if the length of strNM103 is more than 35
                strN2 = "N2*" & Mid(Replace(IfNull(rst.Fields("fldProviderLastName").Value, ""), "'", ""), 36) & "~"

            Print #1, strN2
                g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
            End If
        End If


        'Provider address
        strN3 = "N3*" & StripDelimiters(rst.Fields("fldFacilityLine2").Value) & "~"
    Print #1, strN3
        g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters

        strN401 = rst.Fields("fldFacilityCity").Value   'city
        strN402 = rst.Fields("fldFacilityState").Value  'State code (2 digits)
        strN403 = rst.Fields("fldFacilityZip").Value    'postal code

        strN4 = "N4*" & strN401 & "*" & strN402 & "*" & strN403 & "~"
    Print #1, strN4
        g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
        'Add the billing provider secondary identification
        If rst.Fields("fldInsType").Value = "MP" Then
            strRef01 = "1C"
            strRef02 = IfNull(rst.Fields("fldGrpPracticeNum").Value, IfNull(rst.Fields("fldIndPracticeNum").Value, ""))
        ElseIf rst.Fields("fldInsType").Value = "MB" Then
            strRef01 = "1C"
            strRef02 = IfNull(rst.Fields("fldGrpPracticeNum").Value, IfNull(rst.Fields("fldIndPracticeNum").Value, ""))
        ElseIf rst.Fields("fldInsType").Value = "MC" Then
            strRef01 = "1D"
            strRef02 = IfNull(rst.Fields("fldGrpPracticeNum").Value, IfNull(rst.Fields("fldIndPracticeNum").Value, ""))
        ElseIf rst.Fields("fldInsType").Value = "BL" Then  'BC/BS
            strRef01 = "1B"
            strRef02 = IfNull(rst.Fields("fldGrpPracticeNum").Value, IfNull(rst.Fields("fldIndPracticeNum").Value, ""))
            'For REF01: 1A-Blue Cross; 1B-Blue Shield.  See page 98 of EDI specification
            'We will record both numbers if payer name is not specific to Blue Cross or Blue Shield

        ElseIf rst.Fields("fldInsType").Value = "CH" Then
            strRef01 = "1H"
            strRef02 = IfNull(rst.Fields("fldGrpPracticeNum").Value, IfNull(rst.Fields("fldIndPracticeNum").Value, ""))
        Else
            'Commercial Claims
            strRef01 = "G2"
            strRef02 = IfNull(rst.Fields("fldGrpPracticeNum").Value, IfNull(rst.Fields("fldIndPracticeNum").Value, ""))
        End If
        If strRef02 <> strNM109 Then
            If strRef02 > "" Then
                strRef = "REF*" & strRef01 & "*" & strRef02 & "~"
            Print #1, strRef
                g_lngNumSeg = g_lngNumSeg + 1
            End If
        End If

        Exit Function

ErrTrap:
        Err.Raise(Err.Number, Err.Source, Err.Description)

    End Function
    Private Sub WriteLoop2000B(ByVal rst As ADODB.Recordset)
        'Writes heirarchy level (this level is always '2').

        Dim strHL, strHL01, strHL02, strHL03, strHL04 As String
        Dim strSBR, strSBR01, strSBR02, strSBR03, strSBR04 As String
        Dim strSBR05, strSBR06, strSBR07, strSBR08, strSBR09 As String

        strHL01 = "2"           'hierarchical ID number - always 2 (see p. 108 of X098.pdf)
        strHL02 = "1"           'hierarchical parent ID
        strHL03 = "22"          'hierarchical level code (22 = subscriber)

        If rst.Fields("fldPatientID").Value <> rst.Fields("fldRPID").Value Then
            strHL04 = "1"           'we have subordinate in hierarchical child code
        Else
            strHL04 = "0"           'hierarchical child code
        End If

        strHL = "HL*" & strHL01 & "*" & strHL02 & "*" & strHL03 & "*" & strHL04 & "~"

        'SBR (subscriber information)
        If rst.Fields("fldVersion").Value = 1 Then
            strSBR01 = "P"             'payer resp. sequence number code (see p. 109)
        ElseIf rst.Fields("fldVersion").Value = 2 Then
            strSBR01 = "S"
        ElseIf rst.Fields("fldVersion").Value > 2 Then
            strSBR01 = "T"
        End If

        If rst.Fields("fldRPID").Value = rst.Fields("fldPatientID").Value Then
            strSBR02 = "18"              'individual relationship code (18 = self)
        Else
            strSBR02 = ""
        End If

        strSBR03 = StripDelimiters(IfNull(rst.Fields("fldGroupNum").Value, IIf(rst.Fields("fldInsType").Value = "MB", "NONE", IIf(rst.Fields("fldInsType").Value = "MP", "NONE", "")))) 'Subscriber group or policy number
        strSBR04 = StripDelimiters(rst.Fields("fldPlanName").Value)               'insured group name
        'insurance type code applies only to Medicare where Medicare is secondary payer
        If rst.Fields("fldInsType").Value = "MP" Then
            strSBR05 = GetClaimFilingCode(rst.Fields("fldInsType").Value)
        ElseIf rst.Fields("fldInsType").Value = "MB" Then
            strSBR05 = "MB"
        ElseIf rst.Fields("fldInsType").Value = "MC" Then
            strSBR05 = "MC"
        ElseIf rst.Fields("fldInsType").Value = "BL" Then 'BC/BS
            strSBR05 = "BL"
        ElseIf rst.Fields("fldInsType").Value = "CH" Then
            strSBR05 = "CH"
        ElseIf rst.Fields("fldInsType").Value = "WC" Then
            strSBR05 = "WC"
        Else
            'Commercial Claims
            strSBR05 = "CI"
        End If
        strSBR05 = ""              'TODO: insurance type code applies only to Medicare where Medicare is secondary payer
        strSBR06 = ""              'not used
        strSBR07 = ""              'not used
        strSBR08 = ""              'not used

        strSBR09 = GetClaimFilingCode(rst.Fields("fldInsType").Value)

        strSBR = "SBR*" & strSBR01 & "*" & strSBR02 & "*" & strSBR03 _
             & "*" & strSBR04 & "*" & strSBR05 & "*" & strSBR06 _
             & "*" & strSBR07 & "*" & strSBR08 & "*" & strSBR09 & "~"

    Print #1, strHL
        g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters

    Print #1, strSBR
        g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters

    End Sub
    Private Function WriteLoop2010BA(ByVal rst As ADODB.Recordset) As Boolean
        'Writes Subscriber information

        Dim strNM101, strNM102, strNM103, strNM104, strNM105 As String
        Dim strNM106, strNM107, strNM108, strNM109, strNM110, strNM111 As String
        Dim strNM1, strN2, strN3, strN4 As String
        Dim strDMG, strRef As String
        Dim dtDOB As Date

        On Error GoTo ErrTrap

        strNM101 = "IL"                    'entity identifier code (IL = insured)
        strNM102 = "1"                     'entity type qualifier (1=person)
        If rst.Fields("fldInsType").Value = "MP" Or _
            rst.Fields("fldInsType").Value = "MB" Or _
            rst.Fields("fldInsType").Value = "MC" Then
            strNM103 = Mid(Replace(IfNull(rst.Fields("fldPatientLastName").Value, ""), "'", ""), 1, 35)    'Last name
            strNM104 = Mid(Replace(IfNull(rst.Fields("fldPatientFirstName").Value, ""), "'", ""), 1, 35)   'first name
            strNM105 = IfNull(rst.Fields("fldPatientMI").Value, "")   'middle name
        Else
            strNM103 = IfNull(rst.Fields("fldInsuredLastName").Value, Mid(Replace(rst.Fields("fldPatientLastName").Value, "'", ""), 1, 35))     'Last name
            strNM104 = IfNull(rst.Fields("fldInsuredFirstName").Value, Mid(Replace(rst.Fields("fldPatientFirstName").Value, "'", ""), 1, 35))   'first name
            strNM105 = IfNull(rst.Fields("fldInsuredMI").Value, IfNull(rst.Fields("fldPatientMI").Value, ""))   'middle name
        End If
        strNM106 = ""                      'name prefix (not used)
        strNM107 = ""                      'name suffix
        strNM108 = "MI"                    'Identification code qualifier (MI=Member Identification Number)
        strNM109 = StripDelimiters(IfNull(rst.Fields("fldCardNum").Value, "00")) 'identification code

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

    Print #1, strNM1
        g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters

        If strN2 <> "" Then
        Print #1, strN2 'Print overflow line
            g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
        End If

        'Subscriber address
        strN3 = "N3*" & StripDelimiters(rst.Fields("fldInsuredStreetNum").Value) & "~"
    Print #1, strN3
        g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters

        strN4 = "N4*" & rst.Fields("fldInsuredCity").Value & "*" & rst.Fields("fldInsuredState").Value & "*" & NumbersOnly(rst.Fields("fldInsuredZip").Value) & "~"
    Print #1, strN4
        g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters

        'Subscriber demographics
        If IsNull(rst.Fields("fldInsdDOB").Value) Then
            dtDOB = 0
        Else
            dtDOB = rst.Fields("fldInsdDOB").Value
        End If

        'Subscriber DOB - If the DOB is not known then this line must be ignored otherwise validation errors occur
        If IsDate(dtDOB) And dtDOB > 0 Then
            strDMG = "DMG*D8*" & Year(dtDOB) & Format(Month(dtDOB), "00") & Format(Day(dtDOB), "00") & "*"
            strDMG = strDMG & IfNull(rst.Fields("fldInsdSex").Value, "U") & "~"
        Print #1, strDMG
            g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
        End If

        'Secondary number to identify subscriber
        If IfNull(rst.Fields("fldGroupNum").Value, "") > "" Then
            strRef = "REF*IG*" & StripDelimiters(rst.Fields("fldGroupNum").Value) & "~"
        Print #1, strRef
            g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
        End If

        Exit Function

ErrTrap:
        Err.Raise(Err.Number, Err.Source, Err.Description)

    End Function
    Private Sub WriteSegmentN3Payer(ByVal strAddress1 As String, ByVal strAddress2 As String)

        'payer address
        Dim strN3_01, strN3_02, strN3 As String

        On Error GoTo ErrTrap


        strN3_01 = strAddress1
        strN3_02 = strAddress2

        If strN3_02 = "" Then
            strN3 = "N3*" & StripDelimiters(strN3_01) & "~"
        Else
            strN3 = "N3*" & StripDelimiters(strN3_01) & "*" & StripDelimiters(strN3_02) & "~"
        End If

    Print #1, strN3

        Exit Sub

ErrTrap:
        Err.Raise(Err.Number, Err.Source, Err.Description)

    End Sub
    Private Function WriteLoop2010BB(ByVal rst As ADODB.Recordset, ByVal strPayerCode As String) As Boolean
        'Payer information

        On Error GoTo ErrTrap

        Dim strNM101, strNM102, strNM103, strNM104, strNM105 As String
        Dim strNM106, strNM107, strNM108, strNM109, strNM110, strNM111 As String
        Dim strNM1, strN2, strN3, strN4 As String

        'Loop 2010BB - Payer Name
        strNM101 = "PR"                'entity identifier code (PR = payer)
        strNM102 = "2"                 'entity type qualifier (2 = non-person)
        strNM103 = Mid(StripDelimiters(IfNull(rst.Fields("fldInsName").Value, "")), 1, 35)       'insurance company name
        strNM104 = ""                  'first name
        strNM105 = ""                  'middle name
        strNM106 = ""                  'name prefix (not used)
        strNM107 = ""                  'name suffix
        strNM108 = "PI"                'identification code qualifier

        If strPayerCode = "ZMIXED" Then
            If rst.Fields("fldInsType").Value = "WC" Then
                strNM109 = "TWCCP"
            Else
                strNM109 = CreatePayerCodePrefix(rst.Fields("fldInsType").Value) & IfNull(rst.Fields("fldPayerCode").Value, "FPRINT")
            End If
        Else
            strNM109 = CreatePayerCodePrefix(rst.Fields("fldInsType").Value) & strPayerCode
        End If

        strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
             & "*" & strNM104 & "*" & strNM105 & "*" & strNM106 _
             & "*" & strNM107 & "*" & strNM108 & "*" & strNM109 _
             & "~"

        'If the payer name is more than 35 characters then we use
        'N2 to hold the overflow characters
        If Len(StripDelimiters(rst.Fields("fldInsName").Value)) > 35 Then
            strN2 = "N2*" & Mid(StripDelimiters(rst.Fields("fldInsName").Value), 36) & "~"
        End If

    Print #1, strNM1
        g_lngNumSeg = g_lngNumSeg + 1

        If strN2 <> "" Then
        Print #1, strN2
            g_lngNumSeg = g_lngNumSeg + 1
        End If

        'Payer address
        strN3 = "N3*" & StripDelimiters(rst.Fields("fldInsAddress1").Value) & "~"
    Print #1, strN3
        g_lngNumSeg = g_lngNumSeg + 1

        strN4 = "N4*" & rst.Fields("fldInsCity").Value & "*" & rst.Fields("fldInsState").Value & "*" & NumbersOnly(rst.Fields("fldInsZip").Value) & "~"
    Print #1, strN4
        g_lngNumSeg = g_lngNumSeg + 1

        Exit Function

ErrTrap:
        Err.Raise(Err.Number, Err.Source, Err.Description)
    End Function
    Private Sub WriteLoop2300(ByVal rst As ADODB.Recordset)
        'Claim information

        'Loop 2300 - Claim Level Information
        Dim strClaim, strClm01, strClm02, strClm03 As String
        Dim strClm04, strClm05, strClm06 As String
        Dim strClm07, strClm08, strClm09 As String
        Dim strClm10, strClm11, strClm12 As String
        Dim strClm05_1, strClm05_2, strClm05_3 As String
        Dim strClm11_1, strClm11_2, strClm11_3, strClm11_4, strClm11_5 As String
        Dim strDTP, strDTP_01, strDTP_02, strDTP_03 As String
        Dim strHI As String

        On Error GoTo ErrTrap

        strClm01 = rst.Fields("fldEncounterLogID").Value   'our account number
        strClm02 = rst.Fields("fldFee").Value         'total claim charge amount
        strClm03 = ""             'not used
        strClm04 = ""             'not used

        'Medicaid specific: For electronic claims, we must undo the numbering sequence
        'that was coded in for paper-based claims
        Select Case rst.Fields("fldPOS").Value            'facility type code
            Case 1
                strClm05_1 = "11"
            Case 2
                strClm05_1 = "12"
            Case 3
                strClm05_1 = "21"
            Case Else
                strClm05_1 = rst.Fields("fldPOS").Value
        End Select

        strClm05_2 = ""           'not used
        strClm05_3 = "1"          'claim frequency code
        strClm06 = "Y"            'provider signature on file?
        strClm07 = "A"            'provider accept assignment code
        strClm08 = "Y"            'assignment of benefits indicator
        strClm09 = "Y"            'release of information code
        strClm10 = "B"            'patient signature source code

        'Sub-elements CLM11-1 to 11-5 pertain to Auto Accident (AA), Other Accident (OA), Employment (EM), etc
        If IfNull(rst.Fields("fldCondEmployYN").Value, "") = "Y" Then
            strClm11_1 = "EM"
        End If

        If IfNull(rst.Fields("fldCondAutoYN").Value, "") = "Y" Then
            strClm11_1 = "AA"
        End If

        If IfNull(rst.Fields("fldCondOtherYN").Value, "") = "Y" Then
            strClm11_1 = "OA"
        End If
        strClm11_2 = ""
        strClm11_3 = ""

        If strClm11_1 = "AA" Then
            strClm11_4 = rst.Fields("fldCondAutoState").Value
        End If

        strClm11 = strClm11_1 & ":" & strClm11_2 & ":" & strClm11_3 & ":" & strClm11_4

        strClaim = "CLM*" & strClm01 & "*" & strClm02 & "*" & strClm03 _
             & "*" & strClm04 & "*" & strClm05_1 & ":" & strClm05_2 & ":" & strClm05_3 & "*" & strClm06 _
             & "*" & strClm07 & "*" & strClm08 & "*" & strClm09 & "*" & strClm10 & "*" & strClm11 & "~"

    Print #1, strClaim
        g_lngNumSeg = g_lngNumSeg + 1

        If (Not IsNull(rst.Fields("fldAdmitDate").Value)) And (rst.Fields("fldPOS").Value <> 11) Then
            'Date of Admission
            strDTP_01 = "435"                'date/time qualifier
            strDTP_02 = "D8"                 'date time period format qualifier
            strDTP_03 = Format(rst.Fields("fldAdmitDate").Value, "yyyymmdd")              'date/time period in CCYYMMDD

            strDTP = "DTP*" & strDTP_01 & "*" & strDTP_02 & "*" & strDTP_03 & "~"

        Print #1, strDTP
            g_lngNumSeg = g_lngNumSeg + 1
        End If

        If (Not IsNull(rst.Fields("fldDischargeDate").Value)) And (rst.Fields("fldPOS").Value <> 11) Then
            'fldDischargeDate
            strDTP_01 = "096"                'date/time qualifier
            strDTP_02 = "D8"                 'date time period format qualifier
            strDTP_03 = Format(rst.Fields("fldDischargeDate").Value, "yyyymmdd")              'date/time period in CCYYMMDD

            strDTP = "DTP*" & strDTP_01 & "*" & strDTP_02 & "*" & strDTP_03 & "~"

        Print #1, strDTP
            g_lngNumSeg = g_lngNumSeg + 1
        End If

        'Cert/authorization information
        If Not IsNull(rst.Fields("fldCertNum").Value) Then
        Print #1, "REF*G1*" & Trim(StripDelimiters(rst.Fields("fldCertNum").Value)) & "~"
            g_lngNumSeg = g_lngNumSeg + 1
        End If

        'DSM Code - decimals are removed from the code
        strHI = "HI*BK:" & StripDecimals(IfNull(rst.Fields("fldDSM_IV").Value, "")) & "~"
    Print #1, strHI
        g_lngNumSeg = g_lngNumSeg + 1

        Exit Sub

ErrTrap:
        Err.Raise(Err.Number, Err.Source, Err.Description)

    End Sub

    Private Sub WriteLoop2310A(ByVal rst As ADODB.Recordset)
        'Referring provider information

        Dim strNM101, strNM102, strNM103, strNM104, strNM105 As String
        Dim strNM106, strNM107, strNM108, strNM109 As String
        Dim strNM1 As String
        Dim strPRV, strPRV01, strPRV02, strPRV03, strPRV04 As String
        Dim strRef, strRef01, strRef02 As String

        On Error GoTo ErrTrap

        strNM101 = "DN"                'entity identifier code (DN = referring physician)
        strNM102 = "1"                 'entity type qualifier (1 = person)
        strNM103 = Mid(StripDelimiters(IfNull(rst.Fields("fldReferPhy").Value, "")), 1, 35) 'last name
        strNM104 = Mid(StripDelimiters(IfNull(rst.Fields("fldReferPhy").Value, "")), 1, 25) 'first name
        strNM105 = ""                  'middle name
        strNM106 = ""                  'name prefix (not used)
        strNM107 = ""                  'name suffix
        strNM108 = "24"                'identification code qualifier
        strNM109 = StripDelimiters(IfNull(rst.Fields("fldReferNum").Value, "")) 'Reference number

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

    Print #1, strNM1
        g_lngNumSeg = g_lngNumSeg + 1

        strPRV01 = "RF"                 'Provider Code (BI-Billing, PT-Payto, RF-Referring)
        strPRV02 = "ZZ"                 'Reference Identification Qualifier
        'Specialty information
        strPRV03 = "101Y00000X"         'ZZ relies on Provider Taxonomy Code published by BC/BS Association

        strPRV = "PRV*" & strPRV01 & "*" & strPRV02 & "*" & strPRV03 & "~"

    Print #1, strPRV
        g_lngNumSeg = g_lngNumSeg + 1

        strRef01 = "1G"                 'Provider UPIN Number
        strRef02 = StripDelimiters(IfNull(rst.Fields("fldReferNum").Value, "")) 'UPIN Only

        strRef = "REF*" & strRef01 & "*" & strRef02 & "~"

        If Len(strRef02) = 6 Then       'UPIN
        Print #1, strRef
            g_lngNumSeg = g_lngNumSeg + 1
        End If

        Exit Sub

ErrTrap:
        Err.Raise(Err.Number, Err.Source, Err.Description)

    End Sub
    Private Sub WriteLoop2310B(ByVal rst As ADODB.Recordset)
        'Rendering provider information

        Dim strNM101, strNM102, strNM103, strNM104, strNM105 As String
        Dim strNM106, strNM107, strNM108, strNM109 As String
        Dim strNM1 As String
        Dim strPRV, strPRV01, strPRV02, strPRV03, strPRV04 As String
        Dim strRef, strRef01, strRef02 As String

        On Error GoTo ErrTrap

        strNM101 = "82"                'entity identifier code (82 = rendering provider)
        strNM102 = "1"                 'entity type qualifier (1 = person)
        strNM103 = Mid(Replace(IfNull(rst.Fields("fldProviderLastName").Value, ""), "'", ""), 1, 35)    'Last name
        strNM104 = Mid(Replace(IfNull(rst.Fields("fldProviderFirstName").Value, ""), "'", ""), 1, 35)   'first name
        strNM105 = IfNull(rst.Fields("fldProviderMI").Value, "")   'middle name
        strNM106 = ""                  'name prefix (not used)
        strNM107 = ""                  'name suffix

        'identification code qualifier
        If rst.Fields("fldTinType").Value = 3 Then
            strNM108 = "34"        'individual SSN
        Else
            strNM108 = "24"        '24 = employer'S ID
        End If

        strNM109 = NumbersOnly(rst.Fields("fldTIN").Value)         'billing provider identifier code

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

    Print #1, strNM1
        g_lngNumSeg = g_lngNumSeg + 1

        strPRV01 = "PE"                 'Provider Code (PE-Performing, BI-Billing, PT-Payto, RF-Referring)
        strPRV02 = "ZZ"                 'Reference Identification Qualifier
        'Specialty information
        strPRV03 = "101Y00000X"         'ZZ relies on Provider Taxonomy Code published by BC/BS Association

        strPRV = "PRV*" & strPRV01 & "*" & strPRV02 & "*" & strPRV03 & "~"

    Print #1, strPRV
        g_lngNumSeg = g_lngNumSeg + 1

        'Physician Type
        If rst.Fields("fldInsType").Value = "MP" Then
            strRef01 = "1C"
            strRef02 = IfNull(rst.Fields("fldIndPracticeNum").Value, "")
        ElseIf rst.Fields("fldInsType").Value = "MB" Then
            strRef01 = "1C"
            strRef02 = IfNull(rst.Fields("fldIndPracticeNum").Value, "")
        ElseIf rst.Fields("fldInsType").Value = "MC" Then
            strRef01 = "1D"
            strRef02 = IfNull(rst.Fields("fldIndPracticeNum").Value, "")
        ElseIf rst.Fields("fldInsType").Value = "BL" Then 'BC/BS
            strRef01 = "1B"
            strRef02 = IfNull(rst.Fields("fldIndPracticeNum").Value, "")
        ElseIf rst.Fields("fldInsType").Value = "CH" Then
            strRef01 = "1H"
            strRef02 = IfNull(rst.Fields("fldIndPracticeNum").Value, "")
        Else        'Commercial Claims
            strRef01 = "EI"
            strRef02 = NumbersOnly(rst.Fields("fldTIN").Value)
        End If

        strRef = "REF*" & strRef01 & "*" & strRef02 & "~"

        If strRef02 > "" And strRef01 <> "EI" Then
        Print #1, strRef
            g_lngNumSeg = g_lngNumSeg + 1
        End If

        Exit Sub

ErrTrap:
        Err.Raise(Err.Number, Err.Source, Err.Description)

    End Sub
    Private Sub WriteLoop2310D(ByVal rst As Recordset)
        'Service Facility information
        On Error GoTo ErrTrap

        Dim strNM101, strNM102, strNM103, strNM104, strNM105 As String
        Dim strNM106, strNM107, strNM108, strNM109 As String
        Dim strNM1, strN2, strN3, strN4 As String
        Dim strPRV, strPRV01, strPRV02, strPRV03, strPRV04 As String
        Dim strRef, strRef01, strRef02 As String
        Dim FacilityID, FacilityTypeID As String

        '1C = Medicare Provider ID
        '1D = Medicaid Provider ID
        '0B = State license
        '1A = Blue Cross Provider ID
        '1B = Blue Shield Provider ID
        '1G = Provider UPIN
        '1H = Champus Provider ID
        'G2 = Commercial Provider ID

        '  If Not IsNull(rst.Fields("FacilityProviderID").Value) Then
        '     FacilityID = Trim(rst.Fields("FacilityProviderID").Value)
        '     FacilityTypeID = "1C"
        '  Else
        '     If Not IsNull(rst.Fields("FacilityStateLicense").Value) Then
        '        FacilityID = Trim(rst.Fields("FacilityStateLicense").Value)
        '        FacilityTypeID = "0B"
        '     Else
        FacilityID = ""
        FacilityTypeID = ""
        '     End If
        '  End If

        strNM101 = "FA"                'entity identifier code (FA = facility, LI = independant laboratory)
        strNM102 = "2"                 'entity type qualifier (1 = person, 2 = non-person)
        strNM103 = Mid(StripDelimiters(IfNull(rst.Fields("fldFacilityLine1").Value, "")), 1, 35)  'Facility Name
        strNM104 = ""                  'first name
        strNM105 = ""                  'middle name
        strNM106 = ""                  'name prefix (not used)
        strNM107 = ""                  'name suffix
        ' If Not IsNull(rst.Fields("FacilityTaxID").Value) Then
        '   strNM108 = "24"                'identification code qualifier
        '   strNM109 = StripDelimiters(rst.Fields("FacilityTaxID").Value) 'Facility Tax ID number
        '   strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
        '       & "*" & strNM104 & "*" & strNM105 & "*" & strNM106 & "*" & strNM107 _
        '       & "*" & strNM108 & "*" & strNM109 & "~"

        '    Print #1, strNM1
        '    g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
        ' Else
        strNM108 = ""                'identification code qualifier
        strNM109 = ""                'Facility Tax ID number
        strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 & "~"
       Print #1, strNM1
        g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
        ' End If

        'If the Facility name is more than 35 characters then we use segment N2 to hold the
        'overflow characters
        If Len(IfNull(rst.Fields("fldFacilityLine1").Value, "")) > 35 Then
            strN2 = "N2*" & Mid(rst.Fields("fldFacilityLine1").Value, 36) & "~"
        Else
            strN2 = ""
        End If

        If strN2 > "" Then
            '   Print #1, strN2  'Print overflow line
            '   g_lngNumSeg = g_lngNumSeg + 1 'Increment segment counters
        End If

        If IfNull(rst.Fields("fldFacilityLine2").Value, "") > "" Then
            strN3 = "N3*" & StripDelimiters(rst.Fields("fldFacilityLine2").Value) & "~"
        Print #1, strN3
            g_lngNumSeg = g_lngNumSeg + 1

            strN4 = "N4*" & rst.Fields("fldFacilityCity").Value & "*" & UCase(rst.Fields("fldFacilityState").Value) & "*" & Mid(rst.Fields("fldFacilityZip").Value, 1, 5) & "~"
        Print #1, strN4
            g_lngNumSeg = g_lngNumSeg + 1
        End If

        strRef01 = FacilityTypeID                       'Facility Type of ID
        strRef02 = Trim(StripDelimiters(FacilityID))    'Facility ID
        strRef = "REF*" & strRef01 & "*" & strRef02 & "~"

        If FacilityID > "" Then
       Print #1, strRef
            g_lngNumSeg = g_lngNumSeg + 1
        End If

        Exit Sub
ErrTrap:
        Err.Raise(Err.Number, Err.Source, Err.Description)
    End Sub
    Private Sub WriteLoop2400(ByVal rst As ADODB.Recordset)
        'Service Line

        Dim strSvc As String
        Dim strSV1, strSV1_01, strSV1_02, strSV1_03 As String
        Dim strSV1_04, strSV1_05, strSV1_06 As String
        Dim strSV1_07, strSV1_08, strSV1_09 As String
        Dim strDTP, strDTP_01, strDTP_02, strDTP_03 As String
        Dim strRef, strRef01, strRef02 As String

        On Error GoTo ErrTrap

        strSvc = "LX*1~"
    Print #1, strSvc
        g_lngNumSeg = g_lngNumSeg + 1

        'SV1 segment (professional service)
        strSV1_01 = "HC"                    'product or service qualifer
        strSV1_01 = strSV1_01 & ":" & rst.Fields("fldCPTCode").Value    'product/service ID
        strSV1_02 = rst.Fields("fldFee").Value    'monetary amount
        strSV1_03 = "UN"                          'unit or basis for measurement code
        strSV1_04 = rst.Fields("fldUnits").Value  'quantity (units or minutes)

        'Medicaid specific: For electronic claims, we must undo the numbering sequence
        'that was coded in for paper-based claims
        Select Case rst.Fields("fldPOS").Value            'facility type code
            Case 1
                strSV1_05 = "11"
            Case 2
                strSV1_05 = "12"
            Case 3
                strSV1_05 = "21"
            Case Else
                strSV1_05 = rst.Fields("fldPOS").Value
        End Select

        strSV1_06 = "1"
        strSV1_07 = "1"   'DSM-IV pointer - points to first diagnosis line in HL* line
        strSV1_08 = ""
        strSV1_09 = "N"                     'Emergency indicatior (Y/N)

        strSV1 = "SV1*" & strSV1_01 & "*" & strSV1_02 & "*" & strSV1_03 & "*" & strSV1_04 & "*" _
            & strSV1_05 & "*" & strSV1_06 & "*" & strSV1_07 & "**" & strSV1_08 & strSV1_09 & "~"

    Print #1, strSV1
        g_lngNumSeg = g_lngNumSeg + 1

        'Date of Service
        strDTP_01 = "472"                'date/time qualifier
        strDTP_02 = "D8"                 'date time period format qualifier
        strDTP_03 = Format(rst.Fields("fldDOS").Value, "yyyymmdd")              'date/time period in CCYYMMDD

        strDTP = "DTP*" & strDTP_01 & "*" & strDTP_02 & "*" & strDTP_03 & "~"

    Print #1, strDTP
        g_lngNumSeg = g_lngNumSeg + 1

        'Line Item Control Number
        strRef01 = "6R"                'Seq Number
        strRef02 = "001"

        strRef = "REF*" & strRef01 & "*" & strRef02 & "~"

    Print #1, strRef
        g_lngNumSeg = g_lngNumSeg + 1

        Exit Sub

ErrTrap:
        Err.Raise(Err.Number, Err.Source, Err.Description)
    End Sub
    Private Sub WriteLoop2420B(ByVal rst As ADODB.Recordset)
        'Rendering provider information

        Dim strNM101, strNM102, strNM103, strNM104, strNM105 As String
        Dim strNM106, strNM107, strNM108, strNM109 As String
        Dim strNM1 As String
        Dim strPRV, strPRV01, strPRV02, strPRV03, strPRV04 As String
        Dim strRef, strRef01, strRef02 As String

        On Error GoTo ErrTrap

        strNM101 = "82"                'entity identifier code (82 = rendering provider)
        strNM102 = "1"                 'entity type qualifier (1 = person)
        strNM103 = Mid(Replace(IfNull(rst.Fields("fldProviderLastName").Value, ""), "'", ""), 1, 35)    'Last name
        strNM104 = Mid(Replace(IfNull(rst.Fields("fldProviderFirstName").Value, ""), "'", ""), 1, 35)   'first name
        strNM105 = IfNull(rst.Fields("fldProviderMI").Value, "")   'middle name
        strNM106 = ""                  'name prefix (not used)
        strNM107 = ""                  'name suffix

        'identification code qualifier
        If rst.Fields("fldTinType").Value = 3 Then
            strNM108 = "34"        'individual SSN
        Else
            strNM108 = "24"        '24 = employer'S ID
        End If

        strNM109 = NumbersOnly(rst.Fields("fldTIN").Value)         'billing provider identifier code

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

    Print #1, strNM1
        g_lngNumSeg = g_lngNumSeg + 1

        strPRV01 = "PE"                 'Provider Code (PE-Performing, BI-Billing, PT-Payto, RF-Referring)
        strPRV02 = "ZZ"                 'Reference Identification Qualifier
        'Specialty information
        strPRV03 = "101Y00000X"         'ZZ relies on Provider Taxonomy Code published by BC/BS Association

        strPRV = "PRV*" & strPRV01 & "*" & strPRV02 & "*" & strPRV03 & "~"

    Print #1, strPRV
        g_lngNumSeg = g_lngNumSeg + 1

        'Physician Type
        If rst.Fields("fldInsType").Value = "MP" Then
            strRef01 = "1C"
            strRef02 = IfNull(rst.Fields("fldIndPracticeNum").Value, "")
        ElseIf rst.Fields("fldInsType").Value = "MB" Then
            strRef01 = "1C"
            strRef02 = IfNull(rst.Fields("fldIndPracticeNum").Value, "")
        ElseIf rst.Fields("fldInsType").Value = "MC" Then
            strRef01 = "1D"
            strRef02 = IfNull(rst.Fields("fldIndPracticeNum").Value, "")
        ElseIf rst.Fields("fldInsType").Value = "BL" Then 'BC/BS
            strRef01 = "1B"
            strRef02 = IfNull(rst.Fields("fldIndPracticeNum").Value, "")
        ElseIf rst.Fields("fldInsType").Value = "CH" Then
            strRef01 = "1H"
            strRef02 = IfNull(rst.Fields("fldIndPracticeNum").Value, "")
        Else   'Commercial Claims
            strRef01 = "EI"
            strRef02 = NumbersOnly(rst.Fields("fldTIN").Value)
        End If

        strRef = "REF*" & strRef01 & "*" & strRef02 & "~"

        If strRef02 > "" And strRef01 <> "EI" Then
        Print #1, strRef
            g_lngNumSeg = g_lngNumSeg + 1
        End If

        Exit Sub

ErrTrap:
        Err.Raise(Err.Number, Err.Source, Err.Description)

    End Sub
    Private Sub WriteTxSetTrailer()

        'SE segment
        Dim strSE As String

        On Error GoTo ErrTrap

        'Increment segment counter one more time - segment count includes the 'SE' (trailer) segment
        g_lngNumSeg = g_lngNumSeg + 1

        strSE = "SE*" & g_lngNumSeg & "*" & Format(CStr(g_lngEndTxNum), "00000000") & "~"

    Print #1, strSE

        g_lngNumSeg = 0

        Exit Sub

ErrTrap:
        Err.Raise(Err.Number, Err.Source, Err.Description)

    End Sub

    Function CreatePayerCodePrefix(ByVal strInsType As String) As String

        Dim strPayerPrefix As String

        On Error GoTo ErrTrap

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
                strPayerPrefix = "C"
            Case Else 'Commercial Claims
                strPayerPrefix = "F"
        End Select

        CreatePayerCodePrefix = strPayerPrefix

        Exit Function

ErrTrap:
        Err.Raise(Err.Number, Err.Source, Err.Description)

    End Function

    Private Function StripDelimiters(ByVal strData As String) As String

        Dim strTemp As String
        Dim intPos As Integer

        strTemp = strData 'Make a copy of the data

        intPos = InStr(1, strData, ":", vbTextCompare)
        If intPos > 0 Then
            Do While intPos > 0
                strTemp = Mid(strData, 1, intPos - 1) & Mid(strData, intPos + 1)
                intPos = InStr(1, strTemp, ":", vbTextCompare)
            Loop
        End If

        intPos = InStr(1, strTemp, "*", vbTextCompare)
        If intPos > 0 Then
            Do While intPos > 0
                strTemp = Mid(strTemp, 1, intPos - 1) & Mid(strTemp, intPos + 1)
                intPos = InStr(1, strTemp, "*", vbTextCompare)
            Loop
        End If

        intPos = InStr(1, strTemp, "-", vbTextCompare)
        If intPos > 0 Then
            Do While intPos > 0
                strTemp = Mid(strTemp, 1, intPos - 1) & Mid(strTemp, intPos + 1)
                intPos = InStr(1, strTemp, "-", vbTextCompare)
            Loop
        End If

        intPos = InStr(1, strTemp, "~", vbTextCompare)
        If intPos > 0 Then
            Do While intPos > 0
                strTemp = Mid(strTemp, 1, intPos - 1) & Mid(strTemp, intPos + 1)
                intPos = InStr(1, strTemp, "~", vbTextCompare)
            Loop
        End If

        intPos = InStr(1, strTemp, "AMYSIS#", vbTextCompare)
        If intPos > 0 Then
            Do While intPos > 0
                strTemp = Mid(strTemp, 1, intPos - 1) & Mid(strTemp, intPos + 7)
                intPos = InStr(1, strTemp, "AMYSIS#", vbTextCompare)
            Loop
        End If

        StripDelimiters = strTemp

    End Function
    Private Function StripDecimals(ByVal strData As String) As String

        Dim strTemp As String
        Dim intPos As Integer

        strTemp = strData 'Make a copy of the data

        intPos = InStr(1, strData, ".", vbTextCompare)
        If intPos > 0 Then
            strTemp = Mid(strData, 1, intPos - 1) & Mid(strData, intPos + 1)
        End If

        StripDecimals = strTemp

    End Function
    Private Sub WriteLoop2000C(ByVal rst As ADODB.Recordset)
        'Patient information - written only if patient is not same person as insured.

        Dim strHL, strHL_01, strHL_02, strHL_03, strHL_04 As String
        Dim strPat As String

        On Error GoTo ErrTrap

        strHL_01 = "3"           'hierarchical ID number - always 3 (see p. 152 of X098.pdf)
        strHL_02 = "2"           'hierarchical parent ID
        strHL_03 = "23"          'hierarchical level code (23 = dependant)
        strHL_04 = "0"           'hierarchical child code

        strHL = "HL*" & strHL_01 & "*" & strHL_02 & "*" & strHL_03 & "*" & strHL_04 & "~"
    Print #1, strHL
        g_lngNumSeg = g_lngNumSeg + 1

        strPat = "PAT*"
        strPat = strPat & GetRelationCode(IfNull(rst.Fields("fldPatientRelation").Value, "")) & "~"
    Print #1, strPat
        g_lngNumSeg = g_lngNumSeg + 1

        Exit Sub

ErrTrap:
        Err.Raise(Err.Number, Err.Source, Err.Description)

    End Sub
    Private Function WriteLoop2000CA(ByVal rst As ADODB.Recordset)
        'Patient name, demographics - written only if patient is not same person as insured.

        Dim strNM101, strNM102, strNM103, strNM104, strNM105 As String
        Dim strNM106, strNM107, strNM108, strNM109, strNM110, strNM111 As String
        Dim strNM1 As String
        Dim strN3, strN4 As String
        Dim strDMG As String
        Dim dtDOB As Date

        On Error GoTo ErrTrap

        'Loop 2000CA
        strNM101 = "QC"         'entity identifier code (QC = submitter)
        strNM102 = "1"          'Qualifier - 2:Non-person entity
        strNM103 = Mid(Replace(IfNull(rst.Fields("fldPatientLastName").Value, ""), "'", ""), 1, 35)    'Last name
        strNM104 = Mid(Replace(IfNull(rst.Fields("fldPatientFirstName").Value, ""), "'", ""), 1, 35)   'first name
        strNM105 = IfNull(rst.Fields("fldPatientMI").Value, "")   'middle name
        strNM106 = ""        'not used (name prefix)
        strNM107 = ""        'not used (name suffix)
        strNM108 = "MI"      'identification code qualifier
        'Subscribers Identification number as assigned by the payer.
        strNM109 = StripDelimiters(IfNull(rst.Fields("fldCardNum").Value, "00"))
        ''03/27/2003 DR: This line caused rejections --- strNM1 = strNM109 = rst.Fields("fldPatientID").Value

        strNM1 = "NM1*" & strNM101 & "*" & strNM102 & "*" & strNM103 _
             & "*" & strNM104 & "*" & strNM105 & "*" & strNM106 _
             & "*" & strNM107 & "*" & strNM108 & "*" & strNM109 _
             & "~"

    Print #1, strNM1
        g_lngNumSeg = g_lngNumSeg + 1

        If Not IsNull(rst.Fields("fldPatientStreetNum").Value) Then
            strN3 = "N3*" & StripDelimiters(rst.Fields("fldPatientStreetNum").Value) & "~"
        Print #1, strN3
            g_lngNumSeg = g_lngNumSeg + 1

            strN4 = "N4*" & rst.Fields("fldPatientCity").Value & "*" & rst.Fields("fldPatientState").Value & "*" & rst.Fields("fldPatientZip").Value & "~"
        Print #1, strN4
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
        Print #1, strDMG
            g_lngNumSeg = g_lngNumSeg + 1
        End If

        Exit Function

ErrTrap:
        Err.Raise(Err.Number, Err.Source, Err.Description)

    End Function
    Private Function GetClaimFilingCode(ByVal strInsType As String)

        Dim strFilingCode As String

        Select Case strInsType
            Case "WC"
                strFilingCode = "WC"
            Case "MP", "MB"
                strFilingCode = "MB"
            Case "MC"
                strFilingCode = "MC"
            Case "BL"
                strFilingCode = "BL"
            Case "CH"
                strFilingCode = "CH"
            Case Else
                strFilingCode = "CI" 'Commercial Insurance
        End Select

        GetClaimFilingCode = strFilingCode

    End Function


    Private Function GetRelationCode(ByVal strPatRelat As String)

        On Error GoTo ErrTrap

        Select Case strPatRelat
            Case "Spouse"
                GetRelationCode = "01"
            Case "Child"
                GetRelationCode = "19"
            Case Else
                GetRelationCode = "21"
        End Select

        Exit Function

ErrTrap:
        Err.Raise(Err.Number, Err.Source, Err.Description)

    End Function

    Private Sub StoreCrossReference(ByVal lngClaimID As Long, ByVal lngEncounterLogID As Long, _
                                    ByVal lngEFileID As Long, ByVal strEFileName As String)
        'Stores the cross-reference number of the electronic claim in tblClaim, so that a reference
        'between THIN and our system exists.

        Dim objEDI As CEDIBz

        On Error GoTo ErrTrap

        objEDI = New CEDIBz
        Call objEDI.EstablishXRef(lngClaimID, lngEncounterLogID, lngEFileID, strEFileName)
        objEDI = Nothing

        Exit Sub

ErrTrap:
        objEDI = Nothing
        Err.Raise(Err.Number, Err.Source, Err.Description)

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

        On Error GoTo ErrTrap

        'DR: 06/06/2003: This is ineffecient but necessary - fldProviderID does not exist in tblClaim ?!
        'This is far less work than restructering all the claim logic.
        objEL = CreateObject("EncounterLogBz.CEncounterLogBz")
        rst = objEL.FetchByID(lngEncounterLogID)
        lngProviderID = rst.Fields("fldProviderID").Value
        objEL = Nothing
        rst = Nothing

        objProv = CreateObject("ProviderBz.CProviderBZ")
        rst = objProv.FetchPracticeNumbers(lngProviderID)
        objProv = Nothing

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

            rst.MoveNext()
        Next intCtr

        rst = Nothing

        If strProvNumber = "" Then
            strProvNumber = strTempNumber
        End If

        FetchProviderPracticeNumber = strProvNumber

        Exit Function

ErrTrap:
        objProv = Nothing
        rst = Nothing
        Err.Raise(Err.Number, Err.Source, Err.Description)

    End Function



End Module
