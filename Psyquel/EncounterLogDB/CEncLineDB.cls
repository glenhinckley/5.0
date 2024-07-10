
'Class Name: CEncLineDB
'Date: 06/15/2017
'Author: Duane C Orth
'Denis J Finkel
'Description:  MTS object designed to host methods associated with
'              data affecting the tblProviderDiagnosis table.
'--------------------------------------------------------------------
' Revision History:
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

Public Class CEncLineDB

    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property




    Private Const CLASS_NAME As String = "CEncLineDB"
    Private Const TABLE_NAME As String = "tblEncDetail"

    Public Function FetchByID(ByVal lngEncDetailID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 06/15/2017
        'Author: Duane C Orth
        'Description:  Returns a row from the tblEncDetail table associated
        '              with a specific EncDetailID.
        'Parameters: lngEncDetailID - ID of the Encounter Detail record to retrieve.
        'Returns: Recordset of Provider Diagnosis's
        '--------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim strSQL As String



        'Prepare the SQL statement.
        strSQL = "SELECT "
        strSQL = strSQL & "*"
        strSQL = strSQL & " FROM "
        strSQL = strSQL & TABLE_NAME
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & "fldEncDetailID = " & lngEncDetailID

        strSQL = strSQL & " ORDER BY "
        strSQL = strSQL & "fldLine, "
        strSQL = strSQL & "fldCPTCode "

        'Instantiate the Recordset.
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = adUseClient

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Populate the recordset
        rstSQL.Open(strSQL, cnnSQL, adOpenForwardOnly, adLockReadOnly, adCmdText + adAsyncFetch)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing
        FetchByID = rstSQL
        cnnSQL.Close()
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        FetchByID = rstSQL
        cnnSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function

    Public Function FetchByEncID(ByVal lngELID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 06/15/2017
        'Author: Duane C Orth
        'Description:  Returns rows from the tblEncDetail table associated
        '              with a specific lngELID.
        'Parameters: lngELID - id of the Encounter to return records for
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim rs As ADODB.Recordset
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        rs = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelEncDetailByEncID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ELID", adInteger, adParamInput, , lngELID))
            .Parameters.Append(.CreateParameter("@IncludeDisabled", adVarChar, adParamInput, 1, IIf(blnIncludeDisabled = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Execute the stored procedure
        rs.CursorLocation = adUseClient
        rs.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchByEncID = rs

        'Disconnect the recordset
        cmd.ActiveConnection = Nothing
        cmd = Nothing
        rs.ActiveConnection = Nothing
        cnn = Nothing

        'Signal successful completion

        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        cmd = Nothing
        rs = Nothing
        cnn = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function

    Public Function Insert(ByVal lngELID As Long, ByVal lngLine As Long, ByVal lngClinicID As Long, ByVal dtDOS As Date, _
                ByVal strCPTCode As String, ByVal dblFee As Double, ByVal dblUnits As Double, ByVal strDiagCode As String, _
                ByVal strModifier1 As String, ByVal strModifier2 As String, _
                ByVal strAddOnCPTCode As String, ByVal dblAddOnFee As Double, ByVal dblAddOnUnits As Double, _
                ByVal strAddOnMod1 As String, ByVal strAddOnMod2 As String, _
                ByVal strAddOnSecCPTCode As String, ByVal dblAddOnSecFee As Double, ByVal dblAddOnSecUnits As Double, _
                ByVal strAddOnSecMod1 As String, ByVal strAddOnSecMod2 As String, _
                ByVal strAddedBy As String) As Long
        '--------------------------------------------------------------------
        'Date: 06/15/2017
        'Author: Duane C Orth
        'Description:  Inserts rows into Encounter Line Item Detail table as needed.
        'Parameters:
        '
        'Returns: Encounter Line ID of the data inserted.
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim prmSQL As ADODB.Parameter



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspInsEncDetailLine"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@EncDetailID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@EncounterLogID", adInteger, adParamInput, , lngELID))
            .Parameters.Append(.CreateParameter("@Line", adInteger, adParamInput, , lngLine))
            .Parameters.Append(.CreateParameter("@ClinicID", adInteger, adParamInput, , lngClinicID))
            .Parameters.Append(.CreateParameter("@DOS", adDBTimeStamp, adParamInput, , dtDOS))
            .Parameters.Append(.CreateParameter("@CPTCode", adVarChar, adParamInput, 10, strCPTCode))
            .Parameters.Append(.CreateParameter("@Fee", adDecimal, adParamInput, , dblFee))
            .Parameters.Append(.CreateParameter("@Units", adDouble, adParamInput, , dblUnits))
            .Parameters.Append(.CreateParameter("@DSMIVCode", adVarChar, adParamInput, 10, strDiagCode))
            .Parameters.Append(.CreateParameter("@Modifier1", adVarChar, adParamInput, 2, UCase(strModifier1)))
            .Parameters.Append(.CreateParameter("@Modifier2", adVarChar, adParamInput, 2, UCase(strModifier2)))
            .Parameters.Append(.CreateParameter("@AddOnCPTCode", adVarChar, adParamInput, 10, strAddOnCPTCode))
            .Parameters.Append(.CreateParameter("@AddOnFee", adDecimal, adParamInput, , dblAddOnFee))
            .Parameters.Append(.CreateParameter("@AddOnUnits", adDouble, adParamInput, , dblAddOnUnits))
            .Parameters.Append(.CreateParameter("@AddOnMod1", adVarChar, adParamInput, 2, UCase(strAddOnMod1)))
            .Parameters.Append(.CreateParameter("@AddOnMod2", adVarChar, adParamInput, 2, UCase(strAddOnMod2)))
            .Parameters.Append(.CreateParameter("@AddOnSecCPTCode", adVarChar, adParamInput, 10, strAddOnSecCPTCode))
            .Parameters.Append(.CreateParameter("@AddOnSecFee", adDecimal, adParamInput, , dblAddOnSecFee))
            .Parameters.Append(.CreateParameter("@AddOnSecUnits", adDouble, adParamInput, , dblAddOnSecUnits))
            .Parameters.Append(.CreateParameter("@AddOnSecMod1", adVarChar, adParamInput, 2, UCase(strAddOnSecMod1)))
            .Parameters.Append(.CreateParameter("@AddOnSecMod2", adVarChar, adParamInput, 2, UCase(strAddOnSecMod2)))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strAddedBy))
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

        Insert = cmdSQL.Parameters(0).Value

        'Close the connection and free all resources
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        cmdSQL = Nothing
        cnnSQL = Nothing
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)

    End Function
    Public Function Update(ByVal lngEncDetailID As Long, ByVal lngELID As Long, ByVal lngLine As Long, ByVal lngClinicID As Long, ByVal dtDOS As Date, _
                ByVal strCPTCode As String, ByVal dblFee As Double, ByVal dblUnits As Double, ByVal strDiagCode As String, _
                ByVal strModifier1 As String, ByVal strModifier2 As String, _
                ByVal strAddOnCPTCode As String, ByVal dblAddOnFee As Double, ByVal dblAddOnUnits As Double, _
                ByVal strAddOnMod1 As String, ByVal strAddOnMod2 As String, _
                ByVal strAddOnSecCPTCode As String, ByVal dblAddOnSecFee As Double, ByVal dblAddOnSecUnits As Double, _
                ByVal strAddOnSecMod1 As String, ByVal strAddOnSecMod2 As String, _
                ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 06/15/2017
        'Author: Duane C Orth
        'Description:  Inserts rows into Encounter Line Item Detail table as needed.
        'Parameters:
        '
        'Returns: Encounter Line ID of the data inserted.
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim prmSQL As ADODB.Parameter



        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdEncDetailLine"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@EncDetailID", adInteger, adParamInput, , lngEncDetailID))
            .Parameters.Append(.CreateParameter("@EncounterLogID", adInteger, adParamInput, , lngELID))
            .Parameters.Append(.CreateParameter("@Line", adInteger, adParamInput, , lngLine))
            .Parameters.Append(.CreateParameter("@ClinicID", adInteger, adParamInput, , lngClinicID))
            .Parameters.Append(.CreateParameter("@DOS", adDBTimeStamp, adParamInput, , dtDOS))
            .Parameters.Append(.CreateParameter("@CPTCode", adVarChar, adParamInput, 10, strCPTCode))
            .Parameters.Append(.CreateParameter("@Fee", adDecimal, adParamInput, , dblFee))
            .Parameters.Append(.CreateParameter("@Units", adDouble, adParamInput, , dblUnits))
            .Parameters.Append(.CreateParameter("@DSMIVCode", adVarChar, adParamInput, 10, strDiagCode))
            .Parameters.Append(.CreateParameter("@Modifier1", adVarChar, adParamInput, 2, UCase(strModifier1)))
            .Parameters.Append(.CreateParameter("@Modifier2", adVarChar, adParamInput, 2, UCase(strModifier2)))
            .Parameters.Append(.CreateParameter("@AddOnCPTCode", adVarChar, adParamInput, 10, strAddOnCPTCode))
            .Parameters.Append(.CreateParameter("@AddOnFee", adDecimal, adParamInput, , dblAddOnFee))
            .Parameters.Append(.CreateParameter("@AddOnUnits", adDouble, adParamInput, , dblAddOnUnits))
            .Parameters.Append(.CreateParameter("@AddOnMod1", adVarChar, adParamInput, 2, UCase(strAddOnMod1)))
            .Parameters.Append(.CreateParameter("@AddOnMod2", adVarChar, adParamInput, 2, UCase(strAddOnMod2)))
            .Parameters.Append(.CreateParameter("@AddOnSecCPTCode", adVarChar, adParamInput, 10, strAddOnSecCPTCode))
            .Parameters.Append(.CreateParameter("@AddOnSecFee", adDecimal, adParamInput, , dblAddOnSecFee))
            .Parameters.Append(.CreateParameter("@AddOnSecUnits", adDouble, adParamInput, , dblAddOnSecUnits))
            .Parameters.Append(.CreateParameter("@AddOnSecMod1", adVarChar, adParamInput, 2, UCase(strAddOnSecMod1)))
            .Parameters.Append(.CreateParameter("@AddOnSecMod2", adVarChar, adParamInput, 2, UCase(strAddOnSecMod2)))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

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

    Public Sub Delete(ByVal lngEncDetailID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 06/15/2017
        'Author: Duane C Orth
        'Description: 'Disables' selected record in tblEncDetail table.
        'Parameters: lngEncDetailID - ID of relationship to be Disables.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim cmd As ADODB.Command
        Dim cnn As ADODB.Connection



        cnn = New ADODB.Connection
        cmd = New ADODB.Command

        'Create the parameter objects
        With cmd
            .CommandText = "uspDelEncDetail"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@EncDetailID", adInteger, adParamInput, , lngEncDetailID))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0))

            'Assign the connection to the Command object and execute the stored procedure
            cnn.Open(_ConnectionString)
            .ActiveConnection = cnn
            .Execute() '()
            cnn = Nothing

            'Check the ErrorNum parameter before deciding to commit the transaction
            If .Parameters("@ErrorNum").Value <> 0 Then
                Exit Sub
            End If
            cmd = Nothing
        End With

        cnn = Nothing
        cmd = Nothing



    End Sub


End Class

