
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


    Public Class EDI_5010_271_005010X279A1_TABLES

        Inherits EDI_5010_COMMON_TABLES



        '******************************************************************************************************************
        '  all vars not declared here are in   EDI_5010_COMMON_DECS   it is inhreted from the tables for this type
        '  if you are going to use it on this class only it goes below if not put it in EDI_5010_COMMON_DECS
        '******************************************************************************************************************

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   A
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public AAA As New DataTable
        Public AMT As New DataTable

        Public EB_AAA As New DataTable
        Public ISL_AAA As New DataTable
        Public IRL_AAA As New DataTable

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   D
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public DMG As New DataTable
        Public DTP As New DataTable

        Public EB_DTP As New DataTable

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   E
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public EB As New DataTable

        Public EB_EB As New DataTable

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   H
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public HL As New DataTable
        Public HI As New DataTable
        Public HSD As New DataTable

        Public EB_HSD As New DataTable

        Public IRL_HL As New DataTable
        Public ISL_HL As New DataTable

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   I
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public III As New DataTable
        Public INS As New DataTable


        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   L
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public LE As New DataTable
        Public LS As New DataTable

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   M
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public MPI As New DataTable
        Public MSG As New DataTable

        Public EB_MSG As New DataTable

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
        Public PER As New DataTable
        Public PRV As New DataTable

        Public ISL_PER As New DataTable

        Public IRL_PRV As New DataTable

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '   R
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public REF As New DataTable

        Public EB_REF As New DataTable
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
                '   AAA
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    AAA.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                    AAA.Columns.Add("DOCUMENT_ID", GetType(Integer))
                    AAA.Columns.Add("FILE_ID", GetType(Integer))
                    AAA.Columns.Add("BATCH_ID", GetType(Integer))
                    AAA.Columns.Add("ISA_ID", GetType(Integer))
                    AAA.Columns.Add("GS_ID", GetType(Integer))
                    AAA.Columns.Add("ST_ID", GetType(Integer))
                    AAA.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                    AAA.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                    AAA.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                    AAA.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                    AAA.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                    AAA.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                    AAA.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                    AAA.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                    AAA.Columns.Add("271_ISL_GUID", GetType(Guid))
                    AAA.Columns.Add("271_IRL_GUID", GetType(Guid))
                    AAA.Columns.Add("271_SL_GUID", GetType(Guid))
                    AAA.Columns.Add("271_DL_GUID", GetType(Guid))
                    AAA.Columns.Add("271_EB_GUID", GetType(Guid))
                    AAA.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    AAA.Columns.Add("271_LS_GUID", GetType(Guid))
                    AAA.Columns.Add("NM1_GUID", GetType(Guid))
                    AAA.Columns.Add("HL01", GetType(Integer))
                    AAA.Columns.Add("HL02", GetType(Integer))
                    AAA.Columns.Add("HL03", GetType(Integer))
                    AAA.Columns.Add("HL04", GetType(Integer))
                    AAA.Columns.Add("AAA01", GetType(String))
                    AAA.Columns.Add("AAA02", GetType(String))
                    AAA.Columns.Add("AAA03", GetType(String))
                    AAA.Columns.Add("AAA04", GetType(String))
                    AAA.Columns.Add("ROW_NUMBER", GetType(Integer))
                    AAA.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
                    AAA.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    AAA.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    AAA.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
                End Try


                Try
                    ISL_AAA.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                    ISL_AAA.Columns.Add("DOCUMENT_ID", GetType(Integer))
                    ISL_AAA.Columns.Add("FILE_ID", GetType(Integer))
                    ISL_AAA.Columns.Add("BATCH_ID", GetType(Integer))
                    ISL_AAA.Columns.Add("ISA_ID", GetType(Integer))
                    ISL_AAA.Columns.Add("GS_ID", GetType(Integer))
                    ISL_AAA.Columns.Add("ST_ID", GetType(Integer))
                    ISL_AAA.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                    ISL_AAA.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                    ISL_AAA.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                    ISL_AAA.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                    ISL_AAA.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                    ISL_AAA.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                    ISL_AAA.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                    ISL_AAA.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                    ISL_AAA.Columns.Add("271_ISL_GUID", GetType(Guid))
                    ISL_AAA.Columns.Add("271_IRL_GUID", GetType(Guid))
                    ISL_AAA.Columns.Add("271_SL_GUID", GetType(Guid))
                    ISL_AAA.Columns.Add("271_DL_GUID", GetType(Guid))
                    ISL_AAA.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    ISL_AAA.Columns.Add("271_EB_GUID", GetType(Guid))
                    ISL_AAA.Columns.Add("271_LS_GUID", GetType(Guid))
                    ISL_AAA.Columns.Add("NM1_GUID", GetType(Guid))
                    ISL_AAA.Columns.Add("HL01", GetType(Integer))
                    ISL_AAA.Columns.Add("HL02", GetType(Integer))
                    ISL_AAA.Columns.Add("HL03", GetType(Integer))
                    ISL_AAA.Columns.Add("HL04", GetType(Integer))
                    ISL_AAA.Columns.Add("AAA01", GetType(String))
                    ISL_AAA.Columns.Add("AAA02", GetType(String))
                    ISL_AAA.Columns.Add("AAA03", GetType(String))
                    ISL_AAA.Columns.Add("AAA04", GetType(String))
                    ISL_AAA.Columns.Add("ROW_NUMBER", GetType(Integer))
                    ISL_AAA.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
                    ISL_AAA.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    ISL_AAA.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    ISL_AAA.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
                End Try



                Try
                    IRL_AAA.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                    IRL_AAA.Columns.Add("DOCUMENT_ID", GetType(Integer))
                    IRL_AAA.Columns.Add("FILE_ID", GetType(Integer))
                    IRL_AAA.Columns.Add("BATCH_ID", GetType(Integer))
                    IRL_AAA.Columns.Add("ISA_ID", GetType(Integer))
                    IRL_AAA.Columns.Add("GS_ID", GetType(Integer))
                    IRL_AAA.Columns.Add("ST_ID", GetType(Integer))
                    IRL_AAA.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                    IRL_AAA.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                    IRL_AAA.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                    IRL_AAA.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                    IRL_AAA.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                    IRL_AAA.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                    IRL_AAA.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                    IRL_AAA.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                    IRL_AAA.Columns.Add("271_ISL_GUID", GetType(Guid))
                    IRL_AAA.Columns.Add("271_IRL_GUID", GetType(Guid))
                    IRL_AAA.Columns.Add("271_SL_GUID", GetType(Guid))
                    IRL_AAA.Columns.Add("271_DL_GUID", GetType(Guid))
                    IRL_AAA.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    IRL_AAA.Columns.Add("271_EB_GUID", GetType(Guid))
                    IRL_AAA.Columns.Add("271_LS_GUID", GetType(Guid))
                    IRL_AAA.Columns.Add("NM1_GUID", GetType(Guid))
                    IRL_AAA.Columns.Add("HL01", GetType(Integer))
                    IRL_AAA.Columns.Add("HL02", GetType(Integer))
                    IRL_AAA.Columns.Add("HL03", GetType(Integer))
                    IRL_AAA.Columns.Add("HL04", GetType(Integer))
                    IRL_AAA.Columns.Add("AAA01", GetType(String))
                    IRL_AAA.Columns.Add("AAA02", GetType(String))
                    IRL_AAA.Columns.Add("AAA03", GetType(String))
                    IRL_AAA.Columns.Add("AAA04", GetType(String))
                    IRL_AAA.Columns.Add("ROW_NUMBER", GetType(Integer))
                    IRL_AAA.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
                    IRL_AAA.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    IRL_AAA.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    IRL_AAA.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
                End Try

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
                    AMT.Columns.Add("271_ISL_GUID", GetType(Guid))
                    AMT.Columns.Add("271_IRL_GUID", GetType(Guid))
                    AMT.Columns.Add("271_SL_GUID", GetType(Guid))
                    AMT.Columns.Add("271_DL_GUID", GetType(Guid))
                    AMT.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    AMT.Columns.Add("271_EB_GUID", GetType(Guid))
                    AMT.Columns.Add("271_LS_GUID", GetType(Guid))
                    AMT.Columns.Add("NM1_GUID", GetType(Guid))
                    AMT.Columns.Add("HL01", GetType(Integer))
                    AMT.Columns.Add("HL02", GetType(Integer))
                    AMT.Columns.Add("HL03", GetType(Integer))
                    AMT.Columns.Add("HL04", GetType(Integer))
                    AMT.Columns.Add("AMT01", GetType(String))
                    AMT.Columns.Add("AMT02", GetType(String))
                    AMT.Columns.Add("AMT03", GetType(String))
                    AMT.Columns.Add("ROW_NUMBER", GetType(Integer))
                    AMT.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
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
                    DMG.Columns.Add("271_ISL_GUID", GetType(Guid))
                    DMG.Columns.Add("271_IRL_GUID", GetType(Guid))
                    DMG.Columns.Add("271_SL_GUID", GetType(Guid))
                    DMG.Columns.Add("271_DL_GUID", GetType(Guid))
                    DMG.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    DMG.Columns.Add("271_EB_GUID", GetType(Guid))
                    DMG.Columns.Add("271_LS_GUID", GetType(Guid))
                    DMG.Columns.Add("NM1_GUID", GetType(Guid))
                    DMG.Columns.Add("HL01", GetType(Integer))
                    DMG.Columns.Add("HL02", GetType(Integer))
                    DMG.Columns.Add("HL03", GetType(Integer))
                    DMG.Columns.Add("HL04", GetType(Integer))
                    DMG.Columns.Add("DMG01", GetType(String))
                    DMG.Columns.Add("DMG02", GetType(String))
                    DMG.Columns.Add("DMG03", GetType(String))
                    DMG.Columns.Add("ROW_NUMBER", GetType(Integer))
                    DMG.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
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
                    DTP.Columns.Add("271_ISL_GUID", GetType(Guid))
                    DTP.Columns.Add("271_IRL_GUID", GetType(Guid))
                    DTP.Columns.Add("271_SL_GUID", GetType(Guid))
                    DTP.Columns.Add("271_DL_GUID", GetType(Guid))
                    DTP.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    DTP.Columns.Add("271_EB_GUID", GetType(Guid))
                    DTP.Columns.Add("271_LS_GUID", GetType(Guid))
                    DTP.Columns.Add("NM1_GUID", GetType(Guid))
                    DTP.Columns.Add("HL01", GetType(Integer))
                    DTP.Columns.Add("HL02", GetType(Integer))
                    DTP.Columns.Add("HL03", GetType(Integer))
                    DTP.Columns.Add("HL04", GetType(Integer))
                    DTP.Columns.Add("DTP01", GetType(String))
                    DTP.Columns.Add("DTP02", GetType(String))
                    DTP.Columns.Add("DTP03", GetType(String))
                    DTP.Columns.Add("ROW_NUMBER", GetType(Integer))
                    DTP.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
                    DTP.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    DTP.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    DTP.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
                End Try


                Try
                    EB_DTP.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                    EB_DTP.Columns.Add("DOCUMENT_ID", GetType(Integer))
                    EB_DTP.Columns.Add("FILE_ID", GetType(Integer))
                    EB_DTP.Columns.Add("BATCH_ID", GetType(Integer))
                    EB_DTP.Columns.Add("ISA_ID", GetType(Integer))
                    EB_DTP.Columns.Add("GS_ID", GetType(Integer))
                    EB_DTP.Columns.Add("ST_ID", GetType(Integer))
                    EB_DTP.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                    EB_DTP.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                    EB_DTP.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                    EB_DTP.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                    EB_DTP.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                    EB_DTP.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                    EB_DTP.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                    EB_DTP.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                    EB_DTP.Columns.Add("271_ISL_GUID", GetType(Guid))
                    EB_DTP.Columns.Add("271_IRL_GUID", GetType(Guid))
                    EB_DTP.Columns.Add("271_SL_GUID", GetType(Guid))
                    EB_DTP.Columns.Add("271_DL_GUID", GetType(Guid))
                    EB_DTP.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    EB_DTP.Columns.Add("271_EB_GUID", GetType(Guid))
                    EB_DTP.Columns.Add("271_LS_GUID", GetType(Guid))
                    EB_DTP.Columns.Add("NM1_GUID", GetType(Guid))
                    EB_DTP.Columns.Add("HL01", GetType(Integer))
                    EB_DTP.Columns.Add("HL02", GetType(Integer))
                    EB_DTP.Columns.Add("HL03", GetType(Integer))
                    EB_DTP.Columns.Add("HL04", GetType(Integer))
                    EB_DTP.Columns.Add("DTP01", GetType(String))
                    EB_DTP.Columns.Add("DTP02", GetType(String))
                    EB_DTP.Columns.Add("DTP03", GetType(String))
                    EB_DTP.Columns.Add("ROW_NUMBER", GetType(Integer))
                    EB_DTP.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
                    EB_DTP.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    EB_DTP.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    EB_DTP.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
                End Try



                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   EB
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    EB.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                    EB.Columns.Add("DOCUMENT_ID", GetType(Integer))
                    EB.Columns.Add("FILE_ID", GetType(Integer))
                    EB.Columns.Add("BATCH_ID", GetType(Integer))
                    EB.Columns.Add("ISA_ID", GetType(Integer))
                    EB.Columns.Add("GS_ID", GetType(Integer))
                    EB.Columns.Add("ST_ID", GetType(Integer))
                    EB.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                    EB.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                    EB.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                    EB.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                    EB.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                    EB.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                    EB.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                    EB.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                    EB.Columns.Add("271_ISL_GUID", GetType(Guid))
                    EB.Columns.Add("271_IRL_GUID", GetType(Guid))
                    EB.Columns.Add("271_SL_GUID", GetType(Guid))
                    EB.Columns.Add("271_DL_GUID", GetType(Guid))
                    EB.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    EB.Columns.Add("271_EB_GUID", GetType(Guid))
                    EB.Columns.Add("271_LS_GUID", GetType(Guid))
                    EB.Columns.Add("NM1_GUID", GetType(Guid))
                    EB.Columns.Add("HL01", GetType(Integer))
                    EB.Columns.Add("HL02", GetType(Integer))
                    EB.Columns.Add("HL03", GetType(Integer))
                    EB.Columns.Add("HL04", GetType(Integer))
                    EB.Columns.Add("EB01", GetType(String))
                    EB.Columns.Add("EB02", GetType(String))
                    EB.Columns.Add("EB03", GetType(String))
                    EB.Columns.Add("PEB03", GetType(String))
                    EB.Columns.Add("EB04", GetType(String))
                    EB.Columns.Add("EB05", GetType(String))
                    EB.Columns.Add("EB06", GetType(String))
                    EB.Columns.Add("EB07", GetType(String))
                    EB.Columns.Add("EB08", GetType(String))
                    EB.Columns.Add("EB09", GetType(String))
                    EB.Columns.Add("EB10", GetType(String))
                    EB.Columns.Add("EB11", GetType(String))
                    EB.Columns.Add("EB12", GetType(String))
                    EB.Columns.Add("EB13", GetType(String))
                    EB.Columns.Add("EB13_1", GetType(String))
                    EB.Columns.Add("EB13_2", GetType(String))
                    EB.Columns.Add("EB13_3", GetType(String))
                    EB.Columns.Add("EB13_4", GetType(String))
                    EB.Columns.Add("EB13_5", GetType(String))
                    EB.Columns.Add("EB13_6", GetType(String))
                    EB.Columns.Add("EB13_7", GetType(String))
                    EB.Columns.Add("ROW_NUMBER", GetType(Integer))
                    EB.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
                    EB.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    EB.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    EB.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
                End Try



                Try
                    EB_EB.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                    EB_EB.Columns.Add("DOCUMENT_ID", GetType(Integer))
                    EB_EB.Columns.Add("FILE_ID", GetType(Integer))
                    EB_EB.Columns.Add("BATCH_ID", GetType(Integer))
                    EB_EB.Columns.Add("ISA_ID", GetType(Integer))
                    EB_EB.Columns.Add("GS_ID", GetType(Integer))
                    EB_EB.Columns.Add("ST_ID", GetType(Integer))
                    EB_EB.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                    EB_EB.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                    EB_EB.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                    EB_EB.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                    EB_EB.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                    EB_EB.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                    EB_EB.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                    EB_EB.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                    EB_EB.Columns.Add("271_ISL_GUID", GetType(Guid))
                    EB_EB.Columns.Add("271_IRL_GUID", GetType(Guid))
                    EB_EB.Columns.Add("271_SL_GUID", GetType(Guid))
                    EB_EB.Columns.Add("271_DL_GUID", GetType(Guid))
                    EB_EB.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    EB_EB.Columns.Add("271_EB_GUID", GetType(Guid))
                    EB_EB.Columns.Add("271_LS_GUID", GetType(Guid))
                    EB_EB.Columns.Add("NM1_GUID", GetType(Guid))
                    EB_EB.Columns.Add("HL01", GetType(Integer))
                    EB_EB.Columns.Add("HL02", GetType(Integer))
                    EB_EB.Columns.Add("HL03", GetType(Integer))
                    EB_EB.Columns.Add("HL04", GetType(Integer))
                    EB_EB.Columns.Add("EB01", GetType(String))
                    EB_EB.Columns.Add("EB02", GetType(String))
                    EB_EB.Columns.Add("EB03", GetType(String))
                    EB_EB.Columns.Add("PEB03", GetType(String))
                    EB_EB.Columns.Add("EB04", GetType(String))
                    EB_EB.Columns.Add("EB05", GetType(String))
                    EB_EB.Columns.Add("EB06", GetType(String))
                    EB_EB.Columns.Add("EB07", GetType(String))
                    EB_EB.Columns.Add("EB08", GetType(String))
                    EB_EB.Columns.Add("EB09", GetType(String))
                    EB_EB.Columns.Add("EB10", GetType(String))
                    EB_EB.Columns.Add("EB11", GetType(String))
                    EB_EB.Columns.Add("EB12", GetType(String))
                    EB_EB.Columns.Add("EB13", GetType(String))
                    EB_EB.Columns.Add("EB13_1", GetType(String))
                    EB_EB.Columns.Add("EB13_2", GetType(String))
                    EB_EB.Columns.Add("EB13_3", GetType(String))
                    EB_EB.Columns.Add("EB13_4", GetType(String))
                    EB_EB.Columns.Add("EB13_5", GetType(String))
                    EB_EB.Columns.Add("EB13_6", GetType(String))
                    EB_EB.Columns.Add("EB13_7", GetType(String))
                    EB_EB.Columns.Add("ROW_NUMBER", GetType(Integer))
                    EB_EB.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
                    EB_EB.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    EB_EB.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    EB_EB.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
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
                    HI.Columns.Add("271_ISL_GUID", GetType(Guid))
                    HI.Columns.Add("271_IRL_GUID", GetType(Guid))
                    HI.Columns.Add("271_SL_GUID", GetType(Guid))
                    HI.Columns.Add("271_DL_GUID", GetType(Guid))
                    HI.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    HI.Columns.Add("271_EB_GUID", GetType(Guid))
                    HI.Columns.Add("271_LS_GUID", GetType(Guid))
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
                    HI.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
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
                    HL.Columns.Add("271_ISL_GUID", GetType(Guid))
                    HL.Columns.Add("271_IRL_GUID", GetType(Guid))
                    HL.Columns.Add("271_SL_GUID", GetType(Guid))
                    HL.Columns.Add("271_DL_GUID", GetType(Guid))
                    '      HL.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    HL.Columns.Add("271_EB_GUID", GetType(Guid))
                    HL.Columns.Add("271_LS_GUID", GetType(Guid))
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
                    ISL_HL.Columns.Add("271_ISL_GUID", GetType(Guid))
                    ISL_HL.Columns.Add("271_IRL_GUID", GetType(Guid))
                    ISL_HL.Columns.Add("271_SL_GUID", GetType(Guid))
                    ISL_HL.Columns.Add("271_DL_GUID", GetType(Guid))
                    ' ISL_HL.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    ISL_HL.Columns.Add("271_EB_GUID", GetType(Guid))
                    ISL_HL.Columns.Add("271_LS_GUID", GetType(Guid))
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
                    IRL_HL.Columns.Add("271_ISL_GUID", GetType(Guid))
                    IRL_HL.Columns.Add("271_IRL_GUID", GetType(Guid))
                    IRL_HL.Columns.Add("271_SL_GUID", GetType(Guid))
                    IRL_HL.Columns.Add("271_DL_GUID", GetType(Guid))
                    '  IRL_HL.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    IRL_HL.Columns.Add("271_EB_GUID", GetType(Guid))
                    IRL_HL.Columns.Add("271_LS_GUID", GetType(Guid))
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
                '   HSD
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    HSD.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                    HSD.Columns.Add("DOCUMENT_ID", GetType(Integer))
                    HSD.Columns.Add("FILE_ID", GetType(Integer))
                    HSD.Columns.Add("BATCH_ID", GetType(Integer))
                    HSD.Columns.Add("ISA_ID", GetType(Integer))
                    HSD.Columns.Add("GS_ID", GetType(Integer))
                    HSD.Columns.Add("ST_ID", GetType(Integer))
                    HSD.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                    HSD.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                    HSD.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                    HSD.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                    HSD.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                    HSD.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                    HSD.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                    HSD.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                    HSD.Columns.Add("271_ISL_GUID", GetType(Guid))
                    HSD.Columns.Add("271_IRL_GUID", GetType(Guid))
                    HSD.Columns.Add("271_SL_GUID", GetType(Guid))
                    HSD.Columns.Add("271_DL_GUID", GetType(Guid))
                    HSD.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    HSD.Columns.Add("271_EB_GUID", GetType(Guid))
                    HSD.Columns.Add("271_LS_GUID", GetType(Guid))
                    HSD.Columns.Add("NM1_GUID", GetType(Guid))
                    HSD.Columns.Add("HL01", GetType(Integer))
                    HSD.Columns.Add("HL02", GetType(Integer))
                    HSD.Columns.Add("HL03", GetType(Integer))
                    HSD.Columns.Add("HL04", GetType(Integer))
                    HSD.Columns.Add("HSD01", GetType(String))
                    HSD.Columns.Add("HSD02", GetType(String))
                    HSD.Columns.Add("HSD03", GetType(String))
                    HSD.Columns.Add("HSD04", GetType(String))
                    HSD.Columns.Add("HSD05", GetType(String))
                    HSD.Columns.Add("HSD06", GetType(String))
                    HSD.Columns.Add("HSD07", GetType(String))
                    HSD.Columns.Add("HSD08", GetType(String))
                    HSD.Columns.Add("ROW_NUMBER", GetType(Integer))
                    HSD.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
                    HSD.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    HSD.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    HSD.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
                End Try



                Try
                    EB_HSD.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                    EB_HSD.Columns.Add("DOCUMENT_ID", GetType(Integer))
                    EB_HSD.Columns.Add("FILE_ID", GetType(Integer))
                    EB_HSD.Columns.Add("BATCH_ID", GetType(Integer))
                    EB_HSD.Columns.Add("ISA_ID", GetType(Integer))
                    EB_HSD.Columns.Add("GS_ID", GetType(Integer))
                    EB_HSD.Columns.Add("ST_ID", GetType(Integer))
                    EB_HSD.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                    EB_HSD.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                    EB_HSD.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                    EB_HSD.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                    EB_HSD.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                    EB_HSD.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                    EB_HSD.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                    EB_HSD.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                    EB_HSD.Columns.Add("271_ISL_GUID", GetType(Guid))
                    EB_HSD.Columns.Add("271_IRL_GUID", GetType(Guid))
                    EB_HSD.Columns.Add("271_SL_GUID", GetType(Guid))
                    EB_HSD.Columns.Add("271_DL_GUID", GetType(Guid))
                    EB_HSD.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    EB_HSD.Columns.Add("271_EB_GUID", GetType(Guid))
                    EB_HSD.Columns.Add("271_LS_GUID", GetType(Guid))
                    EB_HSD.Columns.Add("NM1_GUID", GetType(Guid))
                    EB_HSD.Columns.Add("HL01", GetType(Integer))
                    EB_HSD.Columns.Add("HL02", GetType(Integer))
                    EB_HSD.Columns.Add("HL03", GetType(Integer))
                    EB_HSD.Columns.Add("HL04", GetType(Integer))
                    EB_HSD.Columns.Add("HSD01", GetType(String))
                    EB_HSD.Columns.Add("HSD02", GetType(String))
                    EB_HSD.Columns.Add("HSD03", GetType(String))
                    EB_HSD.Columns.Add("HSD04", GetType(String))
                    EB_HSD.Columns.Add("HSD05", GetType(String))
                    EB_HSD.Columns.Add("HSD06", GetType(String))
                    EB_HSD.Columns.Add("HSD07", GetType(String))
                    EB_HSD.Columns.Add("HSD08", GetType(String))
                    EB_HSD.Columns.Add("ROW_NUMBER", GetType(Integer))
                    EB_HSD.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
                    EB_HSD.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    EB_HSD.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    EB_HSD.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
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
                    III.Columns.Add("271_ISL_GUID", GetType(Guid))
                    III.Columns.Add("271_IRL_GUID", GetType(Guid))
                    III.Columns.Add("271_SL_GUID", GetType(Guid))
                    III.Columns.Add("271_DL_GUID", GetType(Guid))
                    III.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    III.Columns.Add("271_EB_GUID", GetType(Guid))
                    III.Columns.Add("271_LS_GUID", GetType(Guid))
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
                    III.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
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
                    INS.Columns.Add("271_ISL_GUID", GetType(Guid))
                    INS.Columns.Add("271_IRL_GUID", GetType(Guid))
                    INS.Columns.Add("271_SL_GUID", GetType(Guid))
                    INS.Columns.Add("271_DL_GUID", GetType(Guid))
                    INS.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    INS.Columns.Add("271_EB_GUID", GetType(Guid))
                    INS.Columns.Add("271_LS_GUID", GetType(Guid))
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
                    INS.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
                    INS.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    INS.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    INS.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
                End Try



                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   LE
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    LE.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                    LE.Columns.Add("DOCUMENT_ID", GetType(Integer))
                    LE.Columns.Add("FILE_ID", GetType(Integer))
                    LE.Columns.Add("BATCH_ID", GetType(Integer))
                    LE.Columns.Add("ISA_ID", GetType(Integer))
                    LE.Columns.Add("GS_ID", GetType(Integer))
                    LE.Columns.Add("ST_ID", GetType(Integer))
                    LE.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                    LE.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                    LE.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                    LE.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                    LE.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                    LE.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                    LE.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                    LE.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                    LE.Columns.Add("271_ISL_GUID", GetType(Guid))
                    LE.Columns.Add("271_IRL_GUID", GetType(Guid))
                    LE.Columns.Add("271_SL_GUID", GetType(Guid))
                    LE.Columns.Add("271_DL_GUID", GetType(Guid))
                    LE.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    LE.Columns.Add("271_EB_GUID", GetType(Guid))
                    LE.Columns.Add("271_LS_GUID", GetType(Guid))
                    LE.Columns.Add("NM1_GUID", GetType(Guid))
                    LE.Columns.Add("HL01", GetType(Integer))
                    LE.Columns.Add("HL02", GetType(Integer))
                    LE.Columns.Add("HL03", GetType(Integer))
                    LE.Columns.Add("HL04", GetType(Integer))
                    LE.Columns.Add("ROW_NUMBER", GetType(Integer))
                    LE.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
                    LE.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    LE.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    LE.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
                End Try


                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   LS
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    LS.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                    LS.Columns.Add("DOCUMENT_ID", GetType(Integer))
                    LS.Columns.Add("FILE_ID", GetType(Integer))
                    LS.Columns.Add("BATCH_ID", GetType(Integer))
                    LS.Columns.Add("ISA_ID", GetType(Integer))
                    LS.Columns.Add("GS_ID", GetType(Integer))
                    LS.Columns.Add("ST_ID", GetType(Integer))
                    LS.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                    LS.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                    LS.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                    LS.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                    LS.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                    LS.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                    LS.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                    LS.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                    LS.Columns.Add("271_ISL_GUID", GetType(Guid))
                    LS.Columns.Add("271_IRL_GUID", GetType(Guid))
                    LS.Columns.Add("271_SL_GUID", GetType(Guid))
                    LS.Columns.Add("271_DL_GUID", GetType(Guid))
                    LS.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    LS.Columns.Add("271_EB_GUID", GetType(Guid))
                    LS.Columns.Add("271_LS_GUID", GetType(Guid))
                    LS.Columns.Add("NM1_GUID", GetType(Guid))
                    LS.Columns.Add("HL01", GetType(Integer))
                    LS.Columns.Add("HL02", GetType(Integer))
                    LS.Columns.Add("HL03", GetType(Integer))
                    LS.Columns.Add("HL04", GetType(Integer))
                    LS.Columns.Add("LS01", GetType(String))
                    LS.Columns.Add("ROW_NUMBER", GetType(Integer))
                    LS.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
                    LS.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    LS.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    LS.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
                End Try



                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   MPI
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    MPI.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                    MPI.Columns.Add("DOCUMENT_ID", GetType(Integer))
                    MPI.Columns.Add("FILE_ID", GetType(Integer))
                    MPI.Columns.Add("BATCH_ID", GetType(Integer))
                    MPI.Columns.Add("ISA_ID", GetType(Integer))
                    MPI.Columns.Add("GS_ID", GetType(Integer))
                    MPI.Columns.Add("ST_ID", GetType(Integer))
                    MPI.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                    MPI.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                    MPI.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                    MPI.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                    MPI.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                    MPI.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                    MPI.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                    MPI.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                    MPI.Columns.Add("271_ISL_GUID", GetType(Guid))
                    MPI.Columns.Add("271_IRL_GUID", GetType(Guid))
                    MPI.Columns.Add("271_SL_GUID", GetType(Guid))
                    MPI.Columns.Add("271_DL_GUID", GetType(Guid))
                    MPI.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    MPI.Columns.Add("271_EB_GUID", GetType(Guid))
                    MPI.Columns.Add("271_LS_GUID", GetType(Guid))
                    MPI.Columns.Add("NM1_GUID", GetType(Guid))
                    MPI.Columns.Add("HL01", GetType(Integer))
                    MPI.Columns.Add("HL02", GetType(Integer))
                    MPI.Columns.Add("HL03", GetType(Integer))
                    MPI.Columns.Add("HL04", GetType(Integer))
                    MPI.Columns.Add("MPI01", GetType(String))
                    MPI.Columns.Add("MPI02", GetType(String))
                    MPI.Columns.Add("MPI03", GetType(String))
                    MPI.Columns.Add("MPI04", GetType(String))
                    MPI.Columns.Add("MPI05", GetType(String))
                    MPI.Columns.Add("MPI06", GetType(String))
                    MPI.Columns.Add("MPI07", GetType(String))
                    MPI.Columns.Add("ROW_NUMBER", GetType(Integer))
                    MPI.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
                    MPI.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    MPI.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    MPI.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
                End Try




                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                '   MSG
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Try
                    MSG.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                    MSG.Columns.Add("DOCUMENT_ID", GetType(Integer))
                    MSG.Columns.Add("FILE_ID", GetType(Integer))
                    MSG.Columns.Add("BATCH_ID", GetType(Integer))
                    MSG.Columns.Add("ISA_ID", GetType(Integer))
                    MSG.Columns.Add("GS_ID", GetType(Integer))
                    MSG.Columns.Add("ST_ID", GetType(Integer))
                    MSG.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                    MSG.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                    MSG.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                    MSG.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                    MSG.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                    MSG.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                    MSG.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                    MSG.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                    MSG.Columns.Add("271_ISL_GUID", GetType(Guid))
                    MSG.Columns.Add("271_IRL_GUID", GetType(Guid))
                    MSG.Columns.Add("271_SL_GUID", GetType(Guid))
                    MSG.Columns.Add("271_DL_GUID", GetType(Guid))
                    MSG.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    MSG.Columns.Add("271_EB_GUID", GetType(Guid))
                    MSG.Columns.Add("271_LS_GUID", GetType(Guid))
                    MSG.Columns.Add("NM1_GUID", GetType(Guid))
                    MSG.Columns.Add("HL01", GetType(Integer))
                    MSG.Columns.Add("HL02", GetType(Integer))
                    MSG.Columns.Add("HL03", GetType(Integer))
                    MSG.Columns.Add("HL04", GetType(Integer))
                    MSG.Columns.Add("MSG01", GetType(String))
                    MSG.Columns.Add("MSG02", GetType(String))
                    MSG.Columns.Add("MSG03", GetType(String))
                    MSG.Columns.Add("MSG04", GetType(String))
                    MSG.Columns.Add("MSG05", GetType(String))
                    MSG.Columns.Add("MSG06", GetType(String))
                    MSG.Columns.Add("MSG07", GetType(String))
                    MSG.Columns.Add("MSG", GetType(String))
                    MSG.Columns.Add("ROW_NUMBER", GetType(Integer))
                    MSG.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
                    MSG.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    MSG.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    MSG.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
                End Try



                Try
                    EB_MSG.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                    EB_MSG.Columns.Add("DOCUMENT_ID", GetType(Integer))
                    EB_MSG.Columns.Add("FILE_ID", GetType(Integer))
                    EB_MSG.Columns.Add("BATCH_ID", GetType(Integer))
                    EB_MSG.Columns.Add("ISA_ID", GetType(Integer))
                    EB_MSG.Columns.Add("GS_ID", GetType(Integer))
                    EB_MSG.Columns.Add("ST_ID", GetType(Integer))
                    EB_MSG.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                    EB_MSG.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                    EB_MSG.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                    EB_MSG.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                    EB_MSG.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                    EB_MSG.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                    EB_MSG.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                    EB_MSG.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                    EB_MSG.Columns.Add("271_ISL_GUID", GetType(Guid))
                    EB_MSG.Columns.Add("271_IRL_GUID", GetType(Guid))
                    EB_MSG.Columns.Add("271_SL_GUID", GetType(Guid))
                    EB_MSG.Columns.Add("271_DL_GUID", GetType(Guid))
                    EB_MSG.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    EB_MSG.Columns.Add("271_EB_GUID", GetType(Guid))
                    EB_MSG.Columns.Add("271_LS_GUID", GetType(Guid))
                    EB_MSG.Columns.Add("NM1_GUID", GetType(Guid))
                    EB_MSG.Columns.Add("HL01", GetType(Integer))
                    EB_MSG.Columns.Add("HL02", GetType(Integer))
                    EB_MSG.Columns.Add("HL03", GetType(Integer))
                    EB_MSG.Columns.Add("HL04", GetType(Integer))
                    EB_MSG.Columns.Add("MSG01", GetType(String))
                    EB_MSG.Columns.Add("MSG02", GetType(String))
                    EB_MSG.Columns.Add("MSG03", GetType(String))
                    EB_MSG.Columns.Add("MSG04", GetType(String))
                    EB_MSG.Columns.Add("MSG05", GetType(String))
                    EB_MSG.Columns.Add("MSG06", GetType(String))
                    EB_MSG.Columns.Add("MSG07", GetType(String))
                    EB_MSG.Columns.Add("ROW_NUMBER", GetType(Integer))
                    EB_MSG.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
                    EB_MSG.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    EB_MSG.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    EB_MSG.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
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
                    N3.Columns.Add("271_ISL_GUID", GetType(Guid))
                    N3.Columns.Add("271_IRL_GUID", GetType(Guid))
                    N3.Columns.Add("271_SL_GUID", GetType(Guid))
                    N3.Columns.Add("271_DL_GUID", GetType(Guid))
                    N3.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    N3.Columns.Add("271_EB_GUID", GetType(Guid))
                    N3.Columns.Add("271_LS_GUID", GetType(Guid))
                    N3.Columns.Add("NM1_GUID", GetType(Guid))
                    N3.Columns.Add("HL01", GetType(Integer))
                    N3.Columns.Add("HL02", GetType(Integer))
                    N3.Columns.Add("HL03", GetType(Integer))
                    N3.Columns.Add("HL04", GetType(Integer))
                    N3.Columns.Add("N301", GetType(String))
                    N3.Columns.Add("N302", GetType(String))
                    N3.Columns.Add("ROW_NUMBER", GetType(Integer))
                    N3.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
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
                    IRL_N3.Columns.Add("271_ISL_GUID", GetType(Guid))
                    IRL_N3.Columns.Add("271_IRL_GUID", GetType(Guid))
                    IRL_N3.Columns.Add("271_SL_GUID", GetType(Guid))
                    IRL_N3.Columns.Add("271_DL_GUID", GetType(Guid))
                    IRL_N3.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    IRL_N3.Columns.Add("271_EB_GUID", GetType(Guid))
                    IRL_N3.Columns.Add("271_LS_GUID", GetType(Guid))
                    IRL_N3.Columns.Add("NM1_GUID", GetType(Guid))
                    IRL_N3.Columns.Add("HL01", GetType(Integer))
                    IRL_N3.Columns.Add("HL02", GetType(Integer))
                    IRL_N3.Columns.Add("HL03", GetType(Integer))
                    IRL_N3.Columns.Add("HL04", GetType(Integer))
                    IRL_N3.Columns.Add("N301", GetType(String))
                    IRL_N3.Columns.Add("N302", GetType(String))
                    IRL_N3.Columns.Add("ROW_NUMBER", GetType(Integer))
                    IRL_N3.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
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
                    N4.Columns.Add("271_ISL_GUID", GetType(Guid))
                    N4.Columns.Add("271_IRL_GUID", GetType(Guid))
                    N4.Columns.Add("271_SL_GUID", GetType(Guid))
                    N4.Columns.Add("271_DL_GUID", GetType(Guid))
                    N4.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    N4.Columns.Add("271_EB_GUID", GetType(Guid))
                    N4.Columns.Add("271_LS_GUID", GetType(Guid))
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
                    N4.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
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
                    IRL_N4.Columns.Add("271_ISL_GUID", GetType(Guid))
                    IRL_N4.Columns.Add("271_IRL_GUID", GetType(Guid))
                    IRL_N4.Columns.Add("271_SL_GUID", GetType(Guid))
                    IRL_N4.Columns.Add("271_DL_GUID", GetType(Guid))
                    IRL_N4.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    IRL_N4.Columns.Add("271_EB_GUID", GetType(Guid))
                    IRL_N4.Columns.Add("271_LS_GUID", GetType(Guid))
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
                    IRL_N4.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
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
                    NM1.Columns.Add("271_ISL_GUID", GetType(Guid))
                    NM1.Columns.Add("271_IRL_GUID", GetType(Guid))
                    NM1.Columns.Add("271_SL_GUID", GetType(Guid))
                    NM1.Columns.Add("271_DL_GUID", GetType(Guid))
                    NM1.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    NM1.Columns.Add("271_EB_GUID", GetType(Guid))
                    NM1.Columns.Add("271_LS_GUID", GetType(Guid))
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
                    NM1.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
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
                    ISL_NM1.Columns.Add("271_ISL_GUID", GetType(Guid))
                    ISL_NM1.Columns.Add("271_IRL_GUID", GetType(Guid))
                    ISL_NM1.Columns.Add("271_SL_GUID", GetType(Guid))
                    ISL_NM1.Columns.Add("271_DL_GUID", GetType(Guid))
                    ISL_NM1.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    ISL_NM1.Columns.Add("271_EB_GUID", GetType(Guid))
                    ISL_NM1.Columns.Add("271_LS_GUID", GetType(Guid))
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
                    ISL_NM1.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
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
                    IRL_NM1.Columns.Add("271_ISL_GUID", GetType(Guid))
                    IRL_NM1.Columns.Add("271_IRL_GUID", GetType(Guid))
                    IRL_NM1.Columns.Add("271_SL_GUID", GetType(Guid))
                    IRL_NM1.Columns.Add("271_DL_GUID", GetType(Guid))
                    IRL_NM1.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    IRL_NM1.Columns.Add("271_EB_GUID", GetType(Guid))
                    IRL_NM1.Columns.Add("271_LS_GUID", GetType(Guid))
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
                    IRL_NM1.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
                    IRL_NM1.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    IRL_NM1.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    IRL_NM1.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
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
                    PER.Columns.Add("271_ISL_GUID", GetType(Guid))
                    PER.Columns.Add("271_IRL_GUID", GetType(Guid))
                    PER.Columns.Add("271_SL_GUID", GetType(Guid))
                    PER.Columns.Add("271_DL_GUID", GetType(Guid))
                    PER.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    PER.Columns.Add("271_EB_GUID", GetType(Guid))
                    PER.Columns.Add("271_LS_GUID", GetType(Guid))
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
                    PER.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
                    PER.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    PER.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    PER.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
                End Try

                Try
                    ISL_PER.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                    ISL_PER.Columns.Add("DOCUMENT_ID", GetType(Integer))
                    ISL_PER.Columns.Add("FILE_ID", GetType(Integer))
                    ISL_PER.Columns.Add("BATCH_ID", GetType(Integer))
                    ISL_PER.Columns.Add("ISA_ID", GetType(Integer))
                    ISL_PER.Columns.Add("GS_ID", GetType(Integer))
                    ISL_PER.Columns.Add("ST_ID", GetType(Integer))
                    ISL_PER.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                    ISL_PER.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                    ISL_PER.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                    ISL_PER.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                    ISL_PER.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                    ISL_PER.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                    ISL_PER.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                    ISL_PER.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                    ISL_PER.Columns.Add("271_ISL_GUID", GetType(Guid))
                    ISL_PER.Columns.Add("271_IRL_GUID", GetType(Guid))
                    ISL_PER.Columns.Add("271_SL_GUID", GetType(Guid))
                    ISL_PER.Columns.Add("271_DL_GUID", GetType(Guid))
                    ISL_PER.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    ISL_PER.Columns.Add("271_EB_GUID", GetType(Guid))
                    ISL_PER.Columns.Add("271_LS_GUID", GetType(Guid))
                    ISL_PER.Columns.Add("NM1_GUID", GetType(Guid))
                    ISL_PER.Columns.Add("HL01", GetType(Integer))
                    ISL_PER.Columns.Add("HL02", GetType(Integer))
                    ISL_PER.Columns.Add("HL03", GetType(Integer))
                    ISL_PER.Columns.Add("HL04", GetType(Integer))
                    ISL_PER.Columns.Add("PER01", GetType(String))
                    ISL_PER.Columns.Add("PER02", GetType(String))
                    ISL_PER.Columns.Add("PER03", GetType(String))
                    ISL_PER.Columns.Add("PER04", GetType(String))
                    ISL_PER.Columns.Add("PER05", GetType(String))
                    ISL_PER.Columns.Add("PER06", GetType(String))
                    ISL_PER.Columns.Add("PER07", GetType(String))
                    ISL_PER.Columns.Add("PER08", GetType(String))
                    ISL_PER.Columns.Add("PER09", GetType(String))
                    ISL_PER.Columns.Add("ROW_NUMBER", GetType(Integer))
                    ISL_PER.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
                    ISL_PER.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    ISL_PER.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    ISL_PER.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
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
                    PRV.Columns.Add("271_ISL_GUID", GetType(Guid))
                    PRV.Columns.Add("271_IRL_GUID", GetType(Guid))
                    PRV.Columns.Add("271_SL_GUID", GetType(Guid))
                    PRV.Columns.Add("271_DL_GUID", GetType(Guid))
                    PRV.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    PRV.Columns.Add("271_EB_GUID", GetType(Guid))
                    PRV.Columns.Add("271_LS_GUID", GetType(Guid))
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
                    PRV.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
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
                    IRL_PRV.Columns.Add("271_ISL_GUID", GetType(Guid))
                    IRL_PRV.Columns.Add("271_IRL_GUID", GetType(Guid))
                    IRL_PRV.Columns.Add("271_SL_GUID", GetType(Guid))
                    IRL_PRV.Columns.Add("271_DL_GUID", GetType(Guid))
                    IRL_PRV.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    IRL_PRV.Columns.Add("271_EB_GUID", GetType(Guid))
                    IRL_PRV.Columns.Add("271_LS_GUID", GetType(Guid))
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
                    IRL_PRV.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
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
                    REF.Columns.Add("271_ISL_GUID", GetType(Guid))
                    REF.Columns.Add("271_IRL_GUID", GetType(Guid))
                    REF.Columns.Add("271_SL_GUID", GetType(Guid))
                    REF.Columns.Add("271_DL_GUID", GetType(Guid))
                    REF.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    REF.Columns.Add("271_EB_GUID", GetType(Guid))
                    REF.Columns.Add("271_LS_GUID", GetType(Guid))
                    REF.Columns.Add("NM1_GUID", GetType(Guid))
                    REF.Columns.Add("HL01", GetType(Integer))
                    REF.Columns.Add("HL02", GetType(Integer))
                    REF.Columns.Add("HL03", GetType(Integer))
                    REF.Columns.Add("HL04", GetType(Integer))
                    REF.Columns.Add("REF01", GetType(String))
                    REF.Columns.Add("REF02", GetType(String))
                    REF.Columns.Add("REF03", GetType(String))
                    REF.Columns.Add("ROW_NUMBER", GetType(Integer))
                    REF.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
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
                    IRL_REF.Columns.Add("271_ISL_GUID", GetType(Guid))
                    IRL_REF.Columns.Add("271_IRL_GUID", GetType(Guid))
                    IRL_REF.Columns.Add("271_SL_GUID", GetType(Guid))
                    IRL_REF.Columns.Add("271_DL_GUID", GetType(Guid))
                    IRL_REF.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    IRL_REF.Columns.Add("271_EB_GUID", GetType(Guid))
                    IRL_REF.Columns.Add("271_LS_GUID", GetType(Guid))
                    IRL_REF.Columns.Add("NM1_GUID", GetType(Guid))
                    IRL_REF.Columns.Add("HL01", GetType(Integer))
                    IRL_REF.Columns.Add("HL02", GetType(Integer))
                    IRL_REF.Columns.Add("HL03", GetType(Integer))
                    IRL_REF.Columns.Add("HL04", GetType(Integer))
                    IRL_REF.Columns.Add("REF01", GetType(String))
                    IRL_REF.Columns.Add("REF02", GetType(String))
                    IRL_REF.Columns.Add("REF03", GetType(String))
                    IRL_REF.Columns.Add("ROW_NUMBER", GetType(Integer))
                    IRL_REF.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
                    IRL_REF.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    IRL_REF.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    IRL_REF.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
                Catch ex As Exception
                    R = -2
                End Try


                Try
                    EB_REF.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                    EB_REF.Columns.Add("DOCUMENT_ID", GetType(Integer))
                    EB_REF.Columns.Add("FILE_ID", GetType(Integer))
                    EB_REF.Columns.Add("BATCH_ID", GetType(Integer))
                    EB_REF.Columns.Add("ISA_ID", GetType(Integer))
                    EB_REF.Columns.Add("GS_ID", GetType(Integer))
                    EB_REF.Columns.Add("ST_ID", GetType(Integer))
                    EB_REF.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                    EB_REF.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                    EB_REF.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                    EB_REF.Columns.Add("HIPAA_HL_20_GUID", GetType(Guid))
                    EB_REF.Columns.Add("HIPAA_HL_21_GUID", GetType(Guid))
                    EB_REF.Columns.Add("HIPAA_HL_22_GUID", GetType(Guid))
                    EB_REF.Columns.Add("HIPAA_HL_23_GUID", GetType(Guid))
                    EB_REF.Columns.Add("HIPAA_HL_24_GUID", GetType(Guid))
                    EB_REF.Columns.Add("271_ISL_GUID", GetType(Guid))
                    EB_REF.Columns.Add("271_IRL_GUID", GetType(Guid))
                    EB_REF.Columns.Add("271_SL_GUID", GetType(Guid))
                    EB_REF.Columns.Add("271_DL_GUID", GetType(Guid))
                    EB_REF.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    EB_REF.Columns.Add("271_EB_GUID", GetType(Guid))
                    EB_REF.Columns.Add("271_LS_GUID", GetType(Guid))
                    EB_REF.Columns.Add("NM1_GUID", GetType(Guid))
                    EB_REF.Columns.Add("HL01", GetType(Integer))
                    EB_REF.Columns.Add("HL02", GetType(Integer))
                    EB_REF.Columns.Add("HL03", GetType(Integer))
                    EB_REF.Columns.Add("HL04", GetType(Integer))
                    EB_REF.Columns.Add("REF01", GetType(String))
                    EB_REF.Columns.Add("REF02", GetType(String))
                    EB_REF.Columns.Add("REF03", GetType(String))
                    EB_REF.Columns.Add("ROW_NUMBER", GetType(Integer))
                    EB_REF.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
                    EB_REF.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                    EB_REF.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                    EB_REF.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
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
                    TRN.Columns.Add("271_ISL_GUID", GetType(Guid))
                    TRN.Columns.Add("271_IRL_GUID", GetType(Guid))
                    TRN.Columns.Add("271_SL_GUID", GetType(Guid))
                    TRN.Columns.Add("271_DL_GUID", GetType(Guid))
                    TRN.Columns.Add("271_EB_GROUP_GUID", GetType(Guid))
                    TRN.Columns.Add("271_EB_GUID", GetType(Guid))
                    TRN.Columns.Add("271_LS_GUID", GetType(Guid))
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
                    TRN.Columns.Add("EB_ROW_NUMBER", GetType(Integer))
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

