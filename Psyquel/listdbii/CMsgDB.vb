Option Strict Off
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CMsgDB
    '--------------------------------------------------------------------
    'Class Name: CMsgDB                                                 '
    'Date: 06/05/2000                                                   '
    'Author: Rick "Boom Boom" Segura                                    '
    'Description:  MTS object designed to host methods associated with  '
    '              data affecting the tblMessage table.                 '
    '--------------------------------------------------------------------
    ' Revision History:                                                 '
    '   R001 08/22/2000 Segura:  Made connection improvements and leak checks    '
    '   R002 09/11/2000 Richkun:  Added UserID parameter to Fetch() method.
    '   R003 10/02/2000 Richkun:  Added Delete() method
    '   R004 02/19/2001 Segura:  Added HotList Count Parameter to Fetch
    '   R005 05/23/2001 Segura:  Added 2 more Parameters to Fetch()
    '   R006 06/12/2001 Richkun:  Added additional parameters to Fetch()
    '        to support Misdirected Payments and Unbilled appointments
    '   R007 10/31/2001 Richkun: Altered Insert() method to include Patient-Appointment
    '        columns.  Altered Exists() method to take advantage of new columns.
    '   R008 11/05/2001 Pena: Altered Fetch() method to include tenative appts scheduled by the  center.
    '   R008 03/04/2003 Richkun: Altered Fetch() method to fetch only messages; counters retrieved by new method
    '--------------------------------------------------------------------

    Private _ConnectionString As String = String.Empty
    Private Const CLASS_NAME As String = "CMsgDB"
    Private Const TABLE_NAME As String = "tblMessage"


    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property

    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Integer, ByVal strDeletedBy As String)
        '--------------------------------------------------------------------
        'Date: 06/06/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Flags a row in the tblMessage table marking the row as
        '              deleted or undeleted.                                '
        'Parameters: blnDeleted - Boolean value identifying if the record is'
        '                   to be deleted (True) or undeleted (False).      '
        '            lngID - ID of the row in the table whose value will be '
        '               updated.                                            '
        '            strDeletedBy - logon name of user "deleting" record    '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
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
        strSQL = strSQL & " fldDisabledYN = '" & blnFlag & "', "
        strSQL = strSQL & " fldDisabledDate = '" & Now().ToString("mm/dd/yyyy") & "', "
        strSQL = strSQL & " fldDisabledBy = '" & strDeletedBy & "' "
        strSQL = strSQL & "WHERE "
        strSQL = strSQL & "fldMsgID = " & lngID

        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        cmdSQL.CommandText = strSQL
        cmdSQL.CommandType = ADODB.CommandTypeEnum.adCmdText

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute(, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        'Close the connection and free all resources
        cnnSQL.Close()
        'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL = Nothing
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing

        'Signal successful completion






    End Sub


    Public Sub Delete(ByVal lngID As Integer)
        '--------------------------------------------------------------------
        'Date: 10/03/2000                                                   '
        'Author: Dave Richkun
        'Description:  Physiy deletes a message from the tblMessage table.
        'Parameters: lngID - ID of the row in the table whose value will be '
        '               deleted.                                            '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        ' R003 - Created
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim strSQL As String
        Dim blnFlag As String



        'Prepare the SQL statement
        strSQL = "DELETE "
        strSQL = strSQL & "FROM "
        strSQL = strSQL & TABLE_NAME
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & "fldMsgID = " & lngID

        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        cmdSQL.CommandText = strSQL
        cmdSQL.CommandType = ADODB.CommandTypeEnum.adCmdText

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute(, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        'Close the connection and free all resources
        cnnSQL.Close()
        'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL = Nothing
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing

        'Signal successful completion




    End Sub


    Public Function Fetch(ByVal lngUserID As Integer) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 06/06/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Retrieves all messages associated with a user
        'Parameters: lngUserID - ID of User to retrieve messages for
        'Returns: Recordset of requested messages
        '--------------------------------------------------------------------
        'Revision History:
        ' R002 - Added lngUSerID parameter
        '--------------------------------------------------------------------

        Dim cnn As ADODB.Connection
        Dim cmd As ADODB.Command
        Dim rst As New ADODB.Recordset
        Dim SQLErrorNum As Integer



        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        'Acquire the database connection.
        cnn.Open((_ConnectionString))
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelMessage"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , lngUserID))
        End With

        'Execute the stored procedure
        rst.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        rst.Open(cmd, , ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)

        Fetch = rst

        'Disconnect the recordset
        'UPGRADE_NOTE: Object cmd.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmd.ActiveConnection = Nothing
        'UPGRADE_NOTE: Object cmd may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmd = Nothing
        'UPGRADE_NOTE: Object rst.ActiveConnection may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        rst.ActiveConnection = Nothing
        'UPGRADE_NOTE: Object cnn may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnn = Nothing

        'Signal successful completion



    End Function

    Public Function Insert(ByVal strMsg As String, ByVal strAddedBy As String, ByVal lngUserID As Integer, Optional ByVal lngPatApptID As Integer = 0, Optional ByVal strPatApptMsgType As String = "") As Integer
        '--------------------------------------------------------------------
        'Date: 06/05/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Inserts a row into the tblMessage table utilizing a stored procedure.                                  '
        'Parameters: strMsg - Message text
        '            strAddedBy - Login name of user responsible for creating the message
        '            lngUserID - User ID of the message receipient
        '            lngPatApptID - Optional parameter identifying the ID of the row in
        '               tblPatientAppt for which the message is related
        '            strPatApptMsgType - Optional parameter identifying the type of message
        '               related to a Patient Appointment.  If provided, may be one of:
        '                   'C' - Appointment was Cancelled
        '                   'D' - Appointment entry was Deleted
        '                   'N' - Appointment was made without certification
        'Returns: ID (Primary Key) of the row inserted                      '
        '--------------------------------------------------------------------
        'Revision History:
        '  R007
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        ' Instantiate and prepare command object
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspInsMessage"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter(, ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamReturnValue))
            .Parameters.Append(.CreateParameter("@Msg", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 200, Left(strMsg, 200)))
            .Parameters.Append(.CreateParameter("@AddedBy", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 30, strAddedBy))
            'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
            .Parameters.Append(.CreateParameter("@UserID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , IIf(lngUserID = 0, System.DBNull.Value, lngUserID)))
            'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
            .Parameters.Append(.CreateParameter("@PatApptID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , IIf(lngPatApptID = 0, System.DBNull.Value, lngPatApptID)))
            'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
            .Parameters.Append(.CreateParameter("@PatApptMsgType", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, IIf(strPatApptMsgType = "", System.DBNull.Value, strPatApptMsgType)))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute(, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        'Check for errors
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Function
        End If

        Insert = cmdSQL.Parameters(0).Value

        'Close the connection an free all resources
        cnnSQL.Close()
        'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL = Nothing
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing

        ' Signal successful completion



    End Function

    Public Sub Update(ByVal lngID As Integer, ByVal strMessage As String, ByVal strModifiedBy As String)
        '--------------------------------------------------------------------
        'Date: 06/06/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Updates a row into the tblMessage table utilizing    '
        '              a stored procedure.                                  '
        'Parameters:  lngID - ID of the row in the table whose value will be'
        '               updated.                                            '
        '             strMessage - The modified message                     '
        '             strModifiedBy - logon name of user updating message   '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdMessage"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc

            .Parameters.Append(.CreateParameter("@UserID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@Msg", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 200, Left(strMessage, 200)))
            .Parameters.Append(.CreateParameter("@User", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 30, strModifiedBy))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute(, , ADODB.ExecuteOptionEnum.adExecuteNoRecords)

        'Check for errors
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection and free all resources
        cnnSQL.Close()
        'UPGRADE_NOTE: Object cmdSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cmdSQL = Nothing
        'UPGRADE_NOTE: Object cnnSQL may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        cnnSQL = Nothing



    End Sub

    Public Function Exists(ByVal lngUserID As Integer, ByVal lngPatApptID As Integer, ByVal strPatApptMsgType As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 10/31/2001
        'Author: Dave Richkun
        'Description:  Determines if a message related to a Patient Appointment exists
        '              for a specific user.  This method is used in a supporting role
        '              to help prevent users from getting nailed with duplicate messages.
        'Parameters: lngUserID - User ID of message recipient
        '            lngPatApptID - ID of row in tblPatientAppt for which existing messages
        '               are being sought.
        '            strPatApptMsgType - The type of message being sought for the specified
        '               Patient Appointment.  May be one of:
        '                   'C' - Appointment was Cancelled
        '                   'D' - Appointment entry was Deleted
        '                   'N' - Appointment was made without certification
        'Returns: True if the message exists, false otherwise               '
        '--------------------------------------------------------------------
        'Revision History:
        '  R007
        '--------------------------------------------------------------------
        Dim cnn As ADODB.Connection
        Dim cmd As ADODB.Command
        Dim rst As New ADODB.Recordset



        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        'Acquire the database connection.
        cnn.Open((_ConnectionString))
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelApptMessageByUserIDType"
            .CommandType = ADODB.CommandTypeEnum.adCmdStoredProc
            .Parameters.Append(.CreateParameter("@UserID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , lngUserID))
            .Parameters.Append(.CreateParameter("@PatApptID", ADODB.DataTypeEnum.adInteger, ADODB.ParameterDirectionEnum.adParamInput, , lngPatApptID))
            .Parameters.Append(.CreateParameter("@PatApptMsgType", ADODB.DataTypeEnum.adVarChar, ADODB.ParameterDirectionEnum.adParamInput, 1, strPatApptMsgType))
        End With

        'Execute the stored procedure
        rst.CursorLocation = ADODB.CursorLocationEnum.adUseClient
        rst.Open(cmd, , ADODB.CursorTypeEnum.adOpenForwardOnly, ADODB.LockTypeEnum.adLockReadOnly)

        If rst.RecordCount > 0 Then
            Exists = True
        Else
            Exists = False
        End If

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
End Class