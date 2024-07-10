VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 2  'RequiresTransaction
END
Attribute VB_Name = "CPatientDxDB"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CPatientDxDB
'Date: 3/14/2000
'Author: Eric Pena
'Description:  MTS object designed to host methods associated with
'              data affecting the tblPatientDiagnosis table.
'--------------------------------------------------------------------
Option Explicit
Private Const CLASS_NAME As String = "CPatientDxDB"
Private Const TABLE_NAME As String = "tblPatientDiagnosis"
Public Function Insert(ByVal lngPatientID As Long, ByVal intAxisIa As Long, ByVal strAxisIaModifier As String, ByVal intAxisIb As Long, ByVal strAxisIbModifier As String, _
                                    ByVal intAxisIc As Long, ByVal strAxisIcModifier As String, ByVal intAxisId As Long, ByVal strAxisIdModifier As String, _
                                    ByVal intAxisIIa As Long, ByVal strAxisIIaModifier As String, ByVal intAxisIIb As Long, ByVal strAxisIIbModifier As String, _
                                    ByVal strAxisIII As String, ByVal strAxisIV As String, ByVal strAxisV As String, ByVal strDxNotes As String, ByVal strAddedBy As String)
'--------------------------------------------------------------------
'Date: 3/14/2000
'Author: Eric Pena
'Description:  Inserts a row into the tblPatientDiagnosis table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'Returns: null
'--------------------------------------------------------------------

    
    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command
    Dim intCount As Integer
    
    On Error GoTo ErrTrap:
    

    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    'Create the parameter objects
    With cmdSQL
        .CommandText = "uspInsPatientDiagnosis"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, 20, lngPatientID)
        .Parameters.Append .CreateParameter("@AxisIa", adInteger, adParamInput, , IIf(intAxisIa < 0, Null, intAxisIa))
        .Parameters.Append .CreateParameter("@AxisIaModifier", adVarChar, adParamInput, 20, IIf(strAxisIaModifier = "", Null, strAxisIaModifier))
        .Parameters.Append .CreateParameter("@AxisIb", adInteger, adParamInput, , IIf(intAxisIb < 0, Null, intAxisIb))
        .Parameters.Append .CreateParameter("@AxisIbModifier", adVarChar, adParamInput, 20, IIf(strAxisIbModifier = "", Null, strAxisIbModifier))
        .Parameters.Append .CreateParameter("@AxisIc", adInteger, adParamInput, , IIf(intAxisIc < 0, Null, intAxisIc))
        .Parameters.Append .CreateParameter("@AxisIcModifier", adVarChar, adParamInput, 20, IIf(strAxisIcModifier = "", Null, strAxisIcModifier))
        .Parameters.Append .CreateParameter("@AxisId", adInteger, adParamInput, , IIf(intAxisId < 0, Null, intAxisId))
        .Parameters.Append .CreateParameter("@AxisIdModifier", adVarChar, adParamInput, 20, IIf(strAxisIdModifier = "", Null, strAxisIdModifier))
        .Parameters.Append .CreateParameter("@AxisIIa", adInteger, adParamInput, , IIf(intAxisIIa < 0, Null, intAxisIIa))
        .Parameters.Append .CreateParameter("@AxisIIaModifier", adVarChar, adParamInput, 20, IIf(strAxisIIaModifier = "", Null, strAxisIIaModifier))
        .Parameters.Append .CreateParameter("@AxisIIb", adInteger, adParamInput, , IIf(intAxisIIb < 0, Null, intAxisIIb))
        .Parameters.Append .CreateParameter("@AxisIIbModifier", adVarChar, adParamInput, 20, IIf(strAxisIIbModifier = "", Null, strAxisIIbModifier))
        .Parameters.Append .CreateParameter("@AxisIII", adVarChar, adParamInput, 100, IIf(strAxisIII = "", Null, strAxisIII))
        .Parameters.Append .CreateParameter("@AxisIV", adVarChar, adParamInput, 100, IIf(strAxisIV = "", Null, strAxisIV))
        .Parameters.Append .CreateParameter("@AxisV", adVarChar, adParamInput, 50, IIf(strAxisV = "", Null, strAxisV))
        .Parameters.Append .CreateParameter("@DxNotes", adVarChar, adParamInput, 100, IIf(strDxNotes = "", Null, strDxNotes))
        .Parameters.Append .CreateParameter("@AddedBy", adVarChar, adParamInput, 100, strAddedBy)
        .Parameters.Append .CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open("DSN=PsyquelSQL")
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords

    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If
    
    'Close the connection
    cnnSQL.Close
    
    'free all resources
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    GetObjectContext.SetAbort
    Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME)
    
End Function
Public Sub Update(ByVal lngPatientID As Long, ByVal intAxisIa As Integer, ByVal strAxisIaModifier As String, ByVal intAxisIb As Integer, ByVal strAxisIbModifier As String, _
                                    ByVal intAxisIc As Integer, ByVal strAxisIcModifier As String, ByVal intAxisId As Integer, ByVal strAxisIdModifier As String, _
                                    ByVal intAxisIIa As Integer, ByVal strAxisIIaModifier As String, ByVal intAxisIIb As Integer, ByVal strAxisIIbModifier As String, _
                                    ByVal strAxisIII As String, ByVal strAxisIV As String, ByVal strAxisV As String, ByVal strDxNotes As String)
'--------------------------------------------------------------------
'Date: 3/14/2000
'Author: Eric Pena
'Description:  Updates a row in the tblPatientDiagnosis table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'Returns: Null
'--------------------------------------------------------------------
    
    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command

    On Error GoTo ErrTrap:

    
    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    With cmdSQL
        .CommandText = "uspUpdPatientDiag"
        .CommandType = adCmdStoredProc
        .Parameters.Append .CreateParameter("@PatientID", adInteger, adParamInput, 20, lngPatientID)
        .Parameters.Append .CreateParameter("@AxisIa", adInteger, adParamInput, , intAxisIa)
        .Parameters.Append .CreateParameter("@AxisIaModifier", adVarChar, adParamInput, 20, strAxisIaModifier)
        .Parameters.Append .CreateParameter("@AxisIb", adInteger, adParamInput, , intAxisIb)
        .Parameters.Append .CreateParameter("@AxisIbModifier", adVarChar, adParamInput, 20, strAxisIbModifier)
        .Parameters.Append .CreateParameter("@AxisIc", adInteger, adParamInput, , intAxisIc)
        .Parameters.Append .CreateParameter("@AxisIcModifier", adVarChar, adParamInput, 20, strAxisIcModifier)
        .Parameters.Append .CreateParameter("@AxisId", adInteger, adParamInput, , intAxisId)
        .Parameters.Append .CreateParameter("@AxisIdModifier", adVarChar, adParamInput, 20, strAxisIdModifier)
        .Parameters.Append .CreateParameter("@AxisIIa", adInteger, adParamInput, , intAxisIIa)
        .Parameters.Append .CreateParameter("@AxisIIaModifier", adVarChar, adParamInput, 20, strAxisIIaModifier)
        .Parameters.Append .CreateParameter("@AxisIIb", adInteger, adParamInput, , intAxisIIb)
        .Parameters.Append .CreateParameter("@AxisIIbModifier", adVarChar, adParamInput, 20, strAxisIIbModifier)
        .Parameters.Append .CreateParameter("@AxisIII", adVarChar, adParamInput, 100, strAxisIII)
        .Parameters.Append .CreateParameter("@AxisIV", adVarChar, adParamInput, 100, strAxisIV)
        .Parameters.Append .CreateParameter("@AxisV", adVarChar, adParamInput, 50, strAxisV)
        .Parameters.Append .CreateParameter("@DxNotes", adVarChar, adParamInput, 100, strDxNotes)
        .Parameters.Append .CreateParameter("@ErrorNum", adInteger, adParamOutput, , 0)
    End With
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open("DSN=PsyquelSQL")
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords
    'Check the ErrorNum parameter before deciding to commit the transaction
    If cmdSQL.Parameters("@ErrorNum").Value <> 0 Then
        GoTo ErrTrap
    End If

    'Close the connection and free all resources
    cnnSQL.Close
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    GetObjectContext.SetAbort
    Call RaiseError(Err, cmdSQL.Parameters("@ErrorNum").Value, CLASS_NAME, cmdSQL.Parameters("@ErrorMsg").Value)
    
End Sub
Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Long, ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 3/14/2000
'Author: Eric Pena
'Description:  Flags a row in the tblPatientDiagnosis table marking the row as
'              deleted or undeleted.
'Parameters: blnDeleted - Boolean value identifying if the record is to
'               be deleted (True) or undeleted (False).
'            lngID - ID of the row in the table whose value will be
'               updated.
'            strUserName - Login name of the user responsible for
'               marking the row as deleted.
'Returns: Null
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    
    Dim cnnSQL As ADODB.Connection
    Dim cmdSQL As ADODB.Command
    Dim strSQL As String
    Dim strSQL2 As String
    Dim strSQL3 As String
    Dim blnFlag As String

    On Error GoTo ErrTrap:

    

    If blnDeleted = True Then
        blnFlag = "Y"
        'Prepare the SQL statement
        strSQL = "UPDATE "
        strSQL = strSQL & TABLE_NAME
        strSQL = strSQL & " SET "
        strSQL = strSQL & " fldDisabledYN = '" & blnFlag & "', "
        strSQL = strSQL & " fldDateDisabled = '" & Now & "', "
        strSQL = strSQL & " fldDisabledBy = '" & strUserName & "' "
        strSQL = strSQL & "WHERE "
        strSQL = strSQL & "fldBenefactorID = " & lngID
        'Disable Benefactor-clinic associations
        strSQL2 = "UPDATE tblPatientDiagnosisClinic SET "
        strSQL2 = strSQL2 & " fldDisabledYN = '" & blnFlag & "', "
        strSQL2 = strSQL2 & " fldDateDisabled = '" & Now & "', "
        strSQL2 = strSQL2 & " fldDisabledBy = '" & strUserName & "' "
        strSQL2 = strSQL2 & "WHERE "
        strSQL2 = strSQL2 & "fldBenefactorID = " & lngID
        'Disable Benefactor-rp-plan associations
        strSQL3 = "UPDATE tblPatRPPlan SET "
        strSQL3 = strSQL3 & " fldDisabledYN = '" & blnFlag & "', "
        strSQL3 = strSQL3 & " fldDateDisabled = '" & Now & "', "
        strSQL3 = strSQL3 & " fldDisabledBy = '" & strUserName & "' "
        strSQL3 = strSQL3 & "WHERE "
        strSQL3 = strSQL3 & "fldPatientID = " & lngID & " OR "
        strSQL3 = strSQL3 & "fldRPID = " & lngID
    Else
        blnFlag = "N"
        'Prepare the SQL statement
        strSQL = "UPDATE "
        strSQL = strSQL & TABLE_NAME
        strSQL = strSQL & " SET "
        strSQL = strSQL & " fldDisabledYN = '" & blnFlag & "' "
        strSQL = strSQL & "WHERE "
        strSQL = strSQL & "fldBenefactorID = " & lngID
    End If

    'Instantiate and prepare the Command object.
    Set cmdSQL = New ADODB.Command
    cmdSQL.CommandText = strSQL
    cmdSQL.CommandType = adCmdText
    
    'Acquire the database connection.
    Set cnnSQL = New ADODB.Connection
    Call cnnSQL.Open("DSN=PsyquelSQL")
    
    'Assign the connection to the Command object and execute the stored procedure
    Set cmdSQL.ActiveConnection = cnnSQL
    cmdSQL.Execute , , adExecuteNoRecords

    If blnDeleted = True Then
        cmdSQL.CommandText = strSQL2
        cmdSQL.Execute , , adExecuteNoRecords
        cmdSQL.CommandText = strSQL3
        cmdSQL.Execute , , adExecuteNoRecords
    End If
    'Close the connection and free all resources
    cnnSQL.Close
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing

    'Signal successful completion
    GetObjectContext.SetComplete

    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set cmdSQL = Nothing
    Set cnnSQL = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub
