
'--------------------------------------------------------------------
'Class Name: CProviderSpecialtyDB
'Date: 09/25/2001
'Author: Dave Richkun
'Denis J Finkel
'Description:  COM+ object designed to host methods associated with
'              data affecting the tblProviderSpecialty table.
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

Public Class CProviderSpecialtyDB
    Private Const CLASS_NAME As String = "CProviderSpecialtyDB"
    Private Const TABLE_NAME As String = "tblProviderSpecialty"

    Private _ConnectionString As String = String.Empty



    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property



    Public Function Insert(ByVal lngProviderID As Long, ByVal lngSpecialtyID As Long) As Long
        '--------------------------------------------------------------------
        'Date: 09/25/2001
        'Author: Dave Richkun
        'Description:  Inserts a row into the tblProviderSpecialty table utilizing
        '              a stored procedure.
        'Parameters: lngProviderID - internal provider ID
        '            lngSpecialtyID - ID of specialty
        'Returns: new record ID
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim prmSQL As ADODB.Parameter



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspInsProviderSpecialty"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ProvSpecID", adInteger, adParamReturnValue, , 0))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@SpecialtyID", adInteger, adParamInput, , lngSpecialtyID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check for errors
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Function
        End If

        Insert = cmdSQL.Parameters("@ProvSpecID").Value

        'Close the connection and free all resources
        cmdSQL.ActiveConnection = Nothing
        cnnSQL.Close()
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        cmdSQL = Nothing
        cnnSQL = Nothing
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)

    End Function

    Public Sub Deleted(ByVal blnDisable As Boolean, ByVal lngProviderID As Long, _
                       ByVal lngSpecialtyID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 10/01/2001
        'Author: Dave Richkun
        'Description:  Deletes or Undeletes a row in tblProviderSpecialty
        'Parameters: blnDisable - Identifies if row will be deleted (True) or Undeleted (False)
        '            lngProviderID - ID of Provider
        '            lngSpecialtyID - ID of specialty
        '            strUserName - Name of user making the change
        'Returns: Null
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim prmSQL As ADODB.Parameter



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspDisableProviderSpecialty"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@DisableYN", adVarChar, adParamInput, 1, IIf(blnDisable = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@SpecialtyID", adInteger, adParamInput, , lngSpecialtyID))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check for errors
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection and free all resources
        cmdSQL.ActiveConnection = Nothing
        cnnSQL.Close()
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        cmdSQL = Nothing
        cnnSQL = Nothing
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)

    End Sub

End Class
