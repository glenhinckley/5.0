
'--------------------------------------------------------------------
'Class Name: CEMRTemplateBZ
'Date: 09/05/2000
'Author: David Nichol and Joshua Lockwood
'Description:  MTS business object designed to   methods
'              associated with the CEMRTemplateDB class.
'--------------------------------------------------------------------
' Revision History:
'   R001 : Joshua Lockwood, 11SEP00 : Updated code to handle type TEXT
'          for TemplateText being passed.
'   R002 : Joshua Lockwood, 27SEP00 : Made modifications due to significant
'          changes in Template architecture.
'   R003 : Joshua Lockwood, 2OCT00 : Added verification of data and a new
'       method that checks if a name is already present in the DB.
'   R004 : Joshua Lockwood, 02Nov00 : Added param to Item inserts
'       for tblTemplateItem(fldItemNumber), added fetchNext and
'       fetchPrev methods, updateItemNumber method.  'Question' changed
'       to 'Item' everywhere.
'   R005 : David Nichol, 16Jan01 : changed parameters for UpdateItemNumber,
'       requiring lngTemplateID, lngOldItemNumber, and lngNewItemNumber.
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
' Lydia Orth 

Public Class CEMRTemplateBZ
    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CEMRTemplateBz"
    Private Const EMRTEMPLATE_TABLE As String = "tblProviderTemplate"

    Public Function FetchByID(ByVal lngTemplateID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/05/2000
        'Author: Dave Nichol
        'Description:  Retrieves records from the tblProviderTemplate table.
        'Parameters: lngTemplateID - ID number of the record to retrieve, which
        '               is primary key of table.
        '            lngUserID - UserID of user  ing method.
        '            blnIncludeDisabled - boolean value -- whether to include
        '            disabled rows (true) or not (false).
        'Returns: ADODB.Recordset -- should only return one record
        '--------------------------------------------------------------------

        Dim objEMRTemplate As New EMRDB.CEMRTemplateDB
        Dim rstSQL As New ADODB.Recordset


        rstSQL = objEMRTemplate.FetchByID(lngTemplateID)
        FetchByID = rstSQL


        objEMRTemplate = Nothing

    End Function
    Public Function FetchByItemID(ByVal lngItemID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 11/06/2000
        'Author: Joshua Lockwood
        'Description:  Retrieves records from the tblTemplateItem table.
        'Parameters: lngItemID - ID number of the record to retrieve, which
        '               is primary key of table.
        'Returns: ADODB.Recordset -- should only return one record
        '--------------------------------------------------------------------

        ' Declares database object
        Dim objEMRTemplate As New EMRDB.CEMRTemplateDB
        ' Declares recordset for result returned
        Dim rstSQL As New ADODB.Recordset



        ' Instantiates recordset for result returned
        rstSQL = objEMRTemplate.FetchByItemID(lngItemID)

        ' Return value set for function
        FetchByItemID = rstSQL


        ' Set DB object to NULL
        objEMRTemplate = Nothing

    End Function

    Public Function FetchByItemNumber(ByVal lngTemplateID As Long, ByVal lngItemNumber As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 11/08/2000
        'Author: David Nichol
        'Description:  Retrieves records from tblTemplateItem by fldTemplateID
        '       and fldItemNumber.  This should, provided all inserts were
        '       correctly validated, only return a single record!
        'Parameters: lngTemplateID - ID of the template associated with the item
        '               record
        '            lngItemNumber -  number of the record to retrieve, which
        '               is primary key of table.
        'Returns: ADODB.Recordset -- should only return one record
        '--------------------------------------------------------------------

        ' Declares database object
        Dim objEMRTemplate As New EMRDB.CEMRTemplateDB
        ' Declares recordset for result returned
        Dim rstSQL As New ADODB.Recordset



        ' Instantiates recordset for result returned
        rstSQL = objEMRTemplate.FetchByItemNumber(lngTemplateID, lngItemNumber)

        ' Return value set for function
        FetchByItemNumber = rstSQL

        objEMRTemplate = Nothing

    End Function

    Public Function FetchByUser(ByVal lngUserID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 09/05/2000
        'Author: Dave Nichol
        'Description:  Retrieves records from the tblProviderTemplate table.
        'Parameters: lngUserID - UserID of user  ing method.
        '            blnIncludeDisabled - boolean value -- whether to include
        '            disabled rows (true) or not (false).
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------

        Dim objEMRTemplate As New EMRDB.CEMRTemplateDB
        Dim rstSQL As New ADODB.Recordset


        rstSQL = objEMRTemplate.FetchByUser(lngUserID, blnIncludeDisabled)

        FetchByUser = rstSQL

        objEMRTemplate = Nothing

    End Function
    Public Function FetchNextItem(ByVal lngTemplateID As Long, _
                                  ByVal lngCurrentItem As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 11/02/2000
        'Author: Joshua Lockwood
        'Description: Retrieves records from the tblTemplateItem table.   s
        '               stored procedure uspSelNextItem.  New with revision R004
        'Parameters: lngTemplateID - ID of template associated with the Item.
        '            lngCurrentItem - Sequence number tblTemplateItem(fldItemNumber)
        '               associated with current Item. <Next from *>
        'Returns: INTEGER (ItemID)
        '--------------------------------------------------------------------

        Dim objEMRTemplate As New EMRDB.CEMRTemplateDB
        Dim rstSQL As New ADODB.Recordset


        rstSQL = objEMRTemplate.FetchNextItem(lngTemplateID, lngCurrentItem)

        FetchNextItem = rstSQL


        objEMRTemplate = Nothing

    End Function
    Public Function FetchPrevItem(ByVal lngTemplateID As Long, _
                                  ByVal lngCurrentItem As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 11/02/2000
        'Author: Joshua Lockwood
        'Description: Retrieves records from the tblTemplateItem table.   s
        '               stored procedure uspSelNextItem.  New with revision R004
        'Parameters: lngTemplateID - ID of template associated with the Item.
        '            lngCurrentItem - Sequence number tblTemplateItem(fldItemNumber)
        '               associated with current Item. <Prev from *>
        'Returns: INTEGER (ItemID)
        '--------------------------------------------------------------------

        Dim objEMRTemplate As New EMRDB.CEMRTemplateDB
        Dim rstSQL As New ADODB.Recordset



        rstSQL = objEMRTemplate.FetchPrevItem(lngTemplateID, lngCurrentItem)

        FetchPrevItem = rstSQL


        objEMRTemplate = Nothing

    End Function
    Public Function InsertTemplate(ByVal lngUserID As Long, ByVal strTemplateName As String, _
                ByVal strTemplateDesc As String, ByVal strUserName As String) As Long
        '--------------------------------------------------------------------
        'Date: 09/05/2000
        'Author: David Nichol and Joshua Lockwood
        'Description:  Inserts a row into the tblProviderTemplate table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------
        ' R001 - In param list, strTemplateText changed to type Object

        ' Declare DB object (connection)
        Dim objEMRTemplate As New EMRDB.CEMRTemplateDB
        ' Declare buffer for err msg
        Dim strErrMessage As String
        ' Set tag for err trap code

        '+=+=
        If Not VerifyData(strErrMessage, False, lngUserID, , strTemplateName, strTemplateDesc, _
                     strUserName) Then Exit Function

        If NameExists(lngUserID, strTemplateName) Then
            strErrMessage = "Template name already exists. Please use another name."
            Exit Function
        End If


        ' Fire Insert method
        InsertTemplate = objEMRTemplate.InsertTemplate(lngUserID, strTemplateName, strTemplateDesc, strUserName)



        ' Set DB obj to null
        objEMRTemplate = Nothing

    End Function
    Public Function InsertTemplateItem(ByVal lngTemplateID As Long, _
                    ByVal strItemText As String, ByVal strPossibleAnswers As String, _
                    ByVal strDisplayText As String, ByVal strType As String, _
                    ByVal strHTMLName As String, ByVal lngItemNumber As Long) As Long
        '--------------------------------------------------------------------
        'Date: 09/27/2000
        'Author: Joshua Lockwood
        'Description:  Inserts a row into the tblTemplateItem table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------

        ' Declare DB object (connection)
        Dim objEMRTemplate As New EMRDB.CEMRTemplateDB
        ' Declare buffer for err msg
        Dim strErrMessage As String
        ' Declare recordset for result returned
        Dim rst As New ADODB.Recordset




        'Validate data being passed
        If Not VerifyLong(lngTemplateID) Then Exit Function
        If Not VerifyLong(lngItemNumber) Then Exit Function
        'Validate strings, depending on type
        If Left(strType, 1) = "H" Then
            If Not VerifyString(strDisplayText) Then Exit Function
            If Not VerifyString(strHTMLName) Then Exit Function
        ElseIf Left(strType, 1) = "T" Then
            If Not VerifyString(strDisplayText) Then Exit Function
            If Not VerifyString(strItemText) Then Exit Function
            If Not VerifyString(strHTMLName) Then Exit Function
        ElseIf Left(strType, 1) = "S" Then
            If Not VerifyString(strDisplayText) Then Exit Function
            If Not VerifyString(strItemText) Then Exit Function
            If Not VerifyString(strPossibleAnswers) Then Exit Function
            If Not VerifyString(strHTMLName) Then Exit Function
        ElseIf Left(strType, 1) = "M" Then
            If Not VerifyString(strDisplayText) Then Exit Function
            If Not VerifyString(strItemText) Then Exit Function
            If Not VerifyString(strPossibleAnswers) Then Exit Function
            If Not VerifyString(strHTMLName) Then Exit Function
        Else
            strErrMessage = "Invalid data passed to CEMRTemplateBZ.InsertTemplateItem()."
            Exit Function
        End If

        'Prevent insertion of item with templateID-itemNumber combo that exists already
        rst = FetchByItemNumber(lngTemplateID, lngItemNumber)
        If rst.RecordCount <> 0 Then
            strErrMessage = "Template item already exists with that item number."
            Exit Function
        End If



        ' Fire Insert method
        InsertTemplateItem = objEMRTemplate.InsertTemplateItem(lngTemplateID, strItemText, _
                    strPossibleAnswers, strDisplayText, strType, strHTMLName, lngItemNumber)



        ' Set DB obj to null
        objEMRTemplate = Nothing

        ' set record set to nothing
        rst = Nothing


    End Function

    Public Sub UpdateTemplate(ByVal lngTemplateID As Long, ByVal strTemplateName As String, _
                        ByVal strTemplateDesc As String)
        '--------------------------------------------------------------------
        'Date: 09/05/2000
        'Author: David Nichol and Joshua Lockwood
        'Description:  Updates a row in the tblProviderTemplate table utilizing
        '              a stored procedure.
        '   Note: TEXT vars have 8192 character ceiling
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Null
        '--------------------------------------------------------------------

        ' Declare database object
        Dim objEMRTemplate As New EMRDB.CEMRTemplateDB

        ' Declare error message string
        Dim strErrMessage As String



        ' Validate data...leave username option blank  '+=+=
        'If Not VerifyData(strErrMessage, False, lngUserID, lngTemplateID, strTemplateName, _
        If (strTemplateDesc) Then Exit Sub


        ' Run database object
        objEMRTemplate.UpdateTemplate(lngTemplateID, strTemplateName, strTemplateDesc)

        objEMRTemplate = Nothing

    End Sub

    Public Sub UpdateTemplateItem(ByVal lngTemplateID As Long, _
            ByVal lngItemID As Long, ByVal strItemText As String, _
            ByVal strPossibleAnswers As String, ByVal strDisplayText As String, _
            ByVal strType As String, ByVal strHTMLName As String)
        '--------------------------------------------------------------------
        'Date: 09/27/2000
        'Author: Joshua Lockwood
        'Description:  Updates a row into the tblTemplateItem table utilizing
        '              a stored procedure.  New with revision R004
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Nothing
        '--------------------------------------------------------------------

        ' Declare database object
        Dim objEMRTemplate As New EMRDB.CEMRTemplateDB

        ' Declare error message string
        Dim strErrMessage As String


        '' Validate data...leave username option blank '+=+=
        ''If Not VerifyData(strErrMessage, True, , lngItemID, , , , _
        '    strItemText, strPossibleAnswers, strDisplayText, strType, _
        '    strHTMLName) Then Exit Function


        ' Run database object
        objEMRTemplate.UpdateTemplateItem(lngTemplateID, lngItemID, _
            strItemText, strPossibleAnswers, strDisplayText, strType, strHTMLName)

        objEMRTemplate = Nothing

    End Sub
    Public Sub UpdateItemNumber(ByVal lngTemplateID As Long, ByVal lngOldItemNumber As Long, _
                                ByVal lngNewItemNumber As Long)
        '--------------------------------------------------------------------
        'Date: 11/02/2000
        'Author: Joshua Lockwood
        'Description: Updates value of ItemNumber in tblTemplateItem for
        '              the selected ItemID.
        '             Also see R005
        'Parameters: Each parameter identifies the column value that will be
        '              updated.
        '
        'Returns: Null
        '--------------------------------------------------------------------

        ' Declare database object
        Dim objEMRTemplate As New EMRDB.CEMRTemplateDB

        ' Declare error message string
        Dim strErrMessage As String


        ' Run database object
        objEMRTemplate.UpdateItemNumber(lngTemplateID, lngOldItemNumber, lngNewItemNumber)

        objEMRTemplate = Nothing

    End Sub
    Public Sub CommitTemplate(ByVal lngTemplateID As Long)
        '--------------------------------------------------------------------
        'Date: 09/27/2000
        'Author: Joshua Lockwood
        'Description: Commits Items associated with template ID.
        '   New with revision R004
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: Nothing
        '--------------------------------------------------------------------

        ' Declare database object
        Dim objEMRTemplate As New EMRDB.CEMRTemplateDB

        ' Declare error message string
        Dim strErrMessage As String


        ' Run database object
        objEMRTemplate.CommitTemplate(lngTemplateID)


        objEMRTemplate = Nothing

    End Sub
    Public Sub CopyTemplate(ByVal lngProviderID As Long, ByVal lngTemplateID As Long)
        ' Declare database object
        Dim objEMRTemplate As New EMRDB.CEMRTemplateDB

        ' Declare error message string
        Dim strErrMessage As String




        ' Run database object
        objEMRTemplate.CopyTemplate(lngProviderID, lngTemplateID)


        objEMRTemplate = Nothing

    End Sub

    Public Sub PurgeTemplate(ByVal lngTemplateID As Long)
        '--------------------------------------------------------------------
        'Date: 09/27/2000
        'Author: Joshua Lockwood
        'Description:  Updates a row into the tblTemplateItem table utilizing
        '              a stored procedure.
        'Parameters: Each parameter identifies the column value that will be
        '              inserted.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------

        ' Declare database object
        Dim objEMRTemplate As New EMRDB.CEMRTemplateDB

        ' Declare error message string
        Dim strErrMessage As String


        ' Run database object
        objEMRTemplate.PurgeTemplate(lngTemplateID)

        objEMRTemplate = Nothing

    End Sub
    Public Sub Deleted(ByVal lngTemplateID As Long, Optional ByVal blnDisabled As Boolean = False)
        '--------------------------------------------------------------------
        'Date: 09/05/2000
        'Author: David Nichol and Joshua Lockwood
        'Description:  Flags a row in the tblProviderTemplate table marking the row as
        '              deleted or undeleted.
        'Parameters: lngTemplateID - ID of the row in the table whose value will be
        '               updated.
        '            blnIncludeDisabled - boolean value -- whether to include
        '            disabled rows (true) or not (false).
        'Returns: Null
        '--------------------------------------------------------------------
        ' Declare DB obj
        Dim objEMRTemplate As New EMRDB.CEMRTemplateDB


        ' Fire delete method
        objEMRTemplate.Deleted(lngTemplateID, blnDisabled)

        objEMRTemplate = Nothing

    End Sub
    Public Sub DeleteItem(ByVal lngItemID As Long)
        '--------------------------------------------------------------------
        'Date: 09/28/2000
        'Author: Joshua Lockwood
        'Description:  Deletes row from tblTemplateItem.   s stored procedure
        '   uspDelTemplateItem
        'Parameters: lngTemplateID - ID of the row in the table whose value will be
        '               deleted.
        'Returns: Null
        '--------------------------------------------------------------------

        ' Declare DB obj
        Dim objEMRTemplate As New EMRDB.CEMRTemplateDB


        ' Fire delete method
        objEMRTemplate.DeleteItem(lngItemID)

        objEMRTemplate = Nothing

    End Sub

    Public Function NameExists(ByVal lngUserID As Long, ByVal strTemplateName As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 10/2/2000
        'Author: Joshua Lockwood
        'Description:  Verifies existence of name in DB.  Added with R003.
        'Parameters: lngUserID - ID for Fetch by User
        'Returns: Boolean
        '--------------------------------------------------------------------
        Dim objEMRTemplate As New EMRDB.CEMRTemplateDB
        Dim rstSQL As New ADODB.Recordset
        Dim intCtr As Integer



        rstSQL = FetchByUser(lngUserID, True)

        'Make search algorithm more (much more) efficient in future
        '   recordset is ordered by templateName, binary should do
        For intCtr = 1 To rstSQL.RecordCount
            If (rstSQL.Fields("fldTemplateName").Value = strTemplateName) Then
                NameExists = True
                Exit Function
            End If

            rstSQL.MoveNext()
        Next intCtr


        rstSQL = Nothing

        NameExists = False


    End Function


    Private Function VerifyData(ByRef strErrMessage As String, ByRef blnIsTemplateItem As Boolean, _
        Optional ByVal lngUserID As Long = 0, Optional ByVal lngTemplateID As Long = 0, _
        Optional ByVal strTemplateName As String = "", Optional ByVal strTemplateDesc As String = "", _
        Optional ByVal strUserName As String = "", _
        Optional ByVal strItemText As String = "", Optional ByVal strPossibleAnswers As String = "", _
        Optional ByVal strDisplayText As String = "", Optional ByVal strType As String = "", _
        Optional ByVal strHTMLName As String = "") As Boolean
        '--------------------------------------------------------------------
        'Date: 09/05/2000
        'Author: David Nichol and Joshua Lockwood
        'Description:  Verifies all required data has been provided by the user.
        '              Last three params are optional to take into account the
        '              different needs for update and insert.  Boolean value doesn't
        '              be validated because all bool values are acceptable.  Function
        '              header changed R003.  Checks only for required data.
        'Parameters:  The values to be checked.
        ' strTemplateName strTemplateDesc strTemplateText strErrMessage lngUserID lngTemplateID strUserName
        'Returns: Boolean value identifying if all criteria has been satisfied.
        '--------------------------------------------------------------------
        '==============================
        ' For all
        '==============================
        ' Verify that the UserID is valid; must be > 0 (1st index==1)
        ' 0 will be passed if var is not present
        If lngUserID < 0 Then
            strErrMessage = "User ID number provided is not valid."
            VerifyData = False
            Exit Function
        End If

        ' Verify that ID number is valid; must be > 0 (1st index==1)
        ' 0 will be passed if var is not present
        If lngTemplateID < 0 Then
            If blnIsTemplateItem Then
                strErrMessage = "Item ID number provided is not valid."
            Else
                strErrMessage = "Template ID number provided is not valid."
            End If
            VerifyData = False
            Exit Function
        End If

        '==============================
        ' For Not TemplateItems
        '==============================
        ' Verify that the Template name is valid; must have something in it
        If Not blnIsTemplateItem And Trim(strTemplateName) = "" Then
            strErrMessage = "Template name provided is not valid."
            VerifyData = False
            Exit Function
        End If

        ' Check to see if username provided is not null--if it's provided
        ' First check to see if username was passed as a parameter
        If Not blnIsTemplateItem And Trim(strUserName) = "" Then
            strErrMessage = "No valid user name was passed."
            VerifyData = False
            Exit Function
        End If

        '==============================
        ' For TemplateItems
        '==============================
        If blnIsTemplateItem And Trim(strItemText) = "" Then
            strErrMessage = "No display text was entered."
            VerifyData = False
            Exit Function
        End If

        'If we get here, all is well...
        VerifyData = True

    End Function

    Private Function VerifyString(ByVal strToVerify As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 11/08/2000
        'Author: David Nichol
        'Description:  Verifies that the string passed is valid and has length > 0
        'Parameters:  The string to be checked.
        'Returns: Boolean value identifying if string is valid and non-empty.
        '--------------------------------------------------------------------
        If Len(strToVerify) > 0 Then
            VerifyString = True
        Else
            VerifyString = False
        End If
    End Function

    Private Function VerifyLong(ByVal lngToVerify As Long) As Boolean
        '--------------------------------------------------------------------
        'Date: 11/08/2000
        'Author: David Nichol
        'Description:  Verifies that the long integer value passed is valid
        '       and is greater than 0.  Useful for ID values that cannot be 0.
        'Parameters:  The value to be checked.
        'Returns: Boolean value identifying if value is valid
        '-------------------------
        If lngToVerify > 0 Then
            VerifyLong = True
        Else
            VerifyLong = False
        End If
    End Function

End Class
