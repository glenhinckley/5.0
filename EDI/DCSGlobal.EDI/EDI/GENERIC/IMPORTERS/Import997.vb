Option Explicit On
Option Strict On
Option Compare Binary


Imports DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff
Imports System.Text.RegularExpressions
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Collections.Generic
'Imports System.Data.DataSetExtensions

Imports System.IO
Imports System.IO.File
Imports System.Text
Imports System.Threading

Imports DCSGlobal.BusinessRules.Logging


Namespace DCSGlobal.EDI







    Public Class Import997

        Inherits EDI997Tables

        Implements IDisposable

        ' Keep track of when the object is disposed. 
        Protected disposed As Boolean = False
        Dim _ROW_NUM As Integer
        Dim _isFile As Boolean




        ' This method disposes the base object's resources. 
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposed Then
                If disposing Then


                    log = Nothing
                    'email = Nothing
                    ' Insert code to free managed resources. 
                End If
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






        Private log As New logExecption
        Private objss As New StringStuff
        Private oD As New Declarations
        Private _ConnectionString As String = String.Empty
        Dim _BATCH_ID As Integer





        Private _ClassVersion As String = "2.0"
        Private start_time As DateTime
        Private stop_time As DateTime
        Private elapsed_time As TimeSpan


        Private _TablesBuilt As Boolean = False
        Private _ENFlag As Boolean = False

        Private err As String = ""
        Private _chars As String
        Private STFlag As Integer = 0
        'Private LXFlag As Integer = 0
        'Private CLPFlag As Integer = 0
        Private FileID As Int32 = 0

        Private LXList As New List(Of String)


        Private _HSDCount As Integer
        Private _Debug As Integer = 0
        Private V As Boolean = False

        Private _FileToParse As String
        Private _CLPString As String

        Private _EDIList As New List(Of String)


        Private HL_PARENT As String



        Dim _FileID As Integer = 200
        Dim _BatchID As Double






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
        Public WriteOnly Property isFile As Boolean


            Set(value As Boolean)
                _isFile = value
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



        Public ReadOnly Property ErrorMessage As String
            Get
                Return err
            End Get

        End Property








        Public Function Import(ByVal EDIList As List(Of String), ByVal BatchID As Double) As Int32

            Dim x As Integer = -1
            _EDIList = EDIList
            _BatchID = BatchID

            x = Import997()

            Return x

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

                    r = Import997()

                End If
            End If


            Return r

        End Function

        Private Function Import997() As Integer
            V = True


            oD.edi_type = "997"

            oD.HL20Flag = 0
            oD.DataElementSeparatorFlag = 0
            _ROW_NUM = 0

            Dim [end] As DateTime
            Dim start As DateTime = DateTime.Now

            If _isFile = True Then
                ''    oD._FileID = InsertFileName(_FileToParse)

            Else

                oD._FileID = 0

            End If
            Dim rr As Integer = 0


            '     Try

            start_time = Now
            oD.TimeStamp = CDate(FormatDateTime(start_time, DateFormat.ShortDate))



            oD.IEAFlag = CInt(False)


            If V Then
                log.ExceptionDetails("DCSGlobal.EDI.Import997.Import", "Pasring list")
                log.ExceptionDetails("DCSGlobal.EDI.Import997.Import", "### Overall Start List Time: " + start.ToLongTimeString())
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
                AK1.Clear()
                AK2.Clear()
                AK3.Clear()
                AK4.Clear()
                AK5.Clear()
                AK9.Clear()

            End If

            Dim last As String = String.Empty
            '  Dim line As String = String.Empty
            Dim rowcount As Int32 = 0

            'so lets get down to it opn the file in a stream reader and move thru it one line at a time...........

            '  Using r As New StreamReader(FileToParse)
            oD.RowProcessedFlag = 0
            'line = r.ReadLine()
            'Console.WriteLine(line)


            'Console.WriteLine(oD.DataElementSeparator)

            '  Try

            For Each line As String In _EDIList


                _ROW_NUM = _ROW_NUM + 1

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

                '  line = r.ReadLine


                'check for LX

                'If objss.ParseDemlimtedStringEDI(line, oD.DataElementSeparator, 1) = "LX" Then
                '    LXFlag = 1
                'End If




                ' ******************************************************************************************************************************


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
                    ISARow("ROW_NUM") = _ROW_NUM
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
                    GSRow("ROW_NUM") = _ROW_NUM
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

                    ' ComitISAGS()

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
                    STRow("ROW_NUM") = _ROW_NUM
                    STRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    STRow("HIPAA_GS_GUID") = oD.GS_GUID
                    STRow("HIPAA_ST_GUID") = oD.ST_GUID
                    STRow("ST01") = oD.ST01
                    STRow("ST02") = oD.ST02
                    STRow("ST03") = oD.ST03
                    ST.Rows.Add(STRow)

                    oD.ST_ROW_ID = rowcount



                End If

                ' *********************************************************************************************************



                ' begin st
                If oD.ediRowRecordType = "AK1" Then
                    oD.RowProcessedFlag = 1

          

        

                    Dim AK1Row As DataRow = AK1.NewRow
                    AK1Row("ROW_NUM") = _ROW_NUM
                    AK1Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                    AK1Row("HIPAA_GS_GUID") = oD.GS_GUID
                    AK1Row("HIPAA_ST_GUID") = oD.ST_GUID
 





                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then AK1Row("AK101") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else AK1Row("AK101") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then AK1Row("AK102") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else AK1Row("AK102") = DBNull.Value

                    AK1.Rows.Add(AK1Row)





                End If




                If oD.ediRowRecordType = "AK2" Then
                    oD.RowProcessedFlag = 1





                    Dim AK2Row As DataRow = AK2.NewRow
                    AK2Row("ROW_NUM") = _ROW_NUM
                    AK2Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                    AK2Row("HIPAA_GS_GUID") = oD.GS_GUID
                    AK2Row("HIPAA_ST_GUID") = oD.ST_GUID






                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then AK2Row("AK201") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else AK2Row("AK201") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then AK2Row("AK202") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else AK2Row("AK202") = DBNull.Value

                    AK2.Rows.Add(AK2Row)





                End If





                If oD.ediRowRecordType = "AK3" Then
                    oD.RowProcessedFlag = 1





                    Dim AK3Row As DataRow = AK3.NewRow
                    AK3Row("ROW_NUM") = _ROW_NUM
                    AK3Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                    AK3Row("HIPAA_GS_GUID") = oD.GS_GUID
                    AK3Row("HIPAA_ST_GUID") = oD.ST_GUID






                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then AK3Row("AK301") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else AK3Row("AK301") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then AK3Row("AK302") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else AK3Row("AK302") = DBNull.Value

                    AK3.Rows.Add(AK3Row)





                End If



                If oD.ediRowRecordType = "AK4" Then
                    oD.RowProcessedFlag = 1





                    Dim AK4Row As DataRow = AK4.NewRow
                    AK4Row("ROW_NUM") = _ROW_NUM
                    AK4Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                    AK4Row("HIPAA_GS_GUID") = oD.GS_GUID
                    AK4Row("HIPAA_ST_GUID") = oD.ST_GUID






                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then AK4Row("AK401") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else AK4Row("AK401") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then AK4Row("AK402") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else AK4Row("AK402") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then AK4Row("AK403") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else AK4Row("AK403") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then AK4Row("AK404") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else AK4Row("AK404") = DBNull.Value




                    AK4.Rows.Add(AK4Row)





                End If





                If oD.ediRowRecordType = "AK5" Then
                    oD.RowProcessedFlag = 1





                    Dim AK5Row As DataRow = AK5.NewRow
                    AK5Row("ROW_NUM") = _ROW_NUM
                    AK5Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                    AK5Row("HIPAA_GS_GUID") = oD.GS_GUID
                    AK5Row("HIPAA_ST_GUID") = oD.ST_GUID






                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then AK5Row("AK501") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else AK5Row("AK501") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then AK5Row("AK502") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else AK5Row("AK502") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then AK5Row("AK503") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else AK5Row("AK503") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then AK5Row("AK504") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else AK5Row("AK504") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then AK5Row("AK505") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else AK5Row("AK505") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then AK5Row("AK506") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else AK5Row("AK506") = DBNull.Value




                    AK5.Rows.Add(AK5Row)





                End If



                If oD.ediRowRecordType = "AK9" Then
                    oD.RowProcessedFlag = 1





                    Dim AK9Row As DataRow = AK9.NewRow
                    AK9Row("ROW_NUM") = _ROW_NUM
                    AK9Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                    AK9Row("HIPAA_GS_GUID") = oD.GS_GUID
                    AK9Row("HIPAA_ST_GUID") = oD.ST_GUID






                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then AK9Row("AK901") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else AK9Row("AK901") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then AK9Row("AK902") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else AK9Row("AK902") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then AK9Row("AK903") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else AK9Row("AK903") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then AK9Row("AK904") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else AK9Row("AK904") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then AK9Row("AK905") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else AK9Row("AK905") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then AK9Row("AK906") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else AK9Row("AK906") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then AK9Row("AK907") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else AK9Row("AK907") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then AK9Row("AK908") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else AK9Row("AK908") = DBNull.Value
                    If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then AK9Row("AK909") = objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else AK9Row("AK909") = DBNull.Value




                    AK9.Rows.Add(AK9Row)





                End If


                '****************************************************************************************************************





                If oD.RowProcessedFlag = 0 Then
                    Dim UNKRow As DataRow = UNK.NewRow
                    UNKRow("ROW_NUM") = _ROW_NUM
                    UNKRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    UNKRow("HIPAA_GS_GUID") = oD.GS_GUID
                    UNKRow("HIPAA_ST_GUID") = oD.ST_GUID
                    UNKRow("HIPAA_HL_GUID") = oD.HL_GUID
                    UNKRow("ROW_RECORD_TYPE") = oD.ediRowRecordType
                    UNKRow("ROW_DATA") = oD.CurrentRowData

                    UNK.Rows.Add(UNKRow)


                End If






                If oD.ediRowRecordType = "SE" Then


                    ' COMMIT THE LAST LK SET SINCE WE WONT GET TO lx AGAIN AND THE LAST clp SET
                    'ComitLX()
                    ComitALL()

                    '    ComitST()
                    '  ST.Clear()

                    oD.HL20Flag = 0

                    oD.RowProcessedFlag = 1
                    oD.EBFlag = 0

                End If


                If oD.ediRowRecordType = "GE" Then
                    oD.RowProcessedFlag = 1

                    'committ the last ST
                    ComitGS()
                    GS.Clear()
                    oD.GSFlag = 0
                    oD.GS_GUID = Guid.Empty


                End If



                If oD.ediRowRecordType = "IEA" Then
                    oD.ISA_GUID = Guid.Empty
                    oD.IEAFlag = 1
                    oD.ISAFlag = 0

                    ComitISA()

                End If


                '***********************************************************************************************************************
                'end cut
                '***********************************************************************************************************************

            Next

            'Console.WriteLine(last)
            'Console.WriteLine(rowcount)

            'Catch ex As Exception
            '    rr = 30
            '    oD.IEAFlag = 1
            '    RollBack()
            '    log.ExceptionDetails(oD._BatchID.ToString, "DCSGlobal.EDI.Import837I was rolled back due to Error in main loop " + oD.ediRowRecordType + "Rowcount " + Convert.ToString(rowcount))
            '    log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import837 error in main loop" + FileToParse, ex)

            'Finally
            'COMITUNK()

            'End Try


            If oD.IEAFlag = 0 Then
                rr = -40
                RollBack()
                log.ExceptionDetails(oD._BatchID.ToString, "DCSGlobal.EDI.Import997was LIST rolled back due to IEA Not Found")

            End If


            If V Then
                [end] = DateTime.Now
                log.ExceptionDetails("DCSGlobal.EDI.Import997.Import", "### Overall LISt End Time: " + [end].ToLongTimeString())
                log.ExceptionDetails("DCSGlobal.EDI.Import997.Import", "### Overall Lest Run Time: " + ([end] - start).ToString())
            End If


            'Return rr
            If rr > 0 Then
                Return oD.rBatchId
            Else
                Return rr
            End If
        End Function

        Private Function InsertFileName(ByVal FilePath As String, ByVal FileName As String) As Int32


            Dim rr As Integer = -1

            Dim sqlString As String
            sqlString = "usp_EDI_997_ADD_FILE"

            Try

                Using Con As New SqlConnection(oD._ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@FILE_NAME", SqlDbType.VarChar, 255).Value = FileName
                        cmd.Parameters.Add("@FILE_PATH", SqlDbType.VarChar).Value = FilePath
                        cmd.Parameters.Add("@EDI_TYPE", SqlDbType.VarChar, 50).Value = "997"

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


            Return oD.rBatchId


            'Return rr

        End Function

        Private Function ComitALL() As Integer


            Dim i As Integer = -1



            Dim sqlString As String
            sqlString = "usp_claim_status_response_dump_997"

            Try



                Using Con As New SqlConnection(oD._ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)

                        cmd.CommandType = CommandType.StoredProcedure

                        '  cmd.Parameters.AddWithValue("@HIPAA_997_ISA", ISA)
                        '  cmd.Parameters.AddWithValue("@HIPAA_997_GS", GS)

                        cmd.Parameters.AddWithValue("@HIPAA_997_ST", ST)

                        ' cmd.Parameters.AddWithValue("@HIPAA_997_UNK", UNK)



                        cmd.Parameters.AddWithValue("@FILE_ID", _FileID)

                        cmd.Parameters.AddWithValue("@DELETE_FLAG", oD.DeleteFlag)
                        cmd.Parameters.AddWithValue("@cbr_id", oD.ebr_id)
                        ' cmd.Parameters.AddWithValue("@batch_id", oD._BatchID)
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


                        cmd.Parameters.AddWithValue("@batch_id", oD._BatchID)
                        cmd.Parameters("@batch_id").Direction = ParameterDirection.InputOutput



                        i = cmd.ExecuteNonQuery()

                        '  oD.rBatchId = cmd.Parameters("@batch_id").Value

                        'oD.Status = cmd.Parameters("@Status").Value.ToString()
                        'oD.RejectReasonCode = cmd.Parameters("@Reject_Reason_code").Value.ToString()
                        'oD.LoopAgain = cmd.Parameters("@LOOP_AGAIN").Value.ToString()


                        i = 0
                    End Using
                    Con.Close()
                End Using

            Catch ex As Exception
                i = -1

                log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import997ALL", ex)

            Finally





            End Try



            ST.Clear()
     




            Return i

        End Function


        Private Function ComitISA() As Integer


            Dim i As Integer
            Dim param As New SqlParameter()


            Dim sqlString As String

            Try


                sqlString = "usp_EDI_997_ISA"

                Using Con As New SqlConnection(oD._ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)

                        cmd.CommandType = CommandType.StoredProcedure


                        cmd.Parameters.AddWithValue("@HIPAA_997_ISA", ISA)
                        cmd.Parameters.AddWithValue("@FILE_ID", _FileID)
                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using




                i = 0

            Catch ex As Exception
                i = -1


                log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import997.COMITISAGS", ex)

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


                sqlString = "usp_EDI_997_GS"

                Using Con As New SqlConnection(oD._ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)

                        cmd.CommandType = CommandType.StoredProcedure


                        cmd.Parameters.AddWithValue("@HIPAA_997_GS", GS)
                        cmd.Parameters.AddWithValue("@FILE_ID", _FileID)
                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using




                i = 0

            Catch ex As Exception
                i = -1


                log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import997.COMITISAGS", ex)

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


                sqlString = "usp_EDI_997_ROLLBACK"


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

                log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import997.rollback for bactch ID " + oD._BatchID.ToString, ex)

            Finally

            End Try

            Return i


        End Function











    End Class
End Namespace