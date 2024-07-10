
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


    Public Class EDIDataTables










        Private _ConnectionString As String = String.Empty
        Dim _BATCH_ID As Integer




        Public ENVELOP As New DataTable
        Public N3_N4 As New DataTable

        Public EDI_271 As New DataTable
        Public ISA As New DataTable
        Public GS As New DataTable
        Public ST As New DataTable
        Public HL01 As New DataTable
        Public HL02 As New DataTable
        Public HL03 As New DataTable
        Public HL04 As New DataTable
        Public BHT As New DataTable
        Public EB As New DataTable
        Public AAA As New DataTable
        Public AMT As New DataTable
        Public CAS As New DataTable
        Public DMG As New DataTable
        Public DTP As New DataTable
        Public EQ As New DataTable
        Public HSD As New DataTable
        Public HL As New DataTable
        Public III As New DataTable
        Public INS As New DataTable
        Public TMSG As New DataTable
        Public MSG As New DataTable
        Public N1 As New DataTable
        Public N3 As New DataTable
        Public N4 As New DataTable
        Public NM1 As New DataTable
        Public PAT As New DataTable
        Public PER As New DataTable
        Public PRV As New DataTable

        Public REF As New DataTable


        Public SBR As New DataTable



        Public TRN As New DataTable
        Public UNK As New DataTable

        Public CACHE_EB As New DataTable
        Public CACHE_AAA As New DataTable
        Public CACHE_AMT As New DataTable
        Public CACHE_DMG As New DataTable
        Public CACHE_BHT As New DataTable
        Public CACHE_DTP As New DataTable
        Public CACHE_EQ As New DataTable
        Public CACHE_HSD As New DataTable
        Public CACHE_HL As New DataTable
        Public CACHE_III As New DataTable
        Public CACHE_INS As New DataTable
        Public CACHE_TMSG As New DataTable
        Public CACHE_MSG As New DataTable
        Public CACHE_N3 As New DataTable
        Public CACHE_N4 As New DataTable
        Public CACHE_NM1 As New DataTable
        Public CACHE_PER As New DataTable
        Public CACHE_PRV As New DataTable
        Public CACHE_REF As New DataTable
        Public CACHE_TRN As New DataTable

        Public TEBUID As New DataTable




        'Public AMT As New DataTable
        Public BPR As New DataTable
        ' Public CAS As New DataTable
        Public CLP As New DataTable
        Public DTM As New DataTable
        Public LQ As New DataTable
        Public LX As New DataTable
        Public MIA As New DataTable
        Public MOA As New DataTable
        ' Public N1 As New DataTable
        Public PLB As New DataTable
        Public QTY As New DataTable
        Public TS2 As New DataTable
        Public TS3 As New DataTable
        Public SVC As New DataTable
        Public RESPONSE As New DataTable


        'end 835 tables





        Public Sub Decs()







        End Sub
        Public Sub BuildTables()


            'EDI 271

            EDI_271.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            EDI_271.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            EDI_271.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            EDI_271.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            EDI_271.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            EDI_271.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            EDI_271.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            EDI_271.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            EDI_271.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            EDI_271.Columns.Add("P_GUID", GetType(Guid))
            EDI_271.Columns.Add("HL_PARENT", GetType(String))
            EDI_271.Columns.Add("ROW_RECORD_TYPE", GetType(String))
            EDI_271.Columns.Add("ROW_DATA", GetType(String))
            EDI_271.Columns.Add("EBC", GetType(Integer))
            EDI_271.Columns.Add("RowDataParsed", GetType(Integer))
            EDI_271.Columns.Add("BATCH_ID", GetType(Double))
            EDI_271.Columns.Add("TIME_STAMP", GetType(DateTime))


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
            ISA.Columns.Add("IEA01", GetType(String))
            ISA.Columns.Add("IEA02", GetType(String))
            ISA.Columns.Add("BATCH_ID", GetType(Double))
            ISA.Columns.Add("TIME_STAMP", GetType(DateTime))

            GS.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            GS.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            GS.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            GS.Columns.Add("P_GUID", GetType(Guid))
            GS.Columns.Add("HL_PARENT", GetType(String))
            GS.Columns.Add("GS01", GetType(String))
            GS.Columns.Add("GS02", GetType(String))
            GS.Columns.Add("GS03", GetType(String))
            GS.Columns.Add("GS04", GetType(String))
            GS.Columns.Add("GS05", GetType(String))
            GS.Columns.Add("GS06", GetType(String))
            GS.Columns.Add("GS07", GetType(String))
            GS.Columns.Add("GS08", GetType(String))
            GS.Columns.Add("GE01", GetType(String))
            GS.Columns.Add("GE02", GetType(String))
            GS.Columns.Add("BATCH_ID", GetType(Double))
            GS.Columns.Add("TIME_STAMP", GetType(DateTime))


            ST.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            ST.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            ST.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            ST.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            ST.Columns.Add("P_GUID", GetType(Guid))
            ST.Columns.Add("HL_PARENT", GetType(String))
            ST.Columns.Add("ST01", GetType(String))
            ST.Columns.Add("ST02", GetType(String))
            ST.Columns.Add("ST03", GetType(String))
            ST.Columns.Add("SE01", GetType(String))
            ST.Columns.Add("SE02", GetType(String))
            ST.Columns.Add("BATCH_ID", GetType(Double))
            ST.Columns.Add("TIME_STAMP", GetType(DateTime))

            HL01.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            HL01.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            HL01.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            HL01.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            HL01.Columns.Add("P_GUID", GetType(Guid))
            HL01.Columns.Add("HL_PARENT", GetType(String))
            HL01.Columns.Add("HL01", GetType(String))
            HL01.Columns.Add("HL02", GetType(String))
            HL01.Columns.Add("HL03", GetType(String))
            HL01.Columns.Add("HL04", GetType(String))
            HL01.Columns.Add("BATCH_ID", GetType(Double))
            HL01.Columns.Add("TIME_STAMP", GetType(DateTime))


            HL02.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            HL02.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            HL02.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            HL02.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            HL02.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            HL02.Columns.Add("P_GUID", GetType(Guid))
            HL02.Columns.Add("HL_PARENT", GetType(String))
            HL02.Columns.Add("HL01", GetType(String))
            HL02.Columns.Add("HL02", GetType(String))
            HL02.Columns.Add("HL03", GetType(String))
            HL02.Columns.Add("HL04", GetType(String))
            HL02.Columns.Add("BATCH_ID", GetType(Double))
            HL02.Columns.Add("TIME_STAMP", GetType(DateTime))

            HL03.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            HL03.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            HL03.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            HL03.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            HL03.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            HL03.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            HL03.Columns.Add("P_GUID", GetType(Guid))
            HL03.Columns.Add("HL_PARENT", GetType(String))
            HL03.Columns.Add("HL01", GetType(String))
            HL03.Columns.Add("HL02", GetType(String))
            HL03.Columns.Add("HL03", GetType(String))
            HL03.Columns.Add("HL04", GetType(String))
            HL03.Columns.Add("BATCH_ID", GetType(Double))
            HL03.Columns.Add("TIME_STAMP", GetType(DateTime))


            HL04.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            HL04.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            HL04.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            HL04.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            HL04.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            HL04.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            HL04.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            HL04.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            HL04.Columns.Add("P_GUID", GetType(Guid))
            HL04.Columns.Add("HL_PARENT", GetType(String))
            HL04.Columns.Add("HL01", GetType(String))
            HL04.Columns.Add("HL02", GetType(String))
            HL04.Columns.Add("HL03", GetType(String))
            HL04.Columns.Add("HL04", GetType(String))
            HL04.Columns.Add("BATCH_ID", GetType(Double))
            HL04.Columns.Add("TIME_STAMP", GetType(DateTime))

            BHT.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            BHT.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            BHT.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            BHT.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            BHT.Columns.Add("P_GUID", GetType(Guid))
            BHT.Columns.Add("HL_PARENT", GetType(String))
            BHT.Columns.Add("BHT01", GetType(String))
            BHT.Columns.Add("BHT02", GetType(String))
            BHT.Columns.Add("BHT03", GetType(String))
            BHT.Columns.Add("BHT04", GetType(String))
            BHT.Columns.Add("BHT05", GetType(String))
            BHT.Columns.Add("BHT06", GetType(String))
            BHT.Columns.Add("BATCH_ID", GetType(Double))
            BHT.Columns.Add("TIME_STAMP", GetType(DateTime))


            UNK.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            UNK.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            UNK.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            UNK.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            UNK.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            UNK.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            UNK.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            UNK.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            UNK.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            UNK.Columns.Add("P_GUID", GetType(Guid))
            UNK.Columns.Add("HL_PARENT", GetType(String))
            UNK.Columns.Add("ROW_RECORD_TYPE", GetType(String))
            UNK.Columns.Add("ROW_DATA", GetType(String))
            UNK.Columns.Add("BATCH_ID", GetType(Double))
            UNK.Columns.Add("TIME_STAMP", GetType(DateTime))

            EB.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            EB.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            EB.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            EB.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            EB.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            EB.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            EB.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            EB.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            EB.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            EB.Columns.Add("P_GUID", GetType(Guid))
            EB.Columns.Add("HL_PARENT", GetType(String))
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
            EB.Columns.Add("BATCH_ID", GetType(Double))
            EB.Columns.Add("TIME_STAMP", GetType(DateTime))

            AAA.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            AAA.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            AAA.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            AAA.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            AAA.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            AAA.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            AAA.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            AAA.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            AAA.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            AAA.Columns.Add("P_GUID", GetType(Guid))
            AAA.Columns.Add("HL_PARENT", GetType(String))
            AAA.Columns.Add("AAA01", GetType(String))
            AAA.Columns.Add("AAA02", GetType(String))
            AAA.Columns.Add("AAA03", GetType(String))
            AAA.Columns.Add("AAA04", GetType(String))
            AAA.Columns.Add("BATCH_ID", GetType(Double))
            AAA.Columns.Add("TIME_STAMP", GetType(DateTime))


            AMT.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            AMT.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            AMT.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            AMT.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            AMT.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            AMT.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            AMT.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            AMT.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            AMT.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            AMT.Columns.Add("P_GUID", GetType(Guid))
            AMT.Columns.Add("HL_PARENT", GetType(String))
            AMT.Columns.Add("AMT01", GetType(String))
            AMT.Columns.Add("AMT02", GetType(String))
            AMT.Columns.Add("AMT03", GetType(String))
            AMT.Columns.Add("BATCH_ID", GetType(Double))
            AMT.Columns.Add("TIME_STAMP", GetType(DateTime))


            CAS.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            CAS.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            CAS.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            CAS.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            CAS.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            CAS.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            CAS.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            CAS.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            CAS.Columns.Add("HIPAA_EB_GUID", GetType(Guid))


            CAS.Columns.Add("HIPAA_LX_GUID", GetType(Guid))
            CAS.Columns.Add("HIPAA_CLP_GUID", GetType(Guid))
            CAS.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            CAS.Columns.Add("P_GUID", GetType(Guid))
            CAS.Columns.Add("HL_PARENT", GetType(String))
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
            CAS.Columns.Add("BATCH_ID", GetType(Double))
            CAS.Columns.Add("TIME_STAMP", GetType(DateTime))



            DMG.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            DMG.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            DMG.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            DMG.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            DMG.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            DMG.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            DMG.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            DMG.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            DMG.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            DMG.Columns.Add("P_GUID", GetType(Guid))
            DMG.Columns.Add("HL_PARENT", GetType(String))
            DMG.Columns.Add("DMG01", GetType(String))
            DMG.Columns.Add("DMG02", GetType(String))
            DMG.Columns.Add("DMG03", GetType(String))
            DMG.Columns.Add("BATCH_ID", GetType(Double))
            DMG.Columns.Add("TIME_STAMP", GetType(DateTime))

            DTP.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            DTP.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            DTP.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            DTP.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            DTP.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            DTP.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            DTP.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            DTP.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            DTP.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            DTP.Columns.Add("P_GUID", GetType(Guid))
            DTP.Columns.Add("HL_PARENT", GetType(String))
            DTP.Columns.Add("DTP01", GetType(String))
            DTP.Columns.Add("DTP02", GetType(String))
            DTP.Columns.Add("DTP03", GetType(String))
            DTP.Columns.Add("BATCH_ID", GetType(Double))
            DTP.Columns.Add("TIME_STAMP", GetType(DateTime))


            EQ.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            EQ.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            EQ.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            EQ.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            EQ.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            EQ.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            EQ.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            EQ.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            EQ.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            EQ.Columns.Add("P_GUID", GetType(Guid))
            EQ.Columns.Add("HL_PARENT", GetType(String))
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
            EQ.Columns.Add("BATCH_ID", GetType(Double))
            EQ.Columns.Add("TIME_STAMP", GetType(DateTime))

            HSD.Columns.Add("ROW_ID", GetType(Integer))
            HSD.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            HSD.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            HSD.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            HSD.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            HSD.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            HSD.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            HSD.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            HSD.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            HSD.Columns.Add("P_GUID", GetType(Guid))
            HSD.Columns.Add("HL_PARENT", GetType(String))
            HSD.Columns.Add("HSD01", GetType(String))
            HSD.Columns.Add("HSD02", GetType(String))
            HSD.Columns.Add("HSD03", GetType(String))
            HSD.Columns.Add("HSD04", GetType(String))
            HSD.Columns.Add("HSD05", GetType(String))
            HSD.Columns.Add("HSD06", GetType(String))
            HSD.Columns.Add("HSD07", GetType(String))
            HSD.Columns.Add("HSD08", GetType(String))
            HSD.Columns.Add("BATCH_ID", GetType(Double))
            HSD.Columns.Add("TIME_STAMP", GetType(DateTime))

            HL.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            HL.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            HL.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            HL.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            HL.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            HL.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            HL.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            HL.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            HL.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            HL.Columns.Add("P_GUID", GetType(Guid))
            HL.Columns.Add("HL_PARENT", GetType(String))
            HL.Columns.Add("HL01", GetType(String))
            HL.Columns.Add("HL02", GetType(String))
            HL.Columns.Add("HL03", GetType(String))
            HL.Columns.Add("HL04", GetType(String))
            HL.Columns.Add("BATCH_ID", GetType(Double))
            HL.Columns.Add("TIME_STAMP", GetType(DateTime))

            III.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            III.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            III.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            III.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            III.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            III.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            III.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            III.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            III.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            III.Columns.Add("P_GUID", GetType(Guid))
            III.Columns.Add("HL_PARENT", GetType(String))
            III.Columns.Add("III01", GetType(String))
            III.Columns.Add("III02", GetType(String))
            III.Columns.Add("III03", GetType(String))
            III.Columns.Add("III04", GetType(String))
            III.Columns.Add("III05", GetType(String))
            III.Columns.Add("III06", GetType(String))
            III.Columns.Add("III07", GetType(String))
            III.Columns.Add("III08", GetType(String))
            III.Columns.Add("III09", GetType(String))
            III.Columns.Add("BATCH_ID", GetType(Double))
            III.Columns.Add("TIME_STAMP", GetType(DateTime))

            INS.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            INS.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            INS.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            INS.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            INS.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            INS.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            INS.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            INS.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            INS.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            INS.Columns.Add("P_GUID", GetType(Guid))
            INS.Columns.Add("HL_PARENT", GetType(String))
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
            INS.Columns.Add("BATCH_ID", GetType(Double))
            INS.Columns.Add("TIME_STAMP", GetType(DateTime))

            MSG.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            MSG.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            MSG.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            MSG.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            MSG.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            MSG.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            MSG.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            MSG.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            MSG.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            MSG.Columns.Add("P_GUID", GetType(Guid))
            MSG.Columns.Add("HL_PARENT", GetType(String))
            MSG.Columns.Add("MSG01", GetType(String))
            MSG.Columns.Add("MSG02", GetType(String))
            MSG.Columns.Add("MSG03", GetType(String))
            MSG.Columns.Add("MSG04", GetType(String))
            MSG.Columns.Add("MSG05", GetType(String))
            MSG.Columns.Add("MSG06", GetType(String))
            MSG.Columns.Add("MSG07", GetType(String))
            MSG.Columns.Add("BATCH_ID", GetType(Double))
            MSG.Columns.Add("TIME_STAMP", GetType(DateTime))




            N1.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            N1.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            N1.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            N1.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            N1.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            N1.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            N1.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            N1.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            N1.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            N1.Columns.Add("P_GUID", GetType(Guid))
            N1.Columns.Add("HL_PARENT", GetType(String))
            N1.Columns.Add("N101", GetType(String))
            N1.Columns.Add("N102", GetType(String))
            N1.Columns.Add("N103", GetType(String))
            N1.Columns.Add("N104", GetType(String))
            N1.Columns.Add("BATCH_ID", GetType(Double))
            N1.Columns.Add("TIME_STAMP", GetType(DateTime))


            N3.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            N3.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            N3.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            N3.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            N3.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            N3.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            N3.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            N3.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            N3.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            N3.Columns.Add("P_GUID", GetType(Guid))
            N3.Columns.Add("HL_PARENT", GetType(String))
            N3.Columns.Add("N301", GetType(String))
            N3.Columns.Add("N302", GetType(String))
            N3.Columns.Add("BATCH_ID", GetType(Double))
            N3.Columns.Add("TIME_STAMP", GetType(DateTime))


            N4.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            N4.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            N4.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            N4.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            N4.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            N4.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            N4.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            N4.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            N4.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            N4.Columns.Add("P_GUID", GetType(Guid))
            N4.Columns.Add("HL_PARENT", GetType(String))
            N4.Columns.Add("N401", GetType(String))
            N4.Columns.Add("N402", GetType(String))
            N4.Columns.Add("N403", GetType(String))
            N4.Columns.Add("N404", GetType(String))
            N4.Columns.Add("N405", GetType(String))
            N4.Columns.Add("N406", GetType(String))
            N4.Columns.Add("N407", GetType(String))
            N4.Columns.Add("BATCH_ID", GetType(Double))
            N4.Columns.Add("TIME_STAMP", GetType(DateTime))

            NM1.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            NM1.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            NM1.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            NM1.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            NM1.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            NM1.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            NM1.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            NM1.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            NM1.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            NM1.Columns.Add("P_GUID", GetType(Guid))
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
            NM1.Columns.Add("BATCH_ID", GetType(Double))
            NM1.Columns.Add("TIME_STAMP", GetType(DateTime))

            PAT.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            PAT.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            PAT.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            PAT.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            PAT.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            PAT.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            PAT.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            PAT.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            PAT.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            PAT.Columns.Add("P_GUID", GetType(Guid))
            PAT.Columns.Add("HL_PARENT", GetType(String))
            PAT.Columns.Add("PAT01", GetType(String))
            PAT.Columns.Add("PAT02", GetType(String))
            PAT.Columns.Add("PAT03", GetType(String))
            PAT.Columns.Add("PAT04", GetType(String))
            PAT.Columns.Add("PAT05", GetType(String))
            PAT.Columns.Add("PAT06", GetType(String))
            PAT.Columns.Add("PAT07", GetType(String))
            PAT.Columns.Add("PAT08", GetType(String))
            PAT.Columns.Add("PAT09", GetType(String))
            PAT.Columns.Add("BATCH_ID", GetType(Double))
            PAT.Columns.Add("TIME_STAMP", GetType(DateTime))





            PER.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            PER.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            PER.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            PER.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            PER.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            PER.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            PER.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            PER.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            PER.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            PER.Columns.Add("P_GUID", GetType(Guid))
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
            PER.Columns.Add("BATCH_ID", GetType(Double))
            PER.Columns.Add("TIME_STAMP", GetType(DateTime))


            PRV.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            PRV.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            PRV.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            PRV.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            PRV.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            PRV.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            PRV.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            PRV.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            PRV.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            PRV.Columns.Add("P_GUID", GetType(Guid))
            PRV.Columns.Add("HL_PARENT", GetType(String))
            PRV.Columns.Add("PRV01", GetType(String))
            PRV.Columns.Add("PRV02", GetType(String))
            PRV.Columns.Add("PRV03", GetType(String))
            PRV.Columns.Add("PRV04", GetType(String))
            PRV.Columns.Add("PRV05", GetType(String))
            PRV.Columns.Add("PRV06", GetType(String))
            PRV.Columns.Add("BATCH_ID", GetType(Double))
            PRV.Columns.Add("TIME_STAMP", GetType(DateTime))


            REF.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            REF.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            REF.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            REF.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            REF.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            REF.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            REF.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            REF.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            REF.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            REF.Columns.Add("P_GUID", GetType(Guid))
            REF.Columns.Add("HL_PARENT", GetType(String))
            REF.Columns.Add("REF01", GetType(String))
            REF.Columns.Add("REF02", GetType(String))
            REF.Columns.Add("REF03", GetType(String))
            REF.Columns.Add("BATCH_ID", GetType(Double))
            REF.Columns.Add("TIME_STAMP", GetType(DateTime))




            SBR.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            SBR.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            SBR.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            SBR.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            SBR.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            SBR.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            SBR.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            SBR.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            SBR.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            SBR.Columns.Add("P_GUID", GetType(Guid))
            SBR.Columns.Add("HL_PARENT", GetType(String))
            SBR.Columns.Add("SBR01", GetType(String))
            SBR.Columns.Add("SBR02", GetType(String))
            SBR.Columns.Add("SBR03", GetType(String))
            SBR.Columns.Add("SBR04", GetType(String))
            SBR.Columns.Add("SBR05", GetType(String))
            SBR.Columns.Add("SBR06", GetType(String))
            SBR.Columns.Add("SBR07", GetType(String))
            SBR.Columns.Add("SBR08", GetType(String))
            SBR.Columns.Add("SBR09", GetType(String))
            SBR.Columns.Add("BATCH_ID", GetType(Double))
            SBR.Columns.Add("TIME_STAMP", GetType(DateTime))



            TRN.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            TRN.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            TRN.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            TRN.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            TRN.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            TRN.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            TRN.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            TRN.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            TRN.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            TRN.Columns.Add("P_GUID", GetType(Guid))
            TRN.Columns.Add("HL_PARENT", GetType(String))
            TRN.Columns.Add("TRN01", GetType(String))
            TRN.Columns.Add("TRN02", GetType(String))
            TRN.Columns.Add("TRN03", GetType(String))
            TRN.Columns.Add("TRN04", GetType(String))
            TRN.Columns.Add("BATCH_ID", GetType(Double))
            TRN.Columns.Add("TIME_STAMP", GetType(DateTime))



            CACHE_EB.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            CACHE_EB.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            CACHE_EB.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            CACHE_EB.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            CACHE_EB.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            CACHE_EB.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            CACHE_EB.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            CACHE_EB.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            CACHE_EB.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            CACHE_EB.Columns.Add("P_GUID", GetType(Guid))
            CACHE_EB.Columns.Add("HL_PARENT", GetType(String))
            CACHE_EB.Columns.Add("EB01", GetType(String))
            CACHE_EB.Columns.Add("EB02", GetType(String))
            CACHE_EB.Columns.Add("EB03", GetType(String))
            CACHE_EB.Columns.Add("EB04", GetType(String))
            CACHE_EB.Columns.Add("EB05", GetType(String))
            CACHE_EB.Columns.Add("EB06", GetType(String))
            CACHE_EB.Columns.Add("EB07", GetType(String))
            CACHE_EB.Columns.Add("EB08", GetType(String))
            CACHE_EB.Columns.Add("EB09", GetType(String))
            CACHE_EB.Columns.Add("EB10", GetType(String))
            CACHE_EB.Columns.Add("EB11", GetType(String))
            CACHE_EB.Columns.Add("EB12", GetType(String))
            CACHE_EB.Columns.Add("EB13", GetType(String))
            CACHE_EB.Columns.Add("EB13_1", GetType(String))
            CACHE_EB.Columns.Add("EB13_2", GetType(String))
            CACHE_EB.Columns.Add("EB13_3", GetType(String))
            CACHE_EB.Columns.Add("EB13_4", GetType(String))
            CACHE_EB.Columns.Add("EB13_5", GetType(String))
            CACHE_EB.Columns.Add("EB13_6", GetType(String))
            CACHE_EB.Columns.Add("EB13_7", GetType(String))
            CACHE_EB.Columns.Add("BATCH_ID", GetType(Double))
            CACHE_EB.Columns.Add("TIME_STAMP", GetType(DateTime))

            CACHE_AAA.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            CACHE_AAA.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            CACHE_AAA.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            CACHE_AAA.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            CACHE_AAA.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            CACHE_AAA.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            CACHE_AAA.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            CACHE_AAA.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            CACHE_AAA.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            CACHE_AAA.Columns.Add("P_GUID", GetType(Guid))
            CACHE_AAA.Columns.Add("HL_PARENT", GetType(String))
            CACHE_AAA.Columns.Add("AAA01", GetType(String))
            CACHE_AAA.Columns.Add("AAA02", GetType(String))
            CACHE_AAA.Columns.Add("AAA03", GetType(String))
            CACHE_AAA.Columns.Add("AAA04", GetType(String))
            CACHE_AAA.Columns.Add("BATCH_ID", GetType(Double))
            CACHE_AAA.Columns.Add("TIME_STAMP", GetType(DateTime))



            CACHE_AMT.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            CACHE_AMT.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            CACHE_AMT.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            CACHE_AMT.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            CACHE_AMT.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            CACHE_AMT.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            CACHE_AMT.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            CACHE_AMT.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            CACHE_AMT.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            CACHE_AMT.Columns.Add("P_GUID", GetType(Guid))
            CACHE_AMT.Columns.Add("HL_PARENT", GetType(String))
            CACHE_AMT.Columns.Add("AMT01", GetType(String))
            CACHE_AMT.Columns.Add("AMT02", GetType(String))
            CACHE_AMT.Columns.Add("AMT03", GetType(String))
            CACHE_AMT.Columns.Add("BATCH_ID", GetType(Double))
            CACHE_AMT.Columns.Add("TIME_STAMP", GetType(DateTime))

            CACHE_DMG.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            CACHE_DMG.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            CACHE_DMG.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            CACHE_DMG.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            CACHE_DMG.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            CACHE_DMG.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            CACHE_DMG.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            CACHE_DMG.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            CACHE_DMG.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            CACHE_DMG.Columns.Add("HL_PARENT", GetType(String))
            CACHE_DMG.Columns.Add("P_GUID", GetType(Guid))
            CACHE_DMG.Columns.Add("DMG01", GetType(String))
            CACHE_DMG.Columns.Add("DMG02", GetType(String))
            CACHE_DMG.Columns.Add("DMG03", GetType(String))
            CACHE_DMG.Columns.Add("BATCH_ID", GetType(Double))
            CACHE_DMG.Columns.Add("TIME_STAMP", GetType(DateTime))

            CACHE_DTP.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            CACHE_DTP.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            CACHE_DTP.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            CACHE_DTP.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            CACHE_DTP.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            CACHE_DTP.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            CACHE_DTP.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            CACHE_DTP.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            CACHE_DTP.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            CACHE_DTP.Columns.Add("P_GUID", GetType(Guid))
            CACHE_DTP.Columns.Add("HL_PARENT", GetType(String))
            CACHE_DTP.Columns.Add("DTP01", GetType(String))
            CACHE_DTP.Columns.Add("DTP02", GetType(String))
            CACHE_DTP.Columns.Add("DTP03", GetType(String))
            CACHE_DTP.Columns.Add("BATCH_ID", GetType(Double))
            CACHE_DTP.Columns.Add("TIME_STAMP", GetType(DateTime))

            CACHE_EQ.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            CACHE_EQ.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            CACHE_EQ.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            CACHE_EQ.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            CACHE_EQ.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            CACHE_EQ.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            CACHE_EQ.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            CACHE_EQ.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            CACHE_EQ.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            CACHE_EQ.Columns.Add("P_GUID", GetType(Guid))
            CACHE_EQ.Columns.Add("HL_PARENT", GetType(String))
            CACHE_EQ.Columns.Add("EQ01", GetType(String))
            CACHE_EQ.Columns.Add("EQ02", GetType(String))
            CACHE_EQ.Columns.Add("EQ02_1", GetType(String))
            CACHE_EQ.Columns.Add("EQ02_2", GetType(String))
            CACHE_EQ.Columns.Add("EQ02_3", GetType(String))
            CACHE_EQ.Columns.Add("EQ02_4", GetType(String))
            CACHE_EQ.Columns.Add("EQ02_5", GetType(String))
            CACHE_EQ.Columns.Add("EQ02_6", GetType(String))
            CACHE_EQ.Columns.Add("EQ02_7", GetType(String))
            CACHE_EQ.Columns.Add("EQ03", GetType(String))
            CACHE_EQ.Columns.Add("EQ04", GetType(String))
            CACHE_EQ.Columns.Add("BATCH_ID", GetType(Double))
            CACHE_EQ.Columns.Add("TIME_STAMP", GetType(DateTime))

            CACHE_HSD.Columns.Add("ROW_ID", GetType(Integer))
            CACHE_HSD.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            CACHE_HSD.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            CACHE_HSD.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            CACHE_HSD.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            CACHE_HSD.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            CACHE_HSD.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            CACHE_HSD.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            CACHE_HSD.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            CACHE_HSD.Columns.Add("P_GUID", GetType(Guid))
            CACHE_HSD.Columns.Add("HL_PARENT", GetType(String))
            CACHE_HSD.Columns.Add("HSD01", GetType(String))
            CACHE_HSD.Columns.Add("HSD02", GetType(String))
            CACHE_HSD.Columns.Add("HSD03", GetType(String))
            CACHE_HSD.Columns.Add("HSD04", GetType(String))
            CACHE_HSD.Columns.Add("HSD05", GetType(String))
            CACHE_HSD.Columns.Add("HSD06", GetType(String))
            CACHE_HSD.Columns.Add("HSD07", GetType(String))
            CACHE_HSD.Columns.Add("HSD08", GetType(String))
            CACHE_HSD.Columns.Add("BATCH_ID", GetType(Double))
            CACHE_HSD.Columns.Add("TIME_STAMP", GetType(DateTime))

            CACHE_HL.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            CACHE_HL.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            CACHE_HL.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            CACHE_HL.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            CACHE_HL.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            CACHE_HL.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            CACHE_HL.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            CACHE_HL.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            CACHE_HL.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            CACHE_HL.Columns.Add("P_GUID", GetType(Guid))
            CACHE_HL.Columns.Add("HL_PARENT", GetType(String))
            CACHE_HL.Columns.Add("HL01", GetType(String))
            CACHE_HL.Columns.Add("HL02", GetType(String))
            CACHE_HL.Columns.Add("HL03", GetType(String))
            CACHE_HL.Columns.Add("HL04", GetType(String))
            CACHE_HL.Columns.Add("BATCH_ID", GetType(Double))
            CACHE_HL.Columns.Add("TIME_STAMP", GetType(DateTime))

            CACHE_III.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            CACHE_III.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            CACHE_III.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            CACHE_III.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            CACHE_III.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            CACHE_III.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            CACHE_III.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            CACHE_III.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            CACHE_III.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            CACHE_III.Columns.Add("P_GUID", GetType(Guid))
            CACHE_III.Columns.Add("HL_PARENT", GetType(String))
            CACHE_III.Columns.Add("III01", GetType(String))
            CACHE_III.Columns.Add("III02", GetType(String))
            CACHE_III.Columns.Add("III03", GetType(String))
            CACHE_III.Columns.Add("III04", GetType(String))
            CACHE_III.Columns.Add("III05", GetType(String))
            CACHE_III.Columns.Add("III06", GetType(String))
            CACHE_III.Columns.Add("III07", GetType(String))
            CACHE_III.Columns.Add("III08", GetType(String))
            CACHE_III.Columns.Add("III09", GetType(String))
            CACHE_III.Columns.Add("BATCH_ID", GetType(Double))
            CACHE_III.Columns.Add("TIME_STAMP", GetType(DateTime))

            CACHE_INS.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            CACHE_INS.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            CACHE_INS.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            CACHE_INS.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            CACHE_INS.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            CACHE_INS.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            CACHE_INS.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            CACHE_INS.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            CACHE_INS.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            CACHE_INS.Columns.Add("P_GUID", GetType(Guid))
            CACHE_INS.Columns.Add("HL_PARENT", GetType(String))
            CACHE_INS.Columns.Add("INS01", GetType(String))
            CACHE_INS.Columns.Add("INS02", GetType(String))
            CACHE_INS.Columns.Add("INS03", GetType(String))
            CACHE_INS.Columns.Add("INS04", GetType(String))
            CACHE_INS.Columns.Add("INS05", GetType(String))
            CACHE_INS.Columns.Add("INS06", GetType(String))
            CACHE_INS.Columns.Add("INS07", GetType(String))
            CACHE_INS.Columns.Add("INS08", GetType(String))
            CACHE_INS.Columns.Add("INS09", GetType(String))
            CACHE_INS.Columns.Add("INS10", GetType(String))
            CACHE_INS.Columns.Add("INS11", GetType(String))
            CACHE_INS.Columns.Add("INS12", GetType(String))
            CACHE_INS.Columns.Add("INS13", GetType(String))
            CACHE_INS.Columns.Add("INS14", GetType(String))
            CACHE_INS.Columns.Add("INS15", GetType(String))
            CACHE_INS.Columns.Add("INS16", GetType(String))
            CACHE_INS.Columns.Add("INS17", GetType(String))
            CACHE_INS.Columns.Add("BATCH_ID", GetType(Double))
            CACHE_INS.Columns.Add("TIME_STAMP", GetType(DateTime))

            CACHE_MSG.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            CACHE_MSG.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            CACHE_MSG.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            CACHE_MSG.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            CACHE_MSG.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            CACHE_MSG.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            CACHE_MSG.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            CACHE_MSG.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            CACHE_MSG.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            CACHE_MSG.Columns.Add("P_GUID", GetType(Guid))
            CACHE_MSG.Columns.Add("sP_GUID", GetType(String))
            CACHE_MSG.Columns.Add("HL_PARENT", GetType(String))
            CACHE_MSG.Columns.Add("MSG01", GetType(String))
            CACHE_MSG.Columns.Add("MSG02", GetType(String))
            CACHE_MSG.Columns.Add("MSG03", GetType(String))
            CACHE_MSG.Columns.Add("MSG04", GetType(String))
            CACHE_MSG.Columns.Add("MSG05", GetType(String))
            CACHE_MSG.Columns.Add("MSG06", GetType(String))
            CACHE_MSG.Columns.Add("MSG07", GetType(String))
            CACHE_MSG.Columns.Add("BATCH_ID", GetType(Double))
            CACHE_MSG.Columns.Add("TIME_STAMP", GetType(DateTime))


            CACHE_N3.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            CACHE_N3.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            CACHE_N3.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            CACHE_N3.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            CACHE_N3.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            CACHE_N3.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            CACHE_N3.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            CACHE_N3.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            CACHE_N3.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            CACHE_N3.Columns.Add("P_GUID", GetType(Guid))
            CACHE_N3.Columns.Add("HL_PARENT", GetType(String))
            CACHE_N3.Columns.Add("N301", GetType(String))
            CACHE_N3.Columns.Add("N302", GetType(String))
            CACHE_N3.Columns.Add("BATCH_ID", GetType(Double))
            CACHE_N3.Columns.Add("TIME_STAMP", GetType(DateTime))

            CACHE_N4.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            CACHE_N4.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            CACHE_N4.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            CACHE_N4.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            CACHE_N4.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            CACHE_N4.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            CACHE_N4.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            CACHE_N4.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            CACHE_N4.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            CACHE_N4.Columns.Add("P_GUID", GetType(Guid))
            CACHE_N4.Columns.Add("HL_PARENT", GetType(String))
            CACHE_N4.Columns.Add("N401", GetType(String))
            CACHE_N4.Columns.Add("N402", GetType(String))
            CACHE_N4.Columns.Add("N403", GetType(String))
            CACHE_N4.Columns.Add("N404", GetType(String))
            CACHE_N4.Columns.Add("N405", GetType(String))
            CACHE_N4.Columns.Add("N406", GetType(String))
            CACHE_N4.Columns.Add("N407", GetType(String))
            CACHE_N4.Columns.Add("BATCH_ID", GetType(Double))
            CACHE_N4.Columns.Add("TIME_STAMP", GetType(DateTime))

            CACHE_NM1.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            CACHE_NM1.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            CACHE_NM1.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            CACHE_NM1.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            CACHE_NM1.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            CACHE_NM1.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            CACHE_NM1.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            CACHE_NM1.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            CACHE_NM1.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            CACHE_NM1.Columns.Add("P_GUID", GetType(Guid))
            CACHE_NM1.Columns.Add("HL_PARENT", GetType(String))
            CACHE_NM1.Columns.Add("NM101", GetType(String))
            CACHE_NM1.Columns.Add("NM102", GetType(String))
            CACHE_NM1.Columns.Add("NM103", GetType(String))
            CACHE_NM1.Columns.Add("NM104", GetType(String))
            CACHE_NM1.Columns.Add("NM105", GetType(String))
            CACHE_NM1.Columns.Add("NM106", GetType(String))
            CACHE_NM1.Columns.Add("NM107", GetType(String))
            CACHE_NM1.Columns.Add("NM108", GetType(String))
            CACHE_NM1.Columns.Add("NM109", GetType(String))
            CACHE_NM1.Columns.Add("NM110", GetType(String))
            CACHE_NM1.Columns.Add("NM111", GetType(String))
            CACHE_NM1.Columns.Add("BATCH_ID", GetType(Double))
            CACHE_NM1.Columns.Add("TIME_STAMP", GetType(DateTime))

            CACHE_PER.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            CACHE_PER.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            CACHE_PER.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            CACHE_PER.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            CACHE_PER.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            CACHE_PER.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            CACHE_PER.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            CACHE_PER.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            CACHE_PER.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            CACHE_PER.Columns.Add("P_GUID", GetType(Guid))
            CACHE_PER.Columns.Add("HL_PARENT", GetType(String))
            CACHE_PER.Columns.Add("PER01", GetType(String))
            CACHE_PER.Columns.Add("PER02", GetType(String))
            CACHE_PER.Columns.Add("PER03", GetType(String))
            CACHE_PER.Columns.Add("PER04", GetType(String))
            CACHE_PER.Columns.Add("PER05", GetType(String))
            CACHE_PER.Columns.Add("PER06", GetType(String))
            CACHE_PER.Columns.Add("PER07", GetType(String))
            CACHE_PER.Columns.Add("PER08", GetType(String))
            CACHE_PER.Columns.Add("PER09", GetType(String))
            CACHE_PER.Columns.Add("BATCH_ID", GetType(Double))
            CACHE_PER.Columns.Add("TIME_STAMP", GetType(DateTime))

            CACHE_PRV.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            CACHE_PRV.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            CACHE_PRV.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            CACHE_PRV.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            CACHE_PRV.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            CACHE_PRV.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            CACHE_PRV.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            CACHE_PRV.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            CACHE_PRV.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            CACHE_PRV.Columns.Add("P_GUID", GetType(Guid))
            CACHE_PRV.Columns.Add("HL_PARENT", GetType(String))
            CACHE_PRV.Columns.Add("PRV01", GetType(String))
            CACHE_PRV.Columns.Add("PRV02", GetType(String))
            CACHE_PRV.Columns.Add("PRV03", GetType(String))
            CACHE_PRV.Columns.Add("PRV04", GetType(String))
            CACHE_PRV.Columns.Add("PRV05", GetType(String))
            CACHE_PRV.Columns.Add("PRV06", GetType(String))
            CACHE_PRV.Columns.Add("BATCH_ID", GetType(Double))
            CACHE_PRV.Columns.Add("TIME_STAMP", GetType(DateTime))

            CACHE_REF.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            CACHE_REF.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            CACHE_REF.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            CACHE_REF.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            CACHE_REF.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            CACHE_REF.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            CACHE_REF.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            CACHE_REF.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            CACHE_REF.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            CACHE_REF.Columns.Add("P_GUID", GetType(Guid))
            CACHE_REF.Columns.Add("HL_PARENT", GetType(String))
            CACHE_REF.Columns.Add("REF01", GetType(String))
            CACHE_REF.Columns.Add("REF02", GetType(String))
            CACHE_REF.Columns.Add("REF03", GetType(String))
            CACHE_REF.Columns.Add("BATCH_ID", GetType(Double))
            CACHE_REF.Columns.Add("TIME_STAMP", GetType(DateTime))

            CACHE_TRN.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            CACHE_TRN.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            CACHE_TRN.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            CACHE_TRN.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            CACHE_TRN.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            CACHE_TRN.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            CACHE_TRN.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            CACHE_TRN.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            CACHE_TRN.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            CACHE_TRN.Columns.Add("P_GUID", GetType(Guid))
            CACHE_TRN.Columns.Add("HL_PARENT", GetType(String))
            CACHE_TRN.Columns.Add("TRN01", GetType(String))
            CACHE_TRN.Columns.Add("TRN02", GetType(String))
            CACHE_TRN.Columns.Add("TRN03", GetType(String))
            CACHE_TRN.Columns.Add("TRN04", GetType(String))
            CACHE_TRN.Columns.Add("BATCH_ID", GetType(Double))
            CACHE_TRN.Columns.Add("TIME_STAMP", GetType(DateTime))

            N3_N4.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            N3_N4.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            N3_N4.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            N3_N4.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            N3_N4.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            N3_N4.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            N3_N4.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            N3_N4.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            N3_N4.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            N3_N4.Columns.Add("P_GUID", GetType(Guid))
            N3_N4.Columns.Add("HL_PARENT", GetType(String))
            N3_N4.Columns.Add("N301", GetType(String))
            N3_N4.Columns.Add("N302", GetType(String))
            N3_N4.Columns.Add("N401", GetType(String))
            N3_N4.Columns.Add("N402", GetType(String))
            N3_N4.Columns.Add("N403", GetType(String))
            N3_N4.Columns.Add("N404", GetType(String))
            N3_N4.Columns.Add("N405", GetType(String))
            N3_N4.Columns.Add("N406", GetType(String))
            N3_N4.Columns.Add("BATCH_ID", GetType(Double))
            N3_N4.Columns.Add("TIME_STAMP", GetType(DateTime))




            ENVELOP.Columns.Add("ROW_ID", GetType(Integer))
            ENVELOP.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            ENVELOP.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            ENVELOP.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            ENVELOP.Columns.Add("HIPAA_HL01_GUID", GetType(Guid))
            ENVELOP.Columns.Add("HIPAA_HL02_GUID", GetType(Guid))
            ENVELOP.Columns.Add("HIPAA_HL03_GUID", GetType(Guid))
            ENVELOP.Columns.Add("HIPAA_HL04_GUID", GetType(Guid))
            ENVELOP.Columns.Add("HIPAA_EB_GUID", GetType(Guid))
            ENVELOP.Columns.Add("P_GUID", GetType(Guid))
            ENVELOP.Columns.Add("HL_PARENT", GetType(String))
            ENVELOP.Columns.Add("ISA01", GetType(String))
            ENVELOP.Columns.Add("ISA02", GetType(String))
            ENVELOP.Columns.Add("ISA03", GetType(String))
            ENVELOP.Columns.Add("ISA04", GetType(String))
            ENVELOP.Columns.Add("ISA05", GetType(String))
            ENVELOP.Columns.Add("ISA06", GetType(String))
            ENVELOP.Columns.Add("ISA07", GetType(String))
            ENVELOP.Columns.Add("ISA08", GetType(String))
            ENVELOP.Columns.Add("ISA09", GetType(String))
            ENVELOP.Columns.Add("ISA10", GetType(String))
            ENVELOP.Columns.Add("ISA11", GetType(String))
            ENVELOP.Columns.Add("ISA12", GetType(String))
            ENVELOP.Columns.Add("ISA13", GetType(String))
            ENVELOP.Columns.Add("ISA14", GetType(String))
            ENVELOP.Columns.Add("ISA15", GetType(String))
            ENVELOP.Columns.Add("ISA16", GetType(String))
            ENVELOP.Columns.Add("GS01", GetType(String))
            ENVELOP.Columns.Add("GS02", GetType(String))
            ENVELOP.Columns.Add("GS03", GetType(String))
            ENVELOP.Columns.Add("GS04", GetType(String))
            ENVELOP.Columns.Add("GS05", GetType(String))
            ENVELOP.Columns.Add("GS06", GetType(String))
            ENVELOP.Columns.Add("GS07", GetType(String))
            ENVELOP.Columns.Add("GS08", GetType(String))
            ENVELOP.Columns.Add("ST01", GetType(String))
            ENVELOP.Columns.Add("ST02", GetType(String))
            ENVELOP.Columns.Add("ST03", GetType(String))
            ENVELOP.Columns.Add("BHT01", GetType(String))
            ENVELOP.Columns.Add("BHT02", GetType(String))
            ENVELOP.Columns.Add("BHT03", GetType(String))
            ENVELOP.Columns.Add("BHT04", GetType(String))
            ENVELOP.Columns.Add("BHT05", GetType(String))
            ENVELOP.Columns.Add("BHT06", GetType(String))
            ENVELOP.Columns.Add("SE01", GetType(String))
            ENVELOP.Columns.Add("SE02", GetType(String))
            ENVELOP.Columns.Add("GE01", GetType(String))
            ENVELOP.Columns.Add("GE02", GetType(String))
            ENVELOP.Columns.Add("IEA01", GetType(String))
            ENVELOP.Columns.Add("IEA02", GetType(String))
            ENVELOP.Columns.Add("BATCH_ID", GetType(Double))
            ENVELOP.Columns.Add("TIME_STAMP", GetType(DateTime))



            TEBUID.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            TEBUID.Columns.Add("P_GUID", GetType(Guid))
            TEBUID.Columns.Add("ROWPROCESSED", GetType(Integer))
            TEBUID.Columns.Add("TIME_STAMP", GetType(DateTime))










            '835 tables
            'Public BPR As New DataTable
            'Public CAS As New DataTable
            'Public CLP As New DataTable
            'Public DTM As New DataTable
            'Public LQ As New DataTable
            'Public LX As New DataTable
            'Public MIA As New DataTable
            'Public MOA As New DataTable
            'Public N1 As New DataTable
            'Public PLB As New DataTable
            'Public QTY As New DataTable
            'Public TS2 As New DataTable
            'Public TS3 As New DataTable
            'Public SVC As New DataTable
            'Public RESPONSE As New DataTable




            BPR.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            BPR.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            BPR.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            BPR.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            BPR.Columns.Add("HIPAA_LX_GUID", GetType(Guid))
            BPR.Columns.Add("HIPAA_CLP_GUID", GetType(Guid))
            BPR.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
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
            BPR.Columns.Add("BATCH_ID", GetType(Double))
            BPR.Columns.Add("TIME_STAMP", GetType(DateTime))

            CLP.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            CLP.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            CLP.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            CLP.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            CLP.Columns.Add("HIPAA_LX_GUID", GetType(Guid))
            CLP.Columns.Add("HIPAA_CLP_GUID", GetType(Guid))
            CLP.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            CLP.Columns.Add("CLP01", GetType(String))
            CLP.Columns.Add("CLP02", GetType(String))
            CLP.Columns.Add("CLP03", GetType(String))
            CLP.Columns.Add("CLP04", GetType(String))
            CLP.Columns.Add("CLP05", GetType(String))
            CLP.Columns.Add("CLP06", GetType(String))
            CLP.Columns.Add("CLP07", GetType(String))
            CLP.Columns.Add("CLP08", GetType(String))
            CLP.Columns.Add("CLP09", GetType(String))
            CLP.Columns.Add("CLP10", GetType(String))
            CLP.Columns.Add("CLP11", GetType(String))
            CLP.Columns.Add("CLP12", GetType(String))
            CLP.Columns.Add("CLP13", GetType(String))
            CLP.Columns.Add("BATCH_ID", GetType(Double))
            CLP.Columns.Add("TIME_STAMP", GetType(DateTime))

            DTM.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            DTM.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            DTM.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            DTM.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            DTM.Columns.Add("HIPAA_LX_GUID", GetType(Guid))
            DTM.Columns.Add("HIPAA_CLP_GUID", GetType(Guid))
            DTM.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            DTM.Columns.Add("DTM01", GetType(String))
            DTM.Columns.Add("DTM02", GetType(String))
            DTM.Columns.Add("BATCH_ID", GetType(Double))
            DTM.Columns.Add("TIME_STAMP", GetType(DateTime))


            MIA.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            MIA.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            MIA.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            MIA.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            MIA.Columns.Add("HIPAA_LX_GUID", GetType(Guid))
            MIA.Columns.Add("HIPAA_CLP_GUID", GetType(Guid))
            MIA.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
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
            MIA.Columns.Add("BATCH_ID", GetType(Double))
            MIA.Columns.Add("TIME_STAMP", GetType(DateTime))

            MOA.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            MOA.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            MOA.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            MOA.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            MOA.Columns.Add("HIPAA_LX_GUID", GetType(Guid))
            MOA.Columns.Add("HIPAA_CLP_GUID", GetType(Guid))
            MOA.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            MOA.Columns.Add("MOA01", GetType(String))
            MOA.Columns.Add("MOA02", GetType(String))
            MOA.Columns.Add("MOA03", GetType(String))
            MOA.Columns.Add("MOA04", GetType(String))
            MOA.Columns.Add("MOA05", GetType(String))
            MOA.Columns.Add("MOA06", GetType(String))
            MOA.Columns.Add("MOA07", GetType(String))
            MOA.Columns.Add("MOA08", GetType(String))
            MOA.Columns.Add("MOA09", GetType(String))
            MOA.Columns.Add("BATCH_ID", GetType(Double))
            MOA.Columns.Add("TIME_STAMP", GetType(DateTime))


            PLB.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            PLB.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            PLB.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            PLB.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            PLB.Columns.Add("HIPAA_LX_GUID", GetType(Guid))
            PLB.Columns.Add("HIPAA_CLP_GUID", GetType(Guid))
            PLB.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
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
            PLB.Columns.Add("BATCH_ID", GetType(Double))
            PLB.Columns.Add("TIME_STAMP", GetType(DateTime))



            QTY.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            QTY.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            QTY.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            QTY.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            QTY.Columns.Add("HIPAA_LX_GUID", GetType(Guid))
            QTY.Columns.Add("HIPAA_CLP_GUID", GetType(Guid))
            QTY.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            QTY.Columns.Add("DTM01", GetType(String))
            QTY.Columns.Add("DTM02", GetType(String))
            QTY.Columns.Add("BATCH_ID", GetType(Double))
            QTY.Columns.Add("TIME_STAMP", GetType(DateTime))


            SVC.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            SVC.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            SVC.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            SVC.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            SVC.Columns.Add("HIPAA_LX_GUID", GetType(Guid))
            SVC.Columns.Add("HIPAA_CLP_GUID", GetType(Guid))
            SVC.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
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
            SVC.Columns.Add("BATCH_ID", GetType(Double))
            SVC.Columns.Add("TIME_STAMP", GetType(DateTime))

            TS2.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            TS2.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            TS2.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            TS2.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            TS2.Columns.Add("HIPAA_LX_GUID", GetType(Guid))
            TS2.Columns.Add("HIPAA_CLP_GUID", GetType(Guid))
            TS2.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
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
            TS2.Columns.Add("BATCH_ID", GetType(Double))
            TS2.Columns.Add("TIME_STAMP", GetType(DateTime))


            TS3.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            TS3.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            TS3.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            TS3.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            TS3.Columns.Add("HIPAA_LX_GUID", GetType(Guid))
            TS3.Columns.Add("HIPAA_CLP_GUID", GetType(Guid))
            TS3.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
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
            TS3.Columns.Add("BATCH_ID", GetType(Double))
            TS3.Columns.Add("TIME_STAMP", GetType(DateTime))

            'end 835 tables





        End Sub




    End Class

End Namespace

