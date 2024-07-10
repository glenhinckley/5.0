Option Explicit On

Imports DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff
Imports System.Text.RegularExpressions
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Collections.Generic
'Imports System.Data.DataSetExtensions
Imports DCSGlobal.BusinessRules.Logging
Imports System.IO
Imports System.Text
Imports System.Threading

Namespace DCSGlobal.EDI


    Public Class Import278


        Inherits EDI278Tables
        Implements IDisposable


        Protected disposed As Boolean = False

        Private _ClassVersion As String = "2.0"
        Private start_time As DateTime
        Private stop_time As DateTime
        Private elapsed_time As TimeSpan
        Private oD As New Declarations
        Private objss As New StringStuff
        Private _TablesBuilt As Boolean = False
        Private _ENFlag As Boolean = False
        Private distinctDT As DataTable
        Private err As String = ""
        Private _chars As String
        Private STFlag As Integer = 0
        'Private LXFlag As Integer = 0
        'Private CLPFlag As Integer = 0
        Private FileID As Int32 = 0

        Private LXList As New List(Of String)

        Private log As New logExecption
        Private _HSDCount As Integer
        Private _Debug = 0
        Private V As Boolean = False

        Private _FileToParse As String
        Private _CLPString As String

        Private SVC01 As String = String.Empty
        Private SVC06 As String = String.Empty

        Private _EDIList As New List(Of String)



        '       @EDI varchar(max)='',

        '@PAYOR_ID varchar(50)='',

        '@Vendor_name varchar(50)='',

        '@Log_EDI varchar(1)='N'



        Public meddd As IEnumerable
        Private mmm As Integer = 1
        Private lxline As String
        Private PLB03 As String
        Private PLB05 As String
        Private PLB07 As String
        Private PLB09 As String
        Private PLB11 As String
        Private PLB13 As String
        Private _CLMString As Object
        Private CLMFlag As Integer
        Private HI01 As String
        Private HI02 As String
        Private HI03 As String
        Private HI04 As String
        Private HI05 As String
        Private HI06 As String
        Private HI07 As String
        Private HI08 As String
        Private HI09 As String
        Private HI10 As String
        Private HI11 As String
        Private HI12 As String
        Private SV202 As String
        Private CTP05 As String
        Dim SV101 As String
        Dim SV107 As String
        Dim UM05 As String
        Dim SV301 As String
        Dim SV304 As String
        Dim TOO03 As String
        Dim _FileID As Integer = -1
        Dim _BatchID As Double
        Dim FileToParse As String
        Dim _DeadLockFlag As Boolean

        Public Event OnPreParse As EventHandler




        Public Event OnPostParse As EventHandler




        ' =============================================  
        ' Author:   <Jyothi Kunkunoori>  
        ' Create date:  <04-01-2014>  
        ' Release Date: <__-__-2013>  
        ' Description:  <EDI 837 Parsser>  
        ' Revison:   <ALPHA>  
        ' =============================================  

        ' THIS IS NOT THE MASTER COPY, DO NOT COPY, EDIT or ALTER ALL CHANGES WILL BE LOST  
        ' MASTER COPY IS IN VSS UNDER GLENS TEMP PROJECTS  

        '***************************************************************************************************   
        'Change Log  
        ' **************************************************************************************************  
        'EXEC [usp_edi_import_837] (@batchid bigint, @commit int, @DebugLevel int, @edi varchar(max), @UsePARMS int, @ComponentElementSeperatorEX varchar(1), @EBRowDelimiterEX varchar(1),@outISAUIdentity as varchar(300))  

        'alter table request add im_before_ts datetime , im_after_ts datetime

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

        Private Property UM04 As String
        Public Function ImportByList(ByVal EDIList As List(Of String), ByVal BatchID As Double) As Int32

            Dim x As Integer = -1
            _EDIList = EDIList
            _BatchID = BatchID

            x = Import278()

            Return x

        End Function
        Public Function ImportByString(ByVal EDI As String, ByVal BatchID As Double) As Integer

            Dim r As Integer = -1



            Dim SP As New StringPrep()
            _BatchID = BatchID

            _EDIList = SP.SingleEDI(EDI)

            r = Import278()
            Return r

        End Function

        Public Function ImportByFileName(ByVal FilePath As String, ByVal FileName As String) As Integer

            Dim r As Integer = -1


            Using el As New EDIListBuilder

                el.ConnectionString = oD._ConnectionString
                r = el.BuildListByFile(FilePath + FileName)

                If r = 0 Then
                    _EDIList = el.EDIList
                End If
            End Using


            If r = 0 Then


                r = InsertFileName(FilePath, FileName)

                If r = 0 Then

                    r = Import278()

                End If
            End If


            Return r

        End Function

        Private Function Import278() As Int32

            '_FileToParse = FileToParse

            'Dim iEndIndex As Integer
            'Dim iStartIndex As Integer
            'Dim sFolderName As String = ""
            'Try
            '    iEndIndex = _FileToParse.IndexOf("\input")
            '    sFolderName = _FileToParse.Substring(0, iEndIndex)
            '    iStartIndex = sFolderName.LastIndexOf("\")
            '    sFolderName = sFolderName.Substring(iStartIndex + 1)
            'Catch ex As Exception
            '    sFolderName = ""
            'End Try

            Dim [end] As DateTime
            Dim start As DateTime = DateTime.Now

            'oD._FileID = InsertFileName(_FileToParse)
            'oD._FileID = InsertFileName(_FileToParse, sFolderName)
            Dim rr As Integer = 0


            oD.edi_type = "278"

            oD.HL20Flag = 0

            start_time = Now
            oD.TimeStamp = FormatDateTime(start_time, DateFormat.ShortDate)



            oD.IEAFlag = False


            If V Then
                log.ExceptionDetails("DCSGlobal.EDI.Import278.Import", "Pasring File : " + FileToParse)
                log.ExceptionDetails("DCSGlobal.EDI.Import278.Import", "### Overall Start Time: " + start.ToLongTimeString() + " " + FileToParse)
            End If

            _CLPString = String.Empty




            oD.BATCHUID = Guid.NewGuid()



            '' check to see IF we have a bactch ID  if not get out 
            'If oD._BatchID = 0 Then
            '    If (_Debug = 1) Then


            '        log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import837.Import", "BatchID = 0 err -1")
            '    End If

            '    rr = -1
            '    Return rr
            '    Exit Function
            'End If



            'If Not File.Exists(FileToParse) Then
            '    log.ExceptionDetails("DCSGlobal.EDI.Import278.Import", FileToParse + " Does Not Exist ")
            '    Return 1
            '    Exit Function
            'End If



            If _TablesBuilt = False Then
                BuildTables()
                _TablesBuilt = True
            Else

                ISA.Clear()
                GS.Clear()
                ST.Clear()
                BHT.Clear()
                REF.Clear()
                NM1.Clear()
                REF.Clear()
                N3.Clear()
                N4.Clear()
                PER.Clear()
                UM.Clear()
                DTP.Clear()
                HI.Clear()
                SV1.Clear()
                SV2.Clear()
                HL.Clear()
                TRN.Clear()
                DMG.Clear()
                MSG.Clear()
                CL1.Clear()
                HSD.Clear()
                PRV.Clear()
                HCR.Clear()
                UNK.Clear()
                CR1.Clear()
                CR5.Clear()
                CR6.Clear()
                SV3.Clear()
                TOO.Clear()
                PRV.Clear()
                PWK.Clear()
                CRC.Clear()
                AAA.Clear()
                INS.Clear()




            End If

            Dim last As String = String.Empty
            ' Dim line As String = String.Empty
            Dim rowcount As Int32 = 0

            'so lets get down to it opn the file in a stream reader and move thru it one line at a time...........
            For Each line As String In _EDIList

                oD.RowProcessedFlag = 0
                ' line = r.ReadLine()
                'Console.WriteLine(line)

                If oD.DataElementSeparatorFlag = 0 Then
                    oD.DataElementSeparator = Mid(line, 4, 1)
                    oD.DataElementSeparatorFlag = 1
                End If

                'Console.WriteLine(oD.DataElementSeparator)

                '  Try

                ' Add this line to list.
                last = line
                ' Display to console.
                ' 
                ' Read in the next line.

                line = line.Replace("~", "")

                rowcount = rowcount + 1
                oD.ediRowRecordType = objss.ParseDemlimtedStringEDI(line, oD.DataElementSeparator, 1)
                'Console.WriteLine(oD.ediRowRecordType)
                oD.CurrentRowData = line

                oD.RowProcessedFlag = 0

                '    line = r.ReadLine


                'check for LX

                'If objss.ParseDemlimtedStringEDI(line, oD.DataElementSeparator, 1) = "LX" Then
                '    LXFlag = 1
                'End If


                If oD.ediRowRecordType = "ISA" Then

                    oD.RowProcessedFlag = 1

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

                    ISA.Rows.Add(ISARow)

                    oD.ISA_ROW_ID = rowcount

                    oD.CarrotDataDelimiter = oD.ISA11
                    oD.ComponentElementSeperator = oD.ISA16


                    _chars = "RowDataDelimiter: " + oD.DataElementSeparator + " CarrotDataDelimiter: " + oD.CarrotDataDelimiter + " ComponentElementSeperator: " + oD.ComponentElementSeperator

                    oD.ISAFlag = 1
                End If


                If oD.ediRowRecordType = "GS" Then
                    oD.RowProcessedFlag = 1
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
                    GSRow("GS01") = oD.GS01
                    GSRow("GS02") = oD.GS02
                    GSRow("GS03") = oD.GS03
                    GSRow("GS04") = oD.GS04
                    GSRow("GS05") = oD.GS05
                    GSRow("GS06") = oD.GS06
                    GSRow("GS07") = oD.GS07
                    GSRow("GS08") = oD.GS08

                    GS.Rows.Add(GSRow)

                    oD.GS_ROW_ID = rowcount

                    'ComitISAGS()

                End If

                'end GS



                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ' ok so once we find and ST we start a a new string to hold evrey thing to the se and that will get passed to a
                ' thread to process along with all every thing it needs to tag it all.
                '
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


                ' begin st
                If oD.ediRowRecordType = "ST" Then
                    oD.RowProcessedFlag = 1

                    oD.ST_GUID = Guid.NewGuid
                    oD.P_GUID = oD.ST_GUID

                    oD.ST01 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)
                    oD.ST02 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                    oD.ST03 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)

                    Dim STRow As DataRow = ST.NewRow
                    STRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    STRow("HIPAA_GS_GUID") = oD.GS_GUID
                    STRow("HIPAA_ST_GUID") = oD.ST_GUID
                    STRow("ST01") = oD.ST01
                    STRow("ST02") = oD.ST02
                    STRow("ST03") = oD.ST03
                    ST.Rows.Add(STRow)

                    oD.ST_ROW_ID = rowcount




                End If

                ' all the rows get made in to a string. 


                If oD.ediRowRecordType = "BHT" Then

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
                    BHTRow("HIPAA_BHT_GUID") = oD.BHT_GUID
                    BHTRow("BHT01") = oD.BHT01
                    BHTRow("BHT02") = oD.BHT02
                    BHTRow("BHT03") = oD.BHT03
                    BHTRow("BHT04") = oD.BHT04
                    BHTRow("BHT05") = oD.BHT05
                    BHTRow("BHT06") = oD.BHT06
                    BHT.Rows.Add(BHTRow)


                    oD.RowProcessedFlag = 1

                    _ENFlag = True


                End If


                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '      Begin 837
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                If oD.ediRowRecordType = "HL" Then


                    If oD.HL20Flag = 0 Then

                        oD.HL20Flag = 1

                    End If


                    oD.HL_GUID = Guid.NewGuid

                    oD.RowProcessedFlag = 1

                    Dim HLRow As DataRow = HL.NewRow
                    HLRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    HLRow("HIPAA_GS_GUID") = oD.GS_GUID
                    HLRow("HIPAA_ST_GUID") = oD.ST_GUID
                    HLRow("HIPAA_BHT_GUID") = oD.BHT_GUID
                    HLRow("HIPAA_HL_GUID") = oD.HL_GUID


                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then HLRow("HL01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else HLRow("HL01") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then HLRow("HL02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else HLRow("HL02") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then HLRow("HL03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else HLRow("HL03") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then HLRow("HL04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else HLRow("HL04") = DBNull.Value

                    HL.Rows.Add(HLRow)

                End If



                If oD.ediRowRecordType = "NM1" Then

                    oD.RowProcessedFlag = 1
                    oD.HIPAA_NM1_GUID = Guid.NewGuid

                    Dim NM1Row As DataRow = NM1.NewRow
                    NM1Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                    NM1Row("HIPAA_GS_GUID") = oD.GS_GUID
                    NM1Row("HIPAA_ST_GUID") = oD.ST_GUID
                    NM1Row("HIPAA_BHT_GUID") = oD.BHT_GUID
                    NM1Row("HIPAA_HL_GUID") = oD.HL_GUID
                    NM1Row("HIPAA_NM1_GUID") = oD.HIPAA_NM1_GUID
                    NM1Row("HIPAA_HCR_GUID") = oD.HIPAA_HCR_GUID
                    NM1Row("HIPAA_UM_GUID") = oD.HIPAA_UM_GUID



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

                    NM1.Rows.Add(NM1Row)


                End If




                If oD.ediRowRecordType = "UM" Then

                    UM04 = String.Empty

                    oD.RowProcessedFlag = 1
                    oD.HIPAA_UM_GUID = Guid.NewGuid
                    Dim UMRow As DataRow = UM.NewRow
                    UMRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    UMRow("HIPAA_GS_GUID") = oD.GS_GUID
                    UMRow("HIPAA_ST_GUID") = oD.ST_GUID
                    UMRow("HIPAA_BHT_GUID") = oD.BHT_GUID
                    UMRow("HIPAA_HL_GUID") = oD.HL_GUID
                    UMRow("HIPAA_NM1_GUID") = oD.HIPAA_NM1_GUID
                    'UMRow("HIPAA_HCR_GUID") = oD.HIPAA_HCR_GUID
                    UMRow("HIPAA_UM_GUID") = oD.HIPAA_UM_GUID

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then UMRow("UM01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else UMRow("UM01") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then UMRow("UM02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else UMRow("UM02") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then UMRow("UM03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else UMRow("UM03") = DBNull.Value
                    'If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then UMRow("UM04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else UMRow("UM04") = DBNull.Value


                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then
                        UMRow("UM04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5)
                        UM04 = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5)
                    Else
                        UMRow("UM04") = DBNull.Value

                    End If

                    If Not UM04 = String.Empty Then

                        If (objss.ParseDemlimtedString(UM04, oD.ComponentElementSeperator, 1) <> "") Then UMRow("UM04_1") = objss.ParseDemlimtedString(UM04, oD.ComponentElementSeperator, 1) Else UMRow("UM04_1") = DBNull.Value
                        If (objss.ParseDemlimtedString(UM04, oD.ComponentElementSeperator, 2) <> "") Then UMRow("UM04_2") = objss.ParseDemlimtedString(UM04, oD.ComponentElementSeperator, 2) Else UMRow("UM04_2") = DBNull.Value

                    End If

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then
                        UMRow("UM05") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6)
                        UM05 = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6)
                    Else
                        UMRow("UM05") = DBNull.Value

                    End If

                    If Not UM05 = String.Empty Then

                        If (objss.ParseDemlimtedString(UM05, oD.ComponentElementSeperator, 1) <> "") Then UMRow("UM05_1") = objss.ParseDemlimtedString(UM05, oD.ComponentElementSeperator, 1) Else UMRow("UM05_1") = DBNull.Value
                        If (objss.ParseDemlimtedString(UM05, oD.ComponentElementSeperator, 2) <> "") Then UMRow("UM05_2") = objss.ParseDemlimtedString(UM05, oD.ComponentElementSeperator, 2) Else UMRow("UM05_2") = DBNull.Value
                        If (objss.ParseDemlimtedString(UM05, oD.ComponentElementSeperator, 3) <> "") Then UMRow("UM05_3") = objss.ParseDemlimtedString(UM05, oD.ComponentElementSeperator, 1) Else UMRow("UM05_3") = DBNull.Value
                        If (objss.ParseDemlimtedString(UM05, oD.ComponentElementSeperator, 4) <> "") Then UMRow("UM05_4") = objss.ParseDemlimtedString(UM05, oD.ComponentElementSeperator, 2) Else UMRow("UM05_4") = DBNull.Value

                    End If

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then UMRow("UM06") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else UMRow("UM06") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then UMRow("UM07") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else UMRow("UM07") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then UMRow("UM08") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else UMRow("UM08") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then UMRow("UM09") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else UMRow("UM09") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) <> "") Then UMRow("UM10") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) Else UMRow("UM10") = DBNull.Value

                    UM.Rows.Add(UMRow)
                End If


                If oD.ediRowRecordType = "HCR" Then

                    oD.RowProcessedFlag = 1

                    oD.HIPAA_HCR_GUID = Guid.NewGuid

                    Dim HCRRow As DataRow = HCR.NewRow
                    HCRRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    HCRRow("HIPAA_GS_GUID") = oD.GS_GUID
                    HCRRow("HIPAA_ST_GUID") = oD.ST_GUID
                    HCRRow("HIPAA_BHT_GUID") = oD.BHT_GUID
                    HCRRow("HIPAA_HL_GUID") = oD.HL_GUID
                    HCRRow("HIPAA_NM1_GUID") = oD.HIPAA_NM1_GUID
                    HCRRow("HIPAA_HCR_GUID") = oD.HIPAA_HCR_GUID
                    HCRRow("HIPAA_UM_GUID") = oD.HIPAA_UM_GUID

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then HCRRow("HCR01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else HCRRow("HCR01") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then HCRRow("HCR02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else HCRRow("HCR02") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then HCRRow("HCR03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else HCRRow("HCR03") = DBNull.Value


                    HCR.Rows.Add(HCRRow)

                End If



                If oD.ediRowRecordType = "N3" Then

                    oD.RowProcessedFlag = 1

                    Dim N3Row As DataRow = N3.NewRow
                    N3Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                    N3Row("HIPAA_GS_GUID") = oD.GS_GUID
                    N3Row("HIPAA_ST_GUID") = oD.ST_GUID
                    N3Row("HIPAA_BHT_GUID") = oD.BHT_GUID
                    N3Row("HIPAA_HL_GUID") = oD.HL_GUID
                    N3Row("HIPAA_NM1_GUID") = oD.HIPAA_NM1_GUID
                    N3Row("HIPAA_HCR_GUID") = oD.HIPAA_HCR_GUID
                    N3Row("HIPAA_UM_GUID") = oD.HIPAA_UM_GUID

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then N3Row("N301") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else N3Row("N301") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then N3Row("N302") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else N3Row("N302") = DBNull.Value


                    N3.Rows.Add(N3Row)



                End If

                If oD.ediRowRecordType = "N4" Then

                    oD.RowProcessedFlag = 1

                    Dim N4Row As DataRow = N4.NewRow
                    N4Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                    N4Row("HIPAA_GS_GUID") = oD.GS_GUID
                    N4Row("HIPAA_ST_GUID") = oD.ST_GUID
                    N4Row("HIPAA_BHT_GUID") = oD.BHT_GUID
                    N4Row("HIPAA_HL_GUID") = oD.HL_GUID
                    N4Row("HIPAA_NM1_GUID") = oD.HIPAA_NM1_GUID
                    N4Row("HIPAA_HCR_GUID") = oD.HIPAA_HCR_GUID
                    N4Row("HIPAA_UM_GUID") = oD.HIPAA_UM_GUID

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then N4Row("N401") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else N4Row("N401") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then N4Row("N402") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else N4Row("N402") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then N4Row("N403") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else N4Row("N403") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then N4Row("N404") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else N4Row("N404") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then N4Row("N405") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else N4Row("N405") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then N4Row("N406") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else N4Row("N406") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then N4Row("N407") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else N4Row("N407") = DBNull.Value


                    N4.Rows.Add(N4Row)


                End If


                If oD.ediRowRecordType = "REF" Then

                    oD.RowProcessedFlag = 1

                    Dim REFRow As DataRow = REF.NewRow
                    REFRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    REFRow("HIPAA_GS_GUID") = oD.GS_GUID
                    REFRow("HIPAA_ST_GUID") = oD.ST_GUID
                    REFRow("HIPAA_BHT_GUID") = oD.BHT_GUID
                    REFRow("HIPAA_HL_GUID") = oD.HL_GUID
                    REFRow("HIPAA_NM1_GUID") = oD.HIPAA_NM1_GUID
                    REFRow("HIPAA_HCR_GUID") = oD.HIPAA_HCR_GUID
                    REFRow("HIPAA_UM_GUID") = oD.HIPAA_UM_GUID



                    ' REFRow("P_GUID") = oD.P_GUID

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then REFRow("REF01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else REFRow("REF01") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then REFRow("REF02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else REFRow("REF02") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then REFRow("REF03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else REFRow("REF03") = DBNull.Value


                    REF.Rows.Add(REFRow)


                End If


                If oD.ediRowRecordType = "PER" Then

                    oD.RowProcessedFlag = 1

                    Dim PERRow As DataRow = PER.NewRow
                    PERRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    PERRow("HIPAA_GS_GUID") = oD.GS_GUID
                    PERRow("HIPAA_ST_GUID") = oD.ST_GUID
                    PERRow("HIPAA_BHT_GUID") = oD.BHT_GUID
                    PERRow("HIPAA_HL_GUID") = oD.HL_GUID
                    PERRow("HIPAA_NM1_GUID") = oD.HIPAA_NM1_GUID
                    PERRow("HIPAA_HCR_GUID") = oD.HIPAA_HCR_GUID
                    PERRow("HIPAA_UM_GUID") = oD.HIPAA_UM_GUID




                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then PERRow("PER01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else PERRow("PER01") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then PERRow("PER02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else PERRow("PER02") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then PERRow("PER03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else PERRow("PER03") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then PERRow("PER04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else PERRow("PER04") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then PERRow("PER05") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else PERRow("PER05") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then PERRow("PER06") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else PERRow("PER06") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then PERRow("PER07") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else PERRow("PER07") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then PERRow("PER08") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else PERRow("PER08") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then PERRow("PER09") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else PERRow("PER09") = DBNull.Value


                    PER.Rows.Add(PERRow)

                End If


                If oD.ediRowRecordType = "PRV" Then

                    oD.RowProcessedFlag = 1

                    Dim PRVRow As DataRow = PRV.NewRow
                    PRVRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    PRVRow("HIPAA_GS_GUID") = oD.GS_GUID
                    PRVRow("HIPAA_ST_GUID") = oD.ST_GUID
                    PRVRow("HIPAA_BHT_GUID") = oD.BHT_GUID
                    PRVRow("HIPAA_HL_GUID") = oD.HL_GUID
                    PRVRow("HIPAA_NM1_GUID") = oD.HIPAA_NM1_GUID
                    PRVRow("HIPAA_HCR_GUID") = oD.HIPAA_HCR_GUID
                    PRVRow("HIPAA_UM_GUID") = oD.HIPAA_UM_GUID


                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then PRVRow("PRV01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else PRVRow("PRV01") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then PRVRow("PRV02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else PRVRow("PRV02") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then PRVRow("PRV03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else PRVRow("PRV03") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then PRVRow("PRV04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else PRVRow("PRV04") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then PRVRow("PRV05") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else PRVRow("PRV05") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then PRVRow("PRV06") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else PRVRow("PRV06") = DBNull.Value

                    PRV.Rows.Add(PRVRow)

                End If










                If oD.ediRowRecordType = "DTP" Then

                    oD.RowProcessedFlag = 1

                    Dim DTPRow As DataRow = DTP.NewRow
                    DTPRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    DTPRow("HIPAA_GS_GUID") = oD.GS_GUID
                    DTPRow("HIPAA_ST_GUID") = oD.ST_GUID
                    DTPRow("HIPAA_BHT_GUID") = oD.BHT_GUID
                    DTPRow("HIPAA_HL_GUID") = oD.HL_GUID
                    DTPRow("HIPAA_NM1_GUID") = oD.HIPAA_NM1_GUID
                    DTPRow("HIPAA_HCR_GUID") = oD.HIPAA_HCR_GUID
                    DTPRow("HIPAA_UM_GUID") = oD.HIPAA_UM_GUID

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then DTPRow("DTP01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else DTPRow("DTP01") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then DTPRow("DTP02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else DTPRow("DTP02") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then DTPRow("DTP03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else DTPRow("DTP03") = DBNull.Value
                    '    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then DTPRow("DTP04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else DTPRow("DTP04") = DBNull.Value

                    DTP.Rows.Add(DTPRow)

                End If




                If oD.ediRowRecordType = "HI" Then

                    HI01 = String.Empty
                    HI02 = String.Empty
                    HI03 = String.Empty
                    HI04 = String.Empty
                    HI05 = String.Empty
                    HI06 = String.Empty
                    HI07 = String.Empty
                    HI08 = String.Empty
                    HI09 = String.Empty
                    HI10 = String.Empty
                    HI11 = String.Empty
                    HI12 = String.Empty

                    oD.RowProcessedFlag = 1

                    Dim HIRow As DataRow = HI.NewRow
                    HIRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    HIRow("HIPAA_GS_GUID") = oD.GS_GUID
                    HIRow("HIPAA_ST_GUID") = oD.ST_GUID
                    HIRow("HIPAA_BHT_GUID") = oD.BHT_GUID
                    HIRow("HIPAA_HL_GUID") = oD.HL_GUID
                    HIRow("HIPAA_NM1_GUID") = oD.HIPAA_NM1_GUID
                    HIRow("HIPAA_HCR_GUID") = oD.HIPAA_HCR_GUID
                    HIRow("HIPAA_UM_GUID") = oD.HIPAA_UM_GUID

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then
                        HIRow("HI01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2)
                        HI01 = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2)
                    Else
                        HIRow("HI01") = DBNull.Value

                    End If


                    If Not HI01 = String.Empty Then

                        If (objss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 1) <> "") Then HIRow("HI01_1") = objss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 1) Else HIRow("HI01_1") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 2) <> "") Then HIRow("HI01_2") = objss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 2) Else HIRow("HI01_2") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 3) <> "") Then HIRow("HI01_3") = objss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 3) Else HIRow("HI01_3") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 4) <> "") Then HIRow("HI01_4") = objss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 4) Else HIRow("HI01_4") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 5) <> "") Then HIRow("HI01_5") = objss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 5) Else HIRow("HI01_5") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 6) <> "") Then HIRow("HI01_6") = objss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 6) Else HIRow("HI01_6") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 7) <> "") Then HIRow("HI01_7") = objss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 7) Else HIRow("HI01_7") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 8) <> "") Then HIRow("HI01_8") = objss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 8) Else HIRow("HI01_8") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 9) <> "") Then HIRow("HI01_9") = objss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 9) Else HIRow("HI01_9") = DBNull.Value

                    End If


                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then
                        HIRow("HI02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3)
                        HI02 = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3)
                    Else
                        HIRow("HI02") = DBNull.Value

                    End If


                    If Not HI02 = String.Empty Then

                        If (objss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 1) <> "") Then HIRow("HI02_1") = objss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 1) Else HIRow("HI02_1") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 2) <> "") Then HIRow("HI02_2") = objss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 2) Else HIRow("HI02_2") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 3) <> "") Then HIRow("HI02_3") = objss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 3) Else HIRow("HI02_3") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 4) <> "") Then HIRow("HI02_4") = objss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 4) Else HIRow("HI02_4") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 5) <> "") Then HIRow("HI02_5") = objss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 5) Else HIRow("HI02_5") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 6) <> "") Then HIRow("HI02_6") = objss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 6) Else HIRow("HI02_6") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 7) <> "") Then HIRow("HI02_7") = objss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 7) Else HIRow("HI02_7") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 8) <> "") Then HIRow("HI02_8") = objss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 8) Else HIRow("HI02_8") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 9) <> "") Then HIRow("HI02_9") = objss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 9) Else HIRow("HI02_9") = DBNull.Value

                    End If

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then
                        HIRow("HI03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4)
                        HI03 = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4)
                    Else
                        HIRow("HI03") = DBNull.Value

                    End If


                    If Not HI03 = String.Empty Then

                        If (objss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 1) <> "") Then HIRow("HI03_1") = objss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 1) Else HIRow("HI03_1") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 2) <> "") Then HIRow("HI03_2") = objss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 2) Else HIRow("HI03_2") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 3) <> "") Then HIRow("HI03_3") = objss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 3) Else HIRow("HI03_3") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 4) <> "") Then HIRow("HI03_4") = objss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 4) Else HIRow("HI03_4") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 5) <> "") Then HIRow("HI03_5") = objss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 5) Else HIRow("HI03_5") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 6) <> "") Then HIRow("HI03_6") = objss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 6) Else HIRow("HI03_6") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 7) <> "") Then HIRow("HI03_7") = objss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 7) Else HIRow("HI03_7") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 8) <> "") Then HIRow("HI03_8") = objss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 8) Else HIRow("HI03_8") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 9) <> "") Then HIRow("HI03_9") = objss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 9) Else HIRow("HI03_9") = DBNull.Value

                    End If


                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then
                        HIRow("HI04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5)
                        HI04 = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5)
                    Else
                        HIRow("HI04") = DBNull.Value

                    End If


                    If Not HI04 = String.Empty Then

                        If (objss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 1) <> "") Then HIRow("HI04_1") = objss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 1) Else HIRow("HI04_1") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 2) <> "") Then HIRow("HI04_2") = objss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 2) Else HIRow("HI04_2") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 3) <> "") Then HIRow("HI04_3") = objss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 3) Else HIRow("HI04_3") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 4) <> "") Then HIRow("HI04_4") = objss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 4) Else HIRow("HI04_4") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 5) <> "") Then HIRow("HI04_5") = objss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 5) Else HIRow("HI04_5") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 6) <> "") Then HIRow("HI04_6") = objss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 6) Else HIRow("HI04_6") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 7) <> "") Then HIRow("HI04_7") = objss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 7) Else HIRow("HI04_7") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 8) <> "") Then HIRow("HI04_8") = objss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 8) Else HIRow("HI04_8") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 9) <> "") Then HIRow("HI04_9") = objss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 9) Else HIRow("HI04_9") = DBNull.Value

                    End If

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then
                        HIRow("HI05") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6)
                        HI05 = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6)
                    Else
                        HIRow("HI05") = DBNull.Value

                    End If


                    If Not HI05 = String.Empty Then

                        If (objss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 1) <> "") Then HIRow("HI05_1") = objss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 1) Else HIRow("HI05_1") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 2) <> "") Then HIRow("HI05_2") = objss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 2) Else HIRow("HI05_2") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 3) <> "") Then HIRow("HI05_3") = objss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 3) Else HIRow("HI05_3") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 4) <> "") Then HIRow("HI05_4") = objss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 4) Else HIRow("HI05_4") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 5) <> "") Then HIRow("HI05_5") = objss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 5) Else HIRow("HI05_5") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 6) <> "") Then HIRow("HI05_6") = objss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 6) Else HIRow("HI05_6") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 7) <> "") Then HIRow("HI05_7") = objss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 7) Else HIRow("HI05_7") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 8) <> "") Then HIRow("HI05_8") = objss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 8) Else HIRow("HI05_8") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 9) <> "") Then HIRow("HI05_9") = objss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 9) Else HIRow("HI05_9") = DBNull.Value

                    End If



                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then
                        HIRow("HI06") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7)
                        HI06 = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7)
                    Else
                        HIRow("HI06") = DBNull.Value

                    End If


                    If Not HI06 = String.Empty Then

                        If (objss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 1) <> "") Then HIRow("HI06_1") = objss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 1) Else HIRow("HI06_1") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 2) <> "") Then HIRow("HI06_2") = objss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 2) Else HIRow("HI06_2") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 3) <> "") Then HIRow("HI06_3") = objss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 3) Else HIRow("HI06_3") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 4) <> "") Then HIRow("HI06_4") = objss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 4) Else HIRow("HI06_4") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 5) <> "") Then HIRow("HI06_5") = objss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 5) Else HIRow("HI06_5") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 6) <> "") Then HIRow("HI06_6") = objss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 6) Else HIRow("HI06_6") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 7) <> "") Then HIRow("HI06_7") = objss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 7) Else HIRow("HI06_7") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 8) <> "") Then HIRow("HI06_8") = objss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 8) Else HIRow("HI06_8") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 9) <> "") Then HIRow("HI06_9") = objss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 9) Else HIRow("HI06_9") = DBNull.Value

                    End If


                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then
                        HIRow("HI07") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8)
                        HI07 = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8)
                    Else
                        HIRow("HI07") = DBNull.Value

                    End If


                    If Not HI07 = String.Empty Then

                        If (objss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 1) <> "") Then HIRow("HI07_1") = objss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 1) Else HIRow("HI07_1") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 2) <> "") Then HIRow("HI07_2") = objss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 2) Else HIRow("HI07_2") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 3) <> "") Then HIRow("HI07_3") = objss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 3) Else HIRow("HI07_3") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 4) <> "") Then HIRow("HI07_4") = objss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 4) Else HIRow("HI07_4") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 5) <> "") Then HIRow("HI07_5") = objss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 5) Else HIRow("HI07_5") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 6) <> "") Then HIRow("HI07_6") = objss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 6) Else HIRow("HI07_6") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 7) <> "") Then HIRow("HI07_7") = objss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 7) Else HIRow("HI07_7") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 8) <> "") Then HIRow("HI07_8") = objss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 8) Else HIRow("HI07_8") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 9) <> "") Then HIRow("HI07_9") = objss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 9) Else HIRow("HI07_9") = DBNull.Value

                    End If

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then
                        HIRow("HI08") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9)
                        HI08 = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9)
                    Else
                        HIRow("HI08") = DBNull.Value

                    End If


                    If Not HI08 = String.Empty Then

                        If (objss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 1) <> "") Then HIRow("HI08_1") = objss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 1) Else HIRow("HI08_1") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 2) <> "") Then HIRow("HI08_2") = objss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 2) Else HIRow("HI08_2") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 3) <> "") Then HIRow("HI08_3") = objss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 3) Else HIRow("HI08_3") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 4) <> "") Then HIRow("HI08_4") = objss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 4) Else HIRow("HI08_4") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 5) <> "") Then HIRow("HI08_5") = objss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 5) Else HIRow("HI08_5") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 6) <> "") Then HIRow("HI08_6") = objss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 6) Else HIRow("HI08_6") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 7) <> "") Then HIRow("HI08_7") = objss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 7) Else HIRow("HI08_7") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 8) <> "") Then HIRow("HI08_8") = objss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 8) Else HIRow("HI08_8") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 9) <> "") Then HIRow("HI08_9") = objss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 9) Else HIRow("HI08_9") = DBNull.Value

                    End If

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then
                        HIRow("HI09") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10)
                        HI09 = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10)
                    Else
                        HIRow("HI09") = DBNull.Value

                    End If


                    If Not HI09 = String.Empty Then

                        If (objss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 1) <> "") Then HIRow("HI09_1") = objss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 1) Else HIRow("HI09_1") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 2) <> "") Then HIRow("HI09_2") = objss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 2) Else HIRow("HI09_2") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 3) <> "") Then HIRow("HI09_3") = objss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 3) Else HIRow("HI09_3") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 4) <> "") Then HIRow("HI09_4") = objss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 4) Else HIRow("HI09_4") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 5) <> "") Then HIRow("HI09_5") = objss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 5) Else HIRow("HI09_5") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 6) <> "") Then HIRow("HI09_6") = objss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 6) Else HIRow("HI09_6") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 7) <> "") Then HIRow("HI09_7") = objss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 7) Else HIRow("HI09_7") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 8) <> "") Then HIRow("HI09_8") = objss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 8) Else HIRow("HI09_8") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 9) <> "") Then HIRow("HI09_9") = objss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 9) Else HIRow("HI09_9") = DBNull.Value

                    End If


                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) <> "") Then
                        HIRow("HI10") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11)
                        HI10 = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11)
                    Else
                        HIRow("HI10") = DBNull.Value

                    End If


                    If Not HI10 = String.Empty Then

                        If (objss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 1) <> "") Then HIRow("HI10_1") = objss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 1) Else HIRow("HI10_1") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 2) <> "") Then HIRow("HI10_2") = objss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 2) Else HIRow("HI10_2") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 3) <> "") Then HIRow("HI10_3") = objss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 3) Else HIRow("HI10_3") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 4) <> "") Then HIRow("HI10_4") = objss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 4) Else HIRow("HI10_4") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 5) <> "") Then HIRow("HI10_5") = objss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 5) Else HIRow("HI10_5") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 6) <> "") Then HIRow("HI10_6") = objss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 6) Else HIRow("HI10_6") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 7) <> "") Then HIRow("HI10_7") = objss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 7) Else HIRow("HI10_7") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 8) <> "") Then HIRow("HI10_8") = objss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 8) Else HIRow("HI10_8") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 9) <> "") Then HIRow("HI10_9") = objss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 9) Else HIRow("HI10_9") = DBNull.Value

                    End If

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) <> "") Then
                        HIRow("HI11") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12)
                        HI11 = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12)
                    Else
                        HIRow("HI11") = DBNull.Value

                    End If


                    If Not HI11 = String.Empty Then

                        If (objss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 1) <> "") Then HIRow("HI11_1") = objss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 1) Else HIRow("HI11_1") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 2) <> "") Then HIRow("HI11_2") = objss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 2) Else HIRow("HI11_2") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 3) <> "") Then HIRow("HI11_3") = objss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 3) Else HIRow("HI11_3") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 4) <> "") Then HIRow("HI11_4") = objss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 4) Else HIRow("HI11_4") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 5) <> "") Then HIRow("HI11_5") = objss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 5) Else HIRow("HI11_5") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 6) <> "") Then HIRow("HI11_6") = objss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 6) Else HIRow("HI11_6") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 7) <> "") Then HIRow("HI11_7") = objss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 7) Else HIRow("HI11_7") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 8) <> "") Then HIRow("HI11_8") = objss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 8) Else HIRow("HI11_8") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 9) <> "") Then HIRow("HI11_9") = objss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 9) Else HIRow("HI11_9") = DBNull.Value

                    End If


                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) <> "") Then
                        HIRow("HI12") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13)
                        HI12 = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13)
                    Else
                        HIRow("HI12") = DBNull.Value

                    End If


                    If Not HI12 = String.Empty Then

                        If (objss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 1) <> "") Then HIRow("HI12_1") = objss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 1) Else HIRow("HI12_1") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 2) <> "") Then HIRow("HI12_2") = objss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 2) Else HIRow("HI12_2") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 3) <> "") Then HIRow("HI12_3") = objss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 3) Else HIRow("HI12_3") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 4) <> "") Then HIRow("HI12_4") = objss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 4) Else HIRow("HI12_4") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 5) <> "") Then HIRow("HI12_5") = objss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 5) Else HIRow("HI12_5") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 6) <> "") Then HIRow("HI12_6") = objss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 6) Else HIRow("HI12_6") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 7) <> "") Then HIRow("HI12_7") = objss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 7) Else HIRow("HI12_7") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 8) <> "") Then HIRow("HI12_8") = objss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 8) Else HIRow("HI12_8") = DBNull.Value
                        If (objss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 9) <> "") Then HIRow("HI12_9") = objss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 9) Else HIRow("HI12_9") = DBNull.Value

                    End If

                    HI.Rows.Add(HIRow)

                End If


                If oD.ediRowRecordType = "SV1" Then

                    SV101 = String.Empty

                    SV107 = String.Empty

                    oD.RowProcessedFlag = 1

                    Dim SV1Row As DataRow = SV1.NewRow
                    SV1Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                    SV1Row("HIPAA_GS_GUID") = oD.GS_GUID
                    SV1Row("HIPAA_ST_GUID") = oD.ST_GUID
                    SV1Row("HIPAA_BHT_GUID") = oD.BHT_GUID
                    SV1Row("HIPAA_HL_GUID") = oD.HL_GUID
                    SV1Row("HIPAA_NM1_GUID") = oD.HIPAA_NM1_GUID
                    SV1Row("HIPAA_HCR_GUID") = oD.HIPAA_HCR_GUID
                    SV1Row("HIPAA_UM_GUID") = oD.HIPAA_UM_GUID

                    ' //  If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then SV1Row("SV101") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else SV1Row("SV101") = DBNull.Value


                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then
                        SV1Row("SV101") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2)
                        SV101 = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2)
                    Else
                        SV1Row("SV101") = DBNull.Value

                    End If

                    If Not SV101 = String.Empty Then

                        If (objss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 1) <> "") Then SV1Row("SV101_1") = objss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 1) Else SV1Row("SV101_1") = DBNull.Value
                        If (objss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 2) <> "") Then SV1Row("SV101_2") = objss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 2) Else SV1Row("SV101_2") = DBNull.Value
                        If (objss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 3) <> "") Then SV1Row("SV101_3") = objss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 3) Else SV1Row("SV101_3") = DBNull.Value
                        If (objss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 4) <> "") Then SV1Row("SV101_4") = objss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 4) Else SV1Row("SV101_4") = DBNull.Value
                        If (objss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 5) <> "") Then SV1Row("SV101_5") = objss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 5) Else SV1Row("SV101_5") = DBNull.Value
                        If (objss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 6) <> "") Then SV1Row("SV101_6") = objss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 6) Else SV1Row("SV101_6") = DBNull.Value
                        If (objss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 7) <> "") Then SV1Row("SV101_7") = objss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 7) Else SV1Row("SV101_7") = DBNull.Value
                    End If

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then SV1Row("SV103") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else SV1Row("SV103") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then SV1Row("SV104") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else SV1Row("SV104") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then SV1Row("SV105") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else SV1Row("SV105") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then SV1Row("SV106") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else SV1Row("SV106") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then SV1Row("SV107") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else SV1Row("SV107") = DBNull.Value


                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then
                        SV1Row("SV107") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8)
                        SV101 = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8)
                    Else
                        SV1Row("SV107") = DBNull.Value

                    End If

                    If Not SV101 = String.Empty Then

                        If (objss.ParseDemlimtedString(SV107, oD.ComponentElementSeperator, 1) <> "") Then SV1Row("SV107_1") = objss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 1) Else SV1Row("SV107_1") = DBNull.Value
                        If (objss.ParseDemlimtedString(SV107, oD.ComponentElementSeperator, 2) <> "") Then SV1Row("SV107_2") = objss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 2) Else SV1Row("SV107_2") = DBNull.Value
                        If (objss.ParseDemlimtedString(SV107, oD.ComponentElementSeperator, 3) <> "") Then SV1Row("SV107_3") = objss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 3) Else SV1Row("SV107_3") = DBNull.Value
                        If (objss.ParseDemlimtedString(SV107, oD.ComponentElementSeperator, 4) <> "") Then SV1Row("SV107_4") = objss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 4) Else SV1Row("SV107_4") = DBNull.Value
                        '  If (objss.ParseDemlimtedString(SV107, oD.ComponentElementSeperator, 5) <> "") Then SV1Row("SV107_5") = objss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 5) Else SV1Row("SV107_5") = DBNull.Value
                        '  If (objss.ParseDemlimtedString(SV107, oD.ComponentElementSeperator, 6) <> "") Then SV1Row("SV107_6") = objss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 6) Else SV1Row("SV107_6") = DBNull.Value
                        '  If (objss.ParseDemlimtedString(SV107, oD.ComponentElementSeperator, 7) <> "") Then SV1Row("SV107_7") = objss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 7) Else SV1Row("SV107_7") = DBNull.Value
                    End If

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then SV1Row("SV108") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else SV1Row("SV108") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then SV1Row("SV109") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else SV1Row("SV109") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) <> "") Then SV1Row("SV110") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) Else SV1Row("SV110") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) <> "") Then SV1Row("SV111") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) Else SV1Row("SV111") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) <> "") Then SV1Row("SV112") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) Else SV1Row("SV112") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) <> "") Then SV1Row("SV113") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) Else SV1Row("SV113") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 15) <> "") Then SV1Row("SV114") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 15) Else SV1Row("SV114") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 16) <> "") Then SV1Row("SV115") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 16) Else SV1Row("SV115") = DBNull.Value

                    SV1.Rows.Add(SV1Row)
                End If



                If oD.ediRowRecordType = "SV2" Then

                    SV202 = String.Empty

                    oD.RowProcessedFlag = 1

                    Dim SV2Row As DataRow = SV2.NewRow
                    SV2Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                    SV2Row("HIPAA_GS_GUID") = oD.GS_GUID
                    SV2Row("HIPAA_ST_GUID") = oD.ST_GUID
                    SV2Row("HIPAA_BHT_GUID") = oD.BHT_GUID
                    SV2Row("HIPAA_HL_GUID") = oD.HL_GUID
                    SV2Row("HIPAA_NM1_GUID") = oD.HIPAA_NM1_GUID
                    SV2Row("HIPAA_HCR_GUID") = oD.HIPAA_HCR_GUID
                    SV2Row("HIPAA_UM_GUID") = oD.HIPAA_UM_GUID

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then SV2Row("SV201") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else SV2Row("SV201") = DBNull.Value


                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then
                        SV2Row("SV202") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3)
                        SV202 = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3)
                    Else
                        SV2Row("SV202") = DBNull.Value

                    End If

                    If Not SV202 = String.Empty Then

                        If (objss.ParseDemlimtedString(SV202, oD.ComponentElementSeperator, 1) <> "") Then SV2Row("SV202_1") = objss.ParseDemlimtedString(SV202, oD.ComponentElementSeperator, 1) Else SV2Row("SV202_1") = DBNull.Value
                        If (objss.ParseDemlimtedString(SV202, oD.ComponentElementSeperator, 2) <> "") Then SV2Row("SV202_2") = objss.ParseDemlimtedString(SV202, oD.ComponentElementSeperator, 2) Else SV2Row("SV202_2") = DBNull.Value
                        If (objss.ParseDemlimtedString(SV202, oD.ComponentElementSeperator, 3) <> "") Then SV2Row("SV202_3") = objss.ParseDemlimtedString(SV202, oD.ComponentElementSeperator, 3) Else SV2Row("SV202_3") = DBNull.Value
                        If (objss.ParseDemlimtedString(SV202, oD.ComponentElementSeperator, 4) <> "") Then SV2Row("SV202_4") = objss.ParseDemlimtedString(SV202, oD.ComponentElementSeperator, 4) Else SV2Row("SV202_4") = DBNull.Value
                        If (objss.ParseDemlimtedString(SV202, oD.ComponentElementSeperator, 5) <> "") Then SV2Row("SV202_5") = objss.ParseDemlimtedString(SV202, oD.ComponentElementSeperator, 5) Else SV2Row("SV202_5") = DBNull.Value
                        If (objss.ParseDemlimtedString(SV202, oD.ComponentElementSeperator, 6) <> "") Then SV2Row("SV202_6") = objss.ParseDemlimtedString(SV202, oD.ComponentElementSeperator, 6) Else SV2Row("SV202_6") = DBNull.Value
                        If (objss.ParseDemlimtedString(SV202, oD.ComponentElementSeperator, 7) <> "") Then SV2Row("SV202_7") = objss.ParseDemlimtedString(SV202, oD.ComponentElementSeperator, 7) Else SV2Row("SV202_7") = DBNull.Value
                    End If

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then SV2Row("SV202") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else SV2Row("SV202") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then SV2Row("SV203") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else SV2Row("SV203") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then SV2Row("SV204") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else SV2Row("SV204") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then SV2Row("SV205") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else SV2Row("SV205") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then SV2Row("SV206") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else SV2Row("SV206") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then SV2Row("SV207") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else SV2Row("SV207") = DBNull.Value

                    SV2.Rows.Add(SV2Row)

                End If




                If oD.ediRowRecordType = "SV3" Then

                    SV301 = String.Empty

                    oD.RowProcessedFlag = 1

                    Dim SV3Row As DataRow = SV3.NewRow
                    SV3Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                    SV3Row("HIPAA_GS_GUID") = oD.GS_GUID
                    SV3Row("HIPAA_ST_GUID") = oD.ST_GUID
                    SV3Row("HIPAA_BHT_GUID") = oD.BHT_GUID
                    SV3Row("HIPAA_HL_GUID") = oD.HL_GUID
                    SV3Row("HIPAA_NM1_GUID") = oD.HIPAA_NM1_GUID
                    SV3Row("HIPAA_HCR_GUID") = oD.HIPAA_HCR_GUID
                    SV3Row("HIPAA_UM_GUID") = oD.HIPAA_UM_GUID

                    'If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then SV2Row("SV201") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else SV2Row("SV201") = DBNull.Value


                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then
                        SV3Row("SV301") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2)
                        SV301 = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2)
                    Else
                        SV3Row("SV301") = DBNull.Value

                    End If

                    If Not SV301 = String.Empty Then

                        If (objss.ParseDemlimtedString(SV301, oD.ComponentElementSeperator, 1) <> "") Then SV3Row("SV301_1") = objss.ParseDemlimtedString(SV301, oD.ComponentElementSeperator, 1) Else SV3Row("SV301_1") = DBNull.Value
                        If (objss.ParseDemlimtedString(SV301, oD.ComponentElementSeperator, 2) <> "") Then SV3Row("SV301_2") = objss.ParseDemlimtedString(SV301, oD.ComponentElementSeperator, 2) Else SV3Row("SV301_2") = DBNull.Value
                        If (objss.ParseDemlimtedString(SV301, oD.ComponentElementSeperator, 3) <> "") Then SV3Row("SV301_3") = objss.ParseDemlimtedString(SV301, oD.ComponentElementSeperator, 3) Else SV3Row("SV301_3") = DBNull.Value
                        If (objss.ParseDemlimtedString(SV301, oD.ComponentElementSeperator, 4) <> "") Then SV3Row("SV301_4") = objss.ParseDemlimtedString(SV301, oD.ComponentElementSeperator, 4) Else SV3Row("SV301_4") = DBNull.Value
                        If (objss.ParseDemlimtedString(SV301, oD.ComponentElementSeperator, 5) <> "") Then SV3Row("SV301_5") = objss.ParseDemlimtedString(SV301, oD.ComponentElementSeperator, 5) Else SV3Row("SV301_5") = DBNull.Value
                        If (objss.ParseDemlimtedString(SV301, oD.ComponentElementSeperator, 6) <> "") Then SV3Row("SV301_6") = objss.ParseDemlimtedString(SV301, oD.ComponentElementSeperator, 6) Else SV3Row("SV301_6") = DBNull.Value
                        If (objss.ParseDemlimtedString(SV301, oD.ComponentElementSeperator, 7) <> "") Then SV3Row("SV301_7") = objss.ParseDemlimtedString(SV301, oD.ComponentElementSeperator, 7) Else SV3Row("SV301_7") = DBNull.Value
                        If (objss.ParseDemlimtedString(SV301, oD.ComponentElementSeperator, 8) <> "") Then SV3Row("SV301_8") = objss.ParseDemlimtedString(SV301, oD.ComponentElementSeperator, 8) Else SV3Row("SV301_8") = DBNull.Value
                    End If

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then SV3Row("SV302") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else SV3Row("SV302") = DBNull.Value



                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then
                        SV3Row("SV304") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4)
                        SV304 = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4)
                    Else
                        SV3Row("SV304") = DBNull.Value

                    End If

                    If Not SV304 = String.Empty Then

                        If (objss.ParseDemlimtedString(SV304, oD.ComponentElementSeperator, 1) <> "") Then SV3Row("SV304_1") = objss.ParseDemlimtedString(SV304, oD.ComponentElementSeperator, 1) Else SV3Row("SV304_1") = DBNull.Value
                        If (objss.ParseDemlimtedString(SV304, oD.ComponentElementSeperator, 2) <> "") Then SV3Row("SV304_2") = objss.ParseDemlimtedString(SV304, oD.ComponentElementSeperator, 2) Else SV3Row("SV304_2") = DBNull.Value
                        If (objss.ParseDemlimtedString(SV304, oD.ComponentElementSeperator, 3) <> "") Then SV3Row("SV304_3") = objss.ParseDemlimtedString(SV304, oD.ComponentElementSeperator, 3) Else SV3Row("SV304_3") = DBNull.Value
                        If (objss.ParseDemlimtedString(SV304, oD.ComponentElementSeperator, 4) <> "") Then SV3Row("SV304_4") = objss.ParseDemlimtedString(SV304, oD.ComponentElementSeperator, 4) Else SV3Row("SV304_4") = DBNull.Value
                        If (objss.ParseDemlimtedString(SV304, oD.ComponentElementSeperator, 5) <> "") Then SV3Row("SV304_5") = objss.ParseDemlimtedString(SV304, oD.ComponentElementSeperator, 5) Else SV3Row("SV304_5") = DBNull.Value

                    End If



                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then SV3Row("SV305") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else SV3Row("SV305") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then SV3Row("SV306") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else SV3Row("SV306") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then SV3Row("SV307") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else SV3Row("SV307") = DBNull.Value


                    SV3.Rows.Add(SV3Row)

                End If


                If oD.ediRowRecordType = "TOO" Then

                    TOO03 = String.Empty

                    oD.RowProcessedFlag = 1

                    Dim TOORow As DataRow = TOO.NewRow
                    TOORow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    TOORow("HIPAA_GS_GUID") = oD.GS_GUID
                    TOORow("HIPAA_ST_GUID") = oD.ST_GUID
                    TOORow("HIPAA_BHT_GUID") = oD.BHT_GUID
                    TOORow("HIPAA_HL_GUID") = oD.HL_GUID
                    TOORow("HIPAA_NM1_GUID") = oD.HIPAA_NM1_GUID
                    TOORow("HIPAA_HCR_GUID") = oD.HIPAA_HCR_GUID
                    TOORow("HIPAA_UM_GUID") = oD.HIPAA_UM_GUID

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then TOORow("TOO01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else TOORow("TOO01") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then TOORow("TOO02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else TOORow("TOO02") = DBNull.Value

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then
                        TOORow("TOO03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4)
                        TOO03 = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4)
                    Else
                        TOORow("TOO03") = DBNull.Value

                    End If

                    If Not TOO03 = String.Empty Then

                        If (objss.ParseDemlimtedString(TOO03, oD.ComponentElementSeperator, 1) <> "") Then TOORow("TOO03_1") = objss.ParseDemlimtedString(TOO03, oD.ComponentElementSeperator, 1) Else TOORow("TOO03_1") = DBNull.Value
                        If (objss.ParseDemlimtedString(TOO03, oD.ComponentElementSeperator, 2) <> "") Then TOORow("TOO03_2") = objss.ParseDemlimtedString(TOO03, oD.ComponentElementSeperator, 2) Else TOORow("TOO03_2") = DBNull.Value
                        If (objss.ParseDemlimtedString(TOO03, oD.ComponentElementSeperator, 3) <> "") Then TOORow("TOO03_3") = objss.ParseDemlimtedString(TOO03, oD.ComponentElementSeperator, 3) Else TOORow("TOO03_3") = DBNull.Value
                        If (objss.ParseDemlimtedString(TOO03, oD.ComponentElementSeperator, 4) <> "") Then TOORow("TOO03_4") = objss.ParseDemlimtedString(TOO03, oD.ComponentElementSeperator, 4) Else TOORow("TOO03_4") = DBNull.Value
                        If (objss.ParseDemlimtedString(TOO03, oD.ComponentElementSeperator, 5) <> "") Then TOORow("TOO03_5") = objss.ParseDemlimtedString(TOO03, oD.ComponentElementSeperator, 5) Else TOORow("TOO03_5") = DBNull.Value

                    End If

                    TOO.Rows.Add(TOORow)

                End If





                If oD.ediRowRecordType = "TRN" Then

                    oD.RowProcessedFlag = 1

                    Dim TRNRow As DataRow = TRN.NewRow
                    TRNRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    TRNRow("HIPAA_GS_GUID") = oD.GS_GUID
                    TRNRow("HIPAA_ST_GUID") = oD.ST_GUID
                    TRNRow("HIPAA_BHT_GUID") = oD.BHT_GUID
                    TRNRow("HIPAA_HL_GUID") = oD.HL_GUID
                    TRNRow("HIPAA_NM1_GUID") = oD.HIPAA_NM1_GUID
                    TRNRow("HIPAA_HCR_GUID") = oD.HIPAA_HCR_GUID
                    TRNRow("HIPAA_UM_GUID") = oD.HIPAA_UM_GUID

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then TRNRow("TRN01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else TRNRow("TRN01") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then TRNRow("TRN02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else TRNRow("TRN02") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then TRNRow("TRN03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else TRNRow("TRN03") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then TRNRow("TRN04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else TRNRow("TRN04") = DBNull.Value

                    TRN.Rows.Add(TRNRow)

                End If

                If oD.ediRowRecordType = "PWK" Then

                    oD.RowProcessedFlag = 1

                    Dim PWKRow As DataRow = PWK.NewRow
                    PWKRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    PWKRow("HIPAA_GS_GUID") = oD.GS_GUID
                    PWKRow("HIPAA_ST_GUID") = oD.ST_GUID
                    PWKRow("HIPAA_BHT_GUID") = oD.BHT_GUID
                    PWKRow("HIPAA_HL_GUID") = oD.HL_GUID
                    PWKRow("HIPAA_NM1_GUID") = oD.HIPAA_NM1_GUID
                    PWKRow("HIPAA_HCR_GUID") = oD.HIPAA_HCR_GUID
                    PWKRow("HIPAA_UM_GUID") = oD.HIPAA_UM_GUID

                    'Commented below lines..because, table does not have these columns.
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then PWKRow("PWK01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else PWKRow("PWK01") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then PWKRow("PWK02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else PWKRow("PWK02") = DBNull.Value
                    'If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then PWKRow("PWK03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else PWKRow("PWK03") = DBNull.Value
                    'If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then PWKRow("PWK04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else PWKRow("PWK04") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then PWKRow("PWK05") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else PWKRow("PWK05") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then PWKRow("PWK06") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else PWKRow("PWK06") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then PWKRow("PWK07") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else PWKRow("PWK07") = DBNull.Value

                    'If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then PWKRow("PWK08") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else PWKRow("PWK08") = DBNull.Value
                    'If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then PWKRow("PWK09") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else PWKRow("PWK09") = DBNull.Value

                    PWK.Rows.Add(PWKRow)

                End If

                If oD.ediRowRecordType = "DMG" Then

                    oD.RowProcessedFlag = 1

                    Dim DMGRow As DataRow = DMG.NewRow
                    DMGRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    DMGRow("HIPAA_GS_GUID") = oD.GS_GUID
                    DMGRow("HIPAA_ST_GUID") = oD.ST_GUID
                    DMGRow("HIPAA_BHT_GUID") = oD.BHT_GUID
                    DMGRow("HIPAA_HL_GUID") = oD.HL_GUID
                    DMGRow("HIPAA_NM1_GUID") = oD.HIPAA_NM1_GUID
                    DMGRow("HIPAA_HCR_GUID") = oD.HIPAA_HCR_GUID
                    DMGRow("HIPAA_UM_GUID") = oD.HIPAA_UM_GUID

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then DMGRow("DMG01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else DMGRow("DMG01") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then DMGRow("DMG02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else DMGRow("DMG02") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then DMGRow("DMG03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else DMGRow("DMG03") = DBNull.Value

                    DMG.Rows.Add(DMGRow)

                End If


                If oD.ediRowRecordType = "MSG" Then

                    oD.RowProcessedFlag = 1

                    Dim MSGRow As DataRow = MSG.NewRow
                    MSGRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    MSGRow("HIPAA_GS_GUID") = oD.GS_GUID
                    MSGRow("HIPAA_ST_GUID") = oD.ST_GUID
                    MSGRow("HIPAA_BHT_GUID") = oD.BHT_GUID
                    MSGRow("HIPAA_HL_GUID") = oD.HL_GUID
                    MSGRow("HIPAA_NM1_GUID") = oD.HIPAA_NM1_GUID
                    MSGRow("HIPAA_HCR_GUID") = oD.HIPAA_HCR_GUID
                    MSGRow("HIPAA_UM_GUID") = oD.HIPAA_UM_GUID

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then MSGRow("MSG01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else MSGRow("MSG01") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then MSGRow("MSG02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else MSGRow("MSG02") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then MSGRow("MSG03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else MSGRow("MSG03") = DBNull.Value

                    MSG.Rows.Add(MSGRow)

                End If

                If oD.ediRowRecordType = "CL1" Then

                    oD.RowProcessedFlag = 1

                    Dim CL1Row As DataRow = CL1.NewRow
                    CL1Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                    CL1Row("HIPAA_GS_GUID") = oD.GS_GUID
                    CL1Row("HIPAA_ST_GUID") = oD.ST_GUID
                    CL1Row("HIPAA_BHT_GUID") = oD.BHT_GUID
                    CL1Row("HIPAA_HL_GUID") = oD.HL_GUID
                    CL1Row("HIPAA_NM1_GUID") = oD.HIPAA_NM1_GUID
                    CL1Row("HIPAA_HCR_GUID") = oD.HIPAA_HCR_GUID
                    CL1Row("HIPAA_UM_GUID") = oD.HIPAA_UM_GUID

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then CL1Row("CL101") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else CL1Row("CL101") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then CL1Row("CL102") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else CL1Row("CL102") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then CL1Row("CL103") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else CL1Row("CL103") = DBNull.Value

                    CL1.Rows.Add(CL1Row)

                End If


                If oD.ediRowRecordType = "CR1" Then

                    oD.RowProcessedFlag = 1

                    Dim CR1Row As DataRow = CR1.NewRow
                    CR1Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                    CR1Row("HIPAA_GS_GUID") = oD.GS_GUID
                    CR1Row("HIPAA_ST_GUID") = oD.ST_GUID
                    CR1Row("HIPAA_BHT_GUID") = oD.BHT_GUID
                    CR1Row("HIPAA_HL_GUID") = oD.HL_GUID
                    CR1Row("HIPAA_NM1_GUID") = oD.HIPAA_NM1_GUID
                    CR1Row("HIPAA_HCR_GUID") = oD.HIPAA_HCR_GUID
                    CR1Row("HIPAA_UM_GUID") = oD.HIPAA_UM_GUID

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then CR1Row("CR101") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else CR1Row("CR101") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then CR1Row("CR102") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else CR1Row("CR102") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then CR1Row("CR103") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else CR1Row("CR103") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then CR1Row("CR104") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else CR1Row("CR104") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then CR1Row("CR105") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else CR1Row("CR105") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then CR1Row("CR106") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else CR1Row("CR106") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then CR1Row("CR107") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else CR1Row("CR107") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then CR1Row("CR108") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else CR1Row("CR108") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then CR1Row("CR109") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else CR1Row("CR109") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then CR1Row("CR110") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else CR1Row("CR110") = DBNull.Value

                    CR1.Rows.Add(CR1Row)

                End If


                If oD.ediRowRecordType = "CR5" Then

                    oD.RowProcessedFlag = 1

                    Dim CR5Row As DataRow = CR5.NewRow
                    CR5Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                    CR5Row("HIPAA_GS_GUID") = oD.GS_GUID
                    CR5Row("HIPAA_ST_GUID") = oD.ST_GUID
                    CR5Row("HIPAA_BHT_GUID") = oD.BHT_GUID
                    CR5Row("HIPAA_HL_GUID") = oD.HL_GUID
                    CR5Row("HIPAA_NM1_GUID") = oD.HIPAA_NM1_GUID
                    CR5Row("HIPAA_HCR_GUID") = oD.HIPAA_HCR_GUID
                    CR5Row("HIPAA_UM_GUID") = oD.HIPAA_UM_GUID

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then CR5Row("CR501") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else CR5Row("CR501") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then CR5Row("CR502") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else CR5Row("CR502") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then CR5Row("CR503") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else CR5Row("CR503") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then CR5Row("CR504") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else CR5Row("CR504") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then CR5Row("CR505") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else CR5Row("CR505") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then CR5Row("CR506") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else CR5Row("CR506") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then CR5Row("CR507") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else CR5Row("CR507") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then CR5Row("CR508") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else CR5Row("CR508") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then CR5Row("CR509") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else CR5Row("CR509") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) <> "") Then CR5Row("CR510") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else CR5Row("CR510") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) <> "") Then CR5Row("CR511") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else CR5Row("CR511") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) <> "") Then CR5Row("CR512") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else CR5Row("CR512") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) <> "") Then CR5Row("CR513") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else CR5Row("CR513") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 15) <> "") Then CR5Row("CR514") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else CR5Row("CR514") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 16) <> "") Then CR5Row("CR515") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else CR5Row("CR515") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 17) <> "") Then CR5Row("CR516") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else CR5Row("CR516") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 18) <> "") Then CR5Row("CR517") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else CR5Row("CR517") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 19) <> "") Then CR5Row("CR518") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else CR5Row("CR518") = DBNull.Value

                    CR5.Rows.Add(CR5Row)

                End If

                If oD.ediRowRecordType = "CR6" Then

                    oD.RowProcessedFlag = 1

                    Dim CR6Row As DataRow = CR6.NewRow
                    CR6Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                    CR6Row("HIPAA_GS_GUID") = oD.GS_GUID
                    CR6Row("HIPAA_ST_GUID") = oD.ST_GUID
                    CR6Row("HIPAA_BHT_GUID") = oD.BHT_GUID
                    CR6Row("HIPAA_HL_GUID") = oD.HL_GUID
                    CR6Row("HIPAA_NM1_GUID") = oD.HIPAA_NM1_GUID
                    CR6Row("HIPAA_HCR_GUID") = oD.HIPAA_HCR_GUID
                    CR6Row("HIPAA_UM_GUID") = oD.HIPAA_UM_GUID

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then CR6Row("CR601") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else CR6Row("CR601") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then CR6Row("CR602") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else CR6Row("CR602") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then CR6Row("CR603") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else CR6Row("CR603") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then CR6Row("CR604") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else CR6Row("CR604") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then CR6Row("CR605") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else CR6Row("CR605") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then CR6Row("CR606") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else CR6Row("CR606") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then CR6Row("CR607") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else CR6Row("CR607") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then CR6Row("CR608") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else CR6Row("CR608") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then CR6Row("CR609") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else CR6Row("CR609") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) <> "") Then CR6Row("CR610") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) Else CR6Row("CR610") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) <> "") Then CR6Row("CR611") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) Else CR6Row("CR611") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) <> "") Then CR6Row("CR612") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) Else CR6Row("CR612") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) <> "") Then CR6Row("CR613") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) Else CR6Row("CR613") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 15) <> "") Then CR6Row("CR614") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 15) Else CR6Row("CR614") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 16) <> "") Then CR6Row("CR615") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 16) Else CR6Row("CR615") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 17) <> "") Then CR6Row("CR616") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 17) Else CR6Row("CR616") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 18) <> "") Then CR6Row("CR617") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 18) Else CR6Row("CR617") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 19) <> "") Then CR6Row("CR618") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 19) Else CR6Row("CR618") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 20) <> "") Then CR6Row("CR619") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 20) Else CR6Row("CR619") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 21) <> "") Then CR6Row("CR620") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 21) Else CR6Row("CR620") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 22) <> "") Then CR6Row("CR621") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 22) Else CR6Row("CR621") = DBNull.Value

                    CR6.Rows.Add(CR6Row)

                End If


                If oD.ediRowRecordType = "CRC" Then

                    oD.RowProcessedFlag = 1

                    Dim CRCRow As DataRow = CRC.NewRow
                    CRCRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    CRCRow("HIPAA_GS_GUID") = oD.GS_GUID
                    CRCRow("HIPAA_ST_GUID") = oD.ST_GUID
                    CRCRow("HIPAA_BHT_GUID") = oD.BHT_GUID
                    CRCRow("HIPAA_HL_GUID") = oD.HL_GUID
                    CRCRow("HIPAA_NM1_GUID") = oD.HIPAA_NM1_GUID
                    CRCRow("HIPAA_HCR_GUID") = oD.HIPAA_HCR_GUID
                    CRCRow("HIPAA_UM_GUID") = oD.HIPAA_UM_GUID

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then CRCRow("CRC01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else CRCRow("CRC01") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then CRCRow("CRC02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else CRCRow("CRC02") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then CRCRow("CRC03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else CRCRow("CRC03") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then CRCRow("CRC04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else CRCRow("CRC04") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then CRCRow("CRC05") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else CRCRow("CRC05") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then CRCRow("CRC06") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else CRCRow("CRC06") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then CRCRow("CRC07") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else CRCRow("CRC07") = DBNull.Value

                    CRC.Rows.Add(CRCRow)

                End If

                If oD.ediRowRecordType = "AAA" Then

                    oD.RowProcessedFlag = 1

                    Dim AAARow As DataRow = AAA.NewRow
                    AAARow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    AAARow("HIPAA_GS_GUID") = oD.GS_GUID
                    AAARow("HIPAA_ST_GUID") = oD.ST_GUID
                    AAARow("HIPAA_BHT_GUID") = oD.BHT_GUID
                    AAARow("HIPAA_HL_GUID") = oD.HL_GUID
                    AAARow("HIPAA_NM1_GUID") = oD.HIPAA_NM1_GUID
                    AAARow("HIPAA_HCR_GUID") = oD.HIPAA_HCR_GUID
                    AAARow("HIPAA_UM_GUID") = oD.HIPAA_UM_GUID


                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then AAARow("AAA01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else AAARow("AAA01") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then AAARow("AAA02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else AAARow("AAA02") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then AAARow("AAA03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else AAARow("AAA03") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then AAARow("AAA04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else AAARow("AAA04") = DBNull.Value


                    AAA.Rows.Add(AAARow)



                End If





                If oD.ediRowRecordType = "HSD" Then

                    oD.RowProcessedFlag = 1

                    Dim HSDRow As DataRow = HSD.NewRow
                    HSDRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    HSDRow("HIPAA_GS_GUID") = oD.GS_GUID
                    HSDRow("HIPAA_ST_GUID") = oD.ST_GUID
                    HSDRow("HIPAA_BHT_GUID") = oD.BHT_GUID
                    HSDRow("HIPAA_HL_GUID") = oD.HL_GUID
                    HSDRow("HIPAA_NM1_GUID") = oD.HIPAA_NM1_GUID
                    HSDRow("HIPAA_HCR_GUID") = oD.HIPAA_HCR_GUID
                    HSDRow("HIPAA_UM_GUID") = oD.HIPAA_UM_GUID

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then HSDRow("HSD01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else HSDRow("HSD01") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then HSDRow("HSD02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else HSDRow("HSD02") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then HSDRow("HSD03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else HSDRow("HSD03") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then HSDRow("HSD04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else HSDRow("HSD04") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then HSDRow("HSD05") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else HSDRow("HSD05") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then HSDRow("HSD06") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else HSDRow("HSD06") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then HSDRow("HSD07") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else HSDRow("HSD07") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then HSDRow("HSD08") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else HSDRow("HSD08") = DBNull.Value

                    HSD.Rows.Add(HSDRow)

                End If


                If oD.ediRowRecordType = "INS" Then

                    oD.RowProcessedFlag = 1

                    Dim INSRow As DataRow = INS.NewRow
                    INSRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    INSRow("HIPAA_GS_GUID") = oD.GS_GUID
                    INSRow("HIPAA_ST_GUID") = oD.ST_GUID
                    INSRow("HIPAA_BHT_GUID") = oD.BHT_GUID
                    INSRow("HIPAA_HL_GUID") = oD.HL_GUID
                    INSRow("HIPAA_NM1_GUID") = oD.HIPAA_NM1_GUID
                    INSRow("HIPAA_HCR_GUID") = oD.HIPAA_HCR_GUID
                    INSRow("HIPAA_UM_GUID") = oD.HIPAA_UM_GUID

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then INSRow("INS01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else INSRow("INS01") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then INSRow("INS02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else INSRow("INS02") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then INSRow("INS08") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else INSRow("INS08") = DBNull.Value

                    INS.Rows.Add(INSRow)

                End If



                If oD.RowProcessedFlag = 0 Then
                    Dim UNKRow As DataRow = UNK.NewRow
                    UNKRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    UNKRow("HIPAA_GS_GUID") = oD.GS_GUID
                    UNKRow("HIPAA_ST_GUID") = oD.ST_GUID
                    UNKRow("HIPAA_HL_GUID") = oD.HL_GUID
                    UNKRow("ROW_RECORD_TYPE") = oD.ediRowRecordType
                    UNKRow("ROW_DATA") = oD.CurrentRowData

                    UNK.Rows.Add(UNKRow)


                End If



                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '      END adding LX tot he LX List
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


                If oD.ediRowRecordType = "SE" Then
                    ComitALL()
                    oD.HL20Flag = 0
                    oD.RowProcessedFlag = 1
                    oD.EBFlag = 0

                End If


                If oD.ediRowRecordType = "GE" Then
                    oD.RowProcessedFlag = 1


                    ComitGS()
                    'committ the last ST
                    GS.Clear()
                    oD.GSFlag = 0
                    oD.GS_GUID = Guid.Empty


                End If



                If oD.ediRowRecordType = "IEA" Then
                    ComitISA()

                    oD.ISA_GUID = Guid.Empty
                    oD.IEAFlag = 1
                    oD.ISAFlag = 0

                End If



                ''Console.WriteLine(last)
                ''Console.WriteLine(rowcount)

                'Catch ex As Exception
                '    rr = 30
                '    oD.IEAFlag = 1
                '    RollBack()
                '    log.ExceptionDetails(oD._BatchID.ToString, "DCSGlobal.EDI.Import278 was rolled back due to Error in main loop " + oD.ediRowRecordType + "Rowcount " + Convert.ToString(rowcount))
                '    log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import278 error in main loop" + FileToParse, ex)

                'Finally
                '    '  COMITUNK()

                'End Try

            Next


            If oD.IEAFlag = 0 Then
                rr = 40
                RollBack()
                log.ExceptionDetails(oD._BatchID.ToString, "DCSGlobal.EDI.Import278was rolled back due to IEA Not Found" + FileToParse)

            End If


            If V Then
                [end] = DateTime.Now
                log.ExceptionDetails("DCSGlobal.EDI.Import278.Import", "### Overall End Time: " + [end].ToLongTimeString() + " " + FileToParse)
                log.ExceptionDetails("DCSGlobal.EDI.Import278.Import", "### Overall Run Time: " + ([end] - start).ToString() + " " + FileToParse)
            End If


            Return rr

        End Function

        Private Function InsertFileName(ByVal FileName As String) As Int32


            Dim rr As Int32

            Dim param As New SqlParameter()


            Dim sqlString As String
            sqlString = "usp_HIPAA_EDI_ADD_FILE"

            Try

                Using Con As New SqlConnection(oD._ConnectionString)
                    Con.Open()
                    Using Com As New SqlCommand(sqlString, Con)

                        Com.CommandType = CommandType.StoredProcedure
                        Com.Parameters.AddWithValue("@FILE_PATH", FileName)
                        Com.Parameters.AddWithValue("@EDI_TYPE", oD.edi_type)

                        Using RDR = Com.ExecuteReader()
                            If RDR.HasRows Then
                                Do While RDR.Read
                                    rr = Convert.ToInt32(RDR.Item(0))
                                Loop
                            End If
                        End Using
                    End Using
                    Con.Close()
                End Using

            Catch sx As SqlException



                log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.InsertFileName", sx)


            Catch ex As Exception
                rr = -1

                log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.InsertFileName", ex)

            Finally


            End Try





            Return rr

        End Function

        Private Function InsertFileName(ByVal FilePath As String, ByVal FileName As String) As Int32


            Dim rr As Integer = -1

            Dim sqlString As String
            sqlString = "usp_EDI_278_ADD_FILE"

            Try

                Using Con As New SqlConnection(oD._ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@FILE_NAME", SqlDbType.VarChar, 255).Value = FileName
                        cmd.Parameters.Add("@FILE_PATH", SqlDbType.VarChar).Value = FilePath
                        cmd.Parameters.Add("@EDI_TYPE", SqlDbType.VarChar, 50).Value = "278"
                        'Commented below line. Because getting error when we load files from the folder.
                        'cmd.Parameters.Add("@FILE_ID", SqlDbType.BigInt)
                        'cmd.Parameters("@FILE_ID").Direction = ParameterDirection.Output

                        cmd.ExecuteNonQuery()

                        'Commented below line. Because getting error when we load files from the folder.
                        '_FileID = Convert.ToInt32(cmd.Parameters("@FILE_ID").Value)

                        rr = 0
                    End Using
                    Con.Close()
                End Using

            Catch sx As SqlException



                log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.InsertFileName", sx)


            Catch ex As Exception
                rr = -1

                log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.InsertFileName", ex)

            Finally


            End Try

            Return oD.rBatchId


            'Return rr

        End Function



        'Private Function ComitISAGS() As Integer


        '    Dim i As Integer
        '    Dim param As New SqlParameter()


        '    Dim sqlConn As SqlConnection = New SqlConnection
        '    Dim cmd As SqlCommand
        '    Dim sqlString As String


        '    sqlConn.ConnectionString = oD._ConnectionString
        '    sqlConn.Open()

        '    Try


        '        sqlString = "usp_EDI_278_ISA_GS"


        '        cmd = New SqlCommand(sqlString, sqlConn)
        '        cmd.CommandType = CommandType.StoredProcedure
        '        cmd.Parameters.AddWithValue("@HIPAA_278_ISA", ISA)
        '        cmd.Parameters.AddWithValue("@HIPAA_278_GS", GS)

        '        cmd.Parameters.AddWithValue("@FILE_ID", oD._FileID)
        '        'usp_EDI_278_ISA_GS stored procedure does not have ST_ID. Commenting out
        '        cmd.Parameters.AddWithValue("@ST_ID", oD._STID)
        '        '_CLPString = String.Empty

        '        err = cmd.ExecuteNonQuery()


        '        i = 0

        '    Catch ex As Exception
        '        i = -1

        '        log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import278ig", ex)

        '    Finally

        '        sqlConn.Close()



        '    End Try

        '    ISA.Clear()
        '    GS.Clear()





        '    Return i

        'End Function






        Private Function ComitALL() As Integer


            Dim i As Integer
            Dim param As New SqlParameter()


            Dim sqlConn As SqlConnection = New SqlConnection
            Dim cmd As SqlCommand
            Dim sqlString As String


            sqlConn.ConnectionString = oD._ConnectionString
            sqlConn.Open()

            Try


                sqlString = "usp_EDI_278"


                cmd = New SqlCommand(sqlString, sqlConn)
                cmd.CommandType = CommandType.StoredProcedure
                'cmd.Parameters.AddWithValue("@HIPAA_278_ISA", ISA)
                ' cmd.Parameters.AddWithValue("@HIPAA_278_GS", GS)
                cmd.Parameters.AddWithValue("@HIPAA_278_ST", ST)
                cmd.Parameters.AddWithValue("@HIPAA_278_BHT", BHT)
                cmd.Parameters.AddWithValue("@HIPAA_278_HL", HL)

                cmd.Parameters.AddWithValue("@HIPAA_278_NM1", NM1)
                cmd.Parameters.AddWithValue("@HIPAA_278_REF", REF)
                cmd.Parameters.AddWithValue("@HIPAA_278_TRN", TRN)
                cmd.Parameters.AddWithValue("@HIPAA_278_N3", N3)
                cmd.Parameters.AddWithValue("@HIPAA_278_N4", N4)
                cmd.Parameters.AddWithValue("@HIPAA_278_PER", PER)
                cmd.Parameters.AddWithValue("@HIPAA_278_UM", UM)
                cmd.Parameters.AddWithValue("@HIPAA_278_HCR", HCR)
                cmd.Parameters.AddWithValue("@HIPAA_278_DTP", DTP)
                cmd.Parameters.AddWithValue("@HIPAA_278_HI", HI)
                cmd.Parameters.AddWithValue("@HIPAA_278_SV1", SV1)
                cmd.Parameters.AddWithValue("@HIPAA_278_SV2", SV2)


                cmd.Parameters.AddWithValue("@HIPAA_278_DMG", DMG)
                cmd.Parameters.AddWithValue("@HIPAA_278_MSG", MSG)
                cmd.Parameters.AddWithValue("@HIPAA_278_CL1", CL1)
                cmd.Parameters.AddWithValue("@HIPAA_278_HSD", HSD)



                cmd.Parameters.AddWithValue("@HIPAA_278_CR1", CR1)
                cmd.Parameters.AddWithValue("@HIPAA_278_CR5", CR5)
                cmd.Parameters.AddWithValue("@HIPAA_278_CR6", CR6)
                cmd.Parameters.AddWithValue("@HIPAA_278_SV3", SV3)
                cmd.Parameters.AddWithValue("@HIPAA_278_TOO", TOO)

                cmd.Parameters.AddWithValue("@HIPAA_278_PRV", PRV)
                cmd.Parameters.AddWithValue("@HIPAA_278_PWK", PWK)

                cmd.Parameters.AddWithValue("@HIPAA_278_CRC", CRC)
                cmd.Parameters.AddWithValue("@HIPAA_278_AAA", AAA)

                cmd.Parameters.AddWithValue("@HIPAA_278_INS", INS)
                ''cmd.Parameters.AddWithValue("@HIPAA_278_CN1", N4)

                'cmd.Parameters.AddWithValue("@HIPAA_278_UNK", UNK)

                cmd.Parameters.AddWithValue("@ebr_id", oD.ebr_id)
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

                cmd.Parameters.AddWithValue("@BATCH_ID", _BatchID)

                '  cmd.Parameters.AddWithValue("@BATCH_ID", )
                cmd.Parameters.AddWithValue("@FILE_ID", oD._FileID)
                cmd.Parameters.AddWithValue("@ST_ID", oD._STID)
                '_CLPString = String.Empty
                err = cmd.ExecuteNonQuery()
                i = 0

            Catch ex As Exception
                i = -1

                log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import278", ex)

            Finally

                sqlConn.Close()



            End Try


            ST.Clear()
            BHT.Clear()


            CL1.Clear()
            CR1.Clear()
            CR5.Clear()
            CR6.Clear()
            CRC.Clear()
            DMG.Clear()
            DTP.Clear()

            HCR.Clear()
            HI.Clear()
            HL.Clear()
            HSD.Clear()

            INS.Clear()
            MSG.Clear()
            N3.Clear()
            N4.Clear()
            NM1.Clear()
            PER.Clear()
            PRV.Clear()
            PWK.Clear()
            REF.Clear()

            SV1.Clear()
            SV2.Clear()
            SV3.Clear()
            TOO.Clear()
            TRN.Clear()
            UM.Clear()
            UNK.Clear()

            Return i

        End Function

        Private Function ComitISA() As Integer


            Dim i As Integer
            Dim param As New SqlParameter()


            Dim sqlString As String

            Try


                sqlString = "usp_EDI_278_ISA"

                Using Con As New SqlConnection(oD._ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)

                        cmd.CommandType = CommandType.StoredProcedure


                        cmd.Parameters.AddWithValue("@HIPAA_278_ISA", ISA)
                        cmd.Parameters.AddWithValue("@FILE_ID", _FileID)
                        cmd.Parameters.AddWithValue("@BATCH_ID", _BatchID)
                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using




                i = 0

            Catch ex As Exception
                i = -1


                log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import277.COMITISAGS", ex)

            Finally

                'sqlConn.Close()
            End Try

            Return i

        End Function




        Private Function ComitGS() As Integer


            Dim i As Integer
            Dim param As New SqlParameter()


            Dim sqlString As String

            Try


                sqlString = "usp_EDI_278_GS"

                Using Con As New SqlConnection(oD._ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)

                        cmd.CommandType = CommandType.StoredProcedure


                        cmd.Parameters.AddWithValue("@HIPAA_278_GS", GS)
                        cmd.Parameters.AddWithValue("@FILE_ID", _FileID)
                        cmd.Parameters.AddWithValue("@BATCH_ID", _BatchID)
                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using




                i = 0

            Catch ex As Exception
                i = -1


                log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import277.COMITISAGS", ex)

            Finally

                'sqlConn.Close()
            End Try

            Return i

        End Function



        Private Function RollBack() As Integer


            Dim i As Integer
            Dim param As New SqlParameter()

            Dim sqlString As String


            Try


                sqlString = "[usp_EDI_278_ROLLBACK]"


                Using Con As New SqlConnection(oD._ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)

                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.AddWithValue("@FILE_ID", oD._FileID)
                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using

                i = 0

            Catch ex As Exception
                i = -1

                log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import278.rollback for bactch ID " + oD._BatchID.ToString, ex)

            Finally

            End Try

            Return i



        End Function



        Public Function COMITUNK() As Integer


            Dim i As Integer
            Dim param As New SqlParameter()


            Dim sqlConn As SqlConnection = New SqlConnection
            Dim cmd As SqlCommand
            Dim sqlString As String


            sqlConn.ConnectionString = oD._ConnectionString
            sqlConn.Open()

            Try


                sqlString = "[usp_EDI_278_UNK]"


                cmd = New SqlCommand(sqlString, sqlConn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@ST_ID", oD._STID)
                cmd.Parameters.AddWithValue("@HIPAA_278_UNK", UNK)
                cmd.Parameters.AddWithValue("@FILE_ID", _FileID)
                cmd.Parameters.AddWithValue("@BATCH_ID", _BatchID)

                err = cmd.ExecuteNonQuery()



                i = 0

            Catch ex As Exception
                i = -1

                log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import278.COMITUNK" + oD._BatchID.ToString, ex)

            Finally

                sqlConn.Close()
            End Try

            Return i



        End Function


    End Class
End Namespace
