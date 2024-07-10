'------------------------------------
'Class Name: CPatBenVerDB
'Date: 10/26/2000
'Author: Morgan Travis
'Description:  MTS object designed to host methods associated with
'               data affecting Patient Benefits Verification table.
'--------------------------------------------------------------------
'--------------------------------------------------------------------
Option Explicit on

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

Public Class CPatBEnVerDB

    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CPatBenVerDB"
    Private Const TABLE_NAME As String = "tblPatientBenefitVerification"

    Public Function Insert(ByVal lngPatRPPlanID As Long, _
                           ByVal strUser As String, ByVal dteDateTime As Date, _
                           ByVal strContact As String, ByVal strNotes As String) As Long
        '--------------------------------------------------------------------
        'Date: 10/26/2000
        'Author: Morgan Travis
        'Description:  Inserts a row into the tblPatientBenefitVerfication table utilizing
        '              a stored procedure.
        'Parameters: lngPatRPPlanID - Patient Resposible Party Plan ID number to insert into table
        '            strUser - System User that will be inserted into the table.
        '            dteDateTime - The date/time of the entry that will be inserted into the table.
        '            strContact - The Name of the person contacted about the verification
        '            strNotes - Notes about the verification
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        cmdSQL.CommandText = "uspInsPatBenefitVerification"
        cmdSQL.CommandType = adCmdStoredProc

        With cmdSQL
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@PatRPPlanID", adInteger, adParamInput, , lngPatRPPlanID))
            .Parameters.Append(.CreateParameter("@User", adVarChar, adParamInput, 30, strUser))
            .Parameters.Append(.CreateParameter("@DateTime", adDBTimeStamp, adParamInput, , dteDateTime))
            .Parameters.Append(.CreateParameter("@Contact", adVarChar, adParamInput, 30, strContact))
            .Parameters.Append(.CreateParameter("@Notes", adVarChar, adParamInput, 256, strNotes))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

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



    Public Function FetchByPatRPPlan(ByVal lngPatRPPlanID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/26/2000                                                   '
        'Author: Morgan Travis                                              '
        'Description:  Retrieves records from the tblPatientBenefitVerfication table.     '
        'Parameters: lngProviderID - Provider ID number to fetch from table '
        '
        'Returns: Recordset of requested providers
        '-------------------------------------------------------------------'


        Dim cnn As ADODB.Connection
        Dim cmd As ADODB.Command
        Dim rst As ADODB.Recordset

        Dim SQLErrorNum As Long




        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelBenefitVerificationByPatRPPlanID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatRPPlan", adInteger, adParamInput, , lngPatRPPlanID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByPatRPPlan = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        cnn = Nothing
        cmd = Nothing
        rst = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function

    Public Function FetchPendingVerif(ByVal lngUserID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/26/2005                                                   '
        'Author: Duane C Orth                                              '
        'Description:  Retrieves records from the tblPatientBenefitVerfication table.     '
        'Parameters: lngUserID - Provider ID number to fetch from table '
        '
        'Returns: Recordset of requested providers
        '-------------------------------------------------------------------'

        Dim cnn As ADODB.Connection
        Dim cmd As ADODB.Command
        Dim rst As ADODB.Recordset

        Dim SQLErrorNum As Long



        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelPatientsToBeVerifiedByUserID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchPendingVerif = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        cnn = Nothing
        cmd = Nothing
        rst = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function

End Class
