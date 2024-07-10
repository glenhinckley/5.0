
'--------------------------------------------------------------------
'Class Name: CCertProviderDB                                        '
'Date: 03/07/2000                                                   '
'Author: Rick "Boom Boom" Segura                                    '
'Description:  MTS object designed to host methods associated with  '
'              data affecting the tblCertProvider table.            '
'--------------------------------------------------------------------
'Revision History:
'   R001: 10/26/2000 Richkun - Added Update() method
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

Public Class CCertProviderDB
    Private Const CLASS_NAME As String = "CCertProviderDB"
    Private Const TABLE_NAME As String = "tblCertProvider"
    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property

    Public Function Insert(ByVal lngCertID As Long, ByVal lngProviderID As Long) As Long
        '--------------------------------------------------------------------
        'Date: 03/07/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Inserts a single record into the tblCertProvider     '
        '               table                                               '
        'Parameters: lngCertID - ID of cert to link provider with           '
        '            lngProviderID - ID of provider to link cert with       '
        'Returns: ID of record just inserted(on success)                    '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection
        Dim lngErrNum As Long
        Dim strErrMsg As String



        cnn = New ADODB.Connection
        cmd = New ADODB.Command

        cnn.Open(_ConnectionString)

        'Create the parameter objects
        With cmd
            .CommandText = "uspInsCertProvider"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@CertID", adInteger, adParamInput, , lngCertID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        cmd.ActiveConnection = cnn
        cmd.Execute() ', , adExecuteNoRecords

        'Check the ErrorNum parameter before deciding to commit the transaction
        If (cmd.Parameters("@ErrorNum").Value.IsNullorEmpty) Then
            lngErrNum = vbObjectError
            strErrMsg = "Procedure uspInsCertProvider has failed."
            Exit Function
        End If

        If cmd.Parameters("@ErrorNum").Value <> 0 Then
            Exit Function
        End If

        Insert = cmd.Parameters("@ID").Value

        cmd = Nothing
        cnn = Nothing

    End Function


    Public Sub Update(ByVal lngCertProviderID As Long, ByVal lngProviderID As Long)
        '--------------------------------------------------------------------
        'Date: 10/26/2000                                                   '
        'Author: Dave Richkun
        'Description:  Updates a single record into the tblCertProvider table                                               '
        'Parameters: lngCertProviderID - ID of the row in the table to update
        '            lngProviderID - ID of provider associated with cert.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '  R001 - Created
        '--------------------------------------------------------------------

        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        cnn = New ADODB.Connection
        cmd = New ADODB.Command

        'Create the parameter objects
        With cmd
            .CommandText = "uspUpdCertProvider"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@CertProviderID", adInteger, adParamInput, , lngCertProviderID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        cnn.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmd.ActiveConnection = cnn
        cmd.Execute() ', , adExecuteNoRecords

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        cmd = Nothing
        cnn = Nothing



    End Sub
    Public Sub Delete(ByVal lngCertID As Long, ByVal lngProviderID As Long)
        '--------------------------------------------------------------------
        'Date: 10/30/2001
        'Author: Eric Pena
        'Description:  Deletes a single record into the tblCertProvider table                                               '
        'Parameters: lngCertProviderID - ID of the row in the table to update
        '            lngProviderID - ID of provider associated with cert.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '  R001 - Created
        '--------------------------------------------------------------------

        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        cnn = New ADODB.Connection
        cmd = New ADODB.Command

        'Create the parameter objects
        With cmd
            .CommandText = "uspDelCertProvider"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@CertID", adInteger, adParamInput, , lngCertID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        cnn.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmd.ActiveConnection = cnn
        cmd.Execute() ', , adExecuteNoRecords

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmd.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        cmd = Nothing
        cnn = Nothing


    End Sub

End Class


