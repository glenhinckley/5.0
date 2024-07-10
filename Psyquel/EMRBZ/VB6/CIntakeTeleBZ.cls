VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CIntakeTeleBZ"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CIntakeTeleBZ
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  MTS business object designed to call methods
'              associated with the CIntakeTeleDB class.
'--------------------------------------------------------------------
Option Explicit

Private Const CLASS_NAME As String = "CIntakeTeleBZ"
Private Const TABLE_NAME As String = "tblIntakeTeletherapy"
Public Function FetchByID(ByVal lngIntakeTeleID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Retrieves records from the tblIntakeTele table.
'Parameters: lngIntakeTeleID - ID number of the record to retrieve, which
'               is primary key of table.
'            lngUserID - UserID of user calling method.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declares database onject
    Dim obj As EMRDB.CIntakeTeleDB
    ' Declares recordset for result returned
    Dim rstSQL As ADODB.Recordset
    
    On Error GoTo ErrTrap:

    ' Instantiates database onject
    Set obj = CreateObject("EMRDB.CIntakeTeleDB")
    
    ' Instantiates recordset for result returned
    Set rstSQL = obj.FetchByID(lngIntakeTeleID)
    
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
'Description:  Retrieves records from the tblIntakeTele table.
'Parameters: lngPatientID - ID number of the record to retrieve based on the alertID.
'            lngUserID - UserID of user calling method.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------
    
    Dim obj As EMRDB.CIntakeTeleDB
    Dim rstSQL As ADODB.Recordset

    On Error GoTo ErrTrap:

    Set obj = CreateObject("EMRDB.CIntakeTeleDB")
    
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
                       ByVal strInTele_1a As String, ByVal strInTele_1b As String, ByVal strInTele_1c As String, _
                       ByVal strInTele_2a As String, ByVal strInTele_2b As String, ByVal strInTele_2c As String, _
                       ByVal strInTele_3a As String, ByVal strInTele_3b As String, _
                       ByVal strInTele_4a As String, ByVal strInTele_4b As String, _
                       ByVal strInTele_5a As String, ByVal strInTele_5b As String, _
                       ByVal strInTele_6a As String, ByVal strInTele_6b As String, _
                       ByVal strNoteText As String, ByVal blnLockedYN As Boolean, ByVal lngSignedBy As Long, _
                       ByVal dtDateSigned As Date, ByVal strUserName As String) As Long
                       
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Inserts a row into the tblIntakeTele table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'Returns: ID (Primary Key) of the row inserted.
'--------------------------------------------------------------------
    Dim obj As EMRDB.CIntakeTeleDB
    Dim strErrMessage As String
    
    On Error GoTo ErrTrap:
    
    ' Validate data being passed
  ''  If Not VerifyData(strNoteText, strErrMessage, lngPatientID, , strUserName) Then GoTo ErrTrap
       
    Set obj = CreateObject("EMRDB.CIntakeTeleDB")
    
    Insert = obj.Insert(lngPatientID, lngProviderID, lngClinicID, dtNoteDate, _
                       strInTele_1a, strInTele_1b, strInTele_1c, _
                       strInTele_2a, strInTele_2b, strInTele_2c, _
                       strInTele_3a, strInTele_3b, _
                       strInTele_4a, strInTele_4b, _
                       strInTele_5a, strInTele_5b, _
                       strInTele_6a, strInTele_6b, _
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
Public Sub Update(ByVal lngIntakeTeleID As Long, ByVal lngPatientID As Long, ByVal lngProviderID As Long, _
                       ByVal lngClinicID As Long, ByVal dtNoteDate As Date, _
                       ByVal strInTele_1a As String, ByVal strInTele_1b As String, ByVal strInTele_1c As String, _
                       ByVal strInTele_2a As String, ByVal strInTele_2b As String, ByVal strInTele_2c As String, _
                       ByVal strInTele_3a As String, ByVal strInTele_3b As String, _
                       ByVal strInTele_4a As String, ByVal strInTele_4b As String, _
                       ByVal strInTele_5a As String, ByVal strInTele_5b As String, _
                       ByVal strInTele_6a As String, ByVal strInTele_6b As String, _
                       ByVal strNoteText As String, ByVal blnLockedYN As Boolean, ByVal lngSignedBy As Long, _
                       ByVal dtDateSigned As Date, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Updates a row in the tblIntakeTele table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              updated, except for lngUserID, which is for user
'              verification and for the fetch method, but is not passed
'              to EMRDB.CIntakeTeleDB.Update.
'Returns: Null
'--------------------------------------------------------------------
    
    ' Declare database object
    Dim obj As EMRDB.CIntakeTeleDB
    
    ' Declare recordset for fetch note
    Dim rstIntakeTele As ADODB.Recordset
    
    ' Declare error message string
    Dim strErrMessage As String

    On Error GoTo ErrTrap:

    ' Verify that ID number is valid; must be >= 0 (1st index==1)
    ' -- if PatientNoteID was passed, that is.
    If lngIntakeTeleID < 0 Then
        strErrMessage = "IntakeTeleID number provided is not valid."
        GoTo ErrTrap
    End If

    ' Instantiate database object
    Set obj = CreateObject("EMRDB.CIntakeTeleDB")
    
    ' Get the appropriate record
    Set rstIntakeTele = obj.FetchByID(lngIntakeTeleID)
    
    ' Is the note locked?  If so, error, don't update.
    If UCase(rstIntakeTele.Fields("fldLockedYN")) = "Y" Then
        strErrMessage = "Record is locked and cannot be updated."
        GoTo ErrTrap
    End If
    
    Set rstIntakeTele = Nothing
    
    ' Run database object
    obj.Update lngIntakeTeleID, lngPatientID, lngProviderID, lngClinicID, dtNoteDate, _
                       strInTele_1a, strInTele_1b, strInTele_1c, _
                       strInTele_2a, strInTele_2b, strInTele_2c, _
                       strInTele_3a, strInTele_3b, _
                       strInTele_4a, strInTele_4b, _
                       strInTele_5a, strInTele_5b, _
                       strInTele_6a, strInTele_6b, _
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
Public Sub LockNote(ByVal lngIntakeTeleID As Long, ByVal lngSignedBy As Long, ByVal dtDateSigned As Date)
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Flags a row in the tblIntakeTele table marking the row as
'              deleted or undeleted.
'Parameters: lngIntakeTeleID - ID of the row in the table whose value will be
'               updated.
'Returns: Null
'--------------------------------------------------------------------

    ' Declare database object
    Dim obj As EMRDB.CIntakeTeleDB
    
    ' Declare error message string
    Dim strErrMessage As String

    On Error GoTo ErrTrap:

    ' Instantiate database object
    Set obj = CreateObject("EMRDB.CIntakeTeleDB")
    
    ' Run database object
    obj.LockNote lngIntakeTeleID, lngSignedBy, dtDateSigned
    
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
Public Sub UnLockNote(ByVal lngIntakeTeleID As Long, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Flags a row in the tblIntakeTele table marking the row as
'              deleted or undeleted.
'Parameters: lngIntakeTeleID - ID of the row in the table whose value will be
'               updated.
'Returns: Null
'--------------------------------------------------------------------

    ' Declare database object
    Dim obj As EMRDB.CIntakeTeleDB
    
    ' Declare error message string
    Dim strErrMessage As String

    On Error GoTo ErrTrap:

    ' Instantiate database object
    Set obj = CreateObject("EMRDB.CIntakeTeleDB")
    
    ' Run database object
    obj.UnLockNote lngIntakeTeleID, strUserName
    
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
Public Sub Delete(ByVal strUserName As String, ByVal lngIntakeTeleID As Long)
'--------------------------------------------------------------------
'Date: 07/12/2017
'Author: Duane C Orth
'Description:  Flags a row in the tblIntakeTele table marking the row as
'              deleted or undeleted.
'Parameters: lngIntakeTeleID - ID of the row in the table whose value will be
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
    Dim obj As EMRDB.CIntakeTeleDB
    Dim strErrMessage As String
    
    On Error GoTo ErrTrap:

    ' Instanitate DB obj
    Set obj = CreateObject("EMRDB.CIntakeTeleDB")
    
    'Call on valid check
    If Not (obj.FetchByID(lngIntakeTeleID).RecordCount = 0) Then
        If ((obj.FetchByID(lngIntakeTeleID).Fields("fldLockedYN")) = "N") Then
            ' Fire delete method
            Call obj.Delete(strUserName, lngIntakeTeleID)
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
    Optional ByVal lngPatientID As Long, Optional ByVal lngIntakeTeleID As Long, _
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
    If lngIntakeTeleID < 0 Then
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
    
    'If we get here, all is well...
    VerifyData = True

End Function









