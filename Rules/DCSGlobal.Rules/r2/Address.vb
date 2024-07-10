Option Strict On
Option Explicit On

Imports System






Namespace DCSGlobal.Rules




    Public Class Address
        Implements IDisposable



        Private disposedValue As Boolean ' To detect redundant calls

        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub



        Property AddressType As String = String.Empty
        Property StreetAddress As String = String.Empty
        Property StreetAddress2 As String = String.Empty
        Property City As String = String.Empty
        Property State As String = String.Empty
        Property ZipCode As String = String.Empty


        Public Function Clear() As Integer

            Dim r As Integer = -1
            Try

                AddressType = String.Empty
                StreetAddress = String.Empty
                StreetAddress2 = String.Empty
                City = String.Empty
                State = String.Empty
                ZipCode = String.Empty
                r = 0
            Catch ex As Exception

            End Try

            Return r
        End Function

        Public Function Blank() As Boolean
            Dim b As Boolean = False


            Return b

        End Function


    End Class
End Namespace
