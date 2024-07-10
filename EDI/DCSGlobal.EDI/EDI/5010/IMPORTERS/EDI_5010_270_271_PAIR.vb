
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










    Public Class EDI_5010_270_271_PAIR




        Implements IDisposable





        Protected disposed As Boolean = False
        '******************************************************************************************************************
        '  all vars not declared here are in   EDI_5010_COMMON_DECS   it is inhreted from the tables for this type
        '  if you are going to use it on this class only it goes below if not put it in EDI_5010_COMMON_DECS
        '******************************************************************************************************************


        Private log As New logExecption
        Private ss As New StringStuff
        Private d As New DebugOUT


        Private _ConnectionString As String = String.Empty



        Private _EBR_ID As Long = 0

        Private _RES As String = String.Empty
        Private _REQ As String = String.Empty


        Private _SOURCE As String = String.Empty
        Private _PAYOR_ID As String = String.Empty
        Private _INS_TYPE As String = String.Empty
        Private _USER_ID As String = String.Empty
        Private _PAT_HOSP_CODE As String = String.Empty
        Private _PATIENT_NUMBER As String = String.Empty
        Private _HOSP_CODE As String = String.Empty
        Private _VENDOR_NAME As String = String.Empty

        Private _SetProcessStatusFlag As String = String.Empty

        'Private _ClassVersion As String = "3.0"

        'Private start_time As DateTime
        'Private stop_time As DateTime
        'Private elapsed_time As TimeSpan


        'Private _ENFlag As Boolean = False
        'Private distinctDT As DataTable
        'Private err As String = ""
        'Private _sqlString As String = String.Empty

        'Private STFlag As Integer = 0
        'Private LXFlag As Integer = 0
        'Private CLPFlag As Integer = 0
        'Private FileID As Int32 = 0
        'Private _DeadLockRetrys As Integer = 0







        'Dim _sqlGetREQ As String = String.Empty
        'Dim _sqlGetRES As String = String.Empty
        'Dim _USP_GET_ROWS As String = String.Empty
        'Dim _sqlGetREQRES As String = String.Empty
        'Dim _USP_GET_REQRES As String = String.Empty
        'Dim _Usp_eligibility_log_EDI_res As String = String.Empty


        'Private _dID As New Dictionary(Of Integer, Integer)

        Private disposedValue As Boolean ' To detect redundant calls

        Dim re As Integer
        Dim _BatchID As Long = 0





        Public Sub New()
            If Debugger.IsAttached Then
                '  _Verbose = 1
                '  _DEBUG_LEVEL = 1

            End If


            '_CLASS_NAME = "EDI_5010_LOAD_PAIR_270_271"

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



        Public WriteOnly Property EBR_ID As Long

            Set(value As Long)
                _EBR_ID = value

            End Set
        End Property



        Public WriteOnly Property RES As String

            Set(value As String)
                _RES = value

            End Set
        End Property




        Public WriteOnly Property REQ As String

            Set(value As String)
                _REQ = value

            End Set
        End Property



        Public WriteOnly Property SOURCE As String

            Set(value As String)
                _SOURCE = value

            End Set
        End Property

        Public WriteOnly Property PAYOR_ID As String

            Set(value As String)
                _PAYOR_ID = value

            End Set
        End Property



        Public WriteOnly Property INS_TYPE As String

            Set(value As String)
                _INS_TYPE = value

            End Set
        End Property


        Public WriteOnly Property USER_ID As String

            Set(value As String)
                _USER_ID = value

            End Set
        End Property

        Public WriteOnly Property PAT_HOSP_CODE As String

            Set(value As String)
                _PAT_HOSP_CODE = value

            End Set
        End Property

        Public WriteOnly Property PATIENT_NUMBER As String

            Set(value As String)
                _PATIENT_NUMBER = value

            End Set
        End Property

        Public WriteOnly Property HOSP_CODE As String

            Set(value As String)
                _HOSP_CODE = value

            End Set
        End Property

        Public WriteOnly Property VENDOR_NAME As String

            Set(value As String)
                _VENDOR_NAME = value

            End Set
        End Property


        Public Function ImportPair() As Integer

            Dim r = -1



            'Dim vEDI As New ValidateEDI
            'Dim _270 As New Import270
            'Dim _271 As New Import271




            ' '' update I
            ''If (_REQValid And _RESValid) Then
            ''    start_time = Now
            ''    If _Verbose = 1 Then

            ''        log.ExceptionDetails("UCLA", " Batch ID:   " + Convert.ToString(_BatchID) + " Started ")
            ''        Console.Write(vbNewLine + " Batch ID:   " + Convert.ToString(_BatchID) + " Started ")
            ''    End If


            ''  Using ed


            'Try
            '    'import 270
            '    _270.EDI = _REQ
            '    _270.source = oD.source
            '    _270.ebr_id = oD.ebr_id
            '    _270.user_id = oD.user_id
            '    _270.hosp_code = oD.hosp_code
            '    _270.PAYOR_ID = oD.PAYOR_ID
            '    _270.Vendor_name = oD.Vendor_name
            '    _270.pat_hosp_code = oD.pat_hosp_code
            '    _270.Patient_number = oD.Patient_number
            '    _270.ins_type = oD.ins_type
            '    _270.BatchID = Convert.ToDouble(_BatchID)
            '    _270.Import()
            '    oD._BatchID = _270.Comit()
            'Catch ex As Exception
            '    log.ExceptionDetails("UCLA 270 comiit fail", ex)
            'End Try






            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


            Using EDI_270 As New EDI_270_005010X279A1

                '   _270.EDI = _REQ
                '    _270.source = oD.source
                '    _270.ebr_id = oD.ebr_id
                '    _270.user_id = oD.user_id
                '    _270.hosp_code = oD.hosp_code
                '    _270.PAYOR_ID = oD.PAYOR_ID
                '    _270.Vendor_name = oD.Vendor_name
                '    _270.pat_hosp_code = oD.pat_hosp_code
                '    _270.Patient_number = oD.Patient_number
                '    _270.ins_type = oD.ins_type
                '    _270.BatchID = Convert.ToDouble(_BatchID)


            End Using



            Using EDI_270 As New EDI_270_005010X279A1




            End Using

            Using SSP As New SetProcessStatusFlag
                ' SSP.ConnectionString = oD._ConnectionString
                ' SSP.SP_SET_PROCESS_STATUS = _sqlString
                '  SSP.ProcessStartTime = start_time
                '  SSP.ProcessEndTime = stop_time
                'SSP.SetFlag("P", pair.Value)
                SSP.SetFlag("Y", Convert.ToInt32(_BatchID))
                '  cnt = cnt + 1
            End Using


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''



            'Try
            '    'import 271
            '    _271.EDI = _RES
            '    _271.RAW270 = _REQ
            '    _271.DoAuditLog = False
            '    _271.DeleteFlag = "N"
            '    _271.ebr_id = oD.ebr_id
            '    _271.user_id = oD.user_id
            '    _271.hosp_code = oD.hosp_code
            '    _271.PAYOR_ID = oD.PAYOR_ID
            '    _271.Vendor_name = oD.Vendor_name
            '    _271.ServiceTypeCode = oD.ServiceTypeCode
            '    _271.BatchID = oD._BatchID
            '    _271.source = oD.source
            '    _271.Import()
            '    _271.DeadlockRetrys = _DeadLockRetrys
            '    _271.Comit()
            '    stop_time = Now



            '    Using SSP As New SetProcessStatusFlag
            '        SSP.ConnectionString = oD._ConnectionString
            '        SSP.SP_SET_PROCESS_STATUS = _sqlString
            '        '  SSP.ProcessStartTime = start_time
            '        '  SSP.ProcessEndTime = stop_time
            '        'SSP.SetFlag("P", pair.Value)
            '        SSP.SetFlag("Y", Convert.ToInt32(_BatchID))
            '        cnt = cnt + 1
            '    End Using




            'Catch ex As Exception
            '    log.ExceptionDetails("UCLA 271 comiit fail", ex)
            'End Try





            ''
            'If _Verbose = 1 Then
            '    log.ExceptionDetails("UCLA", " Batch ID:   " + Convert.ToString(_BatchID) + " Completed ")
            '    Console.Write(vbNewLine + " Batch ID:   " + Convert.ToString(_BatchID) + " Completed")
            '    Console.Write(vbNewLine)
            'End If

            'Else
            ''
            'If _Verbose = 1 Then
            '    log.ExceptionDetails("UCLA", " Batch ID:   " + Convert.ToString(_BatchID) + " Ignored, becuase of Invalid REQ & RES. ")
            '    Console.Write(vbNewLine + " Batch ID:   " + Convert.ToString(_BatchID) + " Ignored, becuase of Invalid REQ & RES. ")
            'End If

            'End If


            'Catch ex As System.Exception
            '    i = -1
            '    Using SSP As New SetProcessStatusFlag
            '        SSP.ConnectionString = _ConnectionString
            '        SSP.SP_SET_PROCESS_STATUS = _sqlString
            '        ' SSP.ProcessStartTime = start_time
            '        '  SSP.ProcessEndTime = Now
            '        SSP.SetFlag("X", Convert.ToInt32(_BatchID))
            '        ' 
            '        log.ExceptionDetails("UCLA", ex)
            '        log.ExceptionDetails("UCLA", "Flag updated to X. Issue with the Batch ID  " + Convert.ToString(_BatchID))
            '        If _Verbose = 1 Then

            '            Console.Write(vbNewLine + " Flag updated to X. Issue with the Batch ID " + Convert.ToString(_BatchID))
            '            Console.Write(vbNewLine)
            '        End If
            '        '
            '    End Using
            '    '
            'End Try




            'Try
            '    'oSchedulerLog.ConnectionString = oD._ConnectionString
            '    schId = oSchedulerLog.UpdateLogEntry("UCLAConsole", "Console Applicaiton", schId, x, cnt)
            'Catch ex As Exception
            '    log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.UCLA Update Scheduler log Failed ", ex)
            'End Try
            ''
            'If _Verbose = 1 Then
            '    log.ExceptionDetails("UCLA", "Process End...    Batch ID  " + Convert.ToString(_BatchID))

            '    Console.Write(vbNewLine + "Process End...")
            '    Console.Write(vbNewLine)
            'End If
            ''Else
            ' ''
            ''If _Verbose = 1 Then
            ''    Console.Write(vbNewLine)
            ''    Console.Write(vbNewLine + "Waiting for next Batch Process...")
            ''    Console.Write(vbNewLine)
            ''End If
            ' ''
            ''End If

            'i = 0

            ''

            'Catch ex As System.Exception
            '    log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.UCLA.GO Failed " + oD._BatchID.ToString, ex)

            'Finally

            'End Try

            'Return i





            Return r


        End Function


    End Class

End Namespace

