
'--------------------------------------------------------------------
'Class Name: CInsuranceBz
'Date: 02/24/2000
'Author: Eric Pena
'Denis J Finkel
'Description:  MTS business object designed to   methods associated
'              with the CInsuranceDB class.
'--------------------------------------------------------------------
' Revision History:
'   R001 07/05/2000 Richkun: Added FetchByName() method
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
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CInsuranceBz

    Private Const CLASS_NAME As String = "CInsuranceBz"
    Private Const TABLE_NAME As String = "tblInsCompany"

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



    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False, Optional ByVal strWhere As String = "", Optional ByVal strOrderBy As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/24/2000
        'Author: Eric Pena
        'Description:  Retrieves records from the tblInsCompany table.
        'Parameters: blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Disabled' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        '            strWhere - Optional 'Where' clause of the SQL statement by
        '               which records will be filtered.  If the parameter is not
        '               supplied, all records will be retrieved.
        '             strOrderBy - Optional 'Order By' clause in which retrieved
        '               records will be sorted.  The default is set to the
        '               'fldName' column
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim rstSQL As New ADODB.Recordset
        Dim objInsurance As New InsuranceDB.CInsuranceDB



        objInsurance.ConnectionString = _ConnectionString

        rstSQL = objInsurance.Fetch(blnIncludeDisabled, strWhere, strOrderBy)
        Fetch = rstSQL

        objInsurance = Nothing


    End Function

    Public Function FetchByName(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/05/2000
        'Author: Dave Richkun
        'Description:  Retrieves records from the tblInsCompany table utilizing a
        '            stored procedure.
        'Parameters: blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Disabled' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim rstSQL As New ADODB.Recordset
        Dim objInsurance As New InsuranceDB.CInsuranceDB

        objInsurance.ConnectionString = _ConnectionString



        rstSQL = objInsurance.FetchByName(blnIncludeDisabled)
        FetchByName = rstSQL

        objInsurance = Nothing


    End Function


    Public Function Insert(ByVal strName As String, ByVal strPayerCode As String, ByVal blnElect As Boolean, ByVal blnEnroll As Boolean, _
                           ByVal strNotes As String, ByVal strUsername As String) As Long
        '--------------------------------------------------------------------
        'Date: 02/24/2000
        'Author: Eric Pena
        'Description:  Inserts a row into the tblInsCompany table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------

        Dim objInsurance As New InsuranceDB.CInsuranceDB
        Dim strErrMessage As String
        objInsurance.ConnectionString = _ConnectionString

        If Not VerifyData(0, strName, strErrMessage) Then Exit Function

        Insert = objInsurance.Insert(strName, strPayerCode, blnElect, blnEnroll, strNotes, strUsername)
        objInsurance = Nothing


    End Function

    Public Sub Update(ByVal lngID As Long, ByVal strName As String, ByVal strPayerCode As String, ByVal blnElect As Boolean, _
                      ByVal blnEnroll As Boolean, ByVal strNotes As String)
        '--------------------------------------------------------------------
        'Date: 02/24/2000
        'Author: Eric Pena
        'Description:  Updates a row in the tblInsCompany table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objInsurance As New InsuranceDB.CInsuranceDB
        Dim strErrMessage As String

        objInsurance.ConnectionString = _ConnectionString


        If Not VerifyData(lngID, strName, strErrMessage) Then
            Exit Sub
        End If

        objInsurance.Update(lngID, strName, strPayerCode, blnElect, blnEnroll, strNotes)
        objInsurance = Nothing


    End Sub

    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Long, ByVal strDeletedBy As String)
        '--------------------------------------------------------------------
        'Date: 02/24/2000
        'Author: Eric Pena
        'Description:  Flags a row in the tblInsCompany table marking the row as
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


        Dim objInsurance As New InsuranceDB.CInsuranceDB

        objInsurance.ConnectionString = _ConnectionString

        'TODO: decide if cascade disablements are in effect for cpcs, plans.
        'is so, check to make sure there arent any active bills/rps with the affected plans

        objInsurance.Deleted(blnDeleted, lngID, strDeletedBy)

        objInsurance = Nothing

    End Sub

    Private Function VerifyData(ByVal lngID As Long, ByVal strName As String, ByRef strErrMessage As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 02/24/2000
        'Author: Eric Pena
        'Description:  Verifies all required data has been provided by the user.
        'Parameters:  The values to be checked.
        'Returns: Boolean value identifying if all criteria has been satisfied.
        '--------------------------------------------------------------------

        If Trim(strName) = "" Then
            strErrMessage = "Insurance Name is required."
            VerifyData = False
            Exit Function
        End If

        'Check for existance only when inserting new data
        If lngID = 0 And Exists(strName) Then
            strErrMessage = "Insurance Company '" & strName & "' already exists."
            VerifyData = False
            Exit Function
        End If

        'If we get here, all is well...
        VerifyData = True

    End Function

    Private Function Exists(ByVal strName As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 02/24/2000
        'Author: Eric Pena
        'Description:  Determines if the Insurance Name identical to the
        '              strName parameter already exists in the table.
        'Parameters: strName - Insurance name to be checked
        'Returns: True if the name exists, false otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        '

        Dim objInsurance As New InsuranceDB.CInsuranceDB
        objInsurance.ConnectionString = _ConnectionString

        Exists = objInsurance.Exists(strName)
        objInsurance = Nothing
        'Signal successful completion

    End Function

    Public Function InsertHierarchy(ByVal blnInsertIns As Boolean, ByVal InsertCPC As Boolean, ByVal lngInsuranceID As Long, ByVal lngCPCID As Long, _
                                    ByVal strInsName As String, ByVal strPayerCode As String, ByVal blnElect As Boolean, ByVal blnEnroll As Boolean, ByVal strInsNotes As String, _
                                    ByVal strCPCName As String, ByVal strCPCAddress1 As String, ByVal strCPCAddress2 As String, _
                                    ByVal strCPCCity As String, ByVal strCPCState As String, ByVal strCPCZip As String, ByVal strCPCPhone As String, ByVal strCPCExt As String, ByVal strCPCFax As String, _
                                    ByVal strCPCEmail As String, ByVal strCPCContact As String, ByVal strCPCNotes As String, ByVal lngClearingHouseID As Long, ByVal strCPCPayerCode As String, ByVal strCPCInsType As String, _
                                    ByVal strPlnName As String, ByVal lngPlanClassID As Long, ByVal strPlnNotes As String, ByVal lngFileDeadline As Long, ByVal strUsername As String) As Object
        '--------------------------------------------------------------------
        'Date: 02/24/2000
        'Author: Eric Pena
        'Description:  Inserts a row (if applicable) into the tblInsCompany, tblClaimsProcCtr, and tblPlan tables utilizing stored procedures.
        'Parameters:    blnInsertIns - True if an Insurance is to be inserted, False otherwise
        '                       blnInsertCPC - True if a CPC is to be inserted, False otherwise
        '                       lngInsuranceID - Insurance ID to use if blnInsertIns is false, ignored otherwise
        '                       lngCPCID - CPC ID to use if blnInsertCPC is false, ignored otherwise
        '                       All other parameters identifies the column value that will be inserted.
        'Returns: Varaint array of the Insurance, CPC, and Plan IDs (Primary Key) of the row inserted.
        '--------------------------------------------------------------------

        Dim objIns As New InsuranceBz.CInsuranceBz
        Dim objCPC As New InsuranceBz.CCPCBz
        Dim objPln As New InsuranceBz.CPlanBz
        Dim lngPlanID As Long
        Dim strErrMessage As String
        Dim aryIDs(2) As Long

        objIns.ConnectionString = _ConnectionString
        objCPC.ConnectionString = _ConnectionString
        objPln.ConnectionString = _ConnectionString



        'Handle the Insurance Co----------------------------------------------------------------------------------

        If blnInsertIns Then
            lngInsuranceID = objIns.Insert(strInsName, strPayerCode, blnElect, blnEnroll, strInsNotes, strUsername)
        End If
        aryIDs(0) = lngInsuranceID

        'Handle the CPC -------------------------------------------------------------------------------------------

        If InsertCPC Then
            lngCPCID = objCPC.Insert(lngInsuranceID, strCPCName, strCPCAddress1, strCPCAddress2, strCPCCity, strCPCState, strCPCZip, strCPCPhone, strCPCExt, strCPCFax, strCPCEmail, strCPCContact, strCPCNotes, lngClearingHouseID, strCPCPayerCode, strCPCInsType, strUsername)
        End If
        aryIDs(1) = lngCPCID

        'Handle the Plan -------------------------------------------------------------------------------------------

        lngPlanID = objPln.Insert(strPlnName, lngPlanClassID, lngCPCID, strPlnNotes, lngFileDeadline, strUsername)
        aryIDs(2) = lngPlanID
        InsertHierarchy = aryIDs

  
        objIns = Nothing

    End Function

    Public Function FetchByPlan(ByVal lngPlanID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/05/2001
        'Author: Eric Pena
        'Description:  Retrieves records from the tblInsCompany table utilizing a
        '            stored procedure.
        'Parameters: blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Disabled' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim rstSQL As ADODB.Recordset
        Dim objInsurance As New InsuranceDB.CInsuranceDB


        objInsurance.ConnectionString = _ConnectionString

        rstSQL = objInsurance.FetchByPlan(lngPlanID)
        FetchByPlan = rstSQL


        objInsurance = Nothing




    End Function
    Public Function FetchByProvider(ByVal lngProviderID As Long, ByVal lngInsuranceID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 04/21/2006
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblInsCompany table utilizing a
        '            stored procedure.
        'Parameters: blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Disabled' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim rstSQL As New ADODB.Recordset
        Dim objInsurance As New InsuranceDB.CInsuranceDB

        objInsurance.ConnectionString = _ConnectionString


        rstSQL = objInsurance.FetchByProvider(lngProviderID, lngInsuranceID, blnIncludeDisabled)
        FetchByProvider = rstSQL


        objInsurance = Nothing


    End Function

    Public Function FetchByGroup(ByVal lngGroupID As Long, ByVal lngInsuranceID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 04/21/2006
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblInsCompany table utilizing a
        '            stored procedure.
        'Parameters: blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Disabled' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim rstSQL As New ADODB.Recordset
        Dim objInsurance As New InsuranceDB.CInsuranceDB

        objInsurance.ConnectionString = _ConnectionString



        rstSQL = objInsurance.FetchByGroup(lngGroupID, lngInsuranceID, blnIncludeDisabled)
        FetchByGroup = rstSQL


        objInsurance = Nothing

    End Function

    Public Function FetchByBR(ByVal lngBRID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/05/2001
        'Author: Eric Pena
        'Description:  Retrieves records from the tblInsCompany table utilizing a
        '            stored procedure.
        'Parameters: blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Disabled' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim rstSQL As New ADODB.Recordset
        Dim objInsurance As New InsuranceDB.CInsuranceDB

        objInsurance.ConnectionString = _ConnectionString



        rstSQL = objInsurance.FetchByBR(lngBRID)
        FetchByBR = rstSQL


        objInsurance = Nothing


    End Function


End Class
