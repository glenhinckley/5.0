
'--------------------------------------------------------------------
'Class Name: CUserRoleDB                                            '
'Date: 01/04/2000                                                   '
'Author: Dave Richkun                                               '
'Description:  MTS object designed to allow insertions into the     '
'              tblUserRole table.                                   '
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

Public Class CUserRoleDB

    Private _ConnectionString As String = String.Empty

    Private Const TABLE_NAME As String = "tblUserRole"
    Private Const CLASS_NAME As String = "CUserRoleIns"


    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property



    Public Sub Insert(ByVal lngUserID As Long, ByVal varRoles As Object)
        '--------------------------------------------------------------------
        'Date: 01/04/2000                                                   '
        'Author: Dave Richkun                                               '
        'Description:  Inserts one or more rows into the tblUserRole table. '
        'Parameters: lngUserID - ID of the user who is being associated to  '
        '               one or more Roles                                   '
        '            varRoles - Object array containing the IDs of each    '
        '               Role to which the User is associated.               '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------

        Dim cnn As New ADODB.Connection
        Dim cmd As New ADODB.Command
        Dim lngRoleID As Long
        Dim intCTR As Integer


        'Acquire the database connection.


        cnn.Open(_ConnectionString)

        'Delete existing UserClinic rows before adding new rows
        With cmd
            .CommandText = "uspDelUserRole"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
        End With

        'Instantiate and prepare the Command object.
        cmd.ActiveConnection = cnn
        cmd.Execute() ', adExecuteNoRecords
        cmd = Nothing

        If UBound(varRoles) >= 0 Then
            While intCTR <= UBound(varRoles)

                lngRoleID = varRoles(intCTR)

                With cmd
                    .CommandText = "uspInsUserRole"
                    .CommandType = adCmdStoredProc
                    .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
                    .Parameters.Append(.CreateParameter("@RoleID", adInteger, adParamInput, , lngRoleID))
                    .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
                End With

                'Assign the connection to the Command object and execute the stored procedure
                cmd.ActiveConnection = cnn
                cmd.Execute() ', adExecuteNoRecords



                intCTR = intCTR + 1
            End While

            'Close the connection and free all resources
            cnn.Close()
        End If

EMPTY_ARRAY:
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion

    End Sub

    Public Function FetchRolesByUserID(ByVal lngUserID As Long) _
                        As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/29/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Returns a Object array containing a list of all     '
        '              roles to which a user is associated.                 '
        'Parameters: lngUserID - The ID of the user                         '
        'Returns: A Object array containing a list of all roles to which   '
        '         a user is associated.                                     '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        ' 03/02/2000 Segura:  Implemented uspSelUserRole                    '
        '--------------------------------------------------------------------

        Dim cmd As New ADODB.Command
        Dim cnn As New ADODB.Connection
        Dim rst As ADODB.Recordset
        Dim strList As String
        Dim intCTR As Integer





        rst = New ADODB.Recordset

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        ' Prepare stored procedure parameters
        With cmd
            .CommandText = "uspSelUserRoles"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
        End With

        'Instantiate and populate the Recordset.
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rst.ActiveConnection = Nothing
        cnn.Close()
        cnn = Nothing

        FetchRolesByUserID = rst

        cmd = Nothing


    End Function

    Public Function FetchRolesByUserName(ByVal strUserName As String) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/14/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Returns a recordset containing a list of all roles   '
        '              to which a user is associated.                       '
        'Parameters: strUserName - The network login name of the user       '
        'Returns: A recordset containing a list of all roles   to which a   '
        '           user is associated.                                     '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim strSQL As String
        Dim intCTR As Integer


        'Prepare the SQL statement.
        strSQL = "SELECT "
        strSQL = strSQL & "tblRole.fldRoleID, "
        strSQL = strSQL & "tblRole.fldRoleName "
        strSQL = strSQL & "FROM "
        strSQL = strSQL & "tblRole,  "
        strSQL = strSQL & "tblUserRole,  "
        strSQL = strSQL & "tblUser  "
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & "tblRole.fldRoleID = tblUserRole.fldRoleID AND  "
        strSQL = strSQL & "tblUserRole.fldUserID = tblUser.fldUserID AND  "
        strSQL = strSQL & "tblUser.fldUserName = '" & strUserName & "'"

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Instantiate and populate the Recordset.
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(strSQL, cnnSQL, adOpenForwardOnly, adLockReadOnly, adCmdText + adAsyncFetch)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing

        FetchRolesByUserName = rstSQL

        '        'Signal successful completion
        '        

        '        Exit Function

        '
        '        'Signal incompletion and raise the error to the ing environment.
        '         cmdSQL.Execute()
        '        FetchRolesByUserName = Nothing
        '       'err.raise(Err.Number, Err.Source, Err.Description)

    End Function
End Class
