
'--------------------------------------------------------------------
'Class Name: CPatientMedBZ
'Date: 09/07/2000
'Author: Joshua Lockwood
'Description:  MTS business object designed to   methods
'              associated with the CPatientMedDB class.
'--------------------------------------------------------------------
'Revision History:
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


Public Class CPatientMedBZ

    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Dim strErrMessage As String = String.Empty
    Private Const CLASS_NAME As String = "CPatientMedBZ"
    Private Const PATIENTMEDICATION_TABLE As String = "tblPatientMedication"

    Public Function FetchByID(ByVal lngPatientMedicationID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/07/2000
        'Author: Joshua Lockwood
        'Description:  Retrieves records from the tblPatientMedication table.
        'Parameters: lngPatientMedicationID - ID number of the record to retrieve, which
        '               is primary key of table.
        'Returns: ADODB.Recordset -- should only return one record
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        ' Declares database onject
        Dim objPatientMed As New EMRDB.CPatientMedDB
        ' Declares recordset for result returned
        Dim rstSQL As New ADODB.Recordset



        ' Instantiates recordset for result returned
        rstSQL = objPatientMed.FetchByID(lngPatientMedicationID)

        ' Return value set for function
        FetchByID = rstSQL


        objPatientMed = Nothing

    End Function

    Public Function FetchByPatient(ByVal lngPatientID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/07/2000
        'Author: Joshua Lockwood
        'Description:  Retrieves records from the tblPatientMedication table.
        'Parameters: lngPatientID - Unique ID associated with patient specified.
        '            no delete for medication
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objPatientMed As New EMRDB.CPatientMedDB
        Dim rstSQL As New ADODB.Recordset


        '   fetch method and set recordset returned to rstSQL
        rstSQL = objPatientMed.FetchByPatient(lngPatientID)

        ' Set recordset to retval
        FetchByPatient = rstSQL


        objPatientMed = Nothing   ' Delete object

    End Function
    Public Function Insert(ByVal lngPatientID As Long, ByVal strDrugName As String, _
                        ByVal strInstructions As String, ByVal strPrescriber As String, _
                        ByVal strUserName As String, Optional ByVal dtStartDate As Date = Nothing, _
                        Optional ByVal dtEndDate As Date = Nothing) As Long
        '--------------------------------------------------------------------
        'Date: 09/07/2000
        'Author: Joshua Lockwood
        'Description:  Inserts a row into the tblPatientMedication table utilizing
        '              a stored procedure.  Please ensure that the 'Date' data type
        '              is compatable with SQL's SMALLDATETIME.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        ' Declare DB object (connection)
        Dim objPatientMed As New EMRDB.CPatientMedDB
        ' Declare buffer for err msg
        Dim strErrMessage As String
        ' Set tag for err trap code



        ' Validate data...leave username and patientID options blank
        If Not VerifyData(strDrugName, strInstructions, strPrescriber, strUserName, strErrMessage, _
                     , lngPatientID, dtStartDate, dtEndDate) Then Exit Function



        ' Fire Insert method
        Insert = objPatientMed.Insert(lngPatientID, strDrugName, strInstructions, _
                        strPrescriber, strUserName, dtStartDate, dtEndDate)


        ' Set DB obj to null
        objPatientMed = Nothing
        ' Exit on success

    End Function
    Public Sub Update(ByVal lngPatientMedicationID As Long, ByVal strDrugName As String, _
                        ByVal strInstructions As String, ByVal strPrescriber As String, _
                        ByVal strUserName As String, _
                        Optional ByVal dtStartDate As Date = Nothing, Optional ByVal dtEndDate As Date = Nothing)
        '--------------------------------------------------------------------
        'Date: 09/07/2000
        'Author: Joshua Lockwood
        'Description:  Updates a row in the tblPatientMedication table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        ' Declare database object
        Dim objPatientMed As New EMRDB.CPatientMedDB



        ' Validate data...leave username and patientID options blank
        If Not VerifyData(strDrugName, strInstructions, strPrescriber, strUserName, strErrMessage, _
                     lngPatientMedicationID, , dtStartDate, dtEndDate) Then Exit Sub


        ' Run database object
        objPatientMed.Update(lngPatientMedicationID, strDrugName, strInstructions, strPrescriber, _
                        strUserName, dtStartDate, dtEndDate)


        objPatientMed = Nothing

    End Sub

    Public Sub Deleted(ByVal lngPatientMedicationID As Long, ByVal blnDisabled As Boolean, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 09/07/2000
        'Author: Joshua Lockwood
        'Description:  Flags a row in the tblPatientMedication table marking the row as
        '              deleted or undeleted.
        'Parameters: lngPatientMedicationID - ID of the row in the table whose value will be
        '               updated.
        '            blnIncludeDisabled - boolean value -- whether to include
        '            disabled rows (true) or not (false).
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        ' Declare DB obj
        Dim objPatientMed As New EMRDB.CPatientMedDB

        ' Fire delete method
        objPatientMed.Deleted(lngPatientMedicationID, blnDisabled, strUserName)


        objPatientMed = Nothing

    End Sub

    Private Function VerifyData(ByVal strDrugName As String, ByVal strInstructions As String, _
                ByVal strPrescriber As String, ByVal strUserName As String, ByRef strErrMessage As String, _
                Optional ByVal lngPatientMedicationID As Long = 0, Optional ByVal lngPatientID As Long = 0, _
                Optional ByVal dtStartDate As Date = Nothing, Optional ByVal dtEndDate As Date = Nothing _
    ) As Boolean
        '--------------------------------------------------------------------
        'Date: 09/07/2000
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

        ' Verify that the strDrugName text is valid; must contain data
        If Trim(strDrugName) = "" Then
            strErrMessage = "Text for DrugName is not valid."
            VerifyData = False
            Exit Function
        End If

        ' Verify that the strInstructions text is valid; must contain data
        If Trim(strInstructions) = "" Then
            strErrMessage = "Text for Instructions is not valid."
            VerifyData = False
            Exit Function
        End If

        ' Verify that the strPrescriber text is valid; must contain data
        If Trim(strPrescriber) = "" Then
            strErrMessage = "Text for Prescriber is not valid."
            VerifyData = False
            Exit Function
        End If

        ' Verify that the strUserName text is valid; must contain data
        If Trim(strUserName) = "" Then
            strErrMessage = "Text for UserName is not valid."
            VerifyData = False
            Exit Function
        End If

        ' Verify that the PatientMedicationID is valid; must be >= 0 (1st index==1)
        If lngPatientMedicationID < 0 Then
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

        ' Verify that the User name is valid; must have something in it
        If Not IsDate(dtStartDate) Then
            strErrMessage = "StartDate provided is not valid."
            VerifyData = False
            Exit Function
        End If

        ' Verify that the User name is valid; must have something in it
        If Not IsDate(dtEndDate) Then
            strErrMessage = "EndDate provided is not valid."
            VerifyData = False
            Exit Function
        End If

        'If we get here, all is well...
        VerifyData = True

    End Function



End Class




