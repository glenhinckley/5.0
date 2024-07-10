VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CRemitErrBz"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'-------------------------------------------------------------------------------------
'Date: 04/20/2009
'Class Name: RemitErrBz
'Author: Duane C Orth
'Description:   COM object designed to host methods associated with electronic
'               checks.
'--------------------------------------------------------------------------------------
Option Explicit

Private Const CLASS_NAME As String = "CRemitErrBz"
Private Const TABLE_NAME As String = "tblRemittanceErrorCodes"

Public Function Fetch(Optional blnIncludeDisabled As Boolean = False, _
                      Optional ByVal strWhere As String, _
                      Optional ByVal strOrderBy As String) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 04/20/2009
'Author: Duane C Orth
'Description:  Retrieves records from the tblRemittanceErrorCodes table.
'Parameters: blnIncludeDisabled - Optional parameter that identifies if
'               records flagged as 'Disabled' or 'De-activated' are to be
'               included in the record set. The default value is False.
'            strWhere - Optional 'Where' clause of the SQL statement by
'               which records will be filtered.  If the parameter is not
'               supplied, all records will be retrieved.
'            strOrderBy - Optional 'Order By' clause in which retrieved
'               records will be sorted.  The default is set to the
'               'fldName' column
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    
    Dim rstSQL As ADODB.Recordset
    Dim objRemitErr As EDIDB.CRemitErrDB
    
    On Error GoTo ErrTrap:
    
    Set objRemitErr = CreateObject("EDIDB.CRemitErrDB")
    Set rstSQL = objRemitErr.Fetch(blnIncludeDisabled, strWhere, strOrderBy)
    Set Fetch = rstSQL
    
    Set objRemitErr = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Function

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objRemitErr = Nothing
    Set rstSQL = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function

Public Function Insert(ByVal strErrorCode As String, _
                       ByVal strDescription As String, _
                       ByVal blnPostError As Boolean, _
                       ByVal blnPostAmount As Boolean, _
                       ByVal blnCoInsAmount As Boolean, _
                       ByVal lngTxTypeID As Long, _
                       ByVal lngBillStatusID As Long, _
                       ByVal strAddedBy As String) As Long
'--------------------------------------------------------------------
'Date: 04/20/2009
'Author: Duane C Orth
'Description:  Inserts a row into the tblRemittanceErrorCodes table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'Returns: ID (Primary Key) of the row inserted.
'--------------------------------------------------------------------
    
    Dim objRemitErr As EDIDB.CRemitErrDB
    Dim strErrMessage As String
    
    On Error GoTo ErrTrap:
    
    If Not VerifyData(0, strErrorCode, strErrMessage) Then GoTo ErrTrap
    
    Set objRemitErr = CreateObject("EDIDB.CRemitErrDB")
    Insert = objRemitErr.Insert(strErrorCode, strDescription, blnPostError, blnPostAmount, blnCoInsAmount, lngTxTypeID, lngBillStatusID, strAddedBy)
    Set objRemitErr = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    Set objRemitErr = Nothing
    GetObjectContext.SetAbort
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Function

Public Sub Update(ByVal lngID As Long, _
                  ByVal strErrorCode As String, _
                  ByVal strDescription As String, _
                  ByVal blnPostError As Boolean, _
                  ByVal blnPostAmount As Boolean, _
                  ByVal blnCoInsAmount As Boolean, _
                  ByVal lngTxTypeID As Long, _
                  ByVal lngBillStatusID As Long, _
                  ByVal strChangedBy As String)
'--------------------------------------------------------------------
'Date: 04/20/2009
'Author: Duane C Orth
'Description:  Updates a row in the tblRemittanceErrorCodes table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'Returns: Null
'--------------------------------------------------------------------
    
    Dim objRemitErr As EDIDB.CRemitErrDB
    Dim strErrMessage As String

    On Error GoTo ErrTrap:

    
    If Not VerifyData(lngID, strErrorCode, strErrMessage) Then
        GoTo ErrTrap
    End If
    
    Set objRemitErr = CreateObject("EDIDB.CRemitErrDB")
    Call objRemitErr.Update(lngID, strErrorCode, strDescription, blnPostError, blnPostAmount, blnCoInsAmount, lngTxTypeID, lngBillStatusID, strChangedBy)
    Set objRemitErr = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    Set objRemitErr = Nothing
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub

Private Function VerifyData(ByVal lngID As Long, ByVal strName As String, ByRef strErrMessage As String) As Boolean
'--------------------------------------------------------------------
'Date: 04/20/2009
'Author: Duane C Orth
'Description:  Verifies all required data has been provided by the user.
'Parameters:  The values to be checked.
'Returns: Boolean value identifying if all criteria has been satisfied.
'--------------------------------------------------------------------

    If Trim(strName) = "" Then
        strErrMessage = "Error Code is required."
        VerifyData = False
        Exit Function
    End If
    
    'Check for existance only when inserting new data
    If lngID = 0 And Exists(strName) Then
        strErrMessage = "Error Code '" & strName & "' already exists."
        VerifyData = False
        Exit Function
    End If
    
    'If we get here, all is well...
    VerifyData = True

End Function

Private Function Exists(strName As String) As Boolean
'--------------------------------------------------------------------
'Date: 04/20/2009
'Author: Duane C Orth
'Description:  Determines if the Insurance Name identical to the
'              strName parameter already exists in the table.
'Parameters: strName - Insurance name to be checked
'Returns: True if the name exists, false otherwise
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------
'
    Dim objRemitErr As EDIDB.CRemitErrDB

    On Error GoTo ErrTrap:
    
    Set objRemitErr = CreateObject("EDIDB.CRemitErrDB")
    Exists = objRemitErr.Exists(strName)
    Set objRemitErr = Nothing
    'Signal successful completion
    GetObjectContext.SetComplete
    Exit Function

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objRemitErr = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
    Exists = False
End Function


