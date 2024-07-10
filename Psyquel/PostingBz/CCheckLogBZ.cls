
'--------------------------------------------------------------------
'Class Name: CCheckLogBZ
'Date: 10/28/2004
'Author: Duane C Orth
'Denis J Finkel
'Description:  MTS business object designed to   methods associated
'              with the CCheckLogDB class.
'--------------------------------------------------------------------
'Revision History:
'
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

Public Class CCheckLogBZ


    Private _ConnectionString As String = String.Empty


    Private Const CLASS_NAME As String = "CCheckLogBZ"
    Private Const TABLE_NAME As String = "tblPayerCheckLog"


    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property

    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False, _
                          Optional ByVal strWhere As String = "", _
                          Optional ByVal strOrderby As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/28/2004
        'Author: Duane C Orth
        'Description:  Retrieves records from the tblPayerCheckLog table.
        'Parameters: blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as ' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        '            strWhere - Optional 'Where' clause of the SQL statement by
        '               which records will be filtered.  If the parameter is not
        '               supplied, all records will be retrieved.
        '             strOrderBy - Optional 'Order By' clause in which retrieved
        '               records will be sorted.  The default is set to the
        '               'fldPlanName' column
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim objPayerCheckLog As New PostingDB.CCheckLogDB

        objPayerCheckLog.ConnectionString = _ConnectionString


        Dim rstSQL As New ADODB.Recordset


        rstSQL = objPayerCheckLog.Fetch(blnIncludeDisabled, strWhere, strOrderby)
        Fetch = rstSQL


        objPayerCheckLog = Nothing
        Exit Function


    End Function
    Public Function Insert(ByVal intCheckID As Long, _
                           ByVal intInsuranceID As Long, ByVal intProviderID As Long, _
                           ByVal intPatientID As Long, ByVal intEncounterLogID As Long, _
                           ByVal lngEncDetailID As Long, ByVal lngCptRecordID As Long, _
                           ByVal blnPost As Boolean, ByVal dtePostDate As Date, _
                           ByVal curPayment As Decimal, ByVal intPmtTxTypeID As Long, _
                           ByVal curDisallowance As Decimal, ByVal intDisTxTypeID As Long, _
                           ByVal curDeductable As Decimal, ByVal intDedTxTypeID As Long, _
                           ByVal curCoInsurance As Decimal, ByVal intCoInsTxTypeID As Long, _
                           ByVal curRefund As Decimal, ByVal intRefTxTypeID As Long, _
                           ByVal curDenial As Decimal, ByVal intDenTxTypeID As Long, _
                           ByVal strNotes As String, ByVal intNoteTxTypeID As Long, _
                           ByVal blnPrintHcfa As Boolean, ByVal intPlanID As Long, _
                           ByVal strAddedBy As String) As Long
        '--------------------------------------------------------------------
        'Date: 10/28/2004
        'Author: Duane C Orth
        'Description:  Inserts a row into the tblPayerCheckLog table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------

        Dim objPayerCheckLog As New PostingDB.CCheckLogDB

        objPayerCheckLog.ConnectionString = _ConnectionString


        Dim strErrMessage As String

        If Not VerifyData(intCheckID, intInsuranceID, intProviderID, intPatientID, intEncounterLogID, _
                            dtePostDate, intPmtTxTypeID, curPayment, strErrMessage) Then Exit Function

        Insert = objPayerCheckLog.Insert(intCheckID, intInsuranceID, _
                intProviderID, intPatientID, intEncounterLogID, lngEncDetailID, lngCptRecordID, blnPost, _
                dtePostDate, curPayment, intPmtTxTypeID, curDisallowance, _
                intDisTxTypeID, curDeductable, intDedTxTypeID, curCoInsurance, intCoInsTxTypeID, curRefund, _
                intRefTxTypeID, curDenial, intDenTxTypeID, _
                strNotes, intNoteTxTypeID, blnPrintHcfa, intPlanID, _
                strAddedBy)

    End Function
    Public Sub Update(ByVal lngID As Long, _
                        ByVal intCheckID As Long, _
                           ByVal intInsuranceID As Long, ByVal intProviderID As Long, _
                           ByVal intPatientID As Long, ByVal intEncounterLogID As Long, _
                           ByVal lngEncDetailID As Long, ByVal lngCptRecordID As Long, _
                           ByVal blnPost As Boolean, ByVal dtePostDate As Date, _
                           ByVal curPayment As Decimal, ByVal intPmtTxTypeID As Long, _
                           ByVal curDisallowance As Decimal, ByVal intDisTxTypeID As Long, _
                           ByVal curDeductable As Decimal, ByVal intDedTxTypeID As Long, _
                           ByVal curCoInsurance As Decimal, ByVal intCoInsTxTypeID As Long, _
                           ByVal curRefund As Decimal, ByVal intRefTxTypeID As Long, _
                           ByVal curDenial As Decimal, ByVal intDenTxTypeID As Long, _
                           ByVal strNotes As String, ByVal intNoteTxTypeID As Long, _
                           ByVal blnPrintHcfa As Boolean, ByVal intPlanID As Long)
        '--------------------------------------------------------------------
        'Date: 10/28/2004
        'Author: Duane C Orth
        'Description:  Updates a row in the tblPayerCheckLog table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objPayerCheckLog As New PostingDB.CCheckLogDB
        Dim strErrMessage As String


        objPayerCheckLog.ConnectionString = _ConnectionString


        objPayerCheckLog.Update(lngID, intCheckID, intInsuranceID, _
              intProviderID, intPatientID, intEncounterLogID, lngEncDetailID, lngCptRecordID, blnPost, _
              dtePostDate, curPayment, intPmtTxTypeID, curDisallowance, _
              intDisTxTypeID, curDeductable, intDedTxTypeID, curCoInsurance, intCoInsTxTypeID, curRefund, _
              intRefTxTypeID, curDenial, intDenTxTypeID, _
              strNotes, intNoteTxTypeID, blnPrintHcfa, intPlanID)



    End Sub
    Public Sub UpdatePosting(ByVal lngID As Long, _
                        ByVal lngCheckID As Long, _
                        ByVal blnPost As Boolean, _
                        ByVal dtePostDate As Date)
        '--------------------------------------------------------------------
        'Date: 10/28/2004
        'Author: Duane C Orth
        'Description:  Updates a row in the tblPayerCheckLog table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objPayerCheckLog As New PostingDB.CCheckLogDB


        objPayerCheckLog.ConnectionString = _ConnectionString


        Dim strErrMessage As String

        objPayerCheckLog.UpdatePosting(lngID, lngCheckID, blnPost, dtePostDate)



    End Sub
    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Long, ByVal strDeletedBy As String)
        '--------------------------------------------------------------------
        'Date: 10/28/2004
        'Author: Duane C Orth
        'Description:  Flags a row in the tblPayerCheckLog table marking the row as
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

        Dim objPayerCheckLog As New PostingDB.CCheckLogDB
        objPayerCheckLog.ConnectionString = _ConnectionString


        objPayerCheckLog.Deleted(blnDeleted, lngID, strDeletedBy)


        objPayerCheckLog = Nothing


    End Sub

    Private Function VerifyData(ByVal intCheckID As Long, _
                                ByVal intInsuranceID As Long, _
                                ByVal intProviderID As Long, _
                                ByVal intPatientID As Long, _
                                ByVal intEncounterLogID As Long, _
                                ByVal dtePostDate As Date, _
                                ByVal intTxTypeID As Long, _
                                ByVal curAmount As Decimal, _
                                ByVal strErrMessage As String) As Boolean
        '------------------------------------------------------------------------------------
        'Date: 09/25/00
        'Author: Duane C Orth
        'Description:   Verifies that all required data has been provided by the user.
        'Parameters:    Values to be verified.
        'Returns:       Boolean value identifying if all required criteria has been satisfied
        '------------------------------------------------------------------------------------

        If intInsuranceID < 0 Then
            strErrMessage = "A Payer is required."
            VerifyData = False
            Exit Function
        End If

        If intProviderID < 0 Then
            strErrMessage = "A Provider is required."
            VerifyData = False
            Exit Function
        End If

        If intPatientID < 0 Then
            strErrMessage = "A Patient is required."
            VerifyData = False
            Exit Function
        End If

        If intEncounterLogID < 0 Then
            strErrMessage = "Date of Service is required."
            VerifyData = False
            Exit Function
        End If

        If dtePostDate.ToOADate() <> 0 Then
            If dtePostDate > Date.Now Then

                strErrMessage = "Post Date is invalid."
                VerifyData = False
                Exit Function
            End If
        End If

        VerifyData = True

    End Function

End Class
