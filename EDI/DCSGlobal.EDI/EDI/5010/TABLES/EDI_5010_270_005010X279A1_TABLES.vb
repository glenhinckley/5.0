
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


    Public Class EDI_5010_270_005010X279A1_TABLES

        Inherits EDI_5010_COMMON_TABLES



        '******************************************************************************************************************
        '  all vars not declared here are in   EDI_5010_COMMON_DECS   it is inhreted from the tables for this type
        '  if you are going to use it on this class only it goes below if not put it in EDI_5010_COMMON_DECS
        '******************************************************************************************************************

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   A
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Public AMT As New DataTable



        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   D
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public DMG As New DataTable
        Public DTP As New DataTable

        Public EQ_DTP As New DataTable

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   E
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public EQ As New DataTable

        Public EQ_EQ As New DataTable

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   H
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public HL As New DataTable
        Public HI As New DataTable
 

        Public IRL_HL As New DataTable
        Public ISL_HL As New DataTable

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   I
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public III As New DataTable
        Public INS As New DataTable




        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   N
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public N3 As New DataTable
        Public N4 As New DataTable

        Public NM1 As New DataTable
        Public ISL_NM1 As New DataTable
        Public IRL_NM1 As New DataTable

        Public IRL_N3 As New DataTable
        Public IRL_N4 As New DataTable




        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   P
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        Public PRV As New DataTable


        Public IRL_PRV As New DataTable

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   R
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public REF As New DataTable

        Public EQ_REF As New DataTable
        Public IRL_REF As New DataTable


        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   T
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public TRN As New DataTable




        Public Function BuildTables() As Integer

            Dim R = 0

            R = BuildCommonTables()

            If R = 0 Then



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
                    AMT.Columns.Add("270_ISL_GUID", GetType(Guid))
                    AMT.Columns.Add("270_IRL_GUID", GetType(Guid))
                    AMT.Columns.Add("270_SL_GUID", GetType(Guid))
                    AMT.Columns.Add("270_DL_GUID", GetType(Guid))
                    AMT.Columns.Add("270_EQ_GROUP_GUID", GetType(Guid))
                    AMT.Columns.Add("270_EQ_GUID", GetType(Guid))
                    AMT.Columns.Add("NM1_GUID", GetType(Guid))
                    AMT.Columns.Add("HL01", GetType(Integer))
                    AMT.Columns.Add("HL02", GetType(Integer))
                    AMT.Columns.Add("HL03", GetType(Integer))
                    AMT.Columns.Add("HL04", GetType(Integer))
                    AMT.Columns.Add("AMT01", GetType(String))
                    AMT.Columns.Add("AMT02", GetType(String))
                    AMT.Columns.Add("AMT03", GetType(String))
                    AMT.Columns.Add("ROW_NUMBER", GetType(Integer))
                    AMT.Columns.Add("EQ_ROW_NUMBER", GetType(Integer))
                    AMT.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    AMT.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    AMT.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
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
                    DMG.Columns.Add("270_ISL_GUID", GetType(Guid))
                    DMG.Columns.Add("270_IRL_GUID", GetType(Guid))
                    DMG.Columns.Add("270_SL_GUID", GetType(Guid))
                    DMG.Columns.Add("270_DL_GUID", GetType(Guid))
                    DMG.Columns.Add("270_EQ_GROUP_GUID", GetType(Guid))
                    DMG.Columns.Add("270_EQ_GUID", GetType(Guid))
                    DMG.Columns.Add("NM1_GUID", GetType(Guid))
                    DMG.Columns.Add("HL01", GetType(Integer))
                    DMG.Columns.Add("HL02", GetType(Integer))
                    DMG.Columns.Add("HL03", GetType(Integer))
                    DMG.Columns.Add("HL04", GetType(Integer))
                    DMG.Columns.Add("DMG01", GetType(String))
                    DMG.Columns.Add("DMG02", GetType(String))
                    DMG.Columns.Add("DMG03", GetType(String))
                    DMG.Columns.Add("ROW_NUMBER", GetType(Integer))
                    DMG.Columns.Add("EQ_ROW_NUMBER", GetType(Integer))
                    DMG.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    DMG.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    DMG.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
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
                    DTP.Columns.Add("270_ISL_GUID", GetType(Guid))
                    DTP.Columns.Add("270_IRL_GUID", GetType(Guid))
                    DTP.Columns.Add("270_SL_GUID", GetType(Guid))
                    DTP.Columns.Add("270_DL_GUID", GetType(Guid))
                    DTP.Columns.Add("270_EQ_GROUP_GUID", GetType(Guid))
                    DTP.Columns.Add("270_EQ_GUID", GetType(Guid))
                    DTP.Columns.Add("NM1_GUID", GetType(Guid))
                    DTP.Columns.Add("HL01", GetType(Integer))
                    DTP.Columns.Add("HL02", GetType(Integer))
                    DTP.Columns.Add("HL03", GetType(Integer))
                    DTP.Columns.Add("HL04", GetType(Integer))
                    DTP.Columns.Add("DTP01", GetType(String))
                    DTP.Columns.Add("DTP02", GetType(String))
                    DTP.Columns.Add("DTP03", GetType(String))
                    DTP.Columns.Add("ROW_NUMBER", GetType(Integer))
                    DTP.Columns.Add("EQ_ROW_NUMBER", GetType(Integer))
                    DTP.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    DTP.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    DTP.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
                End Try



                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   EQ
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    EQ.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                    EQ.Columns.Add("DOCUMENT_ID", GetType(Integer))
                    EQ.Columns.Add("FILE_ID", GetType(Integer))
                    EQ.Columns.Add("BATCH_ID", GetType(Integer))
                    EQ.Columns.Add("ISA_ID", GetType(Integer))
                    EQ.Columns.Add("GS_ID", GetType(Integer))
                    EQ.Columns.Add("ST_ID", GetType(Integer))
                    EQ.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                    EQ.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                    EQ.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                    EQ.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                    EQ.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                    EQ.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                    EQ.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                    EQ.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                    EQ.Columns.Add("270_ISL_GUID", GetType(Guid))
                    EQ.Columns.Add("270_IRL_GUID", GetType(Guid))
                    EQ.Columns.Add("270_SL_GUID", GetType(Guid))
                    EQ.Columns.Add("270_DL_GUID", GetType(Guid))
                    EQ.Columns.Add("270_EQ_GROUP_GUID", GetType(Guid))
                    EQ.Columns.Add("270_EQ_GUID", GetType(Guid))
                    EQ.Columns.Add("NM1_GUID", GetType(Guid))
                    EQ.Columns.Add("HL01", GetType(Integer))
                    EQ.Columns.Add("HL02", GetType(Integer))
                    EQ.Columns.Add("HL03", GetType(Integer))
                    EQ.Columns.Add("HL04", GetType(Integer))
                    EQ.Columns.Add("EQ01", GetType(String))
                    EQ.Columns.Add("EQ02", GetType(String))
                    EQ.Columns.Add("EQ02_1", GetType(String))
                    EQ.Columns.Add("EQ02_2", GetType(String))
                    EQ.Columns.Add("EQ02_3", GetType(String))
                    EQ.Columns.Add("EQ02_4", GetType(String))
                    EQ.Columns.Add("EQ02_5", GetType(String))
                    EQ.Columns.Add("EQ02_6", GetType(String))
                    EQ.Columns.Add("EQ02_7", GetType(String))
                    EQ.Columns.Add("EQ03", GetType(String))
                    EQ.Columns.Add("EQ04", GetType(String))
                    EQ.Columns.Add("EQ05", GetType(String))
                    EQ.Columns.Add("EQ05_1", GetType(String))
                    EQ.Columns.Add("EQ05_2", GetType(String))
                    EQ.Columns.Add("EQ05_3", GetType(String))
                    EQ.Columns.Add("EQ05_4", GetType(String))
                    EQ.Columns.Add("ROW_NUMBER", GetType(Integer))
                    EQ.Columns.Add("EQ_ROW_NUMBER", GetType(Integer))
                    EQ.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    EQ.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    EQ.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
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
                    HI.Columns.Add("270_ISL_GUID", GetType(Guid))
                    HI.Columns.Add("270_IRL_GUID", GetType(Guid))
                    HI.Columns.Add("270_SL_GUID", GetType(Guid))
                    HI.Columns.Add("270_DL_GUID", GetType(Guid))
                    HI.Columns.Add("270_EQ_GROUP_GUID", GetType(Guid))
                    HI.Columns.Add("270_EQ_GUID", GetType(Guid))
                    HI.Columns.Add("NM1_GUID", GetType(Guid))
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
                    HI.Columns.Add("EQ_ROW_NUMBER", GetType(Integer))
                    HI.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    HI.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    HI.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
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
                    HL.Columns.Add("270_ISL_GUID", GetType(Guid))
                    HL.Columns.Add("270_IRL_GUID", GetType(Guid))
                    HL.Columns.Add("270_SL_GUID", GetType(Guid))
                    HL.Columns.Add("270_DL_GUID", GetType(Guid))
                    HL.Columns.Add("270_EQ_GROUP_GUID", GetType(Guid))
                    HL.Columns.Add("270_EQ_GUID", GetType(Guid))
                    HL.Columns.Add("NM1_GUID", GetType(Guid))
                    HL.Columns.Add("HL01", GetType(Integer))
                    HL.Columns.Add("HL02", GetType(Integer))
                    HL.Columns.Add("HL03", GetType(Integer))
                    HL.Columns.Add("HL04", GetType(Integer))
                    HL.Columns.Add("ROW_NUMBER", GetType(Integer))
                    HL.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    HL.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    HL.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
                End Try





                Try
                    ISL_HL.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                    ISL_HL.Columns.Add("DOCUMENT_ID", GetType(Integer))
                    ISL_HL.Columns.Add("FILE_ID", GetType(Integer))
                    ISL_HL.Columns.Add("BATCH_ID", GetType(Integer))
                    ISL_HL.Columns.Add("ISA_ID", GetType(Integer))
                    ISL_HL.Columns.Add("GS_ID", GetType(Integer))
                    ISL_HL.Columns.Add("ST_ID", GetType(Integer))
                    ISL_HL.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                    ISL_HL.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                    ISL_HL.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                    ISL_HL.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                    ISL_HL.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                    ISL_HL.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                    ISL_HL.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                    ISL_HL.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                    ISL_HL.Columns.Add("270_ISL_GUID", GetType(Guid))
                    ISL_HL.Columns.Add("270_IRL_GUID", GetType(Guid))
                    ISL_HL.Columns.Add("270_SL_GUID", GetType(Guid))
                    ISL_HL.Columns.Add("270_DL_GUID", GetType(Guid))
                    ISL_HL.Columns.Add("270_EQ_GROUP_GUID", GetType(Guid))
                    ISL_HL.Columns.Add("270_EQ_GUID", GetType(Guid))
                    ISL_HL.Columns.Add("NM1_GUID", GetType(Guid))
                    ISL_HL.Columns.Add("HL01", GetType(Integer))
                    ISL_HL.Columns.Add("HL02", GetType(Integer))
                    ISL_HL.Columns.Add("HL03", GetType(Integer))
                    ISL_HL.Columns.Add("HL04", GetType(Integer))
                    ISL_HL.Columns.Add("ROW_NUMBER", GetType(Integer))
                    ISL_HL.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    ISL_HL.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    ISL_HL.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
                End Try

                Try
                    IRL_HL.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                    IRL_HL.Columns.Add("DOCUMENT_ID", GetType(Integer))
                    IRL_HL.Columns.Add("FILE_ID", GetType(Integer))
                    IRL_HL.Columns.Add("BATCH_ID", GetType(Integer))
                    IRL_HL.Columns.Add("ISA_ID", GetType(Integer))
                    IRL_HL.Columns.Add("GS_ID", GetType(Integer))
                    IRL_HL.Columns.Add("ST_ID", GetType(Integer))
                    IRL_HL.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                    IRL_HL.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                    IRL_HL.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                    IRL_HL.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                    IRL_HL.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                    IRL_HL.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                    IRL_HL.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                    IRL_HL.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                    IRL_HL.Columns.Add("270_ISL_GUID", GetType(Guid))
                    IRL_HL.Columns.Add("270_IRL_GUID", GetType(Guid))
                    IRL_HL.Columns.Add("270_SL_GUID", GetType(Guid))
                    IRL_HL.Columns.Add("270_DL_GUID", GetType(Guid))
                    IRL_HL.Columns.Add("270_EQ_GROUP_GUID", GetType(Guid))
                    IRL_HL.Columns.Add("270_EQ_GUID", GetType(Guid))
                    IRL_HL.Columns.Add("NM1_GUID", GetType(Guid))
                    IRL_HL.Columns.Add("HL01", GetType(Integer))
                    IRL_HL.Columns.Add("HL02", GetType(Integer))
                    IRL_HL.Columns.Add("HL03", GetType(Integer))
                    IRL_HL.Columns.Add("HL04", GetType(Integer))
                    IRL_HL.Columns.Add("ROW_NUMBER", GetType(Integer))
                    IRL_HL.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    IRL_HL.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    IRL_HL.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
                End Try







                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   III
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    III.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                    III.Columns.Add("DOCUMENT_ID", GetType(Integer))
                    III.Columns.Add("FILE_ID", GetType(Integer))
                    III.Columns.Add("BATCH_ID", GetType(Integer))
                    III.Columns.Add("ISA_ID", GetType(Integer))
                    III.Columns.Add("GS_ID", GetType(Integer))
                    III.Columns.Add("ST_ID", GetType(Integer))
                    III.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                    III.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                    III.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                    III.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                    III.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                    III.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                    III.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                    III.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                    III.Columns.Add("270_ISL_GUID", GetType(Guid))
                    III.Columns.Add("270_IRL_GUID", GetType(Guid))
                    III.Columns.Add("270_SL_GUID", GetType(Guid))
                    III.Columns.Add("270_DL_GUID", GetType(Guid))
                    III.Columns.Add("270_EQ_GROUP_GUID", GetType(Guid))
                    III.Columns.Add("270_EQ_GUID", GetType(Guid))
                    III.Columns.Add("NM1_GUID", GetType(Guid))
                    III.Columns.Add("HL01", GetType(Integer))
                    III.Columns.Add("HL02", GetType(Integer))
                    III.Columns.Add("HL03", GetType(Integer))
                    III.Columns.Add("HL04", GetType(Integer))
                    III.Columns.Add("III01", GetType(String))
                    III.Columns.Add("III02", GetType(String))
                    III.Columns.Add("III03", GetType(String))
                    III.Columns.Add("III04", GetType(String))
                    III.Columns.Add("III05", GetType(String))
                    III.Columns.Add("III06", GetType(String))
                    III.Columns.Add("III07", GetType(String))
                    III.Columns.Add("III08", GetType(String))
                    III.Columns.Add("III09", GetType(String))
                    III.Columns.Add("ROW_NUMBER", GetType(Integer))
                    III.Columns.Add("EQ_ROW_NUMBER", GetType(Integer))
                    III.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    III.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    III.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
                End Try









                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   INS
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    INS.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                    INS.Columns.Add("DOCUMENT_ID", GetType(Integer))
                    INS.Columns.Add("FILE_ID", GetType(Integer))
                    INS.Columns.Add("BATCH_ID", GetType(Integer))
                    INS.Columns.Add("ISA_ID", GetType(Integer))
                    INS.Columns.Add("GS_ID", GetType(Integer))
                    INS.Columns.Add("ST_ID", GetType(Integer))
                    INS.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                    INS.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                    INS.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                    INS.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                    INS.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                    INS.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                    INS.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                    INS.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                    INS.Columns.Add("270_ISL_GUID", GetType(Guid))
                    INS.Columns.Add("270_IRL_GUID", GetType(Guid))
                    INS.Columns.Add("270_SL_GUID", GetType(Guid))
                    INS.Columns.Add("270_DL_GUID", GetType(Guid))
                    INS.Columns.Add("270_EQ_GROUP_GUID", GetType(Guid))
                    INS.Columns.Add("270_EQ_GUID", GetType(Guid))
                    INS.Columns.Add("NM1_GUID", GetType(Guid))
                    INS.Columns.Add("HL01", GetType(Integer))
                    INS.Columns.Add("HL02", GetType(Integer))
                    INS.Columns.Add("HL03", GetType(Integer))
                    INS.Columns.Add("HL04", GetType(Integer))
                    INS.Columns.Add("INS01", GetType(String))
                    INS.Columns.Add("INS02", GetType(String))
                    INS.Columns.Add("INS03", GetType(String))
                    INS.Columns.Add("INS04", GetType(String))
                    INS.Columns.Add("INS05", GetType(String))
                    INS.Columns.Add("INS06", GetType(String))
                    INS.Columns.Add("INS07", GetType(String))
                    INS.Columns.Add("INS08", GetType(String))
                    INS.Columns.Add("INS09", GetType(String))
                    INS.Columns.Add("INS10", GetType(String))
                    INS.Columns.Add("INS11", GetType(String))
                    INS.Columns.Add("INS12", GetType(String))
                    INS.Columns.Add("INS13", GetType(String))
                    INS.Columns.Add("INS14", GetType(String))
                    INS.Columns.Add("INS15", GetType(String))
                    INS.Columns.Add("INS16", GetType(String))
                    INS.Columns.Add("INS17", GetType(String))
                    INS.Columns.Add("ROW_NUMBER", GetType(Integer))
                    INS.Columns.Add("EQ_ROW_NUMBER", GetType(Integer))
                    INS.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    INS.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    INS.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
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
                    N3.Columns.Add("270_ISL_GUID", GetType(Guid))
                    N3.Columns.Add("270_IRL_GUID", GetType(Guid))
                    N3.Columns.Add("270_SL_GUID", GetType(Guid))
                    N3.Columns.Add("270_DL_GUID", GetType(Guid))
                    N3.Columns.Add("270_EQ_GROUP_GUID", GetType(Guid))
                    N3.Columns.Add("270_EQ_GUID", GetType(Guid))
                    N3.Columns.Add("NM1_GUID", GetType(Guid))
                    N3.Columns.Add("HL01", GetType(Integer))
                    N3.Columns.Add("HL02", GetType(Integer))
                    N3.Columns.Add("HL03", GetType(Integer))
                    N3.Columns.Add("HL04", GetType(Integer))
                    N3.Columns.Add("N301", GetType(String))
                    N3.Columns.Add("N302", GetType(String))
                    N3.Columns.Add("ROW_NUMBER", GetType(Integer))
                    N3.Columns.Add("EQ_ROW_NUMBER", GetType(Integer))
                    N3.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    N3.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    N3.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
                End Try


                Try
                    IRL_N3.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                    IRL_N3.Columns.Add("DOCUMENT_ID", GetType(Integer))
                    IRL_N3.Columns.Add("FILE_ID", GetType(Integer))
                    IRL_N3.Columns.Add("BATCH_ID", GetType(Integer))
                    IRL_N3.Columns.Add("ISA_ID", GetType(Integer))
                    IRL_N3.Columns.Add("GS_ID", GetType(Integer))
                    IRL_N3.Columns.Add("ST_ID", GetType(Integer))
                    IRL_N3.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                    IRL_N3.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                    IRL_N3.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                    IRL_N3.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                    IRL_N3.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                    IRL_N3.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                    IRL_N3.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                    IRL_N3.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                    IRL_N3.Columns.Add("270_ISL_GUID", GetType(Guid))
                    IRL_N3.Columns.Add("270_IRL_GUID", GetType(Guid))
                    IRL_N3.Columns.Add("270_SL_GUID", GetType(Guid))
                    IRL_N3.Columns.Add("270_DL_GUID", GetType(Guid))
                    IRL_N3.Columns.Add("270_EQ_GROUP_GUID", GetType(Guid))
                    IRL_N3.Columns.Add("270_EQ_GUID", GetType(Guid))
                    IRL_N3.Columns.Add("NM1_GUID", GetType(Guid))
                    IRL_N3.Columns.Add("HL01", GetType(Integer))
                    IRL_N3.Columns.Add("HL02", GetType(Integer))
                    IRL_N3.Columns.Add("HL03", GetType(Integer))
                    IRL_N3.Columns.Add("HL04", GetType(Integer))
                    IRL_N3.Columns.Add("N301", GetType(String))
                    IRL_N3.Columns.Add("N302", GetType(String))
                    IRL_N3.Columns.Add("ROW_NUMBER", GetType(Integer))
                    IRL_N3.Columns.Add("EQ_ROW_NUMBER", GetType(Integer))
                    IRL_N3.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    IRL_N3.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    IRL_N3.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
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
                    N4.Columns.Add("270_ISL_GUID", GetType(Guid))
                    N4.Columns.Add("270_IRL_GUID", GetType(Guid))
                    N4.Columns.Add("270_SL_GUID", GetType(Guid))
                    N4.Columns.Add("270_DL_GUID", GetType(Guid))
                    N4.Columns.Add("270_EQ_GROUP_GUID", GetType(Guid))
                    N4.Columns.Add("271_EQ_GUID", GetType(Guid))
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
                    N4.Columns.Add("EQ_ROW_NUMBER", GetType(Integer))
                    N4.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    N4.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    N4.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
                End Try

                Try

                    IRL_N4.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                    IRL_N4.Columns.Add("DOCUMENT_ID", GetType(Integer))
                    IRL_N4.Columns.Add("FILE_ID", GetType(Integer))
                    IRL_N4.Columns.Add("BATCH_ID", GetType(Integer))
                    IRL_N4.Columns.Add("ISA_ID", GetType(Integer))
                    IRL_N4.Columns.Add("GS_ID", GetType(Integer))
                    IRL_N4.Columns.Add("ST_ID", GetType(Integer))
                    IRL_N4.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                    IRL_N4.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                    IRL_N4.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                    IRL_N4.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                    IRL_N4.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                    IRL_N4.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                    IRL_N4.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                    IRL_N4.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                    IRL_N4.Columns.Add("270_ISL_GUID", GetType(Guid))
                    IRL_N4.Columns.Add("270_IRL_GUID", GetType(Guid))
                    IRL_N4.Columns.Add("270_SL_GUID", GetType(Guid))
                    IRL_N4.Columns.Add("270_DL_GUID", GetType(Guid))
                    IRL_N4.Columns.Add("270_EQ_GROUP_GUID", GetType(Guid))
                    IRL_N4.Columns.Add("270_EQ_GUID", GetType(Guid))
                    IRL_N4.Columns.Add("NM1_GUID", GetType(Guid))
                    IRL_N4.Columns.Add("HL01", GetType(Integer))
                    IRL_N4.Columns.Add("HL02", GetType(Integer))
                    IRL_N4.Columns.Add("HL03", GetType(Integer))
                    IRL_N4.Columns.Add("HL04", GetType(Integer))
                    IRL_N4.Columns.Add("N401", GetType(String))
                    IRL_N4.Columns.Add("N402", GetType(String))
                    IRL_N4.Columns.Add("N403", GetType(String))
                    IRL_N4.Columns.Add("N404", GetType(String))
                    IRL_N4.Columns.Add("N405", GetType(String))
                    IRL_N4.Columns.Add("N406", GetType(String))
                    IRL_N4.Columns.Add("N407", GetType(String))
                    IRL_N4.Columns.Add("ROW_NUMBER", GetType(Integer))
                    IRL_N4.Columns.Add("EQ_ROW_NUMBER", GetType(Integer))
                    IRL_N4.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    IRL_N4.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    IRL_N4.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
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
                    NM1.Columns.Add("270_ISL_GUID", GetType(Guid))
                    NM1.Columns.Add("270_IRL_GUID", GetType(Guid))
                    NM1.Columns.Add("270_SL_GUID", GetType(Guid))
                    NM1.Columns.Add("270_DL_GUID", GetType(Guid))
                    NM1.Columns.Add("270_EQ_GROUP_GUID", GetType(Guid))
                    NM1.Columns.Add("270_EQ_GUID", GetType(Guid))
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
                    NM1.Columns.Add("EQ_ROW_NUMBER", GetType(Integer))
                    NM1.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    NM1.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    NM1.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
                End Try


                Try
                    ISL_NM1.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                    ISL_NM1.Columns.Add("DOCUMENT_ID", GetType(Integer))
                    ISL_NM1.Columns.Add("FILE_ID", GetType(Integer))
                    ISL_NM1.Columns.Add("BATCH_ID", GetType(Integer))
                    ISL_NM1.Columns.Add("ISA_ID", GetType(Integer))
                    ISL_NM1.Columns.Add("GS_ID", GetType(Integer))
                    ISL_NM1.Columns.Add("ST_ID", GetType(Integer))
                    ISL_NM1.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                    ISL_NM1.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                    ISL_NM1.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                    ISL_NM1.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                    ISL_NM1.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                    ISL_NM1.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                    ISL_NM1.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                    ISL_NM1.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                    ISL_NM1.Columns.Add("270_ISL_GUID", GetType(Guid))
                    ISL_NM1.Columns.Add("270_IRL_GUID", GetType(Guid))
                    ISL_NM1.Columns.Add("270_SL_GUID", GetType(Guid))
                    ISL_NM1.Columns.Add("270_DL_GUID", GetType(Guid))
                    ISL_NM1.Columns.Add("270_EQ_GROUP_GUID", GetType(Guid))
                    ISL_NM1.Columns.Add("270_EQ_GUID", GetType(Guid))
                    ISL_NM1.Columns.Add("NM1_GUID", GetType(Guid))
                    ISL_NM1.Columns.Add("HL01", GetType(Integer))
                    ISL_NM1.Columns.Add("HL02", GetType(Integer))
                    ISL_NM1.Columns.Add("HL03", GetType(Integer))
                    ISL_NM1.Columns.Add("HL04", GetType(Integer))
                    ISL_NM1.Columns.Add("NM101", GetType(String))
                    ISL_NM1.Columns.Add("NM102", GetType(String))
                    ISL_NM1.Columns.Add("NM103", GetType(String))
                    ISL_NM1.Columns.Add("NM104", GetType(String))
                    ISL_NM1.Columns.Add("NM105", GetType(String))
                    ISL_NM1.Columns.Add("NM106", GetType(String))
                    ISL_NM1.Columns.Add("NM107", GetType(String))
                    ISL_NM1.Columns.Add("NM108", GetType(String))
                    ISL_NM1.Columns.Add("NM109", GetType(String))
                    ISL_NM1.Columns.Add("NM110", GetType(String))
                    ISL_NM1.Columns.Add("NM111", GetType(String))
                    ISL_NM1.Columns.Add("ROW_NUMBER", GetType(Integer))
                    ISL_NM1.Columns.Add("EQ_ROW_NUMBER", GetType(Integer))
                    ISL_NM1.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    ISL_NM1.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    ISL_NM1.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
                End Try


                Try
                    IRL_NM1.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                    IRL_NM1.Columns.Add("DOCUMENT_ID", GetType(Integer))
                    IRL_NM1.Columns.Add("FILE_ID", GetType(Integer))
                    IRL_NM1.Columns.Add("BATCH_ID", GetType(Integer))
                    IRL_NM1.Columns.Add("ISA_ID", GetType(Integer))
                    IRL_NM1.Columns.Add("GS_ID", GetType(Integer))
                    IRL_NM1.Columns.Add("ST_ID", GetType(Integer))
                    IRL_NM1.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                    IRL_NM1.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                    IRL_NM1.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                    IRL_NM1.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                    IRL_NM1.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                    IRL_NM1.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                    IRL_NM1.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                    IRL_NM1.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                    IRL_NM1.Columns.Add("270_ISL_GUID", GetType(Guid))
                    IRL_NM1.Columns.Add("270_IRL_GUID", GetType(Guid))
                    IRL_NM1.Columns.Add("270_SL_GUID", GetType(Guid))
                    IRL_NM1.Columns.Add("270_DL_GUID", GetType(Guid))
                    IRL_NM1.Columns.Add("270_EQ_GROUP_GUID", GetType(Guid))
                    IRL_NM1.Columns.Add("270_EQ_GUID", GetType(Guid))
                    IRL_NM1.Columns.Add("NM1_GUID", GetType(Guid))
                    IRL_NM1.Columns.Add("HL01", GetType(Integer))
                    IRL_NM1.Columns.Add("HL02", GetType(Integer))
                    IRL_NM1.Columns.Add("HL03", GetType(Integer))
                    IRL_NM1.Columns.Add("HL04", GetType(Integer))
                    IRL_NM1.Columns.Add("NM101", GetType(String))
                    IRL_NM1.Columns.Add("NM102", GetType(String))
                    IRL_NM1.Columns.Add("NM103", GetType(String))
                    IRL_NM1.Columns.Add("NM104", GetType(String))
                    IRL_NM1.Columns.Add("NM105", GetType(String))
                    IRL_NM1.Columns.Add("NM106", GetType(String))
                    IRL_NM1.Columns.Add("NM107", GetType(String))
                    IRL_NM1.Columns.Add("NM108", GetType(String))
                    IRL_NM1.Columns.Add("NM109", GetType(String))
                    IRL_NM1.Columns.Add("NM110", GetType(String))
                    IRL_NM1.Columns.Add("NM111", GetType(String))
                    IRL_NM1.Columns.Add("ROW_NUMBER", GetType(Integer))
                    IRL_NM1.Columns.Add("EQ_ROW_NUMBER", GetType(Integer))
                    IRL_NM1.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    IRL_NM1.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    IRL_NM1.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
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
                    PRV.Columns.Add("270_ISL_GUID", GetType(Guid))
                    PRV.Columns.Add("270_IRL_GUID", GetType(Guid))
                    PRV.Columns.Add("270_SL_GUID", GetType(Guid))
                    PRV.Columns.Add("270_DL_GUID", GetType(Guid))
                    PRV.Columns.Add("270_EQ_GROUP_GUID", GetType(Guid))
                    PRV.Columns.Add("270_EQ_GUID", GetType(Guid))
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
                    PRV.Columns.Add("EQ_ROW_NUMBER", GetType(Integer))
                    PRV.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    PRV.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    PRV.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
                End Try


                Try
                    IRL_PRV.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                    IRL_PRV.Columns.Add("DOCUMENT_ID", GetType(Integer))
                    IRL_PRV.Columns.Add("FILE_ID", GetType(Integer))
                    IRL_PRV.Columns.Add("BATCH_ID", GetType(Integer))
                    IRL_PRV.Columns.Add("ISA_ID", GetType(Integer))
                    IRL_PRV.Columns.Add("GS_ID", GetType(Integer))
                    IRL_PRV.Columns.Add("ST_ID", GetType(Integer))
                    IRL_PRV.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                    IRL_PRV.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                    IRL_PRV.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                    IRL_PRV.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                    IRL_PRV.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                    IRL_PRV.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                    IRL_PRV.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                    IRL_PRV.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                    IRL_PRV.Columns.Add("270_ISL_GUID", GetType(Guid))
                    IRL_PRV.Columns.Add("270_IRL_GUID", GetType(Guid))
                    IRL_PRV.Columns.Add("270_SL_GUID", GetType(Guid))
                    IRL_PRV.Columns.Add("270_DL_GUID", GetType(Guid))
                    IRL_PRV.Columns.Add("270_EQ_GROUP_GUID", GetType(Guid))
                    IRL_PRV.Columns.Add("270_EQ_GUID", GetType(Guid))
                    IRL_PRV.Columns.Add("NM1_GUID", GetType(Guid))
                    IRL_PRV.Columns.Add("HL01", GetType(Integer))
                    IRL_PRV.Columns.Add("HL02", GetType(Integer))
                    IRL_PRV.Columns.Add("HL03", GetType(Integer))
                    IRL_PRV.Columns.Add("HL04", GetType(Integer))
                    IRL_PRV.Columns.Add("PRV01", GetType(String))
                    IRL_PRV.Columns.Add("PRV02", GetType(String))
                    IRL_PRV.Columns.Add("PRV03", GetType(String))
                    IRL_PRV.Columns.Add("PRV04", GetType(String))
                    IRL_PRV.Columns.Add("PRV05", GetType(String))
                    IRL_PRV.Columns.Add("PRV06", GetType(String))
                    IRL_PRV.Columns.Add("ROW_NUMBER", GetType(Integer))
                    IRL_PRV.Columns.Add("EQ_ROW_NUMBER", GetType(Integer))
                    IRL_PRV.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    IRL_PRV.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    IRL_PRV.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
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
                    REF.Columns.Add("270_ISL_GUID", GetType(Guid))
                    REF.Columns.Add("270_IRL_GUID", GetType(Guid))
                    REF.Columns.Add("270_SL_GUID", GetType(Guid))
                    REF.Columns.Add("270_DL_GUID", GetType(Guid))
                    REF.Columns.Add("270_EQ_GROUP_GUID", GetType(Guid))
                    REF.Columns.Add("270_EQ_GUID", GetType(Guid))
                    REF.Columns.Add("NM1_GUID", GetType(Guid))
                    REF.Columns.Add("HL01", GetType(Integer))
                    REF.Columns.Add("HL02", GetType(Integer))
                    REF.Columns.Add("HL03", GetType(Integer))
                    REF.Columns.Add("HL04", GetType(Integer))
                    REF.Columns.Add("REF01", GetType(String))
                    REF.Columns.Add("REF02", GetType(String))
                    REF.Columns.Add("REF03", GetType(String))
                    REF.Columns.Add("ROW_NUMBER", GetType(Integer))
                    REF.Columns.Add("EQ_ROW_NUMBER", GetType(Integer))
                    REF.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    REF.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    REF.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
                End Try


                Try
                    IRL_REF.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                    IRL_REF.Columns.Add("DOCUMENT_ID", GetType(Integer))
                    IRL_REF.Columns.Add("FILE_ID", GetType(Integer))
                    IRL_REF.Columns.Add("BATCH_ID", GetType(Integer))
                    IRL_REF.Columns.Add("ISA_ID", GetType(Integer))
                    IRL_REF.Columns.Add("GS_ID", GetType(Integer))
                    IRL_REF.Columns.Add("ST_ID", GetType(Integer))
                    IRL_REF.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                    IRL_REF.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                    IRL_REF.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                    IRL_REF.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                    IRL_REF.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                    IRL_REF.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                    IRL_REF.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                    IRL_REF.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                    IRL_REF.Columns.Add("270_ISL_GUID", GetType(Guid))
                    IRL_REF.Columns.Add("270_IRL_GUID", GetType(Guid))
                    IRL_REF.Columns.Add("270_SL_GUID", GetType(Guid))
                    IRL_REF.Columns.Add("270_DL_GUID", GetType(Guid))
                    IRL_REF.Columns.Add("270_EQ_GROUP_GUID", GetType(Guid))
                    IRL_REF.Columns.Add("270_EQ_GUID", GetType(Guid))
                    IRL_REF.Columns.Add("NM1_GUID", GetType(Guid))
                    IRL_REF.Columns.Add("HL01", GetType(Integer))
                    IRL_REF.Columns.Add("HL02", GetType(Integer))
                    IRL_REF.Columns.Add("HL03", GetType(Integer))
                    IRL_REF.Columns.Add("HL04", GetType(Integer))
                    IRL_REF.Columns.Add("REF01", GetType(String))
                    IRL_REF.Columns.Add("REF02", GetType(String))
                    IRL_REF.Columns.Add("REF03", GetType(String))
                    IRL_REF.Columns.Add("ROW_NUMBER", GetType(Integer))
                    IRL_REF.Columns.Add("EQ_ROW_NUMBER", GetType(Integer))
                    IRL_REF.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    IRL_REF.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    IRL_REF.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
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
                    TRN.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                    TRN.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                    TRN.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                    TRN.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                    TRN.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                    TRN.Columns.Add("270_ISL_GUID", GetType(Guid))
                    TRN.Columns.Add("270_IRL_GUID", GetType(Guid))
                    TRN.Columns.Add("270_SL_GUID", GetType(Guid))
                    TRN.Columns.Add("270_DL_GUID", GetType(Guid))
                    TRN.Columns.Add("270_EQ_GROUP_GUID", GetType(Guid))
                    TRN.Columns.Add("270_EQ_GUID", GetType(Guid))
                    TRN.Columns.Add("NM1_GUID", GetType(Guid))
                    TRN.Columns.Add("HL01", GetType(Integer))
                    TRN.Columns.Add("HL02", GetType(Integer))
                    TRN.Columns.Add("HL03", GetType(Integer))
                    TRN.Columns.Add("HL04", GetType(Integer))
                    TRN.Columns.Add("TRN01", GetType(String))
                    TRN.Columns.Add("TRN02", GetType(String))
                    TRN.Columns.Add("TRN03", GetType(String))
                    TRN.Columns.Add("TRN04", GetType(String))
                    TRN.Columns.Add("ROW_NUMBER", GetType(Integer))
                    TRN.Columns.Add("EQ_ROW_NUMBER", GetType(Integer))
                    TRN.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    TRN.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    TRN.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
                End Try

            End If
            Return R

        End Function




    End Class

End Namespace

