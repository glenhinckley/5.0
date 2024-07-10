
Option Explicit On
Option Strict On
Option Compare Binary

Imports System.Text.RegularExpressions
Imports System.Text





Namespace DCSGlobal.EDI



    Public Class INFO
        Implements IDisposable

        Private _NameSpace As String = "DCSGlobal.BusinessRules.CoreLibraryII"
        Private _Version As String = String.Empty
        Private _Build As Integer = 5000

        Protected disposed As Boolean = False

        Public Sub New()
            If Debugger.IsAttached Then


            End If


        End Sub





        Protected Overrides Sub Finalize()

            Dispose()
            '' Console.WriteLine("Object " & GetHashCode() & " finalized.")
        End Sub

        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposed Then
                If disposing Then





                    'email = Nothing
                    ' Insert code to free managed resources. 
                End If


                ' Insert code to free unmanaged resources. 
            End If
            Me.disposed = True
        End Sub


        ' Do not change or add Overridable to these methods. 
        ' Put cleanup code in Dispose(ByVal disposing As Boolean). 
        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
    End Class
End Namespace


