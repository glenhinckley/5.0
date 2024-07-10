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
    
    If Trim(strMachineName) <> "PSYQUEL-EDI" Then
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
    Dim objMail As CDO.Message
    Dim strRecipient As String
    Dim strMachineName As String
    
    On Error GoTo ErrHand
    
    strRecipient = "QWareAdmin@Psyquel.com"
    strMachineName = GetMachineName()
   
    Set objMail = CreateObject("CDO.Message")
    objMail.From = strMachineName & "_Claims_Generator"
    objMail.To = strRecipient
    objMail.Subject = strSubject
    objMail.TextBody = "The following message was generated from the Claim Generator running on " & strMachineName & ":" & vbCrLf & strMessage
    objMail.Send
    Set objMail = Nothing
    
    Exit Sub
    
ErrHand:
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub

Private Sub GenerateFile()
'-------------------------------------------------------------------------------
'Author: Dave Richkun
'Date: 11/07/2002
'Description: Main function that generates EDI text file compliant to X.12 837 v4010
'Parameters:
'Returns:
'-------------------------------------------------------------------------------

'On Error GoTo ErrTrap:

Dim objEDI As EDIBz.CEDIBz
Dim objClrHse As InsuranceBz.CClearingHouseBz
Dim rst As ADODB.Recordset
Dim rstClrHse As ADODB.Recordset
Dim strWhere, strFile As String
Dim lngClrHseID As Long
Dim lngPrevClrHseID As Long
Dim lngStatusID As Long
Dim strPrevPayerCode As String
Dim strPrevSubmitterID As String
Dim strPrevClaimType As String
Dim strProgram, strLine As String
Dim dblProgID As Double
Dim intExit, intCnt, intWrk As Integer
    
Dim objFS As Scripting.FileSystemObject

Dim objFolder, objFile
Dim objText As Scripting.TextStream

Set objFS = CreateObject("Scripting.FileSystemObject")

Screen.MousePointer = vbHourglass
    
lngClrHseID = 1: intCnt = 1: lngPrevClrHseID = 0

Set objClrHse = CreateObject("InsuranceBZ.CClearingHouseBz")
Set rstClrHse = objClrHse.Fetch(False, " fldNightlyProcessYN = 'Y'", "fldClearingHouseID")
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
         If lngClrHseID <> lngPrevClrHseID Or _
            strPrevClaimType <> IfNull(rst.Fields("fldClaimType").Value, "") Or _
            strPrevPayerCode <> IfNull(rst.Fields("fldPayerCode").Value, "") Or _
            strPrevSubmitterID <> IfNull(rst.Fields("SubmitterID").Value, "") Then
                lngStatusID = OpenEDIFile(rstClrHse)
                lngPrevClrHseID = lngClrHseID
                strPrevClaimType = IfNull(rst.Fields("fldClaimType").Value, "")
                strPrevPayerCode = IfNull(rst.Fields("fldPayerCode").Value, "")
                strPrevSubmitterID = IfNull(rst.Fields("SubmitterID").Value, "")
         End If
    
         g_lngEndTxNum = GenerateElectronicClaims(rstClrHse, rst, g_lngEndTxNum, g_FileNumber, g_FileName, True)
    
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
            Case 1, 200, 35, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 201, 22, 27, 33, 44, 45, 49, 59, 24, 26, 28, 29, 30, 43, 47, 48, 50, 57, 121, 66, 23, 68, 69, 63, 72, 74, 84, 90, 91, 99, 101, 100, 122, 125, 132, 134, 138, 139, 140, 142, 149, 146, 150, 156, 157
              Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
              Select Case rstClrHse.Fields("fldClearingHouseID").Value
               Case 1, 200       'ThinEDI uses FTP
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & "  -site Thin_EDI -p /SendFiles/"
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/SendFiles/*.*" & Chr(34) & " -site Thin_EDI -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & Chr(34) & " -delsrc"
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/ReceiveFiles/*.*" & Chr(34) & " -site Thin_EDI -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & Chr(34) & " -delsrc"
               Case 35       'Availity uses FTP
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & "  -site Availity -p /SendFiles/"
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/SendFiles/*.*" & Chr(34) & " -site Availity -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & Chr(34) & " -delsrc"
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/ReceiveFiles/*.*" & Chr(34) & " -site Availity -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & Chr(34) & " -delsrc"
               Case 2, 3, 4, 5       'AnthemEast uses SFTP
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & "  -site Anthem_AE -p Inbound/"
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "Outbound/*.*" & Chr(34) & " -site Anthem_AE -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & Chr(34) & " -delsrc -o"
               Case 6, 7, 8       'AnthemMW uses SFTP
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & "  -site Anthem_MW -p Inbound/"
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "outbound/*.*" & Chr(34) & " -site Anthem_MW -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & Chr(34) & " -delsrc -o"
               Case 9           'AnthemCo uses SFTP
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & "  -site Anthem_AW -p Inbound/"
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "outbound/*.*" & Chr(34) & " -site Anthem_AW -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & Chr(34) & " -delsrc -o"
               Case 10       'AnthemNV uses SFTP
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & "  -site Anthem_NV -p Inbound/"
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "outbound/*.*" & Chr(34) & " -site Anthem_NV -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & Chr(34) & " -delsrc -o"
               Case 11, 201       'Capario
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & "  -site Capario -p /claims/"
               Case 22              'BcBs of SC
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & " -site BcBs_Of_SC  -p /SFTPUSER/Inbound/ "
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/SFTPUSER/Outbound/*.*" & Chr(34) & " -site BcBs_Of_SC -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Chr(34) & " -delsrc -o "
               Case 27       'BC of CA uses SFTP
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & "  -site Anthem_CA -p Inbound/"
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "outbound/*.*" & Chr(34) & " -site Anthem_CA -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & Chr(34) & " -delsrc -o"
               Case 33       'BC of GA uses SFTP
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & "  -site Anthem_GA -p Inbound/"
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "outbound/*.*" & Chr(34) & " -site Anthem_GA -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & Chr(34) & " -delsrc -o"
               Case 44       'BC of SE uses SFTP
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & "  -site Anthem_SE -p Inbound/"
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "outbound/*.*" & Chr(34) & " -site Anthem_SE -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & Chr(34) & " -delsrc -o"
               Case 45       'BC of MO uses SFTP
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & "  -site Anthem_MO -p Inbound/"
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "outbound/*.*" & Chr(34) & " -site Anthem_MO -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & Chr(34) & " -delsrc -o"
               Case 49       'BC of WI uses SFTP
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & "  -site Anthem_WI -p Inbound/"
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "outbound/*.*" & Chr(34) & " -site Anthem_WI -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & Chr(34) & " -delsrc -o"
               Case 59       'AnthemNY uses SFTP
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & "  -site Anthem_NY -p Inbound/"
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "outbound/*.*" & Chr(34) & " -site Anthem_NY -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & Chr(34) & " -delsrc -o"
               Case 24              'Medicaid Of NC
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & " -site NCTracks  -p /prd/In/ "
               Case 26, 28              'Highmark
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & " -site Highmark -p /hipaa-in/ "
               Case 29, 30, 138         'HighmarkIBC
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & " -site HighmarkIBC -p /hipaa-in/ "
               Case 149         'HighmarkAdv
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & " -site HighmarkAdv -p /hipaa-in/ "
               Case 157              'HighmarkSWV
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & " -site HighmarkSWV -p /hipaa-in/ "
               Case 43                  'Medicaid of IN
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & "-site Medicaid_IN -p " & Chr(34) & "/Distribution/HIPAA Transactions/" & Chr(34)
               Case 47       'ASK-Edi uses FTP
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & " -site AskEDI -p /Home/EDI/6001799/ " '" & Chr(34) & "/virtual_directory/" & Chr(34)
               Case 48                  'Independent Health
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & " -site Independent -p / "
               Case 50                  'Medicaid of VA
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & " -site MedicaidVA -p " & Chr(34) & "/Distribution/EDI/1733/Prod/To-VAMMIS/" & Chr(34)
               Case 57, 121      'BcBs of Kansas ASK-Edi uses FTP
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & " -site BcBs_Of_KS -p /Home/EDI/0006602/ " '" & Chr(34) & "/virtual_directory/" & Chr(34)
               Case 66         'BcBs of RI uses SFTP
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & "  -site BcBs_Of_RI -p / "
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "Inbox/*.*" & Chr(34) & " -site BcBs_Of_RI -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & Chr(34) & " -delsrc -o"
               Case 23         'Value Options has a different file name - 00723801.X12 --05/01/2010 now uses FTP
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & "  -site ValueOptions -p /Uploads/837p/"
               Case 68         'BcBs Of MI now uses FTP
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & "  -site BcBs_Of_MI -p / "
               Case 69              'BcBs of NE
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & " -site BcBs_Of_NE -p /inbound/ "
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/outbound/*.*" & Chr(34) & " -site BcBs_Of_NE -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Chr(34) & " -delsrc -o "
               Case 63         'Value Options  --05/01/2010 now uses FTP
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & "  -site ValueOptionsMD -p /Uploads/837p/"
               Case 72         'Value Options  --05/01/2010 now uses FTP
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & "  -site ValueOptionsMA -p /Uploads/837p/"
               Case 74         'Value Options  --05/01/2010 now uses FTP
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & "  -site ValueOptionsNM -p /Uploads/837p/"
               Case 84              'TricareSouth
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & " -site TricareSouth -p /SFTPUSER/Inbound/ "
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/SFTPUSER/Outbound/*.*" & Chr(34) & " -site TricareSouth -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Chr(34) & " -delsrc -o "
               Case 90, 91, 100, 122, 139      'Post-N-Track
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & "  -site Post_N_Track -p /Upload/"
               Case 99              'BcBs of AR
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & " -site BcBs_Of_AR -p /Inbound/ "
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/Outbound/*.*" & Chr(34) & " -site BcBs_Of_AR -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Chr(34) & " -delsrc -o "
               Case 101              'BcBs of DE
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & " -site BcBs_Of_DE -p /hipaa-in/ "
               Case 125              'BcBs of WV
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & " -site BcBs_Of_WV -p /hipaa-in/ "
               Case 132              'UHA
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & " -site UHA -p In/ "
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "out/*.*" & Chr(34) & " -site UHA -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Chr(34) & " -delsrc -o "
               Case 134         'GateWay EDI
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & " -site GateWayEdi -p /claims/"
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/reports/*.*" & Chr(34) & " -site GateWayEdi -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Chr(34) & " -delsrc -o "
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/remits/*.*" & Chr(34) & " -site GateWayEdi -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Chr(34) & " -delsrc -o "
               Case 140              'BcBs of WA
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & " -site Premera -p /Upload/ "
               Case 142                  'Smoky Mountain
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & " -site SmokyMtn -p /In/ "
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/Out/*.*" & Chr(34) & " -site SmokyMtn -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Chr(34) & " -delsrc -o "
               Case 146              'Medicaid of SC
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & " -site Medicaid_SC -p /SFTP/inbound/ "
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/SFTP/Outbound/*.*" & Chr(34) & " -site Medicaid_SC -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Chr(34) & " -delsrc -o "
               Case 150              'ClaimMD
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & "  -site ClaimMD -p /SendFiles/"
 '               Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/SendFiles/*.*" & Chr(34) & " -site ClaimMD -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & Chr(34) & " -delsrc"
 '               Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/ReceiveFiles/*.*" & Chr(34) & " -site ClaimMD -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & Chr(34) & " -delsrc"
               Case 156              'Capitol Bc
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & " -site CapitolBC -p /Inbound/ "
              End Select
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
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ECPSY.X12") > "" Then
                    Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ECPSY.X12"
                End If
                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(g_FileName, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ECPSY.X12")
            Case 53        'BcBs of AZ has a different file name xxxx_0000????.ttt - 00008480.837
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\00008480.837") > "" Then
                    Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\00008480.837"
                End If
                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(g_FileName, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\00008480.837")
            Case 55, 77, 78, 79, 80, 81, 82, 83            'Medicare of Iowa (WPS) has a different file name xxxx_0000????.ttt - 00008480.837
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\14541.dat") > "" Then
                    Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\14541.dat"
                End If
                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(g_FileName, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\14541.dat")
            Case 60        'HealthNow has a different file name IMS.CLM
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\Z78232.A" & Format(DatePart("D", Now()), "00")) > "" Then
                    Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\Z78232.A" & Format(DatePart("D", Now()), "00")
                End If
                Open (App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\filename.txt") For Output As #2
                Print #2, "Z78232.A" & Format(DatePart("D", Now()), "00")
                Close #2 'Close the file
                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(g_FileName, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Z78232.A" & Format(DatePart("D", Now()), "00"))
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
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & strFile & Chr(34) & " -site BcBs_Of_HA -p / -o"
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
            Case 38        'Medicare Of Georga
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\pgap000" & Format(Now(), "w") - 1 & ".clm") > "" Then Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\pgap000" & Format(Now(), "w") - 1 & ".clm"
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\pgap000" & Format(Now(), "w") - 1 & ".zip") > "" Then Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\pgap000" & Format(Now(), "w") - 1 & ".zip"
                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(g_FileName, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\pgap000" & Format(Now(), "w") - 1 & ".clm")
                strProgram = Environ("WINZIP") & "\pkzip.exe " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\pgap000" & Format(Now(), "w") - 1 & ".zip " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\pgap000" & Format(Now(), "w") - 1 & ".clm"
                dblProgID = Shell(strProgram, vbNormalFocus)
                intExit = WaitOnProgram(dblProgID, True)
                Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\pgap000" & Format(Now(), "w") - 1 & "0.zip" & Chr(34) & " -site MC_Of_GA -p " & Chr(34) & "/gaf76888/" & Chr(34)
                Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                Close #2 'Close the file
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
            Case 17        'Medicare Of Tennesse
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ptnp000" & Format(Now(), "w") - 1 & ".clm") > "" Then Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ptnp000" & Format(Now(), "w") - 1 & ".clm"
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ptnp000" & Format(Now(), "w") - 1 & ".zip") > "" Then Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ptnp000" & Format(Now(), "w") - 1 & ".zip"
                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(g_FileName, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ptnp000" & Format(Now(), "w") - 1 & ".clm")
                strProgram = Environ("WINZIP") & "\pkzip.exe " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ptnp000" & Format(Now(), "w") - 1 & ".zip " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ptnp000" & Format(Now(), "w") - 1 & ".clm"
                dblProgID = Shell(strProgram, vbNormalFocus)
                intExit = WaitOnProgram(dblProgID, True)
                Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ptnp000" & Format(Now(), "w") - 1 & "0.zip" & Chr(34) & " -site MC_Of_TN -p " & Chr(34) & "/tn201312/" & Chr(34)
                Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                Close #2 'Close the file
            Case 107              'BcBs of MT
                strFile = "37020" & Format(Now, "mmdd") & ".dap"
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & strFile) > "" Then strFile = "37021" & Format(Now, "mmdd") & ".dap"
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & strFile) > "" Then strFile = "37022" & Format(Now, "mmdd") & ".dap"
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
            Case 127              'BcBs of LA
                Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & " -site BcBs_Of_LA -p /P0013469/ "
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/P0013469/BCT*.OUT" & Chr(34) & " -site BcBs_Of_LA -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Chr(34) & " -delsrc -o "
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/P0013469/BC9*.OUT" & Chr(34) & " -site BcBs_Of_LA -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Chr(34) & " -delsrc -o "
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/P0013469/BCA*.OUT" & Chr(34) & " -site BcBs_Of_LA -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Chr(34) & " -delsrc -o "
                Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                Close #2 'Close the file
            Case 129              'BcBs of MA
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & Left(IfNull(g_FileName, ""), InStr(1, IfNull(g_FileName, ""), ".")) & "837") > "" Then Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & Left(IfNull(g_FileName, ""), InStr(1, IfNull(g_FileName, ""), ".")) & "837"
                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName, App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Left(IfNull(g_FileName, ""), InStr(1, IfNull(g_FileName, ""), ".")) & "837")
                Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Left(IfNull(g_FileName, ""), InStr(1, IfNull(g_FileName, ""), ".")) & "837" & Chr(34) & " -site BcBs_Of_MA -p " & Chr(34) & " /e__company_tradingpartners_EFE_C3BS/Inbound/" & Chr(34)
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/e__company_tradingpartners_EFE_C3BS/Outbound/*.*" & Chr(34) & " -site BcBs_Of_MA -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Chr(34) & " -delsrc -o "
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
                Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(g_FileName, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt"
                If rstClrHse.Fields("fldClearingHouseID").Value = 129 Then
                    Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Left(IfNull(g_FileName, ""), InStr(1, IfNull(g_FileName, ""), ".")) & "837", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                End If
            End If
            
        End If
  
        If rstClrHse.Fields("fldClearingHouseID").Value = 1 Or _
           rstClrHse.Fields("fldClearingHouseID").Value = 11 Or _
           rstClrHse.Fields("fldClearingHouseID").Value = 26 Or _
           rstClrHse.Fields("fldClearingHouseID").Value = 28 Or _
           rstClrHse.Fields("fldClearingHouseID").Value = 29 Or _
           rstClrHse.Fields("fldClearingHouseID").Value = 30 Or _
           rstClrHse.Fields("fldClearingHouseID").Value = 36 Or _
           rstClrHse.Fields("fldClearingHouseID").Value = 37 Or _
           rstClrHse.Fields("fldClearingHouseID").Value = 52 Or _
           rstClrHse.Fields("fldClearingHouseID").Value = 90 Or _
           rstClrHse.Fields("fldClearingHouseID").Value = 91 Or _
           rstClrHse.Fields("fldClearingHouseID").Value = 92 Or _
           rstClrHse.Fields("fldClearingHouseID").Value = 93 Or _
           rstClrHse.Fields("fldClearingHouseID").Value = 94 Or _
           rstClrHse.Fields("fldClearingHouseID").Value = 100 Or _
           rstClrHse.Fields("fldClearingHouseID").Value = 111 Or _
           rstClrHse.Fields("fldClearingHouseID").Value = 121 Or _
           rstClrHse.Fields("fldClearingHouseID").Value = 129 Or _
           rstClrHse.Fields("fldClearingHouseID").Value = 138 Or _
           rstClrHse.Fields("fldClearingHouseID").Value = 149 Or _
           rstClrHse.Fields("fldClearingHouseID").Value = 150 Or _
           rstClrHse.Fields("fldClearingHouseID").Value = 153 Then        'Availity, Capario has multiple files
            Set objFolder = objFS.GetFolder(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\")
            Set objFile = objFolder.Files
            
            For Each objFile In objFolder.Files
                'inFile = objFile.Name
                'curFile = objFile
                If Len(objFile.Name) = 16 And IsNumeric(Left(objFile.Name, 12)) And Right(objFile.Name, 4) = ".txt" Then
                    If rstClrHse.Fields("fldClearingHouseID").Value = 52 Then        'BcBs of TN has multiple files
                        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ECPSY.X12") > "" Then
                            Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ECPSY.X12"
                        End If
                        Call objFS.CopyFile(objFile, App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ECPSY.X12")
                        Open (App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\filename.txt") For Output As #2
                        Print #2, objFile.Name
                        Close #2 'Close the file
                        If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") > "" Then
                            strProgram = App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat"
                            dblProgID = Shell(strProgram, vbNormalFocus)
                            intExit = WaitOnProgram(dblProgID, True)
                        End If
                        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt") > "" Then
                            Call objFS.MoveFile(objFile, App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                            Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt"
                        End If
                    ElseIf rstClrHse.Fields("fldClearingHouseID").Value = 11 Or _
                       rstClrHse.Fields("fldClearingHouseID").Value = 201 Then        'Capario has a different file name IMS.CLM
                        Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                        Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & objFile.Name & Chr(34) & "  -site Capario -p /claims/"
                        Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                        Close #2 'Close the file
                        Open (App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\filename.txt") For Output As #2
                        Print #2, objFile.Name
                        Close #2 'Close the file
                        If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") > "" Then
                            strProgram = App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat"
                            dblProgID = Shell(strProgram, vbNormalFocus)
                            intExit = WaitOnProgram(dblProgID, True)
                        End If
                        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt") > "" Then
                            Call objFS.MoveFile(objFile, App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                            Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt"
                        End If
                        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\PS4.INS") > "" Then
                            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\PS4.INS", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Format(Now, "YYYYmmdd") & ".INS")
                        End If
                        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\PS4.REC") > "" Then
                            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\PS4.REC", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Format(Now, "YYYYmmdd") & ".REC")
                        End If
                    ElseIf rstClrHse.Fields("fldClearingHouseID").Value = 25 Then     'BcBs Of NC has a different file name IMS.CLM
                        Open (App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\filename.txt") For Output As #2
                        Print #2, objFile.Name
                        Close #2 'Close the file
                        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\P_I_CEXALL") > "" Then
                            Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\P_I_CEXALL"
                        End If
                        Call objFS.CopyFile(objFile, App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\P_I_CEXALL")
                        If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") > "" Then
                            strProgram = App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat"
                            dblProgID = Shell(strProgram, vbNormalFocus)
                            intExit = WaitOnProgram(dblProgID, True)
                        End If
                        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt") > "" Then
                            Call objFS.MoveFile(objFile, App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                            Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt"
                        End If
                    ElseIf rstClrHse.Fields("fldClearingHouseID").Value = 26 Or _
                        rstClrHse.Fields("fldClearingHouseID").Value = 28 Or _
                        rstClrHse.Fields("fldClearingHouseID").Value = 29 Or _
                        rstClrHse.Fields("fldClearingHouseID").Value = 30 Or _
                        rstClrHse.Fields("fldClearingHouseID").Value = 57 Or _
                        rstClrHse.Fields("fldClearingHouseID").Value = 121 Or _
                        rstClrHse.Fields("fldClearingHouseID").Value = 129 Or _
                        rstClrHse.Fields("fldClearingHouseID").Value = 138 Or _
                        rstClrHse.Fields("fldClearingHouseID").Value = 149 Then          'BcBs Of MA, BcBs of Kansas
                        Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                        Select Case rstClrHse.Fields("fldClearingHouseID").Value
                            Case 57, 121      'BcBs Of Kansas
                                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & objFile.Name & Chr(34) & " -site BcBs_Of_KS -p /home/edi/0006602/ " '" & Chr(34) & "/virtual_directory/" & Chr(34)
                            Case 129          'BcBs Of MA
                                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & Left(IfNull(objFile.Name, ""), InStr(1, IfNull(objFile.Name, ""), ".")) & "837") > "" Then Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & Left(IfNull(objFile.Name, ""), InStr(1, IfNull(objFile.Name, ""), ".")) & "837"
                                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & objFile.Name, App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Left(IfNull(objFile.Name, ""), InStr(1, IfNull(objFile.Name, ""), ".")) & "837")
                                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Left(IfNull(objFile.Name, ""), InStr(1, IfNull(objFile.Name, ""), ".")) & "837" & Chr(34) & " -site BcBs_Of_MA -p " & Chr(34) & " /e__company_tradingpartners_EFE_C3BS/Inbound/" & Chr(34)
                                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/e__company_tradingpartners_EFE_C3BS/Outbound/*.*" & Chr(34) & " -site BcBs_Of_MA -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Chr(34) & " -delsrc -o "
                            Case 57, 121      'BcBs Of Kansas
                                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & objFile.Name & Chr(34) & " -site BcBs_Of_KS -p /home/edi/0006602/ " '" & Chr(34) & "/virtual_directory/" & Chr(34)
                            Case 26, 28              'Highmark
                                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & objFile.Name & Chr(34) & " -site Highmark -p /hipaa-in/ "
                            Case 29, 30, 138         'HighmarkIBC
                                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & objFile.Name & Chr(34) & " -site HighmarkIBC -p /hipaa-in/ "
                            Case 149              'HighmarkAdv
                                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & objFile.Name & Chr(34) & " -site HighmarkAdv -p /hipaa-in/ "
                            Case 157              'HighmarkSWV
                                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & objFile.Name & Chr(34) & " -site HighmarkSWV -p /hipaa-in/ "
                        End Select
                        Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                        Close #2 'Close the file
                        Open (App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\filename.txt") For Output As #2
                        Print #2, objFile.Name
                        Close #2 'Close the file
                        If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") > "" Then
                            strProgram = App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat"
                            dblProgID = Shell(strProgram, vbNormalFocus)
                            intExit = WaitOnProgram(dblProgID, True)
                        End If
                        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt") > "" Then
                            Call objFS.MoveFile(objFile, App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                            Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt"
                        End If
                    ElseIf rstClrHse.Fields("fldClearingHouseID").Value = 90 Or _
                       rstClrHse.Fields("fldClearingHouseID").Value = 91 Or _
                       rstClrHse.Fields("fldClearingHouseID").Value = 100 Then        'Post-N-Track
                        Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                        Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & objFile.Name & Chr(34) & "  -site Post_N_Track -p /Upload/"
                        Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                        Close #2 'Close the file
                        Open (App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\filename.txt") For Output As #2
                        Print #2, objFile.Name
                        Close #2 'Close the file
                        If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") > "" Then
                            strProgram = App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat"
                            dblProgID = Shell(strProgram, vbNormalFocus)
                            intExit = WaitOnProgram(dblProgID, True)
                        End If
                        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt") > "" Then
                            Call objFS.MoveFile(objFile, App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                            Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt"
                        End If
                    ElseIf rstClrHse.Fields("fldClearingHouseID").Value = 92 Then        'BcBs Of HA
                        strFile = "ca" & Left(objFile.Name, 10) & "ip.01"
                        For intWrk = 0 To 9
                            If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & strFile) > "" Or Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\" & strFile) > "" Then strFile = "ca" & Left(objFile.Name, 9) & intWrk & "ip.01"
                            If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & strFile) = "" And Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\" & strFile) = "" Then Exit For
                        Next
                        Call objFS.CopyFile(objFile, App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & strFile)
                        Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                        Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & strFile & Chr(34) & " -site BcBs_Of_HA -p / -o"
                        Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                        Close #2 'Close the file
                        Open (App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\filename.txt") For Output As #2
                        Print #2, objFile.Name
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
                    ElseIf rstClrHse.Fields("fldClearingHouseID").Value = 1 Or _
                       rstClrHse.Fields("fldClearingHouseID").Value = 200 And intCnt < 2 Then       'Avality
                        Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                        Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & objFile.Name & Chr(34) & " -site Thin_EDI -p /SendFiles/"
                        Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/SendFiles/*.*" & Chr(34) & " -site Thin_EDI -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & Chr(34) & " -delsrc"
                        Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/ReceiveFiles/*.*" & Chr(34) & " -site Thin_EDI -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & Chr(34) & " -delsrc"
                        Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                        Close #2 'Close the file
                        Open (App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\filename.txt") For Output As #2
                        Print #2, objFile.Name
                        Close #2 'Close the file
                        If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") > "" Then
                            strProgram = App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat"
                            dblProgID = Shell(strProgram, vbNormalFocus)
                            intExit = WaitOnProgram(dblProgID, True)
                        End If
                        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt") > "" Then
                            Call objFS.MoveFile(objFile, App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                            Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt"
                        End If
                        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*success") > "" Then
                            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*success", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                        End If
                        intCnt = intCnt + 1
                    ElseIf rstClrHse.Fields("fldClearingHouseID").Value = 150 Then       'ClaimMD
                        Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                        Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & objFile.Name & Chr(34) & " -site ClaimMD -p /SendFiles/"
      '                  Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/SendFiles/*.*" & Chr(34) & " -site ClaimMD -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & Chr(34) & " -delsrc"
      '                  Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/ReceiveFiles/*.*" & Chr(34) & " -site ClaimMD -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & Chr(34) & " -delsrc"
                        Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                        Close #2 'Close the file
                        Open (App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\filename.txt") For Output As #2
                        Print #2, objFile.Name
                        Close #2 'Close the file
                        If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") > "" Then
                            strProgram = App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat"
                            dblProgID = Shell(strProgram, vbNormalFocus)
                            intExit = WaitOnProgram(dblProgID, True)
                        End If
                        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt") > "" Then
                            Call objFS.MoveFile(objFile, App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                            Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt"
                        End If
                        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*success") > "" Then
                            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*success", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                        End If
                    ElseIf rstClrHse.Fields("fldClearingHouseID").Value = 153 Then       'Trillium
                        Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                        Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & objFile.Name & Chr(34) & " -site TrilliumCH_In -p  / "
                        Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/*.*" & Chr(34) & " -site TrilliumCH_Out -p " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Chr(34) & " -delsrc -o "
                        Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                        Close #2 'Close the file
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
                        strProgram = App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat"
                        dblProgID = Shell(strProgram, vbNormalFocus)
                        intExit = WaitOnProgram(dblProgID, True)
                        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt") > "" Then
                            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(g_FileName, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                            Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt"
                        End If
                        objText.Close
                    End If
                End If
            Next
            Set objFile = Nothing
            Set objFolder = Nothing
        End If

   '     If rstClrHse.Fields("fldClearingHouseID").Value = 66 Then        'BcBs of RI has a different file name 837P_CORP
   '         If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\837P_CORP") > "" Then
   '             Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\837P_CORP"
   '         End If
   '         Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(g_FileName, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\837P_CORP")
   '     End If
          
        If IfNull(rstClrHse.Fields("fldRemittanceYN").Value, "") = "Y" And _
            (Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ZRECV32.wax") > "" Or _
             Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ERAs.bat") > "") Then
          '  If (Format(Now, "w") = 2 Or Format(Now, "w") = 4 Or Format(Now, "w") = 6) And Hour(Now) < 12 Then '2-Monday, 4-Wednesday, 6-friday and in the AM
                If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ERAs.bat") > "" Then        'ThinEDI uses FTP
                    strProgram = App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ERAs.bat"
                Else
                    strProgram = Environ("PROCOMM") & "\pw5.exe " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ZRECV32.wax"
                End If
                dblProgID = Shell(strProgram, vbNormalFocus)
                intExit = WaitOnProgram(dblProgID, True)
          '  End If
        End If
             
        If rstClrHse.Fields("fldClearingHouseID").Value = 50 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 56 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 86 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 92 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 104 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 105 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 111 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 116 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 132 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 151 Then     'BcBs of HA, Medicaid of VA UnZip Files
                If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Unpack.bat") > "" Then
                    strProgram = App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Unpack.bat"
                    dblProgID = Shell(strProgram, vbNormalFocus)
                    intExit = WaitOnProgram(dblProgID, True)
                End If
        End If
        
        If rstClrHse.Fields("fldClearingHouseID").Value <> 48 Then
            Call MoveFiles(rstClrHse, CLng(rstClrHse.Fields("fldClearingHouseID").Value))
        End If
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
    If blnEmailFailure Then
        Call SendEmailMessage("Electronic Claims Failure", "The Electronic Claims Daemon reported a failure in " & Err.Source & "." & vbCrLf & "Unhandled Error: " & Err.Number & ": " & Err.Description & vbCrLf & Me.txtStatus.Text)
    End If
    Call ShowError(Err)
    Set objEDI = Nothing
    Set rstClrHse = Nothing
    Screen.MousePointer = vbDefault
    Unload Me
End Sub
Private Function OpenEDIFile(ByVal rstClrHse As ADODB.Recordset) As Long
'Opens/creates a new EDI file.  File name is based on the create date (MMDDYY)
'and a unique file number retrieved from the database.

    Dim objEDI As EDIBz.CEDIBz
    Dim objCLH As InsuranceBz.CClearingHouseBz
    Dim rst As ADODB.Recordset
    Dim strFileName As String
    Dim lngStartTxNum As Long
   
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
    
    'Check last Clearing House File Name
    If (IfNull(rstClrHse.Fields("fldConcatFileYN").Value, "") = "Y") And _
        (IfNull(rstClrHse.Fields("fldEFileName").Value, "") > "") And _
        (Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(rstClrHse.Fields("fldEFileName").Value, "")) > "") Then
        g_FileName = IfNull(rstClrHse.Fields("fldEFileName").Value, "")
        Open (App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & g_FileName) For Append As #1
    Else
        Set rst = Nothing
        strFileName = Format(CStr(Month(Now)), "00") & Format(CStr(Day(Now)), "00") & Mid(CStr(Year(Now)), 3)
        g_FileName = strFileName & CStr(g_FileNumber) & ".txt" 'Append file number to filename to guarantee uniqueness

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
    
End Function

Private Sub MoveFiles(ByVal rstClrHse As ADODB.Recordset, intClrHse)
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

Set objFS = CreateObject("Scripting.FileSystemObject")

On Error Resume Next
'On Error GoTo ErrTrap:
        
    If rstClrHse.Fields("fldClearingHouseID").Value = 1 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 35 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 62 Then        'Thin, BCBS of FL, BCBS of OR
    '   Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.era", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.999", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.997", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.PCC", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\PCC\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.ECC", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ECC\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.SFC", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\SFC\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.WPS", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\WPS\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.EPS", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\EPS\")
        'Delete HyperTerminal trace files
        Call objFS.DeleteFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.dbg")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*success", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
    End If
       
    If rstClrHse.Fields("fldClearingHouseID").Value = 36 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 37 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 54 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 75 Then 'Medicare NCA SCA OH SC
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\999*.RSP", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\997*.RSP", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
    End If
      
    If rstClrHse.Fields("fldClearingHouseID").Value = 92 Then  'BcBs of HA
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\sa*op.01", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\277\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\rq*op.01", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\rr*op.01", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\jx*op.01", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\aa*op.01", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
    End If
     
    If rstClrHse.Fields("fldClearingHouseID").Value = 134 Then  'GateWay EDI
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.DAT", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\")
    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 150 Then  'ClaimMD
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.txt.result", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\")
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
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\999PBLUEP") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\999PBLUEP", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\999PBLUEP-" & Format(Now, "YYYYmmdd") & Hour(Now) & ".ACK")
        End If
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\HCFAREPORT") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\HCFAREPORT", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\HCFAREPORT-" & Format(Now, "YYYYmmdd") & Hour(Now) & ".RSP")
        End If
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\999PCLMSP") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\999PCLMSP", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\999PCLMSP-" & Format(Now, "YYYYmmdd") & Hour(Now) & ".ACK")
        End If
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\RPTPCLMSP") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\RPTPCLMSP", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\RSP\RPTPCLMSP-" & Format(Now, "YYYYmmdd") & Hour(Now) & ".RSP")
        End If
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\5010835SHIELD") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\5010835SHIELD", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\835\5010835SHIELD-" & Format(Now, "YYYYmmdd") & Hour(Now) & ".835")
        End If
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\277CAPCLMSP") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\277CAPCLMSP", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\277\277CAPCLMSP-" & Format(Now, "YYYYmmdd") & Hour(Now) & ".277")
        End If
    End If

    If rstClrHse.Fields("fldClearingHouseID").Value = 34 Then        'Medicare of Florida
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK.TXT") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK.TXT", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\" & Format(Now, "YYYYmmdd") & Hour(Now) & ".277")
        End If
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\MSG.TXT") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\MSG.TXT", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\MSG\" & Format(Now, "YYYYmmdd") & Hour(Now) & ".MSG")
        End If
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\REJ.TXT") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\REJ.TXT", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Format(Now, "YYYYmmdd") & Hour(Now) & ".REJ")
        End If
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ERN.TXT") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ERN.TXT", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Format(Now, "YYYYmmdd") & Hour(Now) & ".ERN")
        End If
    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 68 Then        'BcBs of MI
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835P5010*.A", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\R277CAA*.A", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\R277CAF*.A", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\R277CAK*.A", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\999P*.*", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
    End If
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 36 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 41 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 55 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 56 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 77 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 78 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 79 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 80 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 81 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 82 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 83 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 86 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 110 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 111 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 131 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 133 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 136 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 151 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 152 Then       'Medicare of IL, WPS, Noridian
       Set objFolder = objFS.GetFolder(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\")
       Set objFile = objFolder.Files

       For Each objFile In objFolder.Files
          inFile = objFile.Name
          curFile = objFile
          If inFile > "" And Right(inFile, 8) = "_835.edi" And InStr(1, inFile, "-") > 0 Then   'Noridian
                Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\" & Right(inFile, Len(inFile) - InStr(1, inFile, "-") - 1))
          End If
          If inFile > "" And Right(inFile, 7) = "835.dat" Then
                Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\" & Left(inFile, 28) & ".835")
          End If
          If inFile > "" And Right(inFile, 18) = "005010X221A1-P.edi" Then   'MaineCare
                Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\" & Right(inFile, 43))
          End If
       Next
       
       Set objFile = Nothing
       Set objFolder = Nothing

    End If
          
If rstClrHse.Fields("fldClearingHouseID").Value = 24 Then 'Medicaid of NC
    If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*1972557213-5T-ISA-0001-.x12") > "" Then
        Set objFS = New Scripting.FileSystemObject
        Set objFolder = objFS.GetFolder(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\")
        Set objFile = objFolder.Files
        For Each objFile In objFolder.Files
            curDir = App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\"
             '  strFileName = App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name
            If Left(objFile.Name, 11) = "R-BXTG1JPQ-" And Right(objFile.Name, 27) = "1972557213-5T-ISA-0001-.x12" Then
                Set objFS = New Scripting.FileSystemObject
                curFile = objFS.GetFileName(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name)
                If objFS.FileExists(curDir & "835\" & Mid(objFile.Name, 12)) Then Call objFS.DeleteFile(curDir & "835\" & Mid(objFile.Name, 12))
                Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name, curDir & "835\" & Mid(objFile.Name, 12))
            End If
        Next
        Set objFS = Nothing
    End If
End If

If rstClrHse.Fields("fldClearingHouseID").Value = 52 Then 'BcBs of TN
    If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*835.edi") > "" Then
        Set objFS = New Scripting.FileSystemObject
        Set objFolder = objFS.GetFolder(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\")
        Set objFile = objFolder.Files
        For Each objFile In objFolder.Files
            curDir = App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\"
             '  strFileName = App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name
            If Left(objFile.Name, 9) = "ecpsy001_" And Right(objFile.Name, 7) = "835.edi" Then
                Set objFS = New Scripting.FileSystemObject
                curFile = objFS.GetFileName(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name)
                If objFS.FileExists(curDir & "835\" & Mid(objFile.Name, 10)) Then Call objFS.DeleteFile(curDir & "835\" & Mid(objFile.Name, 10))
                Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name, curDir & "835\" & Mid(objFile.Name, 10))
            End If
        Next
        Set objFS = Nothing
    End If
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
End If
    
If rstClrHse.Fields("fldClearingHouseID").Value = 1 Then 'Availity sarting to send files with names that are too long
    If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.era") > "" Then
        Set objFS = New Scripting.FileSystemObject
        Set objFolder = objFS.GetFolder(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\")
        Set objFile = objFolder.Files
        For Each objFile In objFolder.Files
            curDir = App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\"
            If Left(objFile.Name, 37) = "ERA-CALIFORNIA_PHYSICIANS_SERVICE_DBA" And Right(objFile.Name, 4) = ".era" Then
                Set objFS = New Scripting.FileSystemObject
                curFile = objFS.GetFileName(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name)
                If objFS.FileExists(curDir & "835\" & Right(objFile.Name, 35)) Then Call objFS.DeleteFile(curDir & "835\" & Right(objFile.Name, 35))
                Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name, curDir & "835\" & Right(objFile.Name, 35))
            ElseIf Right(objFile.Name, 4) = ".era" Then
                Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name, curDir & "835\")
            End If
        Next
        Set objFS = Nothing
    End If
End If

If IfNull(rstClrHse.Fields("fldRemittanceYN").Value, "") = "Y" Then
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.835") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.835", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\835\")
        End If
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.ERN") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.ERN", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\835\")
        End If
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.ERA") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.ERA", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\835\")
        End If
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.EPS") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.EPS", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\EPS\")
        End If
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.RAP") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.RAP", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\835\")
        End If
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
   If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.zip") > "" Then
      Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.zip", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ZIP\")
   End If
   If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.*z") > "" Then
      Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.*z", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ZIP\")
   End If
End If
    
If rstClrHse.Fields("fldClearingHouseID").Value = 90 Or _
   rstClrHse.Fields("fldClearingHouseID").Value = 91 Then  'Post and Track
   Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.HTML", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\")
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
      Right(inFile, 3) <> "RSP" And _
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
                     If InStr(3, strLine, "*997*") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                     If InStr(3, strLine, "*835*") Then
                        If rstClrHse.Fields("fldClearingHouseID").Value = 39 Then        'Medicare of PA
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
                  If InStr(1, strLine, "ST*277*") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\277\")
                  If InStr(1, strLine, "ST^277^") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\277\")
                  If InStr(1, strLine, "ST\277\") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\277\")
                  If InStr(1, strLine, "ST*997*") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                  If InStr(1, strLine, "ST^997^") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                  If InStr(1, strLine, "ST\997\") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                  If InStr(1, strLine, "ST*999*") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                  If InStr(1, strLine, "ST^999^") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                  If InStr(1, strLine, "ST\999\") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                  If InStr(1, strLine, "ST^835^") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
                  If InStr(1, strLine, "ST*835*") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
               End If
            Else
               objText.Close
               If InStr(3, strLine, "TA1*") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\")
               If InStr(1, strLine, "ST*277*") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\277\")
               If InStr(1, strLine, "ST^277^") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\277\")
               If InStr(1, strLine, "ST\277\") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\277\")
               If InStr(1, strLine, "ST*997*") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
               If InStr(1, strLine, "ST^997^") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
               If InStr(1, strLine, "ST\997\") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
               If InStr(1, strLine, "ST*999*") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
               If InStr(1, strLine, "ST^999^") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
               If InStr(1, strLine, "ST\999\") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
               If InStr(1, strLine, "ST^835^") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
               If InStr(1, strLine, "ST*835*") Then
                  If rstClrHse.Fields("fldClearingHouseID").Value = 39 Then        'Medicare of PA
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
Next

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

