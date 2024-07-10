Option Strict Off
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CHHIncomeBZ
    '--------------------------------------------------------------------
    'Class Name: CHHIncomeBz
    'Date: 11/30/2012
    'Author: Duane C Orth
    'Description:  MTS business object designed to  methods associated
    '              with the CHHIncomeDB class.
    '--------------------------------------------------------------------

    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CHHIncomeBz"

    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False, Optional ByVal strWhere As String = "", Optional ByVal strOrderBy As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 11/30/2012
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblHHIncome table.
        'Parameters:  blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Disabled' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        '             strWhere - Optional 'Where' clause of the SQL statement by
        '               which records will be filtered.  If the parameter is not
        '               supplied, all records will be retrieved.
        '             strOrderBy - Optional 'Order By' clause in which retrieved
        '               records will be sorted.  The default is set to the
        '               'fldHHIncome' column
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim obj As New ListDb.CHHIncomeDB
        Dim rstSQL As New ADODB.Recordset

        rstSQL = obj.Fetch(blnIncludeDisabled, strWhere, strOrderBy)
        Fetch = rstSQL

        'UPGRADE_NOTE: Object obj may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        obj = Nothing



    End Function

    Public Function Exists(ByVal strDescription As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 11/30/2012
        'Author: Duane C Orth
        'Description:  Determines if an HHIncome description identical to the
        '              strDescription parameter already exists in the table.
        'Parameters: strDescription - HHIncome record to be checked
        'Returns: True if the name exists, false otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim obj As New ListDb.CHHIncomeDB
        Dim blnExists As Boolean



        blnExists = obj.Exists(strDescription)
        Exists = blnExists

    End Function
    Private Function VerifyData(ByVal lngID As Integer, ByVal strDescription As String, ByRef strErrMessage As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 11/30/2012
        'Author: Duane C Orth
        'Description:  Verifies all required data has been provided by the user.
        'Parameters:  lngID - ID of row where strDescription should exist.  Value
        '               of 0 describes a new entry.
        '             strDescription - Value being sought in table.
        '             strErrMessage - String filled with error message if an error
        '               is encountered.
        'Returns: Boolean value identifying if all criteria has been satisfied.
        '--------------------------------------------------------------------

        If Trim(strDescription) = "" Then
            strErrMessage = "Description is required."
            VerifyData = False
            Exit Function
        End If

        ' Check for existence only when inserting new data
        If lngID = 0 And Exists(strDescription) Then
            strErrMessage = "HHIncome '" & strDescription & "' already exists."
            VerifyData = False
            Exit Function
        End If

        VerifyData = True

    End Function

    Public Function Insert(ByVal strDescription As String) As Integer
        '--------------------------------------------------------------------
        'Date: 11/30/2012
        'Author: Duane C Orth
        'Description:  Inserts a row into the tblHHIncome table utilizing
        '              a stored procedure.
        'Parameters: strDescription - The description of the HHIncome
        '              that will be inserted into the table.
        'Returns: ID (Primary Key) of the row inserted
        '--------------------------------------------------------------------

        Dim obj As New ListDb.CHHIncomeDB
        Dim lngID As Integer
        Dim strErrMessage As String


        ' Verify data before proceeding
        If Not VerifyData(0, strDescription, strErrMessage) Then
            Exit Function
        End If

        lngID = obj.Insert(strDescription)

        'UPGRADE_NOTE: Object obj may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        obj = Nothing


        Insert = lngID

        Exit Function

    End Function

    Public Sub Update(ByVal lngID As Integer, ByVal strDescription As String)
        '--------------------------------------------------------------------
        'Date: 11/30/2012
        'Author: Duane C Orth
        'Description:  Updates a row into the tblHHIncome table.
        'Parameters:  lngID - ID of the row in the table whose value will be
        '               updated.
        '             strDescription - The HHIncome description
        '                to which the record will be changed.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim obj As New ListDb.CHHIncomeDB
        Dim strErrMessage As String


        ' Verify data before proceeding
        If Not VerifyData(lngID, strDescription, strErrMessage) Then
            Exit Sub
        End If

        obj.Update(lngID, strDescription)

        'UPGRADE_NOTE: Object obj may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        obj = Nothing

    End Sub
End Class