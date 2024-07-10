
'----------------------------------------------------------------------------
'Class Name: CPatientTxDB
'Date: 07/09/2002
'Author: Dave Richkun
'Description:  COM+ object designed to host methods associated with patient
'              transactions.
'----------------------------------------------------------------------------
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CInsuranceTxBz


    Private _ConnectionString As String = String.Empty





    Private Const CLASS_NAME As String = "CInsuranceTxBz"


    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property


    Public Function Insert(ByVal lngELID As Long, ByVal lngEncDetailID As Long, ByVal lngPatientID As Long, ByVal lngRPID As Long, _
                ByVal lngPlanID As Long, ByVal lngCPCID As Long, ByVal lngInsuranceID As Long, _
                ByVal strPayerCode As String, ByVal lngTxTypeID As Long, ByVal lngCptRecordID As Long, ByVal dblAmount As Double, _
                ByVal dtRcvDate As Date, ByVal strCheckNum As String, ByVal dtcheckDate As Date, _
                ByVal strTxNotes As String, ByVal dblPMF As Double, ByVal lngCheckID As Long, _
                ByVal strUserName As String, Optional ByVal lngUserID As Long = 0) As Long
        '--------------------------------------------------------------------
        'Date: 07/09/2002
        'Author: Dave Richkun
        'Description:  Inserts a row into the tblInsuranceTx table utilizing a stored procedure.
        'Parameters:
        '
        '   *** TO DO: Define each parameter once procedure is finalized ****
        '
        'Returns: ID of the row inserted.
        '--------------------------------------------------------------------

        Dim objTx As New BillingDB.CInsuranceTxDB

        Insert = objTx.Insert(lngELID, lngEncDetailID, lngPatientID, lngRPID, lngPlanID, lngCPCID, lngInsuranceID, _
                    strPayerCode, lngTxTypeID, lngCptRecordID, dblAmount, dtRcvDate, strCheckNum, dtcheckDate, _
                    strTxNotes, dblPMF, lngCheckID, strUserName, lngUserID)
        objTx = Nothing

    End Function

    Public Sub Update(ByVal lngELID As Long, ByVal lngEncDetailID As Long, ByVal lngPlanID As Long, _
            ByVal lngTxTypeID As Long, ByVal lngCptRecordID As Long, ByVal dblAmount As Double, _
            ByVal dtRcvDate As Date, ByVal strCheckNum As String, _
            ByVal dtcheckDate As Date, ByVal strTxNotes As String, _
            ByVal dblPMF As Double, ByVal lngCheckID As Long, _
            ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 08/02/2002
        'Author: Dave Richkun
        'Description:  Updates a row in tblInsuranceTx table utilizing a stored procedure.
        'Parameters:
        '
        '   *** TO DO: Define each parameter once procedure is finalized ****
        '
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objTx As New BillingDB.CInsuranceTxDB


        objTx.Update(lngELID, lngEncDetailID, lngPlanID, lngTxTypeID, lngCptRecordID, dblAmount, _
                dtRcvDate, strCheckNum, dtcheckDate, strTxNotes, dblPMF, _
                lngCheckID, strUserName)
        objTx = Nothing



    End Sub


    Public Sub CancelAfterSubmit(ByVal lngELID As Long, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 08/08/2002
        'Author: Dave Richkun
        'Description:  Inserts an offsetting transaction into tblInsuranceTx that offsets
        '              the insurance balance owing to a zero amount for a particular encounter.
        'Parameters:  lngELID - ID of the encounter for which insurance transactions are being
        '                zeroed out.
        '             strUserName - Name of user performing the cancellation
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objTx As New BillingDB.CInsuranceTxDB


        objTx.CancelAfterSubmit(lngELID, strUserName)
        objTx = Nothing



    End Sub

    Public Sub Resubmit(ByVal lngOrigELID As Long, ByVal lngNewELID As Long)
        '--------------------------------------------------------------------
        'Date: 08/09/2002
        'Author: Dave Richkun
        'Description:  Reassigns the Encounter Log ID foreign key from one set
        '              of insurance transactions to a new Encounter Log ID.  This
        '              method is used when a claim is resubmitted and a new encounter
        '              is created.  By reassigning the encounter log ID, the original
        '              insurance transactions are preserved when assigned to the new
        '              encounter.
        'Parameters:  lngOrigELID - ID of the original encounter - the ID of the encounter
        '               being cancelled
        '             lngNewELID - ID of the resubmitted encounter - the ID of the new
        '               encounter that supercedes all previous encounters.
        'Returns: Null
        '--------------------------------------------------------------------

        Dim objTx As New BillingDB.CInsuranceTxDB


        objTx.Resubmit(lngOrigELID, lngNewELID)
        objTx = Nothing

    End Sub



End Class
