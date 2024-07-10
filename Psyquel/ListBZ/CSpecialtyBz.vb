Option Strict Off
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CSpecialtyBz
    '--------------------------------------------------------------------
    'Class Name: CSpecialtyBz
    'Date: 09/20/2001
    'Author: Dave Richkun
    'Description:  COM+ business object designed to  methods
    '              associated with the CSpecialtyDB class.
    '--------------------------------------------------------------------
    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CSpecialtyBz"

    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/20/2001
        'Author: Dave Richkun
        'Description:  Retrieves records from the tblSpecialty table.
        'Parameters:  blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Disabled' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objSpec As New ListDb.CSpecialtyDB
        Dim rstSQL As New ADODB.Recordset




        rstSQL = objSpec.Fetch(blnIncludeDisabled)
        Fetch = rstSQL

        'UPGRADE_NOTE: Object objSpec may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objSpec = Nothing


    End Function

    Public Function Insert(ByVal strDescription As String) As Integer
        '--------------------------------------------------------------------
        'Date: 09/20/2001
        'Author: Dave Richkun
        'Description:  Inserts a row into the tblSpecialty table utilizing
        '              a stored procedure.
        'Parameters: strDescription - The description of the Specialty
        '              that will be inserted into the table.
        'Returns: ID (Primary Key) of the row inserted
        '--------------------------------------------------------------------

        Dim objSpec As New ListDb.CSpecialtyDB
        Dim lngID As Integer
        Dim strErrMessage As String



        'Verify data before proceeding.
        If Not VerifyData(0, strDescription, strErrMessage) Then
            Exit Function
        End If

        lngID = objSpec.Insert(strDescription)

        'UPGRADE_NOTE: Object objSpec may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objSpec = Nothing


        Insert = lngID


    End Function

    Public Sub Update(ByVal lngID As Integer, ByVal strDescription As String)
        '--------------------------------------------------------------------
        'Date: 09/20/2001
        'Author: Dave Richkun
        'Description:  Updates a row into the tblSpecialty table.
        'Parameters:  lngID - ID of the row in the table whose value will be
        '               updated.
        '             strDescription - The Specialty description to which the
        '               record will be changed.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objSpec As New ListDb.CSpecialtyDB
        Dim strErrMessage As String



        'Verify data before proceeding.
        If Not VerifyData(lngID, strDescription, strErrMessage) Then
            Exit Sub
        End If

        objSpec.Update(lngID, strDescription)

        'UPGRADE_NOTE: Object objSpec may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objSpec = Nothing


    End Sub


    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Integer)
        '--------------------------------------------------------------------
        'Date: 09/20/2001
        'Author: Dave Richkun
        'Description:  Flags a row in the tblSpecialty table marking the row as
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

        Dim objSpec As New ListDb.CSpecialtyDB




        objSpec.Deleted(blnDeleted, lngID)


        'UPGRADE_NOTE: Object objSpec may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objSpec = Nothing


    End Sub

    Public Function Exists(ByVal strDescription As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 09/20/2001
        'Author: Dave Richkun
        'Description:  Determines if a Specialty description identical
        '              to the strDescription parameter already exists in the table.
        'Parameters: strDescription - Specialty description to be checked
        'Returns: True if the name exists, false otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objSpec As New ListDb.CSpecialtyDB
        Dim blnExists As Boolean




        blnExists = objSpec.Exists(strDescription)
        Exists = blnExists

        'Signal successful completion

        'UPGRADE_NOTE: Object objSpec may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objSpec = Nothing

    End Function



    '-----------------------------
    ' Private Functions
    '-----------------------------

    Private Function VerifyData(ByVal lngID As Integer, ByVal strDescription As String, ByRef strErrMessage As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 09/20/2001
        'Author: Dave Richkun
        'Description:  Verifies all required data has been provided by the user.
        'Parameters:  The values to be checked.
        'Returns: Boolean value identifying if all criteria has been satisfied.
        '--------------------------------------------------------------------

        If Trim(strDescription) = "" Then
            strErrMessage = "Description is required."
            VerifyData = False
            Exit Function
        End If

        'Check for existance only when inserting new data
        If lngID = 0 And Exists(strDescription) Then
            strErrMessage = "Practice Specialty '" & strDescription & "' already exists."
            VerifyData = False
            Exit Function
        End If

        'If we get here, all is well...
        VerifyData = True

    End Function
End Class