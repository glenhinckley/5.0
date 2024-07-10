
'------------------------------------------------------------------------------------
'Date: 10/11/00
'Class Name: CClaimDB
'Author: Juan Castro
'Description:   MTS object designed to host methods associated with data affecting
'               tblSubmission and tblSubmissionSnapShot tables.
'------------------------------------------------------------------------------------
'Revision History:
'  R001: Richkun 01/20/2003 - Added methods to dynamiy retrieve paper and electronic claims
'  R002: Richkun 01/24/2003 - Added SubmitClaim() method with intentions of replacing Submit() method
'  R003: Richkun: 03/10/2003 - Added FetchNextBatchNumber()
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
' Lydia Orth 

Public Class CClaimDB

    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property

    Private Const CLASS_NAME As String = "CClaimDB"
    Private Const TABLE_NAME As String = "tblClaim"



    Public Function Insert(ByVal lngELID As Long, ByVal intVersionNum As Integer, ByVal lngRPID As Long, _
                           ByVal lngClinicID As Long, ByVal lngPlanID As Long, ByVal lngCPCID As Long, _
                           ByVal lngInsuranceID As Long, ByVal intOrder As Integer, ByVal strPostedBy As String) As Long
        '-------------------------------------------------------------------------------------
        'Date:          7/11/2003
        'Author:        Eric Pena
        'Description:   Creates a row in tblClaim (Claim to submit to insurance company).  If a row already exists,
        '               the next version of the claim will be inserted.  (i.e., disable current version, add a row
        '               with an incremented version number.
        'Parameters:    lngELID - ID of DOS to generate claim for (PK in tblEncounterLog)
        '               lngRPID - ID of insured (Responsible party) (FK to tblPatRPPlan
        '               lngPlanID - Plan covering the specified RP (FK to same row as above in tblPatRPPlan)
        'Returns:       ID (Primary Key) of the row inserted.
        '-------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the command object
        cmdSQL = New ADODB.Command

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspInsClaim"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamReturnValue))
            .Parameters.Append(.CreateParameter("@ELID", adInteger, adParamInput, , lngELID))
            .Parameters.Append(.CreateParameter("@VersionNum", adInteger, adParamInput, , intVersionNum))
            .Parameters.Append(.CreateParameter("@RPID", adInteger, adParamInput, , lngRPID))
            .Parameters.Append(.CreateParameter("@ClinicID", adInteger, adParamInput, , lngClinicID))
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
            .Parameters.Append(.CreateParameter("@CPCID", adInteger, adParamInput, , lngCPCID))
            .Parameters.Append(.CreateParameter("@InsuranceID", adInteger, adParamInput, , lngInsuranceID))
            .Parameters.Append(.CreateParameter("@Order", adInteger, adParamInput, , intOrder))
            .Parameters.Append(.CreateParameter("@PostedBy", adVarChar, adParamInput, 30, strPostedBy))
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

        'Free all resources
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion

        Exit Function


        'Signal incompletion and raise the error to the ing enviroment
        cmdSQL.Execute()
        cmdSQL = Nothing
        cnnSQL = Nothing
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)
    End Function


    Public Sub DeleteBeforeSubmit(ByVal lngELID As Long)
        '-------------------------------------------------------------------------------------
        'Date: 08/06/2002
        'Author: Dave Richkun
        'Description: Physiy deletes a row in tblClaim using the Encounter Log ID as the
        '             identifier.  This method is intended to be be ed only when the
        '             claim has not yet been printed to a HCFA or had an electronic claim
        '             record created for it.  Since much more is involved before the row
        '             can be deleted, the ing procedure is responsible for ensuring the
        '             claim status is still Queued.
        'Parameters:  lngELID - ID of encounter log used to identify claim to delete
        'Returns: Null
        '-------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the command object
        cmdSQL = New ADODB.Command

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspDelClaimBeforeSubmit"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ELID", adInteger, adParamInput, , lngELID))
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
            Exit Sub
        End If

        'Close the connection
        cnnSQL.Close()

        'Free all resources
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion

        Exit Sub


        'Signal incompletion and raise the error to the ing enviroment
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()

    End Sub


    Public Function FetchByID(ByVal lngClaimID As Long) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 02/19/2003
        'Author: Dave Richkun
        'Description: Retrieves detailed information for a single claim
        'Parameters: lngClaimID - ID of claim whose information is to be retrieved
        'Returns:       ADODB.Recordset
        '-------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim cmdSQL As ADODB.Command



        rstSQL = New ADODB.Recordset
        cmdSQL = New ADODB.Command
        cnnSQL = New ADODB.Connection

        cnnSQL.Open(_ConnectionString)
        cmdSQL.ActiveConnection = cnnSQL

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspSelClaimByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngClaimID))
        End With

        'Execute the stored procedure
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        FetchByID = rstSQL

        'Disconnect the recordset
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Function


        'Signal incompletion and raise the error to the ing enviroment.
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()

    End Function

    Public Sub Update(ByVal lngSubmissionID As Long, ByVal blnElectClaimYN As Boolean, ByVal strFileName As String, _
                      ByVal strInsuredID As String, ByVal strPatientName As String, ByVal strPatientStreetNum As String, _
                      ByVal strPatientCity As String, ByVal strPatientState As String, ByVal strPatientZip As String, _
                      ByVal strPatientPhone As String, ByVal strPatientRelation As String, ByVal dtePatientDOB As Date, _
                      ByVal strPatientSex As String, ByVal strInsuredName As String, ByVal strInsuredStreetNum As String, _
                      ByVal strInsuredCity As String, ByVal strInsuredState As String, ByVal strInsuredZip As String, _
                      ByVal strInsuredPhone As String, ByVal strMaritalStatus As String, ByVal strEmployStatus As String, _
                      ByVal strOthInsdName As String, ByVal strOthInsdGroupNum As String, ByVal dteOthInsdDOB As Date, _
                      ByVal strOthInsdSex As String, ByVal strOthInsdEmployer As String, ByVal strOthPlanName As String, _
                      ByVal blnConditionEmployYN As Boolean, ByVal blnConditionAutoYN As Boolean, ByVal blnConditionOtherYN As Boolean, _
                      ByVal strInsdGroupNum As String, ByVal dteInsdDOB As Date, ByVal strInsdSex As String, ByVal strInsdEmployer As String, _
                      ByVal strPlanName As String, ByVal blnOthInsYN As Boolean, ByVal strProviderSigned As String, ByVal strCertNum As String, _
                      ByVal strFacilityLine1 As String, ByVal strFacilityLine2 As String, ByVal strFacilityCity As String, _
                      ByVal strFacilityState As String, ByVal strFacilityZip As String, ByVal strBillingCompanyLine1 As String, _
                      ByVal strBillingCompanyCity As String, ByVal strBillingCompanyState As String, ByVal strBillingCompanyZip As String)

        '-------------------------------------------------------------------------------------
        'Date: 10/12/2000
        'Author: Juan Castro
        'Description:   Updates a row in the tblSubmission and tblSubmissionSnapShot tables utilizing
        '               a stored procedure.
        'Parameters:    Each parameter identifies the column value that will be updated.
        'Returns:       Null
        '-------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdSubmission"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngSubmissionID))
            .Parameters.Append(.CreateParameter("@ElectClaimYN", adVarChar, adParamInput, 1, IIf(blnElectClaimYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@FileName", adVarChar, adParamInput, 30, strFileName))
            .Parameters.Append(.CreateParameter("@InsuredID", adVarChar, adParamInput, 60, strInsuredID))
            .Parameters.Append(.CreateParameter("@PatientName", adVarChar, adParamInput, 60, strPatientName))
            .Parameters.Append(.CreateParameter("@PatientStreetNum", adVarChar, adParamInput, 60, strPatientStreetNum))
            .Parameters.Append(.CreateParameter("@PatientCity", adVarChar, adParamInput, 60, strPatientCity))
            .Parameters.Append(.CreateParameter("@PatientState", adVarChar, adParamInput, 2, strPatientState))
            .Parameters.Append(.CreateParameter("@PatientZip", adVarChar, adParamInput, 9, strPatientZip))
            .Parameters.Append(.CreateParameter("@PatientPhone", adVarChar, adParamInput, 10, strPatientPhone))
            .Parameters.Append(.CreateParameter("@PatientRelation", adVarChar, adParamInput, 20, strPatientRelation))
            .Parameters.Append(.CreateParameter("@PatientDOB", adDBTimeStamp, adParamInput, , IIf(CLng(dtePatientDOB.ToOADate()) = 0, DBNull.Value, dtePatientDOB)))
            .Parameters.Append(.CreateParameter("@PatientSex", adVarChar, adParamInput, 1, strPatientSex))
            .Parameters.Append(.CreateParameter("@InsuredName", adVarChar, adParamInput, 60, strInsuredName))
            .Parameters.Append(.CreateParameter("@InsuredStreetNum", adVarChar, adParamInput, 60, strInsuredStreetNum))
            .Parameters.Append(.CreateParameter("@InsuredCity", adVarChar, adParamInput, 60, strInsuredCity))
            .Parameters.Append(.CreateParameter("@InsuredState", adVarChar, adParamInput, 2, strInsuredState))
            .Parameters.Append(.CreateParameter("@InsuredZip", adVarChar, adParamInput, 9, strInsuredZip))
            .Parameters.Append(.CreateParameter("@InsuredPhone", adVarChar, adParamInput, 10, strInsuredPhone))
            .Parameters.Append(.CreateParameter("@MaritalStatus", adVarChar, adParamInput, 30, strMaritalStatus))
            .Parameters.Append(.CreateParameter("@EmployStatus", adVarChar, adParamInput, 30, strEmployStatus))
            .Parameters.Append(.CreateParameter("@OthInsdName", adVarChar, adParamInput, 60, strOthInsdName))
            .Parameters.Append(.CreateParameter("@OthInsdGroupNum", adVarChar, adParamInput, 60, strOthInsdGroupNum))
            .Parameters.Append(.CreateParameter("@OthInsdDOB", adDBTimeStamp, adParamInput, , IIf(CLng(dteOthInsdDOB.ToOADate()) = 0, DBNull.Value, dteOthInsdDOB)))
            .Parameters.Append(.CreateParameter("@OthInsdSex", adVarChar, adParamInput, 1, strOthInsdSex))
            .Parameters.Append(.CreateParameter("@OthInsdEmployer", adVarChar, adParamInput, 60, strOthInsdEmployer))
            .Parameters.Append(.CreateParameter("@OthPlanName", adVarChar, adParamInput, 60, strOthPlanName))
            .Parameters.Append(.CreateParameter("@ConditionEmployYN", adVarChar, adParamInput, 1, IIf(blnConditionEmployYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@ConditionAutoYN", adVarChar, adParamInput, 1, IIf(blnConditionAutoYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@ConditionOtherYN", adVarChar, adParamInput, 1, IIf(blnConditionOtherYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@InsdGroupNum", adVarChar, adParamInput, 60, strInsdGroupNum))
            .Parameters.Append(.CreateParameter("@InsdDOB", adDBTimeStamp, adParamInput, , IIf(CLng(dteInsdDOB.ToOADate()) = 0, DBNull.Value, dteInsdDOB)))
            .Parameters.Append(.CreateParameter("@InsdSex", adVarChar, adParamInput, 1, strInsdSex))
            .Parameters.Append(.CreateParameter("@InsdEmployer", adVarChar, adParamInput, 60, strInsdEmployer))
            .Parameters.Append(.CreateParameter("@PlanName", adVarChar, adParamInput, 60, strPlanName))
            .Parameters.Append(.CreateParameter("@OthInsYN", adVarChar, adParamInput, 1, IIf(blnOthInsYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@ProviderSigned", adVarChar, adParamInput, 60, strProviderSigned))
            .Parameters.Append(.CreateParameter("@CertNum", adVarChar, adParamInput, 30, strCertNum))
            .Parameters.Append(.CreateParameter("@FacilityLine1", adVarChar, adParamInput, 60, strFacilityLine1))
            .Parameters.Append(.CreateParameter("@FacilityLine2", adVarChar, adParamInput, 60, strFacilityLine2))
            .Parameters.Append(.CreateParameter("@FacilityCity", adVarChar, adParamInput, 60, strFacilityCity))
            .Parameters.Append(.CreateParameter("@FacilityState", adVarChar, adParamInput, 2, strFacilityState))
            .Parameters.Append(.CreateParameter("@FacilityZip", adVarChar, adParamInput, 9, strFacilityZip))
            .Parameters.Append(.CreateParameter("@BillingCompanyLine1", adVarChar, adParamInput, 60, strBillingCompanyLine1))
            .Parameters.Append(.CreateParameter("@BillingCompanyCity", adVarChar, adParamInput, 60, strBillingCompanyCity))
            .Parameters.Append(.CreateParameter("@BillingCompanyState", adVarChar, adParamInput, 2, strBillingCompanyState))
            .Parameters.Append(.CreateParameter("@BillingCompanyZip", adVarChar, adParamInput, 9, strBillingCompanyZip))
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


        Exit Sub


        'Signal incompletion and raise the error to the ing enviroment
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()

    End Sub

    Public Sub SetClaimMethod(ByVal lngSubmissionID As Long, ByVal blnElectClaimYN As Boolean, _
                                   ByVal strFileName As String)
        '--------------------------------------------------------------------------------------
        'Date: 10/13/00
        'Author: Juan Castro
        'Description:   Modifies the fldElectClaimYN and fldFileName fields in tbl submission
        'Parameters:    Each parameter identifies the column value that will be updated.
        'Returns:       DBNull.Value,
        '--------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdSubmissionMethod"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngSubmissionID))
            .Parameters.Append(.CreateParameter("@ElectClaimYN", adVarChar, adParamInput, 1, IIf(blnElectClaimYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@FileName", adVarChar, adParamInput, 30, strFileName))
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

        'close the connection and free all resources
        cnnSQL.Close()
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion


        Exit Sub


        'Signal imcompletion and raise the error to the ing enviroment
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()

    End Sub

    Public Sub Queued(ByVal lngSubmissionID As Long, ByVal blnQueuedYN As Boolean)
        '--------------------------------------------------------------------------------------
        'Date: 10/13/2000
        'Author: Juan Castro
        'Description:   Updates the fldQueuedYN field in tblSubmission table.
        'Parameters:    blnQueuedYN - identifies the column value that wil be updated.
        'Returns:       DBNull.Value,
        '--------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdClaimQueued"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngSubmissionID))
            .Parameters.Append(.CreateParameter("@QueuedYN", adVarChar, adParamInput, 1, IIf(blnQueuedYN, "Y", "N")))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection
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


        Exit Sub


        'Signal incompletion and raise the error to the ing enviroment
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()

    End Sub

    Public Sub Resubmittal(ByVal lngSubmissionID As Long, ByVal blnResubmittal As Boolean)
        '--------------------------------------------------------------------------------------
        'Date: 10/13/2000
        'Author: Juan Castro
        'Description:   Updates a fldResubmittalYN field in tblSubmission table.
        'Parameters:    blnResubmittalYN - identifies the column value that wil be updated.
        'Returns:       DBNull.Value,
        '--------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdSubmissionResubmittal"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngSubmissionID))
            .Parameters.Append(.CreateParameter("@ResubmittalYN", adVarChar, adParamInput, 1, IIf(blnResubmittal, "Y", "N")))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection
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


        Exit Sub


        'Signal incompletion and raise the error to the ing enviroment
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()

    End Sub

    Public Sub Submit(ByVal dtePrintDate As Date, ByVal lngClaimID As Long, ByVal strUserName As String, Optional ByVal lngPlanID As Long = -1, Optional ByVal lngCPCID As Long = -1, _
                        Optional ByVal lngInsuranceID As Long = -1, Optional ByVal dteDOSFrom As Date = Nothing, Optional ByVal dteDOSTo As Date = Nothing, _
                        Optional ByVal lngPatientID As Long = -1, Optional ByVal lngProviderID As Long = -1, Optional ByVal strResubmittalYN As String = "", _
                        Optional ByVal strQueuedYN As String = "", Optional ByVal dtePostFrom As Date = Nothing, Optional ByVal dtePostTo As Date = Nothing, _
                        Optional ByVal dteNextActionFrom As Date = Nothing, Optional ByVal dteNextActionTo As Date = Nothing, Optional ByVal dteSubmitFrom As Date = Nothing, _
                        Optional ByVal dteSubmitTo As Date = Nothing, Optional ByVal lngNum As Long = -1, Optional ByVal lngNumTo As Long = -1)
        '--------------------------------------------------------------------------------------
        'Date:          08/07/2002
        'Author:        Eric Pena
        'Description:   Updates Claim flags and posts a submittal note to the claim specified by the search criteria
        'Parameters:    dtePrintDate - stamp to use in setting last print date
        '               lngClaimID - Claim ID for which to set flags, or -1 to search by all other parameters
        '--------------------------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdSubmissionSent"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PrintDate", adDBTimeStamp, adParamInput, , dtePrintDate))
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngClaimID))
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
            .Parameters.Append(.CreateParameter("@CPCID", adInteger, adParamInput, , lngCPCID))
            .Parameters.Append(.CreateParameter("@InsID", adInteger, adParamInput, , lngInsuranceID))
            .Parameters.Append(.CreateParameter("@DOSFrom", adDBTimeStamp, adParamInput, , IIf(CLng(dteDOSFrom.ToOADate()) = 0, DBNull.Value, dteDOSFrom)))
            .Parameters.Append(.CreateParameter("@DOSTo", adDBTimeStamp, adParamInput, , IIf(CLng(dteDOSTo.ToOADate()) = 0, DBNull.Value, dteDOSTo)))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@ProvID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@Resub", adVarChar, adParamInput, 1, strResubmittalYN))
            .Parameters.Append(.CreateParameter("@Queued", adVarChar, adParamInput, 1, strQueuedYN))
            .Parameters.Append(.CreateParameter("@PostFrom", adDBTimeStamp, adParamInput, , IIf(CLng(dtePostFrom.ToOADate()) = 0, DBNull.Value, dtePostFrom)))
            .Parameters.Append(.CreateParameter("@PostTo", adDBTimeStamp, adParamInput, , IIf(CLng(dtePostTo.ToOADate()) = 0, DBNull.Value, dtePostTo)))
            .Parameters.Append(.CreateParameter("@ActionFrom", adDBTimeStamp, adParamInput, , IIf(CLng(dteNextActionFrom.ToOADate()) = 0, DBNull.Value, dteNextActionFrom)))
            .Parameters.Append(.CreateParameter("@ActionTo", adDBTimeStamp, adParamInput, , IIf(CLng(dteNextActionTo.ToOADate()) = 0, DBNull.Value, dteNextActionTo)))
            .Parameters.Append(.CreateParameter("@SubmitFrom", adDBTimeStamp, adParamInput, , IIf(CLng(dteSubmitFrom.ToOADate()) = 0, DBNull.Value, dteSubmitFrom)))
            .Parameters.Append(.CreateParameter("@SubmitTo", adDBTimeStamp, adParamInput, , IIf(CLng(dteSubmitTo.ToOADate()) = 0, DBNull.Value, dteSubmitTo)))
            .Parameters.Append(.CreateParameter("@NumFrom", adInteger, adParamInput, , lngNum))
            .Parameters.Append(.CreateParameter("@NumTo", adInteger, adParamInput, , lngNumTo))
            .Parameters.Append(.CreateParameter("@Username", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection
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


    End Sub

    Public Function FetchClaimCrit(ByVal lngPlanID As Long, ByVal lngCPCID As Long, ByVal lngInsuranceID As Long, ByVal dteDOSFrom As Date, _
                                        ByVal dteDOSTo As Date, ByVal lngPatientID As Long, ByVal lngProviderID As Long, ByVal strResubmittalYN As String, _
                                        ByVal strQueuedYN As String, ByVal dtePostFrom As Date, ByVal dtePostTo As Date, ByVal dteNextActionFrom As Date, _
                                        ByVal dteNextActionTo As Date, ByVal dteSubmitFrom As Date, ByVal dteSubmitTo As Date, ByVal lngNum As Long, ByVal lngNumTo As Long) As ADODB.Recordset
        '--------------------------------------------------------------------------------------
        'Date: 10/13/2000
        'Author: Juan Castro
        'Description:   Returns outstanding claims based on any combination of the specified parameters
        'Parameters:    All parameters are criteria to use in searching for Submissions
        'Returns:       ADODB.Recordset
        '--------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim cmdSQL As ADODB.Command



        rstSQL = New ADODB.Recordset
        cmdSQL = New ADODB.Command
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)
        cmdSQL.ActiveConnection = cnnSQL

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspSelSubmissionByCriteria"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
            .Parameters.Append(.CreateParameter("@CPCID", adInteger, adParamInput, , lngCPCID))
            .Parameters.Append(.CreateParameter("@InsID", adInteger, adParamInput, , lngInsuranceID))
            .Parameters.Append(.CreateParameter("@DOSFrom", adDBTimeStamp, adParamInput, , IIf(CLng(dteDOSFrom.ToOADate()) = 0, DBNull.Value, dteDOSFrom)))
            .Parameters.Append(.CreateParameter("@DOSTo", adDBTimeStamp, adParamInput, , IIf(CLng(dteDOSTo.ToOADate()) = 0, DBNull.Value, dteDOSTo)))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@ProvID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@Resub", adVarChar, adParamInput, 1, strResubmittalYN))
            .Parameters.Append(.CreateParameter("@Queued", adVarChar, adParamInput, 1, strQueuedYN))
            .Parameters.Append(.CreateParameter("@PostFrom", adDBTimeStamp, adParamInput, , IIf(CLng(dtePostFrom.ToOADate()) = 0, DBNull.Value, dtePostFrom)))
            .Parameters.Append(.CreateParameter("@PostTo", adDBTimeStamp, adParamInput, , IIf(CLng(dtePostTo.ToOADate()) = 0, DBNull.Value, dtePostTo)))
            .Parameters.Append(.CreateParameter("@ActionFrom", adDBTimeStamp, adParamInput, , IIf(CLng(dteNextActionFrom.ToOADate()) = 0, DBNull.Value, dteNextActionFrom)))
            .Parameters.Append(.CreateParameter("@ActionTo", adDBTimeStamp, adParamInput, , IIf(CLng(dteNextActionTo.ToOADate()) = 0, DBNull.Value, dteNextActionTo)))
            .Parameters.Append(.CreateParameter("@SubmitFrom", adDBTimeStamp, adParamInput, , IIf(CLng(dteSubmitFrom.ToOADate()) = 0, DBNull.Value, dteSubmitFrom)))
            .Parameters.Append(.CreateParameter("@SubmitTo", adDBTimeStamp, adParamInput, , IIf(CLng(dteSubmitTo.ToOADate()) = 0, DBNull.Value, dteSubmitTo)))
            .Parameters.Append(.CreateParameter("@NumFrom", adInteger, adParamInput, , lngNum))
            .Parameters.Append(.CreateParameter("@NumTo", adInteger, adParamInput, , lngNumTo))
        End With

        'Execute the stored procedure
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        FetchClaimCrit = rstSQL

        'disconnect the recordset
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        cnnSQL = Nothing

        'Signal successful completion


    End Function

    Public Function FetchByBR(ByVal lngBRID As Long) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 10/12/2000
        'Author: Juan Castro
        'Description:   Retrieves records from the tblSubmission and tblSubmissionSnapshot tables.
        'Parameters:    lngSubmissionID - parameter that identifies records in the tables
        'Returns:       ADODB.Recordset
        '-------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim cmdSQL As ADODB.Command



        rstSQL = New ADODB.Recordset
        cmdSQL = New ADODB.Command
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)
        cmdSQL.ActiveConnection = cnnSQL

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspSelSubmissionByBR"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@BRID", adInteger, adParamInput, , lngBRID))
        End With

        'Execute the stored procedure
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        FetchByBR = rstSQL

        'disconnect the recordset
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        cnnSQL = Nothing

        'Signal successful completion



    End Function
    Public Sub SetSubmitStatus(ByVal lngSubmissionID As Long, Optional ByVal strDataBase As String = "")
        '--------------------------------------------------------------------------------------
        'Date: 10/13/00
        'Author: Juan Castro
        'Description:   Updates fields fldQueuedYN, fldResubmittalYN, fldLastSubmitDate, and
        '               fldTimesSubmitted in table tblSubmission
        'Parameters:    lngSubmissionID - identifies row to be updated
        'Returns:       Null
        '--------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdSubmissionSetStatus"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngSubmissionID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection
        cnnSQL = New ADODB.Connection
        Select Case strDataBase
            Case "Test"
                cnnSQL.Open(_ConnectionString)
            Case "PsyquelDirect"
                cnnSQL.Open(_ConnectionString)
            Case Else
                cnnSQL.Open(_ConnectionString)
        End Select

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
    Public Function FetchElectClaims(ByVal lngElectClaimID As Long, Optional ByVal strDataBase As String = "") As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 10/12/2000
        'Author: Juan Castro
        'Description:   Retrieves records from the tblSubmission and tblSubmissionSnapshot tables.
        'Parameters:    lngSubmissionID - parameter that identifies records in the tables
        'Returns:       ADODB.Recordset
        '-------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim cmdSQL As ADODB.Command



        rstSQL = New ADODB.Recordset
        cmdSQL = New ADODB.Command
        cnnSQL = New ADODB.Connection
        Select Case strDataBase
            Case "Test"
                cnnSQL.Open(_ConnectionString)
            Case "PsyquelDirect"
                cnnSQL.Open(_ConnectionString)
            Case Else
                cnnSQL.Open(_ConnectionString)
        End Select
        cmdSQL.ActiveConnection = cnnSQL

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspSelElectClaimsByID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ElectClaimID", adInteger, adParamInput, , lngElectClaimID))
        End With

        'Execute the stored procedure
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        FetchElectClaims = rstSQL

        'disconnect the recordset
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        cnnSQL = Nothing

        'Signal successful completion



    End Function
    Public Function FetchOtherInsInfo(ByVal lngEncounterLogID As Long, ByVal lngOrder As Long) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 10/12/2000
        'Author: Juan Castro
        'Description:   Retrieves records from the tblSubmission and tblSubmissionSnapshot tables.
        'Parameters:    lngSubmissionID - parameter that identifies records in the tables
        'Returns:       ADODB.Recordset
        '-------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim cmdSQL As ADODB.Command



        rstSQL = New ADODB.Recordset
        cmdSQL = New ADODB.Command
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)
        cmdSQL.ActiveConnection = cnnSQL

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspSelOtherInsuranceInfo"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@EncounterLogID", adInteger, adParamInput, , lngEncounterLogID))
            .Parameters.Append(.CreateParameter("@Order", adInteger, adParamInput, , lngOrder))
        End With

        'Execute the stored procedure
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        FetchOtherInsInfo = rstSQL

        'disconnect the recordset
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        cnnSQL = Nothing

        'Signal successful completion


    End Function
    Public Sub CloseElectClaims(Optional ByVal strDataBase As String = "")
        '--------------------------------------------------------------------------------------
        'Date: 10/13/00
        'Author: Juan Castro
        'Description:   Updates fields fldQueuedYN, fldResubmittalYN, fldLastSubmitDate, and
        '               fldTimesSubmitted in table tblSubmission
        'Parameters:    lngSubmissionID - identifies row to be updated
        'Returns:       Null
        '--------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdElectClaimClose"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection
        cnnSQL = New ADODB.Connection
        Select Case strDataBase
            Case "Test"
                cnnSQL.Open(_ConnectionString)
            Case "PsyquelDirect"
                cnnSQL.Open(_ConnectionString)
            Case Else
                cnnSQL.Open(_ConnectionString)
        End Select

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


    End Sub
    Public Function FetchUnsentElectClaims() As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 10/12/2000
        'Author: Juan Castro
        'Description:   Retrieves records from the tblSubmission and tblSubmissionSnapshot tables.
        'Parameters:    lngSubmissionID - parameter that identifies records in the tables
        'Returns:       ADODB.Recordset
        '-------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim cmdSQL As ADODB.Command



        rstSQL = New ADODB.Recordset
        cmdSQL = New ADODB.Command
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)
        cmdSQL.ActiveConnection = cnnSQL

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspSelUnsentElectClaims"
            .CommandType = adCmdStoredProc
        End With

        'Execute the stored procedure
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        FetchUnsentElectClaims = rstSQL

        'disconnect the recordset
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        cnnSQL = Nothing

        'Signal successful completion


    End Function

    Public Sub SendElectClaim(ByVal lngElectClaimID As Long, ByVal strFileName As String, ByVal strUserName As String, Optional ByVal strDataBase As String = "")
        '--------------------------------------------------------------------------------------
        'Date: 10/13/00
        'Author: Juan Castro
        'Description:   Updates fields fldQueuedYN, fldResubmittalYN, fldLastSubmitDate, and
        '               fldTimesSubmitted in table tblSubmission
        'Parameters:    lngSubmissionID - identifies row to be updated
        'Returns:       Null
        '--------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdElectClaimSent"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamInput, , lngElectClaimID))
            .Parameters.Append(.CreateParameter("@FileName", adVarChar, adParamInput, 50, strFileName))
            .Parameters.Append(.CreateParameter("@Username", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection
        cnnSQL = New ADODB.Connection
        Select Case strDataBase
            Case "Test"
                cnnSQL.Open(_ConnectionString)
            Case "PsyquelDirect"
                cnnSQL.Open(_ConnectionString)
            Case Else
                cnnSQL.Open(_ConnectionString)
        End Select

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


    Public Sub GenerateClaim(ByVal lngClaimID As Long, Optional ByVal blnQueueClaim As Boolean = False, Optional ByVal strUserName As String = "")
        '--------------------------------------------------------------------------------------
        'Date: 12/09/2002
        'Author: Dave Richkun
        'Description:   Updates tblClaim, filling all related HCFA data for a specific claim
        'Parameters:    lngClaimID - ID of claim that is to be generated
        '               blnQueueClaim - Identifiies if claim should be re-queued
        'Returns:       Null
        '--------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdCreateSingleClaim"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ClaimID", adInteger, adParamInput, , lngClaimID))
            .Parameters.Append(.CreateParameter("@QueuedYN", adVarChar, adParamInput, 1, IIf(blnQueueClaim = True, "Y", "N")))
            .Parameters.Append(.CreateParameter("@Username", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection
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

    Public Function FetchClaimsByCriteria(ByVal strQueuedYN As String, ByVal lngELID As Long, _
                        ByVal lngPatientID As Long, ByVal strPatientLast As String, _
                        ByVal strPatientFirst As String, ByVal lngProviderID As Long, _
                        ByVal strProviderLast As String, ByVal strProviderFirst As String, _
                        ByVal lngInsuranceID As Long, ByVal lngCPCID As Long, ByVal dtDOSFrom As Date, _
                        ByVal dtDOSTo As Date, ByVal dtSubmitFrom As Date, ByVal dtSubmitTo As Date, _
                        ByVal strEClaimYN As String, Optional ByVal strDataBase As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------------------------
        'Date: 01/23/2003
        'Author: Dave Richkun
        'Description: Returns claims from tblClaim using dynamic criteria
        'Parameters: All parameters are criteria to use in searching for claims
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim rstSQL As ADODB.Recordset
        Dim cmdSQL As ADODB.Command



        rstSQL = New ADODB.Recordset
        cmdSQL = New ADODB.Command
        cnnSQL = New ADODB.Connection

        Select Case strDataBase
            Case "Test"
                cnnSQL.Open(_ConnectionString)
            Case "PsyquelDirect"
                cnnSQL.Open(_ConnectionString)
            Case Else
                cnnSQL.Open(_ConnectionString)
        End Select

        cmdSQL.ActiveConnection = cnnSQL

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspSelClaimsByCriteria"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@QueuedYN", adVarChar, adParamInput, 1, strQueuedYN))
            .Parameters.Append(.CreateParameter("@ELID", adInteger, adParamInput, , lngELID))
            .Parameters.Append(.CreateParameter("@PatientID", adInteger, adParamInput, , lngPatientID))
            .Parameters.Append(.CreateParameter("@PatientLast", adVarChar, adParamInput, 30, strPatientLast))
            .Parameters.Append(.CreateParameter("@PatientFirst", adVarChar, adParamInput, 30, strPatientFirst))
            .Parameters.Append(.CreateParameter("@ProviderID", adInteger, adParamInput, , lngProviderID))
            .Parameters.Append(.CreateParameter("@ProviderLast", adVarChar, adParamInput, 30, strProviderLast))
            .Parameters.Append(.CreateParameter("@ProviderFirst", adVarChar, adParamInput, 30, strProviderFirst))
            .Parameters.Append(.CreateParameter("@InsuranceID", adInteger, adParamInput, , lngInsuranceID))
            .Parameters.Append(.CreateParameter("@CPCID", adInteger, adParamInput, , lngCPCID))
            .Parameters.Append(.CreateParameter("@DOSFrom", adDBTimeStamp, adParamInput, , IIf(CLng(dtDOSFrom.ToOADate()) = 0, DBNull.Value, dtDOSFrom)))
            .Parameters.Append(.CreateParameter("@DOSTo", adDBTimeStamp, adParamInput, , IIf(CLng(dtDOSTo.ToOADate()) = 0, DBNull.Value, dtDOSTo)))
            .Parameters.Append(.CreateParameter("@SubmitFrom", adDBTimeStamp, adParamInput, , IIf(CLng(dtSubmitFrom.ToOADate()) = 0, DBNull.Value, dtSubmitFrom)))
            .Parameters.Append(.CreateParameter("@SubmitTo", adDBTimeStamp, adParamInput, , IIf(CLng(dtSubmitTo.ToOADate()) = 0, DBNull.Value, dtSubmitTo)))
            .Parameters.Append(.CreateParameter("@ElectYN", adVarChar, adParamInput, 1, strEClaimYN))
        End With

        'Execute the stored procedure
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        FetchClaimsByCriteria = rstSQL

        'Disconnect the recordset
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        cnnSQL = Nothing

        'Signal successful completion



    End Function


    Public Sub SubmitClaim(ByVal dtSubmitDate As Date, ByVal lngClaimID As Long, _
                           ByVal lngELID As Long, ByVal lngPlanID As Long, _
                           ByVal lngTxTypeID As Long, ByVal lngBatchNum As Long, _
                           ByVal strUserName As String, Optional ByVal strDataBase As String = "")
        '--------------------------------------------------------------------------------------
        'Date: 01/24/2003
        'Author: Dave Richkun
        'Description: Records information to identify when a claim was submitted, either
        '             via a printed claim or an electronic claim.
        'Parameters:
        'Returns: None
        '--------------------------------------------------------------------------------------
        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdClaimSubmitted"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@SubmitDate", adDBTimeStamp, adParamInput, , dtSubmitDate))
            .Parameters.Append(.CreateParameter("@ClaimID", adInteger, adParamInput, , lngClaimID))
            .Parameters.Append(.CreateParameter("@ELID", adInteger, adParamInput, , lngELID))
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
            .Parameters.Append(.CreateParameter("@TxTypeID", adInteger, adParamInput, , lngTxTypeID))
            .Parameters.Append(.CreateParameter("@BatchNum", adInteger, adParamInput, , lngBatchNum))
            .Parameters.Append(.CreateParameter("@Username", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection
        cnnSQL = New ADODB.Connection
        Select Case strDataBase
            Case "Test"
                cnnSQL.Open(_ConnectionString)
            Case "PsyquelDirect"
                cnnSQL.Open(_ConnectionString)
            Case Else
                cnnSQL.Open(_ConnectionString)
        End Select

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


    End Sub


    Public Function FetchEFileXRef(ByVal lngEFileID As Long) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 03/03/2003
        'Author: Dave Richkun
        'Description: Retrieves Psyquel-related claim information for a given E-Claim
        'Parameters: lngEFileID - ID of the THIN claim to which we will cross-reference
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

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspSelEFileXRef"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@EFileID", adInteger, adParamInput, , lngEFileID))
        End With

        'Execute the stored procedure
        rstSQL.CursorLocation = adUseClient
        rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

        FetchEFileXRef = rstSQL

        'Disconnect the recordset
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        rstSQL.ActiveConnection = Nothing
        cnnSQL = Nothing

        'Signal successful completion



    End Function


    Public Sub PostRejection(ByVal lngELID As Long, ByVal lngEFileID As Long, _
                             ByVal strMsg As String, ByVal strUserName As String, _
                             Optional ByVal dtePost As Date = Nothing, Optional ByVal lngPlanID As Long = 0, _
                             Optional ByVal strDataBase As String = "")
        '--------------------------------------------------------------------------------------
        'Date: 03/03/2003
        'Author: Dave Richkun
        'Description: Posts a rejection transaction to a claim
        'Parameters: lngELID - ID of encounter
        '            lngEFileID - ID of the electronic claim
        '            strMsg - Rejection message from the payer/clearinghouse
        'Returns: Null
        '--------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspInsClaimRejection"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ELID", adInteger, adParamInput, , lngELID))
            .Parameters.Append(.CreateParameter("@EFileID", adInteger, adParamInput, , lngEFileID))
            .Parameters.Append(.CreateParameter("@EFileMsg", adVarChar, adParamInput, 800, Left(strMsg, 800)))
            .Parameters.Append(.CreateParameter("@PostDate", adDBTimeStamp, adParamInput, , IIf(CLng(dtePost.ToOADate()) = 0, DBNull.Value, dtePost)))
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection
        cnnSQL = New ADODB.Connection
        Select Case strDataBase
            Case "Test"
                cnnSQL.Open(_ConnectionString)
            Case "PsyquelDirect"
                cnnSQL.Open(_ConnectionString)
            Case Else
                cnnSQL.Open(_ConnectionString)
        End Select

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

    Public Sub UndoRejection(ByVal lngELID As Long)
        '--------------------------------------------------------------------------------------
        'Date: 03/06/2003
        'Author: Dave Richkun
        'Description: Removes the last rejection transaction from a claim
        'Parameters: lngELID - ID of encounter
        'Returns: Null
        '--------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUndoClaimRejection"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ELID", adInteger, adParamInput, , lngELID))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection
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


    Public Sub PostTransaction(ByVal lngELID As Long, ByVal lngEFileID As Long, _
                               ByVal strMsg As String, ByVal strUserName As String, _
                               Optional ByVal dtePost As Date = Nothing, Optional ByVal lngPlanID As Long = 0, _
                               Optional ByVal strDataBase As String = "")
        '--------------------------------------------------------------------------------------
        'Date: 03/03/2003
        'Author: Dave Richkun
        'Description: Posts a non-rejection transaction to a claim
        'Parameters: lngELID - ID of encounter
        '            lngEFileID - ID of the electronic claim
        '            strMsg - Rejection message from the payer/clearinghouse
        'Returns: Null
        '--------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspInsClaimTransaction"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@ELID", adInteger, adParamInput, , lngELID))
            .Parameters.Append(.CreateParameter("@EFileID", adInteger, adParamInput, , lngEFileID))
            .Parameters.Append(.CreateParameter("@EFileMsg", adVarChar, adParamInput, 800, Left(strMsg, 800)))
            .Parameters.Append(.CreateParameter("@PostDate", adDBTimeStamp, adParamInput, , IIf(CLng(dtePost.ToOADate()) = 0, DBNull.Value, dtePost)))
            .Parameters.Append(.CreateParameter("@PlanID", adInteger, adParamInput, , lngPlanID))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection
        cnnSQL = New ADODB.Connection
        Select Case strDataBase
            Case "Test"
                cnnSQL.Open(_ConnectionString)
            Case "PsyquelDirect"
                cnnSQL.Open(_ConnectionString)
            Case Else
                cnnSQL.Open(_ConnectionString)
        End Select

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


        Exit Sub


        'Signal incompletion and raise the error to the ing enviroment
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()

    End Sub


    Public Function FetchNextBatchNumber() As Long
        '-------------------------------------------------------------------------------------
        'Date: 03/10/2003
        'Author:Dave Richkun
        'Description: Returns the next print batch number to be used when submitting a print
        '             job of claims.  This method was introduced to replace the faulty method of
        '             printing using the submission date.
        'Parameters: None
        'Returns: Batch number to be assigned to the print job.
        '-------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the command object
        cmdSQL = New ADODB.Command

        'Create the parameter objects
        With cmdSQL
            .CommandText = "uspGetTableID"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@TableName", adVarChar, adParamInput, 50, "tblClaimPrintBatch"))
            .Parameters.Append(.CreateParameter("@ID", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection.
        cnnSQL = New ADODB.Connection
        cnnSQL.Open(_ConnectionString)

        'Assign the connection to the Command object and execute the stored procedure
        cmdSQL.ActiveConnection = cnnSQL
        cmdSQL.Execute()


        If cmdSQL.Parameters("@ID").Value > 0 Then
            FetchNextBatchNumber = cmdSQL.Parameters("@ID").Value
        Else
            FetchNextBatchNumber = -1
        End If

        'Close the connection
        cnnSQL.Close()

        'Free all resources
        cmdSQL = Nothing
        cnnSQL = Nothing

        'Signal successful completion

        Exit Function


        'Signal incompletion and raise the error to the ing enviroment
        cmdSQL.Execute()
        cmdSQL = Nothing
        cnnSQL = Nothing
        'RaiseError(Err, Err.Number, CLASS_NAME)
    End Function


    Public Sub UpdateClaimICN(ByVal lngID As Long, _
                       ByVal strICN As String)
        '--------------------------------------------------------------------
        'Date: 01/09/2017                                                   '
        'Author: Rick Duane C Orth                                          '
        'Description:  Update the claim ICN number                          '
        'Parameters: lngID - ID of the row in the table whose value will be '
        '               updated.                                            '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command
        Dim strSQL As String
        Dim blnFlag As String



        'Prepare the SQL statement
        strSQL = "UPDATE "
        strSQL = strSQL & TABLE_NAME
        strSQL = strSQL & " SET "
        strSQL = strSQL & " fldICN = '" & strICN & "', "
        strSQL = strSQL & " fldClaimFrequency = 7 "
        strSQL = strSQL & " WHERE "
        strSQL = strSQL & " fldClaimID = " & lngID

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
        cmdSQL.ActiveConnection = Nothing
        cmdSQL = Nothing
        cnnSQL.Close()
        cnnSQL = Nothing

        'Signal successful completion


        Exit Sub


        'Signal incompletion and raise the error to the ing environment.
        cmdSQL.Execute()
        cmdSQL = Nothing
        cnnSQL = Nothing
        'err.raise(Err.Number, Err.Source, Err.Description)

    End Sub

    Public Sub UpdateEFileAccept(ByVal lngEFileID As Long, ByVal lngAccept As Long, _
                               ByVal lngReject As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------------------------
        'Date: 01/23/2008
        'Author: Duane C Orth
        'Description: Updates Accept and Reject amounts
        'Parameters:lngEFileID - ID of the electronic claim
        'Returns: Null
        '--------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdEFileAccept"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@EFileID", adInteger, adParamInput, , lngEFileID))
            .Parameters.Append(.CreateParameter("@Accept", adInteger, adParamInput, , CLng(lngAccept)))
            .Parameters.Append(.CreateParameter("@Reject", adInteger, adParamInput, , CLng(lngReject)))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection
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


        Exit Sub


        'Signal incompletion and raise the error to the ing enviroment
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()

    End Sub


    Public Sub UpdateEFileAck(ByVal lngEFileID As Long, ByVal strAK1 As String, _
                               ByVal strAK2 As String, ByVal strAkDetail As String, _
                               ByVal strAK5 As String, ByVal strAK9 As String, _
                               ByVal strUserName As String)
        '--------------------------------------------------------------------------------------
        'Date: 01/23/2008
        'Author: Duane C Orth
        'Description: Updates Accept and Reject amounts
        'Parameters:lngEFileID - ID of the electronic claim
        'Returns: Null
        '--------------------------------------------------------------------------------------

        Dim cnnSQL As ADODB.Connection
        Dim cmdSQL As ADODB.Command



        'Instantiate and prepare the command object.
        cmdSQL = New ADODB.Command

        With cmdSQL
            .CommandText = "uspUpdEFileAck"
            .CommandType = adCmdStoredProc
            .Parameters.Append(.CreateParameter("@EFileID", adInteger, adParamInput, , lngEFileID))
            .Parameters.Append(.CreateParameter("@AK1", adVarChar, adParamInput, 512, Mid(strAK1, 1, 512)))
            .Parameters.Append(.CreateParameter("@AK2", adVarChar, adParamInput, 512, Mid(strAK2, 1, 512)))
            .Parameters.Append(.CreateParameter("@AkDetail", adVarChar, adParamInput, 512, Mid(strAkDetail, 1, 512)))
            .Parameters.Append(.CreateParameter("@AK5", adVarChar, adParamInput, 512, Mid(strAK5, 1, 512)))
            .Parameters.Append(.CreateParameter("@AK9", adVarChar, adParamInput, 512, Mid(strAK9, 512)))
            .Parameters.Append(.CreateParameter("@UserName", adVarChar, adParamInput, 30, strUserName))
            .Parameters.Append(.CreateParameter("@SQLErrorNum", adInteger, adParamOutput, , 0))
        End With

        'Acquire the database connection
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


        Exit Sub


        'Signal incompletion and raise the error to the ing enviroment
        'RaiseError(Err, cmdSQL.Parameters("@SQLErrorNum").Value, CLASS_NAME)
        cmdSQL = Nothing
        cnnSQL = Nothing
        cmdSQL.Execute()

    End Sub
End Class
