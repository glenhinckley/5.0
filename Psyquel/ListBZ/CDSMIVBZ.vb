Option Strict Off
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CDSM_IVBZ
    '--------------------------------------------------------------------
    'Class Name: CDSM_IVBz                                              '
    'Date: 02/24/2000                                                   '
    'Author: Rick "Boom Boom" Segura                                    '
    'Description:  MTS business object designed to  methods         '
    '              associated with the CDSMIVDB class.
    '--------------------------------------------------------------------
    ' Revision History:
    '  001 03/10/2003 Richkun: Added Update(), Disable() methods
    '--------------------------------------------------------------------
    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CDSM_IVBz"


    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False, Optional ByVal strWhere As String = "", Optional ByVal strOrderBy As String = "", Optional ByVal lngProviderID As Integer = 0) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/24/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Retrieves records from the tblDSMIV table.           '
        'Parameters:                                                        '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim objDSM_IV As New ListDb.CDSM_IVDB
        Dim rstSQL As New ADODB.Recordset



        rstSQL = objDSM_IV.Fetch(blnIncludeDisabled, strWhere, strOrderBy, lngProviderID)
        Fetch = rstSQL



        'UPGRADE_NOTE: Object objDSM_IV may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objDSM_IV = Nothing



    End Function
    Public Function FetchByID(ByVal lngDSM_IVID As Object) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/25/2000                                                   '
        'Author: Dave Nichol                                                '
        'Description:  Retrieves a record from the tblDSMIV table by        '
        '               primary key. Returns code, description, etc.        '
        'Parameters:   lngDSM_IVID - the ID of the row                      '
        'Returns:      recordset                                            '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------


        Dim objDSM_IV As New ListDb.CDSM_IVDB
        Dim rstSQL As New ADODB.Recordset



        'UPGRADE_WARNING: Couldn't resolve default property of object lngDSM_IVID. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        rstSQL = objDSM_IV.FetchByID(lngDSM_IVID)
        FetchByID = rstSQL



        'UPGRADE_NOTE: Object objDSM_IV may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objDSM_IV = Nothing



    End Function


    Public Function Insert(ByVal strICD9Code As String, ByVal strICD10Code As String, ByVal strDescription As String, ByVal blnColumnHeader As Boolean, ByVal blnAxisI As Boolean, ByVal blnAxisII As Boolean) As Integer
        '--------------------------------------------------------------------
        'Date: 03/14/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Inserts a row into the tblDSM_IV table utilizing     '
        '              a stored procedure.                                  '
        'Parameters: strDescription - The description of the DSM-IV code    '
        '              that will be inserted into the table.                '
        'Returns: ID (Primary Key) of the row inserted                      '
        '--------------------------------------------------------------------

        Dim objDSM_IV As New ListDb.CDSM_IVDB
        Dim lngID As Integer
        Dim strErrMessage As String

        lngID = objDSM_IV.Insert(strICD9Code, strICD10Code, strDescription, blnColumnHeader, blnAxisI, blnAxisII)
        'UPGRADE_NOTE: Object objDSM_IV may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objDSM_IV = Nothing


        Insert = lngID



    End Function

    Public Function Exists(ByVal strCode As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 05/23/2000
        'Author: Eric Pena
        'Description:  Determines if a DSMIV Code identical
        '              to the strCode parameter already exists in the table.
        'Parameters: strCode - DSMIV record to be checked
        'Returns: True if the code exists, false otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim objCPT As New ListDb.CDSM_IVDB
        Dim blnExists As Boolean




        blnExists = objCPT.Exists(strCode)
        Exists = blnExists



        'UPGRADE_NOTE: Object objCPT may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objCPT = Nothing

    End Function


    Public Function GetID(ByVal strCode As String) As Integer
        '--------------------------------------------------------------------
        'Date: 08/08/2000
        'Author: Dave Richkun
        'Description:  Returns the primary key for a specific DSM-IV code.
        'Parameters: strCode - DSM-IV code whose PK will be retrieved
        'Returns: ID (Primary Key) of matching DSM-IV record if found, -1 if
        '         not found.
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim objDSM As New ListDb.CDSM_IVDB





        GetID = objDSM.GetID(strCode)


        'UPGRADE_NOTE: Object objDSM may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objDSM = Nothing



    End Function

    Public Function GetVerificationData(ByVal strCode As String) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/21/2000
        'Author: Dave Nichol
        'Description:  Returns a recordset for a specific DSM-IV code, including
        '               ID of DSM-IV row, axis I/II flags.
        'Parameters: strCode - DSM-IV code whose PK will be retrieved
        'Returns: Recordset.
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objDSM As New ListDb.CDSM_IVDB





        strCode = Trim(strCode)

        GetVerificationData = objDSM.GetVerificationData(strCode)


        'UPGRADE_NOTE: Object objDSM may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objDSM = Nothing



    End Function

    Public Function FetchAllDSMIV(ByVal lngPatientID As Integer) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 11/20/2000                                                   '
        'Author: Chris Dereadt                                              '
        'Description:  Returns all axes of DSM-IV code for a patient.       '
        'Parameters: lngPatientID - ID of patient whose record you are      '
        '            trying to return.                                      '
        'Returns: ID all axes of a patients DSM-IV record                   '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------


        Dim objDSMIV As New ListDb.CDSM_IVDB
        Dim rstSQL As New ADODB.Recordset


        rstSQL = objDSMIV.FetchAllDSMIV(lngPatientID)
        FetchAllDSMIV = rstSQL



        'UPGRADE_NOTE: Object objDSMIV may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objDSMIV = Nothing
        'UPGRADE_NOTE: Object rstSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        rstSQL = Nothing


    End Function


    Public Sub Update(ByVal lngDSMIV_ID As Integer, ByVal strICD9Code As String, ByVal strICD10Code As String, ByVal strDescription As String, ByVal blnColumnHeader As Boolean, ByVal blnAxisI As Boolean, ByVal blnAxisII As Boolean)
        '--------------------------------------------------------------------
        'Date: 03/07/2003
        'Author: Dave Richkun
        'Description:  Updates a row in tblDSM_IV utilizing a stored procedure                                            '
        'Parameters: lngDSMIV_ID - ID of diagnosis code to update
        '            strDSMCode - DSM_IV Code
        '            strDescription - Description of the diagnosis code
        '            blnAxisI - Identifies if diagnosis resides on Axis I
        '            blnAxisII - Identifies if diagnosis resides on Axis II
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim objDSM As New ListDb.CDSM_IVDB



        objDSM.Update(lngDSMIV_ID, strICD9Code, strICD10Code, strDescription, blnColumnHeader, blnAxisI, blnAxisII)
        'UPGRADE_NOTE: Object objDSM may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objDSM = Nothing



    End Sub


    Public Sub Disable(ByVal lngDSMIV_ID As Integer, ByVal blnDisabledYN As Boolean)
        '--------------------------------------------------------------------
        'Date: 03/07/2003
        'Author: Dave Richkun
        'Description:  Disabled/re-enables a row in tblDSM_IV utilizing a stored procedure                                            '
        'Parameters: lngDSMIV_ID - ID of diagnosis code to update
        '            blnDisabledYN - Identifies if row is disabled (true) or re-enabled(false)
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim objDSM As New ListDb.CDSM_IVDB

        objDSM.Disable(lngDSMIV_ID, blnDisabledYN)
        'UPGRADE_NOTE: Object objDSM may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objDSM = Nothing




    End Sub
End Class