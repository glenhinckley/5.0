
Option Explicit On
Option Strict On
Option Compare Binary

Imports System.Data


Namespace DCSGlobal.EDI

    Public Class EDI277Tables




        'ENVELOP EDI TAbles

        Public ISA As New DataTable
        Public GS As New DataTable
        Public ST As New DataTable

        '278 tables start

        Public BHT As New DataTable
        Public NM1 As New DataTable
        Public REF As New DataTable
        Public PER As New DataTable
        Public STC As New DataTable
        Public DTP As New DataTable
        Public SVC As New DataTable


        Public HL As New DataTable
        Public TRN As New DataTable

        Public DMG As New DataTable

        Public UNK As New DataTable

        Public Sub BuildTables()




            ISA.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            ISA.Columns.Add("ROW_NUM", GetType(Integer))
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
            ' ISA.Columns.Add("FILE_ID", GetType(String))
            ' ISA.Columns.Add("BATCH_ID", GetType(String))



            GS.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            GS.Columns.Add("ROW_NUM", GetType(Integer))
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
            ' GS.Columns.Add("FILE_ID", GetType(String))
            ' GS.Columns.Add("BATCH_ID", GetType(String))
            ' GS.Columns.Add("GS_ID", GetType(String))



            ST.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            ST.Columns.Add("ROW_NUM", GetType(Integer))
            ST.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            ST.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            ST.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            ST.Columns.Add("ST01", GetType(String))
            ST.Columns.Add("ST02", GetType(String))
            ST.Columns.Add("ST03", GetType(String))
            'Me.ST.Columns.Add("FILE_ID", GetType(String))
            'Me.ST.Columns.Add("BATCH_ID", GetType(String))
            'Me.ST.Columns.Add("GS_ID", GetType(String))
            'Me.ST.Columns.Add("ST_ID", GetType(String))



            BHT.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            BHT.Columns.Add("ROW_NUM", GetType(Integer))
            BHT.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            BHT.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            BHT.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            BHT.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            BHT.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            BHT.Columns.Add("HIPAA_STC_GUID", GetType(Guid))
            BHT.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            BHT.Columns.Add("HL_PARENT", GetType(String))
            BHT.Columns.Add("BHT01", GetType(String))
            BHT.Columns.Add("BHT02", GetType(String))
            BHT.Columns.Add("BHT03", GetType(String))
            BHT.Columns.Add("BHT04", GetType(String))
            BHT.Columns.Add("BHT05", GetType(String))
            BHT.Columns.Add("BHT06", GetType(String))
            'Me.BHT.Columns.Add("FILE_ID", GetType(String))
            'Me.BHT.Columns.Add("BATCH_ID", GetType(String))
            'Me.BHT.Columns.Add("GS_ID", GetType(String))
            'Me.BHT.Columns.Add("ST_ID", GetType(String))



            HL.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            HL.Columns.Add("ROW_NUM", GetType(Integer))
            HL.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            HL.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            HL.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            HL.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            HL.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            HL.Columns.Add("HIPAA_STC_GUID", GetType(Guid))
            HL.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            HL.Columns.Add("HL_PARENT", GetType(String))
            HL.Columns.Add("HL01", GetType(String))
            HL.Columns.Add("HL02", GetType(String))
            HL.Columns.Add("HL03", GetType(String))
            HL.Columns.Add("HL04", GetType(String))
            'Me.HL.Columns.Add("FILE_ID", GetType(String))
            ' Me.HL.Columns.Add("BATCH_ID", GetType(String))
            ' Me.HL.Columns.Add("GS_ID", GetType(String))
            ' Me.HL.Columns.Add("ST_ID", GetType(String))

            STC.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            STC.Columns.Add("ROW_NUM", GetType(Integer))
            STC.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            STC.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            STC.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            STC.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            STC.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            STC.Columns.Add("HIPAA_STC_GUID", GetType(Guid))
            STC.Columns.Add("HIPAA_P_STC_GUID", GetType(Guid))
            STC.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            STC.Columns.Add("HL_PARENT", GetType(String))
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
            'Me.STC.Columns.Add("FILE_ID", GetType(String))
            'Me.STC.Columns.Add("BATCH_ID", GetType(String))
            'Me.STC.Columns.Add("GS_ID", GetType(String))
            'Me.STC.Columns.Add("ST_ID", GetType(String))
            'Me.STC.Columns.Add("STC_ID", GetType(String))


            NM1.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            NM1.Columns.Add("ROW_NUM", GetType(Integer))
            NM1.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            NM1.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            NM1.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            NM1.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            NM1.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            NM1.Columns.Add("HIPAA_STC_GUID", GetType(Guid))
            NM1.Columns.Add("HIPAA_P_STC_GUID", GetType(Guid))
            NM1.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            NM1.Columns.Add("HL_PARENT", GetType(String))
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
            ' Me.NM1.Columns.Add("FILE_ID", GetType(String))
            ' Me.NM1.Columns.Add("BATCH_ID", GetType(String))
            ' Me.NM1.Columns.Add("GS_ID", GetType(String))
            ' Me.NM1.Columns.Add("ST_ID", GetType(String))
            ' Me.NM1.Columns.Add("STC_ID", GetType(String))


            DMG.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            DMG.Columns.Add("ROW_NUM", GetType(Integer))
            DMG.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            DMG.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            DMG.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            DMG.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            DMG.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            DMG.Columns.Add("HIPAA_STC_GUID", GetType(Guid))
            DMG.Columns.Add("HIPAA_P_STC_GUID", GetType(Guid))
            DMG.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            DMG.Columns.Add("HL_PARENT", GetType(String))
            DMG.Columns.Add("DMG01", GetType(String))
            DMG.Columns.Add("DMG02", GetType(String))
            DMG.Columns.Add("DMG03", GetType(String))
            ' Me.DMG.Columns.Add("FILE_ID", GetType(String))
            ' Me.DMG.Columns.Add("BATCH_ID", GetType(String))
            ' Me.DMG.Columns.Add("GS_ID", GetType(String))
            ' Me.DMG.Columns.Add("ST_ID", GetType(String))
            ' Me.DMG.Columns.Add("STC_ID", GetType(String))



            REF.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            REF.Columns.Add("ROW_NUM", GetType(Integer))
            REF.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            REF.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            REF.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            REF.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            REF.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            REF.Columns.Add("HIPAA_STC_GUID", GetType(Guid))
            REF.Columns.Add("HIPAA_P_STC_GUID", GetType(Guid))
            REF.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            REF.Columns.Add("HL_PARENT", GetType(String))
            REF.Columns.Add("REF01", GetType(String))
            REF.Columns.Add("REF02", GetType(String))
            REF.Columns.Add("REF03", GetType(String))
            '.REF.Columns.Add("FILE_ID", GetType(String))
            '.REF.Columns.Add("BATCH_ID", GetType(String))
            '.REF.Columns.Add("GS_ID", GetType(String))
            '.REF.Columns.Add("ST_ID", GetType(String))
            '.REF.Columns.Add("STC_ID", GetType(String))

            PER.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            PER.Columns.Add("ROW_NUM", GetType(Integer))
            PER.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            PER.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            PER.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            PER.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            PER.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            PER.Columns.Add("HIPAA_STC_GUID", GetType(Guid))
            PER.Columns.Add("HIPAA_P_STC_GUID", GetType(Guid))
            PER.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            PER.Columns.Add("HL_PARENT", GetType(String))
            PER.Columns.Add("PER01", GetType(String))
            PER.Columns.Add("PER02", GetType(String))
            PER.Columns.Add("PER03", GetType(String))
            PER.Columns.Add("PER04", GetType(String))
            PER.Columns.Add("PER05", GetType(String))
            PER.Columns.Add("PER06", GetType(String))
            PER.Columns.Add("PER07", GetType(String))
            PER.Columns.Add("PER08", GetType(String))
            PER.Columns.Add("PER09", GetType(String))
            '.PER.Columns.Add("FILE_ID", GetType(String))
            '.PER.Columns.Add("BATCH_ID", GetType(String))
            '.PER.Columns.Add("GS_ID", GetType(String))
            '.PER.Columns.Add("ST_ID", GetType(String))
            '.PER.Columns.Add("STC_ID", GetType(String))


            DTP.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            DTP.Columns.Add("ROW_NUM", GetType(Integer))
            DTP.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            DTP.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            DTP.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            DTP.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            DTP.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            DTP.Columns.Add("HIPAA_STC_GUID", GetType(Guid))
            DTP.Columns.Add("HIPAA_P_STC_GUID", GetType(Guid))
            DTP.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            DTP.Columns.Add("HL_PARENT", GetType(String))
            DTP.Columns.Add("DTP01", GetType(String))
            DTP.Columns.Add("DTP02", GetType(String))
            DTP.Columns.Add("DTP03", GetType(String))
            '.DTP.Columns.Add("FILE_ID", GetType(String))
            '.DTP.Columns.Add("BATCH_ID", GetType(String))
            '.DTP.Columns.Add("GS_ID", GetType(String))
            '.DTP.Columns.Add("ST_ID", GetType(String))
            '.DTP.Columns.Add("STC_ID", GetType(String))


            SVC.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            SVC.Columns.Add("ROW_NUM", GetType(Integer))
            SVC.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            SVC.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            SVC.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            SVC.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            SVC.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            SVC.Columns.Add("HIPAA_STC_GUID", GetType(Guid))
            SVC.Columns.Add("HIPAA_P_STC_GUID", GetType(Guid))
            SVC.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            SVC.Columns.Add("HL_PARENT", GetType(String))
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
            'Me.SVC.Columns.Add("FILE_ID", GetType(String))
            ' SVC.Columns.Add("BATCH_ID", GetType(String))
            ' SVC.Columns.Add("TIME_STAMP", GetType(DateTime))

            '.SVC.Columns.Add("GS_ID", GetType(String))
            '.SVC.Columns.Add("ST_ID", GetType(String))
            '.SVC.Columns.Add("STC_ID", GetType(String))



            TRN.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            TRN.Columns.Add("ROW_NUM", GetType(Integer))
            TRN.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            TRN.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            TRN.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            TRN.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            TRN.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            TRN.Columns.Add("HIPAA_STC_GUID", GetType(Guid))
            TRN.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            TRN.Columns.Add("HL_PARENT", GetType(String))
            TRN.Columns.Add("TRN01", GetType(String))
            TRN.Columns.Add("TRN02", GetType(String))
            TRN.Columns.Add("TRN03", GetType(String))
            TRN.Columns.Add("TRN04", GetType(String))
            '.TRN.Columns.Add("FILE_ID", GetType(String))
            '.TRN.Columns.Add("BATCH_ID", GetType(String))
            '.TRN.Columns.Add("GS_ID", GetType(String))
            '.TRN.Columns.Add("ST_ID", GetType(String))
            '.TRN.Columns.Add("STC_ID", GetType(String))



            UNK.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            UNK.Columns.Add("ROW_NUM", GetType(Integer))
            UNK.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            UNK.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            UNK.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            UNK.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            UNK.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            UNK.Columns.Add("HIPAA_STC_GUID", GetType(Guid))
            UNK.Columns.Add("HIPAA_P_STC_ID", GetType(Guid))
            UNK.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            UNK.Columns.Add("HL_PARENT", GetType(String))
            UNK.Columns.Add("ROW_RECORD_TYPE", GetType(String))
            UNK.Columns.Add("ROW_DATA", GetType(String))


            'EDI 837i

            'ISA.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            'ISA.Columns.Add("ROW_NUM", GetType(Integer))
            'ISA.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            'ISA.Columns.Add("ISA01", GetType(String))
            'ISA.Columns.Add("ISA02", GetType(String))
            'ISA.Columns.Add("ISA03", GetType(String))
            'ISA.Columns.Add("ISA04", GetType(String))
            'ISA.Columns.Add("ISA05", GetType(String))
            'ISA.Columns.Add("ISA06", GetType(String))
            'ISA.Columns.Add("ISA07", GetType(String))
            'ISA.Columns.Add("ISA08", GetType(String))
            'ISA.Columns.Add("ISA09", GetType(String))
            'ISA.Columns.Add("ISA10", GetType(String))
            'ISA.Columns.Add("ISA11", GetType(String))
            'ISA.Columns.Add("ISA12", GetType(String))
            'ISA.Columns.Add("ISA13", GetType(String))
            'ISA.Columns.Add("ISA14", GetType(String))
            'ISA.Columns.Add("ISA15", GetType(String))
            'ISA.Columns.Add("ISA16", GetType(String))
            'ISA.Columns.Add("FILE_ID", GetType(String))
            'ISA.Columns.Add("BATCH_ID", GetType(String))


            'GS.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            'GS.Columns.Add("ROW_NUM", GetType(Integer))
            'GS.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            'GS.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            'GS.Columns.Add("GS01", GetType(String))
            'GS.Columns.Add("GS02", GetType(String))
            'GS.Columns.Add("GS03", GetType(String))
            'GS.Columns.Add("GS04", GetType(String))
            'GS.Columns.Add("GS05", GetType(String))
            'GS.Columns.Add("GS06", GetType(String))
            'GS.Columns.Add("GS07", GetType(String))
            'GS.Columns.Add("GS08", GetType(String))
            'GS.Columns.Add("FILE_ID", GetType(String))
            'GS.Columns.Add("BATCH_ID", GetType(String))
            'GS.Columns.Add("GS_ID", GetType(String))



            'ST.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            'ST.Columns.Add("ROW_NUM", GetType(Integer))
            'ST.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            'ST.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            'ST.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            'ST.Columns.Add("ST01", GetType(String))
            'ST.Columns.Add("ST02", GetType(String))
            'ST.Columns.Add("ST03", GetType(String))
            'ST.Columns.Add("BATCH_ID", GetType(String))
            'ST.Columns.Add("FILE_ID", GetType(String))
            'ST.Columns.Add("GS_ID", GetType(String))
            'ST.Columns.Add("ST_ID", GetType(String))





            'BHT.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            'BHT.Columns.Add("ROW_NUM", GetType(Integer))
            'BHT.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            'BHT.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            'BHT.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            'BHT.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            'BHT.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            'BHT.Columns.Add("HIPAA_STC_GUID", GetType(Guid))
            'BHT.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            'BHT.Columns.Add("HL_PARENT", GetType(String))
            'BHT.Columns.Add("BHT01", GetType(String))
            'BHT.Columns.Add("BHT02", GetType(String))
            'BHT.Columns.Add("BHT03", GetType(String))
            'BHT.Columns.Add("BHT04", GetType(String))
            'BHT.Columns.Add("BHT05", GetType(String))
            'BHT.Columns.Add("BHT06", GetType(String))
            'BHT.Columns.Add("FILE_ID", GetType(String))
            'BHT.Columns.Add("BATCH_ID", GetType(String))
            'BHT.Columns.Add("GS_ID", GetType(String))
            'BHT.Columns.Add("ST_ID", GetType(String))



            'HL.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            'HL.Columns.Add("ROW_NUM", GetType(Integer))
            'HL.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            'HL.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            'HL.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            'HL.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            'HL.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            'HL.Columns.Add("HIPAA_STC_GUID", GetType(Guid))
            'HL.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            'HL.Columns.Add("HL_PARENT", GetType(String))
            'HL.Columns.Add("HL01", GetType(String))
            'HL.Columns.Add("HL02", GetType(String))
            'HL.Columns.Add("HL03", GetType(String))
            'HL.Columns.Add("HL04", GetType(String))
            'HL.Columns.Add("FILE_ID", GetType(String))
            'HL.Columns.Add("BATCH_ID", GetType(String))
            'HL.Columns.Add("GS_ID", GetType(String))
            'HL.Columns.Add("ST_ID", GetType(String))




            'STC.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            'STC.Columns.Add("ROW_NUM", GetType(Integer))
            'STC.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            'STC.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            'STC.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            'STC.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            'STC.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            'STC.Columns.Add("HIPAA_STC_GUID", GetType(Guid))
            'STC.Columns.Add("HIPAA_P_STC_GUID", GetType(Guid))
            'STC.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            'STC.Columns.Add("HL_PARENT", GetType(String))
            'STC.Columns.Add("STC01", GetType(String))
            'STC.Columns.Add("STC01_1", GetType(String))
            'STC.Columns.Add("STC01_2", GetType(String))
            'STC.Columns.Add("STC01_3", GetType(String))
            'STC.Columns.Add("STC01_4", GetType(String))
            'STC.Columns.Add("STC02", GetType(String))
            'STC.Columns.Add("STC03", GetType(String))
            'STC.Columns.Add("STC04", GetType(String))
            'STC.Columns.Add("STC05", GetType(String))
            'STC.Columns.Add("STC06", GetType(String))
            'STC.Columns.Add("STC07", GetType(String))
            'STC.Columns.Add("STC08", GetType(String))
            'STC.Columns.Add("STC09", GetType(String))
            'STC.Columns.Add("BATCH_ID", GetType(String))
            'STC.Columns.Add("FILE_ID", GetType(String))
            'STC.Columns.Add("GS_ID", GetType(String))
            'STC.Columns.Add("ST_ID", GetType(String))



            'NM1.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            'NM1.Columns.Add("ROW_NUM", GetType(Integer))
            'NM1.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            'NM1.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            'NM1.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            'NM1.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            'NM1.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            'NM1.Columns.Add("HIPAA_STC_GUID", GetType(Guid))
            'NM1.Columns.Add("HIPAA_P_STC_GUID", GetType(Guid))
            'NM1.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            'NM1.Columns.Add("HL_PARENT", GetType(String))
            'NM1.Columns.Add("NM101", GetType(String))
            'NM1.Columns.Add("NM102", GetType(String))
            'NM1.Columns.Add("NM103", GetType(String))
            'NM1.Columns.Add("NM104", GetType(String))
            'NM1.Columns.Add("NM105", GetType(String))
            'NM1.Columns.Add("NM106", GetType(String))
            'NM1.Columns.Add("NM107", GetType(String))
            'NM1.Columns.Add("NM108", GetType(String))
            'NM1.Columns.Add("NM109", GetType(String))
            'NM1.Columns.Add("NM110", GetType(String))
            'NM1.Columns.Add("NM111", GetType(String))
            'NM1.Columns.Add("FILE_ID", GetType(String))
            'NM1.Columns.Add("BATCH_ID", GetType(String))
            'NM1.Columns.Add("GS_ID", GetType(String))
            'NM1.Columns.Add("ST_ID", GetType(String))





            'DMG.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            'DMG.Columns.Add("ROW_NUM", GetType(Integer))
            'DMG.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            'DMG.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            'DMG.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            'DMG.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            'DMG.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            'DMG.Columns.Add("HIPAA_STC_GUID", GetType(Guid))
            'DMG.Columns.Add("HIPAA_P_STC_GUID", GetType(Guid))
            'DMG.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            'DMG.Columns.Add("HL_PARENT", GetType(String))
            'DMG.Columns.Add("DMG01", GetType(String))
            'DMG.Columns.Add("DMG02", GetType(String))
            'DMG.Columns.Add("DMG03", GetType(String))
            'DMG.Columns.Add("FILE_ID", GetType(String))
            'DMG.Columns.Add("BATCH_ID", GetType(String))
            'DMG.Columns.Add("GS_ID", GetType(String))
            'DMG.Columns.Add("ST_ID", GetType(String))





            'REF.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            'REF.Columns.Add("ROW_NUM", GetType(Integer))
            'REF.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            'REF.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            'REF.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            'REF.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            'REF.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            'REF.Columns.Add("HIPAA_STC_GUID", GetType(Guid))
            'REF.Columns.Add("HIPAA_P_STC_GUID", GetType(Guid))
            'REF.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            'REF.Columns.Add("HL_PARENT", GetType(String))
            'REF.Columns.Add("REF01", GetType(String))
            'REF.Columns.Add("REF02", GetType(String))
            'REF.Columns.Add("REF03", GetType(String))
            'REF.Columns.Add("FILE_ID", GetType(String))
            'REF.Columns.Add("BATCH_ID", GetType(String))
            'REF.Columns.Add("GS_ID", GetType(String))
            'REF.Columns.Add("ST_ID", GetType(String))







            'PER.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            'PER.Columns.Add("ROW_NUM", GetType(Integer))
            'PER.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            'PER.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            'PER.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            'PER.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            'PER.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            'PER.Columns.Add("HIPAA_STC_GUID", GetType(Guid))
            'PER.Columns.Add("HIPAA_P_STC_GUID", GetType(Guid))
            'PER.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            'PER.Columns.Add("HL_PARENT", GetType(String))
            'PER.Columns.Add("PER01", GetType(String))
            'PER.Columns.Add("PER02", GetType(String))
            'PER.Columns.Add("PER03", GetType(String))
            'PER.Columns.Add("PER04", GetType(String))
            'PER.Columns.Add("PER05", GetType(String))
            'PER.Columns.Add("PER06", GetType(String))
            'PER.Columns.Add("PER07", GetType(String))
            'PER.Columns.Add("PER08", GetType(String))
            'PER.Columns.Add("PER09", GetType(String))
            'PER.Columns.Add("FILE_ID", GetType(String))
            'PER.Columns.Add("BATCH_ID", GetType(String))
            'PER.Columns.Add("GS_ID", GetType(String))
            'PER.Columns.Add("ST_ID", GetType(String))



            'DTP.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            'DTP.Columns.Add("ROW_NUM", GetType(Integer))
            'DTP.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            'DTP.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            'DTP.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            'DTP.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            'DTP.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            'DTP.Columns.Add("HIPAA_STC_GUID", GetType(Guid))
            'DTP.Columns.Add("HIPAA_P_STC_GUID", GetType(Guid))
            'DTP.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            'DTP.Columns.Add("HL_PARENT", GetType(String))
            'DTP.Columns.Add("DTP01", GetType(String))
            'DTP.Columns.Add("DTP02", GetType(String))
            'DTP.Columns.Add("DTP03", GetType(String))
            'DTP.Columns.Add("FILE_ID", GetType(String))
            'DTP.Columns.Add("BATCH_ID", GetType(String))
            'DTP.Columns.Add("GS_ID", GetType(String))
            'DTP.Columns.Add("ST_ID", GetType(String))




            'SVC.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            'SVC.Columns.Add("ROW_NUM", GetType(Integer))
            'SVC.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            'SVC.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            'SVC.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            'SVC.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            'SVC.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            'SVC.Columns.Add("HIPAA_STC_GUID", GetType(Guid))
            'SVC.Columns.Add("HIPAA_P_STC_GUID", GetType(Guid))
            'SVC.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            'SVC.Columns.Add("HL_PARENT", GetType(String))
            'SVC.Columns.Add("SVC01", GetType(String))
            'SVC.Columns.Add("SVC01_1", GetType(String))
            'SVC.Columns.Add("SVC01_2", GetType(String))
            'SVC.Columns.Add("SVC01_3", GetType(String))
            'SVC.Columns.Add("SVC01_4", GetType(String))
            'SVC.Columns.Add("SVC01_5", GetType(String))
            'SVC.Columns.Add("SVC01_6", GetType(String))
            'SVC.Columns.Add("SVC01_7", GetType(String))
            'SVC.Columns.Add("SVC02", GetType(String))
            'SVC.Columns.Add("SVC03", GetType(String))
            'SVC.Columns.Add("SVC04", GetType(String))
            'SVC.Columns.Add("SVC05", GetType(String))
            'SVC.Columns.Add("SVC06", GetType(String))
            'SVC.Columns.Add("SVC06_1", GetType(String))
            'SVC.Columns.Add("SVC06_2", GetType(String))
            'SVC.Columns.Add("SVC06_3", GetType(String))
            'SVC.Columns.Add("SVC06_4", GetType(String))
            'SVC.Columns.Add("SVC06_5", GetType(String))
            'SVC.Columns.Add("SVC06_6", GetType(String))
            'SVC.Columns.Add("SVC06_7", GetType(String))
            'SVC.Columns.Add("SVC07", GetType(String))
            'SVC.Columns.Add("FILE_ID", GetType(String))
            'SVC.Columns.Add("BATCH_ID", GetType(String))
            'SVC.Columns.Add("GS_ID", GetType(String))
            'SVC.Columns.Add("ST_ID", GetType(String))




            'TRN.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            'TRN.Columns.Add("ROW_NUM", GetType(Integer))
            'TRN.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            'TRN.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            'TRN.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            'TRN.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            'TRN.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            'TRN.Columns.Add("HIPAA_STC_GUID", GetType(Guid))
            'TRN.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            'TRN.Columns.Add("HL_PARENT", GetType(String))
            'TRN.Columns.Add("TRN01", GetType(String))
            'TRN.Columns.Add("TRN02", GetType(String))
            'TRN.Columns.Add("TRN03", GetType(String))
            'TRN.Columns.Add("TRN04", GetType(String))
            'TRN.Columns.Add("FILE_ID", GetType(String))
            'TRN.Columns.Add("BATCH_ID", GetType(String))




            'UNK.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            'UNK.Columns.Add("ROW_NUM", GetType(Integer))
            'UNK.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            'UNK.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            'UNK.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            'UNK.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            'UNK.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            'UNK.Columns.Add("HIPAA_STC_GUID", GetType(Guid))
            'UNK.Columns.Add("HIPAA_P_STC_ID", GetType(Guid))
            'UNK.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            'UNK.Columns.Add("HL_PARENT", GetType(String))
            'UNK.Columns.Add("ROW_RECORD_TYPE", GetType(String))
            'UNK.Columns.Add("ROW_DATA", GetType(String))


            'end 835 tables


        End Sub
    End Class
End Namespace
