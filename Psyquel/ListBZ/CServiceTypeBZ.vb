Option Strict Off
Option Explicit On

Imports ADODB
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CServiceTypeBZ
    '--------------------------------------------------------------------
    'Class Name: CServiceTypeBz                                              '
    'Date: 09/14/2004                                                   '
    'Author: Duane C Orth                                               '
    'Description:  MTS business object designed to  methods         '
    '              associated with the CServiceTypeDB class.                 '
    '--------------------------------------------------------------------
    'Revision History:
    '--------------------------------------------------------------------
    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CServiceTypeBz"

    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/14/2004                                                   '
        'Author: Duane C Orth                                               '
        'Description:  Retrieves records from the tblServiceType table.          '
        'Parameters: blnIncludeDisabled - Optional parameter that identifies'
        '              if records flagged as 'Disabled' or 'De-activated'   '
        '              are to be included in the record set. The default    '
        '              value is False.                                      '
        'Returns: Recordset of Services                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '--------------------------------------------------------------------

        Dim objService As New ListDb.CServiceTypeDB
        Dim rstSQL As New ADODB.Recordset

        '		

        '		objService = CreateObjectXXXXXXX("ListDB.CServiceTypeDB")
        rstSQL = objService.Fetch(blnIncludeDisabled)

        Fetch = rstSQL

        'UPGRADE_NOTE: Object objService may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objService = Nothing

        'Signal successful completion
        '		System.EnterpriseServicesxxxxxxxXXXXXX.ContextUtil.SetComplete()

        Exit Function



    End Function


    Public Function FetchByID(ByVal lngServiceTypeID As Integer) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/12/2001
        'Author: Duane C Orth
        'Description:  Retrieves all Service records associated with a ServiceID
        'Parameters: lngServiceID: ID of Service Group to fetch Service for
        'Returns: Recordset of requested Service
        '--------------------------------------------------------------------
        'Revision History:
        '--------------------------------------------------------------------

        Dim objService As New ListDb.CServiceTypeDB
        Dim rstSQL As New ADODB.Recordset

        'O'n Errorexit

        '		objService = CreateObjectXXXXXXX("ListDB.CServiceTypeDB")
        rstSQL = objService.FetchByID(lngServiceTypeID)

        FetchByID = rstSQL

        'UPGRADE_NOTE: Object objService may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objService = Nothing

        'Signal successful completion
        '		System.EnterpriseServicesxxxxxxxXXXXXX.ContextUtil.SetComplete()



    End Function

    Public Function FetchByCode(ByVal strServiceTypeCode As String) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/12/2001
        'Author: Duane C Orth
        'Description:  Retrieves all Service records associated with a ServiceID
        'Parameters: lngServiceID: ID of Service Group to fetch Service for
        'Returns: Recordset of requested Service
        '--------------------------------------------------------------------
        'Revision History:
        '--------------------------------------------------------------------

        Dim objService As New ListDb.CServiceTypeDB
        Dim rstSQL As New ADODB.Recordset

        '

        rstSQL = objService.FetchByCode(strServiceTypeCode)

        FetchByCode = rstSQL

        'UPGRADE_NOTE: Object objService may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objService = Nothing

        'Signal successful completion
        ''   





    End Function

    Public Function Insert(ByVal strServiceTypeCode As String, ByVal strDescription As String, ByVal blnUse271YN As Boolean, ByVal strUserName As String) As Integer
        '--------------------------------------------------------------------
        'Date: 09/14/2004
        'Author: Duane C Orth
        'Description:  Inserts a row into the tblServiceType table.
        'Parameters: strDescription - The description of the ServiceType
        '              that will be inserted into the table.
        'Returns: ID (Primary Key) of the row inserted
        '--------------------------------------------------------------------


        Dim objService As New ListDb.CServiceTypeDB
        Dim lngID As Integer
        Dim strErrMessage As String

        ' 

        'Verify data before proceeding.
        If Not VerifyData(0, strServiceTypeCode, strDescription, strErrMessage) Then
            Exit Function

        End If

        Insert = objService.Insert(strServiceTypeCode, strDescription, blnUse271YN, strUserName)

        'Signal successful completion


        'Release resources
        'UPGRADE_NOTE: Object objService may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objService = Nothing



    End Function


    Public Sub Update(ByVal lngID As Integer, ByVal strServiceTypeCode As String, ByVal strDescription As String, ByVal blnUse271YN As Boolean)
        '--------------------------------------------------------------------
        'Date: 09/14/2004
        'Author: Duane C Orth
        'Description:  Updates a row into the tblServiceType table.
        'Parameters:  lngID - ID of the row in the table whose value will be
        '               updated.
        '             strDescription - The ServiceType status description
        '                to which the record will be changed.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim objService As New ListDb.CServiceTypeDB
        Dim strErrMessage As String



        'Verify data before proceeding.
        If Not VerifyData(lngID, strServiceTypeCode, strDescription, strErrMessage) Then
            Exit Sub

        End If

        '    objService = CreateObjectXXXXXXX("ListDB.CServiceTypeDB")
        objService.Update(lngID, strServiceTypeCode, strDescription, blnUse271YN)

        'UPGRADE_NOTE: Object objService may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objService = Nothing
        'Signal successful completion



    End Sub

    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Integer, ByVal strDeletedBy As String)
        '--------------------------------------------------------------------
        'Date: 09/14/2004
        'Author: Duane C Orth
        'Description:  Flags a row in the tblServicePeriod table marking the row as
        '              deleted or undeleted.
        'Parameters: blnDeleted - Boolean value identifying if the record is to
        '               be deleted (True) or undeleted (False).
        '            lngID - ID of the row in the table whose value will be
        '               updated.
        '            strUserName - Login name of the user responsible for
        '               marking the row as deleted.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim objService As New ListDb.CServiceTypeDB

        ' 

        'objService = CreateObjectXXXXXXX("ListDB.CServiceTypeDB")

        objService.Deleted(blnDeleted, lngID, strDeletedBy)
        'UPGRADE_NOTE: Object objService may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objService = Nothing


    End Sub


    Public Function Exists(ByVal strServiceTypeCode As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 01/05/2000
        'Author: Duane C Orth
        'Description:  Determines if a ServiceType description identical to the
        '              strDescription parameter already exists in the table.
        'Parameters: strDescription - Service name to be checked
        'Returns: True if the name exists, false otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim objService As New ListDb.CServiceTypeDB
        Dim blnExists As Boolean

        '   

        '    objService = CreateObjectXXXXXXX("ListDB.CServiceTypeDB")

        blnExists = objService.Exists(strServiceTypeCode)
        Exists = blnExists

        'UPGRADE_NOTE: Object objService may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objService = Nothing

    End Function


    Private Function VerifyData(ByVal lngID As Integer, ByVal strServiceTypeCode As String, ByVal strDescription As String, ByRef strErrMessage As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 01/04/2000
        'Author: Duane C Orth
        'Description:  Verifies all required data has been provided by the user.
        'Parameters:  The values to be checked.
        'Returns: Boolean value identifying if all criteria has been satisfied.
        '--------------------------------------------------------------------

        If Trim(strDescription) = "" Then
            strErrMessage = "ServiceType Name is required."
            VerifyData = False
            Exit Function
        End If

        'Check for existance only when inserting new data
        If lngID = 0 And Exists(strServiceTypeCode) Then
            strErrMessage = "ServiceTypeCode '" & strServiceTypeCode & "' already exists."
            VerifyData = False
            Exit Function
        End If

        'If we get here, all is well...
        VerifyData = True

    End Function
End Class