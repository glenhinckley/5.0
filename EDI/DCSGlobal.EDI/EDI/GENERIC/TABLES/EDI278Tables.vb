Option Explicit On
Option Strict On
Option Compare Binary





Imports System.Data


Namespace DCSGlobal.EDI

    Public Class EDI278Tables


        Public ENVELOP As New DataTable
        Public N3_N4 As New DataTable

        Public EDI_271 As New DataTable


        'ENVELOP EDI TAbles

        Public ISA As New DataTable
        Public GS As New DataTable
        Public ST As New DataTable

        '278 tables start

        Public BHT As New DataTable
        Public NM1 As New DataTable
        Public REF As New DataTable
        Public N3 As New DataTable
        Public N4 As New DataTable
        Public PER As New DataTable
        Public PRV As New DataTable
        Public UM As New DataTable
        Public DTP As New DataTable

        Public HI As New DataTable
        Public SV1 As New DataTable
        Public SV2 As New DataTable
        Public SV3 As New DataTable
        Public TOO As New DataTable
        Public HL As New DataTable
        Public TRN As New DataTable
        Public PWK As New DataTable
        Public DMG As New DataTable
        Public MSG As New DataTable
        Public CRC As New DataTable
        Public CL1 As New DataTable
        Public CR1 As New DataTable
        Public CR5 As New DataTable


        Public CR6 As New DataTable

        Public AAA As New DataTable
        Public HSD As New DataTable
        Public HCR As New DataTable

        Public INS As New DataTable

        Public UNK As New DataTable

        Public Sub BuildTables()


            'EDI 837i

            ISA.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
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


            GS.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
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



            ST.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            ST.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            ST.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            ST.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            ST.Columns.Add("ST01", GetType(String))
            ST.Columns.Add("ST02", GetType(String))
            ST.Columns.Add("ST03", GetType(String))




            BHT.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            BHT.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            BHT.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            BHT.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            BHT.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            BHT.Columns.Add("BHT01", GetType(String))
            BHT.Columns.Add("BHT02", GetType(String))
            BHT.Columns.Add("BHT03", GetType(String))
            BHT.Columns.Add("BHT04", GetType(String))
            BHT.Columns.Add("BHT05", GetType(String))
            BHT.Columns.Add("BHT06", GetType(String))


            HL.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            HL.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            HL.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            HL.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            HL.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            HL.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            HL.Columns.Add("HL01", GetType(String))
            HL.Columns.Add("HL02", GetType(String))
            HL.Columns.Add("HL03", GetType(String))
            HL.Columns.Add("HL04", GetType(String))




            NM1.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            NM1.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            NM1.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            NM1.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            NM1.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            NM1.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            NM1.Columns.Add("HIPAA_NM1_GUID", GetType(Guid))
            NM1.Columns.Add("HIPAA_HCR_GUID", GetType(Guid))
            NM1.Columns.Add("HIPAA_UM_GUID", GetType(Guid))
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




            HCR.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            HCR.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            HCR.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            HCR.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            HCR.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            HCR.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            HCR.Columns.Add("HIPAA_NM1_GUID", GetType(Guid))
            HCR.Columns.Add("HIPAA_HCR_GUID", GetType(Guid))
            HCR.Columns.Add("HIPAA_UM_GUID", GetType(Guid))
            HCR.Columns.Add("HCR01", GetType(String))
            HCR.Columns.Add("HCR02", GetType(String))
            HCR.Columns.Add("HCR03", GetType(String))



            N3.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            N3.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            N3.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            N3.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            N3.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            N3.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            N3.Columns.Add("HIPAA_NM1_GUID", GetType(Guid))
            N3.Columns.Add("HIPAA_HCR_GUID", GetType(Guid))
            N3.Columns.Add("HIPAA_UM_GUID", GetType(Guid))
            N3.Columns.Add("N301", GetType(String))
            N3.Columns.Add("N302", GetType(String))



            N4.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            N4.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            N4.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            N4.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            N4.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            N4.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            N4.Columns.Add("HIPAA_NM1_GUID", GetType(Guid))
            N4.Columns.Add("HIPAA_HCR_GUID", GetType(Guid))
            N4.Columns.Add("HIPAA_UM_GUID", GetType(Guid))
            N4.Columns.Add("N401", GetType(String))
            N4.Columns.Add("N402", GetType(String))
            N4.Columns.Add("N403", GetType(String))
            N4.Columns.Add("N404", GetType(String))
            N4.Columns.Add("N405", GetType(String))
            N4.Columns.Add("N406", GetType(String))
            N4.Columns.Add("N407", GetType(String))


            REF.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            REF.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            REF.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            REF.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            REF.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            REF.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            REF.Columns.Add("HIPAA_NM1_GUID", GetType(Guid))
            REF.Columns.Add("HIPAA_HCR_GUID", GetType(Guid))
            REF.Columns.Add("HIPAA_UM_GUID", GetType(Guid))
            REF.Columns.Add("REF01", GetType(String))
            REF.Columns.Add("REF02", GetType(String))
            REF.Columns.Add("REF03", GetType(String))


            PER.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            PER.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            PER.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            PER.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            PER.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            PER.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            PER.Columns.Add("HIPAA_NM1_GUID", GetType(Guid))
            PER.Columns.Add("HIPAA_HCR_GUID", GetType(Guid))
            PER.Columns.Add("HIPAA_UM_GUID", GetType(Guid))
            PER.Columns.Add("PER01", GetType(String))
            PER.Columns.Add("PER02", GetType(String))
            PER.Columns.Add("PER03", GetType(String))
            PER.Columns.Add("PER04", GetType(String))
            PER.Columns.Add("PER05", GetType(String))
            PER.Columns.Add("PER06", GetType(String))
            PER.Columns.Add("PER07", GetType(String))
            PER.Columns.Add("PER08", GetType(String))
            PER.Columns.Add("PER09", GetType(String))

            PRV.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            PRV.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            PRV.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            PRV.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            PRV.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            PRV.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            PRV.Columns.Add("HIPAA_NM1_GUID", GetType(Guid))
            PRV.Columns.Add("HIPAA_HCR_GUID", GetType(Guid))
            PRV.Columns.Add("HIPAA_UM_GUID", GetType(Guid))
            PRV.Columns.Add("PRV01", GetType(String))
            PRV.Columns.Add("PRV02", GetType(String))
            PRV.Columns.Add("PRV03", GetType(String))
            PRV.Columns.Add("PRV04", GetType(String))
            PRV.Columns.Add("PRV05", GetType(String))
            PRV.Columns.Add("PRV06", GetType(String))



            UM.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            UM.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            UM.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            UM.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            UM.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            UM.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            UM.Columns.Add("HIPAA_NM1_GUID", GetType(Guid))
            UM.Columns.Add("HIPAA_HCR_GUID", GetType(Guid))
            UM.Columns.Add("HIPAA_UM_GUID", GetType(Guid))
            UM.Columns.Add("UM01", GetType(String))
            UM.Columns.Add("UM02", GetType(String))
            UM.Columns.Add("UM03", GetType(String))
            UM.Columns.Add("UM04", GetType(String))
            UM.Columns.Add("UM04_1", GetType(String))
            UM.Columns.Add("UM04_2", GetType(String))
            UM.Columns.Add("UM05", GetType(String))
            UM.Columns.Add("UM05_1", GetType(String))
            UM.Columns.Add("UM05_2", GetType(String))
            UM.Columns.Add("UM05_3", GetType(String))
            UM.Columns.Add("UM05_4", GetType(String))
            UM.Columns.Add("UM06", GetType(String))
            UM.Columns.Add("UM07", GetType(String))
            UM.Columns.Add("UM08", GetType(String))
            UM.Columns.Add("UM09", GetType(String))
            UM.Columns.Add("UM10", GetType(String))


            DTP.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            DTP.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            DTP.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            DTP.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            DTP.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            DTP.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            DTP.Columns.Add("HIPAA_NM1_GUID", GetType(Guid))
            DTP.Columns.Add("HIPAA_HCR_GUID", GetType(Guid))
            DTP.Columns.Add("HIPAA_UM_GUID", GetType(Guid))
            DTP.Columns.Add("DTP01", GetType(String))
            DTP.Columns.Add("DTP02", GetType(String))
            DTP.Columns.Add("DTP03", GetType(String))



            HI.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            HI.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            HI.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            HI.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            HI.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            HI.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            HI.Columns.Add("HIPAA_NM1_GUID", GetType(Guid))
            HI.Columns.Add("HIPAA_HCR_GUID", GetType(Guid))
            HI.Columns.Add("HIPAA_UM_GUID", GetType(Guid))
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

            SV1.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            SV1.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            SV1.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            SV1.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            SV1.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            SV1.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            SV1.Columns.Add("HIPAA_NM1_GUID", GetType(Guid))
            SV1.Columns.Add("HIPAA_HCR_GUID", GetType(Guid))
            SV1.Columns.Add("HIPAA_UM_GUID", GetType(Guid))
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




            SV2.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            SV2.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            SV2.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            SV2.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            SV2.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            SV2.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            SV2.Columns.Add("HIPAA_NM1_GUID", GetType(Guid))
            SV2.Columns.Add("HIPAA_HCR_GUID", GetType(Guid))
            SV2.Columns.Add("HIPAA_UM_GUID", GetType(Guid))
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


            SV3.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            SV3.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            SV3.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            SV3.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            SV3.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            SV3.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            SV3.Columns.Add("HIPAA_NM1_GUID", GetType(Guid))
            SV3.Columns.Add("HIPAA_HCR_GUID", GetType(Guid))
            SV3.Columns.Add("HIPAA_UM_GUID", GetType(Guid))
            SV3.Columns.Add("SV301", GetType(String))
            SV3.Columns.Add("SV301_1", GetType(String))
            SV3.Columns.Add("SV301_2", GetType(String))
            SV3.Columns.Add("SV301_3", GetType(String))
            SV3.Columns.Add("SV301_4", GetType(String))
            SV3.Columns.Add("SV301_5", GetType(String))
            SV3.Columns.Add("SV301_6", GetType(String))
            SV3.Columns.Add("SV301_7", GetType(String))
            SV3.Columns.Add("SV301_8", GetType(String))
            SV3.Columns.Add("SV302", GetType(String))
            SV3.Columns.Add("SV304", GetType(String))
            SV3.Columns.Add("SV304_1", GetType(String))
            SV3.Columns.Add("SV304_2", GetType(String))
            SV3.Columns.Add("SV304_3", GetType(String))
            SV3.Columns.Add("SV304_4", GetType(String))
            SV3.Columns.Add("SV304_5", GetType(String))
            SV3.Columns.Add("SV305", GetType(String))
            SV3.Columns.Add("SV306", GetType(String))
            SV3.Columns.Add("SV307", GetType(String))


            TOO.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            TOO.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            TOO.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            TOO.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            TOO.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            TOO.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            TOO.Columns.Add("HIPAA_NM1_GUID", GetType(Guid))
            TOO.Columns.Add("HIPAA_HCR_GUID", GetType(Guid))
            TOO.Columns.Add("HIPAA_UM_GUID", GetType(Guid))
            TOO.Columns.Add("TOO01", GetType(String))
            TOO.Columns.Add("TOO02", GetType(String))
            TOO.Columns.Add("TOO03", GetType(String))
            TOO.Columns.Add("TOO03_1", GetType(String))
            TOO.Columns.Add("TOO03_2", GetType(String))
            TOO.Columns.Add("TOO03_3", GetType(String))
            TOO.Columns.Add("TOO03_4", GetType(String))
            TOO.Columns.Add("TOO03_5", GetType(String))



            TRN.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            TRN.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            TRN.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            TRN.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            TRN.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            TRN.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            TRN.Columns.Add("HIPAA_NM1_GUID", GetType(Guid))
            TRN.Columns.Add("HIPAA_HCR_GUID", GetType(Guid))
            TRN.Columns.Add("HIPAA_UM_GUID", GetType(Guid))
            TRN.Columns.Add("TRN01", GetType(String))
            TRN.Columns.Add("TRN02", GetType(String))
            TRN.Columns.Add("TRN03", GetType(String))
            TRN.Columns.Add("TRN04", GetType(String))



            PWK.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            PWK.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            PWK.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            PWK.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            PWK.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            PWK.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            PWK.Columns.Add("HIPAA_NM1_GUID", GetType(Guid))
            PWK.Columns.Add("HIPAA_HCR_GUID", GetType(Guid))
            PWK.Columns.Add("HIPAA_UM_GUID", GetType(Guid))
            PWK.Columns.Add("PWK01", GetType(String))
            PWK.Columns.Add("PWK02", GetType(String))
            'PWK.Columns.Add("PWK03", GetType(String))
            'PWK.Columns.Add("PWK04", GetType(String))
            PWK.Columns.Add("PWK05", GetType(String))
            PWK.Columns.Add("PWK06", GetType(String))
            PWK.Columns.Add("PWK07", GetType(String))
            'PWK.Columns.Add("PWK08", GetType(String))
            'PWK.Columns.Add("PWK09", GetType(String))




            DMG.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            DMG.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            DMG.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            DMG.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            DMG.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            DMG.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            DMG.Columns.Add("HIPAA_NM1_GUID", GetType(Guid))
            DMG.Columns.Add("HIPAA_HCR_GUID", GetType(Guid))
            DMG.Columns.Add("HIPAA_UM_GUID", GetType(Guid))
            DMG.Columns.Add("DMG01", GetType(String))
            DMG.Columns.Add("DMG02", GetType(String))
            DMG.Columns.Add("DMG03", GetType(String))

            MSG.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            MSG.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            MSG.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            MSG.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            MSG.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            MSG.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            MSG.Columns.Add("HIPAA_NM1_GUID", GetType(Guid))
            MSG.Columns.Add("HIPAA_HCR_GUID", GetType(Guid))
            MSG.Columns.Add("HIPAA_UM_GUID", GetType(Guid))
            MSG.Columns.Add("MSG01", GetType(String))
            MSG.Columns.Add("MSG02", GetType(String))
            MSG.Columns.Add("MSG03", GetType(String))


            CRC.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            CRC.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            CRC.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            CRC.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            CRC.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            CRC.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            CRC.Columns.Add("HIPAA_NM1_GUID", GetType(Guid))
            CRC.Columns.Add("HIPAA_HCR_GUID", GetType(Guid))
            CRC.Columns.Add("HIPAA_UM_GUID", GetType(Guid))
            CRC.Columns.Add("CRC01", GetType(String))
            CRC.Columns.Add("CRC02", GetType(String))
            CRC.Columns.Add("CRC03", GetType(String))
            CRC.Columns.Add("CRC04", GetType(String))
            CRC.Columns.Add("CRC05", GetType(String))
            CRC.Columns.Add("CRC06", GetType(String))
            CRC.Columns.Add("CRC07", GetType(String))

            CL1.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            CL1.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            CL1.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            CL1.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            CL1.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            CL1.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            CL1.Columns.Add("HIPAA_NM1_GUID", GetType(Guid))
            CL1.Columns.Add("HIPAA_HCR_GUID", GetType(Guid))
            CL1.Columns.Add("HIPAA_UM_GUID", GetType(Guid))
            CL1.Columns.Add("CL101", GetType(String))
            CL1.Columns.Add("CL102", GetType(String))
            CL1.Columns.Add("CL103", GetType(String))

            CR1.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            CR1.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            CR1.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            CR1.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            CR1.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            CR1.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            CR1.Columns.Add("HIPAA_NM1_GUID", GetType(Guid))
            CR1.Columns.Add("HIPAA_HCR_GUID", GetType(Guid))
            CR1.Columns.Add("HIPAA_UM_GUID", GetType(Guid))
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



            CR5.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            CR5.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            CR5.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            CR5.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            CR5.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            CR5.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            CR5.Columns.Add("HIPAA_NM1_GUID", GetType(Guid))
            CR5.Columns.Add("HIPAA_HCR_GUID", GetType(Guid))
            CR5.Columns.Add("HIPAA_UM_GUID", GetType(Guid))
            CR5.Columns.Add("CR501", GetType(String))
            CR5.Columns.Add("CR502", GetType(String))
            CR5.Columns.Add("CR503", GetType(String))
            CR5.Columns.Add("CR504", GetType(String))
            CR5.Columns.Add("CR505", GetType(String))
            CR5.Columns.Add("CR506", GetType(String))
            CR5.Columns.Add("CR507", GetType(String))
            CR5.Columns.Add("CR508", GetType(String))
            CR5.Columns.Add("CR509", GetType(String))
            CR5.Columns.Add("CR510", GetType(String))
            CR5.Columns.Add("CR511", GetType(String))
            CR5.Columns.Add("CR512", GetType(String))
            CR5.Columns.Add("CR513", GetType(String))
            CR5.Columns.Add("CR514", GetType(String))
            CR5.Columns.Add("CR515", GetType(String))
            CR5.Columns.Add("CR516", GetType(String))
            CR5.Columns.Add("CR517", GetType(String))
            CR5.Columns.Add("CR518", GetType(String))



            CR6.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            CR6.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            CR6.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            CR6.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            CR6.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            CR6.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            CR6.Columns.Add("HIPAA_NM1_GUID", GetType(Guid))
            CR6.Columns.Add("HIPAA_HCR_GUID", GetType(Guid))
            CR6.Columns.Add("HIPAA_UM_GUID", GetType(Guid))
            CR6.Columns.Add("CR601", GetType(String))
            CR6.Columns.Add("CR602", GetType(String))
            CR6.Columns.Add("CR603", GetType(String))
            CR6.Columns.Add("CR604", GetType(String))
            CR6.Columns.Add("CR605", GetType(String))
            CR6.Columns.Add("CR606", GetType(String))
            CR6.Columns.Add("CR607", GetType(String))
            CR6.Columns.Add("CR608", GetType(String))
            CR6.Columns.Add("CR609", GetType(String))
            CR6.Columns.Add("CR610", GetType(String))
            CR6.Columns.Add("CR611", GetType(String))
            CR6.Columns.Add("CR612", GetType(String))
            CR6.Columns.Add("CR613", GetType(String))
            CR6.Columns.Add("CR614", GetType(String))
            CR6.Columns.Add("CR615", GetType(String))
            CR6.Columns.Add("CR616", GetType(String))
            CR6.Columns.Add("CR617", GetType(String))
            CR6.Columns.Add("CR618", GetType(String))
            CR6.Columns.Add("CR619", GetType(String))
            CR6.Columns.Add("CR620", GetType(String))
            CR6.Columns.Add("CR621", GetType(String))


            AAA.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            AAA.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            AAA.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            AAA.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            AAA.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            AAA.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            AAA.Columns.Add("HIPAA_NM1_GUID", GetType(Guid))
            AAA.Columns.Add("HIPAA_HCR_GUID", GetType(Guid))
            AAA.Columns.Add("HIPAA_UM_GUID", GetType(Guid))
            AAA.Columns.Add("AAA01", GetType(String))
            AAA.Columns.Add("AAA02", GetType(String))
            AAA.Columns.Add("AAA03", GetType(String))
            AAA.Columns.Add("AAA04", GetType(String))


            HSD.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            HSD.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            HSD.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            HSD.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            HSD.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            HSD.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            HSD.Columns.Add("HIPAA_NM1_GUID", GetType(Guid))
            HSD.Columns.Add("HIPAA_HCR_GUID", GetType(Guid))
            HSD.Columns.Add("HIPAA_UM_GUID", GetType(Guid))
            HSD.Columns.Add("HSD01", GetType(String))
            HSD.Columns.Add("HSD02", GetType(String))
            HSD.Columns.Add("HSD03", GetType(String))
            HSD.Columns.Add("HSD04", GetType(String))
            HSD.Columns.Add("HSD05", GetType(String))
            HSD.Columns.Add("HSD06", GetType(String))
            HSD.Columns.Add("HSD07", GetType(String))
            HSD.Columns.Add("HSD08", GetType(String))





            INS.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            INS.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            INS.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            INS.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            INS.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            INS.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            INS.Columns.Add("HIPAA_NM1_GUID", GetType(Guid))
            INS.Columns.Add("HIPAA_HCR_GUID", GetType(Guid))
            INS.Columns.Add("HIPAA_UM_GUID", GetType(Guid))
            INS.Columns.Add("INS01", GetType(String))
            INS.Columns.Add("INS02", GetType(String))
            INS.Columns.Add("INS08", GetType(String))



            UNK.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            UNK.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            UNK.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            UNK.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            UNK.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            UNK.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            UNK.Columns.Add("ROW_RECORD_TYPE", GetType(String))
            UNK.Columns.Add("ROW_DATA", GetType(String))


            'end 835 tables


        End Sub
    End Class
End Namespace
