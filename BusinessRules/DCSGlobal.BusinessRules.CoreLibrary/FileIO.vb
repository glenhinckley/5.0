Imports System
Imports System.IO
Imports System.Threading
Imports System.Runtime.InteropServices
Imports System.Runtime.Remoting.Messaging
Imports System.Security.Permissions
Imports Microsoft.Win32.SafeHandles


Namespace IO


    Public Class FileIO



        Public Function MoveFile(ByVal SourceFile As String, ByVal DestinationFile As String) As Int32
            Dim r As Integer = -1

            '// To move a file or folder to a new location:
            System.IO.File.Move(SourceFile, DestinationFile)

            '// To move an entire directory. To programmatically modify or combine
            '// path strings, use the System.IO.Path class.
            '  System.IO.Directory.Move(@"C:\Users\Public\public\test\", @"C:\Users\Public\private");

            Return r

        End Function


        Public Function MoveDirectory(ByVal SourceDirectory As String, ByVal DestinationDirectory As String) As Int32

            Dim r As Integer = -1
            '// To move a file or folder to a new location:
            'System.IO.File.Move(SourceFile, DestinationFile)

            '// To move an entire directory. To programmatically modify or combine
            '// path strings, use the System.IO.Path class.
            System.IO.Directory.Move(SourceDirectory, DestinationDirectory)

            Return r

        End Function


        Public Function CheckExtension(ByVal File As String, ByVal FileMask As String) As Integer

            Dim r As Integer = -1
            Dim _FileEXT As String = String.Empty
            Dim ss As New StringHandlingStuff.StringStuff
            Dim EXT As String = String.Empty

            Try
                _FileEXT = ss.ParseDemlimtedStringEDI(FileMask, ".", 2)

                EXT = Path.GetExtension(File)


                EXT = EXT.Replace(".", "")


                If EXT = _FileEXT Then
                    r = 0
                Else
                    r = 1
                End If

                If _FileEXT = "*" Then
                    r = 1
                End If

            Catch ex As Exception
                r = -1
            End Try



            Return r



        End Function





        Public Function DeleteFile(ByVal FileToDelete As String) As Int32


            Dim r As Integer = -1

            If (System.IO.File.Exists(FileToDelete)) Then

                '// Use a try block to catch IOExceptions, to
                '// handle the case of the file already being
                ' // opened by another process.
                Try

                    System.IO.File.Delete(FileToDelete)

                Catch e As System.IO.IOException

                    Console.WriteLine(e.Message)
                    'return;
                End Try

            End If
            Return r

        End Function

        Public Function SaveTextToFile(ByVal strData As String, ByVal FullPath As String, Optional ByVal ErrInfo As String = "") As Boolean
            ' Dim Contents As String
            Dim bAns As Boolean = False
            Dim objWriter As StreamWriter
            Dim sToday As String

            Dim y As String = String.Empty
            Dim m As String = String.Empty
            Dim d As String = String.Empty
            Dim h As String = String.Empty
            Dim mm As String = String.Empty




            If (Now.Month < 10) Then
                m = "0" + Now.Month.ToString
            Else
                m = Now.Month.ToString
            End If

            If (Now.Day < 10) Then
                d = "0" + Now.Day.ToString
            Else
                d = Now.Day.ToString
            End If

            Try
                sToday = Now.Year.ToString + m + d
                FullPath = FullPath.Replace(".txt", sToday + ".txt")
                objWriter = New StreamWriter(FullPath, True)
                objWriter.WriteLine(strData)
                objWriter.Close()
                bAns = True
            Catch Ex As Exception
                bAns = False 'subba-040810
                ErrInfo = Ex.Message
            End Try
            Return bAns
        End Function

        Public Function GetFileContents(ByVal FullPath As String, Optional ByRef ErrInfo As String = "") As String

            Dim strContents As String = String.Empty
            Dim objReader As StreamReader
            Try
                objReader = New StreamReader(FullPath)
                strContents = objReader.ReadToEnd()
                objReader.Close()

            Catch Ex As Exception
                ErrInfo = Ex.Message
            End Try

            Return strContents


        End Function

    End Class



End Namespace



