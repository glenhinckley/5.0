VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CClinicBz"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CClinicBz
'Date: 12/05/1999
'Author: Dave Richkun
'Description:  MTS business object designed to call methods associated
'              with the CClinicDB class.
'--------------------------------------------------------------------

Option Explicit

Private Const CLASS_NAME As String = "CClinicBz"
Private Const TABLE_NAME As String = "tblClinic"

Public Function FetchDetail(ByVal lngID As Long, Optional blnIncludeDisabled As Boolean = False) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 03/06/2000                                                   '
'Author: Rick "Boom Boom" Segura                                    '
'Description:  Retrieves records from the tblClinic table.          '
'Parameters: blnIncludeDisabled - Optional parameter that identifies'
'               if records flagged as 'Disabled' or 'De-activated'  '
'               are to be included in the record set. The default   '
'               value is False.                                     '
'Returns: Recordset of clinics ordered by business name             '
'--------------------------------------------------------------------
'Revision History:                                                  '
'                                                                   '
'--------------------------------------------------------------------

    Dim objClinic As ClinicDB.CClinicDB
    Dim rst As ADODB.Recordset

    On Error GoTo ErrTrap:

    Set objClinic = CreateObject("ClinicDB.CClinicDB")
    
    Set rst = objClinic.FetchDetail(lngID, blnIncludeDisabled)
    Set FetchDetail = rst
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Set objClinic = Nothing

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Public Function FetchByGroup(ByVal lngGroupID As Long, Optional blnIncludeDisabled As Boolean = False) As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 02/03/2015                                                   '
'Author: Duane C Orth                                               '
'Description:  Retrieves records from the tblClinic table.          '
'Parameters: blnIncludeDisabled - Optional parameter that identifies'
'               if records flagged as 'Disabled' or 'De-activated'  '
'               are to be included in the record set. The default   '
'               value is False.                                     '
'Returns: Recordset of clinics ordered by Group                     '
'--------------------------------------------------------------------
'Revision History:                                                  '
'                                                                   '
'--------------------------------------------------------------------

    Dim objClinic As ClinicDB.CClinicDB
    Dim rstSQL As ADODB.Recordset

    On Error GoTo ErrTrap:

    Set objClinic = CreateObject("ClinicDB.CClinicDB")
    
    Set rstSQL = objClinic.FetchByGroup(lngGroupID, blnIncludeDisabled)
    Set FetchByGroup = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Set objClinic = Nothing

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Public Function FetchByName(Optional blnIncludeDisabled As Boolean = False) _
    As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 03/02/2000                                                   '
'Author: Rick "Boom Boom" Segura                                    '
'Description:  Retrieves records from the tblClinic table.          '
'Parameters: blnIncludeDisabled - Optional parameter that identifies'
'               if records flagged as 'Disabled' or 'De-activated'  '
'               are to be included in the record set. The default   '
'               value is False.                                     '
'Returns: Recordset of clinics ordered by business name             '
'--------------------------------------------------------------------
'Revision History:                                                  '
'                                                                   '
'--------------------------------------------------------------------

    Dim objClinic As ClinicDB.CClinicDB
    Dim rstSQL As ADODB.Recordset

    On Error GoTo ErrTrap:

    Set objClinic = CreateObject("ClinicDB.CClinicDB")
    
    Set rstSQL = objClinic.FetchByName(blnIncludeDisabled)
    Set FetchByName = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Set objClinic = Nothing

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Function


Public Function FetchByZip(Optional blnIncludeDisabled As Boolean = False) _
    As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 03/02/2000                                                   '
'Author: Rick "Boom Boom" Segura                                    '
'Description:  Retrieves records from the tblClinic table.          '
'Parameters: blnIncludeDisabled - Optional parameter that identifies'
'               if records flagged as 'Disabled' or 'De-activated'  '
'               are to be included in the record set. The default   '
'               value is False.                                     '
'Returns: Recordset of clinics ordered by zip code                  '
'--------------------------------------------------------------------
'Revision History:                                                  '
'                                                                   '
'--------------------------------------------------------------------

    Dim objClinic As ClinicDB.CClinicDB
    Dim rstSQL As ADODB.Recordset

    On Error GoTo ErrTrap:

    Set objClinic = CreateObject("ClinicDB.CClinicDB")
    
    Set rstSQL = objClinic.FetchByZip(blnIncludeDisabled)
    Set FetchByZip = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Set objClinic = Nothing

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Public Function FetchByCity(Optional blnIncludeDisabled As Boolean = False) _
    As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 03/02/2000                                                   '
'Author: Rick "Boom Boom" Segura                                    '
'Description:  Retrieves records from the tblClinic table.          '
'Parameters: blnIncludeDisabled - Optional parameter that identifies'
'               if records flagged as 'Disabled' or 'De-activated'  '
'               are to be included in the record set. The default   '
'               value is False.                                     '
'Returns: Recordset of clinics ordered by city                      '
'--------------------------------------------------------------------
'Revision History:                                                  '
'                                                                   '
'--------------------------------------------------------------------

    Dim objClinic As ClinicDB.CClinicDB
    Dim rstSQL As ADODB.Recordset

    On Error GoTo ErrTrap:

    Set objClinic = CreateObject("ClinicDB.CClinicDB")
    
    Set rstSQL = objClinic.FetchByCity(blnIncludeDisabled)
    Set FetchByCity = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Set objClinic = Nothing

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Public Function FetchByState(Optional blnIncludeDisabled As Boolean = False) _
    As ADODB.Recordset
'--------------------------------------------------------------------
'Date: 03/02/2000                                                   '
'Author: Rick "Boom Boom" Segura                                    '
'Description:  Retrieves records from the tblClinic table.          '
'Parameters: blnIncludeDisabled - Optional parameter that identifies'
'               if records flagged as 'Disabled' or 'De-activated'  '
'               are to be included in the record set. The default   '
'               value is False.                                     '
'Returns: Recordset of clinics ordered by State                     '
'--------------------------------------------------------------------
'Revision History:                                                  '
'                                                                   '
'--------------------------------------------------------------------

    Dim objClinic As ClinicDB.CClinicDB
    Dim rstSQL As ADODB.Recordset

    On Error GoTo ErrTrap:

    Set objClinic = CreateObject("ClinicDB.CClinicDB")
    
    Set rstSQL = objClinic.FetchByState(blnIncludeDisabled)
    Set FetchByState = rstSQL
    
    'Signal successful completion
    GetObjectContext.SetComplete

    Set objClinic = Nothing

    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Function

Public Function Insert(ByVal strBusinessName As String, ByVal strAddress1 As String, _
                       ByVal strAddress2 As String, ByVal strCity As String, _
                       ByVal strState As String, ByVal strZip As String, _
                       ByVal strPhone As String, ByVal strFax As String, _
                       ByVal strSubmitterID As String, ByVal strPOSCode As String, _
                       ByVal strNPI As String, ByVal strTaxonomy As String, _
                       ByVal strAddedBy As String, ByVal strClinicType As String, _
                       ByVal strClaimType As String, ByVal blnShortageAreaYN As Boolean) As Long
'--------------------------------------------------------------------
'Date: 12/05/1999
'Author: Dave Richkun
'Description:  Inserts a row into the tblClinic table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'Returns: ID (Primary Key) of the row inserted.
'--------------------------------------------------------------------

    Dim objClinic As ClinicDB.CClinicDB
    Dim lngID As Long
    Dim strErrMessage As String
    
    On Error GoTo ErrTrap:

    If Not VerifyData(lngID, strBusinessName, strAddress1, strCity, strState, _
                      strClinicType, strErrMessage) Then
        GoTo ErrTrap
    End If
    
    Set objClinic = CreateObject("ClinicDB.CClinicDB")
    lngID = objClinic.Insert(strBusinessName, strAddress1, strAddress2, strCity, _
                             strState, strZip, strPhone, strFax, strSubmitterID, strPOSCode, _
                             strNPI, strTaxonomy, strAddedBy, strClinicType, strClaimType, blnShortageAreaYN)
    
    Insert = lngID
    
    'Signal successful completion
    GetObjectContext.SetComplete
    
    Set objClinic = Nothing
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Function

Public Sub Update(ByVal lngID As Long, ByVal strBusinessName As String, _
                  ByVal strAddress1 As String, ByVal strAddress2 As String, _
                  ByVal strCity As String, ByVal strState As String, _
                  ByVal strZip As String, ByVal strPhone As String, _
                  ByVal strFax As String, ByVal strSubmitterID As String, _
                  ByVal strPOSCode As String, ByVal strNPI As String, _
                  ByVal strTaxonomy As String, ByVal strClinicType As String, _
                  ByVal strClaimType As String, ByVal blnShortageAreaYN As Boolean)
'--------------------------------------------------------------------
'Date: 12/27/1999
'Author: Dave Richkun
'Description:  Updates a row in the tblClinic table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'Returns: Null
'--------------------------------------------------------------------

    Dim objClinic As ClinicDB.CClinicDB
    Dim strErrMessage As String

    On Error GoTo ErrTrap:

    If Not VerifyData(lngID, strBusinessName, strAddress1, strCity, _
                      strState, strClinicType, strErrMessage) Then
        GoTo ErrTrap
    End If

    Set objClinic = CreateObject("ClinicDB.CClinicDB")
    Call objClinic.Update(lngID, strBusinessName, strAddress1, strAddress2, _
                  strCity, strState, strZip, strPhone, strFax, strSubmitterID, _
                  strPOSCode, strNPI, strTaxonomy, strClinicType, strClaimType, blnShortageAreaYN)

    'Signal successful completion
    GetObjectContext.SetComplete
    
    Set objClinic = Nothing
    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
    
End Sub


Public Sub Deleted(ByVal blnDeleted As Boolean, ByVal lngID As Long, _
                   ByVal strUserName As String)
'--------------------------------------------------------------------
'Date: 12/30/1999
'Author: Dave Richkun
'Description:  Flags a row in the tblClinic table marking the row as
'              deleted or undeleted.
'Parameters: blnDeleted - Boolean value identifying if the record is to
'               be deleted (True) or undeleted (False).
'            lngID - ID of the row in the table whose value will be
'               updated.
'            strUserName - Login name of the user responsible for
'               marking the row as deleted.
'Returns: Null
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    Dim objClinic As ClinicDB.CClinicDB
    
    On Error GoTo ErrTrap:

    Set objClinic = CreateObject("ClinicDB.CClinicDB")
    
    Call objClinic.Deleted(blnDeleted, lngID, strUserName)

    'Signal successful completion
    GetObjectContext.SetComplete

    Set objClinic = Nothing

    Exit Sub
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub


Private Function VerifyData(ByVal lngID As Long, ByVal strBusinessName As String, _
                            ByVal strAddress1 As String, ByVal strCity As String, _
                            ByVal strState As String, ByVal strClinicType As String, _
                            ByRef strErrMessage As String) As Boolean
'--------------------------------------------------------------------
'Date: 01/04/2000
'Author: Dave Richkun
'Description:  Verifies all required data has been provided by the user.
'Parameters:  The values to be checked.
'Returns: Boolean value identifying if all criteria has been satisfied.
'--------------------------------------------------------------------

    If Trim(strBusinessName) = "" Then
        strErrMessage = "Business Name is required."
        VerifyData = False
        Exit Function
    End If
    
    If Trim(strAddress1) = "" Then
        strErrMessage = "Street address is required."
        VerifyData = False
        Exit Function
    End If

    If Trim(strCity) = "" Then
        strErrMessage = "City is required."
        VerifyData = False
        Exit Function
    End If

    If Trim(strState) = "" Then
        strErrMessage = "State is required."
        VerifyData = False
        Exit Function
    End If
    
    If Trim(strClinicType) = "" Then
        strErrMessage = "Clinic Type is required."
        VerifyData = False
        Exit Function
    End If
    
    'Check for existance only when inserting new data
    If lngID = 0 And Exists(strBusinessName) Then
        strErrMessage = "Business Name '" & strBusinessName & "' already exists."
        VerifyData = False
        Exit Function
    End If

    'If we get here, all is well...
    VerifyData = True

End Function


Public Function Exists(strBusinessName As String) As Boolean
'--------------------------------------------------------------------
'Date: 01/05/2000
'Author: Dave Richkun
'Description:  Determines if the BusinessName identical to the
'              strBusinessName parameter already exists in the table.
'Parameters: strBusinessName - Business name to be checked
'Returns: True if the name exists, false otherwise
'--------------------------------------------------------------------
'Revision History:
'
'--------------------------------------------------------------------

    Dim objClinic As ClinicDB.CClinicDB
    Dim blnExists As Boolean

    On Error GoTo ErrTrap:

    Set objClinic = CreateObject("ClinicDB.CClinicDB")
    blnExists = objClinic.Exists(strBusinessName)

    Exists = blnExists

    Set objClinic = Nothing

    'Signal successful completion
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Function


