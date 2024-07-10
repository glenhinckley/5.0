VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CPatientNoteBZ"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CPatientNoteBZ
'Date: 09/01/2000
'Author: David Nichol and Joshua Lockwood
'Description:  MTS business object designed to call methods
'              associated with the CPatientNoteDB class.
'--------------------------------------------------------------------
'   --R001 : Joshua Lockwood, 15Sep00 : Added code to delete() that verifies
'       whether a record exists or is locked.
'   --R002 : David Nichol, 01Mar2001 : Added FetchNoteData
'--------------------------------------------------------------------
Option Explicit

Private Const CLASS_NAME As String = "CPatientNoteBz"
Private Const PATIENTALERT_TABLE As String = "tblPatientNote"
Public Function FetchByID(ByVal lngPatientNoteID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 09/01/2000
'Author: Dave Nichol and Joshua Lockwood
'Description:  Retrieves records from the tblPatientNote table.
'Parameters: lngPatientNoteID - ID number of the record to retrieve, which
'               is primary key of table.
'            lngUserID - UserID of user calling method.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declares database onject
    Dim objPatientNote As EMRDB.CPatientNoteDB
    ' Declares recordset for result returned
    Dim rstSQL As ADODB.Recordset
    
    ' Duh
    On Error GoTo ErrTrap:

    ' Instantiates database onject
    Set objPatientNote = CreateObject("EMRDB.CPatientNoteDB")
    
    ' Instantiates recordset for result returned
    Set rstSQL = objPatientNote.FetchByID(lngPatientNoteID)
    
    ' Return value set for function
    Set FetchByID = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete
    ' Set DB object to NULL
    Set objPatientNote = Nothing
    ' Exit on success
    Exit Function
    
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objPatientNote = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Function FetchByPatient(ByVal lngUserID As Long, ByVal lngPatientID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 09/01/2000
'Author: Dave Nichol and Joshua Lockwood
'Description:  Retrieves records from the tblPatientNote table.
'Parameters: lngPatientID - ID number of the record to retrieve based on the alertID.
'            lngUserID - UserID of user calling method.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------
    
    Dim objPatientNote As EMRDB.CPatientNoteDB
    Dim rstSQL As ADODB.Recordset

    On Error GoTo ErrTrap:

    Set objPatientNote = CreateObject("EMRDB.CPatientNoteDB")
    
    Set rstSQL = objPatientNote.FetchByPatient(lngUserID, lngPatientID)
    
    Set FetchByPatient = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objPatientNote = Nothing
    Exit Function
    
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objPatientNote = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Function FetchByAppt(ByVal lngUserID As Long, ByVal lngApptID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 09/01/2000
'Author: Dave Nichol and Joshua Lockwood
'Description:  Retrieves records from the tblPatientNote table.
'Parameters: lngApptID - ID number of the appointment that the user desires.
'            lngUserID - UserID of user calling method.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    Dim objPatientNote As EMRDB.CPatientNoteDB
    Dim rstSQL As ADODB.Recordset

    On Error GoTo ErrTrap:

    Set objPatientNote = CreateObject("EMRDB.CPatientNoteDB")
    
    Set rstSQL = objPatientNote.FetchByAppt(lngUserID, lngApptID)
    
    Set FetchByAppt = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objPatientNote = Nothing
    Exit Function
    
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objPatientNote = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Function FetchByEncounter(ByVal lngUserID As Long, ByVal lngEncLogID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 10/06/2000
'Author: Dave Nichol and Joshua Lockwood
'Description:  Retrieves records from the tblPatientNote table by EncouterLogID.
'Parameters: lngEncLogID - ID number of the encounter log that the user desires.
'            lngUserID - UserID of user calling method.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------

    Dim objPatientNote As EMRDB.CPatientNoteDB
    Dim rstSQL As ADODB.Recordset

    On Error GoTo ErrTrap:

    Set objPatientNote = CreateObject("EMRDB.CPatientNoteDB")
    
    Set rstSQL = objPatientNote.FetchByEncounter(lngUserID, lngEncLogID)
    
    Set FetchByEncounter = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objPatientNote = Nothing
    Exit Function
    
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objPatientNote = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Function FetchNoteData(ByVal lngEncLogID As Long, ByVal lngUserID As Long) As Collection
'--------------------------------------------------------------------
'Date: 02/28/2001
'Author: Dave Nichol and Joshua Lockwood
'Description:  Retrieves recordsets from various tables by EncouterLogID.
'Parameters: lngEncLogID - ID number of the encounter log that the user desires.
'            lngUserID - UserID of user calling method.
'Returns: recordset of ADODB.Recordsets
'--------------------------------------------------------------------

    Dim objPatientNote As EMRDB.CPatientNoteDB
    Dim col As Collection
    
    On Error GoTo ErrTrap:

    Set objPatientNote = CreateObject("EMRDB.CPatientNoteDB")
    
    Set col = objPatientNote.FetchNoteData(lngEncLogID, lngUserID)
    
    Set FetchNoteData = col
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objPatientNote = Nothing
    Set col = Nothing
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objPatientNote = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Function FetchNextNote(ByVal lngUserID As Long, ByVal lngPatientNoteID As Long, _
                              ByVal lngPatientID As Long, Optional ByVal lngEncounterLogID As Long = 0) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 10/06/2000
'Author: Dave Nichol and Joshua Lockwood
'Description:  Retrieves next record from the tblPatientNote table by EncouterLogID.
'Parameters: lngPatientNoteID - ID number of the patient note that the user desires.
'            lngPatientID - ID of the patient associated with the Patient Note
'            lngUserID - UserID of user calling method.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------

    Dim objPatientNote As EMRDB.CPatientNoteDB
    Dim rstSQL As ADODB.Recordset

    On Error GoTo ErrTrap:

    Set objPatientNote = CreateObject("EMRDB.CPatientNoteDB")
    
    Set rstSQL = objPatientNote.FetchNextNote(lngUserID, lngPatientNoteID, lngPatientID)
    
    Set FetchNextNote = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objPatientNote = Nothing
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objPatientNote = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Function FetchPrevNote(ByVal lngUserID As Long, ByVal lngPatientNoteID As Long, _
                              ByVal lngPatientID As Long, Optional ByVal lngEncounterLogID As Long = 0) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 10/06/2000
'Author: Dave Nichol and Joshua Lockwood
'Description:  Retrieves previous record from the tblPatientNote table by EncouterLogID.
'Parameters: lngPatientNoteID - ID number of the patient note that the user desires.
'            lngPatientID - ID of the patient associated with the Patient Note
'            lngUserID - UserID of user calling method.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------

    Dim objPatientNote As EMRDB.CPatientNoteDB
    Dim rstSQL As ADODB.Recordset

    On Error GoTo ErrTrap:

    Set objPatientNote = CreateObject("EMRDB.CPatientNoteDB")
    
    Set rstSQL = objPatientNote.FetchPrevNote(lngUserID, lngPatientNoteID, lngPatientID, lngEncounterLogID)
    
    Set FetchPrevNote = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objPatientNote = Nothing
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objPatientNote = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Function Insert(ByVal lngPatientID As Long, ByVal lngTemplateID As Long, _
                       ByVal lngEncounterLogID As Long, ByVal lngProviderID As Long, ByVal dtNoteDate As Date, _
                       ByVal strNoteTitle As String, ByVal strNoteText As String, ByVal strMipsText As String, ByVal lngSupervisorID As Long, _
                       ByVal lngSignedBy As Long, ByVal dtDateSigned As Date, _
                       ByVal blnLockedYN As Boolean, ByVal strUserName As String) As Long
'--------------------------------------------------------------------
'Date: 09/01/2000
'Author: David Nichol and Joshua Lockwood
'Description:  Inserts a row into the tblPatientNote table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'Returns: ID (Primary Key) of the row inserted.
'--------------------------------------------------------------------
    ' Declare DB object (connection)
    Dim objPatientNote As EMRDB.CPatientNoteDB
    ' Declare buffer for err msg
    Dim strErrMessage As String
    ' Set tag for err trap code
    
    On Error GoTo ErrTrap:
    
    ' Validate data being passed
    If Not VerifyData(lngTemplateID, strNoteText, strErrMessage, lngEncounterLogID, _
        lngPatientID, , strUserName) Then GoTo ErrTrap
       
    ' Instantiate DB obj (connection)
    Set objPatientNote = CreateObject("EMRDB.CPatientNoteDB")
    
    ' Fire Insert method
    Insert = objPatientNote.Insert(lngPatientID, lngTemplateID, lngEncounterLogID, lngProviderID, _
        dtNoteDate, strNoteTitle, strNoteText, strMipsText, lngSupervisorID, lngSignedBy, dtDateSigned, blnLockedYN, strUserName)
        
    'Signal successful completion
    GetObjectContext.SetComplete
    ' Set DB obj to null
    Set objPatientNote = Nothing
    ' Exit on success
    Exit Function
    
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    ' Close DB connection
    Set objPatientNote = Nothing
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Function
Public Sub Update(ByVal lngUserID As Long, ByVal lngPatientNoteID As Long, ByVal lngTemplateID As Long, _
                ByVal dtNoteDate As Date, ByVal strNoteTitle As String, _
                ByVal strNoteText As String, ByVal strMipsText As String, ByVal lngSupervisorID As Long)
'--------------------------------------------------------------------
'Date: 09/01/2000
'Author: David Nichol and Joshua Lockwood
'Description:  Updates a row in the tblPatientNote table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              updated, except for lngUserID, which is for user
'              verification and for the fetch method, but is not passed
'              to EMRDB.CPatientNoteDB.Update.
'Returns: Null
'--------------------------------------------------------------------
    
    ' Declare database object
    Dim objPatientNote As EMRDB.CPatientNoteDB
    
    ' Declare recordset for fetch note
    Dim rstPatientNote As ADODB.Recordset
    
    ' Declare error message string
    Dim strErrMessage As String

    On Error GoTo ErrTrap:

    ' Verify that the TemplateID is valid; must be >= 0 (1st index==1)
    If lngTemplateID < 0 Then
        strErrMessage = "TemplateID number provided is not valid."
        GoTo ErrTrap
    End If
    
    ' Verify that something was entered into patientNotes text field.
    If Trim(strNoteText) = "" Then
        strErrMessage = "No information was entered for the Patient Note."
        GoTo ErrTrap
    End If

    ' Verify that ID number is valid; must be >= 0 (1st index==1)
    ' -- if PatientNoteID was passed, that is.
    If lngPatientNoteID < 0 Then
        strErrMessage = "PatientNoteID number provided is not valid."
        GoTo ErrTrap
    End If

    ' Instantiate database object
    Set objPatientNote = CreateObject("EMRDB.CPatientNoteDB")
    
    ' Get the appropriate record
    Set rstPatientNote = objPatientNote.FetchByID(lngPatientNoteID)
    
    ' Is the note locked?  If so, error, don't update.
    If UCase(rstPatientNote.Fields("fldLockedYN")) = "Y" Then
        strErrMessage = "Record is locked and cannot be updated."
        GoTo ErrTrap
    End If
    
    Set rstPatientNote = Nothing
    
    ' Run database object
    objPatientNote.Update lngPatientNoteID, lngTemplateID, dtNoteDate, strNoteTitle, strNoteText, strMipsText, lngSupervisorID
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objPatientNote = Nothing
    Exit Sub

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    
    Set objPatientNote = Nothing
    
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub
Public Sub LockNote(ByVal lngPatientNoteID As Long, ByVal lngSignedBy As Long, ByVal dtDateSigned As Date)
'--------------------------------------------------------------------
'Date: 09/01/2000
'Author: David Nichol and Joshua Lockwood
'Description:  Flags a row in the tblPatientNote table marking the row as
'              deleted or undeleted.
'Parameters: lngPatientNoteID - ID of the row in the table whose value will be
'               updated.
'Returns: Null
'--------------------------------------------------------------------

    ' Declare database object
    Dim objPatientNote As EMRDB.CPatientNoteDB
    
    ' Declare error message string
    Dim strErrMessage As String

    On Error GoTo ErrTrap:

    ' Instantiate database object
    Set objPatientNote = CreateObject("EMRDB.CPatientNoteDB")
    
    ' Run database object
    objPatientNote.LockNote lngPatientNoteID, lngSignedBy, dtDateSigned
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objPatientNote = Nothing
    Exit Sub

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    
    Set objPatientNote = Nothing
    
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub
Public Sub UnLockNote(ByVal lngPatientNoteID As Long, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 09/01/2000
'Author: David Nichol and Joshua Lockwood
'Description:  Flags a row in the tblPatientNote table marking the row as
'              deleted or undeleted.
'Parameters: lngPatientNoteID - ID of the row in the table whose value will be
'               updated.
'Returns: Null
'--------------------------------------------------------------------

    ' Declare database object
    Dim objPatientNote As EMRDB.CPatientNoteDB
    
    ' Declare error message string
    Dim strErrMessage As String

    On Error GoTo ErrTrap:

    ' Instantiate database object
    Set objPatientNote = CreateObject("EMRDB.CPatientNoteDB")
    
    ' Run database object
    objPatientNote.UnLockNote lngPatientNoteID, strUserName
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objPatientNote = Nothing
    Exit Sub

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    
    Set objPatientNote = Nothing
    
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub
Public Sub Delete(ByVal strUserName As String, ByVal lngPatientNoteID As Long)
'--------------------------------------------------------------------
'Date: 09/01/2000
'Author: David Nichol and Joshua Lockwood
'Description:  Flags a row in the tblPatientNote table marking the row as
'              deleted or undeleted.
'Parameters: lngPatientNoteID - ID of the row in the table whose value will be
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
    Dim objPatientNote As EMRDB.CPatientNoteDB
    Dim strErrMessage As String
    
    On Error GoTo ErrTrap:

    ' Instanitate DB obj
    Set objPatientNote = CreateObject("EMRDB.CPatientNoteDB")
    
    'Call on valid check
    If Not (objPatientNote.FetchByID(lngPatientNoteID).RecordCount = 0) Then
        If ((objPatientNote.FetchByID(lngPatientNoteID).Fields("fldLockedYN")) = "N") Then
            ' Fire delete method
            Call objPatientNote.Delete(strUserName, lngPatientNoteID)
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
    Set objPatientNote = Nothing
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objPatientNote = Nothing
    GetObjectContext.SetAbort
    
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub
Private Function VerifyData(ByVal lngTemplateID As Long, ByVal strNoteText As String, _
    ByRef strErrMessage As String, Optional ByVal lngEncounterLogID As Long, _
    Optional ByVal lngPatientID As Long, Optional ByVal lngPatientNoteID As Long, _
    Optional ByVal strUserName As String) As Boolean
'--------------------------------------------------------------------
'Date: 09/01/2000
'Author: David Nichol and Joshua Lockwood
'Description:  Verifies all required data has been provided by the user.
'              Last two params are optional to take into account the
'              different needs for update and insert.  Boolean value doesn't
'              be validated because all bool values are acceptable.
'Parameters:  The values to be checked.
'Returns: Boolean value identifying if all criteria has been satisfied.
'--------------------------------------------------------------------
'

    ' Verify that the TemplateID is valid; must be >= 0 (1st index==1)
    If lngTemplateID < 0 Then
        strErrMessage = "TemplateID number provided is not valid."
        VerifyData = False
        Exit Function
    End If
    
    ' Verify that something was entered into patientNotes text field.
    If Trim(strNoteText) = "" Then
        strErrMessage = "No information was entered for the Patient Note."
        VerifyData = False
        Exit Function
    End If
   
    ' Verify that ID number is valid; must be >= 0 (1st index==1)
    ' -- if PatientNoteID was passed, that is.
    If lngPatientNoteID < 0 Then
        strErrMessage = "PatientNoteID number provided is not valid."
        VerifyData = False
        Exit Function
    End If
  
    If lngEncounterLogID < 0 Then
        strErrMessage = "EncounterLogID number provided is not valid."
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




