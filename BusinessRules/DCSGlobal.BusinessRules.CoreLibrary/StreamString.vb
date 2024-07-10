Imports System.IO
Imports System.IO.Pipes
Imports System.Security.Principal
Imports System

Imports System.Text


Namespace NPStream

    Public Class StreamString
        Private ioStream As Stream
        Private streamEncoding As UnicodeEncoding

        Public Sub New(ioStream As Stream)
            Me.ioStream = ioStream
            streamEncoding = New UnicodeEncoding(False, False)
        End Sub

        Public Function ReadString() As String
            Dim len As Integer = 0
            len = CType(ioStream.ReadByte(), Integer) * 256
            len += CType(ioStream.ReadByte(), Integer)
            Dim inBuffer As Array = Array.CreateInstance(GetType(Byte), len)
            ioStream.Read(inBuffer, 0, len)

            Return streamEncoding.GetString(inBuffer)
        End Function

        Public Function WriteString(outString As String) As Integer
            Dim outBuffer() As Byte = streamEncoding.GetBytes(outString)
            Dim len As Integer = outBuffer.Length
            If len > UInt16.MaxValue Then
                len = CType(UInt16.MaxValue, Integer)
            End If
            ioStream.WriteByte(CType(len \ 256, Byte))
            ioStream.WriteByte(CType(len And 255, Byte))
            ioStream.Write(outBuffer, 0, outBuffer.Length)
            ioStream.Flush()

            Return outBuffer.Length + 2
        End Function


    End Class
End Namespace
