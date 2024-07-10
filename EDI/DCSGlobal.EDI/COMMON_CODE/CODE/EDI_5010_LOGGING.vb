Option Explicit On
Option Strict On


Imports System.Text.RegularExpressions
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Collections.Generic


Imports System.IO
Imports System.Text
Imports System.Threading

Imports DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff
Imports DCSGlobal.BusinessRules.Logging
Imports DCSGlobal.BusinessRules.CoreLibrary
Imports DCSGlobal.BusinessRules.CoreLibraryII
Imports DCSGlobal.BusinessRules.FileTransferClient




Namespace DCSGlobal.EDI





    Public Class EDI_5010_LOGGING

        Inherits EDI_5010_COMMON_DECS

        Implements IDisposable

        '******************************************************************************************************************
        '  all vars not declared here are in   EDI_5010_COMMON_DECS   it is inhreted from the tables for this type
        '  if you are going to use it on this class only it goes below if not put it in EDI_5010_COMMON_DECS
        '******************************************************************************************************************



        Private log As New logExecption
        Private ss As New StringStuff
        '   Private oD As New Declarations

        Public _ConnectionString As String = String.Empty
        Public _CommandTimeOut As Integer = 90




        Private _SchedulerLogID As Integer = 0


        Private FileLoadStatus As String = String.Empty
        Private iStatus As Integer = 0


        Private _ProcessYearMonth As Integer = 0
        Private _OverrideProcessYearMonth As String = String.Empty





        Private _isAK1 As Boolean = False
        Private _isAK9 As Boolean = False
        Private _isIK9 As Boolean = False
        Private _isTA1 As Boolean = False

        Private _app As String = "Validate EDI"





        Private _AAAFailureCode As String = String.Empty



        Public Sub Dispose() Implements System.IDisposable.Dispose

            GC.SuppressFinalize(Me)

            ''  Console.WriteLine("Object " & GetHashCode() & " disposed.")
        End Sub

        Protected Overrides Sub Finalize()
            log = Nothing
            ss = Nothing
            Dispose()
            '' Console.WriteLine("Object " & GetHashCode() & " finalized.")
        End Sub

        Public ReadOnly Property isTA1 As Boolean
            Get
                Return _isTA1
            End Get

        End Property



        Public Property ProcessYearMonth() As Integer
            Get
                Return _ProcessYearMonth

            End Get
            Set(ByVal value As Integer)
                _ProcessYearMonth = value
            End Set
        End Property

        Public Property OverrideProcessYearMonth() As String
            Get
                Return _OverrideProcessYearMonth

            End Get
            Set(ByVal value As String)
                _OverrideProcessYearMonth = value
            End Set
        End Property

        Public WriteOnly Property ConnectionString As String

            Set(value As String)
                _ConnectionString = value
                log.ConnectionString = value
            End Set
        End Property

        'Public ReadOnly Property ServiceTypeCode As String
        '    Get
        '        Return _ServiceTypeCode
        '    End Get

        'End Property


        'Public ReadOnly Property ReferenceIdentification As String
        '    Get
        '        Return _ReferenceIdentification
        '    End Get

        'End Property

        Public ReadOnly Property NPI As String
            Get
                Return _NPI
            End Get

        End Property

        Public ReadOnly Property AAAFailureCode As String
            Get
                Return _AAAFailureCode
            End Get

        End Property


        Public Property InterchangeControlNumber As String
            Get
                Return _ISA06_InterchangeSenderID
            End Get

            Set(value As String)
                _ISA13_InterchangeControlNumber = value
            End Set
        End Property

        Public Property InterchangeSenderID As String
            Get
                Return _ISA06_InterchangeSenderID
            End Get

            Set(value As String)
                _ISA06_InterchangeSenderID = value
            End Set
        End Property

        Public Property InterchangeReceiverID As String

            Get
                Return _ISA08_InterchangeReceiverID
            End Get

            Set(value As String)
                _ISA08_InterchangeReceiverID = value
            End Set

   
        End Property

        Public Property InterchangeDate As String


            Get
                Return _ISA09_InterchangeDate
            End Get

            Set(value As String)
                _ISA09_InterchangeDate = value
            End Set

        End Property

        Public Property InterchangeTime As String

            Get
                Return _ISA10_InterchangeTime
            End Get

            Set(value As String)
                _ISA10_InterchangeTime = value
            End Set

    
        End Property

        Public Property ImplementationConventionReference As String


            Get
                Return _ImplementationConventionReference
            End Get

            Set(value As String)
                _ImplementationConventionReference = value
            End Set



        End Property

        Public Property TransactionSetIdentifierCode As String


            Get
                Return _TransactionSetIdentifierCode
            End Get

            Set(value As String)
                _TransactionSetIdentifierCode = value
            End Set



        End Property


        Public Property IMPORTER_CLASS As String


            Get
                Return _IMPORTER_CLASS
            End Get

            Set(value As String)
                _IMPORTER_CLASS = value
            End Set



        End Property


        Public Property IMPORTER_BUILD As String


            Get
                Return _IMPORTER_BUILD
            End Get

            Set(value As String)
                _IMPORTER_BUILD = value
            End Set



        End Property


        Public Property FileID As Integer


            Get
                Return CInt(_FILE_ID)
            End Get

            Set(value As Integer)
                _FILE_ID = value
            End Set



        End Property

        Public Property SchedulerLogID As Integer


            Get
                Return _SchedulerLogID
            End Get

            Set(value As Integer)
                _SchedulerLogID = value
            End Set



        End Property


        'Public ReadOnly Property EDI_SEQUENCE_NUMBER As Integer


        '    Get
        '        Return _EDI_SEQUENCE_NUMBER
        '    End Get


        'End Property

        Public Function InsertFileName(ByVal FilePath As String, ByVal FileName As String, ByVal TransactionSetIdentifierCode As String) As Integer

            Dim r As Integer = -1
            _TransactionSetIdentifierCode = TransactionSetIdentifierCode

            r = InsertFileName(FilePath, FileName)

            Return r

        End Function


        Public Function EDI_SEQUENCE(ByVal TransactionSetIdentifierCode As String) As Integer


            Dim _RETURN_CODE As Integer = -1

            Dim sqlString As String = String.Empty


            Select Case _TransactionSetIdentifierCode
                Case "270"
                    sqlString = "SELECT NEXT VALUE FOR EDI_SEQUENCE_270"
                Case "271"
                    sqlString = "SELECT NEXT VALUE FOR EDI_SEQUENCE_271"
                Case "276"
                    sqlString = "SELECT NEXT VALUE FOR EDI_SEQUENCE_276"
                Case "277"
                    sqlString = "SELECT NEXT VALUE FOR EDI_SEQUENCE_277"
                Case "278"
                    sqlString = "SELECT NEXT VALUE FOR EDI_SEQUENCE_278"
                Case "835"
                    sqlString = "SELECT NEXT VALUE FOR EDI_SEQUENCE_835"
                Case "837"
                    sqlString = "SELECT NEXT VALUE FOR EDI_SEQUENCE_837"
                Case "997"
                    sqlString = "SELECT NEXT VALUE FOR EDI_SEQUENCE_997"
                Case "999"
                    sqlString = "SELECT NEXT VALUE FOR EDI_SEQUENCE_999"

            End Select

            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)

                        cmd.CommandType = CommandType.Text


                        Using reader As SqlDataReader = cmd.ExecuteReader()
                            While reader.Read()
                                _EDI_SEQUENCE_NUMBER = Convert.ToInt32(reader(0))
                            End While
                        End Using

                    End Using
                    Con.Close()
                End Using
                _RETURN_CODE = 0
            Catch ex As Exception
                _RETURN_CODE = -1
                log.ExceptionDetails("DCSGlobal.EDI.EDI_SEQUENCE", ex)

            End Try

            Return _RETURN_CODE



        End Function


        Public Function InsertFileName(ByVal FilePath As String, ByVal FileName As String) As Integer


            Dim r As Integer = -1

            Dim sqlString As String = String.Empty



            Select Case _TransactionSetIdentifierCode
                Case "270"
                    sqlString = "usp_EDI_270_ADD_FILE"
                Case "271"
                    sqlString = "usp_EDI_271_ADD_FILE"
                Case "276"
                    sqlString = "usp_EDI_276_ADD_FILE"
                Case "277"
                    sqlString = "usp_EDI_277_ADD_FILE"
                Case "278"
                    sqlString = "usp_EDI_278_ADD_FILE"
                Case "835"
                    sqlString = "usp_EDI_835_ADD_FILE"
                Case "837"
                    sqlString = "usp_EDI_837_ADD_FILE"
                Case "997"
                    sqlString = "usp_EDI_997_ADD_FILE"
                Case "999"
                    sqlString = "usp_EDI_999_ADD_FILE"

            End Select

            'Try
            '    _EDI_SEQUENCE_NUMBER = EDI_SEQUENCE(_TransactionSetIdentifierCode)
            'Catch ex As Exception

            'End Try








            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@FILE_NAME", SqlDbType.VarChar, 255).Value = FileName
                        cmd.Parameters.Add("@FILE_PATH", SqlDbType.VarChar, 255).Value = FilePath
                        cmd.Parameters.Add("@EDI_TYPE", SqlDbType.VarChar, 255).Value = _TransactionSetIdentifierCode
                        cmd.Parameters.Add("@EDI_VERSION", SqlDbType.VarChar, 255).Value = _ImplementationConventionReference
                        cmd.Parameters.Add("@InterchangeSenderID", SqlDbType.VarChar, 255).Value = Convert.ToString(_ISA06_InterchangeSenderID)
                        cmd.Parameters.Add("@InterchangeReceiverID", SqlDbType.VarChar, 255).Value = Convert.ToString(_ISA08_InterchangeReceiverID)
                        cmd.Parameters.Add("@InterchangeTime", SqlDbType.VarChar, 255).Value = Convert.ToString(_ISA10_InterchangeTime)
                        cmd.Parameters.Add("@InterchangeDate", SqlDbType.VarChar, 255).Value = Convert.ToString(_ISA09_InterchangeDate)
                        cmd.Parameters.Add("@InterchangeControlNumber", SqlDbType.VarChar, 255).Value = Convert.ToString(InterchangeControlNumber)
                        '  cmd.Parameters.Add("@EDI_SEQUENCE_NUMBER", SqlDbType.BigInt).Value = _EDI_SEQUENCE_NUMBER

                        cmd.Parameters.Add("@SchedulerLogID", SqlDbType.Int).Value = _SchedulerLogID
                        cmd.Parameters.Add("@isValid", SqlDbType.Int).Value = 1

                        cmd.Parameters.Add("@FileLoadStatus", SqlDbType.VarChar, 255).Value = "VALIDTION COMPLETE"


                        cmd.Parameters.Add("@ProcessStartTime", SqlDbType.DateTime).Value = DateTime.Now


                        cmd.Parameters.Add("@FILE_ID", SqlDbType.BigInt)
                        cmd.Parameters("@FILE_ID").Direction = ParameterDirection.Output

                        cmd.ExecuteNonQuery()

                        'Commented below line. Because getting error when we load files from the folder.
                        _FILE_ID = Convert.ToInt32(cmd.Parameters("@FILE_ID").Value)

                        r = 0
                    End Using
                    Con.Close()
                End Using

            Catch ex As Exception
                r = -1
                log.ExceptionDetails("DCSGlobal.EDI.InsertFileName", ex)

            End Try

            Return r ''oD.rBatchId


            'Return rr

        End Function


        Private Function AddDocument() As Integer


            Dim rr As Integer = -1

            Dim sqlString As String
            sqlString = "[usp_EDI_5010_HIPAA_MASTER_DOCUMENT_INSERT]"

            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@EDI_VERSION", SqlDbType.VarChar, 255).Value = _ImplementationConventionReference
                        cmd.Parameters.Add("@EDI_TYPE", SqlDbType.VarChar, 255).Value = _TransactionSetIdentifierCode
                        '     cmd.Parameters.Add("@FILE_NAME", SqlDbType.VarChar, 255).Value = _FileName


                        'Commented below line. Because getting error when we load files from the folder.
                        cmd.Parameters.Add("@FILE_ID", Data.SqlDbType.BigInt) ' CInt(Convert.ToInt64(_FILE_ID)))
                        cmd.Parameters("@FILE_ID").Direction = ParameterDirection.Output
                        cmd.Parameters.Add("@DOCUMENT_ID", SqlDbType.BigInt)
                        cmd.Parameters("@DOCUMENT_ID").Direction = ParameterDirection.Output

                        cmd.ExecuteNonQuery()

                        'Commented below line. Because getting error when we load files from the folder.
                        _DOCUMENT_ID = CLng(cmd.Parameters("@DOCUMENT_ID").Value)
                        _FILE_ID = CLng(cmd.Parameters("@FILE_ID").Value)
                        rr = 0
                    End Using
                    Con.Close()
                End Using

            Catch sx As SqlException



                ' log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.InsertFileName", sx)


            Catch ex As Exception
                rr = -1

            End Try

            Return rr ''oD.rBatchId



        End Function


        Private Function UnknownDocument() As Integer


            Dim rr As Integer = -1

            Dim sqlString As String
            sqlString = "[HIPAA_MASTER_DOCUMENT_INSERT]"

            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@EDI_VERSION", SqlDbType.VarChar, 255).Value = _ImplementationConventionReference
                        cmd.Parameters.Add("@EDI_TYPE", SqlDbType.VarChar, 255).Value = _TransactionSetIdentifierCode
                        'Commented below line. Because getting error when we load files from the folder.
                        cmd.Parameters.Add("@DOCUMENT_ID", SqlDbType.BigInt)
                        cmd.Parameters("@DOCUMENT_ID").Direction = ParameterDirection.Output

                        cmd.ExecuteNonQuery()

                        'Commented below line. Because getting error when we load files from the folder.
                        _DOCUMENT_ID = CLng(cmd.Parameters("@DOCUMENT_ID").Value)

                        rr = 0
                    End Using
                    Con.Close()
                End Using

            Catch sx As SqlException



                ' log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.InsertFileName", sx)


            Catch ex As Exception
                rr = -1

            End Try

            Return -1 ''oD.rBatchId



        End Function
        Public Function UpdateFileData(ByVal FileID As Integer, ByVal TransactionSetIdentifierCode As String) As Integer
            Dim r As Integer = -1


            _TransactionSetIdentifierCode = TransactionSetIdentifierCode
            r = UpdateFileData(FileID, _TransactionSetIdentifierCode)

            Return r

        End Function


        Public Function UpdateFileData(ByVal FileID As Integer) As Integer

            Dim r As Integer = -1


            Dim sqlString As String = String.Empty



            Select Case _TransactionSetIdentifierCode
                Case "270"
                    sqlString = "usp_EDI_5010_HIPAA_270_UPDATE_FILE"
                Case "271"
                    sqlString = "usp_EDI_5010_HIPAA_271_UPDATE_FILE"
                Case "276"
                    sqlString = "usp_EDI_5010_HIPAA_276_UPDATE_FILE"
                Case "277"
                    sqlString = "usp_EDI_5010_HIPAA_277_UPDATE_FILE"
                Case "278"
                    ' sqlString = "usp_EDI_278_ADD_FILE"
                Case "835"
                    sqlString = "usp_EDI_5010_HIPAA_835_UPDATE_FILE"
                Case "837"
                    sqlString = "usp_EDI_5010_HIPAA_837_UPDATE_FILE"
                Case "997"
                    'sqlString = "usp_EDI_997_ADD_FILE"
                Case "999"
                    ' sqlString = "usp_EDI_999_ADD_FILE"

            End Select



            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@InterchangeControlNumber", SqlDbType.VarChar, 255).Value = _ISA13_InterchangeControlNumber
                        cmd.Parameters.Add("@InterchangeTime", SqlDbType.VarChar, 255).Value = _ISA10_InterchangeTime
                        cmd.Parameters.Add("@InterchangeDate", SqlDbType.VarChar, 255).Value = _ISA09_InterchangeDate
                        cmd.Parameters.Add("@InterchangeSenderID", SqlDbType.VarChar, 255).Value = _ISA06_InterchangeSenderID
                        cmd.Parameters.Add("@InterchangeReceiverID", SqlDbType.VarChar, 255).Value = _ISA08_InterchangeReceiverID
                        cmd.Parameters.Add("@FileLoadStatus", SqlDbType.VarChar, 255).Value = "VALIDTION COMPLETE"
                        'Commented below line. Because getting error when we load files from the folder.
                        cmd.Parameters.Add("@FILE_ID", SqlDbType.BigInt).Value = FileID


                        cmd.ExecuteNonQuery()


                        r = 0
                    End Using
                    Con.Close()
                End Using

            Catch ex As Exception
                r = -1

            End Try

            Return r ''oD.rBatchId


            'Return rr

        End Function

        Public Function UpdateFileStatus(ByVal FileID As Integer, ByVal FileStatus As String, ByVal TransactionSetIdentifierCode As String) As Integer
            Dim r As Integer = -1

            _TransactionSetIdentifierCode = TransactionSetIdentifierCode
            r = UpdateFileStatus(FileID, _TransactionSetIdentifierCode)

            Return r

        End Function



        Public Function UpdateFileStatus(ByVal FileID As Integer, ByVal FileStatus As String) As Integer


            Dim rr As Integer = -1

            Dim sqlString As String = String.Empty



            Select Case _TransactionSetIdentifierCode
                Case "270"
                    sqlString = "usp_EDI_5010_HIPAA_270_UPDATE_FILE_STATUS"
                Case "271"
                    sqlString = "usp_EDI_5010_HIPAA_271_UPDATE_FILE_STATUS"
                Case "276"
                    sqlString = "usp_EDI_5010_HIPAA_276_UPDATE_FILE_STATUS"
                Case "277"
                    sqlString = "usp_EDI_5010_HIPAA_277_UPDATE_FILE_STATUS"
                Case "278"
                    ' sqlString = "usp_EDI_278_ADD_FILE"
                Case "835"
                    sqlString = "usp_EDI_5010_HIPAA_835_UPDATE_FILE_STATUS"
                Case "837"
                    sqlString = "usp_EDI_5010_HIPAA_837_UPDATE_FILE_STATUS"
                Case "997"
                    'sqlString = "usp_EDI_997_ADD_FILE"
                Case "999"
                    ' sqlString = "usp_EDI_999_ADD_FILE"

            End Select

           
            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@IMPORTER_CLASS", SqlDbType.VarChar, 255).Value = _IMPORTER_CLASS
                        cmd.Parameters.Add("@IMPORTER_BUILD", SqlDbType.VarChar, 255).Value = _IMPORTER_BUILD
                        cmd.Parameters.Add("@FileLoadStatus", SqlDbType.VarChar, 255).Value = FileStatus
                        cmd.Parameters.Add("@FILE_ID", SqlDbType.BigInt).Value = FileID


                        cmd.ExecuteNonQuery()


                        rr = 0
                    End Using
                    Con.Close()
                End Using
            Catch ex As Exception
                rr = -1
                If String.IsNullOrEmpty(sqlString) Then
                    ' NO SP is attached  while begin parse
                    ' Keep adding  failure  log entry
                Else
                    log.ExceptionDetails("UPDATE_FILE_STATUS", ex)
                End If


            End Try

            Return rr ''oD.rBatchId


            'Return rr

        End Function



        Public Function UpdateFileLocation(ByVal FileID As Integer, ByVal FilePath As String, ByVal VAULT_ROOT_DIRECTORY As String, ByVal VAULT_CLIENT_DIRECTORY As String, ByVal VAULT_FILE_PATH As String, ByVal TransactionSetIdentifierCode As String) As Integer

            Dim r As Integer = -1
            _TransactionSetIdentifierCode = TransactionSetIdentifierCode
            UpdateFileLocation(FileID, FilePath, VAULT_ROOT_DIRECTORY, VAULT_CLIENT_DIRECTORY, VAULT_FILE_PATH)


            Return r

        End Function

        Public Function UpdateFileLocation(ByVal FileID As Integer, ByVal FilePath As String, ByVal VAULT_ROOT_DIRECTORY As String, ByVal VAULT_CLIENT_DIRECTORY As String, ByVal VAULT_FILE_PATH As String) As Integer


            Dim rr As Integer = -1

            Dim sqlString As String = String.Empty



            Select Case _TransactionSetIdentifierCode
                Case "270"
                    sqlString = "[usp_EDI_5010_HIPAA_270_UPDATE_FILE_LOCATION]"
                Case "271"
                    sqlString = "[usp_EDI_5010_HIPAA_271_UPDATE_FILE_LOCATION]"
                Case "276"
                    sqlString = "[usp_EDI_5010_HIPAA_276_UPDATE_FILE_LOCATION]"
                Case "277"
                    sqlString = "[usp_EDI_5010_HIPAA_277_UPDATE_FILE_LOCATION]"
                Case "278"
                    ' sqlString = "usp_EDI_278_ADD_FILE"
                Case "835"
                    sqlString = "[usp_EDI_5010_HIPAA_835_UPDATE_FILE_LOCATION]"
                Case "837"
                    sqlString = "[usp_EDI_5010_HIPAA_837_UPDATE_FILE_LOCATION]"
                Case "997"
                    'sqlString = "usp_EDI_997_ADD_FILE"
                Case "999"
                    ' sqlString = "usp_EDI_999_ADD_FILE"

            End Select


            Try

                Using Con As New SqlConnection(_ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@VAULT_ROOT_DIRECTORY", SqlDbType.VarChar, 260).Value = VAULT_ROOT_DIRECTORY
                        cmd.Parameters.Add("@VAULT_CLIENT_DIRECTORY", SqlDbType.VarChar, 260).Value = VAULT_CLIENT_DIRECTORY
                        cmd.Parameters.Add("@VAULT_FILE_PATH", SqlDbType.VarChar, 260).Value = VAULT_FILE_PATH
                        cmd.Parameters.Add("@FilePath", SqlDbType.VarChar, 1024).Value = FilePath
                        cmd.Parameters.Add("@FILE_ID", SqlDbType.BigInt).Value = FileID


                        cmd.ExecuteNonQuery()


                        rr = 0
                    End Using
                    Con.Close()
                End Using

            Catch sx As SqlException
                log.ExceptionDetails(sqlString, sx)


            Catch ex As Exception
                rr = -1
                log.ExceptionDetails(sqlString, ex)

            End Try

            Return rr ''oD.rBatchId


            'Return rr

        End Function


    End Class
End Namespace