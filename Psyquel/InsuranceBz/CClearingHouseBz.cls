
'--------------------------------------------------------------------
'Class Name: CInsTypeBz
'Date: 10/06/2004
'Author: Duane C Orth
'Denis J Finkel
'Description:  MTS business object designed to   methods associated
'              with the CClearingHouseDB class.
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

Public Class CClearingHouseBz

    Private Const CLASS_NAME As String = "CInsTypeBz"
    Private Const TABLE_NAME As String = "tblClearingHouse"

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




    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False, Optional ByVal strWhere As String = "", _
                          Optional ByVal strOrderBy As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/06/2004
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblClearingHouse table.
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


        Dim objClearingHouse As New InsuranceDB.CClearingHouseDB
        Dim rstSQL As New ADODB.Recordset

        objClearingHouse.ConnectionString = _ConnectionString



        rstSQL = objClearingHouse.Fetch(blnIncludeDisabled, strWhere, strOrderBy)
        Fetch = rstSQL


        objClearingHouse = Nothing


    End Function
    Public Function Insert(ByVal strDescription As String, _
                           ByVal strFolder As String, _
                           ByVal strSubmitterIDQualifier As String, _
                           ByVal strSubmitterID As String, _
                           ByVal blnSubmitterIDUseYN As Boolean, _
                           ByVal strSubmitterPassword As String, _
                           ByVal blnSubmitterPasswordUseYN As Boolean, _
                           ByVal strReceiverIDQualifier As String, _
                           ByVal strReceiverID As String, _
                           ByVal strVersion As String, _
                           ByVal intAcknowledgment As Integer, _
                           ByVal strReceiverCode As String, _
                           ByVal blnReceiverPrefixYN As Boolean, _
                           ByVal strRelease As String, _
                           ByVal blnUseCrLfYN As Boolean, _
                           ByVal strEFileName As String, _
                           ByVal strPhone As String, _
                           ByVal strExt As String, _
                           ByVal strEmail As String, _
                           ByVal strContact As String, _
                           ByVal strNotes As String, _
                           ByVal strUsername As String) As Long
        '--------------------------------------------------------------------
        'Date: 10/06/2004
        'Author: Duane C Orth
        'Description:  Inserts a row into the tblClearingHouse table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------

        Dim objClearingHouse As New InsuranceDB.CClearingHouseDB
        Dim strErrMessage As String


        objClearingHouse.ConnectionString = _ConnectionString


        Insert = objClearingHouse.Insert(strDescription, strFolder, strSubmitterIDQualifier, strSubmitterID, blnSubmitterIDUseYN, _
            strSubmitterPassword, blnSubmitterPasswordUseYN, strReceiverIDQualifier, strReceiverID, strVersion, intAcknowledgment, _
            strReceiverCode, blnReceiverPrefixYN, strRelease, blnUseCrLfYN, strEFileName, strPhone, strExt, _
            strEmail, strContact, strNotes, strUsername)

        'Signal successful completion

        objClearingHouse = Nothing

    End Function
    Public Sub Update(ByVal lngID As Long, _
                           ByVal strDescription As String, _
                           ByVal strFolder As String, _
                           ByVal strSubmitterIDQualifier As String, _
                           ByVal strSubmitterID As String, _
                           ByVal blnSubmitterIDUseYN As Boolean, _
                           ByVal strSubmitterPassword As String, _
                           ByVal blnSubmitterPasswordUseYN As Boolean, _
                           ByVal strReceiverIDQualifier As String, _
                           ByVal strReceiverID As String, _
                           ByVal strVersion As String, _
                           ByVal intAcknowledgment As Integer, _
                           ByVal strReceiverCode As String, _
                           ByVal blnReceiverPrefixYN As Boolean, _
                           ByVal strRelease As String, _
                           ByVal blnUseCrLfYN As Boolean, _
                           ByVal strEFileName As String, _
                           ByVal strPhone As String, _
                           ByVal strExt As String, _
                           ByVal strEmail As String, _
                           ByVal strContact As String, _
                           ByVal strNotes As String)
        '--------------------------------------------------------------------
        'Date: 10/06/2004
        'Author: Duane C Orth
        'Description:  Updates a row in the tblClearingHouse table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Null
        '--------------------------------------------------------------------


        Dim objClearingHouse As New InsuranceDB.CClearingHouseDB
        Dim strErrMessage As String


        objClearingHouse.ConnectionString = _ConnectionString

        objClearingHouse.Update(lngID, strDescription, strFolder, strSubmitterIDQualifier, strSubmitterID, blnSubmitterIDUseYN, _
            strSubmitterPassword, blnSubmitterPasswordUseYN, strReceiverIDQualifier, strReceiverID, strVersion, intAcknowledgment, _
            strReceiverCode, blnReceiverPrefixYN, strRelease, blnUseCrLfYN, strEFileName, strPhone, strExt, _
            strEmail, strContact, strNotes)



        objClearingHouse = Nothing
 

    End Sub
    Public Sub UpdateEFile(ByVal lngID As Long, ByVal strEFileName As String)
        '--------------------------------------------------------------------
        'Date: 10/06/2004
        'Author: Duane C Orth
        'Description:  Updates EFileName in the tblClearingHouse table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Null
        '--------------------------------------------------------------------


        Dim objClearingHouse As New InsuranceDB.CClearingHouseDB
        Dim strErrMessage As String

        objClearingHouse.ConnectionString = _ConnectionString

        objClearingHouse.UpdateEFile(lngID, strEFileName)

        objClearingHouse = Nothing


    End Sub

    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Long, ByVal strDeletedBy As String)
        '--------------------------------------------------------------------
        'Date: 10/06/2004
        'Author: Duane C Orth
        'Description:  Flags a row in the tblClearingHouse table marking the row as
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

        Dim objClearingHouse As New InsuranceDB.CClearingHouseDB

        objClearingHouse.ConnectionString = _ConnectionString


        objClearingHouse.Deleted(blnDeleted, lngID, strDeletedBy)



        objClearingHouse = Nothing


    End Sub
End Class
