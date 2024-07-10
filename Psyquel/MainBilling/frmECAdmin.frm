VERSION 5.00
Begin VB.Form frmECAdmin 
   Caption         =   "Psquel - Electronic Claims"
   ClientHeight    =   4905
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   5220
   LinkTopic       =   "Form1"
   ScaleHeight     =   4905
   ScaleWidth      =   5220
   StartUpPosition =   1  'CenterOwner
   Begin VB.CommandButton cmdDone 
      Caption         =   "&Cancel"
      Height          =   420
      Index           =   1
      Left            =   3420
      TabIndex        =   1
      Top             =   4320
      Width           =   1140
   End
   Begin VB.CommandButton cmdDone 
      Caption         =   "&OK"
      Height          =   420
      Index           =   0
      Left            =   675
      TabIndex        =   0
      Top             =   4320
      Width           =   1140
   End
End
Attribute VB_Name = "frmECAdmin"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Private Const POS_ISA As Integer = 0
Private Const POS_GS As Integer = 1
Private Const POS_ST As Integer = 2
Private Const POS_BGN As Integer = 3
Private Const POS_PRV As Integer = 4
Private Const POS_NM1 As Integer = 5
Private Const POS_REF As Integer = 6
Private Const POS_PER As Integer = 7
Private Const POS_SBR As Integer = 8
Private Const POS_PAT As Integer = 9
Private Const POS_NM1B As Integer = 10
Private Const POS_DMG As Integer = 11
Private Const POS_REFB As Integer = 12
Private Const POS_CLM As Integer = 13
Private Const POS_AMT As Integer = 14
Private Const POS_REFC As Integer = 15
Private Const POS_HI As Integer = 16
Private Const POS_QTY As Integer = 17
Private Const POS_LS As Integer = 18
Private Const POS_NM1C As Integer = 19
Private Const POS_LE As Integer = 20
Private Const POS_LS2 As Integer = 21
Private Const POS_SBR2 As Integer = 22
Private Const POS_AMT2 As Integer = 23
Private Const POS_NM1D As Integer = 24
Private Const POS_SBR3 As Integer = 25
Private Const POS_LE2 As Integer = 27
Private Const POS_LX As Integer = 28
Private Const POS_SV As Integer = 29
Private Const POS_LS3 As Integer = 30
Private Const POS_SEND As Integer = 31
Private Const POS_END As Integer = 32
Private Const INS_MEDICAID As Integer = 105
Private Const INS_WORKCOMP As Integer = 346
Private Const INS_MEDICARE As Integer = 24
Private Const INS_BCBS As Integer = 158
Private lngNumFiles As Long
Private strFileName() As String
Private CONST_OUTPUT_DIR As String
Private Sub GenerateFile()
    Dim strFile(32) As String
    Dim dteCreateDate As Date
    Dim rstEClaims As ADODB.Recordset
    Dim rstData As ADODB.Recordset
    Dim rstInsInfo As ADODB.Recordset
    Dim obj As SubmissionBz.CSubmissionBz
    Dim lngNumSect As Integer
    Dim lngNumSeg As Integer
    Dim strReferLast As String
    Dim strReferFirst As String
    Dim strReferMI As String
    Dim strTemp As String
    
    Dim TRANSMITNUMBER As String
    Dim FREQCODE As String
    
    On Error GoTo ErrHand
        
    Set obj = CreateObject("SubmissionBz.CSubmissionBz")
    
    Screen.MousePointer = vbHourglass
    obj.CloseElectClaims
    dteCreateDate = Now
    lngNumFiles = 0
    
    Set rstEClaims = obj.FetchUnsentElectClaims
    While Not rstEClaims.EOF
        
        Set rstData = obj.FetchElectClaims(rstEClaims!fldElectClaimID)
        Set rstInsInfo = obj.FetchOtherInsInfo(rstData!fldEncounterLogID, rstData!fldOrder)
        
        TRANSMITNUMBER = rstEClaims!fldElectClaimID
        
        'UNKNOWN VALUES: (RESEARCH)
        FREQCODE = ""
        
        lngNumFiles = lngNumFiles + 1
        ReDim Preserve strFileName(lngNumFiles)
        strFileName(lngNumFiles) = TRANSMITNUMBER & ".ecm"
        lngNumSect = 0
        
        'Format X.12 837 v 3050
        '? 745047157 - ISA08
        strFile(POS_ISA) = "ISA*00*          *00*          *ZZ*445498151      *ZZ*745047157      *" & Format(dteCreateDate, "yymmdd") & "*" & Format(dteCreateDate, "hhnn")
        strFile(POS_ISA) = strFile(POS_ISA) & "*U*00200*" & Format(Hex(TRANSMITNUMBER), "000000000") & "*0*P*>"
        'TRANSMITNUMBER = 9 space # - num for entire transmission
        
        strFile(POS_GS) = "GS*HC*445498151*745047157*" & Format(dteCreateDate, "yymmdd") & "*" & Format(dteCreateDate, "hhnn") & "*" & Hex(TRANSMITNUMBER) & "*X*003050"
        'GROUPNUMBER = up to 9 space # - num for group of records (append # to transmit #? - sequential)
        
        WriteFileHeader TRANSMITNUMBER, strFile
        
        While Not rstData.EOF
            lngNumSect = lngNumSect + 1
            lngNumSeg = 0
            strFile(POS_ST) = "ST*837*" & Mid(Hex(TRANSMITNUMBER), 1, 6) & Format(lngNumSect, "000")
            lngNumSeg = lngNumSeg + 1
            'TXSETNUMBER  = up to 9 space # - for each claim
            
            strFile(POS_BGN) = "BGN*00*" & Hex(TRANSMITNUMBER) & "*" & Format(dteCreateDate, "yymmdd") & "*" & Format(dteCreateDate, "hhnn") & "*  *D*CK"
            lngNumSeg = lngNumSeg + 1
            'PROVBATCHID  = up to 30 space #
            
            strFile(POS_PRV) = "PRV*BS*1D*" & rstData!fldEncounterLogID
            lngNumSeg = lngNumSeg + 1
            
            With rstData
                strFile(POS_NM1) = "NM1*85*1*" & Left(!fldProvLast, 35) & "*" & Left(!fldProvFirst, 25) & "*" & IfNull(!fldProvMI, "") & "***MC*" & Left(UCase(IfNull(!fldPracticeNumber, "NONE")), 20)
                lngNumSeg = lngNumSeg + 1
                
                'BATCHID - 30 dig #, "required for C21 processing?"  new batch header written each time encountered?  use trans number.  only output once
                If lngNumSect = 1 Then
                    strFile(POS_REF) = "REF*BT*" & TRANSMITNUMBER & vbCrLf
                    lngNumSeg = lngNumSeg + 1
                Else
                    strFile(POS_REF) = ""
                End If
            
                strFile(POS_REF) = strFile(POS_REF) & "REF*TJ*" & Left(!fldTaxID, 30)
                lngNumSeg = lngNumSeg + 1
            
                strFile(POS_PER) = "PER*IC**TE*" & IfNull(!fldProvPhone, "")    'stripped, optional
                lngNumSeg = lngNumSeg + 1
            
                strFile(POS_SBR) = "SBR*P"
                lngNumSeg = lngNumSeg + 1
                strFile(POS_PAT) = "PAT*18*****"
                lngNumSeg = lngNumSeg + 1
            
                strFile(POS_NM1B) = "NM1*QC*1*" & Left(!fldPatLast, 35) & "*" & Left(!fldPatFirst, 25) & "*" & IfNull(!fldPatMI, "") & "*" & IfNull(!fldPatSSN, "") & "**93*" & !fldPatientID
                lngNumSeg = lngNumSeg + 1
            
                strFile(POS_DMG) = "DMG*D8*" & Format(!fldPatDOB, "yyyymmdd") & "*" & IfNull(!fldSex, "U")
                lngNumSeg = lngNumSeg + 1
            
                strFile(POS_REFB) = "REF*EA*" & !fldEncounterLogID & "*"
                lngNumSeg = lngNumSeg + 1
            
                strFile(POS_CLM) = "CLM*445498151**"
                lngNumSeg = lngNumSeg + 1
                
                If !fldInsuranceID = INS_MEDICAID Then
                    strFile(POS_CLM) = strFile(POS_CLM) & "MC"
                Else
                    strFile(POS_CLM) = strFile(POS_CLM) & "ZZ"
                End If
                
                '27 = test, 00 = prod
                strFile(POS_CLM) = strFile(POS_CLM) & "*MD*>>" & FREQCODE & "******>********27" & vbCrLf
'                strFile(POS_CLM) = strFile(POS_CLM) & "*MD*" & !fldPOSCode & ">A>" & FREQCODE & "******>********27" & vbCrLf
                
                strFile(POS_CLM) = strFile(POS_CLM) & "DTP*150*D8*" & Format(!fldDOS, "yyyymmdd") & vbCrLf
                lngNumSeg = lngNumSeg + 1
                strFile(POS_CLM) = strFile(POS_CLM) & "DTP*151*D8*" & Format(!fldDOS, "yyyymmdd")
                lngNumSeg = lngNumSeg + 1
                'shouldnt be releveant, but will have to link to tblappt if it is
                'strFile(POS_CLM) = strFile(POS_CLM) & "DTP*435*DT*" & Format(!fldDOS, "YYYYMMDDHHNN")
            End With
            
            strFile(POS_AMT) = ""
            If Not rstInsInfo.EOF Then
                strTemp = Format(rstInsInfo!fldDisAmount, "##.##")
'               If strTemp = "" Then strTemp = "0"
                If Right(strTemp, 1) = "." Then strTemp = Left(strTemp, Len(strTemp) - 1)
                strFile(POS_AMT) = strFile(POS_AMT) & "AMT*F5*" & strTemp & vbCrLf
                lngNumSeg = lngNumSeg + 1
            End If
            
            strTemp = Format(rstData!fldPatCopay, "##.##")
            If Right(strTemp, 1) = "." Then strTemp = Left(strTemp, Len(strTemp) - 1)
            If strTemp = "" Then strTemp = "0"
            strFile(POS_AMT) = strFile(POS_AMT) & "AMT*NG*" & strTemp & vbCrLf
            lngNumSeg = lngNumSeg + 1
            If Not rstInsInfo.EOF Then
                strTemp = Format(rstInsInfo!fldPaidAmount, "##.##")
'               If strTemp = "" Then strTemp = "0"
                If Right(strTemp, 1) = "." Then strTemp = Left(strTemp, Len(strTemp) - 1)
                strFile(POS_AMT) = strFile(POS_AMT) & "AMT*C4*" & strTemp & vbCrLf
                lngNumSeg = lngNumSeg + 1
            End If
            
            With rstData
                strTemp = Format(!fldFee, "##.##")
                If Right(strTemp, 1) = "." Then strTemp = Left(strTemp, Len(strTemp) - 1)
'               If strTemp = "" Then strTemp = "0"
                strFile(POS_AMT) = strFile(POS_AMT) & "AMT*PB*" & strTemp
                lngNumSeg = lngNumSeg + 1
            
                If Len(!fldFreeTextCertNum) > 1 Then
                    strFile(POS_REFC) = "REF*G1*" & Left(!fldFreeTextCertNum, 30)
                    lngNumSeg = lngNumSeg + 1
                Else
                    strFile(POS_REFC) = ""
                End If
            
                strFile(POS_HI) = "HI*BJ>" & Replace(!fldCode, ".", "") & "*BK>" & Replace(!fldCode, ".", "") & vbCrLf
                lngNumSeg = lngNumSeg + 1
                strFile(POS_HI) = strFile(POS_HI) & "HI*BT>" & !fldCPTCode & ">D8>" & Format(!fldDOS, "yyYYMMDD")
                lngNumSeg = lngNumSeg + 1
            
                strFile(POS_QTY) = ""
            
                strFile(POS_LS) = "LS*2310"
                lngNumSeg = lngNumSeg + 1
                strFile(POS_NM1C) = ""
            
                'if no refer prov, next section not needed
                If Len(!fldReferPhy) > 1 Then
                    ParseName Trim(!fldReferPhy), strReferLast, strReferFirst, strReferMI
                    strFile(POS_NM1C) = "NM1*DN*1*" & Left(strReferLast, 25) & "*" & Left(strReferFirst, 15) & "*" & strReferMI
                    If Len(!fldReferPhyID) > 0 Then strFile(POS_NM1C) = strFile(POS_NM1C) & "*MC*" & !fldReferPhyID
                    strFile(POS_NM1C) = strFile(POS_NM1C) & vbCrLf
                    lngNumSeg = lngNumSeg + 1
                End If
                
                strFile(POS_NM1C) = strFile(POS_NM1C) & "NM1*FA*2*" & Left(!fldBusinessName, 35) & "******" & vbCrLf
                lngNumSeg = lngNumSeg + 1
            
                strFile(POS_NM1C) = strFile(POS_NM1C) & "N3*" & Left(Replace(!fldProvAddress, "*", ""), 35) & vbCrLf
                lngNumSeg = lngNumSeg + 1
                strFile(POS_NM1C) = strFile(POS_NM1C) & "N4*" & Left(!fldProvCity, 30) & "*" & !fldProvState & "*" & !fldProvZip
                lngNumSeg = lngNumSeg + 1
                
                strFile(POS_LE) = "LE*2310"
                lngNumSeg = lngNumSeg + 1
                strFile(POS_LS2) = ""
            End With
            
            strFile(POS_LS2) = ""
            strFile(POS_SBR2) = ""
            strFile(POS_AMT2) = ""
            strFile(POS_NM1D) = ""
            
            With rstInsInfo
                If Not .EOF Then
                    'if no prim ins, not needed
                    strFile(POS_LS2) = "LS*2320"
                    lngNumSeg = lngNumSeg + 1
                    strFile(POS_SBR2) = "SBR*N**" & Left(!fldCardNum, 30) & "******"
                    lngNumSeg = lngNumSeg + 1
                    Select Case !fldInsuranceID
                        Case INS_MEDICAID
                            strFile(POS_SBR2) = strFile(POS_SBR2) & "MC"
                        Case INS_BCBS
                            strFile(POS_SBR2) = strFile(POS_SBR2) & "BL"
                        Case INS_WORKCOMP
                            strFile(POS_SBR2) = strFile(POS_SBR2) & "WC"
                        Case INS_MEDICARE
                            strFile(POS_SBR2) = strFile(POS_SBR2) & "MB"
                        Case Else
                            strFile(POS_SBR2) = strFile(POS_SBR2) & "ZZ"
                    End Select
                        
                    strTemp = Format(rstInsInfo!fldPaidAmount, "##.##")
                    If Right(strTemp, 1) = "." Then strTemp = Left(strTemp, Len(strTemp) - 1)
'                  If strTemp = "" Then strTemp = "0"
                    strFile(POS_AMT2) = "AMT*D*" & strTemp
                    lngNumSeg = lngNumSeg + 1
                    strFile(POS_NM1D) = "NM1*PR*2*" & Left(!fldInsName, 35) & vbCrLf
                    lngNumSeg = lngNumSeg + 1
                    strFile(POS_NM1D) = strFile(POS_NM1D) & "N3*" & Left(Replace(!fldAddress1, "*", ""), 35) & vbCrLf
                    lngNumSeg = lngNumSeg + 1
                    strFile(POS_NM1D) = strFile(POS_NM1D) & "N4*" & Left(!fldCity, 35) & "*" & !fldState & "*" & !fldZip & vbCrLf
                    lngNumSeg = lngNumSeg + 1
                    If Len(!fldPhone) > 0 Then
                        strFile(POS_NM1D) = strFile(POS_NM1D) & "PER*AI**TE*" & !fldPhone & vbCrLf
                        lngNumSeg = lngNumSeg + 1
                    End If
                    
                    If CLng(!fldDenied) > 0 Then
                        strFile(POS_NM1D) = strFile(POS_NM1D) & "REF*4N*D" & vbCrLf
                    ElseIf !fldPaidAmount > 0 Then
                        strFile(POS_NM1D) = strFile(POS_NM1D) & "REF*4N*P" & vbCrLf
                    Else
                        strFile(POS_NM1D) = strFile(POS_NM1D) & "REF*4N*R" & vbCrLf
                    End If
                    lngNumSeg = lngNumSeg + 1
                End If
            End With
            
            With rstData
                If Len(!fldEmployerName) > 0 Then strFile(POS_NM1D) = strFile(POS_NM1D) & "NM1*36*2*" & Left(!fldEmployerName, 35) & vbCrLf
                lngNumSeg = lngNumSeg + 1
                strFile(POS_NM1D) = strFile(POS_NM1D) & "NM1*IL*1*" & Left(!fldRPLast, 20) & "*" & Left(!fldRPFirst, 25) & "*" & IfNull(!fldRPMI, "") & "***"
                lngNumSeg = lngNumSeg + 1
            
                If !fldInsuranceID = INS_MEDICAID Then
                    strFile(POS_NM1D) = strFile(POS_NM1D) & "MD*" & Left(!fldCardNum, 25)
                Else
                    strFile(POS_NM1D) = strFile(POS_NM1D) & "C1*" & Left(!fldCardNum, 25)
                End If
                lngNumSeg = lngNumSeg + 1
                
                strFile(POS_LE2) = "LE*2320"
                lngNumSeg = lngNumSeg + 1
            
                strFile(POS_LX) = "LX*1" 'INCREMENT UP TO 28
                lngNumSeg = lngNumSeg + 1
            
                strTemp = Format(!fldFee, "##.##")
'               If strTemp = "" Then strTemp = "0"
                If Right(strTemp, 1) = "." Then strTemp = Left(strTemp, Len(strTemp) - 1)
                strFile(POS_SV) = "SV1*HC>" & !fldCPTCode & ">" & IfNull(!fldModifier, "") & ">>>>*" & strTemp & "*UN*" & !fldUnits & "**3*1" & vbCrLf
                lngNumSeg = lngNumSeg + 1
                strFile(POS_SV) = strFile(POS_SV) & "DTP*150*D8*" & Format(!fldDOS, "yyyymmdd") & vbCrLf
                lngNumSeg = lngNumSeg + 1
                strFile(POS_SV) = strFile(POS_SV) & "DTP*151*D8*" & Format(!fldDOS, "yyyymmdd")
                lngNumSeg = lngNumSeg + 1
            
            End With
            
'            lngNumSeg = lngNumSeg + 1
            strFile(POS_SEND) = "SE*" & lngNumSeg & "*" & Mid(Hex(TRANSMITNUMBER), 1, 6) & Format(lngNumSect, "000")
            WriteFileBody TRANSMITNUMBER, strFile
            rstData.MoveNext
        Wend
        strFile(POS_END) = strFile(POS_END) & "GE*" & lngNumSect & "*" & Hex(TRANSMITNUMBER) & vbCrLf
        strFile(POS_END) = strFile(POS_END) & "IEA*1*" & Format(Hex(TRANSMITNUMBER), "000000000") & vbCrLf
        
        WriteFileTrailer TRANSMITNUMBER, strFile
        
        rstEClaims.MoveNext
    Wend
    
    Set rstEClaims = Nothing
    Set rstData = Nothing
    Set rstInsInfo = Nothing
    Set obj = Nothing
    Screen.MousePointer = vbNormal
    
    MsgBox lngNumFiles & " file(s) generated.", vbInformation
    Exit Sub
ErrHand:
    Set rstEClaims = Nothing
    Set rstData = Nothing
    Set rstInsInfo = Nothing
    Set obj = Nothing
    Screen.MousePointer = vbNormal
    MsgBox "Error: " & Err.Number & " - " & Err.Description, vbCritical, "Error in file export"
End Sub
Private Sub ParseName(ByVal strInputName As String, ByRef strLast As String, ByRef strFirst As String, ByRef strMI As String)
    Dim lngPos1 As Integer
    Dim lngPos2 As Integer
    Dim lngPos3 As Integer
    
    strFirst = ""
    strLast = ""
    strMI = ""
    
    lngPos1 = InStr(strInputName, " ")
    If lngPos1 <= 0 Then Exit Sub
    
    strLast = Left(strInputName, lngPos1 - 1)
    
ResumeParse:
    lngPos2 = InStr(lngPos1 + 1, strInputName, " ")
    lngPos3 = InStr(lngPos1 + 1, strInputName, ",")
    
    If lngPos2 < 1 And lngPos3 < 1 Then Exit Sub
    
    If lngPos2 < 1 Or lngPos3 < 1 Then lngPos1 = Max(lngPos2, lngPos3) Else lngPos1 = Min(lngPos2, lngPos3)
    
    If Mid(strInputName, lngPos1 + 2, 1) = " " Or Mid(strInputName, lngPos1 + 2, 2) = ". " Then
        strMI = Mid(strInputName, lngPos1 + 1, 1)
        GoTo ResumeParse
    Else
        strLast = Mid(strInputName, lngPos1 + 1)
    End If
End Sub
Private Sub InitSettings()
    Dim strTemp As String
    Dim strLoc As String
    Dim lngPos As Long
    On Error GoTo ErrHand
    
    
    'make sure .ini exists
    strLoc = Dir(App.Path & "\EClaim.ini")
    
    If strLoc = "" Then
        'we are resetting
SetLoc:
        Open (App.Path & "\EClaim.ini") For Output As #1
        Print #1, "[EClaim]"
        
        MsgBox "Please choose an output directory", vbInformation, "Initialize settings"
        frmBrowse.Show vbModal
        strLoc = frmBrowse.dirList.Path
        If Right(strLoc, 1) <> "\" Then strLoc = strLoc & "\"
        If strLoc <> "" Then Print #1, "Output=" & strLoc
        Unload frmBrowse
        Close #1
    Else
        'find and set parameters from .ini
        strLoc = ""
        Open (App.Path & "\EClaim.ini") For Input As #1
        Do While Not EOF(1)
            Input #1, strTemp
            lngPos = InStr(strTemp, "=")
            If lngPos > 1 Then
                Select Case Left(strTemp, lngPos - 1)
                    Case "Output"
                        strLoc = Right(strTemp, Len(strTemp) - lngPos)
                End Select
            End If
        Loop
        
        
        Close #1
        
        'if no output dir is found, recreate file
        If strLoc = "" Then
            MsgBox "Invalid settings!", vbInformation, "Initialize settings"
            Kill App.Path & "\EClaim.ini"
            GoTo SetLoc
            Exit Sub
        End If
    End If
    
    CONST_OUTPUT_DIR = strLoc
    
    Exit Sub
ErrHand:
    Close #1
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub
Private Sub WriteFileHeader(ByVal strECNum As String, ByRef strFile() As String)
    On Error GoTo ErrHand
    
    If Dir(CONST_OUTPUT_DIR & "\" & strECNum & ".ECL") > "" Then Kill CONST_OUTPUT_DIR & "\" & strECNum & ".ECL"
    
    Open (CONST_OUTPUT_DIR & "\" & strECNum & ".ECL") For Output As #1
    
    Print #1, strFile(0)
    Print #1, strFile(1)
        
    Close #1
    
    Exit Sub
ErrHand:
    Close #1
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub
Private Sub WriteFileBody(ByVal strECNum As String, ByRef strFile() As String)
    Dim lngCtr As Long
    On Error GoTo ErrHand
    
    Open (CONST_OUTPUT_DIR & "\" & strECNum & ".ECL") For Append As #1
    For lngCtr = POS_ST To POS_SEND
        If Len(strFile(lngCtr)) > 0 Then Print #1, strFile(lngCtr)
    Next lngCtr
        
    Close #1
    
    Exit Sub
ErrHand:
    Close #1
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub
Private Sub WriteFileTrailer(ByVal strECNum As String, ByRef strFile() As String)
    On Error GoTo ErrHand
    
    Open (CONST_OUTPUT_DIR & "\" & strECNum & ".ECL") For Append As #1
    
    Print #1, strFile(POS_END)
    
    Close #1
    
    Exit Sub
ErrHand:
    Close #1
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub
Private Sub cmdDone_Click(Index As Integer)
    Select Case Index
        Case 0
            GenerateFile
        Case 1
            Unload Me
    End Select
End Sub
Private Sub Form_Load()
    InitSettings
End Sub
