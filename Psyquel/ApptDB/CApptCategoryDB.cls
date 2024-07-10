Option Strict Off
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

Public Class CApptCategoryDB
    '--------------------------------------------------------------------
    'Class Name: CApptCategoryDB                                        '
    'Date: 08/28/2000                                                   '
    'Author: Rick "Boom Boom" Segura                                    '
    'Description:  MTS object designed to host methods associated with  '
    '              data affecting the tblApptCaetegory table.           '
    '--------------------------------------------------------------------


    


    Private _ConnectionString As String = String.Empty

    Private Const CLASS_NAME As String = "CApptCategoryDB"
    Private Const TABLE_NAME As String = "tblApptCategory"

    '--------------------------------------------------------------------
    ' Public Methods    +++++++++++++++++++++++++++++++++++++++++++++++++
    '--------------------------------------------------------------------
    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property

    Public Function Insert(ByVal strDescription As String) As Long
        '--------------------------------------------------------------------
        'Date: 08/28/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Inserts an appointment category record into          '
        '               tblApptCategory via a stored procedure              '
        'Parameters:  strDescription - Description of the new appointment   '
        '               category to be inserted                             '
        'Returns:  ApptCategory ID on success, -1 otherwise                 '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '--------------------------------------------------------------------
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        cnn = New ADODB.Connection
        cmd = New ADODB.Command

        'Create the parameter objects
        With cmd
            .CommandText = "uspInsApptCategory"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@Description", adVarChar, adParamInput, 10, strDescription))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))

            'Assign the connection to the Command object and execute the stored procedure
            cnn.Open(_ConnectionString)
            .ActiveConnection = cnn
            .Execute() '()

            Insert = .Parameters("@ID").Value

            ''Check the ErrorNum parameter before deciding to commit the transaction
            'If .Parameters("@ErrorNum").Value <> 0 Then
            '   Exit Function
            'End If

        End With

        ' Clean house
        cnn = Nothing
        cmd = Nothing




    End Function

    Public Function Update(ByVal lngID As Long, ByVal strDescription As String)
        '--------------------------------------------------------------------
        'Date: 08/28/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Updates an appointment category record into          '
        '               tblApptCategory via a stored procedure              '
        'Parameters:  lngID - ID of appt category to update                 '
        '             strDescription - Updated description of the appt category
        'Returns:  Null                                                     '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '--------------------------------------------------------------------
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        cnn = New ADODB.Connection
        cmd = New ADODB.Command

        'Create the parameter objects
        With cmd
            .CommandText = "uspUpdApptCategory"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ApptCattegoryID", adInteger, adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@Description", adVarChar, adParamInput, 10, strDescription))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))

            'Assign the connection to the Command object and execute the stored procedure
            cnn.Open(_ConnectionString)
            .ActiveConnection = cnn
            .Execute() '()



        End With

        cnn = Nothing
        cmd = Nothing



    End Function

    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/28/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Retrieves records from the tblApptCategory table.    '
        'Parameters: blnIncludeDisabled - Optional parameter that identifies'
        '               if records flagged as 'Disabled' or 'De-activated'  '
        '               are to be included in the record set. The default   '
        '               value is False.                                     '
        'Returns: Recordset of requested Appt categories                    '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '--------------------------------------------------------------------
        Dim cnn As ADODB.Connection
        Dim cmd As ADODB.Command
        Dim rst As ADODB.Recordset

        Dim strShowDisabled As String
        Dim SQLErrorNum As Long



        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        If blnIncludeDisabled Then
            strShowDisabled = "Y"
        Else
            strShowDisabled = "N"
        End If

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelApptCategory"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ShowDisabled", adVarChar, adParamInput, 1, strShowDisabled))
            '.Parameters.Append .CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0)
        End With

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        Fetch = rst

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rst.ActiveConnection = Nothing
        cnn = Nothing


    End Function


    Public Function Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Long)
        '--------------------------------------------------------------------
        'Date: 08/28/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Flags a row in the tblApptCategory table marking the '
        '               row as deleted or undeleted.                        '
        'Parameters:   blnDeleted - Boolean value identifying if the record '
        '                   is to be deleted (True) or undeleted (False).   '
        '            lngID - ID of the row in the table whose value will be '
        '               updated.                                            '
        'Returns:  Null                                                     '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '--------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim strSQL As String
        Dim blnFlag As String



        If blnDeleted = True Then
            blnFlag = "Y"
        Else
            blnFlag = "N"
        End If

        'Prepare the SQL statement
        strSQL = "UPDATE "
        strSQL = strSQL & TABLE_NAME
        strSQL = strSQL & " SET "
        strSQL = strSQL & " fldDisabledYN = '" & blnFlag & "' "
        strSQL = strSQL & "WHERE "
        strSQL = strSQL & "fldApptCategoryID = " & lngID

        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        cmdSQL.CommandText = strSQL
        cmdSQL.CommandType = adCmdText

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Close the connection and free all resources
        cnnSQL.Close()
        cmdSQL = Nothing
        cnnSQL = Nothing


    End Function

    Public Function Exists(ByVal strDescription As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 08/28/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description: Determines if an Appointment Category description already
        '               exists in tblApptCategory                           '
        'Parameters:  strDescription - Appointment Category description being
        '               sought in tblApptCategory                             '
        'Returns:  TRUE if description is found, FALSE otherwise            '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
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
        strSQL = strSQL & " fldDescription = '" & strDescription & "' "

        'Instantiate the Recordset.
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = adUseClient

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

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

    '--------------------------------------------------------------------
    ' PrivateMethods    +++++++++++++++++++++++++++++++++++++++++++++++++
    '--------------------------------------------------------------------

    Public Sub New()

    End Sub
End Class
