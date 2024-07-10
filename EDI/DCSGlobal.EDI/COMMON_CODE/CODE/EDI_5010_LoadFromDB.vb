Option Explicit On
Option Strict On
Option Compare Binary

Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Collections.Generic
Imports System.IO
Imports System.Text
Imports System.Threading
Imports DCSGlobal.BusinessRules.Logging
Imports DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff




Namespace DCSGlobal.EDI
    Public Class EDI_5010_LoadFromDB

        Inherits EDI_5010_COMMON_DECS

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



        Dim oSchedulerLog As New SchedulerLog
        Private _ClassVersion As String = "3.0"
        Private _DeadLockRetrys As Integer = 0
        Dim _Usp_eligibility_set_parse_status As String = String.Empty

        Dim _sqlGetREQRES As String = String.Empty
        ' Dim _USP_GET_REQRES As String = String.Empty





        Dim _USP_GET_ROWS As String = String.Empty
        Dim _sqlString As String = String.Empty

        Dim _REQ As String = String.Empty
        Dim _RES As String = String.Empty


        Public Sub New()
            If Debugger.IsAttached Then
                _VERBOSE = 1
                _DEBUG_LEVEL = 1

            End If

            _CONSOLE_NAME = "EDI_5010_LoadFromDB"
            _CLASS_NAME = "EDI_5010_LoadFromDB"

        End Sub
        Protected Overrides Sub Finalize()

            Dispose()
            '' Console.WriteLine("Object " & GetHashCode() & " finalized.")
        End Sub

        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposed Then
                If disposing Then

                    log = Nothing
                    ' oD = Nothing

                    ' Insert code to free managed resources. 
                End If

                ' Insert code to free unmanaged resources. 
            End If
            Me.disposed = True
        End Sub

        Public Property LOG_EDI As String

            Set(value As String)
                _LOG_EDI = value
            End Set
            Get
                Return _LOG_EDI
            End Get
        End Property



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

        Public WriteOnly Property DeadLockRetrys As Integer
            Set(value As Integer)
                _DeadLockRetrys = value
            End Set
        End Property


        Public WriteOnly Property Verbose As Integer
            Set(value As Integer)
                _Verbose = value
            End Set
        End Property

        Public WriteOnly Property SQLString As String

            Set(value As String)
                _sqlString = value

            End Set
        End Property


        Public WriteOnly Property SQLStringREQRES As String

            Set(value As String)
                _sqlGetREQRES = value

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



            Dim r As Integer = -1
            Dim schID As Integer = 0
            Dim _TransactionSetIdentifierCode As String = String.Empty
            Dim x As Integer = -1
            Dim _REQFound As Boolean = False
            Dim _REQValid As Boolean = False

            Dim _RESFound As Boolean = False
            Dim _RESValid As Boolean = False
            Try
                Try
                    If _VERBOSE = 1 Then
                        log.ExceptionDetails(" DCSGlobal.EDI.LoadFromDB", "Started")
                        Console.Write(vbNewLine + " DCSGlobal.EDI.LoadFromDB Started")
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
                                            _RES = Convert.ToString(rdrREQRES.Item("RESPONSE"))
                                            _REQ = Convert.ToString(rdrREQRES.Item("REQUEST"))
                                            _BATCH_ID = Convert.ToInt32(rdrREQRES("ID"))
                                            If _RES.Contains("ISA") Then

                                                _RESValid = True

                                            End If

                                            If _REQ.Contains("ISA") Then

                                                _REQValid = True

                                            End If
                                            If (_REQValid And _RESValid) Then



                                                Using vedi As New ValidateEDI()
                                                    x = vedi.byString(_REQ)
                                                    If x = 0 Then
                                                        _TransactionSetIdentifierCode = vedi.TransactionSetIdentifierCode
                                                    End If
                                                End Using




                                                Select Case _TransactionSetIdentifierCode
                                                    Case "276", "277"
                                                        'Process 277
                                                        r = Load276277Data()
                                                    Case "270", "271"
                                                        r = Load270271Data()
                                                    Case "278"
                                                        'Process 278
                                                    Case "835"
                                                        'Process 835
                                                    Case "837"
                                                        'Process 837
                                                    Case Else
                                                        log.ExceptionDetails("DCSGlobal.EDI.LoadFromDB.Go", "TransactionSetIdentifierCode Not found. TransactionSetIdentifierCode is: " + _TransactionSetIdentifierCode)
                                                End Select
                                            Else
                                                r = -2
                                                log.ExceptionDetails("DCSGlobal.EDI.LoadFromDB", " Batch ID:   " + Convert.ToString(_BATCH_ID) + " Ignored, becuase of Invalid REQ & RES. ")
                                                If _VERBOSE = 1 Then
                                                    Console.Write(vbNewLine + "DCSGlobal.EDI.LoadFromDB  Batch ID:   " + Convert.ToString(_BATCH_ID) + " Ignored, becuase of Invalid REQ & RES. ")
                                                End If
                                            End If
                                        Catch ex As System.Exception
                                            r = -3
                                            log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "DCSGlobal.EDI.LoadFromDB", ex)
                                            log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "DCSGlobal.EDI.LoadFromDB" + "|reg:" + _REQ + "|res:" + _RES, ex)

                                            '
                                        End Try
                                    Loop
                                End If
                            End Using
                        End Using
                    End Using
                    If r = 0 Then
                        If _VERBOSE = 1 Then
                            log.ExceptionDetails("DCSGlobal.EDI.LoadFromDB", "Completed")
                            Console.Write(vbNewLine + " DCSGlobal.EDI.LoadFromDB Completed")
                        End If
                    Else
                        log.ExceptionDetails("DCSGlobal.EDI.LoadFromDB", "Failed")
                        Console.Write(vbNewLine + " DCSGlobal.EDI.LoadFromDB Failed")
                    End If

                Catch ex As Exception
                    log.ExceptionDetails("DCSGlobal.EDI.LoadFromDB Add Scheduler log Failed ", ex)
                End Try
                r = 0
            Catch ex As Exception
                r = -1
            End Try
            Return r
        End Function

        Private Function Load276277Data() As Integer
            _FUNCTION_NAME = "Function Load276277Data() As Integer"

            Dim x As Integer = -1
            Try
                If _VERBOSE = 1 Then
                    log.ExceptionDetails("DCSGlobal.EDI.LoadFromDB.Load276277Data Started", "BatchID " + Convert.ToString(_BATCH_ID))
                    Console.Write(vbNewLine + "DCSGlobal.EDI.LoadFromDB.Load276277Data Started BatchID " + Convert.ToString(_BATCH_ID))
                End If
                Using _276277 As New EDI_5010_LoadPair276_277()
                    _276277.ConnectionString = _ConnectionString
                    _276277.SQLString = _sqlString
                    _276277.SQLStringREQRES = _sqlGetREQRES
                    _276277.BatchID = _BATCH_ID
                    _276277.DeadLockRetrys = _DeadLockRetrys
                    _276277.Verbose = _VERBOSE
                    _276277.CONSOLE_NAME = _CONSOLE_NAME
                    _276277.Go()
                End Using
                x = 0
                If _VERBOSE = 1 Then
                    log.ExceptionDetails("DCSGlobal.EDI.LoadFromDB.Load276277Data Completed", "BatchID " + Convert.ToString(_BATCH_ID))
                    Console.Write(vbNewLine + "DCSGlobal.EDI.LoadFromDB.Load276277Data Completed BatchID " + Convert.ToString(_BATCH_ID))
                End If
            Catch ex As Exception
                x = -2
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "DCSGlobal.EDI.LoadFromDB.Load276277Data Failed For Batch ID  " + Convert.ToString(_BATCH_ID), ex)
            End Try
            Return x
        End Function

        Private Function Load270271Data() As Integer

            _FUNCTION_NAME = "Function Load270271Data() As Integer"



            Dim x As Integer = -1
            Try
                If _VERBOSE = 1 Then


                    log.ExceptionDetails("DCSGlobal.EDI.LoadFromDB.Load276277Data Started", "BatchID " + Convert.ToString(_BATCH_ID))
                    Console.Write(vbNewLine + "DCSGlobal.EDI.LoadFromDB.Load276277Data Started BatchID " + Convert.ToString(_BATCH_ID))
                End If
                Using _270271 As New UCLA()
                    _270271.ConnectionString = _ConnectionString
                    _270271.SQLString = _sqlString
                    _270271.SQLStringREQRES = _sqlGetREQRES
                    _270271.BatchID = _BATCH_ID
                    _270271.DeadLockRetrys = _DeadLockRetrys
                    _270271.Verbose = _VERBOSE
                    _270271.Go()
                End Using
                x = 0
                If _VERBOSE = 1 Then
                    log.ExceptionDetails("DCSGlobal.EDI.LoadFromDB.Load276277Data Completed", "BatchID " + Convert.ToString(_BATCH_ID))
                    Console.Write(vbNewLine + "DCSGlobal.EDI.LoadFromDB.Load276277Data Completed BatchID " + Convert.ToString(_BATCH_ID))
                End If
            Catch ex As Exception
                x = -2

                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "DCSGlobal.EDI.LoadFromDB.Load_270_271_Data Failed For Batch ID  " + Convert.ToString(_BATCH_ID), ex)

            End Try
            Return x
        End Function

    End Class
End Namespace
