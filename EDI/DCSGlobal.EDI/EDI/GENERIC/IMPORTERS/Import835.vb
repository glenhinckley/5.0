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

    Public Class Import835

        Inherits EDI835Tables


        Implements IDisposable



        Protected disposed As Boolean = False





        Private oD As New Declarations
        Private ss As New StringStuff

        Private _EDIFileData As New EDIFile
        Private _ClassVersion As String = "3.0"





        Private _ImportReturnCode As Integer = 0

        Private _InputFolder As String = String.Empty
        Private _FailedFolder As String = String.Empty
        Private _SuccessFolder As String = String.Empty
        Private _DuplicateFolder As String = String.Empty


        Private _ProcessElaspedTime As Long
        Private _ProcessStartTime As DateTime
        Private _ProcessEndTime As DateTime

        'Private _ClassVersion As String = "2.0"
        'Private start_time As DateTime
        'Private stop_time As DateTime

        Private _TablesBuilt As Boolean = False
        Private _ENFlag As Boolean = False
        Private distinctDT As DataTable
        Private err As String = ""
        Private _chars As String
        Private STFlag As Integer = 0
        Private LXFlag As Integer = 0
        Private CLPFlag As Integer = 0
        Private FileID As Int32 = 0

        Private LXList As New List(Of String)
        Private _FileName As String = String.Empty
        Private _FilePath As String = String.Empty
        Private _FileToParse As String
        Private _isValid As Integer = -1




        Private _RowCount As Integer = 0

        Private _SchedulerLogID As Integer = 0


        Private log As New logExecption
        Private _HSDCount As Integer
        Private _Debug = 0
        Private _Verbose As Boolean = False


        Dim _CLPString As String

        Dim SVC01 As String = String.Empty
        Dim SVC06 As String = String.Empty


        Dim RAW_SOURCE As String = String.Empty



        Dim _ParseTree As Integer = 0

        '       @EDI varchar(max)='',

        '@PAYOR_ID varchar(50)='',

        '@Vendor_name varchar(50)='',

        '@Log_EDI varchar(1)='N'



        Public meddd As IEnumerable
        Dim mmm As Integer = 1
        Dim lxline As String
        Dim PLB03 As String
        Dim PLB05 As String
        Dim PLB07 As String
        Dim PLB09 As String
        Dim PLB11 As String
        Dim PLB13 As String

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

                ss = Nothing
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

        Public WriteOnly Property SchedulerLogID As Integer

            Set(value As Integer)
                _SchedulerLogID = value
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

        Public WriteOnly Property Folder As String

            Set(value As String)
                oD.Folder = value
            End Set
        End Property


        Public WriteOnly Property Log_EDI As String

            Set(value As String)
                oD.Log_EDI = value
            End Set
        End Property



        Public Property InputFolder As String
            Get
                Return _InputFolder

            End Get
            Set(ByVal value As String)


                If (ss.GetLastChr(value) <> "\") Then
                    value = value + "\"
                End If
                _InputFolder = value

            End Set
        End Property


        Public Property FailedFolder As String
            Get
                Return _FailedFolder

            End Get
            Set(ByVal value As String)

                If (ss.GetLastChr(value) <> "\") Then
                    value = value + "\"
                End If
                _FailedFolder = value


            End Set
        End Property



        Public Property SuccessFolder As String
            Get
                Return _SuccessFolder

            End Get
            Set(ByVal value As String)

                If (ss.GetLastChr(value) <> "\") Then
                    value = value + "\"
                End If
                _SuccessFolder = value

            End Set
        End Property


        Public Property DuplicateFolder As String
            Get
                Return _DuplicateFolder

            End Get
            Set(ByVal value As String)

                If (ss.GetLastChr(value) <> "\") Then
                    value = value + "\"
                End If
                _DuplicateFolder = value

            End Set
        End Property




        Public WriteOnly Property DeleteFlag As String

            Set(value As String)
                oD.DeleteFlag = value
            End Set
        End Property
        Public WriteOnly Property Verbose As Integer

            Set(value As Integer)
                _Verbose = value
            End Set
        End Property



        '  _ParseTree

        Public WriteOnly Property ParseTree As Integer

            Set(value As Integer)
                oD.ParseTree = value
            End Set
        End Property

        Public WriteOnly Property Dump As Integer

            Set(value As Integer)
                oD.Dump = value
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

        Public Property EDIFileData As EDIFile
            Get
                Return _EDIFileData


            End Get
            Set(value As EDIFile)


                _EDIFileData = value

            End Set
        End Property

        Public Function Import(ByVal FilePath As String, ByVal FileName As String) As Int32
            Dim r As Integer = -1

            _FileName = FileName
            _FilePath = FilePath

            r = Import(FilePath + FileName)
            Return r
        End Function


        Public Function Import(ByVal FileToParse As String) As Int32


            _FileToParse = FileToParse
            _ProcessStartTime = Now



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








            'oD._FileID = InsertFileName(_FileToParse)

            'oD._FileID = InsertFileName(_FileToParse, sFolderName)

            'If _EDIFileData.FileID > 0 Then


            'Else

            '    Dim 

            '    _EDIFileData.ORIGINAL_FILE_NAME = _FileToParse
            '    _EDIFileData.ORGINAL_PATH = sFolderName






            'End If



            'If oD._FileID < 0 Then
            '    Return -9999
            '    Exit Function
            'End If




            '     Try

            '  start_time = Now
            '   oD.TimeStamp = FormatDateTime(start_time, DateFormat.ShortDate)



            oD.IEAFlag = False




            If _Verbose Then
                log.ExceptionDetails("DCSGlobal.EDI.Import835.Import", "Pasring File : " + FileToParse)
                log.ExceptionDetails("DCSGlobal.EDI.Import835.Import", "### Overall Start Time: " + _ProcessStartTime.ToLongTimeString() + " " + FileToParse)
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



            If Not File.Exists(FileToParse) Then
                log.ExceptionDetails("DCSGlobal.EDI.Import835.Import", FileToParse + " Does Not Exist ")
                Return 1
                Exit Function
            End If



            ' GetType all the  goodies out of the file

            Using Val As New ValidateEDI
                Val.ConnectionString = oD._ConnectionString
                _ImportReturnCode = Val.byFile(_FilePath, _FileName, "835")

                If _ImportReturnCode = 0 Then
                    _isValid = 1




                    _EDIFileData.ConnectionString = oD._ConnectionString
                    _EDIFileData.SchedulerLogID = _SchedulerLogID
                    _EDIFileData.OriginalFileName = _FileName
                    _EDIFileData.OriginalPath = _FilePath
                    _EDIFileData.InterchangeSenderID = Val.InterchangeSenderID
                    _EDIFileData.InterchangeReceiverID = Val.InterchangeReceiverID
                    _EDIFileData.InterchangeDate = Val.InterchangeDate
                    _EDIFileData.InterchangeTime = Val.InterchangeTime
                    _EDIFileData.InterchangeControlNumber = Val.InterchangeControlNumber




                    _EDIFileData.TransactionSetIdentifierCode = Val.TransactionSetIdentifierCode

                    oD._FileID = _EDIFileData.InsertEDIFile

                    If _EDIFileData.isDuplicate > 0 Then
                        _ImportReturnCode = _EDIFileData.FileID
                    End If

                Else
                    _isValid = 0

                    _EDIFileData.ConnectionString = oD._ConnectionString
                    _EDIFileData.SchedulerLogID = _SchedulerLogID
                    _EDIFileData.OriginalFileName = _FileName
                    _EDIFileData.OriginalPath = _FilePath
                    _EDIFileData.InterchangeSenderID = Val.InterchangeSenderID
                    _EDIFileData.InterchangeReceiverID = Val.InterchangeReceiverID
                    _EDIFileData.InterchangeDate = Val.InterchangeDate
                    _EDIFileData.InterchangeTime = Val.InterchangeTime
                    _EDIFileData.InterchangeControlNumber = Val.InterchangeControlNumber




                    _EDIFileData.TransactionSetIdentifierCode = Val.TransactionSetIdentifierCode

                    oD._FileID = _EDIFileData.InsertEDIFile

                End If




            End Using



            If _TablesBuilt = False Then
                BuildTables()
                _TablesBuilt = True
            Else

                ISA.Clear()
                GS.Clear()
                ST.Clear()
                BHT.Clear()
                BPR.Clear()
                NM1.Clear()
                N1.Clear()
                N3.Clear()
                N4.Clear()
                TRN.Clear()
                PER.Clear()
                LX.Clear()
                LQ.Clear()
                TS2.Clear()
                TS3.Clear()
                REF.Clear()
                QTY.Clear()
                CLP.Clear()
                CAS.Clear()
                SVC.Clear()
                AMT.Clear()
                DTM.Clear()
                MIA.Clear()
                MOA.Clear()
                UNK.Clear()
                PLB.Clear()

            End If



            Dim last As String = String.Empty
            Dim line As String = String.Empty


            'so lets get down to it opn the file in a stream reader and move thru it one line at a time...........
            If _ImportReturnCode = 0 Then


                Using r As New StreamReader(FileToParse)
                    oD.RowProcessedFlag = 0
                    line = r.ReadLine()
                    'Console.WriteLine(line)
                    oD.DataElementSeparator = Mid(line, 4, 1)



                    'Console.WriteLine(oD.DataElementSeparator)

                    Try
                        Do While (Not line Is Nothing)
                            ' Add this line to list.
                            last = line
                            ' Display to console.
                            ' 
                            ' Read in the next line.
                            If _Verbose Then

                                RAW_SOURCE = RAW_SOURCE + line
                            End If
                            line = line.Replace("~", "")

                            _RowCount = _RowCount + 1
                            oD.ediRowRecordType = ss.ParseDemlimtedStringEDI(line, oD.DataElementSeparator, 1)
                            'Console.WriteLine(oD.ediRowRecordType)
                            oD.CurrentRowData = line



                            line = r.ReadLine





                            'check for LX

                            'If objss.ParseDemlimtedStringEDI(line, oD.DataElementSeparator, 1) = "LX" Then
                            '    LXFlag = 1
                            'End If





                            If oD.ediRowRecordType = "ISA" Then

                                oD.RowProcessedFlag = 1

                                oD.EBCarrotCHAR = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 12)

                                oD.ISA_GUID = Guid.NewGuid
                                oD.P_GUID = oD.ISA_GUID

                                oD.ISA01 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)
                                oD.ISA02 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                                oD.ISA03 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)
                                oD.ISA04 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 5)
                                oD.ISA05 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 6)
                                oD.ISA06 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 7)
                                oD.ISA07 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 8)
                                oD.ISA08 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 9)
                                oD.ISA09 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 10)
                                oD.ISA10 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 11)
                                oD.ISA11 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 12)
                                oD.ISA12 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 13)
                                oD.ISA13 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 14)
                                oD.ISA14 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 15)
                                oD.ISA15 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 16)
                                oD.ISA16 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 17)

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

                                oD.ISA_ROW_ID = _RowCount



                                oD.CarrotDataDelimiter = oD.ISA11
                                oD.ComponentElementSeperator = oD.ISA16


                                _chars = "RowDataDelimiter: " + oD.DataElementSeparator + " CarrotDataDelimiter: " + oD.CarrotDataDelimiter + " ComponentElementSeperator: " + oD.ComponentElementSeperator



                                oD.ISAFlag = 1
                            End If





                            If oD.ediRowRecordType = "GS" Then
                                oD.RowProcessedFlag = 1
                                oD.GS_GUID = Guid.NewGuid
                                oD.P_GUID = oD.GS_GUID

                                oD.GS01 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)
                                oD.GS02 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                                oD.GS03 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)
                                oD.GS04 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 5)
                                oD.GS05 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 6)
                                oD.GS06 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 7)
                                oD.GS07 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 8)
                                oD.GS08 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 9)



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

                                oD.GS_ROW_ID = _RowCount




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



                                oD.ST01 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)
                                oD.ST02 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                                oD.ST03 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)


                                Dim STRow As DataRow = ST.NewRow
                                STRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                                STRow("HIPAA_GS_GUID") = oD.GS_GUID
                                STRow("HIPAA_ST_GUID") = oD.GS_GUID
                                STRow("P_GUID") = oD.GS_GUID
                                STRow("ST01") = oD.ST01
                                STRow("ST02") = oD.ST02
                                STRow("ST03") = oD.ST03
                                STRow("TIME_STAMP") = oD.TimeStamp
                                STRow("BATCH_ID") = oD._BatchID
                                ST.Rows.Add(STRow)

                                oD.ST_ROW_ID = _RowCount



                                oD._STID = ComitST()





                            End If

                            ' all the rows get made in to a string. 




                            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                            '      Begin 835
                            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


                            If oD.ediRowRecordType = "BPR" Then

                                oD.RowProcessedFlag = 1

                                Dim BPRRow As DataRow = BPR.NewRow
                                BPRRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                                BPRRow("HIPAA_GS_GUID") = oD.GS_GUID
                                BPRRow("HIPAA_ST_GUID") = oD.ST_GUID
                                'BPRRow("HIPAA_LX_GUID") = oD.LX_GUID
                                'BPRRow("HIPAA_CLP_GUID") = oD.CLP_GUID
                                'BPRRow("HIPAA_SVC_GUID") = oD.SVC_GUID


                                oD.BPR_GUID = Guid.NewGuid
                                oD.P_GUID = oD.BPR_GUID


                                BPRRow("P_GUID") = oD.P_GUID




                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then BPRRow("BPR01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else BPRRow("BPR01") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then BPRRow("BPR02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else BPRRow("BPR02") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then BPRRow("BPR03") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else BPRRow("BPR03") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then BPRRow("BPR04") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else BPRRow("BPR04") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then BPRRow("BPR05") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else BPRRow("BPR05") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then BPRRow("BPR06") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else BPRRow("BPR06") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then BPRRow("BPR07") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else BPRRow("BPR07") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then BPRRow("BPR08") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else BPRRow("BPR08") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then BPRRow("BPR09") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else BPRRow("BPR09") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) <> "") Then BPRRow("BPR10") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) Else BPRRow("BPR10") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) <> "") Then BPRRow("BPR11") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) Else BPRRow("BPR11") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) <> "") Then BPRRow("BPR12") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) Else BPRRow("BPR12") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) <> "") Then BPRRow("BPR13") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) Else BPRRow("BPR13") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 15) <> "") Then BPRRow("BPR14") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 15) Else BPRRow("BPR14") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 16) <> "") Then BPRRow("BPR15") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 16) Else BPRRow("BPR15") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 17) <> "") Then BPRRow("BPR16") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 17) Else BPRRow("BPR16") = DBNull.Value




                                BPRRow("BATCH_ID") = oD._BatchID
                                BPRRow("TIME_STAMP") = oD.TimeStamp
                                BPR.Rows.Add(BPRRow)




                            End If



                            If oD.ediRowRecordType = "PER" Then

                                oD.RowProcessedFlag = 1

                                Dim PERRow As DataRow = PER.NewRow
                                PERRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                                PERRow("HIPAA_GS_GUID") = oD.GS_GUID
                                PERRow("HIPAA_ST_GUID") = oD.ST_GUID
                                'BPRRow("HIPAA_LX_GUID") = oD.LX_GUID
                                'BPRRow("HIPAA_CLP_GUID") = oD.CLP_GUID
                                'BPRRow("HIPAA_SVC_GUID") = oD.SVC_GUID


                                '    oD.PER_GUID = Guid.NewGuid
                                oD.P_GUID = oD.BPR_GUID


                                PERRow("P_GUID") = oD.P_GUID




                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then PERRow("PER01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else PERRow("PER01") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then PERRow("PER02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else PERRow("PER02") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then PERRow("PER03") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else PERRow("PER03") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then PERRow("PER04") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else PERRow("PER04") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then PERRow("PER05") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else PERRow("PER05") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then PERRow("PER06") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else PERRow("PER06") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then PERRow("PER07") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else PERRow("PER07") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then PERRow("PER08") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else PERRow("PER08") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then PERRow("PER09") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else PERRow("PER09") = DBNull.Value


                                PERRow("BATCH_ID") = oD._BatchID
                                PERRow("TIME_STAMP") = oD.TimeStamp
                                PER.Rows.Add(PERRow)




                            End If


                            If oD.ediRowRecordType = "TRN" Then
                                oD.RowProcessedFlag = 1


                                Dim TRNRow As DataRow = TRN.NewRow
                                TRNRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                                TRNRow("HIPAA_GS_GUID") = oD.GS_GUID
                                TRNRow("HIPAA_ST_GUID") = oD.ST_GUID
                                TRNRow("HIPAA_LX_GUID") = oD.LX_GUID
                                TRNRow("HIPAA_CLP_GUID") = oD.CLP_GUID
                                TRNRow("HIPAA_SVC_GUID") = oD.SVC_GUID

                                TRNRow("P_GUID") = oD.P_GUID


                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then TRNRow("TRN01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else TRNRow("TRN01") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then TRNRow("TRN02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else TRNRow("TRN02") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then TRNRow("TRN03") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else TRNRow("TRN03") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then TRNRow("TRN04") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else TRNRow("TRN04") = DBNull.Value

                                TRNRow("BATCH_ID") = oD._BatchID
                                TRNRow("TIME_STAMP") = oD.TimeStamp
                                TRN.Rows.Add(TRNRow)



                            End If







                            If oD.ediRowRecordType = "N1" Then

                                oD.RowProcessedFlag = 1

                                Dim N1Row As DataRow = N1.NewRow
                                N1Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                                N1Row("HIPAA_GS_GUID") = oD.GS_GUID
                                N1Row("HIPAA_ST_GUID") = oD.ST_GUID
                                N1Row("HIPAA_LX_GUID") = oD.LX_GUID
                                N1Row("HIPAA_CLP_GUID") = oD.CLP_GUID
                                N1Row("HIPAA_SVC_GUID") = oD.SVC_GUID

                                N1Row("P_GUID") = oD.P_GUID


                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then N1Row("N101") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else N1Row("N101") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then N1Row("N102") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else N1Row("N102") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then N1Row("N103") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else N1Row("N103") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then N1Row("N104") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else N1Row("N104") = DBNull.Value



                                N1Row("BATCH_ID") = oD._BatchID
                                N1Row("TIME_STAMP") = oD.TimeStamp
                                N1.Rows.Add(N1Row)




                            End If

                            If oD.ediRowRecordType = "N3" Then

                                oD.RowProcessedFlag = 1

                                Dim N3Row As DataRow = N3.NewRow
                                N3Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                                N3Row("HIPAA_GS_GUID") = oD.GS_GUID
                                N3Row("HIPAA_ST_GUID") = oD.ST_GUID
                                N3Row("HIPAA_LX_GUID") = oD.LX_GUID
                                N3Row("HIPAA_CLP_GUID") = oD.CLP_GUID
                                N3Row("HIPAA_SVC_GUID") = oD.SVC_GUID

                                N3Row("P_GUID") = oD.P_GUID


                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then N3Row("N301") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else N3Row("N301") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then N3Row("N302") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else N3Row("N302") = DBNull.Value




                                N3Row("BATCH_ID") = oD._BatchID
                                N3Row("TIME_STAMP") = oD.TimeStamp
                                N3.Rows.Add(N3Row)



                            End If

                            If oD.ediRowRecordType = "N4" Then

                                oD.RowProcessedFlag = 1

                                Dim N4Row As DataRow = N4.NewRow
                                N4Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                                N4Row("HIPAA_GS_GUID") = oD.GS_GUID
                                N4Row("HIPAA_ST_GUID") = oD.ST_GUID
                                N4Row("HIPAA_LX_GUID") = oD.LX_GUID
                                N4Row("HIPAA_CLP_GUID") = oD.CLP_GUID
                                N4Row("HIPAA_SVC_GUID") = oD.SVC_GUID

                                N4Row("P_GUID") = oD.P_GUID


                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then N4Row("N401") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else N4Row("N401") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then N4Row("N402") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else N4Row("N402") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then N4Row("N403") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else N4Row("N403") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then N4Row("N404") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else N4Row("N404") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then N4Row("N405") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else N4Row("N405") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then N4Row("N406") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else N4Row("N406") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then N4Row("N407") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else N4Row("N407") = DBNull.Value




                                N4Row("BATCH_ID") = oD._BatchID
                                N4Row("TIME_STAMP") = oD.TimeStamp
                                N4.Rows.Add(N4Row)


                            End If






















                            ' ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                            ''    Beggin add the lx to LX list
                            ' ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                            '' set lx flag to 1

                            If oD.ediRowRecordType = "LX" Then

                                If LXFlag = 0 Then
                                    ComitSTHeaders()
                                    LXFlag = 1
                                Else
                                    ComitLX()
                                End If



                                oD.RowProcessedFlag = 1
                                oD.LX_GUID = Guid.NewGuid
                                oD.CLP_GUID = Guid.Empty
                                oD.SVC_GUID = Guid.Empty


                                Dim LXRow As DataRow = LX.NewRow
                                LXRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                                LXRow("HIPAA_GS_GUID") = oD.GS_GUID
                                LXRow("HIPAA_ST_GUID") = oD.ST_GUID
                                LXRow("HIPAA_LX_GUID") = oD.LX_GUID


                                LXRow("P_GUID") = oD.P_GUID
                                oD.P_GUID = oD.LX_GUID

                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then LXRow("LX01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else LXRow("LX01") = DBNull.Value


                                LXRow("BATCH_ID") = oD._BatchID
                                LXRow("TIME_STAMP") = oD.TimeStamp
                                LX.Rows.Add(LXRow)


                            End If



                            If oD.ediRowRecordType = "TS2" Then
                                oD.RowProcessedFlag = 1

                                Dim TS2Row As DataRow = TS2.NewRow
                                TS2Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                                TS2Row("HIPAA_GS_GUID") = oD.GS_GUID
                                TS2Row("HIPAA_ST_GUID") = oD.ST_GUID
                                TS2Row("HIPAA_LX_GUID") = oD.LX_GUID
                                TS2Row("HIPAA_CLP_GUID") = oD.CLP_GUID
                                TS2Row("HIPAA_SVC_GUID") = oD.SVC_GUID

                                TS2Row("P_GUID") = oD.P_GUID


                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then TS2Row("TS201") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else TS2Row("TS201") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then TS2Row("TS202") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else TS2Row("TS202") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then TS2Row("TS203") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else TS2Row("TS203") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then TS2Row("TS204") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else TS2Row("TS204") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then TS2Row("TS205") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else TS2Row("TS205") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then TS2Row("TS206") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else TS2Row("TS206") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then TS2Row("TS207") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else TS2Row("TS207") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then TS2Row("TS208") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else TS2Row("TS208") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then TS2Row("TS209") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else TS2Row("TS209") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) <> "") Then TS2Row("TS210") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) Else TS2Row("TS210") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) <> "") Then TS2Row("TS211") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) Else TS2Row("TS211") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) <> "") Then TS2Row("TS212") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) Else TS2Row("TS212") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) <> "") Then TS2Row("TS213") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) Else TS2Row("TS213") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 15) <> "") Then TS2Row("TS214") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 15) Else TS2Row("TS214") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 16) <> "") Then TS2Row("TS215") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 16) Else TS2Row("TS215") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 17) <> "") Then TS2Row("TS216") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 17) Else TS2Row("TS216") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 18) <> "") Then TS2Row("TS217") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 18) Else TS2Row("TS217") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 19) <> "") Then TS2Row("TS218") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 19) Else TS2Row("TS218") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 20) <> "") Then TS2Row("TS219") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 20) Else TS2Row("TS219") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 21) <> "") Then TS2Row("TS220") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 21) Else TS2Row("TS220") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 22) <> "") Then TS2Row("TS221") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 22) Else TS2Row("TS221") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 23) <> "") Then TS2Row("TS222") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 23) Else TS2Row("TS222") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 24) <> "") Then TS2Row("TS223") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 24) Else TS2Row("TS223") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 25) <> "") Then TS2Row("TS224") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 25) Else TS2Row("TS224") = DBNull.Value


                                TS2Row("BATCH_ID") = oD._BatchID
                                TS2Row("TIME_STAMP") = oD.TimeStamp
                                TS2.Rows.Add(TS2Row)



                            End If









                            If oD.ediRowRecordType = "TS3" Then

                                oD.RowProcessedFlag = 1

                                Dim TS3Row As DataRow = TS3.NewRow
                                TS3Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                                TS3Row("HIPAA_GS_GUID") = oD.GS_GUID
                                TS3Row("HIPAA_ST_GUID") = oD.ST_GUID
                                TS3Row("HIPAA_LX_GUID") = oD.LX_GUID
                                TS3Row("HIPAA_CLP_GUID") = oD.CLP_GUID
                                TS3Row("HIPAA_SVC_GUID") = oD.SVC_GUID

                                TS3Row("P_GUID") = oD.P_GUID


                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then TS3Row("TS301") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else TS3Row("TS301") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then TS3Row("TS302") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else TS3Row("TS302") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then TS3Row("TS303") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else TS3Row("TS303") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then TS3Row("TS304") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else TS3Row("TS304") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then TS3Row("TS305") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else TS3Row("TS305") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then TS3Row("TS306") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else TS3Row("TS306") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then TS3Row("TS307") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else TS3Row("TS307") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then TS3Row("TS308") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else TS3Row("TS308") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then TS3Row("TS309") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else TS3Row("TS309") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) <> "") Then TS3Row("TS310") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) Else TS3Row("TS310") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) <> "") Then TS3Row("TS311") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) Else TS3Row("TS311") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) <> "") Then TS3Row("TS312") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) Else TS3Row("TS312") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) <> "") Then TS3Row("TS313") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) Else TS3Row("TS313") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 15) <> "") Then TS3Row("TS314") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 15) Else TS3Row("TS314") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 16) <> "") Then TS3Row("TS315") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 16) Else TS3Row("TS315") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 17) <> "") Then TS3Row("TS316") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 17) Else TS3Row("TS316") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 18) <> "") Then TS3Row("TS317") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 18) Else TS3Row("TS317") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 19) <> "") Then TS3Row("TS318") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 19) Else TS3Row("TS318") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 20) <> "") Then TS3Row("TS319") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 20) Else TS3Row("TS319") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 21) <> "") Then TS3Row("TS320") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 21) Else TS3Row("TS320") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 22) <> "") Then TS3Row("TS321") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 22) Else TS3Row("TS321") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 23) <> "") Then TS3Row("TS322") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 23) Else TS3Row("TS322") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 24) <> "") Then TS3Row("TS323") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 24) Else TS3Row("TS323") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 25) <> "") Then TS3Row("TS324") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 25) Else TS3Row("TS324") = DBNull.Value

                                TS3Row("BATCH_ID") = oD._BatchID
                                TS3Row("TIME_STAMP") = oD.TimeStamp
                                TS3.Rows.Add(TS3Row)


                            End If





                            If oD.ediRowRecordType = "CLP" Then
                                oD.RowProcessedFlag = 1
                                If CLPFlag = 0 Then
                                    _CLPString = String.Empty

                                    CLPFlag = 1
                                Else
                                    ComitCLP()
                                End If

                                Dim CLPRow As DataRow = CLP.NewRow




                                oD.CLP_GUID = Guid.NewGuid
                                oD.SVC_GUID = Guid.Empty



                                CLPRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                                CLPRow("HIPAA_GS_GUID") = oD.GS_GUID
                                CLPRow("HIPAA_ST_GUID") = oD.ST_GUID
                                CLPRow("HIPAA_LX_GUID") = oD.LX_GUID
                                CLPRow("HIPAA_CLP_GUID") = oD.CLP_GUID
                                CLPRow("HIPAA_SVC_GUID") = oD.SVC_GUID

                                CLPRow("P_GUID") = oD.P_GUID
                                oD.P_GUID = oD.CLP_GUID

                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then CLPRow("CLP01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else CLPRow("CLP01") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then CLPRow("CLP02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else CLPRow("CLP02") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then CLPRow("CLP03") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else CLPRow("CLP03") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then CLPRow("CLP04") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else CLPRow("CLP04") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then CLPRow("CLP05") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else CLPRow("CLP05") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then CLPRow("CLP06") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else CLPRow("CLP06") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then CLPRow("CLP07") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else CLPRow("CLP07") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then CLPRow("CLP08") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else CLPRow("CLP08") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then CLPRow("CLP09") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else CLPRow("CLP09") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) <> "") Then CLPRow("CLP10") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) Else CLPRow("CLP10") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) <> "") Then CLPRow("CLP11") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) Else CLPRow("CLP11") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) <> "") Then CLPRow("CLP12") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) Else CLPRow("CLP12") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) <> "") Then CLPRow("CLP13") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) Else CLPRow("CLP13") = DBNull.Value

                                CLPRow("BATCH_ID") = oD._BatchID
                                CLPRow("TIME_STAMP") = oD.TimeStamp
                                CLP.Rows.Add(CLPRow)



                            End If
                            If _Verbose Then


                                If CLPFlag = 1 Then

                                    If oD.ediRowRecordType <> "LX" Then
                                        _CLPString = _CLPString + oD.CurrentRowData + vbCrLf
                                    End If
                                End If

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
                                SVCRow("HIPAA_LX_GUID") = oD.LX_GUID
                                SVCRow("HIPAA_CLP_GUID") = oD.CLP_GUID
                                SVCRow("HIPAA_SVC_GUID") = oD.SVC_GUID

                                SVCRow("P_GUID") = oD.P_GUID
                                oD.P_GUID = oD.SVC_GUID

                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then
                                    SVCRow("SVC01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2)
                                    SVC01 = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2)
                                Else
                                    SVCRow("SVC01") = DBNull.Value

                                End If


                                If Not SVC01 = String.Empty Then

                                    If (ss.ParseDemlimtedString(SVC01, oD.ComponentElementSeperator, 1) <> "") Then SVCRow("SVC01_1") = ss.ParseDemlimtedString(SVC01, oD.ComponentElementSeperator, 1) Else SVCRow("SVC01_1") = DBNull.Value
                                    If (ss.ParseDemlimtedString(SVC01, oD.ComponentElementSeperator, 2) <> "") Then SVCRow("SVC01_2") = ss.ParseDemlimtedString(SVC01, oD.ComponentElementSeperator, 2) Else SVCRow("SVC01_2") = DBNull.Value
                                    If (ss.ParseDemlimtedString(SVC01, oD.ComponentElementSeperator, 3) <> "") Then SVCRow("SVC01_3") = ss.ParseDemlimtedString(SVC01, oD.ComponentElementSeperator, 3) Else SVCRow("SVC01_3") = DBNull.Value
                                    If (ss.ParseDemlimtedString(SVC01, oD.ComponentElementSeperator, 4) <> "") Then SVCRow("SVC01_4") = ss.ParseDemlimtedString(SVC01, oD.ComponentElementSeperator, 4) Else SVCRow("SVC01_4") = DBNull.Value
                                    If (ss.ParseDemlimtedString(SVC01, oD.ComponentElementSeperator, 5) <> "") Then SVCRow("SVC01_5") = ss.ParseDemlimtedString(SVC01, oD.ComponentElementSeperator, 5) Else SVCRow("SVC01_5") = DBNull.Value
                                    If (ss.ParseDemlimtedString(SVC01, oD.ComponentElementSeperator, 6) <> "") Then SVCRow("SVC01_6") = ss.ParseDemlimtedString(SVC01, oD.ComponentElementSeperator, 6) Else SVCRow("SVC01_6") = DBNull.Value
                                    If (ss.ParseDemlimtedString(SVC01, oD.ComponentElementSeperator, 7) <> "") Then SVCRow("SVC01_7") = ss.ParseDemlimtedString(SVC01, oD.ComponentElementSeperator, 7) Else SVCRow("SVC01_7") = DBNull.Value





                                End If







                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then SVCRow("SVC02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else SVCRow("SVC02") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then SVCRow("SVC03") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else SVCRow("SVC03") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then SVCRow("SVC04") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else SVCRow("SVC04") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then SVCRow("SVC05") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else SVCRow("SVC05") = DBNull.Value




                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then SVCRow("SVC06") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) Else SVCRow("SVC06") = DBNull.Value





                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then
                                    SVCRow("SVC06") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7)
                                    SVC06 = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7)
                                Else
                                    SVCRow("SVC06") = DBNull.Value

                                End If

                                If (ss.ParseDemlimtedString(SVC06, oD.ComponentElementSeperator, 1) <> "") Then SVCRow("SVC06_1") = ss.ParseDemlimtedString(SVC06, oD.ComponentElementSeperator, 1) Else SVCRow("SVC06_1") = DBNull.Value
                                If (ss.ParseDemlimtedString(SVC06, oD.ComponentElementSeperator, 2) <> "") Then SVCRow("SVC06_2") = ss.ParseDemlimtedString(SVC06, oD.ComponentElementSeperator, 2) Else SVCRow("SVC06_2") = DBNull.Value
                                If (ss.ParseDemlimtedString(SVC06, oD.ComponentElementSeperator, 3) <> "") Then SVCRow("SVC06_3") = ss.ParseDemlimtedString(SVC06, oD.ComponentElementSeperator, 3) Else SVCRow("SVC06_3") = DBNull.Value
                                If (ss.ParseDemlimtedString(SVC06, oD.ComponentElementSeperator, 4) <> "") Then SVCRow("SVC06_4") = ss.ParseDemlimtedString(SVC06, oD.ComponentElementSeperator, 4) Else SVCRow("SVC06_4") = DBNull.Value
                                If (ss.ParseDemlimtedString(SVC06, oD.ComponentElementSeperator, 5) <> "") Then SVCRow("SVC06_5") = ss.ParseDemlimtedString(SVC06, oD.ComponentElementSeperator, 5) Else SVCRow("SVC06_5") = DBNull.Value
                                If (ss.ParseDemlimtedString(SVC06, oD.ComponentElementSeperator, 6) <> "") Then SVCRow("SVC06_6") = ss.ParseDemlimtedString(SVC06, oD.ComponentElementSeperator, 6) Else SVCRow("SVC06_6") = DBNull.Value
                                If (ss.ParseDemlimtedString(SVC06, oD.ComponentElementSeperator, 7) <> "") Then SVCRow("SVC06_7") = ss.ParseDemlimtedString(SVC06, oD.ComponentElementSeperator, 7) Else SVCRow("SVC06_7") = DBNull.Value





                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then SVCRow("SVC07") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else SVCRow("SVC07") = DBNull.Value


                                SVCRow("BATCH_ID") = oD._BatchID
                                SVCRow("TIME_STAMP") = oD.TimeStamp
                                SVC.Rows.Add(SVCRow)


                            End If




                            If oD.ediRowRecordType = "CAS" Then

                                oD.RowProcessedFlag = 1

                                Dim CASRow As DataRow = CAS.NewRow
                                CASRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                                CASRow("HIPAA_GS_GUID") = oD.GS_GUID
                                CASRow("HIPAA_ST_GUID") = oD.ST_GUID
                                CASRow("HIPAA_LX_GUID") = oD.LX_GUID
                                CASRow("HIPAA_CLP_GUID") = oD.CLP_GUID
                                CASRow("HIPAA_SVC_GUID") = oD.SVC_GUID

                                CASRow("P_GUID") = oD.P_GUID


                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then CASRow("CAS01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else CASRow("CAS01") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then CASRow("CAS02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else CASRow("CAS02") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then CASRow("CAS03") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else CASRow("CAS03") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then CASRow("CAS04") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else CASRow("CAS04") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then CASRow("CAS05") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else CASRow("CAS05") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then CASRow("CAS06") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else CASRow("CAS06") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then CASRow("CAS07") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else CASRow("CAS07") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then CASRow("CAS08") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else CASRow("CAS08") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then CASRow("CAS09") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else CASRow("CAS09") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) <> "") Then CASRow("CAS10") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) Else CASRow("CAS10") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) <> "") Then CASRow("CAS11") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) Else CASRow("CAS11") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) <> "") Then CASRow("CAS12") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) Else CASRow("CAS12") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) <> "") Then CASRow("CAS13") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) Else CASRow("CAS13") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 15) <> "") Then CASRow("CAS14") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 15) Else CASRow("CAS14") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 16) <> "") Then CASRow("CAS15") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 16) Else CASRow("CAS15") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 17) <> "") Then CASRow("CAS16") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 17) Else CASRow("CAS16") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 18) <> "") Then CASRow("CAS17") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 18) Else CASRow("CAS17") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 19) <> "") Then CASRow("CAS18") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 19) Else CASRow("CAS18") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 20) <> "") Then CASRow("CAS19") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 20) Else CASRow("CAS19") = DBNull.Value


                                CASRow("BATCH_ID") = oD._BatchID
                                CASRow("TIME_STAMP") = oD.TimeStamp
                                CAS.Rows.Add(CASRow)


                            End If



                            If oD.ediRowRecordType = "NM1" Then

                                oD.RowProcessedFlag = 1

                                Dim NM1Row As DataRow = NM1.NewRow
                                NM1Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                                NM1Row("HIPAA_GS_GUID") = oD.GS_GUID
                                NM1Row("HIPAA_ST_GUID") = oD.ST_GUID
                                NM1Row("HIPAA_LX_GUID") = oD.LX_GUID
                                NM1Row("HIPAA_CLP_GUID") = oD.CLP_GUID
                                NM1Row("HIPAA_SVC_GUID") = oD.SVC_GUID

                                ' NM1Row("P_GUID") = oD.P_GUID


                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then NM1Row("NM101") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else NM1Row("NM101") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then NM1Row("NM102") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else NM1Row("NM102") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then NM1Row("NM103") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else NM1Row("NM103") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then NM1Row("NM104") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else NM1Row("NM104") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then NM1Row("NM105") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else NM1Row("NM105") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then NM1Row("NM106") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else NM1Row("NM106") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then NM1Row("NM107") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else NM1Row("NM107") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then NM1Row("NM108") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else NM1Row("NM108") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then NM1Row("NM109") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else NM1Row("NM109") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) <> "") Then NM1Row("NM110") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) Else NM1Row("NM110") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) <> "") Then NM1Row("NM111") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) Else NM1Row("NM111") = DBNull.Value



                                NM1Row("BATCH_ID") = oD._BatchID
                                NM1Row("TIME_STAMP") = oD.TimeStamp
                                NM1.Rows.Add(NM1Row)


                            End If


                            If oD.ediRowRecordType = "REF" Then

                                oD.RowProcessedFlag = 1

                                Dim REFRow As DataRow = REF.NewRow
                                REFRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                                REFRow("HIPAA_GS_GUID") = oD.GS_GUID
                                REFRow("HIPAA_ST_GUID") = oD.ST_GUID
                                REFRow("HIPAA_LX_GUID") = oD.LX_GUID
                                REFRow("HIPAA_CLP_GUID") = oD.CLP_GUID
                                REFRow("HIPAA_SVC_GUID") = oD.SVC_GUID

                                ' REFRow("P_GUID") = oD.P_GUID


                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then REFRow("REF01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else REFRow("REF01") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then REFRow("REF02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else REFRow("REF02") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then REFRow("REF03") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else REFRow("REF03") = DBNull.Value

                                REFRow("BATCH_ID") = oD._BatchID
                                REFRow("TIME_STAMP") = oD.TimeStamp
                                REF.Rows.Add(REFRow)


                            End If

                            If oD.ediRowRecordType = "AMT" Then

                                oD.RowProcessedFlag = 1

                                Dim AMTRow As DataRow = AMT.NewRow
                                AMTRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                                AMTRow("HIPAA_GS_GUID") = oD.GS_GUID
                                AMTRow("HIPAA_ST_GUID") = oD.ST_GUID
                                AMTRow("HIPAA_LX_GUID") = oD.LX_GUID
                                AMTRow("HIPAA_CLP_GUID") = oD.CLP_GUID
                                AMTRow("HIPAA_SVC_GUID") = oD.SVC_GUID
                                AMTRow("P_GUID") = oD.P_GUID





                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then AMTRow("AMT01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else AMTRow("AMT01") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then AMTRow("AMT02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else AMTRow("AMT02") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then AMTRow("AMT03") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else AMTRow("AMT03") = DBNull.Value

                                AMTRow("BATCH_ID") = oD._BatchID
                                AMTRow("TIME_STAMP") = oD.TimeStamp
                                AMT.Rows.Add(AMTRow)



                            End If


                            If oD.ediRowRecordType = "LQ" Then

                                oD.RowProcessedFlag = 1

                                Dim LQRow As DataRow = LQ.NewRow
                                LQRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                                LQRow("HIPAA_GS_GUID") = oD.GS_GUID
                                LQRow("HIPAA_ST_GUID") = oD.ST_GUID
                                LQRow("HIPAA_LX_GUID") = oD.LX_GUID
                                LQRow("HIPAA_CLP_GUID") = oD.CLP_GUID
                                LQRow("HIPAA_SVC_GUID") = oD.SVC_GUID
                                LQRow("P_GUID") = oD.P_GUID





                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then LQRow("LQ01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else LQRow("LQ01") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then LQRow("LQ02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else LQRow("LQ02") = DBNull.Value

                                LQRow("BATCH_ID") = oD._BatchID
                                LQRow("TIME_STAMP") = oD.TimeStamp
                                LQ.Rows.Add(LQRow)



                            End If


                            If oD.ediRowRecordType = "QTY" Then
                                oD.RowProcessedFlag = 1


                                Dim QTYRow As DataRow = QTY.NewRow
                                QTYRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                                QTYRow("HIPAA_GS_GUID") = oD.GS_GUID
                                QTYRow("HIPAA_ST_GUID") = oD.ST_GUID
                                QTYRow("HIPAA_LX_GUID") = oD.LX_GUID
                                QTYRow("HIPAA_CLP_GUID") = oD.CLP_GUID
                                QTYRow("HIPAA_SVC_GUID") = oD.SVC_GUID


                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then QTYRow("QTY01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else QTYRow("QTY01") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then QTYRow("QTY02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else QTYRow("QTY02") = DBNull.Value

                                QTYRow("BATCH_ID") = oD._BatchID
                                QTYRow("TIME_STAMP") = oD.TimeStamp
                                QTY.Rows.Add(QTYRow)



                            End If









                            If oD.ediRowRecordType = "DTM" Then

                                oD.RowProcessedFlag = 1

                                Dim DTMRow As DataRow = DTM.NewRow
                                DTMRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                                DTMRow("HIPAA_GS_GUID") = oD.GS_GUID
                                DTMRow("HIPAA_ST_GUID") = oD.ST_GUID
                                DTMRow("HIPAA_LX_GUID") = oD.LX_GUID
                                DTMRow("HIPAA_CLP_GUID") = oD.CLP_GUID
                                DTMRow("HIPAA_SVC_GUID") = oD.SVC_GUID

                                DTMRow("P_GUID") = oD.P_GUID


                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then DTMRow("DTM01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else DTMRow("DTM01") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then DTMRow("DTM02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else DTMRow("DTM02") = DBNull.Value


                                DTMRow("BATCH_ID") = oD._BatchID
                                DTMRow("TIME_STAMP") = oD.TimeStamp
                                DTM.Rows.Add(DTMRow)


                            End If



                            If oD.ediRowRecordType = "MIA" Then

                                oD.RowProcessedFlag = 1

                                Dim MIARow As DataRow = MIA.NewRow
                                MIARow("HIPAA_ISA_GUID") = oD.ISA_GUID
                                MIARow("HIPAA_GS_GUID") = oD.GS_GUID
                                MIARow("HIPAA_ST_GUID") = oD.ST_GUID
                                MIARow("HIPAA_LX_GUID") = oD.LX_GUID
                                MIARow("HIPAA_CLP_GUID") = oD.CLP_GUID
                                MIARow("HIPAA_SVC_GUID") = oD.SVC_GUID

                                MIARow("P_GUID") = oD.P_GUID


                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then MIARow("MIA01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else MIARow("MIA01") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then MIARow("MIA02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else MIARow("MIA02") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then MIARow("MIA03") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else MIARow("MIA03") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then MIARow("MIA04") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else MIARow("MIA04") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then MIARow("MIA05") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else MIARow("MIA05") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then MIARow("MIA06") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else MIARow("MIA06") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then MIARow("MIA07") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else MIARow("MIA07") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then MIARow("MIA08") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else MIARow("MIA08") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then MIARow("MIA09") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else MIARow("MIA09") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) <> "") Then MIARow("MIA10") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) Else MIARow("MIA10") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) <> "") Then MIARow("MIA11") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) Else MIARow("MIA11") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) <> "") Then MIARow("MIA12") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) Else MIARow("MIA12") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) <> "") Then MIARow("MIA13") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) Else MIARow("MIA13") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 15) <> "") Then MIARow("MIA14") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 15) Else MIARow("MIA14") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 16) <> "") Then MIARow("MIA15") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 16) Else MIARow("MIA15") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 17) <> "") Then MIARow("MIA16") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 17) Else MIARow("MIA16") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 18) <> "") Then MIARow("MIA17") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 18) Else MIARow("MIA17") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 19) <> "") Then MIARow("MIA18") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 19) Else MIARow("MIA18") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 20) <> "") Then MIARow("MIA19") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 20) Else MIARow("MIA19") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 21) <> "") Then MIARow("MIA20") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 21) Else MIARow("MIA20") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 22) <> "") Then MIARow("MIA21") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 22) Else MIARow("MIA21") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 23) <> "") Then MIARow("MIA22") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 23) Else MIARow("MIA22") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 24) <> "") Then MIARow("MIA23") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 24) Else MIARow("MIA23") = DBNull.Value


                                MIARow("BATCH_ID") = oD._BatchID
                                MIARow("TIME_STAMP") = oD.TimeStamp
                                MIA.Rows.Add(MIARow)



                            End If



                            If oD.ediRowRecordType = "MOA" Then
                                oD.RowProcessedFlag = 1
                                Dim MOARow As DataRow = MOA.NewRow
                                MOARow("HIPAA_ISA_GUID") = oD.ISA_GUID
                                MOARow("HIPAA_GS_GUID") = oD.GS_GUID
                                MOARow("HIPAA_ST_GUID") = oD.ST_GUID
                                MOARow("HIPAA_LX_GUID") = oD.LX_GUID
                                MOARow("HIPAA_CLP_GUID") = oD.CLP_GUID
                                MOARow("HIPAA_SVC_GUID") = oD.SVC_GUID

                                MOARow("P_GUID") = oD.P_GUID


                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then MOARow("MOA01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else MOARow("MOA01") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then MOARow("MOA02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else MOARow("MOA02") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then MOARow("MOA03") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else MOARow("MOA03") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then MOARow("MOA04") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else MOARow("MOA04") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then MOARow("MOA05") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else MOARow("MOA05") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then MOARow("MOA06") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else MOARow("MOA06") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then MOARow("MOA07") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else MOARow("MOA07") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then MOARow("MOA08") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else MOARow("MOA08") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then MOARow("MOA09") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else MOARow("MOA09") = DBNull.Value


                                MOARow("BATCH_ID") = oD._BatchID
                                MOARow("TIME_STAMP") = oD.TimeStamp
                                MOA.Rows.Add(MOARow)


                            End If










                            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                            '      END adding LX tot he LX List
                            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''







                            If oD.ediRowRecordType = "PLB" Then

                                oD.RowProcessedFlag = 1


                                Dim PLBRow As DataRow = PLB.NewRow
                                PLBRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                                PLBRow("HIPAA_GS_GUID") = oD.GS_GUID
                                PLBRow("HIPAA_ST_GUID") = oD.ST_GUID
                                'PLBRow("HIPAA_LX_GUID") = oD.LX_GUID
                                'PLBRow("HIPAA_CLP_GUID") = oD.CLP_GUID
                                'PLBRow("HIPAA_SVC_GUID") = oD.SVC_GUID

                                PLBRow("P_GUID") = oD.P_GUID


                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then PLBRow("PLB01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else PLBRow("PLB01") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then PLBRow("PLB02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else PLBRow("PLB02") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then PLBRow("PLB03") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else PLBRow("PLB03") = DBNull.Value


                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then
                                    PLBRow("PLB03") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4)
                                    PLB03 = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4)
                                Else
                                    PLBRow("PLB03") = DBNull.Value

                                End If

                                If (ss.ParseDemlimtedString(PLB03, oD.ComponentElementSeperator, 1) <> "") Then PLBRow("PLB03_1") = ss.ParseDemlimtedString(PLB03, oD.ComponentElementSeperator, 1) Else PLBRow("PLB03_1") = DBNull.Value
                                If (ss.ParseDemlimtedString(PLB03, oD.ComponentElementSeperator, 2) <> "") Then PLBRow("PLB03_2") = ss.ParseDemlimtedString(PLB03, oD.ComponentElementSeperator, 2) Else PLBRow("PLB03_2") = DBNull.Value




                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then PLBRow("PLB04") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else PLBRow("PLB04") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then PLBRow("PLB05") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else PLBRow("PLB05") = DBNull.Value



                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then
                                    PLBRow("PLB05") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6)
                                    PLB05 = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6)
                                Else
                                    PLBRow("PLB05") = DBNull.Value

                                End If


                                If (ss.ParseDemlimtedString(PLB05, oD.ComponentElementSeperator, 1) <> "") Then PLBRow("PLB05_1") = ss.ParseDemlimtedString(PLB05, oD.ComponentElementSeperator, 1) Else PLBRow("PLB05_1") = DBNull.Value
                                If (ss.ParseDemlimtedString(PLB05, oD.ComponentElementSeperator, 2) <> "") Then PLBRow("PLB05_2") = ss.ParseDemlimtedString(PLB05, oD.ComponentElementSeperator, 2) Else PLBRow("PLB05_2") = DBNull.Value


                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then PLBRow("PLB06") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else PLBRow("PLB06") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then PLBRow("PLB07") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else PLBRow("PLB07") = DBNull.Value




                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then
                                    PLBRow("PLB07") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8)
                                    PLB07 = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8)
                                Else
                                    PLBRow("PLB07") = DBNull.Value

                                End If



                                If (ss.ParseDemlimtedString(PLB07, oD.ComponentElementSeperator, 1) <> "") Then PLBRow("PLB07_1") = ss.ParseDemlimtedString(PLB07, oD.ComponentElementSeperator, 1) Else PLBRow("PLB07_1") = DBNull.Value
                                If (ss.ParseDemlimtedString(PLB07, oD.ComponentElementSeperator, 2) <> "") Then PLBRow("PLB07_2") = ss.ParseDemlimtedString(PLB07, oD.ComponentElementSeperator, 2) Else PLBRow("PLB07_2") = DBNull.Value




                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then PLBRow("PLB08") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else PLBRow("PLB08") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then PLBRow("PLB09") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else PLBRow("PLB09") = DBNull.Value


                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then
                                    PLBRow("PLB09") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10)
                                    PLB09 = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10)
                                Else
                                    PLBRow("PLB09") = DBNull.Value

                                End If


                                If (ss.ParseDemlimtedString(PLB09, oD.ComponentElementSeperator, 1) <> "") Then PLBRow("PLB09_1") = ss.ParseDemlimtedString(PLB09, oD.ComponentElementSeperator, 1) Else PLBRow("PLB09_1") = DBNull.Value
                                If (ss.ParseDemlimtedString(PLB09, oD.ComponentElementSeperator, 2) <> "") Then PLBRow("PLB09_2") = ss.ParseDemlimtedString(PLB09, oD.ComponentElementSeperator, 2) Else PLBRow("PLB09_2") = DBNull.Value


                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) <> "") Then PLBRow("PLB10") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) Else PLBRow("PLB10") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) <> "") Then PLBRow("PLB11") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) Else PLBRow("PLB11") = DBNull.Value


                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) <> "") Then
                                    PLBRow("PLB11") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12)
                                    PLB11 = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12)
                                Else
                                    PLBRow("PLB11") = DBNull.Value

                                End If


                                If (ss.ParseDemlimtedString(PLB11, oD.ComponentElementSeperator, 1) <> "") Then PLBRow("PLB11_1") = ss.ParseDemlimtedString(PLB11, oD.ComponentElementSeperator, 1) Else PLBRow("PLB11_1") = DBNull.Value
                                If (ss.ParseDemlimtedString(PLB11, oD.ComponentElementSeperator, 2) <> "") Then PLBRow("PLB11_2") = ss.ParseDemlimtedString(PLB11, oD.ComponentElementSeperator, 2) Else PLBRow("PLB11_2") = DBNull.Value




                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) <> "") Then PLBRow("PLB12") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) Else PLBRow("PLB12") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) <> "") Then PLBRow("PLB13") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) Else PLBRow("PLB13") = DBNull.Value



                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) <> "") Then
                                    PLBRow("PLB13") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14)
                                    PLB13 = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14)
                                Else
                                    PLBRow("PLB13") = DBNull.Value

                                End If


                                If (ss.ParseDemlimtedString(PLB13, oD.ComponentElementSeperator, 1) <> "") Then PLBRow("PLB13_1") = ss.ParseDemlimtedString(PLB13, oD.ComponentElementSeperator, 1) Else PLBRow("PLB13_1") = DBNull.Value
                                If (ss.ParseDemlimtedString(PLB13, oD.ComponentElementSeperator, 2) <> "") Then PLBRow("PLB13_2") = ss.ParseDemlimtedString(PLB13, oD.ComponentElementSeperator, 2) Else PLBRow("PLB13_2") = DBNull.Value


                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 15) <> "") Then PLBRow("PLB14") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 15) Else PLBRow("PLB14") = DBNull.Value

                                PLBRow("BATCH_ID") = oD._BatchID
                                PLBRow("TIME_STAMP") = oD.TimeStamp
                                PLB.Rows.Add(PLBRow)

                                ComitPLB()
                                PLB.Clear()

                            End If





                            If oD.ediRowRecordType = "SE" Then


                                ' COMMIT THE LAST LK SET SINCE WE WONT GET TO lx AGAIN AND THE LAST clp SET
                                ComitLX()
                                ComitCLP()
                                ComitSTHeaders()
                                ST.Clear()


                                oD.RowProcessedFlag = 1
                                oD.EBFlag = 0

                            End If


                            If oD.ediRowRecordType = "GE" Then
                                oD.RowProcessedFlag = 1

                                oD.GSFlag = 0
                                oD.GS_GUID = Guid.Empty


                            End If



                            If oD.ediRowRecordType = "IEA" Then
                                oD.ISA_GUID = Guid.Empty
                                oD.IEAFlag = 1
                                oD.ISAFlag = 0

                            End If




                            If oD.RowProcessedFlag = 0 Then
                                Dim UNKRow As DataRow = UNK.NewRow
                                UNKRow("HIPAA_ISA_GUID") = oD.ISA_GUID
                                UNKRow("HIPAA_GS_GUID") = oD.GS_GUID
                                UNKRow("HIPAA_ST_GUID") = oD.ST_GUID
                                UNKRow("HIPAA_LX_GUID") = oD.LX_GUID
                                UNKRow("HIPAA_CLP_GUID") = oD.CLP_GUID
                                UNKRow("HIPAA_SVC_GUID") = oD.SVC_GUID
                                UNKRow("P_GUID") = oD.P_GUID
                                UNKRow("ediRowRecordType") = oD.ediRowRecordType
                                UNKRow("ROW_DATA") = oD.CurrentRowData
                                UNKRow("BATCH_ID") = oD._BatchID
                                UNK.Rows.Add(UNKRow)




                            End If





                        Loop

                        'Console.WriteLine(last)
                        'Console.WriteLine(rowcount)

                    Catch ex As Exception
                        _ImportReturnCode = -30
                        oD.IEAFlag = 1
                        RollBack()
                        log.ExceptionDetails(oD._BatchID.ToString, "DCSGlobal.EDI.Import835 was rolled back due to Error in main loop " + oD.ediRowRecordType + "Rowcount " + Convert.ToString(_RowCount))
                        log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import835 error in main loop" + FileToParse, ex)



                    Finally
                        COMITUNK()


                        'If _Verbose Then
                        '    UpdateRawSource()
                        'End If


                    End Try



                End Using






                If oD.IEAFlag = False Then
                    _ImportReturnCode = -40
                    RollBack()
                    log.ExceptionDetails(oD._BatchID.ToString, "DCSGlobal.EDI.Import835 was rolled back due to IEA Not Found" + FileToParse)

                End If





                'run clean up
            End If


            _ProcessEndTime = DateTime.Now
            Cleanup()




            Return _ImportReturnCode

        End Function


        Private Function Cleanup() As Int32
            Dim r As String = 0

            Dim FinalPath As String = String.Empty

            Try

                Dim span As TimeSpan

                span = _ProcessEndTime - _ProcessStartTime
                _ProcessElaspedTime = span.TotalMilliseconds

            Catch ex As Exception

            End Try






            Using FM As New FileMove
                FM.Input = _InputFolder
                FM.Success = _SuccessFolder
                FM.Failed = _FailedFolder
                FM.Duplicate = _DuplicateFolder
                FM.ConnectionString = oD._ConnectionString




                _ImportReturnCode = FM.Move(_ImportReturnCode, _FileName)




                FinalPath = FM.FinalPath
            End Using



            Select Case (_ImportReturnCode)





                Case 0

                    _EDIFileData.FileID = oD._FileID
                    _EDIFileData.ProcessEndTime = _ProcessEndTime
                    _EDIFileData.ProcessStartTime = _ProcessStartTime
                    _EDIFileData.ProcessElaspedTime = _ProcessElaspedTime
                    _EDIFileData.FinalPath = FinalPath
                    _EDIFileData.ResultCode = _ImportReturnCode
                    _EDIFileData.RowCount = _RowCount
                    _EDIFileData.isValid = _isValid
                    _EDIFileData.isComplete = 1
                    _EDIFileData.UpdateEDIFile()




                Case Is < 0

                    _EDIFileData.FileID = oD._FileID
                    _EDIFileData.ProcessEndTime = _ProcessEndTime
                    _EDIFileData.ProcessStartTime = _ProcessStartTime
                    _EDIFileData.ProcessElaspedTime = _ProcessElaspedTime
                    _EDIFileData.FinalPath = FinalPath
                    _EDIFileData.ResultCode = _ImportReturnCode
                    _EDIFileData.RowCount = _RowCount
                    _EDIFileData.isValid = _isValid
                    _EDIFileData.isComplete = 1
                    _EDIFileData.UpdateEDIFile()

                Case Is > 0

                    _EDIFileData.FileID = oD._FileID
                    _EDIFileData.ProcessEndTime = _ProcessEndTime
                    _EDIFileData.ProcessStartTime = _ProcessStartTime
                    _EDIFileData.ProcessElaspedTime = _ProcessElaspedTime
                    _EDIFileData.FinalPath = _EDIFileData.FinalPath = FinalPath
                    _EDIFileData.ResultCode = _ImportReturnCode
                    _EDIFileData.RowCount = _RowCount
                    _EDIFileData.isValid = _isValid
                    _EDIFileData.isComplete = 1
                    _EDIFileData.UpdateEDIFile()

                Case Else


            End Select











            ' Clear all the tables
            ISA.Clear()
            GS.Clear()
            ST.Clear()
            BHT.Clear()
            BPR.Clear()
            NM1.Clear()
            N1.Clear()
            N3.Clear()
            N4.Clear()
            TRN.Clear()
            PER.Clear()
            LX.Clear()
            LQ.Clear()
            TS2.Clear()
            TS3.Clear()
            REF.Clear()
            QTY.Clear()
            CLP.Clear()
            CAS.Clear()
            SVC.Clear()
            AMT.Clear()
            DTM.Clear()
            MIA.Clear()
            MOA.Clear()
            UNK.Clear()
            PLB.Clear()


            'reset all the guids

            oD.ISA_GUID = Guid.Empty
            oD.GS_GUID = Guid.Empty
            oD.ST_GUID = Guid.Empty
            oD.LX_GUID = Guid.Empty
            oD.CLP_GUID = Guid.Empty
            oD.SVC_GUID = Guid.Empty
            oD.P_GUID = Guid.Empty

            ' reset all the flags
            STFlag = 0
            LXFlag = 0
            CLPFlag = 0
            FileID = 0


            'reset all the vars
            _HSDCount = 0
            _FileToParse = String.Empty
            _CLPString = String.Empty
            SVC01 = String.Empty
            SVC06 = String.Empty
            RAW_SOURCE = String.Empty
            lxline = String.Empty
            PLB03 = String.Empty
            PLB05 = String.Empty
            PLB07 = String.Empty
            PLB09 = String.Empty
            PLB11 = String.Empty
            ' rowcount = 0
            _chars = String.Empty
            oD.EBCarrotCHAR = String.Empty
            oD.CarrotDataDelimiter = String.Empty
            oD.ComponentElementSeperator = String.Empty
            oD.DataElementSeparator = String.Empty

            oD._BatchID = 0
            oD.ediRowRecordType = String.Empty
            oD.CurrentRowData = String.Empty


            '' move the file



            '' update the file row and save its final state






            Return r







        End Function




        Public Function ComitST() As Int32


            Dim rr As Int32

            Dim param As New SqlParameter()


            Dim sqlString As String
            sqlString = "usp_EDI_835_ST"



            Try



                Using Con As New SqlConnection(oD._ConnectionString)
                    Con.Open()
                    Using Com As New SqlCommand(sqlString, Con)

                        Com.CommandType = CommandType.StoredProcedure
                        Com.Parameters.AddWithValue("@HIPAA_835_ST", ST)
                        Com.Parameters.AddWithValue("@FILE_ID", oD._FileID)

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





            Catch ex As Exception
                rr = -1

                log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.InsertST", ex)

            Finally


            End Try

            BHT.Clear()
            BPR.Clear()
            NM1.Clear()
            N1.Clear()
            N3.Clear()
            N4.Clear()
            TRN.Clear()
            LX.Clear()
            LQ.Clear()
            TS2.Clear()
            TS3.Clear()
            REF.Clear()
            QTY.Clear()
            CLP.Clear()
            CAS.Clear()
            SVC.Clear()
            AMT.Clear()
            DTM.Clear()
            MIA.Clear()
            MOA.Clear()
            UNK.Clear()
            PLB.Clear()



            Return rr

        End Function


        Public Function ComitSTHeaders() As Int32


            Dim rr As Int32

            Dim param As New SqlParameter()


            Dim sqlString As String
            sqlString = "usp_EDI_835_ST_HEADERS"



            Try



                Using Con As New SqlConnection(oD._ConnectionString)
                    Con.Open()
                    Using Com As New SqlCommand(sqlString, Con)

                        Com.CommandType = CommandType.StoredProcedure



                        Com.Parameters.AddWithValue("@HIPAA_835_BPR", BPR)
                        Com.Parameters.AddWithValue("@HIPAA_835_TRN", TRN)
                        Com.Parameters.AddWithValue("@HIPAA_835_N1", N1)
                        Com.Parameters.AddWithValue("@HIPAA_835_N3", N3)
                        Com.Parameters.AddWithValue("@HIPAA_835_N4", N4)
                        Com.Parameters.AddWithValue("@HIPAA_835_DTM", DTM)
                        Com.Parameters.AddWithValue("@HIPAA_835_REF", REF)
                        Com.Parameters.AddWithValue("@HIPAA_835_PER", PER)
                        Com.Parameters.AddWithValue("@ST_ID", oD._STID)

                        Com.Parameters.AddWithValue("@FILE_ID", oD._FileID)
                        Com.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using





            Catch ex As Exception
                rr = -1

                log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.InsertSThEASER", ex)

            Finally


            End Try


            BPR.Clear()
            TRN.Clear()
            N1.Clear()
            N3.Clear()
            N4.Clear()
            DTM.Clear()
            REF.Clear()
            PER.Clear()



            Return rr

        End Function



        Public Function ComitCLP() As Integer


            Dim i As Integer
            Dim param As New SqlParameter()


            Dim sqlConn As SqlConnection = New SqlConnection
            Dim cmd As SqlCommand
            Dim sqlString As String


            sqlConn.ConnectionString = oD._ConnectionString
            sqlConn.Open()

            Try


                sqlString = "usp_EDI_835_CLP"


                cmd = New SqlCommand(sqlString, sqlConn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@HIPAA_835_CLP", CLP)
                cmd.Parameters.AddWithValue("@HIPAA_835_CAS", CAS)
                cmd.Parameters.AddWithValue("@HIPAA_835_SVC", SVC)
                cmd.Parameters.AddWithValue("@HIPAA_835_REF", REF)
                cmd.Parameters.AddWithValue("@HIPAA_835_NM1", NM1)
                cmd.Parameters.AddWithValue("@HIPAA_835_AMT", AMT)
                cmd.Parameters.AddWithValue("@HIPAA_835_DTM", DTM)
                cmd.Parameters.AddWithValue("@HIPAA_835_LQ", LQ)
                cmd.Parameters.AddWithValue("@HIPAA_835_MIA", MIA)
                cmd.Parameters.AddWithValue("@HIPAA_835_MOA", MOA)
                cmd.Parameters.AddWithValue("@HIPAA_835_QTY", QTY)
                cmd.Parameters.AddWithValue("@HIPAA_835_CLP_STRING", _CLPString)
                cmd.Parameters.AddWithValue("@FILE_ID", oD._FileID)
                cmd.Parameters.AddWithValue("@ST_ID", oD._STID)
                err = cmd.ExecuteNonQuery()

                i = 0

            Catch ex As Exception
                i = -1

                log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import835CLP", ex)

            Finally


            End Try

            sqlConn.Close()


            CLP.Clear()
            CAS.Clear()
            REF.Clear()
            SVC.Clear()
            NM1.Clear()
            AMT.Clear()
            DTM.Clear()
            LQ.Clear()
            MIA.Clear()
            MOA.Clear()
            QTY.Clear()
            _CLPString = String.Empty




            Return i

        End Function


        Public Function ComitLX() As Integer


            Dim i As Integer
            Dim param As New SqlParameter()


            Dim sqlConn As SqlConnection = New SqlConnection
            Dim cmd As SqlCommand
            Dim sqlString As String


            sqlConn.ConnectionString = oD._ConnectionString
            sqlConn.Open()

            Try


                sqlString = "usp_EDI_835_LX"


                cmd = New SqlCommand(sqlString, sqlConn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@HIPAA_835_LX", LX)
                cmd.Parameters.AddWithValue("@HIPAA_835_TS2", TS2)
                cmd.Parameters.AddWithValue("@HIPAA_835_TS3", TS3)
                'cmd.Parameters.AddWithValue("@HIPAA_835_CLP", CLP)
                'cmd.Parameters.AddWithValue("@HIPAA_835_CAS", CAS)
                'cmd.Parameters.AddWithValue("@HIPAA_835_REF", REF)
                'cmd.Parameters.AddWithValue("@HIPAA_835_SVC", SVC)
                'cmd.Parameters.AddWithValue("@HIPAA_835_NM1", NM1)
                'cmd.Parameters.AddWithValue("@HIPAA_835_AMT", AMT)
                'cmd.Parameters.AddWithValue("@HIPAA_835_DTM", DTM)
                'cmd.Parameters.AddWithValue("@HIPAA_835_LQ", LQ)
                'cmd.Parameters.AddWithValue("@HIPAA_835_MIA", MIA)
                'cmd.Parameters.AddWithValue("@HIPAA_835_MOA", MOA)


                cmd.Parameters.AddWithValue("@ST_ID", oD._STID)
                cmd.Parameters.AddWithValue("@FILE_ID", oD._FileID)




                err = cmd.ExecuteNonQuery()










                i = 0
                sqlConn.Close()
            Catch ex As Exception
                i = -1

                log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import835LX", ex)

            Finally


            End Try


            LX.Clear()
            TS3.Clear()
            TS2.Clear()

            Return i

        End Function







        Public Function ComitISAGS() As Integer


            Dim i As Integer
            Dim param As New SqlParameter()


            Dim sqlConn As SqlConnection = New SqlConnection
            Dim cmd As SqlCommand
            Dim sqlString As String


            sqlConn.ConnectionString = oD._ConnectionString
            sqlConn.Open()

            Try


                sqlString = "usp_EDI_835_ISA_GS"


                cmd = New SqlCommand(sqlString, sqlConn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@HIPAA_835_ISA", ISA)
                cmd.Parameters.AddWithValue("@HIPAA_835_GS", GS)
                cmd.Parameters.AddWithValue("@FILE_ID", oD._FileID)

                err = cmd.ExecuteNonQuery()


                i = 0

            Catch ex As Exception
                i = -1


                log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import835.COMITISAGS", ex)

            Finally

                sqlConn.Close()
            End Try

            Return i

        End Function

        Public Function ComitPLB() As Integer


            Dim i As Integer
            Dim param As New SqlParameter()


            Dim sqlConn As SqlConnection = New SqlConnection
            Dim cmd As SqlCommand
            Dim sqlString As String


            sqlConn.ConnectionString = oD._ConnectionString
            sqlConn.Open()

            Try


                sqlString = "usp_EDI_835_PLB"


                cmd = New SqlCommand(sqlString, sqlConn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@HIPAA_835_PLB", PLB)
                cmd.Parameters.AddWithValue("@FILE_ID", oD._FileID)

                err = cmd.ExecuteNonQuery()


                i = 0

            Catch ex As Exception
                i = -1

                log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import835.COMITplb", ex)

            Finally

                sqlConn.Close()
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


                sqlString = "[usp_EDI_835_UNK]"


                cmd = New SqlCommand(sqlString, sqlConn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@ST_ID", oD._STID)
                cmd.Parameters.AddWithValue("@HIPAA_835_UNK", UNK)
                cmd.Parameters.AddWithValue("@FILE_ID", oD._FileID)


                err = cmd.ExecuteNonQuery()



                i = 0

            Catch ex As Exception
                i = -1

                log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import835.rollback for bactch ID " + oD._BatchID.ToString, ex)

            Finally

                sqlConn.Close()
            End Try

            Return i



        End Function


        Private Function UpdateRawSource() As Int32

            Dim rr As Int32

            Dim param As New SqlParameter()


            Dim sqlString As String
            sqlString = "usp_EDI_835_UPDATE_RAWSOURCE"



            Try



                Using Con As New SqlConnection(oD._ConnectionString)
                    Con.Open()
                    Using Com As New SqlCommand(sqlString, Con)

                        Com.CommandType = CommandType.StoredProcedure
                        Com.Parameters.AddWithValue("@RAW_SOURCE", RAW_SOURCE)
                        Com.Parameters.AddWithValue("@FILE_ID", oD._FileID)
                        Com.ExecuteNonQuery()


                    End Using
                    Con.Close()
                End Using





            Catch ex As Exception
                rr = -1

                log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI835.Insertrawsource", ex)

            Finally


            End Try





            Return rr




        End Function




        Public Function RollBack() As Integer


            Dim i As Integer
            Dim param As New SqlParameter()


            Dim sqlConn As SqlConnection = New SqlConnection
            Dim cmd As SqlCommand
            Dim sqlString As String


            sqlConn.ConnectionString = oD._ConnectionString
            sqlConn.Open()

            Try


                sqlString = "[usp_EDI_835_ROLLBACK]"


                cmd = New SqlCommand(sqlString, sqlConn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@FILE_ID", oD._FileID)


                err = cmd.ExecuteNonQuery()



                i = 0

            Catch ex As Exception
                i = -1

                log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import835.rollback for bactch ID " + oD._BatchID.ToString, ex)

            Finally

                sqlConn.Close()
            End Try

            Return i



        End Function





    End Class
End Namespace
