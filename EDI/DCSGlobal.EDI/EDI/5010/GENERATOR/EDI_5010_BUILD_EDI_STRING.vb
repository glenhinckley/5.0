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


    Public Class EDI_5010_BUILD_EDI_STRING

        Inherits EDI_5010_COMMON_DECS

        Implements IDisposable

        '******************************************************************************************************************
        '  all vars not declared here are in   EDI_5010_COMMON_DECS   it is inhreted from the tables for this type
        '  if you are going to use it on this class only it goes below if not put it in EDI_5010_COMMON_DECS
        '******************************************************************************************************************


        Private log As New logExecption
        Private ss As New StringStuff
        '   Private oD As New Declarations

        Public _ConnectionString As String = String.Empty
        Public _CommandTimeOut As Integer = 90







        Private FileLoadStatus As String = String.Empty
        Private iStatus As Integer = 0
        Private _isMissingSegments As Boolean = False
        Private _isAAA As Boolean = False


        Private _ProcessYearMonth As Integer = 0
        Private _OverrideProcessYearMonth As String = String.Empty





        Private _isAK1 As Boolean = False
        Private _isAK9 As Boolean = False
        Private _isIK9 As Boolean = False
        Private _isTA1 As Boolean = False

        Private _app As String = "Validate EDI"





        Private _AAAFailureCode As String = String.Empty



        Public Sub Dispose() Implements System.IDisposable.Dispose

            GC.SuppressFinalize(Me)

            ''  Console.WriteLine("Object " & GetHashCode() & " disposed.")
        End Sub

        Protected Overrides Sub Finalize()
            log = Nothing
            ss = Nothing
            Dispose()
            '' Console.WriteLine("Object " & GetHashCode() & " finalized.")
        End Sub

        Public ReadOnly Property isTA1 As Boolean
            Get
                Return _isTA1
            End Get

        End Property


        Public ReadOnly Property isAAA As Boolean
            Get
                Return _isAAA
            End Get

        End Property


        Public ReadOnly Property isMissingSegments As Boolean
            Get
                Return _isMissingSegments
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
                _ConnectionString = value
                log.ConnectionString = value
            End Set
        End Property

        'Public ReadOnly Property ServiceTypeCode As String
        '    Get
        '        Return _ServiceTypeCode
        '    End Get

        'End Property


        'Public ReadOnly Property ReferenceIdentification As String
        '    Get
        '        Return _ReferenceIdentification
        '    End Get

        'End Property


        Public ReadOnly Property RawEDI As String
            Get
                Return _RAW_EDI

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
                Return _ISA13_InterchangeControlNumber
            End Get
        End Property

        Public ReadOnly Property InterchangeSenderID As String
            Get
                Return _ISA06_InterchangeSenderID
            End Get
        End Property

        Public ReadOnly Property InterchangeReceiverID As String
            Get
                Return _ISA08_InterchangeReceiverID
            End Get
        End Property

        Public ReadOnly Property InterchangeDate As String
            Get
                Return _ISA09_InterchangeDate
            End Get
        End Property

        Public ReadOnly Property InterchangeTime As String
            Get
                Return _ISA10_InterchangeTime
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

        Public Property FileID As Integer
            Set(value As Integer)
                _FILE_ID = FileID
            End Set

            Get
                Return CInt(_FILE_ID)
            End Get
        End Property

        Public ReadOnly Property EDI_SEQUENCE_NUMBER As Integer


            Get
                Return _EDI_SEQUENCE_NUMBER
            End Get


        End Property

        Public Function byFile(ByVal FilePath As String, ByVal FileName As String) As Integer
            Dim r As Integer = -1
            Try


                Using el As New EDIListBuilder

                    el.ConnectionString = _ConnectionString
                    r = el.BuildListByFile(FilePath + FileName)

                    If r = 0 Then
                        _EDIList = el.EDIList
                    End If

                End Using

                r = Validate(_EDIList)


                Using e As New EDI_5010_LOGGING
                    e.ConnectionString = _ConnectionString
                    e.ImplementationConventionReference = _ImplementationConventionReference
                    e.TransactionSetIdentifierCode = _TransactionSetIdentifierCode
                    e.InterchangeControlNumber = _ISA13_InterchangeControlNumber
                    e.InterchangeTime = _ISA10_InterchangeTime
                    e.InterchangeDate = _ISA09_InterchangeDate
                    e.InterchangeSenderID = _ISA06_InterchangeSenderID
                    e.InterchangeReceiverID = _ISA08_InterchangeReceiverID

                    e.InsertFileName(FilePath, FileName)
                    '  _EDI_SEQUENCE_NUMBER = e.EDI_SEQUENCE_NUMBER
                    _FILE_ID = e.FileID
                End Using







            Catch ex As Exception

            End Try



            Return r


        End Function




        Public Function byList(ByVal EDIList As List(Of String), Optional ByVal HumanRedable As Boolean = True) As Int32
            Dim r As Integer = -1

            _RAW_EDI = String.Empty

            Try


                For Each line As String In EDIList
                    If HumanRedable Then
                        _RAW_EDI = _RAW_EDI + line + vbLf
                    Else
                        _RAW_EDI = _RAW_EDI + line

                    End If
                Next
                r = 0
            Catch ex As Exception
                r = -1
            End Try



            Return r
        End Function




        Public Function byString(ByVal EDI As String) As Int32
            Dim r As Integer = -1

            Using el As New EDIListBuilder

                el.ConnectionString = _ConnectionString
                r = el.BuildListByString(EDI)

                If r = 0 Then
                    _EDIList = el.EDIList
                    r = Validate(_EDIList)


                End If

            End Using


            Return r


        End Function

        Private Function Validate_Control_Group(ByVal EDIList As List(Of String)) As Integer

            Dim r As Integer = -1





            Try

                For Each line As String In EDIList
                    _RowProcessedFlag = 0

                    If _DataElementSeparatorFlag = 0 Then
                        _DataElementSeparator = Mid(line, 4, 1)
                        _DataElementSeparatorFlag = 1
                    End If


                    line = line.Replace("~", "")
                    _ROW_COUNT = _ROW_COUNT + 1

                    _RowRecordType = ss.ParseDemlimtedStringEDI(line, _DataElementSeparator, 1)
                    _CurrentRowData = line


                    If _RowRecordType = "ISA" Then

                        _ISA_COUNT = _ISA_COUNT + 1


                        _ISA01_AuthorizationInformationQualifier = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _ISA02_AuthorizationInformation = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _ISA03_SecurityInformationQualifier = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                        _ISA04_SecurityInformation = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 5)
                        _ISA05_InterchangeIDQualifierSender = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 6)
                        _ISA06_InterchangeSenderID = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 7)
                        _ISA07_InterchangeIDQualifierReceiver = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 8)
                        _ISA08_InterchangeReceiverID = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 9)
                        _ISA09_InterchangeDate = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 10)
                        _ISA10_InterchangeTime = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 11)
                        _RepetitionSeparator = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 12)
                        _ISA12_InterchangeControlVersionNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 13)
                        _ISA13_InterchangeControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 14)
                        _ISA14_AcknowledgmentRequested = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 15)
                        _ISA15_InterchangeUsageIndicator = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 16)
                        _ComponentElementSeparator = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 17)
                    End If




                    If _RowRecordType = "GS" Then
                        _GS_COUNT = _GS_COUNT + 1

                        _GS01_FunctionalIdentifierCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _GS02_FunctionalApplicationSendersCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _GS03_FunctionalApplicationReceiverCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                        _GS04_FunctionalDate = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 5)
                        _GS05_FunctionalTime = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 6)
                        _GS06_GroupControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 7)
                        _GS07_ResponsibleAgencyCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 8)
                        _ImplementationConventionReference = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 9)
                    End If


                    If _RowRecordType = "ST" Then

                        _ST_COUNT = _ST_COUNT + 1

                        _TransactionSetIdentifierCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _ST02_TransactionSetControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _ST03_ImplementationConventionReference = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                    End If

                    r = 0
                Next
            Catch ex As Exception

                log.ExceptionDetails(_app, ex)
            End Try

            Return r

        End Function

        Private Function Validate(ByVal EDIList As List(Of String)) As Integer

            Dim r As Integer = -1





            Try

                For Each line As String In EDIList
                    _RowProcessedFlag = 0

                    If _DataElementSeparatorFlag = 0 Then
                        _DataElementSeparator = Mid(line, 4, 1)
                        _DataElementSeparatorFlag = 1
                    End If


                    line = line.Replace("~", "")
                    _ROW_COUNT = _ROW_COUNT + 1

                    _RowRecordType = ss.ParseDemlimtedStringEDI(line, _DataElementSeparator, 1)
                    _CurrentRowData = line


                    If _RowRecordType = "ISA" Then

                        _ISA_COUNT = _ISA_COUNT + 1


                        _ISA01_AuthorizationInformationQualifier = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _ISA02_AuthorizationInformation = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _ISA03_SecurityInformationQualifier = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                        _ISA04_SecurityInformation = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 5)
                        _ISA05_InterchangeIDQualifierSender = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 6)
                        _ISA06_InterchangeSenderID = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 7)
                        _ISA07_InterchangeIDQualifierReceiver = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 8)
                        _ISA08_InterchangeReceiverID = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 9)
                        _ISA09_InterchangeDate = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 10)
                        _ISA10_InterchangeTime = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 11)
                        _RepetitionSeparator = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 12)
                        _ISA12_InterchangeControlVersionNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 13)
                        _ISA13_InterchangeControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 14)
                        _ISA14_AcknowledgmentRequested = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 15)
                        _ISA15_InterchangeUsageIndicator = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 16)
                        _ComponentElementSeparator = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 17)
                    End If




                    If _RowRecordType = "GS" Then
                        _GS_COUNT = _GS_COUNT + 1

                        _GS01_FunctionalIdentifierCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _GS02_FunctionalApplicationSendersCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _GS03_FunctionalApplicationReceiverCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                        _GS04_FunctionalDate = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 5)
                        _GS05_FunctionalTime = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 6)
                        _GS06_GroupControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 7)
                        _GS07_ResponsibleAgencyCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 8)
                        _ImplementationConventionReference = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 9)
                    End If


                    If _RowRecordType = "ST" Then

                        _ST_COUNT = _ST_COUNT + 1

                        _TransactionSetIdentifierCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _ST02_TransactionSetControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _ST03_ImplementationConventionReference = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                    End If


                    If _RowRecordType = "BHT" Then
                        _BHT01_HierarchicalStructureCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _BHT02_TransactionSetPurposeCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _BHT03_ReferenceIdentification = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                        _BHT04_Date = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 5)
                        _BHT05_Time = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 6)
                        _BHT06_TransactionTypeCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 7)
                        _BHT07_ResponsibleAgencyCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 8)
                        _BHT08_ImplementationConventionReference = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 9)
                    End If


                    If _RowRecordType = "AAA" Then
                        _isAAA = True
                        _AAAFailureCode = _AAAFailureCode + ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                    End If


                    If _RowRecordType = "NM1" Then
                        _1p = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        If (_1p = "1p" Or _1p = "80" Or _1p = "FA") Then
                            _NPI = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 10)
                        End If
                    End If

                    If _RowRecordType = "TA1" Then
                        _isTA1 = True
                    End If

                    If _RowRecordType = "AK1" Then
                        _isAK1 = True
                    End If

                    If _RowRecordType = "AK9" Then
                        _isAK9 = True
                    End If



                    If _RowRecordType = "SE" Then

                        _SE_COUNT = _SE_COUNT + 1

                        _SE01_NumberofIncludedSegments = Convert.ToInt32(ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2))
                        _SE02_TransactionSetControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)

                    End If

                    If _RowRecordType = "GE" Then

                        _GE_COUNT = _GE_COUNT + 1

                        _GE01_NumberofTransactionSetsIncluded = Convert.ToInt32(ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2))
                        _GE02_GroupControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                    End If


                    If _RowRecordType = "IEA" Then

                        _IEA_COUNT = _IEA_COUNT + 1

                        _IEA01_NumberofFunctionalSetsIncluded = Convert.ToInt32(ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2))
                        _IEA02_InterchangeControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                    End If

                Next


                If ((_ISA_COUNT - _IEA_COUNT) <> 0) Then
                    _isMissingSegments = True
                End If

                If ((_GS_COUNT - _GE_COUNT) <> 0) Then
                    _isMissingSegments = True
                End If

                If ((_ST_COUNT - _SE_COUNT) <> 0) Then
                    _isMissingSegments = True
                End If


                r = 0

            Catch ex As Exception

                log.ExceptionDetails(_app, ex)
            End Try

            Return r

        End Function

        Private Function Validate_5010_270(ByVal EDIList As List(Of String)) As Integer

            Dim r As Integer = -1





            Try

                For Each line As String In EDIList
                    _RowProcessedFlag = 0

                    If _DataElementSeparatorFlag = 0 Then
                        _DataElementSeparator = Mid(line, 4, 1)
                        _DataElementSeparatorFlag = 1
                    End If


                    line = line.Replace("~", "")
                    _ROW_COUNT = _ROW_COUNT + 1

                    _RowRecordType = ss.ParseDemlimtedStringEDI(line, _DataElementSeparator, 1)
                    _CurrentRowData = line


                    If _RowRecordType = "ISA" Then

                        _ISA_COUNT = _ISA_COUNT + 1


                        _ISA01_AuthorizationInformationQualifier = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _ISA02_AuthorizationInformation = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _ISA03_SecurityInformationQualifier = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                        _ISA04_SecurityInformation = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 5)
                        _ISA05_InterchangeIDQualifierSender = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 6)
                        _ISA06_InterchangeSenderID = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 7)
                        _ISA07_InterchangeIDQualifierReceiver = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 8)
                        _ISA08_InterchangeReceiverID = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 9)
                        _ISA09_InterchangeDate = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 10)
                        _ISA10_InterchangeTime = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 11)
                        _RepetitionSeparator = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 12)
                        _ISA12_InterchangeControlVersionNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 13)
                        _ISA13_InterchangeControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 14)
                        _ISA14_AcknowledgmentRequested = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 15)
                        _ISA15_InterchangeUsageIndicator = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 16)
                        _ComponentElementSeparator = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 17)
                    End If




                    If _RowRecordType = "GS" Then
                        _GS_COUNT = _GS_COUNT + 1

                        _GS01_FunctionalIdentifierCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _GS02_FunctionalApplicationSendersCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _GS03_FunctionalApplicationReceiverCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                        _GS04_FunctionalDate = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 5)
                        _GS05_FunctionalTime = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 6)
                        _GS06_GroupControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 7)
                        _GS07_ResponsibleAgencyCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 8)
                        _ImplementationConventionReference = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 9)
                    End If


                    If _RowRecordType = "ST" Then

                        _ST_COUNT = _ST_COUNT + 1

                        _TransactionSetIdentifierCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _ST02_TransactionSetControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _ST03_ImplementationConventionReference = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                    End If


                    If _RowRecordType = "BHT" Then
                        _BHT01_HierarchicalStructureCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _BHT02_TransactionSetPurposeCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _BHT03_ReferenceIdentification = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                        _BHT04_Date = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 5)
                        _BHT05_Time = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 6)
                        _BHT06_TransactionTypeCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 7)
                        _BHT07_ResponsibleAgencyCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 8)
                        _BHT08_ImplementationConventionReference = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 9)
                    End If


                    If _RowRecordType = "AAA" Then
                        _isAAA = True
                        _AAAFailureCode = _AAAFailureCode + ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                    End If


                    If _RowRecordType = "NM1" Then
                        _1p = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        If (_1p = "1p" Or _1p = "80" Or _1p = "FA") Then
                            _NPI = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 10)
                        End If
                    End If

                    If _RowRecordType = "TA1" Then
                        _isTA1 = True
                    End If

                    If _RowRecordType = "AK1" Then
                        _isAK1 = True
                    End If

                    If _RowRecordType = "AK9" Then
                        _isAK9 = True
                    End If



                    If _RowRecordType = "SE" Then

                        _SE_COUNT = _SE_COUNT + 1

                        _SE01_NumberofIncludedSegments = Convert.ToInt32(ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2))
                        _SE02_TransactionSetControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)

                    End If

                    If _RowRecordType = "GE" Then

                        _GE_COUNT = _GE_COUNT + 1

                        _GE01_NumberofTransactionSetsIncluded = Convert.ToInt32(ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2))
                        _GE02_GroupControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                    End If


                    If _RowRecordType = "IEA" Then

                        _IEA_COUNT = _IEA_COUNT + 1

                        _IEA01_NumberofFunctionalSetsIncluded = Convert.ToInt32(ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2))
                        _IEA02_InterchangeControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                    End If

                Next


                If ((_ISA_COUNT - _IEA_COUNT) <> 0) Then
                    _isMissingSegments = True
                End If

                If ((_GS_COUNT - _GE_COUNT) <> 0) Then
                    _isMissingSegments = True
                End If

                If ((_ST_COUNT - _SE_COUNT) <> 0) Then
                    _isMissingSegments = True
                End If


                r = 0

            Catch ex As Exception

                log.ExceptionDetails(_app, ex)
            End Try

            Return r

        End Function

        Private Function Validate_5010_271(ByVal EDIList As List(Of String)) As Integer

            Dim r As Integer = -1





            Try

                For Each line As String In EDIList
                    _RowProcessedFlag = 0

                    If _DataElementSeparatorFlag = 0 Then
                        _DataElementSeparator = Mid(line, 4, 1)
                        _DataElementSeparatorFlag = 1
                    End If


                    line = line.Replace("~", "")
                    _ROW_COUNT = _ROW_COUNT + 1

                    _RowRecordType = ss.ParseDemlimtedStringEDI(line, _DataElementSeparator, 1)
                    _CurrentRowData = line


                    If _RowRecordType = "ISA" Then

                        _ISA_COUNT = _ISA_COUNT + 1


                        _ISA01_AuthorizationInformationQualifier = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _ISA02_AuthorizationInformation = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _ISA03_SecurityInformationQualifier = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                        _ISA04_SecurityInformation = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 5)
                        _ISA05_InterchangeIDQualifierSender = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 6)
                        _ISA06_InterchangeSenderID = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 7)
                        _ISA07_InterchangeIDQualifierReceiver = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 8)
                        _ISA08_InterchangeReceiverID = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 9)
                        _ISA09_InterchangeDate = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 10)
                        _ISA10_InterchangeTime = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 11)
                        _RepetitionSeparator = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 12)
                        _ISA12_InterchangeControlVersionNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 13)
                        _ISA13_InterchangeControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 14)
                        _ISA14_AcknowledgmentRequested = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 15)
                        _ISA15_InterchangeUsageIndicator = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 16)
                        _ComponentElementSeparator = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 17)
                    End If




                    If _RowRecordType = "GS" Then
                        _GS_COUNT = _GS_COUNT + 1

                        _GS01_FunctionalIdentifierCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _GS02_FunctionalApplicationSendersCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _GS03_FunctionalApplicationReceiverCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                        _GS04_FunctionalDate = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 5)
                        _GS05_FunctionalTime = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 6)
                        _GS06_GroupControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 7)
                        _GS07_ResponsibleAgencyCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 8)
                        _ImplementationConventionReference = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 9)
                    End If


                    If _RowRecordType = "ST" Then

                        _ST_COUNT = _ST_COUNT + 1

                        _TransactionSetIdentifierCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _ST02_TransactionSetControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _ST03_ImplementationConventionReference = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                    End If


                    If _RowRecordType = "BHT" Then
                        _BHT01_HierarchicalStructureCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _BHT02_TransactionSetPurposeCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _BHT03_ReferenceIdentification = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                        _BHT04_Date = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 5)
                        _BHT05_Time = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 6)
                        _BHT06_TransactionTypeCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 7)
                        _BHT07_ResponsibleAgencyCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 8)
                        _BHT08_ImplementationConventionReference = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 9)
                    End If


                    If _RowRecordType = "AAA" Then
                        _isAAA = True
                        _AAAFailureCode = _AAAFailureCode + ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                    End If


                    If _RowRecordType = "NM1" Then
                        _1p = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        If (_1p = "1p" Or _1p = "80" Or _1p = "FA") Then
                            _NPI = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 10)
                        End If
                    End If

                    If _RowRecordType = "TA1" Then
                        _isTA1 = True
                    End If

                    If _RowRecordType = "AK1" Then
                        _isAK1 = True
                    End If

                    If _RowRecordType = "AK9" Then
                        _isAK9 = True
                    End If



                    If _RowRecordType = "SE" Then

                        _SE_COUNT = _SE_COUNT + 1

                        _SE01_NumberofIncludedSegments = Convert.ToInt32(ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2))
                        _SE02_TransactionSetControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)

                    End If

                    If _RowRecordType = "GE" Then

                        _GE_COUNT = _GE_COUNT + 1

                        _GE01_NumberofTransactionSetsIncluded = Convert.ToInt32(ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2))
                        _GE02_GroupControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                    End If


                    If _RowRecordType = "IEA" Then

                        _IEA_COUNT = _IEA_COUNT + 1

                        _IEA01_NumberofFunctionalSetsIncluded = Convert.ToInt32(ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2))
                        _IEA02_InterchangeControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                    End If

                Next


                If ((_ISA_COUNT - _IEA_COUNT) <> 0) Then
                    _isMissingSegments = True
                End If

                If ((_GS_COUNT - _GE_COUNT) <> 0) Then
                    _isMissingSegments = True
                End If

                If ((_ST_COUNT - _SE_COUNT) <> 0) Then
                    _isMissingSegments = True
                End If


                r = 0

            Catch ex As Exception

                log.ExceptionDetails(_app, ex)
            End Try

            Return r

        End Function

        Private Function Validate_5010_276(ByVal EDIList As List(Of String)) As Integer

            Dim r As Integer = -1





            Try

                For Each line As String In EDIList
                    _RowProcessedFlag = 0

                    If _DataElementSeparatorFlag = 0 Then
                        _DataElementSeparator = Mid(line, 4, 1)
                        _DataElementSeparatorFlag = 1
                    End If


                    line = line.Replace("~", "")
                    _ROW_COUNT = _ROW_COUNT + 1

                    _RowRecordType = ss.ParseDemlimtedStringEDI(line, _DataElementSeparator, 1)
                    _CurrentRowData = line


                    If _RowRecordType = "ISA" Then

                        _ISA_COUNT = _ISA_COUNT + 1


                        _ISA01_AuthorizationInformationQualifier = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _ISA02_AuthorizationInformation = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _ISA03_SecurityInformationQualifier = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                        _ISA04_SecurityInformation = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 5)
                        _ISA05_InterchangeIDQualifierSender = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 6)
                        _ISA06_InterchangeSenderID = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 7)
                        _ISA07_InterchangeIDQualifierReceiver = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 8)
                        _ISA08_InterchangeReceiverID = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 9)
                        _ISA09_InterchangeDate = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 10)
                        _ISA10_InterchangeTime = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 11)
                        _RepetitionSeparator = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 12)
                        _ISA12_InterchangeControlVersionNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 13)
                        _ISA13_InterchangeControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 14)
                        _ISA14_AcknowledgmentRequested = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 15)
                        _ISA15_InterchangeUsageIndicator = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 16)
                        _ComponentElementSeparator = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 17)
                    End If




                    If _RowRecordType = "GS" Then
                        _GS_COUNT = _GS_COUNT + 1

                        _GS01_FunctionalIdentifierCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _GS02_FunctionalApplicationSendersCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _GS03_FunctionalApplicationReceiverCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                        _GS04_FunctionalDate = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 5)
                        _GS05_FunctionalTime = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 6)
                        _GS06_GroupControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 7)
                        _GS07_ResponsibleAgencyCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 8)
                        _ImplementationConventionReference = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 9)
                    End If


                    If _RowRecordType = "ST" Then

                        _ST_COUNT = _ST_COUNT + 1

                        _TransactionSetIdentifierCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _ST02_TransactionSetControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _ST03_ImplementationConventionReference = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                    End If


                    If _RowRecordType = "BHT" Then
                        _BHT01_HierarchicalStructureCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _BHT02_TransactionSetPurposeCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _BHT03_ReferenceIdentification = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                        _BHT04_Date = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 5)
                        _BHT05_Time = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 6)
                        _BHT06_TransactionTypeCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 7)
                        _BHT07_ResponsibleAgencyCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 8)
                        _BHT08_ImplementationConventionReference = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 9)
                    End If


                    If _RowRecordType = "AAA" Then
                        _isAAA = True
                        _AAAFailureCode = _AAAFailureCode + ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                    End If


                    If _RowRecordType = "NM1" Then
                        _1p = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        If (_1p = "1p" Or _1p = "80" Or _1p = "FA") Then
                            _NPI = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 10)
                        End If
                    End If

                    If _RowRecordType = "TA1" Then
                        _isTA1 = True
                    End If

                    If _RowRecordType = "AK1" Then
                        _isAK1 = True
                    End If

                    If _RowRecordType = "AK9" Then
                        _isAK9 = True
                    End If



                    If _RowRecordType = "SE" Then

                        _SE_COUNT = _SE_COUNT + 1

                        _SE01_NumberofIncludedSegments = Convert.ToInt32(ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2))
                        _SE02_TransactionSetControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)

                    End If

                    If _RowRecordType = "GE" Then

                        _GE_COUNT = _GE_COUNT + 1

                        _GE01_NumberofTransactionSetsIncluded = Convert.ToInt32(ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2))
                        _GE02_GroupControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                    End If


                    If _RowRecordType = "IEA" Then

                        _IEA_COUNT = _IEA_COUNT + 1

                        _IEA01_NumberofFunctionalSetsIncluded = Convert.ToInt32(ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2))
                        _IEA02_InterchangeControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                    End If

                Next


                If ((_ISA_COUNT - _IEA_COUNT) <> 0) Then
                    _isMissingSegments = True
                End If

                If ((_GS_COUNT - _GE_COUNT) <> 0) Then
                    _isMissingSegments = True
                End If

                If ((_ST_COUNT - _SE_COUNT) <> 0) Then
                    _isMissingSegments = True
                End If


                r = 0

            Catch ex As Exception

                log.ExceptionDetails(_app, ex)
            End Try

            Return r

        End Function

        Private Function Validate_5010_277(ByVal EDIList As List(Of String)) As Integer

            Dim r As Integer = -1





            Try

                For Each line As String In EDIList
                    _RowProcessedFlag = 0

                    If _DataElementSeparatorFlag = 0 Then
                        _DataElementSeparator = Mid(line, 4, 1)
                        _DataElementSeparatorFlag = 1
                    End If


                    line = line.Replace("~", "")
                    _ROW_COUNT = _ROW_COUNT + 1

                    _RowRecordType = ss.ParseDemlimtedStringEDI(line, _DataElementSeparator, 1)
                    _CurrentRowData = line


                    If _RowRecordType = "ISA" Then

                        _ISA_COUNT = _ISA_COUNT + 1


                        _ISA01_AuthorizationInformationQualifier = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _ISA02_AuthorizationInformation = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _ISA03_SecurityInformationQualifier = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                        _ISA04_SecurityInformation = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 5)
                        _ISA05_InterchangeIDQualifierSender = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 6)
                        _ISA06_InterchangeSenderID = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 7)
                        _ISA07_InterchangeIDQualifierReceiver = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 8)
                        _ISA08_InterchangeReceiverID = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 9)
                        _ISA09_InterchangeDate = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 10)
                        _ISA10_InterchangeTime = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 11)
                        _RepetitionSeparator = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 12)
                        _ISA12_InterchangeControlVersionNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 13)
                        _ISA13_InterchangeControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 14)
                        _ISA14_AcknowledgmentRequested = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 15)
                        _ISA15_InterchangeUsageIndicator = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 16)
                        _ComponentElementSeparator = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 17)
                    End If




                    If _RowRecordType = "GS" Then
                        _GS_COUNT = _GS_COUNT + 1

                        _GS01_FunctionalIdentifierCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _GS02_FunctionalApplicationSendersCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _GS03_FunctionalApplicationReceiverCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                        _GS04_FunctionalDate = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 5)
                        _GS05_FunctionalTime = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 6)
                        _GS06_GroupControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 7)
                        _GS07_ResponsibleAgencyCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 8)
                        _ImplementationConventionReference = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 9)
                    End If


                    If _RowRecordType = "ST" Then

                        _ST_COUNT = _ST_COUNT + 1

                        _TransactionSetIdentifierCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _ST02_TransactionSetControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _ST03_ImplementationConventionReference = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                    End If


                    If _RowRecordType = "BHT" Then
                        _BHT01_HierarchicalStructureCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _BHT02_TransactionSetPurposeCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _BHT03_ReferenceIdentification = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                        _BHT04_Date = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 5)
                        _BHT05_Time = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 6)
                        _BHT06_TransactionTypeCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 7)
                        _BHT07_ResponsibleAgencyCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 8)
                        _BHT08_ImplementationConventionReference = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 9)
                    End If


                    If _RowRecordType = "AAA" Then
                        _isAAA = True
                        _AAAFailureCode = _AAAFailureCode + ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                    End If


                    If _RowRecordType = "NM1" Then
                        _1p = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        If (_1p = "1p" Or _1p = "80" Or _1p = "FA") Then
                            _NPI = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 10)
                        End If
                    End If

                    If _RowRecordType = "TA1" Then
                        _isTA1 = True
                    End If

                    If _RowRecordType = "AK1" Then
                        _isAK1 = True
                    End If

                    If _RowRecordType = "AK9" Then
                        _isAK9 = True
                    End If



                    If _RowRecordType = "SE" Then

                        _SE_COUNT = _SE_COUNT + 1

                        _SE01_NumberofIncludedSegments = Convert.ToInt32(ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2))
                        _SE02_TransactionSetControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)

                    End If

                    If _RowRecordType = "GE" Then

                        _GE_COUNT = _GE_COUNT + 1

                        _GE01_NumberofTransactionSetsIncluded = Convert.ToInt32(ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2))
                        _GE02_GroupControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                    End If


                    If _RowRecordType = "IEA" Then

                        _IEA_COUNT = _IEA_COUNT + 1

                        _IEA01_NumberofFunctionalSetsIncluded = Convert.ToInt32(ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2))
                        _IEA02_InterchangeControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                    End If

                Next


                If ((_ISA_COUNT - _IEA_COUNT) <> 0) Then
                    _isMissingSegments = True
                End If

                If ((_GS_COUNT - _GE_COUNT) <> 0) Then
                    _isMissingSegments = True
                End If

                If ((_ST_COUNT - _SE_COUNT) <> 0) Then
                    _isMissingSegments = True
                End If


                r = 0

            Catch ex As Exception

                log.ExceptionDetails(_app, ex)
            End Try

            Return r

        End Function

        Private Function Validate_5010_278(ByVal EDIList As List(Of String)) As Integer

            Dim r As Integer = -1





            Try

                For Each line As String In EDIList
                    _RowProcessedFlag = 0

                    If _DataElementSeparatorFlag = 0 Then
                        _DataElementSeparator = Mid(line, 4, 1)
                        _DataElementSeparatorFlag = 1
                    End If


                    line = line.Replace("~", "")
                    _ROW_COUNT = _ROW_COUNT + 1

                    _RowRecordType = ss.ParseDemlimtedStringEDI(line, _DataElementSeparator, 1)
                    _CurrentRowData = line


                    If _RowRecordType = "ISA" Then

                        _ISA_COUNT = _ISA_COUNT + 1


                        _ISA01_AuthorizationInformationQualifier = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _ISA02_AuthorizationInformation = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _ISA03_SecurityInformationQualifier = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                        _ISA04_SecurityInformation = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 5)
                        _ISA05_InterchangeIDQualifierSender = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 6)
                        _ISA06_InterchangeSenderID = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 7)
                        _ISA07_InterchangeIDQualifierReceiver = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 8)
                        _ISA08_InterchangeReceiverID = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 9)
                        _ISA09_InterchangeDate = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 10)
                        _ISA10_InterchangeTime = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 11)
                        _RepetitionSeparator = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 12)
                        _ISA12_InterchangeControlVersionNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 13)
                        _ISA13_InterchangeControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 14)
                        _ISA14_AcknowledgmentRequested = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 15)
                        _ISA15_InterchangeUsageIndicator = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 16)
                        _ComponentElementSeparator = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 17)
                    End If




                    If _RowRecordType = "GS" Then
                        _GS_COUNT = _GS_COUNT + 1

                        _GS01_FunctionalIdentifierCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _GS02_FunctionalApplicationSendersCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _GS03_FunctionalApplicationReceiverCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                        _GS04_FunctionalDate = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 5)
                        _GS05_FunctionalTime = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 6)
                        _GS06_GroupControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 7)
                        _GS07_ResponsibleAgencyCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 8)
                        _ImplementationConventionReference = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 9)
                    End If


                    If _RowRecordType = "ST" Then

                        _ST_COUNT = _ST_COUNT + 1

                        _TransactionSetIdentifierCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _ST02_TransactionSetControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _ST03_ImplementationConventionReference = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                    End If


                    If _RowRecordType = "BHT" Then
                        _BHT01_HierarchicalStructureCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _BHT02_TransactionSetPurposeCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _BHT03_ReferenceIdentification = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                        _BHT04_Date = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 5)
                        _BHT05_Time = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 6)
                        _BHT06_TransactionTypeCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 7)
                        _BHT07_ResponsibleAgencyCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 8)
                        _BHT08_ImplementationConventionReference = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 9)
                    End If


                    If _RowRecordType = "AAA" Then
                        _isAAA = True
                        _AAAFailureCode = _AAAFailureCode + ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                    End If


                    If _RowRecordType = "NM1" Then
                        _1p = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        If (_1p = "1p" Or _1p = "80" Or _1p = "FA") Then
                            _NPI = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 10)
                        End If
                    End If

                    If _RowRecordType = "TA1" Then
                        _isTA1 = True
                    End If

                    If _RowRecordType = "AK1" Then
                        _isAK1 = True
                    End If

                    If _RowRecordType = "AK9" Then
                        _isAK9 = True
                    End If



                    If _RowRecordType = "SE" Then

                        _SE_COUNT = _SE_COUNT + 1

                        _SE01_NumberofIncludedSegments = Convert.ToInt32(ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2))
                        _SE02_TransactionSetControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)

                    End If

                    If _RowRecordType = "GE" Then

                        _GE_COUNT = _GE_COUNT + 1

                        _GE01_NumberofTransactionSetsIncluded = Convert.ToInt32(ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2))
                        _GE02_GroupControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                    End If


                    If _RowRecordType = "IEA" Then

                        _IEA_COUNT = _IEA_COUNT + 1

                        _IEA01_NumberofFunctionalSetsIncluded = Convert.ToInt32(ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2))
                        _IEA02_InterchangeControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                    End If

                Next


                If ((_ISA_COUNT - _IEA_COUNT) <> 0) Then
                    _isMissingSegments = True
                End If

                If ((_GS_COUNT - _GE_COUNT) <> 0) Then
                    _isMissingSegments = True
                End If

                If ((_ST_COUNT - _SE_COUNT) <> 0) Then
                    _isMissingSegments = True
                End If


                r = 0

            Catch ex As Exception

                log.ExceptionDetails(_app, ex)
            End Try

            Return r

        End Function

        Private Function Validate_5010_835(ByVal EDIList As List(Of String)) As Integer

            Dim r As Integer = -1





            Try

                For Each line As String In EDIList
                    _RowProcessedFlag = 0

                    If _DataElementSeparatorFlag = 0 Then
                        _DataElementSeparator = Mid(line, 4, 1)
                        _DataElementSeparatorFlag = 1
                    End If


                    line = line.Replace("~", "")
                    _ROW_COUNT = _ROW_COUNT + 1

                    _RowRecordType = ss.ParseDemlimtedStringEDI(line, _DataElementSeparator, 1)
                    _CurrentRowData = line


                    If _RowRecordType = "ISA" Then

                        _ISA_COUNT = _ISA_COUNT + 1


                        _ISA01_AuthorizationInformationQualifier = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _ISA02_AuthorizationInformation = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _ISA03_SecurityInformationQualifier = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                        _ISA04_SecurityInformation = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 5)
                        _ISA05_InterchangeIDQualifierSender = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 6)
                        _ISA06_InterchangeSenderID = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 7)
                        _ISA07_InterchangeIDQualifierReceiver = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 8)
                        _ISA08_InterchangeReceiverID = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 9)
                        _ISA09_InterchangeDate = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 10)
                        _ISA10_InterchangeTime = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 11)
                        _RepetitionSeparator = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 12)
                        _ISA12_InterchangeControlVersionNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 13)
                        _ISA13_InterchangeControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 14)
                        _ISA14_AcknowledgmentRequested = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 15)
                        _ISA15_InterchangeUsageIndicator = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 16)
                        _ComponentElementSeparator = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 17)
                    End If




                    If _RowRecordType = "GS" Then
                        _GS_COUNT = _GS_COUNT + 1

                        _GS01_FunctionalIdentifierCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _GS02_FunctionalApplicationSendersCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _GS03_FunctionalApplicationReceiverCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                        _GS04_FunctionalDate = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 5)
                        _GS05_FunctionalTime = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 6)
                        _GS06_GroupControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 7)
                        _GS07_ResponsibleAgencyCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 8)
                        _ImplementationConventionReference = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 9)
                    End If


                    If _RowRecordType = "ST" Then

                        _ST_COUNT = _ST_COUNT + 1

                        _TransactionSetIdentifierCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _ST02_TransactionSetControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _ST03_ImplementationConventionReference = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                    End If


                    If _RowRecordType = "BHT" Then
                        _BHT01_HierarchicalStructureCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _BHT02_TransactionSetPurposeCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _BHT03_ReferenceIdentification = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                        _BHT04_Date = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 5)
                        _BHT05_Time = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 6)
                        _BHT06_TransactionTypeCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 7)
                        _BHT07_ResponsibleAgencyCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 8)
                        _BHT08_ImplementationConventionReference = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 9)
                    End If


                    If _RowRecordType = "AAA" Then
                        _isAAA = True
                        _AAAFailureCode = _AAAFailureCode + ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                    End If


                    If _RowRecordType = "NM1" Then
                        _1p = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        If (_1p = "1p" Or _1p = "80" Or _1p = "FA") Then
                            _NPI = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 10)
                        End If
                    End If

                    If _RowRecordType = "TA1" Then
                        _isTA1 = True
                    End If

                    If _RowRecordType = "AK1" Then
                        _isAK1 = True
                    End If

                    If _RowRecordType = "AK9" Then
                        _isAK9 = True
                    End If



                    If _RowRecordType = "SE" Then

                        _SE_COUNT = _SE_COUNT + 1

                        _SE01_NumberofIncludedSegments = Convert.ToInt32(ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2))
                        _SE02_TransactionSetControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)

                    End If

                    If _RowRecordType = "GE" Then

                        _GE_COUNT = _GE_COUNT + 1

                        _GE01_NumberofTransactionSetsIncluded = Convert.ToInt32(ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2))
                        _GE02_GroupControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                    End If


                    If _RowRecordType = "IEA" Then

                        _IEA_COUNT = _IEA_COUNT + 1

                        _IEA01_NumberofFunctionalSetsIncluded = Convert.ToInt32(ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2))
                        _IEA02_InterchangeControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                    End If

                Next


                If ((_ISA_COUNT - _IEA_COUNT) <> 0) Then
                    _isMissingSegments = True
                End If

                If ((_GS_COUNT - _GE_COUNT) <> 0) Then
                    _isMissingSegments = True
                End If

                If ((_ST_COUNT - _SE_COUNT) <> 0) Then
                    _isMissingSegments = True
                End If


                r = 0

            Catch ex As Exception

                log.ExceptionDetails(_app, ex)
            End Try

            Return r

        End Function

        Private Function Validate_5010_837(ByVal EDIList As List(Of String)) As Integer

            Dim r As Integer = -1





            Try

                For Each line As String In EDIList
                    _RowProcessedFlag = 0

                    If _DataElementSeparatorFlag = 0 Then
                        _DataElementSeparator = Mid(line, 4, 1)
                        _DataElementSeparatorFlag = 1
                    End If


                    line = line.Replace("~", "")
                    _ROW_COUNT = _ROW_COUNT + 1

                    _RowRecordType = ss.ParseDemlimtedStringEDI(line, _DataElementSeparator, 1)
                    _CurrentRowData = line


                    If _RowRecordType = "ISA" Then

                        _ISA_COUNT = _ISA_COUNT + 1


                        _ISA01_AuthorizationInformationQualifier = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _ISA02_AuthorizationInformation = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _ISA03_SecurityInformationQualifier = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                        _ISA04_SecurityInformation = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 5)
                        _ISA05_InterchangeIDQualifierSender = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 6)
                        _ISA06_InterchangeSenderID = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 7)
                        _ISA07_InterchangeIDQualifierReceiver = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 8)
                        _ISA08_InterchangeReceiverID = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 9)
                        _ISA09_InterchangeDate = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 10)
                        _ISA10_InterchangeTime = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 11)
                        _RepetitionSeparator = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 12)
                        _ISA12_InterchangeControlVersionNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 13)
                        _ISA13_InterchangeControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 14)
                        _ISA14_AcknowledgmentRequested = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 15)
                        _ISA15_InterchangeUsageIndicator = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 16)
                        _ComponentElementSeparator = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 17)
                    End If




                    If _RowRecordType = "GS" Then
                        _GS_COUNT = _GS_COUNT + 1

                        _GS01_FunctionalIdentifierCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _GS02_FunctionalApplicationSendersCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _GS03_FunctionalApplicationReceiverCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                        _GS04_FunctionalDate = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 5)
                        _GS05_FunctionalTime = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 6)
                        _GS06_GroupControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 7)
                        _GS07_ResponsibleAgencyCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 8)
                        _ImplementationConventionReference = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 9)
                    End If


                    If _RowRecordType = "ST" Then

                        _ST_COUNT = _ST_COUNT + 1

                        _TransactionSetIdentifierCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _ST02_TransactionSetControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _ST03_ImplementationConventionReference = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                    End If


                    If _RowRecordType = "BHT" Then
                        _BHT01_HierarchicalStructureCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _BHT02_TransactionSetPurposeCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _BHT03_ReferenceIdentification = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                        _BHT04_Date = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 5)
                        _BHT05_Time = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 6)
                        _BHT06_TransactionTypeCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 7)
                        _BHT07_ResponsibleAgencyCode = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 8)
                        _BHT08_ImplementationConventionReference = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 9)
                    End If


                    If _RowRecordType = "AAA" Then
                        _isAAA = True
                        _AAAFailureCode = _AAAFailureCode + ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                    End If


                    If _RowRecordType = "NM1" Then
                        _1p = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        If (_1p = "1p" Or _1p = "80" Or _1p = "FA") Then
                            _NPI = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 10)
                        End If
                    End If

                    If _RowRecordType = "TA1" Then
                        _isTA1 = True
                    End If

                    If _RowRecordType = "AK1" Then
                        _isAK1 = True
                    End If

                    If _RowRecordType = "AK9" Then
                        _isAK9 = True
                    End If



                    If _RowRecordType = "SE" Then

                        _SE_COUNT = _SE_COUNT + 1

                        _SE01_NumberofIncludedSegments = Convert.ToInt32(ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2))
                        _SE02_TransactionSetControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)

                    End If

                    If _RowRecordType = "GE" Then

                        _GE_COUNT = _GE_COUNT + 1

                        _GE01_NumberofTransactionSetsIncluded = Convert.ToInt32(ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2))
                        _GE02_GroupControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                    End If


                    If _RowRecordType = "IEA" Then

                        _IEA_COUNT = _IEA_COUNT + 1

                        _IEA01_NumberofFunctionalSetsIncluded = Convert.ToInt32(ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2))
                        _IEA02_InterchangeControlNumber = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                    End If

                Next


                If ((_ISA_COUNT - _IEA_COUNT) <> 0) Then
                    _isMissingSegments = True
                End If

                If ((_GS_COUNT - _GE_COUNT) <> 0) Then
                    _isMissingSegments = True
                End If

                If ((_ST_COUNT - _SE_COUNT) <> 0) Then
                    _isMissingSegments = True
                End If


                r = 0

            Catch ex As Exception

                log.ExceptionDetails(_app, ex)
            End Try

            Return r

        End Function

    End Class

End Namespace

