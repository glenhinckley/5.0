VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.2#0"; "MSCOMCTL.OCX"
Begin VB.Form frmEmail 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Psyquel - Electronic Claims"
   ClientHeight    =   3750
   ClientLeft      =   45
   ClientTop       =   330
   ClientWidth     =   5850
   Icon            =   "frmEmail.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   3750
   ScaleWidth      =   5850
   ShowInTaskbar   =   0   'False
   StartUpPosition =   1  'CenterOwner
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
         Caption         =   "Creating electronic email records ..."
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
      Caption         =   "Appointment Reminder Email Application"
      Height          =   255
      Left            =   240
      TabIndex        =   2
      Top             =   120
      Width           =   4215
   End
End
Attribute VB_Name = "frmEmail"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

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
Private CONST_DAYS As Long
Private CONST_GROUP As Long
Private CONST_PROVIDER As Long

'----------------------
' Form Events
'----------------------
Private Sub Form_Load()

    Dim strMachineName As String
    
    On Error GoTo ErrTrap:
    
    strMachineName = GetMachineName()
    
    If Trim(strMachineName) <> "PSYAPP03" And Trim(strMachineName) <> "PSYMGT01" And Trim(strMachineName) <> "ADM-01" Then
        Call MsgBox("Electronic Emails can only be processed from the PSYMGT01 Server.(" & strMachineName & ")", vbOKOnly + vbCritical, "Close Wizard")
        Me.Tag = "Cancel"
        Me.Hide
        Unload Me
        Exit Sub
    End If

    InitSettings
    Me.Show
    GenerateFile
    
    Call SendEmailMessage("QWareAdmin@Psyquel.com", GetMachineName() & "@psyquel.com", "Electronic Emails", "The Electronic Emails Daemon completed Successfully" & vbCrLf & Me.txtStatus.Text)

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
    Dim lngDays As String
    Dim lngPos As Long
    Dim lngProvider, lngGroup As Long
    On Error GoTo ErrTrap:
    
    blnEmailSuccess = True
    blnEmailFailure = True
    
    'make sure .ini exists
    strLoc = Dir(App.Path & "\Email.ini")
    
    If strLoc = "" Then
        'we are resetting
SetLoc:
        Open (App.Path & "\Email.ini") For Output As #1
        Print #1, "[Email]"
        
        MsgBox "Please choose an output directory", vbInformation, "Initialize settings"
        frmBrowse.Show vbModal
        
        strLoc = frmBrowse.dirList.Path
        If Right(strLoc, 1) <> "\" Then
            strLoc = strLoc & "\"
        End If
        If strLoc <> "" Then
            Print #1, "Output=" & strLoc
        End If
        
        strTemp = InputBox("Number of Days:")
        
        lngDays = frmBrowse.dirList.Path
        If CLng(Trim(strTemp)) > 0 Then
            lngDays = CLng(Trim(strTemp))
        End If
        If lngDays > 0 Then
            Print #1, "Days=" & lngDays
        End If
        
        Unload frmBrowse
        Close #1
    Else
        'find and set parameters from .ini
        strLoc = "": lngDays = 2
        Open (App.Path & "\Email.ini") For Input As #1
        Do While Not EOF(1)
            Input #1, strTemp
            lngPos = InStr(strTemp, "=")
            If lngPos > 1 Then
                Select Case Left(strTemp, lngPos - 1)
                    Case "Output"
                        strLoc = Right(strTemp, Len(strTemp) - lngPos)
                    Case "Days"
                        lngDays = CLng(Trim(Right(strTemp, Len(strTemp) - lngPos)))
                    Case "Group"
                        lngGroup = Right(strTemp, Len(strTemp) - lngPos)
                    Case "Provider"
                        lngProvider = Right(strTemp, Len(strTemp) - lngPos)
                End Select
            End If
        Loop
        
        Close #1
        
        'if no output dir is found, recreate file
        If strLoc = "" Then
            MsgBox "Error"
            Kill App.Path & "\PatStmt.ini"
            GoTo SetLoc
            Exit Sub
        End If
        If lngDays = 0 Then lngDays = 2
            
    End If
    
    CONST_OUTPUT_DIR = strLoc
    CONST_DAYS = lngDays
    CONST_GROUP = lngGroup
    CONST_PROVIDER = lngProvider
    
    Exit Sub
ErrTrap:
    Close #1
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub

Private Sub GenerateFile()
'-------------------------------------------------------------------------------
'Author: Duane C Orth
'Date: 02/08/2012
'Description: Main function that generates Email Appointment Reminders
'Parameters:
'Returns:
'-------------------------------------------------------------------------------

On Error GoTo ErrTrap:

Dim objAppt As ApptBZ.CApptBZ
Dim objPatAppt As ApptBZ.CPatApptBZ
Dim objProvider As ProviderBz.CProviderBZ
Dim objClinic As ClinicBz.CClinicBz
Dim objBFact As BenefactorBz.CBenefactorBz
Dim rst As ADODB.Recordset
Dim rstAppt As ADODB.Recordset
Dim rstPatAppt As ADODB.Recordset
Dim rstExc As ADODB.Recordset
Dim rstProvider As ADODB.Recordset
Dim rstClinic As ADODB.Recordset
Dim rstBFact As ADODB.Recordset
Dim strWhere As String
Dim strProgram As String
Dim dblProgID As Double
Dim intExit As Integer
Dim strSubject As String
Dim strTextBody As String
Dim lngProviderID, lngCnt As Long
Dim strCarrier As String
    
Dim objFS As Scripting.FileSystemObject
Dim objFolder, objFile
Dim objText As Scripting.TextStream

Set objFS = CreateObject("Scripting.FileSystemObject")

Open (CONST_OUTPUT_DIR & "EmailSent" & Format(Now(), "yyyymmdd") & Format(Now(), "ss") & Format(Now(), "nn") & ".log") For Output As #2
Print #2, "Start: " & Format(Now() + CONST_DAYS, "long date") & " " & Format(Now() + CONST_DAYS, "long time") & vbCrLf

Screen.MousePointer = vbHourglass
    
lngProviderID = 1: lngCnt = 1

Set objProvider = CreateObject("ProviderBz.CProviderBz")


If CONST_GROUP > 0 And CONST_GROUP <> 680 Then Set rstProvider = objProvider.Fetch(False, " (A.fldGroupID = " & CONST_GROUP & ") AND (A.fldApptConfirmEmailYN = 'Y' OR A.fldApptConfirmEmailYN = 'E' OR A.fldApptConfirmEmailYN = 'T' OR A.fldApptConfirmEmailYN = 'B' OR A.fldApptConfirmEmailYN = 'O') AND B.fldEmail IS NOT NULL ", "fldProviderID")    'fldProviderID
If CONST_PROVIDER > 0 Then Set rstProvider = objProvider.Fetch(False, " (A.fldProviderID = " & CONST_PROVIDER & ") AND (A.fldApptConfirmEmailYN = 'Y' OR A.fldApptConfirmEmailYN = 'E' OR A.fldApptConfirmEmailYN = 'T' OR A.fldApptConfirmEmailYN = 'B' OR A.fldApptConfirmEmailYN = 'O') AND B.fldEmail IS NOT NULL ", "fldProviderID")    'fldProviderID
If CONST_GROUP <= 0 And CONST_PROVIDER <= 0 Then Set rstProvider = objProvider.Fetch(False, " (A.fldApptConfirmEmailYN = 'Y' OR A.fldApptConfirmEmailYN = 'E' OR A.fldApptConfirmEmailYN = 'T' OR A.fldApptConfirmEmailYN = 'B' OR A.fldApptConfirmEmailYN = 'O') AND B.fldEmail IS NOT NULL ", "fldProviderID")    'fldProviderID

'Set rstProvider = objProvider.Fetch(False, " (A.fldApptConfirmEmailYN = 'Y' OR A.fldApptConfirmEmailYN = 'E' OR A.fldApptConfirmEmailYN = 'T' OR A.fldApptConfirmEmailYN = 'B' OR A.fldApptConfirmEmailYN = 'O') AND B.fldEmail IS NOT NULL ", "fldProviderID")    'fldProviderID

txtStatus.Text = "Start: " & Format(Now() + CONST_DAYS, "long date") & " " & Format(Now() + CONST_DAYS, "long time") & vbCrLf
DoEvents
barStatus.Max = rstProvider.RecordCount

Do While Not rstProvider.EOF

'   Set objAppt = CreateObject("ApptBZ.CApptBz")
   Set rstAppt = FetchProviderAppts(rstProvider.Fields("fldProviderID").Value, CDate(Format(Now() + CONST_DAYS, "mm/dd/yyyy")))
'   Set objAppt = Nothing
    
   Do While Not rstAppt.EOF
      If rstAppt.Fields("fldPatApptID") > 0 Then 'Patient Records only
         Set objPatAppt = CreateObject("ApptBZ.CPatApptBZ")
         Set rstPatAppt = objPatAppt.FetchByAppt(rstAppt.Fields("fldApptID"))
        
On Error GoTo ErrSkip:

         If Not rstPatAppt.EOF Then
            If rstAppt.Fields("fldPatientID") > 0 Then
               Set objBFact = CreateObject("BenefactorBz.CBenefactorBz")
               Set rstBFact = objBFact.FetchByID(rstAppt.Fields("fldPatientID"))
              
               If rstBFact.Fields("fldApptReminderYN") = "Y" And _
                (((rstProvider.Fields("fldApptConfirmEmailYN").Value = "Y" Or rstProvider.Fields("fldApptConfirmEmailYN").Value = "E" Or rstProvider.Fields("fldApptConfirmEmailYN").Value = "B" Or rstProvider.Fields("fldApptConfirmEmailYN").Value = "O") And _
                    rstBFact.Fields("fldEmailYN") = "Y" And _
                    rstBFact.Fields("fldEmail") > "" And _
                    InStr(1, rstBFact.Fields("fldEmail"), "@") > 0 And _
                    InStr(1, rstBFact.Fields("fldEmail"), ".") > 0) Or _
                  ((rstProvider.Fields("fldApptConfirmEmailYN").Value = "T" Or rstProvider.Fields("fldApptConfirmEmailYN").Value = "B" Or rstProvider.Fields("fldApptConfirmEmailYN").Value = "O") And _
                    IIf(IsNull(rstBFact.Fields("fldMobilePhone")), "", rstBFact.Fields("fldMobilePhone")) > "" And _
                    rstBFact.Fields("fldMobileMsgYN") = "Y")) Then
            
                  strCarrier = IIf(IsNull(rstBFact.Fields("fldMobileCarrier").Value), "", rstBFact.Fields("fldMobileCarrier").Value)
                  If IIf(IsNull(rstBFact.Fields("fldMobilePhone")), "", rstBFact.Fields("fldMobilePhone")) > "" And rstBFact.Fields("fldTextYN") = "Y" And (rstProvider.Fields("fldApptConfirmEmailYN").Value = "T" Or rstProvider.Fields("fldApptConfirmEmailYN").Value = "B" Or rstProvider.Fields("fldApptConfirmEmailYN").Value = "O") Then
                     If (IsNull(rstBFact.Fields("fldMobileCarrier").Value) Or IsNull(rstBFact.Fields("fldMobileCarrierID").Value) Or InStr(1, strCarrier, "@") = 0) Then
                        strCarrier = GetCarrierID(rstAppt.Fields("fldPatientID").Value, rstBFact.Fields("fldMobilePhone"), 1)
                     ElseIf rstBFact.Fields("fldMobilePhone") <> Mid(strCarrier, 1, InStr(1, strCarrier, "@") - 1) Then
                        strCarrier = GetCarrierID(rstAppt.Fields("fldPatientID").Value, rstBFact.Fields("fldMobilePhone"), 1)
                     End If
                  End If
              '    If (rstProvider.Fields("fldApptConfirmEmailYN").Value = "B") Then
              '       If (IsNull(rstBFact.Fields("fldMobileCarrier").Value) Or IsNull(rstBFact.Fields("fldMobileCarrierID").Value)) Then
              '          strCarrier = GetCarrierID(rstAppt.Fields("fldPatientID").Value, rstBFact.Fields("fldMobilePhone"), 2)
              '       ElseIf rstBFact.Fields("fldMobilePhone") <> Mid(strCarrier, 1, InStr(1, strCarrier, "@") - 1) Then
              '          strCarrier = GetCarrierID(rstAppt.Fields("fldPatientID").Value, rstBFact.Fields("fldMobilePhone"), 2)
              '       End If
              '    End If
                    
                  Set objClinic = CreateObject("ClinicBz.CClinicBz")
                  If rstAppt.Fields("fldClinicID") > 0 Then
                     Set rstClinic = objClinic.FetchDetail(rstAppt.Fields("fldClinicID"))
                  Else
                     Set rstClinic = objClinic.FetchDetail(rstBFact.Fields("fldClinicID"))
                  End If
               
                  If Not rstClinic.EOF Then
                    strSubject = "Scheduled appointment with " & rstProvider.Fields("fldFirstName") & " " & rstProvider.Fields("fldLastName") & ", " & rstProvider.Fields("fldProviderType") & " on " & Format(Now() + CONST_DAYS, "long date") & " at " & Format(rstAppt.Fields("fldStartDateTime"), "long time")
                    strTextBody = "This is a reminder of your scheduled appointment with " & rstProvider.Fields("fldFirstName") & " " & rstProvider.Fields("fldLastName") & ", " & rstProvider.Fields("fldProviderType") & " on " & Format(Now() + CONST_DAYS, "long date") & " at " & Format(rstAppt.Fields("fldStartDateTime"), "long time") & "." & Chr(10) & Chr(10)
                    strTextBody = strTextBody & "Location:" & Chr(10) & rstClinic.Fields("fldBusinessName").Value & Chr(10)
                    If rstClinic.Fields("fldPOSCode").Value <> "02" And rstClinic.Fields("fldPOSCode").Value <> "10" And rstClinic.Fields("fldPOSCode").Value <> "12" Then
                        strTextBody = strTextBody & rstClinic.Fields("fldAddress1").Value & Chr(10) & rstClinic.Fields("fldCity").Value & ", " & rstClinic.Fields("fldState").Value & " " & rstClinic.Fields("fldZip").Value & Chr(10)
                    End If
                    strTextBody = strTextBody & "Phone: " & Format(rstProvider.Fields("fldPhone1").Value, "(###)###-####") & Chr(10)
                    
                    If (rstClinic.Fields("fldPOSCode").Value = "02" Or rstClinic.Fields("fldPOSCode").Value = "10") And _
                       IIf(IsNull(rstProvider.Fields("fldVirtualSessionYN").Value), "N", rstProvider.Fields("fldVirtualSessionYN").Value) = "Y" And _
                       IIf(IsNull(rstProvider.Fields("fldVirtualPatientURL").Value), "", rstProvider.Fields("fldVirtualSessionYN").Value) > "" Then
                        strTextBody = strTextBody & "Use the following link for your Virtual Session: " & rstProvider.Fields("fldVirtualPatientURL").Value & Chr(10) & Chr(10)
                    Else
                        strTextBody = strTextBody & Chr(10)
                    End If
                        
                    strTextBody = strTextBody & "Please give us a call if you need to cancel or reschedule your appointment. "
                    strTextBody = strTextBody & "Note, there is a cancellation policy. Late cancels and no shows could result in a charge. "
                    If rstClinic.Fields("fldPOSCode").Value = "02" Or rstClinic.Fields("fldPOSCode").Value = "10" Or rstClinic.Fields("fldPOSCode").Value = "12" Then
                        strTextBody = strTextBody & "We look forward to talking with you!" & Chr(10) & Chr(10)
                    Else
                        strTextBody = strTextBody & "We look forward to seeing you!" & Chr(10) & Chr(10)
                    End If
                    strTextBody = strTextBody & "The information contained in this transmission may contain privileged and " & _
                        "confidential information. It is intended only for the use of the person(s) named " & _
                        "above. If you are not the intended recipient, you are hereby notified that any " & _
                        "review, dissemination, distribution or duplication of this communication is " & _
                        "strictly prohibited. If you are not the intended recipient, please contact the " & _
                        "sender by reply email and destroy all copies of the original message."
                 ' Debug.Print strTextBody   rstProvider.Fields("fldPhone1").Value
                    If rstProvider.Fields("fldApptConfirmEmailYN").Value = "T" And strCarrier > "" And rstBFact.Fields("fldTextYN") = "Y" Then
                        Print #2, rstBFact.Fields("fldFirst") & " " & rstBFact.Fields("fldLast") & " : " & strCarrier & " : " & strSubject '& vbCrLf
                        Call SendEmailMessage(strCarrier, "DoNotReply@psyquel.com", "Appointment", strSubject)
                    ElseIf rstProvider.Fields("fldApptConfirmEmailYN").Value = "O" Then
                        If strCarrier > "" And rstBFact.Fields("fldTextYN") = "Y" Then
                            Print #2, rstBFact.Fields("fldFirst") & " " & rstBFact.Fields("fldLast") & " : " & strCarrier & " : " & strSubject '& vbCrLf
                            Call SendEmailMessage(strCarrier, "DoNotReply@psyquel.com", "Appointment", strSubject)
                        ElseIf rstBFact.Fields("fldEmail") > "" And rstBFact.Fields("fldEmailYN") = "Y" Then
                            Print #2, rstBFact.Fields("fldFirst") & " " & rstBFact.Fields("fldLast") & " : " & rstBFact.Fields("fldEmail") & " : " & strSubject '& vbCrLf
                            Call SendEmailMessage(rstBFact.Fields("fldEmail"), "DoNotReply@psyquel.com", strSubject, strTextBody)
                        End If
                    ElseIf rstProvider.Fields("fldApptConfirmEmailYN").Value = "B" Then
                        If strCarrier > "" And rstBFact.Fields("fldTextYN") = "Y" Then
                            Print #2, rstBFact.Fields("fldFirst") & " " & rstBFact.Fields("fldLast") & " : " & strCarrier & " : " & strSubject '& vbCrLf
                            Call SendEmailMessage(strCarrier, "DoNotReply@psyquel.com", "Appointment", strSubject)
                        End If
                        If rstBFact.Fields("fldEmail") > "" And rstBFact.Fields("fldEmailYN") = "Y" Then
                            Print #2, rstBFact.Fields("fldFirst") & " " & rstBFact.Fields("fldLast") & " : " & rstBFact.Fields("fldEmail") & " : " & strSubject '& vbCrLf
                            Call SendEmailMessage(rstBFact.Fields("fldEmail"), "DoNotReply@psyquel.com", strSubject, strTextBody)
                        End If
                    ElseIf rstProvider.Fields("fldApptConfirmEmailYN").Value = "Y" Or rstProvider.Fields("fldApptConfirmEmailYN").Value = "E" Then
                        Print #2, rstBFact.Fields("fldFirst") & " " & rstBFact.Fields("fldLast") & " : " & rstBFact.Fields("fldEmail") & " : " & strSubject '& vbCrLf
                        Call SendEmailMessage(rstBFact.Fields("fldEmail"), "DoNotReply@psyquel.com", strSubject, strTextBody)
                    End If
                  End If
                  rstClinic.Close
                  Set rstClinic = Nothing
                  Set objClinic = Nothing
               End If
               rstBFact.Close
               Set rstBFact = Nothing
               Set objBFact = Nothing
            End If
            'rstPatAppt.MoveNext
         End If
      End If
        
ErrSkip:
      Set rstPatAppt = Nothing
      Set objPatAppt = Nothing
      rstAppt.MoveNext
   Loop
   
On Error GoTo ErrTrap:

   rstAppt.Close
   Set rstAppt = Nothing
   barStatus.Value = lngCnt
   barStatus.Refresh
   lngCnt = lngCnt + 1
   rstProvider.MoveNext
Loop

Print #2, "Finished: " & Format(Now() + CONST_DAYS, "long date") & " " & Format(Now() + CONST_DAYS, "long time") & vbCrLf
Close #2 'Close the file
rstProvider.Close
Set rstProvider = Nothing
Set objProvider = Nothing
Screen.MousePointer = vbDefault
    
Exit Sub
    
ErrTrap:
    Print #2, "Error:" & Err.Description & vbLf & vbLf & "Error Number: " & Err.Number & vbCrLf & txtStatus.Text & vbCrLf
    Close #2 'Close the file
    txtStatus.Text = "Error:" & Err.Description & vbLf & vbLf & "Error Number: " & Err.Number & vbCrLf & txtStatus.Text
 '  Debug.Print txtStatus.Text & strSubject
    DoEvents
    If blnEmailFailure Then
        Call SendEmailMessage("QWareAdmin@Psyquel.com", GetMachineName() & "@psyquel.com", "Electronic Email Failure - GenerateFile", "The Electronic Email Daemon reported a failure in " & Err.Source & "." & vbCrLf & "Unhandled Error: " & Err.Number & ": " & Err.Description & vbCrLf & Me.txtStatus.Text)
    End If
  '  Call ShowError(Err)
    Set rstProvider = Nothing
    Set rstClinic = Nothing
    Set rstBFact = Nothing
    Set rstAppt = Nothing
    Screen.MousePointer = vbDefault
    Unload Me
End Sub


'-------------------------
' Public Routines
'-------------------------
Public Sub SendEmailMessage(ByVal strRecipient As String, ByVal strSender As String, ByVal strSubject As String, ByVal strMessage As String)
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
   Dim strMachineName As String

   On Error GoTo ErrHand
    
   Set Fields = objConfig.Fields

' Set config fields we care about

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
       
    Set objMail.Configuration = objConfig
    If IsNull(strSender) Or strSender <= "" Then
        objMail.From = "QWareAdmin@Psyquel.com"
    Else
        objMail.From = strSender
    End If
    If IsNull(strRecipient) Or strRecipient <= "" Or (InStr(10, strRecipient, " ") > 0 And InStr(10, strRecipient, ";") = 0) Then
        objMail.To = "QWareAdmin@Psyquel.com"
    Else
        objMail.To = strRecipient
    End If
    objMail.Subject = strSubject
    objMail.TextBody = strMessage
    objMail.Send
    Set objMail = Nothing
    
    Wait 2
    
    Exit Sub
ErrHand:
    'MsgBox (Err.Number & ", " & Err.Source & ", " & Err.Description)
    Err.Raise Err.Number, Err.Source, Err.Description
    If blnEmailFailure Then
        Call SendEmailMessage("QWareAdmin@Psyquel.com", GetMachineName() & "@psyquel.com", "Electronic Email Failure - SendEmailMessage", "The Electronic Email Daemon reported a failure in " & Err.Source & "." & vbCrLf & "Unhandled Error: " & Err.Number & ": " & Err.Description & vbCrLf & Me.txtStatus.Text)
    End If
    Unload Me
End Sub


Public Sub Wait(seconds As Integer)
   Dim dTimer As Double

   dTimer = Timer

   Do While Timer < dTimer + seconds
      DoEvents
   Loop

End Sub



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
    If blnEmailFailure Then
        Call SendEmailMessage("QWareAdmin@Psyquel.com", GetMachineName() & "@psyquel.com", "Electronic Email Failure - SendEmailMessage", "The Electronic Email Daemon reported a failure in " & Err.Source & "." & vbCrLf & "Unhandled Error: " & Err.Number & ": " & Err.Description & vbCrLf & Me.txtStatus.Text)
    End If
    
End Function

Public Function FetchProviderAppts(ByVal lngProviderID As Long, ByVal dteStartDate As Date) As ADODB.Recordset
       
    Dim cnn As ADODB.Connection
    Dim cmd As ADODB.Command
    Dim rst As ADODB.Recordset
    
    On Error GoTo ErrTrap:

    Set cmd = New ADODB.Command
    Set cnn = New ADODB.Connection
    
    'Acquire the database connection.
    cnn.Open (CONST_PSYREPL_CNN)
    Set cmd.ActiveConnection = cnn
    
    'Create the parameter objects
    With cmd
        .CommandText = "uspRptSuperbillAttendanceList"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@StartDate", adDBTimeStamp, adParamInput, , dteStartDate)
        .Parameters.Append .CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID)
        .Parameters.Append .CreateParameter("@UserID", adInteger, adParamInput, , lngProviderID)
    End With
    
    'Execute the stored procedure
    Set rst = New ADODB.Recordset
    rst.CursorLocation = adUseClient
    rst.Open cmd, , adOpenForwardOnly, adLockReadOnly
    
    Set FetchProviderAppts = rst
    
    'Disconnect the recordset and clean house
    Set cmd.ActiveConnection = Nothing
    Set cmd = Nothing
    Set rst.ActiveConnection = Nothing
    Set cnn = Nothing
    
    'Signal successful completion
  '  GetObjectContext.SetComplete

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
  '  GetObjectContext.SetAbort
    Set cnn = Nothing
    Set cmd = Nothing
    Set rst = Nothing
    txtStatus.Text = Err.Description
    Err.Raise Err.Number, Err.Source, Err.Description
    If blnEmailFailure Then
        Call SendEmailMessage("QWareAdmin@Psyquel.com", GetMachineName() & "@psyquel.com", "Electronic Email Failure - SendEmailMessage", "The Electronic Email Daemon reported a failure in " & Err.Source & "." & vbCrLf & "Unhandled Error: " & Err.Number & ": " & Err.Description & vbCrLf & Me.txtStatus.Text)
    End If
End Function

Private Function GetCarrierID(ByVal lngPatientID As Long, ByVal strPhoneNbr As String, ByVal intType As Long) As String
' Requires a reference to Microsoft XML library
Dim xmlhttp As MSXML2.xmlhttp
Dim sURL As String
'Dim strXML As String

'The return data.
Dim strStatus As String
Dim strNumber As String
Dim strWireless As String
Dim strCarrier As String
Dim strCarrCode As String
Dim strSMSaddr As String
Dim strMMSaddr As String
Dim strCountry As String
Dim intStart, intEnd, i As Long
Dim arrWrk As Variant
Dim strWrk As String

On Error GoTo ErrTrap:

If IsNull(strPhoneNbr) Then
    GetCarrierID = ""
    Exit Function
End If
Set xmlhttp = New xmlhttp

sURL = "https://api.data247.com/v3.0?key=1002261141e95156d102850088361141e95156e3013438894&api=CU&phone=1" & strPhoneNbr
'& "&addfields=sms_address,mms_address"
'sURL = "https://api.data24-7.com/v/2.0?user=dcorth&pass=psyquel1&api=C&p1=1" & strPhoneNbr & "&addfields=sms_address,mms_address"
'strXML = "user={0}&pass={1}&p1={3}&addfields={4}, dcorth, psyquel1, 12107227416, sms_address,mms_address"

xmlhttp.Open "POST", sURL, False
xmlhttp.setRequestHeader "Content-Type", "application/x-www-form-urlencoded"
xmlhttp.Send 'strXML
'MsgBox xmlhttp.responseXML.XML 'displays any response string
'Debug.Print xmlhttp.responseText
    
If xmlhttp.Status = 200 Then
    strStatus = xmlhttp.statusText
    strWrk = Replace(Mid(xmlhttp.responseText, InStr(1, xmlhttp.responseText, "results") + 12), """", "")
    arrWrk = Split(strWrk, ", ")
    For i = 0 To UBound(arrWrk)
        If InStr(1, arrWrk(i), "status:") > 0 Then
            intStart = InStr(1, arrWrk(i), "status:")
         '   strStatus = Mid(arrWrk(i), intStart + 8)
        End If
        If InStr(1, arrWrk(i), "phone:") > 0 Then
            intStart = InStr(1, arrWrk(i), ":")
            strNumber = Replace(Replace(Trim(Mid(arrWrk(i), intStart + 1)), "}", ""), "]", "")
        End If
        If InStr(1, arrWrk(i), "wless:") > 0 Then
            intStart = InStr(1, arrWrk(i), ":")
            strWireless = Replace(Replace(Trim(Mid(arrWrk(i), intStart + 1)), "}", ""), "]", "")
        End If
        If InStr(1, arrWrk(i), "carrier_name:") > 0 Then
            intStart = InStr(1, arrWrk(i), ":")
            strCarrier = Replace(Replace(Trim(Mid(arrWrk(i), intStart + 1)), "}", ""), "]", "")
        End If
        If InStr(1, arrWrk(i), "carrier_id:") > 0 Then
            intStart = InStr(1, arrWrk(i), ":")
            strCarrCode = Replace(Replace(Trim(Mid(arrWrk(i), intStart + 1)), "}", ""), "]", "")
        End If
        If InStr(1, arrWrk(i), "country:") > 0 Then
            intStart = InStr(1, arrWrk(i), ":")
            strCountry = Replace(Replace(Trim(Mid(arrWrk(i), intStart + 1)), "}", ""), "]", "")
        End If
        If InStr(1, arrWrk(i), "sms_address:") > 0 Then
            intStart = InStr(1, arrWrk(i), ":")
            strSMSaddr = Replace(Replace(Trim(Mid(arrWrk(i), intStart + 1)), "}", ""), "]", "")
        End If
        If InStr(1, arrWrk(i), "mms_address:") > 0 Then
            intStart = InStr(1, arrWrk(i), ":")
            strMMSaddr = Replace(Replace(Trim(Mid(arrWrk(i), intStart + 1)), "}", ""), "]", "")
        End If
    Next i
    
    If intType = 1 And strStatus = "OK" And strSMSaddr > "" Then
        GetCarrierID = strSMSaddr
        Call UpdateCarrierID(lngPatientID, strCarrCode, strSMSaddr)
    End If
    If intType = 2 And strStatus = "OK" And strMMSaddr > "" Then
        GetCarrierID = strMMSaddr
        Call UpdateCarrierID(lngPatientID, strCarrCode, strMMSaddr)
    End If
    
    If strStatus <> "OK" Then
        Call SendEmailMessage("QWareAdmin@Psyquel.com", GetMachineName() & "@psyquel.com", "Electronic Email Failure - SendEmailMessage", "The Electronic Email Daemon reported a failure in UpdateCarrierID " & strStatus & "." & vbCrLf & "PatientID: " & lngPatientID & " : " & vbCrLf & Me.txtStatus.Text)
    End If
End If

Exit Function
    
ErrTrap:
  '  GetObjectContext.SetAbort
    txtStatus.Text = Err.Description
    Err.Raise Err.Number, Err.Source, Err.Description
    If blnEmailFailure Then
        Call SendEmailMessage("QWareAdmin@Psyquel.com", GetMachineName() & "@psyquel.com", "Electronic Email Failure - SendEmailMessage", "The Electronic Email Daemon reported a failure in " & Err.Source & "." & vbCrLf & "Unhandled Error: " & Err.Number & ": " & Err.Description & vbCrLf & Me.txtStatus.Text)
    End If
End Function
Public Sub UpdateCarrierID(ByVal lngPatientID As Long, ByVal lngCarrierID As Long, ByVal strCarrier As String)
    
    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command
    Dim strSQL As String

    On Error GoTo ErrTrap:

    'Prepare the SQL statement
    strSQL = "UPDATE tblBenefactor "
    strSQL = strSQL & " SET "
    strSQL = strSQL & " fldMobileCarrierID = " & lngCarrierID & ", "
    strSQL = strSQL & " fldMobileCarrier = '" & strCarrier & "' "
    strSQL = strSQL & " WHERE "
    strSQL = strSQL & " fldBenefactorID = " & lngPatientID

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
    If blnEmailFailure Then
        Call SendEmailMessage("QWareAdmin@Psyquel.com", GetMachineName() & "@psyquel.com", "Electronic Email Failure - SendEmailMessage", "The Electronic Email Daemon reported a failure in " & Err.Source & "." & vbCrLf & "Unhandled Error: " & Err.Number & ": " & Err.Description & vbCrLf & Me.txtStatus.Text)
    End If
End Sub

