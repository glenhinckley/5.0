VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.2#0"; "MSCOMCTL.OCX"
Begin VB.Form frmPatStmt 
   Caption         =   "Psyquel - Electronic Patient Statments"
   ClientHeight    =   3570
   ClientLeft      =   120
   ClientTop       =   450
   ClientWidth     =   6060
   Icon            =   "frmPatStmt.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   3570
   ScaleWidth      =   6060
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdDone 
      Caption         =   "&Cancel"
      Height          =   420
      Index           =   1
      Left            =   3360
      TabIndex        =   5
      Top             =   2520
      Width           =   1140
   End
   Begin VB.CommandButton cmdDone 
      Caption         =   "&OK"
      Height          =   420
      Index           =   0
      Left            =   960
      TabIndex        =   4
      Top             =   2520
      Width           =   1140
   End
   Begin VB.Frame Frame1 
      Caption         =   "File Format"
      Height          =   1860
      Left            =   240
      TabIndex        =   1
      Top             =   480
      Width           =   5475
      Begin MSComctlLib.ProgressBar barStatus 
         Height          =   255
         Left            =   120
         TabIndex        =   6
         Top             =   480
         Width           =   5055
         _ExtentX        =   8916
         _ExtentY        =   450
         _Version        =   393216
         Appearance      =   1
      End
      Begin VB.TextBox txtStatus 
         Height          =   825
         Left            =   120
         Locked          =   -1  'True
         MultiLine       =   -1  'True
         ScrollBars      =   2  'Vertical
         TabIndex        =   2
         TabStop         =   0   'False
         Top             =   900
         Width           =   5175
      End
      Begin VB.Label lblStatus 
         Alignment       =   2  'Center
         Caption         =   "Creating electronic email records ..."
         Height          =   255
         Left            =   0
         TabIndex        =   3
         Top             =   0
         Width           =   5295
      End
   End
   Begin VB.Label Label1 
      Alignment       =   2  'Center
      Caption         =   "Patient Statement Email Application"
      Height          =   255
      Left            =   600
      TabIndex        =   0
      Top             =   120
      Width           =   4215
   End
End
Attribute VB_Name = "frmPatStmt"
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
Private CONST_GROUP_ID As Integer
Private CONST_PROVIDER_ID As Integer

'----------------------
' Form Events
'----------------------
Private Sub Form_Load()

    Dim strMachineName As String
    
    On Error GoTo ErrTrap:
   
    strMachineName = GetMachineName()
    
'Dim objNote As EMRBZ.CNoteClinicalBZ
'Dim lngNoteID As Integer
'Dim dteDate As Date
'Set objNote = CreateObject("EMRBZ.CNoteClinicalBZ")
'dteDate = CDate("5/9/2019")
'dteDate = CDate("4/7/2019 6:0:00")
'dteDate = CDate("4/7/2019 7:0:00")
'lngNoteID = objNote.Insert(53605, 3, 5717670, 2, -1, CDate("5/9/2019"), 350, CDate("4/7/2019 6:0:00"), CDate("4/7/2019 7:0:00"), "Y:N", "Y:N:N:N:N", "Y:N:N:N:N", "Y:N:N", _
            "Y:N:N:N:N:N:N:N", "Y:N:N:N:N:N", "Y:N:N:N:N:N:N:N:N:N", "", "N:N:N:N", "Y:N", "36", "37", "38", "N:N:N:N:N:N:N:N:N:N:N:N", "N:N:N:N:N", "N:N:N:N:N:N:N:N:N:N", _
            "", "N:N:N:N:N", "", "", "", False, -1, CDate(0), "demosystem")
    
    If Trim(strMachineName) <> "PSYMGT01" And Trim(strMachineName) <> "PSYAPP03" And Trim(strMachineName) <> "ADM-01" Then
        Call MsgBox("Electronic Patient Statments can only be processed from the PSYMGT01 OR PSYAPP03 Server.(" & strMachineName & ")", vbOKOnly + vbCritical, "Close Wizard")
        Me.Tag = "Cancel"
        Me.Hide
        Unload Me
        Exit Sub
    End If

    InitSettings
    Me.Show
    GenerateFile
    
    Call SendEmailMessage("QWareAdmin@Psyquel.com", GetMachineName() & "@psyquel.com", "Electronic PatStmt", "The Electronic PatStmt Daemon completed Successfully" & vbCrLf & Me.txtStatus.Text)

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
    Dim strGroup As String
    Dim strProvider As String
    Dim lngPos As Long
    On Error GoTo ErrTrap:
    
    blnEmailSuccess = True
    blnEmailFailure = True
    
    'make sure .ini exists
    strLoc = Dir(App.Path & "\PatStmt.ini")
    
    If strLoc = "" Then
        'we are resetting
SetLoc:
        Open (App.Path & "\PatStmt.ini") For Output As #1
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
        
        Print #1, "Group=-1"
        Print #1, "Provider=-1"
            
        Unload frmBrowse
        Close #1
    Else
        'find and set parameters from .ini
        strLoc = ""
        Open (App.Path & "\PatStmt.ini") For Input As #1
        Do While Not EOF(1)
            Input #1, strTemp
            lngPos = InStr(strTemp, "=")
            If lngPos > 1 Then
                Select Case Left(strTemp, lngPos - 1)
                    Case "Output"
                        strLoc = Right(strTemp, Len(strTemp) - lngPos)
                    Case "Group"
                        strGroup = Right(strTemp, Len(strTemp) - lngPos)
                    Case "Provider"
                        strProvider = Right(strTemp, Len(strTemp) - lngPos)
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
    End If
    
    CONST_OUTPUT_DIR = strLoc
    CONST_GROUP_ID = CLng(IfNull(strGroup, -1))
    CONST_PROVIDER_ID = CLng(IfNull(strProvider, -1))
    
    Exit Sub
ErrTrap:
    Close #1
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub

Private Sub GenerateFile()
'-------------------------------------------------------------------------------
'Author: Duane C Orth
'Date: 02/08/2012
'Description: Main function that generates Email Patient Statments
'Parameters:
'Returns:
'-------------------------------------------------------------------------------

Dim objProvider As ProviderBz.CProviderBZ
Dim objUser As ClinicBz.CUserBz
Dim rst As ADODB.Recordset
Dim rstProvider As ADODB.Recordset
Dim strWhere As String
Dim strProgram As String
Dim dblProgID As Double
Dim intExit As Integer
Dim strSubject As String
Dim strTextBody, strMessage As String
Dim strReportName As String
Dim lngGroupID, lngPrevGroupID, lngProviderID, lngUserID, lngPatientID, lngCnt, lngCtr, lngPageCountPos, lngTotalPageCount As Long
Dim dteStartDate, dteEndDate As Date

    Dim rptPatStmtA As rptPatientStmtA
    Dim rptPatStmtB As rptPatientStmtB
    Dim rptPatStmtC As rptPatientStmtC
    Dim rptPatStmtD As rptPatientStmtD
 '   Dim crxPrintingStatus As CRAXDRT.PrintingStatus
    Dim rpt As Variant

Dim objFS As Scripting.FileSystemObject
Dim objFolder, objFile
Dim objText As Scripting.TextStream

Set objFS = CreateObject("Scripting.FileSystemObject")

Open (CONST_OUTPUT_DIR & "StmtSent" & Format(Now(), "yyyymmdd") & Format(Now(), "ss") & Format(Now(), "nn") & ".log") For Output As #2
Print #2, "Start: " & Format(Now(), "long date") & " " & Format(Now(), "long time") & vbCrLf
    
On Error GoTo ErrTrap:

Screen.MousePointer = vbHourglass
strReportName = "rptPatientStmtC": lngProviderID = 1: lngCnt = 1
Set rpt = New rptPatientStmtC
    
If GetMachineName() = "ADM-01" Then
    rpt.Database.Tables(1).SetLogOnInfo "PsyquelRpl", "PsyquelProd", CONST_PSYQUEL_UA, CONST_PSYQUEL_AC
End If
' rpt.Database.Tables(1).ConnectionProperties.Item("Trusted_Connection") = 0
' rpt.Database.Tables(1).ConnectionProperties.Item("Password") = CONST_PSYQUEL_AC
For lngCtr = 1 To rpt.Database.Tables.Count
    rpt.Database.Tables(lngCtr).SetLogOnInfo CONST_PSYQUEL_DSN, CONST_PSYQUEL_DATABASE, CONST_PSYQUEL_UA, CONST_PSYQUEL_AC
    rpt.Database.Tables(lngCtr).ConnectionProperties.Item("DSN") = CONST_PSYQUEL_DSN
    rpt.Database.Tables(lngCtr).ConnectionProperties.Item("Database") = CONST_PSYQUEL_DATABASE
'   rpt.Database.Tables(lngCtr).ConnectionProperties.Item("Database") = "psyquel_rep"
    rpt.Database.Tables(lngCtr).ConnectionProperties.Item("Password") = CONST_PSYQUEL_AC          'new
    rpt.Database.Tables(lngCtr).ConnectionProperties.Item("Database Password") = CONST_PSYQUEL_AC
Next lngCtr

'Fetch Managers by GroupID
lngUserID = -1
lngGroupID = CONST_GROUP_ID
lngProviderID = CONST_PROVIDER_ID
dteStartDate = CDate("01/01/1900")
dteEndDate = DateAdd("h", 23, DateSerial(Year(Now()), Month(Now), 0))
'   Debug.Print Now() - Day(Now())
'   Debug.Print DateSerial(Year(Now()), Month(Now), 0)
If lngGroupID <> 680 And lngGroupID > 0 And lngProviderID <= 0 Then
   Set objUser = CreateObject("ClinicBz.CUserBz")
   Set rst = objUser.FetchManagersByGroupID(lngGroupID)
   If rst.EOF Then
            lngUserID = lngProviderID
   Else
            lngUserID = rst.Fields("fldUserID").Value
   End If
   Set rst = Nothing
   Set objUser = Nothing
End If
    
Set rst = FetchPatients(lngGroupID, lngProviderID, -1, dteStartDate, dteEndDate, "", "", "N")
    
txtStatus.Text = "Start: " & Format(Now(), "long date") & " " & Format(Now(), "long time") & vbCrLf
DoEvents
If rst.RecordCount > 0 Then barStatus.Max = rst.RecordCount

Do While Not rst.EOF
    lngGroupID = rst.Fields("fldGroupID").Value
    lngProviderID = rst.Fields("fldProviderID").Value
            
    rpt.EnableParameterPrompting = False
    rpt.DiscardSavedData

    rpt.ExportOptions.PDFExportAllPages = True
    rpt.ExportOptions.HTMLEnableSeparatedPages = True
    rpt.ExportOptions.DestinationType = crEDTDiskFile
    rpt.ExportOptions.FormatType = crEFTPortableDocFormat
    
    rpt.ParameterFields(1).ClearCurrentValueAndRange
    If lngGroupID <> 680 And lngGroupID > 0 And lngUserID > 0 Then
        lngProviderID = -1
        rpt.ParameterFields(1).AddCurrentValue CLng(-1) 'ProviderID
        rpt.ParameterFields(2).AddCurrentValue CLng(lngUserID) 'UserID
    Else
        rpt.ParameterFields(1).AddCurrentValue CLng(lngProviderID) 'ProviderID
        rpt.ParameterFields(2).AddCurrentValue CLng(lngProviderID) 'UserID
    End If
    rpt.ParameterFields(3).AddCurrentValue CLng(-1) 'PatientID
    rpt.ParameterFields(4).AddCurrentValue "N" 'InsuranceYN
    rpt.ParameterFields(5).AddCurrentValue "N" 'BalanceYN
    rpt.ParameterFields(6).AddCurrentValue CDate(dteStartDate)
    rpt.ParameterFields(7).AddCurrentValue CDate(dteEndDate)
    rpt.ParameterFields(8).AddCurrentValue "Y" 'BalForwardYN
    rpt.ParameterFields(9).AddCurrentValue "N" 'EmailYN
    rpt.ParameterFields(10).AddCurrentValue "N" 'SummaryYN
    
    With rst
On Error GoTo ErrSkip:
''       While Not .EOF
            'this is the printable version
            Select Case strReportName
                Case "rptPatientStmtA", "rptPatientStmtC", "rptPatientStmtD"
                    rpt.ParameterFields(3).ClearCurrentValueAndRange
                    rpt.ParameterFields(3).AddCurrentValue CLng(!fldPatientID)
                Case "rptPatientStmtB"
                    rpt.ParameterFields(3).ClearCurrentValueAndRange
                    rpt.ParameterFields(3).AddCurrentValue Trim(!PatientLastName)
                    rpt.ParameterFields(4).ClearCurrentValueAndRange
                    rpt.ParameterFields(4).AddCurrentValue Trim(!PatientFirstName)
            End Select
            'find out location of page count variable
            lngPageCountPos = 7
            If strReportName = "rptPatientStmtA" Then lngPageCountPos = 8
            If strReportName = "rptPatientStmtB" Then lngPageCountPos = 9

            rpt.ExportOptions.DiskFileName = CONST_OUTPUT_DIR & "system" & "\" & strReportName & CLng(!fldPatientID) & ".pdf"
            
            'set total page number
            lngTotalPageCount = rpt.PrintingStatus.NumberOfPages
            rpt.FormulaFields(lngPageCountPos).Text = "'" & lngTotalPageCount & "'"
            rpt.Export False
            
            If !fldCCardYN = "Y" And !fldPatOnlinePayYN = "Y" Then
                If Trim(!RPFirstName) > "" And Trim(!RPlastName) > "" Then
                    strMessage = "<BR><p>Dear " & Trim(!RPFirstName) & " " & Trim(!RPlastName) & ",</p><BR><BR>"
                Else
                    strMessage = "<BR><p>Dear " & Trim(!PatientFirstName) & " " & Trim(!PatientLastName) & ",</p><BR><BR>"
                End If
                strMessage = strMessage & "<p>A patient statement is available in your online account (" & Trim(!PatientFirstName) & " " & Trim(!PatientLastName) & "). Please use the link below to make an online payment towards your balance. "
                strMessage = strMessage & "If you have any questions, please call our office at "
                If !fldPhoneBilling > "" Then
                    strMessage = strMessage & "(" & Mid(!fldPhoneBilling, 1, 3) & ")" & Mid(!fldPhoneBilling, 4, 3) & "-" & Mid(!fldPhoneBilling, 7, 4)
                ElseIf !fldPhone1 > "" Then
                    strMessage = strMessage & "(" & Mid(!fldPhone1, 1, 3) & ")" & Mid(!fldPhone1, 4, 3) & "-" & Mid(!fldPhone1, 7, 4)
                ElseIf !fldPhone2 > "" Then
                    strMessage = strMessage & "(" & Mid(!fldPhone2, 1, 3) & ")" & Mid(!fldPhone2, 4, 3) & "-" & Mid(!fldPhone2, 7, 4)
                End If
                strMessage = strMessage & ".  We appreciate your business and have a great day. </p><BR><BR>"
                strMessage = strMessage & "<p>You can make a Credit Card payment at <a href='https://secure.psyquel.com/members/PatientPay.asp?P=K" & Encrypt(rpt.ParameterFields(3).CurrentValue, 20) & "&L=" & Left(!PatientLastName, 1) & "8" & Left(!PatientFirstName, 1) & "'> Online Payment</a>. </p><BR><BR>"
      '     Debug.Print strMessage
     ''           strMessage = strMessage & "<BR><BR><p>As of 06/2016, Primary Care Psychology Associates has transitioned to electronic statements. It is a paperless, efficient and simple way to bill. If you do not wish to receive e-statements in the future or believe your statement is incorrect, please contact our Director of Operations, Adam Kredow at akredow@pcpachicago.com. If no contact is made, you will no longer receive paper statements. Thank you. </p><BR><BR>"
           
                Print #2, "Patient Statement for " & Trim(!PatientFirstName) & " " & Trim(!PatientLastName) & " : " & Trim(!fldEmail) & " : " & !ProviderName & "." & " " & Format(Now(), "long time") '& strMessage & vbCrLf
' !fldEmail
                SendEmailStmt "DoNotReply@psyquel.com", !fldEmail, "Patient Statement for " & Trim(!PatientFirstName) & " " & Trim(!PatientLastName), _
                        strMessage, CONST_OUTPUT_DIR & "system" & "\" & strReportName & CLng(!fldPatientID) & ".pdf"
                UpdateStmtDate (CLng(!fldPatientID))
            Else
                Print #2, "Patient Statement for " & Trim(!PatientFirstName) & " " & Trim(!PatientLastName) & " : " & Trim(!fldEmail) & " : " & !ProviderName & "." & " " & Format(Now(), "long time") '& strMessage & vbCrLf

                SendEmailStmt "DoNotReply@psyquel.com", !fldEmail, "Patient Statement for " & Trim(!PatientFirstName) & " " & Trim(!PatientLastName), _
                        "Patient Statement from " & !ProviderName & ".", _
                        CONST_OUTPUT_DIR & "system" & "\" & strReportName & CLng(!fldPatientID) & ".pdf"
                UpdateStmtDate (CLng(!fldPatientID))
            End If
    
ErrSkip:
            If Dir(CONST_OUTPUT_DIR & "system" & "\" & strReportName & CLng(!fldPatientID) & ".pdf") > "" Then
             '   Kill CONST_OUTPUT_DIR & "system" & "\" & strReportName & CLng(!fldPatientID) & ".pdf"
            End If
            lngPatientID = CLng(!fldPatientID)
            Do While lngPatientID = CLng(!fldPatientID)
                .MoveNext
                If .EOF Then Exit Do
            Loop
'        Wend
    End With
        
   barStatus.Value = lngCnt
   barStatus.Refresh
   lngCnt = lngCnt + 1
   
   lngPrevGroupID = lngGroupID
   lngGroupID = rst.Fields("fldGroupID").Value
   lngProviderID = rst.Fields("fldProviderID").Value
'   If lngGroupID <> 680 And lngGroupID > 0 And lngProviderID > 0 And lngPrevGroupID = lngGroupID Then
'      Do While lngGroupID = rstProvider.Fields("fldGroupID").Value
'         rstProvider.MoveNext
'         If rstProvider.EOF Then
'            Exit Do
'         End If
'      Loop
'   Else
'      rstProvider.MoveNext
'   End If
Loop
  
Print #2, "Finished: " & Format(dteEndDate, "long date") & " " & Format(Now(), "long date") & vbCrLf
Close #2 'Close the file
rst.Close
Set rst = Nothing
Set rstProvider = Nothing
Set objProvider = Nothing
Set rptPatStmtA = Nothing
Set rptPatStmtB = Nothing
Set rptPatStmtC = Nothing
Set rptPatStmtD = Nothing
Screen.MousePointer = vbDefault
    
Exit Sub
    
ErrTrap:
    Print #2, "Error:" & Err.Description & vbLf & vbLf & "Error Number: " & Err.Number & vbCrLf & txtStatus.Text & vbCrLf
    Close #2 'Close the file
    txtStatus.Text = "Error:" & Err.Description & vbLf & vbLf & "Error Number: " & Err.Number & vbCrLf & txtStatus.Text
  '  Debug.Print txtStatus.Text & strSubject
    DoEvents
  '  If blnEmailFailure Then
        Call SendEmailMessage("QWareAdmin@Psyquel.com", GetMachineName() & "@psyquel.com", "Electronic Email Failure - SendEmailStmt", "The Electronic Email Daemon reported a failure in " & Err.Source & "." & vbCrLf & "Unhandled Error: " & Err.Number & ": " & Err.Description & vbCrLf & Me.txtStatus.Text)
  '  End If
  '  Call ShowError(Err)
    Set rstProvider = Nothing
    Set objProvider = Nothing
    Set rptPatStmtA = Nothing
    Set rptPatStmtB = Nothing
    Set rptPatStmtC = Nothing
    Set rptPatStmtD = Nothing
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
    Dim objMail As CDO.Message
    'Dim strRecipient As String
    Dim strMachineName As String
    
    On Error GoTo ErrHand
       
    Set objMail = CreateObject("CDO.Message")
    objMail.From = strSender
    objMail.To = strRecipient
    objMail.Subject = strSubject
    objMail.TextBody = strMessage
    objMail.Send
    Set objMail = Nothing
    
    Wait 10
    
    Exit Sub
    
ErrHand:
    Err.Raise Err.Number, Err.Source, Err.Description
    If blnEmailFailure Then
        Call SendEmailMessage("QWareAdmin@Psyquel.com", GetMachineName() & "@psyquel.com", "Electronic Email Failure - SendEmailStmt", "The Electronic Email Daemon reported a failure in " & Err.Source & "." & vbCrLf & "Unhandled Error: " & Err.Number & ": " & Err.Description & vbCrLf & Me.txtStatus.Text)
    End If
    Set objMail = Nothing
    Screen.MousePointer = vbDefault
    Unload Me
End Sub

Private Sub SendEmailStmt(ByVal strSender As String, _
                        ByVal strRecipient As String, _
                        ByVal strSubject As String, _
                        ByVal strMessage As String, _
                        ByVal strAttachment As String)
    
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
    objMail.HTMLBody = "<html><body><p>" & strMessage & "</p></body></html>"
    If Dir(strAttachment) > "" Then
        objMail.AddAttachment Trim(strAttachment)
    End If
    objMail.Send
    Set objMail = Nothing
    
    Wait 2
    
    Exit Sub
ErrHand:
    Err.Raise Err.Number, Err.Source, Err.Description & ":" & strSender & ":" & strRecipient & ":" & strSubject
    If blnEmailFailure Then
        Call SendEmailMessage("QWareAdmin@Psyquel.com", GetMachineName() & "@psyquel.com", "Electronic Email Failure - SendEmailStmt", "The Electronic Email Daemon reported a failure in " & Err.Source & "." & vbCrLf & "Unhandled Error: " & Err.Number & ": " & Err.Description & vbCrLf & Me.txtStatus.Text)
    End If
    Set objMail = Nothing
    Screen.MousePointer = vbDefault
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
    Screen.MousePointer = vbDefault
    Unload Me
    
End Function

Public Function FetchPatients(ByVal lngGroupID As Long, ByVal lngProviderID As Long, ByVal lngPatientID As Long, _
                    ByVal dteStartDate As Date, ByVal dteEndDate As Date, _
                    ByVal strLastName As String, ByVal strFirstName As String, _
                    Optional blnIncludeZeroBalance As String = "N") As ADODB.Recordset

    Dim cnnSQL As ADODB.Connection
    Dim rstSQL As ADODB.Recordset
    Dim objUserRole As ClinicBz.CUserBz
    Dim rstUserRole As ADODB.Recordset
    Dim strConcatWhere As String
    Dim strSQL As String

    On Error GoTo ErrTrap:

    Dim strSearchSqlA As String
    Dim strSearchSqlF As String
    Dim strProvider As String
    Dim strDateSQLA As String
    Dim strDateSQLB As String
    Dim intRoleID As Integer
     
    'Set up search criteria
    strDateSQLA = " (A.fldReceiveDate Between CONVERT(DATETIME, '" & Format(dteStartDate, "mm/dd/yyyy") & " 00:00:00', 102) AND CONVERT(DATETIME, '" & Format(dteEndDate, "mm/dd/yyyy") & " 23:59:59', 102) ) "
    strDateSQLB = " (A.fldReceiveDate < CONVERT(DATETIME, '" & Format(dteStartDate, "mm/dd/yyyy") & " 00:00:00', 102) ) "

    If strLastName > "" Or strFirstName > "" Then
        strSearchSqlA = strSearchSqlA & " AND B.fldLast like '" & strLastName & "*'" & " AND B.fldFirst like '" & strFirstName & "*'"
    End If
    
    'If only one patient is requested
    If lngPatientID > 0 Then
        strSearchSqlA = strSearchSqlA & " AND (A.fldPatientID = " & lngPatientID & ") "
    End If
        
    'If an office manager is specified, return information for all providers they service
    If lngProviderID > 0 Then
        strProvider = " AND (fldProviderID = " & lngProviderID & ") "
        strSearchSqlA = strSearchSqlA & " AND (A.fldProviderID = " & lngProviderID & ") "
    ElseIf lngGroupID > 0 And lngGroupID <> 680 Then
        strProvider = ""
        strSearchSqlA = strSearchSqlA & " AND (D.fldGroupID = " & lngGroupID & ") "
    End If

    'Determine whether to include 'zero balance' records'
    If blnIncludeZeroBalance = "N" Then
        strSearchSqlA = strSearchSqlA & _
            " AND ((SELECT ISNULL(SUM(fldAmount), 0) " & _
            "       From tblPatientTx " & _
            "       WHERE fldPatientID = A.fldPatientID AND fldTxTypeCode = 'C' AND fldDisabledYN = 'N' AND " & strDateSQLA & strProvider & ") - " & _
            "       (SELECT ISNULL(SUM(fldAmount), 0) " & _
            "        From tblPatientTx " & _
            "        WHERE fldPatientID = A.fldPatientID AND fldTxTypeCode = 'P' AND fldDisabledYN = 'N' AND " & strDateSQLA & strProvider & ") + " & _
            "       (SELECT ISNULL(SUM(fldAmount), 0) " & _
            "        From tblPatientTx  " & _
            "        WHERE fldPatientID = A.fldPatientID AND fldTxTypeCode = 'A' AND fldDisabledYN = 'N' AND " & strDateSQLA & strProvider & ") > 0) "
            
 '           "       WHERE fldRPID = A.fldRPID AND fldPatientID = A.fldPatientID AND fldTxTypeCode = 'C' AND " & strDateSQLA & strProvider & ") - " & _
 '           "       (SELECT ISNULL(SUM(fldAmount), 0) " & _
 '           "        From tblPatientTx " & _
 '           "        WHERE fldRPID = A.fldRPID AND fldPatientID = A.fldPatientID AND fldTxTypeCode = 'P' AND " & strDateSQLA & strProvider & ") + " & _
 '           "       (SELECT ISNULL(SUM(fldAmount), 0) " & _
 '           "        From tblPatientTx  " & _
 '           "        WHERE fldRPID = A.fldRPID AND fldPatientID = A.fldPatientID AND fldTxTypeCode = 'A' AND " & strDateSQLA & strProvider & ") > 0) "
    End If
    
    'Prepare the SQL statement.
    strSQL = "SELECT A.fldPatientID, A.fldProviderID, B.fldEmail, C.fldEmail AS ProviderEmail, " & _
            "C.fldFirstName + ' ' + C.fldLastName + ', ' + F.fldProviderType AS ProviderName, " & _
            "B.fldLast AS PatientLastName, B.fldFirst AS PatientFirstName, D.fldCCardYN, D.fldPatOnlinePayYN, D.fldPhone1, D.fldPhone2, D.fldPhoneBilling, " & _
            "R.fldLast AS RPlastName, R.fldFirst AS RPFirstName, R.fldEmail AS RPEmail, D.fldGroupID, A.fldProviderID " & _
            "FROM tblPatientTx A INNER JOIN  " & _
            "tblBenefactor B ON A.fldPatientID = B.fldBenefactorID INNER JOIN " & _
            "tblBenefactor R ON A.fldRpID = R.fldBenefactorID INNER JOIN " & _
            "tblUser C ON A.fldProviderID = C.fldUserID  INNER JOIN " & _
            "tblProvider D ON A.fldProviderID = D.fldProviderID INNER JOIN " & _
            "tblProviderType F ON D.fldDegreeCredential = F.fldProviderTypeID INNER JOIN " & _
            "tblPatientProvider AS E ON A.fldPatientID = E.fldPatientID AND A.fldProviderID = E.fldProviderID " & _
            "Where " & strDateSQLA & " AND " & _
            "(B.fldEmail > '') AND (B.fldEmailYN <> 'N') AND (B.fldPrintPatientStmtYN <> 'N') AND " & _
            "(B.fldStmtLastSentDate Is Null Or B.fldStmtLastSentDate < GETDATE()-15) AND " & _
            "(C.fldEmail > '') AND (D.fldPatStmtYNE = 'E')AND (C.fldEmail > '') AND " & _
            "(E.fldDisabledYN <> 'Y') " & _
            strSearchSqlA & _
            "Group By " & _
            "A.fldPatientID, A.fldProviderID, B.fldEmail, C.fldEmail, C.fldFirstName + ' ' + C.fldLastName + ', ' + F.fldProviderType,  " & _
            "B.fldLast, B.fldFirst, D.fldCCardYN, D.fldPatOnlinePayYN, D.fldPhone1, D.fldPhone2, D.fldPhoneBilling, " & _
            "R.fldLast, R.fldFirst, R.fldEmail, D.fldGroupID, A.fldProviderID " & _
            "ORDER BY A.fldProviderID, B.fldLast, B.fldFirst, A.fldPatientID "
    'Debug.Print strSQL
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open(CONST_PSYQUEL_CNN)
    
    'Instantiate and populate the Recordset.
    Set rstSQL = New ADODB.Recordset
    rstSQL.CursorLocation = adUseClient
    Call rstSQL.Open(strSQL, cnnSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)
    
    'Disconnect the recordset, close the connection and return the recordset
    'to the calling environment.
    Set rstSQL.ActiveConnection = Nothing
    
    cnnSQL.Close
    Set cnnSQL = Nothing
    Set FetchPatients = rstSQL
    
    'Signal successful completion
'    GetObjectContext.SetComplete

    Exit Function
    
ErrTrap:
    Set cnnSQL = Nothing
    'Signal incompletion and raise the error to the calling environment.
 '   GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description & " intRoleID:" & intRoleID & " strProvider:" & strProvider & " strSearchSqlA:" & strSearchSqlA
    Screen.MousePointer = vbDefault
    Unload Me

End Function
Public Sub UpdateStmtDate(ByVal lngPatientID As Long)
    
    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command
    Dim strSQL As String

    On Error GoTo ErrTrap:

    'Prepare the SQL statement
    strSQL = "UPDATE tblBenefactor "
    strSQL = strSQL & " SET "
    strSQL = strSQL & " fldStmtLastSentDate = GETDATE() "
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

End Sub
Public Function Encrypt(Name As String, Key As Long) As String

Dim v As Long, c1 As String, z As String

For v = 1 To Len(Name)

 c1 = Asc(Mid(Name, v, 1))

   c1 = Chr(c1 + Key)
  
 z = z & c1

Next v

Encrypt = z

End Function
Public Function Decrypt(Name As String, Key As Long) As String

Dim v As Long, c1 As String, z As String

For v = 1 To Len(Name)

  c1 = Asc(Mid(Name, v, 1))

   c1 = Chr(c1 - Key)

  z = z & c1

Next v

Decrypt = z

End Function
