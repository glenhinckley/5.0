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



    Public Class ImportTA1

        Inherits EDITA1Tables

        Implements IDisposable

        ' Keep track of when the object is disposed. 
        Protected disposed As Boolean = False
        Dim _ROW_NUM As Integer
        Dim _isFile As Boolean




        ' This method disposes the base object's resources. 
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposed Then
                If disposing Then


                    log = Nothing
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






        Private log As New logExecption
        Private objss As New StringStuff
        Private oD As New Declarations
        Private _ConnectionString As String = String.Empty
        Dim _BATCH_ID As Integer





        Private _ClassVersion As String = "2.0"
        Private start_time As DateTime
        Private stop_time As DateTime
        Private elapsed_time As TimeSpan


        Private _TablesBuilt As Boolean = False
        Private _ENFlag As Boolean = False

        Private err As String = ""
        Private _chars As String
        Private STFlag As Integer = 0
        'Private LXFlag As Integer = 0
        'Private CLPFlag As Integer = 0
        Private FileID As Int32 = 0

        Private LXList As New List(Of String)


        Private _HSDCount As Integer
        Private _Debug As Integer = 0
        Private V As Boolean = False

        Private _FileToParse As String
        Private _CLPString As String

        Private _EDIList As New List(Of String)


        Private HL_PARENT As String



        Dim _FileID As Integer = 200
        Dim _BatchID As Double






        Public WriteOnly Property ConnectionString As String

            Set(value As String)
                oD._ConnectionString = value
                log.ConnectionString = value
            End Set
        End Property



        Public WriteOnly Property BatchID As Double

            Set(value As Double)
                oD._BatchID = value

            End Set
        End Property

        Public WriteOnly Property EDI As String

            Set(value As String)
                oD.edi = value
            End Set
        End Property


        Public WriteOnly Property ebr_id As Double


            Set(value As Double)
                oD.ebr_id = value
            End Set
        End Property

        Public WriteOnly Property user_id As String

            Set(value As String)
                oD.user_id = value
            End Set
        End Property


        Public WriteOnly Property hosp_code As String

            Set(value As String)
                oD.hosp_code = value
            End Set
        End Property


        Public WriteOnly Property source As String

            Set(value As String)
                oD.source = value
            End Set
        End Property




        Public WriteOnly Property DebugLevel As Integer

            Set(value As Integer)
                oD.DebugLevel = value
            End Set
        End Property
        Public WriteOnly Property isFile As Boolean


            Set(value As Boolean)
                _isFile = value
            End Set
        End Property

        Public WriteOnly Property PAYOR_ID As String

            Set(value As String)
                oD.PAYOR_ID = value
            End Set
        End Property


        Public WriteOnly Property Vendor_name As String

            Set(value As String)
                oD.Vendor_name = value
            End Set
        End Property




        Public WriteOnly Property Log_EDI As String

            Set(value As String)
                oD.Log_EDI = value
            End Set
        End Property




        Public WriteOnly Property DeleteFlag As String

            Set(value As String)
                oD.DeleteFlag = value
            End Set
        End Property
        Public WriteOnly Property Verbose As Integer

            Set(value As Integer)
                oD.Verbose = value
            End Set
        End Property


        Public Property ExecutionTime As TimeSpan
            Get
                Return elapsed_time


            End Get
            Set(value As TimeSpan)

            End Set
        End Property


        Public ReadOnly Property ErrorMessage As String
            Get
                Return err
            End Get

        End Property

        Public Function ImportByString(ByVal EDI As String, ByVal BatchID As Double) As Integer

            Dim r As Integer = -1


            Using el As New EDIListBuilder

                el.ConnectionString = oD._ConnectionString
                r = el.BuildListByString(EDI)

                If r = 0 Then
                    _EDIList = el.EDIList
                End If
            End Using


            oD._BatchID = BatchID


                If r = 0 Then

                    r = ImportTA1()

                End If



            Return r

        End Function








        Public Function Import(ByVal EDIList As List(Of String), ByVal BatchID As Double) As Int32

            Dim x As Integer = -1
            _EDIList = EDIList
            _BatchID = BatchID

            x = ImportTA1()

            Return x

        End Function


        Public Function ImportByFileName(ByVal FilePath As String, ByVal FileName As String) As Integer

            Dim r As Integer = -1


            Using el As New EDIListBuilder

                el.ConnectionString = oD._ConnectionString
                r = el.BuildListByFile(FilePath + FileName)

                If r = 0 Then
                    _EDIList = el.EDIList
                End If
            End Using


            If r = 0 Then


                r = InsertFileName(FilePath, FileName)

                If r = 0 Then

                    r = ImportTA1()

                End If
            End If


            Return r

        End Function

        Private Function ImportTA1() As Integer
            V = True


            oD.edi_type = "TA1"

            oD.HL20Flag = 0
            oD.DataElementSeparatorFlag = 0
            _ROW_NUM = 0

            Dim [end] As DateTime
            Dim start As DateTime = DateTime.Now

            If _isFile = True Then
                ''    oD._FileID = InsertFileName(_FileToParse)

            Else

                oD._FileID = 0

            End If
            Dim rr As Integer = 0


            '     Try

            start_time = Now
            oD.TimeStamp = CDate(FormatDateTime(start_time, DateFormat.ShortDate))



            oD.IEAFlag = CInt(False)


            If V Then
                log.ExceptionDetails("DCSGlobal.EDI.ImportTA1.Import", "Pasring list")
                log.ExceptionDetails("DCSGlobal.EDI.ImportTA1.Import", "### Overall Start List Time: " + start.ToLongTimeString())
            End If

            _CLPString = String.Empty




            oD.BATCHUID = Guid.NewGuid()

            oD._BatchID = _BatchID






            If _TablesBuilt = False Then
                BuildTables()
                _TablesBuilt = True
            Else

                ISA.Clear()


            End If

            Dim last As String = String.Empty
            '  Dim line As String = String.Empty
            Dim rowcount As Int32 = 0

            'so lets get down to it opn the file in a stream reader and move thru it one line at a time...........

            '  Using r As New StreamReader(FileToParse)
            oD.RowProcessedFlag = 0
            'line = r.ReadLine()
            'Console.WriteLine(line)


            'Console.WriteLine(oD.DataElementSeparator)

            '  Try

            For Each line As String In _EDIList


                _ROW_NUM = _ROW_NUM + 1

                If oD.DataElementSeparatorFlag = 0 Then
                    oD.DataElementSeparator = Mid(line, 4, 1)
                    oD.DataElementSeparatorFlag = 1
                End If
                ' Add this line to list.
                last = line
                ' Display to console.
                ' 
                ' Read in the next line.

                line = line.Replace("~", "")

                rowcount = rowcount + 1
                oD.ediRowRecordType = objss.ParseDemlimtedStringEDI(line, oD.DataElementSeparator, 1)
                'Console.WriteLine(oD.ediRowRecordType)
                oD.CurrentRowData = line

                oD.RowProcessedFlag = 0

                '  line = r.ReadLine


                'check for LX

                'If objss.ParseDemlimtedStringEDI(line, oD.DataElementSeparator, 1) = "LX" Then
                '    LXFlag = 1
                'End If




                ' ******************************************************************************************************************************


                If oD.ediRowRecordType = "ISA" Then

                    oD.RowProcessedFlag = 1

                    oD.EBCarrotCHAR = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 12)

                    oD.ISA_GUID = Guid.NewGuid
                    oD.P_GUID = oD.ISA_GUID

                    oD.ISA01 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)
                    oD.ISA02 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                    oD.ISA03 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)
                    oD.ISA04 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 5)
                    oD.ISA05 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 6)
                    oD.ISA06 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 7)
                    oD.ISA07 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 8)
                    oD.ISA08 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 9)
                    oD.ISA09 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 10)
                    oD.ISA10 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 11)
                    oD.ISA11 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 12)
                    oD.ISA12 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 13)
                    oD.ISA13 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 14)
                    oD.ISA14 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 15)
                    oD.ISA15 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 16)
                    oD.ISA16 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 17)

                    Dim ISARow As DataRow = ISA.NewRow
                    '  ISARow("ROW_NUM") = _ROW_NUM
                    ISARow("HIPAA_ISA_GUID") = oD.ISA_GUID
                    ISARow("ISA01") = oD.ISA01
                    ISARow("ISA02") = oD.ISA02
                    ISARow("ISA03") = oD.ISA03
                    ISARow("ISA04") = oD.ISA04
                    ISARow("ISA05") = oD.ISA05
                    ISARow("ISA06") = oD.ISA06
                    ISARow("ISA07") = oD.ISA07
                    ISARow("ISA08") = oD.ISA08
                    ISARow("ISA09") = oD.ISA09
                    ISARow("ISA10") = oD.ISA10
                    ISARow("ISA11") = oD.ISA11
                    ISARow("ISA12") = oD.ISA12
                    ISARow("ISA13") = oD.ISA13
                    ISARow("ISA14") = oD.ISA14
                    ISARow("ISA15") = oD.ISA15
                    ISARow("ISA16") = oD.ISA16

                    ISA.Rows.Add(ISARow)

                    oD.ISA_ROW_ID = rowcount

                    oD.CarrotDataDelimiter = oD.ISA11
                    oD.ComponentElementSeperator = oD.ISA16


                    _chars = "RowDataDelimiter: " + oD.DataElementSeparator + " CarrotDataDelimiter: " + oD.CarrotDataDelimiter + " ComponentElementSeperator: " + oD.ComponentElementSeperator

                    oD.ISAFlag = 1
                End If


                If oD.ediRowRecordType = "TA1" Then
                    oD.RowProcessedFlag = 1
                    oD.GS_GUID = Guid.NewGuid
                    oD.P_GUID = oD.GS_GUID

                    oD.TA101 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)
                    oD.TA102 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                    oD.TA103 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)
                    oD.TA104 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 5)
                    oD.TA105 = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 6)

                    Dim TA1Row As DataRow = TA1.NewRow
                    '   TA1Row("ROW_NUM") = _ROW_NUM
                    TA1Row("HIPAA_ISA_GUID") = oD.ISA_GUID
                    TA1Row("TA101") = oD.TA101
                    TA1Row("TA102") = oD.TA102
                    TA1Row("TA103") = oD.TA103
                    TA1Row("TA104") = oD.TA104
                    TA1Row("TA105") = oD.TA105


                    TA1.Rows.Add(TA1Row)



                    ' ComitISAGS()

                End If



                If oD.ediRowRecordType = "IEA" Then
                    oD.ISA_GUID = Guid.Empty
                    oD.IEAFlag = 1
                    oD.ISAFlag = 0

                    ComitALL()

                End If





                '***********************************************************************************************************************
                'end cut
                '***********************************************************************************************************************

            Next

            'Console.WriteLine(last)
            'Console.WriteLine(rowcount)

            'Catch ex As Exception
            '    rr = 30
            '    oD.IEAFlag = 1
            '    RollBack()
            '    log.ExceptionDetails(oD._BatchID.ToString, "DCSGlobal.EDI.Import837I was rolled back due to Error in main loop " + oD.ediRowRecordType + "Rowcount " + Convert.ToString(rowcount))
            '    log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.Import837 error in main loop" + FileToParse, ex)

            'Finally
            'COMITUNK()

            'End Try



            '   COMITUNK()



            If oD.IEAFlag = 0 Then
                rr = -40
                RollBack()
                log.ExceptionDetails(oD._BatchID.ToString, "DCSGlobal.EDI.ImportTA1 was LIST rolled back due to IEA Not Found")

            End If


            If V Then
                [end] = DateTime.Now
                log.ExceptionDetails("DCSGlobal.EDI.ImportTA1.Import", "### Overall LISt End Time: " + [end].ToLongTimeString())
                log.ExceptionDetails("DCSGlobal.EDI.ImportTA1.Import", "### Overall Lest Run Time: " + ([end] - start).ToString())
            End If


            'Return rr
            If rr > 0 Then
                Return oD.rBatchId
            Else
                Return rr
            End If
        End Function

        Private Function InsertFileName(ByVal FilePath As String, ByVal FileName As String) As Int32


            Dim rr As Integer = -1

            Dim sqlString As String
            sqlString = "usp_EDI_TA1_ADD_FILE"

            Try

                Using Con As New SqlConnection(oD._ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)

                        cmd.CommandType = CommandType.StoredProcedure
                        cmd.Parameters.Add("@FILE_NAME", SqlDbType.VarChar, 255).Value = FileName
                        cmd.Parameters.Add("@FILE_PATH", SqlDbType.VarChar).Value = FilePath
                        cmd.Parameters.Add("@EDI_TYPE", SqlDbType.VarChar, 50).Value = "TA1"

                        cmd.Parameters.Add("@FILE_ID", SqlDbType.BigInt)
                        cmd.Parameters("@FILE_ID").Direction = ParameterDirection.Output


                        '   cmd.ExecuteNonQuery()

                        _FileID = Convert.ToInt32(cmd.Parameters("@FILE_ID").Value)

                        rr = 0
                    End Using
                    Con.Close()
                End Using

            Catch sx As SqlException



                log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.InsertFileName", sx)


            Catch ex As Exception
                rr = -1

                log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.InsertFileName", ex)

            Finally


            End Try


            Return oD.rBatchId


            'Return rr

        End Function

        Private Function ComitALL() As Integer


            Dim i As Integer = -1



            Dim sqlString As String
            sqlString = "usp_EDI_278_TA1"

            Try



                Using Con As New SqlConnection(oD._ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)

                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.AddWithValue("@HIPAA_278_ISA", ISA)
                        cmd.Parameters.AddWithValue("@HIPAA_278_TA1", TA1)

                        'cmd.Parameters.AddWithValue("@FILE_ID", _FileID)

                        'cmd.Parameters.AddWithValue("@DELETE_FLAG", oD.DeleteFlag)
                        'cmd.Parameters.AddWithValue("@cbr_id", oD.ebr_id)
                        '' cmd.Parameters.AddWithValue("@batch_id", oD._BatchID)
                        'cmd.Parameters.AddWithValue("@user_id", oD.user_id)
                        'cmd.Parameters.AddWithValue("@hosp_code", oD.hosp_code)
                        'cmd.Parameters.AddWithValue("@source", oD.source)


                        'cmd.Parameters.AddWithValue("@EDI", oD.edi)
                        'cmd.Parameters.AddWithValue("@PAYOR_ID", oD.PAYOR_ID)
                        'cmd.Parameters.AddWithValue("@Vendor_name", oD.Vendor_name)
                        'cmd.Parameters.AddWithValue("@Log_EDI", oD.Log_EDI)


                        'cmd.Parameters.Add("@Status", Data.SqlDbType.VarChar, 20)
                        'cmd.Parameters("@Status").Direction = ParameterDirection.Output



                        'cmd.Parameters.Add("@Reject_Reason_code", Data.SqlDbType.VarChar, 10)
                        'cmd.Parameters("@Reject_Reason_code").Direction = ParameterDirection.Output


                        'cmd.Parameters.Add("@LOOP_AGAIN", Data.SqlDbType.VarChar, 1)
                        'cmd.Parameters("@LOOP_AGAIN").Direction = ParameterDirection.Output


                        cmd.Parameters.AddWithValue("@batch_id", oD._BatchID)
                        'cmd.Parameters("@batch_id").Direction = ParameterDirection.InputOutput



                        i = cmd.ExecuteNonQuery()

                        'oD.rBatchId = cmd.Parameters("@batch_id").Value

                        'oD.Status = cmd.Parameters("@Status").Value.ToString()
                        'oD.RejectReasonCode = cmd.Parameters("@Reject_Reason_code").Value.ToString()
                        'oD.LoopAgain = cmd.Parameters("@LOOP_AGAIN").Value.ToString()


                        i = 0
                    End Using
                    Con.Close()
                End Using

            Catch ex As Exception
                i = -1

                log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.ImportTA1ALL", ex)

            Finally





            End Try


            ISA.Clear()
            TA1.Clear()




            Return i

        End Function

        Public Function COMITUNK() As Integer


            Dim i As Integer
            Dim param As New SqlParameter()


            Dim sqlConn As SqlConnection = New SqlConnection
            Dim cmd As SqlCommand
            Dim sqlString As String


            sqlConn.ConnectionString = oD._ConnectionString
            sqlConn.Open()

            Try


                sqlString = "[usp_EDI_TA1_UNK]"


                cmd = New SqlCommand(sqlString, sqlConn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@HIPAA_TA1_UNK", UNK)
                cmd.Parameters.AddWithValue("@FILE_ID", oD._FileID)


                ' i = cmd.ExecuteNonQuery()



                i = 0

            Catch ex As Exception
                i = -1

                log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.TA1 UNK for bactch ID " + oD._BatchID.ToString, ex)

            Finally

                sqlConn.Close()
            End Try

            Return i



        End Function



        Private Function RollBack() As Integer


            Dim i As Integer
            Dim param As New SqlParameter()

            Dim sqlString As String


            Try


                sqlString = "usp_EDI_TA1_ROLLBACK"


                Using Con As New SqlConnection(oD._ConnectionString)
                    Con.Open()
                    Using cmd As New SqlCommand(sqlString, Con)

                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.AddWithValue("@FILE_ID", oD._FileID)
                        '    cmd.ExecuteNonQuery()

                    End Using
                    Con.Close()
                End Using

                i = 0

            Catch ex As Exception
                i = -1

                log.ExceptionDetails(oD.Version + "  " + _ClassVersion + " " + "DCSGlobal.EDI.ImportTA1.rollback for bactch ID " + oD._BatchID.ToString, ex)

            Finally

            End Try

            Return i


        End Function











    End Class
End Namespace