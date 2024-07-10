Option Strict Off
Option Explicit On

Imports ECSubAdm
Public Class CEDIBz
    '-------------------------------------------------------------------------------------
    'Date: 01/07/2003
    'Class Name: CEDIBz
    'Author: Dave Richkun
    'Description:   COM object designed to host methods associated with electronic
    '               data exchange.
    '--------------------------------------------------------------------------------------


    Public Function FetchElectClaims(ByVal strWhere As String, Optional ByVal strDataBase As String = "") As ADODB.Recordset

        '--------------------------------------------------------------------------------------
        'Date: 10/10/2002
        'Author: Dave Richkun
        'Description: Retrieves records from tblClaim matching the criteria specified in
        '             the strWhere parametetr.
        'Parameters: strWhere - String portion of SQL 'where' clause that identifies the
        '              claim selection criteria
        'Returns: ADODB.Recordset
        '--------------------------------------------------------------------------------------

        Dim objEDI As New EDIDB.CEDIDB

   
        'UPGRADE_WARNING: Couldn't resolve default property of object objEDI.FetchElectClaims. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        FetchElectClaims = objEDI.FetchElectClaims(strWhere, strDataBase)
        'UPGRADE_NOTE: Object objEDI may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objEDI = Nothing



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

        Dim objEDI As New EDIDB.CEDIDB

 
        'UPGRADE_WARNING: Couldn't resolve default property of object objEDI.GetNextFileNumber. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        GetNextFileNumber = objEDI.GetNextFileNumber()
        'UPGRADE_NOTE: Object objEDI may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objEDI = Nothing


    End Function


    Public Sub Insert(ByVal lngFileNumber As Integer, ByVal strFileName As String, ByVal lngStartTxNum As Integer, ByVal lngEndTxNum As Integer, ByVal strUserName As String)
 
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

        Dim objEDI As New EDIDB.CEDIDB


        'UPGRADE_WARNING: Couldn't resolve default property of object objEDI.Insert. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        Call objEDI.Insert(lngFileNumber, strFileName, lngStartTxNum, lngEndTxNum, strUserName)
        'UPGRADE_NOTE: Object objEDI may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objEDI = Nothing



    End Sub


    Public Sub EstablishXRef(ByVal lngClaimID As Integer, ByVal lngEncounterLogID As Integer, ByVal lngEFileID As Integer, ByVal strEFileName As String, Optional ByVal strDataBase As String = "")
 
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

        Dim objEDI As New EDIDB.CEDIDB


        'UPGRADE_WARNING: Couldn't resolve default property of object objEDI.EstablishXRef. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        Call objEDI.EstablishXRef(lngClaimID, lngEncounterLogID, lngEFileID, strEFileName, strDataBase)
        'UPGRADE_NOTE: Object objEDI may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objEDI = Nothing



    End Sub


    Public Sub UpdateQueuedStatus(ByVal strFileName As String)
        Dim GetObjectContext As Object
        Dim EDIDB As Object
        '--------------------------------------------------------------------------------------
        'Date: 02/25/2003
        'Author: Dave Richkun
        'Description: Updates the Queued status in tblClaim for all claims submitted under a
        '             particular file name.
        'Parameters:  strEFileName - The name of the file containing the electronic claims whose Queued
        '               status will be updated
        'Returns: Null
        '--------------------------------------------------------------------------------------

        Dim objEDI As New EDIDB.CEDIDB

        'UPGRADE_WARNING: Couldn't resolve default property of object objEDI.UpdateQueuedStatus. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        Call objEDI.UpdateQueuedStatus(strFileName)
        'UPGRADE_NOTE: Object objEDI may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objEDI = Nothing



    End Sub
    Public Sub UpdateACK(ByVal lngEFileID As Integer, ByVal lngStartTxNum As Integer, ByVal lngEndTxNum As Integer, ByVal strAK1 As String, ByVal strAK2 As String, ByVal strAKDetail As String, ByVal strAK5 As String, ByVal strAK9 As String)

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

        Dim objEDI As New EDIDB.CEDIDB

        'UPGRADE_WARNING: Couldn't resolve default property of object objEDI.UpdateACK. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        Call objEDI.UpdateACK(lngEFileID, lngStartTxNum, lngEndTxNum, strAK1, strAK2, strAKDetail, strAK5, strAK9)
        'UPGRADE_NOTE: Object objEDI may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objEDI = Nothing



    End Sub
End Class