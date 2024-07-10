VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "Comdlg32.ocx"
Begin VB.Form frmECInterpretMain 
   Caption         =   "E-Claim Response Interpreter"
   ClientHeight    =   2625
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   4545
   LinkTopic       =   "Form1"
   LockControls    =   -1  'True
   ScaleHeight     =   2625
   ScaleWidth      =   4545
   StartUpPosition =   1  'CenterOwner
   Begin VB.CommandButton cmdDone 
      Caption         =   "&Cancel"
      Height          =   420
      Index           =   2
      Left            =   3195
      TabIndex        =   8
      Top             =   2070
      Width           =   1230
   End
   Begin VB.CommandButton cmdDone 
      Caption         =   "&Print"
      Enabled         =   0   'False
      Height          =   420
      Index           =   1
      Left            =   1710
      TabIndex        =   7
      Top             =   2070
      Width           =   1230
   End
   Begin VB.CommandButton cmdDone 
      Caption         =   "&OK"
      Enabled         =   0   'False
      Height          =   420
      Index           =   0
      Left            =   180
      TabIndex        =   6
      Top             =   2070
      Width           =   1230
   End
   Begin VB.CommandButton cmdABC 
      Caption         =   "..."
      Height          =   330
      Index           =   1
      Left            =   4005
      TabIndex        =   3
      Top             =   1305
      Width           =   330
   End
   Begin VB.TextBox txtDest 
      BackColor       =   &H8000000F&
      Height          =   330
      Left            =   225
      Locked          =   -1  'True
      TabIndex        =   2
      Top             =   1305
      Width           =   3705
   End
   Begin VB.CommandButton cmdABC 
      Caption         =   "..."
      Height          =   330
      Index           =   0
      Left            =   4005
      TabIndex        =   1
      Top             =   495
      Width           =   330
   End
   Begin VB.TextBox txtSource 
      BackColor       =   &H8000000F&
      Height          =   330
      Left            =   225
      Locked          =   -1  'True
      TabIndex        =   0
      Top             =   495
      Width           =   3705
   End
   Begin MSComDlg.CommonDialog dlgBuild 
      Left            =   0
      Top             =   1575
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
   Begin VB.Label Label1 
      Caption         =   "Output directory"
      Height          =   255
      Left            =   225
      TabIndex        =   5
      Top             =   1035
      Width           =   3705
   End
   Begin VB.Label Label6 
      Caption         =   "Datafile to interpret"
      Height          =   255
      Left            =   225
      TabIndex        =   4
      Top             =   225
      Width           =   3705
   End
End
Attribute VB_Name = "frmECInterpretMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
Private Sub cmdABC_Click(Index As Integer)
    Select Case Index
        Case 0
            'source file
            dlgBuild.ShowOpen
            If dlgBuild.FileName > "" Then txtSource.Text = dlgBuild.FileName
        Case 1
            'dest dir
            frmBrowse.Show vbModal
            txtDest.Text = frmBrowse.dirList.Path
    End Select
    
    If txtSource.Text > "" And txtDest.Text > "" Then cmdDone(0).Enabled = True Else cmdDone(0).Enabled = False
End Sub
Private Sub cmdDone_Click(Index As Integer)
    Select Case Index
        Case 0
            ProcessInterpretation
        Case 1
            PrintInterpretation
        Case 2
            Unload Me
    End Select
End Sub
Private Sub ProcessInterpretation()
    Dim strBuffer As String
    Dim strTemp As String
    Dim varParsed As Variant
    
    On Error GoTo ErrHand
    
    Open (txtSource.Text) For Input As #1
    Input #1, strBuffer
    Input #1, strBuffer
    Input #1, strBuffer
    
    varParsed = Split(strBuffer, "*")
    If varParsed(1) <> "835" Then
        MsgBox "Format not supported.", vbCritical, "File Error"
        Close #1
        Exit Sub
    End If
    
    strTemp = txtDest.Text & "\" & Hex(varParsed(2)) & ".txt"
    'if dir, kill
    If Dir(strTemp) > "" Then Kill strTemp
    
    Open (strTemp) For Output As #2
    Print #2, "Electronic Status Report - #" & varParsed(2)
    
    Do While Not EOF(1)
        Input #1, strBuffer
        varParsed = Split(strBuffer, "*")
        
        Select Case varParsed(0)
            Case ""
        End Select
            
    Loop
    
    Close #1
    Close #2
    Exit Sub
ErrHand:
    Close #1
    Close #2
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub
Private Sub PrintInterpretation()

End Sub

