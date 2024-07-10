Option Strict Off
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CCPTCategoryBz
    '--------------------------------------------------------------------
    'Class Name: CCPTCategoryBz                                             '
    'Date: 10/23/2018                                                   '
    'Author: Rick "Boom Boom" Segura                                    '
    'Description:  MTS business object designed to  methods         '
    '              associated with the CCPTCategoryDB class.                '
    '--------------------------------------------------------------------
    '  R001: 03/10/2003 Richkun: Added Update(), Disable() methods
    '--------------------------------------------------------------------

    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CCPTCategoryBz"


    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False, Optional ByVal strWhere As String = "", Optional ByVal strOrderBy As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/23/2018                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Retrieves records from the tblCPTCode table.         '
        'Parameters:                                                        '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim objCPTCategoryCategory As New ListDb.CCPTCategoryDB
        Dim rstSQL As New ADODB.Recordset

        '

        'objCPTCategoryCategory = CreateObjectXXXXXXX("ListDB.CCPTCategoryDB")

        rstSQL = objCPTCategoryCategory.Fetch(blnIncludeDisabled, strWhere, strOrderBy)
        Fetch = rstSQL

        'Signal successful completion
        '	System.EnterpriseServicesxxxxxxxXXXXXX.ContextUtil.SetComplete()

        'UPGRADE_NOTE: Object objCPTCategoryCategory may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objCPTCategoryCategory = Nothing



    End Function

    Public Function Exists(ByVal strCode As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 10/23/2018
        'Author: Duane C Orth
        'Description:  Determines if a CPT Code identical
        '              to the strCode parameter already exists in the table.
        'Parameters: strCode - CPT record to be checked
        'Returns: True if the code exists, false otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim objCPTCategory As New ListDb.CCPTCategoryDB
        Dim blnExists As Boolean

        '	


        blnExists = objCPTCategory.Exists(strCode)
        Exists = blnExists

        'Signal successful completion
        '	System.EnterpriseServicesxxxxxxxXXXXXX.ContextUtil.SetComplete()

        'UPGRADE_NOTE: Object objCPTCategory may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objCPTCategory = Nothing


    End Function


    Public Function Insert(ByVal strDescription As String) As String
        '--------------------------------------------------------------------
        'Date: 10/23/2018
        'Author: Duane C Orth
        'Description:  Inserts a row into tblCPTCode table utilizing a stored procedure.
        'Parameters:  'strCPTCategory - CPT Code to be entered
        '             strDescription - The description of the procedure
        'Returns: CPTCode on success; 0 on failure
        '--------------------------------------------------------------------

        Dim objCPTCategory As New ListDb.CCPTCategoryDB
        Dim lngID As Integer
        Dim strErrMessage As String

        '	

        'Verify data before proceeding.
        If Not VerifyData(strDescription, strErrMessage) Then
            Exit Function

        End If

        lngID = objCPTCategory.Insert(strDescription)
        'UPGRADE_NOTE: Object objCPTCategory may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objCPTCategory = Nothing



        Insert = CStr(lngID)



    End Function


    Public Sub Update(ByVal lngID As Integer, ByVal strDescription As String)
        '--------------------------------------------------------------------
        'Date: 10/23/2018
        'Author: Duane C Orth
        'Description:  Updates a row in tblCPTCode table utilizing a stored procedure.
        'Parameters:  strCPTCategory - CPT Code to be entered (CPTCode is the table's Primary Key)
        '             strDescription - The description of the procedure
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objCPTCategory As New ListDb.CCPTCategoryDB
        Dim strErrMessage As String

        '  

        'Verify data before proceeding.
        If Not VerifyData(strDescription, strErrMessage) Then
            Exit Sub

        End If

        '	objCPTCategory = CreateObjectXXXXXXX("ListDB.CCPTCategoryDB")
        objCPTCategory.Update(lngID, strDescription)
        'UPGRADE_NOTE: Object objCPTCategory may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objCPTCategory = Nothing

        'Signal successful completion
        'System.EnterpriseServicesxxxxxxxXXXXXX.ContextUtil.SetComplete()


    End Sub


    Public Sub Disable(ByVal strCPTCategory As String, ByVal blnDisabledYN As Boolean)
        '--------------------------------------------------------------------
        'Date: 10/23/2018
        'Author: Duane C Orth
        'Description:  Disables or re-enables a row in tblCPTCode table utilizing a stored procedure.
        'Parameters:  strCPTCategory - CPT Code to be disabled/re-enabled
        '             blnDisabledYN - Boolean identifying of row is to be disabled (true) or re-enabled (false)
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objCPTCategory As New ListDb.CCPTCategoryDB
        Dim lngID As Integer
        Dim strErrMessage As String



        '	objCPTCategory = CreateObjectXXXXXXX("ListDB.CCPTCategoryDB")
        objCPTCategory.Disable(CInt(strCPTCategory), blnDisabledYN)
        'UPGRADE_NOTE: Object objCPTCategory may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objCPTCategory = Nothing

        'Signal successful completion




    End Sub


    '-----------------------
    ' Private Methods
    '-----------------------
    Private Function VerifyData(ByVal strDescription As String, ByRef strErrMessage As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 10/23/2018
        'Author: Duane C Orth
        'Description:  'Verifies data is complete
        'Parameters:  strCPTCategory - CPT Code to be checked
        '             strDescription - Description to be checked
        '             strErrMessage - Error message to be returned to ing procedure
        'Returns: True if all data is provided, false otherwise.
        '--------------------------------------------------------------------

        If Trim(strDescription) = "" Then
            strErrMessage = "Category Description is invalid"
            VerifyData = False
        End If

        'If we get here, all is well
        VerifyData = True

    End Function
End Class