Option Strict Off
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CProviderTypeBZ
    '--------------------------------------------------------------------
    'Class Name: CProviderTypeBz
    'Date: 01/31/2000
    'Author: Rick "Boom Boom" Segura
    'Description:  MTS business object designed to  methods associated
    '              with the CProviderTypeBz class.
    '--------------------------------------------------------------------

    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CProviderTypeBz"

    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False, Optional ByVal strWhere As String = "", Optional ByVal strOrderBy As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 01/31/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Retrieves records from the tblProviderType table.
        'Parameters:  blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Disabled' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        '             strWhere - Optional 'Where' clause of the SQL statement by
        '               which records will be filtered.  If the parameter is not
        '               supplied, all records will be retrieved.
        '             strOrderBy - Optional 'Order By' clause in which retrieved
        '               records will be sorted.  The default is set to the
        '               'fldProviderType' column
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim objProviderType As New ListDb.CProviderTypeDB
        Dim rstSQL As New ADODB.Recordset


        rstSQL = objProviderType.Fetch(blnIncludeDisabled, strWhere, strOrderBy)
        Fetch = rstSQL

        'UPGRADE_NOTE: Object objProviderType may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objProviderType = Nothing


    End Function

    Public Function Exists(ByVal strCredential As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 01/31/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Determines if Provider Type description identical to the
        '              strDescription parameter already exists in the table.
        'Parameters: strDescription - Provider Type record to be checked
        'Returns: True if the name exists, false otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim objProviderType As New ListDb.CProviderTypeDB
        Dim blnExists As Boolean




        blnExists = objProviderType.Exists(strCredential)
        Exists = blnExists


        'UPGRADE_NOTE: Object objProviderType may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objProviderType = Nothing


    End Function


    Private Function VerifyData(ByVal lngID As Integer, ByVal strCredential As String, ByRef strErrMessage As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 01/31/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Verifies all required data has been provided by the user.
        'Parameters:  lngID - ID of row where strDescription should exist.  Value
        '               of 0 describes a new entry.
        '             strDescription - Value being sought in table.
        '             strErrMessage - String filled with error message if an error
        '               is encountered.
        'Returns: Boolean value identifying if all criteria has been satisfied.
        '--------------------------------------------------------------------

        If Trim(strCredential) = "" Then
            strErrMessage = "Credential is required."
            VerifyData = False
            Exit Function
        End If

        ' Check for existence only when inserting new data
        If lngID = 0 And Exists(strCredential) Then
            strErrMessage = "Provider Type '" & strCredential & "' already exists."
            VerifyData = False
            Exit Function
        End If

        VerifyData = True

    End Function

    Public Function Insert(ByVal strCredential As String, ByVal strDescription As String) As Integer
        '--------------------------------------------------------------------
        'Date: 01/31/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Inserts a row into the tblProviderType table utilizing
        '              a stored procedure.
        'Parameters: strDescription - The description of the Provider Type
        '              that will be inserted into the table.
        'Returns: ID (Primary Key) of the row inserted
        '--------------------------------------------------------------------


        Dim objProviderType As New ListDb.CProviderTypeDB
        Dim lngID As Integer
        Dim strErrMessage As String




        ' Verify data before proceeding
        If Not VerifyData(0, strCredential, strErrMessage) Then
            Exit Function
        End If

        lngID = objProviderType.Insert(strCredential, strDescription)

        'UPGRADE_NOTE: Object objProviderType may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objProviderType = Nothing


        Insert = lngID


    End Function

    Public Sub Update(ByVal lngID As Integer, ByVal strCredential As String, ByVal strDescription As String)
        '--------------------------------------------------------------------
        'Date: 01/31/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Updates a row into the tblProviderType table.
        'Parameters:  lngID - ID of the row in the table whose value will be
        '               updated.
        '             strDescription - The Provider Type description
        '                to which the record will be changed.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim objProviderType As New ListDb.CProviderTypeDB
        Dim strErrMessage As String




        ' Verify data before proceeding
        If Not VerifyData(lngID, strCredential, strErrMessage) Then
            Exit Sub
        End If

        objProviderType.Update(lngID, strCredential, strDescription)

        'UPGRADE_NOTE: Object objProviderType may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objProviderType = Nothing

    End Sub

    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Integer)
        '--------------------------------------------------------------------
        'Date: 01/31/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Flags a row in the tblProviderType table marking the row as
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


        Dim objProviderType As New ListDb.CProviderTypeDB




        objProviderType.Deleted(blnDeleted, lngID)

        'UPGRADE_NOTE: Object objProviderType may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objProviderType = Nothing

    End Sub
End Class