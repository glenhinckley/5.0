Option Strict Off
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CEthnicBZ
    '--------------------------------------------------------------------
    'Class Name: CEthnicBz
    'Date: 01/20/2000
    'Author: Rick "Boom Boom" Segura
    'Description:  MTS business object designed to  methods associated
    '              with the CEthnicDB class.
    '--------------------------------------------------------------------

    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CEthnicBz"

    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False, Optional ByVal strWhere As String = "", Optional ByVal strOrderBy As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 01/20/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Retrieves records from the tblEthnicity table.
        'Parameters:  blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Disabled' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        '             strWhere - Optional 'Where' clause of the SQL statement by
        '               which records will be filtered.  If the parameter is not
        '               supplied, all records will be retrieved.
        '             strOrderBy - Optional 'Order By' clause in which retrieved
        '               records will be sorted.  The default is set to the
        '               'fldEthnicity' column
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim objEthnic As New ListDb.CEthnicDB
        Dim rstSQL As New ADODB.Recordset



        rstSQL = objEthnic.Fetch(blnIncludeDisabled, strWhere, strOrderBy)
        Fetch = rstSQL

        'UPGRADE_NOTE: Object objEthnic may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objEthnic = Nothing



    End Function

    Public Function Exists(ByVal strDescription As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 01/20/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Determines if an Ethnicity description identical to the
        '              strDescription parameter already exists in the table.
        'Parameters: strDescription - Ethnicity record to be checked
        'Returns: True if the name exists, false otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim objEthnic As New ListDb.CEthnicDB
        Dim blnExists As Boolean



        blnExists = objEthnic.Exists(strDescription)
        Exists = blnExists


        'UPGRADE_NOTE: Object objEthnic may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objEthnic = Nothing


    End Function


    Private Function VerifyData(ByVal lngID As Integer, ByVal strDescription As String, ByRef strErrMessage As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 01/20/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Verifies all required data has been provided by the user.
        'Parameters:  lngID - ID of row where strDescription should exist.  Value
        '               of 0 describes a new entry.
        '             strDescription - Value being sought in table.
        '             strErrMessage - String filled with error message if an error
        '               is encountered.
        'Returns: Boolean value identifying if all criteria has been satisfied.
        '--------------------------------------------------------------------

        If Trim(strDescription) = "" Then
            strErrMessage = "Description is required."
            VerifyData = False
            Exit Function
        End If

        ' Check for existence only when inserting new data
        If lngID = 0 And Exists(strDescription) Then
            strErrMessage = "Ethnicity '" & strDescription & "' already exists."
            VerifyData = False
            Exit Function
        End If

        VerifyData = True

    End Function

    Public Function Insert(ByVal strDescription As String) As Integer
        '--------------------------------------------------------------------
        'Date: 01/20/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Inserts a row into the tblEthnicity table utilizing
        '              a stored procedure.
        'Parameters: strDescription - The description of the Ethnicity
        '              that will be inserted into the table.
        'Returns: ID (Primary Key) of the row inserted
        '--------------------------------------------------------------------


        Dim objEthnic As New ListDb.CEthnicDB
        Dim lngID As Integer
        Dim strErrMessage As String




        ' Verify data before proceeding
        If Not VerifyData(0, strDescription, strErrMessage) Then
            Exit Function

        End If

        lngID = objEthnic.Insert(strDescription)

        'UPGRADE_NOTE: Object objEthnic may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objEthnic = Nothing


        Insert = lngID


    End Function

    Public Sub Update(ByVal lngID As Integer, ByVal strDescription As String)
        '--------------------------------------------------------------------
        'Date: 01/20/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Updates a row into the tblEthnicity table.
        'Parameters:  lngID - ID of the row in the table whose value will be
        '               updated.
        '             strDescription - The Ethnicity description
        '                to which the record will be changed.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim objEthnic As New ListDb.CEthnicDB
        Dim strErrMessage As String



        ' Verify data before proceeding
        If Not VerifyData(lngID, strDescription, strErrMessage) Then
            Exit Sub
        End If

        objEthnic.Update(lngID, strDescription)

        'UPGRADE_NOTE: Object objEthnic may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objEthnic = Nothing

        ' Signal successful completion



    End Sub

    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Integer)
        '--------------------------------------------------------------------
        'Date: 01/20/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Flags a row in the tblEthnicity table marking the row as
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


        Dim objEthnic As New ListDb.CEthnicDB


        objEthnic.Deleted(blnDeleted, lngID)

        ' Signal successful completion


        'UPGRADE_NOTE: Object objEthnic may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objEthnic = Nothing



    End Sub
End Class