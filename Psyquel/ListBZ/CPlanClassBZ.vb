Option Strict Off
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CPlanClassBz
    '--------------------------------------------------------------------
    'Class Name: CPlanClassBz
    'Date: 04/26/2000
    'Author: Eric Pena
    'Description:  MTS business object designed to  methods associated with the CPlanClassDB class.
    '--------------------------------------------------------------------
    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CPlanClassBz"

    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False, Optional ByVal strWhere As String = "", Optional ByVal strOrderBy As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 04/26/2000
        'Author: Eric Pena
        'Description:  Retrieves records from the tblPlanClassification table.
        'Parameters:  blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Disabled' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        '             strWhere - Optional 'Where' clause of the SQL statement by
        '               which records will be filtered.  If the parameter is not
        '               supplied, all records will be retrieved.
        '             strOrderBy - Optional 'Order By' clause in which retrieved
        '               records will be sorted.  The default is set to the
        '               'fldPlanClass' column
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim objPlanClass As New ListDb.CPlanClassDB
        Dim rstSQL As New ADODB.Recordset





        rstSQL = objPlanClass.Fetch(blnIncludeDisabled, strWhere, strOrderBy)
        Fetch = rstSQL

        'UPGRADE_NOTE: Object objPlanClass may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objPlanClass = Nothing


    End Function

    Public Function Insert(ByVal strDescription As String) As Integer
        '--------------------------------------------------------------------
        'Date: 04/26/2000
        'Author: Eric Pena
        'Description:  Inserts a row into the tblPlanClassification table utilizing
        '              a stored procedure.
        'Parameters: strDescription - The description of the Plan Classification
        '              that will be inserted into the table.
        'Returns: ID (Primary Key) of the row inserted
        '--------------------------------------------------------------------


        Dim objPlanClass As New ListDb.CPlanClassDB
        Dim lngID As Integer
        Dim strErrMessage As String





        'Verify data before proceeding.
        If Not VerifyData(0, strDescription, strErrMessage) Then
            Exit Function
        End If

        lngID = objPlanClass.Insert(strDescription)

        'UPGRADE_NOTE: Object objPlanClass may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objPlanClass = Nothing

 
        Insert = lngID



    End Function

    Public Sub Update(ByVal lngID As Integer, ByVal strDescription As String)
        '--------------------------------------------------------------------
        'Date: 04/26/2000
        'Author: Eric Pena
        'Description:  Updates a row into the tblPlanClassification table.
        'Parameters:  lngID - ID of the row in the table whose value will be
        '               updated.
        '             strDescription - The Plan Classification description
        '                to which the record will be changed.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim objPlanClass As New ListDb.CPlanClassDB
        Dim strErrMessage As String





        'Verify data before proceeding.
        If Not VerifyData(lngID, strDescription, strErrMessage) Then
            Exit Sub
        End If

        objPlanClass.Update(lngID, strDescription)

        'UPGRADE_NOTE: Object objPlanClass may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objPlanClass = Nothing



    End Sub


    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Integer)
        '--------------------------------------------------------------------
        'Date: 04/26/2000
        'Author: Eric Pena
        'Description:  Flags a row in the tblPlanClassification table marking the row as
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


        Dim objPlanClass As New ListDb.CPlanClassDB





        objPlanClass.Deleted(blnDeleted, lngID)


        'UPGRADE_NOTE: Object objPlanClass may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objPlanClass = Nothing


    End Sub

    Public Function Exists(ByVal strDescription As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 04/26/2000
        'Author: Eric Pena
        'Description:  Determines if a Plan Classification description identical
        '              to the strDescription parameter already exists in the table.
        'Parameters: strDescription - Plan Classification record to be checked
        'Returns: True if the name exists, false otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim objPlanClass As New ListDb.CPlanClassDB
        Dim blnExists As Boolean





        blnExists = objPlanClass.Exists(strDescription)
        Exists = blnExists


        'UPGRADE_NOTE: Object objPlanClass may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objPlanClass = Nothing

    End Function

    Private Function VerifyData(ByVal lngID As Integer, ByVal strDescription As String, ByRef strErrMessage As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 04/26/2000
        'Author: Eric Pena
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
            strErrMessage = "Plan Classification '" & strDescription & "' already exists."
            VerifyData = False
            Exit Function
        End If

        'If we get here, all is well...
        VerifyData = True

    End Function
End Class