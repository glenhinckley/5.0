
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


    Public Class Import276



        Inherits EDI276Tables

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
        Private _Debug As Integer = 0
        Private V As Boolean = False

        Private _FileToParse As String
        Private _CLPString As String

        Private SVC01 As String = String.Empty
        Private SVC06 As String = String.Empty
        Private _BatchID As Double = 0


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
        Private HL_PARENT As String
        Dim _isFile As Boolean
        Dim _FileID As Integer = 200

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

        Public WriteOnly Property pat_hosp_code As String

            Set(value As String)
                oD.pat_hosp_code = value
            End Set
        End Property


        Public WriteOnly Property cbr_id As Double


            Set(value As Double)
                oD.cbr_id = value
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

        Public WriteOnly Property isFile As Boolean


            Set(value As Boolean)
                _isFile = value
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

        Public WriteOnly Property Patient_number As String

            Set(value As String)
                oD.Patient_number = value
            End Set
        End Property


        Public WriteOnly Property ins_type As String

            Set(value As String)
                oD.ins_type = value
            End Set
        End Property


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

                    r = Import276()

                End If
            End If


            Return r

        End Function




        Public Function Import(ByVal EDI As String) As Integer

            Dim x As Integer = -1



            '  //   _EDIList = StringPrep


            x = Import276()

            Return x


            Return x
        End Function


        Public Function ImportByString(ByVal EDI As String, ByVal BatchID As Double) As Integer

            Dim r As Integer = -1



            Dim SP As New StringPrep()
            _BatchID = BatchID

            _EDIList = SP.SingleEDI(EDI)

            r = Import276()
            Return r

        End Function




        Public Function Import(ByVal EDI As String, ByVal BatchID As Double) As Integer


            Dim x As Integer = -1




            Return x


        End Function

        Public Function Import(ByVal EDIList As List(Of String), ByVal BatchID As Double) As Int32
            Dim x As Integer = -1
            _EDIList = EDIList
            _BatchID = BatchID

            x = Import276()

            Return x

        End Function

        Private Function Import276() As Integer

            '   _FileToParse = FileToParse

            oD.edi_type = "276"

            oD.HL20Flag = 0
            oD.DataElementSeparatorFlag = 0

            Dim filePath As String = "C:\MyDir\MySubDir\myfile.ext"
            Dim directoryPath As String = Path.GetDirectoryName(filePath)

            Dim [end] As DateTime
            Dim start As DateTime = DateTime.Now


            If _isFile = True Then
                oD._FileID = InsertFileName("", _FileToParse)

            Else

                oD._FileID = 0

            End If





            Dim rr As Integer = 0


            '     Try

            start_time = Now
            oD.TimeStamp = FormatDateTime(start_time, DateFormat.ShortDate)



            oD.IEAFlag = False


            If V Then
                log.ExceptionDetails("DCSGlobal.EDI.Import276.Import", "Pasring File :LIST")
                log.ExceptionDetails("DCSGlobal.EDI.Import276.Import", "### Overall Start Time: LIST " + start.ToLongTimeString())
            End If

            _CLPString = String.Empty




            oD.BATCHUID = Guid.NewGuid()





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

                DTP.Clear()
                HL.Clear()

                DMG.Clear()
                SVC.Clear()



                TRN.Clear()
                UNK.Clear()

            End If

            Dim last As String = String.Empty
            '   Dim line As String = String.Empty
            Dim rowcount As Int32 = 0

            'so lets get down to it opn the file in a stream reader and move thru it one line at a time...........

            '  Using r As New StreamReader(FileToParse)
            oD.RowProcessedFlag = 0
            '  line = r.ReadLine()
            'Console.WriteLine(line)
            ' oD.DataElementSeparator = Mid(line, 4, 1)

            'Console.WriteLine(oD.DataElementSeparator)

            '  Try
            For Each line As String In _EDIList


                If oD.DataElementSeparatorFlag = 0 Then
                    oD.DataElementSeparator = Mid(line, 4, 1)
                    oD.DataElementSeparatorFlag = 1
                End If


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

                '   line = r.ReadLine


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

                    ComitISAGS()

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
                    oD.STC_GUID = Guid.Empty

                    If oD.HL20Flag = 0 Then

                        NM1.Clear()

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


                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then

                        HL_PARENT = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2)
                    Else
                        HL_PARENT = 0


                    End If


                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then HLRow("HL01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else HLRow("HL01") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then HLRow("HL02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else HLRow("HL02") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then HLRow("HL03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else HLRow("HL03") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then HLRow("HL04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else HLRow("HL04") = DBNull.Value

                    HL.Rows.Add(HLRow)

                End If





                If oD.ediRowRecordType = "SVC" Then

                    oD.RowProcessedFlag = 1
                    SVC01 = String.Empty
                    SVC06 = String.Empty
                    oD.SVC_GUID = Guid.NewGuid

                    Dim SVCRow As DataRow = SVC.NewRow
                    SVCRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    SVCRow("HIPAA_GS_GUID") = oD.GS_GUID
                    SVCRow("HIPAA_ST_GUID") = oD.ST_GUID
                    SVCRow("HIPAA_BHT_GUID") = oD.BHT_GUID
                    SVCRow("HIPAA_HL_GUID") = oD.HL_GUID
                    SVCRow("HIPAA_SVC_GUID") = oD.SVC_GUID
                    SVCRow("HL_PARENT") = HL_PARENT






                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then
                        SVCRow("SVC01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2)
                        SVC01 = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2)
                    Else
                        SVCRow("SVC01") = DBNull.Value

                    End If


                    If Not SVC01 = String.Empty Then

                        If (objss.ParseDemlimtedString(SVC01, oD.ComponentElementSeperator, 1) <> "") Then SVCRow("SVC01_1") = objss.ParseDemlimtedString(SVC01, oD.ComponentElementSeperator, 1) Else SVCRow("SVC01_1") = DBNull.Value
                        If (objss.ParseDemlimtedString(SVC01, oD.ComponentElementSeperator, 2) <> "") Then SVCRow("SVC01_2") = objss.ParseDemlimtedString(SVC01, oD.ComponentElementSeperator, 2) Else SVCRow("SVC01_2") = DBNull.Value
                        If (objss.ParseDemlimtedString(SVC01, oD.ComponentElementSeperator, 3) <> "") Then SVCRow("SVC01_3") = objss.ParseDemlimtedString(SVC01, oD.ComponentElementSeperator, 3) Else SVCRow("SVC01_3") = DBNull.Value
                        If (objss.ParseDemlimtedString(SVC01, oD.ComponentElementSeperator, 4) <> "") Then SVCRow("SVC01_4") = objss.ParseDemlimtedString(SVC01, oD.ComponentElementSeperator, 4) Else SVCRow("SVC01_4") = DBNull.Value
                        If (objss.ParseDemlimtedString(SVC01, oD.ComponentElementSeperator, 5) <> "") Then SVCRow("SVC01_5") = objss.ParseDemlimtedString(SVC01, oD.ComponentElementSeperator, 5) Else SVCRow("SVC01_5") = DBNull.Value
                        If (objss.ParseDemlimtedString(SVC01, oD.ComponentElementSeperator, 6) <> "") Then SVCRow("SVC01_6") = objss.ParseDemlimtedString(SVC01, oD.ComponentElementSeperator, 6) Else SVCRow("SVC01_6") = DBNull.Value
                        If (objss.ParseDemlimtedString(SVC01, oD.ComponentElementSeperator, 7) <> "") Then SVCRow("SVC01_7") = objss.ParseDemlimtedString(SVC01, oD.ComponentElementSeperator, 7) Else SVCRow("SVC01_7") = DBNull.Value





                    End If







                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then SVCRow("SVC02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else SVCRow("SVC02") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then SVCRow("SVC03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else SVCRow("SVC03") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then SVCRow("SVC04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else SVCRow("SVC04") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then SVCRow("SVC05") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else SVCRow("SVC05") = DBNull.Value




                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then SVCRow("SVC06") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) Else SVCRow("SVC06") = DBNull.Value





                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then
                        SVCRow("SVC06") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7)
                        SVC06 = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7)
                    Else
                        SVCRow("SVC06") = DBNull.Value

                    End If

                    If (objss.ParseDemlimtedString(SVC06, oD.ComponentElementSeperator, 1) <> "") Then SVCRow("SVC06_1") = objss.ParseDemlimtedString(SVC06, oD.ComponentElementSeperator, 1) Else SVCRow("SVC06_1") = DBNull.Value
                    If (objss.ParseDemlimtedString(SVC06, oD.ComponentElementSeperator, 2) <> "") Then SVCRow("SVC06_2") = objss.ParseDemlimtedString(SVC06, oD.ComponentElementSeperator, 2) Else SVCRow("SVC06_2") = DBNull.Value
                    If (objss.ParseDemlimtedString(SVC06, oD.ComponentElementSeperator, 3) <> "") Then SVCRow("SVC06_3") = objss.ParseDemlimtedString(SVC06, oD.ComponentElementSeperator, 3) Else SVCRow("SVC06_3") = DBNull.Value
                    If (objss.ParseDemlimtedString(SVC06, oD.ComponentElementSeperator, 4) <> "") Then SVCRow("SVC06_4") = objss.ParseDemlimtedString(SVC06, oD.ComponentElementSeperator, 4) Else SVCRow("SVC06_4") = DBNull.Value
                    If (objss.ParseDemlimtedString(SVC06, oD.ComponentElementSeperator, 5) <> "") Then SVCRow("SVC06_5") = objss.ParseDemlimtedString(SVC06, oD.ComponentElementSeperator, 5) Else SVCRow("SVC06_5") = DBNull.Value
                    If (objss.ParseDemlimtedString(SVC06, oD.ComponentElementSeperator, 6) <> "") Then SVCRow("SVC06_6") = objss.ParseDemlimtedString(SVC06, oD.ComponentElementSeperator, 6) Else SVCRow("SVC06_6") = DBNull.Value
                    If (objss.ParseDemlimtedString(SVC06, oD.ComponentElementSeperator, 7) <> "") Then SVCRow("SVC06_7") = objss.ParseDemlimtedString(SVC06, oD.ComponentElementSeperator, 7) Else SVCRow("SVC06_7") = DBNull.Value





                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then SVCRow("SVC07") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else SVCRow("SVC07") = DBNull.Value


                    SVCRow("BATCH_ID") = oD._BatchID
                    SVCRow("TIME_STAMP") = oD.TimeStamp
                    SVC.Rows.Add(SVCRow)


                End If


                If oD.ediRowRecordType = "NM1" Then

                    oD.RowProcessedFlag = 1

                    Dim NM1Row As DataRow = NM1.NewRow
                    NM1Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                    NM1Row("HIPAA_GS_GUID") = oD.GS_GUID
                    NM1Row("HIPAA_ST_GUID") = oD.ST_GUID
                    NM1Row("HIPAA_BHT_GUID") = oD.BHT_GUID
                    NM1Row("HIPAA_HL_GUID") = oD.HL_GUID
                    NM1Row("HIPAA_SVC_GUID") = oD.SVC_GUID
                    NM1Row("HL_PARENT") = HL_PARENT


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



                If oD.ediRowRecordType = "REF" Then

                    oD.RowProcessedFlag = 1

                    Dim REFRow As DataRow = REF.NewRow
                    REFRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    REFRow("HIPAA_GS_GUID") = oD.GS_GUID
                    REFRow("HIPAA_ST_GUID") = oD.ST_GUID
                    REFRow("HIPAA_BHT_GUID") = oD.BHT_GUID
                    REFRow("HIPAA_HL_GUID") = oD.HL_GUID
                    REFRow("HIPAA_SVC_GUID") = oD.SVC_GUID
                    REFRow("HL_PARENT") = HL_PARENT

                    ' REFRow("P_GUID") = oD.P_GUID

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then REFRow("REF01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else REFRow("REF01") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then REFRow("REF02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else REFRow("REF02") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then REFRow("REF03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else REFRow("REF03") = DBNull.Value


                    REF.Rows.Add(REFRow)


                End If



                If oD.ediRowRecordType = "DTP" Then

                    oD.RowProcessedFlag = 1

                    Dim DTPRow As DataRow = DTP.NewRow
                    DTPRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    DTPRow("HIPAA_GS_GUID") = oD.GS_GUID
                    DTPRow("HIPAA_ST_GUID") = oD.ST_GUID
                    DTPRow("HIPAA_BHT_GUID") = oD.BHT_GUID
                    DTPRow("HIPAA_HL_GUID") = oD.HL_GUID
                    DTPRow("HIPAA_SVC_GUID") = oD.SVC_GUID
                    DTPRow("HL_PARENT") = HL_PARENT

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then DTPRow("DTP01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else DTPRow("DTP01") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then DTPRow("DTP02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else DTPRow("DTP02") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then DTPRow("DTP03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else DTPRow("DTP03") = DBNull.Value
                    '    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then DTPRow("DTP04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else DTPRow("DTP04") = DBNull.Value

                    DTP.Rows.Add(DTPRow)

                End If


                If oD.ediRowRecordType = "TRN" Then

                    oD.RowProcessedFlag = 1

                    Dim TRNRow As DataRow = TRN.NewRow
                    TRNRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    TRNRow("HIPAA_GS_GUID") = oD.GS_GUID
                    TRNRow("HIPAA_ST_GUID") = oD.ST_GUID
                    TRNRow("HIPAA_BHT_GUID") = oD.BHT_GUID
                    TRNRow("HIPAA_HL_GUID") = oD.HL_GUID
                    TRNRow("HIPAA_SVC_GUID") = oD.SVC_GUID
                    TRNRow("HL_PARENT") = HL_PARENT



                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then TRNRow("TRN01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else TRNRow("TRN01") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then TRNRow("TRN02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else TRNRow("TRN02") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then TRNRow("TRN03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else TRNRow("TRN03") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then TRNRow("TRN04") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else TRNRow("TRN04") = DBNull.Value

                    TRN.Rows.Add(TRNRow)

                End If



                If oD.ediRowRecordType = "DMG" Then

                    oD.RowProcessedFlag = 1

                    Dim DMGRow As DataRow = DMG.NewRow
                    DMGRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    DMGRow("HIPAA_GS_GUID") = oD.GS_GUID
                    DMGRow("HIPAA_ST_GUID") = oD.ST_GUID
                    DMGRow("HIPAA_BHT_GUID") = oD.BHT_GUID
                    DMGRow("HIPAA_HL_GUID") = oD.HL_GUID
                    DMGRow("HIPAA_SVC_GUID") = oD.SVC_GUID
                    DMGRow("HL_PARENT") = HL_PARENT

                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then DMGRow("DMG01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else DMGRow("DMG01") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then DMGRow("DMG02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else DMGRow("DMG02") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then DMGRow("DMG03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else DMGRow("DMG03") = DBNull.Value

                    DMG.Rows.Add(DMGRow)

                End If



                If oD.ediRowRecordType = "AMT" Then

                    oD.RowProcessedFlag = 1

                    Dim AMTRow As DataRow = AMT.NewRow
                    AMTRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    AMTRow("HIPAA_GS_GUID") = oD.GS_GUID
                    AMTRow("HIPAA_ST_GUID") = oD.ST_GUID
                    AMTRow("HIPAA_BHT_GUID") = oD.BHT_GUID
                    AMTRow("HIPAA_HL_GUID") = oD.HL_GUID
                    AMTRow("HIPAA_SVC_GUID") = oD.SVC_GUID
                    AMTRow("HL_PARENT") = HL_PARENT



                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then AMTRow("AMT01") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else AMTRow("AMT01") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then AMTRow("AMT02") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else AMTRow("AMT02") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then AMTRow("AMT03") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else AMTRow("AMT03") = DBNull.Value


                    AMT.Rows.Add(AMTRow)



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


                    ' COMMIT THE LAST LK SET SINCE WE WONT GET TO lx AGAIN AND THE LAST clp SET
                    'ComitLX()
                    ComitALL()

                    '    ComitST()
                    ST.Clear()

                    oD.HL20Flag = 0

                    oD.RowProcessedFlag = 1
                    oD.EBFlag = 0

                End If


                If oD.ediRowRecordType = "GE" Then
                    oD.RowProcessedFlag = 1

                    'committ the last ST

                    oD.GSFlag = 0
                    oD.GS_GUID = Guid.Empty


                End If



                If oD.ediRowRecordType = "IEA" Then
                    oD.ISA_GUID = Guid.Empty
                    oD.IEAFlag = 1
                    oD.ISAFlag = 0

                End If

            Next

            'Console.WriteLine(last)
            'Console.WriteLine(rowcount)

            'Catch ex As Exception
            '    rr = -30
            '    oD.IEAFlag = 1
            '    RollBack()
            '    log.ExceptionDetails(oD._BatchID.ToString, "DCSGlobal.EDI.Import837I was rolled back due to Error in main loop " + oD.ediRowRecordType + "Rowcount " + Convert.ToString(rowcount))
            '    log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import837 error in main loop" + FileToParse, ex)

            'Finally
            COMITUNK()

            'End Try

            '    End Using


            If oD.IEAFlag = 0 Then
                rr = -40
                RollBack()
                log.ExceptionDetails(oD._BatchID.ToString, "DCSGlobal.EDI.Import276was rolled back due to IEA Not Found")

            End If


            If V Then
                [end] = DateTime.Now
                log.ExceptionDetails("DCSGlobal.EDI.Import276.Import", "### Overall End Time: " + [end].ToLongTimeString() + " ")
                log.ExceptionDetails("DCSGlobal.EDI.Import276.Import", "### Overall Run Time: " + ([end] - start).ToString() + " ")
            End If

            'fixxx here   ***************************************************
            'Return rr
            If rr >= 0 Then
                Return oD.rBatchId
            Else
                Return rr
            End If




        End Function




        Private Function InsertFileName(ByVal FilePath As String, FileName As String) As Int32


            Dim rr As Int32

            Dim param As New SqlParameter()


            Dim sqlString As String
            sqlString = "usp_EDI_276_ADD_FILE"

            Try

                Using Con As New SqlConnection(oD._ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@FILE_NAME", SqlDbType.VarChar, 255).Value = FileName
                        cmd.Parameters.Add("@FILE_PATH", SqlDbType.VarChar).Value = FilePath
                        cmd.Parameters.Add("@EDI_TYPE", SqlDbType.VarChar, 50).Value = "276"



                        cmd.Parameters.Add("@FILE_ID", SqlDbType.BigInt)
                        cmd.Parameters("@FILE_ID").Direction = ParameterDirection.Output


                        cmd.ExecuteNonQuery()

                        _FileID = Convert.ToInt32(cmd.Parameters("@FILE_ID").Value)

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

            ComitISAGS()



            Return rr

        End Function




        Private Function ComitALL() As Integer


            Dim i As Integer = -1
            Dim sqlString As String



            Try


                sqlString = "usp_claim_status_request_dump_276"


                Using con As New SqlConnection(oD._ConnectionString)
                    con.Open()
                    Using cmd As New SqlCommand(sqlString, con)

                        cmd.CommandType = CommandType.StoredProcedure


                        cmd.Parameters.AddWithValue("@HIPAA_276_ISA", ISA)
                        cmd.Parameters.AddWithValue("@HIPAA_276_GS", GS)

                        cmd.Parameters.AddWithValue("@HIPAA_276_ST", ST)
                        cmd.Parameters.AddWithValue("@HIPAA_276_BHT", BHT)
                        cmd.Parameters.AddWithValue("@HIPAA_276_HL", HL)
                        cmd.Parameters.AddWithValue("@HIPAA_276_DTP", DTP)
                        cmd.Parameters.AddWithValue("@HIPAA_276_DMG", DMG)




                        cmd.Parameters.AddWithValue("@HIPAA_276_REF", REF)
                        cmd.Parameters.AddWithValue("@HIPAA_276_NM1", NM1)
                        cmd.Parameters.AddWithValue("@HIPAA_276_AMT", AMT)
                        cmd.Parameters.AddWithValue("@HIPAA_276_TRN", TRN)

                        cmd.Parameters.AddWithValue("@HIPAA_276_SVC", SVC)
                        cmd.Parameters.AddWithValue("@HIPAA_276_UNK", UNK)



                        cmd.Parameters.AddWithValue("@DELETE_FLAG", oD.DeleteFlag)
                        cmd.Parameters.AddWithValue("@cbr_id", oD.cbr_id)
                        ''  cmd.Parameters.AddWithValue("@batch_id", oD._BatchID)
                        cmd.Parameters.AddWithValue("@user_id", oD.user_id)
                        cmd.Parameters.AddWithValue("@hosp_code", oD.hosp_code)
                        cmd.Parameters.AddWithValue("@source", oD.source)

                        cmd.Parameters.AddWithValue("@FILE_ID", _FileID)





                        cmd.Parameters.AddWithValue("@pat_hosp_code", oD.pat_hosp_code)
                        cmd.Parameters.AddWithValue("@ins_type", oD.ins_type)
                        '' @request_xml

                        cmd.Parameters.AddWithValue("@request_xml", oD.edi)

                        cmd.Parameters.AddWithValue("@Patient_number", oD.Patient_number)
                        cmd.Parameters.AddWithValue("@PAYOR_ID", oD.PAYOR_ID)
                        cmd.Parameters.AddWithValue("@Vendor_name", oD.Vendor_name)
                        ''  cmd.Parameters.AddWithValue("@Log_EDI", oD.Log_EDI)


                        cmd.Parameters.AddWithValue("@batch_id", oD._BatchID)
                        cmd.Parameters("@batch_id").Direction = ParameterDirection.InputOutput

                        '// get this black working
                        '*****************************************************************
                        ''  cmd.Parameters.Add("@Reject_Reason_code", Data.SqlDbType.VarChar, 10)
                        ''  cmd.Parameters("@Reject_Reason_code").Direction = ParameterDirection.Output


                        ''   cmd.Parameters.Add("@LOOP_AGAIN", Data.SqlDbType.VarChar, 1)
                        ''    cmd.Parameters("@LOOP_AGAIN").Direction = ParameterDirection.Output
                        '*************************************************************************
                        err = cmd.ExecuteNonQuery()

                        '' get thses working
                        '***************************************************************************
                        '  oD.Status = cmd.Parameters("@Status").Value.ToString()
                        '   oD.RejectReasonCode = cmd.Parameters("@Reject_Reason_code").Value.ToString()
                        '  oD.LoopAgain = cmd.Parameters("@LOOP_AGAIN").Value.ToString()
                        '****************************************************************************

                        oD.rBatchId = cmd.Parameters("@batch_id").Value


                    End Using
                    con.Close()
                    i = 1
                End Using






            Catch ex As Exception
                i = -1

                log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import276", ex)

            Finally





            End Try
            ISA.Clear()
            GS.Clear()
            UNK.Clear()
            ST.Clear()
            BHT.Clear()
            HL.Clear()
            DTP.Clear()
            DMG.Clear()
            REF.Clear()
            NM1.Clear()
            AMT.Clear()
            REF.Clear()
            NM1.Clear()
            AMT.Clear()

            Return i

        End Function

        Private Function ComitISA() As Integer


            Dim i As Integer
            Dim param As New SqlParameter()


            Dim sqlString As String

            Try


                sqlString = "usp_EDI_276_ISA"

                Using Con As New SqlConnection(oD._ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)

                        cmd.CommandType = CommandType.StoredProcedure


                        cmd.Parameters.AddWithValue("@HIPAA_276_ISA", ISA)
                        cmd.Parameters.AddWithValue("@FILE_ID", _FileID)
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


                sqlString = "usp_EDI_276_GS"

                Using Con As New SqlConnection(oD._ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)

                        cmd.CommandType = CommandType.StoredProcedure


                        cmd.Parameters.AddWithValue("@HIPAA_276_GS", GS)
                        cmd.Parameters.AddWithValue("@FILE_ID", _FileID)
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


        Private Function ComitISAGS() As Integer


            Dim i As Integer
            Dim param As New SqlParameter()


            Dim sqlString As String

            Try


                sqlString = "usp_EDI_276_ISA_GS"

                Using Con As New SqlConnection(oD._ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)

                        cmd.CommandType = CommandType.StoredProcedure


                        cmd.Parameters.AddWithValue("@HIPAA_276_ISA", ISA)
                        cmd.Parameters.AddWithValue("@HIPAA_276_GS", GS)
                        cmd.Parameters.AddWithValue("@BATCH_ID", oD.rBatchId)
                        ''   cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using




                i = 0

            Catch ex As Exception
                i = -1


                log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import276.COMITISAGS", ex)

            Finally

                'sqlConn.Close()
            End Try

            Return i

        End Function


        Private Function COMITUNK() As Integer


            Dim i As Integer
            Dim param As New SqlParameter()


            Dim sqlString As String


            Try

                sqlString = "usp_EDI_276_UNK"

                Using Con As New SqlConnection(oD._ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)

                        cmd.CommandType = CommandType.StoredProcedure



                        cmd.Parameters.AddWithValue("@HIPAA_276_UNK", UNK)
                        cmd.Parameters.AddWithValue("@FILE_ID", oD._FileID)
                        '     cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using


                i = 0

            Catch ex As Exception
                i = -1

                log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import276.rollback for bactch ID " + oD._BatchID.ToString, ex)

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


                sqlString = "usp_EDI_276_ROLLBACK"


                Using Con As New SqlConnection(oD._ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)

                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.AddWithValue("@FILE_ID", oD._FileID)
                        '  cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using

                i = 0

            Catch ex As Exception
                i = -1

                log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import276.rollback for bactch ID " + oD._BatchID.ToString, ex)

            Finally

            End Try

            Return i


        End Function


    End Class
End Namespace
