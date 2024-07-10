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
    Public Class EDI_5010_837I_005010X223A2_v2

        Inherits EDI_5010_837_TABLES


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

        Private _DocumentType As String = "005010X223A2"

        Private _SP_ISA As String = "[usp_EDI_5010_HIPAA_837_ISA]"
        Private _SP_IEA As String = "[usp_EDI_5010_HIPAA_837_IEA]"

        Private _SP_GS As String = "[usp_EDI_5010_HIPAA_837_GS]"
        Private _SP_GE As String = "[usp_EDI_5010_HIPAA_837_GE]"

        Private _SP_ST As String = "[usp_EDI_5010_HIPAA_837_ST]"
        Private _SP_SE As String = "[usp_EDI_5010_HIPAA_837_SE]"

        Private _SP_HEADERS As String = "[usp_EDI_5010_HIPAA_837_20_HEADER_DUMP]"

        Private _SP_COMIT_ROW_DATA As String = "[usp_EDI_5010_HIPAA_837_22_CLAIM_DUMP]"

        Private _SP_COMIT_HL22_DATA As String = "[usp_EDI_5010_HIPAA_837_22_CLAIM_DUMP]"



        Private _SP_COMIT_HL20_UNIT_TEST As String = "[usp_EDI_5010_HIPAA_837_20_UNIT_TEST]"
        Private _SP_COMIT_HL22_UNIT_TEST As String = "[usp_EDI_5010_HIPAA_837_22_UNIT_TEST]"



        Private _SP_COMIT_UNKNOWN As String = "[usp_EDI_5010_HIPAA_UNKNOWN]"

        Private _SP_ROLLBACK As String = "[usp_EDI_837_ROLLBACK]"


        Private _LX_FLAG As Integer = 0



        Private _837_LX_GUID As Guid = Guid.Empty
        Private _837_CLP_GUID As Guid = Guid.Empty
        Private _837_CLM_GUID As Guid = Guid.Empty
        Private _837_SVC_GUID As Guid = Guid.Empty
        Private _837_SBR_GUID As Guid = Guid.Empty
        Private _837_SVD_GUID As Guid = Guid.Empty






        Private _NM01 As String = String.Empty

        Private CLM05 As String = String.Empty
        Private CLM11 As String = String.Empty

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
        Private SV107 As String = String.Empty
        Private SVD03 As String = String.Empty


        Public Sub New()
            If Debugger.IsAttached Then
                _VERBOSE = 1
                _DEBUG_LEVEL = 1

            End If

            _CONSOLE_NAME = "EDI_5010_837I_005010X223A2_v2"
            _CLASS_NAME = "EDI_5010_837I_005010X223A2_v2"

            _TRANSACTION_GUID = Guid.NewGuid
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

        Public Property LOG_EDI As String

            Set(value As String)
                _LOG_EDI = value
            End Set
            Get
                Return _LOG_EDI
            End Get
        End Property

        Public WriteOnly Property isFile As Boolean

            Set(value As Boolean)
                _IS_FILE = value

            End Set
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
        Public WriteOnly Property SP_DO_TEST As Integer

            Set(value As Integer)
                _SP_DO_TEST = value
            End Set
        End Property
        Public WriteOnly Property SP_HEADERS As String

            Set(value As String)
                _SP_HEADERS = value
            End Set
        End Property

        Public WriteOnly Property SP_COMIT_ROW_DATA As String

            Set(value As String)
                _SP_COMIT_ROW_DATA = value
            End Set
        End Property

        Public WriteOnly Property SP_COMIT_HL20_UNIT_TEST As String

            Set(value As String)
                _SP_COMIT_HL20_UNIT_TEST = value
            End Set
        End Property



        Public WriteOnly Property SP_COMIT_HL22_UNIT_TEST As String

            Set(value As String)
                _SP_COMIT_HL22_UNIT_TEST = value
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




        Public Function Import(ByVal EDIList As List(Of String)) As Integer
            Dim last As String = String.Empty
            Dim rowcount As Int32 = 0


            _FUNCTION_NAME = "Import(ByVal EDIList As List(Of String)) As InTEGER"



            _IMPORTER_CLASS = Me.GetType().Name
            _IMPORTER_BUILD = "2"



            'If _EDI_SEQUENCE_NUMBER = 0 Then

            '    Using e As New EDI_5010_LOGGING
            '        e.ConnectionString = _ConnectionString
            '        e.TransactionSetIdentifierCode = "837"
            '        e.EDI_SEQUENCE("837")
            '        _EDI_SEQUENCE_NUMBER = e.EDI_SEQUENCE_NUMBER
            '    End Using

            'End If


            If (_IS_FILE) Then
                Using e As New EDI_5010_LOGGING
                    e.ConnectionString = _ConnectionString
                    e.IMPORTER_BUILD = _IMPORTER_BUILD
                    e.IMPORTER_CLASS = _IMPORTER_CLASS
                    e.TransactionSetIdentifierCode = "837"
                    e.UpdateFileStatus(CInt(_FILE_ID), "BEGIN PARSE", "837")
                End Using
            End If


            _ProcessStartTime = Now
            Dim _ImportReturnCode As Integer = 0


            If _TablesBuilt = False Then
                BuildTables()
                _TablesBuilt = True
                ClearIAS()
                ClearGS()
                ClearST()
                ClearLoop1000()
            End If

            If _SP_DO_TEST = 1 Then

                _SP_RETURN_CODE = TestSPs()

                If (_SP_RETURN_CODE <> 0) Then
                    _IMPORT_RETURN_STRING = "837I : SP TEST FAILED WITH ERROR CODE " + Convert.ToString(_SP_RETURN_CODE)
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _IMPORT_RETURN_STRING)
                    Return -1
                    Exit Function
                End If
            End If


            '     _DOCUMENT_ID = _EDI_SEQUENCE_NUMBER

            'so lets get down to it opn the file in a stream reader and move thru it one line at a time...........

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

                ' _RAW_EDI = _RAW_EDI + line

                line = line.Replace("~", "")
                rowcount = rowcount + 1




                _RowRecordType = ss.ParseDemlimtedStringEDI(line, _DataElementSeparator, 1)
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





                        _HIPAA_ISA_GUID = Guid.NewGuid


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

                        ' _chars = "RowDataDelimiter: " + _DataElementSeparator + " CarrotDataDelimiter: " + _ISA11_RepetitionSeparator + " ComponentElementSeperator: " + _ISA16_ComponentElementSeparator

                        ComitISA()
                        _RowProcessedFlag = 1
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


                        _GS08_ImplementationConventionReference = _GS08

                        _GS = Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"

                        ComitGS()

                        _RowProcessedFlag = 1
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

                        _ST = Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"

                        ST.Rows.Add(STRow)

                        '   ComitST()
                    End If

                    ' all the rows get made in to a string. 

                    _RowProcessedFlag = 1
                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
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



                        '  _ENFlag = True


                        '    ComitBHT()
                        _BHT = Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"

                        ComitST()

                        _RowProcessedFlag = 1
                    End If

                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
                End Try




                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   BEGIN 837 I 005010X223A2
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

                                If Not _is_ST_BHT_NM1_PER Then
                                    ComitHeader()
                                    _is_ST_BHT_NM1_PER = True
                                    _is_ST_BHT_NM1_PER_CACHED = True
                                End If



                                If _HL22_DIRTY Then
                                    ComitHL22()
                                    _HL22_DIRTY = False
                                    _is_HL20_ON_DISC = False
                                End If

                                _RAW_20 = String.Empty

                            Case 21
                                _HL21_DIRTY = True
                            Case 22

                                If _HL20_DIRTY Then
                                    ComitHL20()
                                    _HL20_DIRTY = False
                                End If

                                If _HL22_DIRTY Then
                                    ComitHL22()
                                    _HL22_DIRTY = False
                                    '  HL.Rows.Add(HLRow)

                                End If



                            Case 23
                                ClearCLM()

                            Case 24

                            Case Else

                        End Select


                    End If



                Catch ex As Exception

                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
                End Try




                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   HL      
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "HL" Then



                        Dim HLRow As DataRow = HL.NewRow
                        HLRow("DOCUMENT_ID") = _DOCUMENT_ID
                        HLRow("FILE_ID") = _FILE_ID
                        HLRow("BATCH_ID") = _BATCH_ID
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



                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then HLRow("HL01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else HLRow("HL01") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then HLRow("HL02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else HLRow("HL02") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then HLRow("HL03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else HLRow("HL03") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then HLRow("HL04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else HLRow("HL04") = DBNull.Value

                        Select Case _HL03   ' Must be a primitive data type
                            Case 20
                                _HL20_DIRTY = True
                                _HIPAA_HL_20_GUID = Guid.NewGuid
                                _HIPAA_HL_21_GUID = Guid.Empty
                                _HIPAA_HL_22_GUID = Guid.Empty
                                _HIPAA_HL_23_GUID = Guid.Empty
                                _HIPAA_HL_24_GUID = Guid.Empty
                            Case 21
                                _HL21_DIRTY = True
                                _HIPAA_HL_21_GUID = Guid.NewGuid
                                _HIPAA_HL_22_GUID = Guid.Empty
                                _HIPAA_HL_23_GUID = Guid.Empty
                                _HIPAA_HL_24_GUID = Guid.Empty
                            Case 22
                                _HL22_DIRTY = True
                                _HIPAA_HL_22_GUID = Guid.NewGuid
                                _HIPAA_HL_23_GUID = Guid.Empty
                                _HIPAA_HL_24_GUID = Guid.Empty
                            Case 23
                                _HL22_DIRTY = True
                                _HIPAA_HL_23_GUID = Guid.NewGuid
                                _HIPAA_HL_24_GUID = Guid.Empty
                            Case 24
                                _HL22_DIRTY = True
                                _HIPAA_HL_24_GUID = Guid.NewGuid
                            Case Else

                        End Select

                        HLRow("ROW_NUMBER") = rowcount
                        HLRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                        HLRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                        HLRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                        HLRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                        HLRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID



                        HLRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        HLRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                        HLRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor






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

                        HL.Rows.Add(HLRow)

                        _RowProcessedFlag = 1
                    End If

                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
                End Try






                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   NM1
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "NM1" Then



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
                        NM1Row("837_SBR_GUID") = _837_SBR_GUID
                        NM1Row("837_CLM_GUID") = _837_CLM_GUID
                        NM1Row("837_LX_GUID") = _837_LX_GUID
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



                        NM1Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        NM1Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                        NM1Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor





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


                        NM1.Rows.Add(NM1Row)

                        _RowProcessedFlag = 1
                    End If

                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
                End Try




                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   SBR
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "SBR" Then

                        _LoopLevelMajor = 2000
                        _LoopLevelSubFix = "B"


                        If _LoopLevelMajor = 2320 Then

                            _837_SBR_GUID = Guid.NewGuid

                        End If




                        _837_SBR_GUID = Guid.NewGuid




                        Dim SBRRow As DataRow = SBR.NewRow
                        SBRRow("DOCUMENT_ID") = _DOCUMENT_ID
                        SBRRow("FILE_ID") = _FILE_ID
                        SBRRow("BATCH_ID") = _BATCH_ID
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
                        SBRRow("837_SBR_GUID") = _837_SBR_GUID
                        SBRRow("HL01") = _HL01
                        SBRRow("HL02") = _HL02
                        SBRRow("HL03") = _HL03
                        SBRRow("HL04") = _HL04






                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then SBRRow("SBR01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else SBRRow("SBR01") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then SBRRow("SBR02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else SBRRow("SBR02") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then SBRRow("SBR03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else SBRRow("SBR03") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then SBRRow("SBR04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else SBRRow("SBR04") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then SBRRow("SBR05") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else SBRRow("SBR05") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then SBRRow("SBR06") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) Else SBRRow("SBR06") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then SBRRow("SBR07") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) Else SBRRow("SBR07") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) <> "") Then SBRRow("SBR08") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) Else SBRRow("SBR08") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) <> "") Then SBRRow("SBR09") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) Else SBRRow("SBR09") = DBNull.Value


                        SBRRow("ROW_NUMBER") = rowcount


                        SBRRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        SBRRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                        SBRRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor









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



                        SBR.Rows.Add(SBRRow)

                        _RowProcessedFlag = 1
                    End If
                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
                End Try




                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   CLM
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "CLM" Then

                        'If CLMFlag = 0 Then


                        _837_CLM_GUID = Guid.NewGuid
                        _NM1_GUID = Guid.Empty

                        Dim CLMRow As DataRow = CLM.NewRow

                        CLM11 = String.Empty
                        CLM05 = String.Empty



                        _LoopLevelMajor = 2300


                        CLMRow("DOCUMENT_ID") = _DOCUMENT_ID
                        CLMRow("FILE_ID") = _FILE_ID
                        CLMRow("BATCH_ID") = _BATCH_ID
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
                        CLMRow("837_SBR_GUID") = _837_SBR_GUID
                        CLMRow("837_CLM_GUID") = _837_CLM_GUID
                        CLMRow("837_LX_GUID") = _837_LX_GUID
                        CLMRow("HL01") = _HL01
                        CLMRow("HL02") = _HL02
                        CLMRow("HL03") = _HL03
                        CLMRow("HL04") = _HL04

                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then CLMRow("CLM01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else CLMRow("CLM01") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then CLMRow("CLM02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else CLMRow("CLM02") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then CLMRow("CLM03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else CLMRow("CLM03") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then CLMRow("CLM04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else CLMRow("CLM04") = DBNull.Value




                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then CLMRow("CLM05") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else CLMRow("CLM05") = DBNull.Value


                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then
                            CLMRow("CLM05") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6)
                            CLM05 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6)
                        Else
                            CLMRow("CLM05") = DBNull.Value

                        End If


                        If Not CLM05 = String.Empty Then

                            If (ss.ParseDemlimtedString(CLM05, _ComponentElementSeparator, 1) <> "") Then CLMRow("CLM05_01") = ss.ParseDemlimtedString(CLM05, _ComponentElementSeparator, 1) Else CLMRow("CLM05_01") = DBNull.Value
                            If (ss.ParseDemlimtedString(CLM05, _ComponentElementSeparator, 2) <> "") Then CLMRow("CLM05_02") = ss.ParseDemlimtedString(CLM05, _ComponentElementSeparator, 2) Else CLMRow("CLM05_02") = DBNull.Value
                            If (ss.ParseDemlimtedString(CLM05, _ComponentElementSeparator, 3) <> "") Then CLMRow("CLM05_03") = ss.ParseDemlimtedString(CLM05, _ComponentElementSeparator, 3) Else CLMRow("CLM05_03") = DBNull.Value

                        End If









                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then CLMRow("CLM06") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) Else CLMRow("CLM06") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then CLMRow("CLM07") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) Else CLMRow("CLM07") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) <> "") Then CLMRow("CLM08") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) Else CLMRow("CLM08") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) <> "") Then CLMRow("CLM09") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) Else CLMRow("CLM09") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11) <> "") Then CLMRow("CLM10") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11) Else CLMRow("CLM10") = DBNull.Value




                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12) <> "") Then CLMRow("CLM11") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12) Else CLMRow("CLM11") = DBNull.Value


                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12) <> "") Then
                            CLMRow("CLM11") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12)
                            CLM11 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12)
                        Else
                            CLMRow("CLM11") = DBNull.Value

                        End If


                        If Not CLM11 = String.Empty Then

                            If (ss.ParseDemlimtedString(CLM11, _ComponentElementSeparator, 1) <> "") Then CLMRow("CLM11_01") = ss.ParseDemlimtedString(CLM11, _ComponentElementSeparator, 1) Else CLMRow("CLM11_01") = DBNull.Value
                            If (ss.ParseDemlimtedString(CLM11, _ComponentElementSeparator, 2) <> "") Then CLMRow("CLM11_02") = ss.ParseDemlimtedString(CLM11, _ComponentElementSeparator, 2) Else CLMRow("CLM11_02") = DBNull.Value
                            If (ss.ParseDemlimtedString(CLM11, _ComponentElementSeparator, 3) <> "") Then CLMRow("CLM11_03") = ss.ParseDemlimtedString(CLM11, _ComponentElementSeparator, 3) Else CLMRow("CLM11_03") = DBNull.Value
                            If (ss.ParseDemlimtedString(CLM11, _ComponentElementSeparator, 4) <> "") Then CLMRow("CLM11_04") = ss.ParseDemlimtedString(CLM11, _ComponentElementSeparator, 2) Else CLMRow("CLM11_04") = DBNull.Value
                            If (ss.ParseDemlimtedString(CLM11, _ComponentElementSeparator, 5) <> "") Then CLMRow("CLM11_05") = ss.ParseDemlimtedString(CLM11, _ComponentElementSeparator, 3) Else CLMRow("CLM11_05") = DBNull.Value

                        End If



                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 13) <> "") Then CLMRow("CLM12") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 13) Else CLMRow("CLM12") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 14) <> "") Then CLMRow("CLM13") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 14) Else CLMRow("CLM13") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 15) <> "") Then CLMRow("CLM14") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 15) Else CLMRow("CLM14") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 16) <> "") Then CLMRow("CLM15") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 16) Else CLMRow("CLM15") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 17) <> "") Then CLMRow("CLM16") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 17) Else CLMRow("CLM16") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 18) <> "") Then CLMRow("CLM17") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 18) Else CLMRow("CLM17") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 19) <> "") Then CLMRow("CLM18") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 19) Else CLMRow("CLM18") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 20) <> "") Then CLMRow("CLM19") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 20) Else CLMRow("CLM19") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 21) <> "") Then CLMRow("CLM20") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 21) Else CLMRow("CLM20") = DBNull.Value

                        CLMRow("ROW_NUMBER") = rowcount



                        CLMRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        CLMRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                        CLMRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor









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

                        CLM.Rows.Add(CLMRow)

                        _RowProcessedFlag = 1
                    End If

                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
                End Try






                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   LX
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "LX" Then



                        _LoopLevelMajor = 2400
                        'ClearLX()
                        'If _LX_FLAG = 0 Then
                        '    ComitHL22()
                        '    ClearLX()
                        '    _LX_FLAG = 1
                        'Else

                        'End If


                        _837_LX_GUID = Guid.NewGuid

                        '  _LX_GUID = Guid.NewGuid
                        '   _CLM_GUID = Guid.Empty
                        ' _SVC_GUID = Guid.Empty


                        Dim LXRow As DataRow = LX.NewRow
                        LXRow("DOCUMENT_ID") = _DOCUMENT_ID
                        LXRow("FILE_ID") = _FILE_ID
                        LXRow("BATCH_ID") = _BATCH_ID
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
                        LXRow("837_SBR_GUID") = _837_SBR_GUID
                        LXRow("837_CLM_GUID") = _837_CLM_GUID
                        LXRow("837_LX_GUID") = _837_LX_GUID
                        LXRow("HL01") = _HL01
                        LXRow("HL02") = _HL02
                        LXRow("HL03") = _HL03
                        LXRow("HL04") = _HL04

                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then LXRow("LX01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else LXRow("LX01") = DBNull.Value

                        LXRow("ROW_NUMBER") = rowcount

                        LXRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        LXRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                        LXRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor



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

                        LX.Rows.Add(LXRow)

                        _RowProcessedFlag = 1
                    End If

                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
                End Try


                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   SVD
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "SVD" Then
                        ' _LoopLevelMajor = 2430

                        'ClearSVD()
                        _837_SVD_GUID = Guid.NewGuid

                        SVD03 = String.Empty
                        Dim SVDRow As DataRow = SVD.NewRow
                        SVDRow("DOCUMENT_ID") = _DOCUMENT_ID
                        SVDRow("FILE_ID") = _FILE_ID
                        SVDRow("BATCH_ID") = _BATCH_ID
                        SVDRow("ISA_ID") = _ISA_ID
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
                        SVDRow("837_SBR_GUID") = _837_SBR_GUID
                        SVDRow("837_CLM_GUID") = _837_CLM_GUID
                        SVDRow("837_LX_GUID") = _837_LX_GUID
                        SVDRow("837_SVD_GUID") = _837_SVD_GUID
                        SVDRow("HL01") = _HL01
                        SVDRow("HL02") = _HL02
                        SVDRow("HL03") = _HL03
                        SVDRow("HL04") = _HL04

                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then SVDRow("SVD01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else SVDRow("SVD01") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then SVDRow("SVD02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else SVDRow("SVD02") = DBNull.Value




                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then
                            SVDRow("SVD03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4)
                            SVD03 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4)
                        Else
                            SVDRow("SVD03") = DBNull.Value

                        End If

                        If Not SVD03 = String.Empty Then

                            If (ss.ParseDemlimtedString(SVD03, _ComponentElementSeparator, 1) <> "") Then SVDRow("SVD03_01") = ss.ParseDemlimtedString(SVD03, _ComponentElementSeparator, 1) Else SVDRow("SVD03_01") = DBNull.Value
                            If (ss.ParseDemlimtedString(SVD03, _ComponentElementSeparator, 2) <> "") Then SVDRow("SVD03_02") = ss.ParseDemlimtedString(SVD03, _ComponentElementSeparator, 2) Else SVDRow("SVD03_02") = DBNull.Value
                            If (ss.ParseDemlimtedString(SVD03, _ComponentElementSeparator, 3) <> "") Then SVDRow("SVD03_03") = ss.ParseDemlimtedString(SVD03, _ComponentElementSeparator, 3) Else SVDRow("SVD03_03") = DBNull.Value
                            If (ss.ParseDemlimtedString(SVD03, _ComponentElementSeparator, 4) <> "") Then SVDRow("SVD03_04") = ss.ParseDemlimtedString(SVD03, _ComponentElementSeparator, 4) Else SVDRow("SVD03_04") = DBNull.Value
                            If (ss.ParseDemlimtedString(SVD03, _ComponentElementSeparator, 5) <> "") Then SVDRow("SVD03_05") = ss.ParseDemlimtedString(SVD03, _ComponentElementSeparator, 5) Else SVDRow("SVD03_05") = DBNull.Value
                            If (ss.ParseDemlimtedString(SVD03, _ComponentElementSeparator, 6) <> "") Then SVDRow("SVD03_06") = ss.ParseDemlimtedString(SVD03, _ComponentElementSeparator, 6) Else SVDRow("SVD03_06") = DBNull.Value
                            If (ss.ParseDemlimtedString(SVD03, _ComponentElementSeparator, 7) <> "") Then SVDRow("SVD03_07") = ss.ParseDemlimtedString(SVD03, _ComponentElementSeparator, 7) Else SVDRow("SVD03_07") = DBNull.Value

                        End If





                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then SVDRow("SVD04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else SVDRow("SVD04") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then SVDRow("SVD05") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else SVDRow("SVD05") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then SVDRow("SVD06") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) Else SVDRow("SVD06") = DBNull.Value

                        SVDRow("ROW_NUMBER") = rowcount

                        SVDRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        SVDRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                        SVDRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix





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

                        SVD.Rows.Add(SVDRow)

                        _RowProcessedFlag = 1
                    End If
                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)

                End Try

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   AMT
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "AMT" Then



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
                        AMTRow("837_SBR_GUID") = _837_SBR_GUID
                        AMTRow("837_CLM_GUID") = _837_CLM_GUID
                        AMTRow("837_LX_GUID") = _837_LX_GUID
                        AMTRow("837_SVD_GUID") = _837_SVD_GUID
                        AMTRow("HL01") = _HL01
                        AMTRow("HL02") = _HL02
                        AMTRow("HL03") = _HL03
                        AMTRow("HL04") = _HL04

                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then AMTRow("AMT01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else AMTRow("AMT01") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then AMTRow("AMT02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else AMTRow("AMT02") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then AMTRow("AMT03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else AMTRow("AMT03") = DBNull.Value

                        AMTRow("ROW_NUMBER") = rowcount


                        AMTRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        AMTRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                        AMTRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor




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


                        AMT.Rows.Add(AMTRow)

                        _RowProcessedFlag = 1
                    End If

                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
                End Try




                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   CAS
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "CAS" Then



                        Dim CASRow As DataRow = CAS.NewRow
                        CASRow("DOCUMENT_ID") = _DOCUMENT_ID
                        CASRow("FILE_ID") = _FILE_ID
                        CASRow("BATCH_ID") = _BATCH_ID
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
                        CASRow("837_SBR_GUID") = _837_SBR_GUID
                        CASRow("837_CLM_GUID") = _837_CLM_GUID
                        CASRow("837_LX_GUID") = _837_LX_GUID
                        CASRow("837_SVD_GUID") = _837_SVD_GUID
                        CASRow("HL01") = _HL01
                        CASRow("HL02") = _HL02
                        CASRow("HL03") = _HL03
                        CASRow("HL04") = _HL04

                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then CASRow("CAS01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else CASRow("CAS01") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then CASRow("CAS02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else CASRow("CAS02") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then CASRow("CAS03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else CASRow("CAS03") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then CASRow("CAS04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else CASRow("CAS04") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then CASRow("CAS05") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else CASRow("CAS05") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then CASRow("CAS06") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) Else CASRow("CAS06") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then CASRow("CAS07") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) Else CASRow("CAS07") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) <> "") Then CASRow("CAS08") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) Else CASRow("CAS08") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) <> "") Then CASRow("CAS09") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) Else CASRow("CAS09") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11) <> "") Then CASRow("CAS10") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11) Else CASRow("CAS10") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12) <> "") Then CASRow("CAS11") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12) Else CASRow("CAS11") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 13) <> "") Then CASRow("CAS12") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 13) Else CASRow("CAS12") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 14) <> "") Then CASRow("CAS13") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 14) Else CASRow("CAS13") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 15) <> "") Then CASRow("CAS14") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 15) Else CASRow("CAS14") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 16) <> "") Then CASRow("CAS15") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 16) Else CASRow("CAS15") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 17) <> "") Then CASRow("CAS16") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 17) Else CASRow("CAS16") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 18) <> "") Then CASRow("CAS17") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 18) Else CASRow("CAS17") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 19) <> "") Then CASRow("CAS18") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 19) Else CASRow("CAS18") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 20) <> "") Then CASRow("CAS19") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 20) Else CASRow("CAS19") = DBNull.Value


                        CASRow("ROW_NUMBER") = rowcount


                        CASRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        CASRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                        CASRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor





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

                        CAS.Rows.Add(CASRow)

                        _RowProcessedFlag = 1
                    End If

                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
                End Try



                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   Cl1
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "CL1" Then



                        Dim CL1Row As DataRow = CL1.NewRow
                        CL1Row("DOCUMENT_ID") = _DOCUMENT_ID
                        CL1Row("FILE_ID") = _FILE_ID
                        CL1Row("BATCH_ID") = _BATCH_ID
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
                        CL1Row("837_SBR_GUID") = _837_SBR_GUID
                        CL1Row("837_CLM_GUID") = _837_CLM_GUID
                        CL1Row("837_LX_GUID") = _837_LX_GUID
                        CL1Row("HL01") = _HL01
                        CL1Row("HL02") = _HL02
                        CL1Row("HL03") = _HL03
                        CL1Row("HL04") = _HL04

                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then CL1Row("CL101") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else CL1Row("CL101") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then CL1Row("CL102") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else CL1Row("CL102") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then CL1Row("CL103") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else CL1Row("CL103") = DBNull.Value


                        CL1Row("ROW_NUMBER") = rowcount

                        CL1Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        CL1Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                        CL1Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor




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


                        CL1.Rows.Add(CL1Row)

                        _RowProcessedFlag = 1
                    End If

                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
                End Try




                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   CR1
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "CR1" Then



                        Dim CR1Row As DataRow = CR1.NewRow
                        CR1Row("DOCUMENT_ID") = _DOCUMENT_ID
                        CR1Row("FILE_ID") = _FILE_ID
                        CR1Row("BATCH_ID") = _BATCH_ID
                        CR1Row("ISA_ID") = _ISA_ID
                        CR1Row("GS_ID") = _GS_ID
                        CR1Row("ST_ID") = _ST_ID
                        CR1Row("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                        CR1Row("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                        CR1Row("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                        CR1Row("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                        CR1Row("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                        CR1Row("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                        CR1Row("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                        CR1Row("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                        CR1Row("837_SBR_GUID") = _837_SBR_GUID
                        CR1Row("837_CLM_GUID") = _837_CLM_GUID
                        CR1Row("837_LX_GUID") = _837_LX_GUID
                        CR1Row("HL01") = _HL01
                        CR1Row("HL02") = _HL02
                        CR1Row("HL03") = _HL03
                        CR1Row("HL04") = _HL04

                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then CR1Row("CR101") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else CR1Row("CR101") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then CR1Row("CR102") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else CR1Row("CR102") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then CR1Row("CR103") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else CR1Row("CR103") = DBNull.Value


                        CR1Row("ROW_NUMBER") = rowcount

                        CR1Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        CR1Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                        CR1Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor




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

                        CR1.Rows.Add(CR1Row)

                        _RowProcessedFlag = 1
                    End If

                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
                End Try





                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   DMG
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "DMG" Then



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
                        DMGRow("837_SBR_GUID") = _837_SBR_GUID
                        DMGRow("837_CLM_GUID") = _837_CLM_GUID
                        DMGRow("837_LX_GUID") = _837_LX_GUID
                        DMGRow("NM1_GUID") = _NM1_GUID
                        DMGRow("HL01") = _HL01
                        DMGRow("HL02") = _HL02
                        DMGRow("HL03") = _HL03
                        DMGRow("HL04") = _HL04

                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then DMGRow("DMG01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else DMGRow("DMG01") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then DMGRow("DMG02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else DMGRow("DMG02") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then DMGRow("DMG03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else DMGRow("DMG03") = DBNull.Value

                        DMGRow("ROW_NUMBER") = rowcount
                        DMGRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        DMGRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                        DMGRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor





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

                        DMG.Rows.Add(DMGRow)

                        _RowProcessedFlag = 1
                    End If
                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
                End Try




                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   DTP
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "DTP" Then



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
                        DTPRow("837_SBR_GUID") = _837_SBR_GUID
                        DTPRow("837_CLM_GUID") = _837_CLM_GUID
                        DTPRow("837_LX_GUID") = _837_LX_GUID
                        DTPRow("837_SVD_GUID") = _837_SVD_GUID
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

                        DTP.Rows.Add(DTPRow)

                        _RowProcessedFlag = 1
                    End If

                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
                End Try





                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   HI
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "HI" Then



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
                        HIRow("837_SBR_GUID") = _837_SBR_GUID
                        HIRow("837_CLM_GUID") = _837_CLM_GUID
                        HIRow("837_LX_GUID") = _837_LX_GUID
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


                        HIRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        HIRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                        HIRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix








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


                        HI.Rows.Add(HIRow)

                        _RowProcessedFlag = 1
                    End If
                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
                End Try






                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   K3
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "K3" Then


                        Dim K3Row As DataRow = K3.NewRow
                        K3Row("DOCUMENT_ID") = _DOCUMENT_ID
                        K3Row("FILE_ID") = _FILE_ID
                        K3Row("BATCH_ID") = _BATCH_ID
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
                        K3Row("837_SBR_GUID") = _837_SBR_GUID
                        K3Row("837_CLM_GUID") = _837_CLM_GUID
                        K3Row("837_LX_GUID") = _837_LX_GUID
                        K3Row("HL01") = _HL01
                        K3Row("HL02") = _HL02
                        K3Row("HL03") = _HL03
                        K3Row("HL04") = _HL04

                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then K3Row("K301") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else K3Row("K301") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then K3Row("K302") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else K3Row("K302") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then K3Row("K303") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else K3Row("K303") = DBNull.Value

                        K3Row("ROW_NUMBER") = rowcount


                        K3Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        K3Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                        K3Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix




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

                        K3.Rows.Add(K3Row)

                        _RowProcessedFlag = 1
                    End If

                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
                End Try


                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   MIA
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                Try
                    If _RowRecordType = "MIA" Then

                        Dim MIARow As DataRow = MIA.NewRow
                        MIARow("DOCUMENT_ID") = _DOCUMENT_ID
                        MIARow("FILE_ID") = _FILE_ID
                        MIARow("BATCH_ID") = _BATCH_ID
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
                        MIARow("837_SBR_GUID") = _837_SBR_GUID
                        MIARow("837_CLM_GUID") = _837_CLM_GUID
                        MIARow("837_LX_GUID") = _837_LX_GUID
                        MIARow("HL01") = _HL01
                        MIARow("HL02") = _HL02
                        MIARow("HL03") = _HL03
                        MIARow("HL04") = _HL04


                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then MIARow("MIA01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else MIARow("MIA01") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then MIARow("MIA02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else MIARow("MIA02") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then MIARow("MIA03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else MIARow("MIA03") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then MIARow("MIA04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else MIARow("MIA04") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then MIARow("MIA05") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else MIARow("MIA05") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then MIARow("MIA06") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) Else MIARow("MIA06") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then MIARow("MIA07") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) Else MIARow("MIA07") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) <> "") Then MIARow("MIA08") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) Else MIARow("MIA08") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) <> "") Then MIARow("MIA09") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) Else MIARow("MIA09") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11) <> "") Then MIARow("MIA10") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11) Else MIARow("MIA10") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12) <> "") Then MIARow("MIA11") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12) Else MIARow("MIA11") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 13) <> "") Then MIARow("MIA12") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 13) Else MIARow("MIA12") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 14) <> "") Then MIARow("MIA13") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 14) Else MIARow("MIA13") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 15) <> "") Then MIARow("MIA14") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 15) Else MIARow("MIA14") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 16) <> "") Then MIARow("MIA15") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 16) Else MIARow("MIA15") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 17) <> "") Then MIARow("MIA16") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 17) Else MIARow("MIA16") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 18) <> "") Then MIARow("MIA17") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 18) Else MIARow("MIA17") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 19) <> "") Then MIARow("MIA18") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 19) Else MIARow("MIA18") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 20) <> "") Then MIARow("MIA19") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 20) Else MIARow("MIA19") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 21) <> "") Then MIARow("MIA20") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 21) Else MIARow("MIA20") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 22) <> "") Then MIARow("MIA21") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 22) Else MIARow("MIA21") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 23) <> "") Then MIARow("MIA22") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 23) Else MIARow("MIA22") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 24) <> "") Then MIARow("MIA23") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 24) Else MIARow("MIA23") = DBNull.Value


                        MIARow("ROW_NUMBER") = rowcount

                        MIARow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        MIARow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                        MIARow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix




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


                        MIA.Rows.Add(MIARow)

                        _RowProcessedFlag = 1
                    End If
                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
                End Try


                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '  LIN
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try

                    If _RowRecordType = "LIN" Then




                        Dim LINRow As DataRow = LIN.NewRow
                        LINRow("DOCUMENT_ID") = _DOCUMENT_ID
                        LINRow("FILE_ID") = _FILE_ID
                        LINRow("BATCH_ID") = _BATCH_ID
                        LINRow("ISA_ID") = _ISA_ID
                        LINRow("GS_ID") = _GS_ID
                        LINRow("ST_ID") = _ST_ID
                        LINRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                        LINRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                        LINRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                        LINRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                        LINRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                        LINRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                        LINRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                        LINRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                        LINRow("837_SBR_GUID") = _837_SBR_GUID
                        LINRow("837_CLM_GUID") = _837_CLM_GUID
                        LINRow("837_LX_GUID") = _837_LX_GUID
                        LINRow("837_SVD_GUID") = _837_SVD_GUID
                        LINRow("HL01") = _HL01
                        LINRow("HL02") = _HL02
                        LINRow("HL03") = _HL03
                        LINRow("HL04") = _HL04


                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then LINRow("LIN01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else LINRow("LIN01") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then LINRow("LIN02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else LINRow("LIN02") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then LINRow("LIN03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else LINRow("LIN03") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then LINRow("LIN04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else LINRow("LIN04") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then LINRow("LIN05") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else LINRow("LIN05") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then LINRow("LIN06") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) Else LINRow("LIN06") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then LINRow("LIN07") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) Else LINRow("LIN07") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) <> "") Then LINRow("LIN08") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) Else LINRow("LIN08") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) <> "") Then LINRow("LIN09") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) Else LINRow("LIN09") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11) <> "") Then LINRow("LIN10") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11) Else LINRow("LIN10") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12) <> "") Then LINRow("LIN11") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12) Else LINRow("LIN11") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 13) <> "") Then LINRow("LIN12") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 13) Else LINRow("LIN12") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 14) <> "") Then LINRow("LIN13") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 14) Else LINRow("LIN13") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 15) <> "") Then LINRow("LIN14") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 15) Else LINRow("LIN14") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 16) <> "") Then LINRow("LIN15") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 16) Else LINRow("LIN15") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 17) <> "") Then LINRow("LIN16") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 17) Else LINRow("LIN16") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 18) <> "") Then LINRow("LIN17") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 18) Else LINRow("LIN17") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 19) <> "") Then LINRow("LIN18") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 19) Else LINRow("LIN18") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 20) <> "") Then LINRow("LIN19") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 20) Else LINRow("LIN19") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 21) <> "") Then LINRow("LIN20") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 21) Else LINRow("LIN20") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 22) <> "") Then LINRow("LIN21") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 22) Else LINRow("LIN21") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 23) <> "") Then LINRow("LIN22") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 23) Else LINRow("LIN22") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 24) <> "") Then LINRow("LIN23") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 24) Else LINRow("LIN23") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 25) <> "") Then LINRow("LIN24") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 25) Else LINRow("LIN24") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 26) <> "") Then LINRow("LIN25") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 26) Else LINRow("LIN25") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 27) <> "") Then LINRow("LIN26") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 27) Else LINRow("LIN26") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 28) <> "") Then LINRow("LIN27") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 28) Else LINRow("LIN27") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 29) <> "") Then LINRow("LIN28") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 29) Else LINRow("LIN28") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 30) <> "") Then LINRow("LIN29") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 30) Else LINRow("LIN29") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 31) <> "") Then LINRow("LIN30") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 31) Else LINRow("LIN30") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 32) <> "") Then LINRow("LIN31") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 32) Else LINRow("LIN31") = DBNull.Value



                        LINRow("ROW_NUMBER") = rowcount

                        LINRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        LINRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                        LINRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix




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

                        LIN.Rows.Add(LINRow)

                        _RowProcessedFlag = 1
                    End If
                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
                End Try




                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '  CRC
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try

                    If _RowRecordType = "CRC" Then




                        Dim CRCRow As DataRow = CRC.NewRow
                        CRCRow("DOCUMENT_ID") = _DOCUMENT_ID
                        CRCRow("FILE_ID") = _FILE_ID
                        CRCRow("BATCH_ID") = _BATCH_ID
                        CRCRow("ISA_ID") = _ISA_ID
                        CRCRow("GS_ID") = _GS_ID
                        CRCRow("ST_ID") = _ST_ID
                        CRCRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                        CRCRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                        CRCRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                        CRCRow("HIPAA_HL_20_GUID") = _HIPAA_HL_20_GUID
                        CRCRow("HIPAA_HL_21_GUID") = _HIPAA_HL_21_GUID
                        CRCRow("HIPAA_HL_22_GUID") = _HIPAA_HL_22_GUID
                        CRCRow("HIPAA_HL_23_GUID") = _HIPAA_HL_23_GUID
                        CRCRow("HIPAA_HL_24_GUID") = _HIPAA_HL_24_GUID
                        CRCRow("837_SBR_GUID") = _837_SBR_GUID
                        CRCRow("837_CLM_GUID") = _837_CLM_GUID
                        CRCRow("837_LX_GUID") = _837_LX_GUID
                        CRCRow("837_SVD_GUID") = _837_SVD_GUID
                        CRCRow("HL01") = _HL01
                        CRCRow("HL02") = _HL02
                        CRCRow("HL03") = _HL03
                        CRCRow("HL04") = _HL04

                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then CRCRow("CRC01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else CRCRow("CRC01") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then CRCRow("CRC02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else CRCRow("CRC02") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then CRCRow("CRC03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else CRCRow("CRC03") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then CRCRow("CRC04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else CRCRow("CRC04") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then CRCRow("CRC05") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else CRCRow("CRC05") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then CRCRow("CRC06") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) Else CRCRow("CRC06") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then CRCRow("CRC07") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) Else CRCRow("CRC07") = DBNull.Value



                        CRCRow("ROW_NUMBER") = rowcount

                        CRCRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        CRCRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                        CRCRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix




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

                        CRC.Rows.Add(CRCRow)

                        _RowProcessedFlag = 1
                    End If
                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
                End Try




                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   MOA
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "MOA" Then

                        Dim MOARow As DataRow = MOA.NewRow
                        MOARow("DOCUMENT_ID") = _DOCUMENT_ID
                        MOARow("FILE_ID") = _FILE_ID
                        MOARow("BATCH_ID") = _BATCH_ID
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
                        MOARow("837_SBR_GUID") = _837_SBR_GUID
                        MOARow("837_CLM_GUID") = _837_CLM_GUID
                        MOARow("837_LX_GUID") = _837_LX_GUID
                        MOARow("HL01") = _HL01
                        MOARow("HL02") = _HL02
                        MOARow("HL03") = _HL03
                        MOARow("HL04") = _HL04


                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then MOARow("MOA01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else MOARow("MOA01") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then MOARow("MOA02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else MOARow("MOA02") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then MOARow("MOA03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else MOARow("MOA03") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then MOARow("MOA04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else MOARow("MOA04") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then MOARow("MOA05") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else MOARow("MOA05") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then MOARow("MOA06") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) Else MOARow("MOA06") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then MOARow("MOA07") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) Else MOARow("MOA07") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) <> "") Then MOARow("MOA08") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) Else MOARow("MOA08") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) <> "") Then MOARow("MOA09") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) Else MOARow("MOA09") = DBNull.Value


                        MOARow("ROW_NUMBER") = rowcount

                        MOARow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        MOARow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                        MOARow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix




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

                        MOA.Rows.Add(MOARow)

                        _RowProcessedFlag = 1
                    End If
                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
                End Try






                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   N3
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "N3" Then


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
                        N3Row("837_SBR_GUID") = _837_SBR_GUID
                        N3Row("837_CLM_GUID") = _837_CLM_GUID
                        N3Row("837_LX_GUID") = _837_LX_GUID
                        N3Row("NM1_GUID") = _NM1_GUID
                        N3Row("HL01") = _HL01
                        N3Row("HL02") = _HL02
                        N3Row("HL03") = _HL03
                        N3Row("HL04") = _HL04

                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then N3Row("N301") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else N3Row("N301") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then N3Row("N302") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else N3Row("N302") = DBNull.Value




                        N3Row("ROW_NUMBER") = rowcount


                        N3Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        N3Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                        N3Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


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

                        N3.Rows.Add(N3Row)

                        _RowProcessedFlag = 1
                    End If
                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
                End Try



                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   N4
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
                        N4Row("837_SBR_GUID") = _837_SBR_GUID
                        N4Row("837_CLM_GUID") = _837_CLM_GUID
                        N4Row("837_LX_GUID") = _837_LX_GUID
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


                        N4Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        N4Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                        N4Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix








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

                        N4.Rows.Add(N4Row)

                        _RowProcessedFlag = 1
                    End If

                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
                End Try




                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   NTE
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "NTE" Then



                        Dim NTERow As DataRow = NTE.NewRow
                        NTERow("DOCUMENT_ID") = _DOCUMENT_ID
                        NTERow("FILE_ID") = _FILE_ID
                        NTERow("BATCH_ID") = _BATCH_ID
                        NTERow("FILE_ID") = _FILE_ID
                        NTERow("GS_ID") = _ISA_ID
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
                        NTERow("837_SBR_GUID") = _837_SBR_GUID
                        NTERow("837_CLM_GUID") = _837_CLM_GUID
                        NTERow("837_LX_GUID") = _837_LX_GUID
                        NTERow("HL01") = _HL01
                        NTERow("HL02") = _HL02
                        NTERow("HL03") = _HL03
                        NTERow("HL04") = _HL04



                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then NTERow("NTE01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else NTERow("NTE01") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then NTERow("NTE02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else NTERow("NTE02") = DBNull.Value


                        NTERow("ROW_NUMBER") = rowcount

                        NTERow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        NTERow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                        NTERow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix






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

                        NTE.Rows.Add(NTERow)

                        _RowProcessedFlag = 1
                    End If
                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
                End Try




                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   OI
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "OI" Then



                        Dim OIRow As DataRow = OI.NewRow
                        OIRow("DOCUMENT_ID") = _DOCUMENT_ID
                        OIRow("FILE_ID") = _FILE_ID
                        OIRow("BATCH_ID") = _BATCH_ID
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
                        OIRow("837_SBR_GUID") = _837_SBR_GUID
                        OIRow("837_CLM_GUID") = _837_CLM_GUID
                        OIRow("837_LX_GUID") = _837_LX_GUID
                        OIRow("HL01") = _HL01
                        OIRow("HL02") = _HL02
                        OIRow("HL03") = _HL03
                        OIRow("HL04") = _HL04

                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then OIRow("OI01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else OIRow("OI01") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then OIRow("OI02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else OIRow("OI02") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then OIRow("OI03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else OIRow("OI03") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then OIRow("OI04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else OIRow("OI04") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then OIRow("OI05") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else OIRow("OI05") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then OIRow("OI06") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) Else OIRow("OI06") = DBNull.Value

                        OIRow("ROW_NUMBER") = rowcount

                        OIRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        OIRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                        OIRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix





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

                        OI.Rows.Add(OIRow)

                        _RowProcessedFlag = 1
                    End If

                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
                End Try



                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   PAT
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "PAT" Then


                        Dim PATRow As DataRow = PAT.NewRow
                        PATRow("DOCUMENT_ID") = _DOCUMENT_ID
                        PATRow("FILE_ID") = _FILE_ID
                        PATRow("BATCH_ID") = _BATCH_ID
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
                        PATRow("837_SBR_GUID") = _837_SBR_GUID
                        PATRow("837_CLM_GUID") = _837_CLM_GUID
                        PATRow("837_LX_GUID") = _837_LX_GUID
                        PATRow("HL01") = _HL01
                        PATRow("HL02") = _HL02
                        PATRow("HL03") = _HL03
                        PATRow("HL04") = _HL04

                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then PATRow("PAT01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else PATRow("PAT01") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then PATRow("PAT02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else PATRow("PAT02") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then PATRow("PAT03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else PATRow("PAT03") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then PATRow("PAT04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else PATRow("PAT04") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then PATRow("PAT05") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else PATRow("PAT05") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then PATRow("PAT06") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) Else PATRow("PAT06") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then PATRow("PAT07") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) Else PATRow("PAT07") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) <> "") Then PATRow("PAT08") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) Else PATRow("PAT08") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) <> "") Then PATRow("PAT09") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) Else PATRow("PAT09") = DBNull.Value

                        PATRow("ROW_NUMBER") = rowcount

                        PATRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        PATRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                        PATRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix






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

                        PAT.Rows.Add(PATRow)

                        _RowProcessedFlag = 1
                    End If
                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
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
                        PERRow("837_SBR_GUID") = _837_SBR_GUID
                        PERRow("837_CLM_GUID") = _837_CLM_GUID
                        PERRow("837_LX_GUID") = _837_LX_GUID
                        PERRow("NM1_GUID") = _NM1_GUID
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
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
                End Try










                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   PRV
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                Try
                    If _RowRecordType = "PRV" Then
                        _RowProcessedFlag = 1

                        Dim PRVRow As DataRow = PRV.NewRow
                        PRVRow("DOCUMENT_ID") = _DOCUMENT_ID
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
                        PRVRow("837_SBR_GUID") = _837_SBR_GUID
                        PRVRow("837_CLM_GUID") = _837_CLM_GUID
                        PRVRow("837_LX_GUID") = _837_LX_GUID
                        PRVRow("HL01") = _HL01
                        PRVRow("HL02") = _HL02
                        PRVRow("HL03") = _HL03
                        PRVRow("HL04") = _HL04

                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then PRVRow("PRV01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else PRVRow("PRV01") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then PRVRow("PRV02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else PRVRow("PRV02") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then PRVRow("PRV03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else PRVRow("PRV03") = DBNull.Value

                        PRVRow("ROW_NUMBER") = rowcount

                        PRVRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        PRVRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                        PRVRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                        PRV.Rows.Add(PRVRow)




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
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
                End Try






                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   PWK
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "PWK" Then

                        _RowProcessedFlag = 1

                        Dim PWKRow As DataRow = PWK.NewRow
                        PWKRow("DOCUMENT_ID") = _DOCUMENT_ID
                        PWKRow("FILE_ID") = _FILE_ID
                        PWKRow("BATCH_ID") = _BATCH_ID
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
                        PWKRow("837_SBR_GUID") = _837_SBR_GUID
                        PWKRow("837_CLM_GUID") = _837_CLM_GUID
                        PWKRow("837_LX_GUID") = _837_LX_GUID
                        PWKRow("HL01") = _HL01
                        PWKRow("HL02") = _HL02
                        PWKRow("HL03") = _HL03
                        PWKRow("HL04") = _HL04


                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then PWKRow("PWK01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else PWKRow("PWK01") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then PWKRow("PWK02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else PWKRow("PWK02") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then PWKRow("PWK03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else PWKRow("PWK03") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then PWKRow("PWK04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else PWKRow("PWK04") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then PWKRow("PWK05") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else PWKRow("PWK05") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then PWKRow("PWK06") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) Else PWKRow("PWK06") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then PWKRow("PWK07") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) Else PWKRow("PWK07") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) <> "") Then PWKRow("PWK08") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) Else PWKRow("PWK08") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) <> "") Then PWKRow("PWK09") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) Else PWKRow("PWK09") = DBNull.Value

                        PWKRow("ROW_NUMBER") = rowcount

                        PWKRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        PWKRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                        PWKRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                        PWK.Rows.Add(PWKRow)



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
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
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
                        REFRow("837_SBR_GUID") = _837_SBR_GUID
                        REFRow("837_CLM_GUID") = _837_CLM_GUID
                        REFRow("837_LX_GUID") = _837_LX_GUID
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
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
                End Try









                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   SV2
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try


                    If _RowRecordType = "SV2" Then

                        SV202 = String.Empty

                        _RowProcessedFlag = 1

                        Dim SV2Row As DataRow = SV2.NewRow
                        SV2Row("DOCUMENT_ID") = _DOCUMENT_ID
                        SV2Row("FILE_ID") = _FILE_ID
                        SV2Row("BATCH_ID") = _BATCH_ID
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
                        SV2Row("837_SBR_GUID") = _837_SBR_GUID
                        SV2Row("837_CLM_GUID") = _837_CLM_GUID
                        SV2Row("837_LX_GUID") = _837_LX_GUID
                        SV2Row("HL01") = _HL01
                        SV2Row("HL02") = _HL02
                        SV2Row("HL03") = _HL03
                        SV2Row("HL04") = _HL04

                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then SV2Row("SV201") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else SV2Row("SV201") = DBNull.Value


                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then
                            SV2Row("SV202") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3)
                            SV202 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3)
                        Else
                            SV2Row("SV202") = DBNull.Value

                        End If

                        If Not SV202 = String.Empty Then

                            If (ss.ParseDemlimtedString(SV202, _ComponentElementSeparator, 1) <> "") Then SV2Row("SV202_1") = ss.ParseDemlimtedString(SV202, _ComponentElementSeparator, 1) Else SV2Row("SV202_1") = DBNull.Value
                            If (ss.ParseDemlimtedString(SV202, _ComponentElementSeparator, 2) <> "") Then SV2Row("SV202_2") = ss.ParseDemlimtedString(SV202, _ComponentElementSeparator, 2) Else SV2Row("SV202_2") = DBNull.Value
                            If (ss.ParseDemlimtedString(SV202, _ComponentElementSeparator, 3) <> "") Then SV2Row("SV202_3") = ss.ParseDemlimtedString(SV202, _ComponentElementSeparator, 3) Else SV2Row("SV202_3") = DBNull.Value
                            If (ss.ParseDemlimtedString(SV202, _ComponentElementSeparator, 4) <> "") Then SV2Row("SV202_4") = ss.ParseDemlimtedString(SV202, _ComponentElementSeparator, 4) Else SV2Row("SV202_4") = DBNull.Value
                            If (ss.ParseDemlimtedString(SV202, _ComponentElementSeparator, 5) <> "") Then SV2Row("SV202_5") = ss.ParseDemlimtedString(SV202, _ComponentElementSeparator, 5) Else SV2Row("SV202_5") = DBNull.Value
                            If (ss.ParseDemlimtedString(SV202, _ComponentElementSeparator, 6) <> "") Then SV2Row("SV202_6") = ss.ParseDemlimtedString(SV202, _ComponentElementSeparator, 6) Else SV2Row("SV202_6") = DBNull.Value
                            If (ss.ParseDemlimtedString(SV202, _ComponentElementSeparator, 7) <> "") Then SV2Row("SV202_7") = ss.ParseDemlimtedString(SV202, _ComponentElementSeparator, 7) Else SV2Row("SV202_7") = DBNull.Value
                        End If

                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then SV2Row("SV203") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else SV2Row("SV203") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then SV2Row("SV204") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else SV2Row("SV204") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then SV2Row("SV205") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else SV2Row("SV205") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then SV2Row("SV206") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) Else SV2Row("SV206") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then SV2Row("SV207") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) Else SV2Row("SV207") = DBNull.Value





                        SV2Row("ROW_NUMBER") = rowcount

                        SV2Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        SV2Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                        SV2Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                        SV2.Rows.Add(SV2Row)




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
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
                End Try










                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   END 837
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
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
                End Try



                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   GE
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "GE" Then
                        _RowProcessedFlag = 1


                        Try



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
                        Catch ex As Exception


                        End Try
                        '_GSFlag = 0
                        '_GS_GUID = Guid.Empty


                    End If
                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
                End Try



                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   IEA
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "IEA" Then

                        _RowProcessedFlag = 1

                        Dim IEARow As DataRow = IEA.NewRow
                        IEARow("FILE_ID") = _FILE_ID


                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then IEARow("IEA01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else IEARow("IEA01") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then IEARow("IEA02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else IEARow("IEA02") = DBNull.Value

                        IEARow("ROW_NUMBER") = rowcount
                        IEA.Rows.Add(IEARow)


                        ComitIEA()
                        _LAST_ISA_ID = _LAST_ISA_ID + " - " + Convert.ToString(_ISA_ID)
                        ClearIAS()



                    End If
                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "GS", ex)
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
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "UNK", ex)
                End Try


            Next





            If _hasUNK Then

                If (_IS_FILE) Then


                    Using e As New EDI_5010_LOGGING
                        e.ConnectionString = _ConnectionString
                        e.TransactionSetIdentifierCode = "837"
                        If _hasERR Then
                            e.UpdateFileStatus(CInt(_FILE_ID), "PARSE COMPLETE WITH UNK SEGMENTS WITH ERRORS", "837")
                            _IMPORT_RETURN_STRING = "837 : PARSE COMPLETE WITH UNK SEGMENTS WITH ERRORS"
                        Else
                            e.UpdateFileStatus(CInt(_FILE_ID), "PARSE COMPLETE WITH UNK SEGMENTS", "837")
                            _IMPORT_RETURN_STRING = "837 : PARSE COMPLETE WITH UNK SEGMENTS"
                        End If



                    End Using

                Else
                    If _hasERR Then

                        _IMPORT_RETURN_STRING = "837 : PARSE COMPLETE WITH UNK SEGMENTS WITH ERRORS"
                    Else

                        _IMPORT_RETURN_STRING = "837 : PARSE COMPLETE WITH UNK SEGMENTS"
                    End If

                End If
                ComitUNK()

            Else
                If (_IS_FILE) Then


                    Using e As New EDI_5010_LOGGING
                        e.ConnectionString = _ConnectionString
                        e.TransactionSetIdentifierCode = "837"
                        If _hasERR Then
                            e.UpdateFileStatus(CInt(_FILE_ID), "PARSE COMPLETE WITH ERRORS", "837")
                            _IMPORT_RETURN_STRING = "837 : PARSE COMPLETE WITH ERRORS"
                        Else
                            e.UpdateFileStatus(CInt(_FILE_ID), "PARSE COMPLETE", "837")
                            _IMPORT_RETURN_STRING = "837 : PARSE COMPLETE"
                        End If
                    End Using
                Else

                    If _hasERR Then

                        _IMPORT_RETURN_STRING = "837 : PARSE COMPLETE WITH ERRORS"
                    Else

                        _IMPORT_RETURN_STRING = "837 : PARSE COMPLETE"
                    End If


                End If




            End If



            'If _hasERR Then
            '    RollBack()
            '    _IMPORT_RETURN_STRING = _IMPORT_RETURN_STRING + " ROLLED BACK EDI_SEQUENCE_NUMBER " + Convert.ToString(_EDI_SEQUENCE_NUMBER)
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

            'Try

            '    'Using Con As New SqlConnection(_ConnectionString)
            '    '    Con.Open()
            '    '    Using cmd As New SqlCommand(_SP_ROLLBACK, Con)

            '    '        cmd.CommandType = CommandType.StoredProcedure

            '    '        cmd.Parameters.AddWithValue("@EDI_SEQUENCE_NUMBER", _EDI_SEQUENCE_NUMBER)


            '    '        cmd.ExecuteNonQuery()

            '    '    End Using
            '    '    Con.Close()
            '    'End Using

            '    _RETURN_CODE = 0

            'Catch ex As Exception
            '    _hasERR = True
            '    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "", ex)
            'End Try

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




        Private Function ComitISA() As Integer
            Dim _RETURN_CODE As Integer = -1

            _FUNCTION_NAME = "Function ComitISA() As Integer"




            Try
                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_ISA, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_837_ISA", ISA)
                        cmd.Parameters.Add("@ISA_ID", Data.SqlDbType.BigInt, 1)
                        cmd.Parameters("@ISA_ID").Direction = ParameterDirection.Output
                        cmd.ExecuteNonQuery()

                        _ISA_ID = Convert.ToInt32(cmd.Parameters("@ISA_ID").Value.ToString())
                    End Using
                    Con.Close()
                End Using

            Catch ex As Exception
                _hasERR = True
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "", ex)
            End Try

            Return _RETURN_CODE

        End Function

        Private Function ComitGS() As Integer

            Dim _RETURN_CODE As Integer = -1
            _FUNCTION_NAME = "Function ComitGS() As Integer"


            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_GS, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_837_GS", GS)

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

        Private Function ComitST() As Integer

            Dim _RETURN_CODE As Integer = -1
            _FUNCTION_NAME = "Function ComitST() As INTEGER"



            Try



                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_ST, Con)

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
                _RETURN_CODE = 0

            Catch ex As Exception
                _hasERR = True
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "", ex)
            End Try

            Return _RETURN_CODE

        End Function

        Private Function ComitHeader() As Integer
            Dim _RETURN_CODE As Integer = -1
            _FUNCTION_NAME = "Function ComitHEADER() As INTEGER"



            Try
                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_HEADERS, Con)

                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.AddWithValue("@HIPAA_837_NM1", NM1)
                        cmd.Parameters.AddWithValue("@HIPAA_837_PER", PER)


                        cmd.ExecuteNonQuery()
                    End Using
                    Con.Close()
                End Using

                CACHE_1000_NM1 = NM1.Copy
                CACHE_1000_PER = PER.Copy


                NM1.Clear()
                PER.Clear()
                _NM1_GUID = Guid.Empty

                _RETURN_CODE = 0

            Catch ex As Exception
                _hasERR = True
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "", ex)
            End Try



            Return _RETURN_CODE

        End Function


        Private Function ComitHL20() As Integer
            Dim _RETURN_CODE As Integer = -1

            _FUNCTION_NAME = "Function ComitHL20() As Integer"



            Try
                _is_HL20_CACHED = True

                CACHE_2000_HL = HL.Copy

                CACHE_2000_N3 = N3.Copy
                CACHE_2000_N4 = N4.Copy
                CACHE_2000_NM1 = NM1.Copy


                CACHE_2000_PER = PER.Copy
                CACHE_2000_PRV = PRV.Copy


                CACHE_2000_REF = REF.Copy



                HL.Clear()

                N3.Clear() ' As New DataTable
                N4.Clear() ' As New DataTable
                NM1.Clear() ' As New DataTable


                PER.Clear() ' As New DataTable
                PRV.Clear() ' As New DataTable

                REF.Clear()

                _RETURN_CODE = 0

            Catch ex As Exception
                _hasERR = True
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "", ex)
            End Try


            Return _RETURN_CODE

        End Function






        Private Function ComitHL22() As Integer

            Dim _RETURN_CODE As Integer = -1

            _FUNCTION_NAME = "Function ComitHL22() As Integer"



            Try



                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_COMIT_HL22_DATA, Con)

                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.AddWithValue("@HIPAA_837_SBR", SBR)
                        cmd.Parameters.AddWithValue("@HIPAA_837_HL", HL)
                        cmd.Parameters.AddWithValue("@HIPAA_837_CLM", CLM)
                        ' ''*     
                        cmd.Parameters.AddWithValue("@HIPAA_837_LX", LX)

                        cmd.Parameters.AddWithValue("@HIPAA_837_AMT", AMT)

                        cmd.Parameters.AddWithValue("@HIPAA_837_CAS", CAS)
                        cmd.Parameters.AddWithValue("@HIPAA_837_CL1", CL1)
                        cmd.Parameters.AddWithValue("@HIPAA_837_CR1", CR1)
                        cmd.Parameters.AddWithValue("@HIPAA_837_CRC", CRC)


                        cmd.Parameters.AddWithValue("@HIPAA_837_DMG", DMG)
                        cmd.Parameters.AddWithValue("@HIPAA_837_DTP", DTP)
                        ''* 
                        cmd.Parameters.AddWithValue("@HIPAA_837_HI", HI)
                        ' ''* 
                        cmd.Parameters.AddWithValue("@HIPAA_837_K3", K3)

                        cmd.Parameters.AddWithValue("@HIPAA_837_LIN", LIN)

                        cmd.Parameters.AddWithValue("@HIPAA_837_MIA", MIA)
                        cmd.Parameters.AddWithValue("@HIPAA_837_MOA", MOA)

                        cmd.Parameters.AddWithValue("@HIPAA_837_N3", N3)
                        cmd.Parameters.AddWithValue("@HIPAA_837_N4", N4)
                        cmd.Parameters.AddWithValue("@HIPAA_837_NM1", NM1)
                        cmd.Parameters.AddWithValue("@HIPAA_837_NTE", NTE)

                        cmd.Parameters.AddWithValue("@HIPAA_837_OI", OI)

                        cmd.Parameters.AddWithValue("@HIPAA_837_PAT", PAT)
                        cmd.Parameters.AddWithValue("@HIPAA_837_PER", PER)
                        cmd.Parameters.AddWithValue("@HIPAA_837_PRV", PRV)
                        cmd.Parameters.AddWithValue("@HIPAA_837_PWK", PWK)

                        cmd.Parameters.AddWithValue("@HIPAA_837_REF", REF)

                        cmd.Parameters.AddWithValue("@HIPAA_837_SV1", SV1)
                        cmd.Parameters.AddWithValue("@HIPAA_837_SV2", SV2)
                        cmd.Parameters.AddWithValue("@HIPAA_837_SV5", SV5)
                        cmd.Parameters.AddWithValue("@HIPAA_837_SVD", SVD)


                        cmd.Parameters.AddWithValue("@HIPAA_CACHE_837_ISA_A", ISA)
                        cmd.Parameters.AddWithValue("@HIPAA_CACHE_837_GS_A", GS)

                        cmd.Parameters.AddWithValue("@HIPAA_CACHE_837_ST_B", ST)
                        cmd.Parameters.AddWithValue("@HIPAA_CACHE_837_BHT_B", BHT)

                        cmd.Parameters.AddWithValue("@HIPAA_CACHE_837_NM1_1000", CACHE_1000_NM1)
                        cmd.Parameters.AddWithValue("@HIPAA_CACHE_837_PER_1000", CACHE_1000_PER)


                        ''*
                        '  cmd.Parameters.AddWithValue("@HIPAA_CACHE_837_DMG_2000", CACHE_2000_DMG)

                        cmd.Parameters.AddWithValue("@HIPAA_CACHE_837_HL_2000", CACHE_2000_HL)

                        cmd.Parameters.AddWithValue("@HIPAA_CACHE_837_N3_2000", CACHE_2000_N3)
                        cmd.Parameters.AddWithValue("@HIPAA_CACHE_837_N4_2000", CACHE_2000_N4)
                        cmd.Parameters.AddWithValue("@HIPAA_CACHE_837_NM1_2000", CACHE_2000_NM1)

                        ' cmd.Parameters.AddWithValue("@HIPAA_CACHE_837_PAT_2000", CACHE_2000_PAT)
                        cmd.Parameters.AddWithValue("@HIPAA_CACHE_837_PER_2000", CACHE_2000_PER)
                        cmd.Parameters.AddWithValue("@HIPAA_CACHE_837_PRV_2000", CACHE_2000_PRV)

                        cmd.Parameters.AddWithValue("@HIPAA_CACHE_837_REF_2000", CACHE_2000_REF)
                        ' cmd.Parameters.AddWithValue("@HIPAA_CACHE_837_SBR_2000", CACHE_2000_SBR)






                        cmd.Parameters.AddWithValue("@FILE_ID", _FILE_ID)

                        cmd.Parameters.AddWithValue("@ISA_GUID", _HIPAA_ISA_GUID)
                        cmd.Parameters.AddWithValue("@GS_GUID", _HIPAA_GS_GUID)
                        cmd.Parameters.AddWithValue("@ST_GUID", _HIPAA_ST_GUID)

                        cmd.Parameters.AddWithValue("@HIPAA_HL_20_GUID", _HIPAA_HL_20_GUID)
                        cmd.Parameters.AddWithValue("@HIPAA_HL_22_GUID", _HIPAA_HL_22_GUID)

                        If _is_HL20_ON_DISC Then
                            cmd.Parameters.AddWithValue("@COMIT_20", 0)

                        Else
                            cmd.Parameters.AddWithValue("@COMIT_20", 1)
                            _is_HL20_ON_DISC = True
                        End If



                        If _RAW_HEADER_BUILT = False Then
                            _RAW_HEADER = _ISA + _GS + _ST + _BHT + _RAW_HEADER
                            _RAW_HEADER_BUILT = True
                        End If

                        cmd.Parameters.AddWithValue("@RAW_HEADER", _RAW_HEADER)
                        cmd.Parameters.AddWithValue("@RAW_20", _RAW_20)
                        cmd.Parameters.AddWithValue("@RAW_22", _RAW_22)






                        '  cmd.Parameters.AddWithValue("@DOCUMENT_TYPE", _DocumentType)
                        cmd.ExecuteNonQuery()
                    End Using
                    Con.Close()
                End Using

                ClearLoopHL22()
                '      ClearLoop1000()
                _RETURN_CODE = 0

            Catch ex As Exception
                _hasERR = True
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "", ex)
            End Try


            Return _RETURN_CODE

        End Function






        Private Function ComitSE() As Int32
            Dim _RETURN_CODE As Integer = -1

            Try
                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_SE, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_837_SE", SE)
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






        Private Function ComitGE() As Integer
            Dim _RETURN_CODE As Integer = -1

            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_GE, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_837_GE", GE)

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







        Private Function ComitIEA() As Integer

            Dim _RETURN_CODE As Integer = -1


            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_IEA, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_837_IEA", IEA)
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

            Dim _RETURN_CODE As Integer = -1



            'Try


            '    Using Con As New SqlConnection(_ConnectionString)
            '        Con.Open()
            '        Using cmd As New SqlCommand(_SP_COMIT_HL20_UNIT_TEST, Con)

            '            cmd.CommandType = CommandType.StoredProcedure

            '            cmd.Parameters.AddWithValue("@HIPAA_837_HL", HL)
            '            cmd.Parameters.AddWithValue("@HIPAA_837_NM1", NM1)
            '            cmd.Parameters.AddWithValue("@HIPAA_837_PER", PER)
            '            cmd.Parameters.AddWithValue("@HIPAA_837_TRN", TRN)
            '            cmd.Parameters.AddWithValue("@HIPAA_837_STC", STC)

            '            cmd.ExecuteNonQuery()

            '        End Using
            '        Con.Close()
            '    End Using

            '    r = 0
            '    _HL20_SP_TEST = True
            'Catch ex As Exception
            '    _HL20_SP_TEST = False
            '    log.ExceptionDetails("HL 20 TEST FAILED 005010X212_837", ex)
            'End Try




            Try



                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_COMIT_HL22_UNIT_TEST, Con)

                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.AddWithValue("@HIPAA_837_SBR", SBR)
                        cmd.Parameters.AddWithValue("@HIPAA_837_HL", HL)
                        cmd.Parameters.AddWithValue("@HIPAA_837_CLM", CLM)


                        cmd.Parameters.AddWithValue("@HIPAA_837_LX", LX)


                        cmd.Parameters.AddWithValue("@HIPAA_837_AMT", AMT)

                        cmd.Parameters.AddWithValue("@HIPAA_837_CAS", CAS)
                        cmd.Parameters.AddWithValue("@HIPAA_837_CL1", CL1)
                        cmd.Parameters.AddWithValue("@HIPAA_837_CR1", CR1)
                        cmd.Parameters.AddWithValue("@HIPAA_837_CRC", CRC)

                        'cmd.Parameters.AddWithValue("@HIPAA_837_DMG", REF)
                        'cmd.Parameters.AddWithValue("@HIPAA_837_DTP", DTP)
                        'cmd.Parameters.AddWithValue("@HIPAA_837_HI", HI)
                        'cmd.Parameters.AddWithValue("@HIPAA_837_K3", K3)
                        cmd.Parameters.AddWithValue("@HIPAA_837_LIN", LIN)




                        cmd.Parameters.AddWithValue("@HIPAA_837_N3", N3)
                        cmd.Parameters.AddWithValue("@HIPAA_837_N4", N4)
                        cmd.Parameters.AddWithValue("@HIPAA_837_NM1", NM1)
                        cmd.Parameters.AddWithValue("@HIPAA_837_NTE", NTE)

                        cmd.Parameters.AddWithValue("@HIPAA_837_MIA", MIA)
                        cmd.Parameters.AddWithValue("@HIPAA_837_MOA", MOA)


                        cmd.Parameters.AddWithValue("@HIPAA_837_OI", OI)


                        cmd.Parameters.AddWithValue("@HIPAA_837_PAT", PAT)
                        cmd.Parameters.AddWithValue("@HIPAA_837_PER", PER)
                        cmd.Parameters.AddWithValue("@HIPAA_837_PRV", PRV)
                        cmd.Parameters.AddWithValue("@HIPAA_837_PWK", PWK)

                        cmd.Parameters.AddWithValue("@HIPAA_837_REF", REF)


                        cmd.Parameters.AddWithValue("@HIPAA_837_SV1", SV1)
                        cmd.Parameters.AddWithValue("@HIPAA_837_SV2", SV2)
                        cmd.Parameters.AddWithValue("@HIPAA_837_SV5", SV5)
                        cmd.Parameters.AddWithValue("@HIPAA_837_SVD", SVD)



                        'cmd.Parameters.AddWithValue("@HIPAA_837_CACHE_HL", CACHE_HL)
                        'cmd.Parameters.AddWithValue("@HIPAA_837_CACHE_NM1", CACHE_NM1)
                        'cmd.Parameters.AddWithValue("@HIPAA_837_CACHE_PER", CACHE_PER)
                        'cmd.Parameters.AddWithValue("@HIPAA_837_CACHE_TRN", CACHE_TRN)
                        'cmd.Parameters.AddWithValue("@HIPAA_837_CACHE_STC", CACHE_STC)

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
 

        Private Sub ClearIAS()

            ISA.Clear()
            IEA.Clear()
            _HIPAA_ISA_GUID = Guid.Empty
            _ISA_ID = 0

            _ISA = String.Empty

            _ISA01 = String.Empty
            _ISA02 = String.Empty
            _ISA03 = String.Empty
            _ISA04 = String.Empty
            _ISA05 = String.Empty
            _ISA06 = String.Empty
            _ISA07 = String.Empty
            _ISA08 = String.Empty
            _ISA09 = String.Empty
            _ISA10 = String.Empty
            _ISA11 = String.Empty
            _ISA12 = String.Empty
            _ISA13 = String.Empty
            _ISA14 = String.Empty
            _ISA15 = String.Empty
            _ISA16 = String.Empty


            _HIPAA_HL_20_GUID = Guid.Empty
            _HIPAA_HL_21_GUID = Guid.Empty
            _HIPAA_HL_22_GUID = Guid.Empty
            _HIPAA_HL_23_GUID = Guid.Empty
            _HIPAA_HL_24_GUID = Guid.Empty

            _837_SBR_GUID = Guid.Empty
            _837_CLM_GUID = Guid.Empty
            _837_LX_GUID = Guid.Empty
            _NM1_GUID = Guid.Empty

            _HL01 = 0
            _HL02 = 0
            _HL03 = 0
            _HL04 = 0

            _LoopLevelMajor = 0
            _LoopLevelMinor = 0
            _LoopLevelSubFix = ""

            ClearGS()

        End Sub


        Private Sub ClearGS()

            GS.Clear()
            GE.Clear()
            _HIPAA_GS_GUID = Guid.Empty
            _GS_ID = 0
            _GS = String.Empty


            _GS01 = String.Empty
            _GS02 = String.Empty
            _GS03 = String.Empty
            _GS04 = String.Empty
            _GS05 = String.Empty
            _GS06 = String.Empty
            _GS07 = String.Empty
            _GS08 = String.Empty

            _HIPAA_HL_20_GUID = Guid.Empty
            _HIPAA_HL_21_GUID = Guid.Empty
            _HIPAA_HL_22_GUID = Guid.Empty
            _HIPAA_HL_23_GUID = Guid.Empty
            _HIPAA_HL_24_GUID = Guid.Empty

            _837_SBR_GUID = Guid.Empty
            _837_CLM_GUID = Guid.Empty
            _837_LX_GUID = Guid.Empty
            _NM1_GUID = Guid.Empty

            _HL01 = 0
            _HL02 = 0
            _HL03 = 0
            _HL04 = 0

            _LoopLevelMajor = 0
            _LoopLevelMinor = 0
            _LoopLevelSubFix = ""
        End Sub





        Private Sub ClearST()


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   BEGIN COMMON
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
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


            _LoopLevelMajor = 1000
            _LoopLevelMinor = 0
            _LoopLevelSubFix = ""


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   END COMMON
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '  BEGIN 837I 
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            _is_ST_BHT_NM1_PER = False

            _ST_ID = 0

            _837_CLM_GUID = Guid.Empty
            _837_SBR_GUID = Guid.Empty

            _837_LX_GUID = Guid.Empty
            _837_SVD_GUID = Guid.Empty



            ''''''''''''''''''''''''''''''''''
            AMT.Clear() ' As New DataTable

            CACHE_AMT.Clear() ' As New DataTable

            ''''''''''''''''''''''''''''''''''

            ''''''''''''''''''''''''''''''''''
            CAS.Clear() ' As New DataTable
            CL1.Clear() ' As New DataTable
            CLM.Clear() ' As New DataTable
            CN1.Clear() ' As New DataTable
            CLP.Clear() ' As New DataTable
            CR1.Clear() ' As New DataTable
            CTP.Clear() ' As New DataTable
            CUR.Clear() ' As New DataTable

            CACHE_CAS.Clear() ' As New DataTable
            CACHE_CL1.Clear() ' As New DataTable
            CACHE_CLM.Clear() ' As New DataTable
            CACHE_CN1.Clear() ' As New DataTable
            CACHE_CLP.Clear() ' As New DataTable
            CACHE_CR1.Clear() ' As New DataTable
            CACHE_CTP.Clear() ' As New DataTable
            CACHE_CUR.Clear() ' As New DataTable


            ''''''''''''''''''''''''''''''''''

            ''''''''''''''''''''''''''''''''''
            DMG.Clear() ' As New DataTable
            DTP.Clear() ' As New DataTable

            CACHE_DMG.Clear() ' As New DataTable
            CACHE_DTP.Clear() ' As New DataTable


            CACHE_2000_DMG.Clear() ' As New DataTable

            ''''''''''''''''''''''''''''''''''

            ''''''''''''''''''''''''''''''''''
            HI.Clear() ' As New DataTable
            HL.Clear() ' As New DataTable

            CACHE_HI.Clear() ' As New DataTable
            CACHE_HL.Clear() ' As New DataTable

            CACHE_2000_HL.Clear() ' As New DataTable

            ''''''''''''''''''''''''''''''''''

            ''''''''''''''''''''''''''''''''''
            K3.Clear() ' As New DataTable

            CACHE_K3.Clear() ' As New DataTable


            ''''''''''''''''''''''''''''''''''

            ''''''''''''''''''''''''''''''''''
            LX.Clear() ' As New DataTable

            CACHE_LX.Clear() ' As New DataTable
            LIN.Clear()

            ''''''''''''''''''''''''''''''''''

            ''''''''''''''''''''''''''''''''''
            MOA.Clear() ' As New DataTable
            MIA.Clear() ' As New DataTable

            CACHE_MOA.Clear() ' As New DataTable
            CACHE_MIA.Clear() ' As New DataTable


            ''''''''''''''''''''''''''''''''''

            ''''''''''''''''''''''''''''''''''
            N1.Clear() ' As New DataTable
            N3.Clear() ' As New DataTable
            N4.Clear() ' As New DataTable
            NM1.Clear() ' As New DataTable
            NTE.Clear() ' As New DataTable

            CACHE_N1.Clear() ' As New DataTable
            CACHE_N3.Clear() ' As New DataTable
            CACHE_N4.Clear() ' As New DataTable
            CACHE_NM1.Clear() ' As New DataTable
            CACHE_NTE.Clear() ' As New DataTable

            CACHE_1000_NM1.Clear() ' As New DataTable

            CACHE_2000_N1.Clear() ' As New DataTable
            CACHE_2000_N3.Clear() ' As New DataTable
            CACHE_2000_N4.Clear() ' As New DataTable
            CACHE_2000_NM1.Clear() ' As New DataTable


            ''''''''''''''''''''''''''''''''''

            ''''''''''''''''''''''''''''''''''
            OI.Clear() ' As New DataTable

            CACHE_OI.Clear() ' As New DataTable

            ''''''''''''''''''''''''''''''''''

            ''''''''''''''''''''''''''''''''''
            PRV.Clear() ' As New DataTable
            PAT.Clear() ' As New DataTable
            PER.Clear() ' As New DataTable
            PSV.Clear() ' As New DataTable
            PS1.Clear() ' As New DataTable
            PWK.Clear() ' As New DataTable

            CACHE_PRV.Clear() ' As New DataTable
            CACHE_PAT.Clear() ' As New DataTable
            CACHE_PER.Clear() ' As New DataTable
            CACHE_PSV.Clear() ' As New DataTable
            CACHE_PS1.Clear() ' As New DataTable
            CACHE_PWK.Clear() ' As New DataTable


            CACHE_1000_PER.Clear() ' As New DataTable

            CACHE_2000_PAT.Clear() ' As New DataTable
            CACHE_2000_PRV.Clear() ' As New DataTable
            CACHE_2000_PER.Clear() ' As New DataTable

            ''''''''''''''''''''''''''''''''''

            ''''''''''''''''''''''''''''''''''
            REF.Clear() ' As New DataTable

            CACHE_2000_REF.Clear() ' As New DataTable

            ''''''''''''''''''''''''''''''''''

            ''''''''''''''''''''''''''''''''''
            SBR.Clear() ' As New DataTable
            SV1.Clear() ' As New DataTable
            SV2.Clear() ' As New DataTable
            SV5.Clear() ' As New DataTable
            SVD.Clear() ' As New DataTable


            CACHE_SBR.Clear() ' As New DataTable
            CACHE_SV1.Clear() ' As New DataTable
            CACHE_SV2.Clear() ' As New DataTable
            CACHE_SV5.Clear() ' As New DataTable
            CACHE_SVD.Clear() ' As New DataTable


            CACHE_2000_SBR.Clear() ' As New DataTable



            _837_SBR_GUID = Guid.Empty
            _837_CLM_GUID = Guid.Empty
            _837_LX_GUID = Guid.Empty
            _NM1_GUID = Guid.Empty

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '  END 837I 
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ClearLoopHL22()


        End Sub
        Private Sub ClearLoopHL20()


            _is_HL20_ON_DISC = False

            _HIPAA_HL_21_GUID = Guid.Empty
            _HIPAA_HL_22_GUID = Guid.Empty
            _HIPAA_HL_23_GUID = Guid.Empty
            _HIPAA_HL_24_GUID = Guid.Empty

            _837_CLM_GUID = Guid.Empty
            _837_LX_GUID = Guid.Empty
            _837_SBR_GUID = Guid.Empty

            CACHE_2000_HL.Clear()

            CACHE_2000_N3.Clear()
            CACHE_2000_N4.Clear()
            CACHE_2000_NM1.Clear()


            CACHE_2000_PER.Clear()
            CACHE_2000_PRV.Clear()


            CACHE_2000_REF.Clear()



            _LoopLevelMajor = 2000
            _LoopLevelSubFix = "A"

        End Sub

        Private Sub ClearLoopHL22()


            _RAW_22 = String.Empty

            _HL22_DIRTY = False

            _HIPAA_HL_22_GUID = Guid.Empty
            _HIPAA_HL_23_GUID = Guid.Empty
            _HIPAA_HL_24_GUID = Guid.Empty
            _837_SBR_GUID = Guid.Empty
            _837_CLM_GUID = Guid.Empty
            _837_LX_GUID = Guid.Empty
            _837_SVD_GUID = Guid.Empty
            _NM1_GUID = Guid.Empty

            SBR.Clear()

            HL.Clear()
            CLM.Clear()

            LX.Clear()

            AMT.Clear()

            CAS.Clear()
            CL1.Clear()
            CR1.Clear()

            DMG.Clear()
            DTP.Clear()

            HI.Clear()

            K3.Clear()
            LIN.Clear()


            MIA.Clear()
            MOA.Clear()

            N3.Clear()
            N4.Clear()
            NM1.Clear()
            NTE.Clear()

            OI.Clear()

            PAT.Clear()
            PER.Clear()
            PRV.Clear()
            PWK.Clear()

            REF.Clear()

            SV1.Clear()
            SV2.Clear()
            SV5.Clear()
            SVD.Clear()



            'CACHE_2000_NM1 = NM1.Copy
            'CACHE_2000_N3 = N3.Copy
            'CACHE_2000_N4 = N4.Copy

            'CACHE_2000_PER = PER.Copy
            'CACHE_2000_PRV = PRV.Copy
            'CACHE_2000_REF = REF.Copy

            NM1.Clear()
            N3.Clear()
            N4.Clear()

            PER.Clear()
            PRV.Clear()
            REF.Clear()


            _LoopLevelMajor = 2000
            _LoopLevelSubFix = "A"


            ClearLX()

        End Sub

        Private Sub ClearCLM()



        End Sub



        Private Sub ClearLX()


            LX.Clear() ' As New DataTable
            SV1.Clear() ' As New DataTable
            SV2.Clear()
            SV5.Clear() ' As New DataTable
            PWK.Clear()
            DTP.Clear()
            REF.Clear()
            NTE.Clear()

            SVD.Clear() ' As New DataTable
            CAS.Clear()
            DTP.Clear()
            AMT.Clear()


            _837_LX_GUID = Guid.Empty
            _837_SVD_GUID = Guid.Empty

            ClearSVD()

        End Sub
        Private Sub ClearSVD()
            SVD.Clear() ' As New DataTable
            CAS.Clear()
            DTP.Clear()
            AMT.Clear()

            _837_SVD_GUID = Guid.Empty




        End Sub

        Private Sub ClearLoop1000()

            HL.Clear()

            AMT.Clear()

            CAS.Clear()
            CLM.Clear()
            CR1.Clear()

            DMG.Clear()
            DTP.Clear()

            HI.Clear()

            LX.Clear()

            K3.Clear()

            MOA.Clear()

            N3.Clear()
            N4.Clear()
            NM1.Clear()
            NTE.Clear()

            OI.Clear()

            PAT.Clear()
            PER.Clear()
            PRV.Clear()
            PWK.Clear()

            REF.Clear()


            SBR.Clear()


            SV1.Clear()
            SV5.Clear()
            SVD.Clear()

        End Sub



        Private Sub CACHE_HEADERS()
            'CACHE_1000_NM1 = NM1.Copy
            'CACHE_1000_PER = PER.Copy


            'NM1.Clear()
            'PER.Clear()
            '_NM1_GUID = Guid.Empty

        End Sub











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
                    If _LoopLevelMajor = 2300 Then
                        _LoopLevelMinor = 30
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
