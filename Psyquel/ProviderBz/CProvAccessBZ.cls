
'--------------------------------------------------------------------
'Class Name: CProvAccessBZ
'Date: 12/20/2000
'Author: Joshua Lockwood
'Denis J Finkel
'Description:  Business object designed to   methods
'               associated with the CProvAccessDB class.
'--------------------------------------------------------------------
'Revision History:
'  R001 - DNichol 03/22/2001 : Modified Insert() to check if the association
'         being inserted already exists, and, if so, to simply re-enable it.
'  R002 - DNichol 03/23/2001 : Added FetchProviderAccessListByID()
'  R003 - DNichol 03/30/2001 : Fixed error in Update()
'  R004 - Richkun 04/11/2001 : Added FetchWebUserListByName() method
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

Public Class CProvAccessBZ
    Private _ConnectionString As String = String.Empty
    Private Const CLASS_NAME As String = "CProvAccessDB"
    Private Const TABLE_NAME As String = "tblProviderAccess"

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property




    Public Function FetchProviderAccessList(ByVal lngUserID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 12/20/2000
        'Author: Joshua Lockwood
        'Description: Retrieves records from the tblProviderAccess table.
        '
        'Parameters: userID - owner ID.
        '            includeDisabled - also include disabled records on true.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------

        ' Declares database onject
        Dim objProviderAccess As New ProviderDB.CProvAccessDB

        objProviderAccess.ConnectionString = _ConnectionString


        ' Declares recordset for result returned
        Dim rstSQL As New ADODB.Recordset

        ' Instantiates recordset for result returned
        rstSQL = objProviderAccess.FetchProviderAccessList(lngUserID, blnIncludeDisabled)

        ' Return value set for function
        FetchProviderAccessList = rstSQL


        ' Set DB object to NULL
        objProviderAccess = Nothing

    End Function

    Public Function FetchProviderAccessListByID(ByVal lngProvAccessID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 03/23/2001
        'Author: David Nichol
        'Description: Retrieves a record from the tblProviderAccess table.
        '
        'Parameters: lngProvAccessID - ID of record from which to fetch prov & colleague.
        '            includeDisabled - also include disabled records on true.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------

        ' Declares database onject
        Dim objProviderAccess As New ProviderDB.CProvAccessDB
        ' Declares recordset for result returned
        Dim rstSQL As New ADODB.Recordset

        objProviderAccess.ConnectionString = _ConnectionString
        ' Instantiates recordset for result returned
        rstSQL = objProviderAccess.FetchProviderAccessListByID(lngProvAccessID, blnIncludeDisabled)

        ' Return value set for function
        FetchProviderAccessListByID = rstSQL

        ' Set DB object to NULL
        objProviderAccess = Nothing

    End Function

    Public Function FetchUserListByName(ByVal strLastName As String, Optional ByVal strFirstName As String = "") As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 01/10/2001
        'Author: Joshua Lockwood
        'Description: Pardon me if I elaborate on functionality.  This method runs
        '   uspSelProviderByName that returns one or more records containing the
        '   name and ID of user whose data is queried for (name passed in).  Data
        '   is retrieved from tblUser.
        '
        'Parameters: strLastName - last name of desired user
        '            strFirstName - first name of desired user
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------

        ' Declares database onject
        Dim objProviderAccess As New ProviderDB.CProvAccessDB
        ' Declares recordset for result returned
        Dim rstSQL As New ADODB.Recordset
        objProviderAccess.ConnectionString = _ConnectionString

        '   fetch method and set recordset returned to rstSQL
        rstSQL = objProviderAccess.FetchUserListByName(strLastName, strFirstName)

        ' Set recordset to retval
        FetchUserListByName = rstSQL


    End Function

    Public Function FetchPatientByOwner(ByVal lngProviderID As Long, Optional ByVal blnIncludeDisabled As Boolean = False) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 12/20/2000
        'Author: Joshua Lockwood
        'Description: Retrieves records from the tblBenefactor table where
        '             benefactor's ownerID matches lngProviderID.
        '
        'Parameters: lngProviderID - owner ID.
        '            blnIncludeDisabled - also include disabled records on true.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------

        ' Declares database onject
        Dim objProviderAccess As New ProviderDB.CProvAccessDB
        ' Declares recordset for result returned
        Dim rstSQL As New ADODB.Recordset

        objProviderAccess.ConnectionString = _ConnectionString

        '   fetch method and set recordset returned to rstSQL
        rstSQL = objProviderAccess.FetchPatientByOwner(lngProviderID, blnIncludeDisabled)

        ' Set recordset to retval
        FetchPatientByOwner = rstSQL

        objProviderAccess = Nothing   ' Delete object

    End Function

    Public Function FetchPatientOwner(ByVal lngPatientID As Long) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 12/20/2000
        'Author: Joshua Lockwood
        'Description: Retrieves records from the tblProviderAccess table.
        '
        'Parameters: lngPatientID - patient ID.
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------

        ' Declares database onject
        Dim objProviderAccess As New ProviderDB.CProvAccessDB
        ' Declares recordset for result returned
        Dim rstSQL As New ADODB.Recordset

        objProviderAccess.ConnectionString = _ConnectionString
        '   fetch method and set recordset returned to rstSQL
        rstSQL = objProviderAccess.FetchPatientOwner(lngPatientID)

        ' Set recordset to retval
        FetchPatientOwner = rstSQL

        objProviderAccess = Nothing   ' Delete object

    End Function

    Public Function Exists(ByVal lngProviderID As Long, ByVal lngColleagueID As Long) As Long
        Dim rstTest As New ADODB.Recordset
        Dim i As Integer


        'Check to see if provider already has a list
        rstTest = FetchProviderAccessList(lngProviderID, True)

        Exists = 0

        If rstTest.RecordCount > 0 Then
            For i = 1 To rstTest.RecordCount
                If (rstTest.Fields("fldColleagueID").Value = lngColleagueID) Then
                    Exists = rstTest.Fields("fldProviderAccessID").Value

                    rstTest = Nothing
                    Exit Function
                End If
                rstTest.MoveNext()
            Next i
        End If

        rstTest = Nothing

    End Function

    Public Function Insert(ByVal strUserName As String, ByVal lngProviderID As Long, ByVal lngColleagueID As Long) As Long
        '--------------------------------------------------------------------
        'Date: 12/20/2000
        'Author: Joshua Lockwood
        'Description: Adds new record to the tblBenefactor table.
        '
        'Parameters: strUserName - for AddedBy.
        '            lngProviderID - ID of the owner granting priviledges to a colleague
        '            lngColleagueID - ID of the colleague being added to access list
        'Returns: Long (ProviderAccessID)
        '--------------------------------------------------------------------
        ' Declare DB object (connection)
        Dim objProviderAccess As New ProviderDB.CProvAccessDB
        ' Declare buffer for err msg
        Dim strErrMessage As String
        ' Declare variable for existing association
        Dim lngExistingRec

        objProviderAccess.ConnectionString = _ConnectionString

        ' Make sure the record does not already exist
        lngExistingRec = Exists(lngProviderID, lngColleagueID)
        If lngExistingRec > 0 Then
            ' Re-enable record
            objProviderAccess.Deleted(strUserName, lngExistingRec, False)
            Insert = lngExistingRec
        Else
            ' Fire Insert method
            Insert = objProviderAccess.Insert(strUserName, lngProviderID, lngColleagueID)
        End If


        ' Set DB obj to null
        objProviderAccess = Nothing
        ' Exit on success

    End Function

    Public Sub Update(ByVal lngUserID As Long, ByVal lngNewOwner As Long, ByVal lngPatientID As Long, ByVal strUpdatedBy As String)
        '--------------------------------------------------------------------
        'Date: 12/20/2000
        'Author: Joshua Lockwood
        'Description: Updates selected records in tblBenefactor table.
        '
        'Parameters: lngUserID - ID of current owner granting the transfer.
        '            lngNewOwner - Owner of the 'soon-to-be' owner.
        '            lngPatientID - ID of the patient which is being transfered.
        'Returns: Nothing
        '--------------------------------------------------------------------
        ' Declare DB object (connection)
        Dim objProviderAccess As New ProviderDB.CProvAccessDB
        ' Declare buffer for err msg
        Dim strErrMessage As String
        'for testing
        Dim objPatProv As New BenefactorBz.CPatientProviderBz
        Dim TestRet As Long



        objProviderAccess.ConnectionString = _ConnectionString
        objPatProv.ConnectionString = _ConnectionString



        TestRet = objPatProv.InsertWeb(lngPatientID, lngNewOwner, lngUserID, strUpdatedBy)

        objPatProv = Nothing
        '---------------------



        ' Run database object
        objProviderAccess.Update(lngUserID, lngNewOwner, lngPatientID)


        objProviderAccess = Nothing

    End Sub

    Public Sub Deleted(ByVal strUserName As String, ByVal lngProviderAccessID As Long, ByVal blnDisabledYN As Boolean)
        '--------------------------------------------------------------------
        'Date: 12/20/2000
        'Author: Joshua Lockwood
        'Description: 'Deletes' selected record in tblProviderAccess table.   s
        '               stored procedure uspDelProviderAccess.
        'Parameters: strUserName - for 'DeletedBy.
        '            lngProviderAccessID - ID of relationship to be deleted.
        '            blnDisabledYN - Toggle disabled flag of relation.
        'Returns: Nothing
        '--------------------------------------------------------------------
        ' Declare DB object (connection)
        Dim objProviderAccess As New ProviderDB.CProvAccessDB
        ' Declare buffer for err msg
        Dim strErrMessage As String
        ' Set tag for err trap code


        objProviderAccess.ConnectionString = _ConnectionString

        ' Fire delete method
        objProviderAccess.Deleted(strUserName, lngProviderAccessID, blnDisabledYN)

        objProviderAccess = Nothing

    End Sub

    Public Function FetchWebUserListByName(ByVal strLastName As String, Optional ByVal strFirstName As String = "", Optional ByVal lngProviderID As Long = 0) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 04/11/2001   R004
        'Author: Dave Richkun
        'Description:  This method was derived from FetchUserListByName() and returns
        '              matching names and IDs of Psyquel Web-based providers.
        'Parameters: strLastName - Last name of desired user
        '            strFirstName - First name of desired user
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------

        Dim objProviderAccess As New ProviderDB.CProvAccessDB
        Dim rstSQL As New ADODB.Recordset
        objProviderAccess.ConnectionString = _ConnectionString

        '   fetch method and set recordset returned to rstSQL
        rstSQL = objProviderAccess.FetchWebUserListByName(strLastName, strFirstName, lngProviderID)

        ' Set recordset to retval
        FetchWebUserListByName = rstSQL

        objProviderAccess = Nothing   ' Delete object

    End Function

End Class
