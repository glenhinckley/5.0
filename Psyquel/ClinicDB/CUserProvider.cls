
'--------------------------------------------------------------------
'Class Name: CUserProvider
'Date: 09/27/2000                                                   '
'Author: Dave Richkun                                               '
'Description:  MTS object designed to allow insertions into the     '
'              tblUserProvider table.                                 '
'--------------------------------------------------------------------
'Revision History:                                                  '
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


Public Class CUserProviderDB


    Private _ConnectionString As String = String.Empty




    Private Const TABLE_NAME As String = "tblUserProvider"
    Private Const CLASS_NAME As String = "CUserProvider"


    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property





    Public Sub Insert(ByVal lngUserID As Long, ByVal lngProviderID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 09/27/2000                                                   '
        'Author: Dave Richkun                                               '
        'Description:  Inserts one or more rows into the tblUserProvider table.                                              '
        'Parameters: lngUserID - ID of the user who is being associated with
        '               one or more providers
        '            lngProviderID - ID of the Provider being associated with a
        '               user
        '            strUserName - Name of user adding record to database
        'Returns: Null                                                      '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim strSQL As String
        Dim intCtr As Integer


        cmdSQL = New ADODB.Command
        cmdSQL.CommandType = adCmdStoredProc

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        With cmdSQL
            .CommandText = "uspInsUserProvider"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute() ', adExecuteNoRecords

        'Close the connection and free all resources
        cnnSQL.Close()

        cmdSQL = Nothing
        cnnSQL = Nothing


    End Sub


    Public Sub DeleteUserProviders(ByVal lngUserID As Long)
        '--------------------------------------------------------------------
        'Date: 09/27/2000                                                   '
        'Author: Dave Richkun                                               '
        'Description:  Deletes all Provider associations for a user
        'Parameters: lngUserID - ID of the user who's provider associatons are
        '               being deleted
        'Returns: Null                                                      '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        cmdSQL = New ADODB.Command
        cmdSQL.CommandType = adCmdStoredProc

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        With cmdSQL
            .CommandText = "uspDelUserProviders"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Close the connection and free all resources
        cnnSQL.Close()

        cmdSQL = Nothing
        cnnSQL = Nothing



    End Sub

End Class
