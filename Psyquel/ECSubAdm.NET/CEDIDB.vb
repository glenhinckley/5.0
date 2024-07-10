Option Strict Off
Option Explicit On
Public Class CEDIDB
    '-------------------------------------------------------------------------------------
    'Date: 01/07/2003
    'Class Name: CEDIBz
    'Author: Dave Richkun
    'Description:   COM object designed to host methods associated with electronic
    '               data exchange.
    '--------------------------------------------------------------------------------------

    Private Const CLASS_NAME As String = "CEDIBz"

    Public Function FetchElectClaims(ByVal strWhere As String, Optional ByVal strDataBase As String = "") As ADODB.Recordset

        '--------------------------------------------------------------------------------------
        'Date: 10/10/2002
        'Author: Dave Richkun
        'Description: Retrieves records from tblClaim matching the criteria specified in
        '             the strWhere parametetr.
        'Parameters: strWhere - String portion of SQL 'where' clause that identifies the
        '              claim selection criteria
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------------------------

        Dim cnnSQL As New ADODB.Connection
        Dim rstSQL As New ADODB.Recordset
        Dim cmdSQL As New ADODB.Command
        Dim strConnect As String
        Dim strSQL As String

 
        Select Case strDataBase
            Case "Test"
                cnnSQL.Open((CONST_TEST_CNN))
            Case "PsyquelDirect"
                cnnSQL.Open((CONST_DIRECT_CNN))
            Case Else
                cnnSQL.Open((CONST_PSYQUEL_CNN))
        End Select

        cmdSQL.ActiveConnection = cnnSQL

        'Create the parameter objects
        With cmdSQL
            strSQL = "SELECT * FROM vwClaim_EDI "
            strSQL = strSQL & strWhere
            strSQL = strSQL & " ORDER BY fldClaimType, fldClearingHouseID, SubmitterID, fldPayerCode, fldInsuranceID, fldInsType, fldProviderLastName, fldProviderFirstName, fldRPID, fldPatientID, fldEncounterLogID "
        End With

        cmdSQL.CommandText = strSQL
        cmdSQL.CommandType = ADODB.CommandTypeEnum.adCmdText

        'Execute the stored procedure
        rstSQL.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        rstSQL.Open(cmdSQL, , ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)

        FetchElectClaims = rstSQL

        'Disconnect the recordset
        'UPGRADE_NOTE: Object cmdSQL.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL.ActiveConnection = Nothing
        'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL = Nothing
        'UPGRADE_NOTE: Object rstSQL.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        rstSQL.ActiveConnection = Nothing
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing


    End Function

    Public Function GetNextFileNumber() As ADODB.Recordset

        '--------------------------------------------------------------------------------------
        'Date: 12/06/2003
        'Author: Dave Richkun
        'Description: Returns the file number and the starting transaction (BHT) number
        '             for a new E-claim file.
        'Parameters: None
        'Returns: ADO Recordset containing new file number and starting transaction number.
        '--------------------------------------------------------------------------------------

        Dim rst As New ADODB.Recordset
        Dim cmd As New ADODB.Command
        Dim cnn As New ADODB.Connection



        cnn.Open((CONST_PSYQUEL_CNN))
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelNextEFile"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
        End With

        'Execute the stored procedure
        rst.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        rst.Open(cmd, , ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)

        GetNextFileNumber = rst

        'Disconnect the recordset
        'UPGRADE_NOTE: Object cmd.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmd.ActiveConnection = Nothing
        'UPGRADE_NOTE: Object cmd may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmd = Nothing
        'UPGRADE_NOTE: Object rst.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        rst.ActiveConnection = Nothing
        'UPGRADE_NOTE: Object cnn may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnn = Nothing



    End Function


    Public Sub Insert(ByVal lngFileNumber As Integer, ByVal strFileName As String, ByVal lngStartTxNum As Integer, ByVal lngEndTxNum As Integer, ByVal strUserName As String)
        Dim GetObjectContext As Object
        '--------------------------------------------------------------------------------------
        'Date: 12/07/2003
        'Author: Dave Richkun
        'Description: Inserts a row into tblEFile via a stored procedure
        'Parameters: lngFileNumber - ID of the file
        '            lngFileName - The name of the file
        '            lngStartTxNum - Starting transaction number within the file
        '            lngEndTxNum - Ending transaction number within the file
        '            strUserName - Name of the user running the procedure
        'Returns: Null
        '--------------------------------------------------------------------------------------

        Dim cnnSQL As New ADODB.Connection
        Dim cmdSQL As New ADODB.Command

 

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspInsEFile"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter("@EFileID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , lngFileNumber))
            .Parameters.Append(.CreateParameter("@FileName", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 20, strFileName))
            .Parameters.Append(.CreateParameter("@StartTxNum", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , lngStartTxNum))
            .Parameters.Append(.CreateParameter("@EndTxNum", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , lngEndTxNum))
            .Parameters.Append(.CreateParameter("@AddedBy", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@ErrorNum", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection

        Call cnnSQL.Open(CONST_PSYQUEL_CNN)
        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute(, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
            GoTo ErrTrap
        End If

        'Close the connection
        cnnSQL.Close()

        'Free all resources
        'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL = Nothing
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing

        'UPGRADE_WARNING: Couldn't resolve default property of object GetObjectContext.SetComplete. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'

        Exit Sub

ErrTrap:
        'Signal incompletion and raise the error to the calling environment.
        'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL = Nothing
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing

        'UPGRADE_WARNING: Couldn't resolve default property of object GetObjectContext.SetAbort. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'

        Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME)

    End Sub

    Public Sub EstablishXRef(ByVal lngClaimID As Integer, ByVal lngEncounterLogID As Integer, ByVal lngEFileID As Integer, ByVal strEFileName As String, Optional ByVal strDataBase As String = "")

        '--------------------------------------------------------------------------------------
        'Date: 01/21/2003
        'Author: Dave Richkun
        'Description: Establishes a cross-reference between a Psyquel claim and an electronically
        '             submitted claim.
        'Parameters: lngClaimID - ID of the row in tblClaim that will be updated
        '            lngEncounterLogID - ID of the encounter log in tblClaim that will be updated
        '            lngEFileID - ID of the number assigned to the electronic claim
        '            strEFileName - The name of the file containing the electronic claim
        'Returns: Null
        '--------------------------------------------------------------------------------------

        Dim cnnSQL As New ADODB.Connection
        Dim cmdSQL As New ADODB.Command

   
        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspUpdClaimXRef"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ClaimID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , lngClaimID))
            .Parameters.Append(.CreateParameter("@ELID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , lngEncounterLogID))
            .Parameters.Append(.CreateParameter("@EFileID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , lngEFileID))
            .Parameters.Append(.CreateParameter("@FileName", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 30, strEFileName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamOutput, , 0))
        End With



        Select Case strDataBase
            Case "Test"
                cnnSQL.Open((CONST_TEST_CNN))
            Case "PsyquelDirect"
                cnnSQL.Open((CONST_DIRECT_CNN))
            Case Else
                cnnSQL.Open((CONST_PSYQUEL_CNN))
        End Select

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute(, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            GoTo ErrTrap
        End If

        'Close the connection
        cnnSQL.Close()

        'Free all resources
        'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL = Nothing
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing


        Exit Sub

ErrTrap:
        'Signal incompletion and raise the error to the calling environment.
        'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL = Nothing
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing

  
        Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME)

    End Sub


    Public Sub UpdateQueuedStatus(ByVal strEFileName As String)
        Dim GetObjectContext As Object
        '--------------------------------------------------------------------------------------
        'Date: 02/25/2003
        'Author: Dave Richkun
        'Description: Updates the Queued status in tblClaim for all claims submitted under a
        '             particular file name.
        'Parameters: lngEFileID - ID of the file under which claims have been electronically submitted
        '            strEFileName - The name of the file containing the electronic claims whose Queued
        '               status will be updated
        'Returns: Null
        '--------------------------------------------------------------------------------------

        Dim cnnSQL As New ADODB.Connection
        Dim cmdSQL As New ADODB.Command



        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspUpdClaimQueued"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter("@FileName", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 30, strEFileName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection

        Call cnnSQL.Open(CONST_PSYQUEL_CNN)
        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute(, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            GoTo ErrTrap
        End If

        'Close the connection
        cnnSQL.Close()

        'Free all resources
        'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL = Nothing
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing


        Exit Sub

ErrTrap:
        'Signal incompletion and raise the error to the calling environment.
        'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL = Nothing
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing


        Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME)

    End Sub

    Public Sub UpdateACK(ByVal lngEFileID As Integer, ByVal lngStartTxNum As Integer, ByVal lngEndTxNum As Integer, ByVal strAK1 As String, ByVal strAK2 As String, ByVal strAKDetail As String, ByVal strAK5 As String, ByVal strAK9 As String)
        Dim GetObjectContext As Object
        '--------------------------------------------------------------------------------------
        'Date: 11/01/2007
        'Author: Duane C Orth
        'Description: Updates the Acknoledegement fields in tblEFile for all claims submitted under a
        '             particular file id.
        'Parameters: lngEFileID - ID of the file under which claims have been electronically submitted
        '            lngStartTxNum - Starting transaction number within the file
        '            lngEndTxNum - Ending transaction number within the file
        'Returns: Null
        '--------------------------------------------------------------------------------------

        Dim cnnSQL As New ADODB.Connection
        Dim cmdSQL As New ADODB.Command



        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspUpdAck"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter("@EFileID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , lngEFileID))
            .Parameters.Append(.CreateParameter("@StartTxNum", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , lngStartTxNum))
            .Parameters.Append(.CreateParameter("@EndTxNum", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , lngEndTxNum))
            .Parameters.Append(.CreateParameter("@Ak1", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 20, strAK1))
            .Parameters.Append(.CreateParameter("@Ak2", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 20, strAK2))
            .Parameters.Append(.CreateParameter("@AkDetail", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 512, strAKDetail))
            .Parameters.Append(.CreateParameter("@Ak5", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 20, strAK5))
            .Parameters.Append(.CreateParameter("@Ak9", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 20, strAK9))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection

        Call cnnSQL.Open(CONST_PSYQUEL_CNN)
        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute(, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            GoTo ErrTrap
        End If

        'Close the connection
        cnnSQL.Close()

        'Free all resources
        'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL = Nothing
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing

     
        Exit Sub

ErrTrap:
        'Signal incompletion and raise the error to the calling environment.
        'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL = Nothing
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing


        Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME)

    End Sub
End Class