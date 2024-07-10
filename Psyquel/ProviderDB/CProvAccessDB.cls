
'--------------------------------------------------------------------
'Class Name: CProvAccessDB
'Date: 12/20/2000
'Author: Joshua Lockwood
'Denis J Finkel
'Description:  Database object designed to implement methods (by ing
'              stored procedures) associated with
'              data affecting the tblProviderAccess table.
'--------------------------------------------------------------------
'Revision History:
'   R001 : DNichol 03/23/2001 : Added FetchProviderAccessListByID
'   R002 : Richkun 04/11/2001 : Added FetchWebUserListByName() method
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

Public Class CProvAccessDB

    Private _ConnectionString As String = String.Empty


    Private Const CLASS_NAME As String = "CProvAccessDB"
    Private Const TABLE_NAME As String = "tblProviderAccess"


    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property



    Public Function FetchProviderAccessList(ByVal lngUserID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 12/20/2000
        'Author: Joshua Lockwood
        'Description: Retrieves records from the tblProviderAccess table.  s
        '               stored procedure uspSelProviderAccessList.  Record set
        '               shows list of providers and associated colleagues
        'Parameters: userID - owner ID.
        '            includeDisabled - also include disabled records on true.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As ADODB.Connection ' Declare DB connection
        Dim rst As ADODB.Recordset  ' Declare recordset
        Dim cmd As ADODB.Command    ' Declare SQL statement



        'Create the parameter objects
        cmd = New ADODB.Command     ' Instantiate SQL statement
        With cmd
            .CommandText = "uspSelProviderAccessList"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
            .Parameters.Append(.CreateParameter("@IncludeDisabledYN", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = False, "N", "Y")))
        End With

        'Acquire the database connection.
        cnn = New ADODB.Connection  ' Instantiate DB connection
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn

        'Set cursor and execute
        rst = New ADODB.Recordset   ' Instantiate recordset
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)   ' open recordset

        ' Cannot close recordset here...close by client
        FetchProviderAccessList = rst ' Set return value

        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        rst.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cnn = Nothing
        cmd = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()

        cnn.Close()   ' close DB connection
        cmd.ActiveConnection = Nothing
        rst.Close()   ' close the recordset --R003
        rst.ActiveConnection = Nothing

        ' De-allocate space for objects
        rst = Nothing
        cmd = Nothing
        cnn = Nothing

        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function

    Public Function FetchProviderAccessListByID(ByVal lngProvAccessID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/23/2001
        'Author: David Nichol
        'Description: Retrieves a record from the tblProviderAccess table.  s
        '               stored procedure uspSelProviderAccessListByID.  Record
        '               includes provider, colleague, and full names of both.
        'Parameters: userID - owner ID.
        '            includeDisabled - also include disabled records on true.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As ADODB.Connection ' Declare DB connection
        Dim rst As ADODB.Recordset  ' Declare recordset
        Dim cmd As ADODB.Command    ' Declare SQL statement



        'Create the parameter objects
        cmd = New ADODB.Command     ' Instantiate SQL statement
        With cmd
            .CommandText = "uspSelProviderAccessListByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProvAccessID", adInteger, adParamInput, , lngProvAccessID))
            .Parameters.Append(.CreateParameter("@IncludeDisabledYN", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = False, "N", "Y")))
        End With

        'Acquire the database connection.
        cnn = New ADODB.Connection  ' Instantiate DB connection
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn

        'Set cursor and execute
        rst = New ADODB.Recordset   ' Instantiate recordset
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)   ' open recordset

        ' Cannot close recordset here...close by client
        FetchProviderAccessListByID = rst ' Set return value

        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        rst.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cnn = Nothing
        cmd = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()

        cnn.Close()   ' close DB connection
        cmd.ActiveConnection = Nothing
        rst.Close()   ' close the recordset --R003
        rst.ActiveConnection = Nothing

        ' De-allocate space for objects
        rst = Nothing
        cmd = Nothing
        cnn = Nothing

        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function

    Public Function FetchUserListByName(ByVal strLastName As String, Optional ByVal strFirstName As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 01/10/2001
        'Author: Joshua Lockwood
        'Description: Pardon me if I elaborate on functionality.  This method runs
        '   uspSelProviderByName that returns one or more records containing the
        '   name and ID of user whose data is queried for (name passed in).  Data
        '   is retrieved from tblUser.
        '
        'Parameters: strLastName - last name of desired user
        '            strFirstName - first name of desired user
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        ' Declare connection, returned recordset, and command
        Dim cnn As ADODB.Connection ' Declare DB connection
        Dim rst As ADODB.Recordset  ' Declare recordset
        Dim cmd As ADODB.Command    ' Declare SQL statement



        'Create the parameter objects
        cmd = New ADODB.Command     ' Instantiate SQL statement
        With cmd
            .CommandText = "uspSelProviderByName"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@LastName", adVarChar, adParamInput, 15, strLastName))
            .Parameters.Append(.CreateParameter("@FirstName", adVarChar, adParamInput, 15, strFirstName))
        End With

        'Acquire the database connection.
        cnn = New ADODB.Connection  ' Instantiate DB connection
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn

        'Set cursor and execute
        rst = New ADODB.Recordset   ' Instantiate recordset
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)   ' open recordset

        FetchUserListByName = rst ' Set return value

        ' Cannot close recordset here...close by client

        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        rst.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cnn = Nothing
        cmd = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()

        cnn.Close()   ' close DB connection
        cmd.ActiveConnection = Nothing
        rst.Close()   ' close the recordset --R003
        rst.ActiveConnection = Nothing

        ' De-allocate space for objects
        rst = Nothing
        cmd = Nothing
        cnn = Nothing

        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function

    Public Function FetchPatientByOwner(ByVal lngProviderID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 12/20/2000
        'Author: Joshua Lockwood
        'Description: Retrieves records from the tblBenefactor table.  s
        '               stored procedure uspSelPatientByOwner.  Record set
        '               returns associated patient information per provider
        '               (associated by tblBenefactor.fldOwnerID).
        'Parameters: lngProviderID - owner ID.
        '            includeDisabled - also include disabled records on true.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As ADODB.Connection ' Declare DB connection
        Dim rst As ADODB.Recordset  ' Declare recordset
        Dim cmd As ADODB.Command    ' Declare SQL statement



        'Create the parameter objects
        cmd = New ADODB.Command     ' Instantiate SQL statement
        With cmd
            .CommandText = "uspSelPatientByOwner"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@IncludeDisabledYN", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = False, "N", "Y")))
        End With

        'Acquire the database connection.
        cnn = New ADODB.Connection  ' Instantiate DB connection
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn

        'Set cursor and execute
        rst = New ADODB.Recordset   ' Instantiate recordset
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)   ' open recordset

        FetchPatientByOwner = rst ' Set return value

        ' Cannot close recordset here...close by client

        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        rst.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cnn = Nothing
        cmd = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()

        cnn.Close()   ' close DB connection
        cmd.ActiveConnection = Nothing
        rst.Close()   ' close the recordset --R003
        rst.ActiveConnection = Nothing

        ' De-allocate space for objects
        rst = Nothing
        cmd = Nothing
        cnn = Nothing

        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function

    Public Function FetchPatientOwner(ByVal lngPatientID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 12/20/2000
        'Author: Joshua Lockwood
        'Description: Retrieves records from the tblProviderAccess table.  s
        '               stored procedure uspSelPatientOwner.  Returns the owner
        '               of the patient whos ID was passed.  Right now the ID
        '               is passed as part of a recordset.
        'Parameters: lngPatientID - patient ID.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As ADODB.Connection ' Declare DB connection
        Dim rst As ADODB.Recordset  ' Declare recordset
        Dim cmd As ADODB.Command    ' Declare SQL statement



        'Create the parameter objects
        cmd = New ADODB.Command     ' Instantiate SQL statement
        With cmd
            .CommandText = "uspSelPatientOwner"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
        End With

        'Acquire the database connection.
        cnn = New ADODB.Connection  ' Instantiate DB connection
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn

        'Set cursor and execute
        rst = New ADODB.Recordset   ' Instantiate recordset
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)   ' open recordset

        FetchPatientOwner = rst ' Set return value

        ' Cannot close recordset here...close by client

        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        rst.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cnn = Nothing
        cmd = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()

        cnn.Close()   ' close DB connection
        cmd.ActiveConnection = Nothing
        rst.Close()   ' close the recordset --R003
        rst.ActiveConnection = Nothing

        ' De-allocate space for objects
        rst = Nothing
        cmd = Nothing
        cnn = Nothing

        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function


    Public Function Insert(ByVal strUserName As String, ByVal lngProviderID As Long, ByVal lngColleagueID As Long) As Long
        '--------------------------------------------------------------------
        'Date: 12/20/2000
        'Author: Joshua Lockwood
        'Description: Adds new record to the tblProviderAccess table.  s
        '               stored procedure uspInsProviderAccess.  Sets up owner /
        '               colleague relationship that allows patient sharing.
        'Parameters: strUserName - for AddedBy.
        '            lngProviderID - ID of the owner granting priviledges to a colleague
        '            lngColleagueID - ID of the colleague being added to access list
        'Returns: Long (ProviderAccessID)
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As ADODB.Connection ' Declare DB connection
        Dim cmd As ADODB.Command    ' Declare SQL statement



        'Create the parameter objects
        cmd = New ADODB.Command     ' Instantiate SQL statement
        With cmd
            .CommandText = "uspInsProviderAccess"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderAccessID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@ColleagueID", adInteger, adParamInput, , lngColleagueID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        cnn = New ADODB.Connection  ' Instantiate DB connection
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn
        cmd.Execute()

        Insert = cmd.Parameters(0).Value

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Function
        End If

        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()

        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cmd = Nothing
        cnn = Nothing

        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function

    Public Sub Update(ByVal lngUserID As Long, ByVal lngNewOwner As Long, ByVal lngPatientID As Long)
        '--------------------------------------------------------------------
        'Date: 12/20/2000
        'Author: Joshua Lockwood
        'Description: Updates selected resords in tblBenefactor table.  s
        '               stored procedure uspUpdProviderAccess.
        'Parameters: lngUserID - ID of current owner granting the transfer.
        '            lngNewOwner - Owner of the 'soon-to-be' owner.
        '            lngPatientID - ID of the patient which is being transfered.
        'Returns: Nothing
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As ADODB.Connection ' Declare DB connection
        Dim cmd As ADODB.Command    ' Declare SQL statement



        'Create the parameter objects
        cmd = New ADODB.Command     ' Instantiate SQL statement
        With cmd
            .CommandText = "uspUpdPatientOwner"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
            .Parameters.Append(.CreateParameter("@NewOwner", adInteger, adParamInput, , lngNewOwner))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        cnn = New ADODB.Connection  ' Instantiate DB connection
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        cmd.Execute()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()

        cmd.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        ' De-allocate space for objects
        cmd = Nothing
        cnn = Nothing

        'err.raise(Err.Number, Err.Source, Err.Description)
    End Sub

    Public Sub Deleted(ByVal strUserName As String, ByVal lngProviderAccessID As Long, ByVal blnDisabledYN As Boolean)
        '--------------------------------------------------------------------
        'Date: 12/20/2000
        'Author: Joshua Lockwood
        'Description: 'Deletes' selected record in tblProviderAccess table.  s
        '               stored procedure uspDelProviderAccess.
        'Parameters: strUserName - for 'DeletedBy.
        '            lngProviderAccessID - ID of relationship to be deleted.
        '            blnDisabledYN - Toggle disabled flag of relation.
        'Returns: Nothing
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As ADODB.Connection ' Declare DB connection
        Dim cmd As ADODB.Command    ' Declare SQL statement



        'Create the parameter objects
        cmd = New ADODB.Command     ' Instantiate SQL statement
        With cmd
            .CommandText = "uspDelProviderAccess"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@ProviderAccessID", adInteger, adParamInput, , lngProviderAccessID))
            .Parameters.Append(.CreateParameter("@DisabledYN", adVarChar, adParamInput, 1, IIf(blnDisabledYN = False, "N", "Y")))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        cnn = New ADODB.Connection  ' Instantiate DB connection
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn
        cmd.Execute()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        cnn.Close()   ' close DB connection
        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        ' Set rst.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion


        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()

        cmd.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        ' De-allocate space for objects
        cmd = Nothing
        cnn = Nothing

        'err.raise(Err.Number, Err.Source, Err.Description)
    End Sub

    Public Function FetchWebUserListByName(ByVal strLastName As String, Optional ByVal strFirstName As String = "", Optional ByVal lngProviderID As Long = 0) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 04/11/2001   R002
        'Author: Dave Richkun
        'Description:  This method was derived from FetchUserListByName() and returns
        '              matching names and IDs of Psyquel Web-based providers.
        'Parameters: strLastName - Last name of desired user
        '            strFirstName - First name of desired user
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------

        Dim cnn As ADODB.Connection
        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command



        cmd = New ADODB.Command     ' Instantiate SQL statement
        With cmd
            .CommandText = "uspSelWebProvidersByName"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@LastName", adVarChar, adParamInput, 15, strLastName))
            .Parameters.Append(.CreateParameter("@FirstName", adVarChar, adParamInput, 15, strFirstName))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
        End With

        'Acquire the database connection.
        cnn = New ADODB.Connection
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Set cursor and execute
        rst = New ADODB.Recordset
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)   ' open recordset

        FetchWebUserListByName = rst

        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        rst.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cnn = Nothing
        cmd = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()

        cnn.Close()   ' close DB connection
        cmd.ActiveConnection = Nothing
        rst.Close()   ' close the recordset --R003
        rst.ActiveConnection = Nothing

        ' De-allocate space for objects
        rst = Nothing
        cmd = Nothing
        cnn = Nothing

        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function

End Class
