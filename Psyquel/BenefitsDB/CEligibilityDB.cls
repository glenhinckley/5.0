
'--------------------------------------------------------------------
'Class Name: CEligibilityDB
'Date: 12/20/2023
'Author: DCO
'Description:  MTS object designed to host methods associated with data affecting Eligibility tables.
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



Public Class CEligibilityDB
    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property



    Private Const CLASS_NAME As String = "CEligibilityDB"
    Private Const TABLE_NAME As String = "tblEligibility"


    Public Function FetchByID(ByVal lngID As Long, Optional ByVal strDataBase As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 12/20/2023
        'Author: DCO
        'Description:  Retrieves records from the tblEligibility table utilizing
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
            .CommandText = "uspSelEligibilityByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@EligibilityID", adInteger, adParamInput, , lngID))
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
    Public Function Insert(ByVal strCompanyID As String, ByVal strInsuredFirstName As String, ByVal strInsuredLastName As String, ByVal strInsuredDOB As Date, _
          ByVal lngCPCID As Long, ByVal strPayerCode As String, ByVal strPatientRelation As String, ByVal strProviderFirstName As String, _
          ByVal strProviderLastName As String, ByVal strProviderNPI As String, ByVal strTIN As String, ByVal strTINType As String, _
          ByVal strCardNum As String, ByVal strGroupNum As String, ByVal strPatientLastName As String, ByVal strPatientFirstName As String, _
          ByVal dtePatientDOB As Date, ByVal strPatientSex As String, ByVal strGroupName As String, ByVal strCPT As String, ByVal strSecCPT As String, _
          ByVal strQueuedYN As String, ByVal strUserName As String, Optional ByVal strDataBase As String = "") As Long

        '--------------------------------------------------------------------
        'Date: 12/20/2023
        'Author: DCO
        'Description:  Inserts a row into the tblEligibility table utilizing
        '              a stored procedure.
        'Parameters: All data values to be inserted into the table
        'Returns: ID (Primary Key) of the inserted row.
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        cmdSQL.CommandText = "uspInsEligibility"
        cmdSQL.CommandType = adCmdStoredProc

        With cmdSQL
            .Parameters.Append(.CreateParameter("@EligibilityID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@CompanyID", adVarChar, adParamInput, 30, strCompanyID))
            .Parameters.Append(.CreateParameter("@InsuredFirstName", adVarChar, adParamInput, 60, strInsuredFirstName))
            .Parameters.Append(.CreateParameter("@InsuredLastName", adVarChar, adParamInput, 60, strInsuredLastName))
            .Parameters.Append(.CreateParameter("@InsuredDOB", adDBTimeStamp, adParamInput, , IIf(CLng(strInsuredDOB.ToOADate()) = 0, DBNull.Value, strInsuredDOB)))
            .Parameters.Append(.CreateParameter("@CPCID", adInteger, adParamInput, , lngCPCID))
            .Parameters.Append(.CreateParameter("@PayerCode", adVarChar, adParamInput, 12, strPayerCode))
            .Parameters.Append(.CreateParameter("@PatientRelation", adVarChar, adParamInput, 20, strPatientRelation))
            .Parameters.Append(.CreateParameter("@ProviderFirstName", adVarChar, adParamInput, 60, strProviderFirstName))
            .Parameters.Append(.CreateParameter("@ProviderLastName", adVarChar, adParamInput, 60, strProviderLastName))
            .Parameters.Append(.CreateParameter("@ProviderNPI", adVarChar, adParamInput, 10, strProviderNPI))
            .Parameters.Append(.CreateParameter("@TIN", adVarChar, adParamInput, 30, strTIN))
            .Parameters.Append(.CreateParameter("@TINType", adVarChar, adParamInput, 1, strTINType))
            .Parameters.Append(.CreateParameter("@CardNum", adVarChar, adParamInput, 30, strCardNum))
            .Parameters.Append(.CreateParameter("@GroupNum", adVarChar, adParamInput, 30, strGroupNum))
            .Parameters.Append(.CreateParameter("@PatientLastName", adVarChar, adParamInput, 60, strPatientLastName))
            .Parameters.Append(.CreateParameter("@PatientFirstName", adVarChar, adParamInput, 60, strPatientFirstName))
            .Parameters.Append(.CreateParameter("@PatientDOB", adDBTimeStamp, adParamInput, , IIf(CLng(dtePatientDOB.ToOADate()) = 0, DBNull.Value, dtePatientDOB)))
            .Parameters.Append(.CreateParameter("@PatientSex", adVarChar, adParamInput, 1, strPatientSex))
            .Parameters.Append(.CreateParameter("@GroupName", adVarChar, adParamInput, 60, strGroupName))
            .Parameters.Append(.CreateParameter("@CPT", adVarChar, adParamInput, 5, strCPT))
            .Parameters.Append(.CreateParameter("@SecCPT", adVarChar, adParamInput, 5, strSecCPT))
            .Parameters.Append(.CreateParameter("@QueuedYN", adVarChar, adParamInput, 1, strQueuedYN))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 50, strUserName))
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

        Insert = cmdSQL.Parameters(0).Value

        'Close the connection and free all resources
        cnnSQL.Close()
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        On Error GoTo 0
        cmdSQL.Execute()
        cnnSQL = Nothing
        cmdSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function
    Public Sub Update(ByVal lngEligibilityID As Long, ByVal strCompanyID As String, ByVal strInsuredFirstName As String, ByVal strInsuredLastName As String, ByVal strInsuredDOB As Date, _
          ByVal lngCPCID As Long, ByVal strPayerCode As String, ByVal strPatientRelation As String, ByVal strProviderFirstName As String, _
          ByVal strProviderLastName As String, ByVal strProviderNPI As String, ByVal strTIN As String, ByVal strTINType As String, _
          ByVal strCardNum As String, ByVal strGroupNum As String, ByVal strPatientLastName As String, ByVal strPatientFirstName As String, _
          ByVal dtePatientDOB As Date, ByVal strPatientSex As String, ByVal strGroupName As String, ByVal strCPT As String, ByVal strSecCPT As String, _
          ByVal strQueuedYN As String, ByVal dteDateProcessed As Date, ByVal strUserName As String, Optional ByVal strDataBase As String = "")
        '--------------------------------------------------------------------
        'Date: 12/20/2023
        'Author: DCO
        'Description:  Updates a row into tblEligibility table utilizing
        '              a stored procedure.
        'Parameters:
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        cmdSQL.CommandText = "uspUpdEligibility"
        cmdSQL.CommandType = adCmdStoredProc

        With cmdSQL
            .Parameters.Append(.CreateParameter("@EligibilityID", adInteger, adParamInput, , lngEligibilityID))
            .Parameters.Append(.CreateParameter("@CompanyID", adVarChar, adParamInput, 30, strCompanyID))
            .Parameters.Append(.CreateParameter("@InsuredFirstName", adVarChar, adParamInput, 60, strInsuredFirstName))
            .Parameters.Append(.CreateParameter("@InsuredLastName", adVarChar, adParamInput, 60, strInsuredLastName))
            .Parameters.Append(.CreateParameter("@InsuredDOB", adDBTimeStamp, adParamInput, , IIf(CLng(strInsuredDOB.ToOADate()) = 0, DBNull.Value, strInsuredDOB)))
            .Parameters.Append(.CreateParameter("@CPCID", adInteger, adParamInput, , lngCPCID))
            .Parameters.Append(.CreateParameter("@PayerCode", adVarChar, adParamInput, 12, strPayerCode))
            .Parameters.Append(.CreateParameter("@PatientRelation", adVarChar, adParamInput, 20, strPatientRelation))
            .Parameters.Append(.CreateParameter("@ProviderFirstName", adVarChar, adParamInput, 60, strProviderFirstName))
            .Parameters.Append(.CreateParameter("@ProviderLastName", adVarChar, adParamInput, 60, strProviderLastName))
            .Parameters.Append(.CreateParameter("@ProviderNPI", adVarChar, adParamInput, 10, strProviderNPI))
            .Parameters.Append(.CreateParameter("@TIN", adVarChar, adParamInput, 30, strTIN))
            .Parameters.Append(.CreateParameter("@TINType", adVarChar, adParamInput, 1, strTINType))
            .Parameters.Append(.CreateParameter("@CardNum", adVarChar, adParamInput, 30, strCardNum))
            .Parameters.Append(.CreateParameter("@GroupNum", adVarChar, adParamInput, 30, strGroupNum))
            .Parameters.Append(.CreateParameter("@PatientLastName", adVarChar, adParamInput, 60, strPatientLastName))
            .Parameters.Append(.CreateParameter("@PatientFirstName", adVarChar, adParamInput, 60, strPatientFirstName))
            .Parameters.Append(.CreateParameter("@PatientDOB", adDBTimeStamp, adParamInput, , IIf(CLng(dtePatientDOB.ToOADate()) = 0, DBNull.Value, dtePatientDOB)))
            .Parameters.Append(.CreateParameter("@PatientSex", adVarChar, adParamInput, 1, strPatientSex))
            .Parameters.Append(.CreateParameter("@GroupName", adVarChar, adParamInput, 60, strGroupName))
            .Parameters.Append(.CreateParameter("@CPT", adVarChar, adParamInput, 5, strCPT))
            .Parameters.Append(.CreateParameter("@SecCPT", adVarChar, adParamInput, 5, strSecCPT))
            .Parameters.Append(.CreateParameter("@QueuedYN", adVarChar, adParamInput, 1, strQueuedYN))
            .Parameters.Append(.CreateParameter("@DateProcessed", adDBTimeStamp, adParamInput, , IIf(CLng(dteDateProcessed.ToOADate()) = 0, DBNull.Value, dteDateProcessed)))
            .Parameters.Append(.CreateParameter("@ProcessedBy", adVarChar, adParamInput, 50, strUserName))
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

        'Signal successful completion


        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        On Error GoTo 0
        cmdSQL.Execute()
        cnnSQL = Nothing
        cmdSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

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


    End Function


End Class


