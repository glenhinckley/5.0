
'-----------------------------------------------------------------------------------
'Class Name: CTxTypeBz
'Date: 09/01/00
'Author: Juan Castro
'Denis J Finkel
'Description:   Business Object designed to   methods associated with
'               CTxTypeDB class.
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

Public Class CTxTypeBz
    Private _ConnectionString As String = String.Empty

    Private Const CLASS_NAME As String = "CTxTypeBz"
    Private Const TABLE_NAME As String = "tblTransactionType"


    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property




    Public Function Insert(ByVal strTypeName As String, ByVal lngTxGroupID As Long, _
                           ByVal strCharge As String, ByVal blnQBillYN As Boolean, ByVal blnQWareYN As Boolean, ByVal blnSystemYN As Boolean, _
                           ByVal strProvAction As String, ByVal strPsyAction As String, ByVal strExtDesc As String, ByVal strAddedBy As String) As Long
        '------------------------------------------------------------------------------------
        'Date: 09/01/00
        'Author: Juan Castro
        'Description:   Inserts a row in to tblTransactionType table utilizing
        '               a stored procedure.
        'Parameters:    Each parameter identifies the column value that will be inserted.
        'Returns:       ID (Primary Key) of the row inserted
        '------------------------------------------------------------------------------------

        Dim objTxType As New PostingDB.CTxTypeDB
        Dim strErrMessage As String
        objTxType.ConnectionString = _ConnectionString


        '  function to verify data inputed if false goes to ErrTrap
        'if True   Insert function in PostingDB object
        If Not VerifyData(0, strTypeName, lngTxGroupID, strErrMessage) Then Exit Function
        Insert = objTxType.Insert(strTypeName, lngTxGroupID, strCharge, blnQBillYN, blnQWareYN, blnSystemYN, strProvAction, strPsyAction, strExtDesc, strAddedBy)


        objTxType = Nothing


    End Function

    Private Function VerifyData(ByVal lngTxTypeID As Long, ByVal strTypeName As String, ByVal lngTxGroupID As Long, _
                               ByVal strErrMessage As String) As Boolean
        '------------------------------------------------------------------------------------
        'Date: 09/01/00
        'Author: Juan Castro
        'Description:   Verifies that all required data has been provided by the user.
        'Parameters:    Values to be verified.
        'Returns:       Boolean value identifying if all required criteria has been satisfied
        '------------------------------------------------------------------------------------

        If Trim(strTypeName) = "" Then
            strErrMessage = "A Transaction Type Name is required."
            VerifyData = False
            Exit Function
        End If

        If Trim(lngTxGroupID) < 1 Then
            strErrMessage = "An associated Group Name is required."
            VerifyData = False
            Exit Function
        End If

        'Check for previous existence of TxType under current Group
        If lngTxTypeID = 0 And Exists(strTypeName) Then
            strErrMessage = "The Transaction Type " & strTypeName & " already exists."
            VerifyData = False
            Exit Function
        End If

        'Return a True statement if all business rules have been satisfied.
        VerifyData = True

    End Function

    Public Function Exists(ByVal strTypeName As String) As Boolean
        '------------------------------------------------------------------------------------
        'Date: 09/01/00
        'Author: Juan Castro
        'Description:   Determines if the Transaction Type Name already exists in the table
        'Parameters:    strTypeName - Transaction Type Name to be checked
        '               lngTxGroupID - Transaction Group ID associated with type.
        'Returns:       True if the parameters exists, false otherwise
        '------------------------------------------------------------------------------------

        Dim objTxType As New PostingDB.CTxTypeDB
        Dim blnExists As Boolean
        objTxType.ConnectionString = _ConnectionString

        blnExists = objTxType.Exists(strTypeName)

        Exists = blnExists

        objTxType = Nothing



    End Function

    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 08/31/00
        'Author: Juan Castro
        'Description:   Retrieves records from the tblTransactionType table utilizing a stored procedure.
        'Parameters:    blnIncludeDisabled- determines if disabled records will be returned.
        'Returns: ADODB.Recordset
        '-------------------------------------------------------------------------------------

        Dim objTxType As New PostingDB.CTxTypeDB
        objTxType.ConnectionString = _ConnectionString

        Fetch = objTxType.Fetch(blnIncludeDisabled)


        objTxType = Nothing



    End Function

    Public Sub Update(ByVal lngTxTypeID As Long, ByVal strTypeName As String, ByVal lngTxGroupID As Long, _
                    ByVal strCharge As String, ByVal blnQBillYN As Boolean, ByVal blnQWareYN As Boolean, ByVal blnSystemYN As Boolean, _
                    ByVal strProvAction As String, ByVal strPsyAction As String, ByVal strExtDesc As String)
        '----------------------------------------------------------------------------------------
        'Date: 09/05/00
        'Author: Juan Castro
        'Description:   Updates a row in the tblTransactionType table utilizing a stored procedure.
        'Parameters:    Each parameter identifies the column value that will be inserted.
        'Returns:       Null
        '----------------------------------------------------------------------------------------

        Dim objTxType As New PostingDB.CTxTypeDB
        Dim strErrMessage As String

        objTxType.ConnectionString = _ConnectionString

        If Not VerifyData(lngTxTypeID, strTypeName, lngTxGroupID, strErrMessage) Then Exit Sub
        objTxType.Update(lngTxTypeID, strTypeName, lngTxGroupID, strCharge, blnQBillYN, blnQWareYN, blnSystemYN, strProvAction, strPsyAction, strExtDesc)



        objTxType = Nothing



    End Sub

    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngTxTypeID As Long, ByVal strDisabledBy As String)
        '------------------------------------------------------------------------------------
        'Date: 09/05/00
        'Author: Juan Castro
        'Description:   Flags a row in the tblTransactionType table marking the row as disabled or undisabled.
        'Parameters:    blnDeleted - Boolen value identifying if the record is to be deleted (True)
        '                   or undeleted (False).
        '               lngTxTypeID - ID of the row in the table whose value will be updated.
        '               strDisabledBy - Login name of the user responsible for marking the row as deleted.
        'Returns:       Null
        '------------------------------------------------------------------------------------

        Dim objTxType As New PostingDB.CTxTypeDB
        objTxType.ConnectionString = _ConnectionString


        objTxType.Deleted(blnDeleted, lngTxTypeID, strDisabledBy)



        objTxType = Nothing



    End Sub

    Public Function FetchByID(ByVal lngID As Long) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 08/31/00
        'Author: Juan Castro
        'Description:   Retrieves records from the tblTransactionGroup table utilizing a stored procedure.
        'Parameters:    blnIncludeDisabled- determines if disabled records will be returned.
        'Returns: ADODB.Recordset
        '-------------------------------------------------------------------------------------

        Dim objTxType As New PostingDB.CTxTypeDB
        objTxType.ConnectionString = _ConnectionString
        FetchByID = objTxType.FetchByID(lngID)

        objTxType = Nothing



    End Function

    Public Function FetchByGroupID(ByVal lngTxGroupID As Long) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 08/31/00
        'Author: Juan Castro
        'Description:   Retrieves records from the tblTransactionType table utilizing a stored procedure.
        'Parameters:    lngTxGroupID - will return Types associated with the Group ID
        'Returns: ADODB.Recordset
        '-------------------------------------------------------------------------------------

        Dim objTxType As New PostingDB.CTxTypeDB
        objTxType.ConnectionString = _ConnectionString

        FetchByGroupID = objTxType.FetchByGroupID(lngTxGroupID)



    End Function
    Public Function FetchProvPostings(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 02/19/2001
        'Author: Eric Pena
        'Description:   Retrieves records from the tblTransactionType table that providers may post under utilizing a stored procedure.
        'Parameters:    blnIncludeDisabled- determines if disabled records will be returned.
        'Returns: ADODB.Recordset
        '-------------------------------------------------------------------------------------

        Dim objTxType As New PostingDB.CTxTypeDB
        objTxType.ConnectionString = _ConnectionString
        FetchProvPostings = objTxType.FetchProvPostings(blnIncludeDisabled)



    End Function
    Public Function FetchCharges(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 02/19/2001
        'Author: Eric Pena
        'Description:   Retrieves records from the tblTransactionType table that providers may post under utilizing a stored procedure.
        'Parameters:    blnIncludeDisabled- determines if disabled records will be returned.
        'Returns: ADODB.Recordset
        '-------------------------------------------------------------------------------------

        Dim objTxType As New PostingDB.CTxTypeDB
        objTxType.ConnectionString = _ConnectionString

        FetchCharges = objTxType.FetchCharges(blnIncludeDisabled)

        'signal successful completion



    End Function


    Public Function FetchAdjustments(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 06/23/2003
        'Author: Dave Richkun
        'Description:   Retrieves adustment transactions from tblTransactionType
        'Parameters:    blnIncludeDisabled- determines if disabled records will be returned.
        'Returns: ADODB.Recordset
        '-------------------------------------------------------------------------------------

        Dim objTxType As New PostingDB.CTxTypeDB
        objTxType.ConnectionString = _ConnectionString
        FetchAdjustments = objTxType.FetchAdjustments(blnIncludeDisabled)
        objTxType = Nothing



    End Function



End Class

