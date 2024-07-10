Option Explicit On
Option Strict On
Option Compare Binary





Imports System.Data


Namespace DCSGlobal.EDI



    Public Class EDITA1Tables





        Public ISA As New DataTable
        Public TA1 As New DataTable


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


            TA1.Columns.Add("ROW_ID", GetType(Integer)).AutoIncrement = True
            TA1.Columns.Add("ROW_NUM", GetType(Integer))
            TA1.Columns.Add("HIPAA_ISA_GUID", GetType(Guid))
            TA1.Columns.Add("TA101", GetType(String))
            TA1.Columns.Add("TA102", GetType(String))
            TA1.Columns.Add("TA103", GetType(String))
            TA1.Columns.Add("TA104", GetType(String))
            TA1.Columns.Add("TA105", GetType(String))


        End Sub






    End Class

End Namespace

