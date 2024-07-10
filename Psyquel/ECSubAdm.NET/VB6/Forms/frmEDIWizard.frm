VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.2#0"; "MSCOMCTL.OCX"
Begin VB.Form frmEDIWizard 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Psyquel - Electronic Claims"
   ClientHeight    =   3750
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   5850
   Icon            =   "frmEDIWizard.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   3750
   ScaleWidth      =   5850
   ShowInTaskbar   =   0   'False
   StartUpPosition =   1  'CenterOwner
   Begin VB.CheckBox chkTest 
      Caption         =   "Submit claims as TEST"
      Height          =   255
      Left            =   240
      TabIndex        =   7
      Top             =   500
      Width           =   1935
   End
   Begin VB.Frame Frame1 
      Caption         =   "File Format"
      Height          =   1860
      Left            =   105
      TabIndex        =   3
      Top             =   850
      Width           =   5475
      Begin VB.TextBox txtStatus 
         Height          =   825
         Left            =   120
         Locked          =   -1  'True
         MultiLine       =   -1  'True
         ScrollBars      =   2  'Vertical
         TabIndex        =   4
         TabStop         =   0   'False
         Top             =   900
         Width           =   5175
      End
      Begin MSComctlLib.ProgressBar barStatus 
         Height          =   255
         Left            =   60
         TabIndex        =   5
         Top             =   360
         Width           =   5295
         _ExtentX        =   9340
         _ExtentY        =   450
         _Version        =   393216
         Appearance      =   1
      End
      Begin VB.Label lblStatus 
         Alignment       =   2  'Center
         Caption         =   "Creating electronic claim records ..."
         Height          =   255
         Left            =   0
         TabIndex        =   6
         Top             =   0
         Width           =   5295
      End
   End
   Begin VB.CommandButton cmdDone 
      Caption         =   "&Cancel"
      Height          =   420
      Index           =   1
      Left            =   3780
      TabIndex        =   1
      Top             =   2880
      Width           =   1140
   End
   Begin VB.CommandButton cmdDone 
      Caption         =   "&OK"
      Height          =   420
      Index           =   0
      Left            =   720
      TabIndex        =   0
      Top             =   2880
      Width           =   1140
   End
   Begin VB.Label Label1 
      Alignment       =   2  'Center
      Caption         =   "ANSI X.12 837 v 5010 Electronic Claim Submission"
      Height          =   255
      Left            =   240
      TabIndex        =   2
      Top             =   120
      Width           =   4215
   End
End
Attribute VB_Name = "frmEDIWizard"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Public g_FileName As String
Public g_strResponseFileName As String
Public g_FileNumber As Long
Public g_lngStartTxNum As Long
Public g_lngEndTxNum As Long
Private g_lngNumSeg As Long 'Data segment counter
Private blnEmailSuccess As Boolean
Private blnEmailFailure As Boolean

Private Declare Function OpenProcess Lib "kernel32" (ByVal dwDesiredAccess As Long, ByVal bInheritHandle As Long, ByVal dwProcessId As Long) As Long
Private Declare Function WaitForSingleObject Lib "kernel32" (ByVal hHandle As Long, ByVal dwMilliseconds As Long) As Long
Private Declare Function GetExitCodeProcess Lib "kernel32" (ByVal hProcess As Long, lpExitCode As Long) As Long
Private Declare Function CloseHandle Lib "kernel32" (ByVal hObject As Long) As Long

Private Const TX_ECLAIM_SUBMITTED As Long = 6
Private Const INFINITE = &HFFFFFFFF
Private Const WAIT_FAILED = -1&
Private Const STILL_ACTIVE = &H103&
Private Const PROCESS_ALL_ACCESS = &H1F0FFF
Private Const SYNCHRONIZE = &H100000
Private Const INSCO_TEXAS_WORKERS_COMP As Long = 606
Private Const INSCO_MEDICARE As Long = 24
Private Const INSCO_MEDICARE_PARTB As Long = 29
Private Const INSCO_MEDICAID As Long = 105
Private Const INSCO_BCBS As Long = 158
Private Const INSCO_BCBS_TX As Long = 6
Private Const INSCO_CHAMPUS As Long = 1050

Private CONST_OUTPUT_DIR As String

'----------------------
' Form Events
'----------------------
Private Sub Form_Load()

    Dim strMachineName As String
    
    On Error GoTo ErrTrap:
    
    strMachineName = GetMachineName()
    
    If Trim(strMachineName) <> "PSYEDI01" And Trim(strMachineName) <> "PSYQUEL-EDI" And Trim(strMachineName) <> "ADM-00" Then
        Call MsgBox("Electronic Claims can only be processed from the PSYQUEL-EDI Server.(" & strMachineName & ")", vbOKOnly + vbCritical, "Close Wizard")
        Me.Tag = "Cancel"
        Me.Hide
        Unload Me
        Exit Sub
    End If

    InitSettings
    Me.Show
    GenerateFile
    
    Call SendEmailMessage("Electronic Claims", "The Electronic Claims Daemon completed Successfully" & vbCrLf & Me.txtStatus.Text)

ErrTrap:
'    Err.Raise Err.Number, Err.Source, Err.Description
    Unload Me
End Sub


Private Sub cmdDone_Click(Index As Integer)
    Select Case Index
        Case 0
            Unload Me
        Case 1
            Unload Me
    End Select
End Sub


'----------------------
' Private Methods
'----------------------

Private Sub InitSettings()
    
    Dim strTemp As String
    Dim strLoc As String
    Dim lngPos As Long
    On Error GoTo ErrTrap:
    
    blnEmailSuccess = True
    blnEmailFailure = True
    
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
        If Right(strLoc, 1) <> "\" Then
            strLoc = strLoc & "\"
        End If
        If strLoc <> "" Then
            Print #1, "Output=" & strLoc
        End If
        
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
            MsgBox "Error"
            Kill App.Path & "\EClaim.ini"
            GoTo SetLoc
            Exit Sub
        End If
    End If
    
    CONST_OUTPUT_DIR = strLoc
    
    Exit Sub
ErrTrap:
    Close #1
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub

'-------------------------
' Public Routines
'-------------------------
Public Sub SendEmailMessage(ByVal strSubject As String, ByVal strMessage As String)
'--------------------------------------------------------------------
'Date: 05/24/2001
'Author: Eric Pena
'Description:  Sends an email message
'Parameters:
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------
   Const cdoSendUsingMethod = "http://schemas.microsoft.com/cdo/configuration/sendusing"
   Const cdoSendUsingPort = 2
   Const cdoSMTPServer = "http://schemas.microsoft.com/cdo/configuration/smtpserver"
   Const cdoSMTPServerPort = "http://schemas.microsoft.com/cdo/configuration/smtpserverport"
   Const cdoSMTPConnectionTimeout = "http://schemas.microsoft.com/cdo/configuration/smtpconnectiontimeout"
   Const cdoSMTPUsess = "http://schemas.microsoft.com/cdo/configuration/smtpusessl"
   Const cdoSMTPAuthenticate = "http://schemas.microsoft.com/cdo/configuration/smtpauthenticate"
   Const cdoBasic = 1
   Const cdoAnonymous = 0
   Const cdoSendUserName = "http://schemas.microsoft.com/cdo/configuration/sendusername"
   Const cdoSendPassword = "http://schemas.microsoft.com/cdo/configuration/sendpassword"
   
   Dim objConfig  As New CDO.Configuration
   Dim objMail    As New CDO.Message
   Dim Fields     As ADODB.Fields
   Dim strRecipient As String
   Dim strMachineName As String
    
   On Error GoTo ErrHand
    
' Set config fields we care about
   Set Fields = objConfig.Fields

   With Fields
    .Item(cdoSendUsingMethod) = 2
    .Item(cdoSMTPUsess) = True
    .Item(cdoSMTPServer) = "email-smtp.us-east-2.amazonaws.com"
    .Item(cdoSMTPServerPort) = 25
    .Item(cdoSMTPConnectionTimeout) = 10
    .Item(cdoSMTPAuthenticate) = 1
    .Item(cdoSendUserName) = "AKIAYYPPLV2F6KWGF6MB"
    .Item(cdoSendPassword) = "BDqepESBgKEyadAKOg3viIXz/V0Bz1LH1xGVWJRJduYc"
    .Update
   End With
   
    strRecipient = "QWareAdmin@Psyquel.com"
    strMachineName = GetMachineName()
   
    Set objMail.Configuration = objConfig
    objMail.From = strMachineName & "@psyquel.com"
    objMail.To = strRecipient
    objMail.Subject = strSubject
    objMail.TextBody = "The following message was generated from the Claim Generator running on " & strMachineName & ":" & vbCrLf & strMessage
    objMail.Send
    Set objMail = Nothing
    
    Exit Sub
    
ErrHand:
    Err.Raise Err.Number, Err.Source, Err.Description
    Call ShowError(Err)
End Sub

Private Sub GenerateFile()
'-------------------------------------------------------------------------------
'Author: Dave Richkun
'Date: 11/07/2002
'Description: Main function that generates EDI text file compliant to X.12 837 v4010
'Parameters:
'Returns:
'-------------------------------------------------------------------------------

On Error GoTo ErrTrap:

Dim objEDI As EDIBz.CEDIBz
Dim objClrHse As InsuranceBz.CClearingHouseBz
Dim rst As ADODB.Recordset
Dim rstClrHse As ADODB.Recordset
Dim strWhere, strFile As String
Dim lngClrHseID As Long
Dim lngPrevClrHseID As Long
Dim lngStatusID As Long
Dim strClaimType As String
Dim strPrevPayerCode As String
Dim strPrevSubmitterID As String
Dim strPrevClaimType As String
Dim strProgram, strLine As String
Dim dblProgID As Double
Dim intExit, intCnt, intWrk As Integer
Dim strMachineName As String
    
strMachineName = GetMachineName()
    
Dim objFS As Scripting.FileSystemObject

Dim objFolder, objFile
Dim objText As Scripting.TextStream

On Error GoTo ErrTrap:
   
Set objFS = CreateObject("Scripting.FileSystemObject")

Screen.MousePointer = vbHourglass
    
lngClrHseID = 1: intCnt = 1: lngPrevClrHseID = 0

    Set rst = getPendingVerif 'Fetch Verifications
            
    Do While Not rst.EOF
        lngClrHseID = 1
        If Not IsNull(rst.Fields("fldClearingHouseID").Value) Then lngClrHseID = rst.Fields("fldClearingHouseID").Value
        
        Set objClrHse = CreateObject("InsuranceBZ.CClearingHouseBz")
        Set rstClrHse = objClrHse.Fetch(False, "fldClearingHouseID = " & lngClrHseID)
        Set objClrHse = Nothing
    
        If rstClrHse.EOF = True Then lngClrHseID = 1

        If lngClrHseID <> lngPrevClrHseID Or _
           strPrevPayerCode <> IfNull(rst.Fields("fldPayerCodeVerify").Value, IfNull(rst.Fields("fldPayerCode").Value, "")) Then
            lngStatusID = OpenEDIFile(rstClrHse, "270", "")
            If lngClrHseID <> 66 Then lngPrevClrHseID = lngClrHseID
            strPrevPayerCode = IfNull(rst.Fields("fldPayerCodeVerify").Value, IfNull(rst.Fields("fldPayerCode").Value, ""))
   '         strPrevSubmitterID = IfNull(rst.Fields("SubmitterID").Value, "")
        End If
    
        g_lngEndTxNum = GenerateVerificationRequest(rstClrHse, rst, g_lngEndTxNum, g_FileNumber, g_FileName)
        
        CloseEDIFile
        
        If Me.Tag = "Cancel" Then Exit Do
    Loop
    Set rst = Nothing
    Set rstClrHse = Nothing

lngClrHseID = 1: intCnt = 1: lngPrevClrHseID = 0: strPrevClaimType = "": strPrevPayerCode = "": strPrevSubmitterID = ""
Set objClrHse = CreateObject("InsuranceBZ.CClearingHouseBz")
Set rstClrHse = objClrHse.Fetch(False, " fldNightlyProcessYN = 'Y'", "fldClearingHouseID")
If Trim(strMachineName) = "ADM-00" Then Set rstClrHse = objClrHse.Fetch(False, "fldClearingHouseID = 92", "fldClearingHouseID")
Set objClrHse = Nothing

txtStatus.Text = "Start: " & Format(Now(), "long date") & " " & Format(Now(), "long time") & vbCrLf
DoEvents
    
Do While Not rstClrHse.EOF

'txtStatus.Text = "Next: " & lngClrHseID & " " & rstClrHse.Fields("fldDescription").Value & vbCrLf & txtStatus.Text
'DoEvents

   If IsNull(rstClrHse.Fields("fldClearingHouseID").Value) Then
        lngClrHseID = 1
   Else
        lngClrHseID = rstClrHse.Fields("fldClearingHouseID").Value
   End If
   strWhere = " Where fldQueuedYN = 'Y' AND fldClearingHouseID = " & lngClrHseID
     
   Set objEDI = CreateObject("EDIBz.CEDIBz")
   Set rst = objEDI.FetchElectClaims(strWhere) 'Fetch claims
   Set objEDI = Nothing
    
   strPrevPayerCode = "": strPrevSubmitterID = "": g_FileName = "No File": g_lngNumClaims = 0
   If Not rst.EOF Then
      barStatus.Min = 0
      barStatus.Max = rst.RecordCount

      Do While Not rst.EOF
         If Not IsNull(rst.Fields("fldClaimType").Value) Then strClaimType = rst.Fields("fldClaimType").Value
        
         If lngClrHseID <> lngPrevClrHseID Or _
            strPrevClaimType <> IfNull(rst.Fields("fldClaimType").Value, "") Or _
            strPrevPayerCode <> IfNull(rst.Fields("fldPayerCode").Value, "") Or _
            strPrevSubmitterID <> IfNull(rst.Fields("SubmitterID").Value, "") Then
                lngStatusID = OpenEDIFile(rstClrHse, "txt", strClaimType)
                lngPrevClrHseID = lngClrHseID
                strPrevClaimType = IfNull(rst.Fields("fldClaimType").Value, "")
                strPrevPayerCode = IfNull(rst.Fields("fldPayerCode").Value, "")
                strPrevSubmitterID = IfNull(rst.Fields("SubmitterID").Value, "")
         End If
    
         g_lngEndTxNum = GenerateElectronicClaims(rstClrHse, rst, g_lngEndTxNum, g_FileNumber, g_FileName, True, "")
    
         CloseEDIFile
        
            'If Me.Tag = "Cancel" Then Exit Do
            If Not rst.EOF Then rst.MoveNext
      Loop
  
        'Free resources
      Set rst = Nothing
      If (IfNull(g_FileName, "") > "") And (g_FileName <> "No File") And _
        (Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(g_FileName, "")) > "") And _
        (Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ZSEND32.wax") > "" Or _
         Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") > "") Then
         Select Case rstClrHse.Fields("fldClearingHouseID").Value
            Case 1 To 11, 13, 15, 17 To 24, 26 To 30, 32 To 51, 54, 56 To 59, 62, 63, 68 To 72, 74, 75, 84 To 91, 93 To 103, 110, 111, 112, 118 To 123, 124, 125, 127, 131 To 134, 136, 138, 139, 141 To 146, 149 To 151, 154 To 158
                Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                If rstClrHse.Fields("fldSftpSite").Value > "" Then
                    If rstClrHse.Fields("fld270").Value = "Y" Then
                        Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Format(CStr(Month(Now)), "00") & "*" & Format(Now(), "YY") & "*.270" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa -log " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\COREFTP.LOG"
                    End If
                    Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Format(CStr(Month(Now)), "00") & "*" & Format(Now(), "YY") & "*.txt" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa -log " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\COREFTP.LOG"
                    If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*" & Format(Now(), "YY") & "*.837i") > "" And rstClrHse.Fields("fldClearingHouseID").Value <> 23 Then
                        Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Format(CStr(Month(Now)), "00") & "*" & Format(Now(), "YY") & "*.837i" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa -log " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\COREFTP.LOG"
                    End If
                    If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*" & Format(Now(), "YY") & "*.837i") > "" And rstClrHse.Fields("fldClearingHouseID").Value = 23 Then
                        Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Format(CStr(Month(Now)), "00") & "*" & Format(Now(), "YY") & "*.837i" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & "/Uploads/837i/" & Chr(34) & " -oa -log " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\COREFTP.LOG"
                    End If
                End If
                Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                Close #2 'Close the file

            Case 31        'BcBs Of MS has a different file name MULTIFILES
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\MULTIFILES") > "" Then
                    Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\MULTIFILES"
                End If
                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(g_FileName, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\MULTIFILES")

            Case 25        'BcBs Of NC has a different file name IMS.CLM
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\P_I_CEXALL") > "" Then
                    Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\P_I_CEXALL"
                End If
                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(g_FileName, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\P_I_CEXALL")
            Case 52        'BcBs Of TN has a different file name IMS.CLM
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & Left(IfNull(g_FileName, ""), InStr(1, IfNull(g_FileName, ""), ".")) & "X12") > "" Then Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & Left(IfNull(g_FileName, ""), InStr(1, IfNull(g_FileName, ""), ".")) & "X12"
                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName, App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Left(IfNull(g_FileName, ""), InStr(1, IfNull(g_FileName, ""), ".")) & "X12")
                Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Left(IfNull(g_FileName, ""), InStr(1, IfNull(g_FileName, ""), ".")) & "X12" & Chr(34) & " -site BcBs_Of_TN -p " & Chr(34) & "Inbox/" & Chr(34)
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "Outbox/*.*" & Chr(34) & " -site BcBs_Of_TN -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Chr(34) & " -delsrc -o "
                Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                Close #2 'Close the file
            Case 53        'BcBs of AZ has a different file name xxxx_0000????.ttt - 00008480.837
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\00008480.837") > "" Then
                    Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\00008480.837"
                End If
                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(g_FileName, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\00008480.837")
            Case 12, 55, 77, 78, 79, 80, 82, 83, 135, 147           'WPS
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\14541.dat") > "" Then
                    Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\14541.dat"
                End If
                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(g_FileName, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\14541.dat")
                Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\14541.dat" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa -log " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\COREFTP.LOG"
                Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                Close #2 'Close the file
            Case 60        'HealthNow has a different file name IMS.CLM
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\Z78232.A" & Format(DatePart("D", Now()), "00")) > "" Then
                    Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\Z78232.A" & Format(DatePart("D", Now()), "00")
                End If
                Open (App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\filename.txt") For Output As #2
                Print #2, "Z78232.A" & Format(DatePart("D", Now()), "00")
                Close #2 'Close the file
                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(g_FileName, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Z78232.A" & Format(DatePart("D", Now()), "00"))
            Case 64        'Medicaid NE has a different file name EDI000000265.837P.0001.DAT
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\EDI000000265.837P." & Format(DatePart("H", Now()), "00") & Format(DatePart("D", Now()), "00") & ".DAT") > "" Then
                    Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\EDI000000265.837P." & Format(DatePart("H", Now()), "00") & Format(DatePart("D", Now()), "00") & ".DAT"
                End If
                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(g_FileName, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\EDI000000265.837P." & Format(DatePart("H", Now()), "00") & Format(DatePart("D", Now()), "00") & ".DAT")
            Case 66        'BcBs Of RI - Can only send one file at a time
                Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                If rstClrHse.Fields("fldSftpSite").Value > "" Then
                    Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Format(CStr(Month(Now)), "00") & "*" & Format(Now(), "YY") & "*.txt" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa -log " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\COREFTP.LOG"
                End If
                Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                Close #2 'Close the file
                strProgram = App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat"
                dblProgID = Shell(strProgram, vbNormalFocus)
                intExit = WaitOnProgram(dblProgID, True)
                If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ERAs.bat") > "" Then
                    strProgram = App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ERAs.bat"
                End If
                dblProgID = Shell(strProgram, vbNormalFocus)
                intExit = WaitOnProgram(dblProgID, True)
                Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                If rstClrHse.Fields("fldSftpSite").Value > "" Then
                    If rstClrHse.Fields("fld270").Value = "Y" Then
                        Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Format(CStr(Month(Now)), "00") & "*" & Format(Now(), "YY") & "*.270" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa -log " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\COREFTP.LOG"
                    End If
                End If
                Close #2 'Close the file
            Case 76        'BcBs Of VT has a different file name - 00723801.X12
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\00723801.X12") > "" Then
                    Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\00723801.X12"
                End If
                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(g_FileName, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\00723801.X12")
            Case 92         'BcBs Of HA
                strFile = "ca" & Left(g_FileName, 10) & "ip.01"
                For intWrk = 0 To 9
                    If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & strFile) > "" Or Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\" & strFile) > "" Then strFile = "ca" & Left(g_FileName, 9) & intWrk & "ip.01"
                    If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & strFile) = "" And Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\" & strFile) = "" Then Exit For
                Next
                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(g_FileName, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & strFile)
                Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ca*ip.01" & Chr(34) & " -site BcBs_Of_HA -p / -o"
     '           If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.270*") > "" Then
     '               Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\la*ip.01" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa "
     '           End If
                Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                Close #2 'Close the file
            Case 104        'BcBs Of Alabama
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\pbcp000" & Format(Now(), "w") - 1 & ".8375010.clm") > "" Then Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\pbcp000" & Format(Now(), "w") - 1 & ".8375010.clm"
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\pbcp000" & Format(Now(), "w") - 1 & ".8375010.zip") > "" Then Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\pbcp000" & Format(Now(), "w") - 1 & ".8375010.zip"
                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(g_FileName, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\pbcp000" & Format(Now(), "w") - 1 & ".8375010.clm")
                Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Pack.bat") For Output As #2
               ' "C:\Program Files\7-Zip\7z.exe" a -t7z "D:\Data\Apps\QBill\BcBsOfAL\pbcp0001.8375010.zip" "D:\Data\Apps\QBill\BcBsOfAL\pbcp0004.8375010.clm"
                Print #2, Chr(34) & "C:\Program Files\WinZip\WINZIP32.EXE" & Chr(34) & " -min -a " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\pbcp000" & Format(Now(), "w") - 1 & ".8375010.zip" & Chr(34) & " " & Chr(34) & App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\pbcp000" & Format(Now(), "w") - 1 & ".8375010.clm" & Chr(34)
                Close #2 'Close the file
                strProgram = App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Pack.bat"
                dblProgID = Shell(strProgram, vbNormalFocus)
                intExit = WaitOnProgram(dblProgID, True)
                Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\pbcp000" & Format(Now(), "w") - 1 & ".8375010.zip" & Chr(34) & " -site BcBsOfAL -p /Inbox/"
                Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                Close #2 'Close the file
            Case 105        'Medicare Of Alabama
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\palp000" & Format(Now(), "w") - 1 & ".clm") > "" Then Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\palp000" & Format(Now(), "w") - 1 & ".clm"
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\palp000" & Format(Now(), "w") - 1 & ".zip") > "" Then Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\palp000" & Format(Now(), "w") - 1 & ".zip"
                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(g_FileName, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\palp000" & Format(Now(), "w") - 1 & ".clm")
                strProgram = Environ("WINZIP") & "\pkzip.exe " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\palp000" & Format(Now(), "w") - 1 & ".zip " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\palp000" & Format(Now(), "w") - 1 & ".clm"
                dblProgID = Shell(strProgram, vbNormalFocus)
                intExit = WaitOnProgram(dblProgID, True)
                Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\palp000" & Format(Now(), "w") - 1 & "0.zip" & Chr(34) & " -site MC_Of_AL -p " & Chr(34) & "/psyq0001/" & Chr(34)
                Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                Close #2 'Close the file
 '           Case 38        'Medicare Of Georga
 '               If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\pgap000" & Format(Now(), "w") - 1 & ".clm") > "" Then Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\pgap000" & Format(Now(), "w") - 1 & ".clm"
 '               If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\pgap000" & Format(Now(), "w") - 1 & ".zip") > "" Then Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\pgap000" & Format(Now(), "w") - 1 & ".zip"
 '               Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(g_FileName, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\pgap000" & Format(Now(), "w") - 1 & ".clm")
 '               strProgram = Environ("WINZIP") & "\pkzip.exe " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\pgap000" & Format(Now(), "w") - 1 & ".zip " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\pgap000" & Format(Now(), "w") - 1 & ".clm"
 '               dblProgID = Shell(strProgram, vbNormalFocus)
 '               intExit = WaitOnProgram(dblProgID, True)
 '               Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
 '               Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\pgap000" & Format(Now(), "w") - 1 & "0.zip" & Chr(34) & " -site MC_Of_GA -p " & Chr(34) & "/gaf76888/" & Chr(34)
 '               Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
 '               Close #2 'Close the file
            Case 85        'Medicare Of Mississippi
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\pmsp000" & Format(Now(), "w") - 1 & ".clm") > "" Then Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\pmsp000" & Format(Now(), "w") - 1 & ".clm"
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\pmsp000" & Format(Now(), "w") - 1 & ".zip") > "" Then Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\pmsp000" & Format(Now(), "w") - 1 & ".zip"
                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(g_FileName, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\pmsp000" & Format(Now(), "w") - 1 & ".clm")
                strProgram = Environ("WINZIP") & "\pkzip.exe " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\pmsp000" & Format(Now(), "w") - 1 & ".zip " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\pmsp000" & Format(Now(), "w") - 1 & ".clm"
                dblProgID = Shell(strProgram, vbNormalFocus)
                intExit = WaitOnProgram(dblProgID, True)
                Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\pmsp000" & Format(Now(), "w") - 1 & "0.zip" & Chr(34) & " -site MC_Of_MS -p " & Chr(34) & "/m7021157/" & Chr(34)
                Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                Close #2 'Close the file
 '           Case 17        'Medicare Of Tennesse
 '               If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ptnp000" & Format(Now(), "w") - 1 & ".clm") > "" Then Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ptnp000" & Format(Now(), "w") - 1 & ".clm"
  ''              If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ptnp000" & Format(Now(), "w") - 1 & ".zip") > "" Then Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ptnp000" & Format(Now(), "w") - 1 & ".zip"
 '               Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(g_FileName, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ptnp000" & Format(Now(), "w") - 1 & ".clm")
 '               strProgram = Environ("WINZIP") & "\pkzip.exe " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ptnp000" & Format(Now(), "w") - 1 & ".zip " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ptnp000" & Format(Now(), "w") - 1 & ".clm"
 '               dblProgID = Shell(strProgram, vbNormalFocus)
 '               intExit = WaitOnProgram(dblProgID, True)
 '               Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
 '               Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ptnp000" & Format(Now(), "w") - 1 & "0.zip" & Chr(34) & " -site MC_Of_TN -p " & Chr(34) & "/tn201312/" & Chr(34)
 '               Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
 '               Close #2 'Close the file
            Case 107              'BcBs of MT
                strFile = "37020" & Format(Now, "mmdd") & ".dap"
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & strFile) > "" Or Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\" & strFile) > "" Then strFile = "37021" & Format(Now, "mmdd") & ".dap"
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & strFile) > "" Or Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\" & strFile) > "" Then strFile = "37022" & Format(Now, "mmdd") & ".dap"
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & strFile) > "" Or Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\" & strFile) > "" Then strFile = "37023" & Format(Now, "mmdd") & ".dap"
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & strFile) > "" Or Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\" & strFile) > "" Then strFile = "37024" & Format(Now, "mmdd") & ".dap"
                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(g_FileName, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & strFile)
                Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & strFile & Chr(34) & " -site BcBs_Of_MT -p " & Chr(34) & "/input/" & Chr(34)
                Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                Close #2 'Close the file
'            Case 126              'WestHighLands
'                strFile = Format(Now, "mmddYY") & "_CL008"
'                strProgram = Environ("WINZIP") & "\pkzip.exe " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & strFile & ".zip " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & strFile
 '               dblProgID = Shell(strProgram, vbNormalFocus)
'                intExit = WaitOnProgram(dblProgID, True)
'                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(g_FileName, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & strFile)
'                Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
'                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & strFile & Chr(34) & " -site WestHighLands -p /inbox/ "
'                Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
'                Close #2 'Close the file
'            Case 127              'BcBs of LA
'                Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
'                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & " -site BcBs_Of_LA -p /P0013469/ "
 '               Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/P0013469/BCT*.OUT" & Chr(34) & " -site BcBs_Of_LA -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Chr(34) & " -delsrc -o "
'                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/P0013469/BC9*.OUT" & Chr(34) & " -site BcBs_Of_LA -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Chr(34) & " -delsrc -o "
'                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/P0013469/BCA*.OUT" & Chr(34) & " -site BcBs_Of_LA -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Chr(34) & " -delsrc -o "
'                Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
'                Close #2 'Close the file
            Case 129              'BcBs of MA
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & Left(IfNull(g_FileName, ""), InStr(1, IfNull(g_FileName, ""), ".")) & "837") > "" Then Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & Left(IfNull(g_FileName, ""), InStr(1, IfNull(g_FileName, ""), ".")) & "837"
                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName, App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Left(IfNull(g_FileName, ""), InStr(1, IfNull(g_FileName, ""), ".")) & "837")
                Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Left(IfNull(g_FileName, ""), InStr(1, IfNull(g_FileName, ""), ".")) & "837" & Chr(34) & " -site BcBs_Of_MA -p " & Chr(34) & "/e__company_tradingpartners_EFE_C3BS/Inbound/" & Chr(34)
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/e__company_tradingpartners_EFE_C3BS/Outbound/*.*" & Chr(34) & " -site BcBs_Of_MA -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Chr(34) & " -delsrc -o "
                Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                Close #2 'Close the file
            Case 140
                Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Format(CStr(Month(Now)), "00") & "*" & Format(Now(), "YY") & "*.txt" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa -log " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\COREFTP.LOG"
                If rstClrHse.Fields("fld270").Value = "Y" Then
                        Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Format(CStr(Month(Now)), "00") & "*" & Format(Now(), "YY") & "*.270" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa -log " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\COREFTP.LOG"
                End If
                Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                Close #2 'Close the file
            Case 153       'Trillium
                Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & " -site TrilliumCH_In -p  / "
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/*.*" & Chr(34) & " -site TrilliumCH_Out -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Chr(34) & " -delsrc -o "
                Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                Close #2 'Close the file
                Set objFolder = objFS.GetFolder(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\")
                Set objFile = objFolder.Files
                For Each objFile In objFolder.Files
                    If Len(objFile.Name) = 16 And IsNumeric(Left(objFile.Name, 12)) And Right(objFile.Name, 4) = ".txt" Then
                        Set objText = objFile.OpenAsTextStream(ForReading)
                        Do While objText.AtEndOfStream = False
                            strLine = objText.ReadLine
                            If InStr(1, strLine, "NM1*85") Then
                                If InStr(1, strLine, "1457696262") Then
                                End If
                                If InStr(1, strLine, "1205070489") Then
                                    Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                                    Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & objFile.Name & Chr(34) & " -site TrilliumGP_In -p  / "
                                    Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/*.*" & Chr(34) & " -site TrilliumGP_Out -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Chr(34) & " -delsrc -o "
                                    Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                                    Close #2 'Close the file
                                End If
                                Exit Do
                            End If
                        Loop
                    End If
                Next
                Set objFile = Nothing
                Set objFolder = Nothing
            Case 159        'TricareEast
                Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & objFile.Name & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa -log " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\COREFTP.LOG"
                If rstClrHse.Fields("fld270").Value = "Y" Then
                        Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Format(CStr(Month(Now)), "00") & "*" & Format(Now(), "YY") & "*.270" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa -log " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\COREFTP.LOG"
                End If
                Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                Close #2 'Close the file
            Case Else
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ElecClaim.txt") > "" Then
                    Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ElecClaim.txt"
                End If
                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(g_FileName, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ElecClaim.txt")
                Open (App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\filename.txt") For Output As #2
                Print #2, IfNull(g_FileName, "")
                Close #2 'Close the file
            End Select
            
            If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") > "" Then        'ThinEDI uses FTP
                strProgram = App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat"
            Else
                strProgram = Environ("PROCOMM") & "\pw5.exe " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ZSEND32.wax"
            End If

            dblProgID = Shell(strProgram, vbNormalFocus)
            intExit = WaitOnProgram(dblProgID, True)
            
            'Move submission files to Submit folder
            If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt") > "" Then
              Select Case rstClrHse.Fields("fldClearingHouseID").Value
                Case 1 To 11, 13, 15, 17 To 24, 26 To 30, 32 To 51, 54, 56 To 59, 62, 63, 66, 68 To 72, 74, 75, 84 To 91, 93 To 103, 110, 111, 112, 118 To 123, 124, 125, 127, 131 To 134, 136, 138, 139, 142 To 146, 149 To 151, 154 To 159
                    If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.271*") > "" Then
                        Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.271*", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\271\")
                    End If
                    If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\271_*") > "" Then
                        Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\271_*", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\271\")
                    End If
                    If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\277_*") > "" Then
                        Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\277_*", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\277\")
                    End If
                    If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Format(CStr(Month(Now)), "00") & "*" & Format(Now(), "YY") & "*.270*") > "" Then
                        Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Format(CStr(Month(Now)), "00") & "*" & Format(Now(), "YY") & "*.270*", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\270\")
                    End If
                    Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*" & Format(Now(), "YY") & "*.txt", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                    If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*" & Format(Now(), "YY") & "*.837i") > "" Then
                        Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*" & Format(Now(), "YY") & "*.837i", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                    End If
                    If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt") > "" Then
                        Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt"
                    End If
                Case 52        'BcBs Of TN has a different file name .X12
                    Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*" & Format(Now(), "YY") & "*.txt", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                    Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*" & Format(Now(), "YY") & "*.X12", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                    If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt") > "" Then
                        Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt"
                    End If
                Case 64        'Medicaid NE has a different file name EDI000000265.837P.0001.DAT
                    Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*" & Format(Now(), "YY") & "*.txt", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                    If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\EDI000000265.837P." & Format(DatePart("H", Now()), "00") & Format(DatePart("D", Now()), "00") & ".DAT") > "" Then
                        Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\EDI000000265.837P." & Format(DatePart("H", Now()), "00") & Format(DatePart("D", Now()), "00") & ".DAT"
                    End If
                    Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*" & Format(DatePart("D", Now()), "00") & "*.DAT", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                    If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt") > "" Then
                        Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt"
                    End If
                Case 92         'BcBs Of HA
                    Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(g_FileName, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                    Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & strFile, App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                    Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt"
                Case 140         'BcBs Of WA
                    Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Format(CStr(Month(Now)), "00") & "*" & Format(Now(), "YY") & "*.txt", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                    If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.271*") > "" Then
                        Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.271*", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\271\")
                    End If
                    If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.270*") > "" Then
                        Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.270*", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\270\")
                    End If
                    If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt") > "" Then
                        Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt"
                    End If
                Case Else
                    Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(g_FileName, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                    If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.271*") > "" Then
                        Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.271*", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\271\")
                    End If
                    If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.270*") > "" Then
                        Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.270*", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\270\")
                    End If
                    Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt"
                    If rstClrHse.Fields("fldClearingHouseID").Value = 129 Then
                        Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Left(IfNull(g_FileName, ""), InStr(1, IfNull(g_FileName, ""), ".")) & "837", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                    End If
              End Select
              
            ' Send Email Message if Status Text File Failed
            Else
                Call SendEmailMessage("Claim EDI Failed", "Claims file not created for " & rstClrHse.Fields("fldClearingHouseID").Value & " - " & rstClrHse.Fields("fldFolder").Value & IfNull(g_FileName, ""))
            End If
                
            'Check FTP Log and verify claim files were sent
            If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\COREFTP.LOG") > "" Then
                If ReadFtpLog(rstClrHse, "COREFTP.LOG", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\") = "Fail" Then
                    Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\COREFTP.LOG", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\LOG\COREFTP_" & Format(Now, "YYYYmmdd") & Second(Now) & ".LOG")
                    Call SendEmailMessage("Coreftp failed", "Claims file not sent: " & rstClrHse.Fields("fldClearingHouseID").Value & " - " & rstClrHse.Fields("fldFolder").Value & IfNull(g_FileName, ""))
                Else
                    Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\COREFTP.LOG"
                End If
            End If
        'End If
              
      End If
            
   End If
            
   'Find any missed files and transmit them
   If rstClrHse.Fields("fldClearingHouseID").Value = 92 Then
      Set objFolder = objFS.GetFolder(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\")
      Set objFile = objFolder.Files
      For Each objFile In objFolder.Files
         If Len(objFile.Name) = 16 And IsNumeric(Mid(objFile.Name, 1, 12)) And Right(objFile.Name, 4) = ".txt" Then
            Select Case rstClrHse.Fields("fldClearingHouseID").Value
               Case 92         'BcBs Of HA
                    strFile = "ca" & Left(objFile.Name, 10) & "ip.01"
                    If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & strFile) > "" Then strFile = "ca" & Left(objFile.Name, 9) & "0ip.01"
                    For intWrk = 0 To 9
                        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & strFile) > "" Or Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\" & strFile) > "" Then strFile = "ca" & Left(objFile.Name, 9) & intWrk & "ip.01"
                        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & strFile) = "" And Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\" & strFile) = "" Then Exit For
                    Next
                    Call objFS.CopyFile(objFile, App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & strFile)
                    Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                    Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ca*ip.01" & Chr(34) & " -site BcBs_Of_HA -p / -o"
     '               If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.270*") > "" Then
     '                       Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\la*ip.01" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa "
     '               End If
                    Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                    Close #2 'Close the file
                    If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") > "" Then
                        strProgram = App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat"
                        dblProgID = Shell(strProgram, vbNormalFocus)
                        intExit = WaitOnProgram(dblProgID, True)
                    End If
                    If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt") > "" Then
                        Call objFS.MoveFile(objFile, App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                        Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & strFile, App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                        Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt"
                    End If
            End Select
         End If
      Next
      Set objFile = Nothing
      Set objFolder = Nothing
   End If
   
   If IfNull(rstClrHse.Fields("fldRemittanceYN").Value, "") = "Y" And _
        (Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ZRECV32.wax") > "" Or _
         Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ERAs.bat") > "") Then
          '  If (Format(Now, "w") = 2 Or Format(Now, "w") = 4 Or Format(Now, "w") = 6) And Hour(Now) < 12 Then '2-Monday, 4-Wednesday, 6-friday and in the AM
          '  If Hour(Now) < 12 Or rstClrHse.Fields("fldClearingHouseID").Value = 23 Then 'Run in the AM
                If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ERAs.bat") > "" Then        'ThinEDI uses FTP
                    strProgram = App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ERAs.bat"
                Else
                    strProgram = Environ("PROCOMM") & "\pw5.exe " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ZRECV32.wax"
                End If
                dblProgID = Shell(strProgram, vbNormalFocus)
                If rstClrHse.Fields("fldClearingHouseID").Value <> 23 Then
                    intExit = WaitOnProgram(dblProgID, True)
                End If
          '  End If
   End If
             
   If rstClrHse.Fields("fldClearingHouseID").Value = 50 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 56 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 64 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 86 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 92 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 104 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 105 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 110 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 111 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 116 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 131 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 132 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 133 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 136 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 151 Then
                If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Unpack.bat") > "" Then
                    strProgram = App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Unpack.bat"
                    dblProgID = Shell(strProgram, vbNormalFocus)
                    intExit = WaitOnProgram(dblProgID, True)
                End If
   End If
        
   If rstClrHse.Fields("fldClearingHouseID").Value <> 48 Then
            Call MoveFiles(rstClrHse, CLng(rstClrHse.Fields("fldClearingHouseID").Value))
   End If
    
        
    txtStatus.Text = "Complete: " & lngClrHseID & " Folder: " & rstClrHse.Fields("fldFolder").Value & "/" & g_FileName & " Claims: " & barStatus.Max & vbCrLf & txtStatus.Text
    DoEvents

    rstClrHse.MoveNext

Loop
    
rstClrHse.Close
Set rstClrHse = Nothing
Screen.MousePointer = vbDefault
    
Exit Sub
    
ErrTrap:
    txtStatus.Text = "Error:" & Err.Description & vbLf & vbLf & "Error Number: " & Err.Number & vbCrLf & txtStatus.Text
    DoEvents
    Call ShowError(Err)
    If blnEmailFailure Then
        Call SendEmailMessage("Electronic Claims Failure", "The Electronic Claims Daemon reported a failure in " & Err.Source & "." & vbCrLf & "Unhandled Error: " & Err.Number & ": " & Err.Description & vbCrLf & Me.txtStatus.Text)
    End If
    Set objEDI = Nothing
    Set rstClrHse = Nothing
    Screen.MousePointer = vbDefault
    Unload Me
End Sub


Public Function ReadFtpLog(ByVal rstClrHse As ADODB.Recordset, ByVal strFileName As String, ByVal curDir As String) As String
    
   Screen.MousePointer = vbHourglass
        
   Dim objFS As Scripting.FileSystemObject
   Dim objFile As Scripting.File
   Dim objText As Scripting.TextStream
   Dim strLine As String
   Dim lngCtr, intPosStart, intPosEnd As Long
    
   Dim strStatus, strMsg, strMsgCode As String
   Dim Cnt As Integer
   Dim intChr, intBefore, intAfter As Long
          
   Const ForReading = 1, ForWriting = 2, ForAppending = 3
        
   Dim Complete, Processed As Boolean
    
   On Error GoTo ErrTrap:
      
   Set objFS = CreateObject("Scripting.FileSystemObject")
   Set objFile = objFS.GetFile(curDir & strFileName)
   Set objText = objFile.OpenAsTextStream(ForReading)
   
   Complete = False: Processed = False
   strLine = objText.ReadLine
   Do While Not objText.AtEndOfStream ' objText.AtEndOfStream    ' Loop until end of file.
  '  Debug.Print strLine    ' Print to the Immediate window.
      If InStr(1, strLine, "SFTP connection error") > 0 Then
         ReadFtpLog = "Fail"
         Exit Function
      End If
      strLine = objText.ReadLine
   Loop
'   Close objFile    ' Close file.
   ReadFtpLog = "Pass"
   
   Exit Function
ErrTrap:
    Call MsgBox("Error: " & curDir & " : " & strFileName, vbOKOnly, "Error Message")
    Screen.MousePointer = vbDefault
    Call ShowError(Err)
End Function


Private Function OpenEDIFile(ByVal rstClrHse As ADODB.Recordset, strFileType, strClaimType) As Long
'Opens/creates a new EDI file.  File name is based on the create date (MMDDYY)
'and a unique file number retrieved from the database.

   Dim objEDI As EDIBz.CEDIBz
   Dim objCLH As InsuranceBz.CClearingHouseBz
   Dim rst As ADODB.Recordset
   Dim strFileName As String
   Dim lngStartTxNum As Long
   
   On Error GoTo ErrTrap:
    
   Set objEDI = CreateObject("EDIBZ.CEDIBz")
   Set rst = objEDI.GetNextFileNumber()
   Set objEDI = Nothing
    
   If rst.EOF = True And rst.BOF Then
      'Defensive programming for first time run-through
      g_FileNumber = 1
      g_lngStartTxNum = 1
      g_lngEndTxNum = 1
   Else
      g_FileNumber = rst.Fields("fldFileID").Value + 1
      g_lngStartTxNum = rst.Fields("fldEndTxNum").Value + 1
      g_lngEndTxNum = rst.Fields("fldEndTxNum").Value + 1
   End If
         
   If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, ""), vbDirectory) = "" Then
      MkDir (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, ""))
   End If
   If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Sub", vbDirectory) = "" Then
      MkDir (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Sub")
   End If
   If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Log", vbDirectory) = "" Then
      MkDir (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Log")
   End If
   If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\RSP", vbDirectory) = "" Then
      MkDir (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\RSP")
   End If
   If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK", vbDirectory) = "" Then
      MkDir (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK")
      MkDir (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\Accepted")
      MkDir (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\Rejected")
   End If
   If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\277", vbDirectory) = "" Then
      MkDir (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\277")
      MkDir (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\277\Accepted")
      MkDir (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\277\Rejected")
   End If
   If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\835", vbDirectory) = "" Then
      MkDir (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\835")
      MkDir (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\835\Archive")
   End If
   
    'Check last Clearing House File Name
   If (IfNull(rstClrHse.Fields("fldConcatFileYN").Value, "") = "Y") And _
        (IfNull(rstClrHse.Fields("fldEFileName").Value, "") > "") And _
        (Right(IfNull(rstClrHse.Fields("fldEFileName").Value, ""), 3) = strFileType) And _
        (Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(rstClrHse.Fields("fldEFileName").Value, "")) > "") Then
      g_FileName = IfNull(rstClrHse.Fields("fldEFileName").Value, "")
      Open (App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & g_FileName) For Append As #1
   Else
      Set rst = Nothing
      strFileName = Format(CStr(Month(Now)), "00") & Format(CStr(Day(Now)), "00") & Mid(CStr(Year(Now)), 3)
        
      If strFileType = "270" Then
        g_FileName = strFileName & CStr(g_FileNumber) & ".270" 'Append file number to filename to guarantee uniqueness
      Else
        If strClaimType = "I" Then
            g_FileName = strFileName & CStr(g_FileNumber) & ".837i" 'Institutional Claims
        Else
            g_FileName = strFileName & CStr(g_FileNumber) & ".txt" 'Append file number to filename to guarantee uniqueness
        End If
      End If
    '
    
      If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, ""), vbDirectory) = "" Then
         MkDir (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, ""))
      End If
      If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Sub", vbDirectory) = "" Then
         MkDir (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Sub")
      End If
        
      If (IfNull(rstClrHse.Fields("fld270").Value, "N") = "Y") Then
         If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\270", vbDirectory) = "" Then
            MkDir (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\270")
         End If
         If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\271", vbDirectory) = "" Then
            MkDir (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\271")
            MkDir (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\271\Archive")
         End If
      End If
      
      'Overwrite file if it exists
      If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & g_FileName) > "" Then
            Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & g_FileName
      End If
    
      Set objCLH = CreateObject("InsuranceBZ.CClearingHouseBz")
      Call objCLH.UpdateEFile(rstClrHse.Fields("fldClearingHouseID").Value, g_FileName)
      Set objCLH = Nothing
        
      Open (App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & g_FileName) For Output As #1
   End If
    
   OpenEDIFile = 1
    
   Exit Function
    
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description
    Set objEDI = Nothing
End Function

Private Function CloseEDIFile()
'Closes the file, and inserts a record of the file in the database.

    Dim objEDI As EDIBz.CEDIBz
    Dim strUserName As String

    On Error GoTo ErrTrap:
    
    Close #1 'Close the file
    
    strUserName = GetLoginName()
    
    If g_lngEndTxNum < g_lngStartTxNum Then
        Call MsgBox("An error has resulted that prevents these claims from being updated correctly.  Please try to send these claims again.", vbOKOnly + vbCritical, "Submission Error")
        Me.Tag = "Cancel"
    Else
        Set objEDI = CreateObject("EDIBz.CEDIBz")
        Call objEDI.Insert(g_FileNumber, g_FileName, g_lngStartTxNum, g_lngEndTxNum, strUserName)
        Set objEDI = Nothing
    End If
    
    Exit Function
    
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description
    Call ShowError(Err)
    Set objEDI = Nothing
    
End Function


Private Function WaitOnProgram(ByVal lngProgID As Long, Optional ByVal blnWaitDead As Boolean) As Long
'-----------------------------------------------------------------------------------
'Author: Dave Richkun
'Date: 23-Nov-1998
'Description: This routine forces Windows to halt execution until the Windows program
'             identified by the lngProgID parameter has finished executing.  Visual
'             Basic's 'Shell' command executes programs asynchronously, a nice feature,
'             but unwanted in the this particular case.
'Parameters:  lngProgID - The ProcessID of the Windows program we are monitoring
'             blnWaitDead - An optional parameter that identifies if execution should
'               be stopped in it's tracks until the monitored process has completed.
'Returns: A long integer identifying success (non-zero) or failure (zero)
'-----------------------------------------------------------------------------------
'Revision History:
'
'-----------------------------------------------------------------------------------

    Dim lngRead As Long
    Dim lngExit As Long
    Dim lngHProg As Long
    Dim lngResult As Long
        
    On Error GoTo ErrTrap:
        
    'Get the process handle of the running program
    lngHProg = OpenProcess(PROCESS_ALL_ACCESS, False, lngProgID)
    If blnWaitDead Then
        'Stop dead until the process terminates
        lngResult = WaitForSingleObject(lngHProg, INFINITE)
        
        If lngResult = WAIT_FAILED Then
            If Err.Source > "" Then
                Err.Raise Err.LastDllError
            End If
        End If
        'Get the return value
        lngResult = GetExitCodeProcess(lngHProg, lngExit)
    Else
        'Get the return value
        lngResult = GetExitCodeProcess(lngHProg, lngExit)
        'Wait, but allow painting and other processing
        Do While lngExit = STILL_ACTIVE
            DoEvents
            lngResult = GetExitCodeProcess(lngHProg, lngExit)
        Loop
    End If
    lngResult = CloseHandle(lngHProg)
    
    WaitOnProgram = lngExit
    
    Exit Function
    
ErrTrap:
    Err.Raise Err.Number, Err.Source, Err.Description
    Call ShowError(Err)
    
End Function

Public Sub MoveFiles(ByVal rstClrHse As ADODB.Recordset, intClrHse)
'Moves Response File from App folder to App\Response folder

Dim MyChar As String
Dim I, cntWrk As Long
Dim curDir, curFile As String
Dim inFile As String
Dim strLine As String
Dim dblProgID As Double
Dim objFS As Scripting.FileSystemObject
Const ForReading = 1, ForWriting = 2, ForAppending = 3
Dim objFolder, objFile
Dim objText As Scripting.TextStream

On Error Resume Next
'On Error GoTo ErrTrap:
    
Set objFS = CreateObject("Scripting.FileSystemObject")
        
    If rstClrHse.Fields("fldClearingHouseID").Value = 1 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 35 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 62 Then        'Thin, BCBS of OR, BcBs of FL
    '   Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.era", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.997", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.999", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.PCC", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\PCC\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.ECC", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ECC\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.SFC", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\SFC\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.WPS", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\WPS\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.PDF", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\PDF\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.EPS*", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\EPS\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.270.*success", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\270\")
        'Delete HyperTerminal trace files
        Call objFS.DeleteFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.dbg")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*success", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
    End If
         
    If rstClrHse.Fields("fldClearingHouseID").Value = 22 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 84 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 124 Then  'Tricare West, TriWest, BcBs of SC
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.RPT", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\RPT\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.RPT.*", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\RPT\")
   '     Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.X12", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
    End If
        
    If rstClrHse.Fields("fldClearingHouseID").Value = 66 Then        'BCBS of RI
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835_*", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\997_*", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\999_*", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\271_*", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\271\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\277_*", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\277\")
    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 54 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 75 Then 'Medicare NCA SCA OH SC
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\997*.RSP", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\999*.RSP", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
    End If
         
    If rstClrHse.Fields("fldClearingHouseID").Value = 109 Then  'BcBs of Iowa
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.Z16", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\")
    End If
             
    If rstClrHse.Fields("fldClearingHouseID").Value = 150 Then  'ClaimMD
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.txt.result", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.270.result", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\270\")
    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 134 Then  'GateWay EDI
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.DAT", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.999", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 25 Then        'BcBs of NC
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\P_O_CEXACK") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\P_O_CEXACK", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\" & Format(Now, "YYYYmmdd") & Hour(Now) & ".ACK")
        End If
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\P_O_CE0689") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\P_O_CE0689", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Format(Now, "YYYYmmdd") & Hour(Now) & ".RSP")
        End If
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\P_O_CE835R") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\P_O_CE835R", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Format(Now, "YYYYmmdd") & Hour(Now) & ".835")
        End If
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\P_O_CE0689_HIP_FTP_AUDIT") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\P_O_CE0689_HIP_FTP_AUDIT", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Format(Now, "YYYYmmdd") & Hour(Now) & ".RSP")
        End If
    End If
            
    If rstClrHse.Fields("fldClearingHouseID").Value = 127 Then        'BcBs of LA
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\BCTA1.out") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\BCTA1.out", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\" & Format(Now, "YYYYmmdd") & Hour(Now) & ".TA1")
        End If
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\BC999.out") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\BC999.out", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\" & Format(Now, "YYYYmmdd") & Hour(Now) & ".ACK")
        End If
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\BCAccNotAccRep.out") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\BCAccNotAccRep.out", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Format(Now, "YYYYmmdd") & Second(Now) & ".RSP")
        End If
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\BC5010835.out") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\BC5010835.out", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Format(Now, "YYYYmmdd") & Second(Now) & ".835")
        End If
    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 31 Then        'BcBs of MS
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179GPM\999PBLUEP") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179GPM\999PBLUEP", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\999PBLUEP-" & Format(Now, "YYYYmmdd") & Second(Now) & ".ACK")
        End If
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179GPM\HCFAREPORT") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179GPM\HCFAREPORT", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\HCFAREPORT-" & Format(Now, "YYYYmmdd") & Second(Now) & ".RSP")
        End If
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179GPM\999PCLMSP") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179GPM\999PCLMSP", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\999PCLMSP-" & Format(Now, "YYYYmmdd") & Second(Now) & ".ACK")
        End If
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179GPM\RPTPCLMSP") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179GPM\RPTPCLMSP", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\RSP\RPTPCLMSP-" & Format(Now, "YYYYmmdd") & Second(Now) & ".RSP")
        End If
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179GPM\5010835SHIELD") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179GPM\5010835SHIELD", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\835\5010835SHIELD-" & Format(Now, "YYYYmmdd") & Second(Now) & ".835")
        End If
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179GPM\277CAPCLMSP") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179GPM\277CAPCLMSP", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\277\277CAPCLMSP-" & Format(Now, "YYYYmmdd") & Second(Now) & ".277")
        End If
        
        
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179ISF\999PBLUEP") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179ISF\999PBLUEP", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\999PBLUEP-" & Format(Now, "YYYYmmdd") & Second(Now) & ".ACK")
        End If
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179ISF\HCFAREPORT") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179ISF\HCFAREPORT", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\HCFAREPORT-" & Format(Now, "YYYYmmdd") & Second(Now) & ".RSP")
        End If
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179ISF\999PCLMSP") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179ISF\999PCLMSP", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\999PCLMSP-" & Format(Now, "YYYYmmdd") & Second(Now) & ".ACK")
        End If
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179ISF\RPTPCLMSP") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179ISF\RPTPCLMSP", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\RSP\RPTPCLMSP-" & Format(Now, "YYYYmmdd") & Second(Now) & ".RSP")
        End If
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179ISF\5010835SHIELD") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179ISF\5010835SHIELD", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\835\5010835SHIELD-" & Format(Now, "YYYYmmdd") & Second(Now) & ".835")
        End If
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179ISF\277CAPCLMSP") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179ISF\277CAPCLMSP", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\277\277CAPCLMSP-" & Format(Now, "YYYYmmdd") & Second(Now) & ".277")
        End If
        
        
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179IUL\999PBLUEP") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179IUL\999PBLUEP", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\999PBLUEP-" & Format(Now, "YYYYmmdd") & Second(Now) & ".ACK")
        End If
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179IUL\HCFAREPORT") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179IUL\HCFAREPORT", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\HCFAREPORT-" & Format(Now, "YYYYmmdd") & Second(Now) & ".RSP")
        End If
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179IUL\999PCLMSP") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179IUL\999PCLMSP", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\999PCLMSP-" & Format(Now, "YYYYmmdd") & Second(Now) & ".ACK")
        End If
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179IUL\RPTPCLMSP") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179IUL\RPTPCLMSP", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\RSP\RPTPCLMSP-" & Format(Now, "YYYYmmdd") & Second(Now) & ".RSP")
        End If
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179IUL\5010835SHIELD") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179IUL\5010835SHIELD", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\835\5010835SHIELD-" & Format(Now, "YYYYmmdd") & Second(Now) & ".835")
        End If
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179IUL\277CAPCLMSP") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179IUL\277CAPCLMSP", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\277\277CAPCLMSP-" & Format(Now, "YYYYmmdd") & Second(Now) & ".277")
        End If
        
    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 68 Then        'BcBs of MI
    Set objFolder = objFS.GetFolder(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\")
    Set objFile = objFolder.Files
        
    For Each objFile In objFolder.Files
        inFile = objFile.Name
        curFile = objFile
        If inFile > "" And (InStr(1, inFile, "835P5010") > 0) Then
                Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\" & Left(inFile, InStr(1, inFile, "-") - 1) & ".835")
        End If
        If inFile > "" And (InStr(1, inFile, "R277CAA") > 0 Or InStr(1, inFile, "R277CAF") > 0 Or InStr(1, inFile, "R277CAK") > 0) Then
                Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\" & Left(inFile, InStr(1, inFile, "-") - 1) & ".rsp")
        End If
        If inFile > "" And (InStr(1, inFile, "999P") > 0) Then
                Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\" & Left(inFile, InStr(1, inFile, "-") - 1) & ".dat")
        End If
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\c0iej*.txt", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\")
 '       Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835P5010*.A", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
 '       Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\R277CAA*.A", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\")
 '       Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\R277CAF*.A", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\")
 '       Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\R277CAK*.A", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\")
 '       Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\999P*.*", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
          
    Next
       
    Set objFile = Nothing
    Set objFolder = Nothing
    End If
       

If rstClrHse.Fields("fldFolder").Value = "eSolutions" Or _
       rstClrHse.Fields("fldFolder").Value = "ECC-000600076" Or _
       rstClrHse.Fields("fldFolder").Value = "ECC-BBB33651B" Or _
       rstClrHse.Fields("fldFolder").Value = "ECC-BS00719" Or _
       rstClrHse.Fields("fldFolder").Value = "ECC-BS01000189" Then      'Noridian
       Set objFolder = objFS.GetFolder(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\")
       Set objFile = objFolder.Files
        
      For Each objFile In objFolder.Files
         inFile = objFile.Name
         curFile = objFile
            
         inFile = Replace(inFile, "~#", "")
         inFile = Replace(inFile, "#~", "_")
         If inFile > "" And (Right(inFile, 4) = ".277" Or Right(inFile, 6) = ".277CA" Or InStr(1, inFile, "277CA") > 0) Then
            Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\277\" & inFile)
         End If
          If inFile > "" And (Right(inFile, 4) = ".999" Or InStr(1, inFile, "999.") > 0) Then
                inFile = Left(inFile, InStr(1, inFile, ".txt.20") + 3)
                Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\" & Replace(Replace(inFile, ".", "_"), "_txt", ".txt"))
          End If
          If inFile > "" And (Right(inFile, 4) = ".TRN" Or InStr(1, inFile, "TRN.") > 0) Then
                Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\" & inFile)
          End If
          If inFile > "" And Right(inFile, 8) = "_835.edi" And InStr(1, inFile, "-") > 0 Then   'Noridian
                Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\" & Right(inFile, Len(inFile) - InStr(1, inFile, "-") - 1))
          End If
          If inFile > "" And Right(inFile, 7) = "835.dat" Then
                Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\" & Left(inFile, 28) & ".835")
          End If
          If inFile > "" And (InStr(1, inFile, "835Ansi") > 0 Or InStr(1, inFile, "5010835") > 0) Then
                Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\" & inFile)
          End If
          
          If inFile > "" And Right(inFile, 8) = "_TA1.edi" And InStr(1, inFile, ".txt") > 0 And Len(inFile) > 24 Then   'Noridian
          '78f97a12bac84050_abe33289790e1a63_100819429376.txt_TA1.edi
                Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\" & Right(inFile, 24))
          End If
          If inFile > "" And Right(inFile, 8) = "_999.edi" And InStr(1, inFile, "_") > 0 And Len(inFile) > 20 Then   'Noridian
          'a8c6b39b0dbf448f_b937bdbbbab0cae4_100819429376_999.edi
                Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\" & Right(inFile, 20))
          End If
          
       Next
       
       Set objFile = Nothing
       Set objFolder = Nothing

End If
      
If rstClrHse.Fields("fldFolder").Value = "WPS" Then       'WPS, TricareEast
      Set objFolder = objFS.GetFolder(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\")
      Set objFile = objFolder.Files
      curDir = App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\"
        
      For Each objFile In objFolder.Files
         inFile = objFile.Name
         curFile = objFile
          
         If inFile > "" And (Right(inFile, 4) = ".277" Or Right(inFile, 6) = ".277CA" Or InStr(1, inFile, "277CA") > 0) Then
            Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\277\" & inFile)
         End If
         If inFile > "" And (Right(inFile, 4) = ".999" Or Right(inFile, 6) = ".999.dat" Or InStr(1, inFile, "999.") > 0) Then
                Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\" & inFile)
         End If
         If inFile > "" And Right(inFile, 4) = ".dat" And InStr(1, inFile, "5010-835") > 0 And InStr(1, inFile, ".P.") > 0 Then
               inFile = Replace(inFile, ".", "-")
            '   inFile = Replace(inFile, "-dat", ".dat")
               inFile = Left(inFile, InStr(1, inFile, "-P-")) & ".dat"
               inFile = Replace(inFile, "-.dat", ".dat")
             '  strFileName = App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name
             '   Set objFS = New Scripting.FileSystemObject
             '   curFile = objFS.GetFileName(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name)
                If objFS.FileExists(curDir & "835\" & Mid(objFile.Name, 12)) Then Call objFS.DeleteFile(curDir & "835\" & Mid(objFile.Name, 12))
                If objFS.FileExists(curDir & "835\" & inFile) Then Call objFS.DeleteFile(curDir & "835\" & inFile)
                Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name, curDir & "835\" & inFile)
         End If
      Next
       
      Set objFile = Nothing
      Set objFolder = Nothing

End If
       
If rstClrHse.Fields("fldClearingHouseID").Value = 92 Or _
   rstClrHse.Fields("fldClearingHouseID").Value = 129 Then  'BcBs of HA, BcBs of MA
   Set objFolder = objFS.GetFolder(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\")
   Set objFile = objFolder.Files
        
   For Each objFile In objFolder.Files
      inFile = objFile.Name
      curFile = objFile
      If inFile > "" And Left(inFile, 2) = "ca" And Right(inFile, 5) = "op.01" Then   'ca*op.01
                If objFS.FileExists(curDir & "SUB\" & inFile) Then Call objFS.DeleteFile(curDir & "SUB\" & inFile)
                Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\" & inFile)
      End If
      If inFile > "" And Left(inFile, 2) = "jx" And Right(inFile, 5) = "op.01" Then   'jx*op.01
                If objFS.FileExists(curDir & "RSP\" & inFile) Then Call objFS.DeleteFile(curDir & "RSP\" & inFile)
                Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\" & inFile)
      End If
      If inFile > "" And Left(inFile, 2) = "jx" And Right(inFile, 5) = "op.02" Then   'jx*op.02
                If objFS.FileExists(curDir & "RSP\" & inFile) Then Call objFS.DeleteFile(curDir & "RSP\" & inFile)
                Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\" & inFile)
      End If
      If inFile > "" And Left(inFile, 2) = "gx" And Right(inFile, 5) = "op.01" Then   'gx*op.01
                If objFS.FileExists(curDir & "RSP\" & inFile) Then Call objFS.DeleteFile(curDir & "RSP\" & inFile)
                Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\" & inFile)
      End If
      If inFile > "" And Left(inFile, 2) = "gx" And Right(inFile, 5) = "op.02" Then   'gx*op.02
                If objFS.FileExists(curDir & "RSP\" & inFile) Then Call objFS.DeleteFile(curDir & "RSP\" & inFile)
                Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\" & inFile)
      End If
      If inFile > "" And Left(inFile, 6) = "BCBSMA" And Right(inFile, 3) = "PDF" Then
         If objFS.FileExists(curDir & "RSP\" & inFile) Then Call objFS.DeleteFile(curDir & "RSP\" & inFile)
         Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\" & inFile)
      End If
   Next
       
   Set objFile = Nothing
   Set objFolder = Nothing
End If
  
    If rstClrHse.Fields("fldClearingHouseID").Value = 92 Then  'BcBs of HA
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ca*ip.01", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\la*op.01", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\271\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\sa*op.01", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\277\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\rb*op.01", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\rf*op.01", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\rq*op.01", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\rr*op.01", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\jx*op.01", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\gx*op.01", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\aa*op.01", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
    End If
    
If rstClrHse.Fields("fldClearingHouseID").Value = 2 Or _
    rstClrHse.Fields("fldClearingHouseID").Value = 3 Or _
    rstClrHse.Fields("fldClearingHouseID").Value = 4 Or _
    rstClrHse.Fields("fldClearingHouseID").Value = 5 Or _
    rstClrHse.Fields("fldClearingHouseID").Value = 6 Or _
    rstClrHse.Fields("fldClearingHouseID").Value = 7 Or _
    rstClrHse.Fields("fldClearingHouseID").Value = 8 Or _
    rstClrHse.Fields("fldClearingHouseID").Value = 9 Or _
    rstClrHse.Fields("fldClearingHouseID").Value = 10 Or _
    rstClrHse.Fields("fldClearingHouseID").Value = 12 Or _
    rstClrHse.Fields("fldClearingHouseID").Value = 14 Or _
    rstClrHse.Fields("fldClearingHouseID").Value = 27 Or _
    rstClrHse.Fields("fldClearingHouseID").Value = 33 Or _
    rstClrHse.Fields("fldClearingHouseID").Value = 44 Or _
    rstClrHse.Fields("fldClearingHouseID").Value = 45 Or _
    rstClrHse.Fields("fldClearingHouseID").Value = 49 Or _
    rstClrHse.Fields("fldClearingHouseID").Value = 59 Then  'Anthem
    If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\277*") > "" Then
        Set objFS = New Scripting.FileSystemObject
        Set objFolder = objFS.GetFolder(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\")
        Set objFile = objFolder.Files
        For Each objFile In objFolder.Files
            curDir = App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\"
             '  strFileName = App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name
            If Left(objFile.Name, 3) = "277" Then
                Set objFS = New Scripting.FileSystemObject
                curFile = objFS.GetFileName(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name)
                If objFS.FileExists(curDir & "277\" & objFile.Name) Then Call objFS.DeleteFile(curDir & "277\" & objFile.Name)
                Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\277\")
            End If
        Next
        Set objFS = Nothing
    End If
    If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.835") > "" Then
        Set objFS = New Scripting.FileSystemObject
        Set objFolder = objFS.GetFolder(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\")
        Set objFile = objFolder.Files
        For Each objFile In objFolder.Files
            curDir = App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\"
             '  strFileName = App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name
            If Right(objFile.Name, 4) = ".835" And Left(objFile.Name, 2) = "HP" Then
                Set objFS = New Scripting.FileSystemObject
                curFile = objFS.GetFileName(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name)
                If objFS.FileExists(curDir & "835\" & objFile.Name) Then Call objFS.DeleteFile(curDir & "835\" & objFile.Name)
                Call objFS.MoveFile(curDir & objFile.Name, curDir & "835\")
                If objFS.FileExists(curDir & "835\Archive\" & objFile.Name) Then
                    Call objFS.DeleteFile(curDir & "835\Archive\" & objFile.Name)
                    Call objFS.MoveFile(curDir & "835\" & objFile.Name, curDir & "835\Archive\")
                End If
            End If
        Next
        Set objFS = Nothing
    End If
    If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.270") > "" Then
        Set objFS = New Scripting.FileSystemObject
        Set objFolder = objFS.GetFolder(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\")
        Set objFile = objFolder.Files
        For Each objFile In objFolder.Files
            curDir = App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\"
             '  strFileName = App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name
            If Right(objFile.Name, 4) = ".270" And (Left(objFile.Name, 2) = "FA" Or Left(objFile.Name, 2) = "TX") Then
                Set objFS = New Scripting.FileSystemObject
                curFile = objFS.GetFileName(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name)
                If objFS.FileExists(curDir & "270\" & objFile.Name) Then Call objFS.DeleteFile(curDir & "270\" & objFile.Name)
                Call objFS.MoveFile(curDir & objFile.Name, curDir & "270\")
            End If
        Next
        Set objFS = Nothing
    End If
End If
  
    
If rstClrHse.Fields("fldClearingHouseID").Value = 24 Then 'Medicaid of NC
    If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*1972557213-5T-ISA-0001-.x12") > "" Then
        Set objFS = New Scripting.FileSystemObject
        Set objFolder = objFS.GetFolder(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\")
        Set objFile = objFolder.Files
        For Each objFile In objFolder.Files
            curDir = App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\"
             '  strFileName = App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name
            If Left(objFile.Name, 4) = "R-BX" And Right(objFile.Name, 27) = "1972557213-5T-ISA-0001-.x12" Then
                Set objFS = New Scripting.FileSystemObject
                curFile = objFS.GetFileName(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name)
                If objFS.FileExists(curDir & "835\" & Mid(objFile.Name, 12)) Then Call objFS.DeleteFile(curDir & "835\" & Mid(objFile.Name, 12))
                Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name, curDir & "835\" & Mid(objFile.Name, 12))
            End If
            
            If Left(objFile.Name, 4) = "R-BX" And Right(objFile.Name, 19) = "FF-03-ISA-0001-.x12" Then  'R-BXYEKT7U-190516071354-1913600000000330FF-03-ISA-0001-.x12
                Set objFS = New Scripting.FileSystemObject
                curFile = objFS.GetFileName(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name)
                If objFS.FileExists(curDir & "ACK\" & Mid(objFile.Name, 12)) Then Call objFS.DeleteFile(curDir & "ACK\" & Mid(objFile.Name, 12))
                Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name, curDir & "ACK\" & Mid(objFile.Name, 12))
            End If
        Next
        Set objFS = Nothing
    End If
End If
    
    
If rstClrHse.Fields("fldClearingHouseID").Value = 159 Then 'Tricare East
'20180131-659951.S001064.20180131-002016470.P.EST.5010-835pru_File19.dat
    If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*5010-835*.dat") > "" Then
        Set objFS = New Scripting.FileSystemObject
        Set objFolder = objFS.GetFolder(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\")
        Set objFile = objFolder.Files
        For Each objFile In objFolder.Files
            inFile = objFile.Name
            curDir = App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\"
            If inFile > "" And Right(inFile, 4) = ".dat" And InStr(1, inFile, "5010-835") > 0 Then
               inFile = Replace(inFile, ".", "-")
               inFile = Replace(inFile, "-dat", ".dat")
               inFile = Left(inFile, InStr(1, inFile, "-P-EST")) & ".dat"
               inFile = Replace(inFile, "-.dat", ".dat")
             '  strFileName = App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name
                Set objFS = New Scripting.FileSystemObject
                curFile = objFS.GetFileName(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name)
                If objFS.FileExists(curDir & "835\" & Mid(objFile.Name, 12)) Then Call objFS.DeleteFile(curDir & "835\" & Mid(objFile.Name, 12))
                If objFS.FileExists(curDir & "835\" & inFile) Then Call objFS.DeleteFile(curDir & "835\" & inFile)
                Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name, curDir & "835\" & inFile)
            End If
        Next
        Set objFS = Nothing
    End If
End If
        
If rstClrHse.Fields("fldClearingHouseID").Value = 52 Then 'BcBs of TN
    If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.edi") > "" Then
        Set objFS = New Scripting.FileSystemObject
        Set objFolder = objFS.GetFolder(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\")
        Set objFile = objFolder.Files
        For Each objFile In objFolder.Files
            curDir = App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\"
            inFile = objFile.Name
            If Left(objFile.Name, 9) = "ecpsy001_" And Right(objFile.Name, 7) = "835.edi" Then
                Set objFS = New Scripting.FileSystemObject
                curFile = objFS.GetFileName(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name)
                If objFS.FileExists(curDir & "835\" & Mid(objFile.Name, 10)) Then Call objFS.DeleteFile(curDir & "835\" & Mid(objFile.Name, 10))
                Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name, curDir & "835\" & Mid(objFile.Name, 10))
            End If
            'ecpsy001_193450217_42516267_121119439996_071933.X12.12112019072729_000439996.12110728426.TA1.edi
            'ecpsy001_193450217_42516267_121119439996_071933-X12-12112019072729_000439996-12110728426-TA1
            '000439996-12110728426-TA1.dat
            If Left(inFile, 9) = "ecpsy001_" And Right(inFile, 7) = "TA1.edi" Then
                inFile = Replace(inFile, ".", "-")
                inFile = Replace(inFile, "TA1-edi", "TA1")
                inFile = Mid(inFile, InStr(1, inFile, "X12-") + 20) & ".dat"
                Set objFS = New Scripting.FileSystemObject
                curFile = objFS.GetFileName(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name)
                If objFS.FileExists(curDir & "SUB\" & Mid(objFile.Name, 12)) Then Call objFS.DeleteFile(curDir & "SUB\" & Mid(objFile.Name, 12))
                If objFS.FileExists(curDir & "SUB\" & inFile) Then Call objFS.DeleteFile(curDir & "SUB\" & inFile)
                Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name, curDir & "SUB\" & inFile)
            End If
            'ecpsy001_193450217_42516267_121119439996_071933.X12.12112019072729_000439996.12110728718.999.edi
            If Left(inFile, 9) = "ecpsy001_" And Right(inFile, 7) = "999.edi" Then
                inFile = Replace(inFile, ".", "-")
                inFile = Replace(inFile, "999-edi", "999")
                inFile = Mid(inFile, InStr(1, inFile, "X12-") + 20) & ".dat"
                Set objFS = New Scripting.FileSystemObject
                curFile = objFS.GetFileName(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name)
                If objFS.FileExists(curDir & "ACK\" & Mid(objFile.Name, 12)) Then Call objFS.DeleteFile(curDir & "ACK\" & Mid(objFile.Name, 12))
                If objFS.FileExists(curDir & "ACK\" & inFile) Then Call objFS.DeleteFile(curDir & "ACK\" & inFile)
                Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name, curDir & "ACK\" & inFile)
            End If
            'ecpsy001_200641163_742983766_000455077_277CA.edi
            If Left(inFile, 9) = "ecpsy001_" And Right(inFile, 9) = "277CA.edi" Then
                inFile = Replace(inFile, ".", "-")
                inFile = Replace(inFile, "277CA-edi", "277.dat")
                'inFile = Mid(inFile, InStr(1, inFile, "X12-") + 20) & ".dat"
                Set objFS = New Scripting.FileSystemObject
                curFile = objFS.GetFileName(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name)
                If objFS.FileExists(curDir & "277\" & Mid(objFile.Name, 12)) Then Call objFS.DeleteFile(curDir & "277\" & Mid(objFile.Name, 12))
                If objFS.FileExists(curDir & "277\" & inFile) Then Call objFS.DeleteFile(curDir & "277\" & inFile)
                Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name, curDir & "277\" & inFile)
            End If
        Next
        Set objFS = Nothing
    End If
End If

If rstClrHse.Fields("fldClearingHouseID").Value = 1 Then 'Availity sarting to send files with names that are too long
    If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.era*") > "" Then
        Set objFS = New Scripting.FileSystemObject
        Set objFolder = objFS.GetFolder(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\")
        Set objFile = objFolder.Files
        For Each objFile In objFolder.Files
            curDir = App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\"
            If Len(objFile.Name) > 60 And _
                Right(objFile.Name, 4) = ".era" Then
                Set objFS = New Scripting.FileSystemObject
                curFile = objFS.GetFileName(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name)
                If objFS.FileExists(curDir & "835\" & Right(objFile.Name, 40)) Then Call objFS.DeleteFile(curDir & "835\" & Right(objFile.Name, 40))
                Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name, curDir & "835\" & Right(objFile.Name, 40))
            ElseIf InStr(1, objFile.Name, "-001.era-") > 0 Then 'ERA-MAGELLAN_BEHAVIORAL_HEALTH_SYSTEMS_LLC-202011091430-001.era-98896898022
                Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name, curDir & "835\" & Left(objFile.Name, InStr(1, objFile.Name, "-001.era-")) & "002.era")
            ElseIf Right(objFile.Name, 4) = ".era" Then
                Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name, curDir & "835\")
            End If
        Next
        Set objFS = Nothing
    End If
End If

If IfNull(rstClrHse.Fields("fldRemittanceYN").Value, "") = "Y" Then
    Set objFolder = objFS.GetFolder(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\")
    Set objFile = objFolder.Files
    For Each objFile In objFolder.Files
        inFile = objFile.Name
        curFile = objFile
        If objFile.Name > "" And _
           (Right(objFile.Name, 4) = ".835" Or _
            Right(objFile.Name, 4) = ".ERN" Or _
            Right(objFile.Name, 4) = ".ARA" Or _
            Right(objFile.Name, 4) = ".RAP" Or _
            Right(objFile.Name, 7) = "835.edi" Or _
            Left(objFile.Name, 4) = "ERA-" Or _
            Left(objFile.Name, 8) = "5010835." Or _
            Left(objFile.Name, 9) = "5010835B." Or _
            Left(objFile.Name, 9) = "835_5010_" Or _
            Left(objFile.Name, 8) = "835Ansi_") _
            Then
             If objFS.FileExists(curDir & "835\" & objFile.Name) And objFile.Size > 0 Then Call objFS.DeleteFile(curDir & "835\" & objFile.Name)
             Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
        End If
        If objFile.Name > "" And InStr(1, objFile.Name, "EPS") > 1 Then
            If objFS.FileExists(curDir & "EPS\" & objFile.Name) And objFile.Size > 0 Then Call objFS.DeleteFile(curDir & "EPS\" & objFile.Name)
            Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\EPS\")
        End If
    Next
    Set objFile = Nothing
    Set objFolder = Nothing
End If

If rstClrHse.Fields("fldClearingHouseID").Value = 11 Then 'Capario
    If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.ACK") > "" Then
        Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.ACK", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
    End If
    If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.TRN") > "" Then
        Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.TRN", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\TRN\")
    End If
    If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.PRA") > "" Then
        Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.PRA", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\PRA\")
    End If
End If

If rstClrHse.Fields("fldClearingHouseID").Value = 57 Then 'BcOfKS
   If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\277\") > "" Then
      If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\277CA*") > "" Then
         Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\277CA*", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\277\")
      End If
   End If
End If
    
If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.trnack") > "" Then
   Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.trnack", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
End If
If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\TRN.*") > "" Then
   Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\TRN.*", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
End If
If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\") > "" Then
   If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.999") > "" Then
      Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.999", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\")
   End If
   If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.997") > "" Then
      Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.997", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\")
   End If
End If
If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\277\") > "" Then
   If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.277") > "" Then
      Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.277", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\277\")
   End If
End If
If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\864\") > "" Then
   If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.864") > "" Then
      Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.864", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\864\")
   End If
End If
If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ZIP\") > "" Then
   If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.ZIP") > "" Then
      Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.ZIP", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ZIP\")
   End If
End If
If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\EPS\") > "" Then
   If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.EPS*") > "" Then
      Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.EPS*", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\EPS\")
   End If
End If

Set objFolder = objFS.GetFolder(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\")
Set objFile = objFolder.Files
For Each objFile In objFolder.Files
   inFile = objFile.Name
   If inFile > "" And _
      Right(inFile, 3) <> "asc" And _
      Right(inFile, 3) <> "bat" And _
      Right(inFile, 3) <> ".ht" And _
      Right(inFile, 3) <> ".cp" And _
      Right(inFile, 3) <> "was" And _
      Right(inFile, 3) <> "wax" And _
      Right(inFile, 13) <> "ElecClaim.txt" And _
      Left(inFile, 8) <> "password" Then
      curFile = objFile
      Set objText = objFile.OpenAsTextStream(ForReading)
      Do While objText.AtEndOfStream = False
         strLine = objText.ReadLine
         If Mid(strLine, 1, 3) = "ISA" Then
            If Len(strLine) <= 107 Then
               strLine = objText.ReadLine
               If Mid(strLine, 1, 3) = "GS*" Then
                  strLine = objText.ReadLine
                  If Mid(strLine, 1, 3) = "ST*" Then
                     objText.Close
                     If InStr(3, strLine, "*TA1*") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\")
                     If InStr(3, strLine, "*997*") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                     If InStr(3, strLine, "*999*") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                     If InStr(3, strLine, "*271*") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\271\")
                     If InStr(3, strLine, "*277*") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\277\")
                     If InStr(3, strLine, "*824*") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\824\")
                     If InStr(3, strLine, "*835*") Then
                        If rstClrHse.Fields("fldClearingHouseID").Value = 39 Or rstClrHse.Fields("fldClearingHouseID").Value = 87 Then        'Medicare of PA, DC
                        '16T4_1922430.000
                           If Len(inFile) = 16 And Mid(inFile, 13, 1) = "." Then
                              Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\" & Left(inFile, Len(inFile) - 4) & "-" & Format(Now(), "YYYYMMDD") & Right(inFile, 4))
                           End If
                        ElseIf rstClrHse.Fields("fldClearingHouseID").Value = 33 Then        'BcBs Of GA
                           If InStr(1, inFile, ".") = (Len(inFile) - 4) Then
                              Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\" & Left(inFile, Len(inFile) - 4) & "-" & Format(Now(), "YYYYMMDD") & Right(inFile, 4))
                           End If
                        Else
                           Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
                        End If
                     End If
                  End If
               ElseIf Mid(strLine, 1, 4) = "TA1*" Then
                     objText.Close
                     Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\")
               ElseIf InStr(3, strLine, "GS*") Then
                  If Not InStr(3, strLine, "TA1*") And Not InStr(3, strLine, "ST*") And Not InStr(3, strLine, "ST^") And Not InStr(3, strLine, "ST\") Then strLine = objText.ReadLine
                  objText.Close
                  If InStr(3, strLine, "TA1*") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\")
                  If InStr(1, strLine, "ST*997*") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                  If InStr(1, strLine, "ST^997^") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                  If InStr(1, strLine, "ST\997\") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                  If InStr(1, strLine, "ST*999*") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                  If InStr(1, strLine, "ST^999^") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                  If InStr(1, strLine, "ST\999\") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                  If InStr(1, strLine, "ST{999{") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                  If InStr(1, strLine, "ST*271*") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\271\")
                  If InStr(1, strLine, "ST^271^") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\271\")
                  If InStr(1, strLine, "ST\271\") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\271\")
                  If InStr(1, strLine, "ST{271{") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\271\")
                  If InStr(1, strLine, "ST*277*") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\277\")
                  If InStr(1, strLine, "ST^277^") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\277\")
                  If InStr(1, strLine, "ST\277\") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\277\")
                  If InStr(1, strLine, "ST{277{") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\277\")
                  If InStr(1, strLine, "ST*835*") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
                  If InStr(1, strLine, "ST^835^") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
                  If InStr(1, strLine, "ST\835\") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
                  If InStr(1, strLine, "ST{835{") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
               End If
            Else
               objText.Close
               If InStr(3, strLine, "TA1*") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\")
               If InStr(1, strLine, "ST*997*") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
               If InStr(1, strLine, "ST^997^") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
               If InStr(1, strLine, "ST\997\") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
               If InStr(1, strLine, "ST*999*") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
               If InStr(1, strLine, "ST^999^") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
               If InStr(1, strLine, "ST\999\") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
               If InStr(1, strLine, "ST{999{") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
               If InStr(1, strLine, "ST*271*") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\271\")
               If InStr(1, strLine, "ST^271^") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\271\")
               If InStr(1, strLine, "ST\271\") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\271\")
               If InStr(1, strLine, "ST{271{") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\271\")
               If InStr(1, strLine, "ST*277*") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\277\")
               If InStr(1, strLine, "ST^277^") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\277\")
               If InStr(1, strLine, "ST\277\") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\277\")
               If InStr(1, strLine, "ST{277{") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\277\")
               If InStr(1, strLine, "ST^835^") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
               If InStr(1, strLine, "ST{835{") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
               If InStr(1, strLine, "ST*835*") Then
                  If rstClrHse.Fields("fldClearingHouseID").Value = 39 Or rstClrHse.Fields("fldClearingHouseID").Value = 87 Then        'Medicare of PA, DC
                     '16T4_1922430.000
                     If Len(inFile) = 16 And Mid(inFile, 13, 1) = "." Then
                        Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\" & Left(inFile, Len(inFile) - 4) & "-" & Format(Now(), "YYYYMMDD") & Right(inFile, 4))
                     End If
                  ElseIf rstClrHse.Fields("fldClearingHouseID").Value = 33 Then        'BcBs Of GA
                     If InStr(1, inFile, ".") = (Len(inFile) - 4) Then
                        Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\" & Left(inFile, Len(inFile) - 4) & "-" & Format(Now(), "YYYYMMDD") & Right(inFile, 4))
                     End If
                  Else
                     Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
                  End If
               End If
            End If
         End If
         Exit Do
      Loop
   End If
   If inFile > "" And _
      Right(inFile, 3) = "zip" Or Right(inFile, 3) = "ZIP" And _
      Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ZIP\") > "" Then
      If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ZIP\" & inFile) > "" Then Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ZIP\" & inFile
      If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.ZIP") > "" Then
         Call objFS.MoveFile(curFile, App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ZIP\")
      End If
   End If
Next
     
If rstClrHse.Fields("fldClearingHouseID").Value = 22 Or _
   rstClrHse.Fields("fldClearingHouseID").Value = 84 Or _
   rstClrHse.Fields("fldClearingHouseID").Value = 124 Then   'Tricare West, TriWest, BcBs of SC
   Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.X12", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
End If
If rstClrHse.Fields("fldClearingHouseID").Value = 90 Or _
   rstClrHse.Fields("fldClearingHouseID").Value = 91 Then  'Post and Track
   Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.HTML", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\")
End If

If rstClrHse.Fields("fldClearingHouseID").Value = 53 Then   'BcBs of ARIZONA
   Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.pgp", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\PGP\")
End If
       
If rstClrHse.Fields("fldClearingHouseID").Value = 50 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 92 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 104 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 105 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 106 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 116 Then       'BcBs of HA, Medicaid of VA UnZip Files
    If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.zip") > "" Then
        Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.zip", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ZIP\")
    End If
    If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.ZIP") > "" Then
        Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.ZIP", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ZIP\")
    End If
    If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.*Z") > "" Then
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ZIP\" & inFile) > "" Then Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ZIP\" & inFile
        Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.*Z", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ZIP\")
    End If
    If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.*z") > "" Then
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ZIP\" & inFile) > "" Then Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ZIP\" & inFile
        Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.*z", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ZIP\")
    End If
End If
    
Set objFS = Nothing
Set objFolder = Nothing

Exit Sub
    
ErrTrap:
    Call ShowError(Err)
    Set objFS = Nothing
End Sub

Private Sub UpdateEClaims(ByVal rst As ADODB.Recordset)
'Updates queued status for electronic claims
    
    Dim lngClaimID As Long
    Dim lngELID As Long
    Dim lngPlanID As Long
    Dim objClaim As ClaimBz.CClaimBz
    Dim lngCtr As Long
    Dim itm As ListItem
    Dim dtPrintDate As Date
        
    Screen.MousePointer = vbHourglass
        
    'Update the database with the claim submit date
    dtPrintDate = CDate(Format(Now, "mm/dd/yyyy hh:nn"))
    
    Set objClaim = CreateObject("ClaimBz.CClaimBz")
    Do While Not rst.EOF
    'For lngCtr = 1 To rst.RecordCount
      '  Set itm = frmSubmission.lstEClaim.ListItems(lngCtr)
        
        lngClaimID = rst.Fields("fldClaimID").Value
        lngELID = rst.Fields("fldEncounterLogID").Value
        lngPlanID = rst.Fields("fldPlanID").Value
        
        Call objClaim.SubmitClaim(dtPrintDate, lngClaimID, lngELID, lngPlanID, TX_ECLAIM_SUBMITTED, -1, GetLoginName())
    'Next lngCtr
        rst.MoveNext
    Loop
   
    Set objClaim = Nothing

    Screen.MousePointer = vbDefault

End Sub

Public Function getPendingVerif() As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 02/01/2018
'Author: Duane C Orth
'Description:  Returns a list of patients for whom initial coverage has not been verified
'Parameters:
'Returns: ADORst
'--------------------------------------------------------------------


    Dim cnnSQL As ADODB.Connection
    Dim rstSQL As ADODB.Recordset
    Dim cmdSQL As ADODB.Command
    Dim strConnect As String
    Dim strSQL As String
    
    On Error GoTo ErrTrap:
    
    Set rstSQL = New ADODB.Recordset
    Set cmdSQL = New ADODB.Command
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
    Set cmdSQL.ActiveConnection = cnnSQL
    
    'Create the parameter objects
    With cmdSQL
    
        strSQL = "SELECT C.fldPatRPPlanID, C.fldPatientID, C.fldRPID, H.fldProviderID, A.fldDateAdded, A.fldAddedBy, A.fldBenefactorID, "
        strSQL = strSQL & "A.fldLast AS fldPatientLastName, A.fldFirst AS fldPatientFirstName, A.fldMI AS fldPatientMI, A.fldAddress1 AS fldPatientStreetNum, A.fldCity AS fldPatientCity, "
        strSQL = strSQL & "A.fldState AS fldPatientState, A.fldZip AS fldPatientZip, A.fldSex AS fldPatientSex, A.fldDOB AS fldPatientDOB, A.fldSSN AS fldPatientSSN, "
        strSQL = strSQL & "D.fldLast AS fldInsuredLastName, D.fldFirst AS fldInsuredFirstName, D.fldMI AS fldInsuredMI, D.fldAddress1 AS fldInsuredStreetNum, D.fldCity AS fldInsuredCity, "
        strSQL = strSQL & "D.fldState AS fldInsuredState, D.fldZip AS fldInsuredZip, D.fldSex AS fldInsdSex, D.fldDOB AS fldInsdDOB, D.fldSSN AS fldInsuredSSN, "
        strSQL = strSQL & "E.fldVerPlanID, E.fldPlanName, CASE WHEN G.fldPayerCodeVerify > '' THEN G.fldPayerCodeVerify ELSE G.fldPayerCode END AS PayerCode, E.fldCardNum, E.fldGroupNum, "
        strSQL = strSQL & "B.fldLastName AS fldProviderLastName, B.fldFirstName AS fldProviderFirstName, B.fldMI AS fldProviderMI, H.fldProviderNPI, H.fldTaxonomyCode, "
        strSQL = strSQL & "H.fldAddress1 AS fldProviderAddress, H.fldCity AS fldProviderCity, H.fldState AS fldProviderState, H.fldZip AS fldProviderZip, "
        strSQL = strSQL & "H.fldSupervisorID, K.fldLastName AS fldSuperLastName, K.fldFirstName AS fldSuperFirstName, K.fldMI AS fldSuperMI, J.fldProviderNPI AS fldSuperNPI, J.fldTaxonomyCode AS fldSuperTaxonomyCode, "
        strSQL = strSQL & "M.fldBusinessName AS fldFacilityName, M.fldAddress1 AS fldFacilityAddress, M.fldCity AS fldFacilityCity, M.fldState AS fldFacilityState, M.fldZip AS fldFacilityZip, "
        strSQL = strSQL & "M.fldPOSCode, M.fldFacilityNPI, M.fldFacilityTaxonomy, "
        strSQL = strSQL & "H.fldGroupID, I.fldGroupNPI, I.fldTaxonomyCode AS fldGroupTaxonomyCode, I.fldGroupName, I.fldTaxID AS GroupTIN, "
        strSQL = strSQL & "G.fldInsuranceID, G.fldCPCID, F.fldPlanID, "
        strSQL = strSQL & "CASE WHEN G.fldVerifyClearingHouseID > 0 THEN G.fldVerifyClearingHouseID ELSE G.fldClearingHouseID END AS fldClearingHouseID, "
        strSQL = strSQL & "G.fldVerifyClearingHouseID, G.fldCPCName, G.fldPayerCode, G.fldPayerCodeVerify, G.fldInsType, "
        strSQL = strSQL & "E.fldVerifyElectronicYN, E.fldReVerifyElectronicYN, E.fldVerifyRejectYN, E.fldVerifyFileID, E.fldVerifyDate "
        strSQL = strSQL & "FROM dbo.tblBenefactor AS A INNER JOIN "
        strSQL = strSQL & "dbo.tblClinic AS M ON M.fldClinicID = A.fldClinicID INNER JOIN "
        strSQL = strSQL & "dbo.tblUser AS B ON A.fldOwnerID = B.fldUserID INNER JOIN "
        strSQL = strSQL & "dbo.tblPatRPPlan AS C ON A.fldBenefactorID = C.fldPatientID INNER JOIN "
        strSQL = strSQL & "dbo.tblBenefactor AS D ON C.fldRPID = D.fldBenefactorID INNER JOIN "
        strSQL = strSQL & "dbo.tblPatRPPlanRule AS E ON C.fldPatRPPlanID = E.fldPatRPPlanID INNER JOIN "
        strSQL = strSQL & "dbo.tblPlan AS F ON E.fldVerPlanID = F.fldPlanID INNER JOIN "
        strSQL = strSQL & "dbo.tblClaimsProcCtr AS G ON G.fldCPCID = F.fldCPCID INNER JOIN "
        strSQL = strSQL & "dbo.tblProvider AS H  ON H.fldProviderID = B.fldUserID INNER JOIN "
        strSQL = strSQL & "dbo.tblProviderGroup AS I ON H.fldGroupID = I.fldGroupID  LEFT OUTER JOIN "
        strSQL = strSQL & "dbo.tblProvider AS J ON H.fldSupervisorID = J.fldProviderID  LEFT OUTER JOIN "
        strSQL = strSQL & "dbo.tblUser AS K ON K.fldUserID = J.fldProviderID "
        strSQL = strSQL & "Where "
        strSQL = strSQL & "(B.fldDisabledYN = 'N') AND "
        strSQL = strSQL & "(C.fldDisabledYN = 'N') AND "
        strSQL = strSQL & "(C.fldPlanID = 3087) AND "
        strSQL = strSQL & "(E.fldVerPlanID > 0) AND "
        strSQL = strSQL & "(G.fldVerifyElectronicYN = 'Y') AND "
        strSQL = strSQL & "(E.fldVerifyFileID IS NULL OR E.fldVerifyFileID = '') AND "
        strSQL = strSQL & "(E.fldVerifyElectronicYN = 'Y' or fldReVerifyElectronicYN = 'Y') "
        strSQL = strSQL & "UNION "
        strSQL = strSQL & "SELECT C.fldPatRPPlanID, C.fldPatientID, C.fldRPID, H.fldProviderID, A.fldDateAdded, A.fldAddedBy, A.fldBenefactorID, "
        strSQL = strSQL & "A.fldLast AS fldPatientLastName, A.fldFirst AS fldPatientFirstName, A.fldMI AS fldPatientMI, A.fldAddress1 AS fldPatientStreetNum, A.fldCity AS fldPatientCity, "
        strSQL = strSQL & "A.fldState AS fldPatientState, A.fldZip AS fldPatientZip, A.fldSex AS fldPatientSex, A.fldDOB AS fldPatientDOB, A.fldSSN AS fldPatientSSN, "
        strSQL = strSQL & "D.fldLast AS fldInsuredLastName, D.fldFirst AS fldInsuredFirstName, D.fldMI AS fldInsuredMI, D.fldAddress1 AS fldInsuredStreetNum, D.fldCity AS fldInsuredCity, "
        strSQL = strSQL & "D.fldState AS fldInsuredState, D.fldZip AS fldInsuredZip, D.fldSex AS fldInsdSex, D.fldDOB AS fldInsdDOB, D.fldSSN AS fldInsuredSSN, "
        strSQL = strSQL & "E.fldVerPlanID, E.fldPlanName, CASE WHEN G.fldPayerCodeVerify > '' THEN G.fldPayerCodeVerify ELSE G.fldPayerCode END AS PayerCode, E.fldCardNum, E.fldGroupNum, "
        strSQL = strSQL & "B.fldLastName AS fldProviderLastName, B.fldFirstName AS fldProviderFirstName, B.fldMI AS fldProviderMI, H.fldProviderNPI, H.fldTaxonomyCode, "
        strSQL = strSQL & "H.fldAddress1 AS fldProviderAddress, H.fldCity AS fldProviderCity, H.fldState AS fldProviderState, H.fldZip AS fldProviderZip, "
        strSQL = strSQL & "H.fldSupervisorID, K.fldLastName AS fldSuperLastName, K.fldFirstName AS fldSuperFirstName, K.fldMI AS fldSuperMI, J.fldProviderNPI AS fldSuperNPI, J.fldTaxonomyCode AS fldSuperTaxonomyCode, "
        strSQL = strSQL & "M.fldBusinessName AS fldFacilityName, M.fldAddress1 AS fldFacilityAddress, M.fldCity AS fldFacilityCity, M.fldState AS fldFacilityState, M.fldZip AS fldFacilityZip, "
        strSQL = strSQL & "M.fldPOSCode, M.fldFacilityNPI, M.fldFacilityTaxonomy, "
        strSQL = strSQL & "H.fldGroupID, I.fldGroupNPI, I.fldTaxonomyCode AS fldGroupTaxonomyCode, I.fldGroupName, I.fldTaxID AS GroupTIN, "
        strSQL = strSQL & "G.fldInsuranceID, G.fldCPCID, F.fldPlanID, "
        strSQL = strSQL & "CASE WHEN G.fldVerifyClearingHouseID > 0 THEN G.fldVerifyClearingHouseID ELSE G.fldClearingHouseID END AS fldClearingHouseID, "
        strSQL = strSQL & "G.fldVerifyClearingHouseID, G.fldCPCName, G.fldPayerCode, G.fldPayerCodeVerify, G.fldInsType, "
        strSQL = strSQL & "E.fldVerifyElectronicYN, E.fldReVerifyElectronicYN, E.fldVerifyRejectYN, E.fldVerifyFileID, E.fldVerifyDate "
        strSQL = strSQL & "FROM dbo.tblBenefactor AS A INNER JOIN "
        strSQL = strSQL & "dbo.tblClinic AS M ON M.fldClinicID = A.fldClinicID INNER JOIN "
        strSQL = strSQL & "dbo.tblUser AS B ON A.fldOwnerID = B.fldUserID INNER JOIN "
        strSQL = strSQL & "dbo.tblPatRPPlan AS C ON A.fldBenefactorID = C.fldPatientID INNER JOIN "
        strSQL = strSQL & "dbo.tblBenefactor AS D ON C.fldRPID = D.fldBenefactorID INNER JOIN "
        strSQL = strSQL & "dbo.tblPatRPPlanRule AS E ON C.fldPatRPPlanID = E.fldPatRPPlanID INNER JOIN "
        strSQL = strSQL & "dbo.tblPlan AS F ON E.fldVerPlanID = F.fldPlanID INNER JOIN "
        strSQL = strSQL & "dbo.tblClaimsProcCtr AS G ON G.fldCPCID = F.fldCPCID INNER JOIN "
        strSQL = strSQL & "dbo.tblProvider AS H  ON H.fldProviderID = B.fldUserID INNER JOIN "
        strSQL = strSQL & "dbo.tblProviderGroup AS I ON H.fldGroupID = I.fldGroupID  LEFT OUTER JOIN "
        strSQL = strSQL & "dbo.tblProvider AS J ON H.fldSupervisorID = J.fldProviderID  LEFT OUTER JOIN "
        strSQL = strSQL & "dbo.tblUser AS K ON K.fldUserID = J.fldProviderID "
        strSQL = strSQL & "Where "
        strSQL = strSQL & "(B.fldDisabledYN = 'N') AND "
        strSQL = strSQL & "(C.fldDisabledYN = 'N') AND "
        strSQL = strSQL & "(C.fldPlanID <> 3087) AND "
        strSQL = strSQL & "(C.fldPlanID > 0) AND "
        strSQL = strSQL & "(G.fldVerifyElectronicYN = 'Y') AND "
        strSQL = strSQL & "(E.fldVerifyFileID IS NULL OR E.fldVerifyFileID = '') AND "
        strSQL = strSQL & "(E.fldVerifyElectronicYN = 'Y' or fldReVerifyElectronicYN = 'Y') "
        strSQL = strSQL & "ORDER BY fldClearingHouseID, fldPayerCode, fldGroupNPI, fldProviderNPI, fldSuperNPI, fldInsuranceID, fldCPCName, fldProviderLastName, fldProviderFirstName, fldRPID, fldPatientID "

 '       strSQL = "SELECT * FROM vw_OpenVerifications "
 '       strSQL = strSQL & " WHERE PlanVerifyElectronicYN = 'Y' AND (fldVerifyFileID IS NULL OR fldVerifyFileID = '') AND (fldVerifyElectronicYN = 'N' or fldReVerifyElectronicYN = 'Y') "
 '       strSQL = strSQL & " ORDER BY fldClearingHouseID, fldPayerCode, fldGroupNPI, fldProviderNPI, fldSuperNPI, fldInsuranceID, fldCPCName, fldProviderLastName, fldProviderFirstName, fldRPID, fldPatientID "
    End With
    
    cmdSQL.CommandText = strSQL
    cmdSQL.CommandType = adCmdText
    
    'Execute the stored procedure
    rstSQL.CursorLocation = adUseClient
    rstSQL.Open cmdSQL, , adOpenForwardOnly, adLockReadOnly
    
    Set getPendingVerif = rstSQL
    
    'Disconnect the recordset
    Set cmdSQL.ActiveConnection = Nothing
    Set cmdSQL = Nothing
    Set rstSQL.ActiveConnection = Nothing
    Set cnnSQL = Nothing
         
    Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
     Set rstSQL = Nothing
     Set cnnSQL = Nothing
     Set cmdSQL = Nothing
     Err.Raise Err.Number, , Err.Description
     Call ShowError(Err)
 End Function


