
'--------------------------------------------------------------------
'Class Name: CCertificationDB                                       '
'Date: 03/07/2000                                                   '
'Author: Rick "Boom Boom" Segura                                    '
'Description:  MTS object designed to host methods associated with  '
'             the CCertificationDB Class                            '
'--------------------------------------------------------------------
' Revision History:
'   R001: Richkun - 08/16/2000 Added conditional statement to Insert
'           method before CPT-Units array is parsed.
'   R002: Richkun - 08/18/2000 Added methodology to check for existance
'           of CPT codes in VerifyData().  More processing required on
'           back end but easier for web users to enter certs.
'   R003: Richkun - 10/18/2000 Provided improved mechanism for updating
'           Authorizations and tightened up business rules.
'   R004: Richkun - 02/14/2001 Added FetchPatientUserCertsDetail() method.
'   R005 Segura - 05/07/2001 Update() rewritten                     '
'   R006 Richkun - 10/17/2001 Added FetchUncoveredDates() method
'   R007 Pena - 10/26/2001 Modified certs to not require a provider (for intake center )
'   R008 Pena - 05/29/2002 Modified cert checks to support multiple/fractional units
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
Imports Psyquel.BusinessRules.CoreLibraryIII.dbStuff

Imports CertificationDB

Public Class CCertificationBz


    Private _ConnectionString As String = String.Empty



    Private Const CLASS_NAME As String = "CCertificationBz"
    Private Const DELIM_ASC As Integer = 124    ' The "|" character
    Private Const COLON_ASC As Integer = 58     ' The ":" character
    Private Const MAX_CPT_UNITS_FROM_UI As Integer = 6


    Private _DB As New db
    Private _MD As New ModCommon

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property


    '--------------------------------------------------------------------
    '                          Public Methods                           '
    '--------------------------------------------------------------------
    Public Function Insert(ByVal lngPatID As Long, _
                    ByVal lngPlanID As Long, ByVal lngProviderID As Long, ByVal dteStart As Date, _
                    ByVal dteEnd As Date, ByVal strCertNum As String, _
                    ByVal strAddedBy As String, ByVal strNote As String, _
                    ByRef varProviders As Object, _
                    ByRef varCPTUnits As Object, _
                    ByVal lngDaysPrior As Long, ByVal lngUnitsPrior As Long) _
                    As Long
        '--------------------------------------------------------------------
        'Date: 03/08/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Authorization driver for inserting information of a   '
        '              a single cert into all appropriate tables.           '
        'Parameters: lngPatID -  ID of authorized patient                   '
        '            lngPlanID - ID of authorizing plan                     '
        '            dteStart - cert Start date                             '
        '            dteEnd - cert End Date                                 '
        '            strCertNum - plan assigned certnumber                  '
        '            strAddedBy - name of user entering cert                '
        '            strNote -  User notes on cert                          '
        '            strProviders - array of authorized providers  and info '
        '            strCPTUnits - array of delimited CPT and unit info     '
        '            lngDaysPrior - Days Prior(Hot List)                    '
        '            lngUnitsPrior - Units Prior(Hot List)                  '
        'Returns: ID of record just inserted on success, -1 otherwise       '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim objCertification As New CertificationDB.CCertificationDB
        Dim objCertProvider As New CertificationDB.CCertProviderDB
        Dim objCertUnit As New CertificationDB.CCertUnitDB
        Dim objCertCPTCode As New CertificationDB.CCertCPTCodeDB
        Dim lngCertID As Long
        Dim lngCertUnitID As Long
        Dim strCertUnitInfo As String
        Dim intCTR As Integer
        Dim intCtr2 As Integer
        Dim varCPTArray As Object
        Dim strErrMsg As String
        Dim lngErrNum As Long

        objCertification.ConnectionString = _ConnectionString
        objCertProvider.ConnectionString = _ConnectionString
        objCertProvider.ConnectionString = _ConnectionString
        objCertCPTCode.ConnectionString = _ConnectionString



        If Not VerifyData(dteStart, dteEnd, lngPatID, lngPlanID, strCertNum, _
                          strAddedBy, lngProviderID, varCPTUnits, strErrMsg, _
                          lngDaysPrior, lngUnitsPrior, 0) Then
            Exit Function
        End If
        lngErrNum = Err.Number
        If lngErrNum > 0 Then
            Exit Function
        End If


        ' First insert core Authorization info and get Cert ID from tblCertification

        lngCertID = objCertification.Insert(lngPatID, lngPlanID, lngProviderID, dteStart, _
                        dteEnd, strCertNum, strAddedBy, strNote, lngDaysPrior, _
                        lngUnitsPrior)
        lngErrNum = Err.Number




        If lngErrNum > 0 Then
            Exit Function
        End If
        objCertification = Nothing

        '    If IsArray(varProviders) Then  'R007 if -1 passed instead of providers dont try to validate
        '        'Use Cert ID to associate all authorized providers
        '        For intCtr = 0 To UBound(varProviders)
        '            Set objCertProvider = CreateObjectXXXXXXX("CertificationDB.CCertProviderDB")
        '              objCertProvider.Insert(lngCertID, varProviders(intCtr))
        '            Set objCertProvider = Nothing
        '        Next
        '    Else
        '        lngErrNum = -1
        '        strErrMsg = "Provider array is not an array."
        '        GoTo 
        '    End If

        ' Insert all cert units an retrieve CertUnit ID from tblCertUnit


        For intCTR = 0 To UBound(varCPTUnits, 1)
            lngCertUnitID = objCertUnit.Insert(varCPTUnits(intCTR, 1), varCPTUnits(intCTR, 2))
            If lngCertID < 0 Then
                lngErrNum = Err.Number
                strErrMsg = "An error occurred while trying to insert a record into tblCertUnit."
                Exit Function
            End If

            varCPTArray = varCPTUnits(intCTR, 3)
            For intCtr2 = 0 To UBound(varCPTArray)
                objCertCPTCode.Insert(lngCertID, varCPTArray(intCtr2), lngCertUnitID, strAddedBy)

                lngErrNum = Err.Number
                If lngErrNum > 0 Then
                    Exit Function
                End If
            Next
        Next

        objCertCPTCode = Nothing
        objCertUnit = Nothing
        objCertification = Nothing
        objCertProvider = Nothing
        objCertUnit = Nothing
        objCertCPTCode = Nothing
        Insert = lngCertID


    End Function


    Public Sub Delete(ByVal lngCertID As Long)
        '--------------------------------------------------------------------
        'Date: 03/09/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Deletes all records associated with lngCertID in     '
        '              tables tblCertification, tblCertProvider,            '
        '              tblCertUnit, and tblCertCPTCode if it has not been   '
        '              used                                                 '
        'Parameters: lngCertID - ID of cert to be deleted                   '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------
        Dim objCertification As New CertificationDB.CCertificationDB
        objCertification.ConnectionString = _ConnectionString


        objCertification.Delete(lngCertID)

        ' Signal successful completion

        objCertification = Nothing


    End Sub


    Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Long)
        '--------------------------------------------------------------------
        'Date: 06/06/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Flags a row in the tblCertification table marking the'
        '           row as enabled or disabled.                             '
        'Parameters: blnDeleted - Boolean value identifying if the record is'
        '               to be disabled (True) or enabled (False).           '
        '            lngID - ID of the row in the table whose value will be '
        '               updated.                                            '
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objCert As New CertificationDB.CCertificationDB
        Dim strFlag As String

        objCert.ConnectionString = _ConnectionString

        If blnDeleted Then
            strFlag = "Y"
        Else
            strFlag = "N"
        End If


        objCert.Deleted(strFlag, lngID)

        objCert = Nothing



    End Sub


    Public Function FetchCertDetail(ByVal lngCertID As Long) As Collection
        '--------------------------------------------------------------------
        'Date: 03/09/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Retrieves recordset containing detailed information  '
        '              of the cert having the ID given as the argument      '
        'Parameters: lngCertID -  ID of cert being described                '
        'Returns: Recordstets of detailed information                       '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim objCertification As New CertificationDB.CCertificationDB

        objCertification.ConnectionString = _ConnectionString

        FetchCertDetail = objCertification.FetchCertDetail(lngCertID)



        objCertification = Nothing



    End Function


    Public Function FetchHotListByManager(ByVal lngManagerID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/15/2001                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Retrieves the Authorization Hot List for a given     '
        '             Provider ID                                           '
        'Parameters: lngProviderID - ID of Provider                         '
        'Returns: Recordsets of detailed information                        '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim objCertification As New CertificationDB.CCertificationDB
        objCertification.ConnectionString = _ConnectionString

        FetchHotListByManager = objCertification.FetchHotListByManager(lngManagerID)



        objCertification = Nothing



    End Function

    Public Function FetchHotListByProvider(ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/15/2001                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Retrieves the Authorization Hot List for a given     '
        '             Provider ID                                           '
        'Parameters: lngProviderID - ID of Provider                         '
        'Returns: Recordsets of detailed information                        '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim objCertification As New CertificationDB.CCertificationDB

        objCertification.ConnectionString = _ConnectionString


        FetchHotListByProvider = objCertification.FetchHotListByProvider(lngProviderID)



        objCertification = Nothing



    End Function
    Public Sub RollBack(ByVal lngELID As Long, ByVal strUserName As String, Optional ByVal lngRPID As Long = -1, Optional ByVal lngPlanID As Long = -1)
        '--------------------------------------------------------------------
        'Date: 07/24/2002
        'Author: Dave Richkun
        'Description: Rolls back Authorization units associated with a claim and
        '             deletes history of usage from tblCertUsage.  Actions are
        '             audited in tblCertAudit.
        'Parameters:  lngELID - ID of the claim whose Authorization units are being
        '               rolled back.
        '             strUsername - Login name of user performng this action
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim objRollBack As New CertificationDB.CCertUnitDB
        Dim objAudit As New CertificationDB.CCertAuditDB
        Dim rstRollBack As New ADODB.Recordset
        Dim strAudit As String

        objRollBack.ConnectionString = _ConnectionString
        objAudit.ConnectionString = _ConnectionString






        rstRollBack = objRollBack.FetchRollBackUnits(lngELID, lngRPID, lngPlanID)

        While Not rstRollBack.EOF
            objRollBack.RollBackUnit(rstRollBack.Fields("fldCertUnitID").Value, _
                    rstRollBack.Fields("fldCertUsageID").Value, rstRollBack.Fields("fldUnits").Value)

            strAudit = "Remaining Units:" & rstRollBack.Fields("fldRemainingUnits").Value
            strAudit = strAudit & ":" & (CLng(rstRollBack.Fields("fldRemainingUnits").Value) + rstRollBack.Fields("fldUnits").Value)

            If objAudit.Insert(rstRollBack.Fields("fldCertID").Value, strUserName, strAudit) < 1 Then
                Exit Sub
            End If

            rstRollBack.MoveNext()
        End While

        'Free resources
        rstRollBack = Nothing
        objRollBack = Nothing
        objAudit = Nothing


    End Sub

    Public Sub RollBackDetLine(ByVal lngEncDetailID As Long, ByVal strUserName As String, Optional ByVal lngRPID As Long = -1, Optional ByVal lngPlanID As Long = -1)
        '--------------------------------------------------------------------
        'Date: 07/24/2002
        'Author: Dave Richkun
        'Description: Rolls back Authorization units associated with a claim and
        '             deletes history of usage from tblCertUsage.  Actions are
        '             audited in tblCertAudit.
        'Parameters:  lngELID - ID of the claim whose Authorization units are being
        '               rolled back.
        '             strUsername - Login name of user performng this action
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim objRollBack As New CertificationDB.CCertUnitDB
        Dim objAudit As New CertificationDB.CCertAuditDB
        Dim rstRollBack As New ADODB.Recordset
        Dim strAudit As String



        objRollBack.ConnectionString = _ConnectionString
        objAudit.ConnectionString = _ConnectionString


        rstRollBack = objRollBack.FetchRollBackDetUnits(lngEncDetailID, lngRPID, lngPlanID)

        While Not rstRollBack.EOF
            objRollBack.RollBackUnit(rstRollBack.Fields("fldCertUnitID").Value, _
                  rstRollBack.Fields("fldCertUsageID").Value, rstRollBack.Fields("fldUnits").Value)

            strAudit = "Remaining Units:" & rstRollBack.Fields("fldRemainingUnits").Value
            strAudit = strAudit & ":" & (CLng(rstRollBack.Fields("fldRemainingUnits").Value) + rstRollBack.Fields("fldUnits").Value)

            If objAudit.Insert(rstRollBack.Fields("fldCertID").Value, strUserName, strAudit) < 1 Then
                Exit Sub
            End If

            rstRollBack.MoveNext()
        End While

        'Free resources
        rstRollBack = Nothing
        objRollBack = Nothing
        objAudit = Nothing



    End Sub
    '--------------------------------------------------------------------
    '                         Private Methods                           '
    '--------------------------------------------------------------------
    Private Function GetCertID(ByVal lngPatID As Long, _
                             ByVal lngPlanID As Long, _
                             ByVal lngProviderID As Long, _
                             ByVal dteDate As Date, _
                             ByVal strCPTCode As String) As Long
        '--------------------------------------------------------------------
        'Date: 04/10/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description: Determines if a DOS is covered by an authorization of '
        '             specific plan                                         '
        'Parameters:  lngPatID - Patient ID                                 '
        '             lngPlanID  -Plan ID                                   '
        '             lngProviderID - ProviderID                            '
        '             lngDate - Date of service                             '
        'Returns: The ID of the authorization that covers the DOS           '
        '         -1 otherwise                                              '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim objCert As New CertificationDB.CCertificationDB

        objCert.ConnectionString = _ConnectionString


        GetCertID = objCert.GetCertID(lngPatID, lngPlanID, lngProviderID, dteDate, strCPTCode)

        objCert = Nothing



    End Function


    Private Function VerifyData(ByVal dteStart As Date, ByVal dteEnd As Date, _
                ByVal lngPatientID As Long, ByVal lngPlanID As Long, _
                ByVal strCertNum As String, ByVal strAddedBy As String, _
                ByVal lngProviderID As Long, ByVal varCertUnits As Object, _
                ByRef strErrMessage As String, ByVal lngDaysPrior As Long, _
                ByVal lngUnitsPrior As Long, ByVal lngCertID As Long) As Boolean 'R003
        '--------------------------------------------------------------------
        'Date: 03/08/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Verifies all required data has been provided by the user.                                           '
        'Parameters:  The values to be checked.                             '
        'Returns: Boolean value identifying if all criteria has been satisfied.                                         '
        '--------------------------------------------------------------------
        Dim strObject As String
        Dim intDim As Integer
        Dim intCTR As Integer
        Dim arrCPTs() As String 'R002



        If dteStart > dteEnd Then
            strErrMessage = "The end date must be on or after the start date."
            VerifyData = False
            Exit Function
        End If

        If lngProviderID < 0 Then          'R007 if -1 passed instead of providers dont try to validate
            strObject = "At least 1 provider must be associated with the cert."
        Else
            ' Will cause Error 9 if array is empty
        End If

        strObject = "Insufficient CPT code and approved units information supplied."
        If UBound(varCertUnits) Then
            ' Will cause Error 9 if array is empty
        End If
        strObject = ""

        'Ensure all CPT Codes in the array exist in the database. 'R002
        'arrCPTs = MakeCPTArray(varCertUnits)
        'strErrMessage = CheckCPTExistance(arrCPTs)
        'If Trim(strErrMessage) > "" Then
        '    strErrMessage = "CPT Code(s) " & Trim(strErrMessage) & " is invalid."
        '    VerifyData = False
        '    Exit Function
        'End If

        If lngDaysPrior < 0 Then
            strErrMessage = "Days Prior value must be zero or greater."
            VerifyData = False
            Exit Function
        End If

        If lngUnitsPrior < 0 Then
            strErrMessage = "Units Prior value must be zero or greater."
            VerifyData = False
            Exit Function
        End If

        'Ensure a CPT Code is not duplicated. - R003
        '    strErrMessage = CheckForDuplicateCPTs(varCertUnits)
        '    If Trim(strErrMessage) > "" Then
        '        strErrMessage = "CPT Code(s) " & Trim(strErrMessage) & " is listed more than once."
        '        VerifyData = False
        '        Exit Function
        '    End If

        Erase arrCPTs

        'If we get here, all is well...
        VerifyData = True

        Exit Function


        If Err.Number = 9 Then
            strErrMessage = strObject
        Else
            'err.raise(Err.Number, Err.Source, Err.Description)
        End If
        VerifyData = False

    End Function

    'Public Sub Update(ByVal lngCertID As Long, ByVal lngPatID As Long, _
    '                ByVal lngPlanID As Long, ByVal dteStart As Date, _
    '                ByVal dteEnd As Date, ByVal strCertNum As String, _
    '                ByVal strUserName As String, ByVal strNote As String, _
    '                ByRef varProviders As Object, ByRef varCPTUnits As Object, _
    '                ByVal lngDaysPrior As Long, ByVal lngUnitsPrior As Long)
    ''--------------------------------------------------------------------
    ''Date: 03/08/2000                                                   '
    ''Author: Rick "Boom Boom" Segura                                    '
    ''Description:  Authorization driver for updating information for a  '
    ''              single cert into all appropriate tables.             '
    ''Parameters: lngPatID -  ID of authorized patient                   '
    ''            lngPlanID - ID of authorizing plan                     '
    ''            dteStart - cert Start date                             '
    ''            dteEnd - cert End Date                                 '
    ''            strCertNum - plan assigned certnumber                  '
    ''            strAddedBy - name of user entering cert                '
    ''            strNote -  User notes on cert                          '
    ''            varProviders - array of authorized providers           '
    ''            varCPTUnits - array of delimited CPT and unit info     '
    ''            lngDaysPrior - Days Prior(Hot List)                    '
    ''            lngUnitsPrior - Units Prior(Hot List)                  '
    ''Returns: NULL                                                      '
    ''--------------------------------------------------------------------
    ''Revision History:                                                  '
    ''                                                                   '
    ''--------------------------------------------------------------------
    '
    '    Dim objCertification As CertificationDB.CCertificationDB
    '    Dim objCertProvider As CertificationDB.CCertProviderDB
    '    Dim objCertUnit As CertificationDB.CCertUnitDB
    '    Dim objCertCPTCode As CertificationDB.CCertCPTCodeDB
    '    Dim lngCertCPTProviderID As Long
    '    Dim lngCertCPTCodeID As Long
    '    Dim lngCertUnitID As Long
    '    Dim strCertUnitInfo As String
    '    Dim intCtr As Integer
    '    Dim strErrMsg As String
    '    Dim arrChanges() As String 'R003
    '    Dim strTemp As String
    '    Dim strTemp2 As String
    '    Dim lngProviderID As Long
    '    Dim lngCertProviderID As Long
    '    Dim strAction As String
    '    Dim strCPT As String
    '    Dim lngUnits As Long
    '
    '    
    '
    '    Set objCertification = CreateObjectXXXXXXX("CertificationDB.CCertificationDB")
    '
    '    If Not VerifyData(dteStart, dteEnd, lngPatID, lngPlanID, strCertNum, _
    '                      strUserName, varProviders, varCPTUnits, strErrMsg, _
    '                      lngDaysPrior, lngUnitsPrior, lngCertID) Then
    '       Exit Function
    '    End If
    '
    '    'Identify the changes being made to a Certification.
    '    arrChanges = IdentifyChanges(lngCertID, dteStart, dteEnd, lngPatID, lngPlanID, _
    '                    strCertNum, varProviders, varCPTUnits)
    '    'DR: To Do - Procedure to ensure changes can be applied
    '    '  VerifyChanges(arrChanges)
    '
    '    'Stop
    '    '  objCertification.DeleteDetail(lngCertID) 'Commented out R003
    '
    '    ' First insert core Authorization info and get Cert ID from tblCertification
    '      objCertification.Update(lngCertID, lngPatID, lngPlanID, _
    '                                 dteStart, dteEnd, strCertNum, strNote, _
    '                                 strUserName, lngDaysPrior, lngUnitsPrior)
    '
    '    'Update Cert-Providers if a change has been recognized.
    '    For intCtr = 0 To UBound(arrChanges, 2)
    '        If arrChanges(0, intCtr) = "fldProviderID" Then
    '            strTemp = arrChanges(1, intCtr)
    '            strTemp2 = ParseTrim(strTemp, COLON_ASC)
    '            strTemp2 = ParseTrim(strTemp, COLON_ASC)
    '            lngCertProviderID = CLng(strTemp)
    '
    '            Set objCertProvider = CreateObjectXXXXXXX("CertificationDB.CCertProviderDB")
    '              objCertProvider.Update(lngCertProviderID, varProviders(0))
    '            Set objCertProvider = Nothing
    '
    '            Exit For 'We've found and performed the update - get out of the loop.
    '        End If
    '    Next intCtr
    '
    '    'Insert, Update or Delete Cert-Units based on changes identified in the arrChanges array.
    '    For intCtr = 0 To UBound(arrChanges, 2)
    '        If arrChanges(0, intCtr) = "fldCertUnit" Then
    '            strAction = arrChanges(1, intCtr)
    '
    '            Set objCertUnit = CreateObjectXXXXXXX("CertificationDB.CCertUnitDB")
    '            Set objCertCPTCode = CreateObjectXXXXXXX("CertificationDB.CCertCPTCodeDB")
    '            Select Case strAction
    '                Case "I" 'Insert
    '                    'Insert the unit value followed by the CPT Code(s)
    '                    strTemp = arrChanges(2, intCtr)
    '                    strCPT = ParseTrim(strTemp, COLON_ASC)
    '                    lngUnits = strTemp
    '                    lngCertUnitID = objCertUnit.Insert(lngUnits, lngUnits)
    '
    '                    Do While InStr(1, strCPT, "|", vbTextCompare) > 0
    '                        strTemp = ParseTrim(strCPT, DELIM_ASC)
    '                          objCertCPTCode.Insert(lngCertID, strTemp, lngCertUnitID, strUserName)
    '                    Loop
    '                    'Insert the last (or only CPT code)
    '                      objCertCPTCode.Insert(lngCertID, strCPT, lngCertUnitID, strUserName)
    '
    '                Case "U" 'Update
    '                    strTemp = arrChanges(2, intCtr)
    '                    lngUnits = ParseTrim(strTemp, COLON_ASC)
    '                    lngCertUnitID = strTemp
    '                      objCertUnit.Update(lngCertUnitID, lngUnits, strUserName)
    '                Case "D" 'Delete
    '                    lngCertUnitID = arrChanges(2, intCtr)
    '                      objCertUnit.Delete(lngCertUnitID, strUserName)
    '            End Select
    '            Set objCertUnit = Nothing
    '            Set objCertCPTCode = Nothing
    '        End If
    '    Next
    '
    '    Set objCertification = Nothing
    '    Erase arrChanges
    '
    '    GetObjectContext.SetComplete
    '
    '    Exit Sub
    '
    '    On Error GoTo 0
    '    'Signal incompletion and raise the error to the  ing environment.
    '    GetObjectContext.SetAbort
    '    Set objCertification = Nothing
    '    Set objCertProvider = Nothing
    '    Set objCertUnit = Nothing
    '    Set objCertCPTCode = Nothing
    '    If Err.Number = 0 Then
    '       'err.raise vbObjectError, CLASS_NAME, strErrMsg
    '    Else
    '       'err.raise Err.Number, Err.Source, Err.Description
    '    End If
    'End Sub

    Public Sub Update(ByVal lngCertID As Long, ByVal lngPatID As Long, _
                    ByVal lngPlanID As Long, ByVal lngProviderID As Long, ByVal dteStart As Date, _
                    ByVal dteEnd As Date, ByVal strCertNum As String, _
                    ByVal strUserName As String, ByVal strNote As String, _
                    ByRef varProviders As Object, ByRef varCPTUnits As Object, _
                    ByVal lngDaysPrior As Long, ByVal lngUnitsPrior As Long)
        '--------------------------------------------------------------------
        'Date: 05/07/2001                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Authorization driver for updating information for a  '
        '              single cert into all appropriate tables.             '
        'Parameters: lngPatID -  ID of authorized patient                   '
        '            lngPlanID - ID of authorizing plan                     '
        '            dteStart - cert Start date                             '
        '            dteEnd - cert End Date                                 '
        '            strCertNum - plan assigned certnumber                  '
        '            strAddedBy - name of user entering cert                '
        '            strNote -  User notes on cert                          '
        '            varProviders - array of authorized providers           '
        '            varCPTUnits - array of delimited CPT and unit info     '
        '            lngDaysPrior - Days Prior(Hot List)                    '
        '            lngUnitsPrior - Units Prior(Hot List)                  '
        'Returns: NULL                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim blnUsed As Boolean
        Dim objCert As New CertificationDB.CCertificationDB
        Dim objCertProvider As New CertificationDB.CCertProviderDB
        Dim objAudit As New CertificationDB.CCertAuditDB
        Dim colRst As Collection                ' Collection of Cert detail recordsets
        Dim rstCert As New ADODB.Recordset          ' base Cert info recrodset
        Dim rstCertCPT As New ADODB.Recordset       ' Cert/CPT recordset
        Dim rstCertProvider As New ADODB.Recordset  ' Cert/Provider recordset
        Dim rstCertUsage As New ADODB.Recordset     ' Cert Usage recordset
        Dim strErrMsg As String                 ' Custom error string
        Dim strAudit As String                  ' Audit String
        Dim lngAuditID As Long


        objCert.ConnectionString = _ConnectionString
        objCertProvider.ConnectionString = _ConnectionString
        objAudit.ConnectionString = _ConnectionString



        colRst = FetchCertDetail(lngCertID)
        rstCert = colRst(1)
        rstCertProvider = colRst(2)
        rstCertCPT = colRst(3)
        rstCertUsage = colRst(4)

        ' If an associated record exists in tblCertUsage, then  '
        ' this cert has been applied to at least 1 DOS          '
        blnUsed = (rstCertUsage.RecordCount > 0)

        ' Vlaidate Core Cert info                               '
        If Not ValidateCertUpdate(lngPatID, lngPlanID, dteStart, _
                                  dteEnd, strCertNum, lngDaysPrior, _
                                  lngUnitsPrior, rstCert, rstCertUsage, _
                                  blnUsed, strAudit, strErrMsg) Then
            ' Data violation found, changes not allowed         '
            Exit Sub
        End If

        ' Vlaidate Provider Cert info                           '
        If Not ValidateProviderUpdate(lngProviderID, rstCertProvider, _
                                  blnUsed, strAudit, strErrMsg) Then
            ' Data violation found, changes not allowed         '
            Exit Sub
        End If

        If Not UpdateCPTUnits(varCPTUnits, rstCertCPT, rstCertUsage, lngCertID, strUserName, strAudit, strErrMsg) Then

        End If



        objCert.Update(lngCertID, lngPatID, lngPlanID, lngProviderID, dteStart, dteEnd, strCertNum, strNote, strUserName, lngDaysPrior, lngUnitsPrior)
        objCert = Nothing
        ' Save Cert provider info
        '    If lngProviderID > 0 Then
        '        Set objCertProvider = CreateObjectXXXXXXX("Certificationdb.CCertProviderDB")
        '        objCertProvider.Update rstCertProvider.Fields("fldCertProviderID").Value, varProviders(0)
        '        Set objCertProvider = Nothing
        '    End If

        ' Log any audit changes                                 '
        If strAudit > "" Then
            lngAuditID = LogAudit(lngCertID, strUserName, strAudit)
            If lngAuditID <= 0 Then
                strErrMsg = "An error occurred while logging the Authorization changes."
                Exit Sub
            End If
        End If

        colRst = Nothing
        rstCert = Nothing
        rstCertProvider = Nothing
        rstCertCPT = Nothing
        rstCertUsage = Nothing


    End Sub

    Public Function FetchCertAuditByCert(ByVal lngCertID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 05/03/2001                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Retrieves recordset of audits for the given cert     '
        'Parameters: lngCertID - ID of Cert to fetch certs for              '
        'Returns: Recordset of audit trail                                  '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim objCertAudit As New CertificationDB.CCertAuditDB

        objCertAudit.ConnectionString = _ConnectionString

        FetchCertAuditByCert = objCertAudit.FetchCertAuditByCert(lngCertID)



        objCertAudit = Nothing



    End Function


    Public Function FetchPatientCerts(ByVal lngPatID As Long, _
        Optional ByVal blnShowDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/09/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Retrieves recordset containing detailed information  '
        '              of the cert having the ID given as the argument      '
        'Parameters: lngCertID -  ID of cert being described                '
        'Returns: Recordstets of detailed information                       '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim objCertification As New CertificationDB.CCertificationDB

        objCertification.ConnectionString = _ConnectionString

        FetchPatientCerts = objCertification.FetchPatientCerts(lngPatID, blnShowDisabled)



        objCertification = Nothing


    End Function


    Public Function FetchPatientCertsDetail(ByVal lngPatID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 08/21/2000                                                   '
        'Author: Dave Richkun
        'Description:  Retrieves recordset of certs for a particular patient
        '              Includes associated providers, and CPT Codes/units.
        'Parameters: lngPatID - ID of Patient to fetch certs for            '
        'Returns: Recordset of detailed cert information                       '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim objCertification As New CertificationDB.CCertificationDB



        FetchPatientCertsDetail = objCertification.FetchPatientCertsDetail(lngPatID)


        objCertification = Nothing



    End Function

    Public Function GetCoverage(ByVal lngPatID As Long, _
                                ByVal lngProvID As Long, _
                                ByVal dteDate As Date, _
                                ByVal strCPTCode As String) As Object
        '--------------------------------------------------------------------
        'Date: 04/10/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Determines if a DOS is covered by all level of a     '
        '              patients insurance plans                             '
        'Returns: An array of cert information                              '
        'Array Format:Element i(CertID, PlanID, CertNum, PlanName)          '
        '   i - level of insurance - 1                                      '
        '   CertID - ID of matching cert, -1 if not found, 0 if not required'
        '   PlanID - ID of Authorizing Plan                                 '
        '   CertNum - Cert Number if match found, EmptyString Othewise      '
        '   PlanName - Plan name if match found, EmptyString Othewise       '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------
        Dim arrCertInfo As Object

        Dim objPlans As New BenefactorBz.CPatRPPlanBz
        Dim objPlanRule As New BenefactorBz.CPatRPPlanRuleBz
        Dim objCert As New CertificationDB.CCertificationDB
        Dim rstPlans As New ADODB.Recordset
        Dim lngCertID As Long
        Dim rstPlanRule As New ADODB.Recordset
        Dim strCertNum As String
        Dim strPlanName As String




        objPlans.ConnectionString = _ConnectionString
        objPlanRule.ConnectionString = _ConnectionString
        objCert.ConnectionString = _ConnectionString




        ' Retrieve all levels of insurance for the patient
        rstPlans = objPlans.FetchRPPlansByPat(lngPatID)

        arrCertInfo = DBNull.Value

        With rstPlans
            If Not (.EOF Or .BOF) Then
                While Not .EOF
                    ' Fish for a cert for the encounter
                    lngCertID = objCert.GetCertID(lngPatID, rstPlans.Fields("fldPlanID").Value, lngProvID, dteDate, strCPTCode) 'objCert.GetCertID(lngPatID, !fldPlanID, lngProvID, dteDate, strCPTCode)

                    'If no cert is found, see if one is required
                    If lngCertID < 0 Then
                        ' if a cert is not required, set the cert ID to 0
                        rstPlanRule = objPlanRule.FetchPatRPPlanRule(lngPatID, rstPlans.Fields("fldRPID").Value, rstPlans.Fields("fldPlanID").Value) 'objPlanRule.FetchPatRPPlanRule(lngPatID, !fldRPID, !fldPlanID)
                        If rstPlanRule.Fields("fldCertReqd").Value = "N" Then lngCertID = 0
                        strCertNum = ""
                    Else
                        strCertNum = objCert.GetCertNumber(lngCertID)

                    End If
                    strPlanName = objCert.GetCertPlanName(rstPlans.Fields("fldPlanID").Value) 'objCert.GetCertPlanName(!fldPlanID)

                    ' Add current level of insurance info to array
                    AddToArray(arrCertInfo, lngCertID, rstPlans.Fields("fldPlanID").Value, strCertNum, strPlanName) 'AddToArray(arrCertInfo, lngCertID, !fldPlanID, strCertNum, strPlanName)
                    .MoveNext()
                End While
            End If
        End With

        GetCoverage = arrCertInfo


        If Not rstPlans Is Nothing Then rstPlans = Nothing
        If Not rstPlanRule Is Nothing Then rstPlanRule = Nothing

        objPlans = Nothing
        objPlanRule = Nothing
        objCert = Nothing


    End Function
    Public Function FetchUncoveredDates(ByVal lngPatientID As Long, ByVal strCPTCode As String, _
            ByVal lngProviderID As Long, ByVal dblNumUnits As Double, ByVal aryRecurDates As Object) As Object
        '--------------------------------------------------------------------
        'Date: 10/17/2001
        'Author: Dave Richkun
        'Description:  Compares a series of recurring appointment dates to active patient
        '      certifications.  Any dates not covered by Authorization are returned in
        '      a 2-dimensional array.  This function was created specifi y to assist
        '      in Authorization checking when making recurring patient appointments.  The
        '      function checks Authorization dates only associated with the patient's
        '      primary insurance plan.
        'Parameters: lngPatientID - ID of patient whose Authorizations are checked
        '            strCPTCode - CPT code under which the appointments were made
        '            lngProviderID - ID of the Provider with whom the appointments were made
        '            aryRecurDates - An array of appointment dates.  This array is not assumed
        '               to be ordered chronologi y
        '            dblNumUnits - number of units to be consumed.
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '  R006: Created
        '  R008: added dblNumUnits
        '--------------------------------------------------------------------

        Dim objCert As New CertificationDB.CCertificationDB
        Dim rst As New ADODB.Recordset
        Dim intCtr1 As Integer
        Dim intCtr2 As Integer
        Dim intUnitsAvailable As Double
        Dim intUnitsConsumed As Double
        Dim intNotCerted As Integer
        Dim arrUnCerted As Object
        Dim dtTemp As Date


        objCert.ConnectionString = _ConnectionString




        rst = objCert.FetchPatientCertsDetailByPatProvCPT(lngPatientID, lngProviderID, strCPTCode)
        objCert = Nothing

        'Sort dates in chronological order using simple bubble-sort
        If IsArray(aryRecurDates) Then
            For intCtr1 = (UBound(aryRecurDates) - 1) To 0 Step -1
                For intCtr2 = 0 To intCtr1
                    If CDate(aryRecurDates(intCtr2)) > CDate(aryRecurDates(intCtr2 + 1)) Then
                        dtTemp = aryRecurDates(intCtr2)
                        aryRecurDates(intCtr2) = aryRecurDates(intCtr2 + 1)
                        aryRecurDates(intCtr2 + 1) = dtTemp
                    End If
                Next
            Next
        End If

        'Loop through the recordset to obtain the total number of units available for the CPT code passed.
        'We need to track the number of units that would be consumed by the appointment dates and alert the
        'user if the number of available units will expire before the end of the appointment series.
        For intCtr1 = 1 To rst.RecordCount
            intUnitsAvailable = intUnitsAvailable + rst.Fields("fldRemainingUnits").Value
        Next intCtr1

        If rst.RecordCount > 0 Then
            rst.MoveFirst()
        End If

        ReDim arrUnCerted(1, 0)
        intUnitsConsumed = 0
        intNotCerted = -1
        For intCtr1 = 0 To UBound(aryRecurDates)
            If intUnitsAvailable < dblNumUnits Then
                If intCtr1 = 0 Then
                    'No units were ever available. All dates are uncovered.
                    intNotCerted = intNotCerted + 1
                    ReDim Preserve arrUnCerted(1, intNotCerted)
                    'ReDim Preserve arrUnCerted(intNotCerted)
                    'arrUnCerted(intNotCerted) = "All Appointment Dates - Authorization does not exist or No Authorization units available"
                    arrUnCerted(0, intNotCerted) = "All Appointment Dates"
                    arrUnCerted(1, intNotCerted) = "Certification does not exist or not enough Authorization units available"
                Else
                    'All dates from this point forward are not covered
                    intNotCerted = intNotCerted + 1
                    ReDim Preserve arrUnCerted(1, intNotCerted)
                    'ReDim Preserve arrUnCerted(intNotCerted)
                    arrUnCerted(0, intNotCerted) = CStr(aryRecurDates(intCtr1))
                    arrUnCerted(1, intNotCerted) = "All available Authorization units would be consumed by earlier appointments"
                    'arrUnCerted(intNotCerted) = CStr(aryRecurDates(intCtr1)) & " - All available Authorization units would be consumed by earlier appointments"
                End If
            Else
                rst.MoveFirst()
                For intCtr2 = 1 To rst.RecordCount
                    If (DateValue(aryRecurDates(intCtr1)) >= DateValue(rst.Fields("fldStartDate").Value) And _
                       DateValue(aryRecurDates(intCtr1)) <= DateValue(rst.Fields("fldEndDate").Value)) Then
                        'Date is certified; increment the 'Consumed' counter and decrement the 'Available' counter
                        intUnitsConsumed = intUnitsConsumed + 1
                        intUnitsAvailable = intUnitsAvailable - 1
                        Exit For
                    Else
                        rst.MoveNext()
                        If rst.EOF Then
                            'Date not certified
                            intNotCerted = intNotCerted + 1
                            ReDim Preserve arrUnCerted(1, intNotCerted)
                            'ReDim Preserve arrUnCerted(intNotCerted)
                            arrUnCerted(0, intNotCerted) = CStr(aryRecurDates(intCtr1))
                            arrUnCerted(1, intNotCerted) = "Appointment date not within certified date range"
                            'arrUnCerted(intNotCerted) = CStr(aryRecurDates(intCtr1)) & " - Appointment date not within certified date range"
                        End If
                    End If
                Next intCtr2
            End If
        Next intCtr1

        rst = Nothing

        FetchUncoveredDates = arrUnCerted

    End Function

    Private Function AddToArray(ByRef varArray As Object, _
                                ByVal lngCertID As Long, _
                                ByVal lngPlanID As Long, _
                                ByVal strCertNum As String, _
                                ByVal strPlanName As String)
        '--------------------------------------------------------------------
        'Date: 04/14/2000                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Adds an element to a dynamic array                   '
        'Parameters: varArray - Object(Array)                              '
        '            lngCertID - ID of Authorization to add to array        '
        '            lngPlanID - ID of Plan                                 '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '   R001 06/27/2000 Segura Added two more elements to 2nd dimension '
        '--------------------------------------------------------------------
        Dim varNewArray As Object
        Dim i As Integer
        Dim j As Integer


        If Not IsArray(varArray) Then
            ' Make the first Dimension of the Array
            ReDim varNewArray(0, 3)
            varNewArray(0, 0) = lngCertID
            varNewArray(0, 1) = lngPlanID
            varNewArray(0, 2) = strCertNum
            varNewArray(0, 3) = strPlanName
        Else
            ' Make a new array that will fit on more element row and
            ' copy the values form the old array, then add new values
            ReDim varNewArray((UBound(varArray) + 1), 3)
            For i = 0 To UBound(varArray, 1)
                For j = 0 To UBound(varArray, 2)
                    varNewArray(i, j) = varArray(i, j)
                Next
            Next
            varNewArray(UBound(varNewArray, 1), 0) = lngCertID
            varNewArray(UBound(varNewArray, 1), 1) = lngPlanID
            varNewArray(UBound(varNewArray, 1), 2) = strCertNum
            varNewArray(UBound(varNewArray, 1), 3) = strPlanName
        End If

        varArray = varNewArray
    End Function

    Private Function MakeCPTArray(ByVal varCPTUnit As Object) As Object
        '--------------------------------------------------------------------
        'Date: 08/18/2000                                                   '
        'Author: Dave Richkun
        'Description:  Creates an array of CPT codes from the varCPTUnit array
        '              passed to Input and Update methods.
        'Parameters: varCPTUnit - Array of CPT Codes and Units
        'Returns: Array containing only CPT codes
        '--------------------------------------------------------------------

        Dim arrCPTs() As String
        Dim strArray As String
        Dim intCTR As Integer
        Dim intStart As Integer
        Dim intPipe As Integer
        Dim strCPTs As String
        Dim strCPT As String
        Dim intElement As Integer


        ReDim arrCPTs(0)
        If IsArray(varCPTUnit) Then
            For intCTR = 0 To UBound(varCPTUnit)
                strArray = varCPTUnit(intCTR)
                'CPT codes start after the 2nd pipe character
                intStart = InStr(1, strArray, "|", vbTextCompare)
                intStart = InStr(intStart + 1, strArray, "|", vbTextCompare) + 1

                strCPTs = Mid(strArray, intStart)
                'Iteratively, look for pipe characters - there may be more than 1 CPT code in the string
                intStart = 1
                intPipe = InStr(1, strCPTs, "|", vbTextCompare)

                If intPipe = 0 Then
                    If strCPTs > "" Then
                        'Write the element to the array
                        ReDim Preserve arrCPTs(intElement)
                        arrCPTs(intElement) = strCPTs
                        intElement = intElement + 1
                    End If
                Else
                    Do Until intPipe = 0
                        ReDim Preserve arrCPTs(intElement)
                        arrCPTs(intElement) = Mid(strCPTs, intStart, intPipe - intStart)
                        intElement = intElement + 1
                        intStart = intPipe + 1
                        intPipe = InStr(intStart, strCPTs, "|", vbTextCompare)
                    Loop

                    ReDim Preserve arrCPTs(intElement)
                    arrCPTs(intElement) = Mid(strCPTs, intStart)
                    intElement = intElement + 1
                End If
            Next intCTR

            MakeCPTArray = arrCPTs
        Else
            MakeCPTArray = Nothing
        End If



    End Function

    Private Function CheckCPTExistance(ByVal arrCPTs As Object) As String
        '--------------------------------------------------------------------
        'Date: 08/18/2000                                                   '
        'Author: Dave Richkun
        'Description:  Parses an array of CPT codes, returns a string message
        '              identifying which CPT Codes do not exist in the database.
        'Parameters: arrCPTs - Object array of CPT Codes
        'Returns: String message identifying which CPT Codes do not exist.
        '--------------------------------------------------------------------
        Dim r As String = String.Empty

        Dim objCPT As New ListBZ.CCPTCodeBz
        Dim strCPT As String
        Dim strMessage As String
        Dim intCTR As Integer

        objCPT.ConnectionString = _ConnectionString

        If Not IsArray(arrCPTs) Then
            CheckCPTExistance = "Invalid Parameter.  The parameter is not an array.)"
            Exit Function
        End If



        For intCTR = 0 To UBound(arrCPTs)
            strCPT = arrCPTs(intCTR)
            If objCPT.Exists(strCPT) = False Then
                strMessage = strMessage & arrCPTs(intCTR) & " "
            End If
        Next intCTR

        CheckCPTExistance = strMessage

        objCPT = Nothing

        Return r
    End Function


    Private Function IdentifyChanges(ByVal lngCertID As Long, ByVal dteStart As Date, ByVal dteEnd As Date, _
                        ByVal lngPatientID As Long, ByVal lngPlanID As Long, _
                        ByVal strCertNumber As String, ByVal varProviders As Object, _
                        ByVal varCertUnits As Object) As Object
        '--------------------------------------------------------------------
        'Date: 10/18/2000                                                   '
        'Author: Dave Richkun
        'Description:  Identifies the changes that have been made to a certification
        '              and stores the changes in a 2-dimensional array.  Since we are
        '              using ReDim Preserve to redimension the size of the array based
        '              on the changes that are discovered, VB requires us to place the
        '              resizeable element as the last dimension of the array.  Thus the
        '              array may appear to be 'backwards'.
        '              Format of the array:
        '                  Element    Description
        '                  -------    -----------
        '                     0       Name of database field whose value has changed
        '                     1       Current value of the field
        '                     2       New value of the field
        'Parameters: Values to which Authorizations are being updated to.  See VerifyData()
        '            for detail information.
        'Returns: Array containing identified changes to the Authorization
        '--------------------------------------------------------------------

        Dim colCerts As Collection
        Dim rst As New ADODB.Recordset
        Dim arrChanges(0, 0) As String
        Dim intCTR As Integer
        Dim intCtr2 As Integer
        Dim strCPT As String
        Dim intUnitsAssigned As Double
        Dim intUnitsRemaining As Double
        Dim blnCPTFound As Boolean
        Dim lngCertUnitID As Long
        Dim strCPTPool As String
        Dim intPoolUnits As Double
        Dim arrOldCPTUnits As Object
        Dim arrNewCPTUnits As Object

        colCerts = FetchCertDetail(lngCertID)
        rst = colCerts(1)
        ReDim Preserve arrChanges(2, 0) 'Initialize the array

        If rst.Fields("fldStartDate").Value <> dteStart Then
            ReDim Preserve arrChanges(2, intCTR)
            arrChanges(0, intCTR) = "fldStartDate"
            arrChanges(1, intCTR) = rst.Fields("fldStartDate").Value
            arrChanges(2, intCTR) = dteStart
            intCTR = intCTR + 1
        End If

        If rst.Fields("fldEndDate").Value <> dteEnd Then
            ReDim Preserve arrChanges(2, intCTR)
            arrChanges(0, intCTR) = "fldEndDate"
            arrChanges(1, intCTR) = rst.Fields("fldEndDate").Value
            arrChanges(2, intCTR) = dteEnd
            intCTR = intCTR + 1
        End If

        If rst.Fields("fldCertNumber").Value <> strCertNumber Then
            ReDim Preserve arrChanges(2, intCTR)
            arrChanges(0, intCTR) = "fldCertNumber"
            arrChanges(1, intCTR) = rst.Fields("fldCertNumber").Value
            arrChanges(2, intCTR) = strCertNumber
            intCTR = intCTR + 1
        End If

        If rst.Fields("fldPatientID").Value <> lngPatientID Then
            ReDim Preserve arrChanges(2, intCTR)
            arrChanges(0, intCTR) = "fldPatientID"
            arrChanges(1, intCTR) = rst.Fields("fldPatientID").Value
            arrChanges(2, intCTR) = lngPatientID
            intCTR = intCTR + 1
        End If

        If rst.Fields("fldPlanID").Value <> lngPlanID Then
            ReDim Preserve arrChanges(2, intCTR)
            arrChanges(0, intCTR) = "fldPlanID"
            arrChanges(1, intCTR) = rst.Fields("fldPlanID").Value
            arrChanges(2, intCTR) = lngPlanID
            intCTR = intCTR + 1
        End If

        'Advance to the second recordset - Providers
        'Currently, the User Interface currently supports a 1:1 relationship between
        'Certifications and Providers.  Therefore, we do not need to loop through the
        'array or recordset, but instead simply observe the first row in each.
        rst = colCerts(2)
        If rst.Fields("fldProviderID").Value <> varProviders(0) Then
            ReDim Preserve arrChanges(2, intCTR)
            arrChanges(0, intCTR) = "fldProviderID"
            arrChanges(1, intCTR) = rst.Fields("fldProviderID").Value & ":U:" & rst.Fields("fldCertProviderID").Value
            arrChanges(2, intCTR) = varProviders(0)
            intCTR = intCTR + 1
        End If

        'Advance to the third recordset - CPT Codes/Units
        rst = colCerts(3)

        'Create a sorted array of old CPT Code/Units.  Pooled CPT Codes assigned a single
        'unit value are also sorted so that we can accurately compare old CPT code pools
        'with new CPT Code pools.
        arrOldCPTUnits = CreateOldCPTUnitArray(rst)
        _MD.QuickSort(arrOldCPTUnits)

        'Create a sorted array of new CPT Code/Units.  Pooled CPT Codes assigned a single
        'unit value are also sorted so that we can accurately compare old CPT code pools
        'with new CPT Code pools.
        arrNewCPTUnits = CreateNewCPTUnitArray(varCertUnits)
        _MD.QuickSort(arrNewCPTUnits)

        'Append CPT Code/Units additions and updates to the arrChanges() array.
        AppendAdditionsAndChanges(arrChanges, arrOldCPTUnits, arrNewCPTUnits)

        'Append CPT Code/Units deletions to the arrChanges() array.
        AppendDeletions(arrChanges, arrOldCPTUnits, arrNewCPTUnits)

        Erase arrOldCPTUnits
        Erase arrNewCPTUnits
        rst = Nothing
        colCerts = Nothing

        IdentifyChanges = arrChanges


    End Function


    Private Sub ParseCPTUnitArray(ByVal strCPTUnits As String, ByRef strCPT As String, _
                                  ByRef intUnitsAssigned As Double, ByRef intUnitsRemaining As Double)
        '--------------------------------------------------------------------
        'Date: 10/18/2000
        'Author: Dave Richkun
        'Description: Parses the CPT Code/Units recordset into individual items.  The
        '             individual items are delimnited with a PIPE character.
        'Parameters: strCPTUnits - The pipe-delimited CPT Code/Units
        '            strCPT - ByRef parameter that will contain the CPT Code(s)
        '            intUnitsAssigned - ByRef parameter that will contain the Units Assigned
        '            intUnitsRemaining - ByRef parameter that will contain the Units Remaining
        'Returns: The ByRef parameters.
        '--------------------------------------------------------------------

        intUnitsAssigned = _MD.ParseTrim(strCPTUnits, DELIM_ASC)
        intUnitsRemaining = _MD.ParseTrim(strCPTUnits, DELIM_ASC)
        strCPT = strCPTUnits

    End Sub


    Private Function CheckForDuplicateCPTs(ByVal varCertUnits As Object) As String
        '--------------------------------------------------------------------
        'Date: 10/23/2000
        'Author: Dave Richkun
        'Description: Parses the CPT Code/Units array into an array of individual CPT
        '             Codes, taking into consideration CPT-Unit pools, and performs
        '             a quicksort.  Once the array is sorted, this function returns
        '             a message if it locates repeating CPT Codes.
        'Parameters: varCertUnits - An array containing CPT Code/Units information.
        'Returns: A string mesage identifying which CPT Code(s) have been duplicated.
        '--------------------------------------------------------------------

        Dim intCTR As Integer
        Dim intCtr2 As Integer
        Dim varCPTs() As Object
        Dim varSort As Object
        Dim strCPT As String
        Dim intUnitsAssigned As Double
        Dim intUnitsRemaining As Double
        Dim strMessage As String



        For intCTR = 0 To UBound(varCertUnits)
            If varCertUnits(intCTR) > "" Then
                ParseCPTUnitArray(varCertUnits(intCTR), strCPT, intUnitsAssigned, intUnitsRemaining)
                Do While InStr(1, strCPT, "|", vbTextCompare) > 0
                    ReDim Preserve varCPTs(intCtr2)
                    varCPTs(intCtr2) = _MD.ParseTrim(strCPT, DELIM_ASC)
                    intCtr2 = intCtr2 + 1
                Loop
                ReDim Preserve varCPTs(intCtr2)
                varCPTs(intCtr2) = _MD.ParseTrim(strCPT, DELIM_ASC)
                intCtr2 = intCtr2 + 1
            End If
        Next intCTR

        'Sort the array
        _MD.QuickSort(varCPTs)

        '    'Check for repeating CPTs.
        '    For intCtr = 0 To UBound(varCPTs)
        '        strCPT = varCPTs(intCtr)
        '        If strCPT = varCPTs(intCtr + 1) Then
        '            If strMessage > "" Then
        '                If InStr(1, strMessage, strCPT, vbTextCompare) = 0 Then
        '                    strMessage = IIf(Len(strMessage) > 0, strMessage & ", " & varCPTs(intCtr), varCPTs(intCtr))
        '                End If
        '            Else
        '                strMessage = varCPTs(intCtr)
        '            End If
        '        End If
        '    Next intCtr

        Erase varCPTs
        CheckForDuplicateCPTs = strMessage



    End Function

    Private Function CreateOldCPTUnitArray(ByVal rst As ADODB.Recordset) As Object
        '--------------------------------------------------------------------
        'Date: 10/24/2000
        'Author: Dave Richkun
        'Description: Create a sorted array of old CPT Code/Units.  Pooled CPT Codes
        '             assigned a single unit value are also sorted so that we can
        '             accurately compare old CPT code pools with new CPT Code pools.
        'Parameters: rst - Recordset containing CPT Codes/Unit values
        'Returns: A sorted Object array of CPT Codes and Units.  CPT Code pools are
        '             delimited with a pipe character.  The units assigned to the
        '             CPT Codes are delimited with a colon e.g. 90801|90806:4
        '--------------------------------------------------------------------
        Dim intRstCtr As Integer
        Dim intTempCtr As Integer
        Dim lngCertUnitID As Long
        Dim arrTemp() As String
        Dim strTemp As String
        Dim arrOldCPTs() As String
        Dim intArrCtr As Integer
        Dim lngUnitsAssigned As Double

        Do While Not rst.EOF
            lngCertUnitID = rst.Fields("fldCertUnitID").Value
            lngUnitsAssigned = rst.Fields("fldAssignedUnits").Value
            lngCertUnitID = rst.Fields("fldCertUnitID").Value

            intTempCtr = 0
            Do While lngCertUnitID = rst.Fields("fldCertUnitID").Value
                ReDim Preserve arrTemp(intTempCtr)
                arrTemp(intTempCtr) = IIf(Len(CStr(arrTemp(intTempCtr))) > 0, "|" & rst.Fields("fldCPTCode").Value, rst.Fields("fldCPTCode").Value)
                If Not rst.EOF Then
                    rst.MoveNext()
                    intTempCtr = intTempCtr + 1
                End If

                If rst.EOF Then Exit Do
            Loop

            _MD.QuickSort(arrTemp)

            strTemp = ConvertCPTArrayToString(arrTemp) & ":" & lngUnitsAssigned & ":" & lngCertUnitID
            Erase arrTemp

            ReDim Preserve arrOldCPTs(intArrCtr)
            arrOldCPTs(intArrCtr) = strTemp
            intArrCtr = intArrCtr + 1
            intRstCtr = intRstCtr + 1
        Loop

        CreateOldCPTUnitArray = (arrOldCPTs)

    End Function


    Private Function CreateNewCPTUnitArray(ByVal varCPTUnits As Object) As Object
        '--------------------------------------------------------------------
        'Date: 10/24/2000
        'Author: Dave Richkun
        'Description: Creates a sorted array of new CPT Code/Units.  Pooled CPT Codes
        '             assigned a single unit value are also sorted so that we can
        '             accurately compare old CPT code pools with new CPT Code pools.
        'Parameters: varCPTUnits - Object array containing new CPT Codes/Unit values
        'Returns: A sorted Object array of CPT Codes and Units.  CPT Code pools are
        '             delimited with a pipe character.  The units assigned to the
        '             CPT Codes are delimited with a colon e.g. 90801|90806:4
        '--------------------------------------------------------------------
        Dim intVarCtr As Integer
        Dim intTempCtr As Integer
        Dim lngCertUnitID As Long
        Dim arrTemp() As String
        Dim strTemp As String
        Dim arrNewCPTs() As String
        Dim intArrCtr As Integer
        Dim intUnitsAssigned As Double
        Dim intUnitsRemaining As Double
        Dim strCPT As String

        For intVarCtr = 0 To (UBound(varCPTUnits) - 1)
            strTemp = varCPTUnits(intVarCtr)
            If Trim(strTemp) > "" Then
                intUnitsAssigned = _MD.ParseTrim(strTemp, DELIM_ASC)
                intUnitsRemaining = _MD.ParseTrim(strTemp, DELIM_ASC)
                'strTemp = ParseTrim(strTemp, DELIM_ASC)

                intTempCtr = 0
                If InStr(1, strTemp, "|", vbTextCompare) > 0 Then
                    Do Until InStr(1, strTemp, "|", vbTextCompare) = 0
                        strCPT = _MD.ParseTrim(strTemp, DELIM_ASC)
                        ReDim Preserve arrTemp(intTempCtr)
                        arrTemp(intTempCtr) = strCPT
                        intTempCtr = intTempCtr + 1
                    Loop
                    'We need to include the last element in the array.
                    ReDim Preserve arrTemp(intTempCtr)
                    arrTemp(intTempCtr) = strTemp
                    intTempCtr = intTempCtr + 1
                Else
                    ReDim Preserve arrTemp(intTempCtr)
                    arrTemp(intTempCtr) = strTemp
                    intTempCtr = intTempCtr + 1
                End If

                _MD.QuickSort(arrTemp)

                strTemp = ConvertCPTArrayToString(arrTemp) & ":" & intUnitsAssigned
                Erase arrTemp

                ReDim Preserve arrNewCPTs(intArrCtr)
                arrNewCPTs(intArrCtr) = strTemp
                intArrCtr = intArrCtr + 1
            End If
        Next intVarCtr

        CreateNewCPTUnitArray = arrNewCPTs
        'CreateNewCPTUnitArray = VBCVar(arrNewCPTs)

    End Function

    Private Function ConvertCPTArrayToString(ByVal arrCPTUnits As Object) As String
        '--------------------------------------------------------------------
        'Date: 10/24/2000
        'Author: Dave Richkun
        'Description: Converts a sorted CPT array to a string.  If the CPT/Unit array
        '             represents a pool of CPT Codes, the CPT Codes are delimited with a
        '             PIPE character.
        'Parameters: arrCPTUnits - Array of CPT Codes to be sorted.
        'Returns: A string of sorted CPT Codes.
        '--------------------------------------------------------------------

        Dim strCPT As String
        Dim intCTR As Integer

        For intCTR = 0 To UBound(arrCPTUnits)
            strCPT = IIf(Len(strCPT) > 0, strCPT & "|" & arrCPTUnits(intCTR), arrCPTUnits(intCTR))
        Next intCTR

        ConvertCPTArrayToString = strCPT

    End Function

    Private Sub AppendAdditionsAndChanges(ByRef arrChanges As Object, ByVal arrOldCPTUnits As Object, _
                                ByVal arrNewCPTUnits As Object)
        '--------------------------------------------------------------------
        'Date: 10/25/2000
        'Author: Dave Richkun
        'Description: Identifies CPT Code/Unit additions to a certification
        '             and appends the changes to the arrChanges() array.
        'Parameters: arrChanges - The array that collects changes to a Certification.
        '            arrOldCPTUnits - An array containing information about the
        '               existing, or old, CPT Code/Units associated with a cartification.
        '            arrNewCPTUnits - An array containing information about the
        '               changes, or new, CPT Code/Units to be associated with a
        '               cartification.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim intNewCtr As Integer
        Dim intOldCtr As Integer
        Dim strNewTemp As String
        Dim strNewCPT As String
        Dim intNewUnits As Double
        Dim strOldTemp As String
        Dim strOldCPT As String
        Dim intOldUnits As Double
        Dim lngOldCertUnitID As Long
        Dim blnFound As Boolean
        Dim intChanges As Integer

        For intNewCtr = 0 To UBound(arrNewCPTUnits)
            blnFound = False
            strNewTemp = arrNewCPTUnits(intNewCtr)
            strNewCPT = _MD.ParseTrim(strNewTemp, COLON_ASC)
            intNewUnits = strNewTemp

            For intOldCtr = 0 To UBound(arrOldCPTUnits)
                strOldTemp = arrOldCPTUnits(intOldCtr)
                strOldCPT = _MD.ParseTrim(strOldTemp, COLON_ASC)
                intOldUnits = _MD.ParseTrim(strOldTemp, COLON_ASC)
                lngOldCertUnitID = strOldTemp

                If strOldCPT = strNewCPT Then
                    blnFound = True
                    'Since we have found a match, check for changes.
                    If intNewUnits <> intOldUnits Then
                        intChanges = UBound(arrChanges, 2) + 1
                        ReDim Preserve arrChanges(2, intChanges)
                        arrChanges(0, intChanges) = "fldCertUnit"
                        arrChanges(1, intChanges) = "U"
                        arrChanges(2, intChanges) = intNewUnits & ":" & lngOldCertUnitID
                    End If
                    Exit For 'If we have found it, then it is not new.
                End If
            Next intOldCtr

            If blnFound = False Then
                'We have found an addition - append it to the arrChanges() array.  Consider
                'that the arrChanges array may still be empty.
                If CStr(arrChanges(0, 0)) = "" Then
                    intChanges = 0
                Else
                    intChanges = UBound(arrChanges, 2) + 1
                End If
                ReDim Preserve arrChanges(2, intChanges)
                arrChanges(0, intChanges) = "fldCertUnit"
                arrChanges(1, intChanges) = "I"
                arrChanges(2, intChanges) = strNewCPT & ":" & intNewUnits

            End If

        Next intNewCtr

    End Sub


    Private Sub AppendDeletions(ByRef arrChanges As Object, ByVal arrOldCPTUnits As Object, _
                                ByVal arrNewCPTUnits As Object)
        '--------------------------------------------------------------------
        'Date: 10/25/2000
        'Author: Dave Richkun
        'Description: Identifies CPT Code/Unit deletions from a certification
        '             and appends the changes to the arrChanges() array.
        'Parameters: arrChanges - The array that collects changes to a Certification.
        '            arrOldCPTUnits - An array containing information about the
        '               existing, or old, CPT Code/Units associated with a cartification.
        '            arrNewCPTUnits - An array containing information about the
        '               changes, or new, CPT Code/Units to be associated with a
        '               cartification.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim intNewCtr As Integer
        Dim intOldCtr As Integer
        Dim strNewTemp As String
        Dim strNewCPT As String
        Dim intNewUnits As Double
        Dim strOldTemp As String
        Dim strOldCPT As String
        Dim intOldUnits As Double
        Dim lngOldCertUnitID As Long
        Dim blnFound As Boolean
        Dim intChanges As Integer

        For intOldCtr = 0 To UBound(arrOldCPTUnits)
            blnFound = False
            strOldTemp = arrOldCPTUnits(intOldCtr)
            strOldCPT = _MD.ParseTrim(strOldTemp, COLON_ASC)
            intOldUnits = _MD.ParseTrim(strOldTemp, COLON_ASC)
            lngOldCertUnitID = strOldTemp

            For intNewCtr = 0 To UBound(arrNewCPTUnits)
                strNewTemp = arrNewCPTUnits(intNewCtr)
                strNewCPT = _MD.ParseTrim(strNewTemp, COLON_ASC)
                intNewUnits = strNewTemp

                If strOldCPT = strNewCPT Then
                    blnFound = True
                    Exit For 'Since we have found a match, it has not been deleted.
                End If
            Next intNewCtr

            If blnFound = False Then
                'We have found a deletion - append it to the arrChanges() array
                intChanges = UBound(arrChanges, 2) + 1
                ReDim Preserve arrChanges(2, intChanges)
                arrChanges(0, intChanges) = "fldCertUnit"
                arrChanges(1, intChanges) = "D"
                arrChanges(2, intChanges) = lngOldCertUnitID
            End If

        Next intOldCtr

    End Sub

    '-- Begin R004 -----
    Public Function FetchPatientUserCertsDetail(ByVal lngPatientID As Long, _
                    ByVal lngUserID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/14/2001
        'Author: Dave Richkun
        'Description:  Returns Authorizations associated with a specific patient
        '              and within the context of the logged in user.  This method
        '              takes into consideraton whether the patient in question is
        '              shared between Providers.  If a patient is shared then
        '              Authorization records for all shared Providers
        '              are returned.  If the patient is not shared, then only records
        '              associated with the current user are returned.
        'Parameters: lngPatientID - ID of Patient to retrieve certs for            '
        '            lngUserID - ID of the user making the   to this method.
        'Returns: Recordset of detailed cert information                       '
        '--------------------------------------------------------------------

        Dim objCertification As New CertificationDB.CCertificationDB



        FetchPatientUserCertsDetail = objCertification.FetchPatientUserCertsDetail(lngPatientID, lngUserID)



        objCertification = Nothing



    End Function

    '-- End R004 -----

    '-- Begin R005 -----
    Public Function FetchPatientUserArchivedCertsDetail(ByVal lngPatientID As Long, _
                    ByVal lngUserID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/21/2001                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Returns archivedcertifications associated with a specific patient
        '              and within the context of the logged in user.  This method
        '              takes into consideraton whether the patient in question is
        '              shared between Providers.  If a patient is shared then
        '              Authorization records for all shared Providers
        '              are returned.  If the patient is not shared, then only records
        '              associated with the current user are returned.
        'Parameters: lngPatientID - ID of Patient to retrieve certs for            '
        '            lngUserID - ID of the user making the   to this method.
        'Returns: Recordset of detailed cert information                       '
        '--------------------------------------------------------------------

        Dim objCertification As New CertificationDB.CCertificationDB



        FetchPatientUserArchivedCertsDetail = objCertification.FetchPatientUserArchivedCertsDetail(lngPatientID, lngUserID)



        objCertification = Nothing



    End Function

    '-- End R005 -----

    Private Function ValidateCertUpdate(ByVal lngPatientID As Long, _
                    ByVal lngPlanID As Long, ByVal dteStart As Date, _
                    ByVal dteEnd As Date, ByVal strCertNum As String, _
                    ByVal lngDaysPrior As Long, ByVal lngUnitsPrior As Double, _
                    ByVal rstCert As ADODB.Recordset, ByVal rstUsage As ADODB.Recordset, _
                    ByVal blnUsed As Boolean, ByRef strAudit As String, _
                    ByRef strErrMsg As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 05/07/2001                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Determines if certain changes are allowed to be made '
        '               a record in tblCertification                        '
        'Parameters:    lngPatientID - ID of patient associated to cert     '
        '               lngPlanID -  ID of plan associated to cert          '
        '               dteStart - Cert Start date                          '
        '               dteEnd - Cert end date                              '
        '               rstCert - Recordset of current cert info            '
        '               rstUsage - Cert usage recordset                     '
        '               blnUsed - Boolean to signify if cert has been used  '
        '               strAudit - Audit string                             '
        '               strErrMsg - Error Message string                    '
        'Returns: True if changes can be made, False otherwise              '
        '--------------------------------------------------------------------
        Dim objBen As New BenefactorBz.CBenefactorBz
        Dim objPlan As New InsuranceBz.CPlanBz
        Dim rstPlan As New ADODB.Recordset
        Dim rstPatient As New ADODB.Recordset
        Dim blnAllow As Boolean


        objBen.ConnectionString = _ConnectionString
        objPlan.ConnectionString = _ConnectionString




        ValidateCertUpdate = True   ' Assume success
        blnAllow = True

        With rstCert
            ' Determine if a patient change is requested                '
            If lngPatientID <> .Fields("fldPatientID").Value Then
                If blnUsed Then
                    blnAllow = False
                    strErrMsg = "The associated patient may not be changed after the Authorization has been used."
                    GoTo NotAllow
                End If

                rstPatient = objBen.FetchByID(lngPatientID)
                _MD.StrCatDel(strAudit, "Patient:" & .Fields("fldFullName").Value & ":" & _
                                    rstPatient.Fields("fldFullName").Value, ";")
                objBen = Nothing
                rstPatient = Nothing
            End If

            ' Determine if a plan change is requested                   '
            If lngPlanID <> .Fields("fldPlanID").Value Then
                If blnUsed Then
                    blnAllow = False
                    strErrMsg = "The associated plan may not be changed after the Authorization has been used."
                    GoTo NotAllow
                End If

                rstPlan = objPlan.FetchByID(lngPlanID)
                _MD.StrCatDel(strAudit, "Insurance Plan:" & .Fields("fldPlanName").Value & ":" & _
                                    rstPlan.Fields("fldPlanName").Value, ";")
                objPlan = Nothing
                rstPlan = Nothing
            End If

            ' Determine if a start date change is requested             '
            If dteStart <> .Fields("fldStartDate").Value Then
                If rstUsage.RecordCount Then rstUsage.MoveFirst()
                While Not rstUsage.EOF
                    If DateValue(rstUsage.Fields("fldDOS").Value) < dteStart Then
                        blnAllow = False
                        strErrMsg = "Changes could not be saved because a date of service is associated with the certifcation that is prior to the given start date."
                        GoTo NotAllow
                    End If
                    rstUsage.MoveNext()
                End While
                _MD.StrCatDel(strAudit, "Start Date:" & .Fields("fldStartDate").Value & ":" & _
                                    dteStart, ";")
            End If

            ' Determine if an end date change is requested              '
            If dteEnd <> .Fields("fldEndDate").Value Then
                If rstUsage.RecordCount Then rstUsage.MoveFirst()
                While Not rstUsage.EOF
                    If DateValue(rstUsage.Fields("fldDOS").Value) > dteEnd Then
                        blnAllow = False
                        strErrMsg = "Changes could not be saved because a date of service is associated with the certifcation that is after to the given end date."
                        GoTo NotAllow
                    End If
                    rstUsage.MoveNext()
                End While
                _MD.StrCatDel(strAudit, "End Date:" & .Fields("fldEndDate").Value & ":" & _
                                    dteEnd, ";")
            End If

            ' Determine if a Cert Number change is requested            '
            If strCertNum <> .Fields("fldCertNumber").Value Then
                _MD.StrCatDel(strAudit, "Certification Number:" & .Fields("fldCertNumber").Value & ":" & _
                                    strCertNum, ";")
            End If

            ' Determine if a Days Prior change is requested             '
            If lngDaysPrior <> .Fields("fldDaysPrior").Value Then
                _MD.StrCatDel(strAudit, "Days Prior:" & .Fields("fldDaysPrior").Value & ":" & _
                                    lngDaysPrior, ";")
            End If

            ' Determine if a Units Prior change is requested            '
            If lngUnitsPrior <> .Fields("fldUnitsPrior").Value Then
                _MD.StrCatDel(strAudit, "Units Prior:" & .Fields("fldUnitsPrior").Value & ":" & _
                                    lngUnitsPrior, ";")
            End If

            ' Determine if date range is valid                          '
            If dteStart > dteEnd Then
                blnAllow = False
                strErrMsg = ""
                GoTo NotAllow
            End If

NotAllow:
            If Not blnAllow Then
                If Not strErrMsg > "" Then
                    strErrMsg = "Changes can not be made to a certifcation that has been previously assigned to a date of service."
                End If
                ValidateCertUpdate = False
            End If

        End With

        Exit Function
Err_Trap:
        objBen = Nothing
        rstPatient = Nothing
        objPlan = Nothing
        rstPlan = Nothing
        ValidateCertUpdate = False
        If Not strErrMsg > "" Then
            strErrMsg = "An error occured while validating certification(base) changes."
        End If
    End Function

    Private Function ValidateProviderUpdate(ByVal lngProviderID As Long, _
                                            ByVal rstCertProvider As ADODB.Recordset, _
                                            ByVal blnUsed As Boolean, ByRef strAudit As String, _
                                            ByRef strErrMsg As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 05/09/2001                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Determines if certain changes are allowed to be made '
        '               a record in tblCertProvider                         '
        'Parameters:    lngProviderID - ID of provider associated to cert   '
        '               rstCertProvider - Recordset of current cert info    '
        '               blnUsed - Boolean to signify if cert has been used  '
        '               strAudit - Audit string                             '
        '               strErrMsg - Error Message string                    '
        'Returns: True if changes can be made, False otherwise              '
        '--------------------------------------------------------------------
        Dim objUser As New ClinicBZ.CUserBz
        Dim rstUser As New ADODB.Recordset
        Dim blnIsProvider As Boolean




        objUser.ConnectionString = _ConnectionString








        ValidateProviderUpdate = True       ' Assume success

        With rstCertProvider
            If lngProviderID <> .Fields("fldProviderID").Value Then
                If blnUsed Then
                    strErrMsg = "The associated provider may not be changed after the Authorization has been used."
                    ValidateProviderUpdate = False
                    Exit Function
                End If
                If lngProviderID > 0 Then
                    rstUser = objUser.FetchDetail(lngProviderID, blnIsProvider)
                    _MD.StrCatDel(strAudit, "Provider:" & .Fields("fldFullName").Value & ":" & _
                                    rstUser.Fields("fldLastName").Value & ", " & _
                                    rstUser.Fields("fldFirstName").Value, ";")
                    objUser = Nothing
                    rstUser = Nothing
                End If
            End If
        End With

        Exit Function

Err_Trap:
        objUser = Nothing
        rstUser = Nothing

        If Not strErrMsg > "" Then
            strErrMsg = "An error occured while validating certification(provider) changes."
        End If
    End Function


    Private Function LogAudit(ByVal lngCertID As Long, _
                           ByVal strUserName As String, _
                           ByVal strNote As String) As Long
        '--------------------------------------------------------------------
        'Date: 05/03/2001                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Inserts a single record into the tblCertAudit table  '
        'Parameters: lngCertID - ID of cert to link CPT Code with           '
        '            strUserName - Name of user executing insert         '
        '            strNote -  Audit text                                  '
        'Returns: ID of record just inserted(on success)                    '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim objCertAudit As New CertificationDB.CCertAuditDB
        Dim strErrMsg As String
        Dim lngID As Long

        objCertAudit.ConnectionString = _ConnectionString



        lngID = objCertAudit.Insert(lngCertID, strUserName, strNote)

        If lngID <= 0 Then
            strErrMsg = "Failed to insert a row into tblCertAudit."
            Exit Function
        End If

        objCertAudit = Nothing
        LogAudit = lngID

    End Function

    Private Function UpdateCPTUnits(ByVal varCPTUnits As Object, _
                                    ByVal rstCPTUnits As ADODB.Recordset, _
                                    ByVal rstUsage As ADODB.Recordset, _
                                    ByVal lngCertID As Long, _
                                    ByVal strUserName As String, _
                                    ByRef strAudit As String, ByRef strErr As String) _
                                    As Boolean
        '--------------------------------------------------------------------
        'Date: 05/09/2001                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Updates, inserts, and deletes records in tbales      '
        '               tblCertCPTCode and tblCertUnit as required by       '
        '               change requests                                     '
        'Parameters: varCPTUnits - array containing CPT Unit and code info  '
        '            rstCPTUnits - recordset containing current CPT Unit and'
        '               Code info                                           '
        '            rstUsage - Recordset of cert usage                     '
        '            strUserName - UserName of person updating the cert info'
        '            strUserName - Name of user executing insert         '
        '            strNote -  Audit text                                  '
        'Returns: True if cahnges were successfully made, False otherwise   '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------
        Dim aryCurrent As Object
        Dim aryNew As Object
        Dim blnChange As Boolean



        UpdateCPTUnits = True           ' Assume success                '
        blnChange = False               ' Assume no changes are made    '
        aryNew = BuildNewCPTArray(varCPTUnits)
        aryCurrent = BuildCurrentCPTArray(rstCPTUnits)

        If CompareCPTArrays(aryCurrent, aryNew, rstUsage, blnChange, strErr) Then
            If CommitCPTChanges(aryCurrent, aryNew, lngCertID, strUserName, strErr) Then
                If blnChange Then BuildCPTAuditString(aryCurrent, aryNew, strAudit)
            End If
        Else
            Exit Function
        End If

        Erase aryNew
        Erase aryCurrent


    End Function

    Private Function BuildCurrentCPTArray(ByVal rstCPTUnits As ADODB.Recordset) As Object
        '--------------------------------------------------------------------
        'Date: 05/10/2001                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Builds array of Current CPT Unit info                '
        'Parameters: rstCPTUnits - recordset containing CPT Unit and code info
        'Returns: Array of CPT Unit Info                                    '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------
        Dim aryCurrent(0, 0) As Object
        Dim intCnt As Integer
        Dim lngID As Long
        ' Dim objTemp As New Dictionary

        BuildCurrentCPTArray = DBNull.Value

        With rstCPTUnits
            lngID = 0
            intCnt = -1
            ' Get a Ubound for new array                        '
            While Not .EOF
                If lngID <> .Fields("fldCertunitID").Value Then
                    lngID = .Fields("fldCertunitID").Value
                    intCnt = intCnt + 1
                End If
                .MoveNext()
            End While

            ' Create new array                                  '
            ReDim aryCurrent(intCnt, 3)

            .MoveFirst()

            lngID = 0
            intCnt = -1
            While Not .EOF
                If lngID <> .Fields("fldCertUnitID").Value Then
                    lngID = .Fields("fldCertUnitID").Value
                    intCnt = intCnt + 1
                    aryCurrent(intCnt, 0) = lngID
                    aryCurrent(intCnt, 1) = .Fields("fldAssignedUnits").Value
                    aryCurrent(intCnt, 2) = .Fields("fldRemainingUnits").Value
                    '  objTemp = aryCurrent(intCnt, 3)
                    '   objTemp.Add.Fields(("fldCPTCode").Value, "N")
                Else
                    'Set objTemp = aryCurrent(intCnt, 3)
                    'If Not objTemp.Exists(.Fields("fldCPTCode").Value) Then
                    '    objTemp.Add.Fields("fldCPTCode").Value, "N"
                    'End If
                End If
                .MoveNext()
            End While
        End With

        BuildCurrentCPTArray = aryCurrent

    End Function

    Private Function BuildNewCPTArray(ByVal varCPTUnits As Object) As Object
        '--------------------------------------------------------------------
        'Date: 05/10/2001                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Builds array of NEW CPT Unit info                    '
        'Parameters: varCPTUnits - array containing CPT Unit and code info  '
        'Returns: Array of CPT Unit Info                                    '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------
        Dim intUnit As Integer
        Dim intCPT As Integer
        Dim aryNew(0, 0) As Object
        Dim varTempArray As Object
        '   Dim objTemp As Scripting.Dictionary
        Dim strTemp As String

        BuildNewCPTArray = DBNull.Value         ' Assume Failure                '

        If Not IsArray(varCPTUnits) Then Exit Function

        ReDim aryNew(UBound(varCPTUnits, 1), 3)

        For intUnit = 0 To UBound(varCPTUnits)
            aryNew(intUnit, 0) = varCPTUnits(intUnit, 0)    ' Straight copy '
            aryNew(intUnit, 1) = varCPTUnits(intUnit, 1)    ' Straight copy '
            aryNew(intUnit, 2) = varCPTUnits(intUnit, 2)    ' Straight copy '

            varTempArray = varCPTUnits(intUnit, 3)          ' Convert array into collection     '
            '   aryNew(intUnit, 3) = CreateObjectXXXXXXX("Scripting.Dictionary")
            '     objTemp = aryNew(intUnit, 3)
            For intCPT = 0 To UBound(varTempArray)
                strTemp = Trim(varTempArray(intCPT))
                'If Not objTemp.Exists(strTemp) Then         ' Ensure every CPT Code is listed only once per Cert Unit '
                '    objTemp.Add(strTemp, "N")
                'End If
            Next
        Next

        BuildNewCPTArray = aryNew
    End Function

    Private Sub BuildCPTAuditString(ByVal aryCurrent As Object, _
                                    ByVal aryNew As Object, _
                                    ByRef strAudit As String)
        '--------------------------------------------------------------------
        'Date: 05/10/2001                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Builds the audit string for CPT/Unit changes         '
        'Parameters: aryCurrent - array of current CPT/Unit info            '
        '            aryNew -   array of new CPT/Unit info                  '
        '            strAudit - Audit string                                '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------
        Dim strTemp As String = ""

        _MD.StrCat(strTemp, "CPT Units")
        _MD.StrCatDel(strTemp, BuildCPTString(aryCurrent), ":")
        _MD.StrCatDel(strTemp, BuildCPTString(aryNew), ":")
        _MD.StrCatDel(strAudit, strTemp, ";")

    End Sub

    Private Function BuildCPTString(ByVal aryCPTUnits As Object) As String
        '--------------------------------------------------------------------
        'Date: 05/11/2001                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Builds the audit substring for CPT/Unit changes      '
        'Parameters: aryCPTUnits - array of CPT/Unit info                   '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------
        Dim strTemp As String
        Dim varKeys As Object
        ' Dim objCPT As Scripting.Dictionary
        Dim intUnit As Double
        Dim intCPT As Integer

        'For intUnit = 0 To UBound(aryCPTUnits, 1)
        '    _MD.StrCatDel(strTemp, aryCPTUnits(intUnit, 1), "#")

        '    objCPT = aryCPTUnits(intUnit, 3)
        '    varKeys = objCPT.Keys
        '    For intCPT = 0 To UBound(varKeys)
        '        _MD.StrCatDel(strTemp, varKeys(intCPT), "&")
        '    Next
        'Next

        'objCPT = Nothing
        BuildCPTString = strTemp
    End Function

    Private Function CompareCPTArrays(ByRef aryCurrent As Object, _
                                      ByRef aryNew As Object, _
                                      ByVal rstUsage As ADODB.Recordset, _
                                      ByRef blnChange As Boolean, _
                                      ByRef strErr As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 05/11/2001                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Compares CPT Unit changes to current records and cert'
        '               usage to ensure changes are valid                   '
        'Parameters: aryCurrent - array of current CPT Unit info            '
        '            aryNew - array of new CPT Unit info                    '
        '            rstUsage - Recordset of a cert's usage                 '
        '            blnChange - Boolean signifying if a change is detected '
        '            strErr - Error Message string                          '
        'Returns:   True if changes are valid, false otherwise              '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------
        Dim intCurUnit As Double
        Dim intCurCode As Integer
        Dim intLoc As Integer
        Dim varKeys As Object
        'Dim objCurCodes As Scripting.Dictionary
        'Dim objNewCodes As Scripting.Dictionary

        CompareCPTArrays = True         ' Assume success                '

        ' Compare current CPT Units to new new ones.                    '
        ' This round determines if there is a change in assigned units  '
        ' It also determines if any CPT Codes, or entire CPT Units, have'
        ' been requested for deletions and the validty of those actions.'
        For intCurUnit = 0 To UBound(aryCurrent)
            intLoc = GetMatchingRow(aryCurrent(intCurUnit, 0), aryNew)
            If intLoc >= 0 Then
                ' Compare unit values and CPT codes                         '
                ' 1st compare assign values                             '
                If CLng(aryCurrent(intCurUnit, 1)) <> CLng(aryNew(intLoc, 1)) Then
                    blnChange = True
                    If (CLng(aryCurrent(intCurUnit, 1)) - CLng(aryCurrent(intCurUnit, 2))) > CLng(aryNew(intCurUnit, 1)) Then
                        ' New value is below the number of units aleady used'
                        CompareCPTArrays = False
                        strErr = "You cannot reduce the assigned units below the amount that has already been used."
                        Exit Function
                    End If
                End If

                '' 2nd look for CPT Codes to be deleted                  '
                'objCurCodes = aryCurrent(intCurUnit, 3)
                'objNewCodes = aryNew(intLoc, 3)
                'varKeys = objCurCodes.Keys
                'For intCurCode = 0 To UBound(varKeys)
                '    If Not objNewCodes.Exists(varKeys(intCurCode)) Then
                '        ' User wants to delete a code from the pool, validate
                '        If IsCPTUsed(varKeys(intCurCode), rstUsage) Then
                '            CompareCPTArrays = False
                '            strErr = "You cannot delete a CPT code from a unit after the CPT code has been used."
                '            Exit Function
                '        Else
                '            blnChange = True
                '            objCurCodes(varKeys(intCurCode)) = "D"
                '        End If
                '    End If
                'Next
                ' aryCurrent(intCurUnit, 3) = objCurCodes

            Else
                ' No matching unit was found....User wants it deleted       '
                ' Determine if it can be deleted                            '
                If CLng(aryCurrent(intCurUnit, 1)) <> CLng(aryCurrent(intCurUnit, 2)) Then
                    ' Unit has been used at least once                  '
                    CompareCPTArrays = False
                    strErr = "You cannot delete a CPT Pool if it has been previously used."
                    Exit Function

                Else
                    blnChange = True
                    aryCurrent(intCurUnit, 1) = -1
                End If

            End If
        Next

        ' Compare new CPT Units to the current DB info                  '
        ' This round determines if there are any new entries to insert  '
        ' into tblCPTCode and/or tblCertUnit                            '
        'For intCurUnit = 0 To UBound(aryNew)
        ' CPT Units with an ID of 0 are new Units that should be    '
        ' inserted.  Units with non-zero IDs need their CPT Codes   '
        ' compared to the Codes of thecorresponding DB Unit to      '
        ' determine if a new CPT Code exists                        '

        '    If aryNew(intCurUnit, 0) = 0 Then
        '        ' New Cert Unit found.  Signal change only.             '
        '        blnChange = True
        '    Else
        '        ' Existing record may be modified. Do comparison of new '
        '        ' codes to old codes.                                   '
        '        intLoc = GetMatchingRow(aryNew(intCurUnit, 0), aryCurrent)
        '        If intLoc < 0 Then
        '            CompareCPTArrays = False
        '            strErr = "An error occurred while validating Authorization changes.(compare)"
        '            Exit Function
        '        Else
        '            objCurCodes = aryCurrent(intLoc, 3)
        '            objNewCodes = aryNew(intCurUnit, 3)

        '            varKeys = objNewCodes.Keys
        '            For intCurCode = 0 To UBound(varKeys)
        '                If Not objCurCodes.Exists(varKeys(intCurCode)) Then
        '                    ' New CPT Code for existing unit, flag this '
        '                    ' Code for insert                           '
        '                    blnChange = True
        '                    objNewCodes(varKeys(intCurCode)) = "I"
        '                End If
        '            Next
        '            aryNew(intCurUnit, 3) = objNewCodes
        '        End If
        '    End If

        'Next

        'objNewCodes = Nothing
        'objCurCodes = Nothing

    End Function

    Private Function GetMatchingRow(ByVal varVal As Object, _
                                    ByVal varArray As Object) As Integer
        '--------------------------------------------------------------------
        'Date: 05/11/2001                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Finds a matching ID element in the given array       '
        'Parameters: varVal - Value being searched in given array           '
        '            varArray - Array where value is being sought           '
        'Returns:  Location of matching ID if found, -1 otherwise           '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------
        Dim intCnt As Integer

        GetMatchingRow = -1

        For intCnt = 0 To UBound(varArray, 1)
            If CLng(varArray(intCnt, 0)) = CLng(varVal) Then
                GetMatchingRow = intCnt
                Exit Function
            End If
        Next

    End Function

    Private Function IsCPTUsed(ByVal strCPTCode As String, _
                               ByVal rstUsage As ADODB.Recordset) As Boolean
        '--------------------------------------------------------------------
        'Date: 05/11/2001                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Searches a given recordset for a given CPT code.  This
        '               determines if the CPT Code has been counted towards '
        '               a DOS.                                              '
        'Parameters: strCPTCode - Value being searched in given array       '
        '            rstUsage - Recordset of used CPT codes                 '
        'Returns:  Location of matching ID if found, -1 otherwise           '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        IsCPTUsed = False       ' Assume failure                        '

        With rstUsage
            If .RecordCount Then
                .MoveFirst()
                While Not .EOF
                    If .Fields("fldCPTCode").Value = strCPTCode Then
                        IsCPTUsed = True
                        Exit Function
                    End If
                    .MoveNext()
                End While
            End If
        End With

    End Function

    Private Function CommitCPTChanges(ByVal aryCurrent As Object, _
                                      ByVal aryNew As Object, _
                                      ByVal lngCertID As Long, _
                                      ByVal strUserName As String, _
                                      ByRef strErr As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 05/11/2001                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:   s on methods to commit changes to the DB based on'
        '               array values                                        '
        'Parameters: aryCurrent - array of current DB info(deletions/updates)
        '            aryNew - array of new info(insertions)                 '
        '            lngCertID - ID of cert changes are applied to          '
        '            strUserName - User login name who requested changes    '
        '            strErr - Error Messsage string                         '
        'Returns:  True on success, False on failure                        '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------
        '     Dim objCodes As New Scripting.Dictionary
        Dim varKeys As Object
        Dim intUnit As Double
        Dim intCode As Integer
        Dim objCertUnit As New CertificationDB.CCertUnitDB
        Dim objCertCPT As New CertificationDB.CCertCPTCodeDB
        Dim lngID As Long

        objCertUnit.ConnectionString = _ConnectionString
        objCertCPT.ConnectionString = _ConnectionString



        CommitCPTChanges = True     ' Assume Success                    '



        '' Insert new units and Codes first                              '
        'For intUnit = 0 To UBound(aryNew, 1)
        '    If aryNew(intUnit, 0) = 0 Then
        '        ' New Unit found, insert it and associated codes as well    '
        '        aryNew(intUnit, 0) = objCertUnit.Insert(aryNew(intUnit, 1), aryNew(intUnit, 2))
        '        If aryNew(intUnit, 0) <= 0 Then
        '            strErr = "An error occurred while inserting a new CPT Pool.(commit)"
        '            Exit Function
        '        End If

        'objCodes = aryNew(intUnit, 3)
        'varKeys = objCodes.Keys
        'For intCode = 0 To UBound(varKeys)
        '    lngID = objCertCPT.Insert(lngCertID, varKeys(intCode), aryNew(intUnit, 0), strUserName)
        '    If lngID <= 0 Then
        '        strErr = "An error occurred while inserting a new CPT Code.(commit)"
        '        Exit Function
        '    End If
        'Next
        'objCodes = Nothing

        '    Else
        '        ' Existing unit, insert new codes if any, update units      '
        '        lngID = objCertUnit.Update(aryNew(intUnit, 0), aryNew(intUnit, 1), strUserName)
        '        If lngID < 0 Then
        '            strErr = "An error occurred while updating Authorization units.(commit)"
        '            Exit Function
        '        End If

        '        objCodes = aryNew(intUnit, 3)
        '        varKeys = objCodes.Keys
        '        For intCode = 0 To UBound(varKeys)
        '            If objCodes(varKeys(intCode)) = "I" Then
        '                lngID = objCertCPT.Insert(lngCertID, varKeys(intCode), aryNew(intUnit, 0), strUserName)
        '                If lngID <= 0 Then
        '                    strErr = "An error occurred while inserting a new CPT Code.(commit)"
        '                    Exit Function
        '                End If
        '            End If
        '        Next
        '        objCodes = Nothing
        '    End If
        'Next

        '' Update units and make deletions                               '
        'For intUnit = 0 To UBound(aryCurrent, 1)
        '    If aryCurrent(intUnit, 1) = -1 Then
        '        ' Delete entire Cert Unit                                   '
        '        objCertUnit.Delete(aryCurrent(intUnit, 0))

        '    Else
        '        ' Delete any Codes flagged with a "D"                       '
        '        objCodes = aryCurrent(intUnit, 3)
        '        varKeys = objCodes.Keys
        '        For intCode = 0 To UBound(varKeys)
        '            If objCodes(varKeys(intCode)) = "D" Then
        '                objCertCPT.DeleteByUnitCode(aryCurrent(intUnit, 0), varKeys(intCode))

        '            End If
        '        Next
        '        objCodes = Nothing
        '    End If
        'Next

        'objCertUnit = Nothing
        'objCertCPT = Nothing
        'objCodes = Nothing

    End Function

    Public Sub RemoveCertFromHotList(ByVal lngCertID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 07/16/2001                                                   '
        'Author: Rick "Boom Boom" Segura                                    '
        'Description:  Sets the fldUnitsPrior and fldDaysPrior filed values '
        '              equal to 0, effectively removing the cert from the   '
        '              Hot List.                                            '
        'Parameters: lngCertID - ID of the cert to remove                   '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------
        Dim objCert As New CertificationDB.CCertificationDB
        Dim objAudit As New CertificationDB.CCertAuditDB
        Dim rst As New ADODB.Recordset
        Dim colRsts As Collection
        Dim strAudit As String

        objCert.ConnectionString = _ConnectionString
        objAudit.ConnectionString = _ConnectionString




        ' Audit Changes
        colRsts = FetchCertDetail(lngCertID)
        rst = colRsts(1)
        With rst

            Dim d As String = String.Empty
            Dim u As String = String.Empty

            d = .Fields("fldDaysPrior").Value
            u = .Fields("fldUnitsPrior").Value

            strAudit = "Days Prior:" + d + ":0;Units Prior:" + u + ":0"
        End With
        objAudit.Insert(lngCertID, strUserName, strAudit)

        '   Cert Removal
        objCert.RemoveFromHotList(lngCertID)

        rst = Nothing
        colRsts = Nothing
        objCert = Nothing
        objAudit = Nothing

    End Sub
    Public Function AddProvider(ByVal lngCertID As Long, ByVal lngProviderID As Long) As Long
        '--------------------------------------------------------------------
        'Date: 10/30/01
        'Author: Eric Pena
        'Description:  Adds a row in tblCertProvider for the specified cert, provider
        'Returns: ID of inserted row in tblCertProvider
        '--------------------------------------------------------------------

        Dim objCertification As New CertificationDB.CCertProviderDB
        objCertification.ConnectionString = _ConnectionString



        AddProvider = objCertification.Insert(lngCertID, lngProviderID)

        objCertification = Nothing


    End Function
    Public Function RemoveProvider(ByVal lngCertID As Long, ByVal lngProviderID As Long) As Long
        '--------------------------------------------------------------------
        'Date: 10/30/01
        'Author: Eric Pena
        'Description:  Adds a row in tblCertProvider for the specified cert, provider
        'Returns: ID of inserted row in tblCertProvider
        '--------------------------------------------------------------------

        Dim objCertification As New CertificationDB.CCertProviderDB
        objCertification.ConnectionString = _ConnectionString

        objCertification.Delete(lngCertID, lngProviderID)


        objCertification = Nothing


    End Function


    Public Function FetchQualifyingCerts(ByVal lngPatientID As Long, ByVal lngProviderID As Long, _
                        ByVal lngPlanID As Long, ByVal dtDOS As Date, _
                        ByVal strCPTCode As String, ByVal dblUnits As Double) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/09/2002
        'Author: Dave Richkun
        'Description:  Retrieves active qualifying certifications.  This procedure is
        '              designed to retrieve Authorizations that will have certification
        '              units deducted from them resulting from service provided.
        '              the Authorizations returned match encounter-related information
        '              passed in the parameter set.
        'Parameters: lngPatientID - ID of patient associated with certifications
        '            lngProviderID - ID of provider owning certification
        '            dtDOS - Date on which provider serviced patient
        '            strCPTCode - CPT Code associated with service
        '            dblUnits - The number of units applied on the date of service
        'Returns: Recordset of qualifying certifications
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim objCert As New CertificationDB.CCertificationDB
        objCert.ConnectionString = _ConnectionString



        FetchQualifyingCerts = objCert.FetchQualifyingCerts(lngPatientID, lngProviderID, _
                                      lngPlanID, dtDOS, strCPTCode, dblUnits)
        objCert = Nothing



    End Function


    Public Sub DeductUnits(ByVal lngEncounterLogID As Long, ByVal lngPatientID As Long, _
                ByVal lngProviderID As Long, ByVal lngPlanID As Long, _
                ByVal dtDOS As Date, ByVal strCPTCode As String, _
                ByVal dblUnits As Double)
        '--------------------------------------------------------------------
        'Date: 07/09/2002
        'Author: Dave Richkun
        'Description: Deducts Authorization units from all active patient certifications
        '             based on matching criteria, and records Authorization usage history.
        'Parameters: lngEncounterLogID - ID of EncounterLog consuming Authorization units
        '            lngPatientID - ID of patient owning Authorizations and for whom service was performed
        '            lngProviderID - ID of servicing provider
        '            dtDOS - Date the service was performed
        '            strCPTCode - CPT Code associated with the service
        '            dblUnits - Number of units applied at the time of service
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objCert As New CertificationDB.CCertificationDB
        Dim objCertUnit As New CertificationDB.CCertUnitDB
        Dim rst As New ADODB.Recordset
        Dim intCTR As Integer

        objCert.ConnectionString = _ConnectionString

        objCert.ConnectionString = _ConnectionString

        'Record Authorization unit usage history in tblCertUsage
        rst = FetchQualifyingCerts(lngPatientID, lngProviderID, lngPlanID, dtDOS, strCPTCode, dblUnits)



        If rst.RecordCount > 0 Then
            objCertUnit.LogUnitUsage(rst.Fields("fldCertID").Value, strCPTCode, dblUnits, _
                             dtDOS, lngProviderID, lngEncounterLogID)

            'Deduct units from tblCertUnit
            objCert.DeductUnits(lngEncounterLogID, lngPatientID, lngProviderID, _
                             lngPlanID, dtDOS, strCPTCode, dblUnits)

            rst.MoveNext()
        End If

        objCert = Nothing
        objCertUnit = Nothing
        rst = Nothing

        If Err.Number <> 0 Then
            Exit Sub
        End If

    End Sub


    Public Sub DeductSNAUnits(ByVal lngELID As Long, ByVal lngPatientID As Long, ByVal lngProviderID As Long, _
                ByVal dtDOS As Date, ByVal strCPTCode As String, ByVal dblUnits As Double, _
                ByVal lngPlanID As Long, ByVal lngCertID As Long, ByVal strCertNum As String)
        '--------------------------------------------------------------------
        'Date: 08/10/2002
        'Author: Dave Richkun
        'Description: Deducts Authorization units from all a specified patient certification
        '             based on matching criteria, and records Authorization usage history.  This
        '             method differs from the DeductUnits() method in that this method is used
        '             during Authorization 'Seek And Assign' i.e. after the Authorization is
        '             entered and un-certifie dates of service are found and applied.
        'Parameters: lngELID - ID of EncounterLog consuming Authorization units
        '            lngPatientID - ID of patient owning Authorizations and for whom service was performed
        '            lngProviderID - ID of servicing provider
        '            dtDOS - Date the service was performed
        '            strCPTCode - CPT Code associated with the service
        '            dblUnits - Number of units applied at the time of service
        '            lngPlanID - ID of Plan associated with the Authorization
        '            lngCertID - ID of the Authorization added
        '            strCertNum - the Authorization number
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objCert As New CertificationDB.CCertificationDB
        Dim objCertUnit As New CertificationDB.CCertUnitDB

        objCert.ConnectionString = _ConnectionString
        objCertUnit.ConnectionString = _ConnectionString




        'Update the Billing Responsibility row for the encounter
        AssignCert(lngELID, lngPlanID, strCertNum)

        objCertUnit.LogUnitUsage(lngCertID, strCPTCode, dblUnits, dtDOS, _
                                      lngProviderID, lngELID)
        objCertUnit = Nothing

        objCert.DeductUnits(lngELID, lngPatientID, lngProviderID, lngPlanID, _
                                 dtDOS, strCPTCode, dblUnits)
        objCert = Nothing

        If Err.Number <> 0 Then
            Exit Sub
        End If



    End Sub

    Public Function FetchUnCertedPlans(ByVal lngPatientID As Long, ByVal lngProviderID As Long, _
                        ByVal lngPlanID As Long, ByVal dtDOS As Date, _
                        ByVal strCPTCode As String, ByVal dblUnits As Double) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 07/16/2002
        'Author: Dave Richkun
        'Description:  Retrieves a list of patient plans that require Authorization but
        '            do not have a matching Authorization matching Date of Service
        '            parameters i.e. show all patient plans that do not match qualifying
        '            certifications.
        'Parameters: lngPatientID - ID of patient associated with certifications/DOS
        '            lngProviderID - ID of provider owning certification
        '            dtDOS - Date on which provider serviced patient
        '            strCPTCode - CPT Code associated with service
        '            dblUnits - The number of units applied on the date of service
        'Returns: Recordset of qualifying certifications
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim objCert As New CertificationDB.CCertificationDB
        objCert.ConnectionString = _ConnectionString

        FetchUnCertedPlans = objCert.FetchUnCertedPlans(lngPatientID, lngProviderID, lngPlanID, dtDOS, strCPTCode, dblUnits)
        objCert = Nothing


    End Function
    Public Sub AssignCert(ByVal lngELID As Long, ByVal lngPlanID As Long, ByVal strCertNum As String)
        '--------------------------------------------------------------------
        'Date: 08/10/2002
        'Author: Dave Richkun
        'Description:  Updates the Authorization Number in tblBillingResponsibility
        '              for a given encounter/plan ID combination.  This method is part
        '              of the Authorization 'Seek And Assign' functionality.
        'Parameters: lngELID - ID of the encounter log for which the cert number is updated
        '            lngPlanID - ID of the plan associated with the Authorization
        '            strCertNum - the Authorization number
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objBR As New CertificationDB.CCertUnitDB
        objBR.ConnectionString = _ConnectionString




        objBR.AssignCert(lngELID, lngPlanID, strCertNum)
        objBR = Nothing


    End Sub

End Class
