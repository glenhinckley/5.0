
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

    Public Class EDICommonDecs


        Public _TablesBuilt As Boolean = False



        Public _LoopLevelMajor As Integer = 1000
        Public _LoopLevelMinor As Integer = 0
        Public _LoopLevelSubFix As String = "A"


        Public _EDIList As New List(Of String)
        Public _EDI As String = String.Empty



        Public _ConnectionString As String = String.Empty


        Public SBR_ID As Integer = 0
        Public ParseTree As Integer = 0
        Public NPI As String = String.Empty
        Public _1p As String = String.Empty



        Public _CommandTimeOut As Integer = 90







        Public Version As String = "3.0"

        Public _MaxRowCount As Long = 5000

        Public PAYOR_ID As String = String.Empty

        Public _ROW_RECORD_TYPE As String = String.Empty


        '***************************************************************************************************************************************************
        'fLAGSs
        '***************************************************************************************************************************************************

        Public _ISAFlag As Integer = 0

        '***************************************************************************************************************************************************
        'IDs
        '***************************************************************************************************************************************************


        Public _DOCUMENT_ID As Long = 0
        Public _BATCH_ID As Long = 0
        Public _FILE_ID As Long = 0
        Public _ISA_ID As Long = 0
        Public _GS_ID As Long = 0
        Public _ST_ID As Long = 0

        Public _ISA_COUNT As Integer = 0
        Public _GS_COUNT As Integer = 0
        Public _ST_COUNT As Integer = 0


        Public _HL_01 As Integer = 0
        Public _HL_02 As Integer = 0
        Public _HL_03 As Integer = 0
        Public _HL_04 As Integer = 0

        '***************************************************************************************************************************************************
        'GUIDS
        '***************************************************************************************************************************************************

        Public _P_GUID As Guid = Guid.Empty


        Public _ISA_GUID As Guid = Guid.Empty
        Public _GS_GUID As Guid = Guid.Empty
        Public _ST_GUID As Guid = Guid.Empty
        Public _BHT_GUID As Guid = Guid.Empty


        Public _HIPAA_HL_20_GUID As Guid = Guid.Empty
        Public _HIPAA_HL_21_GUID As Guid = Guid.Empty
        Public _HIPAA_HL_22_GUID As Guid = Guid.Empty
        Public _HIPAA_HL_23_GUID As Guid = Guid.Empty
        Public _HIPAA_HL_24_GUID As Guid = Guid.Empty


        Public _HIPAA_ISA_GUID As Guid = Guid.Empty
        Public _HIPAA_GS_GUID As Guid = Guid.Empty
        Public _HIPAA_ST_GUID As Guid = Guid.Empty




        Public SBR_GUID As Guid = Guid.Empty
        Public HL_GUID As Guid = Guid.Empty
        Public BPR_GUID As Guid = Guid.Empty




        Public BATCHUID As Guid = Guid.Empty
        Public GSUID As Guid = Guid.Empty
        Public PUID As Guid = Guid.Empty





        '835 guids
        Public LX_GUID As Guid = Guid.Empty
        Public CLP_GUID As Guid = Guid.Empty
        Public SVC_GUID As Guid = Guid.Empty
        Public CLM_GUID As Guid = Guid.Empty






        '278 GUIDS
        Public HIPAA_NM1_GUID As Guid = Guid.Empty
        Public HIPAA_HCR_GUID As Guid = Guid.Empty
        Public HIPAA_UM_GUID As Guid = Guid.Empty




        Public CuurrentRowMSG_P_GUID As Guid
        Public LASTRowMSGPUID As Guid

        Public MSG_HIPAA_ISA_GUID As Guid = Guid.Empty
        Public MSG_HIPAA_GS_GUID As Guid = Guid.Empty
        Public MSG_HIPAA_HL1_GUID As Guid = Guid.Empty
        Public MSG_HIPAA_HL2_GUID As Guid = Guid.Empty
        Public MSG_HIPAA_HL3_GUID As Guid = Guid.Empty
        Public MSG_HIPAA_HL4_GUID As Guid = Guid.Empty
        Public MSG_HIPAA_EB_GUID As Guid = Guid.Empty
        Public MSG_P_GUID As Guid = Guid.Empty












        Public Verbose As Integer = 0



        Public BuildNumber As Integer = 1


        Public _BatchID As Long = 0
        Public commit As Integer = 0
        Public DebugLevel As Integer = 0
        Public CombineMSG As Integer = 0

        Public RDLRevLevel As Integer = 0

        Public edi As String = String.Empty
        Public edi_type As String = String.Empty







        ''public _Debug As String = "N"

        Public debug As Long = 0
        Public Err As Long = 0 ' error flag  
        Public ErrRecordID As Long = 0  'returns the record id that holds any errors WHILE processing  
        Public ErrLogOn As Long = 0  ' turns err logoing on and off  
        Public ErrMsg As String = String.Empty


        Public TimeStamp As DateTime
        Public myBatchID As Long = 0


        Public length As Long = 0
        Public _LineLenght As Long = 0
        Public _LenghtCounter As Long = 0
        Public _RowCounter As Long = 0
        Public _RowProcessedFlag As Integer = 0

        Public _CurrentRowRecordType As String = String.Empty
        Public _CurrentRowData As String = String.Empty
        Public _CurrentRowLength As Long = 0
        Public _CurrentParentID As Guid
        Public _CurrentHLLevel As String = String.Empty
        Public _DataElementSeparatorFlag As Integer

        Public milliseconds As Long = 0

        Public idx_EDI271 As Long = 0

        Public CuurrentRow As Long = 0
        Public _SegmentTerminator As String = "~"
        Public _DataElementSeparator As String = "*"
        Public _SubElementSeparator As String = ":"
        Public _CarrotDataDelimiter As String = "^"
        Public _ComponentElementSeperator As String = String.Empty
        ' ISA/IEA vars  
        Public ISAFlag As Long = 0
        Public ISAbiginterChangeControlNumber As String = String.Empty
        Public IEAbiginterChangeControlNumber As String = String.Empty
        Public IEAFunctionalGroupCount As Integer = 0  ' number of GS groups that are included in the ISA Block  
        Public IEAData As String = String.Empty
        Public ISAIdentity As Long = 0
        Public ISAUID As Guid

        ' GS/GE vars  
        Public GSFlag As Long = 0
        'dim CurrentGSRow as LONG  ' used to process GS records       
        'dim GSRowCount as LONG  ' this is retirived FROM the IEA col 2  
        Public GSIdentity As Long = 0

        'dim GSGroupControlNumber as LONG  
        'dim GSTransActionControlSetCountInGS as LONG  
        'dim GSTransActionControlSetCountActual as LONG  




        ' st/ BHT vars  

        Public STIdentity As Long = 0  ' indenty for the transaction  header   



        ' HL FLAGS  

        Public HL1Indentity As Long = 0
        Public HL01Data As String = String.Empty
        Public HL02Data As String = String.Empty
        Public HL03Data As String = String.Empty
        Public HL04Data As String = String.Empty

        Public HL1TranactionCanceledFlag As Integer = 0


        Public HL20Flag As Integer = 0
        Public HL21Flag As Integer = 0
        Public HL22Flag As Integer = 0
        Public HL23Flag As Integer = 0

        Public HL20FlagComplete As Integer = 0
        Public HL21FlagComplete As Integer = 0
        Public HL22FlagComplete As Integer = 0
        Public HL23FlagComplete As Integer = 0

        Public HL20FlagCount As Integer = 0
        Public HL21FlagCount As Integer = 0
        Public HL22FlagCount As Integer = 0
        Public HL23FlagCount As Integer = 0

        Public HL20FlagIndex As Integer = 0
        Public HL21FlagIndex As Integer = 0
        Public HL22FlagIndex As Integer = 0
        Public HL23FlagIndex As Integer = 0


        Public CurrentHLLevelChildCount As Integer = 0
        Public CurrentHLLevelChildCountIndex As Integer = 0


        Public CurrentHLGroup As String = String.Empty



        Public HL1CHILDCOUNT As Integer = 0
        Public HL2CHILDCOUNT As Integer = 0
        Public HL3CHILDCOUNT As Integer = 0
        Public HL4CHILDCOUNT As Integer = 0

        Public HLCANCELFLAG As Integer = 0


        ' General  



        Public SenderID As String = String.Empty  'right padded with spaces  this is who we sent it for   
        Public ReciverID As String = String.Empty 'right padded with spaces  this is who we got it back FROM    

        ' Provider Inofrmation  

        Public ProviderName As String = String.Empty
        Public ProviderAddress As String = String.Empty
        Public ProviderCity As String = String.Empty
        Public ProviderState As String = String.Empty
        Public ProviderZip As String = String.Empty




        '   Public _ediRowData As String = String.Empty
        '   Public _ediRowRecordType As String = String.Empty
        Public ediTildeCount As Long = 0







        '835 joe





        Public isDirtyBPR As Integer = 0
        Public isDirtyCAS As Integer = 0
        Public isDirtyCLP As Integer = 0
        Public isDirtyDTM As Integer = 0
        Public isDirtyLQ As Integer = 0
        Public isDirtyLX As Integer = 0
        Public isDirtyMIA As Integer = 0
        Public isDirtyMOA As Integer = 0
        Public isDirtyN1 As Integer = 0
        Public isDirtyPLB As Integer = 0
        Public isDirtyQTY As Integer = 0
        Public isDirtyTS2 As Integer = 0
        Public isDirtyTS3 As Integer = 0
        Public isDirtySVC As Integer = 0
        Public isDirtyRESPONSE As Integer = 0

        '835 end


        '835

        Public isDirtyMasterBPR As Integer = 0
        Public isDirtyMasterCAS As Integer = 0
        Public isDirtyMasterCLP As Integer = 0
        Public isDirtyMasterDTM As Integer = 0
        Public isDirtyMasterLQ As Integer = 0
        Public isDirtyMasterLX As Integer = 0
        Public isDirtyMasterMIA As Integer = 0
        Public isDirtyMasterMOA As Integer = 0
        Public isDirtyMasterN1 As Integer = 0
        Public isDirtyMasterPLB As Integer = 0
        Public isDirtyMasterQTY As Integer = 0
        Public isDirtyMasterTS2 As Integer = 0
        Public isDirtyMasterTS3 As Integer = 0
        Public isDirtyMasterSVC As Integer = 0
        Public isDirtyMasterRESPONSE As Integer = 0


        '835 end



        Public EBi As Integer = 0

        Public EBUID As Guid
        Public EBFlag As Integer = 0
        Public EBCarrotFlag As Integer = 0
        Public EBCarrotCount As Integer = 0
        Public EBEB03RowData As String = String.Empty
        Public EBEB03Data As String = String.Empty
        Public EBLASTRow As String = String.Empty
        Public EBLastCarrot As String = String.Empty
        Public EBcount As Integer = 0
        Public EBCarrotCHAR As String = String.Empty


        Public EEEB03RowData As String = String.Empty
        Public EEEB03Data As String = String.Empty



        Public isDirtyAAA As Integer = 0
        Public isDirtyAMT As Integer = 0
        Public isDirtyDMG As Integer = 0
        Public isDirtyDTP As Integer = 0
        Public isDirtyEQ As Integer = 0
        Public isDirtyHSD As Integer = 0
        Public isDirtyIII As Integer = 0
        Public isDirtyINS As Integer = 0
        Public isDirtyMSG As Integer = 0
        Public isDirtyN3 As Integer = 0
        Public isDirtyN4 As Integer = 0
        Public isDirtyNM1 As Integer = 0
        Public isDirtyPER As Integer = 0
        Public isDirtyPRV As Integer = 0
        Public isDirtyREF As Integer = 0
        Public isDirtyTRN As Integer = 0

        Public isDirtyCACHEAAA As Integer = 0
        Public isDirtyCACHEAMT As Integer = 0
        Public isDirtyCACHEDMG As Integer = 0
        Public isDirtyCACHEDTP As Integer = 0
        Public isDirtyCACHEEQ As Integer = 0
        Public isDirtyCACHEHSD As Integer = 0
        Public isDirtyCACHEIII As Integer = 0
        Public isDirtyCACHEINS As Integer = 0
        Public isDirtyCACHEMSG As Integer = 0
        Public isDirtyCACHEN3 As Integer = 0
        Public isDirtyCACHEN4 As Integer = 0
        Public isDirtyCACHENM1 As Integer = 0
        Public isDirtyCACHEPER As Integer = 0
        Public isDirtyCACHEPRV As Integer = 0
        Public isDirtyCACHEREF As Integer = 0
        Public isDirtyCACHETRN As Integer = 0

        Public isDirtyMasterAAA As Integer = 0
        Public isDirtyMasterAMT As Integer = 0
        Public isDirtyMasterDMG As Integer = 0
        Public isDirtyMasterDTP As Integer = 0
        Public isDirtyMasterEQ As Integer = 0
        Public isDirtyMasterHSD As Integer = 0
        Public isDirtyMasterIII As Integer = 0
        Public isDirtyMasterINS As Integer = 0
        Public isDirtyMasterMSG As Integer = 0
        Public isDirtyMasterNM1 As Integer = 0
        Public isDirtyMasterPER As Integer = 0
        Public isDirtyMasterPRV As Integer = 0
        Public isDirtyMasterREF As Integer = 0
        Public isDirtyMasterTRN As Integer = 0
        Public isDirtyMasterN34 As Integer = 0
        Public isDirtyMasterEB As Integer = 0
        Public isDirtyMasterUNK As Integer = 0



        Public ESFlagStart As Integer = 0
        Public ESFlag As Integer = 0
        Public EDIMatch As String = String.Empty


        Public DEBUG_GS_ID As Long = 0
        Public DEBUG_GS_COUNT As Long = 0
        Public DEBUG_ST_ID As Long = 0


        Public MSGEXFlag As Integer = 10000000
        Public MSGEX As String = String.Empty


        Public EBGroupByID As Integer = 0

        Public RowRecordType As String = String.Empty

        Public ISAROWID As Object

        Public P_UID As Guid = Guid.Empty

        Public ISA_UID As Guid = Guid.Empty

        Public ISA_ROW_ID As Integer = 0








        'end 835 guids





        Public GS_ROW_ID As Integer = 0

        Public ST_ROW_ID As Integer = 0


        Public isDirtyMasterN4 As Integer

        Public isDirtyMasterN3 As Integer

        Public isDirtyCACHEHL As Integer

        Public isDirtyMasterHL As Integer

        Public isDirtyHL As Integer










        Public FULLMSGEX As String = String.Empty
        Public CuurrentRowMSG As Integer
        Public CuurrentRowMSGRowID As Integer

        Public CuurrentRowMSGFlag As Integer
        Public CuurrentRowMSGEXRowID As Integer


        Public MSG_HL_PARENT_HL01 As String
        Public MSGDLOOPCOUNTER As Integer
        Public MSGMEGEXLOOPCOUNTER As Integer






        Public _ISA01 As String = String.Empty
        Public _ISA02 As String = String.Empty
        Public _ISA03 As String = String.Empty
        Public _ISA04 As String = String.Empty
        Public _ISA05 As String = String.Empty
        Public _ISA06 As String = String.Empty
        Public _ISA07 As String = String.Empty
        Public _ISA08 As String = String.Empty
        Public _ISA09 As String = String.Empty
        Public _ISA10 As String = String.Empty
        Public _ISA11 As String = String.Empty
        Public _ISA12 As String = String.Empty
        Public _ISA13 As String = String.Empty
        Public _ISA14 As String = String.Empty
        Public _ISA15 As String = String.Empty
        Public _ISA16 As String = String.Empty

        Public _IEA01 As String = String.Empty
        Public _IEA02 As String = String.Empty


        Public _GS01 As String = String.Empty
        Public _GS02 As String = String.Empty
        Public _GS03 As String = String.Empty
        Public _GS04 As String = String.Empty
        Public _GS05 As String = String.Empty
        Public _GS06 As String = String.Empty
        Public _GS07 As String = String.Empty
        Public _GS08 As String = String.Empty

        Public _GE01 As String = String.Empty
        Public _GE02 As String = String.Empty

        Public _ST01 As String = String.Empty
        Public _ST02 As String = String.Empty
        Public _ST03 As String = String.Empty

        Public _SE01 As String = String.Empty
        Public _SE02 As String = String.Empty

        Public _BHT01 As String = String.Empty
        Public _BHT02 As String = String.Empty
        Public _BHT03 As String = String.Empty
        Public _BHT04 As String = String.Empty
        Public _BHT05 As String = String.Empty
        Public _BHT06 As String = String.Empty







        Public Dump As Integer = 0

        Public Status As String = String.Empty

        Public RejectReasonCode As String = String.Empty

        Public LoopAgain As String = String.Empty

        Public ebr_id As Long = 0

        Public user_id As String = String.Empty

        Public hosp_code As String = String.Empty

        Public source As String = String.Empty

        Public DeleteFlag As String = "N"

        Public SearchType As String = String.Empty

        Public XMLString As String = String.Empty

        Public Vendor_name As String = String.Empty

        Public Log_EDI As String = "N"

        Public pat_hosp_code As String

        Public Patient_number As String

        Public ins_type As String

        Public IEAFlag As Integer

        Public _FileID As Object

        Public STC_GUID As Guid

        Public cbr_id As Object



        Public rBatchId As Integer = 0

        Public _DeadLockRetrys As Integer = 0

        Public Folder As String

        Public LineTemiator As String

        Public _AAAFailureCode As String

        Public ServiceTypeCode As String

        Public EQDataSegementSeperator As String

        Public RAW270 As String = String.Empty

        Public DoAuditLog As Boolean = True

        Public _CSN As String = String.Empty

        Public _HAR As String = String.Empty

        Public _MRN As String = String.Empty

        Public _REVENUE_LOC As String = String.Empty

        Public _user_name As String = String.Empty

        Public _Loop_Counter As Integer = 0

        Public _dcs_payor_code As String = String.Empty


        Public LineTemiatorFlag As Integer = 0

        Public TA101 As String = String.Empty
        Public TA102 As String = String.Empty
        Public TA103 As String = String.Empty
        Public TA104 As String = String.Empty
        Public TA105 As String = String.Empty

    End Class
End Namespace
