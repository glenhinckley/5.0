Option Explicit On
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

Imports System.IO
Imports System.IO.File
Imports System.Text
Imports System.Threading

Imports DCSGlobal.BusinessRules.Logging



Namespace DCSGlobal.EDI





    Public Class Load837
        Implements IDisposable

        Private _InstanceName As String = String.Empty
        Dim _app As String
        Private _baseFolder As String
        Dim log As New logExecption()
        Private _connectionString As String = String.Empty
        Private em As New Email()
        Private _SMTPServer As String = String.Empty
        Private _FromMailAddress As String = String.Empty
        Private _ToMailAddress As String = String.Empty
        Private _Input As String = String.Empty
        Private _Failed As String = String.Empty
        Private _Success As String = String.Empty
        Private _Duplicate As String = String.Empty
        Private _FileFilter As String = String.Empty


        Public Sub Dispose() Implements System.IDisposable.Dispose

            GC.SuppressFinalize(Me)

            Console.WriteLine("Object " & GetHashCode() & " disposed.")
        End Sub

        Protected Overrides Sub Finalize()
            log = Nothing
            em = Nothing
            Dispose()
            Console.WriteLine("Object " & GetHashCode() & " finalized.")
        End Sub



        Public WriteOnly Property InstanceName As String
            Set(value As String)
                _InstanceName = value
                _app = _InstanceName + ".Load837"
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


        Public Property BaseFolder() As String
            Get
                Return _baseFolder
            End Get
            Set(ByVal value As String)
                _baseFolder = value
            End Set
        End Property


        Public Property Input() As String
            Get
                Return _Input

            End Get
            Set(ByVal value As String)
                _Input = value
            End Set
        End Property


        Public Property Failed() As String
            Get
                Return _Failed

            End Get
            Set(ByVal value As String)
                _Failed = value
            End Set
        End Property

        Public Property FileFilter() As String
            Get
                Return _FileFilter

            End Get
            Set(ByVal value As String)


                _FileFilter = value


            End Set
        End Property



        Public Property Success() As String
            Get
                Return _Success

            End Get
            Set(ByVal value As String)
                _Success = value
            End Set
        End Property


        Public Property Duplicate() As String
            Get
                Return _Duplicate

            End Get
            Set(ByVal value As String)
                _Duplicate = value
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
            Dim _EDI As New ImportEDI
            Dim _functionName As String = _app + ".LoadFolder"
            ' make a reference to a directory

            Dim _inputFolder As String = _Input
            Dim _successFolder As String = _Success
            Dim _failFolder As String = Failed
            Dim _duplicateFolder As String = _Duplicate


            fp.ConnectionString = _connectionString
            val.ConnectionString = _connectionString


            Try
                Dim Directory As New IO.DirectoryInfo(_inputFolder)
                Dim DirectoryList As IO.FileInfo() = Directory.GetFiles(_FileFilter)
                Dim File As FileInfo

                'list the names of all files in the specified directory
                For Each File In DirectoryList
                    ' ListBox1.Items.Add(dra)


                    _FileName = Convert.ToString(File)
                    Console.Write(File)



                    Try


                        ' in all these sernios if we get any thing back other than 0 it was failure so move on.
                        ' it has already been logged so we just move on

                        If RE = 0 Then

                            RE = fp.SingleFile(_inputFolder + _FileName, _inputFolder + _FileName + ".edi837")

                            _FileName = _FileName + ".edi837"
                        End If


                        If RE = 0 Then
                            'check to see if the file is a duplicate
                            RE = val.byFile(_inputFolder, _FileName, "837")

                        End If



                        If RE = 0 Then
                            'parse the file 
                            RE = _EDI.ImportByFileName(_inputFolder, _FileName)
                            'RE = 0

                            _FQFN = _inputFolder + _FileName
                        End If


                        Select Case RE

                            Case Is = 0

                                Try

                                    If IO.File.Exists(_successFolder + _FileName) = True Then

                                        'they use to just delete it and over right it with this one so no clue what was in thte old one
                                        'should move both to a dulpicte folder and 
                                        ' System.IO.File.Delete(_FQFN)
                                        System.IO.File.Move(_inputFolder + _FileName, _duplicateFolder + "\" + _FileName + ".dupe-")

                                    Else

                                        System.IO.File.Move(_inputFolder + _FileName, _successFolder + "\" + _FileName)


                                    End If
                                    '    System.IO.File.Move(sFileFullPath, sDestDirPath + "\" + sFileName)

                                Catch ix As System.IO.FileNotFoundException

                                    log.ExceptionDetails(_functionName + " " + _FQFN, ix)

                                Catch ix As System.IO.FileLoadException

                                    log.ExceptionDetails(_functionName + " " + _FQFN, ix)

                                Catch ix As System.IO.PathTooLongException

                                    log.ExceptionDetails(_functionName + " " + _FQFN, ix)

                                Catch ex As System.Exception

                                    log.ExceptionDetails(_functionName + " " + _FQFN, ex)
                                End Try




                                ' failed so move it to the failed folder and its already logged

                            Case Is > 0


                                Try

                                    Try
                                        '   em.SendGenericEmail(_failFolder + _FileName, "File failure ", _functionName + " " + _FQFN)
                                    Catch ex As Exception
                                        log.ExceptionDetails(_functionName + " " + _FQFN + " Send failed email failure ", ex)
                                    End Try
                                    If IO.File.Exists(_failFolder + _FileName) = True Then

                                        'they use to just delete it and over right it with this one so no clue what was in thte old one
                                        'should move both to a dulpicte folder and 
                                        ' System.IO.File.Delete(_FQFN)
                                        System.IO.File.Move(_inputFolder + _FileName, _duplicateFolder + "\" + _FileName + ".dupe-")

                                    Else

                                        System.IO.File.Move(_inputFolder + _FileName, _failFolder + "\" + _FileName)

                                    End If

                                Catch ix As System.IO.FileNotFoundException

                                    log.ExceptionDetails(_functionName + " " + _FQFN, ix)

                                Catch ix As System.IO.FileLoadException

                                    log.ExceptionDetails(_functionName + " " + _FQFN, ix)

                                Catch ix As System.IO.PathTooLongException

                                    log.ExceptionDetails(_functionName + " " + _FQFN, ix)

                                Catch ex As System.Exception

                                    log.ExceptionDetails(_functionName + " " + _FQFN, ex)
                                End Try



                                ' dulicate so move it to the duplicate folder and its already logged
                            Case Is < 0


                                Try


                                    Try
                                        '  em.SendGenericEmail(_duplicateFolder + _FileName, "File duplicate " + Convert.ToString(RE), _functionName + " " + _FQFN)
                                    Catch ex As Exception
                                        log.ExceptionDetails(_functionName + " " + _FQFN + " Send duplicate email failure ", ex)
                                    End Try

                                    If IO.File.Exists(_duplicateFolder + _FileName) = True Then

                                        'they use to just delete it and over right it with this one so no clue what was in thte old one
                                        'should move both to a dulpicte folder and 
                                        ' System.IO.File.Delete(_FQFN)
                                        System.IO.File.Move(_inputFolder + _FileName, _duplicateFolder + "\" + _FileName + ".dupe-")

                                    Else

                                        System.IO.File.Move(_inputFolder + _FileName, _duplicateFolder + "\" + _FileName)

                                    End If


                                Catch ix As System.IO.FileNotFoundException

                                    log.ExceptionDetails(_functionName + " " + _FQFN, ix)

                                Catch ix As System.IO.FileLoadException

                                    log.ExceptionDetails(_functionName + " " + _FQFN, ix)

                                Catch ix As System.IO.PathTooLongException

                                    log.ExceptionDetails(_functionName + " " + _FQFN, ix)

                                Catch ex As System.Exception

                                    log.ExceptionDetails(_functionName + " " + _FQFN, ex)
                                End Try


                                ' who know what happend should never ever get here 
                            Case Else

                                log.ExceptionDetails(_functionName + " " + _FQFN, "boken")


                        End Select



                    Catch ex As Exception

                        log.ExceptionDetails(_app, ex)

                    End Try

                Next



            Catch ix As System.IO.DriveNotFoundException


                ' add log and email here
                log.ExceptionDetails(_app, ix)

            Catch ix As System.IO.DirectoryNotFoundException

                ' add log and email here

                log.ExceptionDetails(_app, ix)
            Catch ix As System.IO.PathTooLongException

                ' add log and email here

                log.ExceptionDetails(_app, ix)

            Catch ex As Exception

                log.ExceptionDetails(_app, ex)


            End Try


            Return RE
        End Function





    End Class

End Namespace

