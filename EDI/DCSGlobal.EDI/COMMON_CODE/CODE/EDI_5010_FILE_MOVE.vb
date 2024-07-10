
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
Imports System.Configuration

Imports System.IO
Imports System.IO.File
Imports System.Text
Imports System.Threading

Imports DCSGlobal.BusinessRules.Logging
Imports DCSGlobal.BusinessRules.CoreLibrary.StringHandlingStuff
Imports DCSGlobal.BusinessRules.CoreLibrary.IO
Imports DCSGlobal.BusinessRules.CoreLibraryII
Imports DCSGlobal.BusinessRules.FileTransferClient




Namespace DCSGlobal.EDI



    Public Class EDI_5010_FILE_MOVE

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







        Private _baseFolder As String

        Private _connectionString As String = String.Empty

        Private _SMTPServer As String = String.Empty
        Private _FromMailAddress As String = String.Empty
        Private _ToMailAddress As String = String.Empty
        Private _Input As String = String.Empty
        Private _Failed As String = String.Empty
        Private _Success As String = String.Empty
        Private _Duplicate As String = String.Empty
        Private _ParseTree As Int32 = 0
        Private _FinalPath As String = String.Empty

        Private _functionName As String = String.Empty

        Private _VAULT_ROOT_DIRECTORY As String = String.Empty
        Private _VAULT_CLIENT_DIRECTORY As String = String.Empty




        Private _OverWriteIfFileExist As Boolean = CBool(ConfigurationManager.AppSettings("DCS_VAULT_OverWriteIfFileExist"))
        Private _PassUserCredentials As String = ConfigurationManager.AppSettings("DCS_VAULT_PassUserCredentials").ToString()
        Private _UserName As String = ConfigurationManager.AppSettings("DCS_VAULT_UserName").ToString()
        Private _Password As String = ConfigurationManager.AppSettings("DCS_VAULT_Password").ToString()
        Private _Domain As String = ConfigurationManager.AppSettings("DCS_VAULT_Domain").ToString()
        Private _vaulterrorfoder As String = ConfigurationManager.AppSettings("DCS_VAULT_ERRORFOLDER").ToString()
        Private _AlternateEndpoint As String = ConfigurationManager.AppSettings("DCS_VAULT_AlternateEndPointAddress").ToString()

        Public Sub New()
            If Debugger.IsAttached Then
                _VERBOSE = 1
                _DEBUG_LEVEL = 1

            End If

            _CONSOLE_NAME = "EDI_5010_FILE_MOVE"
            _CLASS_NAME = "EDI_5010_FILE_MOVE"

        End Sub

        Public Sub Dispose() Implements System.IDisposable.Dispose

            GC.SuppressFinalize(Me)

            Console.WriteLine("Object " & GetHashCode() & " disposed.")
        End Sub

        Protected Overrides Sub Finalize()
            log = Nothing
            ' em = Nothing
            d = Nothing
            ss = Nothing
            Dispose()
            Console.WriteLine("Object " & GetHashCode() & " finalized.")
        End Sub




        Public WriteOnly Property AlternateEndpoint As String
            Set(value As String)
                _AlternateEndpoint = value

            End Set
        End Property


        Public WriteOnly Property vaulterrorfoder As String
            Set(value As String)
                _vaulterrorfoder = value

            End Set
        End Property

        Public WriteOnly Property Domain As String
            Set(value As String)
                _Domain = value

            End Set
        End Property

        Public WriteOnly Property Password As String
            Set(value As String)
                _Password = value

            End Set
        End Property

        Public WriteOnly Property VaultUserName As String
            Set(value As String)
                _UserName = value

            End Set
        End Property

        Public WriteOnly Property PassUserCredentials As String
            Set(value As String)
                _PassUserCredentials = value

            End Set
        End Property

        Public WriteOnly Property OverWriteIfFileExist As Boolean
            Set(value As Boolean)
                _OverWriteIfFileExist = value

            End Set
        End Property



        Public WriteOnly Property InstanceName As String
            Set(value As String)
                _CONSOLE_NAME = value
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


        Public WriteOnly Property ConnectionString As String
            Set(value As String)
                _connectionString = value
                log.ConnectionString = value
            End Set
        End Property


        Public WriteOnly Property SMTPServer As String
            Set(value As String)
                _SMTPServer = value
                ' em.Server = value
            End Set
        End Property

        Public WriteOnly Property FromMailAddress As String
            Set(value As String)
                _FromMailAddress = value
                'em.SendFrom = value
            End Set
        End Property

        Public WriteOnly Property ToMailAddress As String
            Set(value As String)
                _ToMailAddress = value
                ' em.SendTo = value
            End Set
        End Property


        Public Property BaseFolder() As String
            Get
                Return _baseFolder
            End Get
            Set(ByVal value As String)
                _baseFolder = value
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


        Public Property FinalPath() As String
            Get
                Return _FinalPath

            End Get
            Set(ByVal value As String)


                If (ss.GetLastChr(value) <> "\") Then
                    value = value + "\"
                End If
                _FinalPath = value

            End Set
        End Property


        Public Property Input() As String
            Get
                Return _Input

            End Get
            Set(ByVal value As String)


                If (ss.GetLastChr(value) <> "\") Then
                    value = value + "\"
                End If
                _Input = value

            End Set
        End Property


        Public Property Failed() As String
            Get
                Return _Failed

            End Get
            Set(ByVal value As String)

                If (ss.GetLastChr(value) <> "\") Then
                    value = value + "\"
                End If
                _Failed = value


            End Set
        End Property





        Public Property Success() As String
            Get
                Return _Success

            End Get
            Set(ByVal value As String)

                If (ss.GetLastChr(value) <> "\") Then
                    value = value + "\"
                End If
                _Success = value

            End Set
        End Property


        Public Property Duplicate() As String
            Get
                Return _Duplicate

            End Get
            Set(ByVal value As String)

                If (ss.GetLastChr(value) <> "\") Then
                    value = value + "\"
                End If
                _Duplicate = value

            End Set
        End Property


        Private Sub GetVaultPaths()
            _FUNCTION_NAME = "Sub GetVaultPaths()"
            Try


                Using Con As New SqlConnection(_connectionString)
                    Con.Open()
                    Using cmd As New SqlCommand("[usp_EDI_5010_HIPAA_837_GET_VAULT_PATH]", Con)

                        cmd.CommandType = CommandType.StoredProcedure

                        cmd.Parameters.Add("@HOSP_CODE", SqlDbType.VarChar, 255)
                        cmd.Parameters("@HOSP_CODE").Direction = ParameterDirection.Output

                        cmd.Parameters.Add("@CLIENT_NAME", SqlDbType.VarChar, 255)
                        cmd.Parameters("@CLIENT_NAME").Direction = ParameterDirection.Output


                        cmd.Parameters.Add("@VAULT_ROOT_DIRECTORY", SqlDbType.VarChar, 260)
                        cmd.Parameters("@VAULT_ROOT_DIRECTORY").Direction = ParameterDirection.Output


                        cmd.Parameters.Add("@VAULT_CLIENT_DIRECTORY", SqlDbType.VarChar, 260)
                        cmd.Parameters("@VAULT_CLIENT_DIRECTORY").Direction = ParameterDirection.Output

                        cmd.ExecuteNonQuery()

                        'Commented below line. Because getting error when we load files from the folder.
                        _VAULT_ROOT_DIRECTORY = Convert.ToString(cmd.Parameters("@VAULT_ROOT_DIRECTORY").Value)
                        _VAULT_CLIENT_DIRECTORY = Convert.ToString(cmd.Parameters("@VAULT_CLIENT_DIRECTORY").Value)
                        _HOSP_CODE = Convert.ToString(cmd.Parameters("@HOSP_CODE").Value)
                        _CLIENT_NAME = Convert.ToString(cmd.Parameters("@CLIENT_NAME").Value)



                        ''    [usp_EDI_5010_HIPAA_837_GET_VAULT_PATH]()
                        ''  rr = 0
                    End Using
                    Con.Close()
                End Using
            Catch ex As Exception
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "", ex)
            End Try

        End Sub








        Public Function Move(ByVal RE As Integer, ByVal FileName As String, ByVal FilePath As String) As Int32

            _FUNCTION_NAME = "Function Move(ByVal RE As Integer, ByVal FileName As String, ByVal FilePath As String) As Int32"


            Dim _FQFN As String = _Success + FileName + ".TXT"
            Dim _FN As String = DateTime.Today.ToString("MMddyyyy") + "\\" + FileName + ".TXT"
            Dim _FN1 As String = FileName + ".TXT"

            Try

                GetVaultPaths()

                If IO.File.Exists(_Success + FileName) = True Then
                    'they use to just delete it and over right it with this one so no clue what was in thte old one
                    'should move both to a dulpicte folder and 
                    ' System.IO.File.Delete(_FQFN)
                    System.IO.File.Move(_Success + FileName, _Success + FileName + "_____" + Convert.ToString(_FILE_ID) + ".dupe")

                End If

                System.IO.File.Move(_Input + FileName, _Success + _FN1)




                Try
                    Using V As New VaultService(_OverWriteIfFileExist, _PassUserCredentials, _UserName, _Password, _Domain, _vaulterrorfoder, _AlternateEndpoint)
                        V.DCSVaultSaveFile(File.ReadAllBytes(_FQFN), _CLIENT_NAME, _HOSP_CODE, _VAULT_ROOT_DIRECTORY, _VAULT_CLIENT_DIRECTORY, _FN, "")
                    End Using
                Catch ex As Exception
                    _VAULT_ROOT_DIRECTORY = "failed"
                    _VAULT_CLIENT_DIRECTORY = "failed"
                    _FN = "failed"
                End Try


                Using e As New EDI_5010_LOGGING
                    e.ConnectionString = _connectionString
                    e.TransactionSetIdentifierCode = _TransactionSetIdentifierCode
                    e.UpdateFileLocation(CInt(_FILE_ID), _Success, _VAULT_ROOT_DIRECTORY, _VAULT_CLIENT_DIRECTORY, _FN)
                End Using

            Catch ex As System.Exception
                log.ExceptionDetails(_CONSOLE_NAME, _CLASS_NAME, _FUNCTION_NAME, "EDI_5010_MOVE FAILED FOR FILE ID  = " + Convert.ToString(_FILE_ID), ex)
            End Try





            Return 0
        End Function

    End Class

End Namespace

