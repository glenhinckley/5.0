Option Explicit On
Option Strict On

Option Compare Binary




Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Collections.Generic
'Imports System.Data.DataSetExtensions

Imports System.IO
Imports System.Text
Imports System.Threading

Imports DCSGlobal.BusinessRules.Logging
Imports DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff


Namespace DCSGlobal.EDI

    Public Class Load277db
        Implements IDisposable


        Private log As New logExecption
        Private oD As New Declarations
        Private objss As New StringStuff

        Public Sub Dispose() Implements System.IDisposable.Dispose

            GC.SuppressFinalize(Me)

            ''  Console.WriteLine("Object " & GetHashCode() & " disposed.")
        End Sub

        Protected Overrides Sub Finalize()
            log = Nothing
            oD = Nothing
            objss = Nothing
            Dispose()
            '' Console.WriteLine("Object " & GetHashCode() & " finalized.")
        End Sub



        Function Go() As Integer

            Dim r As Integer = -1


            ' go get the rows to process from db


            'lop thre rows
            ' assign the response or what ever has the 27x to a string

            ' ImportByString(ByVal this is the string that has the 27x As String, ByVal get it now BatchID As Double)

            'loop


            'your done

            'case 270

            'case 271
            'nblalalbvlalbla


            Return r


        End Function




    End Class




End Namespace
