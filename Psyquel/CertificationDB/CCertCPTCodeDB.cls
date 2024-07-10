
'--------------------------------------------------------------------
'Class Name: CCertCPTCodeDB                                         '
'Date: 03/08/2000                                                   '
'Author: Rick "Boom Boom" Segura                                    '
'Description:  MTS object designed to host methods associated with  '
'              data affecting the tblCertCPTCode table.             '
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

Public Class CCertCPTCodeDB
    Private Const CLASS_NAME As String = "CCertCPTCodeDB"
    Private Const TABLE_NAME As String = "tblCertCPTCode"

    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property

    Public Function Insert(ByVal lngCertID As Long, ByVal strCPTCode As String, ByVal lngCertUnitID As Long, ByVal strUserName As String) As Long
        '--------------------------------------------------------------------
        'Date: 03/08/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Inserts a single record into the tblCertCPTCode      '
        '               table                                               '
        'Parameters: lngCertID - ID of cert to link CPT Code with           '
        '            strCPTCode - CPT Code to associate with cert           '
        '            lngCertUnitID - Cert Unit to associate with pair       '
        '            strUserName - Name of user executing insert 
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
            .CommandText = "uspInsCertCPTCode"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@CertID", adInteger, adParamInput, , lngCertID))
            .Parameters.Append(.CreateParameter("@CPTCode", adVarChar, adParamInput, 10, strCPTCode))
            .Parameters.Append(.CreateParameter("@CertUnitID", adInteger, adParamInput, , lngCertUnitID))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Assign the connection to the Command object and execute the stored procedure
        cmd.ActiveConnection = cnn
        cmd.Execute() ', , adExecuteNoRecords

        'Check the ErrorNum parameter before deciding to commit the transaction
        If (cmd.Parameters("@ErrorNum").Value.varValue.IsNullorEmpty()) Then
            lngErrNum = vbObjectError
            strErrMsg = "Procedure uspInsCertCPTCode has failed."
            Exit Function
        End If

        If cmd.Parameters("@ErrorNum").Value <> 0 Then
            Exit Function
        End If

        Insert = cmd.Parameters("@ID").Value

        cnn = Nothing
        cmd = Nothing


    End Function


    Public Sub DeleteByUnitCode(ByVal lngCertUnitID As Long, ByVal strCPTCode As String)
        '--------------------------------------------------------------------
        'Date: 10/30/2000                                                   '
        'Author: Dave Richkun
        'Description:  Deletes rows from tblCertCPTCode and tblCertUnit where
        '              lngCertUnitID is referenced
        'Parameters: lngCertUnitID - ID of CertUnitID to delete
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        cnn = New ADODB.Connection
        cmd = New ADODB.Command

        'Create the parameter objects
        With cmd
            .CommandText = "uspDelCertCPTByUnitCode"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@CertUnitID", adInteger, adParamInput, , lngCertUnitID))
            .Parameters.Append(.CreateParameter("@CPTCode", adVarChar, adParamInput, 10, strCPTCode))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Open the database connection.
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
