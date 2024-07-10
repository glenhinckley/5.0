Option Explicit On
Option Strict On



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



    Public Class UCLA
        Implements IDisposable


        Private _ClassVersion As String = "3.0"

        Private start_time As DateTime
        Private stop_time As DateTime
        Private elapsed_time As TimeSpan
        Private oD As New Declarations
        Private objss As New StringStuff
        Private _TablesBuilt As Boolean = False
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
        Dim _270 As New Import270
        Dim _271 As New Import271
        Dim _Verbose As Integer = 0
        Dim _sqlGetREQ As String = String.Empty
        Dim _sqlGetRES As String = String.Empty
        Dim _USP_GET_ROWS As String = String.Empty
        Dim _sqlGetREQRES As String = String.Empty
        Dim _USP_GET_REQRES As String = String.Empty
        Dim _Usp_eligibility_log_EDI_res As String = String.Empty
        Private log As New logExecption
        Dim _dID As New Dictionary(Of Integer, Integer)

        Private disposedValue As Boolean ' To detect redundant calls

        Dim re As Integer
        Dim _BatchID As Long = 0



        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub


        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub




        Protected Overrides Sub Finalize()

            log = Nothing
            _270 = Nothing
            _271 = Nothing
            oD = Nothing



            '' em = Nothing
            Dispose()
            '   Console.WriteLine("Object " & GetHashCode() & " finalized.")
        End Sub


        Public WriteOnly Property ConnectionString As String

            Set(value As String)
                oD._ConnectionString = value
                log.ConnectionString = value
                _270.ConnectionString = value
                _271.ConnectionString = value
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
                _Verbose = value
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
                _BatchID = value

            End Set
        End Property



        Public Function Go() As Int32

            Dim cnt As Integer = 0
            Dim schId As Integer = 0
            Dim i As Integer
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



                ' build a dictonary from 

                'Using Con As New SqlConnection(oD._ConnectionString)
                '    Con.Open()
                '    Using cmd As New SqlCommand(_USP_GET_ROWS, Con)


                '        cmd.CommandType = CommandType.StoredProcedure

                '        Using RDR = cmd.ExecuteReader()
                '            If RDR.HasRows Then
                '                '
                '                If _Verbose = 1 Then
                '                    Console.Write(vbNewLine + "Process Started...")
                '                End If

                '                Do While RDR.Read

                '                    _id = Convert.ToInt32(RDR.Item("id"))
                '                    Using SSP As New SetProcessStatusFlag
                '                        SSP.ConnectionString = oD._ConnectionString
                '                        SSP.SP_SET_PROCESS_STATUS = _sqlString
                '                        SSP.SetFlag("I", _id)
                '                    End Using
                '                    _dID.Add(x, _id)
                '                    x = x + 1
                '                Loop
                '            End If
                '        End Using

                '        Con.Close()
                '    End Using

                'End Using




                'If x > 0 Then

                Try
                    oSchedulerLog.ConnectionString = oD._ConnectionString
                    schId = oSchedulerLog.AddLogEntry("UCLAConsole", "Console Applicaiton")
                Catch ex As Exception
                    log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.UCLA Add Scheduler log Failed ", ex)
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


                If _Verbose = 1 Then
                    log.ExceptionDetails("UCLA", "Process start...    Batch ID  " + Convert.ToString(_BatchID))

                    Console.Write(vbNewLine + "Process start...")
                    Console.Write(vbNewLine)
                End If



                Using ConREQRES As New SqlConnection(oD._ConnectionString)
                    ConREQRES.Open()
                    Using cmdREQRES As New SqlCommand(_sqlGetREQRES, ConREQRES)

                        cmdREQRES.CommandType = CommandType.StoredProcedure
                        cmdREQRES.Parameters.AddWithValue("@ID", _BatchID)


                        Using rdrREQRES = cmdREQRES.ExecuteReader()
                            If rdrREQRES.HasRows Then
                                Do While rdrREQRES.Read
                                    Try

                                        _RESFound = True
                                        _RES = Convert.ToString(rdrREQRES.Item("RESPONSE"))
                                        _REQ = Convert.ToString(rdrREQRES.Item("REQUEST"))

                                        oD.Clear()

                                        If Not IsDBNull(rdrREQRES("source")) Then
                                            oD.source = Convert.ToString(rdrREQRES("source"))

                                        Else
                                            'Failure
                                        End If


                                        '
                                        If Not IsDBNull(rdrREQRES("payor_id")) Then
                                            oD.PAYOR_ID = Convert.ToString(rdrREQRES("payor_id"))

                                        Else
                                            'Failure
                                        End If

                                        If Not IsDBNull(rdrREQRES("ins_type")) Then
                                            oD.ins_type = Convert.ToString(rdrREQRES("ins_type"))

                                        Else
                                            'Failure
                                        End If

                                        If Not IsDBNull(rdrREQRES("user_id")) Then
                                            oD.user_id = Convert.ToString(rdrREQRES("user_id"))


                                        Else
                                            'Failure
                                        End If

                                        If Not IsDBNull(rdrREQRES("pat_hosp_code")) Then
                                            oD.pat_hosp_code = Convert.ToString(rdrREQRES("pat_hosp_code"))

                                        Else
                                            'Failure
                                        End If

                                        If Not IsDBNull(rdrREQRES("patient_number")) Then
                                            oD.Patient_number = Convert.ToString(rdrREQRES("patient_number"))

                                        Else
                                            'Failure
                                        End If

                                        If Not IsDBNull(rdrREQRES("hosp_code")) Then
                                            oD.hosp_code = Convert.ToString(rdrREQRES("hosp_code"))

                                        Else
                                            'Failure
                                        End If

                                        If Not IsDBNull(rdrREQRES("ebr_id")) Then
                                            oD.ebr_id = Convert.ToDouble(rdrREQRES("ebr_id"))
                                        Else
                                            'Failure
                                        End If

                                        If Not IsDBNull(rdrREQRES("vendor_name")) Then
                                            oD.Vendor_name = Convert.ToString(rdrREQRES("vendor_name"))

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
                                            If _Verbose = 1 Then

                                                log.ExceptionDetails("UCLA", " Batch ID:   " + Convert.ToString(_BatchID) + " Started ")
                                                Console.Write(vbNewLine + " Batch ID:   " + Convert.ToString(_BatchID) + " Started ")
                                            End If

                                            Try
                                                'import 270
                                                _270.EDI = _REQ
                                                _270.source = oD.source
                                                _270.ebr_id = oD.ebr_id
                                                _270.user_id = oD.user_id
                                                _270.hosp_code = oD.hosp_code
                                                _270.PAYOR_ID = oD.PAYOR_ID
                                                _270.Vendor_name = oD.Vendor_name
                                                _270.pat_hosp_code = oD.pat_hosp_code
                                                _270.Patient_number = oD.Patient_number
                                                _270.ins_type = oD.ins_type
                                                _270.BatchID = Convert.ToDouble(_BatchID)
                                                _270.Import()
                                                oD._BatchID = _270.Comit()
                                            Catch ex As Exception
                                                log.ExceptionDetails("UCLA 270 comiit fail", ex)
                                            End Try
                                            Try
                                                'import 271
                                                _271.EDI = _RES
                                                _271.RAW270 = _REQ
                                                _271.DoAuditLog = False
                                                _271.DeleteFlag = "N"
                                                _271.ebr_id = oD.ebr_id
                                                _271.user_id = oD.user_id
                                                _271.hosp_code = oD.hosp_code
                                                _271.PAYOR_ID = oD.PAYOR_ID
                                                _271.Vendor_name = oD.Vendor_name
                                                _271.ServiceTypeCode = oD.ServiceTypeCode
                                                _271.BatchID = oD._BatchID
                                                _271.source = oD.source
                                                _271.Import()
                                                _271.DeadlockRetrys = _DeadLockRetrys
                                                _271.Comit()
                                                stop_time = Now
                                                Using SSP As New SetProcessStatusFlag
                                                    SSP.ConnectionString = oD._ConnectionString
                                                    SSP.SP_SET_PROCESS_STATUS = _sqlString
                                                    '  SSP.ProcessStartTime = start_time
                                                    '  SSP.ProcessEndTime = stop_time
                                                    'SSP.SetFlag("P", pair.Value)
                                                    SSP.SetFlag("Y", Convert.ToInt32(_BatchID))
                                                    cnt = cnt + 1
                                                End Using
                                            Catch ex As Exception
                                                log.ExceptionDetails("UCLA 271 comiit fail", ex)
                                            End Try





                                            '
                                            If _Verbose = 1 Then
                                                log.ExceptionDetails("UCLA", " Batch ID:   " + Convert.ToString(_BatchID) + " Completed ")
                                                Console.Write(vbNewLine + " Batch ID:   " + Convert.ToString(_BatchID) + " Completed")
                                                Console.Write(vbNewLine)
                                            End If

                                        Else
                                            '
                                            If _Verbose = 1 Then
                                                log.ExceptionDetails("UCLA", " Batch ID:   " + Convert.ToString(_BatchID) + " Ignored, becuase of Invalid REQ & RES. ")
                                                Console.Write(vbNewLine + " Batch ID:   " + Convert.ToString(_BatchID) + " Ignored, becuase of Invalid REQ & RES. ")
                                            End If

                                        End If
                                    Catch ex As System.Exception
                                        i = -1
                                        Using SSP As New SetProcessStatusFlag
                                            SSP.ConnectionString = oD._ConnectionString
                                            SSP.SP_SET_PROCESS_STATUS = _sqlString
                                            ' SSP.ProcessStartTime = start_time
                                            '  SSP.ProcessEndTime = Now
                                            SSP.SetFlag("X", Convert.ToInt32(_BatchID))
                                            ' 
                                            log.ExceptionDetails("UCLA", ex)
                                            log.ExceptionDetails("UCLA", "Flag updated to X. Issue with the Batch ID  " + Convert.ToString(_BatchID))
                                            If _Verbose = 1 Then

                                                Console.Write(vbNewLine + " Flag updated to X. Issue with the Batch ID " + Convert.ToString(_BatchID))
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
                    schId = oSchedulerLog.UpdateLogEntry("UCLAConsole", "Console Applicaiton", schId, x, cnt)
                Catch ex As Exception
                    log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.UCLA Update Scheduler log Failed ", ex)
                End Try
                '
                If _Verbose = 1 Then
                    log.ExceptionDetails("UCLA", "Process End...    Batch ID  " + Convert.ToString(_BatchID))

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
                log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.UCLA.GO Failed " + oD._BatchID.ToString, ex)

            Finally

            End Try

            Return i


        End Function



    End Class

End Namespace

