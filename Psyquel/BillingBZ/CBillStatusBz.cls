
'--------------------------------------------------------------------
'Class Name: CBillStatusBz
'Date: 08/30/2000
'Author: Eric Pena
'Description:  MTS business object designed to   methods associated
'              with the CBillStatusDB class.
'--------------------------------------------------------------------
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CBillStatusBz

    Private _ConnectionString As String = String.Empty
    Private Const CLASS_NAME As String = "CBillStatusBz"
    Private Const TABLE_NAME As String = "tblBillStatus"


    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property





    Public Function Insert(ByVal strDescription As String) As Long
        '--------------------------------------------------------------------
        'Date: 08/30/2000
        'Author: Eric Pena
        'Description:  Inserts a row into the tblBillStatus table utilizing a stored procedure.
        'Parameters: Each parameter identifies the column value that will be  inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------
        Dim objBS As New BillingDB.CBillStatusDB
        Dim strErrMessage As String

        If Not VerifyData(0, strDescription, strErrMessage) Then Exit Function

        Insert = objBS.Insert(strDescription)


        objBS = Nothing

    End Function
    Public Sub Update(ByVal lngID As Long, ByVal strDescription As String)
        '--------------------------------------------------------------------
        'Date: 08/30/2000
        'Author: Eric Pena
        'Description:  Updates a row in the tblBillStatus table utilizing a stored procedure.
        'Parameters: Each parameter identifies the column value that will be inserted.
        'Returns: Null
        '--------------------------------------------------------------------
        Dim objBS As New BillingDB.CBillStatusDB
        Dim strErrMessage As String


        If Not VerifyData(lngID, strDescription, strErrMessage) Then Exit Sub

        objBS.Update(lngID, strDescription)


        objBS = Nothing

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
        Dim objBS As New BillingDB.CBillStatusDB




        objBS.Deleted(blnDeleted, lngID, strDeletedBy)


        objBS = Nothing


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
        Dim objBS As New BillingDB.CBillStatusDB
        Dim blnExists As Boolean


        blnExists = objBS.Exists(strDescription)

        Exists = blnExists

        objBS = Nothing


    End Function
    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/30/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblBillStatus table utilizing a stored procedure
        'Parameters: blnIncludeDisabled - determines if disabled records will be returned
        'Returns: ADORst
        '--------------------------------------------------------------------

        Dim objBS As New BillingDB.CBillStatusDB


        Fetch = objBS.Fetch(blnIncludeDisabled)

        'Signal successful completion
        objBS = Nothing

    End Function
    Public Function FetchByID(ByVal lngID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/30/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblBillStatus table utilizing a stored procedure
        'Parameters: lngID - the PK of the row to return
        'Returns: ADORst
        '--------------------------------------------------------------------

        Dim objBS As New BillingDB.CBillStatusDB




        FetchByID = objBS.FetchByID(lngID)

        objBS = Nothing

    End Function



End Class





