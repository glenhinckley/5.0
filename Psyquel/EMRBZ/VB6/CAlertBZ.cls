VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 2  'RequiresTransaction
END
Attribute VB_Name = "CAlertBZ"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CAlertBZ
'Date: 08/31/2000
'Author: David Nichol and Joshua Lockwood
'Description:  MTS business object designed to call methods
'              associated with the CAlertDB class.
'--------------------------------------------------------------------
'
'--------------------------------------------------------------------
Option Explicit

Private Const CLASS_NAME As String = "CAlertBz"
Private Const PATIENTALERT_TABLE As String = "tblPatientAlert"
Public Function FetchByID(ByVal lngUserID As Long, ByVal lngPatientAlertID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 08/31/2000
'Author: Dave Nichol and Joshua Lockwood
'Description:  Retrieves records from the tblPatientAlert table.
'Parameters: blnIncludeDisabled - Optional parameter that identifies if
'               records flagged as 'Disabled' or 'De-activated' are to be
'               included in the record set. The default value is False.
'            lngPatientAlertID - ID number of the record to retrieve, which
'               is primary key of table.
'            lngUserID - UserID of user calling method.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declares database onject
    Dim objAlert As EMRDB.CAlertDB
    ' Declares recordset for result returned
    Dim rstSQL As ADODB.Recordset
    
    ' Duh
    On Error GoTo ErrTrap:

    ' Instantiates database onject
    Set objAlert = CreateObject("EMRDB.CAlertDB")
    
    ' Instantiates recordset for result returned
    Set rstSQL = objAlert.FetchByID(lngUserID, lngPatientAlertID, blnIncludeDisabled)
    
    ' Return value set for function
    Set FetchByID = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    ' Set DB object to NULL
    Set objAlert = Nothing
    
    ' Exit on success
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objAlert = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function

Public Function FetchByAlert(ByVal lngUserID As Long, ByVal lngAlertID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 08/31/2000
'Author: Dave Nichol and Joshua Lockwood
'Description:  Retrieves records from the tblPatientAlert table.
'Parameters: blnIncludeDisabled - Optional parameter that identifies if
'               records flagged as 'Disabled' or 'De-activated' are to be
'               included in the record set. The default value is False.
'            lngAlertID - ID number of the record to retrieve based on the alertID.
'            lngUserID - UserID of user calling method.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    
    Dim objAlert As EMRDB.CAlertDB
    Dim rstSQL As ADODB.Recordset

    On Error GoTo ErrTrap:

    Set objAlert = CreateObject("EMRDB.CAlertDB")
    Set rstSQL = objAlert.FetchByAlert(lngUserID, lngAlertID, blnIncludeDisabled)
    Set FetchByAlert = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objAlert = Nothing
    Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objAlert = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function


Public Function FetchByPatient(ByVal lngUserID As Long, ByVal lngPatientID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 08/31/2000
'Author: Dave Nichol and Joshua Lockwood
'Description:  Retrieves records from the tblPatientAlert table.
'Parameters: blnIncludeDisabled - Optional parameter that identifies if
'               records flagged as 'Disabled' or 'De-activated' are to be
'               included in the record set. The default value is False.
'            lngPatientID - ID number of the patient whose alerts to retrieve.
'            lngUserID - UserID of user calling method.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    Dim objAlert As EMRDB.CAlertDB
    Dim rstSQL As ADODB.Recordset

    On Error GoTo ErrTrap:

    Set objAlert = CreateObject("EMRDB.CAlertDB")
    Set rstSQL = objAlert.FetchByPatient(lngUserID, lngPatientID, blnIncludeDisabled)
    Set FetchByPatient = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objAlert = Nothing
    Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objAlert = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function

Public Function Insert(ByVal strUserName As String, ByVal lngPatientID As Long, ByVal lngAlertID As Long) As Long
'--------------------------------------------------------------------
'Date: 08/31/2000
'Author: David Nichol and Joshua Lockwood
'Description:  Inserts a row into the tblPatientAlert table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'Returns: ID (Primary Key) of the row inserted.
'--------------------------------------------------------------------
    
    ' Declare DB object (connection)
    Dim objAlert As EMRDB.CAlertDB
    ' Declare buffer for err msg
    Dim strErrMessage As String
    ' Set tag for err trap code
    On Error GoTo ErrTrap:
    
    ' Validate data being passed
    If Not VerifyData(strUserName, lngPatientID, lngAlertID, strErrMessage) Then GoTo ErrTrap
    ' Instantiate DB obj (connection)
    Set objAlert = CreateObject("EMRDB.CAlertDB")
    ' Fire Insert method
    Insert = objAlert.Insert(strUserName, lngPatientID, lngAlertID)
    'Signal successful completion
    GetObjectContext.SetComplete
    ' Set DB obj to null
    Set objAlert = Nothing
    ' Exit on success
    Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    ' Close DB connection
    Set objAlert = Nothing
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Function

Public Sub Update(ByVal lngPatientAlertID As Long, ByVal lngPatientID As Long, ByVal lngAlertID As Long, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 08/31/2000
'Author: David Nichol and Joshua Lockwood
'Description:  Updates a row in the tblPatientAlert table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'Returns: Null
'--------------------------------------------------------------------

    
    Dim objAlert As EMRDB.CAlertDB
    Dim strErrMessage As String

    On Error GoTo ErrTrap:

    If Not VerifyData(strUserName, lngPatientID, lngAlertID, strErrMessage) Then GoTo ErrTrap
    Set objAlert = CreateObject("EMRDB.CAlertDB")
    objAlert.Update lngPatientAlertID, lngPatientID, lngAlertID, strUserName
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objAlert = Nothing
    Exit Sub

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    Set objAlert = Nothing
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub

Public Sub Deleted(ByVal strUserName As String, ByVal lngPatientAlertID As Long, ByVal blnDisabledYN As Boolean)
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
    Dim objAlert As EMRDB.CAlertDB
    
    On Error GoTo ErrTrap:

    ' Instanitate DB obj
    Set objAlert = CreateObject("EMRDB.CAlertDB")
    
    ' Fire delete method
    Call objAlert.Deleted(strUserName, lngPatientAlertID, blnDisabledYN)
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objAlert = Nothing
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objAlert = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub

Private Function VerifyData(ByVal strUserName As String, ByVal lngPatientID As Long, _
    ByVal lngAlertID As Long, ByRef strErrMessage As String) As Boolean
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
    If lngPatientID <= 0 Then
        strErrMessage = "PatientID number provided is not valid."
        VerifyData = False
        Exit Function
    End If
    
    ' Verify that ID number is valid; must be >= 0 (1st index==1)
    If lngAlertID <= 0 Then
        strErrMessage = "AlertID number provided is not valid."
        VerifyData = False
        Exit Function
    End If
  
    'If we get here, all is well...
    VerifyData = True

End Function


