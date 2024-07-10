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


    Public Class EDI_5010_LOAD_PAIR_270_271



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



        Private _sqlGetREQRES As String = String.Empty




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
                '_Verbose = 1
                '                _DEBUG_LEVEL = 1

            End If


            ' _CLASS_NAME = "EDI_5010_LOAD_PAIR_270_271"

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





        Public Function Go() As Int32


            Dim r = -1


            Dim EBR_ID As Long = 0

            Dim RES As String = String.Empty
            Dim REQ As String = String.Empty




            Dim SOURCE As String = String.Empty
            Dim PAYOR_ID As String = String.Empty
            Dim INS_TYPE As String = String.Empty
            Dim USER_ID As String = String.Empty
            Dim PAT_HOSP_CODE As String = String.Empty
            Dim PATIENT_NUMBER As String = String.Empty
            Dim HOSP_CODE As String = String.Empty
            Dim VENDOR_NAME As String = String.Empty


            Using ConREQRES As New SqlConnection(_ConnectionString)

                ConREQRES.Open()
                Using cmdREQRES As New SqlCommand(_sqlGetREQRES, ConREQRES)

                    cmdREQRES.CommandType = CommandType.StoredProcedure
                    cmdREQRES.Parameters.AddWithValue("@ID", _BatchID)


                    Using rdrREQRES = cmdREQRES.ExecuteReader()

                        If rdrREQRES.HasRows Then

                            Do While rdrREQRES.Read


                                RES = Convert.ToString(rdrREQRES.Item("RESPONSE"))
                                REQ = Convert.ToString(rdrREQRES.Item("REQUEST"))



                                If Not IsDBNull(rdrREQRES("source")) Then
                                    SOURCE = Convert.ToString(rdrREQRES("source"))

                                Else
                                    'Failure
                                End If


                                '
                                If Not IsDBNull(rdrREQRES("payor_id")) Then
                                    PAYOR_ID = Convert.ToString(rdrREQRES("payor_id"))

                                Else
                                    'Failure
                                End If

                                If Not IsDBNull(rdrREQRES("ins_type")) Then
                                    INS_TYPE = Convert.ToString(rdrREQRES("ins_type"))

                                Else
                                    'Failure
                                End If

                                If Not IsDBNull(rdrREQRES("user_id")) Then
                                    USER_ID = Convert.ToString(rdrREQRES("user_id"))


                                Else
                                    'Failure
                                End If

                                If Not IsDBNull(rdrREQRES("pat_hosp_code")) Then
                                    PAT_HOSP_CODE = Convert.ToString(rdrREQRES("pat_hosp_code"))

                                Else
                                    'Failure
                                End If

                                If Not IsDBNull(rdrREQRES("patient_number")) Then
                                    PATIENT_NUMBER = Convert.ToString(rdrREQRES("patient_number"))

                                Else
                                    'Failure
                                End If

                                If Not IsDBNull(rdrREQRES("hosp_code")) Then
                                    HOSP_CODE = Convert.ToString(rdrREQRES("hosp_code"))

                                Else
                                    'Failure
                                End If

                                If Not IsDBNull(rdrREQRES("ebr_id")) Then
                                    EBR_ID = Convert.ToInt64(rdrREQRES("ebr_id"))
                                Else
                                    'Failure
                                End If

                                If Not IsDBNull(rdrREQRES("vendor_name")) Then
                                    VENDOR_NAME = Convert.ToString(rdrREQRES("vendor_name"))

                                Else
                                    'Failure
                                End If

                                ' check 270 and 271 are valid


                                ' do parser pair here



                            Loop
                        End If
                    End Using
                End Using
            End Using



            Return r


        End Function
    End Class

End Namespace
