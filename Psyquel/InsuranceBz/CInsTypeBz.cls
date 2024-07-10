
'--------------------------------------------------------------------
'Class Name: CInsTypeBz
'Date: 08/06/2004
'Author: Duane C Orth
'Denis J Finkel
'Description:  MTS business object designed to   methods associated
'              with the CInsTypeDB class.
'--------------------------------------------------------------------
'Revision History:
'
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

Public Class CInsTypeBz

    Private Const CLASS_NAME As String = "CInsTypeBz"
    Private Const TABLE_NAME As String = "tblInsuranceType"

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
        'Date: 08/06/2004
        'Author: Duane C Orth
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


        Dim objInsType As New InsuranceDB.CInsTypeDB
        Dim rstSQL As New ADODB.Recordset

        objInsType.ConnectionString = _ConnectionString



        rstSQL = objInsType.Fetch(blnIncludeDisabled, strWhere, strOrderBy)
        Fetch = rstSQL

        objInsType = Nothing


    End Function
    Public Function Insert(ByVal strInsType As String, ByVal strDescription As String, ByVal strUsername As String) As Long
        '--------------------------------------------------------------------
        'Date: 08/06/2004
        'Author: Duane C Orth
        'Description:  Inserts a row into the tblInsuranceType table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------

        Dim objInsType As New InsuranceDB.CInsTypeDB
        Dim strErrMessage As String
        objInsType.ConnectionString = _ConnectionString

        Insert = objInsType.Insert(strInsType, strDescription, strUsername)

        objInsType = Nothing

    End Function
    Public Sub Update(ByVal lngID As Long, ByVal strInsType As String, ByVal strDescription As String)
        '--------------------------------------------------------------------
        'Date: 08/06/2004
        'Author: Duane C Orth
        'Description:  Updates a row in the tblPlan table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Null
        '--------------------------------------------------------------------


        Dim objInsType As New InsuranceDB.CInsTypeDB
        Dim strErrMessage As String
        objInsType.ConnectionString = _ConnectionString

        objInsType.Update(lngID, strInsType, strDescription)



        objInsType = Nothing


    End Sub
    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Long, ByVal strDeletedBy As String)
        '--------------------------------------------------------------------
        'Date: 08/06/2004
        'Author: Duane C Orth
        'Description:  Flags a row in the tblInsuranceType table marking the row as
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


        Dim objInsType As New InsuranceDB.CInsTypeDB

        objInsType.ConnectionString = _ConnectionString

        objInsType.Deleted(blnDeleted, lngID, strDeletedBy)


        objInsType = Nothing




    End Sub
End Class
