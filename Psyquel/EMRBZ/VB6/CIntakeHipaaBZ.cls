VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CIntakeHipaaBZ"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CIntakeHipaaBZ
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  MTS business object designed to call methods
'              associated with the CIntakeHipaaDB class.
'--------------------------------------------------------------------
Option Explicit

Private Const CLASS_NAME As String = "CIntakeHipaaBZ"
Private Const TABLE_NAME As String = "tblIntakeHipaa"
Public Function FetchByID(ByVal lngIntakeHipaaID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Retrieves records from the tblIntakeHipaa table.
'Parameters: lngIntakeHipaaID - ID number of the record to retrieve, which
'               is primary key of table.
'            lngUserID - UserID of user calling method.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declares database onject
    Dim obj As EMRDB.CIntakeHipaaDB
    ' Declares recordset for result returned
    Dim rstSQL As ADODB.Recordset
    
    On Error GoTo ErrTrap:

    ' Instantiates database onject
    Set obj = CreateObject("EMRDB.CIntakeHipaaDB")
    
    ' Instantiates recordset for result returned
    Set rstSQL = obj.FetchByID(lngIntakeHipaaID)
    
    ' Return value set for function
    Set FetchByID = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete
    ' Set DB object to NULL
    Set obj = Nothing
    ' Exit on success
    Exit Function
    
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set obj = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Function FetchByPatient(ByVal lngPatientID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Retrieves records from the tblIntakeHipaa table.
'Parameters: lngPatientID - ID number of the record to retrieve based on the alertID.
'            lngUserID - UserID of user calling method.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------
    
    Dim obj As EMRDB.CIntakeHipaaDB
    Dim rstSQL As ADODB.Recordset

    On Error GoTo ErrTrap:

    Set obj = CreateObject("EMRDB.CIntakeHipaaDB")
    
    Set rstSQL = obj.FetchByPatient(lngPatientID)
    
    Set FetchByPatient = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set obj = Nothing
    Exit Function
    
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set obj = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Function Insert(ByVal lngPatientID As Long, ByVal lngProviderID As Long, _
                       ByVal lngClinicID As Long, ByVal dtNoteDate As Date, _
                       ByVal strNoteText As String, ByVal blnLockedYN As Boolean, _
                       ByVal lngSignedBy As Long, ByVal dtDateSigned As Date, _
                       ByVal strUserName As String) As Long
                       
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Inserts a row into the tblIntakeHipaa table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'Returns: ID (Primary Key) of the row inserted.
'--------------------------------------------------------------------
    Dim obj As EMRDB.CIntakeHipaaDB
    Dim strErrMessage As String
    
    On Error GoTo ErrTrap:
    
    ' Validate data being passed
  ''  If Not VerifyData(lngTemplateID, strNoteText, strErrMessage, lngEncounterLogID, _
  ''      lngPatientID, , strUserName) Then GoTo ErrTrap
       
    Set obj = CreateObject("EMRDB.CIntakeHipaaDB")
    
    Insert = obj.Insert(lngPatientID, lngProviderID, lngClinicID, dtNoteDate, _
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
Public Sub Update(ByVal lngIntakeHipaaID As Long, ByVal lngPatientID As Long, ByVal lngProviderID As Long, _
                       ByVal lngClinicID As Long, ByVal dtNoteDate As Date, _
                       ByVal strNoteText As String, ByVal blnLockedYN As Boolean, _
                       ByVal lngSignedBy As Long, ByVal dtDateSigned As Date, _
                       ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Updates a row in the tblIntakeHipaa table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              updated, except for lngUserID, which is for user
'              verification and for the fetch method, but is not passed
'              to EMRDB.CIntakeHipaaDB.Update.
'Returns: Null
'--------------------------------------------------------------------
    
    ' Declare database object
    Dim obj As EMRDB.CIntakeHipaaDB
    
    ' Declare recordset for fetch note
    Dim rstIntakeHipaa As ADODB.Recordset
    
    ' Declare error message string
    Dim strErrMessage As String

    On Error GoTo ErrTrap:

    ' Verify that ID number is valid; must be >= 0 (1st index==1)
    ' -- if PatientNoteID was passed, that is.
    If lngIntakeHipaaID < 0 Then
        strErrMessage = "PatientNoteID number provided is not valid."
        GoTo ErrTrap
    End If

    ' Instantiate database object
    Set obj = CreateObject("EMRDB.CIntakeHipaaDB")
    
    ' Get the appropriate record
    Set rstIntakeHipaa = obj.FetchByID(lngIntakeHipaaID)
    
    ' Is the note locked?  If so, error, don't update.
    If UCase(rstIntakeHipaa.Fields("fldLockedYN")) = "Y" Then
        strErrMessage = "Record is locked and cannot be updated."
        GoTo ErrTrap
    End If
    
    Set rstIntakeHipaa = Nothing
    
    ' Run database object
    obj.Update lngIntakeHipaaID, lngPatientID, lngProviderID, lngClinicID, dtNoteDate, _
                       strNoteText, blnLockedYN, lngSignedBy, dtDateSigned, strUserName
    
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
Public Sub LockNote(ByVal lngIntakeHipaaID As Long, ByVal lngSignedBy As Long, ByVal dtDateSigned As Date)
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Flags a row in the tblIntakeHipaa table marking the row as
'              deleted or undeleted.
'Parameters: lngIntakeHipaaID - ID of the row in the table whose value will be
'               updated.
'Returns: Null
'--------------------------------------------------------------------

    ' Declare database object
    Dim obj As EMRDB.CIntakeHipaaDB
    
    ' Declare error message string
    Dim strErrMessage As String

    On Error GoTo ErrTrap:

    ' Instantiate database object
    Set obj = CreateObject("EMRDB.CIntakeHipaaDB")
    
    ' Run database object
    obj.LockNote lngIntakeHipaaID, lngSignedBy, dtDateSigned
    
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
Public Sub UnLockNote(ByVal lngIntakeHipaaID As Long, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Flags a row in the tblIntakeHipaa table marking the row as
'              deleted or undeleted.
'Parameters: lngIntakeHipaaID - ID of the row in the table whose value will be
'               updated.
'Returns: Null
'--------------------------------------------------------------------

    ' Declare database object
    Dim obj As EMRDB.CIntakeHipaaDB
    
    ' Declare error message string
    Dim strErrMessage As String

    On Error GoTo ErrTrap:

    ' Instantiate database object
    Set obj = CreateObject("EMRDB.CIntakeHipaaDB")
    
    ' Run database object
    obj.UnLockNote lngIntakeHipaaID, strUserName
    
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
Public Sub Delete(ByVal strUserName As String, ByVal lngIntakeHipaaID As Long)
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Flags a row in the tblIntakeHipaa table marking the row as
'              deleted or undeleted.
'Parameters: lngIntakeHipaaID - ID of the row in the table whose value will be
'               updated.
'            strUserName - Login name of the user responsible for
'               marking the row as deleted.
'Returns: Null
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declare DB obj
    Dim obj As EMRDB.CIntakeHipaaDB
    Dim strErrMessage As String
    
    On Error GoTo ErrTrap:

    ' Instanitate DB obj
    Set obj = CreateObject("EMRDB.CIntakeHipaaDB")
    
    'Call on valid check
    If Not (obj.FetchByID(lngIntakeHipaaID).RecordCount = 0) Then
        If ((obj.FetchByID(lngIntakeHipaaID).Fields("fldLockedYN")) = "N") Then
            ' Fire delete method
            Call obj.Delete(strUserName, lngIntakeHipaaID)
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
    Optional ByVal lngPatientID As Long, Optional ByVal lngIntakeHipaaID As Long, _
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
    If lngIntakeHipaaID < 0 Then
        strErrMessage = "PatientNoteID number provided is not valid."
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
    
    VerifyData = True

End Function







