Option Explicit On
Option Strict On
Option Compare Binary



Imports System.Text.RegularExpressions
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Collections.Generic
'Imports System.Data.DataSetExtensions
Imports System.Collections
Imports System.IO
Imports System.IO.File
Imports System.Text
Imports System.Threading

Imports DCSGlobal.BusinessRules.Logging
Imports DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff
Imports DCSGlobal.BusinessRules.CoreLibrary.IO


Namespace DCSGlobal.EDI





    Public Class Load835
        Implements IDisposable


        Private log As New logExecption()
        Private em As New Email()
        Private ss As New StringStuff
        Private FM As New FileMove
        Private fio As New FileIO



        Private _InstanceName As String = String.Empty
        Private _app As String
        Private _baseFolder As String

        Private _connectionString As String = String.Empty

        Private _SMTPServer As String = String.Empty
        Private _FromMailAddress As String = String.Empty
        Private _ToMailAddress As String = String.Empty

        Private _ParseTree As Int32 = 0
        Private _ProcessYearMonth As String = String.Empty
        Private _OverrideProcessYearMonth As String = String.Empty


        Private _InputFolder As String = String.Empty
        Private _FailedFolder As String = String.Empty
        Private _SuccessFolder As String = String.Empty
        Private _DuplicateFolder As String = String.Empty
        Private _FileFilter As String = String.Empty
        Private _FileEXT As String = String.Empty

        Private Property targetDirectory As Object




        Public Sub Dispose() Implements System.IDisposable.Dispose

            GC.SuppressFinalize(Me)

            Console.WriteLine("Object " & GetHashCode() & " disposed.")
        End Sub

        Protected Overrides Sub Finalize()
            log = Nothing
            em = Nothing
            Dispose()
            Console.WriteLine("Object " & GetHashCode() & " finalized.")
        End Sub



        Public WriteOnly Property InstanceName As String
            Set(value As String)
                _InstanceName = value
                _app = _InstanceName + ".Load835"
                FM.InstanceName = _app + "MoveFile"
            End Set
        End Property

        Public WriteOnly Property ConnectionString As String
            Set(value As String)
                _connectionString = value
                log.ConnectionString = value
                FM.ConnectionString = value
            End Set
        End Property


        Public WriteOnly Property SMTPServer As String
            Set(value As String)
                _SMTPServer = value
                em.Server = value
                FM.SMTPServer = value
            End Set
        End Property

        Public WriteOnly Property FromMailAddress As String
            Set(value As String)
                _FromMailAddress = value
                em.SendFrom = value
                FM.FromMailAddress = value
            End Set
        End Property

        Public WriteOnly Property ToMailAddress As String
            Set(value As String)
                _ToMailAddress = value
                em.SendTo = value
                FM.ToMailAddress = value
            End Set
        End Property



        Public WriteOnly Property ParseTree As Integer

            Set(value As Integer)
                _ParseTree = value
            End Set
        End Property

        Public Property FileFilter As String
            Get
                Return _FileFilter

            End Get
            Set(ByVal value As String)


                _FileFilter = value


            End Set
        End Property



        Public Property BaseFolder As String
            Get
                Return _baseFolder
            End Get
            Set(ByVal value As String)

                If (ss.GetLastChr(value) <> "\") Then
                    value = value + "\"
                End If
                _baseFolder = value
                FM.BaseFolder = value
            End Set
        End Property


        Public Property InputFolder As String
            Get
                Return _InputFolder

            End Get
            Set(ByVal value As String)


                If (ss.GetLastChr(value) <> "\") Then
                    value = value + "\"
                End If
                _InputFolder = value
                FM.Input = value
            End Set
        End Property


        Public Property FailedFolder As String
            Get
                Return _FailedFolder

            End Get
            Set(ByVal value As String)

                If (ss.GetLastChr(value) <> "\") Then
                    value = value + "\"
                End If
                _FailedFolder = value
                FM.Failed = value

            End Set
        End Property



        Public Property SuccessFolder As String
            Get
                Return _SuccessFolder

            End Get
            Set(ByVal value As String)

                If (ss.GetLastChr(value) <> "\") Then
                    value = value + "\"
                End If
                _SuccessFolder = value
                FM.Success = value
            End Set
        End Property


        Public Property DuplicateFolder As String
            Get
                Return _DuplicateFolder

            End Get
            Set(ByVal value As String)

                If (ss.GetLastChr(value) <> "\") Then
                    value = value + "\"
                End If
                _DuplicateFolder = value
                FM.Duplicate = value
            End Set
        End Property





        Public Property ProcessYearMonth As String
            Get
                Return _ProcessYearMonth

            End Get
            Set(ByVal value As String)
                _ProcessYearMonth = value
            End Set
        End Property

        Public Property OverrideProcessYearMonth As String
            Get
                Return _OverrideProcessYearMonth

            End Get
            Set(ByVal value As String)
                _OverrideProcessYearMonth = value
            End Set
        End Property








        Private FileList As New List(Of String)
        Private V As Boolean = False

        Public Function IsValidProcessInterchangeDate_old(ByVal FilePath As String, ByVal FileName As String) As Boolean
            Dim bStatus As Boolean = False
            Dim last As String = String.Empty
            Dim line As String = String.Empty
            Dim rowcount As Int32 = 0
            Dim oD As New Declarations
            Dim FileLoadStatus As String = ""
            Dim clsValidateEDI As New ValidateEDI
            Dim iStatus As Integer = 0

            Dim InterchangeDate As String = ""
            Dim TransactionSetPurposeCode As String = ""
            Dim TransactionSetIdentifierCode As String = ""
            Dim ImplementationConventionReference As String = ""
            Dim InterchangeTrailer As String = ""
            Dim InterchangeSenderID As String = ""
            Dim InterchangeReceiverID As String = ""
            Dim InterchangeTime As String = ""
            Dim InterchangeControlNumber As String = ""

            'Public Function byFile(ByVal FilePath As String, ByVal FileName As String, ByVal ExpectedEDI As String) As Int32
            If _OverrideProcessYearMonth = "1" Then
                bStatus = True
                Return bStatus
            End If
            Try
                Dim RE As Int32 = 0
                If Not File.Exists(FilePath + FileName) Then
                    log.ExceptionDetails("DCSGlobal.EDI.ValidateEDI.byFile", FileName + " Does Not Exist ")
                    bStatus = False
                End If
                Using r As New StreamReader(FilePath + FileName)
                    oD.RowProcessedFlag = 0
                    line = r.ReadLine()
                    'Console.WriteLine(line)
                    oD.DataElementSeparator = Mid(line, 4, 1)
                    Do While (Not line Is Nothing)
                        ' Add this line to list.
                        last = line
                        line = line.Replace("~", "")
                        rowcount = rowcount + 1
                        oD.ediRowRecordType = ss.ParseDemlimtedStringEDI(line, oD.DataElementSeparator, 1)
                        oD.CurrentRowData = line
                        line = r.ReadLine

                        'This data is used for future purpose. Incase if we want to record this in insertfile method. For now only 3 paramters are passing to Insert File.
                        If oD.ediRowRecordType = "ISA" Then
                            InterchangeSenderID = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 7)
                            InterchangeReceiverID = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 9)
                            InterchangeDate = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 10)
                            InterchangeTime = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 11)
                            InterchangeControlNumber = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 14)
                        End If
                        If oD.ediRowRecordType = "BHT" Then
                            TransactionSetPurposeCode = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                        End If
                        If oD.ediRowRecordType = "ST" Then
                            TransactionSetIdentifierCode = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)
                            ImplementationConventionReference = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)
                        End If
                        If oD.ediRowRecordType = "IEA" Then
                            InterchangeTrailer = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                        End If
                    Loop
                    If _OverrideProcessYearMonth = "0" Then
                        Dim sProcessYearMonth = Convert.ToString(_ProcessYearMonth)
                        Dim sGetMonthYear = Convert.ToString(InterchangeDate).Substring(0, Len(InterchangeDate) - 2)
                        If sGetMonthYear.Trim() = sProcessYearMonth.Trim() Then
                            bStatus = True
                            Return bStatus
                        Else
                            FileLoadStatus = "ANON"


                            iStatus = clsValidateEDI.InsertFileName(_connectionString, FilePath, FileName, FileLoadStatus)
                            bStatus = False
                        End If
                    Else
                        bStatus = True  ' Continue reading all the files without checking InterchangeDate. Because of Override true, ignoring the check.
                        Return bStatus
                    End If
                End Using
            Catch ex As Exception
                log.ExceptionDetails(_app, ex)
            End Try
            Return bStatus
        End Function

        Public Function IsValidProcessInterchangeDate(ByVal FilePath As String, ByVal FileName As String) As Boolean
            Dim bStatus As Boolean = False
            Dim last As String = String.Empty
            Dim line As String = String.Empty
            Dim rowcount As Int32 = 0
            Dim oD As New Declarations

            Dim FileLoadStatus As String = ""
            Dim clsValidateEDI As New ValidateEDI
            Dim iStatus As Integer = 0

            Dim InterchangeDate As String = ""
            Dim TransactionSetPurposeCode As String = ""
            Dim TransactionSetIdentifierCode As String = ""
            Dim ImplementationConventionReference As String = ""
            Dim InterchangeTrailer As String = ""
            Dim InterchangeSenderID As String = ""
            Dim InterchangeReceiverID As String = ""
            Dim InterchangeTime As String = ""
            Dim InterchangeControlNumber As String = ""
            Dim sProcessYearMonth = Convert.ToString(_ProcessYearMonth)
            'Public Function byFile(ByVal FilePath As String, ByVal FileName As String, ByVal ExpectedEDI As String) As Int32
            Try
                Dim RE As Int32 = 0
                If Not File.Exists(FilePath + FileName) Then
                    log.ExceptionDetails("DCSGlobal.EDI.ValidateEDI.byFile", FileName + " Does Not Exist ")
                    bStatus = False
                End If
                'Here is the logic needs to change 
                Using r As New StreamReader(FilePath + FileName)
                    oD.RowProcessedFlag = 0
                    line = r.ReadLine()
                    'Console.WriteLine(line)
                    oD.DataElementSeparator = Mid(line, 4, 1)
                    Do While (Not line Is Nothing)
                        ' Add this line to list.
                        last = line
                        line = line.Replace("~", "")
                        rowcount = rowcount + 1
                        oD.ediRowRecordType = ss.ParseDemlimtedStringEDI(line, oD.DataElementSeparator, 1)
                        oD.CurrentRowData = line
                        line = r.ReadLine

                        'This data is used for future purpose. Incase if we want to record this in insertfile method. For now only 3 paramters are passing to Insert File.
                        If oD.ediRowRecordType = "ISA" Then
                            InterchangeSenderID = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 7)
                            InterchangeReceiverID = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 9)
                            InterchangeDate = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 10)
                            InterchangeTime = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 11)
                            InterchangeControlNumber = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 14)
                        End If
                        If oD.ediRowRecordType = "BHT" Then
                            TransactionSetPurposeCode = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                        End If
                        If oD.ediRowRecordType = "ST" Then
                            TransactionSetIdentifierCode = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)
                            ImplementationConventionReference = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)
                        End If
                        If oD.ediRowRecordType = "IEA" Then
                            InterchangeTrailer = ss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                        End If
                    Loop

                    If Len(sProcessYearMonth.Trim()) = 0 Then
                        bStatus = True
                        Return bStatus
                    Else
                        If Len(sProcessYearMonth.Trim()) = 4 AndAlso IsNumeric(sProcessYearMonth) Then
                            Dim sGetMonthYear = Convert.ToString(InterchangeDate).Substring(0, Len(InterchangeDate) - 2)
                            If sGetMonthYear.Trim() = sProcessYearMonth.Trim() Then
                                bStatus = True
                                Return bStatus
                            Else
                                FileLoadStatus = "ProcessYYMMNotMatch"
                                'In above 2 cases, we are not inserting FileName in this procedure. Insertion will take care 
                                'at the time of Importing..as BAU i.e in val.byFile under LoadFolder or LoadFolders.
                                iStatus = clsValidateEDI.InsertFileName(_connectionString, FilePath, FileName, FileLoadStatus)
                                bStatus = False
                            End If
                        Else
                            'Some other issues. Any file shold not fall under this category. Because we are already taking care in 
                            'Console loader - Program
                            FileLoadStatus = "FAIL"
                            bStatus = False
                            Return bStatus
                        End If
                    End If
                End Using
            Catch ex As Exception
                log.ExceptionDetails(_app, ex)
            End Try
            Return bStatus
        End Function

        Public Function LoadFolder() As Int32
            Dim _FileName As String = String.Empty
            Dim _FQFN As String = String.Empty
            Dim RE As Int32 = 0
            Dim ARE As Integer = 0
            Dim fp As New FilePrep
            Dim val As New ValidateEDI
            Dim _functionName As String = _app + ".LoadFolder"
            ' make a reference to a directory

            Dim _EDI As New Import835()


            _InputFolder = _InputFolder + "\"



            _FileEXT = ss.ParseDemlimtedStringEDI(_FileFilter, ".", 2)




            fp.ConnectionString = _connectionString
            val.ConnectionString = _connectionString
            _EDI.ConnectionString = _connectionString


            Try
                Dim Directory As New IO.DirectoryInfo(_InputFolder)
                Dim DirectoryList As IO.FileInfo() = Directory.GetFiles(_FileFilter)
                Dim DirectoryFolderList As DirectoryInfo() = Directory.GetDirectories()

                Dim File As FileInfo




                Dim bIsValidProcessYearMonth As Boolean
                Dim ext As String = String.Empty


                'for each folder 

                'list the names of all files in the specified directory
                For Each File In DirectoryList
                    ' ListBox1.Items.Add(dra)
                    RE = 0

                    _FileName = Convert.ToString(File)
                    RE = fio.CheckExtension(_FileName, _FileFilter)








                    Console.Write(File)
                    bIsValidProcessYearMonth = False
                    bIsValidProcessYearMonth = IsValidProcessInterchangeDate(_InputFolder, _FileName)
                    If bIsValidProcessYearMonth = False Then
                        Continue For
                    End If


                    Try


                        ' in all these sernios if we get any thing back other than 0 it was failure so move on.
                        ' it has already been logged so we just move on

                        If RE = 0 Then
                            RE = fp.SingleFile(_InputFolder + _FileName)

                            If RE <> 0 Then
                                ARE = 1
                            End If

                        End If


                        If RE = 0 Then
                            'check to see if the file is a duplicate
                            'val.ProcessYearMonth = _ProcessYearMonth
                            'val.OverrideProcessYearMonth = _OverrideProcessYearMonth
                            ' get type of edi and the fileID


                            RE = val.byFile(_InputFolder, _FileName, "835")

                            If RE <> 0 Then
                                ARE = 1
                            End If

                        End If

                        If RE = 0 Then

                            _EDI.InputFolder = _InputFolder
                            _EDI.SuccessFolder = _SuccessFolder
                            _EDI.FailedFolder = _FailedFolder
                            _EDI.DuplicateFolder = _DuplicateFolder
                            RE = _EDI.Import(_InputFolder + _FileName)
                            RE = 0

                            _FQFN = _InputFolder + _FileName


                            If RE <> 0 Then
                                ARE = 1
                            End If

                        End If









                        RE = FM.Move(RE, _FileName)
                        If RE <> 0 Then
                            ARE = 1
                        End If

                    Catch ex As Exception

                        log.ExceptionDetails(_app, ex)

                    End Try

                Next

                File = Nothing

            Catch ix As System.IO.DriveNotFoundException
                ARE = 1

                ' add log and email here
                log.ExceptionDetails(_app, ix)

            Catch ix As System.IO.DirectoryNotFoundException
                ARE = 1
                ' add log and email here

                log.ExceptionDetails(_app, ix)
            Catch ix As System.IO.PathTooLongException
                ARE = 1
                ' add log and email here

                log.ExceptionDetails(_app, ix)

            Catch ex As Exception
                ARE = 1
                log.ExceptionDetails(_app, ex)


            End Try


            fp = Nothing
            val = Nothing
            _EDI = Nothing

            Return ARE
        End Function

        Public Function LoadFolders() As Int32
            Dim _FileName As String = String.Empty
            Dim _FQFN As String = String.Empty
            Dim RE As Int32 = 0
            Dim fp As New FilePrep
            Dim val As New ValidateEDI
            Dim _EDI As New Import835
            Dim _functionName As String = _app + ".LoadFolder"
            ' make a reference to a directory


            Dim bIsValidProcessYearMonth As Boolean = False

            fp.ConnectionString = _connectionString
            val.ConnectionString = _connectionString
            _EDI.ConnectionString = _connectionString

            Try
                Dim Directory As New IO.DirectoryInfo(_baseFolder)
                Dim DirectoryFolderList As DirectoryInfo() = Directory.GetDirectories()
                Dim DirectoryFolder As DirectoryInfo

                'for each folder 

                'list the names of all files in the specified directory

                For Each DirectoryFolder In DirectoryFolderList


                    Dim _DirectoryFolderToLoad As String = String.Empty

                    _DirectoryFolderToLoad = _baseFolder + DirectoryFolder.ToString + "\" + _inputFolder

                    Dim DirectoryFolderToLoad As New IO.DirectoryInfo(_DirectoryFolderToLoad)
                    Dim DirectoryList As IO.FileInfo() = DirectoryFolderToLoad.GetFiles(_FileFilter)
                    Dim File As FileInfo

                    '_inputFolder = DirectoryFolderToLoad.FullName.ToString()

                    For Each File In DirectoryList
                        ' ListBox1.Items.Add(dra)

                        RE = 0
                        _FileName = Convert.ToString(File)
                        Console.Write(vbNewLine + File.ToString())

                        bIsValidProcessYearMonth = False
                        bIsValidProcessYearMonth = IsValidProcessInterchangeDate(_DirectoryFolderToLoad + "\", _FileName)
                        If bIsValidProcessYearMonth = False Then
                            Continue For
                        End If

                        Try


                            ' in all these sernios if we get any thing back other than 0 it was failure so move on.
                            ' it has already been logged so we just move on

                            If RE = 0 Then

                                'RE = fp.SingleFile(_inputFolder + "\" + _FileName, _inputFolder + "\" + _FileName + ".edi835")
                                RE = fp.SingleFile(DirectoryFolderToLoad.FullName.ToString() + "\" + _FileName, DirectoryFolderToLoad.FullName.ToString() + "\" + _FileName + ".DCS_835")


                                _FileName = _FileName + ".DCS_835"
                            End If


                            If RE = 0 Then
                                'check to see if the file is a duplicate
                                'RE = val.byFile(_inputFolder + "\", _FileName, "835")
                                'val.ProcessYearMonth = _ProcessYearMonth
                                'val.OverrideProcessYearMonth = _OverrideProcessYearMonth
                                RE = val.byFile(DirectoryFolderToLoad.FullName.ToString() + "\", _FileName, "835")

                            End If

                            If RE = 0 Then
                                'parse the file 
                                'RE = _EDI.Import(_inputFolder + _FileName)
                                'RE = 0
                                '_FQFN = _inputFolder + _FileName
                                RE = _EDI.Import(DirectoryFolderToLoad.FullName.ToString() + "\" + _FileName)
                                RE = 0
                                _FQFN = DirectoryFolderToLoad.FullName.ToString() + "\" + _FileName

                            End If

                            FM.Move(RE, _FileName, _baseFolder + DirectoryFolder.ToString + "\")

                        Catch ix As System.IO.DriveNotFoundException


                            ' add log and email here
                            log.ExceptionDetails(_app, ix)

                        Catch ix As System.IO.DirectoryNotFoundException

                            ' add log and email here

                            log.ExceptionDetails(_app, ix)
                        Catch ix As System.IO.PathTooLongException

                            ' add log and email here

                            log.ExceptionDetails(_app, ix)

                        Catch ex As Exception

                            log.ExceptionDetails(_app, ex)


                        End Try

                    Next

                Next

            Catch ix As System.IO.DriveNotFoundException


                ' add log and email here
                log.ExceptionDetails(_app, ix)

            Catch ix As System.IO.DirectoryNotFoundException

                ' add log and email here

                log.ExceptionDetails(_app, ix)
            Catch ix As System.IO.PathTooLongException

                ' add log and email here

                log.ExceptionDetails(_app, ix)

            Catch ex As Exception

                log.ExceptionDetails(_app, ex)


            End Try


            Return RE
        End Function



    End Class

End Namespace

