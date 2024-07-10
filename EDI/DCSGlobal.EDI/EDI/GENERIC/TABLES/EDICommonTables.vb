
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


    Public Class EDICommonTables

        Inherits EDICommonDecs


        Public ISA As New DataTable
        Public GS As New DataTable
        Public ST As New DataTable

        Public IEA As New DataTable
        Public GE As New DataTable
        Public SE As New DataTable

        Public BHT As New DataTable
        Public HL As New DataTable

        Public UNK As New DataTable


        Public Sub BuildCommonTables()


            Try

                ISA.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                ISA.Columns.Add("DOCUMENT_ID", GetType(Integer))
                ISA.Columns.Add("FILE_ID", GetType(Integer))
                '  ISA.Columns.Add("BATCH_ID", GetType(Integer))
                ISA.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                ISA.Columns.Add("ISA01", GetType(String))
                ISA.Columns.Add("ISA02", GetType(String))
                ISA.Columns.Add("ISA03", GetType(String))
                ISA.Columns.Add("ISA04", GetType(String))
                ISA.Columns.Add("ISA05", GetType(String))
                ISA.Columns.Add("ISA06", GetType(String))
                ISA.Columns.Add("ISA07", GetType(String))
                ISA.Columns.Add("ISA08", GetType(String))
                ISA.Columns.Add("ISA09", GetType(String))
                ISA.Columns.Add("ISA10", GetType(String))
                ISA.Columns.Add("ISA11", GetType(String))
                ISA.Columns.Add("ISA12", GetType(String))
                ISA.Columns.Add("ISA13", GetType(String))
                ISA.Columns.Add("ISA14", GetType(String))
                ISA.Columns.Add("ISA15", GetType(String))
                ISA.Columns.Add("ISA16", GetType(String))
                ISA.Columns.Add("ROW_NUMBER", GetType(Integer))
            Catch ex As Exception

            End Try

            Try
                IEA.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                IEA.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                IEA.Columns.Add("IEA01", GetType(Integer))
                IEA.Columns.Add("IEA02", GetType(String))
                IEA.Columns.Add("ROW_NUMBER", GetType(Integer))
            Catch ex As Exception

            End Try


            Try
                GS.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                GS.Columns.Add("DOCUMENT_ID", GetType(Integer))
                GS.Columns.Add("FILE_ID", GetType(Integer))
                '  GS.Columns.Add("BATCH_ID", GetType(Integer))
                GS.Columns.Add("ISA_ID", GetType(Integer))
                GS.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                GS.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                GS.Columns.Add("GS01", GetType(String))
                GS.Columns.Add("GS02", GetType(String))
                GS.Columns.Add("GS03", GetType(String))
                GS.Columns.Add("GS04", GetType(String))
                GS.Columns.Add("GS05", GetType(String))
                GS.Columns.Add("GS06", GetType(String))
                GS.Columns.Add("GS07", GetType(String))
                GS.Columns.Add("GS08", GetType(String))
                GS.Columns.Add("ROW_NUMBER", GetType(Integer))

            Catch ex As Exception

            End Try


            Try
                GE.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                GE.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                GE.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                GE.Columns.Add("GE01", GetType(Integer))
                GE.Columns.Add("GE02", GetType(String))
                GE.Columns.Add("ROW_NUMBER", GetType(Integer))
            Catch ex As Exception

            End Try


            Try

                ST.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                ST.Columns.Add("DOCUMENT_ID", GetType(Integer))
                ST.Columns.Add("FILE_ID", GetType(Integer))
                '  ST.Columns.Add("BATCH_ID", GetType(Integer))
                ST.Columns.Add("ISA_ID", GetType(Integer))
                ST.Columns.Add("GS_ID", GetType(Integer))
                ST.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                ST.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                ST.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                ST.Columns.Add("ST01", GetType(String))
                ST.Columns.Add("ST02", GetType(String))
                ST.Columns.Add("ST03", GetType(String))
                ST.Columns.Add("ROW_NUMBER", GetType(Integer))
            Catch ex As Exception

            End Try




            Try
                SE.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                SE.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                SE.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                SE.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                SE.Columns.Add("SE01", GetType(Integer))
                SE.Columns.Add("SE02", GetType(String))
                SE.Columns.Add("ROW_NUMBER", GetType(Integer))
            Catch ex As Exception

            End Try

            Try
                BHT.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                BHT.Columns.Add("DOCUMENT_ID", GetType(Integer))
                ' BHT.Columns.Add("BATCH_ID", GetType(Integer))
                BHT.Columns.Add("FILE_ID", GetType(Integer))
                BHT.Columns.Add("ISA_ID", GetType(Integer))
                BHT.Columns.Add("GS_ID", GetType(Integer))
                BHT.Columns.Add("ST_ID", GetType(Integer))
                BHT.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                BHT.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                BHT.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                BHT.Columns.Add("BHT01", GetType(String))
                BHT.Columns.Add("BHT02", GetType(String))
                BHT.Columns.Add("BHT03", GetType(String))
                BHT.Columns.Add("BHT04", GetType(String))
                BHT.Columns.Add("BHT05", GetType(String))
                BHT.Columns.Add("BHT06", GetType(String))
                BHT.Columns.Add("ROW_NUMBER", GetType(Integer))
            Catch ex As Exception

            End Try


            Try
                HL.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                HL.Columns.Add("DOCUMENT_ID", GetType(Integer))
                HL.Columns.Add("FILE_ID", GetType(Integer))
                '  HL.Columns.Add("BATCH_ID", GetType(Integer))
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



            'Try
            '    AAA.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            '    AAA.Columns.Add("BATCH_ID", GetType(Integer))
            '    AAA.Columns.Add("FILE_ID", GetType(Integer))
            '    AAA.Columns.Add("ISA_ID", GetType(Integer))
            '    AAA.Columns.Add("GS_ID", GetType(Integer))
            '    AAA.Columns.Add("ST_ID", GetType(Integer))
            '    AAA.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            '    AAA.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            '    AAA.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            '    AAA.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
            '    AAA.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
            '    AAA.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
            '    AAA.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
            '    AAA.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
            '    AAA.Columns.Add("HL01", GetType(Integer))
            '    AAA.Columns.Add("HL02", GetType(Integer))
            '    AAA.Columns.Add("HL03", GetType(Integer))
            '    AAA.Columns.Add("HL04", GetType(Integer))
            '    AAA.Columns.Add("AAA01", GetType(String))
            '    AAA.Columns.Add("AAA02", GetType(String))
            '    AAA.Columns.Add("AAA03", GetType(String))
            '    AAA.Columns.Add("AAA04", GetType(String))
            '    AAA.Columns.Add("ROW_NUMBER", GetType(Integer))

            'Catch ex As Exception

            'End Try

            'Try
            '    AMT.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            '    AMT.Columns.Add("FILE_ID", GetType(Integer))
            '    AMT.Columns.Add("BATCH_ID", GetType(Integer))
            '    AMT.Columns.Add("ISA_ID", GetType(Integer))
            '    AMT.Columns.Add("GS_ID", GetType(Integer))
            '    AMT.Columns.Add("ST_ID", GetType(Integer))
            '    AMT.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            '    AMT.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            '    AMT.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            '    AMT.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
            '    AMT.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
            '    AMT.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
            '    AMT.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
            '    AMT.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
            '    AMT.Columns.Add("HL01", GetType(Integer))
            '    AMT.Columns.Add("HL02", GetType(Integer))
            '    AMT.Columns.Add("HL03", GetType(Integer))
            '    AMT.Columns.Add("HL04", GetType(Integer))
            '    AMT.Columns.Add("AMT01", GetType(String))
            '    AMT.Columns.Add("AMT02", GetType(String))
            '    AMT.Columns.Add("AMT03", GetType(String))
            '    AMT.Columns.Add("ROW_NUMBER", GetType(Integer))
            'Catch ex As Exception

            'End Try




            'Try

            '    DMG.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            '    DMG.Columns.Add("BATCH_ID", GetType(Integer))
            '    DMG.Columns.Add("FILE_ID", GetType(Integer))
            '    DMG.Columns.Add("ISA_ID", GetType(Integer))
            '    DMG.Columns.Add("GS_ID", GetType(Integer))
            '    DMG.Columns.Add("ST_ID", GetType(Integer))
            '    DMG.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            '    DMG.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            '    DMG.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            '    DMG.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
            '    DMG.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
            '    DMG.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
            '    DMG.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
            '    DMG.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
            '    DMG.Columns.Add("HL01", GetType(Integer))
            '    DMG.Columns.Add("HL02", GetType(Integer))
            '    DMG.Columns.Add("HL03", GetType(Integer))
            '    DMG.Columns.Add("HL04", GetType(Integer))
            '    DMG.Columns.Add("DMG01", GetType(String))
            '    DMG.Columns.Add("DMG02", GetType(String))
            '    DMG.Columns.Add("DMG03", GetType(String))
            '    DMG.Columns.Add("ROW_NUMBER", GetType(Integer))
            'Catch ex As Exception

            'End Try


            'Try
            '    DTP.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            '    DTP.Columns.Add("BATCH_ID", GetType(Integer))
            '    DTP.Columns.Add("FILE_ID", GetType(Integer))
            '    DTP.Columns.Add("ISA_ID", GetType(Integer))
            '    DTP.Columns.Add("GS_ID", GetType(Integer))
            '    DTP.Columns.Add("ST_ID", GetType(Integer))
            '    DTP.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            '    DTP.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            '    DTP.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            '    DTP.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
            '    DTP.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
            '    DTP.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
            '    DTP.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
            '    DTP.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
            '    DTP.Columns.Add("HL01", GetType(Integer))
            '    DTP.Columns.Add("HL02", GetType(Integer))
            '    DTP.Columns.Add("HL03", GetType(Integer))
            '    DTP.Columns.Add("HL04", GetType(Integer))
            '    DTP.Columns.Add("DTP01", GetType(String))
            '    DTP.Columns.Add("DTP02", GetType(String))
            '    DTP.Columns.Add("DTP03", GetType(String))
            '    DTP.Columns.Add("ROW_NUMBER", GetType(Integer))
            'Catch ex As Exception

            'End Try









            'Try

            '    N1.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            '    N1.Columns.Add("BATCH_ID", GetType(Integer))
            '    N1.Columns.Add("FILE_ID", GetType(Integer))
            '    N1.Columns.Add("ISA_ID", GetType(Integer))
            '    N1.Columns.Add("GS_ID", GetType(Integer))
            '    N1.Columns.Add("ST_ID", GetType(Integer))
            '    N1.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            '    N1.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            '    N1.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            '    N1.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
            '    N1.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
            '    N1.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
            '    N1.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
            '    N1.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
            '    N1.Columns.Add("HL01", GetType(Integer))
            '    N1.Columns.Add("HL02", GetType(Integer))
            '    N1.Columns.Add("HL03", GetType(Integer))
            '    N1.Columns.Add("HL04", GetType(Integer))
            '    N1.Columns.Add("N101", GetType(String))
            '    N1.Columns.Add("N102", GetType(String))
            '    N1.Columns.Add("N103", GetType(String))
            '    N1.Columns.Add("N104", GetType(String))
            '    N1.Columns.Add("ROW_NUMBER", GetType(Integer))
            'Catch ex As Exception

            'End Try





            'Try
            '    N3.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            '    N3.Columns.Add("BATCH_ID", GetType(Integer))
            '    N3.Columns.Add("FILE_ID", GetType(Integer))
            '    N3.Columns.Add("ISA_ID", GetType(Integer))
            '    N3.Columns.Add("GS_ID", GetType(Integer))
            '    N3.Columns.Add("ST_ID", GetType(Integer))
            '    N3.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            '    N3.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            '    N3.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            '    N3.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
            '    N3.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
            '    N3.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
            '    N3.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
            '    N3.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
            '    N3.Columns.Add("HL01", GetType(Integer))
            '    N3.Columns.Add("HL02", GetType(Integer))
            '    N3.Columns.Add("HL03", GetType(Integer))
            '    N3.Columns.Add("HL04", GetType(Integer))
            '    N3.Columns.Add("N301", GetType(String))
            '    N3.Columns.Add("N302", GetType(String))
            '    N3.Columns.Add("ROW_NUMBER", GetType(Integer))
            'Catch ex As Exception

            'End Try


            'Try

            '    N4.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            '    N4.Columns.Add("BATCH_ID", GetType(Integer))
            '    N4.Columns.Add("FILE_ID", GetType(Integer))
            '    N4.Columns.Add("ISA_ID", GetType(Integer))
            '    N4.Columns.Add("GS_ID", GetType(Integer))
            '    N4.Columns.Add("ST_ID", GetType(Integer))
            '    N4.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            '    N4.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            '    N4.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            '    N4.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
            '    N4.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
            '    N4.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
            '    N4.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
            '    N4.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
            '    N4.Columns.Add("HL01", GetType(Integer))
            '    N4.Columns.Add("HL02", GetType(Integer))
            '    N4.Columns.Add("HL03", GetType(Integer))
            '    N4.Columns.Add("HL04", GetType(Integer))
            '    N4.Columns.Add("N401", GetType(String))
            '    N4.Columns.Add("N402", GetType(String))
            '    N4.Columns.Add("N403", GetType(String))
            '    N4.Columns.Add("N404", GetType(String))
            '    N4.Columns.Add("N405", GetType(String))
            '    N4.Columns.Add("N406", GetType(String))
            '    N4.Columns.Add("N407", GetType(String))
            '    N4.Columns.Add("ROW_NUMBER", GetType(Integer))
            'Catch ex As Exception

            'End Try








            'Try
            '    NM1.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            '    NM1.Columns.Add("BATCH_ID", GetType(Integer))
            '    NM1.Columns.Add("FILE_ID", GetType(Integer))
            '    NM1.Columns.Add("ISA_ID", GetType(Integer))
            '    NM1.Columns.Add("GS_ID", GetType(Integer))
            '    NM1.Columns.Add("ST_ID", GetType(Integer))
            '    NM1.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            '    NM1.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            '    NM1.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            '    NM1.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
            '    NM1.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
            '    NM1.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
            '    NM1.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
            '    NM1.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
            '    NM1.Columns.Add("HL01", GetType(Integer))
            '    NM1.Columns.Add("HL02", GetType(Integer))
            '    NM1.Columns.Add("HL03", GetType(Integer))
            '    NM1.Columns.Add("HL04", GetType(Integer))
            '    NM1.Columns.Add("NM101", GetType(String))
            '    NM1.Columns.Add("NM102", GetType(String))
            '    NM1.Columns.Add("NM103", GetType(String))
            '    NM1.Columns.Add("NM104", GetType(String))
            '    NM1.Columns.Add("NM105", GetType(String))
            '    NM1.Columns.Add("NM106", GetType(String))
            '    NM1.Columns.Add("NM107", GetType(String))
            '    NM1.Columns.Add("NM108", GetType(String))
            '    NM1.Columns.Add("NM109", GetType(String))
            '    NM1.Columns.Add("NM110", GetType(String))
            '    NM1.Columns.Add("NM111", GetType(String))
            '    NM1.Columns.Add("ROW_NUMBER", GetType(Integer))

            'Catch ex As Exception

            'End Try







            'Try
            '    PRV.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            '    PRV.Columns.Add("BATCH_ID", GetType(Integer))
            '    PRV.Columns.Add("FILE_ID", GetType(Integer))
            '    PRV.Columns.Add("ISA_ID", GetType(Integer))
            '    PRV.Columns.Add("GS_ID", GetType(Integer))
            '    PRV.Columns.Add("ST_ID", GetType(Integer))
            '    PRV.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            '    PRV.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            '    PRV.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            '    PRV.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
            '    PRV.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
            '    PRV.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
            '    PRV.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
            '    PRV.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
            '    PRV.Columns.Add("HL01", GetType(Integer))
            '    PRV.Columns.Add("HL02", GetType(Integer))
            '    PRV.Columns.Add("HL03", GetType(Integer))
            '    PRV.Columns.Add("HL04", GetType(Integer))
            '    PRV.Columns.Add("PRV01", GetType(String))
            '    PRV.Columns.Add("PRV02", GetType(String))
            '    PRV.Columns.Add("PRV03", GetType(String))
            '    PRV.Columns.Add("PRV04", GetType(String))
            '    PRV.Columns.Add("PRV05", GetType(String))
            '    PRV.Columns.Add("PRV06", GetType(String))
            '    PRV.Columns.Add("ROW_NUMBER", GetType(Integer))

            'Catch ex As Exception

            'End Try

            'Try
            '    REF.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            '    REF.Columns.Add("BATCH_ID", GetType(Integer))
            '    REF.Columns.Add("FILE_ID", GetType(Integer))
            '    REF.Columns.Add("ISA_ID", GetType(Integer))
            '    REF.Columns.Add("GS_ID", GetType(Integer))
            '    REF.Columns.Add("ST_ID", GetType(Integer))
            '    REF.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            '    REF.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            '    REF.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            '    REF.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
            '    REF.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
            '    REF.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
            '    REF.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
            '    REF.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
            '    REF.Columns.Add("HL01", GetType(Integer))
            '    REF.Columns.Add("HL02", GetType(Integer))
            '    REF.Columns.Add("HL03", GetType(Integer))
            '    REF.Columns.Add("HL04", GetType(Integer))
            '    REF.Columns.Add("REF01", GetType(String))
            '    REF.Columns.Add("REF02", GetType(String))
            '    REF.Columns.Add("REF03", GetType(String))
            '    REF.Columns.Add("ROW_NUMBER", GetType(Integer))

            'Catch ex As Exception

            'End Try







            'Try
            '    TRN.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            '    TRN.Columns.Add("BATCH_ID", GetType(Integer))
            '    TRN.Columns.Add("FILE_ID", GetType(Integer))
            '    TRN.Columns.Add("ISA_ID", GetType(Integer))
            '    TRN.Columns.Add("GS_ID", GetType(Integer))
            '    TRN.Columns.Add("ST_ID", GetType(Integer))
            '    TRN.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            '    TRN.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            '    TRN.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            '    TRN.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
            '    TRN.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
            '    TRN.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
            '    TRN.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
            '    TRN.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
            '    TRN.Columns.Add("HL01", GetType(Integer))
            '    TRN.Columns.Add("HL02", GetType(Integer))
            '    TRN.Columns.Add("HL03", GetType(Integer))
            '    TRN.Columns.Add("HL04", GetType(Integer))
            '    TRN.Columns.Add("TRN01", GetType(String))
            '    TRN.Columns.Add("TRN02", GetType(String))
            '    TRN.Columns.Add("TRN03", GetType(String))
            '    TRN.Columns.Add("TRN04", GetType(String))
            '    TRN.Columns.Add("ROW_NUMBER", GetType(Integer))
            'Catch ex As Exception

            'End Try







            Try
                UNK.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                UNK.Columns.Add("DOCUMENT_ID", GetType(Integer))
                UNK.Columns.Add("BATCH_ID", GetType(Integer))
                UNK.Columns.Add("FILE_ID", GetType(Integer))
                UNK.Columns.Add("ISA_ID", GetType(Integer))
                UNK.Columns.Add("GS_ID", GetType(Integer))
                UNK.Columns.Add("ST_ID", GetType(Integer))
                UNK.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                UNK.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                UNK.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                UNK.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                UNK.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                UNK.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                UNK.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                UNK.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                UNK.Columns.Add("HL01", GetType(Integer))
                UNK.Columns.Add("HL02", GetType(Integer))
                UNK.Columns.Add("HL03", GetType(Integer))
                UNK.Columns.Add("HL04", GetType(Integer))
                UNK.Columns.Add("ROW_RECORD_TYPE", GetType(String))
                UNK.Columns.Add("ROW_DATA", GetType(String))
                UNK.Columns.Add("ROW_NUMBER", GetType(Integer))
            Catch ex As Exception

            End Try


        End Sub




    End Class

End Namespace

