VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CPatientAlertBZ"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CAlertBZ
'Date: 08/31/2000
'Author: David Nichol and Joshua Lockwood
'Description:  MTS business object designed to call methods
'              associated with the CPatientAlertDB class.
'--------------------------------------------------------------------
'Revision History:
'--------------------------------------------------------------------
Option Explicit

Private Const CLASS_NAME As String = "CAlertBz"
Private Const PATIENTALERT_TABLE As String = "tblPatientAlert"
Public Function FetchByID(ByVal lngPatientAlertID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 08/31/2000
'Author: Dave Nichol and Joshua Lockwood
'Description:  Retrieves records from the tblPatientAlert table.
'Parameters: lngPatientAlertID - ID number of the record to retrieve, which
'               is primary key of table.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------

    ' Declares database onject
    Dim objPatientAlert As EMRDB.CPatientAlertDB
    ' Declares recordset for result returned
    Dim rstSQL As ADODB.Recordset
    
    ' Duh
    On Error GoTo ErrTrap:

    ' Instantiates database onject
    Set objPatientAlert = CreateObject("EMRDB.CPatientAlertDB")
    
    ' Instantiates recordset for result returned
    Set rstSQL = objPatientAlert.FetchByID(lngPatientAlertID)
    
    ' Return value set for function
    Set FetchByID = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    ' Set DB object to NULL
    Set objPatientAlert = Nothing
    
    ' Exit on success
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objPatientAlert = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function

Public Function FetchByAlert(ByVal strAlertText As String, Optional blnIncludeDisabled As Boolean = False) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 08/31/2000
'Author: Dave Nichol and Joshua Lockwood
'Description:  Retrieves records from the tblPatientAlert table.
'Parameters: blnIncludeDisabled - Optional parameter that identifies if
'               records flagged as 'Disabled' or 'De-activated' are to be
'               included in the record set. The default value is False.
'            strAlertText - Text of actual alert.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
    
    Dim objPatientAlert As EMRDB.CPatientAlertDB
    Dim rstSQL As ADODB.Recordset

    On Error GoTo ErrTrap:

    Set objPatientAlert = CreateObject("EMRDB.CPatientAlertDB")
    Set rstSQL = objPatientAlert.FetchByAlert(strAlertText, blnIncludeDisabled)
    Set FetchByAlert = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objPatientAlert = Nothing
    Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objPatientAlert = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function


Public Function FetchByPatient(ByVal lngPatientID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 08/31/2000
'Author: Dave Nichol and Joshua Lockwood
'Description:  Retrieves records from the tblPatientAlert table.
'Parameters: blnIncludeDisabled - Optional parameter that identifies if
'               records flagged as 'Disabled' or 'De-activated' are to be
'               included in the record set. The default value is False.
'            lngPatientID - ID number of the patient whose alerts to retrieve.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    Dim objPatientAlert As EMRDB.CPatientAlertDB
    Dim rstSQL As ADODB.Recordset

    On Error GoTo ErrTrap:

    Set objPatientAlert = CreateObject("EMRDB.CPatientAlertDB")
    Set rstSQL = objPatientAlert.FetchByPatient(lngPatientID, blnIncludeDisabled)
    Set FetchByPatient = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objPatientAlert = Nothing
    Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objPatientAlert = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function

Public Function Insert(ByVal lngPatientID As Long, ByVal strAlertText As String, ByVal strUserName As String) As Long '--------------------------------------------------------------------
'Date: 08/31/2000
'Author: David Nichol and Joshua Lockwood
'Description:  Inserts a row into the tblPatientAlert table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'Returns: ID (Primary Key) of the row inserted.
'--------------------------------------------------------------------
    
    ' Declare DB object (connection)
    Dim objPatientAlert As EMRDB.CPatientAlertDB
    ' Declare buffer for err msg
    Dim strErrMessage As String
    ' Set tag for err trap code
    On Error GoTo ErrTrap:
    
    ' Validate data being passed
    If Not VerifyData(lngPatientID, strAlertText, strUserName, strErrMessage) Then GoTo ErrTrap
    ' Instantiate DB obj (connection)
    Set objPatientAlert = CreateObject("EMRDB.CPatientAlertDB")
    ' Fire Insert method
    Insert = objPatientAlert.Insert(lngPatientID, strAlertText, strUserName)
    'Signal successful completion
    GetObjectContext.SetComplete
    ' Set DB obj to null
    Set objPatientAlert = Nothing
    ' Exit on success
    Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    ' Close DB connection
    Set objPatientAlert = Nothing
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Function

Public Function Update(ByVal lngPatientAlertID As Long, ByVal lngPatientID As Long, ByVal strAlertText As String, ByVal strUserName As String) As Long
'--------------------------------------------------------------------
'Date: 08/31/2000
'Author: David Nichol and Joshua Lockwood
'Description:  Updates a row in the tblPatientAlert table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'Returns: Null
'--------------------------------------------------------------------

    
    Dim objPatientAlert As EMRDB.CPatientAlertDB
    Dim strErrMessage As String

    On Error GoTo ErrTrap:

    If Not VerifyData(lngPatientID, strAlertText, strUserName, strErrMessage, lngPatientAlertID) Then GoTo ErrTrap
    Set objPatientAlert = CreateObject("EMRDB.CPatientAlertDB")
    objPatientAlert.Update lngPatientAlertID, lngPatientID, strAlertText, strUserName
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objPatientAlert = Nothing
    Exit Function

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    Set objPatientAlert = Nothing
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Function

Public Sub Deleted(ByVal lngPatientAlertID As Long, ByVal blnDisabled As Boolean, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 08/31/2000
'Author: David Nichol and Joshua Lockwood
'Description:  Flags a row in the tblPatientAlert table marking the row as
'              deleted or undeleted.
'Parameters: blnDisabledYN - Boolean value identifying if the record is to
'               be deleted (True) or undeleted (False).
'            lngPatientAlertID - ID of the row in the table whose value will be
'               updated.
'            strUserName - Login name of the user responsible for
'               marking the row as deleted.
'Returns: Null
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declare DB obj
    Dim objPatientAlert As EMRDB.CPatientAlertDB
    
    On Error GoTo ErrTrap:

    ' Instanitate DB obj
    Set objPatientAlert = CreateObject("EMRDB.CPatientAlertDB")
    
    ' Fire delete method
    Call objPatientAlert.Deleted(lngPatientAlertID, blnDisabled, strUserName)
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objPatientAlert = Nothing
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objPatientAlert = Nothing
    GetObjectContext.SetAbort
    
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub

Private Function VerifyData(ByVal lngPatientID As Long, ByVal strAlertText As String, _
    ByVal strUserName As String, ByRef strErrMessage As String, _
    Optional ByVal lngPatientAlertID As Long) As Boolean
'--------------------------------------------------------------------
'Date: 08/31/2000
'Author: David Nichol and Joshua Lockwood
'Description:  Verifies all required data has been provided by the user.
'Parameters:  The values to be checked.
'Returns: Boolean value identifying if all criteria has been satisfied.
'--------------------------------------------------------------------
    ' Check to see if username provided is not null
    If Trim(strUserName) = "" Then
        strErrMessage = "A user name is required."
        VerifyData = False
        Exit Function
    End If
    
    ' Verify that ID number is valid; must be >= 0 (1st index==1)
    If lngPatientID < 0 Then
        strErrMessage = "PatientID number provided is not valid."
        VerifyData = False
        Exit Function
    End If
    
    ' Make sure that alert string is not empty
    If Trim(strAlertText) = "" Then
        strErrMessage = "No text was provided for the alert."
        VerifyData = False
        Exit Function
    End If
  
     ' Verify that ID number is valid; must be >= 0 (1st index==1)
    If lngPatientAlertID < 0 Then
        strErrMessage = "AlertID number provided is not valid."
        VerifyData = False
        Exit Function
    End If
  
   'If we get here, all is well...
    VerifyData = True

End Function


