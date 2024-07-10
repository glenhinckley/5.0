
'------------------------------------------------------------------------------------
'Class Name: CTxGroupBz
'Date: 08/30/00
'Author: Juan Castro
'Denis J Finkel
'Description:   Business object designed to   methods associated with
'               CTxGroupDB class.
'------------------------------------------------------------------------------------
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

Public Class CTxGroupBz



    Private _ConnectionString As String = String.Empty



    Private Const CLASS_NAME As String = "CTxGroupBz"
    Private Const TABLE_NAME As String = "tblTransactionGroup"

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property






    Public Function Insert(ByVal strGroupName As String, ByVal blnNoteOnlyYN As Boolean, ByVal strAddedBy As String) As Long
        '------------------------------------------------------------------------------------
        'Date: 08/30/00
        'Author: Juan Castro
        'Description:   Inserts a row in to tblTransactionGroup table utilizing
        '               a stored procedure.
        'Parameters:    Each parameter identifies the column value that will be inserted.
        'Returns:       ID (Primary Key) of the row inserted
        '------------------------------------------------------------------------------------

        Dim objTxGroup As New PostingDB.CTxGroupDB
        Dim strErrMessage As String

        objTxGroup.ConnectionString = _ConnectionString

        '  function to verify data inputed if false goes to ErrTrap
        'if True   Insert function in PostingDB object
        If Not VerifyData(0, strGroupName, strErrMessage) Then Exit Function
        Insert = objTxGroup.Insert(strGroupName, blnNoteOnlyYN, strAddedBy)


        objTxGroup = Nothing


    End Function

    Private Function VerifyData(ByVal lngTxGroupID As Long, ByVal strGroupName As String, ByVal strErrMessage As String) As Boolean
        '------------------------------------------------------------------------------------
        'Date: 08/31/00
        'Author: Juan Castro
        'Description:   Verifies that all required data has been provided by the user.
        'Parameters:    Values to be verified.
        'Returns:       Boolean value identifying if all required criteria has been satisfied
        '------------------------------------------------------------------------------------

        If Trim(strGroupName) = "" Then
            strErrMessage = "A Group Name is required."
            VerifyData = False
            Exit Function
        ElseIf lngTxGroupID = 0 And Exists(strGroupName) Then
            strErrMessage = "The Group Name " & strGroupName & " already exists."
            VerifyData = False
            Exit Function
        Else
            VerifyData = True
        End If

    End Function

    Public Function Exists(ByVal strGroupName As String) As Boolean
        '------------------------------------------------------------------------------------
        'Date: 08/31/00
        'Author: Juan Castro
        'Description:   Determines if the Transaction Group Name already exists in the table
        'Parameters:    strGroupName - Transaction Group Name to be checked
        'Returns:       True if the name exists, false otherwise
        '------------------------------------------------------------------------------------

        Dim objTxGroup As New PostingDB.CTxGroupDB
        Dim blnExists As Boolean
        objTxGroup.ConnectionString = _ConnectionString

        blnExists = objTxGroup.Exists(strGroupName)

        Exists = blnExists

        objTxGroup = Nothing



    End Function

    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 08/31/00
        'Author: Juan Castro
        'Description:   Retrieves records from the tblTransactionGroup table utilizing a stored procedure.
        'Parameters:    blnIncludeDisabled- determines if disabled records will be returned.
        'Returns: ADODB.Recordset
        '-------------------------------------------------------------------------------------

        Dim objTxGroup As New PostingDB.CTxGroupDB
        objTxGroup.ConnectionString = _ConnectionString
        Fetch = objTxGroup.Fetch(blnIncludeDisabled)

        objTxGroup = Nothing



    End Function

    Public Sub Update(ByVal lngTxGroupID As Long, ByVal strGroupName As String, ByVal blnNoteOnlyYN As Boolean)
        '----------------------------------------------------------------------------------------
        'Date: 09/01/00
        'Author: Juan Castro
        'Description:   Updates a row in the tblTransactionGroup table utilizing a stored procedure.
        'Parameters:    Each parameter identifies the column value that will be inserted.
        'Returns:       Null
        '----------------------------------------------------------------------------------------
        Dim objTxGroup As New PostingDB.CTxGroupDB
        Dim strErrMessage As String

        objTxGroup.ConnectionString = _ConnectionString
        If Not VerifyData(lngTxGroupID, strGroupName, strErrMessage) Then
            Exit Sub
        End If

        objTxGroup.Update(lngTxGroupID, strGroupName, blnNoteOnlyYN)

        'Signal successful completion
        objTxGroup = Nothing



    End Sub

    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngTxGroupID As Long, ByVal strDisabledBy As String)
        '------------------------------------------------------------------------------------
        'Date: 09/05/00
        'Author: Juan Castro
        'Description:   Flags a row in the tblTransactionGroup table marking the row as disabled or undisabled.
        'Parameters:    blnDeleted - Boolen value identifying if the record is to be deleted (True)
        '                   or undeleted (False).
        '               lngTxGroupID - ID of the row in the table whose value will be updated.
        '               strDisabledBy - Login name of the user responsible for marking the row as deleted.
        'Returns:       Null
        '------------------------------------------------------------------------------------

        Dim objTxGroup As New PostingDB.CTxGroupDB

        objTxGroup.ConnectionString = _ConnectionString

        objTxGroup.Deleted(blnDeleted, lngTxGroupID, strDisabledBy)




    End Sub

    Public Function FetchByID(ByVal lngID As Long) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 08/31/00
        'Author: Juan Castro
        'Description:   Retrieves records from the tblTransactionGroup table utilizing a stored procedure.
        'Parameters:    blnIncludeDisabled- determines if disabled records will be returned.
        'Returns: ADODB.Recordset
        '-------------------------------------------------------------------------------------

        Dim objTxGroup As New PostingDB.CTxGroupDB

        objTxGroup.ConnectionString = _ConnectionString
        FetchByID = objTxGroup.FetchByID(lngID)


    End Function


End Class
