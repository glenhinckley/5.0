VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "CEDIBz"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'-------------------------------------------------------------------------------------
'Date: 01/07/2003
'Class Name: CEDIBz
'Author: Dave Richkun
'Description:   COM object designed to host methods associated with electronic
'               data exchange.
'--------------------------------------------------------------------------------------

Option Explicit

Public Function FetchElectClaims(ByVal strWhere As String, Optional ByVal strDataBase As String) As ADODB.Recordset
'--------------------------------------------------------------------------------------
'Date: 10/10/2002
'Author: Dave Richkun
'Description: Retrieves records from tblClaim matching the criteria specified in
'             the strWhere parametetr.
'Parameters: strWhere - String portion of SQL 'where' clause that identifies the
'              claim selection criteria
'Returns: ADODB.Recordset
'--------------------------------------------------------------------------------------

    Dim objEDI As EDIDB.CEDIDB
    
    On Error GoTo ErrTrap:
    
    Set objEDI = CreateObject("EDIDB.CEDIDB")
    Set FetchElectClaims = objEDI.FetchElectClaims(strWhere, strDataBase)
    Set objEDI = Nothing
    
    GetObjectContext.SetComplete
    
    Exit Function
    
ErrTrap:
    'Signal incompletion and raise the error to the calling enviroment.
    Set objEDI = Nothing
    Err.Raise Err.Number, Err.Source, Err.Description
    GetObjectContext.SetAbort
    
End Function


Public Function GetNextFileNumber() As ADODB.Recordset
'--------------------------------------------------------------------------------------
'Date: 12/06/2003
'Author: Dave Richkun
'Description: Returns the file number and the starting transaction (BHT) number
'             for a new E-claim file.
'Parameters: None
'Returns: ADO Recordset containing new file number and starting transaction number.
'--------------------------------------------------------------------------------------

    Dim objEDI As EDIDB.CEDIDB
    
    On Error GoTo ErrTrap:
    
    Set objEDI = CreateObject("EDIDB.CEDIDB")
    Set GetNextFileNumber = objEDI.GetNextFileNumber()
    Set objEDI = Nothing
    
    GetObjectContext.SetComplete
    Exit Function
    
ErrTrap:
    Set objEDI = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Function


Public Sub Insert(ByVal lngFileNumber As Long, ByVal strFileName As String, _
                       ByVal lngStartTxNum As Long, ByVal lngEndTxNum As Long, _
                       ByVal strUserName As String)
'--------------------------------------------------------------------------------------
'Date: 12/07/2003
'Author: Dave Richkun
'Description: Inserts a row into tblEFile via a stored procedure
'Parameters: lngFileNumber - ID of the file
'            lngFileName - The name of the file
'            lngStartTxNum - Starting transaction number within the file
'            lngEndTxNum - Ending transaction number within the file
'            strUserName - Name of the user running the procedure
'Returns: Null
'--------------------------------------------------------------------------------------

    Dim objEDI As EDIDB.CEDIDB
    
    On Error GoTo ErrTrap:
    
    Set objEDI = CreateObject("EDIDB.CEDIDB")
    Call objEDI.Insert(lngFileNumber, strFileName, lngStartTxNum, lngEndTxNum, strUserName)
    Set objEDI = Nothing

    GetObjectContext.SetComplete
    
    Exit Sub
    
ErrTrap:
    Set objEDI = Nothing
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub


Public Sub EstablishXRef(ByVal lngClaimID As Long, ByVal lngEncounterLogID As Long, _
                         ByVal lngEFileID As Long, ByVal strEFileName As String, Optional ByVal strDataBase As String)
'--------------------------------------------------------------------------------------
'Date: 01/21/2003
'Author: Dave Richkun
'Description: Establishes a cross-reference between a Psyquel claim and an electronically
'             submitted claim.
'Parameters: lngClaimID - ID of the row in tblClaim that will be updated
'            lngEncounterLogID - ID of the encounter log in tblClaim that will be updated
'            lngEFileID - ID of the number assigned to the electronic claim
'            strEFileName - The name of the file containing the electronic claim
'Returns: Null
'--------------------------------------------------------------------------------------

    Dim objEDI As EDIDB.CEDIDB
    
    On Error GoTo ErrTrap:
    
    Set objEDI = CreateObject("EDIDB.CEDIDB")
    Call objEDI.EstablishXRef(lngClaimID, lngEncounterLogID, lngEFileID, strEFileName, strDataBase)
    Set objEDI = Nothing

    GetObjectContext.SetComplete
    Exit Sub
    
ErrTrap:
    Set objEDI = Nothing
    
    GetObjectContext.SetAbort
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub


Public Sub UpdateQueuedStatus(ByVal strFileName As String)
'--------------------------------------------------------------------------------------
'Date: 02/25/2003
'Author: Dave Richkun
'Description: Updates the Queued status in tblClaim for all claims submitted under a
'             particular file name.
'Parameters:  strEFileName - The name of the file containing the electronic claims whose Queued
'               status will be updated
'Returns: Null
'--------------------------------------------------------------------------------------

    Dim objEDI As EDIDB.CEDIDB
    
    On Error GoTo ErrTrap:
    
    Set objEDI = CreateObject("EDIDB.CEDIDB")
    Call objEDI.UpdateQueuedStatus(strFileName)
    Set objEDI = Nothing

    GetObjectContext.SetComplete
    Exit Sub
    
ErrTrap:
    Set objEDI = Nothing
    
    GetObjectContext.SetComplete
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub
Public Sub UpdateACK(ByVal lngEFileID As Long, _
                     ByVal lngStartTxNum As Long, ByVal lngEndTxNum As Long, _
                     ByVal strAK1 As String, ByVal strAK2 As String, _
                     ByVal strAKDetail As String, _
                     ByVal strAK5 As String, ByVal strAK9 As String)
'--------------------------------------------------------------------------------------
'Date: 11/01/2007
'Author: Duane C Orth
'Description: Updates the Acknoledegement fields in tblEFile for all claims submitted under a
'             particular file id.
'Parameters: lngEFileID - ID of the file under which claims have been electronically submitted
'            lngStartTxNum - Starting transaction number within the file
'            lngEndTxNum - Ending transaction number within the file
'Returns: Null
'--------------------------------------------------------------------------------------

    Dim objEDI As EDIDB.CEDIDB
    
    On Error GoTo ErrTrap:
    
    Set objEDI = CreateObject("EDIDB.CEDIDB")
    Call objEDI.UpdateACK(lngEFileID, lngStartTxNum, lngEndTxNum, strAK1, strAK2, strAKDetail, strAK5, strAK9)
    Set objEDI = Nothing

    GetObjectContext.SetComplete
    Exit Sub
    
ErrTrap:
    Set objEDI = Nothing
    
    GetObjectContext.SetComplete
    Err.Raise Err.Number, Err.Source, Err.Description

End Sub

