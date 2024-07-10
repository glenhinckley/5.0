
'--------------------------------------------------------------------
'Class Name: CProvFeeBZ
'Date: 10/25/2000
'Author: Morgan Travis
'Description:  MTS object designed to host methods associated with
'               data affecting Provider Fee table.
'--------------------------------------------------------------------
'Revision History:
'  R001: 05/08/2001 Richkun: Added FetchByID() method
'  R002: 05/08/2001 Richkun: Altered Update() method to include CPTCode, ProviderID parameter
'  R003: 05/08/2001 Richkun: Added Delete() method
'--------------------------------------------------------------------
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CProvFeeBZ
    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CProvFeeBZ"


    Public Function Insert(ByVal lngProviderID As Long, _
                           ByVal strCPTCode As String, ByVal strCPTModifier As String, _
                           ByVal strCPTDescription As String, ByVal curFee As Decimal, _
                           ByVal curContractAmount As Decimal, ByVal blnPatSched As Boolean, _
                           ByVal lngDuration As Long, ByVal lngPlanID As Long, _
                           ByVal strNotes As String, ByVal strAddedBy As String) As Integer
        '--------------------------------------------------------------------
        'Date: 10/25/2000
        'Author: Morgan Travis
        'Description:  Inserts a row into the tblProviderFee table utilizing
        '              a stored procedure.
        'Parameters: lngProviderID - Provider ID number to insert into table
        '            strCPTCode - The ten digit CPT Code that will be inserted into the table.
        '            dblFee - The monetary amount that will be inserted into the table.
        '
        '--------------------------------------------------------------------

        Dim objProvFee As New BenefitsDB.CProvFeeDB
        Dim strErrMsg As String


        If VerifyData(lngProviderID, strCPTCode, curFee, curContractAmount, strErrMsg, lngPlanID, 0) Then
            objProvFee.Insert(lngProviderID, strCPTCode, strCPTModifier, _
                            strCPTDescription, curFee, curContractAmount, blnPatSched, lngDuration, _
                            lngPlanID, strNotes, strAddedBy)
        Else
            Exit Function
        End If
        objProvFee = Nothing


    End Function

    Public Function Update(ByVal lngProviderFeeID As Long, ByVal lngProviderID As Long, _
                           ByVal strCPTCode As String, ByVal strCPTModifier As String, _
                           ByVal strCPTDescription As String, ByVal curFee As Decimal, _
                           ByVal curContractAmount As Decimal, ByVal blnPatSched As Boolean, _
                           ByVal lngDuration As Long, ByVal lngPlanID As Long, _
                           ByVal strNotes As String, ByVal strUserName As String) As Integer
        '--------------------------------------------------------------------
        'Date: 10/25/2000
        'Author: Morgan Travis
        'Description:  Updates a row in the tblProviderFee table utilizing
        '              a stored procedure.
        'Parameters: lngProviderFeeID - Provider Fee ID number to update table
        '            lngProviderID - ID of the Provider whose fee is being updated
        '            strCPTCode - CPT Code
        '            curFee - The monetary amount that will be inserted into the table.
        '            strUserName - User name identifying user making the change
        '--------------------------------------------------------------------

        Dim objProvFee As New BenefitsDB.CProvFeeDB
        Dim strErrMsg As String


        If curFee < 0 Then
            strErrMsg = "Fees must be positive amounts."
            Exit Function
        End If

        If Exists(lngProviderID, strCPTCode, lngPlanID, lngProviderFeeID) Then
            strErrMsg = "Update would produce duplicate fee."
            Exit Function
        End If

        objProvFee.Update(lngProviderFeeID, lngProviderID, strCPTCode, strCPTModifier, strCPTDescription, curFee, curContractAmount, _
                    blnPatSched, lngDuration, lngPlanID, strNotes, strUserName)

        objProvFee = Nothing



    End Function

    Public Function Exists(ByVal lngProviderID As Long, ByVal strCPTCode As String, ByVal lngPlanID As Long, Optional ByVal lngProviderFeeID As Long = 0) As Boolean

        '--------------------------------------------------------------------
        'Date: 10/25/2000
        'Author: Morgan Travis
        'Description:  Inserts a row into the tblProviderFee table utilizing
        '              a stored procedure.
        'Parameters: lngProviderID - Provider ID number to check for existing entries in table
        '            strCPTCode - CPT Code
        '--------------------------------------------------------------------
        Dim objProvFee As New BenefitsDB.CProvFeeDB
        Dim blnExists As Boolean


        blnExists = objProvFee.Exists(lngProviderID, strCPTCode, lngPlanID, lngProviderFeeID)
        objProvFee = Nothing
        Exists = blnExists


    End Function

    Public Function FetchByProvider(ByVal lngProviderID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/25/2000                                                   '
        'Author: Morgan Travis                                              '
        'Description:  Retrieves records from the tblProviderFee table.     '
        'Parameters: lngProviderID - Provider ID number to fetch from table '
        '
        'Returns: Recordset of requested providers
        '-------------------------------------------------------------------'

        Dim objProvFee As New BenefitsDB.CProvFeeDB
        Dim rstSQL As New ADODB.Recordset


        rstSQL = objProvFee.FetchByProvider(lngProviderID)

        FetchByProvider = rstSQL
        objProvFee = Nothing

        'Signal successful completion



    End Function
    Public Function FetchByPlan(ByVal lngProviderID As Long, ByVal lngPlanID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/25/2000                                                   '
        'Author: Morgan Travis                                              '
        'Description:  Retrieves records from the tblProviderFee table.     '
        'Parameters: lngProviderID - Provider ID number to fetch from table '
        '
        'Returns: Recordset of requested providers
        '-------------------------------------------------------------------'

        Dim objProvFee As New BenefitsDB.CProvFeeDB
        Dim rstSQL As New ADODB.Recordset

        rstSQL = objProvFee.FetchByPlan(lngProviderID, lngPlanID)

        FetchByPlan = rstSQL
        objProvFee = Nothing


    End Function
    Public Function FetchByProviderCPTCode(ByVal lngProviderID As Long, ByVal strCPTCode As String, ByVal lngPlanID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/25/2000                                                   '
        'Author: Morgan Travis                                              '
        'Description:  Retrieves records from the tblProviderFee table.     '
        'Parameters: lngProviderID - Provider ID number to fetch from table '
        '            strCPTCode - CPT Code to fetch from table
        'Returns: RecordSet with requested fee
        '-------------------------------------------------------------------'

        Dim objProvFee As New BenefitsDB.CProvFeeDB
        Dim rstSQL As New ADODB.Recordset



        rstSQL = objProvFee.FetchByProviderCPTCode(lngProviderID, strCPTCode, lngPlanID)

        FetchByProviderCPTCode = rstSQL
        objProvFee = Nothing



    End Function

    Private Function VerifyData(ByVal lngProviderID As Long, ByVal strCPTCode As String, _
                                ByVal curFee As Decimal, ByVal curContractAmount As Decimal, _
                                ByRef strErrMsg As String, _
                                ByVal lngPlanID As Long, ByVal lngProviderFeeID As Long) As Boolean

        Dim objCPT As New ListBZ.CCPTCodeBz

        VerifyData = False
        If lngProviderFeeID = 0 Then

            If lngProviderID < 1 Then
                strErrMsg = "Provider ID Not Valid"
                Exit Function
            End If

            If curFee < 0 Then
                strErrMsg = "Fee Amount Not Valid"
                Exit Function
            End If

            If curContractAmount < 0 Then
                strErrMsg = "Contract Amount Not Valid"
                Exit Function
            End If


            If Not objCPT.Exists(strCPTCode) Then
                strErrMsg = "Invalid CPT Code"
                Exit Function
            End If

            If Exists(lngProviderID, strCPTCode, lngPlanID, 0) Then
                strErrMsg = "Provider/CPT Code combination already exists"
                Exit Function
            End If

            VerifyData = True
        Else
            If curFee < 0 Then
                strErrMsg = "Fee Amount Not Valid"
                Exit Function
            End If

            If curContractAmount < 0 Then
                strErrMsg = "Contract Amount Not Valid"
                Exit Function
            End If

            VerifyData = True
        End If
    End Function

    Public Function FetchByID(ByVal lngProviderFeeID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 05/08/2001
        'Author: Dave Richkun
        'Description:  Retrieves a record from the tblProviderFee table.
        'Parameters: lngProviderFeeID - Value of the Primary Key identifiying the
        '               row that will be retrieved.
        'Returns: ADO Recordset
        '-------------------------------------------------------------------'
        ' R001 - Created
        '-------------------------------------------------------------------'

        Dim objProvFee As New BenefitsDB.CProvFeeDB
        Dim rstSQL As New ADODB.Recordset


        rstSQL = objProvFee.FetchByID(lngProviderFeeID)

        FetchByID = rstSQL
        objProvFee = Nothing

        'Signal successful completion

    End Function

    Public Sub Delete(ByVal lngProviderFeeID As Long)
        '--------------------------------------------------------------------
        'Date: 05/08/2001
        'Author: Dave Richkun
        'Description:  Deletes a row from the tblProviderFee table utilizing
        '              a stored procedure.
        'Parameters: lngProviderFeeID - ID of row to delete
        '--------------------------------------------------------------------
        '  R003 - Created
        '--------------------------------------------------------------------

        Dim objProvFee As New BenefitsDB.CProvFeeDB
        Dim strErrMsg As String


        objProvFee.Delete(lngProviderFeeID)
        objProvFee = Nothing



    End Sub

End Class
