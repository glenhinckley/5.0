VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CPatientDiagnosisBZ"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CPatientDiagnosisBZ
'Date: 09/06/2000
'Author: Joshua Lockwood
'Description:  MTS business object designed to call methods
'              associated with the CPatientDiagnosisDB class.
'--------------------------------------------------------------------
'Revision History:
'   R001 : Joshua Lockwood, 3OCT00 : Added method that checks whether
'       PAtient ID already exists. (For insert)
'
'--------------------------------------------------------------------
Option Explicit

Private Const CLASS_NAME As String = "CPatientDiagnosisBZ"
Private Const PATIENTDIAGNOSIS_TABLE As String = "tblPatientDiagnosis"
Public Function FetchByID(ByVal lngPatientDiagnosisID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 09/06/2000
'Author: Joshua Lockwood
'Description:  Retrieves records from the tblPatientDiagnosis table.
'Parameters: lngPatientDiagnosisID - ID number of the record to retrieve, which
'               is primary key of table.
'Returns: ADODB.Recordset -- should only return one record
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declares database object
    Dim objPatientDiagnosis As EMRDB.CPatientDiagnosisDB
    ' Declares recordset for result returned
    Dim rstSQL As ADODB.Recordset
    
    ' Duh
    On Error GoTo ErrTrap:

    ' Instantiates database onject
    Set objPatientDiagnosis = CreateObject("EMRDB.CPatientDiagnosisDB")
    
    ' Instantiates recordset for result returned
    Set rstSQL = objPatientDiagnosis.FetchByID(lngPatientDiagnosisID)
    
    ' Return value set for function
    Set FetchByID = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete
    ' Set DB object to NULL
    Set objPatientDiagnosis = Nothing
    ' Exit on success
    Exit Function
    
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objPatientDiagnosis = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function

Public Function FetchByPatient(ByVal lngPatientID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 09/06/2000
'Author: Joshua Lockwood
'Description:  Retrieves records from the tblPatientDiagnosis table.
'Parameters: lngPatientID - Unique ID associated with patient specified.
'            blnIncludeDisabled - boolean value -- whether to include
'               disabled rows (true) or not (false).
'Returns: ADODB.Recordset
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------
    
    Dim objPatientDiagnosis As EMRDB.CPatientDiagnosisDB
    Dim rstSQL As ADODB.Recordset

    On Error GoTo ErrTrap:

    ' Instantiate DB object
    Set objPatientDiagnosis = CreateObject("EMRDB.CPatientDiagnosisDB")
    
    ' Call fetch method and set recordset returned to rstSQL
    Set rstSQL = objPatientDiagnosis.FetchByPatient(lngPatientID, blnIncludeDisabled)
    
    ' Set recordset to retval
    Set FetchByPatient = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objPatientDiagnosis = Nothing   ' Delete object
    Exit Function
    
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objPatientDiagnosis = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function
Public Function Insert(ByVal lngPatientID As Long, ByVal strUserName As String, _
                       Optional ByVal lngAxisIa As Long, Optional ByVal lngAxisIb As Long, _
                       Optional ByVal lngAxisIc As Long, Optional ByVal lngAxisId As Long, _
                       Optional ByVal lngAxisIe As Long, Optional ByVal lngAxisIf As Long, _
                       Optional ByVal lngAxisIg As Long, Optional ByVal lngAxisIh As Long, _
                       Optional ByVal strDxNotes As String _
                    ) As Long
'--------------------------------------------------------------------
'Date: 09/06/2000
'Author: Joshua Lockwood
'Description:  Inserts a row into the tblPatientDiagnosis table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'Returns: ID (Primary Key) of the row inserted.
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------
    ' Declare DB object (connection)
    Dim objPatientDiagnosis As EMRDB.CPatientDiagnosisDB
    ' Declare buffer for err msg
    Dim strErrMessage As String
    ' Set tag for err trap code
    
    On Error GoTo ErrTrap:
    
   ' Validate data being passed...Leave out lngPatientDiagnosisID
    If Not VerifyData(strErrMessage, , lngPatientID, _
                lngAxisIa, lngAxisIb, lngAxisIc, lngAxisId, _
                lngAxisIe, lngAxisIf, lngAxisIg, lngAxisIh, _
                strDxNotes _
            ) Then GoTo ErrTrap
       
    ' Instantiate DB obj (connection)
    Set objPatientDiagnosis = CreateObject("EMRDB.CPatientDiagnosisDB")
    
    ' Fire Insert method
    Insert = objPatientDiagnosis.Insert(lngPatientID, strUserName, _
                lngAxisIa, lngAxisIb, lngAxisIc, lngAxisId, _
                lngAxisIe, lngAxisIf, lngAxisIg, lngAxisIh, _
                strDxNotes)
        
    'Signal successful completion
    GetObjectContext.SetComplete
    ' Set DB obj to null
    Set objPatientDiagnosis = Nothing
    ' Exit on success
    Exit Function
    
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    ' Close DB connection
    Set objPatientDiagnosis = Nothing
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Function
Public Sub Update(ByVal lngPatientDiagnosisID As Long, _
                       Optional ByVal lngAxisIa As Long, Optional ByVal lngAxisIb As Long, _
                       Optional ByVal lngAxisIc As Long, Optional ByVal lngAxisId As Long, _
                       Optional ByVal lngAxisIe As Long, Optional ByVal lngAxisIf As Long, _
                       Optional ByVal lngAxisIg As Long, Optional ByVal lngAxisIh As Long, _
                       Optional ByVal strDxNotes As String)
'--------------------------------------------------------------------
'Date: 09/06/2000
'Author: Joshua Lockwood
'Description:  Updates a row in the tblPatientDiagnosis table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'Returns: Null
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------
    
    ' Declare database object
    Dim objPatientDiagnosis As EMRDB.CPatientDiagnosisDB
    
    ' Declare error message string
    Dim strErrMessage As String

    On Error GoTo ErrTrap:

   ' Validate data...leave username and patientID options blank
    If Not VerifyData(strErrMessage, lngPatientDiagnosisID, , _
                lngAxisIa, lngAxisIb, lngAxisIc, lngAxisId, _
                lngAxisIe, lngAxisIf, lngAxisIg, lngAxisIh, _
                strDxNotes _
            ) Then GoTo ErrTrap
                      
    ' Instantiate database object
    Set objPatientDiagnosis = CreateObject("EMRDB.CPatientDiagnosisDB")
    
    ' Run database object
    objPatientDiagnosis.Update lngPatientDiagnosisID, _
                lngAxisIa, lngAxisIb, lngAxisIc, lngAxisId, _
                lngAxisIe, lngAxisIf, lngAxisIg, lngAxisIh, _
                strDxNotes
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objPatientDiagnosis = Nothing
    Exit Sub

ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    
    Set objPatientDiagnosis = Nothing
    
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Sub

Public Sub Deleted(ByVal lngPatientDiagnosisID As Long, Optional ByVal blnIncludeDisabled As Boolean = False)
'--------------------------------------------------------------------
'Date: 09/06/2000
'Author: Joshua Lockwood
'Description:  Flags a row in the tblPatientDiagnosis table marking the row as
'              deleted or undeleted.
'Parameters: lngPatientDiagnosisID - ID of the row in the table whose value will be
'               updated.
'            blnIncludeDisabled - boolean value -- whether to include
'            disabled rows (true) or not (false).
'Returns: Null
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Declare DB obj
    Dim objPatientDiagnosis As EMRDB.CPatientDiagnosisDB
    
    On Error GoTo ErrTrap:

    ' Instanitate DB obj
    Set objPatientDiagnosis = CreateObject("EMRDB.CPatientDiagnosisDB")
    
    ' Fire delete method
    Call objPatientDiagnosis.Deleted(lngPatientDiagnosisID, blnIncludeDisabled)
    
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objPatientDiagnosis = Nothing
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set objPatientDiagnosis = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Sub

Private Function PatientExists(ByVal lngPatientID As Long) As Boolean
'--------------------------------------------------------------------
'Date: 10/3/2000
'Author: Joshua Lockwood
'Description:  Verifies existence of PAtientID in DB.  Added with R001.
'Parameters: lngPatientID - ID for Fetch by Patient
'Returns: Boolean
'--------------------------------------------------------------------
    Dim objEMRTemplate As EMRDB.CEMRTemplateDB
    Dim rstSQL As ADODB.Recordset
    Dim intCtr As Integer

    On Error GoTo ErrTrap:

    Set rstSQL = FetchByPatient(lngPatientID, True)
    
    'Check to see if there is already a diagnosis for patient
    If (rstSQL.RecordCount > 0) Then
        PatientExists = True
        Exit Function
    End If

    'Signal successful completion
    GetObjectContext.SetComplete
    Set rstSQL = Nothing
    
    PatientExists = False
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    Set rstSQL = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description
End Function

Private Function VerifyData(ByRef strErrMessage As String, _
    Optional ByVal lngPatientDiagnosisID As Long, Optional ByVal lngPatientID As Long, _
    Optional ByVal lngAxisIa As Long, Optional ByVal lngAxisIb As Long, _
    Optional ByVal lngAxisIc As Long, Optional ByVal lngAxisId As Long, _
    Optional ByVal lngAxisIe As Long, Optional ByVal lngAxisIf As Long, _
    Optional ByVal lngAxisIg As Long, Optional ByVal lngAxisIh As Long, _
    Optional ByVal strDxNotes As String) As Boolean
'--------------------------------------------------------------------
'Date: 09/06/2000
'Author: Joshua Lockwood
'Description:  Verifies all required data has been provided by the user.
'              Last three params are optional to take into account the
'              different needs for update and insert.  Boolean value doesn't
'              be validated because all bool values are acceptable.  Due to
'              the nature of upd and ins calls, all params (save error) are
'              Optional.
'Parameters:  The values to be checked.
'Returns: Boolean value identifying if all criteria has been satisfied.
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    ' Verify that the PatientDiagnosisID is valid; must be >= 0 (1st index==1)
    If lngPatientDiagnosisID < 0 Then
        strErrMessage = "Patient Diagnosis ID number provided is not valid."
        VerifyData = False
        Exit Function
    End If

    ' Verify that the PatientID is valid; must be >= 0 (1st index==1)
    If lngPatientID < 0 Then
        strErrMessage = "Patient ID number provided is not valid."
        VerifyData = False
        Exit Function
    End If
    
    If PatientExists(lngPatientID) Then
        strErrMessage = "Diagnosis already exists for this patient. Please use update instead."
        VerifyData = False
        Exit Function
    End If
    
    ' Verify that the Primary is valid; must be >= 0
    If lngAxisIa < 0 Then
        strErrMessage = "Number for Primary Dx is not valid."
        VerifyData = False
        Exit Function
    End If
    
    ' Verify that the Secondary is valid; must be >= 0
    If lngAxisIb < 0 Then
        strErrMessage = "Number for Secondary Dx is not valid."
        VerifyData = False
        Exit Function
    End If
    
    ' Verify that the Tertiary is valid; must be >= 0
    If lngAxisIc < 0 Then
        strErrMessage = "Number for Tertiary Dx is not valid."
        VerifyData = False
        Exit Function
    End If
    
    ' Verify that the Forth is valid; must be >= 0
    If lngAxisId < 0 Then
        strErrMessage = "Number for Forth Dx is not valid."
        VerifyData = False
        Exit Function
    End If
    
    ' Verify that the Fifth is valid; must be >= 0
    If lngAxisIe < 0 Then
        strErrMessage = "Number for Fifth Dx is not valid."
        VerifyData = False
        Exit Function
    End If
    
    ' Verify that the Sixth is valid; must be >= 0
    If lngAxisIf < 0 Then
        strErrMessage = "Number for Sixth Dx is not valid."
        VerifyData = False
        Exit Function
    End If
    
    ' Verify that the Seventh is valid; must be >= 0
    If lngAxisIg < 0 Then
        strErrMessage = "Number for Seventh Dx is not valid."
        VerifyData = False
        Exit Function
    End If
    
    ' Verify that the Eighth is valid; must be >= 0
    If lngAxisIh < 0 Then
        strErrMessage = "Number for Eighth Dx is not valid."
        VerifyData = False
        Exit Function
    End If
    ' Verify that the AxisIIa is valid; must be >= 0
 '   If lngAxisIIa < 0 Then
 '       strErrMessage = "Number for AxisIIa is not valid."
 '       VerifyData = False
 '       Exit Function
 '   End If
 '
    ' Verify that the AxisIIb is valid; must be >= 0
 '   If lngAxisIIb < 0 Then
 '       strErrMessage = "Number for AxisIIb is not valid."
 '       VerifyData = False
 '       Exit Function
 '   End If

    'If we get here, all is well...
    VerifyData = True

End Function





