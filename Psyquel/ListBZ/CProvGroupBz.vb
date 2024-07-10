Option Strict Off
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CProvGroupBz
    '--------------------------------------------------------------------
    'Class Name: CProvGroupBz
    'Date: 03/08/2002
    'Author: Eric Pena
    'Description:  MTS business object designed to  methods associated with the CProvGroupDB class.
    '--------------------------------------------------------------------
    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CProvGroupBz"
    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/08/2002
        'Author: Eric Pena
        'Description:  Retrieves records from the tblServiceGroup table.          '
        'Parameters: blnIncludeDisabled - Optional parameter that identifies'
        '               if records flagged as 'Disabled' or 'De-activated'  '
        '               are to be included in the record set. The default   '
        '               value is False.                                     '
        'Returns: Recordset of requested ServiceGroups                            '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '--------------------------------------------------------------------

        Dim objProvGroup As New ListDb.CProvGroupDB
        Dim rstSQL As New ADODB.Recordset



        rstSQL = objProvGroup.Fetch(blnIncludeDisabled)
        Fetch = rstSQL

        'UPGRADE_NOTE: Object objProvGroup may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objProvGroup = Nothing

    End Function
    Public Function Insert(ByVal strGroupName As String, ByVal strTIN As String, ByVal strNPI As String, ByVal strTaxonomy As String) As Integer
        '--------------------------------------------------------------------
        'Date: 03/08/2002
        'Author: Eric Pena
        'Description:  Inserts a row into the tblProviderGroup table utilizing
        '              a stored procedure.
        'Parameters: strGroupName - The name of the group
        '              strTIN - TIN that will be used in billing
        'Returns: ID (Primary Key) of the row inserted
        '--------------------------------------------------------------------
        Dim objProvGroup As New ListDb.CProvGroupDB
        Dim lngID As Integer
        Dim strErrMessage As String





        'Verify data before proceeding.
        lngID = objProvGroup.Insert(strGroupName, strTIN, strNPI, strTaxonomy)

        'UPGRADE_NOTE: Object objProvGroup may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objProvGroup = Nothing

        'Signal successful completion

        Insert = lngID


    End Function
    Public Sub Update(ByVal lngID As Integer, ByVal strGroupName As String, ByVal strTIN As String, ByVal strNPI As String, ByVal strTaxonomy As String)
        '--------------------------------------------------------------------
        'Date: 03/08/2002
        'Author: Eric Pena
        'Description:  Updates a row into the tblProviderGroup table utilizing
        '              a stored procedure.
        'Parameters:  lngID - ID of the row in the table whose value will be
        '               updated.
        '             strGroupName - The name of the group
        '              strTIN - TIN that will be used in billing
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim objProvGroup As New ListDb.CProvGroupDB
        Dim strErrMessage As String



        objProvGroup.Update(lngID, strGroupName, strTIN, strNPI, strTaxonomy)

        'UPGRADE_NOTE: Object objProvGroup may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objProvGroup = Nothing


    End Sub
    Public Sub Deleted(ByVal lngProvGroupID As Integer, ByVal blnDeleted As Boolean, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 03/08/2002
        'Author: Eric Pena
        'Description:  Disables/enables a record in the tblAlert table utilizing
        '              a stored procedure.
        'Parameters: lngProvGroupID -- the ID of the row to be disabled/enabled
        '            blnDeleted -- true/false: whether row should be disabled or
        '                          enabled
        'Returns: Null
        '--------------------------------------------------------------------
        Dim objProvGroup As New ListDb.CProvGroupDB





        objProvGroup.Deleted(lngProvGroupID, blnDeleted, strUserName)

        'UPGRADE_NOTE: Object objProvGroup may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objProvGroup = Nothing


    End Sub
    Public Function FetchByID(ByVal lngProvGroupID As Integer) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/08/2002
        'Author: Eric Pena
        'Description:  Retrieves all Providers associated with a ProviderGroup
        'Parameters: lngProvGroupID: ID of Provider Group to fetch Providers for
        'Returns: Recordset of requested ProviderGroup Providers
        '--------------------------------------------------------------------
        'Revision History:
        '--------------------------------------------------------------------

        Dim objProvGroup As New ListDb.CProvGroupDB
        Dim rstSQL As New ADODB.Recordset



        rstSQL = objProvGroup.FetchByID(lngProvGroupID)

        FetchByID = rstSQL

        'UPGRADE_NOTE: Object objProvGroup may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objProvGroup = Nothing


    End Function
End Class