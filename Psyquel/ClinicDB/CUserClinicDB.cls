
'--------------------------------------------------------------------
'Class Name: CUserClinicIns                                         '
'Date: 01/04/2000                                                   '
'Author: Dave Richkun                                               '
'Description:  MTS object designed to allow insertions into the     '
'              tblUserClinic table.                                 '
'--------------------------------------------------------------------
'Revision History:                                                  '
'   R001 Richkun 02/03/2000 - Added FetchClinics method             '
'   R002 Segura 02/28/2000 - Removed Update method; not required    '
'--------------------------------------------------------------------

Option Explicit  On




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

Public Class CUserClinicDB

    Private _ConnectionString As String = String.Empty

    Private Const TABLE_NAME As String = "tblUserClinic"
    Private Const CLASS_NAME As String = "CUserClinicIns"

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property






    Public Function FetchClinicsByUserID(ByVal lngUserID As Long) As ADODB.Recordset  'R001
        '--------------------------------------------------------------------
        'Date: 01/10/2000                                                   '
        'Author: Dave Richkun                                               '
        'Description:  Returns a Object array containing a list of all     '
        '              clinics to which a user is associated.               '
        'Parameters: lngUserID - The ID of the user                         '
        'Returns: A Object array containing a list of all clinics to which '
        '         a user is associated.                                     '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cmd As New ADODB.Command
        Dim cnn As New ADODB.Connection
        Dim rst As ADODB.Recordset
        Dim str As String





        rst = New ADODB.Recordset

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        ' Prepare stored procedure parameters
        With cmd
            .CommandText = "uspSelUserClinics"
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

        FetchClinicsByUserID = rst

        cmd = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        rst = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function


    Public Function FetchClinicsByUserName(ByVal strUserName As String) _
                        As ADODB.Recordset 'R002
        '--------------------------------------------------------------------
        'Date: 02/14/2000                                                   '
        'Author: Dave Richkun                                               '
        'Description:  Returns a recordset containing a list of all clinics '
        '              to which a user is associated.                       '
        'Parameters: strUserName - The network login name of the user       '
        'Returns: A recordset containing a list of all clinics to which a   '
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
        strSQL = strSQL & "tblClinic.fldClinicID, "
        strSQL = strSQL & "tblClinic.fldBusinessName "
        strSQL = strSQL & "FROM "
        strSQL = strSQL & "tblClinic,  "
        strSQL = strSQL & "tblUserClinic,  "
        strSQL = strSQL & "tblUsers  "
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & "tblClinic.fldClinicID = tblUserClinic.fldClinicID AND  "
        strSQL = strSQL & "tblUserClinic.fldUserID = tblUsers.fldUserID AND  "
        strSQL = strSQL & "tblUsers.fldUserName = '" & strUserName & "'"

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Instantiate and populate the Recordset.
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(strSQL, cnnSQL, adOpenForwardOnly, adLockReadOnly, _
               adCmdText + adAsyncFetch)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing

        FetchClinicsByUserName = rstSQL



    End Function

    Public Sub Insert(ByVal lngUserID As Long, ByVal lngClinicID As Long)
        '--------------------------------------------------------------------
        'Date: 01/04/2000                                                   '
        'Author: Dave Richkun                                               '
        'Description:  Inserts one or more rows into the tblUserClinic      '
        '               table.                                              '
        'Parameters: lngUserID - ID of the user who is being associated to  '
        '               one or more clinics                                 '
        '            lngClinicID - ID of the clinic being associated with a
        '               user
        'Returns: Null                                                      '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim strSQL As String
        Dim intCTR As Integer



        cmdSQL = New ADODB.Command
        cmdSQL.CommandType = adCmdStoredProc

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        With cmdSQL
            .CommandText = "uspInsUserClinic"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
            .Parameters.Append(.CreateParameter("@ClinicID", adInteger, adParamInput, , lngClinicID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute() ', adExecuteNoRecords

        ' Check for those ungodly errors
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection and free all resources
        cnnSQL.Close()

EMPTY_ARRAY:
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Sub


        'The Clinic array may be empty.
        If Err.Number = 9 Then
            GoTo EMPTY_ARRAY
        Else
            'Signal incompletion and raise the error to the ing environment.
            cmdSQL.Execute()
            'err.raise(Err.Number, Err.Source, Err.Description)
        End If
    End Sub


    Public Sub DeleteUserClinics(ByVal lngUserID As Long)
        '--------------------------------------------------------------------
        'Date: 03/28/2000                                                   '
        'Author: Dave Richkun                                               '
        'Description:  Deletes all clinics associated with a user
        'Parameters: lngUserID - ID of the user who's clinic associatons are
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
            .CommandText = "uspDelUserClinics"
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




    ''Public Sub Update(ByVal lngUserID As Long, ByVal varClinics As Object)
    '--------------------------------------------------------------------
    'Date: 01/04/2000
    'Author: Dave Richkun
    'Description:  Updates a row into the tblUsers table, and inserts rows
    '              into the tblUserClinics table.
    'Parameters: Each parameter identifies the column values that will be
    '              Updated.
    'Returns: Null
    '--------------------------------------------------------------------
    ''
    ''    Dim objUserClinic As ClinicDB.CUserClinicDB
    ''    Dim strErrMessage As String
    ''
    ''   :
    ''
    ''    'Update the UserClinic records
    ''    Set objUserClinic = CreateObjectXXXXXXX("ClinicDB.CUserClinicDB")
    ''     objUserClinic.Insert(lngUserID, varClinics)
    ''    Set objUserClinic = Nothing
    ''
    ''    'Signal successful completion
    ''    GetObjectContext.SetComplete
    ''
    ''    Exit Sub
    ''
    ''
    ''   'Signal incompletion and raise the error to the ing environment.
    ''    GetObjectContext.SetAbort
    ''   'err.raise Err.Number, Err.Source, Err.Description
    ''
    ''End Sub

End Class




