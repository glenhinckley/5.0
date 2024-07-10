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





    Public Class Load278
        Implements IDisposable

        Private _InstanceName As String = String.Empty
        Dim _app As String
        Private _baseFolder As String
        Dim log As New logExecption()
        Private _connectionString As String = String.Empty
        Private em As New Email()
        Private _SMTPServer As String = String.Empty
        Private _FromMailAddress As String = String.Empty
        Private _ToMailAddress As String = String.Empty
        Private _Input As String = String.Empty
        Private _Failed As String = String.Empty
        Private _Success As String = String.Empty
        Private _Duplicate As String = String.Empty
        Private _ParseTree As Int32 = 0
        Private _ProcessYearMonth As String = String.Empty
        Private _OverrideProcessYearMonth As String = String.Empty
        Private FM As New FileMove
        Private _FileFilter As String = String.Empty


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
                _app = _InstanceName + ".Loads278"
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


        Public Property BaseFolder() As String
            Get
                Return _baseFolder
            End Get
            Set(ByVal value As String)
                _baseFolder = value
                FM.BaseFolder = value
            End Set
        End Property


        Public Property Input() As String
            Get
                Return _Input

            End Get
            Set(ByVal value As String)
                _Input = value
                FM.Input = value
            End Set
        End Property


        Public Property Failed() As String
            Get
                Return _Failed

            End Get
            Set(ByVal value As String)
                _Failed = value
                FM.Failed = value
            End Set
        End Property

        Public Property FileFilter() As String
            Get
                Return _FileFilter

            End Get
            Set(ByVal value As String)


                _FileFilter = value


            End Set
        End Property



        Public Property Success() As String
            Get
                Return _Success

            End Get
            Set(ByVal value As String)
                _Success = value
                FM.Success = value
            End Set
        End Property


        Public Property Duplicate() As String
            Get
                Return _Duplicate

            End Get
            Set(ByVal value As String)
                _Duplicate = value
                FM.Duplicate = value
            End Set
        End Property

        Public Property ProcessYearMonth() As String
            Get
                Return _ProcessYearMonth

            End Get
            Set(ByVal value As String)
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

        Public WriteOnly Property ParseTree As Integer

            Set(value As Integer)
                _ParseTree = value
            End Set
        End Property

        Private FileList As New List(Of String)
        Private V As Boolean = False

        Public Function IsValidProcessInterchangeDate(ByVal FilePath As String, ByVal FileName As String) As Boolean
            Dim bStatus As Boolean = False
            Dim last As String = String.Empty
            Dim line As String = String.Empty
            Dim rowcount As Int32 = 0
            Dim oD As New Declarations
            Dim objss As New StringStuff
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
                        oD.ediRowRecordType = objss.ParseDemlimtedStringEDI(line, oD.DataElementSeparator, 1)
                        oD.CurrentRowData = line
                        line = r.ReadLine

                        'This data is used for validation purpose. We are inserting this record at the time of insertfile method. For now only 3 paramters are passing to Insert File.
                        If oD.ediRowRecordType = "ISA" Then
                            InterchangeSenderID = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 7)
                            InterchangeReceiverID = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 9)
                            InterchangeDate = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 10)
                            InterchangeTime = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 11)
                            InterchangeControlNumber = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 14)
                        End If
                        If oD.ediRowRecordType = "BHT" Then
                            TransactionSetPurposeCode = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
                        End If
                        If oD.ediRowRecordType = "ST" Then
                            TransactionSetIdentifierCode = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 2)
                            ImplementationConventionReference = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 4)
                        End If
                        If oD.ediRowRecordType = "IEA" Then
                            InterchangeTrailer = objss.ParseDemlimtedStringEDI(oD.CurrentRowData, oD.DataElementSeparator, 3)
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
                                'This is Failed case, and we are logging file name here.
                                'In success case , we are not inserting FileName here. Insertion will take care 
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

         

        Public Function LoadFolders() As Int32
            Dim _FileName As String = String.Empty
            Dim _FQFN As String = String.Empty
            Dim RE As Int32 = 0
            Dim fp As New FilePrep
            Dim val As New ValidateEDI
            Dim _EDI As New Import278
            Dim _functionName As String = _app + ".LoadFolder"
            ' make a reference to a directory

            Dim _inputFolder As String = _Input
            Dim _successFolder As String = _Success
            Dim _failFolder As String = _Failed
            Dim _duplicateFolder As String = _Duplicate

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

                                'RE = fp.SingleFile(_inputFolder + "\" + _FileName, _inputFolder + "\" + _FileName + ".edi278")
                                RE = fp.SingleFile(DirectoryFolderToLoad.FullName.ToString() + "\" + _FileName, DirectoryFolderToLoad.FullName.ToString() + "\" + _FileName + ".edi278")
                                _FileName = _FileName + ".edi278"
                            End If

                            If RE = 0 Then
                                'check to see if the file is a duplicate
                                RE = val.byFile(DirectoryFolderToLoad.FullName.ToString() + "\", _FileName, "278")

                            End If

                            If RE = 0 Then
                                'parse the file 
                                'RE = _EDI.Import(_inputFolder + _FileName)
                                'RE = 0
                                '_FQFN = _inputFolder + _FileName
                                '      RE = _EDI.Import(DirectoryFolderToLoad.FullName.ToString() + "\" + _FileName)
                                '_FQFN = _inputFolder + _FileName
                                RE = _EDI.ImportByFileName(DirectoryFolderToLoad.FullName.ToString() + "\", _FileName)

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


        Public Function LoadFolder() As Int32
            Dim _FileName As String = String.Empty
            Dim _FQFN As String = String.Empty
            Dim RE As Int32 = 0
            Dim fp As New FilePrep
            Dim val As New ValidateEDI
            Dim _EDI As New Import278
            Dim _functionName As String = _app + ".LoadFolder"
            ' make a reference to a directory

            Dim _inputFolder As String = _baseFolder + _Input + "\"
            Dim _successFolder As String = _baseFolder + _Success + "\"
            Dim _failFolder As String = _baseFolder + _Failed + "\"
            Dim _duplicateFolder As String = _baseFolder + _Duplicate + "\"


            fp.ConnectionString = _connectionString
            val.ConnectionString = _connectionString
            _EDI.ConnectionString = _connectionString


            Try
                Dim Directory As New IO.DirectoryInfo(_inputFolder)
                Dim DirectoryList As IO.FileInfo() = Directory.GetFiles()
                Dim DirectoryFolderList As DirectoryInfo() = Directory.GetDirectories()
                Dim File As FileInfo
                Dim bIsValidProcessYearMonth As Boolean


                'for each folder 

                'list the names of all files in the specified directory
                For Each File In DirectoryList
                    ' ListBox1.Items.Add(dra)
                    RE = 0

                    _FileName = Convert.ToString(File)
                    Console.Write(File)
                    bIsValidProcessYearMonth = False
                    bIsValidProcessYearMonth = IsValidProcessInterchangeDate(_inputFolder, _FileName)
                    If bIsValidProcessYearMonth = False Then
                        Continue For
                    End If
                    Try


                        ' in all these sernios if we get any thing back other than 0 it was failure so move on.
                        ' it has already been logged so we just move on

                        If RE = 0 Then

                            RE = fp.SingleFile(_inputFolder + _FileName, _inputFolder + _FileName + ".edi278")

                            _FileName = _FileName + ".edi278"
                        End If


                        If RE = 0 Then
                            'check to see if the file is a duplicate
                            'val.ProcessYearMonth = _ProcessYearMonth
                            'val.OverrideProcessYearMonth = _OverrideProcessYearMonth
                            RE = val.byFile(_inputFolder, _FileName, "278")
                        End If

                        If RE = 0 Then
                            'parse the file 
                            'RE = _EDI.Import(_inputFolder + _FileName)
                            'RE = 0
                            '_FQFN = _inputFolder + _FileName

                            '     RE = _EDI.Import(_inputFolder + _FileName)
                            RE = 0
                            _FQFN = _inputFolder + _FileName


                        End If




                        FM.Move(RE, _FileName, _baseFolder)


                    Catch ex As Exception

                        log.ExceptionDetails(_app, ex)

                    End Try

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

        'Public Function LoadFolder_Old() As Int32
        '    Dim _FileName As String = String.Empty
        '    Dim _FQFN As String = String.Empty
        '    Dim RE As Int32 = 0
        '    Dim fp As New FilePrep
        '    Dim val As New ValidateEDI
        '    Dim _EDI As New Import837
        '    Dim _functionName As String = _app + ".LoadFolder"
        '    ' make a reference to a directory

        '    Dim _inputFolder As String = _Input
        '    Dim _successFolder As String = _Success
        '    Dim _failFolder As String = Failed
        '    Dim _duplicateFolder As String = _Duplicate


        '    fp.ConnectionString = _connectionString
        '    val.ConnectionString = _connectionString


        '    Try
        '        Dim Directory As New IO.DirectoryInfo(_inputFolder)
        '        Dim DirectoryList As IO.FileInfo() = Directory.GetFiles()
        '        Dim File As FileInfo

        '        'list the names of all files in the specified directory
        '        For Each File In DirectoryList
        '            ' ListBox1.Items.Add(dra)


        '            _FileName = Convert.ToString(File)
        '            Console.Write(File)



        '            Try


        '                ' in all these sernios if we get any thing back other than 0 it was failure so move on.
        '                ' it has already been logged so we just move on

        '                If RE = 0 Then

        '                    RE = fp.SingleFile(_inputFolder + _FileName, _inputFolder + _FileName + ".edi278")

        '                    _FileName = _FileName + ".edi278"
        '                End If


        '                If RE = 0 Then
        '                    'check to see if the file is a duplicate
        '                    RE = val.byFile(_inputFolder, _FileName, "278")

        '                End If



        '                If RE = 0 Then
        '                    'parse the file 
        '                    RE = _EDI.Import(_inputFolder + _FileName)
        '                    'RE = 0

        '                    _FQFN = _inputFolder + _FileName
        '                End If


        '                Select Case RE

        '                    Case Is = 0

        '                        Try

        '                            If IO.File.Exists(_successFolder + _FileName) = True Then

        '                                'they use to just delete it and over right it with this one so no clue what was in thte old one
        '                                'should move both to a dulpicte folder and 
        '                                ' System.IO.File.Delete(_FQFN)
        '                                System.IO.File.Move(_inputFolder + _FileName, _duplicateFolder + "\" + _FileName + ".dupe-")

        '                            Else

        '                                System.IO.File.Move(_inputFolder + _FileName, _successFolder + "\" + _FileName)


        '                            End If
        '                            '    System.IO.File.Move(sFileFullPath, sDestDirPath + "\" + sFileName)

        '                        Catch ix As System.IO.FileNotFoundException

        '                            log.ExceptionDetails(_functionName + " " + _FQFN, ix)

        '                        Catch ix As System.IO.FileLoadException

        '                            log.ExceptionDetails(_functionName + " " + _FQFN, ix)

        '                        Catch ix As System.IO.PathTooLongException

        '                            log.ExceptionDetails(_functionName + " " + _FQFN, ix)

        '                        Catch ex As System.Exception

        '                            log.ExceptionDetails(_functionName + " " + _FQFN, ex)
        '                        End Try




        '                        ' failed so move it to the failed folder and its already logged

        '                    Case Is > 0


        '                        Try

        '                            Try
        '                                '   em.SendGenericEmail(_failFolder + _FileName, "File failure ", _functionName + " " + _FQFN)
        '                            Catch ex As Exception
        '                                log.ExceptionDetails(_functionName + " " + _FQFN + " Send failed email failure ", ex)
        '                            End Try
        '                            If IO.File.Exists(_failFolder + _FileName) = True Then

        '                                'they use to just delete it and over right it with this one so no clue what was in thte old one
        '                                'should move both to a dulpicte folder and 
        '                                ' System.IO.File.Delete(_FQFN)
        '                                System.IO.File.Move(_inputFolder + _FileName, _duplicateFolder + "\" + _FileName + ".dupe-")

        '                            Else

        '                                System.IO.File.Move(_inputFolder + _FileName, _failFolder + "\" + _FileName)

        '                            End If

        '                        Catch ix As System.IO.FileNotFoundException

        '                            log.ExceptionDetails(_functionName + " " + _FQFN, ix)

        '                        Catch ix As System.IO.FileLoadException

        '                            log.ExceptionDetails(_functionName + " " + _FQFN, ix)

        '                        Catch ix As System.IO.PathTooLongException

        '                            log.ExceptionDetails(_functionName + " " + _FQFN, ix)

        '                        Catch ex As System.Exception

        '                            log.ExceptionDetails(_functionName + " " + _FQFN, ex)
        '                        End Try



        '                        ' dulicate so move it to the duplicate folder and its already logged
        '                    Case Is < 0


        '                        Try


        '                            Try
        '                                '  em.SendGenericEmail(_duplicateFolder + _FileName, "File duplicate " + Convert.ToString(RE), _functionName + " " + _FQFN)
        '                            Catch ex As Exception
        '                                log.ExceptionDetails(_functionName + " " + _FQFN + " Send duplicate email failure ", ex)
        '                            End Try

        '                            If IO.File.Exists(_duplicateFolder + _FileName) = True Then

        '                                'they use to just delete it and over right it with this one so no clue what was in thte old one
        '                                'should move both to a dulpicte folder and 
        '                                ' System.IO.File.Delete(_FQFN)
        '                                System.IO.File.Move(_inputFolder + _FileName, _duplicateFolder + "\" + _FileName + ".dupe-")

        '                            Else

        '                                System.IO.File.Move(_inputFolder + _FileName, _duplicateFolder + "\" + _FileName)

        '                            End If


        '                        Catch ix As System.IO.FileNotFoundException

        '                            log.ExceptionDetails(_functionName + " " + _FQFN, ix)

        '                        Catch ix As System.IO.FileLoadException

        '                            log.ExceptionDetails(_functionName + " " + _FQFN, ix)

        '                        Catch ix As System.IO.PathTooLongException

        '                            log.ExceptionDetails(_functionName + " " + _FQFN, ix)

        '                        Catch ex As System.Exception

        '                            log.ExceptionDetails(_functionName + " " + _FQFN, ex)
        '                        End Try


        '                        ' who know what happend should never ever get here 
        '                    Case Else

        '                        log.ExceptionDetails(_functionName + " " + _FQFN, "boken")


        '                End Select



        '            Catch ex As Exception

        '                log.ExceptionDetails(_app, ex)

        '            End Try

        '        Next



        '    Catch ix As System.IO.DriveNotFoundException


        '        ' add log and email here
        '        log.ExceptionDetails(_app, ix)

        '    Catch ix As System.IO.DirectoryNotFoundException

        '        ' add log and email here

        '        log.ExceptionDetails(_app, ix)
        '    Catch ix As System.IO.PathTooLongException

        '        ' add log and email here

        '        log.ExceptionDetails(_app, ix)

        '    Catch ex As Exception

        '        log.ExceptionDetails(_app, ex)


        '    End Try


        '    Return RE
        'End Function





    End Class

End Namespace


