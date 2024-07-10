Option Explicit On
Option Strict On
Option Compare Binary





Imports System.Data


Namespace DCSGlobal.EDI


    Public Class EDI_5010_837_TABLES


        Inherits EDI_5010_COMMON_TABLES

        '******************************************************************************************************************
        '  all vars not declared here are in   EDI_5010_COMMON_DECS   it is inhreted from the tables for this type
        '  if you are going to use it on this class only it goes below if not put it in EDI_5010_COMMON_DECS
        '******************************************************************************************************************

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   A
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public AMT As New DataTable

        Public CACHE_AMT As New DataTable

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   C
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public CAS As New DataTable
        Public CL1 As New DataTable
        Public CLM As New DataTable
        Public CN1 As New DataTable
        Public CLP As New DataTable
        Public CR1 As New DataTable
        'Public CR2 As New DataTable
        Public CRC As New DataTable
        Public CTP As New DataTable
        Public CUR As New DataTable

        Public CACHE_CAS As New DataTable
        Public CACHE_CL1 As New DataTable
        Public CACHE_CLM As New DataTable
        Public CACHE_CN1 As New DataTable
        Public CACHE_CLP As New DataTable
        Public CACHE_CR1 As New DataTable
        'Public CR2 As New DataTable
        ' Public CRC As New DataTable
        Public CACHE_CTP As New DataTable
        Public CACHE_CUR As New DataTable


        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   D
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public DMG As New DataTable
        Public DTP As New DataTable

        Public CACHE_DMG As New DataTable
        Public CACHE_DTP As New DataTable


        Public CACHE_2000_DMG As New DataTable

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   H
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public HI As New DataTable
        Public HL As New DataTable

        Public CACHE_HI As New DataTable
        Public CACHE_HL As New DataTable

        Public CACHE_2000_HL As New DataTable

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   K
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public K3 As New DataTable

        Public CACHE_K3 As New DataTable


        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   L
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public LX As New DataTable
        Public LIN As New DataTable

        Public CACHE_LX As New DataTable


        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   M
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public MOA As New DataTable
        Public MIA As New DataTable

        Public CACHE_MOA As New DataTable
        Public CACHE_MIA As New DataTable


        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   N
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public N1 As New DataTable
        Public N3 As New DataTable
        Public N4 As New DataTable
        Public NM1 As New DataTable
        Public NTE As New DataTable

        Public CACHE_N1 As New DataTable
        Public CACHE_N3 As New DataTable
        Public CACHE_N4 As New DataTable
        Public CACHE_NM1 As New DataTable
        Public CACHE_NTE As New DataTable

        Public CACHE_1000_NM1 As New DataTable

        Public CACHE_2000_N1 As New DataTable
        Public CACHE_2000_N3 As New DataTable
        Public CACHE_2000_N4 As New DataTable
        Public CACHE_2000_NM1 As New DataTable


        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   O
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public OI As New DataTable

        Public CACHE_OI As New DataTable

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   P
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public PRV As New DataTable
        Public PAT As New DataTable
        Public PER As New DataTable
        Public PSV As New DataTable
        Public PS1 As New DataTable
        Public PWK As New DataTable

        Public CACHE_PRV As New DataTable
        Public CACHE_PAT As New DataTable
        Public CACHE_PER As New DataTable
        Public CACHE_PSV As New DataTable
        Public CACHE_PS1 As New DataTable
        Public CACHE_PWK As New DataTable


        Public CACHE_1000_PER As New DataTable

        Public CACHE_2000_PAT As New DataTable
        Public CACHE_2000_PRV As New DataTable
        Public CACHE_2000_PER As New DataTable

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   R
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public REF As New DataTable

        Public CACHE_2000_REF As New DataTable

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   S
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public SBR As New DataTable
        Public SV1 As New DataTable
        Public SV2 As New DataTable
        Public SV5 As New DataTable
        Public SVD As New DataTable


        Public CACHE_SBR As New DataTable
        Public CACHE_SV1 As New DataTable
        Public CACHE_SV2 As New DataTable
        Public CACHE_SV5 As New DataTable
        Public CACHE_SVD As New DataTable


        Public CACHE_2000_SBR As New DataTable


        'end 837 tables


        Public Sub BuildTables()

            BuildCommonTables()


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   AMT
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                AMT.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                AMT.Columns.Add("DOCUMENT_ID", GetType(Integer))
                AMT.Columns.Add("FILE_ID", GetType(Integer))
                AMT.Columns.Add("BATCH_ID", GetType(Integer))
                AMT.Columns.Add("ISA_ID", GetType(Integer))
                AMT.Columns.Add("GS_ID", GetType(Integer))
                AMT.Columns.Add("ST_ID", GetType(Integer))
                AMT.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                AMT.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                AMT.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                AMT.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                AMT.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                AMT.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                AMT.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                AMT.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                AMT.Columns.Add("837_SBR_GUID", GetType(Guid))
                AMT.Columns.Add("837_CLM_GUID", GetType(Guid))
                AMT.Columns.Add("837_LX_GUID", GetType(Guid))
                AMT.Columns.Add("837_SVD_GUID", GetType(Guid))
                AMT.Columns.Add("HL01", GetType(Integer))
                AMT.Columns.Add("HL02", GetType(Integer))
                AMT.Columns.Add("HL03", GetType(Integer))
                AMT.Columns.Add("HL04", GetType(Integer))
                AMT.Columns.Add("AMT01", GetType(String))
                AMT.Columns.Add("AMT02", GetType(String))
                AMT.Columns.Add("AMT03", GetType(String))
                AMT.Columns.Add("ROW_NUMBER", GetType(Integer))
                AMT.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                AMT.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                AMT.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try


            Try
                CACHE_AMT.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_AMT.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_AMT.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_AMT.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_AMT.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_AMT.Columns.Add("GS_ID", GetType(Integer))
                CACHE_AMT.Columns.Add("ST_ID", GetType(Integer))
                CACHE_AMT.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_AMT.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_AMT.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_AMT.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_AMT.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_AMT.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_AMT.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_AMT.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_AMT.Columns.Add("837_SBR_GUID", GetType(Guid))
                CACHE_AMT.Columns.Add("837_CLM_GUID", GetType(Guid))
                CACHE_AMT.Columns.Add("837_LX_GUID", GetType(Guid))
                CACHE_AMT.Columns.Add("HL01", GetType(Integer))
                CACHE_AMT.Columns.Add("HL02", GetType(Integer))
                CACHE_AMT.Columns.Add("HL03", GetType(Integer))
                CACHE_AMT.Columns.Add("HL04", GetType(Integer))
                CACHE_AMT.Columns.Add("AMT01", GetType(String))
                CACHE_AMT.Columns.Add("AMT02", GetType(String))
                CACHE_AMT.Columns.Add("AMT03", GetType(String))
                CACHE_AMT.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_AMT.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_AMT.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_AMT.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   CAS
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try


                CAS.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CAS.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CAS.Columns.Add("FILE_ID", GetType(Integer))
                CAS.Columns.Add("BATCH_ID", GetType(Integer))
                CAS.Columns.Add("ISA_ID", GetType(Integer))
                CAS.Columns.Add("GS_ID", GetType(Integer))
                CAS.Columns.Add("ST_ID", GetType(Integer))
                CAS.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CAS.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CAS.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CAS.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CAS.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CAS.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CAS.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CAS.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CAS.Columns.Add("837_SBR_GUID", GetType(Guid))
                CAS.Columns.Add("837_CLM_GUID", GetType(Guid))
                CAS.Columns.Add("837_LX_GUID", GetType(Guid))
                CAS.Columns.Add("837_SVD_GUID", GetType(Guid))
                CAS.Columns.Add("HL01", GetType(Integer))
                CAS.Columns.Add("HL02", GetType(Integer))
                CAS.Columns.Add("HL03", GetType(Integer))
                CAS.Columns.Add("HL04", GetType(Integer))
                CAS.Columns.Add("CAS01", GetType(String))
                CAS.Columns.Add("CAS02", GetType(String))
                CAS.Columns.Add("CAS03", GetType(String))
                CAS.Columns.Add("CAS04", GetType(String))
                CAS.Columns.Add("CAS05", GetType(String))
                CAS.Columns.Add("CAS06", GetType(String))
                CAS.Columns.Add("CAS07", GetType(String))
                CAS.Columns.Add("CAS08", GetType(String))
                CAS.Columns.Add("CAS09", GetType(String))
                CAS.Columns.Add("CAS10", GetType(String))
                CAS.Columns.Add("CAS11", GetType(String))
                CAS.Columns.Add("CAS12", GetType(String))
                CAS.Columns.Add("CAS13", GetType(String))
                CAS.Columns.Add("CAS14", GetType(String))
                CAS.Columns.Add("CAS15", GetType(String))
                CAS.Columns.Add("CAS16", GetType(String))
                CAS.Columns.Add("CAS17", GetType(String))
                CAS.Columns.Add("CAS18", GetType(String))
                CAS.Columns.Add("CAS19", GetType(String))
                CAS.Columns.Add("ROW_NUMBER", GetType(Integer))
                CAS.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CAS.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CAS.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try

            Try


                CACHE_CAS.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_CAS.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_CAS.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_CAS.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_CAS.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_CAS.Columns.Add("GS_ID", GetType(Integer))
                CACHE_CAS.Columns.Add("ST_ID", GetType(Integer))
                CACHE_CAS.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_CAS.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_CAS.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_CAS.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_CAS.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_CAS.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_CAS.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_CAS.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_CAS.Columns.Add("837_SBR_GUID", GetType(Guid))
                CACHE_CAS.Columns.Add("837_CLM_GUID", GetType(Guid))
                CACHE_CAS.Columns.Add("837_LX_GUID", GetType(Guid))
                CACHE_CAS.Columns.Add("HL01", GetType(Integer))
                CACHE_CAS.Columns.Add("HL02", GetType(Integer))
                CACHE_CAS.Columns.Add("HL03", GetType(Integer))
                CACHE_CAS.Columns.Add("HL04", GetType(Integer))
                CACHE_CAS.Columns.Add("CAS01", GetType(String))
                CACHE_CAS.Columns.Add("CAS02", GetType(String))
                CACHE_CAS.Columns.Add("CAS03", GetType(String))
                CACHE_CAS.Columns.Add("CAS04", GetType(String))
                CACHE_CAS.Columns.Add("CAS05", GetType(String))
                CACHE_CAS.Columns.Add("CAS06", GetType(String))
                CACHE_CAS.Columns.Add("CAS07", GetType(String))
                CACHE_CAS.Columns.Add("CAS08", GetType(String))
                CACHE_CAS.Columns.Add("CAS09", GetType(String))
                CACHE_CAS.Columns.Add("CAS10", GetType(String))
                CACHE_CAS.Columns.Add("CAS11", GetType(String))
                CACHE_CAS.Columns.Add("CAS12", GetType(String))
                CACHE_CAS.Columns.Add("CAS13", GetType(String))
                CACHE_CAS.Columns.Add("CAS14", GetType(String))
                CACHE_CAS.Columns.Add("CAS15", GetType(String))
                CACHE_CAS.Columns.Add("CAS16", GetType(String))
                CACHE_CAS.Columns.Add("CAS17", GetType(String))
                CACHE_CAS.Columns.Add("CAS18", GetType(String))
                CACHE_CAS.Columns.Add("CAS19", GetType(String))
                CACHE_CAS.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_CAS.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                CACHE_CAS.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_CAS.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
            Catch ex As Exception

            End Try


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   CL1
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                CL1.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CL1.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CL1.Columns.Add("FILE_ID", GetType(Integer))
                CL1.Columns.Add("BATCH_ID", GetType(Integer))
                CL1.Columns.Add("ISA_ID", GetType(Integer))
                CL1.Columns.Add("GS_ID", GetType(Integer))
                CL1.Columns.Add("ST_ID", GetType(Integer))
                CL1.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CL1.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CL1.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CL1.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CL1.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CL1.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CL1.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CL1.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CL1.Columns.Add("837_SBR_GUID", GetType(Guid))
                CL1.Columns.Add("837_CLM_GUID", GetType(Guid))
                CL1.Columns.Add("837_LX_GUID", GetType(Guid))
                CL1.Columns.Add("HL01", GetType(Integer))
                CL1.Columns.Add("HL02", GetType(Integer))
                CL1.Columns.Add("HL03", GetType(Integer))
                CL1.Columns.Add("HL04", GetType(Integer))
                CL1.Columns.Add("CL101", GetType(String))
                CL1.Columns.Add("CL102", GetType(String))
                CL1.Columns.Add("CL103", GetType(String))
                CL1.Columns.Add("ROW_NUMBER", GetType(Integer))
                CL1.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CL1.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CL1.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try


            Try
                CACHE_CL1.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_CL1.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_CL1.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_CL1.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_CL1.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_CL1.Columns.Add("GS_ID", GetType(Integer))
                CACHE_CL1.Columns.Add("ST_ID", GetType(Integer))
                CACHE_CL1.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_CL1.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_CL1.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_CL1.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_CL1.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_CL1.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_CL1.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_CL1.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_CL1.Columns.Add("837_SBR_GUID", GetType(Guid))
                CACHE_CL1.Columns.Add("837_CLM_GUID", GetType(Guid))
                CACHE_CL1.Columns.Add("837_LX_GUID", GetType(Guid))
                CACHE_CL1.Columns.Add("HL01", GetType(Integer))
                CACHE_CL1.Columns.Add("HL02", GetType(Integer))
                CACHE_CL1.Columns.Add("HL03", GetType(Integer))
                CACHE_CL1.Columns.Add("HL04", GetType(Integer))
                CACHE_CL1.Columns.Add("CL101", GetType(String))
                CACHE_CL1.Columns.Add("CL102", GetType(String))
                CACHE_CL1.Columns.Add("CL103", GetType(String))
                CACHE_CL1.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_CL1.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_CL1.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_CL1.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   CLM
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                CLM.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CLM.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CLM.Columns.Add("FILE_ID", GetType(Integer))
                CLM.Columns.Add("BATCH_ID", GetType(Integer))
                CLM.Columns.Add("ISA_ID", GetType(Integer))
                CLM.Columns.Add("GS_ID", GetType(Integer))
                CLM.Columns.Add("ST_ID", GetType(Integer))
                CLM.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CLM.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CLM.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CLM.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CLM.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CLM.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CLM.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CLM.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CLM.Columns.Add("837_SBR_GUID", GetType(Guid))
                CLM.Columns.Add("837_CLM_GUID", GetType(Guid))
                CLM.Columns.Add("837_LX_GUID", GetType(Guid))
                CLM.Columns.Add("HL01", GetType(Integer))
                CLM.Columns.Add("HL02", GetType(Integer))
                CLM.Columns.Add("HL03", GetType(Integer))
                CLM.Columns.Add("HL04", GetType(Integer))
                CLM.Columns.Add("CLM01", GetType(String))
                CLM.Columns.Add("CLM02", GetType(String))
                CLM.Columns.Add("CLM03", GetType(String))
                CLM.Columns.Add("CLM04", GetType(String))
                CLM.Columns.Add("CLM05", GetType(String))
                CLM.Columns.Add("CLM05_01", GetType(String))
                CLM.Columns.Add("CLM05_02", GetType(String))
                CLM.Columns.Add("CLM05_03", GetType(String))
                CLM.Columns.Add("CLM06", GetType(String))
                CLM.Columns.Add("CLM07", GetType(String))
                CLM.Columns.Add("CLM08", GetType(String))
                CLM.Columns.Add("CLM09", GetType(String))
                CLM.Columns.Add("CLM10", GetType(String))
                CLM.Columns.Add("CLM11", GetType(String))
                CLM.Columns.Add("CLM11_01", GetType(String))
                CLM.Columns.Add("CLM11_02", GetType(String))
                CLM.Columns.Add("CLM11_03", GetType(String))
                CLM.Columns.Add("CLM11_04", GetType(String))
                CLM.Columns.Add("CLM11_05", GetType(String))
                CLM.Columns.Add("CLM12", GetType(String))
                CLM.Columns.Add("CLM13", GetType(String))
                CLM.Columns.Add("CLM14", GetType(String))
                CLM.Columns.Add("CLM15", GetType(String))
                CLM.Columns.Add("CLM16", GetType(String))
                CLM.Columns.Add("CLM17", GetType(String))
                CLM.Columns.Add("CLM18", GetType(String))
                CLM.Columns.Add("CLM19", GetType(String))
                CLM.Columns.Add("CLM20", GetType(String))
                CLM.Columns.Add("ROW_NUMBER", GetType(Integer))
                CLM.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CLM.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CLM.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try

            Try
                CACHE_CLM.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_CLM.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_CLM.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_CLM.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_CLM.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_CLM.Columns.Add("GS_ID", GetType(Integer))
                CACHE_CLM.Columns.Add("ST_ID", GetType(Integer))
                CACHE_CLM.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_CLM.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_CLM.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_CLM.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_CLM.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_CLM.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_CLM.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_CLM.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_CLM.Columns.Add("837_SBR_GUID", GetType(Guid))
                CACHE_CLM.Columns.Add("837_CLM_GUID", GetType(Guid))
                CACHE_CLM.Columns.Add("837_LX_GUID", GetType(Guid))
                CACHE_CLM.Columns.Add("HL01", GetType(Integer))
                CACHE_CLM.Columns.Add("HL02", GetType(Integer))
                CACHE_CLM.Columns.Add("HL03", GetType(Integer))
                CACHE_CLM.Columns.Add("HL04", GetType(Integer))
                CACHE_CLM.Columns.Add("CLM01", GetType(String))
                CACHE_CLM.Columns.Add("CLM02", GetType(String))
                CACHE_CLM.Columns.Add("CLM03", GetType(String))
                CACHE_CLM.Columns.Add("CLM04", GetType(String))
                CACHE_CLM.Columns.Add("CLM05", GetType(String))
                CACHE_CLM.Columns.Add("CLM05_01", GetType(String))
                CACHE_CLM.Columns.Add("CLM05_02", GetType(String))
                CACHE_CLM.Columns.Add("CLM05_03", GetType(String))
                CACHE_CLM.Columns.Add("CLM06", GetType(String))
                CACHE_CLM.Columns.Add("CLM07", GetType(String))
                CACHE_CLM.Columns.Add("CLM08", GetType(String))
                CACHE_CLM.Columns.Add("CLM09", GetType(String))
                CACHE_CLM.Columns.Add("CLM10", GetType(String))
                CACHE_CLM.Columns.Add("CLM11", GetType(String))
                CACHE_CLM.Columns.Add("CLM11_01", GetType(String))
                CACHE_CLM.Columns.Add("CLM11_02", GetType(String))
                CACHE_CLM.Columns.Add("CLM11_03", GetType(String))
                CACHE_CLM.Columns.Add("CLM11_04", GetType(String))
                CACHE_CLM.Columns.Add("CLM11_05", GetType(String))
                CACHE_CLM.Columns.Add("CLM12", GetType(String))
                CACHE_CLM.Columns.Add("CLM13", GetType(String))
                CACHE_CLM.Columns.Add("CLM14", GetType(String))
                CACHE_CLM.Columns.Add("CLM15", GetType(String))
                CACHE_CLM.Columns.Add("CLM16", GetType(String))
                CACHE_CLM.Columns.Add("CLM17", GetType(String))
                CACHE_CLM.Columns.Add("CLM18", GetType(String))
                CACHE_CLM.Columns.Add("CLM19", GetType(String))
                CACHE_CLM.Columns.Add("CLM20", GetType(String))
                CACHE_CLM.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_CLM.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_CLM.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_CLM.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   CR1
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                CR1.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CR1.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CR1.Columns.Add("FILE_ID", GetType(Integer))
                CR1.Columns.Add("BATCH_ID", GetType(Integer))
                CR1.Columns.Add("ISA_ID", GetType(Integer))
                CR1.Columns.Add("GS_ID", GetType(Integer))
                CR1.Columns.Add("ST_ID", GetType(Integer))
                CR1.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CR1.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CR1.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CR1.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CR1.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CR1.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CR1.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CR1.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CR1.Columns.Add("837_SBR_GUID", GetType(Guid))
                CR1.Columns.Add("837_CLM_GUID", GetType(Guid))
                CR1.Columns.Add("837_LX_GUID", GetType(Guid))
                CR1.Columns.Add("HL01", GetType(Integer))
                CR1.Columns.Add("HL02", GetType(Integer))
                CR1.Columns.Add("HL03", GetType(Integer))
                CR1.Columns.Add("HL04", GetType(Integer))
                CR1.Columns.Add("CR101", GetType(String))
                CR1.Columns.Add("CR102", GetType(String))
                CR1.Columns.Add("CR103", GetType(String))
                CR1.Columns.Add("CR104", GetType(String))
                CR1.Columns.Add("CR105", GetType(String))
                CR1.Columns.Add("CR106", GetType(String))
                CR1.Columns.Add("CR107", GetType(String))
                CR1.Columns.Add("CR108", GetType(String))
                CR1.Columns.Add("CR109", GetType(String))
                CR1.Columns.Add("CR110", GetType(String))
                CR1.Columns.Add("ROW_NUMBER", GetType(Integer))
                CR1.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CR1.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CR1.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try



            Try
                CACHE_CR1.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_CR1.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_CR1.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_CR1.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_CR1.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_CR1.Columns.Add("GS_ID", GetType(Integer))
                CACHE_CR1.Columns.Add("ST_ID", GetType(Integer))
                CACHE_CR1.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_CR1.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_CR1.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_CR1.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_CR1.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_CR1.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_CR1.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_CR1.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_CR1.Columns.Add("837_SBR_GUID", GetType(Guid))
                CACHE_CR1.Columns.Add("837_CLM_GUID", GetType(Guid))
                CACHE_CR1.Columns.Add("837_LX_GUID", GetType(Guid))
                CACHE_CR1.Columns.Add("HL01", GetType(Integer))
                CACHE_CR1.Columns.Add("HL02", GetType(Integer))
                CACHE_CR1.Columns.Add("HL03", GetType(Integer))
                CACHE_CR1.Columns.Add("HL04", GetType(Integer))
                CACHE_CR1.Columns.Add("CR101", GetType(String))
                CACHE_CR1.Columns.Add("CR102", GetType(String))
                CACHE_CR1.Columns.Add("CR103", GetType(String))
                CACHE_CR1.Columns.Add("CR104", GetType(String))
                CACHE_CR1.Columns.Add("CR105", GetType(String))
                CACHE_CR1.Columns.Add("CR106", GetType(String))
                CACHE_CR1.Columns.Add("CR107", GetType(String))
                CACHE_CR1.Columns.Add("CR108", GetType(String))
                CACHE_CR1.Columns.Add("CR109", GetType(String))
                CACHE_CR1.Columns.Add("CR110", GetType(String))
                CACHE_CR1.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_CR1.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_CR1.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_CR1.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))

            Catch ex As Exception

            End Try


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   CR1
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                CRC.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CRC.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CRC.Columns.Add("FILE_ID", GetType(Integer))
                CRC.Columns.Add("BATCH_ID", GetType(Integer))
                CRC.Columns.Add("ISA_ID", GetType(Integer))
                CRC.Columns.Add("GS_ID", GetType(Integer))
                CRC.Columns.Add("ST_ID", GetType(Integer))
                CRC.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CRC.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CRC.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CRC.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CRC.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CRC.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CRC.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CRC.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CRC.Columns.Add("837_SBR_GUID", GetType(Guid))
                CRC.Columns.Add("837_CLM_GUID", GetType(Guid))
                CRC.Columns.Add("837_LX_GUID", GetType(Guid))
                CRC.Columns.Add("837_SVD_GUID", GetType(Guid))
                CRC.Columns.Add("HL01", GetType(Integer))
                CRC.Columns.Add("HL02", GetType(Integer))
                CRC.Columns.Add("HL03", GetType(Integer))
                CRC.Columns.Add("HL04", GetType(Integer))
                CRC.Columns.Add("CRC01", GetType(String))
                CRC.Columns.Add("CRC02", GetType(String))
                CRC.Columns.Add("CRC03", GetType(String))
                CRC.Columns.Add("CRC04", GetType(String))
                CRC.Columns.Add("CRC05", GetType(String))
                CRC.Columns.Add("CRC06", GetType(String))
                CRC.Columns.Add("CRC07", GetType(String))
                CRC.Columns.Add("ROW_NUMBER", GetType(Integer))
                CRC.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CRC.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CRC.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))

            Catch ex As Exception

            End Try



            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   CUR
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                CUR.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CUR.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CUR.Columns.Add("FILE_ID", GetType(Integer))
                CUR.Columns.Add("BATCH_ID", GetType(Integer))
                CUR.Columns.Add("ISA_ID", GetType(Integer))
                CUR.Columns.Add("GS_ID", GetType(Integer))
                CUR.Columns.Add("ST_ID", GetType(Integer))
                CUR.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CUR.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CUR.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CUR.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CUR.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CUR.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CUR.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CUR.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CUR.Columns.Add("837_SBR_GUID", GetType(Guid))
                CUR.Columns.Add("837_CLM_GUID", GetType(Guid))
                CUR.Columns.Add("837_LX_GUID", GetType(Guid))
                CUR.Columns.Add("HL01", GetType(Integer))
                CUR.Columns.Add("HL02", GetType(Integer))
                CUR.Columns.Add("HL03", GetType(Integer))
                CUR.Columns.Add("HL04", GetType(Integer))
                CUR.Columns.Add("CUR01", GetType(String))
                CUR.Columns.Add("CUR02", GetType(String))
                CUR.Columns.Add("ROW_NUMBER", GetType(Integer))
                CUR.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CUR.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CUR.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try

            Try
                CACHE_CUR.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_CUR.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_CUR.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_CUR.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_CUR.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_CUR.Columns.Add("GS_ID", GetType(Integer))
                CACHE_CUR.Columns.Add("ST_ID", GetType(Integer))
                CACHE_CUR.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_CUR.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_CUR.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_CUR.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_CUR.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_CUR.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_CUR.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_CUR.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_CUR.Columns.Add("837_SBR_GUID", GetType(Guid))
                CACHE_CUR.Columns.Add("837_CLM_GUID", GetType(Guid))
                CACHE_CUR.Columns.Add("837_LX_GUID", GetType(Guid))
                CACHE_CUR.Columns.Add("HL01", GetType(Integer))
                CACHE_CUR.Columns.Add("HL02", GetType(Integer))
                CACHE_CUR.Columns.Add("HL03", GetType(Integer))
                CACHE_CUR.Columns.Add("HL04", GetType(Integer))
                CACHE_CUR.Columns.Add("CUR01", GetType(String))
                CACHE_CUR.Columns.Add("CUR02", GetType(String))
                CACHE_CUR.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_CUR.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_CUR.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_CUR.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try



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
                DMG.Columns.Add("837_SBR_GUID", GetType(Guid))
                DMG.Columns.Add("837_CLM_GUID", GetType(Guid))
                DMG.Columns.Add("837_LX_GUID", GetType(Guid))
                DMG.Columns.Add("NM1_GUID", GetType(Guid))
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


            Try

                CACHE_2000_DMG.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_2000_DMG.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_2000_DMG.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_2000_DMG.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_2000_DMG.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_2000_DMG.Columns.Add("GS_ID", GetType(Integer))
                CACHE_2000_DMG.Columns.Add("ST_ID", GetType(Integer))
                CACHE_2000_DMG.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_2000_DMG.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_2000_DMG.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_2000_DMG.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_2000_DMG.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_2000_DMG.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_2000_DMG.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_2000_DMG.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_2000_DMG.Columns.Add("837_SBR_GUID", GetType(Guid))
                CACHE_2000_DMG.Columns.Add("837_CLM_GUID", GetType(Guid))
                CACHE_2000_DMG.Columns.Add("837_LX_GUID", GetType(Guid))
                CACHE_2000_DMG.Columns.Add("NM1_GUID", GetType(Guid))
                CACHE_2000_DMG.Columns.Add("HL01", GetType(Integer))
                CACHE_2000_DMG.Columns.Add("HL02", GetType(Integer))
                CACHE_2000_DMG.Columns.Add("HL03", GetType(Integer))
                CACHE_2000_DMG.Columns.Add("HL04", GetType(Integer))
                CACHE_2000_DMG.Columns.Add("DMG01", GetType(String))
                CACHE_2000_DMG.Columns.Add("DMG02", GetType(String))
                CACHE_2000_DMG.Columns.Add("DMG03", GetType(String))
                CACHE_2000_DMG.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_2000_DMG.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_2000_DMG.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_2000_DMG.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
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
                DTP.Columns.Add("837_SBR_GUID", GetType(Guid))
                DTP.Columns.Add("837_CLM_GUID", GetType(Guid))
                DTP.Columns.Add("837_LX_GUID", GetType(Guid))
                DTP.Columns.Add("837_SVD_GUID", GetType(Guid))
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
                CACHE_DTP.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_DTP.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_DTP.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_DTP.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_DTP.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_DTP.Columns.Add("GS_ID", GetType(Integer))
                CACHE_DTP.Columns.Add("ST_ID", GetType(Integer))
                CACHE_DTP.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_DTP.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_DTP.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_DTP.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_DTP.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_DTP.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_DTP.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_DTP.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_DTP.Columns.Add("837_SBR_GUID", GetType(Guid))
                CACHE_DTP.Columns.Add("837_CLM_GUID", GetType(Guid))
                CACHE_DTP.Columns.Add("837_LX_GUID", GetType(Guid))
                CACHE_DTP.Columns.Add("HL01", GetType(Integer))
                CACHE_DTP.Columns.Add("HL02", GetType(Integer))
                CACHE_DTP.Columns.Add("HL03", GetType(Integer))
                CACHE_DTP.Columns.Add("HL04", GetType(Integer))
                CACHE_DTP.Columns.Add("DTP01", GetType(String))
                CACHE_DTP.Columns.Add("DTP02", GetType(String))
                CACHE_DTP.Columns.Add("DTP03", GetType(String))
                CACHE_DTP.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_DTP.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_DTP.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_DTP.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
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
                HL.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                HL.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                HL.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                HL.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                HL.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                HL.Columns.Add("837_SBR_GUID", GetType(Guid))
                HL.Columns.Add("837_CLM_GUID", GetType(Guid))
                HL.Columns.Add("837_LX_GUID", GetType(Guid))
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
                CACHE_2000_HL.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_2000_HL.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_2000_HL.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_2000_HL.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_2000_HL.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_2000_HL.Columns.Add("GS_ID", GetType(Integer))
                CACHE_2000_HL.Columns.Add("ST_ID", GetType(Integer))
                CACHE_2000_HL.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_2000_HL.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_2000_HL.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_2000_HL.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_2000_HL.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_2000_HL.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_2000_HL.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_2000_HL.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_2000_HL.Columns.Add("837_SBR_GUID", GetType(Guid))
                CACHE_2000_HL.Columns.Add("837_CLM_GUID", GetType(Guid))
                CACHE_2000_HL.Columns.Add("837_LX_GUID", GetType(Guid))
                CACHE_2000_HL.Columns.Add("HL01", GetType(Integer))
                CACHE_2000_HL.Columns.Add("HL02", GetType(Integer))
                CACHE_2000_HL.Columns.Add("HL03", GetType(Integer))
                CACHE_2000_HL.Columns.Add("HL04", GetType(Integer))
                CACHE_2000_HL.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_2000_HL.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_2000_HL.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_2000_HL.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))

            Catch ex As Exception

            End Try



            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   HI
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                HI.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                HI.Columns.Add("DOCUMENT_ID", GetType(Integer))
                HI.Columns.Add("FILE_ID", GetType(Integer))
                HI.Columns.Add("BATCH_ID", GetType(Integer))
                HI.Columns.Add("ISA_ID", GetType(Integer))
                HI.Columns.Add("GS_ID", GetType(Integer))
                HI.Columns.Add("ST_ID", GetType(Integer))
                HI.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                HI.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                HI.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                HI.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                HI.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                HI.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                HI.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                HI.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                HI.Columns.Add("837_SBR_GUID", GetType(Guid))
                HI.Columns.Add("837_CLM_GUID", GetType(Guid))
                HI.Columns.Add("837_LX_GUID", GetType(Guid))
                HI.Columns.Add("HL01", GetType(Integer))
                HI.Columns.Add("HL02", GetType(Integer))
                HI.Columns.Add("HL03", GetType(Integer))
                HI.Columns.Add("HL04", GetType(Integer))
                HI.Columns.Add("HI01", GetType(String))
                HI.Columns.Add("HI01_1", GetType(String))
                HI.Columns.Add("HI01_2", GetType(String))
                HI.Columns.Add("HI01_3", GetType(String))
                HI.Columns.Add("HI01_4", GetType(String))
                HI.Columns.Add("HI01_5", GetType(String))
                HI.Columns.Add("HI01_6", GetType(String))
                HI.Columns.Add("HI01_7", GetType(String))
                HI.Columns.Add("HI01_8", GetType(String))
                HI.Columns.Add("HI01_9", GetType(String))
                HI.Columns.Add("HI02", GetType(String))
                HI.Columns.Add("HI02_1", GetType(String))
                HI.Columns.Add("HI02_2", GetType(String))
                HI.Columns.Add("HI02_3", GetType(String))
                HI.Columns.Add("HI02_4", GetType(String))
                HI.Columns.Add("HI02_5", GetType(String))
                HI.Columns.Add("HI02_6", GetType(String))
                HI.Columns.Add("HI02_7", GetType(String))
                HI.Columns.Add("HI02_8", GetType(String))
                HI.Columns.Add("HI02_9", GetType(String))
                HI.Columns.Add("HI03", GetType(String))
                HI.Columns.Add("HI03_1", GetType(String))
                HI.Columns.Add("HI03_2", GetType(String))
                HI.Columns.Add("HI03_3", GetType(String))
                HI.Columns.Add("HI03_4", GetType(String))
                HI.Columns.Add("HI03_5", GetType(String))
                HI.Columns.Add("HI03_6", GetType(String))
                HI.Columns.Add("HI03_7", GetType(String))
                HI.Columns.Add("HI03_8", GetType(String))
                HI.Columns.Add("HI03_9", GetType(String))
                HI.Columns.Add("HI04", GetType(String))
                HI.Columns.Add("HI04_1", GetType(String))
                HI.Columns.Add("HI04_2", GetType(String))
                HI.Columns.Add("HI04_3", GetType(String))
                HI.Columns.Add("HI04_4", GetType(String))
                HI.Columns.Add("HI04_5", GetType(String))
                HI.Columns.Add("HI04_6", GetType(String))
                HI.Columns.Add("HI04_7", GetType(String))
                HI.Columns.Add("HI04_8", GetType(String))
                HI.Columns.Add("HI04_9", GetType(String))
                HI.Columns.Add("HI05", GetType(String))
                HI.Columns.Add("HI05_1", GetType(String))
                HI.Columns.Add("HI05_2", GetType(String))
                HI.Columns.Add("HI05_3", GetType(String))
                HI.Columns.Add("HI05_4", GetType(String))
                HI.Columns.Add("HI05_5", GetType(String))
                HI.Columns.Add("HI05_6", GetType(String))
                HI.Columns.Add("HI05_7", GetType(String))
                HI.Columns.Add("HI05_8", GetType(String))
                HI.Columns.Add("HI05_9", GetType(String))
                HI.Columns.Add("HI06", GetType(String))
                HI.Columns.Add("HI06_1", GetType(String))
                HI.Columns.Add("HI06_2", GetType(String))
                HI.Columns.Add("HI06_3", GetType(String))
                HI.Columns.Add("HI06_4", GetType(String))
                HI.Columns.Add("HI06_5", GetType(String))
                HI.Columns.Add("HI06_6", GetType(String))
                HI.Columns.Add("HI06_7", GetType(String))
                HI.Columns.Add("HI06_8", GetType(String))
                HI.Columns.Add("HI06_9", GetType(String))
                HI.Columns.Add("HI07", GetType(String))
                HI.Columns.Add("HI07_1", GetType(String))
                HI.Columns.Add("HI07_2", GetType(String))
                HI.Columns.Add("HI07_3", GetType(String))
                HI.Columns.Add("HI07_4", GetType(String))
                HI.Columns.Add("HI07_5", GetType(String))
                HI.Columns.Add("HI07_6", GetType(String))
                HI.Columns.Add("HI07_7", GetType(String))
                HI.Columns.Add("HI07_8", GetType(String))
                HI.Columns.Add("HI07_9", GetType(String))
                HI.Columns.Add("HI08", GetType(String))
                HI.Columns.Add("HI08_1", GetType(String))
                HI.Columns.Add("HI08_2", GetType(String))
                HI.Columns.Add("HI08_3", GetType(String))
                HI.Columns.Add("HI08_4", GetType(String))
                HI.Columns.Add("HI08_5", GetType(String))
                HI.Columns.Add("HI08_6", GetType(String))
                HI.Columns.Add("HI08_7", GetType(String))
                HI.Columns.Add("HI08_8", GetType(String))
                HI.Columns.Add("HI08_9", GetType(String))
                HI.Columns.Add("HI09", GetType(String))
                HI.Columns.Add("HI09_1", GetType(String))
                HI.Columns.Add("HI09_2", GetType(String))
                HI.Columns.Add("HI09_3", GetType(String))
                HI.Columns.Add("HI09_4", GetType(String))
                HI.Columns.Add("HI09_5", GetType(String))
                HI.Columns.Add("HI09_6", GetType(String))
                HI.Columns.Add("HI09_7", GetType(String))
                HI.Columns.Add("HI09_8", GetType(String))
                HI.Columns.Add("HI09_9", GetType(String))
                HI.Columns.Add("HI10", GetType(String))
                HI.Columns.Add("HI10_1", GetType(String))
                HI.Columns.Add("HI10_2", GetType(String))
                HI.Columns.Add("HI10_3", GetType(String))
                HI.Columns.Add("HI10_4", GetType(String))
                HI.Columns.Add("HI10_5", GetType(String))
                HI.Columns.Add("HI10_6", GetType(String))
                HI.Columns.Add("HI10_7", GetType(String))
                HI.Columns.Add("HI10_8", GetType(String))
                HI.Columns.Add("HI10_9", GetType(String))
                HI.Columns.Add("HI11", GetType(String))
                HI.Columns.Add("HI11_1", GetType(String))
                HI.Columns.Add("HI11_2", GetType(String))
                HI.Columns.Add("HI11_3", GetType(String))
                HI.Columns.Add("HI11_4", GetType(String))
                HI.Columns.Add("HI11_5", GetType(String))
                HI.Columns.Add("HI11_6", GetType(String))
                HI.Columns.Add("HI11_7", GetType(String))
                HI.Columns.Add("HI11_8", GetType(String))
                HI.Columns.Add("HI11_9", GetType(String))
                HI.Columns.Add("HI12", GetType(String))
                HI.Columns.Add("HI12_1", GetType(String))
                HI.Columns.Add("HI12_2", GetType(String))
                HI.Columns.Add("HI12_3", GetType(String))
                HI.Columns.Add("HI12_4", GetType(String))
                HI.Columns.Add("HI12_5", GetType(String))
                HI.Columns.Add("HI12_6", GetType(String))
                HI.Columns.Add("HI12_7", GetType(String))
                HI.Columns.Add("HI12_8", GetType(String))
                HI.Columns.Add("HI12_9", GetType(String))
                HI.Columns.Add("ROW_NUMBER", GetType(Integer))
                HI.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                HI.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                HI.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))

            Catch ex As Exception

            End Try


            Try
                CACHE_HI.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_HI.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_HI.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_HI.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_HI.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_HI.Columns.Add("GS_ID", GetType(Integer))
                CACHE_HI.Columns.Add("ST_ID", GetType(Integer))
                CACHE_HI.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_HI.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_HI.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_HI.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_HI.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_HI.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_HI.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_HI.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_HI.Columns.Add("837_SBR_GUID", GetType(Guid))
                CACHE_HI.Columns.Add("837_CLM_GUID", GetType(Guid))
                CACHE_HI.Columns.Add("837_LX_GUID", GetType(Guid))
                CACHE_HI.Columns.Add("HL01", GetType(Integer))
                CACHE_HI.Columns.Add("HL02", GetType(Integer))
                CACHE_HI.Columns.Add("HL03", GetType(Integer))
                CACHE_HI.Columns.Add("HL04", GetType(Integer))
                CACHE_HI.Columns.Add("HI01", GetType(String))
                CACHE_HI.Columns.Add("HI01_1", GetType(String))
                CACHE_HI.Columns.Add("HI01_2", GetType(String))
                CACHE_HI.Columns.Add("HI01_3", GetType(String))
                CACHE_HI.Columns.Add("HI01_4", GetType(String))
                CACHE_HI.Columns.Add("HI01_5", GetType(String))
                CACHE_HI.Columns.Add("HI01_6", GetType(String))
                CACHE_HI.Columns.Add("HI01_7", GetType(String))
                CACHE_HI.Columns.Add("HI01_8", GetType(String))
                CACHE_HI.Columns.Add("HI01_9", GetType(String))
                CACHE_HI.Columns.Add("HI02", GetType(String))
                CACHE_HI.Columns.Add("HI02_1", GetType(String))
                CACHE_HI.Columns.Add("HI02_2", GetType(String))
                CACHE_HI.Columns.Add("HI02_3", GetType(String))
                CACHE_HI.Columns.Add("HI02_4", GetType(String))
                CACHE_HI.Columns.Add("HI02_5", GetType(String))
                CACHE_HI.Columns.Add("HI02_6", GetType(String))
                CACHE_HI.Columns.Add("HI02_7", GetType(String))
                CACHE_HI.Columns.Add("HI02_8", GetType(String))
                CACHE_HI.Columns.Add("HI02_9", GetType(String))
                CACHE_HI.Columns.Add("HI03", GetType(String))
                CACHE_HI.Columns.Add("HI03_1", GetType(String))
                CACHE_HI.Columns.Add("HI03_2", GetType(String))
                CACHE_HI.Columns.Add("HI03_3", GetType(String))
                CACHE_HI.Columns.Add("HI03_4", GetType(String))
                CACHE_HI.Columns.Add("HI03_5", GetType(String))
                CACHE_HI.Columns.Add("HI03_6", GetType(String))
                CACHE_HI.Columns.Add("HI03_7", GetType(String))
                CACHE_HI.Columns.Add("HI03_8", GetType(String))
                CACHE_HI.Columns.Add("HI03_9", GetType(String))
                CACHE_HI.Columns.Add("HI04", GetType(String))
                CACHE_HI.Columns.Add("HI04_1", GetType(String))
                CACHE_HI.Columns.Add("HI04_2", GetType(String))
                CACHE_HI.Columns.Add("HI04_3", GetType(String))
                CACHE_HI.Columns.Add("HI04_4", GetType(String))
                CACHE_HI.Columns.Add("HI04_5", GetType(String))
                CACHE_HI.Columns.Add("HI04_6", GetType(String))
                CACHE_HI.Columns.Add("HI04_7", GetType(String))
                CACHE_HI.Columns.Add("HI04_8", GetType(String))
                CACHE_HI.Columns.Add("HI04_9", GetType(String))
                CACHE_HI.Columns.Add("HI05", GetType(String))
                CACHE_HI.Columns.Add("HI05_1", GetType(String))
                CACHE_HI.Columns.Add("HI05_2", GetType(String))
                CACHE_HI.Columns.Add("HI05_3", GetType(String))
                CACHE_HI.Columns.Add("HI05_4", GetType(String))
                CACHE_HI.Columns.Add("HI05_5", GetType(String))
                CACHE_HI.Columns.Add("HI05_6", GetType(String))
                CACHE_HI.Columns.Add("HI05_7", GetType(String))
                CACHE_HI.Columns.Add("HI05_8", GetType(String))
                CACHE_HI.Columns.Add("HI05_9", GetType(String))
                CACHE_HI.Columns.Add("HI06", GetType(String))
                CACHE_HI.Columns.Add("HI06_1", GetType(String))
                CACHE_HI.Columns.Add("HI06_2", GetType(String))
                CACHE_HI.Columns.Add("HI06_3", GetType(String))
                CACHE_HI.Columns.Add("HI06_4", GetType(String))
                CACHE_HI.Columns.Add("HI06_5", GetType(String))
                CACHE_HI.Columns.Add("HI06_6", GetType(String))
                CACHE_HI.Columns.Add("HI06_7", GetType(String))
                CACHE_HI.Columns.Add("HI06_8", GetType(String))
                CACHE_HI.Columns.Add("HI06_9", GetType(String))
                CACHE_HI.Columns.Add("HI07", GetType(String))
                CACHE_HI.Columns.Add("HI07_1", GetType(String))
                CACHE_HI.Columns.Add("HI07_2", GetType(String))
                CACHE_HI.Columns.Add("HI07_3", GetType(String))
                CACHE_HI.Columns.Add("HI07_4", GetType(String))
                CACHE_HI.Columns.Add("HI07_5", GetType(String))
                CACHE_HI.Columns.Add("HI07_6", GetType(String))
                CACHE_HI.Columns.Add("HI07_7", GetType(String))
                CACHE_HI.Columns.Add("HI07_8", GetType(String))
                CACHE_HI.Columns.Add("HI07_9", GetType(String))
                CACHE_HI.Columns.Add("HI08", GetType(String))
                CACHE_HI.Columns.Add("HI08_1", GetType(String))
                CACHE_HI.Columns.Add("HI08_2", GetType(String))
                CACHE_HI.Columns.Add("HI08_3", GetType(String))
                CACHE_HI.Columns.Add("HI08_4", GetType(String))
                CACHE_HI.Columns.Add("HI08_5", GetType(String))
                CACHE_HI.Columns.Add("HI08_6", GetType(String))
                CACHE_HI.Columns.Add("HI08_7", GetType(String))
                CACHE_HI.Columns.Add("HI08_8", GetType(String))
                CACHE_HI.Columns.Add("HI08_9", GetType(String))
                CACHE_HI.Columns.Add("HI09", GetType(String))
                CACHE_HI.Columns.Add("HI09_1", GetType(String))
                CACHE_HI.Columns.Add("HI09_2", GetType(String))
                CACHE_HI.Columns.Add("HI09_3", GetType(String))
                CACHE_HI.Columns.Add("HI09_4", GetType(String))
                CACHE_HI.Columns.Add("HI09_5", GetType(String))
                CACHE_HI.Columns.Add("HI09_6", GetType(String))
                CACHE_HI.Columns.Add("HI09_7", GetType(String))
                CACHE_HI.Columns.Add("HI09_8", GetType(String))
                CACHE_HI.Columns.Add("HI09_9", GetType(String))
                CACHE_HI.Columns.Add("HI10", GetType(String))
                CACHE_HI.Columns.Add("HI10_1", GetType(String))
                CACHE_HI.Columns.Add("HI10_2", GetType(String))
                CACHE_HI.Columns.Add("HI10_3", GetType(String))
                CACHE_HI.Columns.Add("HI10_4", GetType(String))
                CACHE_HI.Columns.Add("HI10_5", GetType(String))
                CACHE_HI.Columns.Add("HI10_6", GetType(String))
                CACHE_HI.Columns.Add("HI10_7", GetType(String))
                CACHE_HI.Columns.Add("HI10_8", GetType(String))
                CACHE_HI.Columns.Add("HI10_9", GetType(String))
                CACHE_HI.Columns.Add("HI11", GetType(String))
                CACHE_HI.Columns.Add("HI11_1", GetType(String))
                CACHE_HI.Columns.Add("HI11_2", GetType(String))
                CACHE_HI.Columns.Add("HI11_3", GetType(String))
                CACHE_HI.Columns.Add("HI11_4", GetType(String))
                CACHE_HI.Columns.Add("HI11_5", GetType(String))
                CACHE_HI.Columns.Add("HI11_6", GetType(String))
                CACHE_HI.Columns.Add("HI11_7", GetType(String))
                CACHE_HI.Columns.Add("HI11_8", GetType(String))
                CACHE_HI.Columns.Add("HI11_9", GetType(String))
                CACHE_HI.Columns.Add("HI12", GetType(String))
                CACHE_HI.Columns.Add("HI12_1", GetType(String))
                CACHE_HI.Columns.Add("HI12_2", GetType(String))
                CACHE_HI.Columns.Add("HI12_3", GetType(String))
                CACHE_HI.Columns.Add("HI12_4", GetType(String))
                CACHE_HI.Columns.Add("HI12_5", GetType(String))
                CACHE_HI.Columns.Add("HI12_6", GetType(String))
                CACHE_HI.Columns.Add("HI12_7", GetType(String))
                CACHE_HI.Columns.Add("HI12_8", GetType(String))
                CACHE_HI.Columns.Add("HI12_9", GetType(String))
                CACHE_HI.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_HI.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_HI.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_HI.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))



            Catch ex As Exception

            End Try


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   K3
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                K3.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                K3.Columns.Add("DOCUMENT_ID", GetType(Integer))
                K3.Columns.Add("FILE_ID", GetType(Integer))
                K3.Columns.Add("BATCH_ID", GetType(Integer))
                K3.Columns.Add("ISA_ID", GetType(Integer))
                K3.Columns.Add("GS_ID", GetType(Integer))
                K3.Columns.Add("ST_ID", GetType(Integer))
                K3.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                K3.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                K3.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                K3.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                K3.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                K3.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                K3.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                K3.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                K3.Columns.Add("837_SBR_GUID", GetType(Guid))
                K3.Columns.Add("837_CLM_GUID", GetType(Guid))
                K3.Columns.Add("837_LX_GUID", GetType(Guid))
                K3.Columns.Add("HL01", GetType(Integer))
                K3.Columns.Add("HL02", GetType(Integer))
                K3.Columns.Add("HL03", GetType(Integer))
                K3.Columns.Add("HL04", GetType(Integer))
                K3.Columns.Add("K301", GetType(String))
                K3.Columns.Add("K302", GetType(String))
                K3.Columns.Add("K303", GetType(String))
                K3.Columns.Add("ROW_NUMBER", GetType(Integer))
                K3.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                K3.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                K3.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try

            Try
                CACHE_K3.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_K3.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_K3.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_K3.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_K3.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_K3.Columns.Add("GS_ID", GetType(Integer))
                CACHE_K3.Columns.Add("ST_ID", GetType(Integer))
                CACHE_K3.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_K3.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_K3.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_K3.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_K3.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_K3.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_K3.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_K3.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_K3.Columns.Add("837_SBR_GUID", GetType(Guid))
                CACHE_K3.Columns.Add("837_CLM_GUID", GetType(Guid))
                CACHE_K3.Columns.Add("837_LX_GUID", GetType(Guid))
                CACHE_K3.Columns.Add("HL01", GetType(Integer))
                CACHE_K3.Columns.Add("HL02", GetType(Integer))
                CACHE_K3.Columns.Add("HL03", GetType(Integer))
                CACHE_K3.Columns.Add("HL04", GetType(Integer))
                CACHE_K3.Columns.Add("K301", GetType(String))
                CACHE_K3.Columns.Add("K302", GetType(String))
                CACHE_K3.Columns.Add("K303", GetType(String))
                CACHE_K3.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_K3.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_K3.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_K3.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try



            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   LX
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try

                LX.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                LX.Columns.Add("DOCUMENT_ID", GetType(Integer))
                LX.Columns.Add("FILE_ID", GetType(Integer))
                LX.Columns.Add("BATCH_ID", GetType(Integer))
                LX.Columns.Add("ISA_ID", GetType(Integer))
                LX.Columns.Add("GS_ID", GetType(Integer))
                LX.Columns.Add("ST_ID", GetType(Integer))
                LX.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                LX.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                LX.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                LX.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                LX.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                LX.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                LX.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                LX.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                LX.Columns.Add("837_SBR_GUID", GetType(Guid))
                LX.Columns.Add("837_CLM_GUID", GetType(Guid))
                LX.Columns.Add("837_LX_GUID", GetType(Guid))
                LX.Columns.Add("HL01", GetType(Integer))
                LX.Columns.Add("HL02", GetType(Integer))
                LX.Columns.Add("HL03", GetType(Integer))
                LX.Columns.Add("HL04", GetType(Integer))
                LX.Columns.Add("LX01", GetType(String))
                LX.Columns.Add("ROW_NUMBER", GetType(Integer))
                LX.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                LX.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                LX.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))

            Catch ex As Exception

            End Try


            Try

                CACHE_LX.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_LX.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_LX.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_LX.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_LX.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_LX.Columns.Add("GS_ID", GetType(Integer))
                CACHE_LX.Columns.Add("ST_ID", GetType(Integer))
                CACHE_LX.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_LX.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_LX.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_LX.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_LX.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_LX.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_LX.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_LX.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_LX.Columns.Add("837_SBR_GUID", GetType(Guid))
                CACHE_LX.Columns.Add("837_CLM_GUID", GetType(Guid))
                CACHE_LX.Columns.Add("837_LX_GUID", GetType(Guid))
                CACHE_LX.Columns.Add("HL01", GetType(Integer))
                CACHE_LX.Columns.Add("HL02", GetType(Integer))
                CACHE_LX.Columns.Add("HL03", GetType(Integer))
                CACHE_LX.Columns.Add("HL04", GetType(Integer))
                CACHE_LX.Columns.Add("LX01", GetType(String))
                CACHE_LX.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_LX.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_LX.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_LX.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))

            Catch ex As Exception

            End Try


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   LIN
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Try
                LIN.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                LIN.Columns.Add("DOCUMENT_ID", GetType(Integer))
                LIN.Columns.Add("FILE_ID", GetType(Integer))
                LIN.Columns.Add("BATCH_ID", GetType(Integer))
                LIN.Columns.Add("ISA_ID", GetType(Integer))
                LIN.Columns.Add("GS_ID", GetType(Integer))
                LIN.Columns.Add("ST_ID", GetType(Integer))
                LIN.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                LIN.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                LIN.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                LIN.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                LIN.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                LIN.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                LIN.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                LIN.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                LIN.Columns.Add("837_SBR_GUID", GetType(Guid))
                LIN.Columns.Add("837_CLM_GUID", GetType(Guid))
                LIN.Columns.Add("837_LX_GUID", GetType(Guid))
                LIN.Columns.Add("837_SVD_GUID", GetType(Guid))
                LIN.Columns.Add("HL01", GetType(Integer))
                LIN.Columns.Add("HL02", GetType(Integer))
                LIN.Columns.Add("HL03", GetType(Integer))
                LIN.Columns.Add("HL04", GetType(Integer))
                LIN.Columns.Add("LIN01", GetType(String))
                LIN.Columns.Add("LIN02", GetType(String))
                LIN.Columns.Add("LIN03", GetType(String))
                LIN.Columns.Add("LIN04", GetType(String))
                LIN.Columns.Add("LIN05", GetType(String))
                LIN.Columns.Add("LIN06", GetType(String))
                LIN.Columns.Add("LIN07", GetType(String))
                LIN.Columns.Add("LIN08", GetType(String))
                LIN.Columns.Add("LIN09", GetType(String))
                LIN.Columns.Add("LIN10", GetType(String))
                LIN.Columns.Add("LIN11", GetType(String))
                LIN.Columns.Add("LIN12", GetType(String))
                LIN.Columns.Add("LIN13", GetType(String))
                LIN.Columns.Add("LIN14", GetType(String))
                LIN.Columns.Add("LIN15", GetType(String))
                LIN.Columns.Add("LIN16", GetType(String))
                LIN.Columns.Add("LIN17", GetType(String))
                LIN.Columns.Add("LIN18", GetType(String))
                LIN.Columns.Add("LIN19", GetType(String))
                LIN.Columns.Add("LIN20", GetType(String))
                LIN.Columns.Add("LIN21", GetType(String))
                LIN.Columns.Add("LIN22", GetType(String))
                LIN.Columns.Add("LIN23", GetType(String))
                LIN.Columns.Add("LIN24", GetType(String))
                LIN.Columns.Add("LIN25", GetType(String))
                LIN.Columns.Add("LIN26", GetType(String))
                LIN.Columns.Add("LIN27", GetType(String))
                LIN.Columns.Add("LIN28", GetType(String))
                LIN.Columns.Add("LIN29", GetType(String))
                LIN.Columns.Add("LIN30", GetType(String))
                LIN.Columns.Add("LIN31", GetType(String))
                LIN.Columns.Add("ROW_NUMBER", GetType(Integer))
                LIN.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                LIN.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                LIN.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   MIA
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                MIA.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                MIA.Columns.Add("DOCUMENT_ID", GetType(Integer))
                MIA.Columns.Add("FILE_ID", GetType(Integer))
                MIA.Columns.Add("BATCH_ID", GetType(Integer))
                MIA.Columns.Add("ISA_ID", GetType(Integer))
                MIA.Columns.Add("GS_ID", GetType(Integer))
                MIA.Columns.Add("ST_ID", GetType(Integer))
                MIA.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                MIA.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                MIA.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                MIA.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                MIA.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                MIA.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                MIA.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                MIA.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                MIA.Columns.Add("837_SBR_GUID", GetType(Guid))
                MIA.Columns.Add("837_CLM_GUID", GetType(Guid))
                MIA.Columns.Add("837_LX_GUID", GetType(Guid))
                MIA.Columns.Add("HL01", GetType(Integer))
                MIA.Columns.Add("HL02", GetType(Integer))
                MIA.Columns.Add("HL03", GetType(Integer))
                MIA.Columns.Add("HL04", GetType(Integer))
                MIA.Columns.Add("MIA01", GetType(String))
                MIA.Columns.Add("MIA02", GetType(String))
                MIA.Columns.Add("MIA03", GetType(String))
                MIA.Columns.Add("MIA04", GetType(String))
                MIA.Columns.Add("MIA05", GetType(String))
                MIA.Columns.Add("MIA06", GetType(String))
                MIA.Columns.Add("MIA07", GetType(String))
                MIA.Columns.Add("MIA08", GetType(String))
                MIA.Columns.Add("MIA09", GetType(String))
                MIA.Columns.Add("MIA10", GetType(String))
                MIA.Columns.Add("MIA11", GetType(String))
                MIA.Columns.Add("MIA12", GetType(String))
                MIA.Columns.Add("MIA13", GetType(String))
                MIA.Columns.Add("MIA14", GetType(String))
                MIA.Columns.Add("MIA15", GetType(String))
                MIA.Columns.Add("MIA16", GetType(String))
                MIA.Columns.Add("MIA17", GetType(String))
                MIA.Columns.Add("MIA18", GetType(String))
                MIA.Columns.Add("MIA19", GetType(String))
                MIA.Columns.Add("MIA20", GetType(String))
                MIA.Columns.Add("MIA21", GetType(String))
                MIA.Columns.Add("MIA22", GetType(String))
                MIA.Columns.Add("MIA23", GetType(String))
                MIA.Columns.Add("MIA24", GetType(String))
                MIA.Columns.Add("ROW_NUMBER", GetType(Integer))
                MIA.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                MIA.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                MIA.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception


            End Try

            Try
                CACHE_MIA.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_MIA.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_MIA.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_MIA.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_MIA.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_MIA.Columns.Add("GS_ID", GetType(Integer))
                CACHE_MIA.Columns.Add("ST_ID", GetType(Integer))
                CACHE_MIA.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_MIA.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_MIA.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_MIA.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_MIA.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_MIA.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_MIA.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_MIA.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_MIA.Columns.Add("837_SBR_GUID", GetType(Guid))
                CACHE_MIA.Columns.Add("837_CLM_GUID", GetType(Guid))
                CACHE_MIA.Columns.Add("837_LX_GUID", GetType(Guid))
                CACHE_MIA.Columns.Add("HL01", GetType(Integer))
                CACHE_MIA.Columns.Add("HL02", GetType(Integer))
                CACHE_MIA.Columns.Add("HL03", GetType(Integer))
                CACHE_MIA.Columns.Add("HL04", GetType(Integer))
                CACHE_MIA.Columns.Add("MIA01", GetType(String))
                CACHE_MIA.Columns.Add("MIA02", GetType(String))
                CACHE_MIA.Columns.Add("MIA03", GetType(String))
                CACHE_MIA.Columns.Add("MIA04", GetType(String))
                CACHE_MIA.Columns.Add("MIA05", GetType(String))
                CACHE_MIA.Columns.Add("MIA06", GetType(String))
                CACHE_MIA.Columns.Add("MIA07", GetType(String))
                CACHE_MIA.Columns.Add("MIA08", GetType(String))
                CACHE_MIA.Columns.Add("MIA09", GetType(String))
                CACHE_MIA.Columns.Add("MIA10", GetType(String))
                CACHE_MIA.Columns.Add("MIA11", GetType(String))
                CACHE_MIA.Columns.Add("MIA12", GetType(String))
                CACHE_MIA.Columns.Add("MIA13", GetType(String))
                CACHE_MIA.Columns.Add("MIA14", GetType(String))
                CACHE_MIA.Columns.Add("MIA15", GetType(String))
                CACHE_MIA.Columns.Add("MIA16", GetType(String))
                CACHE_MIA.Columns.Add("MIA17", GetType(String))
                CACHE_MIA.Columns.Add("MIA18", GetType(String))
                CACHE_MIA.Columns.Add("MIA19", GetType(String))
                CACHE_MIA.Columns.Add("MIA20", GetType(String))
                CACHE_MIA.Columns.Add("MIA21", GetType(String))
                CACHE_MIA.Columns.Add("MIA22", GetType(String))
                CACHE_MIA.Columns.Add("MIA23", GetType(String))
                CACHE_MIA.Columns.Add("MIA24", GetType(String))
                CACHE_MIA.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_MIA.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_MIA.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_MIA.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception


            End Try

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   MOA
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                MOA.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                MOA.Columns.Add("DOCUMENT_ID", GetType(Integer))
                MOA.Columns.Add("FILE_ID", GetType(Integer))
                MOA.Columns.Add("BATCH_ID", GetType(Integer))
                MOA.Columns.Add("ISA_ID", GetType(Integer))
                MOA.Columns.Add("GS_ID", GetType(Integer))
                MOA.Columns.Add("ST_ID", GetType(Integer))
                MOA.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                MOA.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                MOA.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                MOA.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                MOA.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                MOA.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                MOA.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                MOA.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                MOA.Columns.Add("837_SBR_GUID", GetType(Guid))
                MOA.Columns.Add("837_CLM_GUID", GetType(Guid))
                MOA.Columns.Add("837_LX_GUID", GetType(Guid))
                MOA.Columns.Add("HL01", GetType(Integer))
                MOA.Columns.Add("HL02", GetType(Integer))
                MOA.Columns.Add("HL03", GetType(Integer))
                MOA.Columns.Add("HL04", GetType(Integer))
                MOA.Columns.Add("MOA01", GetType(String))
                MOA.Columns.Add("MOA02", GetType(String))
                MOA.Columns.Add("MOA03", GetType(String))
                MOA.Columns.Add("MOA04", GetType(String))
                MOA.Columns.Add("MOA05", GetType(String))
                MOA.Columns.Add("MOA06", GetType(String))
                MOA.Columns.Add("MOA07", GetType(String))
                MOA.Columns.Add("MOA08", GetType(String))
                MOA.Columns.Add("MOA09", GetType(String))
                MOA.Columns.Add("ROW_NUMBER", GetType(Integer))
                MOA.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                MOA.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                MOA.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception


            End Try



            Try
                CACHE_MOA.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_MOA.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_MOA.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_MOA.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_MOA.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_MOA.Columns.Add("GS_ID", GetType(Integer))
                CACHE_MOA.Columns.Add("ST_ID", GetType(Integer))
                CACHE_MOA.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_MOA.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_MOA.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_MOA.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_MOA.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_MOA.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_MOA.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_MOA.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_MOA.Columns.Add("837_SBR_GUID", GetType(Guid))
                CACHE_MOA.Columns.Add("837_CLM_GUID", GetType(Guid))
                CACHE_MOA.Columns.Add("837_LX_GUID", GetType(Guid))
                CACHE_MOA.Columns.Add("HL01", GetType(Integer))
                CACHE_MOA.Columns.Add("HL02", GetType(Integer))
                CACHE_MOA.Columns.Add("HL03", GetType(Integer))
                CACHE_MOA.Columns.Add("HL04", GetType(Integer))
                CACHE_MOA.Columns.Add("MOA01", GetType(String))
                CACHE_MOA.Columns.Add("MOA02", GetType(String))
                CACHE_MOA.Columns.Add("MOA03", GetType(String))
                CACHE_MOA.Columns.Add("MOA04", GetType(String))
                CACHE_MOA.Columns.Add("MOA05", GetType(String))
                CACHE_MOA.Columns.Add("MOA06", GetType(String))
                CACHE_MOA.Columns.Add("MOA07", GetType(String))
                CACHE_MOA.Columns.Add("MOA08", GetType(String))
                CACHE_MOA.Columns.Add("MOA09", GetType(String))
                CACHE_MOA.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_MOA.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_MOA.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_MOA.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception


            End Try








            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   N3
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                N3.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                N3.Columns.Add("DOCUMENT_ID", GetType(Integer))
                N3.Columns.Add("FILE_ID", GetType(Integer))
                N3.Columns.Add("BATCH_ID", GetType(Integer))
                N3.Columns.Add("ISA_ID", GetType(Integer))
                N3.Columns.Add("GS_ID", GetType(Integer))
                N3.Columns.Add("ST_ID", GetType(Integer))
                N3.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                N3.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                N3.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                N3.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                N3.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                N3.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                N3.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                N3.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                N3.Columns.Add("837_SBR_GUID", GetType(Guid))
                N3.Columns.Add("837_CLM_GUID", GetType(Guid))
                N3.Columns.Add("837_LX_GUID", GetType(Guid))
                N3.Columns.Add("NM1_GUID", GetType(Guid))
                N3.Columns.Add("HL01", GetType(Integer))
                N3.Columns.Add("HL02", GetType(Integer))
                N3.Columns.Add("HL03", GetType(Integer))
                N3.Columns.Add("HL04", GetType(Integer))
                N3.Columns.Add("N301", GetType(String))
                N3.Columns.Add("N302", GetType(String))
                N3.Columns.Add("ROW_NUMBER", GetType(Integer))
                N3.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                N3.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                N3.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try






            Try
                CACHE_2000_N3.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_2000_N3.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_2000_N3.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_2000_N3.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_2000_N3.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_2000_N3.Columns.Add("GS_ID", GetType(Integer))
                CACHE_2000_N3.Columns.Add("ST_ID", GetType(Integer))
                CACHE_2000_N3.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_2000_N3.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_2000_N3.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_2000_N3.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_2000_N3.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_2000_N3.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_2000_N3.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_2000_N3.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_2000_N3.Columns.Add("837_SBR_GUID", GetType(Guid))
                CACHE_2000_N3.Columns.Add("837_CLM_GUID", GetType(Guid))
                CACHE_2000_N3.Columns.Add("837_LX_GUID", GetType(Guid))
                CACHE_2000_N3.Columns.Add("NM1_GUID", GetType(Guid))
                CACHE_2000_N3.Columns.Add("HL01", GetType(Integer))
                CACHE_2000_N3.Columns.Add("HL02", GetType(Integer))
                CACHE_2000_N3.Columns.Add("HL03", GetType(Integer))
                CACHE_2000_N3.Columns.Add("HL04", GetType(Integer))
                CACHE_2000_N3.Columns.Add("N301", GetType(String))
                CACHE_2000_N3.Columns.Add("N302", GetType(String))
                CACHE_2000_N3.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_2000_N3.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_2000_N3.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_2000_N3.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   N4
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try

                N4.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                N4.Columns.Add("DOCUMENT_ID", GetType(Integer))
                N4.Columns.Add("FILE_ID", GetType(Integer))
                N4.Columns.Add("BATCH_ID", GetType(Integer))
                N4.Columns.Add("ISA_ID", GetType(Integer))
                N4.Columns.Add("GS_ID", GetType(Integer))
                N4.Columns.Add("ST_ID", GetType(Integer))
                N4.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                N4.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                N4.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                N4.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                N4.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                N4.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                N4.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                N4.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                N4.Columns.Add("837_SBR_GUID", GetType(Guid))
                N4.Columns.Add("837_CLM_GUID", GetType(Guid))
                N4.Columns.Add("837_LX_GUID", GetType(Guid))
                N4.Columns.Add("NM1_GUID", GetType(Guid))
                N4.Columns.Add("HL01", GetType(Integer))
                N4.Columns.Add("HL02", GetType(Integer))
                N4.Columns.Add("HL03", GetType(Integer))
                N4.Columns.Add("HL04", GetType(Integer))
                N4.Columns.Add("N401", GetType(String))
                N4.Columns.Add("N402", GetType(String))
                N4.Columns.Add("N403", GetType(String))
                N4.Columns.Add("N404", GetType(String))
                N4.Columns.Add("N405", GetType(String))
                N4.Columns.Add("N406", GetType(String))
                N4.Columns.Add("N407", GetType(String))
                N4.Columns.Add("ROW_NUMBER", GetType(Integer))
                N4.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                N4.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                N4.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try


            Try

                CACHE_2000_N4.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_2000_N4.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_2000_N4.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_2000_N4.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_2000_N4.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_2000_N4.Columns.Add("GS_ID", GetType(Integer))
                CACHE_2000_N4.Columns.Add("ST_ID", GetType(Integer))
                CACHE_2000_N4.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_2000_N4.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_2000_N4.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_2000_N4.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_2000_N4.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_2000_N4.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_2000_N4.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_2000_N4.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_2000_N4.Columns.Add("837_SBR_GUID", GetType(Guid))
                CACHE_2000_N4.Columns.Add("837_CLM_GUID", GetType(Guid))
                CACHE_2000_N4.Columns.Add("837_LX_GUID", GetType(Guid))
                CACHE_2000_N4.Columns.Add("NM1_GUID", GetType(Guid))
                CACHE_2000_N4.Columns.Add("HL01", GetType(Integer))
                CACHE_2000_N4.Columns.Add("HL02", GetType(Integer))
                CACHE_2000_N4.Columns.Add("HL03", GetType(Integer))
                CACHE_2000_N4.Columns.Add("HL04", GetType(Integer))
                CACHE_2000_N4.Columns.Add("N401", GetType(String))
                CACHE_2000_N4.Columns.Add("N402", GetType(String))
                CACHE_2000_N4.Columns.Add("N403", GetType(String))
                CACHE_2000_N4.Columns.Add("N404", GetType(String))
                CACHE_2000_N4.Columns.Add("N405", GetType(String))
                CACHE_2000_N4.Columns.Add("N406", GetType(String))
                CACHE_2000_N4.Columns.Add("N407", GetType(String))
                CACHE_2000_N4.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_2000_N4.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_2000_N4.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_2000_N4.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
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
                NM1.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                NM1.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                NM1.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                NM1.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                NM1.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                NM1.Columns.Add("837_SBR_GUID", GetType(Guid))
                NM1.Columns.Add("837_CLM_GUID", GetType(Guid))
                NM1.Columns.Add("837_LX_GUID", GetType(Guid))
                NM1.Columns.Add("NM1_GUID", GetType(Guid))
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
                CACHE_1000_NM1.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_1000_NM1.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_1000_NM1.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_1000_NM1.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_1000_NM1.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_1000_NM1.Columns.Add("GS_ID", GetType(Integer))
                CACHE_1000_NM1.Columns.Add("ST_ID", GetType(Integer))
                CACHE_1000_NM1.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_1000_NM1.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_1000_NM1.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_1000_NM1.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_1000_NM1.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_1000_NM1.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_1000_NM1.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_1000_NM1.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_1000_NM1.Columns.Add("837_SBR_GUID", GetType(Guid))
                CACHE_1000_NM1.Columns.Add("837_CLM_GUID", GetType(Guid))
                CACHE_1000_NM1.Columns.Add("837_LX_GUID", GetType(Guid))
                CACHE_1000_NM1.Columns.Add("NM1_GUID", GetType(Guid))
                CACHE_1000_NM1.Columns.Add("HL01", GetType(Integer))
                CACHE_1000_NM1.Columns.Add("HL02", GetType(Integer))
                CACHE_1000_NM1.Columns.Add("HL03", GetType(Integer))
                CACHE_1000_NM1.Columns.Add("HL04", GetType(Integer))
                CACHE_1000_NM1.Columns.Add("NM101", GetType(String))
                CACHE_1000_NM1.Columns.Add("NM102", GetType(String))
                CACHE_1000_NM1.Columns.Add("NM103", GetType(String))
                CACHE_1000_NM1.Columns.Add("NM104", GetType(String))
                CACHE_1000_NM1.Columns.Add("NM105", GetType(String))
                CACHE_1000_NM1.Columns.Add("NM106", GetType(String))
                CACHE_1000_NM1.Columns.Add("NM107", GetType(String))
                CACHE_1000_NM1.Columns.Add("NM108", GetType(String))
                CACHE_1000_NM1.Columns.Add("NM109", GetType(String))
                CACHE_1000_NM1.Columns.Add("NM110", GetType(String))
                CACHE_1000_NM1.Columns.Add("NM111", GetType(String))
                CACHE_1000_NM1.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_1000_NM1.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_1000_NM1.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_1000_NM1.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))

            Catch ex As Exception

            End Try


            Try
                CACHE_2000_NM1.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_2000_NM1.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_2000_NM1.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_2000_NM1.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_2000_NM1.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_2000_NM1.Columns.Add("GS_ID", GetType(Integer))
                CACHE_2000_NM1.Columns.Add("ST_ID", GetType(Integer))
                CACHE_2000_NM1.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_2000_NM1.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_2000_NM1.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_2000_NM1.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_2000_NM1.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_2000_NM1.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_2000_NM1.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_2000_NM1.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_2000_NM1.Columns.Add("837_SBR_GUID", GetType(Guid))
                CACHE_2000_NM1.Columns.Add("837_CLM_GUID", GetType(Guid))
                CACHE_2000_NM1.Columns.Add("837_LX_GUID", GetType(Guid))
                CACHE_2000_NM1.Columns.Add("NM1_GUID", GetType(Guid))
                CACHE_2000_NM1.Columns.Add("HL01", GetType(Integer))
                CACHE_2000_NM1.Columns.Add("HL02", GetType(Integer))
                CACHE_2000_NM1.Columns.Add("HL03", GetType(Integer))
                CACHE_2000_NM1.Columns.Add("HL04", GetType(Integer))
                CACHE_2000_NM1.Columns.Add("NM101", GetType(String))
                CACHE_2000_NM1.Columns.Add("NM102", GetType(String))
                CACHE_2000_NM1.Columns.Add("NM103", GetType(String))
                CACHE_2000_NM1.Columns.Add("NM104", GetType(String))
                CACHE_2000_NM1.Columns.Add("NM105", GetType(String))
                CACHE_2000_NM1.Columns.Add("NM106", GetType(String))
                CACHE_2000_NM1.Columns.Add("NM107", GetType(String))
                CACHE_2000_NM1.Columns.Add("NM108", GetType(String))
                CACHE_2000_NM1.Columns.Add("NM109", GetType(String))
                CACHE_2000_NM1.Columns.Add("NM110", GetType(String))
                CACHE_2000_NM1.Columns.Add("NM111", GetType(String))
                CACHE_2000_NM1.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_2000_NM1.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_2000_NM1.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_2000_NM1.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))

            Catch ex As Exception

            End Try


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   NTE
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                NTE.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                NTE.Columns.Add("DOCUMENT_ID", GetType(Integer))
                NTE.Columns.Add("FILE_ID", GetType(Integer))
                NTE.Columns.Add("BATCH_ID", GetType(Integer))
                NTE.Columns.Add("ISA_ID", GetType(Integer))
                NTE.Columns.Add("GS_ID", GetType(Integer))
                NTE.Columns.Add("ST_ID", GetType(Integer))
                NTE.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                NTE.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                NTE.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                NTE.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                NTE.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                NTE.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                NTE.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                NTE.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                NTE.Columns.Add("837_SBR_GUID", GetType(Guid))
                NTE.Columns.Add("837_CLM_GUID", GetType(Guid))
                NTE.Columns.Add("837_LX_GUID", GetType(Guid))
                NTE.Columns.Add("HL01", GetType(Integer))
                NTE.Columns.Add("HL02", GetType(Integer))
                NTE.Columns.Add("HL03", GetType(Integer))
                NTE.Columns.Add("HL04", GetType(Integer))
                NTE.Columns.Add("NTE01", GetType(String))
                NTE.Columns.Add("NTE02", GetType(String))
                NTE.Columns.Add("ROW_NUMBER", GetType(Integer))
                NTE.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                NTE.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                NTE.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))

            Catch ex As Exception

            End Try

            Try
                CACHE_NTE.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_NTE.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_NTE.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_NTE.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_NTE.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_NTE.Columns.Add("GS_ID", GetType(Integer))
                CACHE_NTE.Columns.Add("ST_ID", GetType(Integer))
                CACHE_NTE.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_NTE.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_NTE.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_NTE.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_NTE.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_NTE.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_NTE.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_NTE.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_NTE.Columns.Add("837_SBR_GUID", GetType(Guid))
                CACHE_NTE.Columns.Add("837_CLM_GUID", GetType(Guid))
                CACHE_NTE.Columns.Add("837_LX_GUID", GetType(Guid))
                CACHE_NTE.Columns.Add("HL01", GetType(Integer))
                CACHE_NTE.Columns.Add("HL02", GetType(Integer))
                CACHE_NTE.Columns.Add("HL03", GetType(Integer))
                CACHE_NTE.Columns.Add("HL04", GetType(Integer))
                CACHE_NTE.Columns.Add("NTE01", GetType(String))
                CACHE_NTE.Columns.Add("NTE02", GetType(String))
                CACHE_NTE.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_NTE.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_NTE.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_NTE.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))

            Catch ex As Exception

            End Try

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   OI
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                OI.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                OI.Columns.Add("DOCUMENT_ID", GetType(Integer))
                OI.Columns.Add("FILE_ID", GetType(Integer))
                OI.Columns.Add("BATCH_ID", GetType(Integer))
                OI.Columns.Add("ISA_ID", GetType(Integer))
                OI.Columns.Add("GS_ID", GetType(Integer))
                OI.Columns.Add("ST_ID", GetType(Integer))
                OI.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                OI.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                OI.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                OI.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                OI.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                OI.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                OI.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                OI.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                OI.Columns.Add("837_SBR_GUID", GetType(Guid))
                OI.Columns.Add("837_CLM_GUID", GetType(Guid))
                OI.Columns.Add("837_LX_GUID", GetType(Guid))
                OI.Columns.Add("HL01", GetType(Integer))
                OI.Columns.Add("HL02", GetType(Integer))
                OI.Columns.Add("HL03", GetType(Integer))
                OI.Columns.Add("HL04", GetType(Integer))
                OI.Columns.Add("OI01", GetType(String))
                OI.Columns.Add("OI02", GetType(String))
                OI.Columns.Add("OI03", GetType(String))
                OI.Columns.Add("OI04", GetType(String))
                OI.Columns.Add("OI05", GetType(String))
                OI.Columns.Add("OI06", GetType(String))
                OI.Columns.Add("ROW_NUMBER", GetType(Integer))
                OI.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                OI.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                OI.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))

            Catch ex As Exception

            End Try

            Try
                CACHE_OI.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_OI.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_OI.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_OI.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_OI.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_OI.Columns.Add("GS_ID", GetType(Integer))
                CACHE_OI.Columns.Add("ST_ID", GetType(Integer))
                CACHE_OI.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_OI.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_OI.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_OI.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_OI.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_OI.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_OI.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_OI.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_OI.Columns.Add("837_SBR_GUID", GetType(Guid))
                CACHE_OI.Columns.Add("837_CLM_GUID", GetType(Guid))
                CACHE_OI.Columns.Add("837_LX_GUID", GetType(Guid))
                CACHE_OI.Columns.Add("HL01", GetType(Integer))
                CACHE_OI.Columns.Add("HL02", GetType(Integer))
                CACHE_OI.Columns.Add("HL03", GetType(Integer))
                CACHE_OI.Columns.Add("HL04", GetType(Integer))
                CACHE_OI.Columns.Add("OI01", GetType(String))
                CACHE_OI.Columns.Add("OI02", GetType(String))
                CACHE_OI.Columns.Add("OI03", GetType(String))
                CACHE_OI.Columns.Add("OI04", GetType(String))
                CACHE_OI.Columns.Add("OI05", GetType(String))
                CACHE_OI.Columns.Add("OI06", GetType(String))
                CACHE_OI.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_OI.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_OI.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_OI.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))

            Catch ex As Exception

            End Try



            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '  PAT
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                PAT.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                PAT.Columns.Add("DOCUMENT_ID", GetType(Integer))
                PAT.Columns.Add("FILE_ID", GetType(Integer))
                PAT.Columns.Add("BATCH_ID", GetType(Integer))
                PAT.Columns.Add("ISA_ID", GetType(Integer))
                PAT.Columns.Add("GS_ID", GetType(Integer))
                PAT.Columns.Add("ST_ID", GetType(Integer))
                PAT.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                PAT.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                PAT.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                PAT.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                PAT.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                PAT.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                PAT.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                PAT.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                PAT.Columns.Add("837_SBR_GUID", GetType(Guid))
                PAT.Columns.Add("837_CLM_GUID", GetType(Guid))
                PAT.Columns.Add("837_LX_GUID", GetType(Guid))
                PAT.Columns.Add("HL01", GetType(Integer))
                PAT.Columns.Add("HL02", GetType(Integer))
                PAT.Columns.Add("HL03", GetType(Integer))
                PAT.Columns.Add("HL04", GetType(Integer))
                PAT.Columns.Add("PAT01", GetType(String))
                PAT.Columns.Add("PAT02", GetType(String))
                PAT.Columns.Add("PAT03", GetType(String))
                PAT.Columns.Add("PAT04", GetType(String))
                PAT.Columns.Add("PAT05", GetType(String))
                PAT.Columns.Add("PAT06", GetType(String))
                PAT.Columns.Add("PAT07", GetType(String))
                PAT.Columns.Add("PAT08", GetType(String))
                PAT.Columns.Add("PAT09", GetType(String))
                PAT.Columns.Add("ROW_NUMBER", GetType(Integer))
                PAT.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                PAT.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                PAT.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try



            Try
                CACHE_2000_PAT.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_2000_PAT.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_2000_PAT.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_2000_PAT.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_2000_PAT.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_2000_PAT.Columns.Add("GS_ID", GetType(Integer))
                CACHE_2000_PAT.Columns.Add("ST_ID", GetType(Integer))
                CACHE_2000_PAT.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_2000_PAT.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_2000_PAT.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_2000_PAT.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_2000_PAT.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_2000_PAT.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_2000_PAT.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_2000_PAT.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_2000_PAT.Columns.Add("837_SBR_GUID", GetType(Guid))
                CACHE_2000_PAT.Columns.Add("837_CLM_GUID", GetType(Guid))
                CACHE_2000_PAT.Columns.Add("837_LX_GUID", GetType(Guid))
                CACHE_2000_PAT.Columns.Add("HL01", GetType(Integer))
                CACHE_2000_PAT.Columns.Add("HL02", GetType(Integer))
                CACHE_2000_PAT.Columns.Add("HL03", GetType(Integer))
                CACHE_2000_PAT.Columns.Add("HL04", GetType(Integer))
                CACHE_2000_PAT.Columns.Add("PAT01", GetType(String))
                CACHE_2000_PAT.Columns.Add("PAT02", GetType(String))
                CACHE_2000_PAT.Columns.Add("PAT03", GetType(String))
                CACHE_2000_PAT.Columns.Add("PAT04", GetType(String))
                CACHE_2000_PAT.Columns.Add("PAT05", GetType(String))
                CACHE_2000_PAT.Columns.Add("PAT06", GetType(String))
                CACHE_2000_PAT.Columns.Add("PAT07", GetType(String))
                CACHE_2000_PAT.Columns.Add("PAT08", GetType(String))
                CACHE_2000_PAT.Columns.Add("PAT09", GetType(String))
                CACHE_2000_PAT.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_2000_PAT.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_2000_PAT.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_2000_PAT.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
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
                PER.Columns.Add("837_SBR_GUID", GetType(Guid))
                PER.Columns.Add("837_CLM_GUID", GetType(Guid))
                PER.Columns.Add("837_LX_GUID", GetType(Guid))
                PER.Columns.Add("NM1_GUID", GetType(Guid))
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
                CACHE_1000_PER.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_1000_PER.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_1000_PER.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_1000_PER.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_1000_PER.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_1000_PER.Columns.Add("GS_ID", GetType(Integer))
                CACHE_1000_PER.Columns.Add("ST_ID", GetType(Integer))
                CACHE_1000_PER.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_1000_PER.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_1000_PER.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_1000_PER.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_1000_PER.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_1000_PER.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_1000_PER.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_1000_PER.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_1000_PER.Columns.Add("837_SBR_GUID", GetType(Guid))
                CACHE_1000_PER.Columns.Add("837_CLM_GUID", GetType(Guid))
                CACHE_1000_PER.Columns.Add("837_LX_GUID", GetType(Guid))
                CACHE_1000_PER.Columns.Add("NM1_GUID", GetType(Guid))
                CACHE_1000_PER.Columns.Add("HL01", GetType(Integer))
                CACHE_1000_PER.Columns.Add("HL02", GetType(Integer))
                CACHE_1000_PER.Columns.Add("HL03", GetType(Integer))
                CACHE_1000_PER.Columns.Add("HL04", GetType(Integer))
                CACHE_1000_PER.Columns.Add("PER01", GetType(String))
                CACHE_1000_PER.Columns.Add("PER02", GetType(String))
                CACHE_1000_PER.Columns.Add("PER03", GetType(String))
                CACHE_1000_PER.Columns.Add("PER04", GetType(String))
                CACHE_1000_PER.Columns.Add("PER05", GetType(String))
                CACHE_1000_PER.Columns.Add("PER06", GetType(String))
                CACHE_1000_PER.Columns.Add("PER07", GetType(String))
                CACHE_1000_PER.Columns.Add("PER08", GetType(String))
                CACHE_1000_PER.Columns.Add("PER09", GetType(String))
                CACHE_1000_PER.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_1000_PER.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_1000_PER.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_1000_PER.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try



            Try
                CACHE_2000_PER.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_2000_PER.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_2000_PER.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_2000_PER.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_2000_PER.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_2000_PER.Columns.Add("GS_ID", GetType(Integer))
                CACHE_2000_PER.Columns.Add("ST_ID", GetType(Integer))
                CACHE_2000_PER.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_2000_PER.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_2000_PER.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_2000_PER.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_2000_PER.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_2000_PER.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_2000_PER.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_2000_PER.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_2000_PER.Columns.Add("837_SBR_GUID", GetType(Guid))
                CACHE_2000_PER.Columns.Add("837_CLM_GUID", GetType(Guid))
                CACHE_2000_PER.Columns.Add("837_LX_GUID", GetType(Guid))
                CACHE_2000_PER.Columns.Add("NM1_GUID", GetType(Guid))
                CACHE_2000_PER.Columns.Add("HL01", GetType(Integer))
                CACHE_2000_PER.Columns.Add("HL02", GetType(Integer))
                CACHE_2000_PER.Columns.Add("HL03", GetType(Integer))
                CACHE_2000_PER.Columns.Add("HL04", GetType(Integer))
                CACHE_2000_PER.Columns.Add("PER01", GetType(String))
                CACHE_2000_PER.Columns.Add("PER02", GetType(String))
                CACHE_2000_PER.Columns.Add("PER03", GetType(String))
                CACHE_2000_PER.Columns.Add("PER04", GetType(String))
                CACHE_2000_PER.Columns.Add("PER05", GetType(String))
                CACHE_2000_PER.Columns.Add("PER06", GetType(String))
                CACHE_2000_PER.Columns.Add("PER07", GetType(String))
                CACHE_2000_PER.Columns.Add("PER08", GetType(String))
                CACHE_2000_PER.Columns.Add("PER09", GetType(String))
                CACHE_2000_PER.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_2000_PER.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_2000_PER.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_2000_PER.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   PRV
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                PRV.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                PRV.Columns.Add("DOCUMENT_ID", GetType(Integer))
                PRV.Columns.Add("FILE_ID", GetType(Integer))
                PRV.Columns.Add("BATCH_ID", GetType(Integer))
                PRV.Columns.Add("ISA_ID", GetType(Integer))
                PRV.Columns.Add("GS_ID", GetType(Integer))
                PRV.Columns.Add("ST_ID", GetType(Integer))
                PRV.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                PRV.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                PRV.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                PRV.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                PRV.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                PRV.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                PRV.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                PRV.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                PRV.Columns.Add("837_SBR_GUID", GetType(Guid))
                PRV.Columns.Add("837_CLM_GUID", GetType(Guid))
                PRV.Columns.Add("837_LX_GUID", GetType(Guid))
                PRV.Columns.Add("NM1_GUID", GetType(Guid))
                PRV.Columns.Add("HL01", GetType(Integer))
                PRV.Columns.Add("HL02", GetType(Integer))
                PRV.Columns.Add("HL03", GetType(Integer))
                PRV.Columns.Add("HL04", GetType(Integer))
                PRV.Columns.Add("PRV01", GetType(String))
                PRV.Columns.Add("PRV02", GetType(String))
                PRV.Columns.Add("PRV03", GetType(String))
                PRV.Columns.Add("PRV04", GetType(String))
                PRV.Columns.Add("PRV05", GetType(String))
                PRV.Columns.Add("PRV06", GetType(String))
                PRV.Columns.Add("ROW_NUMBER", GetType(Integer))
                PRV.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                PRV.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                PRV.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))

            Catch ex As Exception

            End Try

            Try
                CACHE_2000_PRV.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_2000_PRV.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_2000_PRV.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_2000_PRV.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_2000_PRV.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_2000_PRV.Columns.Add("GS_ID", GetType(Integer))
                CACHE_2000_PRV.Columns.Add("ST_ID", GetType(Integer))
                CACHE_2000_PRV.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_2000_PRV.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_2000_PRV.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_2000_PRV.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_2000_PRV.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_2000_PRV.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_2000_PRV.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_2000_PRV.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_2000_PRV.Columns.Add("837_SBR_GUID", GetType(Guid))
                CACHE_2000_PRV.Columns.Add("837_CLM_GUID", GetType(Guid))
                CACHE_2000_PRV.Columns.Add("837_LX_GUID", GetType(Guid))
                CACHE_2000_PRV.Columns.Add("NM1_GUID", GetType(Guid))
                CACHE_2000_PRV.Columns.Add("HL01", GetType(Integer))
                CACHE_2000_PRV.Columns.Add("HL02", GetType(Integer))
                CACHE_2000_PRV.Columns.Add("HL03", GetType(Integer))
                CACHE_2000_PRV.Columns.Add("HL04", GetType(Integer))
                CACHE_2000_PRV.Columns.Add("PRV01", GetType(String))
                CACHE_2000_PRV.Columns.Add("PRV02", GetType(String))
                CACHE_2000_PRV.Columns.Add("PRV03", GetType(String))
                CACHE_2000_PRV.Columns.Add("PRV04", GetType(String))
                CACHE_2000_PRV.Columns.Add("PRV05", GetType(String))
                CACHE_2000_PRV.Columns.Add("PRV06", GetType(String))
                CACHE_2000_PRV.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_2000_PRV.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_2000_PRV.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_2000_PRV.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))

            Catch ex As Exception

            End Try






            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   PWK
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                PWK.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                PWK.Columns.Add("DOCUMENT_ID", GetType(Integer))
                PWK.Columns.Add("FILE_ID", GetType(Integer))
                PWK.Columns.Add("BATCH_ID", GetType(Integer))
                PWK.Columns.Add("ISA_ID", GetType(Integer))
                PWK.Columns.Add("GS_ID", GetType(Integer))
                PWK.Columns.Add("ST_ID", GetType(Integer))
                PWK.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                PWK.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                PWK.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                PWK.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                PWK.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                PWK.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                PWK.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                PWK.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                PWK.Columns.Add("837_SBR_GUID", GetType(Guid))
                PWK.Columns.Add("837_CLM_GUID", GetType(Guid))
                PWK.Columns.Add("837_LX_GUID", GetType(Guid))
                PWK.Columns.Add("HL01", GetType(Integer))
                PWK.Columns.Add("HL02", GetType(Integer))
                PWK.Columns.Add("HL03", GetType(Integer))
                PWK.Columns.Add("HL04", GetType(Integer))
                PWK.Columns.Add("PWK01", GetType(String))
                PWK.Columns.Add("PWK02", GetType(String))
                PWK.Columns.Add("PWK03", GetType(String))
                PWK.Columns.Add("PWK04", GetType(String))
                PWK.Columns.Add("PWK05", GetType(String))
                PWK.Columns.Add("PWK06", GetType(String))
                PWK.Columns.Add("PWK07", GetType(String))
                PWK.Columns.Add("PWK08", GetType(String))
                PWK.Columns.Add("PWK09", GetType(String))
                PWK.Columns.Add("ROW_NUMBER", GetType(Integer))
                PWK.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                PWK.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                PWK.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try


            Try
                CACHE_PWK.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_PWK.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_PWK.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_PWK.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_PWK.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_PWK.Columns.Add("GS_ID", GetType(Integer))
                CACHE_PWK.Columns.Add("ST_ID", GetType(Integer))
                CACHE_PWK.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_PWK.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_PWK.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_PWK.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_PWK.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_PWK.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_PWK.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_PWK.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_PWK.Columns.Add("837_SBR_GUID", GetType(Guid))
                CACHE_PWK.Columns.Add("837_CLM_GUID", GetType(Guid))
                CACHE_PWK.Columns.Add("837_LX_GUID", GetType(Guid))
                CACHE_PWK.Columns.Add("HL01", GetType(Integer))
                CACHE_PWK.Columns.Add("HL02", GetType(Integer))
                CACHE_PWK.Columns.Add("HL03", GetType(Integer))
                CACHE_PWK.Columns.Add("HL04", GetType(Integer))
                CACHE_PWK.Columns.Add("PWK01", GetType(String))
                CACHE_PWK.Columns.Add("PWK02", GetType(String))
                CACHE_PWK.Columns.Add("PWK03", GetType(String))
                CACHE_PWK.Columns.Add("PWK04", GetType(String))
                CACHE_PWK.Columns.Add("PWK05", GetType(String))
                CACHE_PWK.Columns.Add("PWK06", GetType(String))
                CACHE_PWK.Columns.Add("PWK07", GetType(String))
                CACHE_PWK.Columns.Add("PWK08", GetType(String))
                CACHE_PWK.Columns.Add("PWK09", GetType(String))
                CACHE_PWK.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_PWK.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_PWK.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_PWK.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
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
                REF.Columns.Add("837_SBR_GUID", GetType(Guid))
                REF.Columns.Add("837_CLM_GUID", GetType(Guid))
                REF.Columns.Add("837_LX_GUID", GetType(Guid))
                REF.Columns.Add("NM1_GUID", GetType(Guid))
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
                CACHE_2000_REF.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_2000_REF.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_2000_REF.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_2000_REF.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_2000_REF.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_2000_REF.Columns.Add("GS_ID", GetType(Integer))
                CACHE_2000_REF.Columns.Add("ST_ID", GetType(Integer))
                CACHE_2000_REF.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_2000_REF.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_2000_REF.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_2000_REF.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_2000_REF.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_2000_REF.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_2000_REF.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_2000_REF.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_2000_REF.Columns.Add("837_SBR_GUID", GetType(Guid))
                CACHE_2000_REF.Columns.Add("837_CLM_GUID", GetType(Guid))
                CACHE_2000_REF.Columns.Add("837_LX_GUID", GetType(Guid))
                CACHE_2000_REF.Columns.Add("NM1_GUID", GetType(Guid))
                CACHE_2000_REF.Columns.Add("HL01", GetType(Integer))
                CACHE_2000_REF.Columns.Add("HL02", GetType(Integer))
                CACHE_2000_REF.Columns.Add("HL03", GetType(Integer))
                CACHE_2000_REF.Columns.Add("HL04", GetType(Integer))
                CACHE_2000_REF.Columns.Add("REF01", GetType(String))
                CACHE_2000_REF.Columns.Add("REF02", GetType(String))
                CACHE_2000_REF.Columns.Add("REF03", GetType(String))
                CACHE_2000_REF.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_2000_REF.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_2000_REF.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_2000_REF.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))

            Catch ex As Exception

            End Try


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   SBR
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                SBR.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                SBR.Columns.Add("DOCUMENT_ID", GetType(Integer))
                SBR.Columns.Add("FILE_ID", GetType(Integer))
                SBR.Columns.Add("BATCH_ID", GetType(Integer))
                SBR.Columns.Add("ISA_ID", GetType(Integer))
                SBR.Columns.Add("GS_ID", GetType(Integer))
                SBR.Columns.Add("ST_ID", GetType(Integer))
                SBR.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                SBR.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                SBR.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                SBR.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                SBR.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                SBR.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                SBR.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                SBR.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                SBR.Columns.Add("837_SBR_GUID", GetType(Guid))
                SBR.Columns.Add("837_CLM_GUID", GetType(Guid))
                SBR.Columns.Add("837_LX_GUID", GetType(Guid))
                SBR.Columns.Add("HL01", GetType(Integer))
                SBR.Columns.Add("HL02", GetType(Integer))
                SBR.Columns.Add("HL03", GetType(Integer))
                SBR.Columns.Add("HL04", GetType(Integer))
                SBR.Columns.Add("SBR01", GetType(String))
                SBR.Columns.Add("SBR02", GetType(String))
                SBR.Columns.Add("SBR03", GetType(String))
                SBR.Columns.Add("SBR04", GetType(String))
                SBR.Columns.Add("SBR05", GetType(String))
                SBR.Columns.Add("SBR06", GetType(String))
                SBR.Columns.Add("SBR07", GetType(String))
                SBR.Columns.Add("SBR08", GetType(String))
                SBR.Columns.Add("SBR09", GetType(String))
                SBR.Columns.Add("ROW_NUMBER", GetType(Integer))
                SBR.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                SBR.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                SBR.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))



            Catch ex As Exception

            End Try


            Try
                CACHE_2000_SBR.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_2000_SBR.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_2000_SBR.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_2000_SBR.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_2000_SBR.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_2000_SBR.Columns.Add("GS_ID", GetType(Integer))
                CACHE_2000_SBR.Columns.Add("ST_ID", GetType(Integer))
                CACHE_2000_SBR.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_2000_SBR.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_2000_SBR.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_2000_SBR.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_2000_SBR.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_2000_SBR.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_2000_SBR.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_2000_SBR.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_2000_SBR.Columns.Add("837_SBR_GUID", GetType(Guid))
                CACHE_2000_SBR.Columns.Add("837_CLM_GUID", GetType(Guid))
                CACHE_2000_SBR.Columns.Add("837_LX_GUID", GetType(Guid))
                CACHE_2000_SBR.Columns.Add("HL01", GetType(Integer))
                CACHE_2000_SBR.Columns.Add("HL02", GetType(Integer))
                CACHE_2000_SBR.Columns.Add("HL03", GetType(Integer))
                CACHE_2000_SBR.Columns.Add("HL04", GetType(Integer))
                CACHE_2000_SBR.Columns.Add("SBR01", GetType(String))
                CACHE_2000_SBR.Columns.Add("SBR02", GetType(String))
                CACHE_2000_SBR.Columns.Add("SBR03", GetType(String))
                CACHE_2000_SBR.Columns.Add("SBR04", GetType(String))
                CACHE_2000_SBR.Columns.Add("SBR05", GetType(String))
                CACHE_2000_SBR.Columns.Add("SBR06", GetType(String))
                CACHE_2000_SBR.Columns.Add("SBR07", GetType(String))
                CACHE_2000_SBR.Columns.Add("SBR08", GetType(String))
                CACHE_2000_SBR.Columns.Add("SBR09", GetType(String))
                CACHE_2000_SBR.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_2000_SBR.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_2000_SBR.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_2000_SBR.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))

            Catch ex As Exception

            End Try




            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   SV1
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                SV1.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                SV1.Columns.Add("DOCUMENT_ID", GetType(Integer))
                SV1.Columns.Add("FILE_ID", GetType(Integer))
                SV1.Columns.Add("BATCH_ID", GetType(Integer))
                SV1.Columns.Add("ISA_ID", GetType(Integer))
                SV1.Columns.Add("GS_ID", GetType(Integer))
                SV1.Columns.Add("ST_ID", GetType(Integer))
                SV1.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                SV1.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                SV1.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                SV1.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                SV1.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                SV1.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                SV1.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                SV1.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                SV1.Columns.Add("837_SBR_GUID", GetType(Guid))
                SV1.Columns.Add("837_CLM_GUID", GetType(Guid))
                SV1.Columns.Add("837_LX_GUID", GetType(Guid))
                SV1.Columns.Add("HL01", GetType(Integer))
                SV1.Columns.Add("HL02", GetType(Integer))
                SV1.Columns.Add("HL03", GetType(Integer))
                SV1.Columns.Add("HL04", GetType(Integer))
                SV1.Columns.Add("SV101", GetType(String))
                SV1.Columns.Add("SV101_1", GetType(String))
                SV1.Columns.Add("SV101_2", GetType(String))
                SV1.Columns.Add("SV101_3", GetType(String))
                SV1.Columns.Add("SV101_4", GetType(String))
                SV1.Columns.Add("SV101_5", GetType(String))
                SV1.Columns.Add("SV101_6", GetType(String))
                SV1.Columns.Add("SV101_7", GetType(String))
                SV1.Columns.Add("SV102", GetType(String))
                SV1.Columns.Add("SV103", GetType(String))
                SV1.Columns.Add("SV104", GetType(String))
                SV1.Columns.Add("SV105", GetType(String))
                SV1.Columns.Add("SV106", GetType(String))
                SV1.Columns.Add("SV107", GetType(String))
                SV1.Columns.Add("SV107_1", GetType(String))
                SV1.Columns.Add("SV107_2", GetType(String))
                SV1.Columns.Add("SV107_3", GetType(String))
                SV1.Columns.Add("SV107_4", GetType(String))
                SV1.Columns.Add("SV108", GetType(String))
                SV1.Columns.Add("SV109", GetType(String))
                SV1.Columns.Add("SV110", GetType(String))
                SV1.Columns.Add("SV111", GetType(String))
                SV1.Columns.Add("SV112", GetType(String))
                SV1.Columns.Add("SV113", GetType(String))
                SV1.Columns.Add("SV114", GetType(String))
                SV1.Columns.Add("SV115", GetType(String))
                SV1.Columns.Add("ROW_NUMBER", GetType(Integer))
                SV1.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                SV1.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                SV1.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try


            Try
                CACHE_SV1.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_SV1.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_SV1.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_SV1.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_SV1.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_SV1.Columns.Add("GS_ID", GetType(Integer))
                CACHE_SV1.Columns.Add("ST_ID", GetType(Integer))
                CACHE_SV1.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_SV1.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_SV1.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_SV1.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_SV1.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_SV1.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_SV1.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_SV1.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_SV1.Columns.Add("837_SBR_GUID", GetType(Guid))
                CACHE_SV1.Columns.Add("837_CLM_GUID", GetType(Guid))
                CACHE_SV1.Columns.Add("837_LX_GUID", GetType(Guid))
                CACHE_SV1.Columns.Add("HL01", GetType(Integer))
                CACHE_SV1.Columns.Add("HL02", GetType(Integer))
                CACHE_SV1.Columns.Add("HL03", GetType(Integer))
                CACHE_SV1.Columns.Add("HL04", GetType(Integer))
                CACHE_SV1.Columns.Add("SV101", GetType(String))
                CACHE_SV1.Columns.Add("SV101_1", GetType(String))
                CACHE_SV1.Columns.Add("SV101_2", GetType(String))
                CACHE_SV1.Columns.Add("SV101_3", GetType(String))
                CACHE_SV1.Columns.Add("SV101_4", GetType(String))
                CACHE_SV1.Columns.Add("SV101_5", GetType(String))
                CACHE_SV1.Columns.Add("SV101_6", GetType(String))
                CACHE_SV1.Columns.Add("SV101_7", GetType(String))
                CACHE_SV1.Columns.Add("SV102", GetType(String))
                CACHE_SV1.Columns.Add("SV103", GetType(String))
                CACHE_SV1.Columns.Add("SV104", GetType(String))
                CACHE_SV1.Columns.Add("SV105", GetType(String))
                CACHE_SV1.Columns.Add("SV106", GetType(String))
                CACHE_SV1.Columns.Add("SV107", GetType(String))
                CACHE_SV1.Columns.Add("SV107_1", GetType(String))
                CACHE_SV1.Columns.Add("SV107_2", GetType(String))
                CACHE_SV1.Columns.Add("SV107_3", GetType(String))
                CACHE_SV1.Columns.Add("SV107_4", GetType(String))
                CACHE_SV1.Columns.Add("SV108", GetType(String))
                CACHE_SV1.Columns.Add("SV109", GetType(String))
                CACHE_SV1.Columns.Add("SV110", GetType(String))
                CACHE_SV1.Columns.Add("SV111", GetType(String))
                CACHE_SV1.Columns.Add("SV112", GetType(String))
                CACHE_SV1.Columns.Add("SV113", GetType(String))
                CACHE_SV1.Columns.Add("SV114", GetType(String))
                CACHE_SV1.Columns.Add("SV115", GetType(String))
                CACHE_SV1.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_SV1.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_SV1.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_SV1.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try



            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   SV2
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                SV2.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                SV2.Columns.Add("DOCUMENT_ID", GetType(Integer))
                SV2.Columns.Add("FILE_ID", GetType(Integer))
                SV2.Columns.Add("BATCH_ID", GetType(Integer))
                SV2.Columns.Add("ISA_ID", GetType(Integer))
                SV2.Columns.Add("GS_ID", GetType(Integer))
                SV2.Columns.Add("ST_ID", GetType(Integer))
                SV2.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                SV2.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                SV2.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                SV2.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                SV2.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                SV2.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                SV2.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                SV2.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                SV2.Columns.Add("837_SBR_GUID", GetType(Guid))
                SV2.Columns.Add("837_CLM_GUID", GetType(Guid))
                SV2.Columns.Add("837_LX_GUID", GetType(Guid))
                SV2.Columns.Add("HL01", GetType(Integer))
                SV2.Columns.Add("HL02", GetType(Integer))
                SV2.Columns.Add("HL03", GetType(Integer))
                SV2.Columns.Add("HL04", GetType(Integer))
                SV2.Columns.Add("SV201", GetType(String))
                SV2.Columns.Add("SV202", GetType(String))
                SV2.Columns.Add("SV202_1", GetType(String))
                SV2.Columns.Add("SV202_2", GetType(String))
                SV2.Columns.Add("SV202_3", GetType(String))
                SV2.Columns.Add("SV202_4", GetType(String))
                SV2.Columns.Add("SV202_5", GetType(String))
                SV2.Columns.Add("SV202_6", GetType(String))
                SV2.Columns.Add("SV202_7", GetType(String))
                SV2.Columns.Add("SV203", GetType(String))
                SV2.Columns.Add("SV204", GetType(String))
                SV2.Columns.Add("SV205", GetType(String))
                SV2.Columns.Add("SV206", GetType(String))
                SV2.Columns.Add("SV207", GetType(String))
                SV2.Columns.Add("ROW_NUMBER", GetType(Integer))
                SV2.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                SV2.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                SV2.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try


            Try
                CACHE_SV2.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_SV2.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_SV2.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_SV2.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_SV2.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_SV2.Columns.Add("GS_ID", GetType(Integer))
                CACHE_SV2.Columns.Add("ST_ID", GetType(Integer))
                CACHE_SV2.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_SV2.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_SV2.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_SV2.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_SV2.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_SV2.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_SV2.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_SV2.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_SV2.Columns.Add("837_SBR_GUID", GetType(Guid))
                CACHE_SV2.Columns.Add("837_CLM_GUID", GetType(Guid))
                CACHE_SV2.Columns.Add("837_LX_GUID", GetType(Guid))
                CACHE_SV2.Columns.Add("HL01", GetType(Integer))
                CACHE_SV2.Columns.Add("HL02", GetType(Integer))
                CACHE_SV2.Columns.Add("HL03", GetType(Integer))
                CACHE_SV2.Columns.Add("HL04", GetType(Integer))
                CACHE_SV2.Columns.Add("SV201", GetType(String))
                CACHE_SV2.Columns.Add("SV202", GetType(String))
                CACHE_SV2.Columns.Add("SV202_1", GetType(String))
                CACHE_SV2.Columns.Add("SV202_2", GetType(String))
                CACHE_SV2.Columns.Add("SV202_3", GetType(String))
                CACHE_SV2.Columns.Add("SV202_4", GetType(String))
                CACHE_SV2.Columns.Add("SV202_5", GetType(String))
                CACHE_SV2.Columns.Add("SV202_6", GetType(String))
                CACHE_SV2.Columns.Add("SV202_7", GetType(String))
                CACHE_SV2.Columns.Add("SV203", GetType(String))
                CACHE_SV2.Columns.Add("SV204", GetType(String))
                CACHE_SV2.Columns.Add("SV205", GetType(String))
                CACHE_SV2.Columns.Add("SV206", GetType(String))
                CACHE_SV2.Columns.Add("SV207", GetType(String))
                CACHE_SV2.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_SV2.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_SV2.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_SV2.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try






            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   SV2
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                SV5.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                SV5.Columns.Add("DOCUMENT_ID", GetType(Integer))
                SV5.Columns.Add("FILE_ID", GetType(Integer))
                SV5.Columns.Add("BATCH_ID", GetType(Integer))
                SV5.Columns.Add("ISA_ID", GetType(Integer))
                SV5.Columns.Add("GS_ID", GetType(Integer))
                SV5.Columns.Add("ST_ID", GetType(Integer))
                SV5.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                SV5.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                SV5.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                SV5.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                SV5.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                SV5.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                SV5.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                SV5.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                SV5.Columns.Add("837_SBR_GUID", GetType(Guid))
                SV5.Columns.Add("837_CLM_GUID", GetType(Guid))
                SV5.Columns.Add("837_LX_GUID", GetType(Guid))
                SV5.Columns.Add("HL01", GetType(Integer))
                SV5.Columns.Add("HL02", GetType(Integer))
                SV5.Columns.Add("HL03", GetType(Integer))
                SV5.Columns.Add("HL04", GetType(Integer))
                SV5.Columns.Add("SV501", GetType(String))
                SV5.Columns.Add("SV501_1", GetType(String))
                SV5.Columns.Add("SV501_2", GetType(String))
                SV5.Columns.Add("SV502", GetType(String))
                SV5.Columns.Add("SV503", GetType(String))
                SV5.Columns.Add("SV504", GetType(String))
                SV5.Columns.Add("SV505", GetType(String))
                SV5.Columns.Add("SV506", GetType(String))
                SV5.Columns.Add("ROW_NUMBER", GetType(Integer))
                SV5.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                SV5.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                SV5.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try


            Try
                CACHE_SV5.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_SV5.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_SV5.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_SV5.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_SV5.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_SV5.Columns.Add("GS_ID", GetType(Integer))
                CACHE_SV5.Columns.Add("ST_ID", GetType(Integer))
                CACHE_SV5.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_SV5.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_SV5.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_SV5.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_SV5.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_SV5.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_SV5.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_SV5.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_SV5.Columns.Add("837_SBR_GUID", GetType(Guid))
                CACHE_SV5.Columns.Add("837_CLM_GUID", GetType(Guid))
                CACHE_SV5.Columns.Add("837_LX_GUID", GetType(Guid))
                CACHE_SV5.Columns.Add("HL01", GetType(Integer))
                CACHE_SV5.Columns.Add("HL02", GetType(Integer))
                CACHE_SV5.Columns.Add("HL03", GetType(Integer))
                CACHE_SV5.Columns.Add("HL04", GetType(Integer))
                CACHE_SV5.Columns.Add("SV501", GetType(String))
                CACHE_SV5.Columns.Add("SV501_1", GetType(String))
                CACHE_SV5.Columns.Add("SV501_2", GetType(String))
                CACHE_SV5.Columns.Add("SV502", GetType(String))
                CACHE_SV5.Columns.Add("SV503", GetType(String))
                CACHE_SV5.Columns.Add("SV504", GetType(String))
                CACHE_SV5.Columns.Add("SV505", GetType(String))
                CACHE_SV5.Columns.Add("SV506", GetType(String))
                CACHE_SV5.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_SV5.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_SV5.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_SV5.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   SVD
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                SVD.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                SVD.Columns.Add("DOCUMENT_ID", GetType(Integer))
                SVD.Columns.Add("FILE_ID", GetType(Integer))
                SVD.Columns.Add("BATCH_ID", GetType(Integer))
                SVD.Columns.Add("ISA_ID", GetType(Integer))
                SVD.Columns.Add("GS_ID", GetType(Integer))
                SVD.Columns.Add("ST_ID", GetType(Integer))
                SVD.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                SVD.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                SVD.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                SVD.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                SVD.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                SVD.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                SVD.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                SVD.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                SVD.Columns.Add("837_SBR_GUID", GetType(Guid))
                SVD.Columns.Add("837_CLM_GUID", GetType(Guid))
                SVD.Columns.Add("837_LX_GUID", GetType(Guid))
                SVD.Columns.Add("837_SVD_GUID", GetType(Guid))
                SVD.Columns.Add("HL01", GetType(Integer))
                SVD.Columns.Add("HL02", GetType(Integer))
                SVD.Columns.Add("HL03", GetType(Integer))
                SVD.Columns.Add("HL04", GetType(Integer))
                SVD.Columns.Add("SVD01", GetType(String))
                SVD.Columns.Add("SVD02", GetType(String))
                SVD.Columns.Add("SVD03", GetType(String))
                SVD.Columns.Add("SVD03_01", GetType(String))
                SVD.Columns.Add("SVD03_02", GetType(String))
                SVD.Columns.Add("SVD03_03", GetType(String))
                SVD.Columns.Add("SVD03_04", GetType(String))
                SVD.Columns.Add("SVD03_05", GetType(String))
                SVD.Columns.Add("SVD03_06", GetType(String))
                SVD.Columns.Add("SVD03_07", GetType(String))
                SVD.Columns.Add("SVD04", GetType(String))
                SVD.Columns.Add("SVD05", GetType(String))
                SVD.Columns.Add("SVD06", GetType(String))
                SVD.Columns.Add("ROW_NUMBER", GetType(Integer))
                SVD.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                SVD.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                SVD.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))

            Catch ex As Exception

            End Try

            Try
                CACHE_SVD.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CACHE_SVD.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CACHE_SVD.Columns.Add("FILE_ID", GetType(Integer))
                CACHE_SVD.Columns.Add("BATCH_ID", GetType(Integer))
                CACHE_SVD.Columns.Add("ISA_ID", GetType(Integer))
                CACHE_SVD.Columns.Add("GS_ID", GetType(Integer))
                CACHE_SVD.Columns.Add("ST_ID", GetType(Integer))
                CACHE_SVD.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CACHE_SVD.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CACHE_SVD.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CACHE_SVD.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                CACHE_SVD.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                CACHE_SVD.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                CACHE_SVD.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                CACHE_SVD.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                CACHE_SVD.Columns.Add("837_SBR_GUID", GetType(Guid))
                CACHE_SVD.Columns.Add("837_CLM_GUID", GetType(Guid))
                CACHE_SVD.Columns.Add("837_LX_GUID", GetType(Guid))
                CACHE_SVD.Columns.Add("HL01", GetType(Integer))
                CACHE_SVD.Columns.Add("HL02", GetType(Integer))
                CACHE_SVD.Columns.Add("HL03", GetType(Integer))
                CACHE_SVD.Columns.Add("HL04", GetType(Integer))
                CACHE_SVD.Columns.Add("SVD01", GetType(String))
                CACHE_SVD.Columns.Add("SVD02", GetType(String))
                CACHE_SVD.Columns.Add("SVD03", GetType(String))
                CACHE_SVD.Columns.Add("SVD03_01", GetType(String))
                CACHE_SVD.Columns.Add("SVD03_02", GetType(String))
                CACHE_SVD.Columns.Add("SVD03_03", GetType(String))
                CACHE_SVD.Columns.Add("SVD03_04", GetType(String))
                CACHE_SVD.Columns.Add("SVD03_05", GetType(String))
                CACHE_SVD.Columns.Add("SVD03_06", GetType(String))
                CACHE_SVD.Columns.Add("SVD03_07", GetType(String))
                CACHE_SVD.Columns.Add("SVD04", GetType(String))
                CACHE_SVD.Columns.Add("SVD05", GetType(String))
                CACHE_SVD.Columns.Add("SVD06", GetType(String))
                CACHE_SVD.Columns.Add("ROW_NUMBER", GetType(Integer))
                CACHE_SVD.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CACHE_SVD.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CACHE_SVD.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))

            Catch ex As Exception

            End Try



        End Sub




    End Class

End Namespace

