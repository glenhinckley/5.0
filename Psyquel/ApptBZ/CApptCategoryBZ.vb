Option Strict Off
Option Explicit On
Imports ADODB
Imports Psyquel.BusinessRules.CoreLibraryIII
Public Class CApptCategoryBZ

    Private _ConnectionString As String = String.Empty

    '--------------------------------------------------------------------
    'Class Name: CApptCategoryBz                                        '
    'Date: 08/28/2000                                                   '
    'Author: Rick "Boom Boom" Segura                                    '
    'Description:  MTS business object designed to  methods         '
    '              associated with the CApptCategoryDB class.           '
    '--------------------------------------------------------------------

    Private Const CLASS_NAME As String = "CApptCategoryBz"

    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset

        '--------------------------------------------------------------------
        'Date: 08/28/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Retrieves records from the tblApptCategory table.    '
        'Parameters: blnIncludeDisabled - Optional parameter that identifies'
        '              if records flagged as 'Disabled' or 'De-activated'   '
        '              are to be included in the record set. The default    '
        '              value is False.                                      '
        'Returns: Recordset of appointment categories                       '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '--------------------------------------------------------------------
        Dim objAppt As New ApptDB.CApptCategoryDB
        Dim rstSQL As New ADODB.Recordset



        '  objAppt = CreateObjectXXXXXXX("ApptDB.CApptCategoryDB")
        'UPGRADE_WARNING: Couldn't resolve default property of object objAppt.Fetch. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        rstSQL = objAppt.Fetch(blnIncludeDisabled)

        Fetch = rstSQL

        'UPGRADE_NOTE: Object objAppt may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objAppt = Nothing



    End Function

    Public Function Insert(ByVal strDescription As String) As Integer

        '--------------------------------------------------------------------
        'Date: 08/28/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Inserts a row into the tblApptCategory table.        '
        'Parameters: strDescription - The description of the Appt Category  '
        '              that will be inserted into the table.                '
        'Returns: ID (Primary Key) of the row inserted                      '
        '--------------------------------------------------------------------
        Dim objAppt As New ApptDB.CApptCategoryDB
        Dim lngID As Integer
        Dim strErrMessage As String



        'Verify data before proceeding.
        If Not VerifyData(0, strDescription, strErrMessage) Then
            Exit Function

        End If

        '  objAppt = CreateObjectXXXXXXX("ApptDB.CApptCategoryDB")
        'UPGRADE_WARNING: Couldn't resolve default property of object objAppt.Insert. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        lngID = objAppt.Insert(strDescription)

        Insert = lngID

        'Signal successful completion


        'Release resources
        'UPGRADE_NOTE: Object objAppt may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objAppt = Nothing

    End Function


    Public Sub Update(ByVal lngID As Integer, ByVal strDescription As String)

        '--------------------------------------------------------------------
        'Date: 08/28/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Updates a row into the tblApptCategory table.        '
        'Parameters:  lngID - ID of the row in the table whose value will be'
        '               updated.                                            '
        '             strDescription - The appointment Category description '
        '                to which the record will be changed.               '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------
        Dim objAppt As New ApptDB.CApptCategoryDB
        Dim strErrMessage As String



        'Verify data before proceeding.
        If Not VerifyData(lngID, strDescription, strErrMessage) Then
            Exit Sub
        End If

        ' objAppt = CreateObjectXXXXXXX("ApptDB.CApptCategoryDB")
        'UPGRADE_WARNING: Couldn't resolve default property of object objAppt.Update. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        objAppt.Update(lngID, strDescription)

        'UPGRADE_NOTE: Object objAppt may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objAppt = Nothing
        'Signal successful completion


    End Sub


    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Integer)

        '--------------------------------------------------------------------
        'Date: 08/28/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Flags a row in the tblApptCategory table marking the row
        '               as deleted or undeleted.                            '
        'Parameters: blnDeleted - Boolean value identifying if the record is'
        '               to be deleted (True) or undeleted (False).          '
        '            lngID - ID of the row in the table whose value will be '
        '               updated.                                            '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------
        Dim objAppt As New ApptDB.CApptCategoryDB


        'UPGRADE_WARNING: Couldn't resolve default property of object objAppt.Deleted. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        objAppt.Deleted(blnDeleted, lngID)
        'UPGRADE_NOTE: Object objAppt may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objAppt = Nothing



    End Sub


    Public Function Exists(ByVal strDescription As String) As Boolean

        '--------------------------------------------------------------------
        'Date: 08/28/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Determines if an appoinment Category description     '
        '               identical to the strDescription parameter already   '
        '               exists in the table.                                '
        'Parameters: strDescription - Appointment Category name to be checked
        'Returns: True if the name exists, false otherwise                  '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------
        Dim objAppt As New ApptDB.CApptCategoryDB
        Dim blnExists As Boolean




        'UPGRADE_WARNING: Couldn't resolve default property of object objAppt.Exists. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        blnExists = objAppt.Exists(strDescription)
        Exists = blnExists

        'UPGRADE_NOTE: Object objAppt may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objAppt = Nothing


    End Function


    Private Function VerifyData(ByVal lngID As Integer, ByVal strDescription As String, ByRef strErrMessage As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 08/28/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Verifies all required data has been provided by the user.
        'Parameters:  The values to be checked.                             '
        'Returns: Boolean value identifying if all criteria has been satisfied.
        '--------------------------------------------------------------------

        If Trim(strDescription) = "" Then
            strErrMessage = "Market Name is required."
            VerifyData = False
            Exit Function
        End If

        'Check for existance only when inserting new data
        If lngID = 0 And Exists(strDescription) Then
            strErrMessage = "Appointment Category '" & strDescription & "' already exists."
            VerifyData = False
            Exit Function
        End If

        'If we get here, all is well...
        VerifyData = True

    End Function
End Class