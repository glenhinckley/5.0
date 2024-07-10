
'--------------------------------------------------------------------
'Class Name: CNoPostBz
'Date: 09/06/2001
'Author: Eric Pena
'Description:  MTS business object designed to   methods associated
'              with the CNoPostDB class.
'--------------------------------------------------------------------
Option Explicit  On
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CNoPostBz


    Private _ConnectionString As String = String.Empty


    Private Const CLASS_NAME As String = "CNoPostBz"
    Private Const TABLE_NAME As String = "tblNonPostable"

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property



    Public Function Insert(ByVal lngCheckID As Long, ByVal curPostAmount As Decimal, ByVal curAmtDisallow As Decimal, ByVal curAmtPatient As Decimal, _
                            ByVal lngPatientID As Long, ByVal lngProviderID As Long, ByVal lngInsuranceID As Long, ByVal dteDOS As Date, ByVal strCPT As String, _
                            ByVal strReason As String, ByVal lngDupeCheck As Long, ByVal dteAction As Date, ByVal strAction As String, ByVal lngClaimID As Long, _
                            ByVal strDupeCheckNum As String, ByVal dteDupeCheckDate As Date, ByVal strNotes As String, ByVal strUserName As String) As Long
        '--------------------------------------------------------------------
        'Date: 09/06/2001
        'Author: Eric Pena
        'Description:  Inserts a row into the tblNonPostable table utilizing a stored procedure.
        'Parameters: Each parameter identifies the column value that will be  inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------
        Dim objNP As New BillingDB.CNoPostDB


        Insert = objNP.Insert(lngCheckID, curPostAmount, curAmtDisallow, curAmtPatient, lngPatientID, lngProviderID, lngInsuranceID, dteDOS, strCPT, _
                            strReason, lngDupeCheck, dteAction, strAction, lngClaimID, strDupeCheckNum, dteDupeCheckDate, Trim(strNotes), strUserName)
        objNP = Nothing


    End Function
    Public Sub Update(ByVal lngNPID As Long, ByVal lngOldCheckID As Long, ByVal curOldAmt As Decimal, ByVal lngCheckID As Long, ByVal curPostAmount As Decimal, ByVal curAmtDisallow As Decimal, ByVal curAmtPatient As Decimal, _
                            ByVal lngPatientID As Long, ByVal lngProviderID As Long, ByVal lngInsuranceID As Long, ByVal dteDOS As Date, ByVal strCPT As String, _
                            ByVal strReason As String, ByVal lngDupeCheck As Long, ByVal dteAction As Date, ByVal strAction As String, ByVal lngClaimID As Long, _
                            ByVal strDupeCheckNum As String, ByVal dteDupeCheckDate As Date, ByVal strNotes As String, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 09/06/2001
        'Author: Eric Pena
        'Description:  Updates a row in the tblNonPostable table utilizing a stored procedure.
        'Parameters: Each parameter identifies the column value that will be inserted.
        'Returns: Null
        '--------------------------------------------------------------------
        Dim objNP As New BillingDB.CNoPostDB


        objNP.Update(lngNPID, lngOldCheckID, curOldAmt, lngCheckID, curPostAmount, curAmtDisallow, curAmtPatient, lngPatientID, lngProviderID, lngInsuranceID, _
                            dteDOS, strCPT, strReason, lngDupeCheck, dteAction, strAction, lngClaimID, strDupeCheckNum, dteDupeCheckDate, Trim(strNotes), strUserName)
        objNP = Nothing


    End Sub
    Public Sub Process(ByVal lngNPID As Long, ByVal blnIsRefunded As Boolean, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 09/06/2001
        'Author: Eric Pena
        'Description:  Processed a row in the tblNonPostable table
        'Parameters: lngNPID - PK of the row in the tblNonPostable table to update
        '               blnIsRefunded - If true, will mark the row as refunded.  false will mark the row as posted
        'Returns: Null
        '--------------------------------------------------------------------
        Dim objNP As New BillingDB.CNoPostDB
        Dim strErrMessage As String



        If objNP.isProcessed(lngNPID) Then
            strErrMessage = "This entry has already been processed."
            Exit Sub
        End If

        objNP.Process(lngNPID, blnIsRefunded, strUserName)


        objNP = Nothing

    End Sub
    Public Sub Delete(ByVal lngID As Long, ByVal lngCheckID As Long, ByVal curAmt As Decimal, ByVal strUserName As String)
        '--------------------------------------------------------------------
        'Date: 09/06/2001
        'Author: Eric Pena
        'Description:  Deletes a row in the tblNonPostable table
        'Parameters: lngID - ID of the row in the table whose value will be deleted.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim objNP As New BillingDB.CNoPostDB
        Dim strError As String



        strError = ""

        objNP.Delete(lngID, lngCheckID, curAmt, strUserName)
        objNP = Nothing


    End Sub
    Public Function FetchByID(ByVal lngNPID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/06/2001
        'Author: Eric Pena
        'Description:  Returns rows from the tblNonPostable table utilizing a stored procedure
        'Parameters: PK for row in tblNonPostable that will be fetched.
        'Returns: ADORst
        '--------------------------------------------------------------------

        Dim objNP As New BillingDB.CNoPostDB


        FetchByID = objNP.FetchByID(lngNPID)

        objNP = Nothing

    End Function
    Public Function FetchByCriteria(ByVal dteStartAdded As Date, ByVal dteEndAdded As Date, ByVal dteStartAction As Date, ByVal dteEndAction As Date, _
                                        ByVal strAction As String, ByVal lngPatientID As Long, ByVal strCheckNum As String, ByVal lngInsuranceID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/06/2001
        'Author: Eric Pena
        'Description:  Returns rows from the tblNonPostable table utilizing a stored procedure
        'Parameters: Each parameter identifies the column value that will be filtered.
        'Returns: ADORst
        '--------------------------------------------------------------------

        Dim objNP As New BillingDB.CNoPostDB


        FetchByCriteria = objNP.FetchByCriteria(dteStartAdded, dteEndAdded, dteStartAction, dteEndAction, strAction, lngPatientID, strCheckNum, lngInsuranceID)

        objNP = Nothing

    End Function
End Class
