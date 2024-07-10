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
Public Class CCertAuditDB

    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property


    Public Function Insert(ByVal lngCertID As Long, ByVal strUserName As String, ByVal strNote As String) As Long
        '--------------------------------------------------------------------
        'Date: 05/03/2001                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Inserts a single record into the tblCertAudit table  '
        'Parameters: lngCertID - ID of cert to link CPT Code with           '
        '            strUserName - Name of user executing insert        '
        'Returns: ID of record just inserted(on success)                    '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection
        Dim lngErr As Long



        cnn = New ADODB.Connection
        cmd = New ADODB.Command

        cnn.Open(_ConnectionString)

        'Create the parameter objects
        With cmd
            .CommandText = "uspInsCertAudit"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@CertID", adInteger, adParamInput, , lngCertID))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@Note", adVarChar, adParamInput, 1000, Left(strNote, 1000)))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))

            'Assign the connection to the Command object and execute the stored procedure
            .ActiveConnection = cnn
            .Execute() ', , adExecuteNoRecords

            Insert = .Parameters("@ID").Value

            'Check the ErrorNum parameter before deciding to commit the transaction
            If .Parameters("@ErrorNum").Value <> 0 Then
                lngErr = .Parameters("@ErrorNum").Value
                Exit Function
            End If

        End With

        cnn = Nothing
        cmd = Nothing


    End Function

    Public Function FetchCertAuditByCert(ByVal lngCertID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 05/03/2001                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Retrieves recordset of audits for the given cert     '
        'Parameters: lngCertID - ID of Cert to fetch certs for              '
        'Returns: Recordset of audit trail                                  '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection
        Dim rst As ADODB.Recordset



        cnn = New ADODB.Connection
        cmd = New ADODB.Command
        rst = New ADODB.Recordset

        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelCertAuditByCert"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@CertID", adInteger, adParamInput, , lngCertID))
        End With

        '  Instantiate recordset and populate
        rst.CursorLocation = adUseClient
        rst.Open()

        rst.ActiveConnection = Nothing
        cnn.Close()
        cnn = Nothing
        cmd = Nothing

        FetchCertAuditByCert = rst




    End Function

End Class
