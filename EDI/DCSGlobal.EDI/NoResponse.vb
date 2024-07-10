Option Explicit On


Imports DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff

Imports System.Text.RegularExpressions
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Collections.Generic
Imports DCSGlobal.BusinessRules.Logging



Namespace DCSGlobal.EDI





    Public Class NoResponse
        Implements IDisposable

        Private disposedValue As Boolean ' To detect redundant calls
        Private log As New logExecption
        Private ss As New StringStuff
        Private oD As New Declarations
        Private _ConnectionString As String = String.Empty

        Dim _BATCH_ID As Integer
        Private _REQ As String = String.Empty
        Private _RES As String = String.Empty

        Private _AppName As String = String.Empty


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
            oD = Nothing
            '' em = Nothing
            Dispose()
            '   Console.WriteLine("Object " & GetHashCode() & " finalized.")
        End Sub


        Public WriteOnly Property REQ As String

            Set(value As String)

                _REQ = value
            End Set
        End Property

        ' prop AppName

        Public Property RES As String
            Get
                Return ss.StripCRLF(_RES)
            End Get
            Set(value As String)
                _RES = value
            End Set
        End Property


        Public WriteOnly Property ConnectionString As String

            Set(value As String)
                _ConnectionString = value
                oD._ConnectionString = value
                log.ConnectionString = value
            End Set
        End Property

        Public Function LogNoResponse(ByVal BatchID As Int64) As Integer

            Dim r As Integer = -1
            Dim sqlString = String.Empty

            Dim _batch_id As String = String.Empty
            Dim _EDI As String = String.Empty

            Try
                sqlString = "USP_GENERATE_NORESPONSE_271"


                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using Com As New SqlCommand(sqlString, Con)
                        Com.CommandTimeout = 90
                        Com.CommandType = CommandType.StoredProcedure
                        Com.Parameters.AddWithValue("@id", BatchID)
                        Using RDR = Com.ExecuteReader()
                            If RDR.HasRows Then
                                Do While RDR.Read
                                    _RES = RDR.Item(0)
                                Loop
                                r = 0
                            End If
                        End Using
                    End Using
                    Con.Close()
                End Using
            Catch exsql As SqlException
                log.ExceptionDetails("NoResponse.LogNoResponse" + " " + "DCSGlobal.EDI.LogNoResponse to eligibility_noresponse_271_templates log to db failed for batchid : " + Convert.ToString(BatchID), exsql)
                r = -2
            Catch ex As Exception
                r = -3
                log.ExceptionDetails("NoResponse.LogNoResponse" + " " + "DCSGlobal.EDI.LogNoResponse to eligibility_noresponse_271_templates loge to db failed for batchid : " + Convert.ToString(BatchID), ex)
            Finally
                'sqlConn.Close()
            End Try
            Return r
        End Function

    End Class
End Namespace

