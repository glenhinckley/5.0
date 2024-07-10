
'------------------------------------------------------------------------------------
'Class Name: CCheckDB
'Date: 09/25/00
'Author: Eric Pena
'Denis J Finkel
'Description:   MTS object designed to host methods associated with data affecting tblPayerCheck and tblCheckUsage tables.
'------------------------------------------------------------------------------------
'Revision History:
'  R001: Richkun 09/14/2001 - Added FetchPayerChecks() method
'------------------------------------------------------------------------------------
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

Public Class CCheckDB
    Private _ConnectionString As String = String.Empty


    Private Const CLASS_NAME As String = "CCheckDB"
    Private Const TABLE_NAME As String = "tblPayerCheck"

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property





    Public Function Insert(ByVal lngInsuranceID As Long, _
                            ByVal strCheckNum As String, _
                            ByVal strDepositNum As String, _
                            ByVal dteCheckDate As Date, _
                            ByVal dteReceiveDate As Date, _
                            ByVal curTotalAmount As Decimal, _
                            ByVal strAddedBy As String, _
                            ByVal lngProviderID As Long, _
                            ByVal lngGroupID As Long, _
                            Optional ByVal blnPosted As Boolean = False, _
                            Optional ByVal strNotes As String = "", _
                            Optional ByVal blnElectronic As Boolean = False) As Long
        '------------------------------------------------------------------------------------
        'Date: 09/25/00
        'Author: Eric Pena
        'Description:   Inserts a row into the tblPayerCheck utilizing a stored procedure.
        'Parameters:    Each parameter identifies the column value that will be inserted.
        'Returns:       ID (Primary Key) of the row inserted.
        '------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the command object.
        cmdSQL = New ADODB.Command

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspInsPayerCheck"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@InsuranceID", adInteger, adParamInput, , lngInsuranceID))
            .Parameters.Append(.CreateParameter("@CheckNum", adVarChar, adParamInput, 30, strCheckNum))
            .Parameters.Append(.CreateParameter("@DepNum", adVarChar, adParamInput, 30, strDepositNum))
            .Parameters.Append(.CreateParameter("@CheckDate", adDBTimeStamp, adParamInput, , IIf(dteCheckDate.ToOADate() = 0, DBNull.Value, dteCheckDate)))
            .Parameters.Append(.CreateParameter("@ReceiveDate", adDBTimeStamp, adParamInput, , IIf(dteReceiveDate.ToOADate() = 0, DBNull.Value, dteReceiveDate)))
            .Parameters.Append(.CreateParameter("@TotalAmount", adDecimal, adParamInput, , curTotalAmount))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strAddedBy))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@PGroupID", adInteger, adParamInput, , lngGroupID))
            .Parameters.Append(.CreateParameter("@PostedYN", adVarChar, adParamInput, 1, IIf(blnPosted = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@Notes", adVarChar, adParamInput, 255, strNotes))
            .Parameters.Append(.CreateParameter("@ElectronicYN", adVarChar, adParamInput, 1, IIf(blnElectronic = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        Insert = cmdSQL.Parameters("@ID").Value

        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Function
        End If

        'Close the connection
        cnnSQL.Close()

        'free all resources
        cmdSQL = Nothing
        cnnSQL = Nothing


    End Function
    Public Function Fetch(ByVal lngInsuranceID As Long, ByVal blnOnlyUnreconciled As Boolean) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 09/25/00
        'Author: Eric Pena
        'Description:   Retrieves records from the tblPayerCheck table.
        'Parameters:   blnIncludeDisabled- Optional parameter that identifies if  records flagged as
        '               'Disabled' or 'De-activated' are to be included in the record set. The default value is False
        '
        'Returns: ADODB.Recordset
        '-------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim cmdSQL As ADODB.Command



        rstSQL = New ADODB.Recordset
        cmdSQL = New ADODB.Command
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)
        cmdSQL.ActiveConnection = cnnSQL

        'create the parameter objects
        With cmdSQL
            .CommandText = "uspSelPayerCheck"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PayerID_Plan", adInteger, adParamInput, , lngInsuranceID))
            .Parameters.Append(.CreateParameter("@OnlyUnrec", adVarChar, adParamInput, 1, IIf(blnOnlyUnreconciled, "Y", "N")))
        End With

        'Execute the stored procedure
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        Fetch = rstSQL

        'disconnect the recordset
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        cnnSQL = Nothing

        'Signal successful completion


    End Function

    Public Sub Update(ByVal lngCheckID As Long, _
                        ByVal lngInsuranceID As Long, _
                        ByVal strCheckNum As String, _
                        ByVal strDepositNum As String, _
                        ByVal dteCheckDate As Date, _
                        ByVal dteReceiveDate As Date, _
                        ByVal curTotalAmount As Decimal, _
                        ByVal lngProviderID As Long, _
                        ByVal lngGroupID As Long, _
                        Optional ByVal blnPosted As Boolean = False, _
                        Optional ByVal strNotes As String = "", _
                        Optional ByVal blnElectronic As Boolean = False)
        '------------------------------------------------------------------------------------
        'Date: 09/25/00
        'Author: Eric Pena
        'Description:   Updates a row in the tblPayerCheck table utilizing a stored procedure.
        'Parameters:    Each parameter identifies the column value that will be updated.
        'Returns:       Null
        '-------------------------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdPayerCheck"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngCheckID))
            .Parameters.Append(.CreateParameter("@InsuranceID", adInteger, adParamInput, , lngInsuranceID))
            .Parameters.Append(.CreateParameter("@CheckNum", adVarChar, adParamInput, 30, strCheckNum))
            .Parameters.Append(.CreateParameter("@DepositNum", adVarChar, adParamInput, 30, strDepositNum))
            .Parameters.Append(.CreateParameter("@CheckDate", adDBTimeStamp, adParamInput, , IIf(dteCheckDate = #12:00:00 AM#, DBNull.Value, dteCheckDate)))
            .Parameters.Append(.CreateParameter("@ReceiveDate", adDBTimeStamp, adParamInput, , IIf(dteReceiveDate = #12:00:00 AM#, DBNull.Value, dteReceiveDate)))
            .Parameters.Append(.CreateParameter("@TotalAmount", adDecimal, adParamInput, , curTotalAmount))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@PGroupID", adInteger, adParamInput, , lngGroupID))
            .Parameters.Append(.CreateParameter("@PostedYN", adVarChar, adParamInput, 1, IIf(blnPosted = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@Notes", adVarChar, adParamInput, 255, strNotes))
            .Parameters.Append(.CreateParameter("@ElectronicYN", adVarChar, adParamInput, 1, IIf(blnElectronic = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the command object
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check the SQLErrorNum parameter before deciding to commit the transaction.
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection and free all resources
        cnnSQL.Close()
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


    End Sub

    Public Function FetchByID(ByVal lngID As Long) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 09/25/00
        'Author: Eric Pena
        'Description:   Retrieves records from the tblPayerCheck table.
        'Parameters:   blnIncludeDisabled- Optional parameter that identifies if  records flagged as
        '               'Disabled' or 'De-activated' are to be included in the record set. The default value is False
        '
        'Returns: ADODB.Recordset
        '-------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim cmdSQL As ADODB.Command



        rstSQL = New ADODB.Recordset
        cmdSQL = New ADODB.Command
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)
        cmdSQL.ActiveConnection = cnnSQL

        'create the parameter objects
        With cmdSQL
            .CommandText = "uspSelPayerCheckByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Execute the stored procedure
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        FetchByID = rstSQL

        'disconnect the recordset
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        cnnSQL = Nothing

        'Signal successful completion



    End Function
    Public Function LogCheckUsage(ByVal lngCheckID As String, ByVal lngELID As Long, ByVal curTotalAmount As Decimal, ByVal strAddedBy As String)
        '------------------------------------------------------------------------------------
        'Date: 09/25/00
        'Author: Eric Pena
        'Description:   Inserts a row into the tblCheckUsage utilizing a stored procedure.
        'Parameters:    Each parameter identifies the column value that will be inserted.
        'Returns:       ID (Primary Key) of the row inserted.
        '------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the command object.
        cmdSQL = New ADODB.Command

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspInsCheckUsage"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@CheckID", adInteger, adParamInput, , lngCheckID))
            .Parameters.Append(.CreateParameter("@EncounterLogID", adInteger, adParamInput, , lngELID))
            .Parameters.Append(.CreateParameter("@Amount", adDecimal, adParamInput, , curTotalAmount))
            .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 30, strAddedBy))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()


        'Check the ErrorNum parameter before deciding to commit the transaction
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Function
        End If

        'Close the connection
        cnnSQL.Close()

        'free all resources
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


    End Function
    Public Function FetchByCriteria( _
                            ByVal lngInsuranceID As Long, _
                            ByVal strCheckNum As String, _
                            ByVal dteCheckDate As Date, _
                            ByVal strDepositNum As String, _
                            ByVal blnUnrec As Boolean, _
                            ByVal lngProviderID As Long, _
                            ByVal lngGroupID As Long) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 09/25/00
        'Author: Eric Pena
        'Description:   Retrieves records from the tblPayerCheck table.
        'Parameters:   blnIncludeDisabled- Optional parameter that identifies if  records flagged as
        '               'Disabled' or 'De-activated' are to be included in the record set. The default value is False
        '
        'Returns: ADODB.Recordset
        '-------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim cmdSQL As ADODB.Command



        rstSQL = New ADODB.Recordset
        cmdSQL = New ADODB.Command
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)
        cmdSQL.ActiveConnection = cnnSQL

        'create the parameter objects
        With cmdSQL
            .CommandText = "uspSelPayerCheckByCriteria"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , IIf(lngInsuranceID > 0, lngInsuranceID, DBNull.Value)))
            .Parameters.Append(.CreateParameter("@CheckNum", adVarChar, , 30, IIf(Trim(strCheckNum) <> "", Trim(strCheckNum), DBNull.Value)))
            .Parameters.Append(.CreateParameter("@CheckDate", adDBTimeStamp, , , IIf(dteCheckDate.ToOADate() <> 0, dteCheckDate, DBNull.Value)))
            .Parameters.Append(.CreateParameter("@DepositNum", adVarChar, , 30, IIf(Trim(strDepositNum) <> "", Trim(strDepositNum), DBNull.Value)))
            .Parameters.Append(.CreateParameter("@Unrec", adVarChar, , 1, IIf(blnUnrec, "Y", "N")))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , IIf(lngProviderID > 0, lngProviderID, DBNull.Value)))
            .Parameters.Append(.CreateParameter("@GroupID", adInteger, adParamInput, , IIf(lngGroupID > 0, lngGroupID, DBNull.Value)))
        End With

        'Execute the stored procedure
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        FetchByCriteria = rstSQL

        'disconnect the recordset
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        cnnSQL = Nothing

        'Signal successful completion

    End Function

    Public Function FetchPayments(ByVal lngID As Long) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 09/25/00
        'Author: Eric Pena
        'Description:   Retrieves records from the tblPayerCheck table.
        'Parameters:   blnIncludeDisabled- Optional parameter that identifies if  records flagged as
        '               'Disabled' or 'De-activated' are to be included in the record set. The default value is False
        '
        'Returns: ADODB.Recordset
        '-------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim cmdSQL As ADODB.Command



        rstSQL = New ADODB.Recordset
        cmdSQL = New ADODB.Command
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)
        cmdSQL.ActiveConnection = cnnSQL

        'create the parameter objects
        With cmdSQL
            .CommandText = "uspSelPayerCheckPayments"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngID))
        End With

        'Execute the stored procedure
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        FetchPayments = rstSQL

        'disconnect the recordset
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        cnnSQL = Nothing

        'Signal successful completion



    End Function
    Public Sub Deleted(ByVal lngCheckID As Long, ByVal blnIsDeleted As Boolean, ByVal strDeletedBy As String)
        '------------------------------------------------------------------------------------
        'Date: 09/25/00
        'Author: Eric Pena
        'Description:   Updates a row in the tblPayerCheck table utilizing a stored procedure.
        'Parameters:    Each parameter identifies the column value that will be updated.
        'Returns:       Null
        '-------------------------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspDelPayerCheck"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngCheckID))
            .Parameters.Append(.CreateParameter("@Deleted", adVarChar, adParamInput, 1, IIf(blnIsDeleted, "Y", "N")))
            .Parameters.Append(.CreateParameter("@DeletedBy", adVarChar, adParamInput, 30, strDeletedBy))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the command object
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check the SQLErrorNum parameter before deciding to commit the transaction.
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
            Exit Sub
        End If

        'Close the connection and free all resources
        cnnSQL.Close()
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion

    End Sub
    Public Sub LockCheck(ByVal lngCheckID As Long, ByVal strUserName As String)
        '----------------------------------------------------------------------------------------
        'Date:          09/10/02
        'Author:        Eric Pena
        'Description:   Locks the specified check and transactions associated with it
        'Parameters:    lngCheckID = ID in tblPayerCheck for check to lock or -1 to lock all reconciled checks
        '----------------------------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdLockPayerCheck"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngCheckID))
            .Parameters.Append(.CreateParameter("@Username", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the command object
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()

        'Check the SQLErrorNum parameter before deciding to commit the transaction.
        If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Or (cmdSQL.Parameters("@SQLErrorNum").Value.IsNullorEmpty) Then
            Exit Sub
        End If

        'Close the connection and free all resources
        cnnSQL.Close()
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion



        'RaiseError(Err, -1, CLASS_NAME, "SQL Server error")
    End Sub

End Class
