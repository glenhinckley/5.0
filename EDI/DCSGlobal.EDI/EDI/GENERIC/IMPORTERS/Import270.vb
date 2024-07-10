Option Explicit On
Option Compare Binary



Imports DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff
Imports System.Text.RegularExpressions
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Collections.Generic
Imports DCSGlobal.BusinessRules.Logging


Namespace DCSGlobal.EDI


    Public Class Import270


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
        Public meddd As IEnumerable
        Dim mmm As Integer = 1

        Public Event OnPreParse As EventHandler
        Private _EQ02HasData As Boolean = False
        Dim val As New ValidateEDI
        Dim RE As Integer
        Dim ss As New StringStuff


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



        Public WriteOnly Property ConnectionString As String

            Set(value As String)
                oD._ConnectionString = value
                log.ConnectionString = value
            End Set
        End Property
        Public WriteOnly Property Dump As Integer

            Set(value As Integer)
                oD.Dump = value
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


        Public WriteOnly Property DeleteFlag As String

            Set(value As String)
                oD.DeleteFlag = value
            End Set
        End Property



        Public WriteOnly Property Vendor_name As String

            Set(value As String)
                oD.Vendor_name = value
            End Set
        End Property




        Public WriteOnly Property pat_hosp_code As String

            Set(value As String)
                oD.pat_hosp_code = value
            End Set
        End Property



        Public WriteOnly Property PAYOR_ID As String

            Set(value As String)
                oD.PAYOR_ID = value
            End Set
        End Property


        Public WriteOnly Property ins_type As String

            Set(value As String)
                oD.ins_type = value
            End Set
        End Property




        Public WriteOnly Property Patient_number As String

            Set(value As String)
                oD.Patient_number = value
            End Set
        End Property

        Public Property ReqServiceTypeCode As String
            Get
                Return oD.ServiceTypeCode
            End Get
            Set(value As String)
                oD.ServiceTypeCode = value
            End Set
        End Property

        Public Property SearchType As String
            Get
                Return oD.SearchType
            End Get
            Set(value As String)
                oD.SearchType = value
            End Set
        End Property



        Public Property ReqNPI As String
            Get
                Return oD.NPI
            End Get
            Set(value As String)
                oD.NPI = value
            End Set
        End Property


        Public Function Import() As Integer

            '
            Dim r As Integer = -1
            Dim sqlString = String.Empty



            start_time = Now
            oD.TimeStamp = CDate(FormatDateTime(start_time, DateFormat.ShortDate))


            '*************************************************************************************************************
            '
            '
            '
            '**************************************************************************************************************

            oD.BATCHUID = Guid.NewGuid()


            ' validate EDI 

            ' populate the missing stuff 
            ' then calll log


            'add the logging back here   

            'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

            'Using ARL As New AuditResponseLogging
            '    ARL.ConnectionString = oD._ConnectionString
            '    ARL.LOG270(oD._BatchID, oD.PAYOR_ID, oD.edi, oD.Vendor_name)
            'End Using

            'If oD.Vendor_name = "MEDDATA" Then
            '    Try
            '        'MEDDATA is in xml format.
            '        oD.ServiceTypeCode = ss.ProcessAVAILITYXML(oD.edi, "serviceTypeCode")
            '    Catch ex As Exception
            '        oD.ServiceTypeCode = ""
            '    End Try
            '    Try
            '        'MEDDATA is in xml format.
            '        oD.NPI = ss.ProcessAVAILITYXML(oD.edi, "providerId")
            '    Catch ex As Exception
            '        oD.NPI = ""
            '    End Try
            'Else
            '    'NON MEDDATA ie. They are in EDI format  
            '    Using vedi As New ValidateEDI()
            '        r = vedi.byString(oD.edi)
            '        If r = 0 Then
            '            oD.ServiceTypeCode = vedi.ServiceTypeCode
            '            oD.NPI = vedi.NPI
            '        End If
            '    End Using
            'End If


            '' check to see IF we have a bactch ID  if not get out 
            'If oD._BatchID = 0 Then
            '    Return -1
            '    Exit Function
            'End If


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

            End If


            oD.idx_EDI271 = 0




            oD.ediTildeCount = Regex.Matches(oD.edi, Regex.Escape("~")).Count 'InStr(nt(edi,'~'))   






            For idx_EDI271 = 1 To oD.ediTildeCount

                oD.DataElementSeparator = Mid(oD.edi, 4, 1)


                If idx_EDI271 = 5000 Then
                    Return -2
                    Exit Function
                End If


                oD.ediRowData = objss.ParseDemlimtedStringEDI(oD.edi, oD.SegmentTerminator, CInt(idx_EDI271))
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





                    Select Case (oD.ESFlagStart = 1)
                        'begin

                        Case True

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



                        Case False

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

                    oD.GS_ROW_ID = CInt(row.Item("ROW_ID"))
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


                        Case "1"
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
                                oD.HL1CHILDCOUNT = CInt(oD.HL04Data)
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


                        Case "2"





                            oD.CurrentHLGroup = "21"


                            oD.HL01Data = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)
                            oD.HL02Data = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                            oD.HL03Data = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)
                            oD.HL04Data = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 5)

                            oD.HL21_GUID = Guid.NewGuid

                            oD.P_GUID = oD.HL21_GUID
                            oD.HL2CHILDCOUNT = CInt(oD.HL04Data)
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


                        Case "3"
                            '                  BEGIN()
                            oD.CurrentHLGroup = "22"

                            oD.HL01Data = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)
                            oD.HL02Data = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                            oD.HL03Data = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)
                            oD.HL04Data = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 5)

                            oD.HL22_GUID = Guid.NewGuid

                            oD.P_GUID = oD.HL22_GUID
                            oD.HL3CHILDCOUNT = CInt(oD.HL04Data)
                            oD.HLCANCELFLAG = oD.HL3CHILDCOUNT

                            Dim HL03Row As DataRow = HL03.NewRow
                            HL03Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                            HL03Row("HIPAA_GS_GUID") = oD.GS_GUID
                            HL03Row("HIPAA_ST_GUID") = oD.ST_GUID
                            HL03Row("HIPAA_HL01_GUID") = oD.HL20_GUID
                            HL03Row("HIPAA_HL02_GUID") = oD.HL21_GUID
                            '    HL03Row("HIPAA_HL03_GUID") = oD.HL22_GUID
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


                        Case "4"

                            oD.CurrentHLGroup = "23"


                            oD.HL01Data = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)
                            oD.HL02Data = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                            oD.HL03Data = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)
                            oD.HL04Data = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 5)

                            oD.HL23_GUID = Guid.NewGuid

                            oD.P_GUID = oD.HL23_GUID
                            oD.HL4CHILDCOUNT = CInt(oD.HL04Data)
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





                '******************************************************************************************************************************
                '  Begin  EB
                '******************************************************************************************************************************
                If oD.ROW_RECORD_TYPE = "EB" Then


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
                    EBRow("EB01") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)
                    EBRow("EB02") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                    EBRow("EB03") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)
                    EBRow("EB04") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 5)
                    EBRow("EB05") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 6)
                    EBRow("EB06") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 7)
                    EBRow("EB07") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 8)
                    EBRow("EB08") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 9)
                    EBRow("EB09") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 10)
                    EBRow("EB10") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 11)
                    EBRow("EB11") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 12)
                    EBRow("EB12") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 13)
                    EBRow("EB13") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 14)
                    EBRow("EB13_1") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 15)
                    EBRow("EB13_2") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 16)
                    EBRow("EB13_3") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 17)
                    EBRow("EB13_4") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 18)
                    EBRow("EB13_5") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 19)
                    EBRow("EB13_6") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 20)
                    EBRow("EB13_7") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 21)
                    EBRow("TIME_STAMP") = oD.TimeStamp
                    EBRow("BATCH_ID") = oD._BatchID
                    EB.Rows.Add(EBRow)



                    oD.RowProcessedFlag = 1
                End If
                '******************************************************************************************************************************
                '   End EB
                '******************************************************************************************************************************

                'BEGIN AAA
                If oD.ROW_RECORD_TYPE = "AAA" Then



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



                End If
                'END AAA


                'BEGIN AMT
                If oD.ROW_RECORD_TYPE = "AMT" Then



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




                End If
                'END AMT


                'BEGIN DMG
                If oD.ROW_RECORD_TYPE = "DMG" Then


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



                End If
                'END 

                'BEGIN DTP
                If oD.ROW_RECORD_TYPE = "DTP" Then



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


                End If
                'END 




                'BEGIN EQ
                If oD.ROW_RECORD_TYPE = "EQ" Then

                    _EQ02HasData = False




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

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then
                        _EQ02HasData = True
                    End If


                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then EQRow("EQ02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else EQRow("EQ02") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then EQRow("EQ02_1") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else EQRow("EQ02_1") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then EQRow("EQ02_2") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else EQRow("EQ02_2") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then EQRow("EQ02_3") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else EQRow("EQ02_3") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then EQRow("EQ02_4") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else EQRow("EQ02_4") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then EQRow("EQ02_5") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else EQRow("EQ02_5") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then EQRow("EQ02_6") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else EQRow("EQ02_6") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then EQRow("EQ02_7") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else EQRow("EQ02_7") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) <> "") Then EQRow("EQ03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) Else EQRow("EQ03") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) <> "") Then EQRow("EQ04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) Else EQRow("EQ04") = DBNull.Value


                    'EQRow("EQ01") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 2)
                    'EQRow("EQ02") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 3)
                    'EQRow("EQ02_1") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 4)
                    'EQRow("EQ02_2") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 5)
                    'EQRow("EQ02_3") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 6)
                    'EQRow("EQ02_4") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 7)
                    'EQRow("EQ02_5") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 8)
                    'EQRow("EQ02_6") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 9)
                    'EQRow("EQ02_7") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 10)
                    'EQRow("EQ03") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 11)
                    'EQRow("EQ04") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 12)

                    EQRow("BATCH_ID") = oD._BatchID
                    EQRow("TIME_STAMP") = oD.TimeStamp
                    EQ.Rows.Add(EQRow)

                    oD.RowProcessedFlag = 1



                End If
                'END EQ



                'BEGIN HSD
                If oD.ROW_RECORD_TYPE = "HSD" Then



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



                End If
                'END 


                'BEGIN HL
                If oD.ROW_RECORD_TYPE = "HL" Then



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


                    'HLRow("HL01") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 2)
                    'HLRow("HL02") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 3)
                    'HLRow("HL03") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 4)
                    'HLRow("HL04") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 5)
                    HLRow("BATCH_ID") = oD._BatchID
                    HLRow("TIME_STAMP") = oD.TimeStamp
                    HL.Rows.Add(HLRow)

                    oD.RowProcessedFlag = 1


                End If
                'END HL



                'BEGIN III
                If oD.ROW_RECORD_TYPE = "III" Then



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




                    'IIIRow("III01") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 2)
                    'IIIRow("III02") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 3)
                    'IIIRow("III03") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 4)
                    'IIIRow("III04") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 5)
                    'IIIRow("III05") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 6)
                    'IIIRow("III06") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 7)
                    'IIIRow("III07") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 8)
                    'IIIRow("III08") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 9)
                    IIIRow("BATCH_ID") = oD._BatchID
                    IIIRow("TIME_STAMP") = oD.TimeStamp
                    III.Rows.Add(IIIRow)

                    oD.RowProcessedFlag = 1




                End If
                'END III





                'BEGIN INS
                If oD.ROW_RECORD_TYPE = "INS" Then


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






                    'INSRow("INS01") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 2)
                    'INSRow("INS02") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 3)
                    'INSRow("INS03") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 4)
                    'INSRow("INS04") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 5)
                    'INSRow("INS05") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 6)
                    'INSRow("INS06") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 7)
                    'INSRow("INS07") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 8)
                    'INSRow("INS08") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 9)
                    'INSRow("INS09") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 10)
                    'INSRow("INS10") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 11)
                    'INSRow("INS11") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 12)
                    'INSRow("INS12") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 13)
                    'INSRow("INS13") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 14)
                    'INSRow("INS14") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 15)
                    'INSRow("INS15") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 16)
                    'INSRow("INS16") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 17)
                    'INSRow("INS17") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 18)
                    INSRow("BATCH_ID") = oD._BatchID
                    INSRow("TIME_STAMP") = oD.TimeStamp
                    INS.Rows.Add(INSRow)

                    oD.RowProcessedFlag = 1



                End If
                'END INS




                'BEGIN MSG
                If oD.ROW_RECORD_TYPE = "MSG" Then


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



                End If
                'END MSG





                'BEGIN N3
                If oD.ROW_RECORD_TYPE = "N3" Then

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




                    'N3Row("N301") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 2)
                    'N3Row("N302") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 3)
                    N3Row("BATCH_ID") = oD._BatchID
                    N3Row("TIME_STAMP") = oD.TimeStamp
                    N3.Rows.Add(N3Row)

                    oD.RowProcessedFlag = 1

                End If
                'END N3

                'BEGIN N4
                If oD.ROW_RECORD_TYPE = "N4" Then

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




                    'N4Row("N401") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 2)
                    'N4Row("N402") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 3)
                    'N4Row("N403") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 4)
                    'N4Row("N404") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 5)
                    'N4Row("N405") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 6)
                    'N4Row("N406") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 7)
                    N4Row("BATCH_ID") = oD._BatchID
                    N4Row("TIME_STAMP") = oD.TimeStamp
                    N4.Rows.Add(N4Row)

                    oD.RowProcessedFlag = 1



                End If
                'END N4


                'BEGIN NM1
                If oD.ROW_RECORD_TYPE = "NM1" Then



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



                    'NM1Row("NM101") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 2)
                    'NM1Row("NM102") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 3)
                    'NM1Row("NM103") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 4)
                    'NM1Row("NM104") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 5)
                    'NM1Row("NM105") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 6)
                    'NM1Row("NM106") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 7)
                    'NM1Row("NM107") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 8)
                    'NM1Row("NM108") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 9)
                    'NM1Row("NM109") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 10)
                    'NM1Row("NM110") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 11)
                    'NM1Row("NM111") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 12)
                    NM1Row("BATCH_ID") = oD._BatchID
                    NM1Row("TIME_STAMP") = oD.TimeStamp
                    NM1.Rows.Add(NM1Row)

                    oD.RowProcessedFlag = 1




                End If
                'END NM1


                'BEGIN PER
                If oD.ROW_RECORD_TYPE = "PER" Then





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





                    'PERRow("PER01") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 2)
                    'PERRow("PER02") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 3)
                    'PERRow("PER03") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 4)
                    'PERRow("PER04") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 5)
                    'PERRow("PER05") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 6)
                    'PERRow("PER06") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 7)
                    'PERRow("PER07") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 8)
                    'PERRow("PER08") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 9)
                    'PERRow("PER09") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 10)
                    PERRow("BATCH_ID") = oD._BatchID
                    PERRow("TIME_STAMP") = oD.TimeStamp
                    PER.Rows.Add(PERRow)

                    oD.RowProcessedFlag = 1


                End If
                'END PER







                'BEGIN REF
                If oD.ROW_RECORD_TYPE = "REF" Then


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


                End If
                'END REF




                'BEGIN PER
                If oD.ROW_RECORD_TYPE = "PRV" Then
                    'BEGIN()




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


                    'PRVRow("PRV01") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 2)
                    'PRVRow("PRV02") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 3)
                    'PRVRow("PRV03") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 4)
                    'PRVRow("PRV04") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 5)
                    'PRVRow("PRV05") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 6)
                    'PRVRow("PRV06") = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.RowDataDelimiter, 7)


                    PRVRow("BATCH_ID") = oD._BatchID
                    PRVRow("TIME_STAMP") = oD.TimeStamp
                    PRV.Rows.Add(PRVRow)

                    oD.RowProcessedFlag = 1


                    'od.STIdentity = @@IDENTITY;  
                    ' --od.STFlag = 1;  
                    oD.RowProcessedFlag = 1


                End If
                'END PER








                'BEGIN TRN
                If oD.ROW_RECORD_TYPE = "TRN" Then
                    'BEGIN()


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


                    TRNRow("BATCH_ID") = oD._BatchID
                    TRNRow("TIME_STAMP") = oD.TimeStamp
                    TRN.Rows.Add(TRNRow)
                    oD.RowProcessedFlag = 1

                End If
                'END TRN









                If oD.ROW_RECORD_TYPE = "EE" Then

                    oD.EBFlag = 0
                    oD.RowProcessedFlag = 1
                End If





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







            stop_time = Now
            elapsed_time = stop_time.Subtract(start_time)
            elapsed_time.TotalSeconds.ToString("0.000000")


            Return 0

        End Function






        Public Function Comit() As Int64


            Dim i As Int64
            Dim param As New SqlParameter()


            Dim sqlConn As SqlConnection = New SqlConnection
            Dim cmd As SqlCommand
            Dim sqlString As String



            sqlConn.ConnectionString = oD._ConnectionString
            sqlConn.Open()




            Try


                sqlString = "usp_eligibility_response_dump_270"


                cmd = New SqlCommand(sqlString, sqlConn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandTimeout = oD._CommandTimeOut
                cmd.Parameters.AddWithValue("@HIPPA_270_ENVELOP", ENVELOP)
                ' cmd.Parameters.AddWithValue("@HIPAA_EB", EB)
                cmd.Parameters.AddWithValue("@HIPAA_AAA_270", AAA)
                cmd.Parameters.AddWithValue("@HIPAA_AMT_270", AMT)
                cmd.Parameters.AddWithValue("@HIPAA_DMG_270", DMG)
                cmd.Parameters.AddWithValue("@HIPAA_BHT_270", BHT)
                cmd.Parameters.AddWithValue("@HIPAA_DTP_270", DTP)
                cmd.Parameters.AddWithValue("@HIPAA_EQ_270", EQ)
                'cmd.Parameters.AddWithValue("@HIPAA_270_HSD", HSD)
                'cmd.Parameters.AddWithValue("@HIPAA_270_III", III)
                cmd.Parameters.AddWithValue("@HIPAA_INS_270", INS)
                cmd.Parameters.AddWithValue("@HIPAA_N3_270", N3)
                cmd.Parameters.AddWithValue("@HIPAA_N4_270", N4)
                'cmd.Parameters.AddWithValue("@HIPAG", MSG)
                cmd.Parameters.AddWithValue("@HIPAA_NM1_270", NM1)
                cmd.Parameters.AddWithValue("@HIPAA_PER_270", PER)
                cmd.Parameters.AddWithValue("@HIPAA_PRV_270", PRV)
                cmd.Parameters.AddWithValue("@HIPAA_REF_270", REF)
                cmd.Parameters.AddWithValue("@HIPAA_TRN_270", TRN)
                cmd.Parameters.AddWithValue("@HIPAA_UNK_270", UNK)
                cmd.Parameters.AddWithValue("@DELETE_FLAG", oD.DeleteFlag)
                cmd.Parameters.AddWithValue("@ebr_id", oD.ebr_id)

                cmd.Parameters.AddWithValue("@user_id", oD.user_id)
                cmd.Parameters.AddWithValue("@hosp_code", oD.hosp_code)
                cmd.Parameters.AddWithValue("@source", oD.source)

                cmd.Parameters.AddWithValue("@vendor_name", oD.Vendor_name)
                cmd.Parameters.AddWithValue("@pat_hosp_code", oD.pat_hosp_code)
                cmd.Parameters.AddWithValue("@ins_type", oD.ins_type)
                cmd.Parameters.AddWithValue("@Payor_id", oD.PAYOR_ID)
                cmd.Parameters.AddWithValue("@Patient_number", oD.Patient_number)


                'cmd.Parameters.AddWithValue("@REQ_NPI", oD.NPI)
                'cmd.Parameters.AddWithValue("@REQUESTED_SERVICE_TYPES_270", oD.ServiceTypeCode)








                cmd.Parameters.AddWithValue("@request_xml", oD.edi)


                cmd.Parameters.AddWithValue("@C_SEARCH_TYPE", oD.SearchType)

                cmd.Parameters.AddWithValue("@batch_id", oD._BatchID)
                cmd.Parameters("@batch_id").Direction = ParameterDirection.InputOutput


                '   log.ExceptionDetails("62-Parse.Import270", " Begin Comit" + Convert.ToString(oD.ebr_id))
                err = Convert.ToString(cmd.ExecuteNonQuery())
                '  log.ExceptionDetails("63-Parse.Import270", " end Comit " + Convert.ToString(oD.ebr_id) + " " + err)

                i = Convert.ToInt64(cmd.Parameters("@batch_id").Value)

            Catch ex As Exception
                i = -1
                err = ex.Message

                log.ExceptionDetails("47-" + oD.Version + "  " + _ClassVersion + " " + "EDI.Import_270.comit", ex.Message, ex.StackTrace, Me.ToString)

            Finally

                sqlConn.Close()
            End Try

            Return i

        End Function




    End Class



End Namespace




