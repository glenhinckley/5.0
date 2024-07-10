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


    Public Class EDI_5010_IMPORT

        Inherits EDI_5010_COMMON_DECS

        Implements IDisposable



        Protected disposed As Boolean = False

        '******************************************************************************************************************
        '  all vars not declared here are in   EDI_5010_COMMON_DECS   it is inhreted from the tables for this type
        '  if you are going to use it on this class only it goes below if not put it in EDI_5010_COMMON_DECS
        '******************************************************************************************************************


        Private log As New logExecption
        Private ss As New StringStuff
        Private d As New DebugOUT
        '   Private oD As New Declarations

        Public _ConnectionString As String = String.Empty
        Public _CommandTimeOut As Integer = 90

        Private _DocumentType As String = String.Empty









        Private _FileName As String = String.Empty
        Private _FilePath As String = String.Empty


        Private _EDI_VERSION As String = String.Empty


        Private _FileToParse As String


        Public Sub New()
            If Debugger.IsAttached Then
                _VERBOSE = 1
                _DEBUG_LEVEL = 1

            End If

            _CONSOLE_NAME = "EDI_5010_IMPORT"
            _CLASS_NAME = "EDI_5010_IMPORT"

        End Sub




        Protected Overrides Sub Finalize()

            Dispose()
            '' Console.WriteLine("Object " & GetHashCode() & " finalized.")
        End Sub

        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposed Then
                If disposing Then

                    log = Nothing



                    'email = Nothing
                    ' Insert code to free managed resources. 
                End If

                ss = Nothing
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

        Public Property LOG_EDI As String

            Set(value As String)
                _LOG_EDI = value
            End Set
            Get
                Return _LOG_EDI
            End Get
        End Property


        Public WriteOnly Property ConnectionString As String

            Set(value As String)
                _ConnectionString = value
                log.ConnectionString = value
            End Set
        End Property



        Public WriteOnly Property BatchID As Long

            Set(value As Long)
                _BATCH_ID = value

            End Set
        End Property

        Public WriteOnly Property isFile As Boolean

            Set(value As Boolean)
                _IS_FILE = value

            End Set
        End Property


        Public WriteOnly Property Verbose As Integer

            Set(value As Integer)
                _VERBOSE = value
            End Set
        End Property

        Public Property FILE_ID As Long
            Get
                Return _FILE_ID
            End Get
            Set(value As Long)
                _FILE_ID = value
            End Set
        End Property

        Public Property CONSOLE_NAME As String
            Get
                Return _CONSOLE_NAME
            End Get

            Set(value As String)
                _CONSOLE_NAME = value
            End Set
        End Property

        Public ReadOnly Property TransactionSetIdentifierCode As String
            Get
                Return _TransactionSetIdentifierCode
            End Get


        End Property




        Public Function ImportByList(ByVal EDIList As List(Of String), ByVal BatchID As Double) As Int32

            _IS_LIST = True

            Dim x As Integer = -1
            _EDIList = EDIList
            _BATCH_ID = CLng(BatchID)

            Import()

            Return x

        End Function
        Public Function ImportByString(ByVal EDI As String, ByVal BatchID As Double) As Integer


            _IS_STRING = True

            Dim r As Integer = -1



            Dim SP As New StringPrep()
            _BATCH_ID = CLng(BatchID)

            _EDIList = SP.SingleEDI(EDI)

            Import()

            Return r

        End Function



        Public Function ImportByList(ByVal EDIList As List(Of String), ByVal BatchID As Long) As Int32

            _IS_LIST = True

            Dim x As Integer = -1
            _EDIList = EDIList
            _BATCH_ID = BatchID

            Import()

            Return x

        End Function
        Public Function ImportByString(ByVal EDI As String, ByVal BatchID As Long) As Integer
            _IS_STRING = True
            Dim r As Integer = -1



            Dim SP As New StringPrep()
            _BATCH_ID = BatchID

            _EDIList = SP.SingleEDI(EDI)

            Import()
            Return r

        End Function

        Public Function ImportByFileName(ByVal FilePath As String, ByVal FileName As String) As Integer

            _IS_FILE = True
            Dim r As Integer = -1


            _FileName = FileName
            _FilePath = FilePath


            Using el As New EDIListBuilder

                el.ConnectionString = _ConnectionString
                r = el.BuildListByFile(FilePath + FileName)

                If r = 0 Then
                    _EDIList = el.EDIList
                End If
            End Using


            If r = 0 Then




                If r = 0 Then

                    Import()

                End If
            End If


            Return r

        End Function
        Public Function Import() As Integer

            _FUNCTION_NAME = "Function Import() As Integer"

            Dim r As Integer = -1



            If _IS_FILE Then

                Using e As New EDI_5010_VALIDATE

                    e.ConnectionString = _ConnectionString
                    e.byFile(_FilePath, _FileName)
                    _FILE_ID = e.FileID
                    _EDI_SEQUENCE_NUMBER = e.EDI_SEQUENCE_NUMBER
                    _TransactionSetIdentifierCode = e.TransactionSetIdentifierCode
                    _ImplementationConventionReference = e.ImplementationConventionReference
                End Using


            End If






            Select Case _TransactionSetIdentifierCode

                Case "270"
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   270
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '5010 270/271

                    '270
                    'Using EDI As New EDI_270_005010X279A1
                    '    EDI.ConnectionString = _ConnectionString
                    '    EDI.Import(_EDIList)

                    'End Using



                Case "271"
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   271
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


                    '271
                    'Using EDI As New EDI_271_005010X279A1
                    '    EDI.ConnectionString = _ConnectionString
                    '    EDI.Import(_EDIList)

                    'End Using




                Case "276"

                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   276
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    Select Case _ImplementationConventionReference

                        Case "005010X212"
                            '5010 276

                            Using EDI As New EDI_5010_276_005010X212_DCS
                                EDI.ConnectionString = _ConnectionString
                                EDI.LOG_EDI = _LOG_EDI
                                EDI.BATCH_ID = _BATCH_ID
                                EDI.FILE_ID = _FILE_ID
                                EDI.isFile = _IS_FILE
                                EDI.EDI_SEQUENCE_NUMBER = _EDI_SEQUENCE_NUMBER
                                EDI.Import(_EDIList)

                            End Using


                        Case Else
                            '276 failover


                            '    UnknownDocument()

                            Using EDI As New EDI_5010_276_005010X212_DCS
                                EDI.ConnectionString = _ConnectionString
                                EDI.LOG_EDI = _LOG_EDI
                                EDI.BATCH_ID = _BATCH_ID
                                EDI.FILE_ID = _FILE_ID
                                EDI.isFile = _IS_FILE
                                EDI.EDI_SEQUENCE_NUMBER = _EDI_SEQUENCE_NUMBER
                                EDI.Import(_EDIList)

                            End Using

                    End Select


                Case "277"

                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   277
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


                    Select Case _ImplementationConventionReference

                        Case "005010X212"
                            '5010 277

                            Using EDI As New EDI_5010_277_005010X212_v2
                                EDI.ConnectionString = _ConnectionString
                                EDI.LOG_EDI = _LOG_EDI
                                EDI.BATCH_ID = _BATCH_ID
                                EDI.FILE_ID = _FILE_ID
                                EDI.isFile = _IS_FILE
                                EDI.EDI_SEQUENCE_NUMBER = _EDI_SEQUENCE_NUMBER
                                EDI.Import(_EDIList)

                            End Using


                        Case Else
                            '277 failover


                            '     UnknownDocument()

                            Using EDI As New EDI_5010_277_005010X212_v2
                                EDI.ConnectionString = _ConnectionString
                                EDI.LOG_EDI = _LOG_EDI
                                EDI.BATCH_ID = _BATCH_ID
                                EDI.FILE_ID = _FILE_ID
                                EDI.isFile = _IS_FILE
                                EDI.EDI_SEQUENCE_NUMBER = _EDI_SEQUENCE_NUMBER
                                EDI.Import(_EDIList)

                            End Using

                    End Select




                Case "278"
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   278
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '5010 278

                    'Using EDI As New EDI_278_005010X217
                    '    EDI.ConnectionString = _ConnectionString
                    '    EDI.Import(_EDIList)

                    'End Using




                Case "835"
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   835
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    Select Case _ImplementationConventionReference
                        '5010 835
                        Case "005010X221A1"


                            Using EDI As New EDI_5010_835_005010X221A1
                                EDI.ConnectionString = _ConnectionString
                                EDI.BATCH_ID = _BATCH_ID
                                EDI.LOG_EDI = _LOG_EDI
                                EDI.FILE_ID = _FILE_ID
                                EDI.isFile = _IS_FILE
                                EDI.EDI_SEQUENCE_NUMBER = _EDI_SEQUENCE_NUMBER
                                EDI.Import(_EDIList)

                            End Using



                        Case Else

                            Using EDI As New EDI_5010_835_005010X221A1
                                EDI.ConnectionString = _ConnectionString
                                EDI.BATCH_ID = _BATCH_ID
                                EDI.LOG_EDI = _LOG_EDI
                                EDI.FILE_ID = _FILE_ID
                                EDI.isFile = _IS_FILE
                                EDI.EDI_SEQUENCE_NUMBER = _EDI_SEQUENCE_NUMBER
                                EDI.Import(_EDIList)

                            End Using
                    End Select



                Case "837"
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    '   837
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    Select Case _ImplementationConventionReference

                        Case "005010X222A1"
                            '5010 837p


                            Using EDI As New EDI_5010_837P_005010X222A1_v2
                                EDI.ConnectionString = _ConnectionString
                                EDI.LOG_EDI = _LOG_EDI
                                EDI.BATCH_ID = _BATCH_ID
                                EDI.FILE_ID = _FILE_ID
                                EDI.isFile = _IS_FILE
                                EDI.EDI_SEQUENCE_NUMBER = _EDI_SEQUENCE_NUMBER
                                EDI.Import(_EDIList)

                            End Using




                        Case "005010X223A2"
                            '5010 837i


                            Using EDI As New EDI_5010_837I_005010X223A2_v2
                                EDI.ConnectionString = _ConnectionString
                                EDI.LOG_EDI = _LOG_EDI
                                EDI.BATCH_ID = _BATCH_ID
                                EDI.FILE_ID = _FILE_ID
                                EDI.isFile = _IS_FILE
                                EDI.EDI_SEQUENCE_NUMBER = _EDI_SEQUENCE_NUMBER
                                EDI.Import(_EDIList)

                            End Using


                        Case Else
                            '837 failover



                            '    UnknownDocument()

                            Using EDI As New EDI_5010_837I_005010X223A2_v2
                                EDI.ConnectionString = _ConnectionString
                                EDI.LOG_EDI = _LOG_EDI
                                EDI.BATCH_ID = _BATCH_ID
                                EDI.FILE_ID = _FILE_ID
                                EDI.isFile = _IS_FILE
                                EDI.EDI_SEQUENCE_NUMBER = _EDI_SEQUENCE_NUMBER
                                EDI.Import(_EDIList)

                            End Using




                    End Select


                Case Else

            End Select




            Return r
        End Function




    End Class
End Namespace
