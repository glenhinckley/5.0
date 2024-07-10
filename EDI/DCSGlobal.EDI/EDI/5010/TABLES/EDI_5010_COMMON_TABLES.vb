Option Explicit On
Option Strict On
Option Compare Binary

Imports System.Data



Namespace DCSGlobal.EDI


    Public Class EDI_5010_COMMON_TABLES

        Inherits EDI_5010_COMMON_DECS


        '******************************************************************************************************************
        '  all vars not declared here are in   EDI_5010_COMMON_DECS   it is inhreted from the tables for this type
        '  if you are going to use it on this class only it goes below if not put it in EDI_5010_COMMON_DECS
        '******************************************************************************************************************



        Public ISA As New DataTable
        Public GS As New DataTable
        Public ST As New DataTable

        Public IEA As New DataTable
        Public GE As New DataTable
        Public SE As New DataTable

        Public IEA_N As New DataTable
        Public GE_N As New DataTable
        Public SE_N As New DataTable

        Public BHT As New DataTable


        Public UNK As New DataTable

        Private _ERRROR_STRING As String = String.Empty


        Public Function BuildCommonTables() As Integer

            Dim R = 0



            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   ISA
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try

                ISA.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                ISA.Columns.Add("DOCUMENT_ID", GetType(Integer))
                ISA.Columns.Add("FILE_ID", GetType(Integer))
                ISA.Columns.Add("BATCH_ID", GetType(Integer))
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
                R = -1

            End Try



            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   IEA
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                IEA.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                IEA.Columns.Add("FILE_ID", GetType(Integer))
                IEA.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                IEA.Columns.Add("IEA01", GetType(Integer))
                IEA.Columns.Add("IEA02", GetType(String))
                IEA.Columns.Add("ROW_NUMBER", GetType(Integer))
            Catch ex As Exception
                R = -1
            End Try


            Try
                IEA_N.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                IEA_N.Columns.Add("FILE_ID", GetType(Integer))
                IEA_N.Columns.Add("BATCH_ID", GetType(Integer))
                IEA_N.Columns.Add("ISA_ID", GetType(Integer))
                IEA_N.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                IEA_N.Columns.Add("IEA01", GetType(Integer))
                IEA_N.Columns.Add("IEA02", GetType(String))
                IEA_N.Columns.Add("ROW_NUMBER", GetType(Integer))
            Catch ex As Exception
                R = -1
            End Try


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   GS
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                GS.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                GS.Columns.Add("DOCUMENT_ID", GetType(Integer))
                GS.Columns.Add("FILE_ID", GetType(Integer))
                GS.Columns.Add("BATCH_ID", GetType(Integer))
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


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   GE
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                GE.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                GE.Columns.Add("FILE_ID", GetType(Integer))
                GE.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                GE.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                GE.Columns.Add("GE01", GetType(Integer))
                GE.Columns.Add("GE02", GetType(String))
                GE.Columns.Add("ROW_NUMBER", GetType(Integer))
            Catch ex As Exception
                R = -1
            End Try



            Try
                GE_N.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                GE_N.Columns.Add("FILE_ID", GetType(Integer))
                GE_N.Columns.Add("BATCH_ID", GetType(Integer))
                GE_N.Columns.Add("ISA_ID", GetType(Integer))
                GE_N.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                GE_N.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                GE_N.Columns.Add("GE01", GetType(Integer))
                GE_N.Columns.Add("GE02", GetType(String))
                GE_N.Columns.Add("ROW_NUMBER", GetType(Integer))
            Catch ex As Exception
                R = -1
            End Try

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   ST
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try

                ST.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                ST.Columns.Add("DOCUMENT_ID", GetType(Integer))
                ST.Columns.Add("FILE_ID", GetType(Integer))
                ST.Columns.Add("BATCH_ID", GetType(Integer))
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
                R = -1
            End Try



            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   SE
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                SE.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                SE.Columns.Add("FILE_ID", GetType(Integer))
                SE.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                SE.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                SE.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                SE.Columns.Add("SE01", GetType(Integer))
                SE.Columns.Add("SE02", GetType(String))
                SE.Columns.Add("ROW_NUMBER", GetType(Integer))
            Catch ex As Exception
                R = -1
            End Try

            Try
                SE_N.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                SE_N.Columns.Add("FILE_ID", GetType(Integer))
                SE_N.Columns.Add("BATCH_ID", GetType(Integer))
                SE_N.Columns.Add("ISA_ID", GetType(Integer))
                SE_N.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
                SE_N.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
                SE_N.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
                SE_N.Columns.Add("SE01", GetType(Integer))
                SE_N.Columns.Add("SE02", GetType(String))
                SE_N.Columns.Add("ROW_NUMBER", GetType(Integer))
            Catch ex As Exception
                R = -1
            End Try

  



            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   BHT
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Try
                BHT.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                BHT.Columns.Add("DOCUMENT_ID", GetType(Integer))
                BHT.Columns.Add("FILE_ID", GetType(Integer))
                BHT.Columns.Add("BATCH_ID", GetType(Integer))
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
                R = -1
            End Try





            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            '   UNK
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
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
                UNK.Columns.Add("HIPAA_HL_19_GUID", GetType(Guid))
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
                UNK.Columns.Add("LOOP_LEVEL_MAJOR", GetType(Integer))
                UNK.Columns.Add("LOOP_LEVEL_MINOR", GetType(Integer))
                UNK.Columns.Add("LOOP_LEVEL_SUBFIX", GetType(String))
            Catch ex As Exception
                R = -1
            End Try


            Return R
        End Function




    End Class

End Namespace

