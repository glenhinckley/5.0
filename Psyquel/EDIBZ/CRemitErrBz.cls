
'-------------------------------------------------------------------------------------
'Date: 04/20/2009
'Class Name: RemitErrBz
'Author: Duane C Orth
'Description:   COM object designed to host methods associated with electronic
'               checks.
'--------------------------------------------------------------------------------------
Option Explicit On


Imports ADODB.CommandTypeEnum
Imports ADODB.CompareEnum
Imports ADODB.ParameterAttributesEnum



Imports ADODB.DataTypeEnum
Imports ADODB.ParameterDirectionEnum
Imports ADODB.ExecuteOptionEnum

Imports ADODB.CursorTypeEnum
Imports ADODB.CursorLocationEnum
Imports ADODB.CursorOptionEnum
Imports ADODB.LockTypeEnum
Imports Psyquel.BusinessRules.CoreLibraryIII
' Lydia Orth 

Public Class CRemitErrBz

    Private Const CLASS_NAME As String = "CRemitErrBz"
    Private Const TABLE_NAME As String = "tblRemittanceErrorCodes"




    Private _ConnectionString As String = String.Empty
    Private _AppPath As String = String.Empty
    Private _EDIOutput As String = String.Empty
    Private _MachineName As String = String.Empty
    Private _UserLoginName As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property





    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False, Optional ByVal strWhere As String = "", Optional ByVal strOrderBy As String = "") As ADODB.Recordset
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
        Dim objRemitErr As New EDIDB.CRemitErrDB

        rstSQL = objRemitErr.Fetch(blnIncludeDisabled, strWhere, strOrderBy)
        Fetch = rstSQL

        objRemitErr = Nothing


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

        Dim objRemitErr As New EDIDB.CRemitErrDB
        Dim strErrMessage As String



        If Not VerifyData(0, strErrorCode, strErrMessage) Then Exit Function

        Insert = objRemitErr.Insert(strErrorCode, strDescription, blnPostError, blnPostAmount, blnCoInsAmount, lngTxTypeID, lngBillStatusID, strAddedBy)
        objRemitErr = Nothing

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

        Dim objRemitErr As New EDIDB.CRemitErrDB
        Dim strErrMessage As String




        If Not VerifyData(lngID, strErrorCode, strErrMessage) Then
            Exit Sub
        End If

        objRemitErr.Update(lngID, strErrorCode, strDescription, blnPostError, blnPostAmount, blnCoInsAmount, lngTxTypeID, lngBillStatusID, strChangedBy)
        objRemitErr = Nothing


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

    Private Function Exists(ByVal strName As String) As Boolean
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
        Dim objRemitErr As New EDIDB.CRemitErrDB

        Exists = objRemitErr.Exists(strName)
        objRemitErr = Nothing

    End Function


End Class
