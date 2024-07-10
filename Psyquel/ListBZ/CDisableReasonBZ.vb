Option Strict Off
Option Explicit On
Imports ADODB
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CDisableReasonBZ
    '--------------------------------------------------------------------
    'Class Name: CDisableReasonBz                                       '
    'Date: 03/17/2022                                                   '
    'Author: Duane C Orth                                               '
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
    Private Const CLASS_NAME As String = "CDisableReasonBz"

    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/17/2022                                                   '
        'Author: Duane C Orth                                               '
        'Description:  Retrieves records from the tblDisableReason table.   '
        'Parameters:                                                        '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim obj As New ListDb.CDisableReasonDB
        Dim rstSQL As New ADODB.Recordset




        rstSQL = obj.Fetch(blnIncludeDisabled)
        Fetch = rstSQL

        'UPGRADE_NOTE: Object obj may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        obj = Nothing

    End Function
    Public Function FetchByID(ByVal lngDisableReasonID As Object) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/25/2000                                                   '
        'Author: Dave Nichol                                                '
        'Description:  Retrieves a record from the tblDisableReason table by '
        '               primary key. Returns code, description, etc.        '
        'Parameters:   lngDisableReasonID - the ID of the row               '
        'Returns:      recordset                                            '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim obj As New ListDb.CDisableReasonDB
        Dim rstSQL As New ADODB.Recordset




        'UPGRADE_WARNING: Couldn't resolve default property of object lngDisableReasonID. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        rstSQL = obj.FetchByID(lngDisableReasonID)
        FetchByID = rstSQL


        'UPGRADE_NOTE: Object obj may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        obj = Nothing


    End Function

    Public Function Insert(ByVal strDescription As String, ByVal strType As String) As Integer
        '--------------------------------------------------------------------
        'Date: 03/14/2000                                                   '
        'Author: Duane C Orth                                               '
        'Description:  Inserts a row into the tblDisableReason table utilizing '
        '              a stored procedure.                                  '
        'Parameters: strDescription - The description of the DSM-IV code    '
        '              that will be inserted into the table.                '
        'Returns: ID (Primary Key) of the row inserted                      '
        '--------------------------------------------------------------------

        Dim obj As New ListDb.CDisableReasonDB
        Dim lngID As Integer
        Dim strErrMessage As String


        lngID = obj.Insert(strDescription, strType)
        'UPGRADE_NOTE: Object obj may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        obj = Nothing


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

        Dim objCPT As New ListDb.CDisableReasonDB
        Dim blnExists As Boolean




        blnExists = objCPT.Exists(strCode)
        Exists = blnExists

        'UPGRADE_NOTE: Object objCPT may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objCPT = Nothing

    End Function

    Public Sub Update(ByVal lngID As Integer, ByVal strDescription As String, ByVal strType As String)
        '--------------------------------------------------------------------
        'Date: 03/17/2022
        'Author: Duane C Orth
        'Description:  Updates a row in tblDisableReason utilizing a stored procedure                                            '
        'Parameters: lngID - ID of diagnosis code to update
        '            strDescription - Description of the diagnosis code
        '            strType - Identifies
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim obj As New ListDb.CDisableReasonDB



        obj.Update(lngID, strDescription, strType)
        'UPGRADE_NOTE: Object obj may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        obj = Nothing

 

    End Sub

    Public Sub Disable(ByVal lngID As Integer, ByVal blnDisabledYN As Boolean, ByVal strDisabledBy As String)
        '--------------------------------------------------------------------
        'Date: 03/17/2022
        'Author: Duane C Orth
        'Description:  Disabled/re-enables a row in tblDisableReason utilizing a stored procedure   '
        'Parameters: lngID - ID of diagnosis code to update
        '            blnDisabledYN - Identifies if row is disabled (true) or re-enabled(false)
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim obj As New ListDb.CDisableReasonDB



        obj.Disable(lngID, blnDisabledYN, strDisabledBy)
        'UPGRADE_NOTE: Object obj may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        obj = Nothing



    End Sub
End Class