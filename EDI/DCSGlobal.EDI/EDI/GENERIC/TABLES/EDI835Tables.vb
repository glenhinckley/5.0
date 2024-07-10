Option Explicit On
Option Strict On
Option Compare Binary





Imports System.Data


Namespace DCSGlobal.EDI


    Public Class EDI835Tables


        Public ENVELOP As New DataTable
        Public N3_N4 As New DataTable

        Public EDI_271 As New DataTable


        'ENVELOP EDI TAbles

        Public ISA As New DataTable
        Public GS As New DataTable
        Public ST As New DataTable
        Public BHT As New DataTable


        Public BPR As New DataTable
        Public NM1 As New DataTable

        Public N1 As New DataTable
        Public N3 As New DataTable
        Public N4 As New DataTable

        Public TRN As New DataTable

        Public LX As New DataTable

        Public LQ As New DataTable

        Public TS2 As New DataTable
        Public TS3 As New DataTable

        Public PER As New DataTable
        Public REF As New DataTable


        Public QTY As New DataTable

        Public CLP As New DataTable
        Public CAS As New DataTable

        Public SVC As New DataTable
        Public AMT As New DataTable
        Public DTM As New DataTable
        Public MIA As New DataTable
        Public MOA As New DataTable

        Public UNK As New DataTable
        Public PLB As New DataTable

        'end 835 tables






        Public Sub BuildTables()


            'EDI 271

            EDI_271.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            EDI_271.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            EDI_271.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            EDI_271.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            EDI_271.Columns.Add("HIPAA_LX_GUID", GetType(Guid))
            EDI_271.Columns.Add("HIPAA_CLP_GUID", GetType(Guid))
            EDI_271.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
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
            ISA.Columns.Add("BATCH_ID", GetType(Double))
            ISA.Columns.Add("TIME_STAMP", GetType(DateTime))

            GS.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            GS.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            GS.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            GS.Columns.Add("P_GUID", GetType(Guid))
            GS.Columns.Add("GS01", GetType(String))
            GS.Columns.Add("GS02", GetType(String))
            GS.Columns.Add("GS03", GetType(String))
            GS.Columns.Add("GS04", GetType(String))
            GS.Columns.Add("GS05", GetType(String))
            GS.Columns.Add("GS06", GetType(String))
            GS.Columns.Add("GS07", GetType(String))
            GS.Columns.Add("GS08", GetType(String))
            GS.Columns.Add("BATCH_ID", GetType(Double))
            GS.Columns.Add("TIME_STAMP", GetType(DateTime))


            ST.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            ST.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            ST.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            ST.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            ST.Columns.Add("P_GUID", GetType(Guid))
            ST.Columns.Add("ST01", GetType(String))
            ST.Columns.Add("ST02", GetType(String))
            ST.Columns.Add("ST03", GetType(String))
            ST.Columns.Add("BATCH_ID", GetType(Double))
            ST.Columns.Add("TIME_STAMP", GetType(DateTime))



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
            UNK.Columns.Add("HIPAA_LX_GUID", GetType(Guid))
            UNK.Columns.Add("HIPAA_CLP_GUID", GetType(Guid))
            UNK.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            UNK.Columns.Add("P_GUID", GetType(Guid))

            UNK.Columns.Add("ROW_RECORD_TYPE", GetType(String))
            UNK.Columns.Add("ROW_DATA", GetType(String))
            UNK.Columns.Add("BATCH_ID", GetType(Double))
            UNK.Columns.Add("TIME_STAMP", GetType(DateTime))





            AMT.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            AMT.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            AMT.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            AMT.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            AMT.Columns.Add("HIPAA_LX_GUID", GetType(Guid))
            AMT.Columns.Add("HIPAA_CLP_GUID", GetType(Guid))
            AMT.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            AMT.Columns.Add("P_GUID", GetType(Guid))
            AMT.Columns.Add("AMT01", GetType(String))
            AMT.Columns.Add("AMT02", GetType(String))
            AMT.Columns.Add("AMT03", GetType(String))
            AMT.Columns.Add("BATCH_ID", GetType(Double))
            AMT.Columns.Add("TIME_STAMP", GetType(DateTime))


            CAS.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            CAS.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            CAS.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            CAS.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            CAS.Columns.Add("HIPAA_LX_GUID", GetType(Guid))
            CAS.Columns.Add("HIPAA_CLP_GUID", GetType(Guid))
            CAS.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            CAS.Columns.Add("P_GUID", GetType(Guid))
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









            N1.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            N1.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            N1.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            N1.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            N1.Columns.Add("HIPAA_LX_GUID", GetType(Guid))
            N1.Columns.Add("HIPAA_CLP_GUID", GetType(Guid))
            N1.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            N1.Columns.Add("P_GUID", GetType(Guid))
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
            N3.Columns.Add("HIPAA_LX_GUID", GetType(Guid))
            N3.Columns.Add("HIPAA_CLP_GUID", GetType(Guid))
            N3.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            N3.Columns.Add("P_GUID", GetType(Guid))
            N3.Columns.Add("N301", GetType(String))
            N3.Columns.Add("N302", GetType(String))
            N3.Columns.Add("BATCH_ID", GetType(Double))
            N3.Columns.Add("TIME_STAMP", GetType(DateTime))


            N4.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            N4.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            N4.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            N4.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            N4.Columns.Add("HIPAA_LX_GUID", GetType(Guid))
            N4.Columns.Add("HIPAA_CLP_GUID", GetType(Guid))
            N4.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            N4.Columns.Add("P_GUID", GetType(Guid))
            N4.Columns.Add("N401", GetType(String))
            N4.Columns.Add("N402", GetType(String))
            N4.Columns.Add("N403", GetType(String))
            N4.Columns.Add("N404", GetType(String))
            N4.Columns.Add("N405", GetType(String))
            N4.Columns.Add("N406", GetType(String))
            N4.Columns.Add("N407", GetType(String))
            N4.Columns.Add("BATCH_ID", GetType(Double))
            N4.Columns.Add("TIME_STAMP", GetType(DateTime))














            TRN.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            TRN.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            TRN.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            TRN.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            TRN.Columns.Add("HIPAA_LX_GUID", GetType(Guid))
            TRN.Columns.Add("HIPAA_CLP_GUID", GetType(Guid))
            TRN.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            TRN.Columns.Add("P_GUID", GetType(Guid))
            TRN.Columns.Add("TRN01", GetType(String))
            TRN.Columns.Add("TRN02", GetType(String))
            TRN.Columns.Add("TRN03", GetType(String))
            TRN.Columns.Add("TRN04", GetType(String))
            TRN.Columns.Add("BATCH_ID", GetType(Double))
            TRN.Columns.Add("TIME_STAMP", GetType(DateTime))







            N3_N4.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            N3_N4.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            N3_N4.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            N3_N4.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            N3_N4.Columns.Add("HIPAA_LX_GUID", GetType(Guid))
            N3_N4.Columns.Add("HIPAA_CLP_GUID", GetType(Guid))
            N3_N4.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
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
            ENVELOP.Columns.Add("HIPAA_LX_GUID", GetType(Guid))
            ENVELOP.Columns.Add("HIPAA_CLP_GUID", GetType(Guid))
            ENVELOP.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
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

            LX.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            LX.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            LX.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            LX.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            LX.Columns.Add("HIPAA_LX_GUID", GetType(Guid))
            LX.Columns.Add("HIPAA_CLP_GUID", GetType(Guid))
            LX.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            LX.Columns.Add("P_GUID", GetType(Guid))
            LX.Columns.Add("LX01", GetType(String))
            LX.Columns.Add("BATCH_ID", GetType(Double))
            LX.Columns.Add("TIME_STAMP", GetType(DateTime))



            BPR.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            BPR.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            BPR.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            BPR.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            BPR.Columns.Add("P_GUID", GetType(Guid))
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




            PER.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            PER.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            PER.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            PER.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            PER.Columns.Add("P_GUID", GetType(Guid))
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


            CLP.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            CLP.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            CLP.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            CLP.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            CLP.Columns.Add("HIPAA_LX_GUID", GetType(Guid))
            CLP.Columns.Add("HIPAA_CLP_GUID", GetType(Guid))
            CLP.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            CLP.Columns.Add("P_GUID", GetType(Guid))
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




            NM1.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            NM1.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            NM1.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            NM1.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            NM1.Columns.Add("HIPAA_LX_GUID", GetType(Guid))
            NM1.Columns.Add("HIPAA_CLP_GUID", GetType(Guid))
            NM1.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            '    NM1.Columns.Add("P_GUID", GetType(Guid))
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




            REF.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            REF.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            REF.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            REF.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            REF.Columns.Add("HIPAA_LX_GUID", GetType(Guid))
            REF.Columns.Add("HIPAA_CLP_GUID", GetType(Guid))
            REF.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            '  REF.Columns.Add("P_GUID", GetType(Guid))
            REF.Columns.Add("REF01", GetType(String))
            REF.Columns.Add("REF02", GetType(String))
            REF.Columns.Add("REF03", GetType(String))

            REF.Columns.Add("BATCH_ID", GetType(Double))
            REF.Columns.Add("TIME_STAMP", GetType(DateTime))



            DTM.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            DTM.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            DTM.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            DTM.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            DTM.Columns.Add("HIPAA_LX_GUID", GetType(Guid))
            DTM.Columns.Add("HIPAA_CLP_GUID", GetType(Guid))
            DTM.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            DTM.Columns.Add("P_GUID", GetType(Guid))
            DTM.Columns.Add("DTM01", GetType(String))
            DTM.Columns.Add("DTM02", GetType(String))
            DTM.Columns.Add("BATCH_ID", GetType(Double))
            DTM.Columns.Add("TIME_STAMP", GetType(DateTime))



            LQ.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            LQ.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            LQ.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            LQ.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            LQ.Columns.Add("HIPAA_LX_GUID", GetType(Guid))
            LQ.Columns.Add("HIPAA_CLP_GUID", GetType(Guid))
            LQ.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            LQ.Columns.Add("HIPAA_CAS_GUID", GetType(Guid))
            LQ.Columns.Add("P_GUID", GetType(Guid))
            LQ.Columns.Add("LQ02", GetType(String))
            LQ.Columns.Add("LQ01", GetType(String))
            LQ.Columns.Add("BATCH_ID", GetType(Double))
            LQ.Columns.Add("TIME_STAMP", GetType(DateTime))



            MIA.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            MIA.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            MIA.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            MIA.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            MIA.Columns.Add("HIPAA_LX_GUID", GetType(Guid))
            MIA.Columns.Add("HIPAA_CLP_GUID", GetType(Guid))
            MIA.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            MIA.Columns.Add("P_GUID", GetType(Guid))
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
            MOA.Columns.Add("P_GUID", GetType(Guid))
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
            PLB.Columns.Add("P_GUID", GetType(Guid))
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
            QTY.Columns.Add("QTY01", GetType(String))
            QTY.Columns.Add("QTY02", GetType(String))
            QTY.Columns.Add("BATCH_ID", GetType(Double))
            QTY.Columns.Add("TIME_STAMP", GetType(DateTime))


            SVC.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            SVC.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            SVC.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            SVC.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            SVC.Columns.Add("HIPAA_LX_GUID", GetType(Guid))
            SVC.Columns.Add("HIPAA_CLP_GUID", GetType(Guid))
            SVC.Columns.Add("HIPAA_SVC_GUID", GetType(Guid))
            SVC.Columns.Add("P_GUID", GetType(Guid))
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
            TS2.Columns.Add("P_GUID", GetType(Guid))
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
            TS3.Columns.Add("P_GUID", GetType(Guid))
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

