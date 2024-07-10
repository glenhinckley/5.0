Option Explicit On
Option Strict On
Option Compare Binary





Imports System.Data


Namespace DCSGlobal.EDI


    Public Class EDI_5010_999_00510X231A1_TABLES


        Inherits EDI_5010_COMMON_TABLES


        '******************************************************************************************************************
        '  all vars not declared here are in   EDI_5010_COMMON_DECS   it is inhreted from the tables for this type
        '  if you are going to use it on this class only it goes below if not put it in EDI_5010_COMMON_DECS
        '******************************************************************************************************************

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   A
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Public AK1 As New DataTable
        Public AK2 As New DataTable
        Public AK9 As New DataTable

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   C
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Public CTX As New DataTable


        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   I
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''


        Public IK3 As New DataTable
        Public IK4 As New DataTable
        Public IK5 As New DataTable





        'end 999 tables


        Public Sub BuildTables()

            BuildCommonTables()


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   AK1
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                AK1.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                AK1.Columns.Add("DOCUMENT_ID", GetType(Integer))
                AK1.Columns.Add("FILE_ID", GetType(Integer))
                AK1.Columns.Add("BATCH_ID", GetType(Integer))
                AK1.Columns.Add("ISA_ID", GetType(Integer))
                AK1.Columns.Add("GS_ID", GetType(Integer))
                AK1.Columns.Add("ST_ID", GetType(Integer))
                AK1.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                AK1.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                AK1.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                AK1.Columns.Add("999_AK2_GUID", GetType(Guid))
                AK1.Columns.Add("999_IK3_GUID", GetType(Guid))
                AK1.Columns.Add("999_IK4_GUID", GetType(Guid))
                AK1.Columns.Add("AK101", GetType(String))
                AK1.Columns.Add("AK102", GetType(String))
                AK1.Columns.Add("AK103", GetType(String))
                AK1.Columns.Add("ROW_NUMBER", GetType(Integer))
                AK1.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                AK1.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                AK1.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try



            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   AK2
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                AK2.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                AK2.Columns.Add("DOCUMENT_ID", GetType(Integer))
                AK2.Columns.Add("FILE_ID", GetType(Integer))
                AK2.Columns.Add("BATCH_ID", GetType(Integer))
                AK2.Columns.Add("ISA_ID", GetType(Integer))
                AK2.Columns.Add("GS_ID", GetType(Integer))
                AK2.Columns.Add("ST_ID", GetType(Integer))
                AK2.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                AK2.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                AK2.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                AK2.Columns.Add("999_AK2_GUID", GetType(Guid))
                AK2.Columns.Add("999_IK3_GUID", GetType(Guid))
                AK2.Columns.Add("999_IK4_GUID", GetType(Guid))
                AK2.Columns.Add("AK201", GetType(String))
                AK2.Columns.Add("AK202", GetType(String))
                AK2.Columns.Add("AK203", GetType(String))
                AK2.Columns.Add("ROW_NUMBER", GetType(Integer))
                AK2.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                AK2.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                AK2.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try




            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   AK9
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                AK9.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                AK9.Columns.Add("DOCUMENT_ID", GetType(Integer))
                AK9.Columns.Add("FILE_ID", GetType(Integer))
                AK9.Columns.Add("BATCH_ID", GetType(Integer))
                AK9.Columns.Add("ISA_ID", GetType(Integer))
                AK9.Columns.Add("GS_ID", GetType(Integer))
                AK9.Columns.Add("ST_ID", GetType(Integer))
                AK9.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                AK9.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                AK9.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                AK9.Columns.Add("999_AK2_GUID", GetType(Guid))
                AK9.Columns.Add("999_IK3_GUID", GetType(Guid))
                AK9.Columns.Add("999_IK4_GUID", GetType(Guid))
                AK9.Columns.Add("AK901", GetType(String))
                AK9.Columns.Add("AK902", GetType(String))
                AK9.Columns.Add("AK903", GetType(String))
                AK9.Columns.Add("AK904", GetType(String))
                AK9.Columns.Add("AK905", GetType(String))
                AK9.Columns.Add("AK906", GetType(String))
                AK9.Columns.Add("AK907", GetType(String))
                AK9.Columns.Add("AK908", GetType(String))
                AK9.Columns.Add("AK909", GetType(String))
                AK9.Columns.Add("ROW_NUMBER", GetType(Integer))
                AK9.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                AK9.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                AK9.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try







            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   CTX
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try

                CTX.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CTX.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CTX.Columns.Add("FILE_ID", GetType(Integer))
                CTX.Columns.Add("BATCH_ID", GetType(Integer))
                CTX.Columns.Add("ISA_ID", GetType(Integer))
                CTX.Columns.Add("GS_ID", GetType(Integer))
                CTX.Columns.Add("ST_ID", GetType(Integer))
                CTX.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CTX.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CTX.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CTX.Columns.Add("999_AK2_GUID", GetType(Guid))
                CTX.Columns.Add("999_IK3_GUID", GetType(Guid))
                CTX.Columns.Add("999_IK4_GUID", GetType(Guid))
                CTX.Columns.Add("CTX01", GetType(String))
                CTX.Columns.Add("CTX01_1", GetType(String))
                CTX.Columns.Add("CTX01_2", GetType(String))
                CTX.Columns.Add("CTX04", GetType(String))
                CTX.Columns.Add("CTX04_1", GetType(String))
                CTX.Columns.Add("CTX04_2", GetType(String))
                CTX.Columns.Add("CTX05", GetType(String))
                CTX.Columns.Add("CTX05_1", GetType(String))
                CTX.Columns.Add("CTX05_2", GetType(String))
                CTX.Columns.Add("ROW_NUMBER", GetType(Integer))
                CTX.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CTX.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CTX.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try




            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   ITX
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try

                IK3.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                IK3.Columns.Add("DOCUMENT_ID", GetType(Integer))
                IK3.Columns.Add("FILE_ID", GetType(Integer))
                IK3.Columns.Add("BATCH_ID", GetType(Integer))
                IK3.Columns.Add("ISA_ID", GetType(Integer))
                IK3.Columns.Add("GS_ID", GetType(Integer))
                IK3.Columns.Add("ST_ID", GetType(Integer))
                IK3.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                IK3.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                IK3.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                IK3.Columns.Add("999_AK2_GUID", GetType(Guid))
                IK3.Columns.Add("999_IK3_GUID", GetType(Guid))
                IK3.Columns.Add("999_IK4_GUID", GetType(Guid))
                IK3.Columns.Add("IK301", GetType(String))
                IK3.Columns.Add("IK302", GetType(String))
                IK3.Columns.Add("IK303", GetType(String))
                IK3.Columns.Add("IK304", GetType(String))
                IK3.Columns.Add("ROW_NUMBER", GetType(Integer))
                IK3.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                IK3.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                IK3.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   ITX
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try

                IK4.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                IK4.Columns.Add("DOCUMENT_ID", GetType(Integer))
                IK4.Columns.Add("FILE_ID", GetType(Integer))
                IK4.Columns.Add("BATCH_ID", GetType(Integer))
                IK4.Columns.Add("ISA_ID", GetType(Integer))
                IK4.Columns.Add("GS_ID", GetType(Integer))
                IK4.Columns.Add("ST_ID", GetType(Integer))
                IK4.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                IK4.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                IK4.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                IK4.Columns.Add("999_AK2_GUID", GetType(Guid))
                IK4.Columns.Add("999_IK3_GUID", GetType(Guid))
                IK4.Columns.Add("999_IK4_GUID", GetType(Guid))
                IK4.Columns.Add("IK401", GetType(String))
                IK4.Columns.Add("IK401_1", GetType(String))
                IK4.Columns.Add("IK401_2", GetType(String))
                IK4.Columns.Add("IK401_3", GetType(String))
                IK4.Columns.Add("IK402", GetType(String))
                IK4.Columns.Add("IK403", GetType(String))
                IK4.Columns.Add("IK404", GetType(String))
                IK4.Columns.Add("ROW_NUMBER", GetType(Integer))
                IK4.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                IK4.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                IK4.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   ITX
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try

                IK5.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                IK5.Columns.Add("DOCUMENT_ID", GetType(Integer))
                IK5.Columns.Add("FILE_ID", GetType(Integer))
                IK5.Columns.Add("BATCH_ID", GetType(Integer))
                IK5.Columns.Add("ISA_ID", GetType(Integer))
                IK5.Columns.Add("GS_ID", GetType(Integer))
                IK5.Columns.Add("ST_ID", GetType(Integer))
                IK5.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                IK5.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                IK5.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                IK5.Columns.Add("999_AK2_GUID", GetType(Guid))
                IK5.Columns.Add("999_IK3_GUID", GetType(Guid))
                IK5.Columns.Add("999_IK4_GUID", GetType(Guid))
                IK5.Columns.Add("IK501", GetType(String))
                IK5.Columns.Add("IK502", GetType(String))
                IK5.Columns.Add("IK503", GetType(String))
                IK5.Columns.Add("IK504", GetType(String))
                IK5.Columns.Add("IK506", GetType(String))
                IK5.Columns.Add("ROW_NUMBER", GetType(Integer))
                IK5.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                IK5.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                IK5.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try








        End Sub




    End Class

End Namespace

