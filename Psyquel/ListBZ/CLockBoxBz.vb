Option Strict Off
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CLockBoxBz
    '--------------------------------------------------------------------
    'Class Name: CLockBoxBz                                             '
    'Date: 06/06/2000                                                   '
    'Author: Rick "Boom Boom" Segura                                    '
    'Description:  MTS business object designed to  methods         '
    '              associated with the CLockboxDB class.                '
    '--------------------------------------------------------------------
    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CLockBoxBz"

    '--------------------------------------------------------------------
    ' Public Methods                                                    '
    '--------------------------------------------------------------------

    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Integer)
        '--------------------------------------------------------------------
        'Date: 06/06/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Flags a row in the tblLockBox table marking the row as
        '              deleted or undeleted.                                '
        'Parameters: blnDeleted - Boolean value identifying if the record is'
        '               to be deleted (True) or undeleted (False).          '
        '            lngID - ID of the row in the table whose value will be '
        '               updated.                                            '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------


        Dim objLockBox As New ListDb.CLockBoxDB





        objLockBox.Deleted(blnDeleted, lngID)

        'UPGRADE_NOTE: Object objLockBox may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objLockBox = Nothing



    End Sub

    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 06/06/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Retrieves records from the tblLockBox table.         '
        'Parameters: None                                                   '
        'Returns: Recordset of markets                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim objLockBox As New ListDb.CLockBoxDB
        Dim rstSQL As New ADODB.Recordset



        rstSQL = objLockBox.Fetch(blnIncludeDisabled)

        Fetch = rstSQL

        'UPGRADE_NOTE: Object objLockBox may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objLockBox = Nothing

    End Function


    Public Function FetchForProvider(ByVal lngProviderID As Integer) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/21/2000                                                   '
        'Author: Dave Richkun
        'Description:  Retrieves LockBox records for a specific provider
        'Parameters: lngProvdierID - ID of Provider to fetch records for
        'Returns: Recordset of Provider's lockboxes
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim objLockBox As New ListDb.CLockBoxDB
        Dim rstSQL As New ADODB.Recordset



        rstSQL = objLockBox.FetchForProvider(lngProviderID)

        FetchForProvider = rstSQL

        'UPGRADE_NOTE: Object objLockBox may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objLockBox = Nothing


    End Function


    Public Function Insert(ByVal strIdentifier As String, ByVal strAddress As String, ByVal strCity As String, ByVal strState As String, ByVal strZip As String) As Integer
        '--------------------------------------------------------------------
        'Date: 06/05/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Inserts a row into the tblLockBox table.             '
        'Parameters: strIdentifier - Name to distinguish lock boxes         '
        '            strAddress - "Street" address of lock box              '
        '            strCity - City address of lock box                     '
        '            strState - State address of Lock Box                   '
        '            strZip -  Zip code of Lack Box                         '
        'Returns: ID (Primary Key) of the row inserted                      '
        '--------------------------------------------------------------------


        Dim objLockBox As New ListDb.CLockBoxDB
        Dim lngID As Integer
        Dim strErrMessage As String





        'Verify data before proceeding.
        If Not VerifyData(strIdentifier, strAddress, strCity, strState, strZip, strErrMessage) Then
            Exit Function
        End If


        If objLockBox.Exists(strIdentifier) Then
            strErrMessage = "Duplicate Lock Box Identifier Violation"
            'UPGRADE_NOTE: Object objLockBox may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            objLockBox = Nothing
            Exit Function
        End If

        lngID = objLockBox.Insert(strIdentifier, strAddress, strCity, strState, strZip)

        Insert = lngID

        'UPGRADE_NOTE: Object objLockBox may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objLockBox = Nothing
        'Signal successful completion


    End Function

    Public Sub Update(ByVal lngID As Integer, ByVal strIdentifier As String, ByVal strAddress As String, ByVal strCity As String, ByVal strState As String, ByVal strZip As String)
        '--------------------------------------------------------------------
        'Date: 06/06/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Updates a row into the tblLockBox table.             '
        'Parameters: lngID - ID of the row in the table whose value will be '
        '               updated.                                            '
        '            strIdentifier - Name to distinguish lock boxes         '
        '            strAddress - "Street" address of lock box              '
        '            strCity - City address of lock box                     '
        '            strState - State address of Lock Box                   '
        '            strZip -  Zip code of Lack Box                         '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim objLockBox As New ListDb.CLockBoxDB
        Dim strErrMessage As String





        'Verify data before proceeding.
        If Not VerifyData(strIdentifier, strAddress, strCity, strState, strZip, strErrMessage) Then
            Exit Sub
        End If


        If objLockBox.Exists(strIdentifier, lngID) Then
            strErrMessage = "Duplicate Lock Box Identifier Violation"
            'UPGRADE_NOTE: Object objLockBox may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            objLockBox = Nothing
            Exit Sub
        End If

        objLockBox.Update(lngID, strIdentifier, strAddress, strCity, strState, strZip)

        'UPGRADE_NOTE: Object objLockBox may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objLockBox = Nothing


    End Sub

    '--------------------------------------------------------------------
    ' Private Methods                                                   '
    '--------------------------------------------------------------------

    Private Function VerifyData(ByVal strIdentifier As String, ByVal strAddress As String, ByVal strCity As String, ByVal strState As String, ByVal strZip As String, ByRef strErrMessage As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 06/06/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Verifies all required data has been provided by the  '
        '              user.                                                '
        'Parameters:  The values to be checked.                             '
        'Returns: Boolean value identifying if all criteria has been        '
        '         satisfied.                                                '
        '--------------------------------------------------------------------

        If Trim(strIdentifier) = "" Then
            strErrMessage = "Lock Box identifier is required."
            VerifyData = False
            Exit Function
        End If

        If Trim(strAddress) = "" Then
            strErrMessage = "Lock Box 'street' address is required."
            VerifyData = False
            Exit Function
        End If

        If Trim(strCity) = "" Then
            strErrMessage = "Lock Box city is required."
            VerifyData = False
            Exit Function
        End If

        If Trim(strState) = "" Then
            strErrMessage = "Lock Box state is required."
            VerifyData = False
            Exit Function
        End If

        If Trim(strZip) = "" Then
            strErrMessage = "Lock Box zip is required."
            VerifyData = False
            Exit Function
        End If

        'If we get here, all is well...
        VerifyData = True

    End Function
End Class