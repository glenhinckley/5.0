
'--------------------------------------------------------------------
'Class Name: CCheckInDB                                             '
'Date: 11/16/2000                                                   '
'Author: Chris Dereadt                                              '
'Description:  MTS object designed to host methods associated with  '
'              data affecting the CheckIn Process.                  '
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

Public Class CCheckInDB
    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CCheckInDB"

    '--------------------------------------------------------------------
    ' Public Methods    +++++++++++++++++++++++++++++++++++++++++++++++++
    '--------------------------------------------------------------------

    Public Function Fetch(ByVal lngID As Long, ByVal dteStartDate As Date, _
                          ByVal dteEndDate As Date) As ADODB.Recordset

        '--------------------------------------------------------------------
        'Date: 11/16/2000                                                   '
        'Author: Chris Dereadt                                              '
        'Description:  Fetch CheckIn information                            '
        'Parameters:  lngID - User ID or Provider/Office Manager            '
        '             dteStartDate - Desired start date of all appointment  '
        '             dteEndDate - Desired end date of all appointment      '
        'Returns:  ADODB Recordset                                          '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '--------------------------------------------------------------------
        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection
        Dim varParamErr As Object



        rst = New ADODB.Recordset
        cnn = New ADODB.Connection
        cmd = New ADODB.Command

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelCheckInInfo"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@StartDate", adDBTimeStamp, adParamInput, , dteStartDate))
            .Parameters.Append(.CreateParameter("@EndDate", adDBTimeStamp, adParamInput, , dteEndDate))
            '.Parameters.Append .CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0)
        End With

        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        Fetch = rst

        ' Clean House
        rst = Nothing
        cnn = Nothing
        cmd = Nothing

        ' Signal Completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        rst = Nothing
        cnn = Nothing
        cmd = Nothing
        If Err.Number <> 0 Then
            'err.raise(Err.Number, Err.Source, Err.Description)
        Else
            'RaiseError(Err, varParamErr, CLASS_NAME)
        End If

    End Function

    Public Function FetchAmountOwed(ByVal lngPatRPPlanID As Long, ByVal lngPatientID As Long, _
                          ByVal lngRPID As Long, ByVal lngPlanID As Long, ByVal lngProviderID As Long, _
                          ByVal strCPTCode As String, ByVal strCPTCode2 As String, ByVal lngProviderCredential As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 11/21/2000                                                   '
        'Author: Morgan Travis                                              '
        'Description:  Fetch CheckIn Amount owed information                '
        'Parameters:                                                        '
        'Returns:  ADODB Recordset                                          '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '--------------------------------------------------------------------
        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection
        Dim varParamErr As Object



        rst = New ADODB.Recordset
        cnn = New ADODB.Connection
        cmd = New ADODB.Command

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelCheckInAmountInfo"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatRPPlanID", adInteger, adParamInput, , lngPatRPPlanID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@RPID", adInteger, adParamInput, , lngRPID))
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@CPTCode", adVarChar, adParamInput, 10, strCPTCode))
            .Parameters.Append(.CreateParameter("@CPTCode2", adVarChar, adParamInput, 10, strCPTCode2))
            .Parameters.Append(.CreateParameter("@ProviderCredential", adInteger, adParamInput, , lngProviderCredential)) '.Parameters.Append .CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0)
        End With




        ' App.LogEvent(lngPatRPPlanID & "," & lngPatientID & "," & lngRPID & "," & lngPlanID & "," & lngProviderID & "," & strCPTCode & "," & strCPTCode2 & "," & lngProviderCredential, 4)




        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchAmountOwed = rst

        ' Clean House
        rst = Nothing
        cnn = Nothing
        cmd = Nothing

        ' Signal Completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        rst = Nothing
        cnn = Nothing
        cmd = Nothing
        If Err.Number <> 0 Then
            'err.raise(Err.Number, Err.Source, Err.Description)
        Else
            'RaiseError(Err, varParamErr, CLASS_NAME)
        End If

    End Function

    Public Function FetchCopayInfo(ByVal lngPatientID As Long, ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/12/2001                                                   '
        'Author: Chris Dereadt                                              '
        'Description:  Fetch Copay information                              '
        'Parameters:  lngPatientID - Id for the patient                     '
        'Returns:  ADODB Recordset                                          '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '--------------------------------------------------------------------
        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection
        Dim varParamErr As Object



        rst = New ADODB.Recordset
        cnn = New ADODB.Connection
        cmd = New ADODB.Command

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelCopayAmount"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            '.Parameters.Append .CreateParameter("@Cred", adInteger, adParamInput, , lngCredential)
            '.Parameters.Append .CreateParameter("@Sever", adVarChar, adParamInput, 1, strSeverity)
            '.Parameters.Append .CreateParameter("@Cpt", adVarChar, adParamInput, 10, strCPTCode)
            '.Parameters.Append .CreateParameter("@VisNo", adInteger, adParamInput, , lngVisits)
        End With

        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchCopayInfo = rst

        ' Clean up, 'cause I'm gong home

        rst = Nothing
        cnn = Nothing
        cmd = Nothing

        ' Signal Completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        rst = Nothing
        cnn = Nothing
        cmd = Nothing
        If Err.Number <> 0 Then
            'err.raise(Err.Number, Err.Source, Err.Description)
        Else
            'RaiseError(Err, varParamErr, CLASS_NAME)
        End If

    End Function

    Public Function FetchCopayData(ByVal lngProviderID As Long, ByVal dteStartDate As Date, _
                          ByVal dteEndDate As Date, ByVal lngPatientID As Long) As Collection
        '--------------------------------------------------------------------
        'Date: 02/12/2001                                                   '
        'Author: Chris Dereadt                                              '
        'Description:  Fetch Copay information                              '
        'Parameters:  lngPatientID - Id for the patient                     '
        'Returns:  ADODB Recordset                                          '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '--------------------------------------------------------------------
        'Declare connection, returned recordset, and command
        Dim cnn As ADODB.Connection
        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim col As New Collection 'Declare Collection of recordsets
        Dim intCtr As Integer
        Dim varParamErr As Object



        'Instantiation
        cmd = New ADODB.Command

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelCheckInData"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@StartDate", adDBTimeStamp, adParamInput, , dteStartDate))
            .Parameters.Append(.CreateParameter("@EndDate", adDBTimeStamp, adParamInput, , dteEndDate))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
        End With

        'Aquire DB Connection
        cnn = New ADODB.Connection 'Instantiation
        cnn.Open(_ConnectionString)    'Open DB Connection
        cmd.ActiveConnection = cnn

        'Set Cursor and execution
        rst = New ADODB.Recordset   'Instantiation
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        'Test each recordset and add it to the collection
        intCtr = 0
        While Not (rst Is Nothing)
            intCtr = intCtr + 1
            col.Add(rst, CStr(intCtr))
            rst = rst.NextRecordset
        End While

        'De-Allocation
        cmd.ActiveConnection = Nothing

        FetchCopayData = col  'set return value

        ' Clean up, 'cause I'm gong home
        cnn = Nothing
        cmd = Nothing

        ' Signal Completion


        Exit Function

        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()

        cmd.ActiveConnection = Nothing
        rst.ActiveConnection = Nothing
        rst.Close()
        cnn.Close()

        'De-alloction for objects
        rst = Nothing
        cnn = Nothing
        cmd = Nothing

        'If Err.Number <> 0 Then
        'err.raise(Err.Number, Err.Source, Err.Description)
        'Else
        '     'RaiseError(Err, varParamErr, CLASS_NAME)
        'End If

    End Function

End Class
