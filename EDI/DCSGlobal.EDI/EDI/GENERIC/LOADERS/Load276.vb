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

Imports System.IO
Imports System.IO.File
Imports System.Text
Imports System.Threading

Imports DCSGlobal.BusinessRules.Logging
Imports DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff
Imports DCSGlobal.BusinessRules.CoreLibrary.IO


Namespace DCSGlobal.EDI





    Public Class Load276
        Implements IDisposable


        Private log As New logExecption()
        Private em As New Email()
        Private ss As New StringStuff()
        Private FM As New FileMove
        Private fio As New FileIO


        Private _InstanceName As String = String.Empty
        Dim _app As String
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

        Public Sub Dispose() Implements System.IDisposable.Dispose

            GC.SuppressFinalize(Me)

            Console.WriteLine("Object " & GetHashCode() & " disposed.")
        End Sub

        Protected Overrides Sub Finalize()
            log = Nothing
            em = Nothing
            ss = Nothing
            FM = Nothing
            Dispose()
            Console.WriteLine("Object " & GetHashCode() & " finalized.")
        End Sub



        Public WriteOnly Property InstanceName As String
            Set(value As String)
                _InstanceName = value
                _app = _InstanceName + ".Load276"
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
                em.Server = value
            End Set
        End Property

        Public WriteOnly Property FromMailAddress As String
            Set(value As String)
                _FromMailAddress = value
                em.SendFrom = value
            End Set
        End Property

        Public WriteOnly Property ToMailAddress As String
            Set(value As String)
                _ToMailAddress = value
                em.SendTo = value
            End Set
        End Property


        Public Property FileFilter As String
            Get
                Return _FileFilter

            End Get
            Set(ByVal value As String)


                _FileFilter = value


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
                FM.BaseFolder = value
            End Set
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
                FM.Input = value
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
                FM.Failed = value

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
                FM.Success = value
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
                FM.Duplicate = value
            End Set
        End Property


        Private FileList As New List(Of String)
        Private V As Boolean = False



        Public Function LoadFolder() As Int32
            Dim _FileName As String = String.Empty
            Dim _FQFN As String = String.Empty
            Dim RE As Int32 = 0
            Dim fp As New FilePrep
            Dim val As New ValidateEDI
            Dim _EDI As New Import276
            Dim _functionName As String = _app + ".LoadFolder"
            ' make a reference to a directory

            Dim ARE As Integer = 0




            fp.ConnectionString = _connectionString
            val.ConnectionString = _connectionString
            _EDI.ConnectionString = _connectionString




            Try
                Dim Directory As New IO.DirectoryInfo(_InputFolder)
                Dim DirectoryList As IO.FileInfo() = Directory.GetFiles(_FileFilter)
                Dim File As FileInfo

                'list the names of all files in the specified directory
                For Each File In DirectoryList
                    ' ListBox1.Items.Add(dra)


                    _FileName = Convert.ToString(File)
                    RE = fio.CheckExtension(_FileName, _FileFilter)



                    Try
                        If RE = 0 Then
                            'fix the file
                            RE = fp.SingleFile(_InputFolder + _FileName)
                            If RE <> 0 Then
                                ARE = -1
                            End If
                        End If


                        If RE = 0 Then
                            'check to see if the file is a duplicate
                            RE = val.byFile(_InputFolder, _FileName, "276")
                            If RE <> 0 Then
                                ARE = -1
                            End If
                        End If



                        If RE = 0 Then
                            'parse the file 
                            '   RE = _EDI.Import(_Input + _FileName)
                            'RE = 0
                            RE = _EDI.ImportByFileName(_InputFolder, _FileName)
                            _FQFN = _InputFolder + _FileName


                            If RE <> 0 Then
                                ARE = -1
                            End If
                        End If

                        If RE > 0 Then
                            RE = 0


                        End If
                        RE = FM.Move(RE, _FileName, _InputFolder)
                        If RE <> 0 Then
                            ARE = -1
                        End If

                    Catch ex As Exception

                        log.ExceptionDetails(_app, ex)
                        ARE = -1
                    End Try

                Next



            Catch ix As System.IO.DriveNotFoundException


                ' add log and email here
                log.ExceptionDetails(_app, ix)
                ARE = -1
            Catch ix As System.IO.DirectoryNotFoundException

                ' add log and email here

                log.ExceptionDetails(_app, ix)
                ARE = -1
            Catch ix As System.IO.PathTooLongException

                ' add log and email here

                log.ExceptionDetails(_app, ix)
                ARE = -1
            Catch ex As Exception

                log.ExceptionDetails(_app, ex)
                ARE = -1

            End Try


            Return ARE
        End Function





    End Class

End Namespace

