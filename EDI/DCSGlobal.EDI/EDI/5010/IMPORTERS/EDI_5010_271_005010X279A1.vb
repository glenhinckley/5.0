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

    Public Class EDI_271_005010X279A1

        Inherits EDI_5010_271_005010X279A1_TABLES

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

        Private _ConnectionString As String = String.Empty
        Private _CommandTimeOut As Integer = 90

        Private _DocumentType As String = "005010X279A1"


        Private _SP_ISA As String = "[usp_EDI_5010_HIPAA_271_DUMP_ISA]"
        Private _SP_IEA As String = "[usp_EDI_5010_HIPAA_271_DUMP_IEA]"

        Private _SP_GS As String = "[usp_EDI_5010_HIPAA_271_DUMP_GS]"
        Private _SP_GE As String = "[usp_EDI_5010_HIPAA_271_DUMP_GE]"

        Private _SP_ST As String = "[usp_EDI_5010_HIPAA_271_DUMP_ST]"
        Private _SP_SE As String = "[usp_EDI_5010_HIPAA_271_DUMP_SE]"




        Private _SP_COMIT_2000A_UNIT_TEST As String = "[usp_EDI_5010_HIPAA_271_DUMP_UNIT_TEST_2000A_HEADER]"
        Private _SP_COMIT_2000B_UNIT_TEST As String = "[usp_EDI_5010_HIPAA_271_DUMP_UNIT_TEST_2000B_HEADER]"
        Private _SP_COMIT_HL22_UNIT_TEST As String = "[usp_EDI_5010_HIPAA_271_DUMP_UNIT_TEST_22]"


        Private _SP_COMIT_2000A_DATA As String = "[usp_EDI_5010_HIPAA_271_DUMP_2000A_HEADER]"
        Private _SP_COMIT_2000B_DATA As String = "[usp_EDI_5010_HIPAA_271_DUMP_2000B_HEADER]"
        Private _SP_COMIT_HL22_DATA As String = "[usp_EDI_5010_HIPAA_271_DUMP_22]"



        Private _SP_COMIT_UNKNOWN As String = "[usp_HIPAA_EDI_UNKNOWN]"

        Private _SP_ROLLBACK As String = "[usp_EDI_5010_HIPAA_271_ROLLBACK]"


        Private _EBIList As New List(Of String)
        Private _LAST_EB As String = String.Empty
        Private _CURRENT_EB As String = String.Empty

        Private _271_ISL_GUID As Guid = Guid.Empty
        Private _271_IRL_GUID As Guid = Guid.Empty
        Private _271_SL_GUID As Guid = Guid.Empty
        Private _271_DL_GUID As Guid = Guid.Empty


        Private _271_EB_GUID As Guid = Guid.Empty
        Private _271_EB_GROUP_GUID As Guid = Guid.Empty
        Private _271_LS_GUID As Guid = Guid.Empty


        Private _2000A_DIRTY As Boolean = False
        Private _2000B_DIRTY As Boolean = False
        Private _2000C_DIRTY As Boolean = True
        Private _EB_DIRTY As Boolean = False
        Private _ST_DIRTY As Boolean = True


        Private _271_LOOP_LEVEL As String = "HEADER"



        Private _EPICOutEDIString As String = String.Empty
        Private _isEPIC As Boolean = False

        Private mmm As Integer = 1



        Private _271_ISL_STRING As String = String.Empty
        Private _271_IRL_STRING As String = String.Empty
        Private _271_SL_STRING As String = String.Empty
        Private _271_DL_STRING As String = String.Empty
        Private _271_EB_STRING As String = String.Empty



        Private _PEB As String = String.Empty
        Private _FIRST_EB_FOUND As Boolean = False
        Private _IN_EB As Boolean = False




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

        Public WriteOnly Property CONSOLE_NAME As String

            Set(value As String)
                _CONSOLE_NAME = value

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


            _IS_STRING = True

            Dim r As Integer = -1

            _RAW_EDI = EDI


            Dim SP As New StringPrep()
            _BATCH_ID = CLng(BatchID)

            _EDIList = SP.SingleEDI(_RAW_EDI)

            r = Import(_EDIList)



            Return r

        End Function

        Public Function Import(ByVal EDIList As List(Of String), ByVal BatchID As Double) As Int32

            Dim x As Integer = -1

            _BATCH_ID = Convert.ToInt64(BatchID)

            x = Import(EDIList)

            Return x

        End Function

        Public Function Import(ByVal EDIList As List(Of String)) As Int32

            Dim last As String = String.Empty
            Dim rowcount As Int32 = 0


            _ProcessStartTime = Now
            Dim _ImportReturnCode As Integer = 0


            If _TablesBuilt = False Then
                BuildTables()
                _TablesBuilt = True
                ClearISA()
            End If

            'so lets get down to it opn the file in a stream reader and move thru it one line at a time...........


            _HL20_DIRTY = True


            _SP_RETURN_CODE = TestSPs()

            If (_SP_RETURN_CODE <> 0) Then
                _IMPORT_RETURN_STRING = "271 : SP TEST FAILED WITH ERROR CODE " + Convert.ToString(_SP_RETURN_CODE)
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _IMPORT_RETURN_STRING)
                Return -1
                Exit Function
            End If


            Using e As New EDI_5010_LOGGING
                e.ConnectionString = _ConnectionString
                e.TransactionSetIdentifierCode = "271"
                e.UpdateFileStatus(CInt(_FILE_ID), "271", "BEGIN PARSE")
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
                rowcount = rowcount + 1





                _RowRecordType = ss.ParseDemlimtedStringEDI(line, _DataElementSeparator, 1)
                'Console.WriteLine(_RowRecordType)
                _CurrentRowData = line



                If _VERBOSE = 1 Then

                    _DEBUG_STRING = ""
                    _DEBUG_STRING = Convert.ToString(rowcount) + "::" + _RowRecordType + "::" + _CurrentRowData

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



                        '_ISA_ROW_ID = rowcount
                        ISARow("ROW_NUMBER") = rowcount

                        ISA.Rows.Add(ISARow)



                        _RepetitionSeparator = _ISA11
                        _ComponentElementSeparator = _ISA16

                        _ISA = Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"


                        '  _chars = "RowDataDelimiter: " + _DataElementSeparator + " CarrotDataDelimiter: " + _CarrotDataDelimiter + " ComponentElementSeperator: " + _ISA16_ComponentElementSeparator



                        'o'D.ISAFlag = 1

                        ComitISA()


                        Select Case _271_LOOP_LEVEL
                            Case "HEADER"
                                _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
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

                        'o() 'D.GS_ROW_ID = rowcount

                        GSRow("ROW_NUMBER") = rowcount
                        GS.Rows.Add(GSRow)

                        _GS = Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"

                        ComitGS()


                        Select Case _271_LOOP_LEVEL
                            Case "HEADER"
                                _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
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



                        STRow("ROW_NUMBER") = rowcount
                        ST.Rows.Add(STRow)



                        _ST = Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"

                        Select Case _271_LOOP_LEVEL
                            Case "HEADER"
                                _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
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
                        BHTRow("ROW_NUMBER") = rowcount

                        BHT.Rows.Add(BHTRow)

                        _BHT = Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"


                        Select Case _271_LOOP_LEVEL
                            Case "HEADER"
                                _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                        End Select

                        _RowProcessedFlag = 1
                        ComitST()


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


                                _271_ISL_STRING = String.Empty
                                _271_LOOP_LEVEL = "ISL"

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

                                _271_IRL_STRING = String.Empty
                                _271_LOOP_LEVEL = "IRL"
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
                                    COMIT_271_22_Dump()
                                    '  ComitHL20()
                                    _HL22_DIRTY = False
                                End If
                                _LoopLevelSubFix = "C"
                                _HL22_DIRTY = True
                                _271_LOOP_LEVEL = "SL"
                                _271_SL_STRING = String.Empty

                            Case 23
                                _HL22_DIRTY = True
                                _271_LOOP_LEVEL = "DL"
                                _271_DL_STRING = String.Empty

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
                                HLRow("ISA_ID") = _ISA_ID
                                HLRow("GS_ID") = _GS_ID
                                HLRow("ST_ID") = _ST_ID
                                HLRow("BATCH_ID") = _BATCH_ID
                                HLRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                                HLRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                                HLRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID

                                _271_ISL_GUID = Guid.NewGuid
                                _271_IRL_GUID = Guid.Empty

                                _271_LOOP_LEVEL = "ISL"

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






                                HLRow("ROW_NUMBER") = rowcount



                                HLRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                HLRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                                HLRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor

                                ISL_HL.Rows.Add(HLRow)


                            Case 21
                                Dim HLRow As DataRow = IRL_HL.NewRow


                                HLRow("FILE_ID") = _FILE_ID
                                HLRow("ISA_ID") = _ISA_ID
                                HLRow("GS_ID") = _GS_ID
                                HLRow("ST_ID") = _ST_ID
                                HLRow("BATCH_ID") = _BATCH_ID
                                HLRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                                HLRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                                HLRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID


                                ' _271_ISL_GUID = Guid.Empty
                                _271_IRL_GUID = Guid.NewGuid

                                _271_LOOP_LEVEL = "IRL"

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






                                HLRow("ROW_NUMBER") = rowcount



                                HLRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                HLRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                                HLRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor

                                IRL_HL.Rows.Add(HLRow)



                            Case 22
                                Dim HLRow As DataRow = HL.NewRow

                                HLRow("FILE_ID") = _FILE_ID
                                HLRow("ISA_ID") = _ISA_ID
                                HLRow("GS_ID") = _GS_ID
                                HLRow("ST_ID") = _ST_ID
                                HLRow("BATCH_ID") = _BATCH_ID
                                HLRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                                HLRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                                HLRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID



                                _271_LOOP_LEVEL = "SL"

                                '  _271_ISL_GUID = Guid.Empty
                                '  _271_IRL_GUID = Guid.Empty
                                _271_SL_GUID = Guid.NewGuid
                                _271_DL_GUID = Guid.Empty
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





                                HLRow("ROW_NUMBER") = rowcount



                                HLRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                HLRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                                HLRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor

                                HL.Rows.Add(HLRow)


                            Case 23
                                Dim HLRow As DataRow = HL.NewRow

                                HLRow("FILE_ID") = _FILE_ID
                                HLRow("ISA_ID") = _ISA_ID
                                HLRow("GS_ID") = _GS_ID
                                HLRow("ST_ID") = _ST_ID
                                HLRow("BATCH_ID") = _BATCH_ID
                                HLRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                                HLRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                                HLRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID

                                _NM1_GUID = Guid.Empty

                                _271_LOOP_LEVEL = "DL"

                                _2000C_DIRTY = True
                                _HIPAA_HL_23_GUID = Guid.NewGuid
                                _271_DL_GUID = Guid.NewGuid




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






                                HLRow("ROW_NUMBER") = rowcount



                                HLRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                HLRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                                HLRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor

                                HL.Rows.Add(HLRow)



                        End Select



                        Select Case _271_LOOP_LEVEL
                            Case "HEADER"
                                _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            Case "ISL"
                                _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            Case "IRL"
                                _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            Case "SL"
                                _271_SL_STRING = _271_SL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            Case "DL"
                                _271_DL_STRING = _271_DL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                        End Select


                    End If

                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                End Try





                '==========================================================================================================================================================
                ' BEGIN ISL BLOCK
                If _271_LOOP_LEVEL = "ISL" Then





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
                            NM1Row("BATCH_ID") = _BATCH_ID
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
                            NM1Row("271_ISL_GUID") = _271_ISL_GUID
                            NM1Row("271_IRL_GUID") = _271_IRL_GUID
                            NM1Row("271_SL_GUID") = _271_SL_GUID
                            NM1Row("271_DL_GUID") = _271_DL_GUID
                            NM1Row("271_EB_GUID") = _271_EB_GUID
                            NM1Row("271_EB_GROUP_GUID") = _271_EB_GROUP_GUID
                            NM1Row("271_LS_GUID") = _271_LS_GUID
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

                            NM1Row("ROW_NUMBER") = rowcount
                            NM1Row("EB_ROW_NUMBER") = 0


                            NM1Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            NM1Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                            NM1Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor



                            ISL_NM1.Rows.Add(NM1Row)

                            Select Case _271_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "ISL"
                                    _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "IRL"
                                    _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "SL"
                                    _271_SL_STRING = _271_SL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "DL"
                                    _271_DL_STRING = _271_DL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            End Select



                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                    End Try



                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   ISL :: PER
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "PER" Then

                            _RowProcessedFlag = 1

                            Dim PERRow As DataRow = ISL_PER.NewRow
                            PERRow("DOCUMENT_ID") = _DOCUMENT_ID
                            PERRow("FILE_ID") = _FILE_ID
                            PERRow("BATCH_ID") = _BATCH_ID
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
                            PERRow("271_ISL_GUID") = _271_ISL_GUID
                            PERRow("271_IRL_GUID") = _271_IRL_GUID
                            PERRow("271_SL_GUID") = _271_SL_GUID
                            PERRow("271_DL_GUID") = _271_DL_GUID
                            PERRow("271_EB_GUID") = _271_EB_GUID
                            PERRow("271_EB_GROUP_GUID") = _271_EB_GROUP_GUID
                            PERRow("271_LS_GUID") = _271_LS_GUID
                            PERRow("HL01") = _HL01
                            PERRow("HL02") = _HL02
                            PERRow("HL03") = _HL03
                            PERRow("HL04") = _HL04


                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then PERRow("PER01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else PERRow("PER01") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then PERRow("PER02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else PERRow("PER02") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then PERRow("PER03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else PERRow("PER03") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then PERRow("PER04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else PERRow("PER04") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then PERRow("PER05") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else PERRow("PER05") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then PERRow("PER06") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) Else PERRow("PER06") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then PERRow("PER07") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) Else PERRow("PER07") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) <> "") Then PERRow("PER08") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) Else PERRow("PER08") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) <> "") Then PERRow("PER09") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) Else PERRow("PER09") = DBNull.Value

                            PERRow("ROW_NUMBER") = rowcount
                            PERRow("EB_ROW_NUMBER") = 0


                            PERRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            PERRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            PERRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                            ISL_PER.Rows.Add(PERRow)

                            Select Case _271_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "ISL"
                                    _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "IRL"
                                    _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "SL"
                                    _271_SL_STRING = _271_SL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "DL"
                                    _271_DL_STRING = _271_DL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            End Select

                        End If
                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                    End Try



                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   ISL :: AAA
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "AAA" Then

                            _RowProcessedFlag = 1

                            Dim AAARow As DataRow = ISL_AAA.NewRow
                            AAARow("DOCUMENT_ID") = _DOCUMENT_ID
                            AAARow("FILE_ID") = _FILE_ID
                            AAARow("BATCH_ID") = _BATCH_ID
                            AAARow("ISA_ID") = _ISA_ID
                            AAARow("GS_ID") = _GS_ID
                            AAARow("ST_ID") = _ST_ID
                            AAARow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            AAARow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            AAARow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            AAARow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            AAARow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            AAARow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            AAARow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            AAARow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            AAARow("271_ISL_GUID") = _271_ISL_GUID
                            AAARow("271_IRL_GUID") = _271_IRL_GUID
                            AAARow("271_SL_GUID") = _271_SL_GUID
                            AAARow("271_DL_GUID") = _271_DL_GUID
                            AAARow("271_EB_GUID") = _271_EB_GUID
                            AAARow("271_EB_GROUP_GUID") = _271_EB_GROUP_GUID
                            AAARow("271_LS_GUID") = _271_LS_GUID
                            AAARow("HL01") = _HL01
                            AAARow("HL02") = _HL02
                            AAARow("HL03") = _HL03
                            AAARow("HL04") = _HL04



                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then AAARow("AAA01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else AAARow("AAA01") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then AAARow("AAA02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else AAARow("AAA02") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then AAARow("AAA03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else AAARow("AAA03") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then AAARow("AAA04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else AAARow("AAA04") = DBNull.Value

                            AAARow("ROW_NUMBER") = rowcount
                            AAARow("EB_ROW_NUMBER") = 0


                            AAARow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            AAARow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            AAARow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                            ISL_AAA.Rows.Add(AAARow)

                            Select Case _271_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "ISL"
                                    _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "IRL"
                                    _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "SL"
                                    _271_SL_STRING = _271_SL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "DL"
                                    _271_DL_STRING = _271_DL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
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
                If _271_LOOP_LEVEL = "IRL" Then




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
                            NM1Row("BATCH_ID") = _BATCH_ID
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
                            NM1Row("271_ISL_GUID") = _271_ISL_GUID
                            NM1Row("271_IRL_GUID") = _271_IRL_GUID
                            NM1Row("271_SL_GUID") = _271_SL_GUID
                            NM1Row("271_DL_GUID") = _271_DL_GUID
                            NM1Row("271_EB_GUID") = _271_EB_GUID
                            NM1Row("271_EB_GROUP_GUID") = _271_EB_GROUP_GUID
                            NM1Row("271_LS_GUID") = _271_LS_GUID
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

                            NM1Row("ROW_NUMBER") = rowcount
                            NM1Row("EB_ROW_NUMBER") = 0



                            NM1Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            NM1Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                            NM1Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor



                            IRL_NM1.Rows.Add(NM1Row)

                            Select Case _271_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "ISL"
                                    _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "IRL"
                                    _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "SL"
                                    _271_SL_STRING = _271_SL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "DL"
                                    _271_DL_STRING = _271_DL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
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
                            REFRow("271_ISL_GUID") = _271_ISL_GUID
                            REFRow("271_IRL_GUID") = _271_IRL_GUID
                            REFRow("271_SL_GUID") = _271_SL_GUID
                            REFRow("271_DL_GUID") = _271_DL_GUID
                            REFRow("271_EB_GUID") = _271_EB_GUID
                            REFRow("271_EB_GROUP_GUID") = _271_EB_GROUP_GUID
                            REFRow("271_LS_GUID") = _271_LS_GUID
                            REFRow("NM1_GUID") = _NM1_GUID
                            REFRow("HL01") = _HL01
                            REFRow("HL02") = _HL02
                            REFRow("HL03") = _HL03
                            REFRow("HL04") = _HL04
                            ' REFRow("P_GUID") = _P_GUID


                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then REFRow("REF01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else REFRow("REF01") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then REFRow("REF02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else REFRow("REF02") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then REFRow("REF03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else REFRow("REF03") = DBNull.Value

                            REFRow("ROW_NUMBER") = rowcount
                            REFRow("EB_ROW_NUMBER") = 0

                            REFRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            REFRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            REFRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                            IRL_REF.Rows.Add(REFRow)

                            Select Case _271_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "ISL"
                                    _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "IRL"
                                    _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "SL"
                                    _271_SL_STRING = _271_SL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "DL"
                                    _271_DL_STRING = _271_DL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
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
                            N3Row("BATCH_ID") = _BATCH_ID
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
                            N3Row("271_ISL_GUID") = _271_ISL_GUID
                            N3Row("271_IRL_GUID") = _271_IRL_GUID
                            N3Row("271_SL_GUID") = _271_SL_GUID
                            N3Row("271_DL_GUID") = _271_DL_GUID
                            N3Row("271_EB_GUID") = _271_EB_GUID
                            N3Row("271_EB_GROUP_GUID") = _271_EB_GROUP_GUID
                            N3Row("271_LS_GUID") = _271_LS_GUID
                            N3Row("NM1_GUID") = _NM1_GUID
                            N3Row("HL01") = _HL01
                            N3Row("HL02") = _HL02
                            N3Row("HL03") = _HL03
                            N3Row("HL04") = _HL04


                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then N3Row("N301") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else N3Row("N301") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then N3Row("N302") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else N3Row("N302") = DBNull.Value




                            N3Row("ROW_NUMBER") = rowcount
                            N3Row("EB_ROW_NUMBER") = 0


                            N3Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            N3Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            N3Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                            IRL_N3.Rows.Add(N3Row)

                            Select Case _271_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "ISL"
                                    _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "IRL"
                                    _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "SL"
                                    _271_SL_STRING = _271_SL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "DL"
                                    _271_DL_STRING = _271_DL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
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
                            N4Row("BATCH_ID") = _BATCH_ID
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
                            N4Row("271_ISL_GUID") = _271_ISL_GUID
                            N4Row("271_IRL_GUID") = _271_IRL_GUID
                            N4Row("271_SL_GUID") = _271_SL_GUID
                            N4Row("271_DL_GUID") = _271_DL_GUID
                            N4Row("271_EB_GUID") = _271_EB_GUID
                            N4Row("271_EB_GROUP_GUID") = _271_EB_GROUP_GUID
                            N4Row("271_LS_GUID") = _271_LS_GUID
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



                            N4Row("ROW_NUMBER") = rowcount
                            N4Row("EB_ROW_NUMBER") = 0

                            N4Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            N4Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            N4Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                            IRL_N4.Rows.Add(N4Row)

                            _RowProcessedFlag = 1

                            Select Case _271_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "ISL"
                                    _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "IRL"
                                    _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "SL"
                                    _271_SL_STRING = _271_SL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "DL"
                                    _271_DL_STRING = _271_DL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            End Select



                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                    End Try




                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   IRL :: AAA
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "AAA" Then

                            _RowProcessedFlag = 1

                            Dim AAARow As DataRow = IRL_AAA.NewRow
                            AAARow("DOCUMENT_ID") = _DOCUMENT_ID
                            AAARow("FILE_ID") = _FILE_ID
                            AAARow("BATCH_ID") = _BATCH_ID
                            AAARow("ISA_ID") = _ISA_ID
                            AAARow("GS_ID") = _GS_ID
                            AAARow("ST_ID") = _ST_ID
                            AAARow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            AAARow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            AAARow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            AAARow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            AAARow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            AAARow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            AAARow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            AAARow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            AAARow("271_ISL_GUID") = _271_ISL_GUID
                            AAARow("271_IRL_GUID") = _271_IRL_GUID
                            AAARow("271_SL_GUID") = _271_SL_GUID
                            AAARow("271_DL_GUID") = _271_DL_GUID
                            AAARow("271_EB_GUID") = _271_EB_GUID
                            AAARow("271_EB_GROUP_GUID") = _271_EB_GROUP_GUID
                            AAARow("271_LS_GUID") = _271_LS_GUID
                            AAARow("HL01") = _HL01
                            AAARow("HL02") = _HL02
                            AAARow("HL03") = _HL03
                            AAARow("HL04") = _HL04



                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then AAARow("AAA01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else AAARow("AAA01") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then AAARow("AAA02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else AAARow("AAA02") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then AAARow("AAA03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else AAARow("AAA03") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then AAARow("AAA04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else AAARow("AAA04") = DBNull.Value

                            AAARow("ROW_NUMBER") = rowcount
                            AAARow("EB_ROW_NUMBER") = 0

                            AAARow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            AAARow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            AAARow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                            IRL_AAA.Rows.Add(AAARow)

                            Select Case _271_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "ISL"
                                    _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "IRL"
                                    _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "SL"
                                    _271_SL_STRING = _271_SL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "DL"
                                    _271_DL_STRING = _271_DL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
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
                            PRVRow("BATCH_ID") = _BATCH_ID
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
                            PRVRow("271_ISL_GUID") = _271_ISL_GUID
                            PRVRow("271_IRL_GUID") = _271_IRL_GUID
                            PRVRow("271_SL_GUID") = _271_SL_GUID
                            PRVRow("271_DL_GUID") = _271_DL_GUID
                            PRVRow("271_EB_GUID") = _271_EB_GUID
                            PRVRow("271_EB_GROUP_GUID") = _271_EB_GROUP_GUID
                            PRVRow("271_LS_GUID") = _271_LS_GUID
                            PRVRow("HL01") = _HL01
                            PRVRow("HL02") = _HL02
                            PRVRow("HL03") = _HL03
                            PRVRow("HL04") = _HL04

                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then PRVRow("PRV01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else PRVRow("PRV01") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then PRVRow("PRV02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else PRVRow("PRV02") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then PRVRow("PRV03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else PRVRow("PRV03") = DBNull.Value

                            PRVRow("ROW_NUMBER") = rowcount
                            PRVRow("EB_ROW_NUMBER") = 0

                            PRVRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            PRVRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            PRVRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                            IRL_PRV.Rows.Add(PRVRow)


                            Select Case _271_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "ISL"
                                    _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "IRL"
                                    _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "SL"
                                    _271_SL_STRING = _271_SL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case "DL"
                                    _271_DL_STRING = _271_DL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
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

                If _271_LOOP_LEVEL = "SL" Or _271_LOOP_LEVEL = "DL" Then


                    If _RowRecordType = "EB" Then
                        _EB_DIRTY = True
                        _LAST_EB = _CURRENT_EB

                        _CURRENT_EB = _CurrentRowData


                        'If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then
                        '    _PEB = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4)

                        'End If





                    End If


                    If _RowRecordType = "EB" Then

                        _IN_EB = True

                        If _FIRST_EB_FOUND = False Then
                            _FIRST_EB_FOUND = True
                        Else
                            ProcessEB(_EBIList, _LAST_EB)
                            _EBIList.Clear()
                        End If

                    End If


                    If _IN_EB Then
                        _RowProcessedFlag = 1
                        _EBIList.Add(Convert.ToString(rowcount) + "`" + _CurrentRowData)
                    Else


                        Try
                            If _RowRecordType = "TRN" Then



                                Dim TRNRow As DataRow = TRN.NewRow
                                TRNRow("DOCUMENT_ID") = _DOCUMENT_ID
                                TRNRow("FILE_ID") = _FILE_ID
                                TRNRow("BATCH_ID") = _BATCH_ID
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
                                TRNRow("271_ISL_GUID") = _271_ISL_GUID
                                TRNRow("271_IRL_GUID") = _271_IRL_GUID
                                TRNRow("271_SL_GUID") = _271_SL_GUID
                                TRNRow("271_DL_GUID") = _271_DL_GUID
                                TRNRow("271_EB_GUID") = _271_EB_GUID
                                TRNRow("271_EB_GROUP_GUID") = _271_EB_GROUP_GUID
                                TRNRow("271_LS_GUID") = _271_LS_GUID
                                TRNRow("NM1_GUID") = _NM1_GUID
                                TRNRow("HL01") = _HL01
                                TRNRow("HL02") = _HL02
                                TRNRow("HL03") = _HL03
                                TRNRow("HL04") = _HL04


                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then TRNRow("TRN01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else TRNRow("TRN01") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then TRNRow("TRN02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else TRNRow("TRN02") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then TRNRow("TRN03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else TRNRow("TRN03") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then TRNRow("TRN04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else TRNRow("TRN04") = DBNull.Value



                                TRNRow("ROW_NUMBER") = rowcount
                                TRNRow("EB_ROW_NUMBER") = 0

                                TRNRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                TRNRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                                TRNRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                                Select Case _271_LOOP_LEVEL
                                    Case "HEADER"
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "ISL"
                                        _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "IRL"
                                        _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "SL"
                                        _271_SL_STRING = _271_SL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "DL"
                                        _271_DL_STRING = _271_DL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
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
                                NM1Row("BATCH_ID") = _BATCH_ID
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
                                NM1Row("271_ISL_GUID") = _271_ISL_GUID
                                NM1Row("271_IRL_GUID") = _271_IRL_GUID
                                NM1Row("271_SL_GUID") = _271_SL_GUID
                                NM1Row("271_DL_GUID") = _271_DL_GUID
                                NM1Row("271_EB_GUID") = _271_EB_GUID
                                NM1Row("271_EB_GROUP_GUID") = _271_EB_GROUP_GUID
                                NM1Row("271_LS_GUID") = _271_LS_GUID
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

                                NM1Row("ROW_NUMBER") = rowcount
                                NM1Row("EB_ROW_NUMBER") = 0



                                NM1Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                NM1Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                                NM1Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor



                                NM1.Rows.Add(NM1Row)

                                Select Case _271_LOOP_LEVEL
                                    Case "HEADER"
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "ISL"
                                        _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "IRL"
                                        _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "SL"
                                        _271_SL_STRING = _271_SL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "DL"
                                        _271_DL_STRING = _271_DL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
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
                                REFRow("271_ISL_GUID") = _271_ISL_GUID
                                REFRow("271_IRL_GUID") = _271_IRL_GUID
                                REFRow("271_SL_GUID") = _271_SL_GUID
                                REFRow("271_DL_GUID") = _271_DL_GUID
                                REFRow("271_EB_GUID") = _271_EB_GUID
                                REFRow("271_EB_GROUP_GUID") = _271_EB_GROUP_GUID
                                REFRow("271_LS_GUID") = _271_LS_GUID
                                REFRow("NM1_GUID") = _NM1_GUID
                                REFRow("HL01") = _HL01
                                REFRow("HL02") = _HL02
                                REFRow("HL03") = _HL03
                                REFRow("HL04") = _HL04

                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then REFRow("REF01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else REFRow("REF01") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then REFRow("REF02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else REFRow("REF02") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then REFRow("REF03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else REFRow("REF03") = DBNull.Value

                                REFRow("ROW_NUMBER") = rowcount
                                REFRow("EB_ROW_NUMBER") = 0

                                REFRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                REFRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                                REFRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                                REF.Rows.Add(REFRow)

                                Select Case _271_LOOP_LEVEL
                                    Case "HEADER"
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "ISL"
                                        _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "IRL"
                                        _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "SL"
                                        _271_SL_STRING = _271_SL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "DL"
                                        _271_DL_STRING = _271_DL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
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
                                N3Row("BATCH_ID") = _BATCH_ID
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
                                N3Row("271_ISL_GUID") = _271_ISL_GUID
                                N3Row("271_IRL_GUID") = _271_IRL_GUID
                                N3Row("271_SL_GUID") = _271_SL_GUID
                                N3Row("271_DL_GUID") = _271_DL_GUID
                                N3Row("271_EB_GUID") = _271_EB_GUID
                                N3Row("271_EB_GROUP_GUID") = _271_EB_GROUP_GUID
                                N3Row("271_LS_GUID") = _271_LS_GUID
                                N3Row("NM1_GUID") = _NM1_GUID
                                N3Row("HL01") = _HL01
                                N3Row("HL02") = _HL02
                                N3Row("HL03") = _HL03
                                N3Row("HL04") = _HL04


                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then N3Row("N301") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else N3Row("N301") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then N3Row("N302") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else N3Row("N302") = DBNull.Value




                                N3Row("ROW_NUMBER") = rowcount
                                N3Row("EB_ROW_NUMBER") = 0


                                N3Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                N3Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                                N3Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                                N3.Rows.Add(N3Row)

                                Select Case _271_LOOP_LEVEL
                                    Case "HEADER"
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "ISL"
                                        _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "IRL"
                                        _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "SL"
                                        _271_SL_STRING = _271_SL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "DL"
                                        _271_DL_STRING = _271_DL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
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
                                N4Row("BATCH_ID") = _BATCH_ID
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
                                N4Row("271_ISL_GUID") = _271_ISL_GUID
                                N4Row("271_IRL_GUID") = _271_IRL_GUID
                                N4Row("271_SL_GUID") = _271_SL_GUID
                                N4Row("271_DL_GUID") = _271_DL_GUID
                                N4Row("271_EB_GUID") = _271_EB_GUID
                                N4Row("271_EB_GROUP_GUID") = _271_EB_GROUP_GUID
                                N4Row("271_LS_GUID") = _271_LS_GUID
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



                                N4Row("ROW_NUMBER") = rowcount
                                N4Row("EB_ROW_NUMBER") = 0

                                N4Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                N4Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                                N4Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                                N4.Rows.Add(N4Row)

                                _RowProcessedFlag = 1

                                Select Case _271_LOOP_LEVEL
                                    Case "HEADER"
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "ISL"
                                        _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "IRL"
                                        _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "SL"
                                        _271_SL_STRING = _271_ISL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "DL"
                                        _271_DL_STRING = _271_IRL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                End Select



                            End If

                        Catch ex As Exception
                            _RowProcessedFlag = -2
                            _hasERR = True
                            log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                        End Try




                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        '   SL/DL :: AAA
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        Try
                            If _RowRecordType = "AAA" Then

                                _RowProcessedFlag = 1

                                Dim AAARow As DataRow = AAA.NewRow
                                AAARow("DOCUMENT_ID") = _DOCUMENT_ID
                                AAARow("FILE_ID") = _FILE_ID
                                AAARow("BATCH_ID") = _BATCH_ID
                                AAARow("ISA_ID") = _ISA_ID
                                AAARow("GS_ID") = _GS_ID
                                AAARow("ST_ID") = _ST_ID
                                AAARow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                                AAARow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                                AAARow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                                AAARow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                                AAARow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                                AAARow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                                AAARow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                                AAARow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                                AAARow("271_ISL_GUID") = _271_ISL_GUID
                                AAARow("271_IRL_GUID") = _271_IRL_GUID
                                AAARow("271_SL_GUID") = _271_SL_GUID
                                AAARow("271_DL_GUID") = _271_DL_GUID
                                AAARow("271_EB_GUID") = _271_EB_GUID
                                AAARow("271_EB_GROUP_GUID") = _271_EB_GROUP_GUID
                                AAARow("271_LS_GUID") = _271_LS_GUID
                                AAARow("NM1_GUID") = _NM1_GUID
                                AAARow("HL01") = _HL01
                                AAARow("HL02") = _HL02
                                AAARow("HL03") = _HL03
                                AAARow("HL04") = _HL04



                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then AAARow("AAA01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else AAARow("AAA01") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then AAARow("AAA02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else AAARow("AAA02") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then AAARow("AAA03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else AAARow("AAA03") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then AAARow("AAA04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else AAARow("AAA04") = DBNull.Value

                                AAARow("ROW_NUMBER") = rowcount
                                AAARow("EB_ROW_NUMBER") = 0

                                AAARow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                AAARow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                                AAARow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                                AAA.Rows.Add(AAARow)

                                Select Case _271_LOOP_LEVEL
                                    Case "HEADER"
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "ISL"
                                        _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "IRL"
                                        _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "SL"
                                        _271_SL_STRING = _271_SL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "DL"
                                        _271_DL_STRING = _271_DL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
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
                                PRVRow("BATCH_ID") = _BATCH_ID
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
                                PRVRow("271_ISL_GUID") = _271_ISL_GUID
                                PRVRow("271_IRL_GUID") = _271_IRL_GUID
                                PRVRow("271_SL_GUID") = _271_SL_GUID
                                PRVRow("271_DL_GUID") = _271_DL_GUID
                                PRVRow("271_EB_GUID") = _271_EB_GUID
                                PRVRow("271_EB_GROUP_GUID") = _271_EB_GROUP_GUID
                                PRVRow("271_LS_GUID") = _271_LS_GUID
                                PRVRow("NM1_GUID") = _NM1_GUID
                                PRVRow("HL01") = _HL01
                                PRVRow("HL02") = _HL02
                                PRVRow("HL03") = _HL03
                                PRVRow("HL04") = _HL04

                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then PRVRow("PRV01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else PRVRow("PRV01") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then PRVRow("PRV02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else PRVRow("PRV02") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then PRVRow("PRV03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else PRVRow("PRV03") = DBNull.Value

                                PRVRow("ROW_NUMBER") = rowcount
                                PRVRow("EB_ROW_NUMBER") = 0

                                PRVRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                PRVRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                                PRVRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                                PRV.Rows.Add(PRVRow)


                                Select Case _271_LOOP_LEVEL
                                    Case "HEADER"
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "ISL"
                                        _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "IRL"
                                        _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "SL"
                                        _271_SL_STRING = _271_SL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "DL"
                                        _271_DL_STRING = _271_DL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
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
                                DMGRow("271_ISL_GUID") = _271_ISL_GUID
                                DMGRow("271_IRL_GUID") = _271_IRL_GUID
                                DMGRow("271_SL_GUID") = _271_SL_GUID
                                DMGRow("271_DL_GUID") = _271_DL_GUID
                                DMGRow("271_EB_GUID") = _271_EB_GUID
                                DMGRow("271_EB_GROUP_GUID") = _271_EB_GROUP_GUID
                                DMGRow("271_LS_GUID") = _271_LS_GUID
                                DMGRow("NM1_GUID") = _NM1_GUID
                                DMGRow("HL01") = _HL01
                                DMGRow("HL02") = _HL02
                                DMGRow("HL03") = _HL03
                                DMGRow("HL04") = _HL04

                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then DMGRow("DMG01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else DMGRow("DMG01") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then DMGRow("DMG02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else DMGRow("DMG02") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then DMGRow("DMG03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else DMGRow("DMG03") = DBNull.Value

                                DMGRow("ROW_NUMBER") = rowcount
                                DMGRow("EB_ROW_NUMBER") = 0


                                DMGRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                DMGRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                                DMGRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor


                                DMG.Rows.Add(DMGRow)


                                Select Case _271_LOOP_LEVEL
                                    Case "HEADER"
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "ISL"
                                        _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "IRL"
                                        _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "SL"
                                        _271_SL_STRING = _271_SL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "DL"
                                        _271_DL_STRING = _271_DL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
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
                                INSRow("BATCH_ID") = _BATCH_ID
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
                                INSRow("271_ISL_GUID") = _271_ISL_GUID
                                INSRow("271_IRL_GUID") = _271_IRL_GUID
                                INSRow("271_SL_GUID") = _271_SL_GUID
                                INSRow("271_DL_GUID") = _271_DL_GUID
                                INSRow("271_EB_GUID") = _271_EB_GUID
                                INSRow("271_EB_GROUP_GUID") = _271_EB_GROUP_GUID
                                INSRow("271_LS_GUID") = _271_LS_GUID
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




                                INSRow("ROW_NUMBER") = rowcount
                                INSRow("EB_ROW_NUMBER") = 0

                                INSRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                INSRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                                INSRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                                INS.Rows.Add(INSRow)


                                Select Case _271_LOOP_LEVEL
                                    Case "HEADER"
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "ISL"
                                        _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "IRL"
                                        _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "SL"
                                        _271_SL_STRING = _271_SL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "DL"
                                        _271_DL_STRING = _271_DL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
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
                                DTPRow("271_ISL_GUID") = _271_ISL_GUID
                                DTPRow("271_IRL_GUID") = _271_IRL_GUID
                                DTPRow("271_SL_GUID") = _271_SL_GUID
                                DTPRow("271_DL_GUID") = _271_DL_GUID
                                DTPRow("271_EB_GUID") = _271_EB_GUID
                                DTPRow("271_EB_GROUP_GUID") = _271_EB_GROUP_GUID
                                DTPRow("271_LS_GUID") = _271_LS_GUID
                                DTPRow("NM1_GUID") = _NM1_GUID
                                DTPRow("HL01") = _HL01
                                DTPRow("HL02") = _HL02
                                DTPRow("HL03") = _HL03
                                DTPRow("HL04") = _HL04

                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then DTPRow("DTP01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else DTPRow("DTP01") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then DTPRow("DTP02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else DTPRow("DTP02") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then DTPRow("DTP03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else DTPRow("DTP03") = DBNull.Value
                                '    If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then DTPRow("DTP04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else DTPRow("DTP04") = DBNull.Value


                                DTPRow("ROW_NUMBER") = rowcount
                                DTPRow("EB_ROW_NUMBER") = 0
                                DTPRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                DTPRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                                DTPRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                                DTP.Rows.Add(DTPRow)


                                Select Case _271_LOOP_LEVEL
                                    Case "HEADER"
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "ISL"
                                        _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "IRL"
                                        _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "SL"
                                        _271_SL_STRING = _271_SL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "DL"
                                        _271_DL_STRING = _271_DL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
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
                                HIRow("BATCH_ID") = _BATCH_ID
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
                                HIRow("271_ISL_GUID") = _271_ISL_GUID
                                HIRow("271_IRL_GUID") = _271_IRL_GUID
                                HIRow("271_SL_GUID") = _271_SL_GUID
                                HIRow("271_DL_GUID") = _271_DL_GUID
                                HIRow("271_EB_GUID") = _271_EB_GUID
                                HIRow("271_EB_GROUP_GUID") = _271_EB_GROUP_GUID
                                HIRow("271_LS_GUID") = _271_LS_GUID
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

                                HIRow("ROW_NUMBER") = rowcount
                                HIRow("EB_ROW_NUMBER") = 0


                                HIRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                HIRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                                HIRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                                HI.Rows.Add(HIRow)




                                Select Case _271_LOOP_LEVEL
                                    Case "HEADER"
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "ISL"
                                        _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "IRL"
                                        _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "SL"
                                        _271_SL_STRING = _271_SL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "DL"
                                        _271_DL_STRING = _271_DL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                End Select

                            End If
                        Catch ex As Exception

                            _RowProcessedFlag = -2
                            _hasERR = True
                            log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                        End Try



                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        '   SL/DL :: MPI
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


                        Try
                            If _RowRecordType = "MPI" Then
                                _RowProcessedFlag = 1

                                Dim MPIRow As DataRow = MPI.NewRow
                                MPIRow("FILE_ID") = _FILE_ID
                                MPIRow("BATCH_ID") = _BATCH_ID
                                MPIRow("ISA_ID") = _ISA_ID
                                MPIRow("GS_ID") = _GS_ID
                                MPIRow("ST_ID") = _ST_ID
                                MPIRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                                MPIRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                                MPIRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                                MPIRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                                MPIRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                                MPIRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                                MPIRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                                MPIRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                                MPIRow("271_ISL_GUID") = _271_ISL_GUID
                                MPIRow("271_IRL_GUID") = _271_IRL_GUID
                                MPIRow("271_SL_GUID") = _271_SL_GUID
                                MPIRow("271_DL_GUID") = _271_DL_GUID
                                MPIRow("271_EB_GUID") = _271_EB_GUID
                                MPIRow("271_LS_GUID") = _271_LS_GUID
                                MPIRow("NM1_GUID") = _NM1_GUID
                                MPIRow("HL01") = _HL01
                                MPIRow("HL02") = _HL02
                                MPIRow("HL03") = _HL03
                                MPIRow("HL04") = _HL04

                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then MPIRow("MPI01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else MPIRow("MPI01") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then MPIRow("MPI02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else MPIRow("MPI02") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then MPIRow("MPI03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else MPIRow("MPI03") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then MPIRow("MPI04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else MPIRow("MPI04") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then MPIRow("MPI05") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else MPIRow("MPI05") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then MPIRow("MPI06") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) Else MPIRow("MPI06") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then MPIRow("MPI07") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) Else MPIRow("MPI07") = DBNull.Value


                                MPIRow("ROW_NUMBER") = rowcount
                                MPIRow("EB_ROW_NUMBER") = 0

                                MPIRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                MPIRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                                MPIRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                                MPI.Rows.Add(MPIRow)


                                Select Case _271_LOOP_LEVEL
                                    Case "HEADER"
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "ISL"
                                        _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "IRL"
                                        _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "SL"
                                        _271_SL_STRING = _271_SL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case "DL"
                                        _271_DL_STRING = _271_DL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
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




                        Dim SERow As DataRow = SE.NewRow
                        SERow("FILE_ID") = _FILE_ID
                        SERow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                        SERow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                        SERow("HIPAA_ST_GUID") = _HIPAA_ST_GUID

                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then SERow("SE01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else SERow("SE01") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then SERow("SE02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else SERow("SE02") = DBNull.Value

                        SERow("ROW_NUMBER") = rowcount
                        SE.Rows.Add(SERow)





                        If _2000A_DIRTY Then

                            COMIT2000AHeaderDump()
                            _2000A_DIRTY = False

                        End If

                        If _2000B_DIRTY Then

                            COMIT2000BHeaderDump()
                            _2000B_DIRTY = False

                        End If

                        ProcessEB(_EBIList, _CURRENT_EB)

                        If _HL22_DIRTY Then
                            COMIT_271_22_Dump()
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






                        Dim GERow As DataRow = GE.NewRow
                        GERow("FILE_ID") = _FILE_ID
                        GERow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                        GERow("HIPAA_GS_GUID") = _HIPAA_GS_GUID


                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then GERow("GE01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else GERow("GE01") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then GERow("GE02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else GERow("GE02") = DBNull.Value

                        GERow("ROW_NUMBER") = rowcount
                        GE.Rows.Add(GERow)


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

                        Dim IEARow As DataRow = IEA.NewRow
                        IEARow("FILE_ID") = _FILE_ID
                        IEARow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID


                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then IEARow("IEA01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else IEARow("IEA01") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then IEARow("IEA02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else IEARow("IEA02") = DBNull.Value

                        IEARow("ROW_NUMBER") = rowcount
                        IEA.Rows.Add(IEARow)


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
                        UNKRow("ROW_RECORD_TYPE") = _RowRecordType
                        UNKRow("ROW_DATA") = _CurrentRowData
                        UNKRow("ROW_NUMBER") = rowcount
                        UNKRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        UNKRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                        UNKRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                        UNK.Rows.Add(UNKRow)




                        Select Case _271_LOOP_LEVEL
                            Case "HEADER"
                                _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            Case "ISL"
                                _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            Case "IRL"
                                _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            Case "SL"
                                _271_SL_STRING = _271_SL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            Case "DL"
                                _271_DL_STRING = _271_DL_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            Case "EB"
                                _271_EB_STRING = _271_EB_STRING + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
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
                    e.TransactionSetIdentifierCode = "271"
                    If _hasERR Then
                        e.UpdateFileStatus(CInt(_FILE_ID), "271", "PARSE COMPLETE WITH UNK SEGMENTS WITH ERRORS")
                    Else
                        e.UpdateFileStatus(CInt(_FILE_ID), "271", "PARSE COMPLETE WITH UNK SEGMENTS")
                    End If



                End Using

                ComitUNK()

            Else

                Using e As New EDI_5010_LOGGING
                    e.ConnectionString = _ConnectionString
                    e.TransactionSetIdentifierCode = "271"
                    If _hasERR Then
                        e.UpdateFileStatus(CInt(_FILE_ID), "271", "PARSE COMPLETE WITH ERRORS")
                    Else
                        e.UpdateFileStatus(CInt(_FILE_ID), "271", "PARSE COMPLETE")
                    End If
                End Using

            End If




            _ProcessEndTime = DateTime.Now

            Return _ImportReturnCode

        End Function
        Public Function CountCharacter(ByVal value As String, ByVal ch As Char) As Integer
            Return value.Count(Function(c As Char) c = ch)
        End Function

        Private Function ProcessEB(ByVal EBIList As List(Of String), ByVal PEB As String) As Integer
            Dim r As Integer = -1
            Dim c As Integer = 0
            Dim x As Integer = 0






            _FUNCTION_NAME = "ProcessEB(ByVal EBIList As List(Of String), ByVal PEB As String) As Integer"

            Dim __EB_RowRecordType As String = String.Empty
            Dim __EB_CurrentRowData As String = String.Empty
            Dim __EB_STRING As String = String.Empty
            Dim __EB13 As String = String.Empty
            Dim __EB_ROWCOUNT As Integer = 0
            Dim __ORD_RowCount As Integer = 0
            Dim __MSGList As List(Of String)
            Dim __MSGList_SMAHASY As List(Of String)
            Dim MSG_STRING As String = String.Empty
            Dim __EB_GROUP_GUID As Guid = Guid.NewGuid
            Dim __LS_GUID As Guid = Guid.Empty
            Dim __EB_GUID As Guid = Guid.Empty
            _LoopLevelMajor = 2100
            _LoopLevelMinor = 10


            _NM1_GUID = Guid.Empty


            __EB_STRING = ss.ParseDemlimtedStringEDI(PEB, _DataElementSeparator, 4)



            '  __MSGList = EBIList

            '  __MSGList_SMAHASY = MSGSmasherThingy(__MSGList)


            c = CountCharacter(PEB, CChar(_RepetitionSeparator))

            Do Until x = c + 1
                x += 1

                Dim __EBIList As List(Of String)


                __EBIList = EBIList



                For Each line As String In __EBIList

                    '  Dim ___MSGList_SMAHASY As List(Of String)
                    '  ___MSGList_SMAHASY = __MSGList_SMAHASY

                    __ORD_RowCount = Convert.ToInt32(ss.ParseDemlimtedStringEDI(line, "`", 1))


                    Dim _EB_STRING_CURRENT = ss.ParseDemlimtedStringEDI(__EB_STRING, _RepetitionSeparator, x)

                    __EB_ROWCOUNT = __EB_ROWCOUNT + 1




                        'Console.WriteLine(_RowRecordType)
                    __EB_CurrentRowData = ss.ParseDemlimtedStringEDI(line, "`", 2)
                    __EB_RowRecordType = ss.ParseDemlimtedStringEDI(__EB_CurrentRowData, _DataElementSeparator, 1)

                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        '   EB :: EB
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If __EB_RowRecordType = "EB" Then

                            __EB_GUID = Guid.NewGuid



                            Dim EBRow As DataRow = EB.NewRow

                            EBRow("DOCUMENT_ID") = _DOCUMENT_ID
                            EBRow("FILE_ID") = _FILE_ID
                            EBRow("BATCH_ID") = _BATCH_ID
                            EBRow("ISA_ID") = _ISA_ID
                            EBRow("GS_ID") = _GS_ID
                            EBRow("ST_ID") = _ST_ID
                            EBRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            EBRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            EBRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            EBRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            EBRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            EBRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            EBRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            EBRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            EBRow("271_ISL_GUID") = _271_ISL_GUID
                            EBRow("271_IRL_GUID") = _271_IRL_GUID
                            EBRow("271_SL_GUID") = _271_SL_GUID
                            EBRow("271_DL_GUID") = _271_DL_GUID
                            EBRow("271_EB_GROUP_GUID") = __EB_GROUP_GUID
                            EBRow("271_EB_GUID") = __EB_GUID
                            EBRow("271_LS_GUID") = __LS_GUID
                            EBRow("NM1_GUID") = _NM1_GUID
                            EBRow("HL01") = _HL01
                            EBRow("HL02") = _HL02
                            EBRow("HL03") = _HL03
                            EBRow("HL04") = _HL04

                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 2) <> "") Then EBRow("EB01") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 2) Else EBRow("EB02") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 3) <> "") Then EBRow("EB02") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 3) Else EBRow("EB02") = DBNull.Value


                            ' SURESH HAD ME SWAP EB03 AND PEBO3

                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 4) <> "") Then EBRow("PEB03") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 4) Else EBRow("PEB03") = DBNull.Value

                            EBRow("EB03") = _EB_STRING_CURRENT


                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 5) <> "") Then EBRow("EB04") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 5) Else EBRow("EB04") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 6) <> "") Then EBRow("EB05") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 6) Else EBRow("EB05") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 7) <> "") Then EBRow("EB06") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 7) Else EBRow("EB06") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 8) <> "") Then EBRow("EB07") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 8) Else EBRow("EB07") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 9) <> "") Then EBRow("EB08") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 9) Else EBRow("EB08") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 10) <> "") Then EBRow("EB09") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 10) Else EBRow("EB09") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 11) <> "") Then EBRow("EB10") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 11) Else EBRow("EB10") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 12) <> "") Then EBRow("EB11") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 12) Else EBRow("EB11") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 13) <> "") Then EBRow("EB12") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 13) Else EBRow("EB12") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 14) <> "") Then EBRow("EB13") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 14) Else EBRow("EB13") = DBNull.Value



                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 14) <> "") Then
                                EBRow("EB13") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 14)
                                __EB13 = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 14)
                            Else
                                EBRow("EB13") = DBNull.Value
                                End If


                            If (ss.ParseDemlimtedString(__EB13, _ComponentElementSeparator, 1) <> "") Then EBRow("EB13_1") = ss.ParseDemlimtedString(__EB13, _ComponentElementSeparator, 1) Else EBRow("EB13_1") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB13, _ComponentElementSeparator, 2) <> "") Then EBRow("EB13_2") = ss.ParseDemlimtedString(__EB13, _ComponentElementSeparator, 2) Else EBRow("EB13_2") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB13, _ComponentElementSeparator, 3) <> "") Then EBRow("EB13_3") = ss.ParseDemlimtedString(__EB13, _ComponentElementSeparator, 3) Else EBRow("EB13_3") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB13, _ComponentElementSeparator, 4) <> "") Then EBRow("EB13_4") = ss.ParseDemlimtedString(__EB13, _ComponentElementSeparator, 4) Else EBRow("EB13_4") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB13, _ComponentElementSeparator, 5) <> "") Then EBRow("EB13_5") = ss.ParseDemlimtedString(__EB13, _ComponentElementSeparator, 5) Else EBRow("EB13_5") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB13, _ComponentElementSeparator, 6) <> "") Then EBRow("EB13_6") = ss.ParseDemlimtedString(__EB13, _ComponentElementSeparator, 6) Else EBRow("EB13_6") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB13, _ComponentElementSeparator, 7) <> "") Then EBRow("EB13_7") = ss.ParseDemlimtedString(__EB13, _ComponentElementSeparator, 7) Else EBRow("EB13_7") = DBNull.Value

                            EBRow("ROW_NUMBER") = __ORD_RowCount
                            EBRow("EB_ROW_NUMBER") = __EB_ROWCOUNT


                            EBRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            EBRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            EBRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                            EB.Rows.Add(EBRow)


                            Select Case _271_LOOP_LEVEL
                                    Case "HEADER"
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                    Case "ISL"
                                        _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                    Case "IRL"
                                        _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"

                                    Case "SL"
                                        _271_SL_STRING = _271_SL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                    Case "DL"
                                        _271_DL_STRING = _271_DL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                End Select


                            End If

                        Catch ex As Exception
                            _RowProcessedFlag = -2
                            _hasERR = True
                            log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, __EB_RowRecordType, ex)
                        End Try


                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        '   EB :: HSD
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        Try
                            If __EB_RowRecordType = "HSD" Then


                                Dim HSDRow As DataRow = HSD.NewRow
                                HSDRow("DOCUMENT_ID") = _DOCUMENT_ID
                                HSDRow("FILE_ID") = _FILE_ID
                                HSDRow("BATCH_ID") = _BATCH_ID
                                HSDRow("ISA_ID") = _ISA_ID
                                HSDRow("GS_ID") = _GS_ID
                                HSDRow("ST_ID") = _ST_ID
                                HSDRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                                HSDRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                                HSDRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                                HSDRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                                HSDRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                                HSDRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                                HSDRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                                HSDRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                                HSDRow("271_ISL_GUID") = _271_ISL_GUID
                                HSDRow("271_IRL_GUID") = _271_IRL_GUID
                                HSDRow("271_SL_GUID") = _271_SL_GUID
                                HSDRow("271_DL_GUID") = _271_DL_GUID
                                HSDRow("271_EB_GROUP_GUID") = __EB_GROUP_GUID
                                HSDRow("271_EB_GUID") = __EB_GUID
                                HSDRow("271_LS_GUID") = __LS_GUID
                                HSDRow("NM1_GUID") = _NM1_GUID
                                HSDRow("HL01") = _HL01
                                HSDRow("HL02") = _HL02
                                HSDRow("HL03") = _HL03
                                HSDRow("HL04") = _HL04


                                If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 2) <> "") Then HSDRow("HSD01") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 2) Else HSDRow("HSD01") = DBNull.Value
                                If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 3) <> "") Then HSDRow("HSD02") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 3) Else HSDRow("HSD02") = DBNull.Value
                                If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 4) <> "") Then HSDRow("HSD03") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 4) Else HSDRow("HSD03") = DBNull.Value
                                If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 5) <> "") Then HSDRow("HSD04") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 5) Else HSDRow("HSD04") = DBNull.Value
                                If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 6) <> "") Then HSDRow("HSD05") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 6) Else HSDRow("HSD05") = DBNull.Value
                                If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 7) <> "") Then HSDRow("HSD06") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 7) Else HSDRow("HSD06") = DBNull.Value
                                If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 8) <> "") Then HSDRow("HSD07") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 8) Else HSDRow("HSD07") = DBNull.Value
                                If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 9) <> "") Then HSDRow("HSD08") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 9) Else HSDRow("HSD08") = DBNull.Value







                                HSDRow("ROW_NUMBER") = __ORD_RowCount
                                HSDRow("EB_ROW_NUMBER") = __EB_ROWCOUNT
                                HSDRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                HSDRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                                HSDRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                                HSD.Rows.Add(HSDRow)

                                _RowProcessedFlag = 1




                                Select Case _271_LOOP_LEVEL
                                    Case "HEADER"
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                    Case "ISL"
                                        _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                    Case "IRL"
                                        _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"

                                    Case "SL"
                                        _271_SL_STRING = _271_SL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                    Case "DL"
                                        _271_DL_STRING = _271_DL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                End Select

                            End If

                        Catch ex As Exception
                            _RowProcessedFlag = -2
                            _hasERR = True
                            log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, __EB_RowRecordType, ex)
                        End Try



                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        '   EB :: REF
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        Try
                            If __EB_RowRecordType = "REF" Then

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
                                REFRow("271_ISL_GUID") = _271_ISL_GUID
                                REFRow("271_IRL_GUID") = _271_IRL_GUID
                                REFRow("271_SL_GUID") = _271_SL_GUID
                                REFRow("271_DL_GUID") = _271_DL_GUID
                                REFRow("271_EB_GROUP_GUID") = __EB_GROUP_GUID
                                REFRow("271_EB_GUID") = __EB_GUID
                                REFRow("271_LS_GUID") = __LS_GUID
                                REFRow("NM1_GUID") = _NM1_GUID
                                REFRow("HL01") = _HL01
                                REFRow("HL02") = _HL02
                                REFRow("HL03") = _HL03
                                REFRow("HL04") = _HL04


                                If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 2) <> "") Then REFRow("REF01") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 2) Else REFRow("REF01") = DBNull.Value
                                If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 3) <> "") Then REFRow("REF02") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 3) Else REFRow("REF02") = DBNull.Value
                                If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 4) <> "") Then REFRow("REF03") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 4) Else REFRow("REF03") = DBNull.Value











                                REFRow("ROW_NUMBER") = __ORD_RowCount
                                REFRow("EB_ROW_NUMBER") = __EB_ROWCOUNT
                                REFRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                REFRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                                REFRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                                REF.Rows.Add(REFRow)




                                Select Case _271_LOOP_LEVEL
                                    Case "HEADER"
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                    Case "ISL"
                                        _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                    Case "IRL"
                                        _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"

                                    Case "SL"
                                        _271_SL_STRING = _271_SL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                    Case "DL"
                                        _271_DL_STRING = _271_DL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                End Select

                            End If

                        Catch ex As Exception
                            _RowProcessedFlag = -2
                            _hasERR = True
                            log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, __EB_RowRecordType, ex)
                        End Try



                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        '  EB ::  DTP
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                        Try

                            If __EB_RowRecordType = "DTP" Then

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
                                DTPRow("271_ISL_GUID") = _271_ISL_GUID
                                DTPRow("271_IRL_GUID") = _271_IRL_GUID
                                DTPRow("271_SL_GUID") = _271_SL_GUID
                                DTPRow("271_DL_GUID") = _271_DL_GUID
                                DTPRow("271_EB_GROUP_GUID") = __EB_GROUP_GUID
                                DTPRow("271_EB_GUID") = __EB_GUID
                                DTPRow("271_LS_GUID") = __LS_GUID
                                DTPRow("NM1_GUID") = _NM1_GUID
                                DTPRow("HL01") = _HL01
                                DTPRow("HL02") = _HL02
                                DTPRow("HL03") = _HL03
                                DTPRow("HL04") = _HL04


                                If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 2) <> "") Then DTPRow("DTP01") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 2) Else DTPRow("DTP01") = DBNull.Value
                                If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 3) <> "") Then DTPRow("DTP02") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 3) Else DTPRow("DTP02") = DBNull.Value
                                If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 4) <> "") Then DTPRow("DTP03") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 4) Else DTPRow("DTP03") = DBNull.Value





                                DTPRow("ROW_NUMBER") = __ORD_RowCount
                                DTPRow("EB_ROW_NUMBER") = __EB_ROWCOUNT
                                DTPRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                DTPRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                                DTPRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                                DTP.Rows.Add(DTPRow)



                                Select Case _271_LOOP_LEVEL
                                    Case "HEADER"
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                    Case "ISL"
                                        _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                    Case "IRL"
                                        _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"

                                    Case "SL"
                                        _271_SL_STRING = _271_SL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                    Case "DL"
                                        _271_DL_STRING = _271_DL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                End Select

                            End If

                        Catch ex As Exception
                            _RowProcessedFlag = -2
                            _hasERR = True
                            log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, __EB_RowRecordType, ex)
                        End Try


                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        '  EB ::  AAA
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        Try
                            If __EB_RowRecordType = "AAA" Then

                                Dim AAARow As DataRow = AAA.NewRow

                                AAARow("DOCUMENT_ID") = _DOCUMENT_ID
                                AAARow("FILE_ID") = _FILE_ID
                                AAARow("BATCH_ID") = _BATCH_ID
                                AAARow("ISA_ID") = _ISA_ID
                                AAARow("GS_ID") = _GS_ID
                                AAARow("ST_ID") = _ST_ID
                                AAARow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                                AAARow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                                AAARow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                                AAARow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                                AAARow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                                AAARow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                                AAARow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                                AAARow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                                AAARow("271_ISL_GUID") = _271_ISL_GUID
                                AAARow("271_IRL_GUID") = _271_IRL_GUID
                                AAARow("271_SL_GUID") = _271_SL_GUID
                                AAARow("271_DL_GUID") = _271_DL_GUID
                                AAARow("271_EB_GROUP_GUID") = __EB_GROUP_GUID
                                AAARow("271_EB_GUID") = __EB_GUID
                                AAARow("271_LS_GUID") = __LS_GUID
                                AAARow("NM1_GUID") = _NM1_GUID
                                AAARow("HL01") = _HL01
                                AAARow("HL02") = _HL02
                                AAARow("HL03") = _HL03
                                AAARow("HL04") = _HL04


                                If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 2) <> "") Then AAARow("AAA01") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 2) Else AAARow("AAA01") = DBNull.Value
                                If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 3) <> "") Then AAARow("AAA02") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 3) Else AAARow("AAA02") = DBNull.Value
                                If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 4) <> "") Then AAARow("AAA03") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 4) Else AAARow("AAA03") = DBNull.Value
                                If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 5) <> "") Then AAARow("AAA04") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 5) Else AAARow("AAA04") = DBNull.Value







                                AAARow("ROW_NUMBER") = __ORD_RowCount
                                AAARow("EB_ROW_NUMBER") = __EB_ROWCOUNT
                                AAARow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                AAARow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                                AAARow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                                AAA.Rows.Add(AAARow)




                                Select Case _271_LOOP_LEVEL
                                    Case "HEADER"
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                    Case "ISL"
                                        _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                    Case "IRL"
                                        _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"

                                    Case "SL"
                                        _271_SL_STRING = _271_SL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                    Case "DL"
                                        _271_DL_STRING = _271_DL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                End Select

                            End If

                        Catch ex As Exception
                            _RowProcessedFlag = -2
                            _hasERR = True
                            log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, __EB_RowRecordType, ex)
                        End Try

                        'MSG
                        'MESSAGE SMASY PROBLEM
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        '  EB ::  AAA
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        Try
                        If __EB_RowRecordType = "MSG" Then




                            ''  For Each MSGG_line As String In ___MSGList_SMAHASY

                            Dim MSG01 As String = String.Empty
                            Dim MSG02 As String = String.Empty
                            Dim MSG03 As String = String.Empty

                            Dim MSG02_NEW_LINE_COUNT As Integer = 0

                            Dim MSGRow As DataRow = MSG.NewRow

                            MSGRow("DOCUMENT_ID") = _DOCUMENT_ID
                            MSGRow("FILE_ID") = _FILE_ID
                            MSGRow("BATCH_ID") = _BATCH_ID
                            MSGRow("ISA_ID") = _ISA_ID
                            MSGRow("GS_ID") = _GS_ID
                            MSGRow("ST_ID") = _ST_ID
                            MSGRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            MSGRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            MSGRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            MSGRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            MSGRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            MSGRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            MSGRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            MSGRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            MSGRow("271_ISL_GUID") = _271_ISL_GUID
                            MSGRow("271_IRL_GUID") = _271_IRL_GUID
                            MSGRow("271_SL_GUID") = _271_SL_GUID
                            MSGRow("271_DL_GUID") = _271_DL_GUID
                            MSGRow("271_EB_GROUP_GUID") = __EB_GROUP_GUID
                            MSGRow("271_EB_GUID") = __EB_GUID
                            MSGRow("271_LS_GUID") = __LS_GUID
                            MSGRow("NM1_GUID") = _NM1_GUID
                            MSGRow("HL01") = _HL01
                            MSGRow("HL02") = _HL02
                            MSGRow("HL03") = _HL03
                            MSGRow("HL04") = _HL04




                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 2) <> "") Then

                                MSG01 = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 2)

                            End If



                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 3) <> "") Then

                                MSG02 = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 3)

                            End If

                            If Regex.IsMatch(MSG02, "^[0-9 ]+$") Then

                                MSG02_NEW_LINE_COUNT = Convert.ToInt32(MSG02)

                            End If


                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 4) <> "") Then

                                MSG03 = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 4)
                            End If


                            MSG_STRING = MSG01
                            If MSG02_NEW_LINE_COUNT > 0 Then


                                For I As Integer = 1 To MSG02_NEW_LINE_COUNT

                                    MSG_STRING = MSG_STRING + vbNewLine

                                Next

                                MSG_STRING = MSG_STRING + MSG03
                            End If

                            MSGRow("MSG01") = MSG01
                            MSGRow("MSG02") = MSG02
                            MSGRow("MSG03") = MSG03

                            MSGRow("MSG") = MSG_STRING



                            MSGRow("ROW_NUMBER") = __ORD_RowCount
                            MSGRow("EB_ROW_NUMBER") = __EB_ROWCOUNT
                            MSGRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            MSGRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            MSGRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                            MSG.Rows.Add(MSGRow)




                            Select Case _271_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                Case "ISL"
                                    _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                Case "IRL"
                                    _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                Case "SL"
                                    _271_SL_STRING = _271_SL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                Case "DL"
                                    _271_DL_STRING = _271_DL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                            End Select



                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, __EB_RowRecordType, ex)
                    End Try




                    'III
                    'III LOOP SEGMENT
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '  EB ::  III
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If __EB_RowRecordType = "III" Then


                            _LoopLevelMajor = 2100
                            _LoopLevelMinor = 15



                            _RowProcessedFlag = 1

                            Dim IIIRow As DataRow = III.NewRow
                            IIIRow("DOCUMENT_ID") = _DOCUMENT_ID
                            IIIRow("FILE_ID") = _FILE_ID
                            IIIRow("BATCH_ID") = _BATCH_ID
                            IIIRow("ISA_ID") = _ISA_ID
                            IIIRow("GS_ID") = _GS_ID
                            IIIRow("ST_ID") = _ST_ID
                            IIIRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            IIIRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            IIIRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            IIIRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            IIIRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            IIIRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            IIIRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            IIIRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            IIIRow("271_ISL_GUID") = _271_ISL_GUID
                            IIIRow("271_IRL_GUID") = _271_IRL_GUID
                            IIIRow("271_SL_GUID") = _271_SL_GUID
                            IIIRow("271_DL_GUID") = _271_DL_GUID
                            IIIRow("271_EB_GROUP_GUID") = __EB_GROUP_GUID
                            IIIRow("271_EB_GUID") = __EB_GUID
                            IIIRow("271_LS_GUID") = __LS_GUID
                            IIIRow("NM1_GUID") = _NM1_GUID
                            IIIRow("HL01") = _HL01
                            IIIRow("HL02") = _HL02
                            IIIRow("HL03") = _HL03
                            IIIRow("HL04") = _HL04

                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 2) <> "") Then IIIRow("III01") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 2) Else IIIRow("III01") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 3) <> "") Then IIIRow("III02") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 3) Else IIIRow("III02") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 4) <> "") Then IIIRow("III03") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 4) Else IIIRow("III03") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 5) <> "") Then IIIRow("III04") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 5) Else IIIRow("III04") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 6) <> "") Then IIIRow("III05") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 6) Else IIIRow("III05") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 7) <> "") Then IIIRow("III06") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 7) Else IIIRow("III06") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 8) <> "") Then IIIRow("III07") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 8) Else IIIRow("III07") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 9) <> "") Then IIIRow("III08") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 9) Else IIIRow("III08") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 10) <> "") Then IIIRow("III09") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 10) Else IIIRow("III09") = DBNull.Value

                            IIIRow("ROW_NUMBER") = __ORD_RowCount
                            IIIRow("EB_ROW_NUMBER") = __EB_ROWCOUNT
                            IIIRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            IIIRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            IIIRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                            III.Rows.Add(IIIRow)

                            Select Case _271_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                Case "ISL"
                                    _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                Case "IRL"

                                Case "SL"
                                    _271_SL_STRING = _271_SL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                Case "DL"
                                    _271_DL_STRING = _271_DL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                            End Select

                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, __EB_RowRecordType, ex)
                    End Try




                    'LS LOOP SEGMENT
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '  EB ::  LS ::  LS
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If __EB_RowRecordType = "LS" Then


                            _LoopLevelMajor = 2100
                            _LoopLevelMinor = 20
                            _LoopLevelSubFix = ""


                            _RowProcessedFlag = 1

                            __LS_GUID = Guid.NewGuid

                            Dim LSRow As DataRow = LS.NewRow
                            LSRow("DOCUMENT_ID") = _DOCUMENT_ID
                            LSRow("FILE_ID") = _FILE_ID
                            LSRow("BATCH_ID") = _BATCH_ID
                            LSRow("ISA_ID") = _ISA_ID
                            LSRow("GS_ID") = _GS_ID
                            LSRow("ST_ID") = _ST_ID
                            LSRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            LSRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            LSRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            LSRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            LSRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            LSRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            LSRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            LSRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            LSRow("271_ISL_GUID") = _271_ISL_GUID
                            LSRow("271_IRL_GUID") = _271_IRL_GUID
                            LSRow("271_SL_GUID") = _271_SL_GUID
                            LSRow("271_DL_GUID") = _271_DL_GUID
                            LSRow("271_EB_GROUP_GUID") = __EB_GROUP_GUID
                            LSRow("271_EB_GUID") = __EB_GUID
                            LSRow("271_LS_GUID") = __LS_GUID
                            LSRow("NM1_GUID") = _NM1_GUID
                            LSRow("HL01") = _HL01
                            LSRow("HL02") = _HL02
                            LSRow("HL03") = _HL03
                            LSRow("HL04") = _HL04

                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 2) <> "") Then LSRow("LS01") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 2) Else LSRow("LS01") = DBNull.Value



                            LSRow("ROW_NUMBER") = __ORD_RowCount
                            LSRow("EB_ROW_NUMBER") = __EB_ROWCOUNT
                            LSRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            LSRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            LSRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix









                            LS.Rows.Add(LSRow)


                            Select Case _271_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                Case "ISL"
                                    _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                Case "IRL"
                                    _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"

                                Case "SL"
                                    _271_SL_STRING = _271_SL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                Case "DL"
                                    _271_DL_STRING = _271_DL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                            End Select


                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, __EB_RowRecordType, ex)
                    End Try




                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   EB ::  LS ::   NM1
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If __EB_RowRecordType = "NM1" Then

                            _RowProcessedFlag = 1

                            _NM1_GUID = Guid.NewGuid

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
                            NM1Row("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            NM1Row("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            NM1Row("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            NM1Row("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            NM1Row("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            NM1Row("271_ISL_GUID") = _271_ISL_GUID
                            NM1Row("271_IRL_GUID") = _271_IRL_GUID
                            NM1Row("271_SL_GUID") = _271_SL_GUID
                            NM1Row("271_DL_GUID") = _271_DL_GUID
                            NM1Row("271_EB_GROUP_GUID") = __EB_GROUP_GUID
                            NM1Row("271_EB_GUID") = __EB_GUID
                            NM1Row("271_LS_GUID") = __LS_GUID
                            NM1Row("NM1_GUID") = _NM1_GUID
                            NM1Row("HL01") = _HL01
                            NM1Row("HL02") = _HL02
                            NM1Row("HL03") = _HL03
                            NM1Row("HL04") = _HL04


                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 2) <> "") Then
                                NM1Row("NM101") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 2)
                                _NM01 = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 2)
                            Else
                                NM1Row("NM101") = DBNull.Value
                            End If


                            NM1Lookup(_NM01)

                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 3) <> "") Then NM1Row("NM102") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 3) Else NM1Row("NM102") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 4) <> "") Then NM1Row("NM103") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 4) Else NM1Row("NM103") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 5) <> "") Then NM1Row("NM104") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 5) Else NM1Row("NM104") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 6) <> "") Then NM1Row("NM105") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 6) Else NM1Row("NM105") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 7) <> "") Then NM1Row("NM106") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 7) Else NM1Row("NM106") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 8) <> "") Then NM1Row("NM107") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 8) Else NM1Row("NM107") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 9) <> "") Then NM1Row("NM108") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 9) Else NM1Row("NM108") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 10) <> "") Then NM1Row("NM109") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 10) Else NM1Row("NM109") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 11) <> "") Then NM1Row("NM110") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 11) Else NM1Row("NM110") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 12) <> "") Then NM1Row("NM111") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 12) Else NM1Row("NM111") = DBNull.Value

                            NM1Row("ROW_NUMBER") = __ORD_RowCount
                            NM1Row("EB_ROW_NUMBER") = __EB_ROWCOUNT
                            NM1Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            NM1Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            NM1Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix



                            NM1.Rows.Add(NM1Row)

                            Select Case _271_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                Case "ISL"
                                    _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                Case "IRL"
                                    _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"

                                Case "SL"
                                    _271_SL_STRING = _271_SL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                Case "DL"
                                    _271_DL_STRING = _271_DL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                            End Select


                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, __EB_RowRecordType, ex)
                    End Try



                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   EB ::  LS ::   N3
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If __EB_RowRecordType = "N3" Then


                            _RowProcessedFlag = 1
                            Dim N3Row As DataRow = N3.NewRow
                            N3Row("DOCUMENT_ID") = _DOCUMENT_ID
                            N3Row("FILE_ID") = _FILE_ID
                            N3Row("BATCH_ID") = _BATCH_ID
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
                            N3Row("271_ISL_GUID") = _271_ISL_GUID
                            N3Row("271_IRL_GUID") = _271_IRL_GUID
                            N3Row("271_SL_GUID") = _271_SL_GUID
                            N3Row("271_DL_GUID") = _271_DL_GUID
                            N3Row("271_EB_GROUP_GUID") = __EB_GROUP_GUID
                            N3Row("271_EB_GUID") = __EB_GUID
                            N3Row("271_LS_GUID") = __LS_GUID
                            N3Row("NM1_GUID") = _NM1_GUID
                            N3Row("HL01") = _HL01
                            N3Row("HL02") = _HL02
                            N3Row("HL03") = _HL03
                            N3Row("HL04") = _HL04


                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 2) <> "") Then N3Row("N301") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 2) Else N3Row("N301") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 3) <> "") Then N3Row("N302") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 3) Else N3Row("N302") = DBNull.Value




                            N3Row("ROW_NUMBER") = __ORD_RowCount
                            N3Row("EB_ROW_NUMBER") = __EB_ROWCOUNT
                            N3Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            N3Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            N3Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                            N3.Rows.Add(N3Row)


                            Select Case _271_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                Case "ISL"
                                    _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                Case "IRL"
                                    _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"

                                Case "SL"
                                    _271_SL_STRING = _271_SL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                Case "DL"
                                    _271_DL_STRING = _271_DL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                            End Select

                            _RowProcessedFlag = 1


                        End If
                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, __EB_RowRecordType, ex)
                    End Try





                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '    EB ::  LS ::  N4
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If __EB_RowRecordType = "N4" Then


                            Dim N4Row As DataRow = N4.NewRow
                            N4Row("DOCUMENT_ID") = _DOCUMENT_ID
                            N4Row("FILE_ID") = _FILE_ID
                            N4Row("BATCH_ID") = _BATCH_ID
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
                            N4Row("271_ISL_GUID") = _271_ISL_GUID
                            N4Row("271_IRL_GUID") = _271_IRL_GUID
                            N4Row("271_SL_GUID") = _271_SL_GUID
                            N4Row("271_DL_GUID") = _271_DL_GUID
                            N4Row("271_EB_GROUP_GUID") = __EB_GROUP_GUID
                            N4Row("271_EB_GUID") = __EB_GUID
                            N4Row("271_LS_GUID") = __LS_GUID
                            N4Row("NM1_GUID") = _NM1_GUID
                            N4Row("HL01") = _HL01
                            N4Row("HL02") = _HL02
                            N4Row("HL03") = _HL03
                            N4Row("HL04") = _HL04



                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 2) <> "") Then N4Row("N401") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 2) Else N4Row("N401") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 3) <> "") Then N4Row("N402") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 3) Else N4Row("N402") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 4) <> "") Then N4Row("N403") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 4) Else N4Row("N403") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 5) <> "") Then N4Row("N404") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 5) Else N4Row("N404") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 6) <> "") Then N4Row("N405") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 6) Else N4Row("N405") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 7) <> "") Then N4Row("N406") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 7) Else N4Row("N406") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 8) <> "") Then N4Row("N407") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 8) Else N4Row("N407") = DBNull.Value




                            N4Row("ROW_NUMBER") = __ORD_RowCount
                            N4Row("EB_ROW_NUMBER") = __EB_ROWCOUNT
                            N4Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            N4Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            N4Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                            N4.Rows.Add(N4Row)

                            _RowProcessedFlag = 1


                            Select Case _271_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                Case "ISL"
                                    _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                Case "IRL"
                                    _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"

                                Case "SL"
                                    _271_SL_STRING = _271_SL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                Case "DL"
                                    _271_DL_STRING = _271_DL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                            End Select



                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, __EB_RowRecordType, ex)
                    End Try



                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '    EB ::  LS :: PER
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If __EB_RowRecordType = "PER" Then

                            _RowProcessedFlag = 1

                            Dim PERRow As DataRow = PER.NewRow
                            PERRow("DOCUMENT_ID") = _DOCUMENT_ID
                            PERRow("FILE_ID") = _FILE_ID
                            PERRow("BATCH_ID") = _BATCH_ID
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
                            PERRow("271_ISL_GUID") = _271_ISL_GUID
                            PERRow("271_IRL_GUID") = _271_IRL_GUID
                            PERRow("271_SL_GUID") = _271_SL_GUID
                            PERRow("271_DL_GUID") = _271_DL_GUID
                            PERRow("271_EB_GROUP_GUID") = __EB_GROUP_GUID
                            PERRow("271_EB_GUID") = __EB_GUID
                            PERRow("271_LS_GUID") = __LS_GUID
                            PERRow("NM1_GUID") = _NM1_GUID
                            PERRow("HL01") = _HL01
                            PERRow("HL02") = _HL02
                            PERRow("HL03") = _HL03
                            PERRow("HL04") = _HL04

                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 2) <> "") Then PERRow("PER01") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 2) Else PERRow("PER01") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 3) <> "") Then PERRow("PER02") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 3) Else PERRow("PER02") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 4) <> "") Then PERRow("PER03") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 4) Else PERRow("PER03") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 5) <> "") Then PERRow("PER04") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 5) Else PERRow("PER04") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 6) <> "") Then PERRow("PER05") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 6) Else PERRow("PER05") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 7) <> "") Then PERRow("PER06") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 7) Else PERRow("PER06") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 8) <> "") Then PERRow("PER07") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 8) Else PERRow("PER07") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 9) <> "") Then PERRow("PER08") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 9) Else PERRow("PER08") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 10) <> "") Then PERRow("PER09") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 10) Else PERRow("PER09") = DBNull.Value


                            PERRow("ROW_NUMBER") = __ORD_RowCount
                            PERRow("EB_ROW_NUMBER") = __EB_ROWCOUNT
                            PERRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            PERRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            PERRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                            PER.Rows.Add(PERRow)


                            Select Case _271_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                Case "ISL"
                                    _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                Case "IRL"
                                    _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"

                                Case "SL"
                                    _271_SL_STRING = _271_SL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                Case "DL"
                                    _271_DL_STRING = _271_DL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                            End Select



                        End If
                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, __EB_RowRecordType, ex)
                    End Try


                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '    EB ::  LS ::  PRV
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    Try
                        If __EB_RowRecordType = "PRV" Then
                            _RowProcessedFlag = 1

                            Dim PRVRow As DataRow = PRV.NewRow
                            PRVRow("FILE_ID") = _FILE_ID
                            PRVRow("BATCH_ID") = _BATCH_ID
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
                            PRVRow("271_ISL_GUID") = _271_ISL_GUID
                            PRVRow("271_IRL_GUID") = _271_IRL_GUID
                            PRVRow("271_SL_GUID") = _271_SL_GUID
                            PRVRow("271_DL_GUID") = _271_DL_GUID
                            PRVRow("271_EB_GROUP_GUID") = __EB_GROUP_GUID
                            PRVRow("271_EB_GUID") = __EB_GUID
                            PRVRow("271_LS_GUID") = __LS_GUID
                            PRVRow("NM1_GUID") = _NM1_GUID
                            PRVRow("HL01") = _HL01
                            PRVRow("HL02") = _HL02
                            PRVRow("HL03") = _HL03
                            PRVRow("HL04") = _HL04

                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 2) <> "") Then PRVRow("PRV01") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 2) Else PRVRow("PRV01") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 3) <> "") Then PRVRow("PRV02") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 3) Else PRVRow("PRV02") = DBNull.Value
                            If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 4) <> "") Then PRVRow("PRV03") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 4) Else PRVRow("PRV03") = DBNull.Value

                            PRVRow("ROW_NUMBER") = __ORD_RowCount
                            PRVRow("EB_ROW_NUMBER") = __EB_ROWCOUNT
                            PRVRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            PRVRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            PRVRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                            PRV.Rows.Add(PRVRow)



                            Select Case _271_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                Case "ISL"
                                    _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                Case "IRL"
                                    _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"

                                Case "SL"
                                    _271_SL_STRING = _271_SL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                Case "DL"
                                    _271_DL_STRING = _271_DL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                            End Select

                        End If
                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, __EB_RowRecordType, ex)
                    End Try


                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   EB ::  LS ::   LE
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If __EB_RowRecordType = "LE" Then



                            _NM1_GUID = Guid.Empty


                            _RowProcessedFlag = 1



                            Dim LERow As DataRow = LE.NewRow
                            LERow("DOCUMENT_ID") = _DOCUMENT_ID
                            LERow("FILE_ID") = _FILE_ID
                            LERow("BATCH_ID") = _BATCH_ID
                            LERow("ISA_ID") = _ISA_ID
                            LERow("GS_ID") = _GS_ID
                            LERow("ST_ID") = _ST_ID
                            LERow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            LERow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            LERow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            LERow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            LERow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            LERow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            LERow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            LERow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            LERow("271_ISL_GUID") = _271_ISL_GUID
                            LERow("271_IRL_GUID") = _271_IRL_GUID
                            LERow("271_SL_GUID") = _271_SL_GUID
                            LERow("271_DL_GUID") = _271_DL_GUID
                            LERow("271_EB_GROUP_GUID") = __EB_GROUP_GUID
                            LERow("271_EB_GUID") = __EB_GUID
                            LERow("271_LS_GUID") = __LS_GUID
                            LERow("NM1_GUID") = _NM1_GUID
                            LERow("HL01") = _HL01
                            LERow("HL02") = _HL02
                            LERow("HL03") = _HL03
                            LERow("HL04") = _HL04

                            ' If (ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 2) <> "") Then LERow("LE01") = ss.ParseDemlimtedString(__EB_CurrentRowData, _DataElementSeparator, 2) Else LERow("LE01") = DBNull.Value


                            LERow("ROW_NUMBER") = __ORD_RowCount
                            LERow("EB_ROW_NUMBER") = __EB_ROWCOUNT
                            LERow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            LERow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            LERow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                            LE.Rows.Add(LERow)


                            Select Case _271_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                Case "ISL"
                                    _271_ISL_STRING = _271_ISL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                Case "IRL"
                                    _271_IRL_STRING = _271_IRL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"

                                Case "SL"
                                    _271_SL_STRING = _271_SL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                                Case "DL"
                                    _271_DL_STRING = _271_DL_STRING + Convert.ToString(__EB_ROWCOUNT) + "::" + __EB_CurrentRowData + "~"
                            End Select

                            __LS_GUID = Guid.Empty
                            _NM1_GUID = Guid.Empty


                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, __EB_RowRecordType, ex)
                    End Try

                    'LE




                Next

            Loop

                _EB_DIRTY = False

                Return r


        End Function

        Private Function MSGSmasherThingy(MSGList As List(Of String)) As List(Of String)

            _FUNCTION_NAME = "MSGSmasherThingy(MSGList As List(Of String)) As String"


            Dim MSG_RowRecordType As String = String.Empty
            Dim MSG_CurrentRowData As String = String.Empty
            Dim MSG_STRING As String = String.Empty

            Dim MSG01 As String = String.Empty
            Dim MSG02 As String = String.Empty
            Dim MSG02_NEW_LINE_COUNT As Integer = 0
            Dim MSG03 As String = String.Empty
            Dim MSG04 As String = String.Empty
            Dim MSG05 As String = String.Empty
            Dim MSG06 As String = String.Empty


            Dim __MSGList As List(Of String)
            Dim ___MSGList As New List(Of String)


            __MSGList = MSGList
            For Each line As String In __MSGList
                Dim MLINE As String = String.Empty

                MLINE = ss.ParseDemlimtedStringEDI(line, "`", 2)

                MSG_RowRecordType = ss.ParseDemlimtedStringEDI(MLINE, _DataElementSeparator, 1)
                MSG_CurrentRowData = MLINE

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   MSG :: MSG
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If MSG_RowRecordType = "MSG" Then

                        '  _RowProcessedFlag = 1






                        If (ss.ParseDemlimtedString(MSG_CurrentRowData, _DataElementSeparator, 2) <> "") Then

                            MSG01 = ss.ParseDemlimtedString(MSG_CurrentRowData, _DataElementSeparator, 2)

                        End If



                        If (ss.ParseDemlimtedString(MSG_CurrentRowData, _DataElementSeparator, 3) <> "") Then

                            MSG02 = ss.ParseDemlimtedString(MSG_CurrentRowData, _DataElementSeparator, 3)

                        End If

                        If Regex.IsMatch(MSG02, "^[0-9 ]+$") Then

                            MSG02_NEW_LINE_COUNT = Convert.ToInt32(MSG02)

                        End If


                        If (ss.ParseDemlimtedString(MSG_CurrentRowData, _DataElementSeparator, 4) <> "") Then

                            MSG03 = ss.ParseDemlimtedString(MSG_CurrentRowData, _DataElementSeparator, 4)
                        End If


                        MSG_STRING = MSG01
                        If MSG02_NEW_LINE_COUNT > 0 Then


                            For I As Integer = 1 To MSG02_NEW_LINE_COUNT

                                MSG_STRING = MSG_STRING + vbNewLine

                            Next

                            MSG_STRING = MSG_STRING + MSG03
                        End If

                        ___MSGList.Add(MSG_STRING)

                    End If
                Catch ex As Exception

                    MSG_STRING = "MSG SMASHER THINGY BLEW UP ERROR"
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                End Try

            Next
            Return ___MSGList

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
                        cmd.Parameters.AddWithValue("@HIPAA_271_ISA", ISA)
                        cmd.Parameters.Add("@ISA_ID", Data.SqlDbType.BigInt, 1)
                        cmd.Parameters("@ISA_ID").Direction = ParameterDirection.Output
                        cmd.ExecuteNonQuery()

                        _ISA_ID = Convert.ToInt32(cmd.Parameters("@ISA_ID").Value.ToString())
                    End Using
                    Con.Close()
                End Using
                r = 0
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
                        cmd.Parameters.AddWithValue("@HIPAA_271_GS", GS)

                        cmd.Parameters.Add("@GS_ID", Data.SqlDbType.BigInt, 1)
                        cmd.Parameters("@GS_ID").Direction = ParameterDirection.Output

                        cmd.ExecuteNonQuery()

                        _GS_ID = Convert.ToInt32(cmd.Parameters("@GS_ID").Value.ToString())


                    End Using
                    Con.Close()
                End Using
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
                        cmd.Parameters.AddWithValue("@HIPAA_271_ST", ST)
                        cmd.Parameters.AddWithValue("@HIPAA_271_BHT", BHT)
                        cmd.Parameters.Add("@ST_ID", Data.SqlDbType.BigInt, 1)
                        cmd.Parameters("@ST_ID").Direction = ParameterDirection.Output
                        cmd.ExecuteNonQuery()

                        _ST_ID = Convert.ToInt32(cmd.Parameters("@ST_ID").Value.ToString())
                    End Using
                    Con.Close()
                End Using
                r = 0

            Catch ex As Exception

                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _SP_COMIT_2000A_DATA, ex)
            End Try

            Return r

        End Function



        Private Function COMIT2000AHeaderDump() As Integer

            Dim r As Integer = -1

            _FUNCTION_NAME = "COMIT2000AHeaderDump()"


            Try


                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_COMIT_2000A_DATA, Con)

                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.AddWithValue("@HIPAA_271_HL", ISL_HL)

                        cmd.Parameters.AddWithValue("@HIPAA_271_AAA", ISL_AAA)
                        cmd.Parameters.AddWithValue("@HIPAA_271_NM1", ISL_NM1)
                        cmd.Parameters.AddWithValue("@HIPAA_271_PER", ISL_PER)


                        cmd.Parameters.AddWithValue("@RAW_HEADER", _RAW_HEADER)
                        cmd.Parameters.AddWithValue("@RAW_ISL", _271_ISL_STRING)


                        cmd.ExecuteNonQuery()


                    End Using
                    Con.Close()
                    '   Clear2000A()
                End Using
                r = 0

            Catch ex As Exception
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _SP_COMIT_2000A_DATA, ex)

            End Try





            Return r



        End Function



        Private Function COMIT2000BHeaderDump() As Integer

            Dim r As Integer = -1

            _FUNCTION_NAME = "COMIT2000BHeaderDump()"


            Try


                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_COMIT_2000B_DATA, Con)

                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.AddWithValue("@HIPAA_271_HL", IRL_HL)

                        cmd.Parameters.AddWithValue("@HIPAA_271_AAA", IRL_AAA)

                        cmd.Parameters.AddWithValue("@HIPAA_271_NM1", IRL_NM1)
                        cmd.Parameters.AddWithValue("@HIPAA_271_N3", IRL_N3)
                        cmd.Parameters.AddWithValue("@HIPAA_271_N4", IRL_N4)

                        cmd.Parameters.AddWithValue("@HIPAA_271_REF", IRL_REF)

                        cmd.Parameters.AddWithValue("@HIPAA_271_PRV", IRL_PRV)



                        cmd.Parameters.AddWithValue("@RAW_HEADER", _RAW_HEADER)
                        cmd.Parameters.AddWithValue("@RAW_ISL", _271_ISL_STRING)
                        cmd.Parameters.AddWithValue("@RAW_IRL", _271_IRL_STRING)

                        cmd.ExecuteNonQuery()



                    End Using
                    Con.Close()
                End Using
                r = 0
                '  Clear2000B()
            Catch ex As Exception
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _SP_COMIT_2000B_DATA, ex)

            End Try





            Return r



        End Function

        Private Function COMIT_271_22_Dump() As Integer

            Dim r As Integer = -1

            _FUNCTION_NAME = "COMIT_271_22_Dump()"


            Try


                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_COMIT_HL22_DATA, Con)


                        cmd.CommandType = CommandType.StoredProcedure



                        '2000a

                        cmd.Parameters.AddWithValue("@HIPAA_271_ISL_HL", ISL_HL)

                        cmd.Parameters.AddWithValue("@HIPAA_271_ISL_AAA", ISL_AAA)
                        cmd.Parameters.AddWithValue("@HIPAA_271_ISL_NM1", ISL_NM1)
                        cmd.Parameters.AddWithValue("@HIPAA_271_ISL_PER", ISL_PER)







                        '2000b

                        cmd.Parameters.AddWithValue("@HIPAA_271_IRL_HL", IRL_HL)

                        cmd.Parameters.AddWithValue("@HIPAA_271_IRL_AAA", IRL_AAA)

                        cmd.Parameters.AddWithValue("@HIPAA_271_IRL_NM1", IRL_NM1)
                        cmd.Parameters.AddWithValue("@HIPAA_271_IRL_N3", IRL_N3)
                        cmd.Parameters.AddWithValue("@HIPAA_271_IRL_N4", IRL_N4)

                        cmd.Parameters.AddWithValue("@HIPAA_271_IRL_REF", IRL_REF)

                        cmd.Parameters.AddWithValue("@HIPAA_271_IRL_PRV", IRL_PRV)


                        '22

                        cmd.Parameters.AddWithValue("@HIPAA_271_HL", HL)

                        cmd.Parameters.AddWithValue("@HIPAA_271_AAA", AAA)

                        cmd.Parameters.AddWithValue("@HIPAA_271_DMG", DMG)
                        cmd.Parameters.AddWithValue("@HIPAA_271_DTP", DTP)

                        cmd.Parameters.AddWithValue("@HIPAA_271_EB", EB)

                        cmd.Parameters.AddWithValue("@HIPAA_271_III", III)
                        cmd.Parameters.AddWithValue("@HIPAA_271_INS", INS)


                        cmd.Parameters.AddWithValue("@HIPAA_271_HI", HI)
                        cmd.Parameters.AddWithValue("@HIPAA_271_HSD", HSD)

                        cmd.Parameters.AddWithValue("@HIPAA_271_LS", LS)
                        cmd.Parameters.AddWithValue("@HIPAA_271_LE", LE)

                        cmd.Parameters.AddWithValue("@HIPAA_271_MPI", MPI)
                        cmd.Parameters.AddWithValue("@HIPAA_271_MSG", MSG)

                        cmd.Parameters.AddWithValue("@HIPAA_271_NM1", NM1)
                        cmd.Parameters.AddWithValue("@HIPAA_271_N3", N3)
                        cmd.Parameters.AddWithValue("@HIPAA_271_N4", N4)




                        cmd.Parameters.AddWithValue("@HIPAA_271_PER", PER)
                        cmd.Parameters.AddWithValue("@HIPAA_271_PRV", PRV)

                        cmd.Parameters.AddWithValue("@HIPAA_271_REF", REF)


                        cmd.Parameters.AddWithValue("@HIPAA_271_TRN", TRN)


                        cmd.Parameters.AddWithValue("@RAW_HEADER", _RAW_HEADER)
                        cmd.Parameters.AddWithValue("@RAW_ISL", _271_ISL_STRING)
                        cmd.Parameters.AddWithValue("@RAW_IRL", _271_IRL_STRING)
                        cmd.Parameters.AddWithValue("@RAW_SL", _271_SL_STRING)
                        cmd.Parameters.AddWithValue("@RAW_DL", _271_DL_STRING)



                        cmd.Parameters.AddWithValue("@request_xml", _RAW_EDI_EX)
                        cmd.Parameters.AddWithValue("@DELETE_FLAG", _DELETE_FLAG)
                        cmd.Parameters.AddWithValue("@ebr_id", _EBR_ID)
                        cmd.Parameters.AddWithValue("@batch_id", _BATCH_ID)
                        cmd.Parameters.AddWithValue("@user_id", _USER_ID)
                        cmd.Parameters.AddWithValue("@hosp_code", _HOSP_CODE)
                        cmd.Parameters.AddWithValue("@source", _SOURCE)
                        cmd.Parameters.AddWithValue("@EDI", _RAW_EDI)
                        cmd.Parameters.AddWithValue("@PAYOR_ID", _PAYOR_ID)
                        cmd.Parameters.AddWithValue("@Vendor_name", _VENDOR_NAME)
                        cmd.Parameters.AddWithValue("@Log_EDI", _LOG_EDI)

                        'cmd.Parameters.AddWithValue("@EDI", _RAW_EDI)
                        'cmd.Parameters.AddWithValue("@PAYOR_ID", _PAYOR_ID)
                        'cmd.Parameters.AddWithValue("@Vendor_name", _VENDOR_NAME)

                        cmd.Parameters.Add("@Status", Data.SqlDbType.VarChar, 20)
                        cmd.Parameters("@Status").Direction = ParameterDirection.Output

                        cmd.Parameters.Add("@Reject_Reason_code", Data.SqlDbType.VarChar, 10)
                        cmd.Parameters("@Reject_Reason_code").Direction = ParameterDirection.Output

                        cmd.Parameters.Add("@LOOP_AGAIN", Data.SqlDbType.VarChar, 1)
                        cmd.Parameters("@LOOP_AGAIN").Direction = ParameterDirection.Output







                        cmd.ExecuteNonQuery()

                        _STATUS = cmd.Parameters("@Status").Value.ToString()
                        _REJECT_REASON_CODE = cmd.Parameters("@Reject_Reason_code").Value.ToString()
                        _LOOP_AGAIN = cmd.Parameters("@LOOP_AGAIN").Value.ToString()



                    End Using
                    Con.Close()
                End Using

                Clear22Dump()
                r = 0

            Catch ex As Exception
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _SP_COMIT_HL22_DATA, ex)

            End Try





            Return r



        End Function



        Private Function ComitSE() As Int32
            Dim r As Integer
            _FUNCTION_NAME = "ComitSE()"
            Try
                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_SE, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_271_SE", SE_N)
                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using


            Catch ex As Exception
                ' log.ExceptionDetails("COMIT_SE_DATA 005010X221A1", ex)
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _SP_SE, ex)
            End Try

            Return r

        End Function






        Private Function ComitGE() As Integer
            Dim i As Integer

            _FUNCTION_NAME = "ComitGE()"
            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_GE, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_271_GE", GE_N)

                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using

            Catch ex As Exception
                'log.ExceptionDetails("COMIT_GE_DATA 005010X221A1", ex)
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _SP_GE, ex)
            End Try

            Return i

        End Function







        Private Function ComitIEA() As Integer


            Dim i As Integer

            _FUNCTION_NAME = "ComitIEA()"
            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_IEA, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_271_IEA", IEA_N)
                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using

                i = 0

            Catch ex As Exception
                'log.ExceptionDetails("COMIT_IEA_DATA 005010X221A1", ex)
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _SP_IEA, ex)
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

            ISL_AAA.Clear()
            ISL_NM1.Clear()
            ISL_PER.Clear()


            _271_ISL_STRING = String.Empty

            Clear2000B()

        End Sub


        Private Sub Clear2000B()

            IRL_HL.Clear()

            IRL_AAA.Clear()

            IRL_NM1.Clear()
            IRL_N3.Clear()
            IRL_N4.Clear()

            IRL_REF.Clear()
            IRL_PRV.Clear()


            _271_IRL_STRING = String.Empty

            Clear22Dump()

        End Sub

        Private Sub Clear22Dump()

            HL.Clear()
            AAA.Clear()
            DMG.Clear()
            DTP.Clear()
            EB.Clear()
            III.Clear()
            INS.Clear()

            HI.Clear()
            HSD.Clear()

            LS.Clear()
            LE.Clear()

            MPI.Clear()
            MSG.Clear()


            NM1.Clear()
            N3.Clear()
            N4.Clear()

            PER.Clear()
            PRV.Clear()


            REF.Clear()

            TRN.Clear()

            _271_SL_STRING = String.Empty
            _271_DL_STRING = String.Empty

        End Sub

        Private Function ComitUNK() As Integer


            Dim i As Integer

            _FUNCTION_NAME = "ComitUNK()"
            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_COMIT_UNKNOWN, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_5010_UNK", UNK)
                        cmd.Parameters.AddWithValue("@IMPORTER", "EDI_5010_271005010X221A1")
                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using

                i = 0

            Catch ex As Exception
                _hasERR = True
                _ERROR = ex.Message
                'log.ExceptionDetails("COMIT_UNK_DATA 005010X221A1", ex)
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _SP_COMIT_UNKNOWN, ex)
            End Try

            Return i

        End Function

        Private Function RollBack() As Integer

            Dim i As Integer

            _FUNCTION_NAME = "RollBack()"

            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_ROLLBACK, Con)

                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.AddWithValue("@FILE_ID", _FILE_ID)
                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using

                i = 0

            Catch ex As Exception
                'log.ExceptionDetails("ROLL_BACK 005010X221A1", ex)
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _SP_ROLLBACK, ex)
            End Try

            Return i



        End Function

        Private Function NM1Lookup(ByVal nm01 As String) As String
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



        Private Function TestSPs() As Integer

            Dim r As Integer = 0

            _FUNCTION_NAME = "TestSPs()"


            Try


                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_COMIT_2000A_UNIT_TEST, Con)

                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.AddWithValue("@HIPAA_271_HL", ISL_HL)

                        cmd.Parameters.AddWithValue("@HIPAA_271_AAA", ISL_AAA)
                        cmd.Parameters.AddWithValue("@HIPAA_271_NM1", ISL_NM1)
                        cmd.Parameters.AddWithValue("@HIPAA_271_PER", ISL_PER)


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

                        cmd.Parameters.AddWithValue("@HIPAA_271_HL", IRL_HL)

                        cmd.Parameters.AddWithValue("@HIPAA_271_AAA", IRL_AAA)

                        cmd.Parameters.AddWithValue("@HIPAA_271_NM1", IRL_NM1)
                        cmd.Parameters.AddWithValue("@HIPAA_271_N3", IRL_N3)
                        cmd.Parameters.AddWithValue("@HIPAA_271_N4", IRL_N4)

                        cmd.Parameters.AddWithValue("@HIPAA_271_REF", IRL_REF)

                        cmd.Parameters.AddWithValue("@HIPAA_271_PRV", IRL_PRV)

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

                        cmd.Parameters.AddWithValue("@HIPAA_271_HL", HL)

                        cmd.Parameters.AddWithValue("@HIPAA_271_AAA", AAA)

                        cmd.Parameters.AddWithValue("@HIPAA_271_DMG", DMG)
                        cmd.Parameters.AddWithValue("@HIPAA_271_DTP", DTP)

                        cmd.Parameters.AddWithValue("@HIPAA_271_EB", EB)

                        cmd.Parameters.AddWithValue("@HIPAA_271_III", III)
                        cmd.Parameters.AddWithValue("@HIPAA_271_INS", INS)


                        cmd.Parameters.AddWithValue("@HIPAA_271_HI", HI)
                        cmd.Parameters.AddWithValue("@HIPAA_271_HSD", HSD)

                        cmd.Parameters.AddWithValue("@HIPAA_271_LS", LS)
                        cmd.Parameters.AddWithValue("@HIPAA_271_LE", LE)

                        cmd.Parameters.AddWithValue("@HIPAA_271_MPI", MPI)
                        cmd.Parameters.AddWithValue("@HIPAA_271_MSG", MSG)

                        cmd.Parameters.AddWithValue("@HIPAA_271_NM1", NM1)
                        cmd.Parameters.AddWithValue("@HIPAA_271_N3", N3)
                        cmd.Parameters.AddWithValue("@HIPAA_271_N4", N4)




                        cmd.Parameters.AddWithValue("@HIPAA_271_PER", PER)
                        cmd.Parameters.AddWithValue("@HIPAA_271_PRV", PRV)

                        cmd.Parameters.AddWithValue("@HIPAA_271_REF", REF)


                        cmd.Parameters.AddWithValue("@HIPAA_271_TRN", TRN)

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








        Private Function ComitEPIC() As Integer

            Dim i As Integer

            If _DEAD_LOCK_COUNT < _DEAD_LOCK_RETRYS Then

                Dim param As New SqlParameter()
                Dim sqlConn As SqlConnection = New SqlConnection
                Dim cmd As SqlCommand
                Dim sqlString As String = String.Empty

                sqlConn.ConnectionString = _ConnectionString
                sqlConn.Open()

                Try





                    sqlString = "usp_eligibility_response_dump_271_epic"




                    cmd = New SqlCommand(sqlString, sqlConn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.CommandTimeout = _CommandTimeOut
                    '   cmd.Parameters.AddWithValue("@HIPPA_ENVELOP", ENVELOP)
                    cmd.Parameters.AddWithValue("@HIPAA_EB", EB)
                    cmd.Parameters.AddWithValue("@HIPAA_AAA", AAA)
                    cmd.Parameters.AddWithValue("@HIPAA_AMT", AMT)
                    cmd.Parameters.AddWithValue("@HIPAA_DMG", DMG)
                    cmd.Parameters.AddWithValue("@HIPAA_BHT", BHT)
                    cmd.Parameters.AddWithValue("@HIPAA_DTP", DTP)
                    '  cmd.Parameters.AddWithValue("@HIPAA_EQ", EQ)
                    cmd.Parameters.AddWithValue("@HIPAA_HSD", HSD)
                    cmd.Parameters.AddWithValue("@HIPAA_III", III)
                    cmd.Parameters.AddWithValue("@HIPAA_INS", INS)
                    cmd.Parameters.AddWithValue("@HIPAA_N3", N3)
                    cmd.Parameters.AddWithValue("@HIPAA_N4", N4)
                    cmd.Parameters.AddWithValue("@HIPAA_MSG", MSG)
                    cmd.Parameters.AddWithValue("@HIPAA_NM1", NM1)
                    cmd.Parameters.AddWithValue("@HIPAA_PER", PER)
                    cmd.Parameters.AddWithValue("@HIPAA_PRV", PRV)
                    cmd.Parameters.AddWithValue("@HIPAA_REF", REF)
                    cmd.Parameters.AddWithValue("@HIPAA_TRN", TRN)
                    cmd.Parameters.AddWithValue("@HIPAA_UNK", UNK)
                    cmd.Parameters.AddWithValue("@ebr_id", _EBR_ID)
                    cmd.Parameters.AddWithValue("@batch_id", _BATCH_ID)
                    cmd.Parameters.AddWithValue("@user_id", _USER_ID)
                    cmd.Parameters.AddWithValue("@hosp_code", _HOSP_CODE)
                    cmd.Parameters.AddWithValue("@source", _SOURCE)
                    '   cmd.Parameters.AddWithValue("@EDI", "ebr_ID=" + _EBR_ID + "|" + "batch_ID=" + _BATCH_ID + "|" + _edi)
                    cmd.Parameters.AddWithValue("@PAYOR_ID", _PAYOR_ID)
                    cmd.Parameters.AddWithValue("@Vendor_name", _VENDOR_NAME)
                    cmd.Parameters.AddWithValue("@Log_EDI", _LOG_EDI)
                    cmd.Parameters.Add("@Status", Data.SqlDbType.VarChar, 20)
                    cmd.Parameters("@Status").Direction = ParameterDirection.Output
                    cmd.Parameters.Add("@Reject_Reason_code", Data.SqlDbType.VarChar, 10)
                    cmd.Parameters("@Reject_Reason_code").Direction = ParameterDirection.Output
                    cmd.Parameters.Add("@LOOP_AGAIN", Data.SqlDbType.VarChar, 1)
                    cmd.Parameters("@LOOP_AGAIN").Direction = ParameterDirection.Output

                    If _DEAD_LOCK_FLAG Then

                        cmd.Parameters.AddWithValue("@DELETE_FLAG", "Y")
                    Else
                        cmd.Parameters.AddWithValue("@DELETE_FLAG", _DELETE_FLAG)
                    End If


                    'If (_isEPIC = True) Then
                    '    Then

                    '    cmd.Parameters.Add("@EPICOutEDIString", Data.SqlDbType.VarChar)
                    '    cmd.Parameters("@EPICOutEDIString").Direction = ParameterDirection.Output

                    'End If


                    '   _ERROR = cmd.ExecuteNonQuery()


                    _EPICOutEDIString = cmd.Parameters("@EPICOutEDIString").Value.ToString()



                    _STATUS = cmd.Parameters("@Status").Value.ToString()
                    _REJECT_REASON_CODE = cmd.Parameters("@Reject_Reason_code").Value.ToString()
                    _LOOP_AGAIN = cmd.Parameters("@LOOP_AGAIN").Value.ToString()

                    i = 0

                Catch sx As SqlException
                    '   log.ExceptionDetails("55-" + _Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import271" + Me.ToString, sx)

                    log.ExceptionDetails("56-Parse.Import271", "Save to db failed Parse sucessful for batchID : " + Convert.ToString(_BATCH_ID), _RAW_EDI, Me.ToString)

                    If sx.Message.Contains("deadlocked") Or sx.Message.Contains("timeout") Then
                        log.ExceptionDetails("57-Parse.Import271", "Dead lock rtrying  " + Convert.ToString(_BATCH_ID) + " Deadlock count  " + Convert.ToString(_DEAD_LOCK_COUNT), _RAW_EDI, Me.ToString)
                        _DEAD_LOCK_FLAG = True
                        _DEAD_LOCK_COUNT = _DEAD_LOCK_COUNT + 1
                        Comit()
                        i = -1
                    End If

                Catch ex As Exception
                    i = -1

                    '   log.ExceptionDetails("58-" + _Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import271 " + Convert.ToString(_BATCH_ID), ex)
                    '

                    '% was deadlocked %' in SQL.Exception. IF Found RERUN with DELETE_FLAG='Y' only DUMP method
                Finally

                    sqlConn.Close()
                End Try




            Else



                log.ExceptionDetails("59-Parse.Import271", "Dead lock count execced giving up on   " + Convert.ToString(_BATCH_ID) + " Deadlock count  " + Convert.ToString(_DEAD_LOCK_COUNT), _RAW_EDI, Me.ToString)
                i = -1

            End If


            Return i

        End Function


        Private Function Comit() As Integer

            Dim i As Integer = -1

            Dim LoopCount As Integer = 0







            If _DEAD_LOCK_COUNT < _DEAD_LOCK_RETRYS Then
                LoopCount = LoopCount + 1
                If _VERBOSE = 1 Then
                    log.ExceptionDetails("49-271 loop count ", "Batch ID  " + Convert.ToString(_BATCH_ID) + " loop count   " + Convert.ToString(LoopCount))
                End If


                Dim param As New SqlParameter()
                Dim sqlConn As SqlConnection = New SqlConnection
                Dim cmd As SqlCommand
                Dim sqlString As String = String.Empty

                sqlConn.ConnectionString = _ConnectionString
                sqlConn.Open()

                '

                Try

                    sqlString = "usp_eligibility_response_dump"





                    cmd = New SqlCommand(sqlString, sqlConn)
                    cmd.CommandTimeout = 90
                    cmd.CommandType = CommandType.StoredProcedure
                    '  cmd.Parameters.AddWithValue("@HIPPA_ENVELOP", ENVELOP)
                    cmd.Parameters.AddWithValue("@HIPAA_EB", EB)
                    cmd.Parameters.AddWithValue("@HIPAA_AAA", AAA)
                    '    cmd.Parameters.AddWithValue("@HIPAA_AMT", AMT)
                    cmd.Parameters.AddWithValue("@HIPAA_DMG", DMG)
                    cmd.Parameters.AddWithValue("@HIPAA_BHT", BHT)
                    cmd.Parameters.AddWithValue("@HIPAA_DTP", DTP)
                    '   cmd.Parameters.AddWithValue("@HIPAA_EQ", EQ)
                    cmd.Parameters.AddWithValue("@HIPAA_HSD", HSD)
                    cmd.Parameters.AddWithValue("@HIPAA_III", III)
                    cmd.Parameters.AddWithValue("@HIPAA_INS", INS)
                    cmd.Parameters.AddWithValue("@HIPAA_N3", N3)
                    cmd.Parameters.AddWithValue("@HIPAA_N4", N4)
                    cmd.Parameters.AddWithValue("@HIPAA_MSG", MSG)
                    cmd.Parameters.AddWithValue("@HIPAA_NM1", NM1)
                    cmd.Parameters.AddWithValue("@HIPAA_PER", PER)
                    cmd.Parameters.AddWithValue("@HIPAA_PRV", PRV)
                    cmd.Parameters.AddWithValue("@HIPAA_REF", REF)
                    cmd.Parameters.AddWithValue("@HIPAA_TRN", TRN)
                    cmd.Parameters.AddWithValue("@HIPAA_UNK", UNK)
                    cmd.Parameters.AddWithValue("@ebr_id", _EBR_ID)
                    cmd.Parameters.AddWithValue("@batch_id", _BATCH_ID)
                    cmd.Parameters.AddWithValue("@user_id", _USER_ID)
                    cmd.Parameters.AddWithValue("@hosp_code", _HOSP_CODE)
                    cmd.Parameters.AddWithValue("@source", _SOURCE)
                    cmd.Parameters.AddWithValue("@EDI", _RAW_EDI)
                    cmd.Parameters.AddWithValue("@PAYOR_ID", _PAYOR_ID)
                    cmd.Parameters.AddWithValue("@Vendor_name", _VENDOR_NAME)
                    cmd.Parameters.AddWithValue("@Log_EDI", _LOG_EDI)
                    cmd.Parameters.Add("@Status", Data.SqlDbType.VarChar, 20)
                    cmd.Parameters("@Status").Direction = ParameterDirection.Output
                    cmd.Parameters.Add("@Reject_Reason_code", Data.SqlDbType.VarChar, 10)
                    cmd.Parameters("@Reject_Reason_code").Direction = ParameterDirection.Output
                    cmd.Parameters.Add("@LOOP_AGAIN", Data.SqlDbType.VarChar, 1)
                    cmd.Parameters("@LOOP_AGAIN").Direction = ParameterDirection.Output

                    If _DEAD_LOCK_FLAG Then
                        cmd.Parameters.AddWithValue("@DELETE_FLAG", "Y")
                    Else
                        cmd.Parameters.AddWithValue("@DELETE_FLAG", _DELETE_FLAG)
                    End If


                    'If (_isEPIC = True) Then
                    '    Then

                    '    cmd.Parameters.Add("@EPICOutEDIString", Data.SqlDbType.VarChar)
                    '    cmd.Parameters("@EPICOutEDIString").Direction = ParameterDirection.Output

                    'End If


                    ''   err = cmd.ExecuteNonQuery()



                    '  log.ExceptionDetails("64-Parse.Import271", " Begin Comit" + Convert.ToString(_EBR_ID))
                    _ERROR = Convert.ToString(cmd.ExecuteNonQuery())
                    '  log.ExceptionDetails("65-Parse.Import271", " end Comit " + Convert.ToString(_EBR_ID) + " " + err)


                    _STATUS = cmd.Parameters("@Status").Value.ToString()
                    _REJECT_REASON_CODE = cmd.Parameters("@Reject_Reason_code").Value.ToString()
                    _LOOP_AGAIN = cmd.Parameters("@LOOP_AGAIN").Value.ToString()

                    i = 0

                Catch sx As SqlException
                    '  log.ExceptionDetails("50-" + _Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import271" + Me.ToString, sx)

                    log.ExceptionDetails("51-Parse.Import271", "Save to db failed Parse sucessful for batchID : " + Convert.ToString(_BATCH_ID), _RAW_EDI, Me.ToString)

                    If sx.Message.Contains("deadlocked") Or sx.Message.Contains("timeout") Then
                        log.ExceptionDetails("52-Parse.Import271", "Dead lock rtrying  " + Convert.ToString(_BATCH_ID) + " Deadlock count  " + Convert.ToString(_DEAD_LOCK_COUNT), _RAW_EDI, Me.ToString)
                        _DEAD_LOCK_FLAG = True
                        _DEAD_LOCK_COUNT = _DEAD_LOCK_COUNT + 1
                        Comit()  'Commented by Mohan - as per Suresh/Manoj - 09152016 
                        i = -1
                    End If

                Catch ex As Exception
                    i = -1

                    '  log.ExceptionDetails("53-" + _Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import271 " + Convert.ToString(_BATCH_ID), ex)
                    '

                    '% was deadlocked %' in SQL.Exception. IF Found RERUN with DELETE_FLAG='Y' only DUMP method
                Finally

                    sqlConn.Close()
                End Try




            Else



                log.ExceptionDetails("54-Parse.Import271", "Dead lock count execced giving up on   " + Convert.ToString(_BATCH_ID) + " Deadlock count  " + Convert.ToString(_DEAD_LOCK_COUNT), _RAW_EDI, Me.ToString)
                i = -1

            End If


            Return i




        End Function











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

        Public WriteOnly Property BatchID As Double

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





        Public WriteOnly Property ebr_id As Double


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


        Public WriteOnly Property source As String

            Set(value As String)
                _SOURCE = value
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



        Public Property EBLoopCount As Integer
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
