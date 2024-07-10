VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 2  'RequiresTransaction
END
Attribute VB_Name = "CProvAccessBZ"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CProvAccessBZ
'Date: 12/20/2000
'Author: Joshua Lockwood
'Description:  Business object designed to call methods
'               associated with the CProvAccessDB class.
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------
Option Explicit

Private Const CLASS_NAME As String = "CProvAccessDB"
Private Const TABLE_NAME As String = "tblProviderAccess"
Public Function FetchProviderAccessList(ByVal lngUserID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 12/20/2000
'Author: Joshua Lockwood
'Description: Retrieves records from the tblProviderAccess table.
'
'Parameters: userID - owner ID.
'            includeDisabled - also include disabled records on true.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------

    ' Declares database onject
    Dim objProviderAccess As EMRDB.CProvAccessDB
    ' Declares recordset for result returned
    Dim rstSQL As ADODB.Recordset
    
    ' Duh
    On Error GoTo ErrTrap:

    ' Instantiates database onject
    Set objProviderAccess = CreateObject("EMRDB.CProvAccessDB")
    
    ' Instantiates recordset for result returned
    Set rstSQL = objProviderAccess.FetchProviderAccessList(lngUserID, blnIncludeDisabled)
    
    ' Return value set for function
    Set FetchProviderAccessList = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete
    ' Set DB object to NULL
    Set objProviderAccess = Nothing
    ' Exit on success
    Exit Function
    
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objProviderAccess = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function

Public Function FetchPatientByOwner(ByVal lngProviderID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 12/20/2000
'Author: Joshua Lockwood
'Description: Retrieves records from the tblBenefactor table where
'             benefactor's ownerID matches lngProviderID.
'
'Parameters: lngProviderID - owner ID.
'            blnIncludeDisabled - also include disabled records on true.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
    
    ' Declares database onject
    Dim objProviderAccess As EMRDB.CProvAccessDB
    ' Declares recordset for result returned
    Dim rstSQL As ADODB.Recordset
    
    ' Duh
    On Error GoTo ErrTrap:

    ' Instantiates database onject
    Set objProviderAccess = CreateObject("EMRDB.CProvAccessDB")
    
    ' Call fetch method and set recordset returned to rstSQL
    Set rstSQL = objProviderAccess.FetchPatientByOwner(lngProviderID, blnIncludeDisabled)
    
    ' Set recordset to retval
    Set FetchPatientByOwner = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objProviderAccess = Nothing   ' Delete object
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objProviderAccess = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Function FetchPatientOwner(ByVal lngPatientID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 12/20/2000
'Author: Joshua Lockwood
'Description: Retrieves records from the tblProviderAccess table.
'
'Parameters: lngPatientID - patient ID.
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
    
    ' Declares database onject
    Dim objProviderAccess As EMRDB.CProvAccessDB
    ' Declares recordset for result returned
    Dim rstSQL As ADODB.Recordset
    
    ' Duh
    On Error GoTo ErrTrap:

    ' Instantiates database onject
    Set objProviderAccess = CreateObject("EMRDB.CProvAccessDB")
    
    ' Call fetch method and set recordset returned to rstSQL
    Set rstSQL = objProviderAccess.FetchPatientOwner(lngPatientID)
    
    ' Set recordset to retval
    Set FetchPatientOwner = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objProviderAccess = Nothing   ' Delete object
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objProviderAccess = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Function Insert(ByVal strUserName As String, ByVal lngProviderID As Long, ByVal lngColleagueID As Long) As Long
'--------------------------------------------------------------------
'Date: 12/20/2000
'Author: Joshua Lockwood
'Description: Adds new record to the tblBenefactor table.
'
'Parameters: strUserName - for AddedBy.
'            lngProviderID - ID of the owner granting priviledges to a colleague
'            lngColleagueID - ID of the colleague being added to access list
'Returns: Long (ProviderAccessID)
'--------------------------------------------------------------------
    ' Declare DB object (connection)
    Dim objProviderAccess As EMRDB.CProvAccessDB
    ' Declare buffer for err msg
    Dim strErrMessage As String
    ' Set tag for err trap code
    On Error GoTo ErrTrap:
    
    '---------------------
    '???Data Validation???
    '---------------------
    
    ' Instantiate DB obj (connection)
    Set objProviderAccess = CreateObject("EMRDB.CProvAccessDB")
    
    ' Fire Insert method
    Insert = objProviderAccess.Insert(strUserName, lngProviderID, lngColleagueID)
        
    'Signal successful completion
    GetObjectContext.SetComplete
    ' Set DB obj to null
    Set objProviderAccess = Nothing
    ' Exit on success
    Exit Function
    
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    ' Close DB connection
    Set objProviderAccess = Nothing
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Function

Public Sub Update(ByVal lngUserID As Long, ByVal lngNewOwner As Long, ByVal lngPatientID As Long, ByVal strUpdatedBy As String)
'--------------------------------------------------------------------
'Date: 12/20/2000
'Author: Joshua Lockwood
'Description: Updates selected records in tblBenefactor table.
'
'Parameters: lngUserID - ID of current owner granting the transfer.
'            lngNewOwner - Owner of the 'soon-to-be' owner.
'            lngPatientID - ID of the patient which is being transfered.
'Returns: Nothing
'--------------------------------------------------------------------
    ' Declare DB object (connection)
    Dim objProviderAccess As EMRDB.CProvAccessDB
    ' Declare buffer for err msg
    Dim strErrMessage As String
    'for testing
    Dim objPatProv As BenefactorDB.CPatientProviderDB
    Dim TestRet As Long
    
    ' Set tag for err trap code
    On Error GoTo ErrTrap:
    
    '---------------------
    '???Data Validation???
    'Check to see if the future owner has PatientProvider relationship
    '   with patient using Benefactor's 'Exists()' method.
    Set objPatProv = CreateObject("BenefactorDB.CPatientProviderDB")
        
    If Not objPatProv.Exists(lngPatientID, lngNewOwner) Then 'make relationship
        Call objPatProv.Insert(lngPatientID, lngNewOwner, strUpdatedBy)
    End If
    
    Set objPatProv = Nothing
    '---------------------
    
    ' Instantiate database object
    Set objProviderAccess = CreateObject("EMRDB.CProvAccessDB")
    
    ' Run database object
    objProviderAccess.Update lngUserID, lngNewOwner, lngPatientID
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objProviderAccess = Nothing
    Exit Sub

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    
    Set objProviderAccess = Nothing
    
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub

Public Sub Deleted(ByVal strUserName As String, ByVal lngProviderAccessID As Long, ByVal blnDisabledYN As Boolean)
'--------------------------------------------------------------------
'Date: 12/20/2000
'Author: Joshua Lockwood
'Description: 'Deletes' selected record in tblProviderAccess table.  Calls
'               stored procedure uspDelProviderAccess.
'Parameters: strUserName - for 'DeletedBy.
'            lngProviderAccessID - ID of relationship to be deleted.
'            blnDisabledYN - Toggle disabled flag of relation.
'Returns: Nothing
'--------------------------------------------------------------------
    ' Declare DB object (connection)
    Dim objProviderAccess As EMRDB.CProvAccessDB
    ' Declare buffer for err msg
    Dim strErrMessage As String
    ' Set tag for err trap code
    
    On Error GoTo ErrTrap:
    
    ' Instanitate DB obj
    Set objProviderAccess = CreateObject("EMRDB.CProvAccessDB")
    
    ' Fire delete method
    Call objProviderAccess.Deleted(strUserName, lngProviderAccessID, blnDisabledYN)
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objProviderAccess = Nothing
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objProviderAccess = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub

