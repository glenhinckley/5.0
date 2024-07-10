
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


Namespace ListBuilder


    Public Class ListBuilder
        Implements IDisposable

        Private _List As New List(Of String)
        Private ss As New StringHandlingStuff.StringStuff()
        Private _ERR As Exception

        Public Sub Dispose() Implements System.IDisposable.Dispose

            GC.SuppressFinalize(Me)

            ' Console.WriteLine("Object " & GetHashCode() & " disposed.")
        End Sub


        Protected Overrides Sub Finalize()

            ss = Nothing
            _List = Nothing
            Dispose()
            ' Console.WriteLine("Object " & GetHashCode() & " finalized.")
        End Sub

        Public ReadOnly Property ERR As Exception
            Get
                Return _ERR
            End Get
        End Property


        Public ReadOnly Property TheList As List(Of String)
            Get
                Return _List
            End Get
        End Property


        Private Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
            target = value
            Return value
        End Function

        ''' <summary>
        ''' list from String 
        ''' </summary>
        ''' <returns>0 on success -x on fail  the list will be in the EDIList Property</returns>
        ''' <remarks></remarks>
        Public Function BuildListByString(ByVal TheString As String) As Integer

            Dim line As String = String.Empty
            Dim x As Int32 = -1

            Dim cr As Integer = 0


            _List.Clear()
            Try

                Using r As New StringReader(TheString)
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
                                    _List.Add(line)
                                    line = String.Empty

                                End If

                            Else
                                line = line & Convert.ToString(buffer(i))
                            End If
                        Next

                    End While
                    x = 0
                End Using
            Catch ex As Exception
                _ERR = ex
            End Try

            Return x

        End Function


        ''' <summary>
        ''' list from a file
        ''' </summary>
        ''' <returns>0 on success -x on fail  the list will be in the EDIList Property</returns>
        ''' <remarks></remarks>
        Public Function BuildListByFile(ByVal FileName As String) As Integer

            Dim line As String = String.Empty
            Dim x As Int32 = -1

            Dim cr As Integer = 0


            _List.Clear()
            Try

                Using r As New StreamReader(FileName)
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
                                    _List.Add(line)
                                    line = String.Empty

                                End If

                            Else
                                line = line & Convert.ToString(buffer(i))
                            End If
                        Next

                    End While

                End Using
                x = 0


            Catch ex As Exception
                _ERR = ex
            End Try

            Return x

        End Function





    End Class


End Namespace