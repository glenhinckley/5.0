
'--------------------------------------------------------------------
'Class Name: CEMRTemplateDB
'Date: 09/05/2000
'Author: Joshua Lockwood
'Description:  Database object designed to implement methods (by ing
'              stored procedures) associated with
'              data affecting the tblProviderTemplate table.
'--------------------------------------------------------------------
'  Revisions:
'   R001 : Joshua Lockwood, 8Sep00 :
'       - Broad changes made that repair s to DB
'       - Worthless references to recordsets made in non fetch methods removed.
'       - Minor typo fixes
'   R002 : Joshua Lockwood, 11SEP00 : Made changes to account for using
'       type TEXT for Template Text.
'   R003 : Joshua Lockwood, 15Sep00
'       - Numerous changes geared at sealing memory holes
'   R004 : Joshua Lockwood, 27SEP00 : Removed use of TEXT for template.
'       Changed DB object s to conform to new stored procedures.
'   R005 : Joshua Lockwood, 02Nov00 : Added param to Item inserts
'       for tblTemplateItem(fldItemNumber), added fetchNext and
'       fetchPrev methods, updateItemNumber method.
'   R006 : Joshua Lockwood, 08Nov00 : Added FetchByItemNumber
'   R007 : David Nichol, 16Jan01 : Changed UpdateItemNumber to take templateID,
'       newItemNumber, and oldItemNumber
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
' Lydia Orth 

Public Class CEMRTemplateDB

    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CEMRTemplateDB"
    Private Const TABLE_NAME As String = "tblProviderTemplate"

    Public Function FetchByID(ByVal lngTemplateID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/05/2000
        'Author: David Nichol and Joshua Lockwood
        'Description: Retrieves records from the tblProviderTemplate table.  s
        '               stored procedure uspSelTemplateByID.
        'Parameters: lngTemplateID - 'Where' clause of the SQL statement by
        '               which records will be fetched.  Matches tblProviderTemplate's
        '               primary key.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        '   --R001

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim rst As New ADODB.Recordset  ' Declare recordset
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        'Create the parameter objects
        ' Instantiate SQL statement
        With cmd
            .CommandText = "uspSelTemplateByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@TemplateID", adInteger, adParamInput, , lngTemplateID))
        End With

        'Acquire the database connection.
        ' Instantiate DB connection
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn

        'Set cursor and execute
        ' Instantiate recordset
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)   ' open recordset


        ' Cannot close recordset here...close by client

        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        rst.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cnn = Nothing
        cmd = Nothing

        FetchByID = rst ' Set return value

        'Signal successful completion


        Exit Function


    End Function
    Public Function FetchByItemID(ByVal lngItemID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 11/06/2000
        'Author: David Nichol and Joshua Lockwood
        'Description: Retrieves records from the tblTemplateQuestion table.  s
        '               stored procedure uspSelTemplateItemByID.
        'Parameters: lngItemID - 'Where' clause of the SQL statement by
        '               which records will be fetched.  Matches tblTemplateQuestion's
        '               primary key.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        '   --R001

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim rst As New ADODB.Recordset  ' Declare recordset
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        'Create the parameter objects
        ' Instantiate SQL statement
        With cmd
            .CommandText = "uspSelTemplateItemByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ItemID", adInteger, adParamInput, , lngItemID))
        End With

        'Acquire the database connection.
        ' Instantiate DB connection
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn

        'Set cursor and execute
        ' Instantiate recordset
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)   ' open recordset


        ' Cannot close recordset here...close by client

        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        rst.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cnn = Nothing
        cmd = Nothing

        FetchByItemID = rst ' Set return value

        'Signal successful completion



        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function
    Public Function FetchByItemNumber(ByVal lngTemplateID As Long, ByVal lngItemNumber As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 11/08/2000
        'Author: Joshua Lockwood
        'Description: Retrieves records from the tblTemplateQuestion table using the
        '           TemplateID and ItemNumber.
        'Parameters: lngItemID - 'Where' clause of the SQL statement by
        '               which records will be fetched.  Matches tblTemplateQuestion's
        '               primary key.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        '   --R006

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim rst As New ADODB.Recordset  ' Declare recordset
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        'Create the parameter objects
        ' Instantiate SQL statement
        With cmd
            .CommandText = "uspSelTemplateByItemNumber"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@TemplateID", adInteger, adParamInput, , lngTemplateID))
            .Parameters.Append(.CreateParameter("@ItemNumber", adInteger, adParamInput, , lngItemNumber))
        End With

        'Acquire the database connection.
        ' Instantiate DB connection
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn

        'Set cursor and execute
        ' Instantiate recordset
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)   ' open recordset


        ' Cannot close recordset here...close by client

        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        rst.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cnn = Nothing
        cmd = Nothing

        FetchByItemNumber = rst ' Set return value

        'Signal successful completion



    End Function
    Public Function FetchByUser(ByVal lngUserID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/05/2000
        'Author: Dave Nichol and Joshua Lockwood
        'Description:  Retrieves records from the tblProviderTemplate table. s
        '               stored procedure uspSelTemplateByUser
        'Parameters: lngUserID - UserID of user ing method.
        '            blnIncludeDisabled - boolean value -- whether to include
        '            disabled rows (true) or not (false).
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim rst As New ADODB.Recordset  ' Declare recordset
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        'Create the parameter objects
        ' Instantiate SQL statement --R003
        With cmd
            .CommandText = "uspSelTemplateByUser"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
            .Parameters.Append(.CreateParameter("@IncludeDisabledYN", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = False, "N", "Y")))
        End With

        'Set cursor and execute
        ' Instantiate DB connection --R003
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        ' Instantiate recordset --R003
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)   ' open recordset

        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        rst.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cmd = Nothing
        cnn = Nothing

        FetchByUser = rst    ' Set return value
        'Signal successful completion


    End Function
    Public Function FetchNextItem(ByVal lngTemplateID As Long, _
                                  ByVal lngCurrentItem As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 11/02/2000 - R005
        'Author: Joshua Lockwood
        'Description: Retrieves records from the tblTemplateItem table.  s
        '               stored procedure uspSelNextItem.  New with revision R005
        'Parameters: lngTemplateID - ID of template associated with the Item.
        '            lngCurrentItem - Sequence number tblTemplateItem(fldItemNumber)
        '               associated with current Item. <Next from *>
        'Returns: INTEGER (ItemID)
        '--------------------------------------------------------------------
        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim rst As New ADODB.Recordset  ' Declare recordset
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        ' Instantiate the objects
        ' Instantiate SQL statement

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelNextItem"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@TemplateID", adInteger, adParamInput, , lngTemplateID))
            .Parameters.Append(.CreateParameter("@CurrentItem", adInteger, adParamInput, , lngCurrentItem))
        End With

        'Acquire the database connection.
        ' Instantiate DB connection
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn

        'Set cursor and execute
        ' Instantiate recordset
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)   ' open recordset


        'Disconnect the recordset
        ' Cannot close recordset here
        'rst.Close   ' close recordset

        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        rst.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cnn = Nothing
        cmd = Nothing

        FetchNextItem = rst ' Set return value

        'Signal successful completion



    End Function
    Public Function FetchPrevItem(ByVal lngTemplateID As Long, _
                                  ByVal lngCurrentItem As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 11/02/2000
        'Author: Joshua Lockwood
        'Description: Retrieves records from the tblTemplateItem table.  s
        '               stored procedure uspSelNextItem.  New with revision R005
        'Parameters: lngTemplateID - ID of template associated with the Item.
        '            lngCurrentItem - Sequence number tblTemplateItem(fldItemNumber)
        '               associated with current Item. <Prev from *>
        'Returns: INTEGER (ItemID)
        '--------------------------------------------------------------------
        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim rst As New ADODB.Recordset  ' Declare recordset
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        ' Instantiate the objects
        ' Instantiate SQL statement

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelPrevItem"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@TemplateID", adInteger, adParamInput, , lngTemplateID))
            .Parameters.Append(.CreateParameter("@CurrentItem", adInteger, adParamInput, , lngCurrentItem))
        End With

        'Acquire the database connection.
        ' Instantiate DB connection
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn

        'Set cursor and execute
        ' Instantiate recordset
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)   ' open recordset


        'Disconnect the recordset
        ' Cannot close recordset here
        'rst.Close   ' close recordset

        ' De-allocate space for objects
        cmd.ActiveConnection = Nothing
        rst.ActiveConnection = Nothing
        cnn.Close()   ' close DB connection
        cnn = Nothing
        cmd = Nothing

        FetchPrevItem = rst ' Set return value

        'Signal successful completion



    End Function


    Public Function InsertTemplate(ByVal lngUserID As Long, ByVal strTemplateName As String, _
                           ByVal strTemplateDesc As String, ByVal strUserName As String) As Long
        '--------------------------------------------------------------------
        'Date: 09/05/2000
        'Author: Joshua Lockwood
        'Description:  Inserts a row into the tblProviderTemplate table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        '   lngUserID strTemplateName strTemplateDesc strUserName SQLErrorNum
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------
        ' R002 - In param list, strTemplateText changed to type Object

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        'Create the parameter objects
        ' Instantiate SQL statement --R003
        With cmd
            .CommandText = "uspInsTemplate"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@TemplateID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@UserID", adInteger, adParamInput, , lngUserID))
            .Parameters.Append(.CreateParameter("@TemplateName", adVarChar, adParamInput, 30, strTemplateName))
            .Parameters.Append(.CreateParameter("@TemplateDesc", adVarChar, adParamInput, 128, strTemplateDesc))
            ' R004 - Use of text no longer valid...cut templateText
            ' R003 - .AppendChunk is needed for processing type TEXT items
            ' TEXT limit is 8192 characters, after that data will need to be 'chunked' out
            '   chunk size will also be 8192; add code ASAP for chunk handler
            '        .Parameters.Append .CreateParameter("@TemplateText", adLongVarChar, adParamInput, Len(strTemplateText))
            '        .Parameters("@TemplateText").AppendChunk strTemplateText
            ' R004
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        ' Instantiate DB connection
        'Assign the connection to the Command object and execute
        cnn.Open(_ConnectionString)  ' open DB connection
        cmd.ActiveConnection = cnn
        cmd.Execute() '()

        InsertTemplate = cmd.Parameters("@TemplateID").Value

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Function
        End If

        ' De-allocate space for objects
        cnn.Close()   ' close DB connection
        cmd.ActiveConnection = Nothing 'Close ADO stuff explicitly --R003
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion



    End Function
    Public Function InsertTemplateItem(ByVal lngTemplateID As Long, _
                    ByVal strItemText As String, ByVal strPossibleAnswers As String, _
                    ByVal strDisplayText As String, ByVal strType As String, _
                    ByVal strHTMLName As String, ByVal lngItemNumber As Long) As Long
        '--------------------------------------------------------------------
        'Date: 09/05/2000
        'Author: Joshua Lockwood
        'Description:  Inserts a row into the tblTemplate/Item table utilizing
        '              a stored procedure.  New with revision R004
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------
        ' R004 - Added with new revision

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        'Create the parameter objects
        ' Instantiate SQL statement
        With cmd
            .CommandText = "uspInsTemplateItem"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@TemplateID", adInteger, adParamInput, , lngTemplateID))
            .Parameters.Append(.CreateParameter("@ItemText", adVarChar, adParamInput, 200, strItemText))
            .Parameters.Append(.CreateParameter("@PossibleAnswers", adVarChar, adParamInput, 200, strPossibleAnswers))
            .Parameters.Append(.CreateParameter("@DisplayText", adVarChar, adParamInput, 350, strDisplayText))
            .Parameters.Append(.CreateParameter("@Type", adVarChar, adParamInput, 1, strType))
            .Parameters.Append(.CreateParameter("@HTMLName", adVarChar, adParamInput, 20, strHTMLName))
            .Parameters.Append(.CreateParameter("@ItemNumber", adInteger, adParamInput, , lngItemNumber)) 'R005
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        ' Instantiate DB connection
        'Assign the connection to the Command object and execute
        cnn.Open(_ConnectionString)  ' open DB connection
        cmd.ActiveConnection = cnn
        cmd.Execute()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Function
        End If

        InsertTemplateItem = cmd.Parameters(0).Value

        ' De-allocate space for objects
        cnn.Close()   ' close DB connection
        cmd.ActiveConnection = Nothing 'Close ADO stuff explicitly
        cmd = Nothing
        cnn = Nothing


        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function
    Public Sub UpdateTemplate(ByVal lngTemplateID As Long, _
            ByVal strTemplateName As String, ByVal strTemplateDesc As String)
        '--------------------------------------------------------------------
        'Date: 09/05/2000
        'Author: Joshua Lockwood
        'Description: Updates selected records in the tblProviderTemplate table.  s
        '               stored procedure uspUpdTemplate.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        '   lngTemplateID strTemplateName strTemplateDesc SQLErrorNum
        'Returns: Null
        '--------------------------------------------------------------------
        ' R002 - In param list, strTemplateText changed to type Object

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        'Create the parameter objects
        ' Instantiate SQL statement --R002
        With cmd
            .CommandText = "uspUpdTemplate"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@TemplateID", adInteger, adParamInput, , lngTemplateID))
            .Parameters.Append(.CreateParameter("@TemplateName", adVarChar, adParamInput, 30, strTemplateName))
            .Parameters.Append(.CreateParameter("@TemplateDesc", adVarChar, adParamInput, 128, strTemplateDesc))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        ' Instantiate DB connection --R003
        cnn.Open(_ConnectionString) ' open DB connection
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



    End Sub
    Public Sub UpdateTemplateItem(ByVal lngTemplateID As Long, _
            ByVal lngItemID As Long, ByVal strItemText As String, _
            ByVal strPossibleAnswers As String, ByVal strDisplayText As String, _
            ByVal strType As String, ByVal strHTMLName As String)
        '--------------------------------------------------------------------
        'Date: 09/27/2000
        'Author: Joshua Lockwood
        'Description: Updates selected records in the tblTemplateItem table.  s
        '               stored procedure uspUpdTemplateItem.  New with revision R004
        'Parameters: Each parameter identifies the column value that will be
        '              updated.
        '
        'Returns: Null
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        'Create the parameter objects
        ' Instantiate SQL statement --R002
        With cmd
            .CommandText = "uspUpdTemplateItem"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ItemID", adInteger, adParamInput, , lngItemID))
            .Parameters.Append(.CreateParameter("@TempalteID", adInteger, adParamInput, , lngTemplateID))
            .Parameters.Append(.CreateParameter("@ItemText", adVarChar, adParamInput, 200, strItemText))
            .Parameters.Append(.CreateParameter("@strPossibleAnswers", adVarChar, adParamInput, 200, strPossibleAnswers))
            .Parameters.Append(.CreateParameter("@strDisplayText", adVarChar, adParamInput, 350, strDisplayText))
            .Parameters.Append(.CreateParameter("@strType", adVarChar, adParamInput, 1, strType))
            .Parameters.Append(.CreateParameter("@strHTMLName", adVarChar, adParamInput, 20, strHTMLName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        ' Instantiate DB connection --R003
        cnn.Open(_ConnectionString) ' open DB connection
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



    End Sub
    Public Sub UpdateItemNumber(ByVal lngTemplateID As Long, ByVal lngOldItemNumber As Long, _
                                ByVal lngNewItemNumber As Long)
        '--------------------------------------------------------------------
        'Date: 11/02/2000
        'Author: Joshua Lockwood
        'Description: Updates value of ItemNumber in tblTemplateItem for
        '              the selected ItemID.  New with revision R005
        '             Also see R007 1/16/01
        'Parameters: Each parameter identifies the column value that will be
        '              updated.
        'Returns: Null
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        'Create the parameter objects
        ' Instantiate SQL statement --R002
        With cmd
            .CommandText = "uspUpdTemplateItemOrder"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@TemplateID", adInteger, adParamInput, , lngTemplateID))
            .Parameters.Append(.CreateParameter("@OldItemNumber", adInteger, adParamInput, , lngOldItemNumber))
            .Parameters.Append(.CreateParameter("@NewItemNumber", adInteger, adParamInput, , lngNewItemNumber))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        ' Instantiate DB connection --R003
        cnn.Open(_ConnectionString) ' open DB connection
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


    End Sub
    Public Sub CommitTemplate(ByVal lngTemplateID As Long)
        '--------------------------------------------------------------------
        'Date: 09/27/2000
        'Author: Joshua Lockwood
        'Description: Commits Items associated with template ID.
        '   New with revision R004
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        '
        'Returns: Null
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        'Create the parameter objects
        ' Instantiate SQL statement --R002
        With cmd
            .CommandText = "uspUpdCommitTemplate"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@TemplateID", adInteger, adParamInput, , lngTemplateID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        ' Instantiate DB connection --R003
        cnn.Open(_ConnectionString) ' open DB connection
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


    End Sub

    Public Sub CopyTemplate(ByVal lngProviderID As Long, ByVal lngTemplateID As Long)
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        'Create the parameter objects
        ' Instantiate SQL statement --R002
        With cmd
            .CommandText = "uspCopyNoteTemplate"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@TemplateID", adInteger, adParamInput, , lngTemplateID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        ' Instantiate DB connection --R002
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn
        cmd.Execute()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        ' De-allocate space for objects
        cnn.Close()   ' close DB connection
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion


    End Sub

    Public Sub PurgeTemplate(ByVal lngTemplateID As Long)
        '--------------------------------------------------------------------
        'Date: 09/05/2000
        'Author: Joshua Lockwood
        'Description:  Deletes all the Items that have not been committed
        'Parameters: lngTemplateID - ID of the row in the table whose value will be
        '               deleted.
        'Returns: Null
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        'Create the parameter objects
        ' Instantiate SQL statement --R002
        With cmd
            .CommandText = "uspDelPurgeTemplate"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@TemplateID", adInteger, adParamInput, , lngTemplateID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        ' Instantiate DB connection --R002
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn
        cmd.Execute()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        ' De-allocate space for objects
        cnn.Close()   ' close DB connection
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion


    End Sub

    Public Sub Deleted(ByVal lngTemplateID As Long, Optional ByVal blnDisabled As Boolean = False)
        '--------------------------------------------------------------------
        'Date: 09/05/2000
        'Author: Joshua Lockwood
        'Description:  Flags a row in the tblProviderTemplate table marking the row as
        '              deleted or undeleted.  s stored procedure uspDelTemplate
        'Parameters: lngTemplateID - ID of the row in the table whose value will be
        '               deleted.
        '            blnIncludeDisabled - boolean value -- whether to include
        '            disabled rows (true) or not (false).
        'Returns: Null
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        'Create the parameter objects
        ' Instantiate SQL statement --R002
        With cmd
            .CommandText = "uspDelTemplate"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@TemplateID", adInteger, adParamInput, , lngTemplateID))
            .Parameters.Append(.CreateParameter("@DisabledYN", adVarChar, adParamInput, 1, IIf(blnDisabled = False, "N", "Y")))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        ' Instantiate DB connection --R002
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn
        cmd.Execute()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        ' De-allocate space for objects
        cnn.Close()   ' close DB connection
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion


    End Sub


    Public Sub DeleteItem(ByVal lngItemID As Long)
        '--------------------------------------------------------------------
        'Date: 09/28/2000
        'Author: Joshua Lockwood
        'Description:  Deletes row from tblTemplateItem.  s stored procedure
        '   uspDelTemplateItem
        'Parameters: lngTemplateID - ID of the row in the table whose value will be
        '               deleted.
        'Returns: Null
        '--------------------------------------------------------------------

        ' Declare connection, returned recordset, and command
        Dim cnn As New ADODB.Connection ' Declare DB connection
        Dim cmd As New ADODB.Command    ' Declare SQL statement



        'Create the parameter objects
        ' Instantiate SQL statement --R002
        With cmd
            .CommandText = "uspDelTemplateItem"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ItemID", adInteger, adParamInput, , lngItemID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        ' Instantiate DB connection --R002
        cnn.Open(_ConnectionString) ' open DB connection
        cmd.ActiveConnection = cnn
        cmd.Execute()

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        ' De-allocate space for objects
        cnn.Close()   ' close DB connection
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        cnn = Nothing

        'Signal successful completion

        'err.raise(Err.Number, Err.Source, Err.Description)
    End Sub


End Class




