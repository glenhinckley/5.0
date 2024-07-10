﻿Option Explicit On
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
Imports DCSGlobal.BusinessRules.Logging
Imports System.IO
Imports System.Text
Imports System.Threading

Namespace DCSGlobal.EDI



    Public Class EDI_5010_LoadPair276_277





        Inherits EDI_5010_COMMON_DECS

        Implements IDisposable

        Protected disposed As Boolean = False

        '******************************************************************************************************************
        '  all vars not declared here are in   EDI_5010_COMMON_DECS   it is inhreted from the tables for this type
        '  if you are going to use it on this class only it goes below if not put it in EDI_5010_COMMON_DECS
        '******************************************************************************************************************


        Private log As New logExecption
        Private ss As New StringStuff



        Public _ConnectionString As String = String.Empty
        Public _CommandTimeOut As Integer = 90


        '   Private oD As New Declarations

        Private _ClassVersion As String = "3.0"

        Private start_time As DateTime
        Private stop_time As DateTime
        Private elapsed_time As TimeSpan




        Private _ENFlag As Boolean = False
        Private distinctDT As DataTable
        Private err As String = ""
        Private _sqlString As String = String.Empty

        Private STFlag As Integer = 0
        Private LXFlag As Integer = 0
        Private CLPFlag As Integer = 0
        Private FileID As Int32 = 0
        Private _DeadLockRetrys As Integer = 0

        Dim oSchedulerLog As New SchedulerLog
        Dim vEDI As New ValidateEDI


        Dim _sqlGetREQ As String = String.Empty
        Dim _sqlGetRES As String = String.Empty
        Dim _USP_GET_ROWS As String = String.Empty
        Dim _sqlGetREQRES As String = String.Empty
        Dim _USP_GET_REQRES As String = String.Empty
        Dim _Usp_eligibility_log_EDI_res As String = String.Empty

        Dim _dID As New Dictionary(Of Integer, Integer)

        Private disposedValue As Boolean ' To detect redundant calls

        Dim re As Integer




        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.  aafdsfdsf
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub


        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        Public Sub New()
            If Debugger.IsAttached Then
                _VERBOSE = 1
                _DEBUG_LEVEL = 1

            End If

            _CONSOLE_NAME = "EDI_5010_LoadPair276_277"
            _CLASS_NAME = "EDI_5010_LoadPair276_277"

        End Sub


        Protected Overrides Sub Finalize()

            log = Nothing





            '' em = Nothing
            Dispose()
            '   Console.WriteLine("Object " & GetHashCode() & " finalized.")
        End Sub


        Public WriteOnly Property ConnectionString As String

            Set(value As String)
                _ConnectionString = value
                log.ConnectionString = value

            End Set
        End Property

        Public WriteOnly Property Usp_eligibility_log_EDI_res As String
            Set(value As String)
                _Usp_eligibility_log_EDI_res = value
            End Set
        End Property
        Public WriteOnly Property DeadLockRetrys As Integer
            Set(value As Integer)
                _DeadLockRetrys = value
            End Set
        End Property

        Public WriteOnly Property Verbose As Integer
            Set(value As Integer)
                _VERBOSE = value
            End Set
        End Property

        Public WriteOnly Property USP_GET_REQRES As String
            Set(value As String)
                _USP_GET_REQRES = value
            End Set
        End Property
        Public WriteOnly Property SQLString As String

            Set(value As String)
                _sqlString = value

            End Set
        End Property

        Public WriteOnly Property USP_GET_ROWS As String
            Set(value As String)
                _USP_GET_ROWS = value
            End Set
        End Property



        Public WriteOnly Property SQLStringREQRES As String

            Set(value As String)
                _sqlGetREQRES = value

            End Set
        End Property





        Public WriteOnly Property SQLStringREQ As String

            Set(value As String)
                _sqlGetREQ = value

            End Set
        End Property


        Public WriteOnly Property SQLStringRES As String


            Set(value As String)
                _sqlGetRES = value

            End Set
        End Property



        Public WriteOnly Property BatchID As Long

            Set(value As Long)
                _BATCH_ID = value

            End Set
        End Property
        Public Property CONSOLE_NAME As String
            Get
                Return _CONSOLE_NAME
            End Get

            Set(value As String)
                _CONSOLE_NAME = value
            End Set
        End Property


        Public Function Go() As Int32
            _FUNCTION_NAME = "Function Go() As Int32"
            Dim cnt As Integer = 0
            Dim schId As Integer = 0
            Dim i As Integer = -1
            Dim param As New SqlParameter()

            Dim _id As Integer = 0
            Dim _REQ As String = String.Empty
            Dim _REQFound As Boolean = False
            Dim _REQValid As Boolean = False



            Dim _RES As String = String.Empty
            Dim _RESFound As Boolean = False
            Dim _RESValid As Boolean = False
            Dim x As Integer = 0

            Dim BatchId As Long = 0
            Dim bStatus As Boolean = False
            Dim start_time As DateTime
            Dim stop_time As DateTime


            Try

                Try
                    oSchedulerLog.ConnectionString = _ConnectionString
                    schId = oSchedulerLog.AddLogEntry("LoadPair276_277", "Started")
                Catch ex As Exception
                    log.ExceptionDetails("DCSGlobal.EDI.LoadPair276_277 Add Scheduler log Failed ", ex)
                End Try


                _sqlGetREQ = String.Empty
                _sqlGetRES = String.Empty
                _id = 0
                _REQ = String.Empty
                _RES = String.Empty
                _REQFound = False
                _RESFound = False
                _REQValid = False
                _RESValid = False


                If _VERBOSE = 1 Then
                    log.ExceptionDetails("LoadPair276_277", "Process start...    Batch ID  " + Convert.ToString(_BATCH_ID))
                    Console.Write(vbNewLine + "Process start...")
                    Console.Write(vbNewLine)
                End If



                Using ConREQRES As New SqlConnection(_ConnectionString)
                    ConREQRES.Open()
                    Using cmdREQRES As New SqlCommand(_sqlGetREQRES, ConREQRES)

                        cmdREQRES.CommandType = CommandType.StoredProcedure
                        cmdREQRES.Parameters.AddWithValue("@ID", _BATCH_ID)


                        Using rdrREQRES = cmdREQRES.ExecuteReader()
                            If rdrREQRES.HasRows Then
                                Do While rdrREQRES.Read
                                    Try

                                        _RESFound = True
                                        _RES = Convert.ToString(rdrREQRES.Item("RESPONSE"))
                                        _REQ = Convert.ToString(rdrREQRES.Item("REQUEST"))



                                        If Not IsDBNull(rdrREQRES("source")) Then
                                            _SOURCE = Convert.ToString(rdrREQRES("source"))

                                        Else
                                            'Failure
                                        End If


                                        '
                                        If Not IsDBNull(rdrREQRES("payor_id")) Then
                                            _PAYOR_ID = Convert.ToString(rdrREQRES("payor_id"))

                                        Else
                                            'Failure
                                        End If

                                        If Not IsDBNull(rdrREQRES("ins_type")) Then
                                            _INS_TYPE = Convert.ToString(rdrREQRES("ins_type"))

                                        Else
                                            'Failure
                                        End If

                                        If Not IsDBNull(rdrREQRES("user_id")) Then
                                            _USER_ID = Convert.ToString(rdrREQRES("user_id"))


                                        Else
                                            'Failure
                                        End If

                                        If Not IsDBNull(rdrREQRES("pat_hosp_code")) Then
                                            _PAT_HOSP_CODE = Convert.ToString(rdrREQRES("pat_hosp_code"))

                                        Else
                                            'Failure
                                        End If

                                        If Not IsDBNull(rdrREQRES("patient_number")) Then
                                            _PATIENT_NUMBER = Convert.ToString(rdrREQRES("patient_number"))

                                        Else
                                            'Failure
                                        End If

                                        If Not IsDBNull(rdrREQRES("hosp_code")) Then
                                            _HOSP_CODE = Convert.ToString(rdrREQRES("hosp_code"))

                                        Else
                                            'Failure
                                        End If

                                        If Not IsDBNull(rdrREQRES("ebr_id")) Then
                                            _EBR_ID = CLng(Convert.ToDouble(rdrREQRES("ebr_id")))
                                        Else
                                            'Failure
                                        End If

                                        If Not IsDBNull(rdrREQRES("vendor_name")) Then
                                            _VENDOR_NAME = Convert.ToString(rdrREQRES("vendor_name"))

                                        Else
                                            'Failure
                                        End If

                                        ' check 270 and 271 are valid

                                        If _RES.Contains("ISA") Then

                                            _RESValid = True

                                        End If

                                        If _REQ.Contains("ISA") Then

                                            _REQValid = True

                                        End If

                                        ' update I
                                        If (_REQValid And _RESValid) Then
                                            start_time = Now
                                            If _VERBOSE = 1 Then
                                                log.ExceptionDetails("LoadPair276_277", " REQ & RES is valid and process starts  for the Batch ID:   " + Convert.ToString(_BATCH_ID))
                                                Console.Write(vbNewLine + " Batch ID:   " + Convert.ToString(_BATCH_ID) + " Process Started")

                                            End If
                                            Using _276 As New EDI_5010_276_005010X212_DCS   '   Import276

                                                Try
                                                    _276.ConnectionString = _ConnectionString
                                                    _276.source = _SOURCE
                                                    _276.cbr_id = _EBR_ID
                                                    _276.user_id = _USER_ID
                                                    _276.hosp_code = _HOSP_CODE
                                                    _276.PAYOR_ID = _PAYOR_ID
                                                    _276.Vendor_name = _VENDOR_NAME
                                                    _276.pat_hosp_code = _PAT_HOSP_CODE
                                                    _276.Patient_number = _PATIENT_NUMBER
                                                    _276.ins_type = _INS_TYPE
                                                    _276.Verbose = _VERBOSE
                                                    _276.EDI = _REQ
                                                    _276.BatchID = _BATCH_ID
                                                    _BATCH_ID = _276.ImportByString(_REQ, _BATCH_ID)

                                                Catch ex As Exception
                                                    log.ExceptionDetails("LoadPair276_277 276 comiit fail", ex)
                                                End Try
                                            End Using


                                            Using _277 As New EDI_5010_277_005010X212_v2

                                                Try
                                                    _277.ConnectionString = _ConnectionString
                                                    _277.DeleteFlag = "N"
                                                    _277.ebr_id = _EBR_ID
                                                    _277.user_id = _USER_ID
                                                    _277.hosp_code = _HOSP_CODE
                                                    _277.PAYOR_ID = _PAYOR_ID
                                                    _277.Vendor_name = _VENDOR_NAME
                                                    _277.BatchID = _BATCH_ID
                                                    _277.source = "277" 'oD.source
                                                    _277.EDI = _RES
                                                    _277.Verbose = _VERBOSE
                                                    _277.CONSOLE_NAME = _CONSOLE_NAME
                                                    _BATCH_ID = _277.ImportByString(_RES, _BATCH_ID)
                                                    stop_time = Now
                                                    Using SSP As New SetProcessStatusFlag
                                                        SSP.ConnectionString = _ConnectionString
                                                        SSP.SP_SET_PROCESS_STATUS = _sqlString
                                                        SSP.SetFlag("Y", Convert.ToInt32(_BATCH_ID))
                                                        cnt = cnt + 1
                                                    End Using
                                                Catch ex As Exception

                                                    log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "LoadPair276_277 277 comiit fail", ex)

                                                End Try
                                            End Using



                                            If _VERBOSE = 1 Then
                                                log.ExceptionDetails("LoadPair276_277", " Process Completed  for the Batch ID :   " + Convert.ToString(_BATCH_ID))
                                                Console.Write(vbNewLine + " Batch ID:   " + Convert.ToString(_BATCH_ID) + "Process Completed")
                                                Console.Write(vbNewLine)
                                            End If

                                        Else
                                            '
                                            If _VERBOSE = 1 Then
                                                log.ExceptionDetails("LoadPair276_277", " Batch ID:   " + Convert.ToString(_BATCH_ID) + " Ignored, becuase of Invalid REQ & RES. ")
                                                Console.Write(vbNewLine + " Batch ID:   " + Convert.ToString(_BATCH_ID) + " Ignored, becuase of Invalid REQ & RES. ")
                                            End If

                                        End If

                                    Catch ex As System.Exception
                                        i = -1
                                        Using SSP As New SetProcessStatusFlag
                                            SSP.ConnectionString = _ConnectionString
                                            SSP.SP_SET_PROCESS_STATUS = _sqlString
                                            ' SSP.ProcessStartTime = start_time
                                            '  SSP.ProcessEndTime = Now
                                            SSP.SetFlag("X", Convert.ToInt32(_BATCH_ID))
                                            ' 
                                            log.ExceptionDetails("Childrens", "Flag updated to X. Issue with the Batch ID  " + Convert.ToString(_BATCH_ID))
                                            If _VERBOSE = 1 Then

                                                Console.Write(vbNewLine + " Flag updated to X. Issue with the Batch ID " + Convert.ToString(_BATCH_ID))
                                                Console.Write(vbNewLine)
                                            End If
                                            '
                                        End Using
                                        '
                                    End Try
                                Loop
                            End If
                        End Using
                    End Using
                End Using




                Try
                    'oSchedulerLog.ConnectionString = oD._ConnectionString
                    schId = oSchedulerLog.UpdateLogEntry("LoadPair276_277", "Completed", schId, x, cnt)
                Catch ex As Exception
                    log.ExceptionDetails("DCSGlobal.EDI.Childrens Update Scheduler log Failed ", ex)
                End Try
                '
                If _VERBOSE = 1 Then
                    log.ExceptionDetails("Childrens", "Process End...    Batch ID  " + Convert.ToString(_BATCH_ID))

                    Console.Write(vbNewLine + "Process End...")
                    Console.Write(vbNewLine)
                End If
                'Else
                ''
                'If _Verbose = 1 Then
                '    Console.Write(vbNewLine)
                '    Console.Write(vbNewLine + "Waiting for next Batch Process...")
                '    Console.Write(vbNewLine)
                'End If
                ''
                'End If

                i = 0

                '

            Catch ex As System.Exception
                log.ExceptionDetails("DCSGlobal.EDI.Childrens.GO Failed " + _BATCH_ID.ToString, ex)

            Finally

            End Try

            Return i


        End Function



    End Class

End Namespace
