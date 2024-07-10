Option Explicit On


Imports System.Text.RegularExpressions
Imports System.Collections.Generic
Imports DCSGlobal.BusinessRules.Logging
Imports System.IO
Imports System.Text
Imports System.Threading
Imports DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff

Namespace DCSGlobal.EDI



    Public Class FilePrep
        Implements IDisposable


        Dim FileGuid As Guid = Guid.Empty
        Dim _Path As String = String.Empty
        Dim oD As New Declarations
        Dim ss As New StringStuff
        Dim log As New logExecption

        Dim _Verbose As Boolean = False



        Public Sub Dispose() Implements System.IDisposable.Dispose

            GC.SuppressFinalize(Me)

            'Console.WriteLine("Object " & GetHashCode() & " disposed.")
        End Sub

        Protected Overrides Sub Finalize()

            log = Nothing
            ss = Nothing
            oD = Nothing


            Dispose()
            ' Console.WriteLine("Object " & GetHashCode() & " finalized.")
        End Sub

        Public WriteOnly Property DirPath As String
            Set(value As String)
                _Path = value
            End Set
        End Property


        Public WriteOnly Property ConnectionString As String
            Set(value As String)
                log.ConnectionString = value
            End Set
        End Property

        Public WriteOnly Property Verbose As Boolean
            Set(value As Boolean)
                _Verbose = value
            End Set
        End Property


        Public Function SingleFile(ByVal FileToFix As String, ByVal FixedFileName As String, ByVal ValidationMethod As Int32) As Int32

            Dim r As Int32


            r = FileFix(FileToFix, FixedFileName)





            Return r
        End Function


        Public Function SingleFile(ByVal FileToFix As String, ByVal FixedFileName As String) As Int32

            Dim r As Int32


            r = FileFix(FileToFix, FixedFileName)

            If r = 0 Then
                File.Delete(FileToFix)
                File.Move(FixedFileName, FileToFix)


            End If

            Return r
        End Function



        Public Function SingleFile(ByVal FileToFix As String) As Int32
            Dim FixedFileName As String = String.Empty

            Dim r As Int32 = 0
            FileGuid = Guid.NewGuid



            '   _Path = Path.GetFullPath(FileToFix)

            _Path = Path.GetDirectoryName(FileToFix)



            FixedFileName = _Path + "\" + Convert.ToString(FileGuid) + ".tmp"


            r = FileFix(FileToFix, FixedFileName)

            If r = 0 Then


                File.Delete(FileToFix)
                File.Move(FixedFileName, FileToFix)


            End If






            Return r
        End Function

        Public Function Folder(ByVal FolderName As String) As Int32


            Return 0
        End Function



        Private Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
            target = value
            Return value
        End Function


        Private Function FileToList(ByVal FileToLoad As String) As Int32


            Dim line As String = String.Empty
            Dim re As Int32

            Dim cr As Integer = 0


            If Not File.Exists(FileToLoad) Then

                log.ExceptionDetails("DCSGlobal.EDI.FiletoList", "does not  Exists " + FileToLoad)
                Return 2
                Exit Function
            End If

            Try


                Using r As New StreamReader(FileToLoad)
                    Dim buffer As Char() = New Char(1023) {}
                    Dim read As Integer

                    While (InlineAssignHelper(read, r.ReadBlock(buffer, 0, buffer.Length))) > 0

                        For i As Integer = 0 To read - 1
                            If Convert.ToString(buffer(i)) = "~" Then
                                If Convert.ToString(buffer(i)) <> vbCr AndAlso Convert.ToString(buffer(i)) <> vbLf Then
                                    line = line & Convert.ToString(buffer(i))
                                End If

                                If Not line = String.Empty Then

                                    line = ss.StripCRLF(line)

                                    If cr = 0 Then
                                        cr = 1
                                        'sw.Write(line)
                                    Else
                                        ' sw.Write(vbCrLf + line)
                                    End If




                                    line = String.Empty
                                End If

                            Else
                                line = line & Convert.ToString(buffer(i))
                            End If
                        Next

                    End While

                End Using



                re = 0

            Catch ex As Exception
                log.ExceptionDetails("DCSGlobal.EDI.FileToList", ex)
                re = 2

            End Try

            Return re


        End Function


        Private Function FileFix(ByVal FileToFix As String, ByVal FixedFileName As String) As Int32


            Dim line As String = String.Empty
            Dim re As Int32

            Dim cr As Integer = 0


            If Not File.Exists(FixedFileName) Then
                Using sw As StreamWriter = File.CreateText(FixedFileName)
                End Using
            Else
                log.ExceptionDetails("DCSGlobal.EDI.FileFix.Go", " Exists " + FixedFileName)
                Return 2
                Exit Function
            End If

            Try
                Using sw As StreamWriter = File.AppendText(FixedFileName)

                    Using r As New StreamReader(FileToFix)
                        Dim buffer As Char() = New Char(1023) {}
                        Dim read As Integer

                        While (InlineAssignHelper(read, r.ReadBlock(buffer, 0, buffer.Length))) > 0

                            For i As Integer = 0 To read - 1
                                If Convert.ToString(buffer(i)) = "~" Then
                                    If Convert.ToString(buffer(i)) <> vbCr AndAlso Convert.ToString(buffer(i)) <> vbLf Then
                                        line = line & Convert.ToString(buffer(i))
                                    End If

                                    If Not line = String.Empty Then

                                        line = ss.StripCRLF(line)

                                        If cr = 0 Then
                                            cr = 1
                                            sw.Write(line)
                                        Else
                                            sw.Write(vbCrLf + line)
                                        End If




                                        line = String.Empty
                                    End If

                                Else
                                    line = line & Convert.ToString(buffer(i))
                                End If
                            Next

                        End While

                    End Using

                End Using

                re = 0

            Catch ex As Exception
                log.ExceptionDetails("DCSGlobal.EDI.FileFix.Go", ex)
                re = 2

            End Try

            Return re


        End Function


    End Class


End Namespace
