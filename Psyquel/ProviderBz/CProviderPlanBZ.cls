
'--------------------------------------------------------------------
'Class Name: CProviderPlanBZ
'Date: 03/05/2010
'Author: Duane C Orth
'Denis J Finkel
'Description:  MTS business object designed to   methods
'              associated with the CProviderPlanDB class.
'--------------------------------------------------------------------
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

Public Class CProviderPlanBZ


    Private _ConnectionString As String = String.Empty


    Private Const CLASS_NAME As String = "CProviderPlanBZ"
    Private Const TABLE_NAME As String = "tblProviderPlan"


    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property



    Public Function Fetch(ByVal lngProviderID As Long, Optional ByVal blnDefault As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/05/2010
        'Author: Duane C Orth
        'Description:  Retrieves a recordset associated Provider CPT Code's and related
        '              info for the given provider ID
        'Parameters:  lngID - ID of provider to search info for
        'Returns: Recordset of requested record(s)
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim obj As New ProviderDB.CProviderPlanDB
        Dim rst As New ADODB.Recordset

        obj.ConnectionString = _ConnectionString


        rst = obj.Fetch(lngProviderID, blnDefault)

        obj = Nothing

        Fetch = rst



    End Function
    Public Function Insert(ByVal lngProviderID As Long, _
                           ByVal lngPlanID As Long, _
                           ByVal blnDefault As Boolean) As Long
        '--------------------------------------------------------------------
        'Date: 03/05/2010
        'Author: Duane C Orth
        'Description:  Inserts a row into the tblProviderCPT table utilizing
        '              a stored procedure.
        'Parameters: lngProviderID - internal provider ID
        '            lngCPTRecordID - internal CPT Record ID
        '            blnDefault -
        'Returns: Nothing
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim obj As New ProviderDB.CProviderPlanDB
        Dim rst As New ADODB.Recordset
        Dim lngID As Long
        Dim strErrMessage As String

        obj.ConnectionString = _ConnectionString


        obj.Insert(lngProviderID, lngPlanID, blnDefault)
        obj = Nothing



        Insert = lngID



    End Function
    Public Sub Delete(ByVal lngProviderPlanRecordID As Long)
        '--------------------------------------------------------------------
        'Date: 03/05/2010
        'Author: Duane C Orth
        'Description: 'Deletes' selected record in tblProviderAccess table.   s
        '               stored procedure uspDelProviderAccess.
        'Parameters: lngProviderCPTRecordID - ID of relationship to be deleted.
        'Returns: Nothing
        '--------------------------------------------------------------------
        Dim obj As New ProviderDB.CProviderPlanDB
        Dim strErrMessage As String

        obj.ConnectionString = _ConnectionString

        obj.Delete(lngProviderPlanRecordID)


    End Sub


End Class

