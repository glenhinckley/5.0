
'--------------------------------------------------------------------
'Class Name: CPatRPPlanBz
'Date: 02/06/2000
'Author: Eric Pena
'Description:  MTS object designed to fetch from the tblPatRPPlan table.
'--------------------------------------------------------------------
'Revision History:                                                  '
' R001 - 11/07/2000 Travis:  Moved FetchPatRPPlanRule to CPatRPPlanRuleBZ               '
' R002 - 11/07/2000 Travis:  Moved original Update() to CPatRPPlanRuleBZ
' R003 - 06/15/2000 Richkun: Removed IsEnabled() check from SetOrder.
'           The Pat-RP-Plan record will we re-enabled if it is found
'           to be disabled.
' R004 - 07/24/2000 Richkun: Added Append() method.
' R005 - 03/22/2001 Nichol: Added ExistsPatRPPlan()
' R006 - 05/16/2001 Richkun: Added FetchDisabledRPPlansByPatient()
' R007 - 01/24/2002 Richkun: Added FetchPatientRPNames()
'--------------------------------------------------------------------
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII
Imports BenefactorDB

Public Class CPatRPPlanBz

    Private _ConnectionString As String = String.Empty


    Private Const TABLE_NAME As String = "tblPatRPPlan"
    Private Const CLASS_NAME As String = "CPatRPPlanDB"


    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property


    Public Function FetchPlans(ByVal lngPatientID As Long, ByVal lngRPID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/06/2000
        'Author: Eric Pena
        'Description:  Returns a Object 2d-array containing a list of all Plans and levels to which a Patient-RP combo is associated.
        'Parameters: lngPatientID - The ID of the Patient
        '                    lngRPID - - The ID of the RP
        'Returns: An ADODB.Recordset containing a list of all plans and levels  to which a Patient-RP combo is associated.
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objPatRPPlan As New BenefactorDB.CPatRPPlanDB
        objPatRPPlan.ConnectionString = _ConnectionString


        FetchPlans = objPatRPPlan.FetchPlans(lngPatientID, lngRPID)
        objPatRPPlan = Nothing

    End Function

    Public Function IsEnabled(ByVal lngPatientID As Long, ByVal lngRPID As Long, ByVal lngPlanID As Long) As Boolean
        '--------------------------------------------------------------------
        'Date: 02/06/2000
        'Author: Eric Pena
        'Description:  Lets the  er know if a given Patient-RP-Plan combo association is active
        'Parameters: lngPatientID - The ID of the Patient
        '                  lngRPID - The ID of the RP
        '                  lngPlanID - The ID of the Plan
        'Returns: T if the association is enabled/exists, F otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objPatRPPlan As New CPatRPPlanDB
        objPatRPPlan.ConnectionString = _ConnectionString
        If objPatRPPlan.IsEnabled(lngPatientID, lngRPID, lngPlanID) > 0 Then
            IsEnabled = True
        Else
            IsEnabled = False
        End If
        'Signal successful completion
        objPatRPPlan = Nothing


    End Function

    Public Function Append(ByVal lngPatientID As Long, ByVal lngRPID As Long, ByVal lngPlanID As Long, ByVal strAddedBy As String) As Long
        '--------------------------------------------------------------------
        'Date: 07/24/2000
        'Author: Dave Richkun
        'Description:  Creates a new Patient-RP-Plan combination.  The Order value
        '              is automati y assigned the next available number based on
        '              existing plan assignments.
        'Parameters: lngPatientID - The ID of the Patient
        '            lngRPID - The ID of the RP
        '            lngPlanID - The ID of the Plan
        'Returns: ID of the new row inserted into tblPatRPPlan
        '--------------------------------------------------------------------
        'Revision History:
        '  R004 - Created
        '--------------------------------------------------------------------


        Dim objPatRPPlan As New BenefactorDB.CPatRPPlanDB

        objPatRPPlan.ConnectionString = _ConnectionString

        Append = objPatRPPlan.Append(lngPatientID, lngRPID, lngPlanID, strAddedBy)

        objPatRPPlan = Nothing

    End Function


    Public Function Insert(ByVal lngPatientID As Long, ByVal lngRPID As Long, ByVal lngPlanID As Long, ByVal intOrder As Integer, ByVal strAddedBy As String) As Long
        '--------------------------------------------------------------------
        'Date: 03/01/2000
        'Author: Eric Pena
        'Description:  Creates a billing association for the given patient-rp-plan combonation
        'Parameters: lngPatientID - The ID of the Patient
        '                  lngRPID - The ID of the RP
        '                  lngPlanID - The ID of the Plan
        'Returns: T if the association is enabled/exists, F otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objPatRPPlan As New BenefactorDB.CPatRPPlanDB
        objPatRPPlan.ConnectionString = _ConnectionString
        Insert = objPatRPPlan.Insert(lngPatientID, lngRPID, lngPlanID, intOrder, strAddedBy)
        'Signal successful completion
        objPatRPPlan = Nothing

    End Function



    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngPatientID As Long, ByVal lngRPID As Long, ByVal lngPlanID As Long, ByVal strDeletedBy As String)
        '--------------------------------------------------------------------
        'Date: 03/01/2000
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

        Dim objBenefactor As New BenefactorDB.CPatRPPlanDB

        objBenefactor.ConnectionString = _ConnectionString

        'TODO: Check for active claims
        objBenefactor.Deleted(blnDeleted, lngPatientID, lngRPID, lngPlanID, strDeletedBy)


        objBenefactor = Nothing
    End Sub


    Public Sub DeleteRP(ByVal lngPatientID As Long, ByVal lngRPID As Long, _
                        ByVal strDeletedBy As String)
        '--------------------------------------------------------------------
        'Date: 07/31/2000
        'Author: Dave Richkun
        'Description:  Marks every Plan record associated with a Patient's Responsible
        '              Party as disabled.
        'Parameters:  lngPatientID - The ID of the Patient
        '             lngRPID - The ID of the RP
        '             strUserName - Login name of the user responsible for
        '               marking the rows as deleted.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim objPRPP As New BenefactorDB.CPatRPPlanDB
        objPRPP.ConnectionString = _ConnectionString



        objPRPP.DeleteRP(lngPatientID, lngRPID, strDeletedBy)

        objPRPP = Nothing

    End Sub


    Public Function SetOrder(ByVal lngPatientID As Long, ByVal lngRPID As Long, _
                        ByVal lngPlanID As Long, ByVal intOrder As Integer) As Boolean
        '--------------------------------------------------------------------
        'Date: 02/06/2000
        'Author: Eric Pena
        'Description:  Changes the order of the given Patient-RP-Plan combo association above
        'Parameters: lngPatientID - The ID of the Patient
        '                  lngRPID - The ID of the RP
        '                  lngPlanID - The ID of the Plan
        '                  intOrder - The order to assign to the association
        'Returns: True if the record was succesfully updated, otherwise False.
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objPatRPPlan As New BenefactorDB.CPatRPPlanDB
        objPatRPPlan.ConnectionString = _ConnectionString

        '    Removed R003
        '    If Not IsEnabled(lngPatientID, lngRPID, lngPlanID) Then
        '        getobjectcontext.SetAbort
        '       Exit Function
        '    End If
        SetOrder = objPatRPPlan.SetOrder(lngPatientID, lngRPID, lngPlanID, intOrder)

        objPatRPPlan = Nothing

    End Function
    Public Function FetchRPsByPat(ByVal intPatient As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 3/08/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblBenefactor table utilizing a stored procedure
        'Parameters: blnIncludeDisabled - determines if disabled records will be returned
        '                   intProvider - provider to match
        '                   strLast - last name to match
        '                   strFirst - first name to match
        'Returns: ADORst
        '--------------------------------------------------------------------

        Dim objBenefactor As New BenefactorDB.CPatRPPlanDB



        FetchRPsByPat = objBenefactor.FetchRPsByPat(intPatient, blnIncludeDisabled)

        objBenefactor = Nothing

    End Function
    Public Function FetchPlansByRP(ByVal intRPID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 3/08/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblBenefactor table utilizing a stored procedure
        'Parameters: blnIncludeDisabled - determines if disabled records will be returned
        '                   intProvider - provider to match
        '                   strLast - last name to match
        '                   strFirst - first name to match
        'Returns: ADORst
        '--------------------------------------------------------------------

        Dim objBenefactor As New BenefactorDB.CPatRPPlanDB

        objBenefactor.ConnectionString = _ConnectionString

        FetchPlansByRP = objBenefactor.FetchPlansByRP(intRPID, blnIncludeDisabled)

        objBenefactor = Nothing
    End Function

    Public Function FetchRPPlansByPat(ByVal lngPatientID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 3/08/2000
        'Author: Eric Pena
        'Description:  Returns plan information for a specific patient
        'Parameters: lngPatientID - ID of patient whose plans will be retrieved
        '            blnIncludeDisabled - determines if disabled plans will be included in
        '                   the resultset
        'Returns: Recordset of patient plans
        '--------------------------------------------------------------------

        Dim objBFact As New BenefactorDB.CPatRPPlanDB
        objBFact.ConnectionString = _ConnectionString

        FetchRPPlansByPat = objBFact.FetchRPPlansByPat(lngPatientID, blnIncludeDisabled)
        objBFact = Nothing



    End Function


    Public Function FetchDisabledRPPlansByPatient(ByVal lngPatientID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 05/16/2001
        'Author: Dave Richkun
        'Description:  Returns disabled Patient-RP-Plan rows from the tblPatRPPlan table
        '              utilizing a stored procedure
        'Parameters: lngPatientID - ID of the patient whose disabled palns will be retrieved
        'Returns: ADO Recordset
        '--------------------------------------------------------------------
        '  R006 - Created
        '--------------------------------------------------------------------

        Dim objBenefactor As New BenefactorDB.CPatRPPlanDB
        objBenefactor.ConnectionString = _ConnectionString

        FetchDisabledRPPlansByPatient = objBenefactor.FetchDisabledRPPlansByPatient(lngPatientID)

        'Signal successful completion
        objBenefactor = Nothing



    End Function



    Public Function ExistsPatRPPlan(ByVal lngPatientID As Long, ByVal lngRPID As Long, ByVal lngPlanID As Long, _
                                Optional ByVal blnIncludeDisabled As Boolean = False) As Long
        '--------------------------------------------------------------------
        'Date: 3/21/2001
        'Author: David Nichol
        'Description:  If an patient/RP/plan combination already exists, returns its ID in tblPatientRPPlan.
        '               Otherwise it returns 0.
        'Parameters: lngPatientID - patient ID
        '            lngRPID - responsible party ID
        '            lngPlanID - plan ID
        '            blnIncludeDisabled - Y/N as to whether to include disabled records, defaulting to false
        'Returns: Long - ID of the existing record, or 0
        '--------------------------------------------------------------------

        Dim objBenefactor As New BenefactorDB.CPatRPPlanDB
        objBenefactor.ConnectionString = _ConnectionString


        ExistsPatRPPlan = objBenefactor.ExistsPatRPPlan(lngPatientID, lngRPID, lngPlanID, blnIncludeDisabled)


        objBenefactor = Nothing

    End Function

    '++++++++++++                R001         +++++++++++++++++++++'

    '---------------------- Begin R005 -----------------------------
    Public Function ReorderPlan(ByVal lngPatientID As Long, ByVal intOldPriority As Integer, ByVal intNewPriority As Integer) As Boolean
        '--------------------------------------------------------------------
        'Date: 01/12/2001
        'Author: Dave Nichol
        'Description:   s DB method that changes the order of the a Patient-RP-Plan
        '   combo association, then adjusts the order of the other pat-rpplan
        '   records as needed.
        'Parameters: lngPatientID - The ID of the Patient
        '            lngOldPriority - The current order # of the plan
        '            lngNewPriority - The new order # of the plan
        'Returns: True if the record was updated, False otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '--------------------------------------------------------------------

        Dim objBenefactor As New BenefactorDB.CPatRPPlanDB
        Dim strErrMessage As String

        objBenefactor.ConnectionString = _ConnectionString

        'verify parameter
        If Not lngPatientID > 0 Then
            strErrMessage = "Patient ID is not valid."

        End If
        If Not intOldPriority > 0 Then
            strErrMessage = "Old priority is not valid."

        End If
        If Not intNewPriority > 0 Then
            strErrMessage = "New priority is not valid."

        End If

        ReorderPlan = objBenefactor.ReorderPlan(lngPatientID, intOldPriority, intNewPriority)

        objBenefactor = Nothing

    End Function

    '---------------------- End R005 -----------------------------

    Public Function FetchPatientRPNames(ByVal lngPatientID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 01/24/2002
        'Author: Dave Richkun
        'Description:  Retrieves list of Responsible Party names associated with a patient
        'Parameters: lngPatientID - ID of the Patient
        'Returns: Recordset of RP names
        '--------------------------------------------------------------------
        'Revision History:
        '--------------------------------------------------------------------

        Dim objBenefactor As New BenefactorDB.CPatRPPlanDB
        objBenefactor.ConnectionString = _ConnectionString

        FetchPatientRPNames = objBenefactor.FetchPatientRPNames(lngPatientID)

        'Signal successful completion
        objBenefactor = Nothing



    End Function

End Class

