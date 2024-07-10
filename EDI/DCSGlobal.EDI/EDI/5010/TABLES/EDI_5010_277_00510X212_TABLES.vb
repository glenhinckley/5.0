Option Explicit On
Option Strict On
Option Compare Binary





Imports System.Data


Namespace DCSGlobal.EDI


    Public Class EDI_5010_277_00510X212_TABLES


        Inherits EDI_5010_COMMON_TABLES


        '******************************************************************************************************************
        '  all vars not declared here are in   EDI_5010_COMMON_DECS   it is inhreted from the tables for this type
        '  if you are going to use it on this class only it goes below if not put it in EDI_5010_COMMON_DECS
        '******************************************************************************************************************

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   D
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Public DMG As New DataTable
        Public DTP As New DataTable
        Public DMG As New DataTable
        Public CLS_DTP As New DataTable

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   H
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public HL As New DataTable
        Public CACHE_HL As New DataTable
        Public HL_19 As New DataTable




        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   N
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public NM1 As New DataTable
        Public CACHE_NM1 As New DataTable
        Public NM1_19 As New DataTable



        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   P
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public PER As New DataTable

        Public CACHE_PER As New DataTable



        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   R
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public REF As New DataTable

        Public CLS_REF As New DataTable


        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   S
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public STC As New DataTable
        Public SVC As New DataTable

        Public CLS_STC As New DataTable
        Public STC_19 As New DataTable
        Public CACHE_STC As New DataTable


        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   T
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public TRN As New DataTable

        Public CLS_TRN As New DataTable
        Public TRN_19 As New DataTable
        Public CACHE_TRN As New DataTable



        'end 277 tables


        Public Sub BuildTables()

            BuildCommonTables()




            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   DMG
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try

                DMG.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                DMG.Columns.Add("DOCUMENT_ID", GetType(Integer))
                DMG.Columns.Add("FILE_ID", GetType(Integer))
                DMG.Columns.Add("BATCH_ID", GetType(Integer))
                DMG.Columns.Add("ISA_ID", GetType(Integer))
                DMG.Columns.Add("GS_ID", GetType(Integer))
                DMG.Columns.Add("ST_ID", GetType(Integer))
                DMG.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                DMG.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                DMG.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                DMG.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                DMG.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                DMG.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                DMG.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                DMG.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                DMG.Columns.Add("277_ISL_GUID", GetType(Guid))
                DMG.Columns.Add("277_IRL_GUID", GetType(Guid))
                DMG.Columns.Add("277_SPL_GUID", GetType(Guid))
                DMG.Columns.Add("277_CLS_GUID", GetType(Guid))
                DMG.Columns.Add("277_SLS_GUID", GetType(Guid))
                DMG.Columns.Add("HL01", GetType(Integer))
                DMG.Columns.Add("HL02", GetType(Integer))
                DMG.Columns.Add("HL03", GetType(Integer))
                DMG.Columns.Add("HL04", GetType(Integer))
                DMG.Columns.Add("DMG01", GetType(String))
                DMG.Columns.Add("DMG02", GetType(String))
                DMG.Columns.Add("DMG03", GetType(String))
                DMG.Columns.Add("ROW_NUMBER", GetType(Integer))
                DMG.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                DMG.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                DMG.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   DTP
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                DTP.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                DTP.Columns.Add("DOCUMENT_ID", GetType(Integer))
                DTP.Columns.Add("FILE_ID", GetType(Integer))
                DTP.Columns.Add("BATCH_ID", GetType(Integer))
                DTP.Columns.Add("ISA_ID", GetType(Integer))
                DTP.Columns.Add("GS_ID", GetType(Integer))
                DTP.Columns.Add("ST_ID", GetType(Integer))
                DTP.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                DTP.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                DTP.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                DTP.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                DTP.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                DTP.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                DTP.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                DTP.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                DTP.Columns.Add("277_ISL_GUID", GetType(Guid))
                DTP.Columns.Add("277_IRL_GUID", GetType(Guid))
                DTP.Columns.Add("277_SPL_GUID", GetType(Guid))
                DTP.Columns.Add("277_CLS_GUID", GetType(Guid))
                DTP.Columns.Add("277_SLS_GUID", GetType(Guid))
                DTP.Columns.Add("HL01", GetType(Integer))
                DTP.Columns.Add("HL02", GetType(Integer))
                DTP.Columns.Add("HL03", GetType(Integer))
                DTP.Columns.Add("HL04", GetType(Integer))
                DTP.Columns.Add("DTP01", GetType(String))
                DTP.Columns.Add("DTP02", GetType(String))
                DTP.Columns.Add("DTP03", GetType(String))
                DTP.Columns.Add("ROW_NUMBER", GetType(Integer))
                DTP.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                DTP.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                DTP.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try



            Try
                CLS_DTP.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CLS_DTP.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CLS_DTP.Columns.Add("FILE_ID", GetType(Integer))
                CLS_DTP.Columns.Add("BATCH_ID", GetType(Integer))
                CLS_DTP.Columns.Add("ISA_ID", GetType(Integer))
                CLS_DTP.Columns.Add("GS_ID", GetType(Integer))
                CLS_DTP.Columns.Add("ST_ID", GetType(Integer))
                CLS_DTP.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CLS_DTP.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CLS_DTP.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CLS_DTP.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CLS_DTP.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CLS_DTP.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CLS_DTP.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CLS_DTP.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CLS_DTP.Columns.Add("277_ISL_GUID", GetType(Guid))
                CLS_DTP.Columns.Add("277_IRL_GUID", GetType(Guid))
                CLS_DTP.Columns.Add("277_SPL_GUID", GetType(Guid))
                CLS_DTP.Columns.Add("277_CLS_GUID", GetType(Guid))
                CLS_DTP.Columns.Add("277_SLS_GUID", GetType(Guid))
                CLS_DTP.Columns.Add("HL01", GetType(Integer))
                CLS_DTP.Columns.Add("HL02", GetType(Integer))
                CLS_DTP.Columns.Add("HL03", GetType(Integer))
                CLS_DTP.Columns.Add("HL04", GetType(Integer))
                CLS_DTP.Columns.Add("DTP01", GetType(String))
                CLS_DTP.Columns.Add("DTP02", GetType(String))
                CLS_DTP.Columns.Add("DTP03", GetType(String))
                CLS_DTP.Columns.Add("ROW_NUMBER", GetType(Integer))
                CLS_DTP.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CLS_DTP.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CLS_DTP.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   HL
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                HL.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                HL.Columns.Add("DOCUMENT_ID", GetType(Integer))
                HL.Columns.Add("FILE_ID", GetType(Integer))
                HL.Columns.Add("BATCH_ID", GetType(Integer))
                HL.Columns.Add("ISA_ID", GetType(Integer))
                HL.Columns.Add("GS_ID", GetType(Integer))
                HL.Columns.Add("ST_ID", GetType(Integer))
                HL.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                HL.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                HL.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                HL.Columns.Add("HIPAA_HL_19_GUID", GetType(Guid))
                HL.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                HL.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                HL.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                HL.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                HL.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                HL.Columns.Add("HL01", GetType(Integer))
                HL.Columns.Add("HL02", GetType(Integer))
                HL.Columns.Add("HL03", GetType(Integer))
                HL.Columns.Add("HL04", GetType(Integer))
                HL.Columns.Add("ROW_NUMBER", GetType(Integer))
                HL.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                HL.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                HL.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))

            Catch ex As Exception

            End Try



            Try
                CACHE_HL.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_HL.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_HL.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_HL.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_HL.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_HL.Columns.Add("GS_ID", GetType(Integer))
                CACHE_HL.Columns.Add("ST_ID", GetType(Integer))
                CACHE_HL.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_HL.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_HL.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_HL.Columns.Add("HIPAA_HL_19_GUID", GetType(Guid))
                CACHE_HL.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_HL.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_HL.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_HL.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_HL.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_HL.Columns.Add("HL01", GetType(Integer))
                CACHE_HL.Columns.Add("HL02", GetType(Integer))
                CACHE_HL.Columns.Add("HL03", GetType(Integer))
                CACHE_HL.Columns.Add("HL04", GetType(Integer))
                CACHE_HL.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_HL.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_HL.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_HL.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))

            Catch ex As Exception

            End Try


            Try
                HL.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                HL.Columns.Add("DOCUMENT_ID", GetType(Integer))
                HL.Columns.Add("FILE_ID", GetType(Integer))
                HL.Columns.Add("BATCH_ID", GetType(Integer))
                HL.Columns.Add("ISA_ID", GetType(Integer))
                HL.Columns.Add("GS_ID", GetType(Integer))
                HL.Columns.Add("ST_ID", GetType(Integer))
                HL.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                HL.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                HL.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                HL.Columns.Add("HIPAA_HL_19_GUID", GetType(Guid))
                HL.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                HL.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                HL.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                HL.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                HL.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                HL.Columns.Add("HL01", GetType(Integer))
                HL.Columns.Add("HL02", GetType(Integer))
                HL.Columns.Add("HL03", GetType(Integer))
                HL.Columns.Add("HL04", GetType(Integer))
                HL.Columns.Add("ROW_NUMBER", GetType(Integer))
                HL.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                HL.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                HL.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))

            Catch ex As Exception

            End Try

            Try
                HL_19.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                HL_19.Columns.Add("DOCUMENT_ID", GetType(Integer))
                HL_19.Columns.Add("FILE_ID", GetType(Integer))
                HL_19.Columns.Add("BATCH_ID", GetType(Integer))
                HL_19.Columns.Add("ISA_ID", GetType(Integer))
                HL_19.Columns.Add("GS_ID", GetType(Integer))
                HL_19.Columns.Add("ST_ID", GetType(Integer))
                HL_19.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                HL_19.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                HL_19.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                HL_19.Columns.Add("HIPAA_HL_19_GUID", GetType(Guid))
                HL_19.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                HL_19.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                HL_19.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                HL_19.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                HL_19.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                HL_19.Columns.Add("HL01", GetType(Integer))
                HL_19.Columns.Add("HL02", GetType(Integer))
                HL_19.Columns.Add("HL03", GetType(Integer))
                HL_19.Columns.Add("HL04", GetType(Integer))
                HL_19.Columns.Add("ROW_NUMBER", GetType(Integer))
                HL_19.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                HL_19.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                HL_19.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))

            Catch ex As Exception

            End Try



            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   NM1
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                NM1.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                NM1.Columns.Add("DOCUMENT_ID", GetType(Integer))
                NM1.Columns.Add("FILE_ID", GetType(Integer))
                NM1.Columns.Add("BATCH_ID", GetType(Integer))
                NM1.Columns.Add("ISA_ID", GetType(Integer))
                NM1.Columns.Add("GS_ID", GetType(Integer))
                NM1.Columns.Add("ST_ID", GetType(Integer))
                NM1.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                NM1.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                NM1.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                NM1.Columns.Add("HIPAA_HL_19_GUID", GetType(Guid))
                NM1.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                NM1.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                NM1.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                NM1.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                NM1.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                NM1.Columns.Add("277_ISL_GUID", GetType(Guid))
                NM1.Columns.Add("277_IRL_GUID", GetType(Guid))
                NM1.Columns.Add("277_SPL_GUID", GetType(Guid))
                NM1.Columns.Add("277_CLS_GUID", GetType(Guid))
                NM1.Columns.Add("277_SLS_GUID", GetType(Guid))
                NM1.Columns.Add("HL01", GetType(Integer))
                NM1.Columns.Add("HL02", GetType(Integer))
                NM1.Columns.Add("HL03", GetType(Integer))
                NM1.Columns.Add("HL04", GetType(Integer))
                NM1.Columns.Add("NM101", GetType(String))
                NM1.Columns.Add("NM102", GetType(String))
                NM1.Columns.Add("NM103", GetType(String))
                NM1.Columns.Add("NM104", GetType(String))
                NM1.Columns.Add("NM105", GetType(String))
                NM1.Columns.Add("NM106", GetType(String))
                NM1.Columns.Add("NM107", GetType(String))
                NM1.Columns.Add("NM108", GetType(String))
                NM1.Columns.Add("NM109", GetType(String))
                NM1.Columns.Add("NM110", GetType(String))
                NM1.Columns.Add("NM111", GetType(String))
                NM1.Columns.Add("ROW_NUMBER", GetType(Integer))
                NM1.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                NM1.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                NM1.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))

            Catch ex As Exception

            End Try


            Try
                CACHE_NM1.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_NM1.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_NM1.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_NM1.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_NM1.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_NM1.Columns.Add("GS_ID", GetType(Integer))
                CACHE_NM1.Columns.Add("ST_ID", GetType(Integer))
                CACHE_NM1.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_NM1.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_NM1.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_NM1.Columns.Add("HIPAA_HL_19_GUID", GetType(Guid))
                CACHE_NM1.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_NM1.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_NM1.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_NM1.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_NM1.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_NM1.Columns.Add("277_ISL_GUID", GetType(Guid))
                CACHE_NM1.Columns.Add("277_IRL_GUID", GetType(Guid))
                CACHE_NM1.Columns.Add("277_SPL_GUID", GetType(Guid))
                CACHE_NM1.Columns.Add("277_CLS_GUID", GetType(Guid))
                CACHE_NM1.Columns.Add("277_SLS_GUID", GetType(Guid))
                CACHE_NM1.Columns.Add("HL01", GetType(Integer))
                CACHE_NM1.Columns.Add("HL02", GetType(Integer))
                CACHE_NM1.Columns.Add("HL03", GetType(Integer))
                CACHE_NM1.Columns.Add("HL04", GetType(Integer))
                CACHE_NM1.Columns.Add("NM101", GetType(String))
                CACHE_NM1.Columns.Add("NM102", GetType(String))
                CACHE_NM1.Columns.Add("NM103", GetType(String))
                CACHE_NM1.Columns.Add("NM104", GetType(String))
                CACHE_NM1.Columns.Add("NM105", GetType(String))
                CACHE_NM1.Columns.Add("NM106", GetType(String))
                CACHE_NM1.Columns.Add("NM107", GetType(String))
                CACHE_NM1.Columns.Add("NM108", GetType(String))
                CACHE_NM1.Columns.Add("NM109", GetType(String))
                CACHE_NM1.Columns.Add("NM110", GetType(String))
                CACHE_NM1.Columns.Add("NM111", GetType(String))
                CACHE_NM1.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_NM1.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_NM1.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_NM1.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))

            Catch ex As Exception

            End Try



            Try
                NM1_19.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                NM1_19.Columns.Add("DOCUMENT_ID", GetType(Integer))
                NM1_19.Columns.Add("FILE_ID", GetType(Integer))
                NM1_19.Columns.Add("BATCH_ID", GetType(Integer))
                NM1_19.Columns.Add("ISA_ID", GetType(Integer))
                NM1_19.Columns.Add("GS_ID", GetType(Integer))
                NM1_19.Columns.Add("ST_ID", GetType(Integer))
                NM1_19.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                NM1_19.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                NM1_19.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                NM1_19.Columns.Add("HIPAA_HL_19_GUID", GetType(Guid))
                NM1_19.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                NM1_19.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                NM1_19.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                NM1_19.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                NM1_19.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                NM1_19.Columns.Add("277_ISL_GUID", GetType(Guid))
                NM1_19.Columns.Add("277_IRL_GUID", GetType(Guid))
                NM1_19.Columns.Add("277_SPL_GUID", GetType(Guid))
                NM1_19.Columns.Add("277_CLS_GUID", GetType(Guid))
                NM1_19.Columns.Add("277_SLS_GUID", GetType(Guid))
                NM1_19.Columns.Add("HL01", GetType(Integer))
                NM1_19.Columns.Add("HL02", GetType(Integer))
                NM1_19.Columns.Add("HL03", GetType(Integer))
                NM1_19.Columns.Add("HL04", GetType(Integer))
                NM1_19.Columns.Add("NM101", GetType(String))
                NM1_19.Columns.Add("NM102", GetType(String))
                NM1_19.Columns.Add("NM103", GetType(String))
                NM1_19.Columns.Add("NM104", GetType(String))
                NM1_19.Columns.Add("NM105", GetType(String))
                NM1_19.Columns.Add("NM106", GetType(String))
                NM1_19.Columns.Add("NM107", GetType(String))
                NM1_19.Columns.Add("NM108", GetType(String))
                NM1_19.Columns.Add("NM109", GetType(String))
                NM1_19.Columns.Add("NM110", GetType(String))
                NM1_19.Columns.Add("NM111", GetType(String))
                NM1_19.Columns.Add("ROW_NUMBER", GetType(Integer))
                NM1_19.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                NM1_19.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                NM1_19.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))

            Catch ex As Exception

            End Try


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   PER
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                PER.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                PER.Columns.Add("DOCUMENT_ID", GetType(Integer))
                PER.Columns.Add("FILE_ID", GetType(Integer))
                PER.Columns.Add("BATCH_ID", GetType(Integer))
                PER.Columns.Add("ISA_ID", GetType(Integer))
                PER.Columns.Add("GS_ID", GetType(Integer))
                PER.Columns.Add("ST_ID", GetType(Integer))
                PER.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                PER.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                PER.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                PER.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                PER.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                PER.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                PER.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                PER.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                PER.Columns.Add("HL01", GetType(Integer))
                PER.Columns.Add("HL02", GetType(Integer))
                PER.Columns.Add("HL03", GetType(Integer))
                PER.Columns.Add("HL04", GetType(Integer))
                PER.Columns.Add("PER01", GetType(String))
                PER.Columns.Add("PER02", GetType(String))
                PER.Columns.Add("PER03", GetType(String))
                PER.Columns.Add("PER04", GetType(String))
                PER.Columns.Add("PER05", GetType(String))
                PER.Columns.Add("PER06", GetType(String))
                PER.Columns.Add("PER07", GetType(String))
                PER.Columns.Add("PER08", GetType(String))
                PER.Columns.Add("PER09", GetType(String))
                PER.Columns.Add("ROW_NUMBER", GetType(Integer))
                PER.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                PER.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                PER.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try


            Try
                CACHE_PER.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_PER.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_PER.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_PER.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_PER.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_PER.Columns.Add("GS_ID", GetType(Integer))
                CACHE_PER.Columns.Add("ST_ID", GetType(Integer))
                CACHE_PER.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_PER.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_PER.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_PER.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_PER.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_PER.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_PER.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_PER.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_PER.Columns.Add("HL01", GetType(Integer))
                CACHE_PER.Columns.Add("HL02", GetType(Integer))
                CACHE_PER.Columns.Add("HL03", GetType(Integer))
                CACHE_PER.Columns.Add("HL04", GetType(Integer))
                CACHE_PER.Columns.Add("PER01", GetType(String))
                CACHE_PER.Columns.Add("PER02", GetType(String))
                CACHE_PER.Columns.Add("PER03", GetType(String))
                CACHE_PER.Columns.Add("PER04", GetType(String))
                CACHE_PER.Columns.Add("PER05", GetType(String))
                CACHE_PER.Columns.Add("PER06", GetType(String))
                CACHE_PER.Columns.Add("PER07", GetType(String))
                CACHE_PER.Columns.Add("PER08", GetType(String))
                CACHE_PER.Columns.Add("PER09", GetType(String))
                CACHE_PER.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_PER.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_PER.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_PER.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try



            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   REF
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                REF.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                REF.Columns.Add("DOCUMENT_ID", GetType(Integer))
                REF.Columns.Add("FILE_ID", GetType(Integer))
                REF.Columns.Add("BATCH_ID", GetType(Integer))
                REF.Columns.Add("ISA_ID", GetType(Integer))
                REF.Columns.Add("GS_ID", GetType(Integer))
                REF.Columns.Add("ST_ID", GetType(Integer))
                REF.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                REF.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                REF.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                REF.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                REF.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                REF.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                REF.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                REF.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                REF.Columns.Add("277_ISL_GUID", GetType(Guid))
                REF.Columns.Add("277_IRL_GUID", GetType(Guid))
                REF.Columns.Add("277_SPL_GUID", GetType(Guid))
                REF.Columns.Add("277_CLS_GUID", GetType(Guid))
                REF.Columns.Add("277_SLS_GUID", GetType(Guid))
                REF.Columns.Add("HL01", GetType(Integer))
                REF.Columns.Add("HL02", GetType(Integer))
                REF.Columns.Add("HL03", GetType(Integer))
                REF.Columns.Add("HL04", GetType(Integer))
                REF.Columns.Add("REF01", GetType(String))
                REF.Columns.Add("REF02", GetType(String))
                REF.Columns.Add("REF03", GetType(String))
                REF.Columns.Add("ROW_NUMBER", GetType(Integer))
                REF.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                REF.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                REF.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))

            Catch ex As Exception

            End Try


            Try
                CLS_REF.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CLS_REF.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CLS_REF.Columns.Add("FILE_ID", GetType(Integer))
                CLS_REF.Columns.Add("BATCH_ID", GetType(Integer))
                CLS_REF.Columns.Add("ISA_ID", GetType(Integer))
                CLS_REF.Columns.Add("GS_ID", GetType(Integer))
                CLS_REF.Columns.Add("ST_ID", GetType(Integer))
                CLS_REF.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CLS_REF.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CLS_REF.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CLS_REF.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CLS_REF.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CLS_REF.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CLS_REF.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CLS_REF.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CLS_REF.Columns.Add("277_ISL_GUID", GetType(Guid))
                CLS_REF.Columns.Add("277_IRL_GUID", GetType(Guid))
                CLS_REF.Columns.Add("277_SPL_GUID", GetType(Guid))
                CLS_REF.Columns.Add("277_CLS_GUID", GetType(Guid))
                CLS_REF.Columns.Add("277_SLS_GUID", GetType(Guid))
                CLS_REF.Columns.Add("HL01", GetType(Integer))
                CLS_REF.Columns.Add("HL02", GetType(Integer))
                CLS_REF.Columns.Add("HL03", GetType(Integer))
                CLS_REF.Columns.Add("HL04", GetType(Integer))
                CLS_REF.Columns.Add("REF01", GetType(String))
                CLS_REF.Columns.Add("REF02", GetType(String))
                CLS_REF.Columns.Add("REF03", GetType(String))
                CLS_REF.Columns.Add("ROW_NUMBER", GetType(Integer))
                CLS_REF.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CLS_REF.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CLS_REF.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))

            Catch ex As Exception

            End Try


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   STC
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                STC.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                STC.Columns.Add("DOCUMENT_ID", GetType(Integer))
                STC.Columns.Add("FILE_ID", GetType(Integer))
                STC.Columns.Add("BATCH_ID", GetType(Integer))
                STC.Columns.Add("ISA_ID", GetType(Integer))
                STC.Columns.Add("GS_ID", GetType(Integer))
                STC.Columns.Add("ST_ID", GetType(Integer))
                STC.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                STC.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                STC.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                STC.Columns.Add("HIPAA_HL_19_GUID", GetType(Guid))
                STC.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                STC.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                STC.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                STC.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                STC.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                STC.Columns.Add("277_ISL_GUID", GetType(Guid))
                STC.Columns.Add("277_IRL_GUID", GetType(Guid))
                STC.Columns.Add("277_SPL_GUID", GetType(Guid))
                STC.Columns.Add("277_CLS_GUID", GetType(Guid))
                STC.Columns.Add("277_SLS_GUID", GetType(Guid))
                STC.Columns.Add("HL01", GetType(Integer))
                STC.Columns.Add("HL02", GetType(Integer))
                STC.Columns.Add("HL03", GetType(Integer))
                STC.Columns.Add("HL04", GetType(Integer))
                STC.Columns.Add("STC01", GetType(String))
                STC.Columns.Add("STC01_1", GetType(String))
                STC.Columns.Add("STC01_2", GetType(String))
                STC.Columns.Add("STC01_3", GetType(String))
                STC.Columns.Add("STC01_4", GetType(String))
                STC.Columns.Add("STC02", GetType(String))
                STC.Columns.Add("STC03", GetType(String))
                STC.Columns.Add("STC04", GetType(String))
                STC.Columns.Add("STC05", GetType(String))
                STC.Columns.Add("STC06", GetType(String))
                STC.Columns.Add("STC07", GetType(String))
                STC.Columns.Add("STC08", GetType(String))
                STC.Columns.Add("STC09", GetType(String))
                STC.Columns.Add("STC10", GetType(String))
                STC.Columns.Add("STC10_1", GetType(String))
                STC.Columns.Add("STC10_2", GetType(String))
                STC.Columns.Add("STC10_3", GetType(String))
                STC.Columns.Add("STC10_4", GetType(String))
                STC.Columns.Add("STC11", GetType(String))
                STC.Columns.Add("STC11_1", GetType(String))
                STC.Columns.Add("STC11_2", GetType(String))
                STC.Columns.Add("STC11_3", GetType(String))
                STC.Columns.Add("STC11_4", GetType(String))
                STC.Columns.Add("STC12", GetType(String))
                STC.Columns.Add("ROW_NUMBER", GetType(Integer))
                STC.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                STC.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                STC.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try

            Try
                STC_19.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                STC_19.Columns.Add("DOCUMENT_ID", GetType(Integer))
                STC_19.Columns.Add("FILE_ID", GetType(Integer))
                STC_19.Columns.Add("BATCH_ID", GetType(Integer))
                STC_19.Columns.Add("ISA_ID", GetType(Integer))
                STC_19.Columns.Add("GS_ID", GetType(Integer))
                STC_19.Columns.Add("ST_ID", GetType(Integer))
                STC_19.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                STC_19.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                STC_19.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                STC_19.Columns.Add("HIPAA_HL_19_GUID", GetType(Guid))
                STC_19.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                STC_19.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                STC_19.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                STC_19.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                STC_19.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                STC_19.Columns.Add("277_ISL_GUID", GetType(Guid))
                STC_19.Columns.Add("277_IRL_GUID", GetType(Guid))
                STC_19.Columns.Add("277_SPL_GUID", GetType(Guid))
                STC_19.Columns.Add("277_CLS_GUID", GetType(Guid))
                STC_19.Columns.Add("277_SLS_GUID", GetType(Guid))
                STC_19.Columns.Add("HL01", GetType(Integer))
                STC_19.Columns.Add("HL02", GetType(Integer))
                STC_19.Columns.Add("HL03", GetType(Integer))
                STC_19.Columns.Add("HL04", GetType(Integer))
                STC_19.Columns.Add("STC01", GetType(String))
                STC_19.Columns.Add("STC01_1", GetType(String))
                STC_19.Columns.Add("STC01_2", GetType(String))
                STC_19.Columns.Add("STC01_3", GetType(String))
                STC_19.Columns.Add("STC01_4", GetType(String))
                STC_19.Columns.Add("STC02", GetType(String))
                STC_19.Columns.Add("STC03", GetType(String))
                STC_19.Columns.Add("STC04", GetType(String))
                STC_19.Columns.Add("STC05", GetType(String))
                STC_19.Columns.Add("STC06", GetType(String))
                STC_19.Columns.Add("STC07", GetType(String))
                STC_19.Columns.Add("STC08", GetType(String))
                STC_19.Columns.Add("STC09", GetType(String))
                STC_19.Columns.Add("STC10", GetType(String))
                STC_19.Columns.Add("STC10_1", GetType(String))
                STC_19.Columns.Add("STC10_2", GetType(String))
                STC_19.Columns.Add("STC10_3", GetType(String))
                STC_19.Columns.Add("STC10_4", GetType(String))
                STC_19.Columns.Add("STC11", GetType(String))
                STC_19.Columns.Add("STC11_1", GetType(String))
                STC_19.Columns.Add("STC11_2", GetType(String))
                STC_19.Columns.Add("STC11_3", GetType(String))
                STC_19.Columns.Add("STC11_4", GetType(String))
                STC_19.Columns.Add("STC12", GetType(String))
                STC_19.Columns.Add("ROW_NUMBER", GetType(Integer))
                STC_19.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                STC_19.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                STC_19.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try


            Try
                CACHE_STC.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_STC.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_STC.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_STC.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_STC.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_STC.Columns.Add("GS_ID", GetType(Integer))
                CACHE_STC.Columns.Add("ST_ID", GetType(Integer))
                CACHE_STC.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_STC.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_STC.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_STC.Columns.Add("HIPAA_HL_19_GUID", GetType(Guid))
                CACHE_STC.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_STC.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_STC.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_STC.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_STC.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_STC.Columns.Add("HL01", GetType(Integer))
                CACHE_STC.Columns.Add("HL02", GetType(Integer))
                CACHE_STC.Columns.Add("HL03", GetType(Integer))
                CACHE_STC.Columns.Add("HL04", GetType(Integer))
                CACHE_STC.Columns.Add("277_ISL_GUID", GetType(Guid))
                CACHE_STC.Columns.Add("277_IRL_GUID", GetType(Guid))
                CACHE_STC.Columns.Add("277_SPL_GUID", GetType(Guid))
                CACHE_STC.Columns.Add("277_CLS_GUID", GetType(Guid))
                CACHE_STC.Columns.Add("277_SLS_GUID", GetType(Guid))
                CACHE_STC.Columns.Add("STC01", GetType(String))
                CACHE_STC.Columns.Add("STC01_1", GetType(String))
                CACHE_STC.Columns.Add("STC01_2", GetType(String))
                CACHE_STC.Columns.Add("STC01_3", GetType(String))
                CACHE_STC.Columns.Add("STC01_4", GetType(String))
                CACHE_STC.Columns.Add("STC02", GetType(String))
                CACHE_STC.Columns.Add("STC03", GetType(String))
                CACHE_STC.Columns.Add("STC04", GetType(String))
                CACHE_STC.Columns.Add("STC05", GetType(String))
                CACHE_STC.Columns.Add("STC06", GetType(String))
                CACHE_STC.Columns.Add("STC07", GetType(String))
                CACHE_STC.Columns.Add("STC08", GetType(String))
                CACHE_STC.Columns.Add("STC09", GetType(String))
                CACHE_STC.Columns.Add("STC10", GetType(String))
                CACHE_STC.Columns.Add("STC10_1", GetType(String))
                CACHE_STC.Columns.Add("STC10_2", GetType(String))
                CACHE_STC.Columns.Add("STC10_3", GetType(String))
                CACHE_STC.Columns.Add("STC10_4", GetType(String))
                CACHE_STC.Columns.Add("STC11", GetType(String))
                CACHE_STC.Columns.Add("STC11_1", GetType(String))
                CACHE_STC.Columns.Add("STC11_2", GetType(String))
                CACHE_STC.Columns.Add("STC11_3", GetType(String))
                CACHE_STC.Columns.Add("STC11_4", GetType(String))
                CACHE_STC.Columns.Add("STC12", GetType(String))
                CACHE_STC.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_STC.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_STC.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_STC.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                CLS_STC.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CLS_STC.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CLS_STC.Columns.Add("FILE_ID", GetType(Integer))
                CLS_STC.Columns.Add("BATCH_ID", GetType(Integer))
                CLS_STC.Columns.Add("ISA_ID", GetType(Integer))
                CLS_STC.Columns.Add("GS_ID", GetType(Integer))
                CLS_STC.Columns.Add("ST_ID", GetType(Integer))
                CLS_STC.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CLS_STC.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CLS_STC.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CLS_STC.Columns.Add("HIPAA_HL_19_GUID", GetType(Guid))
                CLS_STC.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CLS_STC.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CLS_STC.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CLS_STC.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CLS_STC.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CLS_STC.Columns.Add("277_ISL_GUID", GetType(Guid))
                CLS_STC.Columns.Add("277_IRL_GUID", GetType(Guid))
                CLS_STC.Columns.Add("277_SPL_GUID", GetType(Guid))
                CLS_STC.Columns.Add("277_CLS_GUID", GetType(Guid))
                CLS_STC.Columns.Add("277_SLS_GUID", GetType(Guid))
                CLS_STC.Columns.Add("HL01", GetType(Integer))
                CLS_STC.Columns.Add("HL02", GetType(Integer))
                CLS_STC.Columns.Add("HL03", GetType(Integer))
                CLS_STC.Columns.Add("HL04", GetType(Integer))
                CLS_STC.Columns.Add("STC01", GetType(String))
                CLS_STC.Columns.Add("STC01_1", GetType(String))
                CLS_STC.Columns.Add("STC01_2", GetType(String))
                CLS_STC.Columns.Add("STC01_3", GetType(String))
                CLS_STC.Columns.Add("STC01_4", GetType(String))
                CLS_STC.Columns.Add("STC02", GetType(String))
                CLS_STC.Columns.Add("STC03", GetType(String))
                CLS_STC.Columns.Add("STC04", GetType(String))
                CLS_STC.Columns.Add("STC05", GetType(String))
                CLS_STC.Columns.Add("STC06", GetType(String))
                CLS_STC.Columns.Add("STC07", GetType(String))
                CLS_STC.Columns.Add("STC08", GetType(String))
                CLS_STC.Columns.Add("STC09", GetType(String))
                CLS_STC.Columns.Add("STC10", GetType(String))
                CLS_STC.Columns.Add("STC10_1", GetType(String))
                CLS_STC.Columns.Add("STC10_2", GetType(String))
                CLS_STC.Columns.Add("STC10_3", GetType(String))
                CLS_STC.Columns.Add("STC10_4", GetType(String))
                CLS_STC.Columns.Add("STC11", GetType(String))
                CLS_STC.Columns.Add("STC11_1", GetType(String))
                CLS_STC.Columns.Add("STC11_2", GetType(String))
                CLS_STC.Columns.Add("STC11_3", GetType(String))
                CLS_STC.Columns.Add("STC11_4", GetType(String))
                CLS_STC.Columns.Add("STC12", GetType(String))
                CLS_STC.Columns.Add("ROW_NUMBER", GetType(Integer))
                CLS_STC.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CLS_STC.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CLS_STC.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try



            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   SVC
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                SVC.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                SVC.Columns.Add("DOCUMENT_ID", GetType(Integer))
                SVC.Columns.Add("FILE_ID", GetType(Integer))
                SVC.Columns.Add("BATCH_ID", GetType(Integer))
                SVC.Columns.Add("ISA_ID", GetType(Integer))
                SVC.Columns.Add("GS_ID", GetType(Integer))
                SVC.Columns.Add("ST_ID", GetType(Integer))
                SVC.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                SVC.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                SVC.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                SVC.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                SVC.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                SVC.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                SVC.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                SVC.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                SVC.Columns.Add("277_ISL_GUID", GetType(Guid))
                SVC.Columns.Add("277_IRL_GUID", GetType(Guid))
                SVC.Columns.Add("277_SPL_GUID", GetType(Guid))
                SVC.Columns.Add("277_CLS_GUID", GetType(Guid))
                SVC.Columns.Add("277_SLS_GUID", GetType(Guid))
                SVC.Columns.Add("HL01", GetType(Integer))
                SVC.Columns.Add("HL02", GetType(Integer))
                SVC.Columns.Add("HL03", GetType(Integer))
                SVC.Columns.Add("HL04", GetType(Integer))
                SVC.Columns.Add("SVC01", GetType(String))
                SVC.Columns.Add("SVC01_1", GetType(String))
                SVC.Columns.Add("SVC01_2", GetType(String))
                SVC.Columns.Add("SVC01_3", GetType(String))
                SVC.Columns.Add("SVC01_4", GetType(String))
                SVC.Columns.Add("SVC01_5", GetType(String))
                SVC.Columns.Add("SVC01_6", GetType(String))
                SVC.Columns.Add("SVC01_7", GetType(String))
                SVC.Columns.Add("SVC02", GetType(String))
                SVC.Columns.Add("SVC03", GetType(String))
                SVC.Columns.Add("SVC04", GetType(String))
                SVC.Columns.Add("SVC05", GetType(String))
                SVC.Columns.Add("SVC06", GetType(String))
                SVC.Columns.Add("SVC06_1", GetType(String))
                SVC.Columns.Add("SVC06_2", GetType(String))
                SVC.Columns.Add("SVC06_3", GetType(String))
                SVC.Columns.Add("SVC06_4", GetType(String))
                SVC.Columns.Add("SVC06_5", GetType(String))
                SVC.Columns.Add("SVC06_6", GetType(String))
                SVC.Columns.Add("SVC06_7", GetType(String))
                SVC.Columns.Add("SVC07", GetType(String))
                SVC.Columns.Add("ROW_NUMBER", GetType(Integer))
                SVC.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                SVC.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                SVC.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try






            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   TRN
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                TRN.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                TRN.Columns.Add("DOCUMENT_ID", GetType(Integer))
                TRN.Columns.Add("FILE_ID", GetType(Integer))
                TRN.Columns.Add("BATCH_ID", GetType(Integer))
                TRN.Columns.Add("ISA_ID", GetType(Integer))
                TRN.Columns.Add("GS_ID", GetType(Integer))
                TRN.Columns.Add("ST_ID", GetType(Integer))
                TRN.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                TRN.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                TRN.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                TRN.Columns.Add("HIPAA_HL_19_GUID", GetType(Guid))
                TRN.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                TRN.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                TRN.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                TRN.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                TRN.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                TRN.Columns.Add("277_ISL_GUID", GetType(Guid))
                TRN.Columns.Add("277_IRL_GUID", GetType(Guid))
                TRN.Columns.Add("277_SPL_GUID", GetType(Guid))
                TRN.Columns.Add("277_CLS_GUID", GetType(Guid))
                TRN.Columns.Add("277_SLS_GUID", GetType(Guid))
                TRN.Columns.Add("HL01", GetType(Integer))
                TRN.Columns.Add("HL02", GetType(Integer))
                TRN.Columns.Add("HL03", GetType(Integer))
                TRN.Columns.Add("HL04", GetType(Integer))
                TRN.Columns.Add("TRN01", GetType(String))
                TRN.Columns.Add("TRN02", GetType(String))
                TRN.Columns.Add("TRN03", GetType(String))
                TRN.Columns.Add("TRN04", GetType(String))
                TRN.Columns.Add("ROW_NUMBER", GetType(Integer))
                TRN.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                TRN.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                TRN.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try

            '         [277_ISL_GUID]
            '         [277_IRL_GUID]
            '         [277_SPL_GUID]
            '         [277_CLS_GUID]
            '         [277_SLS_GUID]



            Try
                CACHE_TRN.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_TRN.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_TRN.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_TRN.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_TRN.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_TRN.Columns.Add("GS_ID", GetType(Integer))
                CACHE_TRN.Columns.Add("ST_ID", GetType(Integer))
                CACHE_TRN.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_TRN.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_TRN.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_TRN.Columns.Add("HIPAA_HL_19_GUID", GetType(Guid))
                CACHE_TRN.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_TRN.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_TRN.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_TRN.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_TRN.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_TRN.Columns.Add("277_ISL_GUID", GetType(Guid))
                CACHE_TRN.Columns.Add("277_IRL_GUID", GetType(Guid))
                CACHE_TRN.Columns.Add("277_SPL_GUID", GetType(Guid))
                CACHE_TRN.Columns.Add("277_CLS_GUID", GetType(Guid))
                CACHE_TRN.Columns.Add("277_SLS_GUID", GetType(Guid))
                CACHE_TRN.Columns.Add("HL01", GetType(Integer))
                CACHE_TRN.Columns.Add("HL02", GetType(Integer))
                CACHE_TRN.Columns.Add("HL03", GetType(Integer))
                CACHE_TRN.Columns.Add("HL04", GetType(Integer))
                CACHE_TRN.Columns.Add("TRN01", GetType(String))
                CACHE_TRN.Columns.Add("TRN02", GetType(String))
                CACHE_TRN.Columns.Add("TRN03", GetType(String))
                CACHE_TRN.Columns.Add("TRN04", GetType(String))
                CACHE_TRN.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_TRN.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_TRN.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_TRN.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try


            Try
                CLS_TRN.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CLS_TRN.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CLS_TRN.Columns.Add("FILE_ID", GetType(Integer))
                CLS_TRN.Columns.Add("BATCH_ID", GetType(Integer))
                CLS_TRN.Columns.Add("ISA_ID", GetType(Integer))
                CLS_TRN.Columns.Add("GS_ID", GetType(Integer))
                CLS_TRN.Columns.Add("ST_ID", GetType(Integer))
                CLS_TRN.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CLS_TRN.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CLS_TRN.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CLS_TRN.Columns.Add("HIPAA_HL_19_GUID", GetType(Guid))
                CLS_TRN.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CLS_TRN.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CLS_TRN.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CLS_TRN.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CLS_TRN.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CLS_TRN.Columns.Add("277_ISL_GUID", GetType(Guid))
                CLS_TRN.Columns.Add("277_IRL_GUID", GetType(Guid))
                CLS_TRN.Columns.Add("277_SPL_GUID", GetType(Guid))
                CLS_TRN.Columns.Add("277_CLS_GUID", GetType(Guid))
                CLS_TRN.Columns.Add("277_SLS_GUID", GetType(Guid))
                CLS_TRN.Columns.Add("HL01", GetType(Integer))
                CLS_TRN.Columns.Add("HL02", GetType(Integer))
                CLS_TRN.Columns.Add("HL03", GetType(Integer))
                CLS_TRN.Columns.Add("HL04", GetType(Integer))
                CLS_TRN.Columns.Add("TRN01", GetType(String))
                CLS_TRN.Columns.Add("TRN02", GetType(String))
                CLS_TRN.Columns.Add("TRN03", GetType(String))
                CLS_TRN.Columns.Add("TRN04", GetType(String))
                CLS_TRN.Columns.Add("ROW_NUMBER", GetType(Integer))
                CLS_TRN.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CLS_TRN.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CLS_TRN.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try


            Try
                TRN_19.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                TRN_19.Columns.Add("DOCUMENT_ID", GetType(Integer))
                TRN_19.Columns.Add("FILE_ID", GetType(Integer))
                TRN_19.Columns.Add("BATCH_ID", GetType(Integer))
                TRN_19.Columns.Add("ISA_ID", GetType(Integer))
                TRN_19.Columns.Add("GS_ID", GetType(Integer))
                TRN_19.Columns.Add("ST_ID", GetType(Integer))
                TRN_19.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                TRN_19.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                TRN_19.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                TRN_19.Columns.Add("HIPAA_HL_19_GUID", GetType(Guid))
                TRN_19.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                TRN_19.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                TRN_19.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                TRN_19.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                TRN_19.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                TRN_19.Columns.Add("277_ISL_GUID", GetType(Guid))
                TRN_19.Columns.Add("277_IRL_GUID", GetType(Guid))
                TRN_19.Columns.Add("277_SPL_GUID", GetType(Guid))
                TRN_19.Columns.Add("277_CLS_GUID", GetType(Guid))
                TRN_19.Columns.Add("277_SLS_GUID", GetType(Guid))
                TRN_19.Columns.Add("HL01", GetType(Integer))
                TRN_19.Columns.Add("HL02", GetType(Integer))
                TRN_19.Columns.Add("HL03", GetType(Integer))
                TRN_19.Columns.Add("HL04", GetType(Integer))
                TRN_19.Columns.Add("TRN01", GetType(String))
                TRN_19.Columns.Add("TRN02", GetType(String))
                TRN_19.Columns.Add("TRN03", GetType(String))
                TRN_19.Columns.Add("TRN04", GetType(String))
                TRN_19.Columns.Add("ROW_NUMBER", GetType(Integer))
                TRN_19.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                TRN_19.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                TRN_19.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try



        End Sub




    End Class

End Namespace

