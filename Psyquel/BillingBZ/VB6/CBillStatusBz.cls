VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 2  'RequiresTransaction
END
Attribute VB_Name = "CBillStatusBz"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CBillStatusBz
'Date: 08/30/2000
'Author: Eric Pena
'Description:  MTS business object designed to call methods associated
'              with the CBillStatusDB class.
'--------------------------------------------------------------------
Option Explicit
Private Const CLASS_NAME As String = "CBillStatusBz"
Private Const TABLE_NAME As String = "tblBillStatus"
Public Function Insert(ByVal strDescription As String) As Long
'--------------------------------------------------------------------
'Date: 08/30/2000
'Author: Eric Pena
'Description:  Inserts a row into the tblBillStatus table utilizing a stored procedure.
'Parameters: Each parameter identifies the column value that will be  inserted.
'Returns: ID (Primary Key) of the row inserted.
'--------------------------------------------------------------------
    Dim objBS As BillingDB.CBillStatusDB
    Dim strErrMessage As String
    On Error GoTo ErrTrap:
    
    If Not VerifyData(0, strDescription, strErrMessage) Then GoTo ErrTrap
    Set objBS = CreateObject("BillingDB.CBillStatusDB")
    Insert = objBS.Insert(strDescription)
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objBS = Nothing
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    Set objBS = Nothing
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Function
Public Sub Update(ByVal lngID As Long, ByVal strDescription As String)
'--------------------------------------------------------------------
'Date: 08/30/2000
'Author: Eric Pena
'Description:  Updates a row in the tblBillStatus table utilizing a stored procedure.
'Parameters: Each parameter identifies the column value that will be inserted.
'Returns: Null
'--------------------------------------------------------------------
    Dim objBS As BillingDB.CBillStatusDB
    Dim strErrMessage As String

    On Error GoTo ErrTrap:
    
    If Not VerifyData(lngID, strDescription, strErrMessage) Then GoTo ErrTrap
    Set objBS = CreateObject("BillingDB.CBillStatusDB")
    objBS.Update lngID, strDescription
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objBS = Nothing
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    Set objBS = Nothing
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub
Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Long, ByVal strDeletedBy As String)
'--------------------------------------------------------------------
'Date: 08/30/2000
'Author: Eric Pena
'Description:  Flags a row in the tblBillStatus table marking the row as deleted or undeleted.
'Parameters: blnDeleted - Boolean value identifying if the record is to be deleted (True) or undeleted (False).
'            lngID - ID of the row in the table whose value will be updated.
'            strUserName - Login name of the user responsible for marking the row as deleted.
'Returns: Null
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------
    Dim objBS As BillingDB.CBillStatusDB
    
    On Error GoTo ErrTrap:
    
    Set objBS = CreateObject("BillingDB.CBillStatusDB")
    
    Call objBS.Deleted(blnDeleted, lngID, strDeletedBy)

    'Signal successful completion
    GetObjectContext.SetComplete

    Set objBS = Nothing

    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objBS = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub
Private Function VerifyData(ByVal lngID As Long, ByVal strDescription As String, ByRef strErrMessage As String) As Boolean
'--------------------------------------------------------------------
'Date: 08/30/2000
'Author: Eric Pena
'Description:  Verifies all required data has been provided by the user.
'Parameters:  The values to be checked.
'Returns: Boolean value identifying if all criteria has been satisfied.
'--------------------------------------------------------------------
    If Trim(strDescription) = "" Then
        strErrMessage = "A Description is required."
        VerifyData = False
        Exit Function
    End If
    
    'Check for existance only when inserting new data
    If lngID = 0 And Exists(strDescription) Then
        strErrMessage = "Bill Status " & strDescription & " already exists."
        VerifyData = False
        Exit Function
    End If
    
    'If we get here, all is well...
    VerifyData = True

End Function
Public Function Exists(ByVal strDescription As String) As Boolean
'--------------------------------------------------------------------
'Date: 08/30/2000
'Author: Eric Pena
'Description:  Determines if the Bill status identical to the strDescription parameter already exists in the table.
'Parameters: strBusinessName - Business name to be checked
'Returns: True if the record exists, false otherwise
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------
'
    Dim objBS As BillingDB.CBillStatusDB
    Dim blnExists As Boolean

    On Error GoTo ErrTrap:
    
    Set objBS = CreateObject("BillingDB.CBillStatusDB")
    blnExists = objBS.Exists(strDescription)

    Exists = blnExists

    Set objBS = Nothing

    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Function

ErrTrap:
   'Signal incompletion and raise the error to the calling environment.
    Set objBS = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
 Public Function Fetch(Optional blnIncludeDisabled As Boolean = False) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 08/30/2000
'Author: Eric Pena
'Description:  Returns rows from the tblBillStatus table utilizing a stored procedure
'Parameters: blnIncludeDisabled - determines if disabled records will be returned
'Returns: ADORst
'--------------------------------------------------------------------
     
     Dim objBS As BillingDB.CBillStatusDB
     
     On Error GoTo ErrTrap:
     
     
     Set objBS = CreateObject("BillingDB.CBillStatusDB")
     Set Fetch = objBS.Fetch(blnIncludeDisabled)
    
    'Signal successful completion
     GetObjectContext.SetComplete
     Set objBS = Nothing
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objBS = Nothing
     GetObjectContext.SetAbort
     Err.Raise Err.Number, Err.Source, Err.Description
 End Function
 Public Function FetchByID(lngID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 08/30/2000
'Author: Eric Pena
'Description:  Returns rows from the tblBillStatus table utilizing a stored procedure
'Parameters: lngID - the PK of the row to return
'Returns: ADORst
'--------------------------------------------------------------------
     
     Dim objBS As BillingDB.CBillStatusDB
     
     On Error GoTo ErrTrap:
     
     
     Set objBS = CreateObject("BillingDB.CBillStatusDB")
     Set FetchByID = objBS.FetchByID(lngID)
    
    'Signal successful completion
     GetObjectContext.SetComplete
     Set objBS = Nothing
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objBS = Nothing
     GetObjectContext.SetAbort
     Err.Raise Err.Number, Err.Source, Err.Description
 End Function








