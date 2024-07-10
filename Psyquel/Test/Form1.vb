
'Imports ListBZ
Imports Logging
Imports System.Diagnostics


Public Class Form1

    Private _ConnectionString As String = String.Empty
    Private _Logger As New LogExecption()

    ' This Does Nothing an should not show up
    Public Function Test() As String
        Touch()
        Return "Test"
    End Function


    ' This Does Nothing an should not show up
    Public Sub DeadFunction()

    End Sub

    Private Sub Touch()
        'Dim _Frame As New StackFrame(True)
        Dim stackTrace As New StackTrace()
        Dim stackFrame As StackFrame = stackTrace.GetFrame(3) ' 0=Touch() | 1=Test() | 2=Form1_Load() 
        Console.WriteLine(stackFrame.GetMethod().Name)
        Console.WriteLine(stackFrame.GetFileName)
        Console.WriteLine(stackFrame.GetFileLineNumber)
        'MessageBox.Show(_Frame.GetFileName())
        'MessageBox.Show(_Frame.GetFileLineNumber)
    End Sub


    Private Sub Touch(ByVal sender As Object)
        MessageBox.Show(sender.GetType().Assembly.Location)
    End Sub


    Private Sub Trace()

        Dim tracer As New StackTrace(True)
        Dim indent As String = ""
        Dim i As Integer

        Try
            For i = 0 To tracer.FrameCount - 1
                Dim stackFrame As StackFrame = tracer.GetFrame(i)
                Console.WriteLine()
                Console.WriteLine(indent + " Method: {0}", _
                    stackFrame.GetMethod())
                Console.WriteLine(indent + " File: {0}", _
                    stackFrame.GetFileName())
                Console.WriteLine(indent + " Line Number: {0}", _
                    stackFrame.GetFileLineNumber())

                indent += "  "
            Next i

        Catch ex As Exception
            Dim st As New StackTrace(ex)
            MessageBox.Show(st.GetFrame(1).GetMethod().Name)

        End Try

    End Sub


   


    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Test()
        DeadFunction()
    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Trace()

    End Sub







End Class
