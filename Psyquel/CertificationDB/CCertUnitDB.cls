
'--------------------------------------------------------------------
'Class Name: CCertUnitDB                                            '
'Date: 03/08/2000                                                   '
'Author: Rick "Boom Boom" Segura                                    '
'Description:  MTS object designed to host methods associated with  '
'              data affecting the tblCertUnit table.                '
'--------------------------------------------------------------------
' Revision History:
'   R001: 10/30/2000 Richkun - Added Update(), Delete() methods.
'   R002: 05/10/2002 Richkun - Added support for fractional unit values
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

'---------------------------


Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CCertUnitDB

    Private Const CLASS_NAME As String = "CCertUnitDB"
    Private Const TABLE_NAME As String = "tblCertUnit"

    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property


    Public Function Insert(ByVal lngAssignedUnits As Double, _
                  Optional ByVal lngRemainingUnits As Double = Nothing) As Long
        '--------------------------------------------------------------------
        'Date: 03/08/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Inserts a single record into the tblCertCPTCode      '
        '               table                                               '
        'Parameters: lngCertID - ID of cert to link CPT Code with           '
        '            strCPTCode - CPT Code to associate with cert           '
        '            lngCertUnitID - Cert Unit to associate with pair       '
        'Returns: ID of record just inserted(on success)                    '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection
        Dim lngErrNum As Long
        Dim strErrMsg As String



        cnn = New ADODB.Connection
        cmd = New ADODB.Command

        Call cnn.Open(_ConnectionString)

        'Create the parameter objects
        With cmd
            .CommandText = "uspInsCertUnit"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@AssignedUnits", adDouble, adParamInput, , lngAssignedUnits))
            .Parameters.Append(.CreateParameter("@RemainingUnits", adDouble, adParamInput, , lngRemainingUnits))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        cmd.ActiveConnection = cnn
        cmd.Execute() ', , adExecuteNoRecords

        Insert = cmd.Parameters("@ID").Value

        'Check the ErrorNum parameter before deciding to commit the transaction
        If (cmd.Parameters("@ErrorNum").Value).IsNullorEmpty Then
            lngErrNum = vbObjectError
            strErrMsg = "Procedure uspInsCertUnit failed."
            Exit Function
        End If

        If cmd.Parameters("@ErrorNum").Value <> 0 Then
            Exit Function
        End If

        cmd = Nothing
        cnn = Nothing


    End Function

    Public Function LogUnitUsage(ByVal lngCertID As Long, ByVal strCPTCode As String, _
                    ByVal dblUnits As Double, ByVal dtDOS As Date, ByVal lngProviderID As Long, _
                    ByVal lngELID As Long)
        '--------------------------------------------------------------------
        'Date: 07/10/2002
        'Author: Dave Richkun
        'Description:  Records certification usage history in tblCertUsage
        'Parameters:  lngCertID - ID of certification whose units were applied
        '             strCPTCode - CPT Code associated with unit usage
        '             dblUnits - The units applied at the time of service
        '             dtDOS - Date service was provided by provider
        '             lngProviderID - ID of provider offering service to patient
        '             lngELID - ID of Encounter associated with certification unit consumption
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        cnn = New ADODB.Connection
        cmd = New ADODB.Command

        Call cnn.Open(_ConnectionString)

        'Create the parameter objects
        With cmd
            .CommandText = "uspInsCertUsage"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@CertID", adInteger, adParamInput, , lngCertID))
            .Parameters.Append(.CreateParameter("@CPTCode", adVarChar, adParamInput, 10, strCPTCode))
            .Parameters.Append(.CreateParameter("@Units", adDouble, adParamInput, , dblUnits))
            .Parameters.Append(.CreateParameter("@DOS", adDBTimeStamp, adParamInput, , dtDOS))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@ELID", adInteger, adParamInput, , lngELID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        cmd.ActiveConnection = cnn
        cmd.Execute()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Function

        End If

        cmd = Nothing
        cnn = Nothing


    End Function

    Public Function Update(ByVal lngCertUnitID As Long, ByVal lngAssignedUnits As Double, _
                           ByVal strUserName As String) As Long
        '--------------------------------------------------------------------
        'Date: 10/30/2000                                                   '
        'Author: Dave Richkun
        'Description:  Updates a single record into the tblCertCPTUnit table                                               '
        'Parameters: lngCertUnitID - ID of cert to update
        '            lngAssignedUnits - Number of units assigned to the certification
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection


        cnn = New ADODB.Connection
        cmd = New ADODB.Command

        'Create the parameter objects
        With cmd
            .CommandText = "uspUpdCertUnit"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@CertUnitID", adInteger, adParamInput, , lngCertUnitID))
            .Parameters.Append(.CreateParameter("@AssignedUnits", adDouble, adParamInput, , lngAssignedUnits))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Open the database connection.
        Call cnn.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmd.ActiveConnection = cnn
        cmd.Execute() ', , adExecuteNoRecords

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Function
        End If

        cmd = Nothing
        cnn = Nothing


    End Function


    Public Function Delete(ByVal lngCertUnitID As Long) As Long
        '--------------------------------------------------------------------
        'Date: 10/30/2000                                                   '
        'Author: Dave Richkun
        'Description:  Deletes rows from tblCertCPTCode and tblCertUnit where
        '              lngCertUnitID is referenced
        'Parameters: lngCertUnitID - ID of CertUnitID to delete
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        cnn = New ADODB.Connection
        cmd = New ADODB.Command

        'Create the parameter objects
        With cmd
            .CommandText = "uspDelCertUnit"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@CertUnitID", adInteger, adParamInput, , lngCertUnitID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Open the database connection.
        Call cnn.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmd.ActiveConnection = cnn
        cmd.Execute() ', , adExecuteNoRecords

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Function
        End If

        cmd = Nothing
        cnn = Nothing


    End Function

    Public Sub RollBackUnit(ByVal lngCertUnitID As Long, ByVal lngCertUsageID As Long, _
                            ByVal dblUnits As Double)
        '--------------------------------------------------------------------
        'Date: 05/16/2001                                                   '
        'Author: Rick "Boom boom" Segura                                    '
        'Description:  Increments the "Units Remaining" field by 1 unit and '
        '               deletes the corresponding row in tblCertUsage
        'Parameters: lngCertUnitID - ID of CertUnitID to rollback
        '            lngCertUsageID - ID of row in tblCertUsage to Delete
        '            dblUnits - The number of units to rollback
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        ' 'R099
        '--------------------------------------------------------------------

        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection


        cnn = New ADODB.Connection
        cmd = New ADODB.Command

        'Create the parameter objects
        With cmd
            .CommandText = "uspUpdCertUnitRollBack"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@CertUnitID", adInteger, adParamInput, , lngCertUnitID))
            .Parameters.Append(.CreateParameter("@CertUsageID", adInteger, adParamInput, , lngCertUsageID))
            .Parameters.Append(.CreateParameter("@Units", adDouble, adParamInput, , dblUnits))
        End With

        'Open the database connection.
        Call cnn.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmd.ActiveConnection = cnn
        cmd.Execute(, , adExecuteNoRecords)

        cmd = Nothing
        cnn = Nothing


    End Sub

    Public Function FetchRollBackUnits(ByVal lngELID As Long, ByVal lngRPID As Long, ByVal lngPlanID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 05/16/2001                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Retrieves recordset of certs units associated with a '
        '               Bill to be rolled back                              '
        'Parameters: lngBillID - Bill asssociated with certs to roll back   '
        'Returns: Recordstets of detailed information                       '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection
        Dim rst As ADODB.Recordset


        cnn = New ADODB.Connection
        cmd = New ADODB.Command
        rst = New ADODB.Recordset

        Call cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelCertUnitRollBack"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@BillID", adInteger, adParamInput, , lngELID))
            .Parameters.Append(.CreateParameter("@RPID", adInteger, adParamInput, , lngRPID))
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
        End With

        '  Instantiate recordset and populate
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchRollBackUnits = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing


    End Function

    Public Function FetchRollBackDetUnits(ByVal lngEncDetailID As Long, ByVal lngRPID As Long, ByVal lngPlanID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 05/16/2001                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Retrieves recordset of certs units associated with a '
        '               Bill to be rolled back                              '
        'Parameters: lngBillID - Bill asssociated with certs to roll back   '
        'Returns: Recordstets of detailed information                       '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection
        Dim rst As ADODB.Recordset



        cnn = New ADODB.Connection
        cmd = New ADODB.Command
        rst = New ADODB.Recordset

        Call cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelCertDetUnitRollBack"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@EncDetID", adInteger, adParamInput, , lngEncDetailID))
            .Parameters.Append(.CreateParameter("@RPID", adInteger, adParamInput, , lngRPID))
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
        End With

        '  Instantiate recordset and populate
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchRollBackDetUnits = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing



    End Function

    Public Sub AssignCert(ByVal lngELID As Long, ByVal lngPlanID As Long, ByVal strCertNum As String)
        '--------------------------------------------------------------------
        'Date: 08/10/2002
        'Author: Dave Richkun
        'Description:  Updates the Certification Number in tblBillingResponsibility
        '              for a given encounter/plan ID combination.  This method is part
        '              of the certification 'Seek And Assign' functionality.
        'Parameters: lngELID - ID of the encounter log for which the cert number is updated
        '            lngPlanID - ID of the plan associated with the certification
        '            strCertNum - The certification number
        'Returns: Null
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command


        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdBRCertNum"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ELID", adInteger, adParamInput, , lngELID))
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
            .Parameters.Append(.CreateParameter("@CertNum", adVarChar, adParamInput, 30, strCertNum))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        Call cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute(, , adExecuteNoRecords)

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub

        End If

        'Close the connection and free all resources
        cnnSQL.Close()

        cmdSQL = Nothing
        cnnSQL = Nothing



    End Sub

End Class