
'--------------------------------------------------------------------
'Class Name: CEligibilityTxDB
'Date: 12/20/2023
'Author: DCO
'Description:  MTS object designed to host methods associated with data affecting EligibilityTx tables.
'--------------------------------------------------------------------
' Revision History:
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

Public Class CEligibilityTxDB

    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property

    Private _DB As New dbStuff.db
    Private _MD As New dbStuff.ModCommon


    Private Const CLASS_NAME As String = "CEligibilityTxDB"
    Private Const TABLE_NAME As String = "tblEligibilityTx"

    Public Function FetchByID(ByVal lngID As Long, Optional ByVal strDataBase As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 12/20/2023
        'Author: DCO
        'Description:  Retrieves records from the tblEligibilityTx table utilizing
        '              a stored procedure.
        'Parameters: None
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim cnn As ADODB.Connection
        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command



        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelEligibilityTxByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@EligibilityTxID", adInteger, adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        Select Case strDataBase
            Case "Test"
                cnn.Open(_ConnectionString)
            Case "PsyquelDirect"
                cnn.Open(_ConnectionString)
            Case Else
                cnn.Open(_ConnectionString)
        End Select
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByID = rst

        'Disconnect the recordset
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion



    End Function
    Public Function Insert(ByVal lngEligibilityID As Long, ByVal lngVisitLimit As Long, ByVal lngVisitsApplied As Long, ByVal strHasCoverageYN As String, ByVal strOutOfNetworkYN As String, _
             ByVal lngNumVisit As Long, ByVal strBenefitsText As String, ByVal strNotes As String, _
             ByVal strPlanName As String, ByVal strPlanAddress As String, ByVal strPlanCity As String, ByVal strPlanState As String, ByVal strPlanZip As String, _
             ByVal dteEffDate As Date, ByVal dteTermDate As Date, ByVal strMSPCode As String, ByVal dteVerifyDate As Date, ByVal strQueuedYN As String, _
             ByRef varINN As Object, ByRef varOON As Object, ByRef varCoPay As Object, ByRef varCert As Object, _
             ByVal dteDateProcessed As Date, ByVal strUserName As String, Optional ByVal strDataBase As String = "")

        '--------------------------------------------------------------------
        'Date: 12/20/2023
        'Author: DCO
        'Description:  Inserts a row into the tblEligibilityTx table utilizing
        '              a stored procedure.
        'Parameters: All data values to be inserted into the table
        'Returns: ID (Primary Key) of the inserted row.
        '--------------------------------------------------------------------
        Dim curINIndDeduct, curINIndDeductRem, curINIndDeductPaid, curINIndOOP, curINIndOOPRem, curINIndOOPPaid As Decimal
        Dim curINFamDeduct, curINFamDeductRem, curINFamDeductPaid, curINFamOOP, curINFamOOPRem, curINFamOOPPaid As Decimal
        Dim curOONIndDeduct, curOONIndDeductRem, curOONIndDeductPaid, curOONIndOOP, curOONIndOOPRem, curOONIndOOPPaid As Decimal
        Dim curOONFamDeduct, curOONFamDeductRem, curOONFamDeductPaid, curOONFamOOP, curOONFamOOPRem, curOONFamOOPPaid As Decimal
        Dim lngINPriCopayTypeID As Long
        Dim curINPriCopayAmt As Decimal
        Dim lngINSecCopayTypeID As Long
        Dim curINSecCopayAmt As Decimal
        Dim lngINTerCopayTypeID As Long
        Dim curINTerCopayAmt As Decimal
        Dim lngOONPriCopayTypeID As Long
        Dim curOONPriCopayAmt As Decimal
        Dim lngOONSecCopayTypeID As Long
        Dim curOONSecCopayAmt As Decimal
        Dim lngOONTerCopayTypeID As Long
        Dim curOONTerCopayAmt As Decimal
        Dim strCertReqd, strCertNumber As String
        Dim dteCertStartDate, dteCertEndDate As Date
        Dim lngCertUnits As Long
        Dim strCertCPT, strTestCertReqd, strTestCertNumber As String
        Dim dteTestCertStartDate, dteTestCertEndDate As Date
        Dim lngTestCertUnits As Long
        Dim strTestCertCPT As String

        Dim intCtr As Integer
        Dim lngID As Long
        Dim arrINN() As Object
        ReDim arrINN(12)
        Dim arrOON() As Object
        ReDim arrOON(12)
        Dim arrCoPay() As Object
        ReDim arrCoPay(12)
        Dim arrCert() As Object
        ReDim arrCert(12)

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        If Not IsArray(varINN) Or varINN.IsNullorEmpty Then
            For intCtr = 0 To UBound(arrINN)
                arrINN(intCtr) = 0
            Next intCtr
        Else
            For intCtr = 0 To UBound(varINN)
                arrINN(intCtr) = _DB.IfNull(varINN(intCtr), 0)
            Next intCtr
        End If

        If Not IsArray(varOON) Or varOON.IsNullorEmpty Then
            For intCtr = 0 To UBound(arrOON)
                arrOON(intCtr) = 0
            Next intCtr
        Else
            For intCtr = 0 To UBound(varOON)
                arrOON(intCtr) = _DB.IfNull(varOON(intCtr), 0)
            Next intCtr
        End If

        If Not IsArray(varCoPay) Or varCoPay.IsNullorEmpty Then
            For intCtr = 0 To UBound(arrCoPay)
                arrCoPay(intCtr) = 0
            Next intCtr
        Else
            For intCtr = 0 To UBound(varCoPay)
                arrCoPay(intCtr) = _DB.IfNull(varCoPay(intCtr), 0)
            Next intCtr
        End If

        If Not IsArray(varCert) Or varCert.IsNullorEmpty Then
            arrCert(0) = "N"
            arrCert(1) = ""
            arrCert(2) = DBNull.Value
            arrCert(3) = DBNull.Value
            arrCert(4) = 0
            arrCert(5) = ""
            arrCert(6) = "N"
            arrCert(7) = ""
            arrCert(8) = DBNull.Value
            arrCert(9) = DBNull.Value
            arrCert(10) = 0
            arrCert(11) = ""
        Else
            arrCert(0) = _DB.IfNull(varCert(0), "N")
            arrCert(1) = _DB.IfNull(varCert(1), "")
            arrCert(2) = IIf(CLng(varCert(2)) = 0, DBNull.Value, varCert(2))
            arrCert(3) = IIf(CLng(varCert(3)) = 0, DBNull.Value, varCert(2))
            arrCert(4) = _DB.IfNull(varCert(4), 0)
            arrCert(5) = _DB.IfNull(varCert(5), "")
            arrCert(6) = _DB.IfNull(varCert(6), "")
            arrCert(7) = _DB.IfNull(varCert(7), "")
            arrCert(8) = IIf(CLng(varCert(8)) = 0, DBNull.Value, varCert(8))
            arrCert(9) = IIf(CLng(varCert(9)) = 0, DBNull.Value, varCert(9))
            arrCert(10) = _DB.IfNull(varCert(10), 0)
            arrCert(11) = _DB.IfNull(varCert(11), "")
        End If

        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        cmdSQL.CommandText = "uspInsEligibilityTx"
        cmdSQL.CommandType = adCmdStoredProc

        With cmdSQL
            .Parameters.Append(.CreateParameter("@EligibilityID", adInteger, adParamInput, , lngEligibilityID))
            .Parameters.Append(.CreateParameter("@INIndDeduct", adDecimal, adParamInput, , arrINN(0)))
            .Parameters.Append(.CreateParameter("@INIndDeductRem", adDecimal, adParamInput, , arrINN(1)))
            .Parameters.Append(.CreateParameter("@INIndDeductPaid", adDecimal, adParamInput, , arrINN(2)))
            .Parameters.Append(.CreateParameter("@INIndOOP", adDecimal, adParamInput, , arrINN(3)))
            .Parameters.Append(.CreateParameter("@INIndOOPRem", adDecimal, adParamInput, , arrINN(4)))
            .Parameters.Append(.CreateParameter("@INIndOOPPaid", adDecimal, adParamInput, , arrINN(5)))
            .Parameters.Append(.CreateParameter("@INFamDeduct", adDecimal, adParamInput, , arrINN(6)))
            .Parameters.Append(.CreateParameter("@INFamDeductRem", adDecimal, adParamInput, , arrINN(7)))
            .Parameters.Append(.CreateParameter("@INFamDeductPaid", adDecimal, adParamInput, , arrINN(8)))
            .Parameters.Append(.CreateParameter("@INFamOOP", adDecimal, adParamInput, , arrINN(9)))
            .Parameters.Append(.CreateParameter("@INFamOOPRem", adDecimal, adParamInput, , arrINN(10)))
            .Parameters.Append(.CreateParameter("@INFamOOPPaid", adDecimal, adParamInput, , arrINN(11)))
            .Parameters.Append(.CreateParameter("@OONIndDeduct", adDecimal, adParamInput, , arrOON(0)))
            .Parameters.Append(.CreateParameter("@OONIndDeductRem", adDecimal, adParamInput, , arrOON(1)))
            .Parameters.Append(.CreateParameter("@OONIndDeductPaid", adDecimal, adParamInput, , arrOON(2)))
            .Parameters.Append(.CreateParameter("@OONIndOOP", adDecimal, adParamInput, , arrOON(3)))
            .Parameters.Append(.CreateParameter("@OONIndOOPRem", adDecimal, adParamInput, , arrOON(4)))
            .Parameters.Append(.CreateParameter("@OONIndOOPPaid", adDecimal, adParamInput, , arrOON(5)))
            .Parameters.Append(.CreateParameter("@OONFamDeduct", adDecimal, adParamInput, , arrOON(6)))
            .Parameters.Append(.CreateParameter("@OONFamDeductRem", adDecimal, adParamInput, , arrOON(7)))
            .Parameters.Append(.CreateParameter("@OONFamDeductPaid", adDecimal, adParamInput, , arrOON(8)))
            .Parameters.Append(.CreateParameter("@OONFamOOP", adDecimal, adParamInput, , arrOON(9)))
            .Parameters.Append(.CreateParameter("@OONFamOOPRem", adDecimal, adParamInput, , arrOON(10)))
            .Parameters.Append(.CreateParameter("@OONFamOOPPaid", adDecimal, adParamInput, , arrOON(11)))
            .Parameters.Append(.CreateParameter("@VisitLimit", adInteger, adParamInput, , lngVisitLimit))
            .Parameters.Append(.CreateParameter("@VisitsApplied", adInteger, adParamInput, , lngVisitsApplied))
            .Parameters.Append(.CreateParameter("@HasCoverageYN", adVarChar, adParamInput, 1, strHasCoverageYN))
            .Parameters.Append(.CreateParameter("@OutOfNetworkYN", adVarChar, adParamInput, 1, strOutOfNetworkYN))
            .Parameters.Append(.CreateParameter("@NumVisit", adInteger, adParamInput, , lngNumVisit))
            .Parameters.Append(.CreateParameter("@BenefitsText", adLongVarChar, adParamInput, IIf(Len(strBenefitsText) = 0, 1, Len(strBenefitsText))))
            .Parameters("@BenefitsText").AppendChunk(strBenefitsText)
            .Parameters.Append(.CreateParameter("@Notes", adLongVarChar, adParamInput, IIf(Len(strNotes) = 0, 1, Len(strNotes))))
            .Parameters("@Notes").AppendChunk(strNotes)
            .Parameters.Append(.CreateParameter("@INPriCopayTypeID", adInteger, adParamInput, , arrCoPay(0)))
            .Parameters.Append(.CreateParameter("@INPriCopayAmt", adDecimal, adParamInput, , arrCoPay(1)))
            .Parameters.Append(.CreateParameter("@INSecCopayTypeID", adInteger, adParamInput, , arrCoPay(2)))
            .Parameters.Append(.CreateParameter("@INSecCopayAmt", adDecimal, adParamInput, , arrCoPay(3)))
            .Parameters.Append(.CreateParameter("@INTerCopayTypeID", adInteger, adParamInput, , arrCoPay(4)))
            .Parameters.Append(.CreateParameter("@INTerCopayAmt", adDecimal, adParamInput, , arrCoPay(5)))
            .Parameters.Append(.CreateParameter("@OONPriCopayTypeID", adInteger, adParamInput, , arrCoPay(6)))
            .Parameters.Append(.CreateParameter("@OONPriCopayAmt", adDecimal, adParamInput, , arrCoPay(7)))
            .Parameters.Append(.CreateParameter("@OONSecCopayTypeID", adInteger, adParamInput, , arrCoPay(8)))
            .Parameters.Append(.CreateParameter("@OONSecCopayAmt", adDecimal, adParamInput, , arrCoPay(9)))
            .Parameters.Append(.CreateParameter("@OONTerCopayTypeID", adInteger, adParamInput, , arrCoPay(10)))
            .Parameters.Append(.CreateParameter("@OONTerCopayAmt", adDecimal, adParamInput, , arrCoPay(11)))
            .Parameters.Append(.CreateParameter("@PlanName", adVarChar, adParamInput, 30, strPlanName))
            .Parameters.Append(.CreateParameter("@PlanAddress", adVarChar, adParamInput, 50, strPlanAddress))
            .Parameters.Append(.CreateParameter("@PlanCity", adVarChar, adParamInput, 50, strPlanCity))
            .Parameters.Append(.CreateParameter("@PlanState", adVarChar, adParamInput, 2, strPlanState))
            .Parameters.Append(.CreateParameter("@PlanZip", adVarChar, adParamInput, 10, strPlanZip))
            .Parameters.Append(.CreateParameter("@EffDate", adDBTimeStamp, adParamInput, , IIf(CLng(dteEffDate.ToOADate()) = 0, DBNull.Value, dteEffDate)))
            .Parameters.Append(.CreateParameter("@TermDate", adDBTimeStamp, adParamInput, , IIf(CLng(dteTermDate.ToOADate()) = 0, DBNull.Value, dteTermDate)))
            .Parameters.Append(.CreateParameter("@CertReqd", adVarChar, adParamInput, 1, arrCert(0)))
            .Parameters.Append(.CreateParameter("@CertNumber", adVarChar, adParamInput, 30, arrCert(1)))
            .Parameters.Append(.CreateParameter("@CertStartDate", adDBTimeStamp, adParamInput, , IIf(CLng(arrCert(2)) = 0, DBNull.Value, arrCert(2))))
            .Parameters.Append(.CreateParameter("@CertEndDate", adDBTimeStamp, adParamInput, , IIf(CLng(arrCert(3)) = 0, DBNull.Value, arrCert(3))))
            .Parameters.Append(.CreateParameter("@CertUnits", adInteger, adParamInput, , arrCert(4)))
            .Parameters.Append(.CreateParameter("@CertCPT", adVarChar, adParamInput, 5, arrCert(5)))
            .Parameters.Append(.CreateParameter("@TestCertReqd", adVarChar, adParamInput, 1, arrCert(6)))
            .Parameters.Append(.CreateParameter("@TestCertNumber", adVarChar, adParamInput, 10, arrCert(7)))
            .Parameters.Append(.CreateParameter("@TestCertStartDate", adDBTimeStamp, adParamInput, , IIf(CLng(arrCert(8)) = 0, DBNull.Value, arrCert(8))))
            .Parameters.Append(.CreateParameter("@TestCertEndDate", adDBTimeStamp, adParamInput, , IIf(CLng(arrCert(9)) = 0, DBNull.Value, arrCert(9))))
            .Parameters.Append(.CreateParameter("@TestCertUnits", adInteger, adParamInput, , arrCert(10)))
            .Parameters.Append(.CreateParameter("@TestCertCPT", adVarChar, adParamInput, 5, arrCert(11)))
            .Parameters.Append(.CreateParameter("@MSPcode", adVarChar, adParamInput, 2, strMSPCode))
            .Parameters.Append(.CreateParameter("@VerifyDate", adDBTimeStamp, adParamInput, , IIf(CLng(dteVerifyDate.ToOADate()) = 0, DBNull.Value, dteVerifyDate)))
            .Parameters.Append(.CreateParameter("@QueuedYN", adVarChar, adParamInput, 1, strQueuedYN))
            .Parameters.Append(.CreateParameter("@DateProcessed", adDBTimeStamp, adParamInput, , IIf(CLng(dteDateProcessed.ToOADate()) = 0, DBNull.Value, dteDateProcessed)))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        Select Case strDataBase
            Case "Test"
                cnnSQL.Open(_ConnectionString)
            Case "PsyquelDirect"
                cnnSQL.Open(_ConnectionString)
            Case Else
                cnnSQL.Open(_ConnectionString)
        End Select

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute() '()

        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Function
        End If

        'Close the connection and free all resources
        cnnSQL.Close()
        cmdSQL = Nothing
        cnnSQL = Nothing


    End Function
    Public Sub Update(ByVal lngEligibilityID As Long, ByVal lngVisitLimit As Long, ByVal lngVisitsApplied As Long, ByVal strHasCoverageYN As String, ByVal strOutOfNetworkYN As String, _
             ByVal lngNumVisit As Long, ByVal strBenefitsText As String, ByVal strNotes As String, _
             ByVal strPlanName As String, ByVal strPlanAddress As String, ByVal strPlanCity As String, ByVal strPlanState As String, ByVal strPlanZip As String, _
             ByVal dteEffDate As Date, ByVal dteTermDate As Date, ByVal strMSPCode As String, ByVal dteVerifyDate As Date, ByVal strQueuedYN As String, _
             ByRef varINN As Object, ByRef varOON As Object, ByRef varCoPay As Object, ByRef varCert As Object, _
             ByVal dteDateProcessed As Date, ByVal strUserName As String, Optional ByVal strDataBase As String = "")
        'Date: 12/20/2023
        'Author: DCO
        'Description:  Updates a row into tblEligibilityTx table utilizing
        '              a stored procedure.
        'Parameters:
        '--------------------------------------------------------------------

        Dim intCtr As Integer
        Dim lngID As Long
        Dim arrINN() As Object
        ReDim arrINN(12)
        Dim arrOON() As Object
        ReDim arrOON(12)
        Dim arrCoPay() As Object
        ReDim arrCoPay(12)
        Dim arrCert() As Object
        ReDim arrCert(12)

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        If Not IsArray(varINN) Or varINN.IsNullorEmpty Then
            For intCtr = 0 To UBound(arrINN)
                arrINN(intCtr) = 0
            Next intCtr
        Else
            For intCtr = 0 To UBound(varINN)
                arrINN(intCtr) = _DB.IfNull(varINN(intCtr), 0)
            Next intCtr
        End If

        If Not IsArray(varOON) Or varOON.IsNullorEmpty Then
            For intCtr = 0 To UBound(arrOON)
                arrOON(intCtr) = 0
            Next intCtr
        Else
            For intCtr = 0 To UBound(varOON)
                arrOON(intCtr) = _DB.IfNull(varOON(intCtr), 0)
            Next intCtr
        End If

        If Not IsArray(varCoPay) Or varCoPay.IsNullorEmpty Then
            For intCtr = 0 To UBound(arrCoPay)
                arrCoPay(intCtr) = 0
            Next intCtr
        Else
            For intCtr = 0 To UBound(varCoPay)
                arrCoPay(intCtr) = _DB.IfNull(varCoPay(intCtr), 0)
            Next intCtr
        End If

        If Not IsArray(varCert) Or varCert.IsNullorEmpty Then
            arrCert(0) = "N"
            arrCert(1) = ""
            arrCert(2) = DBNull.Value
            arrCert(3) = DBNull.Value
            arrCert(4) = 0
            arrCert(5) = ""
            arrCert(6) = "N"
            arrCert(7) = ""
            arrCert(8) = DBNull.Value
            arrCert(9) = DBNull.Value
            arrCert(10) = 0
            arrCert(11) = ""
        Else
            arrCert(0) = _DB.IfNull(varCert(0), "N")
            arrCert(1) = _DB.IfNull(varCert(1), "")
            arrCert(2) = IIf(CLng(varCert(2)) = 0, DBNull.Value, varCert(2))
            arrCert(3) = IIf(CLng(varCert(3)) = 0, DBNull.Value, varCert(2))
            arrCert(4) = _DB.IfNull(varCert(4), 0)
            arrCert(5) = _DB.IfNull(varCert(5), "")
            arrCert(6) = _DB.IfNull(varCert(6), "")
            arrCert(7) = _DB.IfNull(varCert(7), "")
            arrCert(8) = IIf(CLng(varCert(8)) = 0, DBNull.Value, varCert(8))
            arrCert(9) = IIf(CLng(varCert(9)) = 0, DBNull.Value, varCert(9))
            arrCert(10) = _DB.IfNull(varCert(10), 0)
            arrCert(11) = _DB.IfNull(varCert(11), "")
        End If

        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        cmdSQL.CommandText = "uspUpdEligibilityTx"
        cmdSQL.CommandType = adCmdStoredProc

        With cmdSQL
            .Parameters.Append(.CreateParameter("@EligibilityID", adInteger, adParamInput, , lngEligibilityID))
            .Parameters.Append(.CreateParameter("@INIndDeduct", adDecimal, adParamInput, , arrINN(0)))
            .Parameters.Append(.CreateParameter("@INIndDeductRem", adDecimal, adParamInput, , arrINN(1)))
            .Parameters.Append(.CreateParameter("@INIndDeductPaid", adDecimal, adParamInput, , arrINN(2)))
            .Parameters.Append(.CreateParameter("@INIndOOP", adDecimal, adParamInput, , arrINN(3)))
            .Parameters.Append(.CreateParameter("@INIndOOPRem", adDecimal, adParamInput, , arrINN(4)))
            .Parameters.Append(.CreateParameter("@INIndOOPPaid", adDecimal, adParamInput, , arrINN(5)))
            .Parameters.Append(.CreateParameter("@INFamDeduct", adDecimal, adParamInput, , arrINN(6)))
            .Parameters.Append(.CreateParameter("@INFamDeductRem", adDecimal, adParamInput, , arrINN(7)))
            .Parameters.Append(.CreateParameter("@INFamDeductPaid", adDecimal, adParamInput, , arrINN(8)))
            .Parameters.Append(.CreateParameter("@INFamOOP", adDecimal, adParamInput, , arrINN(9)))
            .Parameters.Append(.CreateParameter("@INFamOOPRem", adDecimal, adParamInput, , arrINN(10)))
            .Parameters.Append(.CreateParameter("@INFamOOPPaid", adDecimal, adParamInput, , arrINN(11)))
            .Parameters.Append(.CreateParameter("@OONIndDeduct", adDecimal, adParamInput, , arrOON(0)))
            .Parameters.Append(.CreateParameter("@OONIndDeductRem", adDecimal, adParamInput, , arrOON(1)))
            .Parameters.Append(.CreateParameter("@OONIndDeductPaid", adDecimal, adParamInput, , arrOON(2)))
            .Parameters.Append(.CreateParameter("@OONIndOOP", adDecimal, adParamInput, , arrOON(3)))
            .Parameters.Append(.CreateParameter("@OONIndOOPRem", adDecimal, adParamInput, , arrOON(4)))
            .Parameters.Append(.CreateParameter("@OONIndOOPPaid", adDecimal, adParamInput, , arrOON(5)))
            .Parameters.Append(.CreateParameter("@OONFamDeduct", adDecimal, adParamInput, , arrOON(6)))
            .Parameters.Append(.CreateParameter("@OONFamDeductRem", adDecimal, adParamInput, , arrOON(7)))
            .Parameters.Append(.CreateParameter("@OONFamDeductPaid", adDecimal, adParamInput, , arrOON(8)))
            .Parameters.Append(.CreateParameter("@OONFamOOP", adDecimal, adParamInput, , arrOON(9)))
            .Parameters.Append(.CreateParameter("@OONFamOOPRem", adDecimal, adParamInput, , arrOON(10)))
            .Parameters.Append(.CreateParameter("@OONFamOOPPaid", adDecimal, adParamInput, , arrOON(11)))
            .Parameters.Append(.CreateParameter("@VisitLimit", adInteger, adParamInput, , lngVisitLimit))
            .Parameters.Append(.CreateParameter("@VisitsApplied", adInteger, adParamInput, , lngVisitsApplied))
            .Parameters.Append(.CreateParameter("@HasCoverageYN", adVarChar, adParamInput, 1, strHasCoverageYN))
            .Parameters.Append(.CreateParameter("@OutOfNetworkYN", adVarChar, adParamInput, 1, strOutOfNetworkYN))
            .Parameters.Append(.CreateParameter("@NumVisit", adInteger, adParamInput, , lngNumVisit))
            .Parameters.Append(.CreateParameter("@BenefitsText", adLongVarChar, adParamInput, IIf(Len(strBenefitsText) = 0, 1, Len(strBenefitsText))))
            .Parameters("@BenefitsText").AppendChunk(strBenefitsText)
            .Parameters.Append(.CreateParameter("@Notes", adLongVarChar, adParamInput, IIf(Len(strNotes) = 0, 1, Len(strNotes))))
            .Parameters("@Notes").AppendChunk(strNotes)
            .Parameters.Append(.CreateParameter("@INPriCopayTypeID", adInteger, adParamInput, , arrCoPay(0)))
            .Parameters.Append(.CreateParameter("@INPriCopayAmt", adDecimal, adParamInput, , arrCoPay(1)))
            .Parameters.Append(.CreateParameter("@INSecCopayTypeID", adInteger, adParamInput, , arrCoPay(2)))
            .Parameters.Append(.CreateParameter("@INSecCopayAmt", adDecimal, adParamInput, , arrCoPay(3)))
            .Parameters.Append(.CreateParameter("@INTerCopayTypeID", adInteger, adParamInput, , arrCoPay(4)))
            .Parameters.Append(.CreateParameter("@INTerCopayAmt", adDecimal, adParamInput, , arrCoPay(5)))
            .Parameters.Append(.CreateParameter("@OONPriCopayTypeID", adInteger, adParamInput, , arrCoPay(6)))
            .Parameters.Append(.CreateParameter("@OONPriCopayAmt", adDecimal, adParamInput, , arrCoPay(7)))
            .Parameters.Append(.CreateParameter("@OONSecCopayTypeID", adInteger, adParamInput, , arrCoPay(8)))
            .Parameters.Append(.CreateParameter("@OONSecCopayAmt", adDecimal, adParamInput, , arrCoPay(9)))
            .Parameters.Append(.CreateParameter("@OONTerCopayTypeID", adInteger, adParamInput, , arrCoPay(10)))
            .Parameters.Append(.CreateParameter("@OONTerCopayAmt", adDecimal, adParamInput, , arrCoPay(11)))
            .Parameters.Append(.CreateParameter("@PlanName", adVarChar, adParamInput, 30, strPlanName))
            .Parameters.Append(.CreateParameter("@PlanAddress", adVarChar, adParamInput, 50, strPlanAddress))
            .Parameters.Append(.CreateParameter("@PlanCity", adVarChar, adParamInput, 50, strPlanCity))
            .Parameters.Append(.CreateParameter("@PlanState", adVarChar, adParamInput, 2, strPlanState))
            .Parameters.Append(.CreateParameter("@PlanZip", adVarChar, adParamInput, 10, strPlanZip))
            .Parameters.Append(.CreateParameter("@EffDate", adDBTimeStamp, adParamInput, , IIf(CLng(dteEffDate.ToOADate()) = 0, DBNull.Value, dteEffDate)))
            .Parameters.Append(.CreateParameter("@TermDate", adDBTimeStamp, adParamInput, , IIf(CLng(dteTermDate.ToOADate()) = 0, DBNull.Value, dteTermDate)))
            .Parameters.Append(.CreateParameter("@CertReqd", adVarChar, adParamInput, 1, arrCert(0)))
            .Parameters.Append(.CreateParameter("@CertNumber", adVarChar, adParamInput, 30, arrCert(1)))
            .Parameters.Append(.CreateParameter("@CertStartDate", adDBTimeStamp, adParamInput, , IIf(CLng(arrCert(2)) = 0, DBNull.Value, arrCert(2))))
            .Parameters.Append(.CreateParameter("@CertEndDate", adDBTimeStamp, adParamInput, , IIf(CLng(arrCert(3)) = 0, DBNull.Value, arrCert(3))))
            .Parameters.Append(.CreateParameter("@CertUnits", adInteger, adParamInput, , arrCert(4)))
            .Parameters.Append(.CreateParameter("@CertCPT", adVarChar, adParamInput, 5, arrCert(5)))
            .Parameters.Append(.CreateParameter("@TestCertReqd", adVarChar, adParamInput, 1, arrCert(6)))
            .Parameters.Append(.CreateParameter("@TestCertNumber", adVarChar, adParamInput, 10, arrCert(7)))
            .Parameters.Append(.CreateParameter("@TestCertStartDate", adDBTimeStamp, adParamInput, , IIf(CLng(arrCert(8)) = 0, DBNull.Value, arrCert(8))))
            .Parameters.Append(.CreateParameter("@TestCertEndDate", adDBTimeStamp, adParamInput, , IIf(CLng(arrCert(9)) = 0, DBNull.Value, arrCert(9))))
            .Parameters.Append(.CreateParameter("@TestCertUnits", adInteger, adParamInput, , arrCert(10)))
            .Parameters.Append(.CreateParameter("@TestCertCPT", adVarChar, adParamInput, 5, arrCert(11)))
            .Parameters.Append(.CreateParameter("@MSPcode", adVarChar, adParamInput, 2, strMSPCode))
            .Parameters.Append(.CreateParameter("@VerifyDate", adDBTimeStamp, adParamInput, , IIf(CLng(dteVerifyDate.ToOADate()) = 0, DBNull.Value, dteVerifyDate)))
            .Parameters.Append(.CreateParameter("@QueuedYN", adVarChar, adParamInput, 1, strQueuedYN))
            .Parameters.Append(.CreateParameter("@DateProcessed", adDBTimeStamp, adParamInput, , IIf(CLng(dteDateProcessed.ToOADate()) = 0, DBNull.Value, dteDateProcessed)))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        Select Case strDataBase
            Case "Test"
                cnnSQL.Open(_ConnectionString)
            Case "PsyquelDirect"
                cnnSQL.Open(_ConnectionString)
            Case Else
                cnnSQL.Open(_ConnectionString)
        End Select

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute() '()

        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection and free all resources
        cnnSQL.Close()
        cmdSQL = Nothing
        cnnSQL = Nothing



    End Sub

    Public Function Exists(ByVal lngID As Long, Optional ByVal strDataBase As String = "") As Boolean


        '--------------------------------------------------------------------
        'Date: 12/20/2023
        'Author: DCO
        'Description:  Checks for existing entries in tblEligibilityTx
        'Parameters: lngID - fldEligibilityID number to check for existing entries in table
        '
        '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim strSQL As String
        Dim blnExists As Boolean



        'Prepare the SQL statement

        strSQL = "SELECT "
        strSQL = strSQL & "COUNT(*) AS TOTAL "
        strSQL = strSQL & "FROM "
        strSQL = strSQL & TABLE_NAME
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & " fldEligibilityID = " & lngID

        'Instantiate the Recordset.
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = adUseClient

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        Select Case strDataBase
            Case "Test"
                cnnSQL.Open(_ConnectionString)
            Case "PsyquelDirect"
                cnnSQL.Open(_ConnectionString)
            Case Else
                cnnSQL.Open(_ConnectionString)
        End Select

        ' Populate the recordset
        rstSQL.Open(strSQL, cnnSQL, adOpenStatic, adLockReadOnly, adCmdText + adAsyncFetch)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing

        'Close the connection and free all resources
        cnnSQL.Close()
        cnnSQL = Nothing

        'Get the value from the Recordset then destroy it.
        If rstSQL.Fields("TOTAL").Value > 0 Then
            Exists = True
        Else
            Exists = False
        End If
        rstSQL = Nothing

        'Signal successful completion



    End Function


End Class



