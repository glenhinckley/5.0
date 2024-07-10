Option Strict Off
Option Explicit On
Friend Class frmBrowse
	Inherits System.Windows.Forms.Form
	'----------------------------------------------------------------------------------
	'Module Name: frmBrowse
	'Author: Dave Richkun
	'Date: 23-Nov-1998
	'Description: The frmBrowse is an extremely quick and dirty navigation utility
	'             that allows the user to select folders.  The Automated Build
	'             Utility expects the user to select the folder where source code
	'             files reside, and where they wish compiled files to be placed.  The
	'             Build Utility only needs the folder name, not the file name,
	'             thus the Common Dialog control is not the most appropriate tool
	'             for the job...but it works.
	'-----------------------------------------------------------------------------------
	'Revision History:
	'
	'-----------------------------------------------------------------------------------
	
	Private Sub cmdBrowse_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdBrowse.Click
		Dim Index As Short = cmdBrowse.GetIndex(eventSender)
		'-----------------------------------------------------------------------------------
		'Author: Dave Richkun
		'Date: 23-Nov-1998
		'Description: This event is fired each time one of the command buttons are clicked.
		'             Code here either hides or unloads the form.
		'Parameters:  Index - An integer that identifies which of the command buttons has
		'             been clicked.
		'Returns: Null
		'-----------------------------------------------------------------------------------
		'Revision History:
		'
		'-----------------------------------------------------------------------------------
		
		On Error Resume Next
		
		Select Case Index
			Case 0 'OK button
				Me.Hide()
			Case 1 'Cancel button
				Me.Close()
		End Select
		
	End Sub
	
	Private Sub drvList_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles drvList.SelectedIndexChanged
		'-----------------------------------------------------------------------------------
		'Author: Dave Richkun
		'Date: 23-Nov-1998
		'Description: This event is fired each time the Drive ComboBox changes.  Code here
		'             synchronizes the directory list with the selected drive.
		'Parameters:  None
		'Returns: Null
		'-----------------------------------------------------------------------------------
		'Revision History:
		'
		'-----------------------------------------------------------------------------------
		
		On Error Resume Next
		dirList.Path = drvList.Drive
		
	End Sub

    Private Sub _cmdBrowse_0_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _cmdBrowse_0.Click

    End Sub
End Class