
'-------------------------------------------------------------------------------------
'Date: 01/07/2003
'Class Name: CEDIBz
'Author: Dave Richkun
'Description:   COM object designed to host methods associated with electronic
'               data exchange.
'--------------------------------------------------------------------------------------
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
' Lydia Orth 

Public Class CEDIDB
    Private Const CLASS_NAME As String = "CEDIBz"

    Private _ConnectionString As String = String.Empty
    Private _AppPath As String = String.Empty
    Private _EDIOutput As String = String.Empty
    Private _MachineName As String = String.Empty
    Private _UserLoginName As String = String.Empty
    Private _isDebug As Boolean = False



    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property


    Public WriteOnly Property isDebug() As Boolean
        Set(ByVal value As Boolean)
            '  log.ConnectionString = value
            _isDebug = value

        End Set
    End Property





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

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim cmdSQL As ADODB.Command
        Dim strConnect As String
        Dim strSQL As String



        rstSQL = New ADODB.Recordset
        cmdSQL = New ADODB.Command
        cnnSQL = New ADODB.Connection

        Select Case strDataBase
            Case "Test"
                cnnSQL.Open(_ConnectionString)
            Case "PsyquelDirect"
                cnnSQL.Open(_ConnectionString)
            Case Else
                cnnSQL.Open(_ConnectionString)
        End Select

        cmdSQL.ActiveConnection = cnnSQL

        If _isDebug Then
            strSQL = "SELECT * FROM vwClaim_EDI "
            strSQL = strSQL & strWhere
            strSQL = strSQL & " ORDER BY fldClaimType, fldClearingHouseID, SubmitterID, fldPayerCode, fldInsuranceID, fldInsType, fldProviderLastName, fldProviderFirstName, fldRPID, fldPatientID, fldEncounterLogID "

        Else


            strSQL = "SELECT top 100 * FROM vwClaim_EDI   Where fldQueuedYN = 'N' AND fldClearingHouseID = 92 "
            strSQL = strSQL & " ORDER BY fldClaimType, fldClearingHouseID, SubmitterID, fldPayerCode, fldInsuranceID, fldInsType, fldProviderLastName, fldProviderFirstName, fldRPID, fldPatientID, fldEncounterLogID "

        End If


        cmdSQL.CommandText = strSQL
        cmdSQL.CommandType = adCmdText

        'Execute the stored procedure
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        FetchElectClaims = rstSQL

        'Disconnect the recordset
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        cnnSQL = Nothing



        Exit Function


        'Signal incompletion and raise the error to the ing enviroment.
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()

        'RaiseError(Err, Err.Number, CLASS_NAME)
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




        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelNextEFile"
            .CommandType = adCmdStoredProc
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        GetNextFileNumber = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing



        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        cmd = Nothing
        rst = Nothing
        cnn = Nothing

        'cmdSQL.Execute()
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function


    Public Sub Insert(ByVal lngFileNumber As Long, ByVal strFileName As String, _
                           ByVal lngStartTxNum As Long, ByVal lngEndTxNum As Long, _
                           ByVal strUserName As String)
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

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspInsEFile"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@EFileID", adInteger, adParamInput, , lngFileNumber))
            .Parameters.Append(.CreateParameter("@FileName", adVarChar, adParamInput, 20, strFileName))
            .Parameters.Append(.CreateParameter("@StartTxNum", adInteger, adParamInput, , lngStartTxNum))
            .Parameters.Append(.CreateParameter("@EndTxNum", adInteger, adParamInput, , lngEndTxNum))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection

        cnnSQL.Open(_ConnectionString)
        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection
        cnnSQL.Close()

        'Free all resources
        cmdSQL = Nothing
        cnnSQL = Nothing


        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL = Nothing
        cnnSQL = Nothing

        cmdSQL.Execute()
        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME)

    End Sub

    Public Sub EstablishXRef(ByVal lngClaimID As Long, ByVal lngEncounterLogID As Long, _
                             ByVal lngEFileID As Long, ByVal strEFileName As String, Optional ByVal strDataBase As String = "")
        '--------------------------------------------------------------------------------------
        'Date: 01/21/2003
        'Author: Dave Richkun
        'Description: Establishes a cross-reference between a Psyquel claim and an electroniy
        '             submitted claim.
        'Parameters: lngClaimID - ID of the row in tblClaim that will be updated
        '            lngEncounterLogID - ID of the encounter log in tblClaim that will be updated
        '            lngEFileID - ID of the number assigned to the electronic claim
        '            strEFileName - The name of the file containing the electronic claim
        'Returns: Null
        '--------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspUpdClaimXRef"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ClaimID", adInteger, adParamInput, , lngClaimID))
            .Parameters.Append(.CreateParameter("@ELID", adInteger, adParamInput, , lngEncounterLogID))
            .Parameters.Append(.CreateParameter("@EFileID", adInteger, adParamInput, , lngEFileID))
            .Parameters.Append(.CreateParameter("@FileName", adVarChar, adParamInput, 30, strEFileName))
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
        cmdSQL.Execute()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection
        cnnSQL.Close()

        'Free all resources
        cmdSQL = Nothing
        cnnSQL = Nothing


        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL = Nothing
        cnnSQL = Nothing

        cmdSQL.Execute()
        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME)

    End Sub


    Public Sub UpdateQueuedStatus(ByVal strEFileName As String)
        '--------------------------------------------------------------------------------------
        'Date: 02/25/2003
        'Author: Dave Richkun
        'Description: Updates the Queued status in tblClaim for all claims submitted under a
        '             particular file name.
        'Parameters: lngEFileID - ID of the file under which claims have been electroniy submitted
        '            strEFileName - The name of the file containing the electronic claims whose Queued
        '               status will be updated
        'Returns: Null
        '--------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspUpdClaimQueued"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@FileName", adVarChar, adParamInput, 30, strEFileName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection

        cnnSQL.Open(_ConnectionString)
        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection
        cnnSQL.Close()

        'Free all resources
        cmdSQL = Nothing
        cnnSQL = Nothing


        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL = Nothing
        cnnSQL = Nothing

        cmdSQL.Execute()
        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME)

    End Sub

    Public Sub UpdateACK(ByVal lngEFileID As Long, _
                         ByVal lngStartTxNum As Long, ByVal lngEndTxNum As Long, _
                         ByVal strAK1 As String, ByVal strAK2 As String, _
                         ByVal strAKDetail As String, _
                         ByVal strAK5 As String, ByVal strAK9 As String)
        '--------------------------------------------------------------------------------------
        'Date: 11/01/2007
        'Author: Duane C Orth
        'Description: Updates the Acknoledegement fields in tblEFile for all claims submitted under a
        '             particular file id.
        'Parameters: lngEFileID - ID of the file under which claims have been electroniy submitted
        '            lngStartTxNum - Starting transaction number within the file
        '            lngEndTxNum - Ending transaction number within the file
        'Returns: Null
        '--------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspUpdAck"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@EFileID", adInteger, adParamInput, , lngEFileID))
            .Parameters.Append(.CreateParameter("@StartTxNum", adInteger, adParamInput, , lngStartTxNum))
            .Parameters.Append(.CreateParameter("@EndTxNum", adInteger, adParamInput, , lngEndTxNum))
            .Parameters.Append(.CreateParameter("@Ak1", adVarChar, adParamInput, 20, strAK1))
            .Parameters.Append(.CreateParameter("@Ak2", adVarChar, adParamInput, 20, strAK2))
            .Parameters.Append(.CreateParameter("@AkDetail", adVarChar, adParamInput, 512, strAKDetail))
            .Parameters.Append(.CreateParameter("@Ak5", adVarChar, adParamInput, 20, strAK5))
            .Parameters.Append(.CreateParameter("@Ak9", adVarChar, adParamInput, 20, strAK9))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection

        cnnSQL.Open(_ConnectionString)
        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection
        cnnSQL.Close()

        'Free all resources
        cmdSQL = Nothing
        cnnSQL = Nothing


        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL = Nothing
        cnnSQL = Nothing

        cmdSQL.Execute()
        'RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME)

    End Sub


End Class
