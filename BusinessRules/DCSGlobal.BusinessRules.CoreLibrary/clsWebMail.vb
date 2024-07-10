Imports System.Web.mail




Public Class WebMail

    Private Structure stUserInfo

        Public MailTo As String
        Public MailCC As String
        Public MailBCC As String
        Public MailFrom As String
        Public MailBody As String
        Public MailSMTP As String
        Public MailSubject As String

    End Structure

    Private objMailInfo As New stUserInfo
    Public eMsg As String = String.Empty


    Public Function Ver(ByVal test As String) As String

        Dim v As String = "24"

        Return v + test



    End Function

    Private Function DoEmail() As Integer
        'Dim objmail As New MailMessage
        'Dim eBody As String
        ''Try



        'objmail.To = objMailInfo.MailTo
        'objmail.Cc = objMailInfo.MailCC
        'objmail.Bcc = objMailInfo.MailBCC
        'objmail.Subject = objMailInfo.MailSubject
        'objmail.Body = objMailInfo.MailBody
        'objmail.From = objMailInfo.MailFrom
        'SmtpMail.SmtpServer = objMailInfo.MailSMTP
        'SmtpMail.Send(objmail)




        Return 0
        'Exit Try

        'Catch ex As Exception
        '  Dim MyLog As New EventLog
        '  MyLog.WriteEntry(ex.Message)
        ' MyLog = Nothing
        'Return -1
        'End Try

    End Function

    WriteOnly Property MailTo() As String
        Set(ByVal Value As String)
            objMailInfo.MailTo = Value
        End Set
    End Property
    WriteOnly Property MailCC() As String

        Set(ByVal Value As String)
            objMailInfo.MailCC = Value

        End Set
    End Property
    WriteOnly Property MailBCC() As String

        Set(ByVal Value As String)
            objMailInfo.MailBCC = Value

        End Set
    End Property
    WriteOnly Property MailFrom() As String

        Set(ByVal Value As String)
            objMailInfo.MailFrom = Value

        End Set
    End Property
    WriteOnly Property MailSMTP() As String

        Set(ByVal Value As String)

            objMailInfo.MailSMTP = Value


        End Set
    End Property
    WriteOnly Property MailSubject() As String

        Set(ByVal Value As String)
            objMailInfo.MailSubject = Value

        End Set
    End Property
    WriteOnly Property MailBody() As String

        Set(ByVal Value As String)
            objMailInfo.MailBody = Value

        End Set
    End Property
    WriteOnly Property AddBodyLine() As String
        Set(ByVal Value As String)
            objMailInfo.MailBody = objMailInfo.MailBody + Chr(13) + Value
        End Set

    End Property
    WriteOnly Property MailToList() As String
        Set(ByVal Value As String)

        End Set

    End Property

    Public Function SendSingleMail() As Integer

        Return DoEmail()

    End Function

    Public Function ICANN_EXTisOK(ByVal sEXT As String) As Integer

        Dim EXT As String = String.Empty
        Dim X As Long = 0

        ICANN_EXTisOK = 1

        If Left(sEXT, 1) <> "." Then sEXT = "." & sEXT

        sEXT = UCase(sEXT) 'just to avoid errors
        EXT = EXT & ".COM.EDU.GOV.NET.BIZ.ORG.TV"
        EXT = EXT & ".AF.AL.DZ.AS.AD.AO.AI.AQ.AG.AP.AR.AM.AW.AU.AT.AZ.BS.BH.BD.BB.BY"
        EXT = EXT & ".BE.BZ.BJ.BM.BT.BO.BA.BW.BV.BR.IO.BN.BG.BF.MM.BI.KH.CM.CA.CV.KY"
        EXT = EXT & ".CF.TD.CL.CN.CX.CC.CO.KM.CG.CD.CK.CR.CI.HR.CU.CY.CZ.DK.DJ.DM.DO"
        EXT = EXT & ".TP.EC.EG.SV.GQ.ER.EE.ET.FK.FO.FJ.FI.CS.SU.FR.FX.GF.PF.TF.GA.GM.GE.DE"
        EXT = EXT & ".GH.GI.GB.GR.GL.GD.GP.GU.GT.GN.GW.GY.HT.HM.HN.HK.HU.IS.IN.ID.IR.IQ"
        EXT = EXT & ".IE.IL.IT.JM.JP.JO.KZ.KE.KI.KW.KG.LA.LV.LB.LS.LR.LY.LI.LT.LU.MO.MK.MG"
        EXT = EXT & ".MW.MY.MV.ML.MT.MH.MQ.MR.MU.YT.MX.FM.MD.MC.MN.MS.MA.MZ.NA"
        EXT = EXT & ".NR.NP.NL.AN.NT.NC.NZ.NI.NE.NG.NU.NF.KP.MP.NO.OM.PK.PW.PA.PG.PY"
        EXT = EXT & ".PE.PH.PN.PL.PT.PR.QA.RE.RO.RU.RW.GS.SH.KN.LC.PM.ST.VC.SM.SA.SN.SC"
        EXT = EXT & ".SL.SG.SK.SI.SB.SO.ZA.KR.ES.LK.SD.SR.SJ.SZ.SE.CH.SY.TJ.TW.TZ.TH.TG.TK"
        EXT = EXT & ".TO.TT.TN.TR.TM.TC.TV.UG.UA.AE.UK.US.UY.UM.UZ.VU.VA.VE.VN.VG.VI"
        EXT = EXT & ".WF.WS.EH.YE.YU.ZR.ZM.ZW"
        EXT = UCase(EXT) 'just to avoid errors

        If InStr(1, sEXT, EXT, vbBinaryCompare) <> 0 Then ICANN_EXTisOK = 0

    End Function

    Public Function ValidateEmail(ByVal strEmail As String) As Integer
        Dim strTmp As String, n As Long, sEXT As String
        eMsg = "" 'reset on open for good form
        ValidateEmail = 0 'Assume true on init


        Dim objProfanityCheck As New StringHandlingStuff.StringStuff



        sEXT = strEmail
        Do While InStr(1, sEXT, ".") <> 0
            sEXT = Right(sEXT, Len(sEXT) - InStr(1, sEXT, "."))
        Loop

        If strEmail = "" Then
            ValidateEmail = 1
            eMsg = eMsg & "<BR>You did not enter an email address!"
        ElseIf InStr(1, strEmail, "@") = 0 Then
            ValidateEmail = 1
            eMsg = eMsg & "<BR>Your email address does not contain an @ sign."
        ElseIf InStr(1, strEmail, "@") = 1 Then
            ValidateEmail = 1
            eMsg = eMsg & "<BR>Your @ sign can not be the first character in your email address!"
        ElseIf InStr(1, strEmail, "@") = Len(strEmail) Then
            ValidateEmail = 1
            eMsg = eMsg & "<BR>Your @sign can not be the last character in your email address!"
        ElseIf InStr(1, UCase(strEmail), "SPAM") = 1 Then
            ValidateEmail = 1
            eMsg = eMsg & "<BR>Your email address contains the word spam, come on get real!"
        ElseIf ICANN_EXTisOK(sEXT) = 0 Then
            ValidateEmail = 1
            eMsg = eMsg & "<BR>Your email address is not carrying a valid ending!"
            eMsg = eMsg & "<BR>It must be one of the following..."
            eMsg = eMsg & "<BR>.com, .net, .gov, .org, .edu, .biz, .us Or end in your country's assigned country code"
        ElseIf objProfanityCheck.ProfanityCheck(strEmail) = 1 Then
            ValidateEmail = 1
            eMsg = eMsg & "<BR>Your email address failed the profanity rules for this site!"
        ElseIf Len(strEmail) < 6 Then
            ValidateEmail = 1
            eMsg = eMsg & "<BR>Your email address is shorter than 6 characters which is impossible."
        End If
        strTmp = strEmail
        Do While InStr(1, strTmp, "@") <> 0
            n = 1
            strTmp = Right(strTmp, Len(strTmp) - InStr(1, strTmp, "@"))
        Loop
        If n > 1 Then
            ValidateEmail = 1 'found more than one @ sign
            eMsg = eMsg & "<BR>You have more than 1 @ sign in your email address"
        End If
    End Function
End Class
