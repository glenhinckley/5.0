Option Explicit On
Option Strict On
Option Compare Binary





Imports System.Data


Namespace DCSGlobal.EDI



    Public Class EDI997Tables





        Public ISA As New DataTable
        Public GS As New DataTable
        Public ST As New DataTable


        Public AK1 As New DataTable
        Public AK2 As New DataTable
        Public AK3 As New DataTable
        Public AK4 As New DataTable
        Public AK5 As New DataTable
        Public AK9 As New DataTable

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





            AK1.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            AK1.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            AK1.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            AK1.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            AK1.Columns.Add("AK101", GetType(String))
            AK1.Columns.Add("AK102", GetType(String))


            AK2.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            AK2.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            AK2.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            AK2.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            AK2.Columns.Add("AK201", GetType(String))
            AK2.Columns.Add("AK202", GetType(String))


            AK3.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            AK3.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            AK3.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            AK3.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            AK3.Columns.Add("AK301", GetType(String))
            AK3.Columns.Add("AK302", GetType(String))
            AK3.Columns.Add("AK303", GetType(String))
            AK3.Columns.Add("AK304", GetType(String))


            AK4.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            AK4.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            AK4.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            AK4.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            AK4.Columns.Add("AK401", GetType(String))
            AK4.Columns.Add("AK402", GetType(String))
            AK4.Columns.Add("AK403", GetType(String))
            AK4.Columns.Add("AK404", GetType(String))


            AK5.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            AK5.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            AK5.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            AK5.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            AK5.Columns.Add("AK101", GetType(String))
            AK5.Columns.Add("AK502", GetType(String))
            AK5.Columns.Add("AK503", GetType(String))
            AK5.Columns.Add("AK504", GetType(String))
            AK5.Columns.Add("AK505", GetType(String))
            AK5.Columns.Add("AK506", GetType(String))


            AK9.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            AK9.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            AK9.Columns.Add("HIPAA_GS_GUID", GetType(Guid))
            AK9.Columns.Add("HIPAA_ST_GUID", GetType(Guid))
            AK9.Columns.Add("AK901", GetType(String))
            AK9.Columns.Add("AK902", GetType(String))
            AK9.Columns.Add("AK903", GetType(String))
            AK9.Columns.Add("AK904", GetType(String))
            AK9.Columns.Add("AK905", GetType(String))
            AK9.Columns.Add("AK906", GetType(String))
            AK9.Columns.Add("AK907", GetType(String))
            AK9.Columns.Add("AK908", GetType(String))
            AK9.Columns.Add("AK909", GetType(String))


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



        End Sub






    End Class

End Namespace

