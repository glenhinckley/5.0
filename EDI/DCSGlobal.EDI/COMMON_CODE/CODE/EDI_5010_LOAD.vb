Option Explicit On
Option Strict On
Option Compare Binary



Imports System.Text.RegularExpressions
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Collections.Generic
'Imports System.Data.DataSetExtensions

Imports System.IO.Pipes
Imports System.IO
Imports System.IO.File
Imports System.Text
Imports System.Threading

Imports DCSGlobal.BusinessRules.Logging
Imports DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff
Imports DCSGlobal.BusinessRules.CoreLibrary.IO
Imports DCSGlobal.BusinessRules.CoreLibraryII

Imports System.Security.Principal



Namespace DCSGlobal.EDI





    Public Class EDI_5010_LOAD

        Inherits EDI_5010_COMMON_DECS

        Implements IDisposable



        Protected disposed As Boolean = False

        '******************************************************************************************************************
        '  all vars not declared here are in   EDI_5010_COMMON_DECS   it is inhreted from the tables for this type
        '  if you are going to use it on this class only it goes below if not put it in EDI_5010_COMMON_DECS
        '******************************************************************************************************************


        Private log As New logExecption
        Private ss As New StringStuff
        Private d As New DebugOUT

        Private _InstanceName As String = String.Empty


        Private _baseFolder As String

        Private _connectionString As String = String.Empty

        Private _SMTPServer As String = String.Empty
        Private _FromMailAddress As String = String.Empty
        Private _ToMailAddress As String = String.Empty
        Private _InputFolder As String = String.Empty





        Private _FailedFolder As String = String.Empty
        Private _SuccessFolder As String = String.Empty
        Private _DuplicateFolder As String = String.Empty
        Private _FileFilter As String = String.Empty
        Private _AppName As String = String.Empty



        Public Sub New()
            If Debugger.IsAttached Then
                _VERBOSE = 1
                _DEBUG_LEVEL = 1

            End If

            _CONSOLE_NAME = "EDI_5010_LOAD"
            _CLASS_NAME = "EDI_5010_LOAD"

        End Sub


        Public Sub Dispose() Implements System.IDisposable.Dispose

            GC.SuppressFinalize(Me)

            Console.WriteLine("Object " & GetHashCode() & " disposed.")
        End Sub

        Protected Overrides Sub Finalize()
            log = Nothing
            '    em = Nothing
            Dispose()
            Console.WriteLine("Object " & GetHashCode() & " finalized.")
        End Sub
        Public Property FileFilter As String
            Get
                Return _FileFilter

            End Get
            Set(ByVal value As String)


                _FileFilter = value


            End Set
        End Property


        Public WriteOnly Property InstanceName As String
            Set(value As String)
                _InstanceName = value
                '_app = _InstanceName + ".Load837"
            End Set
        End Property

        Public WriteOnly Property ConnectionString As String
            Set(value As String)
                _connectionString = value
                log.ConnectionString = value
            End Set
        End Property


        Public WriteOnly Property SMTPServer As String
            Set(value As String)
                _SMTPServer = value
                ' em.Server = value
            End Set
        End Property

        Public WriteOnly Property FromMailAddress As String
            Set(value As String)
                _FromMailAddress = value
                ' em.SendFrom = value
            End Set
        End Property

        Public WriteOnly Property ToMailAddress As String
            Set(value As String)
                _ToMailAddress = value
                ' em.SendTo = value
            End Set
        End Property


        Public Property BaseFolder As String
            Get
                Return _baseFolder
            End Get
            Set(ByVal value As String)

                If (ss.GetLastChr(value) <> "\") Then
                    value = value + "\"
                End If
                _baseFolder = value
                ''  FM.BaseFolder = value
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


        Public Property USE_VALUT As Integer

            Set(value As Integer)
                _USE_VAULT = value
            End Set
            Get
                Return _USE_VAULT
            End Get
        End Property

        Public Property InputFolder As String
            Get
                Return _InputFolder

            End Get
            Set(ByVal value As String)


                If (ss.GetLastChr(value) <> "\") Then
                    value = value + "\"
                End If
                _InputFolder = value
                '   FM.Input = value
            End Set
        End Property


        Public Property FailedFolder As String
            Get
                Return _FailedFolder

            End Get
            Set(ByVal value As String)

                If (ss.GetLastChr(value) <> "\") Then
                    value = value + "\"
                End If
                _FailedFolder = value
                ' FM.Failed = value

            End Set
        End Property



        Public Property SuccessFolder As String
            Get
                Return _SuccessFolder

            End Get
            Set(ByVal value As String)

                If (ss.GetLastChr(value) <> "\") Then
                    value = value + "\"
                End If
                _SuccessFolder = value
                '    FM.Success = value
            End Set
        End Property


        Public Property DuplicateFolder As String
            Get
                Return _DuplicateFolder

            End Get
            Set(ByVal value As String)

                If (ss.GetLastChr(value) <> "\") Then
                    value = value + "\"
                End If
                _DuplicateFolder = value
                '   FM.Duplicate = value
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


        Private FileList As New List(Of String)
        Private V As Boolean = False



        Public Function LoadFolder() As Int32
            Dim _FileName As String = String.Empty
            Dim _FQFN As String = String.Empty
            Dim RE As Int32 = 0
            ' Dim fp As New FilePrep
            ' Dim val As New Validate_EDI
            _FUNCTION_NAME = "Function LoadFolder() As Int32"

            ' make a reference to a directory



            ' fp.ConnectionString = _connectionString
            '  val.ConnectionString = _connectionString


            Try
                Dim orderedFiles = New System.IO.DirectoryInfo(_InputFolder).GetFiles().OrderBy(Function(x) x.CreationTime)

                ' Dim File As FileInfo
                Dim _FILE_ID As Integer = 0
                Dim _TransactionSetIdentifierCode As String = String.Empty

                'list the names of all files in the specified directory
                For Each FILE As System.IO.FileInfo In orderedFiles
                    ' ListBox1.Items.Add(dra)


                    _FileName = Convert.ToString(FILE)
                    Console.WriteLine()


                    If _VERBOSE = 1 Then


                        _DEBUG_STRING = String.Format("{0,-15} {1,12}", FILE.Name, FILE.CreationTime.ToString)

                        d.WriteDebugLine(_DEBUG_LEVEL, _DEBUG_STRING)

                    End If




                    Try
                        Using _EDI As EDI_5010_IMPORT = New EDI_5010_IMPORT
                            _EDI.isFile = True
                            _EDI.ConnectionString = _connectionString
                            RE = _EDI.ImportByFileName(_InputFolder, _FileName)
                            _FILE_ID = CInt(_EDI.FILE_ID)
                            _TransactionSetIdentifierCode = _EDI.TransactionSetIdentifierCode
                        End Using
                    Catch ex As Exception

                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "IMPORT", ex)

                    End Try


                    Try
                        If RE = 0 Then
                            Using F As EDI_5010_FILE_MOVE = New EDI_5010_FILE_MOVE
                                F.ConnectionString = _connectionString
                                F.Success = _SuccessFolder
                                F.Input = _InputFolder
                                F._TransactionSetIdentifierCode = _TransactionSetIdentifierCode
                                F.FILE_ID = CInt(_FILE_ID)
                                RE = F.Move(RE, _FileName, _SuccessFolder)
                            End Using
                        End If

                    Catch ex As Exception

                        log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "MOVE", ex)

                    End Try


                Next

            Catch ex As Exception

                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "PARSE FOLDER", ex)


            End Try


            Return RE
        End Function








        Private Function GetRowData() As String

            Dim s As String = ""

            Dim pipeClient As New NamedPipeClientStream(".", _AppName, PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation)

            pipeClient.Connect()
            Dim sss As String = ""
            Dim ss As New StreamString(pipeClient)



            sss = ss.ReadString()

            If sss <> "EOF" Then
                s = sss

            Else
                s = "EOF"

                '  Console.WriteLine("Server could not be verified.")
            End If
            pipeClient.Close()


            ' Give the client process some time to display results before exiting.
            ' Thread.Sleep(4000)


            Return s


        End Function
    End Class
End Namespace

