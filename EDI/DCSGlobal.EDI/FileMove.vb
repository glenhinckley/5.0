
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



    Public Class FileMove


        Implements IDisposable

        Private log As New logExecption()
        Private em As New Email()
        Private ss As New StringStuff()

        Private _InstanceName As String = String.Empty
        Dim _app As String
        Private _baseFolder As String

        Private _connectionString As String = String.Empty

        Private _SMTPServer As String = String.Empty
        Private _FromMailAddress As String = String.Empty
        Private _ToMailAddress As String = String.Empty
        Private _Input As String = String.Empty
        Private _Failed As String = String.Empty
        Private _Success As String = String.Empty
        Private _Duplicate As String = String.Empty
        Private _ParseTree As Int32 = 0
        Private _FinalPath As String = String.Empty

        Private _functionName As String = String.Empty







        Public Sub Dispose() Implements System.IDisposable.Dispose

            GC.SuppressFinalize(Me)

            Console.WriteLine("Object " & GetHashCode() & " disposed.")
        End Sub

        Protected Overrides Sub Finalize()
            log = Nothing
            em = Nothing
            ss = Nothing
            Dispose()
            Console.WriteLine("Object " & GetHashCode() & " finalized.")
        End Sub



        Public WriteOnly Property InstanceName As String
            Set(value As String)
                _InstanceName = value
                _functionName = value + ".FileMove"
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





        Public Property FinalPath() As String
            Get
                Return _FinalPath

            End Get
            Set(ByVal value As String)


                If (ss.GetLastChr(value) <> "\") Then
                    value = value + "\"
                End If
                _FinalPath = value

            End Set
        End Property


        Public Property Input() As String
            Get
                Return _Input

            End Get
            Set(ByVal value As String)


                If (ss.GetLastChr(value) <> "\") Then
                    value = value + "\"
                End If
                _Input = value

            End Set
        End Property


        Public Property Failed() As String
            Get
                Return _Failed

            End Get
            Set(ByVal value As String)

                If (ss.GetLastChr(value) <> "\") Then
                    value = value + "\"
                End If
                _Failed = value


            End Set
        End Property





        Public Property Success() As String
            Get
                Return _Success

            End Get
            Set(ByVal value As String)

                If (ss.GetLastChr(value) <> "\") Then
                    value = value + "\"
                End If
                _Success = value

            End Set
        End Property


        Public Property Duplicate() As String
            Get
                Return _Duplicate

            End Get
            Set(ByVal value As String)

                If (ss.GetLastChr(value) <> "\") Then
                    value = value + "\"
                End If
                _Duplicate = value

            End Set
        End Property


        Public Property ParseTree() As Int32

            Get
                Return _ParseTree

            End Get
            Set(ByVal value As Int32)
                _ParseTree = value
            End Set
        End Property





        Public Function Move(ByVal RE As Integer, ByVal FileName As String) As Int32

            Dim _FQFN As String = _Input + FileName
   
            Dim r As Integer = 0


            Select Case RE

                Case Is = 0

                    Try

                        If IO.File.Exists(_Success + FileName) = True Then

                            'they use to just delete it and over right it with this one so no clue what was in thte old one
                            'should move both to a dulpicte folder and 
                            ' System.IO.File.Delete(_FQFN)
                            System.IO.File.Move(_Input + FileName, _Duplicate + FileName + ".dupe-")
                            _FinalPath = _Duplicate
                        Else

                            System.IO.File.Move(_Input + FileName, _Success + FileName)
                            _FinalPath = _Success

                        End If
                        '    System.IO.File.Move(sFileFullPath, sDestDirPath + "\" + sFileName)

                    Catch ix As System.IO.FileNotFoundException
                        r = -1
                        log.ExceptionDetails(_functionName + " " + _FQFN, ix)

                    Catch ix As System.IO.FileLoadException
                        r = -1
                        log.ExceptionDetails(_functionName + " " + _FQFN, ix)

                    Catch ix As System.IO.PathTooLongException
                        r = -1
                        log.ExceptionDetails(_functionName + " " + _FQFN, ix)

                    Catch ex As System.Exception
                        r = -1
                        log.ExceptionDetails(_functionName + " " + _FQFN, ex)
                    End Try




                    ' failed so move it to the failed folder and its already logged

                Case Is < 0


                    Try

                        Try
                            '   em.SendGenericEmail(_failFolder + _FileName, "File failure ", _functionName + " " + _FQFN)
                        Catch ex As Exception
                            r = -1
                            log.ExceptionDetails(_functionName + " " + _FQFN + " Send failed email failure ", ex)
                        End Try
                        If IO.File.Exists(_Failed + FileName) = True Then

                            'they use to just delete it and over right it with this one so no clue what was in thte old one
                            'should move both to a dulpicte folder and 
                            ' System.IO.File.Delete(_FQFN)
                            System.IO.File.Move(_Input + FileName, _Duplicate + FileName + ".dupe-")
                            _FinalPath = _Duplicate
                        Else

                            System.IO.File.Move(_Input + FileName, _Failed + FileName)
                            _FinalPath = _Failed
                        End If

                    Catch ix As System.IO.FileNotFoundException
                        r = -1
                        log.ExceptionDetails(_functionName + " " + _FQFN, ix)

                    Catch ix As System.IO.FileLoadException
                        r = -1
                        log.ExceptionDetails(_functionName + " " + _FQFN, ix)

                    Catch ix As System.IO.PathTooLongException
                        r = -1
                        log.ExceptionDetails(_functionName + " " + _FQFN, ix)

                    Catch ex As System.Exception
                        r = -1
                        log.ExceptionDetails(_functionName + " " + _FQFN, ex)
                    End Try



                    ' dulicate so move it to the duplicate folder and its already logged
                Case Is > 0


                    Try


                        Try
                            '  em.SendGenericEmail(_duplicateFolder + _FileName, "File duplicate " + Convert.ToString(RE), _functionName + " " + _FQFN)
                        Catch ex As Exception
                            log.ExceptionDetails(_functionName + " " + _FQFN + " Send duplicate email failure ", ex)
                        End Try

                        If IO.File.Exists(_Duplicate + FileName) = True Then

                            'they use to just delete it and over right it with this one so no clue what was in thte old one
                            'should move both to a dulpicte folder and 
                            ' System.IO.File.Delete(_FQFN)
                            System.IO.File.Move(_Input + FileName, _Duplicate + FileName + ".dupe-")
                            _FinalPath = _Duplicate
                        Else

                            System.IO.File.Move(_Input + FileName, _Duplicate + FileName)
                            _FinalPath = _Duplicate
                        End If


                    Catch ix As System.IO.FileNotFoundException
                        r = -1
                        log.ExceptionDetails(_functionName + " " + _FQFN, ix)

                    Catch ix As System.IO.FileLoadException
                        r = -1
                        log.ExceptionDetails(_functionName + " " + _FQFN, ix)

                    Catch ix As System.IO.PathTooLongException
                        r = -1
                        log.ExceptionDetails(_functionName + " " + _FQFN, ix)

                    Catch ex As System.Exception
                        r = -1
                        log.ExceptionDetails(_functionName + " " + _FQFN, ex)
                    End Try


                    ' who know what happend should never ever get here 
                Case Else
                    r = -1
                    log.ExceptionDetails(_functionName + " " + _FQFN, "boken")


            End Select


            Return r
        End Function


        Public Function Move(ByVal RE As Integer, ByVal FileName As String, ByVal FilePath As String) As Int32


            Dim _FQFN As String = FilePath + _Input + FileName
 
            Select Case RE

                Case Is = 0

                    Try

                        If IO.File.Exists(FilePath + _Success + FileName) = True Then

                            'they use to just delete it and over right it with this one so no clue what was in thte old one
                            'should move both to a dulpicte folder and 
                            ' System.IO.File.Delete(_FQFN)
                            System.IO.File.Move(FilePath + _Input + FileName, FilePath + _Duplicate + FileName + ".dupe-")

                        Else

                            System.IO.File.Move(FilePath + _Input + FileName, FilePath + _Success + FileName)


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
                            '   em.SendGenericEmail(_failFolder + Filename, "File failure ", _functionName + " " + _FQFN)
                        Catch ex As Exception
                            log.ExceptionDetails(_functionName + " " + _FQFN + " Send failed email failure ", ex)
                        End Try
                        If IO.File.Exists(FilePath + _Failed + FileName) = True Then

                            'they use to just delete it and over right it with this one so no clue what was in thte old one
                            'should move both to a dulpicte folder and 
                            ' System.IO.File.Delete(_FQFN)
                            System.IO.File.Move(FilePath + _Input + FileName, FilePath + _Duplicate + FileName + ".dupe-")

                        Else

                            System.IO.File.Move(FilePath + _Input + FileName, FilePath + _Failed + FileName)

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
                            '  em.SendGenericEmail(_Duplicate + Filename, "File duplicate " + Convert.ToString(RE), _functionName + " " + _FQFN)
                        Catch ex As Exception
                            log.ExceptionDetails(_functionName + " " + _FQFN + " Send duplicate email failure ", ex)
                        End Try

                        If IO.File.Exists(FilePath + _Duplicate + FileName) = True Then

                            'they use to just delete it and over right it with this one so no clue what was in thte old one
                            'should move both to a dulpicte folder and 
                            ' System.IO.File.Delete(_FQFN)
                            System.IO.File.Move(FilePath + _Input + FileName, FilePath + _Duplicate + FileName + ".dupe-")

                        Else

                            System.IO.File.Move(FilePath + _Input + FileName, FilePath + _Duplicate + FileName)

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


            Return 0
        End Function

    End Class

End Namespace

