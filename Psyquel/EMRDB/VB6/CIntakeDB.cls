VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CIntakeDB"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CIntakeDB
'Date: 05/25/2020
'Author: Duane C Orth
'Description:  Database object designed to implement methods (by calling
'              stored procedures) associated with
'              data affecting the tblIntake table.
'--------------------------------------------------------------------
'  Revisions:
'--------------------------------------------------------------------
Option Explicit

Private Const CLASS_NAME As String = "CIntakeDB"
Private Const TABLE_NAME As String = "tblIntake"

Public Function FetchByID(ByVal lngIntakeID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 05/25/2020
'Author: Duane C Orth
'Description: Retrieves records from the tblIntake table.  Calls
'               stored procedure uspSelIntakeByID.
'Parameters: lngIntakeID - 'Where' clause of the SQL statement by
'               which records will be fetched.  Matches tblIntake's
'               primary key.
'             lngUserID - UserID of the user performing the fetch.  Not
'               really used currently.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection ' Declare DB connection
    Dim rst As ADODB.Recordset  ' Declare recordset
    Dim cmd As ADODB.Command    ' Declare SQL statement

    On Error GoTo ErrTrap:

    Set cmd = New ADODB.Command     ' Instantiate SQL statement --R001
    With cmd
        .CommandText = "uspSelIntakeByID"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@IntakeID", adInteger, adParamInput, , lngIntakeID)
    End With
    
    Set cnn = New ADODB.Connection  ' Instantiate DB connection
    cnn.Open (CONST_PSYQUEL_CNN) ' open DB connection
    Set cmd.ActiveConnection = cnn
    
    Set rst = New ADODB.Recordset   ' Instantiate recordset
    rst.CursorLocation = adUseClient
    rst.Open cmd, , adOpenForwardOnly, adLockReadOnly   ' open recordset

    Set cmd.ActiveConnection = Nothing
    Set rst.ActiveConnection = Nothing
    cnn.Close   ' close DB connection
    Set cnn = Nothing
    Set cmd = Nothing
    
    Set FetchByID = rst ' Set return value
   
    GetObjectContext.SetComplete
    
    Exit Function
   
ErrTrap:
    GetObjectContext.SetAbort
    
    Set cmd.ActiveConnection = Nothing
    Set rst.ActiveConnection = Nothing
    rst.Close   ' close recordset
    cnn.Close   ' close DB connection
    
    ' De-allocate space for objects
    Set rst = Nothing
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Function FetchByPatient(ByVal lngPatientID As Long, Optional ByVal lngTemplateID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 05/25/2020
'Author: Duane C Orth
'Description: Retrieves records from the tblIntake table.  Calls
'               stored procedure uspSelIntakeByPatient.
'Parameters: lngPatientID - 'Where' clause of the SQL statement by
'               which records will be fetched.
'            lngUserID - UserID of the user performing the fetch.  Not
'               really used currently.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection ' Declare DB connection
    Dim rst As ADODB.Recordset  ' Declare recordset
    Dim cmd As ADODB.Command    ' Declare SQL statement

    On Error GoTo ErrTrap:
    
    Set cmd = New ADODB.Command     ' Instantiate SQL statement --R001
    With cmd
        .CommandText = "uspSelIntakeByPatient"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
        .Parameters.Append .CreateParameter("@TemplateID", adInteger, adParamInput, , lngTemplateID)
    End With
    
    Set cnn = New ADODB.Connection  ' Instantiate DB connection
    cnn.Open (CONST_PSYQUEL_CNN) ' open DB connection
    Set cmd.ActiveConnection = cnn
    
    Set rst = New ADODB.Recordset   ' Instantiate recordset
    rst.CursorLocation = adUseClient
    rst.Open cmd, , adOpenForwardOnly, adLockReadOnly   ' open recordset

    Set cmd.ActiveConnection = Nothing
    Set rst.ActiveConnection = Nothing
    cnn.Close   ' close DB connection
    Set cnn = Nothing
    Set cmd = Nothing
    
    Set FetchByPatient = rst ' Set return value
   
    GetObjectContext.SetComplete
    
    Exit Function
   
ErrTrap:
    GetObjectContext.SetAbort
    
    Set cmd.ActiveConnection = Nothing  ' --R001
    Set rst.ActiveConnection = Nothing  ' --R001
    rst.Close   ' close recordset
    cnn.Close   ' close DB connection
    
    Set rst = Nothing
    Set cmd = Nothing
    Set cnn = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Function
Public Function Insert(ByVal lngPatientID As Long, ByVal lngTemplateID As Long, ByVal lngProviderID As Long, _
                       ByVal lngClinicID As Long, ByVal strNoteTitle As String, ByVal dtNoteDate As Date, _
                       ByVal strChar_1 As String, ByVal strChar_2 As String, ByVal strChar_3 As String, ByVal strChar_4 As String, _
                       ByVal strChar_5 As String, ByVal strChar_6 As String, ByVal strChar_7 As String, ByVal strChar_8 As String, _
                       ByVal strChar_9 As String, ByVal strChar_10 As String, ByVal strChar_11 As String, ByVal strChar_12 As String, _
                       ByVal strChar_13 As String, ByVal strChar_14 As String, ByVal strChar_15 As String, ByVal strChar_16 As String, _
                       ByVal strChar_17 As String, ByVal strChar_18 As String, ByVal strChar_19 As String, ByVal strChar_20 As String, _
                       ByVal dtDate_1 As Date, ByVal dtDate_2 As Date, ByVal dtDate_3 As Date, ByVal dtDate_4 As Date, _
                       ByVal dtDate_5 As Date, ByVal dtDate_6 As Date, ByVal strText_1 As String, ByVal strText_2 As String, _
                       ByVal strNoteText As String, ByVal blnLockedYN As Boolean, ByVal lngSignedBy As Long, _
                       ByVal dtDateSigned As Date, ByVal strUserName As String) As Long
'--------------------------------------------------------------------
'Date: 05/25/2020
'Author: Duane C Orth
'Description: Inserts records into the tblIntake table.  Calls
'               stored procedure uspInsIntake.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'Returns: Long
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection ' Declare DB connection
    Dim cmd As ADODB.Command    ' Declare SQL statement

    On Error GoTo ErrTrap:

    ' Instantiate the objects
    Set cmd = New ADODB.Command     ' Instantiate SQL statement
    
    'Create the parameter objects
    With cmd
        .CommandText = "uspInsIntake"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@IntakeID", adInteger, adParamReturnValue)
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
        .Parameters.Append .CreateParameter("@TemplateID", adInteger, adParamInput, , lngTemplateID)
        .Parameters.Append .CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID)
        .Parameters.Append .CreateParameter("@ClinicID", adInteger, adParamInput, , IIf(lngClinicID <= 0, Null, lngClinicID))
        .Parameters.Append .CreateParameter("@NoteTitle", adVarChar, adParamInput, 128, Left(strNoteTitle, 128))
        .Parameters.Append .CreateParameter("@NoteDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtNoteDate) = 0, Now(), dtNoteDate))
        .Parameters.Append .CreateParameter("@Char_1", adVarChar, adParamInput, 128, Left(strChar_1, 128))
        .Parameters.Append .CreateParameter("@Char_2", adVarChar, adParamInput, 128, Left(strChar_2, 128))
        .Parameters.Append .CreateParameter("@Char_3", adVarChar, adParamInput, 128, Left(strChar_3, 128))
        .Parameters.Append .CreateParameter("@Char_4", adVarChar, adParamInput, 128, Left(strChar_4, 128))
        .Parameters.Append .CreateParameter("@Char_5", adVarChar, adParamInput, 128, Left(strChar_5, 128))
        .Parameters.Append .CreateParameter("@Char_6", adVarChar, adParamInput, 128, Left(strChar_6, 128))
        .Parameters.Append .CreateParameter("@Char_7", adVarChar, adParamInput, 128, Left(strChar_7, 128))
        .Parameters.Append .CreateParameter("@Char_8", adVarChar, adParamInput, 128, Left(strChar_8, 128))
        .Parameters.Append .CreateParameter("@Char_9", adVarChar, adParamInput, 128, Left(strChar_9, 128))
        .Parameters.Append .CreateParameter("@Char_10", adVarChar, adParamInput, 128, Left(strChar_10, 128))
        .Parameters.Append .CreateParameter("@Char_11", adVarChar, adParamInput, 128, Left(strChar_11, 128))
        .Parameters.Append .CreateParameter("@Char_12", adVarChar, adParamInput, 128, Left(strChar_12, 128))
        .Parameters.Append .CreateParameter("@Char_13", adVarChar, adParamInput, 128, Left(strChar_13, 128))
        .Parameters.Append .CreateParameter("@Char_14", adVarChar, adParamInput, 128, Left(strChar_14, 128))
        .Parameters.Append .CreateParameter("@Char_15", adVarChar, adParamInput, 128, Left(strChar_15, 128))
        .Parameters.Append .CreateParameter("@Char_16", adVarChar, adParamInput, 128, Left(strChar_16, 128))
        .Parameters.Append .CreateParameter("@Char_17", adVarChar, adParamInput, 128, Left(strChar_17, 128))
        .Parameters.Append .CreateParameter("@Char_18", adVarChar, adParamInput, 128, Left(strChar_18, 128))
        .Parameters.Append .CreateParameter("@Char_19", adVarChar, adParamInput, 128, Left(strChar_19, 128))
        .Parameters.Append .CreateParameter("@Char_20", adVarChar, adParamInput, 128, Left(strChar_20, 128))
        .Parameters.Append .CreateParameter("@Date_1", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDate_1) = 0, Null, dtDate_1))
        .Parameters.Append .CreateParameter("@Date_2", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDate_2) = 0, Null, dtDate_2))
        .Parameters.Append .CreateParameter("@Date_3", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDate_3) = 0, Null, dtDate_3))
        .Parameters.Append .CreateParameter("@Date_4", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDate_4) = 0, Null, dtDate_4))
        .Parameters.Append .CreateParameter("@Date_5", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDate_5) = 0, Null, dtDate_5))
        .Parameters.Append .CreateParameter("@Date_6", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDate_6) = 0, Null, dtDate_6))
        .Parameters.Append .CreateParameter("@Text_1", adLongVarChar, adParamInput, IIf(Len(strText_1) = 0, 1, Len(strText_1)))
        .Parameters("@Text_1").AppendChunk strText_1
        .Parameters.Append .CreateParameter("@Text_2", adLongVarChar, adParamInput, IIf(Len(strText_2) = 0, 1, Len(strText_2)))
        .Parameters("@Text_2").AppendChunk strText_2
        .Parameters.Append .CreateParameter("@NoteText", adLongVarChar, adParamInput, IIf(Len(strNoteText) = 0, 1, Len(strNoteText)))
        .Parameters("@NoteText").AppendChunk strNoteText
        .Parameters.Append .CreateParameter("@LockedYN", adVarChar, adParamInput, 1, IIf(blnLockedYN = False, "N", "Y"))
        .Parameters.Append .CreateParameter("@SignedBy", adInteger, adParamInput, , IIf(lngSignedBy <= 0, Null, lngSignedBy))
        .Parameters.Append .CreateParameter("@DateSigned", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDateSigned) = 0, Null, dtDateSigned))
        .Parameters.Append .CreateParameter("@AddedBy", adVarChar, adParamInput, 30, Left(strUserName, 30))
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    Set cnn = New ADODB.Connection
    cnn.Open (CONST_PSYQUEL_CNN)
    Set cmd.ActiveConnection = cnn
    cmd.Execute , , adExecuteNoRecords

    Insert = cmd.Parameters(0).Value
    
    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
           
    Set cmd.ActiveConnection = Nothing
    cnn.Close
    Set cnn = Nothing
    Set cmd = Nothing
    
    GetObjectContext.SetComplete
    
    Exit Function
   
ErrTrap:
    GetObjectContext.SetAbort
    
    Set cmd.ActiveConnection = Nothing
    cnn.Close   ' close DB connection
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Sub Update(ByVal lngIntakeID As Long, ByVal lngPatientID As Long, ByVal lngTemplateID As Long, ByVal lngProviderID As Long, _
                       ByVal lngClinicID As Long, ByVal strNoteTitle As String, ByVal dtNoteDate As Date, _
                       ByVal strChar_1 As String, ByVal strChar_2 As String, ByVal strChar_3 As String, ByVal strChar_4 As String, _
                       ByVal strChar_5 As String, ByVal strChar_6 As String, ByVal strChar_7 As String, ByVal strChar_8 As String, _
                       ByVal strChar_9 As String, ByVal strChar_10 As String, ByVal strChar_11 As String, ByVal strChar_12 As String, _
                       ByVal strChar_13 As String, ByVal strChar_14 As String, ByVal strChar_15 As String, ByVal strChar_16 As String, _
                       ByVal strChar_17 As String, ByVal strChar_18 As String, ByVal strChar_19 As String, ByVal strChar_20 As String, _
                       ByVal dtDate_1 As Date, ByVal dtDate_2 As Date, ByVal dtDate_3 As Date, ByVal dtDate_4 As Date, _
                       ByVal dtDate_5 As Date, ByVal dtDate_6 As Date, ByVal strText_1 As String, ByVal strText_2 As String, _
                       ByVal strNoteText As String, ByVal blnLockedYN As Boolean, ByVal lngSignedBy As Long, _
                       ByVal dtDateSigned As Date, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 05/25/2020
'Author: Duane C Orth
'Description: Updates selected records in the tblIntake table.  Calls
'               stored procedure uspUpdIntake.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'Returns: Long
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection ' Declare DB connection
    Dim cmd As ADODB.Command    ' Declare SQL statement

    On Error GoTo ErrTrap:

    Set cmd = New ADODB.Command     ' Instantiate SQL statement
    
    'Create the parameter objects
    With cmd
        .CommandText = "uspUpdIntake"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@IntakeID", adInteger, adParamInput, , lngIntakeID)
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID)
        .Parameters.Append .CreateParameter("@TemplateID", adInteger, adParamInput, , lngTemplateID)
        .Parameters.Append .CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID)
        .Parameters.Append .CreateParameter("@ClinicID", adInteger, adParamInput, , IIf(lngClinicID <= 0, Null, lngClinicID))
        .Parameters.Append .CreateParameter("@NoteTitle", adVarChar, adParamInput, 128, Left(strNoteTitle, 128))
        .Parameters.Append .CreateParameter("@NoteDate", adDBTimeStamp, adParamInput, , IIf(CDbl(dtNoteDate) = 0, Now(), dtNoteDate))
        .Parameters.Append .CreateParameter("@Char_1", adVarChar, adParamInput, 128, Left(strChar_1, 128))
        .Parameters.Append .CreateParameter("@Char_2", adVarChar, adParamInput, 128, Left(strChar_2, 128))
        .Parameters.Append .CreateParameter("@Char_3", adVarChar, adParamInput, 128, Left(strChar_3, 128))
        .Parameters.Append .CreateParameter("@Char_4", adVarChar, adParamInput, 128, Left(strChar_4, 128))
        .Parameters.Append .CreateParameter("@Char_5", adVarChar, adParamInput, 128, Left(strChar_5, 128))
        .Parameters.Append .CreateParameter("@Char_6", adVarChar, adParamInput, 128, Left(strChar_6, 128))
        .Parameters.Append .CreateParameter("@Char_7", adVarChar, adParamInput, 128, Left(strChar_7, 128))
        .Parameters.Append .CreateParameter("@Char_8", adVarChar, adParamInput, 128, Left(strChar_8, 128))
        .Parameters.Append .CreateParameter("@Char_9", adVarChar, adParamInput, 128, Left(strChar_9, 128))
        .Parameters.Append .CreateParameter("@Char_10", adVarChar, adParamInput, 128, Left(strChar_10, 128))
        .Parameters.Append .CreateParameter("@Char_11", adVarChar, adParamInput, 128, Left(strChar_11, 128))
        .Parameters.Append .CreateParameter("@Char_12", adVarChar, adParamInput, 128, Left(strChar_12, 128))
        .Parameters.Append .CreateParameter("@Char_13", adVarChar, adParamInput, 128, Left(strChar_13, 128))
        .Parameters.Append .CreateParameter("@Char_14", adVarChar, adParamInput, 128, Left(strChar_14, 128))
        .Parameters.Append .CreateParameter("@Char_15", adVarChar, adParamInput, 128, Left(strChar_15, 128))
        .Parameters.Append .CreateParameter("@Char_16", adVarChar, adParamInput, 128, Left(strChar_16, 128))
        .Parameters.Append .CreateParameter("@Char_17", adVarChar, adParamInput, 128, Left(strChar_17, 128))
        .Parameters.Append .CreateParameter("@Char_18", adVarChar, adParamInput, 128, Left(strChar_18, 128))
        .Parameters.Append .CreateParameter("@Char_19", adVarChar, adParamInput, 128, Left(strChar_19, 128))
        .Parameters.Append .CreateParameter("@Char_20", adVarChar, adParamInput, 128, Left(strChar_20, 128))
        .Parameters.Append .CreateParameter("@Date_1", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDate_1) = 0, Null, dtDate_1))
        .Parameters.Append .CreateParameter("@Date_2", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDate_2) = 0, Null, dtDate_2))
        .Parameters.Append .CreateParameter("@Date_3", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDate_3) = 0, Null, dtDate_3))
        .Parameters.Append .CreateParameter("@Date_4", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDate_4) = 0, Null, dtDate_4))
        .Parameters.Append .CreateParameter("@Date_5", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDate_5) = 0, Null, dtDate_5))
        .Parameters.Append .CreateParameter("@Date_6", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDate_6) = 0, Null, dtDate_6))
        .Parameters.Append .CreateParameter("@Text_1", adLongVarChar, adParamInput, IIf(Len(strText_1) = 0, 1, Len(strText_1)))
        .Parameters("@Text_1").AppendChunk strText_1
        .Parameters.Append .CreateParameter("@Text_2", adLongVarChar, adParamInput, IIf(Len(strText_2) = 0, 1, Len(strText_2)))
        .Parameters("@Text_2").AppendChunk strText_2
        .Parameters.Append .CreateParameter("@NoteText", adLongVarChar, adParamInput, IIf(Len(strNoteText) = 0, 1, Len(strNoteText)))
        .Parameters("@NoteText").AppendChunk strNoteText
        .Parameters.Append .CreateParameter("@LockedYN", adVarChar, adParamInput, 1, IIf(blnLockedYN = False, "N", "Y"))
        .Parameters.Append .CreateParameter("@SignedBy", adInteger, adParamInput, , IIf(lngSignedBy <= 0, Null, lngSignedBy))
        .Parameters.Append .CreateParameter("@DateSigned", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDateSigned) = 0, Null, dtDateSigned))
        .Parameters.Append .CreateParameter("@UserName", adVarChar, adParamInput, 30, Left(strUserName, 30))
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
   
    Set cnn = New ADODB.Connection  ' Instantiate DB connection
    cnn.Open (CONST_PSYQUEL_CNN)
    Set cmd.ActiveConnection = cnn
    cmd.Execute , , adExecuteNoRecords

    If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
        
    Set cmd.ActiveConnection = Nothing
    cnn.Close   ' close DB connection
    Set cmd = Nothing
    Set cnn = Nothing
    
    GetObjectContext.SetComplete

    Exit Sub
    
ErrTrap:
    GetObjectContext.SetAbort
    
    cmd.ActiveConnection = Nothing
    cnn.Close   ' close DB connection
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub

Public Sub LockNote(ByVal lngIntakeID As Long, ByVal lngSignedBy As Long, ByVal dtDateSigned As Date)
'--------------------------------------------------------------------
'Date: 05/25/2020
'Author: Duane C Orth
'Description: Updates a selected record in the tblIntake table, setting
'               fldLockedYN to Y.  Cannot unlock record!!!  Calls stored
'               procedure uspUpdLockIntake.
'Parameters:  Passes the ID of the record to update.
'Returns: Long
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection ' Declare DB connection
    Dim cmd As ADODB.Command    ' Declare SQL statement

    On Error GoTo ErrTrap:

    Set cmd = New ADODB.Command     ' Instantiate SQL statement
    
    'Create the parameter objects
    With cmd
        .CommandText = "uspUpdLockIntake"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@IntakeID", adInteger, adParamInput, , lngIntakeID)
        .Parameters.Append .CreateParameter("@SignedBy", adInteger, adParamInput, , IIf(lngSignedBy <= 0, Null, lngSignedBy))
        .Parameters.Append .CreateParameter("@DateSigned", adDBTimeStamp, adParamInput, , IIf(CDbl(dtDateSigned) = 0, Null, dtDateSigned))
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
   
    'Assign the connection to the Command object and execute the stored procedure
    Set cnn = New ADODB.Connection  ' Instantiate DB connection
    cnn.Open (CONST_PSYQUEL_CNN)
    Set cmd.ActiveConnection = cnn
    cmd.Execute , , adExecuteNoRecords

    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
        
    ' De-allocate space for objects
    Set cmd.ActiveConnection = Nothing
    cnn.Close   ' close DB connection
    Set cmd = Nothing
    Set cnn = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    
    ' De-allocate space for objects
    cmd.ActiveConnection = Nothing  ' --R001
    cnn.Close   ' close DB connection
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub
Public Sub UnLockNote(ByVal lngIntakeID As Long, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 05/25/2020
'Author: Duane C Orth
'Description: Updates a selected record in the tblIntake table, setting
'               fldLockedYN to N.
'Parameters:  Passes the ID of the record to update.
'Returns: Long
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection ' Declare DB connection
    Dim cmd As ADODB.Command    ' Declare SQL statement

    On Error GoTo ErrTrap:

    Set cmd = New ADODB.Command     ' Instantiate SQL statement
    
    'Create the parameter objects
    With cmd
        .CommandText = "uspUpdUnLockIntake"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@IntakeID", adInteger, adParamInput, , lngIntakeID)
        .Parameters.Append .CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
   
    'Assign the connection to the Command object and execute the stored procedure
    Set cnn = New ADODB.Connection  ' Instantiate DB connection
    cnn.Open (CONST_PSYQUEL_CNN)
    Set cmd.ActiveConnection = cnn
    cmd.Execute , , adExecuteNoRecords

    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
        
    ' De-allocate space for objects
    Set cmd.ActiveConnection = Nothing
    cnn.Close   ' close DB connection
    Set cmd = Nothing
    Set cnn = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    
    ' De-allocate space for objects
    cmd.ActiveConnection = Nothing  ' --R001
    cnn.Close   ' close DB connection
    Set cmd = Nothing
    Set cnn = Nothing
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub

Public Sub Delete(ByVal strUserName As String, ByVal lngIntakeID As Long)
'--------------------------------------------------------------------
'Date: 05/25/2020
'Author: Duane C Orth
'Description: Deletes selected record.  Calls
'               stored procedure uspDelIntake.
'Parameters:  lngIntakeID - ID of the patient.
'             strUserName - username of user deleting data : not used now
'Returns: Long
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declare connection, returned recordset, and command
    Dim cnn As ADODB.Connection ' Declare DB connection
    Dim cmd As ADODB.Command    ' Declare SQL statement

    On Error GoTo ErrTrap:

    ' Instantiate the objects
    ' Set rst = New ADODB.Recordset   ' Instantiate recordset
    Set cmd = New ADODB.Command     ' Instantiate SQL statement
    Set cnn = New ADODB.Connection  ' Instantiate DB connection
    
    'Create the parameter objects
    With cmd
        .CommandText = "uspDelIntake"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@IntakeID", adInteger, adParamInput, , lngIntakeID)
        .Parameters.Append .CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName)
        .Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Assign the connection to the Command object and execute the stored procedure
    cnn.Open (CONST_PSYQUEL_CNN) ' open DB connection
    Set cmd.ActiveConnection = cnn
    cmd.Execute , , adExecuteNoRecords

    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
        
    ' De-allocate space for objects
    Set cmd.ActiveConnection = Nothing
    cnn.Close   ' close DB connection
    Set cmd = Nothing
    Set cnn = Nothing
        
    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    
    ' De-allocate space for objects
    cmd.ActiveConnection = Nothing
    cnn.Close
    Set cmd = Nothing
    Set cnn = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub






