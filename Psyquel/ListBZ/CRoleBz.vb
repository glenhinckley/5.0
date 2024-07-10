Option Strict Off
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CRoleBz
    '--------------------------------------------------------------------
    'Class Name: CRoleDB
    'Date: 12/01/1999
    'Author: Dave Richkun
    'Description:  MTS business object designed to  methods associated
    '              with the CRoleDB class.
    '--------------------------------------------------------------------

    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CRoleDB"


    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False, Optional ByVal strWhere As String = "", Optional ByVal strOrderBy As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 12/01/1999
        'Author: Dave Richkun
        'Description:  Retrieves records from the tblUserRoles table.
        'Parameters: blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Disabled' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        '            strWhere - Optional 'Where' clause of the SQL statement by
        '               which records will be filtered.  If the parameter is not
        '               supplied, all records will be retrieved.
        '             strOrderBy - Optional 'Order By' clause in which retrieved
        '               records will be sorted.  The default is set to the
        '               'fldRole' column
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim rstSQL As New ADODB.Recordset
        Dim objRole As New ListDb.CRoleDB





        rstSQL = objRole.Fetch(blnIncludeDisabled, strWhere, strOrderBy)
        Fetch = rstSQL

        'Signal successful completion


        'UPGRADE_NOTE: Object objRole may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objRole = Nothing

 

    End Function


    Public Function Insert(ByVal strRoleName As String, ByVal strDescription As String) As Integer
        '--------------------------------------------------------------------
        'Date: 12/01/1999
        'Author: Dave Richkun
        'Description:  Inserts a row into the tblUserRoles table utilizing
        '              a stored procedure.
        'Parameters: strRoleName - The name given to the Role
        '            strDescription - The description of the Role
        '              that will be inserted into the table.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------


        Dim objRole As New ListDb.CRoleDB
        Dim lngID As Integer
        Dim strErrMessage As String





        'Verify data before proceeding.
        If Not VerifyData(0, strRoleName, strErrMessage) Then
            Exit Function
        End If

        lngID = objRole.Insert(strRoleName, strDescription)
        Insert = lngID

        'Signal successful completion


        'UPGRADE_NOTE: Object objRole may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objRole = Nothing


    End Function


    Public Sub Update(ByVal lngID As Integer, ByVal strRoleName As String, ByVal strDescription As String)
        '--------------------------------------------------------------------
        'Date: 12/05/1999
        'Author: Dave Richkun
        'Description:  Updates a row into the tblUserRoles table utilizing
        '              a stored procedure.
        'Parameters:  lngID - ID of the row in the table whose value will be
        '               updated.
        '             strRoleName - The name given to the role.
        '             strDescription - The Role status description
        '                to which the record will be changed.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim objRole As New ListDb.CRoleDB
        Dim strErrMessage As String





        'Verify data before proceeding.
        If Not VerifyData(lngID, strRoleName, strErrMessage) Then
            Exit Sub
        End If

        objRole.Update(lngID, strRoleName, strDescription)

        'UPGRADE_NOTE: Object objRole may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objRole = Nothing

        'Signal successful completion


        'UPGRADE_NOTE: Object objRole may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objRole = Nothing


    End Sub


    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Integer)
        '--------------------------------------------------------------------
        'Date: 12/30/1999
        'Author: Dave Richkun
        'Description:  Flags a row in the tblRoles table marking the row as
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


        Dim objRole As New ListDb.CRoleDB





        objRole.Deleted(blnDeleted, lngID)

        'Signal successful completion


        'UPGRADE_NOTE: Object objRole may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objRole = Nothing

  

    End Sub


    Public Function Exists(ByVal strRoleName As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 01/05/2000
        'Author: Dave Richkun
        'Description:  Determines if an Employment Status description identical
        '              to the strDescription parameter already exists in the table.
        'Parameters: strRoleName - Role record to be checked
        'Returns: True if the name exists, false otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim objRole As New ListDb.CRoleDB
        Dim blnExists As Boolean





        blnExists = objRole.Exists(strRoleName)
        Exists = blnExists

        'Signal successful completion


        'UPGRADE_NOTE: Object objRole may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objRole = Nothing


    End Function



    Private Function VerifyData(ByVal lngID As Integer, ByVal strRoleName As String, ByRef strErrMessage As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 01/04/2000
        'Author: Dave Richkun
        'Description:  Verifies all required data has been provided by the user.
        'Parameters:  The values to be checked.
        'Returns: Boolean value identifying if all criteria has been satisfied.
        '--------------------------------------------------------------------

        If Trim(strRoleName) = "" Then
            strErrMessage = "Role name is required."
            VerifyData = False
            Exit Function
        End If

        'Check for existance only when inserting new data
        If lngID = 0 And Exists(strRoleName) Then
            strErrMessage = "Role name '" & strRoleName & "' already exists."
            VerifyData = False
            Exit Function
        End If

        'If we get here, all is well...
        VerifyData = True

    End Function
End Class