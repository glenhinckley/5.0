Option Strict Off
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CSrvGroupBz
    '--------------------------------------------------------------------
    'Class Name: CServiceGroupBz                                              '
    'Date: 12/01/1999                                                   '
    'Author: Dave Richkun                                               '
    'Description:  MTS business object designed to  methods         '
    '              associated with the CSrvGroupDB class.                 '
    '--------------------------------------------------------------------
    'Revision History:
    '  R001: 09/10/2001 Richkun - Changed to CServiceGroupBz from CMarketBz
    '  R002: 09/12/2001 Richkun - Added FetchByID() method
    '--------------------------------------------------------------------
    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CServiceGroupBz"

    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 12/01/1999                                                   '
        'Author: Dave Richkun                                               '
        'Description:  Retrieves records from the tblServiceGroup table.          '
        'Parameters: blnIncludeDisabled - Optional parameter that identifies'
        '              if records flagged as 'Disabled' or 'De-activated'   '
        '              are to be included in the record set. The default    '
        '              value is False.                                      '
        'Returns: Recordset of ServiceGroups                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '   03/02/2000 Removed Where and OrderBy parametes and implemented  '
        '              stored procedure uspSelServiceGroup                        '
        '--------------------------------------------------------------------

        Dim objSrvGroup As New ListDb.CSrvGroupDB
        Dim rstSQL As New ADODB.Recordset



        rstSQL = objSrvGroup.Fetch(blnIncludeDisabled)

        Fetch = rstSQL

        'UPGRADE_NOTE: Object objSrvGroup may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objSrvGroup = Nothing

    End Function


    Public Function FetchByID(ByVal lngSrvGroupID As Integer) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/12/2001
        'Author: Dave Richkun
        'Description:  Retrieves all Providers associated with a ServiceGroup
        'Parameters: lngSrvGroupID: ID of Service Group to fetch Providers for
        'Returns: Recordset of requested ServiceGroup Providers
        '--------------------------------------------------------------------
        'Revision History:
        '  R002: Created
        '--------------------------------------------------------------------

        Dim objSrvGroup As New ListDb.CSrvGroupDB
        Dim rstSQL As New ADODB.Recordset



        rstSQL = objSrvGroup.FetchByID(lngSrvGroupID)

        FetchByID = rstSQL

        'UPGRADE_NOTE: Object objSrvGroup may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objSrvGroup = Nothing

        'Signal successful completion

    End Function


    Public Function Insert(ByVal strDescription As String) As Integer
        '--------------------------------------------------------------------
        'Date: 12/01/1999
        'Author: Dave Richkun
        'Description:  Inserts a row into the tblServiceGroup table.
        'Parameters: strDescription - The description of the ServiceGroup
        '              that will be inserted into the table.
        'Returns: ID (Primary Key) of the row inserted
        '--------------------------------------------------------------------


        Dim objSrvGroup As New ListDb.CSrvGroupDB
        Dim lngID As Integer
        Dim strErrMessage As String





        'Verify data before proceeding.
        If Not VerifyData(0, strDescription, strErrMessage) Then
            Exit Function
        End If

        lngID = objSrvGroup.Insert(strDescription)

        Insert = lngID

        'Signal successful completion

        'Release resources
        'UPGRADE_NOTE: Object objSrvGroup may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objSrvGroup = Nothing


    End Function


    Public Sub Update(ByVal lngID As Integer, ByVal strDescription As String)
        '--------------------------------------------------------------------
        'Date: 12/01/1999
        'Author: Dave Richkun
        'Description:  Updates a row into the tblServiceGroup table.
        'Parameters:  lngID - ID of the row in the table whose value will be
        '               updated.
        '             strDescription - The ServiceGroup status description
        '                to which the record will be changed.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim objSrvGroup As New ListDb.CSrvGroupDB
        Dim strErrMessage As String





        'Verify data before proceeding.
        If Not VerifyData(lngID, strDescription, strErrMessage) Then
            Exit Sub
        End If

        objSrvGroup.Update(lngID, strDescription)

        'UPGRADE_NOTE: Object objSrvGroup may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objSrvGroup = Nothing
        'Signal successful completion


    End Sub


    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Integer)
        '--------------------------------------------------------------------
        'Date: 12/30/1999
        'Author: Dave Richkun
        'Description:  Flags a row in the tblServiceGroup table marking the row as
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


        Dim objSrvGroup As New ListDb.CSrvGroupDB





        objSrvGroup.Deleted(blnDeleted, lngID)
        'UPGRADE_NOTE: Object objSrvGroup may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objSrvGroup = Nothing
        'Signal successful completion
    End Sub


    Public Function Exists(ByVal strDescription As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 01/05/2000
        'Author: Dave Richkun
        'Description:  Determines if a ServiceGroup description identical to the
        '              strDescription parameter already exists in the table.
        'Parameters: strDescription - ServiceGroup name to be checked
        'Returns: True if the name exists, false otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim objSrvGroup As New ListDb.CSrvGroupDB
        Dim blnExists As Boolean





        blnExists = objSrvGroup.Exists(strDescription)
        Exists = blnExists

        'UPGRADE_NOTE: Object objSrvGroup may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objSrvGroup = Nothing

    End Function


    Private Function VerifyData(ByVal lngID As Integer, ByVal strDescription As String, ByRef strErrMessage As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 01/04/2000
        'Author: Dave Richkun
        'Description:  Verifies all required data has been provided by the user.
        'Parameters:  The values to be checked.
        'Returns: Boolean value identifying if all criteria has been satisfied.
        '--------------------------------------------------------------------

        If Trim(strDescription) = "" Then
            strErrMessage = "ServiceGroup Name is required."
            VerifyData = False
            Exit Function
        End If

        'Check for existance only when inserting new data
        If lngID = 0 And Exists(strDescription) Then
            strErrMessage = "ServiceGroup '" & strDescription & "' already exists."
            VerifyData = False
            Exit Function
        End If

        'If we get here, all is well...
        VerifyData = True

    End Function
End Class