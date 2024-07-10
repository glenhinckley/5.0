Option Strict Off
Option Explicit On
Imports Psyquel.BusinessRules.CoreLibraryIII

Public Class CDrugBZ
    '--------------------------------------------------------------------
    'Class Name: CDrugDB
    'Date: 10/24/2012
    'Author: Duane C Orth
    'Description:  MTS business object designed to  methods associated
    '              with the CDrugDB class.
    '--------------------------------------------------------------------

    Private _ConnectionString As String = String.Empty

    Public WriteOnly Property ConnectionString() As String
        Set(ByVal value As String)
            '  log.ConnectionString = value
            _ConnectionString = value

        End Set
    End Property
    Private Const CLASS_NAME As String = "CDrugDB"

    Public Function Fetch(Optional ByVal blnIncludeDisabled As Boolean = False, Optional ByVal strWhere As String = "", Optional ByVal strOrderBy As String = "", Optional ByVal lngProviderID As Integer = 0) As ADODB.Recordset
        '--------------------------------------------------------------------
        'Date: 10/24/2012                                                   '
        'Author: Duane C Orth                                               '
        'Description:  Retrieves records from the tblDrug table.            '
        'Parameters:                                                        '
        'Returns: Null                                                      '
        '--------------------------------------------------------------------
        'Revision History:                                                  '
        '                                                                   '
        '--------------------------------------------------------------------

        Dim objDrug As New ListDb.CDrugDB
        Dim rstSQL As New ADODB.Recordset



        rstSQL = objDrug.Fetch(blnIncludeDisabled, strWhere, strOrderBy, lngProviderID)
        Fetch = rstSQL


        'UPGRADE_NOTE: Object objDrug may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objDrug = Nothing



    End Function
    Public Function Insert(ByVal strDrugName As String) As Integer
        '--------------------------------------------------------------------
        'Date: 10/24/2012                                                   '
        'Author: Duane C Orth                                               '
        'Description:  Inserts a row into the tblDrug table utilizing
        '              a stored procedure.
        'Parameters: strDrugName - The name of the Drug
        '              that will be inserted into the table.
        'Returns: ID (Primary Key) of the row inserted.
        '--------------------------------------------------------------------


        Dim objDrug As New ListDb.CDrugDB
        Dim lngID As Integer
        Dim strErrMessage As String



        'Verify data before proceeding.
        If Not VerifyData(0, strDrugName, strErrMessage) Then
            Exit Function
        End If

        lngID = CInt(objDrug.Insert(strDrugName))
        Insert = lngID


        'UPGRADE_NOTE: Object objDrug may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objDrug = Nothing



    End Function


    Public Sub Update(ByVal lngID As Integer, ByVal strDrugName As String)
        '--------------------------------------------------------------------
        'Date: 10/24/2012                                                   '
        'Author: Duane C Orth                                               '
        'Description:  Updates a row into the tblDrug table utilizing
        '              a stored procedure.
        'Parameters:  lngID - ID of the row in the table whose value will be
        '               updated.
        '             strDrugName - The name given to the Drug.
        '             strDescription - The Drug status description
        '                to which the record will be changed.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------
        Dim objDrug As New ListDb.CDrugDB
        Dim strErrMessage As String



        'Verify data before proceeding.
        If Not VerifyData(lngID, strDrugName, strErrMessage) Then
            Exit Sub
        End If

        objDrug.Update(lngID, strDrugName)

        'UPGRADE_NOTE: Object objDrug may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objDrug = Nothing



        'UPGRADE_NOTE: Object objDrug may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objDrug = Nothing



    End Sub


    Public Sub Delete(ByVal blnDeleted As Boolean, ByVal lngID As Integer)
        '--------------------------------------------------------------------
        'Date: 10/24/2012                                                   '
        'Author: Duane C Orth                                               '
        'Description:  Flags a row in the tblDrugs table marking the row as
        '              deleted or undeleted.
        'Parameters: blnDeleted - Boolean value identifying if the record is to
        '               be deleted (True) or undeleted (False).
        '            lngID - ID of the row in the table whose value will be
        '               updated.
        'Returns: Null
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim objDrug As New ListDb.CDrugDB





        objDrug.Delete(blnDeleted, lngID)

 
        'UPGRADE_NOTE: Object objDrug may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objDrug = Nothing

        Exit Sub


    End Sub


    Public Function Exists(ByVal strDrugName As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 10/24/2012                                                   '
        'Author: Duane C Orth                                               '
        'Description:  Determines if an Employment Status description identical
        '              to the strDescription parameter already exists in the table.
        'Parameters: strDrugName - Drug record to be checked
        'Returns: True if the name exists, false otherwise
        '--------------------------------------------------------------------
        'Revision History:
        '
        '--------------------------------------------------------------------


        Dim objDrug As New ListDb.CDrugDB
        Dim blnExists As Boolean






        blnExists = objDrug.Exists(strDrugName)
        Exists = blnExists



        'UPGRADE_NOTE: Object objDrug may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        objDrug = Nothing

 
    End Function



    Private Function VerifyData(ByVal lngID As Integer, ByVal strDrugName As String, ByRef strErrMessage As String) As Boolean
        '--------------------------------------------------------------------
        'Date: 10/24/2012                                                   '
        'Author: Duane C Orth                                               '
        'Description:  Verifies all required data has been provided by the user.
        'Parameters:  The values to be checked.
        'Returns: Boolean value identifying if all criteria has been satisfied.
        '--------------------------------------------------------------------

        If Trim(strDrugName) = "" Then
            strErrMessage = "Drug name is required."
            VerifyData = False
            Exit Function
        End If

        'Check for existance only when inserting new data
        If lngID = 0 And Exists(strDrugName) Then
            strErrMessage = "Drug name '" & strDrugName & "' already exists."
            VerifyData = False
            Exit Function
        End If

        'If we get here, all is well...
        VerifyData = True

    End Function
End Class