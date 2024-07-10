Option Strict Off
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CMsgBz
    '--------------------------------------------------------------------
    'Class Name: CMsgBz                                                 '
    'Date: 06/05/2000                                                   '
    'Author: Rick "Boom Boom" Segura                                    '
    'Description:  MTS business object designed to  methods         '
    '              associated with the CMsgDB class.                    '
    '--------------------------------------------------------------------
    ' Revision History:
    ' R001: 09/11/2000 Richkun - Added UserID parameter to Fetch() method.
    ' R002: 10/02/2000 Richkun - Added Delete() method.
    ' R003: 02/19/2000 Segura - Added Hot List Count to Fetch
    ' R004: 06/12/2001 Richkun - Added Misdirected Insurance Payment List,
    '       Unbilled Appointment count to Fetch
    ' R005: 10/31/2001 Richkun - Altered Insert() method to include Patient-Appointment
    '        columns.  Altered Exists() method to take advantage of new columns
    ' R006: 11/05/2001 Pena: Altered Fetch() method to include tenative appts scheduled by the  center.
    ' R007 03/04/2003 Richkun: Altered Fetch() method to fetch only messages; counters retrieved by new method
    '--------------------------------------------------------------------
    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CMsgBz"

    '--------------------------------------------------------------------
    ' Public Methods                                                    '
    '--------------------------------------------------------------------

    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Integer, ByVal strDeletedBy As Object)
        '--------------------------------------------------------------------
        'Date: 06/06/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Flags a row in the tblMessage table marking the row as
        '              deleted or undeleted.                                '
        'Parameters: blnDeleted - Boolean value identifying if the record is'
        '               to be deleted (True) or undeleted (False).          '
        '            lngID - ID of the row in the table whose value will be '
        '               updated.                                            '
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objMsg As New ListDb.CMsgDB




        'UPGRADE_WARNING: Couldn't resolve default property of object strDeletedBy. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        objMsg.Deleted(blnDeleted, lngID, strDeletedBy)

        'UPGRADE_NOTE: Object objMsg may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objMsg = Nothing


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
        ' R002 - Created
        '--------------------------------------------------------------------

        Dim objMsg As New ListDb.CMsgDB




        objMsg.Delete(lngID)

        'UPGRADE_NOTE: Object objMsg may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objMsg = Nothing


    End Sub


    Public Function Fetch(ByVal lngUserID As Integer) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 06/06/2000
        'Author: Rick "Boom Boom" Segura
        'Description:  Retrieves records from the tblMessage table.
        'Parameters: lngUserID - ID of User to retrieve messages for
        'Returns: Recordset of messages
        '--------------------------------------------------------------------
        'Revision History:
        '  R001 Added parameter
        '--------------------------------------------------------------------

        Dim objMsg As New ListDb.CMsgDB
        Dim rstSQL As New ADODB.Recordset



        rstSQL = objMsg.Fetch(lngUserID)
        Fetch = rstSQL

        'UPGRADE_NOTE: Object objMsg may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objMsg = Nothing


    End Function

    Public Function Insert(ByVal strMsg As String, ByVal strAddedBy As String, Optional ByVal lngUserID As Integer = 0, Optional ByVal lngPatApptID As Integer = 0, Optional ByVal strPatApptMsgType As String = "") As Integer
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
        '  R005
        '--------------------------------------------------------------------

        Dim objMsg As New ListDb.CMsgDB
        Dim lngID As Integer
        Dim strErrMessage As String





        'Verify data before proceeding.
        If Not VerifyData(strMsg, strAddedBy, strErrMessage) Then
            Exit Function
        End If

        lngID = objMsg.Insert(strMsg, strAddedBy, lngUserID, lngPatApptID, strPatApptMsgType)

        'UPGRADE_NOTE: Object objMsg may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objMsg = Nothing

        Insert = lngID
    End Function

    Public Sub Update(ByVal lngID As Integer, ByVal strMessage As String, ByVal strModifiedBy As String)
        '--------------------------------------------------------------------
        'Date: 06/06/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Updates a row into the tblMessage table.             '
        'Parameters:  lngID - ID of the row in the table whose value will be'
        '               updated.                                            '
        '             strMessage - The modified message                     '
        '             strModifiedBy - logon name of user modifying message  '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim objMsg As New ListDb.CMsgDB
        Dim strErrMessage As String





        'Verify data before proceeding.
        If Not VerifyData(strMessage, strModifiedBy, strErrMessage) Then
            Exit Sub
        End If

        objMsg.Update(lngID, strMessage, strModifiedBy)

        'UPGRADE_NOTE: Object objMsg may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objMsg = Nothing


    End Sub

    Public Function Exists(ByVal lngUserID As Integer, ByVal lngPatApptID As Integer, ByVal strPatApptMsgType As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 10/24/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
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
        'Revision History:                                                  '
        '  R005                                                                 '
        '--------------------------------------------------------------------

        Dim objMsg As New ListDb.CMsgDB
        Dim blnExists As Boolean




        Exists = objMsg.Exists(lngUserID, lngPatApptID, strPatApptMsgType)

        'UPGRADE_NOTE: Object objMsg may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objMsg = Nothing

    End Function

    '--------------------------------------------------------------------
    ' Private Methods                                                   '
    '--------------------------------------------------------------------

    Private Function VerifyData(ByVal strMessage As String, ByVal strUser As String, ByRef strErrMessage As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 06/06/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Verifies all required data has been provided by the  '
        '              user.                                                '
        'Parameters:  The values to be checked.                             '
        'Returns: Boolean value identifying if all criteria has been        '
        '         satisfied.                                                '
        '--------------------------------------------------------------------

        If Trim(strMessage) = "" Then
            strErrMessage = "Message is required."
            VerifyData = False
            Exit Function
        End If

        If Trim(strUser) = "" Then
            strErrMessage = "User Name is required."
            VerifyData = False
            Exit Function
        End If

        'If we get here, all is well...
        VerifyData = True

    End Function
End Class