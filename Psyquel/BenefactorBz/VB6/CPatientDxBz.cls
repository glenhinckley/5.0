VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 2  'RequiresTransaction
END
Attribute VB_Name = "CPatientDxBz"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
'--------------------------------------------------------------------
'Class Name: CPatientDxBz
'Date: 01/18/2000
'Author: Eric Pena
'Description:  MTS business object designed to call methods associated
'              with the CPatientDxDB class.
'--------------------------------------------------------------------
Option Explicit
Private Const CLASS_NAME As String = "CPatientDxBz"
Private Const TABLE_NAME As String = "tblPatientDiagnosis"
Public Function Insert(ByVal lngPatientID As Long, ByVal intAxisIa As Long, ByVal strAxisIaModifier As String, ByVal intAxisIb As Long, ByVal strAxisIbModifier As String, _
                                    ByVal intAxisIc As Long, ByVal strAxisIcModifier As String, ByVal intAxisId As Long, ByVal strAxisIdModifier As String, _
                                    ByVal intAxisIIa As Long, ByVal strAxisIIaModifier As String, ByVal intAxisIIb As Long, ByVal strAxisIIbModifier As String, _
                                    ByVal strAxisIII As String, ByVal strAxisIV As String, ByVal strAxisV As String, ByVal strDxNotes As String, ByVal strAddedBy As String)
'--------------------------------------------------------------------
'Date: 01/29/2000
'Author: Eric Pena
'Description:  Inserts a row into the tblPatientDiagnosis table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'Returns: null
'--------------------------------------------------------------------
    
    Dim objPtDiag As BenefactorDB.CPatientDxDB
    Dim strErrMessage As String
    
    On Error GoTo ErrTrap:
    
    
    Set objPtDiag = CreateObject("BenefactorDB.CPatientDxDB")
    objPtDiag.Insert lngPatientID, intAxisIa, strAxisIaModifier, intAxisIb, strAxisIbModifier, intAxisIc, _
                            strAxisIcModifier, intAxisId, strAxisIdModifier, intAxisIIa, strAxisIIaModifier, intAxisIIb, strAxisIIbModifier, _
                            strAxisIII, strAxisIV, strAxisV, strDxNotes, strAddedBy
                            
    'Signal successful completion
    GetObjectContext.SetComplete
    Set objPtDiag = Nothing
    Exit Function
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    GetObjectContext.SetAbort
    Set objPtDiag = Nothing
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
End Function
Public Sub Update(ByVal lngPatientID As Long, ByVal intAxisIa As Integer, ByVal strAxisIaModifier As String, ByVal intAxisIb As Integer, ByVal strAxisIbModifier As String, _
                                    ByVal intAxisIc As Integer, ByVal strAxisIcModifier As String, ByVal intAxisId As Integer, ByVal strAxisIdModifier As String, _
                                    ByVal intAxisIIa As Integer, ByVal strAxisIIaModifier As String, ByVal intAxisIIb As Integer, ByVal strAxisIIbModifier As String, _
                                    ByVal strAxisIII As String, ByVal strAxisIV As String, ByVal strAxisV As String, ByVal strDxNotes As String)
'--------------------------------------------------------------------
'Date: 01/29/2000
'Author: Eric Pena
'Description:  Updates a row in the tblPatientDiagnosis table utilizing
'              a stored procedure.
'Parameters: Each parameter identifies the column value that will be
'              inserted.
'Returns: Null
'--------------------------------------------------------------------

    
    Dim objPtDiag As BenefactorDB.CPatientDxDB
    Dim strErrMessage As String

    On Error GoTo ErrTrap:

    

    Set objPtDiag = CreateObject("BenefactorDB.CPatientDxDB")
    objPtDiag.Update lngPatientID, intAxisIa, strAxisIaModifier, intAxisIb, strAxisIbModifier, intAxisIc, _
                            strAxisIcModifier, intAxisId, strAxisIdModifier, intAxisIIa, strAxisIIaModifier, intAxisIIb, strAxisIIbModifier, _
                            strAxisIII, strAxisIV, strAxisV, strDxNotes

    'Signal successful completion
    GetObjectContext.SetComplete
    
    Set objPtDiag = Nothing
    Exit Sub
ErrTrap:
    'Signal incompletion and raise the error to the calling environment.  The
    'condition handles custom business rule checks we may have established.
    Set objPtDiag = Nothing
    GetObjectContext.SetAbort
    If Err.Number = 0 Then
        Err.Raise vbObjectError, CLASS_NAME, strErrMessage
    Else
        Err.Raise Err.Number, Err.Source, Err.Description
    End If
    
End Sub
