Option Explicit On



Imports DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff
Imports DCSGlobal.BusinessRules.CoreLibrary.IO
Imports System.Text.RegularExpressions
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Collections.Generic
Imports System.Threading
Imports System.Collections


Imports System.IO
Imports System.IO.MemoryMappedFiles
Imports System.Runtime.InteropServices

Imports DCSGlobal.BusinessRules.Logging






Namespace DCSGlobal.EDI





    Public Class EDI_5010_PARSE

        Inherits EDI_5010_COMMON_DECS

        Implements IDisposable



        Protected disposed As Boolean = False

        '******************************************************************************************************************
        '  all vars not declared here are in   EDI_5010_COMMON_DECS   it is inhreted from the tables for this type
        '  if you are going to use it on this class only it goes below if not put it in EDI_5010_COMMON_DECS
        '******************************************************************************************************************


        Private log As New logExecption
        Private ss As New StringStuff

  


        Private au As New AuditResponseLogging
        ' Dim email As New Email
        ' Dim f As New FileIO

        Private start_time As DateTime
        Private stop_time As DateTime
        Private elapsed_time As TimeSpan
        Private _BatchID As Double = 0
        Private objss As New StringStuff

        Private _ConnectionString As String = String.Empty

        Private distinctDT As DataTable
        Private err As String = ""


        Dim _DeadlockRetrys As Int32 = 1



        ' Private meddd As IEnumerable
        Private mmm As Integer = 1

        Private _RejectReasonCode As String = String.Empty

        Private _LoopAgain As String = String.Empty
        Private _TempFolder As String = String.Empty
        Private _InputFolder As String = String.Empty
        Private _FailFolder As String = String.Empty
        Private _SuccessFolder As String = String.Empty
        Private _FileToFix As String = String.Empty
        Private _FileToParse As String = String.Empty

        Private _ThreadNumber As Integer = 0
        Private _CallingApp As String = String.Empty

        ' Private _LOG_EDI As String = "N"

        Private _EmailFrom As String = String.Empty
        Private _EmailTo As String = String.Empty
        Private _EmailServer As String = String.Empty

        ' Private _verbose As Int32

        Private _reqServiceTypeCodes As String = String.Empty



        Private EDI_270 As Object
        Private _EPICOutEDIString As String = String.Empty
        Private _RAW270 As String = String.Empty
        '  Private _VERBOSE As Integer = 0
        '  Private _DEBUG_LEVEL As Integer = 0
        '  Private _CONSOLE_NAME As String = String.Empty
        '  Private _CLASS_NAME As String = String.Empty
        '  Private _LOG_EDI As String = String.Empty
        '  Private _NPI As String = String.Empty
        ' Private _Status As String = String.Empty
        ' Private _FUNCTION_NAME As String = String.Empty

        Private _SearchType As String = String.Empty

        Private _AAAFailureCode As String = String.Empty
        'Private _C_SEARCH_TYPE As String = String.Empty


        Public Sub New()
            If Debugger.IsAttached Then
                _VERBOSE = 1
                _DEBUG_LEVEL = 1
            End If
            _CONSOLE_NAME = "EDI_5010_PARSE"
            _CLASS_NAME = "EDI_5010_PARSE"
        End Sub


        ' This method disposes the base object's resources. 
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposed Then
                If disposing Then


                    log = Nothing
                    'email = Nothing
                    ' Insert code to free managed resources. 
                End If
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



        Public Property LOG_EDI As String

            Set(value As String)
                _LOG_EDI = value
            End Set
            Get
                Return _LOG_EDI
            End Get
        End Property


        Public Property SearchType As String
            Get
                Return _SearchType

            End Get
            Set(value As String)
                _SearchType = value
            End Set
        End Property

        Public Property NPI As String
            Get
                Return _NPI

            End Get
            Set(value As String)
                _NPI = value
            End Set
        End Property


        Public WriteOnly Property AAAFailureCode As String
            Set(value As String)
                _AAAFailureCode = value
            End Set
        End Property





        Public Property ReqServiceTypeCodes As String

            Get
                Return _reqServiceTypeCodes

            End Get

            Set(value As String)
                _reqServiceTypeCodes = value
            End Set
        End Property

        Public WriteOnly Property DeadlockRetrys As Integer

            Set(value As Integer)

                _DeadlockRetrys = value
            End Set
        End Property

        Protected Overrides Sub Finalize()
            Dispose(False)
            MyBase.Finalize()
        End Sub

        Public WriteOnly Property InputFolder As String
            Set(value As String)

                _TempFolder = value

            End Set

        End Property

        Public WriteOnly Property FailFolder As String
            Set(value As String)

                _FailFolder = value

            End Set

        End Property

        Public WriteOnly Property SuccessFolder As String
            Set(value As String)

                _SuccessFolder = value

            End Set

        End Property


        Public WriteOnly Property TempFolder As String
            Set(value As String)

                _TempFolder = value

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



        Public WriteOnly Property ConnectionString As String

            Set(value As String)
                _ConnectionString = value
                log.ConnectionString = _ConnectionString
                au.ConnectionString = _ConnectionString
            End Set
        End Property




        Public ReadOnly Property ErrorMessage As String
            Get
                Return err
            End Get

        End Property


        Public ReadOnly Property Status As String
            Get
                Return _Status

            End Get

        End Property


        Public Property C_SEARCH_TYPE As String

            Set(value As String)
                _C_SEARCH_TYPE = value
            End Set


            Get
                Return _C_SEARCH_TYPE
            End Get

        End Property

        Public ReadOnly Property Reject_Reason_code As String
            Get
                Return _RejectReasonCode
            End Get

        End Property

        Public ReadOnly Property LOOP_AGAIN As String
            Get
                Return _LoopAgain

            End Get

        End Property

        Public WriteOnly Property Verbose As Integer


            Set(value As Integer)
                _verbose = value
            End Set
        End Property



        Public ReadOnly Property EPICOutEDIString As String


            Get
                Return _EPICOutEDIString

            End Get
        End Property

        Public WriteOnly Property RAW270 As String


            Set(value As String)
                _RAW270 = value
            End Set
        End Property








        Public Function Import270_271(ByVal EDI_270 As String, ByVal EDI_271 As String, ByVal DELETE_FLAG_270 As String, ByVal DELETE_FLAG_271 As String, _
                                       ByVal EBR_ID As Double, ByVal USER_ID As String, ByVal HOSP_CODE As String, ByVal SOURCE As String, _
                                       ByVal PAYOR_ID As String, ByVal VENDOR_NAME As String, ByVal INS_TYPE As String, _
                                       ByVal PATIENT_NUMBER As String, ByVal PAT_HSOP_CODE As String, ByVal LOG_EDI As String, ByVal BatchID As Double) As Int64


            Dim i270 As New EDI_270_005010X279A1()
            Dim i271 As New EDI_271_005010X279A1()
            Dim r As Int64 = -1
            Dim r271 As Integer
            Dim r270 As Int64

            i271.DeadlockRetrys = _DeadlockRetrys
            Try






                i270.ConnectionString = _ConnectionString
                i270.EDI = EDI_270
                i270.DeleteFlag = DELETE_FLAG_270
                i270.ebr_id = EBR_ID
                i270.user_id = USER_ID
                i270.source = SOURCE
                i270.hosp_code = HOSP_CODE
                i270.Patient_number = PATIENT_NUMBER
                i270.PAYOR_ID = PAYOR_ID
                i270.ins_type = INS_TYPE
                i270.pat_hosp_code = PAT_HSOP_CODE
                i270.Vendor_name = VENDOR_NAME

                i270.ImportByString(EDI_270, BatchID)

                r270 = i270.BatchID


                'r270 = i270.Comit

                r = r270



                If r270 > 0 Then



                    'i270.DeleteFlag = DELETE_FLAG_270




                    i271.BatchID = r270
                    i271.ConnectionString = _ConnectionString

                    'aded by glen to fix the loging to auit temp
                    i271.RAW270 = EDI_270


                    i271.EDI = EDI_271
                    i271.DeleteFlag = DELETE_FLAG_271
                    i271.ebr_id = EBR_ID
                    i271.user_id = USER_ID
                    i271.source = SOURCE
                    i271.hosp_code = HOSP_CODE

                    i271.PAYOR_ID = PAYOR_ID

                    i271.Vendor_name = VENDOR_NAME
                    i271.Log_EDI = _LOG_EDI


                    r271 = i271.ImportByString(EDI_271, BatchID)
                    If r271 = 0 Then




                        '  r271 = i271.Comit

                        _LoopAgain = i271.LOOP_AGAIN
                        _RejectReasonCode = i271.Reject_Reason_code
                        _Status = i271.Status


                    Else
                        If _verbose = 1 Then
                            log.ExceptionDetails("90-Parse.Import271", "no batch id or parse tables empty", "", Me.ToString)
                        End If
                        'Throw New Exception("271 Execption : no batch id or parse tables empty")

                    End If






                End If





            Catch ex As Exception

                log.ExceptionDetails("91-Parse.Import270_271", ex.Message, ex.StackTrace, Me.ToString)

                'Throw New Exception("Import270_271 Execption")
            Finally

                i270 = Nothing
                i271 = Nothing


            End Try



            Return r



        End Function


        Public Function Import270_EPIC(ByVal EDI_270 As String, ByVal DELETE_FLAG_270 As String, _
                                        ByVal EBR_ID As Double, ByVal USER_ID As String, ByVal HOSP_CODE As String, ByVal SOURCE As String, _
                                        ByVal PAYOR_ID As String, ByVal VENDOR_NAME As String, ByVal INS_TYPE As String, _
                                        ByVal PATIENT_NUMBER As String, ByVal PAT_HSOP_CODE As String,
                                        ByVal BatchID As Double) As Int64





            ' 276 is the same,  any changes to 276 proball need to get propicated there

            Dim i270 As New EDI_270_005010X279A1()

            Dim r As Integer = -1

            Dim r270 As Integer


            Try




                i270.ConnectionString = _ConnectionString
                i270.EDI = EDI_270
                i270.DeleteFlag = DELETE_FLAG_270
                i270.ebr_id = EBR_ID
                i270.user_id = USER_ID
                i270.source = SOURCE
                i270.hosp_code = HOSP_CODE
                i270.Patient_number = PATIENT_NUMBER
                i270.PAYOR_ID = PAYOR_ID
                i270.ins_type = INS_TYPE
                i270.pat_hosp_code = PAT_HSOP_CODE
                i270.Vendor_name = VENDOR_NAME
                'i270.BatchID = BatchID
                ''i270.DeleteFlag = DELETE_FLAG_270






                i270.ImportByString(EDI_270, BatchID)
                r270 = i270.BatchID
                'r270 = i270.Comit


                _LoopAgain = i270.LOOP_AGAIN
                '_RejectReasonCode = i270.Reject_Reason_code
                _Status = i270.Status


                If (r270 > 0) Then
                    r = r270
                End If

            Catch ex As Exception


                log.ExceptionDetails("92-Parse.Import270", ex.Message, ex.StackTrace, Me.ToString)

                'Throw New Exception("270 Execption")
            Finally

                i270 = Nothing
                'log.ExceptionDetails("Parse.Import270","End parse ")

            End Try



            Return r



        End Function
        Public Function Import270(ByVal EDI_270 As String, ByVal DELETE_FLAG_270 As String, _
                                ByVal EBR_ID As Int64, ByVal USER_ID As String, ByVal HOSP_CODE As String, ByVal SOURCE As String, _
                                ByVal PAYOR_ID As String, ByVal VENDOR_NAME As String, ByVal INS_TYPE As String, _
                                ByVal PATIENT_NUMBER As String, ByVal PAT_HSOP_CODE As String,
                                ByVal BatchID As Int64) As Int64


            ' 276 is the same,  any changes to 276 proball need to get propicated there

            '     log.ExceptionDetails("61-Parse.Import270", " Begin " + Convert.ToString(EBR_ID))


            Dim i270 As New EDI_270_005010X279A1()

            Dim r As Integer = -1

            Dim r270Return As Integer = -1


            Dim r270 As Integer = 0


            Try




                i270.ConnectionString = _ConnectionString
                'i270.EDI = EDI_270
                i270.DeleteFlag = DELETE_FLAG_270
                i270.EBR_ID = EBR_ID
                i270.user_id = USER_ID
                i270.source = SOURCE
                i270.hosp_code = HOSP_CODE
                i270.Patient_number = PATIENT_NUMBER
                i270.PAYOR_ID = PAYOR_ID
                i270.ins_type = INS_TYPE
                i270.pat_hosp_code = PAT_HSOP_CODE
                i270.Vendor_name = VENDOR_NAME
                ' i270.BatchID = BatchIDe
                'i270.DeleteFlag = DELETE_FLAG_270
                i270._C_SEARCH_TYPE = _SearchType





                r270Return = i270.ImportByString(EDI_270, BatchID)

                r270 = i270.BatchID

                '_LoopAgain = i270.LOOP_AGAIN
                '_RejectReasonCode = i270.Reject_Reason_code
                '_Status = i270.Status

                '_reqServiceTypeCodes = i270.ReqServiceTypeCode
                '_NPI = i270.ReqNPI

                If (r270Return = 0) Then
                    r = r270
                Else
                    r = r270Return

                End If

            Catch ex As Exception

                r = -999
                log.ExceptionDetails("60-Parse.Import270", ex.Message, ex.StackTrace, Me.ToString)

                ' Throw New Exception("270 Execption")
            Finally

                i270 = Nothing
                'log.ExceptionDetails("Parse.Import270","End parse ")

            End Try



            Return r


        End Function


        'Public Function Import270_DNU(ByVal EDI_270 As String, ByVal DELETE_FLAG_270 As String, _
        '                ByVal EBR_ID As Int64, ByVal USER_ID As String, ByVal HOSP_CODE As String, ByVal SOURCE As String, _
        '                ByVal PAYOR_ID As String, ByVal VENDOR_NAME As String, ByVal INS_TYPE As String, _
        '                ByVal PATIENT_NUMBER As String, ByVal PAT_HSOP_CODE As String,
        '                ByVal BatchID As Int64) As Int64


        '    ' 276 is the same,  any changes to 276 proball need to get propicated there

        '    '     log.ExceptionDetails("61-Parse.Import270", " Begin " + Convert.ToString(EBR_ID))


        '    Dim i270 As New EDI_270_005010X279A1()

        '    Dim r As Integer = -1

        '    Dim r270Return As Integer = -1


        '    Dim r270 As Integer = 0


        '    Try




        '        i270.ConnectionString = _ConnectionString
        '        'i270.EDI = EDI_270
        '        i270.DeleteFlag = DELETE_FLAG_270
        '        i270.EBR_ID = EBR_ID
        '        i270.user_id = USER_ID
        '        i270.source = SOURCE
        '        i270.hosp_code = HOSP_CODE
        '        i270.Patient_number = PATIENT_NUMBER
        '        i270.PAYOR_ID = PAYOR_ID
        '        i270.ins_type = INS_TYPE
        '        i270.pat_hosp_code = PAT_HSOP_CODE
        '        i270.Vendor_name = VENDOR_NAME
        '        ' i270.BatchID = BatchIDe
        '        'i270.DeleteFlag = DELETE_FLAG_270
        '        i270._C_SEARCH_TYPE = _SearchType





        '        r270Return = i270.ImportByString(EDI_270, BatchID)

        '        r270 = i270.BatchID

        '        '_LoopAgain = i270.LOOP_AGAIN
        '        '_RejectReasonCode = i270.Reject_Reason_code
        '        '_Status = i270.Status

        '        '_reqServiceTypeCodes = i270.ReqServiceTypeCode
        '        '_NPI = i270.ReqNPI

        '        If (r270Return = 0) Then
        '            r = r270
        '        Else
        '            r = r270Return

        '        End If

        '    Catch ex As Exception

        '        r = -999
        '        log.ExceptionDetails("60-Parse.Import270", ex.Message, ex.StackTrace, Me.ToString)

        '        ' Throw New Exception("270 Execption")
        '    Finally

        '        i270 = Nothing
        '        'log.ExceptionDetails("Parse.Import270","End parse ")

        '    End Try



        '    Return r


        'End Function



        '<Obsolete("This FUNCTION is deprecated, use  Public Function Import270(ByVal EDI_270 As String, ByVal DELETE_FLAG_270 As String, ByVal EBR_ID As Double, ByVal USER_ID As String, ByVal HOSP_CODE As String, ByVal SOURCE As String, ByVal PAYOR_ID As String, ByVal VENDOR_NAME As String, ByVal INS_TYPE As String,  ByVal PATIENT_NUMBER As String, ByVal PAT_HSOP_CODE As String, ByVal BatchID As Int64) As Int64")>
        'Public Function Import270(ByVal EDI_270 As String, ByVal DELETE_FLAG_270 As String, _
        '                                ByVal EBR_ID As Double, ByVal USER_ID As String, ByVal HOSP_CODE As String, ByVal SOURCE As String, _
        '                                ByVal PAYOR_ID As String, ByVal VENDOR_NAME As String, ByVal INS_TYPE As String, _
        '                                ByVal PATIENT_NUMBER As String, ByVal PAT_HSOP_CODE As String,
        '                                ByVal BatchID As Double) As Int64



        '    ' 276 is the same,  any changes to 276 proball need to get propicated there

        '    '     log.ExceptionDetails("61-Parse.Import270", " Begin " + Convert.ToString(EBR_ID))


        '    Dim i270 As New EDI_270_005010X279A1()

        '    Dim r As Integer = -1

        '    Dim r270 As Integer


        '    Try




        '        i270.ConnectionString = _ConnectionString
        '        'i270.EDI = EDI_270
        '        i270.DeleteFlag = DELETE_FLAG_270
        '        i270.EBR_ID = EBR_ID
        '        i270.user_id = USER_ID
        '        i270.source = SOURCE
        '        i270.hosp_code = HOSP_CODE
        '        i270.Patient_number = PATIENT_NUMBER
        '        i270.PAYOR_ID = PAYOR_ID
        '        i270.ins_type = INS_TYPE
        '        i270.pat_hosp_code = PAT_HSOP_CODE
        '        i270.Vendor_name = VENDOR_NAME
        '        ' i270.BatchID = BatchIDe
        '        'i270.DeleteFlag = DELETE_FLAG_270
        '        i270._C_SEARCH_TYPE = _SearchType





        '        i270.ImportByString(EDI_270, BatchID)

        '        r270 = i270.BatchID

        '        '_LoopAgain = i270.LOOP_AGAIN
        '        '_RejectReasonCode = i270.Reject_Reason_code
        '        '_Status = i270.Status

        '        '_reqServiceTypeCodes = i270.ReqServiceTypeCode
        '        '_NPI = i270.ReqNPI

        '        If (r270 > 0) Then
        '            r = r270
        '        End If

        '    Catch ex As Exception


        '        log.ExceptionDetails("60-Parse.Import270", ex.Message, ex.StackTrace, Me.ToString)

        '        ' Throw New Exception("270 Execption")
        '    Finally

        '        i270 = Nothing
        '        'log.ExceptionDetails("Parse.Import270","End parse ")

        '    End Try



        '    Return r

        'End Function


        Public Function Import271_EPIC(ByVal EDI_271 As String, ByVal DELETE_FLAG_271 As String, _
                                   ByVal EBR_ID As String, ByVal USER_ID As String, ByVal HOSP_CODE As String, ByVal SOURCE As String,
                                   ByVal PAYOR_ID As String, ByVal VENDOR_NAME As String, _
                                   ByVal LOG_EDI As String, ByVal BatchID As Int64) As Integer


            'add the logging back here   




            'Using ARL As New AuditResponseLogging
            '    ARL.ConnectionString = _ConnectionString
            '    ARL.Log270(BatchID, PAYOR_ID, EDI_271, EBR_ID)
            'End Using





            Dim i271 As New EDI_271_005010X279A1()
            Dim r As Integer
            Dim r271 As Integer



            i271.DeadlockRetrys = _DeadlockRetrys

            '' i271.


            Try


                If _VERBOSE = 1 Then

                    log.ExceptionDetails("93-Parse.Import271 EPIC ", "begin batch id " + BatchID.ToString)

                End If


                '  i271.BatchID = BatchID
                i271.ConnectionString = _ConnectionString
                '  i271.EDI = EDI_271
                i271.DeleteFlag = DELETE_FLAG_271
                i271.ebr_id = EBR_ID
                i271.user_id = USER_ID
                i271.source = SOURCE
                i271.hosp_code = HOSP_CODE
                i271.isEPIC = True

                i271.PAYOR_ID = PAYOR_ID

                i271.Vendor_name = VENDOR_NAME
                i271.LOG_EDI = _LOG_EDI
                If _VERBOSE = 1 Then
                    log.ExceptionDetails("94-Parse.Import271", "calling import for " + BatchID.ToString)

                End If


                r271 = i271.ImportByString(EDI_271, BatchID)


                If r271 = 0 Then





                    'r271 = i271.ComitEPIC
                    _EPICOutEDIString = i271.EPICOutEDIString
                    _LoopAgain = i271.LOOP_AGAIN
                    _RejectReasonCode = i271.Reject_Reason_code
                    _STATUS = i271.Status

                    If _VERBOSE = 1 Then

                        log.ExceptionDetails("95-Parse.Import271 EPIC", ("LoopAgain " + _LoopAgain + "   RejectReasonCode  " + _RejectReasonCode + "  Status " + _STATUS + " for batchID" + BatchID.ToString))
                    End If

                Else

                    log.ExceptionDetails("64-Parse.Import271", "no batch id or parse tables empty  for bacthchid : + |" + Convert.ToString(BatchID) + "ES|" + EDI_271 + "|EE  RC|" + Convert.ToString(r271) + "|")

                    '   Throw New Exception("271 Execption  CPIC : no batch id or parse tables empty look in ed")

                End If

                If (r271 > 0) Then
                    r = r271
                End If

            Catch ex As Exception
                log.ExceptionDetails("97-Parse.Import271 EPIC", ex.Message, ex.StackTrace, Me.ToString)


                'Throw New Exception(ex.Message)


            Finally


                i271 = Nothing


            End Try



            Return r






        End Function




        Public Function Import271(ByVal EDI_271 As String, ByVal DELETE_FLAG_271 As String, _
                                            ByVal EBR_ID As Int64, ByVal USER_ID As String, ByVal HOSP_CODE As String, ByVal SOURCE As String,
                                            ByVal PAYOR_ID As String, ByVal VENDOR_NAME As String, _
                                            ByVal LOG_EDI As String, ByVal BatchID As Int64) As Integer






            Dim i271 As New EDI_271_005010X279A1()
            Dim r As Integer = -1
            Dim r271 As Integer = 0
            Dim DoParseFlag = True
            Dim r271LoopCount As Integer = 0



            i271.DeadlockRetrys = _DeadlockRetrys


            Try


                If _VERBOSE = 1 Then

                    log.ExceptionDetails("61-Parse.Import271", "begin batch id " + BatchID.ToString)

                End If


                i271.BatchID = BatchID
                i271.ConnectionString = _ConnectionString
                i271.EDI = EDI_271
                i271.DeleteFlag = DELETE_FLAG_271
                i271.ebr_id = EBR_ID
                i271.user_id = USER_ID
                i271.source = SOURCE
                i271.hosp_code = HOSP_CODE

                '   i271.RAW270 = RAW270


                i271.PAYOR_ID = PAYOR_ID


                'added by glen on 8-24 to sup[ress dull loinging 
                ' i271.DoAuditLog = False



                i271.Vendor_name = VENDOR_NAME
                i271.LOG_EDI = LOG_EDI
                If _VERBOSE = 1 Then
                    log.ExceptionDetails("62-Parse.Import271", "calling import for " + BatchID.ToString)

                End If

                '
                i271.ServiceTypeCode = _reqServiceTypeCodes
                i271.NPI = _NPI


                r271 = i271.ImportByString(EDI_271, BatchID)
                _LoopAgain = i271.LOOP_AGAIN
                _RejectReasonCode = i271.Reject_Reason_code
                _STATUS = i271.Status


                'While r271 >= 0
                '    r271LoopCount = r271LoopCount + 1
                '    r271 = i271.ImportByString(EDI_271, BatchID)
                '    _LoopAgain = i271.LOOP_AGAIN
                '    _RejectReasonCode = i271.Reject_Reason_code
                '    _STATUS = i271.Status

                '    If r271LoopCount = 3 Then
                '        r271 = -999
                '        log.ExceptionDetails("63-Parse.5010_Import271", ("LoopAgain " + _LoopAgain + "   RejectReasonCode  " + _RejectReasonCode + "  Status " + _STATUS + " for batchID" + BatchID.ToString))

                '    End If


                'End While


            Catch ex As Exception
                log.ExceptionDetails("65-Parse.Import271", ex.Message, ex.StackTrace, Me.ToString)
            Finally
                i271 = Nothing
            End Try
            ' End If


            Return r271

        End Function








        Public Function Import835(ByVal FileName As String) As Integer

            Dim r As Integer = 0








            Return r


        End Function



        'Public Function Import837File(ByVal FileName As String) As Integer

        '    Dim r As Integer = 0
        '    Dim i837 As New Import837()
        '    Dim ff As New FilePrep()
        '    Dim _guid As Guid = Guid.Empty






        '    _FileToParse = _TempFolder + Convert.ToString(_guid) + ".837"
        '    _FileToFix = _InputFolder + FileName

        '    'copy the file to the tmep folder with a guid name and fix it
        '    'prase adn import the file




        '    i837.ConnectionString = _ConnectionString
        '    ff.ConnectionString = _ConnectionString




        '    If r = 0 Then
        '        r = ff.SingleFile(_FileToFix, _FileToParse)
        '    End If





        '    If r > 0 Then
        '        r = i837.Import(_FileToParse)
        '    End If



        '    If r > 0 Then



        '    Else



        '    End If





        '    'move the file to sucess or fail
        '    'delete the temp file
        '    'delete the orginal.






        '    Return r


        'End Function



        'Public Function Import837Folder(ByVal FolderName As String) As Integer

        '    Dim r As Integer = 0
        '    Dim i837 As New Import837()
        '    Dim ff As New FilePrep()
        '    Dim _guid As Guid = Guid.Empty


        '    '' validate the file

        '    ''ValidateEDI.vb

        '    _FileToParse = _TempFolder + Convert.ToString(_guid) + ".837"
        '    _FileToFix = _InputFolder + FolderName

        '    'copy the file to the tmep folder with a guid name and fix it
        '    'prase adn import the file




        '    i837.ConnectionString = _ConnectionString
        '    ff.ConnectionString = _ConnectionString




        '    If r = 0 Then
        '        r = ff.SingleFile(_FileToFix, _FileToParse)
        '    End If





        '    If r > 0 Then
        '        r = i837.Import(_FileToParse)
        '    End If



        '    If r > 0 Then



        '    Else



        '    End If





        '    'move the file to sucess or fail
        '    'delete the temp file
        '    'delete the orginal.






        '    Return r


        'End Function



        'Public Function Import276(ByVal FileName As String) As Integer

        '    Dim r As Integer = 0








        '    Return r


        'End Function



        Public Function Import276(ByVal EDI_276 As String, ByVal DELETE_FLAG_276 As String, _
                                        ByVal CBR_ID As Double, ByVal USER_ID As String, ByVal HOSP_CODE As String, ByVal SOURCE As String, _
                                        ByVal PAYOR_ID As String, ByVal VENDOR_NAME As String, ByVal INS_TYPE As String, _
                                        ByVal PATIENT_NUMBER As String, ByVal PAT_HSOP_CODE As String,
                                        ByVal BatchID As Double) As Int64


            'same as 270 actually cut and pasted it 
            ' any changes to 270 proball need to get propicated here






            Dim r As Integer = 0

            Dim l276 As List(Of String)
            Dim r276 As Integer

            Try
                Using SP As New StringPrep()

                    l276 = SP.SingleEDI(EDI_276)

                End Using

                Using i276 As New EDI_5010_276_005010X212_DCS()
                    i276.ConnectionString = _ConnectionString
                    i276.EDI = EDI_276
                    i276.isFile = False
                    i276.DeleteFlag = DELETE_FLAG_276
                    i276.cbr_id = CBR_ID
                    i276.user_id = USER_ID
                    i276.source = SOURCE
                    i276.hosp_code = HOSP_CODE
                    i276.Patient_number = PATIENT_NUMBER
                    i276.PAYOR_ID = PAYOR_ID
                    i276.ins_type = INS_TYPE
                    i276.pat_hosp_code = PAT_HSOP_CODE
                    i276.Vendor_name = VENDOR_NAME
                    i276.BatchID = BatchID
                    i276.LOG_EDI = _LOG_EDI
                    'i270.DeleteFlag = DELETE_FLAG_270






                    r276 = i276.Import(l276, BatchID)

                    ' r270 = i270.Comit


                    _LoopAgain = i276.LOOP_AGAIN
                    _RejectReasonCode = i276.Reject_Reason_code
                    _STATUS = i276.Status


                End Using

            Catch ex As Exception


                log.ExceptionDetails("EDI_5010_PARSE.Import276", ex)

                'Throw New Exception("276 Execption")


            End Try







            Return r276


        End Function





        Public Function Import277(ByVal EDI_277 As String, ByVal DELETE_FLAG_277 As String, _
                                            ByVal CBR_ID As String, ByVal USER_ID As String, ByVal HOSP_CODE As String, ByVal SOURCE As String,
                                            ByVal PAYOR_ID As String, ByVal VENDOR_NAME As String, _
                                            ByVal LOG_EDI As String, ByVal BatchID As Double) As Integer


            'same as 271 actually cut and pasted it 
            ' any changes to 271 proball need to get propicated here

            _FUNCTION_NAME = "Import277"

            ' _verbose = 1

            au.Log277(BatchID, PAYOR_ID, EDI_277, CBR_ID)
            Dim r As Integer = 0

            Dim r277 As Integer = 0


            Dim SP As New StringPrep()




            Dim l277 As List(Of String)


            Try


                If _VERBOSE = 1 Then
                    log.ExceptionDetails("97-Parse.Import277", "begin batch id " + BatchID.ToString)
                End If



                Using i277 As New EDI_5010_277_005010X212_v2()
                    l277 = SP.SingleEDI(EDI_277)
                    i277.BatchID = BatchID
                    i277.isFile = False
                    i277.ConnectionString = _ConnectionString
                    i277.EDI = EDI_277
                    i277.DeleteFlag = DELETE_FLAG_277
                    i277.ebr_id = CBR_ID
                    i277.user_id = USER_ID
                    i277.source = SOURCE
                    i277.hosp_code = HOSP_CODE

                    i277.CONSOLE_NAME = _CONSOLE_NAME

                    i277.PAYOR_ID = PAYOR_ID

                    i277.Vendor_name = VENDOR_NAME
                    i277.LOG_EDI = _LOG_EDI
                    If _VERBOSE = 1 Then
                        log.ExceptionDetails("98-Parse.Import277", "calling import for " + BatchID.ToString)

                    End If
                    r277 = i277.Import(l277, BatchID)



                    If r277 > 0 Then




                        '  r271 = i271.Comit

                        _LoopAgain = i277.LOOP_AGAIN
                        _RejectReasonCode = i277.Reject_Reason_code
                        _STATUS = i277.Status

                        If _VERBOSE = 1 Then

                            log.ExceptionDetails("99-Parse.Import277", ("LoopAgain " + _LoopAgain + "   RejectReasonCode  " + _RejectReasonCode + "  Status " + _STATUS + " for batchID" + BatchID.ToString))
                        End If

                    Else

                        If _VERBOSE = 1 Then
                            log.ExceptionDetails("99A-Parse.Import277", "no batch id or parse tables empty", "", Me.ToString)
                        End If

                        '    Throw New Exception("277 Execption : no batch id or parse tables empty")

                    End If
                End Using

            Catch ex As Exception
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "99B-Parse.Import277", ex)

            End Try


            Return r


        End Function

        Public Function Import277(ByVal EDIString As String, ByVal TMPFolder As String) As Integer

            Dim r As Integer = 0








            Return r


        End Function


        Public Function Import278Request(ByVal EDI_278 As String, ByVal DELETE_FLAG_278 As String, _
                                        ByVal AUTH_ID As String, ByVal USER_ID As String, ByVal HOSP_CODE As String, ByVal SOURCE As String,
                                            ByVal PAYOR_ID As String, ByVal VENDOR_NAME As String, ByVal INS_TYPE As String, _
                                            ByVal account_number As String, ByVal PatHospCode As String) As Integer
            Dim r As Integer = -1

            Try
                Using arl As New AuditResponseLogging()
                    arl.ConnectionString = _ConnectionString
                    r = arl.LOG278toREQ(EDI_278, DELETE_FLAG_278, AUTH_ID, USER_ID, HOSP_CODE, SOURCE, PAYOR_ID, VENDOR_NAME, INS_TYPE, account_number, PatHospCode)
                End Using
            Catch ex As Exception
                r = -4
            End Try


            Return r


        End Function


        Public Function Import278Response(ByVal EDI_278 As String, ByVal DELETE_FLAG_278 As String, ByVal BATCH_ID As String, _
                                ByVal AUTH_ID As String, ByVal USER_ID As String, ByVal HOSP_CODE As String, ByVal SOURCE As String,
                                    ByVal PAYOR_ID As String, ByVal VENDOR_NAME As String, ByVal INS_TYPE As String, _
                                    ByVal account_number As String, ByVal PatHospCode As String) As Integer
            Dim r As Integer = -1
            Dim isTA1 As Boolean = False



            Try
                Using arl As New AuditResponseLogging()
                    arl.ConnectionString = _ConnectionString
                    r = arl.LOG278toRES(EDI_278, DELETE_FLAG_278, AUTH_ID, BATCH_ID, USER_ID, _
                                        HOSP_CODE, SOURCE, PAYOR_ID, VENDOR_NAME, INS_TYPE, account_number, PatHospCode)
                End Using




            Catch ex As Exception
                r = -4
            End Try





            Dim SP As New StringPrep()
            Dim i278 As New Import278()



            Dim l278 As List(Of String)
            Dim r278 As Integer

            l278 = SP.SingleEDI(EDI_278)

            ' check for TA1 
            Using v As New ValidateEDI()
                v.byList(l278, "278")

                isTA1 = v.isTA1
            End Using



            If isTA1 = False Then
                Try

                    i278.ConnectionString = _ConnectionString
                    i278.EDI = EDI_278
                    i278.ebr_id = AUTH_ID
                    i278.DeleteFlag = DELETE_FLAG_278
                    ' i278.cbr_id = CBR_ID
                    i278.user_id = USER_ID
                    i278.source = SOURCE
                    i278.hosp_code = HOSP_CODE
                    i278.Vendor_name = VENDOR_NAME
                    i278.PAYOR_ID = PAYOR_ID
                    ' i278.ins_type = INS_TYPE
                    i278.PAYOR_ID = PAYOR_ID
                    i278.Vendor_name = VENDOR_NAME
                    i278.BatchID = BATCH_ID
                    i278.DeleteFlag = DELETE_FLAG_278






                    r278 = i278.ImportByList(l278, BATCH_ID)

                    ' r270 = i270.Comit


                    _LoopAgain = i278.LOOP_AGAIN
                    _RejectReasonCode = i278.Reject_Reason_code
                    _STATUS = i278.Status


                    r = r278

                Catch ex As Exception

                    r = -1
                    log.ExceptionDetails("80A-Parse.Import278", ex.Message, ex.StackTrace, Me.ToString)

                    'Throw New Exception("276 Execption")
                Finally
                    '  r = -2
                    i278 = Nothing
                    'log.ExceptionDetails("Parse.Import270","End parse ")

                End Try


            Else

                Try

                    Dim ITA1 As New ImportTA1()
                    ITA1.ConnectionString = _ConnectionString
                    ITA1.Import(l278, BATCH_ID)

                Catch ex As Exception
                    r = -2
                    log.ExceptionDetails("80B-Parse.Import278.TA1", ex.Message, ex.StackTrace, Me.ToString)
                End Try


            End If










            Return r


        End Function



        Public Function Import278(ByVal EDI_278 As String, ByVal DELETE_FLAG_278 As String, _
                                        ByVal EBR_ID As String, ByVal USER_ID As String, ByVal HOSP_CODE As String, ByVal SOURCE As String,
                                            ByVal PAYOR_ID As String, ByVal VENDOR_NAME As String, _
                                            ByVal LOG_EDI As String, ByVal BatchID As Int64) As Integer

            Dim r As Integer = -1



            Dim SP As New StringPrep()
            Dim i278 As New Import278()



            Dim l278 As List(Of String)
            Dim r278 As Integer

            Try


                l278 = SP.SingleEDI(EDI_278)



                i278.ConnectionString = _ConnectionString
                i278.EDI = EDI_278
                i278.ebr_id = EBR_ID
                i278.DeleteFlag = DELETE_FLAG_278
                ' i278.cbr_id = CBR_ID
                i278.user_id = USER_ID
                i278.source = SOURCE
                i278.hosp_code = HOSP_CODE
                i278.Vendor_name = VENDOR_NAME
                i278.PAYOR_ID = PAYOR_ID
                ' i278.ins_type = INS_TYPE
                i278.PAYOR_ID = PAYOR_ID
                i278.Vendor_name = VENDOR_NAME
                i278.BatchID = BatchID
                i278.DeleteFlag = DELETE_FLAG_278






                r278 = i278.ImportByList(l278, BatchID)

                ' r270 = i270.Comit


                _LoopAgain = i278.LOOP_AGAIN
                _RejectReasonCode = i278.Reject_Reason_code
                _STATUS = i278.Status


                r = r278

            Catch ex As Exception

                r = -1
                log.ExceptionDetails("80C-Parse.Import278", ex.Message, ex.StackTrace, Me.ToString)

                'Throw New Exception("276 Execption")
            Finally
                r = -2
                i278 = Nothing
                'log.ExceptionDetails("Parse.Import270","End parse ")

            End Try

            Return r


        End Function



    End Class
End Namespace
