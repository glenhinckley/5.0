
'--------------------------------------------------------------------
'Class Name: CFormMailBz
'Date: 09/20/2008
'Author: Rick Duane C Orth
'Denis J Finkel
'Description:  MTS business object designed to   methods
'              associated with the FormMail class.
'--------------------------------------------------------------------
'
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

Public Class CFormMailBZ

    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CFormMailBz"
    Private Const FORMMAIL_TABLE As String = "tblFormMail"

    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False, Optional ByVal strWhere As String = "", Optional ByVal strOrderBy As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/20/2008                                                   '
        'Author: Rick Duane C Orth                                          '
        'Description:  Retrieves records from the tblFormMail table.
        'Parameters:  blnIncludeDisabled - Optional parameter that identifies if
        '               records flagged as 'Disabled' or 'De-activated' are to be
        '               included in the record set. The default value is False.
        '             strWhere - Optional 'Where' clause of the SQL statement by
        '               which records will be filtered.  If the parameter is not
        '               supplied, all records will be retrieved.
        '             strOrderBy - Optional 'Order By' clause in which retrieved
        '               records will be sorted.  The default is set to the
        '               'fldMaritalStatus' column
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objFormMail As New FormMailDB.CFormMailDB
        Dim rstSQL As New ADODB.Recordset




        rstSQL = objFormMail.Fetch(blnIncludeDisabled, strWhere, strOrderBy)
        Fetch = rstSQL

        objFormMail = Nothing




    End Function

    Public Function Insert(ByVal strRecipient As String, ByVal strBrowser As String, _
                           ByVal strFullname As String, ByVal strEmail As String, _
                           ByVal strBody As String) As Long
        '--------------------------------------------------------------------
        'Date: 09/20/2008                                                   '
        'Author: Rick Duane C Orth                                          '
        'Description:  Inserts a row into the tblFormMail table utilizing   '
        '              a stored procedure.                                  '
        'Parameters: strRecipient - Recipient                               '
        '            strBrowser - Subject                                   '
        '            strFullname - Full name                                '
        '            strEmail - "" E-mail Address                           '
        '            strBody - Body                                         '
        'Returns: ID (Primary Key) of the row inserted                      '
        '--------------------------------------------------------------------
        ' Revision History:                                                 '
        '   R001 - Added varTaxIDs and varPracticeNums parameters           '
        '--------------------------------------------------------------------

        Dim objFormMail As New FormMailDB.CFormMailDB
        Dim lngID As Long
        Dim lngInsCompanyID As Long
        Dim strErrMessage As String


        'Verify data before proceeding.
        If Not VerifyData(0, strRecipient, strBrowser, strFullname, strEmail, strBody, strErrMessage) Then
            Exit Function
        End If

        'Insert a row into tblFormMail

        lngID = objFormMail.Insert(strRecipient, strBrowser, strFullname, strEmail, strBody)
        objFormMail = Nothing


        Insert = lngID

    End Function

    Public Sub Update(ByVal lngID As Long, ByVal strRecipient As String, ByVal strBrowser As String, _
                           ByVal strFullname As String, ByVal strEmail As String, _
                           ByVal strBody As String, ByVal strUpdatedBy As String)
        '--------------------------------------------------------------------
        'Date: 09/20/2008                                                   '
        'Author: Rick Duane C Orth                                          '
        'Description:  Updates a row into the tblFormMail table.            '
        'Parameters: lngID - ID of the row in the table whose value will be '
        '            strRecipient - Recipient                               '
        '            strBrowser - Subject                                   '
        '            strFullname - Full name                                '
        '            strEmail - "" E-mail Address                           '
        '            strBody - Body                                         '
        '            strUpdatedBy - Login ID of user adding provider        '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        ' R003 - Added varTaxIDs and varPracticeNums parameters             '
        '--------------------------------------------------------------------
        Dim objFormMail As New FormMailDB.CFormMailDB
        Dim strErrMessage As String


        'Verify data before proceeding.
        If Not VerifyData(lngID, strRecipient, strBrowser, strFullname, strEmail, strBody, strErrMessage) Then
            Exit Sub
        End If

        objFormMail.Update(lngID, strRecipient, strBrowser, strFullname, strEmail, strBody, strUpdatedBy)
        objFormMail = Nothing




    End Sub

    Public Function Exists(ByVal strFullname As String, ByVal strEmail As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 09/20/2008                                                   '
        'Author: Rick Duane C Orth                                          '
        'Description:  Determines if a provider record identical to the
        '              name parameters is already associated to the clinic
        '              whose ID matches lngClinic.
        'Parameters: strFullname - Full name                                '
        '            strEmail - "" E-mail Address                           '
        'Returns: True if the association exists, false otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim objFormMail As New FormMailDB.CFormMailDB
        Dim blnExists As Boolean



        blnExists = objFormMail.Exists(strFullname, strEmail)

        Exists = blnExists



        objFormMail = Nothing


    End Function

    Private Function VerifyData(ByVal lngID As Long, ByVal strRecipient As String, ByVal strBrowser As String, _
                           ByVal strFullname As String, ByVal strEmail As String, _
                           ByVal strBody As String, ByRef strErrMessage As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 09/20/2008                                                   '
        'Author: Rick Duane C Orth                                          '
        'Description:  Verifies all required data has been provided by the user.
        'Parameters:  The values to be checked.
        'Returns: Boolean value identifying if all criteria has been satisfied.
        '--------------------------------------------------------------------
        Dim s As String = strEmail




        If Trim(strRecipient) = "" Then
            strErrMessage = "Recipient is required."
            VerifyData = False
            Exit Function
        End If

        If Trim(strBrowser) = "" Then
            strErrMessage = "Subject is required."
            VerifyData = False
            Exit Function
        End If

        If Trim(strFullname) = "" Then
            strErrMessage = "Full Name is required."
            VerifyData = False
            Exit Function
        End If

        If IsDBNull(strEmail) Then
            strErrMessage = "Email Address is required."
            VerifyData = False
            Exit Function
        End If

        'If we get here, all is well...
        VerifyData = True

    End Function

    Public Function Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Long, ByVal strDeletedBy As String) As Integer
        '--------------------------------------------------------------------
        'Date: 09/20/2008                                                   '
        'Author: Rick Duane C Orth                                          '
        'Description:  Flags a row in the tblFormMail table marking the row as
        '              deleted or undeleted.
        'Parameters: blnDeleted - Boolean value identifying if the record is to
        '               be deleted (True) or undeleted (False).
        '            lngID - ID of the row in the table whose value will be
        '               updated.
        '            strDeletedBy - Login name of the user performing the action.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------

        Dim objFormMail As New FormMailDB.CFormMailDB


        Deleted = 0 ' Assume no errors



        objFormMail.Deleted(blnDeleted, lngID, strDeletedBy)



        objFormMail = Nothing


    End Function

End Class

