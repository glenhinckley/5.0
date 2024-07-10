Option Strict Off
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CSalesBz
    '--------------------------------------------------------------------
    'Class Name: CSalesDB
    'Date: 10/27/2009
    'Author: Duane C Orth
    'Description:  MTS business object designed to  methods associated
    '              with the CSalesDB class.
    '--------------------------------------------------------------------
    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CSalesDB"

    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False, Optional ByVal strWhere As String = "", Optional ByVal strOrderBy As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/27/2009
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblSalesRep table.
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
        Dim objSales As New ListDb.CSalesDB




        rstSQL = objSales.Fetch(blnIncludeDisabled, strWhere, strOrderBy)
        Fetch = rstSQL

        'UPGRADE_NOTE: Object objSales may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objSales = Nothing


    End Function

    Public Function Insert(ByVal strSalesRepDesc As String, ByVal lngBaseCom As Integer, ByVal dblFullServiceCom As Double, ByVal dblSubmissionCom As Double) As Integer
        '--------------------------------------------------------------------
        'Date: 10/27/2009
        'Author: Duane C Orth
        'Description:  Inserts a row into the tblSalesRep table utilizing
        '              a stored procedure.
        'Parameters: strSalesRepDesc - The name given to the Role
        '            strDescription - The description of the Role
        '              that will be inserted into the table.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------

        Dim objSales As New ListDb.CSalesDB
        Dim lngID As Integer
        Dim strErrMessage As String



        'Verify data before proceeding.
        If Not VerifyData(0, strSalesRepDesc, strErrMessage) Then
            Exit Function
        End If

        lngID = objSales.Insert(strSalesRepDesc, lngBaseCom, dblFullServiceCom, dblSubmissionCom)
        Insert = lngID


        'UPGRADE_NOTE: Object objSales may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objSales = Nothing


    End Function


    Public Sub Update(ByVal lngID As Integer, ByVal strSalesRepDesc As String, ByVal lngBaseCom As Integer, ByVal dblFullServiceCom As Double, ByVal dblSubmissionCom As Double)
        '--------------------------------------------------------------------
        'Date: 10/27/2009
        'Author: Duane C Orth
        'Description:  Updates a row into the tblSalesRep table utilizing
        '              a stored procedure.
        'Parameters:  lngID - ID of the row in the table whose value will be
        '               updated.
        '             strSalesRepDesc - The name given to the role.
        '             strDescription - The Role status description
        '                to which the record will be changed.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objSales As New ListDb.CSalesDB
        Dim strErrMessage As String



        'Verify data before proceeding.
        If Not VerifyData(lngID, strSalesRepDesc, strErrMessage) Then
            Exit Sub
        End If

        objSales.Update(lngID, strSalesRepDesc, lngBaseCom, dblFullServiceCom, dblSubmissionCom)

        'UPGRADE_NOTE: Object objSales may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objSales = Nothing

        'Signal successful completion


        'UPGRADE_NOTE: Object objSales may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objSales = Nothing

    End Sub


    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Integer, ByVal strDeletedBy As String)
        '--------------------------------------------------------------------
        'Date: 12/30/1999
        'Author: Duane C Orth
        'Description:  Flags a row in the tblSalesRep table marking the row as
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

        Dim objSales As New ListDb.CSalesDB




        objSales.Deleted(blnDeleted, lngID, strDeletedBy)

        'UPGRADE_NOTE: Object objSales may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objSales = Nothing

    End Sub


    Public Function Exists(ByVal strSalesRepDesc As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 01/05/2000
        'Author: Duane C Orth
        'Description:  Determines if an Employment Status description identical
        '              to the strDescription parameter already exists in the table.
        'Parameters: strSalesRepDesc - Role record to be checked
        'Returns: True if the name exists, false otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objSales As New ListDb.CSalesDB
        Dim blnExists As Boolean





        blnExists = objSales.Exists(strSalesRepDesc)
        Exists = blnExists


        'UPGRADE_NOTE: Object objSales may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objSales = Nothing

    End Function


    Private Function VerifyData(ByVal lngID As Integer, ByVal strSalesRepDesc As String, ByRef strErrMessage As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 01/04/2000
        'Author: Duane C Orth
        'Description:  Verifies all required data has been provided by the user.
        'Parameters:  The values to be checked.
        'Returns: Boolean value identifying if all criteria has been satisfied.
        '--------------------------------------------------------------------

        If Trim(strSalesRepDesc) = "" Then
            strErrMessage = "Sales Rep name is required."
            VerifyData = False
            Exit Function
        End If

        'Check for existance only when inserting new data
        If lngID = 0 And Exists(strSalesRepDesc) Then
            strErrMessage = "Sales Rep name '" & strSalesRepDesc & "' already exists."
            VerifyData = False
            Exit Function
        End If

        'If we get here, all is well...
        VerifyData = True

    End Function
End Class