VERSION 5.00
Begin VB.Form frmMain 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Psyquel - Electronic Claims"
   ClientHeight    =   2940
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   4620
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   2940
   ScaleWidth      =   4620
   ShowInTaskbar   =   0   'False
   StartUpPosition =   1  'CenterOwner
   Begin VB.Frame Frame1 
      Caption         =   "File Format"
      Height          =   1380
      Left            =   105
      TabIndex        =   3
      Top             =   600
      Width           =   4335
      Begin VB.TextBox txtPayorCode 
         Height          =   285
         Left            =   2640
         TabIndex        =   6
         Text            =   "87726"
         Top             =   1005
         Width           =   735
      End
      Begin VB.OptionButton optMultiple 
         Caption         =   "File contains claims for this payor ID only"
         Height          =   255
         Index           =   1
         Left            =   240
         TabIndex        =   5
         Top             =   720
         Value           =   -1  'True
         Width           =   3735
      End
      Begin VB.OptionButton optMultiple 
         Caption         =   "File contains claims for multiple payors"
         Height          =   255
         Index           =   0
         Left            =   240
         TabIndex        =   4
         Top             =   360
         Width           =   3735
      End
      Begin VB.Label Label2 
         Alignment       =   1  'Right Justify
         Caption         =   "Electronic Payor ID"
         Height          =   255
         Left            =   1080
         TabIndex        =   7
         Top             =   1035
         Width           =   1455
      End
   End
   Begin VB.CommandButton cmdDone 
      Caption         =   "&Cancel"
      Height          =   420
      Index           =   1
      Left            =   2760
      TabIndex        =   1
      Top             =   2280
      Width           =   1140
   End
   Begin VB.CommandButton cmdDone 
      Caption         =   "&OK"
      Height          =   420
      Index           =   0
      Left            =   600
      TabIndex        =   0
      Top             =   2280
      Width           =   1140
   End
   Begin VB.Label Label1 
      Alignment       =   2  'Center
      Caption         =   "ANSI X.12 837 v 4010 Electronic Claim Submission"
      Height          =   255
      Left            =   240
      TabIndex        =   2
      Top             =   120
      Width           =   4215
   End
End
Attribute VB_Name = "frmMain"
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
    InitSettings
    GenerateFile
    Unload Me
End Sub


Private Sub cmdDone_Click(Index As Integer)
    Select Case Index
        Case 0
        Case 1
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
Dim objFS As Scripting.FileSystemObject
Dim strWhere As String
Dim lngClrHseID As Long
Dim lngPrevClrHseID As Long
Dim lngStatusID As Long
Dim strPrevPayerCode As String
Dim strPrevSubmitterID As String
    Dim strProgram As String
    Dim dblProgID As Double
    Dim intExit As Integer
    
Set objFS = CreateObject("Scripting.FileSystemObject")
    
lngClrHseID = 1
    'fldNightlyProcessYN
Set objClrHse = CreateObject("InsuranceBZ.CClearingHouseBz")
Set rstClrHse = objClrHse.Fetch(False, " fldNightlyProcessYN = 'Y'", "fldClearingHouseID")
Set objClrHse = Nothing
    
Do While Not rstClrHse.EOF
    If IsNull(rstClrHse.Fields("fldClearingHouseID").Value) Then
        lngClrHseID = 1
    Else
        lngClrHseID = rstClrHse.Fields("fldClearingHouseID").Value
    End If
    strWhere = " Where fldClearingHouseID = " & lngClrHseID
        
    Set objEDI = CreateObject("EDIBz.CEDIBz")
    Set rst = objEDI.FetchElectClaims(strWhere) 'Fetch claims
    Set objEDI = Nothing
    
        'barStatus.Max = rst.RecordCount
        
    Do While Not rst.EOF
        
        If lngClrHseID <> lngPrevClrHseID Or _
               strPrevPayerCode <> IfNull(rst.Fields("fldPayerCode").Value, "") Or _
               strPrevSubmitterID <> IfNull(rst.Fields("SubmitterID").Value, "") Then
                lngStatusID = OpenEDIFile(rstClrHse)
                lngPrevClrHseID = lngClrHseID
                strPrevPayerCode = IfNull(rst.Fields("fldPayerCode").Value, "")
                strPrevSubmitterID = IfNull(rst.Fields("SubmitterID").Value, "")
        End If
    
        g_lngEndTxNum = GenerateElectronicClaims(rstClrHse, rst, g_lngEndTxNum, g_FileNumber, g_FileName)
    
        CloseEDIFile
        
            'If Me.Tag = "Cancel" Then Exit Do
    Loop
    
    'lblStatus.Caption = "Complete"

    'Free resources
    Set rst = Nothing
    'If Me.Tag <> "Cancel" Then
    '    MsgBox ("Claims Generated!")
    'End If
    If IfNull(rstClrHse.Fields("fldEFileName").Value, "") > "" And _
        (Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(rstClrHse.Fields("fldEFileName").Value, "")) > "") And _
        (Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ZSEND32.wax") > "" Or _
         Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") > "") Then
             
        'Overwrite file if it exists
        If rstClrHse.Fields("fldClearingHouseID").Value = 1 Then        'ThinEDI uses FTP
                Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & rstClrHse.Fields("fldEFileName").Value & Chr(34) & "  -site Thin_EDI -p /SendFiles/"
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/SendFiles/*.*" & Chr(34) & " -site Thin_EDI -p " & Chr(34) & "D:\Data\Apps\QBill\THIN" & Chr(34) & " -delsrc"
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/ReceiveFiles/*.*" & Chr(34) & " -site Thin_EDI -p " & Chr(34) & "D:\Data\Apps\QBill\THIN" & Chr(34) & " -delsrc"
                Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                Close #2 'Close the file
        ElseIf rstClrHse.Fields("fldClearingHouseID").Value = 11 Then        'ProxyMed has a different file name IMS.CLM
                Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & rstClrHse.Fields("fldEFileName").Value & Chr(34) & "  -site Medavent -p /claims/"
                Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                Close #2 'Close the file
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\IMS.CLM") > "" Then
                    Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\IMS.CLM"
                End If
                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(rstClrHse.Fields("fldEFileName").Value, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\IMS.CLM")
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\PS4.INS") > "" Then
                    Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\PS4.INS", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Format(Now, "YYYYmmdd") & ".INS")
                End If
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\PS4.REC") > "" Then
                    Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\PS4.REC", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Format(Now, "YYYYmmdd") & ".REC")
                End If
        ElseIf rstClrHse.Fields("fldClearingHouseID").Value = 25 Then        'BcBs Of NC has a different file name IMS.CLM
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\P_I_CEXALL") > "" Then
                    Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\P_I_CEXALL"
                End If
                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(rstClrHse.Fields("fldEFileName").Value, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\P_I_CEXALL")
        ElseIf rstClrHse.Fields("fldClearingHouseID").Value = 52 Then        'BcBs Of TN has a different file name IMS.CLM
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ECPSY.X12") > "" Then
                    Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ECPSY.X12"
                End If
                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(rstClrHse.Fields("fldEFileName").Value, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ECPSY.X12")
        ElseIf rstClrHse.Fields("fldClearingHouseID").Value = 53 Then        'BcBs of AZ has a different file name xxxx_0000????.ttt - 00008480.837
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\00008480.837") > "" Then
                    Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\00008480.837"
                End If
                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(rstClrHse.Fields("fldEFileName").Value, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\00008480.837")
        ElseIf rstClrHse.Fields("fldClearingHouseID").Value = 55 Or _
                   rstClrHse.Fields("fldClearingHouseID").Value = 77 Or _
                   rstClrHse.Fields("fldClearingHouseID").Value = 78 Or _
                   rstClrHse.Fields("fldClearingHouseID").Value = 79 Or _
                   rstClrHse.Fields("fldClearingHouseID").Value = 80 Or _
                   rstClrHse.Fields("fldClearingHouseID").Value = 81 Or _
                   rstClrHse.Fields("fldClearingHouseID").Value = 82 Or _
                   rstClrHse.Fields("fldClearingHouseID").Value = 83 Then            'Medicare of Iowa (WPS) has a different file name xxxx_0000????.ttt - 00008480.837
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\14541.dat") > "" Then
                    Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\14541.dat"
                End If
                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(rstClrHse.Fields("fldEFileName").Value, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\14541.dat")
        ElseIf rstClrHse.Fields("fldClearingHouseID").Value = 60 Then        'HealthNow has a different file name IMS.CLM
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\Z78232.A" & Format(DatePart("D", Now()), "00")) > "" Then
                    Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\Z78232.A" & Format(DatePart("D", Now()), "00")
                End If
                Open (App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\filename.txt") For Output As #2
                Print #2, "Z78232.A" & Format(DatePart("D", Now()), "00")
                Close #2 'Close the file
                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(rstClrHse.Fields("fldEFileName").Value, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Z78232.A" & Format(DatePart("D", Now()), "00"))
        ElseIf rstClrHse.Fields("fldClearingHouseID").Value = 76 Then        'BcBs Of VT has a different file name - 00723801.X12
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\00723801.X12") > "" Then
                    Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\00723801.X12"
                End If
                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(rstClrHse.Fields("fldEFileName").Value, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\00723801.X12")
        ElseIf rstClrHse.Fields("fldClearingHouseID").Value = 23 Or _
                   rstClrHse.Fields("fldClearingHouseID").Value = 72 Or _
                   rstClrHse.Fields("fldClearingHouseID").Value = 74 Then        'Value Options has a different file name - 00723801.X12
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\IMS.CLM") > "" Then Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\IMS.CLM"
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\IMS.CNT") > "" Then Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\IMS.CNT"
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\IMS.AMT") > "" Then Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\IMS.AMT"
                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(rstClrHse.Fields("fldEFileName").Value, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\IMS.CLM")
                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Left(IfNull(rstClrHse.Fields("fldEFileName").Value, ""), InStr(1, IfNull(rstClrHse.Fields("fldEFileName").Value, ""), ".")) & "CNT", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\IMS.CNT")
                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Left(IfNull(rstClrHse.Fields("fldEFileName").Value, ""), InStr(1, IfNull(rstClrHse.Fields("fldEFileName").Value, ""), ".")) & "AMT", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\IMS.AMT")
        ElseIf rstClrHse.Fields("fldClearingHouseID").Value = 90 Or _
                   rstClrHse.Fields("fldClearingHouseID").Value = 91 Or _
                   rstClrHse.Fields("fldClearingHouseID").Value = 100 Then        'Post-N-Track
                Open (App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & rstClrHse.Fields("fldEFileName").Value & Chr(34) & "  -site Post_N_Track -p /Upload/"
                Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                Close #2 'Close the file
        Else
                If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ElecClaim.txt") > "" Then
                    Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ElecClaim.txt"
                End If
                Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(rstClrHse.Fields("fldEFileName").Value, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ElecClaim.txt")
                Open (App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\filename.txt") For Output As #2
                Print #2, IfNull(rstClrHse.Fields("fldEFileName").Value, "")
                Close #2 'Close the file
        End If
            
        If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") > "" Then        'ThinEDI uses FTP
                strProgram = App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat"
        Else
                strProgram = Environ("PROCOMM") & "\pw5.exe " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ZSEND32.wax"
        End If

        dblProgID = Shell(strProgram, vbNormalFocus)
        intExit = WaitOnProgram(dblProgID, True)
            
        'Move submission files to Submit folder
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt") > "" Then
                Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(rstClrHse.Fields("fldEFileName").Value, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt"
        End If
            
    End If
        
     '   If rstClrHse.Fields("fldClearingHouseID").Value = 60 Then 'HealthNow
     '           Open (App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\rptname.txt") For Output As #2
     '           Print #2, "Z*." & Format(DatePart("y", Now()), "000")
     '           Close #2 'Close the file
     '           strProgram = Environ("PROCOMM") & "\pw5.exe " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ZRECV32.wax"
     '           dblProgID = Shell(strProgram, vbNormalFocus)
     '           intExit = WaitOnProgram(dblProgID, True)
     '   End If
        
    If rstClrHse.Fields("fldClearingHouseID").Value = 66 Then        'BcBs of RI has a different file name 837P_CORP
            If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\837P_CORP") > "" Then
                Kill App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\837P_CORP"
            End If
            Call objFS.CopyFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & IfNull(rstClrHse.Fields("fldEFileName").Value, ""), App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\837P_CORP")
    End If

    If IfNull(rstClrHse.Fields("fldRemittanceYN").Value, "") = "Y" And _
            (Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ZRECV32.wax") > "" Or _
             Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ERAs.bat") > "") Then
        If (Format(Now, "w") = 2 Or Format(Now, "w") = 4 Or Format(Now, "w") = 6) And Hour(Now) < 12 Then '2-Monday, 4-Wednesday, 6-friday and in the AM
            If Dir(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ERAs.bat") > "" Then        'ThinEDI uses FTP
                    strProgram = App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ERAs.bat"
            Else
                    strProgram = Environ("PROCOMM") & "\pw5.exe " & App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ZRECV32.wax"
            End If
            dblProgID = Shell(strProgram, vbNormalFocus)
            intExit = WaitOnProgram(dblProgID, True)
        End If
    End If
    If rstClrHse.Fields("fldClearingHouseID").Value <> 48 Then
            Call MoveFiles(rstClrHse, CLng(rstClrHse.Fields("fldClearingHouseID").Value))
    End If
        
    rstClrHse.MoveNext

Loop
    
rstClrHse.Close
Set rstClrHse = Nothing
    
Exit Sub
    
ErrTrap:
    Call ShowError(Err)
    Set objEDI = Nothing
    Set rst = Nothing

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
Dim objFolder
Dim objFile As Scripting.File
Dim objText As Scripting.TextStream

On Error Resume Next
'On Error GoTo ErrTrap:
    
Set objFS = CreateObject("Scripting.FileSystemObject")
        
    If rstClrHse.Fields("fldClearingHouseID").Value = 1 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 62 Then        'Thin, BCBS of OR
'''    'Move Response files to Response folder
'''    'If objFS.FileExists(App.Path & "\THIN\*.RSP") Then
'''        Call objFS.MoveFile(App.Path & "\THIN\*.RSP", App.Path & "\THIN\RSP\")
'''    'End If
    '   Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.txt", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.997", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\997\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.PCC", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\PCC\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.ECC", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ECC\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.SFC", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\SFC\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.WPS", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\WPS\")
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.EPS", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\EPS\")
        'Delete HyperTerminal trace files
        Call objFS.DeleteFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*.dbg")
    End If
    
       
    If rstClrHse.Fields("fldClearingHouseID").Value = 36 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 37 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 54 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 75 Then 'Medicare NCA SCA OH SC
        Call objFS.MoveFile(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\997*.RSP", App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
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
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 34 Then        'Medicare of Florida
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK.TXT") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK.TXT", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\" & Format(Now, "YYYYmmdd") & Hour(Now) & ".ACK")
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
    
    If rstClrHse.Fields("fldClearingHouseID").Value = 41 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 55 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 77 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 78 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 79 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 80 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 81 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 82 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 83 Then       'Medicare of IL, WPS
       Set objFolder = objFS.GetFolder(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\")
       Set objFile = objFolder.Files

       For Each objFile In objFolder.Files
          inFile = objFile.Name
          curFile = objFile
          If inFile > "" And Right(inFile, 7) = "835.dat" Then
           '  If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\*835.dat") > "" Then
                Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\" & Left(inFile, 28) & ".835")
           '  End If
          End If
       Next
       
       Set objFile = Nothing
       Set objFolder = Nothing

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
  
    If rstClrHse.Fields("fldClearingHouseID").Value = 2 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 6 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 9 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 27 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 33 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 44 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 45 Or _
       rstClrHse.Fields("fldClearingHouseID").Value = 49 Then  'Anthem
        If Dir(App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\AAL*.*") > "" Then
            Call objFS.MoveFile(App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\AAL*.*", App.Path & "\" & IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\AAL\")
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
                     Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
               ElseIf InStr(3, strLine, "GS*") Then
                  If Not InStr(3, strLine, "TA1*") And Not InStr(3, strLine, "ST*") And Not InStr(3, strLine, "ST^") And Not InStr(3, strLine, "ST\") Then strLine = objText.ReadLine
                  objText.Close
                  If InStr(3, strLine, "TA1*") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                  If InStr(3, strLine, "ST*997*") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                  If InStr(3, strLine, "ST^997^") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                  If InStr(3, strLine, "ST\997\") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                  If InStr(3, strLine, "ST^835^") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
                  If InStr(3, strLine, "ST*835*") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
               End If
            Else
               objText.Close
               If InStr(3, strLine, "TA1*") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
               If InStr(3, strLine, "ST*997*") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
               If InStr(3, strLine, "ST^997^") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
               If InStr(3, strLine, "ST\997\") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
               If InStr(3, strLine, "ST^835^") Then Call objFS.MoveFile(curFile, App.Path & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
               If InStr(3, strLine, "ST*835*") Then
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

