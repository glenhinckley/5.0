Option Strict Off
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII


Imports System

Public Class CStateDB
    '--------------------------------------------------------------------
    'Class Name: CStateSel
    'Date: 12/05/1999
    'Author: Dave Richkun
    'Description:  MTS object designed to retrieve records from the
    '              tblState table.
    '--------------------------------------------------------------------
    ' Revision History:                                                 '
    '   08/22/2000 RS:  Made connection improvements and leak checks    '
    '--------------------------------------------------------------------

    Private _ConnectionString As String = String.Empty

    Private Const TABLE_NAME As String = "tblState"
    Public Enum SortOrder
        StateCode = 0
        StateName = 1 '
    End Enum
    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property


    Public Function Fetch(Optional ByVal intSortOrder As SortOrder = SortOrder.StateCode) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 12/05/1999
        'Author: Dave Richkun
        'Description:  Retrieves records from the tblState table.
        'Parameters: None
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As New ADODB.Recordset
        Dim strSQL As String

        '      



        'Prepare the SQL statement.
        strSQL = "SELECT * "
        strSQL = strSQL & "FROM "
        strSQL = strSQL & TABLE_NAME

        If intSortOrder = SortOrder.StateName Then
            strSQL = strSQL & " ORDER BY "
            strSQL = strSQL & "fldStateName "
        Else
            strSQL = strSQL & " ORDER BY "
            strSQL = strSQL & "fldStateCode "
        End If

        'Instantiate the Recordset.
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = ADODB.CursorLocationEnum.adUseClient

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Populate the Recordset
        rstSQL.Open(strSQL, cnnSQL, ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly, ADODB.CommandTypeEnum.adCmdText + ADODB.ExecuteOptionEnum.adAsyncFetch)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        'UPGRADE_NOTE: Object rstSQL.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        rstSQL.ActiveConnection = Nothing

        cnnSQL.Close()
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing
        Fetch = rstSQL

        'Signal successful completion
        '      

        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        '       System.EnterpriseServicesxxxxxxxXXXXXX.ContextUtil.SetAbort()
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing
        'UPGRADE_NOTE: Object rstSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        rstSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function
End Class