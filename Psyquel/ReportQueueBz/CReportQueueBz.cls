
'Class Name: CReportQueueBz
'Date: 04/26/2001
'Author: Eric Pena
'Denis J Finkel
'Description:  MTS business object designed to   methods associated with the CReportQueueDB class.
'--------------------------------------------------------------------
' Revision History:
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

Public Class CReportQueueBz


    Private _ConnectionString As String = String.Empty



    Private Const CLASS_NAME As String = "CReportQueueBz"
    Private Const TABLE_NAME As String = "tblReportQueue"
    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property

    Public Function Insert(ByVal strRptName As String, ByVal strExportType As String, _
                           ByVal strRptParams As String, ByVal strDescription As String, _
                           ByVal lngUserID As Long, ByVal strUserName As String) As Long
        '--------------------------------------------------------------------
        'Date: 04/26/2001
        'Author: Eric Pena
        'Description:  Inserts a row into the tblReportQueue table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------

        Dim objRQ As New ReportQueueDB.CReportQueueDB
        Dim strErrMessage As String
        objRQ.ConnectionString = _ConnectionString


        If Not VerifyData(strRptName, strRptParams, strExportType, strErrMessage) Then
            Exit Function
        End If

        Insert = objRQ.Insert(strRptName, strExportType, strRptParams, strDescription, lngUserID, strUserName)

        'Signal successful completion


        objRQ = Nothing

        Exit Function


        'Signal incompletion and raise the error to the  ing environment.  The
        'condition handles custom business rule checks we may have established.
        objRQ = Nothing
        'cmdSQL.Execute()
        If Err.Number = 0 Then
            'err.raise(vbObjectError, CLASS_NAME, strErrMessage)
        Else
            'err.raise(Err.Number, Err.Source, Err.Description)
        End If
    End Function

    Public Sub Process(ByVal lngID As Long)
        '--------------------------------------------------------------------
        'Date: 04/26/2001
        'Author: Eric Pena
        'Description:  Updates a row in the tblReportQueue  table utilizing a stored procedure.
        'Parameters: lngID - The ID of the row that will be updated
        'Returns: Null
        '--------------------------------------------------------------------
        Dim objRQ As New ReportQueueDB.CReportQueueDB
        Dim strErrMessage As String

        objRQ.ConnectionString = _ConnectionString

        objRQ.Process(lngID)

        'Signal successful completion


        objRQ = Nothing

    End Sub

    Public Function Delete(ByVal lngRptID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 04/26/2001
        'Author: Eric Pena
        'Description:  Either flags a row in the tblReportQueue table marking the row as deleted or physi y deletes the row.
        '               If the row has already been procesed, the fldProcessedYN flag will be set,
        '                   otherwise the row will be physi y deleted.
        'Parameters: lngID - ID of the row in the table whose value will be updated.
        '            strUserName - Login name of the user responsible for marking the row as deleted.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim objRQ As New ReportQueueDB.CReportQueueDB
        objRQ.ConnectionString = _ConnectionString

        objRQ.Delete(lngRptID, strUserName)

        'Signal successful completion

        objRQ = Nothing

    End Function

    Public Function Remove(ByVal lngRptID As Long)
        '--------------------------------------------------------------------
        'Date: 04/26/2001
        'Author: Eric Pena
        'Description:  Flags a row in the tblReportQueue  table marking the row as removed.
        'Parameters: lngID - ID of the row in the table whose value will be updated.
        '            strUserName - Login name of the user responsible for marking the row as deleted.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim objRQ As New ReportQueueDB.CReportQueueDB
        objRQ.ConnectionString = _ConnectionString

        objRQ.Remove(lngRptID)


    End Function

    Public Function FetchByUser(ByVal lngUserID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 04/26/2001
        'Author: Eric Pena
        'Description:  Retrieves records from the tblReportQueue table utilizing a stored procedure.
        'Parameters: None
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim rstSQL As New ADODB.Recordset
        Dim objRQ As New ReportQueueDB.CReportQueueDB
        objRQ.ConnectionString = _ConnectionString

        rstSQL = objRQ.FetchByUser(lngUserID)
        FetchByUser = rstSQL

        'Signal successful completion

        objRQ = Nothing

    End Function


    Public Function FetchByUserName(ByVal strUserName As String) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/02/2003
        'Author: Dave Richkun
        'Description:  Retrieves undeleted report records from tblReportQueue
        '              added by a specific user.
        'Parameters: strUserName - Name of user who added the report
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim rstSQL As New ADODB.Recordset
        Dim objRQ As New ReportQueueDB.CReportQueueDB
        objRQ.ConnectionString = _ConnectionString


        rstSQL = objRQ.FetchByUserName(strUserName)
        FetchByUserName = rstSQL

        'Signal successful completion

        objRQ = Nothing


    End Function

    Public Function FetchByID(ByVal lngReportID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 04/03/2003
        'Author: Dave Richkun
        'Description:  Retrieves a single report record from the database
        'Parameters: lngReportID - ID Of report to be retrieved
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim rst As New ADODB.Recordset
        Dim objRQ As New ReportQueueDB.CReportQueueDB

        objRQ.ConnectionString = _ConnectionString

        rst = objRQ.FetchByID(lngReportID)
        FetchByID = rst

        'Signal successful completion

        objRQ = Nothing


    End Function

    Public Function FetchQueuedByUser(ByVal lngUserID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 04/26/2001
        'Author: Eric Pena
        'Description:  Retrieves records from the tblReportQueue table utilizing a stored procedure.
        'Parameters: None
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim rstSQL As New ADODB.Recordset
        Dim objRQ As New ReportQueueDB.CReportQueueDB
        objRQ.ConnectionString = _ConnectionString

        rstSQL = objRQ.FetchQueuedByUser(lngUserID)
        FetchQueuedByUser = rstSQL

        'Signal successful completion

        objRQ = Nothing

    End Function

    Public Function FetchQueued() As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 04/26/2001
        'Author: Eric Pena
        'Description:  Retrieves records from the tblReportQueue table utilizing a stored procedure.
        'Parameters: None
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim rstSQL As New ADODB.Recordset
        Dim objRQ As New ReportQueueDB.CReportQueueDB

        objRQ.ConnectionString = _ConnectionString

        rstSQL = objRQ.FetchQueued()
        FetchQueued = rstSQL

        'Signal successful completion

        objRQ = Nothing
 
    End Function

    Public Function FetchPendingRemove() As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 04/26/2001
        'Author: Eric Pena
        'Description:  Retrieves records from the tblReportQueue table utilizing a stored procedure.
        'Parameters: None
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim rstSQL As New ADODB.Recordset
        Dim objRQ As New ReportQueueDB.CReportQueueDB

        objRQ.ConnectionString = _ConnectionString

        rstSQL = objRQ.FetchPendingRemove()
        FetchPendingRemove = rstSQL

        'Signal successful completion

        objRQ = Nothing

    End Function

    Private Function VerifyData(ByVal strRptName As String, ByVal strRptParams As String, ByVal strExportType As String, ByRef strErrMessage As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 04/26/2001
        'Author: Eric Pena
        'Description:  Verifies all required data has been correctly provided by the user.
        'Parameters:  The values to be checked.
        'Returns: Boolean value identifying if all criteria has been satisfied.
        '--------------------------------------------------------------------

        If Trim(strRptName) = "" Then
            strErrMessage = "Report Name is required."
            VerifyData = False
            Exit Function
        End If

        If InStr("WXRHTPEVI", strExportType) < 1 Then
            strErrMessage = "Invalid Export Type."
            VerifyData = False
            Exit Function
        End If

        'If we get here, all is well...
        VerifyData = True
    End Function

End Class
