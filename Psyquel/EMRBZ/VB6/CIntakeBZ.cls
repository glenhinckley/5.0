VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CIntakeBZ"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CIntakeBZ
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  MTS business object designed to call methods
'              associated with the CIntakeDB class.
'--------------------------------------------------------------------
Option Explicit

Private Const CLASS_NAME As String = "CIntakeBZ"
Private Const TABLE_NAME As String = "tblIntake"
Public Function FetchByID(ByVal lngIntakeID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Retrieves records from the tblIntake table.
'Parameters: lngIntakeID - ID number of the record to retrieve, which
'               is primary key of table.
'            lngUserID - UserID of user calling method.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    Dim obj As EMRDB.CIntakeDB
    Dim rstSQL As ADODB.Recordset
    
    On Error GoTo ErrTrap:

    Set obj = CreateObject("EMRDB.CIntakeDB")
    Set rstSQL = obj.FetchByID(lngIntakeID)
    Set FetchByID = rstSQL
    
    GetObjectContext.SetComplete
    Set obj = Nothing
    Exit Function
    
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set obj = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Function FetchByPatient(ByVal lngPatientID As Long, Optional ByVal lngTemplateID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Retrieves records from the tblIntake table.
'Parameters: lngPatientID - ID number of the record to retrieve based on the alertID.
'            lngUserID - UserID of user calling method.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------
    
    Dim obj As EMRDB.CIntakeDB
    Dim rstSQL As ADODB.Recordset

    On Error GoTo ErrTrap:

    Set obj = CreateObject("EMRDB.CIntakeDB")
    Set rstSQL = obj.FetchByPatient(lngPatientID, lngTemplateID)
    Set FetchByPatient = rstSQL
    
    GetObjectContext.SetComplete
    Set obj = Nothing
    Exit Function
    
ErrTrap:
    Set obj = Nothing
    GetObjectContext.SetAbort
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
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Inserts a row into the tblIntake table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'Returns: ID (Primary Key) of the row inserted.
'--------------------------------------------------------------------
    Dim obj As EMRDB.CIntakeDB
    Dim strErrMessage As String
    
    On Error GoTo ErrTrap:
    
    ' Validate data being passed
  ''  If Not VerifyData(strNoteText, strErrMessage, lngPatientID, , strUserName) Then GoTo ErrTrap
       
    Set obj = CreateObject("EMRDB.CIntakeDB")
    
    Insert = obj.Insert(lngPatientID, lngTemplateID, lngProviderID, lngClinicID, strNoteTitle, dtNoteDate, _
                       strChar_1, strChar_2, strChar_3, strChar_4, strChar_5, strChar_6, strChar_7, strChar_8, strChar_9, strChar_10, _
                       strChar_11, strChar_12, strChar_13, strChar_14, strChar_15, strChar_16, strChar_17, strChar_18, strChar_19, strChar_20, _
                       dtDate_1, dtDate_2, dtDate_3, dtDate_4, dtDate_5, dtDate_6, strText_1, strText_2, _
                       strNoteText, blnLockedYN, lngSignedBy, dtDateSigned, strUserName)
       
    'Signal successful completion
    GetObjectContext.SetComplete
    Set obj = Nothing
    Exit Function
    
ErrTrap:
    GetObjectContext.SetAbort
    Set obj = Nothing
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
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
'Date: 05/26/2020
'Author: Duane C Orth
'Description:  Updates a row in the tblIntake table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              updated, except for lngUserID, which is for user
'              verification and for the fetch method, but is not passed
'              to EMRDB.CIntakeDB.Update.
'Returns: Null
'--------------------------------------------------------------------
    
    Dim obj As EMRDB.CIntakeDB
    Dim rstIntake As ADODB.Recordset
    Dim strErrMessage As String

    On Error GoTo ErrTrap:

    ' Verify that ID number is valid; must be >= 0 (1st index==1)
    ' -- if PatientNoteID was passed, that is.
    If lngIntakeID < 0 Then
        strErrMessage = "IntakeID number provided is not valid."
        GoTo ErrTrap
    End If

    Set obj = CreateObject("EMRDB.CIntakeDB")
    Set rstIntake = obj.FetchByID(lngIntakeID)
    
    ' Is the note locked?  If so, error, don't update.
    If UCase(rstIntake.Fields("fldLockedYN")) = "Y" Then
        strErrMessage = "Record is locked and cannot be updated."
        GoTo ErrTrap
    End If
    
    Set rstIntake = Nothing
    
    ' Run database object
    obj.Update lngIntakeID, lngPatientID, lngTemplateID, lngProviderID, lngClinicID, strNoteTitle, dtNoteDate, _
                       strChar_1, strChar_2, strChar_3, strChar_4, strChar_5, strChar_6, strChar_7, strChar_8, strChar_9, strChar_10, _
                       strChar_11, strChar_12, strChar_13, strChar_14, strChar_15, strChar_16, strChar_17, strChar_18, strChar_19, strChar_20, _
                       dtDate_1, dtDate_2, dtDate_3, dtDate_4, dtDate_5, dtDate_6, strText_1, strText_2, _
                       strNoteText, blnLockedYN, lngSignedBy, dtDateSigned, strUserName
    
    GetObjectContext.SetComplete
    Set obj = Nothing
    Exit Sub

ErrTrap:
    GetObjectContext.SetAbort
    Set obj = Nothing
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub
Public Sub LockNote(ByVal lngIntakeID As Long, ByVal lngSignedBy As Long, ByVal dtDateSigned As Date)
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Flags a row in the tblIntake table marking the row as
'              deleted or undeleted.
'Parameters: lngIntakeID - ID of the row in the table whose value will be
'               updated.
'Returns: Null
'--------------------------------------------------------------------

    Dim obj As EMRDB.CIntakeDB
    Dim strErrMessage As String

    On Error GoTo ErrTrap:

    Set obj = CreateObject("EMRDB.CIntakeDB")
    obj.LockNote lngIntakeID, lngSignedBy, dtDateSigned
    
    GetObjectContext.SetComplete
    
    Set obj = Nothing
    Exit Sub

ErrTrap:
    GetObjectContext.SetAbort
    Set obj = Nothing
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub
Public Sub UnLockNote(ByVal lngIntakeID As Long, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Flags a row in the tblIntake table marking the row as
'              deleted or undeleted.
'Parameters: lngIntakeID - ID of the row in the table whose value will be
'               updated.
'Returns: Null
'--------------------------------------------------------------------

    ' Declare database object
    Dim obj As EMRDB.CIntakeDB
    
    ' Declare error message string
    Dim strErrMessage As String

    On Error GoTo ErrTrap:

    ' Instantiate database object
    Set obj = CreateObject("EMRDB.CIntakeDB")
    
    ' Run database object
    obj.UnLockNote lngIntakeID, strUserName
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set obj = Nothing
    Exit Sub

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    
    Set obj = Nothing
    
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub
Public Sub Delete(ByVal strUserName As String, ByVal lngIntakeID As Long)
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Flags a row in the tblIntake table marking the row as
'              deleted or undeleted.
'Parameters: lngIntakeID - ID of the row in the table whose value will be
'               updated.
'            strUserName - Login name of the user responsible for
'               marking the row as deleted.
'            lngUserID - UserID must be passed to verify LockedYN
'Returns: Null
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declare DB obj
    Dim obj As EMRDB.CIntakeDB
    Dim strErrMessage As String
    
    On Error GoTo ErrTrap:

    ' Instanitate DB obj
    Set obj = CreateObject("EMRDB.CIntakeDB")
    
    'Call on valid check
    If Not (obj.FetchByID(lngIntakeID).RecordCount = 0) Then
        If ((obj.FetchByID(lngIntakeID).Fields("fldLockedYN")) = "N") Then
            ' Fire delete method
            Call obj.Delete(strUserName, lngIntakeID)
        Else
            ' Tell user that record is locked
            strErrMessage = "The record you are trying to delete is locked."
            GoTo ErrTrap
        End If
    Else
        'Tell user that record doesn't exist
        strErrMessage = "The record you are trying to delete does not exist."
        GoTo ErrTrap
    End If
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set obj = Nothing
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set obj = Nothing
    GetObjectContext.SetAbort
    
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub
Private Function VerifyData(ByVal strNoteText As String, _
    ByRef strErrMessage As String, _
    Optional ByVal lngPatientID As Long, Optional ByVal lngIntakeID As Long, _
    Optional ByVal strUserName As String) As Boolean
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Verifies all required data has been provided by the user.
'              Last two params are optional to take into account the
'              different needs for update and insert.  Boolean value doesn't
'              be validated because all bool values are acceptable.
'Parameters:  The values to be checked.
'Returns: Boolean value identifying if all criteria has been satisfied.
'--------------------------------------------------------------------
'
    ' Verify that something was entered into patientNotes text field.
    If Trim(strNoteText) = "" Then
        strErrMessage = "No information was entered for the Patient Note."
        VerifyData = False
        Exit Function
    End If
   
    ' Verify that ID number is valid; must be >= 0 (1st index==1)
    ' -- if PatientNoteID was passed, that is.
    If lngIntakeID < 0 Then
        strErrMessage = "Intake NoteID number provided is not valid."
        VerifyData = False
        Exit Function
    End If
    
    If lngPatientID < 0 Then
        strErrMessage = "PatientID number provided is not valid."
        VerifyData = False
        Exit Function
    End If
  
    ' Check to see if username provided is not null--if it's provided
    ' First check to see if username was passed as a parameter
    If Trim(strNoteText) = "" Then
        strErrMessage = "A user name is required."
        VerifyData = False
        Exit Function
    End If
    
    'If we get here, all is well...
    VerifyData = True

End Function










