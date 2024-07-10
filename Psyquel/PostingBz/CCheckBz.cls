
'------------------------------------------------------------------------------------
'Class Name: CCheckBz
'Date: 09/25/00
'Author: Eric Pena
'Denis J Finkel
'Description:   MTS object designed to host methods associated with data affecting tblPayerCheck and tblCheckUsage tables.
'------------------------------------------------------------------------------------
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

Public Class CCheckBz
    Private _ConnectionString As String = String.Empty

    Private Const CLASS_NAME As String = "CCheckBz"
    Private Const TABLE_NAME As String = "tblPayerCheck"
    Private Const CONST_NO_DATE As Date = #12:00:00 AM#

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property




    Public Function Insert(ByVal lngInsuranceID As Long, _
                           ByVal strCheckNum As String, _
                           ByVal strDepositNum As String, _
                           ByVal dteCheckDate As Date, _
                           ByVal dteReceiveDate As Date, _
                           ByVal curTotalAmount As Decimal, _
                           ByVal strAddedBy As String, _
                           ByVal lngProviderID As Long, _
                           ByVal lngGroupID As Long, _
                            Optional ByVal blnPosted As Boolean = False, _
                            Optional ByVal strNotes As String = "", _
                            Optional ByVal blnElectronic As Boolean = False) As Long
        '------------------------------------------------------------------------------------
        'Date: 09/25/00
        'Author: Eric Pena
        'Description:   Inserts a row in to tblPayerCheck table utilizing
        '               a stored procedure.
        'Parameters:    Each parameter identifies the column value that will be inserted.
        'Returns:       ID (Primary Key) of the row inserted
        '------------------------------------------------------------------------------------

        Dim objCheck As New PostingDB.CCheckDB
        Dim strErrMessage As String
        objCheck.ConnectionString = _ConnectionString

        '  function to verify data inputed if false goes to ErrTrap
        'if True   Insert function in PostingDB object
        If Not blnElectronic Then
            If Not VerifyData(0, lngInsuranceID, strCheckNum, dteCheckDate, dteReceiveDate, strErrMessage) Then Exit Function
        End If

        Insert = objCheck.Insert(lngInsuranceID, strCheckNum, strDepositNum, dteCheckDate, dteReceiveDate, _
                                curTotalAmount, strAddedBy, lngProviderID, lngGroupID, blnPosted, strNotes, blnElectronic)

        objCheck = Nothing

    End Function
    Private Function VerifyData(ByVal lngCheckID As Long, _
                                ByVal lngInsuranceID As Long, _
                                ByVal strCheckNum As String, _
                                ByVal dteCheckDate As Date, _
                                ByVal dteReceiveDate As Date, _
                                ByVal strErrMessage As String) As Boolean
        '------------------------------------------------------------------------------------
        'Date: 09/25/00
        'Author: Eric Pena
        'Description:   Verifies that all required data has been provided by the user.
        'Parameters:    Values to be verified.
        'Returns:       Boolean value identifying if all required criteria has been satisfied
        '------------------------------------------------------------------------------------

        Dim rst As New ADODB.Recordset
        Dim objCheck As New PostingBz.CCheckBz
        objCheck.ConnectionString = _ConnectionString
        If lngInsuranceID <= 0 Then
            strErrMessage = "A Payer is required."
            VerifyData = False
            Exit Function
        End If

        If dteCheckDate.ToOADate() <> 0 Then
            If dteCheckDate > Date.Now Then
                strErrMessage = "Check Date is invalid."
                VerifyData = False
                Exit Function
            End If
        End If

        If dteReceiveDate.ToOADate() <> 0 Then
            If dteReceiveDate > Date.Now Then
                strErrMessage = "Receive Date is invalid."
                VerifyData = False
                Exit Function
            End If
        End If

        If lngInsuranceID > 0 Then
            If String.IsNullOrEmpty(strCheckNum) Then
                strErrMessage = "Check Number is Required."
                VerifyData = False
                Exit Function
            End If

            'If a Check Number is given, make sure it does not exist
            If Len(Trim(strCheckNum)) > 0 Then

                rst = objCheck.FetchByCriteria(lngInsuranceID, "='" & Trim(strCheckNum) & "'", dteCheckDate, "", 0, 0, False)
                If Not rst.EOF Then
                    strErrMessage = "Cannot enter a check that already exists!"
                    VerifyData = False
                    objCheck = Nothing
                    Exit Function
                End If
                objCheck = Nothing
            End If
            If Len(Trim(strCheckNum)) > 0 Then

                rst = objCheck.FetchByCriteria(0, "='" & Trim(strCheckNum) & "'", dteCheckDate, "", 0, 0, False)
                If Not rst.EOF Then

                    Dim d As DateTime

                    d = Convert.ToDateTime(rst.Fields("fldCheckDate").Value)

                    strErrMessage = "Cannot enter a check that already exists! " & rst.Fields("fldPayerName").Value & " " & d.ToShortDateString()

                    VerifyData = False
                    objCheck = Nothing
                    Exit Function
                End If
                objCheck = Nothing
            End If
        End If

        VerifyData = True

    End Function
    Public Function Fetch(ByVal lngInsuranceID As Long, Optional ByVal blnOnlyUnreconciled As Boolean = False) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 09/25/00
        'Author: Eric Pena
        'Description:   Retrieves records from the tblPayerCheck table utilizing a stored procedure.
        'Parameters:    blnIncludeDisabled- determines if disabled records will be returned.
        'Returns: ADODB.Recordset
        '-------------------------------------------------------------------------------------

        Dim objCheck As New PostingDB.CCheckDB


        Fetch = objCheck.Fetch(lngInsuranceID, blnOnlyUnreconciled)


        objCheck = Nothing


    End Function
    Public Sub Update(ByVal lngCheckID As Long, _
                        ByVal lngInsuranceID As Long, _
                        ByVal strCheckNum As String, _
                        ByVal strDepositNum As String, _
                        ByVal dteCheckDate As Date, _
                        ByVal dteReceiveDate As Date, _
                        ByVal curTotalAmount As Decimal, _
                        ByVal lngProviderID As Long, _
                        ByVal lngGroupID As Long, _
                        Optional ByVal blnPosted As Boolean = False, _
                        Optional ByVal strNotes As String = "", _
                        Optional ByVal blnElectronic As Boolean = False)
        '----------------------------------------------------------------------------------------
        'Date: 09/25/00
        'Author: Eric Pena
        'Description:   Updates a row in the tblPayerCheck table utilizing a stored procedure.
        'Parameters:    Each parameter identifies the column value that will be inserted.
        'Returns:       Null
        '----------------------------------------------------------------------------------------
        Dim objCheck As New PostingDB.CCheckDB
        Dim strErrMessage As String


        If Not blnElectronic Then
            If curTotalAmount < 0 Then
                strErrMessage = "You must specify a positive check amount"
                Exit Sub
            End If
        End If

        objCheck.Update(lngCheckID, lngInsuranceID, strCheckNum, strDepositNum, dteCheckDate, dteReceiveDate, curTotalAmount, lngProviderID, lngGroupID, blnPosted, strNotes, blnElectronic)

        objCheck = Nothing


    End Sub
    Public Function FetchByID(ByVal lngID As Long) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 09/25/00
        'Author: Eric Pena
        'Description:   Retrieves records from the tblPayerCheck table utilizing a stored procedure.
        'Parameters:    blnIncludeDisabled- determines if disabled records will be returned.
        'Returns: ADODB.Recordset
        '-------------------------------------------------------------------------------------

        Dim objCheck As New PostingDB.CCheckDB

        FetchByID = objCheck.FetchByID(lngID)



    End Function
    Public Function LogCheckUsage(ByVal lngCheckID As String, ByVal lngELID As Long, ByVal curTotalAmount As Decimal, ByVal strAddedBy As String)
        '------------------------------------------------------------------------------------
        'Date: 09/25/00
        'Author: Eric Pena
        'Description:   Inserts a row into the tblCheckUsage utilizing a stored procedure.
        'Parameters:    Each parameter identifies the column value that will be inserted.
        'Returns:       ID (Primary Key) of the row inserted.
        '------------------------------------------------------------------------------------

        Dim objCheck As New PostingDB.CCheckDB
        Dim strErrMessage As String



        If curTotalAmount < 0 Then
            strErrMessage = "You must specify a positive check amount"
            Exit Function
        End If

        objCheck.LogCheckUsage(lngCheckID, lngELID, curTotalAmount, strAddedBy)


    End Function
    Public Function FetchByCriteria(ByVal lngInsuranceID As Long, _
                                    ByVal strCheckNum As String, _
                                    ByVal dteCheckDate As Date, _
                                    ByVal strDepositNum As String, _
                                    ByVal lngProviderID As Long, _
                                    ByVal lngGroupID As Long, _
                                    Optional ByVal blnUnrec As Boolean = False) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 09/25/00
        'Author: Eric Pena
        'Description:   Retrieves records from the tblPayerCheck table utilizing a stored procedure.
        'Parameters:    all parameters represent values to search for.
        'Returns: ADODB.Recordset
        '-------------------------------------------------------------------------------------

        Dim objCheck As New PostingDB.CCheckDB

        FetchByCriteria = objCheck.FetchByCriteria(lngInsuranceID, _
                        strCheckNum, dteCheckDate, strDepositNum, blnUnrec, _
                        lngProviderID, lngGroupID)


        objCheck = Nothing



    End Function
    Public Function FetchPayments(ByVal lngID As Long) As ADODB.Recordset
        '-------------------------------------------------------------------------------------
        'Date: 09/25/00
        'Author: Eric Pena
        'Description:   Retrieves records from the tblPayerCheck table utilizing a stored procedure.
        'Parameters:    blnIncludeDisabled- determines if disabled records will be returned.
        'Returns: ADODB.Recordset
        '-------------------------------------------------------------------------------------

        Dim objCheck As New PostingDB.CCheckDB


        FetchPayments = objCheck.FetchPayments(lngID)



    End Function
    Public Sub Deleted(ByVal lngCheckID As Long, ByVal blnIsDeleted As Boolean, ByVal strDeletedBy As String)
        '----------------------------------------------------------------------------------------
        'Date: 09/25/00
        'Author: Eric Pena
        'Description:   Updates a row in the tblPayerCheck table utilizing a stored procedure.
        'Parameters:    Each parameter identifies the column value that will be inserted.
        'Returns:       Null
        '----------------------------------------------------------------------------------------
        Dim objCheck As New PostingDB.CCheckDB
        Dim rst As New ADODB.Recordset
        Dim strErrMessage As String



        If blnIsDeleted Then
            rst = objCheck.FetchPayments(lngCheckID)
            If Not rst.EOF Then
                strErrMessage = "Cannot delete a check that has already had postings tied to it!"
                Exit Sub
            End If
        End If

        objCheck.Deleted(lngCheckID, blnIsDeleted, strDeletedBy)

        objCheck = Nothing
        rst = Nothing



    End Sub
    Public Sub LockCheck(ByVal lngCheckID As Long, ByVal strUsername As String)
        '----------------------------------------------------------------------------------------
        'Date:          09/10/02
        'Author:        Eric Pena
        'Description:   Locks the specified check and transactions associated with it
        'Parameters:    lngCheckID = ID in tblPayerCheck for check to lock or -1 to lock all reconciled checks
        '----------------------------------------------------------------------------------------
        Dim objCheck As New PostingDB.CCheckDB
        Dim rst As New ADODB.Recordset
        Dim strErrMessage As String

        objCheck.LockCheck(lngCheckID, strUsername)
        objCheck = Nothing



    End Sub


End Class
