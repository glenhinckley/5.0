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
Imports DCSGlobal.BusinessRules.CoreLibraryII

Namespace DCSGlobal.EDI

    Public Class EDI_5010_276_005010X212_DCS

        Inherits EDI_5010_276_00510X212_TABLES

        Implements IDisposable


        Protected disposed As Boolean = False

        '******************************************************************************************************************
        '  all vars not declared here are in   EDI_5010_COMMON_DECS   it is inhreted from the tables for this type
        '  if you are going to use it on this class only it goes below if not put it in EDI_5010_COMMON_DECS
        '******************************************************************************************************************


        Private log As New logExecption
        Private ss As New StringStuff
        Private d As New DebugOUT
        '   Private oD As New Declarations

        Public _ConnectionString As String = String.Empty
        Public _CommandTimeOut As Integer = 90


        Private _DocumentType As String = "276_005010X212"

        Private _SP_ISA As String = "[usp_EDI_5010_HIPAA_276_ISA]"
        Private _SP_IEA As String = "[usp_EDI_5010_HIPAA_276_IEA]"

        Private _SP_GS As String = "[usp_EDI_5010_HIPAA_276_GS]"
        Private _SP_GE As String = "[usp_EDI_5010_HIPAA_276_GE]"

        Private _SP_ST As String = "[usp_EDI_5010_HIPAA_276_ST]"
        Private _SP_SE As String = "[usp_EDI_5010_HIPAA_276_SE]"

        Private _SP_HEADERS As String = "[usp_EDI_5010_HIPAA_276_HEADERS]"

        Private _SP_COMIT_HL20_UNIT_TEST As String = "[usp_EDI_5010_HIPAA_276_20_UNIT_TEST]"
        Private _SP_COMIT_HL20_DATA As String = "[usp_EDI_5010_HIPAA_276_20_HEADER_DUMP]"

        Private _SP_COMIT_HL22_UNIT_TEST As String = "[usp_EDI_5010_HIPAA_276_22_UNIT_TEST]"
        Private _SP_COMIT_HL22_DATA As String = "[usp_EDI_5010_HIPAA_276_22_CLAIM_STATUS_RESPONSE_DUMP]"


        Private _SP_COMIT_UNKNOWN As String = "[usp_EDI_5010_HIPAA_UNKNOWN]"

        Private _SP_ROLLBACK As String = "[usp_EDI_276_ROLLBACK]"

        Private _276_TRN_GUID As Guid = Guid.Empty
        Private _276_STC_GUID As Guid = Guid.Empty
        Private _276_SVC_GUID As Guid = Guid.Empty

        Private _276_P_STC_GUID As Guid = Guid.Empty

        Private _276_ISL_GUID As Guid = Guid.Empty
        Private _276_IRL_GUID As Guid = Guid.Empty
        Private _276_SPL_GUID As Guid = Guid.Empty


        Private _276_CLS_GUID As Guid = Guid.Empty
        Private _276_SLS_GUID As Guid = Guid.Empty

        Private _276_2200D_BEGIN As Boolean = False

        Private _276_2200E_BEGIN As Boolean = False

        Private _276_2200D_DTP_FOUND As Boolean = False
        Private _276_2200E_DTP_FOUND As Boolean = False

        Private _276_22_CLS_NM1_FOUND As Boolean = False
        Private _276_23_CLS_NM1_FOUND As Boolean = False


        Private isCLS_ON_DISC As Boolean = False
        Private _TRN01 As String = String.Empty


        Private _STLine As String = String.Empty
        Private _NM01 As String = String.Empty


        Private _STC01 As String = String.Empty
        Private _STC10 As String = String.Empty
        Private _STC11 As String = String.Empty

        Private _SVC01 As String = String.Empty
        Private _SVC06 As String = String.Empty


        'Private payor_name As String = String.Empty
        'Private payor_id As String = String.Empty

        'Private provider_name As String = String.Empty
        'Private Npi As String = String.Empty



        Public Sub New()
            If Debugger.IsAttached Then
                _VERBOSE = 1
                _DEBUG_LEVEL = 1

            End If

            _CONSOLE_NAME = "EDI_5010_276_005010X212_DCS"
            _CLASS_NAME = "EDI_5010_276_005010X212_DCS"


        End Sub

        Protected Overrides Sub Finalize()

            Dispose()
            '' Console.WriteLine("Object " & GetHashCode() & " finalized.")
        End Sub

        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposed Then
                If disposing Then

                    log = Nothing

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

        Public WriteOnly Property CONSOLE_NAME As String

            Set(value As String)
                _CONSOLE_NAME = value

            End Set
        End Property

        Public WriteOnly Property ConnectionString As String

            Set(value As String)
                _ConnectionString = value
                log.ConnectionString = value
            End Set
        End Property

        Public WriteOnly Property FILE_ID As Long

            Set(value As Long)
                _FILE_ID = value
                _IS_FILE = True
            End Set
        End Property

        Public WriteOnly Property BATCH_ID As Long

            Set(value As Long)
                _BATCH_ID = value
            End Set
        End Property

        Public WriteOnly Property EDI_SEQUENCE_NUMBER As Integer

            Set(value As Integer)
                _EDI_SEQUENCE_NUMBER = value
            End Set

        End Property


        Public WriteOnly Property SP_ISA As String

            Set(value As String)
                _SP_ISA = value
            End Set
        End Property

        Public WriteOnly Property SP_IEA As String

            Set(value As String)
                _SP_IEA = value
            End Set
        End Property

        Public WriteOnly Property SP_GS As String

            Set(value As String)
                _SP_GS = value
            End Set
        End Property

        Public WriteOnly Property SP_GE As String

            Set(value As String)
                _SP_GE = value
            End Set
        End Property

        Public WriteOnly Property SP_ST As String

            Set(value As String)
                _SP_ST = value
            End Set
        End Property


        Public WriteOnly Property SP_SE As String

            Set(value As String)
                _SP_SE = value
            End Set
        End Property

        Public WriteOnly Property SP_HEADERS As String

            Set(value As String)
                _SP_HEADERS = value
            End Set
        End Property

        Public WriteOnly Property SP_COMIT_HL20_UNIT_TEST As String

            Set(value As String)
                _SP_COMIT_HL20_UNIT_TEST = value
            End Set
        End Property
        Public WriteOnly Property SP_DO_TEST As Integer

            Set(value As Integer)
                _SP_DO_TEST = value
            End Set
        End Property
        Public WriteOnly Property SP_COMIT_HL20_DATA As String

            Set(value As String)
                _SP_COMIT_HL20_DATA = value
            End Set
        End Property


        Public WriteOnly Property SP_COMIT_HL22_UNIT_TEST As String

            Set(value As String)
                _SP_COMIT_HL22_UNIT_TEST = value
            End Set
        End Property

        Public WriteOnly Property SP_COMIT_HL22_DATA As String

            Set(value As String)
                _SP_COMIT_HL22_DATA = value
            End Set
        End Property

        Public WriteOnly Property SP_COMIT_UNKNOWN As String

            Set(value As String)
                _SP_COMIT_UNKNOWN = value
            End Set
        End Property

        Public WriteOnly Property SP_ROLLBACK As String

            Set(value As String)
                _SP_ROLLBACK = value
            End Set
        End Property

        Public ReadOnly Property IMPORT_RETURN_STRING As String

            Get
                Return _IMPORT_RETURN_STRING
            End Get


        End Property

        Public Function Import(ByVal EDIList As List(Of String), ByVal BatchID As Double) As Int32
            Dim x As Integer = -1

            _BATCH_ID = Convert.ToInt64(BatchID)

            x = Import(EDIList)

            Return x

        End Function
        Public Function ImportByString(ByVal EDI As String, ByVal BatchID As Double) As Integer


            _IS_STRING = True

            Dim r As Integer = -1



            Dim SP As New StringPrep()
            _BATCH_ID = CLng(BatchID)

            _EDIList = SP.SingleEDI(EDI)

            r = Import(_EDIList)


            Return r

        End Function

        Public Function Import(ByVal EDIList As List(Of String)) As Integer

            Dim last As String = String.Empty


            _FUNCTION_NAME = "Import(ByVal EDIList As List(Of String)) As InTEGER"



            'If _EDI_SEQUENCE_NUMBER = 0 Then

            '    Using e As New EDI_5010_LOGGING
            '        e.ConnectionString = _ConnectionString
            '        e.TransactionSetIdentifierCode = "276"
            '        e.EDI_SEQUENCE("276")
            '        _EDI_SEQUENCE_NUMBER = e.EDI_SEQUENCE_NUMBER
            '    End Using

            'End If



            _ProcessStartTime = Now
            Dim _ImportReturnCode As Integer = 0


            If _TablesBuilt = False Then
                BuildTables()
                _TablesBuilt = True
                ClearIAS()
            End If

            'so lets get down to it opn the file in a stream reader and move thru it one line at a time...........


            _HL20_DIRTY = True

            If _SP_DO_TEST = 1 Then
                _SP_RETURN_CODE = TestSPs()

                If _SP_DO_TEST = 1 Then
                    If (_SP_RETURN_CODE <> 0) Then
                        _IMPORT_RETURN_STRING = "276 : SP TEST FAILED WITH ERROR CODE " + Convert.ToString(_SP_RETURN_CODE)
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _IMPORT_RETURN_STRING)
                        Return -1
                        Exit Function
                    End If
                End If
            End If

            If (_IS_FILE) Then
                Using e As New EDI_5010_LOGGING
                    e.ConnectionString = _ConnectionString
                    e.TransactionSetIdentifierCode = "276"
                    e.UpdateFileStatus(CInt(_FILE_ID), "276", "BEGIN PARSE")
                End Using
            End If

            '            _DOCUMENT_ID = _EDI_SEQUENCE_NUMBER


            For Each line As String In EDIList

                'If _hasERR Then
                '    RollBack()
                '    _IMPORT_RETURN_STRING = _IMPORT_RETURN_STRING + " ROLLED BACK EDI_SEQUENCE_NUMBER " + Convert.ToString(_EDI_SEQUENCE_NUMBER)
                '    Return -2
                '    Exit Function
                'End If





                _RowProcessedFlag = 0

                If _DataElementSeparatorFlag = 0 Then
                    _DataElementSeparator = Mid(line, 4, 1)
                    _DataElementSeparatorFlag = 1
                End If

                last = line

                '   _RAW_EDI = _RAW_EDI + line

                line = line.Replace("~", "")
                _ROW_COUNT = _ROW_COUNT + 1

                '  __RowCount()


                _RowRecordType = ss.ParseDemlimtedStringEDI(line, _DataElementSeparator, 1)
                'Console.WriteLine(_RowRecordType)
                _CurrentRowData = line


                If _VERBOSE = 1 Then

                    _DEBUG_STRING = ""
                    _DEBUG_STRING = Convert.ToString(_ROW_COUNT) + "::" + _RowRecordType + "::" + _CurrentRowData

                    d.WriteDebugLine(_DEBUG_LEVEL, _DEBUG_STRING)

                End If


                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   ISA
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "ISA" Then

                        _RowProcessedFlag = 1


                        _HIPAA_ISA_GUID = Guid.NewGuid
                        '_P_GUID = _ISA_GUID

                        _ISA01 = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _ISA02 = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _ISA03 = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                        _ISA04 = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 5)
                        _ISA05 = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 6)
                        _ISA06 = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 7)
                        _ISA07 = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 8)
                        _ISA08 = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 9)
                        _ISA09 = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 10)
                        _ISA10 = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 11)
                        _ISA11 = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 12)
                        _ISA12 = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 13)
                        _ISA13 = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 14)
                        _ISA14 = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 15)
                        _ISA15 = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 16)
                        _ISA16 = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 17)

                        Dim ISARow As DataRow = ISA.NewRow
                        ISARow("DOCUMENT_ID") = _DOCUMENT_ID
                        ISARow("FILE_ID") = _FILE_ID
                        ISARow("BATCH_ID") = _BATCH_ID
                        ISARow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                        ISARow("ISA01") = _ISA01
                        ISARow("ISA02") = _ISA02
                        ISARow("ISA03") = _ISA03
                        ISARow("ISA04") = _ISA04
                        ISARow("ISA05") = _ISA05
                        ISARow("ISA06") = _ISA06
                        ISARow("ISA07") = _ISA07
                        ISARow("ISA08") = _ISA08
                        ISARow("ISA09") = _ISA09
                        ISARow("ISA10") = _ISA10
                        ISARow("ISA11") = _ISA11
                        ISARow("ISA12") = _ISA12
                        ISARow("ISA13") = _ISA13
                        ISARow("ISA14") = _ISA14
                        ISARow("ISA15") = _ISA15
                        ISARow("ISA16") = _ISA16



                        '_ISA_ROW_ID = _RowCount
                        ISARow("ROW_NUMBER") = _ROW_COUNT

                        ISA.Rows.Add(ISARow)



                        _RepetitionSeparator = _ISA11
                        _ComponentElementSeparator = _ISA16

                        _ISA = Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"


                        '  _chars = "RowDataDelimiter: " + _DataElementSeparator + " CarrotDataDelimiter: " + _CarrotDataDelimiter + " ComponentElementSeperator: " + _ISA16_ComponentElementSeparator



                        'o'D.ISAFlag = 1

                        ComitISA()


                        Select Case _HL03
                            Case 0
                                _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case 19
                                _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case 20
                                _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case 21
                                _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case Else
                                _RAW_22 = _RAW_22 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                        End Select

                    End If

                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "ISA", ex)
                End Try



                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   GS
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "GS" Then
                        _RowProcessedFlag = 1
                        _HIPAA_GS_GUID = Guid.NewGuid
                        'o() 'D.P_GUID = _GS_GUID

                        _GS01 = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _GS02 = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _GS03 = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                        _GS04 = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 5)
                        _GS05 = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 6)
                        _GS06 = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 7)
                        _GS07 = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 8)
                        _GS08 = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 9)



                        Dim GSRow As DataRow = GS.NewRow
                        GSRow("DOCUMENT_ID") = _DOCUMENT_ID
                        GSRow("FILE_ID") = _FILE_ID
                        GSRow("BATCH_ID") = _BATCH_ID
                        GSRow("ISA_ID") = _ISA_ID
                        GSRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                        GSRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                        GSRow("GS01") = _GS01
                        GSRow("GS02") = _GS02
                        GSRow("GS03") = _GS03
                        GSRow("GS04") = _GS04
                        GSRow("GS05") = _GS05
                        GSRow("GS06") = _GS06
                        GSRow("GS07") = _GS07
                        GSRow("GS08") = _GS08

                        GSRow("FILE_ID") = _FILE_ID

                        'o() 'D.GS_ROW_ID = _RowCount

                        GSRow("ROW_NUMBER") = _ROW_COUNT
                        GS.Rows.Add(GSRow)

                        _GS = Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"

                        ComitGS()


                        Select Case _HL03
                            Case 0
                                _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case 19
                                _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case 20
                                _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case 21
                                _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case Else
                                _RAW_22 = _RAW_22 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                        End Select

                    End If

                    'end GS


                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
                End Try


                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ' ok so once we find and ST we start a a new string to hold evrey thing to the se and that will get passed to a
                ' thread to process along with all every thing it needs to tag it all.
                '
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   ST
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try ' begin st
                    If _RowRecordType = "ST" Then
                        _RowProcessedFlag = 1


                        _HIPAA_ST_GUID = Guid.NewGuid
                        '_P_GUID = _ST_GUID



                        _ST01 = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _ST02 = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _ST03 = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)


                        Dim STRow As DataRow = ST.NewRow
                        STRow("DOCUMENT_ID") = _DOCUMENT_ID
                        STRow("FILE_ID") = _FILE_ID
                        STRow("BATCH_ID") = _BATCH_ID
                        STRow("FILE_ID") = _FILE_ID
                        STRow("ISA_ID") = _ISA_ID
                        STRow("GS_ID") = _GS_ID

                        STRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                        STRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                        STRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                        STRow("ST01") = _ST01
                        STRow("ST02") = _ST02
                        STRow("ST03") = _ST03



                        STRow("ROW_NUMBER") = _ROW_COUNT
                        ST.Rows.Add(STRow)



                        _ST = Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"

                        Select Case _HL03
                            Case 0
                                _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case 19
                                _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case 20
                                _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case 21
                                _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case Else
                                _RAW_22 = _RAW_22 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                        End Select
                        '   ComitST()
                    End If

                    ' all the rows get made in to a string. 

                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "ST", ex)
                End Try



                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   BHT
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "BHT" Then

                        '_BHT_GUID = Guid.NewGuid
                        '_P_GUID = _ST_GUID

                        _BHT01 = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 2)
                        _BHT02 = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 3)
                        _BHT03 = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 4)
                        _BHT04 = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 5)
                        _BHT05 = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 6)
                        _BHT06 = ss.ParseDemlimtedStringEDI(_CurrentRowData, _DataElementSeparator, 7)



                        Dim BHTRow As DataRow = BHT.NewRow
                        BHTRow("DOCUMENT_ID") = _DOCUMENT_ID
                        BHTRow("FILE_ID") = _FILE_ID
                        BHTRow("BATCH_ID") = _BATCH_ID
                        BHTRow("ISA_ID") = _ISA_ID
                        BHTRow("GS_ID") = _GS_ID
                        BHTRow("ST_ID") = _ST_ID
                        BHTRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                        BHTRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                        BHTRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                        BHTRow("BHT01") = _BHT01
                        BHTRow("BHT02") = _BHT02
                        BHTRow("BHT03") = _BHT03
                        BHTRow("BHT04") = _BHT04
                        BHTRow("BHT05") = _BHT05
                        BHTRow("BHT06") = _BHT06
                        BHTRow("ROW_NUMBER") = _ROW_COUNT

                        BHT.Rows.Add(BHTRow)

                        _BHT = Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"


                        Select Case _HL03
                            Case 0
                                _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case 19
                                _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case 20
                                _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case 21
                                _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case Else
                                _RAW_22 = _RAW_22 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                        End Select
                        _RowProcessedFlag = 1

                        ComitST()


                    End If

                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "BHT", ex)
                End Try


                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '  BEGIN 276
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''



                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   BEGIN HL LEVELS
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   HL       NASTY
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "HL" Then


                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then
                            _HL01 = Convert.ToInt32(ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2))
                        Else
                            _HL01 = 0
                        End If

                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then
                            _HL02 = Convert.ToInt32(ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3))
                        Else
                            _HL02 = 0
                        End If



                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then
                            _HL03 = Convert.ToInt32(ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4))
                        Else
                            _HL03 = 0
                        End If



                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then
                            _HL04 = Convert.ToInt32(ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5))
                        Else
                            _HL04 = 0
                        End If




                        Select Case _HL03   ' Must be a primitive data type
                            Case 20
                                _RAW_20 = String.Empty

                            Case 21

                            Case 22

                                If _HL20_DIRTY Then
                                    ComitHL20()
                                    _HL20_DIRTY = False
                                End If

                                If _HL22_DIRTY = True Then
                                    ComitHL22()
                                    _HL22_DIRTY = False
                                End If

                            Case 23

                            Case 24

                            Case Else

                        End Select


                    End If



                Catch


                End Try





                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   HL
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "HL" Then


                        'If _HL20Flag = 0 Then
                        '    ComitSTHeaders()
                        '    NM1.Clear()
                        '    PER.Clear()
                        '    _HL20Flag = 1

                        'End If


                        '   _HL_GUID = Guid.NewGuid

                        '' _NM1_GUID = Guid.Empty


                        _RowProcessedFlag = 1

                        Dim HLRow As DataRow = HL.NewRow
                        HLRow("FILE_ID") = _FILE_ID
                        HLRow("ISA_ID") = _ISA_ID
                        HLRow("BATCH_ID") = _BATCH_ID
                        HLRow("GS_ID") = _GS_ID
                        HLRow("ST_ID") = _ST_ID
                        HLRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                        HLRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                        HLRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID




                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then
                            _HL01 = Convert.ToInt32(ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2))
                        Else
                            _HL01 = 0
                        End If

                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then
                            _HL02 = Convert.ToInt32(ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3))
                        Else
                            _HL02 = 0
                        End If



                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then
                            _HL03 = Convert.ToInt32(ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4))
                        Else
                            _HL03 = 0
                        End If



                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then
                            _HL04 = Convert.ToInt32(ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5))
                        Else
                            _HL04 = 0
                        End If


                        Select Case _HL03   ' Must be a primitive data type

                            Case 19
                                _HL19_DIRTY = True
                                _HIPAA_HL_19_GUID = Guid.NewGuid
                                '_HIPAA_HL_20_GUID = Guid.Empty
                                '_HIPAA_HL_21_GUID = Guid.Empty
                                '_HIPAA_HL_22_GUID = Guid.Empty
                                '_HIPAA_HL_23_GUID = Guid.Empty
                                '_HIPAA_HL_24_GUID = Guid.Empty

                                _276_SPL_GUID = Guid.NewGuid
                                _LoopLevelMajor = 2000
                                _LoopLevelMinor = 0
                                _LoopLevelSubFix = "C"

                            Case 20
                                _276_SPL_GUID = Guid.Empty
                                '    _HIPAA_HL_19_GUID = Guid.Empty
                                _LoopLevelMajor = 2000
                                _HL20_DIRTY = True
                                _HIPAA_HL_20_GUID = Guid.NewGuid
                                _HIPAA_HL_21_GUID = Guid.Empty
                                _HIPAA_HL_22_GUID = Guid.Empty
                                _HIPAA_HL_23_GUID = Guid.Empty
                                _HIPAA_HL_24_GUID = Guid.Empty
                            Case 21

                                _276_SPL_GUID = Guid.Empty
                                '   _HIPAA_HL_19_GUID = Guid.Empty


                                _HL21_DIRTY = True
                                _HIPAA_HL_21_GUID = Guid.NewGuid
                                _HIPAA_HL_22_GUID = Guid.Empty
                                _HIPAA_HL_23_GUID = Guid.Empty
                                _HIPAA_HL_24_GUID = Guid.Empty

                                _LoopLevelMajor = 2000
                                _LoopLevelMinor = 0
                                _LoopLevelSubFix = "B"
                            Case 22

                                '  CLEAR_CLS_DATA()

                                _276_SPL_GUID = Guid.Empty
                                _HIPAA_HL_19_GUID = Guid.Empty

                                _HL22_DIRTY = True
                                _HIPAA_HL_22_GUID = Guid.NewGuid
                                _HIPAA_HL_23_GUID = Guid.Empty
                                _HIPAA_HL_24_GUID = Guid.Empty

                                _LoopLevelMajor = 2000
                                _LoopLevelMinor = 0
                                _LoopLevelSubFix = "D"

                            Case 23

                                '   _276_SPL_GUID = Guid.Empty
                                '  _HIPAA_HL_19_GUID = Guid.Empty
                                _HL22_DIRTY = True
                                _HIPAA_HL_23_GUID = Guid.NewGuid



                                _LoopLevelMajor = 2000
                                _LoopLevelMinor = 0
                                _LoopLevelSubFix = "E"

                            Case 24
                                _HIPAA_HL_24_GUID = Guid.Empty
                                '  _276_SPL_GUID = Guid.Empty
                                '  _HIPAA_HL_19_GUID = Guid.Empty

                                _HL22_DIRTY = True
                                '   _HIPAA_HL_24_GUID = Guid.NewGuid
                            Case Else

                        End Select


                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then HLRow("HL01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else HLRow("HL01") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then HLRow("HL02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else HLRow("HL02") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then HLRow("HL03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else HLRow("HL03") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then HLRow("HL04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else HLRow("HL04") = DBNull.Value



                        HLRow("HIPAA_HL_19_GUID") = _HIPAA_HL_19_GUID
                        HLRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                        HLRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                        HLRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                        HLRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                        HLRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID



                        HLRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        HLRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                        HLRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor


                        HLRow("ROW_NUMBER") = _ROW_COUNT



                        HLRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        HLRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                        HLRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor

                        HL.Rows.Add(HLRow)


                        Select Case _HL03
                            Case 0
                                _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case 19
                                _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case 20
                                _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case 21
                                _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case Else
                                _RAW_22 = _RAW_22 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                        End Select


                    End If

                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "HL", ex)
                End Try

                If _HL03 < 22 Then


                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   NM1
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "NM1" Then



                            _RowProcessedFlag = 1



                            Dim NM1Row As DataRow = NM1.NewRow
                            NM1Row("DOCUMENT_ID") = _DOCUMENT_ID
                            NM1Row("FILE_ID") = _FILE_ID
                            NM1Row("BATCH_ID") = _BATCH_ID
                            NM1Row("ISA_ID") = _ISA_ID
                            NM1Row("GS_ID") = _GS_ID
                            NM1Row("ST_ID") = _ST_ID
                            NM1Row("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            NM1Row("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            NM1Row("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            NM1Row("HIPAA_HL_19_GUID") = _HIPAA_HL_19_GUID
                            NM1Row("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            NM1Row("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            NM1Row("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            NM1Row("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            'NM1Row("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            'NM1Row("276_ISL_GUID") = _276_ISL_GUID
                            'NM1Row("276_IRL_GUID") = _276_IRL_GUID
                            'NM1Row("276_SPL_GUID") = _276_SPL_GUID
                            'NM1Row("276_CLS_GUID") = _276_CLS_GUID
                            'NM1Row("276_SLS_GUID") = _276_SLS_GUID

                            NM1Row("HL01") = _HL01
                            NM1Row("HL02") = _HL02
                            NM1Row("HL03") = _HL03
                            NM1Row("HL04") = _HL04


                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then
                                NM1Row("NM101") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2)
                                _NM01 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2)
                            Else
                                NM1Row("NM101") = DBNull.Value
                            End If


                            NM1Lookup(_NM01)

                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then NM1Row("NM102") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else NM1Row("NM102") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then NM1Row("NM103") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else NM1Row("NM103") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then NM1Row("NM104") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else NM1Row("NM104") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then NM1Row("NM105") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else NM1Row("NM105") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then NM1Row("NM106") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) Else NM1Row("NM106") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then NM1Row("NM107") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) Else NM1Row("NM107") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) <> "") Then NM1Row("NM108") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) Else NM1Row("NM108") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) <> "") Then NM1Row("NM109") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) Else NM1Row("NM109") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11) <> "") Then NM1Row("NM110") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11) Else NM1Row("NM110") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12) <> "") Then NM1Row("NM111") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12) Else NM1Row("NM111") = DBNull.Value

                            NM1Row("ROW_NUMBER") = _ROW_COUNT



                            NM1Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor

                            NM1Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            NM1Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                            NM1.Rows.Add(NM1Row)

                            '_Loop = 0

                            '_Loop = _LoopLevelMajor + _LoopLevelMinor


                            ' CHECK TO SE IF WERE AT THE 2100D 

                            'If _Loop = 2100 And _LoopLevelSubFix = "D" Then

                            '    _276_CLS_GUID = Guid.NewGuid
                            '    _276_22_CLS_NM1_FOUND = True
                            '    _LoopLevelMinor = 200

                            'End If



                            Select Case _HL03
                                Case 0
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case 19
                                    BUILD_CSR_NM1(_CurrentRowData)
                                    _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case 20
                                    BUILD_CSR_NM1(_CurrentRowData)
                                    _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case 21
                                    BUILD_CSR_NM1(_CurrentRowData)
                                    _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case Else
                                    _RAW_22 = _RAW_22 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select

                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "HL > 22 :: NM1", ex)
                    End Try


                End If




                If _HL03 >= 22 Then



                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   DMG
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "DMG" Then

                            _RowProcessedFlag = 1

                            Dim DMGRow As DataRow = DMG.NewRow
                            DMGRow("DOCUMENT_ID") = _DOCUMENT_ID
                            DMGRow("FILE_ID") = _FILE_ID
                            DMGRow("BATCH_ID") = _BATCH_ID
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
                            DMGRow("276_ISL_GUID") = _276_ISL_GUID
                            DMGRow("276_IRL_GUID") = _276_IRL_GUID
                            DMGRow("276_SPL_GUID") = _276_SPL_GUID
                            DMGRow("276_CLS_GUID") = _276_CLS_GUID
                            DMGRow("276_SLS_GUID") = Guid.Empty
                            DMGRow("HL01") = _HL01
                            DMGRow("HL02") = _HL02
                            DMGRow("HL03") = _HL03
                            DMGRow("HL04") = _HL04

                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then DMGRow("DMG01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else DMGRow("DMG01") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then DMGRow("DMG02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else DMGRow("DMG02") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then DMGRow("DMG03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else DMGRow("DMG03") = DBNull.Value
                            '    If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then DMGRow("DMG04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else DMGRow("DMG04") = DBNull.Value


                            DMGRow("ROW_NUMBER") = _ROW_COUNT
                            DMGRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            DMGRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            DMGRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                            DMG.Rows.Add(DMGRow)


                            Select Case _HL03
                                Case 0
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case 19
                                    _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case 20
                                    _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case 21
                                    _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case Else
                                    _RAW_22 = _RAW_22 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select


                        End If



                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "DMG", ex)
                    End Try





                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   NM1
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "NM1" Then



                            _RowProcessedFlag = 1



                            Dim NM1Row As DataRow = NM1.NewRow
                            NM1Row("DOCUMENT_ID") = _DOCUMENT_ID
                            NM1Row("FILE_ID") = _FILE_ID
                            NM1Row("BATCH_ID") = _BATCH_ID
                            NM1Row("ISA_ID") = _ISA_ID
                            NM1Row("GS_ID") = _GS_ID
                            NM1Row("ST_ID") = _ST_ID
                            NM1Row("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            NM1Row("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            NM1Row("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            NM1Row("HIPAA_HL_19_GUID") = _HIPAA_HL_19_GUID
                            NM1Row("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            NM1Row("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            NM1Row("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            NM1Row("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            'NM1Row("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            'NM1Row("276_ISL_GUID") = _276_ISL_GUID
                            'NM1Row("276_IRL_GUID") = _276_IRL_GUID
                            'NM1Row("276_SPL_GUID") = _276_SPL_GUID
                            'NM1Row("276_CLS_GUID") = _276_CLS_GUID
                            'NM1Row("276_SLS_GUID") = _276_SLS_GUID

                            NM1Row("HL01") = _HL01
                            NM1Row("HL02") = _HL02
                            NM1Row("HL03") = _HL03
                            NM1Row("HL04") = _HL04


                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then
                                NM1Row("NM101") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2)
                                _NM01 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2)
                            Else
                                NM1Row("NM101") = DBNull.Value
                            End If


                            NM1Lookup(_NM01)

                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then NM1Row("NM102") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else NM1Row("NM102") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then NM1Row("NM103") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else NM1Row("NM103") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then NM1Row("NM104") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else NM1Row("NM104") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then NM1Row("NM105") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else NM1Row("NM105") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then NM1Row("NM106") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) Else NM1Row("NM106") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then NM1Row("NM107") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) Else NM1Row("NM107") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) <> "") Then NM1Row("NM108") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) Else NM1Row("NM108") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) <> "") Then NM1Row("NM109") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) Else NM1Row("NM109") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11) <> "") Then NM1Row("NM110") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11) Else NM1Row("NM110") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12) <> "") Then NM1Row("NM111") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12) Else NM1Row("NM111") = DBNull.Value

                            NM1Row("ROW_NUMBER") = _ROW_COUNT



                            NM1Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor

                            NM1Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            NM1Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                            NM1.Rows.Add(NM1Row)

                            '_Loop = 0

                            '_Loop = _LoopLevelMajor + _LoopLevelMinor


                            ' CHECK TO SE IF WERE AT THE 2100D 

                            'If _Loop = 2100 And _LoopLevelSubFix = "D" Then

                            '    _276_CLS_GUID = Guid.NewGuid
                            '    _276_22_CLS_NM1_FOUND = True
                            '    _LoopLevelMinor = 200

                            'End If



                            Select Case _HL03
                                Case 0
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case 19
                                    BUILD_CSR_NM1(_CurrentRowData)
                                    _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case 20
                                    BUILD_CSR_NM1(_CurrentRowData)
                                    _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case 21
                                    BUILD_CSR_NM1(_CurrentRowData)
                                    _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case Else
                                    _RAW_22 = _RAW_22 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select

                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "NM1", ex)
                    End Try





                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   TRN
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try


                        If _RowRecordType = "TRN" Then

                            _RowProcessedFlag = 1





                            Dim TRNRow As DataRow = TRN.NewRow
                            TRNRow("DOCUMENT_ID") = _DOCUMENT_ID
                            TRNRow("FILE_ID") = _FILE_ID
                            TRNRow("ISA_ID") = _ISA_ID
                            TRNRow("BATCH_ID") = _BATCH_ID
                            TRNRow("GS_ID") = _GS_ID
                            TRNRow("ST_ID") = _ST_ID
                            TRNRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            TRNRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            TRNRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            TRNRow("HIPAA_HL_19_GUID") = _HIPAA_HL_19_GUID
                            TRNRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            TRNRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            TRNRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            TRNRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            TRNRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            TRNRow("276_ISL_GUID") = _276_ISL_GUID
                            TRNRow("276_IRL_GUID") = _276_IRL_GUID
                            TRNRow("276_SPL_GUID") = _276_SPL_GUID
                            TRNRow("276_CLS_GUID") = _276_CLS_GUID
                            TRNRow("276_SLS_GUID") = Guid.Empty
                            TRNRow("HL01") = _HL01
                            TRNRow("HL02") = _HL02
                            TRNRow("HL03") = _HL03
                            TRNRow("HL04") = _HL04





                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then
                                TRNRow("TRN01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2)
                                TRNLookup(ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2))
                            Else
                                TRNRow("TRN01") = DBNull.Value
                            End If






                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then TRNRow("TRN02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else TRNRow("TRN02") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then TRNRow("TRN03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else TRNRow("TRN03") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then TRNRow("TRN04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else TRNRow("TRN04") = DBNull.Value

                            'TRNRow("FILE_ID") = _File_ID
                            'TRNRow("BATCH_ID") = _BATCH_ID
                            'TRNRow("GS_ID") = _GS_ID
                            'TRNRow("ST_ID") = _ST_ID
                            'TRNRow("STC_ID") = _STC_ID

                            TRNRow("ROW_NUMBER") = _ROW_COUNT



                            TRNRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            TRNRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            TRNRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                            TRN.Rows.Add(TRNRow)


                            Select Case _HL03
                                Case 0
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case 19
                                    _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case 20
                                    _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case 21
                                    _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case Else
                                    _RAW_22 = _RAW_22 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select




                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "TRN", ex)
                    End Try






                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   REF
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "REF" Then
                            _RowProcessedFlag = 1

                            Dim REFRow As DataRow = REF.NewRow
                            REFRow("DOCUMENT_ID") = _DOCUMENT_ID
                            REFRow("FILE_ID") = _FILE_ID
                            REFRow("BATCH_ID") = _BATCH_ID
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
                            REFRow("276_ISL_GUID") = _276_ISL_GUID
                            REFRow("276_IRL_GUID") = _276_IRL_GUID
                            REFRow("276_SPL_GUID") = _276_SPL_GUID
                            REFRow("276_CLS_GUID") = _276_CLS_GUID
                            REFRow("276_SLS_GUID") = _276_SLS_GUID
                            REFRow("HL01") = _HL01
                            REFRow("HL02") = _HL02
                            REFRow("HL03") = _HL03
                            REFRow("HL04") = _HL04
                            ' REFRow("P_GUID") = _P_GUID


                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then REFRow("REF01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else REFRow("REF01") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then REFRow("REF02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else REFRow("REF02") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then REFRow("REF03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else REFRow("REF03") = DBNull.Value

                            REFRow("ROW_NUMBER") = _ROW_COUNT

                            REFRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            REFRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            REFRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                            REF.Rows.Add(REFRow)

                            Select Case _HL03
                                Case 0
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case 19
                                    _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case 20
                                    _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case 21
                                    _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case Else
                                    _RAW_22 = _RAW_22 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select


                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "REF", ex)

                    End Try





                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   AMT
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "AMT" Then

                            _RowProcessedFlag = 1

                            Dim AMTRow As DataRow = AMT.NewRow
                            AMTRow("DOCUMENT_ID") = _DOCUMENT_ID
                            AMTRow("FILE_ID") = _FILE_ID
                            AMTRow("BATCH_ID") = _BATCH_ID
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
                            AMTRow("276_ISL_GUID") = _276_ISL_GUID
                            AMTRow("276_IRL_GUID") = _276_IRL_GUID
                            AMTRow("276_SPL_GUID") = _276_SPL_GUID
                            AMTRow("276_CLS_GUID") = _276_CLS_GUID
                            AMTRow("276_SLS_GUID") = Guid.Empty
                            AMTRow("HL01") = _HL01
                            AMTRow("HL02") = _HL02
                            AMTRow("HL03") = _HL03
                            AMTRow("HL04") = _HL04

                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then AMTRow("AMT01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else AMTRow("AMT01") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then AMTRow("AMT02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else AMTRow("AMT02") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then AMTRow("AMT03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else AMTRow("AMT03") = DBNull.Value
                            '    If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then AMTRow("AMT04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else AMTRow("AMT04") = DBNull.Value


                            AMTRow("ROW_NUMBER") = _ROW_COUNT
                            AMTRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            AMTRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            AMTRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                            AMT.Rows.Add(AMTRow)


                            Select Case _HL03
                                Case 0
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case 19
                                    _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case 20
                                    _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case 21
                                    _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"

                                Case Else
                                    _RAW_22 = _RAW_22 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select


                        End If



                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "AMT", ex)
                    End Try


                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   DTP
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try



                        If _RowRecordType = "DTP" Then
                            _RowProcessedFlag = 1




                            Dim DTPRow As DataRow = DTP.NewRow
                            DTPRow("DOCUMENT_ID") = _DOCUMENT_ID
                            DTPRow("FILE_ID") = _FILE_ID
                            DTPRow("BATCH_ID") = _BATCH_ID
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
                            DTPRow("276_ISL_GUID") = _276_ISL_GUID
                            DTPRow("276_IRL_GUID") = _276_IRL_GUID
                            DTPRow("276_SPL_GUID") = _276_SPL_GUID
                            DTPRow("276_CLS_GUID") = _276_CLS_GUID
                            DTPRow("276_SLS_GUID") = _276_SLS_GUID
                            DTPRow("HL01") = _HL01
                            DTPRow("HL02") = _HL02
                            DTPRow("HL03") = _HL03
                            DTPRow("HL04") = _HL04

                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then DTPRow("DTP01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else DTPRow("DTP01") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then DTPRow("DTP02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else DTPRow("DTP02") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then DTPRow("DTP03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else DTPRow("DTP03") = DBNull.Value
                            '    If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then DTPRow("DTP04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else DTPRow("DTP04") = DBNull.Value


                            DTPRow("ROW_NUMBER") = _ROW_COUNT
                            DTPRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            DTPRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            DTPRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                            DTP.Rows.Add(DTPRow)


                            Select Case _HL03
                                Case 0
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case 19
                                    _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case 20
                                    _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case 21
                                    _RAW_20 = _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case Else
                                    _RAW_22 = _RAW_22 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select


                        End If

                        '_276_SLS_GUID = Guid.NewGuid()
                        ''    CLEAR_SLS_DATA()


                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "DTP", ex)
                    End Try



                End If


                '        End If








                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   SE
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "SE" Then




                        Dim SERow As DataRow = SE_N.NewRow
                        SERow("FILE_ID") = _FILE_ID
                        SERow("ISA_ID") = _ISA_ID
                        SERow("BATCH_ID") = _BATCH_ID
                        SERow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                        SERow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                        SERow("HIPAA_ST_GUID") = _HIPAA_ST_GUID

                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then SERow("SE01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else SERow("SE01") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then SERow("SE02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else SERow("SE02") = DBNull.Value

                        SERow("ROW_NUMBER") = _ROW_COUNT
                        SE_N.Rows.Add(SERow)

                        ' COMMIT THE LAST LK SET SINCE WE WONT GET TO lx AGAIN AND THE LAST clm SET
                        '  ComitLX()
                        'ComitRowData()


                        If _HL22_DIRTY Then
                            ComitHL22()
                        End If

                        ComitSE()

                        ClearST()


                        _RowProcessedFlag = 1


                    End If
                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "SE", ex)
                End Try



                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   GE
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "GE" Then






                        Dim GERow As DataRow = GE_N.NewRow
                        GERow("FILE_ID") = _FILE_ID
                        GERow("ISA_ID") = _ISA_ID
                        GERow("BATCH_ID") = _BATCH_ID
                        GERow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                        GERow("HIPAA_GS_GUID") = _HIPAA_GS_GUID


                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then GERow("GE01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else GERow("GE01") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then GERow("GE02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else GERow("GE02") = DBNull.Value

                        GERow("ROW_NUMBER") = _ROW_COUNT
                        GE_N.Rows.Add(GERow)


                        ComitGE()
                        ClearGS()
                        'committ the last ST
                        _RowProcessedFlag = 1



                    End If
                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GE", ex)
                End Try



                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   IEA
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "IEA" Then




                        Dim IEARow As DataRow = IEA_N.NewRow
                        IEARow("FILE_ID") = _FILE_ID
                        IEARow("ISA_ID") = _ISA_ID
                        IEARow("BATCH_ID") = _BATCH_ID
                        IEARow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID


                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then IEARow("IEA01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else IEARow("IEA01") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then IEARow("IEA02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else IEARow("IEA02") = DBNull.Value

                        IEARow("ROW_NUMBER") = _ROW_COUNT
                        IEA_N.Rows.Add(IEARow)


                        ComitIEA()

                        _LAST_ISA_ID = _LAST_ISA_ID + " - " + Convert.ToString(_ISA_ID)
                        ClearIAS()
                        _RowProcessedFlag = 1

                    End If
                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "IEA", ex)
                End Try






                Try
                    If _RowProcessedFlag = 0 Then

                        _hasUNK = True

                        Dim UNKRow As DataRow = UNK.NewRow
                        UNKRow("DOCUMENT_ID") = _DOCUMENT_ID
                        UNKRow("FILE_ID") = _FILE_ID
                        UNKRow("BATCH_ID") = _BATCH_ID
                        UNKRow("ISA_ID") = _ISA_ID
                        UNKRow("GS_ID") = _GS_ID
                        UNKRow("ST_ID") = _ST_ID
                        UNKRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                        UNKRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                        UNKRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                        UNKRow("HIPAA_HL_19_GUID") = _HIPAA_HL_19_GUID
                        UNKRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                        UNKRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                        UNKRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                        UNKRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                        UNKRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                        UNKRow("HL01") = _HL01
                        UNKRow("HL02") = _HL02
                        UNKRow("HL03") = _HL03
                        UNKRow("HL04") = _HL04
                        UNKRow("ROW_RECORD_TYPE") = _RowRecordType
                        UNKRow("ROW_DATA") = _CurrentRowData
                        UNKRow("ROW_NUMBER") = _ROW_COUNT
                        UNKRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        UNKRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                        UNKRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                        UNK.Rows.Add(UNKRow)

                        Select Case _HL03
                            Case 0
                                _RAW_HEADER = "UNK::" + _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case 19
                                _RAW_20 = "UNK::" + _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case 20
                                _RAW_20 = "UNK::" + _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case 21
                                _RAW_20 = "UNK::" + _RAW_20 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case Else
                                _RAW_22 = "UNK::" + _RAW_22 + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                        End Select

                    End If

                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "UNK", ex)
                End Try




            Next


            If _hasUNK Then

                If (_IS_FILE) Then


                    Using e As New EDI_5010_LOGGING
                        e.ConnectionString = _ConnectionString
                        e.TransactionSetIdentifierCode = "276"
                        If _hasERR Then
                            e.UpdateFileStatus(CInt(_FILE_ID), "PARSE COMPLETE WITH UNK SEGMENTS WITH ERRORS", "276")
                            _IMPORT_RETURN_STRING = "276 : PARSE COMPLETE WITH UNK SEGMENTS WITH ERRORS  :: ISA_ID = " + Convert.ToString(_LAST_ISA_ID)
                        Else
                            e.UpdateFileStatus(CInt(_FILE_ID), "PARSE COMPLETE WITH UNK SEGMENTS", "276")
                            _IMPORT_RETURN_STRING = "276 : PARSE COMPLETE WITH UNK SEGMENTS  :: ISA_ID = " + Convert.ToString(_LAST_ISA_ID)
                        End If



                    End Using

                Else
                    If _hasERR Then

                        _IMPORT_RETURN_STRING = "276 : PARSE COMPLETE WITH UNK SEGMENTS WITH ERRORS  :: ISA_ID = " + Convert.ToString(_LAST_ISA_ID)
                    Else

                        _IMPORT_RETURN_STRING = "276 : PARSE COMPLETE WITH UNK SEGMENTS  :: ISA_ID = " + Convert.ToString(_LAST_ISA_ID)
                    End If

                End If
                ComitUNK()

            Else
                If (_IS_FILE) Then


                    Using e As New EDI_5010_LOGGING
                        e.ConnectionString = _ConnectionString
                        e.TransactionSetIdentifierCode = "276"
                        If _hasERR Then
                            e.UpdateFileStatus(CInt(_FILE_ID), "PARSE COMPLETE WITH ERRORS", "276")
                            _IMPORT_RETURN_STRING = "276 : PARSE COMPLETE WITH ERRORS"
                        Else
                            e.UpdateFileStatus(CInt(_FILE_ID), "PARSE COMPLETE", "276")
                            _IMPORT_RETURN_STRING = "276 : PARSE COMPLETE  :: ISA_ID = " + Convert.ToString(_LAST_ISA_ID)
                        End If
                    End Using
                Else

                    If _hasERR Then

                        _IMPORT_RETURN_STRING = "276 : PARSE COMPLETE WITH ERRORS  :: ISA_ID = " + Convert.ToString(_LAST_ISA_ID)
                    Else

                        _IMPORT_RETURN_STRING = "276 : PARSE COMPLETE  :: ISA_ID = " + Convert.ToString(_LAST_ISA_ID)
                    End If


                End If




            End If



            'If _hasERR Then
            '    RollBack()
            '    _IMPORT_RETURN_STRING = _IMPORT_RETURN_STRING '+ " ROLLED BACK EDI_SEQUENCE_NUMBER " + Convert.ToString(_EDI_SEQUENCE_NUMBER)
            'End If



            Cleanup()

            If _ImportReturnCode >= 0 Then
                Return CInt(_BATCH_ID)
            Else
                Return _ImportReturnCode
            End If




        End Function

        Private Function RollBack() As Integer
            _FUNCTION_NAME = "Function TestROLLBACK() As Integer"

            Dim _RETURN_CODE As Integer = -1

            Try

                'Using Con As New SqlConnection(_ConnectionString)
                '    Con.Open()
                '    Using cmd As New SqlCommand(_SP_ROLLBACK, Con)

                '        cmd.CommandType = CommandType.StoredProcedure

                '        cmd.Parameters.AddWithValue("@EDI_SEQUENCE_NUMBER", _EDI_SEQUENCE_NUMBER)


                '        '    cmd.ExecuteNonQuery()

                '    End Using
                '    Con.Close()
                'End Using

                _RETURN_CODE = 0

            Catch ex As Exception
                _hasERR = True
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "", ex)
            End Try

            Return _RETURN_CODE

        End Function

        Private Function Cleanup() As Integer

            _FUNCTION_NAME = "Function CLEANUP() As Integer"
            Dim _RETURN_CODE As Integer = -1

            Dim FinalPath As String = String.Empty

            Try

                Dim span As TimeSpan

                span = _ProcessEndTime - _ProcessStartTime
                _ProcessElaspedTime = CLng(span.TotalMilliseconds)
                _RETURN_CODE = 0
            Catch ex As Exception
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "", ex)
            End Try

            Return _RETURN_CODE
        End Function


        Private Function ComitUNK() As Integer
            Dim _RETURN_CODE As Integer = -1

            _FUNCTION_NAME = "Function ComitUNK As Integer"


            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_COMIT_UNKNOWN, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_5010_UNK", UNK)
                        cmd.Parameters.AddWithValue("@IMPORTER", _CLASS_NAME)
                        cmd.Parameters.AddWithValue("@ST03", _ST03_ImplementationConventionReference)


                        cmd.ExecuteNonQuery()
                    End Using
                    Con.Close()
                End Using

                _RETURN_CODE = 0

            Catch ex As Exception
                _hasERR = True
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "", ex)
            End Try

            Return _RETURN_CODE

        End Function
 


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ComitISA() As Integer

            _FUNCTION_NAME = "Function ComitISA() As Integer"


            Dim _RETURN_CODE As Integer = -1


            Try
                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_ISA, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_276_ISA", ISA)



                        '' this will only be here in subba console
                        cmd.Parameters.AddWithValue("@request_xml", _RAW_EDI)



                        cmd.Parameters.AddWithValue("@DELETE_FLAG", _DELETE_FLAG)
                        ' messed up
                        cmd.Parameters.AddWithValue("@cbr_id", _CBR_ID)
                        cmd.Parameters.AddWithValue("@user_id", _USER_ID)
                        cmd.Parameters.AddWithValue("@hosp_code", _HOSP_CODE)
                        cmd.Parameters.AddWithValue("@source", _SOURCE)


                        ''cmd.Parameters.AddWithValue("@EDI", _RAW_EDI)
                        cmd.Parameters.AddWithValue("@PAYOR_ID", _PAYOR_ID)
                        cmd.Parameters.AddWithValue("@Vendor_name", _VENDOR_NAME)

                        cmd.Parameters.AddWithValue("@pat_hosp_code", _PAT_HOSP_CODE)
                        cmd.Parameters.AddWithValue("@ins_type", _INS_TYPE)

                        cmd.Parameters.AddWithValue("@Patient_number", _PATIENT_NUMBER)
                        '
                        '  cmd.Parameters.AddWithValue("@Log_EDI", _LOG_EDI)


                        cmd.Parameters.AddWithValue("@batch_id", _BATCH_ID)
                        cmd.Parameters("@batch_id").Direction = ParameterDirection.InputOutput

                        '   cmd.Parameters.AddWithValue("@batch_id", _BATCH_ID)







                        cmd.Parameters.Add("@ISA_ID", Data.SqlDbType.BigInt, 1)
                        cmd.Parameters("@ISA_ID").Direction = ParameterDirection.Output
                        cmd.ExecuteNonQuery()
                        _BATCH_ID = Convert.ToInt64(cmd.Parameters("@batch_id").Value)
                        _ISA_ID = Convert.ToInt32(cmd.Parameters("@ISA_ID").Value.ToString())
                    End Using
                    Con.Close()
                End Using
                _RETURN_CODE = 0

            Catch ex As Exception
                _hasERR = True
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "", ex)
            End Try

            Return _RETURN_CODE

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ComitGS() As Integer

            _FUNCTION_NAME = "Function ComitGS() As Integer"

            Dim _RETURN_CODE As Integer = -1
            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_GS, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_276_GS", GS)



                        cmd.Parameters.Add("@GS_ID", Data.SqlDbType.BigInt, 1)
                        cmd.Parameters("@GS_ID").Direction = ParameterDirection.Output

                        cmd.ExecuteNonQuery()

                        _GS_ID = Convert.ToInt32(cmd.Parameters("@GS_ID").Value.ToString())


                    End Using
                    Con.Close()
                End Using

                _RETURN_CODE = 0
            Catch ex As Exception
                _hasERR = True
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "", ex)
            End Try

            Return _RETURN_CODE

        End Function


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ComitST() As Int32

            _FUNCTION_NAME = "Function ComitST() As Integer"

            Dim _RETURN_CODE As Integer = -1

            Try



                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_ST, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_276_ST", ST)
                        cmd.Parameters.AddWithValue("@HIPAA_276_BHT", BHT)



                        cmd.Parameters.Add("@ST_ID", Data.SqlDbType.BigInt, 1)
                        cmd.Parameters("@ST_ID").Direction = ParameterDirection.Output
                        cmd.ExecuteNonQuery()

                        _ST_ID = Convert.ToInt32(cmd.Parameters("@ST_ID").Value.ToString())
                    End Using
                    Con.Close()
                End Using

                _RETURN_CODE = 0
            Catch ex As Exception
                _hasERR = True
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "", ex)
            End Try

            Return _RETURN_CODE

        End Function


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ComitHL20() As Integer

            _FUNCTION_NAME = "Function ComiTHL20() As Integer"

            Dim _RETURN_CODE As Integer = -1

            Try



                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_COMIT_HL20_DATA, Con)

                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.AddWithValue("@HIPAA_276_HL", HL)
                        cmd.Parameters.AddWithValue("@HIPAA_276_NM1", NM1)



                        cmd.Parameters.AddWithValue("@RAW_HEADER", _RAW_HEADER)
                        cmd.Parameters.AddWithValue("@RAW_20", _RAW_20)

                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()


                    CLEAR_HEADER_DATA()
                End Using

                _RETURN_CODE = 0
            Catch ex As Exception
                _hasERR = True
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "", ex)
            End Try

            Return _RETURN_CODE
        End Function


        Private Function ComitHL22() As Integer

            _FUNCTION_NAME = "Function ComitHL22() As Integer"

            Dim _RETURN_CODE As Integer = -1

            Try


                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_COMIT_HL22_DATA, Con)

                        cmd.CommandType = CommandType.StoredProcedure


                        cmd.Parameters.AddWithValue("@HIPAA_276_AMT", AMT)



                        cmd.Parameters.AddWithValue("@HIPAA_276_DMG", DMG)
                        cmd.Parameters.AddWithValue("@HIPAA_276_DTP", DTP)


                        cmd.Parameters.AddWithValue("@HIPAA_276_HL", HL)

                        cmd.Parameters.AddWithValue("@HIPAA_276_NM1", NM1)


                        cmd.Parameters.AddWithValue("@HIPAA_276_REF", REF)

                        cmd.Parameters.AddWithValue("@HIPAA_276_SVC", SVC)

                        cmd.Parameters.AddWithValue("@HIPAA_276_TRN", TRN) ' 

                        cmd.Parameters.AddWithValue("@HIPAA_276_CSR_ST", ST)
                        cmd.Parameters.AddWithValue("@HIPAA_276_CSR_BHT", BHT)






                        cmd.Parameters.AddWithValue("@HIPAA_276_CSR_NM1", CSR_NM1)


                        _ST_LINE = String.Empty
                        _ST_LINE = _ST + _BHT + _RAW_20 + _RAW_22

                        cmd.Parameters.AddWithValue("@HIPAA_276_ST_STRING", _ST_LINE)

                        cmd.Parameters.AddWithValue("@RAW_HEADER", _RAW_HEADER)
                        cmd.Parameters.AddWithValue("@RAW_20", _RAW_20)
                        cmd.Parameters.AddWithValue("@RAW_22", _RAW_22)




                        cmd.Parameters.AddWithValue("@FILE_ID", _FILE_ID)
                        cmd.Parameters.AddWithValue("@ISA_ID", _ISA_ID)
                        cmd.Parameters.AddWithValue("@GS_ID", _GS_ID)
                        cmd.Parameters.AddWithValue("@ST_ID", _ST_ID)


                        '' this will only be here in subba console
                        cmd.Parameters.AddWithValue("@request_xml", _RAW_EDI)



                        cmd.Parameters.AddWithValue("@DELETE_FLAG", _DELETE_FLAG)
                        ' messed up
                        cmd.Parameters.AddWithValue("@cbr_id", _CBR_ID)
                        cmd.Parameters.AddWithValue("@user_id", _USER_ID)
                        cmd.Parameters.AddWithValue("@hosp_code", _HOSP_CODE)
                        cmd.Parameters.AddWithValue("@source", _SOURCE)


                        ''cmd.Parameters.AddWithValue("@EDI", _RAW_EDI)
                        cmd.Parameters.AddWithValue("@PAYOR_ID", _PAYOR_ID)
                        cmd.Parameters.AddWithValue("@Vendor_name", _VENDOR_NAME)

                        cmd.Parameters.AddWithValue("@pat_hosp_code", _PAT_HOSP_CODE)
                        cmd.Parameters.AddWithValue("@ins_type", _INS_TYPE)

                        cmd.Parameters.AddWithValue("@Patient_number", _PATIENT_NUMBER)
                        '
                        '  cmd.Parameters.AddWithValue("@Log_EDI", _LOG_EDI)


                        cmd.Parameters.AddWithValue("@batch_id", _BATCH_ID)
                        cmd.Parameters("@batch_id").Direction = ParameterDirection.InputOutput

                        '   cmd.Parameters.AddWithValue("@batch_id", _BATCH_ID)



                        cmd.ExecuteNonQuery()

                        '  _BATCH_ID = Convert.ToInt64(cmd.Parameters("@batch_id").Value)


                    End Using
                    Con.Close()
                End Using
                CLEAR_22_DATA()

                _RETURN_CODE = 0

            Catch ex As Exception
                _hasERR = True
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "", ex)
            End Try



            Return _RETURN_CODE

        End Function



        Private Function ComitSE() As Integer

            _FUNCTION_NAME = "Function ComitSE() As Integer"

            Dim _RETURN_CODE As Integer = -1

            Try
                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_SE, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_276_SE", SE_N)



                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using

                _RETURN_CODE = 0
            Catch ex As Exception
                _hasERR = True
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "", ex)
            End Try

            Return _RETURN_CODE

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ComitGE() As Integer

            _FUNCTION_NAME = "Function ComitGE() As Integer"

            Dim _RETURN_CODE As Integer = -1

            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_GE, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_276_GE", GE_N)



                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using
                _RETURN_CODE = 0
            Catch ex As Exception
                _hasERR = True
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "", ex)
            End Try

            Return _RETURN_CODE

        End Function


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ComitIEA() As Integer

            _FUNCTION_NAME = "Function ComitIEA() As Integer"

            Dim _RETURN_CODE As Integer = -1

            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_IEA, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_276_IEA", IEA_N)


                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using

                _RETURN_CODE = 0

            Catch ex As Exception
                _hasERR = True
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "", ex)
            End Try

            Return _RETURN_CODE

        End Function


        Public Function TestSPs() As Integer


            _FUNCTION_NAME = "Function TestSPs() As Integer"

            Dim _RETURN_CODE As Integer = -1



            Try


                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_COMIT_HL20_UNIT_TEST, Con)

                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.AddWithValue("@HIPAA_276_HL", HL)
                        cmd.Parameters.AddWithValue("@HIPAA_276_NM1", NM1)
                        cmd.Parameters.AddWithValue("@HIPAA_276_PER", PER)
                        cmd.Parameters.AddWithValue("@HIPAA_276_TRN", TRN)
                        cmd.Parameters.AddWithValue("@HIPAA_276_STC", STC)

                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using

                _RETURN_CODE = 0
                _HL20_SP_TEST = True
            Catch ex As Exception
                _HL20_SP_TEST = False
                log.ExceptionDetails("HL 20 TEST FAILED 005010X212_276", ex)
            End Try




            Try



                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_COMIT_HL22_UNIT_TEST, Con)

                        cmd.CommandType = CommandType.StoredProcedure


                        cmd.Parameters.AddWithValue("@HIPAA_276_AMT", AMT)
                        cmd.Parameters.AddWithValue("@HIPAA_276_DTP", DTP)
                        cmd.Parameters.AddWithValue("@HIPAA_276_HL", HL)
                        cmd.Parameters.AddWithValue("@HIPAA_276_NM1", NM1)
                        cmd.Parameters.AddWithValue("@HIPAA_276_PER", PER)
                        cmd.Parameters.AddWithValue("@HIPAA_276_REF", REF)
                        cmd.Parameters.AddWithValue("@HIPAA_276_STC", STC)
                        cmd.Parameters.AddWithValue("@HIPAA_276_SVC", SVC)
                        cmd.Parameters.AddWithValue("@HIPAA_276_TRN", TRN)




                        cmd.Parameters.AddWithValue("@HIPAA_276_CACHE_HL", CACHE_HL)
                        cmd.Parameters.AddWithValue("@HIPAA_276_CACHE_NM1", CACHE_NM1)
                        cmd.Parameters.AddWithValue("@HIPAA_276_CACHE_PER", CACHE_PER)
                        cmd.Parameters.AddWithValue("@HIPAA_276_CACHE_STC", CACHE_STC)
                        cmd.Parameters.AddWithValue("@HIPAA_276_CACHE_TRN", CACHE_TRN)
                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using

                _RETURN_CODE = 0

            Catch ex As Exception
                _hasERR = True
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "", ex)
            End Try

            Return _RETURN_CODE



        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub ClearIAS()
            ISA.Clear()
            IEA_N.Clear()
            _HIPAA_ISA_GUID = Guid.Empty
            _ISA_ID = 0

            _RowProcessedFlag = 0
            _DataElementSeparator = String.Empty
            _RepetitionSeparator = String.Empty
            _ComponentElementSeparator = String.Empty

            ClearGS()

        End Sub


        Private Sub ClearGS()

            GS.Clear()
            GE_N.Clear()
            _HIPAA_GS_GUID = Guid.Empty
            _GS_ID = 0
            ClearST()

        End Sub

        Private Sub ClearST()

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   BEGIN COMMON
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            ST.Clear()
            SE_N.Clear()
            BHT.Clear()
            _ST_ID = 0
            _HIPAA_ST_GUID = Guid.Empty


            _ST = String.Empty
            _BHT = String.Empty

            _RAW_HEADER_BUILT = False
            _RAW_HEADER = String.Empty
            _RAW_20 = String.Empty
            _RAW_22 = String.Empty

            CSR_NM1.Clear()

            _HIPAA_HL_20_GUID = Guid.Empty
            _HIPAA_HL_21_GUID = Guid.Empty
            _HIPAA_HL_22_GUID = Guid.Empty
            _HIPAA_HL_23_GUID = Guid.Empty
            _HIPAA_HL_24_GUID = Guid.Empty

            _HL19_DIRTY = False
            _HL20_DIRTY = False
            _HL21_DIRTY = False
            _HL22_DIRTY = False
            _HL23_DIRTY = False
            _HL24_DIRTY = False

            _HL01 = 0
            _HL02 = 0
            _HL03 = 0
            _HL04 = 0


            ST.Clear()
            SE.Clear()
            BHT.Clear()


            _BHT01 = String.Empty
            _BHT02 = String.Empty
            _BHT03 = String.Empty
            _BHT04 = String.Empty
            _BHT05 = String.Empty
            _BHT06 = String.Empty


            _ST01 = String.Empty
            _ST02 = String.Empty
            _ST03 = String.Empty


            _LoopLevelMajor = 0
            _LoopLevelMinor = 0
            _LoopLevelSubFix = ""


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   END COMMON
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '  BEGIN 276 
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            CLEAR_HEADER_DATA()


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '  END 276 
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        End Sub


        Private Sub CLEAR_HEADER_DATA()


            CACHE_HL = HL.Copy
            CACHE_NM1 = NM1.Copy
            CACHE_PER = PER.Copy
            CACHE_TRN = TRN.Copy
            CACHE_STC = STC.Copy

            HL.Clear()
            NM1.Clear()
            PER.Clear()
            TRN.Clear()
            STC.Clear()

            '    _HL_PARENT = String.Empty
            ' _RAW_20 = String.Empty
            _RAW_22 = String.Empty

            _276_TRN_GUID = Guid.Empty
            _276_STC_GUID = Guid.Empty
            _276_SVC_GUID = Guid.Empty
            _276_P_STC_GUID = Guid.Empty

            _NM1_GUID = Guid.Empty
            CLEAR_CLS_DATA()

        End Sub





        Private Sub CACHE_CLS_DATA()

            '  CACHE_HL = HL.Copy
            CLS_DTP = DTP.Copy
            CLS_REF = REF.Copy
            CLS_STC = STC.Copy




        End Sub


        Private Sub CLEAR_22_DATA()
            HL.Clear()
            NM1.Clear()
            _RAW_22 = String.Empty
            _276_CLS_GUID = Guid.Empty
            _276_SLS_GUID = Guid.Empty

            _276_2200D_DTP_FOUND = False

            _LoopLevelMinor = 200

            CLEAR_CLS_DATA()

        End Sub


        Private Sub CLEAR_CLS_DATA()




            DTP.Clear()
            REF.Clear()
            STC.Clear()
            TRN.Clear()
            AMT.Clear()
            DMG.Clear()



            _276_CLS_GUID = Guid.Empty
            _276_SLS_GUID = Guid.Empty
            CLEAR_SLS_DATA()

        End Sub


        Private Sub CLEAR_SLS_DATA()


            SVC.Clear()
            STC.Clear()
            DTP.Clear()
            REF.Clear()
            _276_SLS_GUID = Guid.Empty

        End Sub

        Private Sub MERGE_TABLES()

            Using m As New DataTableMaintance
                m.MergeTable(CLS_STC, STC)
                m.MergeTable(CLS_REF, REF)
                m.MergeTable(CLS_DTP, DTP)
                m.MergeTable(CLS_TRN, TRN)
            End Using

        End Sub



        Public Sub TRNLookup(ByVal TRN01 As String)

            Select Case TRN01

                Case "2"
                    If _LoopLevelMajor = 2000 Then

                        If _LoopLevelSubFix = "B" Then
                            _LoopLevelMinor = 200
                        End If

                        If _LoopLevelSubFix = "D" Then
                            _LoopLevelMinor = 200
                        End If

                    End If
                Case "1"
                    If _LoopLevelMajor = 2000 Then
                        _LoopLevelMinor = 200
                        _LoopLevelSubFix = "C"
                    End If

            End Select
        End Sub

        Public Sub NM1Lookup(ByVal nm01 As String)
            Dim s As String = String.Empty

            Select Case nm01

                'Case "40"
                '  _LoopLevelMajor = 1000
                '  _LoopLevelSubFix = "B"


                Case "41"
                    If _LoopLevelMajor = 2000 Then
                        _LoopLevelMinor = 100
                        _LoopLevelSubFix = "D"
                    End If


                Case "1P"

                    If _LoopLevelMajor = 2000 Then
                        _LoopLevelMinor = 100
                        _LoopLevelSubFix = "C"

                    End If




                    'Case "71"
                    '    If _LoopLevelMajor = 2330 Then

                    '        _LoopLevelSubFix = "C"
                    '    Else
                    '        _LoopLevelMajor = 2310
                    '        _LoopLevelSubFix = "A"
                    '    End If

                    'Case "72"
                    '    If _LoopLevelMajor = 2330 Then

                    '        _LoopLevelSubFix = "D"
                    '    Else
                    '        _LoopLevelMajor = 2310
                    '        _LoopLevelSubFix = "B"
                    '    End If

                    'Case "77"

                    '    If _LoopLevelMajor = 2330 Then

                    '        _LoopLevelSubFix = "F"
                    '    Else
                    '        _LoopLevelMajor = 2310
                    '        _LoopLevelSubFix = "E"
                    '    End If

                    'Case "85"
                    '    _LoopLevelMajor = 2010
                    '    _LoopLevelSubFix = "AA"


                    'Case "87"
                    '    _LoopLevelMajor = 2010
                    '    _LoopLevelSubFix = "AB"


                Case "IL"

                    If _LoopLevelMajor = 2000 Then
                        _LoopLevelMinor = 100
                        _LoopLevelSubFix = "D"

                        _276_2200D_BEGIN = True
                        _276_CLS_GUID = Guid.NewGuid
                    End If
                    '        _LoopLevelMajor = 2010
                    '        _LoopLevelSubFix = "BA"
                    '    End If



                Case "PR"

                    If _LoopLevelMajor = 2000 Then
                        _LoopLevelMinor = 100
                        _LoopLevelSubFix = "A"
                    End If



                    'Case "QC"
                    '    _LoopLevelMajor = 2010
                    '    _LoopLevelSubFix = "CA"


                    'Case "ZZ"
                    '    _LoopLevelMajor = 2310
                    '    _LoopLevelSubFix = "C"


                Case Else





            End Select




        End Sub


        Public Sub BUILD_CSR_NM1(ByVal NM1_ROWDATA As String)



            Dim CSR_NM1Row As DataRow = CSR_NM1.NewRow
            CSR_NM1Row("DOCUMENT_ID") = _DOCUMENT_ID
            CSR_NM1Row("FILE_ID") = _FILE_ID
            CSR_NM1Row("ISA_ID") = _ISA_ID
            CSR_NM1Row("GS_ID") = _GS_ID
            CSR_NM1Row("ST_ID") = _ST_ID
            CSR_NM1Row("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
            CSR_NM1Row("HIPAA_GS_GUID") = _HIPAA_GS_GUID
            CSR_NM1Row("HIPAA_ST_GUID") = _HIPAA_ST_GUID
            CSR_NM1Row("HIPAA_HL_19_GUID") = _HIPAA_HL_19_GUID
            CSR_NM1Row("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
            CSR_NM1Row("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
            CSR_NM1Row("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
            CSR_NM1Row("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID

            CSR_NM1Row("HL01") = _HL01
            CSR_NM1Row("HL02") = _HL02
            CSR_NM1Row("HL03") = _HL03
            CSR_NM1Row("HL04") = _HL04


            If (ss.ParseDemlimtedString(NM1_ROWDATA, _DataElementSeparator, 3) <> "") Then CSR_NM1Row("NM102") = ss.ParseDemlimtedString(NM1_ROWDATA, _DataElementSeparator, 3) Else CSR_NM1Row("NM102") = DBNull.Value
            If (ss.ParseDemlimtedString(NM1_ROWDATA, _DataElementSeparator, 4) <> "") Then CSR_NM1Row("NM103") = ss.ParseDemlimtedString(NM1_ROWDATA, _DataElementSeparator, 4) Else CSR_NM1Row("NM103") = DBNull.Value
            If (ss.ParseDemlimtedString(NM1_ROWDATA, _DataElementSeparator, 5) <> "") Then CSR_NM1Row("NM104") = ss.ParseDemlimtedString(NM1_ROWDATA, _DataElementSeparator, 5) Else CSR_NM1Row("NM104") = DBNull.Value
            If (ss.ParseDemlimtedString(NM1_ROWDATA, _DataElementSeparator, 6) <> "") Then CSR_NM1Row("NM105") = ss.ParseDemlimtedString(NM1_ROWDATA, _DataElementSeparator, 6) Else CSR_NM1Row("NM105") = DBNull.Value
            If (ss.ParseDemlimtedString(NM1_ROWDATA, _DataElementSeparator, 7) <> "") Then CSR_NM1Row("NM106") = ss.ParseDemlimtedString(NM1_ROWDATA, _DataElementSeparator, 7) Else CSR_NM1Row("NM106") = DBNull.Value
            If (ss.ParseDemlimtedString(NM1_ROWDATA, _DataElementSeparator, 8) <> "") Then CSR_NM1Row("NM107") = ss.ParseDemlimtedString(NM1_ROWDATA, _DataElementSeparator, 8) Else CSR_NM1Row("NM107") = DBNull.Value
            If (ss.ParseDemlimtedString(NM1_ROWDATA, _DataElementSeparator, 9) <> "") Then CSR_NM1Row("NM108") = ss.ParseDemlimtedString(NM1_ROWDATA, _DataElementSeparator, 9) Else CSR_NM1Row("NM108") = DBNull.Value
            If (ss.ParseDemlimtedString(NM1_ROWDATA, _DataElementSeparator, 10) <> "") Then CSR_NM1Row("NM109") = ss.ParseDemlimtedString(NM1_ROWDATA, _DataElementSeparator, 10) Else CSR_NM1Row("NM109") = DBNull.Value
            If (ss.ParseDemlimtedString(NM1_ROWDATA, _DataElementSeparator, 11) <> "") Then CSR_NM1Row("NM110") = ss.ParseDemlimtedString(NM1_ROWDATA, _DataElementSeparator, 11) Else CSR_NM1Row("NM110") = DBNull.Value
            If (ss.ParseDemlimtedString(NM1_ROWDATA, _DataElementSeparator, 12) <> "") Then CSR_NM1Row("NM111") = ss.ParseDemlimtedString(NM1_ROWDATA, _DataElementSeparator, 12) Else CSR_NM1Row("NM111") = DBNull.Value

            CSR_NM1Row("ROW_NUMBER") = _ROW_COUNT

            CSR_NM1Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
            CSR_NM1Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor
            CSR_NM1Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


            CSR_NM1.Rows.Add(CSR_NM1Row)


        End Sub

        Public WriteOnly Property pat_hosp_code As String

            Set(value As String)
                _PAT_HOSP_CODE = value
            End Set
        End Property


        Public WriteOnly Property cbr_id As Double


            Set(value As Double)
                _CBR_ID = Convert.ToInt64(value)
            End Set
        End Property

        Public WriteOnly Property user_id As String

            Set(value As String)
                _USER_ID = value
            End Set
        End Property


        Public WriteOnly Property hosp_code As String

            Set(value As String)
                _HOSP_CODE = value
            End Set
        End Property


        Public WriteOnly Property source As String

            Set(value As String)
                _SOURCE = value
            End Set
        End Property

        Public WriteOnly Property isFile As Boolean


            Set(value As Boolean)
                _IS_FILE = value
            End Set
        End Property


        Public WriteOnly Property DebugLevel As Integer

            Set(value As Integer)
                _DEBUG_LEVEL = value
            End Set
        End Property


        Public WriteOnly Property PAYOR_ID As String

            Set(value As String)
                _PAYOR_ID = value
            End Set
        End Property


        Public WriteOnly Property Vendor_name As String

            Set(value As String)
                _VENDOR_NAME = value
            End Set
        End Property




        Public Property LOG_EDI As String

            Set(value As String)
                _LOG_EDI = value
            End Set
            Get
                Return _LOG_EDI
            End Get
        End Property


        Public WriteOnly Property BatchID As Double

            Set(value As Double)
                _BATCH_ID = Convert.ToInt64(value)

            End Set
        End Property


        Public WriteOnly Property DeleteFlag As String

            Set(value As String)
                _DELETE_FLAG = value
            End Set
        End Property
        Public WriteOnly Property Verbose As Integer

            Set(value As Integer)
                _VERBOSE = value
            End Set
        End Property



        Public WriteOnly Property Dump As Integer

            Set(value As Integer)
                _DUMP = value
            End Set
        End Property

        Public Property ExecutionTime As TimeSpan
            Get
                Return _ElapsedTime


            End Get
            Set(value As TimeSpan)

            End Set
        End Property


        Public ReadOnly Property ErrorMessage As String
            Get
                Return _ERROR
            End Get

        End Property

        Public ReadOnly Property chars As String
            Get
                Return _CHARS
            End Get

        End Property

        Public ReadOnly Property Status As String
            Get
                Return _STATUS

            End Get

        End Property
        Public WriteOnly Property EDI As String

            Set(value As String)
                _RAW_EDI = value
            End Set
        End Property

        Public ReadOnly Property Reject_Reason_code As String
            Get
                Return _REJECT_REASON_CODE
            End Get

        End Property

        Public ReadOnly Property LOOP_AGAIN As String
            Get
                Return _LOOP_AGAIN

            End Get

        End Property

        Public WriteOnly Property Patient_number As String

            Set(value As String)
                _PATIENT_NUMBER = value
            End Set
        End Property


        Public WriteOnly Property ins_type As String

            Set(value As String)
                _INS_TYPE = value
            End Set
        End Property

    End Class

End Namespace
