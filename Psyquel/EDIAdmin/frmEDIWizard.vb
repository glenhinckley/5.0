

Option Explicit On


Imports System.Configuration.ConfigurationSettings

Imports System
Imports System.IO
Imports System.Text
Imports System.Collections.Generic

Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility

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

Imports ClaimBz

Imports InsuranceBz

Imports Microsoft.VisualBasic.FileSystem
Imports System.Net.Mail

Public Class frmEDIWizard
    Private _DB As New dbStuff.db
    Private _MD As New dbStuff.ModCommon
    Private _isDebug As Boolean = False



    Public g_FileName As String
    Public g_strResponseFileName As String
    Public g_FileNumber As Long
    Public g_lngStartTxNum As Long
    Public g_lngEndTxNum As Long
    Private g_lngNumSeg As Long 'Data segment counter
    Private blnEmailSuccess As Boolean
    Private blnEmailFailure As Boolean


    Dim g_lngNumClaims As Long = 0

    Private Declare Function OpenProcess Lib "kernel32" (ByVal dwDesiredAccess As Long, ByVal bInheritHandle As Long, ByVal dwProcessId As Long) As Long
    Private Declare Function WaitForSingleObject Lib "kernel32" (ByVal hHandle As Long, ByVal dwMilliseconds As Long) As Long
    Private Declare Function GetExitCodeProcess Lib "kernel32" (ByVal hProcess As Long, ByVal lpExitCode As Long) As Long
    Private Declare Function CloseHandle Lib "kernel32" (ByVal hObject As Long) As Long

    Private Const TX_ECLAIM_SUBMITTED As Long = 6
    Private Const INFINITE = &HFFFFFFFF
    Private Const WAIT_FAILED = -1&
    Private Const STILL_ACTIVE = &H103&
    Private Const PROCESS_ALL_ACCESS = &H1F0FFF
    Private Const SYNCHRONIZE = &H100000
    Private Const INSCO_TEXAS_WORKERS_COMP As Long = 606
    Private Const INSCO_MEDICARE As Long = 24
    Private Const INSCO_MEDICARE_PARTB As Long = 29
    Private Const INSCO_MEDICAID As Long = 105
    Private Const INSCO_BCBS As Long = 158
    Private Const INSCO_BCBS_TX As Long = 6
    Private Const INSCO_CHAMPUS As Long = 1050

    Private CONST_OUTPUT_DIR As String


    Private _ConnectionString As String = String.Empty
    Private _EDIOutput As String = String.Empty
    Private _AppPath As String = String.Empty
    Private _MachineName As String = String.Empty
    Private _UserLoginName As String = String.Empty


    Public Function LeftVB6(ByVal str As String, ByVal length As Integer) As String
        Return str.Substring(0, Math.Min(str.Length, length))
    End Function

    Public Function RightVB6(ByVal str As String, ByVal length As Integer) As String
        Return str.Substring(0, Math.Min(str.Length, length))
    End Function

    Public Function GetFileName(ByVal g As String) As String

        Dim r As String

        Return r

    End Function

    Public Function GetFolder(ByVal g As String) As String

        Dim r As String

        r = g


        Return r

    End Function


    Public Function Day(ByVal noclue As Object) As Integer

    End Function



    Private Sub cmdOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOk.Click

        Me.Close()


    End Sub

    Private Sub cmdDone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDone.Click
        Me.Close()
    End Sub

    Private Sub frmEDIWizard_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Dim _MachineName As String = String.Empty



        _AppPath = Environment.CurrentDirectory
        _MachineName = Environment.MachineName
        _UserLoginName = Environment.UserName




        _MachineName = _MachineName


        '**************************************************************************************************************************************************************
        '' glen --  now has proper debug mode so no need only set isdebug in app confif in prod
        'If Trim(_MachineName) <> "PSYEDI01" And Trim(_MachineName) <> "PSYQUEL-EDI" And Trim(_MachineName) <> "ADM-00" And "EDI-01" <> Trim(_MachineName) Then
        '    MsgBox("Electronic Claims can only be processed from the PSYQUEL-EDI Server.(" & _MachineName & ")", vbOKOnly + vbCritical, "Close Wizard")
        '    Me.Tag = "Cancel"
        '    Me.Hide()
        '    Me.Close()

        '    Exit Sub
        'End If
        '**************************************************************************************************************************************************************




        GetSettings()

        InitSettings()
        Me.Show()
        GenerateFile()

        '**************************************************************************************************************************************************************
        ' this now in secheule log
        ' SendEmailMessage("Electronic Claims", "The Electronic Claims Daemon completed Successfully" & vbCrLf & Me.txtStatus.Text)
        '**************************************************************************************************************************************************************


    End Sub
    Private Sub InitSettings()

        Dim strTemp As String
        Dim strLoc As String
        Dim lngPos As Long

        Dim sb As New StringBuilder()






        blnEmailSuccess = True
        blnEmailFailure = True




        'If Not File.Exists(FixedFileName) Then
        '    Using sw As StreamWriter = File.CreateText(FixedFileName)
        '    End Using
        'Else
        '    log.ExceptionDetails("DCSGlobal.EDI.FileFix.Go", " Exists " + FixedFileName)
        '    Return 2
        '    Exit Sub
        'End If

        Try

            'System.IO.f()



            '' test to make sure its here
            If Not Directory.Exists(_EDIOutput) Then
                Directory.CreateDirectory(_EDIOutput)
            End If

            '' change to that direcotry
            My.Computer.FileSystem.CurrentDirectory = _EDIOutput

            CONST_OUTPUT_DIR = _EDIOutput




            '' this below is all pointless

            'make sure .ini exists
            ' '' '' '' '' '' '' ''            strLoc = Dir("\EClaim.ini")

            ' '' '' '' '' '' '' ''            If strLoc = "" Then
            ' '' '' '' '' '' '' ''                'we are resetting
            ' '' '' '' '' '' '' ''SetLoc:
            ' '' '' '' '' '' '' ''                '  Open (_EDIOutput & "\EClaim.ini") For Output As #1
            ' '' '' '' '' '' '' ''                FileOpen(1, "strLoc", OpenMode.Output, OpenAccess.Default, OpenShare.Shared)

            ' '' '' '' '' '' '' ''                'Print #1, "[EClaim]"
            ' '' '' '' '' '' '' ''                PrintLine(1, "[EClaim]")

            ' '' '' '' '' '' '' ''                ' '' '' '' '' ''Microsoft.VisualBasic.Compatibility.VB6.

            ' '' '' '' '' '' '' ''                ' '' '' '' '' ''  MsgBox("Please choose an output directory Initialize settings")
            ' '' '' '' '' '' '' ''                ' '' '' '' '' ''    frmBrowse.Show()

            ' '' '' '' '' '' '' ''                ' '' '' '' '' ''     strLoc = frmBrowse.dirList.Path
            ' '' '' '' '' '' '' ''                If RightVB6(strLoc, 1) <> "\" Then
            ' '' '' '' '' '' '' ''                    strLoc = strLoc & "\"
            ' '' '' '' '' '' '' ''                End If

            ' '' '' '' '' '' '' ''                If strLoc <> "" Then
            ' '' '' '' '' '' '' ''                    '' '' '' '' ''    'Print #1, "Output=" & strLoc
            ' '' '' '' '' '' '' ''                    PrintLine(1, "Output=" & _EDIOutput)

            ' '' '' '' '' '' '' ''                End If


            ' '' '' '' '' '' '' ''                '' '' '' '' ''frmBrowse.Close()

            ' '' '' '' '' '' '' ''                'Close #1
            ' '' '' '' '' '' '' ''                FileClose(1)

            ' '' '' '' '' '' '' ''            Else
            ' '' '' '' '' '' '' ''                'find and set parameters from .ini
            ' '' '' '' '' '' '' ''                strLoc = ""
            ' '' '' '' '' '' '' ''                '  Open (_EDIOutput & "\EClaim.ini") For Input As #1

            ' '' '' '' '' '' '' ''                ' FileOpen(1, _EDIOutput & "\EClaim.ini", OpenMode.Input, OpenAccess.Default, OpenShare.Shared)





            ' '' '' '' '' '' '' ''                Do While Not EOF(1)

            ' '' '' '' '' '' '' ''                    'Input #1, strTemp

            ' '' '' '' '' '' '' ''                    Input(1, strTemp)


            ' '' '' '' '' '' '' ''                    lngPos = InStr(strTemp, "=")
            ' '' '' '' '' '' '' ''                    If lngPos > 1 Then
            ' '' '' '' '' '' '' ''                        Select Case LeftVB6(strTemp, lngPos - 1)
            ' '' '' '' '' '' '' ''                            Case "Output"
            ' '' '' '' '' '' '' ''                                strLoc = RightVB6(strTemp, Len(strTemp) - lngPos)
            ' '' '' '' '' '' '' ''                        End Select
            ' '' '' '' '' '' '' ''                    End If
            ' '' '' '' '' '' '' ''                Loop

            ' '' '' '' '' '' '' ''                FileClose(1)

            ' '' '' '' '' '' '' ''                'if no output dir is found, recreate file
            ' '' '' '' '' '' '' ''                If strLoc = "" Then
            ' '' '' '' '' '' '' ''                    MsgBox("Error")
            ' '' '' '' '' '' '' ''                    Kill(_EDIOutput & "\EClaim.ini")
            ' '' '' '' '' '' '' ''                    GoTo SetLoc
            ' '' '' '' '' '' '' ''                    Exit Sub
            ' '' '' '' '' '' '' ''                End If
            ' '' '' '' '' '' '' ''            End If

            ' '' '' '' '' '' '' ''            CONST_OUTPUT_DIR = strLoc


        Catch ex As Exception
            MsgBox(ex.Message)

        End Try


    End Sub


    Public Sub SendEmailMessage(ByVal strSubject As String, ByVal strMessage As String)
        '--------------------------------------------------------------------
        'Date: 05/24/2001
        'Author: Eric Pena
        'Description:  Sends an email message
        'Parameters:
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Const cdoSendUsingMethod = "http:'schemas.microsoft.com/cdo/configuration/sendusing"
        Const cdoSendUsingPort = 2
        Const cdoSMTPServer = "http:'schemas.microsoft.com/cdo/configuration/smtpserver"
        Const cdoSMTPServerPort = "http:'schemas.microsoft.com/cdo/configuration/smtpserverport"
        Const cdoSMTPConnectionTimeout = "http:'schemas.microsoft.com/cdo/configuration/smtpconnectiontimeout"
        Const cdoSMTPUsess = "http:'schemas.microsoft.com/cdo/configuration/smtpusessl"
        Const cdoSMTPAuthenticate = "http:'schemas.microsoft.com/cdo/configuration/smtpauthenticate"
        Const cdoBasic = 1
        Const cdoAnonymous = 0
        Const cdoSendUserName = "http:'schemas.microsoft.com/cdo/configuration/sendusername"
        Const cdoSendPassword = "http:'schemas.microsoft.com/cdo/configuration/sendpassword"




        Dim _Server As String = String.Empty
        Dim _ServerAccount As String = String.Empty

        Dim _ServerPassword = String.Empty

        Dim _SendFrom As String = String.Empty
        Dim _SendTo As String = String.Empty






        'Dim objConfig As New CDO.Configuration
        'Dim objMail As New CDO.Message
        'Dim Fields As ADODB.Fields
        Dim strRecipient As String
        'Dim _MachineName As String

        'On Error GoTo ErrHand

        '' '' '' '' '' '' Set config fields we care about
        ' '' '' '' '' ''Fields = objConfig.Fields

        ' '' '' '' '' ''With Fields
        ' '' '' '' '' ''    .Item(cdoSendUsingMethod) = 2
        ' '' '' '' '' ''    .Item(cdoSMTPUsess) = True
        ' '' '' '' '' ''    .Item(cdoSMTPServer) = "email-smtp.us-east-2.amazonaws.com"
        ' '' '' '' '' ''    .Item(cdoSMTPServerPort) = 25
        ' '' '' '' '' ''    .Item(cdoSMTPConnectionTimeout) = 10
        ' '' '' '' '' ''    .Item(cdoSMTPAuthenticate) = 1
        ' '' '' '' '' ''    .Item(cdoSendUserName) = "AKIAYYPPLV2F6KWGF6MB"
        ' '' '' '' '' ''    .Item(cdoSendPassword) = "BDqepESBgKEyadAKOg3viIXz/V0Bz1LH1xGVWJRJduYc"
        ' '' '' '' '' ''    .Update()
        ' '' '' '' '' ''End With


        _Server = "email-smtp.us-east-2.amazonaws.com"
        _ServerAccount = "AKIAYYPPLV2F6KWGF6MB"
        _ServerPassword = "BDqepESBgKEyadAKOg3viIXz/V0Bz1LH1xGVWJRJduYc"

        strRecipient = "QWareAdmin@Psyquel.com"
        '_MachineName = GetMachineName()

        'objMail.Configuration = objConfig
        'objMail.From = _MachineName & "@psyquel.com"
        'objMail.To = strRecipient
        'objMail.Subject = strSubject
        'objMail.TextBody = "The following message was generated from the Claim Generator running on " & _MachineName & ":" & vbCrLf & strMessage
        'objMail.Send()
        'objMail = Nothing



        'ErrHand:
        '        Err.Raise(Err.Number, Err.Source, Err.Description)
        '        ShowError(Err)



        'create the mail message
        Dim mail As New MailMessage()




        'set the addresses
        mail.From = New MailAddress(_MachineName & "@psyquel.com")
        mail.[To].Add(strRecipient)

        'set the content
        mail.Subject = strSubject
        mail.Body = "The following message was generated from the Claim Generator running on " & _MachineName & ":" & vbCrLf & strMessage

        'set the server
        Dim smtp As New SmtpClient("email-smtp.us-east-2.amazonaws.com")

        'send the message
        Try
            smtp.Send(mail)
            ' Response.Write("Your Email has been sent sucessfully - Thank You")
        Catch exc As Exception



            'Response.Write("Send failure: " & exc.ToString())
        End Try










    End Sub






    Public Function getPendingVerif() As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 02/01/2018
        'Author: Duane C Orth
        'Description:  Returns a list of patients for whom initial coverage has not been verified
        'Parameters:
        'Returns: ADORst
        '--------------------------------------------------------------------
        Dim i As Integer = -1

        Dim cnnSQL As New ADODB.Connection
        Dim rstSQL As New ADODB.Recordset



        Dim cmdSQL As New ADODB.Command
        Dim strConnect As String
        Dim strSQL As String

        Try


            cnnSQL.Open(_ConnectionString)
            cmdSQL.ActiveConnection = cnnSQL

            'Create the parameter objects
            With cmdSQL

                strSQL = "SELECT C.fldPatRPPlanID, C.fldPatientID, C.fldRPID, H.fldProviderID, A.fldDateAdded, A.fldAddedBy, A.fldBenefactorID, "
                strSQL = strSQL & "A.fldLast AS fldPatientLastName, A.fldFirst AS fldPatientFirstName, A.fldMI AS fldPatientMI, A.fldAddress1 AS fldPatientStreetNum, A.fldCity AS fldPatientCity, "
                strSQL = strSQL & "A.fldState AS fldPatientState, A.fldZip AS fldPatientZip, A.fldSex AS fldPatientSex, A.fldDOB AS fldPatientDOB, A.fldSSN AS fldPatientSSN, "
                strSQL = strSQL & "D.fldLast AS fldInsuredLastName, D.fldFirst AS fldInsuredFirstName, D.fldMI AS fldInsuredMI, D.fldAddress1 AS fldInsuredStreetNum, D.fldCity AS fldInsuredCity, "
                strSQL = strSQL & "D.fldState AS fldInsuredState, D.fldZip AS fldInsuredZip, D.fldSex AS fldInsdSex, D.fldDOB AS fldInsdDOB, D.fldSSN AS fldInsuredSSN, "
                strSQL = strSQL & "E.fldVerPlanID, E.fldPlanName, CASE WHEN G.fldPayerCodeVerify > '' THEN G.fldPayerCodeVerify ELSE G.fldPayerCode END AS PayerCode, E.fldCardNum, E.fldGroupNum, "
                strSQL = strSQL & "B.fldLastName AS fldProviderLastName, B.fldFirstName AS fldProviderFirstName, B.fldMI AS fldProviderMI, H.fldProviderNPI, H.fldTaxonomyCode, "
                strSQL = strSQL & "H.fldAddress1 AS fldProviderAddress, H.fldCity AS fldProviderCity, H.fldState AS fldProviderState, H.fldZip AS fldProviderZip, "
                strSQL = strSQL & "H.fldSupervisorID, K.fldLastName AS fldSuperLastName, K.fldFirstName AS fldSuperFirstName, K.fldMI AS fldSuperMI, J.fldProviderNPI AS fldSuperNPI, J.fldTaxonomyCode AS fldSuperTaxonomyCode, "
                strSQL = strSQL & "M.fldBusinessName AS fldFacilityName, M.fldAddress1 AS fldFacilityAddress, M.fldCity AS fldFacilityCity, M.fldState AS fldFacilityState, M.fldZip AS fldFacilityZip, "
                strSQL = strSQL & "M.fldPOSCode, M.fldFacilityNPI, M.fldFacilityTaxonomy, "
                strSQL = strSQL & "H.fldGroupID, I.fldGroupNPI, I.fldTaxonomyCode AS fldGroupTaxonomyCode, I.fldGroupName, I.fldTaxID AS GroupTIN, "
                strSQL = strSQL & "G.fldInsuranceID, G.fldCPCID, F.fldPlanID, "
                strSQL = strSQL & "CASE WHEN G.fldVerifyClearingHouseID > 0 THEN G.fldVerifyClearingHouseID ELSE G.fldClearingHouseID END AS fldClearingHouseID, "
                strSQL = strSQL & "G.fldVerifyClearingHouseID, G.fldCPCName, G.fldPayerCode, G.fldPayerCodeVerify, G.fldInsType, "
                strSQL = strSQL & "E.fldVerifyElectronicYN, E.fldReVerifyElectronicYN, E.fldVerifyRejectYN, E.fldVerifyFileID, E.fldVerifyDate "
                strSQL = strSQL & "FROM dbo.tblBenefactor AS A INNER JOIN "
                strSQL = strSQL & "dbo.tblClinic AS M ON M.fldClinicID = A.fldClinicID INNER JOIN "
                strSQL = strSQL & "dbo.tblUser AS B ON A.fldOwnerID = B.fldUserID INNER JOIN "
                strSQL = strSQL & "dbo.tblPatRPPlan AS C ON A.fldBenefactorID = C.fldPatientID INNER JOIN "
                strSQL = strSQL & "dbo.tblBenefactor AS D ON C.fldRPID = D.fldBenefactorID INNER JOIN "
                strSQL = strSQL & "dbo.tblPatRPPlanRule AS E ON C.fldPatRPPlanID = E.fldPatRPPlanID INNER JOIN "
                strSQL = strSQL & "dbo.tblPlan AS F ON E.fldVerPlanID = F.fldPlanID INNER JOIN "
                strSQL = strSQL & "dbo.tblClaimsProcCtr AS G ON G.fldCPCID = F.fldCPCID INNER JOIN "
                strSQL = strSQL & "dbo.tblProvider AS H  ON H.fldProviderID = B.fldUserID INNER JOIN "
                strSQL = strSQL & "dbo.tblProviderGroup AS I ON H.fldGroupID = I.fldGroupID  LEFT OUTER JOIN "
                strSQL = strSQL & "dbo.tblProvider AS J ON H.fldSupervisorID = J.fldProviderID  LEFT OUTER JOIN "
                strSQL = strSQL & "dbo.tblUser AS K ON K.fldUserID = J.fldProviderID "
                strSQL = strSQL & "Where "
                strSQL = strSQL & "(B.fldDisabledYN = 'N') AND "
                strSQL = strSQL & "(C.fldDisabledYN = 'N') AND "
                strSQL = strSQL & "(C.fldPlanID = 3087) AND "
                strSQL = strSQL & "(E.fldVerPlanID > 0) AND "
                strSQL = strSQL & "(G.fldVerifyElectronicYN = 'Y') AND "
                strSQL = strSQL & "(E.fldVerifyFileID IS NULL OR E.fldVerifyFileID = '') AND "
                strSQL = strSQL & "(E.fldVerifyElectronicYN = 'Y' or fldReVerifyElectronicYN = 'Y') "
                strSQL = strSQL & "UNION "
                strSQL = strSQL & "SELECT C.fldPatRPPlanID, C.fldPatientID, C.fldRPID, H.fldProviderID, A.fldDateAdded, A.fldAddedBy, A.fldBenefactorID, "
                strSQL = strSQL & "A.fldLast AS fldPatientLastName, A.fldFirst AS fldPatientFirstName, A.fldMI AS fldPatientMI, A.fldAddress1 AS fldPatientStreetNum, A.fldCity AS fldPatientCity, "
                strSQL = strSQL & "A.fldState AS fldPatientState, A.fldZip AS fldPatientZip, A.fldSex AS fldPatientSex, A.fldDOB AS fldPatientDOB, A.fldSSN AS fldPatientSSN, "
                strSQL = strSQL & "D.fldLast AS fldInsuredLastName, D.fldFirst AS fldInsuredFirstName, D.fldMI AS fldInsuredMI, D.fldAddress1 AS fldInsuredStreetNum, D.fldCity AS fldInsuredCity, "
                strSQL = strSQL & "D.fldState AS fldInsuredState, D.fldZip AS fldInsuredZip, D.fldSex AS fldInsdSex, D.fldDOB AS fldInsdDOB, D.fldSSN AS fldInsuredSSN, "
                strSQL = strSQL & "E.fldVerPlanID, E.fldPlanName, CASE WHEN G.fldPayerCodeVerify > '' THEN G.fldPayerCodeVerify ELSE G.fldPayerCode END AS PayerCode, E.fldCardNum, E.fldGroupNum, "
                strSQL = strSQL & "B.fldLastName AS fldProviderLastName, B.fldFirstName AS fldProviderFirstName, B.fldMI AS fldProviderMI, H.fldProviderNPI, H.fldTaxonomyCode, "
                strSQL = strSQL & "H.fldAddress1 AS fldProviderAddress, H.fldCity AS fldProviderCity, H.fldState AS fldProviderState, H.fldZip AS fldProviderZip, "
                strSQL = strSQL & "H.fldSupervisorID, K.fldLastName AS fldSuperLastName, K.fldFirstName AS fldSuperFirstName, K.fldMI AS fldSuperMI, J.fldProviderNPI AS fldSuperNPI, J.fldTaxonomyCode AS fldSuperTaxonomyCode, "
                strSQL = strSQL & "M.fldBusinessName AS fldFacilityName, M.fldAddress1 AS fldFacilityAddress, M.fldCity AS fldFacilityCity, M.fldState AS fldFacilityState, M.fldZip AS fldFacilityZip, "
                strSQL = strSQL & "M.fldPOSCode, M.fldFacilityNPI, M.fldFacilityTaxonomy, "
                strSQL = strSQL & "H.fldGroupID, I.fldGroupNPI, I.fldTaxonomyCode AS fldGroupTaxonomyCode, I.fldGroupName, I.fldTaxID AS GroupTIN, "
                strSQL = strSQL & "G.fldInsuranceID, G.fldCPCID, F.fldPlanID, "
                strSQL = strSQL & "CASE WHEN G.fldVerifyClearingHouseID > 0 THEN G.fldVerifyClearingHouseID ELSE G.fldClearingHouseID END AS fldClearingHouseID, "
                strSQL = strSQL & "G.fldVerifyClearingHouseID, G.fldCPCName, G.fldPayerCode, G.fldPayerCodeVerify, G.fldInsType, "
                strSQL = strSQL & "E.fldVerifyElectronicYN, E.fldReVerifyElectronicYN, E.fldVerifyRejectYN, E.fldVerifyFileID, E.fldVerifyDate "
                strSQL = strSQL & "FROM dbo.tblBenefactor AS A INNER JOIN "
                strSQL = strSQL & "dbo.tblClinic AS M ON M.fldClinicID = A.fldClinicID INNER JOIN "
                strSQL = strSQL & "dbo.tblUser AS B ON A.fldOwnerID = B.fldUserID INNER JOIN "
                strSQL = strSQL & "dbo.tblPatRPPlan AS C ON A.fldBenefactorID = C.fldPatientID INNER JOIN "
                strSQL = strSQL & "dbo.tblBenefactor AS D ON C.fldRPID = D.fldBenefactorID INNER JOIN "
                strSQL = strSQL & "dbo.tblPatRPPlanRule AS E ON C.fldPatRPPlanID = E.fldPatRPPlanID INNER JOIN "
                strSQL = strSQL & "dbo.tblPlan AS F ON E.fldVerPlanID = F.fldPlanID INNER JOIN "
                strSQL = strSQL & "dbo.tblClaimsProcCtr AS G ON G.fldCPCID = F.fldCPCID INNER JOIN "
                strSQL = strSQL & "dbo.tblProvider AS H  ON H.fldProviderID = B.fldUserID INNER JOIN "
                strSQL = strSQL & "dbo.tblProviderGroup AS I ON H.fldGroupID = I.fldGroupID  LEFT OUTER JOIN "
                strSQL = strSQL & "dbo.tblProvider AS J ON H.fldSupervisorID = J.fldProviderID  LEFT OUTER JOIN "
                strSQL = strSQL & "dbo.tblUser AS K ON K.fldUserID = J.fldProviderID "
                strSQL = strSQL & "Where "
                strSQL = strSQL & "(B.fldDisabledYN = 'N') AND "
                strSQL = strSQL & "(C.fldDisabledYN = 'N') AND "
                strSQL = strSQL & "(C.fldPlanID <> 3087) AND "
                strSQL = strSQL & "(C.fldPlanID > 0) AND "
                strSQL = strSQL & "(G.fldVerifyElectronicYN = 'Y') AND "
                strSQL = strSQL & "(E.fldVerifyFileID IS NULL OR E.fldVerifyFileID = '') AND "
                strSQL = strSQL & "(E.fldVerifyElectronicYN = 'Y' or fldReVerifyElectronicYN = 'Y') "
                strSQL = strSQL & "ORDER BY fldClearingHouseID, fldPayerCode, fldGroupNPI, fldProviderNPI, fldSuperNPI, fldInsuranceID, fldCPCName, fldProviderLastName, fldProviderFirstName, fldRPID, fldPatientID "

                '       strSQL = "SELECT * FROM vw_OpenVerifications "
                '       strSQL = strSQL & " WHERE PlanVerifyElectronicYN = 'Y' AND (fldVerifyFileID IS NULL OR fldVerifyFileID = '') AND (fldVerifyElectronicYN = 'N' or fldReVerifyElectronicYN = 'Y') "
                '       strSQL = strSQL & " ORDER BY fldClearingHouseID, fldPayerCode, fldGroupNPI, fldProviderNPI, fldSuperNPI, fldInsuranceID, fldCPCName, fldProviderLastName, fldProviderFirstName, fldRPID, fldPatientID "
            End With

            cmdSQL.CommandText = strSQL
            cmdSQL.CommandType = adCmdText

            'Execute the stored procedure
            rstSQL.CursorLocation = adUseClient
            rstSQL.Open(cmdSQL, , adOpenForwardOnly, adLockReadOnly)

            getPendingVerif = rstSQL
            i = 0


        Catch ex As Exception

        End Try
        ''Disconnect the recordset
        'cmdSQL.ActiveConnection = Nothing
        'cmdSQL = Nothing
        'rstSQL.ActiveConnection = Nothing
        'cnnSQL = Nothing
        Return rstSQL

        'ShowError(Err)
    End Function



    Private Function WaitOnProgram(ByVal lngProgID As Long, Optional ByVal blnWaitDead As Boolean = False) As Long
        '-----------------------------------------------------------------------------------
        'Author: Dave Richkun
        'Date: 23-Nov-1998
        'Description: This routine forces Windows to halt execution until the Windows program
        '             identified by the lngProgID parameter has finished executing.  Visual
        '             Basic's 'Shell' command executes programs asynchronously, a nice feature,
        '             but unwanted in the this particular case.
        'Parameters:  lngProgID - The ProcessID of the Windows program we are monitoring
        '             blnWaitDead - An optional parameter that identifies if execution should
        '               be stopped in it's tracks until the monitored process has completed.
        'Returns: A long integer identifying success (non-zero) or failure (zero)
        '-----------------------------------------------------------------------------------
        'Revision History:
        '
        '-----------------------------------------------------------------------------------

        Dim lngRead As Long
        Dim lngExit As Long
        Dim lngHProg As Long
        Dim lngResult As Long


        'Get the process handle of the running program
        lngHProg = OpenProcess(PROCESS_ALL_ACCESS, False, lngProgID)
        If blnWaitDead Then
            'Stop dead until the process terminates
            lngResult = WaitForSingleObject(lngHProg, INFINITE)

            If lngResult = WAIT_FAILED Then
                If Err.Source > "" Then
                    Err.Raise(Err.LastDllError)
                End If
            End If
            'Get the return value
            lngResult = GetExitCodeProcess(lngHProg, lngExit)
        Else
            'Get the return value
            lngResult = GetExitCodeProcess(lngHProg, lngExit)
            'Wait, but allow painting and other processing
            Do While lngExit = STILL_ACTIVE
                'DoEvents()
                Application.DoEvents()


                lngResult = GetExitCodeProcess(lngHProg, lngExit)
            Loop
        End If
        lngResult = CloseHandle(lngHProg)

        WaitOnProgram = lngExit

    End Function

    Private Function OpenEDIFile(ByVal rstClrHse As ADODB.Recordset, ByVal strFileType As String, ByVal strClaimType As String) As Long
        'Opens/creates a new EDI file.  File name is based on the create date (MMDDYY)
        'and a unique file number retrieved from the database.

        Dim objEDI As New EDIBZ.CEDIBz
        Dim objCLH As New InsuranceBz.CClearingHouseBz
        Dim rst As New ADODB.Recordset
        Dim strFileName As String
        Dim lngStartTxNum As Long


        'objEDI = CreateObject("EDIBZ.CEDIBz")

        objEDI.ConnectionString = _ConnectionString
        objCLH.ConnectionString = _ConnectionString


        rst = objEDI.GetNextFileNumber()
        objEDI = Nothing

        If rst.EOF = True And rst.BOF Then
            'Defensive programming for first time run-through
            g_FileNumber = 1
            g_lngStartTxNum = 1
            g_lngEndTxNum = 1
        Else
            g_FileNumber = rst.Fields("fldFileID").Value + 1
            g_lngStartTxNum = rst.Fields("fldEndTxNum").Value + 1
            g_lngEndTxNum = rst.Fields("fldEndTxNum").Value + 1
        End If

        If Dir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, ""), vbDirectory) = "" Then
            MkDir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, ""))
        End If


        If Dir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Sub", vbDirectory) = "" Then
            MkDir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Sub")
        End If



        If Dir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Log", vbDirectory) = "" Then
            MkDir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Log")
        End If


        If Dir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\RSP", vbDirectory) = "" Then
            MkDir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\RSP")
        End If


        If Dir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK", vbDirectory) = "" Then
            MkDir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK")
            MkDir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\Accepted")
            MkDir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\Rejected")
        End If


        If Dir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\277", vbDirectory) = "" Then
            MkDir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\277")
            MkDir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\277\Accepted")
            MkDir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\277\Rejected")
        End If


        If Dir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\835", vbDirectory) = "" Then
            MkDir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\835")
            MkDir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\835\Archive")
        End If

        'Check last Clearing House File Name


        Dim _fldConcatFileYN As String = String.Empty
        Dim _fldEFileName As String = String.Empty




        _fldConcatFileYN = rstClrHse.Fields("fldConcatFileYN").Value





        If (_DB.IfNull(rstClrHse.Fields("fldConcatFileYN").Value, "") = "Y") And _
                     (_DB.IfNull(rstClrHse.Fields("fldEFileName").Value, "") > "") And _
                     (RightVB6(_DB.IfNull(rstClrHse.Fields("fldEFileName").Value, ""), 3) = strFileType) And _
        (Dir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & _DB.IfNull(rstClrHse.Fields("fldEFileName").Value, "")) > "") Then
            g_FileName = _DB.IfNull(rstClrHse.Fields("fldEFileName").Value, "")



            '= Open (_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & g_FileName) For Append As #1
            g_FileName = (_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & g_FileName)
            FileOpen(1, g_FileName, OpenMode.Append, OpenAccess.Default, OpenShare.Shared)



        Else
            rst = Nothing
            strFileName = Now().ToString("MM") & VB6.Format(Convert.ToString(Day(Now)), "00") & Mid(Convert.ToString(Year(Now)), 3)

            If strFileType = "270" Then
                g_FileName = strFileName & Convert.ToString(g_FileNumber) & ".270" 'Append file number to filename to guarantee uniqueness
            Else
                If strClaimType = "I" Then
                    g_FileName = strFileName & Convert.ToString(g_FileNumber) & ".837i" 'Institutional Claims
                Else
                    g_FileName = strFileName & Convert.ToString(g_FileNumber) & ".txt" 'Append file number to filename to guarantee uniqueness
                End If
            End If
            '

            If Dir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, ""), vbDirectory) = "" Then
                MkDir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, ""))
            End If
            If Dir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Sub", vbDirectory) = "" Then
                MkDir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Sub")
            End If

            If (_DB.IfNull(rstClrHse.Fields("fld270").Value, "N") = "Y") Then
                If Dir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\270", vbDirectory) = "" Then
                    MkDir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\270")
                End If
                If Dir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\271", vbDirectory) = "" Then
                    MkDir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\271")
                    MkDir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\271\Archive")
                End If
            End If

            'Overwrite file if it exists
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & g_FileName) > "" Then
                Kill(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & g_FileName)
            End If

            objCLH.UpdateEFile(rstClrHse.Fields("fldClearingHouseID").Value, g_FileName)
            objCLH = Nothing
            Dim __FileName As String = String.Empty


            __FileName = (_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & g_FileName)
            '  Open (_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & g_FileName) For Output As #1
            FileOpen(2, __FileName, OpenMode.Output, OpenAccess.Default, OpenShare.Shared)


        End If

        OpenEDIFile = 1


    End Function

    Private Function CloseEDIFile()
        'Closes the file, and inserts a record of the file in the database.

        Dim objEDI As New EDIBZ.CEDIBz



        ' Close #1 'Close the file
        FileClose(1)

        'this now done in form looad and is scoped to entire class
        '_UserLoginName = GetLoginName()

        If g_lngEndTxNum < g_lngStartTxNum Then

            MsgBox("An error has resulted that prevents these claims from being updated correctly.  Please try to send these claims again.", vbOKOnly + vbCritical, "Submission Error")
            Me.Tag = "Cancel"

        Else

            'not needed 
            'objEDI = CreateObject("EDIBz.CEDIBz")
            objEDI.Insert(g_FileNumber, g_FileName, g_lngStartTxNum, g_lngEndTxNum, _UserLoginName)
            objEDI = Nothing


        End If



    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GenerateFile()
        '-------------------------------------------------------------------------------
        'Author: Dave Richkun
        'Date: 11/07/2002
        'Description: Main function that generates EDI text file compliant to X.12 837 v4010
        'Parameters:
        'Returns:
        '-------------------------------------------------------------------------------
        ' this builds both 270 and 837 probally 2767 at one time but not any more


        Dim objEDI As New EDIBZ.CEDIBz
        Dim objClrHse As New InsuranceBz.CClearingHouseBz
        Dim rst As New ADODB.Recordset
        Dim rstClrHse As New ADODB.Recordset
        Dim strWhere, strFile As String
        Dim lngClrHseID As Long
        Dim lngPrevClrHseID As Long
        Dim lngStatusID As Long
        Dim strClaimType As String
        Dim strPrevPayerCode As String
        Dim strPrevSubmitterID As String
        Dim strPrevClaimType As String
        Dim strProgram, strLine As String
        Dim dblProgID As Double
        Dim intExit, intCnt, intWrk As Integer


        Dim __FileName As String = String.Empty
        Dim __LineString As String = String.Empty


        objEDI.ConnectionString = _ConnectionString
        objClrHse.ConnectionString = _ConnectionString


        'Dim _MachineName As String

        '_MachineName = GetMachineName()

        '    Dim objFS As New Scripting.FileSystemObject

        Dim objFolder 'objFile
        ' Dim objText As New Scripting.TextStream

        '    On Error GoTo ErrTrap

        ' objFS = CreateObject("Scripting.FileSystemObject")

        'Screen.MousePointer  = vbHourglass

        lngClrHseID = 1
        intCnt = 1
        lngPrevClrHseID = 0


        '************************************************************************************************************************************
        ' 270

        '*************************************
        'CheckBox to see if in debug and we actaully want to test 270

        Dim Do270 As Boolean = False


        If Do270 Then

            rst = getPendingVerif() 'Fetch Verifications
            Do Until rst.EOF
                lngClrHseID = 1

                Try
                    lngClrHseID = rst.Fields("fldClearingHouseID").Value
                Catch ex As Exception

                End Try

                ' this does nothing ?
                If Not IsDBNull(rst.Fields("fldClearingHouseID").Value) Then

                End If

                'not needed 
                'objClrHse = CreateObject("InsuranceBZ.CClearingHouseBz")
                rstClrHse = objClrHse.Fetch(False, "fldClearingHouseID = " & lngClrHseID)
                objClrHse = Nothing

                If rstClrHse.EOF = True Then
                    lngClrHseID = 1
                End If


                If lngClrHseID <> lngPrevClrHseID Or _
                   strPrevPayerCode <> _DB.IfNull(rst.Fields("fldPayerCodeVerify").Value, _DB.IfNull(rst.Fields("fldPayerCode").Value, "")) Then
                    lngStatusID = OpenEDIFile(rstClrHse, "270", "")

                    If lngClrHseID <> 66 Then
                        lngPrevClrHseID = lngClrHseID
                    End If

                    strPrevPayerCode = _DB.IfNull(rst.Fields("fldPayerCodeVerify").Value, _DB.IfNull(rst.Fields("fldPayerCode").Value, ""))
                    '         strPrevSubmitterID = _DB.IfNull(rst.Fields("SubmitterID").Value, "")
                End If

                '    g_lngEndTxNum = GenerateVerificationRequest(rstClrHse, rst, g_lngEndTxNum, g_FileNumber, g_FileName)

                '  CloseEDIFile()

                If Me.Tag = "Cancel" Then
                    Exit Do
                End If

            Loop
            rst = Nothing
            rstClrHse = Nothing


        End If



        '*************************************
        '************************************************************************************************************************************

















        lngClrHseID = 1 : intCnt = 1 : lngPrevClrHseID = 0 : strPrevClaimType = "" : strPrevPayerCode = "" : strPrevSubmitterID = ""







        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'Start 837
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''



        'not needed 
        'objClrHse = CreateObject("InsuranceBZ.CClearingHouseBz")

        '' why did we call this look to see what machine we are on and call it again    --- I need to go fix all that nonsense
        rstClrHse = objClrHse.Fetch(False, " fldNightlyProcessYN = 'Y'", "fldClearingHouseID")

        objClrHse = Nothing

        txtStatus.Text = "Start: " & VB6.Format(Now(), "long date") & " " & VB6.Format(Now(), "long time") & vbCrLf


        'DoEvents()

        Do While Not rstClrHse.EOF

            'txtStatus.Text = "Next: " & lngClrHseID & " " & rstClrHse.Fields("fldDescription").Value & vbCrLf & txtStatus.Text
            'DoEvents

            If IsDBNull(rstClrHse.Fields("fldClearingHouseID").Value) Then
                lngClrHseID = 1
            Else
                lngClrHseID = rstClrHse.Fields("fldClearingHouseID").Value
            End If



            strWhere = " Where fldQueuedYN = 'Y' AND fldClearingHouseID = " & lngClrHseID


            'not needed 
            'objEDI = CreateObject("EDIBz.CEDIBz")
            rst = objEDI.FetchElectClaims(strWhere) 'Fetch claims
            objEDI = Nothing



            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' this block creates the 837 file and moves it and what ever

            strPrevPayerCode = "" : strPrevSubmitterID = "" : g_FileName = "No File" : g_lngNumClaims = 0
            If Not rst.EOF Then
                '  barStatus. = 0
                '   barStatus.Max = rst.RecordCount

                Do While Not rst.EOF
                    If Not IsDBNull(rst.Fields("fldClaimType").Value) Then strClaimType = rst.Fields("fldClaimType").Value

                    If lngClrHseID <> lngPrevClrHseID Or strPrevClaimType <> _DB.IfNull(rst.Fields("fldClaimType").Value, "") Or strPrevPayerCode <> _DB.IfNull(rst.Fields("fldPayerCode").Value, "") Or strPrevSubmitterID <> _DB.IfNull(rst.Fields("SubmitterID").Value, "") Then

                        lngStatusID = OpenEDIFile(rstClrHse, "txt", strClaimType)
                        lngPrevClrHseID = lngClrHseID
                        strPrevClaimType = _DB.IfNull(rst.Fields("fldClaimType").Value, "")
                        strPrevPayerCode = _DB.IfNull(rst.Fields("fldPayerCode").Value, "")
                        strPrevSubmitterID = _DB.IfNull(rst.Fields("SubmitterID").Value, "")

                    End If
                    Dim tl As New List(Of String)
                    tl.Clear()
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    ' atuall call to creat 837 docuemnt
                    tl = xGenerateElectronicClaims(rstClrHse, rst, g_lngEndTxNum, g_FileNumber, g_FileName, True, "")
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''






                    CloseEDIFile()

                    'If Me.Tag = "Cancel" Then Exit Do
                    If Not rst.EOF Then
                        rst.MoveNext()
                    End If

                Loop

                'Free resources
                rst = Nothing



                If (_DB.IfNull(g_FileName, "") > "") And (g_FileName <> "No File") And _
                  (Dir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & _DB.IfNull(g_FileName, "")) > "") And _
                  (Dir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ZSEND32.wax") > "" Or _
                   Dir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") > "") Then


                    '************************************************************************************************************************************
                    '************************************************************************************************************************************
                    '************************************************************************************************************************************

                    Select Case rstClrHse.Fields("fldClearingHouseID").Value
                        Case 1 To 11, 13, 15, 17 To 24, 26 To 30, 32 To 51, 54, 56 To 59, 62, 63, 68 To 72, 74, 75, 84 To 91, 93 To 103, 110, 111, 112, 118 To 123, 124, 125, 127, 131 To 134, 136, 138, 139, 141 To 146, 149 To 151, 154 To 158
                            '**********************************************************************************************************************************
                            __FileName = String.Empty
                            __LineString = String.Empty

                            'OpenEDIFile #2 = (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") ' For Output As #2
                            __FileName = (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") ' For Output As #2
                            FileOpen(2, __FileName, OpenMode.Output, OpenAccess.Default, OpenShare.Shared)

                            If rstClrHse.Fields("fldSftpSite").Value > "" Then
                                If rstClrHse.Fields("fld270").Value = "Y" Then
                                    'Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Now().ToString("MM")& "*" & Now().ToString("yy") & "*.270" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa -log " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\COREFTP.LOG"
                                    __LineString = String.Empty
                                    __LineString = Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Now().ToString("MM") & "*" & Now().ToString("yy") & "*.270" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa -log " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\COREFTP.LOG"
                                    PrintLine(2, __LineString)
                                End If

                                ' Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Now().ToString("MM")& "*" & Now().ToString("yy") & "*.txt" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa -log " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\COREFTP.LOG"
                                __LineString = String.Empty
                                __LineString = Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Now().ToString("MM") & "*" & Now().ToString("yy") & "*.txt" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa -log " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\COREFTP.LOG"
                                PrintLine(2, __LineString)

                                If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*" & Now().ToString("yy") & "*.837i") > "" And rstClrHse.Fields("fldClearingHouseID").Value <> 23 Then
                                    'Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Now().ToString("MM")& "*" & Now().ToString("yy") & "*.837i" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa -log " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\COREFTP.LOG"
                                    __LineString = String.Empty
                                    __LineString = Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Now().ToString("MM") & "*" & Now().ToString("yy") & "*.837i" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa -log " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\COREFTP.LOG"
                                    PrintLine(2, __LineString)
                                End If

                                If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*" & Now().ToString("yy") & "*.837i") > "" And rstClrHse.Fields("fldClearingHouseID").Value = 23 Then
                                    'Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Now().ToString("MM")& "*" & Now().ToString("yy") & "*.837i" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & "/Uploads/837i/" & Chr(34) & " -oa -log " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\COREFTP.LOG"
                                    __LineString = String.Empty
                                    __LineString = Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Now().ToString("MM") & "*" & Now().ToString("yy") & "*.837i" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & "/Uploads/837i/" & Chr(34) & " -oa -log " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\COREFTP.LOG"
                                    PrintLine(2, __LineString)
                                End If

                            End If

                            '  Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            __LineString = String.Empty
                            __LineString = "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            PrintLine(2, __LineString)

                            'Close #2 'Close the file
                            FileClose(2)
                            '************************************************************************************************************************************



                        Case 31        'BcBs Of MS has a different file name MULTIFILES
                            '************************************************************************************************************************************

                            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\MULTIFILES") > "" Then
                                Kill(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\MULTIFILES")
                            End If
                            IO.File.Copy(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & _DB.IfNull(g_FileName, ""), _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\MULTIFILES")
                            '************************************************************************************************************************************






                        Case 25        'BcBs Of NC has a different file name IMS.CLM
                            '************************************************************************************************************************************

                            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\P_I_CEXALL") > "" Then
                                Kill(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\P_I_CEXALL")
                            End If
                            IO.File.Copy(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & _DB.IfNull(g_FileName, ""), _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\P_I_CEXALL")

                            '************************************************************************************************************************************



                        Case 52        'BcBs Of TN has a different file name IMS.CLM
                            '************************************************************************************************************************************

                            __FileName = String.Empty
                            __LineString = String.Empty



                            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & LeftVB6(_DB.IfNull(g_FileName, ""), InStr(1, _DB.IfNull(g_FileName, ""), ".")) & "X12") > "" Then Kill(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & LeftVB6(_DB.IfNull(g_FileName, ""), InStr(1, _DB.IfNull(g_FileName, ""), ".")) & "X12")
                            IO.File.Copy(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName, _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & LeftVB6(_DB.IfNull(g_FileName, ""), InStr(1, _DB.IfNull(g_FileName, ""), ".")) & "X12")


                            'Open (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                            __FileName = (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat")
                            FileOpen(2, __FileName, OpenMode.Output, OpenAccess.Default, OpenShare.Shared)

                            'Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Left(_DB.IfNull(g_FileName, ""), InStr(1, _DB.IfNull(g_FileName, ""), ".")) & "X12" & Chr(34) & " -site BcBs_Of_TN -p " & Chr(34) & "Inbox/" & Chr(34)
                            __LineString = String.Empty
                            __LineString = Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & LeftVB6(_DB.IfNull(g_FileName, ""), InStr(1, _DB.IfNull(g_FileName, ""), ".")) & "X12" & Chr(34) & " -site BcBs_Of_TN -p " & Chr(34) & "Inbox/" & Chr(34)

                            PrintLine(2, __LineString)

                            'Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "Outbox/*.*" & Chr(34) & " -site BcBs_Of_TN -p " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Chr(34) & " -delsrc -o "
                            __LineString = String.Empty
                            __LineString = Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "Outbox/*.*" & Chr(34) & " -site BcBs_Of_TN -p " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Chr(34) & " -delsrc -o "
                            PrintLine(2, __LineString)

                            ' Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            __LineString = String.Empty
                            __LineString = "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            PrintLine(2, __LineString)


                            ' Close #2 'Close the file
                            FileClose(2)
                            '************************************************************************************************************************************


                        Case 53        'BcBs of AZ has a different file name xxxx_0000????.ttt - 00008480.837
                            '************************************************************************************************************************************

                            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\00008480.837") > "" Then
                                Kill(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\00008480.837")
                            End If
                            IO.File.Copy(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & _DB.IfNull(g_FileName, ""), _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\00008480.837")
                            '************************************************************************************************************************************


                        Case 12, 55, 77, 78, 79, 80, 82, 83, 135, 147           'WPS
                            '************************************************************************************************************************************
                            __FileName = String.Empty
                            __LineString = String.Empty

                            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\14541.dat") > "" Then
                                Kill(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\14541.dat")
                            End If
                            IO.File.Copy(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & _DB.IfNull(g_FileName, ""), _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\14541.dat")

                            ' Open (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                            __FileName = (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat")
                            FileOpen(2, __FileName, OpenMode.Output, OpenAccess.Default, OpenShare.Shared)

                            ' Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\14541.dat" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa -log " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\COREFTP.LOG"
                            __LineString = String.Empty
                            __LineString = Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\14541.dat" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa -log " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\COREFTP.LOG"
                            PrintLine(2, __LineString)

                            'Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            __LineString = String.Empty
                            __LineString = "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            PrintLine(2, __LineString)


                            'Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            __LineString = String.Empty
                            __LineString = "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            PrintLine(2, __LineString)

                            'Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            __LineString = String.Empty
                            __LineString = "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            PrintLine(2, __LineString)


                            'Close #2 'Close the file
                            FileClose(2)
                            '************************************************************************************************************************************



                        Case 60        'HealthNow has a different file name IMS.CLM
                            '************************************************************************************************************************************
                            __FileName = String.Empty
                            __LineString = String.Empty


                            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\Z78232.A" & Now().ToString("dd")) > "" Then
                                Kill(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\Z78232.A" & Now().ToString("dd"))
                            End If

                            ' Open (_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\filename.txt") For Output As #2
                            __FileName = (_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\filename.txt")
                            FileOpen(2, __FileName, OpenMode.Output, OpenAccess.Default, OpenShare.Shared)

                            'Print #2, "Z78232.A" & Now().ToString("dd")
                            __LineString = String.Empty
                            __LineString = "Z78232.A" & Now().ToString("dd")
                            PrintLine(2, __LineString)

                            'Close #2 'Close the file
                            FileClose(2)

                            IO.File.Copy(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & _DB.IfNull(g_FileName, ""), _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Z78232.A" & Now().ToString("dd"))
                            '************************************************************************************************************************************


                        Case 64        'Medicaid NE has a different file name EDI000000265.837P.0001.DAT
                            '************************************************************************************************************************************

                            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\EDI000000265.837P." & Now().ToString("HH") & Now().ToString("dd") & ".DAT") > "" Then
                                Kill(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\EDI000000265.837P." & Now().ToString("HH") & Now().ToString("dd") & ".DAT")
                            End If

                            IO.File.Copy(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & _DB.IfNull(g_FileName, ""), _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\EDI000000265.837P." & Now().ToString("HH") & Now().ToString("dd") & ".DAT")
                            '************************************************************************************************************************************


                        Case 66        'BcBs Of RI - Can only send one file at a time
                            '************************************************************************************************************************************
                            __FileName = String.Empty
                            __LineString = String.Empty

                            'Open (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2   
                            __FileName = (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat")
                            FileOpen(2, __FileName, OpenMode.Output, OpenAccess.Default, OpenShare.Shared)


                            If rstClrHse.Fields("fldSftpSite").Value > "" Then
                                ' Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Now().ToString("MM")& "*" & Now().ToString("yy") & "*.txt" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa -log " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\COREFTP.LOG"
                                __LineString = String.Empty
                                __LineString = Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Now().ToString("MM") & "*" & Now().ToString("yy") & "*.txt" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa -log " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\COREFTP.LOG"
                                PrintLine(2, __LineString)
                            End If


                            'Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            __LineString = String.Empty
                            __LineString = "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            PrintLine(2, __LineString)

                            'Close #2 'Close the file
                            FileClose(2)
                            '*************************************

                            strProgram = _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat"
                            dblProgID = Shell(strProgram, vbNormalFocus)
                            intExit = WaitOnProgram(dblProgID, True)

                            If Dir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ERAs.bat") > "" Then
                                strProgram = _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ERAs.bat"
                            End If

                            dblProgID = Shell(strProgram, vbNormalFocus)
                            intExit = WaitOnProgram(dblProgID, True)


                            'Open (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                            __FileName = (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat")
                            FileOpen(2, __FileName, OpenMode.Output, OpenAccess.Default, OpenShare.Shared)

                            If rstClrHse.Fields("fldSftpSite").Value > "" Then
                                If rstClrHse.Fields("fld270").Value = "Y" Then

                                    'Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Now().ToString("MM")& "*" & Now().ToString("yy") & "*.270" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa -log " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\COREFTP.LOG"
                                    __LineString = String.Empty
                                    __LineString = Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Now().ToString("MM") & "*" & Now().ToString("yy") & "*.270" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa -log " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\COREFTP.LOG"
                                    PrintLine(2, __LineString)
                                End If
                            End If

                            ' Close #2 'Close the file
                            FileClose(2)
                            '************************************************************************************************************************************




                        Case 76        'BcBs Of VT has a different file name - 00723801.X12
                            '************************************************************************************************************************************

                            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\00723801.X12") > "" Then
                                Kill(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\00723801.X12")
                            End If
                            IO.File.Copy(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & _DB.IfNull(g_FileName, ""), _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\00723801.X12")
                            '************************************************************************************************************************************


                        Case 92         'BcBs Of HA
                            '************************************************************************************************************************************
                            __FileName = String.Empty
                            __LineString = String.Empty

                            strFile = "ca" & LeftVB6(g_FileName, 10) & "ip.01"
                            For intWrk = 0 To 9
                                If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & strFile) > "" Or Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\" & strFile) > "" Then strFile = "ca" & LeftVB6(g_FileName, 9) & intWrk & "ip.01"
                                If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & strFile) = "" And Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\" & strFile) = "" Then Exit For
                            Next
                            IO.File.Copy(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & _DB.IfNull(g_FileName, ""), _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & strFile)


                            ' Open (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                            __FileName = (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat")
                            FileOpen(2, __FileName, OpenMode.Output, OpenAccess.Default, OpenShare.Shared)

                            'Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ca*ip.01" & Chr(34) & " -site BcBs_Of_HA -p / -o"
                            __LineString = String.Empty
                            __LineString = Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ca*ip.01" & Chr(34) & " -site BcBs_Of_HA -p / -o"
                            PrintLine(2, __LineString)

                            ''           If Dir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.270*") > "" Then
                            ''               Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\la*ip.01" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa "
                            ''           End If


                            ' Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            __LineString = String.Empty
                            __LineString = "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            PrintLine(2, __LineString)

                            'Close #2 'Close the file
                            FileClose(2)
                            '************************************************************************************************************************************

                        Case 104        'BcBs Of Alabama
                            '************************************************************************************************************************************
                            __FileName = String.Empty
                            __LineString = String.Empty

                            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\pbcp000" & Now().ToString("dddd") - 1 & ".8375010.clm") > "" Then
                                Kill(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\pbcp000" & Now().ToString("dddd") - 1 & ".8375010.clm")
                            End If

                            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\pbcp000" & Now().ToString("dddd") - 1 & ".8375010.zip") > "" Then
                                Kill(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\pbcp000" & Now().ToString("dddd") - 1 & ".8375010.zip")
                            End If


                            IO.File.Copy(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & _DB.IfNull(g_FileName, ""), _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\pbcp000" & Now().ToString("dddd") - 1 & ".8375010.clm")


                            'Open (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Pack.bat") For Output As #2
                            __FileName = (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Pack.bat")
                            FileOpen(2, __FileName, OpenMode.Output, OpenAccess.Default, OpenShare.Shared)

                            ' "C:\Program Files\7-Zip\7z.exe" a -t7z "D:\Data\Apps\QBill\BcBsOfAL\pbcp0001.8375010.zip" "D:\Data\Apps\QBill\BcBsOfAL\pbcp0004.8375010.clm"

                            'Print #2, Chr(34) & "C:\Program Files\WinZip\WINZIP32.EXE" & Chr(34) & " -min -a " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\pbcp000" & Now().ToString("dddd") - 1 & ".8375010.zip" & Chr(34) & " " & Chr(34) & _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\pbcp000" & Now().ToString("dddd") - 1 & ".8375010.clm" & Chr(34)
                            __LineString = String.Empty
                            __LineString = Chr(34) & "C:\Program Files\WinZip\WINZIP32.EXE" & Chr(34) & " -min -a " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\pbcp000" & Now().ToString("dddd") - 1 & ".8375010.zip" & Chr(34) & " " & Chr(34) & _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\pbcp000" & Now().ToString("dddd") - 1 & ".8375010.clm" & Chr(34)

                            PrintLine(2, __LineString)


                            'Close #2 'Close the file
                            FileClose(2)
                            '*************************************



                            '*************************************
                            __FileName = String.Empty
                            __LineString = String.Empty

                            strProgram = _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Pack.bat"
                            dblProgID = Shell(strProgram, vbNormalFocus)
                            intExit = WaitOnProgram(dblProgID, True)


                            'Open (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                            __FileName = (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat")
                            FileOpen(2, __FileName, OpenMode.Output, OpenAccess.Default, OpenShare.Shared)



                            'Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\pbcp000" & Now().ToString("dddd") - 1 & ".8375010.zip" & Chr(34) & " -site BcBsOfAL -p /Inbox/"
                            __LineString = String.Empty
                            __LineString = Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\pbcp000" & Now().ToString("dddd") - 1 & ".8375010.zip" & Chr(34) & " -site BcBsOfAL -p /Inbox/"
                            PrintLine(2, __LineString)

                            'Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            __LineString = String.Empty
                            __LineString = "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            PrintLine(2, __LineString)



                            'Close #2 'Close the file
                            FileClose(2)
                            '************************************************************************************************************************************




                        Case 105        'Medicare Of Alabama
                            '************************************************************************************************************************************
                            __FileName = String.Empty
                            __LineString = String.Empty

                            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\palp000" & Now().ToString("dddd") - 1 & ".clm") > "" Then
                                Kill(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\palp000" & Now().ToString("dddd") - 1 & ".clm")
                            End If

                            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\palp000" & Now().ToString("dddd") - 1 & ".zip") > "" Then
                                Kill(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\palp000" & Now().ToString("dddd") - 1 & ".zip")
                            End If

                            IO.File.Copy(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & _DB.IfNull(g_FileName, ""), _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\palp000" & Now().ToString("dddd") - 1 & ".clm")
                            strProgram = Environ("WINZIP") & "\pkzip.exe " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\palp000" & Now().ToString("dddd") - 1 & ".zip " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\palp000" & Now().ToString("dddd") - 1 & ".clm"
                            dblProgID = Shell(strProgram, vbNormalFocus)
                            intExit = WaitOnProgram(dblProgID, True)


                            'Open (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                            __FileName = (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat")
                            FileOpen(2, __FileName, OpenMode.Output, OpenAccess.Default, OpenShare.Shared)


                            'Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\palp000" & Now().ToString("dddd") - 1 & "0.zip" & Chr(34) & " -site MC_Of_AL -p " & Chr(34) & "/psyq0001/" & Chr(34)
                            __LineString = String.Empty
                            __LineString = Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\palp000" & Now().ToString("dddd") - 1 & "0.zip" & Chr(34) & " -site MC_Of_AL -p " & Chr(34) & "/psyq0001/" & Chr(34)
                            PrintLine(2, __LineString)

                            '  Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            __LineString = String.Empty
                            __LineString = "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            PrintLine(2, __LineString)

                            'Close #2 'Close the file
                            FileClose(2)
                            '************************************************************************************************************************************



                        Case 38        'Medicare Of Georga
                            '************************************************************************************************************************************

                            '               If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\pgap000" & Now().ToString("dddd") - 1 & ".clm") > "" Then Kill _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\pgap000" & Now().ToString("dddd") - 1 & ".clm"
                            '               If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\pgap000" & Now().ToString("dddd") - 1 & ".zip") > "" Then Kill _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\pgap000" & Now().ToString("dddd") - 1 & ".zip"
                            '               IO.File.Copy(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & _DB.IfNull(g_FileName, ""), _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\pgap000" & Now().ToString("dddd") - 1 & ".clm")
                            '               strProgram = Environ("WINZIP") & "\pkzip.exe " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\pgap000" & Now().ToString("dddd") - 1 & ".zip " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\pgap000" & Now().ToString("dddd") - 1 & ".clm"
                            '               dblProgID = Shell(strProgram, vbNormalFocus)
                            '               intExit = WaitOnProgram(dblProgID, True)
                            '               Open (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                            '               Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\pgap000" & Now().ToString("dddd") - 1 & "0.zip" & Chr(34) & " -site MC_Of_GA -p " & Chr(34) & "/gaf76888/" & Chr(34)
                            '               Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            '               Close #2 'Close the file
                            '************************************************************************************************************************************



                        Case 85        'Medicare Of Mississippi
                            '************************************************************************************************************************************
                            __FileName = String.Empty
                            __LineString = String.Empty


                            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\pmsp000" & Now().ToString("dddd") - 1 & ".clm") > "" Then
                                Kill(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\pmsp000" & Now().ToString("dddd") - 1 & ".clm")
                            End If

                            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\pmsp000" & Now().ToString("dddd") - 1 & ".zip") > "" Then
                                Kill(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\pmsp000" & Now().ToString("dddd") - 1 & ".zip")
                            End If

                            IO.File.Copy(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & _DB.IfNull(g_FileName, ""), _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\pmsp000" & Now().ToString("dddd") - 1 & ".clm")
                            strProgram = Environ("WINZIP") & "\pkzip.exe " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\pmsp000" & Now().ToString("dddd") - 1 & ".zip " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\pmsp000" & Now().ToString("dddd") - 1 & ".clm"
                            dblProgID = Shell(strProgram, vbNormalFocus)
                            intExit = WaitOnProgram(dblProgID, True)


                            'Open (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                            __FileName = (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat")
                            FileOpen(2, __FileName, OpenMode.Output, OpenAccess.Default, OpenShare.Shared)

                            ' Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\pmsp000" & Now().ToString("dddd") - 1 & "0.zip" & Chr(34) & " -site MC_Of_MS -p " & Chr(34) & "/m7021157/" & Chr(34)
                            __LineString = String.Empty
                            __LineString = Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\pmsp000" & Now().ToString("dddd") - 1 & "0.zip" & Chr(34) & " -site MC_Of_MS -p " & Chr(34) & "/m7021157/" & Chr(34)
                            PrintLine(2, __LineString)

                            'Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            __LineString = String.Empty
                            __LineString = "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            PrintLine(2, __LineString)


                            'Close #2 'Close the file
                            FileClose(2)
                            '************************************************************************************************************************************


                        Case 17        'Medicare Of Tennesse
                            '************************************************************************************************************************************
                            '               If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ptnp000" & Now().ToString("dddd") - 1 & ".clm") > "" Then Kill _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ptnp000" & Now().ToString("dddd") - 1 & ".clm"
                            ''              If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ptnp000" & Now().ToString("dddd") - 1 & ".zip") > "" Then Kill _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ptnp000" & Now().ToString("dddd") - 1 & ".zip"
                            '               IO.File.Copy(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & _DB.IfNull(g_FileName, ""), _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ptnp000" & Now().ToString("dddd") - 1 & ".clm")
                            '               strProgram = Environ("WINZIP") & "\pkzip.exe " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ptnp000" & Now().ToString("dddd") - 1 & ".zip " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ptnp000" & Now().ToString("dddd") - 1 & ".clm"
                            '               dblProgID = Shell(strProgram, vbNormalFocus)
                            '               intExit = WaitOnProgram(dblProgID, True)
                            '               Open (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                            '               Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ptnp000" & Now().ToString("dddd") - 1 & "0.zip" & Chr(34) & " -site MC_Of_TN -p " & Chr(34) & "/tn201312/" & Chr(34)
                            '               Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            '               Close #2 'Close the file
                            '************************************************************************************************************************************



                        Case 107              'BcBs of MT
                            '************************************************************************************************************************************
                            __FileName = String.Empty
                            __LineString = String.Empty

                            strFile = "37020" & Now().ToString("mmdd") & ".dap"
                            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & strFile) > "" Or Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\" & strFile) > "" Then
                                strFile = "37021" & Now().ToString("mmdd") & ".dap"
                            End If

                            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & strFile) > "" Or Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\" & strFile) > "" Then
                                strFile = "37022" & Now().ToString("mmdd") & ".dap"
                            End If

                            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & strFile) > "" Or Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\" & strFile) > "" Then
                                strFile = "37023" & Now().ToString("mmdd") & ".dap"
                            End If

                            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & strFile) > "" Or Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\" & strFile) > "" Then
                                strFile = "37024" & Now().ToString("mmdd") & ".dap"
                            End If

                            IO.File.Copy(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & _DB.IfNull(g_FileName, ""), _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & strFile)



                            'Open (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                            __FileName = (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat")
                            FileOpen(2, __FileName, OpenMode.Output, OpenAccess.Default, OpenShare.Shared)



                            'Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & strFile & Chr(34) & " -site BcBs_Of_MT -p " & Chr(34) & "/input/" & Chr(34)
                            __LineString = String.Empty
                            __LineString = Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & strFile & Chr(34) & " -site BcBs_Of_MT -p " & Chr(34) & "/input/" & Chr(34)
                            PrintLine(2, __LineString)



                            'Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            __LineString = String.Empty
                            __LineString = "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            PrintLine(2, __LineString)


                            'Close #2 'Close the file
                            FileClose(2)
                            '************************************************************************************************************************************





                        Case 126              'WestHighLands
                            '************************************************************************************************************************************
                            '                strFile = VB6.Format(Now, "mmddYY") & "_CL008"
                            '                strProgram = Environ("WINZIP") & "\pkzip.exe " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & strFile & ".zip " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & strFile
                            '               dblProgID = Shell(strProgram, vbNormalFocus)
                            '                intExit = WaitOnProgram(dblProgID, True)
                            '                IO.File.Copy(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & _DB.IfNull(g_FileName, ""), _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & strFile)
                            '                Open (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                            '                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & strFile & Chr(34) & " -site WestHighLands -p /inbox/ "
                            '                Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            '                Close #2 'Close the file
                            '************************************************************************************************************************************



                        Case 127              'BcBs of LA
                            '************************************************************************************************************************************
                            '                Open (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                            '                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & " -site BcBs_Of_LA -p /P0013469/ "
                            '               Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/P0013469/BCT*.OUT" & Chr(34) & " -site BcBs_Of_LA -p " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Chr(34) & " -delsrc -o "
                            '                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/P0013469/BC9*.OUT" & Chr(34) & " -site BcBs_Of_LA -p " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Chr(34) & " -delsrc -o "
                            '                Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/P0013469/BCA*.OUT" & Chr(34) & " -site BcBs_Of_LA -p " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Chr(34) & " -delsrc -o "
                            '                Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            '                Close #2 'Close the file
                            '************************************************************************************************************************************



                        Case 129              'BcBs of MA
                            '************************************************************************************************************************************
                            __FileName = String.Empty
                            __LineString = String.Empty

                            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & LeftVB6(_DB.IfNull(g_FileName, ""), InStr(1, _DB.IfNull(g_FileName, ""), ".")) & "837") > "" Then
                                Kill(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & LeftVB6(_DB.IfNull(g_FileName, ""), InStr(1, _DB.IfNull(g_FileName, ""), ".")) & "837")
                            End If

                            IO.File.Copy(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName, _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & LeftVB6(_DB.IfNull(g_FileName, ""), InStr(1, _DB.IfNull(g_FileName, ""), ".")) & "837")


                            ' Open (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                            __FileName = (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat")
                            FileOpen(2, __FileName, OpenMode.Output, OpenAccess.Default, OpenShare.Shared)


                            'Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Left(_DB.IfNull(g_FileName, ""), InStr(1, _DB.IfNull(g_FileName, ""), ".")) & "837" & Chr(34) & " -site BcBs_Of_MA -p " & Chr(34) & "/e__company_tradingpartners_EFE_C3BS/Inbound/" & Chr(34)
                            __LineString = String.Empty
                            __LineString = Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & LeftVB6(_DB.IfNull(g_FileName, ""), InStr(1, _DB.IfNull(g_FileName, ""), ".")) & "837" & Chr(34) & " -site BcBs_Of_MA -p " & Chr(34) & "/e__company_tradingpartners_EFE_C3BS/Inbound/" & Chr(34)
                            PrintLine(2, __LineString)

                            'Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/e__company_tradingpartners_EFE_C3BS/Outbound/*.*" & Chr(34) & " -site BcBs_Of_MA -p " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Chr(34) & " -delsrc -o "
                            __LineString = String.Empty
                            __LineString = Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/e__company_tradingpartners_EFE_C3BS/Outbound/*.*" & Chr(34) & " -site BcBs_Of_MA -p " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Chr(34) & " -delsrc -o "
                            PrintLine(2, __LineString)


                            ' Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            __LineString = String.Empty
                            __LineString = "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            PrintLine(2, __LineString)

                            'Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            __LineString = String.Empty
                            __LineString = "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            PrintLine(2, __LineString)

                            'Close #2 'Close the file
                            FileClose(2)
                            '************************************************************************************************************************************




                        Case 140
                            '************************************************************************************************************************************
                            __FileName = String.Empty
                            __LineString = String.Empty

                            '     Open (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                            __FileName = (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat")
                            FileOpen(2, __FileName, OpenMode.Output, OpenAccess.Default, OpenShare.Shared)


                            'Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Now().ToString("MM")& "*" & Now().ToString("yy") & "*.txt" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa -log " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\COREFTP.LOG"
                            __LineString = String.Empty
                            __LineString = "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Now().ToString("MM") & "*" & Now().ToString("yy") & "*.txt" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa -log " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\COREFTP.LOG"
                            PrintLine(2, __LineString)

                            If rstClrHse.Fields("fld270").Value = "Y" Then
                                'Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Now().ToString("MM")& "*" & Now().ToString("yy") & "*.270" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa -log " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\COREFTP.LOG"
                                __LineString = String.Empty
                                __LineString = Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Now().ToString("MM") & "*" & Now().ToString("yy") & "*.270" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa -log " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\COREFTP.LOG"
                                PrintLine(2, __LineString)
                            End If


                            ' Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            __LineString = String.Empty
                            __LineString = "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            PrintLine(2, __LineString)

                            'Close #2 'Close the file
                            FileClose(2)
                            '************************************************************************************************************************************




                        Case 153       'Trillium
                            '************************************************************************************************************************************
                            __FileName = String.Empty
                            __LineString = String.Empty

                            'Open (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                            __FileName = (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat")
                            FileOpen(2, __FileName, OpenMode.Output, OpenAccess.Default, OpenShare.Shared)

                            ' Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & " -site TrilliumCH_In -p  / "
                            __LineString = String.Empty
                            __LineString = Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & g_FileName & Chr(34) & " -site TrilliumCH_In -p  / "
                            PrintLine(2, __LineString)


                            ' Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/*.*" & Chr(34) & " -site TrilliumCH_Out -p " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Chr(34) & " -delsrc -o "
                            __LineString = String.Empty
                            __LineString = Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/*.*" & Chr(34) & " -site TrilliumCH_Out -p " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Chr(34) & " -delsrc -o "
                            PrintLine(2, __LineString)

                            'Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            __LineString = String.Empty
                            __LineString = "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            PrintLine(2, __LineString)

                            ' Close #2 'Close the file
                            FileClose(2)
                            '*************************************




                            'objFolder = GetFolder(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\")
                            'objFile = objFolder.Files


                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************




                            'For Each objFile In objFolder.Files
                            '    If Len(objFile.Name) = 16 And IsNumeric(Left(objFile.Name, 12)) And Right(objFile.Name, 4) = ".txt" Then
                            '        objText = objFile.OpenAsTextStream(ForReading)

                            '        Do While objText.AtEndOfStream = False
                            '            strLine = objText.ReadLine

                            '            If InStr(1, strLine, "NM1*85") Then

                            '                If InStr(1, strLine, "1457696262") Then
                            '                End If

                            '                If InStr(1, strLine, "1205070489") Then
                            '                    '*************************************
                            '                    __FileName = String.Empty
                            '                    __LineString = String.Empty

                            '                    'Open (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                            '                    __FileName = (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat")
                            '                    FileOpen(2, __FileName, OpenMode.Output, OpenAccess.Default, OpenShare.Shared)


                            '                    __LineString = String.Empty
                            '                    __LineString = Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & objFile.Name & Chr(34) & " -site TrilliumGP_In -p  / "
                            '                    PrintLine(2, __LineString)

                            '                    '   Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/*.*" & Chr(34) & " -site TrilliumGP_Out -p " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Chr(34) & " -delsrc -o "
                            '                    __LineString = String.Empty
                            '                    __LineString = Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -d " & Chr(34) & "/*.*" & Chr(34) & " -site TrilliumGP_Out -p " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Chr(34) & " -delsrc -o "
                            '                    PrintLine(2, __LineString)

                            '                    '  Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            '                    __LineString = String.Empty
                            '                    __LineString = "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            '                    PrintLine(2, __LineString)

                            '                    'Close #2 'Close the file
                            '                    FileClose(2)
                            '                    '*************************************
                            '                End If
                            '                Exit Do
                            '            End If
                            '        Loop
                            '    End If
                            'Next
                            'objFile = Nothing
                            'objFolder = Nothing
                            ''************************************************************************************************************************************






                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************
                            '************************************************************************************************************************************

























                        Case 159        'TricareEast
                            '************************************************************************************************************************************
                            __FileName = String.Empty
                            __LineString = String.Empty

                            '   Open (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                            __FileName = (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat")
                            FileOpen(2, __FileName, OpenMode.Output, OpenAccess.Default, OpenShare.Shared)


                            '  Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & objFile.Name & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa -log " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\COREFTP.LOG"
                            __LineString = String.Empty
                            '    __LineString = Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & objFile.Name & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa -log " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\COREFTP.LOG"
                            PrintLine(2, __LineString)



                            If rstClrHse.Fields("fld270").Value = "Y" Then
                                'Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Now().ToString("MM")& "*" & Now().ToString("yy") & "*.270" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa -log " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\COREFTP.LOG"
                                __LineString = String.Empty
                                __LineString = Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Now().ToString("MM") & "*" & Now().ToString("yy") & "*.270" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa -log " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\COREFTP.LOG"
                                PrintLine(2, __LineString)
                            End If



                            ' Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            __LineString = String.Empty
                            __LineString = "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                            PrintLine(2, __LineString)


                            'Close #2 'Close the file
                            FileClose(2)
                            '************************************************************************************************************************************




                        Case Else
                            '************************************************************************************************************************************
                            __FileName = String.Empty
                            __LineString = String.Empty

                            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ElecClaim.txt") > "" Then
                                Kill(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ElecClaim.txt")
                            End If

                            IO.File.Copy(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & _DB.IfNull(g_FileName, ""), _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ElecClaim.txt")


                            'Open (_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\filename.txt") For Output As #2
                            __FileName = (_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\filename.txt")
                            FileOpen(2, __FileName, OpenMode.Output, OpenAccess.Default, OpenShare.Shared)



                            'Print #2, _DB.IfNull(g_FileName, "")
                            __LineString = String.Empty
                            __LineString = _DB.IfNull(g_FileName, "")
                            PrintLine(2, __LineString)


                            'Close #2 'Close the file
                            FileClose(2)
                            '************************************************************************************************************************************



                    End Select
                    '************************************************************************************************************************************
                    '************************************************************************************************************************************
                    '************************************************************************************************************************************

                    If Dir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") > "" Then        'ThinEDI uses FTP
                        strProgram = _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat"
                    Else
                        strProgram = Environ("PROCOMM") & "\pw5.exe " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ZSEND32.wax"
                    End If

                    dblProgID = Shell(strProgram, vbNormalFocus)
                    intExit = WaitOnProgram(dblProgID, True)









                    'Move submission files to Submit folder
                    If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt") > "" Then
                        Select Case rstClrHse.Fields("fldClearingHouseID").Value
                            Case 1 To 11, 13, 15, 17 To 24, 26 To 30, 32 To 51, 54, 56 To 59, 62, 63, 66, 68 To 72, 74, 75, 84 To 91, 93 To 103, 110, 111, 112, 118 To 123, 124, 125, 127, 131 To 134, 136, 138, 139, 142 To 146, 149 To 151, 154 To 159
                                If Dir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.271*") > "" Then
                                    System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.271*", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\271\")
                                End If
                                If Dir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\271_*") > "" Then
                                    System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\271_*", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\271\")
                                End If
                                If Dir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\277_*") > "" Then
                                    System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\277_*", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\277\")
                                End If
                                If Dir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Now().ToString("MM") & "*" & Now().ToString("yy") & "*.270*") > "" Then
                                    System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Now().ToString("MM") & "*" & Now().ToString("yy") & "*.270*", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\270\")
                                End If
                                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*" & Now().ToString("yy") & "*.txt", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                                If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*" & Now().ToString("yy") & "*.837i") > "" Then
                                    System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*" & Now().ToString("yy") & "*.837i", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                                End If
                                If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt") > "" Then
                                    Kill(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt")
                                End If
                            Case 52        'BcBs Of TN has a different file name .X12
                                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*" & Now().ToString("yy") & "*.txt", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*" & Now().ToString("yy") & "*.X12", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                                If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt") > "" Then
                                    Kill(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt")
                                End If
                            Case 64        'Medicaid NE has a different file name EDI000000265.837P.0001.DAT
                                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*" & Now().ToString("yy") & "*.txt", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                                If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\EDI000000265.837P." & Now().ToString("HH") & Now().ToString("dd") & ".DAT") > "" Then
                                    Kill(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\EDI000000265.837P." & Now().ToString("HH") & Now().ToString("dd") & ".DAT")
                                End If
                                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*" & Now().ToString("dd") & "*.DAT", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                                If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt") > "" Then
                                    Kill(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt")
                                End If
                            Case 92         'BcBs Of HA
                                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & _DB.IfNull(g_FileName, ""), _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & strFile, _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                                Kill(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt")
                            Case 140         'BcBs Of WA
                                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Now().ToString("MM") & "*" & Now().ToString("yy") & "*.txt", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                                If Dir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.271*") > "" Then
                                    System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.271*", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\271\")
                                End If
                                If Dir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.270*") > "" Then
                                    System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.270*", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\270\")
                                End If
                                If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt") > "" Then
                                    Kill(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt")
                                End If
                            Case Else
                                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & _DB.IfNull(g_FileName, ""), _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                                If Dir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.271*") > "" Then
                                    System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.271*", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\271\")
                                End If
                                If Dir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.270*") > "" Then
                                    System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.270*", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\270\")
                                End If
                                Kill(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt")
                                If rstClrHse.Fields("fldClearingHouseID").Value = 129 Then
                                    System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & LeftVB6(_DB.IfNull(g_FileName, ""), InStr(1, _DB.IfNull(g_FileName, ""), ".")) & "837", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                                End If
                        End Select

                        ' Send Email Message if Status Text File Failed
                    Else
                        SendEmailMessage("Claim EDI Failed", "Claims file not created for " & rstClrHse.Fields("fldClearingHouseID").Value & " - " & rstClrHse.Fields("fldFolder").Value & _DB.IfNull(g_FileName, ""))
                    End If

                    'Check FTP Log and verify claim files were sent
                    If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\COREFTP.LOG") > "" Then
                        If ReadFtpLog("COREFTP.LOG", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\") = "Fail" Then
                            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\COREFTP.LOG", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\LOG\COREFTP_" & Now().ToString("yyyymmdd") & Second(Now) & ".LOG")
                            SendEmailMessage("Coreftp failed", "Claims file not sent: " & rstClrHse.Fields("fldClearingHouseID").Value & " - " & rstClrHse.Fields("fldFolder").Value & _DB.IfNull(g_FileName, ""))
                        Else
                            Kill(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\COREFTP.LOG")
                        End If
                    End If
                    'End If

                End If

            End If

            'Find any missed files and transmit them
            If rstClrHse.Fields("fldClearingHouseID").Value = 92 Then
                objFolder = GetFolder(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\")
                'objFile = objFolder.Files

                'For Each objFile In objFolder.Files



                Dim files() As String = IO.Directory.GetFiles(objFolder)

                For Each objFile As String In files
                    ' Do work, example
                    'Dim text As String = IO.File.ReadAllText(File)
                    'Next





                    If Len(objFile) = 16 And IsNumeric(Mid(objFile, 1, 12)) And RightVB6(objFile, 4) = ".txt" Then

                        Select Case rstClrHse.Fields("fldClearingHouseID").Value

                            Case 92         'BcBs Of HA
                                '************************************************************************************************************************************

                                __FileName = String.Empty
                                __LineString = String.Empty


                                strFile = "ca" & LeftVB6(objFile, 10) & "ip.01"
                                If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & strFile) > "" Then
                                    strFile = "ca" & LeftVB6(objFile, 9) & "0ip.01"
                                End If

                                For intWrk = 0 To 9
                                    If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & strFile) > "" Or Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\" & strFile) > "" Then
                                        strFile = "ca" & LeftVB6(objFile, 9) & intWrk & "ip.01"
                                    End If

                                    If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & strFile) = "" And Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\" & strFile) = "" Then
                                        Exit For
                                    End If

                                Next


                                IO.File.Copy(objFile, _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & strFile)



                                ' Open (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") For Output As #2
                                __FileName = (_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat")
                                FileOpen(2, __FileName, OpenMode.Output, OpenAccess.Default, OpenShare.Shared)


                                'Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ca*ip.01" & Chr(34) & " -site BcBs_Of_HA -p / -o"
                                __LineString = String.Empty
                                __LineString = Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ca*ip.01" & Chr(34) & " -site BcBs_Of_HA -p / -o"
                                PrintLine(2, __LineString)
                                ' ''               If Dir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.270*") > "" Then
                                ' ''                       Print #2, Chr(34) & "C:\Program Files\CoreFTP\coreftp.exe" & Chr(34) & " -u " & Chr(34) & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\la*ip.01" & Chr(34) & " -site " & rstClrHse.Fields("fldSftpSite").Value & " -p " & Chr(34) & rstClrHse.Fields("fldSftpDestFolder").Value & Chr(34) & " -oa "
                                ' ''               End If



                                'Print #2, "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                                __LineString = String.Empty
                                __LineString = "echo " & Chr(34) & "Finished" & Chr(34) & " > " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\status.txt"
                                PrintLine(2, __LineString)

                                '  Close #2 'Close the file
                                FileClose(2)
                                '*************************************



                                If Dir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat") > "" Then
                                    strProgram = _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Claims.bat"
                                    dblProgID = Shell(strProgram, vbNormalFocus)
                                    intExit = WaitOnProgram(dblProgID, True)
                                End If

                                If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt") > "" Then
                                    System.IO.File.Move(objFile, _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                                    System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & strFile, _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
                                    Kill(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\status.txt")
                                End If

                                '************************************************************************************************************************************
                        End Select

                    End If

                Next

                objFolder = Nothing

            End If

            If _DB.IfNull(rstClrHse.Fields("fldRemittanceYN").Value, "") = "Y" And _
                 (Dir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ZRECV32.wax") > "" Or _
                  Dir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ERAs.bat") > "") Then
                '  If (VB6.Format(Now, "w") = 2 Or VB6.Format(Now, "w") = 4 Or VB6.Format(Now, "w") = 6) And Hour(Now) < 12 Then '2-Monday, 4-Wednesday, 6-friday and in the AM
                '  If Hour(Now) < 12 Or rstClrHse.Fields("fldClearingHouseID").Value = 23 Then 'Run in the AM

                If Dir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ERAs.bat") > "" Then        'ThinEDI uses FTP
                    strProgram = _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ERAs.bat"
                Else
                    strProgram = Environ("PROCOMM") & "\pw5.exe " & _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ZRECV32.wax"
                End If

                dblProgID = Shell(strProgram, vbNormalFocus)

                If rstClrHse.Fields("fldClearingHouseID").Value <> 23 Then
                    intExit = WaitOnProgram(dblProgID, True)
                End If
                '  End If
            End If

            If rstClrHse.Fields("fldClearingHouseID").Value = 50 Or _
                     rstClrHse.Fields("fldClearingHouseID").Value = 56 Or _
                     rstClrHse.Fields("fldClearingHouseID").Value = 64 Or _
                     rstClrHse.Fields("fldClearingHouseID").Value = 86 Or _
                     rstClrHse.Fields("fldClearingHouseID").Value = 92 Or _
                     rstClrHse.Fields("fldClearingHouseID").Value = 104 Or _
                     rstClrHse.Fields("fldClearingHouseID").Value = 105 Or _
                     rstClrHse.Fields("fldClearingHouseID").Value = 110 Or _
                     rstClrHse.Fields("fldClearingHouseID").Value = 111 Or _
                     rstClrHse.Fields("fldClearingHouseID").Value = 116 Or _
                     rstClrHse.Fields("fldClearingHouseID").Value = 131 Or _
                     rstClrHse.Fields("fldClearingHouseID").Value = 132 Or _
                     rstClrHse.Fields("fldClearingHouseID").Value = 133 Or _
                     rstClrHse.Fields("fldClearingHouseID").Value = 136 Or _
                     rstClrHse.Fields("fldClearingHouseID").Value = 151 Then
                If Dir(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Unpack.bat") > "" Then
                    strProgram = _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\Unpack.bat"
                    dblProgID = Shell(strProgram, vbNormalFocus)
                    intExit = WaitOnProgram(dblProgID, True)
                End If
            End If

            If rstClrHse.Fields("fldClearingHouseID").Value <> 48 Then
                MoveFiles(rstClrHse, CLng(rstClrHse.Fields("fldClearingHouseID").Value))
            End If


            txtStatus.Text = "Complete: " & lngClrHseID & " Folder: " & rstClrHse.Fields("fldFolder").Value & "/" & g_FileName & " Claims: " ' & barStatus.Max & vbCrLf & txtStatus.Text
            'DoEvents()

            rstClrHse.MoveNext()

        Loop

        rstClrHse.Close()
        rstClrHse = Nothing
        '   'Screen.MousePointer  = vbDefault

        Exit Sub

        'ErrTrap:
        '        txtStatus.Text = "Error:" & Err.Description & vbLf & vbLf & "Error Number: " & Err.Number & vbCrLf & txtStatus.Text
        '        'DoEvents()
        '        ' ShowError(Err)
        '        If blnEmailFailure Then
        '            SendEmailMessage("Electronic Claims Failure", "The Electronic Claims Daemon reported a failure in " & Err.Source & "." & vbCrLf & "Unhandled Error: " & Err.Number & ": " & Err.Description & vbCrLf & Me.txtStatus.Text)
        '        End If
        '        objEDI = Nothing
        '        rstClrHse = Nothing
        ' 'Screen.MousePointer  = vbDefault
        ' Unload(Me)
    End Sub

    ''' <summary>
    ''' Opens a file to look for the word Fail
    ''' </summary>
    ''' <param name="strFileName"></param>
    ''' <param name="curDir"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ReadFtpLog(ByVal strFileName As String, ByVal curDir As String) As String


        Dim r As String = "Fail"
        Dim LogFileLine As String = String.Empty
        Dim LogFileFQD As String = String.Empty
        'Dim Complete As Boolean = False
        'Dim Processed As Boolean = False

        'Screen.MousePointer  = vbHourglass

        'Dim objFS As Scripting.FileSystemObject
        'Dim objFile As Scripting.File
        'Dim objText As Scripting.TextStream


        ' Dim lngCtr, intPosStart, intPosEnd As Long

        'Dim strStatus, strMsg, strMsgCode As String
        '  Dim Cnt As Integer
        '   Dim intChr, intBefore, intAfter As Long

        'Const ForReading = 1, ForWriting = 2, ForAppending = 3


        ' On Error GoTo ErrTrap

        'objFS = CreateObject("Scripting.FileSystemObject")
        'objFile = objFS.GetFile(curDir & strFileName)
        'objText = objFile.OpenAsTextStream(ForReading)
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        ' fix 2

        Try



            LogFileLine = String.Empty
            LogFileFQD = String.Empty


            LogFileFQD = curDir + strFileName



            Using reader As StreamReader = New StreamReader(LogFileFQD)

                LogFileLine = reader.ReadToEnd

                If InStr(1, LogFileLine, "SFTP connection error") > 0 Then
                    r = "Fail"
                    '        Exit Function
                Else
                    r = True
                End If



            End Using




        Catch ex As Exception
            r = "Fail"
        End Try



        'strLine = objText.ReadLine
        'Do While Not objText.AtEndOfStream ' objText.AtEndOfStream    ' Loop until end of file.
        '    '  Debug.Print strLine    ' Print to the Immediate window.
        '    If InStr(1, strLine, "SFTP connection error") > 0 Then
        '        r = "Fail"
        '        Exit Function
        '    End If
        '    strLine = objText.ReadLine
        'Loop
        ''   Close objFile    ' Close file.
        'ReadFtpLog = "Pass"

        '        Exit Function
        'ErrTrap:
        '        MsgBox("Error: " & curDir & " : " & strFileName, vbOKOnly, "Error Message")
        '        'Screen.MousePointer  = vbDefault
        '        ShowError(Err)
    End Function






    Public Sub MoveFiles(ByVal rstClrHse As ADODB.Recordset, ByVal intClrHse As Integer)
        'Moves Response File from App folder to App\Response folder

        Dim MyChar As String
        Dim I, cntWrk As Long
        Dim curDir, curFile As String
        Dim inFile As String
        Dim strLine As String
        Dim dblProgID As Double
        '    Dim objFS As New System.IO.File
        Const ForReading = 1, ForWriting = 2, ForAppending = 3
        Dim objFolder, objFile
        '   Dim objText As Scripting.TextStream

        On Error Resume Next
        'On Error GoTo ErrTrap:

        '     objFS = CreateObject("Scripting.FileSystemObject")


        '  System.IO.File.Move(_Success + FileName, _Success + FileName + "_____" + Convert.ToString(_FILE_ID) + ".dupe")



        ' System.IO.File.Delete()






        If rstClrHse.Fields("fldClearingHouseID").Value = 1 Or _
           rstClrHse.Fields("fldClearingHouseID").Value = 35 Or _
           rstClrHse.Fields("fldClearingHouseID").Value = 62 Then        'Thin, BCBS of OR, BcBs of FL
            '    System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.era", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.997", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.999", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.PCC", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\PCC\")
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.ECC", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ECC\")
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.SFC", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\SFC\")
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.WPS", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\WPS\")
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.PDF", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\PDF\")
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.EPS*", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\EPS\")
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.270.*success", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\270\")
            'Delete HyperTerminal trace files
            System.IO.File.Delete(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.dbg")
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*success", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
        End If

        If rstClrHse.Fields("fldClearingHouseID").Value = 22 Or _
           rstClrHse.Fields("fldClearingHouseID").Value = 84 Or _
           rstClrHse.Fields("fldClearingHouseID").Value = 124 Then  'Tricare West, TriWest, BcBs of SC
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.RPT", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\RPT\")
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.RPT.*", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\RPT\")
            '      System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.X12", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
        End If

        If rstClrHse.Fields("fldClearingHouseID").Value = 66 Then        'BCBS of RI
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\835_*", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\997_*", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\999_*", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\271_*", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\271\")
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\277_*", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\277\")
        End If

        If rstClrHse.Fields("fldClearingHouseID").Value = 54 Or _
           rstClrHse.Fields("fldClearingHouseID").Value = 75 Then 'Medicare NCA SCA OH SC
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\997*.RSP", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\999*.RSP", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
        End If

        If rstClrHse.Fields("fldClearingHouseID").Value = 109 Then  'BcBs of Iowa
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.Z16", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\")
        End If

        If rstClrHse.Fields("fldClearingHouseID").Value = 150 Then  'ClaimMD
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.txt.result", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\")
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.270.result", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\270\")
        End If

        If rstClrHse.Fields("fldClearingHouseID").Value = 134 Then  'GateWay EDI
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.DAT", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\")
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.999", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
        End If

        If rstClrHse.Fields("fldClearingHouseID").Value = 25 Then        'BcBs of NC
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\P_O_CEXACK") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\P_O_CEXACK", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\" & Now().ToString("yyyymmdd") & Hour(Now) & ".ACK")
            End If
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\P_O_CE0689") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\P_O_CE0689", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Now().ToString("yyyymmdd") & Hour(Now) & ".RSP")
            End If
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\P_O_CE835R") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\P_O_CE835R", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Now().ToString("yyyymmdd") & Hour(Now) & ".835")
            End If
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\P_O_CE0689_HIP_FTP_AUDIT") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\P_O_CE0689_HIP_FTP_AUDIT", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Now().ToString("yyyymmdd") & Hour(Now) & ".RSP")
            End If
        End If

        If rstClrHse.Fields("fldClearingHouseID").Value = 127 Then        'BcBs of LA
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\BCTA1.out") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\BCTA1.out", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\" & Now().ToString("yyyymmdd") & Hour(Now) & ".TA1")
            End If
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\BC999.out") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\BC999.out", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\" & Now().ToString("yyyymmdd") & Hour(Now) & ".ACK")
            End If
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\BCAccNotAccRep.out") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\BCAccNotAccRep.out", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Now().ToString("yyyymmdd") & Second(Now) & ".RSP")
            End If
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\BC5010835.out") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\BC5010835.out", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & Now().ToString("yyyymmdd") & Second(Now) & ".835")
            End If
        End If

        If rstClrHse.Fields("fldClearingHouseID").Value = 31 Then        'BcBs of MS
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179GPM\999PBLUEP") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179GPM\999PBLUEP", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\999PBLUEP-" & Now().ToString("yyyymmdd") & Second(Now) & ".ACK")
            End If
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179GPM\HCFAREPORT") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179GPM\HCFAREPORT", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\HCFAREPORT-" & Now().ToString("yyyymmdd") & Second(Now) & ".RSP")
            End If
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179GPM\999PCLMSP") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179GPM\999PCLMSP", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\999PCLMSP-" & Now().ToString("yyyymmdd") & Second(Now) & ".ACK")
            End If
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179GPM\RPTPCLMSP") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179GPM\RPTPCLMSP", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\RSP\RPTPCLMSP-" & Now().ToString("yyyymmdd") & Second(Now) & ".RSP")
            End If
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179GPM\5010835SHIELD") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179GPM\5010835SHIELD", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\835\5010835SHIELD-" & Now().ToString("yyyymmdd") & Second(Now) & ".835")
            End If
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179GPM\277CAPCLMSP") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179GPM\277CAPCLMSP", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\277\277CAPCLMSP-" & Now().ToString("yyyymmdd") & Second(Now) & ".277")
            End If


            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179ISF\999PBLUEP") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179ISF\999PBLUEP", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\999PBLUEP-" & Now().ToString("yyyymmdd") & Second(Now) & ".ACK")
            End If
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179ISF\HCFAREPORT") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179ISF\HCFAREPORT", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\HCFAREPORT-" & Now().ToString("yyyymmdd") & Second(Now) & ".RSP")
            End If
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179ISF\999PCLMSP") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179ISF\999PCLMSP", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\999PCLMSP-" & Now().ToString("yyyymmdd") & Second(Now) & ".ACK")
            End If
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179ISF\RPTPCLMSP") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179ISF\RPTPCLMSP", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\RSP\RPTPCLMSP-" & Now().ToString("yyyymmdd") & Second(Now) & ".RSP")
            End If
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179ISF\5010835SHIELD") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179ISF\5010835SHIELD", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\835\5010835SHIELD-" & Now().ToString("yyyymmdd") & Second(Now) & ".835")
            End If
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179ISF\277CAPCLMSP") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179ISF\277CAPCLMSP", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\277\277CAPCLMSP-" & Now().ToString("yyyymmdd") & Second(Now) & ".277")
            End If


            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179IUL\999PBLUEP") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179IUL\999PBLUEP", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\999PBLUEP-" & Now().ToString("yyyymmdd") & Second(Now) & ".ACK")
            End If
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179IUL\HCFAREPORT") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179IUL\HCFAREPORT", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\HCFAREPORT-" & Now().ToString("yyyymmdd") & Second(Now) & ".RSP")
            End If
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179IUL\999PCLMSP") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179IUL\999PCLMSP", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\999PCLMSP-" & Now().ToString("yyyymmdd") & Second(Now) & ".ACK")
            End If
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179IUL\RPTPCLMSP") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179IUL\RPTPCLMSP", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\RSP\RPTPCLMSP-" & Now().ToString("yyyymmdd") & Second(Now) & ".RSP")
            End If
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179IUL\5010835SHIELD") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179IUL\5010835SHIELD", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\835\5010835SHIELD-" & Now().ToString("yyyymmdd") & Second(Now) & ".835")
            End If
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\S2179IUL\277CAPCLMSP") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\S2179IUL\277CAPCLMSP", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\277\277CAPCLMSP-" & Now().ToString("yyyymmdd") & Second(Now) & ".277")
            End If

        End If

        If rstClrHse.Fields("fldClearingHouseID").Value = 68 Then        'BcBs of MI
            objFolder = GetFolder(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\")
            objFile = objFolder.Files

            For Each objFile In objFolder.Files
                inFile = objFile.Name
                curFile = objFile

                If inFile > "" And (InStr(1, inFile, "835P5010") > 0) Then
                    System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\835\" & LeftVB6(inFile, InStr(1, inFile, "-") - 1) & ".835")
                End If

                If inFile > "" And (InStr(1, inFile, "R277CAA") > 0 Or InStr(1, inFile, "R277CAF") > 0 Or InStr(1, inFile, "R277CAK") > 0) Then
                    System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\" & LeftVB6(inFile, InStr(1, inFile, "-") - 1) & ".rsp")
                End If

                If inFile > "" And (InStr(1, inFile, "999P") > 0) Then
                    System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\" & LeftVB6(inFile, InStr(1, inFile, "-") - 1) & ".dat")
                End If
                System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\c0iej*.txt", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\")
                '        System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\835P5010*.A", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
                '        System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\R277CAA*.A", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\")
                '        System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\R277CAF*.A", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\")
                '        System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\R277CAK*.A", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\")
                '        System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\999P*.*", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")

            Next

            objFile = Nothing
            objFolder = Nothing
        End If


        If rstClrHse.Fields("fldFolder").Value = "eSolutions" Or _
               rstClrHse.Fields("fldFolder").Value = "ECC-000600076" Or _
               rstClrHse.Fields("fldFolder").Value = "ECC-BBB33651B" Or _
               rstClrHse.Fields("fldFolder").Value = "ECC-BS00719" Or _
               rstClrHse.Fields("fldFolder").Value = "ECC-BS01000189" Then      'Noridian



            objFolder = GetFolder(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\")


            objFile = objFolder.Files

            For Each objFile In objFolder.Files
                inFile = objFile.Name
                curFile = objFile

                inFile = Replace(inFile, "~#", "")
                inFile = Replace(inFile, "#~", "_")

                If inFile > "" And (RightVB6(inFile, 4) = ".277" Or RightVB6(inFile, 6) = ".277CA" Or InStr(1, inFile, "277CA") > 0) Then
                    System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\277\" & inFile)
                End If

                If inFile > "" And (RightVB6(inFile, 4) = ".999" Or InStr(1, inFile, "999.") > 0) Then
                    inFile = LeftVB6(inFile, InStr(1, inFile, ".txt.20") + 3)
                    System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\" & Replace(Replace(inFile, ".", "_"), "_txt", ".txt"))
                End If

                If inFile > "" And (RightVB6(inFile, 4) = ".TRN" Or InStr(1, inFile, "TRN.") > 0) Then
                    System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\" & inFile)
                End If

                If inFile > "" And RightVB6(inFile, 8) = "_835.edi" And InStr(1, inFile, "-") > 0 Then   'Noridian
                    System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\835\" & RightVB6(inFile, Len(inFile) - InStr(1, inFile, "-") - 1))
                End If

                If inFile > "" And RightVB6(inFile, 7) = "835.dat" Then
                    System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\835\" & LeftVB6(inFile, 28) & ".835")
                End If

                If inFile > "" And (InStr(1, inFile, "835Ansi") > 0 Or InStr(1, inFile, "5010835") > 0) Then
                    System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\835\" & inFile)
                End If

                If inFile > "" And RightVB6(inFile, 8) = "_TA1.edi" And InStr(1, inFile, ".txt") > 0 And Len(inFile) > 24 Then   'Noridian
                    '78f97a12bac84050_abe33289790e1a63_100819429376.txt_TA1.edi
                    System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\" & RightVB6(inFile, 24))
                End If
                If inFile > "" And RightVB6(inFile, 8) = "_999.edi" And InStr(1, inFile, "_") > 0 And Len(inFile) > 20 Then   'Noridian
                    'a8c6b39b0dbf448f_b937bdbbbab0cae4_100819429376_999.edi
                    System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\" & RightVB6(inFile, 20))
                End If

            Next

            objFile = Nothing
            objFolder = Nothing

        End If

        If rstClrHse.Fields("fldFolder").Value = "WPS" Then       'WPS, TricareEast
            objFolder = GetFolder(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\")
            objFile = objFolder.Files
            curDir = _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\"

            For Each objFile In objFolder.Files
                inFile = objFile.Name
                curFile = objFile

                If inFile > "" And (RightVB6(inFile, 4) = ".277" Or RightVB6(inFile, 6) = ".277CA" Or InStr(1, inFile, "277CA") > 0) Then
                    System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\277\" & inFile)
                End If

                If inFile > "" And (RightVB6(inFile, 4) = ".999" Or RightVB6(inFile, 6) = ".999.dat" Or InStr(1, inFile, "999.") > 0) Then
                    System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\" & inFile)
                End If

                If inFile > "" And RightVB6(inFile, 4) = ".dat" And InStr(1, inFile, "5010-835") > 0 And InStr(1, inFile, ".P.") > 0 Then
                    inFile = Replace(inFile, ".", "-")
                    '   inFile = Replace(inFile, "-dat", ".dat")
                    inFile = LeftVB6(inFile, InStr(1, inFile, "-P-")) & ".dat"

                    inFile = Replace(inFile, "-.dat", ".dat")
                    '  strFileName = _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name
                    '   Set objFS = New Scripting.FileSystemObject
                    '   curFile = GetFileName(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name)
                    If System.IO.File.Exists(curDir & "835\" & Mid(objFile.Name, 12)) Then System.IO.File.Delete(curDir & "835\" & Mid(objFile.Name, 12))
                    If System.IO.File.Exists(curDir & "835\" & inFile) Then System.IO.File.Delete(curDir & "835\" & inFile)
                    System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name, curDir & "835\" & inFile)
                End If

            Next

            objFile = Nothing
            objFolder = Nothing

        End If

        If rstClrHse.Fields("fldClearingHouseID").Value = 92 Or _
           rstClrHse.Fields("fldClearingHouseID").Value = 129 Then  'BcBs of HA, BcBs of MA
            objFolder = GetFolder(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\")
            objFile = objFolder.Files

            For Each objFile In objFolder.Files
                inFile = objFile.Name
                curFile = objFile
                If inFile > "" And LeftVB6(inFile, 2) = "ca" And RightVB6(inFile, 5) = "op.01" Then   'ca*op.01
                    If System.IO.File.Exists(curDir & "SUB\" & inFile) Then System.IO.File.Delete(curDir & "SUB\" & inFile)
                    System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\" & inFile)
                End If
                If inFile > "" And LeftVB6(inFile, 2) = "jx" And RightVB6(inFile, 5) = "op.01" Then   'jx*op.01
                    If System.IO.File.Exists(curDir & "RSP\" & inFile) Then System.IO.File.Delete(curDir & "RSP\" & inFile)
                    System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\" & inFile)
                End If
                If inFile > "" And LeftVB6(inFile, 2) = "jx" And RightVB6(inFile, 5) = "op.02" Then   'jx*op.02
                    If System.IO.File.Exists(curDir & "RSP\" & inFile) Then System.IO.File.Delete(curDir & "RSP\" & inFile)
                    System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\" & inFile)
                End If
                If inFile > "" And LeftVB6(inFile, 2) = "gx" And RightVB6(inFile, 5) = "op.01" Then   'gx*op.01
                    If System.IO.File.Exists(curDir & "RSP\" & inFile) Then System.IO.File.Delete(curDir & "RSP\" & inFile)
                    System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\" & inFile)
                End If
                If inFile > "" And LeftVB6(inFile, 2) = "gx" And RightVB6(inFile, 5) = "op.02" Then   'gx*op.02
                    If System.IO.File.Exists(curDir & "RSP\" & inFile) Then System.IO.File.Delete(curDir & "RSP\" & inFile)
                    System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\" & inFile)
                End If
                If inFile > "" And LeftVB6(inFile, 6) = "BCBSMA" And RightVB6(inFile, 3) = "PDF" Then
                    If System.IO.File.Exists(curDir & "RSP\" & inFile) Then System.IO.File.Delete(curDir & "RSP\" & inFile)
                    System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\" & inFile)
                End If
            Next

            objFile = Nothing
            objFolder = Nothing
        End If

        If rstClrHse.Fields("fldClearingHouseID").Value = 92 Then  'BcBs of HA
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ca*ip.01", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\")
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\la*op.01", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\271\")
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\sa*op.01", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\277\")
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\rb*op.01", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\rf*op.01", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\rq*op.01", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\rr*op.01", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\jx*op.01", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\")
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\gx*op.01", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\")
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\aa*op.01", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
        End If

        If rstClrHse.Fields("fldClearingHouseID").Value = 2 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 3 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 4 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 5 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 6 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 7 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 8 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 9 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 10 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 12 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 14 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 27 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 33 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 44 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 45 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 49 Or _
            rstClrHse.Fields("fldClearingHouseID").Value = 59 Then  'Anthem


            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\277*") > "" Then
                'objFS = New Scripting.FileSystemObject
                objFolder = GetFolder(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\")
                objFile = objFolder.Files


                For Each objFile In objFolder.Files
                    curDir = _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\"
                    '  strFileName = _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name
                    If LeftVB6(objFile.Name, 3) = "277" Then
                        '    objFS = New Scripting.FileSystemObject
                        curFile = GetFileName(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name)
                        If System.IO.File.Exists(curDir & "277\" & objFile.Name) Then System.IO.File.Delete(curDir & "277\" & objFile.Name)
                        System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\277\")
                    End If
                Next
                'objFS = Nothing
            End If




            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.835") > "" Then
                ' objFS = New Scripting.FileSystemObject
                objFolder = GetFolder(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\")
                objFile = objFolder.Files
                For Each objFile In objFolder.Files
                    curDir = _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\"
                    '  strFileName = _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name
                    If RightVB6(objFile.Name, 4) = ".835" And LeftVB6(objFile.Name, 2) = "HP" Then
                        ' objFS = New Scripting.FileSystemObject
                        curFile = GetFileName(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name)
                        If System.IO.File.Exists(curDir & "835\" & objFile.Name) Then System.IO.File.Delete(curDir & "835\" & objFile.Name)
                        System.IO.File.Move(curDir & objFile.Name, curDir & "835\")
                        If System.IO.File.Exists(curDir & "835\Archive\" & objFile.Name) Then
                            System.IO.File.Delete(curDir & "835\Archive\" & objFile.Name)
                            System.IO.File.Move(curDir & "835\" & objFile.Name, curDir & "835\Archive\")
                        End If
                    End If
                Next
                'objFS = Nothing
            End If




            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.270") > "" Then
                '  objFS = New Scripting.FileSystemObject
                objFolder = GetFolder(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\")
                objFile = objFolder.Files
                For Each objFile In objFolder.Files
                    curDir = _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\"
                    '  strFileName = _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name
                    If RightVB6(objFile.Name, 4) = ".270" And (LeftVB6(objFile.Name, 2) = "FA" Or LeftVB6(objFile.Name, 2) = "TX") Then
                        '  objFS = New Scripting.FileSystemObject
                        curFile = GetFileName(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name)
                        If System.IO.File.Exists(curDir & "270\" & objFile.Name) Then System.IO.File.Delete(curDir & "270\" & objFile.Name)
                        System.IO.File.Move(curDir & objFile.Name, curDir & "270\")
                    End If
                Next
                'objFS = Nothing
            End If
        End If


        If rstClrHse.Fields("fldClearingHouseID").Value = 24 Then 'Medicaid of NC
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*1972557213-5T-ISA-0001-.x12") > "" Then
                'objFS = New Scripting.FileSystemObject
                objFolder = GetFolder(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\")
                objFile = objFolder.Files
                For Each objFile In objFolder.Files
                    curDir = _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\"
                    '  strFileName = _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name
                    If LeftVB6(objFile.Name, 4) = "R-BX" And RightVB6(objFile.Name, 27) = "1972557213-5T-ISA-0001-.x12" Then
                        '  objFS = New Scripting.FileSystemObject
                        curFile = GetFileName(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name)
                        If System.IO.File.Exists(curDir & "835\" & Mid(objFile.Name, 12)) Then System.IO.File.Delete(curDir & "835\" & Mid(objFile.Name, 12))
                        System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name, curDir & "835\" & Mid(objFile.Name, 12))
                    End If

                    If LeftVB6(objFile.Name, 4) = "R-BX" And RightVB6(objFile.Name, 19) = "FF-03-ISA-0001-.x12" Then  'R-BXYEKT7U-190516071354-1913600000000330FF-03-ISA-0001-.x12
                        ' objFS = New Scripting.FileSystemObject
                        curFile = GetFileName(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name)
                        If System.IO.File.Exists(curDir & "ACK\" & Mid(objFile.Name, 12)) Then System.IO.File.Delete(curDir & "ACK\" & Mid(objFile.Name, 12))
                        System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name, curDir & "ACK\" & Mid(objFile.Name, 12))
                    End If
                Next
                'objFS = Nothing
            End If
        End If


        If rstClrHse.Fields("fldClearingHouseID").Value = 159 Then 'Tricare East
            '20180131-659951.S001064.20180131-002016470.P.EST.5010-835pru_File19.dat
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*5010-835*.dat") > "" Then
                '  objFS = New Scripting.FileSystemObject
                objFolder = GetFolder(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\")


                objFile = objFolder.Files




                For Each objFile In objFolder.Files
                    inFile = objFile.Name
                    curDir = _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\"
                    If inFile > "" And RightVB6(inFile, 4) = ".dat" And InStr(1, inFile, "5010-835") > 0 Then
                        inFile = Replace(inFile, ".", "-")
                        inFile = Replace(inFile, "-dat", ".dat")
                        inFile = LeftVB6(inFile, InStr(1, inFile, "-P-EST")) & ".dat"
                        inFile = Replace(inFile, "-.dat", ".dat")
                        '  strFileName = _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name
                        '   objFS = New Scripting.FileSystemObject
                        curFile = GetFileName(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name)
                        If System.IO.File.Exists(curDir & "835\" & Mid(objFile.Name, 12)) Then System.IO.File.Delete(curDir & "835\" & Mid(objFile.Name, 12))
                        If System.IO.File.Exists(curDir & "835\" & inFile) Then System.IO.File.Delete(curDir & "835\" & inFile)
                        System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name, curDir & "835\" & inFile)
                    End If
                Next
                'objFS = Nothing
            End If
        End If

        If rstClrHse.Fields("fldClearingHouseID").Value = 52 Then 'BcBs of TN
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.edi") > "" Then
                '     objFS = New Scripting.FileSystemObject
                objFolder = GetFolder(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\")
                objFile = objFolder.Files
                For Each objFile In objFolder.Files
                    curDir = _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\"
                    inFile = objFile.Name
                    If LeftVB6(objFile.Name, 9) = "ecpsy001_" And RightVB6(objFile.Name, 7) = "835.edi" Then
                        '     objFS = New Scripting.FileSystemObject



                        curFile = GetFileName(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name)
                        If System.IO.File.Exists(curDir & "835\" & Mid(objFile.Name, 10)) Then System.IO.File.Delete(curDir & "835\" & Mid(objFile.Name, 10))
                        System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name, curDir & "835\" & Mid(objFile.Name, 10))
                    End If
                    'ecpsy001_193450217_42516267_121119439996_071933.X12.12112019072729_000439996.12110728426.TA1.edi
                    'ecpsy001_193450217_42516267_121119439996_071933-X12-12112019072729_000439996-12110728426-TA1
                    '000439996-12110728426-TA1.dat
                    If LeftVB6(inFile, 9) = "ecpsy001_" And RightVB6(inFile, 7) = "TA1.edi" Then
                        inFile = Replace(inFile, ".", "-")
                        inFile = Replace(inFile, "TA1-edi", "TA1")
                        inFile = Mid(inFile, InStr(1, inFile, "X12-") + 20) & ".dat"
                        '     objFS = New Scripting.FileSystemObject
                        curFile = GetFileName(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name)
                        If System.IO.File.Exists(curDir & "SUB\" & Mid(objFile.Name, 12)) Then System.IO.File.Delete(curDir & "SUB\" & Mid(objFile.Name, 12))
                        If System.IO.File.Exists(curDir & "SUB\" & inFile) Then System.IO.File.Delete(curDir & "SUB\" & inFile)
                        System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name, curDir & "SUB\" & inFile)
                    End If
                    'ecpsy001_193450217_42516267_121119439996_071933.X12.12112019072729_000439996.12110728718.999.edi
                    If LeftVB6(inFile, 9) = "ecpsy001_" And RightVB6(inFile, 7) = "999.edi" Then
                        inFile = Replace(inFile, ".", "-")
                        inFile = Replace(inFile, "999-edi", "999")
                        inFile = Mid(inFile, InStr(1, inFile, "X12-") + 20) & ".dat"
                        '     objFS = New Scripting.FileSystemObject
                        curFile = GetFileName(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name)
                        If System.IO.File.Exists(curDir & "ACK\" & Mid(objFile.Name, 12)) Then System.IO.File.Delete(curDir & "ACK\" & Mid(objFile.Name, 12))
                        If System.IO.File.Exists(curDir & "ACK\" & inFile) Then System.IO.File.Delete(curDir & "ACK\" & inFile)
                        System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name, curDir & "ACK\" & inFile)
                    End If
                    'ecpsy001_200641163_742983766_000455077_277CA.edi
                    If LeftVB6(inFile, 9) = "ecpsy001_" And RightVB6(inFile, 9) = "277CA.edi" Then
                        inFile = Replace(inFile, ".", "-")
                        inFile = Replace(inFile, "277CA-edi", "277.dat")
                        'inFile = Mid(inFile, InStr(1, inFile, "X12-") + 20) & ".dat"
                        '     objFS = New Scripting.FileSystemObject
                        curFile = GetFileName(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name)
                        If System.IO.File.Exists(curDir & "277\" & Mid(objFile.Name, 12)) Then System.IO.File.Delete(curDir & "277\" & Mid(objFile.Name, 12))
                        If System.IO.File.Exists(curDir & "277\" & inFile) Then System.IO.File.Delete(curDir & "277\" & inFile)
                        System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name, curDir & "277\" & inFile)
                    End If
                Next
                'objFS = Nothing
            End If
        End If

        If rstClrHse.Fields("fldClearingHouseID").Value = 1 Then 'Availity sarting to send files with names that are too long
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.era*") > "" Then
                '     objFS = New Scripting.FileSystemObject
                objFolder = GetFolder(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\")
                objFile = objFolder.Files
                For Each objFile In objFolder.Files
                    curDir = _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\"
                    If Len(objFile.Name) > 60 And _
                        RightVB6(objFile.Name, 4) = ".era" Then
                        '     objFS = New Scripting.FileSystemObject
                        curFile = GetFileName(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name)
                        If System.IO.File.Exists(curDir & "835\" & RightVB6(objFile.Name, 40)) Then System.IO.File.Delete(curDir & "835\" & RightVB6(objFile.Name, 40))
                        System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name, curDir & "835\" & RightVB6(objFile.Name, 40))
                    ElseIf InStr(1, objFile.Name, "-001.era-") > 0 Then 'ERA-MAGELLAN_BEHAVIORAL_HEALTH_SYSTEMS_LLC-202011091430-001.era-98896898022
                        System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name, curDir & "835\" & LeftVB6(objFile.Name, InStr(1, objFile.Name, "-001.era-")) & "002.era")
                    ElseIf RightVB6(objFile.Name, 4) = ".era" Then
                        System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name, curDir & "835\")
                    End If
                Next
                'objFS = Nothing
            End If
        End If

        If _DB.IfNull(rstClrHse.Fields("fldRemittanceYN").Value, "") = "Y" Then
            objFolder = GetFolder(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\")
            objFile = objFolder.Files
            For Each objFile In objFolder.Files
                inFile = objFile.Name
                curFile = objFile
                If objFile.Name > "" And _
                   (RightVB6(objFile.Name, 4) = ".835" Or _
                    RightVB6(objFile.Name, 4) = ".ERN" Or _
                    RightVB6(objFile.Name, 4) = ".ARA" Or _
                    RightVB6(objFile.Name, 4) = ".RAP" Or _
                    RightVB6(objFile.Name, 7) = "835.edi" Or _
                    LeftVB6(objFile.Name, 4) = "ERA-" Or _
                    LeftVB6(objFile.Name, 8) = "5010835." Or _
                    LeftVB6(objFile.Name, 9) = "5010835B." Or _
                    LeftVB6(objFile.Name, 9) = "835_5010_" Or _
                    LeftVB6(objFile.Name, 8) = "835Ansi_") _
                    Then
                    If System.IO.File.Exists(curDir & "835\" & objFile.Name) And objFile.Size > 0 Then System.IO.File.Delete(curDir & "835\" & objFile.Name)
                    System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
                End If
                If objFile.Name > "" And InStr(1, objFile.Name, "EPS") > 1 Then
                    If System.IO.File.Exists(curDir & "EPS\" & objFile.Name) And objFile.Size > 0 Then System.IO.File.Delete(curDir & "EPS\" & objFile.Name)
                    System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\" & objFile.Name, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\EPS\")
                End If
            Next
            objFile = Nothing
            objFolder = Nothing
        End If

        If rstClrHse.Fields("fldClearingHouseID").Value = 11 Then 'Capario
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.ACK") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.ACK", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
            End If
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.TRN") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.TRN", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\TRN\")
            End If
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.PRA") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.PRA", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\PRA\")
            End If
        End If

        If rstClrHse.Fields("fldClearingHouseID").Value = 57 Then 'BcOfKS
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\277\") > "" Then
                If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\277CA*") > "" Then
                    System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\277CA*", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\277\")
                End If
            End If
        End If

        If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.trnack") > "" Then
            System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.trnack", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
        End If
        If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\TRN.*") > "" Then
            System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\TRN.*", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\SUB\")
        End If
        If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\") > "" Then
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.999") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.999", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\")
            End If
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.997") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.997", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ACK\")
            End If
        End If
        If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\277\") > "" Then
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.277") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.277", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\277\")
            End If
        End If
        If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\864\") > "" Then
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.864") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.864", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\864\")
            End If
        End If
        If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ZIP\") > "" Then
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.ZIP") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.ZIP", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ZIP\")
            End If
        End If
        If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\EPS\") > "" Then
            If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.EPS*") > "" Then
                System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.EPS*", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\EPS\")
            End If
        End If
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        ' FIX

        ' does in infile exist out side of next


        'objFolder = GetFolder(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\")
        'objFile = objFolder.Files


        Dim SearchFolder As String = String.Empty
        SearchFolder = _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\"



        For Each foundFile As String In My.Computer.FileSystem.GetFiles(SearchFolder)

            '' For Each objFile In objFolder.Files
            'foundFile = objFile.Name
            If foundFile > "" And _
               RightVB6(foundFile, 3) <> "asc" And _
               RightVB6(foundFile, 3) <> "bat" And _
               RightVB6(foundFile, 3) <> ".ht" And _
               RightVB6(foundFile, 3) <> ".cp" And _
               RightVB6(foundFile, 3) <> "was" And _
               RightVB6(foundFile, 3) <> "wax" And _
               RightVB6(foundFile, 13) <> "ElecClaim.txt" And _
               LeftVB6(foundFile, 8) <> "password" Then




                '************************************************************************************************************************************
                '************************************************************************************************************************************



                '' objText = objFile.OpenAsTextStream(ForReading)

                ' Create new StreamReader instance with Using block.

                ' FIX prbally need to us full path
                Using reader As StreamReader = New StreamReader(foundFile)
                    ' Read one line from file


                    Dim line As String = String.Empty




                    Do Until reader.EndOfStream
                        line = reader.ReadLine

                        strLine = line
                        If Mid(strLine, 1, 3) = "ISA" Then
                            If Len(strLine) <= 107 Then
                                strLine = line
                                If Mid(strLine, 1, 3) = "GS*" Then
                                    strLine = line
                                    If Mid(strLine, 1, 3) = "ST*" Then
                                        reader.Close()

                                        If InStr(3, strLine, "*TA1*") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\")
                                        If InStr(3, strLine, "*997*") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                                        If InStr(3, strLine, "*999*") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                                        If InStr(3, strLine, "*271*") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\271\")
                                        If InStr(3, strLine, "*277*") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\277\")
                                        If InStr(3, strLine, "*824*") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\824\")
                                        If InStr(3, strLine, "*835*") Then
                                            If rstClrHse.Fields("fldClearingHouseID").Value = 39 Or rstClrHse.Fields("fldClearingHouseID").Value = 87 Then        'Medicare of PA, DC
                                                '16T4_1922430.000
                                                If Len(foundFile) = 16 And Mid(foundFile, 13, 1) = "." Then
                                                    System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\835\" & LeftVB6(foundFile, Len(foundFile) - 4) & "-" & VB6.Format(Now(), "YYYYMMDD") & RightVB6(foundFile, 4))
                                                End If
                                            ElseIf rstClrHse.Fields("fldClearingHouseID").Value = 33 Then        'BcBs Of GA
                                                If InStr(1, foundFile, ".") = (Len(foundFile) - 4) Then
                                                    System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\835\" & LeftVB6(foundFile, Len(foundFile) - 4) & "-" & VB6.Format(Now(), "YYYYMMDD") & RightVB6(foundFile, 4))
                                                End If
                                            Else
                                                System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
                                            End If
                                        End If
                                    End If
                                ElseIf Mid(strLine, 1, 4) = "TA1*" Then
                                    reader.Close()
                                    System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\")
                                ElseIf InStr(3, strLine, "GS*") Then
                                    If Not InStr(3, strLine, "TA1*") And Not InStr(3, strLine, "ST*") And Not InStr(3, strLine, "ST^") And Not InStr(3, strLine, "ST\") Then strLine = line
                                    reader.Close()
                                    If InStr(3, strLine, "TA1*") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\")
                                    If InStr(1, strLine, "ST*997*") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                                    If InStr(1, strLine, "ST^997^") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                                    If InStr(1, strLine, "ST\997\") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                                    If InStr(1, strLine, "ST*999*") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                                    If InStr(1, strLine, "ST^999^") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                                    If InStr(1, strLine, "ST\999\") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                                    If InStr(1, strLine, "ST{999{") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                                    If InStr(1, strLine, "ST*271*") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\271\")
                                    If InStr(1, strLine, "ST^271^") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\271\")
                                    If InStr(1, strLine, "ST\271\") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\271\")
                                    If InStr(1, strLine, "ST{271{") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\271\")
                                    If InStr(1, strLine, "ST*277*") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\277\")
                                    If InStr(1, strLine, "ST^277^") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\277\")
                                    If InStr(1, strLine, "ST\277\") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\277\")
                                    If InStr(1, strLine, "ST{277{") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\277\")
                                    If InStr(1, strLine, "ST*835*") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
                                    If InStr(1, strLine, "ST^835^") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
                                    If InStr(1, strLine, "ST\835\") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
                                    If InStr(1, strLine, "ST{835{") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
                                End If
                            Else
                                reader.Close()
                                If InStr(3, strLine, "TA1*") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\SUB\")
                                If InStr(1, strLine, "ST*997*") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                                If InStr(1, strLine, "ST^997^") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                                If InStr(1, strLine, "ST\997\") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                                If InStr(1, strLine, "ST*999*") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                                If InStr(1, strLine, "ST^999^") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                                If InStr(1, strLine, "ST\999\") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                                If InStr(1, strLine, "ST{999{") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
                                If InStr(1, strLine, "ST*271*") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\271\")
                                If InStr(1, strLine, "ST^271^") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\271\")
                                If InStr(1, strLine, "ST\271\") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\271\")
                                If InStr(1, strLine, "ST{271{") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\271\")
                                If InStr(1, strLine, "ST*277*") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\277\")
                                If InStr(1, strLine, "ST^277^") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\277\")
                                If InStr(1, strLine, "ST\277\") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\277\")
                                If InStr(1, strLine, "ST{277{") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\277\")
                                If InStr(1, strLine, "ST^835^") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
                                If InStr(1, strLine, "ST{835{") Then System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
                                If InStr(1, strLine, "ST*835*") Then
                                    If rstClrHse.Fields("fldClearingHouseID").Value = 39 Or rstClrHse.Fields("fldClearingHouseID").Value = 87 Then        'Medicare of PA, DC
                                        '16T4_1922430.000
                                        If Len(foundFile) = 16 And Mid(foundFile, 13, 1) = "." Then
                                            System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\835\" & LeftVB6(foundFile, Len(foundFile) - 4) & "-" & VB6.Format(Now(), "YYYYMMDD") & RightVB6(foundFile, 4))
                                        End If
                                    ElseIf rstClrHse.Fields("fldClearingHouseID").Value = 33 Then        'BcBs Of GA
                                        If InStr(1, foundFile, ".") = (Len(foundFile) - 4) Then
                                            System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\835\" & LeftVB6(foundFile, Len(foundFile) - 4) & "-" & VB6.Format(Now(), "YYYYMMDD") & RightVB6(foundFile, 4))
                                        End If
                                    Else
                                        System.IO.File.Move(curFile, _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\835\")
                                    End If
                                End If
                            End If
                        End If
                    Loop
                End Using
            End If
            If foundFile > "" And _
               RightVB6(foundFile, 3) = "zip" Or RightVB6(foundFile, 3) = "ZIP" And _
               Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ZIP\") > "" Then
                If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ZIP\" & foundFile) > "" Then Kill(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ZIP\" & foundFile)
                If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.ZIP") > "" Then
                    System.IO.File.Move(curFile, _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ZIP\")
                End If
            End If
        Next



        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************

























        If rstClrHse.Fields("fldClearingHouseID").Value = 22 Or _
           rstClrHse.Fields("fldClearingHouseID").Value = 84 Or _
           rstClrHse.Fields("fldClearingHouseID").Value = 124 Then   'Tricare West, TriWest, BcBs of SC
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.X12", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ACK\")
        End If
        If rstClrHse.Fields("fldClearingHouseID").Value = 90 Or _
           rstClrHse.Fields("fldClearingHouseID").Value = 91 Then  'Post and Track
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.HTML", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\RSP\")
        End If

        If rstClrHse.Fields("fldClearingHouseID").Value = 53 Then   'BcBs of ARIZONA
            System.IO.File.Move(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.pgp", _EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\PGP\")
        End If


        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************




        'If rstClrHse.Fields("fldClearingHouseID").Value = 50 Or _
        '       rstClrHse.Fields("fldClearingHouseID").Value = 92 Or _
        '       rstClrHse.Fields("fldClearingHouseID").Value = 104 Or _
        '       rstClrHse.Fields("fldClearingHouseID").Value = 105 Or _
        '       rstClrHse.Fields("fldClearingHouseID").Value = 106 Or _
        '       rstClrHse.Fields("fldClearingHouseID").Value = 116 Then       'BcBs of HA, Medicaid of VA UnZip Files
        '    If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.zip") > "" Then
        '        System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.zip", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ZIP\")
        '    End If
        '    If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.ZIP") > "" Then
        '        System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.ZIP", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ZIP\")
        '    End If
        '    If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.*Z") > "" Then
        '        If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ZIP\" & foundFile) > "" Then Kill(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ZIP\" & foundFile)
        '        System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.*Z", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ZIP\")
        '    End If
        '    If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\*.*z") > "" Then
        '        If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ZIP\" & foundFile) > "" Then Kill(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\ZIP\" & foundFile)
        '        System.IO.File.Move(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\*.*z", _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\ZIP\")
        '    End If
        'End If


        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************
        '************************************************************************************************************************************

















        'objFS = Nothing
        objFolder = Nothing

        Exit Sub

ErrTrap:
        ShowError(Err)
        'objFS = Nothing
    End Sub












    Private Sub UpdateEClaims(ByVal rst As ADODB.Recordset)
        'Updates queued status for electronic claims

        Dim lngClaimID As Long
        Dim lngELID As Long
        Dim lngPlanID As Long
        Dim objClaim As ClaimBz.CClaimBz
        Dim lngCtr As Long
        ' Dim itm As ListItem
        Dim dtPrintDate As Date

        'Screen.MousePointer  = vbHourglass

        'Update the database with the claim submit date
        dtPrintDate = CDate(VB6.Format(Now, "mm/dd/yyyy hh:nn"))

        objClaim = CreateObject("ClaimBz.CClaimBz")
        Do While Not rst.EOF
            'For lngCtr = 1 To rst.RecordCount
            '  Set itm = frmSubmission.lstEClaim.ListItems(lngCtr)

            lngClaimID = rst.Fields("fldClaimID").Value
            lngELID = rst.Fields("fldEncounterLogID").Value
            lngPlanID = rst.Fields("fldPlanID").Value

            objClaim.SubmitClaim(dtPrintDate, lngClaimID, lngELID, lngPlanID, TX_ECLAIM_SUBMITTED, -1, "GLENXXX")
            'Next lngCtr
            rst.MoveNext()
        Loop

        objClaim = Nothing

        'Screen.MousePointer  = vbDefault

    End Sub

    'Private Function Case31(ByVal AppPath As String, ByVal b) As Integer


    '    If Dir(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\MULTIFILES") > "" Then
    '        Kill(_EDIOutput & "\" & rstClrHse.Fields("fldFolder").Value & "\MULTIFILES")
    '    End If
    '    objFS.CopyFile(_EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\" & _DB.IfNull(g_FileName, ""), _EDIOutput & "\" & _DB.IfNull(rstClrHse.Fields("fldFolder").Value, "") & "\MULTIFILES")
    '    '************************************************************************************************************************************







    'End Function


    Private Sub GetSettings()

        '   Dim aConfig As Configuration.ConfigurationSettings

        _isDebug = Configuration.ConfigurationSettings.AppSettings("isDebug")


        If _isDebug Then
            _ConnectionString = Configuration.ConfigurationSettings.AppSettings("ConnectionString_Backup")
        Else
            _ConnectionString = Configuration.ConfigurationSettings.AppSettings("ConnectionString")
        End If





        _EDIOutput = Configuration.ConfigurationSettings.AppSettings("EDIOutput")
        '' _SourcePath = Convert.ToString(app.GetValue("SourcePath", _SourcePath.GetType())); 'ConfigurationSettings.AppSettings["BaseURL"].ToString();
        '        _Verbose = Convert.ToInt32(app.GetValue("Verbose", _Verbose.GetType())); 'ConfigurationSettings.AppSettings["BaseURL"].ToString();
        '' _LogFilePath = Convert.ToString(app.GetValue("LogFilePath", _LogFilePath.GetType()));
        '        _AppName = Convert.ToString(app.GetValue("InstanceName", _AppName.GetType()));
        '        _WaitForEnterToExit = Convert.ToString(app.GetValue("WaitForEnterToExit", _WaitForEnterToExit.GetType()));
        ''_ParseTree = Convert.ToInt32(app.GetValue("ParseTree", _ParseTree.GetType()));



        ''  _LOG_EDI = Convert.ToString(app.GetValue("_LOG_EDI", _LOG_EDI.GetType()));


        '        _ForceClearTextPasswd = Convert.ToInt32(app.GetValue("ForceClearTextPasswd", _ForceClearTextPasswd.GetType()));


        '                _ProcessYearMonth = Convert.ToString(app.GetValue("ProcessYearMonth", _ProcessYearMonth.GetType()));

        '_OverrideProcessYearMonth = Convert.ToString(app.GetValue("OverrideProcessYearMonth", _OverrideProcessYearMonth.GetType()));



        '_FilePath = Convert.ToString(app.GetValue("FilePath", _FilePath.GetType()));
        '_FileName = Convert.ToString(app.GetValue("FileName", _FileName.GetType()));
        '_ClientName = Convert.ToString(app.GetValue("ClientName", _ClientName.GetType()));
        '_HOSP_CODE = Convert.ToString(app.GetValue("HOSP_CODE", _HOSP_CODE.GetType()));




        '_SMTPServer = Convert.ToString(app.GetValue("SMTPServer", _SMTPServer.GetType())); 'ConfigurationSettings.AppSettings["BaseURL"].ToString();
        '_FromMailAddress = Convert.ToString(app.GetValue("FromMailAddress", _FromMailAddress.GetType())); 'ConfigurationSettings.AppSettings["BaseURL"].ToString();
        '_ToMailAddress = Convert.ToString(app.GetValue("ToMailAddress", _ToMailAddress.GetType()));




        '' 270

        '_270Load = Convert.ToInt32(app.GetValue("Load270", _270Load.GetType()));
        '_FileFilter270 = Convert.ToString(app.GetValue("FileFilter270", _FileFilter270.GetType()));
        '_270Input = Convert.ToString(app.GetValue("Input270", _270Input.GetType()));
        '_270Failed = Convert.ToString(app.GetValue("Failed270", _270Failed.GetType()));
        '_270Success = Convert.ToString(app.GetValue("Success270", _270Success.GetType()));
        ''_270Duplicate = Convert.ToString(app.GetValue("Duplicate270", _270Duplicate.GetType()));
        ''_270_VAULT_ROOT_DIRECTORY = Convert.ToString(app.GetValue("VAULT_ROOT_DIRECTORY_270", _270_VAULT_ROOT_DIRECTORY.GetType()));
        ''_270_VAULT_CLIENT_DIRECTORY = Convert.ToString(app.GetValue("VAULT_CLIENT_DIRECTORY_270", _270_VAULT_CLIENT_DIRECTORY.GetType()));
        ''_270_VAULT_FILE_PATH = Convert.ToString(app.GetValue("VAULT_FILE_PATH_270", _270_VAULT_FILE_PATH.GetType()));

        ''dirs.Rows.Add(_270Load, "270", _FileFilter270, _270Input, _270Failed, _270Success, _270Duplicate, _270_VAULT_ROOT_DIRECTORY, _270_VAULT_CLIENT_DIRECTORY, _270_VAULT_FILE_PATH);



        ' ''271

        ''_271Load = Convert.ToInt32(app.GetValue("Load271", _271Load.GetType()));
        ''_FileFilter271 = Convert.ToString(app.GetValue("FileFilter271", _FileFilter271.GetType()));
        ''_271Input = Convert.ToString(app.GetValue("Input271", _271Input.GetType()));
        ''_271Failed = Convert.ToString(app.GetValue("Failed271", _271Failed.GetType()));
        ''_271Success = Convert.ToString(app.GetValue("Success271", _271Success.GetType()));
        ''_271Duplicate = Convert.ToString(app.GetValue("Duplicate271", _271Duplicate.GetType()));
        ''_271_VAULT_ROOT_DIRECTORY = Convert.ToString(app.GetValue("VAULT_ROOT_DIRECTORY_271", _271_VAULT_ROOT_DIRECTORY.GetType()));
        ''_271_VAULT_CLIENT_DIRECTORY = Convert.ToString(app.GetValue("VAULT_CLIENT_DIRECTORY_271", _271_VAULT_CLIENT_DIRECTORY.GetType()));
        ''_271_VAULT_FILE_PATH = Convert.ToString(app.GetValue("VAULT_FILE_PATH_271", _271_VAULT_FILE_PATH.GetType()));

        ''dirs.Rows.Add(_271Load, "271", _FileFilter271, _271Input, _271Failed, _271Success, _271Duplicate, _271_VAULT_ROOT_DIRECTORY, _271_VAULT_CLIENT_DIRECTORY, _271_VAULT_FILE_PATH);

        ''/276

        '        _276Load = Convert.ToInt32(app.GetValue("Load276", _276Load.GetType()));

        '        _FileFilter276 = Convert.ToString(app.GetValue("FileFilter276", _FileFilter276.GetType()));
        '        _276Input = Convert.ToString(app.GetValue("Input276", _276Input.GetType()));
        '        _276Failed = Convert.ToString(app.GetValue("Failed276", _276Failed.GetType()));
        '        _276Success = Convert.ToString(app.GetValue("Success276", _276Success.GetType()));
        '        _276Duplicate = Convert.ToString(app.GetValue("Duplicate276", _276Duplicate.GetType()));
        '        _276_VAULT_ROOT_DIRECTORY = Convert.ToString(app.GetValue("VAULT_ROOT_DIRECTORY_276", _276_VAULT_ROOT_DIRECTORY.GetType()));
        '        _276_VAULT_CLIENT_DIRECTORY = Convert.ToString(app.GetValue("VAULT_CLIENT_DIRECTORY_276", _276_VAULT_CLIENT_DIRECTORY.GetType()));
        '        _276_VAULT_FILE_PATH = Convert.ToString(app.GetValue("VAULT_FILE_PATH_276", _276_VAULT_FILE_PATH.GetType()));

        '        dirs.Rows.Add(
        '            _276Load,
        '            "276",
        '            _FileFilter276,
        '            _276Input,
        '            _276Failed,
        '            _276Success,
        '            _276Duplicate,
        '            _276_VAULT_ROOT_DIRECTORY,
        '            _276_VAULT_CLIENT_DIRECTORY,
        '_276_VAULT_FILE_PATH()
        '            );

        ' ''277

        '        _277Load = Convert.ToInt32(app.GetValue("Load277", _277Load.GetType()));

        '        _FileFilter277 = Convert.ToString(app.GetValue("FileFilter277", _FileFilter277.GetType()));
        '        _277Input = Convert.ToString(app.GetValue("Input277", _277Input.GetType()));
        '        _277Failed = Convert.ToString(app.GetValue("Failed277", _277Failed.GetType()));
        '        _277Success = Convert.ToString(app.GetValue("Success277", _277Success.GetType()));
        '        _277Duplicate = Convert.ToString(app.GetValue("Duplicate277", _277Duplicate.GetType()));
        '        _277_VAULT_ROOT_DIRECTORY = Convert.ToString(app.GetValue("VAULT_ROOT_DIRECTORY_277", _277_VAULT_ROOT_DIRECTORY.GetType()));
        '        _277_VAULT_CLIENT_DIRECTORY = Convert.ToString(app.GetValue("VAULT_CLIENT_DIRECTORY_277", _277_VAULT_CLIENT_DIRECTORY.GetType()));
        '        _277_VAULT_FILE_PATH = Convert.ToString(app.GetValue("VAULT_FILE_PATH_277", _277_VAULT_FILE_PATH.GetType()));

        '        dirs.Rows.Add(
        '            _277Load, 
        '            "277", 
        '            _FileFilter277,
        '            _277Input,
        '            _277Failed,
        '            _277Success, 
        '            _277Duplicate, 
        '            _277_VAULT_ROOT_DIRECTORY, 
        '            _277_VAULT_CLIENT_DIRECTORY,
        '_277_VAULT_FILE_PATH()
        '            );



        '' 278

        '_278Load = Convert.ToInt32(app.GetValue("Load278", _278Load.GetType()));

        '_ParseTree278 = Convert.ToInt32(app.GetValue("ParseTree278", _ParseTree278.GetType()));
        '_FileFilter278 = Convert.ToString(app.GetValue("FileFilter278", _FileFilter278.GetType()));
        '_278Input = Convert.ToString(app.GetValue("Input278", _278Input.GetType()));
        '_278Failed = Convert.ToString(app.GetValue("Failed278", _278Failed.GetType()));
        '_278Success = Convert.ToString(app.GetValue("Success278", _278Success.GetType()));
        '_278Duplicate = Convert.ToString(app.GetValue("Duplicate278", _278Duplicate.GetType()));
        '_278_VAULT_ROOT_DIRECTORY = Convert.ToString(app.GetValue("VAULT_ROOT_DIRECTORY_278", _278_VAULT_ROOT_DIRECTORY.GetType()));
        '_278_VAULT_CLIENT_DIRECTORY = Convert.ToString(app.GetValue("VAULT_CLIENT_DIRECTORY_278", _278_VAULT_CLIENT_DIRECTORY.GetType()));
        '_278_VAULT_FILE_PATH = Convert.ToString(app.GetValue("VAULT_FILE_PATH_278", _278_VAULT_FILE_PATH.GetType()));

        'dirs.Rows.Add(_278Load, "278", _FileFilter278, _278Input, _278Failed, _278Success, _278Duplicate, _278_VAULT_ROOT_DIRECTORY, _278_VAULT_CLIENT_DIRECTORY, _278_VAULT_FILE_PATH);



        ' '' 835

        '        _835Load = Convert.ToInt32(app.GetValue("Load835", _835Load.GetType()));

        ''_ParseTree835 = Convert.ToInt32(app.GetValue("ParseTree835", _ParseTree835.GetType()));
        '        _FileFilter835 = Convert.ToString(app.GetValue("FileFilter835", _FileFilter835.GetType()));
        '        _835Input = Convert.ToString(app.GetValue("Input835", _835Input.GetType()));
        '        _835Failed = Convert.ToString(app.GetValue("Failed835", _835Failed.GetType()));
        '        _835Success = Convert.ToString(app.GetValue("Success835", _835Success.GetType()));
        '        _835Duplicate = Convert.ToString(app.GetValue("Duplicate835", _835Duplicate.GetType()));
        '        _835_VAULT_ROOT_DIRECTORY = Convert.ToString(app.GetValue("VAULT_ROOT_DIRECTORY_835", _835_VAULT_ROOT_DIRECTORY.GetType()));
        '        _835_VAULT_CLIENT_DIRECTORY = Convert.ToString(app.GetValue("VAULT_CLIENT_DIRECTORY_835", _835_VAULT_CLIENT_DIRECTORY.GetType()));
        '        _835_VAULT_FILE_PATH = Convert.ToString(app.GetValue("VAULT_FILE_PATH_835", _835_VAULT_FILE_PATH.GetType()));

        ''   dirs.Rows.Add(_835Load, "835", _FileFilter835, _835Input, _835Failed, _835Success, _835Duplicate, _835_VAULT_ROOT_DIRECTORY, _835_VAULT_CLIENT_DIRECTORY, _835_VAULT_FILE_PATH);


        '        dirs.Rows.Add(
        '            _835Load,
        '            "835",
        '            _FileFilter835,
        '            _835Input,
        '            _835Failed,
        '            _835Success,
        '            _835Duplicate,
        '            _835_VAULT_ROOT_DIRECTORY,
        '            _835_VAULT_CLIENT_DIRECTORY,
        '_835_VAULT_FILE_PATH()
        '            );




        '        _837Load = Convert.ToInt32(app.GetValue("Load837", _837Load.GetType()));

        ''_ParseTree835 = Convert.ToInt32(app.GetValue("ParseTree835", _ParseTree835.GetType()));
        '        _FileFilter837 = Convert.ToString(app.GetValue("FileFilter837", _FileFilter837.GetType()));
        '        _837Input = Convert.ToString(app.GetValue("Input837", _837Input.GetType()));
        '        _837Failed = Convert.ToString(app.GetValue("Failed837", _837Failed.GetType()));
        '        _837Success = Convert.ToString(app.GetValue("Success837", _837Success.GetType()));
        '        _837Duplicate = Convert.ToString(app.GetValue("Duplicate837", _837Duplicate.GetType()));
        '        _837_VAULT_ROOT_DIRECTORY = Convert.ToString(app.GetValue("VAULT_ROOT_DIRECTORY_837", _837_VAULT_ROOT_DIRECTORY.GetType()));
        '        _837_VAULT_CLIENT_DIRECTORY = Convert.ToString(app.GetValue("VAULT_CLIENT_DIRECTORY_837", _837_VAULT_CLIENT_DIRECTORY.GetType()));
        '        _837_VAULT_FILE_PATH = Convert.ToString(app.GetValue("VAULT_FILE_PATH_837", _837_VAULT_FILE_PATH.GetType()));

        ''   dirs.Rows.Add(_835Load, "835", _FileFilter835, _835Input, _835Failed, _835Success, _835Duplicate, _835_VAULT_ROOT_DIRECTORY, _835_VAULT_CLIENT_DIRECTORY, _835_VAULT_FILE_PATH);


        '        dirs.Rows.Add(
        '            _837Load,
        '            "837",
        '            _FileFilter837,
        '            _837Input,
        '            _837Failed,
        '            _837Success,
        '            _837Duplicate,
        '            _837_VAULT_ROOT_DIRECTORY,
        '            _837_VAULT_CLIENT_DIRECTORY,
        '_837_VAULT_FILE_PATH()
        '            );



        '    }
        '    catch (Exception ex)
        '    {


        '        Console.Write("Get Settings Fail" + ex.Message + "\n\r");
        '        Environment.Exit(0);

        '    }
        '    Console.Write("Runing as: " + _AppName + "\n\r");
        '    Console.Write("Get Settings Complete" + "\n\r");

        '}


    End Sub





End Class