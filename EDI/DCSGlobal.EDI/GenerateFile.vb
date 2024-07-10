Option Explicit On
Option Strict On
Imports DCSGlobal.BusinessRules.CoreLibrary
Imports System.Text.RegularExpressions
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Collections.Generic
'Imports System.Data.DataSetExtensions
Imports DCSGlobal.BusinessRules.Logging
Imports System.IO
Imports System.Text
Imports System.Threading
Imports DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff

Namespace DCSGlobal.EDI

    Public Class GenerateFile
        Implements IDisposable

        Private log As New logExecption
        Private _ConnectionString As String = String.Empty
        Private _ProcessStartTime As DateTime = Now
        Private _ProcessEndTime As DateTime = Now
        Private _SP_SET_PROCESS_STATUS As String = String.Empty
        Private _SP_NAME_UPDATE As String = String.Empty

        Dim FileGuid As Guid = Guid.Empty
        Dim _Path As String = String.Empty
        Dim _Content As String = String.Empty
        Dim _USP_GET_ROWS As String = String.Empty
        Dim _usp_set_status As String = String.Empty
        Dim _FileName As String = String.Empty
        Dim _ARCHIVE_FOLDER As String = String.Empty
        Dim _Move_FileName As String = String.Empty

        Dim oD As New Declarations
        Dim ss As New StringStuff
        Dim _TaskID As Long
        Dim _ID As Long = 0
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

 

        Public WriteOnly Property Verbose As Boolean
            Set(value As Boolean)
                _Verbose = value
            End Set
        End Property
        Public WriteOnly Property SP_SET_PROCESS_STATUS As String

            Set(value As String)

                _SP_SET_PROCESS_STATUS = value

            End Set
        End Property
        Public WriteOnly Property USP_GET_ROWS As String
            Set(value As String)
                _USP_GET_ROWS = value
            End Set
        End Property
        Public WriteOnly Property usp_set_status As String
            Set(value As String)
                _usp_set_status = value
            End Set
        End Property

        Public WriteOnly Property ID As Long

            Set(value As Long)
                _ID = value

            End Set
        End Property

        Public WriteOnly Property SP_NAME_UPDATE As String
            Set(value As String)
                _SP_NAME_UPDATE = value
            End Set
        End Property

        Public WriteOnly Property Content As String
            Set(value As String)
                _Content = value
            End Set
        End Property
        Public WriteOnly Property FileName As String
            Set(value As String)
                _FileName = value
            End Set
        End Property
        Public WriteOnly Property ARCHIVE_FOLDER As String
            Set(value As String)
                _ARCHIVE_FOLDER = value
            End Set
        End Property

        Public WriteOnly Property ProcessStartTime As DateTime

            Set(value As DateTime)

                _ProcessStartTime = value

            End Set
        End Property

        Public WriteOnly Property ProcessEndTime As DateTime

            Set(value As DateTime)

                _ProcessEndTime = value

            End Set
        End Property



        Public WriteOnly Property ConnectionString As String

            Set(value As String)

                log.ConnectionString = value
                _ConnectionString = value

            End Set
        End Property


        
        Public Function SingleFile(ByVal FileToFix As String) As Int32
            Dim FixedFileName As String = String.Empty

            Dim r As Int32 = 0
            FileGuid = Guid.NewGuid

            '   _Path = Path.GetFullPath(FileToFix)

            _Path = Path.GetDirectoryName(FileToFix)

            FixedFileName = _Path + Convert.ToString(FileGuid) + ".tmp"

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

        Public Function Go() As Int32
            'Dim r As Integer = 0
            Dim line As String = String.Empty
            Dim re As Int32

            Dim cr As Integer = 0
            Dim array() As Byte
            Dim rSSP As Integer


            _TaskID = 0
            rSSP = 0
            _TaskID = Convert.ToInt32(_ID)
            If _Verbose = True Then
                Console.WriteLine("Taskid:" + Convert.ToString(_TaskID) + "Task started")
            End If

            array = Encoding.UTF8.GetBytes(_Content)
            Dim s As New MemoryStream(array)

            _Move_FileName = String.Empty
            _Move_FileName = Path.GetFileName(_FileName)


            Using sf As New DCSGlobal.EDI.GenerateFile()
                sf.ConnectionString = _ConnectionString
                sf.SP_SET_PROCESS_STATUS = _SP_NAME_UPDATE
                rSSP = sf.SetFlag("0", Convert.ToInt32(_ID))
                If _Verbose = True Then
                    If rSSP = 0 Then
                        Console.WriteLine("Taskid:" + Convert.ToString(_TaskID) + "Updated Flag from I to " + Convert.ToString(rSSP) + " Completed.")
                    Else
                        Console.WriteLine("Taskid:" + Convert.ToString(_TaskID) + "Updated Flag from I to  " + Convert.ToString(rSSP) + " Completed.")
                    End If
                End If
            End Using

            File.WriteAllText(_FileName, _Content)
 
            'If File.Exists(_FileName) Then
            '    File.Delete(_Move_FileName)
            '    Using sw As StreamWriter = File.CreateText(_FileName)
            '    End Using
            'Else
            '    Using sw As StreamWriter = File.CreateText(_FileName)
            '    End Using
            'End If
            'If Not File.Exists(_FileName) Then
            '    Using sw As StreamWriter = File.CreateText(_FileName)
            '    End Using
            'Else
            '    If File.Exists(_ARCHIVE_FOLDER + _Move_FileName) Then
            '        File.Delete(_ARCHIVE_FOLDER + _Move_FileName)
            '    End If
            '    File.Move(_FileName, _ARCHIVE_FOLDER + _Move_FileName)
            '    log.ExceptionDetails("DCSGlobal.EDI.Generate.Go", " Exists " + _FileName + " Moved to " + _ARCHIVE_FOLDER)
            '    'Return 2
            '    'Exit Function
            'End If

            Try


                'Using sw As StreamWriter = File.AppendText(_FileName)

                '    Using r As New StreamReader(s)
                '        Dim buffer As Char() = New Char(1023) {}
                '        Dim read As Integer

                '        While (InlineAssignHelper(read, r.ReadBlock(buffer, 0, buffer.Length))) > 0
                '            For i As Integer = 0 To read - 1
                '                If Convert.ToString(buffer(i)) = "~" Then
                '                    If Convert.ToString(buffer(i)) <> vbCr AndAlso Convert.ToString(buffer(i)) <> vbLf Then
                '                        line = line & Convert.ToString(buffer(i))
                '                    End If

                '                    If Not line = String.Empty Then

                '                        line = ss.StripCRLF(line)

                '                        If cr = 0 Then
                '                            cr = 1
                '                            sw.Write(line)
                '                        Else
                '                            sw.Write(vbCrLf + line)
                '                        End If
                '                        line = String.Empty
                '                    End If

                '                Else
                '                    line = line & Convert.ToString(buffer(i))
                '                End If
                '            Next
                '        End While
                '    End Using
                'End Using
                re = 0
                If _Verbose = True Then
                    Console.WriteLine("Taskid:" + Convert.ToString(_TaskID) + "Writing Edi Content into File Completed. ")
                End If
                Using sf As New DCSGlobal.EDI.GenerateFile()
                    sf.ConnectionString = _ConnectionString
                    sf.SP_SET_PROCESS_STATUS = _SP_NAME_UPDATE
                    rSSP = sf.SetFlag("P", Convert.ToInt32(_ID))
                    If _Verbose = True Then
                        Console.WriteLine("Taskid:" + Convert.ToString(_TaskID) + "Updated Processed Flag to P.")
                        Console.WriteLine("Taskid:" + Convert.ToString(_TaskID) + "Completed. ")
                    End If
                End Using
            Catch ex As Exception
                If _Verbose = True Then
                    Console.WriteLine("Taskid:" + Convert.ToString(_TaskID) + "Writing Edi Content into File Failed. " + ex.Message)
                    Console.WriteLine("Taskid:" + Convert.ToString(_TaskID) + "Failed. ")
                End If
                log.ExceptionDetails("DCSGlobal.EDI.FileFix.Go", ex)
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


        Public Function SetFlag(ByVal StatusFlag As String, ByVal ID As Int32) As Int32
            Dim r As Integer = -1

            Try
                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()


                    '' _sqlGetREQ = "Select REQUEST from REQ where id " + Convert.ToString(_id)


                    Using cmd As New SqlCommand(_SP_SET_PROCESS_STATUS, Con)
                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@ID", ID)
                        cmd.Parameters.AddWithValue("@status", StatusFlag)
                        cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using

                r = 0
            Catch sx As SqlException

                log.ExceptionDetails("SetFlag I for ID " + Convert.ToString(ID), sx)


            Catch ex As Exception

                log.ExceptionDetails("SetFlag I for ID " + Convert.ToString(ID), ex)

            End Try

            Return r
        End Function

 
        'Public Function SingleFile(ByVal FileToFix As String, ByVal FixedFileName As String, ByVal ValidationMethod As Int32) As Int32

        '    Dim r As Int32


        '    r = FileFix(FileToFix, FixedFileName)





        '    Return r
        'End Function


        'Public Function SingleFile(ByVal FileToFix As String, ByVal FixedFileName As String) As Int32

        '    Dim r As Int32


        '    r = FileFix(FileToFix, FixedFileName)

        '    If r = 0 Then
        '        File.Delete(FileToFix)
        '        File.Move(FixedFileName, FileToFix)


        '    End If

        '    Return r
        'End Function


    End Class


End Namespace
