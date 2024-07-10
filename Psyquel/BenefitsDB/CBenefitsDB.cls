
'--------------------------------------------------------------------
'Class Name: CBenefitsDB
'Date: 09/11/2000
'Author: Eric Pena
'Description:  MTS object designed to host methods associated with data affecting Benefits tables.
'--------------------------------------------------------------------
' Revision History:
'   R001: 11/06/2001 Richkun: Added FetchCoPayByPatientProvider() method
'--------------------------------------------------------------------
Option Explicit



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
' Lydia Orth 


Public Class CBenefitsDB

    Private Const CLASS_NAME As String = "CBenefitsDB"
    Private Const TABLE_NAME As String = ""
    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property

    Public Function FetchBenefitsByPatient(ByVal lngPatientID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/11/2000
        'Author: Eric Pena
        'Description:  Retrieves records from the tblInsurance table utilizing
        '              a stored procedure.
        'Parameters: None
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim cnn As New ADODB.Connection
        Dim rst As New ADODB.Recordset
        Dim cmd As New ADODB.Command




        'Create the parameter objects
        With cmd
            .CommandText = "uspSelBenefitsByPatient"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchBenefitsByPatient = rst

        'Disconnect the recordset
        cmd = Nothing
        cnn = Nothing


    End Function


    Public Function FetchCoPayByPatientProvider(ByVal lngPatientID As Long, ByVal lngProviderID As Long) As Double
        '--------------------------------------------------------------------
        'Date: 11/07/2001
        'Author: Dave Richkun
        'Description:  Returns the CoPay amount for a patient's Primary plan
        '              based on the credentials of a Provider
        'Parameters: lngPatientID - ID of the patient
        '            lngProviderID - ID of the Provider
        'Returns: Copay amount
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim cnn As ADODB.Connection
        Dim rst As ADODB.Recordset
        Dim cmd As ADODB.Command



        rst = New ADODB.Recordset
        cmd = New ADODB.Command
        cnn = New ADODB.Connection

        'Create the parameter objects
        With cmd
            .CommandText = "uspSelCoPayByPatientProvider"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
        End With

        'Acquire the database connection.
        cnn.Open(_ConnectionString)
        cmd.ActiveConnection = cnn

        'Execute the stored procedure
        rst.CursorLocation = adUseClient
        rst.Open(cmd, , adOpenForwardOnly, adLockReadOnly)

        FetchCoPayByPatientProvider = rst.Fields("fldCopay").Value

        'Disconnect the recordset
        rst = Nothing
        cmd = Nothing
        cnn = Nothing


    End Function

End Class

