Option Explicit On


Imports DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff

Imports System.Text.RegularExpressions
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Collections.Generic
Imports DCSGlobal.BusinessRules.Logging













Namespace DCSGlobal.EDI


    Public Class Import271
        Inherits EDIDataTables
        Implements IDisposable

        Protected disposed As Boolean = False

        Dim _ClassVersion As String = "2.0"
        Dim start_time As DateTime
        Dim stop_time As DateTime
        Dim elapsed_time As TimeSpan
        Dim oD As New Declarations
        Dim objss As New StringStuff
        Dim _TablesBuilt As Boolean = False
        Dim _ENFlag As Boolean = False
        Dim distinctDT As DataTable
        Dim err As String = ""
        Dim _chars As String

        Dim log As New logExecption
        Dim _HSDCount As Integer
        Dim _Debug = 0
        Dim _DeadLockCount As Integer = 0
        Dim _DeadLockFlag As Boolean = False
        Dim _EPICOutEDIString As String

        Dim Val As New ValidateEDI

        '       @EDI varchar(max)='',

        '@PAYOR_ID varchar(50)='',

        '@Vendor_name varchar(50)='',

        '@Log_EDI varchar(1)='N'



        Public meddd As IEnumerable
        Dim mmm As Integer = 1
        Dim _isEPIC As Boolean = False


        Public Event OnPreParse As EventHandler




        Public Event OnPostParse As EventHandler


        Protected Overrides Sub Finalize()

            Dispose()
            '' Console.WriteLine("Object " & GetHashCode() & " finalized.")
        End Sub

        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposed Then
                If disposing Then

                    log = Nothing
                    oD = Nothing


                    'email = Nothing
                    ' Insert code to free managed resources. 
                End If

                objss = Nothing
                ' Insert code to free unmanaged resources. 
            End If
            Me.disposed = True
        End Sub


        ' Do not change or add Overridable to these methods. 
        ' Put cleanup code in Dispose(ByVal disposing As Boolean). 
        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub



        Public WriteOnly Property DoAuditLog As Boolean

            Set(value As Boolean)
                oD.DoAuditLog = value
            End Set
        End Property


        Public Property AAAFailureCode As String
            Get
                Return oD._AAAFailureCode
            End Get
            Set(value As String)

            End Set
        End Property






        Public WriteOnly Property ConnectionString As String

            Set(value As String)
                oD._ConnectionString = value
                log.ConnectionString = value
            End Set
        End Property


        Public WriteOnly Property NPI As String

            Set(value As String)
                oD.NPI = value
            End Set
        End Property

        Public WriteOnly Property ServiceTypeCode As String

            Set(value As String)
                oD.ServiceTypeCode = value
            End Set
        End Property

        Public WriteOnly Property BatchID As Double

            Set(value As Double)
                oD._BatchID = value

            End Set
        End Property

        Public WriteOnly Property EDI As String

            Set(value As String)
                oD.edi = value
            End Set
        End Property


        Public WriteOnly Property RAW270 As String

            Set(value As String)
                oD.RAW270 = value
            End Set
        End Property


        Public WriteOnly Property DeadlockRetrys As Integer

            Set(value As Integer)
                oD._DeadLockRetrys = value

            End Set
        End Property



        Public Property DTdistinctDT As DataTable
            Get
                Return distinctDT
            End Get
            Set(value As DataTable)

            End Set
        End Property

        Public WriteOnly Property Commit As Integer

            Set(value As Integer)
                oD.commit = value
            End Set
        End Property





        Public WriteOnly Property ebr_id As Double


            Set(value As Double)
                oD.ebr_id = value
            End Set
        End Property

        Public WriteOnly Property user_id As String

            Set(value As String)
                oD.user_id = value
            End Set
        End Property


        Public WriteOnly Property hosp_code As String

            Set(value As String)
                oD.hosp_code = value
            End Set
        End Property


        Public WriteOnly Property source As String

            Set(value As String)
                oD.source = value
            End Set
        End Property




        Public WriteOnly Property DebugLevel As Integer

            Set(value As Integer)
                oD.DebugLevel = value
            End Set
        End Property


        Public WriteOnly Property PAYOR_ID As String

            Set(value As String)
                oD.PAYOR_ID = value
            End Set
        End Property


        Public WriteOnly Property Vendor_name As String

            Set(value As String)
                oD.Vendor_name = value
            End Set
        End Property




        Public WriteOnly Property Log_EDI As String

            Set(value As String)
                oD.Log_EDI = value
            End Set
        End Property




        Public WriteOnly Property DeleteFlag As String

            Set(value As String)
                oD.DeleteFlag = value
            End Set
        End Property
        Public WriteOnly Property Verbose As Integer

            Set(value As Integer)
                oD.Verbose = value
            End Set
        End Property



        Public WriteOnly Property Dump As Integer

            Set(value As Integer)
                oD.Dump = value
            End Set
        End Property

        Public Property ExecutionTime As TimeSpan
            Get
                Return elapsed_time


            End Get
            Set(value As TimeSpan)

            End Set
        End Property

        Public Property EBLoopCount As Integer
            Get
                Return mmm
            End Get
            Set(value As Integer)

            End Set
        End Property

        Public ReadOnly Property ErrorMessage As String
            Get
                Return err
            End Get

        End Property

        Public ReadOnly Property chars As String
            Get
                Return _chars
            End Get

        End Property

        Public ReadOnly Property Status As String
            Get
                Return oD.Status

            End Get

        End Property


        Public ReadOnly Property Reject_Reason_code As String
            Get
                Return oD.RejectReasonCode
            End Get

        End Property

        Public ReadOnly Property LOOP_AGAIN As String
            Get
                Return oD.LoopAgain

            End Get

        End Property




        Public ReadOnly Property EPICOutEDIString As String

            Get
                Return _EPICOutEDIString

            End Get

        End Property



        Public WriteOnly Property isEPIC As Boolean


            Set(value As Boolean)
                _isEPIC = value
            End Set
        End Property


        Public Function Import() As Integer

            '
            Dim sqlConn As SqlConnection = New SqlConnection

            Dim sqlString As String = String.Empty
            '            Dim RE As Integer
            Dim r As Integer = -1
            'Try


           

            'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

            'Using ARL As New AuditResponseLogging
            '    ARL.ConnectionString = oD._ConnectionString


            '    'parse the 271 and find AAA
            '    Using vedi As New ValidateEDI()
            '        r = vedi.byString(oD.edi)
            '        If r = 0 Then
            '            oD._AAAFailureCode = vedi.AAAFailureCode
            '        End If
            '    End Using

            '    'parse the 270 and find NPI and 
            '    Using vedi As New ValidateEDI()
            '        r = vedi.byString(oD.RAW270)
            '        If r = 0 Then
            '            oD.NPI = vedi.NPI
            '            oD.ServiceTypeCode = vedi.ServiceTypeCode
            '        End If
            '    End Using

            '    ARL.Log271(oD._BatchID, oD.PAYOR_ID, oD.edi, oD.ebr_id, oD.Vendor_name, oD._AAAFailureCode, oD.NPI, oD.ServiceTypeCode)
            'End Using





            ''Get Service Type code from 270
            'RE = Val.byString(oD.edi)

            'oD.ServiceTypeCode = Val.ServiceTypeCode

            ''Get AAA & NPI from RAW271
            'RE = Val.byString(oD.edi)

            'oD._AAAFailureCode = Val.AAAFailureCode

            'oD.NPI = Val.NPI
            ''

            'sqlString = "usp_eligibility_log_EDI_271_response"

            'Using Con As New SqlConnection(oD._ConnectionString)
            '    Con.Open()
            '    Using cmd As New SqlCommand(sqlString, Con)

            '        cmd.CommandType = CommandType.StoredProcedure

            '        cmd.Parameters.AddWithValue("@batch_id", oD._BatchID)
            '        cmd.Parameters.AddWithValue("@PAYOR_ID", oD.PAYOR_ID)
            '        cmd.Parameters.AddWithValue("@EDI", oD.edi)
            '        cmd.Parameters.AddWithValue("@ebr_id", oD.ebr_id)
            '        cmd.Parameters.AddWithValue("@vendor_id", oD.Vendor_name)
            '        cmd.Parameters.AddWithValue("@Res_Reject_Reason_Code", oD._AAAFailureCode)
            '        cmd.Parameters.AddWithValue("@res_npi", oD.NPI)
            '        cmd.Parameters.AddWithValue("@req_ServiceType_Code", oD.ServiceTypeCode)
            '        cmd.ExecuteNonQuery()

            '    End Using
            '    Con.Close()
            'End Using

            'Catch ex As Exception
            '    '
            '    log.ExceptionDetails("LOG EDI 271 failed for batch ID : " + Convert.ToString(oD._BatchID), "")

            'End Try


            '*************************************************************************************************************
            '
            '
            '
            '**************************************************************************************************************
            Dim rr As Integer = 0


            '     Try

            start_time = Now
            oD.TimeStamp = FormatDateTime(start_time, DateFormat.ShortDate)


            '*************************************************************************************************************
            '
            '
            '
            '**************************************************************************************************************

            oD.BATCHUID = Guid.NewGuid()



            ' check to see IF we have a bactch ID  if not get out 
            If oD._BatchID = 0 Then
                If (_Debug = 1) Then


                    ''    log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import271", "BatchID = 0 err -1")
                End If

                rr = -1
                Return rr
                Exit Function
            End If


            '*************************************************************************************************************
            '
            '  ediToTableParse ... BREAK the edi INTO rows and put then in edi271  
            '
            '**************************************************************************************************************




            oD.edi = objss.StripCRLF(oD.edi)


            oD.ESFlag = 0
            oD.ESFlagStart = 0



            ' call the sub to build the tables
            If _TablesBuilt = False Then
                BuildTables()
                _TablesBuilt = True
            Else

                ENVELOP.Clear()
                N3_N4.Clear()

                EDI_271.Clear()
                ISA.Clear()
                GS.Clear()
                ST.Clear()
                HL01.Clear()
                HL02.Clear()
                HL03.Clear()
                HL04.Clear()
                BHT.Clear()
                EB.Clear()
                AAA.Clear()
                AMT.Clear()
                DMG.Clear()
                DTP.Clear()
                EQ.Clear()
                HSD.Clear()
                HL.Clear()
                III.Clear()
                INS.Clear()
                TMSG.Clear()
                MSG.Clear()
                N3.Clear()
                N4.Clear()
                NM1.Clear()
                PER.Clear()
                PRV.Clear()
                REF.Clear()
                TRN.Clear()
                UNK.Clear()

                CACHE_EB.Clear()
                CACHE_AAA.Clear()
                CACHE_AMT.Clear()
                CACHE_DMG.Clear()
                CACHE_DTP.Clear()
                CACHE_EQ.Clear()
                CACHE_HSD.Clear()
                CACHE_HL.Clear()
                CACHE_III.Clear()
                CACHE_INS.Clear()
                CACHE_TMSG.Clear()
                CACHE_MSG.Clear()
                CACHE_N3.Clear()
                CACHE_N4.Clear()
                CACHE_NM1.Clear()
                CACHE_PER.Clear()
                CACHE_PRV.Clear()
                CACHE_REF.Clear()
                CACHE_TRN.Clear()


                oD.LoopAgain = ""
                oD.RejectReasonCode = ""
                oD.Status = ""
                err = 0

                _chars = 0

            End If


            oD.idx_EDI271 = 0


            Try


                oD.ediTildeCount = Regex.Matches(oD.edi, Regex.Escape("~")).Count 'InStr(nt(edi,'~')) 


            Catch ex As Exception

                log.ExceptionDetails("48-" + oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import271 " + Convert.ToString(oD._BatchID), ex)


                rr = -7
                Return rr
                Exit Function

            End Try



            oD.DataElementSeparator = Mid(oD.edi, 4, 1)



            For idx_EDI271 = 1 To oD.ediTildeCount




                If idx_EDI271 = 5000 Then
                    If (_Debug = 1) Then
                        '    log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import271", "idx_EDI271 exceeded index count execed " + idx_EDI271.ToString + "  err -2")
                    End If

                    rr = -2
                    Return rr
                    Exit Function
                End If


                oD.ediRowData = objss.ParseDemlimtedStringEDI(oD.edi, oD.SegmentTerminator, idx_EDI271)
                'oD.ediRowRecordType = (SELECT dbo.[udf_ParseDemlimtedString](ediRowData,""',0)) 
                oD.ediRowRecordType = objss.ParseDemlimtedStringEDI(oD.ediRowData, oD.DataElementSeparator, 1)

                'being the messy task of the eb




                If oD.ediRowRecordType = "EB" Then


                    If oD.ESFlagStart = 0 Then
                        '    'begin

                        oD.ESFlagStart = 1
                        oD.ESFlag = 1
                        '    'end
                    End If





                    Select Case oD.ESFlagStart = 1
                        'begin

                        Case 1

                            Select Case oD.ESFlag

                                Case 0


                                    Dim nSERow As DataRow = EDI_271.NewRow
                                    nSERow("ROW_RECORD_TYPE") = "EE"
                                    nSERow("ROW_DATA") = "EE ** " + oD.EDIMatch
                                    nSERow("RowDataParsed") = 0
                                    nSERow("BATCH_ID") = oD._BatchID

                                    EDI_271.Rows.Add(nSERow)

                                    'begin
                                    oD.EDIMatch = oD.ediRowData
                                    oD.ESFlag = 0
                                    'end


                                Case 1
                                    Dim nSEERow As DataRow = EDI_271.NewRow
                                    nSEERow("ROW_RECORD_TYPE") = oD.ediRowRecordType
                                    nSEERow("ROW_DATA") = oD.ediRowData
                                    nSEERow("RowDataParsed") = 0
                                    nSEERow("BATCH_ID") = oD._BatchID

                                    EDI_271.Rows.Add(nSEERow)



                                    oD.EDIMatch = oD.ediRowData
                                    oD.ESFlag = 1
                                    'end
                            End Select
                            'end



                        Case 0

                    End Select



                Else

                    If oD.ediRowRecordType = "SE" Then

                        'INSERT INTO  edi271( ROW_RECORD_TYPE, ROW_DATA, RowProcessed, BATCH_ID ) VALUES  
                        '( 'EE' ,'EE""' + EDIMatch, 0 , BatchID)  

                        Dim nSERow As DataRow = EDI_271.NewRow
                        nSERow("ROW_RECORD_TYPE") = "EE"
                        nSERow("ROW_DATA") = "EE ** " + oD.EDIMatch
                        nSERow("RowDataParsed") = 0
                        nSERow("BATCH_ID") = oD._BatchID

                        EDI_271.Rows.Add(nSERow)


                        'Dim nSEERow As DataRow = EDI_271.NewRow
                        'nSEERow("ROW_RECORD_TYPE") = oD.ediRowRecordType
                        'nSEERow("ROW_DATA") = oD.ediRowData
                        'nSEERow("RowDataParsed") = 0
                        'nSEERow("BATCH_ID") = oD._BatchID

                        'EDI_271.Rows.Add(nSEERow)


                    End If

                End If


                ' end the messy task od the RB/ES

                If oD.ESFlag = 0 Then


                    Dim nRow As DataRow = EDI_271.NewRow
                    nRow("ROW_RECORD_TYPE") = oD.ediRowRecordType
                    nRow("ROW_DATA") = oD.ediRowData
                    nRow("RowDataParsed") = 0
                    nRow("BATCH_ID") = oD._BatchID

                    EDI_271.Rows.Add(nRow)

                End If

                oD.ESFlag = 0
            Next


            '' ok so lets get down to busines
            oD.RowProcessedFlag = 0


            For Each row As DataRow In EDI_271.Rows
                oD.CurrentRowData = row.Item("ROW_DATA")
                oD.ROW_RECORD_TYPE = row.Item("ROW_RECORD_TYPE")






                'BEGIN



                ' begin process ISA

                If oD.ROW_RECORD_TYPE = "ISA" Then



                    oD.EBCarrotCHAR = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 12)

                    oD.ISA_GUID = Guid.NewGuid
                    oD.P_GUID = oD.ISA_GUID

                    oD.ISA01 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)
                    oD.ISA02 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                    oD.ISA03 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)
                    oD.ISA04 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 5)
                    oD.ISA05 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 6)
                    oD.ISA06 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 7)
                    oD.ISA07 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 8)
                    oD.ISA08 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 9)
                    oD.ISA09 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 10)
                    oD.ISA10 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 11)
                    oD.ISA11 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 12)
                    oD.ISA12 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 13)
                    oD.ISA13 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 14)
                    oD.ISA14 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 15)
                    oD.ISA15 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 16)
                    oD.ISA16 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 17)

                    Dim ISARow As DataRow = ISA.NewRow
                    ISARow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    ISARow("ISA01") = oD.ISA01
                    ISARow("ISA02") = oD.ISA02
                    ISARow("ISA03") = oD.ISA03
                    ISARow("ISA04") = oD.ISA04
                    ISARow("ISA05") = oD.ISA05
                    ISARow("ISA06") = oD.ISA06
                    ISARow("ISA07") = oD.ISA07
                    ISARow("ISA08") = oD.ISA08
                    ISARow("ISA09") = oD.ISA09
                    ISARow("ISA10") = oD.ISA10
                    ISARow("ISA11") = oD.ISA11
                    ISARow("ISA12") = oD.ISA12
                    ISARow("ISA13") = oD.ISA13
                    ISARow("ISA14") = oD.ISA14
                    ISARow("ISA15") = oD.ISA15
                    ISARow("ISA16") = oD.ISA16
                    ISARow("TIME_STAMP") = oD.TimeStamp
                    ISARow("BATCH_ID") = oD._BatchID
                    ISA.Rows.Add(ISARow)

                    oD.ISA_ROW_ID = row.Item("ROW_ID")



                    oD.CarrotDataDelimiter = oD.ISA11
                    oD.ComponentElementSeperator = oD.ISA16


                    _chars = "RowDataDelimiter: " + oD.DataElementSeparator + " CarrotDataDelimiter: " + oD.CarrotDataDelimiter + " ComponentElementSeperator: " + oD.ComponentElementSeperator


                    oD.RowProcessedFlag = 1
                    oD.ISAFlag = 1
                End If


                'set some vars
















                ' end ISA


                ' begin GS


                If oD.ROW_RECORD_TYPE = "GS" Then

                    oD.GS_GUID = Guid.NewGuid
                    oD.P_GUID = oD.GS_GUID

                    oD.GS01 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)
                    oD.GS02 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                    oD.GS03 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)
                    oD.GS04 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 5)
                    oD.GS05 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 6)
                    oD.GS06 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 7)
                    oD.GS07 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 8)
                    oD.GS08 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 9)



                    Dim GSRow As DataRow = GS.NewRow
                    GSRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    GSRow("HIPAA_GS_GUID") = oD.GS_GUID
                    GSRow("P_GUID") = oD.ISA_GUID
                    GSRow("GS01") = oD.GS01
                    GSRow("GS02") = oD.GS02
                    GSRow("GS03") = oD.GS03
                    GSRow("GS04") = oD.GS04
                    GSRow("GS05") = oD.GS05
                    GSRow("GS06") = oD.GS06
                    GSRow("GS07") = oD.GS07
                    GSRow("GS08") = oD.GS08
                    GSRow("TIME_STAMP") = oD.TimeStamp
                    GSRow("BATCH_ID") = oD._BatchID
                    GS.Rows.Add(GSRow)

                    oD.GS_ROW_ID = row.Item("ROW_ID")
                    oD.RowProcessedFlag = 1

                End If

                'end GS

                ' begin st


                If oD.ROW_RECORD_TYPE = "ST" Then

                    oD.ST_GUID = Guid.NewGuid
                    oD.P_GUID = oD.ST_GUID

                    oD.ST01 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)
                    oD.ST02 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                    oD.ST03 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)

                    Dim STRow As DataRow = ST.NewRow
                    STRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    STRow("HIPAA_GS_GUID") = oD.GS_GUID
                    STRow("HIPAA_ST_GUID") = oD.ST_GUID
                    STRow("P_GUID") = oD.GS_GUID
                    STRow("ST01") = oD.ST01
                    STRow("ST02") = oD.ST02
                    STRow("ST03") = oD.ST03
                    STRow("TIME_STAMP") = oD.TimeStamp
                    STRow("BATCH_ID") = oD._BatchID
                    ST.Rows.Add(STRow)

                    oD.ST_ROW_ID = row.Item("ROW_ID")
                    oD.RowProcessedFlag = 1

                End If

                'end ST



                ' begin st


                If oD.ROW_RECORD_TYPE = "BHT" Then

                    oD.BHT_GUID = Guid.NewGuid
                    'oD.P_GUID = oD.ST_GUID

                    oD.BHT01 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)
                    oD.BHT02 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                    oD.BHT03 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)
                    oD.BHT04 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 5)
                    oD.BHT05 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 6)
                    oD.BHT06 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 7)



                    Dim BHTRow As DataRow = BHT.NewRow
                    BHTRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    BHTRow("HIPAA_GS_GUID") = oD.GS_GUID
                    BHTRow("HIPAA_ST_GUID") = oD.ST_GUID
                    BHTRow("P_GUID") = oD.GS_GUID
                    BHTRow("BHT01") = oD.BHT01
                    BHTRow("BHT02") = oD.BHT02
                    BHTRow("BHT03") = oD.BHT03
                    BHTRow("BHT04") = oD.BHT04
                    BHTRow("BHT05") = oD.BHT05
                    BHTRow("BHT06") = oD.BHT06
                    BHTRow("TIME_STAMP") = oD.TimeStamp
                    BHTRow("BATCH_ID") = oD._BatchID
                    BHT.Rows.Add(BHTRow)


                    oD.RowProcessedFlag = 1

                    _ENFlag = True


                End If

                'end ST



                '-- HERE WE BGIN THE HICHRICAL REOCRD  
                '-- BEGIN HL  
                '-- SO NEED HL1  

                '******************************************************************************************************************************
                ' handle the enlove stuff
                '******************************************************************************************************************************
                If _ENFlag Then




                    Dim ENVELOPRow As DataRow = ENVELOP.NewRow
                    ENVELOPRow("ROW_ID") = oD.BuildNumber
                    ENVELOPRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    ENVELOPRow("HIPAA_GS_GUID") = oD.GS_GUID
                    ENVELOPRow("HIPAA_ST_GUID") = oD.ST_GUID
                    ENVELOPRow("P_GUID") = oD.GS_GUID
                    ENVELOPRow("ISA01") = oD.ISA01
                    ENVELOPRow("ISA02") = oD.ISA02
                    ENVELOPRow("ISA03") = oD.ISA03
                    ENVELOPRow("ISA04") = oD.ISA04
                    ENVELOPRow("ISA05") = oD.ISA05
                    ENVELOPRow("ISA06") = oD.ISA06
                    ENVELOPRow("ISA07") = oD.ISA07
                    ENVELOPRow("ISA08") = oD.ISA08
                    ENVELOPRow("ISA09") = oD.ISA09
                    ENVELOPRow("ISA10") = oD.ISA10
                    ENVELOPRow("ISA11") = oD.ISA11
                    ENVELOPRow("ISA12") = oD.ISA12
                    ENVELOPRow("ISA13") = oD.ISA13
                    ENVELOPRow("ISA14") = oD.ISA14
                    ENVELOPRow("ISA15") = oD.ISA15
                    ENVELOPRow("ISA16") = oD.ISA16
                    ENVELOPRow("GS01") = oD.GS01
                    ENVELOPRow("GS02") = oD.GS02
                    ENVELOPRow("GS03") = oD.GS03
                    ENVELOPRow("GS04") = oD.GS04
                    ENVELOPRow("GS05") = oD.GS05
                    ENVELOPRow("GS06") = oD.GS06
                    ENVELOPRow("GS07") = oD.GS07
                    ENVELOPRow("GS08") = oD.GS08
                    ENVELOPRow("ST01") = oD.ST01
                    ENVELOPRow("ST02") = oD.ST02
                    ENVELOPRow("ST03") = oD.ST03
                    ENVELOPRow("BHT01") = oD.BHT01
                    ENVELOPRow("BHT02") = oD.BHT02
                    ENVELOPRow("BHT03") = oD.BHT03
                    ENVELOPRow("BHT04") = oD.BHT04
                    ENVELOPRow("BATCH_ID") = oD._BatchID
                    ENVELOPRow("TIME_STAMP") = oD.TimeStamp
                    ENVELOP.Rows.Add(ENVELOPRow)

                    _ENFlag = False
                End If




                'oD.ISA_ROW_ID = row.Item("ROW_ID")
                'oD.RowProcessedFlag = 1
                'oD.ISAFlag = 1






                '******************************************************************************************************************************
                '  end the enlovep stuff
                '******************************************************************************************************************************


                If oD.ROW_RECORD_TYPE = "HL" Then

                    'BEGIN()

                    '-- NEED TO FIGURE WHAT LEVEL WE ARE AT and what has been done and a ton of stuff that would be way easier with colections or super simple in oracle  
                    '-- but nooooooooooooo I have GWBasic FROM 1985 with syntax highliting at least it had arrays  
                    '-- ok rant over now, its just access on sterroids, no seriously now its over   
                    oD.CurrentHLLevel = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)

                    Select Case oD.CurrentHLLevel


                        Case 1
                            'BEGIN()
                            oD.CurrentHLGroup = "20"

                            '-- ok so let since this is and HL one we should look to see IF there is already and HL one tag in this GS but will just COUNT on the HLFLag for now  
                            '-- so check the flag to see IF this a new hl1 SET IF not then something is wrong and get out  
                            If oD.HL20Flag = 0 Then
                                ' BEGIN()

                                '-- so lets et a few things   
                                '-- since the parent id is not used we skip it  


                                '-- lets create a new row in the hl TABLE and dump it threre  


                                '-- get HL level code  

                                oD.HL01Data = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)
                                oD.HL02Data = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                                oD.HL03Data = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)
                                oD.HL04Data = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 5)

                                oD.HL20_GUID = Guid.NewGuid

                                oD.P_GUID = oD.HL20_GUID
                                oD.HL1CHILDCOUNT = oD.HL04Data
                                oD.HLCANCELFLAG = oD.HL1CHILDCOUNT


                                Dim HL01Row As DataRow = HL01.NewRow
                                HL01Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                                HL01Row("HIPAA_GS_GUID") = oD.GS_GUID
                                HL01Row("HIPAA_ST_GUID") = oD.ST_GUID
                                HL01Row("P_GUID") = oD.P_GUID
                                HL01Row("HL_PARENT") = oD.CurrentHLGroup
                                HL01Row("HL01") = oD.HL01Data
                                HL01Row("HL02") = oD.HL02Data
                                HL01Row("HL03") = oD.HL03Data
                                HL01Row("HL04") = oD.HL04Data
                                HL01Row("BATCH_ID") = oD._BatchID
                                HL01Row("TIME_STAMP") = oD.TimeStamp
                                HL01.Rows.Add(HL01Row)



                                oD.HL20Flag = 1
                            ElseIf oD.HL20Flag = 1 Then
                                Return -3
                                Exit Function
                            End If


                        Case 2





                            oD.CurrentHLGroup = "21"


                            oD.HL01Data = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)
                            oD.HL02Data = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                            oD.HL03Data = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)
                            oD.HL04Data = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 5)

                            oD.HL21_GUID = Guid.NewGuid

                            oD.P_GUID = oD.HL21_GUID
                            oD.HL2CHILDCOUNT = oD.HL04Data
                            oD.HLCANCELFLAG = oD.HL2CHILDCOUNT

                            Dim HL02Row As DataRow = HL02.NewRow
                            HL02Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                            HL02Row("HIPAA_GS_GUID") = oD.GS_GUID
                            HL02Row("HIPAA_ST_GUID") = oD.ST_GUID
                            HL02Row("HIPAA_HL01_GUID") = oD.HL20_GUID
                            HL02Row("P_GUID") = oD.P_GUID
                            HL02Row("HL_PARENT") = oD.CurrentHLGroup
                            HL02Row("HL01") = oD.HL01Data
                            HL02Row("HL02") = oD.HL02Data
                            HL02Row("HL03") = oD.HL03Data
                            HL02Row("HL04") = oD.HL04Data
                            HL02Row("BATCH_ID") = oD._BatchID
                            HL02Row("TIME_STAMP") = oD.TimeStamp
                            HL02.Rows.Add(HL02Row)





                            oD.HL21Flag = 1


                        Case 3
                            '                  BEGIN()
                            oD.CurrentHLGroup = "22"

                            oD.HL01Data = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)
                            oD.HL02Data = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                            oD.HL03Data = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)
                            oD.HL04Data = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 5)

                            oD.HL22_GUID = Guid.NewGuid

                            oD.P_GUID = oD.HL22_GUID
                            oD.HL3CHILDCOUNT = oD.HL04Data
                            oD.HLCANCELFLAG = oD.HL3CHILDCOUNT

                            Dim HL03Row As DataRow = HL03.NewRow
                            HL03Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                            HL03Row("HIPAA_GS_GUID") = oD.GS_GUID
                            HL03Row("HIPAA_ST_GUID") = oD.ST_GUID
                            HL03Row("HIPAA_HL01_GUID") = oD.HL20_GUID
                            HL03Row("HIPAA_HL02_GUID") = oD.HL21_GUID
                            HL03Row("P_GUID") = oD.P_GUID
                            HL03Row("HL_PARENT") = oD.CurrentHLGroup
                            HL03Row("HL01") = oD.HL01Data
                            HL03Row("HL02") = oD.HL02Data
                            HL03Row("HL03") = oD.HL03Data
                            HL03Row("HL04") = oD.HL04Data
                            HL03Row("BATCH_ID") = oD._BatchID
                            HL03.Rows.Add(HL03Row)
                            HL03Row("TIME_STAMP") = oD.TimeStamp


                            oD.HL22Flag = 1


                        Case 4


                            oD.CurrentHLGroup = "23"

                            oD.HL01Data = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)
                            oD.HL02Data = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                            oD.HL03Data = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)
                            oD.HL04Data = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 5)

                            oD.HL23_GUID = Guid.NewGuid

                            oD.P_GUID = oD.HL23_GUID
                            oD.HL4CHILDCOUNT = oD.HL04Data
                            oD.HLCANCELFLAG = oD.HL4CHILDCOUNT


                            Dim HL04Row As DataRow = HL04.NewRow
                            HL04Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                            HL04Row("HIPAA_GS_GUID") = oD.GS_GUID
                            HL04Row("HIPAA_ST_GUID") = oD.ST_GUID
                            HL04Row("HIPAA_HL01_GUID") = oD.HL20_GUID
                            HL04Row("HIPAA_HL02_GUID") = oD.HL21_GUID
                            HL04Row("HIPAA_HL03_GUID") = oD.HL22_GUID
                            HL04Row("HIPAA_HL04_GUID") = oD.HL23_GUID
                            HL04Row("P_GUID") = oD.P_GUID
                            HL04Row("HL_PARENT") = oD.CurrentHLGroup
                            HL04Row("HL01") = oD.HL01Data
                            HL04Row("HL02") = oD.HL02Data
                            HL04Row("HL03") = oD.HL03Data
                            HL04Row("HL04") = oD.HL04Data
                            HL04Row("BATCH_ID") = oD._BatchID
                            HL04Row("TIME_STAMP") = oD.TimeStamp
                            HL04.Rows.Add(HL04Row)


                            oD.HL23Flag = 1



                    End Select


                    oD.RowProcessedFlag = 1

                End If
                '' ok thats all the control headers now the mess in 271 that is EB





                ' add msg here







                '******************************************************************************************************************************
                '  Begin  EB
                '******************************************************************************************************************************
                If oD.ROW_RECORD_TYPE = "EB" Then


                    '********************************************************************************************************'  
                    ''EB Start'  
                    '********************************************************************************************************'  






                    oD.EB_GUID = Guid.NewGuid

                    oD.P_GUID = oD.EB_GUID
                    oD.EBCarrotCount = 0

                    ''********'  
                    ''SET all @EBUID to a new UID and SET the EB^ COUNT to 0'  
                    ''SET @EBGroupById = @BatchID   + @CuurrentRow  that way it is always unique'  
                    ''*********'


                    oD.EBEB03RowData = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)
                    oD.EBLASTRow = oD.CurrentRowData


                    If oD.EBEB03RowData = "" Then
                        '                                        BEGIN()

                        oD.EBCarrotFlag = 0

                        '
                        '

                        ''********'  
                        ''This should fire IF the EB03 row data is  ""od.EBCarrotFlag = 0'  
                        ''********'  


                    End If

                    If oD.EBEB03RowData <> "" Then
                        '                                                BEGIN()


                        oD.EBCarrotCount = Regex.Matches(oD.EBEB03RowData, Regex.Escape(oD.EBCarrotCHAR)).Count 'InStr(nt(edi,'~'))   
                        '      od.EBCarrotCount =  (SELECT dbo.udf_charCount(@EBEB03RowData, @EBCarrotCHAR))  

                        If oD.EBCarrotCount <> 0 Then
                            oD.EBCarrotFlag = 1
                        End If

                        If oD.EBCarrotCount = 0 Then
                            oD.EBCarrotFlag = 0
                        End If
                    End If

                    ' '********'  
                    ' 'and do we yes or no ' + CONVERT(varchar(max),@EBCarrotFlag)   
                    ' '*********'  
                    ' 
                    '

                    '                                                            End




                    If oD.EBCarrotFlag = 0 Then
                        '********'  
                        'so at this point commit the EB IF @EBCarrotFlag = 0'  
                        '*********'  

                        oD.EBcount = oD.EBcount + 1



                        Dim EBRow As DataRow = EB.NewRow
                        EBRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                        EBRow("HIPAA_GS_GUID") = oD.GS_GUID
                        EBRow("HIPAA_ST_GUID") = oD.ST_GUID
                        EBRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                        EBRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                        EBRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                        EBRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                        EBRow("HIPAA_EB_GUID") = oD.EB_GUID
                        EBRow("P_GUID") = oD.P_GUID
                        EBRow("HL_PARENT") = oD.CurrentHLGroup


                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then EBRow("EB01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else EBRow("EB01") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then EBRow("EB02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else EBRow("EB02") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then EBRow("EB03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else EBRow("EB03") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then EBRow("EB04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else EBRow("EB04") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then EBRow("EB05") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else EBRow("EB05") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then EBRow("EB06") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else EBRow("EB06") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then EBRow("EB07") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else EBRow("EB07") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then EBRow("EB08") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else EBRow("EB08") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then EBRow("EB09") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else EBRow("EB09") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) <> "") Then EBRow("EB10") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) Else EBRow("EB10") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) <> "") Then EBRow("EB11") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) Else EBRow("EB11") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) <> "") Then EBRow("EB12") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) Else EBRow("EB12") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) <> "") Then EBRow("EB13") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) Else EBRow("EB13") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 15) <> "") Then EBRow("EB13_1") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 15) Else EBRow("EB13_1") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 16) <> "") Then EBRow("EB13_2") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 16) Else EBRow("EB13_2") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 17) <> "") Then EBRow("EB13_3") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 17) Else EBRow("EB13_3") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 18) <> "") Then EBRow("EB13_4") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 18) Else EBRow("EB13_4") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 19) <> "") Then EBRow("EB13_5") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 19) Else EBRow("EB13_5") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 20) <> "") Then EBRow("EB13_6") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 20) Else EBRow("EB13_6") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 21) <> "") Then EBRow("EB13_7") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 21) Else EBRow("EB13_7") = DBNull.Value

                        EBRow("TIME_STAMP") = oD.TimeStamp
                        EBRow("BATCH_ID") = oD._BatchID
                        EB.Rows.Add(EBRow)
                    End If


                    If oD.EBCarrotFlag = 1 Then
                        '********'  
                        'There are ^ and the flag is SET so defer inserting the eb row and move on'  
                        '*********'  


                    End If

                    oD.RowProcessedFlag = 1
                End If
                '******************************************************************************************************************************
                '   End EB
                '******************************************************************************************************************************



                'BEGIN AAA
                If oD.ROW_RECORD_TYPE = "AAA" Then


                    oD._AAAFailureCode = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)

                    'BEGIN()
                    If oD.EBCarrotFlag = 0 Then
                        'BEGIN()
                        oD.isDirtyAAA = 1
                        oD.isDirtyMasterAAA = 1


                        Dim AAARow As DataRow = AAA.NewRow
                        AAARow("HIPAA_ISA_GUID") = oD.ISA_GUID
                        AAARow("HIPAA_GS_GUID") = oD.GS_GUID
                        AAARow("HIPAA_ST_GUID") = oD.ST_GUID
                        AAARow("HIPAA_HL01_GUID") = oD.HL20_GUID
                        AAARow("HIPAA_HL02_GUID") = oD.HL21_GUID
                        AAARow("HIPAA_HL03_GUID") = oD.HL22_GUID
                        AAARow("HIPAA_HL04_GUID") = oD.HL23_GUID
                        AAARow("HIPAA_EB_GUID") = oD.EB_GUID
                        AAARow("P_GUID") = oD.P_GUID
                        AAARow("HL_PARENT") = oD.CurrentHLGroup



                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then AAARow("AAA01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else AAARow("AAA01") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then AAARow("AAA02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else AAARow("AAA02") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then AAARow("AAA03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else AAARow("AAA03") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then AAARow("AAA04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else AAARow("AAA04") = DBNull.Value





                        'AAARow("AAA01") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 2)
                        'AAARow("AAA02") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 3)
                        'AAARow("AAA03") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 4)
                        'AAARow("AAA04") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 5)
                        AAARow("BATCH_ID") = oD._BatchID
                        AAARow("TIME_STAMP") = oD.TimeStamp
                        AAA.Rows.Add(AAARow)

                        oD.RowProcessedFlag = 1


                        'od.STIdentity = @@IDENTITY;  
                        ' --od.STFlag = 1;  
                        oD.RowProcessedFlag = 1

                    End If
                    If oD.EBCarrotFlag = 1 Then

                        oD.isDirtyCACHEAAA = 1
                        oD.isDirtyMasterAAA = 1



                        Dim CAAARow As DataRow = CACHE_AAA.NewRow
                        CAAARow("HIPAA_ISA_GUID") = oD.ISA_GUID
                        CAAARow("HIPAA_GS_GUID") = oD.GS_GUID
                        CAAARow("HIPAA_ST_GUID") = oD.ST_GUID
                        CAAARow("HIPAA_HL01_GUID") = oD.HL20_GUID
                        CAAARow("HIPAA_HL02_GUID") = oD.HL21_GUID
                        CAAARow("HIPAA_HL03_GUID") = oD.HL22_GUID
                        CAAARow("HIPAA_HL04_GUID") = oD.HL23_GUID
                        CAAARow("HIPAA_EB_GUID") = oD.EB_GUID
                        CAAARow("P_GUID") = oD.P_GUID
                        CAAARow("HL_PARENT") = oD.CurrentHLGroup




                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then CAAARow("AAA01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else CAAARow("AAA01") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then CAAARow("AAA02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else CAAARow("AAA02") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then CAAARow("AAA03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else CAAARow("AAA03") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then CAAARow("AAA04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else CAAARow("AAA04") = DBNull.Value




                        'CAAARow("AAA01") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 2)
                        'CAAARow("AAA02") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 3)
                        'CAAARow("AAA03") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 4)
                        'CAAARow("AAA04") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 5)

                        CAAARow("BATCH_ID") = oD._BatchID
                        CAAARow("TIME_STAMP") = oD.TimeStamp
                        CACHE_AAA.Rows.Add(CAAARow)

                        oD.RowProcessedFlag = 1

                    End If

                End If
                'END AAA


                'BEGIN AMT
                If oD.ROW_RECORD_TYPE = "AMT" Then
                    'BEGIN()
                    If oD.EBCarrotFlag = 0 Then
                        'BEGIN()
                        oD.isDirtyAMT = 1
                        oD.isDirtyMasterAMT = 1


                        Dim AMTRow As DataRow = AMT.NewRow
                        AMTRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                        AMTRow("HIPAA_GS_GUID") = oD.GS_GUID
                        AMTRow("HIPAA_ST_GUID") = oD.ST_GUID
                        AMTRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                        AMTRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                        AMTRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                        AMTRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                        AMTRow("HIPAA_EB_GUID") = oD.EB_GUID
                        AMTRow("P_GUID") = oD.P_GUID
                        AMTRow("HL_PARENT") = oD.CurrentHLGroup




                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then AMTRow("AMT01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else AMTRow("AMT01") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then AMTRow("AMT02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else AMTRow("AMT02") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then AMTRow("AMT03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else AMTRow("AMT03") = DBNull.Value


                        'AMTRow("AMT01") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 2)
                        'AMTRow("AMT02") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 3)
                        'AMTRow("AMT03") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 4)
                        AMTRow("BATCH_ID") = oD._BatchID
                        AMTRow("TIME_STAMP") = oD.TimeStamp
                        AMT.Rows.Add(AMTRow)

                        oD.RowProcessedFlag = 1


                        'od.STIdentity = @@IDENTITY;  
                        ' --od.STFlag = 1;  
                        oD.RowProcessedFlag = 1

                    End If
                    If oD.EBCarrotFlag = 1 Then

                        oD.isDirtyCACHEAMT = 1
                        oD.isDirtyMasterAMT = 1



                        Dim CAMTRow As DataRow = CACHE_AMT.NewRow
                        CAMTRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                        CAMTRow("HIPAA_GS_GUID") = oD.GS_GUID
                        CAMTRow("HIPAA_ST_GUID") = oD.ST_GUID
                        CAMTRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                        CAMTRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                        CAMTRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                        CAMTRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                        CAMTRow("HIPAA_EB_GUID") = oD.EB_GUID
                        CAMTRow("P_GUID") = oD.P_GUID
                        CAMTRow("HL_PARENT") = oD.CurrentHLGroup



                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then CAMTRow("AMT01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else CAMTRow("AMT01") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then CAMTRow("AMT02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else CAMTRow("AMT02") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then CAMTRow("AMT03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else CAMTRow("AMT03") = DBNull.Value


                        'CAMTRow("AMT01") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 2)
                        'CAMTRow("AMT02") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 3)
                        'CAMTRow("AMT03") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 4)


                        CAMTRow("BATCH_ID") = oD._BatchID
                        CAMTRow("TIME_STAMP") = oD.TimeStamp
                        CACHE_AMT.Rows.Add(CAMTRow)

                        oD.RowProcessedFlag = 1

                    End If

                End If
                'END AMT


                'BEGIN DMG
                If oD.ROW_RECORD_TYPE = "DMG" Then
                    'BEGIN()
                    If oD.EBCarrotFlag = 0 Then
                        'BEGIN()
                        oD.isDirtyDMG = 1
                        oD.isDirtyMasterDMG = 1


                        Dim DMGRow As DataRow = DMG.NewRow
                        DMGRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                        DMGRow("HIPAA_GS_GUID") = oD.GS_GUID
                        DMGRow("HIPAA_ST_GUID") = oD.ST_GUID
                        DMGRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                        DMGRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                        DMGRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                        DMGRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                        DMGRow("HIPAA_EB_GUID") = oD.EB_GUID
                        DMGRow("P_GUID") = oD.P_GUID
                        DMGRow("HL_PARENT") = oD.CurrentHLGroup

                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then DMGRow("DMG01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else DMGRow("DMG01") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then DMGRow("DMG02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else DMGRow("DMG02") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then DMGRow("DMG03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else DMGRow("DMG03") = DBNull.Value



                        'DMGRow("DMG01") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 2)
                        'DMGRow("DMG02") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 3)
                        'DMGRow("DMG03") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 4)
                        DMGRow("BATCH_ID") = oD._BatchID
                        DMGRow("TIME_STAMP") = oD.TimeStamp
                        DMG.Rows.Add(DMGRow)

                        oD.RowProcessedFlag = 1


                        'od.STIdentity = @@IDENTITY;  
                        ' --od.STFlag = 1;  
                        oD.RowProcessedFlag = 1

                    End If
                    If oD.EBCarrotFlag = 1 Then

                        oD.isDirtyCACHEDMG = 1
                        oD.isDirtyMasterDMG = 1



                        Dim CDMGRow As DataRow = CACHE_DMG.NewRow
                        CDMGRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                        CDMGRow("HIPAA_GS_GUID") = oD.GS_GUID
                        CDMGRow("HIPAA_ST_GUID") = oD.ST_GUID
                        CDMGRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                        CDMGRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                        CDMGRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                        CDMGRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                        CDMGRow("HIPAA_EB_GUID") = oD.EB_GUID
                        CDMGRow("P_GUID") = oD.P_GUID
                        CDMGRow("HL_PARENT") = oD.CurrentHLGroup


                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then CDMGRow("DMG01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else CDMGRow("DMG01") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then CDMGRow("DMG02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else CDMGRow("DMG02") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then CDMGRow("DMG03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else CDMGRow("DMG03") = DBNull.Value




                        'CDMGRow("DMG01") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 2)
                        'CDMGRow("DMG02") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 3)
                        'CDMGRow("DMG03") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 4)
                        CDMGRow("BATCH_ID") = oD._BatchID
                        CDMGRow("TIME_STAMP") = oD.TimeStamp
                        CACHE_DMG.Rows.Add(CDMGRow)

                        oD.RowProcessedFlag = 1

                    End If

                End If
                'END 

                'BEGIN DTP
                If oD.ROW_RECORD_TYPE = "DTP" Then
                    'BEGIN()
                    If oD.EBCarrotFlag = 0 Then
                        'BEGIN()
                        oD.isDirtyDTP = 1
                        oD.isDirtyMasterDTP = 1


                        Dim DTPRow As DataRow = DTP.NewRow
                        DTPRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                        DTPRow("HIPAA_GS_GUID") = oD.GS_GUID
                        DTPRow("HIPAA_ST_GUID") = oD.ST_GUID
                        DTPRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                        DTPRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                        DTPRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                        DTPRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                        DTPRow("HIPAA_EB_GUID") = oD.EB_GUID
                        DTPRow("P_GUID") = oD.P_GUID
                        DTPRow("HL_PARENT") = oD.CurrentHLGroup

                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then DTPRow("DTP01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else DTPRow("DTP01") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then DTPRow("DTP02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else DTPRow("DTP02") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then DTPRow("DTP03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else DTPRow("DTP03") = DBNull.Value



                        'DTPRow("DTP01") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 2)
                        'DTPRow("DTP02") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 3)
                        'DTPRow("DTP03") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 4)
                        DTPRow("BATCH_ID") = oD._BatchID
                        DTPRow("TIME_STAMP") = oD.TimeStamp
                        DTP.Rows.Add(DTPRow)

                        oD.RowProcessedFlag = 1


                        'od.STIdentity = @@IDENTITY;  
                        ' --od.STFlag = 1;  
                        oD.RowProcessedFlag = 1

                    End If
                    If oD.EBCarrotFlag = 1 Then

                        oD.isDirtyCACHEDTP = 1
                        oD.isDirtyMasterDTP = 1



                        Dim CDTPRow As DataRow = CACHE_DTP.NewRow
                        CDTPRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                        CDTPRow("HIPAA_GS_GUID") = oD.GS_GUID
                        CDTPRow("HIPAA_ST_GUID") = oD.ST_GUID
                        CDTPRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                        CDTPRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                        CDTPRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                        CDTPRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                        CDTPRow("HIPAA_EB_GUID") = oD.EB_GUID
                        CDTPRow("P_GUID") = oD.P_GUID
                        CDTPRow("HL_PARENT") = oD.CurrentHLGroup


                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then CDTPRow("DTP01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else CDTPRow("DTP01") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then CDTPRow("DTP02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else CDTPRow("DTP02") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then CDTPRow("DTP03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else CDTPRow("DTP03") = DBNull.Value


                        'CDTPRow("DTP01") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 2)
                        'CDTPRow("DTP02") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 3)
                        'CDTPRow("DTP03") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 4)

                        CDTPRow("BATCH_ID") = oD._BatchID
                        CDTPRow("TIME_STAMP") = oD.TimeStamp
                        CACHE_DTP.Rows.Add(CDTPRow)

                        oD.RowProcessedFlag = 1

                    End If

                End If
                'END 




                'BEGIN EQ
                If oD.ROW_RECORD_TYPE = "EQ" Then
                    'BEGIN()

                    If oD.EBCarrotFlag = 0 Then
                        'BEGIN()
                        oD.isDirtyEQ = 1
                        oD.isDirtyMasterEQ = 1


                        Dim EQRow As DataRow = EQ.NewRow
                        EQRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                        EQRow("HIPAA_GS_GUID") = oD.GS_GUID
                        EQRow("HIPAA_ST_GUID") = oD.ST_GUID
                        EQRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                        EQRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                        EQRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                        EQRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                        EQRow("HIPAA_EB_GUID") = oD.EB_GUID
                        EQRow("P_GUID") = oD.P_GUID
                        EQRow("HL_PARENT") = oD.CurrentHLGroup



                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then EQRow("EQ01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else EQRow("EQ01") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then EQRow("EQ02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else EQRow("EQ02") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then EQRow("EQ02_1") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else EQRow("EQ02_1") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then EQRow("EQ02_2") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else EQRow("EQ02_2") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then EQRow("EQ02_3") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else EQRow("EQ02_3") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then EQRow("EQ02_4") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else EQRow("EQ02_4") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then EQRow("EQ02_5") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else EQRow("EQ02_5") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then EQRow("EQ02_6") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else EQRow("EQ02_6") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then EQRow("EQ02_7") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else EQRow("EQ02_7") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) <> "") Then EQRow("EQ03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) Else EQRow("EQ03") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 21) <> "") Then EQRow("EQ04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) Else EQRow("EQ04") = DBNull.Value


                        EQRow("BATCH_ID") = oD._BatchID
                        EQRow("TIME_STAMP") = oD.TimeStamp
                        EQ.Rows.Add(EQRow)

                        oD.RowProcessedFlag = 1


                        'od.STIdentity = @@IDENTITY;  
                        ' --od.STFlag = 1;  
                        oD.RowProcessedFlag = 1

                    End If
                    If oD.EBCarrotFlag = 1 Then

                        oD.isDirtyCACHEEQ = 1
                        oD.isDirtyMasterEQ = 1



                        Dim CEQRow As DataRow = CACHE_EQ.NewRow
                        CEQRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                        CEQRow("HIPAA_GS_GUID") = oD.GS_GUID
                        CEQRow("HIPAA_ST_GUID") = oD.ST_GUID
                        CEQRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                        CEQRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                        CEQRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                        CEQRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                        CEQRow("HIPAA_EB_GUID") = oD.EB_GUID
                        CEQRow("P_GUID") = oD.P_GUID
                        CEQRow("HL_PARENT") = oD.CurrentHLGroup









                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then CEQRow("EQ01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else CEQRow("EQ01") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then CEQRow("EQ02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else CEQRow("EQ02") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then CEQRow("EQ02_1") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else CEQRow("EQ02_1") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then CEQRow("EQ02_2") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else CEQRow("EQ02_2") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then CEQRow("EQ02_3") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else CEQRow("EQ02_3") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then CEQRow("EQ02_4") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else CEQRow("EQ02_4") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then CEQRow("EQ02_5") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else CEQRow("EQ02_5") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then CEQRow("EQ02_6") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else CEQRow("EQ02_6") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then CEQRow("EQ02_7") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else CEQRow("EQ02_7") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) <> "") Then CEQRow("EQ03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) Else CEQRow("EQ03") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) <> "") Then CEQRow("EQ04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) Else CEQRow("EQ04") = DBNull.Value


                        CEQRow("BATCH_ID") = oD._BatchID
                        CEQRow("TIME_STAMP") = oD.TimeStamp
                        CACHE_EQ.Rows.Add(CEQRow)

                        oD.RowProcessedFlag = 1

                    End If

                End If
                'END EQ



                'BEGIN HSD
                If oD.ROW_RECORD_TYPE = "HSD" Then
                    'BEGIN()
                    If oD.EBCarrotFlag = 0 Then
                        'BEGIN()
                        oD.isDirtyHSD = 1
                        oD.isDirtyMasterHSD = 1


                        Dim HSDRow As DataRow = HSD.NewRow
                        HSDRow("ROW_ID") = oD.EBcount
                        HSDRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                        HSDRow("HIPAA_GS_GUID") = oD.GS_GUID
                        HSDRow("HIPAA_ST_GUID") = oD.ST_GUID
                        HSDRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                        HSDRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                        HSDRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                        HSDRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                        HSDRow("HIPAA_EB_GUID") = oD.EB_GUID
                        HSDRow("P_GUID") = oD.P_GUID
                        HSDRow("HL_PARENT") = oD.CurrentHLGroup


                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then HSDRow("HSD01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else HSDRow("HSD01") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then HSDRow("HSD02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else HSDRow("HSD02") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then HSDRow("HSD03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else HSDRow("HSD03") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then HSDRow("HSD04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else HSDRow("HSD04") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then HSDRow("HSD05") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else HSDRow("HSD05") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then HSDRow("HSD06") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else HSDRow("HSD06") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then HSDRow("HSD07") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else HSDRow("HSD07") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then HSDRow("HSD08") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else HSDRow("HSD08") = DBNull.Value



                        'HSDRow("HSD01") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 2)
                        'HSDRow("HSD02") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 3)
                        'HSDRow("HSD03") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 4)
                        'HSDRow("HSD04") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 5)
                        'HSDRow("HSD05") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 6)
                        'HSDRow("HSD06") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 7)
                        'HSDRow("HSD07") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 8)
                        'HSDRow("HSD08") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 9)
                        HSDRow("BATCH_ID") = oD._BatchID
                        HSDRow("TIME_STAMP") = oD.TimeStamp
                        HSD.Rows.Add(HSDRow)

                        oD.RowProcessedFlag = 1


                        'od.STIdentity = @@IDENTITY;  
                        ' --od.STFlag = 1;  
                        oD.RowProcessedFlag = 1

                    End If
                    If oD.EBCarrotFlag = 1 Then

                        oD.isDirtyCACHEHSD = 1
                        oD.isDirtyMasterHSD = 1



                        Dim CHSDRow As DataRow = CACHE_HSD.NewRow
                        ' oD.EBcount

                        CHSDRow("ROW_ID") = oD.EBcount
                        CHSDRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                        CHSDRow("HIPAA_GS_GUID") = oD.GS_GUID
                        CHSDRow("HIPAA_ST_GUID") = oD.ST_GUID
                        CHSDRow("HIPAA_HL01_GUID") = oD.HL20_GUID()
                        CHSDRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                        CHSDRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                        CHSDRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                        CHSDRow("HIPAA_EB_GUID") = oD.EB_GUID
                        CHSDRow("P_GUID") = oD.P_GUID
                        CHSDRow("HL_PARENT") = oD.CurrentHLGroup


                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then CHSDRow("HSD01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else CHSDRow("HSD01") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then CHSDRow("HSD02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else CHSDRow("HSD02") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then CHSDRow("HSD03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else CHSDRow("HSD03") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then CHSDRow("HSD04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else CHSDRow("HSD04") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then CHSDRow("HSD05") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else CHSDRow("HSD05") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then CHSDRow("HSD06") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else CHSDRow("HSD06") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then CHSDRow("HSD07") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else CHSDRow("HSD07") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then CHSDRow("HSD08") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else CHSDRow("HSD08") = DBNull.Value

                        CHSDRow("BATCH_ID") = oD._BatchID
                        CHSDRow("TIME_STAMP") = oD.TimeStamp
                        CACHE_HSD.Rows.Add(CHSDRow)

                        oD.RowProcessedFlag = 1

                    End If

                End If
                'END 


                'BEGIN HL
                If oD.ROW_RECORD_TYPE = "HL" Then
                    'BEGIN()
                    If oD.EBCarrotFlag = 0 Then
                        'BEGIN()
                        oD.isDirtyHL = 1
                        oD.isDirtyMasterHL = 1


                        Dim HLRow As DataRow = HL.NewRow
                        HLRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                        HLRow("HIPAA_GS_GUID") = oD.GS_GUID
                        HLRow("HIPAA_ST_GUID") = oD.ST_GUID
                        HLRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                        HLRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                        HLRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                        HLRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                        HLRow("HIPAA_EB_GUID") = oD.EB_GUID
                        HLRow("P_GUID") = oD.P_GUID
                        HLRow("HL_PARENT") = oD.CurrentHLGroup

                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then HLRow("HL01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else HLRow("HL01") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then HLRow("HL02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else HLRow("HL02") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then HLRow("HL03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else HLRow("HL03") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then HLRow("HL04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else HLRow("HL04") = DBNull.Value


                        HLRow("BATCH_ID") = oD._BatchID
                        HLRow("TIME_STAMP") = oD.TimeStamp
                        HL.Rows.Add(HLRow)

                        oD.RowProcessedFlag = 1


                        'od.STIdentity = @@IDENTITY;  
                        ' --od.STFlag = 1;  
                        oD.RowProcessedFlag = 1

                    End If
                    If oD.EBCarrotFlag = 1 Then

                        oD.isDirtyCACHEHL = 1
                        oD.isDirtyMasterHL = 1



                        Dim CHLRow As DataRow = CACHE_HL.NewRow
                        CHLRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                        CHLRow("HIPAA_GS_GUID") = oD.GS_GUID
                        CHLRow("HIPAA_ST_GUID") = oD.ST_GUID
                        CHLRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                        CHLRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                        CHLRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                        CHLRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                        CHLRow("HIPAA_EB_GUID") = oD.EB_GUID
                        CHLRow("P_GUID") = oD.P_GUID
                        CHLRow("HL_PARENT") = oD.CurrentHLGroup


                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then CHLRow("HL01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else CHLRow("HL01") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then CHLRow("HL02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else CHLRow("HL02") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then CHLRow("HL03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else CHLRow("HL03") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then CHLRow("HL04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else CHLRow("HL04") = DBNull.Value




                        CHLRow("BATCH_ID") = oD._BatchID
                        CHLRow("TIME_STAMP") = oD.TimeStamp
                        CACHE_HL.Rows.Add(CHLRow)

                        oD.RowProcessedFlag = 1

                    End If

                End If
                'END HL



                'BEGIN III
                If oD.ROW_RECORD_TYPE = "III" Then
                    'BEGIN()
                    If oD.EBCarrotFlag = 0 Then
                        'BEGIN()
                        oD.isDirtyIII = 1
                        oD.isDirtyMasterIII = 1


                        Dim IIIRow As DataRow = III.NewRow
                        IIIRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                        IIIRow("HIPAA_GS_GUID") = oD.GS_GUID
                        IIIRow("HIPAA_ST_GUID") = oD.ST_GUID
                        IIIRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                        IIIRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                        IIIRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                        IIIRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                        IIIRow("HIPAA_EB_GUID") = oD.EB_GUID
                        IIIRow("P_GUID") = oD.P_GUID
                        IIIRow("HL_PARENT") = oD.CurrentHLGroup

                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then IIIRow("III01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else IIIRow("III01") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then IIIRow("III02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else IIIRow("III02") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then IIIRow("III03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else IIIRow("III03") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then IIIRow("III04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else IIIRow("III04") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then IIIRow("III05") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else IIIRow("III05") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then IIIRow("III06") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else IIIRow("III06") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then IIIRow("III07") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else IIIRow("III07") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then IIIRow("III08") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else IIIRow("III08") = DBNull.Value



                        IIIRow("BATCH_ID") = oD._BatchID
                        IIIRow("TIME_STAMP") = oD.TimeStamp
                        III.Rows.Add(IIIRow)

                        oD.RowProcessedFlag = 1


                        'od.STIdentity = @@IDENTITY;  
                        ' --od.STFlag = 1;  
                        oD.RowProcessedFlag = 1

                    End If
                    If oD.EBCarrotFlag = 1 Then

                        oD.isDirtyCACHEIII = 1
                        oD.isDirtyMasterIII = 1



                        Dim CIIIRow As DataRow = CACHE_III.NewRow
                        CIIIRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                        CIIIRow("HIPAA_GS_GUID") = oD.GS_GUID
                        CIIIRow("HIPAA_ST_GUID") = oD.ST_GUID
                        CIIIRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                        CIIIRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                        CIIIRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                        CIIIRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                        CIIIRow("HIPAA_EB_GUID") = oD.EB_GUID
                        CIIIRow("P_GUID") = oD.P_GUID
                        CIIIRow("HL_PARENT") = oD.CurrentHLGroup

                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then CIIIRow("III01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else CIIIRow("III01") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then CIIIRow("III02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else CIIIRow("III02") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then CIIIRow("III03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else CIIIRow("III03") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then CIIIRow("III04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else CIIIRow("III04") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then CIIIRow("III05") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else CIIIRow("III05") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then CIIIRow("III06") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else CIIIRow("III06") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then CIIIRow("III07") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else CIIIRow("III07") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then CIIIRow("III08") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else CIIIRow("III08") = DBNull.Value





                        CIIIRow("BATCH_ID") = oD._BatchID
                        CIIIRow("TIME_STAMP") = oD.TimeStamp
                        CACHE_III.Rows.Add(CIIIRow)

                        oD.RowProcessedFlag = 1

                    End If

                End If
                'END III





                'BEGIN INS
                If oD.ROW_RECORD_TYPE = "INS" Then
                    'BEGIN()
                    If oD.EBCarrotFlag = 0 Then
                        'BEGIN()
                        oD.isDirtyINS = 1
                        oD.isDirtyMasterINS = 1


                        Dim INSRow As DataRow = INS.NewRow
                        INSRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                        INSRow("HIPAA_GS_GUID") = oD.GS_GUID
                        INSRow("HIPAA_ST_GUID") = oD.ST_GUID
                        INSRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                        INSRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                        INSRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                        INSRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                        INSRow("HIPAA_EB_GUID") = oD.EB_GUID
                        INSRow("P_GUID") = oD.P_GUID
                        INSRow("HL_PARENT") = oD.CurrentHLGroup


                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then INSRow("INS01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else INSRow("INS01") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then INSRow("INS02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else INSRow("INS02") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then INSRow("INS03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else INSRow("INS03") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then INSRow("INS04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else INSRow("INS04") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then INSRow("INS05") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else INSRow("INS05") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then INSRow("INS06") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else INSRow("INS06") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then INSRow("INS07") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else INSRow("INS07") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then INSRow("INS08") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else INSRow("INS08") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then INSRow("INS09") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else INSRow("INS09") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) <> "") Then INSRow("INS10") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) Else INSRow("INS10") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) <> "") Then INSRow("INS11") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) Else INSRow("INS11") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) <> "") Then INSRow("INS12") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) Else INSRow("INS12") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) <> "") Then INSRow("INS13") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) Else INSRow("INS13") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 15) <> "") Then INSRow("INS14") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 15) Else INSRow("INS14") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 16) <> "") Then INSRow("INS15") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 16) Else INSRow("INS15") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 17) <> "") Then INSRow("INS16") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 17) Else INSRow("INS16") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 18) <> "") Then INSRow("INS17") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 18) Else INSRow("INS17") = DBNull.Value




                        INSRow("BATCH_ID") = oD._BatchID
                        INSRow("TIME_STAMP") = oD.TimeStamp
                        INS.Rows.Add(INSRow)

                        oD.RowProcessedFlag = 1


                        'od.STIdentity = @@IDENTITY;  
                        ' --od.STFlag = 1;  
                        oD.RowProcessedFlag = 1

                    End If
                    If oD.EBCarrotFlag = 1 Then

                        oD.isDirtyCACHEINS = 1
                        oD.isDirtyMasterINS = 1



                        Dim CINSRow As DataRow = CACHE_INS.NewRow
                        CINSRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                        CINSRow("HIPAA_GS_GUID") = oD.GS_GUID
                        CINSRow("HIPAA_ST_GUID") = oD.ST_GUID
                        CINSRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                        CINSRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                        CINSRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                        CINSRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                        CINSRow("HIPAA_EB_GUID") = oD.EB_GUID
                        CINSRow("P_GUID") = oD.P_GUID
                        CINSRow("HL_PARENT") = oD.CurrentHLGroup


                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then CINSRow("INS01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else CINSRow("INS01") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then CINSRow("INS02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else CINSRow("INS02") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then CINSRow("INS03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else CINSRow("INS03") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then CINSRow("INS04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else CINSRow("INS04") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then CINSRow("INS05") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else CINSRow("INS05") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then CINSRow("INS06") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else CINSRow("INS06") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then CINSRow("INS07") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else CINSRow("INS07") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then CINSRow("INS08") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else CINSRow("INS08") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then CINSRow("INS09") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else CINSRow("INS09") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) <> "") Then CINSRow("INS10") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) Else CINSRow("INS10") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) <> "") Then CINSRow("INS11") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) Else CINSRow("INS11") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) <> "") Then CINSRow("INS12") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) Else CINSRow("INS12") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) <> "") Then CINSRow("INS13") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) Else CINSRow("INS13") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 15) <> "") Then CINSRow("INS14") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 15) Else CINSRow("INS14") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 16) <> "") Then CINSRow("INS15") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 16) Else CINSRow("INS15") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 17) <> "") Then CINSRow("INS16") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 17) Else CINSRow("INS16") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 18) <> "") Then CINSRow("INS17") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 18) Else CINSRow("INS17") = DBNull.Value






                        CINSRow("BATCH_ID") = oD._BatchID
                        CINSRow("TIME_STAMP") = oD.TimeStamp
                        CACHE_INS.Rows.Add(CINSRow)

                        oD.RowProcessedFlag = 1

                    End If

                End If
                'END INS




                'BEGIN MSG
                If oD.ROW_RECORD_TYPE = "MSG" Then
                    'BEGIN()
                    If oD.EBCarrotFlag = 0 Then
                        'BEGIN()
                        oD.isDirtyMSG = 1
                        oD.isDirtyMasterMSG = 1


                        Dim MSGRow As DataRow = MSG.NewRow
                        MSGRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                        MSGRow("HIPAA_GS_GUID") = oD.GS_GUID
                        MSGRow("HIPAA_ST_GUID") = oD.ST_GUID
                        MSGRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                        MSGRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                        MSGRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                        MSGRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                        MSGRow("HIPAA_EB_GUID") = oD.EB_GUID
                        MSGRow("P_GUID") = oD.P_GUID
                        MSGRow("HL_PARENT") = oD.CurrentHLGroup





                        MSGRow("MSG01") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)
                        MSGRow("MSG02") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                        MSGRow("MSG03") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)
                        MSGRow("MSG04") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 5)
                        MSGRow("MSG05") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 6)
                        MSGRow("MSG06") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 7)
                        MSGRow("MSG07") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 8)






                        MSGRow("BATCH_ID") = oD._BatchID
                        MSGRow("TIME_STAMP") = oD.TimeStamp
                        MSG.Rows.Add(MSGRow)

                        oD.RowProcessedFlag = 1


                        'od.STIdentity = @@IDENTITY;  
                        ' --od.STFlag = 1;  
                        oD.RowProcessedFlag = 1

                    End If
                    If oD.EBCarrotFlag = 1 Then

                        oD.isDirtyCACHEMSG = 1
                        oD.isDirtyMasterMSG = 1



                        Dim CMSGRow As DataRow = CACHE_MSG.NewRow
                        CMSGRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                        CMSGRow("HIPAA_GS_GUID") = oD.GS_GUID
                        CMSGRow("HIPAA_ST_GUID") = oD.ST_GUID
                        CMSGRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                        CMSGRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                        CMSGRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                        CMSGRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                        CMSGRow("HIPAA_EB_GUID") = oD.EB_GUID
                        CMSGRow("P_GUID") = oD.P_GUID
                        CMSGRow("HL_PARENT") = oD.CurrentHLGroup
                        CMSGRow("MSG01") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)
                        CMSGRow("MSG02") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                        CMSGRow("MSG03") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)
                        CMSGRow("MSG04") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 5)
                        CMSGRow("MSG05") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 6)
                        CMSGRow("MSG06") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 7)
                        CMSGRow("MSG07") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 8)
                        CMSGRow("BATCH_ID") = oD._BatchID
                        CMSGRow("TIME_STAMP") = oD.TimeStamp
                        CACHE_MSG.Rows.Add(CMSGRow)

                        oD.RowProcessedFlag = 1

                    End If

                End If
                'END MSG





                'BEGIN N3
                If oD.ROW_RECORD_TYPE = "N3" Then
                    'BEGIN()
                    If oD.EBCarrotFlag = 0 Then
                        'BEGIN()
                        oD.isDirtyN3 = 1
                        oD.isDirtyMasterN3 = 1


                        Dim N3Row As DataRow = N3.NewRow
                        N3Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                        N3Row("HIPAA_GS_GUID") = oD.GS_GUID
                        N3Row("HIPAA_ST_GUID") = oD.ST_GUID
                        N3Row("HIPAA_HL01_GUID") = oD.HL20_GUID
                        N3Row("HIPAA_HL02_GUID") = oD.HL21_GUID
                        N3Row("HIPAA_HL03_GUID") = oD.HL22_GUID
                        N3Row("HIPAA_HL04_GUID") = oD.HL23_GUID
                        N3Row("HIPAA_EB_GUID") = oD.EB_GUID
                        N3Row("P_GUID") = oD.P_GUID
                        N3Row("HL_PARENT") = oD.CurrentHLGroup



                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then N3Row("N301") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else N3Row("N301") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then N3Row("N302") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else N3Row("N302") = DBNull.Value



                        N3Row("BATCH_ID") = oD._BatchID
                        N3Row("TIME_STAMP") = oD.TimeStamp
                        N3.Rows.Add(N3Row)

                        oD.RowProcessedFlag = 1


                        'od.STIdentity = @@IDENTITY;  
                        ' --od.STFlag = 1;  
                        oD.RowProcessedFlag = 1

                    End If
                    If oD.EBCarrotFlag = 1 Then

                        oD.isDirtyCACHEN3 = 1
                        oD.isDirtyMasterN3 = 1



                        Dim CN3Row As DataRow = CACHE_N3.NewRow
                        CN3Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                        CN3Row("HIPAA_GS_GUID") = oD.GS_GUID
                        CN3Row("HIPAA_ST_GUID") = oD.ST_GUID
                        CN3Row("HIPAA_HL01_GUID") = oD.HL20_GUID
                        CN3Row("HIPAA_HL02_GUID") = oD.HL21_GUID
                        CN3Row("HIPAA_HL03_GUID") = oD.HL22_GUID
                        CN3Row("HIPAA_HL04_GUID") = oD.HL23_GUID
                        CN3Row("HIPAA_EB_GUID") = oD.EB_GUID
                        CN3Row("P_GUID") = oD.P_GUID
                        CN3Row("HL_PARENT") = oD.CurrentHLGroup


                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then CN3Row("N301") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else CN3Row("N301") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then CN3Row("N302") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else CN3Row("N302") = DBNull.Value


                        CN3Row("BATCH_ID") = oD._BatchID
                        CN3Row("TIME_STAMP") = oD.TimeStamp
                        CACHE_N3.Rows.Add(CN3Row)

                        oD.RowProcessedFlag = 1

                    End If

                End If
                'END N3

                'BEGIN N4
                If oD.ROW_RECORD_TYPE = "N4" Then
                    'BEGIN()
                    If oD.EBCarrotFlag = 0 Then
                        'BEGIN()
                        oD.isDirtyN4 = 1
                        oD.isDirtyMasterN4 = 1


                        Dim N4Row As DataRow = N4.NewRow
                        N4Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                        N4Row("HIPAA_GS_GUID") = oD.GS_GUID
                        N4Row("HIPAA_ST_GUID") = oD.ST_GUID
                        N4Row("HIPAA_HL01_GUID") = oD.HL20_GUID
                        N4Row("HIPAA_HL02_GUID") = oD.HL21_GUID
                        N4Row("HIPAA_HL03_GUID") = oD.HL22_GUID
                        N4Row("HIPAA_HL04_GUID") = oD.HL23_GUID
                        N4Row("HIPAA_EB_GUID") = oD.EB_GUID
                        N4Row("P_GUID") = oD.P_GUID
                        N4Row("HL_PARENT") = oD.CurrentHLGroup



                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then N4Row("N401") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else N4Row("N401") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then N4Row("N402") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else N4Row("N402") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then N4Row("N403") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else N4Row("N403") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then N4Row("N404") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else N4Row("N404") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then N4Row("N405") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else N4Row("N405") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then N4Row("N406") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else N4Row("N406") = DBNull.Value





                        N4Row("BATCH_ID") = oD._BatchID
                        N4Row("TIME_STAMP") = oD.TimeStamp
                        N4.Rows.Add(N4Row)

                        oD.RowProcessedFlag = 1


                        'od.STIdentity = @@IDENTITY;  
                        ' --od.STFlag = 1;  
                        oD.RowProcessedFlag = 1

                    End If
                    If oD.EBCarrotFlag = 1 Then

                        oD.isDirtyCACHEN4 = 1
                        oD.isDirtyMasterN4 = 1



                        Dim CN4Row As DataRow = CACHE_N4.NewRow
                        CN4Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                        CN4Row("HIPAA_GS_GUID") = oD.GS_GUID
                        CN4Row("HIPAA_ST_GUID") = oD.ST_GUID
                        CN4Row("HIPAA_HL01_GUID") = oD.HL20_GUID
                        CN4Row("HIPAA_HL02_GUID") = oD.HL21_GUID
                        CN4Row("HIPAA_HL03_GUID") = oD.HL22_GUID
                        CN4Row("HIPAA_HL04_GUID") = oD.HL23_GUID
                        CN4Row("HIPAA_EB_GUID") = oD.EB_GUID
                        CN4Row("P_GUID") = oD.P_GUID
                        CN4Row("HL_PARENT") = oD.CurrentHLGroup


                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then CN4Row("N401") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else CN4Row("N401") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then CN4Row("N402") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else CN4Row("N402") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then CN4Row("N403") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else CN4Row("N403") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then CN4Row("N404") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else CN4Row("N404") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then CN4Row("N405") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else CN4Row("N405") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then CN4Row("N406") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else CN4Row("N406") = DBNull.Value



                        CN4Row("BATCH_ID") = oD._BatchID
                        CN4Row("TIME_STAMP") = oD.TimeStamp
                        CACHE_N4.Rows.Add(CN4Row)

                        oD.RowProcessedFlag = 1

                    End If

                End If
                'END N4


                'BEGIN NM1
                If oD.ROW_RECORD_TYPE = "NM1" Then
                    'BEGIN()




                    oD._1p = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)


                    If (oD._1p = "1P" Or oD._1p = "80" Or oD._1p = "FA") Then

                        oD.NPI = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 10)
                    End If




                    If oD.EBCarrotFlag = 0 Then
                        'BEGIN()
                        oD.isDirtyNM1 = 1
                        oD.isDirtyMasterNM1 = 1


                        Dim NM1Row As DataRow = NM1.NewRow
                        NM1Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                        NM1Row("HIPAA_GS_GUID") = oD.GS_GUID
                        NM1Row("HIPAA_ST_GUID") = oD.ST_GUID
                        NM1Row("HIPAA_HL01_GUID") = oD.HL20_GUID
                        NM1Row("HIPAA_HL02_GUID") = oD.HL21_GUID
                        NM1Row("HIPAA_HL03_GUID") = oD.HL22_GUID
                        NM1Row("HIPAA_HL04_GUID") = oD.HL23_GUID
                        NM1Row("HIPAA_EB_GUID") = oD.EB_GUID
                        NM1Row("P_GUID") = oD.P_GUID
                        NM1Row("HL_PARENT") = oD.CurrentHLGroup


                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then NM1Row("NM101") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else NM1Row("NM101") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then NM1Row("NM102") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else NM1Row("NM102") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then NM1Row("NM103") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else NM1Row("NM103") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then NM1Row("NM104") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else NM1Row("NM104") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then NM1Row("NM105") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else NM1Row("NM105") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then NM1Row("NM106") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else NM1Row("NM106") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then NM1Row("NM107") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else NM1Row("NM107") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then NM1Row("NM108") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else NM1Row("NM108") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then NM1Row("NM109") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else NM1Row("NM109") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) <> "") Then NM1Row("NM110") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) Else NM1Row("NM110") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) <> "") Then NM1Row("NM111") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) Else NM1Row("NM111") = DBNull.Value



                        NM1Row("BATCH_ID") = oD._BatchID
                        NM1Row("TIME_STAMP") = oD.TimeStamp
                        NM1.Rows.Add(NM1Row)

                        oD.RowProcessedFlag = 1


                        'od.STIdentity = @@IDENTITY;  
                        ' --od.STFlag = 1;  
                        oD.RowProcessedFlag = 1

                    End If
                    If oD.EBCarrotFlag = 1 Then

                        oD.isDirtyCACHENM1 = 1
                        oD.isDirtyMasterNM1 = 1



                        Dim CNM1Row As DataRow = CACHE_NM1.NewRow
                        CNM1Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                        CNM1Row("HIPAA_GS_GUID") = oD.GS_GUID
                        CNM1Row("HIPAA_ST_GUID") = oD.ST_GUID
                        CNM1Row("HIPAA_HL01_GUID") = oD.HL20_GUID
                        CNM1Row("HIPAA_HL02_GUID") = oD.HL21_GUID
                        CNM1Row("HIPAA_HL03_GUID") = oD.HL22_GUID
                        CNM1Row("HIPAA_HL04_GUID") = oD.HL23_GUID
                        CNM1Row("HIPAA_EB_GUID") = oD.EB_GUID
                        CNM1Row("P_GUID") = oD.P_GUID
                        CNM1Row("HL_PARENT") = oD.CurrentHLGroup
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then CNM1Row("NM101") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else CNM1Row("NM101") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then CNM1Row("NM102") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else CNM1Row("NM102") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then CNM1Row("NM103") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else CNM1Row("NM103") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then CNM1Row("NM104") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else CNM1Row("NM104") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then CNM1Row("NM105") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else CNM1Row("NM105") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then CNM1Row("NM106") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else CNM1Row("NM106") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then CNM1Row("NM107") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else CNM1Row("NM107") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then CNM1Row("NM108") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else CNM1Row("NM108") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then CNM1Row("NM109") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else CNM1Row("NM109") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) <> "") Then CNM1Row("NM110") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) Else CNM1Row("NM110") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) <> "") Then CNM1Row("NM111") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) Else CNM1Row("NM111") = DBNull.Value


                        CNM1Row("BATCH_ID") = oD._BatchID
                        CNM1Row("TIME_STAMP") = oD.TimeStamp
                        CACHE_NM1.Rows.Add(CNM1Row)

                        oD.RowProcessedFlag = 1

                    End If

                End If
                'END NM1


                'BEGIN PER
                If oD.ROW_RECORD_TYPE = "PER" Then
                    'BEGIN()
                    If oD.EBCarrotFlag = 0 Then
                        'BEGIN()
                        oD.isDirtyPER = 1
                        oD.isDirtyMasterPER = 1




                        Dim PERRow As DataRow = PER.NewRow
                        PERRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                        PERRow("HIPAA_GS_GUID") = oD.GS_GUID
                        PERRow("HIPAA_ST_GUID") = oD.ST_GUID
                        PERRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                        PERRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                        PERRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                        PERRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                        PERRow("HIPAA_EB_GUID") = oD.EB_GUID
                        PERRow("P_GUID") = oD.P_GUID
                        PERRow("HL_PARENT") = oD.CurrentHLGroup


                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then PERRow("PER01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else PERRow("PER01") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then PERRow("PER02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else PERRow("PER02") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then PERRow("PER03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else PERRow("PER03") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then PERRow("PER04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else PERRow("PER04") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then PERRow("PER05") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else PERRow("PER05") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then PERRow("PER06") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else PERRow("PER06") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then PERRow("PER07") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else PERRow("PER07") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then PERRow("PER08") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else PERRow("PER08") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then PERRow("PER09") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else PERRow("PER09") = DBNull.Value



                        PERRow("BATCH_ID") = oD._BatchID
                        PERRow("TIME_STAMP") = oD.TimeStamp
                        PER.Rows.Add(PERRow)

                        oD.RowProcessedFlag = 1


                        'od.STIdentity = @@IDENTITY;  
                        ' --od.STFlag = 1;  
                        oD.RowProcessedFlag = 1

                    End If
                    If oD.EBCarrotFlag = 1 Then

                        oD.isDirtyCACHEPER = 1
                        oD.isDirtyMasterPER = 1



                        Dim CPERRow As DataRow = CACHE_PER.NewRow
                        CPERRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                        CPERRow("HIPAA_GS_GUID") = oD.GS_GUID
                        CPERRow("HIPAA_ST_GUID") = oD.ST_GUID
                        CPERRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                        CPERRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                        CPERRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                        CPERRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                        CPERRow("HIPAA_EB_GUID") = oD.EB_GUID
                        CPERRow("P_GUID") = oD.P_GUID
                        CPERRow("HL_PARENT") = oD.CurrentHLGroup



                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then CPERRow("PER01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else CPERRow("PER01") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then CPERRow("PER02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else CPERRow("PER02") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then CPERRow("PER03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else CPERRow("PER03") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then CPERRow("PER04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else CPERRow("PER04") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then CPERRow("PER05") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else CPERRow("PER05") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then CPERRow("PER06") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else CPERRow("PER06") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then CPERRow("PER07") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else CPERRow("PER07") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then CPERRow("PER08") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else CPERRow("PER08") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then CPERRow("PER09") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else CPERRow("PER09") = DBNull.Value


                        CPERRow("BATCH_ID") = oD._BatchID
                        CPERRow("TIME_STAMP") = oD.TimeStamp
                        CACHE_PER.Rows.Add(CPERRow)

                        oD.RowProcessedFlag = 1

                    End If

                End If
                'END PER






                'BEGIN PER
                If oD.ROW_RECORD_TYPE = "PRV" Then
                    'BEGIN()
                    If oD.EBCarrotFlag = 0 Then
                        'BEGIN()
                        oD.isDirtyPER = 1
                        oD.isDirtyMasterPER = 1




                        Dim PRVRow As DataRow = PRV.NewRow
                        PRVRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                        PRVRow("HIPAA_GS_GUID") = oD.GS_GUID
                        PRVRow("HIPAA_ST_GUID") = oD.ST_GUID
                        PRVRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                        PRVRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                        PRVRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                        PRVRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                        PRVRow("HIPAA_EB_GUID") = oD.EB_GUID
                        PRVRow("P_GUID") = oD.P_GUID
                        PRVRow("HL_PARENT") = oD.CurrentHLGroup




                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then PRVRow("PRV01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else PRVRow("PRV01") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then PRVRow("PRV02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else PRVRow("PRV02") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then PRVRow("PRV03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else PRVRow("PRV03") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then PRVRow("PRV04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else PRVRow("PRV04") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then PRVRow("PRV05") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else PRVRow("PRV05") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then PRVRow("PRV06") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else PRVRow("PRV06") = DBNull.Value



                        PRVRow("BATCH_ID") = oD._BatchID
                        PRVRow("TIME_STAMP") = oD.TimeStamp
                        PRV.Rows.Add(PRVRow)

                        oD.RowProcessedFlag = 1


                        'od.STIdentity = @@IDENTITY;  
                        ' --od.STFlag = 1;  
                        oD.RowProcessedFlag = 1

                    End If
                    If oD.EBCarrotFlag = 1 Then

                        oD.isDirtyCACHEPER = 1
                        oD.isDirtyMasterPER = 1



                        Dim CPRVRow As DataRow = CACHE_PRV.NewRow
                        CPRVRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                        CPRVRow("HIPAA_GS_GUID") = oD.GS_GUID
                        CPRVRow("HIPAA_ST_GUID") = oD.ST_GUID
                        CPRVRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                        CPRVRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                        CPRVRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                        CPRVRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                        CPRVRow("HIPAA_EB_GUID") = oD.EB_GUID
                        CPRVRow("P_GUID") = oD.P_GUID
                        CPRVRow("HL_PARENT") = oD.CurrentHLGroup


                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then CPRVRow("PRV01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else CPRVRow("PRV01") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then CPRVRow("PRV02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else CPRVRow("PRV02") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then CPRVRow("PRV03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else CPRVRow("PRV03") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then CPRVRow("PRV04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else CPRVRow("PRV04") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then CPRVRow("PRV05") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else CPRVRow("PRV05") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then CPRVRow("PRV06") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else CPRVRow("PRV06") = DBNull.Value


                        'CPRVRow("PER01") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 2)
                        'CPRVRow("PER02") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 3)
                        'CPRVRow("PER03") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 4)
                        'CPRVRow("PER04") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 5)
                        'CPRVRow("PER05") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 6)
                        'CPRVRow("PER06") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 7)
                        CPRVRow("BATCH_ID") = oD._BatchID
                        CPRVRow("TIME_STAMP") = oD.TimeStamp
                        CACHE_PRV.Rows.Add(CPRVRow)

                        oD.RowProcessedFlag = 1

                    End If

                End If
                'END PER








                'BEGIN REF
                If oD.ROW_RECORD_TYPE = "REF" Then
                    'BEGIN()
                    If oD.EBCarrotFlag = 0 Then
                        'BEGIN()
                        oD.isDirtyREF = 1
                        oD.isDirtyMasterREF = 1




                        Dim REFRow As DataRow = REF.NewRow
                        REFRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                        REFRow("HIPAA_GS_GUID") = oD.GS_GUID
                        REFRow("HIPAA_ST_GUID") = oD.ST_GUID
                        REFRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                        REFRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                        REFRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                        REFRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                        REFRow("HIPAA_EB_GUID") = oD.EB_GUID
                        REFRow("P_GUID") = oD.P_GUID
                        REFRow("HL_PARENT") = oD.CurrentHLGroup



                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then REFRow("REF01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else REFRow("REF01") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then REFRow("REF02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else REFRow("REF02") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then REFRow("REF03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else REFRow("REF03") = DBNull.Value


                        'REFRow("REF01") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 2)
                        'REFRow("REF02") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 3)
                        'REFRow("REF03") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 4)
                        ' REFRow("REF04") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 5) ' fix the last ^ thinmg
                        REFRow("BATCH_ID") = oD._BatchID
                        REFRow("TIME_STAMP") = oD.TimeStamp
                        REF.Rows.Add(REFRow)

                        oD.RowProcessedFlag = 1


                        'od.STIdentity = @@IDENTITY;  
                        ' --od.STFlag = 1;  
                        oD.RowProcessedFlag = 1

                    End If
                    If oD.EBCarrotFlag = 1 Then

                        oD.isDirtyCACHEREF = 1
                        oD.isDirtyMasterREF = 1



                        Dim CREFRow As DataRow = CACHE_REF.NewRow
                        CREFRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                        CREFRow("HIPAA_GS_GUID") = oD.GS_GUID
                        CREFRow("HIPAA_ST_GUID") = oD.ST_GUID
                        CREFRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                        CREFRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                        CREFRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                        CREFRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                        CREFRow("HIPAA_EB_GUID") = oD.EB_GUID
                        CREFRow("P_GUID") = oD.P_GUID
                        CREFRow("HL_PARENT") = oD.CurrentHLGroup



                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then CREFRow("REF01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else CREFRow("REF01") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then CREFRow("REF02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else CREFRow("REF02") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then CREFRow("REF03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else CREFRow("REF03") = DBNull.Value


                        'CREFRow("REF01") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 2)
                        'CREFRow("REF02") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 3)
                        'CREFRow("REF03") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 4)
                        ' CREFRow("REF04") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 5) ' fix the last ^ thinmg
                        CREFRow("BATCH_ID") = oD._BatchID
                        CREFRow("TIME_STAMP") = oD.TimeStamp
                        CACHE_REF.Rows.Add(CREFRow)

                        oD.RowProcessedFlag = 1

                    End If


                End If
                'END REF












                'BEGIN TRN
                If oD.ROW_RECORD_TYPE = "TRN" Then
                    'BEGIN()
                    If oD.EBCarrotFlag = 0 Then
                        'BEGIN()
                        oD.isDirtyTRN = 1
                        oD.isDirtyMasterTRN = 1




                        Dim TRNRow As DataRow = TRN.NewRow
                        TRNRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                        TRNRow("HIPAA_GS_GUID") = oD.GS_GUID
                        TRNRow("HIPAA_ST_GUID") = oD.ST_GUID
                        TRNRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                        TRNRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                        TRNRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                        TRNRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                        TRNRow("HIPAA_EB_GUID") = oD.EB_GUID
                        TRNRow("P_GUID") = oD.P_GUID
                        TRNRow("HL_PARENT") = oD.CurrentHLGroup


                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then TRNRow("TRN01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else TRNRow("TRN01") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then TRNRow("TRN02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else TRNRow("TRN02") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then TRNRow("TRN03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else TRNRow("TRN03") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then TRNRow("TRN04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else TRNRow("TRN04") = DBNull.Value


                        'TRNRow("TRN01") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 2)
                        'TRNRow("TRN02") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 3)
                        'TRNRow("TRN03") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 4)
                        'TRNRow("TRN04") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 5) ' fix the last ^ thinmg
                        TRNRow("BATCH_ID") = oD._BatchID
                        TRNRow("TIME_STAMP") = oD.TimeStamp
                        TRN.Rows.Add(TRNRow)

                        oD.RowProcessedFlag = 1


                        'od.STIdentity = @@IDENTITY;  
                        ' --od.STFlag = 1;  
                        oD.RowProcessedFlag = 1

                    End If
                    If oD.EBCarrotFlag = 1 Then

                        oD.isDirtyCACHETRN = 1
                        oD.isDirtyMasterTRN = 1



                        Dim CTRNRow As DataRow = CACHE_TRN.NewRow
                        CTRNRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                        CTRNRow("HIPAA_GS_GUID") = oD.GS_GUID
                        CTRNRow("HIPAA_ST_GUID") = oD.ST_GUID
                        CTRNRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                        CTRNRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                        CTRNRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                        CTRNRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                        CTRNRow("HIPAA_EB_GUID") = oD.EB_GUID
                        CTRNRow("P_GUID") = oD.P_GUID
                        CTRNRow("HL_PARENT") = oD.CurrentHLGroup


                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then CTRNRow("TRN01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else CTRNRow("TRN01") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then CTRNRow("TRN02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else CTRNRow("TRN02") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then CTRNRow("TRN03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else CTRNRow("TRN03") = DBNull.Value
                        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then CTRNRow("TRN04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else CTRNRow("TRN04") = DBNull.Value

                        'CTRNRow("TRN01") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 2)
                        'CTRNRow("TRN02") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 3)
                        'CTRNRow("TRN03") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 4)
                        'CTRNRow("TRN04") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 5) ' fix the last ^ thinmg
                        CTRNRow("BATCH_ID") = oD._BatchID
                        CTRNRow("TIME_STAMP") = oD.TimeStamp
                        CACHE_TRN.Rows.Add(CTRNRow)

                        oD.RowProcessedFlag = 1

                    End If


                End If
                'END TRN





                '******************************************************************************************************************************
                '   begin EE
                '******************************************************************************************************************************

                If oD.ROW_RECORD_TYPE = "EE" Then


                    ''********************************************************************************************************'  
                    ''EE Start -- Two senreios the @EBCarrotFlag = 0 and there is nothing to do are we have ^ and bugs bunny goes to work'  
                    ''********************************************************************************************************'  
                    '



                    If oD.EBCarrotFlag = 0 Then

                        '
                        '
                        '

                        ''********'  
                        ''IF @EBCarrotFlag  0 nothing to do'  
                        ''*********'  
                        ''  
                        '




                    End If



                    If oD.EBCarrotFlag = 1 Then


                        'BEGIN()
                        'IF@debug=2
                        'BEGIN()

                        'Print()'********'
                        'Print()'IF@EBCarrotFlag1soletsseethemoneyasManojputit'
                        'Print()'Setalltheflagsandcounterstodefaults'
                        'PRINT@EBLASTRow
                        'Print()'*********'
                        'Print()''
                        'End


                        oD.EBi = 1
                        oD.EBLastCarrot = ""
                        oD.EEEB03Data = ""
                        'objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 2)
                        'oD.EEEB03Data =  ((dbo.[udf_ParseDemlimtedString](@EBLASTRow,'*',3)))

                        oD.EEEB03Data = objss.ParseDemlimtedStringEDI(oD.EBLASTRow, oD.DataElementSeparator, 4)


                        'IF@debug=2
                        'BEGIN()

                        'Print()'********'
                        'Print()'@EEEB03Datacoldata'

                        'PRINT@EEEB03Data
                        'Print()'*********'
                        'Print()''
                        'End


                        'IF@debug=2
                        'BEGIN()

                        'Print()'********'
                        'Print()'Sodumptheebandcachetablestothemaintables'
                        'Print()'*********'
                        'Print()''
                        'End
                        oD.EBCarrotCount = Regex.Matches(oD.EEEB03Data, Regex.Escape(oD.CarrotDataDelimiter)).Count 'InStr(nt(edi,'~'))   


                        For EBi = 1 To oD.EBCarrotCount + 1


                            'WHILE(@EBLastCarrot)isnotnull--'**EOL**'
                            'BEGIN()


                            oD.EBLastCarrot = objss.ParseDemlimtedStringEDI(oD.EEEB03Data, oD.CarrotDataDelimiter, EBi)
                            ' oD.EBLastCarrot=((dbo.[udf_ParseDemlimtedString](@EEEB03Data,@EBCarrotCHAR,@EBi)))
                            '--od.EBLASTRow=oD.CurrentRowData


                            '******************************************************************************************************************************
                            '    need to fix this
                            '******************************************************************************************************************************

                            If oD.EBLastCarrot = "" Then
                                'BEGIN()
                                'IF@debug=2
                                'BEGIN()
                                'Print()'@EBLastCarrotisnull'
                                'End
                                'End
                            Else

                                'BEGIN()

                                'IF@debug=2
                                'BEGIN()

                                oD.EBcount = oD.EBcount + 1
                                'PRINTCONVERT(varchar(5),@EBcount)+','+@EBLastCarrot--((dbo.[udf_ParseDemlimtedString](@EBLastCarrot,'*',3)))

                                'End


                                oD.isDirtyMasterEB = 1
                                oD.EB_GUID = Guid.NewGuid
                                oD.P_GUID = oD.EB_GUID



                                Dim EBRow As DataRow = EB.NewRow
                                EBRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                                EBRow("HIPAA_GS_GUID") = oD.GS_GUID
                                EBRow("HIPAA_ST_GUID") = oD.ST_GUID
                                EBRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                                EBRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                                EBRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                                EBRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                                EBRow("HIPAA_EB_GUID") = oD.EB_GUID
                                EBRow("P_GUID") = oD.P_GUID
                                EBRow("HL_PARENT") = oD.CurrentHLGroup


                                If (objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 2) <> "") Then EBRow("EB01") = objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 2) Else EBRow("EB01") = DBNull.Value
                                If (objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 3) <> "") Then EBRow("EB02") = objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 3) Else EBRow("EB02") = DBNull.Value
                                If (oD.EBLastCarrot <> "") Then EBRow("EB03") = oD.EBLastCarrot Else EBRow("EB03") = DBNull.Value
                                If (objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 5) <> "") Then EBRow("EB04") = objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 5) Else EBRow("EB04") = DBNull.Value
                                If (objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 6) <> "") Then EBRow("EB05") = objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 6) Else EBRow("EB05") = DBNull.Value
                                If (objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 7) <> "") Then EBRow("EB06") = objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 7) Else EBRow("EB06") = DBNull.Value
                                If (objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 8) <> "") Then EBRow("EB07") = objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 8) Else EBRow("EB07") = DBNull.Value
                                If (objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 9) <> "") Then EBRow("EB08") = objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 9) Else EBRow("EB08") = DBNull.Value
                                If (objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 10) <> "") Then EBRow("EB09") = objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 10) Else EBRow("EB09") = DBNull.Value
                                If (objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 11) <> "") Then EBRow("EB10") = objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 11) Else EBRow("EB10") = DBNull.Value
                                If (objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 12) <> "") Then EBRow("EB11") = objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 12) Else EBRow("EB11") = DBNull.Value
                                If (objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 13) <> "") Then EBRow("EB12") = objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 13) Else EBRow("EB12") = DBNull.Value
                                If (objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 14) <> "") Then EBRow("EB13") = objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 14) Else EBRow("EB13") = DBNull.Value
                                If (objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 15) <> "") Then EBRow("EB13_1") = objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 15) Else EBRow("EB13_1") = DBNull.Value
                                If (objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 16) <> "") Then EBRow("EB13_2") = objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 16) Else EBRow("EB13_2") = DBNull.Value
                                If (objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 17) <> "") Then EBRow("EB13_3") = objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 17) Else EBRow("EB13_3") = DBNull.Value
                                If (objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 18) <> "") Then EBRow("EB13_4") = objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 18) Else EBRow("EB13_4") = DBNull.Value
                                If (objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 19) <> "") Then EBRow("EB13_5") = objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 19) Else EBRow("EB13_5") = DBNull.Value
                                If (objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 20) <> "") Then EBRow("EB13_6") = objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 20) Else EBRow("EB13_6") = DBNull.Value
                                If (objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 21) <> "") Then EBRow("EB13_7") = objss.ParseDemlimtedString(oD.EBLASTRow, oD.DataElementSeparator, 21) Else EBRow("EB13_7") = DBNull.Value

                                EBRow("BATCH_ID") = oD._BatchID
                                EBRow("TIME_STAMP") = oD.TimeStamp
                                EB.Rows.Add(EBRow)

                                ' do all the work here of putting the tables back together



                                '******************************************************************************************************************************
                                ' so start dumping the caace tables for the carrots
                                '******************************************************************************************************************************




                                If oD.isDirtyCACHEAAA = 1 Then
                                    Dim dvAAA As New DataView(CACHE_AAA)

                                    For Each rowView As DataRowView In dvAAA
                                        Dim rowAAA As DataRow = rowView.Row



                                        Dim AAARow As DataRow = AAA.NewRow
                                        AAARow("HIPAA_ISA_GUID") = oD.ISA_GUID
                                        AAARow("HIPAA_GS_GUID") = oD.GS_GUID
                                        AAARow("HIPAA_ST_GUID") = oD.ST_GUID
                                        AAARow("HIPAA_HL01_GUID") = oD.HL20_GUID
                                        AAARow("HIPAA_HL02_GUID") = oD.HL21_GUID
                                        AAARow("HIPAA_HL03_GUID") = oD.HL22_GUID
                                        AAARow("HIPAA_HL04_GUID") = oD.HL23_GUID
                                        AAARow("HIPAA_EB_GUID") = oD.EB_GUID
                                        AAARow("P_GUID") = oD.P_GUID
                                        AAARow("HL_PARENT") = oD.CurrentHLGroup
                                        AAARow("AAA01") = rowAAA("AAA01")
                                        AAARow("AAA02") = rowAAA("AAA02")
                                        AAARow("AAA03") = rowAAA("AAA03")
                                        AAARow("BATCH_ID") = rowAAA("BATCH_ID")
                                        AAARow("TIME_STAMP") = rowAAA("TIME_STAMP")
                                        AAA.Rows.Add(AAARow)


                                    Next
                                End If






                                If oD.isDirtyCACHEAMT = 1 Then
                                    Dim dvAMT As New DataView(CACHE_AMT)

                                    For Each rowView As DataRowView In dvAMT
                                        Dim rowAMT As DataRow = rowView.Row



                                        Dim AMTRow As DataRow = AMT.NewRow
                                        AMTRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                                        AMTRow("HIPAA_GS_GUID") = oD.GS_GUID
                                        AMTRow("HIPAA_ST_GUID") = oD.ST_GUID
                                        AMTRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                                        AMTRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                                        AMTRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                                        AMTRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                                        AMTRow("HIPAA_EB_GUID") = oD.EB_GUID
                                        AMTRow("P_GUID") = oD.P_GUID
                                        AMTRow("HL_PARENT") = oD.CurrentHLGroup
                                        AMTRow("AMT01") = rowAMT("AMT01")
                                        AMTRow("AMT02") = rowAMT("AMT02")
                                        AMTRow("AMT03") = rowAMT("AMT03")
                                        AMTRow("BATCH_ID") = rowAMT("BATCH_ID")
                                        AMTRow("TIME_STAMP") = rowAMT("TIME_STAMP")
                                        AMT.Rows.Add(AMTRow)


                                    Next
                                End If






                                If oD.isDirtyCACHEDMG = 1 Then
                                    Dim dvDMG As New DataView(CACHE_DMG)

                                    For Each rowView As DataRowView In dvDMG
                                        Dim rowDMG As DataRow = rowView.Row



                                        Dim DMGRow As DataRow = DMG.NewRow
                                        DMGRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                                        DMGRow("HIPAA_GS_GUID") = oD.GS_GUID
                                        DMGRow("HIPAA_ST_GUID") = oD.ST_GUID
                                        DMGRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                                        DMGRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                                        DMGRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                                        DMGRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                                        DMGRow("HIPAA_EB_GUID") = oD.EB_GUID
                                        DMGRow("P_GUID") = oD.P_GUID
                                        DMGRow("HL_PARENT") = oD.CurrentHLGroup
                                        DMGRow("DMG01") = rowDMG("DMG01")
                                        DMGRow("DMG02") = rowDMG("DMG02")
                                        DMGRow("DMG03") = rowDMG("DMG03")
                                        DMGRow("BATCH_ID") = rowDMG("BATCH_ID")
                                        DMGRow("TIME_STAMP") = rowDMG("TIME_STAMP")
                                        DMG.Rows.Add(DMGRow)


                                    Next
                                End If





                                If oD.isDirtyCACHEDTP = 1 Then
                                    Dim dvDTP As New DataView(CACHE_DTP)

                                    For Each rowView As DataRowView In dvDTP
                                        Dim rowDTP As DataRow = rowView.Row



                                        Dim DTPRow As DataRow = DTP.NewRow
                                        DTPRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                                        DTPRow("HIPAA_GS_GUID") = oD.GS_GUID
                                        DTPRow("HIPAA_ST_GUID") = oD.ST_GUID
                                        DTPRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                                        DTPRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                                        DTPRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                                        DTPRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                                        DTPRow("HIPAA_EB_GUID") = oD.EB_GUID
                                        DTPRow("P_GUID") = oD.P_GUID
                                        DTPRow("HL_PARENT") = oD.CurrentHLGroup
                                        DTPRow("DTP01") = rowDTP("DTP01")
                                        DTPRow("DTP02") = rowDTP("DTP02")
                                        DTPRow("DTP03") = rowDTP("DTP03")
                                        DTPRow("BATCH_ID") = rowDTP("BATCH_ID")
                                        DTPRow("TIME_STAMP") = rowDTP("TIME_STAMP")
                                        DTP.Rows.Add(DTPRow)


                                    Next
                                End If




                                If oD.isDirtyCACHEEQ = 1 Then
                                    Dim dvEQ As New DataView(CACHE_EQ)

                                    For Each rowView As DataRowView In dvEQ
                                        Dim rowEQ As DataRow = rowView.Row



                                        Dim EQRow As DataRow = EQ.NewRow
                                        EQRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                                        EQRow("HIPAA_GS_GUID") = oD.GS_GUID
                                        EQRow("HIPAA_ST_GUID") = oD.ST_GUID
                                        EQRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                                        EQRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                                        EQRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                                        EQRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                                        EQRow("HIPAA_EB_GUID") = oD.EB_GUID
                                        EQRow("P_GUID") = oD.P_GUID
                                        EQRow("HL_PARENT") = oD.CurrentHLGroup
                                        EQRow("EQ01") = rowEQ("EQ01")
                                        EQRow("EQ02") = rowEQ("EQ02")
                                        EQRow("EQ02_1") = rowEQ("EQ02_1")
                                        EQRow("EQ02_2") = rowEQ("EQ02_2")
                                        EQRow("EQ02_3") = rowEQ("EQ02_3")
                                        EQRow("EQ02_4") = rowEQ("EQ02_4")
                                        EQRow("EQ02_5") = rowEQ("EQ02_5")
                                        EQRow("EQ02_6") = rowEQ("EQ02_6")
                                        EQRow("EQ02_7") = rowEQ("EQ02_7")
                                        EQRow("EQ03") = rowEQ("EQ03")
                                        EQRow("EQ04") = rowEQ("EQ04")
                                        EQRow("BATCH_ID") = rowEQ("BATCH_ID")
                                        EQRow("TIME_STAMP") = rowEQ("TIME_STAMP")
                                        EQ.Rows.Add(EQRow)


                                    Next
                                End If





                                If oD.isDirtyCACHEHSD = 1 Then
                                    Dim dvHSD As New DataView(CACHE_HSD)

                                    For Each rowView As DataRowView In dvHSD
                                        Dim rowHSD As DataRow = rowView.Row



                                        Dim HSDRow As DataRow = HSD.NewRow
                                        HSDRow("ROW_ID") = oD.EBcount
                                        HSDRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                                        HSDRow("HIPAA_GS_GUID") = oD.GS_GUID
                                        HSDRow("HIPAA_ST_GUID") = oD.ST_GUID
                                        HSDRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                                        HSDRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                                        HSDRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                                        HSDRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                                        HSDRow("HIPAA_EB_GUID") = oD.EB_GUID
                                        HSDRow("P_GUID") = oD.P_GUID
                                        HSDRow("HL_PARENT") = oD.CurrentHLGroup
                                        HSDRow("HSD01") = rowHSD("HSD01")
                                        HSDRow("HSD02") = rowHSD("HSD02")
                                        HSDRow("HSD03") = rowHSD("HSD03")
                                        HSDRow("HSD04") = rowHSD("HSD04")
                                        HSDRow("HSD05") = rowHSD("HSD05")
                                        HSDRow("HSD06") = rowHSD("HSD06")
                                        HSDRow("HSD07") = rowHSD("HSD07")
                                        HSDRow("HSD08") = rowHSD("HSD08")
                                        HSDRow("BATCH_ID") = rowHSD("BATCH_ID")
                                        HSDRow("TIME_STAMP") = rowHSD("TIME_STAMP")
                                        HSD.Rows.Add(HSDRow)


                                    Next
                                End If





                                If oD.isDirtyCACHEIII = 1 Then
                                    Dim dvIII As New DataView(CACHE_III)

                                    For Each rowView As DataRowView In dvIII
                                        Dim rowIII As DataRow = rowView.Row



                                        Dim IIIRow As DataRow = III.NewRow
                                        IIIRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                                        IIIRow("HIPAA_GS_GUID") = oD.GS_GUID
                                        IIIRow("HIPAA_ST_GUID") = oD.ST_GUID
                                        IIIRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                                        IIIRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                                        IIIRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                                        IIIRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                                        IIIRow("HIPAA_EB_GUID") = oD.EB_GUID
                                        IIIRow("P_GUID") = oD.P_GUID
                                        IIIRow("HL_PARENT") = oD.CurrentHLGroup
                                        IIIRow("III01") = rowIII("III01")
                                        IIIRow("III02") = rowIII("III02")
                                        IIIRow("III03") = rowIII("III03")
                                        IIIRow("III04") = rowIII("III04")
                                        IIIRow("III05") = rowIII("III05")
                                        IIIRow("III06") = rowIII("III06")
                                        IIIRow("III07") = rowIII("III07")
                                        IIIRow("III08") = rowIII("III08")
                                        IIIRow("BATCH_ID") = rowIII("BATCH_ID")
                                        IIIRow("TIME_STAMP") = rowIII("TIME_STAMP")
                                        III.Rows.Add(IIIRow)


                                    Next
                                End If



                                If oD.isDirtyCACHEINS = 1 Then
                                    Dim dvINS As New DataView(CACHE_INS)

                                    For Each rowView As DataRowView In dvINS
                                        Dim rowINS As DataRow = rowView.Row



                                        Dim INSRow As DataRow = INS.NewRow
                                        INSRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                                        INSRow("HIPAA_GS_GUID") = oD.GS_GUID
                                        INSRow("HIPAA_ST_GUID") = oD.ST_GUID
                                        INSRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                                        INSRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                                        INSRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                                        INSRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                                        INSRow("HIPAA_EB_GUID") = oD.EB_GUID
                                        INSRow("P_GUID") = oD.P_GUID
                                        INSRow("HL_PARENT") = oD.CurrentHLGroup
                                        INSRow("INS01") = rowINS("INS01")
                                        INSRow("INS02") = rowINS("INS02")
                                        INSRow("INS03") = rowINS("INS03")
                                        INSRow("INS04") = rowINS("INS04")
                                        INSRow("INS05") = rowINS("INS05")
                                        INSRow("INS06") = rowINS("INS06")
                                        INSRow("INS07") = rowINS("INS07")
                                        INSRow("INS08") = rowINS("INS08")
                                        INSRow("INS09") = rowINS("INS09")
                                        INSRow("INS10") = rowINS("INS10")
                                        INSRow("INS11") = rowINS("INS11")
                                        INSRow("INS12") = rowINS("INS12")
                                        INSRow("INS13") = rowINS("INS13")
                                        INSRow("INS14") = rowINS("INS14")
                                        INSRow("INS15") = rowINS("INS15")
                                        INSRow("INS16") = rowINS("INS16")
                                        INSRow("INS17") = rowINS("INS17")
                                        INSRow("BATCH_ID") = rowINS("BATCH_ID")
                                        INSRow("TIME_STAMP") = rowINS("TIME_STAMP")
                                        INS.Rows.Add(INSRow)


                                    Next
                                End If




                                If oD.isDirtyCACHEMSG = 1 Then
                                    Dim dvMSG As New DataView(CACHE_MSG)

                                    For Each rowView As DataRowView In dvMSG
                                        Dim rowMSG As DataRow = rowView.Row



                                        Dim MSGRow As DataRow = MSG.NewRow
                                        MSGRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                                        MSGRow("HIPAA_GS_GUID") = oD.GS_GUID
                                        MSGRow("HIPAA_ST_GUID") = oD.ST_GUID
                                        MSGRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                                        MSGRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                                        MSGRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                                        MSGRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                                        MSGRow("HIPAA_EB_GUID") = oD.EB_GUID
                                        MSGRow("P_GUID") = oD.P_GUID
                                        MSGRow("HL_PARENT") = oD.CurrentHLGroup
                                        MSGRow("MSG01") = rowMSG("MSG01")
                                        MSGRow("MSG02") = rowMSG("MSG02")
                                        MSGRow("MSG03") = rowMSG("MSG03")
                                        MSGRow("MSG04") = rowMSG("MSG04")
                                        MSGRow("MSG05") = rowMSG("MSG05")
                                        MSGRow("MSG06") = rowMSG("MSG06")
                                        MSGRow("MSG07") = rowMSG("MSG07")
                                        MSGRow("BATCH_ID") = rowMSG("BATCH_ID")
                                        MSGRow("TIME_STAMP") = rowMSG("TIME_STAMP")
                                        MSG.Rows.Add(MSGRow)


                                    Next
                                End If





                                If oD.isDirtyCACHEN3 = 1 Then
                                    Dim dvN3 As New DataView(CACHE_N3)

                                    For Each rowView As DataRowView In dvN3
                                        Dim rowN3 As DataRow = rowView.Row



                                        Dim N3Row As DataRow = N3.NewRow
                                        N3Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                                        N3Row("HIPAA_GS_GUID") = oD.GS_GUID
                                        N3Row("HIPAA_ST_GUID") = oD.ST_GUID
                                        N3Row("HIPAA_HL01_GUID") = oD.HL20_GUID
                                        N3Row("HIPAA_HL02_GUID") = oD.HL21_GUID
                                        N3Row("HIPAA_HL03_GUID") = oD.HL22_GUID
                                        N3Row("HIPAA_HL04_GUID") = oD.HL23_GUID
                                        N3Row("HIPAA_EB_GUID") = oD.EB_GUID
                                        N3Row("P_GUID") = oD.P_GUID
                                        N3Row("HL_PARENT") = oD.CurrentHLGroup
                                        N3Row("N301") = rowN3("N301")
                                        N3Row("N302") = rowN3("N302")
                                        N3Row("BATCH_ID") = rowN3("BATCH_ID")
                                        N3Row("TIME_STAMP") = rowN3("TIME_STAMP")
                                        N3.Rows.Add(N3Row)


                                    Next
                                End If





                                If oD.isDirtyCACHEN4 = 1 Then
                                    Dim dvN4 As New DataView(CACHE_N4)

                                    For Each rowView As DataRowView In dvN4
                                        Dim rowN4 As DataRow = rowView.Row



                                        Dim N4Row As DataRow = N4.NewRow
                                        N4Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                                        N4Row("HIPAA_GS_GUID") = oD.GS_GUID
                                        N4Row("HIPAA_ST_GUID") = oD.ST_GUID
                                        N4Row("HIPAA_HL01_GUID") = oD.HL20_GUID
                                        N4Row("HIPAA_HL02_GUID") = oD.HL21_GUID
                                        N4Row("HIPAA_HL03_GUID") = oD.HL22_GUID
                                        N4Row("HIPAA_HL04_GUID") = oD.HL23_GUID
                                        N4Row("HIPAA_EB_GUID") = oD.EB_GUID
                                        N4Row("P_GUID") = oD.P_GUID
                                        N4Row("HL_PARENT") = oD.CurrentHLGroup
                                        N4Row("N401") = rowN4("N401")
                                        N4Row("N402") = rowN4("N402")
                                        N4Row("N403") = rowN4("N403")
                                        N4Row("N404") = rowN4("N404")
                                        N4Row("N405") = rowN4("N405")
                                        N4Row("N406") = rowN4("N406")
                                        N4Row("BATCH_ID") = rowN4("BATCH_ID")
                                        N4Row("TIME_STAMP") = rowN4("TIME_STAMP")
                                        N4.Rows.Add(N4Row)


                                    Next
                                End If





                                If oD.isDirtyCACHENM1 = 1 Then
                                    Dim dvNM1 As New DataView(CACHE_NM1)

                                    For Each rowView As DataRowView In dvNM1
                                        Dim rowNM1 As DataRow = rowView.Row



                                        Dim NM1Row As DataRow = NM1.NewRow
                                        NM1Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                                        NM1Row("HIPAA_GS_GUID") = oD.GS_GUID
                                        NM1Row("HIPAA_ST_GUID") = oD.ST_GUID
                                        NM1Row("HIPAA_HL01_GUID") = oD.HL20_GUID
                                        NM1Row("HIPAA_HL02_GUID") = oD.HL21_GUID
                                        NM1Row("HIPAA_HL03_GUID") = oD.HL22_GUID
                                        NM1Row("HIPAA_HL04_GUID") = oD.HL23_GUID
                                        NM1Row("HIPAA_EB_GUID") = oD.EB_GUID
                                        NM1Row("P_GUID") = oD.P_GUID
                                        NM1Row("HL_PARENT") = oD.CurrentHLGroup
                                        NM1Row("NM101") = rowNM1("NM101")
                                        NM1Row("NM102") = rowNM1("NM102")
                                        NM1Row("NM103") = rowNM1("NM103")
                                        NM1Row("NM104") = rowNM1("NM104")
                                        NM1Row("NM105") = rowNM1("NM105")
                                        NM1Row("NM106") = rowNM1("NM106")
                                        NM1Row("NM107") = rowNM1("NM107")
                                        NM1Row("NM108") = rowNM1("NM108")
                                        NM1Row("NM109") = rowNM1("NM109")
                                        NM1Row("NM110") = rowNM1("NM110")
                                        NM1Row("NM111") = rowNM1("NM111")
                                        NM1Row("BATCH_ID") = rowNM1("BATCH_ID")
                                        NM1Row("TIME_STAMP") = rowNM1("TIME_STAMP")
                                        NM1.Rows.Add(NM1Row)


                                    Next
                                End If



                                If oD.isDirtyCACHEPER = 1 Then
                                    Dim dvPER As New DataView(CACHE_PER)

                                    For Each rowView As DataRowView In dvPER
                                        Dim rowPER As DataRow = rowView.Row



                                        Dim PERRow As DataRow = PER.NewRow
                                        PERRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                                        PERRow("HIPAA_GS_GUID") = oD.GS_GUID
                                        PERRow("HIPAA_ST_GUID") = oD.ST_GUID
                                        PERRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                                        PERRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                                        PERRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                                        PERRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                                        PERRow("HIPAA_EB_GUID") = oD.EB_GUID
                                        PERRow("P_GUID") = oD.P_GUID
                                        PERRow("HL_PARENT") = oD.CurrentHLGroup
                                        PERRow("PER01") = rowPER("PER01")
                                        PERRow("PER02") = rowPER("PER02")
                                        PERRow("PER03") = rowPER("PER03")
                                        PERRow("PER04") = rowPER("PER04")
                                        PERRow("PER05") = rowPER("PER05")
                                        PERRow("PER06") = rowPER("PER06")
                                        PERRow("PER07") = rowPER("PER07")
                                        PERRow("PER08") = rowPER("PER08")
                                        PERRow("PER09") = rowPER("PER09")
                                        PERRow("BATCH_ID") = rowPER("BATCH_ID")
                                        PERRow("TIME_STAMP") = rowPER("TIME_STAMP")
                                        PER.Rows.Add(PERRow)


                                    Next
                                End If






                                If oD.isDirtyCACHEPRV = 1 Then
                                    Dim dvPRV As New DataView(CACHE_PRV)

                                    For Each rowView As DataRowView In dvPRV
                                        Dim rowPRV As DataRow = rowView.Row



                                        Dim PRVRow As DataRow = PRV.NewRow
                                        PRVRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                                        PRVRow("HIPAA_GS_GUID") = oD.GS_GUID
                                        PRVRow("HIPAA_ST_GUID") = oD.ST_GUID
                                        PRVRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                                        PRVRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                                        PRVRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                                        PRVRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                                        PRVRow("HIPAA_EB_GUID") = oD.EB_GUID
                                        PRVRow("P_GUID") = oD.P_GUID
                                        PRVRow("HL_PARENT") = oD.CurrentHLGroup
                                        PRVRow("PRV01") = rowPRV("PRV01")
                                        PRVRow("PRV02") = rowPRV("PRV02")
                                        PRVRow("PRV03") = rowPRV("PRV03")
                                        PRVRow("PRV04") = rowPRV("PRV04")
                                        PRVRow("PRV05") = rowPRV("PRV05")
                                        PRVRow("PRV06") = rowPRV("PRV06")
                                        PRVRow("BATCH_ID") = rowPRV("BATCH_ID")
                                        PRVRow("TIME_STAMP") = rowPRV("TIME_STAMP")
                                        PRV.Rows.Add(PRVRow)


                                    Next
                                End If






                                If oD.isDirtyCACHEREF = 1 Then
                                    Dim dvREF As New DataView(CACHE_REF)
                                    For Each rowView As DataRowView In dvREF
                                        Dim rowREF As DataRow = rowView.Row



                                        Dim REFRow As DataRow = REF.NewRow
                                        REFRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                                        REFRow("HIPAA_GS_GUID") = oD.GS_GUID
                                        REFRow("HIPAA_ST_GUID") = oD.ST_GUID
                                        REFRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                                        REFRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                                        REFRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                                        REFRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                                        REFRow("HIPAA_EB_GUID") = oD.EB_GUID
                                        REFRow("P_GUID") = oD.P_GUID
                                        REFRow("HL_PARENT") = oD.CurrentHLGroup
                                        REFRow("REF01") = rowREF("REF01")
                                        REFRow("REF02") = rowREF("REF02")
                                        REFRow("REF03") = rowREF("REF03")
                                        REFRow("BATCH_ID") = rowREF("BATCH_ID")
                                        REFRow("TIME_STAMP") = rowREF("TIME_STAMP")
                                        REF.Rows.Add(REFRow)


                                    Next
                                End If




                                If oD.isDirtyCACHETRN = 1 Then
                                    Dim dvTRN As New DataView(CACHE_TRN)
                                    For Each rowView As DataRowView In dvTRN
                                        Dim rowTRN As DataRow = rowView.Row



                                        Dim TRNRow As DataRow = TRN.NewRow
                                        TRNRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                                        TRNRow("HIPAA_GS_GUID") = oD.GS_GUID
                                        TRNRow("HIPAA_ST_GUID") = oD.ST_GUID
                                        TRNRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                                        TRNRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                                        TRNRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                                        TRNRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                                        TRNRow("HIPAA_EB_GUID") = oD.EB_GUID
                                        TRNRow("P_GUID") = oD.P_GUID
                                        TRNRow("HL_PARENT") = oD.CurrentHLGroup
                                        TRNRow("TRN01") = rowTRN("TRN01")
                                        TRNRow("TRN02") = rowTRN("TRN02")
                                        TRNRow("TRN03") = rowTRN("TRN03")
                                        TRNRow("TRN04") = rowTRN("TRN04")
                                        TRNRow("TIME_STAMP") = rowTRN("TIME_STAMP")
                                        TRNRow("BATCH_ID") = rowTRN("BATCH_ID")
                                        TRN.Rows.Add(TRNRow)

                                    Next

                                End If





                                '******************************************************************************************************************************
                                'ok this is the end of the cache tables
                                '******************************************************************************************************************************










                                ' end the work of putting all back to gther
                                '--incrementtheindex

                            End If

                            mmm = mmm + 1
                            'End

                        Next 'End

                    End If
                    '******************************************************************************************************************************
                    '  this is the end of EB  looping for ^
                    '******************************************************************************************************************************


                    '******************************************************************************************************************************
                    ' clear all the cahce tables
                    '******************************************************************************************************************************

                    If oD.isDirtyCACHEAAA = 1 Then
                        CACHE_AAA.Clear()
                    End If

                    If oD.isDirtyCACHEAMT = 1 Then
                        'BEGIN.clear
                        CACHE_AMT.Clear()
                    End If

                    If oD.isDirtyCACHEDMG = 1 Then
                        'BEGIN.clear
                        CACHE_DMG.Clear()
                    End If

                    If oD.isDirtyCACHEDTP = 1 Then
                        'BEGIN.clear
                        CACHE_DTP.Clear()
                    End If

                    If oD.isDirtyCACHEEQ = 1 Then
                        'BEGIN.clear
                        CACHE_EQ.Clear()
                    End If

                    If oD.isDirtyCACHEHSD = 1 Then
                        'BEGIN.clear
                        CACHE_HSD.Clear()
                    End If

                    If oD.isDirtyCACHEIII = 1 Then
                        'BEGIN.clear
                        CACHE_III.Clear()
                    End If

                    If oD.isDirtyCACHEMSG = 1 Then
                        'BEGIN.clear
                        CACHE_MSG.Clear()
                    End If

                    If oD.isDirtyCACHEN3 = 1 Then
                        'BEGIN.clear
                        CACHE_N3.Clear()
                    End If

                    If oD.isDirtyCACHEN4 = 1 Then
                        'BEGIN.clear
                        CACHE_N4.Clear()
                    End If

                    If oD.isDirtyCACHENM1 = 1 Then
                        'BEGIN.clear
                        CACHE_NM1.Clear()
                    End If

                    If oD.isDirtyCACHEPER = 1 Then
                        'BEGIN.clear
                        CACHE_PER.Clear()
                    End If

                    If oD.isDirtyCACHEPRV = 1 Then
                        'BEGIN.clear
                        CACHE_PRV.Clear()
                    End If

                    If oD.isDirtyCACHEREF = 1 Then
                        'BEGIN.clear
                        CACHE_REF.Clear()
                    End If


                    If oD.isDirtyCACHETRN = 1 Then
                        'BEGIN.clear
                        CACHE_TRN.Clear()
                    End If


                    '******************************************************************************************************************************
                    ' end clear tables
                    '******************************************************************************************************************************








                    '******************************************************************************************************************************
                    '  reset all the flags
                    '******************************************************************************************************************************'

                    oD.isDirtyCACHEAAA = 0
                    oD.isDirtyCACHEAMT = 0
                    oD.isDirtyCACHEDMG = 0
                    oD.isDirtyCACHEDTP = 0
                    oD.isDirtyCACHEEQ = 0
                    oD.isDirtyCACHEHSD = 0
                    oD.isDirtyCACHEIII = 0
                    oD.isDirtyCACHEINS = 0
                    oD.isDirtyCACHEMSG = 0
                    oD.isDirtyCACHEN3 = 0
                    oD.isDirtyCACHEN4 = 0
                    oD.isDirtyCACHENM1 = 0
                    oD.isDirtyCACHEPER = 0
                    oD.isDirtyCACHEPRV = 0
                    oD.isDirtyCACHEREF = 0
                    oD.isDirtyCACHETRN = 0

                    '******************************************************************************************************************************
                    '   thats done
                    '  resert the ^ flags so we can go again
                    '******************************************************************************************************************************

                    oD.EBi = 0
                    oD.EBLastCarrot = ""
                    oD.EEEB03Data = ""
                    oD.EBLASTRow = ""

                    'If String.IsNullOrEmpty(pd_first_name.Text.ToString().Trim) = True Then
                    '    frmFirstName = DBNull.Value
                    'Else
                    '    frmFirstName = pd_first_name.Text
                    'End If



                    oD.RowProcessedFlag = 1
                End If

                '******************************************************************************************************************************
                '     End EE
                '******************************************************************************************************************************


                ' thats all i know about so log it tounknow an move on




                If oD.ROW_RECORD_TYPE = "SE" Then

                    oD.EBFlag = 0
                    oD.RowProcessedFlag = 1
                End If


                If oD.ROW_RECORD_TYPE = "GE" Then
                    '           BEGIN()
                    '-- SET THE GS FLAG BACK TO 0 SO WE CAN CHECK FOR ANOTHER GS RECORD  


                    ' -- UPDATE  od.GS SET   
                    ' --   GE01 = ((dbo.[udf_ParseDemlimtedString]( od.CurrentRowData,'*', 1))) ,   
                    ' --   GE02 = ((dbo.[udf_ParseDemlimtedString]( od.CurrentRowData,'*', 2)))  
                    ' -- WHERE HIPAA_GS_UID =   od.GSUID   
                    oD.HL23UID = Guid.Empty
                    oD.HL22UID = Guid.Empty
                    oD.HL21UID = Guid.Empty
                    oD.HL20UID = Guid.Empty
                    oD.HL23Flag = 0
                    oD.HL22Flag = 0
                    oD.HL21Flag = 0
                    oD.HL20Flag = 0
                    oD.GSFlag = 0


                    oD.GS_GUID = Guid.Empty
                    oD.RowProcessedFlag = 1



                End If



                If oD.ROW_RECORD_TYPE = "IEA" Then
                    '           BEGIN()
                    '-- SET THE hlflag BACK TO ZERO THERE SHOULD NOT BE AND HL1 TAG TILL THE END  


                    ' -- UPDATE  od.GS SET   
                    ' --   GE01 = ((dbo.[udf_ParseDemlimtedString]( od.CurrentRowData,'*', 1))) ,   
                    ' --   GE02 = ((dbo.[udf_ParseDemlimtedString]( od.CurrentRowData,'*', 2)))  
                    ' -- WHERE HIPAA_GS_UID =   od.GSUID   

                    oD.ISA_GUID = Guid.Empty
                    oD.ISAFlag = 0


                    oD.RowProcessedFlag = 1

                End If






                If oD.RowProcessedFlag = 0 Then
                    Dim UNKRow As DataRow = UNK.NewRow
                    UNKRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    UNKRow("HIPAA_GS_GUID") = oD.GS_GUID
                    UNKRow("HIPAA_ST_GUID") = oD.ST_GUID
                    UNKRow("HIPAA_HL01_GUID") = oD.HL20_GUID
                    UNKRow("HIPAA_HL02_GUID") = oD.HL21_GUID
                    UNKRow("HIPAA_HL03_GUID") = oD.HL22_GUID
                    UNKRow("HIPAA_HL04_GUID") = oD.HL23_GUID
                    UNKRow("HIPAA_EB_GUID") = oD.EB_GUID
                    UNKRow("P_GUID") = oD.P_GUID
                    UNKRow("HL_PARENT") = oD.CurrentHLGroup
                    UNKRow("ROW_RECORD_TYPE") = oD.ROW_RECORD_TYPE
                    UNKRow("ROW_DATA") = oD.CurrentRowData
                    UNKRow("BATCH_ID") = oD._BatchID
                    UNK.Rows.Add(UNKRow)


                    oD.RowProcessedFlag = 1
                    oD.ISAFlag = 1
                End If



                ' so update the EDI271  table rinse and repeat


                'begin


                row.Item("HIPAA_ISA_GUID") = oD.ISA_GUID
                row.Item("HIPAA_GS_GUID") = oD.GS_GUID
                row.Item("HIPAA_ST_GUID") = oD.ST_GUID
                row.Item("HIPAA_HL01_GUID") = oD.HL20_GUID
                row.Item("HIPAA_HL02_GUID") = oD.HL21_GUID
                row.Item("HIPAA_HL03_GUID") = oD.HL22_GUID
                row.Item("HIPAA_HL04_GUID") = oD.HL23_GUID
                row.Item("HIPAA_EB_GUID") = oD.EB_GUID
                row.Item("P_GUID") = oD.P_GUID
                row.Item("HL_PARENT") = oD.CurrentHLGroup
                row.Item("RowDataParsed") = 1
                oD.RowProcessedFlag = 0

                'end




            Next row

            '******************************************************************************************************************************
            '    begin msg smashy thingy
            '******************************************************************************************************************************
            If oD.MSGEXFlag = 10000000 Then
                '         





                '--move the all the msg to the cahce table and dump the hipaa table

                CACHE_MSG.Clear()



                Dim dvMSG As New DataView(MSG)
                For Each rowView As DataRowView In dvMSG
                    Dim rowMSG As DataRow = rowView.Row

                    'sP_GUID

                    Dim CMSGRow As DataRow = CACHE_MSG.NewRow
                    CMSGRow("HIPAA_ISA_GUID") = rowMSG("HIPAA_ISA_GUID")
                    CMSGRow("HIPAA_GS_GUID") = rowMSG("HIPAA_GS_GUID")
                    CMSGRow("HIPAA_ST_GUID") = rowMSG("HIPAA_ST_GUID")
                    CMSGRow("HIPAA_HL01_GUID") = rowMSG("HIPAA_HL01_GUID")
                    CMSGRow("HIPAA_HL02_GUID") = rowMSG("HIPAA_HL02_GUID")
                    CMSGRow("HIPAA_HL03_GUID") = rowMSG("HIPAA_HL03_GUID")
                    CMSGRow("HIPAA_HL04_GUID") = rowMSG("HIPAA_HL04_GUID")
                    CMSGRow("HIPAA_EB_GUID") = rowMSG("HIPAA_EB_GUID")
                    CMSGRow("P_GUID") = rowMSG("P_GUID")
                    CMSGRow("sP_GUID") = Convert.ToString(rowMSG("P_GUID"))
                    CMSGRow("HL_PARENT") = rowMSG("HL_PARENT")
                    CMSGRow("MSG01") = rowMSG("MSG01")
                    CMSGRow("MSG02") = rowMSG("MSG02")
                    CMSGRow("MSG03") = rowMSG("MSG03")
                    CMSGRow("MSG04") = rowMSG("MSG04")
                    CMSGRow("MSG05") = rowMSG("MSG05")
                    CMSGRow("MSG06") = rowMSG("MSG06")
                    CMSGRow("MSG07") = rowMSG("MSG07")
                    CMSGRow("BATCH_ID") = rowMSG("BATCH_ID")
                    CACHE_MSG.Rows.Add(CMSGRow)

                Next

                MSG.Clear()

                '***************************************************************************************************************************
                ' If the message HIPAA_EB_GUID is 0000 or null then it does not belong to and ED so dump it the cleared MSG table From Cache
                '  begin
                '****************************************************************************************************************************


                Dim CdvMSG As New DataView(CACHE_MSG)
                For Each CrowView As DataRowView In CdvMSG
                    Dim crowMSG As DataRow = CrowView.Row

                    'sP_GUID



                    If (crowMSG("HIPAA_EB_GUID") = Guid.Empty) Then

                        Dim CCMSGRow As DataRow = MSG.NewRow
                        CCMSGRow("HIPAA_ISA_GUID") = crowMSG("HIPAA_ISA_GUID")
                        CCMSGRow("HIPAA_GS_GUID") = crowMSG("HIPAA_GS_GUID")
                        CCMSGRow("HIPAA_ST_GUID") = crowMSG("HIPAA_ST_GUID")
                        CCMSGRow("HIPAA_HL01_GUID") = crowMSG("HIPAA_HL01_GUID")
                        CCMSGRow("HIPAA_HL02_GUID") = crowMSG("HIPAA_HL02_GUID")
                        CCMSGRow("HIPAA_HL03_GUID") = crowMSG("HIPAA_HL03_GUID")
                        CCMSGRow("HIPAA_HL04_GUID") = crowMSG("HIPAA_HL04_GUID")
                        CCMSGRow("HIPAA_EB_GUID") = crowMSG("HIPAA_EB_GUID")
                        CCMSGRow("P_GUID") = crowMSG("P_GUID")
                        ' CCMSGRow("sP_GUID") = Convert.ToString(crowMSG("P_GUID"))
                        CCMSGRow("HL_PARENT") = crowMSG("HL_PARENT")
                        CCMSGRow("MSG01") = crowMSG("MSG01") + " " + crowMSG("MSG02") + " " + crowMSG("MSG03") + " " + crowMSG("MSG04") + " " + crowMSG("MSG05") + " " + crowMSG("MSG06") + " " + crowMSG("MSG07")
                        CCMSGRow("MSG02") = String.Empty
                        CCMSGRow("MSG03") = String.Empty
                        CCMSGRow("MSG04") = String.Empty
                        CCMSGRow("MSG05") = String.Empty
                        CCMSGRow("MSG06") = String.Empty
                        CCMSGRow("MSG07") = "AAA"
                        CCMSGRow("BATCH_ID") = crowMSG("BATCH_ID")
                        MSG.Rows.Add(CCMSGRow)
                    End If
                Next




                '***************************************************************************************************************************
                '   end
                'If the message HIPAA_EB_GUID is 0000 or null then it does not belong to and ED so dump it the cleared MSG table From Cache
                '
                '****************************************************************************************************************************




                distinctDT = EB.DefaultView.ToTable(True, "P_GUID")


                For Each row As DataRow In distinctDT.Rows


                    Dim sGUID As Guid = row("P_GUID")

                    Dim rf As String = "sP_GUID = '" + Convert.ToString(sGUID) + "'"


                    ''Change  thos to a like

                    ' Dim tdistinctMSG As DataTable = CACHE_MSG.DefaultView.ToTable
                    Dim tdistinctMSG As New DataView(CACHE_MSG)

                    tdistinctMSG.RowFilter = rf

                    Dim dvDsitinctMSg As DataView = tdistinctMSG

                    Dim ta As DataTable

                    ta = dvDsitinctMSg.ToTable

                    If ta.Rows.Count > 0 Then
                        Dim BigMsg As String = ""
                        Dim newrowFlag = 0

                        Dim tMSG As New DataView(ta)
                        For Each rowView As DataRowView In tMSG


                            If newrowFlag = 0 Then

                                BigMsg = BigMsg + rowView("MSG01") + " " + rowView("MSG02") + " " + rowView("MSG03") + " " + rowView("MSG04") + " " + rowView("MSG05") + " " + rowView("MSG06") + " " + rowView("MSG07")

                            Else


                                BigMsg = BigMsg + Chr(11) + Chr(13) + Chr(11) + Chr(13) + rowView("MSG01") + " " + rowView("MSG02") + " " + rowView("MSG03") + " " + rowView("MSG04") + " " + rowView("MSG05") + " " + rowView("MSG06") + " " + rowView("MSG07")

                            End If




                            newrowFlag = 1

                        Next


                        Dim rowMSG As DataRow = ta.Rows(0)


                        Dim MSGRow As DataRow = MSG.NewRow
                        MSGRow("HIPAA_ISA_GUID") = rowMSG("HIPAA_ISA_GUID")
                        MSGRow("HIPAA_GS_GUID") = rowMSG("HIPAA_GS_GUID")
                        MSGRow("HIPAA_ST_GUID") = rowMSG("HIPAA_ST_GUID")
                        MSGRow("HIPAA_HL01_GUID") = rowMSG("HIPAA_HL01_GUID")
                        MSGRow("HIPAA_HL02_GUID") = rowMSG("HIPAA_HL02_GUID")
                        MSGRow("HIPAA_HL03_GUID") = rowMSG("HIPAA_HL03_GUID")
                        MSGRow("HIPAA_HL04_GUID") = rowMSG("HIPAA_HL04_GUID")
                        MSGRow("HIPAA_EB_GUID") = rowMSG("HIPAA_EB_GUID")
                        MSGRow("P_GUID") = rowMSG("P_GUID")
                        MSGRow("MSG01") = BigMsg  'rowMSG("MSG01") + " " + rowMSG("MSG02") + " " + rowMSG("MSG03") + " " + rowMSG("MSG04") + " " + rowMSG("MSG05") + " " + rowMSG("MSG06") + " " + rowMSG("MSG07")
                        MSGRow("BATCH_ID") = rowMSG("BATCH_ID")
                        MSG.Rows.Add(MSGRow)

                    End If




                Next

            End If

            '******************************************************************************************************************************
            '    end msg smashy
            '******************************************************************************************************************************








            stop_time = Now
            elapsed_time = stop_time.Subtract(start_time)
            elapsed_time.TotalSeconds.ToString("0.000000")




            Return 0

        End Function

        Public Function Comit() As Integer

            Dim i As Integer = -1

            Dim LoopCount As Integer = 0







            If _DeadLockCount < oD._DeadLockRetrys Then
                LoopCount = LoopCount + 1
                If oD.Verbose = 1 Then
                    log.ExceptionDetails("49-271 loop count ", "Batch ID  " + Convert.ToString(oD._BatchID) + " loop count   " + Convert.ToString(LoopCount))
                End If


                Dim param As New SqlParameter()
                Dim sqlConn As SqlConnection = New SqlConnection
                Dim cmd As SqlCommand
                Dim sqlString As String = String.Empty

                sqlConn.ConnectionString = oD._ConnectionString
                sqlConn.Open()

                '

                Try

                    sqlString = "usp_eligibility_response_dump"





                    cmd = New SqlCommand(sqlString, sqlConn)
                    cmd.CommandTimeout = 90
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@HIPPA_ENVELOP", ENVELOP)
                    cmd.Parameters.AddWithValue("@HIPAA_EB", EB)
                    cmd.Parameters.AddWithValue("@HIPAA_AAA", AAA)
                    cmd.Parameters.AddWithValue("@HIPAA_AMT", AMT)
                    cmd.Parameters.AddWithValue("@HIPAA_DMG", DMG)
                    cmd.Parameters.AddWithValue("@HIPAA_BHT", BHT)
                    cmd.Parameters.AddWithValue("@HIPAA_DTP", DTP)
                    cmd.Parameters.AddWithValue("@HIPAA_EQ", EQ)
                    cmd.Parameters.AddWithValue("@HIPAA_HSD", HSD)
                    cmd.Parameters.AddWithValue("@HIPAA_III", III)
                    cmd.Parameters.AddWithValue("@HIPAA_INS", INS)
                    cmd.Parameters.AddWithValue("@HIPAA_N3", N3)
                    cmd.Parameters.AddWithValue("@HIPAA_N4", N4)
                    cmd.Parameters.AddWithValue("@HIPAA_MSG", MSG)
                    cmd.Parameters.AddWithValue("@HIPAA_NM1", NM1)
                    cmd.Parameters.AddWithValue("@HIPAA_PER", PER)
                    cmd.Parameters.AddWithValue("@HIPAA_PRV", PRV)
                    cmd.Parameters.AddWithValue("@HIPAA_REF", REF)
                    cmd.Parameters.AddWithValue("@HIPAA_TRN", TRN)
                    cmd.Parameters.AddWithValue("@HIPAA_UNK", UNK)
                    cmd.Parameters.AddWithValue("@ebr_id", oD.ebr_id)
                    cmd.Parameters.AddWithValue("@batch_id", oD._BatchID)
                    cmd.Parameters.AddWithValue("@user_id", oD.user_id)
                    cmd.Parameters.AddWithValue("@hosp_code", oD.hosp_code)
                    cmd.Parameters.AddWithValue("@source", oD.source)
                    cmd.Parameters.AddWithValue("@EDI", oD.edi)
                    cmd.Parameters.AddWithValue("@PAYOR_ID", oD.PAYOR_ID)
                    cmd.Parameters.AddWithValue("@Vendor_name", oD.Vendor_name)
                    cmd.Parameters.AddWithValue("@Log_EDI", oD.Log_EDI)
                    cmd.Parameters.Add("@Status", Data.SqlDbType.VarChar, 20)
                    cmd.Parameters("@Status").Direction = ParameterDirection.Output
                    cmd.Parameters.Add("@Reject_Reason_code", Data.SqlDbType.VarChar, 10)
                    cmd.Parameters("@Reject_Reason_code").Direction = ParameterDirection.Output
                    cmd.Parameters.Add("@LOOP_AGAIN", Data.SqlDbType.VarChar, 1)
                    cmd.Parameters("@LOOP_AGAIN").Direction = ParameterDirection.Output

                    If _DeadLockFlag Then
                        cmd.Parameters.AddWithValue("@DELETE_FLAG", "Y")
                    Else
                        cmd.Parameters.AddWithValue("@DELETE_FLAG", oD.DeleteFlag)
                    End If


                    'If (_isEPIC = True) Then
                    '    Then

                    '    cmd.Parameters.Add("@EPICOutEDIString", Data.SqlDbType.VarChar)
                    '    cmd.Parameters("@EPICOutEDIString").Direction = ParameterDirection.Output

                    'End If


                    ''   err = cmd.ExecuteNonQuery()



                    '  log.ExceptionDetails("64-Parse.Import271", " Begin Comit" + Convert.ToString(oD.ebr_id))
                    err = Convert.ToString(cmd.ExecuteNonQuery())
                    '  log.ExceptionDetails("65-Parse.Import271", " end Comit " + Convert.ToString(oD.ebr_id) + " " + err)


                    oD.Status = cmd.Parameters("@Status").Value.ToString()
                    oD.RejectReasonCode = cmd.Parameters("@Reject_Reason_code").Value.ToString()
                    oD.LoopAgain = cmd.Parameters("@LOOP_AGAIN").Value.ToString()

                    i = 0

                Catch sx As SqlException
                    log.ExceptionDetails("50-" + oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import271" + Me.ToString, sx)

                    log.ExceptionDetails("51-Parse.Import271", "Save to db failed Parse sucessful for batchID : " + Convert.ToString(oD._BatchID), oD.edi, Me.ToString)

                    If sx.Message.Contains("deadlocked") Or sx.Message.Contains("timeout") Then
                        log.ExceptionDetails("52-Parse.Import271", "Dead lock rtrying  " + Convert.ToString(oD._BatchID) + " Deadlock count  " + Convert.ToString(_DeadLockCount), oD.edi, Me.ToString)
                        _DeadLockFlag = True
                        _DeadLockCount = _DeadLockCount + 1
                        Comit()  'Commented by Mohan - as per Suresh/Manoj - 09152016 
                        i = -1
                    End If

                Catch ex As Exception
                    i = -1

                    log.ExceptionDetails("53-" + oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import271 " + Convert.ToString(oD._BatchID), ex)
                    '

                    '% was deadlocked %' in SQL.Exception. IF Found RERUN with DELETE_FLAG='Y' only DUMP method
                Finally

                    sqlConn.Close()
                End Try




            Else



                log.ExceptionDetails("54-Parse.Import271", "Dead lock count execced giving up on   " + Convert.ToString(oD._BatchID) + " Deadlock count  " + Convert.ToString(_DeadLockCount), oD.edi, Me.ToString)
                i = -1

            End If


            Return i

        End Function



        Public Function ComitEPIC() As Integer

            Dim i As Integer

            If _DeadLockCount < oD._DeadLockRetrys Then

                Dim param As New SqlParameter()
                Dim sqlConn As SqlConnection = New SqlConnection
                Dim cmd As SqlCommand
                Dim sqlString As String = String.Empty

                sqlConn.ConnectionString = oD._ConnectionString
                sqlConn.Open()

                Try





                    sqlString = "usp_eligibility_response_dump_271_epic"




                    cmd = New SqlCommand(sqlString, sqlConn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.CommandTimeout = oD._CommandTimeOut
                    cmd.Parameters.AddWithValue("@HIPPA_ENVELOP", ENVELOP)
                    cmd.Parameters.AddWithValue("@HIPAA_EB", EB)
                    cmd.Parameters.AddWithValue("@HIPAA_AAA", AAA)
                    cmd.Parameters.AddWithValue("@HIPAA_AMT", AMT)
                    cmd.Parameters.AddWithValue("@HIPAA_DMG", DMG)
                    cmd.Parameters.AddWithValue("@HIPAA_BHT", BHT)
                    cmd.Parameters.AddWithValue("@HIPAA_DTP", DTP)
                    cmd.Parameters.AddWithValue("@HIPAA_EQ", EQ)
                    cmd.Parameters.AddWithValue("@HIPAA_HSD", HSD)
                    cmd.Parameters.AddWithValue("@HIPAA_III", III)
                    cmd.Parameters.AddWithValue("@HIPAA_INS", INS)
                    cmd.Parameters.AddWithValue("@HIPAA_N3", N3)
                    cmd.Parameters.AddWithValue("@HIPAA_N4", N4)
                    cmd.Parameters.AddWithValue("@HIPAA_MSG", MSG)
                    cmd.Parameters.AddWithValue("@HIPAA_NM1", NM1)
                    cmd.Parameters.AddWithValue("@HIPAA_PER", PER)
                    cmd.Parameters.AddWithValue("@HIPAA_PRV", PRV)
                    cmd.Parameters.AddWithValue("@HIPAA_REF", REF)
                    cmd.Parameters.AddWithValue("@HIPAA_TRN", TRN)
                    cmd.Parameters.AddWithValue("@HIPAA_UNK", UNK)
                    cmd.Parameters.AddWithValue("@ebr_id", oD.ebr_id)
                    cmd.Parameters.AddWithValue("@batch_id", oD._BatchID)
                    cmd.Parameters.AddWithValue("@user_id", oD.user_id)
                    cmd.Parameters.AddWithValue("@hosp_code", oD.hosp_code)
                    cmd.Parameters.AddWithValue("@source", oD.source)
                    cmd.Parameters.AddWithValue("@EDI", "ebr_ID=" + oD.ebr_id + "|" + "batch_ID=" + oD._BatchID + "|" + oD.edi)
                    cmd.Parameters.AddWithValue("@PAYOR_ID", oD.PAYOR_ID)
                    cmd.Parameters.AddWithValue("@Vendor_name", oD.Vendor_name)
                    cmd.Parameters.AddWithValue("@Log_EDI", oD.Log_EDI)
                    cmd.Parameters.Add("@Status", Data.SqlDbType.VarChar, 20)
                    cmd.Parameters("@Status").Direction = ParameterDirection.Output
                    cmd.Parameters.Add("@Reject_Reason_code", Data.SqlDbType.VarChar, 10)
                    cmd.Parameters("@Reject_Reason_code").Direction = ParameterDirection.Output
                    cmd.Parameters.Add("@LOOP_AGAIN", Data.SqlDbType.VarChar, 1)
                    cmd.Parameters("@LOOP_AGAIN").Direction = ParameterDirection.Output

                    If _DeadLockFlag Then
                        cmd.Parameters.AddWithValue("@DELETE_FLAG", "Y")
                    Else
                        cmd.Parameters.AddWithValue("@DELETE_FLAG", oD.DeleteFlag)
                    End If


                    'If (_isEPIC = True) Then
                    '    Then

                    '    cmd.Parameters.Add("@EPICOutEDIString", Data.SqlDbType.VarChar)
                    '    cmd.Parameters("@EPICOutEDIString").Direction = ParameterDirection.Output

                    'End If


                    err = cmd.ExecuteNonQuery()


                    _EPICOutEDIString = cmd.Parameters("@EPICOutEDIString").Value.ToString()



                    oD.Status = cmd.Parameters("@Status").Value.ToString()
                    oD.RejectReasonCode = cmd.Parameters("@Reject_Reason_code").Value.ToString()
                    oD.LoopAgain = cmd.Parameters("@LOOP_AGAIN").Value.ToString()

                    i = 0

                Catch sx As SqlException
                    log.ExceptionDetails("55-" + oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import271" + Me.ToString, sx)

                    log.ExceptionDetails("56-Parse.Import271", "Save to db failed Parse sucessful for batchID : " + Convert.ToString(oD._BatchID), oD.edi, Me.ToString)

                    If sx.Message.Contains("deadlocked") Or sx.Message.Contains("timeout") Then
                        log.ExceptionDetails("57-Parse.Import271", "Dead lock rtrying  " + Convert.ToString(oD._BatchID) + " Deadlock count  " + Convert.ToString(_DeadLockCount), oD.edi, Me.ToString)
                        _DeadLockFlag = True
                        _DeadLockCount = _DeadLockCount + 1
                        Comit()
                        i = -1
                    End If

                Catch ex As Exception
                    i = -1

                    log.ExceptionDetails("58-" + oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import271 " + Convert.ToString(oD._BatchID), ex)
                    '

                    '% was deadlocked %' in SQL.Exception. IF Found RERUN with DELETE_FLAG='Y' only DUMP method
                Finally

                    sqlConn.Close()
                End Try




            Else



                log.ExceptionDetails("59-Parse.Import271", "Dead lock count execced giving up on   " + Convert.ToString(oD._BatchID) + " Deadlock count  " + Convert.ToString(_DeadLockCount), oD.edi, Me.ToString)
                i = -1

            End If


            Return i

        End Function


        'Public Function ComitEX() As Integer


        '    Dim r As Integer = -1


        '    r = ComitTables()

        '    If r < 0 Then
        '        'cleanup
        '        Return -1

        '    End If


        '    r = SetParseStatus()

        '    If r = -2 Then

        '        For x = 0 To _DeadLockCount Step 1

        '            If SetParseStatus() = 0 Then
        '                Exit For
        '            Else
        '                'cleanup
        '                Return -2
        '            End If

        '        Next
        '        'clean up 
        '    End If


        '    r = RulesVBParse()

        '    If r < 0 Then
        '        'cleanup
        '        Return -1

        '    End If


        '    r = SetRules()

        '    If r < 0 Then
        '        'cleanup
        '        Return -1

        '    End If




        '    Return r

        'End Function



        'Public Function ComitTables() As Integer
        '    Dim r As Integer = -1

        '    Using cn As New SqlConnection(oD._ConnectionString)
        '        cn.Open()

        '        Using cmd As New SqlCommand("usp_eligibility_response_dump", cn)
        '            Try

        '                cmd.Connection = cn
        '                cmd.CommandType = CommandType.StoredProcedure
        '                cmd.Parameters.AddWithValue("@HIPPA_ENVELOP", ENVELOP)
        '                cmd.Parameters.AddWithValue("@HIPAA_EB", EB)
        '                cmd.Parameters.AddWithValue("@HIPAA_AAA", AAA)
        '                cmd.Parameters.AddWithValue("@HIPAA_AMT", AMT)
        '                cmd.Parameters.AddWithValue("@HIPAA_DMG", DMG)
        '                cmd.Parameters.AddWithValue("@HIPAA_BHT", BHT)
        '                cmd.Parameters.AddWithValue("@HIPAA_DTP", DTP)
        '                cmd.Parameters.AddWithValue("@HIPAA_EQ", EQ)
        '                cmd.Parameters.AddWithValue("@HIPAA_HSD", HSD)
        '                cmd.Parameters.AddWithValue("@HIPAA_III", III)
        '                cmd.Parameters.AddWithValue("@HIPAA_INS", INS)
        '                cmd.Parameters.AddWithValue("@HIPAA_N3", N3)
        '                cmd.Parameters.AddWithValue("@HIPAA_N4", N4)
        '                cmd.Parameters.AddWithValue("@HIPAA_MSG", MSG)
        '                cmd.Parameters.AddWithValue("@HIPAA_NM1", NM1)
        '                cmd.Parameters.AddWithValue("@HIPAA_PER", PER)
        '                cmd.Parameters.AddWithValue("@HIPAA_PRV", PRV)
        '                cmd.Parameters.AddWithValue("@HIPAA_REF", REF)
        '                cmd.Parameters.AddWithValue("@HIPAA_TRN", TRN)
        '                cmd.Parameters.AddWithValue("@HIPAA_UNK", UNK)
        '                cmd.Parameters.AddWithValue("@ebr_id", oD.ebr_id)
        '                cmd.Parameters.AddWithValue("@batch_id", oD._BatchID)
        '                cmd.Parameters.AddWithValue("@user_id", oD.user_id)
        '                cmd.Parameters.AddWithValue("@hosp_code", oD.hosp_code)
        '                cmd.Parameters.AddWithValue("@source", oD.source)
        '                cmd.Parameters.AddWithValue("@EDI", oD.edi)
        '                cmd.Parameters.AddWithValue("@PAYOR_ID", oD.PAYOR_ID)
        '                cmd.Parameters.AddWithValue("@Vendor_name", oD.Vendor_name)
        '                cmd.Parameters.AddWithValue("@Log_EDI", oD.Log_EDI)
        '                cmd.Parameters.Add("@Status", Data.SqlDbType.VarChar, 20)
        '                cmd.Parameters("@Status").Direction = ParameterDirection.Output
        '                cmd.Parameters.Add("@Reject_Reason_code", Data.SqlDbType.VarChar, 10)
        '                cmd.Parameters("@Reject_Reason_code").Direction = ParameterDirection.Output
        '                cmd.Parameters.Add("@LOOP_AGAIN", Data.SqlDbType.VarChar, 1)
        '                cmd.Parameters("@LOOP_AGAIN").Direction = ParameterDirection.Output

        '                cmd.ExecuteNonQuery()
        '                r = 0

        '            Catch sx As SqlException
        '                log.ExceptionDetails("DCSGlobal.EDI.Import271.ComitTables" + Me.ToString, sx)
        '                log.ExceptionDetails("DCSGlobal.EDI.Import271.ComitTables", "Save to db failed Parse sucessful for batchID : " + Convert.ToString(oD._BatchID), oD.edi, Me.ToString)

        '                If sx.Message.Contains("deadlocked") Or sx.Message.Contains("timeout") Then
        '                    ' log.ExceptionDetails("DCSGlobal.EDI.Import271.RulesVBParse", "Dead lock rtrying  " + Convert.ToString(oD._BatchID) + " Deadlock count  " + Convert.ToString(_DeadLockCount), oD.edi, Me.ToString)
        '                    r = -2
        '                End If

        '            Catch ex As Exception
        '                log.ExceptionDetails("DCSGlobal.EDI.Import271.ComitTables" + Convert.ToString(oD._BatchID), ex)
        '                r = -1
        '            End Try
        '        End Using
        '    End Using


        '    Return r
        'End Function



        'Public Function SetParseStatus() As Integer
        '    Dim r As Integer = -1

        '    Using cn As New SqlConnection(oD._ConnectionString)
        '        cn.Open()

        '        Using cmd As New SqlCommand("usp_eligibility_set_parse_status_v2", cn)
        '            Try

        '                cmd.Connection = cn
        '                cmd.CommandType = CommandType.StoredProcedure
        '                cmd.Parameters.AddWithValue("@HIPAA_EB", EB)
        '                cmd.Parameters.AddWithValue("@HIPAA_AAA", AAA)
        '                cmd.Parameters.AddWithValue("@HIPPA_ENVELOP", ENVELOP)
        '                cmd.Parameters.AddWithValue("@EDI", oD.edi)
        '                cmd.Parameters.AddWithValue("@ebr_id", oD.ebr_id)
        '                cmd.Parameters.AddWithValue("@batch_id", oD._BatchID)
        '                cmd.Parameters.AddWithValue("@user_id", oD.user_id)
        '                cmd.Parameters.AddWithValue("@hosp_code", oD.hosp_code)
        '                cmd.Parameters.AddWithValue("@source", oD.source)
        '                cmd.Parameters.AddWithValue("@PAYOR_ID", oD.PAYOR_ID)


        '                cmd.Parameters.Add("@Status", Data.SqlDbType.VarChar, 20)
        '                cmd.Parameters("@Status").Direction = ParameterDirection.Output
        '                cmd.Parameters.Add("@Reject_Reason_code", Data.SqlDbType.VarChar, 10)
        '                cmd.Parameters("@Reject_Reason_code").Direction = ParameterDirection.Output
        '                cmd.Parameters.Add("@LOOP_AGAIN", Data.SqlDbType.VarChar, 1)
        '                cmd.Parameters("@LOOP_AGAIN").Direction = ParameterDirection.Output


        '                cmd.ExecuteNonQuery()


        '                oD.Status = cmd.Parameters("@Status").Value.ToString()
        '                oD.RejectReasonCode = cmd.Parameters("@Reject_Reason_code").Value.ToString()
        '                oD.LoopAgain = cmd.Parameters("@LOOP_AGAIN").Value.ToString()


        '            Catch sx As SqlException
        '                log.ExceptionDetails("DCSGlobal.EDI.Import271.SetParseStatus" + Me.ToString, sx)
        '                log.ExceptionDetails("DCSGlobal.EDI.Import271.SetParseStatus", "Save to db failed Parse sucessful for batchID : " + Convert.ToString(oD._BatchID), oD.edi, Me.ToString)

        '                If sx.Message.Contains("deadlocked") Or sx.Message.Contains("timeout") Then
        '                    ' log.ExceptionDetails("DCSGlobal.EDI.Import271.RulesVBParse", "Dead lock rtrying  " + Convert.ToString(oD._BatchID) + " Deadlock count  " + Convert.ToString(_DeadLockCount), oD.edi, Me.ToString)
        '                    r = -2
        '                End If

        '            Catch ex As Exception
        '                log.ExceptionDetails("DCSGlobal.EDI.Import271.SetParseStatus" + Convert.ToString(oD._BatchID), ex)
        '                r = -1
        '            End Try
        '        End Using
        '    End Using



        '    Return r
        'End Function


        'Public Function RulesVBParse() As Integer

        '    Dim r As Integer = -1


        '    Using cn As New SqlConnection(oD._ConnectionString)
        '        cn.Open()

        '        Using cmd As New SqlCommand("usp_eligibility_set_rules_status_db", cn)
        '            Try
        '                cmd.Connection = cn
        '                cmd.CommandType = CommandType.StoredProcedure
        '                cmd.Parameters.AddWithValue("@HIPAA_EB", EB)
        '                cmd.Parameters.AddWithValue("@HIPAA_REF", REF)
        '                cmd.Parameters.AddWithValue("@STR_USER", oD.user_id)
        '                cmd.Parameters.AddWithValue("@batch_id", oD._BatchID)
        '                cmd.Parameters.AddWithValue("@DEBUG", "N")
        '                cmd.ExecuteNonQuery()
        '                r = 0
        '            Catch sx As SqlException
        '                log.ExceptionDetails("DCSGlobal.EDI.Import271.RulesVBParse" + Me.ToString, sx)
        '                log.ExceptionDetails("DCSGlobal.EDI.Import271.RulesVBParse", "Save to db failed Parse sucessful for batchID : " + Convert.ToString(oD._BatchID), oD.edi, Me.ToString)

        '                If sx.Message.Contains("deadlocked") Or sx.Message.Contains("timeout") Then
        '                    ' log.ExceptionDetails("DCSGlobal.EDI.Import271.RulesVBParse", "Dead lock rtrying  " + Convert.ToString(oD._BatchID) + " Deadlock count  " + Convert.ToString(_DeadLockCount), oD.edi, Me.ToString)
        '                    r = -2
        '                End If

        '            Catch ex As Exception
        '                log.ExceptionDetails("DCSGlobal.EDI.Import271.RulesVBParse" + Convert.ToString(oD._BatchID), ex)
        '                r = -1
        '            End Try
        '        End Using
        '    End Using


        '    Return r
        'End Function









        'Public Function SetRules() As Integer

        '    Dim r As Integer = -1

        '    Using cn As New SqlConnection(oD._ConnectionString)
        '        cn.Open()

        '        Using cmd As New SqlCommand("usp_eligibility_set_rules_status_db", cn)
        '            Try

        '                cmd.Connection = cn
        '                cmd.CommandType = CommandType.StoredProcedure
        '                cmd.Parameters.AddWithValue("@batch_id", oD._BatchID)
        '                cmd.ExecuteNonQuery()
        '                r = 0

        '            Catch sx As SqlException
        '                log.ExceptionDetails("Parse.Import271", "SetRules : " + Convert.ToString(oD._BatchID), oD.edi, Me.ToString)
        '                r = -1
        '            Catch ex As Exception
        '                log.ExceptionDetails("DCSGlobal.EDI.Import271.SetRules " + Convert.ToString(oD._BatchID), ex)
        '                r = -1
        '            End Try
        '        End Using
        '    End Using


        '    Return r
        'End Function




        'Public Function SetProceesedFlagN() As Integer

        '    Dim r As Integer = -1

        '    Using cn As New SqlConnection(oD._ConnectionString)
        '        cn.Open()

        '        Using cmd As New SqlCommand("usp_eligibility_set_rules_status_db", cn)
        '            Try

        '                cmd.Connection = cn
        '                cmd.CommandType = CommandType.StoredProcedure
        '                cmd.Parameters.AddWithValue("@batch_id", oD._BatchID)
        '                cmd.ExecuteNonQuery()
        '                r = 0

        '            Catch sx As SqlException
        '                log.ExceptionDetails("Parse.Import271", "SetRules : " + Convert.ToString(oD._BatchID), oD.edi, Me.ToString)
        '                r = -1
        '            Catch ex As Exception
        '                log.ExceptionDetails("DCSGlobal.EDI.Import271.SetRules " + Convert.ToString(oD._BatchID), ex)
        '                r = -1
        '            End Try
        '        End Using
        '    End Using


        '    Return r
        'End Function



    End Class


End Namespace

