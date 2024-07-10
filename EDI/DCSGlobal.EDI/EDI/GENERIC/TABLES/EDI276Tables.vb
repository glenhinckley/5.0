Option Explicit On
Option Strict On
Option Compare Binary





Imports System.Data


Namespace DCSGlobal.EDI


    Public Class EDI276Tables





        'ENVELOP EDI TAbles

        Public ISA As New DataTable
        Public GS As New DataTable
        Public ST As New DataTable

        '278 tables start

        Public BHT As New DataTable
        Public NM1 As New DataTable
        Public REF As New DataTable
        Public AMT As New DataTable
        Public SVC As New DataTable
        Public DTP As New DataTable

        Public HL As New DataTable
        Public TRN As New DataTable

        Public DMG As New DataTable

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
            BHT.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            BHT.Columns.Add("HIPAA_STC_GUID", GetType(Guid))
            BHT.Columns.Add("HL_PARENT", GetType(String))
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
            HL.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            HL.Columns.Add("HL_PARENT", GetType(String))
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

            DMG.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            DMG.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            DMG.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            DMG.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            DMG.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            DMG.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            DMG.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            DMG.Columns.Add("HL_PARENT", GetType(String))
            DMG.Columns.Add("DMG01", GetType(String))
            DMG.Columns.Add("DMG02", GetType(String))
            DMG.Columns.Add("DMG03", GetType(String))


            REF.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            REF.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            REF.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            REF.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            REF.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            REF.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            REF.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            REF.Columns.Add("HL_PARENT", GetType(String))
            REF.Columns.Add("REF01", GetType(String))
            REF.Columns.Add("REF02", GetType(String))
            REF.Columns.Add("REF03", GetType(String))

            AMT.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            AMT.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            AMT.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            AMT.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            AMT.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            AMT.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            AMT.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            AMT.Columns.Add("HL_PARENT", GetType(String))
            AMT.Columns.Add("AMT01", GetType(String))
            AMT.Columns.Add("AMT02", GetType(String))
            AMT.Columns.Add("AMT03", GetType(String))

            DTP.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            DTP.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            DTP.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            DTP.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            DTP.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            DTP.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            DTP.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            DTP.Columns.Add("HL_PARENT", GetType(String))
            DTP.Columns.Add("DTP01", GetType(String))
            DTP.Columns.Add("DTP02", GetType(String))
            DTP.Columns.Add("DTP03", GetType(String))


            TRN.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            TRN.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            TRN.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            TRN.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            TRN.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            TRN.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            TRN.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            TRN.Columns.Add("HL_PARENT", GetType(String))
            TRN.Columns.Add("TRN01", GetType(String))
            TRN.Columns.Add("TRN02", GetType(String))
            TRN.Columns.Add("TRN03", GetType(String))
            TRN.Columns.Add("TRN04", GetType(String))



            SVC.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            SVC.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            SVC.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            SVC.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            SVC.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            SVC.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
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


            UNK.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            UNK.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            UNK.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            UNK.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            UNK.Columns.Add("HIPAA_BHT_GUID", GetType(Guid))
            UNK.Columns.Add("HIPAA_HL_GUID", GetType(Guid))
            UNK.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            UNK.Columns.Add("HL_PARENT", GetType(String))
            UNK.Columns.Add("ROW_RECORD_TYPE", GetType(String))
            UNK.Columns.Add("ROW_DATA", GetType(String))


            'end 835 tables

        End Sub
    End Class
End Namespace
