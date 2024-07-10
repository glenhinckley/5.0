Option Explicit On

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
Imports DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff


Namespace DCSGlobal.EDI


    Public Class EDIFile
        Implements IDisposable



        Private ss As New StringStuff
        Private log As New logExecption

        Private _Verbose As Boolean = False

        Private _ConnectionString As String = String.Empty


        Private _FileID As Integer = 0
        Private _FileName As String = String.Empty
        Private _OriginalPath As String = String.Empty
        Private _FinalPath As String = String.Empty
        Private _EDI_TYPE As String = String.Empty
        Private _EDI_VERSION As String = String.Empty
        Private _InterchangeControlNumber As String = String.Empty
        Private _InterchangeTime As String = String.Empty
        Private _TransactionSetIdentifierCode As String = String.Empty
        Private _ORIGINAL_FILE_NAME As String = String.Empty
        Private _SchedulerLogID As Integer = 0
        Private _InterchangeDate As String = String.Empty
        Private _ORGINAL_PATH As String = String.Empty
        Private _isComplete As Integer = 0
        Private _isDuplicate As Integer = 0
        Private _InterchangeSenderID As String = String.Empty
        Private _InterchangeReceiverID As String = String.Empty
        Private _isValid As Integer = 0
        Private _RowCount As Integer = 0



        Private _ProcessStartTime As DateTime
        Private _ProcessEndTime As DateTime
        Private _ProcessElaspedTime As Long = 0
        Private _ResultCode As Integer = 0





        Public Sub Dispose() Implements System.IDisposable.Dispose

            GC.SuppressFinalize(Me)

            'Console.WriteLine("Object " & GetHashCode() & " disposed.")
        End Sub

        Protected Overrides Sub Finalize()

            log = Nothing
            ss = Nothing



            Dispose()
            ' Console.WriteLine("Object " & GetHashCode() & " finalized.")
        End Sub

        Public WriteOnly Property ConnectionString As String
            Set(value As String)
                log.ConnectionString = value
                _ConnectionString = value

            End Set
        End Property
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns>File ID or SQQL Error code</returns>
        ''' <remarks></remarks>
        Public Function InsertEDIFile() As Integer
            Dim r As Integer = -1




            Dim sqlString As String = String.Empty




            Select Case (_TransactionSetIdentifierCode)

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

            End Select
            If sqlString <> String.Empty Then

                Try

                    Using Con As New SqlConnection(_ConnectionString)
                        Con.Open()
                        Using Com As New SqlCommand(sqlString, Con)

                            Com.CommandType = CommandType.StoredProcedure
                            Com.Parameters.AddWithValue("@FILE_NAME", _ORIGINAL_FILE_NAME)
                            Com.Parameters.AddWithValue("@InterchangeControlNumber", _InterchangeControlNumber)
                            Com.Parameters.AddWithValue("@InterchangeTime", _InterchangeTime)
                            Com.Parameters.AddWithValue("@InterchangeDate", InterchangeDate)
                            Com.Parameters.AddWithValue("@ORIGINAL_PATH", _OriginalPath)
                            Com.Parameters.AddWithValue("@InterchangeSenderID", _InterchangeSenderID)
                            Com.Parameters.AddWithValue("@InterchangeReceiverID", _InterchangeReceiverID)
                            Com.Parameters.AddWithValue("@SchedulerLogID", _SchedulerLogID)



                            Com.Parameters.Add("@FILE_ID", Data.SqlDbType.BigInt, 1)
                            Com.Parameters("@FILE_ID").Direction = ParameterDirection.Output

                            Com.Parameters.Add("@isDuplicate", Data.SqlDbType.Int, 1)
                            Com.Parameters("@isDuplicate").Direction = ParameterDirection.Output


                            r = Com.ExecuteNonQuery()

                            If r > 0 Then
                                _FileID = Convert.ToInt32(Com.Parameters("@FILE_ID").Value.ToString())
                                r = Convert.ToInt32(Com.Parameters("@FILE_ID").Value.ToString())
                                _isDuplicate = Convert.ToInt32(Com.Parameters("@isDuplicate").Value.ToString())
                            End If

                        End Using
                        Con.Close()
                    End Using





                Catch ex As Exception
                    r = -1



                Finally


                End Try


            End If






            Return r
        End Function

        Public Function UpdateEDIFile() As Integer
            Dim r As Integer = -1

            Dim sqlString As String = String.Empty




            Select Case (_TransactionSetIdentifierCode)

                Case "270"
                    sqlString = "usp_EDI_270_UPDATE_FILE"
                Case "271"
                    sqlString = "usp_EDI_271_UPDATE_FILE"
                Case "276"
                    sqlString = "usp_EDI_276_UPDATE_FILE"
                Case "277"
                    sqlString = "usp_EDI_277_UPDATE_FILE"
                Case "278"
                    sqlString = "usp_EDI_278_UPDATE_FILE"
                Case "835"
                    sqlString = "usp_EDI_835_UPDATE_FILE"
                Case "837"
                    sqlString = "usp_EDI_837_UPDATE_FILE"

            End Select


            If sqlString <> String.Empty Then
                Try
                    Using Con As New SqlConnection(_ConnectionString)
                        Con.Open()
                        Using Com As New SqlCommand(sqlString, Con)

                            Com.CommandType = CommandType.StoredProcedure


                            Com.Parameters.AddWithValue("@FILE_ID", _FileID)
                            Com.Parameters.AddWithValue("@ProcessStartTime", _ProcessStartTime)
                            Com.Parameters.AddWithValue("@ProcessEndTime", _ProcessEndTime)
                            Com.Parameters.AddWithValue("@ProcessElaspedTime", _ProcessElaspedTime)
                            Com.Parameters.AddWithValue("@ResultCode", _ResultCode)
                            Com.Parameters.AddWithValue("@isComplete", _isComplete)
                            Com.Parameters.AddWithValue("@isValid", _isValid)
                            Com.Parameters.AddWithValue("@FINAL_FILE_PATH", _FinalPath)
                            Com.Parameters.AddWithValue("@RowCount", _RowCount)
                            r = Com.ExecuteNonQuery()


                        End Using
                        Con.Close()
                    End Using





                Catch ex As Exception
                    r = -1



                Finally


                End Try

            End If


            Return r
        End Function

        Private Function GetEDIFile(ByVal ORIGINAL_FILE_NAME_EX As String, ByVal EDI_TYPE_EX As String) As Integer
            Dim r As Integer = -1




            Dim sqlString As String = String.Empty




            Select Case (EDI_TYPE_EX)

                Case "270"
                    sqlString = "usp_EDI_270_GET_FILE"
                Case "271"
                    sqlString = "usp_EDI_271_GET_FILE"
                Case "276"
                    sqlString = "usp_EDI_276_GET_FILE"
                Case "277"
                    sqlString = "usp_EDI_277_GET_FILE"
                Case "278"
                    sqlString = "usp_EDI_278_GET_FILE"
                Case "835"
                    sqlString = "usp_EDI_835_GET_FILE"
                Case "837"
                    sqlString = "usp_EDI_837_GET_FILE"

            End Select

            If sqlString <> String.Empty Then

                Try

                    Using Con As New SqlConnection(_ConnectionString)
                        Con.Open()
                        Using Com As New SqlCommand(sqlString, Con)

                            Com.CommandType = CommandType.StoredProcedure
                            Com.Parameters.AddWithValue("@SEARCH_METHOD", 2)
                            Com.Parameters.AddWithValue("@FILE_NAME", ORIGINAL_FILE_NAME_EX)

                            Using RDR = Com.ExecuteReader()
                                If RDR.HasRows Then
                                    Do While RDR.Read

                                        If Not IsDBNull(RDR("FILE_NAME")) Then
                                            _FileName = RDR("FILE_NAME")
                                        Else
                                            _FileName = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("FILE_PATH")) Then
                                            _FinalPath = RDR("FILE_PATH")
                                        Else
                                            _FinalPath = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("ORIGINAL_PATH")) Then
                                            _ORGINAL_PATH = RDR("ORIGINAL_PATH")
                                        Else
                                            _ORGINAL_PATH = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("TransactionSetIdentifierCode")) Then
                                            _TransactionSetIdentifierCode = RDR("TransactionSetIdentifierCode")
                                        Else
                                            _TransactionSetIdentifierCode = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("InterchangeControlNumber")) Then
                                            _InterchangeControlNumber = RDR("InterchangeControlNumber")
                                        Else
                                            _InterchangeControlNumber = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("InterchangeTime")) Then
                                            _InterchangeTime = RDR("InterchangeTime")
                                        Else
                                            _InterchangeTime = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("InterchangeDate")) Then
                                            _InterchangeDate = RDR("InterchangeDate")
                                        Else
                                            _InterchangeDate = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("InterchangeSenderID")) Then
                                            _InterchangeSenderID = RDR("InterchangeSenderID")
                                        Else
                                            _InterchangeSenderID = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("InterchangeDate")) Then
                                            _InterchangeDate = RDR("InterchangeDate")
                                        Else
                                            _InterchangeDate = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("SchedulerLogID")) Then
                                            _SchedulerLogID = RDR("SchedulerLogID")
                                        Else
                                            _SchedulerLogID = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("RowCount")) Then
                                            _RowCount = RDR("RowCount")
                                        Else
                                            _RowCount = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("isValid")) Then
                                            _isValid = RDR("isValid")
                                        Else
                                            _isValid = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("isComplete")) Then
                                            _isComplete = RDR("isComplete")
                                        Else
                                            _isComplete = String.Empty
                                        End If
                                    Loop
                                End If
                            End Using
                        End Using
                        Con.Close()
                    End Using





                Catch ex As Exception
                    r = -1



                Finally


                End Try


            End If





            Return r
        End Function


        Private Function GetEDIFile(ByVal FileID_EX As Long, ByVal EDI_TYPE_EX As String) As Integer
            Dim r As Integer = -1




            Dim sqlString As String = String.Empty




            Select Case (EDI_TYPE_EX)

                Case "270"
                    sqlString = "usp_EDI_270_GET_FILE"
                Case "271"
                    sqlString = "usp_EDI_271_GET_FILE"
                Case "276"
                    sqlString = "usp_EDI_276_GET_FILE"
                Case "277"
                    sqlString = "usp_EDI_277_GET_FILE"
                Case "278"
                    sqlString = "usp_EDI_278_GET_FILE"
                Case "835"
                    sqlString = "usp_EDI_835_GET_FILE"
                Case "837"
                    sqlString = "usp_EDI_837_GET_FILE"

            End Select

            If sqlString <> String.Empty Then

                Try

                    Using Con As New SqlConnection(_ConnectionString)
                        Con.Open()
                        Using Com As New SqlCommand(sqlString, Con)

                            Com.CommandType = CommandType.StoredProcedure
                            Com.Parameters.AddWithValue("@SEARCH_METHOD", 1)
                            Com.Parameters.AddWithValue("@FILE_ID", FileID_EX)

                            Using RDR = Com.ExecuteReader()
                                If RDR.HasRows Then
                                    Do While RDR.Read

                                        If Not IsDBNull(RDR("FILE_NAME")) Then
                                            _FileName = RDR("FILE_NAME")
                                        Else
                                            _FileName = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("FILE_PATH")) Then
                                            _FinalPath = RDR("FILE_PATH")
                                        Else
                                            _FinalPath = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("ORIGINAL_PATH")) Then
                                            _ORGINAL_PATH = RDR("ORIGINAL_PATH")
                                        Else
                                            _ORGINAL_PATH = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("TransactionSetIdentifierCode")) Then
                                            _TransactionSetIdentifierCode = RDR("TransactionSetIdentifierCode")
                                        Else
                                            _TransactionSetIdentifierCode = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("InterchangeControlNumber")) Then
                                            _InterchangeControlNumber = RDR("InterchangeControlNumber")
                                        Else
                                            _InterchangeControlNumber = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("InterchangeTime")) Then
                                            _InterchangeTime = RDR("InterchangeTime")
                                        Else
                                            _InterchangeTime = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("InterchangeDate")) Then
                                            _InterchangeDate = RDR("InterchangeDate")
                                        Else
                                            _InterchangeDate = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("InterchangeSenderID")) Then
                                            _InterchangeSenderID = RDR("InterchangeSenderID")
                                        Else
                                            _InterchangeSenderID = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("InterchangeDate")) Then
                                            _InterchangeDate = RDR("InterchangeDate")
                                        Else
                                            _InterchangeDate = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("SchedulerLogID")) Then
                                            _SchedulerLogID = RDR("SchedulerLogID")
                                        Else
                                            _SchedulerLogID = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("RowCount")) Then
                                            _RowCount = RDR("RowCount")
                                        Else
                                            _RowCount = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("isValid")) Then
                                            _isValid = RDR("isValid")
                                        Else
                                            _isValid = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("isComplete")) Then
                                            _isComplete = RDR("isComplete")
                                        Else
                                            _isComplete = String.Empty
                                        End If


                                    Loop
                                End If
                            End Using
                        End Using
                        Con.Close()
                    End Using





                Catch ex As Exception
                    r = -1



                Finally


                End Try


            End If





            Return r
        End Function



        Private Function GetEDIFile(ByVal InterchangeControlNumber_EX As String, ByVal InterchangeDate_EX As String, ByVal InterchangeTime_EX As String, ByVal EDI_TYPE_EX As String) As Integer
            Dim r As Integer = -1




            Dim sqlString As String = String.Empty




            Select Case (EDI_TYPE_EX)

                Case "270"
                    sqlString = "usp_EDI_270_GET_FILE"
                Case "271"
                    sqlString = "usp_EDI_271_GET_FILE"
                Case "276"
                    sqlString = "usp_EDI_276_GET_FILE"
                Case "277"
                    sqlString = "usp_EDI_277_GET_FILE"
                Case "278"
                    sqlString = "usp_EDI_278_GET_FILE"
                Case "835"
                    sqlString = "usp_EDI_835_GET_FILE"
                Case "837"
                    sqlString = "usp_EDI_837_GET_FILE"

            End Select


            If sqlString <> String.Empty Then

                Try

                    Using Con As New SqlConnection(_ConnectionString)
                        Con.Open()
                        Using Com As New SqlCommand(sqlString, Con)

                            Com.CommandType = CommandType.StoredProcedure
                            Com.Parameters.AddWithValue("@SEARCH_METHOD", 3)
                            Com.Parameters.AddWithValue("@InterchangeControlNumber", InterchangeControlNumber_EX)

                            Using RDR = Com.ExecuteReader()
                                If RDR.HasRows Then
                                    Do While RDR.Read

                                        If Not IsDBNull(RDR("FILE_NAME")) Then
                                            _FileName = RDR("FILE_NAME")
                                        Else
                                            _FileName = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("FILE_PATH")) Then
                                            _FinalPath = RDR("FILE_PATH")
                                        Else
                                            _FinalPath = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("ORIGINAL_PATH")) Then
                                            _ORGINAL_PATH = RDR("ORIGINAL_PATH")
                                        Else
                                            _ORGINAL_PATH = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("TransactionSetIdentifierCode")) Then
                                            _TransactionSetIdentifierCode = RDR("TransactionSetIdentifierCode")
                                        Else
                                            _TransactionSetIdentifierCode = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("InterchangeControlNumber")) Then
                                            _InterchangeControlNumber = RDR("InterchangeControlNumber")
                                        Else
                                            _InterchangeControlNumber = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("InterchangeTime")) Then
                                            _InterchangeTime = RDR("InterchangeTime")
                                        Else
                                            _InterchangeTime = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("InterchangeDate")) Then
                                            _InterchangeDate = RDR("InterchangeDate")
                                        Else
                                            _InterchangeDate = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("InterchangeSenderID")) Then
                                            _InterchangeSenderID = RDR("InterchangeSenderID")
                                        Else
                                            _InterchangeSenderID = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("InterchangeDate")) Then
                                            _InterchangeDate = RDR("InterchangeDate")
                                        Else
                                            _InterchangeDate = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("SchedulerLogID")) Then
                                            _SchedulerLogID = RDR("SchedulerLogID")
                                        Else
                                            _SchedulerLogID = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("RowCount")) Then
                                            _RowCount = RDR("RowCount")
                                        Else
                                            _RowCount = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("isValid")) Then
                                            _isValid = RDR("isValid")
                                        Else
                                            _isValid = String.Empty
                                        End If

                                        If Not IsDBNull(RDR("isComplete")) Then
                                            _isComplete = RDR("isComplete")
                                        Else
                                            _isComplete = String.Empty
                                        End If
                                    Loop
                                End If
                            End Using
                        End Using
                        Con.Close()
                    End Using





                Catch ex As Exception
                    r = -1



                Finally


                End Try


            End If




            Return r
        End Function

        Public Property ResultCode As Integer
            Get
                Return _ResultCode

            End Get
            Set(value As Integer)
                _ResultCode = value
            End Set
        End Property

        Public Property ProcessElaspedTime As Long
            Get
                Return _ProcessElaspedTime

            End Get
            Set(value As Long)
                _ProcessElaspedTime = value
            End Set
        End Property

        Public Property ProcessEndTime As DateTime
            Get
                Return _ProcessEndTime

            End Get
            Set(value As DateTime)
                _ProcessEndTime = value
            End Set
        End Property

        Public Property ProcessStartTime As DateTime
            Get
                Return _ProcessStartTime

            End Get
            Set(value As DateTime)
                _ProcessStartTime = value
            End Set
        End Property

        Public Property FileID As Integer
            Get
                Return _FileID

            End Get
            Set(value As Integer)
                _FileID = value
            End Set
        End Property

        Public Property FileName As String
            Get
                Return _FileName
            End Get
            Set(value As String)
                _FileName = value

            End Set
        End Property

        Public Property OriginalPath As String
            Get
                Return _OriginalPath
            End Get
            Set(value As String)
                _OriginalPath = value
            End Set
        End Property

        Public Property FinalPath As String
            Get
                Return _FinalPath
            End Get
            Set(value As String)

                _FinalPath = value

            End Set
        End Property

        Public Property EDI_TYPE As String
            Get
                Return _EDI_TYPE
            End Get
            Set(value As String)
                _EDI_TYPE = value
            End Set
        End Property

        Public Property EDI_VERSION As String
            Get
                Return _EDI_VERSION
            End Get
            Set(value As String)
                _EDI_VERSION = value
            End Set
        End Property

        Public Property InterchangeControlNumber As String
            Get
                Return _InterchangeControlNumber
            End Get
            Set(value As String)
                _InterchangeControlNumber = value
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

        Public Property OriginalFileName As String
            Get
                Return _ORIGINAL_FILE_NAME

            End Get
            Set(value As String)
                _ORIGINAL_FILE_NAME = value
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



        Public Property InterchangeTime As String
            Get
                Return _InterchangeTime
            End Get
            Set(value As String)
                _InterchangeTime = value
            End Set
        End Property


        Public Property InterchangeDate As String
            Get
                Return _InterchangeDate
            End Get
            Set(value As String)
                _InterchangeDate = value
            End Set
        End Property


        Public Property isDuplicate As Integer
            Get
                Return _isDuplicate
            End Get
            Set(value As Integer)
                _isDuplicate = value
            End Set
        End Property


        Public Property isComplete As Integer
            Get
                Return _isComplete
            End Get
            Set(value As Integer)
                _isComplete = value
            End Set
        End Property

        Public Property InterchangeSenderID As String
            Get
                Return _InterchangeSenderID
            End Get
            Set(value As String)
                _InterchangeSenderID = value

            End Set
        End Property

        Public Property InterchangeReceiverID As String
            Get
                Return _InterchangeReceiverID
            End Get
            Set(value As String)
                _InterchangeReceiverID = value
            End Set
        End Property

        Public Property isValid As Integer
            Get
                Return _isValid
            End Get
            Set(value As Integer)
                _isValid = value
            End Set
        End Property


        Public Property RowCount As Integer
            Get
                Return _RowCount
            End Get
            Set(value As Integer)
                _RowCount = value
            End Set
        End Property







    End Class






End Namespace

