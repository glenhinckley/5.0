
'--------------------------------------------------------------------
'Class Name: CCPCBz
'Date: 01/18/2000
'Author: Eric Pena
'Denis J Finkel
'Description:  MTS business object designed to   methods associated
'              with the CCPCDB class.
'--------------------------------------------------------------------
Option Explicit On


Imports ADODB.CommandTypeEnum
Imports ADODB.CompareEnum
Imports ADODB.ParameterAttributesEnum

Imports Psyquel.BusinessRules.CoreLibraryIII.dbStuff

Imports ADODB.DataTypeEnum
Imports ADODB.ParameterDirectionEnum
Imports ADODB.ExecuteOptionEnum

Imports ADODB.CursorTypeEnum
Imports ADODB.CursorLocationEnum
Imports ADODB.CursorOptionEnum
Imports ADODB.LockTypeEnum
Imports Psyquel.BusinessRules.CoreLibraryIII


Public Class CCPCBz

    Private Const CLASS_NAME As String = "CCPCBz"
    Private Const TABLE_NAME As String = "tblClaimsProcCtr"

    Private _ConnectionString As String = String.Empty
    Private _AppPath As String = String.Empty
    Private _EDIOutput As String = String.Empty
    Private _MachineName As String = String.Empty
    Private _UserLoginName As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property




    Dim md As New ModCommon
    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False, Optional ByVal strWhere As String = "", Optional ByVal strOrderBy As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/24/2000
        'Author: Eric Pena
        'Description:  Retrieves records from the tblClaimsProcCtr table.
        'Parameters: blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Disabled' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        '            strWhere - Optional 'Where' clause of the SQL statement by
        '               which records will be filtered.  If the parameter is not
        '               supplied, all records will be retrieved.
        '             strOrderBy - Optional 'Order By' clause in which retrieved
        '               records will be sorted.  The default is set to the
        '               'fldClinic' column
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim objCPC As New InsuranceDB.CCPCDB
        Dim rstSQL As New ADODB.Recordset


        rstSQL = objCPC.Fetch(blnIncludeDisabled, strWhere, strOrderBy)
        Fetch = rstSQL


        objCPC = Nothing

    End Function
    Public Function Insert(ByVal lngInsuranceID As Long, _
                        ByVal strName As String, _
                        ByVal strAddress1 As String, _
                        ByVal strAddress2 As String, _
                        ByVal strCity As String, _
                        ByVal strState As String, _
                        ByVal strZip As String, _
                        ByVal strPhone As String, _
                        ByVal strExt As String, _
                        ByVal strFax As String, _
                        ByVal strEmail As String, _
                        ByVal strContact As String, _
                        ByVal strNotes As String, _
                        ByVal lngClearingHouseID As Long, _
                        ByVal strPayerCode As String, _
                        ByVal strInsType As String, _
                        ByVal strUsername As String) As Long
        '--------------------------------------------------------------------
        'Date: 02/24/2000
        'Author: Eric Pena
        'Description:  Inserts a row into the tblClaimsProcCtr table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------

        Dim objCPC As New InsuranceDB.CCPCDB
        Dim strErrMessage As String


        If Not VerifyData(0, lngInsuranceID, strName, strAddress1, strCity, strState, strZip, strPhone, strErrMessage) Then Exit Function

        Insert = objCPC.Insert(lngInsuranceID, strName, strAddress1, strAddress2, strCity, strState, strZip, strPhone, strExt, strFax, strEmail, strContact, strNotes, lngClearingHouseID, strPayerCode, strInsType, strUsername)

        objCPC = Nothing

    End Function
    Public Sub Update(ByVal lngID As Long, _
                        ByVal lngInsuranceID As Long, _
                        ByVal strName As String, _
                        ByVal strAddress1 As String, _
                        ByVal strAddress2 As String, _
                        ByVal strCity As String, _
                        ByVal strState As String, _
                        ByVal strZip As String, _
                        ByVal strPhone As String, _
                        ByVal strExt As String, _
                        ByVal strFax As String, _
                        ByVal strEmail As String, _
                        ByVal strContact As String, _
                        ByVal strNotes As String, _
                        ByVal lngClearingHouseID As Long, _
                        ByVal strPayerCode As String, _
                        ByVal strInsType As String)
        '--------------------------------------------------------------------
        'Date: 02/24/2000
        'Author: Eric Pena
        'Description:  Updates a row in the tblClaimsProcCtr table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Null
        '--------------------------------------------------------------------


        Dim objCPC As New InsuranceDB.CCPCDB
        Dim strErrMessage As String





        If Not VerifyData(lngID, lngInsuranceID, strName, strAddress1, strCity, strState, strZip, strPhone, strErrMessage) Then Exit Sub

        objCPC.Update(lngID, lngInsuranceID, strName, strAddress1, strAddress2, strCity, strState, strZip, strPhone, strExt, strFax, strEmail, strContact, strNotes, lngClearingHouseID, strPayerCode, strInsType)

        objCPC = Nothing

    End Sub
    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Long, ByVal strDeletedBy As String)
        '--------------------------------------------------------------------
        'Date: 02/24/2000
        'Author: Eric Pena
        'Description:  Flags a row in the tblClaimsProcCtr table marking the row as
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


        Dim objCPC As New InsuranceDB.CCPCDB



        'TODO:  If cascading, check for active claims/rps with associated plans
        objCPC.Deleted(blnDeleted, lngID, strDeletedBy)



        objCPC = Nothing



    End Sub
    Private Function VerifyData(ByVal lngID As Long, ByVal lngInsuranceID As Long, ByVal strName As String, ByVal strAddress1 As String, ByVal strCity As String, ByVal strState As String, _
                                                ByRef strZip As String, ByRef strPhone As String, ByRef strErrMessage As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 02/24/2000
        'Author: Eric Pena
        'Description:  Verifies all required data has been provided by the user.
        'Parameters:  The values to be checked.
        'Returns: Boolean value identifying if all criteria has been satisfied.
        '--------------------------------------------------------------------
        If Trim(strName) = "" Then
            strErrMessage = "A Claims Processing Center Name is required."
            VerifyData = False
            Exit Function
        End If
        If Trim(strAddress1) = "" Then
            strErrMessage = "An Address is required."
            VerifyData = False
            Exit Function
        End If

        If Trim(strCity) = "" Then
            strErrMessage = "A City is required."
            VerifyData = False
            Exit Function
        End If
        If Trim(strState) = "" Then
            strErrMessage = "A State is required."
            VerifyData = False
            Exit Function
        End If

        strZip = md.NumbersOnly(strZip)

        If Trim(strZip) = "" Or (Len(strZip) <> 5 And Len(strZip) <> 9) Then
            strErrMessage = "A correct Zip Code is required."
            VerifyData = False
            Exit Function
        End If

        If lngInsuranceID < 0 Then
            strErrMessage = "An associated Insurance Company is required."
            VerifyData = False
            Exit Function
        End If

        strPhone = md.NumbersOnly(strPhone)

        If (Len(strPhone) <> 10 And Len(strPhone) <> 0) Then
            strErrMessage = "Phone Number is invalid."
            VerifyData = False
            Exit Function
        End If

        'Check for existance only when inserting new data
        If lngID = 0 And Exists(strAddress1, strCity, strState, strZip, lngInsuranceID) Then
            strErrMessage = "CPC at address: " & vbCrLf & strAddress1 & vbCrLf & strCity & ", " & strState & "  " & strZip & vbCrLf & " already exists."
            VerifyData = False
            Exit Function
        End If

        'If we get here, all is well...
        VerifyData = True

    End Function
    Public Function Exists(ByVal strAddress1 As String, ByVal strCity As String, ByVal strState As String, ByVal strZip As String, ByVal lngInsuranceID As Long) As Boolean
        '--------------------------------------------------------------------
        'Date: 02/24/2000
        'Author: Eric Pena
        'Description:  Determines if the BusinessName identical to the
        '              strBusinessName parameter already exists in the table.
        'Parameters: strBusinessName - Business name to be checked
        'Returns: True if the name exists, false otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        '

        Dim objCPC As New InsuranceDB.CCPCDB
        Dim blnExists As Boolean

        blnExists = objCPC.Exists(strAddress1, strCity, strState, strZip, lngInsuranceID)

        Exists = blnExists

        objCPC = Nothing


    End Function
    Public Function FetchByIns(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 5/01/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblClaimsProcCtr and tblInsCompany tables utilizing a stored procedure
        'Parameters: blnIncludeDisabled - determines if disabled records will be returned
        'Returns: ADORst
        '--------------------------------------------------------------------

        Dim objCPC As New InsuranceDB.CCPCDB


        FetchByIns = objCPC.FetchByIns(blnIncludeDisabled)


        objCPC = Nothing
    End Function

End Class
