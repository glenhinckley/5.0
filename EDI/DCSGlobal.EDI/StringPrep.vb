Option Explicit On
Option Strict On
Option Compare Binary


Imports System.Text.RegularExpressions
Imports System.Collections.Generic
Imports DCSGlobal.BusinessRules.Logging
Imports System.IO
Imports System.Text
Imports System.Threading
Imports DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff



Namespace DCSGlobal.EDI



    Public Class StringPrep

        Implements IDisposable








        Dim FileGuid As Guid = Guid.Empty


        Dim l As New List(Of String)


        Dim BasePath As String = "c:\\usr\\fixedfiles\\"

        Dim oD As New Declarations

        Dim log As New logExecption
        Dim V As Boolean = False

        Dim ss As New StringStuff()

        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                    log = Nothing
                    oD = Nothing
                    ss = Nothing
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

        Public Sub Dispose() Implements System.IDisposable.Dispose

            GC.SuppressFinalize(Me)

            'Console.WriteLine("Object " & GetHashCode() & " disposed.")
        End Sub

        Protected Overrides Sub Finalize()


            Dispose()
            'Console.WriteLine("Object " & GetHashCode() & " finalized.")
        End Sub


        Public WriteOnly Property ConnectionString As String
            Set(value As String)
                log.ConnectionString = value
            End Set
        End Property

        Public WriteOnly Property Debug As Boolean
            Set(value As Boolean)
                V = value
            End Set
        End Property


        Public Function SingleEDI(ByVal EDI As String) As List(Of String)

            Dim r As Int32


            r = StringFix(EDI)
            If r <> 0 Then

                l.Clear()
                l.Add(Convert.ToString(r))
            End If

            Return l

        End Function

        Public Function SingleEDIFile(ByVal FixedFileName As String) As List(Of String)

            Dim r As Int32


            r = EDIFile(FixedFileName)

            If r <> 0 Then

                l.Clear()
                l.Add(Convert.ToString(r))
            End If

            Return l


        End Function






        Private Function EDIFile(ByVal FixedFileName As String) As Integer

            Dim line As String = String.Empty
            Dim re As Int32

            Dim cr As Integer = 0


            l.Clear()

            If Not File.Exists(FixedFileName) Then
                Using sw As StreamWriter = File.CreateText(FixedFileName)
                End Using
            Else
                log.ExceptionDetails("DCSGlobal.EDI.FileFix.Go", " Exists " + FixedFileName)
                Return -1
                Exit Function
            End If

            Try


                Using r As New StreamReader(FixedFileName)
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
                                    l.Add(line)
                                    line = String.Empty

                                End If

                            Else
                                line = line & Convert.ToString(buffer(i))
                            End If
                        Next

                    End While

                End Using
            Catch ex As Exception
                log.ExceptionDetails("DCSGlobal.EDI.FileFix.Go", ex)

            End Try

            Return -1

        End Function


        Private Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
            target = value
            Return value
        End Function

        Private Function StringFix(ByVal EDIString As String) As Int32

            Dim line As String = String.Empty
            Dim re As Int32

            Dim cr As Integer = 0
            l.Clear()

            'If Not File.Exists(FixedFileName) Then
            '    Using sw As StreamWriter = File.CreateText(FixedFileName)
            '    End Using
            'Else
            '    log.ExceptionDetails("DCSGlobal.EDI.FileFix.Go", " Exists " + FixedFileName)
            '    Return 1
            '    Exit Function
            'End If

            Dim array() As Byte

            'take the EDI string and put it in a byte array
            array = Encoding.UTF8.GetBytes(EDIString)
            '/ convert string to stream
            'byte[] byteArray = Encoding.UTF8.GetBytes(contents)
            '//byte[] byteArray = Encoding.ASCII.GetBytes(contents);
            'MemoryStream stream = new MemoryStream(byteArray);





            ' pass the bytearray to the memor stram class so we can pas it to the chunckler 
            ' this not a clue why i did it this was got to be a better way oh well its done now




            Dim s As New MemoryStream(array)




            Try


                Using r As New StreamReader(s)
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
                                    l.Add(line)
                                    line = String.Empty

                                End If

                            Else
                                line = line & Convert.ToString(buffer(i))
                            End If
                        Next

                    End While

                End Using



                Return 0

            Catch ex As Exception
                log.ExceptionDetails("DCSGlobal.EDI.FileFix.Go", ex)
                Return 2

            End Try


        End Function





    End Class


End Namespace
