Option Strict On
Option Explicit On



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
Imports DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff
Imports DCSGlobal.BusinessRules.CoreLibrary.IO


Namespace DCSGlobal.EDI

    Public Class Import837

        Inherits EDI837Tables

        Implements IDisposable


        Protected disposed As Boolean = False



        Private log As New logExecption()
        Private em As New Email()
        Private ss As New StringStuff
        Private FM As New FileMove
        Private fio As New FileIO
        Private oD As New Declarations
        Private _EDIFileData As New EDIFile

        Public _837_CLM_GUID As Guid = Guid.Empty
        Public _837_LX_GUID As Guid = Guid.Empty



        Private distinctDT As DataTable

        Private _ClassVersion As String = "2.0"




        Private _ENFlag As Boolean = False


        Private _chars As String
        Private STFlag As Integer = 0
        Private LXFlag As Integer = 0
        Private CLPFlag As Integer = 0
        Private FileID As Int32 = 0


        Private _NM01 As String = String.Empty


        Private _HSDCount As Integer



        Private _ImportReturnCode As Integer = 0
        Private _RowCount As Integer = 0
        Private _SchedulerLogID As Integer = 0


        Private _NM1_GUID As Guid = Guid.Empty








        Private _HL01 As Integer = 0
        Private _HL02 As Integer = 0
        Private _HL03 As Integer = 0
        Private _HL04 As Integer = 0

        Public meddd As IEnumerable
        Private mmm As Integer = 1
        Private lxline As String = String.Empty


        Private _CLMString As Object
        Private CLMFlag As Integer
        Private HI01 As String = String.Empty
        Private HI02 As String = String.Empty
        Private HI03 As String = String.Empty
        Private HI04 As String = String.Empty
        Private HI05 As String = String.Empty
        Private HI06 As String = String.Empty
        Private HI07 As String = String.Empty
        Private HI08 As String = String.Empty
        Private HI09 As String = String.Empty
        Private HI10 As String = String.Empty
        Private HI11 As String = String.Empty
        Private HI12 As String = String.Empty

        Private SV202 As String = String.Empty

        Private CTP05 As String = String.Empty


        Private SVD03 As String = String.Empty


        Dim _SBRDirty As Integer = 0
        Dim _HLFlag As Integer = 0


        Dim CLM05 As String = String.Empty
        Dim CLM11 As String = String.Empty





        Private _InputFolder As String = String.Empty
        Private _FailedFolder As String = String.Empty
        Private _SuccessFolder As String = String.Empty
        Private _DuplicateFolder As String = String.Empty
        Private _FileFilter As String = String.Empty
        Private _FileEXT As String = String.Empty

        Private _SMTPServer As String = String.Empty
        Private _FromMailAddress As String = String.Empty
        Private _ToMailAddress As String = String.Empty

        Private _FileName As String = String.Empty
        Private _FilePath As String = String.Empty
        Private _FileToParse As String
        Private _isValid As Integer = -1

        Private _ProcessElaspedTime As Long
        Private _ProcessStartTime As DateTime
        Private _ProcessEndTime As DateTime



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
                _ConnectionString = value
                log.ConnectionString = value
            End Set
        End Property


        Public WriteOnly Property SchedulerLogID As Integer

            Set(value As Integer)
                _SchedulerLogID = value
            End Set
        End Property


        Public Property DTdistinctDT As DataTable
            Get
                Return distinctDT
            End Get
            Set(value As DataTable)

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
                FM.Input = value
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
                FM.Failed = value

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
                FM.Success = value
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
                FM.Duplicate = value
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




            oD.edi_type = "837i"

            oD.HL20Flag = 0


            Dim filePath As String = "C:\MyDir\MySubDir\myfile.ext"
            Dim directoryPath As String = Path.GetDirectoryName(filePath)


            Dim _ImportReturnCode As Integer = 0


            '     Try


            '  oD.TimeStamp = FormatDateTime(_ProcessStartTime, DateFormat.ShortDate)







            '' check to see IF we have a bactch ID  if not get out 
            'If oD._BatchID = 0 Then
            '    If (_Debug = 1) Then


            '        log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import837.Import", "BatchID = 0 err -1")
            '    End If

            '    _ImportReturnCode = -1
            '    Return _ImportReturnCode
            '    Exit Function
            'End If



            If Not File.Exists(FileToParse) Then
                log.ExceptionDetails("DCSGlobal.EDI.Import837i.Import", FileToParse + " Does Not Exist ")
                Return 1
                Exit Function
            End If




            ' GetType all the  goodies out of the file

            Using Val As New ValidateEDI
                Val.ConnectionString = oD._ConnectionString
                _ImportReturnCode = Val.byFile(_FilePath, _FileName, "837")

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

                    _FILE_ID = _EDIFileData.InsertEDIFile

                    If _EDIFileData.isDuplicate > 0 Then
                        _ImportReturnCode = _EDIFileData.FileID
                        _FILE_ID = _ImportReturnCode
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

                    _FILE_ID = _EDIFileData.InsertEDIFile

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

                HL.Clear()

                AMT.Clear()

                CAS.Clear()
                CL1.Clear()
                CLM.Clear()

                DMG.Clear()
                DTP.Clear()

                HI.Clear()

                LX.Clear()

                K3.Clear()

                MIA.Clear()
                MOA.Clear()

                N3.Clear()
                N4.Clear()
                NM1.Clear()
                NTE.Clear()

                OI.Clear()



                PAT.Clear()
                PER.Clear()
                PWK.Clear()

                REF.Clear()


                SV2.Clear()
                SVD.Clear()

                SE.Clear()
                GS.Clear()
                IEA.Clear()

            End If



            Dim last As String = String.Empty
            Dim line As String = String.Empty
            Dim rowcount As Int32 = 0

            'so lets get down to it opn the file in a stream reader and move thru it one line at a time...........

            Using r As New StreamReader(FileToParse)
                oD.RowProcessedFlag = 0
                line = r.ReadLine()

                'Console.WriteLine(line)
                oD.DataElementSeparator = Mid(line, 4, 1)

                'Console.WriteLine(oD.DataElementSeparator)

                '  Try
                Do While (Not line Is Nothing)
                    ' Add this line to list.
                    last = line
                    ' Display to console.
                    ' 
                    ' Read in the next line.


                    line = line.Replace("~", "")

                    rowcount = rowcount + 1
                    oD.ediRowRecordType = ss.ParseDemlimtedStringEDI(line, oD.DataElementSeparator, 1)
                    'Console.WriteLine(oD.ediRowRecordType)
                    oD.CurrentRowData = line

                    oD.RowProcessedFlag = 0

                    line = r.ReadLine





                    'check for LX

                    'If ss.ParseDemlimtedStringEDI(line, oD.DataElementSeparator, 1) = "LX" Then
                    '    LXFlag = 1
                    'End If





                    'ISA ******************************************************************************************************
                    Try
                        If oD.ediRowRecordType = "ISA" Then

                            oD.RowProcessedFlag = 1

                            oD.EBCarrotCHAR = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 12)

                            _HIPAA_ISA_GUID = Guid.NewGuid
                            'oD.P_GUID = oD.ISA_GUID

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
                            ISARow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            ISARow("FILE_ID") = _FILE_ID
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



                            'oD.ISA_ROW_ID = rowcount
                            ISARow("ROW_NUMBER") = rowcount

                            ISA.Rows.Add(ISARow)

                            oD.CarrotDataDelimiter = oD.ISA11
                            oD.ComponentElementSeperator = oD.ISA16


                            _chars = "RowDataDelimiter: " + oD.DataElementSeparator + " CarrotDataDelimiter: " + oD.CarrotDataDelimiter + " ComponentElementSeperator: " + oD.ComponentElementSeperator



                            'o'D.ISAFlag = 1

                            ComitISA()

                        End If

                    Catch ex As Exception
                        log.ExceptionDetails("ISA 005010X223A2", ex)
                    End Try



                    'GS ******************************************************************************************************
                    Try
                        If oD.ediRowRecordType = "GS" Then
                            oD.RowProcessedFlag = 1
                            _HIPAA_GS_GUID = Guid.NewGuid
                            'o() 'D.P_GUID = oD.GS_GUID

                            oD.GS01 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)
                            oD.GS02 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                            oD.GS03 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)
                            oD.GS04 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 5)
                            oD.GS05 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 6)
                            oD.GS06 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 7)
                            oD.GS07 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 8)
                            oD.GS08 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 9)



                            Dim GSRow As DataRow = GS.NewRow
                            GSRow("ISA_ID") = _ISA_ID
                            GSRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            GSRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            GSRow("GS01") = oD.GS01
                            GSRow("GS02") = oD.GS02
                            GSRow("GS03") = oD.GS03
                            GSRow("GS04") = oD.GS04
                            GSRow("GS05") = oD.GS05
                            GSRow("GS06") = oD.GS06
                            GSRow("GS07") = oD.GS07
                            GSRow("GS08") = oD.GS08

                            GSRow("FILE_ID") = _FILE_ID

                            'o() 'D.GS_ROW_ID = rowcount

                            GSRow("ROW_NUMBER") = rowcount
                            GS.Rows.Add(GSRow)

                            ComitGS()

                        End If

                        'end GS


                    Catch ex As Exception
                        log.ExceptionDetails("GS 005010X223A2", ex)
                    End Try


                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    ' ok so once we find and ST we start a a new string to hold evrey thing to the se and that will get passed to a
                    ' thread to process along with all every thing it needs to tag it all.
                    '
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


                    'ST ******************************************************************************************************
                    Try ' begin st
                        If oD.ediRowRecordType = "ST" Then
                            oD.RowProcessedFlag = 1


                            _HIPAA_ST_GUID = Guid.NewGuid
                            'oD.P_GUID = oD.ST_GUID



                            oD.ST01 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)
                            oD.ST02 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                            oD.ST03 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)


                            Dim STRow As DataRow = ST.NewRow
                            STRow("FILE_ID") = _FILE_ID
                            STRow("ISA_ID") = _ISA_ID
                            STRow("GS_ID") = _GS_ID

                            STRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            STRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            STRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            STRow("ST01") = oD.ST01
                            STRow("ST02") = oD.ST02
                            STRow("ST03") = oD.ST03



                            STRow("ROW_NUMBER") = rowcount
                            ST.Rows.Add(STRow)

                            '   ComitST()
                        End If

                        ' all the rows get made in to a string. 

                    Catch ex As Exception
                        log.ExceptionDetails("ST 005010X223A2", ex)
                    End Try



                    'BHT ******************************************************************************************************
                    Try
                        If oD.ediRowRecordType = "BHT" Then

                            'oD.BHT_GUID = Guid.NewGuid
                            'oD.P_GUID = oD.ST_GUID

                            oD.BHT01 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)
                            oD.BHT02 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                            oD.BHT03 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)
                            oD.BHT04 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 5)
                            oD.BHT05 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 6)
                            oD.BHT06 = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 7)



                            Dim BHTRow As DataRow = BHT.NewRow
                            BHTRow("FILE_ID") = _FILE_ID
                            BHTRow("ISA_ID") = _ISA_ID
                            BHTRow("GS_ID") = _GS_ID
                            BHTRow("ST_ID") = _ST_ID
                            BHTRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            BHTRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            BHTRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            BHTRow("BHT01") = oD.BHT01
                            BHTRow("BHT02") = oD.BHT02
                            BHTRow("BHT03") = oD.BHT03
                            BHTRow("BHT04") = oD.BHT04
                            BHTRow("BHT05") = oD.BHT05
                            BHTRow("BHT06") = oD.BHT06
                            BHTRow("ROW_NUMBER") = rowcount

                            BHT.Rows.Add(BHTRow)


                            oD.RowProcessedFlag = 1

                            _ENFlag = True


                            '    ComitBHT()

                            ComitST()


                        End If

                    Catch ex As Exception
                        log.ExceptionDetails("BHT 005010X223A2", ex)
                    End Try

                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '      Begin 837
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''













                    'HL ******************************************************************************************************
                    Try
                        If oD.ediRowRecordType = "HL" Then


                            'If oD.HL20Flag = 0 Then
                            '    ComitSTHeaders()
                            '    NM1.Clear()
                            '    PER.Clear()
                            '    oD.HL20Flag = 1

                            'End If


                            '   oD.HL_GUID = Guid.NewGuid

                            _NM1_GUID = Guid.Empty


                            oD.RowProcessedFlag = 1

                            Dim HLRow As DataRow = HL.NewRow
                            HLRow("FILE_ID") = _FILE_ID
                            HLRow("ISA_ID") = _ISA_ID
                            HLRow("GS_ID") = _GS_ID
                            HLRow("ST_ID") = _ST_ID
                            HLRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            HLRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            HLRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID




                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then
                                _HL01 = Convert.ToInt32(ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2))
                            Else
                                _HL01 = 0
                            End If

                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then
                                _HL02 = Convert.ToInt32(ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3))
                            Else
                                _HL02 = 0
                            End If



                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then
                                _HL03 = Convert.ToInt32(ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4))
                            Else
                                _HL03 = 0
                            End If



                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then
                                _HL04 = Convert.ToInt32(ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5))
                            Else
                                _HL04 = 0
                            End If

                            Select Case _HL03   ' Must be a primitive data type
                                Case 20

                                    _HIPAA_HL_20_GUID = Guid.NewGuid
                                    _HIPAA_HL_21_GUID = Guid.Empty
                                    _HIPAA_HL_22_GUID = Guid.Empty
                                    _HIPAA_HL_23_GUID = Guid.Empty
                                    _HIPAA_HL_24_GUID = Guid.Empty
                                    _837_CLM_GUID = Guid.Empty
                                    _837_LX_GUID = Guid.Empty

                                    _LoopLevelMajor = 2000
                                    _LoopLevelSubFix = "A"
                                Case 21
                                    _HIPAA_HL_21_GUID = Guid.NewGuid
                                    _HIPAA_HL_22_GUID = Guid.Empty
                                    _HIPAA_HL_23_GUID = Guid.Empty
                                    _HIPAA_HL_24_GUID = Guid.Empty
                                Case 22
                                    _HIPAA_HL_22_GUID = Guid.NewGuid
                                    _HIPAA_HL_23_GUID = Guid.Empty
                                    _HIPAA_HL_24_GUID = Guid.Empty
                                Case 23
                                    _HIPAA_HL_23_GUID = Guid.NewGuid
                                    _HIPAA_HL_24_GUID = Guid.Empty

                                    _LoopLevelMajor = 2000
                                    _LoopLevelSubFix = "C"
                                Case 24
                                    _HIPAA_HL_24_GUID = Guid.NewGuid



                                Case Else

                            End Select



                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then HLRow("HL01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else HLRow("HL01") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then HLRow("HL02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else HLRow("HL02") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then HLRow("HL03") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else HLRow("HL03") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then HLRow("HL04") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else HLRow("HL04") = DBNull.Value


                            HLRow("ROW_NUMBER") = rowcount
                            HLRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            HLRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            HLRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            HLRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            HLRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID



                            HLRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            HLRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                            HLRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor



                            HL.Rows.Add(HLRow)

                            If _HL01 = 1 Then
                                ComitHeader()


                            End If




                            'Private _HIPAA_HL_20_GUID As Guid
                            'Private _HIPAA_HL_21_GUID As Guid
                            'Private _HIPAA_HL_22_GUID As Guid
                            'Private _HIPAA_HL_23_GUID As Guid
                            'Private _HIPAA_HL_24_GUID As Guid


                        End If

                    Catch ex As Exception
                        log.ExceptionDetails("HL 005010X223A2", ex)
                    End Try


                    'NM1 ******************************************************************************************************
                    Try
                        If oD.ediRowRecordType = "NM1" Then

                            oD.RowProcessedFlag = 1

                            _NM1_GUID = Guid.NewGuid

                            Dim NM1Row As DataRow = NM1.NewRow
                            NM1Row("FILE_ID") = _FILE_ID
                            NM1Row("ISA_ID") = _ISA_ID
                            NM1Row("GS_ID") = _GS_ID
                            NM1Row("ST_ID") = _ST_ID
                            NM1Row("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            NM1Row("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            NM1Row("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            NM1Row("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            NM1Row("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            NM1Row("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            NM1Row("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            NM1Row("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            NM1Row("837_CLM_GUID") = _837_CLM_GUID
                            NM1Row("837_LX_GUID") = _837_LX_GUID
                            NM1Row("NM1_GUID") = _NM1_GUID
                            NM1Row("HL01") = _HL01
                            NM1Row("HL02") = _HL02
                            NM1Row("HL03") = _HL03
                            NM1Row("HL04") = _HL04


                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then
                                NM1Row("NM101") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2)
                                _NM01 = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2)
                            Else
                                NM1Row("NM101") = DBNull.Value
                            End If


                            NM1Lookup(_NM01)

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

                            NM1Row("ROW_NUMBER") = rowcount



                            NM1Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            NM1Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                            NM1Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor



                            NM1.Rows.Add(NM1Row)

                        End If

                    Catch ex As Exception
                        log.ExceptionDetails("NM1 005010X223A2", ex)
                    End Try




                    'SBR ******************************************************************************************************
                    Try
                        If oD.ediRowRecordType = "SBR" Then
                            ' CLMFlag = 0

                            _LoopLevelMajor = 2000
                            _LoopLevelSubFix = "B"




                            oD.RowProcessedFlag = 1
                            oD.SBR_GUID = Guid.NewGuid
                            Dim SBRRow As DataRow = SBR.NewRow
                            SBRRow("FILE_ID") = _FILE_ID
                            SBRRow("ISA_ID") = _ISA_ID
                            SBRRow("GS_ID") = _GS_ID
                            SBRRow("ST_ID") = _ST_ID
                            SBRRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            SBRRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            SBRRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            SBRRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            SBRRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            SBRRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            SBRRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            SBRRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            SBRRow("HL01") = _HL01
                            SBRRow("HL02") = _HL02
                            SBRRow("HL03") = _HL03
                            SBRRow("HL04") = _HL04






                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then SBRRow("SBR01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else SBRRow("SBR01") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then SBRRow("SBR02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else SBRRow("SBR02") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then SBRRow("SBR03") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else SBRRow("SBR03") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then SBRRow("SBR04") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else SBRRow("SBR04") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then SBRRow("SBR05") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else SBRRow("SBR05") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then SBRRow("SBR06") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else SBRRow("SBR06") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then SBRRow("SBR07") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else SBRRow("SBR07") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then SBRRow("SBR08") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else SBRRow("SBR08") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then SBRRow("SBR09") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else SBRRow("SBR09") = DBNull.Value


                            SBRRow("ROW_NUMBER") = rowcount


                            SBRRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            SBRRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                            SBRRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor






                            SBR.Rows.Add(SBRRow)

                            ''  CommitSBR()

                        End If

                    Catch ex As Exception
                        log.ExceptionDetails("SBR 005010X223A2", ex)
                    End Try


                    'CLM ******************************************************************************************************
                    Try
                        If oD.ediRowRecordType = "CLM" Then
                            oD.RowProcessedFlag = 1
                            'If CLMFlag = 0 Then


                            _837_CLM_GUID = Guid.NewGuid



                            '    _CLMString = String.Empty

                            '    CLMFlag = 1
                            'Else
                            '    '  ComitCLM()
                            'End If

                            Dim CLMRow As DataRow = CLM.NewRow

                            CLM11 = String.Empty
                            CLM05 = String.Empty

                            oD.CLM_GUID = Guid.NewGuid

                            _LoopLevelMajor = 2300


                            CLMRow("FILE_ID") = _FILE_ID
                            CLMRow("ISA_ID") = _ISA_ID
                            CLMRow("GS_ID") = _GS_ID
                            CLMRow("ST_ID") = _ST_ID
                            CLMRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            CLMRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            CLMRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            CLMRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            CLMRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            CLMRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            CLMRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            CLMRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            CLMRow("837_CLM_GUID") = _837_CLM_GUID
                            CLMRow("837_LX_GUID") = _837_LX_GUID
                            CLMRow("HL01") = _HL01
                            CLMRow("HL02") = _HL02
                            CLMRow("HL03") = _HL03
                            CLMRow("HL04") = _HL04

                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then CLMRow("CLM01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else CLMRow("CLM01") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then CLMRow("CLM02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else CLMRow("CLM02") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then CLMRow("CLM03") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else CLMRow("CLM03") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then CLMRow("CLM04") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else CLMRow("CLM04") = DBNull.Value




                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then CLMRow("CLM05") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else CLMRow("CLM05") = DBNull.Value


                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then
                                CLMRow("CLM05") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6)
                                CLM05 = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6)
                            Else
                                CLMRow("CLM05") = DBNull.Value

                            End If


                            If Not CLM05 = String.Empty Then

                                If (ss.ParseDemlimtedString(CLM05, oD.ComponentElementSeperator, 1) <> "") Then CLMRow("CLM05_01") = ss.ParseDemlimtedString(CLM05, oD.ComponentElementSeperator, 1) Else CLMRow("CLM05_01") = DBNull.Value
                                If (ss.ParseDemlimtedString(CLM05, oD.ComponentElementSeperator, 2) <> "") Then CLMRow("CLM05_02") = ss.ParseDemlimtedString(CLM05, oD.ComponentElementSeperator, 2) Else CLMRow("CLM05_02") = DBNull.Value
                                If (ss.ParseDemlimtedString(CLM05, oD.ComponentElementSeperator, 3) <> "") Then CLMRow("CLM05_03") = ss.ParseDemlimtedString(CLM05, oD.ComponentElementSeperator, 3) Else CLMRow("CLM05_03") = DBNull.Value

                            End If









                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then CLMRow("CLM06") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else CLMRow("CLM06") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then CLMRow("CLM07") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else CLMRow("CLM07") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then CLMRow("CLM08") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else CLMRow("CLM08") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then CLMRow("CLM09") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else CLMRow("CLM09") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) <> "") Then CLMRow("CLM10") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) Else CLMRow("CLM10") = DBNull.Value




                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) <> "") Then CLMRow("CLM11") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) Else CLMRow("CLM11") = DBNull.Value


                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) <> "") Then
                                CLMRow("CLM11") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12)
                                CLM11 = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12)
                            Else
                                CLMRow("CLM11") = DBNull.Value

                            End If


                            If Not CLM11 = String.Empty Then

                                If (ss.ParseDemlimtedString(CLM11, oD.ComponentElementSeperator, 1) <> "") Then CLMRow("CLM11_01") = ss.ParseDemlimtedString(CLM11, oD.ComponentElementSeperator, 1) Else CLMRow("CLM11_01") = DBNull.Value
                                If (ss.ParseDemlimtedString(CLM11, oD.ComponentElementSeperator, 2) <> "") Then CLMRow("CLM11_02") = ss.ParseDemlimtedString(CLM11, oD.ComponentElementSeperator, 2) Else CLMRow("CLM11_02") = DBNull.Value
                                If (ss.ParseDemlimtedString(CLM11, oD.ComponentElementSeperator, 3) <> "") Then CLMRow("CLM11_03") = ss.ParseDemlimtedString(CLM11, oD.ComponentElementSeperator, 3) Else CLMRow("CLM11_03") = DBNull.Value
                                If (ss.ParseDemlimtedString(CLM11, oD.ComponentElementSeperator, 4) <> "") Then CLMRow("CLM11_04") = ss.ParseDemlimtedString(CLM11, oD.ComponentElementSeperator, 2) Else CLMRow("CLM11_04") = DBNull.Value
                                If (ss.ParseDemlimtedString(CLM11, oD.ComponentElementSeperator, 5) <> "") Then CLMRow("CLM11_05") = ss.ParseDemlimtedString(CLM11, oD.ComponentElementSeperator, 3) Else CLMRow("CLM11_05") = DBNull.Value

                            End If



                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) <> "") Then CLMRow("CLM12") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) Else CLMRow("CLM12") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) <> "") Then CLMRow("CLM13") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) Else CLMRow("CLM13") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 15) <> "") Then CLMRow("CLM14") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 15) Else CLMRow("CLM14") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 16) <> "") Then CLMRow("CLM15") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 16) Else CLMRow("CLM15") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 17) <> "") Then CLMRow("CLM16") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 17) Else CLMRow("CLM16") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 18) <> "") Then CLMRow("CLM17") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 18) Else CLMRow("CLM17") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 19) <> "") Then CLMRow("CLM18") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 19) Else CLMRow("CLM18") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 20) <> "") Then CLMRow("CLM19") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 20) Else CLMRow("CLM19") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 21) <> "") Then CLMRow("CLM20") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 21) Else CLMRow("CLM20") = DBNull.Value

                            CLMRow("ROW_NUMBER") = rowcount



                            CLMRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            CLMRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                            CLMRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor



                            CLM.Rows.Add(CLMRow)



                        End If

                        'If CLMFlag = 1 Then

                        '    If oD.ediRowRecordType <> "LX" Then
                        '        _CLMString = _CLMString + oD.CurrentRowData + vbCrLf
                        '    End If
                        'End If


                    Catch ex As Exception
                        log.ExceptionDetails("CLM 005010X223A2", ex)
                    End Try






                    'LX ******************************************************************************************************
                    Try
                        If oD.ediRowRecordType = "LX" Then
                            _LoopLevelMajor = 2400
                            _837_LX_GUID = Guid.NewGuid
                            oD.RowProcessedFlag = 1
                            '  oD.LX_GUID = Guid.NewGuid
                            '   oD.CLM_GUID = Guid.Empty
                            ' oD.SVC_GUID = Guid.Empty


                            Dim LXRow As DataRow = LX.NewRow
                            LXRow("FILE_ID") = _FILE_ID
                            LXRow("ISA_ID") = _ISA_ID
                            LXRow("GS_ID") = _GS_ID
                            LXRow("ST_ID") = _ST_ID
                            LXRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            LXRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            LXRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            LXRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            LXRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            LXRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            LXRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            LXRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            LXRow("837_CLM_GUID") = _837_CLM_GUID
                            LXRow("837_LX_GUID") = _837_LX_GUID
                            LXRow("HL01") = _HL01
                            LXRow("HL02") = _HL02
                            LXRow("HL03") = _HL03
                            LXRow("HL04") = _HL04

                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then LXRow("LX01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else LXRow("LX01") = DBNull.Value

                            LXRow("ROW_NUMBER") = rowcount

                            LXRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            LXRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                            LXRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor

                            LX.Rows.Add(LXRow)


                        End If

                    Catch ex As Exception
                        log.ExceptionDetails("LX 005010X223A2", ex)
                    End Try


                    'AMT ******************************************************************************************************
                    Try
                        If oD.ediRowRecordType = "AMT" Then

                            oD.RowProcessedFlag = 1

                            Dim AMTRow As DataRow = AMT.NewRow
                            AMTRow("FILE_ID") = _FILE_ID
                            AMTRow("ISA_ID") = _ISA_ID
                            AMTRow("GS_ID") = _GS_ID
                            AMTRow("ST_ID") = _ST_ID
                            AMTRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            AMTRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            AMTRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            AMTRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            AMTRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            AMTRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            AMTRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            AMTRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            AMTRow("837_CLM_GUID") = _837_CLM_GUID
                            AMTRow("837_LX_GUID") = _837_LX_GUID
                            AMTRow("HL01") = _HL01
                            AMTRow("HL02") = _HL02
                            AMTRow("HL03") = _HL03
                            AMTRow("HL04") = _HL04

                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then AMTRow("AMT01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else AMTRow("AMT01") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then AMTRow("AMT02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else AMTRow("AMT02") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then AMTRow("AMT03") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else AMTRow("AMT03") = DBNull.Value

                            AMTRow("ROW_NUMBER") = rowcount


                            AMTRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            AMTRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                            AMTRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor


                            AMT.Rows.Add(AMTRow)



                        End If

                    Catch ex As Exception
                        log.ExceptionDetails("ATM 005010X223A2", ex)
                    End Try




                    'CAS ******************************************************************************************************
                    Try
                        If oD.ediRowRecordType = "CAS" Then

                            oD.RowProcessedFlag = 1

                            Dim CASRow As DataRow = CAS.NewRow
                            CASRow("FILE_ID") = _FILE_ID
                            CASRow("ISA_ID") = _ISA_ID
                            CASRow("GS_ID") = _GS_ID
                            CASRow("ST_ID") = _ST_ID
                            CASRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            CASRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            CASRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            CASRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            CASRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            CASRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            CASRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            CASRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            CASRow("837_CLM_GUID") = _837_CLM_GUID
                            CASRow("837_LX_GUID") = _837_LX_GUID
                            CASRow("HL01") = _HL01
                            CASRow("HL02") = _HL02
                            CASRow("HL03") = _HL03
                            CASRow("HL04") = _HL04

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


                            CASRow("ROW_NUMBER") = rowcount


                            CASRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            CASRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                            CASRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor



                            CAS.Rows.Add(CASRow)


                        End If

                    Catch ex As Exception
                        log.ExceptionDetails("CAS 005010X223A2", ex)
                    End Try


                    'CL1 ******************************************************************************************************
                    Try
                        If oD.ediRowRecordType = "CL1" Then

                            oD.RowProcessedFlag = 1

                            Dim CL1Row As DataRow = CL1.NewRow
                            CL1Row("FILE_ID") = _FILE_ID
                            CL1Row("ISA_ID") = _ISA_ID
                            CL1Row("GS_ID") = _GS_ID
                            CL1Row("ST_ID") = _ST_ID
                            CL1Row("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            CL1Row("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            CL1Row("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            CL1Row("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            CL1Row("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            CL1Row("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            CL1Row("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            CL1Row("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            CL1Row("837_CLM_GUID") = _837_CLM_GUID
                            CL1Row("837_LX_GUID") = _837_LX_GUID
                            CL1Row("HL01") = _HL01
                            CL1Row("HL02") = _HL02
                            CL1Row("HL03") = _HL03
                            CL1Row("HL04") = _HL04

                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then CL1Row("CL101") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else CL1Row("CL101") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then CL1Row("CL102") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else CL1Row("CL102") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then CL1Row("CL103") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else CL1Row("CL103") = DBNull.Value


                            CL1Row("ROW_NUMBER") = rowcount

                            CL1Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            CL1Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                            CL1Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor


                            CL1.Rows.Add(CL1Row)



                        End If

                    Catch ex As Exception
                        log.ExceptionDetails("CL1 005010X223A2", ex)
                    End Try


                    'If oD.ediRowRecordType = "CN1" Then

                    '    oD.RowProcessedFlag = 1

                    '    Dim CN1Row As DataRow = CN1.NewRow
                    '    CN1Row("FILE_ID") = _FILE_ID
                    '    CN1Row("ISA_ID") = _ISA_ID
                    '    CN1Row("GS_ID") = _GS_ID
                    '    CN1Row("ST_ID") = _ST_ID
                    '    CN1Row("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                    '    CN1Row("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                    '    CN1Row("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                    '    CN1Row("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                    '    CN1Row("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                    '    CN1Row("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                    '    CN1Row("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                    '    CN1Row("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                    '    CN1Row("837_CLM_GUID") = _837_CLM_GUID
                    '    CN1Row("837_LX_GUID") = _837_LX_GUID
                    '    CN1Row("HL01") = _HL01
                    '    CN1Row("HL02") = _HL02
                    '    CN1Row("HL03") = _HL03
                    '    CN1Row("HL04") = _HL04

                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then CN1Row("CN101") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else CN1Row("CN101") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then CN1Row("CN102") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else CN1Row("CN102") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then CN1Row("CN103") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else CN1Row("CN103") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then CN1Row("CN104") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else CN1Row("CN104") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then CN1Row("CN105") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else CN1Row("CN105") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then CN1Row("CN106") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else CN1Row("CN106") = DBNull.Value

                    '    CN1Row("ROW_NUMBER") = rowcount


                    '    CN1Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                    '    CN1Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                    '    CN1Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor

                    '    CN1.Rows.Add(CN1Row)



                    'End If



                    'If oD.ediRowRecordType = "CRC" Then

                    '    '    oD.RowProcessedFlag = 1

                    '    Dim CRCRow As DataRow = CRC.NewRow
                    '    CRCRow("FILE_ID") = _FILE_ID
                    '    CRCRow("ISA_ID") = _ISA_ID
                    '    CRCRow("GS_ID") = _GS_ID
                    '    CRCRow("ST_ID") = _ST_ID
                    '    CRCRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                    '    CRCRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                    '    CRCRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                    '    CRCRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                    '    CRCRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                    '    CRCRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                    '    CRCRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                    '    CRCRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                    '    CRCRow("837_CLM_GUID") = _837_CLM_GUID
                    '    CRCRow("837_LX_GUID") = _837_LX_GUID
                    '    CRCRow("HL01") = _HL01
                    '    CRCRow("HL02") = _HL02
                    '    CRCRow("HL03") = _HL03
                    '    CRCRow("HL04") = _HL04


                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then CRCRow("CRC01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else CRCRow("CRC01") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then CRCRow("CRC02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else CRCRow("CRC02") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then CRCRow("CRC03") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else CRCRow("CRC03") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then CRCRow("CRC04") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else CRCRow("CRC04") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then CRCRow("CRC05") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else CRCRow("CRC05") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then CRCRow("CRC06") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else CRCRow("CRC06") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then CRCRow("CRC07") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else CRCRow("CRC07") = DBNull.Value

                    '    CRCRow("ROW_NUMBER") = rowcount


                    '    CRCRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                    '    CRCRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                    '    CRCRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor




                    '    CRC.Rows.Add(CRCRow)



                    'End If



                    'If oD.ediRowRecordType = "CTP" Then

                    '    oD.RowProcessedFlag = 1

                    '    Dim CTPRow As DataRow = CTP.NewRow
                    '    CTPRow("FILE_ID") = _FILE_ID
                    '    CTPRow("ISA_ID") = _ISA_ID
                    '    CTPRow("GS_ID") = _GS_ID
                    '    CTPRow("ST_ID") = _ST_ID
                    '    CTPRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                    '    CTPRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                    '    CTPRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                    '    CTPRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                    '    CTPRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                    '    CTPRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                    '    CTPRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                    '    CTPRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                    '    CTPRow("837_CLM_GUID") = _837_CLM_GUID
                    '    CTPRow("837_LX_GUID") = _837_LX_GUID
                    '    CTPRow("HL01") = _HL01
                    '    CTPRow("HL02") = _HL02
                    '    CTPRow("HL03") = _HL03
                    '    CTPRow("HL04") = _HL04

                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then CTPRow("CTP01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else CTPRow("CTP01") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then CTPRow("CTP02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else CTPRow("CTP02") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then CTPRow("CTP03") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else CTPRow("CTP03") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then CTPRow("CTP04") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else CTPRow("CTP04") = DBNull.Value






                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then
                    '        CTPRow("CTP05") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6)
                    '        CTP05 = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6)
                    '    Else
                    '        CTPRow("CTP05") = DBNull.Value

                    '    End If


                    '    If Not CTP05 = String.Empty Then

                    '        If (ss.ParseDemlimtedString(CTP05, oD.ComponentElementSeperator, 1) <> "") Then CTPRow("CTP05_01") = ss.ParseDemlimtedString(CTP05, oD.ComponentElementSeperator, 1) Else CTPRow("CTP05_01") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(CTP05, oD.ComponentElementSeperator, 2) <> "") Then CTPRow("CTP05_02") = ss.ParseDemlimtedString(CTP05, oD.ComponentElementSeperator, 2) Else CTPRow("CTP05_02") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(CTP05, oD.ComponentElementSeperator, 3) <> "") Then CTPRow("CTP05_03") = ss.ParseDemlimtedString(CTP05, oD.ComponentElementSeperator, 3) Else CTPRow("CTP05_03") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(CTP05, oD.ComponentElementSeperator, 4) <> "") Then CTPRow("CTP05_04") = ss.ParseDemlimtedString(CTP05, oD.ComponentElementSeperator, 4) Else CTPRow("CTP05_04") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(CTP05, oD.ComponentElementSeperator, 5) <> "") Then CTPRow("CTP05_05") = ss.ParseDemlimtedString(CTP05, oD.ComponentElementSeperator, 5) Else CTPRow("CTP05_05") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(CTP05, oD.ComponentElementSeperator, 6) <> "") Then CTPRow("CTP05_06") = ss.ParseDemlimtedString(CTP05, oD.ComponentElementSeperator, 6) Else CTPRow("CTP05_06") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(CTP05, oD.ComponentElementSeperator, 7) <> "") Then CTPRow("CTP05_07") = ss.ParseDemlimtedString(CTP05, oD.ComponentElementSeperator, 7) Else CTPRow("CTP05_07") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(CTP05, oD.ComponentElementSeperator, 8) <> "") Then CTPRow("CTP05_08") = ss.ParseDemlimtedString(CTP05, oD.ComponentElementSeperator, 8) Else CTPRow("CTP05_08") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(CTP05, oD.ComponentElementSeperator, 9) <> "") Then CTPRow("CTP05_09") = ss.ParseDemlimtedString(CTP05, oD.ComponentElementSeperator, 9) Else CTPRow("CTP05_09") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(CTP05, oD.ComponentElementSeperator, 10) <> "") Then CTPRow("CTP05_10") = ss.ParseDemlimtedString(CTP05, oD.ComponentElementSeperator, 10) Else CTPRow("CTP05_10") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(CTP05, oD.ComponentElementSeperator, 11) <> "") Then CTPRow("CTP05_11") = ss.ParseDemlimtedString(CTP05, oD.ComponentElementSeperator, 11) Else CTPRow("CTP05_11") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(CTP05, oD.ComponentElementSeperator, 12) <> "") Then CTPRow("CTP05_12") = ss.ParseDemlimtedString(CTP05, oD.ComponentElementSeperator, 12) Else CTPRow("CTP05_12") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(CTP05, oD.ComponentElementSeperator, 13) <> "") Then CTPRow("CTP05_13") = ss.ParseDemlimtedString(CTP05, oD.ComponentElementSeperator, 13) Else CTPRow("CTP05_13") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(CTP05, oD.ComponentElementSeperator, 14) <> "") Then CTPRow("CTP05_14") = ss.ParseDemlimtedString(CTP05, oD.ComponentElementSeperator, 14) Else CTPRow("CTP05_14") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(CTP05, oD.ComponentElementSeperator, 15) <> "") Then CTPRow("CTP05_15") = ss.ParseDemlimtedString(CTP05, oD.ComponentElementSeperator, 15) Else CTPRow("CTP05_15") = DBNull.Value


                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then CTPRow("CTP06") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else CTPRow("CTP06") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then CTPRow("CTP07") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else CTPRow("CTP07") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then CTPRow("CTP08") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else CTPRow("CTP08") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then CTPRow("CTP09") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else CTPRow("CTP09") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) <> "") Then CTPRow("CTP10") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) Else CTPRow("CTP10") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) <> "") Then CTPRow("CTP11") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) Else CTPRow("CTP11") = DBNull.Value


                    '        CTPRow("ROW_NUMBER") = rowcount

                    '        CTPRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                    '        CTPRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                    '        CTPRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor

                    '        CTP.Rows.Add(CTPRow)


                    '    End If
                    'End If





                    'DMG ******************************************************************************************************
                    Try
                        If oD.ediRowRecordType = "DMG" Then

                            oD.RowProcessedFlag = 1

                            Dim DMGRow As DataRow = DMG.NewRow
                            DMGRow("FILE_ID") = _FILE_ID
                            DMGRow("ISA_ID") = _ISA_ID
                            DMGRow("GS_ID") = _GS_ID
                            DMGRow("ST_ID") = _ST_ID
                            DMGRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            DMGRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            DMGRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            DMGRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            DMGRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            DMGRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            DMGRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            DMGRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            DMGRow("837_CLM_GUID") = _837_CLM_GUID
                            DMGRow("837_LX_GUID") = _837_LX_GUID
                            DMGRow("NM1_GUID") = _NM1_GUID
                            DMGRow("HL01") = _HL01
                            DMGRow("HL02") = _HL02
                            DMGRow("HL03") = _HL03
                            DMGRow("HL04") = _HL04

                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then DMGRow("DMG01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else DMGRow("DMG01") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then DMGRow("DMG02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else DMGRow("DMG02") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then DMGRow("DMG03") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else DMGRow("DMG03") = DBNull.Value

                            DMGRow("ROW_NUMBER") = rowcount
                            DMGRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            DMGRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                            DMGRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor


                            DMG.Rows.Add(DMGRow)
                        End If

                    Catch ex As Exception
                        log.ExceptionDetails("DMG 005010X223A2", ex)
                    End Try


                    'DTP ******************************************************************************************************
                    Try
                        If oD.ediRowRecordType = "DTP" Then

                            oD.RowProcessedFlag = 1

                            Dim DTPRow As DataRow = DTP.NewRow
                            DTPRow("FILE_ID") = _FILE_ID
                            DTPRow("ISA_ID") = _ISA_ID
                            DTPRow("GS_ID") = _GS_ID
                            DTPRow("ST_ID") = _ST_ID
                            DTPRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            DTPRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            DTPRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            DTPRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            DTPRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            DTPRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            DTPRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            DTPRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            DTPRow("837_CLM_GUID") = _837_CLM_GUID
                            DTPRow("837_LX_GUID") = _837_LX_GUID
                            DTPRow("HL01") = _HL01
                            DTPRow("HL02") = _HL02
                            DTPRow("HL03") = _HL03
                            DTPRow("HL04") = _HL04

                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then DTPRow("DTP01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else DTPRow("DTP01") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then DTPRow("DTP02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else DTPRow("DTP02") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then DTPRow("DTP03") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else DTPRow("DTP03") = DBNull.Value
                            '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then DTPRow("DTP04") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else DTPRow("DTP04") = DBNull.Value


                            DTPRow("ROW_NUMBER") = rowcount
                            DTPRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            DTPRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            DTPRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                            DTP.Rows.Add(DTPRow)




                        End If

                    Catch ex As Exception
                        log.ExceptionDetails("DTP 005010X223A2", ex)
                    End Try


                    'If oD.ediRowRecordType = "DTM" Then

                    '    oD.RowProcessedFlag = 1

                    '    Dim DTMRow As DataRow = DTM.NewRow
                    '    DTMRow("FILE_ID") = _FILE_ID
                    '    DTMRow("ISA_ID") = _ISA_ID
                    '    DTMRow("GS_ID") = _GS_ID
                    '    DTMRow("ST_ID") = _ST_ID
                    '    DTMRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                    '    DTMRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                    '    DTMRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                    '    DTMRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                    '    DTMRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                    '    DTMRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                    '    DTMRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                    '    DTMRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                    '    DTMRow("837_CLM_GUID") = _837_CLM_GUID
                    '    DTMRow("837_LX_GUID") = _837_LX_GUID
                    '    DTMRow("HL01") = _HL01
                    '    DTMRow("HL02") = _HL02
                    '    DTMRow("HL03") = _HL03
                    '    DTMRow("HL04") = _HL04


                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then DTMRow("DTM01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else DTMRow("DTM01") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then DTMRow("DTM02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else DTMRow("DTM02") = DBNull.Value


                    '    DTMRow("ROW_NUMBER") = rowcount
                    '    DTMRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                    '    DTMRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                    '    DTMRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                    '    DTM.Rows.Add(DTMRow)


                    'End If



                    'HI ******************************************************************************************************
                    Try
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

                            HIRow("FILE_ID") = _FILE_ID
                            HIRow("ISA_ID") = _ISA_ID
                            HIRow("GS_ID") = _GS_ID
                            HIRow("ST_ID") = _ST_ID
                            HIRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            HIRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            HIRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            HIRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            HIRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            HIRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            HIRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            HIRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            HIRow("837_CLM_GUID") = _837_CLM_GUID
                            HIRow("837_LX_GUID") = _837_LX_GUID
                            HIRow("HL01") = _HL01
                            HIRow("HL02") = _HL02
                            HIRow("HL03") = _HL03
                            HIRow("HL04") = _HL04

                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then
                                HIRow("HI01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2)
                                HI01 = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2)
                            Else
                                HIRow("HI01") = DBNull.Value

                            End If


                            If Not HI01 = String.Empty Then

                                If (ss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 1) <> "") Then HIRow("HI01_1") = ss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 1) Else HIRow("HI01_1") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 2) <> "") Then HIRow("HI01_2") = ss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 2) Else HIRow("HI01_2") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 3) <> "") Then HIRow("HI01_3") = ss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 3) Else HIRow("HI01_3") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 4) <> "") Then HIRow("HI01_4") = ss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 4) Else HIRow("HI01_4") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 5) <> "") Then HIRow("HI01_5") = ss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 5) Else HIRow("HI01_5") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 6) <> "") Then HIRow("HI01_6") = ss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 6) Else HIRow("HI01_6") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 7) <> "") Then HIRow("HI01_7") = ss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 7) Else HIRow("HI01_7") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 8) <> "") Then HIRow("HI01_8") = ss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 8) Else HIRow("HI01_8") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 9) <> "") Then HIRow("HI01_9") = ss.ParseDemlimtedString(HI01, oD.ComponentElementSeperator, 9) Else HIRow("HI01_9") = DBNull.Value

                            End If


                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then
                                HIRow("HI02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3)
                                HI02 = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3)
                            Else
                                HIRow("HI02") = DBNull.Value

                            End If


                            If Not HI02 = String.Empty Then

                                If (ss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 1) <> "") Then HIRow("HI02_1") = ss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 1) Else HIRow("HI02_1") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 2) <> "") Then HIRow("HI02_2") = ss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 2) Else HIRow("HI02_2") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 3) <> "") Then HIRow("HI02_3") = ss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 3) Else HIRow("HI02_3") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 4) <> "") Then HIRow("HI02_4") = ss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 4) Else HIRow("HI02_4") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 5) <> "") Then HIRow("HI02_5") = ss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 5) Else HIRow("HI02_5") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 6) <> "") Then HIRow("HI02_6") = ss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 6) Else HIRow("HI02_6") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 7) <> "") Then HIRow("HI02_7") = ss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 7) Else HIRow("HI02_7") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 8) <> "") Then HIRow("HI02_8") = ss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 8) Else HIRow("HI02_8") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 9) <> "") Then HIRow("HI02_9") = ss.ParseDemlimtedString(HI02, oD.ComponentElementSeperator, 9) Else HIRow("HI02_9") = DBNull.Value

                            End If



                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then
                                HIRow("HI03") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4)
                                HI03 = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4)
                            Else
                                HIRow("HI03") = DBNull.Value

                            End If


                            If Not HI03 = String.Empty Then

                                If (ss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 1) <> "") Then HIRow("HI03_1") = ss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 1) Else HIRow("HI03_1") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 2) <> "") Then HIRow("HI03_2") = ss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 2) Else HIRow("HI03_2") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 3) <> "") Then HIRow("HI03_3") = ss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 3) Else HIRow("HI03_3") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 4) <> "") Then HIRow("HI03_4") = ss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 4) Else HIRow("HI03_4") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 5) <> "") Then HIRow("HI03_5") = ss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 5) Else HIRow("HI03_5") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 6) <> "") Then HIRow("HI03_6") = ss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 6) Else HIRow("HI03_6") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 7) <> "") Then HIRow("HI03_7") = ss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 7) Else HIRow("HI03_7") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 8) <> "") Then HIRow("HI03_8") = ss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 8) Else HIRow("HI03_8") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 9) <> "") Then HIRow("HI03_9") = ss.ParseDemlimtedString(HI03, oD.ComponentElementSeperator, 9) Else HIRow("HI03_9") = DBNull.Value

                            End If


                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then
                                HIRow("HI04") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5)
                                HI04 = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5)
                            Else
                                HIRow("HI04") = DBNull.Value

                            End If


                            If Not HI04 = String.Empty Then

                                If (ss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 1) <> "") Then HIRow("HI04_1") = ss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 1) Else HIRow("HI04_1") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 2) <> "") Then HIRow("HI04_2") = ss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 2) Else HIRow("HI04_2") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 3) <> "") Then HIRow("HI04_3") = ss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 3) Else HIRow("HI04_3") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 4) <> "") Then HIRow("HI04_4") = ss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 4) Else HIRow("HI04_4") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 5) <> "") Then HIRow("HI04_5") = ss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 5) Else HIRow("HI04_5") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 6) <> "") Then HIRow("HI04_6") = ss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 6) Else HIRow("HI04_6") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 7) <> "") Then HIRow("HI04_7") = ss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 7) Else HIRow("HI04_7") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 8) <> "") Then HIRow("HI04_8") = ss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 8) Else HIRow("HI04_8") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 9) <> "") Then HIRow("HI04_9") = ss.ParseDemlimtedString(HI04, oD.ComponentElementSeperator, 9) Else HIRow("HI04_9") = DBNull.Value

                            End If

                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then
                                HIRow("HI05") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6)
                                HI05 = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6)
                            Else
                                HIRow("HI05") = DBNull.Value

                            End If


                            If Not HI05 = String.Empty Then

                                If (ss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 1) <> "") Then HIRow("HI05_1") = ss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 1) Else HIRow("HI05_1") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 2) <> "") Then HIRow("HI05_2") = ss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 2) Else HIRow("HI05_2") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 3) <> "") Then HIRow("HI05_3") = ss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 3) Else HIRow("HI05_3") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 4) <> "") Then HIRow("HI05_4") = ss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 4) Else HIRow("HI05_4") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 5) <> "") Then HIRow("HI05_5") = ss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 5) Else HIRow("HI05_5") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 6) <> "") Then HIRow("HI05_6") = ss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 6) Else HIRow("HI05_6") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 7) <> "") Then HIRow("HI05_7") = ss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 7) Else HIRow("HI05_7") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 8) <> "") Then HIRow("HI05_8") = ss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 8) Else HIRow("HI05_8") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 9) <> "") Then HIRow("HI05_9") = ss.ParseDemlimtedString(HI05, oD.ComponentElementSeperator, 9) Else HIRow("HI05_9") = DBNull.Value

                            End If



                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then
                                HIRow("HI06") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7)
                                HI06 = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7)
                            Else
                                HIRow("HI06") = DBNull.Value

                            End If


                            If Not HI06 = String.Empty Then

                                If (ss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 1) <> "") Then HIRow("HI06_1") = ss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 1) Else HIRow("HI06_1") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 2) <> "") Then HIRow("HI06_2") = ss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 2) Else HIRow("HI06_2") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 3) <> "") Then HIRow("HI06_3") = ss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 3) Else HIRow("HI06_3") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 4) <> "") Then HIRow("HI06_4") = ss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 4) Else HIRow("HI06_4") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 5) <> "") Then HIRow("HI06_5") = ss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 5) Else HIRow("HI06_5") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 6) <> "") Then HIRow("HI06_6") = ss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 6) Else HIRow("HI06_6") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 7) <> "") Then HIRow("HI06_7") = ss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 7) Else HIRow("HI06_7") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 8) <> "") Then HIRow("HI06_8") = ss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 8) Else HIRow("HI06_8") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 9) <> "") Then HIRow("HI06_9") = ss.ParseDemlimtedString(HI06, oD.ComponentElementSeperator, 9) Else HIRow("HI06_9") = DBNull.Value

                            End If




                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then
                                HIRow("HI07") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8)
                                HI07 = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8)
                            Else
                                HIRow("HI07") = DBNull.Value

                            End If


                            If Not HI07 = String.Empty Then

                                If (ss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 1) <> "") Then HIRow("HI07_1") = ss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 1) Else HIRow("HI07_1") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 2) <> "") Then HIRow("HI07_2") = ss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 2) Else HIRow("HI07_2") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 3) <> "") Then HIRow("HI07_3") = ss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 3) Else HIRow("HI07_3") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 4) <> "") Then HIRow("HI07_4") = ss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 4) Else HIRow("HI07_4") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 5) <> "") Then HIRow("HI07_5") = ss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 5) Else HIRow("HI07_5") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 6) <> "") Then HIRow("HI07_6") = ss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 6) Else HIRow("HI07_6") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 7) <> "") Then HIRow("HI07_7") = ss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 7) Else HIRow("HI07_7") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 8) <> "") Then HIRow("HI07_8") = ss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 8) Else HIRow("HI07_8") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 9) <> "") Then HIRow("HI07_9") = ss.ParseDemlimtedString(HI07, oD.ComponentElementSeperator, 9) Else HIRow("HI07_9") = DBNull.Value

                            End If

                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then
                                HIRow("HI08") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9)
                                HI08 = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9)
                            Else
                                HIRow("HI08") = DBNull.Value

                            End If


                            If Not HI08 = String.Empty Then

                                If (ss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 1) <> "") Then HIRow("HI08_1") = ss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 1) Else HIRow("HI08_1") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 2) <> "") Then HIRow("HI08_2") = ss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 2) Else HIRow("HI08_2") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 3) <> "") Then HIRow("HI08_3") = ss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 3) Else HIRow("HI08_3") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 4) <> "") Then HIRow("HI08_4") = ss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 4) Else HIRow("HI08_4") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 5) <> "") Then HIRow("HI08_5") = ss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 5) Else HIRow("HI08_5") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 6) <> "") Then HIRow("HI08_6") = ss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 6) Else HIRow("HI08_6") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 7) <> "") Then HIRow("HI08_7") = ss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 7) Else HIRow("HI08_7") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 8) <> "") Then HIRow("HI08_8") = ss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 8) Else HIRow("HI08_8") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 9) <> "") Then HIRow("HI08_9") = ss.ParseDemlimtedString(HI08, oD.ComponentElementSeperator, 9) Else HIRow("HI08_9") = DBNull.Value

                            End If

                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then
                                HIRow("HI09") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10)
                                HI09 = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10)
                            Else
                                HIRow("HI09") = DBNull.Value

                            End If


                            If Not HI09 = String.Empty Then

                                If (ss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 1) <> "") Then HIRow("HI09_1") = ss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 1) Else HIRow("HI09_1") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 2) <> "") Then HIRow("HI09_2") = ss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 2) Else HIRow("HI09_2") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 3) <> "") Then HIRow("HI09_3") = ss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 3) Else HIRow("HI09_3") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 4) <> "") Then HIRow("HI09_4") = ss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 4) Else HIRow("HI09_4") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 5) <> "") Then HIRow("HI09_5") = ss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 5) Else HIRow("HI09_5") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 6) <> "") Then HIRow("HI09_6") = ss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 6) Else HIRow("HI09_6") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 7) <> "") Then HIRow("HI09_7") = ss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 7) Else HIRow("HI09_7") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 8) <> "") Then HIRow("HI09_8") = ss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 8) Else HIRow("HI09_8") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 9) <> "") Then HIRow("HI09_9") = ss.ParseDemlimtedString(HI09, oD.ComponentElementSeperator, 9) Else HIRow("HI09_9") = DBNull.Value

                            End If


                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) <> "") Then
                                HIRow("HI10") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11)
                                HI10 = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11)
                            Else
                                HIRow("HI10") = DBNull.Value

                            End If


                            If Not HI10 = String.Empty Then

                                If (ss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 1) <> "") Then HIRow("HI10_1") = ss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 1) Else HIRow("HI10_1") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 2) <> "") Then HIRow("HI10_2") = ss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 2) Else HIRow("HI10_2") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 3) <> "") Then HIRow("HI10_3") = ss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 3) Else HIRow("HI10_3") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 4) <> "") Then HIRow("HI10_4") = ss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 4) Else HIRow("HI10_4") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 5) <> "") Then HIRow("HI10_5") = ss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 5) Else HIRow("HI10_5") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 6) <> "") Then HIRow("HI10_6") = ss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 6) Else HIRow("HI10_6") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 7) <> "") Then HIRow("HI10_7") = ss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 7) Else HIRow("HI10_7") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 8) <> "") Then HIRow("HI10_8") = ss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 8) Else HIRow("HI10_8") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 9) <> "") Then HIRow("HI10_9") = ss.ParseDemlimtedString(HI10, oD.ComponentElementSeperator, 9) Else HIRow("HI10_9") = DBNull.Value

                            End If

                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) <> "") Then
                                HIRow("HI11") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12)
                                HI11 = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12)
                            Else
                                HIRow("HI11") = DBNull.Value

                            End If


                            If Not HI11 = String.Empty Then

                                If (ss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 1) <> "") Then HIRow("HI11_1") = ss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 1) Else HIRow("HI11_1") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 2) <> "") Then HIRow("HI11_2") = ss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 2) Else HIRow("HI11_2") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 3) <> "") Then HIRow("HI11_3") = ss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 3) Else HIRow("HI11_3") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 4) <> "") Then HIRow("HI11_4") = ss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 4) Else HIRow("HI11_4") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 5) <> "") Then HIRow("HI11_5") = ss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 5) Else HIRow("HI11_5") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 6) <> "") Then HIRow("HI11_6") = ss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 6) Else HIRow("HI11_6") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 7) <> "") Then HIRow("HI11_7") = ss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 7) Else HIRow("HI11_7") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 8) <> "") Then HIRow("HI11_8") = ss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 8) Else HIRow("HI11_8") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 9) <> "") Then HIRow("HI11_9") = ss.ParseDemlimtedString(HI11, oD.ComponentElementSeperator, 9) Else HIRow("HI11_9") = DBNull.Value

                            End If


                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) <> "") Then
                                HIRow("HI12") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13)
                                HI12 = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13)
                            Else
                                HIRow("HI12") = DBNull.Value

                            End If


                            If Not HI12 = String.Empty Then

                                If (ss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 1) <> "") Then HIRow("HI12_1") = ss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 1) Else HIRow("HI12_1") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 2) <> "") Then HIRow("HI12_2") = ss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 2) Else HIRow("HI12_2") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 3) <> "") Then HIRow("HI12_3") = ss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 3) Else HIRow("HI12_3") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 4) <> "") Then HIRow("HI12_4") = ss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 4) Else HIRow("HI12_4") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 5) <> "") Then HIRow("HI12_5") = ss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 5) Else HIRow("HI12_5") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 6) <> "") Then HIRow("HI12_6") = ss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 6) Else HIRow("HI12_6") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 7) <> "") Then HIRow("HI12_7") = ss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 7) Else HIRow("HI12_7") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 8) <> "") Then HIRow("HI12_8") = ss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 8) Else HIRow("HI12_8") = DBNull.Value
                                If (ss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 9) <> "") Then HIRow("HI12_9") = ss.ParseDemlimtedString(HI12, oD.ComponentElementSeperator, 9) Else HIRow("HI12_9") = DBNull.Value

                            End If

                            HIRow("ROW_NUMBER") = rowcount


                            HIRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            HIRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            HIRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                            HI.Rows.Add(HIRow)



                        End If

                    Catch ex As Exception
                        log.ExceptionDetails("HI 005010X223A2", ex)
                    End Try






                    'K3 ******************************************************************************************************
                    Try
                        If oD.ediRowRecordType = "K3" Then

                            oD.RowProcessedFlag = 1

                            Dim K3Row As DataRow = K3.NewRow
                            K3Row("FILE_ID") = _FILE_ID
                            K3Row("ISA_ID") = _ISA_ID
                            K3Row("GS_ID") = _GS_ID
                            K3Row("ST_ID") = _ST_ID
                            K3Row("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            K3Row("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            K3Row("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            K3Row("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            K3Row("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            K3Row("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            K3Row("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            K3Row("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            K3Row("837_CLM_GUID") = _837_CLM_GUID
                            K3Row("837_LX_GUID") = _837_LX_GUID
                            K3Row("HL01") = _HL01
                            K3Row("HL02") = _HL02
                            K3Row("HL03") = _HL03
                            K3Row("HL04") = _HL04

                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then K3Row("K301") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else K3Row("K301") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then K3Row("K302") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else K3Row("K302") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then K3Row("K303") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else K3Row("K303") = DBNull.Value

                            K3Row("ROW_NUMBER") = rowcount


                            K3Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            K3Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            K3Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                            K3.Rows.Add(K3Row)



                        End If

                    Catch ex As Exception
                        log.ExceptionDetails("K3 005010X223A2", ex)
                    End Try


                    'LIN ******************************************************************************************************
                    'Try

                    '    If oD.ediRowRecordType = "LIN" Then

                    '        oD.RowProcessedFlag = 1

                    '        Dim LINRow As DataRow = LIN.NewRow
                    '        LINRow("FILE_ID") = _FILE_ID
                    '        LINRow("ISA_ID") = _ISA_ID
                    '        LINRow("GS_ID") = _GS_ID
                    '        LINRow("ST_ID") = _ST_ID
                    '        LINRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                    '        LINRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                    '        LINRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                    '        LINRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                    '        LINRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                    '        LINRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                    '        LINRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                    '        LINRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                    '        LINRow("837_CLM_GUID") = _837_CLM_GUID
                    '        LINRow("837_LX_GUID") = _837_LX_GUID
                    '        LINRow("HL01") = _HL01
                    '        LINRow("HL02") = _HL02
                    '        LINRow("HL03") = _HL03
                    '        LINRow("HL04") = _HL04

                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then LINRow("LIN01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else LINRow("LIN01") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then LINRow("LIN02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else LINRow("LIN02") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then LINRow("LIN03") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else LINRow("LIN03") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then LINRow("LIN04") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else LINRow("LIN04") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then LINRow("LIN05") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else LINRow("LIN05") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then LINRow("LIN06") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else LINRow("LIN06") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then LINRow("LIN07") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else LINRow("LIN07") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then LINRow("LIN08") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else LINRow("LIN08") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then LINRow("LIN09") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else LINRow("LIN09") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) <> "") Then LINRow("LIN10") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) Else LINRow("LIN10") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) <> "") Then LINRow("LIN11") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) Else LINRow("LIN11") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) <> "") Then LINRow("LIN12") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) Else LINRow("LIN12") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) <> "") Then LINRow("LIN13") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) Else LINRow("LIN13") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 15) <> "") Then LINRow("LIN14") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 15) Else LINRow("LIN14") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 16) <> "") Then LINRow("LIN15") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 16) Else LINRow("LIN15") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 17) <> "") Then LINRow("LIN16") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 17) Else LINRow("LIN16") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 18) <> "") Then LINRow("LIN17") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 18) Else LINRow("LIN17") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 19) <> "") Then LINRow("LIN18") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 19) Else LINRow("LIN18") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 20) <> "") Then LINRow("LIN19") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 20) Else LINRow("LIN19") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 21) <> "") Then LINRow("LIN20") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 21) Else LINRow("LIN20") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 22) <> "") Then LINRow("LIN21") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 22) Else LINRow("LIN21") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 23) <> "") Then LINRow("LIN22") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 23) Else LINRow("LIN22") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 24) <> "") Then LINRow("LIN23") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 24) Else LINRow("LIN23") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 25) <> "") Then LINRow("LIN24") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 25) Else LINRow("LIN24") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 26) <> "") Then LINRow("LIN25") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 26) Else LINRow("LIN25") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 27) <> "") Then LINRow("LIN26") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 27) Else LINRow("LIN26") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 28) <> "") Then LINRow("LIN27") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 28) Else LINRow("LIN27") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 29) <> "") Then LINRow("LIN28") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 29) Else LINRow("LIN28") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 30) <> "") Then LINRow("LIN29") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 30) Else LINRow("LIN29") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 31) <> "") Then LINRow("LIN30") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 31) Else LINRow("LIN30") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 32) <> "") Then LINRow("LIN31") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 32) Else LINRow("LIN31") = DBNull.Value

                    '        LINRow("ROW_NUMBER") = rowcount



                    '        LINRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                    '        LINRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                    '        LINRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                    '        LIN.Rows.Add(LINRow)



                    '    End If

                    'Catch ex As Exception

                    'End Try


                    'MOA ******************************************************************************************************
                    Try
                        If oD.ediRowRecordType = "MOA" Then
                            oD.RowProcessedFlag = 1
                            Dim MOARow As DataRow = MOA.NewRow
                            MOARow("FILE_ID") = _FILE_ID
                            MOARow("ISA_ID") = _ISA_ID
                            MOARow("GS_ID") = _GS_ID
                            MOARow("ST_ID") = _ST_ID
                            MOARow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            MOARow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            MOARow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            MOARow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            MOARow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            MOARow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            MOARow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            MOARow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            MOARow("837_CLM_GUID") = _837_CLM_GUID
                            MOARow("837_LX_GUID") = _837_LX_GUID
                            MOARow("HL01") = _HL01
                            MOARow("HL02") = _HL02
                            MOARow("HL03") = _HL03
                            MOARow("HL04") = _HL04


                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then MOARow("MOA01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else MOARow("MOA01") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then MOARow("MOA02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else MOARow("MOA02") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then MOARow("MOA03") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else MOARow("MOA03") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then MOARow("MOA04") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else MOARow("MOA04") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then MOARow("MOA05") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else MOARow("MOA05") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then MOARow("MOA06") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else MOARow("MOA06") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then MOARow("MOA07") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else MOARow("MOA07") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then MOARow("MOA08") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else MOARow("MOA08") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then MOARow("MOA09") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else MOARow("MOA09") = DBNull.Value


                            MOARow("ROW_NUMBER") = rowcount

                            MOARow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            MOARow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            MOARow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                            MOA.Rows.Add(MOARow)


                        End If
                    Catch ex As Exception
                        log.ExceptionDetails("MOA 005010X223A2", ex)
                    End Try





                    'MIA ******************************************************************************************************
                    Try
                        If oD.ediRowRecordType = "MIA" Then
                            oD.RowProcessedFlag = 1
                            Dim MIARow As DataRow = MIA.NewRow
                            MIARow("FILE_ID") = _FILE_ID
                            MIARow("ISA_ID") = _ISA_ID
                            MIARow("GS_ID") = _GS_ID
                            MIARow("ST_ID") = _ST_ID
                            MIARow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            MIARow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            MIARow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            MIARow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            MIARow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            MIARow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            MIARow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            MIARow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            MIARow("837_CLM_GUID") = _837_CLM_GUID
                            MIARow("837_LX_GUID") = _837_LX_GUID
                            MIARow("HL01") = _HL01
                            MIARow("HL02") = _HL02
                            MIARow("HL03") = _HL03
                            MIARow("HL04") = _HL04


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
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 25) <> "") Then MIARow("MIA24") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 25) Else MIARow("MIA24") = DBNull.Value


                            MIARow("ROW_NUMBER") = rowcount

                            MIARow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            MIARow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            MIARow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                            MIA.Rows.Add(MIARow)


                        End If

                    Catch ex As Exception
                        log.ExceptionDetails("MIA 005010X223A2", ex)
                    End Try







                    'If oD.ediRowRecordType = "N1" Then

                    '    oD.RowProcessedFlag = 1

                    '    Dim N1Row As DataRow = N1.NewRow
                    '    N1Row("FILE_ID") = _FILE_ID
                    '    N1Row("ISA_ID") = _ISA_ID
                    '    N1Row("GS_ID") = _GS_ID
                    '    N1Row("ST_ID") = _ST_ID
                    '    N1Row("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                    '    N1Row("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                    '    N1Row("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                    '    N1Row("837_CLM_GUID") = _837_CLM_GUID
                    '    N1Row("837_LX_GUID") = _837_LX_GUID
                    '    N1Row("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                    '    N1Row("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                    '    N1Row("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                    '    N1Row("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                    '    N1Row("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                    '    N1Row("HL01") = _HL01
                    '    N1Row("HL02") = _HL02
                    '    N1Row("HL03") = _HL03
                    '    N1Row("HL04") = _HL04

                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then N1Row("N101") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else N1Row("N101") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then N1Row("N102") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else N1Row("N102") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then N1Row("N103") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else N1Row("N103") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then N1Row("N104") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else N1Row("N104") = DBNull.Value

                    '    N1Row("ROW_NUMBER") = rowcount

                    '    N1Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                    '    N1Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                    '    N1Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                    '    N1.Rows.Add(N1Row)




                    'End If




                    'N3 *******************************************************************************************************
                    Try
                        If oD.ediRowRecordType = "N3" Then

                            oD.RowProcessedFlag = 1
                            Dim N3Row As DataRow = N3.NewRow
                            N3Row("FILE_ID") = _FILE_ID
                            N3Row("ISA_ID") = _ISA_ID
                            N3Row("GS_ID") = _GS_ID
                            N3Row("ST_ID") = _ST_ID
                            N3Row("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            N3Row("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            N3Row("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            N3Row("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            N3Row("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            N3Row("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            N3Row("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            N3Row("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            N3Row("837_CLM_GUID") = _837_CLM_GUID
                            N3Row("837_LX_GUID") = _837_LX_GUID
                            N3Row("NM1_GUID") = _NM1_GUID
                            N3Row("HL01") = _HL01
                            N3Row("HL02") = _HL02
                            N3Row("HL03") = _HL03
                            N3Row("HL04") = _HL04

                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then N3Row("N301") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else N3Row("N301") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then N3Row("N302") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else N3Row("N302") = DBNull.Value




                            N3Row("ROW_NUMBER") = rowcount


                            N3Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            N3Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            N3Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                            N3.Rows.Add(N3Row)





                        End If

                    Catch ex As Exception
                        log.ExceptionDetails("N3 005010X223A2", ex)
                    End Try



                    'N4 *******************************************************************************************************
                    Try
                        If oD.ediRowRecordType = "N4" Then


                            Dim N4Row As DataRow = N4.NewRow
                            N4Row("FILE_ID") = _FILE_ID
                            N4Row("ISA_ID") = _ISA_ID
                            N4Row("GS_ID") = _GS_ID
                            N4Row("ST_ID") = _ST_ID
                            N4Row("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            N4Row("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            N4Row("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            N4Row("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            N4Row("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            N4Row("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            N4Row("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            N4Row("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            N4Row("837_CLM_GUID") = _837_CLM_GUID
                            N4Row("837_LX_GUID") = _837_LX_GUID
                            N4Row("NM1_GUID") = _NM1_GUID
                            N4Row("HL01") = _HL01
                            N4Row("HL02") = _HL02
                            N4Row("HL03") = _HL03
                            N4Row("HL04") = _HL04

                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then N4Row("N401") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else N4Row("N401") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then N4Row("N402") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else N4Row("N402") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then N4Row("N403") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else N4Row("N403") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then N4Row("N404") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else N4Row("N404") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then N4Row("N405") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else N4Row("N405") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then N4Row("N406") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else N4Row("N406") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then N4Row("N407") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else N4Row("N407") = DBNull.Value

                            N4Row("ROW_NUMBER") = rowcount


                            N4Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            N4Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            N4Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                            N4.Rows.Add(N4Row)

                            oD.RowProcessedFlag = 1


                        End If
                    Catch ex As Exception
                        log.ExceptionDetails("N4 005010X223A2", ex)
                    End Try




                    'NTE ******************************************************************************************************
                    Try
                        If oD.ediRowRecordType = "NTE" Then

                            oD.RowProcessedFlag = 1

                            Dim NTERow As DataRow = NTE.NewRow
                            NTERow("ISA_ID") = _ISA_ID
                            NTERow("FILE_ID") = _FILE_ID
                            NTERow("GS_ID") = _GS_ID
                            NTERow("ST_ID") = _ST_ID
                            NTERow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            NTERow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            NTERow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            NTERow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            NTERow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            NTERow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            NTERow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            NTERow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            NTERow("837_CLM_GUID") = _837_CLM_GUID
                            NTERow("837_LX_GUID") = _837_LX_GUID
                            NTERow("HL01") = _HL01
                            NTERow("HL02") = _HL02
                            NTERow("HL03") = _HL03
                            NTERow("HL04") = _HL04



                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then NTERow("NTE01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else NTERow("NTE01") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then NTERow("NTE02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else NTERow("NTE02") = DBNull.Value


                            NTERow("ROW_NUMBER") = rowcount

                            NTERow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            NTERow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            NTERow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                            NTE.Rows.Add(NTERow)



                        End If

                    Catch ex As Exception
                        log.ExceptionDetails("NTE 005010X223A2", ex)
                    End Try




                    'If oD.ediRowRecordType = "CR2" Then

                    '    oD.RowProcessedFlag = 1

                    '    Dim CR2Row As DataRow = CR2.NewRow
                    '    CR2Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                    '    CR2Row("HIPAA_GS_GUID") = oD.GS_GUID
                    '    CR2Row("HIPAA_ST_GUID") = oD.ST_GUID
                    '    CR2Row("HIPAA_BHT_GUID") = oD.BHT_GUID
                    '    CR2Row("HIPAA_HL_GUID") = oD.HL_GUID
                    '    CR2Row("HIPAA_CLM_GUID") = oD.CLM_GUID
                    '    CR2Row("HIPAA_LX_GUID") = oD.LX_GUID


                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then CR2Row("CR201") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else CR2Row("CR201") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then CR2Row("CR202") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else CR2Row("CR202") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then CR2Row("CR203") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else CR2Row("CR203") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then CR2Row("CR204") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else CR2Row("CR204") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then CR2Row("CR205") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else CR2Row("CR205") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then CR2Row("CR206") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else CR2Row("CR206") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then CR2Row("CR207") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else CR2Row("CR207") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then CR2Row("CR208") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else CR2Row("CR208") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then CR2Row("CR209") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else CR2Row("CR209") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) <> "") Then CR2Row("CR210") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) Else CR2Row("CR210") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) <> "") Then CR2Row("CR211") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) Else CR2Row("CR211") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) <> "") Then CR2Row("CR212") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) Else CR2Row("CR212") = DBNull.Value


                    '    CR2.Rows.Add(CR2Row)



                    'End If







                    'OI ******************************************************************************************************
                    Try
                        If oD.ediRowRecordType = "OI" Then

                            oD.RowProcessedFlag = 1

                            Dim OIRow As DataRow = OI.NewRow
                            OIRow("FILE_ID") = _FILE_ID
                            OIRow("ISA_ID") = _ISA_ID
                            OIRow("GS_ID") = _GS_ID
                            OIRow("ST_ID") = _ST_ID
                            OIRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            OIRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            OIRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            OIRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            OIRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            OIRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            OIRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            OIRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            OIRow("837_CLM_GUID") = _837_CLM_GUID
                            OIRow("837_LX_GUID") = _837_LX_GUID
                            OIRow("HL01") = _HL01
                            OIRow("HL02") = _HL02
                            OIRow("HL03") = _HL03
                            OIRow("HL04") = _HL04

                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then OIRow("OI01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else OIRow("OI01") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then OIRow("OI02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else OIRow("OI02") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then OIRow("OI03") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else OIRow("OI03") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then OIRow("OI04") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else OIRow("OI04") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then OIRow("OI05") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else OIRow("OI05") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then OIRow("OI06") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else OIRow("OI06") = DBNull.Value

                            OIRow("ROW_NUMBER") = rowcount

                            OIRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            OIRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            OIRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                            OI.Rows.Add(OIRow)



                        End If
                    Catch ex As Exception
                        log.ExceptionDetails("OI 005010X223A2", ex)
                    End Try



                    'PAT ******************************************************************************************************
                    Try
                        If oD.ediRowRecordType = "PAT" Then

                            oD.RowProcessedFlag = 1
                            Dim PATRow As DataRow = PAT.NewRow
                            PATRow("DOCUMENT_ID") = _DOCUMENT_ID
                            PATRow("FILE_ID") = _FILE_ID
                            PATRow("ISA_ID") = _ISA_ID
                            PATRow("GS_ID") = _GS_ID
                            PATRow("ST_ID") = _ST_ID
                            PATRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            PATRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            PATRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            PATRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            PATRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            PATRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            PATRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            PATRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            PATRow("837_CLM_GUID") = _837_CLM_GUID
                            PATRow("837_LX_GUID") = _837_LX_GUID
                            PATRow("HL01") = _HL01
                            PATRow("HL02") = _HL02
                            PATRow("HL03") = _HL03
                            PATRow("HL04") = _HL04

                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then PATRow("PAT01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else PATRow("PAT01") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then PATRow("PAT02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else PATRow("PAT02") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then PATRow("PAT03") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else PATRow("PAT03") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then PATRow("PAT04") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else PATRow("PAT04") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then PATRow("PAT05") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else PATRow("PAT05") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then PATRow("PAT06") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else PATRow("PAT06") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then PATRow("PAT07") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else PATRow("PAT07") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then PATRow("PAT08") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else PATRow("PAT08") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then PATRow("PAT09") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else PATRow("PAT09") = DBNull.Value

                            PATRow("ROW_NUMBER") = rowcount

                            PATRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            PATRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            PATRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                            PAT.Rows.Add(PATRow)



                        End If

                    Catch ex As Exception
                        log.ExceptionDetails("PAT 005010X223A2", ex)

                    End Try



                    'PER ******************************************************************************************************
                    Try
                        If oD.ediRowRecordType = "PER" Then

                            oD.RowProcessedFlag = 1

                            Dim PERRow As DataRow = PER.NewRow
                            PERRow("FILE_ID") = _FILE_ID
                            PERRow("ISA_ID") = _ISA_ID
                            PERRow("GS_ID") = _GS_ID
                            PERRow("ST_ID") = _ST_ID
                            PERRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            PERRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            PERRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            PERRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            PERRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            PERRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            PERRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            PERRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            PERRow("837_CLM_GUID") = _837_CLM_GUID
                            PERRow("837_LX_GUID") = _837_LX_GUID
                            PERRow("NM1_GUID") = _NM1_GUID
                            PERRow("HL01") = _HL01
                            PERRow("HL02") = _HL02
                            PERRow("HL03") = _HL03
                            PERRow("HL04") = _HL04


                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then PERRow("PER01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else PERRow("PER01") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then PERRow("PER02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else PERRow("PER02") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then PERRow("PER03") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else PERRow("PER03") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then PERRow("PER04") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else PERRow("PER04") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then PERRow("PER05") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else PERRow("PER05") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then PERRow("PER06") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else PERRow("PER06") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then PERRow("PER07") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else PERRow("PER07") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then PERRow("PER08") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else PERRow("PER08") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then PERRow("PER09") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else PERRow("PER09") = DBNull.Value

                            PERRow("ROW_NUMBER") = rowcount


                            PERRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            PERRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            PERRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                            PER.Rows.Add(PERRow)

                        End If
                    Catch ex As Exception
                        log.ExceptionDetails("PER 005010X223A2", ex)
                    End Try










                    ''PRV ******************************************************************************************************

                    'If oD.ediRowRecordType = "PRV" Then
                    '    Try
                    '        oD.RowProcessedFlag = 1

                    '        Dim PRVRow As DataRow = PRV.NewRow
                    '        PRVRow("DOCUMENT_ID") = _DOCUMENT_ID
                    '        PRVRow("FILE_ID") = _FILE_ID
                    '        PRVRow("ISA_ID") = _ISA_ID
                    '        PRVRow("GS_ID") = _GS_ID
                    '        PRVRow("ST_ID") = _ST_ID
                    '        PRVRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                    '        PRVRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                    '        PRVRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                    '        PRVRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                    '        PRVRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                    '        PRVRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                    '        PRVRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                    '        PRVRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                    '        PRVRow("837_CLM_GUID") = _837_CLM_GUID
                    '        PRVRow("837_LX_GUID") = _837_LX_GUID
                    '        PRVRow("HL01") = _HL01
                    '        PRVRow("HL02") = _HL02
                    '        PRVRow("HL03") = _HL03
                    '        PRVRow("HL04") = _HL04

                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then PRVRow("PRV01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else PRVRow("PRV01") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then PRVRow("PRV02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else PRVRow("PRV02") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then PRVRow("PRV03") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else PRVRow("PRV03") = DBNull.Value

                    '        PRVRow("ROW_NUMBER") = rowcount

                    '        PRVRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                    '        PRVRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                    '        PRVRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                    '        PRV.Rows.Add(PRVRow)
                    '    Catch ex As Exception

                    '    End Try




                    '  End If


                    'PWK ******************************************************************************************************
                    Try
                        If oD.ediRowRecordType = "PWK" Then

                            oD.RowProcessedFlag = 1

                            Dim PWKRow As DataRow = PWK.NewRow
                            PWKRow("DOCUMENT_ID") = _DOCUMENT_ID
                            PWKRow("FILE_ID") = _FILE_ID
                            PWKRow("ISA_ID") = _ISA_ID
                            PWKRow("GS_ID") = _GS_ID
                            PWKRow("ST_ID") = _ST_ID
                            PWKRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            PWKRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            PWKRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            PWKRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            PWKRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            PWKRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            PWKRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            PWKRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            PWKRow("837_CLM_GUID") = _837_CLM_GUID
                            PWKRow("837_LX_GUID") = _837_LX_GUID
                            PWKRow("HL01") = _HL01
                            PWKRow("HL02") = _HL02
                            PWKRow("HL03") = _HL03
                            PWKRow("HL04") = _HL04


                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then PWKRow("PWK01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else PWKRow("PWK01") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then PWKRow("PWK02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else PWKRow("PWK02") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then PWKRow("PWK03") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else PWKRow("PWK03") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then PWKRow("PWK04") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else PWKRow("PWK04") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then PWKRow("PWK05") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else PWKRow("PWK05") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then PWKRow("PWK06") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else PWKRow("PWK06") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then PWKRow("PWK07") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else PWKRow("PWK07") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then PWKRow("PWK08") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else PWKRow("PWK08") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then PWKRow("PWK09") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else PWKRow("PWK09") = DBNull.Value

                            PWKRow("ROW_NUMBER") = rowcount

                            PWKRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            PWKRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            PWKRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                            PWK.Rows.Add(PWKRow)



                        End If

                    Catch ex As Exception

                    End Try



                    'REF ******************************************************************************************************
                    Try
                        If oD.ediRowRecordType = "REF" Then
                            oD.RowProcessedFlag = 1

                            Dim REFRow As DataRow = REF.NewRow
                            REFRow("DOCUMENT_ID") = _DOCUMENT_ID
                            REFRow("FILE_ID") = _FILE_ID
                            REFRow("ISA_ID") = _ISA_ID
                            REFRow("GS_ID") = _GS_ID
                            REFRow("ST_ID") = _ST_ID
                            REFRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            REFRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            REFRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            REFRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            REFRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            REFRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            REFRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            REFRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            REFRow("837_CLM_GUID") = _837_CLM_GUID
                            REFRow("837_LX_GUID") = _837_LX_GUID
                            REFRow("NM1_GUID") = _NM1_GUID
                            REFRow("HL01") = _HL01
                            REFRow("HL02") = _HL02
                            REFRow("HL03") = _HL03
                            REFRow("HL04") = _HL04
                            ' REFRow("P_GUID") = oD.P_GUID


                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then REFRow("REF01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else REFRow("REF01") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then REFRow("REF02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else REFRow("REF02") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then REFRow("REF03") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else REFRow("REF03") = DBNull.Value

                            REFRow("ROW_NUMBER") = rowcount

                            REFRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            REFRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            REFRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                            REF.Rows.Add(REFRow)
                        End If

                    Catch ex As Exception
                        log.ExceptionDetails("REF 005010X223A2", ex)
                    End Try










                    'If oD.ediRowRecordType = "SV1" Then



                    '    SV101 = String.Empty

                    '    SV107 = String.Empty



                    '    oD.RowProcessedFlag = 1

                    '    Dim SV1Row As DataRow = SV1.NewRow
                    '    SV1Row("DOCUMENT_ID") = _DOCUMENT_ID
                    '    SV1Row("FILE_ID") = _FILE_ID
                    '    SV1Row("ISA_ID") = _ISA_ID
                    '    SV1Row("GS_ID") = _GS_ID
                    '    SV1Row("ST_ID") = _ST_ID
                    '    SV1Row("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                    '    SV1Row("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                    '    SV1Row("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                    '    SV1Row("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                    '    SV1Row("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                    '    SV1Row("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                    '    SV1Row("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                    '    SV1Row("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                    '    SV1Row("837_CLM_GUID") = _837_CLM_GUID
                    '    SV1Row("837_LX_GUID") = _837_LX_GUID
                    '    SV1Row("HL01") = _HL01
                    '    SV1Row("HL02") = _HL02
                    '    SV1Row("HL03") = _HL03
                    '    SV1Row("HL04") = _HL04

                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then
                    '        SV1Row("SV101") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2)
                    '        SV101 = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2)
                    '    Else
                    '        SV1Row("SV101") = DBNull.Value

                    '    End If

                    '    If Not SV101 = String.Empty Then

                    '        If (ss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 1) <> "") Then SV1Row("SV101_1") = ss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 1) Else SV1Row("SV101_1") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 2) <> "") Then SV1Row("SV101_2") = ss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 2) Else SV1Row("SV101_2") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 3) <> "") Then SV1Row("SV101_3") = ss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 3) Else SV1Row("SV101_3") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 4) <> "") Then SV1Row("SV101_4") = ss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 4) Else SV1Row("SV101_4") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 5) <> "") Then SV1Row("SV101_5") = ss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 5) Else SV1Row("SV101_5") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 6) <> "") Then SV1Row("SV101_6") = ss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 6) Else SV1Row("SV101_6") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 7) <> "") Then SV1Row("SV101_7") = ss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 7) Else SV1Row("SV101_7") = DBNull.Value
                    '    End If

                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then SV1Row("SV103") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else SV1Row("SV103") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then SV1Row("SV104") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else SV1Row("SV104") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then SV1Row("SV105") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else SV1Row("SV105") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then SV1Row("SV106") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else SV1Row("SV106") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then SV1Row("SV107") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else SV1Row("SV107") = DBNull.Value






                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then
                    '        SV1Row("SV107") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8)
                    '        SV101 = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8)
                    '    Else
                    '        SV1Row("SV107") = DBNull.Value

                    '    End If

                    '    If Not SV101 = String.Empty Then

                    '        If (ss.ParseDemlimtedString(SV107, oD.ComponentElementSeperator, 1) <> "") Then SV1Row("SV107_1") = ss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 1) Else SV1Row("SV107_1") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(SV107, oD.ComponentElementSeperator, 2) <> "") Then SV1Row("SV107_2") = ss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 2) Else SV1Row("SV107_2") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(SV107, oD.ComponentElementSeperator, 3) <> "") Then SV1Row("SV107_3") = ss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 3) Else SV1Row("SV107_3") = DBNull.Value
                    '        If (ss.ParseDemlimtedString(SV107, oD.ComponentElementSeperator, 4) <> "") Then SV1Row("SV107_4") = ss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 4) Else SV1Row("SV107_4") = DBNull.Value
                    '        '  If (ss.ParseDemlimtedString(SV107, oD.ComponentElementSeperator, 5) <> "") Then SV1Row("SV107_5") = ss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 5) Else SV1Row("SV107_5") = DBNull.Value
                    '        '  If (ss.ParseDemlimtedString(SV107, oD.ComponentElementSeperator, 6) <> "") Then SV1Row("SV107_6") = ss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 6) Else SV1Row("SV107_6") = DBNull.Value
                    '        '  If (ss.ParseDemlimtedString(SV107, oD.ComponentElementSeperator, 7) <> "") Then SV1Row("SV107_7") = ss.ParseDemlimtedString(SV101, oD.ComponentElementSeperator, 7) Else SV1Row("SV107_7") = DBNull.Value
                    '    End If





                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) <> "") Then SV1Row("SV108") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 9) Else SV1Row("SV108") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) <> "") Then SV1Row("SV109") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 10) Else SV1Row("SV109") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) <> "") Then SV1Row("SV110") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 11) Else SV1Row("SV110") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) <> "") Then SV1Row("SV111") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 12) Else SV1Row("SV111") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) <> "") Then SV1Row("SV112") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 13) Else SV1Row("SV112") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) <> "") Then SV1Row("SV113") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 14) Else SV1Row("SV113") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 15) <> "") Then SV1Row("SV114") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 15) Else SV1Row("SV114") = DBNull.Value
                    '    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 16) <> "") Then SV1Row("SV115") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 16) Else SV1Row("SV115") = DBNull.Value

                    '    SV1Row("ROW_NUMBER") = rowcount

                    '    SV1Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                    '    SV1Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                    '    SV1Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                    '    SV1.Rows.Add(SV1Row)

                    'End If


                    'SV2 ******************************************************************************************************
                    Try
                        If oD.ediRowRecordType = "SV2" Then

                            SV202 = String.Empty
                            oD.RowProcessedFlag = 1

                            Dim SV2Row As DataRow = SV2.NewRow
                            SV2Row("DOCUMENT_ID") = _DOCUMENT_ID
                            SV2Row("FILE_ID") = _FILE_ID
                            SV2Row("ISA_ID") = _ISA_ID
                            SV2Row("GS_ID") = _GS_ID
                            SV2Row("ST_ID") = _ST_ID
                            SV2Row("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            SV2Row("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            SV2Row("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            SV2Row("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            SV2Row("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            SV2Row("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            SV2Row("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            SV2Row("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            SV2Row("837_CLM_GUID") = _837_CLM_GUID
                            SV2Row("837_LX_GUID") = _837_LX_GUID
                            SV2Row("HL01") = _HL01
                            SV2Row("HL02") = _HL02
                            SV2Row("HL03") = _HL03
                            SV2Row("HL04") = _HL04

                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then SV2Row("SV201") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else SV2Row("SV201") = DBNull.Value


                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then
                                SV2Row("SV202") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3)
                                SV202 = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3)
                            Else
                                SV2Row("SV202") = DBNull.Value

                            End If

                            If Not SV202 = String.Empty Then

                                If (ss.ParseDemlimtedString(SV202, oD.ComponentElementSeperator, 1) <> "") Then SV2Row("SV202_1") = ss.ParseDemlimtedString(SV202, oD.ComponentElementSeperator, 1) Else SV2Row("SV202_1") = DBNull.Value
                                If (ss.ParseDemlimtedString(SV202, oD.ComponentElementSeperator, 2) <> "") Then SV2Row("SV202_2") = ss.ParseDemlimtedString(SV202, oD.ComponentElementSeperator, 2) Else SV2Row("SV202_2") = DBNull.Value
                                If (ss.ParseDemlimtedString(SV202, oD.ComponentElementSeperator, 3) <> "") Then SV2Row("SV202_3") = ss.ParseDemlimtedString(SV202, oD.ComponentElementSeperator, 3) Else SV2Row("SV202_3") = DBNull.Value
                                If (ss.ParseDemlimtedString(SV202, oD.ComponentElementSeperator, 4) <> "") Then SV2Row("SV202_4") = ss.ParseDemlimtedString(SV202, oD.ComponentElementSeperator, 4) Else SV2Row("SV202_4") = DBNull.Value
                                If (ss.ParseDemlimtedString(SV202, oD.ComponentElementSeperator, 5) <> "") Then SV2Row("SV202_5") = ss.ParseDemlimtedString(SV202, oD.ComponentElementSeperator, 5) Else SV2Row("SV202_5") = DBNull.Value
                                If (ss.ParseDemlimtedString(SV202, oD.ComponentElementSeperator, 6) <> "") Then SV2Row("SV202_6") = ss.ParseDemlimtedString(SV202, oD.ComponentElementSeperator, 6) Else SV2Row("SV202_6") = DBNull.Value
                                If (ss.ParseDemlimtedString(SV202, oD.ComponentElementSeperator, 7) <> "") Then SV2Row("SV202_7") = ss.ParseDemlimtedString(SV202, oD.ComponentElementSeperator, 7) Else SV2Row("SV202_7") = DBNull.Value
                            End If



                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then SV2Row("SV202") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else SV2Row("SV202") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then SV2Row("SV203") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) Else SV2Row("SV203") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then SV2Row("SV204") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else SV2Row("SV204") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then SV2Row("SV205") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else SV2Row("SV205") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then SV2Row("SV206") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else SV2Row("SV206") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) <> "") Then SV2Row("SV207") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 8) Else SV2Row("SV207") = DBNull.Value

                            SV2Row("ROW_NUMBER") = rowcount

                            SV2Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            SV2Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            SV2Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                            SV2.Rows.Add(SV2Row)
                        End If

                    Catch ex As Exception
                        log.ExceptionDetails("SV2 005010X223A2", ex)
                    End Try



                    'SVD ******************************************************************************************************
                    Try
                        If oD.ediRowRecordType = "SVD" Then
                            _LoopLevelMajor = 2430

                            SVD03 = String.Empty
                            Dim SVDRow As DataRow = SVD.NewRow
                            SVDRow("DOCUMENT_ID") = _DOCUMENT_ID
                            SVDRow("FILE_ID") = _FILE_ID
                            SVDRow("GS_ID") = _GS_ID
                            SVDRow("ST_ID") = _ST_ID
                            SVDRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            SVDRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            SVDRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            SVDRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            SVDRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            SVDRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            SVDRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            SVDRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            SVDRow("837_CLM_GUID") = _837_CLM_GUID
                            SVDRow("837_LX_GUID") = _837_LX_GUID
                            SVDRow("HL01") = _HL01
                            SVDRow("HL02") = _HL02
                            SVDRow("HL03") = _HL03
                            SVDRow("HL04") = _HL04

                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then SVDRow("SVD01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else SVDRow("SVD01") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then SVDRow("SVD02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else SVDRow("SVD02") = DBNull.Value




                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4) <> "") Then
                                SVDRow("SVD03") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4)
                                SVD03 = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 4)
                            Else
                                SVDRow("SVD03") = DBNull.Value

                            End If

                            If Not SVD03 = String.Empty Then

                                If (ss.ParseDemlimtedString(SVD03, oD.ComponentElementSeperator, 1) <> "") Then SVDRow("SVD03_01") = ss.ParseDemlimtedString(SVD03, oD.ComponentElementSeperator, 1) Else SVDRow("SVD03_01") = DBNull.Value
                                If (ss.ParseDemlimtedString(SVD03, oD.ComponentElementSeperator, 2) <> "") Then SVDRow("SVD03_02") = ss.ParseDemlimtedString(SVD03, oD.ComponentElementSeperator, 2) Else SVDRow("SVD03_02") = DBNull.Value
                                If (ss.ParseDemlimtedString(SVD03, oD.ComponentElementSeperator, 3) <> "") Then SVDRow("SVD03_03") = ss.ParseDemlimtedString(SVD03, oD.ComponentElementSeperator, 3) Else SVDRow("SVD03_03") = DBNull.Value
                                If (ss.ParseDemlimtedString(SVD03, oD.ComponentElementSeperator, 4) <> "") Then SVDRow("SVD03_04") = ss.ParseDemlimtedString(SVD03, oD.ComponentElementSeperator, 4) Else SVDRow("SVD03_04") = DBNull.Value
                                If (ss.ParseDemlimtedString(SVD03, oD.ComponentElementSeperator, 5) <> "") Then SVDRow("SVD03_05") = ss.ParseDemlimtedString(SVD03, oD.ComponentElementSeperator, 5) Else SVDRow("SVD03_05") = DBNull.Value
                                If (ss.ParseDemlimtedString(SVD03, oD.ComponentElementSeperator, 6) <> "") Then SVDRow("SVD03_06") = ss.ParseDemlimtedString(SVD03, oD.ComponentElementSeperator, 6) Else SVDRow("SVD03_06") = DBNull.Value
                                If (ss.ParseDemlimtedString(SVD03, oD.ComponentElementSeperator, 7) <> "") Then SVDRow("SVD03_07") = ss.ParseDemlimtedString(SVD03, oD.ComponentElementSeperator, 7) Else SVDRow("SVD03_07") = DBNull.Value

                            End If





                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) <> "") Then SVDRow("SVD04") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 5) Else SVDRow("SVD04") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) <> "") Then SVDRow("SVD05") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 6) Else SVDRow("SVD05") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) <> "") Then SVDRow("SVD06") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 7) Else SVDRow("SVD06") = DBNull.Value

                            SVDRow("ROW_NUMBER") = rowcount

                            SVDRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            SVDRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            SVDRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                            SVD.Rows.Add(SVDRow)
                            oD.RowProcessedFlag = 1
                        End If


                    Catch ex As Exception

                        log.ExceptionDetails("SVD 005010X223A2", ex)

                    End Try







                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '      END adding LX tot he LX List
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''




                    'SE ******************************************************************************************************
                    Try
                        If oD.ediRowRecordType = "SE" Then




                            Dim SERow As DataRow = SE.NewRow
                            SERow("HIPAA_ISA_GUID") = _ISA_GUID
                            SERow("HIPAA_GS_GUID") = _GS_GUID
                            SERow("HIPAA_ST_GUID") = _ST_GUID

                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then SERow("SE01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else SERow("SE01") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then SERow("SE02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else SERow("SE02") = DBNull.Value

                            SERow("ROW_NUMBER") = rowcount
                            SE.Rows.Add(SERow)

                            ' COMMIT THE LAST LK SET SINCE WE WONT GET TO lx AGAIN AND THE LAST clm SET
                            '  ComitLX()
                            ComitRowData()
                            ComitSE()

                            ClearST()
                            ClearLoop1000()

                            oD.RowProcessedFlag = 1


                        End If
                    Catch ex As Exception
                        log.ExceptionDetails("SE 005010X223A2", ex)
                    End Try



                    'GE ******************************************************************************************************
                    Try
                        If oD.ediRowRecordType = "GE" Then
                            oD.RowProcessedFlag = 1


                            Try



                                Dim GERow As DataRow = GE.NewRow
                                GERow("HIPAA_ISA_GUID") = _ISA_GUID
                                GERow("HIPAA_GS_GUID") = _GS_GUID


                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then GERow("GE01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else GERow("GE01") = DBNull.Value
                                If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then GERow("GE02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else GERow("GE02") = DBNull.Value

                                GERow("ROW_NUMBER") = rowcount
                                GE.Rows.Add(GERow)


                                ComitGE()
                                ClearGS()
                                'committ the last ST
                            Catch ex As Exception


                            End Try
                            'oD.GSFlag = 0
                            'oD.GS_GUID = Guid.Empty


                        End If
                    Catch ex As Exception
                        log.ExceptionDetails("GE 005010X223A2", ex)
                    End Try



                    'IEA ******************************************************************************************************
                    Try
                        If oD.ediRowRecordType = "IEA" Then

                            Dim IEARow As DataRow = IEA.NewRow
                            IEARow("HIPAA_ISA_GUID") = _ISA_GUID


                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> "") Then IEARow("IEA01") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) Else IEARow("IEA01") = DBNull.Value
                            If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) <> "") Then IEARow("IEA02") = ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 3) Else IEARow("IEA02") = DBNull.Value

                            IEARow("ROW_NUMBER") = rowcount
                            IEA.Rows.Add(IEARow)


                            ComitIEA()
                            ClearIAS()


                        End If
                    Catch ex As Exception
                        log.ExceptionDetails("IEA 005010X223A2", ex)
                    End Try

                Loop

                'Console.WriteLine(last)
                'Console.WriteLine(rowcount)

                'Catch ex As Exception
                '    _ImportReturnCode = 30
                '    oD.IEAFlag = 1
                '    RollBack()
                '    log.ExceptionDetails(oD._BatchID.ToString, "DCSGlobal.EDI.Import837I was rolled back due to Error in main loop " + oD.ediRowRecordType + "Rowcount " + Convert.ToString(rowcount))
                '    log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import837 error in main loop" + FileToParse, ex)

                'Finally
                ''  COMITUNK()

                'End Try



            End Using






 



            _ProcessEndTime = DateTime.Now

            Cleanup()

            Return _ImportReturnCode

        End Function





        Private Function Cleanup() As Int32
            Dim r As Integer = 0

            Dim FinalPath As String = String.Empty

            Try

                Dim span As TimeSpan

                span = _ProcessEndTime - _ProcessStartTime
                '  _ProcessElaspedTime = span.TotalMilliseconds

            Catch ex As Exception
                log.ExceptionDetails("CLEANUP 005010X223A2", ex)
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

                    _EDIFileData.FileID = Convert.ToInt32(_FILE_ID)
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

                    _EDIFileData.FileID = Convert.ToInt32(_FILE_ID)
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

                    _EDIFileData.FileID = Convert.ToInt32(_FILE_ID)
                    _EDIFileData.ProcessEndTime = _ProcessEndTime
                    _EDIFileData.ProcessStartTime = _ProcessStartTime
                    _EDIFileData.ProcessElaspedTime = _ProcessElaspedTime
                    ' _EDIFileData.FinalPath =
                    _EDIFileData.FinalPath = FinalPath
                    _EDIFileData.ResultCode = _ImportReturnCode
                    _EDIFileData.RowCount = _RowCount
                    _EDIFileData.isValid = _isValid
                    _EDIFileData.isComplete = 1
                    _EDIFileData.UpdateEDIFile()

                Case Else


            End Select

            ClearIAS()
            ClearGS()
            ClearST()
            ClearLoop1000()

            'reset all the guids



            ' reset all the flags
            STFlag = 0
            LXFlag = 0
            CLPFlag = 0
            FileID = 0


            'reset all the vars
            _HSDCount = 0
            _FileToParse = String.Empty



            ' rowcount = 0
            _chars = String.Empty


            _CLMString = 0
            CLMFlag = 0
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

            SV202 = String.Empty

            CTP05 = String.Empty


            oD.EBCarrotCHAR = String.Empty
            oD.CarrotDataDelimiter = String.Empty
            oD.ComponentElementSeperator = String.Empty
            oD.DataElementSeparator = String.Empty

            oD._BatchID = 0
            oD.ediRowRecordType = String.Empty
            oD.CurrentRowData = String.Empty
            Return r
        End Function


        Private Function ComitRowData() As Integer
            Dim i As Integer = -1
            Dim param As New SqlParameter()


            Dim sqlConn As SqlConnection = New SqlConnection
            Dim cmd As SqlCommand
            Dim sqlString As String


            sqlConn.ConnectionString = oD._ConnectionString
            sqlConn.Open()

            Try

                'sqlString = "usp_EDI_837_CLM2_test_DNU"

                sqlString = "usp_EDI_837_CLM3"


                cmd = New SqlCommand(sqlString, sqlConn)
                cmd.CommandType = CommandType.StoredProcedure

                cmd.Parameters.AddWithValue("@HIPAA_837_SBR", SBR)
                cmd.Parameters.AddWithValue("@HIPAA_837_HL", HL)
                cmd.Parameters.AddWithValue("@HIPAA_837_CLM", CLM)
                ' ''*     
                cmd.Parameters.AddWithValue("@HIPAA_837_LX", LX)

                cmd.Parameters.AddWithValue("@HIPAA_837_AMT", AMT)

                cmd.Parameters.AddWithValue("@HIPAA_837_CAS", CAS)
                cmd.Parameters.AddWithValue("@HIPAA_837_CL1", CL1)

                cmd.Parameters.AddWithValue("@HIPAA_837_DMG", DMG)
                cmd.Parameters.AddWithValue("@HIPAA_837_DTP", DTP)
                ''* 
                cmd.Parameters.AddWithValue("@HIPAA_837_HI", HI)
                ''* 
                cmd.Parameters.AddWithValue("@HIPAA_837_K3", K3)

                cmd.Parameters.AddWithValue("@HIPAA_837_MIA", MIA)
                cmd.Parameters.AddWithValue("@HIPAA_837_MOA", MOA)

                cmd.Parameters.AddWithValue("@HIPAA_837_N3", N3)
                cmd.Parameters.AddWithValue("@HIPAA_837_N4", N4)
                cmd.Parameters.AddWithValue("@HIPAA_837_NM1", NM1)
                cmd.Parameters.AddWithValue("@HIPAA_837_NTE", NTE)

                cmd.Parameters.AddWithValue("@HIPAA_837_OI", OI)

                cmd.Parameters.AddWithValue("@HIPAA_837_PAT", PAT)
                cmd.Parameters.AddWithValue("@HIPAA_837_PER", PER)
                cmd.Parameters.AddWithValue("@HIPAA_837_PWK", PWK)

                cmd.Parameters.AddWithValue("@HIPAA_837_REF", REF)

                cmd.Parameters.AddWithValue("@HIPAA_837_SV2", SV2)
                cmd.Parameters.AddWithValue("@HIPAA_837_SVD", SVD)

                Err = cmd.ExecuteNonQuery()


                i = 0

            Catch ex As Exception
                i = -1
                log.ExceptionDetails("COMIT_ROW_DATA 005010X223A2", ex)
            Finally

                sqlConn.Close()

                '  Environment.Exit(0)


            End Try
            ' Environment.Exit(0)
            ClearLoop1000()
            Return i
        End Function



        Private Function ComitST() As Int32


            Dim r As Integer


            Dim param As New SqlParameter()


            Dim sqlString As String
            sqlString = "usp_EDI_837_ST"



            Try



                Using Con As New SqlConnection(oD._ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_837_ST", ST)
                        cmd.Parameters.AddWithValue("@HIPAA_837_BHT", BHT)

                        cmd.Parameters.Add("@ST_ID", Data.SqlDbType.BigInt, 1)
                        cmd.Parameters("@ST_ID").Direction = ParameterDirection.Output



                        cmd.ExecuteNonQuery()


                        _ST_ID = Convert.ToInt32(cmd.Parameters("@ST_ID").Value.ToString())
                    End Using
                    Con.Close()
                End Using





            Catch ex As Exception
                log.ExceptionDetails("COMIT_ST_DATA 005010X223A2", ex)
            End Try

            Return r

        End Function


        Private Function ComitSE() As Int32


            Dim r As Integer


            Dim param As New SqlParameter()


            Dim sqlString As String
            sqlString = "usp_EDI_837_SE"



            Try



                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_837_SE", SE)

                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using





            Catch ex As Exception
                _ImportReturnCode = -1
                log.ExceptionDetails("COMIT_SE_DATA 005010X223A2", ex)
            Finally

            End Try





            Return r

        End Function

        Private Function ComitHeader() As Integer


            Dim i As Integer
            Dim sqlString As String

            Try


                sqlString = "usp_EDI_837_HEADERS"


                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)

                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.AddWithValue("@HIPAA_837_NM1", NM1)
                        cmd.Parameters.AddWithValue("@HIPAA_837_PER", PER)


                        cmd.ExecuteNonQuery()
                    End Using
                    Con.Close()
                End Using


            Catch ex As Exception
                i = -1

                log.ExceptionDetails("COMIT_HEADERS 005010X223A2", ex)

            End Try

            PER.Clear()
            NM1.Clear()

            Return i

        End Function


        Private Function ComitGS() As Integer


            Dim i As Integer
            Dim param As New SqlParameter()


            Dim sqlString As String

            Try


                sqlString = "usp_EDI_837_GS"



                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_837_GS", GS)


                        cmd.Parameters.Add("@GS_ID", Data.SqlDbType.BigInt, 1)
                        cmd.Parameters("@GS_ID").Direction = ParameterDirection.Output



                        cmd.ExecuteNonQuery()


                        _GS_ID = Convert.ToInt32(cmd.Parameters("@GS_ID").Value.ToString())


                    End Using
                    Con.Close()
                End Using


                i = 0

            Catch ex As Exception
                i = -1
                log.ExceptionDetails("COMIT_GS_DATA 005010X223A2", ex)
            Finally

                'sqlConn.Close()
            End Try

            Return i

        End Function


        Private Function ComitGE() As Integer


            Dim i As Integer
            Dim param As New SqlParameter()


            Dim sqlString As String

            Try


                sqlString = "usp_EDI_837_GE"



                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_837_GE", GE)

                        '    cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using
                i = 0

            Catch ex As Exception
                i = -1
                log.ExceptionDetails("COMIT_GE_DATA 005010X223A2", ex)
            End Try

            Return i

        End Function



        Private Function ComitISA() As Integer


            Dim i As Integer
            Dim param As New SqlParameter()


            Dim sqlString As String

            Try


                sqlString = "usp_EDI_837_ISA"



                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)

                        cmd.CommandType = CommandType.StoredProcedure


                        cmd.Parameters.AddWithValue("@HIPAA_837_ISA", ISA)




                        cmd.Parameters.Add("@ISA_ID", Data.SqlDbType.BigInt, 1)
                        cmd.Parameters("@ISA_ID").Direction = ParameterDirection.Output



                        cmd.ExecuteNonQuery()


                        _ISA_ID = Convert.ToInt32(cmd.Parameters("@ISA_ID").Value.ToString())
                    End Using
                    Con.Close()
                End Using




                i = 0

            Catch ex As Exception
                i = -1
                log.ExceptionDetails("COMIT_ISA_DATA 005010X223A2", ex)
            End Try

            Return i

        End Function



        Private Function ComitIEA() As Integer


            Dim i As Integer
            Dim param As New SqlParameter()


            Dim sqlString As String

            Try


                sqlString = "usp_EDI_837_IEA"



                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)

                        cmd.CommandType = CommandType.StoredProcedure


                        cmd.Parameters.AddWithValue("@HIPAA_837_IEA", IEA)
                        '     cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using




                i = 0

            Catch ex As Exception
                i = -1
                log.ExceptionDetails("COMIT_IEA_DATA 005010X223A2", ex)
            End Try

            Return i

        End Function

        Private Sub ClearIAS()
            ISA.Clear()
            IEA.Clear()
            _HIPAA_ISA_GUID = Guid.Empty
            _ISA_ID = 0
        End Sub


        Private Sub ClearGS()

            GS.Clear()
            GE.Clear()
            _HIPAA_GS_GUID = Guid.Empty
            _GS_ID = 0


        End Sub

        Private Sub ClearST()

            ST.Clear()
            SE.Clear()
            BHT.Clear()
            _ST_ID = 0
            _HIPAA_ST_GUID = Guid.Empty

        End Sub


        Private Sub ClearLoop1000()

            HL.Clear()

            AMT.Clear()

            CAS.Clear()
            CL1.Clear()
            CLM.Clear()

            DMG.Clear()
            DTP.Clear()

            HI.Clear()

            LX.Clear()

            K3.Clear()

            MIA.Clear()
            MOA.Clear()

            N3.Clear()
            N4.Clear()
            NM1.Clear()
            NTE.Clear()

            OI.Clear()

            PAT.Clear()
            PER.Clear()
            PWK.Clear()

            REF.Clear()

            SV2.Clear()
            SVD.Clear()


            _HIPAA_HL_20_GUID = Guid.Empty
            _HIPAA_HL_21_GUID = Guid.Empty
            _HIPAA_HL_22_GUID = Guid.Empty
            _HIPAA_HL_23_GUID = Guid.Empty
            _HIPAA_HL_24_GUID = Guid.Empty
            _837_CLM_GUID = Guid.Empty
            _837_LX_GUID = Guid.Empty
            _NM1_GUID = Guid.Empty
            _HL01 = 0
            _HL02 = 0
            _HL03 = 0
            _HL04 = 0

            _LoopLevelMajor = 1000
            _LoopLevelMinor = 0
            _LoopLevelSubFix = ""

        End Sub




        Private Function COMITUNK() As Integer


            'Dim i As Integer
            'Dim param As New SqlParameter()


            'Dim sqlString As String



            'Try


            '    sqlString = "[usp_EDI_837_UNK]"


            '    Using Con As New SqlConnection(oD._ConnectionString)
            '        Con.Open()
            '        Using cmd As New SqlCommand(sqlString, Con)

            '            cmd.CommandType = CommandType.StoredProcedure



            '            cmd.Parameters.AddWithValue("@HIPAA_837_UNK", UNK)
            '            cmd.Parameters.AddWithValue("@FILE_ID", oD._FileID)
            '            ' cmd.ExecuteNonQuery()

            '        End Using
            '        Con.Close()
            '    End Using




            '    i = 0

            'Catch ex As Exception
            '    i = -1

            '    log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import837i.rollback for bactch ID " + oD._BatchID.ToString, ex)

            'Finally

            '    'sqlConn.Close()
            'End Try

            'Return i



        End Function



        Private Function RollBack() As Integer


            Dim i As Integer
            Dim param As New SqlParameter()



            Dim sqlString As String




            Try


                sqlString = "[usp_EDI_837_ROLLBACK]"



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

                log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import837i.rollback for bactch ID " + oD._BatchID.ToString, ex)

            Finally

            End Try

            Return i



        End Function

        Public Function NM1Lookup(ByVal nm01 As String) As String
            Dim s As String = String.Empty

            Select nm01

                Case "40"
                    _LoopLevelMajor = 1000
                    _LoopLevelSubFix = "B"


                Case "41"
                    _LoopLevelMajor = 1000
                    _LoopLevelMinor = 0
                    _LoopLevelSubFix = "A"


                Case "71"
                    If _LoopLevelMajor = 2330 Then

                        _LoopLevelSubFix = "C"
                    Else
                        _LoopLevelMajor = 2310
                        _LoopLevelSubFix = "A"
                    End If

                Case "72"
                    If _LoopLevelMajor = 2330 Then

                        _LoopLevelSubFix = "D"
                    Else
                        _LoopLevelMajor = 2310
                        _LoopLevelSubFix = "B"
                    End If

                Case "77"

                    If _LoopLevelMajor = 2330 Then

                        _LoopLevelSubFix = "F"
                    Else
                        _LoopLevelMajor = 2310
                        _LoopLevelSubFix = "E"
                    End If

                Case "85"
                    _LoopLevelMajor = 2010
                    _LoopLevelSubFix = "AA"


                Case "87"
                    _LoopLevelMajor = 2010
                    _LoopLevelSubFix = "AB"


                Case "IL"

                    If _LoopLevelMajor = 2300 Then
                        _LoopLevelMajor = 2330
                        _LoopLevelSubFix = "A"
                    Else
                        _LoopLevelMajor = 2010
                        _LoopLevelSubFix = "BA"
                    End If



                Case "PR"

                    If _LoopLevelMajor = 2330 Then

                        _LoopLevelSubFix = "B"
                    Else
                        _LoopLevelMajor = 2010
                        _LoopLevelSubFix = "BB"
                    End If



                Case "QC"
                    _LoopLevelMajor = 2010
                    _LoopLevelSubFix = "CA"


                Case "ZZ"
                    _LoopLevelMajor = 2310
                    _LoopLevelSubFix = "C"


                Case Else





            End Select


            Return s


        End Function



    End Class
End Namespace
