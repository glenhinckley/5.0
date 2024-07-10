VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 2  'RequiresTransaction
END
Attribute VB_Name = "CPatientCopayBZ"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True

'--------------------------------------------------------------------
'Class Name: CPatientCopayBZ
'Date: 10/31/2000
'Author: Morgan Travis
'Description:  MTS object designed to host methods associated with
'               data affecting Patient Copay table.
'--------------------------------------------------------------------
'Revision History:
'   R001 : David Nichol, 01/24/2001 : Added UpdateRows
'   R002 : Joshua Lockwood, 02/02/01 : Added Copay Validator to UpdateRows
'   R003 : Joshua Lockwood and David Nichol
'   R004 : Joshua Lockwood, 02/09/01 : Cut ValidVisits(), VerifyData() ...
'       All is handled by CCopayValidator now.
'--------------------------------------------------------------------
Option Explicit

Private Const CLASS_NAME As String = "CPatientCopayBZ"

Public Function Insert(ByVal lngPatRPPlanID As Long, ByVal strCPTCode As String, _
                       ByVal lngProviderCredential As Long, ByVal strSeverity As String, _
                       ByVal lngVisitStart As Long, ByVal lngVisitEnd As Long, _
                       ByVal curRNCDollar As Currency, ByVal curRNCPercent As Currency, _
                       ByVal curCopayPercent As Currency, ByVal curCopay As Currency) As Long
'--------------------------------------------------------------------
'Date: 10/31/2000
'Author: Morgan Travis
'Description:  Inserts a row into the tblPatientCopay table utilizing
'              a stored procedure.
'Parameters: lngPatRPPlanID - Patient Responsible Party Plan ID number to insert into table
'            strCPTCode - The ten digit CPT Code that will be inserted into the table.
'            lngProviderCredential - Numerical representation of providers credentials.
'            strSeverity - DSM-IV Code for treatment
'            lngVisitStart - Starting visit for plan info
'            lngVisitEnd - Ending visit for plan info
'            curRNCDollar - Amount paid to out of plan providers
'            curRNCPercent - Percentage paid to out of plan providers
'            curCopayPercent - Copay percent paid to in plan providers
'            curCopay - Copay amount paid to in plan providers
'--------------------------------------------------------------------
    
    ' Declare DB object (connection)
    Dim objPatientCopay As BenefitsDB.CPatientCopayDB
    ' Declare buffer for err msg
    Dim strErrMessage As String
    ' Set tag for err trap code
    On Error GoTo ErrTrap:
    
    ' Validate Patient RP Plan ID

    If lngPatRPPlanID < 1 Then
        strErrMessage = "Patient RP Plan ID Not Valid"
        GoTo ErrTrap
    End If

    ' Instantiate DB obj (connection)
    Set objPatientCopay = CreateObject("BenefitsDB.CPatientCopayDB")
    
    ' Fire Insert method
    Insert = objPatientCopay.Insert(lngPatRPPlanID, strCPTCode, lngProviderCredential, _
                                    strSeverity, lngVisitStart, lngVisitEnd, curRNCDollar, _
                                    curRNCPercent, curCopayPercent, curCopay)
'    Insert = 0

    'Signal successful completion
    GetObjectContext.SetComplete
    ' Set DB obj to null
    Set objPatientCopay = Nothing
    ' Exit on success
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set objPatientCopay = Nothing
    
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, CLASS_NAME, Err.Description
    End If
End Function
Public Function UpdateRows(ByRef lngPatientRPPlanID As Variant, ByRef aryPatCopayID As Variant, _
                       ByRef aryCPTCode As Variant, ByRef aryProviderCredential As Variant, _
                       ByRef arySeverity As Variant, ByRef aryVisitStart As Variant, _
                       ByRef aryVisitEnd As Variant, ByRef aryRNCDollar As Variant, _
                       ByRef aryRNCPercent As Variant, ByRef aryCopayPercent As Variant, _
                       ByRef aryCopay As Variant) As String
''''''''''''''''''''''''''''
' This here doesn't work, Dave wins.
'Public Function UpdateRows(ByVal lngPatientRPPlanID As Long, aryPatCopayID() As Long, _
                       aryCPTCode() As String, aryProviderCredential() As Long, _
                       arySeverity() As String, aryVisitStart() As Long, aryVisitEnd() As Long, _
                       aryRNCDollar() As Currency, aryRNCPercent() As Currency, aryCopayPercent() As Currency, _
                       aryCopay() As Currency) As String
'--------------------------------------------------------------------
'Date: 01/24/2001
'Author: David Nichol
'Description:  Is passed an array of rows for a given patient that correspond with
'              tblPatientCoPay rows and updates, inserts, and deletes rows
'              from that table to match the rows passed.  Before doing this,
'              however, it verifies business rules.
'Parameters: lngPatRPPlanID - Patient Responsible Party Plan ID number to insert into table
'            strCPTCode - The ten digit CPT Code that will be inserted into the table.
'            lngProviderCredential - Numerical representation of providers credentials.
'            arySeverity - DSM-IV Code for treatment
'            lngVisitStart - Starting visit for plan info
'            lngVisitEnd - Ending visit for plan info
'            curRNCDollar - Amount paid to out of plan providers
'            dblRNCPercent - Percentage paid to out of plan providers
'            dblCopayPercent - Copay percent paid to in plan providers
'            curCopay - Copay amount paid to in plan providers
'--------------------------------------------------------------------

    Dim objPatientCopay As BenefitsDB.CPatientCopayDB
    Dim objCopayValidator As BenefitsBz.CCopayValidator 'R002
    Dim aryErrMsg As Variant
    Dim strErrMsg, strReturnMsg As String
    Dim i As Integer   'counter variable
    Dim lngTest As Long 'testVar

    On Error GoTo ErrTrap:

    '*** create objects ***
    Set objCopayValidator = CreateObject("BenefitsBz.CCopayValidator")
    Set objPatientCopay = CreateObject("BenefitsDB.CPatientCopayDB")    'R002

    '*** validate rows ***
    aryErrMsg = objCopayValidator.TestCopay(aryProviderCredential, arySeverity, aryCPTCode, aryVisitStart, aryVisitEnd, aryCopayPercent, _
                                            aryCopay, aryRNCPercent, aryRNCDollar)  'R002

'AAAAAAAAAAAAAAAAAAAAAGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGHHHHHHHHHHHHHHHHHHHHHHHHHH!!!!!!!!!!!!!!
    '*** split error message array elements into cr delimeted string ***
    strReturnMsg = ""  'R002
    'Make sure the damn thing is an array first (produces Invalid OLEVERB structure if not)
    If (IsArray(aryErrMsg)) Then 'Making sure here
        For i = 0 To UBound(aryErrMsg)
        ' KoolNotez: (Invalid OLEVERB structure [ASP]) is returned if some stupid array
        '   declaration like 'Dim aryArray() As String' is used.  Notice no upper
        '   bound given in declaration...curious.
            If Not aryErrMsg(i) = "" Then
                strReturnMsg = strReturnMsg & aryErrMsg(i) & vbCrLf
            End If
        Next
    End If 'Done making sure. R003
'AAAAAAAAAAAAAAAAAAAAAGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGHHHHHHHHHHHHHHHHHHHHHHHHHH!!!!!!!!!!!!!!

    '*** if there is an error message, exit ***
    If Not Len(strReturnMsg) > 0 Then  'R002
        For i = 0 To UBound(aryProviderCredential)
            If aryPatCopayID(i) > 0 Then
                'Update the current row
                Call objPatientCopay.Update(aryPatCopayID(i), aryCPTCode(i), aryProviderCredential(i), _
                      arySeverity(i), aryVisitStart(i), aryVisitEnd(i), aryRNCDollar(i), aryRNCPercent(i), _
                      aryCopayPercent(i), aryCopay(i))
            Else
                'Insert a new copay row
                lngTest = Insert(lngPatientRPPlanID, aryCPTCode(i), aryProviderCredential(i), _
                    arySeverity(i), aryVisitStart(i), aryVisitEnd(i), aryRNCDollar(i), aryRNCPercent(i), _
                    aryCopayPercent(i), aryCopay(i))
            End If
        Next 'i
    End If
    
    UpdateRows = strReturnMsg 'Return err msg, if okay, empty is returned. R003
    
    Set objPatientCopay = Nothing
    Set objCopayValidator = Nothing 'R002

    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set objCopayValidator = Nothing
    Set objPatientCopay = Nothing
    
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMsg
    Else
        Err.Raise Err.Number, CLASS_NAME, Err.Description
    End If
End Function

Public Sub Update(ByVal lngPatRPPlanID As Long, _
                       ByVal strCPTCode As String, ByVal lngProviderCredential As Long, _
                       ByVal strSeverity As String, ByVal lngVisitStart As Long, ByVal lngVisitEnd As Long, _
                       ByVal curRNCDollar As Currency, ByVal curRNCPercent As Currency, ByVal curCopayPercent As Currency, _
                       ByVal curCopay As Currency, ByVal lngPatCopayID As Long)
'--------------------------------------------------------------------
'Date: 10/31/2000
'Author: Morgan Travis
'Description:  Updates a row into the tblPatientCopay table utilizing
'              a stored procedure.
'Parameters: lngPatRPPlanID - Patient Responsible Party Plan ID number to insert into table
'            strCPTCode - The ten digit CPT Code that will be inserted into the table.
'            lngProviderCredential - Numerical representation of providers credentials.
'            strSeverity - DSM-IV Code for treatment
'            lngVisitStart - Starting visit for plan info
'            lngVisitEnd - Ending visit for plan info
'            curRNCDollar - Amount paid to out of plan providers
'            dblRNCPercent - Percentage paid to out of plan providers
'            dblCopayPercent - Copay percent paid to in plan providers
'            curCopay - Copay amount paid to in plan providers
'
'--------------------------------------------------------------------
    
    Dim objPatientCopay As BenefitsDB.CPatientCopayDB
    Dim strErrMsg As String
    
    On Error GoTo ErrTrap:
    
    Set objPatientCopay = CreateObject("BenefitsDB.CPatientCopayDB")
    
    'Verify RPPlanID
    If lngPatRPPlanID < 1 Then
        strErrMsg = "Patient RP Plan ID Not Valid"
        GoTo ErrTrap
    End If

    Call objPatientCopay.Update(lngPatCopayID, strCPTCode, lngProviderCredential, _
                  strSeverity, lngVisitStart, lngVisitEnd, curRNCDollar, curRNCPercent, _
                  curCopayPercent, curCopay)
    Set objPatientCopay = Nothing
    
    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set objPatientCopay = Nothing
    
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMsg
    Else
        Err.Raise Err.Number, CLASS_NAME, Err.Description
    End If
End Sub


Public Function Exists(ByVal lngPatRPPlanID As Long, _
                       ByVal strCPTCode As String, ByVal lngProviderCredential As Long, _
                       ByVal strSeverity As String, ByVal lngVisitStart As Long) As Boolean
'--------------------------------------------------------------------
'Date: 10/31/2000
'Author: Morgan Travis
'Description:  Checks for existing entries into tblPatientCopay
'Parameters: lngPatRPPlanID - Provider ID number to check for existing entries in table
'            strCPTCode - The ten digit CPT Code that will be inserted into the table.
'            lngProviderCredential - The integer associated with Providers credentials.
'
'--------------------------------------------------------------------
    Dim objPatientCopay As BenefitsDB.CPatientCopayDB
    Dim blnExists As Boolean
    
    On Error GoTo ErrTrap:
    
    Set objPatientCopay = CreateObject("BenefitsDB.CPatientCopayDB")
    blnExists = objPatientCopay.Exists(lngPatRPPlanID, strCPTCode, lngProviderCredential, strSeverity, lngVisitStart)
    Set objPatientCopay = Nothing
    Exists = blnExists
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    On Error GoTo 0
    GetObjectContext.SetAbort
    Set objPatientCopay = Nothing
        
    Err.Raise Err.Number, CLASS_NAME, Err.Description
End Function

Public Function FetchByPatRPPlan(ByVal lngPatRPPlanID As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 10/31/2000                                                   '
'Author: Morgan Travis                                              '
'Description:  Retrieves records from the tblProviderFee table.     '
'Parameters: lngPatRPPlanID - Patient Responsible Party Plan ID number to insert into table
'            strCPTCode - The ten digit CPT Code that will be inserted into the table.
'            lngProviderCredential - Numerical representation of providers credentials.
'
'Returns: Recordset of requested providers
'-------------------------------------------------------------------'

    Dim objPatientCopay As BenefitsDB.CPatientCopayDB
    Dim rstSQL As ADODB.Recordset
    Dim blnExists As Boolean
    On Error GoTo ErrTrap:
    
    Set objPatientCopay = CreateObject("BenefitsDB.CPatientCopayDB")
           
        Set rstSQL = objPatientCopay.FetchByPatRPPlan(lngPatRPPlanID)
        
    Set FetchByPatRPPlan = rstSQL
    Set objPatientCopay = Nothing
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set objPatientCopay = Nothing
    Set rstSQL = Nothing
    Err.Raise Err.Number, CLASS_NAME, Err.Description
End Function


'R004
'Public Function ValidVisits(ByVal lngPatRPPlanID As Long, _
'                            ByVal strCPTCode As String, ByVal lngProviderCredential As Long, _
'                            ByVal strSeverity As String, ByVal lngVisitStart As Long, ByVal lngVisitEnd As Long) As Boolean
'
'    Dim objBenefit As BenefitsBz.CPatientCopayBZ
'    Dim rst As ADODB.Recordset
'
'    Set objBenefit = CreateObject("BenefitsBz.CPatientCopayBZ")
'    Set rst = objBenefit.FetchByPatRPPlanCPTCredential(lngPatRPPlanID, strCPTCode, lngProviderCredential, strSeverity, lngVisitStart)
'
'    ValidVisits = True
'
'    Do While Not rst.EOF
'
'
'
'        If lngVisitStart > rst.Fields("fldVisitStart").Value And lngVisitStart <= rst.Fields("fldVisitEnd").Value Then
'            ValidVisits = False
'        End If
'
'        If lngVisitStart > (rst.Fields("fldVisitEnd").Value + 1) Then
'            ValidVisits = False
'        End If
'
'        If lngVisitStart = (rst.Fields("fldVisitEnd").Value + 1) Then
'            ValidVisits = True
'            Set rst = Nothing
'            Set objBenefit = Nothing
'            Exit Function
'
'        End If
'
'        If lngVisitStart <= rst.Fields("fldVisitStart").Value And lngVisitEnd >= rst.Fields("fldVisitStart").Value Then
'            ValidVisits = False
'        End If
'
'    rst.MoveNext
'    Loop
'
'    Set rst = Nothing
'    Set objBenefit = Nothing
'
'End Function

Public Function FetchByPatRPPlanCPTCredential(ByVal lngPatRPPlanID As Long, ByVal strCPTCode As String, _
                                 ByVal lngProviderCredential As Long, ByVal strSeverity As String, ByVal lngVisitStart As Long) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 10/31/2000                                                   '
'Author: Morgan Travis                                              '
'Description:  Retrieves records from the tblProviderFee table.     '
'Parameters: lngPatRPPlanID - Patient Responsible Party Plan ID number to insert into table
'            strCPTCode - The ten digit CPT Code that will be inserted into the table.
'            lngProviderCredential - Numerical representation of providers credentials.
'
'Returns: Recordset of requested providers
'-------------------------------------------------------------------'

    Dim objPatientCopay As BenefitsDB.CPatientCopayDB
    Dim rstSQL As ADODB.Recordset
    Dim blnExists As Boolean
    On Error GoTo ErrTrap:
    
    Set objPatientCopay = CreateObject("BenefitsDB.CPatientCopayDB")
        
    'blnExists = objPatientCopay.Exists(lngPatRPPlanID, strCPTCode, lngProviderCredential, strSeverity, lngVisitStart)
    
    'If blnExists Then
        Set rstSQL = objPatientCopay.FetchByPatRPPlanCPTCredential(lngPatRPPlanID, strCPTCode, lngProviderCredential, strSeverity)
    'Else
    '    strCPTCode = "OPEN"
    '    Set rstSQL = objPatientCopay.FetchByPatRPPlanCPTCredential(lngPatRPPlanID, strCPTCode, lngProviderCredential, strSeverity)
    'End If
    
    Set FetchByPatRPPlanCPTCredential = rstSQL
    Set objPatientCopay = Nothing
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set objPatientCopay = Nothing
    Set rstSQL = Nothing
    Err.Raise Err.Number, CLASS_NAME, Err.Description
    

End Function

Public Function Delete(ByVal lngPatCopayID As Long)
'--------------------------------------------------------------------
'Date: 10/31/2000
'Author: Morgan Travis
'Description:  Deletes a row into the tblPatientCopay table utilizing
'              a stored procedure.
'Parameters: lngPatCopayID - Patient Responsible Party Plan ID number to insert into table
'
'--------------------------------------------------------------------

    Dim objPatientCopay As BenefitsDB.CPatientCopayDB
    Dim strErrMsg As String
    
    On Error GoTo ErrTrap:
    
    Set objPatientCopay = CreateObject("BenefitsDB.CPatientCopayDB")
    
    Call objPatientCopay.Delete(lngPatCopayID)
    
    Set objPatientCopay = Nothing
    
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Set objPatientCopay = Nothing
    
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMsg
    Else
        Err.Raise Err.Number, CLASS_NAME, Err.Description
    End If
End Function
