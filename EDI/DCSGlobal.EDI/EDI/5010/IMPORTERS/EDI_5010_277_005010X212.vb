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

    Public Class EDI_5010_277_005010X212

        Inherits EDI_5010_277_00510X212_TABLES

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


        Private _DocumentType As String = "277_005010X212"

        Private _SP_ISA As String = "[usp_EDI_5010_HIPAA_277_ISA]"
        Private _SP_IEA As String = "[usp_EDI_5010_HIPAA_277_IEA]"

        Private _SP_GS As String = "[usp_EDI_5010_HIPAA_277_GS]"
        Private _SP_GE As String = "[usp_EDI_5010_HIPAA_277_GE]"

        Private _SP_ST As String = "[usp_EDI_5010_HIPAA_277_ST]"
        Private _SP_SE As String = "[usp_EDI_5010_HIPAA_277_SE]"

        Private _SP_HEADERS As String = "[usp_EDI_5010_HIPAA_277_HEADERS]"

        Private _SP_COMIT_HL20_UNIT_TEST As String = "[usp_EDI_5010_HIPAA_277_20_UNIT_TEST]"
        Private _SP_COMIT_HL22_UNIT_DATA As String = "[usp_EDI_5010_HIPAA_277_22_UNIT_TEST]"


        Private _SP_COMIT_HL20_DATA As String = "[usp_EDI_5010_HIPAA_277_20_HEADER_DUMP]"
        Private _SP_COMIT_HL22_DATA As String = "[usp_EDI_5010_HIPAA_277_22_CLAIM_STATUS_RESPONSE_DUMP]"

        Private _SP_COMIT_UNKNOWN As String = "[usp_EDI_5010_HIPAA_UNKNOWN]"

        Private _SP_ROLLBACK As String = "[usp_EDI_277_ROLLBACK]"


        '      [usp_Claim_status_log_EDI_277_response]


        Private _277_TRN_GUID As Guid = Guid.Empty
        Private _277_STC_GUID As Guid = Guid.Empty
        Private _277_SVC_GUID As Guid = Guid.Empty

        Private _277_P_STC_GUID As Guid = Guid.Empty

        Private _277_ISL_GUID As Guid = Guid.Empty
        Private _277_IRL_GUID As Guid = Guid.Empty
        Private _277_SPL_GUID As Guid = Guid.Empty


        Private _277_CLS_GUID As Guid = Guid.Empty
        Private _277_SLS_GUID As Guid = Guid.Empty

        Private _277_2200D_BEGIN As Boolean = False

        Private MAKE_CLS As Boolean = False

        Private _277_2200E_BEGIN As Boolean = False

        Private _277_2200D_DTP_FOUND As Boolean = False
        Private _277_2200E_DTP_FOUND As Boolean = False

        Private _277_22_CLS_NM1_FOUND As Boolean = False
        Private _277_23_CLS_NM1_FOUND As Boolean = False


        Private isCLS_ON_DISC As Boolean = False
        Private _TRN01 As String = String.Empty

        Private DTP_FOUND As Boolean = False

        Private _STLine As String = String.Empty
        Private _NM01 As String = String.Empty


        Private _STC01 As String = String.Empty
        Private _STC10 As String = String.Empty
        Private _STC11 As String = String.Empty

        Private _SVC01 As String = String.Empty
        Private _SVC06 As String = String.Empty



        Dim _SVC_FOUND As Boolean = False
        Dim _TRN_FOUND As Boolean = False
        Dim _UNX As Boolean = False
        Dim _UNX_FLAG As Boolean = False
        Dim _UNX_STRING As String = String.Empty


        Dim _SE_FOUND As Boolean = False


        Public Sub New()
            If Debugger.IsAttached Then
                _VERBOSE = 1
                _DEBUG_LEVEL = 1
            End If

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


  

        Public WriteOnly Property ConnectionString As String

            Set(value As String)
                _ConnectionString = value
                log.ConnectionString = value
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


        Public WriteOnly Property FILE_ID As Long

            Set(value As Long)
                _FILE_ID = value
            End Set
        End Property

        Public WriteOnly Property BATCH_ID As Long

            Set(value As Long)
                _BATCH_ID = value
            End Set
        End Property

        Public WriteOnly Property DOCUMENT_ID As Long

            Set(value As Long)
                _DOCUMENT_ID = value
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

        Public WriteOnly Property SP_COMIT_ROW_DATA As String

            Set(value As String)
                _SP_COMIT_HL20_DATA = value
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


        Public Function ImportByString(ByVal EDI As String, ByVal BatchID As Double) As Integer


            _IS_STRING = True

            Dim r As Integer = -1



            Dim SP As New StringPrep()
            _BATCH_ID = CLng(BatchID)

            _EDIList = SP.SingleEDI(EDI)

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
                ClearIAS()
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
                e.TransactionSetIdentifierCode = "277"
                e.UpdateFileStatus(CInt(_FILE_ID), "277", "BEGIN PARSE")
            End Using



            For Each line As String In EDIList
                _RowProcessedFlag = 0

                If _DataElementSeparatorFlag = 0 Then
                    _DataElementSeparator = Mid(line, 4, 1)
                    _DataElementSeparatorFlag = 1
                End If

                last = line

                ' _RAW_EDI = _RAW_EDI + line

                line = line.Replace("~", "")
                rowcount = rowcount + 1

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



                        '_ISA_ROW_ID = rowcount
                        ISARow("ROW_NUMBER") = rowcount

                        ISA.Rows.Add(ISARow)



                        _RepetitionSeparator = _ISA11
                        _ComponentElementSeparator = _ISA16

                        _ISA = Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"


                        '  _chars = "RowDataDelimiter: " + _DataElementSeparator + " CarrotDataDelimiter: " + _CarrotDataDelimiter + " ComponentElementSeperator: " + _ISA16_ComponentElementSeparator



                        'o'D.ISAFlag = 1

                        ComitISA()


                        Select Case _HL03
                            Case 0
                                _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            Case 19
                                _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            Case 20
                                _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            Case 21
                                _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            Case Else
                                _RAW_22 = _RAW_22 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                        End Select

                    End If

                Catch ex As Exception
                    _RowProcessedFlag = -1
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

                        'o() 'D.GS_ROW_ID = rowcount

                        GSRow("ROW_NUMBER") = rowcount
                        GS.Rows.Add(GSRow)

                        _GS = Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"

                        ComitGS()


                        Select Case _HL03
                            Case 0
                                _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            Case 19
                                _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            Case 20
                                _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            Case 21
                                _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            Case Else
                                _RAW_22 = _RAW_22 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                        End Select

                    End If

                    'end GS


                Catch ex As Exception
                    _RowProcessedFlag = -1
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



                        STRow("ROW_NUMBER") = rowcount
                        ST.Rows.Add(STRow)



                        _ST = Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"

                        Select Case _HL03
                            Case 0
                                _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            Case 19
                                _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            Case 20
                                _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            Case 21
                                _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            Case Else
                                _RAW_22 = _RAW_22 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                        End Select
                        '   ComitST()
                    End If

                    ' all the rows get made in to a string. 

                Catch ex As Exception
                    _RowProcessedFlag = -1
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
                        BHTRow("ROW_NUMBER") = rowcount

                        BHT.Rows.Add(BHTRow)

                        _BHT = Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"


                        Select Case _HL03
                            Case 0
                                _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            Case 19
                                _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            Case 20
                                _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            Case 21
                                _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            Case Else
                                _RAW_22 = _RAW_22 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                        End Select

                        _RowProcessedFlag = 1
                        ComitST()


                    End If

                Catch ex As Exception
                    _RowProcessedFlag = -1
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "BHT", ex)
                End Try


                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '  BEGIN 277
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


                                _LoopLevelMajor = 2000
                                _LoopLevelMinor = 0
                                _LoopLevelSubFix = "C"

                            Case 20
                                _277_SPL_GUID = Guid.Empty
                                '    _HIPAA_HL_19_GUID = Guid.Empty
                                _LoopLevelMajor = 2000
                                _HL20_DIRTY = True
                                _HIPAA_HL_19_GUID = Guid.Empty
                                _HIPAA_HL_20_GUID = Guid.NewGuid
                                _HIPAA_HL_21_GUID = Guid.Empty
                                _HIPAA_HL_22_GUID = Guid.Empty
                                _HIPAA_HL_23_GUID = Guid.Empty
                                _HIPAA_HL_24_GUID = Guid.Empty
                            Case 21


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

                                '  _277_SPL_GUID = Guid.NewGuid
                                _277_CLS_GUID = Guid.Empty
                                _277_SLS_GUID = Guid.Empty


                                _HIPAA_HL_19_GUID = Guid.Empty

                                _HL22_DIRTY = True
                                _HIPAA_HL_22_GUID = Guid.NewGuid
                                _HIPAA_HL_23_GUID = Guid.Empty
                                _HIPAA_HL_24_GUID = Guid.Empty

                                _LoopLevelMajor = 2000
                                _LoopLevelMinor = 0
                                _LoopLevelSubFix = "D"

                            Case 23

                                _277_CLS_GUID = Guid.Empty
                                _277_SLS_GUID = Guid.Empty

                                '   _277_SPL_GUID = Guid.Empty
                                '  _HIPAA_HL_19_GUID = Guid.Empty
                                _HL22_DIRTY = True
                                _HIPAA_HL_23_GUID = Guid.NewGuid



                                _LoopLevelMajor = 2000
                                _LoopLevelMinor = 0
                                _LoopLevelSubFix = "E"

                            Case 24
                                _HIPAA_HL_24_GUID = Guid.Empty
                                '  _277_SPL_GUID = Guid.Empty
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


                        HLRow("ROW_NUMBER") = rowcount



                        HLRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        HLRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                        HLRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor

                        HL.Rows.Add(HLRow)


                        Select Case _HL03
                            Case 0
                                _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            Case 19
                                _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            Case 20
                                _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            Case 21
                                _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            Case Else
                                _RAW_22 = _RAW_22 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                        End Select


                    End If

                Catch ex As Exception
                    _RowProcessedFlag = -1
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "HL", ex)
                End Try

                If _HL03 < 22 Then

                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   NM1
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "NM1" Then


                            Select Case _HL03
                                Case 0
                                    '_RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case 19
                                    '_RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case 20
                                    ' _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case 22
                                    ' _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case 23
                                    ' _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case Else
                                    ' _RAW_22 = _RAW_22 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            End Select


                            _RowProcessedFlag = 1

                            _NM1_GUID = Guid.NewGuid

                            Dim NM1Row As DataRow = NM1.NewRow
                            NM1Row("DOCUMENT_ID") = _DOCUMENT_ID
                            NM1Row("FILE_ID") = _FILE_ID
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
                            'NM1Row("277_ISL_GUID") = _277_ISL_GUID
                            'NM1Row("277_IRL_GUID") = _277_IRL_GUID
                            'NM1Row("277_SPL_GUID") = _277_SPL_GUID
                            'NM1Row("277_CLS_GUID") = _277_CLS_GUID
                            'NM1Row("277_SLS_GUID") = _277_SLS_GUID

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



                            NM1Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            NM1Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                            NM1Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor



                            NM1.Rows.Add(NM1Row)


                            Select Case _HL03
                                Case 0
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case 19
                                    _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case 20
                                    _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case 21
                                    _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case Else
                                    _RAW_22 = _RAW_22 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            End Select

                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -1
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "NM1", ex)
                    End Try




                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   PER
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "PER" Then

                            _RowProcessedFlag = 1

                            Dim PERRow As DataRow = PER.NewRow
                            PERRow("DOCUMENT_ID") = _DOCUMENT_ID
                            PERRow("FILE_ID") = _FILE_ID
                            PERRow("ISA_ID") = _ISA_ID
                            PERRow("GS_ID") = _GS_ID
                            PERRow("ST_ID") = _ST_ID
                            PERRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            PERRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            PERRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            ' PERRow("HIPAA_HL_19_GUID") = _HIPAA_HL_19_GUID
                            PERRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            PERRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            PERRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            PERRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            PERRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            'PERRow("NM1_GUID") = _NM1_GUID
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


                            PERRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            PERRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            PERRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                            PER.Rows.Add(PERRow)


                            Select Case _HL03
                                Case 0
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case 19
                                    _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case 20
                                    _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case 21
                                    _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case Else
                                    _RAW_22 = _RAW_22 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            End Select



                        End If
                    Catch ex As Exception
                        _RowProcessedFlag = -1
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "PER", ex)
                    End Try


                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   TRN
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try


                        If _RowRecordType = "TRN" Then

                            _RowProcessedFlag = 1



                            Dim TRN01 As String = String.Empty






                            Dim TRNRow As DataRow = TRN.NewRow
                            TRNRow("DOCUMENT_ID") = _DOCUMENT_ID
                            TRNRow("FILE_ID") = _FILE_ID
                            TRNRow("ISA_ID") = _ISA_ID
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
                            TRNRow("277_ISL_GUID") = _277_ISL_GUID
                            TRNRow("277_IRL_GUID") = _277_IRL_GUID
                            TRNRow("277_SPL_GUID") = _277_SPL_GUID
                            TRNRow("277_CLS_GUID") = _277_CLS_GUID
                            TRNRow("277_SLS_GUID") = _277_SLS_GUID
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

                            TRNRow("ROW_NUMBER") = rowcount



                            TRNRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            TRNRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            TRNRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                            TRN.Rows.Add(TRNRow)


                            Select Case _HL03
                                Case 0
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case 19
                                    _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case 20
                                    _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case 21
                                    _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case Else
                                    _RAW_22 = _RAW_22 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            End Select
                            ' bad



                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -1
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "TRN", ex)
                    End Try

                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   STC
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    Try
                        If _RowRecordType = "STC" Then

                            _RowProcessedFlag = 1
                            '_LoopLevelMinor = 220
                            '_277_STC_GUID = Guid.NewGuid

                            _STC01 = String.Empty
                            _STC10 = String.Empty
                            _STC11 = String.Empty

                            Dim STCRow As DataRow = STC.NewRow
                            STCRow("DOCUMENT_ID") = _DOCUMENT_ID
                            STCRow("FILE_ID") = _FILE_ID
                            STCRow("ISA_ID") = _ISA_ID
                            STCRow("GS_ID") = _GS_ID
                            STCRow("ST_ID") = _ST_ID
                            STCRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            STCRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            STCRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            STCRow("HIPAA_HL_19_GUID") = _HIPAA_HL_19_GUID
                            STCRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                            STCRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                            STCRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                            STCRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                            STCRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                            STCRow("277_ISL_GUID") = _277_ISL_GUID
                            STCRow("277_IRL_GUID") = _277_IRL_GUID
                            STCRow("277_SPL_GUID") = _277_SPL_GUID
                            STCRow("277_CLS_GUID") = _277_CLS_GUID
                            STCRow("277_SLS_GUID") = _277_SLS_GUID
                            STCRow("HL01") = _HL01
                            STCRow("HL02") = _HL02
                            STCRow("HL03") = _HL03
                            STCRow("HL04") = _HL04


                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then
                                STCRow("STC01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2)
                                _STC01 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2)
                            Else
                                STCRow("STC01") = DBNull.Value

                            End If

                            If Not _STC01 = String.Empty Then

                                If (ss.ParseDemlimtedString(_STC01, _ComponentElementSeparator, 1) <> "") Then STCRow("STC01_1") = ss.ParseDemlimtedString(_STC01, _ComponentElementSeparator, 1) Else STCRow("STC01_1") = DBNull.Value
                                If (ss.ParseDemlimtedString(_STC01, _ComponentElementSeparator, 2) <> "") Then STCRow("STC01_2") = ss.ParseDemlimtedString(_STC01, _ComponentElementSeparator, 2) Else STCRow("STC01_2") = DBNull.Value
                                If (ss.ParseDemlimtedString(_STC01, _ComponentElementSeparator, 3) <> "") Then STCRow("STC01_3") = ss.ParseDemlimtedString(_STC01, _ComponentElementSeparator, 3) Else STCRow("STC01_3") = DBNull.Value
                                If (ss.ParseDemlimtedString(_STC01, _ComponentElementSeparator, 4) <> "") Then STCRow("STC01_4") = ss.ParseDemlimtedString(_STC01, _ComponentElementSeparator, 4) Else STCRow("STC01_4") = DBNull.Value

                            End If


                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then STCRow("STC02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else STCRow("STC02") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then STCRow("STC03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else STCRow("STC03") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then STCRow("STC04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else STCRow("STC04") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then STCRow("STC05") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else STCRow("STC05") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then STCRow("STC06") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) Else STCRow("STC06") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then STCRow("STC07") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) Else STCRow("STC07") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) <> "") Then STCRow("STC08") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) Else STCRow("STC08") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) <> "") Then STCRow("STC09") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) Else STCRow("STC09") = DBNull.Value




                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11) <> "") Then
                                STCRow("STC10") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11)
                                _STC10 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11)
                            Else
                                STCRow("STC10") = DBNull.Value

                            End If

                            If Not _STC10 = String.Empty Then

                                If (ss.ParseDemlimtedString(_STC10, _ComponentElementSeparator, 1) <> "") Then STCRow("STC10_1") = ss.ParseDemlimtedString(_STC10, _ComponentElementSeparator, 1) Else STCRow("STC10_1") = DBNull.Value
                                If (ss.ParseDemlimtedString(_STC10, _ComponentElementSeparator, 2) <> "") Then STCRow("STC10_2") = ss.ParseDemlimtedString(_STC10, _ComponentElementSeparator, 2) Else STCRow("STC10_2") = DBNull.Value
                                If (ss.ParseDemlimtedString(_STC10, _ComponentElementSeparator, 3) <> "") Then STCRow("STC10_3") = ss.ParseDemlimtedString(_STC10, _ComponentElementSeparator, 3) Else STCRow("STC10_3") = DBNull.Value
                                If (ss.ParseDemlimtedString(_STC10, _ComponentElementSeparator, 4) <> "") Then STCRow("STC10_4") = ss.ParseDemlimtedString(_STC10, _ComponentElementSeparator, 4) Else STCRow("STC10_4") = DBNull.Value

                            End If




                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12) <> "") Then
                                STCRow("STC11") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12)
                                _STC11 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12)
                            Else
                                STCRow("STC11") = DBNull.Value

                            End If

                            If Not _STC11 = String.Empty Then

                                If (ss.ParseDemlimtedString(_STC11, _ComponentElementSeparator, 1) <> "") Then STCRow("STC11_1") = ss.ParseDemlimtedString(_STC11, _ComponentElementSeparator, 1) Else STCRow("STC11_1") = DBNull.Value
                                If (ss.ParseDemlimtedString(_STC11, _ComponentElementSeparator, 2) <> "") Then STCRow("STC11_2") = ss.ParseDemlimtedString(_STC11, _ComponentElementSeparator, 2) Else STCRow("STC11_2") = DBNull.Value
                                If (ss.ParseDemlimtedString(_STC11, _ComponentElementSeparator, 3) <> "") Then STCRow("STC11_3") = ss.ParseDemlimtedString(_STC11, _ComponentElementSeparator, 3) Else STCRow("STC11_3") = DBNull.Value
                                If (ss.ParseDemlimtedString(_STC11, _ComponentElementSeparator, 4) <> "") Then STCRow("STC11_4") = ss.ParseDemlimtedString(_STC11, _ComponentElementSeparator, 4) Else STCRow("STC11_4") = DBNull.Value

                            End If




                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 13) <> "") Then STCRow("STC12") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 13) Else STCRow("STC12") = DBNull.Value


                            'STCRow("FILE_ID") = _File_ID
                            'STCRow("BATCH_ID") = _BATCH_ID
                            'STCRow("GS_ID") = _GS_ID
                            'STCRow("ST_ID") = _ST_ID
                            'STCRow("STC_ID") = _STC_ID


                            STCRow("ROW_NUMBER") = rowcount



                            STCRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            STCRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                            STCRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor


                            STC.Rows.Add(STCRow)


                            Select Case _HL03
                                Case 0
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case 19
                                    _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case 20
                                    _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case 21
                                    _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case Else
                                    _RAW_22 = _RAW_22 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            End Select



                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -1
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "STC", ex)
                    End Try

                End If





                If _RowRecordType = "SE" Then
                    _SE_FOUND = True
                End If

                If _HL03 >= 22 And _SE_FOUND = False Then

                    'If _LoopLevelMajor = 2000 And _LoopLevelMinor < 220 Then

                    '    'If MAKE_CLS = False Then
                    '    '    _277_CLS_GUID = Guid.NewGuid
                    '    '    MAKE_CLS = True

                    '    'End If



                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   NM1
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "NM1" Then
                            '   _277_CLS_GUID = Guid.Empty
                            ' _277_SLS_GUID = Guid.Empty

                            Select Case _HL03
                                Case 0
                                    '_RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case 19
                                    '_RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case 20
                                    ' _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case 22
                                    _277_CLS_GUID = Guid.Empty
                                    _277_SLS_GUID = Guid.Empty
                                    _LoopLevelMajor = 2000

                                    _LoopLevelMinor = 100
                                    _LoopLevelSubFix = "D"

                                Case 23
                                    _277_CLS_GUID = Guid.Empty
                                    _277_SLS_GUID = Guid.Empty
                                    _LoopLevelMajor = 2000

                                    _LoopLevelMinor = 100
                                    _LoopLevelSubFix = "E"
                                    ' _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case Else
                                    ' _RAW_22 = _RAW_22 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            End Select


                            _RowProcessedFlag = 1

                            _NM1_GUID = Guid.NewGuid

                            Dim NM1Row As DataRow = NM1.NewRow
                            NM1Row("DOCUMENT_ID") = _DOCUMENT_ID
                            NM1Row("FILE_ID") = _FILE_ID
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
                            'NM1Row("277_ISL_GUID") = _277_ISL_GUID
                            'NM1Row("277_IRL_GUID") = _277_IRL_GUID
                            NM1Row("277_SPL_GUID") = _277_SPL_GUID
                            NM1Row("277_CLS_GUID") = _277_CLS_GUID
                            NM1Row("277_SLS_GUID") = _277_SLS_GUID

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


                            '    NM1Lookup(_NM01)

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



                            NM1Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            NM1Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                            NM1Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor



                            NM1.Rows.Add(NM1Row)


                            Select Case _HL03
                                Case 0
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case 19
                                    _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case 20
                                    _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case 21
                                    _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                Case Else
                                    _RAW_22 = _RAW_22 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            End Select
                            _UNX = False
                        End If


                    Catch ex As Exception
                        _RowProcessedFlag = -1
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "NM1", ex)
                    End Try





                    If _RowRecordType = "TRN" Then

                        _277_CLS_GUID = Guid.NewGuid
                        _277_SLS_GUID = Guid.Empty
                        _TRN_FOUND = True
                        _SVC_FOUND = False
                        _UNX = False

                    End If

                    If _RowRecordType = "SVC" Then

                        _277_SLS_GUID = Guid.NewGuid
                        _TRN_FOUND = False
                        _SVC_FOUND = True
                        _UNX = False

                    End If




                    If _UNX Then
                        Using em As New Email

                        End Using
                        _UNX_FLAG = True
                        _UNX_STRING = "UNX  ROW RECORD TYPE EDI_5010_277_005010X212 " + Convert.ToString(rowcount) + " ::  " + _RowRecordType + " --- " + _CurrentRowData
                        log.ExceptionDetails("UNX  ROW RECORD TYPE EDI_5010_277_005010X212  " + Convert.ToString(rowcount) + " :: " + _RowRecordType, _CurrentRowData)
                        'Return -1000
                        'Exit Function

                    End If


                    '==================================================================================================================================================
                    If _TRN_FOUND Then

                        '    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        '    '   TRN
                        '    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


                        Try
                            If _RowRecordType = "TRN" Then

                                _RowProcessedFlag = 1



                                Dim TRN01 As String = String.Empty


                                Select Case _HL03
                                    Case 0
                                        '_RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case 19
                                        '_RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case 20
                                        ' _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case 22

                                        _LoopLevelMajor = 2000

                                        _LoopLevelMinor = 200
                                        _LoopLevelSubFix = "D"

                                    Case 23

                                        _LoopLevelMajor = 2000

                                        _LoopLevelMinor = 200
                                        _LoopLevelSubFix = "E"
                                        ' _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case Else
                                        ' _RAW_22 = _RAW_22 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                End Select


                                Dim TRNRow As DataRow = TRN.NewRow
                                TRNRow("DOCUMENT_ID") = _DOCUMENT_ID
                                TRNRow("FILE_ID") = _FILE_ID
                                TRNRow("ISA_ID") = _ISA_ID
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
                                TRNRow("277_ISL_GUID") = _277_ISL_GUID
                                TRNRow("277_IRL_GUID") = _277_IRL_GUID
                                TRNRow("277_SPL_GUID") = _277_SPL_GUID
                                TRNRow("277_CLS_GUID") = _277_CLS_GUID
                                TRNRow("277_SLS_GUID") = _277_SLS_GUID
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

                                TRNRow("ROW_NUMBER") = rowcount



                                TRNRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                TRNRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                                TRNRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                                TRN.Rows.Add(TRNRow)


                                Select Case _HL03
                                    Case 0
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case 19
                                        _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case 20
                                        _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case 21
                                        _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case Else
                                        _RAW_22 = _RAW_22 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                End Select



                            End If


                        Catch ex As Exception
                            _RowProcessedFlag = -1
                            log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "TRN", ex)
                        End Try




                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        '   STC
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                        Try
                            If _RowRecordType = "STC" Then

                                _RowProcessedFlag = 1


                                _STC01 = String.Empty
                                _STC10 = String.Empty
                                _STC11 = String.Empty

                                Dim STCRow As DataRow = STC.NewRow
                                STCRow("DOCUMENT_ID") = _DOCUMENT_ID
                                STCRow("FILE_ID") = _FILE_ID
                                STCRow("ISA_ID") = _ISA_ID
                                STCRow("GS_ID") = _GS_ID
                                STCRow("ST_ID") = _ST_ID
                                STCRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                                STCRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                                STCRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                                STCRow("HIPAA_HL_19_GUID") = _HIPAA_HL_19_GUID
                                STCRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                                STCRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                                STCRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                                STCRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                                STCRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                                STCRow("277_ISL_GUID") = _277_ISL_GUID
                                STCRow("277_IRL_GUID") = _277_IRL_GUID
                                STCRow("277_SPL_GUID") = _277_SPL_GUID
                                STCRow("277_CLS_GUID") = _277_CLS_GUID
                                STCRow("277_SLS_GUID") = _277_SLS_GUID
                                STCRow("HL01") = _HL01
                                STCRow("HL02") = _HL02
                                STCRow("HL03") = _HL03
                                STCRow("HL04") = _HL04


                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then
                                    STCRow("STC01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2)
                                    _STC01 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2)
                                Else
                                    STCRow("STC01") = DBNull.Value

                                End If

                                If Not _STC01 = String.Empty Then

                                    If (ss.ParseDemlimtedString(_STC01, _ComponentElementSeparator, 1) <> "") Then STCRow("STC01_1") = ss.ParseDemlimtedString(_STC01, _ComponentElementSeparator, 1) Else STCRow("STC01_1") = DBNull.Value
                                    If (ss.ParseDemlimtedString(_STC01, _ComponentElementSeparator, 2) <> "") Then STCRow("STC01_2") = ss.ParseDemlimtedString(_STC01, _ComponentElementSeparator, 2) Else STCRow("STC01_2") = DBNull.Value
                                    If (ss.ParseDemlimtedString(_STC01, _ComponentElementSeparator, 3) <> "") Then STCRow("STC01_3") = ss.ParseDemlimtedString(_STC01, _ComponentElementSeparator, 3) Else STCRow("STC01_3") = DBNull.Value
                                    If (ss.ParseDemlimtedString(_STC01, _ComponentElementSeparator, 4) <> "") Then STCRow("STC01_4") = ss.ParseDemlimtedString(_STC01, _ComponentElementSeparator, 4) Else STCRow("STC01_4") = DBNull.Value

                                End If


                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then STCRow("STC02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else STCRow("STC02") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then STCRow("STC03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else STCRow("STC03") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then STCRow("STC04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else STCRow("STC04") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then STCRow("STC05") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else STCRow("STC05") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then STCRow("STC06") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) Else STCRow("STC06") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then STCRow("STC07") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) Else STCRow("STC07") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) <> "") Then STCRow("STC08") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) Else STCRow("STC08") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) <> "") Then STCRow("STC09") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) Else STCRow("STC09") = DBNull.Value




                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11) <> "") Then
                                    STCRow("STC10") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11)
                                    _STC10 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11)
                                Else
                                    STCRow("STC10") = DBNull.Value

                                End If

                                If Not _STC10 = String.Empty Then

                                    If (ss.ParseDemlimtedString(_STC10, _ComponentElementSeparator, 1) <> "") Then STCRow("STC10_1") = ss.ParseDemlimtedString(_STC10, _ComponentElementSeparator, 1) Else STCRow("STC10_1") = DBNull.Value
                                    If (ss.ParseDemlimtedString(_STC10, _ComponentElementSeparator, 2) <> "") Then STCRow("STC10_2") = ss.ParseDemlimtedString(_STC10, _ComponentElementSeparator, 2) Else STCRow("STC10_2") = DBNull.Value
                                    If (ss.ParseDemlimtedString(_STC10, _ComponentElementSeparator, 3) <> "") Then STCRow("STC10_3") = ss.ParseDemlimtedString(_STC10, _ComponentElementSeparator, 3) Else STCRow("STC10_3") = DBNull.Value
                                    If (ss.ParseDemlimtedString(_STC10, _ComponentElementSeparator, 4) <> "") Then STCRow("STC10_4") = ss.ParseDemlimtedString(_STC10, _ComponentElementSeparator, 4) Else STCRow("STC10_4") = DBNull.Value

                                End If




                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12) <> "") Then
                                    STCRow("STC11") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12)
                                    _STC11 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12)
                                Else
                                    STCRow("STC11") = DBNull.Value

                                End If

                                If Not _STC11 = String.Empty Then

                                    If (ss.ParseDemlimtedString(_STC11, _ComponentElementSeparator, 1) <> "") Then STCRow("STC11_1") = ss.ParseDemlimtedString(_STC11, _ComponentElementSeparator, 1) Else STCRow("STC11_1") = DBNull.Value
                                    If (ss.ParseDemlimtedString(_STC11, _ComponentElementSeparator, 2) <> "") Then STCRow("STC11_2") = ss.ParseDemlimtedString(_STC11, _ComponentElementSeparator, 2) Else STCRow("STC11_2") = DBNull.Value
                                    If (ss.ParseDemlimtedString(_STC11, _ComponentElementSeparator, 3) <> "") Then STCRow("STC11_3") = ss.ParseDemlimtedString(_STC11, _ComponentElementSeparator, 3) Else STCRow("STC11_3") = DBNull.Value
                                    If (ss.ParseDemlimtedString(_STC11, _ComponentElementSeparator, 4) <> "") Then STCRow("STC11_4") = ss.ParseDemlimtedString(_STC11, _ComponentElementSeparator, 4) Else STCRow("STC11_4") = DBNull.Value

                                End If




                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 13) <> "") Then STCRow("STC12") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 13) Else STCRow("STC12") = DBNull.Value


                                'STCRow("FILE_ID") = _File_ID
                                'STCRow("BATCH_ID") = _BATCH_ID
                                'STCRow("GS_ID") = _GS_ID
                                'STCRow("ST_ID") = _ST_ID
                                'STCRow("STC_ID") = _STC_ID


                                STCRow("ROW_NUMBER") = rowcount



                                STCRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                STCRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                                STCRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor


                                STC.Rows.Add(STCRow)


                                Select Case _HL03
                                    Case 0
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case 19
                                        _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case 20
                                        _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case 21
                                        _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case Else
                                        _RAW_22 = _RAW_22 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                End Select



                            End If

                        Catch ex As Exception
                            _RowProcessedFlag = -1
                            log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "STC", ex)
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
                                REFRow("277_ISL_GUID") = _277_ISL_GUID
                                REFRow("277_IRL_GUID") = _277_IRL_GUID
                                REFRow("277_SPL_GUID") = _277_SPL_GUID
                                REFRow("277_CLS_GUID") = _277_CLS_GUID
                                REFRow("277_SLS_GUID") = _277_SLS_GUID
                                REFRow("HL01") = _HL01
                                REFRow("HL02") = _HL02
                                REFRow("HL03") = _HL03
                                REFRow("HL04") = _HL04
                                ' REFRow("P_GUID") = _P_GUID


                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then REFRow("REF01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else REFRow("REF01") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then REFRow("REF02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else REFRow("REF02") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then REFRow("REF03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else REFRow("REF03") = DBNull.Value

                                REFRow("ROW_NUMBER") = rowcount

                                REFRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                REFRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                                REFRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                                REF.Rows.Add(REFRow)

                                Select Case _HL03
                                    Case 0
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case 19
                                        _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case 20
                                        _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case 21
                                        _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case Else
                                        _RAW_22 = _RAW_22 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                End Select


                            End If

                        Catch ex As Exception
                            log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "REF", ex)
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
                                DTPRow("277_ISL_GUID") = _277_ISL_GUID
                                DTPRow("277_IRL_GUID") = _277_IRL_GUID
                                DTPRow("277_SPL_GUID") = _277_SPL_GUID
                                DTPRow("277_CLS_GUID") = _277_CLS_GUID
                                DTPRow("277_SLS_GUID") = _277_SLS_GUID
                                DTPRow("HL01") = _HL01
                                DTPRow("HL02") = _HL02
                                DTPRow("HL03") = _HL03
                                DTPRow("HL04") = _HL04

                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then DTPRow("DTP01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else DTPRow("DTP01") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then DTPRow("DTP02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else DTPRow("DTP02") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then DTPRow("DTP03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else DTPRow("DTP03") = DBNull.Value
                                '    If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then DTPRow("DTP04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else DTPRow("DTP04") = DBNull.Value


                                DTPRow("ROW_NUMBER") = rowcount
                                DTPRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                DTPRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                                DTPRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                                DTP.Rows.Add(DTPRow)





                                Select Case _HL03
                                    Case 0
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case 19
                                        _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case 20
                                        _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case 21
                                        _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"

                                    Case Else
                                        _RAW_22 = _RAW_22 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                End Select

                                _TRN_FOUND = False
                                _UNX = True

                            End If



                        Catch ex As Exception
                            _RowProcessedFlag = -1
                            log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "DTP", ex)
                        End Try




                    End If






                    '==================================================================================================================================================
                    If _SVC_FOUND Then

                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        '   SVC
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        Try

                            If _RowRecordType = "SVC" Then

                                _RowProcessedFlag = 1

                                '  _277_SVC_GUID = Guid.NewGuid

                                Dim SVCRow As DataRow = SVC.NewRow

                                SVCRow("DOCUMENT_ID") = _DOCUMENT_ID
                                SVCRow("FILE_ID") = _FILE_ID
                                SVCRow("ISA_ID") = _ISA_ID
                                SVCRow("GS_ID") = _GS_ID
                                SVCRow("ST_ID") = _ST_ID
                                SVCRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                                SVCRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                                SVCRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                                SVCRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                                SVCRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                                SVCRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                                SVCRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                                SVCRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                                SVCRow("277_ISL_GUID") = _277_ISL_GUID
                                SVCRow("277_IRL_GUID") = _277_IRL_GUID
                                SVCRow("277_SPL_GUID") = _277_SPL_GUID
                                SVCRow("277_CLS_GUID") = _277_CLS_GUID
                                SVCRow("277_SLS_GUID") = _277_SLS_GUID
                                SVCRow("HL01") = _HL01
                                SVCRow("HL02") = _HL02
                                SVCRow("HL03") = _HL03
                                SVCRow("HL04") = _HL04


                                _SVC01 = String.Empty
                                _SVC06 = String.Empty






                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then
                                    SVCRow("SVC01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2)
                                    _SVC01 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2)
                                Else
                                    SVCRow("SVC01") = DBNull.Value

                                End If
                                If Not _SVC01 = String.Empty Then

                                    If (ss.ParseDemlimtedString(_SVC01, _ComponentElementSeparator, 1) <> "") Then SVCRow("SVC01_1") = ss.ParseDemlimtedString(_SVC01, _ComponentElementSeparator, 1) Else SVCRow("SVC01_1") = DBNull.Value
                                    If (ss.ParseDemlimtedString(_SVC01, _ComponentElementSeparator, 2) <> "") Then SVCRow("SVC01_2") = ss.ParseDemlimtedString(_SVC01, _ComponentElementSeparator, 2) Else SVCRow("SVC01_2") = DBNull.Value
                                    If (ss.ParseDemlimtedString(_SVC01, _ComponentElementSeparator, 3) <> "") Then SVCRow("SVC01_3") = ss.ParseDemlimtedString(_SVC01, _ComponentElementSeparator, 3) Else SVCRow("SVC01_3") = DBNull.Value
                                    If (ss.ParseDemlimtedString(_SVC01, _ComponentElementSeparator, 4) <> "") Then SVCRow("SVC01_4") = ss.ParseDemlimtedString(_SVC01, _ComponentElementSeparator, 4) Else SVCRow("SVC01_4") = DBNull.Value
                                    If (ss.ParseDemlimtedString(_SVC01, _ComponentElementSeparator, 5) <> "") Then SVCRow("SVC01_5") = ss.ParseDemlimtedString(_SVC01, _ComponentElementSeparator, 5) Else SVCRow("SVC01_5") = DBNull.Value
                                    If (ss.ParseDemlimtedString(_SVC01, _ComponentElementSeparator, 6) <> "") Then SVCRow("SVC01_6") = ss.ParseDemlimtedString(_SVC01, _ComponentElementSeparator, 6) Else SVCRow("SVC01_6") = DBNull.Value
                                    If (ss.ParseDemlimtedString(_SVC01, _ComponentElementSeparator, 7) <> "") Then SVCRow("SVC01_7") = ss.ParseDemlimtedString(_SVC01, _ComponentElementSeparator, 7) Else SVCRow("SVC01_7") = DBNull.Value

                                End If

                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then SVCRow("SVC02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else SVCRow("SVC02") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then SVCRow("SVC03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else SVCRow("SVC03") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then SVCRow("SVC04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else SVCRow("SVC04") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then SVCRow("SVC05") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else SVCRow("SVC05") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then SVCRow("SVC06") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 14) Else SVCRow("SVC06") = DBNull.Value

                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then
                                    SVCRow("SVC06") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7)
                                    _SVC06 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7)
                                Else
                                    SVCRow("SVC06") = DBNull.Value

                                End If

                                If (ss.ParseDemlimtedString(_SVC06, _ComponentElementSeparator, 1) <> "") Then SVCRow("SVC06_1") = ss.ParseDemlimtedString(_SVC06, _ComponentElementSeparator, 1) Else SVCRow("SVC06_1") = DBNull.Value
                                If (ss.ParseDemlimtedString(_SVC06, _ComponentElementSeparator, 2) <> "") Then SVCRow("SVC06_2") = ss.ParseDemlimtedString(_SVC06, _ComponentElementSeparator, 2) Else SVCRow("SVC06_2") = DBNull.Value
                                If (ss.ParseDemlimtedString(_SVC06, _ComponentElementSeparator, 3) <> "") Then SVCRow("SVC06_3") = ss.ParseDemlimtedString(_SVC06, _ComponentElementSeparator, 3) Else SVCRow("SVC06_3") = DBNull.Value
                                If (ss.ParseDemlimtedString(_SVC06, _ComponentElementSeparator, 4) <> "") Then SVCRow("SVC06_4") = ss.ParseDemlimtedString(_SVC06, _ComponentElementSeparator, 4) Else SVCRow("SVC06_4") = DBNull.Value
                                If (ss.ParseDemlimtedString(_SVC06, _ComponentElementSeparator, 5) <> "") Then SVCRow("SVC06_5") = ss.ParseDemlimtedString(_SVC06, _ComponentElementSeparator, 5) Else SVCRow("SVC06_5") = DBNull.Value
                                If (ss.ParseDemlimtedString(_SVC06, _ComponentElementSeparator, 6) <> "") Then SVCRow("SVC06_6") = ss.ParseDemlimtedString(_SVC06, _ComponentElementSeparator, 6) Else SVCRow("SVC06_6") = DBNull.Value
                                If (ss.ParseDemlimtedString(_SVC06, _ComponentElementSeparator, 7) <> "") Then SVCRow("SVC06_7") = ss.ParseDemlimtedString(_SVC06, _ComponentElementSeparator, 7) Else SVCRow("SVC06_7") = DBNull.Value

                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then SVCRow("SVC07") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) Else SVCRow("SVC07") = DBNull.Value





                                'SVCRow("FILE_ID") = _File_ID
                                'SVCRow("BATCH_ID") = _BATCH_ID
                                'SVCRow("GS_ID") = _GS_ID
                                'SVCRow("ST_ID") = _ST_ID
                                'SVCRow("STC_ID") = _STC_ID

                                SVCRow("ROW_NUMBER") = rowcount

                                SVCRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                SVCRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                                SVCRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                                SVC.Rows.Add(SVCRow)


                                Select Case _HL03
                                    Case 0
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case 19
                                        _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case 20
                                        _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case 21
                                        _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case Else
                                        _RAW_22 = _RAW_22 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                End Select


                            End If
                        Catch ex As Exception
                            _RowProcessedFlag = -1
                            log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "REF", ex)
                        End Try


                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        '   STC
                        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                        Try
                            If _RowRecordType = "STC" Then

                                _RowProcessedFlag = 1

                                _STC01 = String.Empty
                                _STC10 = String.Empty
                                _STC11 = String.Empty

                                Dim STCRow As DataRow = STC.NewRow
                                STCRow("DOCUMENT_ID") = _DOCUMENT_ID
                                STCRow("FILE_ID") = _FILE_ID
                                STCRow("ISA_ID") = _ISA_ID
                                STCRow("GS_ID") = _GS_ID
                                STCRow("ST_ID") = _ST_ID
                                STCRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                                STCRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                                STCRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                                STCRow("HIPAA_HL_19_GUID") = _HIPAA_HL_19_GUID
                                STCRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                                STCRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                                STCRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                                STCRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                                STCRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                                STCRow("277_ISL_GUID") = _277_ISL_GUID
                                STCRow("277_IRL_GUID") = _277_IRL_GUID
                                STCRow("277_SPL_GUID") = _277_SPL_GUID
                                STCRow("277_CLS_GUID") = _277_CLS_GUID
                                STCRow("277_SLS_GUID") = _277_SLS_GUID
                                STCRow("HL01") = _HL01
                                STCRow("HL02") = _HL02
                                STCRow("HL03") = _HL03
                                STCRow("HL04") = _HL04


                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then
                                    STCRow("STC01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2)
                                    _STC01 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2)
                                Else
                                    STCRow("STC01") = DBNull.Value

                                End If

                                If Not _STC01 = String.Empty Then

                                    If (ss.ParseDemlimtedString(_STC01, _ComponentElementSeparator, 1) <> "") Then STCRow("STC01_1") = ss.ParseDemlimtedString(_STC01, _ComponentElementSeparator, 1) Else STCRow("STC01_1") = DBNull.Value
                                    If (ss.ParseDemlimtedString(_STC01, _ComponentElementSeparator, 2) <> "") Then STCRow("STC01_2") = ss.ParseDemlimtedString(_STC01, _ComponentElementSeparator, 2) Else STCRow("STC01_2") = DBNull.Value
                                    If (ss.ParseDemlimtedString(_STC01, _ComponentElementSeparator, 3) <> "") Then STCRow("STC01_3") = ss.ParseDemlimtedString(_STC01, _ComponentElementSeparator, 3) Else STCRow("STC01_3") = DBNull.Value
                                    If (ss.ParseDemlimtedString(_STC01, _ComponentElementSeparator, 4) <> "") Then STCRow("STC01_4") = ss.ParseDemlimtedString(_STC01, _ComponentElementSeparator, 4) Else STCRow("STC01_4") = DBNull.Value

                                End If


                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then STCRow("STC02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else STCRow("STC02") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then STCRow("STC03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else STCRow("STC03") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then STCRow("STC04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else STCRow("STC04") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then STCRow("STC05") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else STCRow("STC05") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then STCRow("STC06") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) Else STCRow("STC06") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then STCRow("STC07") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) Else STCRow("STC07") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) <> "") Then STCRow("STC08") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) Else STCRow("STC08") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) <> "") Then STCRow("STC09") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) Else STCRow("STC09") = DBNull.Value




                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11) <> "") Then
                                    STCRow("STC10") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11)
                                    _STC10 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11)
                                Else
                                    STCRow("STC10") = DBNull.Value

                                End If

                                If Not _STC10 = String.Empty Then

                                    If (ss.ParseDemlimtedString(_STC10, _ComponentElementSeparator, 1) <> "") Then STCRow("STC10_1") = ss.ParseDemlimtedString(_STC10, _ComponentElementSeparator, 1) Else STCRow("STC10_1") = DBNull.Value
                                    If (ss.ParseDemlimtedString(_STC10, _ComponentElementSeparator, 2) <> "") Then STCRow("STC10_2") = ss.ParseDemlimtedString(_STC10, _ComponentElementSeparator, 2) Else STCRow("STC10_2") = DBNull.Value
                                    If (ss.ParseDemlimtedString(_STC10, _ComponentElementSeparator, 3) <> "") Then STCRow("STC10_3") = ss.ParseDemlimtedString(_STC10, _ComponentElementSeparator, 3) Else STCRow("STC10_3") = DBNull.Value
                                    If (ss.ParseDemlimtedString(_STC10, _ComponentElementSeparator, 4) <> "") Then STCRow("STC10_4") = ss.ParseDemlimtedString(_STC10, _ComponentElementSeparator, 4) Else STCRow("STC10_4") = DBNull.Value

                                End If




                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12) <> "") Then
                                    STCRow("STC11") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12)
                                    _STC11 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12)
                                Else
                                    STCRow("STC11") = DBNull.Value

                                End If

                                If Not _STC11 = String.Empty Then

                                    If (ss.ParseDemlimtedString(_STC11, _ComponentElementSeparator, 1) <> "") Then STCRow("STC11_1") = ss.ParseDemlimtedString(_STC11, _ComponentElementSeparator, 1) Else STCRow("STC11_1") = DBNull.Value
                                    If (ss.ParseDemlimtedString(_STC11, _ComponentElementSeparator, 2) <> "") Then STCRow("STC11_2") = ss.ParseDemlimtedString(_STC11, _ComponentElementSeparator, 2) Else STCRow("STC11_2") = DBNull.Value
                                    If (ss.ParseDemlimtedString(_STC11, _ComponentElementSeparator, 3) <> "") Then STCRow("STC11_3") = ss.ParseDemlimtedString(_STC11, _ComponentElementSeparator, 3) Else STCRow("STC11_3") = DBNull.Value
                                    If (ss.ParseDemlimtedString(_STC11, _ComponentElementSeparator, 4) <> "") Then STCRow("STC11_4") = ss.ParseDemlimtedString(_STC11, _ComponentElementSeparator, 4) Else STCRow("STC11_4") = DBNull.Value

                                End If




                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 13) <> "") Then STCRow("STC12") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 13) Else STCRow("STC12") = DBNull.Value


                                'STCRow("FILE_ID") = _File_ID
                                'STCRow("BATCH_ID") = _BATCH_ID
                                'STCRow("GS_ID") = _GS_ID
                                'STCRow("ST_ID") = _ST_ID
                                'STCRow("STC_ID") = _STC_ID


                                STCRow("ROW_NUMBER") = rowcount



                                STCRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                STCRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                                STCRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor


                                STC.Rows.Add(STCRow)


                                Select Case _HL03
                                    Case 0
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case 19
                                        _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case 20
                                        _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case 21
                                        _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case Else
                                        _RAW_22 = _RAW_22 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                End Select



                            End If

                        Catch ex As Exception
                            _RowProcessedFlag = -1
                            log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "STC", ex)
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
                                REFRow("277_ISL_GUID") = _277_ISL_GUID
                                REFRow("277_IRL_GUID") = _277_IRL_GUID
                                REFRow("277_SPL_GUID") = _277_SPL_GUID
                                REFRow("277_CLS_GUID") = _277_CLS_GUID
                                REFRow("277_SLS_GUID") = _277_SLS_GUID
                                REFRow("HL01") = _HL01
                                REFRow("HL02") = _HL02
                                REFRow("HL03") = _HL03
                                REFRow("HL04") = _HL04
                                ' REFRow("P_GUID") = _P_GUID


                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then REFRow("REF01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else REFRow("REF01") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then REFRow("REF02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else REFRow("REF02") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then REFRow("REF03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else REFRow("REF03") = DBNull.Value

                                REFRow("ROW_NUMBER") = rowcount

                                REFRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                REFRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                                REFRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                                REF.Rows.Add(REFRow)

                                Select Case _HL03
                                    Case 0
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case 19
                                        _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case 20
                                        _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case 21
                                        _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case Else
                                        _RAW_22 = _RAW_22 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                End Select


                            End If

                        Catch ex As Exception
                            log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "REF", ex)
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
                                DTPRow("277_ISL_GUID") = _277_ISL_GUID
                                DTPRow("277_IRL_GUID") = _277_IRL_GUID
                                DTPRow("277_SPL_GUID") = _277_SPL_GUID
                                DTPRow("277_CLS_GUID") = _277_CLS_GUID
                                DTPRow("277_SLS_GUID") = _277_SLS_GUID
                                DTPRow("HL01") = _HL01
                                DTPRow("HL02") = _HL02
                                DTPRow("HL03") = _HL03
                                DTPRow("HL04") = _HL04

                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then DTPRow("DTP01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else DTPRow("DTP01") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then DTPRow("DTP02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else DTPRow("DTP02") = DBNull.Value
                                If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then DTPRow("DTP03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else DTPRow("DTP03") = DBNull.Value
                                '    If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then DTPRow("DTP04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else DTPRow("DTP04") = DBNull.Value


                                DTPRow("ROW_NUMBER") = rowcount
                                DTPRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                                DTPRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                                DTPRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                                DTP.Rows.Add(DTPRow)
                                ' _277_SLS_GUID = Guid.NewGuid()

                                Select Case _HL03
                                    Case 0
                                        _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case 19
                                        _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case 20
                                        _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                    Case 21
                                        _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"

                                    Case Else
                                        _RAW_22 = _RAW_22 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                                End Select

                                _SVC_FOUND = False
                                _UNX = True

                            End If



                        Catch ex As Exception
                            _RowProcessedFlag = -1
                            log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "DTP", ex)
                        End Try


                    End If

                End If




                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   END 277 
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''



                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   SE
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "SE" Then



                        _RowProcessedFlag = 1

                        Dim SERow As DataRow = SE.NewRow
                        SERow("FILE_ID") = _FILE_ID
                        SERow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                        SERow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                        SERow("HIPAA_ST_GUID") = _HIPAA_ST_GUID

                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then SERow("SE01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else SERow("SE01") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then SERow("SE02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else SERow("SE02") = DBNull.Value

                        SERow("ROW_NUMBER") = rowcount
                        SE.Rows.Add(SERow)

                        ' COMMIT THE LAST LK SET SINCE WE WONT GET TO lx AGAIN AND THE LAST clm SET
                        '  ComitLX()
                        'ComitRowData()

                        If _HL20_DIRTY Then
                            ComitHL20()
                            _HL20_DIRTY = False
                        End If

                        If _HL22_DIRTY Then
                            ComitHL22()
                        End If

                        ComitSE()

                        ClearST()




                    End If
                Catch ex As Exception
                    _hasERR = True
                    _ERROR = ex.Message
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "SE 005010X222A1", ex)
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
                    _hasERR = True
                    _ERROR = ex.Message
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GE 005010X222A1", ex)
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
                        ClearIAS()


                    End If
                Catch ex As Exception
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "IEA 005010X222A1", ex)
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
                        UNKRow("ROW_NUMBER") = rowcount
                        UNKRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        UNKRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                        UNKRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                        UNK.Rows.Add(UNKRow)

                        Select Case _HL03
                            Case 0
                                _RAW_HEADER = "UNK::" + _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            Case 19
                                _RAW_20 = "UNK::" + _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            Case 20
                                _RAW_20 = "UNK::" + _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            Case 21
                                _RAW_20 = "UNK::" + _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                            Case Else
                                _RAW_22 = "UNK::" + _RAW_22 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                        End Select

                    End If

                Catch ex As Exception
                    _hasERR = True
                    _ERROR = ex.Message
                    log.ExceptionDetails("IEA 005010X222A1", ex)
                End Try


                If _hasERR Then


                End If


            Next


            If _hasUNK Then
                Using e As New EDI_5010_LOGGING
                    e.ConnectionString = _ConnectionString
                    e.TransactionSetIdentifierCode = "277"
                    If _hasERR Then
                        e.UpdateFileStatus(CInt(_FILE_ID), "277", "PARSE COMPLETE WITH UNK SEGMENTS WITH ERRORS")
                    Else
                        e.UpdateFileStatus(CInt(_FILE_ID), "277", "PARSE COMPLETE WITH UNK SEGMENTS")
                    End If



                End Using

                ComitUNK()

            Else

                Using e As New EDI_5010_LOGGING
                    e.ConnectionString = _ConnectionString
                    e.TransactionSetIdentifierCode = "277"
                    If _hasERR Then
                        e.UpdateFileStatus(CInt(_FILE_ID), "277", "PARSE COMPLETE WITH ERRORS")
                    Else
                        e.UpdateFileStatus(CInt(_FILE_ID), "277", "PARSE COMPLETE")
                    End If
                End Using

            End If




            _ProcessEndTime = DateTime.Now

            Cleanup()



            If _ImportReturnCode >= 0 Then
                Return CInt(_BATCH_ID)
            Else
                Return _ImportReturnCode
            End If


            '  Return _ImportReturnCode

        End Function

        Private Function ComitUNK() As Integer


            Dim i As Integer

            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_COMIT_UNKNOWN, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_5010_UNK", UNK)
                        cmd.Parameters.AddWithValue("@IMPORTER", "EDI_5010_277_005010X212")
                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using

                i = 0

            Catch ex As Exception
                _hasERR = True
                _ERROR = ex.Message
                log.ExceptionDetails("COMIT_UNK_DATA 005010X212_277", ex)
            End Try

            Return i

        End Function



        Private Function Cleanup() As Int32
            Dim r As Integer = 0

            Dim FinalPath As String = String.Empty

            Try

                Dim span As TimeSpan

                span = _ProcessEndTime - _ProcessStartTime
                '  _ProcessElaspedTime = span.TotalMilliseconds

            Catch ex As Exception
                log.ExceptionDetails("CLEANUP 005010X212_277", ex)
            End Try






            Return r
        End Function


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ComitISA() As Integer


            Dim i As Integer


            Try
                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_ISA, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_277_ISA", ISA)
                        cmd.Parameters.Add("@ISA_ID", Data.SqlDbType.BigInt, 1)
                        cmd.Parameters("@ISA_ID").Direction = ParameterDirection.Output
                        cmd.ExecuteNonQuery()

                        _ISA_ID = Convert.ToInt32(cmd.Parameters("@ISA_ID").Value.ToString())
                    End Using
                    Con.Close()
                End Using

            Catch ex As Exception
                i = -1
                log.ExceptionDetails("COMIT_ISA_DATA 005010X212_277", ex)
            End Try

            Return i

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ComitGS() As Integer


            Dim i As Integer
            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_GS, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_277_GS", GS)

                        cmd.Parameters.Add("@GS_ID", Data.SqlDbType.BigInt, 1)
                        cmd.Parameters("@GS_ID").Direction = ParameterDirection.Output

                        cmd.ExecuteNonQuery()

                        _GS_ID = Convert.ToInt32(cmd.Parameters("@GS_ID").Value.ToString())


                    End Using
                    Con.Close()
                End Using

            Catch ex As Exception
                log.ExceptionDetails("COMIT_GS_DATA 005010X212_277", ex)
            End Try

            Return i

        End Function


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ComitST() As Int32


            Dim r As Integer

            Try



                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_ST, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_277_ST", ST)
                        cmd.Parameters.AddWithValue("@HIPAA_277_BHT", BHT)

                        cmd.Parameters.Add("@ST_ID", Data.SqlDbType.BigInt, 1)
                        cmd.Parameters("@ST_ID").Direction = ParameterDirection.Output
                        cmd.ExecuteNonQuery()

                        _ST_ID = Convert.ToInt32(cmd.Parameters("@ST_ID").Value.ToString())
                    End Using
                    Con.Close()
                End Using


            Catch ex As Exception
                log.ExceptionDetails("COMIT_ST_DATA 005010X212_277", ex)
            End Try

            Return r

        End Function


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ComitHL20() As Integer
            Dim i As Integer = -1

            Try



                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_COMIT_HL20_DATA, Con)

                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.AddWithValue("@HIPAA_277_HL", HL)
                        cmd.Parameters.AddWithValue("@HIPAA_277_TRN", TRN)
                        cmd.Parameters.AddWithValue("@HIPAA_277_STC", STC)
                        cmd.Parameters.AddWithValue("@HIPAA_277_NM1", NM1)
                        cmd.Parameters.AddWithValue("@HIPAA_277_PER", PER)
                        cmd.Parameters.AddWithValue("@RAW_HEADER", _RAW_HEADER)
                        cmd.Parameters.AddWithValue("@RAW_20", _RAW_20)

                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()


                    CLEAR_HEADER_DATA()
                End Using


            Catch ex As Exception
                log.ExceptionDetails("COMIT_HEADER_DATA 005010X212_277", ex)
            End Try

            Return i
        End Function


        Private Function ComitHL22() As Integer
            Dim i As Integer = -1

            Try

                'If isCLS_ON_DISC Then
                'Else
                '    MERGE_TABLES()
                '    isCLS_ON_DISC = True
                'End If


                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_COMIT_HL22_DATA, Con)

                        cmd.CommandType = CommandType.StoredProcedure


                        cmd.Parameters.AddWithValue("@HIPAA_277_DTP", DTP)
                        cmd.Parameters.AddWithValue("@HIPAA_277_HL", HL)
                        cmd.Parameters.AddWithValue("@HIPAA_277_NM1", NM1)
                        cmd.Parameters.AddWithValue("@HIPAA_277_PER", PER)
                        cmd.Parameters.AddWithValue("@HIPAA_277_REF", REF)
                        cmd.Parameters.AddWithValue("@HIPAA_277_STC", STC)
                        cmd.Parameters.AddWithValue("@HIPAA_277_SVC", SVC)
                        cmd.Parameters.AddWithValue("@HIPAA_277_TRN", TRN)

                        cmd.Parameters.AddWithValue("@HIPAA_277_CACHE_ST", ST)
                        cmd.Parameters.AddWithValue("@HIPAA_277_CACHE_BHT", BHT)

                        cmd.Parameters.AddWithValue("@HIPAA_277_CACHE_HL", CACHE_HL)
                        cmd.Parameters.AddWithValue("@HIPAA_277_CACHE_NM1", CACHE_NM1)
                        cmd.Parameters.AddWithValue("@HIPAA_277_CACHE_PER", CACHE_PER)
                        cmd.Parameters.AddWithValue("@HIPAA_277_CACHE_TRN", CACHE_TRN)
                        cmd.Parameters.AddWithValue("@HIPAA_277_CACHE_STC", CACHE_STC)

                        _ST_LINE = String.Empty
                        _ST_LINE = _ST + _BHT + _RAW_20 + _RAW_22

                        cmd.Parameters.AddWithValue("@HIPAA_277_ST_STRING", _ST_LINE)

                        cmd.Parameters.AddWithValue("@RAW_HEADER", _RAW_HEADER)
                        cmd.Parameters.AddWithValue("@RAW_20", _RAW_20)
                        cmd.Parameters.AddWithValue("@RAW_22", _RAW_22)

                        cmd.Parameters.AddWithValue("@DOCUMENT_ID", _DOCUMENT_ID)
                        cmd.Parameters.AddWithValue("@FILE_ID", _FILE_ID)
                        cmd.Parameters.AddWithValue("@ISA_ID", _ISA_ID)
                        cmd.Parameters.AddWithValue("@GS_ID", _GS_ID)
                        cmd.Parameters.AddWithValue("@ST_ID", _ST_ID)





                        cmd.Parameters.AddWithValue("@DELETE_FLAG", _DELETE_FLAG)
                        cmd.Parameters.AddWithValue("@cbr_id", _EBR_ID)
                        cmd.Parameters.AddWithValue("@user_id", _USER_ID)
                        cmd.Parameters.AddWithValue("@hosp_code", _HOSP_CODE)
                        cmd.Parameters.AddWithValue("@source", _SOURCE)


                        '  cmd.Parameters.AddWithValue("@Log_EDI", _LOG_EDI)



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


                        cmd.Parameters.AddWithValue("@batch_id", _BATCH_ID)
                        cmd.Parameters("@batch_id").Direction = ParameterDirection.InputOutput



                        cmd.ExecuteNonQuery()

                        _RETURN_BATCH_ID = Convert.ToInt64(cmd.Parameters("@batch_id").Value)

                        _STATUS = cmd.Parameters("@Status").Value.ToString()
                        _REJECT_REASON_CODE = cmd.Parameters("@Reject_Reason_code").Value.ToString()
                        _LOOP_AGAIN = cmd.Parameters("@LOOP_AGAIN").Value.ToString()


                        i = 0
                    End Using
                    Con.Close()
                End Using
                CLEAR_22_DATA()
            Catch ex As Exception
                log.ExceptionDetails("COMIT_ROW_DATA 005010X212_277", ex)
            End Try



            Return i

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ComitSE() As Int32
            Dim r As Integer

            Try
                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_SE, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_277_SE", SE)
                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using


            Catch ex As Exception
                log.ExceptionDetails("COMIT_SE_DATA 005010X212_277", ex)
            End Try

            Return r

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ComitGE() As Integer
            Dim i As Integer

            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_GE, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_277_GE", GE)

                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using

            Catch ex As Exception
                log.ExceptionDetails("COMIT_GE_DATA 005010X212_277", ex)
            End Try

            Return i

        End Function


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ComitIEA() As Integer


            Dim i As Integer

            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_IEA, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_277_IEA", IEA)
                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using

                i = 0

            Catch ex As Exception
                log.ExceptionDetails("COMIT_IEA_DATA 005010X212_277", ex)
            End Try

            Return i

        End Function


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub ClearIAS()
            ISA.Clear()
            IEA.Clear()
            _HIPAA_ISA_GUID = Guid.Empty
            _ISA_ID = 0

            ' _RowProcessedFlag = 0
            _DataElementSeparator = String.Empty
            _RepetitionSeparator = String.Empty
            _ComponentElementSeparator = String.Empty

            ClearGS()

        End Sub


        Private Sub ClearGS()

            GS.Clear()
            GE.Clear()
            _HIPAA_GS_GUID = Guid.Empty
            _GS_ID = 0
            ClearST()

        End Sub

        Private Sub ClearST()

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   BEGIN COMMON
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            _SE_FOUND = False
            ST.Clear()
            SE.Clear()
            BHT.Clear()
            _ST_ID = 0
            _HIPAA_ST_GUID = Guid.Empty


            _ST = String.Empty
            _BHT = String.Empty

            _RAW_HEADER_BUILT = False
            _RAW_HEADER = String.Empty
            _RAW_20 = String.Empty
            _RAW_22 = String.Empty


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
            '  BEGIN 277 
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            CLEAR_HEADER_DATA()


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '  END 277 
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
            '  _RAW_20 = String.Empty
            _RAW_22 = String.Empty

            _277_TRN_GUID = Guid.Empty
            _277_STC_GUID = Guid.Empty
            _277_SVC_GUID = Guid.Empty
            _277_P_STC_GUID = Guid.Empty

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
            _277_CLS_GUID = Guid.Empty
            _277_SLS_GUID = Guid.Empty

            _277_2200D_DTP_FOUND = False

            _LoopLevelMinor = 200

            _SVC_FOUND = False
            _TRN_FOUND = False
            _UNX = False


            CLEAR_CLS_DATA()

        End Sub


        Private Sub CLEAR_CLS_DATA()




            DTP.Clear()
            REF.Clear()
            STC.Clear()
            TRN.Clear()




            _277_CLS_GUID = Guid.Empty
            _277_SLS_GUID = Guid.Empty
            CLEAR_SLS_DATA()

        End Sub


        Private Sub CLEAR_SLS_DATA()


            SVC.Clear()
            STC.Clear()
            DTP.Clear()
            REF.Clear()
            _277_SLS_GUID = Guid.Empty

        End Sub

        Private Sub MERGE_TABLES()

            Using m As New DataTableMaintance
                m.MergeTable(CLS_STC, STC)
                m.MergeTable(CLS_REF, REF)
                m.MergeTable(CLS_DTP, DTP)
                m.MergeTable(CLS_TRN, TRN)
            End Using

        End Sub


        Private Function RollBack() As Integer


            Dim i As Integer

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
                log.ExceptionDetails("ROLL_BACK 005010X212_277", ex)
            End Try

            Return i

        End Function



        Public Function TestSPs() As Integer

            Dim r As Integer = -1



            Try


                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_COMIT_HL20_UNIT_TEST, Con)

                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.AddWithValue("@HIPAA_277_HL", HL)
                        cmd.Parameters.AddWithValue("@HIPAA_277_NM1", NM1)
                        cmd.Parameters.AddWithValue("@HIPAA_277_PER", PER)
                        cmd.Parameters.AddWithValue("@HIPAA_277_TRN", TRN)
                        cmd.Parameters.AddWithValue("@HIPAA_277_STC", STC)

                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using

                r = 0
                _HL20_SP_TEST = True
            Catch ex As Exception
                _HL20_SP_TEST = False
                log.ExceptionDetails("HL 20 TEST FAILED 005010X212_277", ex)
            End Try




            Try



                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_COMIT_HL22_UNIT_DATA, Con)

                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.AddWithValue("@HIPAA_277_DTP", DTP)
                        cmd.Parameters.AddWithValue("@HIPAA_277_HL", HL)
                        cmd.Parameters.AddWithValue("@HIPAA_277_NM1", NM1)
                        cmd.Parameters.AddWithValue("@HIPAA_277_PER", PER)
                        cmd.Parameters.AddWithValue("@HIPAA_277_REF", REF)
                        cmd.Parameters.AddWithValue("@HIPAA_277_STC", STC)
                        cmd.Parameters.AddWithValue("@HIPAA_277_SVC", SVC)
                        cmd.Parameters.AddWithValue("@HIPAA_277_TRN", TRN)




                        cmd.Parameters.AddWithValue("@HIPAA_277_CACHE_HL", CACHE_HL)
                        cmd.Parameters.AddWithValue("@HIPAA_277_CACHE_NM1", CACHE_NM1)
                        cmd.Parameters.AddWithValue("@HIPAA_277_CACHE_PER", CACHE_PER)
                        cmd.Parameters.AddWithValue("@HIPAA_277_CACHE_TRN", CACHE_TRN)
                        cmd.Parameters.AddWithValue("@HIPAA_277_CACHE_STC", CACHE_STC)

                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using

                r = 0

            Catch ex As Exception

                log.ExceptionDetails("HL 22 TEST FAILED 005010X212_277", ex)
            End Try

            Return r



        End Function
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
                        _LoopLevelSubFix = "B"
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

                        _277_2200D_BEGIN = True
                        ' _277_CLS_GUID = Guid.NewGuid
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




        ' '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''   DTP
        ' '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        'Try
        '    If _RowRecordType = "DMG" Then



        '        '    oD.RowProcessedFlag = 1

        '        Dim DMGRow As DataRow = DMG.NewRow
        '        DMGRow("DOCUMENT_ID") = _DOCUMENT_ID
        '        DMGRow("FILE_ID") = _FILE_ID
        '        DMGRow("ISA_ID") = _ISA_ID
        '        DMGRow("GS_ID") = _GS_ID
        '        DMGRow("ST_ID") = _ST_ID
        '        DMGRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
        '        DMGRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
        '        DMGRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
        '        DMGRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
        '        DMGRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
        '        DMGRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
        '        DMGRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
        '        DMGRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
        '        DMGRow("277_ISL_GUID") = _277_ISL_GUID
        '        DMGRow("277_IRL_GUID") = _277_IRL_GUID
        '        DMGRow("277_SPL_GUID") = _277_SPL_GUID
        '        DMGRow("277_CLS_GUID") = _277_CLS_GUID
        '        DMGRow("277_SLS_GUID") = _277_SLS_GUID
        '        DMGRow("HL01") = _HL01
        '        DMGRow("HL02") = _HL02
        '        DMGRow("HL03") = _HL03
        '        DMGRow("HL04") = _HL04

        '        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then DMGRow("DMG01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else DMGRow("DMG01") = DBNull.Value
        '        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then DMGRow("DMG02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else DMGRow("DMG02") = DBNull.Value
        '        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then DMGRow("DMG03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else DMGRow("DMG03") = DBNull.Value


        '        DMGRow("ROW_NUMBER") = rowcount



        '        DMGRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
        '        DMGRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
        '        DMGRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor


        '        DMG.Rows.Add(DMGRow)

        '        _RowProcessedFlag = 1


        '        Select Case _HL03
        '            Case 0
        '                _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
        '            Case 19
        '                _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
        '            Case 20
        '                _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
        '            Case 21
        '                _RAW_20 = _RAW_20 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
        '            Case Else
        '                _RAW_22 = _RAW_22 + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
        '        End Select

        '    End If


        'Catch ex As Exception
        '    _RowProcessedFlag = -1
        '    '   log.ExceptionDetails("HL", _DocumentType, ex)
        'End Try


        Public WriteOnly Property BatchID As Double

            Set(value As Double)
                _BATCH_ID = Convert.ToInt64(value)

            End Set
        End Property

        Public WriteOnly Property EDI As String

            Set(value As String)
                _RAW_EDI = value
            End Set
        End Property


        'Public Property DTdistinctDT As DataTable
        '    Get
        '        Return distinctDT
        '    End Get
        '    Set(value As DataTable)

        '    End Set
        'End Property

        Public WriteOnly Property Commit As Integer

            Set(value As Integer)
                _COMMIT = value
            End Set
        End Property

        Public WriteOnly Property ebr_id As Double


            Set(value As Double)
                _EBR_ID = Convert.ToInt64(value)
            End Set
        End Property


        Public WriteOnly Property CBR_ID As Double


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




        Public WriteOnly Property DebugLevel As Integer

            Set(value As Integer)
                _DEBUG_LEVEL = value
            End Set
        End Property
        Public WriteOnly Property isFile As Boolean


            Set(value As Boolean)
                _IS_FILE = value
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

        Public Property ExecutionTime As TimeSpan
            Get
                Return _ElapsedTime


            End Get
            Set(value As TimeSpan)

            End Set
        End Property

        'Public Property EBLoopCount As Integer
        '    Get
        '        Return mmm
        '    End Get
        '    Set(value As Integer)

        '    End Set
        'End Property

        'Public ReadOnly Property ErrorMessage As String
        '    Get
        '        Return err
        '    End Get

        'End Property

        'Public ReadOnly Property chars As String
        '    Get
        '        Return _chars
        '    End Get

        'End Property

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

        Public WriteOnly Property Patient_number As String

            Set(value As String)
                _PATIENT_NUMBER = value
            End Set
        End Property


    End Class


End Namespace
