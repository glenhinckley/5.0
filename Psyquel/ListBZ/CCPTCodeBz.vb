Option Strict Off
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CCPTCodeBz
    '--------------------------------------------------------------------
    'Class Name: CCPTCodeBz                                             '
    'Date: 02/24/2000                                                   '
    'Author: Rick "Boom Boom" Segura                                    '
    'Description:  MTS business object designed to  methods         '
    '              associated with the CCPTCodeDB class.                '
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
    Private Const CLASS_NAME As String = "CCPTCodeBz"


    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False, Optional ByVal strWhere As String = "", Optional ByVal strOrderBy As String = "", Optional ByVal lngProviderID As Integer = 0) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/24/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Retrieves records from the tblCPTCode table.         '
        'Parameters:                                                        '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim objCPTCode As New ListDb.CCPTCodeDB
        Dim rstSQL As New ADODB.Recordset

        '

        '		objCPTCode = CreateObjectXXXXXXX("ListDB.CCPTCodeDB")

        rstSQL = objCPTCode.Fetch(blnIncludeDisabled, strWhere, strOrderBy, lngProviderID)
        Fetch = rstSQL

        'Signal successful completion
        '		System.EnterpriseServicesxxxxxxxXXXXXX.ContextUtil.SetComplete()

        'UPGRADE_NOTE: Object objCPTCode may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objCPTCode = Nothing

        Exit Function



    End Function

    Public Function Exists(ByVal strCode As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 05/23/2000
        'Author: Eric Pena
        'Description:  Determines if a CPT Code identical
        '              to the strCode parameter already exists in the table.
        'Parameters: strCode - CPT record to be checked
        'Returns: True if the code exists, false otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim objCPT As New ListDb.CCPTCodeDB
        Dim blnExists As Boolean

        '

        '	objCPT = CreateObjectXXXXXXX("ListDB.CCPTCodeDB")

        blnExists = objCPT.Exists(strCode)
        Exists = blnExists

        'Signal successful completion
        '	System.EnterpriseServicesxxxxxxxXXXXXX.ContextUtil.SetComplete()

        'UPGRADE_NOTE: Object objCPT may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objCPT = Nothing


    End Function


    Public Function Insert(ByVal strCPTCode As String, ByVal strDescription As String, ByVal strRevenueCode As String, ByVal lngCategoryID As Integer, ByVal blnAddOn As Boolean, ByVal blnUseAddOn As Boolean, ByVal blnSecAddOn As Boolean, ByVal blnUseSecAddOn As Boolean, ByVal blnAllowMultiple As Boolean) As Integer
        '--------------------------------------------------------------------
        'Date: 03/07/2003
        'Author: Dave Richkun
        'Description:  Inserts a row into tblCPTCode table utilizing a stored procedure.
        'Parameters:  'strCPTCode - CPT Code to be entered
        '             strDescription - The description of the procedure
        'Returns: CPTCode on success; 0 on failure
        '--------------------------------------------------------------------

        Dim objCPT As New ListDb.CCPTCodeDB
        Dim lngID As Integer
        Dim strErrMessage As String

        '

        'Verify data before proceeding.
        If Not VerifyData(strCPTCode, strDescription, strErrMessage) Then
            Exit Function
        End If

        '	objCPT = CreateObjectXXXXXXX("ListDB.CCPTCodeDB")
        lngID = objCPT.Insert(strCPTCode, strDescription, strRevenueCode, lngCategoryID, blnAddOn, blnUseAddOn, blnSecAddOn, blnUseSecAddOn, blnAllowMultiple)
        'UPGRADE_NOTE: Object objCPT may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objCPT = Nothing

        'Signal successful completion
        ''System.EnterpriseServicesxxxxxxxXXXXXX.ContextUtil.SetComplete()

        Insert = lngID



    End Function


    Public Sub Update(ByVal lngID As Integer, ByVal strCPTCode As String, ByVal strDescription As String, ByVal strRevenueCode As String, ByVal lngCategoryID As Integer, ByVal blnAddOn As Boolean, ByVal blnUseAddOn As Boolean, ByVal blnSecAddOn As Boolean, ByVal blnUseSecAddOn As Boolean, ByVal blnAllowMultiple As Boolean)
        '--------------------------------------------------------------------
        'Date: 03/07/2003
        'Author: Dave Richkun
        'Description:  Updates a row in tblCPTCode table utilizing a stored procedure.
        'Parameters:  strCPTCode - CPT Code to be entered (CPTCode is the table's Primary Key)
        '             strDescription - The description of the procedure
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objCPT As New ListDb.CCPTCodeDB
        Dim strErrMessage As String

        '	

        'Verify data before proceeding.
        If Not VerifyData(strCPTCode, strDescription, strErrMessage) Then
            '		GoTo ErrTrap
            Exit Sub

        End If

        'objCPT = CreateObjectXXXXXXX("ListDB.CCPTCodeDB")
        objCPT.Update(lngID, strCPTCode, strDescription, strRevenueCode, lngCategoryID, blnAddOn, blnUseAddOn, blnSecAddOn, blnUseSecAddOn, blnAllowMultiple)
        'UPGRADE_NOTE: Object objCPT may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objCPT = Nothing



    End Sub


    Public Sub Disable(ByVal strCPTCode As String, ByVal blnDisabledYN As Boolean)
        '--------------------------------------------------------------------
        'Date: 03/07/2003
        'Author: Dave Richkun
        'Description:  Disables or re-enables a row in tblCPTCode table utilizing a stored procedure.
        'Parameters:  strCPTCode - CPT Code to be disabled/re-enabled
        '             blnDisabledYN - Boolean identifying of row is to be disabled (true) or re-enabled (false)
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objCPT As New ListDb.CCPTCodeDB
        Dim lngID As Integer
        Dim strErrMessage As String


        '	objCPT = CreateObjectXXXXXXX("ListDB.CCPTCodeDB")
        objCPT.Disable(CInt(strCPTCode), blnDisabledYN)
        'UPGRADE_NOTE: Object objCPT may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objCPT = Nothing

        'Signal successful completion




    End Sub


    '-----------------------
    ' Private Methods
    '-----------------------
    Private Function VerifyData(ByVal strCPTCode As String, ByVal strDescription As String, ByRef strErrMessage As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 03/07/2003
        'Author: Dave Richkun
        'Description:  'Verifies data is complete
        'Parameters:  strCPTCode - CPT Code to be checked
        '             strDescription - Description to be checked
        '             strErrMessage - Error message to be returned to ing procedure
        'Returns: True if all data is provided, false otherwise.
        '--------------------------------------------------------------------

        If Trim(strCPTCode) = "" Then
            strErrMessage = "CPT Code is invalid"
            VerifyData = False
        End If

        If Trim(strDescription) = "" Then
            strErrMessage = "Description is invalid"
            VerifyData = False
        End If

        'If we get here, all is well
        VerifyData = True

    End Function
End Class