
'--------------------------------------------------------------------
'Class Name: CClinicBz
'Date: 12/05/1999
'Author: Dave Richkun
'Description:  MTS business object designed to   methods associated
'              with the CClinicDB class.
'--------------------------------------------------------------------

Option Explicit On


Imports ADODB.CommandTypeEnum
Imports ADODB.CompareEnum
Imports ADODB.ParameterAttributesEnum



Imports ADODB.DataTypeEnum
Imports ADODB.ParameterDirectionEnum
Imports ADODB.ExecuteOptionEnum

Imports ADODB.CursorTypeEnum
Imports ADODB.CursorLocationEnum
Imports ADODB.CursorOptionEnum
Imports ADODB.LockTypeEnum
Imports Psyquel.BusinessRules.CoreLibraryIII.dbStuff
' Lydia Orth 


Public Class CClinicBz



    Private _ConnectionString As String = String.Empty

    Private Const CLASS_NAME As String = "CClinicBz"
    Private Const TABLE_NAME As String = "tblClinic"


    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property



    Public Function FetchDetail(ByVal lngID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/06/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Retrieves records from the tblClinic table.          '
        'Parameters: blnIncludeDisabled - Optional parameter that identifies'
        '               if records flagged as 'Disabled' or 'De-activated'  '
        '               are to be included in the record set. The default   '
        '               value is False.                                     '
        'Returns: Recordset of clinics ordered by business name             '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim objClinic As New ClinicDB.CClinicDB
        Dim rst As New ADODB.Recordset



        rst = objClinic.FetchDetail(lngID, blnIncludeDisabled)
        FetchDetail = rst



        objClinic = Nothing



    End Function

    Public Function FetchByGroup(ByVal lngGroupID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/03/2015                                                   '
        'Author: Duane C Orth                                               '
        'Description:  Retrieves records from the tblClinic table.          '
        'Parameters: blnIncludeDisabled - Optional parameter that identifies'
        '               if records flagged as 'Disabled' or 'De-activated'  '
        '               are to be included in the record set. The default   '
        '               value is False.                                     '
        'Returns: Recordset of clinics ordered by Group                     '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim objClinic As New ClinicDB.CClinicDB
        Dim rstSQL As New ADODB.Recordset



        rstSQL = objClinic.FetchByGroup(lngGroupID, blnIncludeDisabled)
        FetchByGroup = rstSQL



        objClinic = Nothing



    End Function

    Public Function FetchByName(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/02/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Retrieves records from the tblClinic table.          '
        'Parameters: blnIncludeDisabled - Optional parameter that identifies'
        '               if records flagged as 'Disabled' or 'De-activated'  '
        '               are to be included in the record set. The default   '
        '               value is False.                                     '
        'Returns: Recordset of clinics ordered by business name             '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim objClinic As New ClinicDB.CClinicDB
        Dim rstSQL As New ADODB.Recordset



        rstSQL = objClinic.FetchByName(blnIncludeDisabled)
        FetchByName = rstSQL



        objClinic = Nothing



    End Function


    Public Function FetchByZip(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/02/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Retrieves records from the tblClinic table.          '
        'Parameters: blnIncludeDisabled - Optional parameter that identifies'
        '               if records flagged as 'Disabled' or 'De-activated'  '
        '               are to be included in the record set. The default   '
        '               value is False.                                     '
        'Returns: Recordset of clinics ordered by zip code                  '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim objClinic As New ClinicDB.CClinicDB
        Dim rstSQL As New ADODB.Recordset

  

        rstSQL = objClinic.FetchByZip(blnIncludeDisabled)
        FetchByZip = rstSQL

        objClinic = Nothing


    End Function

    Public Function FetchByCity(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/02/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Retrieves records from the tblClinic table.          '
        'Parameters: blnIncludeDisabled - Optional parameter that identifies'
        '               if records flagged as 'Disabled' or 'De-activated'  '
        '               are to be included in the record set. The default   '
        '               value is False.                                     '
        'Returns: Recordset of clinics ordered by city                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim objClinic As New ClinicDB.CClinicDB
        Dim rstSQL As ADODB.Recordset



        rstSQL = objClinic.FetchByCity(blnIncludeDisabled)
        FetchByCity = rstSQL



        objClinic = Nothing


    End Function

    Public Function FetchByState(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/02/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Retrieves records from the tblClinic table.          '
        'Parameters: blnIncludeDisabled - Optional parameter that identifies'
        '               if records flagged as 'Disabled' or 'De-activated'  '
        '               are to be included in the record set. The default   '
        '               value is False.                                     '
        'Returns: Recordset of clinics ordered by State                     '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim objClinic As New ClinicDB.CClinicDB
        Dim rstSQL As New ADODB.Recordset



        rstSQL = objClinic.FetchByState(blnIncludeDisabled)
        FetchByState = rstSQL



        objClinic = Nothing



    End Function

    Public Function Insert(ByVal strBusinessName As String, ByVal strAddress1 As String, _
                           ByVal strAddress2 As String, ByVal strCity As String, _
                           ByVal strState As String, ByVal strZip As String, _
                           ByVal strPhone As String, ByVal strFax As String, _
                           ByVal strSubmitterID As String, ByVal strPOSCode As String, _
                           ByVal strNPI As String, ByVal strTaxonomy As String, _
                           ByVal strAddedBy As String, ByVal strClinicType As String, _
                           ByVal strClaimType As String, ByVal blnShortageAreaYN As Boolean) As Long
        '--------------------------------------------------------------------
        'Date: 12/05/1999
        'Author: Dave Richkun
        'Description:  Inserts a row into the tblClinic table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------

        Dim objClinic As New ClinicDB.CClinicDB
        Dim lngID As Long
        Dim strErrMessage As String



        If Not VerifyData(lngID, strBusinessName, strAddress1, strCity, strState, _
                          strClinicType, strErrMessage) Then
            Exit Function
        End If

        lngID = objClinic.Insert(strBusinessName, strAddress1, strAddress2, strCity, _
                                 strState, strZip, strPhone, strFax, strSubmitterID, strPOSCode, _
                                 strNPI, strTaxonomy, strAddedBy, strClinicType, strClaimType, blnShortageAreaYN)

        Insert = lngID



        objClinic = Nothing


    End Function

    Public Sub Update(ByVal lngID As Long, ByVal strBusinessName As String, _
                      ByVal strAddress1 As String, ByVal strAddress2 As String, _
                      ByVal strCity As String, ByVal strState As String, _
                      ByVal strZip As String, ByVal strPhone As String, _
                      ByVal strFax As String, ByVal strSubmitterID As String, _
                      ByVal strPOSCode As String, ByVal strNPI As String, _
                      ByVal strTaxonomy As String, ByVal strClinicType As String, _
                      ByVal strClaimType As String, ByVal blnShortageAreaYN As Boolean)
        '--------------------------------------------------------------------
        'Date: 12/27/1999
        'Author: Dave Richkun
        'Description:  Updates a row in the tblClinic table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objClinic As New ClinicDB.CClinicDB
        Dim strErrMessage As String


        If Not VerifyData(lngID, strBusinessName, strAddress1, strCity, _
                          strState, strClinicType, strErrMessage) Then
            Exit Sub
        End If


        objClinic.Update(lngID, strBusinessName, strAddress1, strAddress2, _
                      strCity, strState, strZip, strPhone, strFax, strSubmitterID, _
                      strPOSCode, strNPI, strTaxonomy, strClinicType, strClaimType, blnShortageAreaYN)



        objClinic = Nothing


    End Sub


    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Long, _
                       ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 12/30/1999
        'Author: Dave Richkun
        'Description:  Flags a row in the tblClinic table marking the row as
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

        Dim objClinic As New ClinicDB.CClinicDB


        objClinic.Deleted(blnDeleted, lngID, strUserName)



        objClinic = Nothing



    End Sub


    Private Function VerifyData(ByVal lngID As Long, ByVal strBusinessName As String, _
                                ByVal strAddress1 As String, ByVal strCity As String, _
                                ByVal strState As String, ByVal strClinicType As String, _
                                ByRef strErrMessage As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 01/04/2000
        'Author: Dave Richkun
        'Description:  Verifies all required data has been provided by the user.
        'Parameters:  The values to be checked.
        'Returns: Boolean value identifying if all criteria has been satisfied.
        '--------------------------------------------------------------------

        If Trim(strBusinessName) = "" Then
            strErrMessage = "Business Name is required."
            VerifyData = False
            Exit Function
        End If

        If Trim(strAddress1) = "" Then
            strErrMessage = "Street address is required."
            VerifyData = False
            Exit Function
        End If

        If Trim(strCity) = "" Then
            strErrMessage = "City is required."
            VerifyData = False
            Exit Function
        End If

        If Trim(strState) = "" Then
            strErrMessage = "State is required."
            VerifyData = False
            Exit Function
        End If

        If Trim(strClinicType) = "" Then
            strErrMessage = "Clinic Type is required."
            VerifyData = False
            Exit Function
        End If

        'Check for existance only when inserting new data
        If lngID = 0 And Exists(strBusinessName) Then
            strErrMessage = "Business Name '" & strBusinessName & "' already exists."
            VerifyData = False
            Exit Function
        End If

        'If we get here, all is well...
        VerifyData = True

    End Function


    Public Function Exists(ByVal strBusinessName As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 01/05/2000
        'Author: Dave Richkun
        'Description:  Determines if the BusinessName identical to the
        '              strBusinessName parameter already exists in the table.
        'Parameters: strBusinessName - Business name to be checked
        'Returns: True if the name exists, false otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objClinic As New ClinicDB.CClinicDB
        Dim blnExists As Boolean



        blnExists = objClinic.Exists(strBusinessName)

        Exists = blnExists

        objClinic = Nothing



    End Function


End Class
