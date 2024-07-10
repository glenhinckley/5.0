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

    Public Class EDI_270_005010X279A1_DNU

        Inherits EDI_5010_270_005010X279A1_TABLES

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

        Private _DocumentType As String = "005010X279A1"


        Private _SP_ISA As String = "[usp_EDI_5010_HIPAA_270_DUMP_ISA]"
        Private _SP_IEA As String = "[usp_EDI_5010_HIPAA_270_DUMP_IEA]"

        Private _SP_GS As String = "[usp_EDI_5010_HIPAA_270_DUMP_GS]"
        Private _SP_GE As String = "[usp_EDI_5010_HIPAA_270_DUMP_GE]"

        Private _SP_ST As String = "[usp_EDI_5010_HIPAA_270_DUMP_ST]"
        Private _SP_SE As String = "[usp_EDI_5010_HIPAA_270_DUMP_SE]"




        Private _SP_COMIT_2000A_UNIT_TEST As String = "[usp_EDI_5010_HIPAA_270_DUMP_UNIT_TEST_2000A_HEADER]"
        Private _SP_COMIT_2000B_UNIT_TEST As String = "[usp_EDI_5010_HIPAA_270_DUMP_UNIT_TEST_2000B_HEADER]"
        Private _SP_COMIT_HL22_UNIT_TEST As String = "[usp_EDI_5010_HIPAA_270_DUMP_UNIT_TEST_22]"


        Private _SP_COMIT_2000A_DATA As String = "[usp_EDI_5010_HIPAA_270_DUMP_2000A_HEADER]"
        Private _SP_COMIT_2000B_DATA As String = "[usp_EDI_5010_HIPAA_270_DUMP_2000B_HEADER]"
        Private _SP_COMIT_HL22_DATA As String = "[usp_EDI_5010_HIPAA_270_DUMP_22]"



        Private _SP_COMIT_UNKNOWN As String = "[usp_HIPAA_EDI_UNKNOWN]"

        Private _SP_ROLLBACK As String = "[usp_EDI_5010_HIPAA_270_ROLLBACK]"


        Private _EQIList As New List(Of String)
        Private _LAST_EQ As String = String.Empty
        Private _CURRENT_EQ As String = String.Empty

        Private _270_ISL_GUID As Guid = Guid.Empty
        Private _270_IRL_GUID As Guid = Guid.Empty
        Private _270_SL_GUID As Guid = Guid.Empty
        Private _270_DL_GUID As Guid = Guid.Empty
        Private _270_EQ_GUID As Guid = Guid.Empty

        Private _271_LS_GUID As Guid = Guid.Empty

        Private _2000A_DIRTY As Boolean = False
        Private _2000B_DIRTY As Boolean = False
        Private _2000C_DIRTY As Boolean = True
        Private _EQ_DIRTY As Boolean = False
        Private _ST_DIRTY As Boolean = True


        Private _270_LOOP_LEVEL As String = "HEADER"


        Private _270_BATCH_ID As Long = 0



        Private _EPICOutEDIString As String = String.Empty
        Private _isEPIC As Boolean = False

        Private mmm As Integer = 1



        Private _270_ISL_STRING As String = String.Empty
        Private _270_IRL_STRING As String = String.Empty
        Private _270_SL_STRING As String = String.Empty
        Private _270_DL_STRING As String = String.Empty
        Private _270_EQ_STRING As String = String.Empty



        Private _PEQ As String = String.Empty
        Private _FIRST_EQ_FOUND As Boolean = False
        Private _IN_EQ As Boolean = False




        Private _NM01 As String = String.Empty


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


        Public Sub New()
            If Debugger.IsAttached Then
                _VERBOSE = 1
                _DEBUG_LEVEL = 1

            End If

            _CONSOLE_NAME = "EDI_271_005010X279A1"
            _CLASS_NAME = "EDI_271_005010X279A1"

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
        Public WriteOnly Property isFile As Boolean

            Set(value As Boolean)
                _IS_FILE = value

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
        Public WriteOnly Property ConnectionString As String

            Set(value As String)
                _ConnectionString = value
                log.ConnectionString = value
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
        Public WriteOnly Property SP_COMIT_2000A_UNIT_TEST As String

            Set(value As String)
                _SP_COMIT_2000A_UNIT_TEST = value
            End Set
        End Property
        Public WriteOnly Property SP_COMIT_2000B_UNIT_TEST As String

            Set(value As String)
                _SP_COMIT_2000B_UNIT_TEST = value
            End Set
        End Property



        Public WriteOnly Property SP_COMIT_HL22_UNIT_TEST As String

            Set(value As String)
                _SP_COMIT_HL22_UNIT_TEST = value
            End Set
        End Property


        Public WriteOnly Property SP_COMIT_2000A_DATA As String

            Set(value As String)
                _SP_COMIT_2000A_DATA = value
            End Set
        End Property
        Public WriteOnly Property SP_COMIT_2000B_DATA As String

            Set(value As String)
                _SP_COMIT_2000B_DATA = value
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

        Public Function ImportByString(ByVal EDI As String, ByVal BatchID As Double) As Integer

            Dim r As Integer = -1


            _IS_STRING = True
            _RAW_EDI = EDI

            Try
                Using SP As New StringPrep()
                    _BATCH_ID = CLng(BatchID)
                    _EDIList = SP.SingleEDI(_RAW_EDI)
                End Using

                r = Import(_EDIList)

            Catch ex As Exception
                log.ExceptionDetails("", ex)
            End Try

            Return r

        End Function

        Public Function Import(ByVal EDIList As List(Of String)) As Int32

            Dim last As String = String.Empty
            'Dim _ROW_COUNT As Int32 = 0



            ' THIS HACK WAS ADDED SINCE THE _BATCH_ID GETS CHAGESD TO NUMBER RETRUNED FROM 22
            _270_BATCH_ID = _EBR_ID



            _ProcessStartTime = Now
            Dim _ImportReturnCode As Integer = 0


            If _TablesBuilt = False Then
                BuildTables()
                _TablesBuilt = True
                ClearISA()
            End If

            'so lets get down to it opn the file in a stream reader and move thru it one line at a time...........


            _HL20_DIRTY = True

            TestSPs()

            Using e As New EDI_5010_LOGGING
                e.ConnectionString = _ConnectionString
                e.TransactionSetIdentifierCode = "270"
                e.UpdateFileStatus(CInt(_FILE_ID), "270", "BEGIN PARSE")
            End Using



            Using s As New EDI_5010_BUILD_EDI_STRING
                If s.byList(EDIList) = 0 Then
                    _RAW_EDI_EX = s.RawEDI
                Else
                    _RAW_EDI_EX = "Build EDI Failed"
                End If
            End Using



            For Each line As String In EDIList
                _RowProcessedFlag = 0

                If _DataElementSeparatorFlag = 0 Then
                    _DataElementSeparator = Mid(line, 4, 1)
                    _DataElementSeparatorFlag = 1
                End If

                last = line

                _RAW_EDI = _RAW_EDI + line

                line = line.Replace("~", "")
                _ROW_COUNT = _ROW_COUNT + 1





                _RowRecordType = ss.ParseDemlimtedStringEDI(line, _DataElementSeparator, 1)
                'Console.WriteLine(_RowRecordType)
                _CurrentRowData = line


                If _RowRecordType = "SE" Then
                    _IN_EQ = False
                End If



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
                        ISARow("BATCH_ID") = _270_BATCH_ID
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



                        '_ISA_ROW_ID = _ROW_COUNT
                        ISARow("ROW_NUMBER") = _ROW_COUNT

                        ISA.Rows.Add(ISARow)



                        _RepetitionSeparator = _ISA11
                        _ComponentElementSeparator = _ISA16

                        _ISA = Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"


                        '  _chars = "RowDataDelimiter: " + _DataElementSeparator + " CarrotDataDelimiter: " + _CarrotDataDelimiter + " ComponentElementSeperator: " + _ISA16_ComponentElementSeparator



                        'o'D.ISAFlag = 1

                        ComitISA()


                        Select Case _270_LOOP_LEVEL
                            Case "HEADER"
                                _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                        End Select

                    End If

                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
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
                        GSRow("BATCH_ID") = _270_BATCH_ID
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

                        'o() 'D.GS_ROW_ID = _ROW_COUNT

                        GSRow("ROW_NUMBER") = _ROW_COUNT
                        GS.Rows.Add(GSRow)

                        _GS = Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"

                        ComitGS()


                        Select Case _270_LOOP_LEVEL
                            Case "HEADER"
                                _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                        End Select

                    End If

                    'end GS


                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
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
                        STRow("BATCH_ID") = _270_BATCH_ID
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

                        Select Case _270_LOOP_LEVEL
                            Case "HEADER"
                                _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                        End Select
                        '   ComitST()
                    End If

                    ' all the rows get made in to a string. 

                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
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
                        BHTRow("BATCH_ID") = _270_BATCH_ID
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


                        Select Case _270_LOOP_LEVEL
                            Case "HEADER"
                                _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                        End Select

                        _RowProcessedFlag = 1
                        ComitST()

                        'BHT.Clear()

                        'Dim BHTRowX As DataRow = BHT.NewRow
                        'BHTRowX("DOCUMENT_ID") = _DOCUMENT_ID
                        'BHTRowX("FILE_ID") = _FILE_ID
                        'BHTRowX("BATCH_ID") = _270_BATCH_ID
                        'BHTRowX("ISA_ID") = _ISA_ID
                        'BHTRowX("GS_ID") = _GS_ID
                        'BHTRowX("ST_ID") = _ST_ID
                        'BHTRowX("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                        'BHTRowX("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                        'BHTRowX("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                        'BHTRowX("BHT01") = _BHT01
                        'BHTRowX("BHT02") = _BHT02
                        'BHTRowX("BHT03") = _BHT03
                        'BHTRowX("BHT04") = _BHT04
                        'BHTRowX("BHT05") = _BHT05
                        'BHTRowX("BHT06") = _BHT06
                        'BHTRowX("ROW_NUMBER") = _ROW_COUNT

                        'BHT.Rows.Add(BHTRowX)


                    End If

                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                End Try


                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '  BEGIN 271
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''



                '**********************************************************************************************************************************************************
                'THIS GETS US TO THE hl LEVEL BLA BLA BLA'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
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





                                If _HL22_DIRTY Then
                                    '  ComitHL20()
                                    _HL22_DIRTY = False
                                End If


                                _270_ISL_STRING = String.Empty
                                _270_LOOP_LEVEL = "ISL"

                                _2000A_DIRTY = True

                            Case 21


                                If _2000A_DIRTY Then

                                    COMIT2000AHeaderDump()
                                    _2000A_DIRTY = False

                                End If

                                If _HL22_DIRTY Then
                                    '  ComitHL20()
                                    _HL22_DIRTY = False
                                End If

                                _270_IRL_STRING = String.Empty
                                _270_LOOP_LEVEL = "IRL"
                                _2000B_DIRTY = True

                            Case 22

                                If _2000A_DIRTY Then

                                    COMIT2000AHeaderDump()
                                    _2000A_DIRTY = False

                                End If

                                If _2000B_DIRTY Then

                                    COMIT2000BHeaderDump()
                                    _2000B_DIRTY = False

                                End If


                                If _HL22_DIRTY Then
                                    COMIT_270_22_Dump()
                                    '  ComitHL20()
                                    _HL22_DIRTY = False
                                End If
                                _LoopLevelSubFix = "C"
                                _HL22_DIRTY = True
                                _270_LOOP_LEVEL = "SL"
                                _270_SL_STRING = String.Empty

                            Case 23
                                _HL22_DIRTY = True
                                _270_LOOP_LEVEL = "DL"
                                _270_DL_STRING = String.Empty

                                _LoopLevelSubFix = "D"
                            Case 24

                            Case Else

                        End Select


                    End If



                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                End Try





                '**********************************************************************************************************************************************************


                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   BEGIN HL LEVELS
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''







                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   HL
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "HL" Then

                        _RowProcessedFlag = 1




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
                                Dim HLRow As DataRow = ISL_HL.NewRow


                                HLRow("FILE_ID") = _FILE_ID
                                HLRow("BATCH_ID") = _270_BATCH_ID
                                HLRow("ISA_ID") = _ISA_ID
                                HLRow("GS_ID") = _GS_ID
                                HLRow("ST_ID") = _ST_ID
                                HLRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                                HLRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                                HLRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID

                                _270_ISL_GUID = Guid.NewGuid
                                _270_IRL_GUID = Guid.Empty

                                _270_LOOP_LEVEL = "ISL"

                                _LoopLevelMajor = 2000
                                _LoopLevelMinor = 0
                                _LoopLevelSubFix = "A"
                                _2000A_DIRTY = True
                                _HIPAA_HL_20_GUID = Guid.NewGuid
                                _HIPAA_HL_21_GUID = Guid.Empty
                                _HIPAA_HL_22_GUID = Guid.Empty
                                _HIPAA_HL_23_GUID = Guid.Empty
                                _HIPAA_HL_24_GUID = Guid.Empty


                                _NM1_GUID = Guid.Empty

                                HLRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                                HLRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                                HLRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                                HLRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                                HLRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID


                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then HLRow("HL01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else HLRow("HL01") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then HLRow("HL02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else HLRow("HL02") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then HLRow("HL03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else HLRow("HL03") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then HLRow("HL04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else HLRow("HL04") = DBNull.Value






                                HLRow("ROW_NUMBER") = _ROW_COUNT



                                HLRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                HLRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                                HLRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor

                                ISL_HL.Rows.Add(HLRow)


                            Case 21
                                Dim HLRow As DataRow = IRL_HL.NewRow


                                HLRow("FILE_ID") = _FILE_ID
                                HLRow("BATCH_ID") = _270_BATCH_ID
                                HLRow("ISA_ID") = _ISA_ID
                                HLRow("GS_ID") = _GS_ID
                                HLRow("ST_ID") = _ST_ID
                                HLRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                                HLRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                                HLRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID


                                ' _270_ISL_GUID = Guid.Empty
                                _270_IRL_GUID = Guid.NewGuid

                                _270_LOOP_LEVEL = "IRL"

                                _NM1_GUID = Guid.Empty
                                _2000B_DIRTY = True
                                _HIPAA_HL_21_GUID = Guid.NewGuid
                                _HIPAA_HL_22_GUID = Guid.Empty
                                _HIPAA_HL_23_GUID = Guid.Empty
                                _HIPAA_HL_24_GUID = Guid.Empty




                                HLRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                                HLRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                                HLRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                                HLRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                                HLRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID


                                _LoopLevelMajor = 2000
                                _LoopLevelMinor = 0
                                _LoopLevelSubFix = "B"



                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then HLRow("HL01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else HLRow("HL01") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then HLRow("HL02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else HLRow("HL02") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then HLRow("HL03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else HLRow("HL03") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then HLRow("HL04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else HLRow("HL04") = DBNull.Value






                                HLRow("ROW_NUMBER") = _ROW_COUNT



                                HLRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                HLRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                                HLRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor

                                IRL_HL.Rows.Add(HLRow)



                            Case 22
                                Dim HLRow As DataRow = HL.NewRow

                                HLRow("FILE_ID") = _FILE_ID
                                HLRow("BATCH_ID") = _270_BATCH_ID
                                HLRow("ISA_ID") = _ISA_ID
                                HLRow("GS_ID") = _GS_ID
                                HLRow("ST_ID") = _ST_ID
                                HLRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                                HLRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                                HLRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID



                                _270_LOOP_LEVEL = "SL"

                                '  _270_ISL_GUID = Guid.Empty
                                '  _270_IRL_GUID = Guid.Empty
                                _270_SL_GUID = Guid.NewGuid
                                _270_DL_GUID = Guid.Empty
                                _NM1_GUID = Guid.Empty

                                _2000C_DIRTY = True
                                _HIPAA_HL_22_GUID = Guid.NewGuid
                                _HIPAA_HL_23_GUID = Guid.Empty
                                _HIPAA_HL_24_GUID = Guid.Empty


                                HLRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                                HLRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                                HLRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                                HLRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                                HLRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID

                                _LoopLevelMajor = 2000
                                _LoopLevelMinor = 0
                                _LoopLevelSubFix = "C"

                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then HLRow("HL01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else HLRow("HL01") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then HLRow("HL02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else HLRow("HL02") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then HLRow("HL03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else HLRow("HL03") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then HLRow("HL04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else HLRow("HL04") = DBNull.Value





                                HLRow("ROW_NUMBER") = _ROW_COUNT



                                HLRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                HLRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                                HLRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor

                                HL.Rows.Add(HLRow)


                            Case 23
                                Dim HLRow As DataRow = HL.NewRow

                                HLRow("FILE_ID") = _FILE_ID
                                HLRow("BATCH_ID") = _270_BATCH_ID
                                HLRow("ISA_ID") = _ISA_ID
                                HLRow("GS_ID") = _GS_ID
                                HLRow("ST_ID") = _ST_ID
                                HLRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                                HLRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                                HLRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID

                                _NM1_GUID = Guid.Empty

                                _270_LOOP_LEVEL = "DL"

                                _2000C_DIRTY = True
                                _HIPAA_HL_23_GUID = Guid.NewGuid
                                _270_DL_GUID = Guid.NewGuid




                                HLRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                                HLRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                                HLRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                                HLRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                                HLRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID


                                _LoopLevelMajor = 2000
                                _LoopLevelMinor = 0
                                _LoopLevelSubFix = "D"


                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then HLRow("HL01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else HLRow("HL01") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then HLRow("HL02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else HLRow("HL02") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then HLRow("HL03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else HLRow("HL03") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then HLRow("HL04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else HLRow("HL04") = DBNull.Value






                                HLRow("ROW_NUMBER") = _ROW_COUNT



                                HLRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                HLRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                                HLRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor

                                HL.Rows.Add(HLRow)



                        End Select



                        Select Case _270_LOOP_LEVEL
                            Case "HEADER"
                                _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case "ISL"
                                _270_ISL_STRING = _270_ISL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case "IRL"
                                _270_IRL_STRING = _270_IRL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case "SL"
                                _270_SL_STRING = _270_SL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case "DL"
                                _270_DL_STRING = _270_DL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                        End Select


                    End If

                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                End Try





                '==========================================================================================================================================================
                ' BEGIN ISL BLOCK
                If _270_LOOP_LEVEL = "ISL" Then





                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '  ISL ::  NM1
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "NM1" Then

                            _RowProcessedFlag = 1

                            _NM1_GUID = Guid.NewGuid

                            Dim NM1Row As DataRow = ISL_NM1.NewRow
                            NM1Row("DOCUMENT_ID") = _DOCUMENT_ID
                            NM1Row("FILE_ID") = _FILE_ID
                            NM1Row("BATCH_ID") = _270_BATCH_ID
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
                            NM1Row("270_ISL_GUID") = _270_ISL_GUID
                            NM1Row("270_IRL_GUID") = _270_IRL_GUID
                            NM1Row("270_SL_GUID") = _270_SL_GUID
                            NM1Row("270_DL_GUID") = _270_DL_GUID
                            NM1Row("270_EQ_GUID") = _270_EQ_GUID
                            NM1Row("NM1_GUID") = _NM1_GUID
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
                            NM1Row("EQ_ROW_NUMBER") = 0


                            NM1Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            NM1Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                            NM1Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor



                            ISL_NM1.Rows.Add(NM1Row)

                            Select Case _270_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "ISL"
                                    _270_ISL_STRING = _270_ISL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "IRL"
                                    _270_IRL_STRING = _270_IRL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "SL"
                                    _270_SL_STRING = _270_SL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "DL"
                                    _270_DL_STRING = _270_DL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select



                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                    End Try






                End If
                'END ISL BLOCK
                '==========================================================================================================================================================





                '==========================================================================================================================================================
                ' BEGIN IRL BLOCK
                If _270_LOOP_LEVEL = "IRL" Then




                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '  IRL ::  NM1
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "NM1" Then

                            _RowProcessedFlag = 1

                            _NM1_GUID = Guid.NewGuid

                            Dim NM1Row As DataRow = IRL_NM1.NewRow
                            NM1Row("DOCUMENT_ID") = _DOCUMENT_ID
                            NM1Row("FILE_ID") = _FILE_ID
                            NM1Row("BATCH_ID") = _270_BATCH_ID
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
                            NM1Row("270_ISL_GUID") = _270_ISL_GUID
                            NM1Row("270_IRL_GUID") = _270_IRL_GUID
                            NM1Row("270_SL_GUID") = _270_SL_GUID
                            NM1Row("270_DL_GUID") = _270_DL_GUID
                            NM1Row("270_EQ_GUID") = _270_EQ_GUID
                            NM1Row("NM1_GUID") = _NM1_GUID

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
                            NM1Row("EQ_ROW_NUMBER") = 0



                            NM1Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            NM1Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                            NM1Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor



                            IRL_NM1.Rows.Add(NM1Row)

                            Select Case _270_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "ISL"
                                    _270_ISL_STRING = _270_ISL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "IRL"
                                    _270_IRL_STRING = _270_IRL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "SL"
                                    _270_SL_STRING = _270_SL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "DL"
                                    _270_DL_STRING = _270_DL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select


                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                    End Try






                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   REF
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "REF" Then
                            _RowProcessedFlag = 1

                            Dim REFRow As DataRow = IRL_REF.NewRow
                            REFRow("DOCUMENT_ID") = _DOCUMENT_ID
                            REFRow("FILE_ID") = _FILE_ID
                            REFRow("BATCH_ID") = _270_BATCH_ID
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
                            REFRow("270_ISL_GUID") = _270_ISL_GUID
                            REFRow("270_IRL_GUID") = _270_IRL_GUID
                            REFRow("270_SL_GUID") = _270_SL_GUID
                            REFRow("270_DL_GUID") = _270_DL_GUID
                            REFRow("270_EQ_GUID") = _270_EQ_GUID
                            REFRow("NM1_GUID") = _NM1_GUID
                            REFRow("HL01") = _HL01
                            REFRow("HL02") = _HL02
                            REFRow("HL03") = _HL03
                            REFRow("HL04") = _HL04
                            ' REFRow("P_GUID") = _P_GUID


                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then REFRow("REF01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else REFRow("REF01") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then REFRow("REF02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else REFRow("REF02") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then REFRow("REF03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else REFRow("REF03") = DBNull.Value

                            REFRow("ROW_NUMBER") = _ROW_COUNT
                            REFRow("EQ_ROW_NUMBER") = 0

                            REFRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            REFRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            REFRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                            IRL_REF.Rows.Add(REFRow)

                            Select Case _270_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "ISL"
                                    _270_ISL_STRING = _270_ISL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "IRL"
                                    _270_IRL_STRING = _270_IRL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "SL"
                                    _270_SL_STRING = _270_SL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "DL"
                                    _270_DL_STRING = _270_DL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select

                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                    End Try






                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '  IRL ::  N3
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "N3" Then


                            _RowProcessedFlag = 1
                            Dim N3Row As DataRow = IRL_N3.NewRow
                            N3Row("DOCUMENT_ID") = _DOCUMENT_ID
                            N3Row("FILE_ID") = _FILE_ID
                            N3Row("BATCH_ID") = _270_BATCH_ID
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
                            N3Row("270_ISL_GUID") = _270_ISL_GUID
                            N3Row("270_IRL_GUID") = _270_IRL_GUID
                            N3Row("270_SL_GUID") = _270_SL_GUID
                            N3Row("270_DL_GUID") = _270_DL_GUID
                            N3Row("270_EQ_GUID") = _270_EQ_GUID
                            N3Row("NM1_GUID") = _NM1_GUID
                            N3Row("HL01") = _HL01
                            N3Row("HL02") = _HL02
                            N3Row("HL03") = _HL03
                            N3Row("HL04") = _HL04


                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then N3Row("N301") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else N3Row("N301") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then N3Row("N302") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else N3Row("N302") = DBNull.Value




                            N3Row("ROW_NUMBER") = _ROW_COUNT
                            N3Row("EQ_ROW_NUMBER") = 0


                            N3Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            N3Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            N3Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                            IRL_N3.Rows.Add(N3Row)

                            Select Case _270_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "ISL"
                                    _270_ISL_STRING = _270_ISL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "IRL"
                                    _270_IRL_STRING = _270_IRL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "SL"
                                    _270_SL_STRING = _270_SL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "DL"
                                    _270_DL_STRING = _270_DL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select

                            _RowProcessedFlag = 1


                        End If
                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                    End Try





                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   IRL :: N4
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "N4" Then


                            Dim N4Row As DataRow = IRL_N4.NewRow
                            N4Row("DOCUMENT_ID") = _DOCUMENT_ID
                            N4Row("FILE_ID") = _FILE_ID
                            N4Row("BATCH_ID") = _270_BATCH_ID
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
                            N4Row("270_ISL_GUID") = _270_ISL_GUID
                            N4Row("270_IRL_GUID") = _270_IRL_GUID
                            N4Row("270_SL_GUID") = _270_SL_GUID
                            N4Row("270_DL_GUID") = _270_DL_GUID
                            N4Row("270_EQ_GUID") = _270_EQ_GUID
                            N4Row("NM1_GUID") = _NM1_GUID
                            N4Row("HL01") = _HL01
                            N4Row("HL02") = _HL02
                            N4Row("HL03") = _HL03
                            N4Row("HL04") = _HL04



                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then N4Row("N401") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else N4Row("N401") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then N4Row("N402") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else N4Row("N402") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then N4Row("N403") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else N4Row("N403") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then N4Row("N404") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else N4Row("N404") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then N4Row("N405") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else N4Row("N405") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then N4Row("N406") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) Else N4Row("N406") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then N4Row("N407") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) Else N4Row("N407") = DBNull.Value



                            N4Row("ROW_NUMBER") = _ROW_COUNT
                            N4Row("EQ_ROW_NUMBER") = 0

                            N4Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            N4Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            N4Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                            IRL_N4.Rows.Add(N4Row)

                            _RowProcessedFlag = 1

                            Select Case _270_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "ISL"
                                    _270_ISL_STRING = _270_ISL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "IRL"
                                    _270_IRL_STRING = _270_IRL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "SL"
                                    _270_SL_STRING = _270_SL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "DL"
                                    _270_DL_STRING = _270_DL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select



                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                    End Try



                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   PRV
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    Try
                        If _RowRecordType = "PRV" Then
                            _RowProcessedFlag = 1

                            Dim PRVRow As DataRow = IRL_PRV.NewRow
                            PRVRow("FILE_ID") = _FILE_ID
                            PRVRow("BATCH_ID") = _270_BATCH_ID
                            PRVRow("ISA_ID") = _ISA_ID
                            PRVRow("GS_ID") = _GS_ID
                            PRVRow("ST_ID") = _ST_ID
                            PRVRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            PRVRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            PRVRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            PRVRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            PRVRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            PRVRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            PRVRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            PRVRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            PRVRow("270_ISL_GUID") = _270_ISL_GUID
                            PRVRow("270_IRL_GUID") = _270_IRL_GUID
                            PRVRow("270_SL_GUID") = _270_SL_GUID
                            PRVRow("270_DL_GUID") = _270_DL_GUID
                            PRVRow("270_EQ_GUID") = _270_EQ_GUID
                            PRVRow("271_LS_GUID") = _271_LS_GUID
                            PRVRow("HL01") = _HL01
                            PRVRow("HL02") = _HL02
                            PRVRow("HL03") = _HL03
                            PRVRow("HL04") = _HL04

                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then PRVRow("PRV01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else PRVRow("PRV01") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then PRVRow("PRV02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else PRVRow("PRV02") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then PRVRow("PRV03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else PRVRow("PRV03") = DBNull.Value

                            PRVRow("ROW_NUMBER") = _ROW_COUNT
                            PRVRow("EQ_ROW_NUMBER") = 0

                            PRVRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            PRVRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            PRVRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                            IRL_PRV.Rows.Add(PRVRow)


                            Select Case _270_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "ISL"
                                    _270_ISL_STRING = _270_ISL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "IRL"
                                    _270_IRL_STRING = _270_IRL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "SL"
                                    _270_SL_STRING = _270_SL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "DL"
                                    _270_DL_STRING = _270_DL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select

                        End If
                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                    End Try


                End If
                'END IRL BLOCK
                '==========================================================================================================================================================







                '==========================================================================================================================================================
                ' BEGIN SL/DL BLOCK



                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '  SL/DL ::  TRN
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                If _270_LOOP_LEVEL = "SL" Or _270_LOOP_LEVEL = "DL" Then


                    If _RowRecordType = "EQ" Then
                        _EQ_DIRTY = True
                        _LAST_EQ = _CURRENT_EQ

                        _CURRENT_EQ = _CurrentRowData


                        'If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then
                        '    _PEQ = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4)

                        'End If





                    End If


                    If _RowRecordType = "EQ" Then

                        _IN_EQ = True

                        If _FIRST_EQ_FOUND = False Then
                            _FIRST_EQ_FOUND = True
                        Else
                            ProcessEQ(_EQIList, _LAST_EQ)
                            _EQIList.Clear()
                        End If

                    End If


                    If _IN_EQ Then
                        _RowProcessedFlag = 1
                        _EQIList.Add(Convert.ToString(_ROW_COUNT) + "`" + _CurrentRowData)
                    Else


                        Try
                            If _RowRecordType = "TRN" Then



                                Dim TRNRow As DataRow = TRN.NewRow
                                TRNRow("DOCUMENT_ID") = _DOCUMENT_ID
                                TRNRow("FILE_ID") = _FILE_ID
                                TRNRow("BATCH_ID") = _270_BATCH_ID
                                TRNRow("ISA_ID") = _ISA_ID
                                TRNRow("GS_ID") = _GS_ID
                                TRNRow("ST_ID") = _ST_ID
                                TRNRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                                TRNRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                                TRNRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                                TRNRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                                TRNRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                                TRNRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                                TRNRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                                TRNRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                                TRNRow("270_ISL_GUID") = _270_ISL_GUID
                                TRNRow("270_IRL_GUID") = _270_IRL_GUID
                                TRNRow("270_SL_GUID") = _270_SL_GUID
                                TRNRow("270_DL_GUID") = _270_DL_GUID
                                TRNRow("270_EQ_GUID") = _270_EQ_GUID
                                TRNRow("NM1_GUID") = _NM1_GUID
                                TRNRow("HL01") = _HL01
                                TRNRow("HL02") = _HL02
                                TRNRow("HL03") = _HL03
                                TRNRow("HL04") = _HL04


                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then TRNRow("TRN01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else TRNRow("TRN01") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then TRNRow("TRN02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else TRNRow("TRN02") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then TRNRow("TRN03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else TRNRow("TRN03") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then TRNRow("TRN04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else TRNRow("TRN04") = DBNull.Value



                                TRNRow("ROW_NUMBER") = _ROW_COUNT
                                TRNRow("EQ_ROW_NUMBER") = 0

                                TRNRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                TRNRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                                TRNRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                                Select Case _270_LOOP_LEVEL
                                    Case "HEADER"
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "ISL"
                                        _270_ISL_STRING = _270_ISL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "IRL"
                                        _270_IRL_STRING = _270_IRL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "SL"
                                        _270_SL_STRING = _270_SL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "DL"
                                        _270_DL_STRING = _270_DL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                End Select


                                TRN.Rows.Add(TRNRow)

                                _RowProcessedFlag = 1



                            End If
                        Catch ex As Exception
                            _RowProcessedFlag = -2
                            _hasERR = True
                            log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                        End Try


                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        '  SL/DL ::  NM1
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        Try
                            If _RowRecordType = "NM1" Then

                                _RowProcessedFlag = 1

                                _NM1_GUID = Guid.NewGuid

                                Dim NM1Row As DataRow = NM1.NewRow
                                NM1Row("DOCUMENT_ID") = _DOCUMENT_ID
                                NM1Row("FILE_ID") = _FILE_ID
                                NM1Row("BATCH_ID") = _270_BATCH_ID
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
                                NM1Row("270_ISL_GUID") = _270_ISL_GUID
                                NM1Row("270_IRL_GUID") = _270_IRL_GUID
                                NM1Row("270_SL_GUID") = _270_SL_GUID
                                NM1Row("270_DL_GUID") = _270_DL_GUID
                                NM1Row("270_EQ_GUID") = _270_EQ_GUID
                                NM1Row("NM1_GUID") = _NM1_GUID
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
                                NM1Row("EQ_ROW_NUMBER") = 0



                                NM1Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                NM1Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                                NM1Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor



                                NM1.Rows.Add(NM1Row)

                                Select Case _270_LOOP_LEVEL
                                    Case "HEADER"
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "ISL"
                                        _270_ISL_STRING = _270_ISL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "IRL"
                                        _270_IRL_STRING = _270_IRL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "SL"
                                        _270_SL_STRING = _270_SL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "DL"
                                        _270_DL_STRING = _270_DL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                End Select


                            End If

                        Catch ex As Exception
                            _RowProcessedFlag = -2
                            _hasERR = True
                            log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                        End Try






                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        '   SL/DL REF
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        Try
                            If _RowRecordType = "REF" Then
                                _RowProcessedFlag = 1

                                Dim REFRow As DataRow = REF.NewRow
                                REFRow("DOCUMENT_ID") = _DOCUMENT_ID
                                REFRow("FILE_ID") = _FILE_ID
                                REFRow("BATCH_ID") = _270_BATCH_ID
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
                                REFRow("270_ISL_GUID") = _270_ISL_GUID
                                REFRow("270_IRL_GUID") = _270_IRL_GUID
                                REFRow("270_SL_GUID") = _270_SL_GUID
                                REFRow("270_DL_GUID") = _270_DL_GUID
                                REFRow("270_EQ_GUID") = _270_EQ_GUID
                                REFRow("NM1_GUID") = _NM1_GUID
                                REFRow("HL01") = _HL01
                                REFRow("HL02") = _HL02
                                REFRow("HL03") = _HL03
                                REFRow("HL04") = _HL04

                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then REFRow("REF01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else REFRow("REF01") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then REFRow("REF02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else REFRow("REF02") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then REFRow("REF03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else REFRow("REF03") = DBNull.Value

                                REFRow("ROW_NUMBER") = _ROW_COUNT
                                REFRow("EQ_ROW_NUMBER") = 0

                                REFRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                REFRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                                REFRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                                REF.Rows.Add(REFRow)

                                Select Case _270_LOOP_LEVEL
                                    Case "HEADER"
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "ISL"
                                        _270_ISL_STRING = _270_ISL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "IRL"
                                        _270_IRL_STRING = _270_IRL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "SL"
                                        _270_SL_STRING = _270_SL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "DL"
                                        _270_DL_STRING = _270_DL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                End Select

                            End If

                        Catch ex As Exception
                            _RowProcessedFlag = -2
                            _hasERR = True
                            log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                        End Try






                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        '  SL\\/DL ::  N3
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        Try
                            If _RowRecordType = "N3" Then


                                _RowProcessedFlag = 1
                                Dim N3Row As DataRow = N3.NewRow
                                N3Row("DOCUMENT_ID") = _DOCUMENT_ID
                                N3Row("FILE_ID") = _FILE_ID
                                N3Row("BATCH_ID") = _270_BATCH_ID
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
                                N3Row("270_ISL_GUID") = _270_ISL_GUID
                                N3Row("270_IRL_GUID") = _270_IRL_GUID
                                N3Row("270_SL_GUID") = _270_SL_GUID
                                N3Row("270_DL_GUID") = _270_DL_GUID
                                N3Row("270_EQ_GUID") = _270_EQ_GUID
                                N3Row("NM1_GUID") = _NM1_GUID
                                N3Row("HL01") = _HL01
                                N3Row("HL02") = _HL02
                                N3Row("HL03") = _HL03
                                N3Row("HL04") = _HL04


                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then N3Row("N301") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else N3Row("N301") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then N3Row("N302") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else N3Row("N302") = DBNull.Value




                                N3Row("ROW_NUMBER") = _ROW_COUNT
                                N3Row("EQ_ROW_NUMBER") = 0


                                N3Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                N3Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                                N3Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                                N3.Rows.Add(N3Row)

                                Select Case _270_LOOP_LEVEL
                                    Case "HEADER"
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "ISL"
                                        _270_ISL_STRING = _270_ISL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "IRL"
                                        _270_IRL_STRING = _270_IRL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "SL"
                                        _270_SL_STRING = _270_SL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "DL"
                                        _270_DL_STRING = _270_DL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                End Select

                                _RowProcessedFlag = 1


                            End If
                        Catch ex As Exception
                            _RowProcessedFlag = -2
                            _hasERR = True
                            log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                        End Try





                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        '   SL/DL :: N4
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        Try
                            If _RowRecordType = "N4" Then


                                Dim N4Row As DataRow = N4.NewRow
                                N4Row("DOCUMENT_ID") = _DOCUMENT_ID
                                N4Row("FILE_ID") = _FILE_ID
                                N4Row("BATCH_ID") = _270_BATCH_ID
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
                                N4Row("270_ISL_GUID") = _270_ISL_GUID
                                N4Row("270_IRL_GUID") = _270_IRL_GUID
                                N4Row("270_SL_GUID") = _270_SL_GUID
                                N4Row("270_DL_GUID") = _270_DL_GUID
                                N4Row("270_EQ_GUID") = _270_EQ_GUID
                                N4Row("NM1_GUID") = _NM1_GUID
                                N4Row("HL01") = _HL01
                                N4Row("HL02") = _HL02
                                N4Row("HL03") = _HL03
                                N4Row("HL04") = _HL04



                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then N4Row("N401") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else N4Row("N401") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then N4Row("N402") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else N4Row("N402") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then N4Row("N403") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else N4Row("N403") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then N4Row("N404") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else N4Row("N404") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then N4Row("N405") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else N4Row("N405") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then N4Row("N406") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) Else N4Row("N406") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then N4Row("N407") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) Else N4Row("N407") = DBNull.Value



                                N4Row("ROW_NUMBER") = _ROW_COUNT
                                N4Row("EQ_ROW_NUMBER") = 0

                                N4Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                N4Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                                N4Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                                N4.Rows.Add(N4Row)

                                _RowProcessedFlag = 1

                                Select Case _270_LOOP_LEVEL
                                    Case "HEADER"
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "ISL"
                                        _270_ISL_STRING = _270_ISL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "IRL"
                                        _270_IRL_STRING = _270_IRL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "SL"
                                        _270_SL_STRING = _270_ISL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "DL"
                                        _270_DL_STRING = _270_IRL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                End Select



                            End If

                        Catch ex As Exception
                            _RowProcessedFlag = -2
                            _hasERR = True
                            log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                        End Try




                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        '  SL/DL :: PRV
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                        Try
                            If _RowRecordType = "PRV" Then
                                _RowProcessedFlag = 1

                                Dim PRVRow As DataRow = PRV.NewRow
                                PRVRow("FILE_ID") = _FILE_ID
                                PRVRow("BATCH_ID") = _270_BATCH_ID
                                PRVRow("FILE_ID") = _FILE_ID
                                PRVRow("ISA_ID") = _ISA_ID
                                PRVRow("GS_ID") = _GS_ID
                                PRVRow("ST_ID") = _ST_ID
                                PRVRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                                PRVRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                                PRVRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                                PRVRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                                PRVRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                                PRVRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                                PRVRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                                PRVRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                                PRVRow("270_ISL_GUID") = _270_ISL_GUID
                                PRVRow("270_IRL_GUID") = _270_IRL_GUID
                                PRVRow("270_SL_GUID") = _270_SL_GUID
                                PRVRow("270_DL_GUID") = _270_DL_GUID
                                PRVRow("270_EQ_GUID") = _270_EQ_GUID
                                PRVRow("NM1_GUID") = _NM1_GUID
                                PRVRow("HL01") = _HL01
                                PRVRow("HL02") = _HL02
                                PRVRow("HL03") = _HL03
                                PRVRow("HL04") = _HL04

                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then PRVRow("PRV01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else PRVRow("PRV01") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then PRVRow("PRV02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else PRVRow("PRV02") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then PRVRow("PRV03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else PRVRow("PRV03") = DBNull.Value

                                PRVRow("ROW_NUMBER") = _ROW_COUNT
                                PRVRow("EQ_ROW_NUMBER") = 0

                                PRVRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                PRVRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                                PRVRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                                PRV.Rows.Add(PRVRow)


                                Select Case _270_LOOP_LEVEL
                                    Case "HEADER"
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "ISL"
                                        _270_ISL_STRING = _270_ISL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "IRL"
                                        _270_IRL_STRING = _270_IRL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "SL"
                                        _270_SL_STRING = _270_SL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "DL"
                                        _270_DL_STRING = _270_DL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                End Select

                            End If
                        Catch ex As Exception
                            _RowProcessedFlag = -2
                            _hasERR = True
                            log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                        End Try





                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        ' SL/DL ::  DMG
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        Try
                            If _RowRecordType = "DMG" Then

                                _RowProcessedFlag = 1

                                Dim DMGRow As DataRow = DMG.NewRow
                                DMGRow("DOCUMENT_ID") = _DOCUMENT_ID
                                DMGRow("FILE_ID") = _FILE_ID
                                DMGRow("BATCH_ID") = _270_BATCH_ID
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
                                DMGRow("270_ISL_GUID") = _270_ISL_GUID
                                DMGRow("270_IRL_GUID") = _270_IRL_GUID
                                DMGRow("270_SL_GUID") = _270_SL_GUID
                                DMGRow("270_DL_GUID") = _270_DL_GUID
                                DMGRow("270_EQ_GUID") = _270_EQ_GUID
                                DMGRow("NM1_GUID") = _NM1_GUID
                                DMGRow("HL01") = _HL01
                                DMGRow("HL02") = _HL02
                                DMGRow("HL03") = _HL03
                                DMGRow("HL04") = _HL04

                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then DMGRow("DMG01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else DMGRow("DMG01") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then DMGRow("DMG02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else DMGRow("DMG02") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then DMGRow("DMG03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else DMGRow("DMG03") = DBNull.Value

                                DMGRow("ROW_NUMBER") = _ROW_COUNT
                                DMGRow("EQ_ROW_NUMBER") = 0


                                DMGRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                DMGRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                                DMGRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor


                                DMG.Rows.Add(DMGRow)


                                Select Case _270_LOOP_LEVEL
                                    Case "HEADER"
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "ISL"
                                        _270_ISL_STRING = _270_ISL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "IRL"
                                        _270_IRL_STRING = _270_IRL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "SL"
                                        _270_SL_STRING = _270_SL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "DL"
                                        _270_DL_STRING = _270_DL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                End Select

                            End If
                        Catch ex As Exception
                            _RowProcessedFlag = -2
                            _hasERR = True
                            log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                        End Try



                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        '  SL/DL :: INS
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                        Try
                            If _RowRecordType = "INS" Then
                                _RowProcessedFlag = 1

                                Dim INSRow As DataRow = INS.NewRow
                                INSRow("FILE_ID") = _FILE_ID
                                INSRow("BATCH_ID") = _270_BATCH_ID
                                INSRow("ISA_ID") = _ISA_ID
                                INSRow("GS_ID") = _GS_ID
                                INSRow("ST_ID") = _ST_ID
                                INSRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                                INSRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                                INSRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                                INSRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                                INSRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                                INSRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                                INSRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                                INSRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                                INSRow("270_ISL_GUID") = _270_ISL_GUID
                                INSRow("270_IRL_GUID") = _270_IRL_GUID
                                INSRow("270_SL_GUID") = _270_SL_GUID
                                INSRow("270_DL_GUID") = _270_DL_GUID
                                INSRow("270_EQ_GUID") = _270_EQ_GUID
                                INSRow("NM1_GUID") = _NM1_GUID
                                INSRow("HL01") = _HL01
                                INSRow("HL02") = _HL02
                                INSRow("HL03") = _HL03
                                INSRow("HL04") = _HL04


                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then INSRow("INS01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else INSRow("INS01") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then INSRow("INS02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else INSRow("INS02") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then INSRow("INS03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else INSRow("INS03") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then INSRow("INS04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else INSRow("INS04") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then INSRow("INS05") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else INSRow("INS05") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then INSRow("INS06") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) Else INSRow("INS06") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then INSRow("INS07") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) Else INSRow("INS07") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) <> "") Then INSRow("INS08") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) Else INSRow("INS08") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) <> "") Then INSRow("INS09") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) Else INSRow("INS09") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11) <> "") Then INSRow("INS10") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11) Else INSRow("INS10") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12) <> "") Then INSRow("INS11") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12) Else INSRow("INS11") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 13) <> "") Then INSRow("INS12") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 13) Else INSRow("INS12") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 14) <> "") Then INSRow("INS13") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 14) Else INSRow("INS13") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 15) <> "") Then INSRow("INS14") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 15) Else INSRow("INS14") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 16) <> "") Then INSRow("INS15") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 16) Else INSRow("INS15") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 17) <> "") Then INSRow("INS16") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 17) Else INSRow("INS16") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 18) <> "") Then INSRow("INS17") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 18) Else INSRow("INS17") = DBNull.Value




                                INSRow("ROW_NUMBER") = _ROW_COUNT
                                INSRow("EQ_ROW_NUMBER") = 0

                                INSRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                INSRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                                INSRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                                INS.Rows.Add(INSRow)


                                Select Case _270_LOOP_LEVEL
                                    Case "HEADER"
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "ISL"
                                        _270_ISL_STRING = _270_ISL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "IRL"
                                        _270_IRL_STRING = _270_IRL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "SL"
                                        _270_SL_STRING = _270_SL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "DL"
                                        _270_DL_STRING = _270_DL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                End Select

                            End If
                        Catch ex As Exception
                            _RowProcessedFlag = -2
                            _hasERR = True
                            log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                        End Try









                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        '   HI
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        Try
                            If _RowRecordType = "HI" Then

                                _RowProcessedFlag = 1

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






                                Dim HIRow As DataRow = HI.NewRow

                                HIRow("DOCUMENT_ID") = _DOCUMENT_ID
                                HIRow("FILE_ID") = _FILE_ID
                                HIRow("BATCH_ID") = _270_BATCH_ID
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
                                HIRow("270_ISL_GUID") = _270_ISL_GUID
                                HIRow("270_IRL_GUID") = _270_IRL_GUID
                                HIRow("270_SL_GUID") = _270_SL_GUID
                                HIRow("270_DL_GUID") = _270_DL_GUID
                                HIRow("270_EQ_GUID") = _270_EQ_GUID
                                HIRow("NM1_GUID") = _NM1_GUID
                                HIRow("HL01") = _HL01
                                HIRow("HL02") = _HL02
                                HIRow("HL03") = _HL03
                                HIRow("HL04") = _HL04

                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then
                                    HIRow("HI01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2)
                                    HI01 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2)
                                Else
                                    HIRow("HI01") = DBNull.Value

                                End If


                                If Not HI01 = String.Empty Then

                                    If (ss.ParseDemlimtedString(HI01, _ComponentElementSeparator, 1) <> "") Then HIRow("HI01_1") = ss.ParseDemlimtedString(HI01, _ComponentElementSeparator, 1) Else HIRow("HI01_1") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI01, _ComponentElementSeparator, 2) <> "") Then HIRow("HI01_2") = ss.ParseDemlimtedString(HI01, _ComponentElementSeparator, 2) Else HIRow("HI01_2") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI01, _ComponentElementSeparator, 3) <> "") Then HIRow("HI01_3") = ss.ParseDemlimtedString(HI01, _ComponentElementSeparator, 3) Else HIRow("HI01_3") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI01, _ComponentElementSeparator, 4) <> "") Then HIRow("HI01_4") = ss.ParseDemlimtedString(HI01, _ComponentElementSeparator, 4) Else HIRow("HI01_4") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI01, _ComponentElementSeparator, 5) <> "") Then HIRow("HI01_5") = ss.ParseDemlimtedString(HI01, _ComponentElementSeparator, 5) Else HIRow("HI01_5") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI01, _ComponentElementSeparator, 6) <> "") Then HIRow("HI01_6") = ss.ParseDemlimtedString(HI01, _ComponentElementSeparator, 6) Else HIRow("HI01_6") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI01, _ComponentElementSeparator, 7) <> "") Then HIRow("HI01_7") = ss.ParseDemlimtedString(HI01, _ComponentElementSeparator, 7) Else HIRow("HI01_7") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI01, _ComponentElementSeparator, 8) <> "") Then HIRow("HI01_8") = ss.ParseDemlimtedString(HI01, _ComponentElementSeparator, 8) Else HIRow("HI01_8") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI01, _ComponentElementSeparator, 9) <> "") Then HIRow("HI01_9") = ss.ParseDemlimtedString(HI01, _ComponentElementSeparator, 9) Else HIRow("HI01_9") = DBNull.Value

                                End If


                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then
                                    HIRow("HI02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3)
                                    HI02 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3)
                                Else
                                    HIRow("HI02") = DBNull.Value

                                End If


                                If Not HI02 = String.Empty Then

                                    If (ss.ParseDemlimtedString(HI02, _ComponentElementSeparator, 1) <> "") Then HIRow("HI02_1") = ss.ParseDemlimtedString(HI02, _ComponentElementSeparator, 1) Else HIRow("HI02_1") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI02, _ComponentElementSeparator, 2) <> "") Then HIRow("HI02_2") = ss.ParseDemlimtedString(HI02, _ComponentElementSeparator, 2) Else HIRow("HI02_2") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI02, _ComponentElementSeparator, 3) <> "") Then HIRow("HI02_3") = ss.ParseDemlimtedString(HI02, _ComponentElementSeparator, 3) Else HIRow("HI02_3") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI02, _ComponentElementSeparator, 4) <> "") Then HIRow("HI02_4") = ss.ParseDemlimtedString(HI02, _ComponentElementSeparator, 4) Else HIRow("HI02_4") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI02, _ComponentElementSeparator, 5) <> "") Then HIRow("HI02_5") = ss.ParseDemlimtedString(HI02, _ComponentElementSeparator, 5) Else HIRow("HI02_5") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI02, _ComponentElementSeparator, 6) <> "") Then HIRow("HI02_6") = ss.ParseDemlimtedString(HI02, _ComponentElementSeparator, 6) Else HIRow("HI02_6") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI02, _ComponentElementSeparator, 7) <> "") Then HIRow("HI02_7") = ss.ParseDemlimtedString(HI02, _ComponentElementSeparator, 7) Else HIRow("HI02_7") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI02, _ComponentElementSeparator, 8) <> "") Then HIRow("HI02_8") = ss.ParseDemlimtedString(HI02, _ComponentElementSeparator, 8) Else HIRow("HI02_8") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI02, _ComponentElementSeparator, 9) <> "") Then HIRow("HI02_9") = ss.ParseDemlimtedString(HI02, _ComponentElementSeparator, 9) Else HIRow("HI02_9") = DBNull.Value

                                End If



                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then
                                    HIRow("HI03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4)
                                    HI03 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4)
                                Else
                                    HIRow("HI03") = DBNull.Value

                                End If


                                If Not HI03 = String.Empty Then

                                    If (ss.ParseDemlimtedString(HI03, _ComponentElementSeparator, 1) <> "") Then HIRow("HI03_1") = ss.ParseDemlimtedString(HI03, _ComponentElementSeparator, 1) Else HIRow("HI03_1") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI03, _ComponentElementSeparator, 2) <> "") Then HIRow("HI03_2") = ss.ParseDemlimtedString(HI03, _ComponentElementSeparator, 2) Else HIRow("HI03_2") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI03, _ComponentElementSeparator, 3) <> "") Then HIRow("HI03_3") = ss.ParseDemlimtedString(HI03, _ComponentElementSeparator, 3) Else HIRow("HI03_3") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI03, _ComponentElementSeparator, 4) <> "") Then HIRow("HI03_4") = ss.ParseDemlimtedString(HI03, _ComponentElementSeparator, 4) Else HIRow("HI03_4") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI03, _ComponentElementSeparator, 5) <> "") Then HIRow("HI03_5") = ss.ParseDemlimtedString(HI03, _ComponentElementSeparator, 5) Else HIRow("HI03_5") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI03, _ComponentElementSeparator, 6) <> "") Then HIRow("HI03_6") = ss.ParseDemlimtedString(HI03, _ComponentElementSeparator, 6) Else HIRow("HI03_6") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI03, _ComponentElementSeparator, 7) <> "") Then HIRow("HI03_7") = ss.ParseDemlimtedString(HI03, _ComponentElementSeparator, 7) Else HIRow("HI03_7") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI03, _ComponentElementSeparator, 8) <> "") Then HIRow("HI03_8") = ss.ParseDemlimtedString(HI03, _ComponentElementSeparator, 8) Else HIRow("HI03_8") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI03, _ComponentElementSeparator, 9) <> "") Then HIRow("HI03_9") = ss.ParseDemlimtedString(HI03, _ComponentElementSeparator, 9) Else HIRow("HI03_9") = DBNull.Value

                                End If


                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then
                                    HIRow("HI04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5)
                                    HI04 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5)
                                Else
                                    HIRow("HI04") = DBNull.Value

                                End If


                                If Not HI04 = String.Empty Then

                                    If (ss.ParseDemlimtedString(HI04, _ComponentElementSeparator, 1) <> "") Then HIRow("HI04_1") = ss.ParseDemlimtedString(HI04, _ComponentElementSeparator, 1) Else HIRow("HI04_1") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI04, _ComponentElementSeparator, 2) <> "") Then HIRow("HI04_2") = ss.ParseDemlimtedString(HI04, _ComponentElementSeparator, 2) Else HIRow("HI04_2") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI04, _ComponentElementSeparator, 3) <> "") Then HIRow("HI04_3") = ss.ParseDemlimtedString(HI04, _ComponentElementSeparator, 3) Else HIRow("HI04_3") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI04, _ComponentElementSeparator, 4) <> "") Then HIRow("HI04_4") = ss.ParseDemlimtedString(HI04, _ComponentElementSeparator, 4) Else HIRow("HI04_4") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI04, _ComponentElementSeparator, 5) <> "") Then HIRow("HI04_5") = ss.ParseDemlimtedString(HI04, _ComponentElementSeparator, 5) Else HIRow("HI04_5") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI04, _ComponentElementSeparator, 6) <> "") Then HIRow("HI04_6") = ss.ParseDemlimtedString(HI04, _ComponentElementSeparator, 6) Else HIRow("HI04_6") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI04, _ComponentElementSeparator, 7) <> "") Then HIRow("HI04_7") = ss.ParseDemlimtedString(HI04, _ComponentElementSeparator, 7) Else HIRow("HI04_7") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI04, _ComponentElementSeparator, 8) <> "") Then HIRow("HI04_8") = ss.ParseDemlimtedString(HI04, _ComponentElementSeparator, 8) Else HIRow("HI04_8") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI04, _ComponentElementSeparator, 9) <> "") Then HIRow("HI04_9") = ss.ParseDemlimtedString(HI04, _ComponentElementSeparator, 9) Else HIRow("HI04_9") = DBNull.Value

                                End If

                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then
                                    HIRow("HI05") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6)
                                    HI05 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6)
                                Else
                                    HIRow("HI05") = DBNull.Value

                                End If


                                If Not HI05 = String.Empty Then

                                    If (ss.ParseDemlimtedString(HI05, _ComponentElementSeparator, 1) <> "") Then HIRow("HI05_1") = ss.ParseDemlimtedString(HI05, _ComponentElementSeparator, 1) Else HIRow("HI05_1") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI05, _ComponentElementSeparator, 2) <> "") Then HIRow("HI05_2") = ss.ParseDemlimtedString(HI05, _ComponentElementSeparator, 2) Else HIRow("HI05_2") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI05, _ComponentElementSeparator, 3) <> "") Then HIRow("HI05_3") = ss.ParseDemlimtedString(HI05, _ComponentElementSeparator, 3) Else HIRow("HI05_3") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI05, _ComponentElementSeparator, 4) <> "") Then HIRow("HI05_4") = ss.ParseDemlimtedString(HI05, _ComponentElementSeparator, 4) Else HIRow("HI05_4") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI05, _ComponentElementSeparator, 5) <> "") Then HIRow("HI05_5") = ss.ParseDemlimtedString(HI05, _ComponentElementSeparator, 5) Else HIRow("HI05_5") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI05, _ComponentElementSeparator, 6) <> "") Then HIRow("HI05_6") = ss.ParseDemlimtedString(HI05, _ComponentElementSeparator, 6) Else HIRow("HI05_6") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI05, _ComponentElementSeparator, 7) <> "") Then HIRow("HI05_7") = ss.ParseDemlimtedString(HI05, _ComponentElementSeparator, 7) Else HIRow("HI05_7") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI05, _ComponentElementSeparator, 8) <> "") Then HIRow("HI05_8") = ss.ParseDemlimtedString(HI05, _ComponentElementSeparator, 8) Else HIRow("HI05_8") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI05, _ComponentElementSeparator, 9) <> "") Then HIRow("HI05_9") = ss.ParseDemlimtedString(HI05, _ComponentElementSeparator, 9) Else HIRow("HI05_9") = DBNull.Value

                                End If



                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then
                                    HIRow("HI06") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7)
                                    HI06 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7)
                                Else
                                    HIRow("HI06") = DBNull.Value

                                End If


                                If Not HI06 = String.Empty Then

                                    If (ss.ParseDemlimtedString(HI06, _ComponentElementSeparator, 1) <> "") Then HIRow("HI06_1") = ss.ParseDemlimtedString(HI06, _ComponentElementSeparator, 1) Else HIRow("HI06_1") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI06, _ComponentElementSeparator, 2) <> "") Then HIRow("HI06_2") = ss.ParseDemlimtedString(HI06, _ComponentElementSeparator, 2) Else HIRow("HI06_2") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI06, _ComponentElementSeparator, 3) <> "") Then HIRow("HI06_3") = ss.ParseDemlimtedString(HI06, _ComponentElementSeparator, 3) Else HIRow("HI06_3") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI06, _ComponentElementSeparator, 4) <> "") Then HIRow("HI06_4") = ss.ParseDemlimtedString(HI06, _ComponentElementSeparator, 4) Else HIRow("HI06_4") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI06, _ComponentElementSeparator, 5) <> "") Then HIRow("HI06_5") = ss.ParseDemlimtedString(HI06, _ComponentElementSeparator, 5) Else HIRow("HI06_5") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI06, _ComponentElementSeparator, 6) <> "") Then HIRow("HI06_6") = ss.ParseDemlimtedString(HI06, _ComponentElementSeparator, 6) Else HIRow("HI06_6") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI06, _ComponentElementSeparator, 7) <> "") Then HIRow("HI06_7") = ss.ParseDemlimtedString(HI06, _ComponentElementSeparator, 7) Else HIRow("HI06_7") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI06, _ComponentElementSeparator, 8) <> "") Then HIRow("HI06_8") = ss.ParseDemlimtedString(HI06, _ComponentElementSeparator, 8) Else HIRow("HI06_8") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI06, _ComponentElementSeparator, 9) <> "") Then HIRow("HI06_9") = ss.ParseDemlimtedString(HI06, _ComponentElementSeparator, 9) Else HIRow("HI06_9") = DBNull.Value

                                End If




                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then
                                    HIRow("HI07") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8)
                                    HI07 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8)
                                Else
                                    HIRow("HI07") = DBNull.Value

                                End If


                                If Not HI07 = String.Empty Then

                                    If (ss.ParseDemlimtedString(HI07, _ComponentElementSeparator, 1) <> "") Then HIRow("HI07_1") = ss.ParseDemlimtedString(HI07, _ComponentElementSeparator, 1) Else HIRow("HI07_1") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI07, _ComponentElementSeparator, 2) <> "") Then HIRow("HI07_2") = ss.ParseDemlimtedString(HI07, _ComponentElementSeparator, 2) Else HIRow("HI07_2") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI07, _ComponentElementSeparator, 3) <> "") Then HIRow("HI07_3") = ss.ParseDemlimtedString(HI07, _ComponentElementSeparator, 3) Else HIRow("HI07_3") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI07, _ComponentElementSeparator, 4) <> "") Then HIRow("HI07_4") = ss.ParseDemlimtedString(HI07, _ComponentElementSeparator, 4) Else HIRow("HI07_4") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI07, _ComponentElementSeparator, 5) <> "") Then HIRow("HI07_5") = ss.ParseDemlimtedString(HI07, _ComponentElementSeparator, 5) Else HIRow("HI07_5") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI07, _ComponentElementSeparator, 6) <> "") Then HIRow("HI07_6") = ss.ParseDemlimtedString(HI07, _ComponentElementSeparator, 6) Else HIRow("HI07_6") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI07, _ComponentElementSeparator, 7) <> "") Then HIRow("HI07_7") = ss.ParseDemlimtedString(HI07, _ComponentElementSeparator, 7) Else HIRow("HI07_7") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI07, _ComponentElementSeparator, 8) <> "") Then HIRow("HI07_8") = ss.ParseDemlimtedString(HI07, _ComponentElementSeparator, 8) Else HIRow("HI07_8") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI07, _ComponentElementSeparator, 9) <> "") Then HIRow("HI07_9") = ss.ParseDemlimtedString(HI07, _ComponentElementSeparator, 9) Else HIRow("HI07_9") = DBNull.Value

                                End If

                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) <> "") Then
                                    HIRow("HI08") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9)
                                    HI08 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9)
                                Else
                                    HIRow("HI08") = DBNull.Value

                                End If


                                If Not HI08 = String.Empty Then

                                    If (ss.ParseDemlimtedString(HI08, _ComponentElementSeparator, 1) <> "") Then HIRow("HI08_1") = ss.ParseDemlimtedString(HI08, _ComponentElementSeparator, 1) Else HIRow("HI08_1") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI08, _ComponentElementSeparator, 2) <> "") Then HIRow("HI08_2") = ss.ParseDemlimtedString(HI08, _ComponentElementSeparator, 2) Else HIRow("HI08_2") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI08, _ComponentElementSeparator, 3) <> "") Then HIRow("HI08_3") = ss.ParseDemlimtedString(HI08, _ComponentElementSeparator, 3) Else HIRow("HI08_3") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI08, _ComponentElementSeparator, 4) <> "") Then HIRow("HI08_4") = ss.ParseDemlimtedString(HI08, _ComponentElementSeparator, 4) Else HIRow("HI08_4") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI08, _ComponentElementSeparator, 5) <> "") Then HIRow("HI08_5") = ss.ParseDemlimtedString(HI08, _ComponentElementSeparator, 5) Else HIRow("HI08_5") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI08, _ComponentElementSeparator, 6) <> "") Then HIRow("HI08_6") = ss.ParseDemlimtedString(HI08, _ComponentElementSeparator, 6) Else HIRow("HI08_6") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI08, _ComponentElementSeparator, 7) <> "") Then HIRow("HI08_7") = ss.ParseDemlimtedString(HI08, _ComponentElementSeparator, 7) Else HIRow("HI08_7") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI08, _ComponentElementSeparator, 8) <> "") Then HIRow("HI08_8") = ss.ParseDemlimtedString(HI08, _ComponentElementSeparator, 8) Else HIRow("HI08_8") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI08, _ComponentElementSeparator, 9) <> "") Then HIRow("HI08_9") = ss.ParseDemlimtedString(HI08, _ComponentElementSeparator, 9) Else HIRow("HI08_9") = DBNull.Value

                                End If

                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) <> "") Then
                                    HIRow("HI09") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10)
                                    HI09 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10)
                                Else
                                    HIRow("HI09") = DBNull.Value

                                End If


                                If Not HI09 = String.Empty Then

                                    If (ss.ParseDemlimtedString(HI09, _ComponentElementSeparator, 1) <> "") Then HIRow("HI09_1") = ss.ParseDemlimtedString(HI09, _ComponentElementSeparator, 1) Else HIRow("HI09_1") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI09, _ComponentElementSeparator, 2) <> "") Then HIRow("HI09_2") = ss.ParseDemlimtedString(HI09, _ComponentElementSeparator, 2) Else HIRow("HI09_2") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI09, _ComponentElementSeparator, 3) <> "") Then HIRow("HI09_3") = ss.ParseDemlimtedString(HI09, _ComponentElementSeparator, 3) Else HIRow("HI09_3") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI09, _ComponentElementSeparator, 4) <> "") Then HIRow("HI09_4") = ss.ParseDemlimtedString(HI09, _ComponentElementSeparator, 4) Else HIRow("HI09_4") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI09, _ComponentElementSeparator, 5) <> "") Then HIRow("HI09_5") = ss.ParseDemlimtedString(HI09, _ComponentElementSeparator, 5) Else HIRow("HI09_5") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI09, _ComponentElementSeparator, 6) <> "") Then HIRow("HI09_6") = ss.ParseDemlimtedString(HI09, _ComponentElementSeparator, 6) Else HIRow("HI09_6") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI09, _ComponentElementSeparator, 7) <> "") Then HIRow("HI09_7") = ss.ParseDemlimtedString(HI09, _ComponentElementSeparator, 7) Else HIRow("HI09_7") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI09, _ComponentElementSeparator, 8) <> "") Then HIRow("HI09_8") = ss.ParseDemlimtedString(HI09, _ComponentElementSeparator, 8) Else HIRow("HI09_8") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI09, _ComponentElementSeparator, 9) <> "") Then HIRow("HI09_9") = ss.ParseDemlimtedString(HI09, _ComponentElementSeparator, 9) Else HIRow("HI09_9") = DBNull.Value

                                End If


                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11) <> "") Then
                                    HIRow("HI10") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11)
                                    HI10 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11)
                                Else
                                    HIRow("HI10") = DBNull.Value

                                End If


                                If Not HI10 = String.Empty Then

                                    If (ss.ParseDemlimtedString(HI10, _ComponentElementSeparator, 1) <> "") Then HIRow("HI10_1") = ss.ParseDemlimtedString(HI10, _ComponentElementSeparator, 1) Else HIRow("HI10_1") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI10, _ComponentElementSeparator, 2) <> "") Then HIRow("HI10_2") = ss.ParseDemlimtedString(HI10, _ComponentElementSeparator, 2) Else HIRow("HI10_2") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI10, _ComponentElementSeparator, 3) <> "") Then HIRow("HI10_3") = ss.ParseDemlimtedString(HI10, _ComponentElementSeparator, 3) Else HIRow("HI10_3") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI10, _ComponentElementSeparator, 4) <> "") Then HIRow("HI10_4") = ss.ParseDemlimtedString(HI10, _ComponentElementSeparator, 4) Else HIRow("HI10_4") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI10, _ComponentElementSeparator, 5) <> "") Then HIRow("HI10_5") = ss.ParseDemlimtedString(HI10, _ComponentElementSeparator, 5) Else HIRow("HI10_5") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI10, _ComponentElementSeparator, 6) <> "") Then HIRow("HI10_6") = ss.ParseDemlimtedString(HI10, _ComponentElementSeparator, 6) Else HIRow("HI10_6") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI10, _ComponentElementSeparator, 7) <> "") Then HIRow("HI10_7") = ss.ParseDemlimtedString(HI10, _ComponentElementSeparator, 7) Else HIRow("HI10_7") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI10, _ComponentElementSeparator, 8) <> "") Then HIRow("HI10_8") = ss.ParseDemlimtedString(HI10, _ComponentElementSeparator, 8) Else HIRow("HI10_8") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI10, _ComponentElementSeparator, 9) <> "") Then HIRow("HI10_9") = ss.ParseDemlimtedString(HI10, _ComponentElementSeparator, 9) Else HIRow("HI10_9") = DBNull.Value

                                End If

                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12) <> "") Then
                                    HIRow("HI11") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12)
                                    HI11 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12)
                                Else
                                    HIRow("HI11") = DBNull.Value

                                End If


                                If Not HI11 = String.Empty Then

                                    If (ss.ParseDemlimtedString(HI11, _ComponentElementSeparator, 1) <> "") Then HIRow("HI11_1") = ss.ParseDemlimtedString(HI11, _ComponentElementSeparator, 1) Else HIRow("HI11_1") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI11, _ComponentElementSeparator, 2) <> "") Then HIRow("HI11_2") = ss.ParseDemlimtedString(HI11, _ComponentElementSeparator, 2) Else HIRow("HI11_2") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI11, _ComponentElementSeparator, 3) <> "") Then HIRow("HI11_3") = ss.ParseDemlimtedString(HI11, _ComponentElementSeparator, 3) Else HIRow("HI11_3") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI11, _ComponentElementSeparator, 4) <> "") Then HIRow("HI11_4") = ss.ParseDemlimtedString(HI11, _ComponentElementSeparator, 4) Else HIRow("HI11_4") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI11, _ComponentElementSeparator, 5) <> "") Then HIRow("HI11_5") = ss.ParseDemlimtedString(HI11, _ComponentElementSeparator, 5) Else HIRow("HI11_5") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI11, _ComponentElementSeparator, 6) <> "") Then HIRow("HI11_6") = ss.ParseDemlimtedString(HI11, _ComponentElementSeparator, 6) Else HIRow("HI11_6") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI11, _ComponentElementSeparator, 7) <> "") Then HIRow("HI11_7") = ss.ParseDemlimtedString(HI11, _ComponentElementSeparator, 7) Else HIRow("HI11_7") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI11, _ComponentElementSeparator, 8) <> "") Then HIRow("HI11_8") = ss.ParseDemlimtedString(HI11, _ComponentElementSeparator, 8) Else HIRow("HI11_8") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI11, _ComponentElementSeparator, 9) <> "") Then HIRow("HI11_9") = ss.ParseDemlimtedString(HI11, _ComponentElementSeparator, 9) Else HIRow("HI11_9") = DBNull.Value

                                End If


                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 13) <> "") Then
                                    HIRow("HI12") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 13)
                                    HI12 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 13)
                                Else
                                    HIRow("HI12") = DBNull.Value

                                End If


                                If Not HI12 = String.Empty Then

                                    If (ss.ParseDemlimtedString(HI12, _ComponentElementSeparator, 1) <> "") Then HIRow("HI12_1") = ss.ParseDemlimtedString(HI12, _ComponentElementSeparator, 1) Else HIRow("HI12_1") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI12, _ComponentElementSeparator, 2) <> "") Then HIRow("HI12_2") = ss.ParseDemlimtedString(HI12, _ComponentElementSeparator, 2) Else HIRow("HI12_2") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI12, _ComponentElementSeparator, 3) <> "") Then HIRow("HI12_3") = ss.ParseDemlimtedString(HI12, _ComponentElementSeparator, 3) Else HIRow("HI12_3") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI12, _ComponentElementSeparator, 4) <> "") Then HIRow("HI12_4") = ss.ParseDemlimtedString(HI12, _ComponentElementSeparator, 4) Else HIRow("HI12_4") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI12, _ComponentElementSeparator, 5) <> "") Then HIRow("HI12_5") = ss.ParseDemlimtedString(HI12, _ComponentElementSeparator, 5) Else HIRow("HI12_5") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI12, _ComponentElementSeparator, 6) <> "") Then HIRow("HI12_6") = ss.ParseDemlimtedString(HI12, _ComponentElementSeparator, 6) Else HIRow("HI12_6") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI12, _ComponentElementSeparator, 7) <> "") Then HIRow("HI12_7") = ss.ParseDemlimtedString(HI12, _ComponentElementSeparator, 7) Else HIRow("HI12_7") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI12, _ComponentElementSeparator, 8) <> "") Then HIRow("HI12_8") = ss.ParseDemlimtedString(HI12, _ComponentElementSeparator, 8) Else HIRow("HI12_8") = DBNull.Value
                                    If (ss.ParseDemlimtedString(HI12, _ComponentElementSeparator, 9) <> "") Then HIRow("HI12_9") = ss.ParseDemlimtedString(HI12, _ComponentElementSeparator, 9) Else HIRow("HI12_9") = DBNull.Value

                                End If

                                HIRow("ROW_NUMBER") = _ROW_COUNT
                                HIRow("EQ_ROW_NUMBER") = 0


                                HIRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                HIRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                                HIRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                                HI.Rows.Add(HIRow)




                                Select Case _270_LOOP_LEVEL
                                    Case "HEADER"
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "ISL"
                                        _270_ISL_STRING = _270_ISL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "IRL"
                                        _270_IRL_STRING = _270_IRL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "SL"
                                        _270_SL_STRING = _270_SL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "DL"
                                        _270_DL_STRING = _270_DL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                End Select

                            End If
                        Catch ex As Exception

                            _RowProcessedFlag = -2
                            _hasERR = True
                            log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                        End Try




                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        ' SL/DL  DTP
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        Try
                            If _RowRecordType = "DTP" Then

                                _RowProcessedFlag = 1

                                Dim DTPRow As DataRow = DTP.NewRow
                                DTPRow("DOCUMENT_ID") = _DOCUMENT_ID
                                DTPRow("FILE_ID") = _FILE_ID
                                DTPRow("BATCH_ID") = _270_BATCH_ID
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
                                DTPRow("270_ISL_GUID") = _270_ISL_GUID
                                DTPRow("270_IRL_GUID") = _270_IRL_GUID
                                DTPRow("270_SL_GUID") = _270_SL_GUID
                                DTPRow("270_DL_GUID") = _270_DL_GUID
                                DTPRow("270_EQ_GUID") = _270_EQ_GUID
                                DTPRow("NM1_GUID") = _NM1_GUID
                                DTPRow("HL01") = _HL01
                                DTPRow("HL02") = _HL02
                                DTPRow("HL03") = _HL03
                                DTPRow("HL04") = _HL04

                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then DTPRow("DTP01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else DTPRow("DTP01") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then DTPRow("DTP02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else DTPRow("DTP02") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then DTPRow("DTP03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else DTPRow("DTP03") = DBNull.Value
                                '    If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then DTPRow("DTP04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else DTPRow("DTP04") = DBNull.Value


                                DTPRow("ROW_NUMBER") = _ROW_COUNT
                                DTPRow("EQ_ROW_NUMBER") = 0
                                DTPRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                DTPRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                                DTPRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                                DTP.Rows.Add(DTPRow)


                                Select Case _270_LOOP_LEVEL
                                    Case "HEADER"
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "ISL"
                                        _270_ISL_STRING = _270_ISL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "IRL"
                                        _270_IRL_STRING = _270_IRL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "SL"
                                        _270_SL_STRING = _270_SL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                    Case "DL"
                                        _270_DL_STRING = _270_DL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                End Select
                            End If

                        Catch ex As Exception

                            _RowProcessedFlag = -2
                            _hasERR = True
                            log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                        End Try


                    End If
                    'END ED if
                    '==========================================================================================================================================================


                End If
                'END SL/DL BLOCK
                '==========================================================================================================================================================









                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   END 271 
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''









                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   SE
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "SE" Then




                        Dim SERow As DataRow = SE_N.NewRow
                        SERow("FILE_ID") = _FILE_ID
                        SERow("BATCH_ID") = _270_BATCH_ID
                        SERow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                        SERow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                        SERow("HIPAA_ST_GUID") = _HIPAA_ST_GUID

                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then SERow("SE01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else SERow("SE01") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then SERow("SE02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else SERow("SE02") = DBNull.Value

                        SERow("ROW_NUMBER") = _ROW_COUNT
                        SE_N.Rows.Add(SERow)


                        ProcessEQ(_EQIList, _CURRENT_EQ)


                        If _2000A_DIRTY Then

                            COMIT2000AHeaderDump()
                            _2000A_DIRTY = False

                        End If

                        If _2000B_DIRTY Then

                            COMIT2000BHeaderDump()
                            _2000B_DIRTY = False

                        End If



                        If _HL22_DIRTY Then
                            COMIT_270_22_Dump()
                            '  ComitHL20()
                            _HL22_DIRTY = False
                        End If




                        ComitSE()

                        ClearST()


                        _RowProcessedFlag = 1


                    End If
                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                End Try



                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   GE
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "GE" Then
                        _RowProcessedFlag = 1






                        Dim GERow As DataRow = GE_N.NewRow
                        GERow("FILE_ID") = _FILE_ID
                        GERow("BATCH_ID") = _270_BATCH_ID
                        GERow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                        GERow("HIPAA_GS_GUID") = _HIPAA_GS_GUID


                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then GERow("GE01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else GERow("GE01") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then GERow("GE02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else GERow("GE02") = DBNull.Value

                        GERow("ROW_NUMBER") = _ROW_COUNT
                        GE_N.Rows.Add(GERow)


                        ComitGE()
                        ClearGS()
                        'committ the last ST



                    End If
                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                End Try



                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   IEA
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "IEA" Then


                        _RowProcessedFlag = 1

                        Dim IEARow As DataRow = IEA_N.NewRow
                        IEARow("FILE_ID") = _FILE_ID
                        IEARow("BATCH_ID") = _270_BATCH_ID
                        IEARow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID


                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then IEARow("IEA01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else IEARow("IEA01") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then IEARow("IEA02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else IEARow("IEA02") = DBNull.Value

                        IEARow("ROW_NUMBER") = _ROW_COUNT
                        IEA_N.Rows.Add(IEARow)


                        ComitIEA()
                        ClearISA()


                    End If
                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                End Try


                Try
                    If _RowProcessedFlag = 0 Then

                        _hasUNK = True

                        Dim UNKRow As DataRow = UNK.NewRow
                        UNKRow("DOCUMENT_ID") = _DOCUMENT_ID
                        UNKRow("FILE_ID") = _FILE_ID
                        UNKRow("BATCH_ID") = _270_BATCH_ID
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
                        UNKRow("ROW_RECORD_TYPE") = _RowRecordType
                        UNKRow("ROW_DATA") = _CurrentRowData
                        UNKRow("ROW_NUMBER") = _ROW_COUNT
                        UNKRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        UNKRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                        UNKRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                        UNK.Rows.Add(UNKRow)




                        Select Case _270_LOOP_LEVEL
                            Case "HEADER"
                                _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case "ISL"
                                _270_ISL_STRING = _270_ISL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case "IRL"
                                _270_IRL_STRING = _270_IRL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case "SL"
                                _270_SL_STRING = _270_SL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case "DL"
                                _270_DL_STRING = _270_DL_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                'Case "EQ"
                                '    _271_EQ_STRING = _271_EQ_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                        End Select


                    End If

                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                End Try


                If _hasERR Then


                End If


            Next


            If _hasUNK Then
                Using e As New EDI_5010_LOGGING
                    e.ConnectionString = _ConnectionString
                    e.TransactionSetIdentifierCode = "270"
                    If _hasERR Then
                        e.UpdateFileStatus(CInt(_FILE_ID), "270", "PARSE COMPLETE WITH UNK SEGMENTS WITH ERRORS")
                    Else
                        e.UpdateFileStatus(CInt(_FILE_ID), "270", "PARSE COMPLETE WITH UNK SEGMENTS")
                    End If



                End Using

                ComitUNK()

            Else

                Using e As New EDI_5010_LOGGING
                    e.ConnectionString = _ConnectionString
                    e.TransactionSetIdentifierCode = "270"
                    If _hasERR Then
                        e.UpdateFileStatus(CInt(_FILE_ID), "270", "PARSE COMPLETE WITH ERRORS")
                    Else
                        e.UpdateFileStatus(CInt(_FILE_ID), "270", "PARSE COMPLETE")
                    End If
                End Using

            End If




            _ProcessEndTime = DateTime.Now

            Return _ImportReturnCode

        End Function
        Public Function CountCharacter(ByVal value As String, ByVal ch As Char) As Integer
            Return value.Count(Function(c As Char) c = ch)
        End Function

        Private Function ProcessEQ(ByVal EQIList As List(Of String), ByVal PEQ As String) As Integer
            Dim r As Integer = -1
            Dim c As Integer = 0
            Dim x As Integer = 0




            _FUNCTION_NAME = "ProcessEQ(ByVal EQIList As List(Of String), ByVal PEQ As String) As Integer"

            Dim __EQ_RowRecordType As String = String.Empty
            Dim __EQ_CurrentRowData As String = String.Empty
            Dim __EQ_STRING As String = String.Empty
            Dim __EQ13 As String = String.Empty
            Dim __EQ__ROW_COUNT As Integer = 0
            Dim __ORD__ROW_COUNT As Integer = 0


            Dim __EQ_GROUP_GUID As Guid = Guid.NewGuid
            Dim __LS_GUID As Guid = Guid.Empty
            Dim __EQ_GUID As Guid = Guid.Empty
            _LoopLevelMajor = 2100
            _LoopLevelMinor = 10





            __EQ_STRING = ss.ParseDemlimtedStringEDI(PEQ, _DataElementSeparator, 4)



            '  __MSGList = EQIList

            '  __MSGList_SMAHASY = MSGSmasherThingy(__MSGList)


            c = CountCharacter(PEQ, CChar(_RepetitionSeparator))

            Do Until x = c + 1
                x += 1

                Dim __EQIList As List(Of String)


                __EQIList = EQIList



                For Each line As String In __EQIList

                    '  Dim ___MSGList_SMAHASY As List(Of String)
                    '  ___MSGList_SMAHASY = __MSGList_SMAHASY

                    __ORD__ROW_COUNT = Convert.ToInt32(ss.ParseDemlimtedStringEDI(line, "`", 1))


                    Dim _EQ_STRING_CURRENT = ss.ParseDemlimtedStringEDI(__EQ_STRING, _RepetitionSeparator, x)

                    __EQ__ROW_COUNT = __EQ__ROW_COUNT + 1




                    'Console.WriteLine(_RowRecordType)
                    __EQ_CurrentRowData = ss.ParseDemlimtedStringEDI(line, "`", 2)
                    __EQ_RowRecordType = ss.ParseDemlimtedStringEDI(__EQ_CurrentRowData, _DataElementSeparator, 1)

                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   EQ :: EQ
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If __EQ_RowRecordType = "EQ" Then

                            __EQ_GUID = Guid.NewGuid



                            Dim EQRow As DataRow = EQ.NewRow

                            EQRow("DOCUMENT_ID") = _DOCUMENT_ID
                            EQRow("FILE_ID") = _FILE_ID
                            EQRow("BATCH_ID") = _270_BATCH_ID
                            EQRow("ISA_ID") = _ISA_ID
                            EQRow("GS_ID") = _GS_ID
                            EQRow("ST_ID") = _ST_ID
                            EQRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            EQRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            EQRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            EQRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            EQRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            EQRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            EQRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            EQRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            EQRow("270_ISL_GUID") = _270_ISL_GUID
                            EQRow("270_IRL_GUID") = _270_IRL_GUID
                            EQRow("270_SL_GUID") = _270_SL_GUID
                            EQRow("270_DL_GUID") = _270_DL_GUID
                            EQRow("270_EQ_GROUP_GUID") = __EQ_GROUP_GUID
                            EQRow("270_EQ_GUID") = __EQ_GUID
                            EQRow("NM1_GUID") = _NM1_GUID
                            EQRow("HL01") = _HL01
                            EQRow("HL02") = _HL02
                            EQRow("HL03") = _HL03
                            EQRow("HL04") = _HL04

                            If (ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 2) <> "") Then EQRow("EQ01") = ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 2) Else EQRow("EQ02") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 3) <> "") Then EQRow("EQ02") = ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 3) Else EQRow("EQ02") = DBNull.Value


                            Dim __EQ_2 As String = String.Empty

                            If (ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 3) <> "") Then
                                EQRow("EQ02") = ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 3)
                                __EQ_2 = ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 3)
                            Else
                                EQRow("EQ02") = DBNull.Value
                            End If


                            If (ss.ParseDemlimtedString(__EQ_2, _ComponentElementSeparator, 1) <> "") Then EQRow("EQ02_1") = ss.ParseDemlimtedString(__EQ_2, _ComponentElementSeparator, 1) Else EQRow("EQ02_1") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EQ_2, _ComponentElementSeparator, 2) <> "") Then EQRow("EQ02_2") = ss.ParseDemlimtedString(__EQ_2, _ComponentElementSeparator, 2) Else EQRow("EQ02_2") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EQ_2, _ComponentElementSeparator, 3) <> "") Then EQRow("EQ02_3") = ss.ParseDemlimtedString(__EQ_2, _ComponentElementSeparator, 3) Else EQRow("EQ02_3") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EQ_2, _ComponentElementSeparator, 4) <> "") Then EQRow("EQ02_4") = ss.ParseDemlimtedString(__EQ_2, _ComponentElementSeparator, 4) Else EQRow("EQ02_4") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EQ_2, _ComponentElementSeparator, 5) <> "") Then EQRow("EQ02_5") = ss.ParseDemlimtedString(__EQ_2, _ComponentElementSeparator, 5) Else EQRow("EQ02_5") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EQ_2, _ComponentElementSeparator, 6) <> "") Then EQRow("EQ02_6") = ss.ParseDemlimtedString(__EQ_2, _ComponentElementSeparator, 6) Else EQRow("EQ02_6") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EQ_2, _ComponentElementSeparator, 7) <> "") Then EQRow("EQ02_7") = ss.ParseDemlimtedString(__EQ_2, _ComponentElementSeparator, 7) Else EQRow("EQ02_7") = DBNull.Value




                            If (ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 4) <> "") Then EQRow("EQ03") = ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 4) Else EQRow("EQ03") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 5) <> "") Then EQRow("EQ04") = ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 5) Else EQRow("EQ04") = DBNull.Value





                            Dim __EQ_5 As String = String.Empty

                            If (ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 6) <> "") Then
                                EQRow("EQ05") = ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 6)
                                __EQ_5 = ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 6)
                            Else
                                EQRow("EQ05") = DBNull.Value
                            End If


                            If (ss.ParseDemlimtedString(__EQ_5, _ComponentElementSeparator, 1) <> "") Then EQRow("EQ05_1") = ss.ParseDemlimtedString(__EQ_5, _ComponentElementSeparator, 1) Else EQRow("EQ05_1") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EQ_5, _ComponentElementSeparator, 2) <> "") Then EQRow("EQ05_2") = ss.ParseDemlimtedString(__EQ_5, _ComponentElementSeparator, 2) Else EQRow("EQ05_2") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EQ_5, _ComponentElementSeparator, 3) <> "") Then EQRow("EQ05_3") = ss.ParseDemlimtedString(__EQ_5, _ComponentElementSeparator, 3) Else EQRow("EQ05_3") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EQ_5, _ComponentElementSeparator, 4) <> "") Then EQRow("EQ05_4") = ss.ParseDemlimtedString(__EQ_5, _ComponentElementSeparator, 4) Else EQRow("EQ05_4") = DBNull.Value




                            'If (ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 5) <> "") Then EQRow("EQ04") = ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 5) Else EQRow("EQ04") = DBNull.Value
                            'If (ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 6) <> "") Then EQRow("EQ05") = ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 6) Else EQRow("EQ05") = DBNull.Value
                            'If (ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 7) <> "") Then EQRow("EQ06") = ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 7) Else EQRow("EQ06") = DBNull.Value
                            'If (ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 8) <> "") Then EQRow("EQ07") = ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 8) Else EQRow("EQ07") = DBNull.Value
                            'If (ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 9) <> "") Then EQRow("EQ08") = ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 9) Else EQRow("EQ08") = DBNull.Value
                            'If (ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 10) <> "") Then EQRow("EQ09") = ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 10) Else EQRow("EQ09") = DBNull.Value
                            'If (ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 11) <> "") Then EQRow("EQ10") = ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 11) Else EQRow("EQ10") = DBNull.Value
                            'If (ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 12) <> "") Then EQRow("EQ11") = ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 12) Else EQRow("EQ11") = DBNull.Value
                            'If (ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 13) <> "") Then EQRow("EQ12") = ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 13) Else EQRow("EQ12") = DBNull.Value
                            'If (ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 14) <> "") Then EQRow("EQ13") = ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 14) Else EQRow("EQ13") = DBNull.Value



                            'If (ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 14) <> "") Then
                            '    EQRow("EQ13") = ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 14)
                            '    __EQ13 = ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 14)
                            'Else
                            '    EQRow("EQ13") = DBNull.Value
                            'End If


                            'If (ss.ParseDemlimtedString(__EQ13, _ComponentElementSeparator, 1) <> "") Then EQRow("EQ13_1") = ss.ParseDemlimtedString(__EQ13, _ComponentElementSeparator, 1) Else EQRow("EQ13_1") = DBNull.Value
                            'If (ss.ParseDemlimtedString(__EQ13, _ComponentElementSeparator, 2) <> "") Then EQRow("EQ13_2") = ss.ParseDemlimtedString(__EQ13, _ComponentElementSeparator, 2) Else EQRow("EQ13_2") = DBNull.Value
                            'If (ss.ParseDemlimtedString(__EQ13, _ComponentElementSeparator, 3) <> "") Then EQRow("EQ13_3") = ss.ParseDemlimtedString(__EQ13, _ComponentElementSeparator, 3) Else EQRow("EQ13_3") = DBNull.Value
                            'If (ss.ParseDemlimtedString(__EQ13, _ComponentElementSeparator, 4) <> "") Then EQRow("EQ13_4") = ss.ParseDemlimtedString(__EQ13, _ComponentElementSeparator, 4) Else EQRow("EQ13_4") = DBNull.Value
                            'If (ss.ParseDemlimtedString(__EQ13, _ComponentElementSeparator, 5) <> "") Then EQRow("EQ13_5") = ss.ParseDemlimtedString(__EQ13, _ComponentElementSeparator, 5) Else EQRow("EQ13_5") = DBNull.Value
                            'If (ss.ParseDemlimtedString(__EQ13, _ComponentElementSeparator, 6) <> "") Then EQRow("EQ13_6") = ss.ParseDemlimtedString(__EQ13, _ComponentElementSeparator, 6) Else EQRow("EQ13_6") = DBNull.Value
                            'If (ss.ParseDemlimtedString(__EQ13, _ComponentElementSeparator, 7) <> "") Then EQRow("EQ13_7") = ss.ParseDemlimtedString(__EQ13, _ComponentElementSeparator, 7) Else EQRow("EQ13_7") = DBNull.Value

                            EQRow("ROW_NUMBER") = __ORD__ROW_COUNT
                            EQRow("EQ_ROW_NUMBER") = __EQ__ROW_COUNT


                            EQRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            EQRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            EQRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                            EQ.Rows.Add(EQRow)


                            Select Case _270_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(__EQ__ROW_COUNT) + "::" + __EQ_CurrentRowData + "~"
                                Case "ISL"
                                    _270_ISL_STRING = _270_ISL_STRING + Convert.ToString(__EQ__ROW_COUNT) + "::" + __EQ_CurrentRowData + "~"
                                Case "IRL"
                                    _270_IRL_STRING = _270_IRL_STRING + Convert.ToString(__EQ__ROW_COUNT) + "::" + __EQ_CurrentRowData + "~"

                                Case "SL"
                                    _270_SL_STRING = _270_SL_STRING + Convert.ToString(__EQ__ROW_COUNT) + "::" + __EQ_CurrentRowData + "~"
                                Case "DL"
                                    _270_DL_STRING = _270_DL_STRING + Convert.ToString(__EQ__ROW_COUNT) + "::" + __EQ_CurrentRowData + "~"
                            End Select


                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, __EQ_RowRecordType, ex)
                    End Try


                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '  EQ ::  AMT
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    Try

                        If __EQ_RowRecordType = "AMT" Then

                            Dim AMTRow As DataRow = AMT.NewRow

                            AMTRow("DOCUMENT_ID") = _DOCUMENT_ID
                            AMTRow("FILE_ID") = _FILE_ID
                            AMTRow("BATCH_ID") = _270_BATCH_ID
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
                            AMTRow("270_ISL_GUID") = _270_ISL_GUID
                            AMTRow("270_IRL_GUID") = _270_IRL_GUID
                            AMTRow("270_SL_GUID") = _270_SL_GUID
                            AMTRow("270_DL_GUID") = _270_DL_GUID
                            AMTRow("270_EQ_GROUP_GUID") = __EQ_GROUP_GUID
                            AMTRow("270_EQ_GUID") = __EQ_GUID
                            AMTRow("NM1_GUID") = _NM1_GUID
                            AMTRow("HL01") = _HL01
                            AMTRow("HL02") = _HL02
                            AMTRow("HL03") = _HL03
                            AMTRow("HL04") = _HL04


                            If (ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 2) <> "") Then AMTRow("AMT01") = ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 2) Else AMTRow("AMT01") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 3) <> "") Then AMTRow("AMT02") = ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 3) Else AMTRow("AMT02") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 4) <> "") Then AMTRow("AMT03") = ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 4) Else AMTRow("AMT03") = DBNull.Value


                            AMTRow("ROW_NUMBER") = __ORD__ROW_COUNT
                            AMTRow("EQ_ROW_NUMBER") = __EQ__ROW_COUNT
                            AMTRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            AMTRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            AMTRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                            AMT.Rows.Add(AMTRow)



                            Select Case _270_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(__EQ__ROW_COUNT) + "::" + __EQ_CurrentRowData + "~"
                                Case "ISL"
                                    _270_ISL_STRING = _270_ISL_STRING + Convert.ToString(__EQ__ROW_COUNT) + "::" + __EQ_CurrentRowData + "~"
                                Case "IRL"
                                    _270_IRL_STRING = _270_IRL_STRING + Convert.ToString(__EQ__ROW_COUNT) + "::" + __EQ_CurrentRowData + "~"

                                Case "SL"
                                    _270_SL_STRING = _270_SL_STRING + Convert.ToString(__EQ__ROW_COUNT) + "::" + __EQ_CurrentRowData + "~"
                                Case "DL"
                                    _270_DL_STRING = _270_DL_STRING + Convert.ToString(__EQ__ROW_COUNT) + "::" + __EQ_CurrentRowData + "~"
                            End Select

                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, __EQ_RowRecordType, ex)
                    End Try




                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   EQ :: REF
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If __EQ_RowRecordType = "REF" Then

                            Dim REFRow As DataRow = REF.NewRow

                            REFRow("DOCUMENT_ID") = _DOCUMENT_ID
                            REFRow("FILE_ID") = _FILE_ID
                            REFRow("BATCH_ID") = _270_BATCH_ID
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
                            REFRow("270_ISL_GUID") = _270_ISL_GUID
                            REFRow("270_IRL_GUID") = _270_IRL_GUID
                            REFRow("270_SL_GUID") = _270_SL_GUID
                            REFRow("270_DL_GUID") = _270_DL_GUID
                            REFRow("270_EQ_GROUP_GUID") = __EQ_GROUP_GUID
                            REFRow("270_EQ_GUID") = __EQ_GUID
                            REFRow("NM1_GUID") = _NM1_GUID
                            REFRow("HL01") = _HL01
                            REFRow("HL02") = _HL02
                            REFRow("HL03") = _HL03
                            REFRow("HL04") = _HL04


                            If (ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 2) <> "") Then REFRow("REF01") = ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 2) Else REFRow("REF01") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 3) <> "") Then REFRow("REF02") = ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 3) Else REFRow("REF02") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 4) <> "") Then REFRow("REF03") = ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 4) Else REFRow("REF03") = DBNull.Value


                            REFRow("ROW_NUMBER") = __ORD__ROW_COUNT
                            REFRow("EQ_ROW_NUMBER") = __EQ__ROW_COUNT
                            REFRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            REFRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            REFRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                            REF.Rows.Add(REFRow)



                            Select Case _270_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(__EQ__ROW_COUNT) + "::" + __EQ_CurrentRowData + "~"
                                Case "ISL"
                                    _270_ISL_STRING = _270_ISL_STRING + Convert.ToString(__EQ__ROW_COUNT) + "::" + __EQ_CurrentRowData + "~"
                                Case "IRL"
                                    _270_IRL_STRING = _270_IRL_STRING + Convert.ToString(__EQ__ROW_COUNT) + "::" + __EQ_CurrentRowData + "~"

                                Case "SL"
                                    _270_SL_STRING = _270_SL_STRING + Convert.ToString(__EQ__ROW_COUNT) + "::" + __EQ_CurrentRowData + "~"
                                Case "DL"
                                    _270_DL_STRING = _270_DL_STRING + Convert.ToString(__EQ__ROW_COUNT) + "::" + __EQ_CurrentRowData + "~"
                            End Select

                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, __EQ_RowRecordType, ex)
                    End Try






                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '  EQ ::  DTP
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    Try

                        If __EQ_RowRecordType = "DTP" Then

                            Dim DTPRow As DataRow = DTP.NewRow

                            DTPRow("DOCUMENT_ID") = _DOCUMENT_ID
                            DTPRow("FILE_ID") = _FILE_ID
                            DTPRow("BATCH_ID") = _270_BATCH_ID
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
                            DTPRow("270_ISL_GUID") = _270_ISL_GUID
                            DTPRow("270_IRL_GUID") = _270_IRL_GUID
                            DTPRow("270_SL_GUID") = _270_SL_GUID
                            DTPRow("270_DL_GUID") = _270_DL_GUID
                            DTPRow("270_EQ_GROUP_GUID") = __EQ_GROUP_GUID
                            DTPRow("270_EQ_GUID") = __EQ_GUID
                            DTPRow("NM1_GUID") = _NM1_GUID
                            DTPRow("HL01") = _HL01
                            DTPRow("HL02") = _HL02
                            DTPRow("HL03") = _HL03
                            DTPRow("HL04") = _HL04


                            If (ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 2) <> "") Then DTPRow("DTP01") = ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 2) Else DTPRow("DTP01") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 3) <> "") Then DTPRow("DTP02") = ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 3) Else DTPRow("DTP02") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 4) <> "") Then DTPRow("DTP03") = ss.ParseDemlimtedString(__EQ_CurrentRowData, _DataElementSeparator, 4) Else DTPRow("DTP03") = DBNull.Value



                            DTPRow("ROW_NUMBER") = __ORD__ROW_COUNT
                            DTPRow("EQ_ROW_NUMBER") = __EQ__ROW_COUNT
                            DTPRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            DTPRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            DTPRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                            DTP.Rows.Add(DTPRow)



                            Select Case _270_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(__EQ__ROW_COUNT) + "::" + __EQ_CurrentRowData + "~"
                                Case "ISL"
                                    _270_ISL_STRING = _270_ISL_STRING + Convert.ToString(__EQ__ROW_COUNT) + "::" + __EQ_CurrentRowData + "~"
                                Case "IRL"
                                    _270_IRL_STRING = _270_IRL_STRING + Convert.ToString(__EQ__ROW_COUNT) + "::" + __EQ_CurrentRowData + "~"

                                Case "SL"
                                    _270_SL_STRING = _270_SL_STRING + Convert.ToString(__EQ__ROW_COUNT) + "::" + __EQ_CurrentRowData + "~"
                                Case "DL"
                                    _270_DL_STRING = _270_DL_STRING + Convert.ToString(__EQ__ROW_COUNT) + "::" + __EQ_CurrentRowData + "~"
                            End Select

                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, __EQ_RowRecordType, ex)
                    End Try

                Next

            Loop

            _EQ_DIRTY = False

            Return r


        End Function




        Private Function Cleanup() As Int32
            Dim r As Integer = 0


            _FUNCTION_NAME = "Cleanup() As Int32"

            Dim FinalPath As String = String.Empty

            Try

                Dim span As TimeSpan

                span = _ProcessEndTime - _ProcessStartTime
                '  _ProcessElaspedTime = span.TotalMilliseconds

            Catch ex As Exception
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
            End Try






            Return r
        End Function

        Private Function ComitISA() As Integer


            _FUNCTION_NAME = "ComitISA()"
            Dim r As Integer = -1

            Try
                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_ISA, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_270_ISA", ISA)
                        cmd.Parameters.Add("@ISA_ID", Data.SqlDbType.BigInt, 1)
                        cmd.Parameters("@ISA_ID").Direction = ParameterDirection.Output
                        cmd.ExecuteNonQuery()

                        _ISA_ID = Convert.ToInt32(cmd.Parameters("@ISA_ID").Value.ToString())
                    End Using
                    Con.Close()
                End Using
                r = 0
                ' ClearISA()

            Catch ex As Exception
                r = -1
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _SP_ISA, ex)
            End Try

            Return r

        End Function





        Private Function ComitGS() As Integer


            _FUNCTION_NAME = "ComitGS()"
            Dim r As Integer = -1


            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_GS, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_270_GS", GS)

                        cmd.Parameters.Add("@GS_ID", Data.SqlDbType.BigInt, 1)
                        cmd.Parameters("@GS_ID").Direction = ParameterDirection.Output

                        cmd.ExecuteNonQuery()

                        _GS_ID = Convert.ToInt32(cmd.Parameters("@GS_ID").Value.ToString())


                    End Using
                    Con.Close()
                End Using
                '   ClearGS()
                r = 0
            Catch ex As Exception
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _SP_GS, ex)
            End Try

            Return r

        End Function

        Private Function ComitST() As Integer

            _FUNCTION_NAME = "ComitST()"
            Dim r As Integer = -1

            Try



                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_ST, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_270_ST", ST)
                        cmd.Parameters.AddWithValue("@HIPAA_270_BHT", BHT)
                        cmd.Parameters.Add("@ST_ID", Data.SqlDbType.BigInt, 1)
                        cmd.Parameters("@ST_ID").Direction = ParameterDirection.Output
                        cmd.ExecuteNonQuery()

                        _ST_ID = Convert.ToInt32(cmd.Parameters("@ST_ID").Value.ToString())
                    End Using
                    Con.Close()
                End Using
                r = 0
                '    ClearST()
            Catch ex As Exception

                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _SP_COMIT_2000A_DATA, ex)
            End Try

            Return r

        End Function



        Public Function COMIT2000AHeaderDump() As Integer

            Dim r As Integer = -1

            _FUNCTION_NAME = "COMIT2000AHeaderDump()"


            Try


                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_COMIT_2000A_DATA, Con)

                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.AddWithValue("@HIPAA_270_HL", ISL_HL)
                        cmd.Parameters.AddWithValue("@HIPAA_270_NM1", ISL_NM1)

                        cmd.Parameters.AddWithValue("@RAW_HEADER", _RAW_HEADER)
                        cmd.Parameters.AddWithValue("@RAW_ISL", _270_ISL_STRING)


                        cmd.ExecuteNonQuery()


                    End Using
                    Con.Close()
                End Using
                r = 0

            Catch ex As Exception
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _SP_COMIT_2000A_DATA, ex)

            End Try





            Return r



        End Function



        Public Function COMIT2000BHeaderDump() As Integer

            Dim r As Integer = -1

            _FUNCTION_NAME = "COMIT2000BHeaderDump()"


            Try


                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_COMIT_2000B_DATA, Con)

                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.AddWithValue("@HIPAA_270_HL", IRL_HL)


                        cmd.Parameters.AddWithValue("@HIPAA_270_NM1", IRL_NM1)
                        cmd.Parameters.AddWithValue("@HIPAA_270_N3", IRL_N3)
                        cmd.Parameters.AddWithValue("@HIPAA_270_N4", IRL_N4)

                        cmd.Parameters.AddWithValue("@HIPAA_270_REF", IRL_REF)

                        cmd.Parameters.AddWithValue("@HIPAA_270_PRV", IRL_PRV)



                        cmd.Parameters.AddWithValue("@RAW_HEADER", _RAW_HEADER)
                        cmd.Parameters.AddWithValue("@RAW_ISL", _270_ISL_STRING)
                        cmd.Parameters.AddWithValue("@RAW_IRL", _270_IRL_STRING)

                        cmd.ExecuteNonQuery()



                    End Using
                    Con.Close()
                End Using
                r = 0

            Catch ex As Exception
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _SP_COMIT_2000B_DATA, ex)

            End Try





            Return r



        End Function

        Public Function COMIT_270_22_Dump() As Integer

            Dim r As Integer = -1

            _FUNCTION_NAME = "COMIT_270_22_Dump()"


            Try


                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_COMIT_HL22_DATA, Con)


                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.AddWithValue("@HIPAA_270_HL", HL)
                        cmd.Parameters.AddWithValue("@HIPAA_270_DMG", DMG)
                        cmd.Parameters.AddWithValue("@HIPAA_270_DTP", DTP)
                        cmd.Parameters.AddWithValue("@HIPAA_270_EQ", EQ)
                        cmd.Parameters.AddWithValue("@HIPAA_270_III", III)
                        cmd.Parameters.AddWithValue("@HIPAA_270_INS", INS)
                        cmd.Parameters.AddWithValue("@HIPAA_270_HI", HI)
                        cmd.Parameters.AddWithValue("@HIPAA_270_NM1", NM1)
                        cmd.Parameters.AddWithValue("@HIPAA_270_N3", N3)
                        cmd.Parameters.AddWithValue("@HIPAA_270_N4", N4)
                        cmd.Parameters.AddWithValue("@HIPAA_270_PRV", PRV)
                        cmd.Parameters.AddWithValue("@HIPAA_270_REF", REF)
                        cmd.Parameters.AddWithValue("@HIPAA_270_TRN", TRN)


                        cmd.Parameters.AddWithValue("@RAW_HEADER", _RAW_HEADER)
                        cmd.Parameters.AddWithValue("@RAW_ISL", _270_ISL_STRING)
                        cmd.Parameters.AddWithValue("@RAW_IRL", _270_IRL_STRING)
                        cmd.Parameters.AddWithValue("@RAW_SL", _270_SL_STRING)
                        cmd.Parameters.AddWithValue("@RAW_DL", _270_DL_STRING)



                        cmd.Parameters.AddWithValue("@HIPAA_270_ISA", ISA)
                        cmd.Parameters.AddWithValue("@HIPAA_270_GS", GS)
                        cmd.Parameters.AddWithValue("@HIPAA_270_ST", ST)
                        cmd.Parameters.AddWithValue("@HIPAA_270_BHT", BHT)


                        cmd.Parameters.AddWithValue("@HIPAA_270_NM1_ISL", ISL_NM1)



                        cmd.Parameters.AddWithValue("@HIPAA_270_NM1_IRL", IRL_NM1)
                        cmd.Parameters.AddWithValue("@HIPAA_270_N3_IRL", IRL_N3)
                        cmd.Parameters.AddWithValue("@HIPAA_270_N4_IRL", IRL_N4)
                        cmd.Parameters.AddWithValue("@HIPAA_270_REF_IRL", IRL_REF)
                        cmd.Parameters.AddWithValue("@HIPAA_270_PRV_IRL", IRL_PRV)




                        cmd.Parameters.AddWithValue("@request_xml", _RAW_EDI_EX)
                        cmd.Parameters.AddWithValue("@DELETE_FLAG", _DELETE_FLAG)
                        cmd.Parameters.AddWithValue("@ebr_id", _EBR_ID)
                        cmd.Parameters.AddWithValue("@user_id", _USER_ID)
                        cmd.Parameters.AddWithValue("@hosp_code", _HOSP_CODE)
                        cmd.Parameters.AddWithValue("@source", _SOURCE)
                        cmd.Parameters.AddWithValue("@pat_hosp_code", _PAT_HOSP_CODE)
                        cmd.Parameters.AddWithValue("@Vendor_name", _VENDOR_NAME)
                        cmd.Parameters.AddWithValue("@ins_type", _INS_TYPE)
                        cmd.Parameters.AddWithValue("@Payor_id", _PAYOR_ID)
                        cmd.Parameters.AddWithValue("@Patient_number", _PATIENT_NUMBER)
                        cmd.Parameters.AddWithValue("@C_SEARCH_TYPE", _C_SEARCH_TYPE)

                        cmd.Parameters.Add("@batch_id", Data.SqlDbType.BigInt)
                        cmd.Parameters("@batch_id").Direction = ParameterDirection.Output




                        '@request_xml varchar(max) ='',  





                        '	@batch_id bigint=0 OUT,
                        cmd.ExecuteNonQuery()

                        _BATCH_ID = Convert.ToInt64(cmd.Parameters("@batch_id").Value)

                    End Using
                    Con.Close()
                End Using


                r = 0

            Catch ex As Exception
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _SP_COMIT_2000B_DATA, ex)

            End Try





            Return r



        End Function



        Private Function ComitSE() As Int32
            Dim r As Integer

            Try
                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_SE, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_270_SE", SE_N)
                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using


            Catch ex As Exception
                log.ExceptionDetails("COMIT_SE_DATA 005010X221A1", ex)
            End Try

            Return r

        End Function






        Private Function ComitGE() As Integer
            Dim i As Integer

            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_GE, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_270_GE", GE_N)

                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using

            Catch ex As Exception
                log.ExceptionDetails("COMIT_GE_DATA 005010X221A1", ex)
            End Try

            Return i

        End Function







        Private Function ComitIEA() As Integer


            Dim i As Integer

            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_IEA, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_270_IEA", IEA_N)
                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using

                i = 0

            Catch ex As Exception
                log.ExceptionDetails("COMIT_IEA_DATA 005010X221A1", ex)
            End Try

            Return i

        End Function

        Private Sub ClearISA()
            ISA.Clear()
            IEA.Clear()
            _HIPAA_ISA_GUID = Guid.Empty
            _ISA_ID = 0


            _HIPAA_HL_20_GUID = Guid.Empty
            _HIPAA_HL_21_GUID = Guid.Empty
            _HIPAA_HL_22_GUID = Guid.Empty
            _HIPAA_HL_23_GUID = Guid.Empty
            _HIPAA_HL_24_GUID = Guid.Empty


            _HL01 = 0
            _HL02 = 0
            _HL03 = 0
            _HL04 = 0

            _LoopLevelMajor = 1000
            _LoopLevelMinor = 0
            _LoopLevelSubFix = ""

            ClearGS()
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

            _RAW_HEADER = String.Empty

        End Sub

        Private Sub Clear2000A()

            ISL_HL.Clear()
            ISL_NM1.Clear()
            _270_ISL_STRING = String.Empty

            Clear2000B()

        End Sub


        Private Sub Clear2000B()

            IRL_HL.Clear()
            IRL_NM1.Clear()
            IRL_N3.Clear()
            IRL_N4.Clear()

            IRL_REF.Clear()
            IRL_PRV.Clear()


            _270_IRL_STRING = String.Empty

            Clear22Dump()

        End Sub

        Private Sub Clear22Dump()

            HL.Clear()
            DMG.Clear()
            DTP.Clear()
            EQ.Clear()
            III.Clear()
            INS.Clear()
            HI.Clear()
            NM1.Clear()
            N3.Clear()
            N4.Clear()
            PRV.Clear()
            REF.Clear()
            TRN.Clear()

            _270_SL_STRING = String.Empty
            _270_DL_STRING = String.Empty

        End Sub

        Private Function ComitUNK() As Integer


            Dim i As Integer

            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_COMIT_UNKNOWN, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_5010_UNK", UNK)
                        cmd.Parameters.AddWithValue("@IMPORTER", "EDI_5010_270005010X221A1")
                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using

                i = 0

            Catch ex As Exception
                _hasERR = True
                _ERROR = ex.Message
                log.ExceptionDetails("COMIT_UNK_DATA 005010X221A1", ex)
            End Try

            Return i

        End Function

        Private Function RollBack() As Integer

            Dim i As Integer

            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_ROLLBACK, Con)

                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.AddWithValue("@FILE_ID", _FILE_ID)
                        '           cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using

                i = 0

            Catch ex As Exception
                log.ExceptionDetails("ROLL_BACK 005010X221A1", ex)
            End Try

            Return i



        End Function

        Public Function NM1Lookup(ByVal nm01 As String) As String
            Dim s As String = String.Empty

            Select Case nm01

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



        Public Function TestSPs() As Integer

            Dim r As Integer = 0

            _FUNCTION_NAME = "TestSPs()"


            Try


                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_COMIT_2000A_UNIT_TEST, Con)

                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.AddWithValue("@HIPAA_270_HL", ISL_HL)
                        cmd.Parameters.AddWithValue("@HIPAA_270_NM1", ISL_NM1)



                        cmd.ExecuteNonQuery()


                    End Using
                    Con.Close()
                End Using


            Catch ex As Exception
                r = -1
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _SP_COMIT_2000A_UNIT_TEST, ex)

            End Try



            Try


                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_COMIT_2000B_UNIT_TEST, Con)

                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.AddWithValue("@HIPAA_270_HL", IRL_HL)

                        cmd.Parameters.AddWithValue("@HIPAA_270_NM1", IRL_NM1)
                        cmd.Parameters.AddWithValue("@HIPAA_270_N3", IRL_N3)
                        cmd.Parameters.AddWithValue("@HIPAA_270_N4", IRL_N4)

                        cmd.Parameters.AddWithValue("@HIPAA_270_REF", IRL_REF)
                        cmd.Parameters.AddWithValue("@HIPAA_270_PRV", IRL_PRV)

                        cmd.ExecuteNonQuery()


                    End Using
                    Con.Close()
                End Using


            Catch ex As Exception
                r = -1
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _SP_COMIT_2000B_UNIT_TEST, ex)

            End Try


            Try


                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_COMIT_HL22_UNIT_TEST, Con)

                        cmd.CommandType = CommandType.StoredProcedure


                        cmd.Parameters.AddWithValue("@HIPAA_270_HL", HL)
                        cmd.Parameters.AddWithValue("@HIPAA_270_DMG", DMG)
                        cmd.Parameters.AddWithValue("@HIPAA_270_DTP", DTP)
                        cmd.Parameters.AddWithValue("@HIPAA_270_EQ", EQ)
                        cmd.Parameters.AddWithValue("@HIPAA_270_III", III)
                        cmd.Parameters.AddWithValue("@HIPAA_270_INS", INS)
                        cmd.Parameters.AddWithValue("@HIPAA_270_HI", HI)
                        cmd.Parameters.AddWithValue("@HIPAA_270_NM1", NM1)
                        cmd.Parameters.AddWithValue("@HIPAA_270_N3", N3)
                        cmd.Parameters.AddWithValue("@HIPAA_270_N4", N4)
                        cmd.Parameters.AddWithValue("@HIPAA_270_PRV", PRV)
                        cmd.Parameters.AddWithValue("@HIPAA_270_REF", REF)
                        cmd.Parameters.AddWithValue("@HIPAA_270_TRN", TRN)

                        cmd.ExecuteNonQuery()


                    End Using
                    Con.Close()
                End Using


            Catch ex As Exception
                r = -1
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _SP_COMIT_HL22_UNIT_TEST, ex)

            End Try



            Return r



        End Function








        'Public Function ComitEPIC() As Integer

        '    Dim i As Integer

        '    If _DEAD_LOCK_COUNT < _DEAD_LOCK_RETRYS Then

        '        Dim param As New SqlParameter()
        '        Dim sqlConn As SqlConnection = New SqlConnection
        '        Dim cmd As SqlCommand
        '        Dim sqlString As String = String.Empty

        '        sqlConn.ConnectionString = _ConnectionString
        '        sqlConn.Open()

        '        Try





        '            sqlString = "usp_eligibility_response_dump_271_epic"




        '            cmd = New SqlCommand(sqlString, sqlConn)
        '            cmd.CommandType = CommandType.StoredProcedure
        '            cmd.CommandTimeout = _CommandTimeOut
        '            '   cmd.Parameters.AddWithValue("@HIPPA_ENVELOP", ENVELOP)
        '            cmd.Parameters.AddWithValue("@HIPAA_EQ", EQ)
        '            '  cmd.Parameters.AddWithValue("@HIPAA_AAA", AAA)
        '            cmd.Parameters.AddWithValue("@HIPAA_AMT", AMT)
        '            cmd.Parameters.AddWithValue("@HIPAA_DMG", DMG)
        '            cmd.Parameters.AddWithValue("@HIPAA_BHT", BHT)
        '            cmd.Parameters.AddWithValue("@HIPAA_DTP", DTP)
        '            '  cmd.Parameters.AddWithValue("@HIPAA_EQ", EQ)
        '            ' cmd.Parameters.AddWithValue("@HIPAA_HSD", HSD)
        '            cmd.Parameters.AddWithValue("@HIPAA_III", III)
        '            cmd.Parameters.AddWithValue("@HIPAA_INS", INS)
        '            cmd.Parameters.AddWithValue("@HIPAA_N3", N3)
        '            cmd.Parameters.AddWithValue("@HIPAA_N4", N4)
        '            'cmd.Parameters.AddWithValue("@HIPAA_MSG", MSG)
        '            cmd.Parameters.AddWithValue("@HIPAA_NM1", NM1)
        '            '   cmd.Parameters.AddWithValue("@HIPAA_PER", PER)
        '            'cmd.Parameters.AddWithValue("@HIPAA_PRV", PRV)
        '            cmd.Parameters.AddWithValue("@HIPAA_REF", REF)
        '            cmd.Parameters.AddWithValue("@HIPAA_TRN", TRN)
        '            cmd.Parameters.AddWithValue("@HIPAA_UNK", UNK)
        '            '  cmd.Parameters.AddWithValue("@EQr_id", _EQR_ID)
        '            cmd.Parameters.AddWithValue("@batch_id", _BATCH_ID)
        '            cmd.Parameters.AddWithValue("@user_id", _USER_ID)
        '            cmd.Parameters.AddWithValue("@hosp_code", _HOSP_CODE)
        '            cmd.Parameters.AddWithValue("@source", _SOURCE)
        '            '   cmd.Parameters.AddWithValue("@EDI", "EQr_ID=" + _EQR_ID + "|" + "batch_ID=" + _BATCH_ID + "|" + _edi)
        '            cmd.Parameters.AddWithValue("@PAYOR_ID", _PAYOR_ID)
        '            cmd.Parameters.AddWithValue("@Vendor_name", _VENDOR_NAME)
        '            cmd.Parameters.AddWithValue("@Log_EDI", _LOG_EDI)
        '            cmd.Parameters.Add("@Status", Data.SqlDbType.VarChar, 20)
        '            cmd.Parameters("@Status").Direction = ParameterDirection.Output
        '            cmd.Parameters.Add("@Reject_Reason_code", Data.SqlDbType.VarChar, 10)
        '            cmd.Parameters("@Reject_Reason_code").Direction = ParameterDirection.Output
        '            cmd.Parameters.Add("@LOOP_AGAIN", Data.SqlDbType.VarChar, 1)
        '            cmd.Parameters("@LOOP_AGAIN").Direction = ParameterDirection.Output

        '            If _DEAD_LOCK_FLAG Then

        '                cmd.Parameters.AddWithValue("@DELETE_FLAG", "Y")
        '            Else
        '                cmd.Parameters.AddWithValue("@DELETE_FLAG", _DELETE_FLAG)
        '            End If


        '            'If (_isEPIC = True) Then
        '            '    Then

        '            '    cmd.Parameters.Add("@EPICOutEDIString", Data.SqlDbType.VarChar)
        '            '    cmd.Parameters("@EPICOutEDIString").Direction = ParameterDirection.Output

        '            'End If


        '            '   _ERROR = cmd.ExecuteNonQuery()


        '            _EPICOutEDIString = cmd.Parameters("@EPICOutEDIString").Value.ToString()



        '            _STATUS = cmd.Parameters("@Status").Value.ToString()
        '            _REJECT_REASON_CODE = cmd.Parameters("@Reject_Reason_code").Value.ToString()
        '            _LOOP_AGAIN = cmd.Parameters("@LOOP_AGAIN").Value.ToString()

        '            i = 0

        '        Catch sx As SqlException
        '            '   log.ExceptionDetails("55-" + _Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import271" + Me.ToString, sx)

        '            log.ExceptionDetails("56-Parse.Import271", "Save to db failed Parse sucessful for batchID : " + Convert.ToString(_BATCH_ID), _RAW_EDI, Me.ToString)

        '            If sx.Message.Contains("deadlocked") Or sx.Message.Contains("timeout") Then
        '                log.ExceptionDetails("57-Parse.Import271", "Dead lock rtrying  " + Convert.ToString(_BATCH_ID) + " Deadlock count  " + Convert.ToString(_DEAD_LOCK_COUNT), _RAW_EDI, Me.ToString)
        '                _DEAD_LOCK_FLAG = True
        '                _DEAD_LOCK_COUNT = _DEAD_LOCK_COUNT + 1
        '                Comit()
        '                i = -1
        '            End If

        '        Catch ex As Exception
        '            i = -1

        '            '   log.ExceptionDetails("58-" + _Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import271 " + Convert.ToString(_BATCH_ID), ex)
        '            '

        '            '% was deadlocked %' in SQL.Exception. IF Found RERUN with DELETE_FLAG='Y' only DUMP method
        '        Finally

        '            sqlConn.Close()
        '        End Try




        '    Else



        '        log.ExceptionDetails("59-Parse.Import271", "Dead lock count execced giving up on   " + Convert.ToString(_BATCH_ID) + " Deadlock count  " + Convert.ToString(_DEAD_LOCK_COUNT), _RAW_EDI, Me.ToString)
        '        i = -1

        '    End If


        '    Return i

        'End Function


        'Public Function Comit() As Integer

        '    Dim i As Integer = -1

        '    Dim LoopCount As Integer = 0







        '    If _DEAD_LOCK_COUNT < _DEAD_LOCK_RETRYS Then
        '        LoopCount = LoopCount + 1
        '        If _VERBOSE = 1 Then
        '            log.ExceptionDetails("49-271 loop count ", "Batch ID  " + Convert.ToString(_BATCH_ID) + " loop count   " + Convert.ToString(LoopCount))
        '        End If


        '        Dim param As New SqlParameter()
        '        Dim sqlConn As SqlConnection = New SqlConnection
        '        Dim cmd As SqlCommand
        '        Dim sqlString As String = String.Empty

        '        sqlConn.ConnectionString = _ConnectionString
        '        sqlConn.Open()

        '        '

        '        Try

        '            sqlString = "usp_eligibility_response_dump"





        '            cmd = New SqlCommand(sqlString, sqlConn)
        '            cmd.CommandType = CommandType.StoredProcedure
        '            '  cmd.Parameters.AddWithValue("@HIPPA_ENVELOP", ENVELOP)
        '            cmd.Parameters.AddWithValue("@HIPAA_EQ", EQ)
        '            '   cmd.Parameters.AddWithValue("@HIPAA_AAA", AAA)
        '            '    cmd.Parameters.AddWithValue("@HIPAA_AMT", AMT)
        '            cmd.Parameters.AddWithValue("@HIPAA_DMG", DMG)
        '            cmd.Parameters.AddWithValue("@HIPAA_BHT", BHT)
        '            cmd.Parameters.AddWithValue("@HIPAA_DTP", DTP)
        '            '   cmd.Parameters.AddWithValue("@HIPAA_EQ", EQ)
        '            '  cmd.Parameters.AddWithValue("@HIPAA_HSD", HSD)
        '            cmd.Parameters.AddWithValue("@HIPAA_III", III)
        '            cmd.Parameters.AddWithValue("@HIPAA_INS", INS)
        '            cmd.Parameters.AddWithValue("@HIPAA_N3", N3)
        '            cmd.Parameters.AddWithValue("@HIPAA_N4", N4)
        '            ' cmd.Parameters.AddWithValue("@HIPAA_MSG", MSG)
        '            cmd.Parameters.AddWithValue("@HIPAA_NM1", NM1)
        '            'cmd.Parameters.AddWithValue("@HIPAA_PER", PER)
        '            cmd.Parameters.AddWithValue("@HIPAA_PRV", PRV)
        '            cmd.Parameters.AddWithValue("@HIPAA_REF", REF)
        '            cmd.Parameters.AddWithValue("@HIPAA_TRN", TRN)
        '            cmd.Parameters.AddWithValue("@HIPAA_UNK", UNK)
        '            '      cmd.Parameters.AddWithValue("@EQr_id", _EQR_ID)
        '            cmd.Parameters.AddWithValue("@batch_id", _BATCH_ID)
        '            cmd.Parameters.AddWithValue("@user_id", _USER_ID)
        '            cmd.Parameters.AddWithValue("@hosp_code", _HOSP_CODE)
        '            cmd.Parameters.AddWithValue("@source", _SOURCE)
        '            cmd.Parameters.AddWithValue("@EDI", _RAW_EDI)
        '            cmd.Parameters.AddWithValue("@PAYOR_ID", _PAYOR_ID)
        '            cmd.Parameters.AddWithValue("@Vendor_name", _VENDOR_NAME)
        '            cmd.Parameters.AddWithValue("@Log_EDI", _LOG_EDI)
        '            cmd.Parameters.Add("@Status", Data.SqlDbType.VarChar, 20)
        '            cmd.Parameters("@Status").Direction = ParameterDirection.Output
        '            cmd.Parameters.Add("@Reject_Reason_code", Data.SqlDbType.VarChar, 10)
        '            cmd.Parameters("@Reject_Reason_code").Direction = ParameterDirection.Output
        '            cmd.Parameters.Add("@LOOP_AGAIN", Data.SqlDbType.VarChar, 1)
        '            cmd.Parameters("@LOOP_AGAIN").Direction = ParameterDirection.Output

        '            If _DEAD_LOCK_FLAG Then
        '                cmd.Parameters.AddWithValue("@DELETE_FLAG", "Y")
        '            Else
        '                cmd.Parameters.AddWithValue("@DELETE_FLAG", _DELETE_FLAG)
        '            End If


        '            'If (_isEPIC = True) Then
        '            '    Then

        '            '    cmd.Parameters.Add("@EPICOutEDIString", Data.SqlDbType.VarChar)
        '            '    cmd.Parameters("@EPICOutEDIString").Direction = ParameterDirection.Output

        '            'End If


        '            ''   err = cmd.ExecuteNonQuery()



        '            '  log.ExceptionDetails("64-Parse.Import271", " Begin Comit" + Convert.ToString(_EQR_ID))
        '            _ERROR = Convert.ToString(cmd.ExecuteNonQuery())
        '            '  log.ExceptionDetails("65-Parse.Import271", " end Comit " + Convert.ToString(_EQR_ID) + " " + err)


        '            _STATUS = cmd.Parameters("@Status").Value.ToString()
        '            _REJECT_REASON_CODE = cmd.Parameters("@Reject_Reason_code").Value.ToString()
        '            _LOOP_AGAIN = cmd.Parameters("@LOOP_AGAIN").Value.ToString()

        '            i = 0

        '        Catch sx As SqlException
        '            '  log.ExceptionDetails("50-" + _Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import271" + Me.ToString, sx)

        '            log.ExceptionDetails("51-Parse.Import271", "Save to db failed Parse sucessful for batchID : " + Convert.ToString(_BATCH_ID), _RAW_EDI, Me.ToString)

        '            If sx.Message.Contains("deadlocked") Or sx.Message.Contains("timeout") Then
        '                log.ExceptionDetails("52-Parse.Import271", "Dead lock rtrying  " + Convert.ToString(_BATCH_ID) + " Deadlock count  " + Convert.ToString(_DEAD_LOCK_COUNT), _RAW_EDI, Me.ToString)
        '                _DEAD_LOCK_FLAG = True
        '                _DEAD_LOCK_COUNT = _DEAD_LOCK_COUNT + 1
        '                Comit()  'Commented by Mohan - as per Suresh/Manoj - 09152016 
        '                i = -1
        '            End If

        '        Catch ex As Exception
        '            i = -1

        '            '  log.ExceptionDetails("53-" + _Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import271 " + Convert.ToString(_BATCH_ID), ex)
        '            '

        '            '% was deadlocked %' in SQL.Exception. IF Found RERUN with DELETE_FLAG='Y' only DUMP method
        '        Finally

        '            sqlConn.Close()
        '        End Try




        '    Else



        '        log.ExceptionDetails("54-Parse.Import271", "Dead lock count execced giving up on   " + Convert.ToString(_BATCH_ID) + " Deadlock count  " + Convert.ToString(_DEAD_LOCK_COUNT), _RAW_EDI, Me.ToString)
        '        i = -1

        '    End If


        '    Return i




        'End Function











        Public WriteOnly Property NPI As String

            Set(value As String)
                _NPI = value
            End Set
        End Property

        Public WriteOnly Property ServiceTypeCode As String

            Set(value As String)
                _SERVICE_TYPE_CODE = value
            End Set
        End Property

        Public Property BatchID As Double

            Get
                Return (_BATCH_ID)
            End Get
            Set(value As Double)
                _BATCH_ID = CLng(value)

            End Set
        End Property

        Public WriteOnly Property EDI As String

            Set(value As String)
                _RAW_EDI = value
            End Set
        End Property


        Public WriteOnly Property RAW270 As String

            Set(value As String)
                _RAW_270 = value
            End Set
        End Property


        Public WriteOnly Property DeadlockRetrys As Integer

            Set(value As Integer)
                _DEAD_LOCK_RETRYS = value

            End Set
        End Property





        Public WriteOnly Property Commit As Integer

            Set(value As Integer)
                _COMMIT = value
            End Set
        End Property





        Public WriteOnly Property EBR_ID As Double


            Set(value As Double)
                _EBR_ID = CLng(value)
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

        Public WriteOnly Property pat_hosp_code As String

            Set(value As String)
                _PAT_HOSP_CODE = value
            End Set
        End Property

        Public WriteOnly Property ins_type As String

            Set(value As String)
                _INS_TYPE = value
            End Set
        End Property

        Public WriteOnly Property source As String

            Set(value As String)
                _SOURCE = value
            End Set
        End Property


        Public WriteOnly Property Patient_number As String

            Set(value As String)
                _PATIENT_NUMBER = value
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



        Public Property EQLoopCount As Integer
            Get
                Return mmm
            End Get
            Set(value As Integer)

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




    End Class
End Namespace
