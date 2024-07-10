
'--------------------------------------------------------------------
'Class Name: CRPCredBz
'Date: 10/19/2000
'Author: Eric Pena
'Denis J Finkel
'Description:  MTS business object designed to   methods associated with the CRPCredDB class.
'--------------------------------------------------------------------
'Revision History:
'--------------------------------------------------------------------
Option Explicit On


Imports ADODB.CommandTypeEnum
Imports ADODB.CompareEnum
Imports ADODB.ParameterAttributesEnum



Imports ADODB.DataTypeEnum
Imports ADODB.ParameterDirectionEnum
Imports ADODB.ExecuteOptionEnum

Imports ADODB.CursorTypeEnum
Imports ADODB.CursorLocationEnum
Imports ADODB.CursorOptionEnum
Imports ADODB.LockTypeEnum
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CRPCredBz
    Public Const CONST_DIRECT_CNN As String = "Provider=SQLOLEDB.1;Password=psy1234!;Persist Security Info=True;User ID=sa;Initial Catalog=PsyquelDirect;Data Source=192.168.4.25"
    Public Const CONST_PROD_CNN As String = "Provider=SQLOLEDB.1;Password=psy1234!;Persist Security Info=True;User ID=sa;Initial Catalog=PsyquelProd;Data Source=192.168.4.25"
    Private Const CLASS_NAME As String = "CRPCredBz"
    Private Const TABLE_NAME As String = "tblRPProviderCredit"
    Private Const POST_TOCR As Long = 8
    Private Const POST_FRCR As Long = 9
    Private Const CONST_NO_CURR As Decimal = -666
    Private Const CONST_NO_DATE As Date = #12:00:00 AM#
    Public Function StoreCredit(ByVal lngID As Long, ByVal lngBRID As Long, ByVal curAmt As Decimal, ByVal strAddedBy As String) As Long
        '--------------------------------------------------------------------
        'Date: 10/19/2000
        'Author: Eric Pena
        'Description:  Modifies/Inserts a row into the tblRPProviderCredit  and tblTransaction table utilizing a stored procedure.
        'Parameters: lngid - the Id in tblRPProviderCredit to use, or -1 if a new record is to be added
        '                   lngelid - the EL to post from (will deduct from this EL)
        '                   curamt - amount to pot to credit
        'Returns: null
        '--------------------------------------------------------------------
        Dim objRPCred As New PostingDB.CRPCredDB
        Dim objPost As New PostingBz.CTxBz
        Dim rst As New ADODB.Recordset

        Dim strErrMessage As String


        rst = objPost.FetchBRByID(lngBRID)

        If Not VerifyData(-1, lngID, -1, curAmt, -1, strErrMessage) Then Exit Function

        'store the credit
        StoreCredit = objRPCred.StoreCredit(lngID, rst.Fields("fldEncounterLogID").Value, rst.Fields("fldPayerID_RP").Value, curAmt)
        objRPCred = Nothing

        'post the Tx
        objPost.Insert(lngBRID, strAddedBy, POST_TOCR, Abs(curAmt) * -1, CONST_NO_CURR, "", CONST_NO_DATE, "", True)
        objPost = Nothing

        'Signal successful completion

    End Function
    Public Sub ApplyCredit(ByVal lngID As Long, ByVal lngBRID As Long, ByVal curAmt As Decimal, ByVal strAddedBy As String)
        '--------------------------------------------------------------------
        'Date: 10/19/2000
        'Author: Eric Pena
        'Description:  Updates a row in the tblRPProviderCredit, inserts a row into the tbltransaction  table utilizing a stored procedure.
        'Parameters: lngID:  The ID of the row to be updated All other parameters identifies the column value that will be  updated.
        'Returns: Null
        '--------------------------------------------------------------------
        Dim objRPCred As New PostingDB.CRPCredDB
        Dim objPost As New PostingBz.CTxBz
        Dim rst As New ADODB.Recordset
        Dim strErrMessage As String


        rst = objPost.FetchBRByID(lngBRID)

        If Not VerifyData(1, lngID, rst.Fields("fldEncounterLogID").Value, curAmt, rst.Fields("fldPayerID_RP").Value, strErrMessage) Then Exit Sub

        objRPCred.ApplyCredit(lngID, curAmt)
        objRPCred = Nothing

        'post the Tx
        objPost.Insert(lngBRID, strAddedBy, POST_FRCR, curAmt, CONST_NO_CURR, "", "01\01\1900", "")
        objPost = Nothing

    End Sub
    Private Function VerifyData(ByVal lngMode As Long, ByVal lngID As Long, ByVal lngELID As Long, ByVal curAmt As Decimal, ByVal lngRPID As Long, ByRef strErrMessage As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 10/19/2000
        'Author: Eric Pena
        'Description:  Verifies all required data has been provided by the user.
        'Parameters:  The values to be checked.
        'Returns: Boolean value identifying if all criteria has been satisfied.
        '--------------------------------------------------------------------
        Dim objEL As New EncounterLogBZ.CEncounterLogBz
        Dim rstCred As New ADODB.Recordset
        Dim rstEL As New ADODB.Recordset



        If curAmt = 0 Then
            strErrMessage = "You must specify an amount."
            VerifyData = False
            Exit Function
        End If

        'posting TO a credit balance
        If curAmt < 0 Then
            strErrMessage = "You cannot post negative credits."
            VerifyData = False
            Exit Function
        End If

        If lngMode > 0 Then
            'POSTING FROM a credit
            If lngID < 0 Then
                strErrMessage = "You must specify the credit to post from."
                VerifyData = False
                Exit Function
            End If

            'make sure the credit source is valid
            rstCred = FetchByID(lngID)

            rstEL = objEL.FetchByID(lngELID)

            If rstCred.Fields("fldProviderID").Value <> rstEL.Fields("fldProviderID").Value Then
                strErrMessage = "Patient ID#" & rstEL.Fields("fldPatientID").Value & " does not have credit with the specified Provider"
                VerifyData = False
                Exit Function
            End If

            If rstCred.Fields("fldRPID").Value <> lngRPID Then
                strErrMessage = "The credit specified does not belong to the RP specified"
                VerifyData = False
                Exit Function
            End If

            If curAmt > rstCred.Fields("fldAmount").Value Then
                strErrMessage = "You are trying to apply " & Format(CCur(curAmt - rstCred.Fields("fldAmount").Value), "Decimal") & " more than the patient has credit for."
                VerifyData = False
                Exit Function
            End If
        End If

        'If we get here, all is well...
        VerifyData = True

        objEL = Nothing
        rstCred = Nothing
        rstEL = Nothing

    End Function
    Public Function FetchByID(ByVal lngID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/19/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblRPProviderCredit table ordered by patient name utilizing a stored procedure
        'Parameters: lngID - PK to return records for
        'Returns: ADORst
        '--------------------------------------------------------------------

        Dim objRPCred As New PostingDB.CRPCredDB


        FetchByID = objRPCred.FetchByID(lngID)

    End Function
    Public Function FetchByPatient(ByVal lngPatientID As Long, ByVal blnZeroBal As Boolean) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/19/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblRPProviderCredit table ordered by provider name utilizing a stored procedure
        'Parameters: lngProviderID - id of the patient  to return records for
        'Returns: ADORst
        '--------------------------------------------------------------------

        Dim objRPCred As New PostingDB.CRPCredDB

        FetchByPatient = objRPCred.FetchByPatient(lngPatientID, blnZeroBal)

        'Signal successful completion

    End Function
    Public Function Fetch(ByVal blnZeroBal As Boolean) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/19/2000
        'Author: Eric Pena
        'Description:  Returns a row from the tblRPProviderCredit table utilizing a stored procedure
        'Parameters: lngID - id of the row to return records for
        'Returns: ADORst
        '--------------------------------------------------------------------
        Dim objRPCred As New PostingDB.CRPCredDB



        Fetch = objRPCred.Fetch(blnZeroBal)


        objRPCred = Nothing

    End Function
    Public Function FetchByProvider(ByVal lngProviderID As Long, ByVal blnZeroBal As Boolean) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/19/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblRPProviderCredit table ordered by provider name utilizing a stored procedure
        'Parameters: lngProviderID - id of the patient  to return records for
        'Returns: ADORst
        '--------------------------------------------------------------------

        Dim objRPCred As New PostingDB.CRPCredDB

        FetchByProvider = objRPCred.FetchByPatient(lngProviderID, blnZeroBal)

        objRPCred = Nothing

    End Function
    Public Function FetchByEL(ByVal lngELID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/19/2000
        'Author: Eric Pena
        'Description:  Returns rows from the tblRPProviderCredit table ordered by patient name utilizing a stored procedure
        'Parameters: lngID - PK to return records for
        'Returns: ADORst
        '--------------------------------------------------------------------

        Dim objRPCred As New PostingDB.CRPCredDB

        FetchByEL = objRPCred.FetchByEL(lngELID)


    End Function
    Public Function FetchEligibleCredRefund(ByVal lngBRID As Long, ByVal curMinAmt As Decimal) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 6/1/2001
        'Author: Eric Pena
        'Description:  Finds a DOS for which an amount of curMinAmt can be moved to a credit balance
        'Parameters: lngBRID - BR ID of RP to post credit toward
        '               curMinAmt - amount that needs to be moved to a credit balance
        'Returns: ADORst
        '--------------------------------------------------------------------

        Dim objRPCred As New PostingDB.CRPCredDB

        FetchEligibleCredRefund = objRPCred.FetchEligibleCredRefund(lngBRID, curMinAmt)

    End Function



End Class
