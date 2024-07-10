
'--------------------------------------------------------------------
'Class Name: CBenefactorClinicDB
'Date: 02/06/2000
'Author: Eric Pena
'Description:  MTS object designed to allow insertions into the tblBenefactorClinic table.
'--------------------------------------------------------------------
'Revision History:
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
'---------------------------

Imports Psyquel.BusinessRules.CoreLibraryIII


Public Class CBenefactorClinicDB

    Private Const TABLE_NAME As String = "tblBenefactorClinic"
    Private Const CLASS_NAME As String = "CBenefactorClinicDB"
    Private _ConnectionString As String = String.Empty


    Private _DB As New dbStuff.db
    Private _MD As New dbStuff.ModCommon


    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property





    Public Function FetchClinics(ByVal lngBenefactorID As Long) As Object
        '--------------------------------------------------------------------
        'Date: 02/06/2000
        'Author: Eric Pena
        'Description:  Returns a Object array containing a list of all Clinics to which a Benefactor is associated.
        'Parameters: lngBenefactorID - The ID of the Benefactor
        'Returns: A Object array containing a list of all clinics to which a Benefactor is associated.
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim strSQL As String
        Dim strList As String
        Dim intCTR As Integer
        Dim arrList() As String
        Dim varList As Object





        'Prepare the SQL statement.
        strSQL = "SELECT "
        strSQL = strSQL & "fldClinicID "
        strSQL = strSQL & "FROM "
        strSQL = strSQL & "tblBenefactorClinic "
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & "fldBenefactorID = " & lngBenefactorID

        'Instantiate and populate the Recordset.
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = adUseClient

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        rstSQL.Open(strSQL, cnnSQL, adOpenForwardOnly, adLockReadOnly, adCmdText + adAsyncFetch)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing

        'set to null if there are no current associations (new Benefactor)
        If rstSQL.RecordCount = 0 Then
            FetchClinics = DBNull.Value
            GoTo EMPTY_RECORDSET
        End If

        'Parse the recordset and build the comma-separated list.
        ReDim arrList(rstSQL.RecordCount - 1)
        For intCTR = 0 To rstSQL.RecordCount - 1
            arrList(intCTR) = rstSQL.Fields("fldClinicID").Value
            rstSQL.MoveNext()
        Next intCTR

        varList = arrList
        FetchClinics = varList

EMPTY_RECORDSET:
        'Signal successful completion

        cnnSQL = Nothing
        rstSQL = Nothing
        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        'cmdSQL.Execute()
        cnnSQL = Nothing
        rstSQL = Nothing
        varList = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Function
    Public Sub Insert(ByVal lngBenefactorID As Long, ByVal rstClinics As ADODB.Recordset, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 01/04/2000
        'Author: Dave Richkun
        'Description:  Inserts one or more rows into the tblBenefactorClinic table.
        'Parameters: lngBenefactorID - ID of the Benefactor who is being associated to one or more clinics
        '                   varClinics - Object array containing the IDs of each clinic to which the Benefactor is associated.
        'Returns: Null
        '--------------------------------------------------------------------


        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim strSQL As String
        Dim intCTR As Integer
        Dim varCurrClinics As Object
        Dim blnFound As Boolean





        'get list of currently associated clinics
        varCurrClinics = FetchClinics(lngBenefactorID)

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Instantiate and prepare the Command object.
        While Not rstClinics.EOF
            blnFound = False
            'if there are no current clinics, insert all
            If varCurrClinics.IsNullorEmpty Then
                GoTo NO_ASSOC
            End If
            'for each selected clinic, find out if it already exists
            For intCTR = 0 To UBound(varCurrClinics)
                If CInt(rstClinics.Fields("fldClinicID").Value) = CInt(varCurrClinics(intCTR)) Then
                    blnFound = True
                End If
            Next intCTR
            'if it exists, make it enabled
            If blnFound Then
                Deleted(False, lngBenefactorID, rstClinics.Fields("fldClinicID").Value, strUserName)
            Else
NO_ASSOC:
                'add the association if it is not found
                cmdSQL = New ADODB.Command
                With cmdSQL
                    .CommandType = adCmdText
                    .CommandText = "uspInsBenefactorClinic"
                    .CommandType = adCmdStoredProc
                    .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngBenefactorID))
                    .Parameters.Append(.CreateParameter("@ClinicID", adInteger, adParamInput, , rstClinics.Fields("fldClinicID").Value))
                    'TODO: Add the real parameter
                    .Parameters.Append(.CreateParameter("@LocalID", adVarChar, adParamInput, 20, DBNull.Value))
                    .Parameters.Append(.CreateParameter("@AddedBy", adVarChar, adParamInput, 20, strUserName))
                    .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
                End With
                cmdSQL.ActiveConnection = cnnSQL
                cmdSQL.Execute()
                'Check the ErrorNum parameter before deciding to commit the transaction
                If cmdSQL.Parameters("@SQLErrorNum").Value <> 0 Then
                    Exit Sub
                End If
            End If
            rstClinics.MoveNext()
        End While

        'disable associations that were just unassociated by the user
        If Not varCurrClinics.IsNullorEmpty Then
            For intCTR = 0 To UBound(varCurrClinics)
                blnFound = False
                rstClinics.MoveFirst()
                While Not rstClinics.EOF
                    If CInt(varCurrClinics(intCTR)) = CInt(rstClinics.Fields("fldClinicID").Value) Then
                        blnFound = True
                    End If
                    rstClinics.MoveNext()
                End While
                If Not blnFound Then
                    Deleted(True, lngBenefactorID, varCurrClinics(intCTR), strUserName)
                End If
            Next intCTR
        End If

        'Close the connection and free all resources
        cnnSQL.Close()
EMPTY_ARRAY:
        cmdSQL = Nothing
        cnnSQL = Nothing
        'Signal successful completion

        Exit Sub

        'The Clinic array may be empty.
        If Err.Number = 9 Then
            GoTo EMPTY_ARRAY
        Else
            'Signal incompletion and raise the error to the ing environment.
            cmdSQL = Nothing
            cnnSQL = Nothing
            cmdSQL.Execute()
            'err.raise(Err.Number, Err.Source, Err.Description)
        End If
    End Sub
    Public Function IsEnabled(ByVal lngBenefactorID As Long, ByVal lngClinicID As Long) As Boolean
        '--------------------------------------------------------------------
        'Date: 02/06/2000
        'Author: Eric Pena
        'Description:  Lets the er know if a given Benefactor/clinic association is active
        'Parameters: lngBenefactorID - The ID of the Benefactor
        '                  lngClinicID - The ID of the Clinic
        'Returns: T if the association is enabled/exists, F otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim strSQL As String
        Dim strList As String
        Dim intCTR As Integer
        Dim arrList() As String
        Dim varList As Object

        IsEnabled = False




        'Prepare the SQL statement.
        strSQL = "SELECT "
        strSQL = strSQL & "fldDisabledYN "
        strSQL = strSQL & "FROM "
        strSQL = strSQL & "tblBenefactorClinic "
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & "fldBenefactorID = " & lngBenefactorID & " "
        strSQL = strSQL & "AND fldClinicID = " & lngClinicID

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Instantiate and populate the Recordset.
        rstSQL = New ADODB.Recordset
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(strSQL, cnnSQL, adOpenForwardOnly, adLockReadOnly, adCmdText + adAsyncFetch)

        'Disconnect the recordset, close the connection and return the recordset
        'to the ing environment.
        rstSQL.ActiveConnection = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing

        'set to null if there are no current associations (new Benefactor)
        If rstSQL.RecordCount = 0 Then
            GoTo EMPTY_RECORDSET
        End If
        If rstSQL.Fields("fldDisabledYN").Value = "N" Then
            IsEnabled = True
        End If

EMPTY_RECORDSET:
        'Signal successful completion

        cnnSQL = Nothing
        rstSQL = Nothing
        Exit Function


        'Signal incompletion and raise the error to the ing environment.
        ' cmdSQL.Execute()
        cnnSQL = Nothing
        rstSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)
    End Function
    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngBenefactorID As Long, ByVal lngClinicID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 3/01/2000
        'Author: Eric Pena
        'Description:  Flags a row in the tblBenefactor table marking the row as
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
        Dim blnFlag As String





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
            strSQL = strSQL & "fldBenefactorID = " & lngBenefactorID & " AND "
            strSQL = strSQL & "fldClinicID = " & lngClinicID

        Else
            blnFlag = "N"
            'Prepare the SQL statement
            strSQL = "UPDATE "
            strSQL = strSQL & TABLE_NAME
            strSQL = strSQL & " SET "
            strSQL = strSQL & " fldDisabledYN = '" & blnFlag & "' "
            strSQL = strSQL & "WHERE "
            strSQL = strSQL & "fldBenefactorID = " & lngBenefactorID & " AND "
            strSQL = strSQL & "fldClinicID = " & lngClinicID
        End If

        'Instantiate and prepare the Command object.
        cmdSQL = New ADODB.Command
        cmdSQL.CommandText = strSQL
        cmdSQL.CommandType = adCmdText

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
        'Signal successful completion

        Exit Sub

        'Signal incompletion and raise the error to the ing environment.
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()
        'err.raise(Err.Number, Err.Source, Err.Description)
    End Sub
End Class
