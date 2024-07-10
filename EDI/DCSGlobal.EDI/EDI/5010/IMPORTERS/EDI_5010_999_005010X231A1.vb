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

    Public Class EDI_5010_999_005010X231A1

        Inherits EDI_5010_999_00510X231A1_TABLES


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

        Private _DocumentType As String = "005010X231A1"




        Private _SP_999_DUMP As String = "[usp_EDI_5010_HIPAA_999_DUMP]"
        Private _SP_COMIT_UNKNOWN As String = "[usp_HIPAA_EDI_UNKNOWN]"

        Private _999_AK2_GUID As Guid = Guid.Empty
        Private _999_IK3_GUID As Guid = Guid.Empty
        Private _999_IK4_GUID As Guid = Guid.Empty





        Public Sub New()
            If Debugger.IsAttached Then
                _VERBOSE = 1
                _DEBUG_LEVEL = 1

            End If

            _CONSOLE_NAME = "EDI_5010_999_005010X231A1"
            _CLASS_NAME = "EDI_5010_999_005010X231A1"

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


        Public WriteOnly Property ConnectionString As String

            Set(value As String)
                _ConnectionString = value
                log.ConnectionString = value
            End Set
        End Property

        Public WriteOnly Property SP_999_DUMP As String

            Set(value As String)
                _SP_999_DUMP = value
            End Set
        End Property

        Public WriteOnly Property SP_COMIT_UNKNOWN As String

            Set(value As String)
                _SP_COMIT_UNKNOWN = value
            End Set
        End Property


        Public ReadOnly Property IMPORT_RETURN_STRING As String

            Get
                Return _IMPORT_RETURN_STRING
            End Get


        End Property

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


            '    _SP_RETURN_CODE = TestSPs()

            If (_SP_RETURN_CODE <> 0) Then
                _IMPORT_RETURN_STRING = "999 : SP TEST FAILED WITH ERROR CODE " + Convert.ToString(_SP_RETURN_CODE)
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _IMPORT_RETURN_STRING)
                Return -1
                Exit Function
            End If


            Using e As New EDI_5010_LOGGING
                e.ConnectionString = _ConnectionString
                e.TransactionSetIdentifierCode = "999"
                e.UpdateFileStatus(CInt(_FILE_ID), "999", "BEGIN PARSE")
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

                        'ComitISA()


                        'Select Case _999_LOOP_LEVEL
                        '    Case "HEADER"
                        '        _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                        'End Select

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

                        'ComitGS()


                        'Select Case _999_LOOP_LEVEL
                        '    Case "HEADER"
                        '        _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                        'End Select

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



                        '_ST = Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"

                        'Select Case _999_LOOP_LEVEL
                        '    Case "HEADER"
                        '        _RAW_HEADER = _RAW_HEADER + Convert.ToString(rowcount) + "::" + _CurrentRowData + "~"
                        'End Select
                        '   ComitST()
                    End If

                    ' all the rows get made in to a string. 

                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                End Try






                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '  BEGIN 999
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '  AK1
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "AK1" Then

                        _RowProcessedFlag = 1



                        Dim AK1Row As DataRow = AK1.NewRow
                        AK1Row("DOCUMENT_ID") = _DOCUMENT_ID
                        AK1Row("FILE_ID") = _FILE_ID
                        AK1Row("BATCH_ID") = _BATCH_ID
                        AK1Row("ISA_ID") = _ISA_ID
                        AK1Row("GS_ID") = _GS_ID
                        AK1Row("ST_ID") = _ST_ID
                        AK1Row("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                        AK1Row("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                        AK1Row("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                        AK1Row("_999_AK2_GUID") = _999_AK2_GUID
                        AK1Row("_999_IK3_GUID") = _999_IK3_GUID
                        AK1Row("_999_IK4_GUID") = _999_IK4_GUID



                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then AK1Row("AK101") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else AK1Row("AK101") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then AK1Row("AK102") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else AK1Row("AK102") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then AK1Row("AK103") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else AK1Row("AK103") = DBNull.Value


                        AK1.Rows.Add(AK1Row)

                    End If

                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                End Try


                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '  AK2
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "AK2" Then

                        _RowProcessedFlag = 1

                        _999_AK2_GUID = Guid.NewGuid


                        Dim AK2Row As DataRow = AK2.NewRow
                        AK2Row("DOCUMENT_ID") = _DOCUMENT_ID
                        AK2Row("FILE_ID") = _FILE_ID
                        AK2Row("BATCH_ID") = _BATCH_ID
                        AK2Row("ISA_ID") = _ISA_ID
                        AK2Row("GS_ID") = _GS_ID
                        AK2Row("ST_ID") = _ST_ID
                        AK2Row("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                        AK2Row("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                        AK2Row("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                        AK2Row("_999_AK2_GUID") = _999_AK2_GUID
                        AK2Row("_999_IK3_GUID") = _999_IK3_GUID
                        AK2Row("_999_IK4_GUID") = _999_IK4_GUID



                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then AK2Row("AK201") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else AK2Row("AK201") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then AK2Row("AK202") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else AK2Row("AK202") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then AK2Row("AK203") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else AK2Row("AK203") = DBNull.Value


                        AK2.Rows.Add(AK2Row)

                    End If

                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                End Try




                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '  IK3
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "IK3" Then

                        _RowProcessedFlag = 1

                        _999_IK3_GUID = Guid.NewGuid


                        Dim IK3Row As DataRow = IK3.NewRow
                        IK3Row("DOCUMENT_ID") = _DOCUMENT_ID
                        IK3Row("FILE_ID") = _FILE_ID
                        IK3Row("BATCH_ID") = _BATCH_ID
                        IK3Row("ISA_ID") = _ISA_ID
                        IK3Row("GS_ID") = _GS_ID
                        IK3Row("ST_ID") = _ST_ID
                        IK3Row("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                        IK3Row("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                        IK3Row("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                        IK3Row("_999_AK2_GUID") = _999_AK2_GUID
                        IK3Row("_999_IK3_GUID") = _999_IK3_GUID
                        IK3Row("_999_IK4_GUID") = _999_IK4_GUID



                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then IK3Row("IK301") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else IK3Row("IK301") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then IK3Row("IK302") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else IK3Row("IK302") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then IK3Row("IK303") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else IK3Row("IK303") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then IK3Row("IK304") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else IK3Row("IK304") = DBNull.Value




                        IK3.Rows.Add(IK3Row)

                    End If

                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                End Try


                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '  IK4
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "IK4" Then

                        _RowProcessedFlag = 1

                        _999_IK4_GUID = Guid.NewGuid

                        Dim _IK401 As String = String.Empty
            
                        Dim IK4Row As DataRow = IK4.NewRow
                        IK4Row("DOCUMENT_ID") = _DOCUMENT_ID
                        IK4Row("FILE_ID") = _FILE_ID
                        IK4Row("BATCH_ID") = _BATCH_ID
                        IK4Row("ISA_ID") = _ISA_ID
                        IK4Row("GS_ID") = _GS_ID
                        IK4Row("ST_ID") = _ST_ID
                        IK4Row("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                        IK4Row("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                        IK4Row("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                        IK4Row("_999_AK2_GUID") = _999_AK2_GUID
                        IK4Row("_999_IK3_GUID") = _999_IK3_GUID
                        IK4Row("_999_IK4_GUID") = _999_IK4_GUID



                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then IK4Row("IK401") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else IK4Row("IK401") = DBNull.Value






                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then IK4Row("IK402") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else IK4Row("IK402") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then IK4Row("IK403") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else IK4Row("IK403") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then IK4Row("IK404") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else IK4Row("IK404") = DBNull.Value




                        IK4.Rows.Add(IK4Row)

                    End If

                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                End Try







                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '  AK9
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    If _RowRecordType = "AK9" Then

                        _RowProcessedFlag = 1


                        _999_AK2_GUID = Guid.Empty
                        _999_IK3_GUID = Guid.Empty
                        _999_IK4_GUID = Guid.Empty


                        Dim AK9Row As DataRow = AK9.NewRow
                        AK9Row("DOCUMENT_ID") = _DOCUMENT_ID
                        AK9Row("FILE_ID") = _FILE_ID
                        AK9Row("BATCH_ID") = _BATCH_ID
                        AK9Row("ISA_ID") = _ISA_ID
                        AK9Row("GS_ID") = _GS_ID
                        AK9Row("ST_ID") = _ST_ID
                        AK9Row("HIPAA_ISA_GUID") = _HIPAA_ISA_GUID
                        AK9Row("HIPAA_GS_GUID") = _HIPAA_GS_GUID
                        AK9Row("HIPAA_ST_GUID") = _HIPAA_ST_GUID
                        AK9Row("_999_AK2_GUID") = _999_AK2_GUID
                        AK9Row("_999_IK3_GUID") = _999_IK3_GUID
                        AK9Row("_999_IK4_GUID") = _999_IK4_GUID



                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) <> "") Then AK9Row("AK901") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 2) Else AK9Row("AK901") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) <> "") Then AK9Row("AK902") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 3) Else AK9Row("AK902") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) <> "") Then AK9Row("AK903") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 4) Else AK9Row("AK903") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) <> "") Then AK9Row("AK904") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 5) Else AK9Row("AK904") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) <> "") Then AK9Row("AK905") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 6) Else AK9Row("AK905") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) <> "") Then AK9Row("AK907") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 7) Else AK9Row("AK906") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) <> "") Then AK9Row("AK908") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 8) Else AK9Row("AK907") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) <> "") Then AK9Row("AK909") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 9) Else AK9Row("AK908") = DBNull.Value
                        If (ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) <> "") Then AK9Row("AK909") = ss.ParseDemlimtedString(_CurrentRowData, _DataElementSeparator, 10) Else AK9Row("AK909") = DBNull.Value


                        AK9.Rows.Add(AK9Row)

                    End If

                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                End Try


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






                    End If

                Catch ex As Exception
                    _RowProcessedFlag = -2
                    _hasERR = True
                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _RowRecordType, ex)
                End Try


            Next


            If _hasUNK Then
                Using e As New EDI_5010_LOGGING
                    e.ConnectionString = _ConnectionString
                    e.TransactionSetIdentifierCode = "999"
                    If _hasERR Then
                        e.UpdateFileStatus(CInt(_FILE_ID), "999", "PARSE COMPLETE WITH UNK SEGMENTS WITH ERRORS")
                    Else
                        e.UpdateFileStatus(CInt(_FILE_ID), "999", "PARSE COMPLETE WITH UNK SEGMENTS")
                    End If



                End Using

                ComitUNK()

            Else

                Using e As New EDI_5010_LOGGING
                    e.ConnectionString = _ConnectionString
                    e.TransactionSetIdentifierCode = "999"
                    If _hasERR Then
                        e.UpdateFileStatus(CInt(_FILE_ID), "999", "PARSE COMPLETE WITH ERRORS")
                    Else
                        e.UpdateFileStatus(CInt(_FILE_ID), "999", "PARSE COMPLETE")
                    End If
                End Using

            End If




            _ProcessEndTime = DateTime.Now

            Return _ImportReturnCode

        End Function
        Public Function CountCharacter(ByVal value As String, ByVal ch As Char) As Integer
            Return value.Count(Function(c As Char) c = ch)
        End Function




        Public Function COMIT_999_Dump() As Integer

            Dim r As Integer = -1

            _FUNCTION_NAME = "COMIT_999_Dump()"


            Try


                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_999_DUMP, Con)


                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.AddWithValue("@HIPAA_999_ISA", ISA)
                        cmd.Parameters.AddWithValue("@HIPAA_999_GS", GS)
                        cmd.Parameters.AddWithValue("@HIPAA_999_ST", ST)

                        cmd.Parameters.AddWithValue("@HIPAA_999_AK1", AK1)
                        cmd.Parameters.AddWithValue("@HIPAA_999_AK2", AK2)
                        cmd.Parameters.AddWithValue("@HIPAA_999_AK9", AK9)


                        cmd.Parameters.AddWithValue("@HIPAA_999_CTX", CTX)


                        cmd.Parameters.AddWithValue("@HIPAA_999_IK3", IK3)
                        cmd.Parameters.AddWithValue("@HIPAA_999_IK3", IK4)
                        cmd.Parameters.AddWithValue("@HIPAA_999_IK3", IK5)
    


                        cmd.Parameters.AddWithValue("@HIPAA_999_SE", SE)
                        cmd.Parameters.AddWithValue("@HIPAA_999_GE", GE)
                        cmd.Parameters.AddWithValue("@HIPAA_999_IEA", IEA)







                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using


                r = 0

            Catch ex As Exception
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, _SP_999_DUMP, ex)

            End Try





            Return r



        End Function
        Private Sub ClearISA()
            ISA.Clear()
            IEA.Clear()
            _HIPAA_ISA_GUID = Guid.Empty
            _ISA_ID = 0


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

            _ST_ID = 0
            _HIPAA_ST_GUID = Guid.Empty



        End Sub


        Private Sub ClearDUMP()

            AK1.Clear()
            AK2.Clear()
            AK9.Clear()

            CTX.Clear()

            IK3.Clear()
            IK4.Clear()
            IK5.Clear()


        End Sub




        Private Function ComitUNK() As Integer


            Dim i As Integer

            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_COMIT_UNKNOWN, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@HIPAA_5010_UNK", UNK)
                        cmd.Parameters.AddWithValue("@IMPORTER", "EDI_5010_999_005010X231A1")
                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using

                i = 0

            Catch ex As Exception
                _hasERR = True
                _ERROR = ex.Message
                log.ExceptionDetails("COMIT_UNK_DATA 005010X231A1", ex)
            End Try

            Return i

        End Function



    End Class
End Namespace
