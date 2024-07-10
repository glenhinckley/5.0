Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Friend Class frmMain
	Inherits System.Windows.Forms.Form
	
	Private g_FileName As String
	Private g_FileNumber As Integer
	Private g_lngStartTxNum As Integer
	Private g_lngEndTxNum As Integer
	
	Private Const INSCO_TEXAS_WORKERS_COMP As Integer = 606
	Private Const INSCO_MEDICARE As Integer = 24
	Private Const INSCO_MEDICARE_PARTB As Integer = 29
	Private Const INSCO_MEDICAID As Integer = 105
	Private Const INSCO_BCBS As Integer = 158
	Private Const INSCO_BCBS_TX As Integer = 6
	Private Const INSCO_CHAMPUS As Integer = 1050
	
	Private CONST_OUTPUT_DIR As String
	
	'----------------------
	' Form Events
	'----------------------
	Private Sub cmdDone_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDone.Click
		Dim Index As Short = cmdDone.GetIndex(eventSender)
		Select Case Index
			Case 0
				GenerateFile()
			Case 1
				Me.Close()
		End Select
	End Sub
	
	Private Sub frmMain_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		InitSettings()
	End Sub
	
	
	'----------------------
	' Private Methods
	'----------------------
	
	Private Sub InitSettings()
		
		Dim strTemp As String
		Dim strLoc As String
		Dim lngPos As Integer
		On Error GoTo ErrTrap
		
		'make sure .ini exists
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		strLoc = Dir(My.Application.Info.DirectoryPath & "\EClaim.ini")
		
		If strLoc = "" Then
			'we are resetting
SetLoc: 
			FileOpen(1, (My.Application.Info.DirectoryPath & "\EClaim.ini"), OpenMode.Output)
			PrintLine(1, "[EClaim]")
			
			MsgBox("Please choose an output directory", MsgBoxStyle.Information, "Initialize settings")
			frmBrowse.ShowDialog()
			
			strLoc = frmBrowse.dirList.Path
			If VB.Right(strLoc, 1) <> "\" Then
				strLoc = strLoc & "\"
			End If
			If strLoc <> "" Then
				PrintLine(1, "Output=" & strLoc)
			End If
			
			frmBrowse.Close()
			FileClose(1)
		Else
			'find and set parameters from .ini
			strLoc = ""
			FileOpen(1, (My.Application.Info.DirectoryPath & "\EClaim.ini"), OpenMode.Input)
			Do While Not EOF(1)
				Input(1, strTemp)
				lngPos = InStr(strTemp, "=")
				If lngPos > 1 Then
					Select Case VB.Left(strTemp, lngPos - 1)
						Case "Output"
							strLoc = VB.Right(strTemp, Len(strTemp) - lngPos)
					End Select
				End If
			Loop 
			
			FileClose(1)
			
			'if no output dir is found, recreate file
			If strLoc = "" Then
				MsgBox("Error")
				Kill(My.Application.Info.DirectoryPath & "\EClaim.ini")
				GoTo SetLoc
				Exit Sub
			End If
		End If
		
		CONST_OUTPUT_DIR = strLoc
		
		Exit Sub
ErrTrap: 
		FileClose(1)
		Err.Raise(Err.Number, Err.Source, Err.Description)
		
	End Sub
	
	
	Private Sub GenerateFile()
		'-------------------------------------------------------------------------------
		'Author: Dave Richkun
		'Date: 11/07/2002
		'Description: Main function that generates EDI text file compliant to X.12 837 v4010
		'Parameters:
		'Returns:
		'-------------------------------------------------------------------------------
		
		On Error GoTo ErrTrap
		
		Dim objEDI As New CEDIBz
        Dim rst As New ADODB.Recordset
		Dim strPayerCode As String
		Dim strPayerCodeTmp As String
		Dim lngNumClaims As Integer
		
		'UPGRADE_WARNING: Screen property Screen.MousePointer has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"'
        'System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor
		
		CreateEDIFile()
		
		rst = objEDI.FetchElectClaims()
		
		'If payer code is not in database then RecvrID = 'ZMIXED' and PayerID = 'PRINT'
		'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
		If IsDbNull(rst.Fields("fldPayerCode").Value) Or rst.Fields("fldPayerCode").Value = "" Then
			strPayerCode = "ZMIXED"
		Else
			strPayerCode = rst.Fields("fldPayerCode").Value
		End If
		
		Call WriteISAHeader(rst.Fields("fldInsuranceID").Value, strPayerCode)
		Call WriteGroupHeader(rst.Fields("fldInsuranceID").Value, strPayerCode)
		
		Do While Not rst.EOF
			'Assign PayerCode from recordset to temp variable, considering Null and empty string conditions
			'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
			If IsDbNull(rst.Fields("fldPayerCode").Value) Or rst.Fields("fldPayerCode").Value = "" Then
				strPayerCodeTmp = "ZMIXED"
			Else
				strPayerCodeTmp = rst.Fields("fldPayerCode").Value
			End If
			
			If strPayerCode = strPayerCodeTmp Then
				Call WriteClaim(rst, strPayerCode)
				lngNumClaims = lngNumClaims + 1
			Else
				'If payer code is not in database then RecvrID = 'ZMIXED' and PayerID = 'PRINT'
				'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
				If IsDbNull(rst.Fields("fldPayerCode").Value) Or rst.Fields("fldPayerCode").Value = "" Then
					strPayerCode = "ZMIXED"
				Else
					strPayerCode = rst.Fields("fldPayerCode").Value
				End If
				
				Call WriteClaim(rst, strPayerCode)
				lngNumClaims = lngNumClaims + 1
			End If
			
			If Not rst.EOF Then
				rst.MoveNext()
			Else
				Exit Do
				Call WriteGroupTrailer(lngNumClaims)
			End If
		Loop 
		
		If rst.EOF Then
			Call WriteGroupTrailer(lngNumClaims)
			Call WriteISATrailer()
		End If
		
		'UPGRADE_NOTE: Object rst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
		rst = Nothing
		
		CloseFile()
		
		'UPGRADE_ISSUE: Unable to determine which constant to upgrade vbNormal to. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="B3B44E51-B5F1-4FD7-AA29-CAD31B71F487"'
		'UPGRADE_ISSUE: Screen property Screen.MousePointer does not support custom mousepointers. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="45116EAB-7060-405E-8ABE-9DBB40DC2E86"'
		'UPGRADE_WARNING: Screen property Screen.MousePointer has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"'
        'System.Windows.Forms.Cursor.Current = vbNormal
		
		Call MsgBox("The EDI file has been succesfully generated.", MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation, "Complete")
		
		Exit Sub
		
ErrTrap: 
		'UPGRADE_ISSUE: Unable to determine which constant to upgrade vbNormal to. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="B3B44E51-B5F1-4FD7-AA29-CAD31B71F487"'
		'UPGRADE_ISSUE: Screen property Screen.MousePointer does not support custom mousepointers. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="45116EAB-7060-405E-8ABE-9DBB40DC2E86"'
		'UPGRADE_WARNING: Screen property Screen.MousePointer has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"'
        'System.Windows.Forms.Cursor.Current = vbNormal
		MsgBox("Error: " & Err.Number & " - " & Err.Description, MsgBoxStyle.Critical, "Error in file export")
		
	End Sub
	
	
	Private Function CreateEDIFile() As Object
		'Open/create the file
		
        Dim objEDI As New CEDIBz
		Dim strFileName As String
		Dim lngStartTxNum As Integer
        Dim rst As New ADODB.Recordset
		

		rst = objEDI.GetNextFileNumber()
		'UPGRADE_NOTE: Object objEDI may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
		objEDI = Nothing
		
		If rst.EOF = True And rst.BOF Then
			'Defensive programming for first time run-through
			g_FileNumber = 1
			g_lngStartTxNum = 1
			g_lngEndTxNum = 1
		Else
			g_FileNumber = rst.Fields("fldFileID").Value + 1
			g_lngStartTxNum = rst.Fields("fldEndTxNum").Value + 1
			g_lngEndTxNum = rst.Fields("fldEndTxNum").Value + 1
		End If
		'UPGRADE_NOTE: Object rst may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
		rst = Nothing
		
		strFileName = VB6.Format(CStr(Month(Now)), "00") & VB6.Format(CStr(VB.Day(Now)), "00") & Mid(CStr(Year(Now)), 3)
		g_FileName = strFileName & CStr(g_FileNumber) & ".txt" 'Append file number to filename to guarantee uniqueness
		
		'UPGRADE_WARNING: Dir has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
		If Dir(CONST_OUTPUT_DIR & "\" & g_FileName) > "" Then
			Kill(CONST_OUTPUT_DIR & "\" & g_FileName)
		End If
		
		FileOpen(1, (CONST_OUTPUT_DIR & "\" & g_FileName), OpenMode.Output)
		
	End Function
	
	Private Function CloseFile() As Object
		Dim GetLoginName As Object
		'Closes the file, and inserts a record of the file in the database.
		
		Dim objEDI As CEDIBz
		Dim strUserName As String
		
		On Error GoTo ErrTrap
		
		FileClose(1) 'Close the file
		
		'UPGRADE_WARNING: Couldn't resolve default property of object GetLoginName(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strUserName = GetLoginName()
		
		objEDI = New CEDIBz
		Call objEDI.Insert(g_FileNumber, g_FileName, g_lngStartTxNum, g_lngEndTxNum - 1, strUserName)
		'UPGRADE_NOTE: Object objEDI may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
		objEDI = Nothing
		
		Exit Function
		
ErrTrap: 
		Err.Raise(Err.Number, Err.Source, Err.Description)
		'UPGRADE_NOTE: Object objEDI may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
		objEDI = Nothing
		
	End Function
	
	
	Private Sub WriteISAHeader(ByVal lngInsuranceID As Integer, ByVal strPayerCode As String)
		
		Dim strISA As String
		Dim strISA_I03, strISA_I01, strISA_I02, strISA_I04 As Object
		Dim strISA_I05 As String
		Dim strISA_I08, strISA_I06, strISA_I07, strISA_I09 As Object
		Dim strISA_I10 As String
		Dim strISA_I14, strISA_I12, strISA_I11, strISA_I13, strISA_I15 As Object
		Dim strISA_I16 As String
		
		Dim obj As CEDIBz
		Dim dteCreateDate As Date
		Dim lngNumSect As Short
		Dim lngNumSeg As Short
		
		On Error GoTo ErrTrap
		
		dteCreateDate = Now
		
		'ISA (Interchange Control Header)
		'Note: The ISA header is a fixed length segment
		'UPGRADE_WARNING: Couldn't resolve default property of object strISA_I01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strISA_I01 = "00" 'authorization information qualifier
		'UPGRADE_WARNING: Couldn't resolve default property of object strISA_I02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strISA_I02 = "          " 'authorization information
		'UPGRADE_WARNING: Couldn't resolve default property of object strISA_I03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strISA_I03 = "01" 'security information qualifier
		'UPGRADE_WARNING: Couldn't resolve default property of object strISA_I04. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strISA_I04 = "V06218    " 'security information
		strISA_I05 = "ZZ" 'interchange ID qualifier (ZZ = mutually defined)
		'UPGRADE_WARNING: Couldn't resolve default property of object strISA_I06. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strISA_I06 = "V06218         " 'interchange sender ID (length must be 15)
		'UPGRADE_WARNING: Couldn't resolve default property of object strISA_I07. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strISA_I07 = "ZZ" 'interchange ID qualifier (ZZ = mutually defined)
		
		'If the file contains multiple payers then ISA108 should read ZMIXED
		'strISA_I08 = "ZMIXED         " 'Padded to 15 characters
		'UPGRADE_WARNING: Couldn't resolve default property of object strISA_I08. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strISA_I08 = RPad(CreatePayerCodePrefix(lngInsuranceID, strPayerCode), 15 - Len(strPayerCode) - 1) 'interchange receiver ID (length must be 15)
		
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strISA_I09. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strISA_I09 = VB6.Format(dteCreateDate, "yymmdd") 'interchange date
		strISA_I10 = VB6.Format(Now, "HHMM") 'interchange time
		'UPGRADE_WARNING: Couldn't resolve default property of object strISA_I11. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strISA_I11 = "U" 'interchange control standards identifier
		'UPGRADE_WARNING: Couldn't resolve default property of object strISA_I12. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strISA_I12 = "00401" 'interchange control version number
		'UPGRADE_WARNING: Couldn't resolve default property of object strISA_I13. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strISA_I13 = VB6.Format(g_FileNumber, "000000000") 'interchange control number
		'UPGRADE_WARNING: Couldn't resolve default property of object strISA_I14. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strISA_I14 = "1" 'acknowledgment requested (0 = no ack requested; 1 ack requested)
		'UPGRADE_WARNING: Couldn't resolve default property of object strISA_I15. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strISA_I15 = "P" 'usage indicator (P = production data / T = test data)
		strISA_I16 = ":" 'component element separator
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strISA_I15. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strISA_I14. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strISA_I13. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strISA_I12. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strISA_I11. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strISA_I09. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strISA_I08. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strISA_I07. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strISA_I06. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strISA_I04. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strISA_I03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strISA_I02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strISA_I01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strISA = "ISA*" & strISA_I01 & "*" & strISA_I02 & "*" & strISA_I03 & "*" & strISA_I04 & "*" & strISA_I05 & "*" & strISA_I06 & "*" & strISA_I07 & "*" & strISA_I08 & "*" & strISA_I09 & "*" & strISA_I10 & "*" & strISA_I11 & "*" & strISA_I12 & "*" & strISA_I13 & "*" & strISA_I14 & "*" & strISA_I15 & "*" & strISA_I16 & "~"
		
		PrintLine(1, strISA)
		
		Exit Sub
		
ErrTrap: 
		Err.Raise(Err.Number, Err.Source, Err.Description)
		
	End Sub
	
	Private Sub WriteGroupHeader(ByVal lngInsuranceID As Integer, ByVal strPayerCode As String)
		
		Dim strGS As String
		Dim strGS_GS02, strGS_GS01, strGS_GS03 As Object
		Dim strGS_GS04 As String
		Dim strGS_GS06, strGS_GS05, strGS_GS07 As Object
		Dim strGS_GS08 As String
		Dim dteCreateDate As Date
		
		On Error GoTo ErrTrap
		
		'GS (Functional Group Header)
		'UPGRADE_WARNING: Couldn't resolve default property of object strGS_GS01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strGS_GS01 = "HC" 'functional identifier code
		'UPGRADE_WARNING: Couldn't resolve default property of object strGS_GS02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strGS_GS02 = "V06218" 'application sender's code
		
		'strGS_GS03 = strPayerCode        'application receiver's code
		'UPGRADE_WARNING: Couldn't resolve default property of object strGS_GS03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strGS_GS03 = CreatePayerCodePrefix(lngInsuranceID, strPayerCode)
		
		strGS_GS04 = VB6.Format(dteCreateDate, "yyyymmdd") 'date
		'UPGRADE_WARNING: Couldn't resolve default property of object strGS_GS05. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strGS_GS05 = VB6.Format(dteCreateDate, "hhnn") 'time
		'UPGRADE_WARNING: Couldn't resolve default property of object strGS_GS06. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strGS_GS06 = g_FileNumber 'group control number
		'UPGRADE_WARNING: Couldn't resolve default property of object strGS_GS07. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strGS_GS07 = "X" 'responsible agency code
		strGS_GS08 = "004010X098" 'version/release/industry identifier code
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strGS_GS07. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strGS_GS06. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strGS_GS05. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strGS_GS03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strGS_GS02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strGS_GS01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strGS = "GS*" & strGS_GS01 & "*" & strGS_GS02 & "*" & strGS_GS03 & "*" & strGS_GS04 & "*" & strGS_GS05 & "*" & strGS_GS06 & "*" & strGS_GS07 & "*" & strGS_GS08 & "~"
		
		PrintLine(1, strGS)
		
		Exit Sub
		
ErrTrap: 
		Err.Raise(Err.Number, Err.Source, Err.Description)
		
	End Sub
	
	
	Private Sub WriteClaim(ByVal rst As ADODB.Recordset, ByVal strPayerCode As String)
		
		Dim lngNumSeg As Integer
		Dim lngNumSect As Integer
		
		Dim strBHT As String
		Dim strSvc As String
		Dim strRef As String
		Dim strTempCodePrefix As String
		Dim strPRV As String
		
		strTempCodePrefix = CreatePayerCodePrefix(rst.Fields("fldInsuranceID").Value, strPayerCode)
		
		Call WriteTxSetHeader(rst)
		lngNumSeg = lngNumSeg + 1
		
		Call WriteBHT()
		lngNumSeg = lngNumSeg + 1
		
		'REF (transmission type identification)
		'Note: when in production mode value is 004010X098;
		'      when in test mode value is 004010X098D
		strRef = "REF*87*004010X098~"
		PrintLine(1, strRef)
		lngNumSeg = lngNumSeg + 1
		
		If WriteLoop1000A(rst) = True Then
			lngNumSeg = lngNumSeg + 2 'If overflow line is needed, WriteLoop1000A() returns True
		Else
			lngNumSeg = lngNumSeg + 1
		End If
		
		Call WritePER()
		lngNumSeg = lngNumSeg + 1
		
		If WriteLoop1000B(rst, strPayerCode) = True Then
			lngNumSeg = lngNumSeg + 2 'WriteLoop1000B returns True if overflow line used
		Else
			lngNumSeg = lngNumSeg + 1
		End If
		
		WriteLoop2000A()
		lngNumSeg = lngNumSeg + 1
		
		'billing/payto provider info needed if payer code prefix = F
		If Mid(strTempCodePrefix, 1, 1) = "F" Then
			'TO DO: verify values in each element;
			'last element is hardcoded for testing
			strPRV = "PRV*BI*ZZ*101Y00000X~" 'ZZ relies on Provider Taxonomy Code published by BC/BS Association
			PrintLine(1, strPRV)
			lngNumSeg = lngNumSeg + 1
		End If
		
		If WriteLoop2010AA(rst) = True Then
			lngNumSeg = lngNumSeg + 2 'WriteLoop2010AA returns True if overflow line used
		Else
			lngNumSeg = lngNumSeg + 1
		End If
		
		'N3 (billing provider address)
		Call WriteSegmentN3(rst.Fields("fldProviderAddress").Value)
		lngNumSeg = lngNumSeg + 1
		
		'N4 (billing provider city/state/zip)
		Call WriteSegmentN4(rst.Fields("fldProviderCity").Value, rst.Fields("fldProviderState").Value, rst.Fields("fldProviderZip").Value)
		lngNumSeg = lngNumSeg + 1
		
		''    DR: Test if this will succeed without this information
		''    'add the billing provider secondary identification if payer code prefix = G
		''    If Left(strTempCodePrefix, 1) = "G" Then 'BC/BS
		''        'TO DO: use correct value for last element; (1) hardcoded for testing
		''        strRef = "REF*1B*1~"
		''        lngNumSeg = lngNumSeg + 1
		''        Print #1, strRef
		''    End If
		
		'Loop 2000B - Subscriber Hierarchival Level
		''    If rst.Fields("fldPatientID").Value = rst.Fields("fldRPID").Value Then
		Call WriteLoop2000B(rst)
		lngNumSeg = lngNumSeg + 1
		''    End If
		
		'Subscriber/Insured Information
		Call WriteSBR(rst)
		lngNumSeg = lngNumSeg + 1
		
		If WriteLoop2010BA(rst) = True Then
			lngNumSeg = lngNumSeg + 2
		Else
			lngNumSeg = lngNumSeg + 1
		End If
		
		If rst.Fields("fldPatientID").Value = rst.Fields("fldRPID").Value Then
			Call WriteInsuredInfo(rst)
			lngNumSeg = lngNumSeg + 3
		End If
		
		If WriteLoop2010BB(rst, strPayerCode) = True Then
			lngNumSeg = lngNumSeg + 2
		Else
			lngNumSeg = lngNumSeg + 1
		End If
		
		Call WritePayerAddress(rst)
		lngNumSeg = lngNumSeg + 2
		
		If rst.Fields("fldPatientID").Value <> rst.Fields("fldRPID").Value Then
			'Patient (non-subscriber/insured) information
			Call WriteLoop2000C(rst)
			lngNumSeg = lngNumSeg + 6
		End If
		
		Call WriteLoop2300(rst, strPayerCode)
		lngNumSeg = lngNumSeg + 1
		
		'Write Cert/authorization information
		If IfNull(rst.Fields("fldCertNum").Value, "") > "" Then
			PrintLine(1, "REF*G1*" & Trim(StripDelimiters(rst.Fields("fldCertNum").Value)) & "~")
			lngNumSeg = lngNumSeg + 1
		End If
		
		'Loop 2400 - Service Line
		strSvc = "LX*1~"
		PrintLine(1, strSvc)
		lngNumSeg = lngNumSeg + 1
		
		Call WriteLoopSV1(rst)
		lngNumSeg = lngNumSeg + 1
		
		Call WriteLoopDTP(rst)
		lngNumSeg = lngNumSeg + 1
		
		lngNumSeg = lngNumSeg + 1 'increment this before calling the sub b/c the
		'segment count includes the 'SE' segment
		Call WriteTxSetTrailer(lngNumSeg, rst)
		
		'reset the number of segments
		lngNumSeg = 0
		
		lngNumSect = lngNumSect + 1
		
	End Sub
	
	Private Sub WriteISATrailer()
		
		On Error GoTo ErrTrap
		
		'ISA footer
		Dim strIEA As String
		strIEA = "IEA*1*" & VB6.Format(g_FileNumber, "000000000") & "~"
		
		PrintLine(1, strIEA)
		
		Exit Sub
ErrTrap: 
		Err.Raise(Err.Number, Err.Source, Err.Description)
		
	End Sub
	
	Private Sub WriteGroupTrailer(ByVal intNumClaims As Short)
		
		On Error GoTo ErrTrap
		
		'GS footer
		Dim strGE As String
		
		strGE = "GE*" & CStr(intNumClaims) & "*" & CStr(g_FileNumber) & "~"
		
		PrintLine(1, strGE)
		
		Exit Sub
		
ErrTrap: 
		Err.Raise(Err.Number, Err.Source, Err.Description)
		
	End Sub
	
	Private Sub WriteTxSetHeader(ByVal rst As ADODB.Recordset)
		
		On Error GoTo ErrTrap
		
		Dim strST_ST01 As Object
		Dim strST_ST02 As String
		Dim strST As String
		
		'ST (transaction set header)
		'UPGRADE_WARNING: Couldn't resolve default property of object strST_ST01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strST_ST01 = "837" 'transaction set identifier code
		strST_ST02 = CStr(rst.Fields("fldEncounterLogID").Value) 'transaction set control number
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strST_ST01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strST = "ST*" & strST_ST01 & "*" & strST_ST02 & "~"
		
		PrintLine(1, strST)
		
		Exit Sub
		
ErrTrap: 
		Err.Raise(Err.Number, Err.Source, Err.Description)
		
	End Sub
	
	Private Sub WriteBHT()
		
		'BHT (beginning of hierarchical transaction)
		Dim strBHT As String
		Dim strBHT_BHT04, strBHT_BHT02, strBHT_BHT01, strBHT_BHT03, strBHT_BHT05 As Object
		Dim strBHT_BHT06 As String
		Dim dteCreateDate As Date
		
		On Error Resume Next
		
		dteCreateDate = Now
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strBHT_BHT01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strBHT_BHT01 = "0019" 'hierarchical structure code
		'UPGRADE_WARNING: Couldn't resolve default property of object strBHT_BHT02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strBHT_BHT02 = "00" 'transaction set purpose code
		'UPGRADE_WARNING: Couldn't resolve default property of object strBHT_BHT03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strBHT_BHT03 = VB6.Format(g_lngEndTxNum, "000000") 'reference identification
		'UPGRADE_WARNING: Couldn't resolve default property of object strBHT_BHT04. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strBHT_BHT04 = VB6.Format(dteCreateDate, "yyyymmdd") 'date
		'UPGRADE_WARNING: Couldn't resolve default property of object strBHT_BHT05. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strBHT_BHT05 = VB6.Format(dteCreateDate, "hhnn") 'time
		strBHT_BHT06 = "CH" 'transaction type code (CH = Chargable /RP = Reporting)
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strBHT_BHT05. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strBHT_BHT04. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strBHT_BHT03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strBHT_BHT02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strBHT_BHT01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strBHT = "BHT*" & strBHT_BHT01 & "*" & strBHT_BHT02 & "*" & strBHT_BHT03 & "*" & strBHT_BHT04 & "*" & strBHT_BHT05 & "*" & strBHT_BHT06 & "~"
		
		PrintLine(1, strBHT)
		
		g_lngEndTxNum = g_lngEndTxNum + 1 'According to THIN, each BHT must have a unique identifier
		
		Exit Sub
		
ErrTrap: 
		Err.Raise(Err.Number, Err.Source, Err.Description)
		
	End Sub
	
	
	Private Function WriteLoop1000A(ByVal rst As ADODB.Recordset) As Boolean
		
		Dim strNM1_03, strNM1_01, strNM1_02, strNM1_04 As Object
		Dim strNM1_05 As String
		Dim strNM1_09, strNM1_07, strNM1_06, strNM1_08, strNM1_10 As Object
		Dim strNM1_11 As String
		Dim strNM1 As Object
		Dim strN2 As String
		
		'Loop 1000A Submitter Name
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_01 = "41" 'entity identifier code (41 = submitter)
		
		'determine if we need to use a provider's last name or organization name;
		'if fldTINType = 3 then the submitter is an individual, otherwise it is an entity
		'NM1 segment (Submitter name)
		If rst.Fields("fldTinType").Value = 3 Then
			'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strNM1_02 = "1"
			
			'if the last name is more than 35 characters then we need to use
			'segment N2 to hold the overflow characters
			If Len(rst.Fields("fldProviderLastName").Value) > 35 Then
				'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				strNM1_03 = Trim(VB.Left(rst.Fields("fldProviderLastName").Value, 35))
				
				'N2 segment (Additional submitter name information)
				'this segment is used if the length of strNM1_03 is more than 35
				strN2 = "N2*" & VB.Right(rst.Fields("fldProviderLastName").Value, Len(rst.Fields("fldProviderLastName").Value) - 35) & "~"
			Else
				'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				strNM1_03 = Trim(rst.Fields("fldProviderLastName").Value)
				strN2 = ""
			End If
			
			'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_04. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strNM1_04 = VB.Left(rst.Fields("fldProviderFirstName").Value, 25)
			'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
			If IsDbNull(rst.Fields("fldProviderMI").Value) Then
				strNM1_05 = ""
			Else
				strNM1_05 = VB.Left(rst.Fields("fldProviderMI").Value, 25)
			End If
		Else
			'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strNM1_02 = "2"
			
			'if the group name is more than 35 characters then we need to use
			'segment N2 to hold the overflow characters
			If Len(rst.Fields("fldGroupName").Value) > 35 Then
				'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				strNM1_03 = Trim(VB.Left(rst.Fields("fldGroupName").Value, 35))
				
				'N2 segment (Additional submitter name information)
				'this segment is used if the length of strNM1_03 is more than 35
				strN2 = "N2*" & VB.Right(rst.Fields("fldGroupName").Value, Len(rst.Fields("fldGroupName").Value) - 35) & "~"
			Else
				'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				strNM1_03 = rst.Fields("fldGroupName").Value
				strN2 = ""
			End If
			
			'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_04. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strNM1_04 = ""
			strNM1_05 = ""
		End If
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_06. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_06 = "" 'not used (name prefix)
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_07. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_07 = "" 'not used (name suffix)
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_08. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_08 = "46" 'identification code qualifier (46 = ETIN)
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_09. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_09 = "S03588" 'submitter ID 'TO DO: Confirm correct data with THIN
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_09. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_08. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_07. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_06. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_04. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1 = "NM1*" & strNM1_01 & "*" & strNM1_02 & "*" & strNM1_03 & "*" & strNM1_04 & "*" & strNM1_05 & "*" & strNM1_06 & "*" & strNM1_07 & "*" & strNM1_08 & "*" & strNM1_09 & "~"
		
		'add segment N2 if it exists
		If strN2 <> "" Then
			WriteLoop1000A = True
		End If
		
		PrintLine(1, strNM1)
		
		If strN2 <> "" Then
			PrintLine(1, strN2) 'Print overflow line
		End If
		
		Exit Function
		
ErrTrap: 
		Err.Raise(Err.Number, Err.Source, Err.Description)
		
	End Function
	
	Private Sub WritePER()
		
		'PER segment (submitter EDI contact information)
		Dim strPer_04, strPer_02, strPer, strPer_01, strPer_03, strPer_05 As Object
		Dim strPer_06 As String
		
		On Error GoTo ErrTrap
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strPer_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strPer_01 = "IC" 'contact function code (IC = Information Contact)
		'UPGRADE_WARNING: Couldn't resolve default property of object strPer_02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strPer_02 = "PSYQUEL LP" 'submitter contact name
		'UPGRADE_WARNING: Couldn't resolve default property of object strPer_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strPer_03 = "TE" 'communication number qualifier (TE = Telephone)
		'UPGRADE_WARNING: Couldn't resolve default property of object strPer_04. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strPer_04 = "2106941354" 'communication number (phone should be XXXYYYZZZZ)
		'UPGRADE_WARNING: Couldn't resolve default property of object strPer_05. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strPer_05 = "FX" 'communication number qualifier (FX = Fax)
		strPer_06 = "2106941399" 'communication number (phone should be XXXYYYZZZZ)
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strPer_05. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strPer_04. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strPer_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strPer_02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strPer_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strPer. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strPer = "PER*" & strPer_01 & "*" & strPer_02 & "*" & strPer_03 & "*" & strPer_04 & "*" & strPer_05 & "*" & strPer_06 & "~"
		
		PrintLine(1, strPer)
		
		Exit Sub
		
ErrTrap: 
		Err.Raise(Err.Number, Err.Source, Err.Description)
		
	End Sub
	
	Private Function WriteLoop1000B(ByVal rst As ADODB.Recordset, ByVal strPayerCode As String) As Boolean
		
		'Loop 1000B segment
		Dim strNM1_03, strNM1_01, strNM1_02, strNM1_04 As Object
		Dim strNM1_05 As String
		Dim strNM1_09, strNM1_07, strNM1_06, strNM1_08, strNM1_10 As Object
		Dim strNM1_11 As String
		Dim strNM1 As Object
		Dim strN2 As String
		
		Dim strRec As String
		
		On Error GoTo ErrTrap
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_01 = "40" 'entity ID code (40 = receiver)
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_02 = "2" 'entity type qualifier (2 = non-person)
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_03 = StripDelimiters(rst.Fields("fldPlanName").Value) 'last name or organization
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_04. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_04 = "" 'first name (optional)
		strNM1_05 = "" 'middle name (optional)
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_06. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_06 = "" 'name prefix (optional)
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_07. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_07 = "" 'name suffix (optional)
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_08. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_08 = "46" 'ID code qualifier (46 = ETIN)
		
		If strPayerCode = "ZMIXED" Then
			'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_09. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strNM1_09 = strPayerCode
		Else
			'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_09. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strNM1_09 = CreatePayerCodePrefix(rst.Fields("fldInsuranceID").Value, strPayerCode)
		End If
		
		'if the receiver name is more than 35 characters then we need to use
		'segment N2 to hold the overflow characters
		If Len(strNM1_03) > 35 Then
			'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strNM1_03 = Trim(VB.Left(strNM1_03, 35))
			
			'N2 segment (Additional receiver name information)
			'this segment is used if the length of strNM1_03 is more than 35
			'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strN2 = "N2*" & VB.Right(strNM1_03, Len(strNM1_03) - 35) & "~"
		Else
			'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strNM1_03 = strNM1_03
			strN2 = ""
		End If
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_09. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_08. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_07. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_06. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_04. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strRec = "NM1*" & strNM1_01 & "*" & strNM1_02 & "*" & strNM1_03 & "*" & strNM1_04 & "*" & strNM1_05 & "*" & strNM1_06 & "*" & strNM1_07 & "*" & strNM1_08 & "*" & strNM1_09 & "~"
		
		'add segment N2 if it exists
		If strN2 <> "" Then
			WriteLoop1000B = True
		End If
		
		PrintLine(1, strRec)
		
		If strN2 <> "" Then
			PrintLine(1, strN2) 'Print overflow line
		End If
		
		
		Exit Function
		
ErrTrap: 
		Err.Raise(Err.Number, Err.Source, Err.Description)
		
	End Function
	
	Private Function WriteLoop2010AA(ByVal rst As ADODB.Recordset) As Boolean
		
		Dim str2010AA As String
		
		Dim strNM1_03, strNM1_01, strNM1_02, strNM1_04 As Object
		Dim strNM1_05 As String
		Dim strNM1_09, strNM1_07, strNM1_06, strNM1_08, strNM1_10 As Object
		Dim strNM1_11 As String
		Dim strNM1 As Object
		Dim strN2 As String
		
		On Error GoTo ErrTrap
		
		'LOOP 2010AA - Billing Provider Name (this loops actually defines the
		'              billing entity)
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_01 = "85" 'entity code identifier (85 - billing provider)
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_02 = "1" 'entity type qualifier (1=person; 2=non-person)
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_03 = rst.Fields("fldProviderLastName").Value 'last name or organization
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_04. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_04 = rst.Fields("fldProviderFirstName").Value 'billing provider first name (optional)
		strNM1_05 = IfNull(rst.Fields("fldProviderMI").Value, "") 'billing provider MI (optional)
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_06. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_06 = "" 'name prefix (not used)
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_07. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_07 = "" 'name suffix (not used)
		
		'identification code qualifier
		If rst.Fields("fldTinType").Value = 3 Then
			'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_08. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strNM1_08 = "34" 'individual SSN
		Else
			'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_08. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strNM1_08 = "24" '24 = employer's ID
		End If
		
		'UPGRADE_WARNING: Couldn't resolve default property of object NumbersOnly(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_09. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_09 = NumbersOnly((rst.Fields("fldTIN").Value)) 'billing provider identifier code
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_09. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_08. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_07. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_06. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_04. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		str2010AA = "NM1*" & strNM1_01 & "*" & strNM1_02 & "*" & strNM1_03 & "*" & strNM1_04 & "*" & strNM1_05 & "*" & strNM1_06 & "*" & strNM1_07 & "*" & strNM1_08 & "*" & strNM1_09 & "~"
		
		'if the billing provider name is more than 35 characters then we need to use
		'segment N2 to hold the overflow characters
		If Len(strNM1_03) > 35 Then
			'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strNM1_03 = Trim(VB.Left(strNM1_03, 35))
			
			'N2 segment (Additional receiver name information)
			'this segment is used if the length of strNM1_03 is more than 35
			'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strN2 = "N2*" & VB.Right(strNM1_03, Len(strNM1_03) - 35) & "~"
		Else
			'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strNM1_03 = strNM1_03
			strN2 = ""
		End If
		
		'add segment N2 if it exists
		If strN2 <> "" Then
			WriteLoop2010AA = True
		End If
		
		PrintLine(1, str2010AA)
		
		If strN2 <> "" Then
			PrintLine(1, strN2)
		End If
		
		Exit Function
		
ErrTrap: 
		Err.Raise(Err.Number, Err.Source, Err.Description)
		
	End Function
	
	Private Sub WriteSBR(ByVal rst As ADODB.Recordset)
		
		Dim strSBR_02, strSBR, strSBR_01, strSBR_03 As Object
		Dim strSBR_04 As String
		Dim strSBR_07, strSBR_05, strSBR_06, strSBR_08 As Object
		Dim strSBR_09 As String
		
		On Error GoTo ErrTrap
		
		'SBR (subscriber information)
		If rst.Fields("fldVersion").Value = 1 Then
			'UPGRADE_WARNING: Couldn't resolve default property of object strSBR_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strSBR_01 = "P" 'payer resp. sequence number code (see p. 109)
		ElseIf rst.Fields("fldVersion").Value = 2 Then 
			'UPGRADE_WARNING: Couldn't resolve default property of object strSBR_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strSBR_01 = "S"
		ElseIf rst.Fields("fldVersion").Value > 2 Then 
			'UPGRADE_WARNING: Couldn't resolve default property of object strSBR_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strSBR_01 = "T"
		End If
		
		If rst.Fields("fldRPID").Value = rst.Fields("fldPatientID").Value Then
			'UPGRADE_WARNING: Couldn't resolve default property of object strSBR_02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strSBR_02 = "18" 'individual relationship code (18 = self)
		Else
			'UPGRADE_WARNING: Couldn't resolve default property of object strSBR_02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strSBR_02 = ""
		End If
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strSBR_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strSBR_03 = StripDelimiters(IfNull(rst.Fields("fldCardNum").Value, "")) 'subscriber group or policy number
		strSBR_04 = StripDelimiters(rst.Fields("fldPlanName").Value) 'insured group name
		'UPGRADE_WARNING: Couldn't resolve default property of object strSBR_05. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strSBR_05 = "" 'TODO: insurance type code applies only to Medicare where Medicare is secondary payer
		'UPGRADE_WARNING: Couldn't resolve default property of object strSBR_06. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strSBR_06 = "" 'not used
		'UPGRADE_WARNING: Couldn't resolve default property of object strSBR_07. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strSBR_07 = "" 'not used
		'UPGRADE_WARNING: Couldn't resolve default property of object strSBR_08. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strSBR_08 = "" 'not used
		
		'UPGRADE_WARNING: Couldn't resolve default property of object GetClaimFilingCode(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strSBR_09 = GetClaimFilingCode(rst.Fields("fldInsuranceID").Value)
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strSBR_08. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strSBR_07. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strSBR_06. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strSBR_05. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strSBR_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strSBR_02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strSBR_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strSBR. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strSBR = "SBR*" & strSBR_01 & "*" & strSBR_02 & "*" & strSBR_03 & "*" & strSBR_04 & "*" & strSBR_05 & "*" & strSBR_06 & "*" & strSBR_07 & "*" & strSBR_08 & "*" & strSBR_09 & "~"
		
		PrintLine(1, strSBR)
		
		Exit Sub
		
ErrTrap: 
		Err.Raise(Err.Number, Err.Source, Err.Description)
		
	End Sub
	
	Private Function WriteLoop2010BA(ByVal rst As ADODB.Recordset) As Boolean
		
		Dim strNM1_03, strNM1_01, strNM1_02, strNM1_04 As Object
		Dim strNM1_05 As String
		Dim strNM1_09, strNM1_07, strNM1_06, strNM1_08, strNM1_10 As Object
		Dim strNM1_11 As String
		Dim strNM1 As Object
		Dim strN2 As String
		
		On Error GoTo ErrTrap
		
		'Loop 2010BA - Subscriber Name
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_01 = "IL" 'entity identifier code (IL = insured)
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_02 = "1" 'entity type qualifier (1=person)
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_03 = Mid(rst.Fields("fldInsuredLastName").Value, 1, 35) 'last name or organization
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_04. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_04 = rst.Fields("fldInsuredFirstName").Value 'first name
		strNM1_05 = IfNull(rst.Fields("fldInsuredMI").Value, "") 'middle name
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_06. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_06 = "" 'name prefix (not used)
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_07. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_07 = "" 'name suffix
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_08. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_08 = "MI" 'identification code qualifier (MI=Member Identification Number)
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_09. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_09 = StripDelimiters(IfNull(rst.Fields("fldCardNum").Value, "XX")) 'identification code
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_09. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_08. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_07. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_06. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_04. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1 = "NM1*" & strNM1_01 & "*" & strNM1_02 & "*" & strNM1_03 & "*" & strNM1_04 & "*" & strNM1_05 & "*" & strNM1_06 & "*" & strNM1_07 & "*" & strNM1_08 & "*" & strNM1_09 & "~"
		
		'N2 (additional subscriber name information)
		'if the subscriber name is more than 35 characters then we need to use
		'segment N2 to hold the overflow characters
		If Len(strNM1_03) > 35 Then
			'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strNM1_03 = VB.Left(strNM1_03, 35)
			
			'N2 segment (Additional receiver name information)
			'this segment is used if the length of strNM1_03 is more than 35
			'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strN2 = "N2*" & VB.Right(strNM1_03, Len(strNM1_03) - 35) & "~"
		Else
			strN2 = ""
		End If
		
		'add segment N2 if it exists
		If strN2 <> "" Then
			WriteLoop2010BA = True
		End If
		
		PrintLine(1, strNM1)
		
		If strN2 <> "" Then
			PrintLine(1, strN2) 'Print overflow line
		End If
		
		Exit Function
		
ErrTrap: 
		Err.Raise(Err.Number, Err.Source, Err.Description)
		
	End Function
	
	Private Sub WriteSegmentN3(ByVal strAddress As String)
		
		Dim strN3_01 As Object
		Dim strN3 As String
		
		On Error GoTo ErrTrap
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strN3_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strN3_01 = strAddress
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strN3_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strN3 = "N3*" & StripDelimiters(strN3_01) & "~"
		
		PrintLine(1, strN3)
		
		Exit Sub
		
ErrTrap: 
		Err.Raise(Err.Number, Err.Source, Err.Description)
		
	End Sub
	Private Sub WriteSegmentN3Payer(ByVal strAddress1 As String, ByVal strAddress2 As String)
		
		'payer address
		Dim strN3_01, strN3_02 As Object
		Dim strN3 As String
		
		On Error GoTo ErrTrap
		
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strN3_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strN3_01 = strAddress1
		'UPGRADE_WARNING: Couldn't resolve default property of object strN3_02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strN3_02 = strAddress2
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strN3_02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		If strN3_02 = "" Then
			'UPGRADE_WARNING: Couldn't resolve default property of object strN3_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strN3 = "N3*" & StripDelimiters(strN3_01) & "~"
		Else
			'UPGRADE_WARNING: Couldn't resolve default property of object strN3_02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			'UPGRADE_WARNING: Couldn't resolve default property of object strN3_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strN3 = "N3*" & StripDelimiters(strN3_01) & "*" & StripDelimiters(strN3_02) & "~"
		End If
		
		PrintLine(1, strN3)
		
		Exit Sub
		
ErrTrap: 
		Err.Raise(Err.Number, Err.Source, Err.Description)
		
	End Sub
	Private Sub WriteSegmentN4(ByVal strCity As String, ByVal strState As String, ByVal strZip As String)
		
		Dim strN4_02, strN4_01, strN4_03 As Object
		Dim strN4 As String
		
		On Error GoTo ErrTrap
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strN4_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strN4_01 = strCity 'city
		'UPGRADE_WARNING: Couldn't resolve default property of object strN4_02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strN4_02 = strState 'state code (2 digits)
		'UPGRADE_WARNING: Couldn't resolve default property of object strN4_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strN4_03 = strZip 'postal code
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strN4_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strN4_02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strN4_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strN4 = "N4*" & strN4_01 & "*" & strN4_02 & "*" & strN4_03 & "~"
		
		PrintLine(1, strN4)
		
		Exit Sub
		
ErrTrap: 
		Err.Raise(Err.Number, Err.Source, Err.Description)
		
	End Sub
	
	
	Private Function WriteLoop2010BB(ByVal rst As ADODB.Recordset, ByVal strPayerCode As String) As Boolean
		
		On Error GoTo ErrTrap
		
		Dim strNM1_03, strNM1_01, strNM1_02, strNM1_04 As Object
		Dim strNM1_05 As String
		Dim strNM1_09, strNM1_07, strNM1_06, strNM1_08, strNM1_10 As Object
		Dim strNM1_11 As String
		Dim strNM1 As Object
		Dim strN2 As String
		
		'Loop 2010BB - Payer Name
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_01 = "PR" 'entity identifier code (PR = payer)
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_02 = "2" 'entity type qualifier (2 = non-person)
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_03 = Mid(StripDelimiters(rst.Fields("fldInsName").Value), 1, 35) 'insurance company name
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_04. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_04 = "" 'first name
		strNM1_05 = "" 'middle name
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_06. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_06 = "" 'name prefix (not used)
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_07. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_07 = "" 'name suffix
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_08. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1_08 = "PI" 'identification code qualifier
		
		If strPayerCode = "ZMIXED" Then
			If rst.Fields("fldInsuranceID").Value = INSCO_TEXAS_WORKERS_COMP Then
				'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_09. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				strNM1_09 = "TWCCP"
			Else
				'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_09. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				strNM1_09 = "FPRINT"
			End If
		Else
			strPayerCode = CreatePayerCodePrefix(rst.Fields("fldInsuranceID").Value, strPayerCode)
			'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_09. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strNM1_09 = strPayerCode 'identification code
		End If
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_09. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_08. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_07. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_06. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_04. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1 = "NM1*" & strNM1_01 & "*" & strNM1_02 & "*" & strNM1_03 & "*" & strNM1_04 & "*" & strNM1_05 & "*" & strNM1_06 & "*" & strNM1_07 & "*" & strNM1_08 & "*" & strNM1_09 & "~"
		
		'N2 (additional payer name information)
		'if the subscriber name is more than 35 characters then we need to use
		'segment N2 to hold the overflow characters
		If Len(strNM1_03) > 35 Then
			'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strNM1_03 = VB.Left(strNM1_03, 35)
			
			'N2 segment (Additional receiver name information)
			'this segment is used if the length of strNM1_03 is more than 35
			'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strN2 = "N2*" & VB.Right(strNM1_03, Len(strNM1_03) - 35) & "~"
		Else
			'UPGRADE_WARNING: Couldn't resolve default property of object strNM1_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strNM1_03 = strNM1_03
			strN2 = ""
		End If
		
		'add segment N2 if it exists
		If strN2 <> "" Then
			WriteLoop2010BB = True
		End If
		
		PrintLine(1, strNM1)
		
		If strN2 <> "" Then
			PrintLine(1, strN2)
		End If
		
		Exit Function
		
ErrTrap: 
		Err.Raise(Err.Number, Err.Source, Err.Description)
	End Function
	
	Private Sub WriteLoop2300(ByVal rst As ADODB.Recordset, ByVal strPayerCode As String)
		
		'Loop 2300 - Claim Level Information
		Dim strClaim_01, strClaim, strClaim_02 As Object
		Dim strClaim_03 As String
		Dim strClaim_04, strClaim_05 As Object
		Dim strClaim_06 As String
		Dim strClaim_07, strClaim_08 As Object
		Dim strClaim_09 As String
		Dim strClaim_10, strClaim_11 As Object
		Dim strClaim_12 As String
		Dim strClaim_05_1, strClaim_05_2 As Object
		Dim strClaim_05_3 As String
		
		On Error GoTo ErrTrap
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strClaim_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strClaim_01 = rst.Fields("fldPatientID").Value 'patient's account number
		'UPGRADE_WARNING: Couldn't resolve default property of object strClaim_02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strClaim_02 = rst.Fields("fldFee").Value 'total claim charge amount
		strClaim_03 = "" 'not used
		'UPGRADE_WARNING: Couldn't resolve default property of object strClaim_04. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strClaim_04 = "" 'not used
		
		'Medicaid specific: For electronic claims, we must undo the numbering sequence
		'that was coded in for paper-based claims
		Select Case rst.Fields("fldPOS").Value 'facility type code
			Case 1
				'UPGRADE_WARNING: Couldn't resolve default property of object strClaim_05_1. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				strClaim_05_1 = "11"
			Case 2
				'UPGRADE_WARNING: Couldn't resolve default property of object strClaim_05_1. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				strClaim_05_1 = "12"
			Case 3
				'UPGRADE_WARNING: Couldn't resolve default property of object strClaim_05_1. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				strClaim_05_1 = "21"
			Case Else
				'UPGRADE_WARNING: Couldn't resolve default property of object strClaim_05_1. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				strClaim_05_1 = rst.Fields("fldPOS").Value
		End Select
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strClaim_05_2. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strClaim_05_2 = "" 'not used
		strClaim_05_3 = "1" 'claim frequency code
		strClaim_06 = "Y" 'provider signature on file?
		'UPGRADE_WARNING: Couldn't resolve default property of object strClaim_07. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strClaim_07 = "A" 'provider accept assignment code
		'UPGRADE_WARNING: Couldn't resolve default property of object strClaim_08. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strClaim_08 = "Y" 'assignment of benefits indicator
		strClaim_09 = "Y" 'release of information code
		'UPGRADE_WARNING: Couldn't resolve default property of object strClaim_10. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strClaim_10 = "B" 'patient signature source code
		
		'TO DO:
		'Sub-elements CLM11-1 to 11-5 pertain to Auto Accident (AA), Other Accident (OA), Employment (EM), etc
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strClaim_10. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strClaim_08. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strClaim_07. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strClaim_05_2. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strClaim_05_1. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strClaim_04. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strClaim_02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strClaim_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strClaim. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strClaim = "CLM*" & strClaim_01 & "*" & strClaim_02 & "*" & strClaim_03 & "*" & strClaim_04 & "*" & strClaim_05_1 & ":" & strClaim_05_2 & ":" & strClaim_05_3 & "*" & strClaim_06 & "*" & strClaim_07 & "*" & strClaim_08 & "*" & strClaim_09 & "*" & strClaim_10 & "~"
		
		PrintLine(1, strClaim)
		
		Exit Sub
		
ErrTrap: 
		Err.Raise(Err.Number, Err.Source, Err.Description)
		
	End Sub
	
	Private Sub WriteLoopSV1(ByVal rst As ADODB.Recordset)
		
		'SV1 segment (professional service)
		Dim strSV1_01, strSV1, strSV1_02 As Object
		Dim strSV1_03 As String
		Dim strSV1_04, strSV1_05 As Object
		Dim strSV1_06 As String
		Dim strSV1_07, strSV1_08 As Object
		Dim strSV1_09 As String
		
		On Error GoTo ErrTrap
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strSV1_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strSV1_01 = "HC" 'product or service qualifer
		'UPGRADE_WARNING: Couldn't resolve default property of object strSV1_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strSV1_01 = strSV1_01 & ":" & rst.Fields("fldCPTCode").Value 'product/service ID
		'UPGRADE_WARNING: Couldn't resolve default property of object strSV1_02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strSV1_02 = rst.Fields("fldFee").Value 'monetary amount
		strSV1_03 = "UN" 'unit or basis for measurement code
		'UPGRADE_WARNING: Couldn't resolve default property of object strSV1_04. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strSV1_04 = "1" 'quantity (units or miniutes)
		
		'Medicaid specific: For electronic claims, we must undo the numbering sequence
		'that was coded in for paper-based claims
		Select Case rst.Fields("fldPOS").Value 'facility type code
			Case 1
				'UPGRADE_WARNING: Couldn't resolve default property of object strSV1_05. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				strSV1_05 = "11"
			Case 2
				'UPGRADE_WARNING: Couldn't resolve default property of object strSV1_05. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				strSV1_05 = "12"
			Case 3
				'UPGRADE_WARNING: Couldn't resolve default property of object strSV1_05. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				strSV1_05 = "21"
			Case Else
				'UPGRADE_WARNING: Couldn't resolve default property of object strSV1_05. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				strSV1_05 = rst.Fields("fldPOS").Value
		End Select
		
		strSV1_06 = "1"
		'UPGRADE_WARNING: Couldn't resolve default property of object strSV1_07. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strSV1_07 = ""
		'UPGRADE_WARNING: Couldn't resolve default property of object strSV1_08. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strSV1_08 = ""
		strSV1_09 = "N" 'Emergency indicatior (Y/N)
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strSV1_08. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strSV1_07. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strSV1_05. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strSV1_04. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strSV1_02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strSV1_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strSV1. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strSV1 = "SV1*" & strSV1_01 & "*" & strSV1_02 & "*" & strSV1_03 & "*" & strSV1_04 & "*" & strSV1_05 & "*" & strSV1_06 & "*" & strSV1_07 & "**" & strSV1_08 & strSV1_09 & "~"
		
		PrintLine(1, strSV1)
		
		Exit Sub
		
ErrTrap: 
		Err.Raise(Err.Number, Err.Source, Err.Description)
		
	End Sub
	
	Private Sub WriteLoopDTP(ByVal rst As ADODB.Recordset)
		
		'DTP segment (date - service date)
		Dim strDTP_01, strDTP, strDTP_02 As Object
		Dim strDTP_03 As String
		
		On Error GoTo ErrTrap
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strDTP_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strDTP_01 = "472" 'date/time qualifier
		'UPGRADE_WARNING: Couldn't resolve default property of object strDTP_02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strDTP_02 = "D8" 'date time period format qualifier
		strDTP_03 = VB6.Format(rst.Fields("fldDOS").Value, "yyyymmdd") 'date/time period in CCYYMMDD
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strDTP_02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strDTP_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strDTP. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strDTP = "DTP*" & strDTP_01 & "*" & strDTP_02 & "*" & strDTP_03 & "~"
		
		PrintLine(1, strDTP)
		
		Exit Sub
		
ErrTrap: 
		Err.Raise(Err.Number, Err.Source, Err.Description)
		
	End Sub
	
	Private Sub WriteTxSetTrailer(ByVal lngNumSeg As Integer, ByVal rst As ADODB.Recordset)
		
		'SE segment
		Dim strSE As String
		
		On Error GoTo ErrTrap
		
		'strSE = "SE*" & lngNumSeg & "*" & Mid(g_FileNumber, 1, 6) & Format(lngNumSect, "000") & "~"
		strSE = "SE*" & lngNumSeg & "*" & rst.Fields("fldEncounterLogID").Value & "~"
		
		PrintLine(1, strSE)
		
		Exit Sub
		
ErrTrap: 
		Err.Raise(Err.Number, Err.Source, Err.Description)
		
	End Sub
	
	Function CreatePayerCodePrefix(ByVal lngInsuranceCode As Integer, ByVal strPayerCode As String) As String
		
		On Error GoTo ErrTrap
		
		Select Case lngInsuranceCode
			Case INSCO_MEDICARE
				strPayerCode = "C" & strPayerCode
			Case INSCO_MEDICARE_PARTB
				strPayerCode = "C" & strPayerCode
			Case INSCO_MEDICAID
				strPayerCode = "D" & strPayerCode
			Case INSCO_BCBS
				strPayerCode = "G" & strPayerCode
			Case INSCO_BCBS_TX
				strPayerCode = "G" & strPayerCode
			Case INSCO_CHAMPUS
				strPayerCode = "C" & strPayerCode
				
			Case Else 'Commercial Claims
				strPayerCode = "F" & strPayerCode
		End Select
		
		CreatePayerCodePrefix = strPayerCode
		
ErrTrap: 
		'    Err.Raise Err.Number, Err.Source, Err.Description
		
	End Function
	
	Private Function StripDelimiters(ByVal strData As String) As String
		
		Dim strTemp As String
		Dim intPos As Short
		
		strTemp = strData 'Make a copy of the data
		
		intPos = InStr(1, strData, ":", CompareMethod.Text)
		If intPos > 0 Then
			strTemp = Mid(strData, 1, intPos - 1) & Mid(strData, intPos + 1)
		End If
		
		intPos = InStr(1, strTemp, "*", CompareMethod.Text)
		If intPos > 0 Then
			strTemp = Mid(strTemp, 1, intPos - 1) & Mid(strTemp, intPos + 1)
		End If
		
		intPos = InStr(1, strTemp, "~", CompareMethod.Text)
		If intPos > 0 Then
			strTemp = Mid(strTemp, 1, intPos - 1) & Mid(strTemp, intPos + 1)
		End If
		
		intPos = InStr(1, strTemp, "AMYSIS#", CompareMethod.Text)
		If intPos > 0 Then
			strTemp = Mid(strTemp, 1, intPos - 1) & Mid(strTemp, intPos + 7)
		End If
		
		StripDelimiters = strTemp
		
	End Function
	
	Private Sub WriteLoop2000A()
		
		Dim strHL_02, strHL, strHL_01, strHL_03 As Object
		Dim strHL_04 As String
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strHL_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strHL_01 = "1" 'hierarchical ID number (see pg. 77 of X098.pdf)
		'UPGRADE_WARNING: Couldn't resolve default property of object strHL_02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strHL_02 = "" 'hierarchical parent ID
		'UPGRADE_WARNING: Couldn't resolve default property of object strHL_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strHL_03 = "20" 'hierarchical level code(20 = information source)
		strHL_04 = "1" 'hierarchical child code
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strHL_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strHL_02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strHL_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strHL. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strHL = "HL*" & strHL_01 & "*" & strHL_02 & "*" & strHL_03 & "*" & strHL_04 & "~"
		PrintLine(1, strHL)
		
	End Sub
	
	Private Sub WriteLoop2000B(ByVal rst As ADODB.Recordset)
		
		Dim strHL_02, strHL, strHL_01, strHL_03 As Object
		Dim strHL_04 As String
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strHL_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strHL_01 = "2" 'hierarchical ID number (see p. 108 of X098.pdf)
		'UPGRADE_WARNING: Couldn't resolve default property of object strHL_02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strHL_02 = "1" 'hierarchical parent ID
		'UPGRADE_WARNING: Couldn't resolve default property of object strHL_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strHL_03 = "22" 'hierarchical level code (22 = subscriber)
		
		If rst.Fields("fldPatientID").Value <> rst.Fields("fldRPID").Value Then
			strHL_04 = "1" 'we have subordinate in hierarchical child code
		Else
			strHL_04 = "0" 'hierarchical child code
		End If
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strHL_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strHL_02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strHL_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strHL. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strHL = "HL*" & strHL_01 & "*" & strHL_02 & "*" & strHL_03 & "*" & strHL_04 & "~"
		PrintLine(1, strHL)
		
	End Sub
	
	Private Sub WriteLoop2000C(ByVal rst As ADODB.Recordset)
		
		Dim strHL_02, strHL, strHL_01, strHL_03 As Object
		Dim strHL_04 As String
		Dim strPat As String
		Dim strNM1 As String
		Dim strN3 As Object
		Dim strN4 As String
		Dim strDMG As String
		Dim dtDOB As Date
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strHL_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strHL_01 = "3" 'hierarchical ID number (see p. 152 of X098.pdf)
		'UPGRADE_WARNING: Couldn't resolve default property of object strHL_02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strHL_02 = "2" 'hierarchical parent ID
		'UPGRADE_WARNING: Couldn't resolve default property of object strHL_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strHL_03 = "23" 'hierarchical level code (23 = dependant)
		strHL_04 = "0" 'hierarchical child code
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strHL_03. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strHL_02. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strHL_01. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strHL. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strHL = "HL*" & strHL_01 & "*" & strHL_02 & "*" & strHL_03 & "*" & strHL_04 & "~"
		PrintLine(1, strHL)
		
		strPat = "PAT*"
		'UPGRADE_WARNING: Couldn't resolve default property of object GetRelationCode(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strPat = strPat & GetRelationCode(IfNull(rst.Fields("fldPatientRelation").Value, "")) & "~"
		PrintLine(1, strPat)
		
		strNM1 = "NM1*QC*1*" & rst.Fields("fldPatientLastName").Value & "*"
		strNM1 = strNM1 & rst.Fields("fldPatientFirstName").Value & "*"
		strNM1 = strNM1 & IfNull(rst.Fields("fldPatientMI").Value, "") & "***"
		strNM1 = strNM1 & "MI*" & rst.Fields("fldPatientID").Value & "~"
		PrintLine(1, strNM1)
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strN3. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strN3 = "N3*" & StripDelimiters(rst.Fields("fldPatientStreetNum").Value) & "~"
		PrintLine(1, strN3)
		
		strN4 = "N4*" & rst.Fields("fldPatientCity").Value & "*" & rst.Fields("fldPatientState").Value & "*" & rst.Fields("fldPatientZip").Value & "~"
		PrintLine(1, strN4)
		
		'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
		If IsDbNull(rst.Fields("fldPatientDOB").Value) Then
			dtDOB = CDate("01/01/1970") 'TO DO - FOR TESTING
		Else
			dtDOB = rst.Fields("fldPatientDOB").Value
		End If
		
		strDMG = "DMG*D8*" & Year(dtDOB) & VB6.Format(Month(dtDOB), "00") & VB6.Format(VB.Day(dtDOB), "00") & "*"
		strDMG = strDMG & IfNull(rst.Fields("fldPatientSex").Value, "U") & "~"
		PrintLine(1, strDMG)
		
	End Sub
	
	
	Private Function GetClaimFilingCode(ByVal lngInsuranceID As Integer) As Object
		
		Dim strFilingCode As String
		
		Select Case lngInsuranceID
			Case INSCO_TEXAS_WORKERS_COMP
				strFilingCode = "WC"
			Case INSCO_MEDICARE, INSCO_MEDICARE_PARTB
				strFilingCode = "MB"
			Case INSCO_MEDICAID
				strFilingCode = "MC"
			Case INSCO_BCBS, INSCO_BCBS_TX
				strFilingCode = "BL"
			Case INSCO_CHAMPUS
				strFilingCode = "CH"
			Case Else
				strFilingCode = "CI" 'Commercial Insurance
		End Select
		
		'UPGRADE_WARNING: Couldn't resolve default property of object GetClaimFilingCode. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		GetClaimFilingCode = strFilingCode
		
	End Function
	
	
	Private Function WriteInsuredInfo(ByVal rst As ADODB.Recordset) As Object
		
		Dim strNM1 As Object
		Dim dtDOB As Date
		
		On Error GoTo ErrTrap
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1 = "N3*" & StripDelimiters(rst.Fields("fldPatientStreetNum").Value) & "~"
		PrintLine(1, strNM1)
		
		'UPGRADE_WARNING: Couldn't resolve default property of object NumbersOnly(rst.Fields(fldPatientZip).Value). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1 = "N4*" & rst.Fields("fldPatientCity").Value & "*" & rst.Fields("fldPatientState").Value & "*" & NumbersOnly((rst.Fields("fldPatientZip").Value)) & "~"
		PrintLine(1, strNM1)
		
		'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="2EED02CB-5C0E-4DC1-AE94-4FAA3A30F51A"'
		If IsDbNull(rst.Fields("fldPatientDOB").Value) Then
			dtDOB = CDate("01/01/1970") 'TO DO - FOR TESTING
		Else
			dtDOB = rst.Fields("fldPatientDOB").Value
		End If
		
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1 = "DMG*D8*" & Year(dtDOB) & VB6.Format(Month(dtDOB), "00") & VB6.Format(VB.Day(dtDOB), "00") & "*"
		'UPGRADE_WARNING: Couldn't resolve default property of object strNM1. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1 = strNM1 & IfNull(rst.Fields("fldPatientSex").Value, "U") & "~"
		PrintLine(1, strNM1)
		
		Exit Function
		
ErrTrap: 
		Err.Raise(Err.Number, Err.Source, Err.Description)
		
	End Function
	
	
	Private Function WritePayerAddress(ByVal rst As ADODB.Recordset) As Object
		
		Dim strNM1 As String
		
		On Error GoTo ErrTrap
		
		strNM1 = "N3*" & StripDelimiters(rst.Fields("fldInsAddress1").Value) & "~"
		PrintLine(1, strNM1)
		
		'UPGRADE_WARNING: Couldn't resolve default property of object NumbersOnly(rst.Fields(fldInsZip).Value). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		strNM1 = "N4*" & rst.Fields("fldInsCity").Value & "*" & rst.Fields("fldInsState").Value & "*" & NumbersOnly((rst.Fields("fldInsZip").Value)) & "~"
		PrintLine(1, strNM1)
		
		Exit Function
		
ErrTrap: 
		Err.Raise(Err.Number, Err.Source, Err.Description)
		
	End Function
	
	
	Private Function GetRelationCode(ByVal strPatRelat As String) As Object
		
		On Error GoTo ErrTrap
		
		Select Case strPatRelat
			Case "Spouse"
				'UPGRADE_WARNING: Couldn't resolve default property of object GetRelationCode. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				GetRelationCode = "01"
			Case "Child"
				'UPGRADE_WARNING: Couldn't resolve default property of object GetRelationCode. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				GetRelationCode = "19"
			Case Else
				'UPGRADE_WARNING: Couldn't resolve default property of object GetRelationCode. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
				GetRelationCode = "21"
		End Select
		
		Exit Function
		
ErrTrap: 
		Err.Raise(Err.Number, Err.Source, Err.Description)
		
	End Function
End Class