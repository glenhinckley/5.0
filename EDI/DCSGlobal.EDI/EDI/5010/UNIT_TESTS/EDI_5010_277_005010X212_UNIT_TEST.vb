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

    Public Class EDI_277_005010X212_UNIT_TEST



        Inherits EDI_5010_277_00510X212_TABLES

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



        Private _SP_ISA As String = "[usp_EDI_5010_HIPAA_277_ISA]"
        Private _SP_IEA As String = "[usp_EDI_5010_HIPAA_277_IEA]"

        Private _SP_GS As String = "[usp_EDI_5010_HIPAA_277_GS]"
        Private _SP_GE As String = "[usp_EDI_5010_HIPAA_277_GE]"

        Private _SP_ST As String = "[usp_EDI_5010_HIPAA_277_ST]"
        Private _SP_SE As String = "[usp_EDI_5010_HIPAA_277_SE]"

        Private _SP_HEADERS As String = "[usp_EDI_5010_HIPAA_277_HEADERS]"

        Private _SP_COMIT_ROW_DATA As String = "[usp_5010_HIPAA_277_UTYPE_TEST_DNU]"

        Private _SP_COMIT_HL22_DATA As String = "[usp_5010_HIPAA_277_UTYPE_TEST_DNU]"

        Private _SP_COMIT_UNKNOWN As String = "[usp_EDI_UNKNOWN]"

        Private _SP_ROLLBACK As String = "[usp_EDI_277_ROLLBACK]"


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
                _SP_COMIT_ROW_DATA = value
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


        Public WriteOnly Property HL20_SP_TEST As Boolean

            Set(value As Boolean)
                _HL20_SP_TEST = value
            End Set
        End Property





        Public Function RunAllTests() As Integer
            Dim i As Integer






            Return i

        End Function










        Private Function TestSPs() As Integer

            Dim r As Integer = -1



            Try



                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(_SP_COMIT_HL22_DATA, Con)

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
                    Using cmd As New SqlCommand(_SP_COMIT_HL22_DATA, Con)

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
                _HL22_SP_TEST = True
            Catch ex As Exception
                _HL22_SP_TEST = False
                log.ExceptionDetails("HL 20 TEST FAILED 005010X212_277", ex)
            End Try

            Return r



        End Function

    End Class

End Namespace
