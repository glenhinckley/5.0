Option Strict Off
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII
Imports ListDb

Public Class CStateBz
    '--------------------------------------------------------------------
    'Class Name: CStateBz
    'Date: 12/01/1999
    'Author: Dave Richkun
    'Description:  MTS business object designed to  methods associated
    '              with the CStateDB class.
    '--------------------------------------------------------------------
    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CStateBz"








    Public Enum SortOrder
        StateCode = 0
        StateName = 1
    End Enum

    Public Function Fetch(Optional ByVal intSortOrder As SortOrder = SortOrder.StateCode) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 12/01/1999
        'Author: Dave Richkun
        'Description:  Retrieves records from the tblUserRoles table.
        'Parameters: intSortOrder - Identifies if the returned recordset is to
        '               be sorted by StateCode (default) or State Name.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim objState As New ListDb.CStateDB
        Dim rstSQL As New ADODB.Recordset

        '


        'objState = CreateObjectXXXXXXX("ListDB.CStateDB")

        '	rstSQL = objState.Fetch(intSortOrder)
        Fetch = objState.Fetch(intSortOrder)

        'Signal successful completion
        'System.EnterpriseServicesxxxxxxxXXXXXX.ContextUtil.SetComplete()

        'UPGRADE_NOTE: Object objState may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objState = Nothing

        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        '	System.EnterpriseServicesxxxxxxxXXXXXX.ContextUtil.SetAbort()
        'UPGRADE_NOTE: Object objState may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        '	objState = Nothing
        'Err.Raise(Err.Number, Err.Source, Err.Description)

    End Function
End Class