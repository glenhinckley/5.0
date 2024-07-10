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


    Public Class ValidateEDI
        Implements IDisposable

        '      Inherits EDI837iTables
        Private _ClassVersion As String = "2.0"

        Private V As Boolean = False

        Private EDILbuilder As New EDI.EDIListBuilder
        Private RE As Int32 = 0
        Private _EDIList As New List(Of String)
        Private _ROW_NUM As Int32 = 0

        Private log As New logExecption
        Private oD As New Declarations
        Private ss As New StringStuff
        Private _isAK1 As Boolean = False
        Private _isAK9 As Boolean = False
        Private _isIK9 As Boolean

        Private _app As String = "Validate EDI"

        Public Event OnPreParse As EventHandler
        Public Event OnPostParse As EventHandler

        Private _1p As String


        Private _isTA1 As Boolean = False
        Private _ServiceTypeCode As String = String.Empty
        Private _NPI As String = String.Empty
        Private _AAAFailureCode As String = String.Empty
        Private _FileName As String = String.Empty
        Private _FilePath As String = String.Empty
        Private _InterchangeControlNumber As String = String.Empty
        Private _InterchangeSenderID As String = String.Empty
        Private _InterchangeReceiverID As String = String.Empty
        Private _InterchangeDate As String = String.Empty
        Private _InterchangeTime As String = String.Empty
        Private _ImplementationConventionReference As String = String.Empty
        Private _TransactionSetIdentifierCode As String = String.Empty
        Private _TransactionSetPurposeCode As String = String.Empty
        Private _InterchangeTrailer As String = String.Empty
        Private _FileLoadStatus As String = String.Empty
        Private _ProcessYearMonth As Int32 = 0
        Private _OverrideProcessYearMonth As String = String.Empty
        Private _ComponentElementSeperator As String = String.Empty
        Private _CarrotDataDelimiter As String = String.Empty
        Private _ReferenceIdentification As String = String.Empty


        Public Sub Dispose() Implements System.IDisposable.Dispose

            GC.SuppressFinalize(Me)

            ''  Console.WriteLine("Object " & GetHashCode() & " disposed.")
        End Sub

        Protected Overrides Sub Finalize()
            log = Nothing
            oD = Nothing
            ss = Nothing
            Dispose()
            '' Console.WriteLine("Object " & GetHashCode() & " finalized.")
        End Sub

        Public ReadOnly Property isTA1 As Boolean
            Get
                Return _isTA1
            End Get

        End Property



        Public Property ProcessYearMonth() As Integer
            Get
                Return _ProcessYearMonth

            End Get
            Set(ByVal value As Integer)
                _ProcessYearMonth = value
            End Set
        End Property

        Public Property OverrideProcessYearMonth() As String
            Get
                Return _OverrideProcessYearMonth

            End Get
            Set(ByVal value As String)
                _OverrideProcessYearMonth = value
            End Set
        End Property

        Public WriteOnly Property ConnectionString As String

            Set(value As String)
                oD._ConnectionString = value
                log.ConnectionString = value
            End Set
        End Property

        Public ReadOnly Property ServiceTypeCode As String
            Get
                Return _ServiceTypeCode
            End Get

        End Property


        Public ReadOnly Property ReferenceIdentification As String
            Get
                Return _ReferenceIdentification
            End Get

        End Property

        Public ReadOnly Property NPI As String
            Get
                Return _NPI
            End Get

        End Property

        Public ReadOnly Property AAAFailureCode As String
            Get
                Return _AAAFailureCode
            End Get

        End Property


        Public ReadOnly Property InterchangeControlNumber As String
            Get
                Return _InterchangeControlNumber
            End Get
        End Property

        Public ReadOnly Property InterchangeSenderID As String
            Get
                Return _InterchangeSenderID
            End Get
        End Property

        Public ReadOnly Property InterchangeReceiverID As String
            Get
                Return _InterchangeReceiverID
            End Get
        End Property

        Public ReadOnly Property InterchangeDate As String
            Get
                Return _InterchangeDate
            End Get
        End Property

        Public ReadOnly Property InterchangeTime As String
            Get
                Return _InterchangeTime
            End Get
        End Property

        Public ReadOnly Property ImplementationConventionReference As String
            Get
                Return _ImplementationConventionReference
            End Get
        End Property

        Public ReadOnly Property TransactionSetIdentifierCode As String
            Get
                Return _TransactionSetIdentifierCode
            End Get
        End Property

        Public ReadOnly Property TransactionSetPurposeCode As String
            Get
                Return _TransactionSetPurposeCode
            End Get
        End Property


        Public ReadOnly Property InterchangeTrailer As String
            Get
                Return _InterchangeTrailer
            End Get
        End Property



        Public ReadOnly Property CarrotDataDelimiter As String
            Get
                Return _CarrotDataDelimiter
            End Get
        End Property


        Public ReadOnly Property ComponentElementSeperator As String
            Get
                Return _ComponentElementSeperator
            End Get
        End Property





        Public Function byFile(ByVal FileName As String, ByVal ExpectedEDI As String) As Int32
            Return -99
        End Function

        Public Function IsValidProcessInterchangeDate_old(ByVal FilePath As String, ByVal FileName As String) As Boolean
            Dim bStatus As Boolean = False
            Dim last As String = String.Empty
            Dim line As String = String.Empty
            Dim rowcount As Int32 = 0
            Dim oD As New Declarations
            Dim FileLoadStatus As String = ""
            Dim clsValidateEDI As New ValidateEDI
            Dim iStatus As Integer = 0

            Dim InterchangeDate As String = ""
            Dim TransactionSetPurposeCode As String = ""
            Dim TransactionSetIdentifierCode As String = ""
            Dim ImplementationConventionReference As String = ""
            Dim InterchangeTrailer As String = ""
            Dim InterchangeSenderID As String = ""
            Dim InterchangeReceiverID As String = ""
            Dim InterchangeTime As String = ""
            Dim InterchangeControlNumber As String = ""

            'Public Function byFile(ByVal FilePath As String, ByVal FileName As String, ByVal ExpectedEDI As String) As Int32
            If _OverrideProcessYearMonth = "1" Then
                bStatus = True
                Return bStatus
            End If
            Try
                Dim RE As Int32 = 0
                If Not File.Exists(FilePath + FileName) Then
                    log.ExceptionDetails("DCSGlobal.EDI.ValidateEDI.byFile", FileName + " Does Not Exist ")
                    bStatus = False
                End If
                Using r As New StreamReader(FilePath + FileName)
                    oD.RowProcessedFlag = 0
                    line = r.ReadLine()
                    'Console.WriteLine(line)
                    oD.DataElementSeparator = Mid(line, 4, 1)
                    Do While (Not line Is Nothing)
                        ' Add this line to list.
                        last = line
                        line = line.Replace("~", "")
                        rowcount = rowcount + 1
                        oD.ediRowRecordType = ss.ParseDemlimtedStringEDI(line, oD.DataElementSeparator, 1)
                        oD.CurrentRowData = line
                        line = r.ReadLine

                        'This data is used for future purpose. Incase if we want to record this in insertfile method. For now only 3 paramters are passing to Insert File.
                        If oD.ediRowRecordType = "ISA" Then
                            InterchangeSenderID = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 7)
                            InterchangeReceiverID = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 9)
                            InterchangeDate = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 10)
                            InterchangeTime = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 11)
                            InterchangeControlNumber = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 14)
                        End If
                        If oD.ediRowRecordType = "BHT" Then
                            TransactionSetPurposeCode = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                        End If
                        If oD.ediRowRecordType = "ST" Then
                            TransactionSetIdentifierCode = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)
                            ImplementationConventionReference = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)
                        End If
                        If oD.ediRowRecordType = "IEA" Then
                            InterchangeTrailer = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                        End If
                    Loop
                    If _OverrideProcessYearMonth = "0" Then
                        Dim sProcessYearMonth = Convert.ToString(_ProcessYearMonth)
                        Dim sGetMonthYear = Convert.ToString(InterchangeDate).Substring(0, Len(InterchangeDate) - 2)
                        If sGetMonthYear.Trim() = sProcessYearMonth.Trim() Then
                            bStatus = True
                            Return bStatus
                        Else
                            FileLoadStatus = "ANON"


                            iStatus = clsValidateEDI.InsertFileName(oD._ConnectionString, FilePath, FileName, FileLoadStatus)
                            bStatus = False
                        End If
                    Else
                        bStatus = True  ' Continue reading all the files without checking InterchangeDate. Because of Override true, ignoring the check.
                        Return bStatus
                    End If
                End Using
            Catch ex As Exception
                log.ExceptionDetails(_app, ex)
            End Try
            Return bStatus
        End Function

        Public Function IsValidProcessInterchangeDate(ByVal FilePath As String, ByVal FileName As String) As Boolean
            Dim bStatus As Boolean = False
            Dim last As String = String.Empty
            Dim line As String = String.Empty
            Dim rowcount As Int32 = 0
            Dim oD As New Declarations

            Dim FileLoadStatus As String = ""
            Dim clsValidateEDI As New ValidateEDI
            Dim iStatus As Integer = 0

            Dim InterchangeDate As String = ""
            Dim TransactionSetPurposeCode As String = ""
            Dim TransactionSetIdentifierCode As String = ""
            Dim ImplementationConventionReference As String = ""
            Dim InterchangeTrailer As String = ""
            Dim InterchangeSenderID As String = ""
            Dim InterchangeReceiverID As String = ""
            Dim InterchangeTime As String = ""
            Dim InterchangeControlNumber As String = ""
            Dim sProcessYearMonth = Convert.ToString(_ProcessYearMonth)
            'Public Function byFile(ByVal FilePath As String, ByVal FileName As String, ByVal ExpectedEDI As String) As Int32
            Try
                Dim RE As Int32 = 0
                If Not File.Exists(FilePath + FileName) Then
                    log.ExceptionDetails("DCSGlobal.EDI.ValidateEDI.byFile", FileName + " Does Not Exist ")
                    bStatus = False
                End If
                'Here is the logic needs to change 
                Using r As New StreamReader(FilePath + FileName)
                    oD.RowProcessedFlag = 0
                    line = r.ReadLine()
                    'Console.WriteLine(line)
                    oD.DataElementSeparator = Mid(line, 4, 1)
                    Do While (Not line Is Nothing)
                        ' Add this line to list.
                        last = line
                        line = line.Replace("~", "")
                        rowcount = rowcount + 1
                        oD.ediRowRecordType = ss.ParseDemlimtedStringEDI(line, oD.DataElementSeparator, 1)
                        oD.CurrentRowData = line
                        line = r.ReadLine

                        'This data is used for future purpose. Incase if we want to record this in insertfile method. For now only 3 paramters are passing to Insert File.
                        If oD.ediRowRecordType = "ISA" Then
                            InterchangeSenderID = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 7)
                            InterchangeReceiverID = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 9)
                            InterchangeDate = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 10)
                            InterchangeTime = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 11)
                            InterchangeControlNumber = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 14)
                        End If
                        If oD.ediRowRecordType = "BHT" Then
                            TransactionSetPurposeCode = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                        End If
                        If oD.ediRowRecordType = "ST" Then
                            TransactionSetIdentifierCode = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)
                            ImplementationConventionReference = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)
                        End If
                        If oD.ediRowRecordType = "IEA" Then
                            InterchangeTrailer = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                        End If
                    Loop

                    If Len(sProcessYearMonth.Trim()) = 0 Then
                        bStatus = True
                        Return bStatus
                    Else
                        If Len(sProcessYearMonth.Trim()) = 4 AndAlso IsNumeric(sProcessYearMonth) Then
                            Dim sGetMonthYear = Convert.ToString(InterchangeDate).Substring(0, Len(InterchangeDate) - 2)
                            If sGetMonthYear.Trim() = sProcessYearMonth.Trim() Then
                                bStatus = True
                                Return bStatus
                            Else
                                FileLoadStatus = "ProcessYYMMNotMatch"
                                'In above 2 cases, we are not inserting FileName in this procedure. Insertion will take care 
                                'at the time of Importing..as BAU i.e in val.byFile under LoadFolder or LoadFolders.
                                iStatus = clsValidateEDI.InsertFileName(oD._ConnectionString, FilePath, FileName, FileLoadStatus)
                                bStatus = False
                            End If
                        Else
                            'Some other issues. Any file shold not fall under this category. Because we are already taking care in 
                            'Console loader - Program
                            FileLoadStatus = "FAIL"
                            bStatus = False
                            Return bStatus
                        End If
                    End If
                End Using
            Catch ex As Exception
                log.ExceptionDetails(_app, ex)
            End Try
            Return bStatus
        End Function


        Public Function byFile(ByVal FilePath As String, ByVal FileName As String, ByVal ExpectedEDI As String) As Int32

            _FileName = FileName
            _FilePath = FilePath
            _FileLoadStatus = ""
            Dim RE As Int32 = 0

            If Not File.Exists(_FilePath + _FileName) Then
                log.ExceptionDetails("70-DCSGlobal.EDI.ValidateEDI.byFile", _FileName + " Does Not Exist ")
                Return 1
                Exit Function
            End If


            Dim last As String = String.Empty
            Dim line As String = String.Empty
            Dim rowcount As Int32 = 0

            'so lets get down to it opn the file in a stream reader and move thru it one line at a time...........

            Using r As New StreamReader(_FilePath + _FileName)
                oD.RowProcessedFlag = 0
                line = r.ReadLine()
                'Console.WriteLine(line)
                oD.DataElementSeparator = Mid(line, 4, 1)

                'Console.WriteLine(oD.DataElementSeparator)

                '  Try
                Do While (Not line Is Nothing)
                    ' Add this line to list.
                    last = line



                    line = line.Replace("~", "")

                    rowcount = rowcount + 1
                    oD.ediRowRecordType = ss.ParseDemlimtedStringEDI(line, oD.DataElementSeparator, 1)
                    'Console.WriteLine(oD.ediRowRecordType)
                    oD.CurrentRowData = line


                    line = r.ReadLine

                    If oD.ediRowRecordType = "ISA" Then

                        _InterchangeSenderID = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 7)
                        _InterchangeReceiverID = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 9)
                        _InterchangeDate = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 10)
                        _InterchangeTime = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 11)
                        _InterchangeControlNumber = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 14)

                        oD.ISAFlag = 1
                    End If

                    If oD.ediRowRecordType = "GS" Then


                        'oD.GS01 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)
                        'oD.GS02 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                        'oD.GS03 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)
                        'oD.GS04 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 5)
                        'oD.GS05 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 6)
                        'oD.GS06 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 7)
                        'oD.GS07 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 8)
                        'oD.GS08 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 9)




                    End If




                    If oD.ediRowRecordType = "BHT" Then

                        _TransactionSetPurposeCode = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)





                    End If

                    If oD.ediRowRecordType = "ST" Then

                        _TransactionSetIdentifierCode = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)

                        _ImplementationConventionReference = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)

                    End If

                    If oD.ediRowRecordType = "AAA" Then

                        _AAAFailureCode = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                    End If



                    If oD.ediRowRecordType = "NM1" Then

                        _1p = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)


                        If (_1p = "1p" Or _1p = "80" Or _1p = "FA") Then

                            _NPI = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 10)
                        End If


                    End If



                    If oD.ediRowRecordType = "IEA" Then


                        _InterchangeTrailer = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)


                    End If


                    If oD.ediRowRecordType = "TA1" Then

                        _isTA1 = True


                    End If

                    If oD.ediRowRecordType = "AK1" Then

                        _isAK1 = True


                    End If

                    If oD.ediRowRecordType = "AK9" Then

                        _isAK9 = True


                    End If


                Loop



                If _InterchangeTrailer <> _InterchangeControlNumber Then
                    Return -2  'For moving a file to duplicate folder
                    Exit Function

                End If

                If _TransactionSetIdentifierCode = ExpectedEDI Or ExpectedEDI = "EDI" Then
                    _FileLoadStatus = "SUCCESS"
                    InsertFileName()
                Else
                    Return -3
                    Exit Function

                End If



            End Using










            Return RE

        End Function


        Public Function byFile(ByVal FileName As String) As Int32



            _FileName = FileName

            _FileLoadStatus = ""
            Dim RE As Int32 = 0

            If Not File.Exists(_FilePath + _FileName) Then
                log.ExceptionDetails("70-DCSGlobal.EDI.ValidateEDI.byFile", _FileName + " Does Not Exist ")
                Return 1
                Exit Function
            End If


            Dim last As String = String.Empty
            Dim line As String = String.Empty
            Dim rowcount As Int32 = 0

            'so lets get down to it opn the file in a stream reader and move thru it one line at a time...........

            Using r As New StreamReader(_FilePath + _FileName)
                oD.RowProcessedFlag = 0
                line = r.ReadLine()
                'Console.WriteLine(line)
                oD.DataElementSeparator = Mid(line, 4, 1)

                'Console.WriteLine(oD.DataElementSeparator)

                '  Try
                Do While (Not line Is Nothing)
                    ' Add this line to list.
                    last = line



                    line = line.Replace("~", "")

                    rowcount = rowcount + 1
                    oD.ediRowRecordType = ss.ParseDemlimtedStringEDI(line, oD.DataElementSeparator, 1)
                    'Console.WriteLine(oD.ediRowRecordType)
                    oD.CurrentRowData = line


                    line = r.ReadLine

                    If oD.ediRowRecordType = "ISA" Then

                        _InterchangeSenderID = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 7)
                        _InterchangeReceiverID = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 9)
                        _InterchangeDate = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 10)
                        _InterchangeTime = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 11)
                        _InterchangeControlNumber = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 14)

                        oD.ISAFlag = 1
                    End If

                    If oD.ediRowRecordType = "GS" Then


                        'oD.GS01 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)
                        'oD.GS02 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                        'oD.GS03 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)
                        'oD.GS04 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 5)
                        'oD.GS05 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 6)
                        'oD.GS06 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 7)
                        'oD.GS07 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 8)
                        'oD.GS08 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 9)




                    End If




                    If oD.ediRowRecordType = "BHT" Then

                        _TransactionSetPurposeCode = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)





                    End If

                    If oD.ediRowRecordType = "ST" Then

                        _TransactionSetIdentifierCode = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)

                        _ImplementationConventionReference = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)

                    End If

                    If oD.ediRowRecordType = "AAA" Then

                        _AAAFailureCode = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                    End If



                    If oD.ediRowRecordType = "NM1" Then

                        _1p = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)


                        If (_1p = "1p" Or _1p = "80" Or _1p = "FA") Then

                            _NPI = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 10)
                        End If


                    End If



                    If oD.ediRowRecordType = "IEA" Then


                        _InterchangeTrailer = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)


                    End If


                    If oD.ediRowRecordType = "TA1" Then

                        _isTA1 = True


                    End If

                    If oD.ediRowRecordType = "AK1" Then

                        _isAK1 = True


                    End If

                    If oD.ediRowRecordType = "AK9" Then

                        _isAK9 = True


                    End If


                Loop



                If _InterchangeTrailer <> _InterchangeControlNumber Then
                    Return -2  'For moving a file to duplicate folder
                    Exit Function

                End If

                'If _TransactionSetIdentifierCode = ExpectedEDI Or ExpectedEDI = "EDI" Then
                '    _FileLoadStatus = "SUCCESS"
                '    InsertFileName()
                'Else
                '    Return -3
                '    Exit Function

                'End If



            End Using










            Return RE

        End Function

        Public Function byString(ByVal EDI As String) As Int32

            Dim r As Integer = -1


            Using el As New EDIListBuilder

                el.ConnectionString = oD._ConnectionString
                r = el.BuildListByString(EDI)

                If r = 0 Then
                    _EDIList = el.EDIList
                End If
            End Using



            ValidateEDI()

            Return r
        End Function

        Public Function byList(ByVal EDIList As List(Of String), ByVal ExpectedEDI As String) As Int32

            Dim r = -1



            _EDIList = EDIList

            r = ValidateEDI()


            '_FileLoadStatus = ""
            'Dim RE As Int32 = 0
            '' Dim line As String = String.Empty
            'Dim last As String = String.Empty
            'Dim rowcount As Integer = 0



            'For Each line As String In EDIList

            '    oD.RowProcessedFlag = 0
            '    ' line = r.ReadLine()
            '    'Console.WriteLine(line)

            '    If oD.DataElementSeparatorFlag = 0 Then
            '        oD.DataElementSeparator = Mid(line, 4, 1)
            '        oD.DataElementSeparatorFlag = 1
            '    End If

            '    ' Add this line to list.
            '    last = line



            '    line = line.Replace("~", "")

            '    rowcount = rowcount + 1
            '    oD.ediRowRecordType = objss.ParseDemlimtedStringEDI(line, oD.DataElementSeparator, 1)
            '    'Console.WriteLine(oD.ediRowRecordType)
            '    oD.CurrentRowData = line




            '    If oD.ediRowRecordType = "ISA" Then

            '        _InterchangeSenderID = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 7)
            '        _InterchangeReceiverID = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 9)
            '        _InterchangeDate = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 10)
            '        _InterchangeTime = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 11)
            '        _InterchangeControlNumber = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 14)

            '        oD.ISAFlag = 1
            '    End If

            '    If oD.ediRowRecordType = "GS" Then


            '        'oD.GS01 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)
            '        'oD.GS02 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
            '        'oD.GS03 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)
            '        'oD.GS04 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 5)
            '        'oD.GS05 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 6)
            '        'oD.GS06 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 7)
            '        'oD.GS07 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 8)
            '        'oD.GS08 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 9)




            '    End If




            '    If oD.ediRowRecordType = "BHT" Then

            '        _TransactionSetPurposeCode = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)





            '    End If

            '    If oD.ediRowRecordType = "ST" Then

            '        _TransactionSetIdentifierCode = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)

            '        _ImplementationConventionReference = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)

            '    End If

            '    If oD.ediRowRecordType = "AAA" Then

            '        _AAAFailureCode = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
            '    End If



            '    If oD.ediRowRecordType = "NM1" Then

            '        _1p = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)


            '        If (_1p = "1P" Or _1p = "80" Or _1p = "FA") Then

            '            _NPI = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 10)
            '        End If


            '    End If


            '    If oD.ediRowRecordType = "EQ" Then
            '        If (objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> String.Empty) Then
            '            _ServiceTypeCode = _ServiceTypeCode + objss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) + oD.EBCarrotCHAR
            '        End If
            '    End If


            '    If oD.ediRowRecordType = "TA1" Then

            '        _isTA1 = True


            '    End If

            '    If oD.ediRowRecordType = "AK1" Then

            '        _isAK1 = True


            '    End If

            '    If oD.ediRowRecordType = "IK1" Then

            '        _isIK9 = True


            '    End If



            '    If oD.ediRowRecordType = "IEA" Then


            '        _InterchangeTrailer = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)


            '    End If


            'Next



            'If _InterchangeTrailer <> _InterchangeControlNumber Then
            '    Return -2  'For moving a file to duplicate folder
            '    Exit Function

            'End If

            'If _TransactionSetIdentifierCode = ExpectedEDI Or ExpectedEDI = "EDI" Then
            '    _FileLoadStatus = "SUCCESS"
            '    InsertFileName()
            'Else
            '    Return -3
            '    Exit Function

            'End If













            Return r

        End Function



        'Public Function getEDIRowDataVal(ByVal ExpectedEDI As String) As Integer
        '    Dim RE As Int32 = 0
        '    Try
        '        Dim _1p As String
        '        Dim NPI As String = String.Empty
        '        Dim idx_EDI271 As Int32 = 0
        '        Dim ediTildeCount As Integer
        '        Dim ediRowData As String
        '        Dim ediRowRecordType As String
        '        Dim ss As New StringStuff
        '        Dim retVal As String = String.Empty

        '        retVal = ss.ReplaceRowDelimiter(ExpectedEDI, "~")

        '        Using strReader As New StringReader(retVal)
        '            ediTildeCount = Regex.Matches(retVal, Regex.Escape("~")).Count 'InStr(nt(edi,'~')) 
        '            For idx_EDI271 = 1 To ediTildeCount
        '                ediRowData = objss.ParseDemlimtedStringEDI(retVal, oD.SegmentTerminator, idx_EDI271)
        '                ediRowRecordType = objss.ParseDemlimtedStringEDI(ediRowData, oD.DataElementSeparator, 1)
        '                'BEGIN AAA
        '                If ediRowRecordType = "AAA" Then
        '                    If (objss.ParseDemlimtedString(ediRowData, oD.DataElementSeparator, 4) <> "") Then
        '                        _AAAFailureCode = objss.ParseDemlimtedStringEDI(ediRowData, oD.DataElementSeparator, 4)
        '                    End If
        '                End If
        '                'BEGIN NM1
        '                If ediRowRecordType = "NM1" Then
        '                    _1p = objss.ParseDemlimtedStringEDI(ediRowData, oD.DataElementSeparator, 2)
        '                    If (_1p = "1P" Or _1p = "80" Or _1p = "FA") Then
        '                        _NPI = objss.ParseDemlimtedStringEDI(ediRowData, oD.DataElementSeparator, 10)
        '                    End If
        '                End If
        '            Next
        '        End Using

        '    Catch ex As Exception
        '        RE = -1
        '        log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.ValidateEDI.getEDIRowDataVal", ex)
        '    End Try
        '    Return RE
        'End Function







        ''' <summary>
        '''      This method is used for validate Appconfig ProcessYearMonth value with file InterchangeDate and if it not match insert this record with ANON status. i.e will process next run.
        ''' </summary>
        ''' <param name="ConnString"></param>
        ''' <param name="FilePath"></param>
        ''' <param name="FileName"></param>
        ''' <param name="FileLoadStatus"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function InsertFileName(ByVal ConnString As String, ByVal FilePath As String, ByVal FileName As String, ByVal FileLoadStatus As String) As Int32
            oD._ConnectionString = ConnString
            _FileLoadStatus = FileLoadStatus
            _FilePath = FilePath
            _FileName = FileName
            Dim rr As Int32
            Dim param As New SqlParameter()
            Dim sqlString As String
            sqlString = "usp_HIPAA_EDI_ADD_FILE"
            Try
                Using Con As New SqlConnection(oD._ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@ORGINAL_PATH", _FilePath)
                        cmd.Parameters.AddWithValue("@ORIGINAL_FILE_NAME", _FileName)
                        cmd.Parameters.AddWithValue("@FileLoadStatus", _FileLoadStatus)
                        cmd.Parameters.Add("@FILE_ID", Data.SqlDbType.BigInt)
                        cmd.Parameters("@FILE_ID").Direction = ParameterDirection.Output
                        rr = cmd.ExecuteNonQuery()
                        oD.Status = cmd.Parameters("@FILE_ID").Value.ToString()
                    End Using

                End Using
            Catch sx As SqlException
                log.ExceptionDetails("71-" + oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.InsertFileName", sx)
            Catch ex As Exception
                rr = -1
                log.ExceptionDetails("72-" + oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.InsertFileName", ex)

            Finally

            End Try
            Return rr
        End Function





        'Public Function UpdateFileStatus(FileID)

        'End Function













        Private Function InsertFileName() As Integer


            Dim rr As Int32


            Try



                Using Con As New SqlConnection(oD._ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand("usp_HIPAA_EDI_ADD_FILE", Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@ORGINAL_PATH", _FilePath)
                        cmd.Parameters.AddWithValue("@ORIGINAL_FILE_NAME", _FileName)
                        cmd.Parameters.AddWithValue("@TransactionSetIdentifierCode", _TransactionSetIdentifierCode)
                        cmd.Parameters.AddWithValue("@InterchangeControlNumber", _InterchangeControlNumber)
                        cmd.Parameters.AddWithValue("@TransactionSetPurposeCode", _TransactionSetPurposeCode)
                        cmd.Parameters.AddWithValue("@InterchangeDate", _InterchangeDate)
                        cmd.Parameters.AddWithValue("@InterchangeTime", _InterchangeTime)

                        cmd.Parameters.AddWithValue("@InterchangeSenderID", _InterchangeSenderID)
                        cmd.Parameters.AddWithValue("@InterchangeReceiverID", _InterchangeReceiverID)

                        cmd.Parameters.AddWithValue("@isValid", 1)

                        cmd.Parameters.AddWithValue("@FileLoadStatus", _FileLoadStatus)


                        cmd.Parameters.Add("@FILE_ID", Data.SqlDbType.BigInt)
                        cmd.Parameters("@FILE_ID").Direction = ParameterDirection.Output



                        rr = cmd.ExecuteNonQuery()



                        oD.Status = cmd.Parameters("@FILE_ID").Value.ToString()

                    End Using

                End Using


            Catch sx As SqlException



                log.ExceptionDetails("73-" + oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.InsertFileName", sx)


            Catch ex As Exception
                rr = -1

                log.ExceptionDetails("74-" + oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.InsertFileName", ex)

            Finally


            End Try





            Return rr

        End Function

        ''' <summary>
        ''' Private function to validate edi 
        ''' </summary>
        ''' <returns>int 0 success</returns>
        ''' <remarks></remarks>
        Private Function ValidateEDI() As Integer

            Dim r As Integer = -1
            Dim FirstEQ As Boolean = False




            For Each line As String In _EDIList

                If oD.LineTemiatorFlag = 0 Then
                    '''Commented below line- 081/5/2018 ; Issue - client-- Dallas children
                    ''' select * from  [Claim_Status_AUDIT_RAW_RESPONSE] where id > 1071039 AND INSERT_TS='2018-08-14 16:30:07.850' 
                    ''' the above EDI has less than 106 for ISA segment line.
                    'oD.LineTemiator = Mid(line, 106, 1)
                    oD.LineTemiator = Mid(line, Len(line), 1)
                    oD.LineTemiatorFlag = 1
                End If

                If oD.DataElementSeparatorFlag = 0 Then
                    oD.DataElementSeparator = Mid(line, 4, 1)
                    oD.DataElementSeparatorFlag = 1
                End If











                line = line.Replace(oD.LineTemiator, "")


                oD.ediRowRecordType = ss.ParseDemlimtedStringEDI(line, oD.DataElementSeparator, 1)
                'Console.WriteLine(oD.ediRowRecordType)
                oD.CurrentRowData = line




                If oD.ediRowRecordType = "ISA" Then

                    _InterchangeSenderID = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 7)
                    _InterchangeReceiverID = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 9)
                    _InterchangeDate = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 10)
                    _InterchangeTime = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 11)
                    _CarrotDataDelimiter = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 12)
                    _InterchangeControlNumber = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 14)
                    _ComponentElementSeperator = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 17)


                    oD.ISAFlag = 1
                End If

                If oD.ediRowRecordType = "GS" Then
                    'oD.GS01 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)
                    'oD.GS02 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                    'oD.GS03 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)
                    'oD.GS04 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 5)
                    'oD.GS05 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 6)
                    'oD.GS06 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 7)
                    'oD.GS07 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 8)
                    'oD.GS08 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 9)


                End If




                If oD.ediRowRecordType = "BHT" Then

                    _TransactionSetPurposeCode = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)


                    _ReferenceIdentification = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)


                End If


                If oD.ediRowRecordType = "TA1" Then

                    _isTA1 = True


                End If

                If oD.ediRowRecordType = "ST" Then

                    _TransactionSetIdentifierCode = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)

                    _ImplementationConventionReference = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)

                End If

                If oD.ediRowRecordType = "AAA" Then

                    _AAAFailureCode = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)
                End If



                If oD.ediRowRecordType = "NM1" Then

                    _1p = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)


                    If (_1p = "1P" Or _1p = "80" Or _1p = "FA") Then

                        _NPI = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 10)
                    End If


                End If


                If oD.ediRowRecordType = "EQ" Then
                    If (ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2) <> String.Empty) Then


                        If (FirstEQ = False) Then
                            FirstEQ = True
                            _ServiceTypeCode = _ServiceTypeCode + ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2)

                        Else

                            _ServiceTypeCode = _ServiceTypeCode + _CarrotDataDelimiter + ss.ParseDemlimtedString(oD.CurrentRowData, oD.DataElementSeparator, 2)
                        End If




                    End If
                End If



                If oD.ediRowRecordType = "IEA" Then


                    _InterchangeTrailer = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)


                End If
            Next



            If (_isTA1 = True) Then
                _TransactionSetIdentifierCode = "TA1"

            End If


            'XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
            '
            '  sanity checks go here
            '
            '
            '
            'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx





            Return r

        End Function


    End Class

End Namespace

