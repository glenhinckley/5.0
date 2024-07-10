
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








    Public Class EDIListBuilder
        Implements IDisposable





        Private _EDIList As New List(Of String)

        Private oD As New Declarations

        Private log As New logExecption()
        Private em As New Email()
        Private ss As New StringStuff()


        Private _AppName As String = "DCS.EDI.EDIListBuilder."


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

        Private _functionName As String = String.Empty




        Public Sub Dispose() Implements System.IDisposable.Dispose

            GC.SuppressFinalize(Me)

            ' Console.WriteLine("Object " & GetHashCode() & " disposed.")
        End Sub


        Protected Overrides Sub Finalize()
            log = Nothing
            em = Nothing
            ss = Nothing
            _EDIList = Nothing
            Dispose()
            ' Console.WriteLine("Object " & GetHashCode() & " finalized.")
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


        Public ReadOnly Property EDIList As List(Of String)


            Get

                Return _EDIList

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


            _EDIList.Clear()
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
                                    _EDIList.Add(line)
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
                log.ExceptionDetails("DCSGlobal.EDI.FileFix.Go", ex)

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


            _EDIList.Clear()
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
                                    _EDIList.Add(line)
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
                log.ExceptionDetails("DCSGlobal.EDI.FileFix.Go", ex)

            End Try






            Return x

        End Function





    End Class


End Namespace