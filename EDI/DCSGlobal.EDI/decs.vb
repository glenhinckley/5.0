Option Explicit On
Option Strict On


Namespace DCSGlobal.EDI


    Public Class Declarations

        Property SBR_ID As Integer = 0
        Property ParseTree As Integer = 0
        Property NPI As String = String.Empty
        Property _1p As String = String.Empty



        Property _CommandTimeOut As Integer = 90




        Property _STID As Integer = -1


        Property Version As String = "3.0"

        Property _MaxRowCount As Double = 5000

        Property PAYOR_ID As String = String.Empty


        '***************************************************************************************************************************************************
        'IDs
        '***************************************************************************************************************************************************
        Property FILE_ID As Double = 0
        Property ISA_ID As Double = 0
        Property GS_ID As Double = 0
        Property ST_ID As Double = 0


        Property HL_01 As Integer = 0
        Property HL_02 As Integer = 0
        Property HL_03 As Integer = 0
        Property HL_04 As Integer = 0

        '***************************************************************************************************************************************************
        'GUIDS
        '***************************************************************************************************************************************************

        Property HL_20_GUID As Guid = Guid.Empty
        Property HL_21_GUID As Guid = Guid.Empty
        Property HL_22_GUID As Guid = Guid.Empty
        Property HL_23_GUID As Guid = Guid.Empty
        Property HL_24_GUID As Guid = Guid.Empty







        Property SBR_GUID As Guid = Guid.Empty
        Property HL_GUID As Guid = Guid.Empty
        Property BPR_GUID As Guid = Guid.Empty

        Property HL20UID As Guid = Guid.Empty
        Property HL21UID As Guid = Guid.Empty
        Property HL22UID As Guid = Guid.Empty
        Property HL23UID As Guid = Guid.Empty
        Property HL24UID As Guid = Guid.Empty


        Property BATCHUID As Guid = Guid.Empty
        Property GSUID As Guid = Guid.Empty
        Property PUID As Guid = Guid.Empty



        Property P_GUID As Guid = Guid.Empty
        Property ISA_GUID As Guid = Guid.Empty
        Property GS_GUID As Guid = Guid.Empty
        Property ST_GUID As Guid = Guid.Empty
        Property HL20_GUID As Guid = Guid.Empty
        Property HL21_GUID As Guid = Guid.Empty
        Property HL22_GUID As Guid = Guid.Empty
        Property HL23_GUID As Guid = Guid.Empty
        Property EB_GUID As Guid = Guid.Empty


        '835 guids
        Property LX_GUID As Guid = Guid.Empty
        Property CLP_GUID As Guid = Guid.Empty
        Property SVC_GUID As Guid = Guid.Empty
        Property CLM_GUID As Guid = Guid.Empty



        Property BHT_GUID As Guid = Guid.Empty
        Property HL20GUID As Guid = Guid.Empty
        Property HL21GUID As Guid = Guid.Empty
        Property HL22GUID As Guid = Guid.Empty
        Property HL23GUID As Guid = Guid.Empty


        '278 GUIDS
        Property HIPAA_NM1_GUID As Guid = Guid.Empty
        Property HIPAA_HCR_GUID As Guid = Guid.Empty
        Property HIPAA_UM_GUID As Guid = Guid.Empty




        Property CuurrentRowMSG_P_GUID As Guid
        Property LASTRowMSGPUID As Guid

        Property MSG_HIPAA_ISA_GUID As Guid = Guid.Empty
        Property MSG_HIPAA_GS_GUID As Guid = Guid.Empty
        Property MSG_HIPAA_HL1_GUID As Guid = Guid.Empty
        Property MSG_HIPAA_HL2_GUID As Guid = Guid.Empty
        Property MSG_HIPAA_HL3_GUID As Guid = Guid.Empty
        Property MSG_HIPAA_HL4_GUID As Guid = Guid.Empty
        Property MSG_HIPAA_EB_GUID As Guid = Guid.Empty
        Property MSG_P_GUID As Guid = Guid.Empty












        Property Verbose As Integer = 0



        Property BuildNumber As Integer = 1


        Property _BatchID As Double = 0
        Property commit As Integer = 0
        Property DebugLevel As Integer = 0
        Property CombineMSG As Integer = 0

        Property RDLRevLevel As Integer = 0

        Property edi As String = String.Empty
        Property edi_type As String = String.Empty







        ''Property _Debug As String = "N"

        Property debug As Double = 0
        Property Err As Double = 0 ' error flag  
        Property ErrRecordID As Double = 0  'returns the record id that holds any errors WHILE processing  
        Property ErrLogOn As Double = 0  ' turns err logoing on and off  
        Property ErrMsg As String = String.Empty


        Property TimeStamp As DateTime
        Property myBatchID As Double = 0


        Property length As Double = 0
        Property LineLenght As Double = 0
        Property LenghtCounter As Double = 0
        Property RowCounter As Double = 0
        Property RowProcessedFlag As Integer = 0

        Property CurrentRowRecordType As String = String.Empty
        Property CurrentRowData As String = String.Empty
        Property CurrentRowLength As Double = 0
        Property CurrentParentID As Guid
        Property CurrentHLLevel As String = String.Empty


        Property milliseconds As Double = 0

        Property idx_EDI271 As Double = 0

        Property CuurrentRow As Double = 0
        Property SegmentTerminator As String = "~"
        Property DataElementSeparator As String = "*"
        Property SubElementSeparator As String = ":"
        Property CarrotDataDelimiter As String = "^"
        Property ComponentElementSeperator As String = String.Empty
        ' ISA/IEA vars  
        Property ISAFlag As Double = 0
        Property ISAbiginterChangeControlNumber As String = String.Empty
        Property IEAbiginterChangeControlNumber As String = String.Empty
        Property IEAFunctionalGroupCount As Integer = 0  ' number of GS groups that are included in the ISA Block  
        Property IEAData As String = String.Empty
        Property ISAIdentity As Double = 0
        Property ISAUID As Guid

        ' GS/GE vars  
        Property GSFlag As Double = 0
        'dim CurrentGSRow as double  ' used to process GS records       
        'dim GSRowCount as double  ' this is retirived FROM the IEA col 2  
        Property GSIdentity As Double = 0

        'dim GSGroupControlNumber as double  
        'dim GSTransActionControlSetCountInGS as double  
        'dim GSTransActionControlSetCountActual as double  




        ' st/ BHT vars  

        Property STIdentity As Double = 0  ' indenty for the transaction  header   



        ' HL FLAGS  

        Property HL1Indentity As Double = 0
        Property HL01Data As String = String.Empty
        Property HL02Data As String = String.Empty
        Property HL03Data As String = String.Empty
        Property HL04Data As String = String.Empty

        Property HL1TranactionCanceledFlag As Integer = 0


        Property HL20Flag As Integer = 0
        Property HL21Flag As Integer = 0
        Property HL22Flag As Integer = 0
        Property HL23Flag As Integer = 0

        Property HL20FlagComplete As Integer = 0
        Property HL21FlagComplete As Integer = 0
        Property HL22FlagComplete As Integer = 0
        Property HL23FlagComplete As Integer = 0

        Property HL20FlagCount As Integer = 0
        Property HL21FlagCount As Integer = 0
        Property HL22FlagCount As Integer = 0
        Property HL23FlagCount As Integer = 0

        Property HL20FlagIndex As Integer = 0
        Property HL21FlagIndex As Integer = 0
        Property HL22FlagIndex As Integer = 0
        Property HL23FlagIndex As Integer = 0


        Property CurrentHLLevelChildCount As Integer = 0
        Property CurrentHLLevelChildCountIndex As Integer = 0


        Property CurrentHLGroup As String = String.Empty



        Property HL1CHILDCOUNT As Integer = 0
        Property HL2CHILDCOUNT As Integer = 0
        Property HL3CHILDCOUNT As Integer = 0
        Property HL4CHILDCOUNT As Integer = 0

        Property HLCANCELFLAG As Integer = 0


        ' General  



        Property SenderID As String = String.Empty  'right padded with spaces  this is who we sent it for   
        Property ReciverID As String = String.Empty 'right padded with spaces  this is who we got it back FROM    

        ' Provider Inofrmation  

        Property ProviderName As String = String.Empty
        Property ProviderAddress As String = String.Empty
        Property ProviderCity As String = String.Empty
        Property ProviderState As String = String.Empty
        Property ProviderZip As String = String.Empty




        Property ediRowData As String = String.Empty
        Property ediRowRecordType As String = String.Empty
        Property ediTildeCount As Double = 0







        '835 joe





        Property isDirtyBPR As Integer = 0
        Property isDirtyCAS As Integer = 0
        Property isDirtyCLP As Integer = 0
        Property isDirtyDTM As Integer = 0
        Property isDirtyLQ As Integer = 0
        Property isDirtyLX As Integer = 0
        Property isDirtyMIA As Integer = 0
        Property isDirtyMOA As Integer = 0
        Property isDirtyN1 As Integer = 0
        Property isDirtyPLB As Integer = 0
        Property isDirtyQTY As Integer = 0
        Property isDirtyTS2 As Integer = 0
        Property isDirtyTS3 As Integer = 0
        Property isDirtySVC As Integer = 0
        Property isDirtyRESPONSE As Integer = 0

        '835 end


        '835

        Property isDirtyMasterBPR As Integer = 0
        Property isDirtyMasterCAS As Integer = 0
        Property isDirtyMasterCLP As Integer = 0
        Property isDirtyMasterDTM As Integer = 0
        Property isDirtyMasterLQ As Integer = 0
        Property isDirtyMasterLX As Integer = 0
        Property isDirtyMasterMIA As Integer = 0
        Property isDirtyMasterMOA As Integer = 0
        Property isDirtyMasterN1 As Integer = 0
        Property isDirtyMasterPLB As Integer = 0
        Property isDirtyMasterQTY As Integer = 0
        Property isDirtyMasterTS2 As Integer = 0
        Property isDirtyMasterTS3 As Integer = 0
        Property isDirtyMasterSVC As Integer = 0
        Property isDirtyMasterRESPONSE As Integer = 0


        '835 end



        Property EBi As Integer = 0

        Property EBUID As Guid
        Property EBFlag As Integer = 0
        Property EBCarrotFlag As Integer = 0
        Property EBCarrotCount As Integer = 0
        Property EBEB03RowData As String = String.Empty
        Property EBEB03Data As String = String.Empty
        Property EBLASTRow As String = String.Empty
        Property EBLastCarrot As String = String.Empty
        Property EBcount As Integer = 0
        Property EBCarrotCHAR As String = String.Empty


        Property EEEB03RowData As String = String.Empty
        Property EEEB03Data As String = String.Empty



        Property isDirtyAAA As Integer = 0
        Property isDirtyAMT As Integer = 0
        Property isDirtyDMG As Integer = 0
        Property isDirtyDTP As Integer = 0
        Property isDirtyEQ As Integer = 0
        Property isDirtyHSD As Integer = 0
        Property isDirtyIII As Integer = 0
        Property isDirtyINS As Integer = 0
        Property isDirtyMSG As Integer = 0
        Property isDirtyN3 As Integer = 0
        Property isDirtyN4 As Integer = 0
        Property isDirtyNM1 As Integer = 0
        Property isDirtyPER As Integer = 0
        Property isDirtyPRV As Integer = 0
        Property isDirtyREF As Integer = 0
        Property isDirtyTRN As Integer = 0

        Property isDirtyCACHEAAA As Integer = 0
        Property isDirtyCACHEAMT As Integer = 0
        Property isDirtyCACHEDMG As Integer = 0
        Property isDirtyCACHEDTP As Integer = 0
        Property isDirtyCACHEEQ As Integer = 0
        Property isDirtyCACHEHSD As Integer = 0
        Property isDirtyCACHEIII As Integer = 0
        Property isDirtyCACHEINS As Integer = 0
        Property isDirtyCACHEMSG As Integer = 0
        Property isDirtyCACHEN3 As Integer = 0
        Property isDirtyCACHEN4 As Integer = 0
        Property isDirtyCACHENM1 As Integer = 0
        Property isDirtyCACHEPER As Integer = 0
        Property isDirtyCACHEPRV As Integer = 0
        Property isDirtyCACHEREF As Integer = 0
        Property isDirtyCACHETRN As Integer = 0

        Property isDirtyMasterAAA As Integer = 0
        Property isDirtyMasterAMT As Integer = 0
        Property isDirtyMasterDMG As Integer = 0
        Property isDirtyMasterDTP As Integer = 0
        Property isDirtyMasterEQ As Integer = 0
        Property isDirtyMasterHSD As Integer = 0
        Property isDirtyMasterIII As Integer = 0
        Property isDirtyMasterINS As Integer = 0
        Property isDirtyMasterMSG As Integer = 0
        Property isDirtyMasterNM1 As Integer = 0
        Property isDirtyMasterPER As Integer = 0
        Property isDirtyMasterPRV As Integer = 0
        Property isDirtyMasterREF As Integer = 0
        Property isDirtyMasterTRN As Integer = 0
        Property isDirtyMasterN34 As Integer = 0
        Property isDirtyMasterEB As Integer = 0
        Property isDirtyMasterUNK As Integer = 0



        Property ESFlagStart As Integer = 0
        Property ESFlag As Integer = 0
        Property EDIMatch As String = String.Empty


        Property DEBUG_GS_ID As Double = 0
        Property DEBUG_GS_COUNT As Double = 0
        Property DEBUG_ST_ID As Double = 0


        Property MSGEXFlag As Integer = 10000000
        Property MSGEX As String = String.Empty


        Property EBGroupByID As Integer = 0

        Property RowRecordType As String = String.Empty

        Property ISAROWID As Object

        Property P_UID As Guid = Guid.Empty

        Property ISA_UID As Guid = Guid.Empty

        Property ISA_ROW_ID As Integer = 0








        'end 835 guids



        Property ROW_RECORD_TYPE As String = String.Empty

        Property GS_ROW_ID As Integer = 0

        Property ST_ROW_ID As Integer = 0


        Property isDirtyMasterN4 As Integer

        Property isDirtyMasterN3 As Integer

        Property isDirtyCACHEHL As Integer

        Property isDirtyMasterHL As Integer

        Property isDirtyHL As Integer










        Property FULLMSGEX As String = String.Empty
        Property CuurrentRowMSG As Integer
        Property CuurrentRowMSGRowID As Integer

        Property CuurrentRowMSGFlag As Integer
        Property CuurrentRowMSGEXRowID As Integer


        Property MSG_HL_PARENT_HL01 As String
        Property MSGDLOOPCOUNTER As Integer
        Property MSGMEGEXLOOPCOUNTER As Integer






        Property ISA01 As String = String.Empty
        Property ISA02 As String = String.Empty
        Property ISA03 As String = String.Empty
        Property ISA04 As String = String.Empty
        Property ISA05 As String = String.Empty
        Property ISA06 As String = String.Empty
        Property ISA07 As String = String.Empty
        Property ISA08 As String = String.Empty
        Property ISA09 As String = String.Empty
        Property ISA10 As String = String.Empty
        Property ISA11 As String = String.Empty
        Property ISA12 As String = String.Empty
        Property ISA13 As String = String.Empty
        Property ISA14 As String = String.Empty
        Property ISA15 As String = String.Empty
        Property ISA16 As String = String.Empty

        Property IEA01 As String = String.Empty
        Property IEA02 As String = String.Empty


        Property GS01 As String = String.Empty
        Property GS02 As String = String.Empty
        Property GS03 As String = String.Empty
        Property GS04 As String = String.Empty
        Property GS05 As String = String.Empty
        Property GS06 As String = String.Empty
        Property GS07 As String = String.Empty
        Property GS08 As String = String.Empty

        Property GE01 As String = String.Empty
        Property GE02 As String = String.Empty

        Property ST01 As String = String.Empty
        Property ST02 As String = String.Empty
        Property ST03 As String = String.Empty

        Property SE01 As String = String.Empty
        Property SE02 As String = String.Empty

        Property BHT01 As String = String.Empty
        Property BHT02 As String = String.Empty
        Property BHT03 As String = String.Empty
        Property BHT04 As String = String.Empty
        Property BHT05 As String = String.Empty
        Property BHT06 As String = String.Empty


        Property _ConnectionString As String = String.Empty





        Property Dump As Integer = 0

        Property Status As String = String.Empty

        Property RejectReasonCode As String = String.Empty

        Property LoopAgain As String = String.Empty

        Property ebr_id As Double = 0

        Property user_id As String = String.Empty

        Property hosp_code As String = String.Empty

        Property source As String = String.Empty

        Property DeleteFlag As String = "N"

        Property SearchType As String = String.Empty

        Property XMLString As String = String.Empty

        Property Vendor_name As String = String.Empty

        Property Log_EDI As String = "N"

        Property pat_hosp_code As String

        Property Patient_number As String

        Property ins_type As String

        Property IEAFlag As Integer

        Property _FileID As Object

        Property STC_GUID As Guid

        Property cbr_id As Object

        Property DataElementSeparatorFlag As Integer

        Property rBatchId As Integer = 0

        Property _DeadLockRetrys As Integer = 0

        Property Folder As String

        Property LineTemiator As String

        Property _AAAFailureCode As String

        Property ServiceTypeCode As String

        Property EQDataSegementSeperator As String

        Property RAW270 As String = String.Empty

        Property DoAuditLog As Boolean = True

        Property _CSN As String = String.Empty

        Property _HAR As String = String.Empty

        Property _MRN As String = String.Empty

        Property _REVENUE_LOC As String = String.Empty

        Property _user_name As String = String.Empty

        Property _Loop_Counter As Integer = 0

        Property _dcs_payor_code As String = String.Empty


        Property LineTemiatorFlag As Integer = 0

        Property TA101 As String = String.Empty
        Property TA102 As String = String.Empty
        Property TA103 As String = String.Empty
        Property TA104 As String = String.Empty
        Property TA105 As String = String.Empty




        Public Function Clear() As Integer


            _BatchID = 0
            commit = 0
            DebugLevel = 0
            CombineMSG = 0

            RDLRevLevel = 0

            edi = String.empty
            edi_type = String.empty

            SearchType = String.Empty

            SBR_ID = 0
            ParseTree = 0
            NPI = String.Empty

            _AAAFailureCode = String.Empty

            ServiceTypeCode = String.Empty

            debug = 0
            Err = 0 ' error flag  
            ErrRecordID = 0  'returns the record id that holds any errors WHILE processing  
            ErrLogOn = 0  ' turns err logoing on and off  
            ErrMsg = String.empty



            myBatchID = 0


            length = 0
            LineLenght = 0
            LenghtCounter = 0
            RowCounter = 0
            RowProcessedFlag = 0

            CurrentRowRecordType = String.empty
            CurrentRowData = String.empty
            CurrentRowLength = 0

            CurrentHLLevel = String.empty


            milliseconds = 0

            idx_EDI271 = 0

            CuurrentRow = 0
            SegmentTerminator = "~"
            DataElementSeparator = "*"
            CarrotDataDelimiter = "^"
            ComponentElementSeperator = String.empty
            ' ISA/IEA vars  
            ISAFlag = 0
            ISAbiginterChangeControlNumber = String.empty
            IEAbiginterChangeControlNumber = String.empty
            IEAFunctionalGroupCount = 0  ' number of GS groups that are included in the ISA Block  
            IEAData = String.empty
            ISAIdentity = 0


            ' GS/GE vars  
            GSFlag = 0
            'dim CurrentGSRow as double  ' used to process GS records       
            'dim GSRowCount as double  ' this is retirived FROM the IEA col 2  
            GSIdentity = 0
            STIdentity = 0  ' indenty for the transaction  header   

            HL1Indentity = 0
            HL01Data = String.empty
            HL02Data = String.empty
            HL03Data = String.empty
            HL04Data = String.empty

            HL1TranactionCanceledFlag = 0


            HL20Flag = 0
            HL21Flag = 0
            HL22Flag = 0
            HL23Flag = 0

            HL20FlagComplete = 0
            HL21FlagComplete = 0
            HL22FlagComplete = 0
            HL23FlagComplete = 0

            HL20FlagCount = 0
            HL21FlagCount = 0
            HL22FlagCount = 0
            HL23FlagCount = 0

            HL20FlagIndex = 0
            HL21FlagIndex = 0
            HL22FlagIndex = 0
            HL23FlagIndex = 0


            CurrentHLLevelChildCount = 0
            CurrentHLLevelChildCountIndex = 0


            CurrentHLGroup = String.empty



            HL1CHILDCOUNT = 0
            HL2CHILDCOUNT = 0
            HL3CHILDCOUNT = 0
            HL4CHILDCOUNT = 0

            HLCANCELFLAG = 0


            ' General  



            SenderID = String.empty  'right padded with spaces  this is who we sent it for   
            ReciverID = String.empty 'right padded with spaces  this is who we got it back FROM    

            ' Provider Inofrmation  

            ProviderName = String.empty
            ProviderAddress = String.empty
            ProviderCity = String.empty
            ProviderState = String.empty
            ProviderZip = String.empty




            ediRowData = String.empty
            ediRowRecordType = String.empty
            ediTildeCount = 0









            EBi = 0


            EBFlag = 0
            EBCarrotFlag = 0
            EBCarrotCount = 0
            EBEB03RowData = String.empty
            EBEB03Data = String.empty
            EBLASTRow = String.empty
            EBLastCarrot = String.empty
            EBcount = 0
            EBCarrotCHAR = String.empty


            EEEB03RowData = String.empty
            EEEB03Data = String.empty



            isDirtyAAA = 0
            isDirtyAMT = 0
            isDirtyDMG = 0
            isDirtyDTP = 0
            isDirtyEQ = 0
            isDirtyHSD = 0
            isDirtyIII = 0
            isDirtyINS = 0
            isDirtyMSG = 0
            isDirtyN3 = 0
            isDirtyN4 = 0
            isDirtyNM1 = 0
            isDirtyPER = 0
            isDirtyPRV = 0
            isDirtyREF = 0
            isDirtyTRN = 0

            isDirtyCACHEAAA = 0
            isDirtyCACHEAMT = 0
            isDirtyCACHEDMG = 0
            isDirtyCACHEDTP = 0
            isDirtyCACHEEQ = 0
            isDirtyCACHEHSD = 0
            isDirtyCACHEIII = 0
            isDirtyCACHEINS = 0
            isDirtyCACHEMSG = 0
            isDirtyCACHEN3 = 0
            isDirtyCACHEN4 = 0
            isDirtyCACHENM1 = 0
            isDirtyCACHEPER = 0
            isDirtyCACHEPRV = 0
            isDirtyCACHEREF = 0
            isDirtyCACHETRN = 0

            isDirtyMasterAAA = 0
            isDirtyMasterAMT = 0
            isDirtyMasterDMG = 0
            isDirtyMasterDTP = 0
            isDirtyMasterEQ = 0
            isDirtyMasterHSD = 0
            isDirtyMasterIII = 0
            isDirtyMasterINS = 0
            isDirtyMasterMSG = 0
            isDirtyMasterNM1 = 0
            isDirtyMasterPER = 0
            isDirtyMasterPRV = 0
            isDirtyMasterREF = 0
            isDirtyMasterTRN = 0
            isDirtyMasterN34 = 0
            isDirtyMasterEB = 0
            isDirtyMasterUNK = 0



            ESFlagStart = 0
            ESFlag = 0
            EDIMatch = String.empty


            DEBUG_GS_ID = 0
            DEBUG_GS_COUNT = 0
            DEBUG_ST_ID = 0


            MSGEXFlag = 10000000
            MSGEX = String.empty


            EBGroupByID = 0

            RowRecordType = String.empty












            FULLMSGEX = String.empty







            ISA01 = String.empty
            ISA02 = String.empty
            ISA03 = String.empty
            ISA04 = String.empty
            ISA05 = String.empty
            ISA06 = String.empty
            ISA07 = String.empty
            ISA08 = String.empty
            ISA09 = String.empty
            ISA10 = String.empty
            ISA11 = String.empty
            ISA12 = String.empty
            ISA13 = String.empty
            ISA14 = String.empty
            ISA15 = String.empty
            ISA16 = String.empty
            GS01 = String.empty
            GS02 = String.empty
            GS03 = String.empty
            GS04 = String.empty
            GS05 = String.empty
            GS06 = String.empty
            GS07 = String.empty
            GS08 = String.empty
            ST01 = String.empty
            ST02 = String.empty
            ST03 = String.empty
            BHT01 = String.empty
            BHT02 = String.empty
            BHT03 = String.empty
            BHT04 = String.empty


            BHT06 = String.empty

            BHT05 = String.empty

            Dump = 0

            Status = String.empty

            RejectReasonCode = String.empty

            LoopAgain = String.empty

            ebr_id = 0

            user_id = String.empty

            hosp_code = String.empty

            source = String.empty
            RAW270 = String.empty

            DoAuditLog = False

            _CSN = String.empty

            _HAR = String.empty

            _MRN = String.empty

            _REVENUE_LOC = String.empty

            _user_name = String.empty

            _Loop_Counter = 0

            _dcs_payor_code = String.empty



            TA101 = String.Empty
            TA102 = String.Empty
            TA103 = String.Empty
            TA104 = String.Empty
            TA105 = String.Empty


            Return 0


        End Function
    End Class




End Namespace
