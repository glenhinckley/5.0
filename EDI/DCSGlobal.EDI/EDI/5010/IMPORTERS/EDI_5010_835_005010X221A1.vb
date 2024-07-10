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
    Public Class EDI_5010_835_005010X221A1

        Inherits EDI_5010_835_005010X221A1_TABLES


        Implements IDisposable



        Protected disposed As Boolean = False

        '******************************************************************************************************************
        '  all vars not declared here are in   EDI_5010_COMMON_DECS   it is inhreted from the tables for this type
        '  if you are going to use it on this class only it goes below if not put it in EDI_5010_COMMON_DECS
        '******************************************************************************************************************

        Private log As New logExecption
        Private ss As New StringStuff
        '   Private oD As New Declarations

        Public _ConnectionString As String = String.Empty
        Public _CommandTimeOut As Integer = 90


        Private _DocumentType As String = "005010X221A1"

        Private _SP_ISA As String = "[usp_EDI_5010_HIPAA_835_ISA]"
        Private _SP_IEA As String = "[usp_EDI_5010_HIPAA_835_IEA]"

        Private _SP_GS As String = "[usp_EDI_5010_HIPAA_835_GS]"
        Private _SP_GE As String = "[usp_EDI_5010_HIPAA_835_GE]"

        Private _SP_ST As String = "[usp_EDI_5010_HIPAA_835_ST]"
        Private _SP_SE As String = "[usp_EDI_5010_HIPAA_835_SE]"

        Private _SP_HEADERS As String = "[usp_EDI_5010_HIPAA_835_ST_HEADER_DUMP]"
        Private _SP_HEADERS_TEST As String = "[usp_EDI_5010_HIPAA_835_ST_HEADER_TEST]"


        Private _SP_COMIT_LX_DATA As String = "[usp_EDI_5010_HIPAA_835_LX]"


        Private _SP_COMIT_CLP_TEST As String = "[usp_EDI_5010_HIPAA_835_CLP_TEST]"
        Private _SP_COMIT_CLP_DATA As String = "[usp_EDI_5010_HIPAA_835_CLP_DUMP]"


        Private _SP_COMIT_PLB_DATA As String = "[usp_EDI_5010_HIPAA_835_PLB]"


        Private _SP_COMIT_UNKNOWN As String = "[usp_EDI_5010_HIPAA_UNKNOWN]"

        Private _SP_ROLLBACK As String = "[usp_EDI_835_ROLLBACK]"






        Private _CLP_DIRTY As Boolean = False
        Private _LX_DIRTY As Boolean = False
        Private _HEADER_DIRTY As Boolean = True
        Private _ST_DIRTY As Boolean = True

        Private _IX_DTM_FOUND As Boolean = False




        Private _835_TSH_GUID As Guid = Guid.Empty
        Private _835_PAYOR_GUID As Guid = Guid.Empty
        Private _835_PAYEE_GUID As Guid = Guid.Empty
        Private _T_835_PAYOR_GUID As Guid = Guid.Empty
        Private _T_835_PAYEE_GUID As Guid = Guid.Empty



        Private _835_LX_GUID As Guid = Guid.Empty
        Private _835_CLP_GUID As Guid = Guid.Empty
        Private _835_SVC_GUID As Guid = Guid.Empty
        Private _REF_FOUND As Boolean = False

        Private _835_HEADER_STRING As String = String.Empty
        Private _835_BPR_STRING As String = String.Empty
        Private _835_LX_STRING As String = String.Empty
        Private _835_CLP_STRING As String = String.Empty
        Private _835_SVC_STRING As String = String.Empty
        Private _835_PLB_STRING As String = String.Empty

        Private _835_LOOP_LEVEL As String = "HEADER"







        ' MULTI PART EMLEMNTS
        Private _SVC01 As String = String.Empty
        Private _SVC06 As String = String.Empty


        Private _PLB03 As String = String.Empty
        Private _PLB05 As String = String.Empty
        Private _PLB07 As String = String.Empty
        Private _PLB09 As String = String.Empty
        Private _PLB11 As String = String.Empty
        Private _PLB13 As String = String.Empty











        Private _NM01 As String = String.Empty








        Protected Overrides Sub Finalize()

            Dispose()
            '' Console.WriteLine("Object " & GetHashCode() & " finalized.")
        End Sub

        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposed Then
                If disposing Then

                    log = Nothing
                    '    oD = Nothing


                    'email = Nothing
                    ' Insert code to free managed resources. 
                End If

                ss = Nothing
                ' Insert code to free unmanaged resources. 
            End If
            Me.disposed = True
        End Sub

        Public Sub New()
            If Debugger.IsAttached Then
                _VERBOSE = 1
                _DEBUG_LEVEL = 1

            End If

            _CONSOLE_NAME = "EDI_5010_835_005010X221A1"
            _CLASS_NAME = "EDI_5010_835_005010X221A1"
            _TRANSACTION_GUID = Guid.NewGuid

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



        Public WriteOnly Property SP_DO_TEST As Integer

            Set(value As Integer)
                _SP_DO_TEST = value
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

        Public WriteOnly Property SP_HEADERS As String

            Set(value As String)
                _SP_HEADERS = value
            End Set
        End Property


        Public WriteOnly Property SP_HEADERS_TEST As String

            Set(value As String)
                _SP_HEADERS_TEST = value
            End Set
        End Property

        Public WriteOnly Property SP_COMIT_LX_DATA As String

            Set(value As String)
                _SP_COMIT_LX_DATA = value
            End Set
        End Property

        Public WriteOnly Property SP_COMIT_CLP_TEST As String

            Set(value As String)
                _SP_COMIT_CLP_TEST = value
            End Set
        End Property


        Public WriteOnly Property SP_COMIT_CLP_DATA As String

            Set(value As String)
                _SP_COMIT_CLP_DATA = value
            End Set
        End Property

        Public WriteOnly Property SP_COMIT_PLB_DATA As String

            Set(value As String)
                _SP_COMIT_PLB_DATA = value
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
            ' Dim _ROW_COUNT As Integer = 0


            'If _EDI_SEQUENCE_NUMBER = 0 Then

            '    Using e As New EDI_5010_LOGGING
            '        e.ConnectionString = _ConnectionString
            '        e.TransactionSetIdentifierCode = "835"
            '        e.EDI_SEQUENCE("835")
            '        _EDI_SEQUENCE_NUMBER = e.EDI_SEQUENCE_NUMBER
            '    End Using

            'End If

            _ProcessStartTime = Now
            Dim _ImportReturnCode As Integer = 0


            If _TablesBuilt = False Then
                BuildTables()
                _TablesBuilt = True
                ClearISA()
            End If


            If _SP_DO_TEST = 1 Then
                _SP_RETURN_CODE = TestSPs()

                If (_SP_RETURN_CODE <> 0) Then
                    _IMPORT_RETURN_STRING = "835 : SP TEST FAILED WITH ERROR CODE " + Convert.ToString(_SP_RETURN_CODE)
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _IMPORT_RETURN_STRING)
                    Return -1
                    Exit Function
                End If

            End If

            If (_IS_FILE) Then
                Using e As New EDI_5010_LOGGING
                    e.ConnectionString = _ConnectionString
                    e.TransactionSetIdentifierCode = "835"
                    e.UpdateFileStatus(CInt(_FILE_ID), "835", "BEGIN PARSE")
                End Using
            End If



            ' _DOCUMENT_ID = _EDI_SEQUENCE_NUMBER


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

                _RAW_EDI = _RAW_EDI + line

                line = line.Replace("~", "")
                _ROW_COUNT = _ROW_COUNT + 1





                _RowRecordType = ss.ParseDemlimtedStringEDI(line, _DataElementSeparator, 1)
                _CurrentRowData = line

                '  _835_CLP_STRING = _835_CLP_STRING + _CurrentRowData

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



                        '_ISA_ROW_ID = _ROW_COUNT
                        ISARow("ROW_NUMBER") = _ROW_COUNT

                        ISA.Rows.Add(ISARow)

                        _RepetitionSeparator = _ISA11
                        _ComponentElementSeparator = _ISA16

                        _ISA = Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"

                        ' _chars = "RowDataDelimiter: " + _DataElementSeparator + " CarrotDataDelimiter: " + _CarrotDataDelimiter + " ComponentElementSeperator: " + _ISA16_ComponentElementSeparator


                        Select Case _835_LOOP_LEVEL
                            Case "HEADER"
                                _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                        End Select


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

                        'o() 'D.GS_ROW_ID = _ROW_COUNT

                        GSRow("ROW_NUMBER") = _ROW_COUNT






                        _GS = Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"

                        _GS08_ImplementationConventionReference = _GS08

                        Select Case _835_LOOP_LEVEL
                            Case "HEADER"
                                _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                        End Select

                        GS.Rows.Add(GSRow)
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
                        _835_TSH_GUID = Guid.NewGuid



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
                        STRow("835_TSH_GUID") = _835_TSH_GUID
                        STRow("ST01") = _ST01
                        STRow("ST02") = _ST02
                        STRow("ST03") = _ST03

                        _ST03_ImplementationConventionReference = _ST03

                        STRow("ROW_NUMBER") = _ROW_COUNT

                        _ST = Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"


                        Select Case _835_LOOP_LEVEL
                            Case "HEADER"
                                _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                        End Select



                        ST.Rows.Add(STRow)

                        _RowProcessedFlag = 1
                    End If

                    ' all the rows get made in to a string. 

                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "ST", ex)
                End Try



                '_835_PLB_STRING = GET_PLB(EDIList)

                'If _835_PLB_STRING <> "ERR" Then
                '    ' WHAT TO DO HERE
                'End If

                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   BPR
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "BPR" Then



                        Dim BPRRow As DataRow = BPR.NewRow
                        BPRRow("DOCUMENT_ID") = _DOCUMENT_ID
                        BPRRow("FILE_ID") = _FILE_ID
                        BPRRow("BATCH_ID") = _BATCH_ID
                        BPRRow("ISA_ID") = _ISA_ID
                        BPRRow("GS_ID") = _GS_ID
                        BPRRow("ST_ID") = _ST_ID
                        BPRRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                        BPRRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                        BPRRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                        BPRRow("835_TSH_GUID") = _835_TSH_GUID


                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then BPRRow("BPR01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else BPRRow("BPR01") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then BPRRow("BPR02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else BPRRow("BPR02") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then BPRRow("BPR03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else BPRRow("BPR03") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then BPRRow("BPR04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else BPRRow("BPR04") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then BPRRow("BPR05") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else BPRRow("BPR05") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then BPRRow("BPR06") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) Else BPRRow("BPR06") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then BPRRow("BPR07") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) Else BPRRow("BPR07") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) <> "") Then BPRRow("BPR08") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) Else BPRRow("BPR08") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) <> "") Then BPRRow("BPR09") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) Else BPRRow("BPR09") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11) <> "") Then BPRRow("BPR10") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11) Else BPRRow("BPR10") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12) <> "") Then BPRRow("BPR11") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12) Else BPRRow("BPR11") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 13) <> "") Then BPRRow("BPR12") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 13) Else BPRRow("BPR12") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 14) <> "") Then BPRRow("BPR13") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 14) Else BPRRow("BPR13") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 15) <> "") Then BPRRow("BPR14") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 15) Else BPRRow("BPR14") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 16) <> "") Then BPRRow("BPR15") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 16) Else BPRRow("BPR15") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 17) <> "") Then BPRRow("BPR16") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 17) Else BPRRow("BPR16") = DBNull.Value



                        BPRRow("ROW_NUMBER") = _ROW_COUNT



                        BPRRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        BPRRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                        BPRRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor







                        Select Case _835_LOOP_LEVEL
                            Case "HEADER"
                                _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                        End Select

                        BPR.Rows.Add(BPRRow)

                        _RowProcessedFlag = 1
                    End If
                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "BPR", ex)
                End Try




                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   BEGIN 835 P 005010X221A1
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                '**********************************************************************************************************************************************************

                'THIS SHOULD START THE PR LEVEL BUT NOT SURE IT COULD BE THE N1 BELOW AS
                If _RowRecordType = "DTM" Then

                    If _IX_DTM_FOUND = False Then

                        _LoopLevelSubFix = "A"
                        _835_PAYOR_GUID = Guid.NewGuid
                        _835_PAYEE_GUID = Guid.Empty
                        _IX_DTM_FOUND = True
                    End If
                End If


                'THIS SETS THE PR PE LEVELS AND MAKES THE GUIDS
                If _RowRecordType = "N1" Then

                    Dim N101X As String = String.Empty
                    N101X = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2)


                    If N101X = "PR" Then
                        _LoopLevelSubFix = "A"
                        _835_PAYOR_GUID = Guid.NewGuid
                        _835_PAYEE_GUID = Guid.Empty
                    End If

                    If N101X = "PE" Then
                        _LoopLevelSubFix = "B"
                        _835_PAYOR_GUID = Guid.Empty
                        _835_PAYEE_GUID = Guid.NewGuid
                    End If

                End If

                '**********************************************************************************************************************************************************
                'THIS GETS US TO THE lx LEVEL BLA BLA BLA
                If _RowRecordType = "LX" Then

                    If _HEADER_DIRTY Then
                        ComitHeader()
                    End If

                    _835_LOOP_LEVEL = "LX"

                    _LoopLevelMajor = 2000
                    _LoopLevelMinor = 0
                    _LoopLevelSubFix = ""

                    _835_PAYOR_GUID = Guid.Empty
                    _835_PAYEE_GUID = Guid.Empty
                    _835_LX_GUID = Guid.NewGuid
                    _835_CLP_GUID = Guid.Empty
                    _835_SVC_GUID = Guid.Empty

                End If


                '**********************************************************************************************************************************************************
                'CLP ...
                If _RowRecordType = "CLP" Then

                    If _LX_DIRTY Then
                        ComitLX()
                        ClearLX()

                    End If


                    If _CLP_DIRTY Then
                        ComitCLP()
                        ClearCLP()
                        _CLP_DIRTY = False
                    End If

                    _835_PAYOR_GUID = Guid.Empty
                    _835_PAYEE_GUID = Guid.Empty

                    _835_CLP_GUID = Guid.NewGuid
                    _835_SVC_GUID = Guid.Empty

                    _LoopLevelMajor = 2000
                    _LoopLevelMinor = 100
                    _LoopLevelSubFix = ""

                    _835_LOOP_LEVEL = "CLP"
                End If


                '**********************************************************************************************************************************************************
                'SVC
                If _RowRecordType = "SVC" Then

                    _835_PAYOR_GUID = Guid.Empty
                    _835_PAYEE_GUID = Guid.Empty

                    _835_SVC_GUID = Guid.NewGuid

                    _LoopLevelMajor = 2000
                    _LoopLevelMinor = 200
                    _LoopLevelSubFix = ""

                    _835_LOOP_LEVEL = "SVC"
                End If



                '==========================================================================================================================================================
                ' BEGIN HEADER BLOCK
                If _835_LOOP_LEVEL = "HEADER" Then


                    _T_835_PAYOR_GUID = _835_PAYEE_GUID
                    _T_835_PAYEE_GUID = _835_PAYEE_GUID

                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '  HEADER :: TRN
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
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
                            TRNRow("835_TSH_GUID") = _835_TSH_GUID
                            TRNRow("835_PAYOR_GUID") = _835_PAYOR_GUID
                            TRNRow("835_PAYEE_GUID") = _835_PAYEE_GUID
                            TRNRow("835_LX_GUID") = Guid.Empty
                            TRNRow("835_CLP_GUID") = Guid.Empty
                            TRNRow("835_SVC_GUID") = Guid.Empty

                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then TRNRow("TRN01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else TRNRow("TRN01") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then TRNRow("TRN02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else TRNRow("TRN02") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then TRNRow("TRN03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else TRNRow("TRN03") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then TRNRow("TRN04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else TRNRow("TRN04") = DBNull.Value



                            TRNRow("ROW_NUMBER") = _ROW_COUNT

                            TRNRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            TRNRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            TRNRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                            Select Case _835_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "LX"
                                    _835_LX_STRING = _835_LX_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "CLP"
                                    _835_CLP_STRING = _835_CLP_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "SVC"
                                    _835_SVC_STRING = _835_SVC_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select


                            TRN.Rows.Add(TRNRow)

                            _RowProcessedFlag = 1
                        End If
                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "HEADER :: TRN", ex)
                    End Try


                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '  HEADER :: REF
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "REF" Then



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
                            REFRow("835_TSH_GUID") = _835_TSH_GUID
                            REFRow("835_PAYOR_GUID") = _835_PAYOR_GUID
                            REFRow("835_PAYEE_GUID") = _835_PAYEE_GUID
                            REFRow("835_LX_GUID") = Guid.Empty
                            REFRow("835_CLP_GUID") = Guid.Empty
                            REFRow("835_SVC_GUID") = Guid.Empty

                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then REFRow("REF01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else REFRow("REF01") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then REFRow("REF02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else REFRow("REF02") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then REFRow("REF03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else REFRow("REF03") = DBNull.Value



                            REFRow("ROW_NUMBER") = _ROW_COUNT

                            REFRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            REFRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            REFRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                            Select Case _835_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "LX"
                                    _835_LX_STRING = _835_LX_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "CLP"
                                    _835_CLP_STRING = _835_CLP_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "SVC"
                                    _835_SVC_STRING = _835_SVC_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select

                            REF.Rows.Add(REFRow)

                            _RowProcessedFlag = 1
                        End If
                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "HEADER :: REF", ex)
                    End Try



                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '  HEADER :: DTM
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "DTM" Then



                            Dim DTMRow As DataRow = DTM.NewRow
                            DTMRow("DOCUMENT_ID") = _DOCUMENT_ID
                            DTMRow("FILE_ID") = _FILE_ID
                            DTMRow("BATCH_ID") = _BATCH_ID
                            DTMRow("ISA_ID") = _ISA_ID
                            DTMRow("GS_ID") = _GS_ID
                            DTMRow("ST_ID") = _ST_ID
                            DTMRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            DTMRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            DTMRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID

                            DTMRow("835_TSH_GUID") = _835_TSH_GUID
                            DTMRow("835_PAYOR_GUID") = _835_PAYOR_GUID
                            DTMRow("835_PAYEE_GUID") = _835_PAYEE_GUID
                            DTMRow("835_LX_GUID") = Guid.Empty
                            DTMRow("835_CLP_GUID") = Guid.Empty
                            DTMRow("835_SVC_GUID") = Guid.Empty


                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then DTMRow("DTM01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else DTMRow("DTM01") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then DTMRow("DTM02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else DTMRow("DTM02") = DBNull.Value


                            DTMRow("ROW_NUMBER") = _ROW_COUNT
                            DTMRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            DTMRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            DTMRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix






                            Select Case _835_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "LX"
                                    _835_LX_STRING = _835_LX_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "CLP"
                                    _835_CLP_STRING = _835_CLP_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "SVC"
                                    _835_SVC_STRING = _835_SVC_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select


                            DTM.Rows.Add(DTMRow)

                            _RowProcessedFlag = 1
                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "HEADER :: DTM", ex)
                    End Try



                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   HEADER :: PER
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "PER" Then



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

                            PERRow("835_TSH_GUID") = _835_TSH_GUID
                            PERRow("835_PAYOR_GUID") = _835_PAYOR_GUID
                            PERRow("835_PAYEE_GUID") = _835_PAYEE_GUID
                            PERRow("835_LX_GUID") = Guid.Empty
                            PERRow("835_CLP_GUID") = Guid.Empty
                            PERRow("835_SVC_GUID") = Guid.Empty



                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then PERRow("PER01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else PERRow("PER01") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then PERRow("PER02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else PERRow("PER02") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then PERRow("PER03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else PERRow("PER03") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then PERRow("PER04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else PERRow("PER04") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then PERRow("PER05") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else PERRow("PER05") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then PERRow("PER06") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) Else PERRow("PER06") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then PERRow("PER07") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) Else PERRow("PER07") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) <> "") Then PERRow("PER08") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) Else PERRow("PER08") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) <> "") Then PERRow("PER09") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) Else PERRow("PER09") = DBNull.Value

                            PERRow("ROW_NUMBER") = _ROW_COUNT


                            PERRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            PERRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            PERRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix



                            Select Case _835_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "LX"
                                    _835_LX_STRING = _835_LX_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "CLP"
                                    _835_CLP_STRING = _835_CLP_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "SVC"
                                    _835_SVC_STRING = _835_SVC_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select

                            PER.Rows.Add(PERRow)

                            _RowProcessedFlag = 1
                        End If
                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "HEADER :: PER", ex)
                    End Try


                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '  HEADER :: N1
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "N1" Then


                            Dim N101 As String = String.Empty


                            Dim N1Row As DataRow = N1.NewRow
                            N1Row("DOCUMENT_ID") = _DOCUMENT_ID
                            N1Row("FILE_ID") = _FILE_ID
                            N1Row("BATCH_ID") = _BATCH_ID
                            N1Row("ISA_ID") = _ISA_ID
                            N1Row("GS_ID") = _GS_ID
                            N1Row("ST_ID") = _ST_ID
                            N1Row("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            N1Row("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            N1Row("HIPAA_ST_GUID") = _HIPAA_ST_GUID

                            N1Row("835_TSH_GUID") = _835_TSH_GUID
                            N1Row("835_PAYOR_GUID") = _835_PAYOR_GUID
                            N1Row("835_PAYEE_GUID") = _835_PAYEE_GUID
                            N1Row("835_LX_GUID") = Guid.Empty
                            N1Row("835_CLP_GUID") = Guid.Empty
                            N1Row("835_SVC_GUID") = Guid.Empty


                            'If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then
                            '    N1Row("N101") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2)
                            '    N101 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2)
                            'Else
                            '    N1Row("N101") = DBNull.Value
                            'End If

                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then N1Row("N101") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else N1Row("N101") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then N1Row("N102") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else N1Row("N102") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then N1Row("N103") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else N1Row("N103") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then N1Row("N104") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else N1Row("N104") = DBNull.Value

                            'If N101 = "PR" Then
                            '    _LoopLevelSubFix = "A"
                            '    _835_PAYEE_GUID = Guid.NewGuid
                            'End If


                            'If N101 = "PE" Then
                            '    _LoopLevelSubFix = "B"
                            '    _835_PAYEE_GUID = Guid.NewGuid
                            'End If




                            N1Row("ROW_NUMBER") = _ROW_COUNT


                            N1Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            N1Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            N1Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix



                            Select Case _835_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "LX"
                                    _835_LX_STRING = _835_LX_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "CLP"
                                    _835_CLP_STRING = _835_CLP_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "SVC"
                                    _835_SVC_STRING = _835_SVC_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select

                            N1.Rows.Add(N1Row)

                            _RowProcessedFlag = 1
                        End If
                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "HEADER :: N1", ex)
                    End Try



                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '  HEADER ::  N3
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

                            N3Row("835_TSH_GUID") = _835_TSH_GUID
                            N3Row("835_PAYOR_GUID") = _835_PAYOR_GUID
                            N3Row("835_PAYEE_GUID") = _835_PAYEE_GUID
                            N3Row("835_LX_GUID") = Guid.Empty
                            N3Row("835_CLP_GUID") = Guid.Empty
                            N3Row("835_SVC_GUID") = Guid.Empty


                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then N3Row("N301") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else N3Row("N301") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then N3Row("N302") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else N3Row("N302") = DBNull.Value




                            N3Row("ROW_NUMBER") = _ROW_COUNT




                            N3Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            N3Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            N3Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix



                            Select Case _835_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "LX"
                                    _835_LX_STRING = _835_LX_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "CLP"
                                    _835_CLP_STRING = _835_CLP_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "SVC"
                                    _835_SVC_STRING = _835_SVC_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select

                            N3.Rows.Add(N3Row)

                            _RowProcessedFlag = 1
                        End If
                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "HEADER ::  N3", ex)
                    End Try





                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   HEADER :: N4
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

                            N4Row("835_TSH_GUID") = _835_TSH_GUID
                            N4Row("835_PAYOR_GUID") = _835_PAYOR_GUID
                            N4Row("835_PAYEE_GUID") = _835_PAYEE_GUID
                            N4Row("835_LX_GUID") = Guid.Empty
                            N4Row("835_CLP_GUID") = Guid.Empty
                            N4Row("835_SVC_GUID") = Guid.Empty


                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then N4Row("N401") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else N4Row("N401") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then N4Row("N402") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else N4Row("N402") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then N4Row("N403") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else N4Row("N403") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then N4Row("N404") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else N4Row("N404") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then N4Row("N405") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else N4Row("N405") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then N4Row("N406") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) Else N4Row("N406") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then N4Row("N407") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) Else N4Row("N407") = DBNull.Value



                            N4Row("ROW_NUMBER") = _ROW_COUNT



                            N4Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            N4Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            N4Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix





                            Select Case _835_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "LX"
                                    _835_LX_STRING = _835_LX_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "CLP"
                                    _835_CLP_STRING = _835_CLP_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "SVC"
                                    _835_SVC_STRING = _835_SVC_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select

                            N4.Rows.Add(N4Row)

                            _RowProcessedFlag = 1
                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "HEADER ::  N4", ex)
                    End Try


                End If
                'END HEADER BLOCK
                '==========================================================================================================================================================










                '==========================================================================================================================================================
                'BEGIN LX BLOCK
                If _835_LOOP_LEVEL = "LX" Then



                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   LX  :: LX
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "LX" Then


                            _LX_DIRTY = True


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
                            LXRow("835_TSH_GUID") = _835_TSH_GUID
                            LXRow("835_PAYOR_GUID") = Guid.Empty
                            LXRow("835_PAYEE_GUID") = Guid.Empty
                            LXRow("835_LX_GUID") = _835_LX_GUID
                            LXRow("835_CLP_GUID") = Guid.Empty
                            LXRow("835_SVC_GUID") = Guid.Empty


                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then LXRow("LX01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else LXRow("LX01") = DBNull.Value


                            LXRow("ROW_NUMBER") = _ROW_COUNT
                            LXRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            LXRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            LXRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix



                            Select Case _835_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "LX"
                                    _835_LX_STRING = _835_LX_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "CLP"
                                    _835_CLP_STRING = _835_CLP_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "SVC"
                                    _835_SVC_STRING = _835_SVC_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select

                            LX.Rows.Add(LXRow)

                            _RowProcessedFlag = 1
                        End If


                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "LX  :: LX", ex)
                    End Try





                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   LX  :: TS2
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "TS2" Then


                            Dim TS2Row As DataRow = TS2.NewRow
                            TS2Row("DOCUMENT_ID") = _DOCUMENT_ID
                            TS2Row("FILE_ID") = _FILE_ID
                            TS2Row("BATCH_ID") = _BATCH_ID
                            TS2Row("ISA_ID") = _ISA_ID
                            TS2Row("GS_ID") = _GS_ID
                            TS2Row("ST_ID") = _ST_ID
                            TS2Row("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            TS2Row("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            TS2Row("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            TS2Row("835_TSH_GUID") = _835_TSH_GUID
                            TS2Row("835_PAYOR_GUID") = Guid.Empty
                            TS2Row("835_PAYEE_GUID") = Guid.Empty
                            TS2Row("835_LX_GUID") = _835_LX_GUID
                            TS2Row("835_CLP_GUID") = Guid.Empty
                            TS2Row("835_SVC_GUID") = Guid.Empty

                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then TS2Row("TS201") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else TS2Row("TS201") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then TS2Row("TS202") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else TS2Row("TS202") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then TS2Row("TS203") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else TS2Row("TS203") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then TS2Row("TS204") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else TS2Row("TS204") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then TS2Row("TS205") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else TS2Row("TS205") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then TS2Row("TS206") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) Else TS2Row("TS206") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then TS2Row("TS207") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) Else TS2Row("TS207") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) <> "") Then TS2Row("TS208") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) Else TS2Row("TS208") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) <> "") Then TS2Row("TS209") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) Else TS2Row("TS209") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11) <> "") Then TS2Row("TS210") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11) Else TS2Row("TS210") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12) <> "") Then TS2Row("TS211") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12) Else TS2Row("TS211") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 13) <> "") Then TS2Row("TS212") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 13) Else TS2Row("TS212") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 14) <> "") Then TS2Row("TS213") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 14) Else TS2Row("TS213") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 15) <> "") Then TS2Row("TS214") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 15) Else TS2Row("TS214") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 16) <> "") Then TS2Row("TS215") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 16) Else TS2Row("TS215") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 17) <> "") Then TS2Row("TS216") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 17) Else TS2Row("TS216") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 18) <> "") Then TS2Row("TS217") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 18) Else TS2Row("TS217") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 19) <> "") Then TS2Row("TS218") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 19) Else TS2Row("TS218") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 20) <> "") Then TS2Row("TS219") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 20) Else TS2Row("TS219") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 21) <> "") Then TS2Row("TS220") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 21) Else TS2Row("TS220") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 22) <> "") Then TS2Row("TS221") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 22) Else TS2Row("TS221") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 23) <> "") Then TS2Row("TS222") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 23) Else TS2Row("TS222") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 24) <> "") Then TS2Row("TS223") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 24) Else TS2Row("TS223") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 25) <> "") Then TS2Row("TS223") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 25) Else TS2Row("TS223") = DBNull.Value





                            TS2Row("ROW_NUMBER") = _ROW_COUNT



                            TS2Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            TS2Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                            TS2Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor





                            Select Case _835_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "LX"
                                    _835_LX_STRING = _835_LX_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "CLP"
                                    _835_CLP_STRING = _835_CLP_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "SVC"
                                    _835_SVC_STRING = _835_SVC_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select

                            TS2.Rows.Add(TS2Row)

                            _RowProcessedFlag = 1
                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "LX  :: TS2", ex)
                    End Try



                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '  LX :: TS3
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "TS3" Then


                            Dim TS3Row As DataRow = TS3.NewRow
                            TS3Row("DOCUMENT_ID") = _DOCUMENT_ID
                            TS3Row("FILE_ID") = _FILE_ID
                            TS3Row("BATCH_ID") = _BATCH_ID
                            TS3Row("ISA_ID") = _ISA_ID
                            TS3Row("GS_ID") = _GS_ID
                            TS3Row("ST_ID") = _ST_ID
                            TS3Row("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            TS3Row("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            TS3Row("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            TS3Row("835_TSH_GUID") = _835_TSH_GUID
                            TS3Row("835_PAYOR_GUID") = Guid.Empty
                            TS3Row("835_PAYEE_GUID") = Guid.Empty
                            TS3Row("835_LX_GUID") = _835_LX_GUID
                            TS3Row("835_CLP_GUID") = Guid.Empty
                            TS3Row("835_SVC_GUID") = Guid.Empty

                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then TS3Row("TS301") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else TS3Row("TS301") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then TS3Row("TS302") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else TS3Row("TS302") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then TS3Row("TS303") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else TS3Row("TS303") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then TS3Row("TS304") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else TS3Row("TS304") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then TS3Row("TS305") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else TS3Row("TS305") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then TS3Row("TS306") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) Else TS3Row("TS306") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then TS3Row("TS307") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) Else TS3Row("TS307") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) <> "") Then TS3Row("TS308") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) Else TS3Row("TS308") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) <> "") Then TS3Row("TS309") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) Else TS3Row("TS309") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11) <> "") Then TS3Row("TS310") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11) Else TS3Row("TS310") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12) <> "") Then TS3Row("TS311") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12) Else TS3Row("TS311") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 13) <> "") Then TS3Row("TS312") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 13) Else TS3Row("TS312") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 14) <> "") Then TS3Row("TS313") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 14) Else TS3Row("TS313") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 15) <> "") Then TS3Row("TS314") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 15) Else TS3Row("TS314") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 16) <> "") Then TS3Row("TS315") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 16) Else TS3Row("TS315") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 17) <> "") Then TS3Row("TS316") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 17) Else TS3Row("TS316") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 18) <> "") Then TS3Row("TS317") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 18) Else TS3Row("TS317") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 19) <> "") Then TS3Row("TS318") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 19) Else TS3Row("TS318") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 20) <> "") Then TS3Row("TS319") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 20) Else TS3Row("TS319") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 21) <> "") Then TS3Row("TS320") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 21) Else TS3Row("TS320") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 22) <> "") Then TS3Row("TS321") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 22) Else TS3Row("TS321") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 23) <> "") Then TS3Row("TS322") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 23) Else TS3Row("TS322") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 24) <> "") Then TS3Row("TS323") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 24) Else TS3Row("TS323") = DBNull.Value

                            TS3Row("ROW_NUMBER") = _ROW_COUNT



                            TS3Row("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            TS3Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                            TS3Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor





                            Select Case _835_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "LX"
                                    _835_LX_STRING = _835_LX_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "CLP"
                                    _835_CLP_STRING = _835_CLP_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "SVC"
                                    _835_SVC_STRING = _835_SVC_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select

                            TS3.Rows.Add(TS3Row)

                            _RowProcessedFlag = 1
                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "LX  :: TS3", ex)
                    End Try

                End If
                'END LX BLOCK
                '==========================================================================================================================================================





                '==========================================================================================================================================================
                ' CLP BLOCK
                If _835_LOOP_LEVEL = "CLP" Then




                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   LX - CLP :: CLP 
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "CLP" Then





                            Dim CLPRow As DataRow = CLP_CLP.NewRow
                            CLPRow("DOCUMENT_ID") = _DOCUMENT_ID
                            CLPRow("FILE_ID") = _FILE_ID
                            CLPRow("BATCH_ID") = _BATCH_ID
                            CLPRow("ISA_ID") = _ISA_ID
                            CLPRow("GS_ID") = _GS_ID
                            CLPRow("ST_ID") = _ST_ID
                            CLPRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            CLPRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            CLPRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            CLPRow("835_TSH_GUID") = _835_TSH_GUID
                            CLPRow("835_PAYOR_GUID") = Guid.Empty
                            CLPRow("835_PAYEE_GUID") = Guid.Empty
                            CLPRow("835_LX_GUID") = _835_LX_GUID
                            CLPRow("835_CLP_GUID") = _835_CLP_GUID
                            CLPRow("835_SVC_GUID") = Guid.Empty

                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then CLPRow("CLP01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else CLPRow("CLP01") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then CLPRow("CLP02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else CLPRow("CLP02") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then CLPRow("CLP03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else CLPRow("CLP03") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then CLPRow("CLP04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else CLPRow("CLP04") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then CLPRow("CLP05") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else CLPRow("CLP05") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then CLPRow("CLP06") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) Else CLPRow("CLP06") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then CLPRow("CLP07") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) Else CLPRow("CLP07") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) <> "") Then CLPRow("CLP08") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) Else CLPRow("CLP08") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) <> "") Then CLPRow("CLP09") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) Else CLPRow("CLP09") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11) <> "") Then CLPRow("CLP10") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11) Else CLPRow("CLP10") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12) <> "") Then CLPRow("CLP11") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12) Else CLPRow("CLP11") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 13) <> "") Then CLPRow("CLP12") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 13) Else CLPRow("CLP12") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 14) <> "") Then CLPRow("CLP13") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 14) Else CLPRow("CLP13") = DBNull.Value

                            CLPRow("ROW_NUMBER") = _ROW_COUNT



                            CLPRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            CLPRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                            CLPRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor






                            Select Case _835_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "LX"
                                    _835_LX_STRING = _835_LX_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "CLP"
                                    _835_CLP_STRING = _835_CLP_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "SVC"
                                    _835_SVC_STRING = _835_SVC_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select

                            CLP_CLP.Rows.Add(CLPRow)
                            _CLP_DIRTY = True

                            _RowProcessedFlag = 1
                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "LX - CLP :: CLP ", ex)
                    End Try



                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '  LX - CLP :: CAS
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "CAS" Then


                            Dim CASRow As DataRow = CLP_CAS.NewRow
                            CASRow("DOCUMENT_ID") = _DOCUMENT_ID
                            CASRow("FILE_ID") = _FILE_ID
                            CASRow("BATCH_ID") = _BATCH_ID
                            CASRow("ISA_ID") = _ISA_ID
                            CASRow("GS_ID") = _GS_ID
                            CASRow("ST_ID") = _ST_ID
                            CASRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            CASRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            CASRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            CASRow("835_TSH_GUID") = _835_TSH_GUID
                            CASRow("835_PAYOR_GUID") = Guid.Empty
                            CASRow("835_PAYEE_GUID") = Guid.Empty
                            CASRow("835_LX_GUID") = _835_LX_GUID
                            CASRow("835_CLP_GUID") = _835_CLP_GUID
                            CASRow("835_SVC_GUID") = Guid.Empty



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



                            CASRow("ROW_NUMBER") = _ROW_COUNT


                            CASRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            CASRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                            CASRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor




                            Select Case _835_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "LX"
                                    _835_LX_STRING = _835_LX_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "CLP"
                                    _835_CLP_STRING = _835_CLP_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "SVC"
                                    _835_SVC_STRING = _835_SVC_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select

                            CLP_CAS.Rows.Add(CASRow)

                            _RowProcessedFlag = 1
                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "LX - CLP :: CAS", ex)
                    End Try



                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '  LX - CLP ::  NM1
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "NM1" Then



                            _NM1_GUID = Guid.NewGuid

                            Dim NM1Row As DataRow = CLP_NM1.NewRow
                            NM1Row("DOCUMENT_ID") = _DOCUMENT_ID
                            NM1Row("FILE_ID") = _FILE_ID
                            NM1Row("BATCH_ID") = _BATCH_ID
                            NM1Row("ISA_ID") = _ISA_ID
                            NM1Row("GS_ID") = _GS_ID
                            NM1Row("ST_ID") = _ST_ID
                            NM1Row("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            NM1Row("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            NM1Row("HIPAA_ST_GUID") = _HIPAA_ST_GUID

                            NM1Row("835_TSH_GUID") = _835_TSH_GUID
                            NM1Row("835_PAYOR_GUID") = Guid.Empty
                            NM1Row("835_PAYEE_GUID") = Guid.Empty
                            NM1Row("835_LX_GUID") = _835_LX_GUID
                            NM1Row("835_CLP_GUID") = _835_CLP_GUID
                            NM1Row("835_SVC_GUID") = Guid.Empty


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
                            NM1Row("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                            NM1Row("LOOP_LEVEL_MINOR") = _LoopLevelMinor





                            Select Case _835_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "LX"
                                    _835_LX_STRING = _835_LX_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "CLP"
                                    _835_CLP_STRING = _835_CLP_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "SVC"
                                    _835_SVC_STRING = _835_SVC_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select

                            CLP_NM1.Rows.Add(NM1Row)

                            _RowProcessedFlag = 1
                        End If
                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "LX - CLP :: NM1", ex)
                    End Try

                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '    LX - CLP :: MIA
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "MIA" Then

                            Dim MIARow As DataRow = CLP_MIA.NewRow
                            MIARow("DOCUMENT_ID") = _DOCUMENT_ID
                            MIARow("FILE_ID") = _FILE_ID
                            MIARow("BATCH_ID") = _BATCH_ID
                            MIARow("ISA_ID") = _ISA_ID
                            MIARow("GS_ID") = _GS_ID
                            MIARow("ST_ID") = _ST_ID
                            MIARow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            MIARow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            MIARow("HIPAA_ST_GUID") = _HIPAA_ST_GUID

                            MIARow("835_TSH_GUID") = _835_TSH_GUID
                            MIARow("835_PAYOR_GUID") = Guid.Empty
                            MIARow("835_PAYEE_GUID") = Guid.Empty
                            MIARow("835_LX_GUID") = _835_LX_GUID
                            MIARow("835_CLP_GUID") = _835_CLP_GUID
                            MIARow("835_SVC_GUID") = Guid.Empty



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


                            MIARow("ROW_NUMBER") = _ROW_COUNT

                            MIARow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            MIARow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            MIARow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix



                            Select Case _835_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "LX"
                                    _835_LX_STRING = _835_LX_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "CLP"
                                    _835_CLP_STRING = _835_CLP_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "SVC"
                                    _835_SVC_STRING = _835_SVC_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select

                            CLP_MIA.Rows.Add(MIARow)

                            _RowProcessedFlag = 1
                        End If
                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "LX - CLP :: MIA", ex)
                    End Try





                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '    LX - CLP :: MOA
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "MOA" Then

                            Dim MOARow As DataRow = CLP_MOA.NewRow
                            MOARow("DOCUMENT_ID") = _DOCUMENT_ID
                            MOARow("FILE_ID") = _FILE_ID
                            MOARow("BATCH_ID") = _BATCH_ID
                            MOARow("ISA_ID") = _ISA_ID
                            MOARow("GS_ID") = _GS_ID
                            MOARow("ST_ID") = _ST_ID
                            MOARow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            MOARow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            MOARow("HIPAA_ST_GUID") = _HIPAA_ST_GUID

                            MOARow("835_TSH_GUID") = _835_TSH_GUID
                            MOARow("835_PAYOR_GUID") = Guid.Empty
                            MOARow("835_PAYEE_GUID") = Guid.Empty
                            MOARow("835_LX_GUID") = _835_LX_GUID
                            MOARow("835_CLP_GUID") = _835_CLP_GUID
                            MOARow("835_SVC_GUID") = Guid.Empty


                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then MOARow("MOA01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else MOARow("MOA01") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then MOARow("MOA02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else MOARow("MOA02") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then MOARow("MOA03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else MOARow("MOA03") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then MOARow("MOA04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else MOARow("MOA04") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then MOARow("MOA05") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else MOARow("MOA05") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then MOARow("MOA06") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) Else MOARow("MOA06") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then MOARow("MOA07") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) Else MOARow("MOA07") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) <> "") Then MOARow("MOA08") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) Else MOARow("MOA08") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) <> "") Then MOARow("MOA09") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) Else MOARow("MOA09") = DBNull.Value


                            MOARow("ROW_NUMBER") = _ROW_COUNT

                            MOARow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            MOARow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            MOARow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix



                            Select Case _835_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "LX"
                                    _835_LX_STRING = _835_LX_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "CLP"
                                    _835_CLP_STRING = _835_CLP_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "SVC"
                                    _835_SVC_STRING = _835_SVC_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select

                            CLP_MOA.Rows.Add(MOARow)

                            _RowProcessedFlag = 1
                        End If
                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "LX - CLP :: MOA", ex)
                    End Try



                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   lx - clp :: PER
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "PER" Then



                            Dim PERRow As DataRow = CLP_PER.NewRow
                            PERRow("DOCUMENT_ID") = _DOCUMENT_ID
                            PERRow("FILE_ID") = _FILE_ID
                            PERRow("BATCH_ID") = _BATCH_ID
                            PERRow("ISA_ID") = _ISA_ID
                            PERRow("GS_ID") = _GS_ID
                            PERRow("ST_ID") = _ST_ID
                            PERRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            PERRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            PERRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID

                            PERRow("835_TSH_GUID") = _835_TSH_GUID
                            PERRow("835_PAYOR_GUID") = Guid.Empty
                            PERRow("835_PAYEE_GUID") = Guid.Empty
                            PERRow("835_LX_GUID") = _835_LX_GUID
                            PERRow("835_CLP_GUID") = _835_CLP_GUID
                            PERRow("835_SVC_GUID") = Guid.Empty

                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then PERRow("PER01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else PERRow("PER01") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then PERRow("PER02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else PERRow("PER02") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then PERRow("PER03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else PERRow("PER03") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then PERRow("PER04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else PERRow("PER04") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then PERRow("PER05") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else PERRow("PER05") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then PERRow("PER06") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) Else PERRow("PER06") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then PERRow("PER07") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) Else PERRow("PER07") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) <> "") Then PERRow("PER08") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) Else PERRow("PER08") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) <> "") Then PERRow("PER09") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) Else PERRow("PER09") = DBNull.Value

                            PERRow("ROW_NUMBER") = _ROW_COUNT


                            PERRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            PERRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            PERRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix



                            Select Case _835_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "LX"
                                    _835_LX_STRING = _835_LX_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "CLP"
                                    _835_CLP_STRING = _835_CLP_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "SVC"
                                    _835_SVC_STRING = _835_SVC_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select


                            ' Fixed by mohan he has learned well
                            CLP_PER.Rows.Add(PERRow)

                            _RowProcessedFlag = 1
                        End If
                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "LX - CLP :: PER", ex)
                    End Try


                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   LX - CLP ::  REF
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "REF" Then



                            Dim REFRow As DataRow = CLP_REF.NewRow
                            REFRow("DOCUMENT_ID") = _DOCUMENT_ID
                            REFRow("FILE_ID") = _FILE_ID
                            REFRow("BATCH_ID") = _BATCH_ID
                            REFRow("ISA_ID") = _ISA_ID
                            REFRow("GS_ID") = _GS_ID
                            REFRow("ST_ID") = _ST_ID
                            REFRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            REFRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            REFRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID

                            REFRow("835_TSH_GUID") = _835_TSH_GUID
                            REFRow("835_PAYOR_GUID") = Guid.Empty
                            REFRow("835_PAYEE_GUID") = Guid.Empty
                            REFRow("835_LX_GUID") = _835_LX_GUID
                            REFRow("835_CLP_GUID") = _835_CLP_GUID
                            REFRow("835_SVC_GUID") = Guid.Empty


                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then REFRow("REF01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else REFRow("REF01") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then REFRow("REF02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else REFRow("REF02") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then REFRow("REF03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else REFRow("REF03") = DBNull.Value



                            REFRow("ROW_NUMBER") = _ROW_COUNT

                            REFRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            REFRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            REFRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                            Select Case _835_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "LX"
                                    _835_LX_STRING = _835_LX_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "CLP"
                                    _835_CLP_STRING = _835_CLP_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "SVC"
                                    _835_SVC_STRING = _835_SVC_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select

                            CLP_REF.Rows.Add(REFRow)

                            _RowProcessedFlag = 1
                        End If
                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "LX - CLP :: REF", ex)
                    End Try



                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '  LX - CLP  :: DTM
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "DTM" Then



                            Dim DTMRow As DataRow = CLP_DTM.NewRow
                            DTMRow("DOCUMENT_ID") = _DOCUMENT_ID
                            DTMRow("FILE_ID") = _FILE_ID
                            DTMRow("BATCH_ID") = _BATCH_ID
                            DTMRow("ISA_ID") = _ISA_ID
                            DTMRow("GS_ID") = _GS_ID
                            DTMRow("ST_ID") = _ST_ID
                            DTMRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            DTMRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            DTMRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID

                            DTMRow("835_TSH_GUID") = _835_TSH_GUID
                            DTMRow("835_PAYOR_GUID") = Guid.Empty
                            DTMRow("835_PAYEE_GUID") = Guid.Empty
                            DTMRow("835_LX_GUID") = _835_LX_GUID
                            DTMRow("835_CLP_GUID") = _835_CLP_GUID
                            DTMRow("835_SVC_GUID") = Guid.Empty


                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then DTMRow("DTM01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else DTMRow("DTM01") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then DTMRow("DTM02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else DTMRow("DTM02") = DBNull.Value


                            DTMRow("ROW_NUMBER") = _ROW_COUNT
                            DTMRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            DTMRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            DTMRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix



                            Select Case _835_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "LX"
                                    _835_LX_STRING = _835_LX_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "CLP"
                                    _835_CLP_STRING = _835_CLP_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "SVC"
                                    _835_SVC_STRING = _835_SVC_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select


                            CLP_DTM.Rows.Add(DTMRow)

                            _RowProcessedFlag = 1
                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "LX - CLP :: DTM", ex)
                    End Try



                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   LX - CLP  :: AMT
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "AMT" Then


                            Dim AMTRow As DataRow = CLP_AMT.NewRow
                            AMTRow("DOCUMENT_ID") = _DOCUMENT_ID
                            AMTRow("FILE_ID") = _FILE_ID
                            AMTRow("BATCH_ID") = _BATCH_ID
                            AMTRow("ISA_ID") = _ISA_ID
                            AMTRow("GS_ID") = _GS_ID
                            AMTRow("ST_ID") = _ST_ID
                            AMTRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            AMTRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            AMTRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            AMTRow("835_TSH_GUID") = _835_TSH_GUID
                            AMTRow("835_PAYOR_GUID") = Guid.Empty
                            AMTRow("835_PAYEE_GUID") = Guid.Empty
                            AMTRow("835_LX_GUID") = _835_LX_GUID
                            AMTRow("835_CLP_GUID") = _835_CLP_GUID
                            AMTRow("835_SVC_GUID") = Guid.Empty

                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then AMTRow("AMT01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else AMTRow("AMT01") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then AMTRow("AMT02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else AMTRow("AMT02") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then AMTRow("AMT03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else AMTRow("AMT03") = DBNull.Value

                            AMTRow("ROW_NUMBER") = _ROW_COUNT


                            AMTRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            AMTRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                            AMTRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor




                            Select Case _835_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "LX"
                                    _835_LX_STRING = _835_LX_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "CLP"
                                    _835_CLP_STRING = _835_CLP_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "SVC"
                                    _835_SVC_STRING = _835_SVC_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select

                            CLP_AMT.Rows.Add(AMTRow)

                            _RowProcessedFlag = 1
                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "LX - CLP :: AMT", ex)
                    End Try



                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   LX - CLP  :: QTY
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "QTY" Then


                            Dim QTYRow As DataRow = CLP_QTY.NewRow
                            QTYRow("DOCUMENT_ID") = _DOCUMENT_ID
                            QTYRow("FILE_ID") = _FILE_ID
                            QTYRow("BATCH_ID") = _BATCH_ID
                            QTYRow("ISA_ID") = _ISA_ID
                            QTYRow("GS_ID") = _GS_ID
                            QTYRow("ST_ID") = _ST_ID
                            QTYRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            QTYRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            QTYRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            QTYRow("835_TSH_GUID") = _835_TSH_GUID
                            QTYRow("835_PAYOR_GUID") = Guid.Empty
                            QTYRow("835_PAYEE_GUID") = Guid.Empty
                            QTYRow("835_LX_GUID") = _835_LX_GUID
                            QTYRow("835_CLP_GUID") = _835_CLP_GUID
                            QTYRow("835_SVC_GUID") = Guid.Empty


                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then QTYRow("QTY01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else QTYRow("QTY01") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then QTYRow("QTY02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else QTYRow("QTY02") = DBNull.Value

                            QTYRow("ROW_NUMBER") = _ROW_COUNT


                            QTYRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            QTYRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                            QTYRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor

                            CLP_QTY.Rows.Add(QTYRow)

                            _RowProcessedFlag = 1
                        End If



                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "LX - CLP :: QTY", ex)
                    End Try

                End If
                ' END CLP BLOCK
                '==========================================================================================================================================================








                '==========================================================================================================================================================
                ' SVC BLOCK
                If _835_LOOP_LEVEL = "SVC" Then


                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   LX - CLP - SVC :: SVC
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "SVC" Then


                            Dim SVCRow As DataRow = SVC_SVC.NewRow
                            SVCRow("DOCUMENT_ID") = _DOCUMENT_ID
                            SVCRow("FILE_ID") = _FILE_ID
                            SVCRow("BATCH_ID") = _BATCH_ID
                            SVCRow("ISA_ID") = _ISA_ID
                            SVCRow("GS_ID") = _GS_ID
                            SVCRow("ST_ID") = _ST_ID
                            SVCRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            SVCRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            SVCRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID

                            SVCRow("835_TSH_GUID") = _835_TSH_GUID
                            SVCRow("835_PAYOR_GUID") = Guid.Empty
                            SVCRow("835_PAYEE_GUID") = Guid.Empty
                            SVCRow("835_LX_GUID") = _835_LX_GUID
                            SVCRow("835_CLP_GUID") = _835_CLP_GUID
                            SVCRow("835_SVC_GUID") = _835_SVC_GUID









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



                            SVCRow("ROW_NUMBER") = _ROW_COUNT

                            SVCRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            SVCRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            SVCRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix


                            Select Case _835_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "LX"
                                    _835_LX_STRING = _835_LX_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "CLP"
                                    _835_CLP_STRING = _835_CLP_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "SVC"
                                    _835_SVC_STRING = _835_SVC_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select


                            SVC_SVC.Rows.Add(SVCRow)

                            _RowProcessedFlag = 1
                        End If



                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "LX - CLP - SVC :: SVC", ex)
                    End Try



                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '  LX - CLP - SVC :: DTM
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "DTM" Then



                            Dim DTMRow As DataRow = SVC_DTM.NewRow
                            DTMRow("DOCUMENT_ID") = _DOCUMENT_ID
                            DTMRow("FILE_ID") = _FILE_ID
                            DTMRow("BATCH_ID") = _BATCH_ID
                            DTMRow("ISA_ID") = _ISA_ID
                            DTMRow("GS_ID") = _GS_ID
                            DTMRow("ST_ID") = _ST_ID
                            DTMRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            DTMRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            DTMRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID

                            DTMRow("835_TSH_GUID") = _835_TSH_GUID
                            DTMRow("835_PAYOR_GUID") = Guid.Empty
                            DTMRow("835_PAYEE_GUID") = Guid.Empty
                            DTMRow("835_LX_GUID") = _835_LX_GUID
                            DTMRow("835_CLP_GUID") = _835_CLP_GUID
                            DTMRow("835_SVC_GUID") = _835_SVC_GUID


                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then DTMRow("DTM01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else DTMRow("DTM01") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then DTMRow("DTM02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else DTMRow("DTM02") = DBNull.Value


                            DTMRow("ROW_NUMBER") = _ROW_COUNT
                            DTMRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            DTMRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            DTMRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix






                            Select Case _835_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "LX"
                                    _835_LX_STRING = _835_LX_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "CLP"
                                    _835_CLP_STRING = _835_CLP_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "SVC"
                                    _835_SVC_STRING = _835_SVC_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select


                            SVC_DTM.Rows.Add(DTMRow)

                            _RowProcessedFlag = 1
                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "LX - CLP - SVC :: DTM", ex)
                    End Try



                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '  LX - CLP - SVC :: CAS
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "CAS" Then


                            Dim CASRow As DataRow = SVC_CAS.NewRow
                            CASRow("DOCUMENT_ID") = _DOCUMENT_ID
                            CASRow("FILE_ID") = _FILE_ID
                            CASRow("BATCH_ID") = _BATCH_ID
                            CASRow("ISA_ID") = _ISA_ID
                            CASRow("GS_ID") = _GS_ID
                            CASRow("ST_ID") = _ST_ID
                            CASRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            CASRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            CASRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            CASRow("835_TSH_GUID") = _835_TSH_GUID
                            CASRow("835_PAYOR_GUID") = Guid.Empty
                            CASRow("835_PAYEE_GUID") = Guid.Empty
                            CASRow("835_LX_GUID") = _835_LX_GUID
                            CASRow("835_CLP_GUID") = _835_CLP_GUID
                            CASRow("835_SVC_GUID") = _835_SVC_GUID



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



                            CASRow("ROW_NUMBER") = _ROW_COUNT


                            CASRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            CASRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                            CASRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor


                            SVC_CAS.Rows.Add(CASRow)

                            Select Case _835_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "LX"
                                    _835_LX_STRING = _835_LX_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "CLP"
                                    _835_CLP_STRING = _835_CLP_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "SVC"
                                    _835_SVC_STRING = _835_SVC_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select

                            _RowProcessedFlag = 1
                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "LX - CLP - SVC :: CAS", ex)
                    End Try


                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   LX - CLP - SVC :: REF
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "REF" Then



                            Dim REFRow As DataRow = SVC_REF.NewRow
                            REFRow("DOCUMENT_ID") = _DOCUMENT_ID
                            REFRow("FILE_ID") = _FILE_ID
                            REFRow("BATCH_ID") = _BATCH_ID
                            REFRow("ISA_ID") = _ISA_ID
                            REFRow("GS_ID") = _GS_ID
                            REFRow("ST_ID") = _ST_ID
                            REFRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            REFRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            REFRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID

                            REFRow("835_TSH_GUID") = _835_TSH_GUID
                            REFRow("835_PAYOR_GUID") = Guid.Empty
                            REFRow("835_PAYEE_GUID") = Guid.Empty
                            REFRow("835_LX_GUID") = _835_LX_GUID
                            REFRow("835_CLP_GUID") = _835_CLP_GUID
                            REFRow("835_SVC_GUID") = _835_SVC_GUID


                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then REFRow("REF01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else REFRow("REF01") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then REFRow("REF02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else REFRow("REF02") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then REFRow("REF03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else REFRow("REF03") = DBNull.Value



                            REFRow("ROW_NUMBER") = _ROW_COUNT

                            REFRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            REFRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            REFRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                            Select Case _835_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "LX"
                                    _835_LX_STRING = _835_LX_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "CLP"
                                    _835_CLP_STRING = _835_CLP_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "SVC"
                                    _835_SVC_STRING = _835_SVC_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select

                            SVC_REF.Rows.Add(REFRow)

                            _RowProcessedFlag = 1
                        End If
                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "LX - CLP - SVC :: REF", ex)
                    End Try

                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   LX - CLP - SVC  :: AMT
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "AMT" Then


                            Dim AMTRow As DataRow = SVC_AMT.NewRow
                            AMTRow("DOCUMENT_ID") = _DOCUMENT_ID
                            AMTRow("FILE_ID") = _FILE_ID
                            AMTRow("BATCH_ID") = _BATCH_ID
                            AMTRow("ISA_ID") = _ISA_ID
                            AMTRow("GS_ID") = _GS_ID
                            AMTRow("ST_ID") = _ST_ID
                            AMTRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            AMTRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            AMTRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                            AMTRow("835_TSH_GUID") = _835_TSH_GUID
                            AMTRow("835_PAYOR_GUID") = Guid.Empty
                            AMTRow("835_PAYEE_GUID") = Guid.Empty
                            AMTRow("835_LX_GUID") = _835_LX_GUID
                            AMTRow("835_CLP_GUID") = _835_CLP_GUID
                            AMTRow("835_SVC_GUID") = _835_SVC_GUID

                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then AMTRow("AMT01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else AMTRow("AMT01") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then AMTRow("AMT02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else AMTRow("AMT02") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then AMTRow("AMT03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else AMTRow("AMT03") = DBNull.Value

                            AMTRow("ROW_NUMBER") = _ROW_COUNT


                            AMTRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            AMTRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                            AMTRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor


                            SVC_AMT.Rows.Add(AMTRow)

                            Select Case _835_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "LX"
                                    _835_LX_STRING = _835_LX_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "CLP"
                                    _835_CLP_STRING = _835_CLP_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "SVC"
                                    _835_SVC_STRING = _835_SVC_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select


                            _RowProcessedFlag = 1
                        End If

                    Catch ex As Exception

                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "LX - CLP - SVC :: AMT", ex)
                    End Try






                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   LX - CLP - SVC  :: LQ
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    Try
                        If _RowRecordType = "LQ" Then



                            Dim LQRow As DataRow = SVC_LQ.NewRow
                            LQRow("DOCUMENT_ID") = _DOCUMENT_ID
                            LQRow("FILE_ID") = _FILE_ID
                            LQRow("BATCH_ID") = _BATCH_ID
                            LQRow("ISA_ID") = _ISA_ID
                            LQRow("GS_ID") = _GS_ID
                            LQRow("ST_ID") = _ST_ID
                            LQRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                            LQRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                            LQRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID

                            LQRow("835_TSH_GUID") = _835_TSH_GUID
                            LQRow("835_PAYOR_GUID") = Guid.Empty
                            LQRow("835_PAYEE_GUID") = Guid.Empty
                            LQRow("835_LX_GUID") = _835_LX_GUID
                            LQRow("835_CLP_GUID") = _835_CLP_GUID
                            LQRow("835_SVC_GUID") = _835_SVC_GUID


                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then LQRow("LQ01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else LQRow("LQ01") = DBNull.Value
                            If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then LQRow("LQ02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else LQRow("LQ02") = DBNull.Value


                            LQRow("ROW_NUMBER") = _ROW_COUNT
                            LQRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                            LQRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                            LQRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix

                            Select Case _835_LOOP_LEVEL
                                Case "HEADER"
                                    _RAW_HEADER = _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "LX"
                                    _835_LX_STRING = _835_LX_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "CLP"
                                    _835_CLP_STRING = _835_CLP_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                                Case "SVC"
                                    _835_SVC_STRING = _835_SVC_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            End Select


                            SVC_LQ.Rows.Add(LQRow)


                            _RowProcessedFlag = 1
                        End If

                    Catch ex As Exception
                        _RowProcessedFlag = -2
                        _hasERR = True
                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "LX - CLP - SVC :: LQ", ex)
                    End Try


                End If
                ' END SVC BLOCK
                '==========================================================================================================================================================





                Try
                    If _RowRecordType = "PLB" Then



                        Dim PLBRow As DataRow = PLB.NewRow
                        PLBRow("DOCUMENT_ID") = _DOCUMENT_ID
                        PLBRow("FILE_ID") = _FILE_ID
                        PLBRow("BATCH_ID") = _BATCH_ID
                        PLBRow("ISA_ID") = _ISA_ID
                        PLBRow("GS_ID") = _GS_ID
                        PLBRow("ST_ID") = _ST_ID
                        PLBRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                        PLBRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                        PLBRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID




                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then PLBRow("PLB01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else PLBRow("PLB01") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then PLBRow("PLB02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else PLBRow("PLB02") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then PLBRow("PLB03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else PLBRow("PLB03") = DBNull.Value


                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then
                            PLBRow("PLB03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4)
                            _PLB03 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4)
                        Else
                            PLBRow("PLB03") = DBNull.Value

                        End If

                        If (ss.ParseDemlimtedString(_PLB03, _ComponentElementSeparator, 1) <> "") Then PLBRow("PLB03_1") = ss.ParseDemlimtedString(_PLB03, _ComponentElementSeparator, 1) Else PLBRow("PLB03_1") = DBNull.Value
                        If (ss.ParseDemlimtedString(_PLB03, _ComponentElementSeparator, 2) <> "") Then PLBRow("PLB03_2") = ss.ParseDemlimtedString(_PLB03, _ComponentElementSeparator, 2) Else PLBRow("PLB03_2") = DBNull.Value




                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then PLBRow("PLB04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else PLBRow("PLB04") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then PLBRow("PLB05") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else PLBRow("PLB05") = DBNull.Value



                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then
                            PLBRow("PLB05") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6)
                            _PLB05 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6)
                        Else
                            PLBRow("PLB05") = DBNull.Value

                        End If


                        If (ss.ParseDemlimtedString(_PLB05, _ComponentElementSeparator, 1) <> "") Then PLBRow("PLB05_1") = ss.ParseDemlimtedString(_PLB05, _ComponentElementSeparator, 1) Else PLBRow("PLB05_1") = DBNull.Value
                        If (ss.ParseDemlimtedString(_PLB05, _ComponentElementSeparator, 2) <> "") Then PLBRow("PLB05_2") = ss.ParseDemlimtedString(_PLB05, _ComponentElementSeparator, 2) Else PLBRow("PLB05_2") = DBNull.Value


                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then PLBRow("PLB06") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) Else PLBRow("PLB06") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then PLBRow("PLB07") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) Else PLBRow("PLB07") = DBNull.Value




                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then
                            PLBRow("PLB07") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8)
                            _PLB07 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8)
                        Else
                            PLBRow("PLB07") = DBNull.Value

                        End If



                        If (ss.ParseDemlimtedString(_PLB07, _ComponentElementSeparator, 1) <> "") Then PLBRow("PLB07_1") = ss.ParseDemlimtedString(_PLB07, _ComponentElementSeparator, 1) Else PLBRow("PLB07_1") = DBNull.Value
                        If (ss.ParseDemlimtedString(_PLB07, _ComponentElementSeparator, 2) <> "") Then PLBRow("PLB07_2") = ss.ParseDemlimtedString(_PLB07, _ComponentElementSeparator, 2) Else PLBRow("PLB07_2") = DBNull.Value




                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) <> "") Then PLBRow("PLB08") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) Else PLBRow("PLB08") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) <> "") Then PLBRow("PLB09") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) Else PLBRow("PLB09") = DBNull.Value


                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) <> "") Then
                            PLBRow("PLB09") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10)
                            _PLB09 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10)
                        Else
                            PLBRow("PLB09") = DBNull.Value

                        End If


                        If (ss.ParseDemlimtedString(_PLB09, _ComponentElementSeparator, 1) <> "") Then PLBRow("PLB09_1") = ss.ParseDemlimtedString(_PLB09, _ComponentElementSeparator, 1) Else PLBRow("PLB09_1") = DBNull.Value
                        If (ss.ParseDemlimtedString(_PLB09, _ComponentElementSeparator, 2) <> "") Then PLBRow("PLB09_2") = ss.ParseDemlimtedString(_PLB09, _ComponentElementSeparator, 2) Else PLBRow("PLB09_2") = DBNull.Value


                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11) <> "") Then PLBRow("PLB10") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11) Else PLBRow("PLB10") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12) <> "") Then PLBRow("PLB11") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12) Else PLBRow("PLB11") = DBNull.Value


                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12) <> "") Then
                            PLBRow("PLB11") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12)
                            _PLB11 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12)
                        Else
                            PLBRow("PLB11") = DBNull.Value

                        End If


                        If (ss.ParseDemlimtedString(_PLB11, _ComponentElementSeparator, 1) <> "") Then PLBRow("PLB11_1") = ss.ParseDemlimtedString(_PLB11, _ComponentElementSeparator, 1) Else PLBRow("PLB11_1") = DBNull.Value
                        If (ss.ParseDemlimtedString(_PLB11, _ComponentElementSeparator, 2) <> "") Then PLBRow("PLB11_2") = ss.ParseDemlimtedString(_PLB11, _ComponentElementSeparator, 2) Else PLBRow("PLB11_2") = DBNull.Value




                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 13) <> "") Then PLBRow("PLB12") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 13) Else PLBRow("PLB12") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 14) <> "") Then PLBRow("PLB13") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 14) Else PLBRow("PLB13") = DBNull.Value



                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 14) <> "") Then
                            PLBRow("PLB13") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 14)
                            _PLB13 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 14)
                        Else
                            PLBRow("PLB13") = DBNull.Value

                        End If


                        If (ss.ParseDemlimtedString(_PLB13, _ComponentElementSeparator, 1) <> "") Then PLBRow("PLB13_1") = ss.ParseDemlimtedString(_PLB13, _ComponentElementSeparator, 1) Else PLBRow("PLB13_1") = DBNull.Value
                        If (ss.ParseDemlimtedString(_PLB13, _ComponentElementSeparator, 2) <> "") Then PLBRow("PLB13_2") = ss.ParseDemlimtedString(_PLB13, _ComponentElementSeparator, 2) Else PLBRow("PLB13_2") = DBNull.Value


                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 15) <> "") Then PLBRow("PLB14") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 15) Else PLBRow("PLB14") = DBNull.Value


                        PLBRow("ROW_NUMBER") = _ROW_COUNT

                        PLBRow("LOOP_LEVEL_MAJOR") = 0
                        PLBRow("LOOP_LEVEL_MINOR") = 0
                        PLBRow("LOOP_LEVEL_SUBFIX") = ""


                        PLB.Rows.Add(PLBRow)
                        _835_PLB_STRING = Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"


                        _RowProcessedFlag = 1
                    End If
                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "PLB", ex)
                End Try









                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   END 835
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

                        SERow("ROW_NUMBER") = _ROW_COUNT
                        SE.Rows.Add(SERow)

                        ' COMMIT THE LAST LK SET SINCE WE WONT GET TO lx AGAIN AND THE LAST clm SET
                        '  ComitLX()
                        'ComitRowData()



                        If _LX_DIRTY Then
                            ComitLX()
                        End If

                        If _CLP_DIRTY Then
                            ComitCLP()
                            ClearCLP()
                            _CLP_DIRTY = False
                        End If

                        ComitPLB()

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







                        Dim GERow As DataRow = GE.NewRow
                        GERow("FILE_ID") = _FILE_ID
                        GERow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                        GERow("HIPAA_GS_GUID") = _HIPAA_GS_GUID


                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then GERow("GE01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else GERow("GE01") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then GERow("GE02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else GERow("GE02") = DBNull.Value

                        GERow("ROW_NUMBER") = _ROW_COUNT
                        GE.Rows.Add(GERow)


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




                        Dim IEARow As DataRow = IEA.NewRow
                        IEARow("FILE_ID") = _FILE_ID
                        IEARow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID


                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then IEARow("IEA01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else IEARow("IEA01") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then IEARow("IEA02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else IEARow("IEA02") = DBNull.Value

                        IEARow("ROW_NUMBER") = _ROW_COUNT
                        IEA.Rows.Add(IEARow)


                        ComitIEA()
                        ClearISA()

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
                        UNKRow("HIPAA_HL_19_GUID") = _835_TSH_GUID
                        UNKRow("HIPAA_HL_20_GUID") = _835_PAYOR_GUID
                        UNKRow("HIPAA_HL_21_GUID") = _835_PAYEE_GUID
                        UNKRow("HIPAA_HL_22_GUID") = _835_LX_GUID
                        UNKRow("HIPAA_HL_23_GUID") = _835_CLP_GUID
                        UNKRow("HIPAA_HL_24_GUID") = _835_SVC_GUID
                        UNKRow("ROW_RECORD_TYPE") = _RowRecordType
                        UNKRow("ROW_DATA") = _CurrentRowData
                        UNKRow("ROW_NUMBER") = _ROW_COUNT
                        UNKRow("LOOP_LEVEL_MAJOR") = _LoopLevelMajor
                        UNKRow("LOOP_LEVEL_MINOR") = _LoopLevelMinor
                        UNKRow("LOOP_LEVEL_SUBFIX") = _LoopLevelSubFix
                        UNK.Rows.Add(UNKRow)




                        Select Case _835_LOOP_LEVEL
                            Case "HEADER"
                                _RAW_HEADER = "UNK::" + _RAW_HEADER + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case "LX"
                                _835_LX_STRING = "UNK::" + _835_LX_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case "CLP"
                                _835_CLP_STRING = "UNK::" + _835_CLP_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
                            Case "SVC"
                                _835_SVC_STRING = "UNK::" + _835_SVC_STRING + Convert.ToString(_ROW_COUNT) + "::" + _CurrentRowData + "~"
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
                        e.TransactionSetIdentifierCode = "835"
                        If _hasERR Then
                            e.UpdateFileStatus(CInt(_FILE_ID), "PARSE COMPLETE WITH UNK SEGMENTS WITH ERRORS", "835")
                            _IMPORT_RETURN_STRING = "835 : PARSE COMPLETE WITH UNK SEGMENTS WITH ERRORS"
                        Else
                            e.UpdateFileStatus(CInt(_FILE_ID), "PARSE COMPLETE WITH UNK SEGMENTS", "835")
                            _IMPORT_RETURN_STRING = "835 : PARSE COMPLETE WITH UNK SEGMENTS"
                        End If



                    End Using

                Else
                    If _hasERR Then

                        _IMPORT_RETURN_STRING = "835 : PARSE COMPLETE WITH UNK SEGMENTS WITH ERRORS"
                    Else

                        _IMPORT_RETURN_STRING = "835 : PARSE COMPLETE WITH UNK SEGMENTS"
                    End If

                End If
                ComitUNK()

            Else
                If (_IS_FILE) Then


                    Using e As New EDI_5010_LOGGING
                        e.ConnectionString = _ConnectionString
                        e.TransactionSetIdentifierCode = "835"
                        If _hasERR Then
                            e.UpdateFileStatus(CInt(_FILE_ID), "PARSE COMPLETE WITH ERRORS", "835")
                            _IMPORT_RETURN_STRING = "835 : PARSE COMPLETE WITH ERRORS"
                        Else
                            e.UpdateFileStatus(CInt(_FILE_ID), "PARSE COMPLETE", "835")
                            _IMPORT_RETURN_STRING = "835 : PARSE COMPLETE"
                        End If
                    End Using
                Else

                    If _hasERR Then

                        _IMPORT_RETURN_STRING = "835 : PARSE COMPLETE WITH ERRORS"
                    Else

                        _IMPORT_RETURN_STRING = "835 : PARSE COMPLETE"
                    End If


                End If




            End If



            'If _hasERR Then
            '    '  RollBack()
            '    _IMPORT_RETURN_STRING = _IMPORT_RETURN_STRING ' + " ROLLED BACK EDI_SEQUENCE_NUMBER " + Convert.ToString(_EDI_SEQUENCE_NUMBER)
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
                        cmd.Parameters.AddWithValue("@HIPAA_835_ISA", ISA)
                        cmd.Parameters.Add("@ISA_ID", Data.SqlDbType.BigInt, 1)
                        cmd.Parameters("@ISA_ID").Direction = ParameterDirection.Output
                        cmd.ExecuteNonQuery()

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





        Private Function ComitGS() As Integer

            Dim _RETURN_CODE As Integer = -1
            _FUNCTION_NAME = "Function ComitGS() As Integer"

            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_GS, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_835_GS", GS)

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
            _FUNCTION_NAME = "Function ComitST() As Integer"

            Try



                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_ST, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_835_ST", ST)
                        cmd.Parameters.AddWithValue("@HIPAA_835_BPR", BPR)
                        cmd.Parameters.AddWithValue("@HIPAA_835_TRN", TRN)
                        cmd.Parameters.AddWithValue("@HIPAA_835_REF", REF)
                        cmd.Parameters.Add("@ST_ID", Data.SqlDbType.BigInt, 1)
                        cmd.Parameters("@ST_ID").Direction = ParameterDirection.Output
                        '  cmd.ExecuteNonQuery()

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
            _FUNCTION_NAME = "Function ComitHEADER() As Integer"

            Try
                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_HEADERS, Con)

                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.AddWithValue("@HIPAA_835_ST", ST)
                        cmd.Parameters.AddWithValue("@HIPAA_835_BPR", BPR)
                        cmd.Parameters.AddWithValue("@HIPAA_835_TRN", TRN)
                        cmd.Parameters.AddWithValue("@HIPAA_835_N1", N1)
                        cmd.Parameters.AddWithValue("@HIPAA_835_N3", N3)
                        cmd.Parameters.AddWithValue("@HIPAA_835_N4", N4)
                        cmd.Parameters.AddWithValue("@HIPAA_835_DTM", DTM)
                        cmd.Parameters.AddWithValue("@HIPAA_835_REF", REF)
                        cmd.Parameters.AddWithValue("@HIPAA_835_PER", PER)

                        cmd.Parameters.Add("@ST_ID", Data.SqlDbType.BigInt, 1)
                        cmd.Parameters("@ST_ID").Direction = ParameterDirection.Output
                        cmd.ExecuteNonQuery()

                        _ST_ID = Convert.ToInt32(cmd.Parameters("@ST_ID").Value.ToString())

                        CACHEHeader()

                        _HEADER_DIRTY = False

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

        Private Function ComitLX() As Integer
            Dim _RETURN_CODE As Integer = -1
            _FUNCTION_NAME = "Function ComitLX() As Integer"

            Try



                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_COMIT_LX_DATA, Con)

                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.AddWithValue("@HIPAA_835_LX", LX)
                        cmd.Parameters.AddWithValue("@HIPAA_835_TS2", TS2)
                        cmd.Parameters.AddWithValue("@HIPAA_835_TS3", TS3)
                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using

                _LX_DIRTY = False

                _RETURN_CODE = 0

            Catch ex As Exception
                _hasERR = True
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "", ex)
            End Try





            Return _RETURN_CODE
        End Function

        Private Function ComitCLP() As Integer
            Dim _RETURN_CODE As Integer = -1
            _FUNCTION_NAME = "Function ComitCLP() As Integer"

            Try
                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_COMIT_CLP_DATA, Con)

                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.AddWithValue("@HIPAA_CACHE_835_ST", ST)
                        cmd.Parameters.AddWithValue("@HIPAA_CACHE_835_BPR", BPR)
                        cmd.Parameters.AddWithValue("@HIPAA_CACHE_835_TRN", TRN)
                        cmd.Parameters.AddWithValue("@HIPAA_CACHE_835_REF", REF)
                        cmd.Parameters.AddWithValue("@HIPAA_CACHE_835_DTM", DTM)
                        cmd.Parameters.AddWithValue("@HIPAA_CACHE_835_N1", N1)
                        cmd.Parameters.AddWithValue("@HIPAA_CACHE_835_N3", N3)
                        cmd.Parameters.AddWithValue("@HIPAA_CACHE_835_N4", N4)
                        cmd.Parameters.AddWithValue("@HIPAA_CACHE_835_PER", PER)


                        cmd.Parameters.AddWithValue("@HIPAA_835_LX", LX)
                        cmd.Parameters.AddWithValue("@HIPAA_835_TS2", TS2)
                        cmd.Parameters.AddWithValue("@HIPAA_835_TS3", TS3)

                        cmd.Parameters.AddWithValue("@HIPAA_835_PLB", PLB)

                        M_CAS.Merge(SVC_CAS)
                        M_CAS.Merge(CLP_CAS)

                        M_REF.Merge(SVC_REF)
                        M_REF.Merge(CLP_REF)

                        M_AMT.Merge(SVC_AMT)
                        M_AMT.Merge(CLP_AMT)

                        M_DTM.Merge(SVC_DTM)
                        M_DTM.Merge(CLP_DTM)

                        cmd.Parameters.AddWithValue("@HIPAA_835_CAS", M_CAS)
                        cmd.Parameters.AddWithValue("@HIPAA_835_REF", M_REF)
                        cmd.Parameters.AddWithValue("@HIPAA_835_AMT", M_AMT)
                        cmd.Parameters.AddWithValue("@HIPAA_835_DTM", M_DTM)


                        ' CLP SEGMENTS
                        cmd.Parameters.AddWithValue("@HIPAA_835_CLP_CLP", CLP_CLP)
                        cmd.Parameters.AddWithValue("@HIPAA_835_CLP_CAS", CLP_CAS)
                        cmd.Parameters.AddWithValue("@HIPAA_835_CLP_NM1", CLP_NM1)
                        cmd.Parameters.AddWithValue("@HIPAA_835_CLP_MIA", CLP_MIA)
                        cmd.Parameters.AddWithValue("@HIPAA_835_CLP_MOA", CLP_MOA)
                        cmd.Parameters.AddWithValue("@HIPAA_835_CLP_REF", CLP_REF)
                        cmd.Parameters.AddWithValue("@HIPAA_835_CLP_DTM", CLP_DTM)
                        cmd.Parameters.AddWithValue("@HIPAA_835_CLP_AMT", CLP_AMT)
                        cmd.Parameters.AddWithValue("@HIPAA_835_CLP_QTY", CLP_QTY)

                        'SVC SEGMENTS
                        cmd.Parameters.AddWithValue("@HIPAA_835_SVC_SVC", SVC_SVC)
                        cmd.Parameters.AddWithValue("@HIPAA_835_SVC_DTM", SVC_DTM)
                        cmd.Parameters.AddWithValue("@HIPAA_835_SVC_CAS", SVC_CAS)
                        cmd.Parameters.AddWithValue("@HIPAA_835_SVC_REF", SVC_REF)
                        cmd.Parameters.AddWithValue("@HIPAA_835_SVC_AMT", SVC_AMT)
                        cmd.Parameters.AddWithValue("@HIPAA_835_SVC_LQ", SVC_LQ)



                        cmd.Parameters.AddWithValue("@HIPAA_835_TSH_GUID", _835_TSH_GUID)
                        cmd.Parameters.AddWithValue("@HIPAA_835_PAYOR_GUID", _T_835_PAYOR_GUID)
                        cmd.Parameters.AddWithValue("@HIPAA_835_PAYEE_GUID", _T_835_PAYEE_GUID)


                        cmd.Parameters.AddWithValue("@HIPAA_835_CLP_GUID", _835_CLP_GUID)
                        cmd.Parameters.AddWithValue("@HIPAA_835_LX_GUID", _835_LX_GUID)


                        cmd.Parameters.AddWithValue("@HIPAA_835_HEADER_STRING", _835_HEADER_STRING)
                        cmd.Parameters.AddWithValue("@HIPAA_835_LX_STRING", _835_LX_STRING)
                        cmd.Parameters.AddWithValue("@HIPAA_835_CLP_STRING", _835_CLP_STRING)
                        cmd.Parameters.AddWithValue("@HIPAA_835_SVC_STRING", _835_SVC_STRING)

                        cmd.Parameters.AddWithValue("@HIPAA_835_PLB_STRING", _835_PLB_STRING)


                        cmd.Parameters.AddWithValue("@FILE_ID", _FILE_ID)
                        cmd.Parameters.AddWithValue("@ISA_ID", _ISA_ID)
                        cmd.Parameters.AddWithValue("@GS_ID", _GS_ID)
                        cmd.Parameters.AddWithValue("@ST_ID", _ST_ID)
                        ' REAL
                        cmd.ExecuteNonQuery()




                    End Using
                    Con.Close()
                End Using


                _CLP_DIRTY = False

                'CACHE_1000_NM1 = NM1.Copy
                'CACHE_1000_PER = PER.Copy


                'NM1.Clear()
                'PER.Clear()
                '_NM1_GUID = Guid.Empty
                _RETURN_CODE = 0

            Catch ex As Exception
                _hasERR = True
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "", ex)
            End Try



            Return _RETURN_CODE

        End Function







        Public Function ComitPLB() As Integer


            Dim _RETURN_CODE As Integer = -1
            _FUNCTION_NAME = "Function ComitPLB() As Integer"

            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_COMIT_PLB_DATA, Con)


                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_835_PLB", PLB)

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


        Private Function ComitSE() As Integer
            Dim _RETURN_CODE As Integer = -1
            _FUNCTION_NAME = "Function ComitSE() As Integer"

            Try
                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_SE, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_835_SE", SE)
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
            _FUNCTION_NAME = "Function ComitGE() As Integer"

            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_GE, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_835_GE", GE)


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
            _FUNCTION_NAME = "Function ComitIEA() As Integer"

            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_IEA, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_835_IEA", IEA)
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

        Private Function ComitRAW() As Integer


            Dim _RETURN_CODE As Integer = -1
            _FUNCTION_NAME = "Function ComitRAW() As Integer"

            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand("usp_EDI_5010_HIPAA_835_INSERT_RAW_EDI", Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@RAW_EDI", _RAW_EDI)
                        cmd.Parameters.AddWithValue("@FILE_ID", _FILE_ID)
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


        Private Sub ClearISA()

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







            _LoopLevelMajor = 0
            _LoopLevelMinor = 0
            _LoopLevelSubFix = ""

            ClearST()

        End Sub


        Private Sub ClearST()

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   BEGIN COMMON
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ST.Clear()
            SE.Clear()

            _ST_ID = 0
            _HIPAA_ST_GUID = Guid.Empty


            _ST = String.Empty

            _IX_DTM_FOUND = False

            BPR.Clear()
            DTM.Clear()

            N1.Clear()
            N3.Clear()
            N4.Clear()

            PER.Clear()

            REF.Clear()

            TRN.Clear()

            DTM.Clear()
            REF.Clear()


            ST.Clear()
            SE.Clear()
            BHT.Clear()

            _835_TSH_GUID = Guid.Empty
            _835_PAYOR_GUID = Guid.Empty
            _835_PAYEE_GUID = Guid.Empty

            _835_LX_GUID = Guid.Empty
            _835_CLP_GUID = Guid.Empty
            _835_SVC_GUID = Guid.Empty

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
            '  BEGIN 835 
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            REF.Clear()
            DTM.Clear()
            '_is_ST_BHT_NM1_PER = False


            _ST = String.Empty
            _835_HEADER_STRING = String.Empty
            _835_BPR_STRING = String.Empty

            '_BHT = String.Empty



            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '  BEGIN 835 
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''



            ClearHeader()


        End Sub

        Private Sub CACHEHeader()

            ''  CACHE_DTM = DTM.Copy
            '  CACHE_REF = REF.Copy

            '   DTM.Clear()
            '   REF.Clear()

            '  _835_TSH_GUID = Guid.Empty
            '  _835_PAYOR_GUID = Guid.Empty
            ' _835_PAYEE_GUID = Guid.Empty

        End Sub

        Private Sub ClearHeader()
            DTM.Clear()
            REF.Clear()

            _T_835_PAYOR_GUID = Guid.Empty
            _T_835_PAYEE_GUID = Guid.Empty
            '   CACHE_DTM.Clear()
            '  CACHE_REF.Clear()

            '_835_TSH_GUID = Guid.Empty
            ' _835_PAYOR_GUID = Guid.Empty
            ' _835_PAYEE_GUID = Guid.Empty

            ClearLX()
        End Sub



        Private Sub ClearLX()

            '     _835_LX_GUID = Guid.Empty
            '      _835_LX_STRING = String.Empty


            LX.Clear()

            TS2.Clear()
            TS3.Clear()


            '   ClearCLP()


        End Sub

        Private Sub ClearCLP()
            _835_CLP_GUID = Guid.Empty
            _835_SVC_GUID = Guid.Empty
            _P_GUID = Guid.Empty

            _835_CLP_STRING = String.Empty

            CLP_CLP.Clear()
            CLP_CAS.Clear()
            CLP_NM1.Clear()
            CLP_MIA.Clear()
            CLP_MOA.Clear()
            CLP_REF.Clear()
            CLP_DTM.Clear()
            CLP_AMT.Clear()
            CLP_QTY.Clear()



            M_CAS.Clear()
            M_REF.Clear()
            M_AMT.Clear()
            M_DTM.Clear()


            ClearSVC()

        End Sub

        Private Sub ClearSVC()
            _835_SVC_GUID = Guid.Empty
            _P_GUID = Guid.Empty

            _835_SVC_STRING = String.Empty

            SVC_SVC.Clear()
            SVC_DTM.Clear()
            SVC_CAS.Clear()
            SVC_REF.Clear()
            SVC_AMT.Clear()
            SVC_LQ.Clear()
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

                'Case "40"
                '    _LoopLevelMajor = 1000
                '    _LoopLevelSubFix = "B"


                'Case "41"
                '    _LoopLevelMajor = 1000
                '    _LoopLevelMinor = 0
                '    _LoopLevelSubFix = "A"


                'Case "71"
                '    If _LoopLevelMajor = 2300 Then
                '        _LoopLevelMinor = 30
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


                'Case "IL"

                '    If _LoopLevelMajor = 2300 Then
                '        _LoopLevelMajor = 2330
                '        _LoopLevelSubFix = "A"
                '    Else
                '        _LoopLevelMajor = 2010
                '        _LoopLevelSubFix = "BA"
                '    End If



                'Case "PR"

                '    If _LoopLevelMajor = 2330 Then

                '        _LoopLevelSubFix = "B"
                '    Else
                '        _LoopLevelMajor = 2010
                '        _LoopLevelSubFix = "BB"
                '    End If



                'Case "QC"
                '    _LoopLevelMajor = 2010
                '    _LoopLevelSubFix = "CA"


                'Case "ZZ"
                '    _LoopLevelMajor = 2310
                '    _LoopLevelSubFix = "C"


                Case Else





            End Select


            Return s


        End Function



        Public Function GET_PLB(ByVal EDI_PLB_List As List(Of String)) As String
            Dim _RETURN_CODE As Integer = -1


            Dim RAWPLB As String = String.Empty
            Dim PLB__ROW_COUNT As Integer = 0



            For Each line As String In EDI_PLB_List
                _RowProcessedFlag = 0

                If _DataElementSeparatorFlag = 0 Then
                    _DataElementSeparator = Mid(line, 4, 1)
                    _DataElementSeparatorFlag = 1
                End If


                line = line.Replace("~", "")
                PLB__ROW_COUNT = PLB__ROW_COUNT + 1

                _RowRecordType = ss.ParseDemlimtedStringEDI(line, _DataElementSeparator, 1)
                _CurrentRowData = line

                Try
                    If _RowRecordType = "PLB" Then



                        Dim PLBRow As DataRow = PLB.NewRow
                        PLBRow("DOCUMENT_ID") = _DOCUMENT_ID
                        PLBRow("FILE_ID") = _FILE_ID
                        PLBRow("BATCH_ID") = _BATCH_ID
                        PLBRow("ISA_ID") = _ISA_ID
                        PLBRow("GS_ID") = _GS_ID
                        PLBRow("ST_ID") = _ST_ID
                        PLBRow("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                        PLBRow("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                        PLBRow("HIPAA_ST_GUID") = _HIPAA_ST_GUID




                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then PLBRow("PLB01") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else PLBRow("PLB01") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then PLBRow("PLB02") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else PLBRow("PLB02") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then PLBRow("PLB03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else PLBRow("PLB03") = DBNull.Value


                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then
                            PLBRow("PLB03") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4)
                            _PLB03 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4)
                        Else
                            PLBRow("PLB03") = DBNull.Value

                        End If

                        If (ss.ParseDemlimtedString(_PLB03, _ComponentElementSeparator, 1) <> "") Then PLBRow("PLB03_1") = ss.ParseDemlimtedString(_PLB03, _ComponentElementSeparator, 1) Else PLBRow("PLB03_1") = DBNull.Value
                        If (ss.ParseDemlimtedString(_PLB03, _ComponentElementSeparator, 2) <> "") Then PLBRow("PLB03_2") = ss.ParseDemlimtedString(_PLB03, _ComponentElementSeparator, 2) Else PLBRow("PLB03_2") = DBNull.Value




                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then PLBRow("PLB04") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else PLBRow("PLB04") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then PLBRow("PLB05") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else PLBRow("PLB05") = DBNull.Value



                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then
                            PLBRow("PLB05") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6)
                            _PLB05 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6)
                        Else
                            PLBRow("PLB05") = DBNull.Value

                        End If


                        If (ss.ParseDemlimtedString(_PLB05, _ComponentElementSeparator, 1) <> "") Then PLBRow("PLB05_1") = ss.ParseDemlimtedString(_PLB05, _ComponentElementSeparator, 1) Else PLBRow("PLB05_1") = DBNull.Value
                        If (ss.ParseDemlimtedString(_PLB05, _ComponentElementSeparator, 2) <> "") Then PLBRow("PLB05_2") = ss.ParseDemlimtedString(_PLB05, _ComponentElementSeparator, 2) Else PLBRow("PLB05_2") = DBNull.Value


                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then PLBRow("PLB06") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) Else PLBRow("PLB06") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then PLBRow("PLB07") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) Else PLBRow("PLB07") = DBNull.Value




                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then
                            PLBRow("PLB07") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8)
                            _PLB07 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8)
                        Else
                            PLBRow("PLB07") = DBNull.Value

                        End If



                        If (ss.ParseDemlimtedString(_PLB07, _ComponentElementSeparator, 1) <> "") Then PLBRow("PLB07_1") = ss.ParseDemlimtedString(_PLB07, _ComponentElementSeparator, 1) Else PLBRow("PLB07_1") = DBNull.Value
                        If (ss.ParseDemlimtedString(_PLB07, _ComponentElementSeparator, 2) <> "") Then PLBRow("PLB07_2") = ss.ParseDemlimtedString(_PLB07, _ComponentElementSeparator, 2) Else PLBRow("PLB07_2") = DBNull.Value




                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) <> "") Then PLBRow("PLB08") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) Else PLBRow("PLB08") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) <> "") Then PLBRow("PLB09") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) Else PLBRow("PLB09") = DBNull.Value


                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) <> "") Then
                            PLBRow("PLB09") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10)
                            _PLB09 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10)
                        Else
                            PLBRow("PLB09") = DBNull.Value

                        End If


                        If (ss.ParseDemlimtedString(_PLB09, _ComponentElementSeparator, 1) <> "") Then PLBRow("PLB09_1") = ss.ParseDemlimtedString(_PLB09, _ComponentElementSeparator, 1) Else PLBRow("PLB09_1") = DBNull.Value
                        If (ss.ParseDemlimtedString(_PLB09, _ComponentElementSeparator, 2) <> "") Then PLBRow("PLB09_2") = ss.ParseDemlimtedString(_PLB09, _ComponentElementSeparator, 2) Else PLBRow("PLB09_2") = DBNull.Value


                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11) <> "") Then PLBRow("PLB10") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 11) Else PLBRow("PLB10") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12) <> "") Then PLBRow("PLB11") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12) Else PLBRow("PLB11") = DBNull.Value


                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12) <> "") Then
                            PLBRow("PLB11") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12)
                            _PLB11 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 12)
                        Else
                            PLBRow("PLB11") = DBNull.Value

                        End If


                        If (ss.ParseDemlimtedString(_PLB11, _ComponentElementSeparator, 1) <> "") Then PLBRow("PLB11_1") = ss.ParseDemlimtedString(_PLB11, _ComponentElementSeparator, 1) Else PLBRow("PLB11_1") = DBNull.Value
                        If (ss.ParseDemlimtedString(_PLB11, _ComponentElementSeparator, 2) <> "") Then PLBRow("PLB11_2") = ss.ParseDemlimtedString(_PLB11, _ComponentElementSeparator, 2) Else PLBRow("PLB11_2") = DBNull.Value




                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 13) <> "") Then PLBRow("PLB12") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 13) Else PLBRow("PLB12") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 14) <> "") Then PLBRow("PLB13") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 14) Else PLBRow("PLB13") = DBNull.Value



                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 14) <> "") Then
                            PLBRow("PLB13") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 14)
                            _PLB13 = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 14)
                        Else
                            PLBRow("PLB13") = DBNull.Value

                        End If


                        If (ss.ParseDemlimtedString(_PLB13, _ComponentElementSeparator, 1) <> "") Then PLBRow("PLB13_1") = ss.ParseDemlimtedString(_PLB13, _ComponentElementSeparator, 1) Else PLBRow("PLB13_1") = DBNull.Value
                        If (ss.ParseDemlimtedString(_PLB13, _ComponentElementSeparator, 2) <> "") Then PLBRow("PLB13_2") = ss.ParseDemlimtedString(_PLB13, _ComponentElementSeparator, 2) Else PLBRow("PLB13_2") = DBNull.Value


                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 15) <> "") Then PLBRow("PLB14") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 15) Else PLBRow("PLB14") = DBNull.Value


                        PLBRow("ROW_NUMBER") = PLB__ROW_COUNT

                        PLBRow("LOOP_LEVEL_MAJOR") = 0
                        PLBRow("LOOP_LEVEL_MINOR") = 0
                        PLBRow("LOOP_LEVEL_SUBFIX") = ""


                        PLB.Rows.Add(PLBRow)
                        RAWPLB = Convert.ToString(PLB__ROW_COUNT) + "::" + _CurrentRowData + "~"
                        _RowProcessedFlag = 1


                        _RETURN_CODE = 0

                    End If


                Catch ex As Exception
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "", ex)
                End Try


            Next

            ComitPLB()


            Return RAWPLB
        End Function



        Public Function TestSPs() As Integer

            Dim _RETURN_CODE As Integer = -1

            _FUNCTION_NAME = "Function TestSPs() As Integer"

            Try



                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_HEADERS_TEST, Con)

                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.AddWithValue("@HIPAA_835_ST", ST)
                        cmd.Parameters.AddWithValue("@HIPAA_835_BPR", BPR)
                        cmd.Parameters.AddWithValue("@HIPAA_835_TRN", TRN)

                        cmd.Parameters.AddWithValue("@HIPAA_835_N1", N1)
                        cmd.Parameters.AddWithValue("@HIPAA_835_N3", N3)
                        cmd.Parameters.AddWithValue("@HIPAA_835_N4", N4)

                        cmd.Parameters.AddWithValue("@HIPAA_835_DTM", DTM)
                        cmd.Parameters.AddWithValue("@HIPAA_835_REF", REF)
                        cmd.Parameters.AddWithValue("@HIPAA_835_PER", PER)


                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using

                _RETURN_CODE = 0

            Catch ex As Exception
                _hasERR = True
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _SP_HEADERS_TEST, ex)
            End Try

            _RETURN_CODE = -1


            Try


                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_COMIT_CLP_TEST, Con)

                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.AddWithValue("@HIPAA_CACHE_835_ST", ST)
                        cmd.Parameters.AddWithValue("@HIPAA_CACHE_835_BPR", BPR)
                        cmd.Parameters.AddWithValue("@HIPAA_CACHE_835_TRN", TRN)
                        cmd.Parameters.AddWithValue("@HIPAA_CACHE_835_REF", REF)
                        cmd.Parameters.AddWithValue("@HIPAA_CACHE_835_DTM", DTM)
                        cmd.Parameters.AddWithValue("@HIPAA_CACHE_835_N1", N1)
                        cmd.Parameters.AddWithValue("@HIPAA_CACHE_835_N3", N3)
                        cmd.Parameters.AddWithValue("@HIPAA_CACHE_835_N4", N4)
                        cmd.Parameters.AddWithValue("@HIPAA_CACHE_835_PER", PER)


                        cmd.Parameters.AddWithValue("@HIPAA_835_LX", LX)
                        cmd.Parameters.AddWithValue("@HIPAA_835_TS2", TS2)
                        cmd.Parameters.AddWithValue("@HIPAA_835_TS3", TS3)

                        cmd.Parameters.AddWithValue("@HIPAA_835_PLB", PLB)


                        cmd.Parameters.AddWithValue("@HIPAA_835_CAS", M_CAS)
                        cmd.Parameters.AddWithValue("@HIPAA_835_REF", M_REF)
                        cmd.Parameters.AddWithValue("@HIPAA_835_AMT", M_AMT)
                        cmd.Parameters.AddWithValue("@HIPAA_835_DTM", M_DTM)
                        '







                        '' CLP SEGMENTS
                        cmd.Parameters.AddWithValue("@HIPAA_835_CLP_CLP", CLP_CLP)
                        cmd.Parameters.AddWithValue("@HIPAA_835_CLP_CAS", CLP_CAS)
                        cmd.Parameters.AddWithValue("@HIPAA_835_CLP_NM1", CLP_NM1)
                        cmd.Parameters.AddWithValue("@HIPAA_835_CLP_MIA", CLP_MIA)
                        cmd.Parameters.AddWithValue("@HIPAA_835_CLP_MOA", CLP_MOA)
                        cmd.Parameters.AddWithValue("@HIPAA_835_CLP_REF", CLP_REF)
                        cmd.Parameters.AddWithValue("@HIPAA_835_CLP_DTM", CLP_DTM)
                        cmd.Parameters.AddWithValue("@HIPAA_835_CLP_AMT", CLP_AMT)
                        cmd.Parameters.AddWithValue("@HIPAA_835_CLP_QTY", CLP_QTY)

                        ''SVC SEGMENTS
                        'cmd.Parameters.AddWithValue("@HIPAA_835_SVC_SVC", SVC_SVC)
                        'cmd.Parameters.AddWithValue("@HIPAA_835_SVC_DTM", SVC_DTM)
                        'cmd.Parameters.AddWithValue("@HIPAA_835_SVC_CAS", SVC_CAS)
                        'cmd.Parameters.AddWithValue("@HIPAA_835_SVC_REF", SVC_REF)
                        'cmd.Parameters.AddWithValue("@HIPAA_835_SVC_AMT", SVC_AMT)
                        'cmd.Parameters.AddWithValue("@HIPAA_835_SVC_LQ", SVC_LQ)



                        cmd.Parameters.AddWithValue("@HIPAA_835_TSH_GUID", _835_TSH_GUID)
                        cmd.Parameters.AddWithValue("@HIPAA_835_PAYOR_GUID", _T_835_PAYOR_GUID)
                        cmd.Parameters.AddWithValue("@HIPAA_835_PAYEE_GUID", _T_835_PAYEE_GUID)


                        cmd.Parameters.AddWithValue("@HIPAA_835_CLP_GUID", _835_CLP_GUID)
                        cmd.Parameters.AddWithValue("@HIPAA_835_LX_GUID", _835_LX_GUID)


                        cmd.Parameters.AddWithValue("@HIPAA_835_HEADER_STRING", _835_HEADER_STRING)
                        cmd.Parameters.AddWithValue("@HIPAA_835_LX_STRING", _835_LX_STRING)
                        cmd.Parameters.AddWithValue("@HIPAA_835_CLP_STRING", _835_CLP_STRING)
                        cmd.Parameters.AddWithValue("@HIPAA_835_SVC_STRING", _835_SVC_STRING)

                        cmd.Parameters.AddWithValue("@HIPAA_835_PLB_STRING", _835_PLB_STRING)


                        cmd.Parameters.AddWithValue("@FILE_ID", _FILE_ID)
                        cmd.Parameters.AddWithValue("@ISA_ID", _ISA_ID)
                        cmd.Parameters.AddWithValue("@GS_ID", _GS_ID)
                        cmd.Parameters.AddWithValue("@ST_ID", _ST_ID)

                        cmd.ExecuteNonQuery()


                    End Using
                    Con.Close()
                End Using

                _RETURN_CODE = 0
                _HL20_SP_TEST = True
            Catch ex As Exception
                _hasERR = True
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _SP_COMIT_CLP_TEST, ex)

            End Try





            Return _RETURN_CODE



        End Function


    End Class
End Namespace
