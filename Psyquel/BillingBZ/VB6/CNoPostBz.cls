VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 2  'RequiresTransaction
END
Attribute VB_Name = "CNoPostBz"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CNoPostBz
'Date: 09/06/2001
'Author: Eric Pena
'Description:  MTS business object designed to call methods associated
'              with the CNoPostDB class.
'--------------------------------------------------------------------
Option Explicit
Private Const CLASS_NAME As String = "CNoPostBz"
Private Const TABLE_NAME As String = "tblNonPostable"
Public Function Insert(ByVal lngCheckID As Long, ByVal curPostAmount As Currency, ByVal curAmtDisallow As Currency, ByVal curAmtPatient As Currency, _
                        ByVal lngPatientID As Long, ByVal lngProviderID As Long, ByVal lngInsuranceID As Long, ByVal dteDOS As Date, ByVal strCPT As String, _
                        ByVal strReason As String, ByVal lngDupeCheck As Long, ByVal dteAction As Date, ByVal strAction As String, ByVal lngClaimID As Long, _
                        ByVal strDupeCheckNum As String, ByVal dteDupeCheckDate As Date, ByVal strNotes As String, ByVal strUserName As String) As Long
'--------------------------------------------------------------------
'Date: 09/06/2001
'Author: Eric Pena
'Description:  Inserts a row into the tblNonPostable table utilizing a stored procedure.
'Parameters: Each parameter identifies the column value that will be  inserted.
'Returns: ID (Primary Key) of the row inserted.
'--------------------------------------------------------------------
    Dim objNP As BillingDB.CNoPostDB
    
    On Error GoTo ErrTrap:
    
    Set objNP = CreateObject("BillingDB.CNoPostDB")
    Insert = objNP.Insert(lngCheckID, curPostAmount, curAmtDisallow, curAmtPatient, lngPatientID, lngProviderID, lngInsuranceID, dteDOS, strCPT, _
                        strReason, lngDupeCheck, dteAction, strAction, lngClaimID, strDupeCheckNum, dteDupeCheckDate, Trim(strNotes), strUserName)
    Set objNP = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    Set objNP = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Sub Update(ByVal lngNPID As Long, ByVal lngOldCheckID As Long, ByVal curOldAmt As Currency, ByVal lngCheckID As Long, ByVal curPostAmount As Currency, ByVal curAmtDisallow As Currency, ByVal curAmtPatient As Currency, _
                        ByVal lngPatientID As Long, ByVal lngProviderID As Long, ByVal lngInsuranceID As Long, ByVal dteDOS As Date, ByVal strCPT As String, _
                        ByVal strReason As String, ByVal lngDupeCheck As Long, ByVal dteAction As Date, ByVal strAction As String, ByVal lngClaimID As Long, _
                        ByVal strDupeCheckNum As String, ByVal dteDupeCheckDate As Date, ByVal strNotes As String, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 09/06/2001
'Author: Eric Pena
'Description:  Updates a row in the tblNonPostable table utilizing a stored procedure.
'Parameters: Each parameter identifies the column value that will be inserted.
'Returns: Null
'--------------------------------------------------------------------
    Dim objNP As BillingDB.CNoPostDB

    On Error GoTo ErrTrap:
    
    Set objNP = CreateObject("BillingDB.CNoPostDB")
    objNP.Update lngNPID, lngOldCheckID, curOldAmt, lngCheckID, curPostAmount, curAmtDisallow, curAmtPatient, lngPatientID, lngProviderID, lngInsuranceID, _
                        dteDOS, strCPT, strReason, lngDupeCheck, dteAction, strAction, lngClaimID, strDupeCheckNum, dteDupeCheckDate, Trim(strNotes), strUserName
    Set objNP = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    Set objNP = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub
Public Sub Process(ByVal lngNPID As Long, ByVal blnIsRefunded As Boolean, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 09/06/2001
'Author: Eric Pena
'Description:  Processed a row in the tblNonPostable table
'Parameters: lngNPID - PK of the row in the tblNonPostable table to update
'               blnIsRefunded - If true, will mark the row as refunded.  false will mark the row as posted
'Returns: Null
'--------------------------------------------------------------------
    Dim objNP As BillingDB.CNoPostDB
    Dim strErrMessage As String

    On Error GoTo ErrTrap:
    
    Set objNP = CreateObject("BillingDB.CNoPostDB")
    
    If objNP.isProcessed(lngNPID) Then
        strErrMessage = "This entry has already been processed."
        GoTo ErrTrap
    End If
    
    objNP.Process lngNPID, blnIsRefunded, strUserName
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objNP = Nothing
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    Set objNP = Nothing
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub
Public Sub Delete(ByVal lngID As Long, ByVal lngCheckID As Long, ByVal curAmt As Currency, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 09/06/2001
'Author: Eric Pena
'Description:  Deletes a row in the tblNonPostable table
'Parameters: lngID - ID of the row in the table whose value will be deleted.
'Returns: Null
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------
    Dim objNP As BillingDB.CNoPostDB
    Dim strError As String
    
    On Error GoTo ErrTrap:
    
    strError = ""
        
    Set objNP = CreateObject("BillingDB.CNoPostDB")
    objNP.Delete lngID, lngCheckID, curAmt, strUserName
    Set objNP = Nothing

    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objNP = Nothing
    GetObjectContext.SetAbort
    If strError = "" Then strError = Err.Description
    Err.Raise Err.Number, Err.Source, strError
End Sub
Public Function FetchByID(ByVal lngNPID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 09/06/2001
'Author: Eric Pena
'Description:  Returns rows from the tblNonPostable table utilizing a stored procedure
'Parameters: PK for row in tblNonPostable that will be fetched.
'Returns: ADORst
'--------------------------------------------------------------------
    
     Dim objNP As BillingDB.CNoPostDB
     
     On Error GoTo ErrTrap:
     
     
     Set objNP = CreateObject("BillingDB.CNoPostDB")
     Set FetchByID = objNP.FetchByID(lngNPID)
    
    'Signal successful completion
     GetObjectContext.SetComplete
     Set objNP = Nothing
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objNP = Nothing
     GetObjectContext.SetAbort
     Err.Raise Err.Number, Err.Source, Err.Description
 End Function
Public Function FetchByCriteria(ByVal dteStartAdded As Date, ByVal dteEndAdded As Date, ByVal dteStartAction As Date, ByVal dteEndAction As Date, _
                                    ByVal strAction As String, ByVal lngPatientID As Long, ByVal strCheckNum As String, ByVal lngInsuranceID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 09/06/2001
'Author: Eric Pena
'Description:  Returns rows from the tblNonPostable table utilizing a stored procedure
'Parameters: Each parameter identifies the column value that will be filtered.
'Returns: ADORst
'--------------------------------------------------------------------
     
     Dim objNP As BillingDB.CNoPostDB
     
     On Error GoTo ErrTrap:
     
     
     Set objNP = CreateObject("BillingDB.CNoPostDB")
     Set FetchByCriteria = objNP.FetchByCriteria(dteStartAdded, dteEndAdded, dteStartAction, dteEndAction, strAction, lngPatientID, strCheckNum, lngInsuranceID)
    
    'Signal successful completion
     GetObjectContext.SetComplete
     Set objNP = Nothing
     Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objNP = Nothing
     GetObjectContext.SetAbort
     Err.Raise Err.Number, Err.Source, Err.Description
 End Function
