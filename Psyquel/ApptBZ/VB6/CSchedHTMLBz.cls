VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 2  'RequiresTransaction
END
Attribute VB_Name = "CSchedHTMLBz"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CPatApptBz                                             '
'Date: 08/29/2000                                                   '
'Author: Rick "Boom Boom" Segura                                    '
'Description:  MTS business object designed to call methods         '
'              associated with the CSchedHTMLDB class.              '
'--------------------------------------------------------------------

Const ELMNT_CELL_ID As Integer = 0
Const ELMNT_APPT_IDS As Integer = 1
Const ELMNT_PARENT_ID As Integer = 2
Const ELMNT_APPT_TYPE As Integer = 3
Const ELMNT_DURATION As Integer = 4
Const ELMNT_DESCRIPTION As Integer = 5
Const ELMNT_COLSPAN As Integer = 6
Const ELMNT_STATUS As Integer = 7

'** PUBLIC FUNCTIONS ************************************************

Public Function BuildSchedule(ByVal lngProviderID As Long, _
                              ByVal dteDate As Date) As String
'--------------------------------------------------------------------
'Date: 09/25/2000                                                   '
'Author: Rick "Boom Boom" Segura                                    '
'Description:  Generates the HTML code that produces the schedule   '
'               web table                                           '
'Parameters: lngProviderID - ID of provider whose schedule is being '
'               produced                                            '
'            DTEDate - Day of week within the week of schedule to   '
'               produce                                             '
'Returns: String of HTML code                                       '
'--------------------------------------------------------------------
'Revision History:                                                  '
'--------------------------------------------------------------------
    Dim objAppt As ApptDB.CApptDB
    Dim rst As ADODB.Recordset
    Dim dteStart As Date
    Dim dteEnd As Date

    ' Calculate the week date range
    dteStart = GetSundayDate(dteDate)
    dteEnd = DateAdd("d", 6, dteStart)

    Set objAppt = CreateObject("ApptDB.CApptDB")
    Set rst = objAppt.FetchByProviderDateRangeHTML(lngProviderID, dteStart, DateAdd("d", 1, dteEnd))
    
    
    BuildSchedule = BuildSchedHTML(rst, dteStart)

    
    Set rst = Nothing
    Set objAppt = Nothing
    GetObjectContext.SetComplete
    
    Exit Function
    
Err_Trap:
    'Signal incompletion and raise the error to the calling environment.
    Set rst = Nothing
    Set objAppt = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function

'** PRIVATE FUNCTIONS ***********************************************

Private Function GetSundayDate(dteDate)
'--------------------------------------------------------------------
'Date: 09/25/2000                                                   '
'Author: Rick "Boom Boom" Segura                                    '
'Description:  Calculates the prior Sunday of given date            '
'Parameters: dteDate - Date to be used relative to the Sunday being sought
'Returns: Date of the 1st prior Sunday                              '
'--------------------------------------------------------------------
'Revision History:                                                  '
'--------------------------------------------------------------------
    Dim lngOffset
    
    lngOffset = DatePart("w", dteDate) - 1
    
    GetSundayDate = DateAdd("d", lngOffset * -1, dteDate)
    
End Function

Private Function BuildSchedHTML(ByRef rstSched As ADODB.Recordset, ByVal dteStart As Date) As String
    
    Dim arySched(6, 59, 7) As Variant
    Dim i As Integer, j As Integer
    Dim dteStartDateTime As Date
    Dim strCellID As String
    
    On Error GoTo Err_Trap
    
    BuildSchedHTML = ""
    dteStartDateTime = CDate(CStr(dteStart) & " 7:00:00 AM")
    
    Call InitializeArray(arySched, dteStartDateTime)
    Call FillArray(arySched, rstSched, dteStartDateTime)
    
    
    For j = 0 To 59
        BuildSchedHTML = BuildSchedHTML & "<TR>" & vbCrLf
        
        For i = 0 To 6
            If i = 0 Then
                BuildSchedHTML = BuildSchedHTML & vbTab & "<TD>" & Format(arySched(i, j, ELMNT_CELL_ID), "Medium Time") & "</TD>" & vbCrLf
            End If
            
            If IsDrawn(arySched(i, j, ELMNT_PARENT_ID)) Then
            
                If IsNull(arySched(i, j, ELMNT_PARENT_ID)) Then
                    strCellID = Replace(Replace(arySched(i, j, ELMNT_CELL_ID), " ", "|", 1, 1), " ", "")
                Else
                    strCellID = arySched(i, j, ELMNT_APPT_IDS)
                End If
            
                BuildSchedHTML = BuildSchedHTML & vbTab & "<TD ALIGN='CENTER' "
                BuildSchedHTML = BuildSchedHTML & " ROWSPAN=" & arySched(i, j, ELMNT_COLSPAN) & " "
                BuildSchedHTML = BuildSchedHTML & " ID=" & strCellID & " "
                BuildSchedHTML = BuildSchedHTML & " onmouseover=this.style.cursor='hand'; "
                BuildSchedHTML = BuildSchedHTML & " onmouseout=this.style.cursor='default'; "
                BuildSchedHTML = BuildSchedHTML & " BGCOLOR=" & GetBKGRDColor(arySched(i, j, ELMNT_APPT_TYPE), arySched(i, j, ELMNT_STATUS)) & " "
                'ONCLICK
                BuildSchedHTML = BuildSchedHTML & ">"
                BuildSchedHTML = BuildSchedHTML & arySched(i, j, ELMNT_DESCRIPTION)
                BuildSchedHTML = BuildSchedHTML & "</TD>" & vbCrLf
                
            End If
            
        Next
         BuildSchedHTML = BuildSchedHTML & "</TR>" & vbCrLf
    Next
    
    Exit Function

Err_Trap:
    MsgBox Err.Description
End Function

Private Sub InitializeArray(ByRef ary As Variant, ByVal dteDate As Date)
'--------------------------------------------------------------------
'Date: 09/26/2000                                                   '
'Author: Rick "Boom Boom" Segura                                    '
'Description:  Initializes some protions of the schedule array to   '
'               default values                                      '
'Parameters: lngProviderID - ID of provider whose schedule is being '
'               produced                                            '
'            DTEDate - Day of week within the week of schedule to   '
'               produce                                             '
'Returns: String of HTML code                                       '
'--------------------------------------------------------------------
'Revision History:                                                  '
'--------------------------------------------------------------------
    
    Dim i As Integer, j As Integer
    Dim dteWrite As Date, dteTemp As Date
    
    On Error GoTo Err_Trap
    
    dteTemp = dteDate
    
    For i = 0 To 6
        dteWrite = dteTemp
        For j = 0 To 59
            ary(i, j, ELMNT_CELL_ID) = dteWrite
            ary(i, j, ELMNT_APPT_IDS) = Null
            ary(i, j, ELMNT_APPT_TYPE) = 0
            ary(i, j, ELMNT_DURATION) = 15
            ary(i, j, ELMNT_COLSPAN) = 1
            ary(i, j, ELMNT_PARENT_ID) = Null
            ary(i, j, ELMNT_DESCRIPTION) = ""
            ary(i, j, ELMNT_STATUS) = Null
            
            dteWrite = DateAdd("n", 15, dteWrite)
        Next
        dteTemp = DateAdd("d", 1, dteTemp)
    Next
    
    Exit Sub

Err_Trap:
    MsgBox Err.Description
End Sub

Private Sub FillArray(ByRef arySched As Variant, ByRef rstSched As ADODB.Recordset, ByVal dteStart As Date)
    
    Dim intDay As Integer
    Dim intTime As Integer
    Dim strDesritpion As String
    
    On Error GoTo Err_Trap
    
    With rstSched
        Call MapDateTime(intDay, intTime, dteStart, .Fields("fldStartDateTime"))
        
        Select Case .Fields("fldCategoryID")
            Case 1  ' *** Patient Appointment  ***
                strDescription = .Fields("fldFirst") & " " & .Fields("fldLast")
                If .Fields("fldApptStatusID") <> 4 Then
                    Call FillCell(intDay, intTime, .Fields("fldApptID"), _
                        .Fields("fldCategoryID"), .Fields("fldStartDateTime"), _
                        .Fields("fldDuration"), strDescription, arySched)
                End If
            Case 2  ' *** Block Appointment  ***
            
            Case 3  ' *** Hold Appointment  ***
            
        End Select
        
        .MoveNext
    End With
    
    Exit Sub

Err_Trap:
    MsgBox Err.Description
End Sub

Private Sub FillCell(ByVal intDay As Integer, ByVal intTime As Integer, _
                     ByVal lngApptID As Long, ByVal lngApptCategoryID, _
                     ByVal dteStart As Date, ByVal lngDuration As Long, _
                     ByVal strDescription As String, ByRef arySched As Variant)
    Dim lngSpan As Long
    Dim intCtr As Integer
    Dim intTemp As Integer
    
    If IsNull(arySched(intDay, intTime, ELMNT_PARENT_ID)) Then
    
        lngSpan = lngDuration / 15
        
        arySched(intDay, intTime, ELMNT_APPT_IDS) = lngApptID
        arySched(intDay, intTime, ELMNT_PARENT_ID) = -1
        arySched(intDay, intTime, ELMNT_APPT_TYPE) = lngApptCategoryID
        arySched(intDay, intTime, ELMNT_DURATION) = lngDuration
        arySched(intDay, intTime, ELMNT_DESCRIPTION) = strDescription
        arySched(intDay, intTime, ELMNT_COLSPAN) = lngSpan
        
        intTemp = intTime
        For intCtr = 2 To (lngSpan - 1)
            arySched(intDay, intTemp, ELMNT_PARENT_ID) = intTime
            intTemp = intTemp + 1
        Next

    End If
    
End Sub

Private Sub MapDateTime(ByRef intDay As Integer, ByRef intTime As Integer, _
                             ByVal dteSunday As Date, ByVal dteDateTime As Date)
    Dim dteStartTime As Date, dteEndTime As Date
'--------------------------------------------------------------------
'Date: 09/26/2000                                                   '
'Author: Rick "Boom Boom" Segura                                    '
'Description:  Maps an appointment date and time to a cell in the   '
'               schedule array                                      '
'Parameters: intDay - The Day(Column) value of the cell             '
'            intTime - The Time(Row) value of the cell              '
'            dteSunday - Date of first Sunday of the week           '
'            dteDatTime - Date/Time value being mapped              '
'Returns: Nothing...parameters are passed by reference              '
'--------------------------------------------------------------------
'Revision History:                                                  '
'--------------------------------------------------------------------

    On Error GoTo Err_Trap
    
    dteStartTime = Format(dteSunday, "Short Time")
    dteEndTime = Format(dteDateTime, "Short Time")
    
    intDay = DateDiff("d", dteSunday, dteDateTime)
    intTime = DateDiff("n", dteStartTime, dteEndTime) / 15
    
    Exit Sub

Err_Trap:
    MsgBox Err.Description
End Sub

Private Function GetBKGRDColor(ByVal ApptType As Variant, ByVal Status As Variant) As String
    
    Select Case ApptType
        Case 0  ' Open Slot
            GetBKGRDColor = "'WHITE'"
            
        Case 1  ' Patient Appt
            Select Case Status
                Case 1
                    GetBKGRDColor = "#6495ED"
                Case 2
                    GetBKGRDColor = "#87CEEB"
                Case 3
                    GetBKGRDColor = "#D3D3D3"
                Case Else
                
            End Select
            
        Case 2  ' Blocked Appt
            GetBKGRDColor = "'DARKBLUE'"
            
        Case 3  ' Held Appt
            'GetBKGRDColor = "'DARKBLUE'"
            
        Case 4  ' Conflict Appt
            'GetBKGRDColor = "'DARKBLUE'"
            
    End Select
    
End Function

Private Function IsDrawn(ByVal varTest As Variant) As Boolean
     
     IsDrawn = True
     
     If IsNull(varTest) Then
        Exit Function
     ElseIf varTest > 0 Then
        Exit Function
     End If
     
    IsDrawn = False
End Function
