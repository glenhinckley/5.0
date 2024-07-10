Option Explicit On
Option Strict On
Option Compare Binary





Imports System.Data


Namespace DCSGlobal.EDI


    Public Class EDI270Tables

        Inherits EDICommonTables

        Public EQ As New DataTable

        Public AMT As New DataTable
        Public DMG As New DataTable
        Public DTP As New DataTable

        Public III As New DataTable
        Public INS As New DataTable

        Public N3 As New DataTable
        Public N4 As New DataTable
        Public NM1 As New DataTable

        Public PRV As New DataTable

        Public REF As New DataTable

        Public TRN As New DataTable


        Public Sub BuildTables()

            BuildCommonTables()



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
                AMT.Columns.Add("HL01", GetType(Integer))
                AMT.Columns.Add("HL02", GetType(Integer))
                AMT.Columns.Add("HL03", GetType(Integer))
                AMT.Columns.Add("HL04", GetType(Integer))
                AMT.Columns.Add("AMT01", GetType(String))
                AMT.Columns.Add("AMT02", GetType(String))
                AMT.Columns.Add("AMT03", GetType(String))
                AMT.Columns.Add("ROW_NUMBER", GetType(Integer))
            Catch ex As Exception

            End Try




            Try

                DMG.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                DMG.Columns.Add("DOCUMENT_ID", GetType(Integer))
                DMG.Columns.Add("BATCH_ID", GetType(Integer))
                DMG.Columns.Add("FILE_ID", GetType(Integer))
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
                DMG.Columns.Add("HL01", GetType(Integer))
                DMG.Columns.Add("HL02", GetType(Integer))
                DMG.Columns.Add("HL03", GetType(Integer))
                DMG.Columns.Add("HL04", GetType(Integer))
                DMG.Columns.Add("DMG01", GetType(String))
                DMG.Columns.Add("DMG02", GetType(String))
                DMG.Columns.Add("DMG03", GetType(String))
                DMG.Columns.Add("ROW_NUMBER", GetType(Integer))
            Catch ex As Exception

            End Try


            Try
                DTP.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                DTP.Columns.Add("DOCUMENT_ID", GetType(Integer))
                DTP.Columns.Add("BATCH_ID", GetType(Integer))
                DTP.Columns.Add("FILE_ID", GetType(Integer))
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
                DTP.Columns.Add("HL01", GetType(Integer))
                DTP.Columns.Add("HL02", GetType(Integer))
                DTP.Columns.Add("HL03", GetType(Integer))
                DTP.Columns.Add("HL04", GetType(Integer))
                DTP.Columns.Add("DTP01", GetType(String))
                DTP.Columns.Add("DTP02", GetType(String))
                DTP.Columns.Add("DTP03", GetType(String))
                DTP.Columns.Add("ROW_NUMBER", GetType(Integer))
            Catch ex As Exception

            End Try


            Try
                III.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                III.Columns.Add("DOCUMENT_ID", GetType(Integer))
                III.Columns.Add("BATCH_ID", GetType(Integer))
                III.Columns.Add("FILE_ID", GetType(Integer))
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
            Catch ex As Exception

            End Try






            Try
                INS.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                INS.Columns.Add("DOCUMENT_ID", GetType(Integer))
                INS.Columns.Add("BATCH_ID", GetType(Integer))
                INS.Columns.Add("FILE_ID", GetType(Integer))
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


            Catch ex As Exception

            End Try










            Try
                N3.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                N3.Columns.Add("DOCUMENT_ID", GetType(Integer))
                N3.Columns.Add("BATCH_ID", GetType(Integer))
                N3.Columns.Add("FILE_ID", GetType(Integer))
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
                N3.Columns.Add("HL01", GetType(Integer))
                N3.Columns.Add("HL02", GetType(Integer))
                N3.Columns.Add("HL03", GetType(Integer))
                N3.Columns.Add("HL04", GetType(Integer))
                N3.Columns.Add("N301", GetType(String))
                N3.Columns.Add("N302", GetType(String))
                N3.Columns.Add("ROW_NUMBER", GetType(Integer))
            Catch ex As Exception

            End Try


            Try

                N4.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                N4.Columns.Add("DOCUMENT_ID", GetType(Integer))
                N4.Columns.Add("BATCH_ID", GetType(Integer))
                N4.Columns.Add("FILE_ID", GetType(Integer))
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
            Catch ex As Exception

            End Try








            Try
                NM1.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
                NM1.Columns.Add("DOCUMENT_ID", GetType(Integer))
                NM1.Columns.Add("BATCH_ID", GetType(Integer))
                NM1.Columns.Add("FILE_ID", GetType(Integer))
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

            Catch ex As Exception

            End Try







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

            Catch ex As Exception

            End Try



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
                REF.Columns.Add("HL01", GetType(Integer))
                REF.Columns.Add("HL02", GetType(Integer))
                REF.Columns.Add("HL03", GetType(Integer))
                REF.Columns.Add("HL04", GetType(Integer))
                REF.Columns.Add("REF01", GetType(String))
                REF.Columns.Add("REF02", GetType(String))
                REF.Columns.Add("REF03", GetType(String))
                REF.Columns.Add("ROW_NUMBER", GetType(Integer))

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
                HL.Columns.Add("HL01", GetType(Integer))
                HL.Columns.Add("HL02", GetType(Integer))
                HL.Columns.Add("HL03", GetType(Integer))
                HL.Columns.Add("HL04", GetType(Integer))
                HL.Columns.Add("ROW_NUMBER", GetType(Integer))
            Catch ex As Exception

            End Try



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
                TRN.Columns.Add("HL01", GetType(Integer))
                TRN.Columns.Add("HL02", GetType(Integer))
                TRN.Columns.Add("HL03", GetType(Integer))
                TRN.Columns.Add("HL04", GetType(Integer))
                TRN.Columns.Add("TRN01", GetType(String))
                TRN.Columns.Add("TRN02", GetType(String))
                TRN.Columns.Add("TRN03", GetType(String))
                TRN.Columns.Add("TRN04", GetType(String))
                TRN.Columns.Add("ROW_NUMBER", GetType(Integer))
            Catch ex As Exception

            End Try







        End Sub




    End Class

End Namespace

