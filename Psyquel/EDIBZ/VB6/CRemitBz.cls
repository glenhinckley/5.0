VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CRemitBZ"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'-------------------------------------------------------------------------------------
'Date: 04/20/2009
'Class Name: RemitBz
'Author: Duane C Orth
'Description:   COM object designed to host methods associated with electronic
'               checks.
'--------------------------------------------------------------------------------------
Option Explicit

Private Const CLASS_NAME As String = "CRemitBz"
Private Const TABLE_NAME As String = "tblRemittance"

Public Function Fetch(Optional blnIncludeDisabled As Boolean = False, _
                      Optional ByVal strWhere As String, _
                      Optional ByVal strOrderBy As String) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 04/20/2009
'Author: Duane C Orth
'Description:  Retrieves records from the tblRemittance table.
'Parameters: blnIncludeDisabled - Optional parameter that identifies if
'               records flagged as 'Disabled' or 'De-activated' are to be
'               included in the record set. The default value is False.
'            strWhere - Optional 'Where' clause of the SQL statement by
'               which records will be filtered.  If the parameter is not
'               supplied, all records will be retrieved.
'            strOrderBy - Optional 'Order By' clause in which retrieved
'               records will be sorted.  The default is set to the
'               'fldName' column
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    
    Dim rstSQL As ADODB.Recordset
    Dim objRemit As EDIDB.CRemitDB
    
    On Error GoTo ErrTrap:
    
    Set objRemit = CreateObject("EDIDB.CRemitDB")
    Set rstSQL = objRemit.Fetch(blnIncludeDisabled, strWhere, strOrderBy)
    Set Fetch = rstSQL
    
    Set objRemit = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Function

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objRemit = Nothing
    Set rstSQL = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function

Public Function Insert(ByVal strPayorID As String, ByVal strHealthPlanID As String, ByVal strHealthPlanName As String, _
                       ByVal dteCreationDate As Date, ByVal strBatchNumber As String, ByVal strProvider As String, ByVal strProviderNbr As String, _
                       ByVal strCheckNumber As String, ByVal detCheckDate As Date, ByVal dblCheckAmount As Double, ByVal dblAmountPaid As Double, _
                       ByVal dblInterestAmount As Double, ByVal dteRemittanceDate As Date, ByVal dteProcessDate As Date, ByVal strPatientNumber As String, _
                       ByVal strInsuredGroupNbr As String, ByVal strInsuredPolicyID As String, ByVal strInsuredLastName As String, ByVal strInsuredFirstName As String, _
                       ByVal strInsuredMiddleInitial As String, ByVal strPatientGroupNbr As String, ByVal strPatientPolicyID As String, ByVal strPatientLastName As String, _
                       ByVal strPatientFirstName As String, ByVal strPatientMiddleInitial As String, ByVal strPatientSex As String, _
                       ByVal dtePatientDateOfBirth As Date, ByVal blnCompInsuranceFlag As Boolean, ByVal strMedicalRecordNumber As String, _
                       ByVal strMessageCode1 As String, ByVal strMessageCode2 As String, ByVal strMessageCode3 As String, ByVal strMessageCode4 As String, ByVal strMessageCode5 As String, _
                       ByVal strSupplementalInsurerName1 As String, ByVal strSupplementalInsurerID1 As String, ByVal strSupplementalInsurerName2 As String, ByVal strSupplementalInsurerID2 As String, _
                       ByVal lngInsuranceID As Long, ByVal lngProviderID As Long, ByVal lngPatientID As Long, ByVal lngEncounterLogID As Long, ByVal lngCheckID As Long, _
                       ByVal strErnFileName As String, ByVal blnPosted As Boolean, ByVal dtePostedDate As Date, ByVal strErrorMessage As String, _
                       ByVal strAddedBy As String) As Long
'--------------------------------------------------------------------
'Date: 04/20/2009
'Author: Duane C Orth
'Description:  Inserts a row into the tblRemittance table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'Returns: ID (Primary Key) of the row inserted.
'--------------------------------------------------------------------
    
    Dim objRemit As EDIDB.CRemitDB
    Dim strErrMessage As String
    
    On Error GoTo ErrTrap:
    
    Set objRemit = CreateObject("EDIDB.CRemitDB")
    Insert = objRemit.Insert(strPayorID, strHealthPlanID, strHealthPlanName, _
                       dteCreationDate, strBatchNumber, strProvider, strProviderNbr, _
                       strCheckNumber, detCheckDate, dblCheckAmount, dblAmountPaid, _
                       dblInterestAmount, dteRemittanceDate, dteProcessDate, strPatientNumber, _
                       strInsuredGroupNbr, strInsuredPolicyID, strInsuredLastName, strInsuredFirstName, _
                       strInsuredMiddleInitial, strPatientGroupNbr, strPatientPolicyID, strPatientLastName, _
                       strPatientFirstName, strPatientMiddleInitial, strPatientSex, _
                       dtePatientDateOfBirth, blnCompInsuranceFlag, strMedicalRecordNumber, _
                       strMessageCode1, strMessageCode2, strMessageCode3, strMessageCode4, strMessageCode5, _
                       strSupplementalInsurerName1, strSupplementalInsurerID1, strSupplementalInsurerName2, strSupplementalInsurerID2, _
                       lngInsuranceID, lngProviderID, lngPatientID, lngEncounterLogID, lngCheckID, _
                       strErnFileName, blnPosted, dtePostedDate, strErrorMessage, _
                       strAddedBy)
    Set objRemit = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    Set objRemit = Nothing
    GetObjectContext.SetAbort
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Function

Public Sub Update(ByVal lngID As Long, ByVal strPayorID As String, ByVal strHealthPlanID As String, ByVal strHealthPlanName As String, _
                  ByVal dteCreationDate As Date, ByVal strBatchNumber As String, ByVal strProvider As String, ByVal strProviderNbr As String, _
                  ByVal strCheckNumber As String, ByVal detCheckDate As Date, ByVal dblCheckAmount As Double, ByVal dblAmountPaid As Double, _
                  ByVal dblInterestAmount As Double, ByVal dteRemittanceDate As Date, ByVal dteProcessDate As Date, ByVal strPatientNumber As String, _
                  ByVal strInsuredGroupNbr As String, ByVal strInsuredPolicyID As String, ByVal strInsuredLastName As String, ByVal strInsuredFirstName As String, _
                  ByVal strInsuredMiddleInitial As String, ByVal strPatientGroupNbr As String, ByVal strPatientPolicyID As String, ByVal strPatientLastName As String, _
                  ByVal strPatientFirstName As String, ByVal strPatientMiddleInitial As String, ByVal strPatientSex As String, _
                  ByVal dtePatientDateOfBirth As Date, ByVal blnCompInsuranceFlag As Boolean, ByVal strMedicalRecordNumber As String, _
                  ByVal strMessageCode1 As String, ByVal strMessageCode2 As String, ByVal strMessageCode3 As String, ByVal strMessageCode4 As String, ByVal strMessageCode5 As String, _
                  ByVal strSupplementalInsurerName1 As String, ByVal strSupplementalInsurerID1 As String, ByVal strSupplementalInsurerName2 As String, ByVal strSupplementalInsurerID2 As String, _
                  ByVal lngInsuranceID As Long, ByVal lngProviderID As Long, ByVal lngPatientID As Long, ByVal lngEncounterLogID As Long, ByVal lngCheckID As Long, _
                  ByVal strErnFileName As String, ByVal blnPosted As Boolean, ByVal dtePostedDate As Date, ByVal strErrorMessage As String, _
                  ByVal strAddedBy As String)
'--------------------------------------------------------------------
'Date: 04/20/2009
'Author: Duane C Orth
'Description:  Updates a row in the tblRemittance table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'Returns: Null
'--------------------------------------------------------------------
    
    Dim objRemit As EDIDB.CRemitDB
    Dim strErrMessage As String

    On Error GoTo ErrTrap:

    Set objRemit = CreateObject("EDIDB.CRemitDB")
    Call objRemit.Update(lngID, strPayorID, strHealthPlanID, strHealthPlanName, _
                       dteCreationDate, strBatchNumber, strProvider, strProviderNbr, _
                       strCheckNumber, detCheckDate, dblCheckAmount, dblAmountPaid, _
                       dblInterestAmount, dteRemittanceDate, dteProcessDate, strPatientNumber, _
                       strInsuredGroupNbr, strInsuredPolicyID, strInsuredLastName, strInsuredFirstName, _
                       strInsuredMiddleInitial, strPatientGroupNbr, strPatientPolicyID, strPatientLastName, _
                       strPatientFirstName, strPatientMiddleInitial, strPatientSex, _
                       dtePatientDateOfBirth, blnCompInsuranceFlag, strMedicalRecordNumber, _
                       strMessageCode1, strMessageCode2, strMessageCode3, strMessageCode4, strMessageCode5, _
                       strSupplementalInsurerName1, strSupplementalInsurerID1, strSupplementalInsurerName2, strSupplementalInsurerID2, _
                       lngInsuranceID, lngProviderID, lngPatientID, lngEncounterLogID, lngCheckID, _
                       strErnFileName, blnPosted, dtePostedDate, strErrorMessage, _
                       strAddedBy)
    Set objRemit = Nothing
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Sub

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    Set objRemit = Nothing
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub

