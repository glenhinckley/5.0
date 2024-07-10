
'--------------------------------------------------------------------
'Class Name: CPlanBz
'Date: 02/24/2000
'Author: Eric Pena
'Denis J Finkel
'Description:  MTS business object designed to   methods associated
'              with the CPlanDB class.
'--------------------------------------------------------------------
'Revision History:
'  R001 07/05/2000 Richkun: Added FetchByID() method
'  R002 07/06/2000 Richkun: Added Filter() method
'  R003 03/26/2001 Nichol: Added lngMaxRows optional parameter to Filter()
'  R004 05/02/2001 Richkun: Added optional strEAP parameter to Filter()
'--------------------------------------------------------------------
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

Public Class CPlanBz

    Private Const CLASS_NAME As String = "CPlanBz"
    Private Const TABLE_NAME As String = "tblPlan"


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




    Public Enum SearchColumn 'R002
        srcInsCompanyID = 1
        SrcPlanName = 2
        SrcCity = 3
        SrcState = 4
        SrcZip = 5
        SrcPOBox = 6
        SrcPayerID = 7
    End Enum

    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/24/2000
        'Author: Eric Pena
        'Description:  Retrieves records from the tblPlan table.
        'Parameters: blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as ' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        '            strWhere - Optional 'Where' clause of the SQL statement by
        '               which records will be filtered.  If the parameter is not
        '               supplied, all records will be retrieved.
        '             strOrderBy - Optional 'Order By' clause in which retrieved
        '               records will be sorted.  The default is set to the
        '               'fldPlanName' column
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim objPlan As New InsuranceDB.CPlanDB
        Dim rstSQL As ADODB.Recordset

        objPlan.ConnectionString = _ConnectionString

        rstSQL = objPlan.Fetch(blnIncludeDisabled)
        Fetch = rstSQL


        objPlan = Nothing


    End Function

    Public Function FetchByID(ByVal lngPlanID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/05/2000
        'Author: Dave Richkun
        'Description:  Retrieves information for a specific record from the
        '               tblPlan table.
        'Parameters: lngPlanID - ID of the Plan to retrieve detail information for
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '  R001 - Created
        '--------------------------------------------------------------------


        Dim objPlan As New InsuranceDB.CPlanDB
        Dim rstSQL As New ADODB.Recordset
        objPlan.ConnectionString = _ConnectionString



        rstSQL = objPlan.FetchByID(lngPlanID)
        FetchByID = rstSQL

  
        objPlan = Nothing


    End Function

    Public Function Filter(ByVal intColumn As SearchColumn, ByVal strValue As String, _
                           ByVal intSortOrder As SearchColumn, Optional ByVal strEAP As String = "N", _
                           Optional ByVal lngMaxRows As Long = 101) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/06/2000
        'Author: Dave Richkun
        'Description:  Retrieves records from the tblPlan table filtering records
        '              using the SearchColumn enumerator parameter.
        'Parameters: intColumn - An enumerator that identifies the record filter
        '               column that will be applied to the resultset
        '            strValue - Identifies the value that will be searched for - used
        '               in conjunction with the intColumn parameter
        '            intSortOrder - Identifies the order in which the results are
        '               sorted.
        '            strEAP - Value to identify if desiring to search for only EAP plans R004
        '        Eg.) Filter(srcState, "TX", srcCity) will return all Plan records
        '             located within the state of Texas, ordered by City
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '  R002 Created
        '--------------------------------------------------------------------

        Dim objPlan As New InsuranceDB.CPlanDB
        Dim rstSQL As New ADODB.Recordset
        objPlan.ConnectionString = _ConnectionString

        'If filtering by Insurance Company ID, ensure that the strValue parameter
        'can be cast to a number.
        If intColumn = 1 Then
            If Not IsNumeric(strValue) Then
                'err.raise(vbObjectError + 512, "CPlanBz.Filter", "Invalid parameter - Insurance Company ID must be numeric")
            End If
        End If

        rstSQL = objPlan.Filter(intColumn, strValue, intSortOrder, strEAP, lngMaxRows)
        Filter = rstSQL


        objPlan = Nothing


    End Function


    Public Function Insert(ByVal strName As String, ByVal lngPlanClassID As Long, _
                         ByVal lngCPCID As Long, ByVal strNotes As String, _
                         ByVal lngFileDeadline As Long, ByVal strUsername As String) As Long
        '--------------------------------------------------------------------
        'Date: 02/24/2000
        'Author: Eric Pena
        'Description:  Inserts a row into the tblPlan table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------

        Dim objPlan As New InsuranceDB.CPlanDB
        Dim strErrMessage As String

        objPlan.ConnectionString = _ConnectionString
        If Not VerifyData(0, strName, lngPlanClassID, lngCPCID, strErrMessage) Then Exit Function

        Insert = objPlan.Insert(strName, lngPlanClassID, lngCPCID, strNotes, lngFileDeadline, strUsername)


        objPlan = Nothing


    End Function
    Public Sub Update(ByVal lngID As Long, ByVal strName As String, _
                         ByVal lngPlanClassID As Long, ByVal lngCPCID As Long, _
                         ByVal strNotes As String, ByVal lngFileDeadline As Long)
        '--------------------------------------------------------------------
        'Date: 02/24/2000
        'Author: Eric Pena
        'Description:  Updates a row in the tblPlan table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Null
        '--------------------------------------------------------------------


        Dim objPlan As New InsuranceDB.CPlanDB
        Dim strErrMessage As String


        objPlan.ConnectionString = _ConnectionString


        If Not VerifyData(lngID, strName, lngPlanClassID, lngCPCID, strErrMessage) Then
            Exit Sub
        End If

        objPlan.Update(lngID, strName, lngPlanClassID, lngCPCID, strNotes, lngFileDeadline)


        objPlan = Nothing


    End Sub
    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Long, ByVal strDeletedBy As String)
        '--------------------------------------------------------------------
        'Date: 02/24/2000
        'Author: Eric Pena
        'Description:  Flags a row in the tblPlan table marking the row as
        '              deleted or undeleted.
        'Parameters: blnDeleted - Boolean value identifying if the record is to
        '               be deleted (True) or undeleted (False).
        '            lngID - ID of the row in the table whose value will be
        '               updated.
        '            strUserName - Login name of the user responsible for
        '               marking the row as deleted.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim objPlan As New InsuranceDB.CPlanDB

        objPlan.ConnectionString = _ConnectionString

        'TODO: Check for active claims, RPS with this Plan
        objPlan.Deleted(blnDeleted, lngID, strDeletedBy)


        objPlan = Nothing


    End Sub
    Private Function VerifyData(ByVal lngID As Long, ByVal strName As String, ByVal lngPlanClassID As Long, ByVal lngCPCID As String, ByRef strErrMessage As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 02/24/2000
        'Author: Eric Pena
        'Description:  Verifies all required data has been provided by the user.
        'Parameters:  The values to be checked.
        'Returns: Boolean value identifying if all criteria has been satisfied.
        '--------------------------------------------------------------------

        If Trim(strName) = "" Then
            strErrMessage = "Plan Name  is required."
            VerifyData = False
            Exit Function
        End If
        If lngCPCID < 1 Then
            strErrMessage = "A Claims Processing Center is required."
            VerifyData = False
            Exit Function
        End If
        If lngPlanClassID < 1 Then
            strErrMessage = "A Plan Classification is required."
            VerifyData = False
            Exit Function
        End If

        'If we get here, all is well...
        VerifyData = True

    End Function
    Public Function FetchByName(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 3/08/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblPlan table ordered by the plan name utilizing a stored procedure
        'Parameters: blnIncludeDisabled - determines if disabled records will be returned
        'Returns: ADORst
        '--------------------------------------------------------------------

        Dim objPlan As New InsuranceDB.CPlanDB
        objPlan.ConnectionString = _ConnectionString

        FetchByName = objPlan.FetchByName(blnIncludeDisabled)

        objPlan = Nothing

    End Function
    Public Function FetchByZip(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 3/08/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblPlan table ordered by the CPC's zip utilizing a stored procedure
        'Parameters: blnIncludeDisabled - determines if disabled records will be returned
        'Returns: ADORst
        '--------------------------------------------------------------------

        Dim objPlan As New InsuranceDB.CPlanDB

        objPlan.ConnectionString = _ConnectionString


        FetchByZip = objPlan.FetchByZip(blnIncludeDisabled)


        objPlan = Nothing

    End Function
    Public Function FetchByCity(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 3/08/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblPlan table ordered by the CPC's city utilizing a stored procedure
        'Parameters: blnIncludeDisabled - determines if disabled records will be returned
        'Returns: ADORst
        '--------------------------------------------------------------------

        Dim objPlan As New InsuranceDB.CPlanDB

        objPlan.ConnectionString = _ConnectionString
        FetchByCity = objPlan.FetchByCity(blnIncludeDisabled)

        objPlan = Nothing

    End Function
    Public Function FetchByState(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 3/08/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblPlan table ordered by the CPC's zip utilizing a stored procedure
        'Parameters: blnIncludeDisabled - determines if disabled records will be returned
        'Returns: ADORst
        '--------------------------------------------------------------------

        Dim objPlan As New InsuranceDB.CPlanDB
        objPlan.ConnectionString = _ConnectionString

        FetchByState = objPlan.FetchByState(blnIncludeDisabled)

        objPlan = Nothing

    End Function
    Public Function FetchByIns(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 5/01/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblPlan, tblClaimsProcCtr, and tblInsCompany tables utilizing a stored procedure
        'Parameters: blnIncludeDisabled - determines if disabled records will be returned
        'Returns: ADORst
        '--------------------------------------------------------------------

        Dim objPlan As New InsuranceDB.CPlanDB
        objPlan.ConnectionString = _ConnectionString

        FetchByIns = objPlan.FetchByIns(blnIncludeDisabled)


        objPlan = Nothing

    End Function


End Class






