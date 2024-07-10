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

    Public Class EDI_5010_LOAD_PAIR_276_277


        Inherits EDI_5010_271_005010X279A1_TABLES

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

        Private _ConnectionString As String = String.Empty
        Private _CommandTimeOut As Integer = 90



        Public Sub New()
            If Debugger.IsAttached Then
                _VERBOSE = 1
                _DEBUG_LEVEL = 1

            End If

            _CONSOLE_NAME = "EDI_271_005010X279A1"
            _CLASS_NAME = "EDI_271_005010X279A1"

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

        Public Property LOG_EDI As String

            Set(value As String)
                _LOG_EDI = value
            End Set
            Get
                Return _LOG_EDI
            End Get
        End Property
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

    End Class
End Namespace

