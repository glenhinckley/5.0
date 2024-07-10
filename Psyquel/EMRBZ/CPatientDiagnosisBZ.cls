
'--------------------------------------------------------------------
'Class Name: CPatientDiagnosisBZ
'Date: 09/06/2000
'Author: Joshua Lockwood
'Description:  MTS business object designed to   methods
'              associated with the CPatientDiagnosisDB class.
'--------------------------------------------------------------------
'Revision History:
'   R001 : Joshua Lockwood, 3OCT00 : Added method that checks whether
'       PAtient ID already exists. (For insert)
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
' Lydia Orth 



Public Class CPatientDiagnosisBZ

    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Dim strErrMessage As String = String.Empty
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
        Dim objPatientDiagnosis As New EMRDB.CPatientDiagnosisDB
        ' Declares recordset for result returned
        Dim rstSQL As New ADODB.Recordset



        ' Instantiates recordset for result returned
        rstSQL = objPatientDiagnosis.FetchByID(lngPatientDiagnosisID)

        ' Return value set for function
        FetchByID = rstSQL


        objPatientDiagnosis = Nothing

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

        Dim objPatientDiagnosis As New EMRDB.CPatientDiagnosisDB
        Dim rstSQL As New ADODB.Recordset



        '   fetch method and set recordset returned to rstSQL
        rstSQL = objPatientDiagnosis.FetchByPatient(lngPatientID, blnIncludeDisabled)

        ' Set recordset to retval
        FetchByPatient = rstSQL


        objPatientDiagnosis = Nothing   ' Delete object

    End Function
    Public Function Insert(ByVal lngPatientID As Long, ByVal strUserName As String, _
                           Optional ByVal lngAxisIa As Long = 0, Optional ByVal lngAxisIb As Long = 0, _
                           Optional ByVal lngAxisIc As Long = 0, Optional ByVal lngAxisId As Long = 0, _
                           Optional ByVal lngAxisIe As Long = 0, Optional ByVal lngAxisIf As Long = 0, _
                           Optional ByVal lngAxisIg As Long = 0, Optional ByVal lngAxisIh As Long = 0, _
                           Optional ByVal strDxNotes As String = "" _
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
        Dim objPatientDiagnosis As New EMRDB.CPatientDiagnosisDB
        ' Declare buffer for err msg
        Dim strErrMessage As String
        ' Set tag for err trap code



        ' Validate data being passed...Leave out lngPatientDiagnosisID
        If Not VerifyData(strErrMessage, , lngPatientID, _
                    lngAxisIa, lngAxisIb, lngAxisIc, lngAxisId, _
                    lngAxisIe, lngAxisIf, lngAxisIg, lngAxisIh, _
                    strDxNotes _
                ) Then Exit Function


        ' Fire Insert method
        Insert = objPatientDiagnosis.Insert(lngPatientID, strUserName, _
                    lngAxisIa, lngAxisIb, lngAxisIc, lngAxisId, _
                    lngAxisIe, lngAxisIf, lngAxisIg, lngAxisIh, _
                    strDxNotes)


        objPatientDiagnosis = Nothing
        ' Exit on success

    End Function
    Public Sub Update(ByVal lngPatientDiagnosisID As Long, _
                           Optional ByVal lngAxisIa As Long = 0, Optional ByVal lngAxisIb As Long = 0, _
                           Optional ByVal lngAxisIc As Long = 0, Optional ByVal lngAxisId As Long = 0, _
                           Optional ByVal lngAxisIe As Long = 0, Optional ByVal lngAxisIf As Long = 0, _
                           Optional ByVal lngAxisIg As Long = 0, Optional ByVal lngAxisIh As Long = 0, _
                           Optional ByVal strDxNotes As String = "")
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
        Dim objPatientDiagnosis As New EMRDB.CPatientDiagnosisDB



        ' Validate data...leave username and patientID options blank
        If Not VerifyData(strErrMessage, lngPatientDiagnosisID, , _
                    lngAxisIa, lngAxisIb, lngAxisIc, lngAxisId, _
                    lngAxisIe, lngAxisIf, lngAxisIg, lngAxisIh, _
                    strDxNotes _
                ) Then Exit Sub



        ' Run database object
        objPatientDiagnosis.Update(lngPatientDiagnosisID, _
                    lngAxisIa, lngAxisIb, lngAxisIc, lngAxisId, _
                    lngAxisIe, lngAxisIf, lngAxisIg, lngAxisIh, _
                    strDxNotes)


        objPatientDiagnosis = Nothing

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
        Dim objPatientDiagnosis As New EMRDB.CPatientDiagnosisDB


        ' Fire delete method
        objPatientDiagnosis.Deleted(lngPatientDiagnosisID, blnIncludeDisabled)


        objPatientDiagnosis = Nothing

    End Sub

    Private Function PatientExists(ByVal lngPatientID As Long) As Boolean
        '--------------------------------------------------------------------
        'Date: 10/3/2000
        'Author: Joshua Lockwood
        'Description:  Verifies existence of PAtientID in DB.  Added with R001.
        'Parameters: lngPatientID - ID for Fetch by Patient
        'Returns: Boolean
        '--------------------------------------------------------------------
        Dim objEMRTemplate As New EMRDB.CEMRTemplateDB
        Dim rstSQL As New ADODB.Recordset
        Dim intCtr As Integer



        rstSQL = FetchByPatient(lngPatientID, True)

        'Check to see if there is already a diagnosis for patient
        If (rstSQL.RecordCount > 0) Then
            PatientExists = True
            Exit Function
        End If

        rstSQL = Nothing

        PatientExists = False

    End Function

    Private Function VerifyData(ByRef strErrMessage As String, _
        Optional ByVal lngPatientDiagnosisID As Long = 0, Optional ByVal lngPatientID As Long = 0, _
        Optional ByVal lngAxisIa As Long = 0, Optional ByVal lngAxisIb As Long = 0, _
        Optional ByVal lngAxisIc As Long = 0, Optional ByVal lngAxisId As Long = 0, _
        Optional ByVal lngAxisIe As Long = 0, Optional ByVal lngAxisIf As Long = 0, _
        Optional ByVal lngAxisIg As Long = 0, Optional ByVal lngAxisIh As Long = 0, _
        Optional ByVal strDxNotes As String = "") As Boolean
        '--------------------------------------------------------------------
        'Date: 09/06/2000
        'Author: Joshua Lockwood
        'Description:  Verifies all required data has been provided by the user.
        '              Last three params are optional to take into account the
        '              different needs for update and insert.  Boolean value doesn't
        '              be validated because all bool values are acceptable.  Due to
        '              the nature of upd and ins  s, all params (save error) are
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

End Class




