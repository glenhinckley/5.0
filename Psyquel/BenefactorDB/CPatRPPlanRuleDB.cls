
'--------------------------------------------------------------------
'Class Name: CPatRPPlanRuleDB
'Date: 11/07/2000
'Author: Morgan Travis
'Description:  MTS object designed to allow insertions into the tblPatRPPlanRule table.
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
' Lydia Orth 


Imports Psyquel.BusinessRules.CoreLibraryIII



Public Class CPatRPPlanRuleDB


    Private _ConnectionString As String = String.Empty



    Private Const TABLE_NAME As String = "tblPatRPPlanRule"
    Private Const CLASS_NAME As String = "CPatRPPlanRuleDB"

    Private _DB As New dbStuff.db
    Private _MD As New dbStuff.ModCommon

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property




    Public Function Update(ByVal lngPatRPPlanID As Long, ByVal strRPRelat As String, ByVal strCardNum As String, ByVal strGroupNum As String, _
                                        ByVal strMedicareHICNum As String, ByVal curAmtOfGrpDeductPd As Decimal, ByVal dteFiscalEndDate As Date, _
                                        ByVal curDeduct As Decimal, ByVal strGroupDeduct As String, ByVal curDeductRem As Decimal, _
                                        ByVal curDollarLimit As Decimal, ByVal curDollarsApplied As Decimal, ByVal intVisitLimit As Integer, _
                                        ByVal intVisitsApplied As Integer, ByVal strCatCap As String, ByVal curCatCapAmt As Decimal, _
                                        ByVal strCertReqd As String, ByVal strNoHCFA As String, ByVal strNoInvoice As String, ByVal strOutpatientRpt As String, _
                                        ByVal strVerbalRept As String, ByVal intNumVisit As Integer, ByVal intDaysPrior As Integer, ByVal strElectClaim As String, ByVal strRecurrCopay As String)
        '--------------------------------------------------------------------
        'Date: 03/01/2000
        'Author: Eric Pena
        'Description:  Creates a billing association for the given patient-rp-plan combonation
        'Parameters: lngPatRPPlanID - The ID of the key in tblPatRPPlanRule
        '                  All other fields represent columns to be updated
        'Returns: T if the association is enabled/exists, F otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim strSQL As String
        Dim intCTR As Integer
        Dim lngID As Long


        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)
        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        With cmdSQL
            .CommandText = "uspUpdPatRPPlanRule"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatRPPlanID", adInteger, adParamInput, , lngPatRPPlanID))
            .Parameters.Append(.CreateParameter("@RPRelat", adVarChar, adParamInput, 30, strRPRelat))
            .Parameters.Append(.CreateParameter("@CardNum", adVarChar, adParamInput, 30, strCardNum))
            .Parameters.Append(.CreateParameter("@GroupNum", adVarChar, adParamInput, 30, strGroupNum))
            .Parameters.Append(.CreateParameter("@MedicareHICNum", adVarChar, adParamInput, 30, strMedicareHICNum))
            .Parameters.Append(.CreateParameter("@AmtOfGrpDeductPd", adDecimal, adParamInput, , curAmtOfGrpDeductPd))
            .Parameters.Append(.CreateParameter("@FiscalEndDate", adDBTimeStamp, adParamInput, , dteFiscalEndDate))
            .Parameters.Append(.CreateParameter("@Deduct", adDecimal, adParamInput, , curDeduct))
            .Parameters.Append(.CreateParameter("@GroupDeduct", adVarChar, adParamInput, 1, strGroupDeduct))
            .Parameters.Append(.CreateParameter("@DeductRem", adDecimal, adParamInput, , curDeductRem))
            .Parameters.Append(.CreateParameter("@DollarLimit", adDecimal, adParamInput, , curDollarLimit))
            .Parameters.Append(.CreateParameter("@DollarsApplied", adDecimal, adParamInput, , curDollarsApplied))
            .Parameters.Append(.CreateParameter("@VisitLimit", adInteger, adParamInput, , intVisitLimit))
            .Parameters.Append(.CreateParameter("@VisitsApplied", adInteger, adParamInput, , intVisitsApplied))
            .Parameters.Append(.CreateParameter("@CatCap", adVarChar, adParamInput, 1, strCatCap))
            .Parameters.Append(.CreateParameter("@CatCapAmt", adDecimal, adParamInput, , curCatCapAmt))
            .Parameters.Append(.CreateParameter("@CertReqd", adVarChar, adParamInput, 1, strCertReqd))
            .Parameters.Append(.CreateParameter("@NoHCFA", adVarChar, adParamInput, 1, strNoHCFA))
            .Parameters.Append(.CreateParameter("@NoInvoice", adVarChar, adParamInput, 1, strNoInvoice))
            .Parameters.Append(.CreateParameter("@OutpatientRpt", adVarChar, adParamInput, 1, strOutpatientRpt))
            .Parameters.Append(.CreateParameter("@VerbalRept", adVarChar, adParamInput, 1, strVerbalRept))
            .Parameters.Append(.CreateParameter("@NumVisit", adInteger, adParamInput, , intNumVisit))
            .Parameters.Append(.CreateParameter("@DaysPrior", adInteger, adParamInput, , intDaysPrior))
            .Parameters.Append(.CreateParameter("@ElectClaim", adVarChar, adParamInput, 1, strElectClaim))
            .Parameters.Append(.CreateParameter("@RecurrCopay", adVarChar, adParamInput, 1, strRecurrCopay))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute() '()
        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
            Exit Function
        End If
        cnnSQL.Close()
        cmdSQL = Nothing
        cnnSQL = Nothing
        'Signal successful completion

    End Function


    Public Function FetchPatRPPlanRule(ByVal lngPatID As Long, _
                                    ByVal lngRPID As Long, _
                                    ByVal lngPlanID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 04/19/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Returns a row from tblPatRPPlanRule with the         '
        '              matching parameters                                  '
        'Parameters: lngPatID - ID of Patient                               '
        '            lngRPID - ID of RP                                     '
        '            lngPlanID - ID of Plan                                 '
        'Returns: ADODB Recordset if match found, otherwise Null            '
        '--------------------------------------------------------------------
        'Note:  Notice how much easier to read this format is?...Eric       '
        '-------------------------------------------------------------------'
        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection





        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        ' Create the parameter objects
        With cmd
            .CommandText = "uspSelPatientRPPlanRule"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatID))
            .Parameters.Append(.CreateParameter("@RPID", adInteger, adParamInput, , lngRPID))
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
        End With

        ' Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchPatRPPlanRule = rst

        ' Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing

        ' Signal successful completion




        'RaiseError(Err, Err.Number, CLASS_NAME)
    End Function

    Public Function FetchPatRPPlanRuleByPatRPPlanID(ByVal lngPatRPPlanID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 01/10/2001                                                   '
        'Author: Morgan Travis                                              '
        'Description:  Returns a row from tblPatRPPlanRule with the         '
        '              matching parameters                                  '
        'Parameters: lngPatRPPlanID - ID of Patient                         '
        '                                                                   '
        'Returns: ADODB Recordset if match found, otherwise Null            '
        '--------------------------------------------------------------------

        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection





        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        ' Create the parameter objects
        With cmd
            .CommandText = "uspSelPatientRPPlanRuleByPatRPPlanID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatRPPlanID", adInteger, adParamInput, , lngPatRPPlanID))
        End With

        ' Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchPatRPPlanRuleByPatRPPlanID = rst

        ' Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing

        ' Signal successful completion


        Exit Function

    End Function

    Public Function UpdatePatientCounters(ByVal lngPatRPPlanID As Long, ByVal intVisitCounter As Integer, ByVal curAmtApplied As Decimal, ByVal curDeductible As Decimal)
        '--------------------------------------------------------------------
        'Date: 01/10/2001
        'Author: Morgan Travis
        'Description:  Updates Patient visit, amount applied, and deductible counters
        'Parameters:
        'Returns:
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim cnn As ADODB.Connection
        Dim cmd As ADODB.Command



        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        'Create the parameter objects
        With cmd
            .CommandText = "uspUpdPatientCounters"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatRPPlanID", adInteger, adParamInput, , lngPatRPPlanID))
            .Parameters.Append(.CreateParameter("@VisitCounter", adInteger, adParamInput, , intVisitCounter))
            .Parameters.Append(.CreateParameter("@AmtApplied", adInteger, adParamInput, , curAmtApplied))
            .Parameters.Append(.CreateParameter("@Deductible", adInteger, adParamInput, , curDeductible))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        cmd.Execute()

        If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Function
        End If

        'Disconnect the recordset
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function

    End Function

End Class


