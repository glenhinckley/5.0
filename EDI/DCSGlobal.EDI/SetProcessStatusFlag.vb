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

Namespace DCSGlobal.EDI

    Public Class SetProcessStatusFlag
        Implements IDisposable

        Private log As New logExecption
        Private _ConnectionString As String = String.Empty
        Private _ProcessStartTime As DateTime = Now
        Private _ProcessEndTime As DateTime = Now
        Private _SP_SET_PROCESS_STATUS As String = String.Empty
       
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
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


        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub




        Protected Overrides Sub Finalize()
            log = Nothing
            '' em = Nothing
            Dispose()
            '   Console.WriteLine("Object " & GetHashCode() & " finalized.")
        End Sub




        Public WriteOnly Property ConnectionString As String

            Set(value As String)

                log.ConnectionString = value
                _ConnectionString = value

            End Set
        End Property


        Public WriteOnly Property SP_SET_PROCESS_STATUS As String

            Set(value As String)

                _SP_SET_PROCESS_STATUS = value

            End Set
        End Property



        Public Function SetFlag(ByVal StatusFlag As String, ByVal ID As Int32) As Int32
            Dim r As Integer = -1

            Try
                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()


                    '' _sqlGetREQ = "Select REQUEST from REQ where id " + Convert.ToString(_id)


                    Using cmd As New SqlCommand(_SP_SET_PROCESS_STATUS, Con)


                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.AddWithValue("@STATUS_FLAG", StatusFlag)
                        cmd.Parameters.AddWithValue("@Start_time", _ProcessStartTime)
                        cmd.Parameters.AddWithValue("@end_time", _ProcessEndTime)
                        cmd.Parameters.AddWithValue("@ID", ID)
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


    End Class

End Namespace

