Option Strict Off
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CMarketBz
    '--------------------------------------------------------------------
    'Class Name: CMarketDB
    'Date: 10/27/2009
    'Author: Duane C Orth
    'Description:  MTS business object designed to  methods associated
    '              with the CMarketDB class.
    '--------------------------------------------------------------------
    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CMarketDB"

    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False, Optional ByVal strWhere As String = "", Optional ByVal strOrderBy As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/27/2009
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblMarketSource table.
        'Parameters: blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Disabled' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        '            strWhere - Optional 'Where' clause of the SQL statement by
        '               which records will be filtered.  If the parameter is not
        '               supplied, all records will be retrieved.
        '             strOrderBy - Optional 'Order By' clause in which retrieved
        '               records will be sorted.  The default is set to the
        '               'fldRole' column
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim rstSQL As New ADODB.Recordset
        Dim objMarket As New ListDb.CMarketDB




        rstSQL = objMarket.Fetch(blnIncludeDisabled, strWhere, strOrderBy)
        Fetch = rstSQL


        'UPGRADE_NOTE: Object objMarket may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objMarket = Nothing

    End Function

    Public Function Insert(ByVal strMarketSourceDesc As String) As Integer
        '--------------------------------------------------------------------
        'Date: 10/27/2009
        'Author: Duane C Orth
        'Description:  Inserts a row into the tblMarketSource table utilizing
        '              a stored procedure.
        'Parameters: strMarketSourceDesc - The name given to the Role
        '            strDescription - The description of the Role
        '              that will be inserted into the table.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------

        Dim objMarket As New ListDb.CMarketDB
        Dim lngID As Integer
        Dim strErrMessage As String



        'Verify data before proceeding.
        If Not VerifyData(0, strMarketSourceDesc, strErrMessage) Then
            Exit Function
        End If

        lngID = objMarket.Insert(strMarketSourceDesc)
        Insert = lngID


        'UPGRADE_NOTE: Object objMarket may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objMarket = Nothing


    End Function


    Public Sub Update(ByVal lngID As Integer, ByVal strMarketSourceDesc As String)
        '--------------------------------------------------------------------
        'Date: 10/27/2009
        'Author: Duane C Orth
        'Description:  Updates a row into the tblMarketSource table utilizing
        '              a stored procedure.
        'Parameters:  lngID - ID of the row in the table whose value will be
        '               updated.
        '             strMarketSourceDesc - The Role status description
        '                to which the record will be changed.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objMarket As New ListDb.CMarketDB
        Dim strErrMessage As String



        'Verify data before proceeding.
        If Not VerifyData(lngID, strMarketSourceDesc, strErrMessage) Then
            Exit Sub
        End If

        objMarket.Update(lngID, strMarketSourceDesc)

        'UPGRADE_NOTE: Object objMarket may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objMarket = Nothing


        'UPGRADE_NOTE: Object objMarket may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objMarket = Nothing


    End Sub


    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Integer, ByVal strDeletedBy As String)
        '--------------------------------------------------------------------
        'Date: 12/30/1999
        'Author: Duane C Orth
        'Description:  Flags a row in the tblMarketSource table marking the row as
        '              deleted or undeleted.
        'Parameters: blnDeleted - Boolean value identifying if the record is to
        '               be deleted (True) or undeleted (False).
        '            lngID - ID of the row in the table whose value will be
        '               updated.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objMarket As New ListDb.CMarketDB




        objMarket.Deleted(blnDeleted, lngID, strDeletedBy)

   
        'UPGRADE_NOTE: Object objMarket may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objMarket = Nothing



    End Sub


    Public Function Exists(ByVal strMarketSourceDesc As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 01/05/2000
        'Author: Duane C Orth
        'Description:  Determines if an Employment Status description identical
        '              to the strDescription parameter already exists in the table.
        'Parameters: strMarketSourceDesc - Role record to be checked
        'Returns: True if the name exists, false otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objMarket As New ListDb.CMarketDB
        Dim blnExists As Boolean





        blnExists = objMarket.Exists(strMarketSourceDesc)
        Exists = blnExists


        'UPGRADE_NOTE: Object objMarket may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objMarket = Nothing


    End Function


    Private Function VerifyData(ByVal lngID As Integer, ByVal strMarketSourceDesc As String, ByRef strErrMessage As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 01/04/2000
        'Author: Duane C Orth
        'Description:  Verifies all required data has been provided by the user.
        'Parameters:  The values to be checked.
        'Returns: Boolean value identifying if all criteria has been satisfied.
        '--------------------------------------------------------------------

        If Trim(strMarketSourceDesc) = "" Then
            strErrMessage = "Market Source description is required."
            VerifyData = False
            Exit Function
        End If

        'Check for existance only when inserting new data
        If lngID = 0 And Exists(strMarketSourceDesc) Then
            strErrMessage = "Market Source description '" & strMarketSourceDesc & "' already exists."
            VerifyData = False
            Exit Function
        End If

        'If we get here, all is well...
        VerifyData = True

    End Function
End Class