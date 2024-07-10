Option Explicit On
Option Strict On
Option Compare Binary





Imports System.Data


Namespace DCSGlobal.EDI


    Public Class EDI_5010_835_005010X221A1_TABLES

        Inherits EDI_5010_COMMON_TABLES


        '******************************************************************************************************************
        '  all vars not declared here are in   EDI_5010_COMMON_DECS   it is inhreted from the tables for this type
        '  if you are going to use it on this class only it goes below if not put it in EDI_5010_COMMON_DECS
        '******************************************************************************************************************


        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   A
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public CLP_AMT As New DataTable
        Public SVC_AMT As New DataTable
        Public M_AMT As New DataTable

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   BPR
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public BPR As New DataTable


        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   C
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public CLP_CAS As New DataTable
        Public SVC_CAS As New DataTable
        Public M_CAS As New DataTable
        Public CLP_CLP As New DataTable



        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   D
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public DTM As New DataTable
        Public CLP_DTM As New DataTable
        Public SVC_DTM As New DataTable
        Public M_DTM As New DataTable
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   H
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public HL As New DataTable





        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   L
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public LX As New DataTable
        Public SVC_LQ As New DataTable


        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   M
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public CLP_MOA As New DataTable
        Public CLP_MIA As New DataTable




        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   N
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public N1 As New DataTable
        Public N3 As New DataTable
        Public N4 As New DataTable
        Public CLP_NM1 As New DataTable


        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   P
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public PER As New DataTable
        Public CLP_PER As New DataTable
        Public PLB As New DataTable



        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   Q
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public CLP_QTY As New DataTable



        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   R
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public REF As New DataTable
        Public CLP_REF As New DataTable
        Public SVC_REF As New DataTable
        Public M_REF As New DataTable


        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   S
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public SVC_SVC As New DataTable



        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   T
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public TRN As New DataTable

        Public TS2 As New DataTable
        Public TS3 As New DataTable



        'end 835 tables






        Public Sub BuildTables()

            BuildCommonTables()

            Try
                'ADD THIS GUID TO THE END OF ST
                ST.Columns.Add("835_TSH_GUID", GetType(Guid))
            Catch ex As Exception

            End Try

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   AMT
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                CLP_AMT.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CLP_AMT.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CLP_AMT.Columns.Add("FILE_ID", GetType(Integer))
                CLP_AMT.Columns.Add("BATCH_ID", GetType(Integer))
                CLP_AMT.Columns.Add("ISA_ID", GetType(Integer))
                CLP_AMT.Columns.Add("GS_ID", GetType(Integer))
                CLP_AMT.Columns.Add("ST_ID", GetType(Integer))
                CLP_AMT.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CLP_AMT.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CLP_AMT.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CLP_AMT.Columns.Add("835_TSH_GUID", GetType(Guid))
                CLP_AMT.Columns.Add("835_PAYOR_GUID", GetType(Guid))
                CLP_AMT.Columns.Add("835_PAYEE_GUID", GetType(Guid))
                CLP_AMT.Columns.Add("835_LX_GUID", GetType(Guid))
                CLP_AMT.Columns.Add("835_CLP_GUID", GetType(Guid))
                CLP_AMT.Columns.Add("835_SVC_GUID", GetType(Guid))
                CLP_AMT.Columns.Add("AMT01", GetType(String))
                CLP_AMT.Columns.Add("AMT02", GetType(String))
                CLP_AMT.Columns.Add("AMT03", GetType(String))
                CLP_AMT.Columns.Add("ROW_NUMBER", GetType(Integer))
                CLP_AMT.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CLP_AMT.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CLP_AMT.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try


            Try
                SVC_AMT.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                SVC_AMT.Columns.Add("DOCUMENT_ID", GetType(Integer))
                SVC_AMT.Columns.Add("FILE_ID", GetType(Integer))
                SVC_AMT.Columns.Add("BATCH_ID", GetType(Integer))
                SVC_AMT.Columns.Add("ISA_ID", GetType(Integer))
                SVC_AMT.Columns.Add("GS_ID", GetType(Integer))
                SVC_AMT.Columns.Add("ST_ID", GetType(Integer))
                SVC_AMT.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                SVC_AMT.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                SVC_AMT.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                SVC_AMT.Columns.Add("835_TSH_GUID", GetType(Guid))
                SVC_AMT.Columns.Add("835_PAYOR_GUID", GetType(Guid))
                SVC_AMT.Columns.Add("835_PAYEE_GUID", GetType(Guid))
                SVC_AMT.Columns.Add("835_LX_GUID", GetType(Guid))
                SVC_AMT.Columns.Add("835_CLP_GUID", GetType(Guid))
                SVC_AMT.Columns.Add("835_SVC_GUID", GetType(Guid))
                SVC_AMT.Columns.Add("AMT01", GetType(String))
                SVC_AMT.Columns.Add("AMT02", GetType(String))
                SVC_AMT.Columns.Add("AMT03", GetType(String))
                SVC_AMT.Columns.Add("ROW_NUMBER", GetType(Integer))
                SVC_AMT.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                SVC_AMT.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                SVC_AMT.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try


            Try
                M_AMT.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                M_AMT.Columns.Add("DOCUMENT_ID", GetType(Integer))
                M_AMT.Columns.Add("FILE_ID", GetType(Integer))
                M_AMT.Columns.Add("BATCH_ID", GetType(Integer))
                M_AMT.Columns.Add("ISA_ID", GetType(Integer))
                M_AMT.Columns.Add("GS_ID", GetType(Integer))
                M_AMT.Columns.Add("ST_ID", GetType(Integer))
                M_AMT.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                M_AMT.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                M_AMT.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                M_AMT.Columns.Add("835_TSH_GUID", GetType(Guid))
                M_AMT.Columns.Add("835_PAYOR_GUID", GetType(Guid))
                M_AMT.Columns.Add("835_PAYEE_GUID", GetType(Guid))
                M_AMT.Columns.Add("835_LX_GUID", GetType(Guid))
                M_AMT.Columns.Add("835_CLP_GUID", GetType(Guid))
                M_AMT.Columns.Add("835_SVC_GUID", GetType(Guid))
                M_AMT.Columns.Add("AMT01", GetType(String))
                M_AMT.Columns.Add("AMT02", GetType(String))
                M_AMT.Columns.Add("AMT03", GetType(String))
                M_AMT.Columns.Add("ROW_NUMBER", GetType(Integer))
                M_AMT.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                M_AMT.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                M_AMT.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   BPR
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                BPR.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                BPR.Columns.Add("DOCUMENT_ID", GetType(Integer))
                BPR.Columns.Add("FILE_ID", GetType(Integer))
                BPR.Columns.Add("BATCH_ID", GetType(Integer))
                BPR.Columns.Add("ISA_ID", GetType(Integer))
                BPR.Columns.Add("GS_ID", GetType(Integer))
                BPR.Columns.Add("ST_ID", GetType(Integer))
                BPR.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                BPR.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                BPR.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                BPR.Columns.Add("835_TSH_GUID", GetType(Guid))
                BPR.Columns.Add("BPR01", GetType(String))
                BPR.Columns.Add("BPR02", GetType(String))
                BPR.Columns.Add("BPR03", GetType(String))
                BPR.Columns.Add("BPR04", GetType(String))
                BPR.Columns.Add("BPR05", GetType(String))
                BPR.Columns.Add("BPR06", GetType(String))
                BPR.Columns.Add("BPR07", GetType(String))
                BPR.Columns.Add("BPR08", GetType(String))
                BPR.Columns.Add("BPR09", GetType(String))
                BPR.Columns.Add("BPR10", GetType(String))
                BPR.Columns.Add("BPR11", GetType(String))
                BPR.Columns.Add("BPR12", GetType(String))
                BPR.Columns.Add("BPR13", GetType(String))
                BPR.Columns.Add("BPR14", GetType(String))
                BPR.Columns.Add("BPR15", GetType(String))
                BPR.Columns.Add("BPR16", GetType(String))
                BPR.Columns.Add("ROW_NUMBER", GetType(Integer))
                BPR.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                BPR.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                BPR.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try



            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   CAS
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try


                CLP_CAS.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CLP_CAS.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CLP_CAS.Columns.Add("FILE_ID", GetType(Integer))
                CLP_CAS.Columns.Add("BATCH_ID", GetType(Integer))
                CLP_CAS.Columns.Add("ISA_ID", GetType(Integer))
                CLP_CAS.Columns.Add("GS_ID", GetType(Integer))
                CLP_CAS.Columns.Add("ST_ID", GetType(Integer))
                CLP_CAS.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CLP_CAS.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CLP_CAS.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CLP_CAS.Columns.Add("835_TSH_GUID", GetType(Guid))
                CLP_CAS.Columns.Add("835_PAYOR_GUID", GetType(Guid))
                CLP_CAS.Columns.Add("835_PAYEE_GUID", GetType(Guid))
                CLP_CAS.Columns.Add("835_LX_GUID", GetType(Guid))
                CLP_CAS.Columns.Add("835_CLP_GUID", GetType(Guid))
                CLP_CAS.Columns.Add("835_SVC_GUID", GetType(Guid))
                CLP_CAS.Columns.Add("CAS01", GetType(String))
                CLP_CAS.Columns.Add("CAS02", GetType(String))
                CLP_CAS.Columns.Add("CAS03", GetType(String))
                CLP_CAS.Columns.Add("CAS04", GetType(String))
                CLP_CAS.Columns.Add("CAS05", GetType(String))
                CLP_CAS.Columns.Add("CAS06", GetType(String))
                CLP_CAS.Columns.Add("CAS07", GetType(String))
                CLP_CAS.Columns.Add("CAS08", GetType(String))
                CLP_CAS.Columns.Add("CAS09", GetType(String))
                CLP_CAS.Columns.Add("CAS10", GetType(String))
                CLP_CAS.Columns.Add("CAS11", GetType(String))
                CLP_CAS.Columns.Add("CAS12", GetType(String))
                CLP_CAS.Columns.Add("CAS13", GetType(String))
                CLP_CAS.Columns.Add("CAS14", GetType(String))
                CLP_CAS.Columns.Add("CAS15", GetType(String))
                CLP_CAS.Columns.Add("CAS16", GetType(String))
                CLP_CAS.Columns.Add("CAS17", GetType(String))
                CLP_CAS.Columns.Add("CAS18", GetType(String))
                CLP_CAS.Columns.Add("CAS19", GetType(String))
                CLP_CAS.Columns.Add("ROW_NUMBER", GetType(Integer))
                CLP_CAS.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CLP_CAS.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CLP_CAS.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try



            Try


                SVC_CAS.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                SVC_CAS.Columns.Add("DOCUMENT_ID", GetType(Integer))
                SVC_CAS.Columns.Add("FILE_ID", GetType(Integer))
                SVC_CAS.Columns.Add("BATCH_ID", GetType(Integer))
                SVC_CAS.Columns.Add("ISA_ID", GetType(Integer))
                SVC_CAS.Columns.Add("GS_ID", GetType(Integer))
                SVC_CAS.Columns.Add("ST_ID", GetType(Integer))
                SVC_CAS.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                SVC_CAS.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                SVC_CAS.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                SVC_CAS.Columns.Add("835_TSH_GUID", GetType(Guid))
                SVC_CAS.Columns.Add("835_PAYOR_GUID", GetType(Guid))
                SVC_CAS.Columns.Add("835_PAYEE_GUID", GetType(Guid))
                SVC_CAS.Columns.Add("835_LX_GUID", GetType(Guid))
                SVC_CAS.Columns.Add("835_CLP_GUID", GetType(Guid))
                SVC_CAS.Columns.Add("835_SVC_GUID", GetType(Guid))
                SVC_CAS.Columns.Add("CAS01", GetType(String))
                SVC_CAS.Columns.Add("CAS02", GetType(String))
                SVC_CAS.Columns.Add("CAS03", GetType(String))
                SVC_CAS.Columns.Add("CAS04", GetType(String))
                SVC_CAS.Columns.Add("CAS05", GetType(String))
                SVC_CAS.Columns.Add("CAS06", GetType(String))
                SVC_CAS.Columns.Add("CAS07", GetType(String))
                SVC_CAS.Columns.Add("CAS08", GetType(String))
                SVC_CAS.Columns.Add("CAS09", GetType(String))
                SVC_CAS.Columns.Add("CAS10", GetType(String))
                SVC_CAS.Columns.Add("CAS11", GetType(String))
                SVC_CAS.Columns.Add("CAS12", GetType(String))
                SVC_CAS.Columns.Add("CAS13", GetType(String))
                SVC_CAS.Columns.Add("CAS14", GetType(String))
                SVC_CAS.Columns.Add("CAS15", GetType(String))
                SVC_CAS.Columns.Add("CAS16", GetType(String))
                SVC_CAS.Columns.Add("CAS17", GetType(String))
                SVC_CAS.Columns.Add("CAS18", GetType(String))
                SVC_CAS.Columns.Add("CAS19", GetType(String))
                SVC_CAS.Columns.Add("ROW_NUMBER", GetType(Integer))
                SVC_CAS.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                SVC_CAS.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                SVC_CAS.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try



            Try


                M_CAS.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                M_CAS.Columns.Add("DOCUMENT_ID", GetType(Integer))
                M_CAS.Columns.Add("FILE_ID", GetType(Integer))
                M_CAS.Columns.Add("BATCH_ID", GetType(Integer))
                M_CAS.Columns.Add("ISA_ID", GetType(Integer))
                M_CAS.Columns.Add("GS_ID", GetType(Integer))
                M_CAS.Columns.Add("ST_ID", GetType(Integer))
                M_CAS.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                M_CAS.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                M_CAS.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                M_CAS.Columns.Add("835_TSH_GUID", GetType(Guid))
                M_CAS.Columns.Add("835_PAYOR_GUID", GetType(Guid))
                M_CAS.Columns.Add("835_PAYEE_GUID", GetType(Guid))
                M_CAS.Columns.Add("835_LX_GUID", GetType(Guid))
                M_CAS.Columns.Add("835_CLP_GUID", GetType(Guid))
                M_CAS.Columns.Add("835_SVC_GUID", GetType(Guid))
                M_CAS.Columns.Add("CAS01", GetType(String))
                M_CAS.Columns.Add("CAS02", GetType(String))
                M_CAS.Columns.Add("CAS03", GetType(String))
                M_CAS.Columns.Add("CAS04", GetType(String))
                M_CAS.Columns.Add("CAS05", GetType(String))
                M_CAS.Columns.Add("CAS06", GetType(String))
                M_CAS.Columns.Add("CAS07", GetType(String))
                M_CAS.Columns.Add("CAS08", GetType(String))
                M_CAS.Columns.Add("CAS09", GetType(String))
                M_CAS.Columns.Add("CAS10", GetType(String))
                M_CAS.Columns.Add("CAS11", GetType(String))
                M_CAS.Columns.Add("CAS12", GetType(String))
                M_CAS.Columns.Add("CAS13", GetType(String))
                M_CAS.Columns.Add("CAS14", GetType(String))
                M_CAS.Columns.Add("CAS15", GetType(String))
                M_CAS.Columns.Add("CAS16", GetType(String))
                M_CAS.Columns.Add("CAS17", GetType(String))
                M_CAS.Columns.Add("CAS18", GetType(String))
                M_CAS.Columns.Add("CAS19", GetType(String))
                M_CAS.Columns.Add("ROW_NUMBER", GetType(Integer))
                M_CAS.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                M_CAS.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                M_CAS.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   CLP
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                CLP_CLP.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CLP_CLP.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CLP_CLP.Columns.Add("FILE_ID", GetType(Integer))
                CLP_CLP.Columns.Add("BATCH_ID", GetType(Integer))
                CLP_CLP.Columns.Add("ISA_ID", GetType(Integer))
                CLP_CLP.Columns.Add("GS_ID", GetType(Integer))
                CLP_CLP.Columns.Add("ST_ID", GetType(Integer))
                CLP_CLP.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CLP_CLP.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CLP_CLP.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CLP_CLP.Columns.Add("835_TSH_GUID", GetType(Guid))
                CLP_CLP.Columns.Add("835_PAYOR_GUID", GetType(Guid))
                CLP_CLP.Columns.Add("835_PAYEE_GUID", GetType(Guid))
                CLP_CLP.Columns.Add("835_LX_GUID", GetType(Guid))
                CLP_CLP.Columns.Add("835_CLP_GUID", GetType(Guid))
                CLP_CLP.Columns.Add("835_SVC_GUID", GetType(Guid))
                CLP_CLP.Columns.Add("CLP01", GetType(String))
                CLP_CLP.Columns.Add("CLP02", GetType(String))
                CLP_CLP.Columns.Add("CLP03", GetType(String))
                CLP_CLP.Columns.Add("CLP04", GetType(String))
                CLP_CLP.Columns.Add("CLP05", GetType(String))
                CLP_CLP.Columns.Add("CLP06", GetType(String))
                CLP_CLP.Columns.Add("CLP07", GetType(String))
                CLP_CLP.Columns.Add("CLP08", GetType(String))
                CLP_CLP.Columns.Add("CLP09", GetType(String))
                CLP_CLP.Columns.Add("CLP10", GetType(String))
                CLP_CLP.Columns.Add("CLP11", GetType(String))
                CLP_CLP.Columns.Add("CLP12", GetType(String))
                CLP_CLP.Columns.Add("CLP13", GetType(String))
                CLP_CLP.Columns.Add("ROW_NUMBER", GetType(Integer))
                CLP_CLP.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CLP_CLP.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CLP_CLP.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try



            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   DTM
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                DTM.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                DTM.Columns.Add("DOCUMENT_ID", GetType(Integer))
                DTM.Columns.Add("FILE_ID", GetType(Integer))
                DTM.Columns.Add("BATCH_ID", GetType(Integer))
                DTM.Columns.Add("ISA_ID", GetType(Integer))
                DTM.Columns.Add("GS_ID", GetType(Integer))
                DTM.Columns.Add("ST_ID", GetType(Integer))
                DTM.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                DTM.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                DTM.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                DTM.Columns.Add("835_TSH_GUID", GetType(Guid))
                DTM.Columns.Add("835_PAYOR_GUID", GetType(Guid))
                DTM.Columns.Add("835_PAYEE_GUID", GetType(Guid))
                DTM.Columns.Add("835_LX_GUID", GetType(Guid))
                DTM.Columns.Add("835_CLP_GUID", GetType(Guid))
                DTM.Columns.Add("835_SVC_GUID", GetType(Guid))
                DTM.Columns.Add("DTM01", GetType(String))
                DTM.Columns.Add("DTM02", GetType(String))
                DTM.Columns.Add("ROW_NUMBER", GetType(Integer))
                DTM.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                DTM.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                DTM.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try



            Try
                CLP_DTM.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CLP_DTM.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CLP_DTM.Columns.Add("FILE_ID", GetType(Integer))
                CLP_DTM.Columns.Add("BATCH_ID", GetType(Integer))
                CLP_DTM.Columns.Add("ISA_ID", GetType(Integer))
                CLP_DTM.Columns.Add("GS_ID", GetType(Integer))
                CLP_DTM.Columns.Add("ST_ID", GetType(Integer))
                CLP_DTM.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CLP_DTM.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CLP_DTM.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CLP_DTM.Columns.Add("835_TSH_GUID", GetType(Guid))
                CLP_DTM.Columns.Add("835_PAYOR_GUID", GetType(Guid))
                CLP_DTM.Columns.Add("835_PAYEE_GUID", GetType(Guid))
                CLP_DTM.Columns.Add("835_LX_GUID", GetType(Guid))
                CLP_DTM.Columns.Add("835_CLP_GUID", GetType(Guid))
                CLP_DTM.Columns.Add("835_SVC_GUID", GetType(Guid))
                CLP_DTM.Columns.Add("DTM01", GetType(String))
                CLP_DTM.Columns.Add("DTM02", GetType(String))
                CLP_DTM.Columns.Add("ROW_NUMBER", GetType(Integer))
                CLP_DTM.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CLP_DTM.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CLP_DTM.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try





            Try
                SVC_DTM.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                SVC_DTM.Columns.Add("DOCUMENT_ID", GetType(Integer))
                SVC_DTM.Columns.Add("FILE_ID", GetType(Integer))
                SVC_DTM.Columns.Add("BATCH_ID", GetType(Integer))
                SVC_DTM.Columns.Add("ISA_ID", GetType(Integer))
                SVC_DTM.Columns.Add("GS_ID", GetType(Integer))
                SVC_DTM.Columns.Add("ST_ID", GetType(Integer))
                SVC_DTM.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                SVC_DTM.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                SVC_DTM.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                SVC_DTM.Columns.Add("835_TSH_GUID", GetType(Guid))
                SVC_DTM.Columns.Add("835_PAYOR_GUID", GetType(Guid))
                SVC_DTM.Columns.Add("835_PAYEE_GUID", GetType(Guid))
                SVC_DTM.Columns.Add("835_LX_GUID", GetType(Guid))
                SVC_DTM.Columns.Add("835_CLP_GUID", GetType(Guid))
                SVC_DTM.Columns.Add("835_SVC_GUID", GetType(Guid))
                SVC_DTM.Columns.Add("DTM01", GetType(String))
                SVC_DTM.Columns.Add("DTM02", GetType(String))
                SVC_DTM.Columns.Add("ROW_NUMBER", GetType(Integer))
                SVC_DTM.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                SVC_DTM.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                SVC_DTM.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try


            Try
                M_DTM.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                M_DTM.Columns.Add("DOCUMENT_ID", GetType(Integer))
                M_DTM.Columns.Add("FILE_ID", GetType(Integer))
                M_DTM.Columns.Add("BATCH_ID", GetType(Integer))
                M_DTM.Columns.Add("ISA_ID", GetType(Integer))
                M_DTM.Columns.Add("GS_ID", GetType(Integer))
                M_DTM.Columns.Add("ST_ID", GetType(Integer))
                M_DTM.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                M_DTM.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                M_DTM.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                M_DTM.Columns.Add("835_TSH_GUID", GetType(Guid))
                M_DTM.Columns.Add("835_PAYOR_GUID", GetType(Guid))
                M_DTM.Columns.Add("835_PAYEE_GUID", GetType(Guid))
                M_DTM.Columns.Add("835_LX_GUID", GetType(Guid))
                M_DTM.Columns.Add("835_CLP_GUID", GetType(Guid))
                M_DTM.Columns.Add("835_SVC_GUID", GetType(Guid))
                M_DTM.Columns.Add("DTM01", GetType(String))
                M_DTM.Columns.Add("DTM02", GetType(String))
                M_DTM.Columns.Add("ROW_NUMBER", GetType(Integer))
                M_DTM.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                M_DTM.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                M_DTM.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   LQ
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                SVC_LQ.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                SVC_LQ.Columns.Add("DOCUMENT_ID", GetType(Integer))
                SVC_LQ.Columns.Add("FILE_ID", GetType(Integer))
                SVC_LQ.Columns.Add("BATCH_ID", GetType(Integer))
                SVC_LQ.Columns.Add("ISA_ID", GetType(Integer))
                SVC_LQ.Columns.Add("GS_ID", GetType(Integer))
                SVC_LQ.Columns.Add("ST_ID", GetType(Integer))
                SVC_LQ.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                SVC_LQ.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                SVC_LQ.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                SVC_LQ.Columns.Add("835_TSH_GUID", GetType(Guid))
                SVC_LQ.Columns.Add("835_PAYOR_GUID", GetType(Guid))
                SVC_LQ.Columns.Add("835_PAYEE_GUID", GetType(Guid))
                SVC_LQ.Columns.Add("835_LX_GUID", GetType(Guid))
                SVC_LQ.Columns.Add("835_CLP_GUID", GetType(Guid))
                SVC_LQ.Columns.Add("835_SVC_GUID", GetType(Guid))
                SVC_LQ.Columns.Add("LQ02", GetType(String))
                SVC_LQ.Columns.Add("LQ01", GetType(String))
                SVC_LQ.Columns.Add("ROW_NUMBER", GetType(Integer))
                SVC_LQ.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                SVC_LQ.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                SVC_LQ.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
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
                LX.Columns.Add("835_TSH_GUID", GetType(Guid))
                LX.Columns.Add("835_PAYOR_GUID", GetType(Guid))
                LX.Columns.Add("835_PAYEE_GUID", GetType(Guid))
                LX.Columns.Add("835_LX_GUID", GetType(Guid))
                LX.Columns.Add("835_CLP_GUID", GetType(Guid))
                LX.Columns.Add("835_SVC_GUID", GetType(Guid))
                LX.Columns.Add("LX01", GetType(String))
                LX.Columns.Add("ROW_NUMBER", GetType(Integer))
                LX.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                LX.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                LX.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try







            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   MIA
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                CLP_MIA.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CLP_MIA.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CLP_MIA.Columns.Add("FILE_ID", GetType(Integer))
                CLP_MIA.Columns.Add("BATCH_ID", GetType(Integer))
                CLP_MIA.Columns.Add("ISA_ID", GetType(Integer))
                CLP_MIA.Columns.Add("GS_ID", GetType(Integer))
                CLP_MIA.Columns.Add("ST_ID", GetType(Integer))
                CLP_MIA.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CLP_MIA.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CLP_MIA.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CLP_MIA.Columns.Add("835_TSH_GUID", GetType(Guid))
                CLP_MIA.Columns.Add("835_PAYOR_GUID", GetType(Guid))
                CLP_MIA.Columns.Add("835_PAYEE_GUID", GetType(Guid))
                CLP_MIA.Columns.Add("835_LX_GUID", GetType(Guid))
                CLP_MIA.Columns.Add("835_CLP_GUID", GetType(Guid))
                CLP_MIA.Columns.Add("835_SVC_GUID", GetType(Guid))
                CLP_MIA.Columns.Add("MIA01", GetType(String))
                CLP_MIA.Columns.Add("MIA02", GetType(String))
                CLP_MIA.Columns.Add("MIA03", GetType(String))
                CLP_MIA.Columns.Add("MIA04", GetType(String))
                CLP_MIA.Columns.Add("MIA05", GetType(String))
                CLP_MIA.Columns.Add("MIA06", GetType(String))
                CLP_MIA.Columns.Add("MIA07", GetType(String))
                CLP_MIA.Columns.Add("MIA08", GetType(String))
                CLP_MIA.Columns.Add("MIA09", GetType(String))
                CLP_MIA.Columns.Add("MIA10", GetType(String))
                CLP_MIA.Columns.Add("MIA11", GetType(String))
                CLP_MIA.Columns.Add("MIA12", GetType(String))
                CLP_MIA.Columns.Add("MIA13", GetType(String))
                CLP_MIA.Columns.Add("MIA14", GetType(String))
                CLP_MIA.Columns.Add("MIA15", GetType(String))
                CLP_MIA.Columns.Add("MIA16", GetType(String))
                CLP_MIA.Columns.Add("MIA17", GetType(String))
                CLP_MIA.Columns.Add("MIA18", GetType(String))
                CLP_MIA.Columns.Add("MIA19", GetType(String))
                CLP_MIA.Columns.Add("MIA20", GetType(String))
                CLP_MIA.Columns.Add("MIA21", GetType(String))
                CLP_MIA.Columns.Add("MIA22", GetType(String))
                CLP_MIA.Columns.Add("MIA23", GetType(String))
                CLP_MIA.Columns.Add("MIA24", GetType(String))
                CLP_MIA.Columns.Add("ROW_NUMBER", GetType(Integer))
                CLP_MIA.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CLP_MIA.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CLP_MIA.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception


            End Try



            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   MOA
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                CLP_MOA.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CLP_MOA.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CLP_MOA.Columns.Add("FILE_ID", GetType(Integer))
                CLP_MOA.Columns.Add("BATCH_ID", GetType(Integer))
                CLP_MOA.Columns.Add("ISA_ID", GetType(Integer))
                CLP_MOA.Columns.Add("GS_ID", GetType(Integer))
                CLP_MOA.Columns.Add("ST_ID", GetType(Integer))
                CLP_MOA.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CLP_MOA.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CLP_MOA.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CLP_MOA.Columns.Add("835_TSH_GUID", GetType(Guid))
                CLP_MOA.Columns.Add("835_PAYOR_GUID", GetType(Guid))
                CLP_MOA.Columns.Add("835_PAYEE_GUID", GetType(Guid))
                CLP_MOA.Columns.Add("835_LX_GUID", GetType(Guid))
                CLP_MOA.Columns.Add("835_CLP_GUID", GetType(Guid))
                CLP_MOA.Columns.Add("835_SVC_GUID", GetType(Guid))
                CLP_MOA.Columns.Add("MOA01", GetType(String))
                CLP_MOA.Columns.Add("MOA02", GetType(String))
                CLP_MOA.Columns.Add("MOA03", GetType(String))
                CLP_MOA.Columns.Add("MOA04", GetType(String))
                CLP_MOA.Columns.Add("MOA05", GetType(String))
                CLP_MOA.Columns.Add("MOA06", GetType(String))
                CLP_MOA.Columns.Add("MOA07", GetType(String))
                CLP_MOA.Columns.Add("MOA08", GetType(String))
                CLP_MOA.Columns.Add("MOA09", GetType(String))
                CLP_MOA.Columns.Add("ROW_NUMBER", GetType(Integer))
                CLP_MOA.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CLP_MOA.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CLP_MOA.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception


            End Try




            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   N1
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                N1.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                N1.Columns.Add("DOCUMENT_ID", GetType(Integer))
                N1.Columns.Add("FILE_ID", GetType(Integer))
                N1.Columns.Add("BATCH_ID", GetType(Integer))
                N1.Columns.Add("ISA_ID", GetType(Integer))
                N1.Columns.Add("GS_ID", GetType(Integer))
                N1.Columns.Add("ST_ID", GetType(Integer))
                N1.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                N1.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                N1.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                N1.Columns.Add("835_TSH_GUID", GetType(Guid))
                N1.Columns.Add("835_PAYOR_GUID", GetType(Guid))
                N1.Columns.Add("835_PAYEE_GUID", GetType(Guid))
                N1.Columns.Add("835_LX_GUID", GetType(Guid))
                N1.Columns.Add("835_CLP_GUID", GetType(Guid))
                N1.Columns.Add("835_SVC_GUID", GetType(Guid))
                N1.Columns.Add("N101", GetType(String))
                N1.Columns.Add("N102", GetType(String))
                N1.Columns.Add("N103", GetType(String))
                N1.Columns.Add("N104", GetType(String))
                N1.Columns.Add("ROW_NUMBER", GetType(Integer))
                N1.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                N1.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                N1.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
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
                N3.Columns.Add("835_TSH_GUID", GetType(Guid))
                N3.Columns.Add("835_PAYOR_GUID", GetType(Guid))
                N3.Columns.Add("835_PAYEE_GUID", GetType(Guid))
                N3.Columns.Add("835_LX_GUID", GetType(Guid))
                N3.Columns.Add("835_CLP_GUID", GetType(Guid))
                N3.Columns.Add("835_SVC_GUID", GetType(Guid))
                N3.Columns.Add("N301", GetType(String))
                N3.Columns.Add("N302", GetType(String))
                N3.Columns.Add("ROW_NUMBER", GetType(Integer))
                N3.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                N3.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                N3.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
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
                N4.Columns.Add("835_TSH_GUID", GetType(Guid))
                N4.Columns.Add("835_PAYOR_GUID", GetType(Guid))
                N4.Columns.Add("835_PAYEE_GUID", GetType(Guid))
                N4.Columns.Add("835_LX_GUID", GetType(Guid))
                N4.Columns.Add("835_CLP_GUID", GetType(Guid))
                N4.Columns.Add("835_SVC_GUID", GetType(Guid))
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



            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   NM1
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                CLP_NM1.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CLP_NM1.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CLP_NM1.Columns.Add("FILE_ID", GetType(Integer))
                CLP_NM1.Columns.Add("BATCH_ID", GetType(Integer))
                CLP_NM1.Columns.Add("ISA_ID", GetType(Integer))
                CLP_NM1.Columns.Add("GS_ID", GetType(Integer))
                CLP_NM1.Columns.Add("ST_ID", GetType(Integer))
                CLP_NM1.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CLP_NM1.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CLP_NM1.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CLP_NM1.Columns.Add("835_TSH_GUID", GetType(Guid))
                CLP_NM1.Columns.Add("835_PAYOR_GUID", GetType(Guid))
                CLP_NM1.Columns.Add("835_PAYEE_GUID", GetType(Guid))
                CLP_NM1.Columns.Add("835_LX_GUID", GetType(Guid))
                CLP_NM1.Columns.Add("835_CLP_GUID", GetType(Guid))
                CLP_NM1.Columns.Add("835_SVC_GUID", GetType(Guid))
                CLP_NM1.Columns.Add("NM101", GetType(String))
                CLP_NM1.Columns.Add("NM102", GetType(String))
                CLP_NM1.Columns.Add("NM103", GetType(String))
                CLP_NM1.Columns.Add("NM104", GetType(String))
                CLP_NM1.Columns.Add("NM105", GetType(String))
                CLP_NM1.Columns.Add("NM106", GetType(String))
                CLP_NM1.Columns.Add("NM107", GetType(String))
                CLP_NM1.Columns.Add("NM108", GetType(String))
                CLP_NM1.Columns.Add("NM109", GetType(String))
                CLP_NM1.Columns.Add("NM110", GetType(String))
                CLP_NM1.Columns.Add("NM111", GetType(String))
                CLP_NM1.Columns.Add("ROW_NUMBER", GetType(Integer))
                CLP_NM1.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CLP_NM1.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CLP_NM1.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
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
                PER.Columns.Add("835_TSH_GUID", GetType(Guid))
                PER.Columns.Add("835_PAYOR_GUID", GetType(Guid))
                PER.Columns.Add("835_PAYEE_GUID", GetType(Guid))
                PER.Columns.Add("835_LX_GUID", GetType(Guid))
                PER.Columns.Add("835_CLP_GUID", GetType(Guid))
                PER.Columns.Add("835_SVC_GUID", GetType(Guid))
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
                CLP_PER.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CLP_PER.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CLP_PER.Columns.Add("FILE_ID", GetType(Integer))
                CLP_PER.Columns.Add("BATCH_ID", GetType(Integer))
                CLP_PER.Columns.Add("ISA_ID", GetType(Integer))
                CLP_PER.Columns.Add("GS_ID", GetType(Integer))
                CLP_PER.Columns.Add("ST_ID", GetType(Integer))
                CLP_PER.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CLP_PER.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CLP_PER.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CLP_PER.Columns.Add("835_TSH_GUID", GetType(Guid))
                CLP_PER.Columns.Add("835_PAYOR_GUID", GetType(Guid))
                CLP_PER.Columns.Add("835_PAYEE_GUID", GetType(Guid))
                CLP_PER.Columns.Add("835_LX_GUID", GetType(Guid))
                CLP_PER.Columns.Add("835_CLP_GUID", GetType(Guid))
                CLP_PER.Columns.Add("835_SVC_GUID", GetType(Guid))
                CLP_PER.Columns.Add("PER01", GetType(String))
                CLP_PER.Columns.Add("PER02", GetType(String))
                CLP_PER.Columns.Add("PER03", GetType(String))
                CLP_PER.Columns.Add("PER04", GetType(String))
                CLP_PER.Columns.Add("PER05", GetType(String))
                CLP_PER.Columns.Add("PER06", GetType(String))
                CLP_PER.Columns.Add("PER07", GetType(String))
                CLP_PER.Columns.Add("PER08", GetType(String))
                CLP_PER.Columns.Add("PER09", GetType(String))
                CLP_PER.Columns.Add("ROW_NUMBER", GetType(Integer))
                CLP_PER.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CLP_PER.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CLP_PER.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   PLB
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                PLB.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                PLB.Columns.Add("DOCUMENT_ID", GetType(Integer))
                PLB.Columns.Add("FILE_ID", GetType(Integer))
                PLB.Columns.Add("BATCH_ID", GetType(Integer))
                PLB.Columns.Add("ISA_ID", GetType(Integer))
                PLB.Columns.Add("GS_ID", GetType(Integer))
                PLB.Columns.Add("ST_ID", GetType(Integer))
                PLB.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                PLB.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                PLB.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                PLB.Columns.Add("835_TSH_GUID", GetType(Guid))
                PLB.Columns.Add("PLB01", GetType(String))
                PLB.Columns.Add("PLB02", GetType(String))
                PLB.Columns.Add("PLB03", GetType(String))
                PLB.Columns.Add("PLB03_1", GetType(String))
                PLB.Columns.Add("PLB03_2", GetType(String))
                PLB.Columns.Add("PLB04", GetType(String))
                PLB.Columns.Add("PLB05", GetType(String))
                PLB.Columns.Add("PLB05_1", GetType(String))
                PLB.Columns.Add("PLB05_2", GetType(String))
                PLB.Columns.Add("PLB06", GetType(String))
                PLB.Columns.Add("PLB07", GetType(String))
                PLB.Columns.Add("PLB07_1", GetType(String))
                PLB.Columns.Add("PLB07_2", GetType(String))
                PLB.Columns.Add("PLB08", GetType(String))
                PLB.Columns.Add("PLB09", GetType(String))
                PLB.Columns.Add("PLB09_1", GetType(String))
                PLB.Columns.Add("PLB09_2", GetType(String))
                PLB.Columns.Add("PLB10", GetType(String))
                PLB.Columns.Add("PLB11", GetType(String))
                PLB.Columns.Add("PLB11_1", GetType(String))
                PLB.Columns.Add("PLB11_2", GetType(String))
                PLB.Columns.Add("PLB12", GetType(String))
                PLB.Columns.Add("PLB13", GetType(String))
                PLB.Columns.Add("PLB13_1", GetType(String))
                PLB.Columns.Add("PLB13_2", GetType(String))
                PLB.Columns.Add("PLB14", GetType(String))
                PLB.Columns.Add("ROW_NUMBER", GetType(Integer))
                PLB.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                PLB.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                PLB.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   QTY
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                CLP_QTY.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CLP_QTY.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CLP_QTY.Columns.Add("FILE_ID", GetType(Integer))
                CLP_QTY.Columns.Add("BATCH_ID", GetType(Integer))
                CLP_QTY.Columns.Add("ISA_ID", GetType(Integer))
                CLP_QTY.Columns.Add("GS_ID", GetType(Integer))
                CLP_QTY.Columns.Add("ST_ID", GetType(Integer))
                CLP_QTY.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CLP_QTY.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CLP_QTY.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CLP_QTY.Columns.Add("835_TSH_GUID", GetType(Guid))
                CLP_QTY.Columns.Add("835_PAYOR_GUID", GetType(Guid))
                CLP_QTY.Columns.Add("835_PAYEE_GUID", GetType(Guid))
                CLP_QTY.Columns.Add("835_LX_GUID", GetType(Guid))
                CLP_QTY.Columns.Add("835_CLP_GUID", GetType(Guid))
                CLP_QTY.Columns.Add("835_SVC_GUID", GetType(Guid))
                CLP_QTY.Columns.Add("QTY01", GetType(String))
                CLP_QTY.Columns.Add("QTY02", GetType(String))
                CLP_QTY.Columns.Add("ROW_NUMBER", GetType(Integer))
                CLP_QTY.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CLP_QTY.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CLP_QTY.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
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
                REF.Columns.Add("835_TSH_GUID", GetType(Guid))
                REF.Columns.Add("835_PAYOR_GUID", GetType(Guid))
                REF.Columns.Add("835_PAYEE_GUID", GetType(Guid))
                REF.Columns.Add("835_LX_GUID", GetType(Guid))
                REF.Columns.Add("835_CLP_GUID", GetType(Guid))
                REF.Columns.Add("835_SVC_GUID", GetType(Guid))
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
                CLP_REF.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                CLP_REF.Columns.Add("DOCUMENT_ID", GetType(Integer))
                CLP_REF.Columns.Add("FILE_ID", GetType(Integer))
                CLP_REF.Columns.Add("BATCH_ID", GetType(Integer))
                CLP_REF.Columns.Add("ISA_ID", GetType(Integer))
                CLP_REF.Columns.Add("GS_ID", GetType(Integer))
                CLP_REF.Columns.Add("ST_ID", GetType(Integer))
                CLP_REF.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                CLP_REF.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                CLP_REF.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                CLP_REF.Columns.Add("835_TSH_GUID", GetType(Guid))
                CLP_REF.Columns.Add("835_PAYOR_GUID", GetType(Guid))
                CLP_REF.Columns.Add("835_PAYEE_GUID", GetType(Guid))
                CLP_REF.Columns.Add("835_LX_GUID", GetType(Guid))
                CLP_REF.Columns.Add("835_CLP_GUID", GetType(Guid))
                CLP_REF.Columns.Add("835_SVC_GUID", GetType(Guid))
                CLP_REF.Columns.Add("REF01", GetType(String))
                CLP_REF.Columns.Add("REF02", GetType(String))
                CLP_REF.Columns.Add("REF03", GetType(String))
                CLP_REF.Columns.Add("ROW_NUMBER", GetType(Integer))
                CLP_REF.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                CLP_REF.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                CLP_REF.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try


            Try
                SVC_REF.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                SVC_REF.Columns.Add("DOCUMENT_ID", GetType(Integer))
                SVC_REF.Columns.Add("FILE_ID", GetType(Integer))
                SVC_REF.Columns.Add("BATCH_ID", GetType(Integer))
                SVC_REF.Columns.Add("ISA_ID", GetType(Integer))
                SVC_REF.Columns.Add("GS_ID", GetType(Integer))
                SVC_REF.Columns.Add("ST_ID", GetType(Integer))
                SVC_REF.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                SVC_REF.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                SVC_REF.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                SVC_REF.Columns.Add("835_TSH_GUID", GetType(Guid))
                SVC_REF.Columns.Add("835_PAYOR_GUID", GetType(Guid))
                SVC_REF.Columns.Add("835_PAYEE_GUID", GetType(Guid))
                SVC_REF.Columns.Add("835_LX_GUID", GetType(Guid))
                SVC_REF.Columns.Add("835_CLP_GUID", GetType(Guid))
                SVC_REF.Columns.Add("835_SVC_GUID", GetType(Guid))
                SVC_REF.Columns.Add("REF01", GetType(String))
                SVC_REF.Columns.Add("REF02", GetType(String))
                SVC_REF.Columns.Add("REF03", GetType(String))
                SVC_REF.Columns.Add("ROW_NUMBER", GetType(Integer))
                SVC_REF.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                SVC_REF.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                SVC_REF.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try

            Try
                M_REF.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                M_REF.Columns.Add("DOCUMENT_ID", GetType(Integer))
                M_REF.Columns.Add("FILE_ID", GetType(Integer))
                M_REF.Columns.Add("BATCH_ID", GetType(Integer))
                M_REF.Columns.Add("ISA_ID", GetType(Integer))
                M_REF.Columns.Add("GS_ID", GetType(Integer))
                M_REF.Columns.Add("ST_ID", GetType(Integer))
                M_REF.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                M_REF.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                M_REF.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                M_REF.Columns.Add("835_TSH_GUID", GetType(Guid))
                M_REF.Columns.Add("835_PAYOR_GUID", GetType(Guid))
                M_REF.Columns.Add("835_PAYEE_GUID", GetType(Guid))
                M_REF.Columns.Add("835_LX_GUID", GetType(Guid))
                M_REF.Columns.Add("835_CLP_GUID", GetType(Guid))
                M_REF.Columns.Add("835_SVC_GUID", GetType(Guid))
                M_REF.Columns.Add("REF01", GetType(String))
                M_REF.Columns.Add("REF02", GetType(String))
                M_REF.Columns.Add("REF03", GetType(String))
                M_REF.Columns.Add("ROW_NUMBER", GetType(Integer))
                M_REF.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                M_REF.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                M_REF.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try



            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   SVC
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                SVC_SVC.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                SVC_SVC.Columns.Add("DOCUMENT_ID", GetType(Integer))
                SVC_SVC.Columns.Add("FILE_ID", GetType(Integer))
                SVC_SVC.Columns.Add("BATCH_ID", GetType(Integer))
                SVC_SVC.Columns.Add("ISA_ID", GetType(Integer))
                SVC_SVC.Columns.Add("GS_ID", GetType(Integer))
                SVC_SVC.Columns.Add("ST_ID", GetType(Integer))
                SVC_SVC.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                SVC_SVC.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                SVC_SVC.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                SVC_SVC.Columns.Add("835_TSH_GUID", GetType(Guid))
                SVC_SVC.Columns.Add("835_PAYOR_GUID", GetType(Guid))
                SVC_SVC.Columns.Add("835_PAYEE_GUID", GetType(Guid))
                SVC_SVC.Columns.Add("835_LX_GUID", GetType(Guid))
                SVC_SVC.Columns.Add("835_CLP_GUID", GetType(Guid))
                SVC_SVC.Columns.Add("835_SVC_GUID", GetType(Guid))
                SVC_SVC.Columns.Add("SVC01", GetType(String))
                SVC_SVC.Columns.Add("SVC01_1", GetType(String))
                SVC_SVC.Columns.Add("SVC01_2", GetType(String))
                SVC_SVC.Columns.Add("SVC01_3", GetType(String))
                SVC_SVC.Columns.Add("SVC01_4", GetType(String))
                SVC_SVC.Columns.Add("SVC01_5", GetType(String))
                SVC_SVC.Columns.Add("SVC01_6", GetType(String))
                SVC_SVC.Columns.Add("SVC01_7", GetType(String))
                SVC_SVC.Columns.Add("SVC02", GetType(String))
                SVC_SVC.Columns.Add("SVC03", GetType(String))
                SVC_SVC.Columns.Add("SVC04", GetType(String))
                SVC_SVC.Columns.Add("SVC05", GetType(String))
                SVC_SVC.Columns.Add("SVC06", GetType(String))
                SVC_SVC.Columns.Add("SVC06_1", GetType(String))
                SVC_SVC.Columns.Add("SVC06_2", GetType(String))
                SVC_SVC.Columns.Add("SVC06_3", GetType(String))
                SVC_SVC.Columns.Add("SVC06_4", GetType(String))
                SVC_SVC.Columns.Add("SVC06_5", GetType(String))
                SVC_SVC.Columns.Add("SVC06_6", GetType(String))
                SVC_SVC.Columns.Add("SVC06_7", GetType(String))
                SVC_SVC.Columns.Add("SVC07", GetType(String))
                SVC_SVC.Columns.Add("ROW_NUMBER", GetType(Integer))
                SVC_SVC.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                SVC_SVC.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                SVC_SVC.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
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
                TRN.Columns.Add("835_TSH_GUID", GetType(Guid))
                TRN.Columns.Add("835_PAYOR_GUID", GetType(Guid))
                TRN.Columns.Add("835_PAYEE_GUID", GetType(Guid))
                TRN.Columns.Add("835_LX_GUID", GetType(Guid))
                TRN.Columns.Add("835_CLP_GUID", GetType(Guid))
                TRN.Columns.Add("835_SVC_GUID", GetType(Guid))
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



            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   TS2
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                TS2.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                TS2.Columns.Add("DOCUMENT_ID", GetType(Integer))
                TS2.Columns.Add("FILE_ID", GetType(Integer))
                TS2.Columns.Add("BATCH_ID", GetType(Integer))
                TS2.Columns.Add("ISA_ID", GetType(Integer))
                TS2.Columns.Add("GS_ID", GetType(Integer))
                TS2.Columns.Add("ST_ID", GetType(Integer))
                TS2.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                TS2.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                TS2.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                TS2.Columns.Add("835_TSH_GUID", GetType(Guid))
                TS2.Columns.Add("835_PAYOR_GUID", GetType(Guid))
                TS2.Columns.Add("835_PAYEE_GUID", GetType(Guid))
                TS2.Columns.Add("835_LX_GUID", GetType(Guid))
                TS2.Columns.Add("835_CLP_GUID", GetType(Guid))
                TS2.Columns.Add("835_SVC_GUID", GetType(Guid))
                TS2.Columns.Add("TS201", GetType(String))
                TS2.Columns.Add("TS202", GetType(String))
                TS2.Columns.Add("TS203", GetType(String))
                TS2.Columns.Add("TS204", GetType(String))
                TS2.Columns.Add("TS205", GetType(String))
                TS2.Columns.Add("TS206", GetType(String))
                TS2.Columns.Add("TS207", GetType(String))
                TS2.Columns.Add("TS208", GetType(String))
                TS2.Columns.Add("TS209", GetType(String))
                TS2.Columns.Add("TS210", GetType(String))
                TS2.Columns.Add("TS211", GetType(String))
                TS2.Columns.Add("TS212", GetType(String))
                TS2.Columns.Add("TS213", GetType(String))
                TS2.Columns.Add("TS214", GetType(String))
                TS2.Columns.Add("TS215", GetType(String))
                TS2.Columns.Add("TS216", GetType(String))
                TS2.Columns.Add("TS217", GetType(String))
                TS2.Columns.Add("TS218", GetType(String))
                TS2.Columns.Add("TS219", GetType(String))
                TS2.Columns.Add("TS220", GetType(String))
                TS2.Columns.Add("TS221", GetType(String))
                TS2.Columns.Add("TS222", GetType(String))
                TS2.Columns.Add("TS223", GetType(String))
                TS2.Columns.Add("TS224", GetType(String))
                TS2.Columns.Add("ROW_NUMBER", GetType(Integer))
                TS2.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                TS2.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                TS2.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try




            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   TS3
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                TS3.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                TS3.Columns.Add("DOCUMENT_ID", GetType(Integer))
                TS3.Columns.Add("FILE_ID", GetType(Integer))
                TS3.Columns.Add("BATCH_ID", GetType(Integer))
                TS3.Columns.Add("ISA_ID", GetType(Integer))
                TS3.Columns.Add("GS_ID", GetType(Integer))
                TS3.Columns.Add("ST_ID", GetType(Integer))
                TS3.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                TS3.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                TS3.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                TS3.Columns.Add("835_TSH_GUID", GetType(Guid))
                TS3.Columns.Add("835_PAYOR_GUID", GetType(Guid))
                TS3.Columns.Add("835_PAYEE_GUID", GetType(Guid))
                TS3.Columns.Add("835_LX_GUID", GetType(Guid))
                TS3.Columns.Add("835_CLP_GUID", GetType(Guid))
                TS3.Columns.Add("835_SVC_GUID", GetType(Guid))
                TS3.Columns.Add("TS301", GetType(String))
                TS3.Columns.Add("TS302", GetType(String))
                TS3.Columns.Add("TS303", GetType(String))
                TS3.Columns.Add("TS304", GetType(String))
                TS3.Columns.Add("TS305", GetType(String))
                TS3.Columns.Add("TS306", GetType(String))
                TS3.Columns.Add("TS307", GetType(String))
                TS3.Columns.Add("TS308", GetType(String))
                TS3.Columns.Add("TS309", GetType(String))
                TS3.Columns.Add("TS310", GetType(String))
                TS3.Columns.Add("TS311", GetType(String))
                TS3.Columns.Add("TS312", GetType(String))
                TS3.Columns.Add("TS313", GetType(String))
                TS3.Columns.Add("TS314", GetType(String))
                TS3.Columns.Add("TS315", GetType(String))
                TS3.Columns.Add("TS316", GetType(String))
                TS3.Columns.Add("TS317", GetType(String))
                TS3.Columns.Add("TS318", GetType(String))
                TS3.Columns.Add("TS319", GetType(String))
                TS3.Columns.Add("TS320", GetType(String))
                TS3.Columns.Add("TS321", GetType(String))
                TS3.Columns.Add("TS322", GetType(String))
                TS3.Columns.Add("TS323", GetType(String))
                TS3.Columns.Add("TS324", GetType(String))
                TS3.Columns.Add("ROW_NUMBER", GetType(Integer))
                TS3.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                TS3.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                TS3.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception

            End Try

            'end 835 tables





        End Sub




    End Class

End Namespace

